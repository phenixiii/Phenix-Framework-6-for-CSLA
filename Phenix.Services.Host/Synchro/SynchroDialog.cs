using System;
using System.Windows.Forms;
using System.Collections.Generic;
using Phenix.Core;

namespace Phenix.Services.Host.Synchro
{
  internal partial class SynchroDialog : Phenix.Core.Windows.DialogForm
  {
    private SynchroDialog(Action<MessageNotifyEventArgs> doMessageNotify)
    {
      InitializeComponent();

      _synchroProxy = new SynchroProxy();
      _synchroProxy.MessageNotify += doMessageNotify;
    }

    #region 工厂

    public static void Execute(Action<MessageNotifyEventArgs> doMessageNotify)
    {
      using (SynchroDialog dialog = new SynchroDialog(doMessageNotify))
      {
        dialog.ShowDialog();
      }
    }

    #endregion

    #region 属性

    private const string HOSTS_KEY = "SynchroHosts";

    private readonly SynchroProxy _synchroProxy;

    #endregion

    #region 方法

    private void LoadConfig()
    {
      string s = AppSettings.ReadValue(HOSTS_KEY, false, true);
      if (!String.IsNullOrEmpty(s))
        hostsCheckedListBox.Items.AddRange(s.Split(new Char[] { AppConfig.VALUE_SEPARATOR }, StringSplitOptions.RemoveEmptyEntries));
    }

    private void SaveConfig()
    {
      string hosts = String.Empty;
      foreach (string s in hostsCheckedListBox.Items)
        hosts = hosts + s + AppConfig.VALUE_SEPARATOR;
      AppSettings.SaveValue(HOSTS_KEY, hosts, false, true);
    }

    #endregion

    #region 事件

    private void SynchroDialog_Shown(object sender, EventArgs e)
    {
      LoadConfig();
    }

    private void hostTextBox_KeyPress(object sender, KeyPressEventArgs e)
    {
      if (e.KeyChar == '\r')
        addButton_Click(sender, e);
    }

    private void addButton_Click(object sender, EventArgs e)
    {
      if (hostsCheckedListBox.Items.Contains(hostTextBox.Text))
        return;
      hostsCheckedListBox.SetItemChecked(hostsCheckedListBox.Items.Add(hostTextBox.Text), true);
      SaveConfig();
    }

    private void delButton_Click(object sender, EventArgs e)
    {
      if (!String.IsNullOrEmpty(hostTextBox.Text))
        hostsCheckedListBox.Items.Remove(hostTextBox.Text);
      else
      {
        if (hostsCheckedListBox.Items.Count > 0)
          hostsCheckedListBox.Items.RemoveAt(0);
      }
      SaveConfig();
    }

    private void okButton_Click(object sender, EventArgs e)
    {
      okButton.Enabled = false;
      cancelButton.Text = "Stop";
      try
      {
        this.Cursor = Cursors.WaitCursor;
        try
        {
          List<string> hosts = new List<string>();
          foreach (string s in hostsCheckedListBox.CheckedItems)
            hosts.Add(s);
          _synchroProxy.Deploy(hosts.ToArray());
        }
        finally
        {
          this.Cursor = Cursors.Default;
        }
      }
      finally
      {
        okButton.Enabled = true;
        cancelButton.Text = "Quit";
      }
    }

    private void cancelButton_Click(object sender, EventArgs e)
    {
      if (_synchroProxy.ShutDown)
        this.DialogResult = DialogResult.Cancel;
      else if (MessageBox.Show("Sure you want to suspend synchronization service?",
        this.Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
        _synchroProxy.ShutDown = true;
    }

    #endregion
  }
}
