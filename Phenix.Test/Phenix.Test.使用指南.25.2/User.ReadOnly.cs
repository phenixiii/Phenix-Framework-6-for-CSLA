﻿namespace Phenix.Test.使用指南._25._2
{
  [System.Serializable]
  [Phenix.Core.Mapping.ReadOnly]
  public class UserReadOnly : User<UserReadOnly>
  {
    private UserReadOnly()
    {
      //禁止添加代码
    }
  }

  /// <summary>
  /// 清单
  /// </summary>
  [System.Serializable]
  public class UserReadOnlyList : Phenix.Business.BusinessListBase<UserReadOnlyList, UserReadOnly>
  {
    private UserReadOnlyList()
    {
      //禁止添加代码
    }
  }
}