using System;
using Phenix.Business.Tunnel;
using Phenix.Core;

namespace Phenix.Test.使用案例.业务数据的读写.快速Save
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

      string test = "t" + Phenix.Core.Data.Sequence.Value.ToString().Remove(9);

      Console.WriteLine("**** 测试业务数据的读写-快速Save功能 ****");
      Console.WriteLine();

      UserList users = new UserList();
      User user = User.New(test, test, null, test, null, null, null, 0, null, null, null, 0);
      users.Add(user);
      Console.WriteLine("新增User对象: " + (users.Count == 1 ? users[0].Name + " ok" : "error"));
      Role rule = Role.New(test, test);
      Console.WriteLine("新增Role对象: " + (rule != null ? rule.Name + " ok" : "error"));
      try
      {
        FastSaveCommand command = new FastSaveCommand();
        command.Businesses.Add(users);
        command.Businesses.Add(rule);
        command.Execute();
        Console.WriteLine("提交 ok");
      }
      catch (Exception ex)
      {
        Console.WriteLine("提交 error: " + AppUtilities.GetErrorHint(ex));
      }
      Console.WriteLine();

      Console.WriteLine("**** 恢复测试环境 ****");
      int i = UserEasyList.DeleteRecord(User.US_IDProperty == user.US_ID);
      Console.WriteLine("删除User对象 " + (i == 1 ? "ok" : "error"));
      try
      {
        FastSaveCommand command = new FastSaveCommand();
        rule.Delete();
        command.Businesses.Add(rule);
        command.Execute();
        Console.WriteLine("删除Role对象 ok");
      }
      catch (Exception ex)
      {
        Console.WriteLine("删除Role对象 error: " + AppUtilities.GetErrorHint(ex));
      }
      Console.WriteLine();
        
      Console.WriteLine("结束, 与数据库交互细节见日志");
      Console.ReadLine();
    }
  }
}
