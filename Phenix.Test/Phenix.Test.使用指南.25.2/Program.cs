using System;
using System.Collections.Generic;
using System.Data.Common;
using Phenix.Core;
using Phenix.Core.Mapping;

namespace Phenix.Test.使用指南._25._2
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
      Console.WriteLine("尝试登陆");
      Phenix.Business.Security.UserPrincipal.User = Phenix.Business.Security.UserPrincipal.CreateTester();
      Phenix.Services.Client.Library.Registration.RegisterEmbeddedWorker(false);
      Console.WriteLine();

      Console.WriteLine("**** 测试UserFetchCommand功能 ****");
      Console.WriteLine();
      try
      {
        GuestUserFetchCommand userFetchCommand = new GuestUserFetchCommand().Execute();
        Console.WriteLine("通过Command的Execute()函数发起对远程服务的请求（本测试为两层架构），返回已在远程服务上执行完成DoExecute()函数的对象: {0}" + Phenix.Core.Reflection.Utilities.JsonSerialize(userFetchCommand.GuestUser));
      }
      catch (Exception ex)
      {
        Console.WriteLine("执行UserFetchCommand失败: " + AppUtilities.GetErrorHint(ex));
      }
      Console.WriteLine();

      Console.WriteLine("**** 测试GuestUserUpdateCommand功能 ****");
      Console.WriteLine();
      try
      {
        new GuestUserUpdateCommand("GU").Execute();
        new GuestUserUpdateCommand(Phenix.Core.Security.UserIdentity.GuestUserName).Execute();
        Console.WriteLine("第一条语句把Guest用户的名称改成'GU', 第二条语句恢复原来名称");
      }
      catch (Exception ex)
      {
        Console.WriteLine("执行UserFetchCommand失败: " + AppUtilities.GetErrorHint(ex));
      }
      Console.WriteLine();

      Console.WriteLine("结束, 与数据库交互细节见日志");
      Console.ReadLine();
    }
  }
}
