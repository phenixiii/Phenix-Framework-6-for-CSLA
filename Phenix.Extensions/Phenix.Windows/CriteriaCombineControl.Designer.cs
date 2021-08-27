namespace Phenix.Windows
{
  partial class CriteriaCombineControl
  {
    /// <summary> 
    /// Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    /// <summary> 
    /// Clean up any resources being used.
    /// </summary>
    /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
    protected override void Dispose(bool disposing)
    {
      if (disposing && (components != null))
      {
        components.Dispose();
      }
      base.Dispose(disposing);
    }

    #region Component Designer generated code

    /// <summary> 
    /// Required method for Designer support - do not modify 
    /// the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
      this.components = new System.ComponentModel.Container();
      this.barManager = new DevExpress.XtraBars.BarManager(this.components);
      this.menuBar = new DevExpress.XtraBars.Bar();
      this.criteriaExpressionLookUpBarEditItem = new DevExpress.XtraBars.BarEditItem();
      this.criteriaExpressionLookUpEdit = new DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit();
      this.criteriaExpressionKeyCaptionCollectionBindingSource = new System.Windows.Forms.BindingSource(this.components);
      this.criteriaExpressionNewBarButtonItem = new DevExpress.XtraBars.BarButtonItem();
      this.criteriaExpressionDeleteBarButtonItem = new DevExpress.XtraBars.BarButtonItem();
      this.criteriaExpressionUpdateBarButtonItem = new DevExpress.XtraBars.BarButtonItem();
      this.addGroupBarButtonItem = new DevExpress.XtraBars.BarButtonItem();
      this.addBarButtonItem = new DevExpress.XtraBars.BarButtonItem();
      this.deleteGroupBarButtonItem = new DevExpress.XtraBars.BarButtonItem();
      this.barDockControlTop = new DevExpress.XtraBars.BarDockControl();
      this.barDockControlBottom = new DevExpress.XtraBars.BarDockControl();
      this.barDockControlLeft = new DevExpress.XtraBars.BarDockControl();
      this.barDockControlRight = new DevExpress.XtraBars.BarDockControl();
      this.criteriaExpressionNavBarControl = new DevExpress.XtraNavBar.NavBarControl();
      this.editValidation = new Phenix.Services.Client.Validation.EditValidation(this.components);
      ((System.ComponentModel.ISupportInitialize)(this.barManager)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.criteriaExpressionLookUpEdit)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.criteriaExpressionKeyCaptionCollectionBindingSource)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.criteriaExpressionNavBarControl)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.editValidation)).BeginInit();
      this.SuspendLayout();
      // 
      // barManager
      // 
      this.barManager.Bars.AddRange(new DevExpress.XtraBars.Bar[] {
            this.menuBar});
      this.barManager.DockControls.Add(this.barDockControlTop);
      this.barManager.DockControls.Add(this.barDockControlBottom);
      this.barManager.DockControls.Add(this.barDockControlLeft);
      this.barManager.DockControls.Add(this.barDockControlRight);
      this.barManager.Form = this;
      this.barManager.Items.AddRange(new DevExpress.XtraBars.BarItem[] {
            this.criteriaExpressionLookUpBarEditItem,
            this.criteriaExpressionUpdateBarButtonItem,
            this.criteriaExpressionDeleteBarButtonItem,
            this.addGroupBarButtonItem,
            this.deleteGroupBarButtonItem,
            this.criteriaExpressionNewBarButtonItem,
            this.addBarButtonItem});
      this.barManager.MainMenu = this.menuBar;
      this.barManager.MaxItemId = 14;
      this.barManager.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.criteriaExpressionLookUpEdit});
      // 
      // menuBar
      // 
      this.menuBar.BarName = "Menu";
      this.menuBar.DockCol = 0;
      this.menuBar.DockRow = 0;
      this.menuBar.DockStyle = DevExpress.XtraBars.BarDockStyle.Top;
      this.menuBar.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[] {
            new DevExpress.XtraBars.LinkPersistInfo(DevExpress.XtraBars.BarLinkUserDefines.Width, this.criteriaExpressionLookUpBarEditItem, "", false, true, true, 252),
            new DevExpress.XtraBars.LinkPersistInfo(this.criteriaExpressionNewBarButtonItem),
            new DevExpress.XtraBars.LinkPersistInfo(this.criteriaExpressionDeleteBarButtonItem),
            new DevExpress.XtraBars.LinkPersistInfo(this.criteriaExpressionUpdateBarButtonItem),
            new DevExpress.XtraBars.LinkPersistInfo(this.addGroupBarButtonItem, true),
            new DevExpress.XtraBars.LinkPersistInfo(this.addBarButtonItem),
            new DevExpress.XtraBars.LinkPersistInfo(this.deleteGroupBarButtonItem)});
      this.menuBar.OptionsBar.AllowQuickCustomization = false;
      this.menuBar.OptionsBar.DisableClose = true;
      this.menuBar.OptionsBar.DisableCustomization = true;
      this.menuBar.OptionsBar.DrawDragBorder = false;
      this.menuBar.OptionsBar.MultiLine = true;
      this.menuBar.OptionsBar.UseWholeRow = true;
      this.menuBar.Text = "Menu";
      // 
      // criteriaExpressionLookUpBarEditItem
      // 
      this.criteriaExpressionLookUpBarEditItem.Edit = this.criteriaExpressionLookUpEdit;
      this.criteriaExpressionLookUpBarEditItem.Id = 1;
      this.criteriaExpressionLookUpBarEditItem.Name = "criteriaExpressionLookUpBarEditItem";
      this.criteriaExpressionLookUpBarEditItem.EditValueChanged += new System.EventHandler(this.criteriaExpressionLookUpBarEditItem_EditValueChanged);
      // 
      // criteriaExpressionLookUpEdit
      // 
      this.criteriaExpressionLookUpEdit.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
      this.criteriaExpressionLookUpEdit.Columns.AddRange(new DevExpress.XtraEditors.Controls.LookUpColumnInfo[] {
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("Caption", "标签", 51, DevExpress.Utils.FormatType.None, "", true, DevExpress.Utils.HorzAlignment.Near),
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("Addtime", "添加时间", 56, DevExpress.Utils.FormatType.DateTime, "yyyy-MM-dd HH:mm:ss", true, DevExpress.Utils.HorzAlignment.Near),
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("UserNumber", "工号", 81, DevExpress.Utils.FormatType.None, "", true, DevExpress.Utils.HorzAlignment.Near)});
      this.criteriaExpressionLookUpEdit.DataSource = this.criteriaExpressionKeyCaptionCollectionBindingSource;
      this.criteriaExpressionLookUpEdit.DisplayMember = "Caption";
      this.criteriaExpressionLookUpEdit.Name = "criteriaExpressionLookUpEdit";
      this.criteriaExpressionLookUpEdit.NullText = "";
      this.criteriaExpressionLookUpEdit.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.Standard;
      this.criteriaExpressionLookUpEdit.ValueMember = "Value";
      // 
      // criteriaExpressionKeyCaptionCollectionBindingSource
      // 
      this.criteriaExpressionKeyCaptionCollectionBindingSource.DataSource = typeof(Phenix.Core.Rule.CriteriaExpressionKeyCaptionCollection);
      // 
      // criteriaExpressionNewBarButtonItem
      // 
      this.criteriaExpressionNewBarButtonItem.Caption = "新增";
      this.criteriaExpressionNewBarButtonItem.Id = 10;
      this.criteriaExpressionNewBarButtonItem.Name = "criteriaExpressionNewBarButtonItem";
      this.criteriaExpressionNewBarButtonItem.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.criteriaExpressionNewBarButtonItem_ItemClick);
      // 
      // criteriaExpressionDeleteBarButtonItem
      // 
      this.criteriaExpressionDeleteBarButtonItem.Caption = "删除";
      this.criteriaExpressionDeleteBarButtonItem.Id = 4;
      this.criteriaExpressionDeleteBarButtonItem.Name = "criteriaExpressionDeleteBarButtonItem";
      this.criteriaExpressionDeleteBarButtonItem.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.criteriaExpressionDeleteBarButtonItem_ItemClick);
      // 
      // criteriaExpressionUpdateBarButtonItem
      // 
      this.criteriaExpressionUpdateBarButtonItem.Caption = "保存";
      this.criteriaExpressionUpdateBarButtonItem.Id = 3;
      this.criteriaExpressionUpdateBarButtonItem.Name = "criteriaExpressionUpdateBarButtonItem";
      this.criteriaExpressionUpdateBarButtonItem.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.criteriaExpressionUpdateBarButtonItem_ItemClick);
      // 
      // addGroupBarButtonItem
      // 
      this.addGroupBarButtonItem.Caption = "++";
      this.addGroupBarButtonItem.Id = 8;
      this.addGroupBarButtonItem.Name = "addGroupBarButtonItem";
      this.addGroupBarButtonItem.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.addGroupBarButtonItem_ItemClick);
      // 
      // addBarButtonItem
      // 
      this.addBarButtonItem.Caption = "+";
      this.addBarButtonItem.Id = 13;
      this.addBarButtonItem.Name = "addBarButtonItem";
      this.addBarButtonItem.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.addBarButtonItem_ItemClick);
      // 
      // deleteGroupBarButtonItem
      // 
      this.deleteGroupBarButtonItem.Caption = "--";
      this.deleteGroupBarButtonItem.Id = 9;
      this.deleteGroupBarButtonItem.Name = "deleteGroupBarButtonItem";
      this.deleteGroupBarButtonItem.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.deleteGroupBarButtonItem_ItemClick);
      // 
      // barDockControlTop
      // 
      this.barDockControlTop.CausesValidation = false;
      this.barDockControlTop.Dock = System.Windows.Forms.DockStyle.Top;
      this.barDockControlTop.Location = new System.Drawing.Point(0, 0);
      this.barDockControlTop.Size = new System.Drawing.Size(731, 24);
      // 
      // barDockControlBottom
      // 
      this.barDockControlBottom.CausesValidation = false;
      this.barDockControlBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
      this.barDockControlBottom.Location = new System.Drawing.Point(0, 398);
      this.barDockControlBottom.Size = new System.Drawing.Size(731, 0);
      // 
      // barDockControlLeft
      // 
      this.barDockControlLeft.CausesValidation = false;
      this.barDockControlLeft.Dock = System.Windows.Forms.DockStyle.Left;
      this.barDockControlLeft.Location = new System.Drawing.Point(0, 24);
      this.barDockControlLeft.Size = new System.Drawing.Size(0, 374);
      // 
      // barDockControlRight
      // 
      this.barDockControlRight.CausesValidation = false;
      this.barDockControlRight.Dock = System.Windows.Forms.DockStyle.Right;
      this.barDockControlRight.Location = new System.Drawing.Point(731, 24);
      this.barDockControlRight.Size = new System.Drawing.Size(0, 374);
      // 
      // criteriaExpressionNavBarControl
      // 
      this.criteriaExpressionNavBarControl.Dock = System.Windows.Forms.DockStyle.Fill;
      this.criteriaExpressionNavBarControl.Location = new System.Drawing.Point(0, 24);
      this.criteriaExpressionNavBarControl.Name = "criteriaExpressionNavBarControl";
      this.criteriaExpressionNavBarControl.OptionsNavPane.ExpandedWidth = 731;
      this.criteriaExpressionNavBarControl.Size = new System.Drawing.Size(731, 374);
      this.criteriaExpressionNavBarControl.SkinExplorerBarViewScrollStyle = DevExpress.XtraNavBar.SkinExplorerBarViewScrollStyle.ScrollBar;
      this.criteriaExpressionNavBarControl.TabIndex = 4;
      // 
      // editValidation
      // 
      this.editValidation.Host = this;
      // 
      // CriteriaCombineControl
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.Controls.Add(this.criteriaExpressionNavBarControl);
      this.Controls.Add(this.barDockControlLeft);
      this.Controls.Add(this.barDockControlRight);
      this.Controls.Add(this.barDockControlBottom);
      this.Controls.Add(this.barDockControlTop);
      this.Name = "CriteriaCombineControl";
      this.Size = new System.Drawing.Size(731, 398);
      ((System.ComponentModel.ISupportInitialize)(this.barManager)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.criteriaExpressionLookUpEdit)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.criteriaExpressionKeyCaptionCollectionBindingSource)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.criteriaExpressionNavBarControl)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.editValidation)).EndInit();
      this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.BindingSource criteriaExpressionKeyCaptionCollectionBindingSource;
    private Services.Client.Validation.EditValidation editValidation;
    private DevExpress.XtraBars.BarManager barManager;
    private DevExpress.XtraBars.Bar menuBar;
    private DevExpress.XtraBars.BarDockControl barDockControlTop;
    private DevExpress.XtraBars.BarDockControl barDockControlBottom;
    private DevExpress.XtraBars.BarDockControl barDockControlLeft;
    private DevExpress.XtraBars.BarDockControl barDockControlRight;
    private DevExpress.XtraNavBar.NavBarControl criteriaExpressionNavBarControl;
    private DevExpress.XtraBars.BarEditItem criteriaExpressionLookUpBarEditItem;
    private DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit criteriaExpressionLookUpEdit;
    private DevExpress.XtraBars.BarButtonItem criteriaExpressionNewBarButtonItem;
    private DevExpress.XtraBars.BarButtonItem criteriaExpressionDeleteBarButtonItem;
    private DevExpress.XtraBars.BarButtonItem criteriaExpressionUpdateBarButtonItem;
    private DevExpress.XtraBars.BarButtonItem addGroupBarButtonItem;
    private DevExpress.XtraBars.BarButtonItem deleteGroupBarButtonItem;
    private DevExpress.XtraBars.BarButtonItem addBarButtonItem;
  }
}
