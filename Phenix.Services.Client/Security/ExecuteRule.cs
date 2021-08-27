using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Drawing.Design;
using System.Windows.Forms;
using Phenix.Business;
using Phenix.Business.Rules;
using Phenix.Core;
using Phenix.Core.Mapping;
using Phenix.Core.Reflection;
using Phenix.Core.Rule;
using Phenix.Core.Windows;
using Phenix.Services.Client.Library;

namespace Phenix.Services.Client.Security
{
  /// <summary>
  /// 执行规则
  /// </summary>
  [Serializable]
  [Designer(typeof(Phenix.Services.Client.Design.ComponentPropertyDesigner))]
  [DesignTimeVisible(false)]
  [ToolboxItem(false)]
  public sealed class ExecuteRule : Component
  {
    /// <summary>
    /// 初始化
    /// </summary>
    public ExecuteRule()
      : base() { }

    /// <summary>
    /// 初始化
    /// </summary>
    public ExecuteRule(IContainer container)
      : base()
    {
      if (container == null)
        throw new ArgumentNullException("container");
      container.Add(this);
    }

    #region 属性

    /// <summary>
    /// 数据源
    /// </summary>
    [DefaultValue(null), Description("数据源"), Category("Data")]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
    public BindingSource BindingSource { get; set; }

    /// <summary>
    /// 数据源的类型
    /// </summary>
    [Description("数据源的类型"), Category("Data")]
    public Type BindingSourceType
    {
      get { return BindingSourceHelper.GetDataSourceType(BindingSource); }
    }

    /// <summary>
    /// 数据源项的类型
    /// </summary>
    [Description("数据源项的类型"), Category("Data")]
    public Type BindingSourceItemType
    {
      get { return Utilities.FindListItemType(BindingSourceType); }
    }

    /// <summary>
    /// 数据源类型的属性名
    /// </summary>
    [DefaultValue(null), Description("数据源类型的属性名\n仅允许设置枚举类型的属性"), Category("Design")]
    [Editor(typeof(EnumTypePropertyNameSelectorEditor), typeof(UITypeEditor))]
    public string TypePropertyName { get; set; }

    /// <summary>
    /// 数据源类型的方法名
    /// </summary>
    [DefaultValue(null), Description("数据源类型的方法名\n仅允许设置被Phenix.Core.Mapping.MethodAttribute标记过的方法"), Category("Design")]
    [Editor(typeof(TypeMethodNameSelectorEditor), typeof(UITypeEditor))]
    public string TypeMethodName { get; set; }

    /// <summary>
    /// 数据源项类型的属性名
    /// </summary>
    [DefaultValue(null), Description("数据源项类型的属性名\n仅允许设置枚举类型的属性"), Category("Design")]
    [Editor(typeof(EnumItemTypePropertyNameSelectorEditor), typeof(UITypeEditor))]
    public string ItemTypePropertyName { get; set; }

    /// <summary>
    /// 数据源项类型的方法名
    /// </summary>
    [DefaultValue(null), Description("数据源项类型的方法名\n仅允许设置被Phenix.Core.Mapping.MethodAttribute标记过的方法"), Category("Design")]
    [Editor(typeof(ItemTypeMethodNameSelectorEditor), typeof(UITypeEditor))]
    public string ItemTypeMethodName { get; set; }

    private System.Reflection.PropertyInfo TypePropertyInfo
    {
      get { return Utilities.FindPropertyInfo(BindingSourceType, TypePropertyName); }
    }

    private System.Reflection.PropertyInfo ItemTypePropertyInfo
    {
      get { return Utilities.FindPropertyInfo(BindingSourceItemType, ItemTypePropertyName); }
    }

    /// <summary>
    /// 数据源类型的属性类型
    /// </summary>
    [Description("数据源类型的属性类型\n枚举类型"), Category("Design")]
    public Type TypePropertyType
    {
      get { return TypePropertyInfo != null ? TypePropertyInfo.PropertyType : null; }
    }

    /// <summary>
    /// 数据源项类型的属性类型
    /// </summary>
    [Description("数据源项类型的属性类型\n枚举类型"), Category("Design")]
    public Type ItemTypePropertyType
    {
      get { return ItemTypePropertyInfo != null ? ItemTypePropertyInfo.PropertyType : null; }
    }

    /// <summary>
    /// 根据数据源对象的属性值提供拒绝执行的规则
    /// </summary>
    [DefaultValue(null), Description("根据数据源对象的属性值提供拒绝执行的规则\n当BindingSource.List的TypePropertyName所指定属性的值为本值时, 本组件的Enabled属性会被置为false"), Category("Behavior")]
    [Editor(typeof(DenyExecuteRuleByTypePropertyValueSelectorEditor), typeof(UITypeEditor))]
    public object DenyExecuteRuleByTypePropertyValue { get; set; }

    /// <summary>
    /// 根据数据源项对象的属性值提供拒绝执行的规则
    /// </summary>
    [DefaultValue(null), Description("根据数据源项对象的属性值提供拒绝执行的规则\n当BindingSource.Current的ItemTypePropertyName所指定属性的值为本值时, 本组件的Enabled属性会被置为false"), Category("Behavior")]
    [Editor(typeof(DenyExecuteRuleByItemTypePropertyValueSelectorEditor), typeof(UITypeEditor))]
    public object DenyExecuteRuleByItemTypePropertyValue { get; set; }

    private readonly Collection<ExecuteRuleMethodArgumentStatus> _checkExecuteMethodContextArguments = 
      new Collection<ExecuteRuleMethodArgumentStatus>();
    /// <summary>
    /// 根据执行规则上下文参数提供检查执行方法的规则
    /// </summary>
    [Description("根据执行规则上下文参数提供检查执行方法的规则\n检查执行方法的规则时调用业务对象CanExecuteMethod()方法需传递的arguments参数"), Category("Behavior")]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
    [Editor(typeof(Phenix.Services.Client.Design.CollectionEditor), typeof(UITypeEditor))]
    public Collection<ExecuteRuleMethodArgumentStatus> CheckExecuteMethodContextArguments
    {
      get { return _checkExecuteMethodContextArguments; }
    }

    #endregion

    #region 方法

    /// <summary>
    /// 是否拒绝执行
    /// </summary>
    public bool DenyExecute()
    {
      if (!String.IsNullOrEmpty(TypeMethodName))
      {
        IAuthorizationObject authorizationObject = BindingSourceHelper.GetDataSourceList(BindingSource) as IAuthorizationObject;
        if (authorizationObject == null)
          return true;
        List<object> arguments = new List<object>();
        foreach (ExecuteRuleMethodArgumentStatus item in CheckExecuteMethodContextArguments)
          arguments.Add(item.GetContextArgumentValue());
        if (!authorizationObject.AllowExecuteMethod(TypeMethodName, arguments.ToArray()))
          return true;
      }

      if (!String.IsNullOrEmpty(ItemTypeMethodName))
      {
        IAuthorizationObject authorizationObject = BindingSourceHelper.GetDataSourceCurrent(BindingSource) as IAuthorizationObject;
        if (authorizationObject == null)
         return true;
        List<object> arguments = new List<object>();
        foreach (ExecuteRuleMethodArgumentStatus item in CheckExecuteMethodContextArguments)
          arguments.Add(item.GetContextArgumentValue());
        if (!authorizationObject.AllowExecuteMethod(ItemTypeMethodName, arguments.ToArray()))
          return true;
      }

      System.Reflection.PropertyInfo typePropertyInfo = TypePropertyInfo;
      if (typePropertyInfo != null)
      {
        IBusinessCollection currentBusinessList = BindingSourceHelper.GetDataSourceList(BindingSource) as IBusinessCollection;
        if (currentBusinessList == null || Enum.Equals(typePropertyInfo.GetValue(typePropertyInfo.GetGetMethod().IsStatic ? null : currentBusinessList, null), DenyExecuteRuleByTypePropertyValue))
          return true;
      }

      System.Reflection.PropertyInfo itemTypePropertyInfo = ItemTypePropertyInfo;
      if (itemTypePropertyInfo != null)
      {
        IBusinessObject currentBusiness = BindingSourceHelper.GetDataSourceCurrent(BindingSource) as IBusinessObject;
        if (currentBusiness == null || Enum.Equals(itemTypePropertyInfo.GetValue(itemTypePropertyInfo.GetGetMethod().IsStatic ? null : currentBusiness, null), DenyExecuteRuleByItemTypePropertyValue))
          return true;
      }
      return false;
    }

    /// <summary>
    /// 字符串表示
    /// </summary>
    public override string ToString()
    {
      string result = String.Empty;
      if (BindingSourceType != null && !String.IsNullOrEmpty(TypeMethodName))
        result += BindingSourceType.ToString() + AppConfig.POINT_SEPARATOR + TypeMethodName + Environment.NewLine;
      if (BindingSourceItemType != null && !String.IsNullOrEmpty(ItemTypeMethodName))
        result += BindingSourceItemType.ToString() + AppConfig.POINT_SEPARATOR + ItemTypeMethodName + Environment.NewLine;
      if (BindingSourceType != null && TypePropertyInfo != null)
        result += BindingSourceType.ToString() + AppConfig.POINT_SEPARATOR + (TypePropertyName ?? String.Empty) + AppConfig.EQUAL_SEPARATOR + 
          (DenyExecuteRuleByTypePropertyValue != null ? DenyExecuteRuleByTypePropertyValue.ToString() : String.Empty) + Environment.NewLine;
      if (BindingSourceItemType != null && ItemTypePropertyInfo != null)
        result += BindingSourceItemType.ToString() + AppConfig.POINT_SEPARATOR + (ItemTypePropertyName ?? String.Empty) + AppConfig.EQUAL_SEPARATOR +
          (DenyExecuteRuleByItemTypePropertyValue != null ? DenyExecuteRuleByItemTypePropertyValue.ToString() : String.Empty) + Environment.NewLine;
      return result;
    }

    #endregion
  }

  internal class EnumTypePropertyNameSelectorEditor : ObjectSelectorEditor
  {
    public EnumTypePropertyNameSelectorEditor()
      : base(false) { }

    public override bool IsDropDownResizable
    {
      get { return true; }
    }

    protected override void FillTreeWithData(ObjectSelectorEditor.Selector selector,
      ITypeDescriptorContext context, IServiceProvider provider)
    {
      base.FillTreeWithData(selector, context, provider);
      if (context == null)
        return;
      ExecuteRule rule = context.Instance as ExecuteRule;
      if (rule == null)
        return;
      if (rule.BindingSourceType != null)
        foreach (System.Reflection.PropertyInfo item in rule.BindingSourceType.GetProperties())
          if (item.CanRead && item.GetGetMethod() != null && Utilities.GetUnderlyingType(item.PropertyType).IsEnum)
            selector.AddNode(item.Name, item.Name, null);
    }
  }

  internal class TypeMethodNameSelectorEditor : ObjectSelectorEditor
  {
    public TypeMethodNameSelectorEditor()
      : base(false)
    {
      Registration.RegisterEmbeddedWorker(false);
    }

    public override bool IsDropDownResizable
    {
      get { return true; }
    }

    protected override void FillTreeWithData(ObjectSelectorEditor.Selector selector,
      ITypeDescriptorContext context, IServiceProvider provider)
    {
      base.FillTreeWithData(selector, context, provider);
      if (context == null)
        return;
      ExecuteRule rule = context.Instance as ExecuteRule;
      if (rule == null)
        return;
      if (rule.BindingSourceType != null)
        foreach (KeyValuePair<string, MethodMapInfo> kvp in ClassMemberHelper.GetMethodMapInfos(rule.BindingSourceType, false))
          selector.AddNode(kvp.Value.FriendlyName, kvp.Value.MethodName, null);
    }
  }

  internal class DenyExecuteRuleByTypePropertyValueSelectorEditor : ObjectSelectorEditor
  {
    public DenyExecuteRuleByTypePropertyValueSelectorEditor()
      : base(false)
    {
      Registration.RegisterEmbeddedWorker(false);
    }

    public override bool IsDropDownResizable
    {
      get { return true; }
    }

    protected override void FillTreeWithData(ObjectSelectorEditor.Selector selector,
      ITypeDescriptorContext context, IServiceProvider provider)
    {
      base.FillTreeWithData(selector, context, provider);
      if (context == null)
        return;
      ExecuteRule rule = context.Instance as ExecuteRule;
      if (rule == null || rule.TypePropertyType == null)
        return;
      Type type = Utilities.GetUnderlyingType(rule.TypePropertyType);
      if (type != null)
        foreach (EnumKeyCaption item in EnumKeyCaptionCollection.Fetch(type))
          selector.AddNode(item.Caption, item.Value, null);
    }
  }

  internal class EnumItemTypePropertyNameSelectorEditor : ObjectSelectorEditor
  {
    public EnumItemTypePropertyNameSelectorEditor()
      : base(false) { }

    public override bool IsDropDownResizable
    {
      get { return true; }
    }

    protected override void FillTreeWithData(ObjectSelectorEditor.Selector selector,
      ITypeDescriptorContext context, IServiceProvider provider)
    {
      base.FillTreeWithData(selector, context, provider);
      if (context == null)
        return;
      ExecuteRule rule = context.Instance as ExecuteRule;
      if (rule == null)
        return;
      if (rule.BindingSourceItemType != null)
        foreach (System.Reflection.PropertyInfo item in rule.BindingSourceItemType.GetProperties())
          if (item.CanRead && item.GetGetMethod() != null && Utilities.GetUnderlyingType(item.PropertyType).IsEnum)
            selector.AddNode(item.Name, item.Name, null);
    }
  }

  internal class ItemTypeMethodNameSelectorEditor : ObjectSelectorEditor
  {
    public ItemTypeMethodNameSelectorEditor()
      : base(false)
    {
      Registration.RegisterEmbeddedWorker(false);
    }

    public override bool IsDropDownResizable
    {
      get { return true; }
    }

    protected override void FillTreeWithData(ObjectSelectorEditor.Selector selector,
      ITypeDescriptorContext context, IServiceProvider provider)
    {
      base.FillTreeWithData(selector, context, provider);
      if (context == null)
        return;
      ExecuteRule rule = context.Instance as ExecuteRule;
      if (rule == null)
        return;
      if (rule.BindingSourceItemType != null)
        foreach (KeyValuePair<string, MethodMapInfo> kvp in ClassMemberHelper.GetMethodMapInfos(rule.BindingSourceItemType, false))
          selector.AddNode(kvp.Value.FriendlyName, kvp.Value.MethodName, null);
    }
  }

  internal class DenyExecuteRuleByItemTypePropertyValueSelectorEditor : ObjectSelectorEditor
  {
    public DenyExecuteRuleByItemTypePropertyValueSelectorEditor()
      : base(false)
    {
      Registration.RegisterEmbeddedWorker(false);
    }

    public override bool IsDropDownResizable
    {
      get { return true; }
    }

    protected override void FillTreeWithData(ObjectSelectorEditor.Selector selector,
      ITypeDescriptorContext context, IServiceProvider provider)
    {
      base.FillTreeWithData(selector, context, provider);
      if (context == null)
        return;
      ExecuteRule rule = context.Instance as ExecuteRule;
      if (rule == null || rule.ItemTypePropertyType == null)
        return;
      Type type = Utilities.GetUnderlyingType(rule.ItemTypePropertyType);
      if (type != null)
        foreach (EnumKeyCaption item in EnumKeyCaptionCollection.Fetch(type))
          selector.AddNode(item.Caption, item.Value, null);
    }
  }
}