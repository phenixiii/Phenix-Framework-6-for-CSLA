using System;
using Phenix.Core.Dictionary;
using Phenix.Core.Windows;
using Phenix.Security.Business;

namespace Phenix.Security.Windows.AssemblyManage
{
  public partial class AssemblyManageForm : Phenix.Core.Windows.BaseForm
  {
    public AssemblyManageForm()
    {
      InitializeComponent();
    }

    #region 属性

    private SelectableRoleForAssemblyClassPropertyList AssemblyClassPropertySelectableRoles
    {
      get { return BindingSourceHelper.GetDataSourceList(this.assemblyClassPropertySelectableRolesBindingSource) as SelectableRoleForAssemblyClassPropertyList; }
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
      get { return BindingSourceHelper.GetDataSourceCurrent(this.assemblyClassesBindingSource) as AssemblyClass; }
    }

    #endregion

    #region 方法

    private void Humanistic()
    {
      businessGroupControl.Text = String.Format("类信息：{0}", WorkingAssemblyClass != null
        ? String.Format("{0}.{1}", WorkingAssemblyClass.Name, WorkingAssemblyClass.Caption)
        : "无");

      if (WorkingAssemblyClass != null)
        switch (WorkingAssemblyClass.Type)
        {
          case Phenix.Core.Dictionary.AssemblyClassType.Business:
            this.colAssemblyClassPermanentExecuteAction.Visible = true;
            this.colAssemblyClassPropertyPermanentExecuteModify.Visible = true;
            this.colAssemblyClassPropertyRequired.Visible = true;
            this.colAssemblyClassPropertyVisible.Visible = true;
            this.colAssemblyClassSelectableRoleAllowCreate.Visible = true;
            this.colAssemblyClassSelectableRoleAllowDelete_.Visible = true;
            this.colAssemblyClassSelectableRoleAllowEdit_.Visible = true;
            this.colAssemblyClassPropertySelectableRoleAllowWrite.Visible = true;
            break;
          case Phenix.Core.Dictionary.AssemblyClassType.Businesses:
            this.colAssemblyClassPermanentExecuteAction.Visible = false;
            this.colAssemblyClassPropertyPermanentExecuteModify.Visible = false;
            this.colAssemblyClassPropertyRequired.Visible = false;
            this.colAssemblyClassPropertyVisible.Visible = false;
            this.colAssemblyClassSelectableRoleAllowCreate.Visible = true;
            this.colAssemblyClassSelectableRoleAllowDelete_.Visible = true;
            this.colAssemblyClassSelectableRoleAllowEdit_.Visible = true;
            this.colAssemblyClassPropertySelectableRoleAllowWrite.Visible = false;
            break;
          case Phenix.Core.Dictionary.AssemblyClassType.Command:
            this.colAssemblyClassPermanentExecuteAction.Visible = true;
            this.colAssemblyClassPropertyPermanentExecuteModify.Visible = false;
            this.colAssemblyClassPropertyRequired.Visible = false;
            this.colAssemblyClassPropertyVisible.Visible = false;
            this.colAssemblyClassSelectableRoleAllowCreate.Visible = false;
            this.colAssemblyClassSelectableRoleAllowDelete_.Visible = false;
            this.colAssemblyClassSelectableRoleAllowEdit_.Visible = true;
            this.colAssemblyClassPropertySelectableRoleAllowWrite.Visible = false;
            break;
          default:
            this.colAssemblyClassPermanentExecuteAction.Visible = false;
            this.colAssemblyClassPropertyPermanentExecuteModify.Visible = false;
            this.colAssemblyClassPropertyRequired.Visible = false;
            this.colAssemblyClassPropertyVisible.Visible = false;
            this.colAssemblyClassSelectableRoleAllowCreate.Visible = false;
            this.colAssemblyClassSelectableRoleAllowDelete_.Visible = false;
            this.colAssemblyClassSelectableRoleAllowEdit_.Visible = false;
            this.colAssemblyClassPropertySelectableRoleAllowWrite.Visible = false;
            break;
        }
    }

    #endregion

    private void assemblyClassPropertySelectAllRolesButton_Click(object sender, System.EventArgs e)
    {
      AssemblyClassPropertySelectableRoles.SelectAll();
    }

    private void assemblyClassPropertyInverseAllRolesButton_Click(object sender, System.EventArgs e)
    {
      AssemblyClassPropertySelectableRoles.InverseAll();
    }

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

    private void barManager_Saved(object sender, Phenix.Windows.BarItemSaveEventArgs e)
    {
      DataDictionaryHub.AssemblyInfoHasChanged();
      DataDictionaryHub.RoleInfoHasChanged();
    }
  }
}
