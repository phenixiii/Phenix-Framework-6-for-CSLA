using System;
using System.Collections.Generic;
using System.Reflection;
using Phenix.Core.Log;
using Phenix.Core.Security;

namespace Phenix.Core.Message
{
  /// <summary>
  /// 消息中心
  /// </summary>
  public static class MessageHub
  {
    #region 属性

    private static IMessage _worker;
    /// <summary>
    /// 实施者
    /// </summary>
    public static IMessage Worker
    {
      get
      {
        if (_worker == null)
          AppUtilities.RegisterWorker();
        return _worker;
      }
      set { _worker = value; }
    }

    #endregion

    #region 方法

    /// <summary>
    /// 检查活动
    /// </summary>
    public static void CheckActive()
    {
      if (Worker == null)
      {
        Exception ex = new InvalidOperationException(Phenix.Core.Properties.Resources.NoService);
        EventLog.SaveLocal(MethodBase.GetCurrentMethod(), ex);
        throw ex;
      }
    }

    /// <summary>
    /// 发送消息
    /// identity = Phenix.Core.Security.UserIdentity.CurrentIdentity
    /// </summary>
    public static void Send(string receiver, string content)
    {
      Send(receiver, content, UserIdentity.CurrentIdentity);
    }

    /// <summary>
    /// 发送消息
    /// </summary>
    [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
    public static void Send(string receiver, string content, UserIdentity identity)
    {
      identity = identity ?? UserIdentity.CurrentIdentity;
      CheckActive();
      Worker.Send(receiver, content, identity);
    }

    /// <summary>
    /// 收取消息
    /// </summary>
    public static IDictionary<long, string> Receive()
    {
      return Receive(UserIdentity.CurrentIdentity);
    }

    /// <summary>
    /// 收取消息
    /// </summary>
    [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
    public static IDictionary<long, string> Receive(UserIdentity identity)
    {
      identity = identity ?? UserIdentity.CurrentIdentity;
      CheckActive();
      return Worker.Receive(identity);
    }

    /// <summary>
    /// 确认收到
    /// </summary>
    public static void AffirmReceived(long id, bool burn)
    {
      AffirmReceived(id, burn, UserIdentity.CurrentIdentity);
    }

    /// <summary>
    /// 确认收到
    /// </summary>
    [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
    public static void AffirmReceived(long id, bool burn, UserIdentity identity)
    {
      identity = identity ?? UserIdentity.CurrentIdentity;
      CheckActive();
      Worker.AffirmReceived(id, burn, identity);
    }

    #endregion
  }
}