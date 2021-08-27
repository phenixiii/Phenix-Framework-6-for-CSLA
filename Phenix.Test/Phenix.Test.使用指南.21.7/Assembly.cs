namespace Phenix.Test.使用指南._21._7.Plugin
{
  [System.Serializable]
  [System.ComponentModel.DisplayName("")]
  public class Assembly : Assembly<Assembly>
  {
    private Assembly()
    {
      //禁止添加代码
    }

    [Newtonsoft.Json.JsonConstructor]
    private Assembly(bool? isNew, bool? isSelfDirty, bool? isSelfDeleted,
      long? AS_ID, string name, string caption)
      : base(isNew, isSelfDirty, isSelfDeleted, AS_ID, name, caption)
    {
    }
  }

  /// <summary>
  /// 清单
  /// </summary>
  [System.Serializable]
  [System.ComponentModel.DisplayName("")]
  public class AssemblyList : Phenix.Business.BusinessListBase<AssemblyList, Assembly>
  {
  }
}
