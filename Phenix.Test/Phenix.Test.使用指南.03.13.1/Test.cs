using System;
using Phenix.Core;

namespace Phenix.Test.使用指南._03._13._1
{
  internal abstract class TestBase<T>
    where T : TestBase<T>
  {
    private static DateTime? _firstDate;
    public static DateTime FirstDate
    {
      get { return AppSettings.GetProperty(typeof(T).FullName, ref _firstDate, DateTime.Now); }
      set { AppSettings.SetProperty(typeof(T).FullName, ref _firstDate, value); }
    }

    public static DateTime LastDate { get; set; }
  }

  internal class TestA : TestBase<TestA>
  {
  }

  internal class TestB : TestBase<TestB>
  {
  }
}
