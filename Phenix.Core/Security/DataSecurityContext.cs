using System;

namespace Phenix.Core.Security
{
  /// <summary>
  /// 数据安全上下文
  /// </summary>
  [Serializable]
  public class DataSecurityContext
  {
    /// <summary>
    /// 初始化
    /// </summary>
    public DataSecurityContext(UserIdentity identity)
    {
      _identity = identity;
      _systemDate = DateTime.Now;
    }

    /// <summary>
    /// 初始化
    /// </summary>
    public DataSecurityContext(UserIdentity identity, string hosts)
      : this(identity)
    {
      _hosts = hosts;
    }

    /// <summary>
    /// 初始化
    /// </summary>
    public DataSecurityContext(UserIdentity identity, string hosts, string message)
      : this(identity, hosts)
    {
      identity.AuthenticatedMessage = message;
    }

    #region 属性
    
    /// <summary>
    /// 内部身份验证类型
    /// </summary>
    public static string InternalAuthenticationType
    {
      get { return "Phenix"; }
    }

    private readonly UserIdentity _identity;
    /// <summary>
    /// 用户身份
    /// </summary>
    public UserIdentity Identity
    {
      get { return _identity; }
    }

    private readonly DateTime _systemDate;
    /// <summary>
    /// 系统时间
    /// </summary>
    public DateTime SystemDate
    {
      get { return _systemDate; }
    }

    private readonly string _hosts;
    /// <summary>
    /// 可登陆的服务器IP地址(由AppSettings.Separator分隔)集合
    /// </summary>
    public string Hosts
    {
      get { return _hosts; }
    }

    /// <summary>
    /// 消息
    /// </summary>
    public string Message
    {
      get { return _identity.AuthenticatedMessage; }
    }

    #endregion
  }
}
