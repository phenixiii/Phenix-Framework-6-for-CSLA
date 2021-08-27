namespace Phenix.Windows
{
  partial class CriteriaExpressionNewDialog
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
      this.splitContainerControl1 = new DevExpress.XtraEditors.SplitContainerControl();
      this.criteriaExpressionKeyCaptionDataLayoutControl = new DevExpress.XtraDataLayout.DataLayoutControl();
      this.CaptionTextEdit = new DevExpress.XtraEditors.TextEdit();
      this.criteriaExpressionKeyCaptionBindingSource = new System.Windows.Forms.BindingSource(this.components);
      this.ReadLevelLookUpEdit = new DevExpress.XtraEditors.LookUpEdit();
      this.readLevelEnumKeyCaptionBindingSource = new System.Windows.Forms.BindingSource(this.components);
      this.layoutControlGroup1 = new DevExpress.XtraLayout.LayoutControlGroup();
      this.layoutControlGroup2 = new DevExpress.XtraLayout.LayoutControlGroup();
      this.ItemForCaption = new DevExpress.XtraLayout.LayoutControlItem();
      this.ItemForReadLevel = new DevExpress.XtraLayout.LayoutControlItem();
      this.emptySpaceItem1 = new DevExpress.XtraLayout.EmptySpaceItem();
      this.cancelButton = new DevExpress.XtraEditors.SimpleButton();
      this.okButton = new DevExpress.XtraEditors.SimpleButton();
      this.editValidation = new Phenix.Services.Client.Validation.EditValidation(this.components);
      ((System.ComponentModel.ISupportInitialize)(this.splitContainerControl1)).BeginInit();
      this.splitContainerControl1.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)(this.criteriaExpressionKeyCaptionDataLayoutControl)).BeginInit();
      this.criteriaExpressionKeyCaptionDataLayoutControl.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)(this.CaptionTextEdit.Properties)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.criteriaExpressionKeyCaptionBindingSource)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.ReadLevelLookUpEdit.Properties)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.readLevelEnumKeyCaptionBindingSource)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup1)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup2)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.ItemForCaption)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.ItemForReadLevel)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem1)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.editValidation)).BeginInit();
      this.SuspendLayout();
      // 
      // splitContainerControl1
      // 
      this.splitContainerControl1.Dock = System.Windows.Forms.DockStyle.Fill;
      this.splitContainerControl1.FixedPanel = DevExpress.XtraEditors.SplitFixedPanel.Panel2;
      this.splitContainerControl1.Location = new System.Drawing.Point(0, 0);
      this.splitContainerControl1.Name = "splitContainerControl1";
      this.splitContainerControl1.Panel1.Controls.Add(this.criteriaExpressionKeyCaptionDataLayoutControl);
      this.splitContainerControl1.Panel1.Text = "Panel1";
      this.splitContainerControl1.Panel2.Controls.Add(this.cancelButton);
      this.splitContainerControl1.Panel2.Controls.Add(this.okButton);
      this.splitContainerControl1.Panel2.Text = "Panel2";
      this.splitContainerControl1.Size = new System.Drawing.Size(524, 122);
      this.splitContainerControl1.SplitterPosition = 118;
      this.splitContainerControl1.TabIndex = 2;
      this.splitContainerControl1.Text = "splitContainerControl1";
      // 
      // criteriaExpressionKeyCaptionDataLayoutControl
      // 
      this.criteriaExpressionKeyCaptionDataLayoutControl.Controls.Add(this.CaptionTextEdit);
      this.criteriaExpressionKeyCaptionDataLayoutControl.Controls.Add(this.ReadLevelLookUpEdit);
      this.criteriaExpressionKeyCaptionDataLayoutControl.DataSource = this.criteriaExpressionKeyCaptionBindingSource;
      this.criteriaExpressionKeyCaptionDataLayoutControl.Dock = System.Windows.Forms.DockStyle.Fill;
      this.criteriaExpressionKeyCaptionDataLayoutControl.Location = new System.Drawing.Point(0, 0);
      this.criteriaExpressionKeyCaptionDataLayoutControl.Name = "criteriaExpressionKeyCaptionDataLayoutControl";
      this.criteriaExpressionKeyCaptionDataLayoutControl.Root = this.layoutControlGroup1;
      this.criteriaExpressionKeyCaptionDataLayoutControl.Size = new System.Drawing.Size(401, 122);
      this.criteriaExpressionKeyCaptionDataLayoutControl.TabIndex = 0;
      this.criteriaExpressionKeyCaptionDataLayoutControl.Text = "dataLayoutControl1";
      // 
      // CaptionTextEdit
      // 
      this.CaptionTextEdit.DataBindings.Add(new System.Windows.Forms.Binding("EditValue", this.criteriaExpressionKeyCaptionBindingSource, "Caption", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
      this.editValidation.SetFriendlyCaption(this.CaptionTextEdit, this.ItemForCaption);
      this.CaptionTextEdit.Location = new System.Drawing.Point(63, 12);
      this.CaptionTextEdit.Name = "CaptionTextEdit";
      this.CaptionTextEdit.Size = new System.Drawing.Size(326, 20);
      this.CaptionTextEdit.StyleController = this.criteriaExpressionKeyCaptionDataLayoutControl;
      this.CaptionTextEdit.TabIndex = 1;
      this.CaptionTextEdit.EditValueChanged += new System.EventHandler(this.CaptionTextEdit_EditValueChanged);
      // 
      // criteriaExpressionKeyCaptionBindingSource
      // 
      this.criteriaExpressionKeyCaptionBindingSource.DataSource = typeof(Phenix.Core.Rule.CriteriaExpressionKeyCaption);
      // 
      // ReadLevelLookUpEdit
      // 
      this.ReadLevelLookUpEdit.DataBindings.Add(new System.Windows.Forms.Binding("EditValue", this.criteriaExpressionKeyCaptionBindingSource, "ReadLevel", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
      this.editValidation.SetFriendlyCaption(this.ReadLevelLookUpEdit, this.ItemForReadLevel);
      this.ReadLevelLookUpEdit.Location = new System.Drawing.Point(63, 36);
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
      this.ReadLevelLookUpEdit.Size = new System.Drawing.Size(326, 20);
      this.ReadLevelLookUpEdit.StyleController = this.criteriaExpressionKeyCaptionDataLayoutControl;
      this.ReadLevelLookUpEdit.TabIndex = 2;
      // 
      // readLevelEnumKeyCaptionBindingSource
      // 
      this.readLevelEnumKeyCaptionBindingSource.DataSource = typeof(Phenix.Core.Rule.EnumKeyCaption);
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
      this.layoutControlGroup1.Size = new System.Drawing.Size(401, 122);
      this.layoutControlGroup1.Text = "layoutControlGroup1";
      this.layoutControlGroup1.TextVisible = false;
      // 
      // layoutControlGroup2
      // 
      this.layoutControlGroup2.AllowDrawBackground = false;
      this.layoutControlGroup2.CustomizationFormText = "autoGeneratedGroup0";
      this.layoutControlGroup2.GroupBordersVisible = false;
      this.layoutControlGroup2.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.ItemForCaption,
            this.ItemForReadLevel,
            this.emptySpaceItem1});
      this.layoutControlGroup2.Location = new System.Drawing.Point(0, 0);
      this.layoutControlGroup2.Name = "autoGeneratedGroup0";
      this.layoutControlGroup2.Size = new System.Drawing.Size(381, 102);
      this.layoutControlGroup2.Text = "autoGeneratedGroup0";
      // 
      // ItemForCaption
      // 
      this.ItemForCaption.AppearanceItemCaption.ForeColor = System.Drawing.Color.Black;
      this.ItemForCaption.AppearanceItemCaption.Options.UseForeColor = true;
      this.ItemForCaption.Control = this.CaptionTextEdit;
      this.ItemForCaption.CustomizationFormText = "标签";
      this.ItemForCaption.Location = new System.Drawing.Point(0, 0);
      this.ItemForCaption.Name = "ItemForCaption";
      this.ItemForCaption.Size = new System.Drawing.Size(381, 24);
      this.ItemForCaption.Text = "标签";
      this.ItemForCaption.TextSize = new System.Drawing.Size(48, 14);
      // 
      // ItemForReadLevel
      // 
      this.ItemForReadLevel.AppearanceItemCaption.ForeColor = System.Drawing.Color.Black;
      this.ItemForReadLevel.AppearanceItemCaption.Options.UseForeColor = true;
      this.ItemForReadLevel.Control = this.ReadLevelLookUpEdit;
      this.ItemForReadLevel.CustomizationFormText = "读取级别";
      this.ItemForReadLevel.Location = new System.Drawing.Point(0, 24);
      this.ItemForReadLevel.Name = "ItemForReadLevel";
      this.ItemForReadLevel.Size = new System.Drawing.Size(381, 24);
      this.ItemForReadLevel.Text = "读取级别";
      this.ItemForReadLevel.TextSize = new System.Drawing.Size(48, 14);
      // 
      // emptySpaceItem1
      // 
      this.emptySpaceItem1.AllowHotTrack = false;
      this.emptySpaceItem1.CustomizationFormText = "emptySpaceItem1";
      this.emptySpaceItem1.Location = new System.Drawing.Point(0, 48);
      this.emptySpaceItem1.Name = "emptySpaceItem1";
      this.emptySpaceItem1.Size = new System.Drawing.Size(381, 54);
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
      this.okButton.Location = new System.Drawing.Point(21, 12);
      this.okButton.Name = "okButton";
      this.okButton.Size = new System.Drawing.Size(75, 23);
      this.okButton.TabIndex = 0;
      this.okButton.Text = global::Phenix.Windows.Properties.Settings.Default.Ok;
      this.okButton.Click += new System.EventHandler(this.okButton_Click);
      // 
      // editValidation
      // 
      this.editValidation.Host = this;
      // 
      // CriteriaExpressionNewDialog
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(524, 122);
      this.Controls.Add(this.splitContainerControl1);
      this.DataBindings.Add(new System.Windows.Forms.Binding("Text", global::Phenix.Windows.Properties.Settings.Default, "CriteriaExpressionNew", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
      this.Name = "CriteriaExpressionNewDialog";
      this.Text = global::Phenix.Windows.Properties.Settings.Default.CriteriaExpressionNew;
      this.Shown += new System.EventHandler(this.CriteriaExpressionNewDialog_Shown);
      ((System.ComponentModel.ISupportInitialize)(this.splitContainerControl1)).EndInit();
      this.splitContainerControl1.ResumeLayout(false);
      ((System.ComponentModel.ISupportInitialize)(this.criteriaExpressionKeyCaptionDataLayoutControl)).EndInit();
      this.criteriaExpressionKeyCaptionDataLayoutControl.ResumeLayout(false);
      ((System.ComponentModel.ISupportInitialize)(this.CaptionTextEdit.Properties)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.criteriaExpressionKeyCaptionBindingSource)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.ReadLevelLookUpEdit.Properties)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.readLevelEnumKeyCaptionBindingSource)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup1)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup2)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.ItemForCaption)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.ItemForReadLevel)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem1)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.editValidation)).EndInit();
      this.ResumeLayout(false);

    }

    #endregion

    private DevExpress.XtraEditors.SplitContainerControl splitContainerControl1;
    private DevExpress.XtraEditors.SimpleButton cancelButton;
    private DevExpress.XtraEditors.SimpleButton okButton;
    private DevExpress.XtraDataLayout.DataLayoutControl criteriaExpressionKeyCaptionDataLayoutControl;
    private System.Windows.Forms.BindingSource criteriaExpressionKeyCaptionBindingSource;
    private DevExpress.XtraEditors.TextEdit CaptionTextEdit;
    private DevExpress.XtraLayout.LayoutControlGroup layoutControlGroup1;
    private DevExpress.XtraLayout.LayoutControlGroup layoutControlGroup2;
    private DevExpress.XtraLayout.LayoutControlItem ItemForCaption;
    private DevExpress.XtraLayout.LayoutControlItem ItemForReadLevel;
    private Services.Client.Validation.EditValidation editValidation;
    private System.Windows.Forms.BindingSource readLevelEnumKeyCaptionBindingSource;
    private DevExpress.XtraEditors.LookUpEdit ReadLevelLookUpEdit;
    private DevExpress.XtraLayout.EmptySpaceItem emptySpaceItem1;
  }
}