using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Drawing;
using System.Windows.Forms;
using Phenix.Business;
using Phenix.Core;
using Phenix.Core.Mapping;
using Phenix.Core.Windows;

namespace Phenix.Services.Client.Security
{
  /// <summary>
  /// 读写属性授权组件
  /// </summary>
  [Description("控制WinForm界面上控件所绑定数据的读写授权, 涉及其属性: ReadOnly(Properties.ReadOnly)")]
  [Designer(typeof(ReadWriteAuthorizationDesigner))]
  [ProvideProperty("ApplyAuthorization", typeof(Control))] //被应用到授权控制
  [ToolboxItem(true), ToolboxBitmap(typeof(ReadWriteAuthorization), "Phenix.Services.Client.Security.ReadWriteAuthorization")]
  public sealed class ReadWriteAuthorization : Component, IExtenderProvider
  {
    /// <summary>
    /// 初始化
    /// </summary>
    public ReadWriteAuthorization()
      : base() { }

    /// <summary>
    /// 初始化
    /// </summary>
    /// <param name="container">组件容器</param>
    public ReadWriteAuthorization(IContainer container)
      : base()
    {
      if (container == null)
        throw new ArgumentNullException("container");
      container.Add(this);
    }

    #region 属性

    private new bool DesignMode
    {
      get { return base.DesignMode || AppConfig.DesignMode; }
    }

    private Control _host;
    /// <summary>
    /// 所属容器
    /// </summary>
    [DefaultValue(null), Browsable(false)]
    public Control Host
    {
      get
      {
        if (_host == null)
        {
          if (DesignMode)
          {
            IDesignerHost designer = this.GetService(typeof(IDesignerHost)) as IDesignerHost;
            if (designer != null)
              _host = designer.RootComponent as Control;
          }
        }
        return _host;
      }
      set
      {
        if (!DesignMode && _host != null)
          throw new InvalidOperationException("运行期不允许修改Host");
        _host = value;
      }
    }

    private readonly Dictionary<Control, ControlAuthorizationStatus> _authorizationStatuses =
      new Dictionary<Control, ControlAuthorizationStatus>();

    #endregion

    #region 扩展程序属性

    /// <summary>
    /// 被应用到授权控制
    /// </summary>
    [Description("本控件将被应用到授权控制"), Category("Phenix")]
    public bool GetApplyAuthorization(Control source)
    {
      ControlAuthorizationStatus result;
      if (_authorizationStatuses.TryGetValue(source, out result))
        return result.Apply;
      return true;
    }

    /// <summary>
    /// 被应用到授权控制
    /// </summary>
    public void SetApplyAuthorization(Control source, bool value)
    {
      ControlAuthorizationStatus status;
      if (_authorizationStatuses.TryGetValue(source, out status))
        status.Apply = value;
      else
        _authorizationStatuses.Add(source, new ControlAuthorizationStatus { Apply = value });
    }

    #endregion

    #region 方法

    #region IExtenderProvider 成员

    /// <summary>
    /// 是否可以将扩展程序属性提供给指定的对象
    /// </summary>
    /// <param name="extendee">要接收扩展程序属性的对象</param>
    public bool CanExtend(object extendee)
    {
      return extendee is Control;
    }

    #endregion

    /// <summary>
    /// 重置控件的读写授权
    /// </summary>
    /// <param name="editMode">编辑状态</param>
    /// <param name="readOnly">只读</param>
    /// <param name="sources">数据源队列</param>
    public void ResetControlAuthorizationRules(bool editMode, bool readOnly, params BindingSource[] sources)
    {
      if (sources != null && sources.Length == 0)
        return;
      foreach (KeyValuePair<Control, ControlAuthorizationStatus> kvp in _authorizationStatuses)
        if (kvp.Value.Apply)
          if (kvp.Key.InvokeRequired)
            kvp.Key.BeginInvoke(new ExecuteResetControlAuthorizationRuleDelegate(ResetControlAuthorizationRule),
              new object[] { kvp.Key, kvp.Value, editMode, readOnly, sources });
          else
            ResetControlAuthorizationRule(kvp.Key, kvp.Value, editMode, readOnly, sources);
    }

    /// <summary>
    /// 重置控件的读写授权
    /// </summary>
    /// <param name="controls">控件队列</param>
    /// <param name="editMode">编辑状态</param>
    /// <param name="readOnly">只读</param>
    /// <param name="sources">数据源队列</param>
    public static void ResetControlAuthorizationRules(IList<Control> controls, bool editMode, bool readOnly, params BindingSource[] sources)
    {
      if (controls == null)
        return;
      if (sources != null && sources.Length == 0)
        return;
      ControlAuthorizationStatus status = new ControlAuthorizationStatus();
      foreach (Control item in controls)
        if (item.InvokeRequired)
          item.BeginInvoke(new ExecuteResetControlAuthorizationRuleDelegate(ResetControlAuthorizationRule),
            new object[] { item, status, editMode, readOnly, sources });
        else
          ResetControlAuthorizationRule(item, status, editMode, readOnly, sources);
    }

    /// <summary>
    /// 重置控件容器内控件的读写授权
    /// </summary>
    /// <param name="container">控件容器</param>
    /// <param name="editMode">编辑状态</param>
    /// <param name="readOnly">只读</param>
    /// <param name="sources">数据源队列</param>
    public static void ResetControlAuthorizationRules(Control container, bool editMode, bool readOnly, params BindingSource[] sources)
    {
      if (container == null)
        return;
      if (sources != null && sources.Length == 0)
        return;
      if (container.InvokeRequired)
        container.BeginInvoke(new ExecuteResetControlAuthorizationRulesDelegate(ExecuteResetControlAuthorizationRules),
          new object[] { container, editMode, readOnly, sources });
      else
        ExecuteResetControlAuthorizationRules(container, editMode, readOnly, sources);
    }

    private delegate void ExecuteResetControlAuthorizationRulesDelegate(Control container, bool editMode, bool readOnly, params BindingSource[] sources);
    private static void ExecuteResetControlAuthorizationRules(Control container, bool editMode, bool readOnly, params BindingSource[] sources)
    {
      ResetControlAuthorizationRule(container, new ControlAuthorizationStatus(), editMode, readOnly, sources);
      foreach (Control item in container.Controls)
        ResetControlAuthorizationRules(item, editMode, readOnly, sources);
    }

    private delegate void ExecuteResetControlAuthorizationRuleDelegate(Control control, ControlAuthorizationStatus status, bool editMode, bool readOnly, params BindingSource[] sources);
    private static void ResetControlAuthorizationRule(Control control, ControlAuthorizationStatus status, bool editMode, bool readOnly, params BindingSource[] sources)
    {
      List<BindingSource> sourceList = null;
      if (sources != null && sources.Length > 0)
        sourceList = new List<BindingSource>(sources);
      BindingSource bindingSource = BindingSourceHelper.GetDataSource(control);
      if (bindingSource != null)
      {
        if (sourceList != null && !sourceList.Contains(bindingSource))
          return;
        object current = BindingSourceHelper.GetDataSourceCurrent(bindingSource);
        if (current is ICriteria)
          return;
        IBusinessObject business = current as IBusinessObject;
        ApplyGridReadRule(control, business);
        ApplyGridWriteRule(control, business != null ? business.GetType() : BindingSourceHelper.GetDataSourceCoreType(bindingSource), business, editMode, readOnly);
      }
      else
        foreach (Binding item in control.DataBindings)
        {
          if (String.IsNullOrEmpty(item.BindingMemberInfo.BindingField))
            continue;
          bindingSource = item.DataSource as BindingSource;
          if (bindingSource == null)
            continue;
          if (sourceList != null && !sourceList.Contains(bindingSource))
            continue;
          object current = BindingSourceHelper.GetDataSourceCurrent(bindingSource);
          if (current is ICriteria)
            continue;
          FieldConfineType fieldConfineType = FieldConfineType.Normal;
          IBusinessObject business = current as IBusinessObject;
          Type coreType = business != null ? business.GetType() : BindingSourceHelper.GetDataSourceCoreType(bindingSource);
          if (coreType != null)
          {
            fieldConfineType = ClassMemberHelper.GetFieldConfineType(coreType, item.BindingMemberInfo.BindingField);
            if (fieldConfineType == FieldConfineType.Unconfined)
              continue;
          }
          ApplyReadRule(control, item, status, 
            business == null || business.AllowReadProperty(item.BindingMemberInfo.BindingField));
          ApplyWriteRule(control, item,
            (fieldConfineType == FieldConfineType.Selectable && (business == null || !business.InSelectableList || editMode)) ||
            (editMode && !readOnly && (business == null || business.AllowWriteProperty(item.BindingMemberInfo.BindingField))));
          break;
        }
    }

    private static void ApplyReadRule(Control control, Binding binding, ControlAuthorizationStatus status, bool canRead)
    {
      if (canRead)
      {
        if (!status.CanRead)
        {
          binding.Format -= ReturnEmpty;
          binding.ReadValue();
        }
      }
      else
      {
        if (status.CanRead)
          binding.Format += ReturnEmpty;
        System.Reflection.PropertyInfo propertyInfo = control.GetType().GetProperty(binding.PropertyName);
        if (propertyInfo != null)
          propertyInfo.SetValue(control, GetEmptyValue(Csla.Utilities.GetPropertyType(propertyInfo.PropertyType)), null);
      }
      status.CanRead = canRead;
    }

    private static void ApplyWriteRule(Control control, Binding binding, bool canWrite)
    {
      Type controlType = control.GetType();
      System.Reflection.PropertyInfo readOnlyInfo;
      bool couldWrite;
      //for Developer Express .NET
      foreach (System.Reflection.PropertyInfo item in controlType.GetProperties())
        if (!item.PropertyType.IsValueType && String.CompareOrdinal(item.Name, "Properties") == 0)
        {
          readOnlyInfo = item.PropertyType.GetProperty("ReadOnly");
          if (readOnlyInfo != null)
          {
            object itemValue = item.GetValue(control, null);
            couldWrite = !(bool)readOnlyInfo.GetValue(itemValue, null);
            readOnlyInfo.SetValue(itemValue, !canWrite, null);
            if (!couldWrite && canWrite)
              binding.ReadValue();
            control.Refresh();
            return;
          }
        }
      //for VS
      readOnlyInfo = controlType.GetProperty("ReadOnly");
      if (readOnlyInfo != null)
      {
        couldWrite = !(bool)readOnlyInfo.GetValue(control, null);
        readOnlyInfo.SetValue(control, !canWrite, null);
        if (!couldWrite && canWrite)
          binding.ReadValue();
        control.Refresh();
        return;
      }
      //没有匹配的属性
      couldWrite = control.Enabled;
      control.Enabled = canWrite;
      if (!couldWrite && canWrite)
        binding.ReadValue();
      control.Refresh();
    }

    private static void ReturnEmpty(object sender, ConvertEventArgs e)
    {
      e.Value = GetEmptyValue(e.DesiredType);
    }

    private static object GetEmptyValue(Type desiredType)
    {
      if (desiredType.IsValueType)
        return Activator.CreateInstance(desiredType, true);
      else
        return null;
    }
    
    private static void ApplyGridReadRule(Control grid, IBusinessObject business)
    {
      if (business == null)
        return;
      Type gridType = grid.GetType();
      if (ApplyGridReadRuleForFocusedView(grid, gridType))
        return;
      if (ApplyGridReadRuleForRows(grid, business, gridType))
        return;
      if (ApplyGridReadRuleForColumns(grid, business, gridType))
        return;
    }

    private static bool ApplyGridReadRuleForFocusedView(Control grid, Type gridType)
    {
      //for Developer Express .NET
      System.Reflection.PropertyInfo focusedViewInfo = gridType.GetProperty("FocusedView"); //GridControl
      if (focusedViewInfo != null)
      {
        object view = focusedViewInfo.GetValue(grid, null);
        if (view == null)
          return true;
        Type viewType = view.GetType();
        System.Reflection.MethodInfo getRowInfo = viewType.GetMethod("GetRow");
        if (getRowInfo == null)
          return true;
        System.Reflection.PropertyInfo focusedRowHandleInfo = viewType.GetProperty("FocusedRowHandle");
        if (focusedRowHandleInfo == null)
          return true;
        IBusinessObject business = getRowInfo.Invoke(view, new object[] { focusedRowHandleInfo.GetValue(view, null) }) as IBusinessObject;
        if (business == null)
          return true;
        foreach (System.Reflection.PropertyInfo item in viewType.GetProperties())
          if (!item.PropertyType.IsValueType && String.CompareOrdinal(item.Name, "Columns") == 0)
          {
            foreach (object column in (IList)item.GetValue(view, null))
            {
              Type columnType = column.GetType();
              System.Reflection.PropertyInfo fieldNameInfo = columnType.GetProperty("FieldName");
              if (fieldNameInfo != null)
              {
                string fieldNameValue = fieldNameInfo.GetValue(column, null) as string;
                if (!String.IsNullOrEmpty(fieldNameValue) && !business.AllowReadProperty(fieldNameValue))
                {
                  System.Reflection.PropertyInfo visibleInfo = columnType.GetProperty("Visible");
                  if (visibleInfo != null)
                    visibleInfo.SetValue(column, false, null);
                }
              }
            }
            break;
          }
        grid.Refresh();
        return true;
      }
      return false;
    }

    private static bool ApplyGridReadRuleForRows(Control grid, IBusinessObject business, Type gridType)
    {
      //for Developer Express .NET
      System.Reflection.PropertyInfo rowsInfo = gridType.GetProperty("Rows"); //VGridControl
      if (rowsInfo != null)
      {
        IList rows = rowsInfo.GetValue(grid, null) as IList;
        if (rows == null)
          return true;
        foreach (object row in rows)
        {
          Type rowType = row.GetType();
          foreach (System.Reflection.PropertyInfo item in rowType.GetProperties())
            if (!item.PropertyType.IsValueType && String.CompareOrdinal(item.Name, "Properties") == 0)
            {
              object itemValue = item.GetValue(row, null);
              if (itemValue == null)
                return true;
              Type itemType = itemValue.GetType();
              System.Reflection.PropertyInfo fieldNameInfo = itemType.GetProperty("FieldName");
              if (fieldNameInfo != null)
              {
                string fieldNameValue = fieldNameInfo.GetValue(itemValue, null) as string;
                if (!String.IsNullOrEmpty(fieldNameValue) && !business.AllowReadProperty(fieldNameValue))
                {
                  System.Reflection.PropertyInfo visibleInfo = rowType.GetProperty("Visible");
                  if (visibleInfo != null)
                    visibleInfo.SetValue(row, false, null);
                }
                break;
              }
            }
        }
        grid.Refresh();
        return true;
      }
      return false;
    }

    private static bool ApplyGridReadRuleForColumns(Control grid, IBusinessObject business, Type gridType)
    {
      System.Reflection.PropertyInfo columnsInfo = gridType.GetProperty("Columns");
      if (columnsInfo != null)
      {
        IList columns = columnsInfo.GetValue(grid, null) as IList;
        if (columns == null)
          return true;
        foreach (object column in columns)
        {
          Type columnType = column.GetType();
          //for Developer Express .NET
          System.Reflection.PropertyInfo fieldNameInfo = columnType.GetProperty("FieldName");
          if (fieldNameInfo != null)
          {
            string fieldNameValue = fieldNameInfo.GetValue(column, null) as string;
            if (!String.IsNullOrEmpty(fieldNameValue) && !business.AllowReadProperty(fieldNameValue))
            {
              System.Reflection.PropertyInfo visibleInfo = columnType.GetProperty("Visible");
              if (visibleInfo != null)
                visibleInfo.SetValue(column, false, null);
            }
            continue;
          }
          //for VS
          System.Reflection.PropertyInfo dataPropertyNameInfo = columnType.GetProperty("DataPropertyName");
          if (dataPropertyNameInfo != null)
          {
            string dataPropertyNameValue = dataPropertyNameInfo.GetValue(column, null) as string;
            if (!String.IsNullOrEmpty(dataPropertyNameValue) && !business.AllowReadProperty(dataPropertyNameValue))
            {
              System.Reflection.PropertyInfo visibleInfo = columnType.GetProperty("Visible");
              if (visibleInfo != null)
                visibleInfo.SetValue(column, false, null);
            }
            continue;
          }
        }
        grid.Refresh();
        return true;
      }
      return false;
    }

    private static void ApplyGridWriteRule(Control grid, Type sourceType, IBusinessObject business, bool editMode, bool readOnly)
    {
      if (ApplyGridWriteRuleForFocusedView(grid, sourceType, editMode, readOnly))
        return;
      if (ApplyGridWriteRuleForRows(grid, sourceType, business, editMode, readOnly))
        return;
      if (ApplyGridWriteRuleForColumns(grid, sourceType, business, editMode, readOnly))
        return;
    }

    private static bool ApplyGridWriteRuleForFocusedView(Control grid, Type sourceType, bool editMode, bool readOnly)
    {
      //for Developer Express .NET
      System.Reflection.PropertyInfo focusedViewInfo = grid.GetType().GetProperty("FocusedView"); //GridControl
      if (focusedViewInfo != null)
      {
        object view = focusedViewInfo.GetValue(grid, null);
        if (view == null)
          return true;
        Type viewType = view.GetType();
        System.Reflection.MethodInfo getRowInfo = viewType.GetMethod("GetRow");
        if (getRowInfo == null)
          return true;
        System.Reflection.PropertyInfo focusedRowHandleInfo = viewType.GetProperty("FocusedRowHandle");
        if (focusedRowHandleInfo == null)
          return true;
        IBusinessObject business = getRowInfo.Invoke(view, new object[] { focusedRowHandleInfo.GetValue(view, null) }) as IBusinessObject;
        if (business != null)
          sourceType = business.GetType();
        foreach (System.Reflection.PropertyInfo item in viewType.GetProperties())
          if (!item.PropertyType.IsValueType && String.CompareOrdinal(item.Name, "Columns") == 0)
          {
            foreach (object column in (IList)item.GetValue(view, null))
            {
              Type columnType = column.GetType();
              System.Reflection.PropertyInfo fieldNameInfo = columnType.GetProperty("FieldName");
              if (fieldNameInfo != null)
              {
                string fieldNameValue = fieldNameInfo.GetValue(column, null) as string;
                if (!String.IsNullOrEmpty(fieldNameValue))
                {
                  FieldConfineType fieldConfineType = FieldConfineType.Normal;
                  if (sourceType != null)
                  {
                    fieldConfineType = ClassMemberHelper.GetFieldConfineType(sourceType, fieldNameValue);
                    if (fieldConfineType == FieldConfineType.Unconfined)
                      continue;
                  }
                  System.Reflection.PropertyInfo optionsColumnInfo = columnType.GetProperty("OptionsColumn");
                  if (optionsColumnInfo != null)
                  {
                    object optionsColumnValue = optionsColumnInfo.GetValue(column, null);
                    if (optionsColumnValue != null)
                      optionsColumnValue.GetType().GetProperty("ReadOnly").SetValue(optionsColumnValue,
                        !((fieldConfineType == FieldConfineType.Selectable && (business == null || !business.InSelectableList || editMode)) ||
                        (editMode && !readOnly && (business == null || business.AllowWriteProperty(fieldNameValue)))), null);
                  }
                }
              }
            }
            break;
          }
        grid.Refresh();
        return true;
      }
      return false;
    }

    private static bool ApplyGridWriteRuleForRows(Control grid, Type sourceType, IBusinessObject business, bool editMode, bool readOnly)
    {
      //for Developer Express .NET
      System.Reflection.PropertyInfo rowsInfo = grid.GetType().GetProperty("Rows"); //VGridControl
      if (rowsInfo != null)
      {
        IList rows = rowsInfo.GetValue(grid, null) as IList;
        if (rows == null)
          return true;
        foreach (object row in rows)
        {
          Type rowType = row.GetType();
          foreach (System.Reflection.PropertyInfo item in rowType.GetProperties())
            if (!item.PropertyType.IsValueType && String.CompareOrdinal(item.Name, "Properties") == 0)
            {
              object itemValue = item.GetValue(row, null);
              if (itemValue == null)
                return true;
              Type itemType = itemValue.GetType();
              System.Reflection.PropertyInfo fieldNameInfo = itemType.GetProperty("FieldName");
              if (fieldNameInfo != null)
              {
                string fieldNameValue = fieldNameInfo.GetValue(itemValue, null) as string;
                if (!String.IsNullOrEmpty(fieldNameValue))
                {
                  FieldConfineType fieldConfineType = FieldConfineType.Normal;
                  if (sourceType != null)
                  {
                    fieldConfineType = ClassMemberHelper.GetFieldConfineType(sourceType, fieldNameValue);
                    if (fieldConfineType == FieldConfineType.Unconfined)
                      continue;
                  }
                  System.Reflection.PropertyInfo readOnlyInfo = itemType.GetProperty("ReadOnly");
                  if (readOnlyInfo != null)
                    readOnlyInfo.SetValue(itemValue,
                    !((fieldConfineType == FieldConfineType.Selectable && (business == null || !business.InSelectableList || editMode)) ||
                    (editMode && !readOnly && (business == null || business.AllowWriteProperty(fieldNameValue)))), null);
                }
                break;
              }
            }
        }
        grid.Refresh();
        return true;
      }
      return false;
    }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
    private static bool ApplyGridWriteRuleForColumns(Control grid, Type sourceType, IBusinessObject business, bool editMode, bool readOnly)
    {
      System.Reflection.PropertyInfo columnsInfo = grid.GetType().GetProperty("Columns");
      if (columnsInfo != null)
      {
        IList columns = columnsInfo.GetValue(grid, null) as IList;
        if (columns == null)
          return true;
        foreach (object column in columns)
        {
          Type columnType = column.GetType();
          //for Developer Express .NET
          System.Reflection.PropertyInfo fieldNameInfo = columnType.GetProperty("FieldName");
          if (fieldNameInfo != null)
          {
            string fieldNameValue = fieldNameInfo.GetValue(column, null) as string;
            if (!String.IsNullOrEmpty(fieldNameValue))
            {
              FieldConfineType fieldConfineType = FieldConfineType.Normal;
              if (sourceType != null)
              {
                fieldConfineType = ClassMemberHelper.GetFieldConfineType(sourceType, fieldNameValue);
                if (fieldConfineType == FieldConfineType.Unconfined)
                  continue;
              }
              System.Reflection.PropertyInfo optionsColumnInfo = columnType.GetProperty("OptionsColumn");
              if (optionsColumnInfo != null)
              {
                object optionsColumnValue = optionsColumnInfo.GetValue(column, null);
                if (optionsColumnValue != null)
                  optionsColumnValue.GetType().GetProperty("ReadOnly").SetValue(optionsColumnValue,
                    !((fieldConfineType == FieldConfineType.Selectable && (business == null || !business.InSelectableList || editMode)) ||
                    (editMode && !readOnly && (business == null || business.AllowWriteProperty(fieldNameValue)))), null);
              }
            }
            continue;
          }
          //for VS
          System.Reflection.PropertyInfo dataPropertyNameInfo = columnType.GetProperty("DataPropertyName");
          if (dataPropertyNameInfo != null)
          {
            string dataPropertyNameValue = dataPropertyNameInfo.GetValue(column, null) as string;
            if (!String.IsNullOrEmpty(dataPropertyNameValue))
            {
              FieldConfineType fieldConfineType = FieldConfineType.Normal;
              if (sourceType != null)
              {
                fieldConfineType = ClassMemberHelper.GetFieldConfineType(sourceType, dataPropertyNameValue);
                if (fieldConfineType == FieldConfineType.Unconfined)
                  continue;
              }
              System.Reflection.PropertyInfo readOnlyInfo = columnType.GetProperty("ReadOnly");
              if (readOnlyInfo != null)
                readOnlyInfo.SetValue(column,
                  !((fieldConfineType == FieldConfineType.Selectable && (business == null || !business.InSelectableList || editMode)) ||
                  (editMode && !readOnly && (business == null || business.AllowWriteProperty(dataPropertyNameValue)))), null);
            }
            continue;
          }
        }
        grid.Refresh();
        return true;
      }
      return false;
    }

    private static string FindName(Component component)
    {
      Type type = component.GetType();
      System.Reflection.PropertyInfo nameInfo = type.GetProperty("Name");
      if (nameInfo != null)
        return nameInfo.GetValue(component, null) as string;
      return String.Empty;
    }

    internal string RuleMessage()
    {
      string result = String.Empty;
      foreach (KeyValuePair<Control, ControlAuthorizationStatus> kvp in _authorizationStatuses)
        if (kvp.Value.Apply)
        {
          result += FindName(kvp.Key) + ":" + Environment.NewLine;
          result += kvp.Value.ToString() + Environment.NewLine + Environment.NewLine;
        }
      return result;
    }

    #endregion

    #region 内嵌类

    [Serializable]
    private class ControlAuthorizationStatus
    {
      private bool _apply = true;
      public bool Apply
      {
        get { return _apply; }
        set { _apply = value; }
      }

      private bool _canRead = true;
      public bool CanRead
      {
        get { return _canRead; }
        set { _canRead = value; }
      }

      public override string ToString()
      {
        return "Apply Authorization && CanRead" + AppConfig.EQUAL_SEPARATOR + CanRead.ToString();
      }
    }

    #endregion
  }
}