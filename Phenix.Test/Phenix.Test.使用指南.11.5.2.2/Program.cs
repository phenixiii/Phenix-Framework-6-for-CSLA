using System;

namespace Phenix.Test.使用指南._11._5._2._2
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

      const string test = "test.11.5.2.2";

      Console.WriteLine("**** 测试Fetch普通业务对象的IsDirtyProperty功能 ****");
      User user = User.Fetch(p => p.Usernumber != test, Phenix.Core.Mapping.OrderByInfo.Descending(User.LoginProperty));
      Console.WriteLine("Fetch最近登录用户User对象：UserNumber=" + user.Usernumber);
      Console.WriteLine("User是否属性值被赋值过(如果写入时的新值与旧值相同则认为未被赋值过): " + (user.PropertyValueChanged ? "是" : "否") + ' ' + (!user.PropertyValueChanged ? "ok" : "error"));
      Console.WriteLine("User是否脏对象: " + (user.IsSelfDirty ? "是" : "否") + ' ' + (!user.IsSelfDirty ? "ok" : "error"));
      Console.WriteLine("UserNumber是否脏属性(如果写入时的新值与旧值相同则认为未被赋值过): " + (user.IsDirtyProperty(User.UsernumberProperty) ? "是" : "否") + ' ' + (!user.IsDirtyProperty(User.UsernumberProperty) ? "ok" : "error"));
      Console.WriteLine("UserNumber是否脏属性(忽略比较新旧值(仅判断是否被赋值过)): " + (user.IsDirtyProperty(User.UsernumberProperty, true) ? "是" : "否") + ' ' + (!user.IsDirtyProperty(User.UsernumberProperty, true) ? "ok" : "error"));
      user.Usernumber = user.Usernumber;
      Console.WriteLine("设置属性UserNumber=" + user.Usernumber);
      Console.WriteLine("User是否属性值被赋值过(如果写入时的新值与旧值相同则认为未被赋值过): " + (user.PropertyValueChanged ? "是" : "否") + ' ' + (!user.PropertyValueChanged ? "ok" : "error"));
      Console.WriteLine("User是否脏对象: " + (user.IsSelfDirty ? "是" : "否") + ' ' + (!user.IsSelfDirty ? "ok" : "error"));
      Console.WriteLine("UserNumber是否脏属性(如果写入时的新值与旧值相同则认为未被赋值过): " + (user.IsDirtyProperty(User.UsernumberProperty) ? "是" : "否") + ' ' + (!user.IsDirtyProperty(User.UsernumberProperty) ? "ok" : "error"));
      Console.WriteLine("UserNumber是否脏属性(忽略比较新旧值(仅判断是否被赋值过)): " + (user.IsDirtyProperty(User.UsernumberProperty, true) ? "是" : "否") + ' ' + (!user.IsDirtyProperty(User.UsernumberProperty, true) ? "ok" : "error"));
      Console.WriteLine();
      user.Usernumber = test;
      Console.WriteLine("设置属性UserNumber=" + user.Usernumber);
      Console.WriteLine("User是否属性值被赋值过(如果写入时的新值与旧值相同则认为未被赋值过): " + (user.PropertyValueChanged ? "是" : "否") + ' ' + (user.PropertyValueChanged ? "ok" : "error"));
      Console.WriteLine("User是否脏对象: " + (user.IsSelfDirty ? "是" : "否") + ' ' + (user.IsSelfDirty ? "ok" : "error"));
      Console.WriteLine("UserNumber是否脏属性(如果写入时的新值与旧值相同则认为未被赋值过): " + (user.IsDirtyProperty(User.UsernumberProperty) ? "是" : "否") + ' ' + (user.IsDirtyProperty(User.UsernumberProperty) ? "ok" : "error"));
      Console.WriteLine("UserNumber是否脏属性(忽略比较新旧值(仅判断是否被赋值过)): " + (user.IsDirtyProperty(User.UsernumberProperty, true) ? "是" : "否") + ' ' + (user.IsDirtyProperty(User.UsernumberProperty, true) ? "ok" : "error"));
      Console.WriteLine();

      Console.WriteLine("**** 测试New普通业务对象的IsDirtyProperty功能 ****");
      user = User.New();
      Console.WriteLine("新增用户User对象：UserNumber=" + user.Usernumber);
      Console.WriteLine("User是否属性值被赋值过(如果写入时的新值与旧值相同则认为未被赋值过): " + (user.PropertyValueChanged ? "是" : "否") + ' ' + (!user.PropertyValueChanged ? "ok" : "error"));
      Console.WriteLine("User是否脏对象: " + (user.IsSelfDirty ? "是" : "否") + ' ' + (user.IsSelfDirty ? "ok" : "error"));
      Console.WriteLine("UserNumber是否脏属性(如果写入时的新值与旧值相同则认为未被赋值过): " + (user.IsDirtyProperty(User.UsernumberProperty) ? "是" : "否") + ' ' + (user.IsDirtyProperty(User.UsernumberProperty) ? "ok" : "error"));
      Console.WriteLine("UserNumber是否脏属性(忽略比较新旧值(仅判断是否被赋值过)): " + (user.IsDirtyProperty(User.UsernumberProperty, true) ? "是" : "否") + ' ' + (!user.IsDirtyProperty(User.UsernumberProperty, true) ? "ok" : "error"));
      Console.WriteLine();

      Console.WriteLine("**** 测试Fetch映射视图业务对象的IsDirtyProperty功能 ****");
      Console.WriteLine("Fetch...");
      UserRoleInfo userRoleInfo;
      while (true)
      {
        userRoleInfo = UserRoleInfo.Fetch(p => p.Usernumber != test);
        if (userRoleInfo != null)
          break;
        Console.WriteLine("请使用Phenix.Security.Windows.UserManage、Phenix.Security.Windows.RoleManage功能新增用户和角色信息");
        Console.WriteLine("如果仍然Fetch不到数据，请通过Phenix.Services.Host.x86.exe注册所有相关的业务程序集");
        Console.Write("注册好后请点回车继续：");
        Console.ReadLine();
      }
      Console.WriteLine("Fetch到UserRoleInfo对象：" + userRoleInfo.Caption);
      Console.WriteLine("UserRoleInfo是否属性值被赋值过(如果写入时的新值与旧值相同则认为未被赋值过): " + (userRoleInfo.PropertyValueChanged ? "是" : "否") + ' ' + (!userRoleInfo.PropertyValueChanged ? "ok" : "error"));
      Console.WriteLine("UserRoleInfo是否脏对象: " + (userRoleInfo.IsSelfDirty ? "是" : "否") + ' ' + (!userRoleInfo.IsSelfDirty ? "ok" : "error"));
      Console.WriteLine("UserNumber是否脏属性(如果写入时的新值与旧值相同则认为未被赋值过): " + (userRoleInfo.IsDirtyProperty(UserRoleInfo.UsernumberProperty) ? "是" : "否") + ' ' + (!userRoleInfo.IsDirtyProperty(UserRoleInfo.UsernumberProperty) ? "ok" : "error"));
      Console.WriteLine("UserNumber是否脏属性(忽略比较新旧值(仅判断是否被赋值过)): " + (userRoleInfo.IsDirtyProperty(UserRoleInfo.UsernumberProperty, true) ? "是" : "否") + ' ' + (!userRoleInfo.IsDirtyProperty(UserRoleInfo.UsernumberProperty, true) ? "ok" : "error"));
      userRoleInfo.Usernumber = userRoleInfo.Usernumber;
      Console.WriteLine("设置属性UserNumber=" + userRoleInfo.Usernumber);
      Console.WriteLine("UserRoleInfo是否属性值被赋值过(如果写入时的新值与旧值相同则认为未被赋值过): " + (userRoleInfo.PropertyValueChanged ? "是" : "否") + ' ' + (!userRoleInfo.PropertyValueChanged ? "ok" : "error"));
      Console.WriteLine("UserRoleInfo是否脏对象: " + (userRoleInfo.IsSelfDirty ? "是" : "否") + ' ' + (!userRoleInfo.IsSelfDirty ? "ok" : "error"));
      Console.WriteLine("UserNumber是否脏属性(如果写入时的新值与旧值相同则认为未被赋值过): " + (userRoleInfo.IsDirtyProperty(User.UsernumberProperty) ? "是" : "否") + ' ' + (!userRoleInfo.IsDirtyProperty(User.UsernumberProperty) ? "ok" : "error"));
      Console.WriteLine("UserNumber是否脏属性(忽略比较新旧值(仅判断是否被赋值过)): " + (userRoleInfo.IsDirtyProperty(User.UsernumberProperty, true) ? "是" : "否") + ' ' + (!userRoleInfo.IsDirtyProperty(User.UsernumberProperty, true) ? "ok" : "error"));
      Console.WriteLine();
      userRoleInfo.Usernumber = test;
      Console.WriteLine("设置属性UserNumber=" + userRoleInfo.Usernumber);
      Console.WriteLine("UserRoleInfo是否属性值被赋值过(如果写入时的新值与旧值相同则认为未被赋值过): " + (userRoleInfo.PropertyValueChanged ? "是" : "否") + ' ' + (userRoleInfo.PropertyValueChanged ? "ok" : "error"));
      Console.WriteLine("UserRoleInfo是否脏对象: " + (userRoleInfo.IsSelfDirty ? "是" : "否") + ' ' + (userRoleInfo.IsSelfDirty ? "ok" : "error"));
      Console.WriteLine("UserNumber是否脏属性(如果写入时的新值与旧值相同则认为未被赋值过): " + (userRoleInfo.IsDirtyProperty(UserRoleInfo.UsernumberProperty) ? "是" : "否") + ' ' + (userRoleInfo.IsDirtyProperty(UserRoleInfo.UsernumberProperty) ? "ok" : "error"));
      Console.WriteLine("UserNumber是否脏属性(忽略比较新旧值(仅判断是否被赋值过)): " + (userRoleInfo.IsDirtyProperty(UserRoleInfo.UsernumberProperty, true) ? "是" : "否") + ' ' + (userRoleInfo.IsDirtyProperty(UserRoleInfo.UsernumberProperty, true) ? "ok" : "error"));
      Console.WriteLine();

      Console.WriteLine("**** 测试New映射视图业务对象的IsDirtyProperty功能 ****");
      userRoleInfo = UserRoleInfo.New();
      Console.WriteLine("新增用户User对象：" + userRoleInfo.Caption);
      Console.WriteLine("UserRoleInfo是否属性值被赋值过(如果写入时的新值与旧值相同则认为未被赋值过): " + (userRoleInfo.PropertyValueChanged ? "是" : "否") + ' ' + (!userRoleInfo.PropertyValueChanged ? "ok" : "error"));
      Console.WriteLine("UserRoleInfo是否脏对象: " + (userRoleInfo.IsSelfDirty ? "是" : "否") + ' ' + (userRoleInfo.IsSelfDirty ? "ok" : "error"));
      Console.WriteLine("UserNumber是否脏属性(如果写入时的新值与旧值相同则认为未被赋值过): " + (userRoleInfo.IsDirtyProperty(UserRoleInfo.UsernumberProperty) ? "是" : "否") + ' ' + (userRoleInfo.IsDirtyProperty(UserRoleInfo.UsernumberProperty) ? "ok" : "error"));
      Console.WriteLine("UserNumber是否脏属性(忽略比较新旧值(仅判断是否被赋值过)): " + (userRoleInfo.IsDirtyProperty(UserRoleInfo.UsernumberProperty, true) ? "是" : "否") + ' ' + (!userRoleInfo.IsDirtyProperty(UserRoleInfo.UsernumberProperty, true) ? "ok" : "error"));
      userRoleInfo.Usernumber = test;
      Console.WriteLine("设置属性UserNumber=" + userRoleInfo.Usernumber);
      Console.WriteLine("UserRoleInfo是否属性值被赋值过(如果写入时的新值与旧值相同则认为未被赋值过): " + (userRoleInfo.PropertyValueChanged ? "是" : "否") + ' ' + (userRoleInfo.PropertyValueChanged ? "ok" : "error"));
      Console.WriteLine("UserRoleInfo是否脏对象: " + (userRoleInfo.IsSelfDirty ? "是" : "否") + ' ' + (userRoleInfo.IsSelfDirty ? "ok" : "error"));
      Console.WriteLine("UserNumber是否脏属性(如果写入时的新值与旧值相同则认为未被赋值过): " + (userRoleInfo.IsDirtyProperty(UserRoleInfo.UsernumberProperty) ? "是" : "否") + ' ' + (userRoleInfo.IsDirtyProperty(UserRoleInfo.UsernumberProperty) ? "ok" : "error"));
      Console.WriteLine("UserNumber是否脏属性(忽略比较新旧值(仅判断是否被赋值过)): " + (userRoleInfo.IsDirtyProperty(UserRoleInfo.UsernumberProperty, true) ? "是" : "否") + ' ' + (userRoleInfo.IsDirtyProperty(UserRoleInfo.UsernumberProperty, true) ? "ok" : "error"));
      Console.WriteLine();

      Console.WriteLine("结束, 与数据库交互细节见日志");
      Console.ReadLine();
    }
  }
}
