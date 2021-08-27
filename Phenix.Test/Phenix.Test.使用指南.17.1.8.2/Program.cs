using System;

namespace Phenix.Test.使用指南._17._1._8._2
{
  class Program
  {
    private static void Main(string[] args)
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

      Console.WriteLine("**** 测试通过申明关联关系嵌入子查询条件 ****");
      Assembly assembly = Assembly.Fetch(
        new AssemblyCriteria()
        {
          Name = "程序集名称",
          AssemblyClassCriteria_Name = "类名",
          AssemblyClassCriteria_AssemblyClassPropertyCriteria_Name = "属性名",
          AssemblyClassCriteria_AssemblyClassMethodCriteria_Name = "第一个方法名",
          AssemblyClassCriteria_AssemblyClassMethodCriteria_Name2 = "第二个方法名",
        });
      Console.WriteLine("Fetch单个Assembly对象: " + (assembly != null ? assembly.Name : "无"));
      Console.WriteLine();

      Console.WriteLine("结束, 与数据库交互细节见日志");
      Console.ReadLine();
    }
  }
}
