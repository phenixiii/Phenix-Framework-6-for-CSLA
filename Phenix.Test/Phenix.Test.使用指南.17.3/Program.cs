using System;
using System.Collections.Generic;

namespace Phenix.Test.使用指南._17._3
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

      User noneUser;
      User adminUser;

      Console.WriteLine("模拟登陆");
      Phenix.Business.Security.UserPrincipal.User = Phenix.Business.Security.UserPrincipal.CreateTester();

      Phenix.Services.Client.Library.Registration.RegisterEmbeddedWorker(false);
      Console.WriteLine();

      const string test = "test.17.3";
      List<string> userNumbers = new List<string> { Phenix.Core.Security.UserIdentity.AdminUserNumber, test };
      string userNumbersFormat = String.Format("('{0}','{1}')", Phenix.Core.Security.UserIdentity.AdminUserNumber, test);
      List<DateTime> dateTimess = new List<DateTime> { DateTime.Now, DateTime.Today };

      Console.WriteLine("**** 测试登录后Fetch单个业务对象功能 ****");
      long? id = 0;
      noneUser = User.Fetch(p => p.US_ID == id);
      noneUser = User.Fetch(p => p.Usernumber == "");
      Console.WriteLine("Fetch数据库不存在的业务对象应返回null对象" + (noneUser == null ? " ok" : " error"));
      Console.WriteLine();
      adminUser = User.Fetch(p => p.Usernumber == Phenix.Core.Security.UserIdentity.AdminUserNumber);
      Console.WriteLine("Fetch单个AdminUser对象：ID=" + adminUser.US_ID + ",Name=" + adminUser.Name + ",UserNumber=" + adminUser.Usernumber + (adminUser.SelfFetched ? " ok" : " error"));
      Console.WriteLine();

      Console.WriteLine("测试利用排序Fetch单个业务对象功能...");
      User loginUser = User.Fetch(Phenix.Core.Mapping.OrderByInfo.Descending(User.LoginProperty));
      Console.WriteLine("按US_LOGIN字段降序Fetch最近登录用户：ID=" + loginUser.US_ID + ",Name=" + loginUser.Name + ",UserNumber=" + loginUser.Usernumber);
      Console.WriteLine();

      Console.WriteLine("测试利用排序Fetch全景集合对象功能...");
      UserList users1 = UserList.Fetch(Phenix.Core.Mapping.OrderByInfo.Ascending(User.US_IDProperty));
      Console.WriteLine("按US_ID字段升序Fetch全景集合对象，业务对象数：" + users1.Count);
      Console.WriteLine("集合中第一个User对象: ID=" + users1[0].US_ID + ",Name=" + users1[0].Name + ",UserNumber=" + users1[0].Usernumber);
      Console.WriteLine();

      Console.WriteLine("**** 测试Fetch全景集合对象功能 ****");
      UserList users2 = UserList.Fetch();
      Console.WriteLine("Fetch全景集合对象");
      Console.WriteLine("集合中第一个User对象: IdValue=" + users2[0].IdValue + ",ID=" + users2[0].US_ID + ",Name=" + users2[0].Name + ",UserNumber=" + users2[0].Usernumber);
      UserList users3 = UserList.Fetch();
      Console.WriteLine("重新Fetch全景集合对象，默认取自本地缓存");
      Console.WriteLine("集合中第一个User对象: IdValue=" + users3[0].IdValue + ",ID=" + users3[0].US_ID + ",Name=" + users3[0].Name + ",UserNumber=" + users3[0].Usernumber);
      Console.WriteLine("前后两个集合对象是否同一实例: " + (users2 == users3 ? "是" : "否") + ' ' + (users2 != users3 ? "ok" : "error"));
      Console.WriteLine("前后两个集合对象的第一个对象是否同一实例: " + (object.ReferenceEquals(users2[0], users3[0]) ? "是" : "否") + ' ' + (!object.ReferenceEquals(users2[0], users3[0]) ? "ok" : "error"));
      Console.WriteLine("前后两个集合对象的第一个对象的IDValue是否相同: " + (users2[0].IdValue == users3[0].IdValue ? "是" : "否") + ' ' + (users2[0].IdValue != users3[0].IdValue ? "ok" : "error"));
      Console.WriteLine("前后两个集合对象的第一个对象的PrimaryKey是否相同: " + (users2[0].PrimaryKey == users3[0].PrimaryKey ? "是" : "否") + ' ' + (users2[0].PrimaryKey == users3[0].PrimaryKey ? "ok" : "error"));
      Console.WriteLine();

      Console.WriteLine("**** 测试字符串比较条件Fetch集合对象功能 ****");
      UserList users4 = UserList.Fetch(p => userNumbers.Contains(p.Usernumber));
      Console.WriteLine("Fetch用户条件：UserNumbers in " + userNumbersFormat + (users4.Count == 1 ? "ok" : "error"));
      UserList users5 = UserList.Fetch(p => p.Usernumber.Contains(Phenix.Core.Security.UserIdentity.AdminUserNumber));
      Console.WriteLine("Fetch用户条件：UserNumbers like '%ADMIN%' " + (users5.Count >= 1 ? "ok" : "error"));
      UserList users6 = UserList.Fetch(p => p.Usernumber.StartsWith(Phenix.Core.Security.UserIdentity.AdminUserNumber));
      Console.WriteLine("Fetch用户条件：UserNumbers like 'ADMIN%' " + (users6.Count >= 1 ? "ok" : "error"));
      UserList users7 = UserList.Fetch(p => p.Usernumber.EndsWith(Phenix.Core.Security.UserIdentity.AdminUserNumber));
      Console.WriteLine("Fetch用户条件：UserNumbers like '%ADMIN' " + (users7.Count >= 1 ? "ok" : "error"));
      Console.WriteLine();

      Console.WriteLine("**** 测试数值比较条件Fetch集合对象功能 ****");
      const int loginFailureCount = 0;
      UserList users8 = UserList.Fetch(p => p.Usernumber == Phenix.Core.Security.UserIdentity.AdminUserNumber && (p.Loginfailurecount == loginFailureCount || p.Loginfailurecount >= loginFailureCount + 1));
      Console.WriteLine("Fetch用户条件：UserNumbers == 'ADMIN' && (Loginfailurecount = 0 || Loginfailurecount > 0 + 1) " + (users8.Count == 1 ? "ok" : "error"));
      Console.WriteLine();

      Console.WriteLine("**** 测试日期比较条件Fetch集合对象功能 ****");
      UserList users9 = UserList.Fetch(p => p.Usernumber == Phenix.Core.Security.UserIdentity.AdminUserNumber && (p.Login == null || p.Login <= DateTime.Now.AddYears(1)));
      Console.WriteLine("Fetch用户条件：UserNumbers == 'ADMIN' && (Login == null || Login <=  DateTime.Now.AddYears(1)) " + (users9.Count == 1 ? "ok" : "error"));
      UserList users10 = UserList.Fetch(p => dateTimess.Contains(p.Passwordchangedtime.Value));
      Console.WriteLine(String.Format("Fetch用户条件：to_char( US_PASSWORDCHANGEDTIME ,'yyyymmddhh24miss') in ('{0}','{1}')", dateTimess[0].ToString("yyyyMMddHHmmss"), dateTimess[1].ToString("yyyyMMddHHmmss")));
      Console.WriteLine();

      Console.WriteLine("**********************************************");
      Console.WriteLine("**** 以下是同样查询条件下测试Entity的功能 ****");
      Console.WriteLine();

      Console.WriteLine("**** 测试Fetch单个实体功能 ****");
      UserEasy adminUserEasy = UserEasy.Fetch(p => p.Usernumber == Phenix.Core.Security.UserIdentity.AdminUserNumber);
      Console.WriteLine("Fetch单个AdminUserEasy对象：ID=" + adminUserEasy.US_ID + ",Name=" + adminUserEasy.Name + ",UserNumber=" + adminUserEasy.Usernumber);
      Console.WriteLine();

      Console.WriteLine("**** 测试利用排序Fetch单个实体功能 ****");
      UserEasy loginUserEasy = UserEasy.Fetch(Phenix.Core.Mapping.OrderByInfo.Descending(UserEasy.LoginProperty));
      Console.WriteLine("按US_LOGIN字段降序Fetch最近登录用户：ID=" + loginUserEasy.US_ID + ",Name=" + loginUserEasy.Name + ",UserNumber=" + loginUserEasy.Usernumber);
      Console.WriteLine();

      Console.WriteLine("**** 测试利用排序Fetch全景集合对象功能 ****");
      UserEasyList userEasyList1 = UserEasyList.Fetch(Phenix.Core.Mapping.OrderByInfo.Ascending(UserEasy.US_IDProperty));
      Console.WriteLine("按US_ID字段升序Fetch全景集合对象，实体数：" + userEasyList1.Count);
      Console.WriteLine("集合中第一个User对象: ID=" + userEasyList1[0].US_ID + ",Name=" + userEasyList1[0].Name + ",UserNumber=" + userEasyList1[0].Usernumber);
      Console.WriteLine();

      Console.WriteLine("**** 测试Fetch全景集合对象功能 ****");
      UserEasyList userEasyList2 = UserEasyList.Fetch();
      Console.WriteLine("Fetch全景集合对象");
      Console.WriteLine("集合中第一个UserEasy对象: ID=" + userEasyList2[0].US_ID + ",Name=" + userEasyList2[0].Name + ",UserNumber=" + userEasyList2[0].Usernumber);
      UserEasyList userEasyList3 = UserEasyList.Fetch();
      Console.WriteLine("重新Fetch全景集合对象，仍然取自数据库");
      Console.WriteLine("集合中第一个UserEasy对象: ID=" + userEasyList3[0].US_ID + ",Name=" + userEasyList3[0].Name + ",UserNumber=" + userEasyList3[0].Usernumber);
      Console.WriteLine("前后两个集合对象是否相同: " + (userEasyList2 == userEasyList3 ? "是" : "否") + ' ' + (userEasyList2 != userEasyList3 ? "ok" : "error"));
      Console.WriteLine("前后两个集合对象的第一个对象是否相同: " + (userEasyList2[0] == userEasyList3[0] ? "是" : "否") + ' ' + (userEasyList2[0] != userEasyList3[0] ? "ok" : "error"));
      Console.WriteLine();

      Console.WriteLine("**** 测试字符串比较条件Fetch集合对象功能 ****");
      UserEasyList userEasyList4 = UserEasyList.Fetch(p => userNumbers.Contains(p.Usernumber));
      Console.WriteLine("Fetch用户条件：UserNumbers in " + userNumbersFormat + (userEasyList4.Count == 1 ? "ok" : "error"));
      UserEasyList userEasyList5 = UserEasyList.Fetch(p => p.Usernumber.Contains(Phenix.Core.Security.UserIdentity.AdminUserNumber));
      Console.WriteLine("Fetch用户条件：UserNumbers like '%ADMIN%' " + (userEasyList5.Count >= 1 ? "ok" : "error"));
      UserEasyList userEasyList6 = UserEasyList.Fetch(p => p.Usernumber.StartsWith(Phenix.Core.Security.UserIdentity.AdminUserNumber));
      Console.WriteLine("Fetch用户条件：UserNumbers like 'ADMIN%' " + (userEasyList6.Count >= 1 ? "ok" : "error"));
      UserEasyList userEasyList7 = UserEasyList.Fetch(p => p.Usernumber.EndsWith(Phenix.Core.Security.UserIdentity.AdminUserNumber));
      Console.WriteLine("Fetch用户条件：UserNumbers like '%ADMIN' " + (userEasyList7.Count >= 1 ? "ok" : "error"));
      Console.WriteLine();

      Console.WriteLine("**** 测试数值比较条件Fetch集合对象功能 ****");
      UserEasyList userEasyList8 = UserEasyList.Fetch(p => p.Usernumber == Phenix.Core.Security.UserIdentity.AdminUserNumber && (p.Loginfailurecount == loginFailureCount || p.Loginfailurecount >= loginFailureCount + 1));
      Console.WriteLine("Fetch用户条件：UserNumbers == 'ADMIN' && (Loginfailurecount = 0 || Loginfailurecount > 0 + 1) " + (userEasyList8.Count == 1 ? "ok" : "error"));
      Console.WriteLine();

      Console.WriteLine("**** 测试日期比较条件Fetch集合对象功能 ****");
      UserEasyList userEasyList9 = UserEasyList.Fetch(p => p.Usernumber == Phenix.Core.Security.UserIdentity.AdminUserNumber && (p.Login == null || p.Login <= DateTime.Now.AddYears(1)));
      Console.WriteLine("Fetch用户条件：UserNumbers == 'ADMIN' && (Login == null || Login <=  DateTime.Now.AddYears(1)) " + (userEasyList9.Count == 1 ? "ok" : "error"));
      UserEasyList userEasyList10 = UserEasyList.Fetch(p => dateTimess.Contains(p.Passwordchangedtime.Value));
      Console.WriteLine(String.Format("Fetch用户条件：to_char( US_PASSWORDCHANGEDTIME ,'yyyymmddhh24miss') in ('{0}','{1}')", dateTimess[0].ToString("yyyyMMddHHmmss"), dateTimess[1].ToString("yyyyMMddHHmmss")));
      Console.WriteLine();

      Console.WriteLine("结束, 与数据库交互细节见日志");
      Console.ReadLine();
    }
  }
}
