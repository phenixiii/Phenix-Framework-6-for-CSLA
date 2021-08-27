using System;

namespace Phenix.Business.Rules
{
  /// <summary>
  /// 必输规则
  /// </summary>
  public class RequiredRule : Csla.Rules.CommonRules.Required
  {
    /// <summary>
    /// 初始化
    /// </summary>
    /// <param name="primaryProperty">主属性信息</param>
    public RequiredRule(Phenix.Core.Mapping.IPropertyInfo primaryProperty)
      : base((Csla.Core.IPropertyInfo)primaryProperty) { }


    /// <summary>
    /// 初始化
    /// </summary>
    /// <param name="primaryProperty">应用规则的属性</param>
    /// <param name="message">提示信息</param>
    public RequiredRule(Phenix.Core.Mapping.IPropertyInfo primaryProperty, string message)
      : base((Csla.Core.IPropertyInfo)primaryProperty, message) { }

    /// <summary>
    /// 初始化
    /// </summary>
    /// <param name="primaryProperty">应用规则的属性</param>
    /// <param name="messageDelegate">提示信息</param>
    public RequiredRule(Phenix.Core.Mapping.IPropertyInfo primaryProperty, Func<string> messageDelegate)
      : base((Csla.Core.IPropertyInfo)primaryProperty, messageDelegate) { }


    #region 属性

    /// <summary>
    /// 执行业务规则的属性
    /// </summary>
    public new Phenix.Core.Mapping.IPropertyInfo PrimaryProperty
    {
      get { return (Phenix.Core.Mapping.IPropertyInfo)base.PrimaryProperty; }
    }

    #endregion

    #region 方法

    /// <summary>
    /// 获取提示信息
    /// </summary>
    /// <value></value>
    protected override string GetMessage()
    {
      return HasMessageDelegate ? MessageText : Phenix.Core.Properties.Resources.RequiredRule;
    }

    ///<summary>
    /// 执行
    ///</summary>
    protected override void Execute(Csla.Rules.RuleContext context)
    {
      IBusinessObject business = context.Target as IBusinessObject;
      if (business != null && business.IsSelfDeleted)
        return;

      object value = context.InputPropertyValues[base.PrimaryProperty];
      if (value == null || String.IsNullOrEmpty(value.ToString()))
      {
        string message = String.Format(GetMessage(), PrimaryProperty.FriendlyName);
        context.Results.Add(new Csla.Rules.RuleResult(RuleName, base.PrimaryProperty, message) { Severity = Severity });
      }
    }

    #endregion
  }
}
