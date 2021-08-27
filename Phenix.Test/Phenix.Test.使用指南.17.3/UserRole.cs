using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Phenix.Test.使用指南._17._3
{
  [System.Serializable]
  [System.ComponentModel.DisplayName("")]
  public class UserRole : UserRole<UserRole>
  {
    private UserRole()
    {
      //禁止添加代码
    }

    [Newtonsoft.Json.JsonConstructor]
    private UserRole(bool? isNew, bool? isSelfDirty, bool? isSelfDeleted, long? UR_ID, long? UR_US_ID, long? UR_RL_ID, string inputer, DateTime? inputtime)
      : base(isNew, isSelfDirty, isSelfDeleted, UR_ID, UR_US_ID, UR_RL_ID, inputer, inputtime)
    {
    }
  }

  /// <summary>
  /// 清单
  /// </summary>
  [System.Serializable]
  [System.ComponentModel.DisplayName("")]
  public class UserRoleList : Phenix.Business.BusinessListBase<UserRoleList, UserRole>
  {
  }
}
