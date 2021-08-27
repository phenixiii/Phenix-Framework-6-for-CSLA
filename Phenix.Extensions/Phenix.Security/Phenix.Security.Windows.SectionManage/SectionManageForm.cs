using System;
using Phenix.Core.Dictionary;
using Phenix.Core.Windows;
using Phenix.Security.Business;

namespace Phenix.Security.Windows.SectionManage
{
  public partial class SectionManageForm : Phenix.Core.Windows.BaseForm
  {
    public SectionManageForm()
    {
      InitializeComponent();
    }

    #region 属性

    private SelectableTableFilterForSectionList SelectableTableFilters
    {
      get { return BindingSourceHelper.GetDataSourceList(this.selectableTableFiltersBindingSource) as SelectableTableFilterForSectionList; }
    }

    #endregion

    private void selectAllButton_Click(object sender, EventArgs e)
    {
      SelectableTableFilters.SelectAll();
    }

    private void inverseAllButton_Click(object sender, EventArgs e)
    {
      SelectableTableFilters.InverseAll();
    }

    private void barManager_Saved(object sender, Phenix.Windows.BarItemSaveEventArgs e)
    {
       DataDictionaryHub.SectionInfoHasChanged();
    }
  }
}
