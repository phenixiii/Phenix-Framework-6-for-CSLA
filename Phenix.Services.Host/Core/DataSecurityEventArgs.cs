using System;
using Phenix.Core.Security;

namespace Phenix.Services.Host.Core
{
  /// <summary>
  /// 数据安全事件数据
  /// </summary>
  internal class DataSecurityEventArgs : EventArgs
  {
    public DataSecurityEventArgs(string localAddress, string userNumber, bool logOn)
      : base()
    {
      _localAddress = localAddress;
      _userNumber = userNumber ?? "anonymity";
      _logOn = logOn;
      _time = DateTime.Now;
    }

    public DataSecurityEventArgs(UserIdentity identity, bool logOn)
      : this(identity.LocalAddress, identity.UserNumber, logOn) { }

    #region 属性

    private string _localAddress;
    /// <summary>
    /// 本地IP地址
    /// </summary>
    public string LocalAddress
    {
      get { return _localAddress; }
      set { _localAddress = value; }
    }

    private readonly string _userNumber;
    /// <summary>
    /// 登录工号
    /// </summary>
    public string UserNumber
    {
      get { return _userNumber; }
    }

    private readonly bool _logOn;
    /// <summary>
    /// 登录?
    /// </summary>
    public bool LogOn
    {
      get { return _logOn; }
    }

    private readonly DateTime _time;
    /// <summary>
    /// 时间
    /// </summary>
    public DateTime Time
    {
      get { return _time; }
    }

    #endregion
  }
}