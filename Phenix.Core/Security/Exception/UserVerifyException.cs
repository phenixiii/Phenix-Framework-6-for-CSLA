using System;
using System.Runtime.Serialization;
using System.Security.Authentication;

namespace Phenix.Core.Security
{
  /// <summary>
  /// 用户验证异常
  /// </summary>
  [Serializable]
  public class UserVerifyException : AuthenticationException
  {
    /// <summary>
    /// 用户验证异常
    /// </summary>
    public UserVerifyException()
      : base(String.Format(Phenix.Core.Properties.Resources.UserVerifyException, String.Empty)) { }
    
    /// <summary>
    /// 用户验证异常
    /// </summary>
    public UserVerifyException(string message)
      : base(String.Format(Phenix.Core.Properties.Resources.UserVerifyException, message)) { }

    /// <summary>
    /// 用户验证异常
    /// </summary>
    public UserVerifyException(System.Exception innerException)
      : base(String.Format(Phenix.Core.Properties.Resources.UserVerifyException, innerException != null ? innerException.Message : String.Empty), innerException) { }

    /// <summary>
    /// 用户验证异常
    /// </summary>
    public UserVerifyException(string message, System.Exception innerException)
      : base(String.Format(Phenix.Core.Properties.Resources.UserVerifyException, message ?? (innerException != null ? innerException.Message : String.Empty)), innerException) { }

    #region Serialization

    /// <summary>
    /// 序列化
    /// </summary>
    protected UserVerifyException(SerializationInfo serializationInfo, StreamingContext streamingContext) 
      : base(serializationInfo, streamingContext) { }

    #endregion
  }
}