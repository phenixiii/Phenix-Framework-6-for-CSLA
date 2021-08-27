namespace Phenix.Services.Host.WebCluster.Core
{
  partial class SystemInfoSetupDialog
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
      this.components = new System.ComponentModel.Container();
      this.cancelButton = new System.Windows.Forms.Button();
      this.okButton = new System.Windows.Forms.Button();
      this.tabControl = new System.Windows.Forms.TabControl();
      this.serviceTabPage = new System.Windows.Forms.TabPage();
      this.ajaxGroupBox = new System.Windows.Forms.GroupBox();
      this.webSocketSslIdnHostLabel = new System.Windows.Forms.Label();
      this.webSocketSslPortLabel = new System.Windows.Forms.Label();
      this.webSocketSslIdnHostTextBox = new System.Windows.Forms.TextBox();
      this.webSocketSslPortNumericUpDown = new System.Windows.Forms.NumericUpDown();
      this.webSocketSslCheckBox = new System.Windows.Forms.CheckBox();
      this.webApiSslIdnHostLabel = new System.Windows.Forms.Label();
      this.webApiSslPortLabel = new System.Windows.Forms.Label();
      this.webApiSslIdnHostTextBox = new System.Windows.Forms.TextBox();
      this.webApiSslPortNumericUpDown = new System.Windows.Forms.NumericUpDown();
      this.webApiSslCheckBox = new System.Windows.Forms.CheckBox();
      this.webMaxConcurrentRequestsLabel = new System.Windows.Forms.Label();
      this.webEnableCorsOriginsLabel = new System.Windows.Forms.Label();
      this.webEnableCorsOriginsTextBox = new System.Windows.Forms.TextBox();
      this.webMaxConcurrentRequestsNumericUpDown = new System.Windows.Forms.NumericUpDown();
      this.webSocketCheckBox = new System.Windows.Forms.CheckBox();
      this.webSocketPortNumericUpDown = new System.Windows.Forms.NumericUpDown();
      this.webApiPortNumericUpDown = new System.Windows.Forms.NumericUpDown();
      this.webApiCheckBox = new System.Windows.Forms.CheckBox();
      this.toolTip = new System.Windows.Forms.ToolTip(this.components);
      this.tabControl.SuspendLayout();
      this.serviceTabPage.SuspendLayout();
      this.ajaxGroupBox.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)(this.webSocketSslPortNumericUpDown)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.webApiSslPortNumericUpDown)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.webMaxConcurrentRequestsNumericUpDown)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.webSocketPortNumericUpDown)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.webApiPortNumericUpDown)).BeginInit();
      this.SuspendLayout();
      // 
      // cancelButton
      // 
      this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
      this.cancelButton.Location = new System.Drawing.Point(505, 55);
      this.cancelButton.Name = "cancelButton";
      this.cancelButton.Size = new System.Drawing.Size(90, 23);
      this.cancelButton.TabIndex = 2;
      this.cancelButton.Text = "Cancel";
      this.cancelButton.UseVisualStyleBackColor = true;
      // 
      // okButton
      // 
      this.okButton.Location = new System.Drawing.Point(505, 23);
      this.okButton.Name = "okButton";
      this.okButton.Size = new System.Drawing.Size(90, 23);
      this.okButton.TabIndex = 1;
      this.okButton.Text = "OK";
      this.okButton.UseVisualStyleBackColor = true;
      this.okButton.Click += new System.EventHandler(this.OK_Click);
      // 
      // tabControl
      // 
      this.tabControl.Controls.Add(this.serviceTabPage);
      this.tabControl.Location = new System.Drawing.Point(13, 12);
      this.tabControl.Name = "tabControl";
      this.tabControl.SelectedIndex = 0;
      this.tabControl.Size = new System.Drawing.Size(466, 379);
      this.tabControl.TabIndex = 0;
      // 
      // serviceTabPage
      // 
      this.serviceTabPage.Controls.Add(this.ajaxGroupBox);
      this.serviceTabPage.Location = new System.Drawing.Point(4, 22);
      this.serviceTabPage.Name = "serviceTabPage";
      this.serviceTabPage.Padding = new System.Windows.Forms.Padding(3);
      this.serviceTabPage.Size = new System.Drawing.Size(458, 353);
      this.serviceTabPage.TabIndex = 1;
      this.serviceTabPage.Text = "Service";
      this.serviceTabPage.UseVisualStyleBackColor = true;
      // 
      // ajaxGroupBox
      // 
      this.ajaxGroupBox.Controls.Add(this.webSocketSslIdnHostLabel);
      this.ajaxGroupBox.Controls.Add(this.webSocketSslPortLabel);
      this.ajaxGroupBox.Controls.Add(this.webSocketSslIdnHostTextBox);
      this.ajaxGroupBox.Controls.Add(this.webSocketSslPortNumericUpDown);
      this.ajaxGroupBox.Controls.Add(this.webSocketSslCheckBox);
      this.ajaxGroupBox.Controls.Add(this.webApiSslIdnHostLabel);
      this.ajaxGroupBox.Controls.Add(this.webApiSslPortLabel);
      this.ajaxGroupBox.Controls.Add(this.webApiSslIdnHostTextBox);
      this.ajaxGroupBox.Controls.Add(this.webApiSslPortNumericUpDown);
      this.ajaxGroupBox.Controls.Add(this.webApiSslCheckBox);
      this.ajaxGroupBox.Controls.Add(this.webMaxConcurrentRequestsLabel);
      this.ajaxGroupBox.Controls.Add(this.webEnableCorsOriginsLabel);
      this.ajaxGroupBox.Controls.Add(this.webEnableCorsOriginsTextBox);
      this.ajaxGroupBox.Controls.Add(this.webMaxConcurrentRequestsNumericUpDown);
      this.ajaxGroupBox.Controls.Add(this.webSocketCheckBox);
      this.ajaxGroupBox.Controls.Add(this.webSocketPortNumericUpDown);
      this.ajaxGroupBox.Controls.Add(this.webApiPortNumericUpDown);
      this.ajaxGroupBox.Controls.Add(this.webApiCheckBox);
      this.ajaxGroupBox.Location = new System.Drawing.Point(25, 24);
      this.ajaxGroupBox.Name = "ajaxGroupBox";
      this.ajaxGroupBox.Size = new System.Drawing.Size(406, 303);
      this.ajaxGroupBox.TabIndex = 0;
      this.ajaxGroupBox.TabStop = false;
      this.ajaxGroupBox.Text = "AJAX";
      // 
      // webSocketSslIdnHostLabel
      // 
      this.webSocketSslIdnHostLabel.AutoSize = true;
      this.webSocketSslIdnHostLabel.Location = new System.Drawing.Point(39, 186);
      this.webSocketSslIdnHostLabel.Name = "webSocketSslIdnHostLabel";
      this.webSocketSslIdnHostLabel.Size = new System.Drawing.Size(53, 12);
      this.webSocketSslIdnHostLabel.TabIndex = 10;
      this.webSocketSslIdnHostLabel.Text = "https://";
      // 
      // webSocketSslPortLabel
      // 
      this.webSocketSslPortLabel.AutoSize = true;
      this.webSocketSslPortLabel.Location = new System.Drawing.Point(287, 186);
      this.webSocketSslPortLabel.Name = "webSocketSslPortLabel";
      this.webSocketSslPortLabel.Size = new System.Drawing.Size(11, 12);
      this.webSocketSslPortLabel.TabIndex = 12;
      this.webSocketSslPortLabel.Text = ":";
      // 
      // webSocketSslIdnHostTextBox
      // 
      this.webSocketSslIdnHostTextBox.DataBindings.Add(new System.Windows.Forms.Binding("Text", global::Phenix.Services.Host.WebCluster.Properties.Settings.Default, "WebSocketSslIdnHost", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
      this.webSocketSslIdnHostTextBox.Location = new System.Drawing.Point(98, 182);
      this.webSocketSslIdnHostTextBox.MaxLength = 30;
      this.webSocketSslIdnHostTextBox.Name = "webSocketSslIdnHostTextBox";
      this.webSocketSslIdnHostTextBox.Size = new System.Drawing.Size(183, 21);
      this.webSocketSslIdnHostTextBox.TabIndex = 11;
      this.webSocketSslIdnHostTextBox.Text = global::Phenix.Services.Host.WebCluster.Properties.Settings.Default.WebSocketSslIdnHost;
      this.toolTip.SetToolTip(this.webSocketSslIdnHostTextBox, "WebSocket+SSL 服务域名");
      // 
      // webSocketSslPortNumericUpDown
      // 
      this.webSocketSslPortNumericUpDown.Location = new System.Drawing.Point(303, 182);
      this.webSocketSslPortNumericUpDown.Maximum = new decimal(new int[] {
            65535,
            0,
            0,
            0});
      this.webSocketSslPortNumericUpDown.Minimum = new decimal(new int[] {
            1024,
            0,
            0,
            0});
      this.webSocketSslPortNumericUpDown.Name = "webSocketSslPortNumericUpDown";
      this.webSocketSslPortNumericUpDown.Size = new System.Drawing.Size(80, 21);
      this.webSocketSslPortNumericUpDown.TabIndex = 13;
      this.toolTip.SetToolTip(this.webSocketSslPortNumericUpDown, "WebSocket+SSL Port");
      this.webSocketSslPortNumericUpDown.Value = new decimal(new int[] {
            8444,
            0,
            0,
            0});
      // 
      // webSocketSslCheckBox
      // 
      this.webSocketSslCheckBox.Checked = global::Phenix.Services.Host.WebCluster.Properties.Settings.Default.LetWebSocketSsl;
      this.webSocketSslCheckBox.DataBindings.Add(new System.Windows.Forms.Binding("Checked", global::Phenix.Services.Host.WebCluster.Properties.Settings.Default, "LetWebSocketSsl", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
      this.webSocketSslCheckBox.Location = new System.Drawing.Point(23, 160);
      this.webSocketSslCheckBox.Name = "webSocketSslCheckBox";
      this.webSocketSslCheckBox.Size = new System.Drawing.Size(120, 16);
      this.webSocketSslCheckBox.TabIndex = 9;
      this.webSocketSslCheckBox.Text = "WebSocket+SSL";
      this.toolTip.SetToolTip(this.webSocketSslCheckBox, "WebSocket+SSL");
      this.webSocketSslCheckBox.UseVisualStyleBackColor = true;
      // 
      // webApiSslIdnHostLabel
      // 
      this.webApiSslIdnHostLabel.AutoSize = true;
      this.webApiSslIdnHostLabel.Location = new System.Drawing.Point(39, 77);
      this.webApiSslIdnHostLabel.Name = "webApiSslIdnHostLabel";
      this.webApiSslIdnHostLabel.Size = new System.Drawing.Size(53, 12);
      this.webApiSslIdnHostLabel.TabIndex = 3;
      this.webApiSslIdnHostLabel.Text = "https://";
      // 
      // webApiSslPortLabel
      // 
      this.webApiSslPortLabel.AutoSize = true;
      this.webApiSslPortLabel.Location = new System.Drawing.Point(287, 77);
      this.webApiSslPortLabel.Name = "webApiSslPortLabel";
      this.webApiSslPortLabel.Size = new System.Drawing.Size(11, 12);
      this.webApiSslPortLabel.TabIndex = 5;
      this.webApiSslPortLabel.Text = ":";
      // 
      // webApiSslIdnHostTextBox
      // 
      this.webApiSslIdnHostTextBox.DataBindings.Add(new System.Windows.Forms.Binding("Text", global::Phenix.Services.Host.WebCluster.Properties.Settings.Default, "webApiSslIdnHost", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
      this.webApiSslIdnHostTextBox.Location = new System.Drawing.Point(98, 73);
      this.webApiSslIdnHostTextBox.MaxLength = 30;
      this.webApiSslIdnHostTextBox.Name = "webApiSslIdnHostTextBox";
      this.webApiSslIdnHostTextBox.Size = new System.Drawing.Size(183, 21);
      this.webApiSslIdnHostTextBox.TabIndex = 4;
      this.webApiSslIdnHostTextBox.Text = global::Phenix.Services.Host.WebCluster.Properties.Settings.Default.WebApiSslIdnHost;
      this.toolTip.SetToolTip(this.webApiSslIdnHostTextBox, "WebAPI+SSL 服务域名");
      this.webApiSslIdnHostTextBox.TextChanged += new System.EventHandler(this.webApiSslIdnHostTextBox_TextChanged);
      // 
      // webApiSslPortNumericUpDown
      // 
      this.webApiSslPortNumericUpDown.Location = new System.Drawing.Point(303, 73);
      this.webApiSslPortNumericUpDown.Maximum = new decimal(new int[] {
            65535,
            0,
            0,
            0});
      this.webApiSslPortNumericUpDown.Minimum = new decimal(new int[] {
            1024,
            0,
            0,
            0});
      this.webApiSslPortNumericUpDown.Name = "webApiSslPortNumericUpDown";
      this.webApiSslPortNumericUpDown.Size = new System.Drawing.Size(80, 21);
      this.webApiSslPortNumericUpDown.TabIndex = 6;
      this.toolTip.SetToolTip(this.webApiSslPortNumericUpDown, "WebAPI+SSL Port");
      this.webApiSslPortNumericUpDown.Value = new decimal(new int[] {
            8443,
            0,
            0,
            0});
      // 
      // webApiSslCheckBox
      // 
      this.webApiSslCheckBox.Checked = global::Phenix.Services.Host.WebCluster.Properties.Settings.Default.LetWebApiSsl;
      this.webApiSslCheckBox.DataBindings.Add(new System.Windows.Forms.Binding("Checked", global::Phenix.Services.Host.WebCluster.Properties.Settings.Default, "LetWebApiSsl", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
      this.webApiSslCheckBox.Location = new System.Drawing.Point(23, 51);
      this.webApiSslCheckBox.Name = "webApiSslCheckBox";
      this.webApiSslCheckBox.Size = new System.Drawing.Size(120, 16);
      this.webApiSslCheckBox.TabIndex = 2;
      this.webApiSslCheckBox.Text = "WebAPI+SSL";
      this.toolTip.SetToolTip(this.webApiSslCheckBox, "WebAPI+SSL");
      this.webApiSslCheckBox.UseVisualStyleBackColor = true;
      // 
      // webMaxConcurrentRequestsLabel
      // 
      this.webMaxConcurrentRequestsLabel.AutoSize = true;
      this.webMaxConcurrentRequestsLabel.Location = new System.Drawing.Point(21, 266);
      this.webMaxConcurrentRequestsLabel.Name = "webMaxConcurrentRequestsLabel";
      this.webMaxConcurrentRequestsLabel.Size = new System.Drawing.Size(143, 12);
      this.webMaxConcurrentRequestsLabel.TabIndex = 16;
      this.webMaxConcurrentRequestsLabel.Text = "Max Concurrent Requests";
      // 
      // webEnableCorsOriginsLabel
      // 
      this.webEnableCorsOriginsLabel.AutoSize = true;
      this.webEnableCorsOriginsLabel.Location = new System.Drawing.Point(21, 240);
      this.webEnableCorsOriginsLabel.Name = "webEnableCorsOriginsLabel";
      this.webEnableCorsOriginsLabel.Size = new System.Drawing.Size(113, 12);
      this.webEnableCorsOriginsLabel.TabIndex = 14;
      this.webEnableCorsOriginsLabel.Text = "EnableCors Origins";
      // 
      // webEnableCorsOriginsTextBox
      // 
      this.webEnableCorsOriginsTextBox.DataBindings.Add(new System.Windows.Forms.Binding("Text", global::Phenix.Services.Host.WebCluster.Properties.Settings.Default, "WebEnableCorsOrigins", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
      this.webEnableCorsOriginsTextBox.Location = new System.Drawing.Point(140, 236);
      this.webEnableCorsOriginsTextBox.MaxLength = 30;
      this.webEnableCorsOriginsTextBox.Name = "webEnableCorsOriginsTextBox";
      this.webEnableCorsOriginsTextBox.Size = new System.Drawing.Size(103, 21);
      this.webEnableCorsOriginsTextBox.TabIndex = 15;
      this.webEnableCorsOriginsTextBox.Text = global::Phenix.Services.Host.WebCluster.Properties.Settings.Default.WebEnableCorsOrigins;
      this.toolTip.SetToolTip(this.webEnableCorsOriginsTextBox, "获取允许访问资源的源");
      // 
      // webMaxConcurrentRequestsNumericUpDown
      // 
      this.webMaxConcurrentRequestsNumericUpDown.DataBindings.Add(new System.Windows.Forms.Binding("Value", global::Phenix.Services.Host.WebCluster.Properties.Settings.Default, "WebMaxConcurrentRequests", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
      this.webMaxConcurrentRequestsNumericUpDown.Location = new System.Drawing.Point(170, 262);
      this.webMaxConcurrentRequestsNumericUpDown.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
      this.webMaxConcurrentRequestsNumericUpDown.Minimum = new decimal(new int[] {
            10,
            0,
            0,
            0});
      this.webMaxConcurrentRequestsNumericUpDown.Name = "webMaxConcurrentRequestsNumericUpDown";
      this.webMaxConcurrentRequestsNumericUpDown.Size = new System.Drawing.Size(73, 21);
      this.webMaxConcurrentRequestsNumericUpDown.TabIndex = 17;
      this.toolTip.SetToolTip(this.webMaxConcurrentRequestsNumericUpDown, "设置可以在任何给定时间处理的并发 HttpRequestMessage 实例数的上限。  默认值是 100 乘以 CPU 内核数。 ");
      this.webMaxConcurrentRequestsNumericUpDown.Value = global::Phenix.Services.Host.WebCluster.Properties.Settings.Default.WebMaxConcurrentRequests;
      // 
      // webSocketCheckBox
      // 
      this.webSocketCheckBox.Checked = global::Phenix.Services.Host.WebCluster.Properties.Settings.Default.LetWebSocket;
      this.webSocketCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
      this.webSocketCheckBox.DataBindings.Add(new System.Windows.Forms.Binding("Checked", global::Phenix.Services.Host.WebCluster.Properties.Settings.Default, "LetWebSocket", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
      this.webSocketCheckBox.Location = new System.Drawing.Point(24, 134);
      this.webSocketCheckBox.Name = "webSocketCheckBox";
      this.webSocketCheckBox.Size = new System.Drawing.Size(80, 16);
      this.webSocketCheckBox.TabIndex = 7;
      this.webSocketCheckBox.Text = "WebSocket";
      this.toolTip.SetToolTip(this.webSocketCheckBox, "WebSocket");
      this.webSocketCheckBox.UseVisualStyleBackColor = true;
      // 
      // webSocketPortNumericUpDown
      // 
      this.webSocketPortNumericUpDown.Location = new System.Drawing.Point(110, 133);
      this.webSocketPortNumericUpDown.Maximum = new decimal(new int[] {
            65535,
            0,
            0,
            0});
      this.webSocketPortNumericUpDown.Minimum = new decimal(new int[] {
            1024,
            0,
            0,
            0});
      this.webSocketPortNumericUpDown.Name = "webSocketPortNumericUpDown";
      this.webSocketPortNumericUpDown.Size = new System.Drawing.Size(80, 21);
      this.webSocketPortNumericUpDown.TabIndex = 8;
      this.toolTip.SetToolTip(this.webSocketPortNumericUpDown, "WebSocket Port");
      this.webSocketPortNumericUpDown.Value = new decimal(new int[] {
            8081,
            0,
            0,
            0});
      // 
      // webApiPortNumericUpDown
      // 
      this.webApiPortNumericUpDown.Location = new System.Drawing.Point(98, 25);
      this.webApiPortNumericUpDown.Maximum = new decimal(new int[] {
            65535,
            0,
            0,
            0});
      this.webApiPortNumericUpDown.Minimum = new decimal(new int[] {
            1024,
            0,
            0,
            0});
      this.webApiPortNumericUpDown.Name = "webApiPortNumericUpDown";
      this.webApiPortNumericUpDown.Size = new System.Drawing.Size(80, 21);
      this.webApiPortNumericUpDown.TabIndex = 1;
      this.toolTip.SetToolTip(this.webApiPortNumericUpDown, "WebAPI Port");
      this.webApiPortNumericUpDown.Value = new decimal(new int[] {
            8080,
            0,
            0,
            0});
      // 
      // webApiCheckBox
      // 
      this.webApiCheckBox.Checked = global::Phenix.Services.Host.WebCluster.Properties.Settings.Default.LetWebApi;
      this.webApiCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
      this.webApiCheckBox.DataBindings.Add(new System.Windows.Forms.Binding("Checked", global::Phenix.Services.Host.WebCluster.Properties.Settings.Default, "LetWebApi", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
      this.webApiCheckBox.Location = new System.Drawing.Point(24, 26);
      this.webApiCheckBox.Name = "webApiCheckBox";
      this.webApiCheckBox.Size = new System.Drawing.Size(80, 16);
      this.webApiCheckBox.TabIndex = 0;
      this.webApiCheckBox.Text = "WebAPI";
      this.toolTip.SetToolTip(this.webApiCheckBox, "WebAPI");
      this.webApiCheckBox.UseVisualStyleBackColor = true;
      // 
      // SystemInfoSetupDialog
      // 
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
      this.CancelButton = this.cancelButton;
      this.ClientSize = new System.Drawing.Size(611, 408);
      this.Controls.Add(this.tabControl);
      this.Controls.Add(this.cancelButton);
      this.Controls.Add(this.okButton);
      this.Name = "SystemInfoSetupDialog";
      this.Text = "System parameter setup";
      this.tabControl.ResumeLayout(false);
      this.serviceTabPage.ResumeLayout(false);
      this.ajaxGroupBox.ResumeLayout(false);
      this.ajaxGroupBox.PerformLayout();
      ((System.ComponentModel.ISupportInitialize)(this.webSocketSslPortNumericUpDown)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.webApiSslPortNumericUpDown)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.webMaxConcurrentRequestsNumericUpDown)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.webSocketPortNumericUpDown)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.webApiPortNumericUpDown)).EndInit();
      this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.Button cancelButton;
    private System.Windows.Forms.Button okButton;
    private System.Windows.Forms.TabControl tabControl;
    private System.Windows.Forms.TabPage serviceTabPage;
    private System.Windows.Forms.ToolTip toolTip;
    private System.Windows.Forms.GroupBox ajaxGroupBox;
    private System.Windows.Forms.CheckBox webApiCheckBox;
    private System.Windows.Forms.NumericUpDown webApiPortNumericUpDown;
    private System.Windows.Forms.NumericUpDown webSocketPortNumericUpDown;
    private System.Windows.Forms.CheckBox webSocketCheckBox;
    private System.Windows.Forms.Label webMaxConcurrentRequestsLabel;
    private System.Windows.Forms.Label webEnableCorsOriginsLabel;
    private System.Windows.Forms.TextBox webEnableCorsOriginsTextBox;
    private System.Windows.Forms.NumericUpDown webMaxConcurrentRequestsNumericUpDown;
    private System.Windows.Forms.CheckBox webApiSslCheckBox;
    private System.Windows.Forms.NumericUpDown webApiSslPortNumericUpDown;
    private System.Windows.Forms.Label webApiSslPortLabel;
    private System.Windows.Forms.TextBox webApiSslIdnHostTextBox;
    private System.Windows.Forms.Label webApiSslIdnHostLabel;
    private System.Windows.Forms.Label webSocketSslIdnHostLabel;
    private System.Windows.Forms.Label webSocketSslPortLabel;
    private System.Windows.Forms.TextBox webSocketSslIdnHostTextBox;
    private System.Windows.Forms.NumericUpDown webSocketSslPortNumericUpDown;
    private System.Windows.Forms.CheckBox webSocketSslCheckBox;
  }
}