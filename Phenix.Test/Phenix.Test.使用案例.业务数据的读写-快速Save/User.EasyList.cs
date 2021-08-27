using System;
using Phenix.Core.Data;
using Phenix.Core.Mapping;

/* 
   builder:    phenixiii
   build time: 2017-04-10 14:30:23
   notes:      User
*/

namespace Phenix.Test.使用案例.业务数据的读写.快速Save
{
  /// <summary>
  /// User
  /// </summary>
  [Serializable]
  [System.ComponentModel.DisplayName("User")]
  public class UserEasyList : EntityListBase<UserEasyList, UserEasy>
  {
    /// <summary>
    /// 构建实体
    /// </summary>
    protected override object CreateInstance()
    {
      return new UserEasyList();
    }
  }
}