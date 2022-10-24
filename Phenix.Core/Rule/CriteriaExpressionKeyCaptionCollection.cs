using System;
using Phenix.Core.Mapping;
using Phenix.Core.Operate;
using Phenix.Core.Security;

namespace Phenix.Core.Rule
{
  /// <summary>
  /// 条件表达式"键-标签"数组
  /// 主要用于填充下拉列表框内容
  /// </summary>
  [Serializable]
  public sealed class CriteriaExpressionKeyCaptionCollection : KeyCaptionCollection<CriteriaExpressionKeyCaption, CriteriaExpression>
  {
    /// <summary>
    /// 初始化
    /// </summary>
    [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
    public CriteriaExpressionKeyCaptionCollection(string name, DateTime? actionTime)
      : base()
    {
      _name = name;
      _actionTime = actionTime;
    }

    #region 工厂

    /// <summary>
    /// 构建
    /// </summary>
    /// <param name="ownerType">所属类</param>
    /// <param name="name">名称</param>
    public static CriteriaExpressionKeyCaptionCollection Fetch(Type ownerType, string name)
    {
      return DataRuleHub.GetCriteriaExpressions(ownerType, name, UserIdentity.CurrentIdentity);
    }

    /// <summary>
    /// 构建
    /// </summary>
    /// <param name="name">名称</param>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
    public static CriteriaExpressionKeyCaptionCollection Fetch<T>(string name)
    {
      return DataRuleHub.GetCriteriaExpressions(typeof(T), name, UserIdentity.CurrentIdentity);
    }

    /// <summary>
    /// 构建
    /// </summary>
    [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
    public static CriteriaExpressionKeyCaptionCollection Fetch(Type ownerType, string name, UserIdentity identity)
    {
      return DataRuleHub.GetCriteriaExpressions(ownerType, name, identity);
    }

    #endregion

    #region 属性

    /// <summary>
    /// 所属类
    /// </summary>
    [System.ComponentModel.Browsable(false)]
    [System.ComponentModel.DataAnnotations.Display(AutoGenerateField = false)]
    public Type OwnerType { get; internal set; }

    private readonly string _name;
    /// <summary>
    /// 名称
    /// </summary>
    public string Name
    {
      get { return _name; }
    }

    private readonly DateTime? _actionTime;
    /// <summary>
    /// 活动时间
    /// </summary>
    public DateTime? ActionTime
    {
      get { return _actionTime; }
    }

    /// <summary>
    /// 标签
    /// </summary>
    public override string Caption
    {
      get { return Phenix.Core.Properties.Resources.CriteriaExpressionFriendlyName; }
    }

    #endregion

    #region 方法

    /// <summary>
    /// 克隆
    /// </summary>
    public new CriteriaExpressionKeyCaptionCollection Clone()
    {
      return (CriteriaExpressionKeyCaptionCollection)base.Clone();
    }

    /// <summary>
    /// 构建条件表达式属性"键-标签"数组
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate")]
    public CriteriaExpressionPropertyKeyCaptionCollection FetchCriteriaExpressionProperties()
    {
      return CriteriaExpressionPropertyKeyCaptionCollection.Fetch(OwnerType);
    }

    /// <summary>
    /// 构建条件表达式属性"键-标签"数组
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate")]
    public CriteriaExpressionPropertyKeyCaptionCollection FetchCriteriaExpressionProperties(CriteriaExpression criteriaExpression)
    {
      return CriteriaExpressionPropertyKeyCaptionCollection.Fetch(OwnerType, criteriaExpression);
    }

    /// <summary>
    /// 保存条件表达式"键-标签"
    /// identity = Phenix.Core.Security.UserIdentity.CurrentIdentity
    /// </summary>
    /// <param name="criteriaExpression">条件表达式</param>
    public void Save(CriteriaExpressionKeyCaption criteriaExpression)
    {
      Save(criteriaExpression, UserIdentity.CurrentIdentity);
    }

    /// <summary>
    /// 保存条件表达式"键-标签"
    /// </summary>
    [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
    public void Save(CriteriaExpressionKeyCaption criteriaExpression, UserIdentity identity)
    {
      DataRuleHub.AddCriteriaExpression(Name, criteriaExpression, identity);
      CriteriaExpressionKeyCaption item = FindByKey(criteriaExpression.Key);
      if (item != null)
      {
        item.Value = criteriaExpression.Value;
        item.ReadLevel = criteriaExpression.ReadLevel;
      }
      else
        base.Add(criteriaExpression);
    }

    /// <summary>
    /// 移除条件表达式"键-标签"
    /// 根据 Key 匹配
    /// </summary>
    /// <param name="criteriaExpression">条件表达式</param>
    public new bool Remove(CriteriaExpressionKeyCaption criteriaExpression)
    {
      if (criteriaExpression == null)
        throw new ArgumentNullException("criteriaExpression");
      DataRuleHub.RemoveCriteriaExpression(Name, criteriaExpression.Key);
      return base.Remove(FindByKey(criteriaExpression.Key));
    }
    
    /// <summary>
    /// 比较类型
    /// 主要用于IDE环境
    /// </summary>
    public static bool Equals(Type objectType)
    {
      if (objectType == null)
        return false;
      return String.CompareOrdinal(objectType.FullName, typeof(CriteriaExpressionKeyCaptionCollection).FullName) == 0;
    }

    #endregion
  }
}
