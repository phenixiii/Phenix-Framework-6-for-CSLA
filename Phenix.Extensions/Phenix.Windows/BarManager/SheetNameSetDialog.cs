using System;
using System.Windows.Forms;

namespace Phenix.Windows
{
  internal partial class SheetNameSetDialog : Phenix.Core.Windows.DialogForm
  {
    private SheetNameSetDialog()
    {
      InitializeComponent();
    }

    #region 工厂

    public static bool Execute(ref string sheetName, ref bool saveSheetConfig)
    {
      using (SheetNameSetDialog dialog = new SheetNameSetDialog())
      {
        dialog.OldSheetName = sheetName;
        dialog.SheetNameText = sheetName;
        dialog.SaveSheetConfig = saveSheetConfig;
        if (dialog.ShowDialog() == DialogResult.OK)
        {
          sheetName = dialog.SheetNameText;
          saveSheetConfig = dialog.SaveSheetConfig;
          return true;
        }
        return false;
      }
    }

    #endregion

    #region 属性

    private string OldSheetName { get; set; }

    private string SheetNameText
    {
      get { return this.SheetNameTextEdit.Text; }
      set { this.SheetNameTextEdit.Text = value; }
    }

    private bool SaveSheetConfig
    {
      get { return this.SaveSheetConfigCheckEdit.Checked; }
      set { this.SaveSheetConfigCheckEdit.Checked = value; }
    }

    #endregion

    #region 方法

    private void Humanistic()
    {
      if (String.CompareOrdinal(OldSheetName, SheetNameText) == 0)
        this.SheetNameTextEdit.Focus();
      else
        this.okButton.Focus();
    }

    private void ApplyRules()
    {
      this.okButton.Enabled = 
        String.CompareOrdinal(OldSheetName, SheetNameText) != 0;
    } 

    #endregion

    private void InputSheetNameDialog_Shown(object sender, EventArgs e)
    {
      ApplyRules();
      Humanistic();
    }

    private void SheetNameTextEdit_EditValueChanged(object sender, EventArgs e)
    {
      ApplyRules();
    }
  }
}
