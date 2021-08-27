using System;

namespace Phenix.Windows
{
  /// <summary>
  /// 数据操作状态
  /// </summary>
  [Serializable]
  public enum DataOperateState
  {
    /// <summary>
    /// 无
    /// </summary>
    None,

    /// <summary>
    /// 检索中
    /// </summary>
    Fetching,

    /// <summary>
    /// 检索中断
    /// </summary>
    FetchSuspend,

    /// <summary>
    /// 检索失败
    /// </summary>
    FetchAborted,

    /// <summary>
    /// 浏览
    /// </summary>
    Browse,

    /// <summary>
    /// 新增
    /// </summary>
    Add,

    /// <summary>
    /// 编辑
    /// </summary>
    Modify,

    /// <summary>
    /// 删除
    /// </summary>
    Delete
  }
}
