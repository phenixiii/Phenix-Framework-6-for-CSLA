using System;
using System.ComponentModel;
using Phenix.Core.Mapping;
using Phenix.Core.Rule;

namespace Phenix.Windows
{
  [ToolboxItem(false)]
  internal partial class CriteriaCombineItemEnumValueControl : CriteriaCombineItemValueBaseControl
  {
    public CriteriaCombineItemEnumValueControl(CriteriaExpression criteriaExpression, Type enumType)
      : base(criteriaExpression)
    {
      InitializeComponent();

      this.enumKeyCaptionBindingSource.DataSource = EnumKeyCaptionCollection.Fetch(enumType);
    }
    
    #region 方法

    protected override bool IsValid(CriteriaOperate value)
    {
      if (CriteriaExpression == null)
        return false;
      switch (value)
      {
        case CriteriaOperate.Equal:
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
      this.valueCheckedComboBoxEdit.Visible = Operate != CriteriaOperate.IsNull && Operate != CriteriaOperate.IsNotNull;
    }

    #endregion
  }
}