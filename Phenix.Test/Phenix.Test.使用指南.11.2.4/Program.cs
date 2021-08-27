﻿using System;
using System.Linq;

namespace Phenix.Test.使用指南._11._2._4
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

      Console.WriteLine("**** 测试业务对象自己置换成一个New业务对象功能 ****");
      User user = User.Fetch(UserRoleList.Exists().Where(UserRole.Link<User>()), Phenix.Core.Mapping.OrderByInfo.Descending(User.LoginProperty));
      Console.WriteLine("Fetch拥有角色的最近登录用户对象user：ID=" + user.US_ID + ",Name=" + user.Name);
      long? oldID = user.US_ID;
      string oldName = user.Name;
      user.MarkNew();
      Console.WriteLine("将user置换成一个New用户对象：ID=" + user.US_ID + ",Name=" + user.Name + " " +
        (user.US_ID != oldID && user.Name == oldName ? "ok" : "error"));
      Console.WriteLine("其UserRoles是空的：" + (user.UserRoles.Count == 0) + " " + (user.UserRoles.Count == 0 ? "ok" : "error"));
      Console.WriteLine();

      Console.WriteLine("**** 测试业务对象自己置换成一个New业务对象（含从业务对象）功能 ****");
      user = User.Fetch(UserRoleList.Exists().Where(UserRole.Link<User>()), Phenix.Core.Mapping.OrderByInfo.Descending(User.LoginProperty));
      Console.WriteLine("Fetch拥有角色的最近登录用户对象user：ID=" + user.US_ID + ",Name=" + user.Name);
      user.MarkNewSelfAndUserRoles();
      Console.WriteLine("将user置换成一个New用户对象（含从业务对象）：ID=" + user.US_ID + ",Name=" + user.Name);
      Console.WriteLine("其UserRoles是不空的：" + (user.UserRoles.Count > 0) + " " + (user.UserRoles.Count > 0 ? "ok" : "error"));
      Console.WriteLine("这些UserRoles在业务逻辑上也挂在这个New用户对象下：" + user.UserRoles.All(p => p.UR_US_ID == user.US_ID) + " " + (user.UserRoles.All(p => p.UR_US_ID == user.US_ID) ? "ok" : "error"));
      Console.WriteLine("user的OnInitializeNew()函数是否被调用：" + user.OnInitializeNewByExecute + ' ' + (!user.OnInitializeNewByExecute ? "ok" : "error"));
      Console.WriteLine();

      Console.WriteLine("结束, 与数据库交互细节见日志");
      Console.ReadLine();
    }
  }
}
