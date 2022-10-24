using System;

namespace Phenix.Core.Net
{
  /// <summary>
  /// Remoting协议类型 
  /// </summary>
  [Serializable]
  public enum RemotingProtocolType
  {
    /// <summary>
    /// HTTP
    /// </summary>
    Http,

    /// <summary>
    /// TCP
    /// </summary>
    Tcp
  }
}
