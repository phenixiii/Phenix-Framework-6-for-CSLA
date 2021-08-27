using System;
using System.ComponentModel;
using Phenix.Core.Mapping;

namespace Phenix.Windows
{
  [ToolboxItem(false)]
  internal partial class CriteriaCombineItemValueControl : CriteriaCombineItemValueBaseControl
  {
    public CriteriaCombineItemValueControl(CriteriaExpression criteriaExpression)
      : base(criteriaExpression)
    {
      InitializeComponent();
    }
    
    #region 方法

    protected override bool IsValid(CriteriaOperate value)
    {
      if (CriteriaExpression == null)
        return false;
      switch (value)
      {
        case CriteriaOperate.Equal:
        case CriteriaOperate.Greater:
        case CriteriaOperate.GreaterOrEqual:
        case CriteriaOperate.Lesser:
        case CriteriaOperate.LesserOrEqual:
        case CriteriaOperate.Unequal:
          return true;
        case CriteriaOperate.Like:
        case CriteriaOperate.Unlike:
          return String.CompareOrdinal(CriteriaExpression.LeftNode.PropertyTypeName, typeof(string).FullName) == 0;
        case CriteriaOperate.IsNull:
        case CriteriaOperate.IsNotNull:
        case CriteriaOperate.In:
        case CriteriaOperate.NotIn:
          return true;
        default:
          return false;
      }
    }

    protected override void ApplyRules()
    {
      this.valueTextEdit.Visible = Operate != CriteriaOperate.IsNull && Operate != CriteriaOperate.IsNotNull;
    }

    #endregion
  }
}
