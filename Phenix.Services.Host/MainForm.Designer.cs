namespace Phenix.Services.Host
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
      this.setDefaultDatabaseToolStripButton = new System.Windows.Forms.ToolStripButton();
      this.systemInfoToolStripButton = new System.Windows.Forms.ToolStripButton();
      this.assemblyInfoToolStripDropDownButton = new System.Windows.Forms.ToolStripDropDownButton();
      this.registerAssemblyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.resetCacheToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.businessCodeFormatToolStripButton = new System.Windows.Forms.ToolStripButton();
      this.toolStripSeparator10 = new System.Windows.Forms.ToolStripSeparator();
      this.synchroHostToolStripButton = new System.Windows.Forms.ToolStripButton();
      this.toolStripSeparator11 = new System.Windows.Forms.ToolStripSeparator();
      this.upgradeServiceToolStripButton = new System.Windows.Forms.ToolStripButton();
      this.markUpgradedToolStripButton = new System.Windows.Forms.ToolStripButton();
      this.markDebuggingToolStripButton = new System.Windows.Forms.ToolStripButton();
      this.suspendServiceToolStripButton = new System.Windows.Forms.ToolStripButton();
      this.toolStripSeparator12 = new System.Windows.Forms.ToolStripSeparator();
      this.exitToolStripButton = new System.Windows.Forms.ToolStripButton();
      this.mainStatusStrip = new System.Windows.Forms.StatusStrip();
      this.usersCaptionToolStripStatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
      this.usersToolStripStatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
      this.pluginsCaptionToolStripStatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
      this.pluginsToolStripStatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
      this.dataSourceCaptionToolStripStatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
      this.dataSourceToolStripStatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
      this.useMemoryCaptionToolStripStatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
      this.useMemoryToolStripStatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
      this.useCpuCaptionToolStripStatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
      this.useCpuToolStripStatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
      this.usePortsCaptionToolStripStatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
      this.usePortsToolStripStatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
      this.copyrightToolStripStatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
      this.splitContainer = new System.Windows.Forms.SplitContainer();
      this.tabControl = new System.Windows.Forms.TabControl();
      this.mainContextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
      this.setDefaultDatabaseToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.systemInfoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.toolStripSeparator20 = new System.Windows.Forms.ToolStripSeparator();
      this.clearPerformanceAnalyseInfosToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.toolStripSeparator21 = new System.Windows.Forms.ToolStripSeparator();
      this.suspendServiceToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.toolStripSeparator22 = new System.Windows.Forms.ToolStripSeparator();
      this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.usersTabPage = new System.Windows.Forms.TabPage();
      this.userListView = new System.Windows.Forms.ListView();
      this.userNumberColumnHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
      this.localAddressColumnHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
      this.lastActionTimeColumnHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
      this.mainImageList = new System.Windows.Forms.ImageList(this.components);
      this.pluginsTabPage = new System.Windows.Forms.TabPage();
      this.pluginListView = new System.Windows.Forms.ListView();
      this.pluginColumnHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
      this.assemblyColumnHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
      this.lastRunTimeColumnHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
      this.hintColumnHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
      this.pluginContextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
      this.addPluginToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.deletePluginToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.openPluginToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.closePluginToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.setupPluginToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.pluginImageList = new System.Windows.Forms.ImageList(this.components);
      this.splitter = new System.Windows.Forms.Splitter();
      this.hintRichTextBox = new System.Windows.Forms.RichTextBox();
      this.mainNotifyIcon = new System.Windows.Forms.NotifyIcon(this.components);
      this.shutdownTimer = new System.Windows.Forms.Timer(this.components);
      this.upgradeTimer = new System.Windows.Forms.Timer(this.components);
      this.pluginHost = new Phenix.Core.Plugin.PluginHost(this.components);
      this.monitorTimer = new System.Windows.Forms.Timer(this.components);
      this.mainMenuToolStrip.SuspendLayout();
      this.mainStatusStrip.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)(this.splitContainer)).BeginInit();
      this.splitContainer.Panel1.SuspendLayout();
      this.splitContainer.Panel2.SuspendLayout();
      this.splitContainer.SuspendLayout();
      this.tabControl.SuspendLayout();
      this.mainContextMenuStrip.SuspendLayout();
      this.usersTabPage.SuspendLayout();
      this.pluginsTabPage.SuspendLayout();
      this.pluginContextMenuStrip.SuspendLayout();
      this.SuspendLayout();
      // 
      // mainMenuToolStrip
      // 
      this.mainMenuToolStrip.Enabled = false;
      this.mainMenuToolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.setDefaultDatabaseToolStripButton,
            this.systemInfoToolStripButton,
            this.assemblyInfoToolStripDropDownButton,
            this.businessCodeFormatToolStripButton,
            this.toolStripSeparator10,
            this.markDebuggingToolStripButton,
            this.suspendServiceToolStripButton,
            this.upgradeServiceToolStripButton,
            this.markUpgradedToolStripButton,
            this.toolStripSeparator11,
            this.synchroHostToolStripButton,
            this.toolStripSeparator12,
            this.exitToolStripButton});
      this.mainMenuToolStrip.Location = new System.Drawing.Point(0, 0);
      this.mainMenuToolStrip.Name = "mainMenuToolStrip";
      this.mainMenuToolStrip.Size = new System.Drawing.Size(1052, 25);
      this.mainMenuToolStrip.TabIndex = 0;
      // 
      // setDefaultDatabaseToolStripButton
      // 
      this.setDefaultDatabaseToolStripButton.Name = "setDefaultDatabaseToolStripButton";
      this.setDefaultDatabaseToolStripButton.Size = new System.Drawing.Size(108, 22);
      this.setDefaultDatabaseToolStripButton.Text = "DefaultDatabase";
      this.setDefaultDatabaseToolStripButton.Click += new System.EventHandler(this.setDefaultDatabaseToolStripButton_Click);
      // 
      // systemInfoToolStripButton
      // 
      this.systemInfoToolStripButton.Name = "systemInfoToolStripButton";
      this.systemInfoToolStripButton.Size = new System.Drawing.Size(76, 22);
      this.systemInfoToolStripButton.Text = "SystemInfo";
      this.systemInfoToolStripButton.Click += new System.EventHandler(this.systemInfoToolStripButton_Click);
      // 
      // assemblyInfoToolStripDropDownButton
      // 
      this.assemblyInfoToolStripDropDownButton.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.registerAssemblyToolStripMenuItem,
            this.resetCacheToolStripMenuItem});
      this.assemblyInfoToolStripDropDownButton.Name = "assemblyInfoToolStripDropDownButton";
      this.assemblyInfoToolStripDropDownButton.Size = new System.Drawing.Size(99, 22);
      this.assemblyInfoToolStripDropDownButton.Text = "AssemblyInfo";
      // 
      // registerAssemblyToolStripMenuItem
      // 
      this.registerAssemblyToolStripMenuItem.Name = "registerAssemblyToolStripMenuItem";
      this.registerAssemblyToolStripMenuItem.Size = new System.Drawing.Size(179, 22);
      this.registerAssemblyToolStripMenuItem.Text = "RegisterAssembly";
      this.registerAssemblyToolStripMenuItem.Click += new System.EventHandler(this.registerAssemblyInfoToolStripMenuItem_Click);
      // 
      // resetCacheToolStripMenuItem
      // 
      this.resetCacheToolStripMenuItem.Name = "resetCacheToolStripMenuItem";
      this.resetCacheToolStripMenuItem.Size = new System.Drawing.Size(179, 22);
      this.resetCacheToolStripMenuItem.Text = "ResetCache";
      this.resetCacheToolStripMenuItem.Click += new System.EventHandler(this.resetCacheToolStripMenuItem_Click);
      // 
      // businessCodeFormatToolStripButton
      // 
      this.businessCodeFormatToolStripButton.Name = "businessCodeFormatToolStripButton";
      this.businessCodeFormatToolStripButton.Size = new System.Drawing.Size(134, 22);
      this.businessCodeFormatToolStripButton.Text = "BusinessCodeFormat";
      this.businessCodeFormatToolStripButton.Click += new System.EventHandler(this.businessCodeFormatToolStripButton_Click);
      // 
      // toolStripSeparator10
      // 
      this.toolStripSeparator10.Name = "toolStripSeparator10";
      this.toolStripSeparator10.Size = new System.Drawing.Size(6, 25);
      // 
      // synchroHostToolStripButton
      // 
      this.synchroHostToolStripButton.Name = "synchroHostToolStripButton";
      this.synchroHostToolStripButton.Size = new System.Drawing.Size(85, 22);
      this.synchroHostToolStripButton.Text = "SynchroHost";
      this.synchroHostToolStripButton.Click += new System.EventHandler(this.synchroHostToolStripButton_Click);
      // 
      // toolStripSeparator11
      // 
      this.toolStripSeparator11.Name = "toolStripSeparator11";
      this.toolStripSeparator11.Size = new System.Drawing.Size(6, 25);
      // 
      // upgradeServiceToolStripButton
      // 
      this.upgradeServiceToolStripButton.Name = "upgradeServiceToolStripButton";
      this.upgradeServiceToolStripButton.Size = new System.Drawing.Size(105, 22);
      this.upgradeServiceToolStripButton.Text = "UpgradeService";
      this.upgradeServiceToolStripButton.Click += new System.EventHandler(this.upgradeServiceToolStripButton_Click);
      // 
      // markUpgradedToolStripButton
      // 
      this.markUpgradedToolStripButton.Name = "markUpgradedToolStripButton";
      this.markUpgradedToolStripButton.Size = new System.Drawing.Size(103, 22);
      this.markUpgradedToolStripButton.Text = "MarkUpgraded";
      this.markUpgradedToolStripButton.Click += new System.EventHandler(this.markUpgradedToolStripButton_Click);
      // 
      // markDebuggingToolStripButton
      // 
      this.markDebuggingToolStripButton.CheckOnClick = true;
      this.markDebuggingToolStripButton.Name = "markDebuggingToolStripButton";
      this.markDebuggingToolStripButton.Size = new System.Drawing.Size(108, 22);
      this.markDebuggingToolStripButton.Text = "MarkDebugging";
      this.markDebuggingToolStripButton.Click += new System.EventHandler(this.markDebuggingToolStripButton_Click);
      // 
      // suspendServiceToolStripButton
      // 
      this.suspendServiceToolStripButton.CheckOnClick = true;
      this.suspendServiceToolStripButton.Name = "suspendServiceToolStripButton";
      this.suspendServiceToolStripButton.Size = new System.Drawing.Size(103, 22);
      this.suspendServiceToolStripButton.Text = "SuspendService";
      this.suspendServiceToolStripButton.Click += new System.EventHandler(this.suspendServiceToolStripButton_Click);
      // 
      // toolStripSeparator12
      // 
      this.toolStripSeparator12.Name = "toolStripSeparator12";
      this.toolStripSeparator12.Size = new System.Drawing.Size(6, 25);
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
            this.usersCaptionToolStripStatusLabel,
            this.usersToolStripStatusLabel,
            this.pluginsCaptionToolStripStatusLabel,
            this.pluginsToolStripStatusLabel,
            this.dataSourceCaptionToolStripStatusLabel,
            this.dataSourceToolStripStatusLabel,
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
      // usersCaptionToolStripStatusLabel
      // 
      this.usersCaptionToolStripStatusLabel.AutoSize = false;
      this.usersCaptionToolStripStatusLabel.Name = "usersCaptionToolStripStatusLabel";
      this.usersCaptionToolStripStatusLabel.Size = new System.Drawing.Size(50, 21);
      this.usersCaptionToolStripStatusLabel.Text = "users";
      this.usersCaptionToolStripStatusLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
      // 
      // usersToolStripStatusLabel
      // 
      this.usersToolStripStatusLabel.AutoSize = false;
      this.usersToolStripStatusLabel.BorderSides = ((System.Windows.Forms.ToolStripStatusLabelBorderSides)((((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left | System.Windows.Forms.ToolStripStatusLabelBorderSides.Top) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Right) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom)));
      this.usersToolStripStatusLabel.BorderStyle = System.Windows.Forms.Border3DStyle.SunkenOuter;
      this.usersToolStripStatusLabel.Name = "usersToolStripStatusLabel";
      this.usersToolStripStatusLabel.Size = new System.Drawing.Size(50, 21);
      // 
      // pluginsCaptionToolStripStatusLabel
      // 
      this.pluginsCaptionToolStripStatusLabel.AutoSize = false;
      this.pluginsCaptionToolStripStatusLabel.Name = "pluginsCaptionToolStripStatusLabel";
      this.pluginsCaptionToolStripStatusLabel.Size = new System.Drawing.Size(60, 21);
      this.pluginsCaptionToolStripStatusLabel.Text = "plugins";
      this.pluginsCaptionToolStripStatusLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
      // 
      // pluginsToolStripStatusLabel
      // 
      this.pluginsToolStripStatusLabel.AutoSize = false;
      this.pluginsToolStripStatusLabel.BorderSides = ((System.Windows.Forms.ToolStripStatusLabelBorderSides)((((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left | System.Windows.Forms.ToolStripStatusLabelBorderSides.Top) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Right) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom)));
      this.pluginsToolStripStatusLabel.BorderStyle = System.Windows.Forms.Border3DStyle.SunkenOuter;
      this.pluginsToolStripStatusLabel.Name = "pluginsToolStripStatusLabel";
      this.pluginsToolStripStatusLabel.Size = new System.Drawing.Size(50, 21);
      // 
      // dataSourceCaptionToolStripStatusLabel
      // 
      this.dataSourceCaptionToolStripStatusLabel.AutoSize = false;
      this.dataSourceCaptionToolStripStatusLabel.Name = "dataSourceCaptionToolStripStatusLabel";
      this.dataSourceCaptionToolStripStatusLabel.Size = new System.Drawing.Size(85, 21);
      this.dataSourceCaptionToolStripStatusLabel.Text = "datasource";
      this.dataSourceCaptionToolStripStatusLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
      // 
      // dataSourceToolStripStatusLabel
      // 
      this.dataSourceToolStripStatusLabel.AutoSize = false;
      this.dataSourceToolStripStatusLabel.BorderSides = ((System.Windows.Forms.ToolStripStatusLabelBorderSides)((((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left | System.Windows.Forms.ToolStripStatusLabelBorderSides.Top) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Right) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom)));
      this.dataSourceToolStripStatusLabel.BorderStyle = System.Windows.Forms.Border3DStyle.SunkenOuter;
      this.dataSourceToolStripStatusLabel.Name = "dataSourceToolStripStatusLabel";
      this.dataSourceToolStripStatusLabel.Size = new System.Drawing.Size(180, 21);
      this.dataSourceToolStripStatusLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
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
      this.copyrightToolStripStatusLabel.Size = new System.Drawing.Size(157, 21);
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
      this.tabControl.ContextMenuStrip = this.mainContextMenuStrip;
      this.tabControl.Controls.Add(this.usersTabPage);
      this.tabControl.Controls.Add(this.pluginsTabPage);
      this.tabControl.Dock = System.Windows.Forms.DockStyle.Fill;
      this.tabControl.Location = new System.Drawing.Point(0, 0);
      this.tabControl.Name = "tabControl";
      this.tabControl.SelectedIndex = 0;
      this.tabControl.Size = new System.Drawing.Size(1052, 387);
      this.tabControl.TabIndex = 0;
      // 
      // mainContextMenuStrip
      // 
      this.mainContextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.setDefaultDatabaseToolStripMenuItem,
            this.systemInfoToolStripMenuItem,
            this.toolStripSeparator20,
            this.clearPerformanceAnalyseInfosToolStripMenuItem,
            this.toolStripSeparator21,
            this.suspendServiceToolStripMenuItem,
            this.toolStripSeparator22,
            this.exitToolStripMenuItem});
      this.mainContextMenuStrip.Name = "AssemblyContextMenuStrip";
      this.mainContextMenuStrip.ShowItemToolTips = false;
      this.mainContextMenuStrip.Size = new System.Drawing.Size(254, 132);
      // 
      // setDefaultDatabaseToolStripMenuItem
      // 
      this.setDefaultDatabaseToolStripMenuItem.Name = "setDefaultDatabaseToolStripMenuItem";
      this.setDefaultDatabaseToolStripMenuItem.Size = new System.Drawing.Size(253, 22);
      this.setDefaultDatabaseToolStripMenuItem.Text = "DefaultDatabase";
      this.setDefaultDatabaseToolStripMenuItem.Click += new System.EventHandler(this.setDefaultDatabaseToolStripButton_Click);
      // 
      // systemInfoToolStripMenuItem
      // 
      this.systemInfoToolStripMenuItem.Name = "systemInfoToolStripMenuItem";
      this.systemInfoToolStripMenuItem.Size = new System.Drawing.Size(253, 22);
      this.systemInfoToolStripMenuItem.Text = "SystemInfo";
      this.systemInfoToolStripMenuItem.Click += new System.EventHandler(this.systemInfoToolStripButton_Click);
      // 
      // toolStripSeparator20
      // 
      this.toolStripSeparator20.Name = "toolStripSeparator20";
      this.toolStripSeparator20.Size = new System.Drawing.Size(250, 6);
      // 
      // clearPerformanceAnalyseInfosToolStripMenuItem
      // 
      this.clearPerformanceAnalyseInfosToolStripMenuItem.Name = "clearPerformanceAnalyseInfosToolStripMenuItem";
      this.clearPerformanceAnalyseInfosToolStripMenuItem.Size = new System.Drawing.Size(253, 22);
      this.clearPerformanceAnalyseInfosToolStripMenuItem.Text = "ClearPerformanceAnalyseInfos";
      this.clearPerformanceAnalyseInfosToolStripMenuItem.Click += new System.EventHandler(this.clearPerformanceAnalyseInfosToolStripMenuItem_Click);
      // 
      // toolStripSeparator21
      // 
      this.toolStripSeparator21.Name = "toolStripSeparator21";
      this.toolStripSeparator21.Size = new System.Drawing.Size(250, 6);
      // 
      // suspendServiceToolStripMenuItem
      // 
      this.suspendServiceToolStripMenuItem.Name = "suspendServiceToolStripMenuItem";
      this.suspendServiceToolStripMenuItem.Size = new System.Drawing.Size(253, 22);
      this.suspendServiceToolStripMenuItem.Text = "SuspendService";
      this.suspendServiceToolStripMenuItem.Click += new System.EventHandler(this.suspendServiceToolStripButton_Click);
      // 
      // toolStripSeparator22
      // 
      this.toolStripSeparator22.Name = "toolStripSeparator22";
      this.toolStripSeparator22.Size = new System.Drawing.Size(250, 6);
      // 
      // exitToolStripMenuItem
      // 
      this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
      this.exitToolStripMenuItem.Size = new System.Drawing.Size(253, 22);
      this.exitToolStripMenuItem.Text = "Exit";
      this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripButton_Click);
      // 
      // usersTabPage
      // 
      this.usersTabPage.Controls.Add(this.userListView);
      this.usersTabPage.Location = new System.Drawing.Point(4, 22);
      this.usersTabPage.Name = "usersTabPage";
      this.usersTabPage.Padding = new System.Windows.Forms.Padding(3);
      this.usersTabPage.Size = new System.Drawing.Size(1044, 361);
      this.usersTabPage.TabIndex = 0;
      this.usersTabPage.Text = "Users";
      this.usersTabPage.UseVisualStyleBackColor = true;
      // 
      // userListView
      // 
      this.userListView.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
      this.userListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.userNumberColumnHeader,
            this.localAddressColumnHeader,
            this.lastActionTimeColumnHeader});
      this.userListView.ContextMenuStrip = this.mainContextMenuStrip;
      this.userListView.Dock = System.Windows.Forms.DockStyle.Fill;
      this.userListView.FullRowSelect = true;
      this.userListView.GridLines = true;
      this.userListView.Location = new System.Drawing.Point(3, 3);
      this.userListView.Name = "userListView";
      this.userListView.Size = new System.Drawing.Size(1038, 355);
      this.userListView.SmallImageList = this.mainImageList;
      this.userListView.Sorting = System.Windows.Forms.SortOrder.Ascending;
      this.userListView.TabIndex = 0;
      this.userListView.UseCompatibleStateImageBehavior = false;
      this.userListView.View = System.Windows.Forms.View.Details;
      // 
      // userNumberColumnHeader
      // 
      this.userNumberColumnHeader.Text = "UserNumber";
      this.userNumberColumnHeader.Width = 420;
      // 
      // localAddressColumnHeader
      // 
      this.localAddressColumnHeader.Text = "LocalAddress";
      this.localAddressColumnHeader.Width = 420;
      // 
      // lastActionTimeColumnHeader
      // 
      this.lastActionTimeColumnHeader.Text = "LastActionTime";
      this.lastActionTimeColumnHeader.Width = 176;
      // 
      // mainImageList
      // 
      this.mainImageList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("mainImageList.ImageStream")));
      this.mainImageList.TransparentColor = System.Drawing.Color.Transparent;
      this.mainImageList.Images.SetKeyName(0, "Disconnected.ico");
      this.mainImageList.Images.SetKeyName(1, "Connected.ico");
      // 
      // pluginsTabPage
      // 
      this.pluginsTabPage.Controls.Add(this.pluginListView);
      this.pluginsTabPage.Location = new System.Drawing.Point(4, 22);
      this.pluginsTabPage.Name = "pluginsTabPage";
      this.pluginsTabPage.Padding = new System.Windows.Forms.Padding(3);
      this.pluginsTabPage.Size = new System.Drawing.Size(1044, 361);
      this.pluginsTabPage.TabIndex = 1;
      this.pluginsTabPage.Text = "Plugins";
      this.pluginsTabPage.UseVisualStyleBackColor = true;
      // 
      // pluginListView
      // 
      this.pluginListView.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
      this.pluginListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.pluginColumnHeader,
            this.assemblyColumnHeader,
            this.lastRunTimeColumnHeader,
            this.hintColumnHeader});
      this.pluginListView.ContextMenuStrip = this.pluginContextMenuStrip;
      this.pluginListView.Dock = System.Windows.Forms.DockStyle.Fill;
      this.pluginListView.FullRowSelect = true;
      this.pluginListView.GridLines = true;
      this.pluginListView.Location = new System.Drawing.Point(3, 3);
      this.pluginListView.Name = "pluginListView";
      this.pluginListView.Size = new System.Drawing.Size(1038, 355);
      this.pluginListView.SmallImageList = this.pluginImageList;
      this.pluginListView.Sorting = System.Windows.Forms.SortOrder.Ascending;
      this.pluginListView.TabIndex = 1;
      this.pluginListView.UseCompatibleStateImageBehavior = false;
      this.pluginListView.View = System.Windows.Forms.View.Details;
      this.pluginListView.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.pluginListView_MouseDoubleClick);
      // 
      // pluginColumnHeader
      // 
      this.pluginColumnHeader.Text = "Plugin";
      this.pluginColumnHeader.Width = 420;
      // 
      // assemblyColumnHeader
      // 
      this.assemblyColumnHeader.Text = "Assembly";
      this.assemblyColumnHeader.Width = 420;
      // 
      // lastRunTimeColumnHeader
      // 
      this.lastRunTimeColumnHeader.Text = "LastRunTime";
      this.lastRunTimeColumnHeader.Width = 176;
      // 
      // hintColumnHeader
      // 
      this.hintColumnHeader.Text = "Hint";
      this.hintColumnHeader.Width = 500;
      // 
      // pluginContextMenuStrip
      // 
      this.pluginContextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.addPluginToolStripMenuItem,
            this.deletePluginToolStripMenuItem,
            this.openPluginToolStripMenuItem,
            this.closePluginToolStripMenuItem,
            this.setupPluginToolStripMenuItem});
      this.pluginContextMenuStrip.Name = "pluginsContextMenuStrip";
      this.pluginContextMenuStrip.Size = new System.Drawing.Size(149, 114);
      // 
      // addPluginToolStripMenuItem
      // 
      this.addPluginToolStripMenuItem.Name = "addPluginToolStripMenuItem";
      this.addPluginToolStripMenuItem.Size = new System.Drawing.Size(148, 22);
      this.addPluginToolStripMenuItem.Text = "AddPlugin";
      this.addPluginToolStripMenuItem.Click += new System.EventHandler(this.addPluginToolStripMenuItem_Click);
      // 
      // deletePluginToolStripMenuItem
      // 
      this.deletePluginToolStripMenuItem.Name = "deletePluginToolStripMenuItem";
      this.deletePluginToolStripMenuItem.Size = new System.Drawing.Size(148, 22);
      this.deletePluginToolStripMenuItem.Text = "DeletePlugin";
      this.deletePluginToolStripMenuItem.Click += new System.EventHandler(this.deletePluginToolStripMenuItem_Click);
      // 
      // openPluginToolStripMenuItem
      // 
      this.openPluginToolStripMenuItem.Name = "openPluginToolStripMenuItem";
      this.openPluginToolStripMenuItem.Size = new System.Drawing.Size(148, 22);
      this.openPluginToolStripMenuItem.Text = "OpenPlugin";
      this.openPluginToolStripMenuItem.Click += new System.EventHandler(this.openPluginToolStripMenuItem_Click);
      // 
      // closePluginToolStripMenuItem
      // 
      this.closePluginToolStripMenuItem.Name = "closePluginToolStripMenuItem";
      this.closePluginToolStripMenuItem.Size = new System.Drawing.Size(148, 22);
      this.closePluginToolStripMenuItem.Text = "ClosePlugin";
      this.closePluginToolStripMenuItem.Click += new System.EventHandler(this.closePluginToolStripMenuItem_Click);
      // 
      // setupPluginToolStripMenuItem
      // 
      this.setupPluginToolStripMenuItem.Name = "setupPluginToolStripMenuItem";
      this.setupPluginToolStripMenuItem.Size = new System.Drawing.Size(148, 22);
      this.setupPluginToolStripMenuItem.Text = "SetupPlugin";
      this.setupPluginToolStripMenuItem.Click += new System.EventHandler(this.setupPluginToolStripMenuItem_Click);
      // 
      // pluginImageList
      // 
      this.pluginImageList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("pluginImageList.ImageStream")));
      this.pluginImageList.TransparentColor = System.Drawing.Color.Transparent;
      this.pluginImageList.Images.SetKeyName(0, "Off.ico");
      this.pluginImageList.Images.SetKeyName(1, "On.ico");
      // 
      // splitter
      // 
      this.splitter.Cursor = System.Windows.Forms.Cursors.HSplit;
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
      // mainNotifyIcon
      // 
      this.mainNotifyIcon.ContextMenuStrip = this.mainContextMenuStrip;
      this.mainNotifyIcon.Icon = ((System.Drawing.Icon)(resources.GetObject("mainNotifyIcon.Icon")));
      this.mainNotifyIcon.Text = "服务容器";
      this.mainNotifyIcon.Visible = true;
      this.mainNotifyIcon.DoubleClick += new System.EventHandler(this.mainNotifyIcon_DoubleClick);
      // 
      // shutdownTimer
      // 
      this.shutdownTimer.Interval = 1000;
      this.shutdownTimer.Tick += new System.EventHandler(this.shutdownTimer_Tick);
      // 
      // upgradeTimer
      // 
      this.upgradeTimer.Interval = 1000;
      this.upgradeTimer.Tick += new System.EventHandler(this.upgradeTimer_Tick);
      // 
      // pluginHost
      // 
      this.pluginHost.Message += new System.EventHandler<Phenix.Core.Plugin.PluginEventArgs>(this.pluginHost_Message);
      this.pluginHost.Finalized += new System.EventHandler<Phenix.Core.Plugin.PluginEventArgs>(this.pluginHost_Finalized);
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
      this.Text = "ServicesHost";
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
      this.mainContextMenuStrip.ResumeLayout(false);
      this.usersTabPage.ResumeLayout(false);
      this.pluginsTabPage.ResumeLayout(false);
      this.pluginContextMenuStrip.ResumeLayout(false);
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.ToolStrip mainMenuToolStrip;
    private System.Windows.Forms.ToolStripButton setDefaultDatabaseToolStripButton;
    private System.Windows.Forms.StatusStrip mainStatusStrip;
    private System.Windows.Forms.SplitContainer splitContainer;
    private System.Windows.Forms.ListView userListView;
    private System.Windows.Forms.ColumnHeader userNumberColumnHeader;
    private System.Windows.Forms.ColumnHeader lastActionTimeColumnHeader;
    private System.Windows.Forms.RichTextBox hintRichTextBox;
    private System.Windows.Forms.ContextMenuStrip mainContextMenuStrip;
    private System.Windows.Forms.NotifyIcon mainNotifyIcon;
    private System.Windows.Forms.ToolStripButton exitToolStripButton;
    private System.Windows.Forms.ToolStripMenuItem setDefaultDatabaseToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
    private System.Windows.Forms.ToolStripStatusLabel copyrightToolStripStatusLabel;
    private System.Windows.Forms.ToolStripStatusLabel usersToolStripStatusLabel;
    private System.Windows.Forms.ToolStripStatusLabel usersCaptionToolStripStatusLabel;
    private System.Windows.Forms.ColumnHeader localAddressColumnHeader;
    private System.Windows.Forms.ImageList mainImageList;
    private System.Windows.Forms.Timer shutdownTimer;
    private System.Windows.Forms.ToolStripSeparator toolStripSeparator10;
    private System.Windows.Forms.ToolStripButton upgradeServiceToolStripButton;
    private System.Windows.Forms.ToolStripSeparator toolStripSeparator21;
    private System.Windows.Forms.ToolStripMenuItem systemInfoToolStripMenuItem;
    private System.Windows.Forms.ToolStripSeparator toolStripSeparator22;
    private System.Windows.Forms.ToolStripMenuItem suspendServiceToolStripMenuItem;
    private System.Windows.Forms.ToolStripButton systemInfoToolStripButton;
    private System.Windows.Forms.Timer upgradeTimer;
    private System.Windows.Forms.ToolStripMenuItem clearPerformanceAnalyseInfosToolStripMenuItem;
    private System.Windows.Forms.ToolStripButton markDebuggingToolStripButton;
    private System.Windows.Forms.ToolStripSeparator toolStripSeparator12;
    private System.Windows.Forms.ToolStripButton synchroHostToolStripButton;
    private System.Windows.Forms.TabControl tabControl;
    private System.Windows.Forms.TabPage usersTabPage;
    private System.Windows.Forms.TabPage pluginsTabPage;
    private System.Windows.Forms.ListView pluginListView;
    private System.Windows.Forms.ColumnHeader assemblyColumnHeader;
    private System.Windows.Forms.ColumnHeader pluginColumnHeader;
    private System.Windows.Forms.ColumnHeader hintColumnHeader;
    private System.Windows.Forms.ColumnHeader lastRunTimeColumnHeader;
    private System.Windows.Forms.ContextMenuStrip pluginContextMenuStrip;
    private Phenix.Core.Plugin.PluginHost pluginHost;
    private System.Windows.Forms.Splitter splitter;
    private System.Windows.Forms.ToolStripMenuItem addPluginToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem deletePluginToolStripMenuItem;
    private System.Windows.Forms.ImageList pluginImageList;
    private System.Windows.Forms.ToolStripMenuItem openPluginToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem closePluginToolStripMenuItem;
    private System.Windows.Forms.ToolStripStatusLabel pluginsCaptionToolStripStatusLabel;
    private System.Windows.Forms.ToolStripStatusLabel pluginsToolStripStatusLabel;
    private System.Windows.Forms.ToolStripButton suspendServiceToolStripButton;
    private System.Windows.Forms.ToolStripDropDownButton assemblyInfoToolStripDropDownButton;
    private System.Windows.Forms.ToolStripSeparator toolStripSeparator20;
    private System.Windows.Forms.ToolStripStatusLabel dataSourceCaptionToolStripStatusLabel;
    private System.Windows.Forms.ToolStripStatusLabel dataSourceToolStripStatusLabel;
    private System.Windows.Forms.ToolStripMenuItem setupPluginToolStripMenuItem;
    private System.Windows.Forms.Timer monitorTimer;
    private System.Windows.Forms.ToolStripButton markUpgradedToolStripButton;
    private System.Windows.Forms.ToolStripStatusLabel useMemoryCaptionToolStripStatusLabel;
    private System.Windows.Forms.ToolStripStatusLabel useMemoryToolStripStatusLabel;
    private System.Windows.Forms.ToolStripMenuItem registerAssemblyToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem resetCacheToolStripMenuItem;
    private System.Windows.Forms.ToolStripStatusLabel useCpuCaptionToolStripStatusLabel;
    private System.Windows.Forms.ToolStripStatusLabel usePortsToolStripStatusLabel;
    private System.Windows.Forms.ToolStripStatusLabel usePortsCaptionToolStripStatusLabel;
    private System.Windows.Forms.ToolStripStatusLabel useCpuToolStripStatusLabel;
    private System.Windows.Forms.ToolStripButton businessCodeFormatToolStripButton;
    private System.Windows.Forms.ToolStripSeparator toolStripSeparator11;
  }
}

