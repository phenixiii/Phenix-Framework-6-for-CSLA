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
  /// ��¼���
  /// </summary>
  [Description("�ṩȱʡ��½����, У���û�����������")]
  [ToolboxItem(true), ToolboxBitmap(typeof(LogOn), "Phenix.Services.Client.Security.LogOn")]
  public sealed class LogOn : Component
  {
    /// <summary>
    /// ��ʼ��
    /// </summary>
    public LogOn()
      : base() { }

    /// <summary>
    /// ��ʼ��
    /// </summary>
    public LogOn(IContainer container)
      : base()
    {
      if (container == null)
        throw new ArgumentNullException("container");
      container.Add(this);
    }

    #region ����

    /// <summary>
    /// ��½����ı���
    /// </summary>
    [DefaultValue(null), Description("��½����ı���"), Category("Appearance")]
    public string Title { get; set; }

    /// <summary>
    /// ��½����ı�־
    /// </summary>
    [DefaultValue(null), Description("��½����ı�־"), Category("Appearance")]
    public Image Logo { get; set; }

    /// <summary>
    /// �����ļ��Ĵ�������
    /// ȱʡΪ null ����Ĭ��ʹ�� Phenix.Core.Net.NetConfig.ProxyType
    /// </summary>
    [DefaultValue(null), Description("�����ļ��Ĵ�������\nĬ��Ϊ��¼�Ĵ�������"), Category("Behavior")]
    public ProxyType? UpgradeProxyType { get; set; }

    /// <summary>
    /// �����ļ��ķ����ַ
    /// ȱʡΪ null ����Ĭ��ʹ�� Phenix.Core.Net.NetConfig.ServicesAddress
    /// </summary>
    [DefaultValue(null), Description("�����ļ��ķ����ַ\nĬ��Ϊ��¼�ķ����ַ"), Category("Behavior")]
    public string UpgradeServicesAddress { get; set; }

    private readonly Collection<string> _upgradeFileFilters = new Collection<string>();
    /// <summary>
    /// �����ļ��Ĺ�������
    /// </summary>
    [Description("�����ļ��Ĺ�������"), Category("Behavior")]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
    [Editor(typeof(UpgradeFileFiltersEditor), typeof(UITypeEditor))]
    public Collection<string> UpgradeFileFilters
    {
      get { return _upgradeFileFilters; }
    }

    #endregion

    #region ����

    /// <summary>
    /// ִ��
    /// </summary>
    public IPrincipal Execute()
    {
      return Execute<LogOnDialog>();
    }

    /// <summary>
    /// ִ��
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
    public IPrincipal Execute<T>()
      where T : LogOnDialog
    {
      Csla.ApplicationContext.PropertyChangedMode = Csla.ApplicationContext.PropertyChangedModes.Windows;
      return LogOnDialog.Execute<T>(Title, Logo, UpgradeProxyType, UpgradeServicesAddress, UpgradeFileFilters);
    }

    #endregion

    #region ��Ƕ��

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
