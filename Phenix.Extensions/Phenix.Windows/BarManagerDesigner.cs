using System;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Drawing;
using System.Windows.Forms;
using DevExpress.XtraBars;
using Phenix.Core.Windows;
using Phenix.Services.Client.Library;
using Phenix.Services.Client.Validation;

namespace Phenix.Windows
{
  /// <summary>
  /// Bar管理器设计器
  /// </summary>
  internal class BarManagerDesigner : DevExpress.XtraBars.Design.BarManagerDesigner
  {
    #region 属性

    public ContainerControl RootComponent
    {
      get { return DesignerHost.RootComponent as ContainerControl; }
    }

    public BarManager BarManager
    {
      get { return Component as BarManager; }
    }

    public override DesignerVerbCollection Verbs
    {
      get
      {
        base.Verbs.Add(new DesignerVerb(Phenix.Windows.Properties.Resources.ResetBindingSources,
          new EventHandler(ResetBindingSources)));
        base.Verbs.Add(new DesignerVerb(Phenix.Services.Client.Properties.Resources.ResetEditFriendlyCaptions,
          new EventHandler(ResetEditFriendlyCaptions)));
        base.Verbs.Add(new DesignerVerb(Phenix.Services.Client.Properties.Resources.ResetGridFriendlyCaptions,
          new EventHandler(ResetGridFriendlyCaptions)));
        base.Verbs.Add(new DesignerVerb(Phenix.Services.Client.Properties.Resources.ResetGridFriendlyLayouts,
          new EventHandler(ResetGridFriendlyLayouts)));
        base.Verbs.Add(new DesignerVerb(Phenix.Services.Client.Properties.Resources.ResetFormRules,
          new EventHandler(ResetFormRules)));
        return base.Verbs;
      }
    }

    private const string HINT = "根据 BindingSources 属性内容";

    #endregion

    #region 事件

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Globalization", "CA1303:DoNotPassLiteralsAsLocalizedParameters")]
    private void ResetBindingSources(object sender, EventArgs e)
    {
      bool succeed;
      using (new DevExpress.Utils.WaitDialogForm(Phenix.Windows.Properties.Resources.ResetBindingSources, Phenix.Core.Properties.Resources.PleaseWait))
      {
        succeed = BarManager.ResetBindingSources(DesignerHost);
      }
      MessageBox.Show(succeed ? Phenix.Windows.Properties.Resources.ResetBindingSourcesSucceed : Phenix.Windows.Properties.Resources.ResetBindingSourcesUnchanged,
        Phenix.Windows.Properties.Resources.ResetBindingSources, MessageBoxButtons.OK, MessageBoxIcon.Information);
    }

    private void ResetEditFriendlyCaptions(object sender, EventArgs e)
    {
      Initialize();
      //重置
      using (new DevExpress.Utils.WaitDialogForm(Phenix.Services.Client.Properties.Resources.ResetEditFriendlyCaptions, Phenix.Core.Properties.Resources.PleaseWait))
      {
        RootComponent.SuspendLayout();
        RaiseComponentChanging(null);
        BarManager.ResetEditFriendlyCaptions(DesignerHost);
        RaiseComponentChanged(null, null, null);
        RootComponent.ResumeLayout(false);
      }
      //提示
      MessageBox.Show(HINT + Phenix.Services.Client.Properties.Resources.ResetEditFriendlyCaptionsSucceed,
        Phenix.Services.Client.Properties.Resources.ResetEditFriendlyCaptions, MessageBoxButtons.OK, MessageBoxIcon.Information);
    }

    private void ResetGridFriendlyCaptions(object sender, EventArgs e)
    {
      Initialize();
      //重置
      using (new DevExpress.Utils.WaitDialogForm(Phenix.Services.Client.Properties.Resources.ResetGridFriendlyCaptions, Phenix.Core.Properties.Resources.PleaseWait))
      {
        RootComponent.SuspendLayout();
        RaiseComponentChanging(null);
        BarManager.ResetGridFriendlyCaptions(RootComponent);
        RaiseComponentChanged(null, null, null);
        RootComponent.ResumeLayout(false);
      }
      //提示
      MessageBox.Show(Phenix.Services.Client.Properties.Resources.ResetGridFriendlyCaptionsSucceed,
        Phenix.Services.Client.Properties.Resources.ResetGridFriendlyCaptions, MessageBoxButtons.OK, MessageBoxIcon.Information);
    }

    private void ResetGridFriendlyLayouts(object sender, EventArgs e)
    {
      Initialize();
      bool succeed;
      //重置
      using (new DevExpress.Utils.WaitDialogForm(Phenix.Services.Client.Properties.Resources.ResetGridFriendlyLayouts, Phenix.Core.Properties.Resources.PleaseWait))
      {
        RootComponent.SuspendLayout();
        RaiseComponentChanging(null);
        succeed = EditValidation.ResetGridFriendlyLayouts(RootComponent);
        RaiseComponentChanged(null, null, null);
        RootComponent.ResumeLayout(false);
      }
      //提示
      if (succeed)
        MessageBox.Show(Phenix.Services.Client.Properties.Resources.ResetGridFriendlyLayoutsSucceed,
          Phenix.Services.Client.Properties.Resources.ResetGridFriendlyLayouts, MessageBoxButtons.OK, MessageBoxIcon.Information);
      else
        MessageBox.Show(Phenix.Services.Client.Properties.Resources.ResetGridFriendlyLayoutsAbort,
          Phenix.Services.Client.Properties.Resources.ResetGridFriendlyLayouts, MessageBoxButtons.OK, MessageBoxIcon.Warning);
    }

    private void ResetFormRules(object sender, EventArgs e)
    {
      Initialize();
      //重置
      using (new DevExpress.Utils.WaitDialogForm(Phenix.Services.Client.Properties.Resources.ResetFormRules, Phenix.Core.Properties.Resources.PleaseWait))
      {
        RootComponent.SuspendLayout();
        RaiseComponentChanging(null);
        ShowMessageDialog.Execute(Phenix.Services.Client.Properties.Resources.ResetFormRules,
          Phenix.Services.Client.Properties.Resources.ResetFormRulesSucceed + Environment.NewLine + BarManager.ResetFormRules(DesignerHost));
        RaiseComponentChanged(null, null, null);
        RootComponent.ResumeLayout(false);
      }
    }

    #endregion

    #region 方法

    private static void Initialize()
    {
      Registration.RegisterEmbeddedWorker(false);
    }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Globalization", "CA1303:DoNotPassLiteralsAsLocalizedParameters")]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Reliability", "CA2000:DisposeObjectsBeforeLosingScope")]
    public override void InitializeNewComponent(System.Collections.IDictionary defaultValues)
    {
      base.InitializeNewComponent(defaultValues);

      if (DesignerHost == null)
        return;

      Phenix.Windows.Helper.DesignerHelper.ReferenceAssembly(this.GetService(typeof(ITypeResolutionService)) as ITypeResolutionService);

      if (BarManager.Bars != null)
        BarManager.Bars.Clear();

      foreach (IComponent item in DesignerHost.Container.Components)
        if (item is Bar)
          DesignerHost.Container.Remove(item);

      RootComponent.SuspendLayout();
      BarManager.BeginInit();

      Bar toolsBar = new Bar(BarManager);
      toolsBar.BarName = "Tools Bar";
      toolsBar.DockStyle = BarDockStyle.Top;
      toolsBar.Text = "Tools Bar";
      BarManager.Bars.Add(toolsBar);

      Bar statusBar = new Bar(BarManager);
      statusBar.BarName = "Status Bar";
      statusBar.CanDockStyle = BarCanDockStyle.Bottom;
      statusBar.DockStyle = BarDockStyle.Bottom;
      statusBar.Text = "Status Bar";
      BarManager.StatusBar = statusBar;
      BarManager.Bars.Add(statusBar);

      BarButtonItem bbiFetch = new BarButtonItem();
      bbiFetch.Caption = Phenix.Windows.Properties.Resources.ToolBarFetchHeaderTip;
      bbiFetch.Id = Convert.ToInt32(BarItemId.Fetch);
      bbiFetch.ItemShortcut = new BarShortcut(Keys.F3);
      bbiFetch.ImageIndex = (int)BarItemId.Fetch - (int)BarItemId.Fetch;
      bbiFetch.Name = "bbiFetch";
      bbiFetch.PaintStyle = BarItemPaintStyle.CaptionGlyph;
      bbiFetch.SuperTip = BarManager.CreateToolTip(
        Phenix.Windows.Properties.Resources.ToolBarFetchHeaderTip, 
        Phenix.Windows.Properties.Resources.ToolBarFetchBodyTip, bbiFetch.ItemShortcut.ToString());

      BarButtonItem bbiReset = new BarButtonItem();
      bbiReset.Caption = Phenix.Windows.Properties.Resources.ToolBarResetHeaderTip;
      bbiReset.Id = Convert.ToInt32(BarItemId.Reset);
      bbiReset.ItemShortcut = new BarShortcut(Keys.F5);
      bbiReset.ImageIndex = (int)BarItemId.Reset - (int)BarItemId.Fetch;
      bbiReset.Name = "bbiReset";
      bbiReset.PaintStyle = BarItemPaintStyle.CaptionGlyph;
      bbiReset.SuperTip = BarManager.CreateToolTip(
        Phenix.Windows.Properties.Resources.ToolBarResetHeaderTip, 
        Phenix.Windows.Properties.Resources.ToolBarResetBodyTip, bbiReset.ItemShortcut.ToString());
      bbiReset.Visibility = BarItemVisibility.Never;

      BarButtonItem bbiRestore = new BarButtonItem();
      bbiRestore.Caption = Phenix.Windows.Properties.Resources.ToolBarRestoreHeaderTip;
      bbiRestore.Id = Convert.ToInt32(BarItemId.Restore); ;
      bbiRestore.ItemShortcut = new BarShortcut((Keys.Control | Keys.R));
      bbiRestore.ImageIndex = (int)BarItemId.Restore - (int)BarItemId.Fetch;
      bbiRestore.Name = "bbiRestore";
      bbiRestore.PaintStyle = BarItemPaintStyle.CaptionGlyph;
      bbiRestore.SuperTip = BarManager.CreateToolTip(
        Phenix.Windows.Properties.Resources.ToolBarRestoreHeaderTip,
        Phenix.Windows.Properties.Resources.ToolBarRestoreBodyTip, bbiRestore.ItemShortcut.ToString());
      bbiRestore.Visibility = BarItemVisibility.Never;

      BarButtonItem bbiAdd = new BarButtonItem();
      bbiAdd.Caption = Phenix.Windows.Properties.Resources.ToolBarAddHeaderTip;
      bbiAdd.Id = Convert.ToInt32(BarItemId.Add);
      bbiAdd.ItemShortcut = new BarShortcut(Keys.F6);
      bbiAdd.ImageIndex = (int)BarItemId.Add - (int)BarItemId.Fetch;
      bbiAdd.Name = "bbiAdd";
      bbiAdd.PaintStyle = BarItemPaintStyle.CaptionGlyph;
      bbiAdd.SuperTip = BarManager.CreateToolTip(
        Phenix.Windows.Properties.Resources.ToolBarAddHeaderTip,
        Phenix.Windows.Properties.Resources.ToolBarAddBodyTip, bbiAdd.ItemShortcut.ToString());
      bbiAdd.Enabled = false;

      BarButtonItem bbiAddClone = new BarButtonItem();
      bbiAddClone.Caption = Phenix.Windows.Properties.Resources.ToolBarAddCloneHeaderTip;
      bbiAddClone.Id = Convert.ToInt32(BarItemId.AddClone);
      bbiAddClone.ItemShortcut = new BarShortcut(Keys.Control | Keys.F6);
      bbiAddClone.ImageIndex = (int)BarItemId.AddClone - (int)BarItemId.Fetch;
      bbiAddClone.Name = "bbiAddClone";
      bbiAddClone.PaintStyle = BarItemPaintStyle.CaptionGlyph;
      bbiAddClone.SuperTip = BarManager.CreateToolTip(
        Phenix.Windows.Properties.Resources.ToolBarAddCloneHeaderTip,
        Phenix.Windows.Properties.Resources.ToolBarAddCloneBodyTip, bbiAddClone.ItemShortcut.ToString());
      bbiAddClone.Visibility = BarItemVisibility.Never;

      BarButtonItem bbiModify = new BarButtonItem();
      bbiModify.Caption = Phenix.Windows.Properties.Resources.ToolBarModifyHeaderTip;
      bbiModify.Id = Convert.ToInt32(BarItemId.Modify);
      bbiModify.ItemShortcut = new BarShortcut(Keys.F2);
      bbiModify.ImageIndex = (int)BarItemId.Modify - (int)BarItemId.Fetch;
      bbiModify.Name = "bbiModify";
      bbiModify.PaintStyle = BarItemPaintStyle.CaptionGlyph;
      bbiModify.SuperTip = BarManager.CreateToolTip(
        Phenix.Windows.Properties.Resources.ToolBarModifyHeaderTip,
        Phenix.Windows.Properties.Resources.ToolBarModifyBodyTip, bbiModify.ItemShortcut.ToString());
      bbiModify.Enabled = false;

      BarButtonItem bbiDelete = new BarButtonItem();
      bbiDelete.Caption = Phenix.Windows.Properties.Resources.ToolBarDeleteHeaderTip;
      bbiDelete.Id = Convert.ToInt32(BarItemId.Delete);
      bbiDelete.ItemShortcut = new BarShortcut((Keys.Shift | Keys.Delete));
      bbiDelete.ImageIndex = (int)BarItemId.Delete - (int)BarItemId.Fetch;
      bbiDelete.Name = "bbiDelete";
      bbiDelete.PaintStyle = BarItemPaintStyle.CaptionGlyph;
      bbiDelete.SuperTip = BarManager.CreateToolTip(
        Phenix.Windows.Properties.Resources.ToolBarDeleteHeaderTip,
        Phenix.Windows.Properties.Resources.ToolBarDeleteBodyTip, bbiDelete.ItemShortcut.ToString());
      bbiDelete.Enabled = false;

      BarButtonItem bbiLocate = new BarButtonItem();
      bbiLocate.Caption = Phenix.Windows.Properties.Resources.ToolBarLocateHeaderTip;
      bbiLocate.Id = Convert.ToInt32(BarItemId.Locate);
      bbiLocate.ItemShortcut = new BarShortcut((Keys.Control | Keys.Home));
      bbiLocate.ImageIndex = (int)BarItemId.Locate - (int)BarItemId.Fetch;
      bbiLocate.Name = "bbiLocate";
      bbiLocate.PaintStyle = BarItemPaintStyle.CaptionGlyph;
      bbiLocate.SuperTip = BarManager.CreateToolTip(
        Phenix.Windows.Properties.Resources.ToolBarLocateHeaderTip,
        Phenix.Windows.Properties.Resources.ToolBarLocateBodyTip, bbiLocate.ItemShortcut.ToString());
      bbiLocate.Enabled = false;

      BarButtonItem bbiCancel = new BarButtonItem();
      bbiCancel.Caption = Phenix.Windows.Properties.Resources.ToolBarCancelHeaderTip;
      bbiCancel.Id = Convert.ToInt32(BarItemId.Cancel); ;
      bbiCancel.ItemShortcut = new BarShortcut((Keys.Control | Keys.Z));
      bbiCancel.ImageIndex = (int)BarItemId.Cancel - (int)BarItemId.Fetch;
      bbiCancel.Name = "bbiCancel";
      bbiCancel.PaintStyle = BarItemPaintStyle.CaptionGlyph;
      bbiCancel.SuperTip = BarManager.CreateToolTip(
        Phenix.Windows.Properties.Resources.ToolBarCancelHeaderTip,
        Phenix.Windows.Properties.Resources.ToolBarCancelBodyTip, bbiCancel.ItemShortcut.ToString());
      bbiCancel.Enabled = false;

      BarButtonItem bbiSave = new BarButtonItem();
      bbiSave.Caption = Phenix.Windows.Properties.Resources.ToolBarSaveHeaderTip;
      bbiSave.Id = Convert.ToInt32(BarItemId.Save);
      bbiSave.ItemShortcut = new BarShortcut(Keys.F12);
      bbiSave.ImageIndex = (int)BarItemId.Save - (int)BarItemId.Fetch;
      bbiSave.Name = "bbiSave";
      bbiSave.PaintStyle = BarItemPaintStyle.CaptionGlyph;
      bbiSave.SuperTip = BarManager.CreateToolTip(
        Phenix.Windows.Properties.Resources.ToolBarSaveHeaderTip,
        Phenix.Windows.Properties.Resources.ToolBarSaveBodyTip, bbiSave.ItemShortcut.ToString());
      bbiSave.Enabled = false;

      BarButtonItem bbiImport = new BarButtonItem();
      bbiImport.Caption = Phenix.Windows.Properties.Resources.ToolBarImportHeaderTip;
      bbiImport.Id = Convert.ToInt32(BarItemId.Import);
      bbiImport.ItemShortcut = new BarShortcut(Keys.F8);
      bbiImport.ImageIndex = (int)BarItemId.Import - (int)BarItemId.Fetch;
      bbiImport.Name = "bbiImport";
      bbiImport.PaintStyle = BarItemPaintStyle.CaptionGlyph;
      bbiImport.SuperTip = BarManager.CreateToolTip(
        Phenix.Windows.Properties.Resources.ToolBarImportHeaderTip,
        Phenix.Windows.Properties.Resources.ToolBarImportBodyTip, bbiImport.ItemShortcut.ToString());
      bbiImport.Visibility = BarItemVisibility.Never;

      BarButtonItem bbiExport = new BarButtonItem();
      bbiExport.Caption = Phenix.Windows.Properties.Resources.ToolBarExportHeaderTip;
      bbiExport.Id = Convert.ToInt32(BarItemId.Export);
      bbiExport.ItemShortcut = new BarShortcut(Keys.F9);
      bbiExport.ImageIndex = (int)BarItemId.Export - (int)BarItemId.Fetch;
      bbiExport.Name = "bbiExport";
      bbiExport.PaintStyle = BarItemPaintStyle.CaptionGlyph;
      bbiExport.SuperTip = BarManager.CreateToolTip(
        Phenix.Windows.Properties.Resources.ToolBarExportHeaderTip,
        Phenix.Windows.Properties.Resources.ToolBarExportBodyTip, bbiExport.ItemShortcut.ToString());
      bbiExport.Enabled = false;

      BarButtonItem bbiPrint = new BarButtonItem();
      bbiPrint.Caption = Phenix.Windows.Properties.Resources.ToolBarPrintHeaderTip;
      bbiPrint.Id = Convert.ToInt32(BarItemId.Print);
      bbiPrint.ItemShortcut = new BarShortcut(Keys.F11);
      bbiPrint.ImageIndex = (int)BarItemId.Print - (int)BarItemId.Fetch;
      bbiPrint.Name = "bbiPrint";
      bbiPrint.PaintStyle = BarItemPaintStyle.CaptionGlyph;
      bbiPrint.SuperTip = BarManager.CreateToolTip(
        Phenix.Windows.Properties.Resources.ToolBarPrintHeaderTip,
        Phenix.Windows.Properties.Resources.ToolBarPrintBodyTip, bbiPrint.ItemShortcut.ToString());
      bbiPrint.Enabled = false;

      BarButtonItem bbiHelp = new BarButtonItem();
      bbiHelp.Caption = Phenix.Windows.Properties.Resources.ToolBarHelpHeaderTip;
      bbiHelp.Id = Convert.ToInt32(BarItemId.Help);
      bbiHelp.ItemShortcut = new BarShortcut(Keys.F1);
      bbiHelp.ImageIndex = (int)BarItemId.Help - (int)BarItemId.Fetch;
      bbiHelp.Name = "bbiHelp";
      bbiHelp.PaintStyle = BarItemPaintStyle.CaptionGlyph;
      bbiHelp.SuperTip = BarManager.CreateToolTip(
        Phenix.Windows.Properties.Resources.ToolBarHelpHeaderTip,
        Phenix.Windows.Properties.Resources.ToolBarHelpBodyTip, bbiHelp.ItemShortcut.ToString());
      bbiHelp.Visibility = BarItemVisibility.Never;

      BarButtonItem bbiSetup = new BarButtonItem();
      bbiSetup.Caption = Phenix.Windows.Properties.Resources.ToolBarSetupHeaderTip;
      bbiSetup.Id = Convert.ToInt32(BarItemId.Setup);
      bbiSetup.ItemShortcut = new BarShortcut(Keys.F10);
      bbiSetup.ImageIndex = (int)BarItemId.Setup - (int)BarItemId.Fetch;
      bbiSetup.Name = "bbiSetup";
      bbiSetup.PaintStyle = BarItemPaintStyle.CaptionGlyph;
      bbiSetup.SuperTip = BarManager.CreateToolTip(
        Phenix.Windows.Properties.Resources.ToolBarSetupHeaderTip,
        Phenix.Windows.Properties.Resources.ToolBarSetupBodyTip, bbiSetup.ItemShortcut.ToString());
      bbiSetup.Visibility = BarItemVisibility.Never;

      BarButtonItem bbiExit = new BarButtonItem();
      bbiExit.Caption = Phenix.Windows.Properties.Resources.ToolBarExitHeaderTip;
      bbiExit.Id = Convert.ToInt32(BarItemId.Exit);
      bbiExit.ItemShortcut = new BarShortcut(Keys.F4);
      bbiExit.ImageIndex = (int)BarItemId.Exit - (int)BarItemId.Fetch;
      bbiExit.Name = "bbiExit";
      bbiExit.PaintStyle = BarItemPaintStyle.CaptionGlyph;
      bbiExit.SuperTip = BarManager.CreateToolTip(
        Phenix.Windows.Properties.Resources.ToolBarExitHeaderTip,
        Phenix.Windows.Properties.Resources.ToolBarExitBodyTip, bbiExit.ItemShortcut.ToString());

      toolsBar.LinksPersistInfo.AddRange(new LinkPersistInfo[] {
        new LinkPersistInfo(bbiFetch),
        new LinkPersistInfo(bbiReset),
        new LinkPersistInfo(bbiRestore),
        new LinkPersistInfo(bbiAdd, true),
        new LinkPersistInfo(bbiAddClone),
        new LinkPersistInfo(bbiModify),
        new LinkPersistInfo(bbiDelete),
        new LinkPersistInfo(bbiLocate),
        new LinkPersistInfo(bbiCancel),
        new LinkPersistInfo(bbiSave),
        new LinkPersistInfo(bbiImport, true),
        new LinkPersistInfo(bbiExport),
        new LinkPersistInfo(bbiPrint),
        new LinkPersistInfo(bbiHelp, true),
        new LinkPersistInfo(bbiSetup),
        new LinkPersistInfo(bbiExit)});

      BarStaticItem bsiState = new BarStaticItem();
      bsiState.Id = Convert.ToInt32(BarItemId.DataOperateState);
      bsiState.Name = "bsiState";
      bsiState.TextAlignment = StringAlignment.Center;

      BarStaticItem bsiRecord = new BarStaticItem();
      bsiRecord.Id = Convert.ToInt32(BarItemId.DataRecordState);
      bsiRecord.Name = "bsiRecord";
      bsiRecord.TextAlignment = StringAlignment.Center;

      BarStaticItem bsiHint = new BarStaticItem();
      bsiHint.AutoSize = BarStaticItemSize.Spring;
      bsiHint.Id = Convert.ToInt32(BarItemId.Hint);
      bsiHint.Name = "bsiHint";
      bsiHint.TextAlignment = StringAlignment.Near;

      statusBar.LinksPersistInfo.AddRange(new LinkPersistInfo[] {
        new LinkPersistInfo(BarLinkUserDefines.PaintStyle, bsiState, BarItemPaintStyle.CaptionGlyph),
        new LinkPersistInfo(bsiRecord),
        new LinkPersistInfo(bsiHint)});

      DesignerHost.Container.Add(bbiFetch, bbiFetch.Name);
      DesignerHost.Container.Add(bbiReset, bbiReset.Name);
      DesignerHost.Container.Add(bbiRestore, bbiRestore.Name);
      DesignerHost.Container.Add(bbiAdd, bbiAdd.Name);
      DesignerHost.Container.Add(bbiAddClone, bbiAddClone.Name);
      DesignerHost.Container.Add(bbiModify, bbiModify.Name);
      DesignerHost.Container.Add(bbiDelete, bbiDelete.Name);
      DesignerHost.Container.Add(bbiLocate, bbiLocate.Name);
      DesignerHost.Container.Add(bbiCancel, bbiCancel.Name);
      DesignerHost.Container.Add(bbiSave, bbiSave.Name);
      DesignerHost.Container.Add(bbiImport, bbiImport.Name);
      DesignerHost.Container.Add(bbiExport, bbiExport.Name);
      DesignerHost.Container.Add(bbiPrint, bbiPrint.Name);
      DesignerHost.Container.Add(bbiHelp, bbiHelp.Name);
      DesignerHost.Container.Add(bbiSetup, bbiSetup.Name);
      DesignerHost.Container.Add(bbiExit, bbiExit.Name);

      DesignerHost.Container.Add(bsiState, bsiState.Name);
      DesignerHost.Container.Add(bsiRecord, bsiRecord.Name);
      DesignerHost.Container.Add(bsiHint, bsiHint.Name);

      DesignerHost.Container.Add(toolsBar, "barTools");
      DesignerHost.Container.Add(statusBar, "barStatus");

      BarManager.EndInit();
      BarManager.ForceInitialize();
      RootComponent.ResumeLayout();
    }

    #endregion
  }
}