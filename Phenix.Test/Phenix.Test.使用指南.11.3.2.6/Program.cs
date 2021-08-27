using System;
using System.Collections.Generic;

namespace Phenix.Test.使用指南._11._3._2._6
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

      const string test = "test.11.3.2.6";

      Console.WriteLine("**** 搭建测试环境 ****");
      UserList users = UserList.Fetch();
      Console.WriteLine("Fetch全景User集合对象，业务对象数：" + users.Count);
      Console.WriteLine();

      Console.WriteLine("**** 测试按条件表达式过滤出业务对象集合功能 ****");
      Console.WriteLine();

      UserList usersA = users.Filter(p => p.Usernumber == Phenix.Core.Security.UserIdentity.AdminUserNumber);
      Console.WriteLine(usersA.Count == 1 ? "从业务对象集合中Filter出AdminUser对象A：ID=" + usersA[0].US_ID + ",Name=" + usersA[0].Name + " ok" : "未能从业务对象集合Filter出AdminUser对象 error");
      UserList usersB = users.Filter(p => p.Usernumber == Phenix.Core.Security.UserIdentity.AdminUserNumber);
      Console.WriteLine(usersB.Count == 1 ? "从业务对象集合中Filter出AdminUser对象B：ID=" + usersB[0].US_ID + ",Name=" + usersB[0].Name + " ok" : "未能从业务对象集合Filter出AdminUser对象 error");
      usersA[0].Name = test;
      Console.WriteLine(usersA[0].Name != usersB[0].Name ? "对象A和对象B是不同的对象 ok" : "对象A和对象B指向了同一个内存 error");
      Console.WriteLine();

      List<User> userList = new List<User>(users);
      UserList usersC = UserList.Filter(userList, p => p.Usernumber == Phenix.Core.Security.UserIdentity.AdminUserNumber);
      Console.WriteLine(usersC.Count == 1 ? "从IEnumerable<TBusiness>中Filter出AdminUser对象C：ID=" + usersC[0].US_ID + ",Name=" + usersC[0].Name + " ok" : "未能从IEnumerable<TBusiness>中Filter出AdminUser对象 error");
      UserList usersD = UserList.Filter(userList, p => p.Usernumber == Phenix.Core.Security.UserIdentity.AdminUserNumber);
      Console.WriteLine(usersD.Count == 1 ? "从IEnumerable<TBusiness>中Filter出AdminUser对象D：ID=" + usersD[0].US_ID + ",Name=" + usersD[0].Name + " ok" : "未能从IEnumerable<TBusiness>中Filter出AdminUser对象 error");
      usersC[0].Name = test;
      Console.WriteLine(usersC[0].Name != usersD[0].Name ? "对象C和对象D是不同的对象 ok" : "对象C和对象D指向了同一个内存 error");
      Console.WriteLine();

      Console.WriteLine("结束, 与数据库交互细节见日志");
      Console.ReadLine();
    }
  }
}
