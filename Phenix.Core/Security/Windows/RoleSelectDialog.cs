using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Phenix.Core.Windows;

namespace Phenix.Core.Security.Windows
{
  /// <summary>
  /// 选择角色对话框
  /// </summary>
  public partial class RoleSelectDialog : DialogForm
  {
    private RoleSelectDialog(string selectedRole, IIdentity identity)
    {
      InitializeComponent();

      this.Text = Resources.RoleSelect;
      this.okButton.Text = Phenix.Core.Properties.Resources.Ok;
      this.cancelButton.Text = Phenix.Core.Properties.Resources.Cancel;

      if (identity != null)
      {
        List<string> grantRoles = new List<string>(identity.GrantRoles.Keys);
        foreach (string s in grantRoles)
          this.rolesListBox.Items.Add(s);
      }
      this.rolesListBox.SelectedItem = selectedRole;
    }

    #region 工厂

    /// <summary>
    /// 执行
    /// </summary>
    public static string Execute(string selectedRole)
    {
      return Execute(selectedRole, UserIdentity.CurrentIdentity);
    }

    /// <summary>
    /// 执行
    /// </summary>
    public static string Execute(string selectedRole, IIdentity identity)
    {
      using (RoleSelectDialog dialog = new RoleSelectDialog(selectedRole, identity))
      {
        if (dialog.ShowDialog() == DialogResult.OK)
          return dialog.SelectedRole;
      }
      return selectedRole;
    }

    #endregion

    #region 属性
    
    /// <summary>
    /// 被选择的角色
    /// </summary>
    public string SelectedRole
    {
      get
      {
        return this.rolesListBox.SelectedIndex >= 0
          ? (string)this.rolesListBox.SelectedItem
          : null;
      }
    }

    #endregion

    private void rolesListBox_DoubleClick(object sender, EventArgs e)
    {
      this.DialogResult = DialogResult.OK;
    }
  }
}
