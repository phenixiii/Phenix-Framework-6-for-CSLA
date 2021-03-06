namespace Phenix.Workflow.Windows.Task
{
  /// <summary>
  /// 主窗体
  /// </summary>
  partial class WorkflowTaskForm
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
      this.barManager = new DevExpress.XtraBars.BarManager(this.components);
      this.toolsBar = new DevExpress.XtraBars.Bar();
      this.fetchTaskBarButtonItem = new DevExpress.XtraBars.BarButtonItem();
      this.executeTaskBarButtonItem = new DevExpress.XtraBars.BarButtonItem();
      this.selectTaskStateBarEditItem = new DevExpress.XtraBars.BarEditItem();
      this.selectTaskStateRepositoryItemComboBox = new DevExpress.XtraEditors.Repository.RepositoryItemComboBox();
      this.statusBar = new DevExpress.XtraBars.Bar();
      this.barDockControlTop = new DevExpress.XtraBars.BarDockControl();
      this.barDockControlBottom = new DevExpress.XtraBars.BarDockControl();
      this.barDockControlLeft = new DevExpress.XtraBars.BarDockControl();
      this.barDockControlRight = new DevExpress.XtraBars.BarDockControl();
      this.workflowTaskInfoBindingSource = new System.Windows.Forms.BindingSource(this.components);
      this.taskFetchTimer = new System.Windows.Forms.Timer(this.components);
      this.taskGridControl = new DevExpress.XtraGrid.GridControl();
      this.taskGridView = new DevExpress.XtraGrid.Views.Grid.GridView();
      this.colCaption = new DevExpress.XtraGrid.Columns.GridColumn();
      this.colMessage = new DevExpress.XtraGrid.Columns.GridColumn();
      this.colDispatchTime = new DevExpress.XtraGrid.Columns.GridColumn();
      this.colReceiveTime = new DevExpress.XtraGrid.Columns.GridColumn();
      this.colCompleteTime = new DevExpress.XtraGrid.Columns.GridColumn();
      this.colWorkflowInstanceId = new DevExpress.XtraGrid.Columns.GridColumn();
      this.colTypeNamespace = new DevExpress.XtraGrid.Columns.GridColumn();
      this.colTypeName = new DevExpress.XtraGrid.Columns.GridColumn();
      this.colTaskContext = new DevExpress.XtraGrid.Columns.GridColumn();
      this.colBookmarkName = new DevExpress.XtraGrid.Columns.GridColumn();
      this.colPluginAssemblyName = new DevExpress.XtraGrid.Columns.GridColumn();
      this.colWorkerRole = new DevExpress.XtraGrid.Columns.GridColumn();
      this.colHoldTime = new DevExpress.XtraGrid.Columns.GridColumn();
      this.colAbortTime = new DevExpress.XtraGrid.Columns.GridColumn();
      this.colDispatchUserNumber = new DevExpress.XtraGrid.Columns.GridColumn();
      ((System.ComponentModel.ISupportInitialize)(this.barManager)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.selectTaskStateRepositoryItemComboBox)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.workflowTaskInfoBindingSource)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.taskGridControl)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.taskGridView)).BeginInit();
      this.SuspendLayout();
      // 
      // barManager
      // 
      this.barManager.Bars.AddRange(new DevExpress.XtraBars.Bar[] {
            this.toolsBar,
            this.statusBar});
      this.barManager.DockControls.Add(this.barDockControlTop);
      this.barManager.DockControls.Add(this.barDockControlBottom);
      this.barManager.DockControls.Add(this.barDockControlLeft);
      this.barManager.DockControls.Add(this.barDockControlRight);
      this.barManager.Form = this;
      this.barManager.Items.AddRange(new DevExpress.XtraBars.BarItem[] {
            this.fetchTaskBarButtonItem,
            this.executeTaskBarButtonItem,
            this.selectTaskStateBarEditItem});
      this.barManager.MaxItemId = 8;
      this.barManager.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.selectTaskStateRepositoryItemComboBox});
      this.barManager.StatusBar = this.statusBar;
      // 
      // toolsBar
      // 
      this.toolsBar.BarName = "Tools";
      this.toolsBar.DockCol = 0;
      this.toolsBar.DockRow = 0;
      this.toolsBar.DockStyle = DevExpress.XtraBars.BarDockStyle.Top;
      this.toolsBar.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[] {
            new DevExpress.XtraBars.LinkPersistInfo(this.fetchTaskBarButtonItem),
            new DevExpress.XtraBars.LinkPersistInfo(this.executeTaskBarButtonItem),
            new DevExpress.XtraBars.LinkPersistInfo(this.selectTaskStateBarEditItem, true)});
      this.toolsBar.Text = "Tools";
      // 
      // fetchTaskBarButtonItem
      // 
      this.fetchTaskBarButtonItem.Caption = "接收任务";
      this.fetchTaskBarButtonItem.Id = 1;
      this.fetchTaskBarButtonItem.Name = "fetchTaskBarButtonItem";
      this.fetchTaskBarButtonItem.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.fetchTaskBarButtonItem_ItemClick);
      // 
      // executeTaskBarButtonItem
      // 
      this.executeTaskBarButtonItem.Caption = "执行任务";
      this.executeTaskBarButtonItem.Id = 2;
      this.executeTaskBarButtonItem.Name = "executeTaskBarButtonItem";
      this.executeTaskBarButtonItem.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.executeTaskBarButtonItem_ItemClick);
      // 
      // selectTaskStateBarEditItem
      // 
      this.selectTaskStateBarEditItem.Edit = this.selectTaskStateRepositoryItemComboBox;
      this.selectTaskStateBarEditItem.Id = 7;
      this.selectTaskStateBarEditItem.Name = "selectTaskStateBarEditItem";
      this.selectTaskStateBarEditItem.Width = 100;
      this.selectTaskStateBarEditItem.EditValueChanged += new System.EventHandler(this.selectTaskStateBarEditItem_EditValueChanged);
      // 
      // selectTaskStateRepositoryItemComboBox
      // 
      this.selectTaskStateRepositoryItemComboBox.AutoHeight = false;
      this.selectTaskStateRepositoryItemComboBox.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
      this.selectTaskStateRepositoryItemComboBox.ImmediatePopup = true;
      this.selectTaskStateRepositoryItemComboBox.Items.AddRange(new object[] {
            "新增的任务",
            "未完成任务",
            "全部的任务"});
      this.selectTaskStateRepositoryItemComboBox.Name = "selectTaskStateRepositoryItemComboBox";
      // 
      // statusBar
      // 
      this.statusBar.BarName = "Status bar";
      this.statusBar.CanDockStyle = DevExpress.XtraBars.BarCanDockStyle.Bottom;
      this.statusBar.DockCol = 0;
      this.statusBar.DockRow = 0;
      this.statusBar.DockStyle = DevExpress.XtraBars.BarDockStyle.Bottom;
      this.statusBar.OptionsBar.AllowQuickCustomization = false;
      this.statusBar.OptionsBar.DrawDragBorder = false;
      this.statusBar.OptionsBar.UseWholeRow = true;
      this.statusBar.Text = "Status bar";
      // 
      // barDockControlTop
      // 
      this.barDockControlTop.CausesValidation = false;
      this.barDockControlTop.Dock = System.Windows.Forms.DockStyle.Top;
      this.barDockControlTop.Location = new System.Drawing.Point(0, 0);
      this.barDockControlTop.Size = new System.Drawing.Size(1046, 31);
      // 
      // barDockControlBottom
      // 
      this.barDockControlBottom.CausesValidation = false;
      this.barDockControlBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
      this.barDockControlBottom.Location = new System.Drawing.Point(0, 493);
      this.barDockControlBottom.Size = new System.Drawing.Size(1046, 23);
      // 
      // barDockControlLeft
      // 
      this.barDockControlLeft.CausesValidation = false;
      this.barDockControlLeft.Dock = System.Windows.Forms.DockStyle.Left;
      this.barDockControlLeft.Location = new System.Drawing.Point(0, 31);
      this.barDockControlLeft.Size = new System.Drawing.Size(0, 462);
      // 
      // barDockControlRight
      // 
      this.barDockControlRight.CausesValidation = false;
      this.barDockControlRight.Dock = System.Windows.Forms.DockStyle.Right;
      this.barDockControlRight.Location = new System.Drawing.Point(1046, 31);
      this.barDockControlRight.Size = new System.Drawing.Size(0, 462);
      // 
      // workflowTaskInfoBindingSource
      // 
      this.workflowTaskInfoBindingSource.DataSource = typeof(Phenix.Core.Workflow.WorkflowTaskInfo);
      // 
      // taskFetchTimer
      // 
      this.taskFetchTimer.Enabled = true;
      this.taskFetchTimer.Interval = 60000;
      this.taskFetchTimer.Tick += new System.EventHandler(this.taskFetchTimer_Tick);
      // 
      // taskGridControl
      // 
      this.taskGridControl.DataSource = this.workflowTaskInfoBindingSource;
      this.taskGridControl.Dock = System.Windows.Forms.DockStyle.Fill;
      this.taskGridControl.Location = new System.Drawing.Point(0, 31);
      this.taskGridControl.MainView = this.taskGridView;
      this.taskGridControl.MenuManager = this.barManager;
      this.taskGridControl.Name = "taskGridControl";
      this.taskGridControl.Size = new System.Drawing.Size(1046, 462);
      this.taskGridControl.TabIndex = 0;
      this.taskGridControl.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.taskGridView});
      // 
      // taskGridView
      // 
      this.taskGridView.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.colCaption,
            this.colMessage,
            this.colDispatchUserNumber,
            this.colDispatchTime,
            this.colReceiveTime,
            this.colCompleteTime});
      this.taskGridView.GridControl = this.taskGridControl;
      this.taskGridView.Name = "taskGridView";
      this.taskGridView.CustomDrawCell += new DevExpress.XtraGrid.Views.Base.RowCellCustomDrawEventHandler(this.taskGridView_CustomDrawCell);
      this.taskGridView.Click += new System.EventHandler(this.taskGridView_Click);
      this.taskGridView.DoubleClick += new System.EventHandler(this.taskGridView_DoubleClick);
      // 
      // colCaption
      // 
      this.colCaption.Caption = "标题";
      this.colCaption.FieldName = "Caption";
      this.colCaption.Name = "colCaption";
      this.colCaption.OptionsColumn.ReadOnly = true;
      this.colCaption.Visible = true;
      this.colCaption.VisibleIndex = 0;
      this.colCaption.Width = 160;
      // 
      // colMessage
      // 
      this.colMessage.Caption = "信息";
      this.colMessage.FieldName = "Message";
      this.colMessage.Name = "colMessage";
      this.colMessage.OptionsColumn.ReadOnly = true;
      this.colMessage.Visible = true;
      this.colMessage.VisibleIndex = 1;
      this.colMessage.Width = 160;
      // 
      // colDispatchTime
      // 
      this.colDispatchTime.Caption = "发布时间";
      this.colDispatchTime.FieldName = "DispatchTime";
      this.colDispatchTime.Name = "colDispatchTime";
      this.colDispatchTime.OptionsColumn.ReadOnly = true;
      this.colDispatchTime.Visible = true;
      this.colDispatchTime.VisibleIndex = 2;
      this.colDispatchTime.Width = 160;
      // 
      // colReceiveTime
      // 
      this.colReceiveTime.Caption = "接收时间";
      this.colReceiveTime.FieldName = "ReceiveTime";
      this.colReceiveTime.Name = "colReceiveTime";
      this.colReceiveTime.OptionsColumn.ReadOnly = true;
      this.colReceiveTime.Visible = true;
      this.colReceiveTime.VisibleIndex = 3;
      this.colReceiveTime.Width = 160;
      // 
      // colCompleteTime
      // 
      this.colCompleteTime.Caption = "执行时间";
      this.colCompleteTime.FieldName = "CompleteTime";
      this.colCompleteTime.Name = "colCompleteTime";
      this.colCompleteTime.OptionsColumn.ReadOnly = true;
      this.colCompleteTime.Visible = true;
      this.colCompleteTime.VisibleIndex = 4;
      this.colCompleteTime.Width = 176;
      // 
      // colWorkflowInstanceId
      // 
      this.colWorkflowInstanceId.FieldName = "WorkflowInstanceId";
      this.colWorkflowInstanceId.Name = "colWorkflowInstanceId";
      this.colWorkflowInstanceId.OptionsColumn.ReadOnly = true;
      this.colWorkflowInstanceId.Visible = true;
      this.colWorkflowInstanceId.VisibleIndex = 7;
      // 
      // colTypeNamespace
      // 
      this.colTypeNamespace.FieldName = "TypeNamespace";
      this.colTypeNamespace.Name = "colTypeNamespace";
      this.colTypeNamespace.OptionsColumn.ReadOnly = true;
      this.colTypeNamespace.Visible = true;
      this.colTypeNamespace.VisibleIndex = 8;
      // 
      // colTypeName
      // 
      this.colTypeName.FieldName = "TypeName";
      this.colTypeName.Name = "colTypeName";
      this.colTypeName.OptionsColumn.ReadOnly = true;
      this.colTypeName.Visible = true;
      this.colTypeName.VisibleIndex = 9;
      // 
      // colTaskContext
      // 
      this.colTaskContext.FieldName = "TaskContext";
      this.colTaskContext.Name = "colTaskContext";
      this.colTaskContext.OptionsColumn.ReadOnly = true;
      this.colTaskContext.Visible = true;
      this.colTaskContext.VisibleIndex = 10;
      // 
      // colBookmarkName
      // 
      this.colBookmarkName.FieldName = "BookmarkName";
      this.colBookmarkName.Name = "colBookmarkName";
      this.colBookmarkName.OptionsColumn.ReadOnly = true;
      this.colBookmarkName.Visible = true;
      this.colBookmarkName.VisibleIndex = 11;
      // 
      // colPluginAssemblyName
      // 
      this.colPluginAssemblyName.FieldName = "PluginAssemblyName";
      this.colPluginAssemblyName.Name = "colPluginAssemblyName";
      this.colPluginAssemblyName.OptionsColumn.ReadOnly = true;
      this.colPluginAssemblyName.Visible = true;
      this.colPluginAssemblyName.VisibleIndex = 12;
      // 
      // colWorkerRole
      // 
      this.colWorkerRole.FieldName = "WorkerRole";
      this.colWorkerRole.Name = "colWorkerRole";
      this.colWorkerRole.OptionsColumn.ReadOnly = true;
      this.colWorkerRole.Visible = true;
      this.colWorkerRole.VisibleIndex = 13;
      // 
      // colHoldTime
      // 
      this.colHoldTime.FieldName = "HoldTime";
      this.colHoldTime.Name = "colHoldTime";
      this.colHoldTime.OptionsColumn.ReadOnly = true;
      this.colHoldTime.Visible = true;
      this.colHoldTime.VisibleIndex = 14;
      // 
      // colAbortTime
      // 
      this.colAbortTime.FieldName = "AbortTime";
      this.colAbortTime.Name = "colAbortTime";
      this.colAbortTime.OptionsColumn.ReadOnly = true;
      this.colAbortTime.Visible = true;
      this.colAbortTime.VisibleIndex = 15;
      // 
      // colDispatchUserNumber
      // 
      this.colDispatchUserNumber.Caption = "发布者";
      this.colDispatchUserNumber.FieldName = "DispatchUserNumber";
      this.colDispatchUserNumber.Name = "colDispatchUserNumber";
      this.colDispatchUserNumber.Visible = true;
      this.colDispatchUserNumber.VisibleIndex = 2;
      // 
      // WorkflowTaskForm
      // 
      this.ClientSize = new System.Drawing.Size(1046, 516);
      this.Controls.Add(this.taskGridControl);
      this.Controls.Add(this.barDockControlLeft);
      this.Controls.Add(this.barDockControlRight);
      this.Controls.Add(this.barDockControlBottom);
      this.Controls.Add(this.barDockControlTop);
      this.Name = "WorkflowTaskForm";
      this.Text = "任务清单";
      this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
      this.Shown += new System.EventHandler(this.WorkflowTaskForm_Shown);
      ((System.ComponentModel.ISupportInitialize)(this.barManager)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.selectTaskStateRepositoryItemComboBox)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.workflowTaskInfoBindingSource)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.taskGridControl)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.taskGridView)).EndInit();
      this.ResumeLayout(false);

    }

    #endregion

    private DevExpress.XtraBars.BarManager barManager;
    private DevExpress.XtraBars.Bar toolsBar;
    private DevExpress.XtraBars.Bar statusBar;
    private DevExpress.XtraBars.BarDockControl barDockControlTop;
    private DevExpress.XtraBars.BarDockControl barDockControlBottom;
    private DevExpress.XtraBars.BarDockControl barDockControlLeft;
    private DevExpress.XtraBars.BarDockControl barDockControlRight;
    private System.Windows.Forms.Timer taskFetchTimer;
    private System.Windows.Forms.BindingSource workflowTaskInfoBindingSource;
    private DevExpress.XtraBars.BarButtonItem fetchTaskBarButtonItem;
    private DevExpress.XtraBars.BarButtonItem executeTaskBarButtonItem;
    private DevExpress.XtraGrid.GridControl taskGridControl;
    private DevExpress.XtraGrid.Views.Grid.GridView taskGridView;
    private DevExpress.XtraGrid.Columns.GridColumn colCaption;
    private DevExpress.XtraGrid.Columns.GridColumn colMessage;
    private DevExpress.XtraGrid.Columns.GridColumn colDispatchTime;
    private DevExpress.XtraGrid.Columns.GridColumn colReceiveTime;
    private DevExpress.XtraGrid.Columns.GridColumn colCompleteTime;
    private DevExpress.XtraGrid.Columns.GridColumn colWorkflowInstanceId;
    private DevExpress.XtraGrid.Columns.GridColumn colTypeNamespace;
    private DevExpress.XtraGrid.Columns.GridColumn colTypeName;
    private DevExpress.XtraGrid.Columns.GridColumn colTaskContext;
    private DevExpress.XtraGrid.Columns.GridColumn colBookmarkName;
    private DevExpress.XtraGrid.Columns.GridColumn colPluginAssemblyName;
    private DevExpress.XtraGrid.Columns.GridColumn colWorkerRole;
    private DevExpress.XtraGrid.Columns.GridColumn colHoldTime;
    private DevExpress.XtraGrid.Columns.GridColumn colAbortTime;
    private DevExpress.XtraGrid.Columns.GridColumn colDispatchUserNumber;
    private DevExpress.XtraBars.BarEditItem selectTaskStateBarEditItem;
    private DevExpress.XtraEditors.Repository.RepositoryItemComboBox selectTaskStateRepositoryItemComboBox;
  }
}