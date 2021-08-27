using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Net.NetworkInformation;
using System.Reflection;
using System.Windows.Forms;
using Phenix.Core;
using Phenix.Core.Data;
using Phenix.Core.Dictionary;
using Phenix.Core.Net;
using Phenix.Core.Plugin;
using Phenix.Core.Workflow;
using Phenix.Services.Host.Core;

namespace Phenix.Services.Host
{
  /// <summary>
  /// 主界面
  /// </summary>
  internal partial class MainForm : Phenix.Core.Windows.BaseForm
  {
    public MainForm()
    {
      InitializeComponent();
    }

    #region 属性

    /// <summary>
    /// 关闭倒计时
    /// </summary>
    private volatile int _closeCountdown = 10;

    internal const string PLUGINS_KEY = "Plugins";
    private readonly object _pluginListViewLock = new object();

    private PerformanceCounter _workingSetCounter;
    private PerformanceCounter WorkingSetCounter
    {
      get
      {
        if (_workingSetCounter == null)
          _workingSetCounter = new PerformanceCounter("Process", "Working Set",  Process.GetCurrentProcess().ProcessName);
        return _workingSetCounter;
      }
    }

    private PerformanceCounter _processorTimeCounter;
    private PerformanceCounter ProcessorTimeCounter
    {
      get
      {
        if (_processorTimeCounter == null)
          _processorTimeCounter = new PerformanceCounter("Process", "% Processor Time", Process.GetCurrentProcess().ProcessName);
        return _processorTimeCounter;
      }
    }

    #endregion

    #region 方法

    #region Plugin

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
    private bool RunPlugin(string sourceFileName)
    {
      bool result = false;
      try
      {
        string fileName = Path.GetFileName(sourceFileName);
        if (String.IsNullOrEmpty(fileName) || !File.Exists(sourceFileName))
        {
          InvokeResetHintRichTextBox(new MessageNotifyEventArgs(MessageNotifyType.Information, "Non-exist the plugin", sourceFileName));
          return false;
        }
        string assemblyPath = Path.Combine(AppConfig.BaseDirectory, fileName);
        if (String.Compare(sourceFileName, fileName, StringComparison.OrdinalIgnoreCase) != 0 &&
          String.Compare(sourceFileName, assemblyPath, StringComparison.OrdinalIgnoreCase) != 0)
          File.Copy(sourceFileName, assemblyPath, true);
        InvokeResetHintRichTextBox(new MessageNotifyEventArgs(MessageNotifyType.Information, "Running the plugin", fileName));
        Assembly assembly = AppDomain.CurrentDomain.Load(Path.GetFileNameWithoutExtension(fileName));
        IPlugin plugin = this.pluginHost.CreatePlugin(assembly, true);
        if (plugin != null)
        {
          try
          {
            IList<MessageNotifyEventArgs> messages = plugin.Initialization();
            if (messages != null)
              foreach (MessageNotifyEventArgs item in messages)
                InvokeResetHintRichTextBox(item);
          }
          catch (Exception ex)
          {
            InvokeResetHintRichTextBox(new MessageNotifyEventArgs(MessageNotifyType.Error, "Running the plugin", " failed to initialization.", ex));
          }
          lock (_pluginListViewLock)
          {
            InvokeResetPluginPrompt(new PluginEventArgs(plugin, fileName));
            if (!plugin.Start())
              InvokeResetHintRichTextBox(new MessageNotifyEventArgs(MessageNotifyType.Warning, "Running the plugin", plugin.Key + " failed to start."));
            InvokeResetPluginPrompt(new PluginEventArgs(plugin));
          }
          result = true;
        }
        else
          InvokeResetHintRichTextBox(new MessageNotifyEventArgs(MessageNotifyType.Warning, "Running the plugin", assembly.FullName + " failed to construct the plugin, may not exist of the plugin."));
      }
      catch (Exception ex)
      {
        InvokeResetHintRichTextBox(new MessageNotifyEventArgs(MessageNotifyType.Error, "Running the plugin", sourceFileName, ex));
      }
      return result;
    }

    private void RestorePlugins()
    {
      lock (_pluginListViewLock)
      {
        this.pluginListView.Items.Clear();
      }
      string value = AppSettings.ReadValue(PLUGINS_KEY, false, true);
      if (!String.IsNullOrEmpty(value))
        foreach (string s in value.Split(new Char[] {AppConfig.VALUE_SEPARATOR}, StringSplitOptions.RemoveEmptyEntries))
          RunPlugin(s);
      InvokeResetPluginPrompt(null);
    }

    private void SavePlugins()
    {
      string value = String.Empty;
      lock (_pluginListViewLock)
      {
        foreach (ListViewItem item in this.pluginListView.Items)
          value = value +
            item.SubItems[1].Text + AppConfig.VALUE_SEPARATOR;
      }
      AppSettings.SaveValue(PLUGINS_KEY, value, false, true);
    }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
    private void FinalizePlugins()
    {
      lock (_pluginListViewLock)
      {
        foreach (ListViewItem item in this.pluginListView.Items)
          try
          {
            InvokeResetHintRichTextBox(new MessageNotifyEventArgs(MessageNotifyType.Information, "Release the plugin", item.SubItems[0].Text));
            IPlugin plugin = item.Tag as IPlugin;
            if (plugin != null)
            {
              plugin.Finalization();
              item.ImageIndex = 0;
              item.Tag = null;
            }
          }
          catch (Exception ex)
          {
            InvokeResetHintRichTextBox(new MessageNotifyEventArgs(MessageNotifyType.Error, "Release the plugin", item.SubItems[0].Text, ex));
          }
      }
    }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
    private void DeleteSelectedPlugin()
    {
      lock (_pluginListViewLock)
      {
        for (int i = this.pluginListView.Items.Count - 1; i >= 0; i--)
        {
          ListViewItem item = this.pluginListView.Items[i];
          if (item.Selected)
            try
            {
              InvokeResetHintRichTextBox(new MessageNotifyEventArgs(MessageNotifyType.Information, "Delete the plugin", item.SubItems[0].Text));
              IPlugin plugin = item.Tag as IPlugin;
              if (plugin != null)
                plugin.Finalization();
              item.Remove();
            }
            catch (Exception ex)
            {
              InvokeResetHintRichTextBox(new MessageNotifyEventArgs(MessageNotifyType.Error, "Delete the plugin", item.SubItems[0].Text, ex));
            }
        }
        InvokeResetPluginPrompt(null);
      }
    }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
    private void SetupSelectedPlugin()
    {
      lock (_pluginListViewLock)
      {
        foreach (ListViewItem item in this.pluginListView.Items)
          if (item.Selected)
            try
            {
              InvokeResetHintRichTextBox(new MessageNotifyEventArgs(MessageNotifyType.Information, "Setup plugin", item.SubItems[0].Text));
              IPlugin plugin = item.Tag as IPlugin;
              if (plugin != null)
              {
                plugin.Setup(this);
                InvokeResetPluginPrompt(new PluginEventArgs(plugin));
              }
            }
            catch (Exception ex)
            {
              InvokeResetHintRichTextBox(new MessageNotifyEventArgs(MessageNotifyType.Error, "Setup plugin", item.SubItems[0].Text, ex));
            }
      }
    }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
    private void OpenSelectedPlugin()
    {
      lock (_pluginListViewLock)
      {
        foreach (ListViewItem item in this.pluginListView.Items)
          if (item.Selected)
            try
            {
              InvokeResetHintRichTextBox(new MessageNotifyEventArgs(MessageNotifyType.Information, "Start plugin", item.SubItems[0].Text));
              IPlugin plugin = item.Tag as IPlugin;
              if (plugin != null)
              {
                plugin.Start();
                InvokeResetPluginPrompt(new PluginEventArgs(plugin));
              }
            }
            catch (Exception ex)
            {
              InvokeResetHintRichTextBox(new MessageNotifyEventArgs(MessageNotifyType.Error, "Start plugin", item.SubItems[0].Text, ex));
            }
      }
    }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
    private void CloseSelectedPlugin()
    {
      lock (_pluginListViewLock)
      {
        foreach (ListViewItem item in this.pluginListView.Items)
          if (item.Selected)
            try
            {
              InvokeResetHintRichTextBox(new MessageNotifyEventArgs(MessageNotifyType.Information, "Close plugin", item.SubItems[0].Text));
              IPlugin plugin = item.Tag as IPlugin;
              if (plugin != null)
              {
                plugin.Suspend();
                InvokeResetPluginPrompt(new PluginEventArgs(plugin));
              }
            }
            catch (Exception ex)
            {
              InvokeResetHintRichTextBox(new MessageNotifyEventArgs(MessageNotifyType.Error, "Close plugin", item.SubItems[0].Text, ex));
            }
      }
    }

    #endregion

    private void DoInitialize()
    {
      this.Cursor = Cursors.WaitCursor;
      try
      {
        if (!DbConnectionInfo.Fetch().IsValid(true))
          goto Label;
        Phenix.Services.Library.Registration.RegisterWorker();
        if (!SystemInfo.Run(new Action<MessageNotifyEventArgs>(InvokeResetHintRichTextBox)))
        {
          if (MessageBox.Show("System initialization failed, whether to continue?",
            this.Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
            goto Label;
        }
        ServiceManager.Run(new Action<MessageNotifyEventArgs>(InvokeResetHintRichTextBox));
        Phenix.Services.Host.Synchro.SynchroService.RegisterService();
        DataSecurityInfoManager.Run(new Action<DataSecurityEventArgs>(InvokeResetUserListView));

        if (ServiceManager.UpgradeState == UpgradeState.Upgrading)
          this.upgradeTimer.Enabled = true;
        else
        {
          this.mainMenuToolStrip.Enabled = true;
          this.mainContextMenuStrip.ShowItemToolTips = true;

          DataDictionaryHub.GetAssemblyInfos(InvokeResetHintRichTextBox);
          RestorePlugins();

          if (ServiceManager.UpgradeState == UpgradeState.Upgraded)
            MarkUpgradedDialog.Execute();
        }

        Humanistic();
        return;
      }
      finally
      {
        this.Cursor = Cursors.Default;
      }
    
    Label:
      MessageBox.Show("Initialization failed, the system will be closed.", this.Text,
        MessageBoxButtons.OK, MessageBoxIcon.Error);
      _closeCountdown = 0;
      Application.Exit();
    }

    private void DoFinalize()
    {
      this.monitorTimer.Enabled = false;

      DataSecurityInfoManager.Stop();
      ServiceManager.Stop();
      SystemInfo.Stop();
    }

    private void StartShutdown()
    {
      if (ServiceManager.UpgradeState != UpgradeState.Upgrading)
      {
        FinalizePlugins();
        this.mainMenuToolStrip.Enabled = false;
        this.mainContextMenuStrip.ShowItemToolTips = false;
      }

      ServiceManager.MarkSuspending("Service has been shutdown !!!");
      InvokeResetHintRichTextBox(new MessageNotifyEventArgs(MessageNotifyType.Warning, ServiceManager.SuspendReason, "..."));

      this.shutdownTimer.Enabled = true;
    }

    private void ResetTimerTitle()
    {
      string text = String.Format("ServicesHost[{0}]", SystemInfo.EnterpriseName);
      if (text.Length >= 40)
        text = text.Substring(text.Length - 40);
      text = text + AppConfig.ROW_SEPARATOR + DateTime.Now.ToString("G", DateTimeFormatInfo.InvariantInfo);
      this.Text = text; //<=60个字符
      this.mainNotifyIcon.Text = text;
      Application.DoEvents();
    }

    private void ResetSystemInfoHint()
    {
      //显示使用内存大小
      float size = WorkingSetCounter.NextValue() / 1024 / 1024;
      this.useMemoryToolStripStatusLabel.Text = String.Format("{0:N}M", size);
      if (!Environment.Is64BitProcess)
        if (size >= 2 * 1024)
          this.useMemoryToolStripStatusLabel.BackColor = Color.Red;
      Application.DoEvents();
      //显示使用CPU比例
      float proportion = ProcessorTimeCounter.NextValue() / Environment.ProcessorCount;
      this.useCpuToolStripStatusLabel.Text = String.Format("{0:N}%", proportion);
      if (proportion >= 50)
        this.useCpuToolStripStatusLabel.BackColor = Color.Red;
      Application.DoEvents();
      //显示使用端口数量
      int count = IPGlobalProperties.GetIPGlobalProperties().GetActiveTcpConnections().Length;
      this.usePortsToolStripStatusLabel.Text = String.Format("{0}", count);
      if (count >= NetConfig.MaxUserPort * 2 / 3)
        this.usePortsToolStripStatusLabel.BackColor = Color.Red;
      Application.DoEvents();
    }

    /// <summary>
    /// 人性化
    /// </summary>
    private void Humanistic()
    {
      this.dataSourceToolStripStatusLabel.Text = DefaultDatabase.DbConnectionInfo.Key;
      AssemblyCopyrightAttribute assemblyCopyrightAttribute =
        (AssemblyCopyrightAttribute)Attribute.GetCustomAttribute(Assembly.GetExecutingAssembly(), typeof(AssemblyCopyrightAttribute));
      this.copyrightToolStripStatusLabel.Text =
        String.Format("{0} v{1} for .NET{2}, {3}", AppConfig.SYSTEM_NAME, SystemInfo.Version,
#if Top
        "4.6",
#else
        "4.0",
#endif
        assemblyCopyrightAttribute.Copyright);
      markDebuggingToolStripButton.Checked = AppConfig.Debugging;
      suspendServiceToolStripButton.Checked = ServiceManager.Suspending;
      suspendServiceToolStripMenuItem.Checked = ServiceManager.Suspending;
      markUpgradedToolStripButton.Checked = ServiceManager.UpgradeState == UpgradeState.Upgraded;
      if (ServiceManager.Suspending)
        InvokeResetHintRichTextBox(new MessageNotifyEventArgs(MessageNotifyType.Warning, "Services have been suspended", ServiceManager.SuspendReason));
      else
        InvokeResetHintRichTextBox(new MessageNotifyEventArgs(MessageNotifyType.Warning, "Services", Phenix.Services.Host.Properties.Resources.Ready));
    }

    private void CopyFile(DirectoryInfo sourceDirectoryInfo, string destDirectoryName, bool inSubdirectory)
    {
      if (!Directory.Exists(destDirectoryName))
        Directory.CreateDirectory(destDirectoryName);
      foreach (FileInfo item in sourceDirectoryInfo.GetFiles())
      {
        string path = Path.Combine(destDirectoryName, item.Name);
        if (!File.Exists(path))
        {
          item.CopyTo(path);
          InvokeResetHintRichTextBox(new MessageNotifyEventArgs(MessageNotifyType.Warning, "System upgrades in...", String.Format("Copy {0} to {1}", item.FullName, destDirectoryName)));
        }
      }
      if (inSubdirectory)
        foreach (DirectoryInfo item in sourceDirectoryInfo.GetDirectories())
          CopyFile(item, Path.Combine(destDirectoryName, item.Name), inSubdirectory);
    }

    #endregion

    #region 处理事件

    private void InvokeResetHintRichTextBox(MessageNotifyEventArgs e)
    {
      if (this.hintRichTextBox.InvokeRequired)
        this.hintRichTextBox.BeginInvoke(new Action<MessageNotifyEventArgs>(ResetHintRichTextBox), new object[] { e });
      else
        ResetHintRichTextBox(e);
      Application.DoEvents();
    }

    private void InvokeResetHintRichTextBox(object sender, MessageNotifyEventArgs e)
    {
      InvokeResetHintRichTextBox(e);
    }

    private void ResetHintRichTextBox(MessageNotifyEventArgs e)
    {
      if (this.hintRichTextBox.Lines.Length > 1000)
        this.hintRichTextBox.Clear();

      if (e.Error == null)
        this.hintRichTextBox.AppendText(
          String.Format("{0} {1} - {2}{3}", DateTime.Now, e.Title, e.Message, Environment.NewLine));
      else
        this.hintRichTextBox.AppendText(
          String.Format("{0} {1} Failed - {2}: {3}{4}", DateTime.Now, e.Title, e.Message, AppUtilities.GetErrorMessage(e.Error), Environment.NewLine));
      this.hintRichTextBox.ScrollToCaret();
    }

    private void InvokeResetUserListView(DataSecurityEventArgs e)
    {
      if (this.userListView.InvokeRequired)
        this.userListView.BeginInvoke(new Action<DataSecurityEventArgs>(ResetUserListView), new object[] { e });
      else
        ResetUserListView(e);
      Application.DoEvents();
    }

    private void ResetUserListView(DataSecurityEventArgs e)
    {
      ListViewItem item;
      if (!this.userListView.Items.ContainsKey(e.UserNumber))
      {
        item = this.userListView.Items.Add(e.UserNumber, e.UserNumber, e.LogOn ? 1 : 0);
        item.SubItems.Add(e.LocalAddress);
        item.SubItems.Add(e.Time.ToString("u"));
      }
      else
      {
        item = this.userListView.Items[e.UserNumber];
        item.ImageIndex = e.LogOn ? 1 : 0;
        item.SubItems[1].Text = e.LocalAddress;
        item.SubItems[2].Text = e.Time.ToString("u");
      }
      this.usersToolStripStatusLabel.Text = this.userListView.Items.Count.ToString();
    }

    private void InvokeResetPluginPrompt(PluginEventArgs e)
    {
      if (this.pluginListView.InvokeRequired)
        this.pluginListView.BeginInvoke(new Action<PluginEventArgs>(ResetPluginPrompt), new object[] { e });
      else
        ResetPluginPrompt(e);
      Application.DoEvents();
    }

    private void ResetPluginPrompt(PluginEventArgs e)
    {
      if (e != null && e.Plugin != null)
      {
        ListViewItem item = null;
        if (!this.pluginListView.Items.ContainsKey(e.Plugin.GetType().FullName))
        {
          if (e.Plugin.State != PluginState.Finalizing)
          {
            item = this.pluginListView.Items.Add(e.Plugin.GetType().FullName, e.Plugin.GetType().FullName,
              e.Plugin.State == PluginState.Finalizing || e.Plugin.State == PluginState.Suspended ? 0 : 1);
            item.SubItems.Add(e.Path);
            item.SubItems.Add(e.Time.ToString("u"));
            item.SubItems.Add(e.Message != null ? e.Message.ToString() : String.Empty);
            item.Tag = e.Plugin;
          }
        }
        else
        {
          item = this.pluginListView.Items[e.Plugin.GetType().FullName];
          item.ImageIndex = e.Plugin.State == PluginState.Finalizing || e.Plugin.State == PluginState.Suspended ? 0 : 1;
          if (e.Path != null)
            item.SubItems[1].Text = e.Path;
          item.SubItems[2].Text = e.Time.ToString("u");
          if (e.Message != null)
            item.SubItems[3].Text = e.Message.ToString();
          item.Tag = e.Plugin;
        }
        if (item != null)
        {
          MessageNotifyEventArgs args = e.Message as MessageNotifyEventArgs;
          item.SubItems[3].ForeColor = args != null ? args.Color : Color.White;
        }
      }

      bool havePlugin = this.pluginListView.Items.Count > 0;
      this.deletePluginToolStripMenuItem.Visible = havePlugin;
      this.openPluginToolStripMenuItem.Visible = havePlugin;
      this.closePluginToolStripMenuItem.Visible = havePlugin;
      this.setupPluginToolStripMenuItem.Visible = havePlugin;
      this.pluginsToolStripStatusLabel.Text = this.pluginListView.Items.Count.ToString();
    }

    #endregion

    private void MainForm_Shown(object sender, EventArgs e)
    {
      DoInitialize();
    }

    private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
    {
      if (_closeCountdown <= 0)
        DoFinalize();
      else if (e.CloseReason == CloseReason.UserClosing)
      {
        e.Cancel = true;
        if (MessageBox.Show("Are you sure you want to exit system?", this.Text,
          MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
          StartShutdown();
      }
    }

    private void MainForm_Resize(object sender, EventArgs e)
    {
      if (this.WindowState == FormWindowState.Minimized)
        this.Hide();
    }

    private void mainNotifyIcon_DoubleClick(object sender, EventArgs e)
    {
      this.Show();
      this.WindowState = FormWindowState.Normal;
    }

    private void setDefaultDatabaseToolStripButton_Click(object sender, EventArgs e)
    {
      do
      {
        if (!DbConnectionInfo.Fetch().Setup())
          break;
        bool succeed;
        this.Cursor = Cursors.WaitCursor;
        try
        {
          succeed = SystemInfo.Initialize();
        }
        finally
        {
          this.Cursor = Cursors.Default;
        }
        if (succeed)
        {
          if (MessageBox.Show(
@"You have changed the default database connection configuration.
Because the original configuration still in operation in the application service, the proposal to restart service off new configuration loading database.
Need to shut down this service?",
            setDefaultDatabaseToolStripButton.Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            StartShutdown();
          break;
        }
      } while (true);
    }

    private void systemInfoToolStripButton_Click(object sender, EventArgs e)
    {
      if (SystemInfo.Setup())
      {
        this.Cursor = Cursors.WaitCursor;
        try
        {
          DataDictionaryHub.ClearCache();
          Humanistic();
        }
        finally
        {
          this.Cursor = Cursors.Default;
        }
      }
    }

    private void clearPerformanceAnalyseInfosToolStripMenuItem_Click(object sender, EventArgs e)
    {
      PerformanceAnalyse.Default.ClearCache();
      InvokeResetHintRichTextBox(new MessageNotifyEventArgs(MessageNotifyType.Information, "clear PerformanceAnalyse infos",
        Phenix.Services.Host.Properties.Resources.Finished));
    }

    private void registerAssemblyInfoToolStripMenuItem_Click(object sender, EventArgs e)
    {
      ServiceManager.RegisterAssembly();
    }

    private void resetCacheToolStripMenuItem_Click(object sender, EventArgs e)
    {
      this.Cursor = Cursors.WaitCursor;
      try
      {
        DataDictionaryHub.AllInfoHasChanged();
        WorkflowHub.AllInfoHasChanged();
        DataDictionaryHub.GetAssemblyInfos(InvokeResetHintRichTextBox);
      }
      finally
      {
        this.Cursor = Cursors.Default;
      }
      InvokeResetHintRichTextBox(new MessageNotifyEventArgs(MessageNotifyType.Information, "reset all cache",
        Phenix.Services.Host.Properties.Resources.Finished));
    }

    private void businessCodeFormatToolStripButton_Click(object sender, EventArgs e)
    {
      Phenix.Core.Dictionary.Windows.BusinessCodeFormatManageDialog.Execute(InvokeResetHintRichTextBox);
    }
    private void markDebuggingToolStripButton_Click(object sender, EventArgs e)
    {
      AppConfig.Debugging = !AppConfig.Debugging;
      if (AppConfig.Debugging)
        Phenix.Core.Dictionary.DataDictionaryHub.MessageNotify += new EventHandler<MessageNotifyEventArgs>(InvokeResetHintRichTextBox);
      else
        Phenix.Core.Dictionary.DataDictionaryHub.MessageNotify -= new EventHandler<MessageNotifyEventArgs>(InvokeResetHintRichTextBox);
      InvokeResetHintRichTextBox(new MessageNotifyEventArgs(MessageNotifyType.Warning, "Debugging", AppConfig.Debugging.ToString()));
    }

    private void suspendServiceToolStripButton_Click(object sender, EventArgs e)
    {
      if (ServiceManager.Suspending)
        ServiceManager.Regain();
      else
        SuspendServiceDialog.Execute();
      Humanistic();
    }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
    private void upgradeServiceToolStripButton_Click(object sender, EventArgs e)
    {
      if (MessageBox.Show(@"Whether to upgrade the service?", this.Text,
        MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) != DialogResult.Yes)
        return;
      InvokeResetHintRichTextBox(new MessageNotifyEventArgs(MessageNotifyType.Warning, "Upgrade service", "start backing up service..."));
      try
      {
        this.Cursor = Cursors.WaitCursor;
        try
        {
          //将根目录上的文件copy到AppConfig.ServiceLibrarySubdirectory子目录里
          File.Copy(AppDomain.CurrentDomain.SetupInformation.ConfigurationFile, Path.Combine(AppConfig.ServiceLibrarySubdirectory, Path.GetFileName(AppDomain.CurrentDomain.SetupInformation.ConfigurationFile)), true);
          CopyFile(new DirectoryInfo(AppConfig.BaseDirectory), AppConfig.ServiceLibrarySubdirectory, false);
          ////将子目录AppConfig.DefaultClientLibrarySubdirectory上的文件copy到AppConfig.ServiceLibrarySubdirectory的AppConfig.ClientLibrarySubdirectoryName子目录里
          //CopyFile(new DirectoryInfo(AppConfig.ClientLibrarySubdirectory), Path.Combine(AppConfig.ServiceLibrarySubdirectory, AppConfig.CLIENT_LIBRARY_SUBDIRECTORY_NAME), true);
          //启动AppConfig.ServiceLibrarySubdirectory子目录上Host
        }
        finally
        {
          this.Cursor = Cursors.Default;
        }
        if (MessageBox.Show(String.Format("Please confirm {0} directory services have been stored at the new components, after the completion of the click this confirmation button continued to escalate.", AppConfig.ServiceLibrarySubdirectory),
          upgradeServiceToolStripButton.Text, MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation) == DialogResult.OK)
        {
          Process.Start(Path.Combine(AppConfig.ServiceLibrarySubdirectory, AppDomain.CurrentDomain.FriendlyName), UpgradeState.Upgrading.ToString());
          StartShutdown();
        }
      }
      catch (Exception ex)
      {
        MessageBox.Show(String.Format("Upgrade service failed:\n{0}", AppUtilities.GetErrorHint(ex)),
          upgradeServiceToolStripButton.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
      }
    }

    private void markUpgradedToolStripButton_Click(object sender, EventArgs e)
    {
      MarkUpgradedDialog.Execute();
      Humanistic();
    }

    private void synchroHostToolStripButton_Click(object sender, EventArgs e)
    {
      Phenix.Services.Host.Synchro.SynchroDialog.Execute(InvokeResetHintRichTextBox);
    }

    private void exitToolStripButton_Click(object sender, EventArgs e)
    {
      this.Close();
    }

    private void shutdownTimer_Tick(object sender, EventArgs e)
    {
      this.Text = Phenix.Core.Properties.Resources.PleaseWait;

      shutdownTimer.Enabled = false;
      try
      {
        InvokeResetHintRichTextBox(new MessageNotifyEventArgs(MessageNotifyType.Warning, "Shutdown", _closeCountdown.ToString()));
        _closeCountdown = _closeCountdown - 1;
        if (_closeCountdown <= 0)
        {
          InvokeResetHintRichTextBox(new MessageNotifyEventArgs(MessageNotifyType.Warning, "Shutdown", String.Empty));
          try
          {
            Process.Start(Path.Combine(Path.GetDirectoryName(AppConfig.BaseDirectory), "Phenix.Services.Host.Kaishaku.exe"));
          }
          catch (System.ComponentModel.Win32Exception ex)
          {
            InvokeResetHintRichTextBox(new MessageNotifyEventArgs(MessageNotifyType.Error, "Running Phenix.Services.Host.Kaishaku.exe", ex));
          }
          this.Close();
        }
      }
      finally
      {
        if (_closeCountdown > 0)
          shutdownTimer.Enabled = true;
      }
    }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
    private void upgradeTimer_Tick(object sender, EventArgs e)
    {
      bool find = false;
      upgradeTimer.Enabled = false;
      try
      {
        Process currentProcess = Process.GetCurrentProcess();
        foreach (Process item in Process.GetProcessesByName(currentProcess.ProcessName))
          if (item.Id != currentProcess.Id)
          {
            find = true;
            break;
          }
        if (!find)
          try
          {
            this.Cursor = Cursors.WaitCursor;
            try
            {
              //取根目录
              string rootDirectory = Path.GetDirectoryName(Path.GetDirectoryName(AppConfig.BaseDirectory));
              //将根目录上的AppConfig.ServiceLibrarySubdirectoryName子目录里文件copy到根目录上
              CopyFile(new DirectoryInfo(Path.Combine(rootDirectory, AppConfig.SERVICE_LIBRARY_SUBDIRECTORY_NAME)), rootDirectory, false);
              ////将根目录上的AppConfig.ServiceLibrarySubdirectoryName子目录的AppConfig.ClientLibrarySubdirectoryName子目录里文件copy到根目录上的AppConfig.ClientLibrarySubdirectoryName目录上
              //CopyFile(new DirectoryInfo(Path.Combine(rootDirectory, AppConfig.SERVICE_LIBRARY_SUBDIRECTORY_NAME, AppConfig.CLIENT_LIBRARY_SUBDIRECTORY_NAME)), Path.Combine(rootDirectory, AppConfig.CLIENT_LIBRARY_SUBDIRECTORY_NAME), true);
              //启动根目录上Host
              InvokeResetHintRichTextBox(new MessageNotifyEventArgs(MessageNotifyType.Warning, "System upgrades in...", "Start formal service..."));
              Process.Start(Path.Combine(rootDirectory, AppDomain.CurrentDomain.FriendlyName), UpgradeState.Upgraded.ToString());
              StartShutdown();
            }
            finally
            {
              this.Cursor = Cursors.Default;
            }
          }
          catch (Exception ex)
          {
            MessageBox.Show(String.Format("Upgrade Failed:\n{0}", AppUtilities.GetErrorHint(ex)),
              upgradeServiceToolStripButton.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
          }
        else
          InvokeResetHintRichTextBox(new MessageNotifyEventArgs(MessageNotifyType.Warning, "System upgrades in...", "The upgrade process please don't do any operation, this program as a temporary service, wait for a service to shut down."));
      }
      finally
      {
        if (find)
          upgradeTimer.Enabled = true;
      }
    }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
    private void addPluginToolStripMenuItem_Click(object sender, EventArgs e)
    {
      using (OpenFileDialog openFileDialog = new OpenFileDialog())
      {
        openFileDialog.RestoreDirectory = true;
        openFileDialog.Filter = "plugin|*.dll";
        openFileDialog.Title = "New plugin";
        if (openFileDialog.ShowDialog() == DialogResult.OK)
          try
          {
            if (RunPlugin(openFileDialog.FileName))
            {
              ServiceManager.RegisterAssembly(openFileDialog.FileName);
              SavePlugins();
            }
          }
          catch (Exception ex)
          {
            MessageBox.Show(String.Format("New plugin Failed:\n{0}", AppUtilities.GetErrorHint(ex)),
              openFileDialog.Title, MessageBoxButtons.OK, MessageBoxIcon.Error);
          }
      }
    }

    private void deletePluginToolStripMenuItem_Click(object sender, EventArgs e)
    {
      if (MessageBox.Show("To determine whether delete choice of plugin?", this.Text,
       MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) != DialogResult.Yes)
        return;
      DeleteSelectedPlugin();
      SavePlugins();
    }

    private void setupPluginToolStripMenuItem_Click(object sender, EventArgs e)
    {
      SetupSelectedPlugin();
    }

    private void openPluginToolStripMenuItem_Click(object sender, EventArgs e)
    {
      OpenSelectedPlugin();
    }

    private void closePluginToolStripMenuItem_Click(object sender, EventArgs e)
    {
      if (MessageBox.Show("To confirm that closed and the choice of plugin?", this.Text,
       MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) != DialogResult.Yes)
        return;
      CloseSelectedPlugin();
    }

    private void pluginListView_MouseDoubleClick(object sender, MouseEventArgs e)
    {
      SetupSelectedPlugin();
    }

    private void pluginHost_Finalized(object sender, Phenix.Core.Plugin.PluginEventArgs e)
    {
      lock (_pluginListViewLock)
      {
        InvokeResetPluginPrompt(e);
      }
    }

    private void pluginHost_Message(object sender, Phenix.Core.Plugin.PluginEventArgs e)
    {
      lock (_pluginListViewLock)
      {
        InvokeResetPluginPrompt(e);
      }
    }

    private void monitorTimer_Tick(object sender, EventArgs e)
    {
      if (this.upgradeTimer.Enabled)
        return;

      this.monitorTimer.Enabled = false;
      try
      {
        ResetTimerTitle();
        ResetSystemInfoHint();
      }
      finally
      {
        if (!this.shutdownTimer.Enabled)
          this.monitorTimer.Enabled = true;        
      }
    }
  }
}