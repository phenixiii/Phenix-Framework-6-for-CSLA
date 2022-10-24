using System;

namespace Phenix.Core.Security
{
  /// <summary>
  /// 过程锁资料
  /// </summary>
  [Serializable]
  public sealed class ProcessLockInfo : ISecurityInfo
  {
    /// <summary>
    /// 过程锁资料
    /// </summary>
    public ProcessLockInfo(string processName, string caption, bool locked, DateTime time, DateTime expiryTime, string userNumber, string remark) 
    {
      _processName = processName;
      _caption = caption;
      _locking = locked;
      _time = time;
      _expiryTime = expiryTime;
      _userNumber = userNumber;
      _remark = remark;
    }

    #region 属性

    private readonly string _processName;
    /// <summary>
    /// 过程名称
    /// </summary>
    public string ProcessName
    {
      get { return _processName; }
    }

    private readonly string _caption;
    /// <summary>
    /// 标签
    /// </summary>
    public string Caption
    {
      get { return _caption; }
    }

    private readonly bool _locking;
    /// <summary>
    /// 锁住?
    /// </summary>
    public bool Locking
    {
      get { return _locking; }
    }

    private readonly DateTime _time;
    /// <summary>
    /// 时间
    /// </summary>
    public DateTime Time
    {
      get { return _time; }
    }

    private readonly DateTime _expiryTime;
    /// <summary>
    /// 限期
    /// </summary>
    public DateTime ExpiryTime
    {
      get { return _expiryTime; }
    }

    private readonly string _userNumber;
    /// <summary>
    /// 登录工号
    /// </summary>
    public string UserNumber
    {
      get { return _userNumber; }
    }

    private readonly string _remark;
    /// <summary>
    /// 备注
    /// </summary>
    public string Remark
    {
      get { return _remark; }
    }

    #endregion
    
    #region 方法

    /// <summary>
    /// 是否允许操作
    /// </summary>
    /// <param name="identity">用户身份</param>
    public bool AllowSet(IIdentity identity)
    {
      if (identity == null)
        return AppConfig.AutoMode;
      if (!Locking || ExpiryTime < DateTime.Now)
        return true;
      return identity.AllowSet(this);
    }

    #endregion
  }
}
