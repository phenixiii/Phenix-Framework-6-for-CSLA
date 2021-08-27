namespace Phenix.Workflow.Windows.Desinger
{
  partial class WorkflowInfoSelectDialog
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
      System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(WorkflowInfoSelectDialog));
      this.toolStrip = new System.Windows.Forms.ToolStrip();
      this.typeNameToolStripLabel = new System.Windows.Forms.ToolStripLabel();
      this.typeNameToolStripTextBox = new System.Windows.Forms.ToolStripTextBox();
      this.cancelToolStripButton = new System.Windows.Forms.ToolStripButton();
      this.okToolStripButton = new System.Windows.Forms.ToolStripButton();
      this.workflowInfoListDataGridView = new System.Windows.Forms.DataGridView();
      this.workflowInfoListBindingSource = new System.Windows.Forms.BindingSource();
      this.typeNameDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
      this.captionDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
      this.typeNamespaceDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
      this.createUserNumberDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
      this.createTimeDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
      this.changeUserNumberDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
      this.changeTimeDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
      this.disableUserNumberDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
      this.disableTimeDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
      this.toolStrip.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)(this.workflowInfoListDataGridView)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.workflowInfoListBindingSource)).BeginInit();
      this.SuspendLayout();
      // 
      // toolStrip
      // 
      this.toolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.typeNameToolStripLabel,
            this.typeNameToolStripTextBox,
            this.cancelToolStripButton,
            this.okToolStripButton});
      this.toolStrip.Location = new System.Drawing.Point(0, 0);
      this.toolStrip.Name = "toolStrip";
      this.toolStrip.Size = new System.Drawing.Size(833, 25);
      this.toolStrip.TabIndex = 0;
      this.toolStrip.Text = "toolStrip1";
      // 
      // typeNameToolStripLabel
      // 
      this.typeNameToolStripLabel.Name = "typeNameToolStripLabel";
      this.typeNameToolStripLabel.Size = new System.Drawing.Size(56, 22);
      this.typeNameToolStripLabel.Text = "类型名称";
      // 
      // typeNameToolStripTextBox
      // 
      this.typeNameToolStripTextBox.Name = "typeNameToolStripTextBox";
      this.typeNameToolStripTextBox.Size = new System.Drawing.Size(220, 25);
      this.typeNameToolStripTextBox.TextChanged += new System.EventHandler(this.typeNameToolStripTextBox_TextChanged);
      // 
      // cancelToolStripButton
      // 
      this.cancelToolStripButton.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
      this.cancelToolStripButton.AutoSize = false;
      this.cancelToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
      this.cancelToolStripButton.Image = ((System.Drawing.Image)(resources.GetObject("cancelToolStripButton.Image")));
      this.cancelToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
      this.cancelToolStripButton.Name = "cancelToolStripButton";
      this.cancelToolStripButton.Size = new System.Drawing.Size(80, 22);
      this.cancelToolStripButton.Text = "取消";
      this.cancelToolStripButton.Click += new System.EventHandler(this.cancelToolStripButton_Click);
      // 
      // okToolStripButton
      // 
      this.okToolStripButton.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
      this.okToolStripButton.AutoSize = false;
      this.okToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
      this.okToolStripButton.Image = ((System.Drawing.Image)(resources.GetObject("okToolStripButton.Image")));
      this.okToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
      this.okToolStripButton.Name = "okToolStripButton";
      this.okToolStripButton.Size = new System.Drawing.Size(80, 22);
      this.okToolStripButton.Text = "确认";
      this.okToolStripButton.Click += new System.EventHandler(this.okToolStripButton_Click);
      // 
      // workflowInfoListDataGridView
      // 
      this.workflowInfoListDataGridView.AllowUserToAddRows = false;
      this.workflowInfoListDataGridView.AllowUserToDeleteRows = false;
      this.workflowInfoListDataGridView.AllowUserToOrderColumns = true;
      this.workflowInfoListDataGridView.AutoGenerateColumns = false;
      this.workflowInfoListDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
      this.workflowInfoListDataGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.typeNameDataGridViewTextBoxColumn,
            this.captionDataGridViewTextBoxColumn,
            this.typeNamespaceDataGridViewTextBoxColumn,
            this.createUserNumberDataGridViewTextBoxColumn,
            this.createTimeDataGridViewTextBoxColumn,
            this.changeUserNumberDataGridViewTextBoxColumn,
            this.changeTimeDataGridViewTextBoxColumn,
            this.disableUserNumberDataGridViewTextBoxColumn,
            this.disableTimeDataGridViewTextBoxColumn});
      this.workflowInfoListDataGridView.DataSource = this.workflowInfoListBindingSource;
      this.workflowInfoListDataGridView.Dock = System.Windows.Forms.DockStyle.Fill;
      this.workflowInfoListDataGridView.Location = new System.Drawing.Point(0, 25);
      this.workflowInfoListDataGridView.MultiSelect = false;
      this.workflowInfoListDataGridView.Name = "workflowInfoListDataGridView";
      this.workflowInfoListDataGridView.RowHeadersWidth = 4;
      this.workflowInfoListDataGridView.RowTemplate.Height = 23;
      this.workflowInfoListDataGridView.Size = new System.Drawing.Size(833, 467);
      this.workflowInfoListDataGridView.TabIndex = 1;
      this.workflowInfoListDataGridView.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.workflowInfoListDataGridView_CellFormatting);
      this.workflowInfoListDataGridView.DoubleClick += new System.EventHandler(this.workflowInfoListDataGridView_DoubleClick);
      // 
      // workflowInfoListBindingSource
      // 
      this.workflowInfoListBindingSource.DataSource = typeof(Phenix.Core.Workflow.WorkflowInfo);
      this.workflowInfoListBindingSource.DataSourceChanged += new System.EventHandler(this.workflowInfoListBindingSource_DataSourceChanged);
      this.workflowInfoListBindingSource.PositionChanged += new System.EventHandler(this.workflowInfoListBindingSource_PositionChanged);
      // 
      // typeNameDataGridViewTextBoxColumn
      // 
      this.typeNameDataGridViewTextBoxColumn.DataPropertyName = "TypeName";
      this.typeNameDataGridViewTextBoxColumn.HeaderText = "类型名称";
      this.typeNameDataGridViewTextBoxColumn.Name = "typeNameDataGridViewTextBoxColumn";
      this.typeNameDataGridViewTextBoxColumn.ReadOnly = true;
      this.typeNameDataGridViewTextBoxColumn.Width = 280;
      // 
      // captionDataGridViewTextBoxColumn
      // 
      this.captionDataGridViewTextBoxColumn.DataPropertyName = "Caption";
      this.captionDataGridViewTextBoxColumn.HeaderText = "标签";
      this.captionDataGridViewTextBoxColumn.Name = "captionDataGridViewTextBoxColumn";
      this.captionDataGridViewTextBoxColumn.ReadOnly = true;
      this.captionDataGridViewTextBoxColumn.Width = 220;
      // 
      // typeNamespaceDataGridViewTextBoxColumn
      // 
      this.typeNamespaceDataGridViewTextBoxColumn.DataPropertyName = "TypeNamespace";
      this.typeNamespaceDataGridViewTextBoxColumn.HeaderText = "命名空间";
      this.typeNamespaceDataGridViewTextBoxColumn.Name = "typeNamespaceDataGridViewTextBoxColumn";
      this.typeNamespaceDataGridViewTextBoxColumn.ReadOnly = true;
      // 
      // createUserNumberDataGridViewTextBoxColumn
      // 
      this.createUserNumberDataGridViewTextBoxColumn.DataPropertyName = "CreateUserNumber";
      this.createUserNumberDataGridViewTextBoxColumn.HeaderText = "构建工号";
      this.createUserNumberDataGridViewTextBoxColumn.Name = "createUserNumberDataGridViewTextBoxColumn";
      this.createUserNumberDataGridViewTextBoxColumn.ReadOnly = true;
      // 
      // createTimeDataGridViewTextBoxColumn
      // 
      this.createTimeDataGridViewTextBoxColumn.DataPropertyName = "CreateTime";
      this.createTimeDataGridViewTextBoxColumn.HeaderText = "构建时间";
      this.createTimeDataGridViewTextBoxColumn.Name = "createTimeDataGridViewTextBoxColumn";
      this.createTimeDataGridViewTextBoxColumn.ReadOnly = true;
      // 
      // changeUserNumberDataGridViewTextBoxColumn
      // 
      this.changeUserNumberDataGridViewTextBoxColumn.DataPropertyName = "ChangeUserNumber";
      this.changeUserNumberDataGridViewTextBoxColumn.HeaderText = "更新工号";
      this.changeUserNumberDataGridViewTextBoxColumn.Name = "changeUserNumberDataGridViewTextBoxColumn";
      this.changeUserNumberDataGridViewTextBoxColumn.ReadOnly = true;
      // 
      // changeTimeDataGridViewTextBoxColumn
      // 
      this.changeTimeDataGridViewTextBoxColumn.DataPropertyName = "ChangeTime";
      this.changeTimeDataGridViewTextBoxColumn.HeaderText = "更新时间";
      this.changeTimeDataGridViewTextBoxColumn.Name = "changeTimeDataGridViewTextBoxColumn";
      this.changeTimeDataGridViewTextBoxColumn.ReadOnly = true;
      // 
      // disableUserNumberDataGridViewTextBoxColumn
      // 
      this.disableUserNumberDataGridViewTextBoxColumn.DataPropertyName = "DisableUserNumber";
      this.disableUserNumberDataGridViewTextBoxColumn.HeaderText = "禁用工号";
      this.disableUserNumberDataGridViewTextBoxColumn.Name = "disableUserNumberDataGridViewTextBoxColumn";
      this.disableUserNumberDataGridViewTextBoxColumn.ReadOnly = true;
      // 
      // disableTimeDataGridViewTextBoxColumn
      // 
      this.disableTimeDataGridViewTextBoxColumn.DataPropertyName = "DisableTime";
      this.disableTimeDataGridViewTextBoxColumn.HeaderText = "禁用时间";
      this.disableTimeDataGridViewTextBoxColumn.Name = "disableTimeDataGridViewTextBoxColumn";
      this.disableTimeDataGridViewTextBoxColumn.ReadOnly = true;
      // 
      // WorkflowInfoSelectDialog
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(833, 492);
      this.Controls.Add(this.workflowInfoListDataGridView);
      this.Controls.Add(this.toolStrip);
      this.Name = "WorkflowInfoSelectDialog";
      this.Text = "选择需编辑的工作流";
      this.toolStrip.ResumeLayout(false);
      this.toolStrip.PerformLayout();
      ((System.ComponentModel.ISupportInitialize)(this.workflowInfoListDataGridView)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.workflowInfoListBindingSource)).EndInit();
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.ToolStrip toolStrip;
    private System.Windows.Forms.ToolStripLabel typeNameToolStripLabel;
    private System.Windows.Forms.ToolStripTextBox typeNameToolStripTextBox;
    private System.Windows.Forms.ToolStripButton okToolStripButton;
    private System.Windows.Forms.ToolStripButton cancelToolStripButton;
    private System.Windows.Forms.DataGridView workflowInfoListDataGridView;
    private System.Windows.Forms.BindingSource workflowInfoListBindingSource;
    private System.Windows.Forms.DataGridViewTextBoxColumn typeNameDataGridViewTextBoxColumn;
    private System.Windows.Forms.DataGridViewTextBoxColumn captionDataGridViewTextBoxColumn;
    private System.Windows.Forms.DataGridViewTextBoxColumn typeNamespaceDataGridViewTextBoxColumn;
    private System.Windows.Forms.DataGridViewTextBoxColumn createUserNumberDataGridViewTextBoxColumn;
    private System.Windows.Forms.DataGridViewTextBoxColumn createTimeDataGridViewTextBoxColumn;
    private System.Windows.Forms.DataGridViewTextBoxColumn changeUserNumberDataGridViewTextBoxColumn;
    private System.Windows.Forms.DataGridViewTextBoxColumn changeTimeDataGridViewTextBoxColumn;
    private System.Windows.Forms.DataGridViewTextBoxColumn disableUserNumberDataGridViewTextBoxColumn;
    private System.Windows.Forms.DataGridViewTextBoxColumn disableTimeDataGridViewTextBoxColumn;
  }
}