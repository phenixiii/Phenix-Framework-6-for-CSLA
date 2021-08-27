namespace Phenix.Windows
{
  partial class FormSetupDialog
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
      this.SuspendLayout();
      // 
      // FormSetupDialog
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(727, 395);
      this.DataBindings.Add(new System.Windows.Forms.Binding("Text", global::Phenix.Windows.Properties.Settings.Default, "FormSetupDialog", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
      this.Name = "FormSetupDialog";
      this.Text = global::Phenix.Windows.Properties.Settings.Default.FormSetupDialog;
      this.ResumeLayout(false);

    }

    #endregion
  }
}