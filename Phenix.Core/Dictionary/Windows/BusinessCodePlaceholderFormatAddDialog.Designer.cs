namespace Phenix.Core.Dictionary.Windows
{
  partial class BusinessCodePlaceholderFormatAddDialog
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
      this.propertyNameLabel = new System.Windows.Forms.Label();
      this.cancelButton = new System.Windows.Forms.Button();
      this.okButton = new System.Windows.Forms.Button();
      this.lengthLabel = new System.Windows.Forms.Label();
      this.lengthNumericUpDown = new System.Windows.Forms.NumericUpDown();
      this.businessCodePlaceholderFormatBindingSource = new System.Windows.Forms.BindingSource(this.components);
      this.formatStringLabel = new System.Windows.Forms.Label();
      this.formatStringTextBox = new System.Windows.Forms.TextBox();
      this.propertyNameTextBox = new System.Windows.Forms.TextBox();
      this.propertySelectNamesBindingSource = new System.Windows.Forms.BindingSource(this.components);
      this.propertyNameComboBox = new System.Windows.Forms.ComboBox();
      ((System.ComponentModel.ISupportInitialize)(this.lengthNumericUpDown)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.businessCodePlaceholderFormatBindingSource)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.propertySelectNamesBindingSource)).BeginInit();
      this.SuspendLayout();
      // 
      // propertyNameLabel
      // 
      this.propertyNameLabel.AutoSize = true;
      this.propertyNameLabel.Location = new System.Drawing.Point(26, 79);
      this.propertyNameLabel.Name = "propertyNameLabel";
      this.propertyNameLabel.Size = new System.Drawing.Size(53, 12);
      this.propertyNameLabel.TabIndex = 4;
      this.propertyNameLabel.Text = "属性名称";
      // 
      // cancelButton
      // 
      this.cancelButton.DataBindings.Add(new System.Windows.Forms.Binding("Text", global::Phenix.Core.Properties.Settings.Default, "Cancel", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
      this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
      this.cancelButton.Location = new System.Drawing.Point(321, 43);
      this.cancelButton.Name = "cancelButton";
      this.cancelButton.Size = new System.Drawing.Size(91, 24);
      this.cancelButton.TabIndex = 8;
      this.cancelButton.Text = global::Phenix.Core.Properties.Settings.Default.Cancel;
      this.cancelButton.UseVisualStyleBackColor = true;
      // 
      // okButton
      // 
      this.okButton.DataBindings.Add(new System.Windows.Forms.Binding("Text", global::Phenix.Core.Properties.Settings.Default, "Ok", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
      this.okButton.DialogResult = System.Windows.Forms.DialogResult.OK;
      this.okButton.Location = new System.Drawing.Point(321, 14);
      this.okButton.Name = "okButton";
      this.okButton.Size = new System.Drawing.Size(91, 24);
      this.okButton.TabIndex = 7;
      this.okButton.Text = global::Phenix.Core.Properties.Settings.Default.Ok;
      this.okButton.UseVisualStyleBackColor = true;
      // 
      // lengthLabel
      // 
      this.lengthLabel.AutoSize = true;
      this.lengthLabel.Location = new System.Drawing.Point(26, 51);
      this.lengthLabel.Name = "lengthLabel";
      this.lengthLabel.Size = new System.Drawing.Size(53, 12);
      this.lengthLabel.TabIndex = 2;
      this.lengthLabel.Text = "固定长度";
      // 
      // lengthNumericUpDown
      // 
      this.lengthNumericUpDown.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.businessCodePlaceholderFormatBindingSource, "Length", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
      this.lengthNumericUpDown.Location = new System.Drawing.Point(85, 49);
      this.lengthNumericUpDown.Maximum = new decimal(new int[] {
            10,
            0,
            0,
            0});
      this.lengthNumericUpDown.Name = "lengthNumericUpDown";
      this.lengthNumericUpDown.Size = new System.Drawing.Size(66, 21);
      this.lengthNumericUpDown.TabIndex = 3;
      // 
      // businessCodePlaceholderFormatBindingSource
      // 
      this.businessCodePlaceholderFormatBindingSource.DataSource = typeof(Phenix.Core.Dictionary.BusinessCodePlaceholderFormat);
      // 
      // formatStringLabel
      // 
      this.formatStringLabel.AutoSize = true;
      this.formatStringLabel.Location = new System.Drawing.Point(26, 25);
      this.formatStringLabel.Name = "formatStringLabel";
      this.formatStringLabel.Size = new System.Drawing.Size(53, 12);
      this.formatStringLabel.TabIndex = 0;
      this.formatStringLabel.Text = "格 式 串";
      // 
      // formatStringTextBox
      // 
      this.formatStringTextBox.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.businessCodePlaceholderFormatBindingSource, "FormatString", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
      this.formatStringTextBox.Location = new System.Drawing.Point(85, 22);
      this.formatStringTextBox.Name = "formatStringTextBox";
      this.formatStringTextBox.ReadOnly = true;
      this.formatStringTextBox.Size = new System.Drawing.Size(206, 21);
      this.formatStringTextBox.TabIndex = 1;
      // 
      // propertyNameTextBox
      // 
      this.propertyNameTextBox.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.businessCodePlaceholderFormatBindingSource, "PropertyName", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
      this.propertyNameTextBox.Location = new System.Drawing.Point(85, 76);
      this.propertyNameTextBox.Name = "propertyNameTextBox";
      this.propertyNameTextBox.Size = new System.Drawing.Size(206, 21);
      this.propertyNameTextBox.TabIndex = 5;
      // 
      // propertySelectNamesBindingSource
      // 
      this.propertySelectNamesBindingSource.DataSource = typeof(Phenix.Core.Operate.IKeyCaption);
      // 
      // propertyNameComboBox
      // 
      this.propertyNameComboBox.DataBindings.Add(new System.Windows.Forms.Binding("SelectedValue", this.businessCodePlaceholderFormatBindingSource, "PropertyName", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
      this.propertyNameComboBox.DataSource = this.propertySelectNamesBindingSource;
      this.propertyNameComboBox.DisplayMember = "Caption";
      this.propertyNameComboBox.FormattingEnabled = true;
      this.propertyNameComboBox.Location = new System.Drawing.Point(85, 76);
      this.propertyNameComboBox.Name = "propertyNameComboBox";
      this.propertyNameComboBox.Size = new System.Drawing.Size(206, 20);
      this.propertyNameComboBox.TabIndex = 6;
      this.propertyNameComboBox.ValueMember = "Key";
      // 
      // BusinessCodePlaceholderFormatAddDialog
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(436, 148);
      this.Controls.Add(this.propertyNameComboBox);
      this.Controls.Add(this.propertyNameLabel);
      this.Controls.Add(this.propertyNameTextBox);
      this.Controls.Add(this.cancelButton);
      this.Controls.Add(this.okButton);
      this.Controls.Add(this.lengthLabel);
      this.Controls.Add(this.lengthNumericUpDown);
      this.Controls.Add(this.formatStringLabel);
      this.Controls.Add(this.formatStringTextBox);
      this.Name = "BusinessCodePlaceholderFormatAddDialog";
      this.Text = "添加业务码占位符格式: : {0}[{1}]";
      ((System.ComponentModel.ISupportInitialize)(this.lengthNumericUpDown)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.businessCodePlaceholderFormatBindingSource)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.propertySelectNamesBindingSource)).EndInit();
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.Button cancelButton;
    private System.Windows.Forms.Button okButton;
    private System.Windows.Forms.Label lengthLabel;
    private System.Windows.Forms.NumericUpDown lengthNumericUpDown;
    private System.Windows.Forms.Label formatStringLabel;
    private System.Windows.Forms.TextBox formatStringTextBox;
    private System.Windows.Forms.BindingSource businessCodePlaceholderFormatBindingSource;
    private System.Windows.Forms.TextBox propertyNameTextBox;
    private System.Windows.Forms.Label propertyNameLabel;
    private System.Windows.Forms.BindingSource propertySelectNamesBindingSource;
    private System.Windows.Forms.ComboBox propertyNameComboBox;
  }
}