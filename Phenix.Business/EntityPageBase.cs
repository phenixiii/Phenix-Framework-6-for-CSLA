using System;

namespace Phenix.Business
{
  /// <summary>
  /// 实体分页基类
  /// </summary>
  [Serializable]
  public abstract class EntityPageBase<T> : Phenix.Core.Data.EntityPageBase<T>
    where T : EntityPageBase<T>
  {
    /// <summary>
    /// for CreateInstance
    /// </summary>
    protected EntityPageBase()
    {
      //禁止添加代码
    }

    /// <summary>
    /// for Newtonsoft.Json.JsonConstructor
    /// </summary>
    protected EntityPageBase(bool? isNew, bool? isSelfDirty, bool? isSelfDeleted)
      : base(isNew, isSelfDirty, isSelfDeleted) { }
  }
}
