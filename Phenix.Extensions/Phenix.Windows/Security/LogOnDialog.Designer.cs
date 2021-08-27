namespace Phenix.Windows.Security
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
      this.components = new System.ComponentModel.Container();
      System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LogOnDialog));
      this.passwordTextBox = new System.Windows.Forms.TextBox();
      this.userNumberTextBox = new System.Windows.Forms.TextBox();
      this.logoPictureBox = new System.Windows.Forms.PictureBox();
      this.hostComboBox = new System.Windows.Forms.ComboBox();
      this.HintLabel = new System.Windows.Forms.Label();
      this.changePasswordButton = new System.Windows.Forms.Button();
      this.hostLabel = new System.Windows.Forms.Label();
      this.quitButton = new System.Windows.Forms.Button();
      this.logOnButton = new System.Windows.Forms.Button();
      this.passwordLabel = new System.Windows.Forms.Label();
      this.userNumberLabel = new System.Windows.Forms.Label();
      this.contextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
      this.ClearCacheToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      ((System.ComponentModel.ISupportInitialize)(this.logoPictureBox)).BeginInit();
      this.contextMenuStrip.SuspendLayout();
      this.SuspendLayout();
      // 
      // passwordTextBox
      // 
      this.passwordTextBox.Location = new System.Drawing.Point(215, 77);
      this.passwordTextBox.MaxLength = 20;
      this.passwordTextBox.Name = "passwordTextBox";
      this.passwordTextBox.PasswordChar = '*';
      this.passwordTextBox.Size = new System.Drawing.Size(215, 21);
      this.passwordTextBox.TabIndex = 3;
      this.passwordTextBox.TextChanged += new System.EventHandler(this.passwordTextBox_TextChanged);
      // 
      // userNumberTextBox
      // 
      this.userNumberTextBox.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
      this.userNumberTextBox.Location = new System.Drawing.Point(215, 46);
      this.userNumberTextBox.MaxLength = 20;
      this.userNumberTextBox.Name = "userNumberTextBox";
      this.userNumberTextBox.Size = new System.Drawing.Size(215, 21);
      this.userNumberTextBox.TabIndex = 1;
      this.userNumberTextBox.TextChanged += new System.EventHandler(this.UserNumberTextBox_TextChanged);
      // 
      // logoPictureBox
      // 
      this.logoPictureBox.Dock = System.Windows.Forms.DockStyle.Fill;
      this.logoPictureBox.Image = ((System.Drawing.Image)(resources.GetObject("logoPictureBox.Image")));
      this.logoPictureBox.Location = new System.Drawing.Point(0, 0);
      this.logoPictureBox.Name = "logoPictureBox";
      this.logoPictureBox.Size = new System.Drawing.Size(474, 220);
      this.logoPictureBox.TabIndex = 7;
      this.logoPictureBox.TabStop = false;
      // 
      // hostComboBox
      // 
      this.hostComboBox.Location = new System.Drawing.Point(215, 108);
      this.hostComboBox.Name = "hostComboBox";
      this.hostComboBox.Size = new System.Drawing.Size(215, 20);
      this.hostComboBox.TabIndex = 5;
      this.hostComboBox.TextChanged += new System.EventHandler(this.hostComboBox_TextChanged);
      this.hostComboBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.hostComboBox_KeyDown);
      // 
      // HintLabel
      // 
      this.HintLabel.BackColor = System.Drawing.SystemColors.Control;
      this.HintLabel.ForeColor = System.Drawing.Color.Blue;
      this.HintLabel.Location = new System.Drawing.Point(176, 173);
      this.HintLabel.Name = "HintLabel";
      this.HintLabel.Size = new System.Drawing.Size(254, 38);
      this.HintLabel.TabIndex = 6;
      // 
      // changePasswordButton
      // 
      this.changePasswordButton.DataBindings.Add(new System.Windows.Forms.Binding("Text", global::Phenix.Services.Client.Properties.Settings.Default, "ChangePassword", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
      this.changePasswordButton.Location = new System.Drawing.Point(264, 147);
      this.changePasswordButton.Name = "changePasswordButton";
      this.changePasswordButton.Size = new System.Drawing.Size(80, 23);
      this.changePasswordButton.TabIndex = 8;
      this.changePasswordButton.TabStop = false;
      this.changePasswordButton.Text = global::Phenix.Services.Client.Properties.Settings.Default.ChangePassword;
      this.changePasswordButton.Click += new System.EventHandler(this.ChangePasswordButton_Click);
      // 
      // hostLabel
      // 
      this.hostLabel.AutoSize = true;
      this.hostLabel.DataBindings.Add(new System.Windows.Forms.Binding("Text", global::Phenix.Services.Client.Properties.Settings.Default, "Host", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
      this.hostLabel.Location = new System.Drawing.Point(176, 111);
      this.hostLabel.Name = "hostLabel";
      this.hostLabel.Size = new System.Drawing.Size(29, 12);
      this.hostLabel.TabIndex = 4;
      this.hostLabel.Text = global::Phenix.Services.Client.Properties.Settings.Default.Host;
      // 
      // quitButton
      // 
      this.quitButton.DataBindings.Add(new System.Windows.Forms.Binding("Text", global::Phenix.Services.Client.Properties.Settings.Default, "Quit", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
      this.quitButton.Location = new System.Drawing.Point(350, 147);
      this.quitButton.Name = "quitButton";
      this.quitButton.Size = new System.Drawing.Size(80, 23);
      this.quitButton.TabIndex = 9;
      this.quitButton.TabStop = false;
      this.quitButton.Text = global::Phenix.Services.Client.Properties.Settings.Default.Quit;
      this.quitButton.Click += new System.EventHandler(this.QuitButton_Click);
      // 
      // logOnButton
      // 
      this.logOnButton.DataBindings.Add(new System.Windows.Forms.Binding("Text", global::Phenix.Services.Client.Properties.Settings.Default, "LogOn", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
      this.logOnButton.Location = new System.Drawing.Point(178, 147);
      this.logOnButton.Name = "logOnButton";
      this.logOnButton.Size = new System.Drawing.Size(80, 23);
      this.logOnButton.TabIndex = 7;
      this.logOnButton.Text = global::Phenix.Services.Client.Properties.Settings.Default.LogOn;
      this.logOnButton.Click += new System.EventHandler(this.LoginButton_Click);
      // 
      // passwordLabel
      // 
      this.passwordLabel.AutoSize = true;
      this.passwordLabel.DataBindings.Add(new System.Windows.Forms.Binding("Text", global::Phenix.Services.Client.Properties.Settings.Default, "Password", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
      this.passwordLabel.Location = new System.Drawing.Point(176, 80);
      this.passwordLabel.Name = "passwordLabel";
      this.passwordLabel.Size = new System.Drawing.Size(29, 12);
      this.passwordLabel.TabIndex = 2;
      this.passwordLabel.Text = global::Phenix.Services.Client.Properties.Settings.Default.Password;
      this.passwordLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
      // 
      // userNumberLabel
      // 
      this.userNumberLabel.AutoSize = true;
      this.userNumberLabel.DataBindings.Add(new System.Windows.Forms.Binding("Text", global::Phenix.Services.Client.Properties.Settings.Default, "UserNumber", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
      this.userNumberLabel.Location = new System.Drawing.Point(176, 49);
      this.userNumberLabel.Name = "userNumberLabel";
      this.userNumberLabel.Size = new System.Drawing.Size(29, 12);
      this.userNumberLabel.TabIndex = 0;
      this.userNumberLabel.Text = global::Phenix.Services.Client.Properties.Settings.Default.UserNumber;
      this.userNumberLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
      // 
      // contextMenuStrip
      // 
      this.contextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ClearCacheToolStripMenuItem});
      this.contextMenuStrip.Name = "contextMenuStrip";
      this.contextMenuStrip.Size = new System.Drawing.Size(125, 26);
      // 
      // ClearCacheToolStripMenuItem
      // 
      this.ClearCacheToolStripMenuItem.Name = "ClearCacheToolStripMenuItem";
      this.ClearCacheToolStripMenuItem.Size = new System.Drawing.Size(124, 22);
      this.ClearCacheToolStripMenuItem.Text = global::Phenix.Services.Client.Properties.Settings.Default.ClearCache;
      this.ClearCacheToolStripMenuItem.Click += new System.EventHandler(this.ClearCacheToolStripMenuItem_Click);
      // 
      // LogOnDialog
      // 
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
      this.ClientSize = new System.Drawing.Size(474, 220);
      this.ContextMenuStrip = this.contextMenuStrip;
      this.Controls.Add(this.changePasswordButton);
      this.Controls.Add(this.HintLabel);
      this.Controls.Add(this.hostComboBox);
      this.Controls.Add(this.hostLabel);
      this.Controls.Add(this.quitButton);
      this.Controls.Add(this.logOnButton);
      this.Controls.Add(this.passwordTextBox);
      this.Controls.Add(this.userNumberTextBox);
      this.Controls.Add(this.passwordLabel);
      this.Controls.Add(this.userNumberLabel);
      this.Controls.Add(this.logoPictureBox);
      this.DataBindings.Add(new System.Windows.Forms.Binding("Text", global::Phenix.Services.Client.Properties.Settings.Default, "LogOnTitle", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
      this.Name = "LogOnDialog";
      this.Text = global::Phenix.Services.Client.Properties.Settings.Default.LogOnTitle;
      this.TopMost = false;
      ((System.ComponentModel.ISupportInitialize)(this.logoPictureBox)).EndInit();
      this.contextMenuStrip.ResumeLayout(false);
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    /// <summary>
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields")]
    protected System.Windows.Forms.ComboBox hostComboBox;
    /// <summary>
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields")]
    protected System.Windows.Forms.Label hostLabel;
    /// <summary>
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields")]
    protected System.Windows.Forms.Button quitButton;
    /// <summary>
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields")]
    protected System.Windows.Forms.Button logOnButton;
    /// <summary>
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields")]
    protected System.Windows.Forms.TextBox passwordTextBox;
    /// <summary>
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields")]
    protected System.Windows.Forms.TextBox userNumberTextBox;
    /// <summary>
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields")]
    protected System.Windows.Forms.Label passwordLabel;
    /// <summary>
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields")]
    protected System.Windows.Forms.Label userNumberLabel;
    /// <summary>
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields")]
    protected System.Windows.Forms.PictureBox logoPictureBox;
    /// <summary>
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields")]
    protected System.Windows.Forms.Label HintLabel;
    /// <summary>
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields")]
    protected System.Windows.Forms.Button changePasswordButton;
    private System.Windows.Forms.ContextMenuStrip contextMenuStrip;
    private System.Windows.Forms.ToolStripMenuItem ClearCacheToolStripMenuItem;
  }
}