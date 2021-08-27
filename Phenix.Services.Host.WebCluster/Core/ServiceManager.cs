using System;
using System.Reflection;
using System.Runtime.InteropServices;
using System.ServiceModel;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Http.Routing;
using System.Web.Http.SelfHost;
using System.Windows.Forms;
using Microsoft.AspNet.SignalR;
using Microsoft.Owin.Cors;
using Microsoft.Owin.Hosting;
using Owin;
using Phenix.Core;
using Phenix.Core.Net;
using Phenix.Core.Security;
using Phenix.Core.Web;
using Phenix.Services.Host.WebCluster.Service.Web;

namespace Phenix.Services.Host.WebCluster.Core
{
  internal class ServiceManager : BaseDisposable
  {
    /// <summary>
    /// for WebApp.Start()
    /// </summary>
    public ServiceManager() { }

    #region 单例

    private ServiceManager(Action<MessageNotifyEventArgs> doMessageNotify)
      : base()
    {
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

    #endregion

    #region 属性

    private HttpSelfHostServer _webApiSelfHostServer;
    private HttpSelfHostServer _webApiSslSelfHostServer;
    private IDisposable _webSocketSelfHostServer;
    private IDisposable _webSocketSslSelfHostServer;

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

      UnregisterWebService();
    }

    protected override void DisposeUnmanagedResources()
    {
    }

    #endregion

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1804:RemoveUnusedLocals")]
    private void DoInitialize()
    {
      Environment.SetEnvironmentVariable(DataSecurityContext.InternalAuthenticationType, AppConfig.BaseDirectory, EnvironmentVariableTarget.User);
   
      OnMessageNotify(new MessageNotifyEventArgs(MessageNotifyType.Information, "Configurate SystemRegistry", "..."));
      if (NetConfig.TcpTimedWaitDelay > 60000)
        NetConfig.TcpTimedWaitDelay = 30000;
      OnMessageNotify(new MessageNotifyEventArgs(MessageNotifyType.Information, "Set TcpTimedWaitDelay", NetConfig.TcpTimedWaitDelay.ToString()));
      if (NetConfig.MaxUserPort < 32768)
        NetConfig.MaxUserPort = 65534;
      OnMessageNotify(new MessageNotifyEventArgs(MessageNotifyType.Information, "Set MaxUserPort", NetConfig.MaxUserPort.ToString()));

      OnMessageNotify(new MessageNotifyEventArgs(MessageNotifyType.Information, "Registration services", "..."));
      RegisterAjaxService();
      OnMessageNotify(new MessageNotifyEventArgs(MessageNotifyType.Information, "Registration services", Phenix.Services.Host.WebCluster.Properties.Resources.Ready));
    }

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
      if (Phenix.Services.Host.WebCluster.Properties.Settings.Default.LetWebApi)
      {
        try
        {
          string baseAddress = String.Format(@"http://{0}:{1}", NetConfig.LocalAddress, WebConfig.WebApiPort);
          if (_webApiSelfHostServer == null)
          {
            HttpSelfHostConfiguration configuration = new HttpSelfHostConfiguration(baseAddress);
            configuration.TransferMode = TransferMode.Streamed;
            configuration.MaxReceivedMessageSize = Int32.MaxValue; // default is 65536
            configuration.MaxConcurrentRequests = Math.Max(Environment.ProcessorCount * (int)Phenix.Services.Host.WebCluster.Properties.Settings.Default.WebMaxConcurrentRequests, (int)Phenix.Services.Host.WebCluster.Properties.Settings.Default.WebMaxConcurrentRequests);
            configuration.EnableCors(new EnableCorsAttribute(Phenix.Services.Host.WebCluster.Properties.Settings.Default.WebEnableCorsOrigins, "*", "*"));
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
      if (Phenix.Services.Host.WebCluster.Properties.Settings.Default.LetWebApiSsl)
      {
        try
        {
          string baseAddress = String.Format(@"https://{0}:{1}",
            !String.IsNullOrEmpty(Phenix.Services.Host.WebCluster.Properties.Settings.Default.WebApiSslIdnHost) ? Phenix.Services.Host.WebCluster.Properties.Settings.Default.WebApiSslIdnHost : NetConfig.LocalAddress,
            WebConfig.WebApiSslPort);
          if (_webApiSslSelfHostServer == null)
          {
            HttpSelfHostConfiguration configuration = new HttpSelfHostConfiguration(baseAddress);
            configuration.TransferMode = TransferMode.Streamed;
            configuration.MaxReceivedMessageSize = Int32.MaxValue; // default is 65536
            configuration.MaxConcurrentRequests = Math.Max(Environment.ProcessorCount * (int)Phenix.Services.Host.WebCluster.Properties.Settings.Default.WebMaxConcurrentRequests, (int)Phenix.Services.Host.WebCluster.Properties.Settings.Default.WebMaxConcurrentRequests);
            configuration.EnableCors(new EnableCorsAttribute(Phenix.Services.Host.WebCluster.Properties.Settings.Default.WebEnableCorsOrigins, "*", "*"));
            configuration.MapHttpAttributeRoutes();
            configuration.Routes.Clear();
            configuration.Routes.MapHttpRoute(
              name: "DefaultApi",
              routeTemplate: "api/{controller}/{id}",
              defaults: new { id = RouteParameter.Optional },
              constraints: null,
              handler: new ServiceHandler(configuration));
            _webApiSslSelfHostServer = new HttpSelfHostServer(configuration);
            _webApiSslSelfHostServer.OpenAsync().Wait();
            OnMessageNotify(new MessageNotifyEventArgs(MessageNotifyType.Information, "WebAPI+SSL",
              String.Format("BaseAddress = {0}, TransferMode = {1}, MaxReceivedMessageSize = {2}, MaxConcurrentRequests = {3}, ReceiveTimeout = {4}, SendTimeout = {5}",
                configuration.BaseAddress, configuration.TransferMode, configuration.MaxReceivedMessageSize, configuration.MaxConcurrentRequests,
                configuration.ReceiveTimeout, configuration.SendTimeout)));
            foreach (IHttpRoute item in configuration.Routes)
              OnMessageNotify(new MessageNotifyEventArgs(MessageNotifyType.Information, "WebAPI+SSL",
                String.Format("RouteTemplate = {0}", item.RouteTemplate)));
          }
        }
        catch (Exception ex)
        {
          MessageBox.Show(@"Registered WebAPI+SSL service failure, if you want to open, please setup SystemInfo:\n" + AppUtilities.GetErrorHint(ex),
            NetConfig.LocalAddress, MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
      }
      if (Phenix.Services.Host.WebCluster.Properties.Settings.Default.LetWebSocket)
      {
        try
        {
          string baseAddress = String.Format(@"http://{0}:{1}", Phenix.Services.Host.WebCluster.Properties.Settings.Default.WebEnableCorsOrigins, WebConfig.WebSocketPort);
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
      if (Phenix.Services.Host.WebCluster.Properties.Settings.Default.LetWebSocketSsl)
      {
        try
        {
          string baseAddress = String.Format(@"https://{0}:{1}", 
            !String.IsNullOrEmpty(Phenix.Services.Host.WebCluster.Properties.Settings.Default.WebSocketSslIdnHost) ? Phenix.Services.Host.WebCluster.Properties.Settings.Default.WebSocketSslIdnHost : Phenix.Services.Host.WebCluster.Properties.Settings.Default.WebEnableCorsOrigins, 
            WebConfig.WebSocketSslPort);
          if (_webSocketSslSelfHostServer == null)
          {
            _webSocketSslSelfHostServer = WebApp.Start<ServiceManager>(baseAddress);
            OnMessageNotify(new MessageNotifyEventArgs(MessageNotifyType.Information, "WebSocket+SSL", baseAddress));
          }
        }
        catch (Exception ex)
        {
          MessageBox.Show(@"Registered WebSocket+SSL service failure, if you want to open, please setup SystemInfo:\n" + AppUtilities.GetErrorHint(ex),
            NetConfig.LocalAddress, MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
      }
      OnMessageNotify(new MessageNotifyEventArgs(MessageNotifyType.Information, "Application GUID",
        ((GuidAttribute)Attribute.GetCustomAttribute(Assembly.GetExecutingAssembly(), typeof(GuidAttribute))).Value));
    }

    private void UnregisterWebService()
    {
      if (_webApiSelfHostServer != null)
      {
        _webApiSelfHostServer.CloseAsync().Wait();
        _webApiSelfHostServer = null;
      }
      if (_webApiSslSelfHostServer != null)
      {
        _webApiSslSelfHostServer.CloseAsync().Wait();
        _webApiSslSelfHostServer = null;
      }
      if (_webSocketSelfHostServer != null)
      {
        _webSocketSelfHostServer.Dispose();
        _webSocketSelfHostServer = null;
      }
      if (_webSocketSslSelfHostServer != null)
      {
        _webSocketSslSelfHostServer.Dispose();
        _webSocketSslSelfHostServer = null;
      }
    }

    public void Reset()
    {
      UnregisterWebService();
      RegisterAjaxService();
    }
    
    #endregion
  }
}