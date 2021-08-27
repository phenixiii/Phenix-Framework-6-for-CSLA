using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNet.SignalR.Client;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Phenix.Core.Security.Cryptography;
using Phenix.Core.Web;

namespace Phenix.Web.Client
{
  /// <summary>
  /// 消息代理
  /// </summary>
  public sealed class MessageProxy : IMessageProxy
  {
    /// <summary>
    /// 初始化
    /// </summary>
    public MessageProxy(HttpClient httpClient)
    {
      _httpClient = httpClient;
    }

    #region 属性

    private const string MESSAGE_URI = "/api/Message";

    private const int DEFAULT_SUBSCRIBE_PORT = 8081;

    private readonly HttpClient _httpClient;
    /// <summary>
    /// HttpClient
    /// </summary>
    public HttpClient HttpClient
    {
      get { return _httpClient; }
    }
    System.Net.Http.HttpClient IMessageProxy.HttpClient
    {
      get { return HttpClient; }
    }

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

    #region Send

    /// <summary>
    /// 发送消息
    /// </summary>
    public async Task SendAsync(string receiver, string content)
    {
      using (HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post,
        String.Format("{0}?receiver={1}",
          MESSAGE_URI,
          receiver)))
      {
        request.Content = new StringContent(content, Encoding.UTF8);
        using (HttpResponseMessage response = await HttpClient.SendAsync(request))
        {
          string result = await response.Content.ReadAsStringAsync();
          if (response.StatusCode != HttpStatusCode.OK)
            throw new HttpRequestException(result);
        }
      }
    }

    /// <summary>
    /// 发送消息
    /// </summary>
    public void Send(string receiver, string content)
    {
      try
      {
        SendAsync(receiver, content).Wait();
      }
      catch (AggregateException ex)
      {
        throw ex.InnerException;
      }
    }

    #endregion

    #region Receive

    /// <summary>
    /// 收取消息
    /// </summary>
    public async Task<IDictionary<long, string>> ReceiveAsync()
    {
      using (HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, MESSAGE_URI))
      {
        using (HttpResponseMessage response = await HttpClient.SendAsync(request))
        {
          string result = await response.Content.ReadAsStringAsync();
          if (response.StatusCode != HttpStatusCode.OK)
            throw new HttpRequestException(result);
          return (IDictionary<long, string>)Phenix.Core.Reflection.Utilities.JsonDeserialize(result, typeof(IDictionary<long, string>));
        }
      }
    }

    /// <summary>
    /// 收取消息
    /// </summary>
    public IDictionary<long, string> Receive()
    {
      try
      {
        return ReceiveAsync().Result;
      }
      catch (AggregateException ex)
      {
        throw ex.InnerException;
      }
    }

    #endregion

    #region AffirmReceived

    /// <summary>
    /// 确认收到
    /// burn = true
    /// </summary>
    public async Task AffirmReceivedAsync(long id)
    {
      await AffirmReceivedAsync(id, true);
    }

    /// <summary>
    /// 确认收到
    /// </summary>
    public async Task AffirmReceivedAsync(long id, bool burn)
    {
      using (HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Delete,
        String.Format("{0}?id={1}&burn={2}",
          MESSAGE_URI,
          id, burn)))
      {
        using (HttpResponseMessage response = await HttpClient.SendAsync(request))
        {
          string result = await response.Content.ReadAsStringAsync();
          if (response.StatusCode != HttpStatusCode.OK)
            throw new HttpRequestException(result);
        }
      }
    }

    /// <summary>
    /// 确认收到
    /// burn = true
    /// </summary>
    public void AffirmReceived(long id)
    {
      try
      {
        AffirmReceivedAsync(id).Wait();
      }
      catch (AggregateException ex)
      {
        throw ex.InnerException;
      }
    }

    /// <summary>
    /// 确认收到
    /// </summary>
    public void AffirmReceived(long id, bool burn)
    {
      try
      {
        AffirmReceivedAsync(id, burn).Wait();
      }
      catch (AggregateException ex)
      {
        throw ex.InnerException;
      }
    }

    #endregion

    #region Subscribe

    /// <summary>
    /// 订阅消息
    /// port = DEFAULT_SUBSCRIBE_PORT
    /// </summary>
    /// <param name="onReceived">处理收到消息</param>
    /// <param name="onError">处理收到错误消息</param>
    public async Task SubscribeAsync(Action<IMessageProxy, IDictionary<long, string>> onReceived, Action<IMessageProxy, string> onError)
    {
      await SubscribeAsync(DEFAULT_SUBSCRIBE_PORT, onReceived, onError);
    }

    /// <summary>
    /// 订阅消息
    /// </summary>
    /// <param name="port">端口号</param>
    /// <param name="onReceived">处理收到消息</param>
    /// <param name="onError">处理收到错误消息</param>
    public async Task SubscribeAsync(int port, Action<IMessageProxy, IDictionary<long, string>> onReceived, Action<IMessageProxy, string> onError)
    {
      if (_hubConnection != null)
      {
        if (_hubConnection.State == ConnectionState.Connected)
          throw new InvalidOperationException("当前已在线, 不允许重复订阅!");
        _hubConnection.Dispose();
      }

      string timestamp = Guid.NewGuid().ToString();
      _hubConnection = new HubConnection(
        String.Format("{0}://{1}:{2}", HttpClient.BaseAddress.Scheme, HttpClient.BaseAddress.Host, port),
        String.Format("{0}={1},{2},{3}", HttpClient.AUTHORIZATION_HEADER_NAME, Uri.EscapeDataString(HttpClient.UserIdentity.UserNumber),
          timestamp, RijndaelCryptoTextProvider.Encrypt(HttpClient.UserIdentity.Password, timestamp))); //身份认证格式: [UserNumber],[timestamp],[signature = Encrypt(Password, timestamp)]
      IHubProxy hubProxy = _hubConnection.CreateHubProxy("messageHub");
      hubProxy.Subscribe("subscribe").Received += delegate(IList<JToken> items)
      {
        if (items.Count == 1)
          onReceived(this, JsonConvert.DeserializeObject<Dictionary<long, string>>(items[0].ToString()));
        else
        {
          Dictionary<long, string> array = new Dictionary<long, string>();
          foreach (JToken item in items)
          foreach (KeyValuePair<long, string> kvp in JsonConvert.DeserializeObject<Dictionary<long, string>>(item.ToString()))
            array.Add(kvp.Key, kvp.Value);
          onReceived(this, array);
        }
      };
      hubProxy.Subscribe("onError").Received += delegate(IList<JToken> items)
      {
        foreach (JToken item in items)
          onError(this, item.ToString());
      };
      await _hubConnection.Start();
    }

    /// <summary>
    /// 订阅消息
    /// port = DEFAULT_SUBSCRIBE_PORT
    /// </summary>
    /// <param name="onReceived">处理收到消息</param>
    /// <param name="onError">处理收到错误消息</param>
    public void Subscribe(Action<IMessageProxy, IDictionary<long, string>> onReceived, Action<IMessageProxy, string> onError)
    {
      try
      {
        SubscribeAsync(onReceived, onError).Wait();
      }
      catch (AggregateException ex)
      {
        throw ex.InnerException;
      }
    }

    /// <summary>
    /// 订阅消息
    /// </summary>
    /// <param name="port">端口号</param>
    /// <param name="onReceived">处理收到消息</param>
    /// <param name="onError">处理收到错误消息</param>
    public void Subscribe(int port, Action<IMessageProxy, IDictionary<long, string>> onReceived, Action<IMessageProxy, string> onError)
    {
      try
      {
        SubscribeAsync(port, onReceived, onError).Wait();
      }
      catch (AggregateException ex)
      {
        throw ex.InnerException;
      }
    }

    #endregion

    #endregion
  }
}
