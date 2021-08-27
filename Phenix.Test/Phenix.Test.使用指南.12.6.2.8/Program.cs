using System;
using System.Reflection;
using Phenix.Core;
using Phenix.Core.Mapping;

namespace Phenix.Test.使用指南._12._6._2._8
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

      Console.WriteLine("**** 搭建测试环境 ****");
      string key = Assembly.GetExecutingAssembly().GetName().Name + ":" + Phenix.Core.Data.Sequence.Value;
      Serial serial = Serial.New(key, 0, DateTime.Now);
      serial.Save();
      serial.Time = DateTime.Now.AddSeconds(1);
      serial.Save();
      Console.WriteLine();

      Console.WriteLine("**** 测试ClassAttribute.AllowIgnoreCheckDirty=false（默认值）配置下的乐观锁功能 ****");
      Serial serial1 = Serial.Fetch(p => p.Key == key);
      Console.WriteLine("Fetch到Serial对象1：Time=" + serial1.Time);
      Serial serial2 = Serial.Fetch(p => p.Key == key);
      Console.WriteLine("Fetch到Serial对象2：Time=" + serial2.Time);
      serial1.Time = DateTime.Now.AddSeconds(2);
      Console.WriteLine("修改Serial对象1：Time=" + serial1.Time);
      serial2.Time = DateTime.Now.AddSeconds(3);
      Console.WriteLine("修改Serial对象2：Time=" + serial2.Time);
      serial1.Save();
      Console.WriteLine("保存Serial对象1成功 ok");
      Console.WriteLine();
      try
      {
        serial2.Save();
        Console.WriteLine("保存Serial对象2成功 error");
        Console.WriteLine();
      }
      catch (Phenix.Business.CheckSaveException ex1)
      {
        Console.WriteLine("拦截到Phenix.Business.CheckSaveException");
        Console.WriteLine("保存Serial对象2时抛出异常：" + AppUtilities.GetErrorMessage(ex1) + " ok");
        Console.WriteLine();
        try
        {
          serial2.Save(false);
          Console.WriteLine("Serial对象2尝试以needCheckDirty=false方式再次保存成功 error");
          Console.WriteLine();
        }
        catch (Phenix.Business.CheckSaveException ex2)
        {
          Console.WriteLine("拦截到Phenix.Business.CheckSaveException");
          Console.WriteLine("Serial对象2尝试以needCheckDirty=false方式再次保存抛出异常：" + AppUtilities.GetErrorMessage(ex2) + " ok");
          Console.WriteLine();
          try
          {
            serial2.Delete();
            serial2.Save(false);
            Console.WriteLine("Serial对象2尝试删除成功 error");
            Console.WriteLine();
          }
          catch (Phenix.Business.CheckSaveException ex3)
          {
            Console.WriteLine("拦截到Phenix.Business.CheckSaveException");
            Console.WriteLine("Serial对象2尝试删除时抛出异常：" + AppUtilities.GetErrorMessage(ex3) + " ok");
            Console.WriteLine();
          }
        }
      }
      catch (Exception ex)
      {
        Console.WriteLine("保存Serial对象2时抛出异常：" + AppUtilities.GetErrorMessage(ex) + " error");
        Console.WriteLine();
      }
      Console.WriteLine();

      Console.WriteLine("**** 测试ClassAttribute.AllowIgnoreCheckDirty=true配置下的乐观锁功能 ****");
      Serial_AllowIgnoreCheckDirty serial_AllowIgnoreCheckDirty1 = Serial_AllowIgnoreCheckDirty.Fetch(p => p.Key == key);
      Console.WriteLine("Fetch到Serial_AllowIgnoreCheckDirty对象1：Time=" + serial_AllowIgnoreCheckDirty1.Time);
      Serial_AllowIgnoreCheckDirty serial_AllowIgnoreCheckDirty2 = Serial_AllowIgnoreCheckDirty.Fetch(p => p.Key == key);
      Console.WriteLine("Fetch到Serial_AllowIgnoreCheckDirty对象2：Time=" + serial_AllowIgnoreCheckDirty2.Time);
      serial_AllowIgnoreCheckDirty1.Time = DateTime.Now.AddSeconds(4);
      Console.WriteLine("修改Serial_AllowIgnoreCheckDirty对象1：Time=" + serial_AllowIgnoreCheckDirty1.Time);
      serial_AllowIgnoreCheckDirty2.Time = DateTime.Now.AddSeconds(5);
      Console.WriteLine("修改Serial_AllowIgnoreCheckDirty对象2：Time=" + serial_AllowIgnoreCheckDirty2.Time);
      serial_AllowIgnoreCheckDirty1.Save();
      Console.WriteLine("保存Serial_AllowIgnoreCheckDirty对象1成功 ok");
      Console.WriteLine();
      try
      {
        serial_AllowIgnoreCheckDirty2.Save();
        Console.WriteLine("保存Serial_AllowIgnoreCheckDirty对象2成功 error");
        Console.WriteLine();
      }
      catch (CheckDirtyException ex1)
      {
        Console.WriteLine("拦截到Phenix.Business.CheckDirtyException");
        Console.WriteLine("保存Serial_AllowIgnoreCheckDirty对象2时抛出异常：" + AppUtilities.GetErrorMessage(ex1) + " ok");
        Console.WriteLine();
        try
        {
          serial_AllowIgnoreCheckDirty2.Save(false);
          Console.WriteLine("Serial_AllowIgnoreCheckDirty对象2尝试以needCheckDirty=false方式再次保存成功 ok");
          Console.WriteLine();
        }
        catch (CheckDirtyException ex2)
        {
          Console.WriteLine("拦截到Phenix.Business.CheckDirtyException");
          Console.WriteLine("Serial_AllowIgnoreCheckDirty对象2尝试以needCheckDirty=false方式再次保存时抛出异常：" + AppUtilities.GetErrorMessage(ex2) + " error");
          Console.WriteLine();
        }
      }
      catch (Exception ex)
      {
        Console.WriteLine("保存Serial对象2时抛出异常：" + AppUtilities.GetErrorMessage(ex) + " error");
        Console.WriteLine();
      }
      Console.WriteLine();

      Console.WriteLine("**** 清理测试环境 ****");
      //serial_AllowIgnoreCheckDirty2.Delete();
      //serial_AllowIgnoreCheckDirty2.Save(false, false);
      serial.Refresh();
      serial.Delete();
      serial.Save();
      Console.WriteLine();

      Console.WriteLine("结束, 与数据库交互细节见日志");
      Console.ReadLine();
    }
  }
}
