namespace Phenix.Windows.Client
{
  partial class LogOnDialog
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
      ((System.ComponentModel.ISupportInitialize)(this.logoPictureBox)).BeginInit();
      this.SuspendLayout();
      // 
      // hostComboBox
      // 
      this.hostComboBox.Items.AddRange(new object[] {
            "embedded"});
      this.hostComboBox.Text = "embedded";
      // 
      // LogOnDialog
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(474, 220);
      this.Name = "LogOnDialog";
      ((System.ComponentModel.ISupportInitialize)(this.logoPictureBox)).EndInit();
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion
  }
}