using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using Phenix.Core.Net;
using Phenix.Core.Security;
using Phenix.Core.SyncCollections;
using Phenix.Core.Web;

namespace Phenix.Services.Host.Service.Web
{
  public class MessageHub : Hub
  {
    #region 属性

    private static readonly SynchronizedDictionary<string, ConnectedInfo> _connectedInfos =
      new SynchronizedDictionary<string, ConnectedInfo>(StringComparer.Ordinal);

    private static Thread _clearThread;

    #endregion

    private class ConnectedInfo
    {
      public ConnectedInfo(UserIdentity userIdentity)
      {
        _userIdentity = userIdentity;
      }

      private readonly UserIdentity _userIdentity;
      public UserIdentity UserIdentity
      {
        get { return _userIdentity; }
      }

      private readonly SynchronizedDictionary<long, DateTime> _sentDateTimes = new SynchronizedDictionary<long, DateTime>();
      public IDictionary<long, DateTime> SentDateTimes
      {
        get { return _sentDateTimes; }
      }

      private DateTime _lastActionTime = DateTime.Now;
      public DateTime LastActionTime
      {
        get { return _lastActionTime; }
        set { _lastActionTime = value; }
      }
    }

    #region 方法

    private static DataSecurityContext CheckIn(HubCallerContext context)
    {
      string value = context.QueryString.Get(WebConfig.AUTHORIZATION_HEADER_NAME);
      if (String.IsNullOrEmpty(value))
        throw new InvalidOperationException(String.Format("HubCallerContext参数的QueryString缺失{0}身份认证信息!", WebConfig.AUTHORIZATION_HEADER_NAME));
      string[] strings = value.Split(',');
      if (strings.Length == 3)
        //身份认证格式: [UserNumber],[timestamp],[signature = Encrypt(Password, timestamp)]
        return DataSecurityHub.CheckIn(NetConfig.LocalAddress, NetConfig.LocalAddress, Uri.UnescapeDataString(strings[0]), strings[1], strings[2], false);
      else
        throw new InvalidOperationException(String.Format("HubCallerContext参数的QueryString身份认证{0}格式错误!", value));
    }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
    private static void ExecutePush()
    {
      try
      {
        IHubContext context = GlobalHost.ConnectionManager.GetHubContext<MessageHub>();
        while (true)
          try
          {
            foreach (KeyValuePair<string, ConnectedInfo> connectedInfo in _connectedInfos)
              if (connectedInfo.Value.SentDateTimes.Count > 0 || DateTime.Now.Subtract(connectedInfo.Value.LastActionTime).TotalSeconds >= 1)
              {
                IDictionary<long, string> receivedMessages = Phenix.Core.Message.MessageHub.Receive(connectedInfo.Value.UserIdentity);
                if (receivedMessages.Count > 0)
                {
                  Dictionary<long, string> sendingMessages = new Dictionary<long, string>(receivedMessages.Count);
                  foreach (KeyValuePair<long, string> receivedMessage in receivedMessages)
                  {
                    DateTime sentDateTime;
                    if (!connectedInfo.Value.SentDateTimes.TryGetValue(receivedMessage.Key, out sentDateTime) || DateTime.Now.Subtract(sentDateTime).TotalMinutes >= 1)
                    {
                      sendingMessages[receivedMessage.Key] = receivedMessage.Value;
                      connectedInfo.Value.SentDateTimes[receivedMessage.Key] = DateTime.Now;
                    }
                  }
                  if (sendingMessages.Count > 0)
                    context.Clients.Client(connectedInfo.Key).Invoke("onReceived", sendingMessages);
                }
                else
                {
                  connectedInfo.Value.SentDateTimes.Clear();
                  connectedInfo.Value.LastActionTime = DateTime.Now;
                }
              }
            Thread.Sleep(100);
          }
          catch (ObjectDisposedException)
          {
            return;
          }
          catch (ThreadAbortException)
          {
            Thread.ResetAbort();
            return;
          }
          catch (Exception)
          {
            Thread.Sleep(NetConfig.TcpTimedWaitDelay);
          }
      }
      finally
      {
        _clearThread = null;
      }
    }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
    public override Task OnConnected()
    {
      try
      {
        _connectedInfos[Context.ConnectionId] = new ConnectedInfo(CheckIn(Context).Identity);
      }
      catch (Exception ex)
      {
        Clients.Caller.Invoke("onError", ex.Message);
      }
      if (_clearThread == null)
        lock (_connectedInfos)
          if (_clearThread == null)
          {
            _clearThread = new Thread(ExecutePush);
            _clearThread.IsBackground = true;
            _clearThread.Start();
          }
      return base.OnConnected();
    }

    public override Task OnDisconnected(bool stopCalled)
    {
      if (stopCalled)
        _connectedInfos.Remove(Context.ConnectionId);
      return base.OnDisconnected(stopCalled);
    }

    #endregion
  }
}
