using System;

namespace Phenix.Test.使用指南._11._5._1._1
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

      Console.WriteLine("**** 测试回滚机制功能 ****");
      Console.WriteLine();
      Assembly assembly;
      while (true)
      {
        //assembly = Assembly.Fetch(AssemblyClassList.Exists(AssemblyClassPropertyList.Exists().Where(AssemblyClassProperty.AP_AC_IDProperty == AssemblyClass.AC_IDProperty)).Where(AssemblyClass.AC_AS_IDProperty == Assembly.AS_IDProperty));
        assembly = Assembly.Fetch(AssemblyClassList.Exists(AssemblyClassPropertyList.Exists().Where(AssemblyClassProperty.Link<AssemblyClass>())).Where(AssemblyClass.Link<Assembly>()));
        if (assembly != null)
          break;
        Console.WriteLine("请通过Phenix.Services.Host.x86.exe注册所有相关的业务程序集");
        Console.Write("注册好后请点回车继续：");
        Console.ReadLine();
      }
      Console.WriteLine("Fetch一个含有类、类属性的程序集对象：" + assembly.Caption);
      Console.WriteLine();

      Console.WriteLine("**** 测试主从结构里从下到上逐层删除，然后主对象做回滚的效果 ****");
      assembly.BeginEdit();
      int assemblyClassesCount = assembly.AssemblyClasses.Count;
      Console.WriteLine("GetCompositionDetail类集合，数量=" + assemblyClassesCount + ' ' + (assembly.AssemblyClasses.Count > 0 ? "ok" : "error"));
      AssemblyClass assemblyClass = assembly.AssemblyClasses[0];
      Console.WriteLine("GetCompositionDetail类对象：" + assemblyClass.Caption);
      Console.WriteLine("GetCompositionDetail类属性集合，数量=" + assemblyClass.AssemblyClassProperties.Count + ' ' + (assemblyClass.AssemblyClassProperties.Count > 0 ? "ok" : "error"));
      assemblyClass.AssemblyClassProperties.Clear();
      Console.WriteLine("删除全部类属性，剩余数量=" + assemblyClass.AssemblyClassProperties.Count + ' ' + (assemblyClass.AssemblyClassProperties.Count == 0 ? "ok" : "error"));
      assemblyClass.Delete();
      Console.WriteLine("删除类对象，删除标志=" + assemblyClass.IsDeleted + ' ' + (assemblyClass.IsDeleted ? "ok" : "error"));
      Console.WriteLine("类集合，数量=" + assembly.AssemblyClasses.Count + ' ' + (assembly.AssemblyClasses.Count < assemblyClassesCount ? "ok" : "error"));
      assembly.Delete();
      Console.WriteLine("删除程序集对象，删除标志=" + assembly.IsDeleted + ' ' + (assembly.IsDeleted ? "ok" : "error"));
      assembly.CancelEdit();
      Console.WriteLine("恢复程序集对象，删除标志=" + assembly.IsDeleted + ' ' + (!assembly.IsDeleted ? "ok" : "error"));
      Console.WriteLine("类对象，删除标志= " + assemblyClass.IsDeleted + ' ' + (!assemblyClass.IsDeleted ? "ok" : "error"));
      Console.WriteLine("类集合，数量=" + assembly.AssemblyClasses.Count + ' ' + (assembly.AssemblyClasses.Count == assemblyClassesCount ? "ok" : "error"));
      Console.WriteLine("类属性集合，数量=" + assemblyClass.AssemblyClassProperties.Count + ' ' + (assemblyClass.AssemblyClassProperties.Count > 0 ? "ok" : "error"));
      Console.WriteLine();

      Console.WriteLine("最后，观察下BeginEdit()业务对象会带来多少内存增幅...");
      GC.Collect();
      GC.WaitForFullGCComplete();
      float startTotalMemory = GC.GetTotalMemory(true);
      assembly.BeginEdit();
      GC.Collect();
      GC.WaitForFullGCComplete();
      float endTotalMemory = GC.GetTotalMemory(true);
      Console.WriteLine("BeginEdit()后，内存在事后:事前的大小之比为{0}", endTotalMemory / startTotalMemory);
      GC.Collect();
      GC.WaitForFullGCComplete();
      assembly.CancelEdit(); ;
      GC.Collect();
      GC.WaitForFullGCComplete();
      endTotalMemory = GC.GetTotalMemory(true);
      Console.WriteLine("CancelEdit()后，内存在事后:事前的大小之比为{0}", endTotalMemory / startTotalMemory);
      Console.WriteLine();
      Console.WriteLine("建议在处理大批量数据的业务场景下，尽可能少用BeginEdit()方法。");
      Console.WriteLine("如果是不用编辑的字段，应该打上[Csla.NotUndoable]标记。");
      Console.WriteLine();

      Console.WriteLine("结束, 与数据库交互细节见日志");
      Console.ReadLine();
    }
  }
}
