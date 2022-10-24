namespace Phenix.Core.Dictionary.Windows
{
  partial class BusinessCodeFormatEditDialog
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
      this.businessCodeFormatBindingSource = new System.Windows.Forms.BindingSource(this.components);
      this.formatStringRichTextBox = new System.Windows.Forms.RichTextBox();
      this.fillOnSavingCheckBox = new System.Windows.Forms.CheckBox();
      this.cancelButton = new System.Windows.Forms.Button();
      this.okButton = new System.Windows.Forms.Button();
      this.addBusinessCodeSerialFormatButton = new System.Windows.Forms.Button();
      this.lengthYearButton = new System.Windows.Forms.Button();
      this.shortYearButton = new System.Windows.Forms.Button();
      this.monthButton = new System.Windows.Forms.Button();
      this.dayButton = new System.Windows.Forms.Button();
      this.departmentButton = new System.Windows.Forms.Button();
      this.userNumberButton = new System.Windows.Forms.Button();
      this.criteriaPropertyNameLabel = new System.Windows.Forms.Label();
      this.criteriaPropertyValueTextBox = new System.Windows.Forms.TextBox();
      this.criteriaPropertyInfoGroupBox = new System.Windows.Forms.GroupBox();
      this.criteriaPropertyInfoSelectGroupBox = new System.Windows.Forms.GroupBox();
      this.criteriaPropertyValueComboBox = new System.Windows.Forms.ComboBox();
      this.criteriaPropertySelectValuesBindingSource = new System.Windows.Forms.BindingSource(this.components);
      this.criteriaPropertyFriendlyNameLabel = new System.Windows.Forms.Label();
      this.addBusinessCodePlaceholderFormatButton = new System.Windows.Forms.Button();
      this.addButton = new System.Windows.Forms.Button();
      ((System.ComponentModel.ISupportInitialize)(this.businessCodeFormatBindingSource)).BeginInit();
      this.criteriaPropertyInfoGroupBox.SuspendLayout();
      this.criteriaPropertyInfoSelectGroupBox.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)(this.criteriaPropertySelectValuesBindingSource)).BeginInit();
      this.SuspendLayout();
      // 
      // businessCodeFormatBindingSource
      // 
      this.businessCodeFormatBindingSource.DataSource = typeof(Phenix.Core.Dictionary.BusinessCodeFormat);
      // 
      // formatStringRichTextBox
      // 
      this.formatStringRichTextBox.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.businessCodeFormatBindingSource, "FormatString", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
      this.formatStringRichTextBox.Location = new System.Drawing.Point(15, 41);
      this.formatStringRichTextBox.Name = "formatStringRichTextBox";
      this.formatStringRichTextBox.Size = new System.Drawing.Size(318, 137);
      this.formatStringRichTextBox.TabIndex = 1;
      this.formatStringRichTextBox.Text = "";
      this.formatStringRichTextBox.TextChanged += new System.EventHandler(this.formatStringRichTextBox_TextChanged);
      // 
      // fillOnSavingCheckBox
      // 
      this.fillOnSavingCheckBox.DataBindings.Add(new System.Windows.Forms.Binding("CheckState", this.businessCodeFormatBindingSource, "FillOnSaving", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
      this.fillOnSavingCheckBox.Location = new System.Drawing.Point(15, 12);
      this.fillOnSavingCheckBox.Name = "fillOnSavingCheckBox";
      this.fillOnSavingCheckBox.Size = new System.Drawing.Size(318, 24);
      this.fillOnSavingCheckBox.TabIndex = 0;
      this.fillOnSavingCheckBox.Text = "是否提交时填充值";
      this.fillOnSavingCheckBox.UseVisualStyleBackColor = true;
      // 
      // cancelButton
      // 
      this.cancelButton.DataBindings.Add(new System.Windows.Forms.Binding("Text", global::Phenix.Core.Properties.Settings.Default, "Cancel", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
      this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
      this.cancelButton.Location = new System.Drawing.Point(349, 41);
      this.cancelButton.Name = "cancelButton";
      this.cancelButton.Size = new System.Drawing.Size(90, 23);
      this.cancelButton.TabIndex = 13;
      this.cancelButton.Text = global::Phenix.Core.Properties.Settings.Default.Cancel;
      this.cancelButton.UseVisualStyleBackColor = true;
      // 
      // okButton
      // 
      this.okButton.DataBindings.Add(new System.Windows.Forms.Binding("Text", global::Phenix.Core.Properties.Settings.Default, "Ok", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
      this.okButton.DialogResult = System.Windows.Forms.DialogResult.OK;
      this.okButton.Location = new System.Drawing.Point(349, 12);
      this.okButton.Name = "okButton";
      this.okButton.Size = new System.Drawing.Size(90, 23);
      this.okButton.TabIndex = 12;
      this.okButton.Text = global::Phenix.Core.Properties.Settings.Default.Ok;
      this.okButton.UseVisualStyleBackColor = true;
      // 
      // addBusinessCodeSerialFormatButton
      // 
      this.addBusinessCodeSerialFormatButton.Location = new System.Drawing.Point(15, 184);
      this.addBusinessCodeSerialFormatButton.Name = "addBusinessCodeSerialFormatButton";
      this.addBusinessCodeSerialFormatButton.Size = new System.Drawing.Size(75, 23);
      this.addBusinessCodeSerialFormatButton.TabIndex = 2;
      this.addBusinessCodeSerialFormatButton.Text = "流水号";
      this.addBusinessCodeSerialFormatButton.UseVisualStyleBackColor = true;
      this.addBusinessCodeSerialFormatButton.Click += new System.EventHandler(this.addBusinessCodeSerialFormatButton_Click);
      // 
      // lengthYearButton
      // 
      this.lengthYearButton.Location = new System.Drawing.Point(15, 213);
      this.lengthYearButton.Name = "lengthYearButton";
      this.lengthYearButton.Size = new System.Drawing.Size(75, 23);
      this.lengthYearButton.TabIndex = 4;
      this.lengthYearButton.Text = "长年";
      this.lengthYearButton.UseVisualStyleBackColor = true;
      this.lengthYearButton.Click += new System.EventHandler(this.lengthYearButton_Click);
      // 
      // shortYearButton
      // 
      this.shortYearButton.Location = new System.Drawing.Point(96, 213);
      this.shortYearButton.Name = "shortYearButton";
      this.shortYearButton.Size = new System.Drawing.Size(75, 23);
      this.shortYearButton.TabIndex = 5;
      this.shortYearButton.Text = "短年";
      this.shortYearButton.UseVisualStyleBackColor = true;
      this.shortYearButton.Click += new System.EventHandler(this.shortYearButton_Click);
      // 
      // monthButton
      // 
      this.monthButton.Location = new System.Drawing.Point(177, 213);
      this.monthButton.Name = "monthButton";
      this.monthButton.Size = new System.Drawing.Size(75, 23);
      this.monthButton.TabIndex = 6;
      this.monthButton.Text = "月";
      this.monthButton.UseVisualStyleBackColor = true;
      this.monthButton.Click += new System.EventHandler(this.monthButton_Click);
      // 
      // dayButton
      // 
      this.dayButton.Location = new System.Drawing.Point(258, 213);
      this.dayButton.Name = "dayButton";
      this.dayButton.Size = new System.Drawing.Size(75, 23);
      this.dayButton.TabIndex = 7;
      this.dayButton.Text = "日";
      this.dayButton.UseVisualStyleBackColor = true;
      this.dayButton.Click += new System.EventHandler(this.dayButton_Click);
      // 
      // departmentButton
      // 
      this.departmentButton.Location = new System.Drawing.Point(15, 242);
      this.departmentButton.Name = "departmentButton";
      this.departmentButton.Size = new System.Drawing.Size(75, 23);
      this.departmentButton.TabIndex = 8;
      this.departmentButton.Text = "部门";
      this.departmentButton.UseVisualStyleBackColor = true;
      this.departmentButton.Click += new System.EventHandler(this.departmentButton_Click);
      // 
      // userNumberButton
      // 
      this.userNumberButton.Location = new System.Drawing.Point(96, 242);
      this.userNumberButton.Name = "userNumberButton";
      this.userNumberButton.Size = new System.Drawing.Size(75, 23);
      this.userNumberButton.TabIndex = 9;
      this.userNumberButton.Text = "工号";
      this.userNumberButton.UseVisualStyleBackColor = true;
      this.userNumberButton.Click += new System.EventHandler(this.userNumberButton_Click);
      // 
      // criteriaPropertyNameLabel
      // 
      this.criteriaPropertyNameLabel.AutoSize = true;
      this.criteriaPropertyNameLabel.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.businessCodeFormatBindingSource, "CriteriaPropertyName", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
      this.criteriaPropertyNameLabel.Location = new System.Drawing.Point(6, 27);
      this.criteriaPropertyNameLabel.Name = "criteriaPropertyNameLabel";
      this.criteriaPropertyNameLabel.Size = new System.Drawing.Size(125, 12);
      this.criteriaPropertyNameLabel.TabIndex = 0;
      this.criteriaPropertyNameLabel.Text = "CriteriaPropertyName";
      // 
      // criteriaPropertyValueTextBox
      // 
      this.criteriaPropertyValueTextBox.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.businessCodeFormatBindingSource, "CriteriaPropertyValue", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
      this.criteriaPropertyValueTextBox.Location = new System.Drawing.Point(8, 42);
      this.criteriaPropertyValueTextBox.Name = "criteriaPropertyValueTextBox";
      this.criteriaPropertyValueTextBox.Size = new System.Drawing.Size(304, 21);
      this.criteriaPropertyValueTextBox.TabIndex = 1;
      this.criteriaPropertyValueTextBox.TextChanged += new System.EventHandler(this.criteriaPropertyValueTextBox_TextChanged);
      // 
      // criteriaPropertyInfoGroupBox
      // 
      this.criteriaPropertyInfoGroupBox.Controls.Add(this.criteriaPropertyNameLabel);
      this.criteriaPropertyInfoGroupBox.Controls.Add(this.criteriaPropertyValueTextBox);
      this.criteriaPropertyInfoGroupBox.Location = new System.Drawing.Point(15, 288);
      this.criteriaPropertyInfoGroupBox.Name = "criteriaPropertyInfoGroupBox";
      this.criteriaPropertyInfoGroupBox.Size = new System.Drawing.Size(318, 83);
      this.criteriaPropertyInfoGroupBox.TabIndex = 10;
      this.criteriaPropertyInfoGroupBox.TabStop = false;
      this.criteriaPropertyInfoGroupBox.Text = "当业务对象存在下述条件属性值时才应用上述格式";
      // 
      // criteriaPropertyInfoSelectGroupBox
      // 
      this.criteriaPropertyInfoSelectGroupBox.Controls.Add(this.criteriaPropertyValueComboBox);
      this.criteriaPropertyInfoSelectGroupBox.Controls.Add(this.criteriaPropertyFriendlyNameLabel);
      this.criteriaPropertyInfoSelectGroupBox.Location = new System.Drawing.Point(15, 282);
      this.criteriaPropertyInfoSelectGroupBox.Name = "criteriaPropertyInfoSelectGroupBox";
      this.criteriaPropertyInfoSelectGroupBox.Size = new System.Drawing.Size(318, 83);
      this.criteriaPropertyInfoSelectGroupBox.TabIndex = 11;
      this.criteriaPropertyInfoSelectGroupBox.TabStop = false;
      this.criteriaPropertyInfoSelectGroupBox.Text = "当业务对象存在下述条件属性值时才应用上述格式";
      // 
      // criteriaPropertyValueComboBox
      // 
      this.criteriaPropertyValueComboBox.DataBindings.Add(new System.Windows.Forms.Binding("SelectedValue", this.businessCodeFormatBindingSource, "CriteriaPropertyValue", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
      this.criteriaPropertyValueComboBox.DataSource = this.criteriaPropertySelectValuesBindingSource;
      this.criteriaPropertyValueComboBox.DisplayMember = "Caption";
      this.criteriaPropertyValueComboBox.FormattingEnabled = true;
      this.criteriaPropertyValueComboBox.Location = new System.Drawing.Point(8, 42);
      this.criteriaPropertyValueComboBox.Name = "criteriaPropertyValueComboBox";
      this.criteriaPropertyValueComboBox.Size = new System.Drawing.Size(304, 20);
      this.criteriaPropertyValueComboBox.TabIndex = 1;
      this.criteriaPropertyValueComboBox.ValueMember = "Key";
      this.criteriaPropertyValueComboBox.TextChanged += new System.EventHandler(this.criteriaPropertyValueComboBox_TextChanged);
      // 
      // criteriaPropertySelectValuesBindingSource
      // 
      this.criteriaPropertySelectValuesBindingSource.DataSource = typeof(Phenix.Core.Operate.IKeyCaption);
      // 
      // criteriaPropertyFriendlyNameLabel
      // 
      this.criteriaPropertyFriendlyNameLabel.AutoSize = true;
      this.criteriaPropertyFriendlyNameLabel.Location = new System.Drawing.Point(6, 27);
      this.criteriaPropertyFriendlyNameLabel.Name = "criteriaPropertyFriendlyNameLabel";
      this.criteriaPropertyFriendlyNameLabel.Size = new System.Drawing.Size(173, 12);
      this.criteriaPropertyFriendlyNameLabel.TabIndex = 0;
      this.criteriaPropertyFriendlyNameLabel.Text = "criteriaPropertyFriendlyName";
      // 
      // addBusinessCodePlaceholderFormatButton
      // 
      this.addBusinessCodePlaceholderFormatButton.Location = new System.Drawing.Point(96, 184);
      this.addBusinessCodePlaceholderFormatButton.Name = "addBusinessCodePlaceholderFormatButton";
      this.addBusinessCodePlaceholderFormatButton.Size = new System.Drawing.Size(75, 23);
      this.addBusinessCodePlaceholderFormatButton.TabIndex = 3;
      this.addBusinessCodePlaceholderFormatButton.Text = "占位符";
      this.addBusinessCodePlaceholderFormatButton.UseVisualStyleBackColor = true;
      this.addBusinessCodePlaceholderFormatButton.Click += new System.EventHandler(this.AddBusinessCodePlaceholderFormatButton_Click);
      // 
      // addButton
      // 
      this.addButton.DataBindings.Add(new System.Windows.Forms.Binding("Text", global::Phenix.Core.Properties.Settings.Default, "Add", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
      this.addButton.Location = new System.Drawing.Point(349, 282);
      this.addButton.Name = "addButton";
      this.addButton.Size = new System.Drawing.Size(90, 23);
      this.addButton.TabIndex = 14;
      this.addButton.Text = global::Phenix.Core.Properties.Settings.Default.Add;
      this.addButton.UseVisualStyleBackColor = true;
      this.addButton.Click += new System.EventHandler(this.addButton_Click);
      // 
      // BusinessCodeFormatEditDialog
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(456, 382);
      this.Controls.Add(this.addButton);
      this.Controls.Add(this.criteriaPropertyInfoSelectGroupBox);
      this.Controls.Add(this.addBusinessCodePlaceholderFormatButton);
      this.Controls.Add(this.criteriaPropertyInfoGroupBox);
      this.Controls.Add(this.userNumberButton);
      this.Controls.Add(this.departmentButton);
      this.Controls.Add(this.dayButton);
      this.Controls.Add(this.monthButton);
      this.Controls.Add(this.shortYearButton);
      this.Controls.Add(this.lengthYearButton);
      this.Controls.Add(this.addBusinessCodeSerialFormatButton);
      this.Controls.Add(this.cancelButton);
      this.Controls.Add(this.okButton);
      this.Controls.Add(this.fillOnSavingCheckBox);
      this.Controls.Add(this.formatStringRichTextBox);
      this.Name = "BusinessCodeFormatEditDialog";
      this.Text = "编辑业务码格式: {0}[{1}]";
      ((System.ComponentModel.ISupportInitialize)(this.businessCodeFormatBindingSource)).EndInit();
      this.criteriaPropertyInfoGroupBox.ResumeLayout(false);
      this.criteriaPropertyInfoGroupBox.PerformLayout();
      this.criteriaPropertyInfoSelectGroupBox.ResumeLayout(false);
      this.criteriaPropertyInfoSelectGroupBox.PerformLayout();
      ((System.ComponentModel.ISupportInitialize)(this.criteriaPropertySelectValuesBindingSource)).EndInit();
      this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.BindingSource businessCodeFormatBindingSource;
    private System.Windows.Forms.RichTextBox formatStringRichTextBox;
    private System.Windows.Forms.CheckBox fillOnSavingCheckBox;
    private System.Windows.Forms.Button cancelButton;
    private System.Windows.Forms.Button okButton;
    private System.Windows.Forms.Button addBusinessCodeSerialFormatButton;
    private System.Windows.Forms.Button lengthYearButton;
    private System.Windows.Forms.Button shortYearButton;
    private System.Windows.Forms.Button monthButton;
    private System.Windows.Forms.Button dayButton;
    private System.Windows.Forms.Button departmentButton;
    private System.Windows.Forms.Button userNumberButton;
    private System.Windows.Forms.Label criteriaPropertyNameLabel;
    private System.Windows.Forms.TextBox criteriaPropertyValueTextBox;
    private System.Windows.Forms.GroupBox criteriaPropertyInfoGroupBox;
    private System.Windows.Forms.Button addBusinessCodePlaceholderFormatButton;
    private System.Windows.Forms.GroupBox criteriaPropertyInfoSelectGroupBox;
    private System.Windows.Forms.Label criteriaPropertyFriendlyNameLabel;
    private System.Windows.Forms.ComboBox criteriaPropertyValueComboBox;
    private System.Windows.Forms.BindingSource criteriaPropertySelectValuesBindingSource;
    private System.Windows.Forms.Button addButton;
  }
}