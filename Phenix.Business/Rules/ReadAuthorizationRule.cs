namespace Phenix.Business.Rules
{
  /// <summary>
  /// 读属性授权规则基类
  /// </summary>
  public abstract class ReadAuthorizationRule : PropertyAuthorizationRule
  {
    /// <summary>
    /// 初始化
    /// </summary>
    /// <param name="propertyInfo">属性信息</param>
    protected ReadAuthorizationRule(Phenix.Core.Mapping.IPropertyInfo propertyInfo)
      : base(Csla.Rules.AuthorizationActions.ReadProperty, propertyInfo) { }
  }
}
