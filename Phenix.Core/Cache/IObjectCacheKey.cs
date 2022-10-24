namespace Phenix.Core.Cache
{
  /// <summary>
  /// 对象缓存键值
  /// </summary>
  public interface IObjectCacheKey
  {
    #region 属性
    
    /// <summary>
    /// 可以缓存对象?
    /// </summary>
    bool CacheEnabled { get; }

    /// <summary>
    /// 可以极端方式缓存对象?
    /// </summary>
    bool ExtremelyCacheEnabled { get; }

    #endregion

    #region 方法

    /// <summary>
    /// 比较对象
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1720:AvoidTypeNamesInParameters")]
    bool Equals(object obj);

    #endregion
  }
}
