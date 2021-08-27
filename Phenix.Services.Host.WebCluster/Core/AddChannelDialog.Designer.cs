namespace Phenix.Services.Host.WebCluster.Core
{
  partial class AddChannelDialog
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
      this.toolTip = new System.Windows.Forms.ToolTip(this.components);
      this.hostTextBox = new System.Windows.Forms.TextBox();
      this.webApiPortNumericUpDown = new System.Windows.Forms.NumericUpDown();
      this.webSocketPortNumericUpDown = new System.Windows.Forms.NumericUpDown();
      this.hostLabel = new System.Windows.Forms.Label();
      this.webApiPortLabel = new System.Windows.Forms.Label();
      this.webSocketPortLabel = new System.Windows.Forms.Label();
      ((System.ComponentModel.ISupportInitialize)(this.webApiPortNumericUpDown)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.webSocketPortNumericUpDown)).BeginInit();
      this.SuspendLayout();
      // 
      // cancelButton
      // 
      this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
      this.cancelButton.Location = new System.Drawing.Point(312, 58);
      this.cancelButton.Name = "cancelButton";
      this.cancelButton.Size = new System.Drawing.Size(90, 23);
      this.cancelButton.TabIndex = 7;
      this.cancelButton.Text = "Cancel";
      this.cancelButton.UseVisualStyleBackColor = true;
      // 
      // okButton
      // 
      this.okButton.DialogResult = System.Windows.Forms.DialogResult.OK;
      this.okButton.Location = new System.Drawing.Point(312, 26);
      this.okButton.Name = "okButton";
      this.okButton.Size = new System.Drawing.Size(90, 23);
      this.okButton.TabIndex = 6;
      this.okButton.Text = "OK";
      this.okButton.UseVisualStyleBackColor = true;
      // 
      // hostTextBox
      // 
      this.hostTextBox.Location = new System.Drawing.Point(97, 24);
      this.hostTextBox.MaxLength = 30;
      this.hostTextBox.Name = "hostTextBox";
      this.hostTextBox.Size = new System.Drawing.Size(186, 21);
      this.hostTextBox.TabIndex = 1;
      this.hostTextBox.Text = "http://127.0.0.1";
      this.toolTip.SetToolTip(this.hostTextBox, "本机访问Host的IP地址");
      // 
      // webApiPortNumericUpDown
      // 
      this.webApiPortNumericUpDown.Location = new System.Drawing.Point(97, 50);
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
      this.webApiPortNumericUpDown.TabIndex = 3;
      this.toolTip.SetToolTip(this.webApiPortNumericUpDown, "本机访问Host的WebAPI Port");
      this.webApiPortNumericUpDown.Value = new decimal(new int[] {
            8080,
            0,
            0,
            0});
      // 
      // webSocketPortNumericUpDown
      // 
      this.webSocketPortNumericUpDown.Location = new System.Drawing.Point(97, 76);
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
      this.webSocketPortNumericUpDown.TabIndex = 5;
      this.toolTip.SetToolTip(this.webSocketPortNumericUpDown, "本机访问Host的WebSocket Port");
      this.webSocketPortNumericUpDown.Value = new decimal(new int[] {
            8081,
            0,
            0,
            0});
      // 
      // hostLabel
      // 
      this.hostLabel.AutoSize = true;
      this.hostLabel.Location = new System.Drawing.Point(29, 30);
      this.hostLabel.Name = "hostLabel";
      this.hostLabel.Size = new System.Drawing.Size(53, 12);
      this.hostLabel.TabIndex = 0;
      this.hostLabel.Text = "Host URL";
      // 
      // webApiPortLabel
      // 
      this.webApiPortLabel.AutoSize = true;
      this.webApiPortLabel.Location = new System.Drawing.Point(41, 54);
      this.webApiPortLabel.Name = "webApiPortLabel";
      this.webApiPortLabel.Size = new System.Drawing.Size(41, 12);
      this.webApiPortLabel.TabIndex = 2;
      this.webApiPortLabel.Text = "WebAPI";
      // 
      // webSocketPortLabel
      // 
      this.webSocketPortLabel.AutoSize = true;
      this.webSocketPortLabel.Location = new System.Drawing.Point(23, 78);
      this.webSocketPortLabel.Name = "webSocketPortLabel";
      this.webSocketPortLabel.Size = new System.Drawing.Size(59, 12);
      this.webSocketPortLabel.TabIndex = 4;
      this.webSocketPortLabel.Text = "WebSocket";
      // 
      // AddChannelDialog
      // 
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
      this.CancelButton = this.cancelButton;
      this.ClientSize = new System.Drawing.Size(426, 136);
      this.Controls.Add(this.webSocketPortLabel);
      this.Controls.Add(this.webApiPortLabel);
      this.Controls.Add(this.hostLabel);
      this.Controls.Add(this.webSocketPortNumericUpDown);
      this.Controls.Add(this.webApiPortNumericUpDown);
      this.Controls.Add(this.hostTextBox);
      this.Controls.Add(this.cancelButton);
      this.Controls.Add(this.okButton);
      this.Name = "AddChannelDialog";
      this.Text = "Add channel";
      ((System.ComponentModel.ISupportInitialize)(this.webApiPortNumericUpDown)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.webSocketPortNumericUpDown)).EndInit();
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.Button cancelButton;
    private System.Windows.Forms.Button okButton;
    private System.Windows.Forms.ToolTip toolTip;
    private System.Windows.Forms.TextBox hostTextBox;
    private System.Windows.Forms.Label hostLabel;
    private System.Windows.Forms.NumericUpDown webApiPortNumericUpDown;
    private System.Windows.Forms.Label webApiPortLabel;
    private System.Windows.Forms.NumericUpDown webSocketPortNumericUpDown;
    private System.Windows.Forms.Label webSocketPortLabel;
  }
}