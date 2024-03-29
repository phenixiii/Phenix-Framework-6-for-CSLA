﻿using System;
using Phenix.Core;

namespace Phenix.Test.使用指南._11._3._2._1._4
{
  class Program
  {
    private static void Main(string[] args)
    {
      Console.WriteLine("本程序为{0}位，请确认所连数据库客户端引擎是否与之匹配？（如不匹配可调整本程序的'平台目标'生成类型）", Environment.Is64BitProcess ? "64" : "32");
      Console.WriteLine("请通过检索“//*”了解代码中的特殊处理");
      Console.WriteLine("本案例中表结构未设置中文友好名，可通过数据库字典相关的Comments内容来自动设置上");
      Console.WriteLine("测试过程中的日志保存在：" + Phenix.Core.AppConfig.TempDirectory);
      Console.WriteLine("因需要初始化本地配置数据，第一次运行会比正常情况下稍慢，请耐心等待");
      Console.WriteLine();

      Console.WriteLine("需事先在数据库中建视图（见PH_USERROLEINFO_V.sql）");
      Console.Write("准备好后请点回车继续：");
      Console.ReadLine();

      Console.WriteLine("设为调试状态");
      Phenix.Core.AppConfig.Debugging = true;
      Console.WriteLine();
      Console.WriteLine("模拟登陆");
      Phenix.Business.Security.UserPrincipal.User = Phenix.Business.Security.UserPrincipal.CreateTester();
      Phenix.Services.Client.Library.Registration.RegisterEmbeddedWorker(false);
      Console.WriteLine();

      Console.WriteLine("**** 测试业务类中包含有业务类字段的处理方法 ****");
      Console.WriteLine("Fetch...");
      UserRoleInfo userRoleInfo;
      while (true)
      {
        userRoleInfo = UserRoleInfo.Fetch();
        if (userRoleInfo != null)
          break;
        Console.WriteLine("请使用Phenix.Security.Windows.UserManage、Phenix.Security.Windows.RoleManage功能新增用户和角色信息");
        Console.WriteLine("如果仍然Fetch不到数据，请通过Phenix.Services.Host.x86.exe注册所有相关的业务程序集");
        Console.Write("注册好后请点回车继续：");
        Console.ReadLine();
      }
      Console.WriteLine("Fetch到UserRoleInfo对象：" + userRoleInfo.Caption);
      Console.WriteLine("取LinkUserDemoOk属性值：" + userRoleInfo.LinkUserDemoOk.Name);
      Console.WriteLine("取OwnerUser属性值：" + userRoleInfo.OwnerUser.Name);
      Console.WriteLine("编辑UserRoleInfo");
      userRoleInfo.BeginEdit();
      Console.WriteLine("UserRoleInfo的编辑状态：" + userRoleInfo.EditMode);
      Console.WriteLine("LinkUserDemoOk属性的编辑状态：" + userRoleInfo.LinkUserDemoOk.EditMode);
      Console.WriteLine("OwnerUser属性的编辑状态：" + userRoleInfo.OwnerUser.EditMode);
      Console.WriteLine("回滚UserRoleInfo");
      userRoleInfo.CancelEdit();
      Console.WriteLine("UserRoleInfo的编辑状态：" + userRoleInfo.EditMode);
      Console.WriteLine("LinkUserDemoOk属性的编辑状态：" + userRoleInfo.LinkUserDemoOk.EditMode);
      Console.WriteLine("OwnerUser属性的编辑状态：" + userRoleInfo.OwnerUser.EditMode);
      Console.WriteLine();
      Console.WriteLine("取LinkUserDemoError属性值：" + userRoleInfo.LinkUserDemoError.Name);
      try
      {
        Console.WriteLine("编辑UserRoleInfo...");
        userRoleInfo.BeginEdit();
        Console.WriteLine("ok");
        Console.WriteLine("回滚UserRoleInfo...");
        userRoleInfo.CancelEdit();
        Console.WriteLine("ok");
      }
      catch (Csla.Core.UndoException ex)
      {
        Console.WriteLine("拦截到Csla.Core.UndoException");
        Console.WriteLine(AppUtilities.GetErrorMessage(ex) + " ok");
      }
      catch (Exception ex)
      {
        Console.WriteLine(AppUtilities.GetErrorMessage(ex) + " error");
      }
      Console.WriteLine();

      Console.WriteLine("结束, 与数据库交互细节见日志");
      Console.ReadLine();
    }
  }
}
