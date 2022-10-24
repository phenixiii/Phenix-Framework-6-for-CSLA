using System;

namespace Phenix.Core.Cache
{
  /// <summary>
  /// "缓存"标签
  /// </summary>
  [AttributeUsage(AttributeTargets.Class)]
  public sealed class ObjectCacheAttribute : Attribute
  {
    #region 属性

    private int _updateInterval = 3;
    /// <summary>
    /// 更新间隔频率(秒)
    /// 缺省为 3
    /// </summary>
    public int UpdateInterval
    {
      get { return _updateInterval; }
      set { _updateInterval = value; }
    }

    private int _maxCount = 10;
    /// <summary>
    /// 缓存对象数量
    /// 缺省为 10
    /// </summary>
    public int MaxCount
    {
      get { return _maxCount; }
      set { _maxCount = value; }
    }

    private bool _isValid = true;
    /// <summary>
    /// 是否有效
    /// 缺省为 true
    /// </summary>
    public bool IsValid
    {
      get { return _isValid; }
      set { _isValid = value; }
    }

    #endregion
  }
}
