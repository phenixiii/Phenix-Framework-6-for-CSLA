using System;
using System.Windows.Forms;
using Phenix.Core.Mapping;
using Phenix.Core.Operate;
using Phenix.Core.Windows;

namespace Phenix.Core.Dictionary.Windows
{
  /// <summary>
  /// 添加业务码占位符格式
  /// </summary>
  public partial class BusinessCodePlaceholderFormatAddDialog : DialogForm
  {
    private BusinessCodePlaceholderFormatAddDialog(Type ownerType)
    {
      InitializeComponent();

      this.formatStringLabel.Text = Resources.FormatString;
      this.lengthLabel.Text = Resources.Length;
      this.propertyNameLabel.Text = Resources.PropertyName;
      this.okButton.Text = Phenix.Core.Properties.Resources.Ok;
      this.cancelButton.Text = Phenix.Core.Properties.Resources.Cancel;

      this.propertyNameTextBox.Visible = ownerType == null;
      this.propertyNameComboBox.Visible = ownerType != null;

      this.businessCodePlaceholderFormatBindingSource.DataSource = new BusinessCodePlaceholderFormat();

      KeyCaptionCollection propertySelectNames = new KeyCaptionCollection(typeof(FieldMapInfo));
      if (ownerType != null)
        foreach (FieldMapInfo item in ClassMemberHelper.DoGetFieldMapInfos(ownerType))
          propertySelectNames.Add(new KeyCaption(item.PropertyName, item.FriendlyName, item));
      this.propertySelectNamesBindingSource.DataSource = propertySelectNames;
    }

    #region 工厂

    /// <summary>
    /// 执行
    /// </summary>
    public static string Execute(BusinessCodeFormat businessCodeFormat, Type ownerType)
    {
      using (BusinessCodePlaceholderFormatAddDialog dialog = new BusinessCodePlaceholderFormatAddDialog(ownerType))
      {
        dialog.Text = string.Format(Resources.BusinessCodePlaceholderFormatAdd, businessCodeFormat.Caption, businessCodeFormat.BusinessCodeName);
        return dialog.ShowDialog() == DialogResult.OK ? dialog.WorkingBusinessCodePlaceholderFormat.FormatString : null;
      }
    }

    #endregion

    #region 属性

    private BusinessCodePlaceholderFormat WorkingBusinessCodePlaceholderFormat
    {
      get { return BindingSourceHelper.GetDataSourceCurrent(this.businessCodePlaceholderFormatBindingSource) as BusinessCodePlaceholderFormat; }
    }

    #endregion
  }
}
