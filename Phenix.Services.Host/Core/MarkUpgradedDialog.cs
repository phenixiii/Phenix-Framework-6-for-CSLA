using System;
using System.Windows.Forms;

namespace Phenix.Services.Host.Core
{
  /// <summary>
  /// 标记已升级
  /// </summary>
  internal partial class MarkUpgradedDialog : Phenix.Core.Windows.DialogForm
  {
    private MarkUpgradedDialog()
    {
      InitializeComponent();
    }

    #region 工厂

    /// <summary>
    /// 执行
    /// </summary>
    public static bool Execute()
    {
      using (MarkUpgradedDialog dialog = new MarkUpgradedDialog())
      {
        if (!String.IsNullOrEmpty(ServiceManager.UpgradeReason))
          dialog.Reason = ServiceManager.UpgradeReason;
        if (dialog.ShowDialog() == DialogResult.OK)
        {
          ServiceManager.MarkUpgraded(dialog.Reason);
          return true;
        }
        return false;
      }
    }

    #endregion

    #region 属性

    private string Reason
    {
      get { return this.upgradeReasonRichTextBox.Text; }
      set { this.upgradeReasonRichTextBox.Text = value; }
    }

    #endregion
  }
}
