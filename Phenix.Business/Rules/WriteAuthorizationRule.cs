namespace Phenix.Business.Rules
{
  /// <summary>
  /// 写属性授权规则基类
  /// </summary>
  public abstract class WriteAuthorizationRule : PropertyAuthorizationRule
  {
    /// <summary>
    /// 初始化
    /// </summary>
    /// <param name="propertyInfo">属性信息</param>
    protected WriteAuthorizationRule(Phenix.Core.Mapping.IPropertyInfo propertyInfo)
      : base(Csla.Rules.AuthorizationActions.WriteProperty, propertyInfo) { }
  }
}
