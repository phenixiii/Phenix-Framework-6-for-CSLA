namespace Phenix.Core.Windows
{
  sealed partial class ShowMessageDialog
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
      this.messageRichTextBox = new System.Windows.Forms.RichTextBox();
      this.SuspendLayout();
      // 
      // messageRichTextBox
      // 
      this.messageRichTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
      this.messageRichTextBox.Location = new System.Drawing.Point(0, 0);
      this.messageRichTextBox.Name = "messageRichTextBox";
      this.messageRichTextBox.ReadOnly = true;
      this.messageRichTextBox.Size = new System.Drawing.Size(466, 360);
      this.messageRichTextBox.TabIndex = 0;
      this.messageRichTextBox.Text = "";
      // 
      // ShowMessageDialog
      // 
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
      this.ClientSize = new System.Drawing.Size(466, 360);
      this.Controls.Add(this.messageRichTextBox);
      this.MinimizeBox = false;
      this.Name = "ShowMessageDialog";
      this.ShowInTaskbar = false;
      this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
      this.Text = "提示信息";
      this.TopMost = true;
      this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.RichTextBox messageRichTextBox;
  }
}