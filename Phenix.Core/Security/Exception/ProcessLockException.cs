using System;
using System.Runtime.Serialization;
using System.Security.Authentication;

namespace Phenix.Core.Security
{
  /// <summary>
  /// 过程锁异常
  /// </summary>
  [Serializable]
  public class ProcessLockException : AuthenticationException
  {
    /// <summary>
    /// 过程锁异常
    /// </summary>
    public ProcessLockException()
      : base() { }

    /// <summary>
    /// 过程锁异常
    /// </summary>
    public ProcessLockException(string message)
     : base(message) { }

    /// <summary>
    /// 过程锁异常
    /// </summary>
    public ProcessLockException(string message, System.Exception innerException)
      : base(message, innerException) { }

    /// <summary>
    /// 过程锁异常
    /// </summary>
    public ProcessLockException(ProcessLockInfo info)
      : base(info != null
        ? String.Format(Phenix.Core.Properties.Resources.ProcessLockException, info.Caption, info.UserNumber, info.Time, info.ExpiryTime, info.Remark)
        : null)
    {
      _info = info;
    }

    #region Serialization

    /// <summary>
    /// 序列化
    /// </summary>
    protected ProcessLockException(SerializationInfo info, StreamingContext context)
      : base(info, context)
    {
      if (info == null)
        throw new ArgumentNullException("info");
      _info = (ProcessLockInfo)info.GetValue("_info", typeof(ProcessLockInfo));
    }

    /// <summary>
    /// 反序列化
    /// </summary>
    [System.Security.SecurityCritical]
    public override void GetObjectData(SerializationInfo info, StreamingContext context)
    {
      if (info == null)
        throw new ArgumentNullException("info");
      base.GetObjectData(info, context);
      info.AddValue("_info", _info);
    }

    #endregion

    #region 属性

    private readonly ProcessLockInfo _info;
    /// <summary>
    /// 过程锁资料
    /// </summary>
    public ProcessLockInfo Info
    {
      get { return _info; }
    }

    #endregion
  }
}