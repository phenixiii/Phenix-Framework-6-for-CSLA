namespace Phenix.Windows.Helper
{
  partial class TemplateSetDialog
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
      this.templateNameTextEdit = new DevExpress.XtraEditors.TextEdit();
      this.cancelButton = new DevExpress.XtraEditors.SimpleButton();
      this.okButton = new DevExpress.XtraEditors.SimpleButton();
      this.templateNameLabelControl = new DevExpress.XtraEditors.LabelControl();
      ((System.ComponentModel.ISupportInitialize)(this.templateNameTextEdit.Properties)).BeginInit();
      this.SuspendLayout();
      // 
      // templateNameTextEdit
      // 
      this.templateNameTextEdit.Location = new System.Drawing.Point(75, 18);
      this.templateNameTextEdit.Name = "templateNameTextEdit";
      this.templateNameTextEdit.Size = new System.Drawing.Size(183, 20);
      this.templateNameTextEdit.TabIndex = 1;
      this.templateNameTextEdit.EditValueChanged += new System.EventHandler(this.templateNameTextEdit_EditValueChanged);
      // 
      // cancelButton
      // 
      this.cancelButton.DataBindings.Add(new System.Windows.Forms.Binding("Text", global::Phenix.Windows.Properties.Settings.Default, "Cancel", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
      this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
      this.cancelButton.Location = new System.Drawing.Point(298, 44);
      this.cancelButton.Name = "cancelButton";
      this.cancelButton.Size = new System.Drawing.Size(75, 23);
      this.cancelButton.TabIndex = 3;
      this.cancelButton.Text = global::Phenix.Windows.Properties.Settings.Default.Cancel;
      // 
      // okButton
      // 
      this.okButton.DataBindings.Add(new System.Windows.Forms.Binding("Text", global::Phenix.Windows.Properties.Settings.Default, "Ok", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
      this.okButton.Location = new System.Drawing.Point(298, 15);
      this.okButton.Name = "okButton";
      this.okButton.Size = new System.Drawing.Size(75, 23);
      this.okButton.TabIndex = 2;
      this.okButton.Text = global::Phenix.Windows.Properties.Settings.Default.Ok;
      this.okButton.Click += new System.EventHandler(this.okButton_Click);
      // 
      // templateNameLabelControl
      // 
      this.templateNameLabelControl.DataBindings.Add(new System.Windows.Forms.Binding("Text", global::Phenix.Windows.Properties.Settings.Default, "TemplateName", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
      this.templateNameLabelControl.Location = new System.Drawing.Point(21, 20);
      this.templateNameLabelControl.Name = "templateNameLabelControl";
      this.templateNameLabelControl.Size = new System.Drawing.Size(48, 14);
      this.templateNameLabelControl.TabIndex = 0;
      this.templateNameLabelControl.Text = global::Phenix.Windows.Properties.Settings.Default.TemplateName;
      // 
      // TemplateSetDialog
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(397, 105);
      this.Controls.Add(this.cancelButton);
      this.Controls.Add(this.okButton);
      this.Controls.Add(this.templateNameTextEdit);
      this.Controls.Add(this.templateNameLabelControl);
      this.DataBindings.Add(new System.Windows.Forms.Binding("Text", global::Phenix.Windows.Properties.Settings.Default, "TemplateSetDialog", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
      this.Name = "TemplateSetDialog";
      this.ShowIcon = false;
      this.ShowInTaskbar = false;
      this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
      this.Text = global::Phenix.Windows.Properties.Settings.Default.TemplateSetDialog;
      this.Shown += new System.EventHandler(this.NewTemplateDialog_Shown);
      ((System.ComponentModel.ISupportInitialize)(this.templateNameTextEdit.Properties)).EndInit();
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private DevExpress.XtraEditors.LabelControl templateNameLabelControl;
    private DevExpress.XtraEditors.TextEdit templateNameTextEdit;
    private DevExpress.XtraEditors.SimpleButton cancelButton;
    private DevExpress.XtraEditors.SimpleButton okButton;
  }
}