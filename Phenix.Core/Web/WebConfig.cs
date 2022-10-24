namespace Phenix.Core.Web
{
  /// <summary>
  /// Web配置信息
  /// </summary>
  public static class WebConfig
  {
    /// <summary>
    /// X-HTTP-Method-Override
    /// </summary>
    public const string METHOD_OVERRIDE_HEADER_NAME = "X-HTTP-Method-Override";

    /// <summary>
    /// Phenix-Authorization
    /// </summary>
    public const string AUTHORIZATION_HEADER_NAME = "Phenix-Authorization";

    /// <summary>
    /// /api/Security
    /// </summary>
    public const string SECURITY_URI = "/api/Security";

    private static int? _webApiPort;
    /// <summary>
    /// WebAPI端口
    /// </summary>
    public static int WebApiPort
    {
      get { return AppSettings.GetProperty(ref _webApiPort, 8080); }
      set { AppSettings.SetProperty(ref _webApiPort, value); }
    }

    private static int? _webApiSslPort;
    /// <summary>
    /// WebAPI+SSL端口
    /// </summary>
    public static int WebApiSslPort
    {
      get { return AppSettings.GetProperty(ref _webApiSslPort, 8443); }
      set { AppSettings.SetProperty(ref _webApiSslPort, value); }
    }

    private static int? _webSocketPort;
    /// <summary>
    /// WebSocket端口
    /// </summary>
    public static int WebSocketPort
    {
      get { return AppSettings.GetProperty(ref _webSocketPort, 8081); }
      set { AppSettings.SetProperty(ref _webSocketPort, value); }
    }

    private static int? _webSocketSslPort;
    /// <summary>
    /// WebSocket+SSL端口
    /// </summary>
    public static int WebSocketSslPort
    {
      get { return AppSettings.GetProperty(ref _webSocketSslPort, 8444); }
      set { AppSettings.SetProperty(ref _webSocketSslPort, value); }
    }
  }
}
