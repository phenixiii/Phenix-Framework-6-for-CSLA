using System.Collections.Generic;
using Phenix.Core.Security;

namespace Phenix.Core.Message
{
  /// <summary>
  /// 消息接口
  /// </summary>
  public interface IMessage
  {
    #region 方法

    /// <summary>
    /// 发送消息
    /// </summary>
    void Send(string receiver, string content, UserIdentity identity);

    /// <summary>
    /// 收取消息
    /// </summary>
    IDictionary<long, string> Receive(UserIdentity identity);

    /// <summary>
    /// 确认收到
    /// </summary>
    void AffirmReceived(long id, bool burn, UserIdentity identity);

    #endregion
  }
}
