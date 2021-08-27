using System;

namespace Phenix.Business
{
  /// <summary>
  /// 实体集合基类
  /// </summary>
  [Serializable]
  public abstract class EntityListBase<T, TEntity> : Phenix.Core.Data.EntityListBase<T, TEntity>
    where T : EntityListBase<T, TEntity>
    where TEntity : EntityBase<TEntity>
  {
  }
}
