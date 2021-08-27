using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;

namespace Phenix.Test.使用指南._21._5
{
  class Program
  {
    [STAThread]
    static void Main(string[] args)
    {
      Console.WriteLine("请启动Bin.Top目录下的Phenix.Services.Host.x86/x64.exe程序");
      Console.WriteLine("如需观察日志（被保存在TempDirectory子目录里）可将Host的Debugging功能菜单点亮");
      Console.Write("准备好后请点回车继续：");
      Console.ReadLine();


      Phenix.Web.Client.Security.UserIdentity userIdentity = new Phenix.Web.Client.Security.UserIdentity("ADMIN", "ADMIN");
      using (Phenix.Web.Client.HttpClient client = new Phenix.Web.Client.HttpClient("127.0.0.1", 8080, userIdentity))
      {
        while (true)
        {
          using (HttpResponseMessage message = client.SecurityProxy.TryLogOnAsync().Result)
          {
            if (message.StatusCode == HttpStatusCode.OK)
            {
              Console.WriteLine("登录成功");
              break;
            }
            Console.WriteLine("登录未成功：" + message.Content.ReadAsStringAsync().Result);
            Console.Write("请重新登录，输入ADMIN的口令：");
          }
          userIdentity.Password = Console.ReadLine();
        }
        Console.WriteLine();

        Console.WriteLine("****************************");
        Console.WriteLine("**** 开始测试拉取消息功能 ****");
        Console.WriteLine();

        try
        {
          client.MessageProxy.Send("ADMIN", "Hello pull!");
          Console.WriteLine("发送消息成功!");
          try
          {
            IDictionary<long, string> messages = client.MessageProxy.Receive();
            foreach (KeyValuePair<long, string> kvp in messages)
            {
              Console.WriteLine("收取消息成功! id: " + kvp.Key + ", content：" + kvp.Value);
              try
              {
                client.MessageProxy.AffirmReceived(kvp.Key);
                Console.WriteLine("确认收到成功! id: " + kvp.Key);
                Console.WriteLine();
              }
              catch (Exception ex)
              {
                Console.WriteLine("调用AffirmReceived失败! error: " + Phenix.Core.AppUtilities.GetErrorMessage(ex));
                Console.ReadLine();
              }
            }
          }
          catch (Exception ex)
          {
            Console.WriteLine("调用Receive失败! error: " + Phenix.Core.AppUtilities.GetErrorMessage(ex));
            Console.ReadLine();
          }
        }
        catch (Exception ex)
        {
          Console.WriteLine("调用Send失败! error: " + Phenix.Core.AppUtilities.GetErrorMessage(ex));
          Console.ReadLine();
        }

        Console.WriteLine("****************************");
        Console.WriteLine("**** 开始测试推送消息功能 ****");
        Console.WriteLine();

        try
        {
          client.MessageProxy.Subscribe((proxy, messages) =>
            {
              foreach (KeyValuePair<long, string> kvp in messages)
              {
                Console.WriteLine("收取消息成功! id: " + kvp.Key + ", content：" + kvp.Value);
                try
                {
                  proxy.AffirmReceived(kvp.Key);
                  Console.WriteLine("确认收到成功! id: " + kvp.Key);
                  Console.WriteLine();
                }
                catch (Exception ex)
                {
                  Console.WriteLine("调用AffirmReceived失败! error: " + Phenix.Core.AppUtilities.GetErrorMessage(ex));
                  Console.ReadLine();
                }
              }
            },
            (proxy, text) =>
            {
              Console.WriteLine(text);
              Console.ReadLine();
            });
        }
        catch (Exception ex)
        {
          Console.WriteLine("调用Subscribe失败! error: " + Phenix.Core.AppUtilities.GetErrorMessage(ex));
          Console.ReadLine();
        }
        try
        {
          client.MessageProxy.Send("ADMIN", "Hello push!");
          Console.WriteLine("发送消息成功!");
        }
        catch (Exception ex)
        {
          Console.WriteLine("调用Send失败! error: " + Phenix.Core.AppUtilities.GetErrorMessage(ex));
          Console.ReadLine();
        }

        Console.WriteLine();
      }

      Console.WriteLine("结束, 与数据库交互细节见日志");
      Console.ReadLine();
    }
  }
}
