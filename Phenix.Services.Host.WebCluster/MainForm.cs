using System;
using System.Diagnostics;
using System.Drawing;
using System.Net.NetworkInformation;
using System.Reflection;
using System.Windows.Forms;
using Phenix.Core;
using Phenix.Core.Net;
using Phenix.Services.Host.WebCluster.Core;

namespace Phenix.Services.Host.WebCluster
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

    private ServiceManager _serviceManager;
    private volatile int _closeCountdown = 10;

    #endregion

    #region 方法

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

    private void ResetChannelTotal()
    {
      this.channelsToolStripStatusLabel.Text = this.channelListView.Items.Count.ToString();
    }

    /// <summary>
    /// 人性化
    /// </summary>
    private void Humanistic()
    {
      AssemblyCopyrightAttribute assemblyCopyrightAttribute =
        (AssemblyCopyrightAttribute)Attribute.GetCustomAttribute(Assembly.GetExecutingAssembly(), typeof(AssemblyCopyrightAttribute));
      this.copyrightToolStripStatusLabel.Text =
        String.Format("{0} for .NET{1}, {2}", AppConfig.SYSTEM_NAME, 
        "4.6",
        assemblyCopyrightAttribute.Copyright);
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

    private void InvokeResetChannelListView(Channel channel)
    {
      if (this.channelListView.InvokeRequired)
        this.channelListView.BeginInvoke(new Action<Channel>(ResetChannelListView), new object[] { channel });
      else
        ResetChannelListView(channel);
      Application.DoEvents();
    }

    private void ResetChannelListView(Channel channel)
    {
      if (channel == null)
        foreach (ListViewItem item in this.channelListView.Items)
          InvokeResetChannelListView((Channel)item.Tag);
      else
      {
        ListViewItem item;
        if (!this.channelListView.Items.ContainsKey(channel.ToString()))
        {
          item = this.channelListView.Items.Add(channel.ToString(), channel.ToString(), 0);
          item.SubItems.Add(channel.LastActionTime.ToString("u"));
          item.SubItems.Add(channel.ResponseTimes.ToString());
          item.SubItems.Add(channel.ErrorTimes.ToString());
          item.SubItems.Add(Phenix.Core.AppUtilities.GetErrorHint(channel.LastError));
          item.Tag = channel;
          ResetChannelTotal();
        }
        else
        {
          item = this.channelListView.Items[channel.ToString()];
          item.ImageIndex = channel.ErrorTimes > 0 ? 2 : channel.ResponseTimes > 0 ? 1 : 0;
          item.SubItems[1].Text = channel.LastActionTime.ToString("u");
          item.SubItems[2].Text = channel.ResponseTimes.ToString();
          item.SubItems[3].Text = channel.ErrorTimes.ToString();
          item.SubItems[4].Text = Phenix.Core.AppUtilities.GetErrorHint(channel.LastError);
        }
      }
    }

    #endregion

    private void MainForm_Shown(object sender, EventArgs e)
    {
      this.Cursor = Cursors.WaitCursor;
      try
      {
        _serviceManager = ServiceManager.Run(new Action<MessageNotifyEventArgs>(InvokeResetHintRichTextBox));
        foreach (Channel item in ChannelManager.Default)
          InvokeResetChannelListView(item);
        Humanistic();
      }
      finally
      {
        this.Cursor = Cursors.Default;
      }
    }

    private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
    {
      if (_closeCountdown <= 0)
      {
        _serviceManager.Dispose();
        return;
      }
      if (e.CloseReason == CloseReason.UserClosing)
      {
        e.Cancel = true;
        if (MessageBox.Show("Are you sure you want to exit system?", this.Text,
          MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
        {
          InvokeResetHintRichTextBox(new MessageNotifyEventArgs(MessageNotifyType.Warning, "Service has been shut down", "No longer response new request, has even terminal will be automatically transfer the other service activities."));
          this.shutdownTimer.Enabled = true;
          this.monitorTimer.Enabled = false;
          this.mainMenuToolStrip.Enabled = false;
          this.mainContextMenuStrip.Enabled = false;
          this.channelContextMenuStrip.Enabled = false;
          this.channelListView.Items.Clear();
          ChannelManager.Default.Dispose();
        }
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

    private void systemInfoToolStripButton_Click(object sender, EventArgs e)
    {
      if (SystemInfoSetupDialog.Execute())
        _serviceManager.Reset();
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
        InvokeResetHintRichTextBox(new MessageNotifyEventArgs(MessageNotifyType.Warning, "System closing...", _closeCountdown.ToString()));
        _closeCountdown = _closeCountdown - 1;
        if (_closeCountdown <= 0)
        {
          InvokeResetHintRichTextBox(new MessageNotifyEventArgs(MessageNotifyType.Warning, "System closed", String.Empty));
          this.Close();
        }
      }
      finally
      {
        if (_closeCountdown > 0)
          shutdownTimer.Enabled = true;
      }
    }

    private void addChannelToolStripMenuItem_Click(object sender, EventArgs e)
    {
      Channel channel = AddChannelDialog.Execute();
      if (channel != null)
      {
        ChannelManager.Default.AddChannel(channel);
        ResetChannelListView(channel);
      }
    }

    private void deleteChannelToolStripMenuItem_Click(object sender, EventArgs e)
    {
      for (int i = this.channelListView.Items.Count - 1; i >= 0; i--)
      {
        ListViewItem item = this.channelListView.Items[i];
        if (item.Selected)
          if (MessageBox.Show(String.Format("Delete this channel: {0}?", item.Text), this.Text,
            MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
          {
            ChannelManager.Default.DeleteChannel((Channel)item.Tag);
            item.Remove();
            ResetChannelTotal();
            InvokeResetHintRichTextBox(new MessageNotifyEventArgs(MessageNotifyType.Information, "Delete the channel", item.Text));
          }
      }
    }

    private void monitorTimer_Tick(object sender, EventArgs e)
    {
      this.monitorTimer.Enabled = false;
      try
      {
        ResetSystemInfoHint();
        InvokeResetChannelListView(null);
      }
      finally
      {
        if (!this.shutdownTimer.Enabled)
          this.monitorTimer.Enabled = true;        
      }
    }
  }
}