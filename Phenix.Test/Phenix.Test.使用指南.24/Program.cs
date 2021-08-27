using System;

namespace Phenix.Test.使用指南._24
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

      Console.WriteLine("**** 测试归档功能 ****");
      UserList users = UserList.Fetch();
      Console.WriteLine("UserList 共 Fetch 对象: " + users.Count);
      users.Archive();
      Console.WriteLine("UserList 归档完成");
      Console.WriteLine();

      Console.WriteLine("**** 测试查询功能 ****");
      UserHistoryList userHistoryList = UserHistoryList.Fetch();
      Console.WriteLine("UserHistoryList 共 Fetch 对象: " + userHistoryList.Count + ' ' + (userHistoryList.Count == users.Count ? "ok" : "error"));
      Console.WriteLine();
      UserHistoryAllList userHistoryAllList = UserHistoryAllList.Fetch();
      Console.WriteLine("UserHistoryAllList 共 Fetch 对象: " + userHistoryAllList.Count + ' ' + (userHistoryAllList.Count == users.Count * 2 ? "ok" : "error"));
      Console.WriteLine();

      Console.WriteLine("结束, 与数据库交互细节见日志");
      Console.ReadLine();
    }
  }
}
