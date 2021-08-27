using System;

namespace Phenix.Business.Rules
{
  /// <summary>
  /// 最大长度规则
  /// </summary>
  public class MaxLengthRule : Csla.Rules.CommonRules.MaxLength
  {
    /// <summary>
    /// 初始化
    /// </summary>
    /// <param name="primaryProperty">主属性信息</param>
    /// <param name="max">最大值</param>
    public MaxLengthRule(Phenix.Core.Mapping.IPropertyInfo primaryProperty, int max)
      : base((Csla.Core.IPropertyInfo)primaryProperty, max) { }

    /// <summary>
    /// 初始化
    /// </summary>
    /// <param name="primaryProperty">主属性信息</param>
    /// <param name="max">最大值</param>
    /// <param name="message">提示信息</param>
    public MaxLengthRule(Phenix.Core.Mapping.IPropertyInfo primaryProperty, int max, string message)
      : base((Csla.Core.IPropertyInfo)primaryProperty, max, message) { }

    /// <summary>
    /// 初始化
    /// </summary>
    /// <param name="primaryProperty">主属性信息</param>
    /// <param name="max">最大值</param>
    /// <param name="messageDelegate">提示信息</param>
    public MaxLengthRule(Phenix.Core.Mapping.IPropertyInfo primaryProperty, int max, Func<string> messageDelegate)
      : base((Csla.Core.IPropertyInfo)primaryProperty, max, messageDelegate) { }

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
      return HasMessageDelegate ? MessageText : Phenix.Core.Properties.Resources.MaxLengthRule;
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
      if (Phenix.Core.Reflection.Utilities.Length(s, IsUnicode) > Max)
      {
        string message = String.Format(GetMessage(), PrimaryProperty.FriendlyName, s, Max);
        context.Results.Add(new Csla.Rules.RuleResult(RuleName, base.PrimaryProperty, message) { Severity = Severity });
      }
    }

    #endregion
  }
}
