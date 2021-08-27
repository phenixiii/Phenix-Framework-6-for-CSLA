namespace Phenix.Services.Client.Security
{
  partial class ChangePasswordDialog
  {
    /// <summary>
    /// 必需的设计器变量。
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    /// <summary>
    /// 清理所有正在使用的资源。
    /// </summary>
    /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
    protected override void Dispose(bool disposing)
    {
      if (disposing && (components != null))
      {
        components.Dispose();
      }
      base.Dispose(disposing);
    }

    #region Windows 窗体设计器生成的代码

    /// <summary>
    /// 设计器支持所需的方法 - 不要
    /// 使用代码编辑器修改此方法的内容。
    /// </summary>
    private void InitializeComponent()
    {
      this.oldPasswordTextBox = new System.Windows.Forms.TextBox();
      this.userNumberTextBox = new System.Windows.Forms.TextBox();
      this.newPassword1TextBox = new System.Windows.Forms.TextBox();
      this.newPassword2TextBox = new System.Windows.Forms.TextBox();
      this.cancelButton = new System.Windows.Forms.Button();
      this.okButton = new System.Windows.Forms.Button();
      this.newPassword2Label = new System.Windows.Forms.Label();
      this.newPassword1Label = new System.Windows.Forms.Label();
      this.oldPasswordLabel = new System.Windows.Forms.Label();
      this.userNumberLabel = new System.Windows.Forms.Label();
      this.SuspendLayout();
      // 
      // oldPasswordTextBox
      // 
      this.oldPasswordTextBox.Location = new System.Drawing.Point(107, 69);
      this.oldPasswordTextBox.MaxLength = 30;
      this.oldPasswordTextBox.Name = "oldPasswordTextBox";
      this.oldPasswordTextBox.PasswordChar = '*';
      this.oldPasswordTextBox.Size = new System.Drawing.Size(215, 21);
      this.oldPasswordTextBox.TabIndex = 3;
      this.oldPasswordTextBox.TextChanged += new System.EventHandler(this.OldPasswordTextBox_TextChanged);
      // 
      // userNumberTextBox
      // 
      this.userNumberTextBox.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
      this.userNumberTextBox.Location = new System.Drawing.Point(107, 41);
      this.userNumberTextBox.MaxLength = 30;
      this.userNumberTextBox.Name = "userNumberTextBox";
      this.userNumberTextBox.Size = new System.Drawing.Size(215, 21);
      this.userNumberTextBox.TabIndex = 1;
      this.userNumberTextBox.TextChanged += new System.EventHandler(this.UserNumberTextBox_TextChanged);
      // 
      // newPassword1TextBox
      // 
      this.newPassword1TextBox.Location = new System.Drawing.Point(107, 97);
      this.newPassword1TextBox.MaxLength = 30;
      this.newPassword1TextBox.Name = "newPassword1TextBox";
      this.newPassword1TextBox.PasswordChar = '*';
      this.newPassword1TextBox.Size = new System.Drawing.Size(215, 21);
      this.newPassword1TextBox.TabIndex = 5;
      this.newPassword1TextBox.TextChanged += new System.EventHandler(this.NewPassword1TextBox_TextChanged);
      // 
      // newPassword2TextBox
      // 
      this.newPassword2TextBox.Location = new System.Drawing.Point(107, 125);
      this.newPassword2TextBox.MaxLength = 30;
      this.newPassword2TextBox.Name = "newPassword2TextBox";
      this.newPassword2TextBox.PasswordChar = '*';
      this.newPassword2TextBox.Size = new System.Drawing.Size(215, 21);
      this.newPassword2TextBox.TabIndex = 7;
      this.newPassword2TextBox.TextChanged += new System.EventHandler(this.NewPassword2TextBox_TextChanged);
      // 
      // cancelButton
      // 
      this.cancelButton.DataBindings.Add(new System.Windows.Forms.Binding("Text", global::Phenix.Services.Client.Properties.Settings.Default, "Cancel", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
      this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
      this.cancelButton.Location = new System.Drawing.Point(353, 62);
      this.cancelButton.Name = "cancelButton";
      this.cancelButton.Size = new System.Drawing.Size(90, 23);
      this.cancelButton.TabIndex = 9;
      this.cancelButton.Text = global::Phenix.Services.Client.Properties.Settings.Default.Cancel;
      this.cancelButton.UseVisualStyleBackColor = true;
      // 
      // okButton
      // 
      this.okButton.DataBindings.Add(new System.Windows.Forms.Binding("Text", global::Phenix.Services.Client.Properties.Settings.Default, "Ok", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
      this.okButton.Location = new System.Drawing.Point(353, 30);
      this.okButton.Name = "okButton";
      this.okButton.Size = new System.Drawing.Size(90, 23);
      this.okButton.TabIndex = 8;
      this.okButton.Text = global::Phenix.Services.Client.Properties.Settings.Default.Ok;
      this.okButton.UseVisualStyleBackColor = true;
      this.okButton.Click += new System.EventHandler(this.OK_Click);
      // 
      // newPassword2Label
      // 
      this.newPassword2Label.AutoSize = true;
      this.newPassword2Label.DataBindings.Add(new System.Windows.Forms.Binding("Text", global::Phenix.Services.Client.Properties.Settings.Default, "NewPassword2", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
      this.newPassword2Label.Location = new System.Drawing.Point(33, 128);
      this.newPassword2Label.Name = "newPassword2Label";
      this.newPassword2Label.Size = new System.Drawing.Size(65, 12);
      this.newPassword2Label.TabIndex = 6;
      this.newPassword2Label.Text = global::Phenix.Services.Client.Properties.Settings.Default.NewPassword2;
      this.newPassword2Label.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
      // 
      // newPassword1Label
      // 
      this.newPassword1Label.AutoSize = true;
      this.newPassword1Label.DataBindings.Add(new System.Windows.Forms.Binding("Text", global::Phenix.Services.Client.Properties.Settings.Default, "NewPassword1", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
      this.newPassword1Label.Location = new System.Drawing.Point(33, 100);
      this.newPassword1Label.Name = "newPassword1Label";
      this.newPassword1Label.Size = new System.Drawing.Size(41, 12);
      this.newPassword1Label.TabIndex = 4;
      this.newPassword1Label.Text = global::Phenix.Services.Client.Properties.Settings.Default.NewPassword1;
      this.newPassword1Label.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
      // 
      // oldPasswordLabel
      // 
      this.oldPasswordLabel.AutoSize = true;
      this.oldPasswordLabel.DataBindings.Add(new System.Windows.Forms.Binding("Text", global::Phenix.Services.Client.Properties.Settings.Default, "OldPassword", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
      this.oldPasswordLabel.Location = new System.Drawing.Point(33, 72);
      this.oldPasswordLabel.Name = "oldPasswordLabel";
      this.oldPasswordLabel.Size = new System.Drawing.Size(41, 12);
      this.oldPasswordLabel.TabIndex = 2;
      this.oldPasswordLabel.Text = global::Phenix.Services.Client.Properties.Settings.Default.OldPassword;
      this.oldPasswordLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
      // 
      // userNumberLabel
      // 
      this.userNumberLabel.AutoSize = true;
      this.userNumberLabel.DataBindings.Add(new System.Windows.Forms.Binding("Text", global::Phenix.Services.Client.Properties.Settings.Default, "UserNumber", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
      this.userNumberLabel.Location = new System.Drawing.Point(33, 44);
      this.userNumberLabel.Name = "userNumberLabel";
      this.userNumberLabel.Size = new System.Drawing.Size(29, 12);
      this.userNumberLabel.TabIndex = 0;
      this.userNumberLabel.Text = global::Phenix.Services.Client.Properties.Settings.Default.UserNumber;
      this.userNumberLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
      // 
      // ChangePasswordDialog
      // 
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
      this.CancelButton = this.cancelButton;
      this.ClientSize = new System.Drawing.Size(474, 195);
      this.Controls.Add(this.cancelButton);
      this.Controls.Add(this.okButton);
      this.Controls.Add(this.newPassword2TextBox);
      this.Controls.Add(this.newPassword2Label);
      this.Controls.Add(this.newPassword1TextBox);
      this.Controls.Add(this.newPassword1Label);
      this.Controls.Add(this.oldPasswordTextBox);
      this.Controls.Add(this.userNumberTextBox);
      this.Controls.Add(this.oldPasswordLabel);
      this.Controls.Add(this.userNumberLabel);
      this.DataBindings.Add(new System.Windows.Forms.Binding("Text", global::Phenix.Services.Client.Properties.Settings.Default, "ChangePassword", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
      this.Name = "ChangePasswordDialog";
      this.Text = global::Phenix.Services.Client.Properties.Settings.Default.ChangePassword;
      this.Shown += new System.EventHandler(this.ChangePasswordForm_Shown);
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.TextBox oldPasswordTextBox;
    private System.Windows.Forms.TextBox userNumberTextBox;
    private System.Windows.Forms.Label oldPasswordLabel;
    private System.Windows.Forms.Label userNumberLabel;
    private System.Windows.Forms.TextBox newPassword1TextBox;
    private System.Windows.Forms.Label newPassword1Label;
    private System.Windows.Forms.TextBox newPassword2TextBox;
    private System.Windows.Forms.Label newPassword2Label;
    private System.Windows.Forms.Button cancelButton;
    private System.Windows.Forms.Button okButton;
  }
}