using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using Phenix.Core.Windows;

namespace Phenix.Core.Security.Windows
{
  /// <summary>
  /// 选择角色对话框
  /// </summary>
  public partial class RolesSelectDialog : DialogForm
  {
    private RolesSelectDialog(string selectedRoles, IIdentity identity)
    {
      InitializeComponent();

      this.Text = Resources.RolesSelect;
      this.unselectedRolesLabel.Text = Resources.UnselectedRoles;
      this.selectedRolesLabel.Text = Resources.SelectedRoles;
      this.okButton.Text = Phenix.Core.Properties.Resources.Ok;
      this.cancelButton.Text = Phenix.Core.Properties.Resources.Cancel;

      List<string> grantRoles = new List<string>(identity.GrantRoles.Keys);
      if (!String.IsNullOrEmpty(selectedRoles))
        foreach (string s in selectedRoles.Split(ROLE_SEPARATOR))
        {
          this.selectedRolesListBox.Items.Add(s);
          grantRoles.Remove(s);
        }
      foreach (string s in grantRoles)
        this.unselectRolesListBox.Items.Add(s);
    }

    #region 工厂

    /// <summary>
    /// 执行
    /// </summary>
    public static string Execute(IIdentity identity)
    {
      return Execute(null, identity);
    }

    /// <summary>
    /// 执行
    /// </summary>
    public static string Execute(string selectedRoles, IIdentity identity)
    {
      using (RolesSelectDialog dialog = new RolesSelectDialog(selectedRoles, identity))
      {
        if (dialog.ShowDialog() == DialogResult.OK)
          return dialog.SelectedRoles;
      }
      return selectedRoles;
    }

    #endregion

    #region 属性

    /// <summary>
    /// 值分隔符
    /// </summary>
    public const char ROLE_SEPARATOR = AppConfig.VALUE_SEPARATOR;

    /// <summary>
    /// 被选择的角色清单
    /// </summary>
    public string SelectedRoles
    {
      get
      {
        StringBuilder result = new StringBuilder();
        foreach (string s in this.selectedRolesListBox.Items)
        {
          result.Append(s);
          result.Append(ROLE_SEPARATOR);
        }
        return result.ToString();
      }
    }

    #endregion

    private void selectAllButton_Click(object sender, System.EventArgs e)
    {
      this.selectedRolesListBox.Items.AddRange(this.unselectRolesListBox.Items);
      this.unselectRolesListBox.Items.Clear();
    }

    private void selectButton_Click(object sender, System.EventArgs e)
    {
      object[] unselectRoles = new object[this.unselectRolesListBox.SelectedItems.Count];
      this.unselectRolesListBox.SelectedItems.CopyTo(unselectRoles, 0);
      foreach (string s in unselectRoles)
      {
        this.selectedRolesListBox.Items.Add(s);
        this.unselectRolesListBox.Items.Remove(s);
      }
    }

    private void unselectButton_Click(object sender, System.EventArgs e)
    {
      object[] selectedRoles = new object[this.selectedRolesListBox.SelectedItems.Count];
      this.selectedRolesListBox.SelectedItems.CopyTo(selectedRoles, 0);
      foreach (string s in selectedRoles)
      {
        this.unselectRolesListBox.Items.Add(s);
        this.selectedRolesListBox.Items.Remove(s);
      }
    }

    private void unselectAllButton_Click(object sender, System.EventArgs e)
    {
      this.unselectRolesListBox.Items.AddRange(this.selectedRolesListBox.Items);
      this.selectedRolesListBox.Items.Clear();
    }

    private void unselectRolesListBox_DoubleClick(object sender, System.EventArgs e)
    {
      selectButton_Click(sender, e);
    }

    private void selectedRolesListBox_DoubleClick(object sender, System.EventArgs e)
    {
      unselectButton_Click(sender, e);
    }
  }
}
