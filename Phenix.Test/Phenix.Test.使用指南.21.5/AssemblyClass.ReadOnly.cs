using System;

/* 
   builder:    phenixiii
   build time: 2016-05-26 14:40:53
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
  public class AssemblyClassReadOnly : AssemblyClass<AssemblyClassReadOnly>
  {
    private AssemblyClassReadOnly()
    {
      //禁止添加代码
    }

    [Newtonsoft.Json.JsonConstructor]
    private AssemblyClassReadOnly(bool? isNew, bool? isSelfDirty, bool? isSelfDeleted, long? AC_ID, long? AC_AS_ID, string name, string caption, int? captionconfigured, string datasourcekey, string tablename, string fetchscript, int? permanentexecuteaction, int? permanentexecuteconfigured, int? type, int? authorised)
      : base(isNew, isSelfDirty, isSelfDeleted, AC_ID, AC_AS_ID, name, caption, captionconfigured)
    {
    }
  }

  /// <summary>
  /// 清单
  /// </summary>
  [System.Serializable]
  [System.ComponentModel.DisplayName("")]
  public class AssemblyClassReadOnlyList : Phenix.Business.BusinessListBase<AssemblyClassReadOnlyList, AssemblyClassReadOnly>
  {
  }
}