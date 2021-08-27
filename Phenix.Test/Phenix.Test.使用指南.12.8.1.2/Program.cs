using System;

namespace Phenix.Test.使用指南._12._8._1._2
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

      Console.WriteLine("**** 测试父子继承+主从结构下的Fetch功能 ****");
      AssemblyClass assemblyClass;
      while (true)
      {
        assemblyClass = AssemblyClass.Fetch();
        if (assemblyClass != null)
          break;
        Console.WriteLine("请通过Phenix.Services.Host.x86.exe注册所有相关的业务程序集");
        Console.Write("注册好后请点回车继续：");
        Console.ReadLine();
      }
      Console.WriteLine("Fetch到AssemblyClass对象：" + assemblyClass.Caption);
      Console.WriteLine();

      Console.WriteLine("**** 测试父子继承+主从结构下的Save功能 ****");
      Console.WriteLine();
      Console.WriteLine("开始编辑AssemblyClass对象...");
      assemblyClass.BeginEdit();
      assemblyClass.AssemblyCaption = assemblyClass.AssemblyCaption + '_';
      Console.WriteLine("AssemblyCaption旧值：" + assemblyClass.GetOldValue(AssemblyClass.AssemblyCaptionProperty));
      Console.WriteLine("AssemblyCaption当前值：" + assemblyClass.AssemblyCaption);
      assemblyClass.AssemblyClassCaption = assemblyClass.AssemblyClassCaption + '_';
      Console.WriteLine("AssemblyClassCaption 旧值：" + assemblyClass.GetOldValue(AssemblyClass.AssemblyClassCaptionProperty));
      Console.WriteLine("AssemblyClassCaption 当前值：" + assemblyClass.AssemblyClassCaption);
      assemblyClass.Save();
      Console.WriteLine("保存成功");
      Console.WriteLine("AssemblyCaption旧值：" + assemblyClass.GetOldValue(AssemblyClass.AssemblyCaptionProperty));
      Console.WriteLine("AssemblyCaptione当前值：" + assemblyClass.AssemblyCaption);
      Console.WriteLine("AssemblyClassCaption旧值：" + assemblyClass.GetOldValue(AssemblyClass.AssemblyClassCaptionProperty));
      Console.WriteLine("AssemblyClassCaption当前值：" + assemblyClass.AssemblyClassCaption);
      Console.WriteLine();
      Console.WriteLine("开始恢复AssemblyClass对象...");
      assemblyClass.BeginEdit();
      assemblyClass.AssemblyCaption = assemblyClass.AssemblyCaption.TrimEnd('_');
      Console.WriteLine("AssemblyCaption旧值：" + assemblyClass.GetOldValue(AssemblyClass.AssemblyCaptionProperty));
      Console.WriteLine("AssemblyCaption当前值：" + assemblyClass.AssemblyCaption);
      assemblyClass.AssemblyClassCaption = assemblyClass.AssemblyClassCaption.TrimEnd('_');
      Console.WriteLine("AssemblyClassCaption旧值：" + assemblyClass.GetOldValue(AssemblyClass.AssemblyClassCaptionProperty));
      Console.WriteLine("AssemblyClassCaption当前值：" + assemblyClass.AssemblyClassCaption);
      assemblyClass.Save();
      Console.WriteLine("保存成功");
      Console.WriteLine("AssemblyCaption旧值：" + assemblyClass.GetOldValue(AssemblyClass.AssemblyCaptionProperty));
      Console.WriteLine("AssemblyCaption当前值：" + assemblyClass.AssemblyCaption);
      Console.WriteLine("AssemblyClassCaption旧值：" + assemblyClass.GetOldValue(AssemblyClass.AssemblyClassCaptionProperty));
      Console.WriteLine("AssemblyClassCaption当前值：" + assemblyClass.AssemblyClassCaption);
      Console.WriteLine();

      Console.WriteLine("结束, 与数据库交互细节见日志");
      Console.ReadLine();
    }
  }
}
