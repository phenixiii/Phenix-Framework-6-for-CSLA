using System;
using Phenix.Core.Windows;
using Phenix.Security.Business;

namespace Phenix.Security.Windows.UserLogManage
{
  public partial class UserLogManageForm : Phenix.Core.Windows.BaseForm
  {
    public UserLogManageForm()
    {
      InitializeComponent();
    }

    #region 属性
    
    private UserLogList WorkingUserLogs
    {
      get { return BindingSourceHelper.GetDataSourceList(this.userLogListBindingSource) as UserLogList; }
    }
    
    private UserLog WorkingUserLog
    {
      get { return BindingSourceHelper.GetDataSourceCurrent(this.userLogListBindingSource) as UserLog; }
    }

    #endregion

    private void selectAllButton_Click(object sender, EventArgs e)
    {
      WorkingUserLogs.SelectAll();
    }

    private void inverseAllButton_Click(object sender, EventArgs e)
    {
      WorkingUserLogs.InverseAll();
    }

    private void barManager_Deleting(object sender, Phenix.Windows.BarItemDeleteEventArgs e)
    {
      e.Applied = true;
      if (!Phenix.Windows.Helper.PersistentHelper.Execute(new Action(WorkingUserLog.DeleteOwnerSelectedOrSelf), "删除日志"))
        e.Succeed = false;
    }
  }
}
