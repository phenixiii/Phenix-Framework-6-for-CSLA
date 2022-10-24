using System;

namespace Phenix.Core
{
  /// <summary>
  /// 消息通报类型
  /// </summary>
  [Serializable]
  public enum MessageNotifyType
  {
    /// <summary>
    /// 消息
    /// </summary>
    Information,

    /// <summary>
    /// 注意
    /// </summary>
    Warning,

    /// <summary>
    /// 错误
    /// </summary>
    Error
  }
}
