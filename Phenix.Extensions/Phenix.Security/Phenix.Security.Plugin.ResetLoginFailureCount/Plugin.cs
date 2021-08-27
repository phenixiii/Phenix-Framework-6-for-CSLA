using System;
using System.Data.Common;
using System.Threading;
using Phenix.Core;
using Phenix.Core.Data;
using Phenix.Core.Plugin;

namespace Phenix.Security.Plugin.ResetLoginFailureCount
{
  public class Plugin : PluginBase<Plugin>
  {
    public Plugin()
    {
      _thread = new Thread(new ThreadStart(Execute));
      _thread.Start();
    }

    #region 属性

    private Thread _thread;

    private const int MIN_RESET_INTERVAL_MINUTES = 3;

    private int? _resetInterval;
    /// <summary>
    /// 解锁时间间隔(分钟)
    /// 缺省为 60 * 24
    /// </summary>
    public int ResetInterval
    {
      get { return AppSettings.GetProperty(ref _resetInterval, 60 * 24); }
      set { AppSettings.SetProperty(ref _resetInterval, value); }
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

    #region 覆写 PluginBase

    /// <summary>
    /// 设置
    /// 由 PluginHost 调用
    /// </summary>
    /// <param name="sender">发起对象</param>
    /// <returns>按需返回</returns>
    public override object Setup(object sender)
    {
      if (SetupForm.Execute(this))
        SendMessage(new MessageNotifyEventArgs(MessageNotifyType.Information, "Setup", String.Format("reset interval = {0} minutes", ResetInterval)));
      return this;
    }

    #endregion

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
    private void Execute()
    {
      try
      {
        while (!Disposing)
          try
          {
            if (!Setuping && DateTime.Now.Minute % MIN_RESET_INTERVAL_MINUTES == 0)
              switch (State)
              {
                case PluginState.Started:
                  if (ResetInterval >= MIN_RESET_INTERVAL_MINUTES)
                  {
                    int resetCount = DefaultDatabase.ExecuteGet(ExecuteReset);
                    SendMessage(new MessageNotifyEventArgs(MessageNotifyType.Information, "Reset", String.Format("user count = {0}", resetCount)));
                  }
                  break;
                case PluginState.Finalizing:
                  return;
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
            SendMessage(new MessageNotifyEventArgs(MessageNotifyType.Error, "Execute", ex));
            Thread.Sleep(1000);
          }
      }
      finally
      {
        _thread = null;
      }
    }

    private int ExecuteReset(DbTransaction transaction)
    {
      using (DbCommand command = DbCommandHelper.CreateCommand(transaction,
@"update PH_User set US_LoginFailureCount = 0
  where US_LoginFailureCount <> 0 and US_LoginFailure <= :US_LoginFailure"
        ))
      {
        DbCommandHelper.CreateParameter(command, "US_LoginFailure", DateTime.Now.AddMinutes(-ResetInterval));
        return DbCommandHelper.ExecuteNonQuery(command);
      }
    }

    #endregion
  }
}