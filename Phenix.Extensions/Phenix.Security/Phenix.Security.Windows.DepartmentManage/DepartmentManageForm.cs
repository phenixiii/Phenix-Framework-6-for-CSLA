using System;
using DevExpress.XtraEditors.Controls;
using Phenix.Core.Dictionary;
using Phenix.Core.Windows;
using Phenix.Security.Business;

namespace Phenix.Security.Windows.DepartmentManage
{
  public partial class DepartmentManageForm : Phenix.Core.Windows.BaseForm
  {
    public DepartmentManageForm()
    {
      InitializeComponent();
    }

    #region 属性

    private DepartmentTreeNode WorkingDepartmentTreeNode
    {
      get { return BindingSourceHelper.GetDataSourceCurrent(this.departmentTreeBindingSource) as DepartmentTreeNode; }
    }

    private UserList SelectableUsers
    {
      get { return BindingSourceHelper.GetDataSourceList(this.selectableUsersBindingSource) as UserList; }
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

    private void DP_PT_IDGridLookUpEdit_Properties_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
    {
      if (e.Button.Kind == ButtonPredefines.Ellipsis)
        if (WorkingDepartmentTreeNode != null)
          Phenix.Core.Plugin.PluginHost.Default.SendSingletonMessage(
            "Phenix.Security.Windows.PositionManage",
            new PositionCriteria() { PT_ID = WorkingDepartmentTreeNode.DP_PT_ID});
    }

    private void barManager_Saved(object sender, Phenix.Windows.BarItemSaveEventArgs e)
    {
      DataDictionaryHub.DepartmentInfoHasChanged();
    }
  }
}
