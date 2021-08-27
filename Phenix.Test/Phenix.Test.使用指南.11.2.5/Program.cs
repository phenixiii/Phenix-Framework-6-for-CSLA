using System;

namespace Phenix.Test.使用指南._11._2._5
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

      Console.WriteLine("**** 测试树状结构的New功能 ****");
      DepartmentList departments = DepartmentList.New();
      Department department = departments.AddNew();
      Console.WriteLine("AddNew主对象1：ID=" + department.DP_ID); 
      Console.WriteLine();
      Department subDepartment1 = department.SubDepartments.AddNew();
      Console.WriteLine("AddNew下级对象1：ID=" + subDepartment1.DP_ID + ",上级ID=" + subDepartment1.DP_DP_ID + ' ' + (subDepartment1.DP_DP_ID == department.DP_ID ? "ok" : "error"));
      Console.WriteLine();
      Department subDepartment2 = department.SubDepartments.AddNew();
      Console.WriteLine("AddNew下级对象2：ID=" + subDepartment2.DP_ID + ",上级ID=" + subDepartment2.DP_DP_ID + ' ' + (subDepartment2.DP_DP_ID == department.DP_ID ? "ok" : "error"));
      Department subDepartment21 = subDepartment2.SubDepartments.AddNew();
      Console.WriteLine("AddNew下级对象2的下级对象21：ID=" + subDepartment21.DP_ID + ",上级ID=" + subDepartment21.DP_DP_ID + ' ' + (subDepartment21.DP_DP_ID == subDepartment2.DP_ID ? "ok" : "error"));
      Department subDepartment22 = subDepartment2.SubDepartments.AddNew();
      Console.WriteLine("AddNew下级对象2的下级对象22：ID=" + subDepartment22.DP_ID + ",上级ID=" + subDepartment22.DP_DP_ID + ' ' + (subDepartment22.DP_DP_ID == subDepartment2.DP_ID ? "ok" : "error"));
      Console.WriteLine();
      //subDepartment2.DP_DP_ID = subDepartment1.DP_ID;//不建议直接赋外键值
      subDepartment2.LinkTo(subDepartment1); //建议调用明确目的的函数
      Console.WriteLine("将下级对象2挂在下级对象1（ID=" + subDepartment1.DP_ID + "）下");
      Console.WriteLine("则下级对象2成为第三层对象：ID=" + subDepartment2.DP_ID + ",上级ID=" + subDepartment2.DP_DP_ID + ' ' + (subDepartment2.DP_DP_ID == subDepartment1.DP_ID ? "ok" : "error"));
      Console.WriteLine("而下级对象21成为第四层对象：ID=" + subDepartment21.DP_ID + ",上级ID=" + subDepartment21.DP_DP_ID + ' ' + (subDepartment21.DP_DP_ID == subDepartment2.DP_ID ? "ok" : "error"));
      Console.WriteLine("而下级对象22成为第四层对象：ID=" + subDepartment22.DP_ID + ",上级ID=" + subDepartment22.DP_DP_ID + ' ' + (subDepartment22.DP_DP_ID == subDepartment2.DP_ID ? "ok" : "error"));
      Console.WriteLine();

      Console.WriteLine("**** 测试主从结构的New功能 ****");
      Assembly assembly = Assembly.New();
      Console.WriteLine("新增Assembly对象：" + assembly.AS_ID);
      AssemblyClass assemblyClass = assembly.AssemblyClasses.AddNew();
      Console.WriteLine("新增AssemblyClass对象：ID=" + assemblyClass.AC_ID + ",上级ID=" + assemblyClass.AC_AS_ID + ' ' + (assemblyClass.AC_AS_ID == assembly.AS_ID ? "ok" : "error")); ;
      AssemblyClassProperty assemblyClassProperty = assemblyClass.AssemblyClassProperties.AddNew();
      Console.WriteLine("新增AssemblyClassProperty对象：ID=" + assemblyClassProperty.AP_ID + ",上级ID=" + assemblyClassProperty.AP_AC_ID + ' ' + (assemblyClassProperty.AP_AC_ID == assemblyClass.AC_ID ? "ok" : "error")); ;
      Console.WriteLine();

      Console.WriteLine("**** 测试主从结构Clone的New功能 ****");
      assembly.ApplyEdit();
      AssemblyClass clonedAssemblyClass = assemblyClass.Clone(true);
      assembly.AssemblyClasses.Add(clonedAssemblyClass);
      Console.WriteLine("Clone的新增AssemblyClass对象：" + (clonedAssemblyClass.IsNew ? "ok" : "error")); ;
      Console.WriteLine("Clone的新增AssemblyClass对象：ID=" + clonedAssemblyClass.AC_ID + ",上级ID=" + clonedAssemblyClass.AC_AS_ID + ' ' + (clonedAssemblyClass.AC_AS_ID == assembly.AS_ID ? "ok" : "error")); ;
      Console.WriteLine("Clone的新增AssemblyClass对象有新的IdValue：" + (clonedAssemblyClass.IdValue != assemblyClass.IdValue ? "ok" : "error")); ;
      Console.WriteLine("Clone的新增AssemblyClass对象有新的PrimaryKey：" + (clonedAssemblyClass.PrimaryKey != assemblyClass.PrimaryKey ? "ok" : "error")); ;
      Console.WriteLine("Clone的新增AssemblyClass对象也Clone了Detail：" + (clonedAssemblyClass.AssemblyClassProperties.Count == assemblyClass.AssemblyClassProperties.Count ? "ok" : "error")); ;
      Console.WriteLine("Clone的新增AssemblyClass对象Detail上级就是自己：" + (clonedAssemblyClass.AssemblyClassProperties[0].AP_AC_ID == clonedAssemblyClass.AC_ID ? "ok" : "error")); ;
      Console.WriteLine();

      Console.WriteLine("结束, 与数据库交互细节见日志");
      Console.ReadLine();
    }
  }
}
