using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Phenix.Test.使用指南._03._5
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
