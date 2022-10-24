using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using Microsoft.Win32;
using Phenix.Core.SyncCollections;

namespace Phenix.Core.Net
{
  /// <summary>
  /// 网络配置信息
  /// </summary>
  public static class NetConfig
  {
    #region 属性

    /// <summary>
    /// 本地主机
    /// </summary>
    public const string LOCAL_HOST = "localhost";

    /// <summary>
    /// 本地IP
    /// </summary>
    public const string LOCAL_IP = "127.0.0.1";

    /// <summary>
    /// 嵌入式服务
    /// </summary>
    public const string EMBEDDED_SERVICE = "embedded";

    private const string TCPIP_PARAMETERS_KEY = @"SYSTEM\CurrentControlSet\Services\TCPIP\Parameters";

    private const string TCP_TIMED_WAIT_DELAY_NAME = "TcpTimedWaitDelay";
    private static int? _tcpTimedWaitDelay;
    /// <summary>
    /// TCP/IP 可释放已关闭连接并重用其资源前必须经过的时间（毫秒）
    /// 缺省为30秒（30000毫秒）（30-300）
    /// </summary>
    public static int TcpTimedWaitDelay
    {
      get
      {
        if (!_tcpTimedWaitDelay.HasValue)
        {
          object registryValue = null;
          RegistryKey registryKey = Registry.LocalMachine.OpenSubKey(TCPIP_PARAMETERS_KEY);
          if (registryKey != null)
            try
            {
              registryValue = registryKey.GetValue(TCP_TIMED_WAIT_DELAY_NAME);
            }
            finally
            {
              registryKey.Close();
            }
          _tcpTimedWaitDelay = (registryValue != null ? (int)registryValue : 30) * 1000;
        }
        return _tcpTimedWaitDelay.Value;
      }
      set
      {
        if (value < 30000)
          value = 30000;
        else if (value > 300000)
          value = 300000;
        RegistryKey registryKey = Registry.LocalMachine.OpenSubKey(TCPIP_PARAMETERS_KEY, true);
        if (registryKey != null)
          try
          {
            registryKey.SetValue(TCP_TIMED_WAIT_DELAY_NAME, value / 1000);
          }
          finally
          {
            registryKey.Close();
          }
        _tcpTimedWaitDelay = value;
      }
    }

    private const string MAX_USER_PORT_NAME = "MaxUserPort";
    private static int? _maxUserPort;
    /// <summary>
    /// TCP/IP 端口最大连接数
    /// 缺省为65534（5000-65534）
    /// </summary>
    public static int MaxUserPort
    {
      get
      {
        if (!_maxUserPort.HasValue)
        {
          object registryValue = null;
          RegistryKey registryKey = Registry.LocalMachine.OpenSubKey(TCPIP_PARAMETERS_KEY);
          if (registryKey != null)
            try
            {
              registryValue = registryKey.GetValue(MAX_USER_PORT_NAME);
            }
            finally
            {
              registryKey.Close();
            }
          _maxUserPort = registryValue != null ? (int)registryValue : 65534;
        }
        return _maxUserPort.Value;
      }
      set
      {
        if (value < 5000)
          value = 5000;
        else if (value > 65534)
          value = 65534;
        RegistryKey registryKey = Registry.LocalMachine.OpenSubKey(TCPIP_PARAMETERS_KEY, true);
        if (registryKey != null)
          try
          {
            registryKey.SetValue(MAX_USER_PORT_NAME, value);
          }
          finally
          {
            registryKey.Close();
          }
        _maxUserPort = value;
      }
    }

    private static string _localAddress;
    /// <summary>
    /// 本机IP地址
    /// </summary>
    public static string LocalAddress
    {
      get
      {
        if (String.IsNullOrEmpty(_localAddress))
        {
          string result = null;
          foreach (IPAddress address in Dns.GetHostAddresses(Dns.GetHostName()))
            if (address.AddressFamily == AddressFamily.InterNetwork)
            {
              result = address.ToString();
              break;
            }
          if (String.IsNullOrEmpty(result))
            result = LOCAL_IP;
          _localAddress = result;
        }
        return _localAddress;
      }
    }

    private static bool? _servicesFixed;
    /// <summary>
    /// 服务器固定?
    /// 缺省为 false
    /// </summary>
    public static bool ServicesFixed
    {
      get { return AppSettings.GetProperty(ref _servicesFixed, false); }
      set { AppSettings.SetProperty(ref _servicesFixed, value); }
    }

    private static ProxyType? _proxyType;
    /// <summary>
    /// 代理类型?
    /// 缺省为 ProxyType.Remoting
    /// </summary>
    public static ProxyType ProxyType
    {
      get
      {
        if (String.Compare(ServicesAddress, EMBEDDED_SERVICE, StringComparison.OrdinalIgnoreCase) == 0)
          return ProxyType.Embedded;
        return AppSettings.GetProperty(ref _proxyType, ProxyType.Remoting);
      }
      set { AppSettings.SetProperty(ref _proxyType, value); }
    }

    private static bool _linkBackupServices;
    internal static bool LinkBackupServices 
    {
      get { return _linkBackupServices; }
    }

    private static string _servicesAddressPost;
    private static string _servicesAddress;
    /// <summary>
    /// 服务器IP地址
    /// 缺省为 EMBEDDED_SERVICE
    /// </summary>
    public static string ServicesAddress
    {
      get
      {
        if (String.IsNullOrEmpty(_servicesAddress))
        {
          string result = null;
          string[] servicesAddressssParts = GetServicesAddressesParts();
          if (!ServicesFixed && servicesAddressssParts.Length > 0)
            result = servicesAddressssParts[0];
          if (String.IsNullOrEmpty(result))
          {
            AppSettings.GetProperty(ref result, EMBEDDED_SERVICE, false);
            if (String.IsNullOrEmpty(result))
              if (ServicesFixed && servicesAddressssParts.Length > 0)
                result = servicesAddressssParts[new Random().Next(servicesAddressssParts.Length)];
            ;
          }
          if (String.Compare(result, LOCAL_HOST, StringComparison.OrdinalIgnoreCase) == 0)
            result = LOCAL_IP;
          _servicesAddress = result;
        }
        return _servicesAddress;
      }
      set
      {
        if (String.Compare(value, LOCAL_HOST, StringComparison.OrdinalIgnoreCase) == 0)
          value = LOCAL_IP;
        if (String.IsNullOrEmpty(value))
          _servicesAddress = null;
        else
          AppSettings.SetProperty(ref _servicesAddress, value);
      }
    }

    private static string _servicesAddresses;
    /// <summary>
    /// 可登陆的服务器IP地址(由AppSettings.Separator分隔)集合
    /// 缺省为 EMBEDDED_SERVICE
    /// </summary>
    public static string ServicesAddresses
    {
      get { return AppSettings.GetProperty(ref _servicesAddresses, EMBEDDED_SERVICE, false); }
      set
      {
        if (!String.IsNullOrEmpty(value))
          AppSettings.SetProperty(ref _servicesAddresses, value);
      }
    }

    private static readonly SynchronizedDictionary<string, string> _servicesCluster =
      new SynchronizedDictionary<string, string>(StringComparer.Ordinal);

    #endregion

    #region 方法

    private static string[] GetServicesAddressesParts()
    {
      return ServicesAddresses.Split(new string[] { AppConfig.SEPARATOR }, StringSplitOptions.RemoveEmptyEntries);
    }

    #region Switch

    /// <summary>
    /// 初始化转盘
    /// </summary>
    public static void InitializeSwitch()
    {
      InitializeSwitch(false);
    }

    private static void InitializeSwitch(bool linkBackupServices)
    {
      _linkBackupServices = linkBackupServices;
      _servicesAddressPost = ServicesAddress;
    }

    /// <summary>
    /// 转盘, 切换到下一个ServicesAddress
    /// </summary>
    public static bool SwitchServicesAddress()
    {
      List<string> servicesAddresses = new List<string>();
      foreach (string s in GetServicesAddressesParts())
        if (!servicesAddresses.Contains(s))
          servicesAddresses.Add(s);
      if (servicesAddresses.Count == 0)
        return false;
      if (String.IsNullOrEmpty(_servicesAddressPost))
        _servicesAddressPost = !String.IsNullOrEmpty(ServicesAddress) ? ServicesAddress : servicesAddresses[0];
      Queue<string> servicesAddressQueue = new Queue<string>(servicesAddresses);
      //整理出以_servicesAddressPost为头的单链
      for (int i = 0; i < servicesAddressQueue.Count; i++)
      {
        if (String.Compare(servicesAddressQueue.Peek(), _servicesAddressPost, StringComparison.OrdinalIgnoreCase) == 0)
          break;
        servicesAddressQueue.Enqueue(servicesAddressQueue.Dequeue());
      }
      //单链从头逐个剔除节点直到剔除了ServicesAddress为止
      for (int i = servicesAddressQueue.Count; i > 0; i--)
        if (String.Compare(servicesAddressQueue.Dequeue(), ServicesAddress, StringComparison.OrdinalIgnoreCase) == 0)
          break;
      //取下一个ServicesAddress
      if (servicesAddressQueue.Count > 0)
      {
        ServicesAddress = servicesAddressQueue.Peek();
        return true;
      }
      if (!LinkBackupServices)
      {
        InitializeSwitch(true);
        return true;
      }
      return false;
    }

    #endregion

    #region ServicesCluster

    private static string AssembleServicesClusterKey(string key)
    {
      return String.Format("ServicesCluster.{0}", key);
    }

    /// <summary>
    /// 注册服务集群
    /// </summary>
    /// <param name="key">键</param>
    /// <param name="servicesAddress">服务器IP地址</param>
    public static void RegisterServicesCluster(string key, string servicesAddress)
    {
      _servicesCluster[key] = servicesAddress;
      AppSettings.SaveValue(AssembleServicesClusterKey(key), servicesAddress);
    }

    /// <summary>
    /// 获取服务集群的服务器IP地址
    /// </summary>
    /// <param name="key">键</param>
    public static string GetServicesClusterAddress(string key)
    {
      string result;
      if (_servicesCluster.TryGetValue(key, out result))
        return result;
      return AppSettings.ReadValue(AssembleServicesClusterKey(key));
    }

    /// <summary>
    /// 获取服务集群的服务器IP地址
    /// </summary>
    public static string GetServicesClusterAddress(ServicesClusterAttribute servicesClusterAttribute)
    {
      if (servicesClusterAttribute == null || String.IsNullOrEmpty(servicesClusterAttribute.Key))
        return null;
      return GetServicesClusterAddress(servicesClusterAttribute.Key);
    }

    #endregion

    #endregion
  }
}