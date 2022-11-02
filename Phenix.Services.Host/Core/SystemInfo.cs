using System;
using System.Data;
using System.Data.Common;
using System.IO;
using System.Net;
using System.Reflection;
using System.Threading;
using System.Windows.Forms;
using Phenix.Core;
using Phenix.Core.Data;
using Phenix.Core.Dictionary;
using Phenix.Core.Log;
using Phenix.Core.Net;
using Phenix.Core.Security;
using Phenix.Core.Workflow;

namespace Phenix.Services.Host.Core
{
  internal class SystemInfo : BaseDisposable
  {
    #region 单例

    private SystemInfo()
    {
      _timer = new System.Threading.Timer(new TimerCallback(Register), null, new TimeSpan(24, 0, 0), new TimeSpan(24, 0, 0));
    }

    private static readonly object _defaultLock = new object();
    private static SystemInfo _default;
    public static bool Run(Action<MessageNotifyEventArgs> doMessageNotify)
    {
      if (_default == null)
        lock (_defaultLock)
          if (_default == null)
          {
            _default = new SystemInfo();
            _default._messageNotify = doMessageNotify;
            return _default.DoInitialize();
          }
      return true;
    }
    public static void Stop()
    {
      SystemInfo info = _default;
      if (info != null)
        info.Dispose();
    }

    #endregion

    #region 属性

    private System.Threading.Timer _timer;

    public static Version Version
    {
      get { return Assembly.GetExecutingAssembly().GetName().Version; }
    }

    private static string _enterpriseName;
    public static string EnterpriseName
    {
      get
      {
        if (_enterpriseName == null)
        {
          if (_default != null)
            lock (_defaultLock)
              if (_default != null)
              {
                _enterpriseName = _default.GetEnterpriseName();
              }
        }
        return _enterpriseName;
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
      if (_timer != null)
      {
        _timer.Dispose();
        _timer = null;
      }
      Unregister();
    }

    protected override void DisposeUnmanagedResources()
    {
    }

    #endregion

    public static bool Initialize()
    {
      if (_default != null)
        lock (_defaultLock)
          if (_default != null)
          {
            return _default.DoInitialize();
          }
      return false;
    }

    public static bool Setup()
    {
      if (_default != null)
        lock (_defaultLock)
          if (_default != null)
          {
            return SystemInfoSetupDialog.Execute(EnterpriseName, _default.ChangeEnterpriseName);
          }
      return false;
    }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
    private bool DoInitialize()
    {
      OnMessageNotify(new MessageNotifyEventArgs(MessageNotifyType.Information, Phenix.Services.Host.Properties.Resources.Initialize, "Construct subdirectories..."));
      if (!Directory.Exists(AppConfig.ClientLibrarySubdirectory))
        Directory.CreateDirectory(AppConfig.ClientLibrarySubdirectory);
      if (!Directory.Exists(AppConfig.ClientLibraryOwnSubdirectory))
        Directory.CreateDirectory(AppConfig.ClientLibraryOwnSubdirectory);
      if (!Directory.Exists(AppConfig.ServiceLibrarySubdirectory))
        Directory.CreateDirectory(AppConfig.ServiceLibrarySubdirectory);
      do
      {
        try
        {
          if (!CheckVersion())
            return false;
          SynchronizeClock();
          DefaultDatabase.Execute(ExecuteInitializeHostInfo);
          DataDictionaryHub.ClearCache();
          WorkflowHub.RefreshCache(true);
          return true;
        }
        catch (Exception ex)
        {
          OnMessageNotify(new MessageNotifyEventArgs(MessageNotifyType.Error, Phenix.Services.Host.Properties.Resources.Initialize, ex));
          if (!DbConnectionInfo.Fetch().Setup())
            return false;
        }
      } while (true);
    }

    private bool CheckVersion()
    {
      Version actualVersion = GetActualVersion();
      while (actualVersion == null)
      {
        if (!DbConnectionInfo.Fetch().Setup())
          return false;
        if (DefaultDatabase.ExecuteGet(ExecuteInitializeDatabase))
          actualVersion = GetActualVersion();
      }
      if (Version.CompareTo(actualVersion) > 0)
      {
        if (!DefaultDatabase.ExecuteGet(ExecuteUpgradeDatabase, actualVersion))
          if (MessageBox.Show("To upgrade the database failed, whether to continue to update the version number?",
            DefaultDatabase.DbConnectionInfo.Key, MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
            return false;
        if (!DefaultDatabase.ExecuteGet(ExecuteSetActualVersion, Version))
          return false;
      }
      OnMessageNotify(new MessageNotifyEventArgs(MessageNotifyType.Information, "Check the version", Phenix.Services.Host.Properties.Resources.Finished));
      return true;
    }

    private void SynchronizeClock()
    {
      using (SafeDataReader reader = new SafeDataReader(
@"select sysdate
  from PH_SystemInfo",
        CommandBehavior.SingleRow, false))
      {
        if (reader.Read())
        {
          Phenix.Core.Win32.NativeMethods.SetClock(reader.GetDateTime(0));
          OnMessageNotify(new MessageNotifyEventArgs(MessageNotifyType.Information, "Synchronize clock", Phenix.Services.Host.Properties.Resources.Finished));
        }
        else
          OnMessageNotify(new MessageNotifyEventArgs(MessageNotifyType.Warning, "Synchronize clock", "Because not all PH_SystemInfo data and not prevailed and database."));
      }
    }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
    private void ExecuteInitializeHostInfo(DbConnection connection)
    {
      bool succeed;
      using (DbCommand command = DbCommandHelper.CreateCommand(connection,
@"update PH_HostInfo set
    HI_Active = 1,
    HI_LinkCount = 0,
    HI_ActiveTime = sysdate
  where HI_Name = :HI_Name"))
      {
        DbCommandHelper.CreateParameter(command, "HI_Name", Dns.GetHostName());
        succeed = DbCommandHelper.ExecuteNonQuery(command, false) != 0;
      }
      if (!succeed)
      {
        using (DbCommand command = DbCommandHelper.CreateCommand(connection,
@"insert into PH_HostInfo
  (HI_Address, HI_Name, HI_Active, HI_LinkCount, HI_ActiveTime)
  values
  (:HI_Address, :HI_Name, 1, 0, sysdate)"))
        {
          DbCommandHelper.CreateParameter(command, "HI_Address", NetConfig.LocalAddress);
          DbCommandHelper.CreateParameter(command, "HI_Name", Dns.GetHostName());
          DbCommandHelper.ExecuteNonQuery(command, false);
        }
      }
      OnMessageNotify(new MessageNotifyEventArgs(MessageNotifyType.Information, "Initialize host information", Phenix.Services.Host.Properties.Resources.Finished));
    }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
    private void Register(object stateInfo)
    {
      if (!DbConnectionInfo.Fetch().IsValid(true))
        return;
      _timer.Change(Timeout.Infinite, Timeout.Infinite);
      try
      {
        try
        {
          DefaultDatabase.Execute(ExecuteRegister);
        }
        catch (Exception ex)
        {
          EventLog.SaveLocal(MethodBase.GetCurrentMethod(), ex);
        }
      }
      finally
      {
        _timer.Change(new TimeSpan(24, 0, 0), new TimeSpan(24, 0, 0));
      }
    }

    private void ExecuteRegister(DbConnection connection)
    {
      using (DbCommand command = DbCommandHelper.CreateCommand(connection,
@"update PH_HostInfo set
    HI_Active = 1,
    HI_ActiveTime = sysdate
  where HI_Name = :HI_Name"))
      {
        DbCommandHelper.CreateParameter(command, "HI_Name", Dns.GetHostName());
        DbCommandHelper.ExecuteNonQuery(command, false);
      }
      OnMessageNotify(new MessageNotifyEventArgs(MessageNotifyType.Information, "Register service", "Heartbeat"));
    }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
    private static void Unregister()
    {
      try
      {
        DefaultDatabase.Execute(ExecuteUnregister);
      }
      catch (Exception ex)
      {
        EventLog.SaveLocal(MethodBase.GetCurrentMethod(), ex);
      }
    }

    private static void ExecuteUnregister(DbConnection connection)
    {
      using (DbCommand command = DbCommandHelper.CreateCommand(connection,
@"update PH_HostInfo set
    HI_Active = 0,
    HI_ActiveTime = sysdate
  where HI_Name = :HI_Name"))
      {
        DbCommandHelper.CreateParameter(command, "HI_Name", Dns.GetHostName());
        DbCommandHelper.ExecuteNonQuery(command, false);
      }
    }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
    private Version GetActualVersion()
    {
      try
      {
        using (SafeDataReader reader = new SafeDataReader(
@"select SI_Version
  from PH_SystemInfo
  where SI_Name = :SI_Name",
           CommandBehavior.SingleRow, false))
        {
          reader.CreateParameter("SI_Name", AppConfig.SYSTEM_NAME);
          if (reader.Read())
            return new Version(reader.GetString(0));
        }
      }
      catch (Exception ex)
      {
        OnMessageNotify(new MessageNotifyEventArgs(MessageNotifyType.Error, "Take version number", "May need to construct the configuration database", ex));
      }
      return null;
    }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
    private bool ExecuteSetActualVersion(DbConnection connection, Version value)
    {
      try
      {
        using (DbCommand command = DbCommandHelper.CreateCommand(connection,
@"update PH_SystemInfo set
    SI_Version = :SI_Version
  where SI_Name = :SI_Name"))
        {
          DbCommandHelper.CreateParameter(command, "SI_Version", value.ToString());
          DbCommandHelper.CreateParameter(command, "SI_Name", Phenix.Core.AppConfig.SYSTEM_NAME);
          if (DbCommandHelper.ExecuteNonQuery(command, false) == 1)
          {
            OnMessageNotify(new MessageNotifyEventArgs(MessageNotifyType.Information, "Set version number", Phenix.Services.Host.Properties.Resources.Succeed));
            return true;
          }
          else
            OnMessageNotify(new MessageNotifyEventArgs(MessageNotifyType.Warning, "Set version number", Phenix.Services.Host.Properties.Resources.Failed));
        }
      }
      catch (Exception ex)
      {
        OnMessageNotify(new MessageNotifyEventArgs(MessageNotifyType.Error, "Set version number", ex));
      }
      return false;
    }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
    private string GetEnterpriseName()
    {
      try
      {
        using (SafeDataReader reader = new SafeDataReader(
@"select SI_Enterprise
  from PH_SystemInfo
  where SI_Name = :SI_Name",
           CommandBehavior.SingleRow, false))
        {
          reader.CreateParameter("SI_Name", AppConfig.SYSTEM_NAME);
          if (reader.Read())
            return reader.GetString(0);
        }
      }
      catch (Exception ex)
      {
        OnMessageNotify(new MessageNotifyEventArgs(MessageNotifyType.Error, "Get enterprise name", ex));
      }
      return null;
    }

    private bool ChangeEnterpriseName(string value)
    {
      return DefaultDatabase.ExecuteGet(ExecuteChangeEnterpriseName, value);
    }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
    private bool ExecuteChangeEnterpriseName(DbConnection connection, string value)
    {
      try
      {
        using (DbCommand command = DbCommandHelper.CreateCommand(connection,
@"update PH_SystemInfo set
    SI_Enterprise = :SI_Enterprise
  where SI_Name = :SI_Name"))
        {
          DbCommandHelper.CreateParameter(command, "SI_Enterprise", value);
          DbCommandHelper.CreateParameter(command, "SI_Name", Phenix.Core.AppConfig.SYSTEM_NAME);
          if (DbCommandHelper.ExecuteNonQuery(command, false) == 1)
          {
            _enterpriseName = value;
            OnMessageNotify(new MessageNotifyEventArgs(MessageNotifyType.Information, "Set enterprise name", Phenix.Services.Host.Properties.Resources.Succeed));
            return true;
          }
          else
            OnMessageNotify(new MessageNotifyEventArgs(MessageNotifyType.Warning, "Set enterprise name", Phenix.Services.Host.Properties.Resources.Failed));
        }
      }
      catch (Exception ex)
      {
        OnMessageNotify(new MessageNotifyEventArgs(MessageNotifyType.Error, "Set enterprise name", ex));
      }
      return false;
    }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
    private bool ExecuteInitializeDatabase(DbConnection connection)
    {
      OnMessageNotify(new MessageNotifyEventArgs(MessageNotifyType.Information, Phenix.Services.Host.Properties.Resources.Initialize, "Construct the configuration database"));
      bool result = true;
      try
      {
        if (connection == null)
        {
          OnMessageNotify(new MessageNotifyEventArgs(MessageNotifyType.Warning, "Construct the configuration database",
            String.Format(Phenix.Core.Properties.Resources.CannotConnectTo, DefaultDatabase.DbConnectionInfo.Key)));
          result = false;
        }
        else
        {
          InitializeSystemTables(connection);
          InitializeSecurityTables(connection);
          InitializeRuleTables(connection);
          InitializeRenovateTables(connection);
          InitializeRenovateProcedures(connection);
          InitializeObjectCacheTables(connection);
          InitializeObjectCacheProcedures(connection);
          InitializeWorkflowTables(connection);
          InitializeMessageTables(connection);
          DataDictionaryHub.AllInfoHasChanged();
          WorkflowHub.AllInfoHasChanged();
        }
      }
      catch (Exception ex)
      {
        OnMessageNotify(new MessageNotifyEventArgs(MessageNotifyType.Error, "Construct the configuration database", ex));
        result = false;
      }
      return result;
    }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1809:AvoidExcessiveLocals")]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Maintainability", "CA1505:AvoidUnmantainableCode")]
    private bool ExecuteUpgradeDatabase(DbConnection connection, Version actualVersion)
    {
      bool result = true;
      //升级 v5.0.X.X 以下配置库
      if (actualVersion.Major < 5 ||
        (actualVersion.Major == 5 && actualVersion.Minor <= 0))
        try
        {
          if (connection == null)
          {
            OnMessageNotify(new MessageNotifyEventArgs(MessageNotifyType.Warning, "Upgrade v5.0.X.X the following the configuration database",
              String.Format(Phenix.Core.Properties.Resources.CannotConnectTo, DefaultDatabase.DbConnectionInfo.Key)));
            return false;
          }
          else
          {
            if (Upgrade_5_0(connection))
              OnMessageNotify(new MessageNotifyEventArgs(MessageNotifyType.Information, "Upgrade v5.0.X.X the following the configuration database", Phenix.Services.Host.Properties.Resources.Finished));
            else
              result = false;
          }
        }
        catch (Exception ex)
        {
          OnMessageNotify(new MessageNotifyEventArgs(MessageNotifyType.Error, "Upgrade v5.0.X.X the following the configuration database", ex));
          return false;
        }
      //升级 v5.1.X.X 以下配置库
      if (actualVersion.Major < 5 ||
        (actualVersion.Major == 5 && actualVersion.Minor <= 1))
        try
        {
          if (connection == null)
          {
            OnMessageNotify(new MessageNotifyEventArgs(MessageNotifyType.Warning, "Upgrade v5.1.X.X the following the configuration database",
              String.Format(Phenix.Core.Properties.Resources.CannotConnectTo, DefaultDatabase.DbConnectionInfo.Key)));
            return false;
          }
          else
          {
            if (Upgrade_5_1(connection))
              OnMessageNotify(new MessageNotifyEventArgs(MessageNotifyType.Information, "Upgrade v5.1.X.X the following the configuration database", Phenix.Services.Host.Properties.Resources.Finished));
            else
              result = false;
          }
        }
        catch (Exception ex)
        {
          OnMessageNotify(new MessageNotifyEventArgs(MessageNotifyType.Error, "Upgrade v5.1.X.X the following the configuration database", ex));
          return false;
        }
      //升级 v5.2.X.X 以下配置库
      if (actualVersion.Major < 5 ||
        (actualVersion.Major == 5 && actualVersion.Minor <= 2))
        try
        {
          if (connection == null)
          {
            OnMessageNotify(new MessageNotifyEventArgs(MessageNotifyType.Warning, "Upgrade v5.2.X.X the following the configuration database",
              String.Format(Phenix.Core.Properties.Resources.CannotConnectTo, DefaultDatabase.DbConnectionInfo.Key)));
            return false;
          }
          else
          {
            if (Upgrade_5_2(connection))
              OnMessageNotify(new MessageNotifyEventArgs(MessageNotifyType.Information, "Upgrade v5.2.X.X the following the configuration database", Phenix.Services.Host.Properties.Resources.Finished));
            else
              result = false;
          }
        }
        catch (Exception ex)
        {
          OnMessageNotify(new MessageNotifyEventArgs(MessageNotifyType.Error, "Upgrade v5.2.X.X the following the configuration database", ex));
          return false;
        }
      //升级 v5.3.X.X 以下配置库
      if (actualVersion.Major < 5 ||
        (actualVersion.Major == 5 && actualVersion.Minor <= 3))
        try
        {
          if (connection == null)
          {
            OnMessageNotify(new MessageNotifyEventArgs(MessageNotifyType.Warning, "Upgrade v5.3.X.X the following the configuration database",
              String.Format(Phenix.Core.Properties.Resources.CannotConnectTo, DefaultDatabase.DbConnectionInfo.Key)));
            return false;
          }
          else
          {
            if (Upgrade_5_3(connection))
              OnMessageNotify(new MessageNotifyEventArgs(MessageNotifyType.Information, "Upgrade v5.3.X.X the following the configuration database", Phenix.Services.Host.Properties.Resources.Finished));
            else
              result = false;
          }
        }
        catch (Exception ex)
        {
          OnMessageNotify(new MessageNotifyEventArgs(MessageNotifyType.Error, "Upgrade v5.3.X.X the following the configuration database", ex));
          return false;
        }
      //升级 v5.4.X.X 以下配置库
      if (actualVersion.Major < 5 ||
        (actualVersion.Major == 5 && actualVersion.Minor <= 4))
        try
        {
          if (connection == null)
          {
            OnMessageNotify(new MessageNotifyEventArgs(MessageNotifyType.Warning, "Upgrade v5.4.X.X the following the configuration database",
              String.Format(Phenix.Core.Properties.Resources.CannotConnectTo, DefaultDatabase.DbConnectionInfo.Key)));
            return false;
          }
          else
          {
            if (Upgrade_5_4(connection))
              OnMessageNotify(new MessageNotifyEventArgs(MessageNotifyType.Information, "Upgrade v5.4.X.X the following the configuration database", Phenix.Services.Host.Properties.Resources.Finished));
            else
              result = false;
          }
        }
        catch (Exception ex)
        {
          OnMessageNotify(new MessageNotifyEventArgs(MessageNotifyType.Error, "Upgrade v5.4.X.X the following the configuration database", ex));
          return false;
        }
      //升级 v5.5.X.X 以下配置库
      if (actualVersion.Major < 5 ||
        (actualVersion.Major == 5 && actualVersion.Minor <= 5))
        try
        {
          if (connection == null)
          {
            OnMessageNotify(new MessageNotifyEventArgs(MessageNotifyType.Warning, "Upgrade v5.5.X.X the following the configuration database",
              String.Format(Phenix.Core.Properties.Resources.CannotConnectTo, DefaultDatabase.DbConnectionInfo.Key)));
            return false;
          }
          else
          {
            if (Upgrade_5_5(connection))
              OnMessageNotify(new MessageNotifyEventArgs(MessageNotifyType.Information, "Upgrade v5.5.X.X the following the configuration database", Phenix.Services.Host.Properties.Resources.Finished));
            else
              result = false;
          }
        }
        catch (Exception ex)
        {
          OnMessageNotify(new MessageNotifyEventArgs(MessageNotifyType.Error, "Upgrade v5.5.X.X the following the configuration database", ex));
          return false;
        }
      //升级 v5.6.X.X 以下配置库
      if (actualVersion.Major < 5 ||
        (actualVersion.Major == 5 && actualVersion.Minor <= 6))
        try
        {
          if (connection == null)
          {
            OnMessageNotify(new MessageNotifyEventArgs(MessageNotifyType.Warning, "Upgrade v5.6.X.X the following the configuration database",
              String.Format(Phenix.Core.Properties.Resources.CannotConnectTo, DefaultDatabase.DbConnectionInfo.Key)));
            return false;
          }
          else
          {
            if (Upgrade_5_6(connection))
              OnMessageNotify(new MessageNotifyEventArgs(MessageNotifyType.Information, "Upgrade v5.6.X.X the following the configuration database", Phenix.Services.Host.Properties.Resources.Finished));
            else
              result = false;
          }
        }
        catch (Exception ex)
        {
          OnMessageNotify(new MessageNotifyEventArgs(MessageNotifyType.Error, "Upgrade v5.6.X.X the following the configuration database", ex));
          return false;
        }
      ////升级 v5.7.X.X 以下配置库
      //if (actualVersion.Major < 5 ||
      //  (actualVersion.Major == 5 && actualVersion.Minor <= 7))
      //  try
      //  {
      //    if (connection == null)
      //    {
      //      OnMessageNotify(new MessageNotifyEventArgs(MessageNotifyType.Warning, "Upgrade v5.7.X.X the following the configuration database",
      //        String.Format(Phenix.Core.Properties.Resources.CannotConnectTo, DefaultDatabase.DbConnectionInfo.Key)));
      //      return false;
      //    }
      //    else
      //    {
      //      if (Upgrade_5_7(connection))
      //        OnMessageNotify(new MessageNotifyEventArgs(MessageNotifyType.Information, "Upgrade v5.7.X.X the following the configuration database", Phenix.Services.Host.Properties.Resources.Finished));
      //      else
      //        result = false;
      //    }
      //  }
      //  catch (Exception ex)
      //  {
      //    OnMessageNotify(new MessageNotifyEventArgs(MessageNotifyType.Error, "Upgrade v5.7.X.X the following the configuration database", ex));
      //    return false;
      //  }
      //升级 v5.8.X.X 以下配置库
      if (actualVersion.Major < 5 ||
        (actualVersion.Major == 5 && actualVersion.Minor <= 8))
        try
        {
          if (connection == null)
          {
            OnMessageNotify(new MessageNotifyEventArgs(MessageNotifyType.Warning, "Upgrade v5.8.X.X the following the configuration database",
              String.Format(Phenix.Core.Properties.Resources.CannotConnectTo, DefaultDatabase.DbConnectionInfo.Key)));
            return false;
          }
          else
          {
            if (Upgrade_5_8(connection))
              OnMessageNotify(new MessageNotifyEventArgs(MessageNotifyType.Information, "Upgrade v5.8.X.X the following the configuration database", Phenix.Services.Host.Properties.Resources.Finished));
            else
              result = false;
          }
        }
        catch (Exception ex)
        {
          OnMessageNotify(new MessageNotifyEventArgs(MessageNotifyType.Error, "Upgrade v5.8.X.X the following the configuration database", ex));
          return false;
        }
      //升级 v6.1.X.X 以下配置库
      if (actualVersion.Major < 6 ||
        (actualVersion.Major == 6 && actualVersion.Minor <= 1))
        try
        {
          if (connection == null)
          {
            OnMessageNotify(new MessageNotifyEventArgs(MessageNotifyType.Warning, "Upgrade v6.1.X.X the following the configuration database",
              String.Format(Phenix.Core.Properties.Resources.CannotConnectTo, DefaultDatabase.DbConnectionInfo.Key)));
            return false;
          }
          else
          {
            if (Upgrade_6_1(connection))
              OnMessageNotify(new MessageNotifyEventArgs(MessageNotifyType.Information, "Upgrade v6.1.X.X the following the configuration database", Phenix.Services.Host.Properties.Resources.Finished));
            else
              result = false;
          }
        }
        catch (Exception ex)
        {
          OnMessageNotify(new MessageNotifyEventArgs(MessageNotifyType.Error, "Upgrade v6.1.X.X the following the configuration database", ex));
          return false;
        }
      //升级 v6.3.X.X 以下配置库
      if (actualVersion.Major < 6 ||
        (actualVersion.Major == 6 && actualVersion.Minor <= 3))
        try
        {
          if (connection == null)
          {
            OnMessageNotify(new MessageNotifyEventArgs(MessageNotifyType.Warning, "Upgrade v6.3.X.X the following the configuration database",
              String.Format(Phenix.Core.Properties.Resources.CannotConnectTo, DefaultDatabase.DbConnectionInfo.Key)));
            return false;
          }
          else
          {
            if (Upgrade_6_3(connection))
              OnMessageNotify(new MessageNotifyEventArgs(MessageNotifyType.Information, "Upgrade v6.3.X.X the following the configuration database", Phenix.Services.Host.Properties.Resources.Finished));
            else
              result = false;
          }
        }
        catch (Exception ex)
        {
          OnMessageNotify(new MessageNotifyEventArgs(MessageNotifyType.Error, "Upgrade v6.3.X.X the following the configuration database", ex));
          return false;
        }
      //升级 v6.4.X.X 以下配置库
      if (actualVersion.Major < 6 ||
        (actualVersion.Major == 6 && actualVersion.Minor <= 4))
        try
        {
          if (connection == null)
          {
            OnMessageNotify(new MessageNotifyEventArgs(MessageNotifyType.Warning, "Upgrade v6.4.X.X the following the configuration database",
              String.Format(Phenix.Core.Properties.Resources.CannotConnectTo, DefaultDatabase.DbConnectionInfo.Key)));
            return false;
          }
          else
          {
            if (Upgrade_6_4(connection))
              OnMessageNotify(new MessageNotifyEventArgs(MessageNotifyType.Information, "Upgrade v6.4.X.X the following the configuration database", Phenix.Services.Host.Properties.Resources.Finished));
            else
              result = false;
          }
        }
        catch (Exception ex)
        {
          OnMessageNotify(new MessageNotifyEventArgs(MessageNotifyType.Error, "Upgrade v6.4.X.X the following the configuration database", ex));
          return false;
        }
      //升级 v6.5.X.X 以下配置库
      if (actualVersion.Major < 6 ||
        (actualVersion.Major == 6 && actualVersion.Minor <= 5))
        try
        {
          if (connection == null)
          {
            OnMessageNotify(new MessageNotifyEventArgs(MessageNotifyType.Warning, "Upgrade v6.5.X.X the following the configuration database",
              String.Format(Phenix.Core.Properties.Resources.CannotConnectTo, DefaultDatabase.DbConnectionInfo.Key)));
            return false;
          }
          else
          {
            if (Upgrade_6_5())
              OnMessageNotify(new MessageNotifyEventArgs(MessageNotifyType.Information, "Upgrade v6.5.X.X the following the configuration database", Phenix.Services.Host.Properties.Resources.Finished));
            else
              result = false;
          }
        }
        catch (Exception ex)
        {
          OnMessageNotify(new MessageNotifyEventArgs(MessageNotifyType.Error, "Upgrade v6.5.X.X the following the configuration database", ex));
          return false;
        }
      //升级 v6.6.X.X 以下配置库
      if (actualVersion.Major < 6 ||
        (actualVersion.Major == 6 && actualVersion.Minor <= 6))
        try
        {
          if (connection == null)
          {
            OnMessageNotify(new MessageNotifyEventArgs(MessageNotifyType.Warning, "Upgrade v6.6.X.X the following the configuration database",
              String.Format(Phenix.Core.Properties.Resources.CannotConnectTo, DefaultDatabase.DbConnectionInfo.Key)));
            return false;
          }
          else
          {
            if (Upgrade_6_6(connection))
              OnMessageNotify(new MessageNotifyEventArgs(MessageNotifyType.Information, "Upgrade v6.6.X.X the following the configuration database", Phenix.Services.Host.Properties.Resources.Finished));
            else
              result = false;
          }
        }
        catch (Exception ex)
        {
          OnMessageNotify(new MessageNotifyEventArgs(MessageNotifyType.Error, "Upgrade v6.6.X.X the following the configuration database", ex));
          return false;
        }
      //升级 v6.7.X.X 以下配置库
      if (actualVersion.Major < 6 ||
        (actualVersion.Major == 6 && actualVersion.Minor <= 7))
        try
        {
          if (connection == null)
          {
            OnMessageNotify(new MessageNotifyEventArgs(MessageNotifyType.Warning, "Upgrade v6.7.X.X the following the configuration database",
              String.Format(Phenix.Core.Properties.Resources.CannotConnectTo, DefaultDatabase.DbConnectionInfo.Key)));
            return false;
          }
          else
          {
            if (Upgrade_6_7(connection))
              OnMessageNotify(new MessageNotifyEventArgs(MessageNotifyType.Information, "Upgrade v6.7.X.X the following the configuration database", Phenix.Services.Host.Properties.Resources.Finished));
            else
              result = false;
          }
        }
        catch (Exception ex)
        {
          OnMessageNotify(new MessageNotifyEventArgs(MessageNotifyType.Error, "Upgrade v6.7.X.X the following the configuration database", ex));
          return false;
        }
      //升级 v6.8.X.X 以下配置库
      if (actualVersion.Major < 6 ||
        (actualVersion.Major == 6 && actualVersion.Minor <= 8))
        try
        {
          if (connection == null)
          {
            OnMessageNotify(new MessageNotifyEventArgs(MessageNotifyType.Warning, "Upgrade v6.8.X.X the following the configuration database",
              String.Format(Phenix.Core.Properties.Resources.CannotConnectTo, DefaultDatabase.DbConnectionInfo.Key)));
            return false;
          }
          else
          {
            if (Upgrade_6_8(connection))
              OnMessageNotify(new MessageNotifyEventArgs(MessageNotifyType.Information, "Upgrade v6.8.X.X the following the configuration database", Phenix.Services.Host.Properties.Resources.Finished));
            else
              result = false;
          }
        }
        catch (Exception ex)
        {
          OnMessageNotify(new MessageNotifyEventArgs(MessageNotifyType.Error, "Upgrade v6.8.X.X the following the configuration database", ex));
          return false;
        }
      //升级 v6.9.X.X 以下配置库
      if (actualVersion.Major < 6 ||
        (actualVersion.Major == 6 && actualVersion.Minor <= 9))
        try
        {
          if (connection == null)
          {
            OnMessageNotify(new MessageNotifyEventArgs(MessageNotifyType.Warning, "Upgrade v6.9.X.X the following the configuration database",
              String.Format(Phenix.Core.Properties.Resources.CannotConnectTo, DefaultDatabase.DbConnectionInfo.Key)));
            return false;
          }
          else
          {
            if (Upgrade_6_9(connection))
              OnMessageNotify(new MessageNotifyEventArgs(MessageNotifyType.Information, "Upgrade v6.9.X.X the following the configuration database", Phenix.Services.Host.Properties.Resources.Finished));
            else
              result = false;
          }
        }
        catch (Exception ex)
        {
          OnMessageNotify(new MessageNotifyEventArgs(MessageNotifyType.Error, "Upgrade v6.9.X.X the following the configuration database", ex));
          return false;
        }
      //升级 v6.10.X.X 以下配置库
      if (actualVersion.Major < 6 ||
        (actualVersion.Major == 6 && actualVersion.Minor <= 10))
        try
        {
          if (connection == null)
          {
            OnMessageNotify(new MessageNotifyEventArgs(MessageNotifyType.Warning, "Upgrade v6.10.X.X the following the configuration database",
              String.Format(Phenix.Core.Properties.Resources.CannotConnectTo, DefaultDatabase.DbConnectionInfo.Key)));
            return false;
          }
          else
          {
            if (Upgrade_6_10(connection))
              OnMessageNotify(new MessageNotifyEventArgs(MessageNotifyType.Information, "Upgrade v6.10.X.X the following the configuration database", Phenix.Services.Host.Properties.Resources.Finished));
            else
              result = false;
          }
        }
        catch (Exception ex)
        {
          OnMessageNotify(new MessageNotifyEventArgs(MessageNotifyType.Error, "Upgrade v6.10.X.X the following the configuration database", ex));
          return false;
        }
      ////升级 v6.11.X.X 以下配置库
      //if (actualVersion.Major < 6 ||
      //  (actualVersion.Major == 6 && actualVersion.Minor <= 11))
      //  try
      //  {
      //    if (connection == null)
      //    {
      //      OnMessageNotify(new MessageNotifyEventArgs(MessageNotifyType.Warning, "Upgrade v6.11.X.X the following the configuration database",
      //        String.Format(Phenix.Core.Properties.Resources.CannotConnectTo, DefaultDatabase.DbConnectionInfo.Key)));
      //      return false;
      //    }
      //    else
      //    {
      //      if (Upgrade_6_11(connection))
      //        OnMessageNotify(new MessageNotifyEventArgs(MessageNotifyType.Information, "Upgrade v6.11.X.X the following the configuration database", Phenix.Services.Host.Properties.Resources.Finished));
      //      else
      //        result = false;
      //    }
      //  }
      //  catch (Exception ex)
      //  {
      //    OnMessageNotify(new MessageNotifyEventArgs(MessageNotifyType.Error, "Upgrade v6.11.X.X the following the configuration database", ex));
      //    return false;
      //  }
      return result;
    }

    /// <summary>
    /// 升级 v5.0.X.X 以下配置库
    /// 2013-09-13
    /// </summary>
    /// <param name="connection">数据库连接</param>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
    private bool Upgrade_5_0(DbConnection connection)
    {
      bool result = true;
      try
      {
        DbCommandHelper.ExecuteNonQuery(connection,
          "CREATE TABLE PH_Workflow (" +
          "WF_ID NUMERIC(15) NOT NULL," +
          "WF_Namespace VARCHAR(255) NOT NULL," +
          "WF_TypeName VARCHAR(255) NOT NULL," +
          "WF_Caption VARCHAR(4000) NULL," +
          "WF_XamlCode LONG /*TEXT*/ NULL," +
          "WF_Create_UserNumber VARCHAR(10) NOT NULL," +
          "WF_Create_Time DATE NOT NULL," +
          "WF_Change_UserNumber VARCHAR(10) NULL," +
          "WF_Change_Time DATE NULL," +
          "WF_Disable_UserNumber VARCHAR(10) NULL," +
          "WF_Disable_Time DATE NULL," +
          "PRIMARY KEY(WF_ID)," +
          "UNIQUE(WF_Namespace, WF_TypeName))", false);
      }
      catch (Exception ex)
      {
        OnMessageNotify(new MessageNotifyEventArgs(MessageNotifyType.Error, "Construct the PH_Workflow", "If already building can be omitted the tip", ex));
        result = false;
      }
      try
      {
        DbCommandHelper.ExecuteNonQuery(connection,
          "ALTER TABLE PH_SystemInfo ADD " +
          "SI_WorkflowInfoChangedTime DATE NULL", false);
      }
      catch (Exception ex)
      {
        OnMessageNotify(new MessageNotifyEventArgs(MessageNotifyType.Error, "Reconstruct the PH_SystemInfo", "If already building can be omitted the tip", ex));
        result = false;
      }
      try
      {
        WorkflowHub.WorkflowInfoHasChanged();
        OnMessageNotify(new MessageNotifyEventArgs(MessageNotifyType.Information, "Set ChangedTime", Phenix.Services.Host.Properties.Resources.Succeed));
      }
      catch (Exception)
      {
        OnMessageNotify(new MessageNotifyEventArgs(MessageNotifyType.Warning, "Set ChangedTime", Phenix.Services.Host.Properties.Resources.Failed));
        result = false;
      }
      return result;
    }

    /// <summary>
    /// 升级 v5.1.X.X 以下配置库
    /// 2013-10-24
    /// </summary>
    /// <param name="connection">数据库连接</param>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
    private bool Upgrade_5_1(DbConnection connection)
    {
      bool result = true;
      try
      {
        DbCommandHelper.ExecuteNonQuery(connection,
          "ALTER TABLE PH_SystemInfo ADD " +
          "SI_EmptyRolesIsDeny NUMERIC(1) default 0 NOT NULL", false);
      }
      catch (Exception ex)
      {
        OnMessageNotify(new MessageNotifyEventArgs(MessageNotifyType.Error, "Reconstruct the PH_SystemInfo", "If already building can be omitted the tip", ex));
        result = false;
      }
      return result;
    }

    /// <summary>
    /// 升级 v5.2.X.X 以下配置库
    /// 2013-10-25
    /// </summary>
    /// <param name="connection">数据库连接</param>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
    private bool Upgrade_5_2(DbConnection connection)
    {
      bool result = true;
      try
      {
        DbCommandHelper.ExecuteNonQuery(connection,
          "ALTER TABLE PH_AssemblyClass ADD " +
          "AC_Authorised NUMERIC(1) default 0 NOT NULL", false);
      }
      catch (Exception ex)
      {
        OnMessageNotify(new MessageNotifyEventArgs(MessageNotifyType.Error, "Reconstruct the PH_AssemblyClass", "If already building can be omitted the tip", ex));
        result = false;
      }
      try
      {
        DbCommandHelper.ExecuteNonQuery(connection,
          "update PH_AssemblyClass set" +
          " AC_Authorised = 1" +
          " where AC_Type = 1 or AC_Type = 9", false); //Phenix.Core.Dictionary.AssemblyClassType.Form/ApiController
      }
      catch (Exception ex)
      {
        OnMessageNotify(new MessageNotifyEventArgs(MessageNotifyType.Error, "Reconstruct the PH_AssemblyClass", "If already building can be omitted the tip", ex));
        result = false;
      }
      return result;
    }

    /// <summary>
    /// 升级 v5.3.X.X 以下配置库
    /// 2013-11-08
    /// </summary>
    /// <param name="connection">数据库连接</param>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
    private bool Upgrade_5_3(DbConnection connection)
    {
      bool result = true;
      try
      {
        DbCommandHelper.ExecuteNonQuery(connection,
          "ALTER TABLE PH_User ADD " +
          "US_PasswordChangedTime DATE NULL", false);
      }
      catch (Exception ex)
      {
        OnMessageNotify(new MessageNotifyEventArgs(MessageNotifyType.Error, "Reconstruct the PH_User", "If already building can be omitted the tip", ex));
        result = false;
      }
      try
      {
        DbCommandHelper.ExecuteNonQuery(connection,
          "update PH_User set" +
          " US_PasswordChangedTime = sysdate", false); 
      }
      catch (Exception ex)
      {
        OnMessageNotify(new MessageNotifyEventArgs(MessageNotifyType.Error, "Reconstruct the PH_User", "If already building can be omitted the tip", ex));
        result = false;
      }
      return result;
    }

    /// <summary>
    /// 升级 v5.4.X.X 以下配置库
    /// 2014-01-23
    /// </summary>
    /// <param name="connection">数据库连接</param>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
    private bool Upgrade_5_4(DbConnection connection)
    {
      bool result = true;
      try
      {
        DbCommandHelper.ExecuteNonQuery(connection,
          "CREATE TABLE PH_Workflow_Instance (" +
          "WI_ID VARCHAR(36) NOT NULL," +
          "WI_WF_Namespace VARCHAR(255) NOT NULL," +
          "WI_WF_TypeName VARCHAR(255) NOT NULL," +
          "WI_Content LONG /*TEXT*/ NULL," +
          "WI_Time DATE NOT NULL," +
          "PRIMARY KEY(WI_ID))", false);
      }
      catch (Exception ex)
      {
        OnMessageNotify(new MessageNotifyEventArgs(MessageNotifyType.Error, "Construct the PH_Workflow_Instance", "If already building can be omitted the tip", ex));
        result = false;
      }
      try
      {
        DbCommandHelper.ExecuteNonQuery(connection,
          "CREATE TABLE PH_Workflow_TaskContext (" +
          "WC_WI_ID VARCHAR(36) NOT NULL," +
          "WC_Worker_UserNumber VARCHAR(10) NULL," +
          "WC_Content LONG /*TEXT*/ NULL," +
          "WC_Time DATE NOT NULL," +
          "PRIMARY KEY(WC_WI_ID))", false);
      }
      catch (Exception ex)
      {
        OnMessageNotify(new MessageNotifyEventArgs(MessageNotifyType.Error, "Construct the PH_Workflow_TaskContext", "If already building can be omitted the tip", ex));
        result = false;
      }
      try
      {
        DbCommandHelper.ExecuteNonQuery(connection,
          "CREATE TABLE PH_Workflow_Task (" +
          "WT_WI_ID VARCHAR(36) NOT NULL," +
          "WT_BookmarkName VARCHAR(255) NOT NULL," +
          "WT_Plugin_AssemblyName VARCHAR(255) NOT NULL," +
          "WT_Worker_RL_Name VARCHAR(100) NULL," +
          "WT_Caption VARCHAR(4000) NULL," +
          "WT_Message VARCHAR(4000) NULL," +
          "WT_Urgent NUMERIC(1) default 0 NOT NULL," +
          "WT_State NUMERIC(5) default 0 NOT NULL," +
          "WT_Dispatch_Time DATE NOT NULL," +
          "WT_Receive_Time DATE NULL," +
          "WT_Hold_Time DATE NULL," +
          "WT_Abortive_Time DATE NULL," +
          "WT_Complete_Time DATE NULL," +
          "PRIMARY KEY(WT_WI_ID, WT_BookmarkName))", false);
      }
      catch (Exception ex)
      {
        OnMessageNotify(new MessageNotifyEventArgs(MessageNotifyType.Error, "Construct the PH_Workflow_Task", "If already building can be omitted the tip", ex));
        result = false;
      }
      return result;
    }

    /// <summary>
    /// 升级 v5.5.X.X 以下配置库
    /// 2014-11-09
    /// </summary>
    /// <param name="connection">数据库连接</param>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
    private bool Upgrade_5_5(DbConnection connection)
    {
      bool result = true;
      try
      {
        DbCommandHelper.ExecuteNonQuery(connection,
          "DROP PROCEDURE PH_Clear_ObjectCache", false);
      }
      catch (Exception ex)
      {
        OnMessageNotify(new MessageNotifyEventArgs(MessageNotifyType.Error, "DROP PROCEDURE PH_Clear_ObjectCache", "If already building can be omitted the tip", ex));
      }
      try
      {
        DbCommandHelper.ExecuteNonQuery(connection,
          "DROP PROCEDURE PH_Record_Has_Changed", false);
      }
      catch (Exception ex)
      {
        OnMessageNotify(new MessageNotifyEventArgs(MessageNotifyType.Error, "DROP PROCEDURE PH_Record_Has_Changed", "If already building can be omitted the tip", ex));
      }
      InitializeObjectCacheProcedures(connection);
      return result;
    }
    
    /// <summary>
    /// 升级 v5.6.X.X 以下配置库
    /// 2014-12-08
    /// </summary>
    /// <param name="connection">数据库连接</param>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
    private bool Upgrade_5_6(DbConnection connection)
    {
      bool result = true;
      try
      {
        DbCommandHelper.ExecuteNonQuery(connection,
          "ALTER TABLE PH_AssemblyClassMethod ADD " +
          "AM_AM_ID NUMERIC(15) NULL", false);
      }
      catch (Exception ex)
      {
        OnMessageNotify(new MessageNotifyEventArgs(MessageNotifyType.Error, "Reconstruct the PH_AssemblyClassMethod", "If already building can be omitted the tip", ex));
        result = false;
      }
      return result;
    }

    ///// <summary>
    ///// 升级 v5.7.X.X 以下配置库
    ///// 2014-12-12
    ///// </summary>
    ///// <param name="connection">数据库连接</param>
    //[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
    //private bool Upgrade_5_7(DbConnection connection)
    //{
    //  bool result = true;
    //  try
    //  {
    //    DbCommandHelper.ExecuteNonQuery(connection,
    //      "ALTER TABLE PH_AssemblyClassMethod ADD " +
    //      "AM_AllowVisible NUMERIC(1) NULL", false);
    //  }
    //  catch (Exception ex)
    //  {
    //    OnMessageNotify(new MessageNotifyEventArgs(MessageNotifyType.Error, "Reconstruct the PH_AssemblyClassMethod", "If already building can be omitted the tip", ex));
    //    result = false;
    //  }
    //  return result;
    //}

    /// <summary>
    /// 升级 v5.8.X.X 以下配置库
    /// 2015-07-30
    /// </summary>
    /// <param name="connection">数据库连接</param>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
    private bool Upgrade_5_8(DbConnection connection)
    {
      bool result = true;
      try
      {
        DbCommandHelper.ExecuteNonQuery(connection,
          "ALTER TABLE PH_Department ADD " +
          "DP_In_Headquarters NUMERIC(1) NULL", false);
      }
      catch (Exception ex)
      {
        OnMessageNotify(new MessageNotifyEventArgs(MessageNotifyType.Error, "Reconstruct the PH_Department", "If already building can be omitted the tip", ex));
        result = false;
      }
      return result;
    }

    /// <summary>
    /// 升级 v6.1.X.X 以下配置库
    /// 2016-10-20
    /// </summary>
    /// <param name="connection">数据库连接</param>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
    private bool Upgrade_6_1(DbConnection connection)
    {
      bool result = true;
      try
      {
        DbCommandHelper.ExecuteNonQuery(connection,
          "ALTER TABLE PH_ExecuteActionLog ADD " +
          "EA_BusinessPrimaryKey VARCHAR(4000) NULL", false);
      }
      catch (Exception ex)
      {
        OnMessageNotify(new MessageNotifyEventArgs(MessageNotifyType.Error, "Reconstruct the PH_ExecuteActionLog", "If already building can be omitted the tip", ex));
        result = false;
      }
      return result;
    }

    /// <summary>
    /// 升级 v6.3.X.X 以下配置库
    /// 2017-05-26
    /// </summary>
    /// <param name="connection">数据库连接</param>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
    private bool Upgrade_6_3(DbConnection connection)
    {
      bool result = true;
      try
      {
        DbCommandHelper.ExecuteNonQuery(connection,
          "ALTER TABLE PH_CriteriaExpression ADD " +
          "CE_Caption VARCHAR(4000) NULL", false);
      }
      catch (Exception ex)
      {
        OnMessageNotify(new MessageNotifyEventArgs(MessageNotifyType.Error, "Reconstruct the PH_CriteriaExpression", "If already building can be omitted the tip", ex));
        result = false;
      }
      return result;
    }

    /// <summary>
    /// 升级 v6.4.X.X 以下配置库
    /// 2017-06-27
    /// </summary>
    /// <param name="connection">数据库连接</param>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
    private bool Upgrade_6_4(DbConnection connection)
    {
      bool result = true;
      try
      {
        DbCommandHelper.ExecuteNonQuery(connection,
          "CREATE TABLE PH_AssemblyClassMethod_Departm (" +
          "AD_ID NUMERIC(15) NOT NULL," +
          "AD_AM_ID NUMERIC(15) NOT NULL," +
          "AD_DP_ID NUMERIC(15) NOT NULL," +
          "PRIMARY KEY(AD_ID)," +
          "UNIQUE(AD_AM_ID, AD_DP_ID))", false);
      }
      catch (Exception ex)
      {
        OnMessageNotify(new MessageNotifyEventArgs(MessageNotifyType.Error, "Construct the PH_AssemblyClassMethod_Departm", "If already building can be omitted the tip", ex));
        result = false;
      }
      return result;
    }

    /// <summary>
    /// 升级 v6.5.X.X 以下配置库
    /// 2017-08-17
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
    private bool Upgrade_6_5()
    {
      bool result = true;
      try
      {
        DefaultDatabase.Execute(DataSecurityHub.AddUser, UserIdentity.GuestId, UserIdentity.GuestUserName, UserIdentity.GuestUserNumber, UserIdentity.GuestUserNumber);
      }
      catch (Exception ex)
      {
        OnMessageNotify(new MessageNotifyEventArgs(MessageNotifyType.Error, "New PH_User guest", "If already building can be omitted the tip", ex));
        result = false;
      }
      return result;
    }

    /// <summary>
    /// 升级 v6.6.X.X 以下配置库
    /// 2017-11-22
    /// </summary>
    /// <param name="connection">数据库连接</param>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
    private bool Upgrade_6_6(DbConnection connection)
    {
      bool result = true;
      try
      {
        DbCommandHelper.ExecuteNonQuery(connection,
          "CREATE TABLE PH_AssemblyClass_Department (" +
          "AD_ID NUMERIC(15) NOT NULL," +
          "AD_AC_ID NUMERIC(15) NOT NULL," +
          "AD_DP_ID NUMERIC(15) NOT NULL," +
          "PRIMARY KEY(AD_ID)," +
          "UNIQUE(AD_AC_ID, AD_DP_ID))", false);
      }
      catch (Exception ex)
      {
        OnMessageNotify(new MessageNotifyEventArgs(MessageNotifyType.Error, "Construct the PH_AssemblyClass_Department", "If already building can be omitted the tip", ex));
        result = false;
      }
      return result;
    }

    /// <summary>
    /// 升级 v6.7.X.X 以下配置库
    /// 2017-12-12
    /// </summary>
    /// <param name="connection">数据库连接</param>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
    private bool Upgrade_6_7(DbConnection connection)
    {
      bool result = true;
      try
      {
        DbCommandHelper.ExecuteNonQuery(connection,
          "ALTER TABLE PH_SystemInfo ADD " +
          "SI_EasyAuthorization NUMERIC(1) default 1 NOT NULL", false);
      }
      catch (Exception ex)
      {
        OnMessageNotify(new MessageNotifyEventArgs(MessageNotifyType.Error, "Reconstruct the PH_SystemInfo", "If already building can be omitted the tip", ex));
        result = false;
      }
      return result;
    }

    /// <summary>
    /// 升级 v6.8.X.X 以下配置库
    /// 2018-07-27
    /// </summary>
    /// <param name="connection">数据库连接</param>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
    private bool Upgrade_6_8(DbConnection connection)
    {
      bool result = true;
      try
      {
        DbCommandHelper.ExecuteNonQuery(connection,
          "ALTER TABLE PH_User ADD " +
          "US_LastOperationTime DATE NULL", false);
      }
      catch (Exception ex)
      {
        OnMessageNotify(new MessageNotifyEventArgs(MessageNotifyType.Error, "Reconstruct the PH_User", "If already building can be omitted the tip", ex));
        result = false;
      }
      try
      {
        DbCommandHelper.ExecuteNonQuery(connection,
          "update PH_User set" +
          " US_LastOperationTime = sysdate", false);
      }
      catch (Exception ex)
      {
        OnMessageNotify(new MessageNotifyEventArgs(MessageNotifyType.Error, "Reconstruct the PH_User", "If already building can be omitted the tip", ex));
        result = false;
      }
      return result;
    }

    /// <summary>
    /// 升级 v6.9.X.X 以下配置库
    /// 2018-08-30
    /// </summary>
    /// <param name="connection">数据库连接</param>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
    private bool Upgrade_6_9(DbConnection connection)
    {
      bool result = true;
      try
      {
        DbCommandHelper.ExecuteNonQuery(connection,
          "CREATE TABLE PH_Message(" +
          "MS_ID NUMERIC(15) NOT NULL," +
          "MS_Send_UserNumber VARCHAR(10) NOT NULL," +
          "MS_Receive_UserNumber VARCHAR(10) NOT NULL," +
          "MS_CreatedTime DATE NOT NULL," +
          "MS_SendedTime DATE NULL," +
          "MS_ReceivedTime DATE NULL," +
          "MS_Content LONG /*TEXT*/ NULL," +
          "PRIMARY KEY(MS_ID))", false);
      }
      catch (Exception ex)
      {
        OnMessageNotify(new MessageNotifyEventArgs(MessageNotifyType.Error, "Construct the PH_Message", "If already building can be omitted the tip", ex));
        result = false;
      }
      try
      {
        DbCommandHelper.ExecuteNonQuery(connection,
          "CREATE INDEX I_MS_Receive_UserNumber on PH_Message(MS_Receive_UserNumber, MS_CreatedTime)", false);
      }
      catch (Exception ex)
      {
        OnMessageNotify(new MessageNotifyEventArgs(MessageNotifyType.Error, "Construct the I_MS_Receive_UserNumber", "If already building can be omitted the tip", ex));
        result = false;
      }
      return result;
    }

    /// <summary>
    /// 升级 v6.10.X.X 以下配置库
    /// 2018-10-16
    /// </summary>
    /// <param name="connection">数据库连接</param>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
    private bool Upgrade_6_10(DbConnection connection)
    {
      bool result = true;
      try
      {
        DbCommandHelper.ExecuteNonQuery(connection,
          "CREATE TABLE PH_SequenceMarker (" +
          "SM_ID NUMERIC(3) default 0 NOT NULL," +
          "SM_HostAddress VARCHAR(39) NOT NULL," +
          "SM_HostName VARCHAR(255) NOT NULL," +
          "SM_ActiveTime DATE NOT NULL," +
          "PRIMARY KEY(SM_ID)," +
          "UNIQUE(SM_HostAddress))", false);
      }
      catch (Exception ex)
      {
        OnMessageNotify(new MessageNotifyEventArgs(MessageNotifyType.Error, "Construct the PH_SequenceMarker", "If already building can be omitted the tip", ex));
        result = false;
      }
      return result;
    }

    ///// <summary>
    ///// 升级 v6.11.X.X 以下配置库
    ///// 2018-12-18
    ///// </summary>
    ///// <param name="connection">数据库连接</param>
    //[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
    //private bool Upgrade_6_11(DbConnection connection)
    //{
    //  try
    //  {
    //    DbCommandHelper.ExecuteNonQuery(connection,
    //      "ALTER TABLE PH_AssemblyClassProperty ADD AP_TableName VARCHAR(30) NULL", false);
    //    DbCommandHelper.ExecuteNonQuery(connection,
    //      "ALTER TABLE PH_AssemblyClassProperty ADD AP_ColumnName VARCHAR(30) NULL", false);
    //    DbCommandHelper.ExecuteNonQuery(connection,
    //      "ALTER TABLE PH_AssemblyClassProperty ADD AP_Alias VARCHAR(30) NULL", false);
    //  }
    //  catch (Exception ex)
    //  {
    //    OnMessageNotify(new MessageNotifyEventArgs(MessageNotifyType.Error, "Construct the PH_AssemblyClassProperty", "If already building can be omitted the tip", ex));
    //  }
    //  return true;
    //}

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
    private void InitializeSystemTables(DbConnection connection)
    {
      try
      {
        switch (DatabaseConverter.GetDatabaseType(connection))
        {
          case DatabaseType.Oracle:
            DbCommandHelper.ExecuteNonQuery(connection,
              "create sequence SEQ_PH", false);
            break;
        }
      }
      catch (Exception ex)
      {
        OnMessageNotify(new MessageNotifyEventArgs(MessageNotifyType.Error, "Construct the SEQ_PH", "If already building can be omitted the tip", ex));
      }
      try
      {
        DbCommandHelper.ExecuteNonQuery(connection,
          "CREATE TABLE PH_SystemInfo (" +
          "SI_Name VARCHAR(30) NOT NULL," +
          "SI_Version VARCHAR(30) NOT NULL," +
          "SI_Enterprise VARCHAR(255) NOT NULL," +
          "SI_AssemblyInfoChangedTime DATE NULL," +
          "SI_TableFilterInfoChangedTime DATE NULL," +
          "SI_RoleInfoChangedTime DATE NULL," +
          "SI_SectionInfoChangedTime DATE NULL," +
          "SI_DepartmentInfoChangedTime DATE NULL," +
          "SI_PositionInfoChangedTime DATE NULL," +
          "SI_TableInfoChangedTime DATE NULL," +
          "SI_WorkflowInfoChangedTime DATE NULL," +
          "SI_EmptyRolesIsDeny NUMERIC(1) default 0 NOT NULL," +
          "SI_EasyAuthorization NUMERIC(1) default 1 NOT NULL," +
          "PRIMARY KEY(SI_Name))", false);
      }
      catch (Exception ex)
      {
        OnMessageNotify(new MessageNotifyEventArgs(MessageNotifyType.Error, "Construct the PH_SystemInfo", "If already building can be omitted the tip", ex));
      }
      try
      {
        DbCommandHelper.ExecuteNonQuery(connection,
          "insert into PH_SystemInfo" +
          "(SI_Name, SI_Version, SI_Enterprise, SI_AssemblyInfoChangedTime, SI_TableFilterInfoChangedTime, SI_RoleInfoChangedTime, SI_SectionInfoChangedTime, SI_DepartmentInfoChangedTime, SI_PositionInfoChangedTime, SI_TableInfoChangedTime, SI_WorkflowInfoChangedTime)" +
          "values" +
          "('" + AppConfig.SYSTEM_NAME + "','" + Version + "','Please fill out the enterprise name', sysdate, sysdate, sysdate, sysdate, sysdate, sysdate, sysdate, sysdate)", false);
      }
      catch (Exception ex)
      {
        OnMessageNotify(new MessageNotifyEventArgs(MessageNotifyType.Error, "New PH_SystemInfo system records", "If already building can be omitted the tip", ex));
      }
      try
      {
        DbCommandHelper.ExecuteNonQuery(connection,
          "CREATE TABLE PH_HostInfo (" +
          "HI_Address VARCHAR(39) NOT NULL," +
          "HI_Name VARCHAR(255) NOT NULL," +
          "HI_Active NUMERIC(1) default 0 NOT NULL," +
          "HI_ActiveTime DATE NOT NULL," +
          "HI_LinkCount NUMERIC(15) default 0 NOT NULL," +
          "PRIMARY KEY(HI_Address, HI_Name))", false);
      }
      catch (Exception ex)
      {
        OnMessageNotify(new MessageNotifyEventArgs(MessageNotifyType.Error, "Construct the PH_HostInfo", "If already building can be omitted the tip", ex));
      }
      try
      {
        DbCommandHelper.ExecuteNonQuery(connection,
          "CREATE INDEX I_PH_HostInfo on PH_HostInfo (" +
          "HI_ActiveTime DESC," +
          "HI_LinkCount ASC)", false);
      }
      catch (Exception ex)
      {
        OnMessageNotify(new MessageNotifyEventArgs(MessageNotifyType.Error, "Construct the I_PH_HostInfo", "If already building can be omitted the tip", ex));
      }
      try
      {
        DbCommandHelper.ExecuteNonQuery(connection,
          "CREATE TABLE PH_SequenceMarker (" +
          "SM_ID NUMERIC(3) default 0 NOT NULL," +
          "SM_HostAddress VARCHAR(39) NOT NULL," +
          "SM_HostName VARCHAR(255) NOT NULL," +
          "SM_ActiveTime DATE NOT NULL," +
          "PRIMARY KEY(SM_ID)," +
          "UNIQUE(SM_HostAddress))", false);
      }
      catch (Exception ex)
      {
        OnMessageNotify(new MessageNotifyEventArgs(MessageNotifyType.Error, "Construct the PH_SequenceMarker", "If already building can be omitted the tip", ex));
      }
    }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Maintainability", "CA1505:AvoidUnmantainableCode")]
    private void InitializeSecurityTables(DbConnection connection)
    {
      try
      {
        DbCommandHelper.ExecuteNonQuery(connection,
          "CREATE TABLE PH_Department (" +
          "DP_ID NUMERIC(15) NOT NULL," +
          "DP_DP_ID NUMERIC(15) NULL," +
          "DP_Name VARCHAR(100) NOT NULL," +
          "DP_Code VARCHAR(10) NOT NULL," +
          "DP_PT_ID NUMERIC(15) NULL," +
          "DP_In_Headquarters NUMERIC(1) NULL," +
          "PRIMARY KEY(DP_ID)," +
          "UNIQUE(DP_DP_ID, DP_Code)," +
          "UNIQUE(DP_DP_ID, DP_Name))", false);
      }
      catch (Exception ex)
      {
        OnMessageNotify(new MessageNotifyEventArgs(MessageNotifyType.Error, "Construct the PH_Department", "If already building can be omitted the tip", ex));
      }
      try
      {
        DbCommandHelper.ExecuteNonQuery(connection,
          "CREATE TABLE PH_Position (" +
          "PT_ID NUMERIC(15) NOT NULL," +
          "PT_PT_ID NUMERIC(15) NULL," +
          "PT_Name VARCHAR(100) NOT NULL," +
          "PT_Code VARCHAR(10) NOT NULL," +
          "PRIMARY KEY(PT_ID)," +
          "UNIQUE(PT_PT_ID, PT_Code)," +
          "UNIQUE(PT_PT_ID, PT_Name))", false);
      }
      catch (Exception ex)
      {
        OnMessageNotify(new MessageNotifyEventArgs(MessageNotifyType.Error, "Construct the PH_Position", "If already building can be omitted the tip", ex));
      }
      try
      {
        DbCommandHelper.ExecuteNonQuery(connection,
          "CREATE TABLE PH_User (" +
          "US_ID NUMERIC(15) NOT NULL," +
          "US_UserNumber VARCHAR(10) NOT NULL," +
          "US_Password VARCHAR(100) NOT NULL," +
          "US_PasswordChangedTime DATE NULL," +
          "US_Name VARCHAR(100) NOT NULL," +
          "US_Login DATE NULL," +
          "US_Logout DATE NULL," +
          "US_LoginFailure DATE NULL," +
          "US_LoginFailureCount NUMERIC(2) default 0 NOT NULL," +
          "US_LoginAddress VARCHAR(39) NULL," +
          "US_LastOperationTime DATE NULL," +
          "US_DP_ID NUMERIC(15) NULL," +
          "US_PT_ID NUMERIC(15) NULL," +
          "US_Locked NUMERIC(1) default 0 NOT NULL," +
          "PRIMARY KEY(US_ID)," +
          "UNIQUE(US_UserNumber))", false);
      }
      catch (Exception ex)
      {
        OnMessageNotify(new MessageNotifyEventArgs(MessageNotifyType.Error, "Construct the PH_User", "If already building can be omitted the tip", ex));
      }
      try
      {
        DbCommandHelper.ExecuteNonQuery(connection,
          "CREATE TABLE PH_UserLog (" +
          "US_ID NUMERIC(15) NOT NULL," +
          "US_UserNumber VARCHAR(10) NOT NULL," +
          "US_Login DATE NULL," +
          "US_Logout DATE NULL," +
          "US_LoginAddress VARCHAR(39) NULL)", false);
      }
      catch (Exception ex)
      {
        OnMessageNotify(new MessageNotifyEventArgs(MessageNotifyType.Error, "Construct the PH_UserLog", "If already building can be omitted the tip", ex));
      }
      try
      {
        DbCommandHelper.ExecuteNonQuery(connection,
          "CREATE TABLE PH_Section (" +
          "ST_ID NUMERIC(15) NOT NULL," +
          "ST_Name VARCHAR(100) NOT NULL," +
          "ST_Caption VARCHAR(100) NULL," +
          "PRIMARY KEY(ST_ID)," +
          "UNIQUE(ST_Name))", false);
      }
      catch (Exception ex)
      {
        OnMessageNotify(new MessageNotifyEventArgs(MessageNotifyType.Error, "Construct the PH_Section", "If already building can be omitted the tip", ex));
      }
      try
      {
        DbCommandHelper.ExecuteNonQuery(connection,
          "CREATE TABLE PH_User_Section (" +
          "US_ID NUMERIC(15) NOT NULL," +
          "US_US_ID NUMERIC(15) NOT NULL," +
          "US_ST_ID NUMERIC(15) NOT NULL," +
          "US_Inputer VARCHAR(10) NULL," +
          "US_InputTime DATE NULL," +
          "PRIMARY KEY(US_ID)," +
          "UNIQUE(US_US_ID, US_ST_ID))", false);
      }
      catch (Exception ex)
      {
        OnMessageNotify(new MessageNotifyEventArgs(MessageNotifyType.Error, "Construct the PH_User_Section", "If already building can be omitted the tip", ex));
      }
      try
      {
        DbCommandHelper.ExecuteNonQuery(connection,
          "CREATE TABLE PH_TableFilter (" +
          "TF_ID NUMERIC(15) NOT NULL," +
          "TF_Name VARCHAR(30) NOT NULL," +
          "TF_Caption VARCHAR(100) NULL," +
          "TF_Compare_ColumnName VARCHAR(30) NOT NULL," +
          "TF_Friendly_ColumnName VARCHAR(30) NOT NULL," +
          "TF_NoneSectionIsDeny NUMERIC(1) default 0 NOT NULL," +
          "PRIMARY KEY(TF_ID)," +
          "UNIQUE(TF_Name, TF_Compare_ColumnName))", false);
      }
      catch (Exception ex)
      {
        OnMessageNotify(new MessageNotifyEventArgs(MessageNotifyType.Error, "Construct the PH_TableFilter", "If already building can be omitted the tip", ex));
      }
      try
      {
        DbCommandHelper.ExecuteNonQuery(connection,
          "CREATE TABLE PH_Section_TableFilter (" +
          "ST_ID NUMERIC(15) NOT NULL," +
          "ST_ST_ID NUMERIC(15) NOT NULL," +
          "ST_TF_ID NUMERIC(15) NOT NULL," +
          "ST_Friendly_ColumnValue VARCHAR(2000) NULL," +
          "ST_AllowRead_ColumnValue VARCHAR(2000) NULL," +
          "ST_AllowEdit NUMERIC(1) default 1 NOT NULL," +
          "PRIMARY KEY(ST_ID)," +
          "UNIQUE(ST_ST_ID, ST_TF_ID, ST_AllowRead_ColumnValue))", false);
      }
      catch (Exception ex)
      {
        OnMessageNotify(new MessageNotifyEventArgs(MessageNotifyType.Error, "Construct the PH_Section_TableFilter", "If already building can be omitted the tip", ex));
      }
      try
      {
        DbCommandHelper.ExecuteNonQuery(connection,
          "CREATE TABLE PH_Role (" +
          "RL_ID NUMERIC(15) NOT NULL," +
          "RL_Name VARCHAR(100) NOT NULL," +
          "RL_Caption VARCHAR(100) NULL," +
          "PRIMARY KEY(RL_ID)," +
          "UNIQUE(RL_Name))", false);
      }
      catch (Exception ex)
      {
        OnMessageNotify(new MessageNotifyEventArgs(MessageNotifyType.Error, "Construct the PH_Role", "If already building can be omitted the tip", ex));
      }
      try
      {
        DbCommandHelper.ExecuteNonQuery(connection,
          "CREATE TABLE PH_User_Role (" +
          "UR_ID NUMERIC(15) NOT NULL," +
          "UR_US_ID NUMERIC(15) NOT NULL," +
          "UR_RL_ID NUMERIC(15) NOT NULL," +
          "UR_Inputer VARCHAR(10) NULL," +
          "UR_InputTime DATE NULL," +
          "PRIMARY KEY(UR_ID)," +
          "UNIQUE(UR_US_ID, UR_RL_ID))", false);
      }
      catch (Exception ex)
      {
        OnMessageNotify(new MessageNotifyEventArgs(MessageNotifyType.Error, "Construct the PH_User_Role", "If already building can be omitted the tip", ex));
      }
      try
      {
        DbCommandHelper.ExecuteNonQuery(connection,
          "CREATE TABLE PH_User_Grant_Role (" +
          "GR_ID NUMERIC(15) NOT NULL," +
          "GR_US_ID NUMERIC(15) NOT NULL," +
          "GR_RL_ID NUMERIC(15) NOT NULL," +
          "GR_Inputer VARCHAR(10) NULL," +
          "GR_InputTime DATE NULL," +
          "PRIMARY KEY(GR_ID)," +
          "UNIQUE(GR_US_ID, GR_RL_ID))", false);
      }
      catch (Exception ex)
      {
        OnMessageNotify(new MessageNotifyEventArgs(MessageNotifyType.Error, "Construct the PH_User_Grant_Role", "If already building can be omitted the tip", ex));
      }
      try
      {
        DbCommandHelper.ExecuteNonQuery(connection,
          "insert into PH_Role" +
          "(RL_ID, RL_Name, RL_Caption)" +
          "values" +
          "(" + UserIdentity.AdminId + ",'" + UserIdentity.AdminRoleName + "','" + UserIdentity.AdminRoleName + "')", false);
      }
      catch (Exception ex)
      {
        OnMessageNotify(new MessageNotifyEventArgs(MessageNotifyType.Error, "New PH_Role administrator role", "If already building can be omitted the tip", ex));
      }
      try
      {
        DefaultDatabase.Execute(DataSecurityHub.AddUser, UserIdentity.AdminId, UserIdentity.AdminUserName, UserIdentity.AdminUserNumber, UserIdentity.AdminUserNumber);
      }
      catch (Exception ex)
      {
        OnMessageNotify(new MessageNotifyEventArgs(MessageNotifyType.Error, "New PH_User administrator", "If already building can be omitted the tip", ex));
      }
      try
      {
        DbCommandHelper.ExecuteNonQuery(connection,
          "insert into PH_User_Role" +
          "(UR_ID, UR_US_ID, UR_RL_ID)" +
          "values" +
          "(" + UserIdentity.AdminId + "," + UserIdentity.AdminId + "," + UserIdentity.AdminId + ")", false);
      }
      catch (Exception ex)
      {
        OnMessageNotify(new MessageNotifyEventArgs(MessageNotifyType.Error, "New PH_User_Role administrator and role association", "If already building can be omitted the tip", ex));
      }
      try
      {
        DefaultDatabase.Execute(DataSecurityHub.AddUser, UserIdentity.GuestId, UserIdentity.GuestUserName, UserIdentity.GuestUserNumber, UserIdentity.GuestUserNumber);
      }
      catch (Exception ex)
      {
        OnMessageNotify(new MessageNotifyEventArgs(MessageNotifyType.Error, "New PH_User guest", "If already building can be omitted the tip", ex));
      }
      try
      {
        DbCommandHelper.ExecuteNonQuery(connection,
          "CREATE TABLE PH_Assembly (" +
          "AS_ID NUMERIC(15) NOT NULL," +
          "AS_Name VARCHAR(255) NOT NULL," +
          "AS_Caption VARCHAR(100) NULL," +
          "PRIMARY KEY(AS_ID)," +
          "UNIQUE(AS_Name))", false);
      }
      catch (Exception ex)
      {
        OnMessageNotify(new MessageNotifyEventArgs(MessageNotifyType.Error, "Construct the PH_Assembly", "If already building can be omitted the tip", ex));
      }
      try
      {
        DbCommandHelper.ExecuteNonQuery(connection,
          "CREATE TABLE PH_AssemblyClass (" +
          "AC_ID NUMERIC(15) NOT NULL," +
          "AC_AS_ID NUMERIC(15) NOT NULL," +
          "AC_Name VARCHAR(255) NOT NULL," +
          "AC_Caption VARCHAR(100) NULL," +
          //"AC_CaptionConfigured NUMERIC(1) default 0 NOT NULL," +
          //"AC_PermanentExecuteAction NUMERIC(5) default 0 NOT NULL," +
          //"AC_PermanentExecuteConfigured NUMERIC(1) default 0 NOT NULL," +
          "AC_Type NUMERIC(5) default 0 NOT NULL," +
          "AC_Authorised NUMERIC(1) default 0 NOT NULL," +
          "PRIMARY KEY(AC_ID)," +
          "UNIQUE(AC_AS_ID, AC_Name))", false);
      }
      catch (Exception ex)
      {
        OnMessageNotify(new MessageNotifyEventArgs(MessageNotifyType.Error, "Construct the PH_AssemblyClass", "If already building can be omitted the tip", ex));
      }
      try
      {
        DbCommandHelper.ExecuteNonQuery(connection,
          "CREATE TABLE PH_AssemblyClass_Group (" +
          "AG_ID NUMERIC(15) NOT NULL," +
          "AG_AC_ID NUMERIC(15) NOT NULL," +
          "AG_Name VARCHAR(30) NOT NULL," +
          "PRIMARY KEY(AG_ID)," +
          "UNIQUE(AG_AC_ID, AG_Name))", false);
      }
      catch (Exception ex)
      {
        OnMessageNotify(new MessageNotifyEventArgs(MessageNotifyType.Error, "Construct the PH_AssemblyClass_Group", "If already building can be omitted the tip", ex));
      }
      try
      {
        DbCommandHelper.ExecuteNonQuery(connection,
          "CREATE TABLE PH_AssemblyClass_Role (" +
          "AR_ID NUMERIC(15) NOT NULL," +
          "AR_AC_ID NUMERIC(15) NOT NULL," +
          "AR_RL_ID NUMERIC(15) NOT NULL," +
          "AR_AllowCreate NUMERIC(1) default 1 NOT NULL," +
          "AR_AllowEdit NUMERIC(1) default 1 NOT NULL," +
          "AR_AllowDelete NUMERIC(1) default 1 NOT NULL," +
          "PRIMARY KEY(AR_ID)," +
          "UNIQUE(AR_AC_ID, AR_RL_ID))", false);
      }
      catch (Exception ex)
      {
        OnMessageNotify(new MessageNotifyEventArgs(MessageNotifyType.Error, "Construct the PH_AssemblyClass_Role", "If already building can be omitted the tip", ex));
      }
      try
      {
        DbCommandHelper.ExecuteNonQuery(connection,
          "CREATE TABLE PH_AssemblyClass_Department (" +
          "AD_ID NUMERIC(15) NOT NULL," +
          "AD_AC_ID NUMERIC(15) NOT NULL," +
          "AD_DP_ID NUMERIC(15) NOT NULL," +
          "PRIMARY KEY(AD_ID)," +
          "UNIQUE(AD_AC_ID, AD_DP_ID))", false);
      }
      catch (Exception ex)
      {
        OnMessageNotify(new MessageNotifyEventArgs(MessageNotifyType.Error, "Construct the PH_AssemblyClass_Department", "If already building can be omitted the tip", ex));
      }
      try
      {
        DbCommandHelper.ExecuteNonQuery(connection,
          "CREATE TABLE PH_AssemblyClassProperty (" +
          "AP_ID NUMERIC(15) NOT NULL," +
          "AP_AC_ID NUMERIC(15) NOT NULL," +
          "AP_Name VARCHAR(255) NOT NULL," +
          "AP_Caption VARCHAR(100) NULL," +
          //"AP_CaptionConfigured NUMERIC(1) default 0 NOT NULL," +
          //"AP_TableName VARCHAR(30) NULL," +
          //"AP_ColumnName VARCHAR(30) NULL," +
          //"AP_Alias VARCHAR(30) NULL," +
          //"AP_PermanentExecuteModify NUMERIC(5) default 7 NOT NULL," +
          //"AP_PermanentExecuteConfigured NUMERIC(1) default 0 NOT NULL," +
          "AP_Configurable NUMERIC(1) default 0 NOT NULL," +
          "AP_ConfigValue VARCHAR(2000) NULL," +
          //"AP_IndexNumber NUMERIC(5) default -1 NOT NULL," +
          //"AP_Required NUMERIC(1) NULL," +
          //"AP_Visible NUMERIC(1) default 1 NOT NULL," +
          "PRIMARY KEY(AP_ID)," +
          "UNIQUE(AP_AC_ID, AP_Name))", false);
      }
      catch (Exception ex)
      {
        OnMessageNotify(new MessageNotifyEventArgs(MessageNotifyType.Error, "Construct the PH_AssemblyClassProperty", "If already building can be omitted the tip", ex));
      }
      try
      {
        DbCommandHelper.ExecuteNonQuery(connection,
          "CREATE TABLE PH_AssemblyClassProperty_Value (" +
          "AV_ID NUMERIC(15) NOT NULL," +
          "AV_AP_ID NUMERIC(15) NOT NULL," +
          "AV_ConfigKey VARCHAR(2000) NOT NULL," +
          "AV_Configurable NUMERIC(1) default 1 NOT NULL," +
          "AV_ConfigValue VARCHAR(2000) NULL," +
          "PRIMARY KEY(AV_ID)," +
          "UNIQUE(AV_AP_ID, AV_ConfigKey))", false);
      }
      catch (Exception ex)
      {
        OnMessageNotify(new MessageNotifyEventArgs(MessageNotifyType.Error, "Reconstruct the PH_AssemblyClassProperty_Value", "If already building can be omitted the tip", ex));
      }
      try
      {
        DbCommandHelper.ExecuteNonQuery(connection,
          "CREATE TABLE PH_AssemblyClassProperty_Role (" +
          "AR_ID NUMERIC(15) NOT NULL," +
          "AR_AP_ID NUMERIC(15) NOT NULL," +
          "AR_RL_ID NUMERIC(15) NOT NULL," +
          "AR_AllowWrite NUMERIC(1) default 1 NOT NULL," +
          "PRIMARY KEY(AR_ID)," +
          "UNIQUE(AR_AP_ID, AR_RL_ID))", false);
      }
      catch (Exception ex)
      {
        OnMessageNotify(new MessageNotifyEventArgs(MessageNotifyType.Error, "Construct the PH_AssemblyClassProperty_Role", "If already building can be omitted the tip", ex));
      }
      try
      {
        DbCommandHelper.ExecuteNonQuery(connection,
          "CREATE TABLE PH_AssemblyClassMethod (" +
           "AM_ID NUMERIC(15) NOT NULL," +
           "AM_AC_ID NUMERIC(15) NOT NULL," +
           "AM_Name VARCHAR(255) NOT NULL," +
           "AM_Caption VARCHAR(100) NULL," +
           //"AM_CaptionConfigured NUMERIC(1) default 0 NOT NULL," +
           //"AM_Tag VARCHAR(4000) NULL," +
           //"AM_AllowVisible NUMERIC(1) NULL," +
           "AM_AM_ID NUMERIC(15) NULL," +
           "PRIMARY KEY(AM_ID)," +
           "UNIQUE(AM_AC_ID, AM_Name))", false);
      }
      catch (Exception ex)
      {
        OnMessageNotify(new MessageNotifyEventArgs(MessageNotifyType.Error, "Construct the PH_AssemblyClassMethod", "If already building can be omitted the tip", ex));
      }
      try
      {
        DbCommandHelper.ExecuteNonQuery(connection,
          "CREATE TABLE PH_AssemblyClassMethod_Role (" +
          "AR_ID NUMERIC(15) NOT NULL," +
          "AR_AM_ID NUMERIC(15) NOT NULL," +
          "AR_RL_ID NUMERIC(15) NOT NULL," +
          "PRIMARY KEY(AR_ID)," +
          "UNIQUE(AR_AM_ID, AR_RL_ID))", false);
      }
      catch (Exception ex)
      {
        OnMessageNotify(new MessageNotifyEventArgs(MessageNotifyType.Error, "Construct the PH_AssemblyClassMethod_Role", "If already building can be omitted the tip", ex));
      }
      try
      {
        DbCommandHelper.ExecuteNonQuery(connection,
          "CREATE TABLE PH_AssemblyClassMethod_Departm (" +
          "AD_ID NUMERIC(15) NOT NULL," +
          "AD_AM_ID NUMERIC(15) NOT NULL," +
          "AD_DP_ID NUMERIC(15) NOT NULL," +
          "PRIMARY KEY(AD_ID)," +
          "UNIQUE(AD_AM_ID, AD_DP_ID))", false);
      }
      catch (Exception ex)
      {
        OnMessageNotify(new MessageNotifyEventArgs(MessageNotifyType.Error, "Construct the PH_AssemblyClassMethod_Departm", "If already building can be omitted the tip", ex));
      }
      try
      {
        DbCommandHelper.ExecuteNonQuery(connection,
          "CREATE TABLE PH_ExecuteLog (" +
          "EL_ID NUMERIC(15) NOT NULL," +
          "EL_Time DATE NOT NULL," +
          "EL_UserNumber VARCHAR(10) NOT NULL," +
          "EL_BusinessName VARCHAR(255) NOT NULL," +
          "EL_Message VARCHAR(4000) NULL," +
          "EL_ExceptionName VARCHAR(255) NULL," +
          "EL_ExceptionMessage VARCHAR(4000) NULL," +
          "PRIMARY KEY(EL_ID))", false);
      }
      catch (Exception ex)
      {
        OnMessageNotify(new MessageNotifyEventArgs(MessageNotifyType.Error, "Construct the PH_ExecuteLog", "If already building can be omitted the tip", ex));
      }
      try
      {
        DbCommandHelper.ExecuteNonQuery(connection,
          "CREATE TABLE PH_ExecuteActionLog (" +
          "EA_ID NUMERIC(15) NOT NULL," +
          "EA_Time DATE NOT NULL," +
          "EA_UserNumber VARCHAR(10) NOT NULL," +
          "EA_BusinessName VARCHAR(255) NOT NULL," +
          "EA_BusinessPrimaryKey VARCHAR(4000) NULL," +
          "EA_Action NUMERIC(5) NOT NULL," +
          "EA_Log LONG /*TEXT*/ NULL," +
          "PRIMARY KEY(EA_ID))", false);
      }
      catch (Exception ex)
      {
        OnMessageNotify(new MessageNotifyEventArgs(MessageNotifyType.Error, "Construct the PH_ExecuteActionLog", "If already building can be omitted the tip", ex));
      }
      try
      {
        DbCommandHelper.ExecuteNonQuery(connection,
          "CREATE INDEX I_PH_EA_Time on PH_ExecuteActionLog (EA_Time)", false);
      }
      catch (Exception ex)
      {
        OnMessageNotify(new MessageNotifyEventArgs(MessageNotifyType.Error, "Construct the I_PH_ExecuteActionLog", "If already building can be omitted the tip", ex));
      }
      try
      {
        DbCommandHelper.ExecuteNonQuery(connection,
          "CREATE TABLE PH_ProcessLock (" +
          "PL_Name VARCHAR(255) NOT NULL," +
          "PL_Caption VARCHAR(1000) NULL," +
          "PL_AllowExecute NUMERIC(1) default 1 NOT NULL," +
          "PL_Time DATE NOT NULL," +
          "PL_ExpiryTime DATE NULL," +
          "PL_UserNumber VARCHAR(10) NOT NULL," +
          "PL_Remark VARCHAR(4000) NULL," +
          "PRIMARY KEY(PL_Name))", false);
      }
      catch (Exception ex)
      {
        OnMessageNotify(new MessageNotifyEventArgs(MessageNotifyType.Error, "Construct the PH_ProcessLock", "If already building can be omitted the tip", ex));
      }
      try
      {
        DbCommandHelper.ExecuteNonQuery(connection,
          "CREATE TABLE PH_BusinessCode (" +
          "BC_Name VARCHAR(4000) NOT NULL," +
          "BC_Caption VARCHAR(100) NULL," +
          "BC_FormatString VARCHAR(4000) NOT NULL," +
          "BC_FillOnSaving NUMERIC(1) default 0 NOT NULL," +
          "PRIMARY KEY(BC_Name))", false);
      }
      catch (Exception ex)
      {
        OnMessageNotify(new MessageNotifyEventArgs(MessageNotifyType.Error, "Construct the PH_BusinessCode", "If already building can be omitted the tip", ex));
      }
      try
      {
        DbCommandHelper.ExecuteNonQuery(connection,
          "CREATE TABLE PH_Serial (" +
          "SR_Key VARCHAR(4000) NOT NULL," +
          "SR_Value NUMERIC(15) NOT NULL," +
          "SR_Time DATE NOT NULL," +
          "PRIMARY KEY(SR_Key))", false);
      }
      catch (Exception ex)
      {
        OnMessageNotify(new MessageNotifyEventArgs(MessageNotifyType.Error, "Construct the PH_Serial", "If already building can be omitted the tip", ex));
      }
    }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
    private void InitializeRuleTables(DbConnection connection)
    {
      try
      {
        DbCommandHelper.ExecuteNonQuery(connection,
          "CREATE TABLE PH_PromptCode (" +
          "PC_Name VARCHAR(61) NOT NULL," +
          "PC_Key VARCHAR(30) NOT NULL," +
          "PC_Caption VARCHAR(4000) NOT NULL," +
          "PC_Value VARCHAR(30) NOT NULL," +
          "PC_ReadLevel NUMERIC(5) default 0 NOT NULL," +
          "PC_Addtime DATE NOT NULL," +
          "PC_UserNumber VARCHAR(10) NULL," +
          "PC_DP_ID NUMERIC(15) NULL," +
          "PC_PT_ID NUMERIC(15) NULL," +
          "PRIMARY KEY(PC_Name, PC_Key))", false);
      }
      catch (Exception ex)
      {
        OnMessageNotify(new MessageNotifyEventArgs(MessageNotifyType.Error, "Construct the PH_PromptCode", "If already building can be omitted the tip", ex));
      }
      try
      {
        DbCommandHelper.ExecuteNonQuery(connection,
          "CREATE TABLE PH_PromptCode_Action (" +
          "PC_Name VARCHAR(61) NOT NULL," +
          "PC_ActionTime DATE NOT NULL," +
          "PRIMARY KEY(PC_Name))", false);
      }
      catch (Exception ex)
      {
        OnMessageNotify(new MessageNotifyEventArgs(MessageNotifyType.Error, "Construct the PH_PromptCode_Action", "If already building can be omitted the tip", ex));
      }
      try
      {
        DbCommandHelper.ExecuteNonQuery(connection,
          "CREATE TABLE PH_CriteriaExpression (" +
          "CE_Name VARCHAR(255) NOT NULL," +
          "CE_Key VARCHAR(30) NOT NULL," +
          "CE_Caption VARCHAR(4000) NULL," +
          "CE_Tree LONG /*TEXT*/ NULL," +
          "CE_ReadLevel NUMERIC(5) default 0 NOT NULL," +
          "CE_Addtime DATE NOT NULL," +
          "CE_UserNumber VARCHAR(10) NULL," +
          "CE_DP_ID NUMERIC(15) NULL," +
          "CE_PT_ID NUMERIC(15) NULL," +
          "PRIMARY KEY(CE_Name, CE_Key))", false);
      }
      catch (Exception ex)
      {
        OnMessageNotify(new MessageNotifyEventArgs(MessageNotifyType.Error, "Construct the PH_CriteriaExpression", "If already building can be omitted the tip", ex));
      }
      try
      {
        DbCommandHelper.ExecuteNonQuery(connection,
          "CREATE TABLE PH_CriteriaExpression_Action (" +
          "CE_Name VARCHAR(255) NOT NULL," +
          "CE_ActionTime DATE NOT NULL," +
          "PRIMARY KEY(CE_Name))", false);
      }
      catch (Exception ex)
      {
        OnMessageNotify(new MessageNotifyEventArgs(MessageNotifyType.Error, "Construct the PH_CriteriaExpression_Action", "If already building can be omitted the tip", ex));
      }
    }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
    private void InitializeRenovateTables(DbConnection connection)
    {
      try
      {
        DbCommandHelper.ExecuteNonQuery(connection,
          "CREATE TABLE PH_RenovateLog (" +
          "RL_TableName VARCHAR(30) NOT NULL," +
          "RL_ROWID VARCHAR(18) NOT NULL," +
          "RL_Time DATE NOT NULL," +
          "RL_Action NUMERIC(5) NOT NULL)", false);
      }
      catch (Exception ex)
      {
        OnMessageNotify(new MessageNotifyEventArgs(MessageNotifyType.Error, "Construct the PH_RenovateLog", "If already building can be omitted the tip", ex));
      }
      try
      {
        DbCommandHelper.ExecuteNonQuery(connection,
          "CREATE INDEX I_PH_RL_Time on PH_RenovateLog (RL_Time)", false);
      }
      catch (Exception ex)
      {
        OnMessageNotify(new MessageNotifyEventArgs(MessageNotifyType.Error, "Construct the I_PH_RL_Time", "If already building can be omitted the tip", ex));
      }
    }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
    private void InitializeRenovateProcedures(DbConnection connection)
    {
      string sql = null;
      switch (DatabaseConverter.GetDatabaseType(connection))
      {
        case DatabaseType.Oracle:
          sql =
@"CREATE PROCEDURE PH_Record_Has_Inserted(i_TableName VARCHAR, i_ROWID VARCHAR) as
begin
  insert into PH_RenovateLog
    (RL_TableName, RL_ROWID, RL_Time, RL_Action)
    values
     (i_TableName, i_ROWID, sysdate, 1);
end;";
          break;
        case DatabaseType.MSSql:
          sql = null;
          break;
        default:
          sql = null;
          break;
      }
      if (!String.IsNullOrEmpty(sql))
        try
        {
          DbCommandHelper.ExecuteNonQuery(connection, sql, false);
        }
        catch (Exception ex)
        {
          OnMessageNotify(new MessageNotifyEventArgs(MessageNotifyType.Error, "Construct the PH_Record_Has_Inserted", ex));
        }
      switch (DatabaseConverter.GetDatabaseType(connection))
      {
        case DatabaseType.Oracle:
          sql =
@"CREATE PROCEDURE PH_Record_Has_Updated(i_TableName VARCHAR, i_ROWID VARCHAR) as
begin
  insert into PH_RenovateLog
    (RL_TableName, RL_ROWID, RL_Time, RL_Action)
    values
     (i_TableName, i_ROWID, sysdate, 2);
end;";
          break;
        case DatabaseType.MSSql:
          sql = null;
          break;
        default:
          sql = null;
          break;
      }
      if (!String.IsNullOrEmpty(sql))
        try
        {
          DbCommandHelper.ExecuteNonQuery(connection, sql, false);
        }
        catch (Exception ex)
        {
          OnMessageNotify(new MessageNotifyEventArgs(MessageNotifyType.Error, "Construct the PH_Record_Has_Updated", ex));
        }
      switch (DatabaseConverter.GetDatabaseType(connection))
      {
        case DatabaseType.Oracle:
          sql =
@"CREATE PROCEDURE PH_Record_Has_Deleted(i_TableName VARCHAR, i_ROWID VARCHAR) as
begin
  insert into PH_RenovateLog
    (RL_TableName, RL_ROWID, RL_Time, RL_Action)
    values
     (i_TableName, i_ROWID, sysdate, 3);
end;";
          break;
        case DatabaseType.MSSql:
          sql = null;
          break;
        default:
          sql = null;
          break;
      }
      if (!String.IsNullOrEmpty(sql))
        try
        {
          DbCommandHelper.ExecuteNonQuery(connection, sql, false);
        }
        catch (Exception ex)
        {
          OnMessageNotify(new MessageNotifyEventArgs(MessageNotifyType.Error, "Construct the PH_Record_Has_Deleted", ex));
        }
    }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
    private void InitializeObjectCacheTables(DbConnection connection)
    {
      try
      {
        DbCommandHelper.ExecuteNonQuery(connection,
          "CREATE TABLE PH_CacheAction (" +
          "CA_ClassName VARCHAR(255) NOT NULL," +
          "CA_ActionTime DATE NOT NULL," +
          "PRIMARY KEY(CA_ClassName))", false);
      }
      catch (Exception ex)
      {
        OnMessageNotify(new MessageNotifyEventArgs(MessageNotifyType.Error, "Construct the PH_CacheAction", "If already building can be omitted the tip", ex));
      }
    }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
    private void InitializeObjectCacheProcedures(DbConnection connection)
    {
      string sql = null;
      switch (DatabaseConverter.GetDatabaseType(connection))
      {
        case DatabaseType.Oracle:
          sql =
@"CREATE PROCEDURE PH_Clear_ObjectCache(i_ClassName VARCHAR) as
begin
  update PH_CacheAction set CA_ActionTime = sysdate
  where CA_ClassName in 
    (select distinct(D.AC_Name)
      from PH_AssemblyClass A, PH_AssemblyClass_Group B, PH_AssemblyClass_Group C, PH_AssemblyClass D 
      where i_ClassName = A.AC_Name and A.AC_ID = B.AG_AC_ID
      and B.AG_Name = C.AG_Name and C.AG_AC_ID = D.AC_ID);
end;";
          break;
        case DatabaseType.MSSql:
          sql =
@"CREATE PROCEDURE PH_Clear_ObjectCache @i_ClassName varchar(255) as
begin
  update PH_CacheAction set CA_ActionTime = getdate() 
  where CA_ClassName in 
    (select distinct(D.AC_Name)
      from PH_AssemblyClass A, PH_AssemblyClass_Group B, PH_AssemblyClass_Group C, PH_AssemblyClass D 
      where @i_ClassName = A.AC_Name and A.AC_ID = B.AG_AC_ID
      and B.AG_Name = C.AG_Name and C.AG_AC_ID = D.AC_ID);
end;";
          break;
        default:
          sql = null;
          break;
      }
      if (!String.IsNullOrEmpty(sql))
        try
        {
          DbCommandHelper.ExecuteNonQuery(connection, sql, false);
        }
        catch (Exception ex)
        {
          OnMessageNotify(new MessageNotifyEventArgs(MessageNotifyType.Error, "Construct the PH_Clear_ObjectCache", ex));
        }
      switch (DatabaseConverter.GetDatabaseType(connection))
      {
        case DatabaseType.Oracle:
          sql =
@"CREATE PROCEDURE PH_Record_Has_Changed(i_TableName VARCHAR) as
begin
  update PH_CacheAction set CA_ActionTime = sysdate
  where CA_ClassName in 
    (select distinct(A.AC_Name)
      from PH_AssemblyClass A, PH_AssemblyClass_Group B 
      where A.AC_ID = B.AG_AC_ID and upper(B.AG_Name) = upper(i_TableName));
end;";
          break;
        case DatabaseType.MSSql:
          sql =
@"CREATE PROCEDURE PH_Record_Has_Changed @i_TableName varchar(30) as
begin
  update PH_CacheAction set CA_ActionTime = getdate()
  where CA_ClassName in 
    (select distinct(A.AC_Name)
      from PH_AssemblyClass A, PH_AssemblyClass_Group B 
      where A.AC_ID = B.AG_AC_ID and upper(B.AG_Name) = upper(@i_TableName));
end;";
          break;
        default:
          sql = null;
          break;
      }
      if (!String.IsNullOrEmpty(sql))
        try
        {
          DbCommandHelper.ExecuteNonQuery(connection, sql, false);
        }
        catch (Exception ex)
        {
          OnMessageNotify(new MessageNotifyEventArgs(MessageNotifyType.Error, "Construct the PH_Record_Has_Changed", ex));
        }
    }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
    private void InitializeWorkflowTables(DbConnection connection)
    {
      try
      {
        DbCommandHelper.ExecuteNonQuery(connection,
          "CREATE TABLE PH_Workflow (" +
          "WF_ID NUMERIC(15) NOT NULL," +
          "WF_Namespace VARCHAR(255) NOT NULL," +
          "WF_TypeName VARCHAR(255) NOT NULL," +
          "WF_Caption VARCHAR(4000) NULL," +
          "WF_XamlCode LONG /*TEXT*/ NULL," +
          "WF_Create_UserNumber VARCHAR(10) NOT NULL," +
          "WF_Create_Time DATE NOT NULL," +
          "WF_Change_UserNumber VARCHAR(10) NULL," +
          "WF_Change_Time DATE NULL," +
          "WF_Disable_UserNumber VARCHAR(10) NULL," +
          "WF_Disable_Time DATE NULL," +
          "PRIMARY KEY(WF_ID)," +
          "UNIQUE(WF_Namespace, WF_TypeName))", false);
      }
      catch (Exception ex)
      {
        OnMessageNotify(new MessageNotifyEventArgs(MessageNotifyType.Error, "Construct the PH_Workflow", "If already building can be omitted the tip", ex));
      }
      try
      {
        DbCommandHelper.ExecuteNonQuery(connection,
          "CREATE TABLE PH_Workflow_Instance (" +
          "WI_ID VARCHAR(36) NOT NULL," +
          "WI_WF_Namespace VARCHAR(255) NOT NULL," +
          "WI_WF_TypeName VARCHAR(255) NOT NULL," +
          "WI_Content LONG /*TEXT*/ NULL," +
          "WI_Time DATE NOT NULL," +
          "PRIMARY KEY(WI_ID))", false);
      }
      catch (Exception ex)
      {
        OnMessageNotify(new MessageNotifyEventArgs(MessageNotifyType.Error, "Construct the PH_Workflow_Instance", "If already building can be omitted the tip", ex));
      }
      try
      {
        DbCommandHelper.ExecuteNonQuery(connection,
          "CREATE TABLE PH_Workflow_TaskContext (" +
          "WC_WI_ID VARCHAR(36) NOT NULL," +
          "WC_Worker_UserNumber VARCHAR(10) NULL," +
          "WC_Content LONG /*TEXT*/ NULL," +
          "WC_Time DATE NOT NULL," +
          "PRIMARY KEY(WC_WI_ID))", false);
      }
      catch (Exception ex)
      {
        OnMessageNotify(new MessageNotifyEventArgs(MessageNotifyType.Error, "Construct the PH_Workflow_TaskContext", "If already building can be omitted the tip", ex));
      }
      try
      {
        DbCommandHelper.ExecuteNonQuery(connection,
          "CREATE TABLE PH_Workflow_Task (" +
          "WT_WI_ID VARCHAR(36) NOT NULL," +
          "WT_BookmarkName VARCHAR(255) NOT NULL," +
          "WT_Plugin_AssemblyName VARCHAR(255) NOT NULL," +
          "WT_Worker_RL_Name VARCHAR(100) NULL," +
          "WT_Caption VARCHAR(4000) NULL," +
          "WT_Message VARCHAR(4000) NULL," +
          "WT_Urgent NUMERIC(1) default 0 NOT NULL," +
          "WT_State NUMERIC(5) default 0 NOT NULL," +
          "WT_Dispatch_Time DATE NOT NULL," +
          "WT_Receive_Time DATE NULL," +
          "WT_Hold_Time DATE NULL," +
          "WT_Abortive_Time DATE NULL," +
          "WT_Complete_Time DATE NULL," +
          "PRIMARY KEY(WT_WI_ID, WT_BookmarkName))", false);
      }
      catch (Exception ex)
      {
        OnMessageNotify(new MessageNotifyEventArgs(MessageNotifyType.Error, "Construct the PH_Workflow_Task", "If already building can be omitted the tip", ex));
      }
    }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
    private void InitializeMessageTables(DbConnection connection)
    { 
      try
      {
        DbCommandHelper.ExecuteNonQuery(connection,
          "CREATE TABLE PH_Message(" +
          "MS_ID NUMERIC(15) NOT NULL," +
          "MS_Send_UserNumber VARCHAR(10) NOT NULL," +
          "MS_Receive_UserNumber VARCHAR(10) NOT NULL," +
          "MS_CreatedTime DATE NOT NULL," +
          "MS_SendedTime DATE NULL," +
          "MS_ReceivedTime DATE NULL," +
          "MS_Content LONG /*TEXT*/ NULL," +
          "PRIMARY KEY(MS_ID))", false);
      }
      catch (Exception ex)
      {
        OnMessageNotify(new MessageNotifyEventArgs(MessageNotifyType.Error, "Construct the PH_Message", "If already building can be omitted the tip", ex));
      }
      try
      {
        DbCommandHelper.ExecuteNonQuery(connection,
          "CREATE INDEX I_MS_Receive_UserNumber on PH_Message(MS_Receive_UserNumber, MS_CreatedTime)", false);
      }
      catch (Exception ex)
      {
        OnMessageNotify(new MessageNotifyEventArgs(MessageNotifyType.Error, "Construct the I_MS_Receive_UserNumber", "If already building can be omitted the tip", ex));
      }
    }

    #endregion
  }
}