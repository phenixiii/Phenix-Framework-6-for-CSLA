namespace Phenix.Core.Security
{
  /// <summary>
  /// 用户接口
  /// </summary>
  public interface IPrincipal : System.Security.Principal.IPrincipal
  {
    #region 属性

    /// <summary>
    /// 特性
    /// </summary>
    new IIdentity Identity { get; }

    #endregion
  }
}
