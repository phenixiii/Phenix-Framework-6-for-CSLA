using System;
using System.ComponentModel;
using System.Drawing.Design;
using System.Windows.Forms;
using Phenix.Core;
using Phenix.Core.Rule;
using Phenix.Core.Windows;

namespace Phenix.Windows
{
  /// <summary>
  /// 枚举数据源状况
  /// </summary>
  [Designer(typeof(Phenix.Services.Client.Design.ComponentPropertyDesigner))]
  [DesignTimeVisible(false)]
  [ToolboxItem(false)]
  public sealed class EnumBindingSourceStatus : Component
  {
    /// <summary>
    /// 初始化
    /// </summary>
    public EnumBindingSourceStatus()
      : base() { }

    /// <summary>
    /// 初始化
    /// </summary>
    public EnumBindingSourceStatus(IContainer container)
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

    private BindingSource _bindingSource;
    /// <summary>
    /// 枚举数据源
    /// </summary>
    [DefaultValue(null), Description("枚举数据源\nEnumKeyCaptionCollection类"), Category("Data")]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
    public BindingSource BindingSource
    {
      get { return _bindingSource; }
      set
      {
        if (DesignMode)
        {
          if (value != null)
          {
            Type dataSourceType = BindingSourceHelper.GetDataSourceType(value);
            if (dataSourceType != null && !EnumKeyCaptionCollection.Equals(dataSourceType))
              throw new InvalidOperationException(String.Format("{0}不符合对数据源要求: EnumKeyCaptionCollection类定义", value.GetType().FullName));
          }
        } 
        _bindingSource = value;
      }
    }

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
    [DefaultValue(null), Description("数据源项的类型\n设置为绑定的枚举类型"), Category("Data")]
    [Editor(typeof(Phenix.Services.Client.Design.EnumSelectorEditor), typeof(UITypeEditor))]
    public Type BindingSourceItemType { get; set; }

    private bool _autoFetch = true;
    /// <summary>
    /// 是否自动Fetch?
    /// </summary>
    [Description("是否自动Fetch?\n在界面Shown时, 本项BindingSource被自动Fetch"), Category("Fetch")]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
    public bool AutoFetch
    {
      get { return _autoFetch; }
      set { _autoFetch = value; }
    }

    #endregion
  }
}