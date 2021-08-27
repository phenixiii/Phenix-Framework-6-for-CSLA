using System;
using System.Threading;
using Phenix.Core;
using Phenix.Core.Net;
using Phenix.Core.SyncCollections;

namespace Phenix.Services.Host.Core
{
  /// <summary>
  /// 性能分析
  /// </summary>
  [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1812:AvoidUninstantiatedInternalClasses")]
  internal class PerformanceAnalyse : BaseDisposable<PerformanceAnalyse>
  {
    private PerformanceAnalyse()
      : base()
    {
      _thread = new Thread(Execute);
      _thread.IsBackground = true;
      _thread.Start();
    }

    #region 属性

    private Thread _thread;

    private readonly SynchronizedDictionary<string, PerformanceFetchCount> _fetchCounts =
      new SynchronizedDictionary<string, PerformanceFetchCount>(StringComparer.Ordinal);
    private readonly SynchronizedDictionary<string, PerformanceFetchElapsedTime> _fetchElapsedTimes =
      new SynchronizedDictionary<string, PerformanceFetchElapsedTime>(StringComparer.Ordinal);
    private readonly SynchronizedDictionary<string, PerformanceSaveElapsedTime> _saveElapsedTimes =
      new SynchronizedDictionary<string, PerformanceSaveElapsedTime>(StringComparer.Ordinal);

    private readonly SynchronizedQueue<PerformanceFetchMaxCount> _newFetchMaxCountQueue =
      new SynchronizedQueue<PerformanceFetchMaxCount>();
    private readonly SynchronizedQueue<PerformanceFetchMaxElapsedTime> _newFetchMaxElapsedTimeQueue =
      new SynchronizedQueue<PerformanceFetchMaxElapsedTime>();
    private readonly SynchronizedQueue<PerformanceSaveMaxElapsedTime> _newSaveMaxElapsedTimeQueue =
      new SynchronizedQueue<PerformanceSaveMaxElapsedTime>();

    private static int? _fetchMaxCountWarnThreshold;
    /// <summary>
    /// Fetch最大值提醒阀值
    /// 缺省为 100000(条)
    /// </summary>
    public static int FetchMaxCountWarnThreshold
    {
      get { return AppSettings.GetProperty(ref _fetchMaxCountWarnThreshold, 100000); }
      set { AppSettings.SetProperty(ref _fetchMaxCountWarnThreshold, value); }
    }

    private static int? _fetchMaxElapsedTimeWarnThreshold;
    /// <summary>
    /// Fetch最大用时提醒阀值
    /// 缺省为 60(秒)
    /// </summary>
    public static int FetchMaxElapsedTimeWarnThreshold
    {
      get { return AppSettings.GetProperty(ref _fetchMaxElapsedTimeWarnThreshold, 60); }
      set { AppSettings.SetProperty(ref _fetchMaxElapsedTimeWarnThreshold, value); }
    }

    private static int? _saveMaxElapsedTimeWarnThreshold;
    /// <summary>
    /// Save最大用时提醒阀值
    /// 缺省为 60(秒)
    /// </summary>
    public static int SaveMaxElapsedTimeWarnThreshold
    {
      get { return AppSettings.GetProperty(ref _saveMaxElapsedTimeWarnThreshold, 60); }
      set { AppSettings.SetProperty(ref _saveMaxElapsedTimeWarnThreshold, value); }
    }

    #endregion

    #region 事件

    /// <summary>
    /// 消息通报
    /// </summary>
    public event Action<MessageNotifyEventArgs> MessageNotify;
    private void OnMessageNotify(MessageNotifyEventArgs e)
    {
      Action<MessageNotifyEventArgs> handler = MessageNotify;
      if (handler != null)
        handler(e);
    }

    #endregion

    #region 方法

    #region 实现 BaseDisposable 抽象函数

    protected override void DisposeManagedResources()
    {
      base.DisposeManagedResources();

      if (_thread != null)
      {
        _thread.Abort();
        _thread = null;
      }
    }

    #endregion

    internal void ClearCache()
    {
      _fetchCounts.Clear();
      _fetchElapsedTimes.Clear();
      _saveElapsedTimes.Clear();
      _newFetchMaxCountQueue.Clear();
      _newFetchMaxElapsedTimeQueue.Clear();
      _newSaveMaxElapsedTimeQueue.Clear();
    }

    /// <summary>
    /// 执行
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
    private void Execute()
    {
      try
      {
        while (!Disposing)
          try
          {
            while (_newFetchMaxCountQueue.Count > 0)
            {
              try
              {
                PerformanceFetchMaxCount newFetchMaxCount = _newFetchMaxCountQueue.Dequeue();
                PerformanceFetchCount fetchCount = _fetchCounts.GetValue(newFetchMaxCount.BusinessName, () => new PerformanceFetchCount(newFetchMaxCount.BusinessName), true);
                if (fetchCount.CheckMaxCount(newFetchMaxCount))
                  OnMessageNotify(new MessageNotifyEventArgs(MessageNotifyType.Information, "maximum count of fetch record", newFetchMaxCount.ToString()));
                if (newFetchMaxCount.Value > FetchMaxCountWarnThreshold)
                {
                  OnMessageNotify(new MessageNotifyEventArgs(MessageNotifyType.Warning, "(*+﹏+*)~@ -->", newFetchMaxCount.ToString()));
                  OnMessageNotify(new MessageNotifyEventArgs(MessageNotifyType.Information, "fetch record history", fetchCount.ToString()));
                }
              }
              catch (Exception ex)
              {
                OnMessageNotify(new MessageNotifyEventArgs(MessageNotifyType.Error, "maximum count of fetch record", ex));
              }
              Thread.Sleep(100);
            }

            while (_newFetchMaxElapsedTimeQueue.Count > 0)
            {
              try
              {
                PerformanceFetchMaxElapsedTime newFetchMaxElapsedTime = _newFetchMaxElapsedTimeQueue.Dequeue();
                PerformanceFetchElapsedTime fetchElapsedTime = _fetchElapsedTimes.GetValue(newFetchMaxElapsedTime.BusinessName, () => new PerformanceFetchElapsedTime(newFetchMaxElapsedTime.BusinessName), true);
                if (fetchElapsedTime.CheckMaxElapsedTime(newFetchMaxElapsedTime))
                  OnMessageNotify(new MessageNotifyEventArgs(MessageNotifyType.Information, "maximum elapsed time of fetch record", newFetchMaxElapsedTime.ToString()));
                if (newFetchMaxElapsedTime.Value > FetchMaxElapsedTimeWarnThreshold)
                {
                  OnMessageNotify(new MessageNotifyEventArgs(MessageNotifyType.Warning, "(*+﹏+*)~@ -->", newFetchMaxElapsedTime.ToString()));
                  OnMessageNotify(new MessageNotifyEventArgs(MessageNotifyType.Information, "fetch record history", fetchElapsedTime.ToString()));
                }
              }
              catch (Exception ex)
              {
                OnMessageNotify(new MessageNotifyEventArgs(MessageNotifyType.Error, "maximum elapsed time of fetch record", ex));
              }
              Thread.Sleep(100);
            }

            while (_newSaveMaxElapsedTimeQueue.Count > 0)
            {
              try
              {
                PerformanceSaveMaxElapsedTime newSaveMaxElapsedTime = _newSaveMaxElapsedTimeQueue.Dequeue();
                PerformanceSaveElapsedTime saveElapsedTime = _saveElapsedTimes.GetValue(newSaveMaxElapsedTime.BusinessName, () => new PerformanceSaveElapsedTime(newSaveMaxElapsedTime.BusinessName), true);
                if (saveElapsedTime.CheckMaxElapsedTime(newSaveMaxElapsedTime))
                  OnMessageNotify(new MessageNotifyEventArgs(MessageNotifyType.Information, "maximum elapsed time of save record", newSaveMaxElapsedTime.ToString()));
                if (newSaveMaxElapsedTime.Value > SaveMaxElapsedTimeWarnThreshold)
                {
                  OnMessageNotify(new MessageNotifyEventArgs(MessageNotifyType.Warning, "(*+﹏+*)~@ -->", newSaveMaxElapsedTime.ToString()));
                  OnMessageNotify(new MessageNotifyEventArgs(MessageNotifyType.Information, "save record history", saveElapsedTime.ToString()));
                }
              }
              catch (Exception ex)
              {
                OnMessageNotify(new MessageNotifyEventArgs(MessageNotifyType.Error, "maximum elapsed time of save record", ex));
              }
              Thread.Sleep(100);
            }

            Thread.Sleep(1000);
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
            OnMessageNotify(new MessageNotifyEventArgs(MessageNotifyType.Error, "check performance of fetch record", ex));
            Thread.Sleep(NetConfig.TcpTimedWaitDelay);
          }
      }
      finally
      {
        _thread = null;
      }
    }

    /// <summary>
    /// 检查Fetch最大记录数
    /// </summary>
    public void CheckFetchMaxCount(string businessName, long value, System.Security.Principal.IPrincipal principal)
    {
      CheckFetchMaxCount(businessName, value, principal.Identity as Phenix.Core.Security.IIdentity);
    }

    /// <summary>
    /// 检查Fetch最大记录数
    /// </summary>
    public void CheckFetchMaxCount(string businessName, long value, Phenix.Core.Security.IIdentity identity)
    {
      _newFetchMaxCountQueue.Enqueue(new PerformanceFetchMaxCount(businessName, value, identity));
    }

    /// <summary>
    /// 检查Fetch最大耗时
    /// </summary>
    public void CheckFetchMaxElapsedTime(string businessName, double value, long recordCount, System.Security.Principal.IPrincipal principal)
    {
      CheckFetchMaxElapsedTime(businessName, value, recordCount, principal.Identity as Phenix.Core.Security.IIdentity);
    }

    /// <summary>
    /// 检查Fetch最大耗时
    /// </summary>
    public void CheckFetchMaxElapsedTime(string businessName, double value, long recordCount, Phenix.Core.Security.IIdentity identity)
    {
      _newFetchMaxElapsedTimeQueue.Enqueue(new PerformanceFetchMaxElapsedTime(businessName, value, recordCount, identity));
    }

    /// <summary>
    /// 检查Save最大耗时
    /// </summary>
    public void CheckSaveMaxElapsedTime(string businessName, double value, long recordCount, System.Security.Principal.IPrincipal principal)
    {
      CheckSaveMaxElapsedTime(businessName, value, recordCount, principal.Identity as Phenix.Core.Security.IIdentity);
    }

    /// <summary>
    /// 检查Save最大耗时
    /// </summary>
    public void CheckSaveMaxElapsedTime(string businessName, double value, long recordCount, Phenix.Core.Security.IIdentity identity)
    {
      _newSaveMaxElapsedTimeQueue.Enqueue(new PerformanceSaveMaxElapsedTime(businessName, value, recordCount, identity));
    }

    #endregion
  }
}
