namespace Phenix.Security.Plugin.Authoriser
{
  partial class TestForm
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
      this.passwordTextBox = new System.Windows.Forms.TextBox();
      this.label2 = new System.Windows.Forms.Label();
      this.userNumberTextBox = new System.Windows.Forms.TextBox();
      this.label1 = new System.Windows.Forms.Label();
      this.cancelButton = new System.Windows.Forms.Button();
      this.okButton = new System.Windows.Forms.Button();
      this.SuspendLayout();
      // 
      // passwordTextBox
      // 
      this.passwordTextBox.Location = new System.Drawing.Point(116, 59);
      this.passwordTextBox.Name = "passwordTextBox";
      this.passwordTextBox.PasswordChar = '*';
      this.passwordTextBox.Size = new System.Drawing.Size(167, 21);
      this.passwordTextBox.TabIndex = 3;
      // 
      // label2
      // 
      this.label2.AutoSize = true;
      this.label2.Location = new System.Drawing.Point(57, 62);
      this.label2.Name = "label2";
      this.label2.Size = new System.Drawing.Size(53, 12);
      this.label2.TabIndex = 2;
      this.label2.Text = "Password";
      // 
      // userNumberTextBox
      // 
      this.userNumberTextBox.Location = new System.Drawing.Point(116, 32);
      this.userNumberTextBox.Name = "userNumberTextBox";
      this.userNumberTextBox.Size = new System.Drawing.Size(167, 21);
      this.userNumberTextBox.TabIndex = 1;
      // 
      // label1
      // 
      this.label1.AutoSize = true;
      this.label1.Location = new System.Drawing.Point(45, 35);
      this.label1.Name = "label1";
      this.label1.Size = new System.Drawing.Size(65, 12);
      this.label1.TabIndex = 0;
      this.label1.Text = "UserNumber";
      // 
      // cancelButton
      // 
      this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
      this.cancelButton.Location = new System.Drawing.Point(330, 59);
      this.cancelButton.Name = "cancelButton";
      this.cancelButton.Size = new System.Drawing.Size(90, 23);
      this.cancelButton.TabIndex = 5;
      this.cancelButton.Text = "Cancel";
      this.cancelButton.UseVisualStyleBackColor = true;
      // 
      // okButton
      // 
      this.okButton.Location = new System.Drawing.Point(330, 30);
      this.okButton.Name = "okButton";
      this.okButton.Size = new System.Drawing.Size(90, 23);
      this.okButton.TabIndex = 4;
      this.okButton.Text = "OK";
      this.okButton.UseVisualStyleBackColor = true;
      this.okButton.Click += new System.EventHandler(this.okButton_Click);
      // 
      // TestForm
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(465, 132);
      this.Controls.Add(this.passwordTextBox);
      this.Controls.Add(this.label2);
      this.Controls.Add(this.userNumberTextBox);
      this.Controls.Add(this.label1);
      this.Controls.Add(this.cancelButton);
      this.Controls.Add(this.okButton);
      this.Name = "TestForm";
      this.Text = "TestForm";
      this.Shown += new System.EventHandler(this.TestForm_Shown);
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.TextBox passwordTextBox;
    private System.Windows.Forms.Label label2;
    private System.Windows.Forms.TextBox userNumberTextBox;
    private System.Windows.Forms.Label label1;
    private System.Windows.Forms.Button cancelButton;
    private System.Windows.Forms.Button okButton;
  }
}