using System.ComponentModel;
using Phenix.Core.Mapping;

namespace Phenix.Windows
{
  [ToolboxItem(false)]
  internal partial class CriteriaCombineItemDateTimeValueControl : CriteriaCombineItemValueBaseControl
  {
    public CriteriaCombineItemDateTimeValueControl(CriteriaExpression criteriaExpression)
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
        case CriteriaOperate.IsNull:
        case CriteriaOperate.IsNotNull:
          return true;
        default:
          return false;
      }
    }

    protected override void ApplyRules()
    {
      this.valueDateEdit.Visible = Operate != CriteriaOperate.IsNull && Operate != CriteriaOperate.IsNotNull;
      this.valueTimeEdit.Visible = Operate != CriteriaOperate.IsNull && Operate != CriteriaOperate.IsNotNull;
    }

    #endregion
  }
}
