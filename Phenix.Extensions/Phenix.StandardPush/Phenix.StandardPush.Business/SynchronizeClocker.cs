using System;
using System.Net;
using System.Text;
using ALAZ.SystemEx.NetEx.SocketsEx;
using Phenix.Core;

namespace Phenix.StandardPush.Business
{
  /// <summary>
  /// 与数据库时钟保持同步
  /// </summary>
  public sealed class SynchronizeClocker : Phenix.Core.BaseDisposable, ISocketService
  {
    /// <summary>
    /// 初始化
    /// </summary>
    public SynchronizeClocker(IPAddress address, int port,
      int socketBufferSize, int messageBufferSize,
      int idleCheckInterval, int idleTimeOutValue)
    {
      _socketClient = new SocketClient(CallbackThreadType.ctWorkerThread, this,
        DelimiterType.dtNone, null,
        socketBufferSize, messageBufferSize,
        idleCheckInterval, idleTimeOutValue);
      _socketClient.AddConnector(null, new IPEndPoint(address, port));
      _socketClient.Start();
    }

    #region 属性

    private readonly object _lock = new object();
    private SocketClient _socketClient;
    private ISocketConnection _connection;
    private string _receivedMessage = String.Empty;

    private DateTime? _value;
    /// <summary>
    /// 值
    /// </summary>
    public DateTime? Value
    {
      get { return _value; }
    }

    #endregion

    #region 方法

    #region 实现 BaseDisposable 抽象函数

    protected override void DisposeManagedResources()
    {
      lock (_lock)
      {
        if (_connection != null)
        {
          _connection.BeginDisconnect();
          _connection = null;
        }
        if (_socketClient != null)
        {
          _socketClient.Dispose();
          _socketClient = null;
        }
      }
    }

    protected override void DisposeUnmanagedResources()
    {
    }

    #endregion

    private void AnalyseReceivedMessage(byte[] buffer)
    {
      if (buffer == null)
        return;
      lock (_lock)
      {
        _receivedMessage = _receivedMessage + Encoding.Default.GetString(buffer);
        if (_receivedMessage.Substring(_receivedMessage.Length - 1, 1) == AppConfig.ROW_SEPARATOR.ToString())
        {
          string[] rows = _receivedMessage.Split(new Char[] { AppConfig.ROW_SEPARATOR }, StringSplitOptions.RemoveEmptyEntries);
          for (int i = 0; i < rows.Length; i++)
            _value = DateTime.Parse(rows[i]);
          _receivedMessage = String.Empty;
        }
      }
    }

    #region ISocketService 成员

    void ISocketService.OnConnected(ALAZ.SystemEx.NetEx.SocketsEx.ConnectionEventArgs e)
    {
      _connection = e.Connection;
      e.Connection.BeginReceive();
    }

    void ISocketService.OnReceived(ALAZ.SystemEx.NetEx.SocketsEx.MessageEventArgs e)
    {
      AnalyseReceivedMessage(e.Buffer);
    }

    void ISocketService.OnSent(ALAZ.SystemEx.NetEx.SocketsEx.MessageEventArgs e)
    {
    }

    void ISocketService.OnDisconnected(ALAZ.SystemEx.NetEx.SocketsEx.ConnectionEventArgs e)
    {
    }

    void ISocketService.OnException(ALAZ.SystemEx.NetEx.SocketsEx.ExceptionEventArgs e)
    {
    }

    #endregion

    #endregion
  }
}
