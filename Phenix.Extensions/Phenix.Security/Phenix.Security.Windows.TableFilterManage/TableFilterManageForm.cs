using System;
using Phenix.Core.Dictionary;
using Phenix.Core.Windows;
using Phenix.Security.Business;

namespace Phenix.Security.Windows.TableFilterManage
{
  public partial class TableFilterManageForm : Phenix.Core.Windows.BaseForm
  {
    public TableFilterManageForm()
    {
      InitializeComponent();
    }

    #region 属性

    private SectionReadOnlyList SelectableSections
    {
      get { return BindingSourceHelper.GetDataSourceList(this.selectableSectionsBindingSource) as SectionReadOnlyList; }
    }

    #endregion

    private void selectAllButton_Click(object sender, EventArgs e)
    {
      SelectableSections.SelectAll();
    }

    private void inverseAllButton_Click(object sender, EventArgs e)
    {
      SelectableSections.InverseAll();
    }

    private void barManager_Saved(object sender, Phenix.Windows.BarItemSaveEventArgs e)
    {
       DataDictionaryHub.TableFilterInfoHasChanged();
    }
  }
}
