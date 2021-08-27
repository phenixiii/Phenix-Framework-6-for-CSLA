using System;
using System.Windows.Forms;
using Phenix.Core;
using Phenix.Core.Rule;
using Phenix.Core.Security;
using Phenix.Core.Windows;

namespace Phenix.Windows
{
  internal partial class CriteriaExpressionNewDialog : Phenix.Core.Windows.DialogForm
  {
    private CriteriaExpressionNewDialog()
    {
      InitializeComponent();

      this.readLevelEnumKeyCaptionBindingSource.DataSource = EnumKeyCaptionCollection.Fetch<ReadLevel>();
    }

    #region 工厂

    public static CriteriaExpressionKeyCaption Execute(CriteriaExpressionKeyCaptionCollection criteriaExpressionKeyCaptionCollection, ReadLevel readLevel)
    {
      CriteriaExpressionPropertyKeyCaptionCollection criteriaExpressionProperties = CriteriaExpressionPropertySelectDialog.Execute(criteriaExpressionKeyCaptionCollection);
      if (criteriaExpressionProperties != null)
        using (CriteriaExpressionNewDialog dialog = new CriteriaExpressionNewDialog())
        {
          dialog.CriteriaExpressionKeyCaptionCollection = criteriaExpressionKeyCaptionCollection;
          dialog.CriteriaExpressionKeyCaption = CriteriaExpressionKeyCaption.New(criteriaExpressionProperties, readLevel, UserIdentity.CurrentIdentity);
          return dialog.ShowDialog() == DialogResult.OK ? dialog.CriteriaExpressionKeyCaption : null;
        }
      return null;
    }

    #endregion

    #region 属性

    private CriteriaExpressionKeyCaptionCollection CriteriaExpressionKeyCaptionCollection { get; set; }

    private CriteriaExpressionKeyCaption CriteriaExpressionKeyCaption
    {
      get { return BindingSourceHelper.GetDataSourceCurrent(this.criteriaExpressionKeyCaptionBindingSource) as CriteriaExpressionKeyCaption; }
      set { this.criteriaExpressionKeyCaptionBindingSource.DataSource = value; }
    }

    #endregion

    #region 方法

    private void ApplyRules()
    {
      this.okButton.Enabled =
        UserIdentity.CurrentIdentity != null && UserIdentity.CurrentIdentity.AllowSet(CriteriaExpressionKeyCaption) &&
        !String.IsNullOrEmpty(this.CaptionTextEdit.Text);
    }

    #endregion

    private void CriteriaExpressionNewDialog_Shown(object sender, System.EventArgs e)
    {
      ApplyRules();
    }

    private void CaptionTextEdit_EditValueChanged(object sender, EventArgs e)
    {
      ApplyRules();
    }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
    private void okButton_Click(object sender, System.EventArgs e)
    {
      try
      {
        CriteriaExpressionKeyCaptionCollection.Save(CriteriaExpressionKeyCaption);
        this.DialogResult = DialogResult.OK;
      }
      catch (Exception ex)
      {
        string hint = String.Format(Phenix.Windows.Properties.Resources.DataSaveAborted, CriteriaExpressionKeyCaptionCollection.Caption, CriteriaExpressionKeyCaption.Caption, AppUtilities.GetErrorHint(ex));
        MessageBox.Show(hint, Phenix.Windows.Properties.Resources.DataSave, MessageBoxButtons.OK, MessageBoxIcon.Error);
      } 
    }
  }
}
