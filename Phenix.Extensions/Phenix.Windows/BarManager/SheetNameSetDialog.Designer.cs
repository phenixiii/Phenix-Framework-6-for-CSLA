namespace Phenix.Windows
{
  partial class SheetNameSetDialog
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

    #region Windows Form Designer generated code

    /// <summary>
    /// Required method for Designer support - do not modify
    /// the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
      this.ItemForSheetName = new DevExpress.XtraLayout.LayoutControlItem();
      this.SheetNameTextEdit = new DevExpress.XtraEditors.TextEdit();
      this.dataLayoutControl1 = new DevExpress.XtraDataLayout.DataLayoutControl();
      this.SaveSheetConfigCheckEdit = new DevExpress.XtraEditors.CheckEdit();
      this.layoutControlGroup1 = new DevExpress.XtraLayout.LayoutControlGroup();
      this.layoutControlGroup2 = new DevExpress.XtraLayout.LayoutControlGroup();
      this.ItemForSaveSheetConfig = new DevExpress.XtraLayout.LayoutControlItem();
      this.emptySpaceItem1 = new DevExpress.XtraLayout.EmptySpaceItem();
      this.cancelButton = new DevExpress.XtraEditors.SimpleButton();
      this.okButton = new DevExpress.XtraEditors.SimpleButton();
      this.splitContainerControl1 = new DevExpress.XtraEditors.SplitContainerControl();
      ((System.ComponentModel.ISupportInitialize)(this.ItemForSheetName)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.SheetNameTextEdit.Properties)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.dataLayoutControl1)).BeginInit();
      this.dataLayoutControl1.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)(this.SaveSheetConfigCheckEdit.Properties)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup1)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup2)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.ItemForSaveSheetConfig)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem1)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.splitContainerControl1)).BeginInit();
      this.splitContainerControl1.SuspendLayout();
      this.SuspendLayout();
      // 
      // ItemForSheetName
      // 
      this.ItemForSheetName.AppearanceItemCaption.ForeColor = System.Drawing.Color.Black;
      this.ItemForSheetName.AppearanceItemCaption.Options.UseForeColor = true;
      this.ItemForSheetName.Control = this.SheetNameTextEdit;
      this.ItemForSheetName.CustomizationFormText = "Sheet名称";
      this.ItemForSheetName.DataBindings.Add(new System.Windows.Forms.Binding("Text", global::Phenix.Windows.Properties.Settings.Default, "SheetName", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
      this.ItemForSheetName.Location = new System.Drawing.Point(0, 0);
      this.ItemForSheetName.Name = "ItemForSheetName";
      this.ItemForSheetName.Size = new System.Drawing.Size(372, 25);
      this.ItemForSheetName.Text = global::Phenix.Windows.Properties.Settings.Default.SheetName;
      this.ItemForSheetName.TextSize = new System.Drawing.Size(108, 14);
      // 
      // SheetNameTextEdit
      // 
      this.SheetNameTextEdit.Location = new System.Drawing.Point(123, 12);
      this.SheetNameTextEdit.Name = "SheetNameTextEdit";
      this.SheetNameTextEdit.Size = new System.Drawing.Size(257, 21);
      this.SheetNameTextEdit.StyleController = this.dataLayoutControl1;
      this.SheetNameTextEdit.TabIndex = 0;
      this.SheetNameTextEdit.EditValueChanged += new System.EventHandler(this.SheetNameTextEdit_EditValueChanged);
      // 
      // dataLayoutControl1
      // 
      this.dataLayoutControl1.Controls.Add(this.SaveSheetConfigCheckEdit);
      this.dataLayoutControl1.Controls.Add(this.SheetNameTextEdit);
      this.dataLayoutControl1.Dock = System.Windows.Forms.DockStyle.Fill;
      this.dataLayoutControl1.Location = new System.Drawing.Point(0, 0);
      this.dataLayoutControl1.Name = "dataLayoutControl1";
      this.dataLayoutControl1.Root = this.layoutControlGroup1;
      this.dataLayoutControl1.Size = new System.Drawing.Size(392, 151);
      this.dataLayoutControl1.TabIndex = 0;
      this.dataLayoutControl1.Text = "dataLayoutControl1";
      // 
      // SaveSheetConfigCheckEdit
      // 
      this.SaveSheetConfigCheckEdit.EditValue = true;
      this.SaveSheetConfigCheckEdit.Location = new System.Drawing.Point(123, 37);
      this.SaveSheetConfigCheckEdit.Name = "SaveSheetConfigCheckEdit";
      this.SaveSheetConfigCheckEdit.Properties.Caption = "";
      this.SaveSheetConfigCheckEdit.Size = new System.Drawing.Size(257, 19);
      this.SaveSheetConfigCheckEdit.StyleController = this.dataLayoutControl1;
      this.SaveSheetConfigCheckEdit.TabIndex = 1;
      // 
      // layoutControlGroup1
      // 
      this.layoutControlGroup1.CustomizationFormText = "layoutControlGroup1";
      this.layoutControlGroup1.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
      this.layoutControlGroup1.GroupBordersVisible = false;
      this.layoutControlGroup1.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutControlGroup2});
      this.layoutControlGroup1.Location = new System.Drawing.Point(0, 0);
      this.layoutControlGroup1.Name = "layoutControlGroup1";
      this.layoutControlGroup1.Size = new System.Drawing.Size(392, 151);
      this.layoutControlGroup1.Spacing = new DevExpress.XtraLayout.Utils.Padding(0, 0, 0, 0);
      this.layoutControlGroup1.Text = "layoutControlGroup1";
      this.layoutControlGroup1.TextVisible = false;
      // 
      // layoutControlGroup2
      // 
      this.layoutControlGroup2.AllowDrawBackground = false;
      this.layoutControlGroup2.CustomizationFormText = "autoGeneratedGroup0";
      this.layoutControlGroup2.GroupBordersVisible = false;
      this.layoutControlGroup2.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.ItemForSheetName,
            this.ItemForSaveSheetConfig,
            this.emptySpaceItem1});
      this.layoutControlGroup2.Location = new System.Drawing.Point(0, 0);
      this.layoutControlGroup2.Name = "autoGeneratedGroup0";
      this.layoutControlGroup2.Size = new System.Drawing.Size(372, 131);
      this.layoutControlGroup2.Text = "autoGeneratedGroup0";
      // 
      // ItemForSaveSheetConfig
      // 
      this.ItemForSaveSheetConfig.Control = this.SaveSheetConfigCheckEdit;
      this.ItemForSaveSheetConfig.CustomizationFormText = "下次默认使用本名称";
      this.ItemForSaveSheetConfig.DataBindings.Add(new System.Windows.Forms.Binding("Text", global::Phenix.Windows.Properties.Settings.Default, "SaveSheetConfig", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
      this.ItemForSaveSheetConfig.Location = new System.Drawing.Point(0, 25);
      this.ItemForSaveSheetConfig.Name = "ItemForSaveSheetConfig";
      this.ItemForSaveSheetConfig.Size = new System.Drawing.Size(372, 23);
      this.ItemForSaveSheetConfig.Text = global::Phenix.Windows.Properties.Settings.Default.SaveSheetConfig;
      this.ItemForSaveSheetConfig.TextSize = new System.Drawing.Size(108, 14);
      // 
      // emptySpaceItem1
      // 
      this.emptySpaceItem1.CustomizationFormText = "emptySpaceItem1";
      this.emptySpaceItem1.Location = new System.Drawing.Point(0, 48);
      this.emptySpaceItem1.Name = "emptySpaceItem1";
      this.emptySpaceItem1.Size = new System.Drawing.Size(372, 83);
      this.emptySpaceItem1.Text = "emptySpaceItem1";
      this.emptySpaceItem1.TextSize = new System.Drawing.Size(0, 0);
      // 
      // cancelButton
      // 
      this.cancelButton.DataBindings.Add(new System.Windows.Forms.Binding("Text", global::Phenix.Windows.Properties.Settings.Default, "Cancel", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
      this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
      this.cancelButton.Location = new System.Drawing.Point(21, 41);
      this.cancelButton.Name = "cancelButton";
      this.cancelButton.Size = new System.Drawing.Size(75, 23);
      this.cancelButton.TabIndex = 1;
      this.cancelButton.Text = global::Phenix.Windows.Properties.Settings.Default.Cancel;
      // 
      // okButton
      // 
      this.okButton.DataBindings.Add(new System.Windows.Forms.Binding("Text", global::Phenix.Windows.Properties.Settings.Default, "Ok", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
      this.okButton.DialogResult = System.Windows.Forms.DialogResult.OK;
      this.okButton.Location = new System.Drawing.Point(21, 12);
      this.okButton.Name = "okButton";
      this.okButton.Size = new System.Drawing.Size(75, 23);
      this.okButton.TabIndex = 0;
      this.okButton.Text = global::Phenix.Windows.Properties.Settings.Default.Ok;
      // 
      // splitContainerControl1
      // 
      this.splitContainerControl1.Dock = System.Windows.Forms.DockStyle.Fill;
      this.splitContainerControl1.FixedPanel = DevExpress.XtraEditors.SplitFixedPanel.Panel2;
      this.splitContainerControl1.Location = new System.Drawing.Point(0, 0);
      this.splitContainerControl1.Name = "splitContainerControl1";
      this.splitContainerControl1.Panel1.Controls.Add(this.dataLayoutControl1);
      this.splitContainerControl1.Panel1.Text = "Panel1";
      this.splitContainerControl1.Panel2.Controls.Add(this.cancelButton);
      this.splitContainerControl1.Panel2.Controls.Add(this.okButton);
      this.splitContainerControl1.Panel2.Text = "Panel2";
      this.splitContainerControl1.Size = new System.Drawing.Size(516, 151);
      this.splitContainerControl1.SplitterPosition = 118;
      this.splitContainerControl1.TabIndex = 4;
      this.splitContainerControl1.Text = "splitContainerControl1";
      // 
      // SheetNameSetDialog
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.CancelButton = this.cancelButton;
      this.ClientSize = new System.Drawing.Size(516, 151);
      this.Controls.Add(this.splitContainerControl1);
      this.DataBindings.Add(new System.Windows.Forms.Binding("Text", global::Phenix.Windows.Properties.Settings.Default, "InputSheetName", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
      this.Name = "SheetNameSetDialog";
      this.Text = global::Phenix.Windows.Properties.Settings.Default.InputSheetName;
      this.Shown += new System.EventHandler(this.InputSheetNameDialog_Shown);
      ((System.ComponentModel.ISupportInitialize)(this.ItemForSheetName)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.SheetNameTextEdit.Properties)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.dataLayoutControl1)).EndInit();
      this.dataLayoutControl1.ResumeLayout(false);
      ((System.ComponentModel.ISupportInitialize)(this.SaveSheetConfigCheckEdit.Properties)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup1)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup2)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.ItemForSaveSheetConfig)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem1)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.splitContainerControl1)).EndInit();
      this.splitContainerControl1.ResumeLayout(false);
      this.ResumeLayout(false);

    }

    #endregion

    private DevExpress.XtraLayout.LayoutControlItem ItemForSheetName;
    private DevExpress.XtraEditors.TextEdit SheetNameTextEdit;
    private DevExpress.XtraDataLayout.DataLayoutControl dataLayoutControl1;
    private DevExpress.XtraLayout.LayoutControlGroup layoutControlGroup1;
    private DevExpress.XtraLayout.LayoutControlGroup layoutControlGroup2;
    private DevExpress.XtraLayout.EmptySpaceItem emptySpaceItem1;
    private DevExpress.XtraEditors.SimpleButton cancelButton;
    private DevExpress.XtraEditors.SimpleButton okButton;
    private DevExpress.XtraEditors.SplitContainerControl splitContainerControl1;
    private DevExpress.XtraEditors.CheckEdit SaveSheetConfigCheckEdit;
    private DevExpress.XtraLayout.LayoutControlItem ItemForSaveSheetConfig;
  }
}