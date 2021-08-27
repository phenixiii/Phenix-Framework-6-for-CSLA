namespace Phenix.Windows
{
  partial class PromptCodeEditDialog
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
      this.components = new System.ComponentModel.Container();
      this.promptCodeKeyCaptionBindingSource = new System.Windows.Forms.BindingSource(this.components);
      this.splitContainerControl1 = new DevExpress.XtraEditors.SplitContainerControl();
      this.promptCodeKeyCaptionDataLayoutControl = new DevExpress.XtraDataLayout.DataLayoutControl();
      this.ReadLevelLookUpEdit = new DevExpress.XtraEditors.LookUpEdit();
      this.readLevelEnumKeyCaptionBindingSource = new System.Windows.Forms.BindingSource(this.components);
      this.ValueTextEdit = new DevExpress.XtraEditors.TextEdit();
      this.CaptionTextEdit = new DevExpress.XtraEditors.TextEdit();
      this.layoutControlGroup1 = new DevExpress.XtraLayout.LayoutControlGroup();
      this.layoutControlGroup2 = new DevExpress.XtraLayout.LayoutControlGroup();
      this.ItemForValue = new DevExpress.XtraLayout.LayoutControlItem();
      this.ItemForCaption = new DevExpress.XtraLayout.LayoutControlItem();
      this.emptySpaceItem1 = new DevExpress.XtraLayout.EmptySpaceItem();
      this.ItemForReadLevel = new DevExpress.XtraLayout.LayoutControlItem();
      this.cancelButton = new DevExpress.XtraEditors.SimpleButton();
      this.deleteButton = new DevExpress.XtraEditors.SimpleButton();
      this.saveButton = new DevExpress.XtraEditors.SimpleButton();
      this.editValidation = new Phenix.Services.Client.Validation.EditValidation(this.components);
      ((System.ComponentModel.ISupportInitialize)(this.promptCodeKeyCaptionBindingSource)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.splitContainerControl1)).BeginInit();
      this.splitContainerControl1.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)(this.promptCodeKeyCaptionDataLayoutControl)).BeginInit();
      this.promptCodeKeyCaptionDataLayoutControl.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)(this.ReadLevelLookUpEdit.Properties)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.readLevelEnumKeyCaptionBindingSource)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.ValueTextEdit.Properties)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.CaptionTextEdit.Properties)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup1)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup2)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.ItemForValue)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.ItemForCaption)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem1)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.ItemForReadLevel)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.editValidation)).BeginInit();
      this.SuspendLayout();
      // 
      // promptCodeKeyCaptionBindingSource
      // 
      this.promptCodeKeyCaptionBindingSource.DataSource = typeof(Phenix.Core.Rule.PromptCodeKeyCaption);
      // 
      // splitContainerControl1
      // 
      this.splitContainerControl1.Dock = System.Windows.Forms.DockStyle.Fill;
      this.splitContainerControl1.FixedPanel = DevExpress.XtraEditors.SplitFixedPanel.Panel2;
      this.splitContainerControl1.Location = new System.Drawing.Point(0, 0);
      this.splitContainerControl1.Name = "splitContainerControl1";
      this.splitContainerControl1.Panel1.Controls.Add(this.promptCodeKeyCaptionDataLayoutControl);
      this.splitContainerControl1.Panel1.Text = "Panel1";
      this.splitContainerControl1.Panel2.Controls.Add(this.cancelButton);
      this.splitContainerControl1.Panel2.Controls.Add(this.deleteButton);
      this.splitContainerControl1.Panel2.Controls.Add(this.saveButton);
      this.splitContainerControl1.Panel2.Text = "Panel2";
      this.splitContainerControl1.Size = new System.Drawing.Size(554, 160);
      this.splitContainerControl1.SplitterPosition = 138;
      this.splitContainerControl1.TabIndex = 0;
      this.splitContainerControl1.Text = "splitContainerControl1";
      // 
      // promptCodeKeyCaptionDataLayoutControl
      // 
      this.promptCodeKeyCaptionDataLayoutControl.Controls.Add(this.ReadLevelLookUpEdit);
      this.promptCodeKeyCaptionDataLayoutControl.Controls.Add(this.ValueTextEdit);
      this.promptCodeKeyCaptionDataLayoutControl.Controls.Add(this.CaptionTextEdit);
      this.promptCodeKeyCaptionDataLayoutControl.DataSource = this.promptCodeKeyCaptionBindingSource;
      this.promptCodeKeyCaptionDataLayoutControl.Dock = System.Windows.Forms.DockStyle.Fill;
      this.promptCodeKeyCaptionDataLayoutControl.Location = new System.Drawing.Point(0, 0);
      this.promptCodeKeyCaptionDataLayoutControl.Name = "promptCodeKeyCaptionDataLayoutControl";
      this.promptCodeKeyCaptionDataLayoutControl.Root = this.layoutControlGroup1;
      this.promptCodeKeyCaptionDataLayoutControl.Size = new System.Drawing.Size(411, 160);
      this.promptCodeKeyCaptionDataLayoutControl.TabIndex = 0;
      this.promptCodeKeyCaptionDataLayoutControl.Text = "dataLayoutControl1";
      // 
      // ReadLevelLookUpEdit
      // 
      this.ReadLevelLookUpEdit.DataBindings.Add(new System.Windows.Forms.Binding("EditValue", this.promptCodeKeyCaptionBindingSource, "ReadLevel", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
      this.editValidation.SetFriendlyCaption(this.ReadLevelLookUpEdit, this.ItemForReadLevel);
      this.ReadLevelLookUpEdit.Location = new System.Drawing.Point(63, 60);
      this.ReadLevelLookUpEdit.Name = "ReadLevelLookUpEdit";
      this.ReadLevelLookUpEdit.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
      this.ReadLevelLookUpEdit.Properties.Columns.AddRange(new DevExpress.XtraEditors.Controls.LookUpColumnInfo[] {
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("Key", "键", 43, DevExpress.Utils.FormatType.None, "", true, DevExpress.Utils.HorzAlignment.Near),
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("Caption", "标签", 51, DevExpress.Utils.FormatType.None, "", true, DevExpress.Utils.HorzAlignment.Near)});
      this.ReadLevelLookUpEdit.Properties.DataSource = this.readLevelEnumKeyCaptionBindingSource;
      this.ReadLevelLookUpEdit.Properties.DisplayMember = "Caption";
      this.ReadLevelLookUpEdit.Properties.ImmediatePopup = true;
      this.ReadLevelLookUpEdit.Properties.NullText = "";
      this.ReadLevelLookUpEdit.Properties.SearchMode = DevExpress.XtraEditors.Controls.SearchMode.OnlyInPopup;
      this.ReadLevelLookUpEdit.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.Standard;
      this.ReadLevelLookUpEdit.Properties.ValueMember = "Value";
      this.ReadLevelLookUpEdit.Size = new System.Drawing.Size(336, 20);
      this.ReadLevelLookUpEdit.StyleController = this.promptCodeKeyCaptionDataLayoutControl;
      this.ReadLevelLookUpEdit.TabIndex = 3;
      // 
      // readLevelEnumKeyCaptionBindingSource
      // 
      this.readLevelEnumKeyCaptionBindingSource.DataSource = typeof(Phenix.Core.Rule.EnumKeyCaption);
      // 
      // ValueTextEdit
      // 
      this.ValueTextEdit.DataBindings.Add(new System.Windows.Forms.Binding("EditValue", this.promptCodeKeyCaptionBindingSource, "Value", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
      this.editValidation.SetFriendlyCaption(this.ValueTextEdit, this.ItemForValue);
      this.ValueTextEdit.Location = new System.Drawing.Point(63, 12);
      this.ValueTextEdit.Name = "ValueTextEdit";
      this.ValueTextEdit.Size = new System.Drawing.Size(336, 20);
      this.ValueTextEdit.StyleController = this.promptCodeKeyCaptionDataLayoutControl;
      this.ValueTextEdit.TabIndex = 1;
      this.ValueTextEdit.EditValueChanged += new System.EventHandler(this.ValueTextEdit_EditValueChanged);
      // 
      // CaptionTextEdit
      // 
      this.CaptionTextEdit.DataBindings.Add(new System.Windows.Forms.Binding("EditValue", this.promptCodeKeyCaptionBindingSource, "Caption", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
      this.editValidation.SetFriendlyCaption(this.CaptionTextEdit, this.ItemForCaption);
      this.CaptionTextEdit.Location = new System.Drawing.Point(63, 36);
      this.CaptionTextEdit.Name = "CaptionTextEdit";
      this.CaptionTextEdit.Size = new System.Drawing.Size(336, 20);
      this.CaptionTextEdit.StyleController = this.promptCodeKeyCaptionDataLayoutControl;
      this.CaptionTextEdit.TabIndex = 2;
      this.CaptionTextEdit.EditValueChanged += new System.EventHandler(this.CaptionTextEdit_EditValueChanged);
      // 
      // layoutControlGroup1
      // 
      this.layoutControlGroup1.CustomizationFormText = "layoutControlGroup1";
      this.layoutControlGroup1.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
      this.layoutControlGroup1.GroupBordersVisible = false;
      this.layoutControlGroup1.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutControlGroup2,
            this.emptySpaceItem1,
            this.ItemForReadLevel});
      this.layoutControlGroup1.Location = new System.Drawing.Point(0, 0);
      this.layoutControlGroup1.Name = "layoutControlGroup1";
      this.layoutControlGroup1.Size = new System.Drawing.Size(411, 160);
      this.layoutControlGroup1.Text = "layoutControlGroup1";
      this.layoutControlGroup1.TextVisible = false;
      // 
      // layoutControlGroup2
      // 
      this.layoutControlGroup2.AllowDrawBackground = false;
      this.layoutControlGroup2.CustomizationFormText = "autoGeneratedGroup0";
      this.layoutControlGroup2.GroupBordersVisible = false;
      this.layoutControlGroup2.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.ItemForValue,
            this.ItemForCaption});
      this.layoutControlGroup2.Location = new System.Drawing.Point(0, 0);
      this.layoutControlGroup2.Name = "autoGeneratedGroup0";
      this.layoutControlGroup2.Size = new System.Drawing.Size(391, 48);
      this.layoutControlGroup2.Text = "autoGeneratedGroup0";
      // 
      // ItemForValue
      // 
      this.ItemForValue.AppearanceItemCaption.ForeColor = System.Drawing.Color.Black;
      this.ItemForValue.AppearanceItemCaption.Options.UseForeColor = true;
      this.ItemForValue.Control = this.ValueTextEdit;
      this.ItemForValue.CustomizationFormText = "Value";
      this.ItemForValue.Location = new System.Drawing.Point(0, 0);
      this.ItemForValue.Name = "ItemForValue";
      this.ItemForValue.Size = new System.Drawing.Size(391, 24);
      this.ItemForValue.Text = "内容";
      this.ItemForValue.TextSize = new System.Drawing.Size(48, 14);
      // 
      // ItemForCaption
      // 
      this.ItemForCaption.AppearanceItemCaption.ForeColor = System.Drawing.Color.Black;
      this.ItemForCaption.AppearanceItemCaption.Options.UseForeColor = true;
      this.ItemForCaption.Control = this.CaptionTextEdit;
      this.ItemForCaption.CustomizationFormText = "Caption";
      this.ItemForCaption.Location = new System.Drawing.Point(0, 24);
      this.ItemForCaption.Name = "ItemForCaption";
      this.ItemForCaption.Size = new System.Drawing.Size(391, 24);
      this.ItemForCaption.Text = "标签";
      this.ItemForCaption.TextSize = new System.Drawing.Size(48, 14);
      // 
      // emptySpaceItem1
      // 
      this.emptySpaceItem1.AllowHotTrack = false;
      this.emptySpaceItem1.CustomizationFormText = "emptySpaceItem1";
      this.emptySpaceItem1.Location = new System.Drawing.Point(0, 72);
      this.emptySpaceItem1.Name = "emptySpaceItem1";
      this.emptySpaceItem1.Size = new System.Drawing.Size(391, 68);
      this.emptySpaceItem1.Text = "emptySpaceItem1";
      this.emptySpaceItem1.TextSize = new System.Drawing.Size(0, 0);
      // 
      // ItemForReadLevel
      // 
      this.ItemForReadLevel.AppearanceItemCaption.ForeColor = System.Drawing.Color.Black;
      this.ItemForReadLevel.AppearanceItemCaption.Options.UseForeColor = true;
      this.ItemForReadLevel.Control = this.ReadLevelLookUpEdit;
      this.ItemForReadLevel.CustomizationFormText = "ReadLevel";
      this.ItemForReadLevel.Location = new System.Drawing.Point(0, 48);
      this.ItemForReadLevel.Name = "ItemForReadLevel";
      this.ItemForReadLevel.Size = new System.Drawing.Size(391, 24);
      this.ItemForReadLevel.Text = "读取级别";
      this.ItemForReadLevel.TextSize = new System.Drawing.Size(48, 14);
      // 
      // cancelButton
      // 
      this.cancelButton.DataBindings.Add(new System.Windows.Forms.Binding("Text", global::Phenix.Windows.Properties.Settings.Default, "Cancel", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
      this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
      this.cancelButton.Location = new System.Drawing.Point(32, 73);
      this.cancelButton.Name = "cancelButton";
      this.cancelButton.Size = new System.Drawing.Size(75, 23);
      this.cancelButton.TabIndex = 2;
      this.cancelButton.Text = global::Phenix.Windows.Properties.Settings.Default.Cancel;
      // 
      // deleteButton
      // 
      this.deleteButton.DataBindings.Add(new System.Windows.Forms.Binding("Text", global::Phenix.Windows.Properties.Settings.Default, "Delete", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
      this.deleteButton.Location = new System.Drawing.Point(32, 44);
      this.deleteButton.Name = "deleteButton";
      this.deleteButton.Size = new System.Drawing.Size(75, 23);
      this.deleteButton.TabIndex = 1;
      this.deleteButton.Text = global::Phenix.Windows.Properties.Settings.Default.Delete;
      this.deleteButton.Click += new System.EventHandler(this.deleteButton_Click);
      // 
      // saveButton
      // 
      this.saveButton.DataBindings.Add(new System.Windows.Forms.Binding("Text", global::Phenix.Windows.Properties.Settings.Default, "Update", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
      this.saveButton.Location = new System.Drawing.Point(32, 15);
      this.saveButton.Name = "saveButton";
      this.saveButton.Size = new System.Drawing.Size(75, 23);
      this.saveButton.TabIndex = 0;
      this.saveButton.Text = global::Phenix.Windows.Properties.Settings.Default.Update;
      this.saveButton.Click += new System.EventHandler(this.saveButton_Click);
      // 
      // editValidation
      // 
      this.editValidation.Host = this;
      // 
      // PromptCodeEditDialog
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.CancelButton = this.cancelButton;
      this.ClientSize = new System.Drawing.Size(554, 160);
      this.Controls.Add(this.splitContainerControl1);
      this.DataBindings.Add(new System.Windows.Forms.Binding("Text", global::Phenix.Windows.Properties.Settings.Default, "PromptCodeEditFormText", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
      this.Name = "PromptCodeEditDialog";
      this.Text = global::Phenix.Windows.Properties.Settings.Default.PromptCodeEdit;
      this.Shown += new System.EventHandler(this.PromptCodeEditDialog_Shown);
      ((System.ComponentModel.ISupportInitialize)(this.promptCodeKeyCaptionBindingSource)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.splitContainerControl1)).EndInit();
      this.splitContainerControl1.ResumeLayout(false);
      ((System.ComponentModel.ISupportInitialize)(this.promptCodeKeyCaptionDataLayoutControl)).EndInit();
      this.promptCodeKeyCaptionDataLayoutControl.ResumeLayout(false);
      ((System.ComponentModel.ISupportInitialize)(this.ReadLevelLookUpEdit.Properties)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.readLevelEnumKeyCaptionBindingSource)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.ValueTextEdit.Properties)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.CaptionTextEdit.Properties)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup1)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup2)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.ItemForValue)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.ItemForCaption)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem1)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.ItemForReadLevel)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.editValidation)).EndInit();
      this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.BindingSource promptCodeKeyCaptionBindingSource;
    private DevExpress.XtraEditors.SplitContainerControl splitContainerControl1;
    private DevExpress.XtraDataLayout.DataLayoutControl promptCodeKeyCaptionDataLayoutControl;
    private DevExpress.XtraEditors.TextEdit ValueTextEdit;
    private DevExpress.XtraEditors.TextEdit CaptionTextEdit;
    private DevExpress.XtraLayout.LayoutControlGroup layoutControlGroup1;
    private DevExpress.XtraLayout.LayoutControlGroup layoutControlGroup2;
    private DevExpress.XtraLayout.LayoutControlItem ItemForValue;
    private DevExpress.XtraLayout.LayoutControlItem ItemForCaption;
    private DevExpress.XtraEditors.SimpleButton deleteButton;
    private DevExpress.XtraEditors.SimpleButton saveButton;
    private DevExpress.XtraEditors.SimpleButton cancelButton;
    private DevExpress.XtraLayout.EmptySpaceItem emptySpaceItem1;
    private DevExpress.XtraEditors.LookUpEdit ReadLevelLookUpEdit;
    private DevExpress.XtraLayout.LayoutControlItem ItemForReadLevel;
    private Services.Client.Validation.EditValidation editValidation;
    private System.Windows.Forms.BindingSource readLevelEnumKeyCaptionBindingSource;
  }
}