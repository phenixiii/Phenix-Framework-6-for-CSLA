using System;
using System.Windows.Forms;

namespace Phenix.Windows.Helper
{
  #region 定义

  internal delegate Exception CheckTemplateExistsCallback(string templateName);

  #endregion

  internal partial class TemplateSetDialog : Phenix.Core.Windows.DialogForm
  {
    private TemplateSetDialog()
    {
      InitializeComponent();
    }

    #region 工厂

    public static string Execute(CheckTemplateExistsCallback checkTemplateExistsCallback)
    {
      using (TemplateSetDialog dialog = new TemplateSetDialog())
      {
        dialog.CheckTemplateExistsCallback = checkTemplateExistsCallback;
        return dialog.ShowDialog() == DialogResult.OK ? dialog.TemplateName : null;
      }
    }

    #endregion

    #region 属性

    private string TemplateName
    {
      get { return TemplateFile.TidyTemplateName(!String.IsNullOrEmpty(this.templateNameTextEdit.Text) ? this.templateNameTextEdit.Text.Trim() : String.Empty); }
    }

    private CheckTemplateExistsCallback CheckTemplateExistsCallback { get; set; }

    #endregion

    #region 方法

    private void Humanistic()
    {
      this.templateNameTextEdit.Focus();
    }

    private void ApplyRules()
    {
      this.okButton.Enabled = !String.IsNullOrEmpty(TemplateName);
    }

    #endregion

    private void NewTemplateDialog_Shown(object sender, EventArgs e)
    {
      ApplyRules();
      Humanistic();
    }
    
    private void templateNameTextEdit_EditValueChanged(object sender, EventArgs e)
    {
      ApplyRules();
    }

    private void okButton_Click(object sender, EventArgs e)
    {
      this.Cursor = Cursors.WaitCursor;
      try
      {
        Exception ex = CheckTemplateExistsCallback(TemplateName);
        if (ex == null)
        {
          this.DialogResult = DialogResult.OK;
        }
        else
        {
          MessageBox.Show(ex.Message, this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
          Humanistic();
        }
      }
      finally
      {
        this.Cursor = Cursors.Default;
      }
    }
  }
}