using System;
using System.Threading;
using System.Web;
using Phenix.Core.Security;

namespace Phenix.Business.Security
{
  /// <summary>
  /// 用户对象
  /// </summary>
  [Serializable]
  public sealed class UserPrincipal : Csla.Security.CslaPrincipal, IPrincipal
  {
    /// <summary>
    /// 初始化
    /// </summary>
    /// <param name="identity">用户身份</param>
    public UserPrincipal(UserIdentity identity)
      : base(identity)
    {
      _identity = identity;
    }

    #region 工厂

    /// <summary>
    /// 构建测试用户
    /// </summary>
    public static UserPrincipal CreateTester()
    {
      return new UserPrincipal(UserIdentity.CreateTester());
    }

    #endregion

    #region 属性

    private readonly UserIdentity _identity;
    /// <summary>
    /// 特性
    /// </summary>
    public new UserIdentity Identity
    {
      get { return _identity; }
    }
    /// <summary>
    /// 特性
    /// </summary>
    IIdentity IPrincipal.Identity
    {
      get { return Identity; }
    }
    /// <summary>
    /// 特性
    /// </summary>
    System.Security.Principal.IIdentity System.Security.Principal.IPrincipal.Identity
    {
      get { return Identity; }
    }

    /// <summary>
    /// 用户
    /// </summary>
    public static UserPrincipal User
    {
      get
      {
        if (HttpContext.Current != null)
        {
          UserPrincipal result = HttpContext.Current.User as UserPrincipal;
          if (result != null)
            return result;
        }
        if (Thread.CurrentPrincipal != null)
        {
          UserPrincipal result = Thread.CurrentPrincipal as UserPrincipal;
          if (result != null)
            return result;
        }
        UserIdentity identity = UserIdentity.CurrentIdentity;
        if (identity != null)
          return new UserPrincipal(identity);
        return null;
      }
      set
      {
        if (HttpContext.Current != null)
          HttpContext.Current.User = value;
        Thread.CurrentPrincipal = value;
        UserIdentity.CurrentIdentity = value._identity;
      }
    }

    #endregion

    #region 方法

    #region IPrincipal 成员

    /// <summary>
    /// 确定是否属于指定的角色
    /// </summary>
    /// <param name="role">角色</param>
    public override bool IsInRole(string role)
    {
      return _identity.IsInRole(role);
    }

    #endregion

    #endregion
  }
}