namespace Phenix.Core.Net
{
  /// <summary>
  /// WCF配置信息
  /// </summary>
  public static class WcfConfig
  {
    #region 属性

    private static WcfProtocolType? _servicesProtocolType;
    /// <summary>
    /// WCF协议类型
    /// 缺省为 NetTcp
    /// </summary>
    public static WcfProtocolType ServicesProtocolType
    {
      get { return AppSettings.GetProperty(ref _servicesProtocolType, WcfProtocolType.NetTcp); }
      set { AppSettings.SetProperty(ref _servicesProtocolType, value); }
    }

    private static int? _basicHttpPort;
    /// <summary>
    /// BasicHTTP端口
    /// 缺省为 9084
    /// </summary>
    public static int BasicHttpPort
    {
      get { return AppSettings.GetProperty(ref _basicHttpPort, 9084); }
      set { AppSettings.SetProperty(ref _basicHttpPort, value); }
    }

    private static int? _netTcpPort;
    /// <summary>
    /// NetTCP端口
    /// 缺省为 9086
    /// </summary>
    public static int NetTcpPort
    {
      get { return AppSettings.GetProperty(ref _netTcpPort, 9086); }
      set { AppSettings.SetProperty(ref _netTcpPort, value); }
    }

    #endregion
  }
}
