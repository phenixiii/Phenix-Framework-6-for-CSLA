using System;
using Phenix.Core.Dictionary;
using Phenix.Core.Windows;
using Phenix.Security.Business;

namespace Phenix.Security.Windows.RoleManage
{
  public partial class RoleManageForm : Phenix.Core.Windows.BaseForm
  {
    public RoleManageForm()
    {
      InitializeComponent();
    }

    #region 属性

    private RoleList WorkingRoles
    {
      get { return BindingSourceHelper.GetDataSourceList(this.roleListBindingSource) as RoleList; }
    }

    private Role WorkingRole
    {
      get { return BindingSourceHelper.GetDataSourceCurrent(this.roleListBindingSource) as Role; }
    }

    private UserReadOnlyList SelectableUsers
    {
      get { return BindingSourceHelper.GetDataSourceList(this.selectableUsersBindingSource) as UserReadOnlyList; }
    }

    #endregion

    private void selectAllButton_Click(object sender, EventArgs e)
    {
      SelectableUsers.SelectAll();
    }

    private void inverseAllButton_Click(object sender, EventArgs e)
    {
      SelectableUsers.InverseAll();
    }

    private void barManager_Saved(object sender, Phenix.Windows.BarItemSaveEventArgs e)
    {
      DataDictionaryHub.RoleInfoHasChanged();
    }
  }
}
