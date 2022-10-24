using System.Windows.Forms;
using Phenix.Core.Rule;
using Phenix.Core.Windows;

namespace Phenix.Core.Dictionary.Windows
{
  /// <summary>
  /// 添加业务码流水号格式
  /// </summary>
  public partial class BusinessCodeSerialFormatAddDialog : DialogForm
  {
    private BusinessCodeSerialFormatAddDialog()
    {
      InitializeComponent();
     
      this.formatStringLabel.Text = Resources.FormatString;
      this.lengthLabel.Text = Resources.Length;
      this.initialValueLabel.Text = Resources.InitialValue;
      this.groupModeLabel.Text = Resources.GroupMode;
      this.resetCycleLabel.Text = Resources.ResetCycle;
      this.okButton.Text = Phenix.Core.Properties.Resources.Ok;
      this.cancelButton.Text = Phenix.Core.Properties.Resources.Cancel;

      this.businessCodeSerialFormatBindingSource.DataSource = new BusinessCodeSerialFormat();
      this.businessCodeSerialGroupModeEnumKeyCaptionCollectionBindingSource.DataSource = EnumKeyCaptionCollection.Fetch<BusinessCodeSerialGroupMode>();
      this.businessCodeSerialResetCycleEnumKeyCaptionCollectionBindingSource.DataSource = EnumKeyCaptionCollection.Fetch<BusinessCodeSerialResetCycle>();
    }

    #region 工厂

    /// <summary>
    /// 执行
    /// </summary>
    public static string Execute(BusinessCodeFormat businessCodeFormat)
    {
      using (BusinessCodeSerialFormatAddDialog dialog = new BusinessCodeSerialFormatAddDialog())
      {
        dialog.Text = string.Format(Resources.BusinessCodeSerialFormatAdd, businessCodeFormat.Caption, businessCodeFormat.BusinessCodeName);
        return dialog.ShowDialog() == DialogResult.OK ? dialog.WorkingBusinessCodeSerialFormat.FormatString : null;
      }
    }

    #endregion

    #region 属性

    private BusinessCodeSerialFormat WorkingBusinessCodeSerialFormat
    {
      get { return BindingSourceHelper.GetDataSourceCurrent(this.businessCodeSerialFormatBindingSource) as BusinessCodeSerialFormat; }
    }

    #endregion

    #region 方法

    private void ResetbusinessCodeSerialFormatBindingSource()
    {
      this.businessCodeSerialFormatBindingSource.ResetBindings(false);
    }

    #endregion

    private void groupModeComboBox_SelectedValueChanged(object sender, System.EventArgs e)
    {
      if (groupModeComboBox.SelectedItem == null)
        return;
      WorkingBusinessCodeSerialFormat.GroupMode = (BusinessCodeSerialGroupMode)((EnumKeyCaption)groupModeComboBox.SelectedItem).Value;
      ResetbusinessCodeSerialFormatBindingSource();
    }

    private void resetCycleComboBox_SelectedValueChanged(object sender, System.EventArgs e)
    {
      if (resetCycleComboBox.SelectedItem == null)
        return;
      WorkingBusinessCodeSerialFormat.ResetCycle = (BusinessCodeSerialResetCycle)((EnumKeyCaption)resetCycleComboBox.SelectedItem).Value;
      ResetbusinessCodeSerialFormatBindingSource();
    }
  }
}
