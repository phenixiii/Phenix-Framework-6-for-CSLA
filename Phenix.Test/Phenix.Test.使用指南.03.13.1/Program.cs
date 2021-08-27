using System;
using System.IO;
using Phenix.Core;

namespace Phenix.Test.使用指南._03._13._1
{
  class Program
  {
    //* 配置项属性
    //* 注意需将字段定义为Nullable<T>类型，以便Phenix.Core.AppSettings用于区分是否已赋过值
    private static bool? _test_InEncrypt;
    /// <summary>
    /// 测试用
    /// 默认：false
    /// </summary>
    public static bool Test_InEncrypt
    {
      get { return Phenix.Core.AppSettings.GetProperty(ref _test_InEncrypt, false, true, true); }
      set { Phenix.Core.AppSettings.SetProperty(ref _test_InEncrypt, value, true, true); }
    }

    //* 配置项属性
    //* 注意需将字段定义为Nullable<T>类型，以便Phenix.Core.AppSettings用于区分是否已赋过值
    private static bool? _test;
    /// <summary>
    /// 测试用
    /// 默认：false
    /// </summary>
    public static bool Test
    {
      get { return Phenix.Core.AppSettings.GetProperty(ref _test, false); }
      set { Phenix.Core.AppSettings.SetProperty(ref _test, value); }
    }

    static void Main(string[] args)
    {
      Console.WriteLine("请通过检索“//*”了解代码中的特殊处理");
      Console.WriteLine("测试过程中的日志保存在：" + Phenix.Core.AppConfig.TempDirectory);
      Console.WriteLine();
      Console.WriteLine("->请使用超级管理员运行本程序<-");
      Console.Write("按回车键继续：");
      Console.ReadLine();
      Console.WriteLine();

      Console.WriteLine("设为调试状态");
      Phenix.Core.AppConfig.Debugging = true;
      Console.WriteLine();
      //Console.WriteLine("模拟登陆");
      //Phenix.Business.Security.UserPrincipal.User = Phenix.Business.Security.UserPrincipal.CreateTester();
      //Phenix.Services.Client.Library.Registration.RegisterEmbeddedWorker(false);
      //Console.WriteLine();

      Console.WriteLine("**** 测试类继承关系下，泛型虚拟父类的静态属性如何在各子类中setter、getter不同值的功能 ****");
      Console.WriteLine("**** 利用类的本地配置功能 ****");
      TestA.FirstDate = DateTime.MaxValue;
      Console.WriteLine("设置TestA.FirstDate=" + TestA.FirstDate);
      TestB.FirstDate = DateTime.MinValue;
      Console.WriteLine("设置TestB.FirstDate=" + TestB.FirstDate);
      Console.WriteLine("读取TestA.FirstDate=" + TestA.FirstDate);
      Console.WriteLine("**** 利用泛型类的静态属性特性 ****");
      TestA.LastDate = DateTime.MaxValue;
      Console.WriteLine("设置TestA.LastDate=" + TestA.LastDate);
      TestB.LastDate = DateTime.MinValue;
      Console.WriteLine("设置TestB.LastDate=" + TestB.LastDate);
      Console.WriteLine("读取TestA.LastDate=" + TestA.LastDate);
      Console.Write("按回车键继续：");
      Console.ReadLine();
      Console.WriteLine();

      Console.WriteLine("**** 测试加密（传inEncrypt=true参数）的配置功能 ****");
      Test_InEncrypt = !Test_InEncrypt;
      Console.WriteLine("设置属性Test_InEncrypt=" + Test_InEncrypt);
      Console.WriteLine("请检查" + Phenix.Core.AppSettings.ConfigFilename + "配置文件中key='Phenix.Test.使用指南._03._13._1.Program.Test_InEncrypt.'的设置值是否已加密");
      Console.Write("按回车键继续：");
      Console.ReadLine();
      Console.WriteLine();

      const string key = "Phenix.Test.使用指南._03._13._1.Program.Test.";

      Console.WriteLine("**** 测试类的本地配置功能 ****");
      Console.WriteLine("提取属性Test=" + Test);
      Console.WriteLine("配置文件存放在：" + Phenix.Core.AppSettings.ConfigFilename);
      Test = !Test;
      Console.WriteLine("设置属性Test=" + Test);
      Console.WriteLine("请检查" + Phenix.Core.AppSettings.ConfigFilename + "配置文件中key='" + key + "'的设置值是否等于" + Test);
      Console.Write("按回车键继续：");
      Console.ReadLine();
      Console.WriteLine();

      Console.WriteLine("**** 测试修改本地配置存放文件功能 ****");
      Phenix.Core.AppSettings.ConfigFilename = System.IO.Path.Combine(@"C:\\", Path.GetFileName(AppSettings.DefaultConfigFilename));
      Console.WriteLine("修改本地配置存放文件为：" + Phenix.Core.AppSettings.ConfigFilename);
      Console.WriteLine("本方法，适用于使用 ClickOnce 来部署 Windows 窗体应用程序，以永久存放系统配置项内容");
      Console.WriteLine("在正式应用中，该语句请紧跟在LogOn.Execute()语句执行完之后被调用!");
      Test = !Test;
      Console.WriteLine("设置属性Test=" + Test);
      Console.WriteLine("请检查" + Phenix.Core.AppSettings.ConfigFilename + "配置文件中key='" + key + "'的设置值是否等于" + Test);
      Console.Write("按回车键继续：");
      Console.ReadLine();
      Console.WriteLine();

      Console.WriteLine("**** 测试删除配置信息后会到缺省配置文件里取值 ****");
      Phenix.Core.AppSettings.RemoveValue(key);
      Console.WriteLine("删除key='" + key + "'配置信息");
      Console.WriteLine("请检查" + Phenix.Core.AppSettings.ConfigFilename + "配置文件中是否已被删？");
      Console.Write("按回车键继续：");
      Console.ReadLine();
      Console.WriteLine();

      Console.WriteLine("缺省配置文件为：" + Phenix.Core.AppSettings.DefaultConfigFilename);
      Console.WriteLine("读取key='" + key + "'配置信息为：" + Phenix.Core.AppSettings.ReadValue(key));
      Console.WriteLine();
      Console.WriteLine("要重置属性Test值，即重新从配置文件中取值，则需清空其字段值");
      _test = null;
      Console.WriteLine("重置后，属性Test=" + Test + " " + (Test == (bool)Phenix.Core.Reflection.Utilities.ChangeType(Phenix.Core.AppSettings.ReadValue(key), typeof(bool)) ? "ok" : "error"));
      Console.WriteLine();

      Console.WriteLine("结束");
      Console.ReadLine();
    }
  }
}
