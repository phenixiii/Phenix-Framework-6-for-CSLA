using System.ComponentModel;
using System.Windows.Forms;
using Phenix.Core.Mapping;
using Phenix.Core.Rule;
using Phenix.Core.Windows;

namespace Phenix.Windows
{
  [ToolboxItem(false)]
  internal partial class CriteriaCombineItemValueBaseControl : UserControl
  {
    private CriteriaCombineItemValueBaseControl()
      : base() 
    {
      InitializeComponent();
    }

    public CriteriaCombineItemValueBaseControl(CriteriaExpression criteriaExpression)
      : this()
    {
      this.criteriaExpressionBindingSource.DataSource = criteriaExpression;
      this.criteriaOperateEnumKeyCaptionBindingSource.DataSource = EnumKeyCaptionCollection.Fetch<CriteriaOperate>(IsValid);
    }

    #region 属性

    protected CriteriaExpression CriteriaExpression
    {
      get { return BindingSourceHelper.GetDataSourceCurrent(this.criteriaExpressionBindingSource) as CriteriaExpression; }
    }

    protected CriteriaOperate Operate
    {
      get { return (CriteriaOperate)this.criteriaOperateLookUpEdit.EditValue; }
    }
   
    #endregion
    
    #region 方法

    protected virtual bool IsValid(CriteriaOperate value)
    {
      return false;
    }

    protected virtual void ApplyRules()
    {
    }

    #endregion

    private void criteriaOperateLookUpEdit_EditValueChanged(object sender, System.EventArgs e)
    {
      ApplyRules();
    }
  }
}
