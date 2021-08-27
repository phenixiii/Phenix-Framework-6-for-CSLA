#if Top
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Http.Dispatcher;
using System.Web.Http.Routing;
using System.Web.Http.SelfHost;
using Microsoft.AspNet.SignalR;
using Microsoft.Owin.Cors;
using Microsoft.Owin.Hosting;
using Owin;
using Phenix.Core.Web;
using Phenix.Services.Host.Service.Web;
#endif

using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Runtime.Remoting;
using System.ServiceModel;
using System.Windows.Forms;
using Phenix.Core;
using Phenix.Core.Dictionary;
using Phenix.Core.Net;
using Phenix.Core.Plugin;
using Phenix.Core.Security;
using Phenix.Services.Contract;

namespace Phenix.Services.Host.Core
{
  /// <summary>
  /// 服务管理器
  /// </summary>
  internal class ServiceManager : BaseDisposable
  {
#if Top

    /// <summary>
    /// for WebApp.Start()
    /// </summary>
    public ServiceManager() { }

#endif

    #region 单例

    private ServiceManager(Action<MessageNotifyEventArgs> doMessageNotify)
      : base()
    {
      PerformanceAnalyse.Default.MessageNotify += doMessageNotify;
      ClearConfigurationLibrary.Default.MessageNotify += doMessageNotify;

      _messageNotify = doMessageNotify;

      DoInitialize();
    }

    private static readonly object _defaultLock = new object();
    private static ServiceManager _default;
    public static ServiceManager Run(Action<MessageNotifyEventArgs> doMessageNotify)
    {
      if (_default == null)
        lock (_defaultLock)
          if (_default == null)
          {
            _default = new ServiceManager(doMessageNotify);
          }
      return _default;
    }
    public static void Stop()
    {
      ServiceManager manager = _default;
      if (manager != null)
        manager.Dispose();
    }

    private static bool _suspending;
    /// <summary>
    /// 是否暂停?
    /// </summary>
    public static bool Suspending
    {
      get { return _suspending; }
      private set { _suspending = value; }
    }
   
    private static string _suspendReason;
    /// <summary>
    /// 暂停原因
    /// </summary>
    public static string SuspendReason
    {
      get { return AppSettings.GetProperty(ref _suspendReason, String.Empty); }
      private set { AppSettings.SetProperty(ref _suspendReason, value); }
    }

    /// <summary>
    /// 标记暂停
    /// </summary>
    public static void MarkSuspending(string reason)
    {
      Suspending = true;
      SuspendReason = reason;
      Phenix.Core.Log.EventLog.SaveLocal("Suspending");
    }
    /// <summary>
    /// 恢复
    /// </summary>
    public static void Regain()
    {
      Suspending = false;
      Phenix.Core.Log.EventLog.SaveLocal("Suspended");
    }

    /// <summary>
    /// 升级状态
    /// </summary>
    public static UpgradeState UpgradeState { get; set; }

    private static string _upgradeReason;
    /// <summary>
    /// 升级原因
    /// </summary>
    public static string UpgradeReason
    {
      get { return AppSettings.GetProperty(ref _upgradeReason, String.Empty); }
      private set { AppSettings.SetProperty(ref _upgradeReason, value); }
    }

    /// <summary>
    /// 标记已升级
    /// </summary>
    public static void MarkUpgraded(string reason)
    {
      Phenix.Services.Library.AppUtilities.ResetServerVersion();
      UpgradeState = UpgradeState.Upgraded;
      UpgradeReason = reason;
      Phenix.Core.Log.EventLog.SaveLocal(UpgradeState.ToString());
    }

    #endregion

    #region 属性

    private static bool? _isBackup;
    /// <summary>
    /// 备用
    /// </summary>
    public static bool IsBackup
    {
      get
      {
        if (!_isBackup.HasValue)
          _isBackup = String.Compare(Path.GetFileName(Path.GetDirectoryName(AppConfig.BaseDirectory)), AppConfig.SERVICE_LIBRARY_SUBDIRECTORY_NAME, StringComparison.OrdinalIgnoreCase) == 0;
        return _isBackup.Value;
      }
    }

    private ServiceHost _dataPortalHost;
    private ServiceHost _dataDictionaryHost;
    private ServiceHost _dataSecurityHost;
    private ServiceHost _dataHost;
    private ServiceHost _dataRuleHost;
    private ServiceHost _permanentLogHost;
    private ServiceHost _objectCacheSynchroHost;
    private ServiceHost _workflowHost;
    private ServiceHost _downloadFilesHost;
    private ServiceHost _messageHost;

#if Top
    private HttpSelfHostServer _webApiSelfHostServer;
    private IDisposable _webSocketSelfHostServer;
#endif

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

      UnregisterWcfService();
#if Top
      UnregisterWebService();
#endif

      PerformanceAnalyse.Default.Dispose();
      ClearConfigurationLibrary.Default.Dispose();
    }

    protected override void DisposeUnmanagedResources()
    {
    }

    #endregion

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1804:RemoveUnusedLocals")]
    private void DoInitialize()
    {
      Environment.SetEnvironmentVariable(DataSecurityContext.InternalAuthenticationType, AppConfig.BaseDirectory, EnvironmentVariableTarget.User);
      //配置CSLA
      AppSettings.SaveValue("CslaAuthentication", DataSecurityContext.InternalAuthenticationType, false, true);
      
//#if Top
//      try
//      {
//        Process.Start(Path.Combine(Path.GetDirectoryName(AppConfig.BaseDirectory), "Phenix.Addin.Install.exe"));
//      }
//      catch (System.ComponentModel.Win32Exception ex)
//      {
//        OnMessageNotify(new MessageNotifyEventArgs(MessageNotifyType.Error, "Install Phenix.Addin", ex));
//      }
//#endif

      OnMessageNotify(new MessageNotifyEventArgs(MessageNotifyType.Information, "Configurate SystemRegistry", "..."));
      if (NetConfig.TcpTimedWaitDelay > 60000)
        NetConfig.TcpTimedWaitDelay = 30000;
      OnMessageNotify(new MessageNotifyEventArgs(MessageNotifyType.Information, "Set TcpTimedWaitDelay", NetConfig.TcpTimedWaitDelay.ToString()));
      if (NetConfig.MaxUserPort < 32768)
        NetConfig.MaxUserPort = 65534;
      OnMessageNotify(new MessageNotifyEventArgs(MessageNotifyType.Information, "Set MaxUserPort", NetConfig.MaxUserPort.ToString()));

      OnMessageNotify(new MessageNotifyEventArgs(MessageNotifyType.Information, "Registration services", "..."));
      RegisterService();
      RegisterWcfService();
#if Top
      RegisterAjaxService();
#endif
      OnMessageNotify(new MessageNotifyEventArgs(MessageNotifyType.Information, "Registration services", Phenix.Services.Host.Properties.Resources.Ready));
    }

    public static void CheckActive()
    {
      if (_default == null || Suspending)
        throw new InvalidOperationException(SuspendReason);
    }

    public static void CheckIn()
    {
      if (UserIdentity.CurrentIdentity == null)
        throw new UserVerifyException();
      CheckIn(UserIdentity.CurrentIdentity);
    }

    public static void CheckIn(System.Security.Principal.IPrincipal principal)
    {
      CheckIn(principal.Identity as UserIdentity);
    }

    public static void CheckIn(UserIdentity identity)
    {
      CheckActive();
      if (UpgradeState == UpgradeState.Upgraded && identity != null && Phenix.Services.Library.AppUtilities.ServerVersion != identity.ServerVersion)
        throw new InvalidOperationException(UpgradeReason);
    }

    public static void RegisterAssembly()
    {
      if (_default != null)
        lock (_defaultLock)
          if (_default != null)
          {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
              openFileDialog.RestoreDirectory = true;
              openFileDialog.Multiselect = true;
              openFileDialog.Filter = "assemblies|*.dll";
              openFileDialog.Title = "Register assemblies";
              if (openFileDialog.ShowDialog() == DialogResult.OK)
                _default.DoRegisterAssembly(openFileDialog.FileNames);
            }
          }
    }

    public static void RegisterAssembly(string fileName)
    {
      if (_default != null)
        lock (_defaultLock)
          if (_default != null)
          {
            _default.DoRegisterAssembly(fileName);
          }
    }

    private void RegisterService()
    {
      if (Phenix.Services.Host.Properties.Settings.Default.LetRemotingHttp)
      {
        RemotingHelper.RegisterHttpServiceChannel(IsBackup ? RemotingConfig.HttpPort + 1 : RemotingConfig.HttpPort, RemotingConfig.EnsureSecurity);
        OnMessageNotify(new MessageNotifyEventArgs(MessageNotifyType.Information, "remoting",
          String.Format("HTTP Port = {0}", IsBackup ? RemotingConfig.HttpPort + 1 : RemotingConfig.HttpPort)));
      }
      if (Phenix.Services.Host.Properties.Settings.Default.LetRemotingTcp)
      {
        RemotingHelper.RegisterTcpServiceChannel(IsBackup ? RemotingConfig.TcpPort + 1 : RemotingConfig.TcpPort, RemotingConfig.EnsureSecurity,
          RemotingConfig.CompressionSupported, RemotingConfig.CompressionThresholdMax);
        OnMessageNotify(new MessageNotifyEventArgs(MessageNotifyType.Information, "remoting",
          String.Format("TCP Port = {0}", IsBackup ? RemotingConfig.TcpPort + 1 : RemotingConfig.TcpPort)));
      }

      RemotingConfiguration.CustomErrorsMode = CustomErrorsModes.Off;
      RemotingConfiguration.Configure(AppDomain.CurrentDomain.SetupInformation.ConfigurationFile, RemotingConfig.EnsureSecurity);
      //OnMessageNotify(new MessageNotifyEventArgs(MessageNotifyType.Information, MethodBase.GetCurrentMethod().Name,
      //  String.Format("load:{0}, EnsureSecurity={1}", 
      //    AppDomain.CurrentDomain.SetupInformation.ConfigurationFile, RemotingHelper.EnsureSecurity)));

      RemotingConfiguration.RegisterWellKnownServiceType(
        typeof(Phenix.Services.Host.Service.DataPortal), ServicesInfo.DATA_PORTAL_URI, WellKnownObjectMode.SingleCall);
      RemotingConfiguration.RegisterWellKnownServiceType(
        typeof(Phenix.Services.Host.Service.DataDictionary), ServicesInfo.DATA_DICTIONARY_URI, WellKnownObjectMode.SingleCall);
      RemotingConfiguration.RegisterWellKnownServiceType(
        typeof(Phenix.Services.Host.Service.DataSecurity), ServicesInfo.DATA_SECURITY_URI, WellKnownObjectMode.SingleCall);
      RemotingConfiguration.RegisterWellKnownServiceType(
        typeof(Phenix.Services.Host.Service.Data), ServicesInfo.DATA_URI, WellKnownObjectMode.SingleCall);
      RemotingConfiguration.RegisterWellKnownServiceType(
        typeof(Phenix.Services.Host.Service.DataRule), ServicesInfo.DATA_RULE_URI, WellKnownObjectMode.SingleCall);
      RemotingConfiguration.RegisterWellKnownServiceType(
        typeof(Phenix.Services.Host.Service.PermanentLog), ServicesInfo.PERMANENT_LOG_URI, WellKnownObjectMode.SingleCall);
      RemotingConfiguration.RegisterWellKnownServiceType(
        typeof(Phenix.Services.Host.Service.ObjectCacheSynchro), ServicesInfo.OBJECT_CACHE_SYNCHRO_URI, WellKnownObjectMode.SingleCall);
      RemotingConfiguration.RegisterWellKnownServiceType(
        typeof(Phenix.Services.Host.Service.Workflow), ServicesInfo.WORKFLOW_URI, WellKnownObjectMode.SingleCall);
      RemotingConfiguration.RegisterWellKnownServiceType(
        typeof(Phenix.Services.Host.Service.DownloadFiles), ServicesInfo.DOWNLOAD_FILES_URI, WellKnownObjectMode.SingleCall);
      RemotingConfiguration.RegisterWellKnownServiceType(
        typeof(Phenix.Services.Host.Service.Message), ServicesInfo.MESSAGE_URI, WellKnownObjectMode.SingleCall);
    }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
    private void RegisterWcfService()
    {
      if (Phenix.Services.Host.Properties.Settings.Default.LetWcfHttp || Phenix.Services.Host.Properties.Settings.Default.LetWcfTcp)
      {
        if (_dataPortalHost == null)
        {
          _dataPortalHost = new ServiceHost(typeof(Phenix.Services.Host.Service.Wcf.DataPortal));
          if (Phenix.Services.Host.Properties.Settings.Default.LetWcfHttp)
            _dataPortalHost.AddServiceEndpoint(typeof(Csla.Server.Hosts.IWcfPortal),
              WcfHelper.CreateBasicHttpBinding(),
              WcfHelper.CreateBasicHttpUrl(NetConfig.LocalAddress, IsBackup ? WcfConfig.BasicHttpPort + 1 : WcfConfig.BasicHttpPort, ServicesInfo.DATA_PORTAL_URI));
          if (Phenix.Services.Host.Properties.Settings.Default.LetWcfTcp)
            _dataPortalHost.AddServiceEndpoint(typeof(Csla.Server.Hosts.IWcfPortal),
              WcfHelper.CreateNetTcpBinding(),
              WcfHelper.CreateNetTcpUrl(NetConfig.LocalAddress, IsBackup ? WcfConfig.NetTcpPort + 1 : WcfConfig.NetTcpPort, ServicesInfo.DATA_PORTAL_URI));
        }
        if (_dataPortalHost.State != CommunicationState.Opening && _dataPortalHost.State != CommunicationState.Opened)
          _dataPortalHost.Open();

        if (_dataDictionaryHost == null)
        {
          _dataDictionaryHost = new ServiceHost(typeof(Phenix.Services.Host.Service.Wcf.DataDictionary));
          if (Phenix.Services.Host.Properties.Settings.Default.LetWcfHttp)
            _dataDictionaryHost.AddServiceEndpoint(typeof(Phenix.Services.Contract.Wcf.IDataDictionary),
              WcfHelper.CreateBasicHttpBinding(),
              WcfHelper.CreateBasicHttpUrl(NetConfig.LocalAddress, IsBackup ? WcfConfig.BasicHttpPort + 1 : WcfConfig.BasicHttpPort, ServicesInfo.DATA_DICTIONARY_URI));
          if (Phenix.Services.Host.Properties.Settings.Default.LetWcfTcp)
            _dataDictionaryHost.AddServiceEndpoint(typeof(Phenix.Services.Contract.Wcf.IDataDictionary),
              WcfHelper.CreateNetTcpBinding(),
              WcfHelper.CreateNetTcpUrl(NetConfig.LocalAddress, IsBackup ? WcfConfig.NetTcpPort + 1 : WcfConfig.NetTcpPort, ServicesInfo.DATA_DICTIONARY_URI));
        }
        if (_dataDictionaryHost.State != CommunicationState.Opening && _dataDictionaryHost.State != CommunicationState.Opened)
          _dataDictionaryHost.Open();

        if (_dataSecurityHost == null)
        {
          _dataSecurityHost = new ServiceHost(typeof(Phenix.Services.Host.Service.Wcf.DataSecurity));
          if (Phenix.Services.Host.Properties.Settings.Default.LetWcfHttp)
            _dataSecurityHost.AddServiceEndpoint(typeof(Phenix.Services.Contract.Wcf.IDataSecurity),
              WcfHelper.CreateBasicHttpBinding(),
              WcfHelper.CreateBasicHttpUrl(NetConfig.LocalAddress, IsBackup ? WcfConfig.BasicHttpPort + 1 : WcfConfig.BasicHttpPort, ServicesInfo.DATA_SECURITY_URI));
          if (Phenix.Services.Host.Properties.Settings.Default.LetWcfTcp)
            _dataSecurityHost.AddServiceEndpoint(typeof(Phenix.Services.Contract.Wcf.IDataSecurity),
              WcfHelper.CreateNetTcpBinding(),
              WcfHelper.CreateNetTcpUrl(NetConfig.LocalAddress, IsBackup ? WcfConfig.NetTcpPort + 1 : WcfConfig.NetTcpPort, ServicesInfo.DATA_SECURITY_URI));
        }
        if (_dataSecurityHost.State != CommunicationState.Opening && _dataSecurityHost.State != CommunicationState.Opened)
          _dataSecurityHost.Open();

        if (_dataHost == null)
        {
          _dataHost = new ServiceHost(typeof(Phenix.Services.Host.Service.Wcf.Data));
          if (Phenix.Services.Host.Properties.Settings.Default.LetWcfHttp)
            _dataHost.AddServiceEndpoint(typeof(Phenix.Services.Contract.Wcf.IData),
              WcfHelper.CreateBasicHttpBinding(),
              WcfHelper.CreateBasicHttpUrl(NetConfig.LocalAddress, IsBackup ? WcfConfig.BasicHttpPort + 1 : WcfConfig.BasicHttpPort, ServicesInfo.DATA_URI));
          if (Phenix.Services.Host.Properties.Settings.Default.LetWcfTcp)
            _dataHost.AddServiceEndpoint(typeof(Phenix.Services.Contract.Wcf.IData),
              WcfHelper.CreateNetTcpBinding(),
              WcfHelper.CreateNetTcpUrl(NetConfig.LocalAddress, IsBackup ? WcfConfig.NetTcpPort + 1 : WcfConfig.NetTcpPort, ServicesInfo.DATA_URI));
        }
        if (_dataHost.State != CommunicationState.Opening && _dataHost.State != CommunicationState.Opened)
          _dataHost.Open();

        if (_dataRuleHost == null)
        {
          _dataRuleHost = new ServiceHost(typeof(Phenix.Services.Host.Service.Wcf.DataRule));
          if (Phenix.Services.Host.Properties.Settings.Default.LetWcfHttp)
            _dataRuleHost.AddServiceEndpoint(typeof(Phenix.Services.Contract.Wcf.IDataRule),
              WcfHelper.CreateBasicHttpBinding(),
              WcfHelper.CreateBasicHttpUrl(NetConfig.LocalAddress, IsBackup ? WcfConfig.BasicHttpPort + 1 : WcfConfig.BasicHttpPort, ServicesInfo.DATA_RULE_URI));
          if (Phenix.Services.Host.Properties.Settings.Default.LetWcfTcp)
            _dataRuleHost.AddServiceEndpoint(typeof(Phenix.Services.Contract.Wcf.IDataRule),
              WcfHelper.CreateNetTcpBinding(),
              WcfHelper.CreateNetTcpUrl(NetConfig.LocalAddress, IsBackup ? WcfConfig.NetTcpPort + 1 : WcfConfig.NetTcpPort, ServicesInfo.DATA_RULE_URI));
        }
        if (_dataRuleHost.State != CommunicationState.Opening && _dataRuleHost.State != CommunicationState.Opened)
          _dataRuleHost.Open();

        if (_permanentLogHost == null)
        {
          _permanentLogHost = new ServiceHost(typeof(Phenix.Services.Host.Service.Wcf.PermanentLog));
          if (Phenix.Services.Host.Properties.Settings.Default.LetWcfHttp)
            _permanentLogHost.AddServiceEndpoint(typeof(Phenix.Services.Contract.Wcf.IPermanentLog),
              WcfHelper.CreateBasicHttpBinding(),
              WcfHelper.CreateBasicHttpUrl(NetConfig.LocalAddress, IsBackup ? WcfConfig.BasicHttpPort + 1 : WcfConfig.BasicHttpPort, ServicesInfo.PERMANENT_LOG_URI));
          if (Phenix.Services.Host.Properties.Settings.Default.LetWcfTcp)
            _permanentLogHost.AddServiceEndpoint(typeof(Phenix.Services.Contract.Wcf.IPermanentLog),
              WcfHelper.CreateNetTcpBinding(),
              WcfHelper.CreateNetTcpUrl(NetConfig.LocalAddress, IsBackup ? WcfConfig.NetTcpPort + 1 : WcfConfig.NetTcpPort, ServicesInfo.PERMANENT_LOG_URI));
        }
        if (_permanentLogHost.State != CommunicationState.Opening && _permanentLogHost.State != CommunicationState.Opened)
          _permanentLogHost.Open();

        if (_objectCacheSynchroHost == null)
        {
          _objectCacheSynchroHost = new ServiceHost(typeof(Phenix.Services.Host.Service.Wcf.ObjectCacheSynchro));
          if (Phenix.Services.Host.Properties.Settings.Default.LetWcfHttp)
            _objectCacheSynchroHost.AddServiceEndpoint(typeof(Phenix.Services.Contract.Wcf.IObjectCacheSynchro),
              WcfHelper.CreateBasicHttpBinding(),
              WcfHelper.CreateBasicHttpUrl(NetConfig.LocalAddress, IsBackup ? WcfConfig.BasicHttpPort + 1 : WcfConfig.BasicHttpPort, ServicesInfo.OBJECT_CACHE_SYNCHRO_URI));
          if (Phenix.Services.Host.Properties.Settings.Default.LetWcfTcp)
            _objectCacheSynchroHost.AddServiceEndpoint(typeof(Phenix.Services.Contract.Wcf.IObjectCacheSynchro),
              WcfHelper.CreateNetTcpBinding(),
              WcfHelper.CreateNetTcpUrl(NetConfig.LocalAddress, IsBackup ? WcfConfig.NetTcpPort + 1 : WcfConfig.NetTcpPort, ServicesInfo.OBJECT_CACHE_SYNCHRO_URI));
        }
        if (_objectCacheSynchroHost.State != CommunicationState.Opening && _objectCacheSynchroHost.State != CommunicationState.Opened)
          _objectCacheSynchroHost.Open();

        if (_workflowHost == null)
        {
          _workflowHost = new ServiceHost(typeof(Phenix.Services.Host.Service.Wcf.Workflow));
          if (Phenix.Services.Host.Properties.Settings.Default.LetWcfHttp)
            _workflowHost.AddServiceEndpoint(typeof(Phenix.Services.Contract.Wcf.IWorkflow),
              WcfHelper.CreateBasicHttpBinding(),
              WcfHelper.CreateBasicHttpUrl(NetConfig.LocalAddress, IsBackup ? WcfConfig.BasicHttpPort + 1 : WcfConfig.BasicHttpPort, ServicesInfo.WORKFLOW_URI));
          if (Phenix.Services.Host.Properties.Settings.Default.LetWcfTcp)
            _workflowHost.AddServiceEndpoint(typeof(Phenix.Services.Contract.Wcf.IWorkflow),
              WcfHelper.CreateNetTcpBinding(),
              WcfHelper.CreateNetTcpUrl(NetConfig.LocalAddress, IsBackup ? WcfConfig.NetTcpPort + 1 : WcfConfig.NetTcpPort, ServicesInfo.WORKFLOW_URI));
        }
        if (_workflowHost.State != CommunicationState.Opening && _workflowHost.State != CommunicationState.Opened)
          _workflowHost.Open();

        if (_downloadFilesHost == null)
        {
          _downloadFilesHost = new ServiceHost(typeof(Phenix.Services.Host.Service.Wcf.DownloadFiles));
          if (Phenix.Services.Host.Properties.Settings.Default.LetWcfHttp)
            _downloadFilesHost.AddServiceEndpoint(typeof(Phenix.Services.Contract.Wcf.IDownloadFiles),
              WcfHelper.CreateBasicHttpBinding(),
              WcfHelper.CreateBasicHttpUrl(NetConfig.LocalAddress, IsBackup ? WcfConfig.BasicHttpPort + 1 : WcfConfig.BasicHttpPort, ServicesInfo.DOWNLOAD_FILES_URI));
          if (Phenix.Services.Host.Properties.Settings.Default.LetWcfTcp)
            _downloadFilesHost.AddServiceEndpoint(typeof(Phenix.Services.Contract.Wcf.IDownloadFiles),
              WcfHelper.CreateNetTcpBinding(),
              WcfHelper.CreateNetTcpUrl(NetConfig.LocalAddress, IsBackup ? WcfConfig.NetTcpPort + 1 : WcfConfig.NetTcpPort, ServicesInfo.DOWNLOAD_FILES_URI));
        }
        if (_downloadFilesHost.State != CommunicationState.Opening && _downloadFilesHost.State != CommunicationState.Opened)
          _downloadFilesHost.Open();

        if (_messageHost == null)
        {
          _messageHost = new ServiceHost(typeof(Phenix.Services.Host.Service.Wcf.Message));
          if (Phenix.Services.Host.Properties.Settings.Default.LetWcfHttp)
            _messageHost.AddServiceEndpoint(typeof(Phenix.Services.Contract.Wcf.IMessage),
              WcfHelper.CreateBasicHttpBinding(),
              WcfHelper.CreateBasicHttpUrl(NetConfig.LocalAddress, IsBackup ? WcfConfig.BasicHttpPort + 1 : WcfConfig.BasicHttpPort, ServicesInfo.MESSAGE_URI));
          if (Phenix.Services.Host.Properties.Settings.Default.LetWcfTcp)
            _messageHost.AddServiceEndpoint(typeof(Phenix.Services.Contract.Wcf.IMessage),
              WcfHelper.CreateNetTcpBinding(),
              WcfHelper.CreateNetTcpUrl(NetConfig.LocalAddress, IsBackup ? WcfConfig.NetTcpPort + 1 : WcfConfig.NetTcpPort, ServicesInfo.MESSAGE_URI));
        }
        if (_messageHost.State != CommunicationState.Opening && _messageHost.State != CommunicationState.Opened)
          _messageHost.Open();

        if (Phenix.Services.Host.Properties.Settings.Default.LetWcfHttp)
          OnMessageNotify(new MessageNotifyEventArgs(MessageNotifyType.Information, "WCF",
            String.Format("HTTP PORT = {0}", IsBackup ? WcfConfig.BasicHttpPort + 1 : WcfConfig.BasicHttpPort)));
        if (Phenix.Services.Host.Properties.Settings.Default.LetWcfTcp)
          OnMessageNotify(new MessageNotifyEventArgs(MessageNotifyType.Information, "WCF",
            String.Format("TCP PORT = {0}", IsBackup ? WcfConfig.NetTcpPort + 1 : WcfConfig.NetTcpPort)));
      }
    }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
    private void UnregisterWcfService()
    {
      if (_dataPortalHost != null && (_dataPortalHost.State == CommunicationState.Opening || _dataPortalHost.State == CommunicationState.Opened))
      {
        _dataPortalHost.Close();
        _dataPortalHost = null;
      }
      if (_dataDictionaryHost != null && (_dataDictionaryHost.State == CommunicationState.Opening || _dataDictionaryHost.State == CommunicationState.Opened))
      {
        _dataDictionaryHost.Close();
        _dataDictionaryHost = null;
      }
      if (_dataSecurityHost != null && (_dataSecurityHost.State == CommunicationState.Opening || _dataSecurityHost.State == CommunicationState.Opened))
      {
        _dataSecurityHost.Close();
        _dataSecurityHost = null;
      }
      if (_dataHost != null && (_dataHost.State == CommunicationState.Opening || _dataHost.State == CommunicationState.Opened))
      {
        _dataHost.Close();
        _dataHost = null;
      }
      if (_dataRuleHost != null && (_dataRuleHost.State == CommunicationState.Opening || _dataRuleHost.State == CommunicationState.Opened))
      {
        _dataRuleHost.Close();
        _dataRuleHost = null;
      }
      if (_permanentLogHost != null && (_permanentLogHost.State == CommunicationState.Opening || _permanentLogHost.State == CommunicationState.Opened))
      {
        _permanentLogHost.Close();
        _permanentLogHost = null;
      }
      if (_objectCacheSynchroHost != null && (_objectCacheSynchroHost.State == CommunicationState.Opening || _objectCacheSynchroHost.State == CommunicationState.Opened))
      {
        _objectCacheSynchroHost.Close();
        _objectCacheSynchroHost = null;
      }
      if (_workflowHost != null && (_workflowHost.State == CommunicationState.Opening || _workflowHost.State == CommunicationState.Opened))
      {
        _workflowHost.Close();
        _workflowHost = null;
      }
      if (_downloadFilesHost != null && (_downloadFilesHost.State == CommunicationState.Opening || _downloadFilesHost.State == CommunicationState.Opened))
      {
        _downloadFilesHost.Close();
        _downloadFilesHost = null;
      }
      if (_messageHost != null && (_messageHost.State == CommunicationState.Opening || _messageHost.State == CommunicationState.Opened))
      {
        _messageHost.Close();
        _messageHost = null;
      }
    }

#if Top

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
    public void Configuration(IAppBuilder app)
    {
      app.Map("/signalr", map =>
      {
        map.UseCors(CorsOptions.AllowAll);
        HubConfiguration hubConfiguration = new HubConfiguration
        {
          EnableDetailedErrors = true,
          EnableJavaScriptProxies = false,
          EnableJSONP = true
        };
        map.RunSignalR(hubConfiguration);
      });
    }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
    private void RegisterAjaxService()
    {
      if (Phenix.Services.Host.Properties.Settings.Default.LetWebApi)
      {
        try
        {
          string baseAddress = String.Format(@"http://{0}:{1}", NetConfig.LocalAddress, WebConfig.WebApiPort);
          if (_webApiSelfHostServer == null)
          {
            HttpSelfHostConfiguration configuration = new HttpSelfHostConfiguration(baseAddress);
            configuration.Services.Replace(typeof(IAssembliesResolver), new PluginAssembliesResolver());
            configuration.TransferMode = TransferMode.Streamed;
            configuration.MaxReceivedMessageSize = Int32.MaxValue; // default is 65536
            configuration.MaxConcurrentRequests = Math.Max(Environment.ProcessorCount * (int)Phenix.Services.Host.Properties.Settings.Default.WebMaxConcurrentRequests, (int)Phenix.Services.Host.Properties.Settings.Default.WebMaxConcurrentRequests);
            configuration.EnableCors(new EnableCorsAttribute(Phenix.Services.Host.Properties.Settings.Default.WebEnableCorsOrigins, "*", "*"));
            configuration.MapHttpAttributeRoutes();
            configuration.Routes.Clear();
            configuration.Routes.MapHttpRoute(
              name: "DefaultApi",
              routeTemplate: "api/{controller}/{id}",
              defaults: new { id = RouteParameter.Optional },
              constraints: null,
              handler: new ServiceHandler(configuration));
            _webApiSelfHostServer = new HttpSelfHostServer(configuration);
            _webApiSelfHostServer.OpenAsync().Wait();
            OnMessageNotify(new MessageNotifyEventArgs(MessageNotifyType.Information, "WebAPI",
              String.Format("BaseAddress = {0}, TransferMode = {1}, MaxReceivedMessageSize = {2}, MaxConcurrentRequests = {3}, ReceiveTimeout = {4}, SendTimeout = {5}",
                configuration.BaseAddress, configuration.TransferMode, configuration.MaxReceivedMessageSize, configuration.MaxConcurrentRequests,
                configuration.ReceiveTimeout, configuration.SendTimeout)));
            foreach (IHttpRoute item in configuration.Routes)
              OnMessageNotify(new MessageNotifyEventArgs(MessageNotifyType.Information, "WebAPI",
                String.Format("RouteTemplate = {0}", item.RouteTemplate)));
          }
        }
        catch (Exception ex)
        {
          MessageBox.Show(@"Registered WebAPI service failure, if you want to open, please setup SystemInfo:\n" + AppUtilities.GetErrorHint(ex),
            NetConfig.LocalAddress, MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
      }
      if (Phenix.Services.Host.Properties.Settings.Default.LetWebSocket)
      {
        try
        {
          string baseAddress = String.Format(@"http://{0}:{1}", Phenix.Services.Host.Properties.Settings.Default.WebEnableCorsOrigins, WebConfig.WebSocketPort);
          if (_webSocketSelfHostServer == null)
          {
            _webSocketSelfHostServer = WebApp.Start<ServiceManager>(baseAddress);
            OnMessageNotify(new MessageNotifyEventArgs(MessageNotifyType.Information, "WebSocket", baseAddress));
          }
        }
        catch (Exception ex)
        {
          MessageBox.Show(@"Registered WebSocket service failure, if you want to open, please setup SystemInfo:\n" + AppUtilities.GetErrorHint(ex),
            NetConfig.LocalAddress, MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
      }
    }

    private void UnregisterWebService()
    {
      if (_webApiSelfHostServer != null)
      {
        _webApiSelfHostServer.CloseAsync().Wait();
        _webApiSelfHostServer = null;
      }
      if (_webSocketSelfHostServer != null)
      {
        _webSocketSelfHostServer.Dispose();
        _webSocketSelfHostServer = null;
      }
    }

#endif

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
    private void DoRegisterAssembly(params string[] fileNames)
    {
      AppDomain domain = AppDomain.CreateDomain(MethodBase.GetCurrentMethod().Name);
      try
      {
        using (PluginHost pluginHost = new PluginHost())
        {
          foreach (string s in fileNames)
            try
            {
              Assembly assembly = domain.Load(Path.GetFileNameWithoutExtension(s));
              try
              {
                IPlugin plugin = pluginHost.CreatePlugin(assembly, true);
                if (plugin != null)
                {
                  OnMessageNotify(new MessageNotifyEventArgs(MessageNotifyType.Information, "Register component", plugin.Key));
                  try
                  {
                    IList<MessageNotifyEventArgs> messages = plugin.Initialization();
                    if (messages != null)
                      foreach (MessageNotifyEventArgs item in messages)
                        OnMessageNotify(item);
                  }
                  catch (Exception ex)
                  {
                    OnMessageNotify(new MessageNotifyEventArgs(MessageNotifyType.Error, "Register component", "If already registered assemblies components can ignore this tip.", ex));
                  }
                }
              }
              catch (Exception ex)
              {
                OnMessageNotify(new MessageNotifyEventArgs(MessageNotifyType.Error, "Register component", "If without registration procedures set components can be omitted the tip.", ex));
              }
              foreach (Type item in assembly.GetExportedTypes())
                if (AppUtilities.Register(item))
                  OnMessageNotify(new MessageNotifyEventArgs(MessageNotifyType.Information, "Register component", item.FullName));
            }
            catch (Exception ex)
            {
              OnMessageNotify(new MessageNotifyEventArgs(MessageNotifyType.Error, "Register component", Path.GetFileName(s), ex));
              Phenix.Core.Log.EventLog.SaveLocal(MethodBase.GetCurrentMethod(), ex);
            }
        }
      }
      finally
      {
        AppDomain.Unload(domain);
        DataDictionaryHub.AssemblyInfoHasChanged();
      }
      OnMessageNotify(new MessageNotifyEventArgs(MessageNotifyType.Information, "Register component", Phenix.Services.Host.Properties.Resources.Finished));
    }

    #endregion
  }
}