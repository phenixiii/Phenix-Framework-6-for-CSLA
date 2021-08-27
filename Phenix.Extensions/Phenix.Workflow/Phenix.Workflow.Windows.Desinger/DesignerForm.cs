using System;
using System.Activities.Presentation;
using System.Activities.Presentation.Toolbox;
using System.Windows.Forms;
using Phenix.Core;
using Phenix.Core.Workflow;
using Phenix.Workflow.Windows.Desinger.Helper;

namespace Phenix.Workflow.Windows.Desinger
{
  internal partial class DesignerForm : Phenix.Core.Windows.BaseForm
  {
    public DesignerForm()
    {
      InitializeComponent();
    }

    #region ����

    private WorkflowInfo _workflowInfo;

    private WorkflowDesigner _workflowDesigner;

    private ToolboxControl _toolboxControl;
    
    #endregion

    #region ����

    private void ApplyRules()
    {
      this.saveMenuItem.Enabled = _workflowInfo != null && !_workflowDesigner.IsInErrorState();
      this.saveAsMenuItem.Enabled = this.saveMenuItem.Enabled;
      this.saveToolStripButton.Enabled = this.saveMenuItem.Enabled;

      this.disableMenuItem.Enabled = _workflowInfo != null && !_workflowInfo.DisableTime.HasValue;
      this.disableToolStripButton.Enabled = this.disableMenuItem.Enabled;
    }

    private void ResetWorkflowDesigner(WorkflowInfo workflowInfo)
    {
      this.Text = String.Format("�����������[{0}]", workflowInfo);

      _workflowInfo = workflowInfo;

      _workflowDesigner = new WorkflowDesigner();
      if (workflowInfo != null)
        _workflowDesigner.Load(workflowInfo.SaveToFile());

      this.designerHost.Child = _workflowDesigner.View;
      this.propertyInspectorHost.Child = _workflowDesigner.PropertyInspectorView;
      
      ApplyRules();
    }
    
    private bool CheckWorkflowInfo(string messageBoxCaption)
    {
      if (_workflowInfo != null)
      {
        _workflowDesigner.Flush();
        if (String.CompareOrdinal(_workflowInfo.XamlCode, _workflowDesigner.Text) != 0)
        {
          switch (MessageBox.Show(String.Format("{0}�ѱ�����, �Ƿ񱣴�?", _workflowInfo),
            messageBoxCaption, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question))
          {
            case DialogResult.Yes:
              if (_workflowDesigner.IsInErrorState())
              {
                if (MessageBox.Show("��������������ڴ���״̬���޷����棬�Ƿ�����������?",
                  messageBoxCaption, MessageBoxButtons.YesNo, MessageBoxIcon.Error) == DialogResult.Yes)
                  return false;
              }
              else
              {
                _workflowInfo = SaveWorkflowInfo(_workflowInfo, _workflowDesigner.Text);
                ApplyRules();
              }
              break;
            case DialogResult.Cancel:
              return false;
          }
        }
      }
      return true;
    }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
    private static WorkflowInfo SaveWorkflowInfo(WorkflowInfo workflowInfo, string xamlCode)
    {
      try
      {
        workflowInfo.XamlCode = xamlCode;
        workflowInfo.SaveToFile();
        WorkflowHub.AddWorkflowInfo(workflowInfo);
        WorkflowInfo result = WorkflowHub.GetWorkflowInfo(workflowInfo.TypeNamespace, workflowInfo.TypeName, true);
        return result;
      }
      catch (Exception ex)
      {
        MessageBox.Show(String.Format("����ʧ��:\n{0}", AppUtilities.GetErrorHint(ex)),
          workflowInfo.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
        return workflowInfo;
      }
    }

    private static WorkflowInfo DisableWorkflowInfo(WorkflowInfo workflowInfo)
    {
      WorkflowHub.DisableWorkflowInfo(workflowInfo);
      WorkflowInfo result = WorkflowHub.GetWorkflowInfo(workflowInfo.TypeNamespace, workflowInfo.TypeName, true);
      return result;
    }

    #endregion
    
    private void DesignerForm_Load(object sender, EventArgs e)
    {
      (new System.Activities.Core.Presentation.DesignerMetadata()).Register();

      ResetWorkflowDesigner(WorkflowInfoSelectDialog.Execute(_workflowInfo) ??
        (MessageBox.Show("�Ƿ���Ҫ�½���������",
        "�½�������", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes ? WorkflowInfoNewDialog.Execute() : null));

      _toolboxControl = ToolboxControlHelper.BuildToolboxControl();
      this.toolBoxControlHost.Child = _toolboxControl;
      this.fileSystemWatcher.Path = AppConfig.BaseDirectory;
    }
    
    private void DesignerForm_FormClosing(object sender, FormClosingEventArgs e)
    {
      if (e.CloseReason == CloseReason.UserClosing)
        e.Cancel = !CheckWorkflowInfo("�˳�");
    }

    private void newMenuItem_Click(object sender, EventArgs e)
    {
      if (CheckWorkflowInfo("�½�������"))
        ResetWorkflowDesigner(WorkflowInfoNewDialog.Execute());
    }

    private void openMenuItem_Click(object sender, EventArgs e)
    {
      if (CheckWorkflowInfo("�򿪹�����"))
      {
        WorkflowInfo workflowInfo = WorkflowInfoSelectDialog.Execute(_workflowInfo);
        if (workflowInfo != null)
          ResetWorkflowDesigner(workflowInfo);
      }
    }

    private void saveMenuItem_Click(object sender, EventArgs e)
    {
      _workflowDesigner.Flush();
      if (_workflowDesigner.IsInErrorState())
        MessageBox.Show("��������������ڴ���״̬���޷����棬������������!",
          "���湤����", MessageBoxButtons.OK, MessageBoxIcon.Error);
      else
      {
        _workflowInfo = SaveWorkflowInfo(_workflowInfo, _workflowDesigner.Text);
        ApplyRules();
      }
    }

    private void saveAsMenuItem_Click(object sender, EventArgs e)
    {
      if (CheckWorkflowInfo("��湤����"))
      {
        _workflowInfo = SaveWorkflowInfo(WorkflowInfoNewDialog.Execute(), _workflowDesigner.Text);
        ApplyRules();
      }
    }
    
    private void disableMenuItem_Click(object sender, EventArgs e)
    {
      if (MessageBox.Show("�Ƿ���ñ�������?",
        "���ù�����", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
      {
       _workflowInfo = DisableWorkflowInfo(_workflowInfo);
       ApplyRules();
      }
    }

    private void exitMenuItem_Click(object sender, EventArgs e)
    {
      this.Close();
    }
    
    private void fileSystemWatcher_Created(object sender, System.IO.FileSystemEventArgs e)
    {
      ToolboxControlHelper.AddToolboxItemWrapper(_toolboxControl, e.FullPath);
    }
  }
}