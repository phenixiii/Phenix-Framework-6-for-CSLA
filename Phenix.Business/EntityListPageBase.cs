using System;

namespace Phenix.Business
{
  /// <summary>
  /// 实体集合分页基类
  /// </summary>
  [Serializable]
  public abstract class EntityListPageBase<T, TEntity> : Phenix.Core.Data.EntityListPageBase<T, TEntity>
    where T : EntityListPageBase<T, TEntity>
    where TEntity : EntityPageBase<TEntity>
  {
  }
}
