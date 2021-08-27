namespace Phenix.Windows
{
  partial class CriteriaExpressionPropertySelectDialog
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
      this.cancelButton = new DevExpress.XtraEditors.SimpleButton();
      this.splitContainerControl1 = new DevExpress.XtraEditors.SplitContainerControl();
      this.criteriaExpressionPropertyKeyCaptionCollectionGridControl = new DevExpress.XtraGrid.GridControl();
      this.criteriaExpressionPropertyKeyCaptionCollectionBindingSource = new System.Windows.Forms.BindingSource(this.components);
      this.criteriaExpressionPropertyKeyCaptionCollectionGridView = new DevExpress.XtraGrid.Views.Grid.GridView();
      this.colSelected = new DevExpress.XtraGrid.Columns.GridColumn();
      this.colCaption = new DevExpress.XtraGrid.Columns.GridColumn();
      this.okButton = new DevExpress.XtraEditors.SimpleButton();
      this.editValidation = new Phenix.Services.Client.Validation.EditValidation(this.components);
      ((System.ComponentModel.ISupportInitialize)(this.splitContainerControl1)).BeginInit();
      this.splitContainerControl1.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)(this.criteriaExpressionPropertyKeyCaptionCollectionGridControl)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.criteriaExpressionPropertyKeyCaptionCollectionBindingSource)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.criteriaExpressionPropertyKeyCaptionCollectionGridView)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.editValidation)).BeginInit();
      this.SuspendLayout();
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
      // splitContainerControl1
      // 
      this.splitContainerControl1.Dock = System.Windows.Forms.DockStyle.Fill;
      this.splitContainerControl1.FixedPanel = DevExpress.XtraEditors.SplitFixedPanel.Panel2;
      this.splitContainerControl1.Location = new System.Drawing.Point(0, 0);
      this.splitContainerControl1.Name = "splitContainerControl1";
      this.splitContainerControl1.Panel1.Controls.Add(this.criteriaExpressionPropertyKeyCaptionCollectionGridControl);
      this.splitContainerControl1.Panel1.Text = "Panel1";
      this.splitContainerControl1.Panel2.Controls.Add(this.cancelButton);
      this.splitContainerControl1.Panel2.Controls.Add(this.okButton);
      this.splitContainerControl1.Panel2.Text = "Panel2";
      this.splitContainerControl1.Size = new System.Drawing.Size(594, 322);
      this.splitContainerControl1.SplitterPosition = 118;
      this.splitContainerControl1.TabIndex = 1;
      this.splitContainerControl1.Text = "splitContainerControl1";
      // 
      // criteriaExpressionPropertyKeyCaptionCollectionGridControl
      // 
      this.criteriaExpressionPropertyKeyCaptionCollectionGridControl.DataSource = this.criteriaExpressionPropertyKeyCaptionCollectionBindingSource;
      this.criteriaExpressionPropertyKeyCaptionCollectionGridControl.Dock = System.Windows.Forms.DockStyle.Fill;
      this.criteriaExpressionPropertyKeyCaptionCollectionGridControl.Location = new System.Drawing.Point(0, 0);
      this.criteriaExpressionPropertyKeyCaptionCollectionGridControl.MainView = this.criteriaExpressionPropertyKeyCaptionCollectionGridView;
      this.criteriaExpressionPropertyKeyCaptionCollectionGridControl.Name = "criteriaExpressionPropertyKeyCaptionCollectionGridControl";
      this.criteriaExpressionPropertyKeyCaptionCollectionGridControl.Size = new System.Drawing.Size(471, 322);
      this.criteriaExpressionPropertyKeyCaptionCollectionGridControl.TabIndex = 0;
      this.criteriaExpressionPropertyKeyCaptionCollectionGridControl.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.criteriaExpressionPropertyKeyCaptionCollectionGridView});
      // 
      // criteriaExpressionPropertyKeyCaptionCollectionBindingSource
      // 
      this.criteriaExpressionPropertyKeyCaptionCollectionBindingSource.DataSource = typeof(Phenix.Core.Rule.CriteriaExpressionPropertyKeyCaptionCollection);
      this.criteriaExpressionPropertyKeyCaptionCollectionBindingSource.CurrentItemChanged += new System.EventHandler(this.criteriaExpressionPropertyKeyCaptionCollectionBindingSource_CurrentItemChanged);
      // 
      // criteriaExpressionPropertyKeyCaptionCollectionGridView
      // 
      this.criteriaExpressionPropertyKeyCaptionCollectionGridView.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.colSelected,
            this.colCaption});
      this.criteriaExpressionPropertyKeyCaptionCollectionGridView.GridControl = this.criteriaExpressionPropertyKeyCaptionCollectionGridControl;
      this.criteriaExpressionPropertyKeyCaptionCollectionGridView.Name = "criteriaExpressionPropertyKeyCaptionCollectionGridView";
      this.criteriaExpressionPropertyKeyCaptionCollectionGridView.OptionsView.ShowGroupPanel = false;
      this.criteriaExpressionPropertyKeyCaptionCollectionGridView.CellValueChanging += new DevExpress.XtraGrid.Views.Base.CellValueChangedEventHandler(this.criteriaExpressionPropertyKeyCaptionCollectionGridView_CellValueChanging);
      // 
      // colSelected
      // 
      this.colSelected.Caption = "选择";
      this.colSelected.FieldName = "Selected";
      this.colSelected.Name = "colSelected";
      this.colSelected.Visible = true;
      this.colSelected.VisibleIndex = 0;
      this.colSelected.Width = 88;
      // 
      // colCaption
      // 
      this.colCaption.AppearanceHeader.ForeColor = System.Drawing.Color.Black;
      this.colCaption.AppearanceHeader.Options.UseForeColor = true;
      this.colCaption.Caption = "属性";
      this.colCaption.FieldName = "Caption";
      this.colCaption.Name = "colCaption";
      this.colCaption.OptionsColumn.ReadOnly = true;
      this.colCaption.Visible = true;
      this.colCaption.VisibleIndex = 1;
      this.colCaption.Width = 365;
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
      // editValidation
      // 
      this.editValidation.Host = this;
      // 
      // CriteriaExpressionPropertySelectDialog
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(594, 322);
      this.Controls.Add(this.splitContainerControl1);
      this.DataBindings.Add(new System.Windows.Forms.Binding("Text", global::Phenix.Windows.Properties.Settings.Default, "CriteriaExpressionPropertySelect", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
      this.Name = "CriteriaExpressionPropertySelectDialog";
      this.Text = global::Phenix.Windows.Properties.Settings.Default.CriteriaExpressionPropertySelect;
      this.Shown += new System.EventHandler(this.CriteriaExpressionPropertySelectDialog_Shown);
      ((System.ComponentModel.ISupportInitialize)(this.splitContainerControl1)).EndInit();
      this.splitContainerControl1.ResumeLayout(false);
      ((System.ComponentModel.ISupportInitialize)(this.criteriaExpressionPropertyKeyCaptionCollectionGridControl)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.criteriaExpressionPropertyKeyCaptionCollectionBindingSource)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.criteriaExpressionPropertyKeyCaptionCollectionGridView)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.editValidation)).EndInit();
      this.ResumeLayout(false);

    }

    #endregion

    private DevExpress.XtraEditors.SimpleButton cancelButton;
    private DevExpress.XtraEditors.SplitContainerControl splitContainerControl1;
    private DevExpress.XtraEditors.SimpleButton okButton;
    private Services.Client.Validation.EditValidation editValidation;
    private DevExpress.XtraGrid.GridControl criteriaExpressionPropertyKeyCaptionCollectionGridControl;
    private DevExpress.XtraGrid.Views.Grid.GridView criteriaExpressionPropertyKeyCaptionCollectionGridView;
    private DevExpress.XtraGrid.Columns.GridColumn colCaption;
    private System.Windows.Forms.BindingSource criteriaExpressionPropertyKeyCaptionCollectionBindingSource;
    private DevExpress.XtraGrid.Columns.GridColumn colSelected;
  }
}