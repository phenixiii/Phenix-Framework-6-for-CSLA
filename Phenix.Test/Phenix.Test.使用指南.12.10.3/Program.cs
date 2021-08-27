using System;
using System.Data;
using System.Data.Common;
using System.Linq;
using Phenix.Core.Data;

namespace Phenix.Test.使用指南._12._10._3
{
    class Program
    {
        private static void BuildCondition()
        {
            DefaultDatabase.ExecuteOle(ExecuteBuildCondition);
        }

        private static void ExecuteBuildCondition(DbConnection connection)
        {
            try
            {
                DbCommandHelper.ExecuteNonQuery(connection, @"
create table CSR_CUSTOMER_INFO 
(
   CTI_ID               NUMERIC(15)          not null,
   CTI_CODE             VARCHAR(25)          not null,
   constraint PK_CSR_CUSTOMER_INFO primary key (CTI_ID)
)", false);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            try
            {
                DbCommandHelper.ExecuteNonQuery(connection, @"
create table CSR_CUSTOMER_IDENTITY 
(
   CCI_CTI_ID               NUMERIC(15)          not null,
   CCI_CUSTOMER_IDENTITY_FG NUMERIC(2)           not null,
   constraint PK_CSR_CUSTOMER_IDENTITY primary key (CCI_CTI_ID, CCI_CUSTOMER_IDENTITY_FG)
)", false);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            try
            {
                DbCommandHelper.ExecuteNonQuery(connection, @"
alter table CSR_CUSTOMER_IDENTITY
   add constraint FK_CCI_CTI_ID foreign key (CCI_CTI_ID)
      references CSR_CUSTOMER_INFO (CTI_ID)", false);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private static void ClearCondition()
        {
            DefaultDatabase.ExecuteOle(ExecuteClearCondition);
        }

        private static void ExecuteClearCondition(DbConnection connection)
        {
            try
            {
                DbCommandHelper.ExecuteNonQuery(connection, @"
alter table CSR_CUSTOMER_IDENTITY
   drop constraint FK_CCI_CTI_ID", false);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            try
            {
                DbCommandHelper.ExecuteNonQuery(connection, @"
drop table CSR_CUSTOMER_IDENTITY cascade constraints", false);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            try
            {
                DbCommandHelper.ExecuteNonQuery(connection, @"
drop table CSR_CUSTOMER_INFO cascade constraints", false);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
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

            Console.WriteLine("**** 测试枚举绑定从业务对象勾选方法的功能 ****");

            Console.WriteLine("新增 CustomerInfo 对象...");
            CustomerInfo customerInfo = CustomerInfo.New();
            customerInfo.Code = "TEST";

            Console.Write("已分配的身份类型: ");
            if (customerInfo.CustomerIdentityList.Count == 0)
                Console.WriteLine("无");
            else
            {
                foreach (CustomerIdentity item in customerInfo.CustomerIdentityList)
                {
                    Console.Write(item.IdentityCaption);
                    Console.Write(",");
                }

                Console.WriteLine();
            }

            Console.Write("供勾选的身份类型: ");
            foreach (CustomerIdentity item in customerInfo.SelectableCustomerIdentityList)
            {
                Console.Write(item.IdentityCaption);
                Console.Write(",");
            }

            Console.WriteLine();

            Console.WriteLine("逐一勾选身份类型: ");
            foreach (CustomerIdentity item in customerInfo.SelectableCustomerIdentityList)
            {
                item.Selected = true;
                Console.Write(item.IdentityCaption + (customerInfo.CustomerIdentityList.Select(p => p.Identity == item.Identity).Any() ? "OK" : "NO"));
                Console.WriteLine();
            }

            Console.WriteLine();

            Console.WriteLine("提交 CustomerInfo 对象并从数据库获取");
            customerInfo.Save();
            customerInfo = CustomerInfo.Fetch(customerInfo.CTI_ID.Value);

            Console.Write("已分配的身份类型: ");
            foreach (CustomerIdentity item in customerInfo.CustomerIdentityList)
            {
                Console.Write(item.IdentityCaption);
                Console.Write(",");
            }

            Console.WriteLine();

            Console.WriteLine("逐一勾去身份类型: ");
            foreach (CustomerIdentity item in customerInfo.SelectableCustomerIdentityList)
            {
                item.Selected = false;
                Console.Write(item.IdentityCaption + (!customerInfo.CustomerIdentityList.Any(p => p.Identity == item.Identity) ? "OK" : "NO"));
                Console.WriteLine();
            }

            Console.WriteLine();

            Console.WriteLine("提交 CustomerInfo 对象并从数据库获取");
            customerInfo.Save();
            customerInfo = CustomerInfo.Fetch(customerInfo.CTI_ID.Value);

            Console.Write("已分配的身份类型: ");
            foreach (CustomerIdentity item in customerInfo.CustomerIdentityList)
            {
                Console.Write(item.IdentityCaption);
                Console.Write(",");
            }

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