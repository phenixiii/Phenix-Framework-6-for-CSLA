using System;
using System.Windows.Forms;

namespace Phenix.Security.Plugin.Authoriser
{
  public partial class TestForm : Phenix.Core.Windows.DialogForm
  {
    public TestForm(string ldapPath, string userNamePrdfix)
    {
      InitializeComponent();

      _ldapPath = ldapPath;
      _userNamePrdfix = userNamePrdfix;
    }

    #region 工厂

    /// <summary>
    /// 执行
    /// </summary>
    public static bool Execute(string ldapPath, string userNamePrdfix)
    {
      using (TestForm form = new TestForm(ldapPath, userNamePrdfix))
      {
        return (form.ShowDialog() == DialogResult.OK);
      }
    }

    #endregion

    #region 属性

    private readonly string _ldapPath;
    private readonly string _userNamePrdfix;

    #endregion
    
    private void TestForm_Shown(object sender, EventArgs e)
    {
      this.userNumberTextBox.Focus();
    }

    private void okButton_Click(object sender, EventArgs e)
    {
      try
      {
        if (Plugin.LogOn(_ldapPath, _userNamePrdfix, this.userNumberTextBox.Text, this.passwordTextBox.Text))
        {
          MessageBox.Show("succeed", "Test", MessageBoxButtons.OK, MessageBoxIcon.Information);
          this.DialogResult = DialogResult.OK;
        }
        else
          MessageBox.Show("nonentity", "Test", MessageBoxButtons.OK, MessageBoxIcon.Error);
      }
      catch (Exception ex)
      {
        MessageBox.Show(Phenix.Core.AppUtilities.GetErrorHint(ex), "Test", MessageBoxButtons.OK, MessageBoxIcon.Error);
      }
      this.userNumberTextBox.Focus();
    }
  }
}
