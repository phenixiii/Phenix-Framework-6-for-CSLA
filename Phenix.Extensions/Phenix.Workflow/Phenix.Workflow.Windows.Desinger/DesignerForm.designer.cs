namespace Phenix.Workflow.Windows.Desinger
{
  partial class DesignerForm
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
      System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DesignerForm));
      this.mainMenu = new System.Windows.Forms.MainMenu(this.components);
      this.FileMenu = new System.Windows.Forms.MenuItem();
      this.newMenuItem = new System.Windows.Forms.MenuItem();
      this.openMenuItem = new System.Windows.Forms.MenuItem();
      this.saveMenuItem = new System.Windows.Forms.MenuItem();
      this.saveAsMenuItem = new System.Windows.Forms.MenuItem();
      this.disableMenuItem = new System.Windows.Forms.MenuItem();
      this.menuItem1 = new System.Windows.Forms.MenuItem();
      this.exitMenuItem = new System.Windows.Forms.MenuItem();
      this.toolStrip = new System.Windows.Forms.ToolStrip();
      this.newToolStripButton = new System.Windows.Forms.ToolStripButton();
      this.openToolStripButton = new System.Windows.Forms.ToolStripButton();
      this.saveToolStripButton = new System.Windows.Forms.ToolStripButton();
      this.disableToolStripButton = new System.Windows.Forms.ToolStripButton();
      this.splitContainer2 = new System.Windows.Forms.SplitContainer();
      this.designerHost = new System.Windows.Forms.Integration.ElementHost();
      this.propertyInspectorTabControl = new System.Windows.Forms.TabControl();
      this.propertyInspectorTabPage = new System.Windows.Forms.TabPage();
      this.propertyInspectorHost = new System.Windows.Forms.Integration.ElementHost();
      this.toolBoxControlHost = new System.Windows.Forms.Integration.ElementHost();
      this.fileSystemWatcher = new System.IO.FileSystemWatcher();
      this.splitContainer1 = new System.Windows.Forms.SplitContainer();
      this.toolBoxTabControl = new System.Windows.Forms.TabControl();
      this.toolBoxTabPage = new System.Windows.Forms.TabPage();
      this.toolStrip.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
      this.splitContainer2.Panel1.SuspendLayout();
      this.splitContainer2.Panel2.SuspendLayout();
      this.splitContainer2.SuspendLayout();
      this.propertyInspectorTabControl.SuspendLayout();
      this.propertyInspectorTabPage.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)(this.fileSystemWatcher)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
      this.splitContainer1.Panel1.SuspendLayout();
      this.splitContainer1.Panel2.SuspendLayout();
      this.splitContainer1.SuspendLayout();
      this.toolBoxTabControl.SuspendLayout();
      this.toolBoxTabPage.SuspendLayout();
      this.SuspendLayout();
      // 
      // mainMenu
      // 
      this.mainMenu.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.FileMenu});
      // 
      // FileMenu
      // 
      this.FileMenu.Index = 0;
      this.FileMenu.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.newMenuItem,
            this.openMenuItem,
            this.saveMenuItem,
            this.saveAsMenuItem,
            this.disableMenuItem,
            this.menuItem1,
            this.exitMenuItem});
      this.FileMenu.Text = "文件";
      // 
      // newMenuItem
      // 
      this.newMenuItem.Index = 0;
      this.newMenuItem.Text = "新建";
      this.newMenuItem.Click += new System.EventHandler(this.newMenuItem_Click);
      // 
      // openMenuItem
      // 
      this.openMenuItem.Index = 1;
      this.openMenuItem.Shortcut = System.Windows.Forms.Shortcut.CtrlO;
      this.openMenuItem.Text = "打开";
      this.openMenuItem.Click += new System.EventHandler(this.openMenuItem_Click);
      // 
      // saveMenuItem
      // 
      this.saveMenuItem.Index = 2;
      this.saveMenuItem.Shortcut = System.Windows.Forms.Shortcut.CtrlS;
      this.saveMenuItem.Text = "保存";
      this.saveMenuItem.Click += new System.EventHandler(this.saveMenuItem_Click);
      // 
      // saveAsMenuItem
      // 
      this.saveAsMenuItem.Index = 3;
      this.saveAsMenuItem.Text = "另存为...";
      this.saveAsMenuItem.Click += new System.EventHandler(this.saveAsMenuItem_Click);
      // 
      // disableMenuItem
      // 
      this.disableMenuItem.Index = 4;
      this.disableMenuItem.Text = "禁用";
      this.disableMenuItem.Click += new System.EventHandler(this.disableMenuItem_Click);
      // 
      // menuItem1
      // 
      this.menuItem1.Index = 5;
      this.menuItem1.Text = "-";
      // 
      // exitMenuItem
      // 
      this.exitMenuItem.Index = 6;
      this.exitMenuItem.Text = "退出";
      this.exitMenuItem.Click += new System.EventHandler(this.exitMenuItem_Click);
      // 
      // toolStrip
      // 
      this.toolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.newToolStripButton,
            this.openToolStripButton,
            this.saveToolStripButton,
            this.disableToolStripButton});
      this.toolStrip.Location = new System.Drawing.Point(0, 0);
      this.toolStrip.Name = "toolStrip";
      this.toolStrip.Size = new System.Drawing.Size(850, 25);
      this.toolStrip.TabIndex = 2;
      // 
      // newToolStripButton
      // 
      this.newToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
      this.newToolStripButton.Image = ((System.Drawing.Image)(resources.GetObject("newToolStripButton.Image")));
      this.newToolStripButton.ImageTransparentColor = System.Drawing.Color.Lime;
      this.newToolStripButton.Name = "newToolStripButton";
      this.newToolStripButton.Size = new System.Drawing.Size(23, 22);
      this.newToolStripButton.Text = "&New";
      this.newToolStripButton.Click += new System.EventHandler(this.newMenuItem_Click);
      // 
      // openToolStripButton
      // 
      this.openToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
      this.openToolStripButton.Image = ((System.Drawing.Image)(resources.GetObject("openToolStripButton.Image")));
      this.openToolStripButton.ImageTransparentColor = System.Drawing.Color.Lime;
      this.openToolStripButton.Name = "openToolStripButton";
      this.openToolStripButton.Size = new System.Drawing.Size(23, 22);
      this.openToolStripButton.Text = "&Open";
      this.openToolStripButton.Click += new System.EventHandler(this.openMenuItem_Click);
      // 
      // saveToolStripButton
      // 
      this.saveToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
      this.saveToolStripButton.Image = ((System.Drawing.Image)(resources.GetObject("saveToolStripButton.Image")));
      this.saveToolStripButton.ImageTransparentColor = System.Drawing.Color.Lime;
      this.saveToolStripButton.Name = "saveToolStripButton";
      this.saveToolStripButton.Size = new System.Drawing.Size(23, 22);
      this.saveToolStripButton.Text = "&Save";
      this.saveToolStripButton.Click += new System.EventHandler(this.saveMenuItem_Click);
      // 
      // disableToolStripButton
      // 
      this.disableToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
      this.disableToolStripButton.Image = ((System.Drawing.Image)(resources.GetObject("disableToolStripButton.Image")));
      this.disableToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
      this.disableToolStripButton.Name = "disableToolStripButton";
      this.disableToolStripButton.Size = new System.Drawing.Size(23, 22);
      this.disableToolStripButton.Text = "&Disable";
      this.disableToolStripButton.Click += new System.EventHandler(this.disableMenuItem_Click);
      // 
      // splitContainer2
      // 
      this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
      this.splitContainer2.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
      this.splitContainer2.Location = new System.Drawing.Point(0, 0);
      this.splitContainer2.Name = "splitContainer2";
      // 
      // splitContainer2.Panel1
      // 
      this.splitContainer2.Panel1.Controls.Add(this.designerHost);
      // 
      // splitContainer2.Panel2
      // 
      this.splitContainer2.Panel2.Controls.Add(this.propertyInspectorTabControl);
      this.splitContainer2.Size = new System.Drawing.Size(635, 472);
      this.splitContainer2.SplitterDistance = 333;
      this.splitContainer2.TabIndex = 3;
      // 
      // designerHost
      // 
      this.designerHost.Dock = System.Windows.Forms.DockStyle.Fill;
      this.designerHost.Location = new System.Drawing.Point(0, 0);
      this.designerHost.Name = "designerHost";
      this.designerHost.Size = new System.Drawing.Size(333, 472);
      this.designerHost.TabIndex = 0;
      this.designerHost.Child = null;
      // 
      // propertyInspectorTabControl
      // 
      this.propertyInspectorTabControl.Controls.Add(this.propertyInspectorTabPage);
      this.propertyInspectorTabControl.Dock = System.Windows.Forms.DockStyle.Fill;
      this.propertyInspectorTabControl.Location = new System.Drawing.Point(0, 0);
      this.propertyInspectorTabControl.Name = "propertyInspectorTabControl";
      this.propertyInspectorTabControl.SelectedIndex = 0;
      this.propertyInspectorTabControl.Size = new System.Drawing.Size(298, 472);
      this.propertyInspectorTabControl.TabIndex = 0;
      // 
      // propertyInspectorTabPage
      // 
      this.propertyInspectorTabPage.Controls.Add(this.propertyInspectorHost);
      this.propertyInspectorTabPage.Location = new System.Drawing.Point(4, 22);
      this.propertyInspectorTabPage.Name = "propertyInspectorTabPage";
      this.propertyInspectorTabPage.Padding = new System.Windows.Forms.Padding(3);
      this.propertyInspectorTabPage.Size = new System.Drawing.Size(290, 446);
      this.propertyInspectorTabPage.TabIndex = 0;
      this.propertyInspectorTabPage.Text = "属性";
      this.propertyInspectorTabPage.UseVisualStyleBackColor = true;
      // 
      // propertyInspectorHost
      // 
      this.propertyInspectorHost.Dock = System.Windows.Forms.DockStyle.Fill;
      this.propertyInspectorHost.Location = new System.Drawing.Point(3, 3);
      this.propertyInspectorHost.Name = "propertyInspectorHost";
      this.propertyInspectorHost.Size = new System.Drawing.Size(284, 440);
      this.propertyInspectorHost.TabIndex = 0;
      this.propertyInspectorHost.Child = null;
      // 
      // toolBoxControlHost
      // 
      this.toolBoxControlHost.Dock = System.Windows.Forms.DockStyle.Fill;
      this.toolBoxControlHost.Location = new System.Drawing.Point(3, 3);
      this.toolBoxControlHost.Name = "toolBoxControlHost";
      this.toolBoxControlHost.Size = new System.Drawing.Size(197, 440);
      this.toolBoxControlHost.TabIndex = 0;
      this.toolBoxControlHost.Child = null;
      // 
      // fileSystemWatcher
      // 
      this.fileSystemWatcher.EnableRaisingEvents = true;
      this.fileSystemWatcher.Filter = "*.dll";
      this.fileSystemWatcher.SynchronizingObject = this;
      this.fileSystemWatcher.Created += new System.IO.FileSystemEventHandler(this.fileSystemWatcher_Created);
      // 
      // splitContainer1
      // 
      this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
      this.splitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
      this.splitContainer1.Location = new System.Drawing.Point(0, 25);
      this.splitContainer1.Name = "splitContainer1";
      // 
      // splitContainer1.Panel1
      // 
      this.splitContainer1.Panel1.Controls.Add(this.toolBoxTabControl);
      // 
      // splitContainer1.Panel2
      // 
      this.splitContainer1.Panel2.Controls.Add(this.splitContainer2);
      this.splitContainer1.Size = new System.Drawing.Size(850, 472);
      this.splitContainer1.SplitterDistance = 211;
      this.splitContainer1.TabIndex = 4;
      // 
      // toolBoxTabControl
      // 
      this.toolBoxTabControl.Controls.Add(this.toolBoxTabPage);
      this.toolBoxTabControl.Dock = System.Windows.Forms.DockStyle.Fill;
      this.toolBoxTabControl.Location = new System.Drawing.Point(0, 0);
      this.toolBoxTabControl.Name = "toolBoxTabControl";
      this.toolBoxTabControl.SelectedIndex = 0;
      this.toolBoxTabControl.Size = new System.Drawing.Size(211, 472);
      this.toolBoxTabControl.TabIndex = 0;
      // 
      // toolBoxTabPage
      // 
      this.toolBoxTabPage.Controls.Add(this.toolBoxControlHost);
      this.toolBoxTabPage.Location = new System.Drawing.Point(4, 22);
      this.toolBoxTabPage.Name = "toolBoxTabPage";
      this.toolBoxTabPage.Padding = new System.Windows.Forms.Padding(3);
      this.toolBoxTabPage.Size = new System.Drawing.Size(203, 446);
      this.toolBoxTabPage.TabIndex = 1;
      this.toolBoxTabPage.Text = "活动";
      this.toolBoxTabPage.UseVisualStyleBackColor = true;
      // 
      // DesignerForm
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(850, 497);
      this.Controls.Add(this.splitContainer1);
      this.Controls.Add(this.toolStrip);
      this.Menu = this.mainMenu;
      this.Name = "DesignerForm";
      this.Text = "工作流设计器";
      this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.DesignerForm_FormClosing);
      this.Load += new System.EventHandler(this.DesignerForm_Load);
      this.toolStrip.ResumeLayout(false);
      this.toolStrip.PerformLayout();
      this.splitContainer2.Panel1.ResumeLayout(false);
      this.splitContainer2.Panel2.ResumeLayout(false);
      ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
      this.splitContainer2.ResumeLayout(false);
      this.propertyInspectorTabControl.ResumeLayout(false);
      this.propertyInspectorTabPage.ResumeLayout(false);
      ((System.ComponentModel.ISupportInitialize)(this.fileSystemWatcher)).EndInit();
      this.splitContainer1.Panel1.ResumeLayout(false);
      this.splitContainer1.Panel2.ResumeLayout(false);
      ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
      this.splitContainer1.ResumeLayout(false);
      this.toolBoxTabControl.ResumeLayout(false);
      this.toolBoxTabPage.ResumeLayout(false);
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.ToolStrip toolStrip;
    private System.Windows.Forms.ToolStripButton newToolStripButton;
    private System.Windows.Forms.ToolStripButton openToolStripButton;
    private System.Windows.Forms.ToolStripButton saveToolStripButton;
    private System.Windows.Forms.MainMenu mainMenu;
    private System.Windows.Forms.MenuItem FileMenu;
    private System.Windows.Forms.MenuItem newMenuItem;
    private System.Windows.Forms.MenuItem openMenuItem;
    private System.Windows.Forms.MenuItem saveMenuItem;
    private System.Windows.Forms.MenuItem saveAsMenuItem;
    private System.Windows.Forms.MenuItem menuItem1;
    private System.Windows.Forms.MenuItem exitMenuItem;
    private System.Windows.Forms.SplitContainer splitContainer1;
    private System.Windows.Forms.TabControl toolBoxTabControl;
    private System.Windows.Forms.TabPage toolBoxTabPage;
    private System.Windows.Forms.Integration.ElementHost toolBoxControlHost;
    private System.IO.FileSystemWatcher fileSystemWatcher;
    private System.Windows.Forms.SplitContainer splitContainer2;
    private System.Windows.Forms.Integration.ElementHost designerHost;
    private System.Windows.Forms.TabControl propertyInspectorTabControl;
    private System.Windows.Forms.ToolStripButton disableToolStripButton;
    private System.Windows.Forms.MenuItem disableMenuItem;
    private System.Windows.Forms.TabPage propertyInspectorTabPage;
    private System.Windows.Forms.Integration.ElementHost propertyInspectorHost;
  }
}

