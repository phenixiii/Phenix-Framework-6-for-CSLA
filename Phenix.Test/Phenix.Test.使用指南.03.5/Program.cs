using System;
using System.Collections.Generic;
using Phenix.Business.Security;
using Phenix.Core;
using Phenix.Core.Mapping;

namespace Phenix.Test.使用指南._03._5
{
  class Program
  {
    static void Main(string[] args)
    {
      Console.WriteLine("本程序为{0}位，请确认所连数据库客户端引擎是否与之匹配？（如不匹配可调整本程序的'平台目标'生成类型）", Environment.Is64BitProcess ? "64" : "32");
      Console.WriteLine("请通过检索“//*”了解代码中的特殊处理");
      Console.WriteLine("本案例中表结构未设置中文友好名，可通过数据库字典相关的Comments内容来自动设置上");
      Console.WriteLine("测试过程中的日志保存在：" + Phenix.Core.AppConfig.TempDirectory);
      Console.WriteLine("因需要初始化本地配置数据，第一次运行会比正常情况下稍慢，请耐心等待");
      Console.WriteLine();
      Console.WriteLine("设为调试状态");
      Phenix.Core.AppConfig.Debugging = true;
      Console.WriteLine();
      Console.WriteLine("模拟登陆");
      UserPrincipal.User = Phenix.Business.Security.UserPrincipal.CreateTester();
      Phenix.Services.Client.Library.Registration.RegisterEmbeddedWorker(false);
      Console.WriteLine();

      Console.WriteLine("**** 测试Fetch()单个Entity功能 ****");
      UserEasy adminUser = UserEasy.Fetch(p => p.Usernumber == Phenix.Core.Security.UserIdentity.AdminUserNumber);
      Console.WriteLine("Fetch单个AdminUser对象：ID=" + adminUser.US_ID + ",Name=" + adminUser.Name + ",Locked=" + adminUser.Locked);
      Console.WriteLine();

      bool? oldLockedValue = adminUser.Locked;

      Console.WriteLine("**** 测试Save()单个Entity的功能 ****");
      Console.WriteLine("无需调用BeginEdit()，只要向某个属性（setter中调用了SetProperty()函数）赋值就会启动编辑状态");
      //adminUser.BeginEdit();
      adminUser.Locked = !oldLockedValue;
      Console.WriteLine("设置属性Locked=" + adminUser.Locked);
      Console.WriteLine("Locked是否脏属性: " + (adminUser.IsDirtyProperty(UserEasy.LockedProperty) ? "是" : "否") + ' ' + (adminUser.IsDirtyProperty(UserEasy.LockedProperty) ? "ok" : "error"));
      Console.WriteLine("Locked属性的旧值是: " + adminUser.GetOldValue(UserEasy.LockedProperty) + ' ' + 
        ((bool?)Phenix.Core.Reflection.Utilities.ChangeType(adminUser.GetOldValue(UserEasy.LockedProperty), typeof(bool?)) == oldLockedValue ? "ok" : "error"));
      try
      {
        if (adminUser.Save())
        {
          Console.WriteLine("提交AdminUser对象之后：");
          Console.WriteLine("Locked是否脏属性: " + (adminUser.IsDirtyProperty(UserEasy.LockedProperty) ? "是" : "否") + ' ' + (!adminUser.IsDirtyProperty(UserEasy.LockedProperty) ? "ok" : "error"));
          Console.WriteLine("Locked属性的旧值是: " + adminUser.GetOldValue(UserEasy.LockedProperty) + ' ' +
            ((bool?)Phenix.Core.Reflection.Utilities.ChangeType(adminUser.GetOldValue(UserEasy.LockedProperty), typeof(bool?)) != oldLockedValue ? "ok" : "error"));
        }
        else
          Console.WriteLine("提交AdminUser对象失败：error");
      }
      catch (Exception ex)
      {
        Console.WriteLine("提交AdminUser对象时抛出异常：" + AppUtilities.GetErrorMessage(ex) + " error");
      }
      Console.WriteLine();

      Console.WriteLine("**** 清理测试环境 ****");
      adminUser.Locked = oldLockedValue;
      try
      {
        adminUser.Save();
        Console.WriteLine("清理完成");
      }
      catch (Exception ex)
      {
        Console.WriteLine("清理时抛出异常：" + AppUtilities.GetErrorMessage(ex) + " error");
      }
      Console.WriteLine();

      Console.WriteLine("**** 测试新增Entity及Save()功能 ****");
      string[] tests = { "test3.5.A", "test3.5.B", "test3.5.C" };
      UserEasyList userEasyList = UserEasyList.New();
      foreach (string s in tests)
      {
        UserEasy userEasy = UserEasy.New();
        userEasy.Usernumber = s;
        userEasy.Name = s;
        userEasy.Password = s;
        userEasy.Locked = false;
        userEasyList.Add(userEasy);
        Console.WriteLine("添加到EntityList的新增UserEasy对象：ID=" + userEasy.US_ID + ",Name=" + userEasy.Name);
      }
      try
      {
        int succeedCount = userEasyList.Save();
        if (succeedCount > 0)
          Console.WriteLine("提交EntityList成功：ok");
        else
          Console.WriteLine("提交EntityList失败：error");
      }
      catch (Exception ex)
      {
        Console.WriteLine("提交EntityList时抛出异常：" + AppUtilities.GetErrorMessage(ex) + " error");
      }
      Console.WriteLine();

      Console.WriteLine("测试修改Entity及Save()功能...");
      foreach (UserEasy item in userEasyList)
      {
        item.Usernumber = item.Usernumber + "_";
        Console.WriteLine("更改UserEasy对象Usernumber属性：ID=" + item.US_ID + ",Name=" + item.Name + ",UserNumber=" + item.Usernumber);
      }
      try
      {
        int succeedCount = userEasyList.Save();
        if (succeedCount > 0)
          Console.WriteLine("提交EntityList成功：ok");
        else
          Console.WriteLine("提交EntityList失败：error");
      }
      catch (Exception ex)
      {
        Console.WriteLine("提交EntityList时抛出异常：" + AppUtilities.GetErrorMessage(ex) + " error");
      }
      Console.WriteLine();

      Console.WriteLine("**** 测试删除Entity及Save()功能 ****");
      foreach (UserEasy item in userEasyList)
      {
        item.Delete();
        Console.WriteLine("删除UserEasy对象：ID=" + item.US_ID + ",Name=" + item.Name + ",UserNumber=" + item.Usernumber + " " + (item.IsSelfDeleted ? "ok" : "error"));
      }
      try
      {
        int succeedCount = userEasyList.Save();
        if (succeedCount > 0)
          Console.WriteLine("提交EntityList成功：ok");
        else
          Console.WriteLine("提交EntityList失败：error");
      }
      catch (Exception ex)
      {
        Console.WriteLine("提交EntityList时抛出异常：" + AppUtilities.GetErrorMessage(ex) + " error");
      }
      Console.WriteLine();

      Console.WriteLine("结束, 与数据库交互细节见日志");
      Console.WriteLine("如需了解Entity的Fetch性能与业务类、DataSet之间的差异，请运行“Phenix.Test.使用指南.03.3”工程");
      Console.ReadLine();
    }
  }
}
