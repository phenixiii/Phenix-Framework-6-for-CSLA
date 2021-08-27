using System;
using System.Data.Common;
using Phenix.Core;
using Phenix.Core.Data;
using Phenix.Core.Mapping;

namespace Phenix.Test.使用指南._12._6._2._7
{
  class Program
  {
    private static readonly long FSP_ID = Sequence.Value;
    private static readonly long FBC_ID = Sequence.Value;
    private static readonly long FSC_ID = Sequence.Value;
    private static readonly long FCR_ID = Sequence.Value;

    private static void BuildCondition()
    {
      DefaultDatabase.ExecuteOle(ExecuteBuildCondition);
    }

    private static void ExecuteBuildCondition(DbConnection connection)
    {
      try
      {
        DbCommandHelper.ExecuteNonQuery(connection,
@"create table CFC_FINANCIAL_STATISTIC_PERIOD 
(
   FSP_ID               NUMERIC(15)          not null,
   constraint PK_CFC_FINANCIAL_STATISTIC_PER primary key (FSP_ID)
)", false);
      }
      catch (Exception ex)
      {
        Console.WriteLine("数据库中已构建了CFC_FINANCIAL_STATISTIC_PERIOD表: " + ex.Message);
      }

      try
      {
        DbCommandHelper.ExecuteNonQuery(connection,
@"create table CFC_FINANCIAL_BIG_CLASS 
(
   FBC_ID               NUMERIC(15)          not null,
   constraint PK_CFC_FINANCIAL_BIG_CLASS primary key (FBC_ID)
)", false);
      }
      catch (Exception ex)
      {
        Console.WriteLine("数据库中已构建了CFC_FINANCIAL_BIG_CLASS表: " + ex.Message);
      }

      try
      {
        DbCommandHelper.ExecuteNonQuery(connection,
@"create table CFC_FINANCIAL_SMALL_CLASS 
(
   FSC_ID               NUMERIC(15)          not null,
   constraint PK_CFC_FINANCIAL_SMALL_CLASS primary key (FSC_ID)
)", false);
      }
      catch (Exception ex)
      {
        Console.WriteLine("数据库中已构建了CFC_FINANCIAL_SMALL_CLASS表: " + ex.Message);
      }

      try
      {
        DbCommandHelper.ExecuteNonQuery(connection,
@"create table CFC_FINANCIAL_CLASS_RELATION 
(
   FCR_ID               NUMERIC(15)          not null,
   FCR_FBC_ID           NUMERIC(15)          not null,
   FCR_FSC_ID           NUMERIC(15)          not null,
   FCR_FSP_ID           NUMERIC(15),
   constraint PK_CFC_FINANCIAL_CLASS_RELATIO primary key (FCR_ID)
)", false);
      }
      catch (Exception ex)
      {
        Console.WriteLine("数据库中已构建了CFC_FINANCIAL_CLASS_RELATION表: " + ex.Message);
      }

      try
      {
        DbCommandHelper.ExecuteNonQuery(connection,
@"create table CFC_FINANCIAL_CLASS_RELATION_I
(
   FCI_ID               NUMERIC(15)          not null,
   FCI_FCR_ID           NUMERIC(15)          not null,
   constraint PK_CFC_FINANCIAL_CLASS_RELATII primary key (FCI_ID)
)", false);
      }
      catch (Exception ex)
      {
        Console.WriteLine("数据库中已构建了CFC_FINANCIAL_CLASS_RELATION表: " + ex.Message);
      }

      try
      {
        DbCommandHelper.ExecuteNonQuery(connection,
@"alter table CFC_FINANCIAL_CLASS_RELATION
   add constraint FK_FCR_FBC_ID foreign key (FCR_FBC_ID)
      references CFC_FINANCIAL_BIG_CLASS (FBC_ID)", false);
      }
      catch (Exception ex)
      {
        Console.WriteLine("数据库中已构建了FK_FCR_FBC_ID约束: " + ex.Message);
      }

      try
      {
        DbCommandHelper.ExecuteNonQuery(connection,
@"alter table CFC_FINANCIAL_CLASS_RELATION
   add constraint FK_FCR_FSC_ID foreign key (FCR_FSC_ID)
      references CFC_FINANCIAL_SMALL_CLASS (FSC_ID)", false);
      }
      catch (Exception ex)
      {
        Console.WriteLine("数据库中已构建了FK_FCR_FSC_ID约束: " + ex.Message);
      }

      //      try
      //      {
      //        DbCommandHelper.ExecuteNonQuery(connection,
      //@"alter table CFC_FINANCIAL_CLASS_RELATION
      //   add constraint FK_FCR_FSP_ID foreign key (FCR_FSP_ID)
      //      references CFC_FINANCIAL_STATISTIC_PERIOD (FSP_ID)", false);
      //      }
      //      catch (Exception ex)
      //      {
      //        Console.WriteLine("数据库中已构建了FK_FCR_FSP_ID约束: " + ex.Message);
      //      }
    }

    private static void BuildTestData()
    {
      DefaultDatabase.ExecuteOle(ExecuteBuildTestData);
    }

    private static void ExecuteBuildTestData(DbTransaction transaction)
    {
      DbCommandHelper.ExecuteNonQuery(transaction,
        "insert into CFC_FINANCIAL_STATISTIC_PERIOD" +
        "(FSP_ID)" +
        "values" +
        "(" + FSP_ID + ")");
      DbCommandHelper.ExecuteNonQuery(transaction,
        "insert into CFC_FINANCIAL_BIG_CLASS" +
        "(FBC_ID)" +
        "values" +
        "(" + FBC_ID + ")");
      DbCommandHelper.ExecuteNonQuery(transaction,
        "insert into CFC_FINANCIAL_SMALL_CLASS" +
        "(FSC_ID)" +
        "values" +
        "(" + FSC_ID + ")");
      DbCommandHelper.ExecuteNonQuery(transaction,
        "insert into CFC_FINANCIAL_CLASS_RELATION" +
        "(FCR_ID, FCR_FBC_ID, FCR_FSC_ID, FCR_FSP_ID)" +
        "values" +
        "(" + FCR_ID + "," + FBC_ID +"," + FSC_ID + "," + FSP_ID + ")");
    }

    private static void ClearCondition()
    {
      DefaultDatabase.ExecuteOle(ExecuteClearCondition);
    }

    private static void ExecuteClearCondition(DbConnection connection)
    {
      try
      {
        DbCommandHelper.ExecuteNonQuery(connection,
@"alter table CFC_FINANCIAL_CLASS_RELATION
   drop constraint FK_FCR_FBC_ID", false);
      }
      catch (Exception ex)
      {
        Console.WriteLine("数据库中已删除了FK_FCR_FBC_ID约束: " + ex.Message);
      }

      try
      {
        DbCommandHelper.ExecuteNonQuery(connection,
@"alter table CFC_FINANCIAL_CLASS_RELATION
   drop constraint FK_FCR_FSC_ID", false);
      }
      catch (Exception ex)
      {
        Console.WriteLine("数据库中已删除了FK_FCR_FSC_ID约束: " + ex.Message);
      }

//      try
//      {
//        DbCommandHelper.ExecuteNonQuery(connection,
//@"alter table CFC_FINANCIAL_CLASS_RELATION
//   drop constraint FK_FCR_FSP_ID", false);
//      }
//      catch (Exception ex)
//      {
//        Console.WriteLine("数据库中已删除了FK_FCR_FSP_ID约束: " + ex.Message);
//      }

      try
      {
        DbCommandHelper.ExecuteNonQuery(connection,
@"drop table CFC_FINANCIAL_CLASS_RELATION", false);
      }
      catch (Exception ex)
      {
        Console.WriteLine("数据库中已删除了CFC_FINANCIAL_CLASS_RELATION表: " + ex.Message);
      }

      try
      {
        DbCommandHelper.ExecuteNonQuery(connection,
@"drop table CFC_FINANCIAL_SMALL_CLASS", false);
      }
      catch (Exception ex)
      {
        Console.WriteLine("数据库中已删除了CFC_FINANCIAL_SMALL_CLASS表: " + ex.Message);
      }

      try
      {
        DbCommandHelper.ExecuteNonQuery(connection,
@"drop table CFC_FINANCIAL_BIG_CLASS", false);
      }
      catch (Exception ex)
      {
        Console.WriteLine("数据库中已删除了CFC_FINANCIAL_BIG_CLASS表: " + ex.Message);
      }

      try
      {
        DbCommandHelper.ExecuteNonQuery(connection,
@"drop table CFC_FINANCIAL_STATISTIC_PERIOD", false);
      }
      catch (Exception ex)
      {
        Console.WriteLine("数据库中已删除了CFC_FINANCIAL_STATISTIC_PERIOD表: " + ex.Message);
      }
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

      Console.WriteLine("**** 搭建测试环境 ****");
      BuildCondition();
      Console.WriteLine();

      Console.WriteLine("**** 搭建测试数据 ****");
      BuildTestData();
      Console.WriteLine();

      Console.WriteLine("**** 测试未显式申明ClassDetailAttribute时禁止级联删除的功能 ****");
      FinancialBigClass financialBigClass = FinancialBigClass.Fetch(FBC_ID);
      financialBigClass.Delete();
      try
      {
        financialBigClass.Save();
        Console.WriteLine("删除FinancialBigClass对象成功 error");
      }
      catch (DeleteException ex)
      {
        Console.WriteLine("拦截到Phenix.Business.DeleteException");
        Console.WriteLine("删除FinancialBigClass对象时抛出异常：" + AppUtilities.GetErrorMessage(ex) + " ok");
      }
      Console.WriteLine();

      Console.WriteLine("**** 测试显式申明ClassDetailAttribute且CascadingDelete = false时自动Unlink子表外键的功能 ****");
      FinancialStatisticPeriod financialStatisticPeriod = FinancialStatisticPeriod.Fetch(FSP_ID);
      financialStatisticPeriod.Delete();
      try
      {
        financialStatisticPeriod.Save();
        FinancialClassRelation financialClassRelation = FinancialClassRelation.Fetch(FCR_ID);
        if (financialClassRelation != null && !financialClassRelation.FCR_FSP_ID.HasValue)
          Console.WriteLine("删除FinancialBigClass对象成功且FinancialClassRelation的FCR_FSP_ID外键被置为null ok");
        else
          Console.WriteLine("删除FinancialBigClass对象成功且FinancialClassRelation的FCR_FSP_ID外键被置为null error");
      }
      catch (Exception ex)
      {
        Console.WriteLine("删除FinancialStatisticPeriod对象时抛出异常：" + AppUtilities.GetErrorMessage(ex) + " error");
      }
      Console.WriteLine();

      Console.WriteLine("**** 测试显式申明ClassDetailAttribute且CascadingDelete = true时自动删除子表外键的功能 ****");
      FinancialSmallClass financialSmallClass = FinancialSmallClass.Fetch(FSC_ID);
      financialSmallClass.Delete();
      try
      {
        financialSmallClass.Save();
        FinancialClassRelation financialClassRelation = FinancialClassRelation.Fetch(FCR_ID);
        if (financialClassRelation == null)
          Console.WriteLine("删除FinancialSmallClass对象也自动删除了FinancialClassRelation对象 ok");
        else
          Console.WriteLine("删除FinancialSmallClass对象但未自动删除FinancialClassRelation对象 error");
      }
      catch (Exception ex)
      {
        Console.WriteLine("删除FinancialSmallClass对象时抛出异常：" + AppUtilities.GetErrorMessage(ex) + " error");
      }
      Console.WriteLine();

      Console.Write("是否清除测试环境?(Y/N)");
      if (String.Compare(Console.ReadLine(), "Y", StringComparison.OrdinalIgnoreCase) == 0)
      {
        ClearCondition();
        Console.WriteLine("完成清除测试环境");
      }
      Console.WriteLine();

      Console.WriteLine("结束, 与数据库交互细节见日志");
      Console.ReadLine();
    }
  }
}
