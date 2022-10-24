using System;
using System.Collections.Generic;

namespace Phenix.Core.Cache
{
  /// <summary>
  /// 对象缓存同步器接口
  /// </summary>
  public interface IObjectCacheSynchro
  {
    #region 方法

    /// <summary>
    /// 获取活动时间
    /// </summary>
    DateTime? GetActionTime(string typeName);

    /// <summary>
    /// 清除全部缓存
    /// </summary>
    void ClearAll();

    /// <summary>
    /// 清除某类的缓存
    /// </summary>
    void Clear(IList<string> typeNames);

    /// <summary>
    /// 声明某表记录发生更改
    /// </summary>
    void RecordHasChanged(string tableName);

    #endregion
  }
}
