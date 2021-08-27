namespace Phenix.Services.Host.Synchro
{
  partial class SynchroDialog
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
      this.hostLabel = new System.Windows.Forms.Label();
      this.hostsCheckedListBox = new System.Windows.Forms.CheckedListBox();
      this.addButton = new System.Windows.Forms.Button();
      this.delButton = new System.Windows.Forms.Button();
      this.hostTextBox = new System.Windows.Forms.TextBox();
      this.SuspendLayout();
      // 
      // cancelButton
      // 
      this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
      this.cancelButton.Location = new System.Drawing.Point(305, 44);
      this.cancelButton.Name = "cancelButton";
      this.cancelButton.Size = new System.Drawing.Size(90, 23);
      this.cancelButton.TabIndex = 4;
      this.cancelButton.Text = "Quit";
      this.cancelButton.UseVisualStyleBackColor = true;
      this.cancelButton.Click += new System.EventHandler(this.cancelButton_Click);
      // 
      // okButton
      // 
      this.okButton.Location = new System.Drawing.Point(305, 12);
      this.okButton.Name = "okButton";
      this.okButton.Size = new System.Drawing.Size(90, 23);
      this.okButton.TabIndex = 3;
      this.okButton.Text = "Deploy";
      this.okButton.UseVisualStyleBackColor = true;
      this.okButton.Click += new System.EventHandler(this.okButton_Click);
      // 
      // hostLabel
      // 
      this.hostLabel.AutoSize = true;
      this.hostLabel.Location = new System.Drawing.Point(16, 17);
      this.hostLabel.Name = "hostLabel";
      this.hostLabel.Size = new System.Drawing.Size(47, 12);
      this.hostLabel.TabIndex = 0;
      this.hostLabel.Text = "service";
      // 
      // hostsCheckedListBox
      // 
      this.hostsCheckedListBox.CheckOnClick = true;
      this.hostsCheckedListBox.FormattingEnabled = true;
      this.hostsCheckedListBox.Location = new System.Drawing.Point(18, 39);
      this.hostsCheckedListBox.Name = "hostsCheckedListBox";
      this.hostsCheckedListBox.Size = new System.Drawing.Size(260, 212);
      this.hostsCheckedListBox.Sorted = true;
      this.hostsCheckedListBox.TabIndex = 2;
      // 
      // addButton
      // 
      this.addButton.Location = new System.Drawing.Point(233, 12);
      this.addButton.Name = "addButton";
      this.addButton.Size = new System.Drawing.Size(22, 23);
      this.addButton.TabIndex = 5;
      this.addButton.Text = "+";
      this.addButton.UseVisualStyleBackColor = true;
      this.addButton.Click += new System.EventHandler(this.addButton_Click);
      // 
      // delButton
      // 
      this.delButton.Location = new System.Drawing.Point(256, 12);
      this.delButton.Name = "delButton";
      this.delButton.Size = new System.Drawing.Size(22, 23);
      this.delButton.TabIndex = 6;
      this.delButton.Text = "-";
      this.delButton.UseVisualStyleBackColor = true;
      this.delButton.Click += new System.EventHandler(this.delButton_Click);
      // 
      // hostTextBox
      // 
      this.hostTextBox.Location = new System.Drawing.Point(69, 12);
      this.hostTextBox.Name = "hostTextBox";
      this.hostTextBox.Size = new System.Drawing.Size(158, 21);
      this.hostTextBox.TabIndex = 1;
      this.hostTextBox.Text = "127.0.0.1:8086";
      this.hostTextBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.hostTextBox_KeyPress);
      // 
      // SynchroDialog
      // 
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
      this.ClientSize = new System.Drawing.Size(409, 268);
      this.Controls.Add(this.delButton);
      this.Controls.Add(this.addButton);
      this.Controls.Add(this.hostTextBox);
      this.Controls.Add(this.hostsCheckedListBox);
      this.Controls.Add(this.hostLabel);
      this.Controls.Add(this.cancelButton);
      this.Controls.Add(this.okButton);
      this.Name = "SynchroDialog";
      this.Text = "Synchro services";
      this.Shown += new System.EventHandler(this.SynchroDialog_Shown);
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.Button cancelButton;
    private System.Windows.Forms.Button okButton;
    private System.Windows.Forms.Label hostLabel;
    private System.Windows.Forms.CheckedListBox hostsCheckedListBox;
    private System.Windows.Forms.TextBox hostTextBox;
    private System.Windows.Forms.Button addButton;
    private System.Windows.Forms.Button delButton;
  }
}