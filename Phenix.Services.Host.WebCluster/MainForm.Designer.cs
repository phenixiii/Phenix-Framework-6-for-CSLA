namespace Phenix.Services.Host.WebCluster
{
  partial class MainForm
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
      System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
      this.mainMenuToolStrip = new System.Windows.Forms.ToolStrip();
      this.systemInfoToolStripButton = new System.Windows.Forms.ToolStripButton();
      this.toolStripSeparator10 = new System.Windows.Forms.ToolStripSeparator();
      this.addChannelToolStripButton = new System.Windows.Forms.ToolStripButton();
      this.deleteChannelToolStripButton = new System.Windows.Forms.ToolStripButton();
      this.toolStripSeparator11 = new System.Windows.Forms.ToolStripSeparator();
      this.exitToolStripButton = new System.Windows.Forms.ToolStripButton();
      this.mainStatusStrip = new System.Windows.Forms.StatusStrip();
      this.channelsCaptionToolStripStatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
      this.channelsToolStripStatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
      this.useMemoryCaptionToolStripStatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
      this.useMemoryToolStripStatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
      this.useCpuCaptionToolStripStatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
      this.useCpuToolStripStatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
      this.usePortsCaptionToolStripStatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
      this.usePortsToolStripStatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
      this.copyrightToolStripStatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
      this.splitContainer = new System.Windows.Forms.SplitContainer();
      this.tabControl = new System.Windows.Forms.TabControl();
      this.channelTabPage = new System.Windows.Forms.TabPage();
      this.channelListView = new System.Windows.Forms.ListView();
      this.hostInfoColumnHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
      this.lastActionTimeColumnHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
      this.responseTimesColumnHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
      this.errorTimesColumnHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
      this.lastErrorColumnHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
      this.channelContextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
      this.addChannelToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.deleteChannelToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.channelImageList = new System.Windows.Forms.ImageList(this.components);
      this.splitter = new System.Windows.Forms.Splitter();
      this.hintRichTextBox = new System.Windows.Forms.RichTextBox();
      this.mainContextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
      this.systemInfoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.toolStripSeparator20 = new System.Windows.Forms.ToolStripSeparator();
      this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.mainNotifyIcon = new System.Windows.Forms.NotifyIcon(this.components);
      this.shutdownTimer = new System.Windows.Forms.Timer(this.components);
      this.monitorTimer = new System.Windows.Forms.Timer(this.components);
      this.mainMenuToolStrip.SuspendLayout();
      this.mainStatusStrip.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)(this.splitContainer)).BeginInit();
      this.splitContainer.Panel1.SuspendLayout();
      this.splitContainer.Panel2.SuspendLayout();
      this.splitContainer.SuspendLayout();
      this.tabControl.SuspendLayout();
      this.channelTabPage.SuspendLayout();
      this.channelContextMenuStrip.SuspendLayout();
      this.mainContextMenuStrip.SuspendLayout();
      this.SuspendLayout();
      // 
      // mainMenuToolStrip
      // 
      this.mainMenuToolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.systemInfoToolStripButton,
            this.toolStripSeparator10,
            this.addChannelToolStripButton,
            this.deleteChannelToolStripButton,
            this.toolStripSeparator11,
            this.exitToolStripButton});
      this.mainMenuToolStrip.Location = new System.Drawing.Point(0, 0);
      this.mainMenuToolStrip.Name = "mainMenuToolStrip";
      this.mainMenuToolStrip.Size = new System.Drawing.Size(1052, 25);
      this.mainMenuToolStrip.TabIndex = 0;
      // 
      // systemInfoToolStripButton
      // 
      this.systemInfoToolStripButton.Name = "systemInfoToolStripButton";
      this.systemInfoToolStripButton.Size = new System.Drawing.Size(76, 22);
      this.systemInfoToolStripButton.Text = "SystemInfo";
      this.systemInfoToolStripButton.Click += new System.EventHandler(this.systemInfoToolStripButton_Click);
      // 
      // toolStripSeparator10
      // 
      this.toolStripSeparator10.Name = "toolStripSeparator10";
      this.toolStripSeparator10.Size = new System.Drawing.Size(6, 25);
      // 
      // addChannelToolStripButton
      // 
      this.addChannelToolStripButton.Name = "addChannelToolStripButton";
      this.addChannelToolStripButton.Size = new System.Drawing.Size(23, 22);
      this.addChannelToolStripButton.Text = "+";
      this.addChannelToolStripButton.Click += new System.EventHandler(this.addChannelToolStripMenuItem_Click);
      // 
      // deleteChannelToolStripButton
      // 
      this.deleteChannelToolStripButton.Name = "deleteChannelToolStripButton";
      this.deleteChannelToolStripButton.Size = new System.Drawing.Size(23, 22);
      this.deleteChannelToolStripButton.Text = "-";
      this.deleteChannelToolStripButton.Click += new System.EventHandler(this.deleteChannelToolStripMenuItem_Click);
      // 
      // toolStripSeparator11
      // 
      this.toolStripSeparator11.Name = "toolStripSeparator11";
      this.toolStripSeparator11.Size = new System.Drawing.Size(6, 25);
      // 
      // exitToolStripButton
      // 
      this.exitToolStripButton.Name = "exitToolStripButton";
      this.exitToolStripButton.Size = new System.Drawing.Size(32, 22);
      this.exitToolStripButton.Text = "Exit";
      this.exitToolStripButton.Click += new System.EventHandler(this.exitToolStripButton_Click);
      // 
      // mainStatusStrip
      // 
      this.mainStatusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.channelsCaptionToolStripStatusLabel,
            this.channelsToolStripStatusLabel,
            this.useMemoryCaptionToolStripStatusLabel,
            this.useMemoryToolStripStatusLabel,
            this.useCpuCaptionToolStripStatusLabel,
            this.useCpuToolStripStatusLabel,
            this.usePortsCaptionToolStripStatusLabel,
            this.usePortsToolStripStatusLabel,
            this.copyrightToolStripStatusLabel});
      this.mainStatusStrip.Location = new System.Drawing.Point(0, 547);
      this.mainStatusStrip.Name = "mainStatusStrip";
      this.mainStatusStrip.RenderMode = System.Windows.Forms.ToolStripRenderMode.ManagerRenderMode;
      this.mainStatusStrip.Size = new System.Drawing.Size(1052, 26);
      this.mainStatusStrip.TabIndex = 2;
      // 
      // channelsCaptionToolStripStatusLabel
      // 
      this.channelsCaptionToolStripStatusLabel.AutoSize = false;
      this.channelsCaptionToolStripStatusLabel.Name = "channelsCaptionToolStripStatusLabel";
      this.channelsCaptionToolStripStatusLabel.Size = new System.Drawing.Size(70, 21);
      this.channelsCaptionToolStripStatusLabel.Text = "channels";
      this.channelsCaptionToolStripStatusLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
      // 
      // channelsToolStripStatusLabel
      // 
      this.channelsToolStripStatusLabel.AutoSize = false;
      this.channelsToolStripStatusLabel.BorderSides = ((System.Windows.Forms.ToolStripStatusLabelBorderSides)((((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left | System.Windows.Forms.ToolStripStatusLabelBorderSides.Top) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Right) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom)));
      this.channelsToolStripStatusLabel.BorderStyle = System.Windows.Forms.Border3DStyle.SunkenOuter;
      this.channelsToolStripStatusLabel.Name = "channelsToolStripStatusLabel";
      this.channelsToolStripStatusLabel.Size = new System.Drawing.Size(80, 21);
      // 
      // useMemoryCaptionToolStripStatusLabel
      // 
      this.useMemoryCaptionToolStripStatusLabel.AutoSize = false;
      this.useMemoryCaptionToolStripStatusLabel.Name = "useMemoryCaptionToolStripStatusLabel";
      this.useMemoryCaptionToolStripStatusLabel.Size = new System.Drawing.Size(70, 21);
      this.useMemoryCaptionToolStripStatusLabel.Text = "memory";
      this.useMemoryCaptionToolStripStatusLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
      // 
      // useMemoryToolStripStatusLabel
      // 
      this.useMemoryToolStripStatusLabel.AutoSize = false;
      this.useMemoryToolStripStatusLabel.BorderSides = ((System.Windows.Forms.ToolStripStatusLabelBorderSides)((((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left | System.Windows.Forms.ToolStripStatusLabelBorderSides.Top) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Right) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom)));
      this.useMemoryToolStripStatusLabel.BorderStyle = System.Windows.Forms.Border3DStyle.SunkenOuter;
      this.useMemoryToolStripStatusLabel.Name = "useMemoryToolStripStatusLabel";
      this.useMemoryToolStripStatusLabel.Size = new System.Drawing.Size(80, 21);
      // 
      // useCpuCaptionToolStripStatusLabel
      // 
      this.useCpuCaptionToolStripStatusLabel.AutoSize = false;
      this.useCpuCaptionToolStripStatusLabel.Name = "useCpuCaptionToolStripStatusLabel";
      this.useCpuCaptionToolStripStatusLabel.Size = new System.Drawing.Size(40, 21);
      this.useCpuCaptionToolStripStatusLabel.Text = "cpu";
      this.useCpuCaptionToolStripStatusLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
      // 
      // useCpuToolStripStatusLabel
      // 
      this.useCpuToolStripStatusLabel.AutoSize = false;
      this.useCpuToolStripStatusLabel.BorderSides = ((System.Windows.Forms.ToolStripStatusLabelBorderSides)((((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left | System.Windows.Forms.ToolStripStatusLabelBorderSides.Top) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Right) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom)));
      this.useCpuToolStripStatusLabel.BorderStyle = System.Windows.Forms.Border3DStyle.SunkenOuter;
      this.useCpuToolStripStatusLabel.Name = "useCpuToolStripStatusLabel";
      this.useCpuToolStripStatusLabel.Size = new System.Drawing.Size(80, 21);
      // 
      // usePortsCaptionToolStripStatusLabel
      // 
      this.usePortsCaptionToolStripStatusLabel.AutoSize = false;
      this.usePortsCaptionToolStripStatusLabel.Name = "usePortsCaptionToolStripStatusLabel";
      this.usePortsCaptionToolStripStatusLabel.Size = new System.Drawing.Size(55, 21);
      this.usePortsCaptionToolStripStatusLabel.Text = "ports";
      this.usePortsCaptionToolStripStatusLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
      // 
      // usePortsToolStripStatusLabel
      // 
      this.usePortsToolStripStatusLabel.AutoSize = false;
      this.usePortsToolStripStatusLabel.BorderSides = ((System.Windows.Forms.ToolStripStatusLabelBorderSides)((((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left | System.Windows.Forms.ToolStripStatusLabelBorderSides.Top) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Right) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom)));
      this.usePortsToolStripStatusLabel.BorderStyle = System.Windows.Forms.Border3DStyle.SunkenOuter;
      this.usePortsToolStripStatusLabel.Name = "usePortsToolStripStatusLabel";
      this.usePortsToolStripStatusLabel.Size = new System.Drawing.Size(80, 21);
      // 
      // copyrightToolStripStatusLabel
      // 
      this.copyrightToolStripStatusLabel.Name = "copyrightToolStripStatusLabel";
      this.copyrightToolStripStatusLabel.Size = new System.Drawing.Size(482, 21);
      this.copyrightToolStripStatusLabel.Spring = true;
      // 
      // splitContainer
      // 
      this.splitContainer.Dock = System.Windows.Forms.DockStyle.Fill;
      this.splitContainer.Location = new System.Drawing.Point(0, 25);
      this.splitContainer.Name = "splitContainer";
      this.splitContainer.Orientation = System.Windows.Forms.Orientation.Horizontal;
      // 
      // splitContainer.Panel1
      // 
      this.splitContainer.Panel1.Controls.Add(this.tabControl);
      // 
      // splitContainer.Panel2
      // 
      this.splitContainer.Panel2.Controls.Add(this.splitter);
      this.splitContainer.Panel2.Controls.Add(this.hintRichTextBox);
      this.splitContainer.Size = new System.Drawing.Size(1052, 522);
      this.splitContainer.SplitterDistance = 387;
      this.splitContainer.TabIndex = 4;
      // 
      // tabControl
      // 
      this.tabControl.Controls.Add(this.channelTabPage);
      this.tabControl.Dock = System.Windows.Forms.DockStyle.Fill;
      this.tabControl.Location = new System.Drawing.Point(0, 0);
      this.tabControl.Name = "tabControl";
      this.tabControl.SelectedIndex = 0;
      this.tabControl.Size = new System.Drawing.Size(1052, 387);
      this.tabControl.TabIndex = 0;
      // 
      // channelTabPage
      // 
      this.channelTabPage.Controls.Add(this.channelListView);
      this.channelTabPage.Location = new System.Drawing.Point(4, 22);
      this.channelTabPage.Name = "channelTabPage";
      this.channelTabPage.Padding = new System.Windows.Forms.Padding(3);
      this.channelTabPage.Size = new System.Drawing.Size(1044, 361);
      this.channelTabPage.TabIndex = 0;
      this.channelTabPage.Text = "Channels";
      this.channelTabPage.UseVisualStyleBackColor = true;
      // 
      // channelListView
      // 
      this.channelListView.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
      this.channelListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.hostInfoColumnHeader,
            this.lastActionTimeColumnHeader,
            this.responseTimesColumnHeader,
            this.errorTimesColumnHeader,
            this.lastErrorColumnHeader});
      this.channelListView.ContextMenuStrip = this.channelContextMenuStrip;
      this.channelListView.Dock = System.Windows.Forms.DockStyle.Fill;
      this.channelListView.FullRowSelect = true;
      this.channelListView.GridLines = true;
      this.channelListView.Location = new System.Drawing.Point(3, 3);
      this.channelListView.Name = "channelListView";
      this.channelListView.Size = new System.Drawing.Size(1038, 355);
      this.channelListView.SmallImageList = this.channelImageList;
      this.channelListView.Sorting = System.Windows.Forms.SortOrder.Ascending;
      this.channelListView.TabIndex = 0;
      this.channelListView.UseCompatibleStateImageBehavior = false;
      this.channelListView.View = System.Windows.Forms.View.Details;
      // 
      // hostInfoColumnHeader
      // 
      this.hostInfoColumnHeader.Text = "Host URL : WebAPI Port | WebSocket Port";
      this.hostInfoColumnHeader.Width = 320;
      // 
      // lastActionTimeColumnHeader
      // 
      this.lastActionTimeColumnHeader.Text = "LastActionTime";
      this.lastActionTimeColumnHeader.Width = 176;
      // 
      // responseTimesColumnHeader
      // 
      this.responseTimesColumnHeader.Text = "ResponseTimes(1min)";
      this.responseTimesColumnHeader.Width = 130;
      // 
      // errorTimesColumnHeader
      // 
      this.errorTimesColumnHeader.Text = "ErrorTimes(1min)";
      this.errorTimesColumnHeader.Width = 130;
      // 
      // lastErrorColumnHeader
      // 
      this.lastErrorColumnHeader.Text = "LastError";
      this.lastErrorColumnHeader.Width = 500;
      // 
      // channelContextMenuStrip
      // 
      this.channelContextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.addChannelToolStripMenuItem,
            this.deleteChannelToolStripMenuItem});
      this.channelContextMenuStrip.Name = "pluginsContextMenuStrip";
      this.channelContextMenuStrip.Size = new System.Drawing.Size(160, 48);
      // 
      // addChannelToolStripMenuItem
      // 
      this.addChannelToolStripMenuItem.Name = "addChannelToolStripMenuItem";
      this.addChannelToolStripMenuItem.Size = new System.Drawing.Size(159, 22);
      this.addChannelToolStripMenuItem.Text = "AddChannel";
      this.addChannelToolStripMenuItem.Click += new System.EventHandler(this.addChannelToolStripMenuItem_Click);
      // 
      // deleteChannelToolStripMenuItem
      // 
      this.deleteChannelToolStripMenuItem.Name = "deleteChannelToolStripMenuItem";
      this.deleteChannelToolStripMenuItem.Size = new System.Drawing.Size(159, 22);
      this.deleteChannelToolStripMenuItem.Text = "DeleteChannel";
      this.deleteChannelToolStripMenuItem.Click += new System.EventHandler(this.deleteChannelToolStripMenuItem_Click);
      // 
      // channelImageList
      // 
      this.channelImageList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("channelImageList.ImageStream")));
      this.channelImageList.TransparentColor = System.Drawing.Color.Transparent;
      this.channelImageList.Images.SetKeyName(0, "Standby.ico");
      this.channelImageList.Images.SetKeyName(1, "On.ico");
      this.channelImageList.Images.SetKeyName(2, "Off.ico");
      // 
      // splitter
      // 
      this.splitter.Dock = System.Windows.Forms.DockStyle.Top;
      this.splitter.Location = new System.Drawing.Point(0, 0);
      this.splitter.Name = "splitter";
      this.splitter.Size = new System.Drawing.Size(1052, 3);
      this.splitter.TabIndex = 0;
      this.splitter.TabStop = false;
      // 
      // hintRichTextBox
      // 
      this.hintRichTextBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
      this.hintRichTextBox.ContextMenuStrip = this.mainContextMenuStrip;
      this.hintRichTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
      this.hintRichTextBox.Location = new System.Drawing.Point(0, 0);
      this.hintRichTextBox.Name = "hintRichTextBox";
      this.hintRichTextBox.Size = new System.Drawing.Size(1052, 131);
      this.hintRichTextBox.TabIndex = 0;
      this.hintRichTextBox.Text = "";
      // 
      // mainContextMenuStrip
      // 
      this.mainContextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.systemInfoToolStripMenuItem,
            this.toolStripSeparator20,
            this.exitToolStripMenuItem});
      this.mainContextMenuStrip.Name = "AssemblyContextMenuStrip";
      this.mainContextMenuStrip.ShowItemToolTips = false;
      this.mainContextMenuStrip.Size = new System.Drawing.Size(141, 54);
      // 
      // systemInfoToolStripMenuItem
      // 
      this.systemInfoToolStripMenuItem.Name = "systemInfoToolStripMenuItem";
      this.systemInfoToolStripMenuItem.Size = new System.Drawing.Size(140, 22);
      this.systemInfoToolStripMenuItem.Text = "SystemInfo";
      this.systemInfoToolStripMenuItem.Click += new System.EventHandler(this.systemInfoToolStripButton_Click);
      // 
      // toolStripSeparator20
      // 
      this.toolStripSeparator20.Name = "toolStripSeparator20";
      this.toolStripSeparator20.Size = new System.Drawing.Size(137, 6);
      // 
      // exitToolStripMenuItem
      // 
      this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
      this.exitToolStripMenuItem.Size = new System.Drawing.Size(140, 22);
      this.exitToolStripMenuItem.Text = "Exit";
      this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripButton_Click);
      // 
      // mainNotifyIcon
      // 
      this.mainNotifyIcon.ContextMenuStrip = this.mainContextMenuStrip;
      this.mainNotifyIcon.Icon = ((System.Drawing.Icon)(resources.GetObject("mainNotifyIcon.Icon")));
      this.mainNotifyIcon.Text = "WebAPI服务负载均衡分拨器";
      this.mainNotifyIcon.Visible = true;
      this.mainNotifyIcon.DoubleClick += new System.EventHandler(this.mainNotifyIcon_DoubleClick);
      // 
      // shutdownTimer
      // 
      this.shutdownTimer.Interval = 1000;
      this.shutdownTimer.Tick += new System.EventHandler(this.shutdownTimer_Tick);
      // 
      // monitorTimer
      // 
      this.monitorTimer.Enabled = true;
      this.monitorTimer.Interval = 1000;
      this.monitorTimer.Tick += new System.EventHandler(this.monitorTimer_Tick);
      // 
      // MainForm
      // 
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
      this.ClientSize = new System.Drawing.Size(1052, 573);
      this.Controls.Add(this.splitContainer);
      this.Controls.Add(this.mainStatusStrip);
      this.Controls.Add(this.mainMenuToolStrip);
      this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
      this.Name = "MainForm";
      this.Text = "WebCluster";
      this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
      this.Shown += new System.EventHandler(this.MainForm_Shown);
      this.Resize += new System.EventHandler(this.MainForm_Resize);
      this.mainMenuToolStrip.ResumeLayout(false);
      this.mainMenuToolStrip.PerformLayout();
      this.mainStatusStrip.ResumeLayout(false);
      this.mainStatusStrip.PerformLayout();
      this.splitContainer.Panel1.ResumeLayout(false);
      this.splitContainer.Panel2.ResumeLayout(false);
      ((System.ComponentModel.ISupportInitialize)(this.splitContainer)).EndInit();
      this.splitContainer.ResumeLayout(false);
      this.tabControl.ResumeLayout(false);
      this.channelTabPage.ResumeLayout(false);
      this.channelContextMenuStrip.ResumeLayout(false);
      this.mainContextMenuStrip.ResumeLayout(false);
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.ToolStrip mainMenuToolStrip;
    private System.Windows.Forms.StatusStrip mainStatusStrip;
    private System.Windows.Forms.SplitContainer splitContainer;
    private System.Windows.Forms.ListView channelListView;
    private System.Windows.Forms.ColumnHeader hostInfoColumnHeader;
    private System.Windows.Forms.ColumnHeader lastActionTimeColumnHeader;
    private System.Windows.Forms.RichTextBox hintRichTextBox;
    private System.Windows.Forms.ContextMenuStrip mainContextMenuStrip;
    private System.Windows.Forms.NotifyIcon mainNotifyIcon;
    private System.Windows.Forms.ToolStripButton exitToolStripButton;
    private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
    private System.Windows.Forms.ToolStripStatusLabel copyrightToolStripStatusLabel;
    private System.Windows.Forms.ImageList channelImageList;
    private System.Windows.Forms.Timer shutdownTimer;
    private System.Windows.Forms.ToolStripSeparator toolStripSeparator10;
    private System.Windows.Forms.ToolStripMenuItem systemInfoToolStripMenuItem;
    private System.Windows.Forms.ToolStripButton systemInfoToolStripButton;
    private System.Windows.Forms.TabControl tabControl;
    private System.Windows.Forms.TabPage channelTabPage;
    private System.Windows.Forms.ContextMenuStrip channelContextMenuStrip;
    private System.Windows.Forms.Splitter splitter;
    private System.Windows.Forms.ToolStripMenuItem addChannelToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem deleteChannelToolStripMenuItem;
    private System.Windows.Forms.ToolStripStatusLabel channelsCaptionToolStripStatusLabel;
    private System.Windows.Forms.ToolStripStatusLabel channelsToolStripStatusLabel;
    private System.Windows.Forms.ToolStripSeparator toolStripSeparator20;
    private System.Windows.Forms.Timer monitorTimer;
    private System.Windows.Forms.ToolStripStatusLabel useMemoryCaptionToolStripStatusLabel;
    private System.Windows.Forms.ToolStripStatusLabel useMemoryToolStripStatusLabel;
    private System.Windows.Forms.ToolStripStatusLabel useCpuCaptionToolStripStatusLabel;
    private System.Windows.Forms.ToolStripStatusLabel usePortsToolStripStatusLabel;
    private System.Windows.Forms.ToolStripStatusLabel usePortsCaptionToolStripStatusLabel;
    private System.Windows.Forms.ToolStripStatusLabel useCpuToolStripStatusLabel;
    private System.Windows.Forms.ColumnHeader responseTimesColumnHeader;
    private System.Windows.Forms.ColumnHeader errorTimesColumnHeader;
    private System.Windows.Forms.ColumnHeader lastErrorColumnHeader;
    private System.Windows.Forms.ToolStripButton addChannelToolStripButton;
    private System.Windows.Forms.ToolStripButton deleteChannelToolStripButton;
    private System.Windows.Forms.ToolStripSeparator toolStripSeparator11;
  }
}

