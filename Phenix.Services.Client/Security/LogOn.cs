using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Drawing;
using System.Drawing.Design;
using Phenix.Core.Net;
using Phenix.Core.Security;

namespace Phenix.Services.Client.Security
{
  /// <summary>
  /// 登录组件
  /// </summary>
  [Description("提供缺省登陆界面, 校验用户及升级服务")]
  [ToolboxItem(true), ToolboxBitmap(typeof(LogOn), "Phenix.Services.Client.Security.LogOn")]
  public sealed class LogOn : Component
  {
    /// <summary>
    /// 初始化
    /// </summary>
    public LogOn()
      : base() { }

    /// <summary>
    /// 初始化
    /// </summary>
    public LogOn(IContainer container)
      : base()
    {
      if (container == null)
        throw new ArgumentNullException("container");
      container.Add(this);
    }

    #region 属性

    /// <summary>
    /// 登陆界面的标题
    /// </summary>
    [DefaultValue(null), Description("登陆界面的标题"), Category("Appearance")]
    public string Title { get; set; }

    /// <summary>
    /// 登陆界面的标志
    /// </summary>
    [DefaultValue(null), Description("登陆界面的标志"), Category("Appearance")]
    public Image Logo { get; set; }

    /// <summary>
    /// 升级文件的代理类型
    /// 缺省为 null 代表将默认使用 Phenix.Core.Net.NetConfig.ProxyType
    /// </summary>
    [DefaultValue(null), Description("升级文件的代理类型\n默认为登录的代理类型"), Category("Behavior")]
    public ProxyType? UpgradeProxyType { get; set; }

    /// <summary>
    /// 升级文件的服务地址
    /// 缺省为 null 代表将默认使用 Phenix.Core.Net.NetConfig.ServicesAddress
    /// </summary>
    [DefaultValue(null), Description("升级文件的服务地址\n默认为登录的服务地址"), Category("Behavior")]
    public string UpgradeServicesAddress { get; set; }

    private readonly Collection<string> _upgradeFileFilters = new Collection<string>();
    /// <summary>
    /// 升级文件的过滤器集
    /// </summary>
    [Description("升级文件的过滤器集"), Category("Behavior")]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
    [Editor(typeof(UpgradeFileFiltersEditor), typeof(UITypeEditor))]
    public Collection<string> UpgradeFileFilters
    {
      get { return _upgradeFileFilters; }
    }

    #endregion

    #region 方法

    /// <summary>
    /// 执行
    /// </summary>
    public IPrincipal Execute()
    {
      return Execute<LogOnDialog>();
    }

    /// <summary>
    /// 执行
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
    public IPrincipal Execute<T>()
      where T : LogOnDialog
    {
      Csla.ApplicationContext.PropertyChangedMode = Csla.ApplicationContext.PropertyChangedModes.Windows;
      return LogOnDialog.Execute<T>(Title, Logo, UpgradeProxyType, UpgradeServicesAddress, UpgradeFileFilters);
    }

    #endregion

    #region 内嵌类

    private class UpgradeFileFiltersEditor : CollectionEditor
    {
      public UpgradeFileFiltersEditor(Type type)
        : base(type) { }

      protected override Object CreateInstance(Type itemType)
      {
        return "*.dll";
      }
    }

    #endregion
  }
}
