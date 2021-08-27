using System;
using Phenix.Core;
using Phenix.Core.Security;
using Phenix.Services.Client.Security;

namespace Phenix.Test.使用指南._04._1._5
{
  class Program
  {
    static void Main(string[] args)
    {
      Console.WriteLine("测试从指定的服务Host下载文件替换和升级客户端");
      Console.WriteLine("需事先启动Host服务（测试在本机进行，请启动Bin.Top目录下的Phenix.Services.Host.x86/x64.exe程序）");
      Console.WriteLine("然后在Host的'ClientLibrary'子目录下随意添加些以dll、xls为后缀的文件，观察是否能被下载到本程序所在目录下");
      Console.WriteLine();
      Console.WriteLine("为区别于下载服务，登录界面上除了正常输入工号/口令（默认为ADMIN/ADMIN）外，请在'服务'输入框里置为二层的内嵌模式（embedded）");
      Console.Write("准备好后请点回车继续：");
      Console.ReadLine();

      while (true)
      {
        using (LogOn logOn = new LogOn())
        {
          logOn.Title = "登录XXX系统"; //title可更换
          //logOn.Logo = System.Drawing.Image.FromFile("Phenix.logo.jpg"); //logo可更换
          //logOn.UpgradeProxyType = Phenix.Core.Net.NetConfig.ProxyType; //升级文件的代理类型
          logOn.UpgradeServicesAddress = "127.0.0.1"; //升级文件的服务地址
          logOn.UpgradeFileFilters.Add("*.dll");
          logOn.UpgradeFileFilters.Add("*.xls");

          IPrincipal user = logOn.Execute<LogOnDialog>();
          if (user != null && user.Identity.IsAuthenticated)
          {
            Console.WriteLine("成功登录到：" + Phenix.Core.Net.NetConfig.ServicesAddress);
            Console.WriteLine("下载服务为：" + logOn.UpgradeServicesAddress);
            Console.WriteLine("请观察是否有文件被下载到'" + Phenix.Core.AppConfig.BaseDirectory + "'目录下？");
            Console.WriteLine();
            break;
          }
        }
      }

      Console.WriteLine("为验证是否由'" + Phenix.Core.Net.NetConfig.ServicesAddress + "'继续提供服务，现在请将Host服务关闭");
      Console.Write("准备好后请点回车继续：");
      Console.ReadLine();
      try
      {
        UserList users = UserList.Fetch();
        Console.WriteLine("调用服务成功，Fetch到User业务对象数：" + users.Count + " ok");
      }
      catch (Exception ex)
      {
        Console.WriteLine("调用服务抛出异常：" + AppUtilities.GetErrorMessage(ex) + " error");
      }
      Console.WriteLine();

      Console.WriteLine("结束, 与数据库交互细节见日志");
      Console.ReadLine();
    }
  }
}
