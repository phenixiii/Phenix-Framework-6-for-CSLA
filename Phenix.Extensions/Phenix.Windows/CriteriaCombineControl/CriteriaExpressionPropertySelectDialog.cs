using System.Windows.Forms;
using Phenix.Core.Rule;
using Phenix.Core.Windows;

namespace Phenix.Windows
{
  internal partial class CriteriaExpressionPropertySelectDialog : Phenix.Core.Windows.DialogForm
  {
    private CriteriaExpressionPropertySelectDialog()
    {
      InitializeComponent();
    }

    #region 工厂

    public static CriteriaExpressionPropertyKeyCaptionCollection Execute(CriteriaExpressionKeyCaptionCollection criteriaExpressionKeyCaptionCollection)
    {
      using (CriteriaExpressionPropertySelectDialog dialog = new CriteriaExpressionPropertySelectDialog())
      {
        dialog.CriteriaExpressionProperties = criteriaExpressionKeyCaptionCollection.FetchCriteriaExpressionProperties();
        return dialog.ShowDialog() == DialogResult.OK ? dialog.CriteriaExpressionProperties : null;
      }
    }

    #endregion

    #region 属性
    
    private CriteriaExpressionPropertyKeyCaptionCollection CriteriaExpressionProperties
    {
      get { return BindingSourceHelper.GetDataSourceList(this.criteriaExpressionPropertyKeyCaptionCollectionBindingSource) as CriteriaExpressionPropertyKeyCaptionCollection; }
      set { this.criteriaExpressionPropertyKeyCaptionCollectionBindingSource.DataSource = value; }
    }

    private CriteriaExpressionPropertyKeyCaption WorkingCriteriaExpressionProperty
    {
      get { return BindingSourceHelper.GetDataSourceCurrent(this.criteriaExpressionPropertyKeyCaptionCollectionBindingSource) as CriteriaExpressionPropertyKeyCaption; }
    }

    #endregion

    #region 方法

    private void ApplyRules()
    {
      this.okButton.Enabled = CriteriaExpressionProperties != null && CriteriaExpressionProperties.SelectedItems.Count > 0;
    }

    #endregion

    private void CriteriaExpressionPropertySelectDialog_Shown(object sender, System.EventArgs e)
    {
      ApplyRules();
    }

    private void criteriaExpressionPropertyKeyCaptionCollectionBindingSource_CurrentItemChanged(object sender, System.EventArgs e)
    {
      ApplyRules();
    }

    private void criteriaExpressionPropertyKeyCaptionCollectionGridView_CellValueChanging(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
    {
      if (e.Column == this.colSelected)
        WorkingCriteriaExpressionProperty.Selected = (bool)e.Value;
    }
  }
}
