using System;
using System.IO;
using System.Windows.Forms;
using Phenix.Core;
using Phenix.Core.Security;
using Phenix.Core.Windows;
using Phenix.Core.Workflow;
using Phenix.Core.Workflow.Windows;

namespace Phenix.Workflow.Windows.Desinger
{
  internal partial class WorkflowInfoNewDialog : DialogForm
  {
    private WorkflowInfoNewDialog(WorkflowIdentityInfo workflowIdentityInfo)
    {
      InitializeComponent();

      this.typeNamespaceTextBox.Text = workflowIdentityInfo.TypeNamespace;
      this.typeNameTextBox.Text = workflowIdentityInfo.TypeName;
      this.captionTextBox.Text = workflowIdentityInfo.Caption;
    }

    #region 工厂

    /// <summary>
    /// 执行
    /// </summary>
    public static WorkflowInfo Execute()
    {
      WorkflowIdentityInfo workflowIdentityInfo = WorkflowTypeSelectDialog.Execute();
      if (workflowIdentityInfo != null)
        using (WorkflowInfoNewDialog dialog = new WorkflowInfoNewDialog(workflowIdentityInfo))
        {
          if (dialog.ShowDialog() == DialogResult.OK)
            return WorkflowInfo.New(dialog.TypeNamespace, dialog.TypeName, dialog.Caption, dialog.XamlCode, UserIdentity.CurrentIdentity);
        }
      return null;
    }

    #endregion

    #region 属性

    private string TypeNamespace
    {
      get { return this.typeNamespaceTextBox.Text; }
    }

    private string TypeName
    {
      get { return this.typeNameTextBox.Text; }
    }

    private string Caption
    {
      get { return this.captionTextBox.Text;}
    }

    private string _xamlCode = Phenix.Workflow.Windows.Desinger.Properties.Resources.WorkflowTemplate;
    private string XamlCode
    {
      get { return _xamlCode.Replace(PLACE_HOLDER, String.Format(PLACE_HOLDER, TypeNamespace, TypeName)); }
    }

    private const string PLACE_HOLDER = @"""{0}.{1}""";
    #endregion

    #region 方法

    private void ApplyRules()
    {
      this.okButton.Enabled =
        !String.IsNullOrEmpty(TypeNamespace) &&
        !String.IsNullOrEmpty(TypeName) &&
        WorkflowHub.GetWorkflowInfo(TypeNamespace, TypeName, false) == null;
    }

    private void Import(string fileName)
    {
      string xamlCode = File.ReadAllText(fileName);
      int i =  xamlCode.IndexOf(@" x:Class=""", StringComparison.Ordinal);
      if (i > 0)
      {
        string s = xamlCode.Substring(i);
        s = s.Substring(s.IndexOf('"') + 1);
        s = s.Remove(s.IndexOf('"'));
        i = s.IndexOf('.');
        if (i > 0)
        {
          _xamlCode = xamlCode.Replace('"' + s + '"', PLACE_HOLDER);
          ApplyRules();
        }
      }
    }

    #endregion

    private void NewWorkflowDialog_Shown(object sender, EventArgs e)
    {
      ApplyRules();
    }

    private void typeNamespaceTextBox_TextChanged(object sender, EventArgs e)
    {
      ApplyRules();
    }

    private void typeNameTextBox_TextChanged(object sender, EventArgs e)
    {
      ApplyRules();
    }

    private void captionTextBox_TextChanged(object sender, EventArgs e)
    {
      ApplyRules();
    }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
    private void importButton_Click(object sender, EventArgs e)
    {
      using (OpenFileDialog openFileDialog = new OpenFileDialog())
      {
        openFileDialog.InitialDirectory = AppConfig.TempDirectory;
        openFileDialog.RestoreDirectory = true;
        openFileDialog.DefaultExt = "xaml";
        openFileDialog.Filter = "xaml files (*.xaml) | *.xaml;*.xamlx";
        openFileDialog.Title = "导入工作流";
        if (openFileDialog.ShowDialog() == DialogResult.OK)
          try
          {
            Import(openFileDialog.FileName);
          }
          catch (Exception ex)
          {
            MessageBox.Show(String.Format("导入失败:\n{0}", AppUtilities.GetErrorHint(ex)),
              openFileDialog.Title, MessageBoxButtons.OK, MessageBoxIcon.Error);
          }
      }
    }
  }
}
