using System;
using System.Drawing;
using System.Windows.Forms;
using Phenix.Core.Windows;
using Phenix.Core.Workflow;

namespace Phenix.Workflow.Windows.Desinger
{
  internal partial class WorkflowInfoSelectDialog : Phenix.Core.Windows.DialogForm
  {
    private WorkflowInfoSelectDialog(WorkflowInfo currentWorkflowInfo)
    {
      InitializeComponent();

      if (WorkflowHub.WorkflowInfos != null)
      {
        this.workflowInfoListBindingSource.DataSource = WorkflowHub.WorkflowInfos.Values;
        this.workflowInfoListBindingSource.Position = this.workflowInfoListBindingSource.IndexOf(currentWorkflowInfo);
      }
    }

    #region 工厂

    /// <summary>
    /// 执行
    /// </summary>
    public static WorkflowInfo Execute(WorkflowInfo currentWorkflowInfo)
    {
      using (WorkflowInfoSelectDialog dialog = new WorkflowInfoSelectDialog(currentWorkflowInfo))
      {
        if (dialog.ShowDialog() == DialogResult.OK)
          return dialog.WorkingWorkflowInfo;
        return null;
      }
    }

    #endregion

    #region 属性

    private WorkflowInfo WorkingWorkflowInfo
    {
      get { return BindingSourceHelper.GetDataSourceCurrent(this.workflowInfoListBindingSource) as WorkflowInfo; }
    }

    #endregion

    #region 方法

    /// <summary>
    /// 人性化
    /// </summary>
    private void Humanistic()
    {
      this.okToolStripButton.Enabled = WorkingWorkflowInfo != null;
    }
    
    #endregion

    private void typeNameToolStripTextBox_TextChanged(object sender, System.EventArgs e)
    {
      this.workflowInfoListBindingSource.Filter = String.Format("TypeName Like '%{0}%'", typeNameToolStripTextBox.Text);
    }

    private void workflowInfoListDataGridView_DoubleClick(object sender, System.EventArgs e)
    {
      this.DialogResult = DialogResult.OK;
    }

    private void okToolStripButton_Click(object sender, System.EventArgs e)
    {
      this.DialogResult = DialogResult.OK;
    }

    private void cancelToolStripButton_Click(object sender, System.EventArgs e)
    {
      this.DialogResult = DialogResult.Cancel;
    }
    
    private void workflowInfoListBindingSource_DataSourceChanged(object sender, System.EventArgs e)
    {
      Humanistic();
    }

    private void workflowInfoListBindingSource_PositionChanged(object sender, System.EventArgs e)
    {
      Humanistic();
    }

    private void workflowInfoListDataGridView_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
    {
      WorkflowInfo workflowInfo;
      try
      {
        workflowInfo = this.workflowInfoListDataGridView.Rows[e.RowIndex].DataBoundItem as WorkflowInfo;
      }
      catch (IndexOutOfRangeException)
      {
        return;
      }
      if (workflowInfo != null && !String.IsNullOrEmpty(workflowInfo.DisableUserNumber))
        e.CellStyle.ForeColor = Color.DarkGray;
    }
  }
}
