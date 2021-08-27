using System;
using Phenix.Core.Mapping;

namespace Phenix.Test.使用指南._12._6._1._3
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

      Console.WriteLine("**** 测试按字段上的FieldOrderByAttribute标签拼装select语句 ****");
      Console.WriteLine("Fetch全景User集合对象");
      UserList users = UserList.Fetch();
      Console.WriteLine("User集合对象中含有业务对象数：" + users.Count);
      foreach (User item in users)
        Console.WriteLine("ID=" + item.US_ID + ",Name=" + item.Name + ",UserNumber=" + item.Usernumber);
      Console.WriteLine("请检查" + Phenix.Core.AppConfig.TempDirectory + "目录下日志记录的select语句中是否含'order by US_USERNUMBER DESC'？");
      Console.Write("按回车键继续：");
      Console.ReadLine();
      Console.WriteLine();

      Console.WriteLine("**** 测试按查询时带入的OrderByInfo及字段上的FieldOrderByAttribute标签拼装select语句 ****");
      Console.WriteLine("Fetch全景User集合对象");
      users = UserList.Fetch(new[] {OrderByInfo.Ascending(User.NameProperty)});
      Console.WriteLine("User集合对象中含有业务对象数：" + users.Count);
      foreach (User item in users)
        Console.WriteLine("ID=" + item.US_ID + ",Name=" + item.Name + ",UserNumber=" + item.Usernumber);
      Console.WriteLine("请检查" + Phenix.Core.AppConfig.TempDirectory + "目录下日志记录的select语句中是否含'order by US_NAME ASC ,US_USERNUMBER DESC '？");
      Console.Write("按回车键继续：");
      Console.ReadLine();
      Console.WriteLine();

      Console.WriteLine("**** 测试维持业务集合对象的排序状态 ****");
      Random random = new Random();
      for (int i = 0; i < 80; i++)
      {
        User user = User.New(random.Next(20).ToString(), random.Next(20).ToString());
        users.Add(user);
        Console.WriteLine("添加: ID=" + user.US_ID + ",Name=" + user.Name + ",UserNumber=" + user.Usernumber);
        for (int j = 0; j < users.Count; j++)
        {
          User item = users[j];
          if (object.ReferenceEquals(item, user))
          {
            if (j - 1 >= 0)
              if (String.CompareOrdinal(users[j - 1].Name, item.Name) > 0 ||
                (String.CompareOrdinal(users[j - 1].Name, item.Name) == 0) && String.CompareOrdinal(users[j - 1].Usernumber, item.Usernumber) < 0)
              {
                item.Tag = true;
                continue;
              }
            if (j + 1 < users.Count)
              if (String.CompareOrdinal(users[j + 1].Name, item.Name) < 0 ||
                (String.CompareOrdinal(users[j + 1].Name, item.Name) == 0) && String.CompareOrdinal(users[j + 1].Usernumber, item.Usernumber) > 0)
              {
                item.Tag = true;
                continue;
              }
          }
        }
      }
      Console.WriteLine("结果：");
      foreach (User item in users)
        Console.WriteLine("ID=" + item.US_ID + ",Name=" + item.Name + ",UserNumber=" + item.Usernumber + (item.Tag ? " error" : ""));
      Console.WriteLine();

      Console.WriteLine("结束, 与数据库交互细节见日志");
      Console.ReadLine();
    }
  }
}
