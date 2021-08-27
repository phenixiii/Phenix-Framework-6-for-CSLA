namespace Phenix.Business.Rules
{
  /// <summary>
  /// 被授权对象接口
  /// </summary>
  public interface IAuthorizationObject
  {
    #region 方法

    /// <summary>
    /// 添加授权规则
    /// </summary>
    void AddAuthorizationRules();
    
    /// <summary>
    /// 允许过程可执行
    /// </summary>
    /// <param name="methodName">过程名</param>
    /// <param name="arguments">参数</param>
    bool AllowExecuteMethod(string methodName, params object[] arguments);

    #endregion
  }
}
