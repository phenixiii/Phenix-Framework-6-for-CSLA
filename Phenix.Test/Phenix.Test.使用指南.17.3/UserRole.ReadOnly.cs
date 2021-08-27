using System;

/* 
   builder:    phenixiii
   build time: 2017-06-04 10:04:01
   notes:      
*/

namespace Phenix.Test.使用指南._17._3
{
  /// <summary>
  /// 
  /// </summary>
  [System.Serializable]
  [System.ComponentModel.DisplayName("")]
  [Phenix.Core.Mapping.ReadOnly]
  public class UserRoleReadOnly : UserRole<UserRoleReadOnly>
  {
    private UserRoleReadOnly()
    {
      //禁止添加代码
    }

    [Newtonsoft.Json.JsonConstructor]
    private UserRoleReadOnly(bool? isNew, bool? isSelfDirty, bool? isSelfDeleted, long? UR_ID, long? UR_US_ID, long? UR_RL_ID, string inputer, DateTime? inputtime)
      : base(isNew, isSelfDirty, isSelfDeleted, UR_ID, UR_US_ID, UR_RL_ID, inputer, inputtime)
    {
    }
  }

  /// <summary>
  /// 清单
  /// </summary>
  [System.Serializable]
  [System.ComponentModel.DisplayName("")]
  public class UserRoleReadOnlyList : Phenix.Business.BusinessListBase<UserRoleReadOnlyList, UserRoleReadOnly>
  {
  }
}