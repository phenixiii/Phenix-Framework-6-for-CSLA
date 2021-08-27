using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Threading;
using Phenix.Core;
using Phenix.Core.Data;
using Phenix.Core.Dictionary;
using Phenix.Core.Net;

namespace Phenix.Services.Host.Core
{
  /// <summary>
  /// 清理配置库
  /// </summary>
  [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1812:AvoidUninstantiatedInternalClasses")]
  internal class ClearConfigurationLibrary : BaseDisposable<ClearConfigurationLibrary>
  {
    private ClearConfigurationLibrary()
      : base()
    {
      _thread = new Thread(Execute);
      _thread.IsBackground = true;
      _thread.Start();
    }
    
    #region 属性

    private static int? _clearLogDeferMonths;
    /// <summary>
    /// 清理几月前的日志
    /// 缺省为 3
    /// </summary>
    public static int ClearLogDeferMonths
    {
      get { return AppSettings.GetProperty(ref _clearLogDeferMonths, 3); }
      set { AppSettings.SetProperty(ref _clearLogDeferMonths, value); }
    }

    private Thread _thread;

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

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
    private void Execute()
    {
      try
      {
        bool alreadyClear = true;
        while (!Disposing)
          try
          {
            if (DateTime.Now.Day % 2 != 0) //2天执行一次
              alreadyClear = false;
            else if (!alreadyClear)
            {
              DefaultDatabase.Execute(ExecuteRemoveBusinessCodeSerial);
              DefaultDatabase.Execute(ExecuteClearExecuteLog);
              DefaultDatabase.Execute(ExecuteClearMessage);
              ExecuteClearLog();
              alreadyClear = true;
            }
            Thread.Sleep(60 * 60 * 1000); //歇1小时
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
            OnMessageNotify(new MessageNotifyEventArgs(MessageNotifyType.Error, MethodBase.GetCurrentMethod().Name, ex));
            Thread.Sleep(NetConfig.TcpTimedWaitDelay);
          }
      }
      finally
      {
        _thread = null;
      }
    }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
    private void ExecuteRemoveBusinessCodeSerial(DbConnection connection)
    {
      foreach (KeyValuePair<string, BusinessCodeFormat> kvp in DataDictionaryHub.BusinessCodeFormats)
      {
        foreach (BusinessCodeSerialFormat serialFormat in kvp.Value.SerialFormats)
          try
          {
            switch (serialFormat.ResetCycle)
            {
              case BusinessCodeSerialResetCycle.Day:
                for (int i = 1; i <= 31; i++)
                  if (i != DateTime.Now.Day)
                    if (ExecuteRemoveBusinessCodeSerial(connection, serialFormat.AssembleLikeKeyValue(i)))
                    {
                      //OnMessageNotify(new MessageNotifyEventArgs(MessageNotifyType.Information, String.Format("Clearing {0}[{1}] for day", kvp.Key, serialFormat.FormatString), i.ToString()));
                    }
                break;
              case BusinessCodeSerialResetCycle.Month:
                if (DateTime.Now.Day == 1)
                  for (int i = 1; i <= 12; i++)
                    if (i != DateTime.Now.Month)
                      if (ExecuteRemoveBusinessCodeSerial(connection, serialFormat.AssembleLikeKeyValue(i)))
                      {
                        //OnMessageNotify(new MessageNotifyEventArgs(MessageNotifyType.Information, String.Format("Clearing {0}[{1}] for month", kvp.Key, serialFormat.FormatString), i.ToString()));
                      }
                break;
            }
          }
          catch (Exception ex)
          {
            OnMessageNotify(new MessageNotifyEventArgs(MessageNotifyType.Error, String.Format("Clearing {0}[{1}]", kvp.Key, serialFormat.FormatString), ex));
          }
        Thread.Sleep(100);
      }
      OnMessageNotify(new MessageNotifyEventArgs(MessageNotifyType.Error, MethodBase.GetCurrentMethod().Name, Phenix.Services.Host.Properties.Resources.Finished));
    }

    private static bool ExecuteRemoveBusinessCodeSerial(DbConnection connection, string likeKey)
    {
      using (DbCommand command = DbCommandHelper.CreateCommand(connection,
@"delete PH_Serial
  where SR_Key like :SR_Key"))
      {
        DbCommandHelper.CreateParameter(command, "SR_Key", likeKey);
        return DbCommandHelper.ExecuteNonQuery(command, false) >= 1;
      }
    }

    private static void ExecuteClearExecuteLog(DbConnection connection)
    {
      using (DbCommand command = DbCommandHelper.CreateCommand(connection,
@"delete PH_ExecuteLog
  where EL_Time <= :EL_Time"))
      {
        DbCommandHelper.CreateParameter(command, "EL_Time", DateTime.Now.AddMonths(-ClearLogDeferMonths));
        DbCommandHelper.ExecuteNonQuery(command, false);
      }
    }

    private static void ExecuteClearMessage(DbConnection connection)
    {
      using (DbCommand command = DbCommandHelper.CreateCommand(connection,
@"delete PH_Message
  where MS_CreatedTime <= :MS_CreatedTime"))
      {
        DbCommandHelper.CreateParameter(command, "MS_CreatedTime", DateTime.Now.AddMonths(-ClearLogDeferMonths));
        DbCommandHelper.ExecuteNonQuery(command, false);
      }
    }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
    private void ExecuteClearLog()
    {
      foreach (string s in Directory.GetDirectories(AppConfig.TempDirectory))
      {
        string directoryName = Path.GetFileName(s);
        DateTime date;
        if (DateTime.TryParseExact(directoryName, "yyyyMMdd", null, DateTimeStyles.None, out date) && date < DateTime.Now.AddMonths(-ClearLogDeferMonths))
          try
          {
            Directory.Delete(s, true);
          }
          catch (Exception ex)
          {
            OnMessageNotify(new MessageNotifyEventArgs(MessageNotifyType.Error, String.Format("Clearing {0}", directoryName), ex));
          }
      }
    }

    #endregion
  }
}
