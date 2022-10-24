using System;
using System.Runtime.Serialization;
using System.Security.Authentication;

namespace Phenix.Core.Security
{
  /// <summary>
  /// 用户口令复杂度验证异常
  /// </summary>
  [Serializable]
  public class UserPasswordComplexityException : AuthenticationException
  {
    /// <summary>
    /// 用户口令复杂度验证异常
    /// </summary>
    public UserPasswordComplexityException()
      : base() { }

    /// <summary>
    /// 用户口令复杂度验证异常
    /// </summary>
    public UserPasswordComplexityException(string message)
      : base(message) { }

    /// <summary>
    /// 用户口令复杂度验证异常
    /// </summary>
    public UserPasswordComplexityException(string message, System.Exception innerException)
      : base(message, innerException) { }

    /// <summary>
    /// 用户口令复杂度验证异常
    /// </summary>
    public UserPasswordComplexityException(int lengthMinimize, int complexityMinimize)
      : base(String.Format(Phenix.Core.Properties.Resources.PasswordComplexityReminder, lengthMinimize, complexityMinimize)) { }

    /// <summary>
    /// 用户口令复杂度验证异常
    /// </summary>
    public UserPasswordComplexityException(UserIdentity identity, int lengthMinimize, int complexityMinimize)
      : this(lengthMinimize, complexityMinimize)
    {
      _identity = identity;
    }

    #region Serialization

    /// <summary>
    /// 序列化
    /// </summary>
    protected UserPasswordComplexityException(SerializationInfo info, StreamingContext context)
      : base(info, context)
    {
      if (info == null)
        throw new ArgumentNullException("info");
      _identity = (UserIdentity)info.GetValue("_identity", typeof(UserIdentity));
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
      info.AddValue("_identity", _identity);
    }

    #endregion

    #region 属性

    private readonly UserIdentity _identity;
    /// <summary>
    /// 用户身份
    /// </summary>
    public UserIdentity Identity
    {
      get { return _identity; }
    }

    #endregion
  }
}