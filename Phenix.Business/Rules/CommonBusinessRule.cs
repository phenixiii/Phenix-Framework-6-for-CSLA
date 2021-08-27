namespace Phenix.Business.Rules
{
  /// <summary>
  /// 公共业务规则基类
  /// </summary>
  public abstract class CommonBusinessRule : Csla.Rules.CommonRules.CommonBusinessRule
  {
    /// <summary>
    /// 初始化
    /// </summary>
    protected CommonBusinessRule()
      : base() { }

    /// <summary>
    /// 初始化
    /// </summary>
    /// <param name="primaryPropertyInfo">主属性信息</param>
    protected CommonBusinessRule(Phenix.Core.Mapping.IPropertyInfo primaryPropertyInfo)
      : base((Csla.Core.IPropertyInfo)primaryPropertyInfo) { }
  }
}
