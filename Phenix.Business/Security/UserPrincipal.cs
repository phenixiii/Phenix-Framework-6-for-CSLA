using System;
using System.Threading;
using System.Web;
using Phenix.Core.Security;

namespace Phenix.Business.Security
{
  /// <summary>
  /// �û�����
  /// </summary>
  [Serializable]
  public sealed class UserPrincipal : Csla.Security.CslaPrincipal, IPrincipal
  {
    /// <summary>
    /// ��ʼ��
    /// </summary>
    /// <param name="identity">�û����</param>
    public UserPrincipal(UserIdentity identity)
      : base(identity)
    {
      _identity = identity;
    }

    #region ����

    /// <summary>
    /// ���������û�
    /// </summary>
    public static UserPrincipal CreateTester()
    {
      return new UserPrincipal(UserIdentity.CreateTester());
    }

    #endregion

    #region ����

    private readonly UserIdentity _identity;
    /// <summary>
    /// ����
    /// </summary>
    public new UserIdentity Identity
    {
      get { return _identity; }
    }
    /// <summary>
    /// ����
    /// </summary>
    IIdentity IPrincipal.Identity
    {
      get { return Identity; }
    }
    /// <summary>
    /// ����
    /// </summary>
    System.Security.Principal.IIdentity System.Security.Principal.IPrincipal.Identity
    {
      get { return Identity; }
    }

    /// <summary>
    /// �û�
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

    #region ����

    #region IPrincipal ��Ա

    /// <summary>
    /// ȷ���Ƿ�����ָ���Ľ�ɫ
    /// </summary>
    /// <param name="role">��ɫ</param>
    public override bool IsInRole(string role)
    {
      return _identity.IsInRole(role);
    }

    #endregion

    #endregion
  }
}