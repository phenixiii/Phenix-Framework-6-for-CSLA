using System;
using Phenix.Core.Dictionary;
using Phenix.Core.Security;
using Phenix.Core.Windows;
using Phenix.Security.Business;

namespace Phenix.Security.Windows.PositionManage
{
  public partial class PositionManageForm : Phenix.Core.Windows.BaseForm
  {
    public PositionManageForm()
    {
      InitializeComponent();
    }

    #region 属性

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

    private void barManager_Saved(object sender, Phenix.Windows.BarItemSaveEventArgs e)
    {
      DataDictionaryHub.PositionInfoHasChanged();
    }
  }
}
