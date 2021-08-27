using Phenix.Core.Security;

namespace Phenix.Services.Library
{
  /// <summary>
  /// 授权者接口
  /// </summary>
  public interface IAuthoriser
  {
    #region 方法

    /// <summary>
    /// 转译用户
    /// </summary>
    UserIdentity Translation(string userNumber);

    /// <summary>
    /// 登录验证
    /// </summary>
    bool? LogOn(string userNumber, string password);

    /// <summary>
    /// 登录核实
    /// </summary>
    bool? LogOnVerify(string userNumber, string tag);

    /// <summary>
    /// 修改登录口令
    /// </summary>
    bool ChangePassword(string userNumber, string newPassword);

    #endregion
  }
}
