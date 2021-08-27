using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Client;
using Microsoft.AspNet.SignalR.Hubs;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Phenix.Core.SyncCollections;
using Phenix.Core.Web;
using Phenix.Services.Host.WebCluster.Core;

namespace Phenix.Services.Host.WebCluster.Service.Web
{
  public class MessageHub : Hub
  {
    #region 属性

    private static readonly SynchronizedDictionary<string, ConnectedInfo> _connectedInfos =
      new SynchronizedDictionary<string, ConnectedInfo>(StringComparer.Ordinal);

    private static readonly IHubContext _context = GlobalHost.ConnectionManager.GetHubContext<MessageHub>();

    #endregion

    private class ConnectedInfo : IDisposable
    {
      public ConnectedInfo(Channel channel, HubCallerContext context)
      {
        _hubConnection = new HubConnection(String.Format("{0}:{1}", channel.HostUrl, channel.WebSocketPort),
          String.Format("{0}={1}", WebConfig.AUTHORIZATION_HEADER_NAME, context.QueryString.Get(WebConfig.AUTHORIZATION_HEADER_NAME)));
        IHubProxy hubProxy = _hubConnection.CreateHubProxy("messageHub");
        hubProxy.Subscribe("onReceived").Received += delegate(IList<JToken> items)
        {
          if (items.Count == 1)
            _context.Clients.Client(context.ConnectionId).Invoke("onReceived", JsonConvert.DeserializeObject<Dictionary<long, string>>(items[0].ToString()));
          else
          {
            Dictionary<long, string> array = new Dictionary<long, string>();
            foreach (JToken item in items)
            foreach (KeyValuePair<long, string> kvp in JsonConvert.DeserializeObject<Dictionary<long, string>>(item.ToString()))
              array.Add(kvp.Key, kvp.Value);
            _context.Clients.Client(context.ConnectionId).Invoke("onReceived", array);
          }
        };
        hubProxy.Subscribe("onError").Received += delegate (IList<JToken> items)
        {
          foreach (JToken item in items)
            _context.Clients.Client(context.ConnectionId).Invoke("onError", item.ToString());
        };
        _hubConnection.Start();
      }

      #region 属性

      private HubConnection _hubConnection;

      #endregion

      #region 方法

      /// <summary>
      /// 释放
      /// </summary>
      public void Dispose()
      {
        if (_hubConnection != null)
        {
          _hubConnection.Dispose();
          _hubConnection = null;
        }
      }

      #endregion
    }

    #region 方法


    public override Task OnConnected()
    {
      Channel channel = ChannelManager.Default.GetChannel();
      if (channel == null)
        Clients.Caller.Invoke("onError", "请在WebCluster上设置需要代理的服务!");
      _connectedInfos[Context.ConnectionId] = new ConnectedInfo(channel, Context);
      return base.OnConnected();
    }

    public override Task OnDisconnected(bool stopCalled)
    {
      if (stopCalled)
        _connectedInfos.Remove(Context.ConnectionId, c => c.Dispose());
      return base.OnDisconnected(stopCalled);
    }

    #endregion
  }
}
