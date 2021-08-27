using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Net;
using System.Text;
using System.Windows.Forms;
using Phenix.Core;
using Phenix.Core.Dictionary;
using Phenix.Core.IO;
using Phenix.Core.Net;
using Phenix.Core.Security;
using Phenix.Services.Client.UpgradeFile;

namespace Phenix.Services.Client.Security
{
  /// <summary>
  /// 登录界面
  /// </summary>
  public partial class LogOnDialog : Phenix.Core.Windows.DialogForm
  {
    /// <summary>
    /// 初始化
    /// </summary>
    protected LogOnDialog()
    {
      InitializeComponent();

      _downLoadFiles = new DownloadFiles();
      _downLoadFiles.DownloadFileInfo += new System.EventHandler<DownloadFileInfoEventArgs>(DownLoadFiles_DownloadFileInfo);
      _downLoadFiles.DownloadFile += new System.EventHandler<DownloadFileEventArgs>(DownLoadFiles_DownloadFile);
    }

    #region 工厂

    /// <summary>
    /// 执行
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
    public static IPrincipal Execute<T>()
      where T : LogOnDialog
    {
      return Execute<T>(null, null, null, null, null);
    }

    /// <summary>
    /// 执行
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
    public static IPrincipal Execute<T>(string title, Image logo,
      ProxyType? upgradeProxyType, string upgradeServicesAddress, ICollection<string> upgradeFileFilters)
      where T : LogOnDialog
    {
      using (T form = (T)Activator.CreateInstance(typeof(T), true))
      {
        if (!String.IsNullOrEmpty(title))
        {
          Properties.Settings.Default.LogOnTitle = title;
          form.Text = title;
        }
        if (logo != null)
          form.Logo = logo;
        form.UpgradeProxyType = upgradeProxyType;
        form.UpgradeServicesAddress = upgradeServicesAddress;
        foreach (string s in upgradeFileFilters)
          form.UpgradeFileFilters.Add(s);
        form.Initialize();
        return form.ShowDialog() == DialogResult.OK ? UserPrincipal.User : null;
      }
    }

    #endregion

    #region 属性

    /// <summary>
    /// 登陆界面的标志
    /// </summary>
    [DefaultValue(null), Browsable(false)]
    public Image Logo
    {
      get { return this.logoPictureBox.Image; }
      set
      {
        if (value == null)
          this.logoPictureBox.Image = null;
        else
          this.logoPictureBox.Image = (Image)(value.Clone());
      }
    }

    private readonly DownloadFiles _downLoadFiles;
    /// <summary>
    /// 升级文件的代理类型
    /// 缺省为 null 代表将默认使用 Phenix.Core.Net.NetConfig.ProxyType
    /// </summary>
    [DefaultValue(null), Browsable(false)]
    public ProxyType? UpgradeProxyType
    {
      get { return _downLoadFiles.UpgradeProxyType; }
      set { _downLoadFiles.UpgradeProxyType = value; }
    }
    /// <summary>
    /// 升级文件的服务地址
    /// 缺省为 null 代表将默认使用 Phenix.Core.Net.NetConfig.ServicesAddress
    /// </summary>
    [DefaultValue(null), Browsable(false)]
    public string UpgradeServicesAddress
    {
      get { return _downLoadFiles.UpgradeServicesAddress; }
      set { _downLoadFiles.UpgradeServicesAddress = value; }
    }
    /// <summary>
    /// 升级文件的过滤器集
    /// </summary>
    [DefaultValue(null), Browsable(false)]
    public ICollection<string> UpgradeFileFilters
    {
      get { return _downLoadFiles.SearchPatterns; }
    }

    private string ServicesAddress
    {
      get { return this.hostComboBox.Text != null ? this.hostComboBox.Text.Trim() : null; }
      set
      {
        _servicesEfficiency = null;
        this.hostComboBox.Text = value;
      }
    }

    private int? _servicesEfficiency;
    private int ServicesEfficiency
    {
      get { return AppSettings.GetProperty(ServicesAddress, ref _servicesEfficiency, 0); }
      set { AppSettings.SetProperty(ServicesAddress, ref _servicesEfficiency, value); }
    }

    private string ServicesAddresses
    {
      get
      {
        StringBuilder result = new StringBuilder();
        foreach (string s in this.hostComboBox.Items)
        {
          result.Append(s);
          result.Append(AppConfig.SEPARATOR);
        }
        if (!this.hostComboBox.Items.Contains(ServicesAddress))
        {
          result.Append(ServicesAddress);
          result.Append(AppConfig.SEPARATOR);
        }
        return result.ToString();
      }
      set
      {
        this.hostComboBox.Items.Clear();
        if (!String.IsNullOrEmpty(value))
        {
          string[] servicesAddresses = value.Split(new string[] { AppConfig.SEPARATOR }, StringSplitOptions.RemoveEmptyEntries);
          int minServicesEfficiency = Int32.MaxValue;
          int minServicesEfficiencyIndex = -1;
          for (int i = 0; i < servicesAddresses.Length; i++)
          {
            int servicesEfficiency;
            if (Int32.TryParse(AppSettings.ReadValue(servicesAddresses[i]), out servicesEfficiency))
              if (minServicesEfficiency > servicesEfficiency)
              {
                minServicesEfficiency = servicesEfficiency;
                minServicesEfficiencyIndex = i;
              }
          }
          if (minServicesEfficiency < Int32.MaxValue)
          {
            ServicesAddress = servicesAddresses[minServicesEfficiencyIndex];
            servicesAddresses[minServicesEfficiencyIndex] = servicesAddresses[0];
            servicesAddresses[0] = ServicesAddress;
          }
          this.hostComboBox.Items.AddRange(servicesAddresses);
        }
      }
    }

    private bool? _userNumberCached;
    /// <summary>
    /// 是否缓存UserNumber
    /// 缺省为 true
    /// </summary>
    [DefaultValue(true), Browsable(false)]
    public bool UserNumberCached
    {
      get { return AppSettings.GetProperty(ref _userNumberCached, true); }
      set { AppSettings.SetProperty(ref _userNumberCached, value); }
    }

    private string UserNumberKey
    {
      get { return String.Format("{0}.UserNumber", this.GetType()); }
    }

    private string UserNumber
    {
      get { return this.userNumberTextBox.Text; }
      set { this.userNumberTextBox.Text = value; }
    }

    private string Password
    {
      get { return this.passwordTextBox.Text; }
    }

    #endregion

    #region 方法

    private void Initialize()
    {
      ServicesAddress = NetConfig.ServicesAddress;
      ServicesAddresses = NetConfig.ServicesAddresses;

      if (UserNumberCached)
        UserNumber = AppSettings.ReadValue(UserNumberKey, false, true);

      ApplyRules();
      Humanistic();
    }

    /// <summary>
    /// 个性化
    /// </summary>
    protected virtual void Humanistic()
    {
      if (String.IsNullOrEmpty(UserNumber))
        this.userNumberTextBox.Focus();
      else if (String.IsNullOrEmpty(Password))
        this.passwordTextBox.Focus();
      else if (String.IsNullOrEmpty(ServicesAddress))
        this.hostComboBox.Focus();
      else
        this.logOnButton.Focus();
    }

    /// <summary>
    /// 应用规则
    /// </summary>
    protected virtual void ApplyRules()
    {
      this.logOnButton.Enabled = !String.IsNullOrEmpty(UserNumber) && !String.IsNullOrEmpty(Password);
    }

    private void InvokeShowHint(MessageNotifyEventArgs e)
    {
      if (this.HintLabel.InvokeRequired)
        this.HintLabel.BeginInvoke(new Action<MessageNotifyEventArgs>(ShowHint), new object[] { e });
      else
        ShowHint(e);
      Application.DoEvents();
    }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Globalization", "CA1303:DoNotPassLiteralsAsLocalizedParameters")]
    private void ShowHint(MessageNotifyEventArgs e)
    {
      this.HintLabel.Text = e.ToString();
    }

    /// <summary>
    /// 登录
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1804:RemoveUnusedLocals")]
    protected void Login()
    {
      if (String.IsNullOrEmpty(ServicesAddress))
        ServicesAddress = IPAddress.Loopback.ToString();
      try
      {
        this.Cursor = Cursors.WaitCursor;
        this.userNumberTextBox.Enabled = false;
        this.passwordTextBox.Enabled = false;
        this.hostComboBox.Enabled = false;
        this.logOnButton.Enabled = false;
        this.changePasswordButton.Enabled = false;
        try
        {
          InvokeShowHint(new MessageNotifyEventArgs(MessageNotifyType.Information,
            Phenix.Services.Client.Properties.Settings.Default.LogOn,
            Phenix.Services.Client.Properties.Resources.Trying));
          DateTime dt = DateTime.Now;
          DataSecurityContext context;
          if (LogOnHelper.TryLogOn(ServicesAddress, UserNumber, Password, out context))
          {
            ServicesEfficiency = DateTime.Now.Subtract(dt).Milliseconds;
            InvokeShowHint(new MessageNotifyEventArgs(MessageNotifyType.Information,
              Phenix.Services.Client.Properties.Resources.UpgradeFile,
              Phenix.Services.Client.Properties.Resources.Trying));

            if (NetConfig.ServicesFixed)
              NetConfig.ServicesAddresses = ServicesAddresses;
            if (UserNumberCached)
              AppSettings.SaveValue(UserNumberKey, UserNumber, false, true);

            if (!String.IsNullOrEmpty(context.Message))
            {
              MessageBox.Show(context.Message, this.Text, MessageBoxButtons.OK, MessageBoxIcon.Warning);
              ChangePasswordDialog.Execute(ServicesAddress);
            }

            do
            {
              try
              {
                this.DialogResult = _downLoadFiles.Execute() ? DialogResult.OK : DialogResult.Abort;
                break;
              }
              catch (Exception ex)
              {
                InvokeShowHint(new MessageNotifyEventArgs(MessageNotifyType.Error,
                  Phenix.Services.Client.Properties.Settings.Default.LogOn, ex));
                if (MessageBox.Show(Phenix.Services.Client.Properties.Resources.UpgradeFileTry + '\n' + Phenix.Core.AppUtilities.GetErrorHint(ex),
                  this.Text, MessageBoxButtons.YesNo, MessageBoxIcon.Error) != DialogResult.Yes)
                {
                  this.DialogResult = DialogResult.Abort;
                  break;
                }
              }
            } while (true);
          }
          else if (context != null && !String.IsNullOrEmpty(context.Message))
          {
            InvokeShowHint(new MessageNotifyEventArgs(MessageNotifyType.Warning, Phenix.Services.Client.Properties.Settings.Default.LogOn, context.Message));
            MessageBox.Show(context.Message, this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
          }
          else
          {
            InvokeShowHint(new MessageNotifyEventArgs(MessageNotifyType.Warning, Phenix.Services.Client.Properties.Settings.Default.LogOn, Phenix.Services.Client.Properties.Resources.LogOnFailed));
            MessageBox.Show(UserNumber + Phenix.Services.Client.Properties.Resources.LogOnFailed, this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
            this.passwordTextBox.Focus();
          }
        }
        finally
        {
          this.userNumberTextBox.Enabled = true;
          this.passwordTextBox.Enabled = true;
          this.hostComboBox.Enabled = true;
          this.logOnButton.Enabled = true;
          this.changePasswordButton.Enabled = true;
          this.Cursor = Cursors.Default;
        }
      }
      catch (UserPasswordComplexityException ex)
      {
        MessageBox.Show(ex.Message, this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
        if (ChangePasswordDialog.Execute(ServicesAddress))
          this.passwordTextBox.Focus();
      }
      catch (Exception ex)
      {
        this.hostComboBox.Visible = true;
        InvokeShowHint(new MessageNotifyEventArgs(MessageNotifyType.Error,
          Phenix.Services.Client.Properties.Settings.Default.LogOn, ex));
        MessageBox.Show(Phenix.Services.Client.Properties.Resources.LogOnFailed + '\n' + Phenix.Core.AppUtilities.GetErrorHint(ex),
          this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
      }
    }

    /// <summary>
    /// 修改口令
    /// </summary>
    protected void ChangePassword()
    {
      if (String.IsNullOrEmpty(ServicesAddress))
        ServicesAddress = IPAddress.Loopback.ToString();
      if (ChangePasswordDialog.Execute(ServicesAddress, UserNumber))
        InvokeShowHint(new MessageNotifyEventArgs(MessageNotifyType.Information,
          Phenix.Services.Client.Properties.Settings.Default.ChangePassword,
          Phenix.Services.Client.Properties.Resources.ModifyPasswordSucceed));
      this.userNumberTextBox.Focus();
    }

    /// <summary>
    /// 退出
    /// </summary>
    protected void Quit()
    {
      if (MessageBox.Show(Phenix.Services.Client.Properties.Resources.QuitConfirm,
        this.Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
      {
        _downLoadFiles.ShutDown = true;
        this.DialogResult = DialogResult.Cancel;
      }
    }

    #endregion

    private void LoginButton_Click(object sender, EventArgs e)
    {
      Login();
    }

    private void ChangePasswordButton_Click(object sender, EventArgs e)
    {
      ChangePassword();
    }

    private void QuitButton_Click(object sender, EventArgs e)
    {
      Quit();
    }

    private void ClearCacheToolStripMenuItem_Click(object sender, EventArgs e)
    {
      DataDictionaryHub.ClearCache();
    }

    private void UserNumberTextBox_TextChanged(object sender, EventArgs e)
    {
      ApplyRules();
    }

    private void passwordTextBox_TextChanged(object sender, EventArgs e)
    {
      ApplyRules();
    }
    private void hostComboBox_KeyDown(object sender, KeyEventArgs e)
    {
      if (e.KeyCode == Keys.Delete && e.Control)
        if (this.hostComboBox.Items.Contains(ServicesAddress))
          this.hostComboBox.Items.Remove(ServicesAddress);
    }

    private void hostComboBox_TextChanged(object sender, EventArgs e)
    {
      if (String.IsNullOrEmpty(ServicesAddress))
        ServicesAddress = NetConfig.EMBEDDED_SERVICE;
      else if (String.CompareOrdinal(ServicesAddress, NetConfig.EMBEDDED_SERVICE) != 0 && ServicesAddress.IndexOf(NetConfig.EMBEDDED_SERVICE, StringComparison.Ordinal) >= 0)
        ServicesAddress = ServicesAddress.Replace(NetConfig.EMBEDDED_SERVICE, String.Empty);
    }

    private void DownLoadFiles_DownloadFileInfo(object sender, DownloadFileInfoEventArgs e)
    {
      DownloadFiles.CheckNeedDownload(e);
      if (!e.Applied)
        InvokeShowHint(new MessageNotifyEventArgs(MessageNotifyType.Information,
          Phenix.Services.Client.Properties.Resources.Download,
          Phenix.Services.Client.Properties.Resources.File +
          e.FileName + Phenix.Services.Client.Properties.Resources.Wait));
    }

    private void DownLoadFiles_DownloadFile(object sender, DownloadFileEventArgs e)
    {
      if (e.Info.ChunkNumber == 1)
        InvokeShowHint(new MessageNotifyEventArgs(MessageNotifyType.Information,
          Phenix.Services.Client.Properties.Resources.UpgradeFile,
          e.Info.FileName + Phenix.Services.Client.Properties.Resources.Wait));
      try
      {
        FileHelper.WriteChunkInfo(e.FileStream, e.Info);
      }
      catch (Exception ex)
      {
        InvokeShowHint(new MessageNotifyEventArgs(MessageNotifyType.Error,
          Phenix.Services.Client.Properties.Resources.UpgradeFile, e.Info.FileName, ex));
        MessageBox.Show(e.Info.FileName + Phenix.Services.Client.Properties.Resources.UpgradeFileFailed + '\n' + Phenix.Core.AppUtilities.GetErrorHint(ex),
          this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
        throw;
      }
      if (e.Succeed && e.Info.ChunkNumber == e.Info.ChunkCount)
      {
        InvokeShowHint(new MessageNotifyEventArgs(MessageNotifyType.Information,
          Phenix.Services.Client.Properties.Resources.UpgradeFile,
          e.Info.FileName + Phenix.Services.Client.Properties.Resources.UpgradeFileSucceed));
        if (String.Compare(e.Info.FileName, AppDomain.CurrentDomain.FriendlyName, StringComparison.OrdinalIgnoreCase) == 0)
        {
          MessageBox.Show(Phenix.Services.Client.Properties.Resources.UpgradeProgramFile,
            this.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
          e.Stop = true;
        }
      }
    }
  }
}