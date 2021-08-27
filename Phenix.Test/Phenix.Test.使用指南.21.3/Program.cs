using System;

namespace Phenix.Test.使用指南._21._3
{
  class Program
  {
    static void Main(string[] args)
    {
      Console.WriteLine("需事先在本机启动WebAPI服务（启动Bin.Top目录下的Phenix.Services.Host.x86/x64.exe程序）");
      Console.WriteLine("如需观察日志，请启动Host后将Debugging功能点亮，测试过程中产生的日志会保存在Host当前目录下的TempDirectory子目录里");
      Console.Write("准备好后请点回车继续：");
      Console.ReadLine();

      DataSecurityTest();

      Console.ReadLine();
    }

    private static async void DataSecurityTest()
    {
      Console.WriteLine("**** 测试身份认证 ****");
      Phenix.Web.Client.Security.UserIdentity userIdentity = new Phenix.Web.Client.Security.UserIdentity("ADMIN", "ADMIN");
      using (Phenix.Web.Client.HttpClient client = new Phenix.Web.Client.HttpClient("127.0.0.1", userIdentity))
      {
        Console.WriteLine("登录ADMIN用户（第二个参数为ADMIN的默认口令，如有被修改过请赋值为当前口令）");
        bool succeed = await client.SecurityProxy.LogOnAsync();
        Console.WriteLine("是否已登录成功：" + (succeed ? "是 ok" : "否 error"));
        Console.WriteLine();

        Console.WriteLine("修改ADMIN登录口令（第二个参数为ADMIN的默认口令，如有被修改过请赋值为当前口令）");
        succeed = await client.SecurityProxy.ChangePasswordAsync("ADMIN");
        Console.WriteLine("是否已修改成功：" + (succeed ? "是 ok" : "否 error"));
        Console.WriteLine();

        succeed = await client.SecurityProxy.LogOffAsync();
        Console.WriteLine("是否已登出成功：" + (succeed ? "是 ok" : "否 error"));
        Console.WriteLine();

        Console.WriteLine("结束");
        Console.ReadLine();
      }
    }
  }
}
