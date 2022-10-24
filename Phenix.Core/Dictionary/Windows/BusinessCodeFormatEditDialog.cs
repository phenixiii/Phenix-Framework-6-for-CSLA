using System;
using System.Drawing;
using System.Windows.Forms;
using Phenix.Core.Mapping;
using Phenix.Core.Operate;
using Phenix.Core.Rule;
using Phenix.Core.Windows;

namespace Phenix.Core.Dictionary.Windows
{
  internal partial class BusinessCodeFormatEditDialog : DialogForm
  {
    private BusinessCodeFormatEditDialog(BusinessCodeFormat businessCodeFormat, Type ownerType, IKeyCaptionCollection criteriaPropertyValues,
      Action newBusinessCodeFormatAction)
    {
      InitializeComponent();

      this.formatStringRichTextBox.Focus();

      this.fillOnSavingCheckBox.Text = Resources.FillOnSaving;
      this.addBusinessCodeSerialFormatButton.Text = Resources.Serial;
      this.addBusinessCodePlaceholderFormatButton.Text = Resources.Placeholder;
      this.lengthYearButton.Text = Resources.LengthYear;
      this.shortYearButton.Text = Resources.ShortYear;
      this.monthButton.Text = Resources.Month;
      this.dayButton.Text = Resources.Day;
      this.departmentButton.Text = Resources.Department;
      this.userNumberButton.Text = Resources.UserNumber;
      this.criteriaPropertyInfoGroupBox.Text = Resources.CriteriaPropertyInfo;
      this.criteriaPropertyInfoSelectGroupBox.Text = Resources.CriteriaPropertyInfo;
      this.okButton.Text = Phenix.Core.Properties.Resources.Ok;
      this.cancelButton.Text = Phenix.Core.Properties.Resources.Cancel;

      _ownerType = ownerType;
      _needEditCriteriaPropertyValue = !businessCodeFormat.IsDefault && !String.IsNullOrEmpty(businessCodeFormat.CriteriaPropertyValue);
      _newBusinessCodeFormatAction = newBusinessCodeFormatAction;

      this.businessCodeFormatBindingSource.DataSource = businessCodeFormat;

      this.criteriaPropertyInfoGroupBox.Visible = _needEditCriteriaPropertyValue && (criteriaPropertyValues == null || criteriaPropertyValues.Count == 0);
      this.criteriaPropertyInfoSelectGroupBox.Visible = _needEditCriteriaPropertyValue && (criteriaPropertyValues != null && criteriaPropertyValues.Count > 0);
      this.addButton.Visible = !businessCodeFormat.IsDefault;

      IPropertyInfo propertyInfo = ownerType != null ? ClassMemberHelper.GetPropertyInfo(ownerType, businessCodeFormat.CriteriaPropertyName) : null;
      this.criteriaPropertyFriendlyNameLabel.Text = propertyInfo != null ? propertyInfo.FriendlyName : businessCodeFormat.CriteriaPropertyName;
      this.criteriaPropertySelectValuesBindingSource.DataSource = criteriaPropertyValues;
    }

    #region 工厂

    public static BusinessCodeFormat Execute(BusinessCodeFormat businessCodeFormat)
    {
      using (BusinessCodeFormatEditDialog dialog = new BusinessCodeFormatEditDialog(businessCodeFormat, businessCodeFormat.OwnerType, null, null))
      {
        dialog.Text = String.Format(Resources.BusinessCodeFormatEdit, businessCodeFormat.Caption, businessCodeFormat.BusinessCodeName);
        return dialog.ShowDialog() == DialogResult.OK ? dialog.WorkingBusinessCodeFormat : null;
      }
    }

    public static BusinessCodeFormat Execute(BusinessCodeFormat businessCodeFormat, Action newBusinessCodeFormatAction)
    {
      using (BusinessCodeFormatEditDialog dialog = new BusinessCodeFormatEditDialog(businessCodeFormat, businessCodeFormat.OwnerType, null, newBusinessCodeFormatAction))
      {
        dialog.Text = String.Format(Resources.BusinessCodeFormatEdit, businessCodeFormat.Caption, businessCodeFormat.BusinessCodeName);
        return dialog.ShowDialog() == DialogResult.OK ? dialog.WorkingBusinessCodeFormat : null;
      }
    }

    public static BusinessCodeFormat Execute(BusinessCodeFormat businessCodeFormat, KeyCaptionCollection criteriaPropertySelectValues)
    {
      using (BusinessCodeFormatEditDialog dialog = new BusinessCodeFormatEditDialog(businessCodeFormat, businessCodeFormat.OwnerType, criteriaPropertySelectValues, null))
      {
        dialog.Text = String.Format(Resources.BusinessCodeFormatEdit, businessCodeFormat.Caption, businessCodeFormat.BusinessCodeName);
        return dialog.ShowDialog() == DialogResult.OK ? dialog.WorkingBusinessCodeFormat : null;
      }
    }

    public static BusinessCodeFormat Execute(BusinessCodeFormat businessCodeFormat, EnumKeyCaptionCollection criteriaPropertySelectValues)
    {
      using (BusinessCodeFormatEditDialog dialog = new BusinessCodeFormatEditDialog(businessCodeFormat, businessCodeFormat.OwnerType, criteriaPropertySelectValues, null))
      {
        dialog.Text = String.Format(Resources.BusinessCodeFormatEdit, businessCodeFormat.Caption, businessCodeFormat.BusinessCodeName);
        return dialog.ShowDialog() == DialogResult.OK ? dialog.WorkingBusinessCodeFormat : null;
      }
    }

    public static BusinessCodeFormat Execute(BusinessCodeFormat businessCodeFormat, Type ownerType)
    {
      using (BusinessCodeFormatEditDialog dialog = new BusinessCodeFormatEditDialog(businessCodeFormat, ownerType, null, null))
      {
        dialog.Text = String.Format(Resources.BusinessCodeFormatEdit, businessCodeFormat.Caption, businessCodeFormat.BusinessCodeName);
        return dialog.ShowDialog() == DialogResult.OK ? dialog.WorkingBusinessCodeFormat : null;
      }
    }

    public static BusinessCodeFormat Execute(BusinessCodeFormat businessCodeFormat, Type ownerType, KeyCaptionCollection criteriaPropertySelectValues)
    {
      using (BusinessCodeFormatEditDialog dialog = new BusinessCodeFormatEditDialog(businessCodeFormat, ownerType, criteriaPropertySelectValues, null))
      {
        dialog.Text = String.Format(Resources.BusinessCodeFormatEdit, businessCodeFormat.Caption, businessCodeFormat.BusinessCodeName);
        return dialog.ShowDialog() == DialogResult.OK ? dialog.WorkingBusinessCodeFormat : null;
      }
    }

    public static BusinessCodeFormat Execute(BusinessCodeFormat businessCodeFormat, Type ownerType, EnumKeyCaptionCollection criteriaPropertySelectValues)
    {
      using (BusinessCodeFormatEditDialog dialog = new BusinessCodeFormatEditDialog(businessCodeFormat, ownerType, criteriaPropertySelectValues, null))
      {
        dialog.Text = String.Format(Resources.BusinessCodeFormatEdit, businessCodeFormat.Caption, businessCodeFormat.BusinessCodeName);
        return dialog.ShowDialog() == DialogResult.OK ? dialog.WorkingBusinessCodeFormat : null;
      }
    }

    #endregion

    #region 属性

    private readonly Type _ownerType;

    private readonly bool _needEditCriteriaPropertyValue;
    
    private readonly Action _newBusinessCodeFormatAction;

    private BusinessCodeFormat WorkingBusinessCodeFormat
    {
      get { return BindingSourceHelper.GetDataSourceCurrent(this.businessCodeFormatBindingSource) as BusinessCodeFormat; }
    }
    
    #endregion

    #region 方法

    private void ApplyRules()
    {
      this.okButton.Enabled = !String.IsNullOrEmpty(WorkingBusinessCodeFormat.FormatString) &&
        (!_needEditCriteriaPropertyValue || !String.IsNullOrEmpty(WorkingBusinessCodeFormat.CriteriaPropertyValue));
    }

    private void InsertText(string text)
    {
      Color oldColor = this.formatStringRichTextBox.SelectionColor;
      try
      {
        this.formatStringRichTextBox.SelectionColor = Color.Blue;
        this.formatStringRichTextBox.SelectedText = text;
      }
      finally
      {
        this.formatStringRichTextBox.SelectionColor = oldColor;
      }
      this.formatStringRichTextBox.Focus();
    }

    private void AddBusinessCodeSerialFormat()
    {
      string formatString = BusinessCodeSerialFormatAddDialog.Execute(WorkingBusinessCodeFormat);
      if (formatString != null)
        InsertText(formatString);
    }

    private void AddBusinessCodePlaceholderFormat()
    {
      string formatString = BusinessCodePlaceholderFormatAddDialog.Execute(WorkingBusinessCodeFormat, _ownerType);
      if (formatString != null)
        InsertText(formatString);
    }

    private void AddBusinessCodeFormat(BusinessCodeFormatItemType businessCodeFormatItemType)
    {
      InsertText(EnumKeyCaption.Fetch(businessCodeFormatItemType).Key);
    }

    #endregion

    private void formatStringRichTextBox_TextChanged(object sender, EventArgs e)
    {
      ApplyRules();
    }

    private void addBusinessCodeSerialFormatButton_Click(object sender, EventArgs e)
    {
      AddBusinessCodeSerialFormat();
    }
    
    private void AddBusinessCodePlaceholderFormatButton_Click(object sender, EventArgs e)
    {
      AddBusinessCodePlaceholderFormat();
    }

    private void lengthYearButton_Click(object sender, EventArgs e)
    {
      AddBusinessCodeFormat(BusinessCodeFormatItemType.LengthYear);
    }

    private void shortYearButton_Click(object sender, EventArgs e)
    {
      AddBusinessCodeFormat(BusinessCodeFormatItemType.ShortYear);
    }

    private void monthButton_Click(object sender, EventArgs e)
    {
      AddBusinessCodeFormat(BusinessCodeFormatItemType.Month);
    }

    private void dayButton_Click(object sender, EventArgs e)
    {
      AddBusinessCodeFormat(BusinessCodeFormatItemType.Day);
    }

    private void departmentButton_Click(object sender, EventArgs e)
    {
      AddBusinessCodeFormat(BusinessCodeFormatItemType.Department);
    }

    private void userNumberButton_Click(object sender, EventArgs e)
    {
      AddBusinessCodeFormat(BusinessCodeFormatItemType.UserNumber);
    }

    private void criteriaPropertyValueTextBox_TextChanged(object sender, EventArgs e)
    {
      ApplyRules();
    }

    private void criteriaPropertyValueComboBox_TextChanged(object sender, EventArgs e)
    {
      ApplyRules();
    }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
    private void addButton_Click(object sender, EventArgs e)
    {
      if (_newBusinessCodeFormatAction != null)
        _newBusinessCodeFormatAction();
      else
      {
        BusinessCodeFormat value = (BusinessCodeFormat)Phenix.Core.Reflection.Utilities.Clone(WorkingBusinessCodeFormat);
        value.CriteriaPropertyValue = Resources.InputCriteriaPropertyValue;
        value = Execute(value);
        if (value != null)
          try
          {
            value.Save();
          }
          catch (Exception ex)
          {
            MessageBox.Show(AppUtilities.GetErrorHint(ex), this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
          }
      }
    }
  }
}
