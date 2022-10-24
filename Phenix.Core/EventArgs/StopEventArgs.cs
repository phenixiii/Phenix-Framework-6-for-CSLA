using System;

namespace Phenix.Core
{
  /// <summary>
  /// 阻塞事件数据
  /// </summary>
  [Serializable]
  public class StopEventArgs : EventArgs
  {
    #region 属性

    /// <summary>
    /// 是否终止
    /// 依此判断是否需要后续的处理过程
    /// 缺省为 false
    /// </summary>
    public bool Stop { get; set; }

    #endregion
  }
}