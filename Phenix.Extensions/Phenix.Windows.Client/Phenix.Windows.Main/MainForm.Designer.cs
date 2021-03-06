namespace Phenix.Windows.Main
{
  /// <summary>
  /// 主窗体
  /// </summary>
  partial class MainForm
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
      this.applicationMenu = new DevExpress.XtraBars.Ribbon.ApplicationMenu(this.components);
      this.barManager = new DevExpress.XtraBars.BarManager(this.components);
      this.mainMenu = new DevExpress.XtraBars.Bar();
      this.bsiSystemSetup = new DevExpress.XtraBars.BarSubItem();
      this.bbiUserManage = new DevExpress.XtraBars.BarButtonItem();
      this.bbiDepartmentManage = new DevExpress.XtraBars.BarButtonItem();
      this.bbiPositionManage = new DevExpress.XtraBars.BarButtonItem();
      this.bbiSectionManage = new DevExpress.XtraBars.BarButtonItem();
      this.bbiRoleManage = new DevExpress.XtraBars.BarButtonItem();
      this.bbiAssemblyManage = new DevExpress.XtraBars.BarButtonItem();
      this.bbiFormClassManage = new DevExpress.XtraBars.BarButtonItem();
      this.bbiTableFilterManage = new DevExpress.XtraBars.BarButtonItem();
      this.bbiProcessLockManage = new DevExpress.XtraBars.BarButtonItem();
      this.bbiWorkflowDesinger = new DevExpress.XtraBars.BarButtonItem();
      this.bbiWorkflowTask = new DevExpress.XtraBars.BarButtonItem();
      this.bbiChangeUser = new DevExpress.XtraBars.BarButtonItem();
      this.bbiExit = new DevExpress.XtraBars.BarButtonItem();
      this.statusBar = new DevExpress.XtraBars.Bar();
      this.bsiUserNameCaption = new DevExpress.XtraBars.BarStaticItem();
      this.bsiUserName = new DevExpress.XtraBars.BarStaticItem();
      this.bsiDepartmentCaption = new DevExpress.XtraBars.BarStaticItem();
      this.bsiDepartment = new DevExpress.XtraBars.BarStaticItem();
      this.bsiPositionCaption = new DevExpress.XtraBars.BarStaticItem();
      this.bsiPosition = new DevExpress.XtraBars.BarStaticItem();
      this.bsiHint = new DevExpress.XtraBars.BarStaticItem();
      this.bsiServicesAddressCaption = new DevExpress.XtraBars.BarStaticItem();
      this.bsiServicesAddress = new DevExpress.XtraBars.BarStaticItem();
      this.bsiCopyright = new DevExpress.XtraBars.BarStaticItem();
      this.barDockControlTop = new DevExpress.XtraBars.BarDockControl();
      this.barDockControlBottom = new DevExpress.XtraBars.BarDockControl();
      this.barDockControlLeft = new DevExpress.XtraBars.BarDockControl();
      this.barDockControlRight = new DevExpress.XtraBars.BarDockControl();
      this.executeAuthorization = new Phenix.Services.Client.Security.ExecuteAuthorization(this.components);
      this.xtraTabbedMdiManager = new DevExpress.XtraTabbedMdi.XtraTabbedMdiManager(this.components);
      this.testBarSubItem = new DevExpress.XtraBars.BarSubItem();
      this.weaveProberTestBarButtonItem = new DevExpress.XtraBars.BarButtonItem();
      ((System.ComponentModel.ISupportInitialize)(this.applicationMenu)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.barManager)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.executeAuthorization)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.xtraTabbedMdiManager)).BeginInit();
      this.SuspendLayout();
      // 
      // applicationMenu
      // 
      this.applicationMenu.Manager = this.barManager;
      this.applicationMenu.Name = "applicationMenu";
      // 
      // barManager
      // 
      this.barManager.AllowCustomization = false;
      this.barManager.AllowMoveBarOnToolbar = false;
      this.barManager.AllowQuickCustomization = false;
      this.barManager.AllowShowToolbarsPopup = false;
      this.barManager.Bars.AddRange(new DevExpress.XtraBars.Bar[] {
            this.mainMenu,
            this.statusBar});
      this.barManager.DockControls.Add(this.barDockControlTop);
      this.barManager.DockControls.Add(this.barDockControlBottom);
      this.barManager.DockControls.Add(this.barDockControlLeft);
      this.barManager.DockControls.Add(this.barDockControlRight);
      this.barManager.Form = this;
      this.barManager.Items.AddRange(new DevExpress.XtraBars.BarItem[] {
            this.bbiExit,
            this.bsiHint,
            this.bsiUserNameCaption,
            this.bsiUserName,
            this.bsiCopyright,
            this.bsiServicesAddressCaption,
            this.bsiServicesAddress,
            this.bsiSystemSetup,
            this.bbiUserManage,
            this.bbiAssemblyManage,
            this.bbiTableFilterManage,
            this.bbiSectionManage,
            this.bbiRoleManage,
            this.bsiDepartmentCaption,
            this.bsiDepartment,
            this.bsiPositionCaption,
            this.bsiPosition,
            this.bbiChangeUser,
            this.bbiDepartmentManage,
            this.bbiPositionManage,
            this.bbiProcessLockManage,
            this.bbiFormClassManage,
            this.bbiWorkflowDesinger,
            this.bbiWorkflowTask,
            this.testBarSubItem,
            this.weaveProberTestBarButtonItem});
      this.barManager.MainMenu = this.mainMenu;
      this.barManager.MaxItemId = 256;
      this.barManager.StatusBar = this.statusBar;
      // 
      // mainMenu
      // 
      this.mainMenu.BarAppearance.Normal.Font = new System.Drawing.Font("微软雅黑", 9F);
      this.mainMenu.BarAppearance.Normal.Options.UseFont = true;
      this.mainMenu.BarName = "Main menu";
      this.mainMenu.DockCol = 0;
      this.mainMenu.DockRow = 0;
      this.mainMenu.DockStyle = DevExpress.XtraBars.BarDockStyle.Top;
      this.mainMenu.FloatLocation = new System.Drawing.Point(69, 153);
      this.mainMenu.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[] {
            new DevExpress.XtraBars.LinkPersistInfo(this.bsiSystemSetup),
            new DevExpress.XtraBars.LinkPersistInfo(this.bbiWorkflowTask, true),
            new DevExpress.XtraBars.LinkPersistInfo(this.bbiChangeUser, true),
            new DevExpress.XtraBars.LinkPersistInfo(this.bbiExit, true),
            new DevExpress.XtraBars.LinkPersistInfo(this.testBarSubItem, true)});
      this.mainMenu.OptionsBar.UseWholeRow = true;
      this.mainMenu.Text = "Main menu";
      // 
      // bsiSystemSetup
      // 
      this.bsiSystemSetup.Caption = "系统配置";
      this.bsiSystemSetup.Id = 47;
      this.bsiSystemSetup.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[] {
            new DevExpress.XtraBars.LinkPersistInfo(this.bbiUserManage),
            new DevExpress.XtraBars.LinkPersistInfo(this.bbiDepartmentManage, true),
            new DevExpress.XtraBars.LinkPersistInfo(this.bbiPositionManage),
            new DevExpress.XtraBars.LinkPersistInfo(this.bbiSectionManage),
            new DevExpress.XtraBars.LinkPersistInfo(this.bbiRoleManage),
            new DevExpress.XtraBars.LinkPersistInfo(this.bbiAssemblyManage, true),
            new DevExpress.XtraBars.LinkPersistInfo(this.bbiFormClassManage),
            new DevExpress.XtraBars.LinkPersistInfo(this.bbiTableFilterManage),
            new DevExpress.XtraBars.LinkPersistInfo(this.bbiProcessLockManage, true),
            new DevExpress.XtraBars.LinkPersistInfo(this.bbiWorkflowDesinger, true)});
      this.bsiSystemSetup.Name = "bsiSystemSetup";
      // 
      // bbiUserManage
      // 
      this.executeAuthorization.SetApplyAuthorization(this.bbiUserManage, true);
      this.bbiUserManage.Caption = "用户管理";
      this.bbiUserManage.Id = 99;
      this.bbiUserManage.Name = "bbiUserManage";
      this.bbiUserManage.Tag = "Phenix.Security.Windows.UserManage";
      this.bbiUserManage.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.bbiUserManage_ItemClick);
      // 
      // bbiDepartmentManage
      // 
      this.executeAuthorization.SetApplyAuthorization(this.bbiDepartmentManage, true);
      this.bbiDepartmentManage.Caption = "部门管理";
      this.bbiDepartmentManage.Id = 248;
      this.bbiDepartmentManage.Name = "bbiDepartmentManage";
      this.bbiDepartmentManage.Tag = "Phenix.Security.Windows.DepartmentManage";
      this.bbiDepartmentManage.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.bbiUserManage_ItemClick);
      // 
      // bbiPositionManage
      // 
      this.executeAuthorization.SetApplyAuthorization(this.bbiPositionManage, true);
      this.bbiPositionManage.Caption = "岗位管理";
      this.bbiPositionManage.Id = 249;
      this.bbiPositionManage.Name = "bbiPositionManage";
      this.bbiPositionManage.Tag = "Phenix.Security.Windows.PositionManage";
      this.bbiPositionManage.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.bbiUserManage_ItemClick);
      // 
      // bbiSectionManage
      // 
      this.executeAuthorization.SetAllowVisible(this.bbiSectionManage, false);
      this.executeAuthorization.SetApplyAuthorization(this.bbiSectionManage, true);
      this.bbiSectionManage.Caption = "切片管理";
      this.bbiSectionManage.Id = 102;
      this.bbiSectionManage.Name = "bbiSectionManage";
      this.bbiSectionManage.Tag = "Phenix.Security.Windows.SectionManage";
      this.bbiSectionManage.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.bbiUserManage_ItemClick);
      // 
      // bbiRoleManage
      // 
      this.executeAuthorization.SetAllowVisible(this.bbiRoleManage, false);
      this.executeAuthorization.SetApplyAuthorization(this.bbiRoleManage, true);
      this.bbiRoleManage.Caption = "角色管理";
      this.bbiRoleManage.Id = 103;
      this.bbiRoleManage.Name = "bbiRoleManage";
      this.bbiRoleManage.Tag = "Phenix.Security.Windows.RoleManage";
      this.bbiRoleManage.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.bbiUserManage_ItemClick);
      // 
      // bbiAssemblyManage
      // 
      this.executeAuthorization.SetAllowVisible(this.bbiAssemblyManage, false);
      this.executeAuthorization.SetApplyAuthorization(this.bbiAssemblyManage, true);
      this.bbiAssemblyManage.Caption = "程序集管理";
      this.bbiAssemblyManage.Id = 100;
      this.bbiAssemblyManage.Name = "bbiAssemblyManage";
      this.bbiAssemblyManage.Tag = "Phenix.Security.Windows.AssemblyManage";
      this.bbiAssemblyManage.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.bbiUserManage_ItemClick);
      // 
      // bbiFormClassManage
      // 
      this.executeAuthorization.SetAllowVisible(this.bbiFormClassManage, false);
      this.executeAuthorization.SetApplyAuthorization(this.bbiFormClassManage, true);
      this.bbiFormClassManage.Caption = "窗体类管理";
      this.bbiFormClassManage.Id = 251;
      this.bbiFormClassManage.Name = "bbiFormClassManage";
      this.bbiFormClassManage.Tag = "Phenix.Security.Windows.FormClassManage";
      this.bbiFormClassManage.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.bbiUserManage_ItemClick);
      // 
      // bbiTableFilterManage
      // 
      this.executeAuthorization.SetAllowVisible(this.bbiTableFilterManage, false);
      this.executeAuthorization.SetApplyAuthorization(this.bbiTableFilterManage, true);
      this.bbiTableFilterManage.Caption = "表过滤器";
      this.bbiTableFilterManage.Id = 101;
      this.bbiTableFilterManage.Name = "bbiTableFilterManage";
      this.bbiTableFilterManage.Tag = "Phenix.Security.Windows.TableFilterManage";
      this.bbiTableFilterManage.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.bbiUserManage_ItemClick);
      // 
      // bbiProcessLockManage
      // 
      this.executeAuthorization.SetApplyAuthorization(this.bbiProcessLockManage, true);
      this.bbiProcessLockManage.Caption = "过程锁管理";
      this.bbiProcessLockManage.Id = 250;
      this.bbiProcessLockManage.Name = "bbiProcessLockManage";
      this.bbiProcessLockManage.Tag = "Phenix.Security.Windows.ProcessLockManage";
      this.bbiProcessLockManage.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.bbiUserManage_ItemClick);
      // 
      // bbiWorkflowDesinger
      // 
      this.bbiWorkflowDesinger.Caption = "工作流设计器";
      this.bbiWorkflowDesinger.Id = 252;
      this.bbiWorkflowDesinger.Name = "bbiWorkflowDesinger";
      this.bbiWorkflowDesinger.Tag = "Phenix.Workflow.Windows.Desinger";
      this.bbiWorkflowDesinger.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.bbiUserManage_ItemClick);
      // 
      // bbiWorkflowTask
      // 
      this.bbiWorkflowTask.Caption = "任务清单";
      this.bbiWorkflowTask.Id = 253;
      this.bbiWorkflowTask.Name = "bbiWorkflowTask";
      this.bbiWorkflowTask.Tag = "Phenix.Workflow.Windows.Task";
      this.bbiWorkflowTask.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.bbiUserManage_ItemClick);
      // 
      // bbiChangeUser
      // 
      this.bbiChangeUser.Caption = "切换用户";
      this.bbiChangeUser.Id = 247;
      this.bbiChangeUser.Name = "bbiChangeUser";
      this.bbiChangeUser.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.bbiChangeUser_ItemClick);
      // 
      // bbiExit
      // 
      this.bbiExit.Caption = "退出";
      this.bbiExit.Id = 6;
      this.bbiExit.Name = "bbiExit";
      this.bbiExit.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.bbiExit_ItemClick);
      // 
      // statusBar
      // 
      this.statusBar.BarName = "Status bar";
      this.statusBar.CanDockStyle = DevExpress.XtraBars.BarCanDockStyle.Bottom;
      this.statusBar.DockCol = 0;
      this.statusBar.DockRow = 0;
      this.statusBar.DockStyle = DevExpress.XtraBars.BarDockStyle.Bottom;
      this.statusBar.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[] {
            new DevExpress.XtraBars.LinkPersistInfo(this.bsiUserNameCaption),
            new DevExpress.XtraBars.LinkPersistInfo(DevExpress.XtraBars.BarLinkUserDefines.PaintStyle, this.bsiUserName, DevExpress.XtraBars.BarItemPaintStyle.Standard),
            new DevExpress.XtraBars.LinkPersistInfo(this.bsiDepartmentCaption),
            new DevExpress.XtraBars.LinkPersistInfo(this.bsiDepartment),
            new DevExpress.XtraBars.LinkPersistInfo(this.bsiPositionCaption),
            new DevExpress.XtraBars.LinkPersistInfo(this.bsiPosition),
            new DevExpress.XtraBars.LinkPersistInfo(this.bsiHint),
            new DevExpress.XtraBars.LinkPersistInfo(this.bsiServicesAddressCaption),
            new DevExpress.XtraBars.LinkPersistInfo(this.bsiServicesAddress),
            new DevExpress.XtraBars.LinkPersistInfo(this.bsiCopyright)});
      this.statusBar.OptionsBar.AllowQuickCustomization = false;
      this.statusBar.OptionsBar.DrawDragBorder = false;
      this.statusBar.OptionsBar.UseWholeRow = true;
      this.statusBar.Text = "Status bar";
      // 
      // bsiUserNameCaption
      // 
      this.bsiUserNameCaption.AutoSize = DevExpress.XtraBars.BarStaticItemSize.None;
      this.bsiUserNameCaption.Caption = "用户";
      this.bsiUserNameCaption.Id = 35;
      this.bsiUserNameCaption.Name = "bsiUserNameCaption";
      this.bsiUserNameCaption.TextAlignment = System.Drawing.StringAlignment.Far;
      this.bsiUserNameCaption.Width = 32;
      // 
      // bsiUserName
      // 
      this.bsiUserName.AutoSize = DevExpress.XtraBars.BarStaticItemSize.None;
      this.bsiUserName.Id = 36;
      this.bsiUserName.Name = "bsiUserName";
      this.bsiUserName.TextAlignment = System.Drawing.StringAlignment.Center;
      this.bsiUserName.Width = 100;
      // 
      // bsiDepartmentCaption
      // 
      this.bsiDepartmentCaption.AutoSize = DevExpress.XtraBars.BarStaticItemSize.None;
      this.bsiDepartmentCaption.Caption = "部门";
      this.bsiDepartmentCaption.Id = 208;
      this.bsiDepartmentCaption.Name = "bsiDepartmentCaption";
      this.bsiDepartmentCaption.TextAlignment = System.Drawing.StringAlignment.Far;
      this.bsiDepartmentCaption.Width = 32;
      // 
      // bsiDepartment
      // 
      this.bsiDepartment.AutoSize = DevExpress.XtraBars.BarStaticItemSize.None;
      this.bsiDepartment.Id = 209;
      this.bsiDepartment.Name = "bsiDepartment";
      this.bsiDepartment.TextAlignment = System.Drawing.StringAlignment.Center;
      this.bsiDepartment.Width = 100;
      // 
      // bsiPositionCaption
      // 
      this.bsiPositionCaption.AutoSize = DevExpress.XtraBars.BarStaticItemSize.None;
      this.bsiPositionCaption.Caption = "岗位";
      this.bsiPositionCaption.Id = 210;
      this.bsiPositionCaption.Name = "bsiPositionCaption";
      this.bsiPositionCaption.TextAlignment = System.Drawing.StringAlignment.Far;
      this.bsiPositionCaption.Width = 32;
      // 
      // bsiPosition
      // 
      this.bsiPosition.AutoSize = DevExpress.XtraBars.BarStaticItemSize.None;
      this.bsiPosition.Id = 211;
      this.bsiPosition.Name = "bsiPosition";
      this.bsiPosition.TextAlignment = System.Drawing.StringAlignment.Center;
      this.bsiPosition.Width = 100;
      // 
      // bsiHint
      // 
      this.bsiHint.AutoSize = DevExpress.XtraBars.BarStaticItemSize.None;
      this.bsiHint.Id = 34;
      this.bsiHint.Name = "bsiHint";
      this.bsiHint.TextAlignment = System.Drawing.StringAlignment.Near;
      this.bsiHint.Width = 260;
      // 
      // bsiServicesAddressCaption
      // 
      this.bsiServicesAddressCaption.AutoSize = DevExpress.XtraBars.BarStaticItemSize.None;
      this.bsiServicesAddressCaption.Caption = "服务";
      this.bsiServicesAddressCaption.Id = 41;
      this.bsiServicesAddressCaption.Name = "bsiServicesAddressCaption";
      this.bsiServicesAddressCaption.TextAlignment = System.Drawing.StringAlignment.Far;
      this.bsiServicesAddressCaption.Width = 32;
      // 
      // bsiServicesAddress
      // 
      this.bsiServicesAddress.AutoSize = DevExpress.XtraBars.BarStaticItemSize.None;
      this.bsiServicesAddress.Id = 42;
      this.bsiServicesAddress.Name = "bsiServicesAddress";
      this.bsiServicesAddress.TextAlignment = System.Drawing.StringAlignment.Center;
      this.bsiServicesAddress.Width = 100;
      // 
      // bsiCopyright
      // 
      this.bsiCopyright.AutoSize = DevExpress.XtraBars.BarStaticItemSize.Spring;
      this.bsiCopyright.Caption = "版权信息";
      this.bsiCopyright.Id = 39;
      this.bsiCopyright.Name = "bsiCopyright";
      this.bsiCopyright.TextAlignment = System.Drawing.StringAlignment.Center;
      this.bsiCopyright.Width = 300;
      // 
      // barDockControlTop
      // 
      this.barDockControlTop.CausesValidation = false;
      this.barDockControlTop.Dock = System.Windows.Forms.DockStyle.Top;
      this.barDockControlTop.Location = new System.Drawing.Point(0, 0);
      this.barDockControlTop.Size = new System.Drawing.Size(1016, 24);
      // 
      // barDockControlBottom
      // 
      this.barDockControlBottom.CausesValidation = false;
      this.barDockControlBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
      this.barDockControlBottom.Location = new System.Drawing.Point(0, 545);
      this.barDockControlBottom.Size = new System.Drawing.Size(1016, 27);
      // 
      // barDockControlLeft
      // 
      this.barDockControlLeft.CausesValidation = false;
      this.barDockControlLeft.Dock = System.Windows.Forms.DockStyle.Left;
      this.barDockControlLeft.Location = new System.Drawing.Point(0, 24);
      this.barDockControlLeft.Size = new System.Drawing.Size(0, 521);
      // 
      // barDockControlRight
      // 
      this.barDockControlRight.CausesValidation = false;
      this.barDockControlRight.Dock = System.Windows.Forms.DockStyle.Right;
      this.barDockControlRight.Location = new System.Drawing.Point(1016, 24);
      this.barDockControlRight.Size = new System.Drawing.Size(0, 521);
      // 
      // executeAuthorization
      // 
      this.executeAuthorization.Host = this;
      // 
      // xtraTabbedMdiManager
      // 
      this.xtraTabbedMdiManager.ClosePageButtonShowMode = DevExpress.XtraTab.ClosePageButtonShowMode.InActiveTabPageHeader;
      this.xtraTabbedMdiManager.MdiParent = this;
      // 
      // testBarSubItem
      // 
      this.testBarSubItem.Caption = "测试";
      this.testBarSubItem.Id = 254;
      this.testBarSubItem.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[] {
            new DevExpress.XtraBars.LinkPersistInfo(this.weaveProberTestBarButtonItem)});
      this.testBarSubItem.Name = "testBarSubItem";
      // 
      // weaveProberTestBarButtonItem
      // 
      this.weaveProberTestBarButtonItem.Caption = "测试织入探针";
      this.weaveProberTestBarButtonItem.Id = 255;
      this.weaveProberTestBarButtonItem.Name = "weaveProberTestBarButtonItem";
      this.weaveProberTestBarButtonItem.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.weaveProberTestBarButtonItem_ItemClick);
      // 
      // MainForm
      // 
      this.ClientSize = new System.Drawing.Size(1016, 572);
      this.Controls.Add(this.barDockControlLeft);
      this.Controls.Add(this.barDockControlRight);
      this.Controls.Add(this.barDockControlBottom);
      this.Controls.Add(this.barDockControlTop);
      this.EnterMoveNextControl = false;
      this.IsMdiContainer = true;
      this.Name = "MainForm";
      this.barManager.SetPopupContextMenu(this, this.applicationMenu);
      this.Text = "主界面";
      this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
      this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
      this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.MainForm_FormClosed);
      this.Load += new System.EventHandler(this.MainForm_Load);
      this.Shown += new System.EventHandler(this.MainForm_Shown);
      ((System.ComponentModel.ISupportInitialize)(this.applicationMenu)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.barManager)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.executeAuthorization)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.xtraTabbedMdiManager)).EndInit();
      this.ResumeLayout(false);

    }

    #endregion

    private DevExpress.XtraBars.Ribbon.ApplicationMenu applicationMenu;
    private DevExpress.XtraBars.BarManager barManager;
    private DevExpress.XtraBars.Bar mainMenu;
    private DevExpress.XtraBars.Bar statusBar;
    private DevExpress.XtraBars.BarDockControl barDockControlTop;
    private DevExpress.XtraBars.BarDockControl barDockControlBottom;
    private DevExpress.XtraBars.BarDockControl barDockControlLeft;
    private DevExpress.XtraBars.BarDockControl barDockControlRight;
    private DevExpress.XtraBars.BarButtonItem bbiExit;
    private DevExpress.XtraBars.BarStaticItem bsiHint;
    private DevExpress.XtraBars.BarStaticItem bsiUserNameCaption;
    private DevExpress.XtraBars.BarStaticItem bsiUserName;
    private DevExpress.XtraBars.BarStaticItem bsiCopyright;
    private DevExpress.XtraBars.BarStaticItem bsiServicesAddressCaption;
    private DevExpress.XtraBars.BarStaticItem bsiServicesAddress;
    private Phenix.Services.Client.Security.ExecuteAuthorization executeAuthorization;
    private DevExpress.XtraTabbedMdi.XtraTabbedMdiManager xtraTabbedMdiManager;
    private DevExpress.XtraBars.BarSubItem bsiSystemSetup;
    private DevExpress.XtraBars.BarButtonItem bbiUserManage;
    private DevExpress.XtraBars.BarButtonItem bbiAssemblyManage;
    private DevExpress.XtraBars.BarButtonItem bbiTableFilterManage;
    private DevExpress.XtraBars.BarButtonItem bbiSectionManage;
    private DevExpress.XtraBars.BarButtonItem bbiRoleManage;
    private DevExpress.XtraBars.BarStaticItem bsiDepartmentCaption;
    private DevExpress.XtraBars.BarStaticItem bsiDepartment;
    private DevExpress.XtraBars.BarStaticItem bsiPositionCaption;
    private DevExpress.XtraBars.BarStaticItem bsiPosition;
    private DevExpress.XtraBars.BarButtonItem bbiChangeUser;
    private DevExpress.XtraBars.BarButtonItem bbiDepartmentManage;
    private DevExpress.XtraBars.BarButtonItem bbiPositionManage;
    private DevExpress.XtraBars.BarButtonItem bbiProcessLockManage;
    private DevExpress.XtraBars.BarButtonItem bbiFormClassManage;
    private DevExpress.XtraBars.BarButtonItem bbiWorkflowDesinger;
    private DevExpress.XtraBars.BarButtonItem bbiWorkflowTask;
    private DevExpress.XtraBars.BarSubItem testBarSubItem;
    private DevExpress.XtraBars.BarButtonItem weaveProberTestBarButtonItem;
  }
}