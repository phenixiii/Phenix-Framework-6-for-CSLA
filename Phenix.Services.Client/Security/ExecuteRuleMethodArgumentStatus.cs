using System;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Drawing.Design;
using System.Windows.Forms;
using Phenix.Business;
using Phenix.Core.Reflection;
using Phenix.Core.Windows;

namespace Phenix.Services.Client.Security
{
  /// <summary>
  /// 执行方法规则上下文参数
  /// </summary>
  [Serializable]
  [Designer(typeof(Phenix.Services.Client.Design.ComponentPropertyDesigner))]
  [DesignTimeVisible(false)]
  [ToolboxItem(false)]
  public sealed class ExecuteRuleMethodArgumentStatus : Component
  {
    /// <summary>
    /// 初始化
    /// </summary>
    public ExecuteRuleMethodArgumentStatus()
      : base() { }

    /// <summary>
    /// 初始化
    /// </summary>
    public ExecuteRuleMethodArgumentStatus(IContainer container)
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
    [DefaultValue(null), Description("数据源类型的属性名"), Category("Design")]
    [Editor(typeof(TypePropertyNameSelectorEditor), typeof(UITypeEditor))]
    public string TypePropertyName { get; set; }

    /// <summary>
    /// 数据源项类型的属性名
    /// </summary>
    [DefaultValue(null), Description("数据源项类型的属性名"), Category("Design")]
    [Editor(typeof(ItemTypePropertyNameSelectorEditor), typeof(UITypeEditor))]
    public string ItemTypePropertyName { get; set; }

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
    [Description("数据源类型的属性类型"), Category("Design")]
    public Type TypePropertyType
    {
      get { return TypePropertyInfo != null ? TypePropertyInfo.PropertyType : null; }
    }

    /// <summary>
    /// 数据源项类型的属性类型
    /// </summary>
    [Description("数据源项类型的属性类型"), Category("Design")]
    public Type ItemTypePropertyType
    {
      get { return ItemTypePropertyInfo != null ? ItemTypePropertyInfo.PropertyType : null; }
    }

    private IBusinessCollection CurrentBusinessList
    {
      get { return BindingSourceHelper.GetDataSourceList(BindingSource) as IBusinessCollection; }
    }

    private IBusinessObject CurrentBusiness
    {
      get { return BindingSourceHelper.GetDataSourceCurrent(BindingSource) as IBusinessObject; }
    }

    #endregion

    #region 方法

    /// <summary>
    /// 取上下文参数值
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate")]
    public object GetContextArgumentValue()
    {
      System.Reflection.PropertyInfo typePropertyInfo = TypePropertyInfo;
      if (typePropertyInfo != null)
      {
        IBusinessCollection currentBusinessList = CurrentBusinessList;
        return currentBusinessList != null
          ? typePropertyInfo.GetValue(typePropertyInfo.GetGetMethod().IsStatic ? null : currentBusinessList, null)
          : typePropertyInfo.PropertyType.IsValueType ? Activator.CreateInstance(typePropertyInfo.PropertyType) : null;
      }

      System.Reflection.PropertyInfo itemTypePropertyInfo = ItemTypePropertyInfo;
      if (itemTypePropertyInfo != null)
      {
        IBusinessObject currentBusiness = CurrentBusiness;
        return currentBusiness != null
          ? itemTypePropertyInfo.GetValue(itemTypePropertyInfo.GetGetMethod().IsStatic ? null : currentBusiness, null)
          : itemTypePropertyInfo.PropertyType.IsValueType ? Activator.CreateInstance(itemTypePropertyInfo.PropertyType) : null;
      }
      return null;
    }

    #endregion
  }

  internal class TypePropertyNameSelectorEditor : ObjectSelectorEditor
  {
    public TypePropertyNameSelectorEditor()
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
      ExecuteRuleMethodArgumentStatus rule = context.Instance as ExecuteRuleMethodArgumentStatus;
      if (rule == null)
        return;
      if (rule.BindingSourceType != null)
        foreach (System.Reflection.PropertyInfo item in rule.BindingSourceType.GetProperties())
          if (item.CanRead && item.GetGetMethod() != null)
            selector.AddNode(item.Name, item.Name, null);
    }
  }

  internal class ItemTypePropertyNameSelectorEditor : ObjectSelectorEditor
  {
    public ItemTypePropertyNameSelectorEditor()
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
      ExecuteRuleMethodArgumentStatus rule = context.Instance as ExecuteRuleMethodArgumentStatus;
      if (rule == null)
        return;
      if (rule.BindingSourceItemType != null)
        foreach (System.Reflection.PropertyInfo item in rule.BindingSourceItemType.GetProperties())
          if (item.CanRead && item.GetGetMethod() != null)
            selector.AddNode(item.Name, item.Name, null);
    }
  }
}