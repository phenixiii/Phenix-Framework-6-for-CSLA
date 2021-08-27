using System;

namespace Phenix.Test.使用指南._11._2._1
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
      UserList users = UserList.New();
      Console.WriteLine("New一个用户集合对象users");
      Console.WriteLine();

      Console.WriteLine("**** 测试业务对象NewPure功能 ****");
      User user = User.NewPure();
      users.Add(user);
      Console.WriteLine("NewPure用户对象user并添加到users里：ID=" + users[0].US_ID + ",Locked=" + users[0].Locked + ' ' + (!users[0].Locked.HasValue ? "ok" : "error"));
      Console.WriteLine("user的OnInitializeNew()函数是否被调用：" + user.OnInitializeNewByExecute + ' ' + (!user.OnInitializeNewByExecute ? "ok" : "error"));
      Console.WriteLine();

      Console.WriteLine("**** 测试业务对象New功能 ****");
      user = User.New();
      users.Add(user);
      Console.WriteLine("New用户对象user并添加到users里：ID=" + users[1].US_ID + ",Locked=" + users[1].Locked + ' ' + (users[1].Locked.HasValue ? "ok" : "error"));
      Console.WriteLine("user的OnInitializeNew()函数是否被调用：" + user.OnInitializeNewByExecute + ' ' + (user.OnInitializeNewByExecute ? "ok" : "error"));
      Console.WriteLine();

      Console.WriteLine("结束");
      Console.ReadLine();
    }
  }
}
