using System;
using Phenix.Core.Mapping;
using Phenix.Core.Operate;

namespace Phenix.Core.Rule
{
  /// <summary>
  /// 条件表达式属性"键-标签"
  /// 主要用于填充下拉列表框内容
  /// </summary>
  [Serializable]
  public sealed class CriteriaExpressionPropertyKeyCaption : KeyCaption<CriteriaExpressionPropertyKeyCaption, FieldMapInfo>, IKeyCaption
  {
    internal CriteriaExpressionPropertyKeyCaption(FieldMapInfo value)
      : base(value.PropertyName, value.FriendlyName, value) { }

    #region 属性

    /// <summary>
    /// 键
    /// </summary>
    public new string Key
    {
      get { return base.Key as string; }
    }
    object IKeyCaption.Key
    {
      get { return Key; }
    }

    #endregion
  }
}