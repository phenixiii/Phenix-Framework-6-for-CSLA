using System;
using Phenix.Core;

namespace Phenix.Test.使用指南._12._6._2._12
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
      Phenix.Business.Security.UserPrincipal.User = Phenix.Business.Security.UserPrincipal.CreateTester();
      Phenix.Services.Client.Library.Registration.RegisterEmbeddedWorker(false);
      Console.WriteLine();

      Console.WriteLine("**** 测试在最后时刻限制保存功能 ****");
      User adminUser = User.Fetch(p => p.Usernumber == Phenix.Core.Security.UserIdentity.AdminUserNumber);
      Console.WriteLine("Fetch单个AdminUser对象：ID=" + adminUser.US_ID + ",Name=" + adminUser.Name + ",UserNumber=" + adminUser.Usernumber);
      adminUser.Locked = true;
      Console.WriteLine("设置属性Locked=" + adminUser.Locked);
      try
      {
        adminUser.Save();
        Console.WriteLine("保存AdminUser对象成功：error");
      }
      catch (Phenix.Business.CheckSaveException ex)
      {
        Console.WriteLine("拦截到Phenix.Business.CheckSaveException");
        Console.WriteLine("保存AdminUser对象时抛出异常：" + AppUtilities.GetErrorMessage(ex) + " ok");
      }
      catch (Exception ex)
      {
        Console.WriteLine("保存AdminUser对象时抛出异常：" + AppUtilities.GetErrorMessage(ex) + " error");
      }
      Console.WriteLine();

      Console.WriteLine("结束, 与数据库交互细节见日志");
      Console.ReadLine();
    }
  }
}
