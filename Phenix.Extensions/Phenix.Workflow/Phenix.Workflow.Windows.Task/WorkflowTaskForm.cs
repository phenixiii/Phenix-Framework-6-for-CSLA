using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Phenix.Core.Operate;
using Phenix.Core.Plugin;
using Phenix.Core.Rule;
using Phenix.Core.Windows;
using Phenix.Core.Workflow;

namespace Phenix.Workflow.Windows.Task
{
  /// <summary>
  /// ������
  /// </summary>
  public partial class WorkflowTaskForm : Phenix.Core.Windows.BaseForm
  {
    /// <summary>
    /// ������
    /// </summary>
    public WorkflowTaskForm()
    {
      InitializeComponent();
    }

    #region ����

    private IList<WorkflowTaskInfo> WorkflowTaskInfos
    {
      get { return BindingSourceHelper.GetDataSourceList(this.workflowTaskInfoBindingSource) as IList<WorkflowTaskInfo>; }
      set
      {
        WorkflowTaskInfo workingWorkflowTaskInfo = WorkingWorkflowTaskInfo;
        this.workflowTaskInfoBindingSource.DataSource = value;
        this.workflowTaskInfoBindingSource.Position = workingWorkflowTaskInfo != null ? this.workflowTaskInfoBindingSource.IndexOf(workingWorkflowTaskInfo) : 0;
      }
    }

    private WorkflowTaskInfo WorkingWorkflowTaskInfo
    {
      get { return BindingSourceHelper.GetDataSourceCurrent(this.workflowTaskInfoBindingSource) as WorkflowTaskInfo; }
    }

    private SelectTaskState SelectedTaskState
    {
      get
      {
        EnumKeyCaption enumKeyCaption = this.selectTaskStateBarEditItem.EditValue as EnumKeyCaption;
        return enumKeyCaption != null ? (SelectTaskState)enumKeyCaption.Value : SelectTaskState.Unfinished;
      }
    }

    #endregion

    #region ����

    private void Initialize()
    {
      EnumKeyCaptionCollection selectTaskStates = EnumKeyCaptionCollection.Fetch<SelectTaskState>();
      this.selectTaskStateRepositoryItemComboBox.Items.Clear();
      foreach (EnumKeyCaption item in selectTaskStates)
        this.selectTaskStateRepositoryItemComboBox.Items.Add(item);

      FetchTask();
    }

    private void ApplyRules()
    {
      this.executeTaskBarButtonItem.Enabled = WorkingWorkflowTaskInfo != null
        && (WorkingWorkflowTaskInfo.State == TaskState.Dispatch || WorkingWorkflowTaskInfo.State == TaskState.Received);
    } 

    private void FetchTask()
    {
      switch (SelectedTaskState)
      {
        case SelectTaskState.New:
          WorkflowTaskInfos = WorkflowHub.FetchWorkflowTask(TaskState.Dispatch | TaskState.Received, DateTime.MinValue, DateTime.MaxValue);
          break;
        case SelectTaskState.Unfinished:
          WorkflowTaskInfos = WorkflowHub.FetchWorkflowTask(TaskState.Dispatch | TaskState.Received | TaskState.Holded | TaskState.Aborted, DateTime.MinValue, DateTime.MaxValue);
          break;
        case SelectTaskState.All:
          WorkflowTaskInfos = WorkflowHub.FetchWorkflowTask(TaskState.Dispatch | TaskState.Received | TaskState.Holded | TaskState.Aborted | TaskState.Completed, DateTime.MinValue, DateTime.MaxValue);
          break;
      }
      ApplyRules();
    }

    private void ReceiveTask(WorkflowTaskInfo workflowTaskInfo)
    {
      if (workflowTaskInfo != null && workflowTaskInfo.State == TaskState.Dispatch)
        workflowTaskInfo.Receive();
    }

    private void ExecuteTask(WorkflowTaskInfo workflowTaskInfo)
    {
      if (workflowTaskInfo != null
        && (workflowTaskInfo.State == TaskState.Dispatch || workflowTaskInfo.State == TaskState.Received))
      {
        object completed = PluginHost.Default.SendSingletonMessage(workflowTaskInfo.PluginAssemblyName, workflowTaskInfo.TaskContext);
        if (!(completed is bool))
          MessageBox.Show("����������ֵ������bool�ͣ�", "����������ʧ��", MessageBoxButtons.OK, MessageBoxIcon.Error);
        else if ((bool)completed)
          workflowTaskInfo.ProceedWorkflow();
      }
    }

    #endregion

    #region ��Ƕ��

    /// <summary>
    /// ѡ������״̬
    /// </summary>
    [Serializable]
    [KeyCaption(FriendlyName = "ѡ������״̬")]
    internal enum SelectTaskState
    {
      /// <summary>
      /// ����������
      /// </summary>
      [EnumCaption("����������")]
      New = 0,

      /// <summary>
      /// δ�������
      /// </summary>
      [EnumCaption("δ�������")]
      Unfinished = 1,
      
      /// <summary>
      /// ȫ��������
      /// </summary>
      [EnumCaption("ȫ��������")]
      All = 2,
    }

    #endregion

    private void WorkflowTaskForm_Shown(object sender, EventArgs e)
    {
      Initialize();
    }

    private void taskFetchTimer_Tick(object sender, EventArgs e)
    {
      FetchTask();
    }

    private void fetchTaskBarButtonItem_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
    {
      FetchTask();
    }

    private void executeTaskBarButtonItem_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
    {
      ExecuteTask(WorkingWorkflowTaskInfo);
    }

    private void selectTaskStateBarEditItem_EditValueChanged(object sender, EventArgs e)
    {
      FetchTask();
    }

    private void taskGridView_CustomDrawCell(object sender, DevExpress.XtraGrid.Views.Base.RowCellCustomDrawEventArgs e)
    {
      WorkflowTaskInfo workflowTaskInfo = taskGridView.GetRow(e.RowHandle) as WorkflowTaskInfo;
      if (workflowTaskInfo == null)
        return;
      switch (workflowTaskInfo.State)
      {
        case TaskState.Dispatch:
          e.Appearance.BackColor = Color.White;
          e.Appearance.ForeColor = workflowTaskInfo.Urgent ? Color.Red : Color.Blue;
          break;
        case TaskState.Received:
          e.Appearance.BackColor = Color.White;
          e.Appearance.ForeColor = workflowTaskInfo.Urgent ? Color.Red : Color.Black;
          break;
        case TaskState.Holded:
        case TaskState.Aborted:
          e.Appearance.BackColor = Color.Bisque;
          e.Appearance.ForeColor = workflowTaskInfo.Urgent ? Color.Red : Color.Black;
          break;
        case TaskState.Completed:
          e.Appearance.BackColor = Color.White;
          e.Appearance.ForeColor = Color.Gray;
          break;
      }
    }

    private void taskGridView_Click(object sender, EventArgs e)
    {
      ReceiveTask(WorkingWorkflowTaskInfo);
    }

    private void taskGridView_DoubleClick(object sender, EventArgs e)
    {
      ExecuteTask(WorkingWorkflowTaskInfo);
    }
  }
}