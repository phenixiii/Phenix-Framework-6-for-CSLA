using System;
using System.Runtime.Serialization;
using System.Security.Authentication;

namespace Phenix.Core.Security.Exception
{
  /// <summary>
  /// 用户找不到异常
  /// </summary>
  [Serializable]
  public class UserNotFoundException : AuthenticationException
  {
    /// <summary>
    /// 用户找不到异常
    /// </summary>
    public UserNotFoundException()
      : base() { }

    /// <summary>
    /// 用户找不到异常
    /// </summary>
    public UserNotFoundException(string userNumber)
      : base(String.Format(Phenix.Core.Properties.Resources.UserNotFoundException, userNumber)) { }

    /// <summary>
    /// 用户找不到异常
    /// </summary>
    public UserNotFoundException(string userNumber, System.Exception innerException)
      : base(String.Format(Phenix.Core.Properties.Resources.UserNotFoundException, userNumber), innerException) { }

    #region Serialization

    /// <summary>
    /// 序列化
    /// </summary>
    protected UserNotFoundException(SerializationInfo serializationInfo, StreamingContext streamingContext) 
      : base(serializationInfo, streamingContext) { }

    #endregion
  }
}
