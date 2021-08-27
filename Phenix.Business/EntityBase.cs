using System;

namespace Phenix.Business
{
  /// <summary>
  /// 实体基类
  /// </summary>
  [Serializable]
  public abstract class EntityBase<T> : Phenix.Core.Data.EntityBase<T>
    where T : EntityBase<T>
  {
    /// <summary>
    /// for CreateInstance
    /// </summary>
    protected EntityBase()
    {
      //禁止添加代码
    }

    /// <summary>
    /// for Newtonsoft.Json.JsonConstructor
    /// </summary>
    protected EntityBase(bool? isNew, bool? isSelfDirty, bool? isSelfDeleted)
      : base(isNew, isSelfDirty, isSelfDeleted) { }
  }
}
