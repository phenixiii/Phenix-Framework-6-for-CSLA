using System;
using System.Windows.Forms;
using Phenix.Core.Net;

namespace Phenix.Services.Host.Core
{
  #region 定义

  /// <summary>
  /// 申明校验系统信息处理函数
  /// </summary>
  /// <param name="enterpriseName">企业名</param>
  internal delegate bool VerifySystemInfoCallback(string enterpriseName);

  #endregion

  /// <summary>
  /// 设置系统信息
  /// </summary>
  internal partial class SystemInfoSetupDialog : Phenix.Core.Windows.DialogForm
  {
    private SystemInfoSetupDialog(string enterpriseName, VerifySystemInfoCallback verifyCallback)
    {
      InitializeComponent();

      this.enterpriseNameTextBox.Text = enterpriseName;
      _verifyCallback = verifyCallback;

      this.clearLogDeferMonthsNumericUpDown.Value = ClearConfigurationLibrary.ClearLogDeferMonths;

      this.fetchMaxCountWarnThresholdNumericUpDown.Value = PerformanceAnalyse.FetchMaxCountWarnThreshold;
      this.fetchMaxElapsedTimeWarnThresholdNumericUpDown.Value = PerformanceAnalyse.FetchMaxElapsedTimeWarnThreshold;
      this.saveMaxElapsedTimeWarnThresholdNumericUpDown.Value = PerformanceAnalyse.SaveMaxElapsedTimeWarnThreshold;

      this.clientLibrarySubdirectoryTextBox.Text = Phenix.Core.AppConfig.ClientLibrarySubdirectory;
      this.serviceLibrarySubdirectoryTextBox.Text = Phenix.Core.AppConfig.ServiceLibrarySubdirectory;

      this.remotingHttpNumericUpDown1.Tag = (decimal)RemotingConfig.HttpPort;
      this.remotingHttpNumericUpDown1.Value = RemotingConfig.HttpPort;
      this.remotingTcpNumericUpDown1.Tag = (decimal)RemotingConfig.TcpPort;
      this.remotingTcpNumericUpDown1.Value = RemotingConfig.TcpPort;
      this.wcfHttpNumericUpDown1.Tag = (decimal)WcfConfig.BasicHttpPort;
      this.wcfHttpNumericUpDown1.Value = WcfConfig.BasicHttpPort;
      this.wcfTcpNumericUpDown1.Tag = (decimal)WcfConfig.NetTcpPort;
      this.wcfTcpNumericUpDown1.Value = WcfConfig.NetTcpPort;

      this.ipSegmentCountNumericUpDown.Value = Phenix.Services.Library.AppConfig.IpSegmentCount;

#if Top
      this.webApiPortNumericUpDown.Value = Phenix.Core.Web.WebConfig.WebApiPort;
      this.webSocketPortNumericUpDown.Value = Phenix.Core.Web.WebConfig.WebSocketPort;
#endif

      this.allowUserMultipleAddressLoginCheckBox.Checked = Phenix.Services.Library.AppConfig.AllowUserMultipleAddressLogin;
      this.loginFailureCountMaximumNumericUpDown.Value = Phenix.Services.Library.AppConfig.LoginFailureCountMaximum;
      this.sessionExpiresMinutesNumericUpDown.Value = Phenix.Services.Library.AppConfig.SessionExpiresMinutes;
      this.remindPasswordComplexityCheckBox.Checked = Phenix.Services.Library.AppConfig.RemindPasswordComplexity;
      this.forcedPasswordComplexityCheckBox.Checked = Phenix.Services.Library.AppConfig.ForcedPasswordComplexity;
      this.passwordLengthMinimizeNumericUpDown.Value = Phenix.Services.Library.AppConfig.PasswordLengthMinimize;
      this.passwordComplexityMinimizeNumericUpDown.Value = Phenix.Services.Library.AppConfig.PasswordComplexityMinimize;
      this.passwordExpirationRemindDaysNumericUpDown.Value = Phenix.Services.Library.AppConfig.PasswordExpirationRemindDays;
      this.passwordExpirationDaysNumericUpDown.Value = Phenix.Services.Library.AppConfig.PasswordExpirationDays;
      this.emptyRolesIsDenyCheckBox.Checked = Phenix.Services.Library.AppConfig.EmptyRolesIsDeny;
      this.easyAuthorizationCheckBox.Checked = Phenix.Services.Library.AppConfig.EasyAuthorization;
      this.needMarkLoginCheckBox.Checked = Phenix.Services.Library.AppConfig.NeedMarkLogin;
      this.noLoginCheckBox.Checked = Phenix.Services.Library.AppConfig.NoLogin;
      this.noLoginReasonTextBox.Text = Phenix.Services.Library.AppConfig.NoLoginReason;
    }

    #region 工厂

    /// <summary>
    /// 执行
    /// </summary>
    /// <param name="enterpriseName">企业名</param>
    /// <param name="verifyCallback">校验函数</param>
    public static bool Execute(string enterpriseName, VerifySystemInfoCallback verifyCallback)
    {
      using (SystemInfoSetupDialog dialog = new SystemInfoSetupDialog(enterpriseName, verifyCallback))
      {
        return (dialog.ShowDialog() == DialogResult.OK);
      }
    }

    #endregion

    #region 属性

    private readonly VerifySystemInfoCallback _verifyCallback;

    #endregion

    #region 方法

    private void ApplyRules()
    {
      if (this.noLoginCheckBox.Checked)
      {
        this.allowUserMultipleAddressLoginCheckBox.Enabled = false;
        this.loginFailureCountMaximumNumericUpDown.Enabled = false;
        this.sessionExpiresMinutesNumericUpDown.Enabled = false;
        this.remindPasswordComplexityCheckBox.Enabled = false;
        this.forcedPasswordComplexityCheckBox.Enabled = false;
        this.passwordLengthMinimizeNumericUpDown.Enabled = false;
        this.passwordComplexityMinimizeNumericUpDown.Enabled = false;
        this.passwordExpirationRemindDaysNumericUpDown.Enabled = false;
        this.emptyRolesIsDenyCheckBox.Enabled = false;
        this.easyAuthorizationCheckBox.Enabled = false;
        this.needMarkLoginCheckBox.Enabled = false;
        this.noLoginReasonTextBox.Enabled = true;
      }
      else
      {
        this.allowUserMultipleAddressLoginCheckBox.Enabled = true;
        this.loginFailureCountMaximumNumericUpDown.Enabled = true;
        this.sessionExpiresMinutesNumericUpDown.Enabled = true;
        this.remindPasswordComplexityCheckBox.Enabled = true;
        this.forcedPasswordComplexityCheckBox.Enabled = true;
        this.passwordLengthMinimizeNumericUpDown.Enabled = this.remindPasswordComplexityCheckBox.Checked || this.forcedPasswordComplexityCheckBox.Checked;
        this.passwordComplexityMinimizeNumericUpDown.Enabled = this.remindPasswordComplexityCheckBox.Checked || this.forcedPasswordComplexityCheckBox.Checked;
        this.passwordExpirationRemindDaysNumericUpDown.Enabled = true;
        this.emptyRolesIsDenyCheckBox.Enabled = true;
        this.easyAuthorizationCheckBox.Enabled = true;
        this.needMarkLoginCheckBox.Enabled = true;
        this.noLoginReasonTextBox.Enabled = false;
      }
    }

    #endregion

    private void SystemInfoDialog_Shown(object sender, EventArgs e)
    {
      ApplyRules();

#if Top
      this.ajaxGroupBox.Enabled = true;
      this.webApiCheckBox.Enabled = true;
      this.webApiPortNumericUpDown.Enabled = true;
      this.webSocketCheckBox.Enabled = true;
      this.webSocketPortNumericUpDown.Enabled = true;
      this.webEnableCorsOriginsTextBox.Enabled = true;
      this.webMaxConcurrentRequestsNumericUpDown.Enabled =true;
#else
      this.ajaxGroupBox.Enabled = false;
      this.webApiCheckBox.Enabled = false;
      this.webApiPortNumericUpDown.Enabled = false;
      this.webSocketCheckBox.Enabled = false;
      this.webSocketPortNumericUpDown.Enabled = false;
      this.webEnableCorsOriginsTextBox.Enabled = false;
      this.webMaxConcurrentRequestsNumericUpDown.Enabled = false;
#endif
    }

    private void enterpriseNameTextBox_KeyPress(object sender, KeyPressEventArgs e)
    {
      ((Control)sender).Tag = 1;
    }

    private void clientLibrarySubdirectoryButton_Click(object sender, EventArgs e)
    {
      using (FolderBrowserDialog dialog = new FolderBrowserDialog())
      {
        dialog.SelectedPath = this.clientLibrarySubdirectoryTextBox.Text;
        if (dialog.ShowDialog() == DialogResult.OK)
          this.clientLibrarySubdirectoryTextBox.Text = dialog.SelectedPath + "\\";
      }
    }

    private void serviceLibrarySubdirectoryButton_Click(object sender, EventArgs e)
    {
      using (FolderBrowserDialog dialog = new FolderBrowserDialog())
      {
        dialog.SelectedPath = this.serviceLibrarySubdirectoryTextBox.Text;
        if (dialog.ShowDialog() == DialogResult.OK)
          this.serviceLibrarySubdirectoryTextBox.Text = dialog.SelectedPath + "\\";
      }
    }

    private void remotingHttpNumericUpDown1_ValueChanged(object sender, EventArgs e)
    {
      this.remotingHttpNumericUpDown2.Value = this.remotingHttpNumericUpDown1.Value + 1;
      if (this.remotingHttpNumericUpDown1.Value == this.remotingTcpNumericUpDown1.Value || this.remotingHttpNumericUpDown1.Value == this.remotingTcpNumericUpDown2.Value ||
        this.remotingHttpNumericUpDown2.Value == this.remotingTcpNumericUpDown1.Value || this.remotingHttpNumericUpDown2.Value == this.remotingTcpNumericUpDown2.Value)
        this.remotingHttpNumericUpDown1.Value = (decimal)this.remotingHttpNumericUpDown1.Tag;
      else
        this.remotingHttpNumericUpDown1.Tag = this.remotingHttpNumericUpDown1.Value;
    }

    private void remotingTcpNumericUpDown1_ValueChanged(object sender, EventArgs e)
    {
      this.remotingTcpNumericUpDown2.Value = this.remotingTcpNumericUpDown1.Value + 1;
      if (this.remotingHttpNumericUpDown1.Value == this.remotingTcpNumericUpDown1.Value || this.remotingHttpNumericUpDown1.Value == this.remotingTcpNumericUpDown2.Value ||
        this.remotingHttpNumericUpDown2.Value == this.remotingTcpNumericUpDown1.Value || this.remotingHttpNumericUpDown2.Value == this.remotingTcpNumericUpDown2.Value)
        this.remotingTcpNumericUpDown1.Value = (decimal)this.remotingTcpNumericUpDown1.Tag;
      else
        this.remotingTcpNumericUpDown1.Tag = this.remotingTcpNumericUpDown1.Value;
    }
    
    private void wcfHttpNumericUpDown1_ValueChanged(object sender, EventArgs e)
    {
      this.wcfHttpNumericUpDown2.Value = this.wcfHttpNumericUpDown1.Value + 1;
      if (this.wcfHttpNumericUpDown1.Value == this.wcfTcpNumericUpDown1.Value || this.wcfHttpNumericUpDown1.Value == this.wcfTcpNumericUpDown2.Value ||
        this.wcfHttpNumericUpDown2.Value == this.wcfTcpNumericUpDown1.Value || this.wcfHttpNumericUpDown2.Value == this.wcfTcpNumericUpDown2.Value)
        this.wcfHttpNumericUpDown1.Value = (decimal)this.wcfHttpNumericUpDown1.Tag;
      else
        this.wcfHttpNumericUpDown1.Tag = this.wcfHttpNumericUpDown1.Value;
    }

    private void wcfTcpNumericUpDown1_ValueChanged(object sender, EventArgs e)
    {
      this.wcfTcpNumericUpDown2.Value = this.wcfTcpNumericUpDown1.Value + 1;
      if (this.wcfTcpNumericUpDown1.Value == this.wcfHttpNumericUpDown1.Value || this.wcfTcpNumericUpDown1.Value == this.wcfHttpNumericUpDown2.Value ||
        this.wcfTcpNumericUpDown2.Value == this.wcfHttpNumericUpDown1.Value || this.wcfTcpNumericUpDown2.Value == this.wcfHttpNumericUpDown2.Value)
        this.wcfTcpNumericUpDown1.Value = (decimal)this.wcfTcpNumericUpDown1.Tag;
      else
        this.wcfTcpNumericUpDown1.Tag = this.wcfTcpNumericUpDown1.Value;
    }
    
    private void remindPasswordComplexityCheckBox_CheckedChanged(object sender, EventArgs e)
    {
      if (!this.remindPasswordComplexityCheckBox.Checked && this.forcedPasswordComplexityCheckBox.Checked)
        this.forcedPasswordComplexityCheckBox.Checked = false;
      else
        ApplyRules();
    }

    private void forcedPasswordComplexityCheckBox_CheckedChanged(object sender, EventArgs e)
    {
      if (this.forcedPasswordComplexityCheckBox.Checked && !this.remindPasswordComplexityCheckBox.Checked)
        this.remindPasswordComplexityCheckBox.Checked = true;
      else
        ApplyRules();
    }

    private void noLoginCheckBox_CheckedChanged(object sender, EventArgs e)
    {
      ApplyRules();
    }

    private void OK_Click(object sender, EventArgs e)
    {
      Phenix.Services.Host.Properties.Settings.Default.Save();

      if (RemotingConfig.HttpPort != (int)this.remotingHttpNumericUpDown1.Value ||
        RemotingConfig.TcpPort != (int)this.remotingTcpNumericUpDown1.Value)
      {
        RemotingConfig.HttpPort = (int)this.remotingHttpNumericUpDown1.Value;
        RemotingConfig.TcpPort = (int)this.remotingTcpNumericUpDown1.Value;
        MessageBox.Show("System has kept new Remoting service ports, shall become effective after the restart.", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
      }
      if (WcfConfig.BasicHttpPort != (int)this.wcfHttpNumericUpDown1.Value ||
        WcfConfig.NetTcpPort != (int)this.wcfTcpNumericUpDown1.Value)
      {
        WcfConfig.BasicHttpPort = (int)this.wcfHttpNumericUpDown1.Value;
        WcfConfig.NetTcpPort = (int)this.wcfTcpNumericUpDown1.Value;
        MessageBox.Show("System has kept new WCF service ports, shall become effective after the restart.", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
      }

      Phenix.Services.Library.AppConfig.IpSegmentCount = (int)this.ipSegmentCountNumericUpDown.Value;

#if Top
      if (Phenix.Core.Web.WebConfig.WebApiPort != (int)this.webApiPortNumericUpDown.Value ||
        Phenix.Core.Web.WebConfig.WebSocketPort != (int)this.webSocketPortNumericUpDown.Value)
      {
        Phenix.Core.Web.WebConfig.WebApiPort = (int)this.webApiPortNumericUpDown.Value;
        Phenix.Core.Web.WebConfig.WebSocketPort = (int)this.webSocketPortNumericUpDown.Value;
        MessageBox.Show("System has kept new AJAX service ports, shall become effective after the restart.", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
      }
#endif

      bool succeed = true;
      if (this.enterpriseNameTextBox.Tag != null)
        if (_verifyCallback != null && _verifyCallback(this.enterpriseNameTextBox.Text))
          MessageBox.Show("Setup Succeed.", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
        else
        {
          MessageBox.Show("Setup Failed.", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
          succeed = false;
        }

      ClearConfigurationLibrary.ClearLogDeferMonths = (int)this.clearLogDeferMonthsNumericUpDown.Value;

      PerformanceAnalyse.FetchMaxCountWarnThreshold = (int)this.fetchMaxCountWarnThresholdNumericUpDown.Value;
      PerformanceAnalyse.FetchMaxElapsedTimeWarnThreshold = (int)this.fetchMaxElapsedTimeWarnThresholdNumericUpDown.Value;
      PerformanceAnalyse.SaveMaxElapsedTimeWarnThreshold = (int)this.saveMaxElapsedTimeWarnThresholdNumericUpDown.Value;

      Phenix.Core.AppConfig.ClientLibrarySubdirectory = this.clientLibrarySubdirectoryTextBox.Text;
      Phenix.Core.AppConfig.ServiceLibrarySubdirectory = this.serviceLibrarySubdirectoryTextBox.Text;

      Phenix.Services.Library.AppConfig.AllowUserMultipleAddressLogin = this.allowUserMultipleAddressLoginCheckBox.Checked;
      Phenix.Services.Library.AppConfig.LoginFailureCountMaximum = (int)this.loginFailureCountMaximumNumericUpDown.Value;
      Phenix.Services.Library.AppConfig.SessionExpiresMinutes = (int)this.sessionExpiresMinutesNumericUpDown.Value;
      Phenix.Services.Library.AppConfig.RemindPasswordComplexity = this.remindPasswordComplexityCheckBox.Checked;
      Phenix.Services.Library.AppConfig.ForcedPasswordComplexity = this.forcedPasswordComplexityCheckBox.Checked;
      Phenix.Services.Library.AppConfig.PasswordLengthMinimize = (int)this.passwordLengthMinimizeNumericUpDown.Value;
      Phenix.Services.Library.AppConfig.PasswordComplexityMinimize = (int)this.passwordComplexityMinimizeNumericUpDown.Value;
      Phenix.Services.Library.AppConfig.PasswordExpirationRemindDays = (int)this.passwordExpirationRemindDaysNumericUpDown.Value;
      Phenix.Services.Library.AppConfig.PasswordExpirationDays = (int)this.passwordExpirationDaysNumericUpDown.Value;
      Phenix.Services.Library.AppConfig.EmptyRolesIsDeny = this.emptyRolesIsDenyCheckBox.Checked;
      Phenix.Services.Library.AppConfig.EasyAuthorization = this.easyAuthorizationCheckBox.Checked;
      Phenix.Services.Library.AppConfig.NeedMarkLogin = this.needMarkLoginCheckBox.Checked;
      Phenix.Services.Library.AppConfig.NoLogin = this.noLoginCheckBox.Checked;
      Phenix.Services.Library.AppConfig.NoLoginReason = this.noLoginReasonTextBox.Text;

      if (succeed)
        this.DialogResult = DialogResult.OK;
    }
  }
}