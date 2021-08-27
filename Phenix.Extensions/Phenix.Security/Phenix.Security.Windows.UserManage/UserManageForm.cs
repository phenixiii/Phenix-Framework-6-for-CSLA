using System;
using System.Collections.Generic;
using System.Drawing;
using DevExpress.XtraEditors.Controls;
using Phenix.Core.Windows;
using Phenix.Security.Business;

namespace Phenix.Security.Windows.UserManage
{
  public partial class UserManageForm : Phenix.Core.Windows.BaseForm
  {
    public UserManageForm()
    {
      InitializeComponent();
    }

    #region 属性

    private UserList WorkingUsers
    {
      get { return BindingSourceHelper.GetDataSourceList(this.userListBindingSource) as UserList; }
    }

    private User WorkingUser
    {
      get { return BindingSourceHelper.GetDataSourceCurrent(this.userListBindingSource) as User; }
    }

    private RoleReadOnlyList SelectableRoles
    {
      get { return BindingSourceHelper.GetDataSourceList(this.selectableRolesBindingSource) as RoleReadOnlyList; }
    }

    private RoleReadOnlyList SelectableGrantRoles
    {
      get { return BindingSourceHelper.GetDataSourceList(this.selectableGrantRolesBindingSource) as RoleReadOnlyList; }
    }

    private SectionReadOnlyList SelectableSections
    {
      get { return BindingSourceHelper.GetDataSourceList(this.selectableSectionsBindingSource) as SectionReadOnlyList; }
    }

    private List<object> _workingUserDetailTag;

    #endregion

    #region 方法

    private bool LockUser()
    {
      return WorkingUser.Lock();
    }

    private bool UnlockUser()
    {
      return WorkingUser.Unlock();
    }

    private bool UnlockPasswordUser()
    {
      return WorkingUser.UnlockPassword();
    }

    private bool ResetPasswordUser()
    {
      return WorkingUser.ResetPassword();
    }

    private void ClearUserDetailDesign()
    {
      this.selectableRolesBindingSource.DataMember = null;
      this.selectableRolesBindingSource.DataSource = null;

      this.selectableGrantRolesBindingSource.DataMember = null;
      this.selectableGrantRolesBindingSource.DataSource = null;

      this.selectableSectionsBindingSource.DataMember = null;
      this.selectableSectionsBindingSource.DataSource = null;
    }

    private void ChangeUserDetailBinding()
    {
      if (WorkingUser == null)
        return;

      if (_workingUserDetailTag != null && _workingUserDetailTag.Contains(WorkingUser))
      {
        if (_workingUserDetailTag.Contains(this.userDetailTabControl.SelectedTabPageIndex))
          return;
      }
      else
      {
        _workingUserDetailTag = new List<object>
        {
          WorkingUser
        };
      }
      _workingUserDetailTag.Add(this.userDetailTabControl.SelectedTabPageIndex);

      if (this.userDetailTabControl.SelectedTabPage == this.selectableRolesTabPage)
        this.selectableRolesBindingSource.DataSource = WorkingUser.SelectableRoles;
      else if (this.userDetailTabControl.SelectedTabPage == this.selectableGrantRolesTabPage)
        this.selectableGrantRolesBindingSource.DataSource = WorkingUser.SelectableGrantRoles;
      else if (this.userDetailTabControl.SelectedTabPage == this.selectableSectionsTabPage)
        this.selectableSectionsBindingSource.DataSource = WorkingUser.SelectableSections;
    }

    #endregion

    private void selectAllRolesButton_Click(object sender, EventArgs e)
    {
      SelectableRoles.SelectAll();
    }

    private void inverseAllRolesButton_Click(object sender, EventArgs e)
    {
      SelectableRoles.InverseAll();
    }

    private void selectAllGrantRolesButton_Click(object sender, EventArgs e)
    {
      SelectableGrantRoles.SelectAll();
    }

    private void inverseAllGrantRolesButton_Click(object sender, EventArgs e)
    {
      SelectableGrantRoles.InverseAll();
    }

    private void selectAllSectionsButton_Click(object sender, EventArgs e)
    {
      SelectableSections.SelectAll();
    }

    private void inverseAllSectionsButton_Click(object sender, EventArgs e)
    {
      SelectableSections.InverseAll();
    }

    private void US_DP_IDLookUpEdit_Properties_ButtonClick(object sender, ButtonPressedEventArgs e)
    {
      if (e.Button.Kind == ButtonPredefines.Ellipsis)
        if (WorkingUser != null)
          Phenix.Core.Plugin.PluginHost.Default.SendSingletonMessage(
            "Phenix.Security.Windows.DepartmentManage",
            new DepartmentCriteria() { DP_ID = WorkingUser.US_DP_ID });
    }

    private void US_PT_IDLookUpEdit_Properties_ButtonClick(object sender, ButtonPressedEventArgs e)
    {
      if (e.Button.Kind == ButtonPredefines.Ellipsis)
        if (WorkingUser != null)
          Phenix.Core.Plugin.PluginHost.Default.SendSingletonMessage(
            "Phenix.Security.Windows.PositionManage",
            new PositionCriteria() { PT_ID = WorkingUser.US_PT_ID });
    }

    private void userListGridView_CustomDrawCell(object sender, DevExpress.XtraGrid.Views.Base.RowCellCustomDrawEventArgs e)
    {
      User user = this.userListGridView.GetRow(e.RowHandle) as User;
      if (user == null)
        return;
      if (user.Locked ?? false)
        e.Appearance.BackColor = Color.Red;
      else if (!this.unifyControlLayout.ResetRule(this.userListGridView))
        e.Appearance.BackColor = Color.White;
    }

    private void bbiLock_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
    {
      Phenix.Windows.Helper.PersistentHelper.Execute((Func<bool>)LockUser, bbiLock.Caption);
    }

    private void bbiUnlock_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
    {
      Phenix.Windows.Helper.PersistentHelper.Execute((Func<bool>)UnlockUser, bbiUnlock.Caption);
    }
    
    private void bbiUnlockPassword_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
    {
      Phenix.Windows.Helper.PersistentHelper.Execute((Func<bool>)UnlockPasswordUser, bbiUnlockPassword.Caption);
    }

    private void bbiResetPassword_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
    {
      Phenix.Windows.Helper.PersistentHelper.Execute((Func<bool>)ResetPasswordUser, bbiResetPassword.Caption);
    }

    private void UserManageForm_Shown(object sender, EventArgs e)
    {
      ClearUserDetailDesign();
    }
    
    private void barManager_Fetching(object sender, Phenix.Windows.BarItemClickEventArgs e)
    {
      ClearUserDetailDesign();
    }
    
    private void barManager_Fetched(object sender, Phenix.Windows.BarItemClickEventArgs e)
    {
      ChangeUserDetailBinding();
    }

    private void userListBindingSource_PositionChanged(object sender, EventArgs e)
    {
      ChangeUserDetailBinding();
    }

    private void userDetailTabControl_SelectedPageChanged(object sender, DevExpress.XtraTab.TabPageChangedEventArgs e)
    {
      ChangeUserDetailBinding();
    }

    private void bbiUserLogManage_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
    {
      Phenix.Core.Plugin.PluginHost.Default.SendSingletonMessage(
        "Phenix.Security.Windows.UserLogManage",
        WorkingUser != null ? new UserLogCriteria() {UserNumber = WorkingUser.UserNumber} : null);
    }
  }
}
