namespace Phenix.Core.Dictionary.Windows
{
  partial class BusinessCodeSerialFormatAddDialog
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
      this.formatStringLabel = new System.Windows.Forms.Label();
      this.groupModeLabel = new System.Windows.Forms.Label();
      this.lengthLabel = new System.Windows.Forms.Label();
      this.initialValueLabel = new System.Windows.Forms.Label();
      this.resetCycleLabel = new System.Windows.Forms.Label();
      this.businessCodeSerialFormatBindingSource = new System.Windows.Forms.BindingSource(this.components);
      this.formatStringTextBox = new System.Windows.Forms.TextBox();
      this.groupModeComboBox = new System.Windows.Forms.ComboBox();
      this.businessCodeSerialGroupModeEnumKeyCaptionCollectionBindingSource = new System.Windows.Forms.BindingSource(this.components);
      this.lengthNumericUpDown = new System.Windows.Forms.NumericUpDown();
      this.initialValueNumericUpDown = new System.Windows.Forms.NumericUpDown();
      this.resetCycleComboBox = new System.Windows.Forms.ComboBox();
      this.businessCodeSerialResetCycleEnumKeyCaptionCollectionBindingSource = new System.Windows.Forms.BindingSource(this.components);
      this.cancelButton = new System.Windows.Forms.Button();
      this.okButton = new System.Windows.Forms.Button();
      ((System.ComponentModel.ISupportInitialize)(this.businessCodeSerialFormatBindingSource)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.businessCodeSerialGroupModeEnumKeyCaptionCollectionBindingSource)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.lengthNumericUpDown)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.initialValueNumericUpDown)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.businessCodeSerialResetCycleEnumKeyCaptionCollectionBindingSource)).BeginInit();
      this.SuspendLayout();
      // 
      // formatStringLabel
      // 
      this.formatStringLabel.AutoSize = true;
      this.formatStringLabel.Location = new System.Drawing.Point(28, 26);
      this.formatStringLabel.Name = "formatStringLabel";
      this.formatStringLabel.Size = new System.Drawing.Size(53, 12);
      this.formatStringLabel.TabIndex = 0;
      this.formatStringLabel.Text = "格 式 串";
      // 
      // groupModeLabel
      // 
      this.groupModeLabel.AutoSize = true;
      this.groupModeLabel.Location = new System.Drawing.Point(28, 80);
      this.groupModeLabel.Name = "groupModeLabel";
      this.groupModeLabel.Size = new System.Drawing.Size(53, 12);
      this.groupModeLabel.TabIndex = 6;
      this.groupModeLabel.Text = "分组模式";
      // 
      // lengthLabel
      // 
      this.lengthLabel.AutoSize = true;
      this.lengthLabel.Location = new System.Drawing.Point(28, 52);
      this.lengthLabel.Name = "lengthLabel";
      this.lengthLabel.Size = new System.Drawing.Size(53, 12);
      this.lengthLabel.TabIndex = 2;
      this.lengthLabel.Text = "固定长度";
      // 
      // initialValueLabel
      // 
      this.initialValueLabel.AutoSize = true;
      this.initialValueLabel.Location = new System.Drawing.Point(158, 52);
      this.initialValueLabel.Name = "initialValueLabel";
      this.initialValueLabel.Size = new System.Drawing.Size(41, 12);
      this.initialValueLabel.TabIndex = 4;
      this.initialValueLabel.Text = "起始值";
      // 
      // resetCycleLabel
      // 
      this.resetCycleLabel.AutoSize = true;
      this.resetCycleLabel.Location = new System.Drawing.Point(28, 106);
      this.resetCycleLabel.Name = "resetCycleLabel";
      this.resetCycleLabel.Size = new System.Drawing.Size(53, 12);
      this.resetCycleLabel.TabIndex = 8;
      this.resetCycleLabel.Text = "重置周期";
      // 
      // businessCodeSerialFormatBindingSource
      // 
      this.businessCodeSerialFormatBindingSource.DataSource = typeof(Phenix.Core.Dictionary.BusinessCodeSerialFormat);
      // 
      // formatStringTextBox
      // 
      this.formatStringTextBox.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.businessCodeSerialFormatBindingSource, "FormatString", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
      this.formatStringTextBox.Location = new System.Drawing.Point(87, 23);
      this.formatStringTextBox.Name = "formatStringTextBox";
      this.formatStringTextBox.ReadOnly = true;
      this.formatStringTextBox.Size = new System.Drawing.Size(205, 21);
      this.formatStringTextBox.TabIndex = 1;
      // 
      // groupModeComboBox
      // 
      this.groupModeComboBox.DataSource = this.businessCodeSerialGroupModeEnumKeyCaptionCollectionBindingSource;
      this.groupModeComboBox.DisplayMember = "Caption";
      this.groupModeComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
      this.groupModeComboBox.FormattingEnabled = true;
      this.groupModeComboBox.Location = new System.Drawing.Point(87, 77);
      this.groupModeComboBox.Name = "groupModeComboBox";
      this.groupModeComboBox.Size = new System.Drawing.Size(205, 20);
      this.groupModeComboBox.TabIndex = 7;
      this.groupModeComboBox.SelectedValueChanged += new System.EventHandler(this.groupModeComboBox_SelectedValueChanged);
      // 
      // businessCodeSerialGroupModeEnumKeyCaptionCollectionBindingSource
      // 
      this.businessCodeSerialGroupModeEnumKeyCaptionCollectionBindingSource.DataSource = typeof(Phenix.Core.Rule.EnumKeyCaption);
      // 
      // lengthNumericUpDown
      // 
      this.lengthNumericUpDown.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.businessCodeSerialFormatBindingSource, "Length", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
      this.lengthNumericUpDown.Location = new System.Drawing.Point(87, 50);
      this.lengthNumericUpDown.Maximum = new decimal(new int[] {
            10,
            0,
            0,
            0});
      this.lengthNumericUpDown.Name = "lengthNumericUpDown";
      this.lengthNumericUpDown.Size = new System.Drawing.Size(65, 21);
      this.lengthNumericUpDown.TabIndex = 3;
      // 
      // initialValueNumericUpDown
      // 
      this.initialValueNumericUpDown.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.businessCodeSerialFormatBindingSource, "InitialValue", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
      this.initialValueNumericUpDown.Location = new System.Drawing.Point(205, 50);
      this.initialValueNumericUpDown.Name = "initialValueNumericUpDown";
      this.initialValueNumericUpDown.Size = new System.Drawing.Size(87, 21);
      this.initialValueNumericUpDown.TabIndex = 5;
      // 
      // resetCycleComboBox
      // 
      this.resetCycleComboBox.DataSource = this.businessCodeSerialResetCycleEnumKeyCaptionCollectionBindingSource;
      this.resetCycleComboBox.DisplayMember = "Caption";
      this.resetCycleComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
      this.resetCycleComboBox.FormattingEnabled = true;
      this.resetCycleComboBox.Location = new System.Drawing.Point(87, 103);
      this.resetCycleComboBox.Name = "resetCycleComboBox";
      this.resetCycleComboBox.Size = new System.Drawing.Size(205, 20);
      this.resetCycleComboBox.TabIndex = 9;
      this.resetCycleComboBox.SelectedValueChanged += new System.EventHandler(this.resetCycleComboBox_SelectedValueChanged);
      // 
      // businessCodeSerialResetCycleEnumKeyCaptionCollectionBindingSource
      // 
      this.businessCodeSerialResetCycleEnumKeyCaptionCollectionBindingSource.DataSource = typeof(Phenix.Core.Rule.EnumKeyCaption);
      // 
      // cancelButton
      // 
      this.cancelButton.DataBindings.Add(new System.Windows.Forms.Binding("Text", global::Phenix.Core.Properties.Settings.Default, "Cancel", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
      this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
      this.cancelButton.Location = new System.Drawing.Point(323, 44);
      this.cancelButton.Name = "cancelButton";
      this.cancelButton.Size = new System.Drawing.Size(90, 23);
      this.cancelButton.TabIndex = 11;
      this.cancelButton.Text = global::Phenix.Core.Properties.Settings.Default.Cancel;
      this.cancelButton.UseVisualStyleBackColor = true;
      // 
      // okButton
      // 
      this.okButton.DataBindings.Add(new System.Windows.Forms.Binding("Text", global::Phenix.Core.Properties.Settings.Default, "Ok", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
      this.okButton.DialogResult = System.Windows.Forms.DialogResult.OK;
      this.okButton.Location = new System.Drawing.Point(323, 15);
      this.okButton.Name = "okButton";
      this.okButton.Size = new System.Drawing.Size(90, 23);
      this.okButton.TabIndex = 10;
      this.okButton.Text = global::Phenix.Core.Properties.Settings.Default.Ok;
      this.okButton.UseVisualStyleBackColor = true;
      // 
      // BusinessCodeSerialFormatAddDialog
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(436, 148);
      this.Controls.Add(this.cancelButton);
      this.Controls.Add(this.okButton);
      this.Controls.Add(this.resetCycleLabel);
      this.Controls.Add(this.resetCycleComboBox);
      this.Controls.Add(this.initialValueLabel);
      this.Controls.Add(this.initialValueNumericUpDown);
      this.Controls.Add(this.lengthLabel);
      this.Controls.Add(this.lengthNumericUpDown);
      this.Controls.Add(this.groupModeLabel);
      this.Controls.Add(this.groupModeComboBox);
      this.Controls.Add(this.formatStringLabel);
      this.Controls.Add(this.formatStringTextBox);
      this.Name = "BusinessCodeSerialFormatAddDialog";
      this.Text = "添加业务码流水号格式: : {0}[{1}]";
      ((System.ComponentModel.ISupportInitialize)(this.businessCodeSerialFormatBindingSource)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.businessCodeSerialGroupModeEnumKeyCaptionCollectionBindingSource)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.lengthNumericUpDown)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.initialValueNumericUpDown)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.businessCodeSerialResetCycleEnumKeyCaptionCollectionBindingSource)).EndInit();
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.BindingSource businessCodeSerialFormatBindingSource;
    private System.Windows.Forms.TextBox formatStringTextBox;
    private System.Windows.Forms.ComboBox groupModeComboBox;
    private System.Windows.Forms.BindingSource businessCodeSerialGroupModeEnumKeyCaptionCollectionBindingSource;
    private System.Windows.Forms.NumericUpDown lengthNumericUpDown;
    private System.Windows.Forms.NumericUpDown initialValueNumericUpDown;
    private System.Windows.Forms.ComboBox resetCycleComboBox;
    private System.Windows.Forms.BindingSource businessCodeSerialResetCycleEnumKeyCaptionCollectionBindingSource;
    private System.Windows.Forms.Button cancelButton;
    private System.Windows.Forms.Button okButton;
    private System.Windows.Forms.Label formatStringLabel;
    private System.Windows.Forms.Label groupModeLabel;
    private System.Windows.Forms.Label lengthLabel;
    private System.Windows.Forms.Label initialValueLabel;
    private System.Windows.Forms.Label resetCycleLabel;
  }
}