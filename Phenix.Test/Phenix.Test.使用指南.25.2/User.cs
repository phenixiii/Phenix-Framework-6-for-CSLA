﻿namespace Phenix.Test.使用指南._25._2
{
  [System.Serializable]
  public class User : User<User>
  {
    private User()
    {
      //禁止添加代码
    }
  }

  /// <summary>
  /// 清单
  /// </summary>
  [System.Serializable]
  public class UserList : Phenix.Business.BusinessListBase<UserList, User>
  {
    private UserList()
    {
      //禁止添加代码
    }
  }
}