using System;
using System.Windows.Forms;
using Phenix.Core;
using Phenix.Core.Mapping;
using Phenix.Core.Rule;
using Phenix.Core.Security;
using Phenix.Core.Windows;

namespace Phenix.Windows
{
  internal partial class PromptCodeEditDialog : Phenix.Core.Windows.DialogForm
  {
    private PromptCodeEditDialog()
    {
      InitializeComponent();

      this.readLevelEnumKeyCaptionBindingSource.DataSource = EnumKeyCaptionCollection.Fetch<ReadLevel>();
    }

    #region 工厂

    public static bool ExecuteAdd(PromptCodeKeyCaptionCollection promptCodeKeyCaptionCollection, ReadLevel readLevel, out string value)
    {
      using (PromptCodeEditDialog dialog = new PromptCodeEditDialog())
      {
        dialog.ExecuteAction = ExecuteAction.Update; ;
        dialog.PromptCodeKeyCaptionCollection = promptCodeKeyCaptionCollection;
        dialog.PromptCodeKeyCaption = promptCodeKeyCaptionCollection.CreatePromptCodeKeyCaption(readLevel);
        if (dialog.ShowDialog() == DialogResult.OK)
        {
          value = dialog.Value;
          return true;
        }
        value = null;
        return false;
      }
    }

    public static bool ExecuteDelete(PromptCodeKeyCaptionCollection promptCodeKeyCaptionCollection, string value)
    {
      using (PromptCodeEditDialog dialog = new PromptCodeEditDialog())
      {
        dialog.ExecuteAction = ExecuteAction.Delete;
        dialog.PromptCodeKeyCaptionCollection = promptCodeKeyCaptionCollection;
        dialog.PromptCodeKeyCaption = promptCodeKeyCaptionCollection.FindByValue(value);
        return dialog.ShowDialog() == DialogResult.OK;
      }
    }

    #endregion

    #region 属性

    private ExecuteAction ExecuteAction { get; set; }

    private PromptCodeKeyCaptionCollection PromptCodeKeyCaptionCollection { get; set; }

    private PromptCodeKeyCaption PromptCodeKeyCaption
    {
      get { return BindingSourceHelper.GetDataSourceCurrent(this.promptCodeKeyCaptionBindingSource) as PromptCodeKeyCaption; }
      set { this.promptCodeKeyCaptionBindingSource.DataSource = value; }
    }

    private string Value
    {
      get { return this.ValueTextEdit.Text; }
    }

    #endregion

    #region 方法

    private void ApplyRules()
    {
      this.ValueTextEdit.Enabled = ExecuteAction == ExecuteAction.Update;
      this.CaptionTextEdit.Enabled = ExecuteAction == ExecuteAction.Update;
      this.ReadLevelLookUpEdit.Enabled = ExecuteAction == ExecuteAction.Update;
      this.saveButton.Enabled = ExecuteAction == ExecuteAction.Update &&
        UserIdentity.CurrentIdentity != null && UserIdentity.CurrentIdentity.AllowSet(PromptCodeKeyCaption) &&
        !String.IsNullOrEmpty(this.ValueTextEdit.Text) &&
        !String.IsNullOrEmpty(this.CaptionTextEdit.Text);
      this.deleteButton.Enabled = ExecuteAction == ExecuteAction.Delete &&
        UserIdentity.CurrentIdentity != null && UserIdentity.CurrentIdentity.AllowSet(PromptCodeKeyCaption);
    }    

    #endregion

    private void PromptCodeEditDialog_Shown(object sender, EventArgs e)
    {
      ApplyRules();
    }

    private void ValueTextEdit_EditValueChanged(object sender, EventArgs e)
    {
      ApplyRules();
    }

    private void CaptionTextEdit_EditValueChanged(object sender, EventArgs e)
    {
      ApplyRules();
    }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
    private void saveButton_Click(object sender, System.EventArgs e)
    {
      try
      {
        PromptCodeKeyCaptionCollection.Save(PromptCodeKeyCaption);
        this.DialogResult = DialogResult.OK;
      }
      catch (Exception ex)
      {
        string hint = String.Format(Phenix.Windows.Properties.Resources.DataSaveAborted, PromptCodeKeyCaptionCollection.Caption, PromptCodeKeyCaption.Caption, AppUtilities.GetErrorHint(ex));
        MessageBox.Show(hint, Phenix.Windows.Properties.Resources.DataSave, MessageBoxButtons.OK, MessageBoxIcon.Error);
      }
    }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
    private void deleteButton_Click(object sender, System.EventArgs e)
    {
      if (MessageBox.Show(String.Format(Phenix.Windows.Properties.Resources.ConfirmDelete, PromptCodeKeyCaptionCollection.Caption, PromptCodeKeyCaption.Caption), Phenix.Windows.Properties.Resources.DataDelete,
        MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
        try
        {
          PromptCodeKeyCaptionCollection.Remove(PromptCodeKeyCaption);
          this.DialogResult = DialogResult.OK;
        }
        catch (Exception ex)
        {
          string hint = String.Format(Phenix.Windows.Properties.Resources.DataDeleteAborted, PromptCodeKeyCaptionCollection.Caption, PromptCodeKeyCaption.Caption, AppUtilities.GetErrorHint(ex));
          MessageBox.Show(hint, Phenix.Windows.Properties.Resources.DataDelete, MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
  }
}
