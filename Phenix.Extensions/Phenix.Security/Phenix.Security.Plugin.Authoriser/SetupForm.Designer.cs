namespace Phenix.Security.Plugin.Authoriser
{
  partial class SetupForm
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
      this.label1 = new System.Windows.Forms.Label();
      this.cancelButton = new System.Windows.Forms.Button();
      this.okButton = new System.Windows.Forms.Button();
      this.ldapPathTextBox = new System.Windows.Forms.TextBox();
      this.userNamePrdfixTextBox = new System.Windows.Forms.TextBox();
      this.label2 = new System.Windows.Forms.Label();
      this.testButton = new System.Windows.Forms.Button();
      this.adminPasswordTextBox = new System.Windows.Forms.TextBox();
      this.label3 = new System.Windows.Forms.Label();
      this.adminNameTextBox = new System.Windows.Forms.TextBox();
      this.label4 = new System.Windows.Forms.Label();
      this.SuspendLayout();
      // 
      // label1
      // 
      this.label1.AutoSize = true;
      this.label1.Location = new System.Drawing.Point(74, 34);
      this.label1.Name = "label1";
      this.label1.Size = new System.Drawing.Size(47, 12);
      this.label1.TabIndex = 0;
      this.label1.Text = "LDAP://";
      // 
      // cancelButton
      // 
      this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
      this.cancelButton.Location = new System.Drawing.Point(477, 58);
      this.cancelButton.Name = "cancelButton";
      this.cancelButton.Size = new System.Drawing.Size(90, 23);
      this.cancelButton.TabIndex = 9;
      this.cancelButton.Text = "Cancel";
      this.cancelButton.UseVisualStyleBackColor = true;
      // 
      // okButton
      // 
      this.okButton.Location = new System.Drawing.Point(477, 29);
      this.okButton.Name = "okButton";
      this.okButton.Size = new System.Drawing.Size(90, 23);
      this.okButton.TabIndex = 8;
      this.okButton.Text = "OK";
      this.okButton.UseVisualStyleBackColor = true;
      this.okButton.Click += new System.EventHandler(this.okButton_Click);
      // 
      // ldapPathTextBox
      // 
      this.ldapPathTextBox.Location = new System.Drawing.Point(127, 31);
      this.ldapPathTextBox.Name = "ldapPathTextBox";
      this.ldapPathTextBox.Size = new System.Drawing.Size(315, 21);
      this.ldapPathTextBox.TabIndex = 1;
      // 
      // userNamePrdfixTextBox
      // 
      this.userNamePrdfixTextBox.Location = new System.Drawing.Point(127, 58);
      this.userNamePrdfixTextBox.Name = "userNamePrdfixTextBox";
      this.userNamePrdfixTextBox.Size = new System.Drawing.Size(315, 21);
      this.userNamePrdfixTextBox.TabIndex = 3;
      // 
      // label2
      // 
      this.label2.AutoSize = true;
      this.label2.Location = new System.Drawing.Point(32, 63);
      this.label2.Name = "label2";
      this.label2.Size = new System.Drawing.Size(89, 12);
      this.label2.TabIndex = 2;
      this.label2.Text = "UserNamePrdfix";
      // 
      // testButton
      // 
      this.testButton.Location = new System.Drawing.Point(477, 87);
      this.testButton.Name = "testButton";
      this.testButton.Size = new System.Drawing.Size(90, 23);
      this.testButton.TabIndex = 10;
      this.testButton.Text = "Test";
      this.testButton.UseVisualStyleBackColor = true;
      this.testButton.Click += new System.EventHandler(this.testButton_Click);
      // 
      // adminPasswordTextBox
      // 
      this.adminPasswordTextBox.Location = new System.Drawing.Point(127, 112);
      this.adminPasswordTextBox.Name = "adminPasswordTextBox";
      this.adminPasswordTextBox.PasswordChar = '*';
      this.adminPasswordTextBox.Size = new System.Drawing.Size(315, 21);
      this.adminPasswordTextBox.TabIndex = 7;
      // 
      // label3
      // 
      this.label3.AutoSize = true;
      this.label3.Location = new System.Drawing.Point(38, 115);
      this.label3.Name = "label3";
      this.label3.Size = new System.Drawing.Size(83, 12);
      this.label3.TabIndex = 6;
      this.label3.Text = "AdminPassword";
      // 
      // adminNameTextBox
      // 
      this.adminNameTextBox.Location = new System.Drawing.Point(127, 85);
      this.adminNameTextBox.Name = "adminNameTextBox";
      this.adminNameTextBox.Size = new System.Drawing.Size(315, 21);
      this.adminNameTextBox.TabIndex = 5;
      // 
      // label4
      // 
      this.label4.AutoSize = true;
      this.label4.Location = new System.Drawing.Point(62, 88);
      this.label4.Name = "label4";
      this.label4.Size = new System.Drawing.Size(59, 12);
      this.label4.TabIndex = 4;
      this.label4.Text = "AdminName";
      // 
      // SetupForm
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(603, 178);
      this.Controls.Add(this.adminPasswordTextBox);
      this.Controls.Add(this.label3);
      this.Controls.Add(this.adminNameTextBox);
      this.Controls.Add(this.label4);
      this.Controls.Add(this.testButton);
      this.Controls.Add(this.userNamePrdfixTextBox);
      this.Controls.Add(this.label2);
      this.Controls.Add(this.ldapPathTextBox);
      this.Controls.Add(this.label1);
      this.Controls.Add(this.cancelButton);
      this.Controls.Add(this.okButton);
      this.Name = "SetupForm";
      this.Text = "Setup";
      this.Shown += new System.EventHandler(this.SetupForm_Shown);
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion
    private System.Windows.Forms.Label label1;
    private System.Windows.Forms.Button cancelButton;
    private System.Windows.Forms.Button okButton;
    private System.Windows.Forms.TextBox ldapPathTextBox;
    private System.Windows.Forms.TextBox userNamePrdfixTextBox;
    private System.Windows.Forms.Label label2;
    private System.Windows.Forms.Button testButton;
    private System.Windows.Forms.TextBox adminPasswordTextBox;
    private System.Windows.Forms.Label label3;
    private System.Windows.Forms.TextBox adminNameTextBox;
    private System.Windows.Forms.Label label4;
  }
}