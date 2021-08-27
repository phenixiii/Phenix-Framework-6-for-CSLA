namespace Phenix.Business.Rules
{
  /// <summary>
  /// 属性有效性规则基类 
  /// </summary>
  public abstract class EditValidationRule : Csla.Rules.PropertyRule
  {
    /// <summary>
    /// 初始化
    /// </summary>
    /// <param name="propertyInfo">属性信息</param>
    protected EditValidationRule(Phenix.Core.Mapping.IPropertyInfo propertyInfo)
      : base((Csla.Core.IPropertyInfo)propertyInfo) { }
  }
}
