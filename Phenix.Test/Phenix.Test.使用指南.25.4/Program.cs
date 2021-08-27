using System;
using System.Collections.Generic;
using System.Data.Common;
using Phenix.Core;
using Phenix.Core.Mapping;

namespace Phenix.Test.使用指南._25._4
{
  class Program
  {
    private static long?[] TestInsert(DbTransaction transaction, int count)
    {
      List<long?> result = new List<long?>(count);
      for (int i = 0; i < count; i++)
      {
        string s = Phenix.Core.Data.Sequence.Value.ToString().Substring(5);
        UserEasy user = UserEasy.New(s, s, s, null, null, null, 0, null, null, null, 0, null);
        if (user.Save(transaction))
          result.Add(user.US_ID);
      }
      return result.ToArray();
    }

    private static int TestUpdate(DbTransaction transaction, long?[] userIds)
    {
      int result = 0;
      foreach (long? userId in userIds)
        result = result +
          UserEasyList.UpdateRecord(transaction, UserEasy.US_IDProperty == userId,
            PropertyValue.Set(UserEasy.LockedProperty, 1),
            PropertyValue.Set(UserEasy.LogoutProperty, DateTime.Now));
      return result;
    }

    private static int DeleteRecord(DbTransaction transaction, long?[] userIds)
    {
      int result = 0;
      foreach (long? userId in userIds)
        result = result +
          UserEasyList.DeleteRecord(transaction, UserEasy.US_IDProperty == userId);
      return result;
    }

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

      int insertCount = 10;
      long?[] userIds = null;

      Console.WriteLine("**** 测试Insert功能 ****");
      Console.WriteLine();
      try
      {
        userIds = Phenix.Core.Data.DefaultDatabase.ExecuteGet(TestInsert, insertCount);
        Console.WriteLine("新增User记录: " + (userIds.Length == insertCount ? " ok" : "error"));
      }
      catch (Exception ex)
      {
        Console.WriteLine("新增User记录: " + AppUtilities.GetErrorHint(ex));
      }
      Console.WriteLine();

      Console.WriteLine("**** 测试Update功能 ****");
      Console.WriteLine();
      try
      {
        int updateCount = Phenix.Core.Data.DefaultDatabase.ExecuteGet(TestUpdate, userIds);
        Console.WriteLine("编辑User记录: " + (updateCount == insertCount ? " ok" : "error"));
      }
      catch (Exception ex)
      {
        Console.WriteLine("编辑User记录: " + AppUtilities.GetErrorHint(ex));
      }
      Console.WriteLine();

      Console.WriteLine("**** 测试删除功能 ****");
      Console.WriteLine();
      try
      {
        int deleteCount = Phenix.Core.Data.DefaultDatabase.ExecuteGet(DeleteRecord, userIds);
        Console.WriteLine("删除User记录: " + (deleteCount == insertCount ? " ok" : "error"));
      }
      catch (Exception ex)
      {
        Console.WriteLine("删除User记录: " + AppUtilities.GetErrorHint(ex));
      }
      Console.WriteLine();

      Console.WriteLine("结束, 与数据库交互细节见日志");
      Console.ReadLine();
    }
  }
}
