using System;

/* 
   builder:    phenixiii
   build time: 2015-09-29 09:50:10
   notes:      
*/

namespace Phenix.Test.使用指南._21._5.Business
{
  /// <summary>
  /// 
  /// </summary>
  [System.Serializable]
  [System.ComponentModel.DisplayName("")]
  [Phenix.Core.Mapping.ReadOnly]
  public class AssemblyReadOnly : Assembly<AssemblyReadOnly>
  {
    private AssemblyReadOnly()
    {
      //禁止添加代码
    }

    [Newtonsoft.Json.JsonConstructor]
    private AssemblyReadOnly(bool? isNew, bool? isSelfDirty, bool? isSelfDeleted,
      long? AS_ID, string name, string caption)
      : base(isNew, isSelfDirty, isSelfDeleted, AS_ID, name, caption) { }
  }

  /// <summary>
  /// 清单
  /// </summary>
  [System.Serializable]
  [System.ComponentModel.DisplayName("")]
  public class AssemblyReadOnlyList : Phenix.Business.BusinessListBase<AssemblyReadOnlyList, AssemblyReadOnly>
  {
  }
}