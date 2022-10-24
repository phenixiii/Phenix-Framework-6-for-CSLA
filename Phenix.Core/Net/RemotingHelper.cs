using System;
using System.Collections;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Http;
using System.Runtime.Remoting.Channels.Tcp;
using System.Security.Permissions;
using Phenix.Core.Log;
using Phenix.Core.Net.RemotingCompression;

namespace Phenix.Core.Net
{
  /// <summary>
  /// Remoting助手
  /// </summary>
  [EnvironmentPermission(SecurityAction.LinkDemand)]
  public static class RemotingHelper
  {
    #region 属性

    private const string CLIENT_HTTP_CHANNEL_NAME = "ClientHttpBinary";
    private static HttpClientChannel _httpClientChannel;
    private const string CLIENT_TCP_CHANNEL_NAME = "ClientTcpBinary";
    private static TcpClientChannel _tcpClientChannel;

    private const string SERVER_HTTP_CHANNEL_NAME = "ServerHttpBinary";
    private static HttpServerChannel _httpServerChannel;
    private const string SERVER_TCP_CHANNEL_NAME = "ServerTcpBinary";
    private static TcpServerChannel _tcpServerChannel;

    private const string HTTP_CHANNEL_NAME = "HttpBinary";
    private static HttpChannel _httpChannel;
    private const string TCP_CHANNEL_NAME = "TcpBinary";
    private static TcpChannel _tcpChannel;

    private static readonly object _lock = new object();

    #endregion

    #region 方法

    /// <summary>
    /// 注册终端信道
    /// </summary>
    public static void RegisterClientChannel()
    {
      RegisterClientChannel(RemotingConfig.EnsureSecurity, RemotingConfig.CompressionSupported, RemotingConfig.CompressionThresholdMax);
    }

    /// <summary>
    /// 注册终端信道
    /// </summary>
    /// <param name="ensureSecurity">启用安全?</param>
    /// <param name="compressionSupported">支持压缩?</param>
    /// <param name="compressionThresholdMax">压缩最大阈值</param>
    public static void RegisterClientChannel(bool ensureSecurity,
      bool compressionSupported, int compressionThresholdMax)
    {
      switch (RemotingConfig.ServicesProtocolType)
      {
        case RemotingProtocolType.Http:
          RegisterHttpClientChannel(ensureSecurity);
          break;
        case RemotingProtocolType.Tcp:
          RegisterTcpClientChannel(ensureSecurity, compressionSupported, compressionThresholdMax);
          break;
      }
    }

    /// <summary>
    /// 注册终端信道
    /// </summary>
    /// <param name="ensureSecurity">启用安全?</param>
    public static void RegisterHttpClientChannel(bool ensureSecurity)
    {
      if (_httpClientChannel == null && ChannelServices.GetChannel(CLIENT_HTTP_CHANNEL_NAME) == null)
        lock (_lock)
          if (_httpClientChannel == null && ChannelServices.GetChannel(CLIENT_HTTP_CHANNEL_NAME) == null)
          {
            IDictionary properties = new Hashtable();
            properties["name"] = CLIENT_HTTP_CHANNEL_NAME;
            properties["timeout"] = RemotingConfig.Timeout;
            properties["useDefaultCredentials"] = RemotingConfig.AlwaysImpersonate;
            BinaryClientFormatterSinkProvider provider = new BinaryClientFormatterSinkProvider();
            //if (compressionSupported)
            //  provider.Next = new CompressionClientChannelSinkProvider(compressionThreshold);
            try
            {
              _httpClientChannel = new HttpClientChannel(properties, provider);
              ChannelServices.RegisterChannel(_httpClientChannel, ensureSecurity);
            }
            catch (RemotingException ex)
            {
              EventLog.SaveLocal(CLIENT_HTTP_CHANNEL_NAME, ex);
            }
          }
    }

    /// <summary>
    /// 注册终端信道
    /// </summary>
    /// <param name="ensureSecurity">启用安全?</param>
    /// <param name="compressionSupported">支持压缩?</param>
    /// <param name="compressionThresholdMax">压缩最大阈值</param>
    public static void RegisterTcpClientChannel(bool ensureSecurity,
      bool compressionSupported, int compressionThresholdMax)
    {
      if (_tcpClientChannel == null && ChannelServices.GetChannel(CLIENT_TCP_CHANNEL_NAME) == null)
        lock (_lock)
          if (_tcpClientChannel == null && ChannelServices.GetChannel(CLIENT_TCP_CHANNEL_NAME) == null)
          {
            IDictionary properties = new Hashtable();
            properties["name"] = CLIENT_TCP_CHANNEL_NAME;
            properties["timeout"] = RemotingConfig.Timeout;
            BinaryClientFormatterSinkProvider provider = new BinaryClientFormatterSinkProvider();
            if (compressionSupported)
              provider.Next = new CompressClientChannelSinkProvider(compressionThresholdMax);
            try
            {
              _tcpClientChannel = new TcpClientChannel(properties, provider);
              ChannelServices.RegisterChannel(_tcpClientChannel, ensureSecurity);
            }
            catch (RemotingException ex)
            {
              EventLog.SaveLocal(CLIENT_TCP_CHANNEL_NAME, ex);
            }
          }
    }

    /// <summary>
    /// 注册服务信道
    /// </summary>
    public static void RegisterServiceChannel()
    {
      switch (RemotingConfig.ServicesProtocolType)
      {
        case RemotingProtocolType.Http:
          RegisterHttpServiceChannel(RemotingConfig.HttpPort, RemotingConfig.EnsureSecurity);
          break;
        case RemotingProtocolType.Tcp:
          RegisterTcpServiceChannel(RemotingConfig.TcpPort, RemotingConfig.EnsureSecurity, RemotingConfig.CompressionSupported, RemotingConfig.CompressionThresholdMax);
          break;
      }
    }

    /// <summary>
    /// 注册服务信道
    /// </summary>
    /// <param name="port">端口</param>
    /// <param name="ensureSecurity">启用安全?</param>
    public static void RegisterHttpServiceChannel(int port, bool ensureSecurity)
    {
      if (_httpServerChannel == null && ChannelServices.GetChannel(SERVER_HTTP_CHANNEL_NAME) == null)
        lock (_lock)
          if (_httpServerChannel == null && ChannelServices.GetChannel(SERVER_HTTP_CHANNEL_NAME) == null)
          {
            BinaryServerFormatterSinkProvider provider = new BinaryServerFormatterSinkProvider();
            provider.TypeFilterLevel = System.Runtime.Serialization.Formatters.TypeFilterLevel.Full;
            //if (compressionSupported)
            //{
            //  CompressionServerChannelSinkProvider compressionProvider = new CompressionServerChannelSinkProvider(compressionThreshold);
            //  compressionProvider.Next = provider;
            //  ChannelServices.RegisterChannel(new HttpServerChannel(SERVER_HTTP_CHANNEL_NAME, port, compressionProvider), ensureSecurity);
            //}
            //else
            _httpServerChannel = new HttpServerChannel(SERVER_HTTP_CHANNEL_NAME, port, provider);
            try
            {
              ChannelServices.RegisterChannel(_httpServerChannel, ensureSecurity);
            }
            catch (RemotingException ex)
            {
              EventLog.SaveLocal(SERVER_HTTP_CHANNEL_NAME, ex);
            }
          }
    }

    /// <summary>
    /// 注册服务信道
    /// </summary>
    /// <param name="port">端口</param>
    /// <param name="ensureSecurity">启用安全?</param>
    /// <param name="compressionSupported">支持压缩?</param>
    /// <param name="compressionThresholdMax">压缩最大阈值</param>
    public static void RegisterTcpServiceChannel(int port, bool ensureSecurity,
      bool compressionSupported, int compressionThresholdMax)
    {
      if (_tcpServerChannel == null && ChannelServices.GetChannel(SERVER_TCP_CHANNEL_NAME) == null)
        lock (_lock)
          if (_tcpServerChannel == null && ChannelServices.GetChannel(SERVER_TCP_CHANNEL_NAME) == null)
          {
            BinaryServerFormatterSinkProvider provider = new BinaryServerFormatterSinkProvider();
            provider.TypeFilterLevel = System.Runtime.Serialization.Formatters.TypeFilterLevel.Full;
            if (compressionSupported)
            {
              CompressServerChannelSinkProvider compressionProvider = new CompressServerChannelSinkProvider(compressionThresholdMax);
              compressionProvider.Next = provider;
              _tcpServerChannel = new TcpServerChannel(SERVER_TCP_CHANNEL_NAME, port, compressionProvider);
            }
            else
              _tcpServerChannel = new TcpServerChannel(SERVER_TCP_CHANNEL_NAME, port, provider);
            try
            {
              ChannelServices.RegisterChannel(_tcpServerChannel, ensureSecurity);
            }
            catch (RemotingException ex)
            {
              EventLog.SaveLocal(SERVER_TCP_CHANNEL_NAME, ex);
            }
          }
    }

    /// <summary>
    /// 注册客户激活的终端信道
    /// </summary>
    public static void RegisterActivatedClientChannel()
    {
      switch (RemotingConfig.ServicesProtocolType)
      {
        case RemotingProtocolType.Http:
          RegisterHttpActivatedClientChannel(RemotingConfig.EnsureSecurity);
          break;
        case RemotingProtocolType.Tcp:
          RegisterTcpActivatedClientChannel(RemotingConfig.EnsureSecurity, RemotingConfig.CompressionSupported, RemotingConfig.CompressionThresholdMax);
          break;
      }
    }

    /// <summary>
    /// 注册客户激活的终端信道
    /// </summary>
    /// <param name="ensureSecurity">启用安全?</param>
    public static void RegisterHttpActivatedClientChannel(bool ensureSecurity)
    {
      if (_httpChannel == null && ChannelServices.GetChannel(HTTP_CHANNEL_NAME) == null)
        lock (_lock)
          if (_httpChannel == null && ChannelServices.GetChannel(HTTP_CHANNEL_NAME) == null)
          {
            IDictionary properties = new Hashtable();
            properties["port"] = 0;
            properties["name"] = HTTP_CHANNEL_NAME;
            properties["timeout"] = RemotingConfig.Timeout;
            properties["useDefaultCredentials"] = RemotingConfig.AlwaysImpersonate;
            BinaryClientFormatterSinkProvider clientProvider = new BinaryClientFormatterSinkProvider();
            BinaryServerFormatterSinkProvider serverProvider = new BinaryServerFormatterSinkProvider();
            serverProvider.TypeFilterLevel = System.Runtime.Serialization.Formatters.TypeFilterLevel.Full;
            //if (compressionEnabled)
            //{
            //  clientProvider.Next = new CompressionClientChannelSinkProvider(compressionThreshold);
            //  CompressionServerChannelSinkProvider serverCompressionProvider = new CompressionServerChannelSinkProvider(compressionThreshold);
            //  serverCompressionProvider.Next = serverProvider;
            //  ChannelServices.RegisterChannel(new HttpChannel(properties, clientProvider, serverCompressionProvider), ensureSecurity);
            //}
            //else
            _httpChannel = new HttpChannel(properties, clientProvider, serverProvider);
            try
            {
              ChannelServices.RegisterChannel(_httpChannel, ensureSecurity);
            }
            catch (RemotingException ex)
            {
              EventLog.SaveLocal(HTTP_CHANNEL_NAME, ex);
            }
          }
    }

    /// <summary>
    /// 注册客户激活的终端信道
    /// </summary>
    /// <param name="ensureSecurity">启用安全?</param>
    /// <param name="compressionEnabled">压缩?</param>
    /// <param name="compressionThresholdMax">压缩最大阈值</param>
    public static void RegisterTcpActivatedClientChannel(bool ensureSecurity, bool compressionEnabled, int compressionThresholdMax)
    {
      if (_tcpChannel == null && ChannelServices.GetChannel(TCP_CHANNEL_NAME) == null)
        lock (_lock)
          if (_tcpChannel == null && ChannelServices.GetChannel(TCP_CHANNEL_NAME) == null)
          {
            IDictionary properties = new Hashtable();
            properties["port"] = 0;
            properties["name"] = TCP_CHANNEL_NAME;
            properties["timeout"] = RemotingConfig.Timeout;
            BinaryClientFormatterSinkProvider clientProvider = new BinaryClientFormatterSinkProvider();
            BinaryServerFormatterSinkProvider serverProvider = new BinaryServerFormatterSinkProvider();
            serverProvider.TypeFilterLevel = System.Runtime.Serialization.Formatters.TypeFilterLevel.Full;
            if (compressionEnabled)
            {
              clientProvider.Next = new CompressClientChannelSinkProvider(compressionThresholdMax);
              CompressServerChannelSinkProvider serverCompressionProvider = new CompressServerChannelSinkProvider(compressionThresholdMax);
              serverCompressionProvider.Next = serverProvider;
              _tcpChannel = new TcpChannel(properties, clientProvider, serverCompressionProvider);
            }
            else
              _tcpChannel = new TcpChannel(properties, clientProvider, serverProvider);
            try
            {
              ChannelServices.RegisterChannel(_tcpChannel, ensureSecurity);
            }
            catch (RemotingException ex)
            {
              EventLog.SaveLocal(TCP_CHANNEL_NAME, ex);
            }
          }
    }

    #region 注册客户激活的远程服务

    /// <summary>
    /// 注册客户激活的远程服务
    /// </summary>
    public static void RegisterActivatedClientType(Type objectType)
    {
      RegisterActivatedClientType(objectType, NetConfig.ServicesAddress);
    }

    /// <summary>
    /// 注册客户激活的远程服务
    /// </summary>
    public static void RegisterActivatedClientType(Type objectType, string host)
    {
      switch (RemotingConfig.ServicesProtocolType)
      {
        case RemotingProtocolType.Http:
          RegisterHttpActivatedClientType(objectType, host);
          break;
        case RemotingProtocolType.Tcp:
          RegisterTcpActivatedClientType(objectType, host);
          break;
      }
    }

    /// <summary>
    /// 注册客户激活的远程服务
    /// </summary>
    public static void RegisterHttpActivatedClientType(Type objectType, string host)
    {
      RegisterHttpActivatedClientType(objectType, host, NetConfig.LinkBackupServices ? RemotingConfig.HttpPort + 1 : RemotingConfig.HttpPort);
    }

    /// <summary>
    /// 注册客户激活的远程服务
    /// </summary>
    public static void RegisterTcpActivatedClientType(Type objectType, string host)
    {
      RegisterTcpActivatedClientType(objectType, host, NetConfig.LinkBackupServices ? RemotingConfig.TcpPort + 1 : RemotingConfig.TcpPort);
    }

    /// <summary>
    /// 注册客户激活的远程服务
    /// </summary>
    public static void RegisterActivatedClientType(Type objectType, string host, int port)
    {
      switch (RemotingConfig.ServicesProtocolType)
      {
        case RemotingProtocolType.Http:
          RegisterHttpActivatedClientType(objectType, host, port);
          break;
        case RemotingProtocolType.Tcp:
          RegisterTcpActivatedClientType(objectType, host, port);
          break;
      }
    }

    /// <summary>
    /// 注册客户激活的远程服务
    /// </summary>
    public static void RegisterHttpActivatedClientType(Type objectType, string host, int port)
    {
      RemotingConfiguration.RegisterActivatedClientType(objectType, String.Format(@"http://{0}:{1}", host, port));
    }

    /// <summary>
    /// 注册客户激活的远程服务
    /// </summary>
    public static void RegisterTcpActivatedClientType(Type objectType, string host, int port)
    {
      RemotingConfiguration.RegisterActivatedClientType(objectType, String.Format(@"tcp://{0}:{1}", host, port));
    }

    #endregion

    #region 注册服务激活的远程服务

    /// <summary>
    /// 注册服务激活的远程服务
    /// </summary>
    public static void RegisterWellKnownClientType(Type objectType, string path)
    {
      RegisterWellKnownClientType(objectType, NetConfig.ServicesAddress, path);
    }

    /// <summary>
    /// 注册服务激活的远程服务
    /// </summary>
    public static void RegisterWellKnownClientType(Type objectType, string host, string path)
    {
      switch (RemotingConfig.ServicesProtocolType)
      {
        case RemotingProtocolType.Http:
          RegisterHttpWellKnownClientType(objectType, host, path);
          break;
        case RemotingProtocolType.Tcp:
          RegisterTcpWellKnownClientType(objectType, host, path);
          break;
      }
    }

    /// <summary>
    /// 注册服务激活的远程服务
    /// </summary>
    public static void RegisterHttpWellKnownClientType(Type objectType, string host, string path)
    {
      RegisterHttpWellKnownClientType(objectType, host, NetConfig.LinkBackupServices ? RemotingConfig.HttpPort + 1 : RemotingConfig.HttpPort, path);
    }

    /// <summary>
    /// 注册服务激活的远程服务
    /// </summary>
    public static void RegisterTcpWellKnownClientType(Type objectType, string host, string path)
    {
      RegisterTcpWellKnownClientType(objectType, host, NetConfig.LinkBackupServices ? RemotingConfig.TcpPort + 1 : RemotingConfig.TcpPort, path);
    }

    /// <summary>
    /// 注册服务激活的远程服务
    /// </summary>
    public static void RegisterWellKnownClientType(Type objectType, string host, int port, string path)
    {
      switch (RemotingConfig.ServicesProtocolType)
      {
        case RemotingProtocolType.Http:
          RegisterHttpWellKnownClientType(objectType, host, port, path);
          break;
        case RemotingProtocolType.Tcp:
          RegisterTcpWellKnownClientType(objectType, host, port, path);
          break;
      }
    }

    /// <summary>
    /// 注册服务激活的远程服务
    /// </summary>
    public static void RegisterHttpWellKnownClientType(Type objectType, string host, int port, string path)
    {
      RemotingConfiguration.RegisterWellKnownClientType(objectType, FormatHttpUrl(host, port, path));
    }

    /// <summary>
    /// 注册服务激活的远程服务
    /// </summary>
    public static void RegisterTcpWellKnownClientType(Type objectType, string host, int port, string path)
    {
      RemotingConfiguration.RegisterWellKnownClientType(objectType, FormatTcpUrl(host, port, path));
    }

    #endregion

    #region 远程对象代理工厂

    /// <summary>
    /// 远程对象代理工厂
    /// </summary>
    public static object CreateRemoteObjectProxy(Type objectType, string path)
    {
      return CreateRemoteObjectProxy(objectType, NetConfig.ServicesAddress, path);
    }

    /// <summary>
    /// 远程对象代理工厂
    /// </summary>
    public static object CreateRemoteObjectProxy(Type objectType, string host, string path)
    {
      switch (RemotingConfig.ServicesProtocolType)
      {
        case RemotingProtocolType.Http:
          return CreateHttpRemoteObjectProxy(objectType, host, path);
        case RemotingProtocolType.Tcp:
          return CreateTcpRemoteObjectProxy(objectType, host, path);
        default:
          return null;
      }
    }

    /// <summary>
    /// 远程对象代理工厂
    /// </summary>
    public static Object CreateHttpRemoteObjectProxy(Type objectType, string host, string path)
    {
      return CreateHttpRemoteObjectProxy(objectType, host, NetConfig.LinkBackupServices ? RemotingConfig.HttpPort + 1 : RemotingConfig.HttpPort, path);
    }

    /// <summary>
    /// 远程对象代理工厂
    /// </summary>
    public static Object CreateTcpRemoteObjectProxy(Type objectType, string host, string path)
    {
      return CreateTcpRemoteObjectProxy(objectType, host, NetConfig.LinkBackupServices ? RemotingConfig.TcpPort + 1 : RemotingConfig.TcpPort, path);
    }

    /// <summary>
    /// 远程对象代理工厂
    /// </summary>
    public static object CreateRemoteObjectProxy(Type objectType, string host, int port, string path)
    {
      switch (RemotingConfig.ServicesProtocolType)
      {
        case RemotingProtocolType.Http:
          return CreateHttpRemoteObjectProxy(objectType, host, port, path);
        case RemotingProtocolType.Tcp:
          return CreateTcpRemoteObjectProxy(objectType, host, port, path);
        default:
          return null;
      }
    }

    /// <summary>
    /// 远程对象代理工厂
    /// </summary>
    public static Object CreateHttpRemoteObjectProxy(Type objectType, string host, int port, string path)
    {
      return Activator.GetObject(objectType, FormatHttpUrl(host, port, path));
    }

    /// <summary>
    /// 远程对象代理工厂
    /// </summary>
    public static Object CreateTcpRemoteObjectProxy(Type objectType, string host, int port, string path)
    {
      return Activator.GetObject(objectType, FormatTcpUrl(host, port, path));
    }

    #endregion

    #region FormatUrl

    /// <summary>
    /// FormatUrl
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1055:UriReturnValuesShouldNotBeStrings")]
    public static string FormatRemoteObjectUrl(string path)
    {
      return FormatRemoteObjectUrl(NetConfig.ServicesAddress, path);
    }

    /// <summary>
    /// FormatUrl
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1055:UriReturnValuesShouldNotBeStrings")]
    public static string FormatRemoteObjectUrl(string host, string path)
    {
      switch (RemotingConfig.ServicesProtocolType)
      {
        case RemotingProtocolType.Http:
          if (NetConfig.LinkBackupServices)
            return FormatHttpUrl(host, RemotingConfig.HttpPort + 1, path);
          else
            return FormatHttpUrl(host, RemotingConfig.HttpPort, path);
        case RemotingProtocolType.Tcp:
          if (NetConfig.LinkBackupServices)
            return FormatTcpUrl(host, RemotingConfig.TcpPort + 1, path);
          else
            return FormatTcpUrl(host, RemotingConfig.TcpPort, path);
        default:
          throw new InvalidOperationException();
      }
    }

    /// <summary>
    /// FormatUrl
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1055:UriReturnValuesShouldNotBeStrings")]
    public static string FormatHttpUrl(string host, int port, string path)
    {
      if (host.IndexOf(':') > 0)
        return String.Format(@"http://{0}/{1}", host, path);
      else
        return String.Format(@"http://{0}:{1}/{2}", host, port, path);
    }

    /// <summary>
    /// FormatUrl
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1055:UriReturnValuesShouldNotBeStrings")]
    public static string FormatTcpUrl(string host, int port, string path)
    {
      if (host.IndexOf(':') > 0)
        return String.Format(@"tcp://{0}/{1}", host, path);
      else
        return String.Format(@"tcp://{0}:{1}/{2}", host, port, path);
    }

    #endregion

    #endregion
  }
}