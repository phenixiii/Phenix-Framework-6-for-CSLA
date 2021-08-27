using System;

namespace Phenix.Test.使用指南._16._3
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

      Console.WriteLine("**** 测试分页检索业务对象功能 ****");
      Console.WriteLine("缺省分页大小：DefaultPageSize=" + UserReadOnlyList.DefaultPageSize);
      UserReadOnlyList.DefaultPageSize = 10;
      Console.WriteLine("可修改为：DefaultPageSize=" + UserReadOnlyList.DefaultPageSize);
      Console.WriteLine();

      UserReadOnlyList users = UserReadOnlyList.Fetch();
      Console.WriteLine("按DefaultPageSize=" + UserReadOnlyList.DefaultPageSize + ", Fetch用户集合对象users");
      Console.WriteLine("其最大记录数，MaxCount=" + users.MaxCount);
      Console.WriteLine("其最大分页号，MaxPageNo=" + users.MaxPageNo);
      Console.WriteLine("其当前对象数，Count=" + users.Count);
      Console.WriteLine();

      users = UserReadOnlyList.Fetch(p => !p.Locked.Value, 5);
      Console.WriteLine("按PageSize=" + users.PageSize + ", Fetch未被锁的用户集合对象users");
      Console.WriteLine("其最大记录数，MaxCount=" + users.MaxCount);
      Console.WriteLine("其最大分页号，MaxPageNo=" + users.MaxPageNo);
      Console.WriteLine("其当前对象数，Count=" + users.Count);
      Console.WriteLine("当前第" + users.PageNo + "页的对象：");
      foreach (UserReadOnly user in users)
        Console.Write(user.Name + ", ");
      Console.WriteLine();
      for (int i = 1; i < users.MaxPageNo; i++)
      {
        Console.WriteLine("Fetch下一页的对象：");
        foreach (UserReadOnly user in users.FetchNextPage())
          Console.Write(user.Name + ", ");
        Console.WriteLine();
        Console.WriteLine("其当前对象数，Count=" + users.Count);
        Console.WriteLine("其本地分页数量，LocalPagesCount=" + users.LocalPagesCount);
        Console.WriteLine("其当前分页号，PageNo=" + users.PageNo);
        Console.Write("按回车键，继续Fetch下一页：");
        Console.ReadLine();
      }
      Console.WriteLine("Fetch完全部页");
      Console.WriteLine();
      int pageNo = new Random().Next(1, users.MaxPageNo);
      Console.WriteLine("也可以随意Fetch第" + pageNo + "页的对象（返回本地缓存对象）：");
      foreach (UserReadOnly user in users.FetchPage(pageNo))
        Console.Write(user.Name + ", ");
      Console.WriteLine();
      Console.WriteLine("其当前对象数，Count=" + users.Count);
      Console.WriteLine("其本地分页数量，LocalPagesCount=" + users.LocalPagesCount);
      Console.WriteLine("其当前分页号，PageNo=" + users.PageNo);
      Console.WriteLine();

      Console.WriteLine("结束, 与数据库交互细节见日志");
      Console.ReadLine();
    }
  }
}
