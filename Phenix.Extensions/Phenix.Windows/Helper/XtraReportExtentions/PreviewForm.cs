using System;
using System.Collections.Generic;
using System.Linq;
using DevExpress.XtraBars;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraPrinting.Preview;
using DevExpress.XtraReports.UI;

namespace Phenix.Windows.Helper
{
  internal partial class PreviewForm : PrintPreviewFormEx
  {
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Reliability", "CA2000:DisposeObjectsBeforeLosingScope")]
    public PreviewForm(XtraReport report)
    {
      InitializeComponent();

      RepositoryItemLookUpEdit templatesRepositoryItemLookUpEdit = new RepositoryItemLookUpEdit();
      templatesRepositoryItemLookUpEdit.DisplayMember = "TemplateName";
      templatesRepositoryItemLookUpEdit.ValueMember = "TemplateName";
      templatesRepositoryItemLookUpEdit.Columns.Add(new LookUpColumnInfo("TemplateName"));
      templatesRepositoryItemLookUpEdit.DataSource = templateFilesBindingSource;
      templatesRepositoryItemLookUpEdit.ImmediatePopup = true;
      templatesRepositoryItemLookUpEdit.NullText = String.Empty;
      templatesRepositoryItemLookUpEdit.TextEditStyle = TextEditStyles.Standard;
      templatesRepositoryItemLookUpEdit.ShowHeader = false;
      templatesRepositoryItemLookUpEdit.BestFit();

      BarEditItem templatesBarEditItem = new BarEditItem(PrintBarManager, templatesRepositoryItemLookUpEdit);
      templatesBarEditItem.Width = 220;
      templatesBarEditItem.Caption = Phenix.Windows.Properties.Resources.Template;
      templatesBarEditItem.PaintStyle = BarItemPaintStyle.CaptionGlyph;
      templatesBarEditItem.EditValueChanged += new EventHandler(selectTemplateBarEditItem_EditValueChanged);

      BarButtonItem editTemplateBarButtonItem = new BarButtonItem();
      editTemplateBarButtonItem.Caption = Phenix.Windows.Properties.Resources.EditTemplate;
      editTemplateBarButtonItem.ItemClick += new ItemClickEventHandler(editTemplateBarButtonItem_ItemClick);

      BarButtonItem resetTemplateBarButtonItem = new BarButtonItem();
      resetTemplateBarButtonItem.Caption = Phenix.Windows.Properties.Resources.ResetTemplate;
      resetTemplateBarButtonItem.ItemClick += new ItemClickEventHandler(resetTemplateBarButtonItem_ItemClick);

      PrintBarManager.Items.Add(templatesBarEditItem);
      PrintBarManager.Items.Add(editTemplateBarButtonItem);
      PrintBarManager.Items.Add(resetTemplateBarButtonItem);
      PrintBarManager.MainMenu.AddItems(new BarItem[] { templatesBarEditItem, editTemplateBarButtonItem, resetTemplateBarButtonItem });
      PrintBarManager.MainMenu.LinksPersistInfo.AddRange(new LinkPersistInfo[]
      {
        new LinkPersistInfo(BarLinkUserDefines.PaintStyle, templatesBarEditItem, "", true, true, true, 0, null, BarItemPaintStyle.CaptionGlyph),
        new LinkPersistInfo(editTemplateBarButtonItem),
        new LinkPersistInfo(resetTemplateBarButtonItem)
      });

      _originalReport = report;

      ResetTemplates();
    }

    #region 属性

    private readonly XtraReport _originalReport;
    private TemplateFile _workingTemplate;

    #endregion

    #region 方法

    private void ResetTemplates()
    {
      this.templateFilesBindingSource.DataSource = TemplateFile.GetTemplates(_originalReport);
      if (_workingTemplate == null)
        _workingTemplate = new TemplateFile(_originalReport);
      ShowReport(_workingTemplate.TemplateName);
    }

    private bool TemplateExists(string templateName)
    {
      return ((List<TemplateFile>)this.templateFilesBindingSource.List).Any(p => String.Compare(p.TemplateName, templateName, StringComparison.OrdinalIgnoreCase) == 0);
    }

    private void ShowReport(string templateName)
    {
      if (TemplateExists(templateName))
      {
        _workingTemplate = new TemplateFile(_originalReport, templateName);
        this.PrintControl.PrintingSystem = _workingTemplate.Report.PrintingSystem;
        _workingTemplate.Report.DataSource = _originalReport.DataSource;
        _workingTemplate.Report.CreateDocument();
      }
    }

    #endregion

    private void selectTemplateBarEditItem_EditValueChanged(object sender, EventArgs e)
    {
      ShowReport(((BarEditItem)sender).EditValue as string);
    }

    private void editTemplateBarButtonItem_ItemClick(object sender, ItemClickEventArgs e)
    {
      using (DesignForm designForm = new DesignForm(_originalReport, _workingTemplate))
      {
        designForm.ShowDialog();
      }
      ResetTemplates();
    }

    private void resetTemplateBarButtonItem_ItemClick(object sender, ItemClickEventArgs e)
    {
      ResetTemplates();
    }
  }
}