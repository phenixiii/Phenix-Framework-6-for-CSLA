using System;
using Phenix.Core;

namespace Phenix.Test.使用指南._12._6._2._5._1
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

      Console.WriteLine("**** 测试禁止持久化单个业务对象功能 ****");
      UserList users = UserList.Fetch();
      Console.WriteLine("Fetch全景User集合对象，业务对象数：" + users.Count);

      string newUserName = users[0].Name + "_test";
      users[0].Name = newUserName;
      Console.WriteLine("修改第一个User业务对象的Name属性：" + users[0].GetOldValue(User.NameProperty) + " 为 " + users[0].Name);
      Console.WriteLine("提交User集合对象...");
      try
      {
        users.Save();
      }
      catch (Exception ex)
      {
        Console.WriteLine(AppUtilities.GetErrorMessage(ex));
      }
      User user = User.Fetch(p => p.Usernumber == newUserName);
      Console.WriteLine("检查是否已提交成功: " + (user == null ? "否 ok" : "是 error"));
      Console.WriteLine();

      Console.WriteLine("结束, 与数据库交互细节见日志");
      Console.ReadLine();
    }
  }
}
