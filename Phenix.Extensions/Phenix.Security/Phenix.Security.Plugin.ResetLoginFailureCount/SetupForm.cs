using System.Windows.Forms;

namespace Phenix.Security.Plugin.ResetLoginFailureCount
{
  public partial class SetupForm : Phenix.Core.Windows.DialogForm
  {
    private SetupForm(Plugin plugin)
    {
      InitializeComponent();

      _plugin = plugin;
      this.resetIntervalNumericUpDown.Value = plugin.ResetInterval;
    }
    
    #region 工厂

    /// <summary>
    /// 执行
    /// </summary>
    /// <param name="plugin">插件</param>
    public static bool Execute(Plugin plugin)
    {
      using (SetupForm form = new SetupForm(plugin))
      {
        return (form.ShowDialog() == DialogResult.OK);
      }
    }

    #endregion

    #region 属性

    private readonly Plugin _plugin;

    #endregion

    private void okButton_Click(object sender, System.EventArgs e)
    {
      _plugin.ResetInterval = (int)this.resetIntervalNumericUpDown.Value;
      this.DialogResult = DialogResult.OK;
    }
  }
}
