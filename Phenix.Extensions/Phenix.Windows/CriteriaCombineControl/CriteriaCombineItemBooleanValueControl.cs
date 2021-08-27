using System.ComponentModel;
using Phenix.Core.Mapping;

namespace Phenix.Windows
{
  [ToolboxItem(false)]
  internal partial class CriteriaCombineItemBooleanValueControl : CriteriaCombineItemValueBaseControl
  {
    public CriteriaCombineItemBooleanValueControl(CriteriaExpression criteriaExpression)
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
          return true;
        default:
          return false;
      }
    }

    #endregion
  }
}