using System;
using System.Windows.Forms;
using Phenix.Core.Reflection;
using Phenix.Core.Windows;

namespace Phenix.Core.Workflow.Windows
{
  /// <summary>
  /// 选择工作流类型对话框
  /// </summary>
  public partial class WorkflowTypeSelectDialog : DialogForm
  {
    private WorkflowTypeSelectDialog(string selectedWorkflowTypeName)
    {
      InitializeComponent();

      this.Text = Resources.WorkflowTypeSelect;
      this.okButton.Text = Phenix.Core.Properties.Resources.Ok;
      this.cancelButton.Text = Phenix.Core.Properties.Resources.Cancel;

      WorkflowIdentityInfo selectedWorkflowIdentityInfo = null;
      foreach (Type item in Utilities.LoadExportedSubclassTypesFromBaseDirectory(false, typeof(IStartCommand)))
      {
        WorkflowIdentityInfo workflowIdentityInfo = WorkflowHub.GetWorkflowIdentityInfo(item, false);
        if (workflowIdentityInfo != null && WorkflowHub.GetWorkflowInfo(workflowIdentityInfo.TypeNamespace, workflowIdentityInfo.TypeName, false) == null)
        {
          this.workflowIdentityInfoListBox.Items.Add(workflowIdentityInfo);
          if (selectedWorkflowIdentityInfo == null && String.CompareOrdinal(workflowIdentityInfo.FullTypeName, selectedWorkflowTypeName) == 0)
            selectedWorkflowIdentityInfo = workflowIdentityInfo;
        }
        this.workflowIdentityInfoListBox.SelectedItem = selectedWorkflowIdentityInfo;
      }
    }

    #region 工厂

    /// <summary>
    /// 执行
    /// </summary>
    public static WorkflowIdentityInfo Execute()
    {
      return Execute(null);
    }

    /// <summary>
    /// 执行
    /// </summary>
    public static WorkflowIdentityInfo Execute(string selectedWorkflowTypeName)
    {
      using (WorkflowTypeSelectDialog dialog = new WorkflowTypeSelectDialog(selectedWorkflowTypeName))
      {
        if (dialog.ShowDialog() == DialogResult.OK)
          return dialog.SelectedWorkflowIdentityInfo;
      }
      return null;
    }

    #endregion

    #region 属性
    
    /// <summary>
    /// 被选择的工作流标识信息
    /// </summary>
    public WorkflowIdentityInfo SelectedWorkflowIdentityInfo
    {
      get
      {
        return this.workflowIdentityInfoListBox.SelectedIndex >= 0
          ? (WorkflowIdentityInfo)this.workflowIdentityInfoListBox.SelectedItem
          : null;
      }
    }

    #endregion

    private void workflowIdentityInfoListBox_DoubleClick(object sender, EventArgs e)
    {
      this.DialogResult = DialogResult.OK;
    }
  }
}
