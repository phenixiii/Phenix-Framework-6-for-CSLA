using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Windows.Forms;
using DevExpress.XtraEditors.DXErrorProvider;
using Phenix.Business;
using Phenix.Core;
using Phenix.Core.Mapping;
using Phenix.Core.Reflection;
using Phenix.Core.Windows;
using Phenix.Services.Client.Security;
using Phenix.Windows.Helper;

namespace Phenix.Windows
{
  /// <summary>
  /// 数据源状况
  /// </summary>
  [Designer(typeof(Phenix.Services.Client.Design.ComponentPropertyDesigner))]
  [DesignTimeVisible(false)]
  [ToolboxItem(false)]
  public sealed class BindingSourceStatus : Component
  {
    /// <summary>
    /// 初始化
    /// </summary>
    public BindingSourceStatus()
      : base() { }

    /// <summary>
    /// 初始化
    /// </summary>
    public BindingSourceStatus(IContainer container)
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
    }

    /// <summary>
    /// 数据源是从数据集
    /// </summary>
    [Description("数据源是从数据集"), Category("Data")]
    public bool BindingSourceIsDetail
    {
      get { return MasterBindingSource != null && MasterBindingSource != BindingSource; }
    }

    private BindingSource _masterBindingSource;
    /// <summary>
    /// 主数据源
    /// </summary>
    [DefaultValue(null), Description("主数据源\n非空时有效\n当BarManager.BindingSource变更为本BindingSource时将使用本属性替换BarManager.MasterBindingSource"), Category("Data")]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
    public BindingSource MasterBindingSource
    {
      get
      {
        if (BindingSource != null && BindingSource.DataSource is BindingSource && !String.IsNullOrEmpty(BindingSource.DataMember))
          return BindingSourceHelper.GetRootDataSource(BindingSource);
        return _masterBindingSource ?? BindingSource;
      }
      set { _masterBindingSource = value; }
    }

    /// <summary>
    /// 主数据源的类型
    /// </summary>
    [Description("主数据源的类型"), Category("Data")]
    public Type MasterBindingSourceType
    {
      get { return BindingSourceHelper.GetDataSourceType(MasterBindingSource); }
    }

    /// <summary>
    /// 主数据源项的类型
    /// </summary>
    [Description("主数据源List项的类型"), Category("Data")]
    public Type MasterBindingSourceItemType
    {
      get { return Utilities.FindListItemType(MasterBindingSourceType); }
    }

    private BindingSource _bindingSource;
    /// <summary>
    /// 数据源
    /// </summary>
    [DefaultValue(null), Description("数据源"), Category("Data")]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
    public BindingSource BindingSource
    {
      get { return _bindingSource; }
      set
      {
        _bindingSource = value;
        if (value == null)
          MasterBindingSource = null;
        if (DesignMode)
        {
          if (value != null && CriteriaBindingSource == null && Host != null)
          {
            BindingSource criteriaBindingSource = BindingSourceHelper.FindCriteriaBindingSource(Host.Container, value);
            if (criteriaBindingSource != null)
              CriteriaBindingSource = criteriaBindingSource;
          }
        }
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
    /// 数据源项的核心类型
    /// </summary>
    [Description("数据源项的类型"), Category("Data")]
    public Type BindingSourceCoreType
    {
      get { return BindingSourceHelper.GetDataSourceCoreType(BindingSource); }
    }

    private BindingSource _criteriaBindingSource;
    /// <summary>
    /// 条件数据源
    /// </summary>
    [DefaultValue(null), Description("条件数据源\n设定检索BindingSource数据集时的条件"), Category("Data")]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
    public BindingSource CriteriaBindingSource
    {
      get { return _criteriaBindingSource; }
      set
      {
        if (DesignMode)
        {
          if (value != null)
          {
            bool? dataSourceIsEnumerable = BindingSourceHelper.DataSourceIsEnumerable(value);
            if (dataSourceIsEnumerable.HasValue && dataSourceIsEnumerable.Value)
              throw new InvalidOperationException(String.Format("{0}不符合条件数据源要求: 非集合的对象或类定义", value.GetType().FullName));
          }
        } 
        _criteriaBindingSource = value;
      }
    }

    /// <summary>
    /// 条件数据源的类型
    /// </summary>
    [Description("条件数据源的类型"), Category("Data")]
    public Type CriteriaBindingSourceType
    {
      get { return BindingSourceHelper.GetDataSourceType(CriteriaBindingSource); }
    }

    private CriteriaCombineControl _criteriaCombineControl;
    /// <summary>
    /// 查询组合框控件
    /// </summary>
    [DefaultValue(null), Description("查询组合框控件\n非空时有效\n当CriteriaBindingSource为空时可使用本属性指定控件CriteriaCombineControl.WorkingCriteriaExpression作为检索BindingSource数据集时的条件"), Category("Data")]
    public CriteriaCombineControl CriteriaCombineControl 
    { 
      get { return _criteriaCombineControl; }
      set
      {
        if (value != null && value.OperateClassType == null)
          value.OperateClassType = BindingSourceCoreType;
        _criteriaCombineControl = value;
      }
    }

    /// <summary>
    /// 是否自动刷新?
    /// </summary>
    [DefaultValue(null), Description("是否自动刷新?\n当数据源数据发生变动时, 本项BindingSource被自动Fetch, 仅适用于只读数据集&主数据集&异步Fetch"), Category("Fetch")]
    public bool? AutoRefresh { get; set; }
    /// <summary>
    /// 是否自动刷新?
    /// </summary>
    [Description("是否自动刷新?"), Category("Fetch")]
    public bool AutoRefreshValue
    {
      get
      {
        if (Editabled || BindingSourceIsDetail || !IsAsynchronousFetchValue)
        {
          if (AutoRefresh.HasValue)
            AutoRefresh = null;
          return false;
        }
        if (AutoRefresh.HasValue)
          return AutoRefresh.Value;
        return false;
      }
    }

    /// <summary>
    /// 是否自动Fetch?
    /// </summary>
    [DefaultValue(null), Description("是否自动Fetch?\n在界面Shown时, 本项BindingSource被自动Fetch, 仅适用于主数据集"), Category("Fetch")]
    public bool? AutoFetch { get; set; }
    /// <summary>
    /// 是否自动Fetch?
    /// </summary>
    [Description("是否自动Fetch?"), Category("Fetch")]
    public bool AutoFetchValue
    {
      get
      {
        if (BindingSourceIsDetail)
        {
          if (AutoFetch.HasValue)
            AutoFetch = null;
          return false;
        }
        if (AutoFetch.HasValue)
          return AutoFetch.Value;
        return !Editabled;
      }
    }

    /// <summary>
    /// 是否用于Fetch功能?
    /// </summary>
    [DefaultValue(null), Description("是否用于Fetch功能?\n根据GroupName分组管理, 仅适用于主数据集"), Category("Fetch")]
    public bool? InFetch { get; set; }
     /// <summary>
    /// 是否用于Fetch功能?
    /// </summary>
    [Description("是否用于Fetch功能?"), Category("Fetch")]
    public bool InFetchValue
    {
      get
      {
        if (BindingSourceIsDetail)
        {
          if (InFetch.HasValue)
            InFetch = null;
          return false;
        }
        if (InFetch.HasValue)
          return InFetch.Value;
        return true;
      }
    }

    /// <summary>
    /// 必须用于Fetch功能?
    /// </summary>
    [DefaultValue(null), Description("必须用于Fetch功能?\n不受GroupName分组管理, 仅适用于主数据集"), Category("Fetch")]
    public bool? MustFetch { get; set; }
    /// <summary>
    /// 必须用于Fetch功能?
    /// </summary>
    [Description("必须用于Fetch功能?"), Category("Fetch")]
    public bool MustFetchValue
    {
      get
      {
        if (BindingSourceIsDetail)
        {
          if (MustFetch.HasValue)
            MustFetch = null;
          return false;
        }
        if (MustFetch.HasValue)
          return MustFetch.Value;
        return false;
      }
    }

    /// <summary>
    /// 是否异步Fetch?
    /// </summary>
    [DefaultValue(null), Description("是否异步Fetch?\n异步Fetch可提升系统响应能力, 适用于静默加载数据而无需担心数据完整性的业务场景, 仅适用于只读数据集&主数据集"), Category("Fetch")]
    public bool? IsAsynchronousFetch { get; set; }
    /// <summary>
    /// 是否异步Fetch?
    /// </summary>
    [Description("是否异步Fetch?"), Category("Fetch")]
    public bool IsAsynchronousFetchValue
    {
      get
      {
        if (Editabled || BindingSourceIsDetail)
        {
          if (IsAsynchronousFetch.HasValue)
            IsAsynchronousFetch = null;
          return false;
        }
        if (IsAsynchronousFetch.HasValue)
          return IsAsynchronousFetch.Value;
        return AutoFetchValue;
      }
    }

    /// <summary>
    /// Fetch时是否需要缓存对象?
    /// </summary>
    [DefaultValue(null), Description("Fetch时是否需要缓存对象? 仅适用于只读数据集&主数据集"), Category("Fetch")]
    public bool? CacheEnabled { get; set; }
    /// <summary>
    /// Fetch时是否需要缓存对象?
    /// </summary>
    [Description("Fetch时是否需要缓存对象?"), Category("Fetch")]
    public bool CacheEnabledValue
    {
      get
      {
        if (Editabled || BindingSourceIsDetail)
        {
          if (CacheEnabled.HasValue)
            CacheEnabled = null;
          return false;
        }
        if (CacheEnabled.HasValue)
          return CacheEnabled.Value;
        return false;
      }
    }

    /// <summary>
    /// Fetch时是否包含禁用记录?
    /// </summary>
    [DefaultValue(null), Description("Fetch时是否包含禁用记录?\n所检索到的数据包含被FieldAttribute.IsDisabledColumn标注的字段值等于CodingStandards.DefaultDisabledTrueValue的记录"), Category("Fetch")]
    public bool? IncludeDisabled { get; set; }
    /// <summary>
    /// Fetch时是否包含禁用记录?
    /// </summary>
    [Description("Fetch时是否包含禁用记录?"), Category("Fetch")]
    public bool IncludeDisabledValue
    {
      get
      {
        if (IncludeDisabled.HasValue)
          return IncludeDisabled.Value;
        return false;
      }
    }

    /// <summary>
    /// 是否自动保存和恢复Criterian内容?
    /// </summary>
    [DefaultValue(false), Description("是否自动保存和恢复 Criterian 内容\n当窗体关闭时, 会自动保存CriteriaBindingSource 内容并在下次窗体开启时恢复"), Category("Behavior")]
    public bool AutoStoreCriteria { get; set; }

    internal bool DisposableCriteria { get; set; }

    /// <summary>
    /// 分组名
    /// </summary>
    [DefaultValue(null), Description("分组名\n用于区分存在处理多组数据的情形\n当与BarManager.GroupName一致时本BindingSourceStatus可被应用"), Category("Behavior")]
    public string GroupName { get; set; }

    private bool _editabled = true;
    /// <summary>
    /// 是否可编辑的?
    /// </summary>
    [DefaultValue(true), Description("是否可编辑的?\n将受到BarManager的编辑功能控制\n根据GroupName分组管理\nIsReadOnly的类始终是false"), Category("Behavior")]
    public bool Editabled
    {
      get
      {
        ClassMapInfo classMapInfo = ClassMemberHelper.GetClassMapInfo(BindingSourceCoreType);
        if (classMapInfo != null && classMapInfo.IsReadOnly)
          return false;
        return _editabled;
      }
      set { _editabled = value; }
    }

    /// <summary>
    /// 是否允许循环添加?
    /// </summary>
    [DefaultValue(null), Description("是否允许循环添加?\n设置为 true 时, 当用户点击Add功能按钮后将处于循环添加状态, 仅适用于可编辑数据集"), Category("Behavior")]
    public bool? AllowLoopAdd { get; set; }
    /// <summary>
    /// 是否允许循环添加?
    /// </summary>
    [Description("是否允许循环添加?"), Category("Behavior")]
    public bool AllowLoopAddValue
    {
      get
      {
        if (!Editabled)
        {
          if (AllowLoopAdd.HasValue)
            AllowLoopAdd = null;
          return false;
        }
        if (AllowLoopAdd.HasValue)
          return AllowLoopAdd.Value;
        return !BindingSourceIsDetail;
      }
    }

    /// <summary>
    /// 超灵敏重置授权?
    /// </summary>
    [DefaultValue(false), Description("超灵敏重置授权?\n设置为 true 时, 当BindingSource的Current对象属性值发生改变时也会重置一下相关控件的读写权限"), Category("Behavior")]
    public bool HyperResetAuthorization { get; set; }

    private Dictionary<Control, IList<Control>> _viewControls;
    private Dictionary<Control, IList<Control>> _editControls;

    private Dictionary<Control, IList<Control>> _criteriaControls;

    /// <summary>
    /// 当前业务对象集合
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
    [Browsable(false)]
    public IBusinessCollection CurrentBusinessList
    {
      get { return BindingSourceHelper.GetDataSourceList(BindingSource) as IBusinessCollection; }
    }

    /// <summary>
    /// 当前业务对象
    /// </summary>
    [DefaultValue(null), Browsable(false)]
    public IBusinessObject CurrentBusiness
    {
      get { return BindingSourceHelper.GetDataSourceCurrent(BindingSource) as IBusinessObject; }
      set
      {
        if (value != null && BindingSource == null)
          throw new InvalidOperationException("请先为 BindingSource 赋值后再将" + value.GetType().FullName + "赋值给 CurrentBusiness");
        if (BindingSource != null)
          BindingSource.Position = BindingSource.IndexOf(value);
      }
    }

    /// <summary>
    /// 当前条件对象
    /// </summary>
    [Browsable(false)]
    public ICriteria CurrentCriteria
    {
      get { return BindingSourceHelper.GetDataSourceCurrent(CriteriaBindingSource) as ICriteria; }
    }

    #endregion

    #region 方法

    /// <summary>
    /// 提取视图控件
    /// </summary>
    public IList<Control> GetViewControls(Control container)
    {
      if (container == null)
        return null;
      IList<Control> result;
      if (_viewControls == null)
        _viewControls = new Dictionary<Control, IList<Control>>();
      if (!_viewControls.TryGetValue(container, out result))
      {
        result = ControlHelper.FindViewControls(container, BindingSource);
        _viewControls[container] = result;
      }
      return result;
    }

    /// <summary>
    /// 提取编辑控件
    /// </summary>
    public IList<Control> GetEditControls(Control container)
    {
      if (container == null)
        return null;
      IList<Control> result;
      if (_editControls == null)
        _editControls = new Dictionary<Control, IList<Control>>();
      if (!_editControls.TryGetValue(container, out result))
      {
        result = ControlHelper.FindEditControls(container, BindingSource);
        _editControls[container] = result;
      }
      return result;
    }

    /// <summary>
    /// 提取条件控件
    /// </summary>
    public IList<Control> GetCriteriaControls(Control container)
    {
      if (container == null)
        return null;
      IList<Control> result;
      if (_criteriaControls == null)
        _criteriaControls = new Dictionary<Control, IList<Control>>();
      if (!_criteriaControls.TryGetValue(container, out result))
      {
        result = ControlHelper.FindEditControls(container, CriteriaBindingSource);
        _criteriaControls[container] = result;
      }
      return result;
    }

    /// <summary>
    /// 重置控件容器内控件的读写授权
    /// </summary>
    /// <param name="container">控件容器</param>
    /// <param name="editMode">编辑状态</param>
    /// <param name="readOnly">只读</param>
    public void ResetControlAuthorizationRules(Control container, bool editMode, bool readOnly)
    {
      ReadWriteAuthorization.ResetControlAuthorizationRules(GetViewControls(container), editMode, readOnly, null);
      ReadWriteAuthorization.ResetControlAuthorizationRules(GetEditControls(container), editMode, readOnly, null);
    }

    /// <summary>
    /// 校验控件容器内编辑控件的失效数据
    /// </summary>
    /// <param name="container">控件容器</param>
    /// <param name="toFocused">聚焦失效控件</param>
    /// <param name="errorProvider">DXErrorProvider</param>
    /// <returns>失效数据事件数据</returns>
    public DataInvalidEventArgs CheckRules(Control container, bool toFocused, DXErrorProvider errorProvider)
    {
      if (container == null)
        return null;
      foreach (Control item in GetEditControls(container))
      {
        DataInvalidEventArgs result = errorProvider.CheckRule(item, toFocused);
        if (result != null)
          return result;
      }
      foreach (Control item in GetCriteriaControls(container))
      {
        DataInvalidEventArgs result = errorProvider.CheckRule(item, toFocused);
        if (result != null)
          return result;
      }
      return null;
    }

    /// <summary>
    /// 重置ErrorProvider
    /// </summary>
    /// <param name="container">控件容器</param>
    /// <param name="errorProvider">DXErrorProvider</param>
    public void ResetErrorProvider(Control container, DXErrorProvider errorProvider)
    {
      if (container == null)
        return;
      foreach (Control item in GetEditControls(container))
        errorProvider.SetError(item, String.Empty, ErrorType.None);
      foreach (Control item in GetCriteriaControls(container))
        errorProvider.SetError(item, String.Empty, ErrorType.None);
    }

    /// <summary>
    /// 必须用于Fetch功能?
    /// </summary>
    /// <param name="groupName">分组名</param>
    public bool MustFetchBy(string groupName)
    {
      return MustFetchValue || String.CompareOrdinal(GroupName, groupName) == 0;
    }

    internal IBusiness FindBusinessRoot()
    {
      return BindingSourceHelper.GetDataSourceList(MasterBindingSource) as IBusiness ??
        BindingSourceHelper.GetDataSourceCurrent(MasterBindingSource) as IBusiness;
    }

    internal bool EqualGroupName(string groupName)
    {
      foreach (string s in ClassMemberHelper.GetGroupNames(BindingSourceType))
        if (String.Compare(s, groupName, StringComparison.OrdinalIgnoreCase) == 0)
          return true;
      return false;
    }

    #endregion
  }
}