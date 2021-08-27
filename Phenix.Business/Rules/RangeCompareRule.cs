using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Phenix.Business.Rules
{
  ///<summary>
  /// 比较范围规则
  ///</summary>
  public class RangeCompareRule<T> : CommonBusinessRule
    where T : IComparable
  {
    /// <summary>
    /// 初始化
    /// </summary>
    ///<param name="primaryPropertyInfo">主属性信息</param>
    ///<param name="minValuePropertyInfo">最小值属性信息</param>
    ///<param name="maxValuePropertyInfo">最大值属性信息</param>
    public RangeCompareRule(Phenix.Core.Mapping.IPropertyInfo primaryPropertyInfo, Phenix.Core.Mapping.IPropertyInfo minValuePropertyInfo, Phenix.Core.Mapping.IPropertyInfo maxValuePropertyInfo)
      : base(primaryPropertyInfo)
    {
      if (!object.Equals(primaryPropertyInfo, minValuePropertyInfo))
        AffectedProperties.Add((Csla.Core.IPropertyInfo)minValuePropertyInfo);
      if (!object.Equals(primaryPropertyInfo, maxValuePropertyInfo))
        AffectedProperties.Add((Csla.Core.IPropertyInfo)maxValuePropertyInfo);

      InputProperties = new List<Csla.Core.IPropertyInfo> { (Csla.Core.IPropertyInfo)minValuePropertyInfo, (Csla.Core.IPropertyInfo)maxValuePropertyInfo };

      _minValuePropertyInfo = minValuePropertyInfo;
      _maxValuePropertyInfo = maxValuePropertyInfo;
    }

    #region Register

    /// <summary>
    /// 注册到业务规则库中
    /// </summary>
    ///<param name="businessRules">业务规则库</param>
    ///<param name="minValuePropertyInfo">最小值属性信息</param>
    ///<param name="maxValuePropertyInfo">最大值属性信息</param>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1000:DoNotDeclareStaticMembersOnGenericTypes")]
    public static void Register(Csla.Rules.BusinessRules businessRules, Phenix.Core.Mapping.IPropertyInfo minValuePropertyInfo, Phenix.Core.Mapping.IPropertyInfo maxValuePropertyInfo)
    {
      businessRules.AddRule(new RangeCompareRule<T>(minValuePropertyInfo, minValuePropertyInfo, maxValuePropertyInfo));
      businessRules.AddRule(new RangeCompareRule<T>(maxValuePropertyInfo, minValuePropertyInfo, maxValuePropertyInfo));
    }

    #endregion

    #region 属性

    private readonly Phenix.Core.Mapping.IPropertyInfo _minValuePropertyInfo;
    /// <summary>
    /// 最小值属性信息
    /// </summary>
    public Phenix.Core.Mapping.IPropertyInfo MinValuePropertyInfo
    {
      get { return _minValuePropertyInfo; }
    }

    private readonly Phenix.Core.Mapping.IPropertyInfo _maxValuePropertyInfo;
    /// <summary>
    /// 最大值属性信息
    /// </summary>
    public Phenix.Core.Mapping.IPropertyInfo MaxValuePropertyInfo
    {
      get { return _maxValuePropertyInfo; }
    }

    #endregion

    #region 方法
    
    ///<summary>
    /// 执行
    ///</summary>
    protected override void Execute(Csla.Rules.RuleContext context)
    {
      IBusinessObject business = context.Target as IBusinessObject;
      if (business != null && business.IsSelfDeleted)
        return;

      IDataErrorInfo errorInfo = context.Target as IDataErrorInfo;
      if (errorInfo != null)
        if (object.ReferenceEquals(PrimaryProperty, MinValuePropertyInfo))
        {
          if (!String.IsNullOrEmpty(errorInfo[MaxValuePropertyInfo.Name]))
            return;
        }
        else if (object.ReferenceEquals(PrimaryProperty, MaxValuePropertyInfo))
        {
          if (!String.IsNullOrEmpty(errorInfo[MinValuePropertyInfo.Name]))
            return;
        }

      object minValueObject = context.InputPropertyValues[(Csla.Core.IPropertyInfo)MinValuePropertyInfo];
      object maxValueObject = context.InputPropertyValues[(Csla.Core.IPropertyInfo)MaxValuePropertyInfo];
      if (object.Equals(minValueObject, null) || object.Equals(maxValueObject, null))
        return;
      T minValue = (T)Phenix.Core.Reflection.Utilities.ChangeType(minValueObject, typeof(T));
      T maxValue = (T)Phenix.Core.Reflection.Utilities.ChangeType(maxValueObject, typeof(T));
      if (object.Equals(minValue, null) || object.Equals(maxValue, null))
        return;
      int result = minValue.CompareTo(maxValue);
      if (result > 0)
      {
        string message = string.Format(Phenix.Business.Properties.Resources.RangeCompareRule,
          MinValuePropertyInfo.FriendlyName, minValue, MaxValuePropertyInfo.FriendlyName, maxValue);
        context.Results.Add(new Csla.Rules.RuleResult(RuleName, PrimaryProperty, message) { Severity = Severity });
      }
    }

    #endregion
  }
}