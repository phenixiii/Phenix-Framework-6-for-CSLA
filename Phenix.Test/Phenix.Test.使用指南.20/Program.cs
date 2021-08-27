using System;
using System.Threading;
using Phenix.Core;
using Phenix.Renovate.Contract;

namespace Phenix.Test.使用指南._20
{
  class Program
  {
    private static bool _connected;
    
    private static void Default_ConnectionChanged(object sender, Phenix.Renovate.Contract.ConnectionEventArgs e)
    {
      switch (e.EventType)
      {
        case ConnectionEventType.Connected:
          _connected = true;
          break;
        case ConnectionEventType.Disconnected:
          _connected = false;
          break;
      }
    }

    static void Main(string[] args)
    {
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

      Console.WriteLine("动态刷新功能需启动Phenix.Renovate.Server.x86.exe程序并连接到测试用的Oracle数据库");
      Console.Write("准备好后请点回车继续：");
      Console.ReadLine();

      Console.WriteLine("注册动态刷新事件");
      Phenix.Renovate.Client.Core.Subscriber.Default.ConnectionChanged += Default_ConnectionChanged;
      Console.WriteLine();

      Console.WriteLine("启动动态刷新");
      Phenix.Renovate.Client.Core.Subscriber.Default.Start();
      int i = 0;
      while (!_connected)
      {
        i = i + 1;
        Thread.Sleep(1000);
        Console.WriteLine(String.Format("连接中...(第{0}秒)", i));
      }
      Console.WriteLine("连接成功");
      Console.WriteLine();

      Console.WriteLine("注册动态刷新程序集");
      Phenix.Renovate.Client.Core.Subscriber.Default.Register<UserReadOnlyList>();
      Console.WriteLine("注册成功");
      Console.WriteLine();

      Console.WriteLine("**** 测试Fetch功能 ****");
      UserReadOnlyList renovatingUsers = Phenix.Renovate.Client.Core.Subscriber.Default.Fetch<UserReadOnlyList>();
      i = 0;
      while (!renovatingUsers.DataLoaded)
      {
        i = i + 1;
        Thread.Sleep(1000);
        Console.WriteLine(String.Format("等待刷新中...(第{0}秒)", i));
      }
      UserList users = UserList.Fetch();
      Console.WriteLine("与数据库里记录数{0}一致 {1}", users.Count, renovatingUsers.Count == users.Count ? "ok" : "error");
      Console.WriteLine();

      string test = "test.20" + new Random().Next(100, 999);

      Console.WriteLine("**** 测试新增业务对象后效果 ****");
      User user = User.New(test, test, false, test);
      users.Add(user);
      try
      {
        users.Save();
        Console.WriteLine("已提交新增User对象：" + user.Name);
        Console.WriteLine("请检查是否提示有刷新数据到本地...");
      }
      catch (Exception ex)
      {
        Console.WriteLine(AppUtilities.GetErrorMessage(ex));
      }

      Console.WriteLine("Sleep3秒, 等待AnalyseRenovateInfo执行完成...");
      for (i = 1; i <= 30; i++)
        Thread.Sleep(100);
      Console.WriteLine();

      Console.WriteLine("**** 测试更新业务对象后效果 ****");
      user.Locked = true;
      try
      {
        users.Save();
        Console.WriteLine("已提交更新User对象" + user.Name + "的Locked属性：" + user.Locked);
        Console.WriteLine("请检查是否提示有刷新数据到本地...");
      }
      catch (Exception ex)
      {
        Console.WriteLine(AppUtilities.GetErrorMessage(ex));
      }

      Console.WriteLine("Sleep3秒, 等待AnalyseRenovateInfo执行完成...");
      for (i = 1; i <= 30; i++)
        Thread.Sleep(100);
      Console.WriteLine();

      Console.WriteLine("**** 测试删除业务对象后效果 ****");
      user.Delete();
      try
      {
        users.Save();
        Console.WriteLine("已提交删除User对象：" + user.Name);
        Console.WriteLine("请检查是否提示有刷新数据到本地...");
      }
      catch (Exception ex)
      {
        Console.WriteLine(AppUtilities.GetErrorMessage(ex));
      }

      Console.WriteLine("Sleep3秒, 等待AnalyseRenovateInfo执行完成...");
      for (i = 1; i <= 30; i++)
        Thread.Sleep(100);
      Console.WriteLine();

      Console.WriteLine("注销动态刷新程序集");
      Phenix.Renovate.Client.Core.Subscriber.Default.Unregister<UserReadOnlyList>();

      Console.WriteLine("结束, 与数据库交互细节见日志");
      Console.ReadLine();
    }
  }
}
