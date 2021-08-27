using System;
using System.Threading;
using Phenix.Core;

namespace Phenix.StandardService.Business
{
  /// <summary>
  /// 同步器
  /// </summary>
  public sealed class Synchronizer : BaseDisposable
  {
    #region 单例

    private Synchronizer(Action<MessageNotifyEventArgs> doMessageNotify)
      : base()
    {
      _messageNotify = doMessageNotify;

      _thread = new Thread(Execute);
      _thread.IsBackground = true;
      _thread.Start();
    }

    private static readonly object _defaultLock = new object();
    private static Synchronizer _default;

    /// <summary>
    /// 运行
    /// </summary>
    public static Synchronizer Run()
    {
      return Run(null);
    }

    /// <summary>
    /// 运行
    /// </summary>
    public static Synchronizer Run(Action<MessageNotifyEventArgs> doMessageNotify)
    {
      Synchronizer result = _default;
      if (result != null)
      {
        result.Suspending = false;
        return result;
      }
      lock (_defaultLock)
      {
        if (_default == null)
          _default = new Synchronizer(doMessageNotify);
        return _default;
      }
    }

    /// <summary>
    /// 关闭
    /// </summary>
    public static void Stop()
    {
      Synchronizer worker = _default;
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
      int interval = Interval * 1000;
      bool needSynchronize = true;
      SynchronizeClockCommand command = new SynchronizeClockCommand();
      //开始循环
      while (!Disposing)
        try
        {
          if (!Suspending && needSynchronize)
          {
            Clock = SynchronizeClockCommand.Execute(command).Value;
            OnMessageNotify(Clock.HasValue
              ? new MessageNotifyEventArgs(MessageNotifyType.Information, this.ToString(), Clock.Value.ToLongTimeString())
              : new MessageNotifyEventArgs(MessageNotifyType.Warning, this.ToString(), Clock.HasValue.ToString()));
          }
          Thread.Sleep(interval);
          //递增并对时
          if (Clock.HasValue)
          {
            Clock = Clock.Value.AddMilliseconds(interval);
            needSynchronize = Math.Abs(DateTime.Now.Subtract(Clock.Value).Seconds) >= Deviation;
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

    public override string ToString()
    {
      return "Synchronizer";
    }

    #endregion
  }
}
