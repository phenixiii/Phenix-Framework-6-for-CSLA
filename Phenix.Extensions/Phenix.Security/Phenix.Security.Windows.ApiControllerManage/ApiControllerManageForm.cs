using System;
using Phenix.Core.Dictionary;
using Phenix.Core.Windows;
using Phenix.Security.Business;

namespace Phenix.Security.Windows.ApiControllerManage
{
  public partial class ApiControllerManageForm : Phenix.Core.Windows.BaseForm
  {
    public ApiControllerManageForm()
    {
      InitializeComponent();
    }

    #region 属性

    private AssemblyClassCriteria AssemblyClassCriteria
    {
      get { return BindingSourceHelper.GetDataSourceCurrent(this.assemblyClassCriteriaBindingSource) as AssemblyClassCriteria; }
    }

    private SelectableRoleForAssemblyClassMethodList AssemblyClassMethodSelectableRoles
    {
      get { return BindingSourceHelper.GetDataSourceList(this.assemblyClassMethodSelectableRolesBindingSource) as SelectableRoleForAssemblyClassMethodList; }
    }

    private SelectableRoleForAssemblyClassList AssemblyClassSelectableRoles
    {
      get { return BindingSourceHelper.GetDataSourceList(this.assemblyClassSelectableRolesBindingSource) as SelectableRoleForAssemblyClassList; }
    }

    private AssemblyClass WorkingAssemblyClass
    {
      get { return BindingSourceHelper.GetDataSourceCurrent(this.assemblyClassListBindingSource) as AssemblyClass; }
    }

    #endregion

    #region 方法

    private void Humanistic()
    {
      businessGroupControl.Text = String.Format("类信息：{0}", WorkingAssemblyClass != null
        ? String.Format("{0}.{1}", WorkingAssemblyClass.Name, WorkingAssemblyClass.Caption)
        : "无");
    }

    #endregion
    
    private void assemblyClassMethodSelectAllRolesButton_Click(object sender, System.EventArgs e)
    {
      AssemblyClassMethodSelectableRoles.SelectAll();
    }

    private void assemblyClassMethodInverseAllRolesButton_Click(object sender, System.EventArgs e)
    {
      AssemblyClassMethodSelectableRoles.InverseAll();
    }

    private void assemblyClassSelectAllRolesButton_Click(object sender, System.EventArgs e)
    {
      AssemblyClassSelectableRoles.SelectAll();
    }

    private void assemblyClassInverseAllRolesButton_Click(object sender, System.EventArgs e)
    {
      AssemblyClassSelectableRoles.InverseAll();
    }

    private void assemblyClassesBindingSource_PositionChanged(object sender, System.EventArgs e)
    {
      Humanistic();
    }

    private void assemblyClassesBindingSource_ListChanged(object sender, System.ComponentModel.ListChangedEventArgs e)
    {
      Humanistic();
    }

    private void barManager_Fetching(object sender, Phenix.Windows.BarItemClickEventArgs e)
    {
      AssemblyClassCriteria.AddType(AssemblyClassType.ApiController);
    }

    private void barManager_Saved(object sender, Phenix.Windows.BarItemSaveEventArgs e)
    {
      DataDictionaryHub.AssemblyInfoHasChanged();
      DataDictionaryHub.RoleInfoHasChanged();
    }
  }
}
