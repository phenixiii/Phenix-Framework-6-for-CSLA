namespace Phenix.Core.Net
{
  /// <summary>
  /// Remoting配置信息
  /// </summary>
  public static class RemotingConfig
  {
    #region 属性

    private static RemotingProtocolType? _servicesProtocolType;
    /// <summary>
    /// 协议类型
    /// 缺省为 TCP
    /// </summary>
    public static RemotingProtocolType ServicesProtocolType
    {
      get { return AppSettings.GetProperty(ref _servicesProtocolType, RemotingProtocolType.Tcp); }
      set { AppSettings.SetProperty(ref _servicesProtocolType, value); }
    }

    private static int? _httpPort;
    /// <summary>
    /// HTTP端口
    /// 缺省为 8084
    /// </summary>
    public static int HttpPort
    {
      get { return AppSettings.GetProperty(ref _httpPort, 8084); }
      set { AppSettings.SetProperty(ref _httpPort, value); }
    }

    private static int? _tcpPort;
    /// <summary>
    /// TCP端口
    /// 缺省为 8086
    /// </summary>
    public static int TcpPort
    {
      get { return AppSettings.GetProperty(ref _tcpPort, 8086); }
      set { AppSettings.SetProperty(ref _tcpPort, value); }
    }

    private static bool? _ensureSecurity;
    /// <summary>
    /// 信道安全?
    /// 缺省为 false
    /// </summary>
    public static bool EnsureSecurity
    {
      get { return AppSettings.GetProperty(ref _ensureSecurity, false); }
      set { AppSettings.SetProperty(ref _ensureSecurity, value); }
    }

    private static bool? _compressionSupported;
    /// <summary>
    /// 支持压缩?
    /// 缺省为 false
    /// </summary>
    public static bool CompressionSupported
    {
      get { return AppSettings.GetProperty(ref _compressionSupported, false); }
      set { AppSettings.SetProperty(ref _compressionSupported, value); }
    }

    private static int? _compressionThresholdMax;
    /// <summary>
    /// 压缩最大阈值：传输流长度
    /// 缺省为 1024 * 1024 * 8
    /// </summary>
    public static int CompressionThresholdMax
    {
      get { return AppSettings.GetProperty(ref _compressionThresholdMax, 1024 * 1024 * 8); }
      set { AppSettings.SetProperty(ref _compressionThresholdMax, value); }
    }

    private static int? _timeout;
    /// <summary>
    /// 指定请求在超时前等待的毫秒数, 0 或 -1 指示超时期限无限
    /// 缺省为 System.Threading.Timeout.Infinite
    /// </summary>
    public static int Timeout
    {
      get { return AppSettings.GetProperty(ref _timeout, System.Threading.Timeout.Infinite); }
      set { AppSettings.SetProperty(ref _timeout, value); }
    }

    private static bool? _alwaysImpersonate;
    /// <summary>
    /// 指定在对服务器信道进行身份验证时，是否为与当前线程关联的身份提供凭据
    /// 缺省为 AppConfig.AppType == AppType.Winform
    /// </summary>
    public static bool AlwaysImpersonate
    {
      get { return AppSettings.GetProperty(ref _alwaysImpersonate, AppConfig.AppType == AppType.Winform); }
      set { AppSettings.SetProperty(ref _alwaysImpersonate, value); }
    }

    #endregion
  }
}
