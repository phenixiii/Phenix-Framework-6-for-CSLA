using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.Design;
using System.Drawing;
using System.Windows.Forms;
using Phenix.Business;
using Phenix.Core;
using Phenix.Core.Mapping;
using Phenix.Core.Reflection;
using Phenix.Core.Windows;
using Phenix.Services.Client.Library;

namespace Phenix.Services.Client.Validation
{
  /// <summary>
  /// 有效性校验组件
  /// </summary>
  [Description("辅助设置WinForm界面上绑定数据控件的一些有效性验证属性值，自动管理关联标签控件的友好性显示内容")]
  [Designer(typeof(EditValidationDesigner))]
  [ProvideProperty("FriendlyCaption", typeof(Control))] //友好性标签
  [ToolboxItem(true), ToolboxBitmap(typeof(EditValidation), "Phenix.Services.Client.Validation.EditValidation")]
  public sealed class EditValidation : Component, IExtenderProvider, ISupportInitialize
  {
    /// <summary>
    /// 初始化
    /// </summary>
    public EditValidation()
      : base() { }

    /// <summary>
    /// 初始化
    /// </summary>
    /// <param name="container">组件容器</param>
    public EditValidation(IContainer container)
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
        if (!DesignMode)
        {
          Form form = value as Form;
          if (form != null)
            form.Shown += new EventHandler(Host_Shown);
        }
      }
    }

    private static Color? _requiredCaptionForeColor;
    /// <summary>
    /// 统一必输栏的关联标签控件前景色
    /// 当为 Color.Empty 时禁用本功能
    /// </summary>
    public static Color RequiredCaptionForeColor
    {
      get { return AppSettings.GetProperty(ref _requiredCaptionForeColor, Color.Blue); }
      set { AppSettings.SetProperty(ref _requiredCaptionForeColor, value); }
    }

    private static bool? _requiredCaptionForeColorDisabled;
    /// <summary>
    /// 禁用"统一必输栏的关联标签控件前景色"功能?
    /// </summary>
    public static bool RequiredCaptionForeColorDisabled
    {
      get
      {
        if (RequiredCaptionForeColor.ToArgb() == Color.Empty.ToArgb())
          return true;
        return AppSettings.GetProperty(ref _requiredCaptionForeColorDisabled, false);
      }
      set { AppSettings.SetProperty(ref _requiredCaptionForeColorDisabled, value); }
    }

    ///// <summary>
    ///// 统一必输栏的关联标签控件前景色
    ///// </summary>
    //public void ResetRequiredCaptionForeColor()
    //{
    //  RequiredCaptionForeColor = Color.Blue;
    //}
    ///// <summary>
    ///// 统一必输栏的关联标签控件前景色
    ///// </summary>
    //public bool ShouldSerializeRequiredCaptionForeColor()
    //{
    //  return RequiredCaptionForeColor != Color.Blue;
    //}

    private static bool? _needResetFriendlyCaption;
    /// <summary>
    /// 是否在运行期重置标签的友好名?
    /// 缺省为 false
    /// </summary>
    public static bool NeedResetFriendlyCaption
    {
      get { return AppSettings.GetProperty(ref _needResetFriendlyCaption, false); }
      set { AppSettings.SetProperty(ref _needResetFriendlyCaption, value); }
    }

    private readonly Dictionary<Control, Component> _friendlyCaptions = new Dictionary<Control, Component>();

    #endregion

    #region 扩展程序属性

    /// <summary>
    /// 友好性标签
    /// </summary>
    [DefaultValue(null), Description("本控件的友好性标签/n将被: 统一关联标签控件的标题、必输项前景色"), Category("Phenix")]
    public Component GetFriendlyCaption(Control source)
    {
      Component result;
      if (!_friendlyCaptions.TryGetValue(source, out result) && DesignMode)
      {
        result = MateFriendlyCaption(source);
        if (result != null)
          _friendlyCaptions[source] = result;
      }
      return result;
    }

    /// <summary>
    /// 友好性标签
    /// </summary>
    public void SetFriendlyCaption(Control source, Component value)
    {
      _friendlyCaptions[source] = value;
    }

    #endregion

    #region 事件

    private void Host_Shown(object sender, EventArgs e)
    {
      ResetEditValidationRules();
      if (NeedResetFriendlyCaption)
      {
        ResetEditFriendlyCaptions();
        ResetGridFriendlyCaptions();
      }
    }

    private static void LookupEdit_KeyDown(object sender, KeyEventArgs e)
    {
      if ((e.KeyCode == Keys.Delete && e.Control) ||
        (e.KeyCode == Keys.Back))
        if (!IsReadOnly((Control)sender))
          ControlHelper.SetEditControlValue((Control)sender, null);
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

    #region ISupportInitialize 成员

    ///<summary>
    /// 开始初始化
    ///</summary>
    public void BeginInit()
    {
    }

    ///<summary>
    /// 结束初始化
    ///</summary>
    public void EndInit()
    {
      if (!DesignMode && !(Host is Form))
        Host_Shown(null, null);
    }

    #endregion

    #region FetchValidation、EditValidation

    private static void ApplyLookupEditKeyDownDelegate(Control lookupEdit)
    {
      if (lookupEdit == null)
        return;
      Type lookupEditType = lookupEdit.GetType();
      //for Developer Express .NET
      foreach (System.Reflection.PropertyInfo item in lookupEditType.GetProperties())
        if (!item.PropertyType.IsValueType && 
          (String.CompareOrdinal(item.Name, "Properties") == 0 || String.CompareOrdinal(item.Name, "ColumnEdit") == 0))
        {
          object itemValue = item.GetValue(lookupEdit, null);
          if (itemValue == null)
            return;
          Type itemType = itemValue.GetType();
          System.Reflection.PropertyInfo dataSourceInfo = itemType.GetProperty("DataSource");
          if (dataSourceInfo != null)
          {
            System.Reflection.PropertyInfo immediatePopupInfo = itemType.GetProperty("ImmediatePopup");
            if (immediatePopupInfo == null || (bool)immediatePopupInfo.GetValue(itemValue, null)) 
            {
              lookupEdit.KeyDown -= new KeyEventHandler(LookupEdit_KeyDown);
              lookupEdit.KeyDown += new KeyEventHandler(LookupEdit_KeyDown);
            }
            return;
          }
        }
    }

    /// <summary>
    /// 匹配友好性标签并填充友好名
    /// </summary>
    /// <param name="control">控件</param>
    public static Component MateFriendlyCaption(Control control)
    {
      if (control == null)
        return null;
      Component result = null;
      try
      {
        //for Developer Express .NET
        Type controlType = control.GetType();
        foreach (System.Reflection.PropertyInfo item in controlType.GetProperties())
          if (!item.PropertyType.IsValueType && String.CompareOrdinal(item.Name, "Properties") == 0)
          {
            object itemValue = item.GetValue(control, null);
            if (itemValue == null)
              continue;
            System.Reflection.PropertyInfo captionInfo = itemValue.GetType().GetProperty("Caption");
            if (captionInfo != null)
            {
              result = control;
              return result;
            }
          }
          else if (String.CompareOrdinal(item.Name, "StyleController") == 0)
          {
            object itemValue = item.GetValue(control, null);
            if (itemValue == null)
              continue;
            System.Reflection.PropertyInfo rootInfo = itemValue.GetType().GetProperty("Root");
            if (rootInfo == null)
              return null;
            result = FindLayoutControlItem(control, rootInfo.GetValue(itemValue, null) as Component);
            return result;
          }
        result = FindLayoutControlItem(control, control.Parent);
        if (result != null)
          return result;
        //for VS
        if (control is ButtonBase)
        {
          result = control;
          return result;
        }
        result = ControlHelper.FindFriendlyCaption(control);
        return result;
      }
      finally
      {
        if (result != null)
        {
          Registration.RegisterEmbeddedWorker(false);
          ResetEditFriendlyCaption(control, result, null);
        }
      }
    }

    private static Component FindLayoutControlItem(Control control, Component root)
    {
      if (root == null)
        return null;
      System.Reflection.PropertyInfo itemsInfo = root.GetType().GetProperty("Items");
      if (itemsInfo == null)
        return null;
      foreach (Component item in (IEnumerable)itemsInfo.GetValue(root, null))
      {
        Component result = FindLayoutControlItem(control, item);
        if (result != null)
          return result;
        System.Reflection.PropertyInfo controlInfo = item.GetType().GetProperty("Control");
        if (controlInfo != null && controlInfo.GetValue(item, null) == control)
          return item;
      }
      return null;
    }

    /// <summary>
    /// 重置Edit有效性校验
    /// </summary>
    public void ResetEditValidationRules()
    {
      ResetEditValidationRules(Host, null);
    }

    /// <summary>
    /// 重置控件容器内Edit有效性校验
    /// </summary>
    /// <param name="container">控件容器</param>
    /// <param name="sources">数据源队列</param>
    public static void ResetEditValidationRules(Control container, params BindingSource[] sources)
    {
      if (container == null)
        return;
      if (sources != null && sources.Length == 0)
        return;
      ResetEditValidationRules(container, sources != null ? new List<BindingSource>(sources) : null);
    }

    private static void ResetEditValidationRules(Control container, IList<BindingSource> sourceList)
    {
      if (container == null)
        return;
      ResetEditValidationRule(container, sourceList);
      foreach (Control item in container.Controls)
        ResetEditValidationRules(item, sourceList);
    }

    private static bool ResetEditValidationRule(Control control, IList<BindingSource> sourceList)
    {
      bool succeed = false;
      bool existRequiredRule = false;
      foreach (Binding item in control.DataBindings)
      {
        if (String.IsNullOrEmpty(item.BindingMemberInfo.BindingField))
          continue;
        BindingSource bindingSource = item.DataSource as BindingSource;
        if (bindingSource == null)
          continue;
        if (sourceList != null && !sourceList.Contains(bindingSource))
          continue;
        Type coreType = BindingSourceHelper.GetDataSourceCoreType(bindingSource);
        if (coreType != null)
        {
          succeed = true;
          //设置控件的有效性校验
          IFieldMapInfo fieldMapInfo = typeof(IBusiness).IsAssignableFrom(coreType)
            ? (IFieldMapInfo)ClassMemberHelper.GetFieldMapInfo(coreType, item.BindingMemberInfo.BindingField, true)
            : (IFieldMapInfo)ClassMemberHelper.GetCriteriaFieldMapInfo(coreType, null, item.BindingMemberInfo.BindingField, true);
          if (fieldMapInfo != null && fieldMapInfo.IsRequired)
            existRequiredRule = true;
        }
      }
      if (succeed && !existRequiredRule)
        ApplyLookupEditKeyDownDelegate(control);
      return succeed;
    }

    #endregion

    #region FetchFriendlyCaption、EditFriendlyCaption

    private static void ApplyVisible(object component, bool visible)
    {
      Type componentType = component.GetType();
      System.Reflection.PropertyInfo visibleInfo = componentType.GetProperty("Visible");
      if (visibleInfo != null)
      {
        if (!object.Equals(visibleInfo.GetValue(component, null), visible))
          visibleInfo.SetValue(component, visible, null);
      }
    }

    private static void ApplyEnabled(Component component, bool enabled)
    {
      Type componentType = component.GetType();
      System.Reflection.PropertyInfo enabledInfo = componentType.GetProperty("Enabled");
      if (enabledInfo != null)
      {
        if (!object.Equals(enabledInfo.GetValue(component, null), enabled))
          enabledInfo.SetValue(component, enabled, null);
      }
    }

    private static void ApplyColor(object component, Color color)
    {
      Type componentType = component.GetType();
      System.Reflection.PropertyInfo foreColorInfo;
      //for Developer Express .NET
      System.Reflection.PropertyInfo appearanceItemCaptionInfo = componentType.GetProperty("AppearanceItemCaption");
      if (appearanceItemCaptionInfo != null)
      {
        foreColorInfo = appearanceItemCaptionInfo.PropertyType.GetProperty("ForeColor");
        if (foreColorInfo != null)
        {
          object appearanceItemCaptionInfoValue = appearanceItemCaptionInfo.GetValue(component, null);
          Color foreColor = (Color)foreColorInfo.GetValue(appearanceItemCaptionInfoValue, null);
          if (foreColor.ToArgb() != color.ToArgb())
            foreColorInfo.SetValue(appearanceItemCaptionInfoValue, color, null);
        }
        return;
      }
      //for VS
      foreColorInfo = componentType.GetProperty("ForeColor");
      if (foreColorInfo != null)
      {
        Color foreColor = (Color)foreColorInfo.GetValue(component, null);
        if (foreColor.ToArgb() != color.ToArgb())
          foreColorInfo.SetValue(component, color, null);
        return;
      }
    }

    private static void ApplyText(object component, string text)
    {
      Type componentType = component.GetType();
      System.Reflection.PropertyInfo textInfo = componentType.GetProperty("Text");
      if (textInfo != null)
      {
        if (!object.Equals(textInfo.GetValue(component, null), text))
          textInfo.SetValue(component, text, null);
      }
    }

    private static string GetBusinessFriendlyName(Control control)
    {
      Type dataSourceType = BindingSourceHelper.GetDataSourceType(BindingSourceHelper.GetDataSource(control));
      System.Reflection.PropertyInfo friendlyNameInfo = Utilities.FindPropertyInfo(dataSourceType, "FriendlyName");
      return friendlyNameInfo != null ? friendlyNameInfo.GetValue(null, null) as string : null;
    }

    /// <summary>
    /// 重置Edit友好性标签
    /// </summary>
    public void ResetEditFriendlyCaptions()
    {
      ResetEditFriendlyCaptions(_friendlyCaptions, null);
    }

    /// <summary>
    /// 重置Edit友好性标签
    /// </summary>
    /// <param name="friendlyCaptionSources">友好标签集合</param>
    /// <param name="sources">数据源队列</param>
    public static void ResetEditFriendlyCaptions(IDictionary<Control, Component> friendlyCaptionSources, params BindingSource[] sources)
    {
      if (friendlyCaptionSources == null)
        throw new ArgumentNullException("friendlyCaptionSources");
      if (sources != null && sources.Length == 0)
        return;
      List<BindingSource> sourceList = sources != null ? new List<BindingSource>(sources) : null;
      foreach (KeyValuePair<Control, Component> kvp in friendlyCaptionSources)
        ResetEditFriendlyCaption(kvp.Key, kvp.Value, sourceList);
    }

    private static void ResetEditFriendlyCaption(Control control, Component friendlyCaption, IList<BindingSource> sourceList)
    {
      string friendlyName = GetBusinessFriendlyName(control);
      if (!String.IsNullOrEmpty(friendlyName))
      {
        ApplyText(friendlyCaption, friendlyName);
        return;
      }
      foreach (Binding item in control.DataBindings)
      {
        if (String.IsNullOrEmpty(item.BindingMemberInfo.BindingField))
          continue;
        BindingSource bindingSource = item.DataSource as BindingSource;
        if (bindingSource == null)
          continue;
        if (sourceList != null && !sourceList.Contains(bindingSource))
          continue;
        Type coreType = BindingSourceHelper.GetDataSourceCoreType(bindingSource);
        if (coreType != null)
        {
          IFieldMapInfo fieldMapInfo = typeof(IBusiness).IsAssignableFrom(coreType)
            ? (IFieldMapInfo)ClassMemberHelper.GetFieldMapInfo(coreType, item.BindingMemberInfo.BindingField, true)
            : (IFieldMapInfo)ClassMemberHelper.GetCriteriaFieldMapInfo(coreType, null, item.BindingMemberInfo.BindingField, true);
          if (fieldMapInfo != null && String.CompareOrdinal(fieldMapInfo.FriendlyName, item.BindingMemberInfo.BindingField) != 0)
            ApplyText(friendlyCaption, fieldMapInfo.FriendlyName);
          else
          {
            Phenix.Core.Mapping.IPropertyInfo propertyInfo = ClassMemberHelper.GetPropertyInfo(coreType, item.BindingMemberInfo.BindingField);
            if (propertyInfo != null && String.CompareOrdinal(propertyInfo.FriendlyName, item.BindingMemberInfo.BindingField) != 0)
              ApplyText(friendlyCaption, propertyInfo.FriendlyName);
            else
            {
              PropertyMapInfo propertyMapInfo = ClassMemberHelper.GetPropertyMapInfo(coreType, item.BindingMemberInfo.BindingField);
              if (propertyMapInfo != null && String.CompareOrdinal(propertyMapInfo.FriendlyName, item.BindingMemberInfo.BindingField) != 0)
                ApplyText(friendlyCaption, propertyMapInfo.FriendlyName);
            }
          }
          if (fieldMapInfo != null)
          {
            if (!fieldMapInfo.Visible)
            {
              ApplyVisible(control, false);
              ApplyVisible(friendlyCaption, false);
            }
            //if (!fieldMapInfo.Enabled)
            //{
            //  ApplyEnabled(control, false);
            //  ApplyEnabled(friendlyCaption, false);
            //}
            if (!RequiredCaptionForeColorDisabled)
              ApplyColor(friendlyCaption, fieldMapInfo.IsRequired ? RequiredCaptionForeColor : Color.Black);
          }
        }
      }
    }

    #endregion

    #region GridFriendlyCaption

    /// <summary>
    /// 重置Grid友好性标签
    /// </summary>
    public void ResetGridFriendlyCaptions()
    {
      ResetGridFriendlyCaptions(Host);
    }

    /// <summary>
    /// 重置控件容器内Grid友好性标签
    /// </summary>
    /// <param name="container">控件容器</param>
    public static void ResetGridFriendlyCaptions(Control container)
    {
      if (container == null)
        throw new ArgumentNullException("container");
      ApplyGridFriendlyCaption(container);
      foreach (Control item in container.Controls)
        ResetGridFriendlyCaptions(item);
    }
    
    /// <summary>
    /// 重置控件容器内Grid友好性标签
    /// </summary>
    /// <param name="container">控件容器</param>
    /// <param name="source">数据源</param>
    /// <param name="text">文本</param>
    public static void ResetGridFriendlyCaptions(Control container, BindingSource source, string text)
    {
      if (container == null)
        throw new ArgumentNullException("container");
      ApplyGridFriendlyCaption(container, source, text);
      foreach (Control item in container.Controls)
        ResetGridFriendlyCaptions(item, source, text);
    }

    private static void ApplyGridFriendlyCaption(Control control)
    {
      try
      {
        if (ApplyGridFriendlyCaptionForViewCollection(control))
          return;
        Type listItemType = Utilities.FindListItemType(BindingSourceHelper.GetDataSourceType(BindingSourceHelper.GetDataSource(control)));
        if (listItemType != null)
        {
          if (ApplyGridFriendlyCaptionForRows(control, listItemType))
            return;
          if (ApplyGridFriendlyCaptionForColumns(control, listItemType))
            return;
        }
      }
      finally
      {
        ApplyLookupEditFriendlyCaption(control);
      }
    }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
    private static bool ApplyGridFriendlyCaptionForViewCollection(Control control)
    {
      //for Developer Express .NET
      System.Reflection.PropertyInfo viewCollectionInfo = control.GetType().GetProperty("ViewCollection"); //GridControl
      if (viewCollectionInfo != null)
      {
        IList viewCollection = viewCollectionInfo.GetValue(control, null) as IList;
        if (viewCollection == null)
          return true;
        foreach (object view in viewCollection)
        {
          Type listItemType = Utilities.FindListItemType(FindDataSourceType(control, view));
          if (listItemType == null)
            continue;
          IList<FieldMapInfo> fieldMapInfos = ClassMemberHelper.GetFieldMapInfos(listItemType);
          if (fieldMapInfos.Count == 0)
            continue;
          foreach (System.Reflection.PropertyInfo item in view.GetType().GetProperties())
            if (!item.PropertyType.IsValueType && String.CompareOrdinal(item.Name, "Columns") == 0)
            {
              IList columns = item.GetValue(view, null) as IList;
              if (columns == null)
                break;
              foreach (object column in columns)
              {
                Type columnType = column.GetType();
                System.Reflection.PropertyInfo fieldNameInfo = columnType.GetProperty("FieldName");
                if (fieldNameInfo == null)
                  break;
                string fieldNameValue = fieldNameInfo.GetValue(column, null) as string;
                if (!String.IsNullOrEmpty(fieldNameValue))
                {
                  FieldMapInfo fieldMapInfo = ClassMemberHelper.GetFieldMapInfo(listItemType, fieldNameValue, true);
                  System.Reflection.PropertyInfo captionInfo = columnType.GetProperty("Caption");
                  if (captionInfo != null)
                  {
                    if (fieldMapInfo != null && String.CompareOrdinal(fieldMapInfo.FriendlyName, fieldNameValue) != 0)
                      captionInfo.SetValue(column, fieldMapInfo.FriendlyName, null);
                    else
                    {
                      Phenix.Core.Mapping.IPropertyInfo propertyInfo = ClassMemberHelper.GetPropertyInfo(listItemType, fieldNameValue);
                      if (propertyInfo != null && String.CompareOrdinal(propertyInfo.FriendlyName, fieldNameValue) != 0)
                        captionInfo.SetValue(column, propertyInfo.FriendlyName, null);
                      else
                      {
                        PropertyMapInfo propertyMapInfo = ClassMemberHelper.GetPropertyMapInfo(listItemType, fieldNameValue);
                        if (propertyMapInfo != null && String.CompareOrdinal(propertyMapInfo.FriendlyName, fieldNameValue) != 0)
                          captionInfo.SetValue(column, propertyMapInfo.FriendlyName, null);
                      }
                    }
                  }
                  if (fieldMapInfo != null)
                  {
                    if (!fieldMapInfo.Visible)
                    {
                      System.Reflection.PropertyInfo visibleInfo = columnType.GetProperty("Visible");
                      if (visibleInfo != null)
                        visibleInfo.SetValue(column, false, null);
                    }
                    //if (!fieldMapInfo.Enabled)
                    //{
                    //  System.Reflection.PropertyInfo optionsColumnInfo = columnType.GetProperty("OptionsColumn");
                    //  if (optionsColumnInfo != null)
                    //  {
                    //    object optionsColumnValue = optionsColumnInfo.GetValue(column, null);
                    //    if (optionsColumnValue != null)
                    //      optionsColumnValue.GetType().GetProperty("ReadOnly").SetValue(optionsColumnValue, true, null);
                    //  }
                    //}
                    if (!RequiredCaptionForeColorDisabled)
                    {
                      System.Reflection.PropertyInfo appearanceHeaderInfo = columnType.GetProperty("AppearanceHeader");
                      if (appearanceHeaderInfo != null)
                      {
                        object appearanceHeaderValue = appearanceHeaderInfo.GetValue(column, null);
                        if (appearanceHeaderValue != null)
                        {
                          System.Reflection.PropertyInfo foreColorInfo = appearanceHeaderInfo.PropertyType.GetProperty("ForeColor");
                          if (foreColorInfo != null)
                            foreColorInfo.SetValue(appearanceHeaderValue, fieldMapInfo.IsRequired ? RequiredCaptionForeColor : Color.Black, null);
                        }
                      }
                    }
                  }
                }
              }
              break;
            }
        }
        return true;
      }
      return false;
    }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
    private static bool ApplyGridFriendlyCaptionForRows(Control control, Type listItemType)
    {
      //for Developer Express .NET
      System.Reflection.PropertyInfo rowsInfo = control.GetType().GetProperty("Rows"); //VGridControl
      if (rowsInfo != null)
      {
        IList rows = rowsInfo.GetValue(control, null) as IList;
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
                if (!string.IsNullOrEmpty(fieldNameValue))
                {
                  FieldMapInfo fieldMapInfo = ClassMemberHelper.GetFieldMapInfo(listItemType, fieldNameValue, true);
                  System.Reflection.PropertyInfo captionInfo = itemType.GetProperty("Caption");
                  if (captionInfo != null)
                  {
                    if (fieldMapInfo != null && String.CompareOrdinal(fieldMapInfo.FriendlyName, fieldNameValue) != 0)
                      captionInfo.SetValue(itemValue, fieldMapInfo.FriendlyName, null);
                    else
                    {
                      Phenix.Core.Mapping.IPropertyInfo propertyInfo = ClassMemberHelper.GetPropertyInfo(listItemType, fieldNameValue);
                      if (propertyInfo != null && String.CompareOrdinal(propertyInfo.FriendlyName, fieldNameValue) != 0)
                        captionInfo.SetValue(itemValue, propertyInfo.FriendlyName, null);
                      else
                      {
                        PropertyMapInfo propertyMapInfo = ClassMemberHelper.GetPropertyMapInfo(listItemType, fieldNameValue);
                        if (propertyMapInfo != null && String.CompareOrdinal(propertyMapInfo.FriendlyName, fieldNameValue) != 0)
                          captionInfo.SetValue(itemValue, propertyMapInfo.FriendlyName, null);
                      }
                    }
                  }
                  if (fieldMapInfo != null)
                  {
                    if (!fieldMapInfo.Visible)
                    {
                      System.Reflection.PropertyInfo visibleInfo = rowType.GetProperty("Visible");
                      if (visibleInfo != null)
                        visibleInfo.SetValue(row, false, null);
                    }
                    //if (!fieldMapInfo.Enabled)
                    //{
                    //  System.Reflection.PropertyInfo optionsColumnInfo = columnType.GetProperty("OptionsColumn");
                    //  if (optionsColumnInfo != null)
                    //  {
                    //    object optionsColumnValue = optionsColumnInfo.GetValue(column, null);
                    //    if (optionsColumnValue != null)
                    //      optionsColumnValue.GetType().GetProperty("ReadOnly").SetValue(optionsColumnValue, true, null);
                    //  }
                    //}
                    if (!RequiredCaptionForeColorDisabled)
                    {
                      System.Reflection.PropertyInfo appearanceInfo = rowType.GetProperty("Appearance");
                      if (appearanceInfo != null)
                      {
                        object appearanceValue = appearanceInfo.GetValue(row, null);
                        if (appearanceValue != null)
                        {
                          System.Reflection.PropertyInfo foreColorInfo = appearanceInfo.PropertyType.GetProperty("ForeColor");
                          if (foreColorInfo != null)
                            foreColorInfo.SetValue(appearanceValue, fieldMapInfo.IsRequired ? RequiredCaptionForeColor : Color.Black, null);
                        }
                      }
                    }
                  }
                }
                break;
              }
            }
        }
        return true;
      }
      return false;
    }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
    private static bool ApplyGridFriendlyCaptionForColumns(Control control, Type listItemType)
    {
      System.Reflection.PropertyInfo columnsInfo = control.GetType().GetProperty("Columns");
      if (columnsInfo != null)
      {
        IList columns = columnsInfo.GetValue(control, null) as IList;
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
            if (!string.IsNullOrEmpty(fieldNameValue))
            {
              FieldMapInfo fieldMapInfo = ClassMemberHelper.GetFieldMapInfo(listItemType, fieldNameValue, true);
              System.Reflection.PropertyInfo captionInfo = columnType.GetProperty("Caption");
              if (captionInfo != null)
              {
                if (fieldMapInfo != null && String.CompareOrdinal(fieldMapInfo.FriendlyName, fieldNameValue) != 0)
                  captionInfo.SetValue(column, fieldMapInfo.FriendlyName, null);
                else
                {
                  Phenix.Core.Mapping.IPropertyInfo propertyInfo = ClassMemberHelper.GetPropertyInfo(listItemType, fieldNameValue);
                  if (propertyInfo != null && String.CompareOrdinal(propertyInfo.FriendlyName, fieldNameValue) != 0)
                    captionInfo.SetValue(column, propertyInfo.FriendlyName, null);
                  else
                  {
                    PropertyMapInfo propertyMapInfo = ClassMemberHelper.GetPropertyMapInfo(listItemType, fieldNameValue);
                    if (propertyMapInfo != null && String.CompareOrdinal(propertyMapInfo.FriendlyName, fieldNameValue) != 0)
                      captionInfo.SetValue(column, propertyMapInfo.FriendlyName, null);
                  }
                }
              }
              if (fieldMapInfo != null)
              {
                if (!fieldMapInfo.Visible)
                {
                  System.Reflection.PropertyInfo visibleInfo = columnType.GetProperty("Visible");
                  if (visibleInfo != null)
                    visibleInfo.SetValue(column, false, null);
                }
                //if (!fieldMapInfo.Enabled)
                //{
                //  System.Reflection.PropertyInfo optionsColumnInfo = columnType.GetProperty("OptionsColumn");
                //  if (optionsColumnInfo != null)
                //  {
                //    object optionsColumnValue = optionsColumnInfo.GetValue(column, null);
                //    if (optionsColumnValue != null)
                //      optionsColumnValue.GetType().GetProperty("ReadOnly").SetValue(optionsColumnValue, true, null);
                //  }
                //}
                if (!RequiredCaptionForeColorDisabled)
                {
                  System.Reflection.PropertyInfo appearanceHeaderInfo = columnType.GetProperty("AppearanceHeader");
                  if (appearanceHeaderInfo != null)
                  {
                    object appearanceHeaderValue = appearanceHeaderInfo.GetValue(column, null);
                    if (appearanceHeaderValue != null)
                    {
                      System.Reflection.PropertyInfo foreColorInfo = appearanceHeaderInfo.PropertyType.GetProperty("ForeColor");
                      if (foreColorInfo != null)
                        foreColorInfo.SetValue(appearanceHeaderValue, fieldMapInfo.IsRequired ? RequiredCaptionForeColor : Color.Black, null);
                    }
                  }
                }
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
              FieldMapInfo fieldMapInfo = ClassMemberHelper.GetFieldMapInfo(listItemType, dataPropertyNameValue, true);
              System.Reflection.PropertyInfo headerTextInfo = columnType.GetProperty("HeaderText");
              if (headerTextInfo != null)
              {
                if (fieldMapInfo != null && String.CompareOrdinal(fieldMapInfo.FriendlyName, dataPropertyNameValue) != 0)
                  headerTextInfo.SetValue(column, fieldMapInfo.FriendlyName, null);
                else
                {
                  Phenix.Core.Mapping.IPropertyInfo propertyInfo = ClassMemberHelper.GetPropertyInfo(listItemType, dataPropertyNameValue);
                  if (propertyInfo != null && String.CompareOrdinal(propertyInfo.FriendlyName, dataPropertyNameValue) != 0)
                    headerTextInfo.SetValue(column, propertyInfo.FriendlyName, null);
                  else
                  {
                    PropertyMapInfo propertyMapInfo = ClassMemberHelper.GetPropertyMapInfo(listItemType, dataPropertyNameValue);
                    if (propertyMapInfo != null && String.CompareOrdinal(propertyMapInfo.FriendlyName, dataPropertyNameValue) != 0)
                      headerTextInfo.SetValue(column, propertyMapInfo.FriendlyName, null);
                  }
                }
              }
              if (fieldMapInfo != null)
              {
                if (!fieldMapInfo.Visible)
                {
                  System.Reflection.PropertyInfo visibleInfo = columnType.GetProperty("Visible");
                  if (visibleInfo != null)
                    visibleInfo.SetValue(column, false, null);
                }
                //if (!fieldMapInfo.Enabled)
                //{
                //  System.Reflection.PropertyInfo readOnlyInfo = columnType.GetProperty("ReadOnly");
                //  if (readOnlyInfo != null)
                //    readOnlyInfo.SetValue(column, true, null);
                //}
              }
            }
            continue;
          }
        }
        return true;
      }
      return false;
    }

    private static void ApplyLookupEditFriendlyCaption(object lookupEdit)
    {
      Type lookupEditType = lookupEdit.GetType();
      //for Developer Express .NET
      foreach (System.Reflection.PropertyInfo item in lookupEditType.GetProperties())
        if (!item.PropertyType.IsValueType && 
          (String.CompareOrdinal(item.Name, "Properties") == 0 || String.CompareOrdinal(item.Name, "ColumnEdit") == 0))
        {
          object itemValue = item.GetValue(lookupEdit, null);
          if (itemValue == null)
            return;
          Type itemType = itemValue.GetType();

          //GridLookUpEdit
          System.Reflection.PropertyInfo viewInfo = itemType.GetProperty("View");
          if (viewInfo != null)
          {
            itemValue = viewInfo.GetValue(itemValue, null);
            if (itemValue == null)
              return;
            itemType = itemValue.GetType();
          }

          Type listItemType = Utilities.FindListItemType(BindingSourceHelper.GetDataSourceType(BindingSourceHelper.GetDataSource(itemValue)));
          if (listItemType == null)
            return;

          //GridLookUpEdit/LookUpEdit
          System.Reflection.PropertyInfo columnsInfo = itemType.GetProperty("Columns");
          if (columnsInfo != null)
            foreach (object column in (IList)columnsInfo.GetValue(itemValue, null))
            {
              Type columnType = column.GetType();
              System.Reflection.PropertyInfo fieldNameInfo = columnType.GetProperty("FieldName");
              if (fieldNameInfo != null)
              {
                string fieldNameValue = fieldNameInfo.GetValue(column, null) as string;
                if (!String.IsNullOrEmpty(fieldNameValue))
                {
                  FieldMapInfo fieldMapInfo = ClassMemberHelper.GetFieldMapInfo(listItemType, fieldNameValue, true);
                  if (fieldMapInfo != null && String.CompareOrdinal(fieldMapInfo.FriendlyName, fieldNameValue) != 0)
                    columnType.GetProperty("Caption").SetValue(column, fieldMapInfo.FriendlyName, null);
                  else
                  {
                    Phenix.Core.Mapping.IPropertyInfo propertyInfo = ClassMemberHelper.GetPropertyInfo(listItemType, fieldNameValue);
                    if (propertyInfo != null && String.CompareOrdinal(propertyInfo.FriendlyName, fieldNameValue) != 0)
                      columnType.GetProperty("Caption").SetValue(column, propertyInfo.FriendlyName, null);
                    else
                    {
                      PropertyMapInfo propertyMapInfo = ClassMemberHelper.GetPropertyMapInfo(listItemType, fieldNameValue);
                      if (propertyMapInfo != null && String.CompareOrdinal(propertyMapInfo.FriendlyName, fieldNameValue) != 0)
                        columnType.GetProperty("Caption").SetValue(column, propertyMapInfo.FriendlyName, null);
                    }
                  }
                  if (fieldMapInfo != null&& !fieldMapInfo.Visible)
                    columnType.GetProperty("Visible").SetValue(column, false, null);
                }
              }
            }
          return;
        }
    }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
    private static void ApplyGridFriendlyCaption(Control control, BindingSource source, string text)
    {
      BindingSource dataSource = BindingSourceHelper.GetDataSource(control);
      if (dataSource == null)
      {
        ApplyLookupEditFriendlyCaption(control, source, text);
        return;
      }
      if (dataSource != source)
        return;

      Type gridType = control.GetType();

      //for Developer Express .NET
      System.Reflection.PropertyInfo viewCollectionInfo = gridType.GetProperty("ViewCollection"); //GridControl
      if (viewCollectionInfo != null)
      {
        IList viewCollection = viewCollectionInfo.GetValue(control, null) as IList;
        if (viewCollection == null)
          return;
        foreach (object view in viewCollection)
          foreach (System.Reflection.PropertyInfo item in view.GetType().GetProperties())
            if (!item.PropertyType.IsValueType && String.CompareOrdinal(item.Name, "Columns") == 0)
            {
              IList columns = item.GetValue(view, null) as IList;
              if (columns == null)
                break;
              foreach (object column in columns)
              {
                Type columnType = column.GetType();
                System.Reflection.PropertyInfo captionInfo = columnType.GetProperty("Caption");
                if (captionInfo != null)
                {
                  captionInfo.SetValue(column, text, null);
                  break;
                }
              }
              break;
            }
        return;
      }

      //for Developer Express .NET
      System.Reflection.PropertyInfo rowsInfo = gridType.GetProperty("Rows"); //VGridControl
      if (rowsInfo != null)
      {
        IList rows = rowsInfo.GetValue(control, null) as IList;
        if (rows == null)
          return;
        foreach (object row in rows)
          foreach (System.Reflection.PropertyInfo item in row.GetType().GetProperties())
            if (!item.PropertyType.IsValueType && String.CompareOrdinal(item.Name, "Properties") == 0)
            {
              object itemValue = item.GetValue(row, null);
              if (itemValue == null)
                return;
              Type itemType = itemValue.GetType();
              System.Reflection.PropertyInfo captionInfo = itemType.GetProperty("Caption");
              if (captionInfo != null)
              {
                captionInfo.SetValue(itemValue, text, null);
                break;
              }
            }
        return;
      }

      System.Reflection.PropertyInfo columnsInfo = gridType.GetProperty("Columns");
      if (columnsInfo != null)
      {
        IList columns = columnsInfo.GetValue(control, null) as IList;
        if (columns == null)
          return;
        foreach (object column in columns)
        {
          Type columnType = column.GetType();
          //for Developer Express .NET
          System.Reflection.PropertyInfo captionInfo = columnType.GetProperty("Caption");
          if (captionInfo != null)
          {
            captionInfo.SetValue(column, text, null);
            break;
          }
          //for VS
          System.Reflection.PropertyInfo headerTextInfo = columnType.GetProperty("HeaderText");
          if (headerTextInfo != null)
          {
            headerTextInfo.SetValue(column, text, null);
            break;
          }
        }
        return;
      }
      ApplyLookupEditFriendlyCaption(control, source, text);
    }

    private static void ApplyLookupEditFriendlyCaption(object lookupEdit, BindingSource source, string text)
    {
      Type lookupEditType = lookupEdit.GetType();
      //for Developer Express .NET
      foreach (System.Reflection.PropertyInfo item in lookupEditType.GetProperties())
        if (!item.PropertyType.IsValueType && 
          (String.CompareOrdinal(item.Name, "Properties") == 0 || String.CompareOrdinal(item.Name, "ColumnEdit") == 0))
        {
          object itemValue = item.GetValue(lookupEdit, null);
          if (itemValue == null)
            return;
          BindingSource dataSource = BindingSourceHelper.GetDataSource(itemValue);
          if (dataSource != source)
            return;

          Type itemType = itemValue.GetType();

          //GridLookUpEdit
          System.Reflection.PropertyInfo viewInfo = itemType.GetProperty("View");
          if (viewInfo != null)
          {
            itemValue = viewInfo.GetValue(itemValue, null);
            if (itemValue == null)
              return;
            itemType = itemValue.GetType();
          }

          //GridLookUpEdit/LookUpEdit
          System.Reflection.PropertyInfo columnsInfo = itemType.GetProperty("Columns");
          if (columnsInfo != null)
            foreach (object column in (IList)columnsInfo.GetValue(itemValue, null))
            {
              Type columnType = column.GetType();
              columnType.GetProperty("Caption").SetValue(column, text, null);
              break;
            }
          return;
        }
    }

    #endregion

    #region GridFriendlyLayout

    /// <summary>
    /// 重置Grid友好性布局
    /// </summary>
    /// <param name="container">控件容器</param>
    public static bool ResetGridFriendlyLayouts(Control container)
    {
      if (container == null)
        throw new ArgumentNullException("container");
      bool result = ApplyGridFriendlyLayout(container);
      foreach (Control item in container.Controls)
        if (!ResetGridFriendlyLayouts(item))
          result = false;
      return result;
    }

    private static bool ApplyGridFriendlyLayout(Control grid)
    {
      BindingSource bindingSource = BindingSourceHelper.GetDataSource(grid);
      if (bindingSource == null)
        return true;

      Type gridType = grid.GetType();

      //for Developer Express .NET
      System.Reflection.PropertyInfo viewCollectionInfo = gridType.GetProperty("ViewCollection"); //GridControl
      if (viewCollectionInfo != null)
      {
        IList viewCollection = viewCollectionInfo.GetValue(grid, null) as IList;
        if (viewCollection == null)
          return true;
        foreach (object view in viewCollection)
        {
          Type listItemType = Utilities.FindListItemType(FindDataSourceType(grid, view));
          if (listItemType == null)
            continue;
          IList<FieldMapInfo> fieldMapInfos = ClassMemberHelper.GetFieldMapInfos(listItemType);
          if (fieldMapInfos.Count == 0)
            continue;
          foreach (System.Reflection.PropertyInfo item in view.GetType().GetProperties())
            if (!item.PropertyType.IsValueType && String.CompareOrdinal(item.Name, "Columns") == 0)
            {
              foreach (object column in (IList)item.GetValue(view, null))
              {
                Type columnType = column.GetType();
                System.Reflection.PropertyInfo fieldNameInfo = columnType.GetProperty("FieldName");
                if (fieldNameInfo != null)
                {
                  string fieldNameValue = fieldNameInfo.GetValue(column, null) as string;
                  if (!string.IsNullOrEmpty(fieldNameValue))
                    ApplyGridFriendlyLayout(grid, column, fieldNameValue, fieldMapInfos, listItemType);
                }
              }
              break;
            }
        }
        return true;
      }

      System.Reflection.PropertyInfo columnsInfo = gridType.GetProperty("Columns");
      if (columnsInfo != null)
      {
        IList columns = columnsInfo.GetValue(grid, null) as IList;
        if (columns == null)
          return true;
        Type listItemType = Utilities.FindListItemType(BindingSourceHelper.GetDataSourceType(bindingSource));
        if (listItemType == null)
          return false;
        IList<FieldMapInfo> fieldMapInfos = ClassMemberHelper.GetFieldMapInfos(listItemType);
        if (fieldMapInfos.Count == 0)
          return false;
        foreach (object column in columns)
        {
          Type columnType = column.GetType();
          //for Developer Express .NET
          System.Reflection.PropertyInfo fieldNameInfo = columnType.GetProperty("FieldName");
          if (fieldNameInfo != null)
          {
            string fieldNameValue = fieldNameInfo.GetValue(column, null) as string;
            if (!string.IsNullOrEmpty(fieldNameValue))
              ApplyGridFriendlyLayout(grid, column, fieldNameValue, fieldMapInfos, listItemType);
            continue;
          }
          //for VS
          System.Reflection.PropertyInfo dataPropertyNameInfo = columnType.GetProperty("DataPropertyName");
          if (dataPropertyNameInfo != null)
          {
            string dataPropertyNameValue = dataPropertyNameInfo.GetValue(column, null) as string;
            if (!string.IsNullOrEmpty(dataPropertyNameValue))
              ApplyGridFriendlyLayout(grid, column, dataPropertyNameValue, fieldMapInfos, listItemType);
            continue;
          }
        }
        return true;
      }
      return true;
    }

    private static void ApplyGridFriendlyLayout(Control grid, object column, string propertyName, IList<FieldMapInfo> fieldMapInfos, Type listItemType)
    {
      int length;
      Type columnType = column.GetType();
      bool find = false;
      foreach (FieldMapInfo fieldMapInfo in fieldMapInfos)
        if (String.CompareOrdinal(fieldMapInfo.PropertyName, propertyName) == 0)
        {
          if (fieldMapInfo.Property != null && fieldMapInfo.Property.PropertyType == typeof(string) &&
            fieldMapInfo.MaximumLength > fieldMapInfo.FriendlyName.Length)
            length = fieldMapInfo.MaximumLength;
          else
            length = fieldMapInfo.FriendlyName.Length;
          columnType.GetProperty("Width").SetValue(column, (int)((length > 10 ? 10 : length < 4 ? 4 : length) * grid.Font.Size * 2), null);
          find = true;
          break;
        }
      if (!find)
      {
        PropertyMapInfo propertyMapInfo = ClassMemberHelper.GetPropertyMapInfo(listItemType, propertyName);
        if (propertyMapInfo != null)
        {
          length = propertyMapInfo.FriendlyName.Length;
          columnType.GetProperty("Width").SetValue(column, (int)((length > 10 ? 10 : length < 4 ? 4 : length) * grid.Font.Size * 2), null);
        }
      }
    }

    #endregion

    #region Rules

    /// <summary>
    /// 重置Edit规则
    /// </summary>
    /// <param name="container">控件容器</param>
    public static string ResetRules(Control container)
    {
      if (container == null)
        throw new ArgumentNullException("container");
      string result = ApplyLookupEditRule(container);
      result += ApplyEditRule(container);
      result += ApplyGridRule(container);
      foreach (Control item in container.Controls)
        result += ResetRules(item);
      if (!String.IsNullOrEmpty(result))
        result = container.Name + Environment.NewLine + result + Environment.NewLine;
      return result;
    }

    private static string ApplyEditRule(Control control)
    {
      string result = String.Empty;
      if (control == null)
        return result;
      foreach (Binding item in control.DataBindings)
      {
        if (String.IsNullOrEmpty(item.BindingMemberInfo.BindingField))
          continue;
        BindingSource bindingSource = item.DataSource as BindingSource;
        if (bindingSource == null)
          continue;
        Type coreType = BindingSourceHelper.GetDataSourceCoreType(bindingSource);
        if (coreType != null)
        {
          IFieldMapInfo fieldMapInfo = typeof(IBusiness).IsAssignableFrom(coreType)
            ? (IFieldMapInfo)ClassMemberHelper.GetFieldMapInfo(coreType, item.BindingMemberInfo.BindingField, true)
            : (IFieldMapInfo)ClassMemberHelper.GetCriteriaFieldMapInfo(coreType, null, item.BindingMemberInfo.BindingField, true);
          if (fieldMapInfo != null)
          {
            Type fieldUnderlyingType = fieldMapInfo.FieldUnderlyingType;
            if (fieldUnderlyingType == typeof(string))
              result += ApplyStringRule(fieldMapInfo, control);
            else if (fieldUnderlyingType == typeof(decimal) ||
              (fieldUnderlyingType.IsPrimitive && !(fieldUnderlyingType == typeof(bool))))
              result += ApplyNumberRule(fieldMapInfo, control);
            else if (fieldUnderlyingType == typeof(DateTime))
              result += ApplyDateRule(fieldMapInfo, control);
          }
        }
      }
      return result;
    }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
    private static string ApplyGridRule(Control grid)
    {
      string result = String.Empty;
      BindingSource bindingSource = BindingSourceHelper.GetDataSource(grid);
      if (bindingSource == null)
        return result;

      Type gridType = grid.GetType();

      //for Developer Express .NET
      System.Reflection.PropertyInfo viewCollectionInfo = gridType.GetProperty("ViewCollection"); //GridControl
      if (viewCollectionInfo != null)
      {
        IList viewCollection = viewCollectionInfo.GetValue(grid, null) as IList;
        if (viewCollection == null)
          return result;
        foreach (object view in viewCollection)
        {
          Type listItemType = Utilities.FindListItemType(FindDataSourceType(grid, view));
          if (listItemType == null)
            continue;
          IList<FieldMapInfo> fieldMapInfos = ClassMemberHelper.GetFieldMapInfos(listItemType);
          if (fieldMapInfos.Count == 0)
            continue;
          foreach (System.Reflection.PropertyInfo item in view.GetType().GetProperties())
            if (!item.PropertyType.IsValueType && String.CompareOrdinal(item.Name, "Columns") == 0)
            {
              foreach (object column in (IList)item.GetValue(view, null))
              {
                result += ApplyLookupEditRule(column);
                Type columnType = column.GetType();
                System.Reflection.PropertyInfo fieldNameInfo = columnType.GetProperty("FieldName");
                if (fieldNameInfo != null)
                {
                  string fieldNameValue = fieldNameInfo.GetValue(column, null) as string;
                  if (!string.IsNullOrEmpty(fieldNameValue))
                    foreach (FieldMapInfo fieldMapInfo in fieldMapInfos)
                      if (String.CompareOrdinal(fieldMapInfo.PropertyName, fieldNameValue) == 0)
                      {
                        Type fieldUnderlyingType = fieldMapInfo.FieldUnderlyingType;
                        if (fieldUnderlyingType == typeof(string))
                          result += ApplyStringRule(fieldMapInfo, column);
                        else if (fieldUnderlyingType == typeof(decimal) ||
                            (fieldUnderlyingType.IsPrimitive && !(fieldUnderlyingType == typeof(bool))))
                          result += ApplyNumberRule(fieldMapInfo, column);
                        else if (fieldUnderlyingType == typeof(DateTime))
                          result += ApplyDateRule(fieldMapInfo, column);
                        break;
                      }
                }
              }
              break;
            }
        }
        return result;
      }

      System.Reflection.PropertyInfo columnsInfo = gridType.GetProperty("Columns");
      if (columnsInfo != null)
      {
        IList columns = columnsInfo.GetValue(grid, null) as IList;
        if (columns == null)
          return result;
        Type listItemType = Utilities.FindListItemType(BindingSourceHelper.GetDataSourceType(bindingSource));
        if (listItemType == null)
          return result;
        IList<FieldMapInfo> fieldMapInfos = ClassMemberHelper.GetFieldMapInfos(listItemType);
        if (fieldMapInfos.Count == 0)
          return result;
        foreach (object column in columns)
        {
          result += ApplyLookupEditRule(column);
          Type columnType = column.GetType();
          //for Developer Express .NET
           System.Reflection.PropertyInfo fieldNameInfo = columnType.GetProperty("FieldName");
           if (fieldNameInfo != null)
           {
             string fieldNameValue = fieldNameInfo.GetValue(column, null) as string;
             if (!string.IsNullOrEmpty(fieldNameValue))
               foreach (FieldMapInfo fieldMapInfo in fieldMapInfos)
                 if (String.CompareOrdinal(fieldMapInfo.PropertyName, fieldNameValue) == 0)
                 {
                   Type fieldUnderlyingType = fieldMapInfo.FieldUnderlyingType;
                   if (fieldUnderlyingType == typeof(string))
                     result += ApplyStringRule(fieldMapInfo, column);
                   else if (fieldUnderlyingType == typeof(decimal) ||
                     (fieldUnderlyingType.IsPrimitive && !(fieldUnderlyingType == typeof(bool))))
                     result += ApplyNumberRule(fieldMapInfo, column);
                   else if (fieldUnderlyingType == typeof(DateTime))
                     result += ApplyDateRule(fieldMapInfo, column);
                   break;
                 }
           }
        }
        return result;
      }
      return result;
    }

    private static string ApplyStringRule(IFieldMapInfo fieldMapInfo, object component)
    {
      if (!fieldMapInfo.InValidation)
        return String.Empty;
      string result = String.Empty;
      if (fieldMapInfo.FieldRuleAttribute != null)
      {
        //ApplyCharacterCasing
        if (fieldMapInfo.FieldRuleAttribute.StringUpperCaseHasValue)
          result += ApplyCharacterCasing(component, fieldMapInfo.FieldRuleAttribute.StringUpperCase ? CharacterCasing.Upper : CharacterCasing.Lower);
        //ApplyImeMode
        if (fieldMapInfo.FieldRuleAttribute.StringOnImeModeHasValue)
          result += ApplyImeMode(component, fieldMapInfo.FieldRuleAttribute.StringOnImeMode ? ImeMode.On : ImeMode.Off);
      }
      //ApplyMaxLength
      if (fieldMapInfo.MaximumLength > 0)
        result += ApplyMaxLength(component, fieldMapInfo.MaximumLength);
      return result;
    }

    private static string ApplyNumberRule(IFieldMapInfo fieldMapInfo, object component)
    {
      if (!fieldMapInfo.InValidation)
        return String.Empty;
      string result = String.Empty;
      //ApplyRangeRule
      foreach (ValidationAttribute item in fieldMapInfo.ValidationAttributes)
        if (String.CompareOrdinal(item.GetType().FullName, typeof(RangeAttribute).FullName) == 0)
          result += ApplyRangeRule(component, ((RangeAttribute)item).Maximum, ((RangeAttribute)item).Minimum);
      ////ApplyScaleRule
      //TableColumnInfo tableColumnInfo = fieldMapInfo.TableColumnInfo;
      //if (tableColumnInfo != null && tableColumnInfo.Scale > 0)
      //  result += ApplyScaleRule(component, tableColumnInfo.Scale);
      return result;
    }

    private static string ApplyDateRule(IFieldMapInfo fieldMapInfo, object component)
    {
      if (!fieldMapInfo.InValidation)
        return String.Empty;
      string result = String.Empty;
      Type componentType = component.GetType();
      //for Developer Express .NET
      foreach (System.Reflection.PropertyInfo item in componentType.GetProperties())
        if (!item.PropertyType.IsValueType &&
          (String.CompareOrdinal(item.Name, "Properties") == 0 || String.CompareOrdinal(item.Name, "ColumnEdit") == 0))
        {
          object itemValue = item.GetValue(component, null);
          if (itemValue == null)
            return result;
          string format;
          if (fieldMapInfo.FieldRuleAttribute != null && fieldMapInfo.FieldRuleAttribute.DateNotContainTimeHasValue && fieldMapInfo.FieldRuleAttribute.DateNotContainTime)
            format = AppConfig.ShortDatePattern;
          else if (fieldMapInfo.FieldRuleAttribute != null && fieldMapInfo.FieldRuleAttribute.DateIsYearMonthHasValue && fieldMapInfo.FieldRuleAttribute.DateIsYearMonth)
            format = AppConfig.YearMonthPattern;
          else
            format = AppConfig.FullDateTimePattern;
          Type itemType = itemValue.GetType();
          System.Reflection.PropertyInfo maskInfo = itemType.GetProperty("Mask");
          if (maskInfo != null)
          {
            object maskValue = maskInfo.GetValue(itemValue, null);
            System.Reflection.PropertyInfo editMaskInfo = maskInfo.PropertyType.GetProperty("EditMask");
            editMaskInfo.SetValue(maskValue, format, null);
            result += item.Name + AppConfig.POINT_SEPARATOR + editMaskInfo.Name + AppConfig.EQUAL_SEPARATOR + format + Environment.NewLine;
          }
          System.Reflection.PropertyInfo displayFormatInfo = itemType.GetProperty("DisplayFormat");
          if (displayFormatInfo != null)
          {
            object displayFormatValue = displayFormatInfo.GetValue(itemValue, null);
            System.Reflection.PropertyInfo formatStringInfo = displayFormatInfo.PropertyType.GetProperty("FormatString");
            formatStringInfo.SetValue(displayFormatValue, format, null);
            result += item.Name + AppConfig.POINT_SEPARATOR + formatStringInfo.Name + AppConfig.EQUAL_SEPARATOR + format + Environment.NewLine;
          }
          return result;
        }
      return result;
    }

    private static string ApplyCharacterCasing(object component, CharacterCasing characterCasing)
    {
      string result = String.Empty;
      Type componentType = component.GetType();
      System.Reflection.PropertyInfo characterCasingInfo;
      //for Developer Express .NET
      foreach (System.Reflection.PropertyInfo item in componentType.GetProperties())
        if (!item.PropertyType.IsValueType && 
          (String.CompareOrdinal(item.Name, "Properties") == 0 || String.CompareOrdinal(item.Name, "ColumnEdit") == 0))
        {
          object itemValue = item.GetValue(component, null);
          if (itemValue == null)
            return result;
          Type itemType = itemValue.GetType();
          characterCasingInfo = itemType.GetProperty("CharacterCasing");
          if (characterCasingInfo != null)
          {
            characterCasingInfo.SetValue(itemValue, characterCasing, null);
            result = item.Name + AppConfig.POINT_SEPARATOR + characterCasingInfo.Name + AppConfig.EQUAL_SEPARATOR + characterCasing.ToString() + Environment.NewLine;
          }
          return result;
        }
      //for VS
      characterCasingInfo = componentType.GetProperty("CharacterCasing");
      if (characterCasingInfo != null)
      {
        characterCasingInfo.SetValue(component, characterCasing, null);
        return characterCasingInfo.Name + AppConfig.EQUAL_SEPARATOR + characterCasing.ToString() + Environment.NewLine;
      }
      return result;
    }

    private static string ApplyImeMode(object component, ImeMode imeMode)
    {
      string result = String.Empty;
      Type componentType = component.GetType();
      System.Reflection.PropertyInfo imeModeInfo = componentType.GetProperty("ImeMode");
      if (imeModeInfo != null)
      {
        imeModeInfo.SetValue(component, imeMode, null);
        result = imeModeInfo.Name + AppConfig.EQUAL_SEPARATOR + imeMode.ToString() + Environment.NewLine;
      }
      return result;
    }

    private static string ApplyMaxLength(object component, int maxLength)
    {
      string result = String.Empty;
      Type componentType = component.GetType();
      System.Reflection.PropertyInfo maxLengthInfo;
      //for Developer Express .NET
      foreach (System.Reflection.PropertyInfo item in componentType.GetProperties())
        if (!item.PropertyType.IsValueType && 
          (String.CompareOrdinal(item.Name, "Properties") == 0 || String.CompareOrdinal(item.Name, "ColumnEdit") == 0))
        {
          object itemValue = item.GetValue(component, null);
          if (itemValue == null)
            return result;
          Type itemType = itemValue.GetType();
          maxLengthInfo = itemType.GetProperty("MaxLength");
          if (maxLengthInfo != null)
          {
            maxLengthInfo.SetValue(itemValue, maxLength, null);
            result = maxLengthInfo.Name + AppConfig.EQUAL_SEPARATOR + maxLength.ToString() + Environment.NewLine;
          }
          return result;
        }
      //for VS
      maxLengthInfo = componentType.GetProperty("MaxLength");
      if (maxLengthInfo != null)
      {
        maxLengthInfo.SetValue(component, maxLength, null);
        return maxLengthInfo.Name + AppConfig.EQUAL_SEPARATOR + maxLength.ToString() + Environment.NewLine;
      }
      return result;
    }

    private static string ApplyRangeRule(object component, object maxValue, object minValue)
    {
      string result = String.Empty;
      Type componentType = component.GetType();
      //for Developer Express .NET
      foreach (System.Reflection.PropertyInfo item in componentType.GetProperties())
        if (!item.PropertyType.IsValueType && 
          (String.CompareOrdinal(item.Name, "Properties") == 0 || String.CompareOrdinal(item.Name, "ColumnEdit") == 0))
        {
          object itemValue = item.GetValue(component, null);
          if (itemValue == null)
            return result;
          Type itemType = itemValue.GetType();
          System.Reflection.PropertyInfo maxValueInfo = itemType.GetProperty("MaxValue");
          if (maxValueInfo != null)
          {
            maxValueInfo.SetValue(itemValue, Utilities.ChangeType(maxValue, typeof(decimal)), null);
            result += String.Format("{0}{1}{2}{3}", maxValueInfo.Name, AppConfig.EQUAL_SEPARATOR, maxValue, Environment.NewLine);
          }
          System.Reflection.PropertyInfo minValueInfo = itemType.GetProperty("MinValue");
          if (minValueInfo != null)
          {
            minValueInfo.SetValue(itemValue, Utilities.ChangeType(minValue, typeof(decimal)), null);
            result += String.Format("{0}{1}{2}{3}", minValueInfo.Name, AppConfig.EQUAL_SEPARATOR, minValue, Environment.NewLine);
          }
          return result;
        }
      //for VS
      System.Reflection.PropertyInfo maximumInfo = componentType.GetProperty("Maximum");
      if (maximumInfo != null)
      {
        maximumInfo.SetValue(component, maxValue, null);
        result += String.Format("{0}{1}{2}{3}", maximumInfo.Name, AppConfig.EQUAL_SEPARATOR, maxValue, Environment.NewLine);
      }
      System.Reflection.PropertyInfo minimumInfo = componentType.GetProperty("Minimum");
      if (minimumInfo != null)
      {
        minimumInfo.SetValue(component, minValue, null);
        result += String.Format("{0}{1}{2}{3}", minimumInfo.Name, AppConfig.EQUAL_SEPARATOR, minValue, Environment.NewLine);
      }
      return result;
    }

    private static string ApplyScaleRule(object component, int scale)
    {
      string result = String.Empty;
      Type componentType = component.GetType();
      //for Developer Express .NET
      foreach (System.Reflection.PropertyInfo item in componentType.GetProperties())
        if (!item.PropertyType.IsValueType && 
          (String.CompareOrdinal(item.Name, "Properties") == 0 || String.CompareOrdinal(item.Name, "ColumnEdit") == 0))
        {
          object itemValue = item.GetValue(component, null);
          if (itemValue == null)
            return result;
          string format = String.Format("f{0}", scale);
          const int FORMAT_TYPE = 1; //DevExpress.Utils.FormatType.Numeric
          Type itemType = itemValue.GetType();
          System.Reflection.PropertyInfo maskInfo = itemType.GetProperty("Mask");
          if (maskInfo != null)
          {
            object maskValue = maskInfo.GetValue(itemValue, null);
            System.Reflection.PropertyInfo editMaskInfo = maskInfo.PropertyType.GetProperty("EditMask");
            editMaskInfo.SetValue(maskValue, format, null);
            result += item.Name + AppConfig.POINT_SEPARATOR + editMaskInfo.Name + AppConfig.EQUAL_SEPARATOR + format + Environment.NewLine;
          }
          System.Reflection.PropertyInfo displayFormatInfo = itemType.GetProperty("DisplayFormat");
          if (displayFormatInfo != null)
          {
            object displayFormatValue = displayFormatInfo.GetValue(itemValue, null);
            System.Reflection.PropertyInfo formatStringInfo = displayFormatInfo.PropertyType.GetProperty("FormatString");
            formatStringInfo.SetValue(displayFormatValue, format, null);
            result += item.Name + AppConfig.POINT_SEPARATOR + formatStringInfo.Name + AppConfig.EQUAL_SEPARATOR + format + Environment.NewLine;
            System.Reflection.PropertyInfo formatTypeInfo = displayFormatInfo.PropertyType.GetProperty("FormatType");
            formatTypeInfo.SetValue(displayFormatValue, FORMAT_TYPE, null);
            result += item.Name + AppConfig.POINT_SEPARATOR + formatTypeInfo.Name + AppConfig.EQUAL_SEPARATOR + FORMAT_TYPE + Environment.NewLine;
          }
          System.Reflection.PropertyInfo editFormatInfo = itemType.GetProperty("EditFormat");
          if (editFormatInfo != null)
          {
            object displayFormatValue = editFormatInfo.GetValue(itemValue, null);
            System.Reflection.PropertyInfo formatStringInfo = editFormatInfo.PropertyType.GetProperty("FormatString");
            formatStringInfo.SetValue(displayFormatValue, format, null);
            result += item.Name + AppConfig.POINT_SEPARATOR + formatStringInfo.Name + AppConfig.EQUAL_SEPARATOR + format + Environment.NewLine;
            System.Reflection.PropertyInfo formatTypeInfo = editFormatInfo.PropertyType.GetProperty("FormatType");
            formatTypeInfo.SetValue(displayFormatValue, FORMAT_TYPE, null);
            result += item.Name + AppConfig.POINT_SEPARATOR + formatTypeInfo.Name + AppConfig.EQUAL_SEPARATOR + FORMAT_TYPE + Environment.NewLine;
          }
          return result; 
        }
      //for VS
      System.Reflection.PropertyInfo decimalPlacesInfo = componentType.GetProperty("DecimalPlaces");
      if (decimalPlacesInfo != null)
      {
        decimalPlacesInfo.SetValue(component, scale, null);
        return decimalPlacesInfo.Name + AppConfig.EQUAL_SEPARATOR + scale.ToString() + Environment.NewLine;  
      }
      return result; 
    }

    private static string ApplyLookupEditRule(object lookupEdit)
    {
      string result = String.Empty;
      if (lookupEdit == null)
        return result;
      Type lookupEditType = lookupEdit.GetType();
      //for Developer Express .NET
      foreach (System.Reflection.PropertyInfo item in lookupEditType.GetProperties())
        if (!item.PropertyType.IsValueType && 
          (String.CompareOrdinal(item.Name, "Properties") == 0 || String.CompareOrdinal(item.Name, "ColumnEdit") == 0))
        {
          object itemValue = item.GetValue(lookupEdit, null);
          if (itemValue == null)
            return result;
          Type itemType = itemValue.GetType();
          System.Reflection.PropertyInfo nullTextInfo = itemType.GetProperty("NullText");
          if (nullTextInfo != null)
          {
            nullTextInfo.SetValue(itemValue, String.Empty, null);
            result += nullTextInfo.Name + AppConfig.EQUAL_SEPARATOR + "String.Empty" + Environment.NewLine;
          }
          System.Reflection.PropertyInfo immediatePopupInfo = itemType.GetProperty("ImmediatePopup");
          if (immediatePopupInfo != null)
          {
            immediatePopupInfo.SetValue(itemValue, true, null);
            result += immediatePopupInfo.Name + AppConfig.EQUAL_SEPARATOR + true.ToString() + Environment.NewLine;
          }
          //System.Reflection.PropertyInfo searchModeInfo = itemType.GetProperty("SearchMode");
          //if (searchModeInfo != null)
          //{
          //  searchModeInfo.SetValue(itemValue, 0, null);
          //  result += searchModeInfo.Name + AppConfig.EQUAL_SEPARATOR + "DevExpress.XtraEditors.Controls.SearchMode.OnlyInPopup" + Environment.NewLine;
          //}
          System.Reflection.PropertyInfo textEditStyleInfo = itemType.GetProperty("TextEditStyle");
          if (textEditStyleInfo != null)
          {
            textEditStyleInfo.SetValue(itemValue, 0, null);
            result += textEditStyleInfo.Name + AppConfig.EQUAL_SEPARATOR + "DevExpress.XtraEditors.Controls.TextEditStyles.Standard" + Environment.NewLine;
          }
          ApplyLookupEditFriendlyCaption(lookupEdit);
          return result;
        }
      return result;
    }

    #endregion

    #region GridLevelNode //for Developer Express .NET

    private static object FindGridLevelNode(IList nodes, object view)
    {
      if (nodes == null)
        return null;
      foreach (object node in nodes)
      {
        Type nodeType = node.GetType();
        if (nodeType.GetProperty("LevelTemplate").GetValue(node, null) == view)
          return node;
        else
        {
          object result = FindGridLevelNode((IList)nodeType.GetProperty("Nodes").GetValue(node, null), view);
          if (result != null)
            return result;
        }
      }
      return null;
    }

    private static object FindGridLevelNode(Control control, object view)
    {
      object levelTree = control.GetType().GetProperty("LevelTree").GetValue(control, null);
      if (levelTree != null)
        return FindGridLevelNode((IList)levelTree.GetType().GetProperty("Nodes").GetValue(levelTree, null), view);
      return null;
    }

    private static Type FindDataSourceType(Control control, object view)
    {
      Type type = BindingSourceHelper.GetDataSourceType(BindingSourceHelper.GetDataSource(control));
      if (type == null)
        return null;
      if (view == control.GetType().GetProperty("MainView").GetValue(control, null))
        return type;
      object node = FindGridLevelNode(control, view);
      if (node == null)
        return null;
      string propertyName = (string)node.GetType().GetProperty("RelationName").GetValue(node, null);
      int levelNumber = 0;
      while (!(bool)node.GetType().GetProperty("IsRootLevel").GetValue(node, null))
      {
        levelNumber = levelNumber + 1;
        node = node.GetType().GetProperty("Parent").GetValue(node, null);
      }
      return Utilities.FindDetailListType(type, propertyName, levelNumber);
    }

    #endregion

    private static bool IsReadOnly(Control control)
    {
      Type controlType = control.GetType();
      System.Reflection.PropertyInfo readOnlyInfo;
      //for Developer Express .NET
      foreach (System.Reflection.PropertyInfo item in controlType.GetProperties())
        if (!item.PropertyType.IsValueType && String.CompareOrdinal(item.Name, "Properties") == 0)
        {
          readOnlyInfo = item.PropertyType.GetProperty("ReadOnly");
          if (readOnlyInfo != null)
            return (bool)readOnlyInfo.GetValue(item.GetValue(control, null), null);
        }
      //for VS
      readOnlyInfo = controlType.GetProperty("ReadOnly");
      if (readOnlyInfo != null)
        return (bool)readOnlyInfo.GetValue(control, null);
      return false;
    }

    #endregion
  }
}