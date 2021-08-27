using System;
using System.Threading;
using Phenix.Core.Security;
using Phenix.Services.Client.Security;

namespace Phenix.StandardPush.Test
{
  class Program
  {
    static void Main(string[] args)
    {
      Console.WriteLine("需事先在本机启动Host服务（启动Bin目录下的Phenix.Services.Host.x86/x64.exe程序，并注册过Phenix.StandardPush.Plugin.dll）");
      Console.Write("准备好后请点回车继续：");
      Console.ReadLine();

      string password = "ADMIN";
      while (true)
        try
        {
          DataSecurityContext context;
          if (LogOnHelper.TryLogOn("127.0.0.1", "ADMIN", password, out context))
            break;
          Console.WriteLine("登录未成功：" + context.Message);
          Console.Write("请重新登录，输入ADMIN的口令：");
          password = Console.ReadLine();
        }
        catch (Exception ex)
        {
          Console.WriteLine(Phenix.Core.AppUtilities.GetErrorHint(ex));
          Console.Write("请调整好后点回车继续：");
          Console.ReadLine();
        }
      Console.WriteLine("{0}登录{1}服务成功：", UserIdentity.CurrentIdentity.UserName, Phenix.Core.Net.NetConfig.ServicesAddress);
      Console.WriteLine();

      Console.WriteLine("测试与数据库时钟保持同步功能...");
      using (Phenix.StandardPush.Business.SynchronizeClocker clocker = Phenix.StandardPush.Business.SynchronizeClockCommand.Execute())
      {
        for (int i = 0; i < 100; i++)
        {
          Console.WriteLine("clocker.Value = {0}", clocker.Value);
          Thread.Sleep(3000);
        }
      }
      //Console.Write("是否继续?(Y/N)");
      //if (String.Compare(Console.ReadLine(), "Y", StringComparison.OrdinalIgnoreCase) != 0)
      //  break;
      Console.WriteLine("结束");
    }
  }
}
