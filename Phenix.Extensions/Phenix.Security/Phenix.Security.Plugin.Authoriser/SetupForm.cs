using System;
using System.Windows.Forms;

namespace Phenix.Security.Plugin.Authoriser
{
  public partial class SetupForm : Phenix.Core.Windows.DialogForm
  {
    private SetupForm(Plugin plugin)
    {
      InitializeComponent();

      _plugin = plugin;

      this.ldapPathTextBox.Text = plugin.LdapPath;
      this.userNamePrdfixTextBox.Text = plugin.UserNamePrdfix;
      this.adminNameTextBox.Text = plugin.AdminName;
      this.adminPasswordTextBox.Text = plugin.AdminPassword;
    }

    #region 工厂

    /// <summary>
    /// 执行
    /// </summary>
    /// <param name="plugin">插件</param>
    public static bool Execute(Plugin plugin)
    {
      using (SetupForm form = new SetupForm(plugin))
      {
        return (form.ShowDialog() == DialogResult.OK);
      }
    }

    #endregion

    #region 属性

    private readonly Plugin _plugin;

    #endregion

    private void SetupForm_Shown(object sender, EventArgs e)
    {
      this.ldapPathTextBox.Focus();
    }

    private void okButton_Click(object sender, System.EventArgs e)
    {
      _plugin.LdapPath = this.ldapPathTextBox.Text;
      _plugin.UserNamePrdfix = this.userNamePrdfixTextBox.Text;
      _plugin.AdminName = this.adminNameTextBox.Text;
      _plugin.AdminPassword = this.adminPasswordTextBox.Text;

      this.DialogResult = DialogResult.OK;
    }

    private void testButton_Click(object sender, System.EventArgs e)
    {
      TestForm.Execute(this.ldapPathTextBox.Text, this.userNamePrdfixTextBox.Text);
    }
  }
}
