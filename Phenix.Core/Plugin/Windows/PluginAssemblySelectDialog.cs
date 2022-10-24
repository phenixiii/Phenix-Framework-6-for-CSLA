using System;
using System.Windows.Forms;
using Phenix.Core.Reflection;
using Phenix.Core.Windows;

namespace Phenix.Core.Plugin.Windows
{
  /// <summary>
  /// 选择插件对话框
  /// </summary>
  public partial class PluginAssemblySelectDialog : DialogForm
  {
    private PluginAssemblySelectDialog(string selectedPluginAssemblyName)
    {
      InitializeComponent();

      this.Text = Resources.PluginAssemblySelect;
      this.okButton.Text = Phenix.Core.Properties.Resources.Ok;
      this.cancelButton.Text = Phenix.Core.Properties.Resources.Cancel;

      PluginInfo selectedPluginInfo = null;
      foreach (Type item in Utilities.LoadExportedSubclassTypesFromBaseDirectory(false, typeof(IPlugin)))
      {
        PluginInfo pluginInfo = PluginHost.GetPluginInfo(item, false);
        if (pluginInfo != null)
        {
          this.pluginInfoListBox.Items.Add(pluginInfo);
          if (selectedPluginInfo == null && String.CompareOrdinal(item.Assembly.GetName().Name, selectedPluginAssemblyName) == 0)
            selectedPluginInfo = pluginInfo;
        }
        this.pluginInfoListBox.SelectedItem = selectedPluginInfo;
      }
    }

    #region 工厂

    /// <summary>
    /// 执行
    /// </summary>
    public static PluginInfo Execute()
    {
      return Execute(null);
    }

    /// <summary>
    /// 执行
    /// </summary>
    public static PluginInfo Execute(string selectedPluginAssemblyName)
    {
      using (PluginAssemblySelectDialog dialog = new PluginAssemblySelectDialog(selectedPluginAssemblyName))
      {
        if (dialog.ShowDialog() == DialogResult.OK)
          return dialog.SelectedPluginInfo;
      }
      return null;
    }

    #endregion

    #region 属性
    
    /// <summary>
    /// 被选择的插件信息
    /// </summary>
    public PluginInfo SelectedPluginInfo
    {
      get
      {
        return this.pluginInfoListBox.SelectedIndex >= 0
          ? (PluginInfo)this.pluginInfoListBox.SelectedItem
          : null;
      }
    }

    #endregion

    private void pluginInfoListBox_DoubleClick(object sender, EventArgs e)
    {
      this.DialogResult = DialogResult.OK;
    }
  }
}
