using System;
using System.Threading;
using Phenix.Core;
using Phenix.Core.Security;
using Phenix.Services.Client.Security;
using Phenix.Test.使用指南._23._2.Business;

namespace Phenix.Test.使用指南._23._2
{
  class Program
  {
    private static SysDateService _service;

    static void Main(string[] args)
    {
      Console.WriteLine("需事先在本机启动Host服务（启动Bin.Top目录下的Phenix.Services.Host.x86/x64.exe程序）");
      Console.WriteLine("如需观察日志，请启动Host后将Debugging功能点亮，测试过程中产生的日志会保存在Host当前目录下的TempDirectory子目录里");
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

      Console.WriteLine("**** 测试同步调用服务功能 ****");
      _service = new SysDateService();
      _service.Execute();
      Console.WriteLine("执行SysDateService服务，返回: " + _service.ExecuteResult.Message);
      Console.WriteLine();

      Console.WriteLine("**** 测试异步调用服务功能 ****");
      Console.Write("希望进度反馈间隔是多少(秒)?");
      int askProgressInterval;
      if (!Int32.TryParse(Console.ReadLine(), out askProgressInterval))
        askProgressInterval = 2;
      _service = new SysDateService();
      _service.ExecuteAsync(DoProgressAsking, DoProgressAsked, askProgressInterval);
      Console.WriteLine("将会间隔{0}秒向服务端发一次询问", _service.AskProgressInterval);
      Console.WriteLine();

      while (!_service.ExecuteResult.Stop)
      {
        Thread.Sleep(askProgressInterval * 1000);
      }
      Console.ReadLine();
    }

    private static void DoProgressAsking(StopEventArgs args)
    {
      Console.Write("是否继续向服务端发起询问?(Y/N)");
      if (String.Compare(Console.ReadLine(), "Y", StringComparison.OrdinalIgnoreCase) != 0)
        args.Stop = true;
    }

    private static void DoProgressAsked(ShallEventArgs args)
    {
      Console.WriteLine("服务端返回消息对象: " + args.Message);
      if (args.Succeed)
        Console.WriteLine("结束");
    }
  }
}
