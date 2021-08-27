using System;
using System.Windows.Forms;

namespace Phenix.Services.Host.Core
{
  /// <summary>
  /// 暂停服务
  /// </summary>
  internal partial class SuspendServiceDialog : Phenix.Core.Windows.DialogForm
  {
    private SuspendServiceDialog()
    {
      InitializeComponent();
    }

    #region 工厂

    /// <summary>
    /// 执行
    /// </summary>
    public static bool Execute()
    {
      using (SuspendServiceDialog dialog = new SuspendServiceDialog())
      {
        if (!String.IsNullOrEmpty(ServiceManager.SuspendReason))
          dialog.Reason = ServiceManager.SuspendReason;
        if (dialog.ShowDialog() == DialogResult.OK)
        {
          ServiceManager.MarkSuspending(dialog.Reason);
          return true;
        }
        return false;
      }
    }

    #endregion

    #region 属性

    private string Reason
    {
      get { return this.suspendReasonRichTextBox.Text; }
      set { this.suspendReasonRichTextBox.Text = value; }
    }

    #endregion
  }
}
