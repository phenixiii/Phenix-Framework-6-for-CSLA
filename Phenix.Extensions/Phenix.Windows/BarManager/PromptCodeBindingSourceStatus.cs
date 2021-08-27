using System;
using System.ComponentModel;
using System.Windows.Forms;
using Phenix.Core;
using Phenix.Core.Rule;
using Phenix.Core.Windows;

namespace Phenix.Windows
{
  /// <summary>
  /// 提示码数据源状况
  /// </summary>
  [Designer(typeof(Phenix.Services.Client.Design.ComponentPropertyDesigner))]
  [DesignTimeVisible(false)]
  [ToolboxItem(false)]
  public sealed class PromptCodeBindingSourceStatus : Component
  {
    /// <summary>
    /// 初始化
    /// </summary>
    public PromptCodeBindingSourceStatus()
      : base() { }

    /// <summary>
    /// 初始化
    /// </summary>
    public PromptCodeBindingSourceStatus(IContainer container)
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
    /// 提示码数据源
    /// </summary>
    [DefaultValue(null), Description("提示码数据源\nPromptCodeCaptionCollection类"), Category("Data")]
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
            if (dataSourceType != null && !PromptCodeKeyCaptionCollection.Equals(dataSourceType))
              throw new InvalidOperationException(String.Format("{0}不符合对数据源要求: PromptCodeKeyCaptionCollection类定义", value.GetType().FullName));
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
    /// 提示码名
    /// </summary>
    [DefaultValue(null), Description("提示码名\n标识提示码数据集以下载和维护其内容"), Category("Data")]
    public string PromptCodeName { get; set; }

    private ReadLevel _defaultReadLevel = ReadLevel.Public;
    /// <summary>
    /// 缺省读取级别
    /// </summary>
    [DefaultValue(ReadLevel.Public), Description("缺省读取级别\n当新增提示码时缺省定义的读取级别"), Category("Data")]
    public ReadLevel DefaultReadLevel
    {
      get { return _defaultReadLevel; }
      set { _defaultReadLevel = value; }
    }

    /// <summary>
    /// 是否自动刷新?
    /// </summary>
    private bool _autoRefresh = true;
    /// <summary>
    /// 是否自动刷新?
    /// </summary>
    [DefaultValue(true), Description("是否自动刷新?\n当数据源数据发生变动时, 本项BindingSource被自动Fetch"), Category("Fetch")]
    public bool AutoRefresh
    {
      get { return _autoRefresh; }
      set { _autoRefresh = value; }
    }

    private bool _autoFetch = true;
    /// <summary>
    /// 是否自动Fetch?
    /// </summary>
    [DefaultValue(true), Description("是否自动Fetch?\n在界面Shown时, 本项BindingSource被自动Fetch"), Category("Fetch")]
    public bool AutoFetch
    {
      get { return _autoFetch; }
      set { _autoFetch = value; }
    }

    private bool _inFetch = true;
    /// <summary>
    /// 是否用于Fetch功能?
    /// </summary>
    [DefaultValue(true), Description("是否用于Fetch功能?\n根据GroupName分组管理"), Category("Fetch")]
    public bool InFetch
    {
      get { return _inFetch; }
      set { _inFetch = value; }
    }

    /// <summary>
    /// 必须用于Fetch功能?
    /// </summary>
    [DefaultValue(false), Description("必须用于Fetch功能?\n不受GroupName分组管理"), Category("Fetch")]
    public bool MustFetch { get; set; }

    private bool _isAsynchronousFetch = true;
    /// <summary>
    /// 是否异步Fetch?
    /// </summary>
    [DefaultValue(true), Description("是否异步Fetch?\n异步Fetch可提升系统响应能力, 适用于静默加载数据而无需担心数据完整性的业务场景"), Category("Fetch")]
    public bool IsAsynchronousFetch
    {
      get { return _isAsynchronousFetch; }
      set { _isAsynchronousFetch = value; }
    }

    /// <summary>
    /// Fetch时是否需要缓存对象?
    /// </summary>
    [Description("Fetch时是否需要缓存对象?"), Category("Fetch")]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
    public bool CacheEnabled
    {
      get { return true; }
    }

    /// <summary>
    /// 分组名
    /// </summary>
    [DefaultValue(null), Description("分组名\n用于区分存在处理多组数据的情形\n与BarManager.GroupName一致时本PromptCodeBindingSourceStatus可被应用"), Category("Behavior")]
    public string GroupName { get; set; }

    #endregion

    #region 方法
    
    /// <summary>
    /// 必须用于Fetch功能?
    /// </summary>
    /// <param name="groupName">分组名</param>
    public bool MustFetchBy(string groupName)
    {
      return MustFetch || String.CompareOrdinal(GroupName, groupName) == 0;
    }

    #endregion
  }
}