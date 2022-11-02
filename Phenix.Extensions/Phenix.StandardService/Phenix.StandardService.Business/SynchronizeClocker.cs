using System;
using System.Threading;
using Phenix.Core;

namespace Phenix.StandardService.Business
{
  /// <summary>
  /// 与数据库时钟保持同步
  /// </summary>
  public sealed class SynchronizeClocker : BaseDisposable
  {
    #region 单例

    private SynchronizeClocker(Action<MessageNotifyEventArgs> doMessageNotify)
      : base()
    {
      _messageNotify = doMessageNotify;

      _thread = new Thread(Execute);
      _thread.IsBackground = true;
      _thread.Start();
    }

    private static readonly object _defaultLock = new object();
    private static SynchronizeClocker _default;

    /// <summary>
    /// 运行
    /// </summary>
    public static SynchronizeClocker Run()
    {
      return Run(null);
    }

    /// <summary>
    /// 运行
    /// </summary>
    public static SynchronizeClocker Run(Action<MessageNotifyEventArgs> doMessageNotify)
    {
      SynchronizeClocker result = _default;
      if (result != null)
      {
        result.Suspending = false;
        return result;
      }
      lock (_defaultLock)
      {
        if (_default == null)
          _default = new SynchronizeClocker(doMessageNotify);
        return _default;
      }
    }

    /// <summary>
    /// 关闭
    /// </summary>
    public static void Stop()
    {
      SynchronizeClocker worker = _default;
      if (worker != null)
        worker.Dispose();
    }

    #endregion

    #region 属性

    private int _interval = 1;
    /// <summary>
    /// 刷新频率(秒)
    /// 缺省为 1
    /// </summary>
    public int Interval
    {
      get { return _interval; }
      set
      {
        if (value >= 1)
          _interval = value;
      }
    }

    private int _deviation = 1;
    /// <summary>
    /// 误差精度(秒)
    /// 缺省为 1
    /// </summary>
    public int Deviation
    {
      get { return _deviation; }
      set
      {
        if (value >= 1)
          _deviation = value;
      }
    }

    private Thread _thread;

    private DateTime? _value;
    /// <summary>
    /// 值
    /// </summary>
    public DateTime? Value
    {
      get { return _value; }
      private set { _value = value; }
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
          Value = null;
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
      if (_thread != null)
      {
        _thread.Abort();
        _thread = null;
      }
    }

    protected override void DisposeUnmanagedResources()
    {
    }

    #endregion

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
    private void Execute()
    {
      try
      {
        int interval = Interval * 1000;
        bool needSynchronize = true;
        SynchronizeClockCommand command = new SynchronizeClockCommand();
        while (!Disposing)
          try
          {
            if (!Suspending && needSynchronize)
            {
              Value = SynchronizeClockCommand.Execute(command).Value;
              OnMessageNotify(Value.HasValue
                ? new MessageNotifyEventArgs(MessageNotifyType.Information, this.ToString(), Value.Value.ToLongTimeString())
                : new MessageNotifyEventArgs(MessageNotifyType.Warning, this.ToString(), Value.HasValue.ToString()));
            }
            Thread.Sleep(interval);
            //递增并对时
            if (Value.HasValue)
            {
              Value = Value.Value.AddMilliseconds(interval);
              needSynchronize = Math.Abs(DateTime.Now.Subtract(Value.Value).Seconds) >= Deviation;
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
            Thread.Sleep(interval);
          }
      }
      finally
      {
        _thread = null;
      }
    }

    public override string ToString()
    {
      return "Synchronizer";
    }

    #endregion
  }
}
