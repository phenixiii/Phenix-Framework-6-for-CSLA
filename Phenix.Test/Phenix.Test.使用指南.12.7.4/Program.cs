using System;

namespace Phenix.Test.使用指南._12._7._4
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

      const string regexColumnName = "^*_LOCKED$";

      Console.WriteLine("**** 测试通过UserPrincipal.User.Identity.SetFieldDefaultValue()函数注册业务数据的缺省值功能 ****");
      Console.WriteLine("默认下业务对象字段的缺省值对应到数据库表字段的缺省值，比如User表结构中US_Locked字段定义为default 0");
      User user = User.New();
      Console.WriteLine("New用户对象user：Locked=" + user.Locked + ' ' + (user.Locked.HasValue && !user.Locked.Value ? "ok" : "error"));
      Phenix.Business.Security.UserPrincipal.User.Identity.SetFieldDefaultValue(regexColumnName, true);
      Console.WriteLine("当SetFieldDefaultValue(\"" + regexColumnName + "\", " + Phenix.Business.Security.UserPrincipal.User.Identity.GetFieldDefaultValue(regexColumnName) + ")后");
      user = User.New();
      Console.WriteLine("New用户对象user：Locked=" + user.Locked + ' ' + (user.Locked.HasValue && user.Locked.Value ? "ok" : "error"));
      Phenix.Business.Security.UserPrincipal.User.Identity.RemoveFieldDefaultValue(regexColumnName);
      Console.WriteLine("当RemoveFieldDefaultValue(\"" + regexColumnName + "\")后");
      user = User.New();
      Console.WriteLine("New用户对象user：Locked=" + user.Locked + ' ' + (user.Locked.HasValue && !user.Locked.Value ? "ok" : "error"));
      Console.WriteLine();

      Console.WriteLine("结束");
      Console.ReadLine();
    }
  }
}
