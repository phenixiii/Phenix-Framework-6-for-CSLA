using System;
using System.Collections.Generic;
using Phenix.Business;
using Phenix.Business.Tunnel;
using Phenix.Core;

namespace Phenix.Test.使用案例.业务数据的读写.快速Fetch
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

      Console.WriteLine("**** 测试业务数据的读写-快速Fetch功能 ****");
      Console.WriteLine();

      try
      {
        FastFetchCommand command = new FastFetchCommand();
        command.AddFetch<User>(p => p.Usernumber == Phenix.Core.Security.UserIdentity.AdminUserNumber);
        command.AddFetch<UserList, User>(p => p.Usernumber.Contains(Phenix.Core.Security.UserIdentity.AdminUserNumber));
        foreach (KeyValuePair<Criterions, IBusiness> keyValuePair in command.Execute())
        {
          User user = keyValuePair.Value as User;
          if (user != null)
          {
            Console.WriteLine("Fetch单个对象：ID=" + user.US_ID + ",Name=" + user.Name + ",UserNumber=" + user.Usernumber);
            continue;
          }
          UserList users = keyValuePair.Value as UserList;
          if (users != null)
          {
            foreach (User item in users)
              Console.WriteLine("Fetch集合对象中：ID=" + item.US_ID + ",Name=" + item.Name + ",UserNumber=" + item.Usernumber);
            continue;
          }
        }
        Console.WriteLine("ok");
      }
      catch (Exception ex)
      {
        Console.WriteLine("error: " + AppUtilities.GetErrorHint(ex));
      }

      Console.WriteLine();
        
      Console.WriteLine("结束, 与数据库交互细节见日志");
      Console.ReadLine();
    }
  }
}
