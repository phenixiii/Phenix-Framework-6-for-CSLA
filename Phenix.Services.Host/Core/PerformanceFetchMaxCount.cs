using System;
using Phenix.Core.Security;

namespace Phenix.Services.Host.Core
{
  /// <summary>
  /// Fetch性能最大记录数
  /// </summary>
  internal class PerformanceFetchMaxCount
  {
    private PerformanceFetchMaxCount(string businessName, long value, string userNumber, DateTime time)
    {
      _businessName = businessName;
      _value = value;
      _userNumber = userNumber;
      _time = time;
    }

    public PerformanceFetchMaxCount(string businessName, long value, IIdentity identity)
      : this(businessName, value, identity != null ? identity.UserNumber : Phenix.Core.Code.Converter.NullSymbolic, DateTime.Now) { }

    #region 属性

    private readonly string _businessName;
    /// <summary>
    /// 业务类名
    /// </summary>
    public string BusinessName
    {
      get { return _businessName; }
    }

    private readonly long _value;
    /// <summary>
    /// 值(条)
    /// </summary>
    public long Value
    {
      get { return _value; }
    }

    private readonly string _userNumber;
    /// <summary>
    /// 登录工号
    /// </summary>
    public string UserNumber
    {
      get { return _userNumber; }
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

    #region 方法

    public override string ToString()
    {
      return String.Format("{0}: fetch {1} records by {2} at {3}", BusinessName, Value, UserNumber, Time);
    }

    #endregion
  }
}
