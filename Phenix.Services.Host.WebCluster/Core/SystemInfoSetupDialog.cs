using System;
using System.Windows.Forms;
using Phenix.Core.Net;
using Phenix.Core.Web;

namespace Phenix.Services.Host.WebCluster.Core
{
  internal partial class SystemInfoSetupDialog : Phenix.Core.Windows.DialogForm
  {
    private SystemInfoSetupDialog()
    {
      InitializeComponent();

      this.webApiPortNumericUpDown.Value = WebConfig.WebApiPort;
      this.webApiSslPortNumericUpDown.Value = WebConfig.WebApiSslPort;
      if (String.IsNullOrEmpty(this.webApiSslIdnHostTextBox.Text))
        this.webApiSslIdnHostTextBox.Text = NetConfig.LocalAddress;
      this.webSocketPortNumericUpDown.Value = WebConfig.WebSocketPort;
      this.webSocketSslPortNumericUpDown.Value = WebConfig.WebSocketSslPort;
      if (String.IsNullOrEmpty(this.webSocketSslIdnHostTextBox.Text))
        this.webSocketSslIdnHostTextBox.Text = Phenix.Services.Host.WebCluster.Properties.Settings.Default.WebEnableCorsOrigins;
    }

    #region ¹¤³§

    /// <summary>
    /// Ö´ÐÐ
    /// </summary>
    public static bool Execute()
    {
      using (SystemInfoSetupDialog dialog = new SystemInfoSetupDialog())
      {
        return dialog.ShowDialog() == DialogResult.OK;
      }
    }

    #endregion

    private void OK_Click(object sender, EventArgs e)
    {
      Phenix.Services.Host.WebCluster.Properties.Settings.Default.Save();

      WebConfig.WebApiPort = (int)this.webApiPortNumericUpDown.Value;
      WebConfig.WebApiSslPort = (int)this.webApiSslPortNumericUpDown.Value;
      WebConfig.WebSocketPort = (int)this.webSocketPortNumericUpDown.Value;
      WebConfig.WebSocketSslPort = (int)this.webSocketSslPortNumericUpDown.Value;

      this.DialogResult = DialogResult.OK;
    }

    private void webApiSslIdnHostTextBox_TextChanged(object sender, EventArgs e)
    {
      if (String.IsNullOrEmpty(this.webApiSslIdnHostTextBox.Text))
        this.webApiSslIdnHostTextBox.Text = NetConfig.LocalAddress;
    }
  }
}