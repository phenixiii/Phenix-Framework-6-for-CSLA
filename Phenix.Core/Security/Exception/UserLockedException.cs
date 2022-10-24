using System;
using System.Runtime.Serialization;
using System.Security.Authentication;

namespace Phenix.Core.Security
{
  /// <summary>
  /// 账户锁定异常
  /// </summary>
  [Serializable]
  public class UserLockedException  : AuthenticationException
  {
    /// <summary>
    /// 账户锁定异常
    /// </summary>
    public UserLockedException()
      : base() { }

    /// <summary>
    /// 账户锁定异常
    /// </summary>
    public UserLockedException(string userNumber)
      : base(String.Format(Phenix.Core.Properties.Resources.UserLockedException, userNumber)) { }

    /// <summary>
    /// 账户锁定异常
    /// </summary>
    public UserLockedException(string userNumber, System.Exception innerException)
      : base(String.Format(Phenix.Core.Properties.Resources.UserLockedException, userNumber), innerException) { }

    /// <summary>
    /// 账户锁定异常
    /// </summary>
    public UserLockedException(string userNumber, string message)
      : base(String.Format("{0}: {1}", String.Format(Phenix.Core.Properties.Resources.UserLockedException, userNumber), message)) { }

    #region Serialization

    /// <summary>
    /// 序列化
    /// </summary>
    protected UserLockedException(SerializationInfo serializationInfo, StreamingContext streamingContext) 
      : base(serializationInfo, streamingContext) { }

    #endregion
  }
}