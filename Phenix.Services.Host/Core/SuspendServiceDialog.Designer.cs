namespace Phenix.Services.Host.Core
{
  partial class SuspendServiceDialog
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
      this.cancelButton = new System.Windows.Forms.Button();
      this.okButton = new System.Windows.Forms.Button();
      this.suspendReasonRichTextBox = new System.Windows.Forms.RichTextBox();
      this.label1 = new System.Windows.Forms.Label();
      this.SuspendLayout();
      // 
      // cancelButton
      // 
      this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
      this.cancelButton.Location = new System.Drawing.Point(380, 52);
      this.cancelButton.Name = "cancelButton";
      this.cancelButton.Size = new System.Drawing.Size(90, 23);
      this.cancelButton.TabIndex = 3;
      this.cancelButton.Text = "Cancal";
      this.cancelButton.UseVisualStyleBackColor = true;
      // 
      // okButton
      // 
      this.okButton.DialogResult = System.Windows.Forms.DialogResult.OK;
      this.okButton.Location = new System.Drawing.Point(380, 20);
      this.okButton.Name = "okButton";
      this.okButton.Size = new System.Drawing.Size(90, 23);
      this.okButton.TabIndex = 2;
      this.okButton.Text = "OK";
      this.okButton.UseVisualStyleBackColor = true;
      // 
      // suspendReasonRichTextBox
      // 
      this.suspendReasonRichTextBox.Location = new System.Drawing.Point(32, 34);
      this.suspendReasonRichTextBox.Name = "suspendReasonRichTextBox";
      this.suspendReasonRichTextBox.Size = new System.Drawing.Size(313, 129);
      this.suspendReasonRichTextBox.TabIndex = 1;
      this.suspendReasonRichTextBox.Text = "因系统需要切割升级，暂停一切服务。\n请见此消息后退出系统重新登录！";
      // 
      // label1
      // 
      this.label1.AutoSize = true;
      this.label1.Location = new System.Drawing.Point(30, 19);
      this.label1.Name = "label1";
      this.label1.Size = new System.Drawing.Size(263, 12);
      this.label1.TabIndex = 0;
      this.label1.Text = "Please input reason to inform the end user:";
      // 
      // SuspendServiceDialog
      // 
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
      this.CancelButton = this.cancelButton;
      this.ClientSize = new System.Drawing.Size(499, 189);
      this.Controls.Add(this.label1);
      this.Controls.Add(this.suspendReasonRichTextBox);
      this.Controls.Add(this.cancelButton);
      this.Controls.Add(this.okButton);
      this.EnterMoveNextControl = false;
      this.Name = "SuspendServiceDialog";
      this.Text = "Pause service";
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.Button cancelButton;
    private System.Windows.Forms.Button okButton;
    private System.Windows.Forms.RichTextBox suspendReasonRichTextBox;
    private System.Windows.Forms.Label label1;
  }
}