using System;

namespace Phenix.Core.Net
{
  /// <summary>
  /// 代理类型
  /// </summary>
  [Serializable]
  public enum ProxyType
  {
    /// <summary>
    /// 内嵌
    /// </summary>
    Embedded,

    /// <summary>
    /// remoting
    /// </summary>
    Remoting,

    /// <summary>
    /// WCF
    /// </summary>
    Wcf
  }
}
