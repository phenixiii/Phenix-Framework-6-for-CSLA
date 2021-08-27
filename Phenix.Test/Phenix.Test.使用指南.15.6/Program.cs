using System;
using Phenix.Test.使用指南._15._6.Business;

namespace Phenix.Test.使用指南._15._6
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

      Console.WriteLine("需事先在数据库中建表（见WGW_DELIVERY_JOB_TICKET_TEST.sql）");
      Console.Write("准备好后请点回车继续：");
      Console.ReadLine();
      
      Console.WriteLine("设为调试状态");
      Phenix.Core.AppConfig.Debugging = true;
      Console.WriteLine();
      Console.WriteLine("模拟登陆");
      Phenix.Business.Security.UserPrincipal.User = Phenix.Business.Security.UserPrincipal.CreateTester();
      Phenix.Services.Client.Library.Registration.RegisterEmbeddedWorker(false);
      Console.WriteLine();

      Console.WriteLine("**** 测试直接操作业务码功能 ****");
      Console.WriteLine();

      Console.WriteLine("**** 测试Fetch业务码格式对象 ****");
      Phenix.Core.Dictionary.BusinessCodeFormat businessCodeFormat;
      while (true)
      {
        businessCodeFormat = Phenix.Core.Dictionary.BusinessCodeFormat.Fetch(DeliveryJobTicket.SerialProperty);
        if (businessCodeFormat != null)
          break;
        Console.WriteLine("请通过Phenix.Services.Host.x86.exe注册Phenix.Test.使用指南.15.6.Business.dll程序集");
        Console.Write("注册好后请点回车继续：");
        Console.ReadLine();
      }
      Console.WriteLine("Fetch到业务码格式对象：" + businessCodeFormat.Caption + " 格式为：" + businessCodeFormat.FormatString);
      Console.WriteLine();

      Console.WriteLine("**** 测试New业务对象时自动生成业务码 ****");
      do
      {
        Console.WriteLine("1，Edit业务码格式对象(注意：不能选'是否提交时填充值')...");
        businessCodeFormat.Edit();
      } while (businessCodeFormat.FillOnSaving);
      businessCodeFormat.Save();
      Console.WriteLine("2，Save业务码格式对象：" + businessCodeFormat.Caption + " 格式为：" + businessCodeFormat.FormatString);
      DeliveryJobTicket deliveryJobTicket = DeliveryJobTicket.New();
      Console.WriteLine("3，新增DeliveryJobTicket对象：Serial=" + deliveryJobTicket.Serial); ;
      Console.WriteLine();

      Console.WriteLine("**** 测试Save业务对象时自动生成业务码 ****");
      do
      {
        Console.WriteLine("1，Edit业务码格式对象(注意：必须选'是否提交时填充值')...");
        businessCodeFormat.Edit();
      } while (!businessCodeFormat.FillOnSaving);
      businessCodeFormat.Save();
      Console.WriteLine("2，Save业务码格式对象：" + businessCodeFormat.Caption + " 格式为：" + businessCodeFormat.FormatString);
      deliveryJobTicket = DeliveryJobTicket.New();
      Console.WriteLine("3，新增DeliveryJobTicket对象：Serial=" + deliveryJobTicket.Serial); ;
      deliveryJobTicket.Save();
      Console.WriteLine("4，提交DeliveryJobTicket对象：Serial=" + deliveryJobTicket.Serial); ;
      Console.WriteLine();

      Console.WriteLine("**** 测试在服务端Get业务码 ****");
      Console.Write("希望测试GetValues()多少量级（千枚）?");
      short count;
      if (!Int16.TryParse(Console.ReadLine(), out count))
        count = 1;
      Console.WriteLine("开始获取'{0}'业务码{1}枚...", businessCodeFormat.Caption, count * 1000);
      DateTime dt = DateTime.Now;
      foreach (string s in businessCodeFormat.GetValues(count * 1000))
      {
        Console.WriteLine(s);
      }
      Console.WriteLine("获取到'{0}'业务码{1}枚, 共用时{2}秒", businessCodeFormat.Caption, count * 1000, DateTime.Now.Subtract(dt).TotalSeconds);
      Console.WriteLine();

      Console.WriteLine("结束");
      Console.ReadLine();
    }
  }
}
