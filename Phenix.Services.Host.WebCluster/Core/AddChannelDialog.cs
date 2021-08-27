using System.Windows.Forms;

namespace Phenix.Services.Host.WebCluster.Core
{
  internal partial class AddChannelDialog : Phenix.Core.Windows.DialogForm
  {
    private AddChannelDialog()
    {
      InitializeComponent();
    }

    #region ¹¤³§

    public static Channel Execute()
    {
      using (AddChannelDialog dialog = new AddChannelDialog())
      {
        dialog.hostTextBox.Focus();
        return dialog.ShowDialog() == DialogResult.OK 
          ? new Channel(dialog.hostTextBox.Text, (int)dialog.webApiPortNumericUpDown.Value, (int)dialog.webSocketPortNumericUpDown.Value)
          : null;
      }
    }

    #endregion
  }
}