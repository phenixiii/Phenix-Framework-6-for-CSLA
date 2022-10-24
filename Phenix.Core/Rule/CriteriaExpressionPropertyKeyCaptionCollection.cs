using System;
using Phenix.Core.Mapping;
using Phenix.Core.Operate;

namespace Phenix.Core.Rule
{
  /// <summary>
  /// 条件表达式属性"键-标签"数组
  /// 主要用于填充下拉列表框内容
  /// </summary>
  [Serializable]
  public sealed class CriteriaExpressionPropertyKeyCaptionCollection : KeyCaptionCollection<CriteriaExpressionPropertyKeyCaption, FieldMapInfo>
  {
    #region 工厂

    internal static CriteriaExpressionPropertyKeyCaptionCollection Fetch(Type objectType)
    {
      CriteriaExpressionPropertyKeyCaptionCollection result = new CriteriaExpressionPropertyKeyCaptionCollection();
      foreach (FieldMapInfo item in ClassMemberHelper.DoGetFieldMapInfos(objectType))
        if (!item.FieldAttribute.NoMapping && item.Property != null)
          result.Add(new CriteriaExpressionPropertyKeyCaption(item));
      return result;
    }

    internal static CriteriaExpressionPropertyKeyCaptionCollection Fetch(Type objectType, CriteriaExpression criteriaExpression)
    {
      CriteriaExpressionPropertyKeyCaptionCollection result = Fetch(objectType);
      bool oldRaiseListChangedEvents = result.RaiseListChangedEvents;
      try
      {
        result.RaiseListChangedEvents = false;
        foreach (CriteriaExpressionPropertyKeyCaption item in result)
          item.Selected = criteriaExpression.Find(item.Value) != null;
      }
      finally
      {
        result.RaiseListChangedEvents = oldRaiseListChangedEvents;
      }
      return result;
    }

    #endregion

    #region 属性

    /// <summary>
    /// 标签
    /// </summary>
    public override string Caption
    {
      get { return Phenix.Core.Properties.Resources.PropertyFriendlyName; }
    }

    #endregion

    #region 方法

    /// <summary>
    /// 克隆
    /// </summary>
    public new CriteriaExpressionPropertyKeyCaptionCollection Clone()
    {
      return (CriteriaExpressionPropertyKeyCaptionCollection)base.Clone();
    }

    /// <summary>
    /// 构建被选择的条件表达式组
    /// Node_N = DefaultValue And ... Node_1 = DefaultValue And True
    /// </summary>
    public CriteriaExpression BuildSelectedCriteriaExpressionGroup()
    {
      CriteriaExpression result = CriteriaExpression.True;
      foreach (CriteriaExpressionPropertyKeyCaption item in SelectedItems)
        result = new CriteriaExpression(
          new CriteriaExpression(new CriteriaExpressionNode(item.Value), CriteriaOperate.Equal, item.Value.DefaultValue),
          CriteriaLogical.And,
          result);
      return result.CriteriaExpressionType != CriteriaExpressionType.Short ? result : null;
    }

    /// <summary>
    /// 添加被选择的条件表达式
    /// Node_N = DefaultValue And ... Node_1 = DefaultValue And True
    /// </summary>
    /// <param name="criteriaExpressionGroup">被添加的条件表达式组</param>
    public CriteriaExpression AddSelectedCriteriaExpression(CriteriaExpression criteriaExpressionGroup)
    {
      CriteriaExpression p = criteriaExpressionGroup;
      while ((object)p != null && p.CriteriaExpressionType == CriteriaExpressionType.CriteriaLogical && p.Logical == CriteriaLogical.And)
      {
        if (p.Right.CriteriaExpressionType == CriteriaExpressionType.Short)
        {
          CriteriaExpression fork = p;
          foreach (CriteriaExpressionPropertyKeyCaption item in SelectedItems)
          {
            p.Right = new CriteriaExpression(
              new CriteriaExpression(new CriteriaExpressionNode(item.Value), CriteriaOperate.Equal, item.Value.DefaultValue),
              CriteriaLogical.And,
              p.Right);
            p = p.Right;
          }
          return SelectedItems.Count > 0 ? fork.Right : null;
        }
        p = p.Right;
      }
      return null;
    }

    /// <summary>
    /// 返回被选择的条件表达式属性"键-标签"数组
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate")]
    public CriteriaExpressionPropertyKeyCaptionCollection GetSelectedItems()
    {
      CriteriaExpressionPropertyKeyCaptionCollection result = new CriteriaExpressionPropertyKeyCaptionCollection();
      foreach (CriteriaExpressionPropertyKeyCaption item in SelectedItems)
        result.Add(item);
      return result;
    }

    #endregion
  }
}
