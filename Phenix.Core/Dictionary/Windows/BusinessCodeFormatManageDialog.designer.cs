namespace Phenix.Core.Dictionary.Windows
{
  partial class BusinessCodeFormatManageDialog
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
      System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(BusinessCodeFormatManageDialog));
      this.businessCodeFormatsBindingSource = new System.Windows.Forms.BindingSource(this.components);
      this.businessCodeFormatBindingNavigator = new System.Windows.Forms.BindingNavigator(this.components);
      this.bindingNavigatorCountItem = new System.Windows.Forms.ToolStripLabel();
      this.bindingNavigatorMoveFirstItem = new System.Windows.Forms.ToolStripButton();
      this.bindingNavigatorMovePreviousItem = new System.Windows.Forms.ToolStripButton();
      this.bindingNavigatorSeparator = new System.Windows.Forms.ToolStripSeparator();
      this.filterToolStripTextBox = new System.Windows.Forms.ToolStripTextBox();
      this.bindingNavigatorPositionItem = new System.Windows.Forms.ToolStripTextBox();
      this.bindingNavigatorSeparator1 = new System.Windows.Forms.ToolStripSeparator();
      this.bindingNavigatorMoveNextItem = new System.Windows.Forms.ToolStripButton();
      this.bindingNavigatorMoveLastItem = new System.Windows.Forms.ToolStripButton();
      this.bindingNavigatorSeparator2 = new System.Windows.Forms.ToolStripSeparator();
      this.addBusinessCodeFormatToolStripButton = new System.Windows.Forms.ToolStripButton();
      this.editBusinessCodeFormatToolStripButton = new System.Windows.Forms.ToolStripButton();
      this.deleteBusinessCodeFormatToolStripButton = new System.Windows.Forms.ToolStripButton();
      this.bindingNavigatorSeparator3 = new System.Windows.Forms.ToolStripSeparator();
      this.exitToolStripButton = new System.Windows.Forms.ToolStripButton();
      this.businessCodeFormatDataGridView = new System.Windows.Forms.DataGridView();
      this.statusStrip = new System.Windows.Forms.StatusStrip();
      this.hintToolStripStatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
      this.businessCodeNameTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
      this.captionTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
      this.formatStringTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
      this.fillOnSavingCheckBoxColumn = new System.Windows.Forms.DataGridViewCheckBoxColumn();
      ((System.ComponentModel.ISupportInitialize)(this.businessCodeFormatsBindingSource)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.businessCodeFormatBindingNavigator)).BeginInit();
      this.businessCodeFormatBindingNavigator.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)(this.businessCodeFormatDataGridView)).BeginInit();
      this.statusStrip.SuspendLayout();
      this.SuspendLayout();
      // 
      // businessCodeFormatsBindingSource
      // 
      this.businessCodeFormatsBindingSource.DataSource = typeof(Phenix.Core.Dictionary.BusinessCodeFormat);
      this.businessCodeFormatsBindingSource.PositionChanged += new System.EventHandler(this.businessCodeFormatBindingSource_PositionChanged);
      // 
      // businessCodeFormatBindingNavigator
      // 
      this.businessCodeFormatBindingNavigator.AddNewItem = null;
      this.businessCodeFormatBindingNavigator.BindingSource = this.businessCodeFormatsBindingSource;
      this.businessCodeFormatBindingNavigator.CountItem = this.bindingNavigatorCountItem;
      this.businessCodeFormatBindingNavigator.DeleteItem = null;
      this.businessCodeFormatBindingNavigator.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.bindingNavigatorMoveFirstItem,
            this.bindingNavigatorMovePreviousItem,
            this.bindingNavigatorSeparator,
            this.filterToolStripTextBox,
            this.bindingNavigatorPositionItem,
            this.bindingNavigatorCountItem,
            this.bindingNavigatorSeparator1,
            this.bindingNavigatorMoveNextItem,
            this.bindingNavigatorMoveLastItem,
            this.bindingNavigatorSeparator2,
            this.addBusinessCodeFormatToolStripButton,
            this.editBusinessCodeFormatToolStripButton,
            this.deleteBusinessCodeFormatToolStripButton,
            this.bindingNavigatorSeparator3,
            this.exitToolStripButton});
      this.businessCodeFormatBindingNavigator.Location = new System.Drawing.Point(0, 0);
      this.businessCodeFormatBindingNavigator.MoveFirstItem = this.bindingNavigatorMoveFirstItem;
      this.businessCodeFormatBindingNavigator.MoveLastItem = this.bindingNavigatorMoveLastItem;
      this.businessCodeFormatBindingNavigator.MoveNextItem = this.bindingNavigatorMoveNextItem;
      this.businessCodeFormatBindingNavigator.MovePreviousItem = this.bindingNavigatorMovePreviousItem;
      this.businessCodeFormatBindingNavigator.Name = "businessCodeFormatBindingNavigator";
      this.businessCodeFormatBindingNavigator.PositionItem = this.bindingNavigatorPositionItem;
      this.businessCodeFormatBindingNavigator.Size = new System.Drawing.Size(894, 25);
      this.businessCodeFormatBindingNavigator.TabIndex = 0;
      // 
      // bindingNavigatorCountItem
      // 
      this.bindingNavigatorCountItem.Name = "bindingNavigatorCountItem";
      this.bindingNavigatorCountItem.Size = new System.Drawing.Size(32, 22);
      this.bindingNavigatorCountItem.Text = "/ {0}";
      this.bindingNavigatorCountItem.ToolTipText = "总项数";
      // 
      // bindingNavigatorMoveFirstItem
      // 
      this.bindingNavigatorMoveFirstItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
      this.bindingNavigatorMoveFirstItem.Image = ((System.Drawing.Image)(resources.GetObject("bindingNavigatorMoveFirstItem.Image")));
      this.bindingNavigatorMoveFirstItem.Name = "bindingNavigatorMoveFirstItem";
      this.bindingNavigatorMoveFirstItem.RightToLeftAutoMirrorImage = true;
      this.bindingNavigatorMoveFirstItem.Size = new System.Drawing.Size(23, 22);
      this.bindingNavigatorMoveFirstItem.Text = "移到第一条记录";
      // 
      // bindingNavigatorMovePreviousItem
      // 
      this.bindingNavigatorMovePreviousItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
      this.bindingNavigatorMovePreviousItem.Image = ((System.Drawing.Image)(resources.GetObject("bindingNavigatorMovePreviousItem.Image")));
      this.bindingNavigatorMovePreviousItem.Name = "bindingNavigatorMovePreviousItem";
      this.bindingNavigatorMovePreviousItem.RightToLeftAutoMirrorImage = true;
      this.bindingNavigatorMovePreviousItem.Size = new System.Drawing.Size(23, 22);
      this.bindingNavigatorMovePreviousItem.Text = "移到上一条记录";
      // 
      // bindingNavigatorSeparator
      // 
      this.bindingNavigatorSeparator.Name = "bindingNavigatorSeparator";
      this.bindingNavigatorSeparator.Size = new System.Drawing.Size(6, 25);
      // 
      // filterToolStripTextBox
      // 
      this.filterToolStripTextBox.Name = "filterToolStripTextBox";
      this.filterToolStripTextBox.Size = new System.Drawing.Size(400, 25);
      this.filterToolStripTextBox.TextChanged += new System.EventHandler(this.filterToolStripTextBox_TextChanged);
      // 
      // bindingNavigatorPositionItem
      // 
      this.bindingNavigatorPositionItem.AccessibleName = "位置";
      this.bindingNavigatorPositionItem.AutoSize = false;
      this.bindingNavigatorPositionItem.Name = "bindingNavigatorPositionItem";
      this.bindingNavigatorPositionItem.Size = new System.Drawing.Size(50, 23);
      this.bindingNavigatorPositionItem.Text = "0";
      this.bindingNavigatorPositionItem.ToolTipText = "当前位置";
      // 
      // bindingNavigatorSeparator1
      // 
      this.bindingNavigatorSeparator1.Name = "bindingNavigatorSeparator1";
      this.bindingNavigatorSeparator1.Size = new System.Drawing.Size(6, 25);
      // 
      // bindingNavigatorMoveNextItem
      // 
      this.bindingNavigatorMoveNextItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
      this.bindingNavigatorMoveNextItem.Image = ((System.Drawing.Image)(resources.GetObject("bindingNavigatorMoveNextItem.Image")));
      this.bindingNavigatorMoveNextItem.Name = "bindingNavigatorMoveNextItem";
      this.bindingNavigatorMoveNextItem.RightToLeftAutoMirrorImage = true;
      this.bindingNavigatorMoveNextItem.Size = new System.Drawing.Size(23, 22);
      this.bindingNavigatorMoveNextItem.Text = "移到下一条记录";
      // 
      // bindingNavigatorMoveLastItem
      // 
      this.bindingNavigatorMoveLastItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
      this.bindingNavigatorMoveLastItem.Image = ((System.Drawing.Image)(resources.GetObject("bindingNavigatorMoveLastItem.Image")));
      this.bindingNavigatorMoveLastItem.Name = "bindingNavigatorMoveLastItem";
      this.bindingNavigatorMoveLastItem.RightToLeftAutoMirrorImage = true;
      this.bindingNavigatorMoveLastItem.Size = new System.Drawing.Size(23, 22);
      this.bindingNavigatorMoveLastItem.Text = "移到最后一条记录";
      // 
      // bindingNavigatorSeparator2
      // 
      this.bindingNavigatorSeparator2.Name = "bindingNavigatorSeparator2";
      this.bindingNavigatorSeparator2.Size = new System.Drawing.Size(6, 25);
      // 
      // addBusinessCodeFormatToolStripButton
      // 
      this.addBusinessCodeFormatToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
      this.addBusinessCodeFormatToolStripButton.Name = "addBusinessCodeFormatToolStripButton";
      this.addBusinessCodeFormatToolStripButton.Size = new System.Drawing.Size(36, 22);
      this.addBusinessCodeFormatToolStripButton.Text = "新增";
      this.addBusinessCodeFormatToolStripButton.Click += new System.EventHandler(this.addBusinessCodeFormatToolStripButton_Click);
      // 
      // editBusinessCodeFormatToolStripButton
      // 
      this.editBusinessCodeFormatToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
      this.editBusinessCodeFormatToolStripButton.Name = "editBusinessCodeFormatToolStripButton";
      this.editBusinessCodeFormatToolStripButton.Size = new System.Drawing.Size(36, 22);
      this.editBusinessCodeFormatToolStripButton.Text = "编辑";
      this.editBusinessCodeFormatToolStripButton.Click += new System.EventHandler(this.editBusinessCodeFormatToolStripButton_Click);
      // 
      // deleteBusinessCodeFormatToolStripButton
      // 
      this.deleteBusinessCodeFormatToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
      this.deleteBusinessCodeFormatToolStripButton.Name = "deleteBusinessCodeFormatToolStripButton";
      this.deleteBusinessCodeFormatToolStripButton.Size = new System.Drawing.Size(36, 22);
      this.deleteBusinessCodeFormatToolStripButton.Text = "删除";
      this.deleteBusinessCodeFormatToolStripButton.Click += new System.EventHandler(this.deleteBusinessCodeFormatToolStripButton_Click);
      // 
      // bindingNavigatorSeparator3
      // 
      this.bindingNavigatorSeparator3.Name = "bindingNavigatorSeparator3";
      this.bindingNavigatorSeparator3.Size = new System.Drawing.Size(6, 25);
      // 
      // exitToolStripButton
      // 
      this.exitToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
      this.exitToolStripButton.Name = "exitToolStripButton";
      this.exitToolStripButton.Size = new System.Drawing.Size(36, 22);
      this.exitToolStripButton.Text = "关闭";
      this.exitToolStripButton.Click += new System.EventHandler(this.exitToolStripButton_Click);
      // 
      // businessCodeFormatDataGridView
      // 
      this.businessCodeFormatDataGridView.AllowUserToAddRows = false;
      this.businessCodeFormatDataGridView.AllowUserToDeleteRows = false;
      this.businessCodeFormatDataGridView.AutoGenerateColumns = false;
      this.businessCodeFormatDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
      this.businessCodeFormatDataGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.businessCodeNameTextBoxColumn,
            this.captionTextBoxColumn,
            this.formatStringTextBoxColumn,
            this.fillOnSavingCheckBoxColumn});
      this.businessCodeFormatDataGridView.DataSource = this.businessCodeFormatsBindingSource;
      this.businessCodeFormatDataGridView.Dock = System.Windows.Forms.DockStyle.Fill;
      this.businessCodeFormatDataGridView.Location = new System.Drawing.Point(0, 25);
      this.businessCodeFormatDataGridView.MultiSelect = false;
      this.businessCodeFormatDataGridView.Name = "businessCodeFormatDataGridView";
      this.businessCodeFormatDataGridView.ReadOnly = true;
      this.businessCodeFormatDataGridView.RowHeadersWidth = 4;
      this.businessCodeFormatDataGridView.RowTemplate.Height = 23;
      this.businessCodeFormatDataGridView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
      this.businessCodeFormatDataGridView.Size = new System.Drawing.Size(894, 447);
      this.businessCodeFormatDataGridView.TabIndex = 1;
      this.businessCodeFormatDataGridView.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.businessCodeFormatDataGridView_MouseDoubleClick);
      // 
      // statusStrip
      // 
      this.statusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.hintToolStripStatusLabel});
      this.statusStrip.Location = new System.Drawing.Point(0, 450);
      this.statusStrip.Name = "statusStrip";
      this.statusStrip.Size = new System.Drawing.Size(894, 22);
      this.statusStrip.TabIndex = 2;
      this.statusStrip.Text = "statusStrip1";
      // 
      // hintToolStripStatusLabel
      // 
      this.hintToolStripStatusLabel.Name = "hintToolStripStatusLabel";
      this.hintToolStripStatusLabel.Size = new System.Drawing.Size(879, 17);
      this.hintToolStripStatusLabel.Spring = true;
      this.hintToolStripStatusLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
      // 
      // businessCodeNameTextBoxColumn
      // 
      this.businessCodeNameTextBoxColumn.DataPropertyName = "BusinessCodeName";
      this.businessCodeNameTextBoxColumn.Frozen = true;
      this.businessCodeNameTextBoxColumn.HeaderText = "名称";
      this.businessCodeNameTextBoxColumn.Name = "businessCodeNameTextBoxColumn";
      this.businessCodeNameTextBoxColumn.ReadOnly = true;
      this.businessCodeNameTextBoxColumn.Width = 260;
      // 
      // captionTextBoxColumn
      // 
      this.captionTextBoxColumn.DataPropertyName = "Caption";
      this.captionTextBoxColumn.Frozen = true;
      this.captionTextBoxColumn.HeaderText = "标签";
      this.captionTextBoxColumn.Name = "captionTextBoxColumn";
      this.captionTextBoxColumn.ReadOnly = true;
      this.captionTextBoxColumn.Width = 150;
      // 
      // formatStringTextBoxColumn
      // 
      this.formatStringTextBoxColumn.DataPropertyName = "FormatString";
      this.formatStringTextBoxColumn.HeaderText = "格式字符串";
      this.formatStringTextBoxColumn.Name = "formatStringTextBoxColumn";
      this.formatStringTextBoxColumn.ReadOnly = true;
      this.formatStringTextBoxColumn.Width = 320;
      // 
      // fillOnSavingCheckBoxColumn
      // 
      this.fillOnSavingCheckBoxColumn.DataPropertyName = "FillOnSaving";
      this.fillOnSavingCheckBoxColumn.HeaderText = "是否提交时填充值";
      this.fillOnSavingCheckBoxColumn.Name = "FillOnSavingCheckBoxColumn";
      this.fillOnSavingCheckBoxColumn.ReadOnly = true;
      this.fillOnSavingCheckBoxColumn.Width = 130;
      // 
      // BusinessCodeFormatManageDialog
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(894, 472);
      this.Controls.Add(this.statusStrip);
      this.Controls.Add(this.businessCodeFormatDataGridView);
      this.Controls.Add(this.businessCodeFormatBindingNavigator);
      this.Name = "BusinessCodeFormatManageDialog";
      this.Text = "管理业务码格式";
      this.Shown += new System.EventHandler(this.BusinessCodeFormatManageDialog_Shown);
      ((System.ComponentModel.ISupportInitialize)(this.businessCodeFormatsBindingSource)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.businessCodeFormatBindingNavigator)).EndInit();
      this.businessCodeFormatBindingNavigator.ResumeLayout(false);
      this.businessCodeFormatBindingNavigator.PerformLayout();
      ((System.ComponentModel.ISupportInitialize)(this.businessCodeFormatDataGridView)).EndInit();
      this.statusStrip.ResumeLayout(false);
      this.statusStrip.PerformLayout();
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.BindingSource businessCodeFormatsBindingSource;
    private System.Windows.Forms.BindingNavigator businessCodeFormatBindingNavigator;
    private System.Windows.Forms.ToolStripLabel bindingNavigatorCountItem;
    private System.Windows.Forms.ToolStripButton bindingNavigatorMoveFirstItem;
    private System.Windows.Forms.ToolStripButton bindingNavigatorMovePreviousItem;
    private System.Windows.Forms.ToolStripSeparator bindingNavigatorSeparator;
    private System.Windows.Forms.ToolStripTextBox bindingNavigatorPositionItem;
    private System.Windows.Forms.ToolStripSeparator bindingNavigatorSeparator1;
    private System.Windows.Forms.ToolStripButton bindingNavigatorMoveNextItem;
    private System.Windows.Forms.ToolStripButton bindingNavigatorMoveLastItem;
    private System.Windows.Forms.ToolStripSeparator bindingNavigatorSeparator2;
    private System.Windows.Forms.DataGridView businessCodeFormatDataGridView;
    private System.Windows.Forms.StatusStrip statusStrip;
    private System.Windows.Forms.ToolStripStatusLabel hintToolStripStatusLabel;
    private System.Windows.Forms.ToolStripButton editBusinessCodeFormatToolStripButton;
    private System.Windows.Forms.ToolStripButton exitToolStripButton;
    private System.Windows.Forms.ToolStripSeparator bindingNavigatorSeparator3;
    private System.Windows.Forms.ToolStripButton addBusinessCodeFormatToolStripButton;
    private System.Windows.Forms.ToolStripButton deleteBusinessCodeFormatToolStripButton;
    private System.Windows.Forms.ToolStripTextBox filterToolStripTextBox;
    private System.Windows.Forms.DataGridViewTextBoxColumn businessCodeNameTextBoxColumn;
    private System.Windows.Forms.DataGridViewTextBoxColumn captionTextBoxColumn;
    private System.Windows.Forms.DataGridViewTextBoxColumn formatStringTextBoxColumn;
    private System.Windows.Forms.DataGridViewCheckBoxColumn fillOnSavingCheckBoxColumn;
  }
}