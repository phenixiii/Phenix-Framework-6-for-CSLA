using System;
using System.Collections.Generic;

namespace Phenix.Test.使用指南._11._3._2._7
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

      Console.WriteLine("**** 搭建测试环境 ****");
      UserList users = UserList.Fetch();
      Console.WriteLine("Fetch全景User集合对象，业务对象数：" + users.Count);
      Console.WriteLine();

      Console.WriteLine("**** 测试排序功能 ****");
      Console.WriteLine();

      Console.WriteLine("按Name属性从业务对象集合中降序排列出业务对象集合");
      UserList usersA = users.OrderBy(Phenix.Core.Mapping.OrderByInfo.Descending(User.NameProperty));
      foreach (User item in usersA)
        Console.WriteLine(item.Name);
      Console.WriteLine();

      Console.WriteLine("按Name属性从IEnumerable<TBusiness>中升序排列出业务对象集合");
      List<User> userList = new List<User>(users);
      UserList usersB = UserList.OrderBy(userList, Phenix.Core.Mapping.OrderByInfo.Ascending(User.NameProperty));
      foreach (User item in usersB)
        Console.WriteLine(item.Name);
      Console.WriteLine();

      Console.WriteLine("结束, 与数据库交互细节见日志");
      Console.ReadLine();
    }
  }
}
