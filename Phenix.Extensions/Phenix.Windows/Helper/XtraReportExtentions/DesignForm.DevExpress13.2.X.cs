using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using DevExpress.XtraBars;
using DevExpress.XtraReports.UI;
using DevExpress.XtraReports.UserDesigner;
using DevExpress.XtraTreeList;
using DevExpress.XtraTreeList.Nodes;
using Phenix.Core.Windows;

namespace Phenix.Windows.Helper
{
  internal partial class DesignForm : XRDesignFormExBase
  {
    public DesignForm(XtraReport originalReport)
      : this(originalReport, null) { }

    public DesignForm(XtraReport originalReport, TemplateFile templateFile)
    {
      InitializeComponent();

      _originalReport = originalReport;

      ResetTemplates(templateFile);
    }
    
    #region 属性

    private readonly XtraReport _originalReport;

    private IList<TemplateFile> Templates
    {
      get { return BindingSourceHelper.GetDataSourceList(this.templateFilesBindingSource) as List<TemplateFile>; }
    }

    private TemplateFile WorkingTemplate
    {
      get { return BindingSourceHelper.GetDataSourceCurrent(this.templateFilesBindingSource) as TemplateFile; }
    }

    #endregion

    #region 方法

    private void ApplyRules()
    {
      this.copyTemplateBarButtonItem.Enabled = WorkingTemplate != null;
      this.deleteTemplateBarButtonItem.Enabled = WorkingTemplate != null && !WorkingTemplate.IsDefaultTemplate;
      this.changeTemplateNameBarButtonItem.Enabled = WorkingTemplate != null && !WorkingTemplate.IsDefaultTemplate;
    }

    private void ResetTemplates(TemplateFile workingTemplate)
    {
      this.templateFilesBindingSource.DataSource = TemplateFile.GetTemplates(_originalReport);
      this.templateFilesBindingSource.Position = workingTemplate != null ? this.templateFilesBindingSource.IndexOf(workingTemplate) : 0;
      ApplyRules();
    }

    private Exception CheckTemplateExists(string templateName)
    {
      if (Templates.Any(p => String.Compare(p.TemplateName, templateName, StringComparison.OrdinalIgnoreCase) == 0))
        return new ArgumentException(String.Format(Phenix.Windows.Properties.Resources.HaveTemplate, templateName));
      return null;
    }

    #endregion

    private void templateFilesTreeList_BeforeFocusNode(object sender, BeforeFocusNodeEventArgs e)
    {
      if (this.DesignPanel.ReportState == ReportState.Changed)
      {
        switch (MessageBox.Show(String.Format(Phenix.Windows.Properties.Resources.SwitchEditTemplateHint, WorkingTemplate.TemplateName),
          Phenix.Windows.Properties.Resources.SwitchEditTemplate, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question))
        {
          case DialogResult.Yes:
            this.Cursor = Cursors.WaitCursor;
            try
            {
              WorkingTemplate.Save();
            }
            finally
            {
              this.Cursor = Cursors.Default;
            }
            break;
          case DialogResult.Cancel:
            e.CanFocus = false;
            break;
        }
      }
    }

    private void templateFilesTreeList_AfterFocusNode(object sender, NodeEventArgs e)
    {
      this.Cursor = Cursors.WaitCursor;
      try
      {
        this.FileName = WorkingTemplate.Path;
        this.OpenReport(WorkingTemplate.Report);
        ApplyRules();
      }
      finally
      {
        this.Cursor = Cursors.Default;
      }
    }

    private void copyTemplateBarButtonItem_ItemClick(object sender, ItemClickEventArgs e)
    {
      string templateName = TemplateSetDialog.Execute(CheckTemplateExists);
      if (!String.IsNullOrEmpty(templateName))
      {
        TemplateFile templateFile = new TemplateFile(_originalReport, templateName);
        this.DesignPanel.SaveReport(templateFile.Path);
        ResetTemplates(templateFile);
      }
    }

    private void changeTemplateNameBarButtonItem_ItemClick(object sender, ItemClickEventArgs e)
    {
      string templateName = TemplateSetDialog.Execute(CheckTemplateExists);
      if (!String.IsNullOrEmpty(templateName))
      {
        File.Delete(WorkingTemplate.Path);
        TemplateFile templateFile = new TemplateFile(_originalReport, templateName);
        this.DesignPanel.SaveReport(templateFile.Path);
        ResetTemplates(templateFile);
      }
    }

    private void deleteTemplateBarButtonItem_ItemClick(object sender, ItemClickEventArgs e)
    {
      if (MessageBox.Show(String.Format(Phenix.Windows.Properties.Resources.DeleteTemplateHint, WorkingTemplate.TemplateName),
        Phenix.Windows.Properties.Resources.DeleteTemplate, MessageBoxButtons.YesNo) == DialogResult.Yes)
      {
        File.Delete(WorkingTemplate.Path);
        TreeListNode node = this.templateFilesTreeList.FocusedNode.NextNode ?? this.templateFilesTreeList.FocusedNode.PrevNode;
        if (node != null)
          this.templateFilesTreeList.SetFocusedNode(node);
        ResetTemplates(WorkingTemplate);
      }
    }
  }
}