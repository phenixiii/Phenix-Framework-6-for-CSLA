using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.OleDb;
using Phenix.Core.Data;

namespace Phenix.Test.使用指南._03._3
{
  class Program
  {
    static void Main(string[] args)
    {
      Console.WriteLine("本程序为{0}位，请确认所连数据库客户端引擎是否与之匹配？（如不匹配可调整本程序的'平台目标'生成类型）", Environment.Is64BitProcess ? "64" : "32");
      Console.WriteLine("请通过检索“//*”了解代码中的特殊处理");
      Console.WriteLine("测试过程中的日志保存在：" + Phenix.Core.AppConfig.TempDirectory);
      Console.WriteLine();
      Console.WriteLine("设为调试状态");
      Phenix.Core.AppConfig.Debugging = true;
      Console.WriteLine();
      Console.WriteLine("模拟登陆");
      Phenix.Business.Security.UserPrincipal.User = Phenix.Business.Security.UserPrincipal.CreateTester();
      Phenix.Services.Client.Library.Registration.RegisterEmbeddedWorker(false);
      Console.WriteLine();

      Console.WriteLine("背景知识");
      Console.WriteLine("连接到数据库服务器通常由几个需要很长时间的步骤组成。");
      Console.WriteLine("实际上，大多数应用程序仅使用一个或几个不同的连接配置。");
      Console.WriteLine("这意味着在执行应用程序期间，许多相同的连接将反复地打开和关闭。");
      Console.WriteLine("为了将打开连接的开销降到最低，ADO.NET使用了一种称为 connection pooling 的优化技术。");
      Console.WriteLine("每当用户在连接上调用 Open 时，池进程就会查找池中可用的连接。如果某个池连接可用，会将该连接返回给调用者，而不是打开新连接。");
      Console.WriteLine("应用程序在该连接上调用 Close 时，池进程会将连接返回到活动连接池集中，而不是关闭连接。");
      Console.WriteLine("连接返回到池中之后，即可在下一个 Open 调用中重复使用。");
      Console.WriteLine();
      Console.WriteLine("phenixヾ除了配套ADO.NET连接池在应用层上做了进一步缓存外，还为我们在不选择ADO.NET连接池（即Pooling=false）时提供了另一种连接池。");
      Console.WriteLine("其本质上是将未调用 Close 的连接存放进连接池集中，当再次被提取出来时就会仍然是 Open 的。");
      Console.WriteLine("但这会隐藏一个风险，就是如果是自行操作数据库数据且是事务处理的话，有可能因代码漏洞将事务挂起到连接池中，首先这会造成假锁现象，其次会将状态迁移到下一个使用该连接的线程中。");
      Console.WriteLine();
      Console.WriteLine("以下是测试这两种配置下的不同效果，并顺带对多种数据打包方式的性能做了对比，供技术选型参考。");
      Console.WriteLine();

      UserList userList = UserList.New();
      OleDbConnection connection;
      const string sql =
@"select US_ID,US_USERNUMBER,US_PASSWORD,US_NAME,US_LOGIN,US_LOGOUT,US_LOGINFAILURE,US_LOGINFAILURECOUNT,US_LOGINADDRESS,US_DP_ID,US_PT_ID,US_LOCKED,US_PASSWORDCHANGEDTIME 
  from PH_USER";

      Console.WriteLine("**** 测试ADO.NET连接池效果 ****");
      DbConnectionInfo dbConnectionInfo = DbConnectionInfo.Fetch();
      int cycleCount = dbConnectionInfo.MaxPoolSize * 3;

      while (true)
      {
        if (dbConnectionInfo.IsValid(true) && dbConnectionInfo.Pooling)
          break;
        Console.WriteLine("请在数据库连接界面上将Pooling做打勾处理并能成功连接！");
        dbConnectionInfo.Setup();
      }
      Console.WriteLine();

      //为后面的计时统计提供一致的环境
      DbConnectionHelper.ClearCache();
      for (int i = 0; i < dbConnectionInfo.MaxPoolSize; i++)
      {
        connection = DbConnectionHelper.GetOleConnection(dbConnectionInfo);
        try
        {
          DbConnectionHelper.OpenConnection(connection);
        }
        finally
        {
          DbConnectionHelper.PutOleConnection(dbConnectionInfo, connection);
        }
      }

      Console.WriteLine("开始循环{0}次的Fetch()操作...", cycleCount);
      DateTime dt = DateTime.Now;
      for (int i = 0; i < cycleCount; i++)
      {
        connection = DbConnectionHelper.GetOleConnection(dbConnectionInfo);
        try
        {
          userList = UserList.Fetch(connection);
        }
        finally
        {
          DbConnectionHelper.PutOleConnection(dbConnectionInfo, connection);
        }
      }
      double fetchTotalSeconds = DateTime.Now.Subtract(dt).TotalSeconds;
      Console.WriteLine("Fetch()得到UserList(含{0}个User对象), 平均用时{1}秒", userList.Count, fetchTotalSeconds / cycleCount);
      Console.WriteLine();

      Console.WriteLine("开始循环{0}次的ExecuteReader()操作...", cycleCount);
      int rowsCount = 0;
      dt = DateTime.Now;
      for (int i = 0; i < cycleCount; i++)
      {
        connection = DbConnectionHelper.GetOleConnection(dbConnectionInfo);
        try
        {
          using (DataSet dataSet = DataSetHelper.ExecuteReader(connection, sql))
          {
            rowsCount = dataSet.Tables[0].Rows.Count;
          }
        }
        finally
        {
          DbConnectionHelper.PutOleConnection(dbConnectionInfo, connection);
        }
      }
      double executeReaderTotalSeconds = DateTime.Now.Subtract(dt).TotalSeconds;
      Console.WriteLine("ExecuteReader()得到{0}条PH_User纪录, 平均用时{1}秒", rowsCount, executeReaderTotalSeconds / cycleCount);
      Console.WriteLine();

      Console.WriteLine("开始循环{0}次的FetchList()操作...", cycleCount);
      IList<UserEasy> userEasies = new List<UserEasy>();
      dt = DateTime.Now;
      for (int i = 0; i < cycleCount; i++)
      {
        connection = DbConnectionHelper.GetOleConnection(dbConnectionInfo);
        try
        {
          userEasies = UserEasy.FetchList(connection);
        }
        finally
        {
          DbConnectionHelper.PutOleConnection(dbConnectionInfo, connection);
        }
      }
      double fetchListTotalSeconds = DateTime.Now.Subtract(dt).TotalSeconds;
      Console.WriteLine("FetchList()得到IList<UserEasy>(含{0}个UserEasy对象), 平均用时{1}秒", userEasies.Count, fetchListTotalSeconds / cycleCount);
      Console.WriteLine();

      UserEasyList userEasyList = UserEasyList.New();
      dt = DateTime.Now;
      for (int i = 0; i < cycleCount; i++)
      {
        connection = DbConnectionHelper.GetOleConnection(dbConnectionInfo);
        try
        {
          userEasyList = UserEasyList.Fetch(connection);
        }
        finally
        {
          DbConnectionHelper.PutOleConnection(dbConnectionInfo, connection);
        }
      }
      fetchListTotalSeconds = DateTime.Now.Subtract(dt).TotalSeconds;
      Console.WriteLine("也可以FetchList()得到UserEasyList(含{0}个UserEasy对象), 平均用时{1}秒", userEasyList.Count, fetchListTotalSeconds / cycleCount);
      Console.WriteLine();

      Console.WriteLine("对照效果为：");
      Console.WriteLine("Fetch()与ExecuteReader()的耗时之比为{0}", fetchTotalSeconds / executeReaderTotalSeconds);
      Console.WriteLine("FetchList()与ExecuteReader()的耗时之比为{0}", fetchListTotalSeconds / executeReaderTotalSeconds);
      connection = DbConnectionHelper.GetOleConnection(dbConnectionInfo);
      try
      {
        using (DataSet dataSet = DataSetHelper.ExecuteReader(connection, sql))
        {
          Console.WriteLine("UserList与DataTable的反序列化大小之比为{0}", (float)Phenix.Core.Reflection.Utilities.BinarySerialize(userList).Length / Phenix.Core.Reflection.Utilities.BinarySerialize(dataSet.Tables[0]).Length);
          Console.WriteLine("UserEasyList与DataTable的反序列化大小之比为{0}", (float)Phenix.Core.Reflection.Utilities.BinarySerialize(userEasyList).Length / Phenix.Core.Reflection.Utilities.BinarySerialize(dataSet.Tables[0]).Length);
        }
      }
      finally
      {
        DbConnectionHelper.PutOleConnection(dbConnectionInfo, connection);
      }
      Console.Write("按回车键继续：");
      Console.ReadLine();
      Console.WriteLine();

      Console.WriteLine("**** 测试phenixヾ数据库连接池效果 ****");

      dbConnectionInfo = DbConnectionInfo.Fetch();
      while (true)
      {
        if (dbConnectionInfo.IsValid(true) && !dbConnectionInfo.Pooling)
          break;
        Console.WriteLine("请在数据库连接界面上将Pooling撤销打勾处理并能成功连接！");
        dbConnectionInfo.Setup();
      }
      Console.WriteLine();

      //为后面的计时统计提供一致的环境
      DbConnectionHelper.ClearCache();
      for (int i = 0; i < cycleCount; i++)
      {
        connection = DbConnectionHelper.GetOleConnection(dbConnectionInfo);
        try
        {
          DbConnectionHelper.OpenConnection(connection);
        }
        finally
        {
          DbConnectionHelper.PutOleConnection(dbConnectionInfo, connection);
        }
      }

      Console.WriteLine("开始循环{0}次的Fetch()操作...", cycleCount);
      dt = DateTime.Now;
      for (int i = 0; i < cycleCount; i++)
      {
        connection = DbConnectionHelper.GetOleConnection(dbConnectionInfo);
        try
        {
          userList = UserList.Fetch(connection);
        }
        finally
        {
          DbConnectionHelper.PutOleConnection(dbConnectionInfo, connection);
        }
      }
      double selfFetchTotalSeconds = DateTime.Now.Subtract(dt).TotalSeconds;
      Console.WriteLine("Fetch()得到UserList(含{0}个User对象), 平均用时{1}秒", userList.Count, selfFetchTotalSeconds / cycleCount);
      Console.WriteLine();

      Console.WriteLine("对照效果为：");
      Console.WriteLine("ADO.NET连接池与phenixヾ数据库连接池的耗时之比为{0}", fetchTotalSeconds / selfFetchTotalSeconds);
      Console.WriteLine();

      Console.WriteLine("结束, 与数据库交互细节见日志");
      Console.WriteLine("如需了解Entity的使用方法，请运行“Phenix.Test.使用指南.03.5”工程");
      Console.ReadLine();
    }
  }
}
