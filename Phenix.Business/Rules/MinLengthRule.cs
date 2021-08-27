using System;

namespace Phenix.Business.Rules
{
  /// <summary>
  /// 最小长度规则
  /// </summary>
  public class MinLengthRule : Csla.Rules.CommonRules.MinLength
  {
    /// <summary>
    /// 初始化
    /// </summary>
    /// <param name="primaryProperty">主属性信息</param>
    /// <param name="min">最小值</param>
    public MinLengthRule(Phenix.Core.Mapping.IPropertyInfo primaryProperty, int min)
      : base((Csla.Core.IPropertyInfo)primaryProperty, min) { }

    /// <summary>
    /// 初始化
    /// </summary>
    /// <param name="primaryProperty">主属性信息</param>
    /// <param name="min">最小值</param>
    /// <param name="message">提示信息</param>
    public MinLengthRule(Phenix.Core.Mapping.IPropertyInfo primaryProperty, int min, string message)
      : base((Csla.Core.IPropertyInfo)primaryProperty, min, message) { }

    /// <summary>
    /// 初始化
    /// </summary>
    /// <param name="primaryProperty">主属性信息</param>
    /// <param name="min">最小值</param>
    /// <param name="messageDelegate">提示信息</param>
    public MinLengthRule(Phenix.Core.Mapping.IPropertyInfo primaryProperty, int min, Func<string> messageDelegate)
      : base((Csla.Core.IPropertyInfo)primaryProperty, min, messageDelegate) { }

    #region 属性

    /// <summary>
    /// 执行业务规则的属性
    /// </summary>
    public new Phenix.Core.Mapping.IPropertyInfo PrimaryProperty
    {
      get { return (Phenix.Core.Mapping.IPropertyInfo)base.PrimaryProperty; }
    }

    private bool? _isUnicode;
    /// <summary>
    /// 是否Unicode
    /// </summary>
    public bool IsUnicode
    {
      get
      {
        if (!_isUnicode.HasValue)
          _isUnicode = PrimaryProperty.FieldMapInfo == null || PrimaryProperty.FieldMapInfo.IsUnicode;
        return _isUnicode.Value;
      }
    }

    #endregion

    #region 方法

    /// <summary>
    /// 获取提示信息
    /// </summary>
    /// <value></value>
    protected override string GetMessage()
    {
      return HasMessageDelegate ? MessageText : Phenix.Core.Properties.Resources.MinLengthRule;
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
      if (value == null)
        return;

      string s = value.ToString();
      if (Phenix.Core.Reflection.Utilities.Length(s, IsUnicode) < Min)
      {
        string message = String.Format(GetMessage(), PrimaryProperty.FriendlyName, s, Min);
        context.Results.Add(new Csla.Rules.RuleResult(RuleName, base.PrimaryProperty, message) { Severity = Severity });
      }
    }

    #endregion
  }
}
