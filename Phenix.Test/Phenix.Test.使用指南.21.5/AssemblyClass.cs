using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Phenix.Test.使用指南._21._5.Business
{
  [System.Serializable]
  [System.ComponentModel.DisplayName("")]
  public class AssemblyClass : AssemblyClass<AssemblyClass>
  {
    private AssemblyClass()
    {
      //禁止添加代码
    }

    [Newtonsoft.Json.JsonConstructor]
    private AssemblyClass(bool? isNew, bool? isSelfDirty, bool? isSelfDeleted, long? AC_ID, long? AC_AS_ID, string name, string caption, int? captionconfigured, string datasourcekey, string tablename, string fetchscript, int? permanentexecuteaction, int? permanentexecuteconfigured, int? type, int? authorised)
      : base(isNew, isSelfDirty, isSelfDeleted, AC_ID, AC_AS_ID, name, caption, captionconfigured)
    {
    }
  }

  /// <summary>
  /// 清单
  /// </summary>
  [System.Serializable]
  [System.ComponentModel.DisplayName("")]
  public class AssemblyClassList : Phenix.Business.BusinessListBase<AssemblyClassList, AssemblyClass>
  {
  }
}
