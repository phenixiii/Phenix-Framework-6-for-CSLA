using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace Phenix.Core.Web
{
  /// <summary>
  /// 消息代理接口
  /// </summary>
  public interface IMessageProxy : IDisposable
  {
    #region 属性

    /// <summary>
    /// HttpClient
    /// </summary>
    HttpClient HttpClient { get; }

    #endregion

    #region 方法

    #region Send

    /// <summary>
    /// 发送消息
    /// </summary>
    Task SendAsync(string receiver, string content);

    /// <summary>
    /// 发送消息
    /// </summary>
    void Send(string receiver, string content);

    #endregion

    #region Receive

    /// <summary>
    /// 收取消息
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures")]
    Task<IDictionary<long, string>> ReceiveAsync();

    /// <summary>
    /// 收取消息
    /// </summary>
    IDictionary<long, string> Receive();

    #endregion

    #region AffirmReceived

    /// <summary>
    /// 确认收到
    /// </summary>
    Task AffirmReceivedAsync(long id);

    /// <summary>
    /// 确认收到
    /// </summary>
    Task AffirmReceivedAsync(long id, bool burn);

    /// <summary>
    /// 确认收到
    /// </summary>
    void AffirmReceived(long id);

    /// <summary>
    /// 确认收到
    /// </summary>
    void AffirmReceived(long id, bool burn);

    #endregion

    #region Subscribe
    
    /// <summary>
    /// 订阅消息
    /// port = DEFAULT_SUBSCRIBE_PORT
    /// </summary>
    /// <param name="onReceived">处理收到消息</param>
    /// <param name="onError">处理收到错误消息</param>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures")]
    Task SubscribeAsync(Action<IMessageProxy, IDictionary<long, string>> onReceived, Action<IMessageProxy, string> onError);

    /// <summary>
    /// 订阅消息
    /// </summary>
    /// <param name="port">端口号</param>
    /// <param name="onReceived">处理收到消息</param>
    /// <param name="onError">处理收到错误消息</param>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures")]
    Task SubscribeAsync(int port, Action<IMessageProxy, IDictionary<long, string>> onReceived, Action<IMessageProxy, string> onError);

    /// <summary>
    /// 订阅消息
    /// port = DEFAULT_SUBSCRIBE_PORT
    /// </summary>
    /// <param name="onReceived">处理收到消息</param>
    /// <param name="onError">处理收到错误消息</param>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures")]
    void Subscribe(Action<IMessageProxy, IDictionary<long, string>> onReceived, Action<IMessageProxy, string> onError);

    /// <summary>
    /// 订阅消息
    /// </summary>
    /// <param name="port">端口号</param>
    /// <param name="onReceived">处理收到消息</param>
    /// <param name="onError">处理收到错误消息</param>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures")]
    void Subscribe(int port, Action<IMessageProxy, IDictionary<long, string>> onReceived, Action<IMessageProxy, string> onError);

    #endregion

    #endregion
  }
}
