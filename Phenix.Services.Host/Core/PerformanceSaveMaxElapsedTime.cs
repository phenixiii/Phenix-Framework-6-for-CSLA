using System;
using Phenix.Core.Security;

namespace Phenix.Services.Host.Core
{
  /// <summary>
  /// Save性能最大耗时
  /// </summary>
  internal class PerformanceSaveMaxElapsedTime
  {
    private PerformanceSaveMaxElapsedTime(string businessName, double value, long recordCount, string userNumber, DateTime time)
    {
      _businessName = businessName;
      _value = value;
      _recordCount = recordCount;
      _userNumber = userNumber;
      _time = time;
    }

    public PerformanceSaveMaxElapsedTime(string businessName, double value, long recordCount, IIdentity identity)
      : this(businessName, value, recordCount, identity != null ? identity.UserNumber : Phenix.Core.Code.Converter.NullSymbolic, DateTime.Now) { }

    #region 属性

    private readonly string _businessName;
    /// <summary>
    /// 业务类名
    /// </summary>
    public string BusinessName
    {
      get { return _businessName; }
    }

    private readonly double _value;
    /// <summary>
    /// 值(秒)
    /// </summary>
    public double Value
    {
      get { return _value; }
    }

    private readonly long _recordCount;
    /// <summary>
    /// 记录数
    /// </summary>
    public long RecordCount
    {
      get { return _recordCount; }
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
      return String.Format("{0}: over the {1} seconds elapsed for save {2} records by {3} at {4}", BusinessName, Value, RecordCount, UserNumber, Time);
    }

    #endregion
  }
}
