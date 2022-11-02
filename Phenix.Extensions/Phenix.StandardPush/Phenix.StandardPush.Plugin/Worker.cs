using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Net;
using System.Text;
using System.Threading;
using ALAZ.SystemEx.NetEx.SocketsEx;
using Phenix.Core;
using Phenix.Core.Data;
using Phenix.Core.Net;
using Phenix.Core.SyncCollections;
using Phenix.StandardPush.Contract;

namespace Phenix.StandardPush.Plugin
{
  /// <summary>
  /// 作业者
  /// </summary>
  public sealed class Worker : BaseDisposable, IWorker, ISocketService
  {
    #region 单例

    private Worker(Action<MessageNotifyEventArgs> doMessageNotify)
      : base()
    {
      _messageNotify = doMessageNotify;

      _socketServer = new SocketServer(CallbackThreadType.ctWorkerThread, this,
        DelimiterType.dtNone, null,
        SocketBufferSize, MessageBufferSize,
        IdleCheckInterval, IdleTimeOutValue);
      _socketServer.AddListener(null, new IPEndPoint(IPAddress.Any, Port));
      _socketServer.Start();

      _thread = new Thread(Execute);
      _thread.IsBackground = true;
      _thread.Start();
    }

    private static readonly object _defaultLock = new object();
    private static Worker _default;

    /// <summary>
    /// 运行
    /// </summary>
    public static Worker Run(Action<MessageNotifyEventArgs> doMessageNotify)
    {
      Worker result = _default;
      if (result != null)
      {
        result.Suspending = false;
        return result;
      }
      lock (_defaultLock)
      {
        if (_default == null)
          _default = new Worker(doMessageNotify);
        return _default;
      }
    }

    /// <summary>
    /// 关闭
    /// </summary>
    public static void Stop()
    {
      Worker worker = _default;
      if (worker != null)
        worker.Dispose();
    }

    #endregion

    #region 属性

    #region Socket配置参数

    private int? _port;
    /// <summary>
    /// 缺省端口: 8088
    /// </summary>
    public int Port
    {
      get { return AppSettings.GetProperty(ref _port, 8088); }
      set { AppSettings.SetProperty(ref _port, value); }
    }

    private int? _socketBufferSize;
    /// <summary>
    /// Socket缓存尺寸: 1024
    /// </summary>
    public int SocketBufferSize
    {
      get { return AppSettings.GetProperty(ref _socketBufferSize, 1024); }
      set { AppSettings.SetProperty(ref _socketBufferSize, value); }
    }

    private int? _messageBufferSize;
    /// <summary>
    /// 消息缓存尺寸: 2048
    /// </summary>
    public int MessageBufferSize
    {
      get { return AppSettings.GetProperty(ref _messageBufferSize, 2048); }
      set { AppSettings.SetProperty(ref _messageBufferSize, value); }
    }

    private int? _idleCheckInterval;
    /// <summary>
    /// 闲置检查间隔: 0
    /// </summary>
    public int IdleCheckInterval
    {
      get { return AppSettings.GetProperty(ref _idleCheckInterval, 0); }
      set { AppSettings.SetProperty(ref _idleCheckInterval, value); }
    }

    private int? _idleTimeOutValue;
    /// <summary>
    /// 闲置超时时间: 0
    /// </summary>
    public int IdleTimeOutValue
    {
      get { return AppSettings.GetProperty(ref _idleTimeOutValue, 0); }
      set { AppSettings.SetProperty(ref _idleTimeOutValue, value); }
    }

    #endregion

    private readonly object _lock = new object();

    private SocketServer _socketServer;
    private readonly SynchronizedDictionary<ISocketConnection, string> _socketConnections =
      new SynchronizedDictionary<ISocketConnection, string>();

    private Thread _thread;

    private DateTime? _clock;
    /// <summary>
    /// 时钟
    /// </summary>
    public DateTime? Clock
    {
      get { return _clock; }
      private set { _clock = value; }
    }

    private bool _suspending;
    /// <summary>
    /// 是否暂停
    /// </summary>
    public bool Suspending
    {
      get { return _suspending; }
      internal set
      {
        _suspending = value;
        if (value)
          Clock = null;
      }
    }

    #endregion

    #region 事件

    private event Action<MessageNotifyEventArgs> _messageNotify;
    private void OnMessageNotify(MessageNotifyEventArgs e)
    {
      Action<MessageNotifyEventArgs> handler = _messageNotify;
      if (handler != null)
        handler(e);
    }

    #endregion

    #region 方法

    #region 实现 BaseDisposable 抽象函数

    protected override void DisposeManagedResources()
    {
      if (_default == this)
        lock (_defaultLock)
          if (_default == this)
          {
            _default = null;
          }
      lock (_lock)
      {
        if (_thread != null)
        {
          _thread.Abort();
          _thread = null;
        }
        if (_socketServer != null)
        {
          _socketServer.Dispose();
          _socketServer = null;
        }
      }
    }

    protected override void DisposeUnmanagedResources()
    {
    }

    #endregion

    #region ISocketService 成员

    void ISocketService.OnConnected(ALAZ.SystemEx.NetEx.SocketsEx.ConnectionEventArgs e)
    {
      if (Disposing)
        return;
      OnMessageNotify(new MessageNotifyEventArgs(MessageNotifyType.Information, this.ToString(),
        String.Format("{0} - Connected", e.Connection.RemoteEndPoint)));

      _socketConnections[e.Connection] = e.Connection.RemoteEndPoint.ToString();
      e.Connection.BeginReceive();
    }

    void ISocketService.OnSent(ALAZ.SystemEx.NetEx.SocketsEx.MessageEventArgs e)
    {
      if (Disposing)
        return;
      OnMessageNotify(new MessageNotifyEventArgs(MessageNotifyType.Information, this.ToString(),
        String.Format("{0} - Sent: {1}", e.Connection.RemoteEndPoint, e.Buffer != null ? Encoding.Default.GetString(e.Buffer) : null)));
    }

    void ISocketService.OnReceived(ALAZ.SystemEx.NetEx.SocketsEx.MessageEventArgs e)
    {
      if (Disposing)
        return;
      OnMessageNotify(new MessageNotifyEventArgs(MessageNotifyType.Information, this.ToString(),
        String.Format("{0} - Received: {1}", e.Connection.RemoteEndPoint, e.Buffer != null ? Encoding.Default.GetString(e.Buffer) : null)));
    }

    void ISocketService.OnDisconnected(ALAZ.SystemEx.NetEx.SocketsEx.ConnectionEventArgs e)
    {
      if (Disposing)
        return;
      OnMessageNotify(new MessageNotifyEventArgs(MessageNotifyType.Information, this.ToString(),
        String.Format("{0} - Disconnected", _socketConnections[e.Connection])));

      _socketConnections.Remove(e.Connection);
    }

    void ISocketService.OnException(ALAZ.SystemEx.NetEx.SocketsEx.ExceptionEventArgs e)
    {
      if (Disposing)
        return;
      OnMessageNotify(new MessageNotifyEventArgs(MessageNotifyType.Error, this.ToString(),
        String.Format("{0} - Error", _socketConnections.ContainsKey(e.Connection) ? _socketConnections[e.Connection] : null), e.Exception));
    }

    #endregion

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
    private void SendData()
    {
      DateTime? clock = Clock;
      if (!clock.HasValue)
        return;
      IList<byte[]> messages = SocketHelper.SplitMessages(clock.ToString() + AppConfig.ROW_SEPARATOR, SocketBufferSize);
      foreach (KeyValuePair<ISocketConnection, string> kvp in _socketConnections)
        try
        {
          foreach (byte[] item in messages)
            if (item != null && item.Length > 0)
              kvp.Key.BeginSend(item);
        }
        catch (Exception ex)
        {
          OnMessageNotify(new MessageNotifyEventArgs(MessageNotifyType.Error, this.ToString(), ex));
        }
    }

    private static DateTime? SynchronizeClock(DbConnection connection)
    {
      using (SafeDataReader reader = new SafeDataReader(connection,
@"select sysdate
  from PH_SystemInfo",
        CommandBehavior.SingleRow, false))
      {
        if (reader.Read())
        {
          DateTime result = reader.GetDateTime(0);
          if (Math.Abs(DateTime.Now.Subtract(result).TotalSeconds) > 0)
            Phenix.Core.Win32.NativeMethods.SetClock(result);
          return result;
        }
        else
        {
          return null;
        }
      }
    }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
    private void Execute()
    {
      try
      {
        const int interval = 1000;
        bool needSynchronize = true;
        while (!Disposing)
          try
          {
            if (!Suspending && needSynchronize)
            {
              Clock = DefaultDatabase.ExecuteGet(SynchronizeClock);
              OnMessageNotify(Clock.HasValue
                ? new MessageNotifyEventArgs(MessageNotifyType.Information, this.ToString(), Clock.Value.ToLongTimeString())
                : new MessageNotifyEventArgs(MessageNotifyType.Warning, this.ToString(), Clock.HasValue.ToString()));
            }
            Thread.Sleep(interval);
            //递增并对时
            if (Clock.HasValue)
            {
              Clock = Clock.Value.AddMilliseconds(interval);
              needSynchronize = Math.Abs(DateTime.Now.Subtract(Clock.Value).TotalSeconds) > 0;
              SendData();
            }
            else
              needSynchronize = true;
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
          catch (Exception ex)
          {
            OnMessageNotify(new MessageNotifyEventArgs(MessageNotifyType.Error, this.ToString(), ex));
            needSynchronize = true;
            Thread.Sleep(1000);
          }
      }
      finally
      {
        _thread = null;
      }
    }

    public override string ToString()
    {
      return "Worker";
    }

    #endregion
  }
}
