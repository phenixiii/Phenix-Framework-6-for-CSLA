using System;

namespace Phenix.Web.Client.Security
{
  /// <summary>
  /// 用户身份
  /// </summary>
  [Serializable]
  public sealed class UserIdentity : Phenix.Core.Security.UserIdentity
  {
    /// <summary>
    /// 初始化
    /// </summary>
    public UserIdentity(string userNumber, string password)
      : base(userNumber, password) { }

    #region 工厂

    /// <summary>
    /// 构建匿名用户
    /// </summary>
    public new static UserIdentity CreateGuest()
    {
      return new UserIdentity(GuestUserNumber, GuestUserNumber);
    }

    #endregion

    #region 属性

    /// <summary>
    /// 已验证?
    /// </summary>
    public new bool IsAuthenticated
    {
      get { return base.IsAuthenticated; }
      internal set { base.IsAuthenticated = value; }
    }

    /// <summary>
    /// 验证消息
    /// </summary>
    public new string AuthenticatedMessage
    {
      get { return base.AuthenticatedMessage; }
      internal set { base.AuthenticatedMessage = value; }
    }

    #endregion
  }
}