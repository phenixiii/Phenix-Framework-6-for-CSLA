using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Phenix.Core.Mapping;
using Phenix.Core.Rule;

namespace Phenix.Windows
{
  [ToolboxItem(false)]
  internal partial class CriteriaCombineItemControl : UserControl
  {
    public CriteriaCombineItemControl(CriteriaExpression criteriaExpression,
      CriteriaExpressionPropertyKeyCaptionCollection selectedProperties, FieldMapInfo leftNodeFieldMapInfo)
      : base()
    {
      InitializeComponent();

      _criteriaExpression = criteriaExpression;

      this.criteriaExpressionPropertyKeyCaptionCollectionBindingSource.DataSource = selectedProperties;
      this.leftNodeFieldMapInfoLookUpEdit.EditValue = leftNodeFieldMapInfo;
    }

    #region 属性

    private readonly CriteriaExpression _criteriaExpression;
    private Control _valueControl;

    #endregion

    #region 方法

    private void ResetComponent()
    {
      if (_valueControl != null)
      {
        this.splitContainerControl.Panel2.Controls.Remove(_valueControl);
        _valueControl.Dispose();
        _valueControl = null;
      }
      FieldMapInfo fieldMapInfo = this.leftNodeFieldMapInfoLookUpEdit.EditValue as FieldMapInfo;
      if (fieldMapInfo == null || String.CompareOrdinal(_criteriaExpression.LeftNode.PropertyName, fieldMapInfo.PropertyName) != 0)
        _criteriaExpression.Reset(fieldMapInfo);
      if (fieldMapInfo != null)
      {
        Type type = fieldMapInfo.FieldCoreUnderlyingType;
        if (type == typeof(Enum))
          _valueControl = new CriteriaCombineItemEnumValueControl(_criteriaExpression, type);
        else if (type == typeof(bool))
          _valueControl = new CriteriaCombineItemBooleanValueControl(_criteriaExpression);
        else if (type == typeof(DateTime))
          _valueControl = new CriteriaCombineItemDateTimeValueControl(_criteriaExpression);
        else if (type == typeof(Color))
          _valueControl = new CriteriaCombineItemColorValueControl(_criteriaExpression);
        else
          _valueControl = new CriteriaCombineItemValueControl(_criteriaExpression);
        _valueControl.Dock = DockStyle.Fill;
        this.splitContainerControl.Panel2.Controls.Add(_valueControl);
      }
    }

    #endregion

    private void leftNodeFieldMapInfoLookUpEdit_EditValueChanged(object sender, System.EventArgs e)
    {
      ResetComponent();
    }
  }
}
