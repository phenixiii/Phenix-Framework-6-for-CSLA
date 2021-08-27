using System;
using System.Windows.Forms;
using Phenix.Core.Net;
using Phenix.Core.Security;

namespace Phenix.Windows.Security
{
  /// <summary>
  /// 修改登录口令窗体
  /// </summary>
  public partial class ChangePasswordDialog : Phenix.Core.Windows.DialogForm
  {
    /// <summary>
    /// 初始化
    /// </summary>
    protected ChangePasswordDialog()
    {
      InitializeComponent();
    }

    #region 工厂

    /// <summary>
    /// 修改登录口令窗体
    /// </summary>
    public static bool Execute()
    {
      using (ChangePasswordDialog dialog = new ChangePasswordDialog())
      {
        dialog.ServicesAddress = NetConfig.ServicesAddress;
        if (UserIdentity.CurrentIdentity != null && UserIdentity.CurrentIdentity.IsAuthenticated)
        {
          dialog.UserNumber = UserIdentity.CurrentIdentity.UserNumber;
          dialog.userNumberTextBox.Enabled = false;
        }
        return dialog.ShowDialog() == DialogResult.OK;
      }
    }

    internal static bool Execute(string servicesAddress)
    {
      using (ChangePasswordDialog dialog = new ChangePasswordDialog())
      {
        dialog.ServicesAddress = servicesAddress;
        if (UserIdentity.CurrentIdentity != null)
        {
          dialog.UserNumber = UserIdentity.CurrentIdentity.UserNumber;
          if (UserIdentity.CurrentIdentity.IsAuthenticated)
          {
            dialog.OldPassword = UserIdentity.CurrentIdentity.Password;
            dialog.userNumberTextBox.Enabled = false;
            dialog.oldPasswordTextBox.Enabled = false;
          }
        }
        return dialog.ShowDialog() == DialogResult.OK;
      }
    }

    internal static bool Execute(string servicesAddress, string userNumber)
    {
      using (ChangePasswordDialog dialog = new ChangePasswordDialog())
      {
        dialog.ServicesAddress = servicesAddress;
        dialog.UserNumber = userNumber;
        return dialog.ShowDialog() == DialogResult.OK;
      }
    }

    #endregion

    #region 属性

    private string ServicesAddress { get; set; }

    private string Caption
    {
      get { return this.Text; }
    }

    private string UserNumber
    {
      get { return this.userNumberTextBox.Text; }
      set { this.userNumberTextBox.Text = value; }
    }

    private string OldPassword
    {
      get { return this.oldPasswordTextBox.Text; }
      set { this.oldPasswordTextBox.Text = value; }
    }

    private string NewPassword1
    {
      get { return this.newPassword1TextBox.Text; }
    }

    private string NewPassword2
    {
      get { return this.newPassword2TextBox.Text; }
    }

    #endregion

    #region 方法

    private void Humanistic()
    {
      if (String.IsNullOrEmpty(UserNumber))
        this.userNumberTextBox.Focus();
      else if (String.IsNullOrEmpty(OldPassword))
        this.oldPasswordTextBox.Focus();
      else if (String.IsNullOrEmpty(NewPassword1))
        this.newPassword1TextBox.Focus();
      else if (String.IsNullOrEmpty(NewPassword2))
        this.newPassword2TextBox.Focus();
      else
        this.okButton.Focus();
    }

    private void ApplyRules()
    {
      this.okButton.Enabled =
        (!String.IsNullOrEmpty(UserNumber)) &&
        (!String.IsNullOrEmpty(OldPassword)) &&
        (!String.IsNullOrEmpty(NewPassword1)) &&
        (!String.IsNullOrEmpty(NewPassword2));
    }

    #endregion

    #region 事件


    private void ChangePasswordForm_Shown(object sender, EventArgs e)
    {
      ApplyRules();
      Humanistic();
    }

    private void UserNumberTextBox_TextChanged(object sender, EventArgs e)
    {
      ApplyRules();
    }

    private void OldPasswordTextBox_TextChanged(object sender, EventArgs e)
    {
      ApplyRules();
    }

    private void NewPassword1TextBox_TextChanged(object sender, EventArgs e)
    {
      ApplyRules();
    }

    private void NewPassword2TextBox_TextChanged(object sender, EventArgs e)
    {
      ApplyRules();
    }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
    private void OK_Click(object sender, EventArgs e)
    {
      if (NewPassword1 != NewPassword2)
      {
        MessageBox.Show(Phenix.Services.Client.Properties.Resources.InputPasswordPlease,
          Caption, MessageBoxButtons.OK, MessageBoxIcon.Warning);
        this.newPassword1TextBox.Focus();
        return;
      }
      this.Cursor = Cursors.WaitCursor;
      try
      {
        if (LogOnHelper.ChangePassword(ServicesAddress, NewPassword1,
          UserIdentity.CurrentIdentity != null && UserIdentity.CurrentIdentity.IsAuthenticated ? UserIdentity.CurrentIdentity : new UserIdentity(UserNumber, OldPassword)))
        {
          MessageBox.Show(Phenix.Services.Client.Properties.Settings.Default.UserNumber + UserNumber +
            Phenix.Services.Client.Properties.Resources.ModifyPasswordSucceed,
            Caption, MessageBoxButtons.OK, MessageBoxIcon.Information);
          this.DialogResult = DialogResult.OK;
        }
        else
        {
          MessageBox.Show(Phenix.Services.Client.Properties.Settings.Default.UserNumber + UserNumber +
            Phenix.Services.Client.Properties.Resources.ModifyPasswordFailed,
            Caption, MessageBoxButtons.OK, MessageBoxIcon.Error);
          this.oldPasswordTextBox.Focus();
        }
      }
      catch (Exception ex)
      {
        MessageBox.Show(Phenix.Services.Client.Properties.Settings.Default.UserNumber + UserNumber +
          Phenix.Services.Client.Properties.Resources.ModifyPasswordFailed + '\n' + Phenix.Core.AppUtilities.GetErrorHint(ex),
          this.Caption, MessageBoxButtons.OK, MessageBoxIcon.Error);
      }
      finally
      {
        this.Cursor = Cursors.Default;
      }
    }

    #endregion
  }
}