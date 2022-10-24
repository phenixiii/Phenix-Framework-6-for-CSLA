namespace Phenix.Core.Security
{
  /// <summary>
  /// 安全数据接口
  /// </summary>
  public interface ISecurityInfo
  {
    #region 属性
    
    /// <summary>
    /// 登录工号
    /// </summary>
    string UserNumber { get; }

    #endregion
  }
}
