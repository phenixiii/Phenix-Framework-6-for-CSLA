using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Data;
using System.Drawing;
using System.Drawing.Design;
using System.Reflection;
using System.Security.Authentication;
using System.Threading;
using System.Windows.Forms;
using DevExpress.Utils;
using DevExpress.XtraBars;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.DXErrorProvider;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraGrid.Views.Base;
using DevExpress.XtraLayout;
using DevExpress.XtraVerticalGrid;
using DevExpress.XtraVerticalGrid.Rows;
using Phenix.Business;
using Phenix.Core;
using Phenix.Core.Cache;
using Phenix.Core.ComponentModel;
using Phenix.Core.Log;
using Phenix.Core.Mapping;
using Phenix.Core.Reflection;
using Phenix.Core.Rule;
using Phenix.Core.Security;
using Phenix.Core.Windows;
using Phenix.Services.Client.Security;
using Phenix.Services.Client.Validation;
using Phenix.Windows.Helper;

namespace Phenix.Windows
{
  /// <summary>
  /// Bar管理器
  /// 
  /// 集成EditValidation、ReadWriteAuthorization组件
  /// 
  /// 通过当前BindingSource以及当前用户的权限, 控制Tools Bar、Status Bar的灰亮、显示内容及绑定数据控件的读写规则、验证规则等
  /// 
  /// Tools Bar上的按钮除了Help外，都有缺省的处理过程
  /// 
  /// 不推荐自行处理Tools Bar按钮事件, 可通过处理本组件的相关按钮的触发前事件、触发后事件来实现相应的过程
  /// 
  /// 在触发前事件里，可将e.Applied置为true, 屏蔽掉本组件的缺省处理过程, 由自己来掌控数据集的增删改等过程
  /// </summary>
  [Description("集成EditValidation、ReadWriteAuthorization组件, 通过BindingSources管理的数据源以及当前用户的权限,/n控制Tools Bar、Status Bar的灰亮、显示内容, 及当前BindingSource绑定数据控件的读写规则、验证规则等")]
  [Designer(typeof(BarManagerDesigner))]
  [ProvideProperty("FriendlyCaption", typeof(Control))] //友好性标签
  [ToolboxItem(true), ToolboxBitmap(typeof(BarManager), "Phenix.Windows.BarManager")]
  [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Maintainability", "CA1506:AvoidExcessiveClassCoupling")]
  public sealed partial class BarManager : DevExpress.XtraBars.BarManager, IExtenderProvider
  {
    /// <summary>
    /// 初始化
    /// </summary>
    public BarManager()
      : base()
    {
      InitializeComponent();
    }

    /// <summary>
    /// 初始化
    /// </summary>
    public BarManager(IContainer container)
      : base()
    {
      if (container == null)
        throw new ArgumentNullException("container"); 
      container.Add(this);

      InitializeComponent();
    }

    #region 属性

    private new bool DesignMode
    {
      get { return base.DesignMode || AppConfig.DesignMode; }
    }
    
    private readonly Dictionary<Control, Component> _friendlyCaptionSources = new Dictionary<Control, Component>();

    /// <summary>
    /// 所属容器
    /// </summary>
    [DefaultValue(null), Browsable(false)]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1721:PropertyNamesShouldNotMatchGetMethods")]
    public new Control Form
    {
      get { return base.Form; }
      set
      {
        base.Form = value;
        if (!DesignMode)
        {
          Form form = value as Form;
          if (form != null)
          {
            form.Shown += new EventHandler(Host_Shown);
            form.FormClosed += new FormClosedEventHandler(Host_FormClosed);
            form.FormClosing += new FormClosingEventHandler(Host_FormClosing);
          }
        }
      }
    }

    private bool _autoStoreGridControlLayout = true;
    /// <summary>
    /// 是否自动保存和恢复GridControl布局
    /// </summary>
    [DefaultValue(true), Description("是否自动保存和恢复 GridControl 布局\n自动保存所属容器关闭前的 GridControl 布局并在下次开启时恢复"), Category("Phenix")]
    public bool AutoStoreGridControlLayout
    {
      get { return _autoStoreGridControlLayout; }
      set { _autoStoreGridControlLayout = value; }
    }

    private bool _autoStoreLayoutControlLayout = true;
    /// <summary>
    /// 是否自动保存和恢复LayoutControl布局
    /// </summary>
    [DefaultValue(true), Description("是否自动保存和恢复 LayoutControl 布局\n自动保存所属容器关闭前的 LayoutControl 布局并在下次开启时恢复"), Category("Phenix")]
    public bool AutoStoreLayoutControlLayout
    {
      get { return _autoStoreLayoutControlLayout; }
      set { _autoStoreLayoutControlLayout = value; }
    }

    private readonly Dictionary<string, DataOperateState> _bindingSourceGroups = new Dictionary<string, DataOperateState>(StringComparer.Ordinal);
    private bool _autoRefreshBindingSources;
    private readonly Collection<BindingSourceStatus> _bindingSources = new Collection<BindingSourceStatus>();
    /// <summary>
    /// 数据源队列
    /// </summary>
    [Description("数据源队列\n是 BindingSource 的集合, 与界面的操作规则、控件的权限规则进行联动"), Category("Phenix")]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
    [Editor(typeof(Phenix.Services.Client.Design.CollectionEditor), typeof(UITypeEditor))]
    public Collection<BindingSourceStatus> BindingSources
    {
      get { return _bindingSources; }
    }

    private readonly Collection<EnumBindingSourceStatus> _enumBindingSources = new Collection<EnumBindingSourceStatus>();
    /// <summary>
    /// 枚举数据源队列
    /// </summary>
    [Description("枚举数据源队列\n负责为数据源自动填充 EnumKeyCaptionCollection 枚举数据集"), Category("Phenix")]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
    [Editor(typeof(Phenix.Services.Client.Design.CollectionEditor), typeof(UITypeEditor))]
    public Collection<EnumBindingSourceStatus> EnumBindingSources
    {
      get { return _enumBindingSources; }
    }

    private bool _autoRefreshPromptCodeBindingSources;
    private readonly Collection<PromptCodeBindingSourceStatus> _promptCodeBindingSources = new Collection<PromptCodeBindingSourceStatus>();
    /// <summary>
    /// 提示码数据源队列
    /// </summary>
    [Description("提示码数据源队列\n负责为数据源自动填充 PromptCodeCaptionCollection 提示码数据集"), Category("Phenix")]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
    [Editor(typeof(Phenix.Services.Client.Design.CollectionEditor), typeof(UITypeEditor))]
    public Collection<PromptCodeBindingSourceStatus> PromptCodeBindingSources
    {
      get { return _promptCodeBindingSources; }
    }

    private BindingSource _masterBindingSource;
    /// <summary>
    /// 主数据源
    /// </summary>
    [DefaultValue(null)]
    [Description("主数据源\n非空时以本数据源为root实现编辑、回滚、提交等数据操作")]
    [Category("Phenix")]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
    public BindingSource MasterBindingSource
    {
      get { return _masterBindingSource ?? BindingSource; }
      set { _masterBindingSource = value; }
    }

    private BindingSource _bindingSource;
    /// <summary>
    /// 数据源
    /// </summary>
    [DefaultValue(null), Description("数据源\n与界面的操作规则、控件的权限规则进行联动"), Category("Phenix")]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
    public BindingSource BindingSource
    {
      get { return _bindingSource; }
      set
      {
        if (value == _bindingSource)
          return;
        BindingSourceChangeEventArgs e = null;
        if (!DesignMode)
        {
          e = new BindingSourceChangeEventArgs(_bindingSource, value);
          OnBindingSourceChanging(e);
          if (e.Stop)
            return;
        }
        if (!DesignMode && _bindingSource != null)
        {
          _bindingSource.DataSourceChanged -= new EventHandler(BindingSource_DataSourceChanged);
          _bindingSource.ListChanged -= new ListChangedEventHandler(BindingSource_ListChanged);
          _bindingSource.CurrentItemChanged -= new EventHandler(BindingSource_CurrentItemChanged);
          _bindingSource.PositionChanged -= new EventHandler(BindingSource_PositionChanged);
        }
        _bindingSource = value;
        if (!DesignMode)
        {
          if (value != null)
          {
            Type type = value.DataSource as Type;
            if (type != null)
              if (String.IsNullOrEmpty(value.DataMember))
                foreach (BindingSourceStatus item in BindingSources)
                  if (item.BindingSource == value)
                  {
                    if (item.Editabled && !item.AutoFetchValue)
                      value.DataSource = Activator.CreateInstance(type, true);
                    break;
                  }
            value.DataSourceChanged += new EventHandler(BindingSource_DataSourceChanged);
            value.ListChanged += new ListChangedEventHandler(BindingSource_ListChanged);
            value.CurrentItemChanged += new EventHandler(BindingSource_CurrentItemChanged);
            value.PositionChanged += new EventHandler(BindingSource_PositionChanged);
            foreach (BindingSourceStatus item in BindingSources)
              if (item.BindingSource == value)
              {
                ApplyBindingSourceStatus(item);
                item.ResetControlAuthorizationRules(Form, EditMode || NotUndoable, ReadOnly);
                break;
              }
            if (OperateState == DataOperateState.None)
              OperateState = DataOperateState.Browse;
          }
          else
          {
            if (OperateState == DataOperateState.Browse)
              OperateState = DataOperateState.None;
          }
          ResetComponentAuthorizationRules();
          ResetRecordState();
          OnBindingSourceChanged(e);
        }
        else if (value != null && CriteriaBindingSource == null && Form != null)
        {
          BindingSource criteriaBindingSource = BindingSourceHelper.FindCriteriaBindingSource(Form.Container, value);
          if (criteriaBindingSource != null)
            CriteriaBindingSource = criteriaBindingSource;
        }
      }
    }

    /// <summary>
    /// 数据源项的核心类型
    /// </summary>
    [Browsable(false)]
    public Type BindingSourceCoreType
    {
      get { return BindingSourceHelper.GetDataSourceCoreType(BindingSource); }
    }

    private BindingSource _criteriaBindingSource;
    /// <summary>
    /// 条件数据源
    /// </summary>
    [DefaultValue(null), Description("条件数据源\n设定检索BindingSource数据集时的条件"), Category("Phenix")]
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

    private CriteriaCombineControl _criteriaCombineControl;
    /// <summary>
    /// 查询组合框控件
    /// </summary>
    [DefaultValue(null), Description("查询组合框控件\n非空时有效\n当CriteriaBindingSource为空时可使用本属性指定控件CriteriaCombineControl.WorkingCriteriaExpression作为检索BindingSource数据集时的条件"), Category("Phenix")]
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
    /// 输出报表的类型
    /// </summary>
    [DefaultValue(null), Description("输出报表的类型\n非空时有效"), Category("Phenix")]
    [Editor(typeof(ReportSelectorEditor), typeof(UITypeEditor))]
    public Type ReportType { get; set; }
    
    private string _groupName;
    /// <summary>
    /// 分组名
    /// </summary>
    [Browsable(false)]
    public string GroupName
    {
      get 
      {
        if (String.IsNullOrEmpty(_groupName))
          return null;
        return _groupName; 
      }
      private set 
      {
        if (String.CompareOrdinal(_groupName, value) == 0)
          return;
        _groupName = value;
        OnGroupNameChanged();
      }
    }

    private bool _bindingSourceEditabled = true;
    /// <summary>
    /// 是否允许编辑
    /// </summary>
    [Browsable(false)]
    public bool BindingSourceEditabled
    {
      get { return _bindingSourceEditabled; }
      private set { _bindingSourceEditabled = value; }
    }

    /// <summary>
    /// 是否允许循环添加
    /// </summary>
    [Browsable(false)]
    public bool BindingSourceAllowLoopAdd { get; private set; }

    /// <summary>
    /// 超灵敏重置授权?
    /// </summary>
    [Browsable(false)]
    public bool HyperResetAuthorization { get; private set; }

    private bool _autoEditOnFetched; 
    /// <summary>
    /// OnFetched时是否自动进入编辑状态
    /// </summary>
    [DefaultValue(false), Description("OnFetched时是否自动进入编辑状态?\n点击Fetch功能按钮检索到数据后自动点击Edit功能按钮"), Category("Phenix")]
    public bool AutoEditOnFetched
    {
      get { return _autoEditOnFetched; }
      set { _autoEditOnFetched = value; }
    }

    private bool _autoEditOnSaved; 
    /// <summary>
    /// OnSaved时是否自动进入编辑状态
    /// </summary>
    [DefaultValue(false), Description("OnSaved时是否自动进入编辑状态?\n点击Save功能按钮保存好数据后自动点击Edit功能按钮"), Category("Phenix")]
    public bool AutoEditOnSaved
    {
      get { return _autoEditOnSaved; }
      set { _autoEditOnSaved = value; }
    }

    private bool _autoEditOnEditControlByDoubleClick = true;
    /// <summary>
    /// 双击编辑控件时是否自动进入编辑状态
    /// </summary>
    [DefaultValue(true), Description("双击编辑控件时是否自动进入编辑状态?\n双击编辑控件后自动点击Edit功能按钮"), Category("Phenix")]
    public bool AutoEditOnEditControlByDoubleClick
    {
      get { return _autoEditOnEditControlByDoubleClick; }
      set { _autoEditOnEditControlByDoubleClick = value; }
    }

    private bool _allowMultistepSubmit = true;
    /// <summary>
    /// 允许多步提交
    /// </summary>
    [DefaultValue(true), Description("允许多步提交\n设置为 true 时, 允许连续处理 BindingSources 的多条记录"), Category("Phenix")]
    public bool AllowMultistepSubmit
    {
      get { return _allowMultistepSubmit; }
      set { _allowMultistepSubmit = value; }
    }

    private bool _allowMultistepSubmitMaster = true;
    /// <summary>
    /// 允许多步提交主业务
    /// </summary>
    [DefaultValue(true), Description("允许多步提交主业务\n设置为 true 且 AllowMultistepSubmit = true 时, 允许连续处理 MasterBindingSource 的多条记录"), Category("Phenix")]
    public bool AllowMultistepSubmitMaster
    {
      get { return _allowMultistepSubmitMaster; }
      set { _allowMultistepSubmitMaster = value; }
    }

    /// <summary>
    /// 处于多步提交模式
    /// </summary>
    [Browsable(false)]
    public bool InMultistepSubmitMode
    {
      get { return AllowMultistepSubmitMode(BindingSource); }
    }

    /// <summary>
    /// 需要单步提交时锁死游标
    /// </summary>
    [DefaultValue(false), Description("单步提交时锁死游标\n当 AllowMultistepSubmit = false 或者 AllowMultistepSubmitMaster = false 状态下设置为 true 时, 不允许移动当前编辑的游标"), Category("Phenix")]
    public bool NeedLockPositionInOnestepSubmit { get; set; }

    /// <summary>
    /// 当前业务对象集合
    /// </summary>
    [Browsable(false)]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
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
    /// 当前业务根对象
    /// </summary>
    [Browsable(false)]
    public IBusiness CurrentBusinessRoot
    {
      get
      {
        return BindingSourceHelper.GetDataSourceList(MasterBindingSource) as IBusiness ??
          BindingSourceHelper.GetDataSourceCurrent(MasterBindingSource) as IBusiness;
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

    private DataOperateState _operateState;
    /// <summary>
    /// 操作状态
    /// </summary>
    [Browsable(false)]
    public DataOperateState OperateState
    {
      get { return _operateState; }
      private set
      {
        bool changed = false;
        if (_operateState != value)
        {
          changed = true;
          _operateState = value;
          BarItem barItem = FindBarItem(BarItemId.DataOperateState);
          if (barItem != null)
            switch (_operateState)
            {
              case DataOperateState.None:
                barItem.ImageIndex = -1;
                break;
              case DataOperateState.Fetching:
              case DataOperateState.FetchSuspend:
              case DataOperateState.FetchAborted:
              case DataOperateState.Browse:
                barItem.ImageIndex = (int)BarItemId.Fetch - (int)BarItemId.Fetch;
                foreach (BindingSourceStatus item in BindingSources)
                  if (item.BindingSource != null &&
                    (item.Editabled || item.InFetchValue) &&
                    (item.MustFetchBy(GroupName) || item.BindingSource == BindingSource))
                  {
                    item.ResetControlAuthorizationRules(Form, EditMode || NotUndoable, true);
                    item.ResetErrorProvider(Form, _dxErrorProvider);
                  }
                break;
              case DataOperateState.Add:
                barItem.ImageIndex = (int)BarItemId.Add - (int)BarItemId.Fetch;
                InvokeShowHint(null);
                break;
              case DataOperateState.Modify:
                barItem.ImageIndex = (int)BarItemId.Modify - (int)BarItemId.Fetch;
                InvokeShowHint(null);
                break;
              case DataOperateState.Delete:
                barItem.ImageIndex = (int)BarItemId.Delete - (int)BarItemId.Fetch;
                InvokeShowHint(null);
                break;
            }
          if (BindingSource != null)
            foreach (BindingSourceStatus item in BindingSources)
              if (item.BindingSource == BindingSource)
              {
                _bindingSourceGroups[item.GroupName ?? String.Empty] = value;
                break;
              }
        }
        if (changed)
        {
          switch (_operateState)
          {
            case DataOperateState.Browse:
              ResetControlAuthorizationRules(true, new BindingSource[] { });
              break;
            case DataOperateState.Add:
            case DataOperateState.Modify:
            case DataOperateState.Delete:
              ResetControlAuthorizationRules(false, new BindingSource[] { });
              break;
          }
          OnOperateStateChanged();
        }
      }
    }

    /// <summary>
    /// 在编辑状态
    /// </summary>
    [Browsable(false)]
    public bool EditMode
    {
      get
      {
        return 
          OperateState == DataOperateState.Add ||
          OperateState == DataOperateState.Modify ||
          OperateState == DataOperateState.Delete;
      }
    }

    /// <summary>
    /// 更改过数据
    /// </summary>
    [Browsable(false)]
    public bool EditDirty
    {
      get
      {
        IBusiness currentBusinessRoot = CurrentBusinessRoot;
        return currentBusinessRoot != null && currentBusinessRoot.IsDirty;
      }
    }

    /// <summary>
    /// 不参与回滚机制并阻断Detail的回滚处理
    /// </summary>
    [Browsable(false)]
    public bool NotUndoable
    {
      get
      {
        IBusinessObject currentBusiness = CurrentBusiness;
        return currentBusiness != null && currentBusiness.NotUndoable;
      }
    }

    /// <summary>
    /// 在只读状态
    /// </summary>
    [Browsable(false)]
    public bool ReadOnly
    {
      get
      {
        if (!BindingSourceEditabled || (!EditMode && !NotUndoable))
          return true;
        IBusinessObject currentBusiness = CurrentBusiness;
        return currentBusiness == null || !currentBusiness.AllowEdit || (!currentBusiness.EditMode && !currentBusiness.NotUndoable) ||
          (!AllowMultistepSubmit && (BindingSource != EditBindingSource || currentBusiness != EditObject)) ||
          (AllowMultistepSubmit && !AllowMultistepSubmitMaster && BindingSource == MasterBindingSource && currentBusiness != EditObject);
      }
    }

    /// <summary>
    /// 允许Add
    /// </summary>
    [Browsable(false)]
    public bool AllowAdd
    {
      get
      {
        if (!BindingSourceEditabled)
          return false;
        IBusinessCollection currentBusinessList = CurrentBusinessList;
        return currentBusinessList != null && currentBusinessList.AllowAddItem &&
          ((!EditMode && !NotUndoable) || ((currentBusinessList.EditMode || currentBusinessList.NotUndoable) && InMultistepSubmitMode));
      }
    }

    /// <summary>
    /// 允许Modify
    /// </summary>
    [Browsable(false)]
    public bool AllowModify
    {
      get
      {
        if (!BindingSourceEditabled || EditMode || NotUndoable)
          return false;
        IBusinessObject currentBusiness = CurrentBusiness;
        IBusinessCollection currentBusinessList = CurrentBusinessList;
        IBusiness currentBusinessRoot = CurrentBusinessRoot;
        return currentBusiness != null && currentBusiness.AllowEdit &&
          (currentBusinessList == null || currentBusinessList.AllowEditItem) &&
          (currentBusinessRoot == null || (currentBusinessRoot is IBusinessObject ? ((IBusinessObject)currentBusinessRoot).AllowEdit : (!(currentBusinessRoot is IBusinessCollection) || ((IBusinessCollection)currentBusinessRoot).AllowEditItem)));
      }
    }

    /// <summary>
    /// 允许Delete
    /// </summary>
    [Browsable(false)]
    public bool AllowDelete
    {
      get
      {
        if (!BindingSourceEditabled)
          return false;
        IBusinessObject currentBusiness = CurrentBusiness;
        IBusinessCollection currentBusinessList = CurrentBusinessList;
        return currentBusiness != null && currentBusiness.AllowDelete &&
          ((!EditMode && !NotUndoable) || (currentBusinessList != null && (currentBusinessList.EditMode || currentBusinessList.NotUndoable) && InMultistepSubmitMode) || OperateState == DataOperateState.Modify);
      }
    }

    /// <summary>
    /// 当前游标
    /// </summary>
    [DefaultValue(-1), Browsable(false)]
    public int CurrentPosition
    {
      get
      {
        if (BindingSource == null)
          return -1;
        else
          return BindingSource.Position;
      }
      set
      {
        if (value != -1 && BindingSource == null)
          throw new InvalidOperationException("请先为 BindingSource 赋值后再为 CurrentPosition 赋值");
        BindingSource.Position = value;
      }
    }

    private readonly Dictionary<BindingSource, List<object>> _editObjectsCache = new Dictionary<BindingSource, List<object>>();

    private BindingSource _editBindingSource;
    /// <summary>
    /// 编辑数据源
    /// </summary>
    [Browsable(false)]
    public BindingSource EditBindingSource 
    {
      get
      {
        if (_editBindingSource != BindingSource)
          if (BindingSource != null)
            if (_editObjectsCache.ContainsKey(BindingSource))
              _editBindingSource = BindingSource;
        return _editBindingSource;
      } 
    }

    private object _editObject;
    /// <summary>
    /// 编辑对象
    /// </summary>
    [Browsable(false)]
    public object EditObject
    {
      get
      {
        BindingSource editBindingSource = EditBindingSource;
        if (editBindingSource != null)
        {
          object currentObject = BindingSourceHelper.GetDataSourceCurrent(editBindingSource);
          if (_editObject != currentObject)
          {
            List<object> editObjects;
            if (_editObjectsCache.TryGetValue(editBindingSource, out editObjects))
              if (editObjects.Count > 0)
                if (editObjects.Contains(currentObject))
                  _editObject = currentObject;
                else if (!editObjects.Contains(_editObject))
                  _editObject = editObjects[0];
          }
        }
        return _editObject;
      }
    }

    /// <summary>
    /// 编辑游标
    /// </summary>
    [Browsable(false)]
    public int EditPosition
    {
      get
      {
        BindingSource editBindingSource = EditBindingSource;
        return editBindingSource != null ? editBindingSource.IndexOf(EditObject) : -1;
      }
    }

    private bool _promptInFetching = true;
    /// <summary>
    /// 在编辑状态下检索数据时提示当前处于编辑状态且已更改过数据
    /// </summary>
    [DefaultValue(true), Description("在编辑状态下检索数据时提示当前处于编辑状态且已更改过数据, 防止误操作"), Category("Phenix")]
    public bool PromptInFetching
    {
      get { return _promptInFetching; }
      set { _promptInFetching = value; }
    }

    private bool _promptInDeleting = true;
    /// <summary>
    /// 删除前提示确认删除操作
    /// </summary>
    [DefaultValue(true), Description("删除前提示确认删除操作, 防止误操作"), Category("Phenix")]
    public bool PromptInDeleting
    {
      get { return _promptInDeleting; }
      set { _promptInDeleting = value; }
    }

    private bool _promptInCanceling = true;
    /// <summary>
    /// 取消前提示确认取消操作
    /// </summary>
    [DefaultValue(true), Description("取消前提示确认取消操作, 防止误操作"), Category("Phenix")]
    public bool PromptInCanceling
    {
      get { return _promptInCanceling; }
      set { _promptInCanceling = value; }
    }

    private bool _promptInSaved = true;
    /// <summary>
    /// 保存后提示保存成功
    /// </summary>
    [DefaultValue(true), Description("保存后提示保存成功, 以确认操作"), Category("Phenix")]
    public bool PromptInSaved
    {
      get { return _promptInSaved; }
      set { _promptInSaved = value; }
    }

    private bool _promptInClosing = true;
    /// <summary>
    /// 在编辑状态下关闭窗体时提示当前处于编辑状态且已更改过数据
    /// </summary>
    [DefaultValue(true), Description("在编辑状态下关闭窗体时提示当前处于编辑状态且已更改过数据, 防止误操作"), Category("Phenix")]
    public bool PromptInClosing
    {
      get { return _promptInClosing; }
      set { _promptInClosing = value; }
    }

    /// <summary>
    /// 需要过程锁独占窗体, 一旦发现被他人占用将提示 ProcessLockException 信息
    /// </summary>
    [DefaultValue(false), Description("需要过程锁独占窗体, 一旦发现被他人占用将提示 ProcessLockException 信息"), Category("Phenix")]
    public bool NeedLockProcess { get; set; }

    private bool _parallelFetch = true;
    /// <summary>
    /// 是否并行Fetch?
    /// </summary>
    [DefaultValue(true), Description("是否并行Fetch?\n默认为阻塞主线程下并行Fetch多个数据集的方法, 仅适用于主数据集且其IsAsynchronousFetch=false"), Category("Phenix")]
    public bool ParallelFetch
    {
      get { return _parallelFetch; }
      set { _parallelFetch = value; }
    }

    private int _inFetchBindingSourceCount;
    /// <summary>
    /// 是否处于Fetch中
    /// </summary>
    [Browsable(false)]
    public bool InFetchBindingSources
    {
      get { return _inFetchBindingSourceCount > 0; }
      private set
      {
        if (value)
        {
          Interlocked.Increment(ref _inFetchBindingSourceCount);
          this.Form.Cursor = Cursors.WaitCursor;
        }
        else
        {
          while (_inFetchBindingSourceCount > 1)
          {
            Application.DoEvents();
            Thread.Sleep(50);
          }
          Interlocked.Decrement(ref _inFetchBindingSourceCount);
          this.Form.Cursor = Cursors.Default;
        }
      }
    }

    private bool _bypassInvalidChecks;

    private DXErrorProvider _dxErrorProvider;

    private ExecuteAuthorization _executeAuthorization;
    private TreeListManager _treeListManager;

    private readonly object _setBindingSourceThreadsLock = new object();
    private SortedList<SetBindingSourceData, Thread> _setBindingSourceThreads;

    private readonly List<GridControl> _gridControls = new List<GridControl>();
    private readonly List<VGridControl> _vGridControls = new List<VGridControl>();
    private readonly List<LayoutControl> _layoutControls = new List<LayoutControl>();

    private static int? _locatePositionMaximum;
    /// <summary>
    /// 定位游标极限 
    /// 缺省为 1000
    /// </summary>
    public static int LocatePositionMaximum
    {
      get { return AppSettings.GetProperty(ref _locatePositionMaximum, 1000); }
      set { AppSettings.SetProperty(ref _locatePositionMaximum, value); }
    }

    private static bool? _toAddLast;
    /// <summary>
    /// Add到集合末尾?
    /// 缺省为 true
    /// </summary>
    public static bool ToAddLast
    {
      get { return AppSettings.GetProperty(ref _toAddLast, true); }
      set { AppSettings.SetProperty(ref _toAddLast, value); }
    }

    private readonly List<BarItem> _barItemSwitchs = new List<BarItem>();

    #endregion

    #region 扩展程序属性

    /// <summary>
    /// 友好性标签
    /// </summary>
    [DefaultValue(null), Description("本控件的友好性标签/n将被: 统一必输栏的关联标题、前景色"), Category("Phenix")]
    public Component GetFriendlyCaption(Control source)
    {
      Component result;
      if (!_friendlyCaptionSources.TryGetValue(source, out result) && DesignMode)
      {
        result = EditValidation.MateFriendlyCaption(source);
        if (result != null)
          _friendlyCaptionSources[source] = result;
      }
      return result;
    }

    /// <summary>
    /// 友好性标签
    /// </summary>
    public void SetFriendlyCaption(Control source, Component value)
    {
      _friendlyCaptionSources[source] = value;
    }

    #endregion

    #region 事件
    
    private event EventHandler<EventArgs> _initialized;
    /// <summary>
    /// 初始化Host的Shown之后触发
    /// </summary>
    [Description("初始化Host的Shown之后触发"), Category("Phenix")]
    public event EventHandler<EventArgs> Initialized
    {
      add
      {
        if (value == null)
          return;
        _initialized += value;
        if (!DesignMode)
          value(this, EventArgs.Empty);
      }
      remove { _initialized -= value; }
    }
    private void OnInitialized()
    {
      if (_initialized != null)
          _initialized.Invoke(this, EventArgs.Empty);
    }

    /// <summary>
    /// 属性BindingSource值发生更改时
    /// </summary>
    [Description("当属性 BindingSource 值更改时触发\n可将 e.Stop 置为 true 取消赋值"), Category("Phenix")]
    public event EventHandler<BindingSourceChangeEventArgs> BindingSourceChanging;
    private void OnBindingSourceChanging(BindingSourceChangeEventArgs e)
    {
      if (BindingSourceChanging != null)
        BindingSourceChanging.Invoke(this, e);
    }

    /// <summary>
    /// 属性BindingSource值发生更改后
    /// </summary>
    [Description("当属性 BindingSource 值更改后触发"), Category("Phenix")]
    public event EventHandler<BindingSourceChangeEventArgs> BindingSourceChanged;
    private void OnBindingSourceChanged(BindingSourceChangeEventArgs e)
    {
      if (BindingSourceChanged != null)
        BindingSourceChanged.Invoke(this, e);
    }

    private event EventHandler<EventArgs> _groupNameChanged;
    /// <summary>
    /// 属性GroupName值发生了更改
    /// </summary>
    [Description("当属性 GroupName 值更改之后触发"), Category("Phenix")]
    public event EventHandler<EventArgs> GroupNameChanged
    {
      add
      {
        if (value == null)
          return;
        _groupNameChanged += value;
        if (!DesignMode)
          value(this, EventArgs.Empty);
      }
      remove { _groupNameChanged -= value; }
    }
    private void OnGroupNameChanged()
    {
      if (_groupNameChanged != null)
        _groupNameChanged.Invoke(this, EventArgs.Empty);
    }

    private event EventHandler<EventArgs> _operateStateChanged;
    /// <summary>
    /// 属性OperateState值发生了更改
    /// </summary>
    [Description("当属性 OperateState 值更改之后触发"), Category("Phenix")]
    public event EventHandler<EventArgs> OperateStateChanged
    {
      add
      {
        if (value == null)
          return;
        _operateStateChanged += value;
        if (!DesignMode)
          value(this, EventArgs.Empty);
      }
      remove { _operateStateChanged -= value; }
    }
    private void OnOperateStateChanged()
    {
      if (_operateStateChanged != null)
        _operateStateChanged.Invoke(this, EventArgs.Empty);
    }

    private event EventHandler<EventArgs> _rulesApplied;
    /// <summary>
    /// 规则已被应用
    /// </summary>
    [Description("当规则被应用之后触发"), Category("Phenix")]
    public event EventHandler<EventArgs> RulesApplied
    {
      add
      {
        if (value == null)
          return;
        _rulesApplied += value;
        if (!DesignMode)
          value(this, EventArgs.Empty);
      }
      remove { _rulesApplied -= value; }
    }
    private void OnRulesApplied()
    {
      if (_rulesApplied != null)
        _rulesApplied.Invoke(this, EventArgs.Empty);
    }

    /// <summary>
    /// 数据失效
    /// </summary>
    [Description("当数据源数据发生错误之后触发"), Category("Phenix")]
    public event EventHandler<DataInvalidEventArgs> DataInvalid;
    private void OnDataInvalid(DataInvalidEventArgs e)
    {
      if (DataInvalid != null)
        DataInvalid.Invoke(this, e);
    }

    /// <summary>
    /// 保存失败
    /// </summary>
    [Description("当保存数据源数据失败之后触发"), Category("Phenix")]
    public event EventHandler<ExceptionEventArgs> SaveFailed;
    private void OnSaveFailed(ExceptionEventArgs e)
    {
      if (SaveFailed != null)
        SaveFailed.Invoke(this, e);
    }

    #region BindingSource 事件

    private void BindingSource_DataSourceChanged(object sender, EventArgs e)
    {
      if (InFetchBindingSources)
        return;
      ResetControlAuthorizationRules((BindingSource)sender);
      ResetComponentAuthorizationRules();
      ResetRecordState();
    }

    private void BindingSource_ListChanged(object sender, ListChangedEventArgs e)
    {
      if (InFetchBindingSources)
        return;
      BindingSource source = (BindingSource)sender;
      if ((EditMode || NotUndoable) && AllowMultistepSubmitMode(source))
        SetEditObject(source);
      ResetControlAuthorizationRules(source);
      ResetComponentAuthorizationRules();
      ResetRecordState();
    }

    private void BindingSource_CurrentItemChanged(object sender, EventArgs e)
    {
      if (InFetchBindingSources)
        return;
      if (HyperResetAuthorization)
        ResetControlAuthorizationRules((BindingSource)sender);
      ResetRecordState();
    }

    private void BindingSource_PositionChanged(object sender, EventArgs e)
    {
      if (InFetchBindingSources)
        return;
      BindingSource source = (BindingSource)sender;
      if (EditMode || NotUndoable)
      {
        if (AllowMultistepSubmitMode(source))
          SetEditObject(source);
        else if (NeedLockPositionInOnestepSubmit && BindingSource == EditBindingSource && CurrentPosition != EditPosition)
        {
          if (MessageBox.Show(Phenix.Windows.Properties.Resources.LockingPosition,
            Form.Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            if (ClickCancelButton().Succeed)
              return;
          Thread thread = new Thread(InvokeClickLocateButton);
          thread.IsBackground = true;
          thread.Start();
          return;
        }
      }
      else
      {
        if (source.Position == source.Count - 1)
        {
          IBusinessCollectionPage page = BindingSourceHelper.GetDataSourceList(source) as IBusinessCollectionPage;
          if (page != null)
            page.FetchNextPage();
        }
      }
      ResetControlAuthorizationRules(source);
      ResetComponentAuthorizationRules();
      ResetRecordState();
      ShowExecuteActionInfo();
    }
    
    #endregion

    #region EnumBindingSource 事件

    private void EnumBindingSource_DataSourceChanged(object sender, EventArgs e)
    {
      BindingSource bindingSource = (BindingSource)sender;
      EnumKeyCaptionCollection entityCollection = bindingSource.DataSource as EnumKeyCaptionCollection;
      if (entityCollection == null)
        return;
      if (EditValidation.NeedResetFriendlyCaption)
        EditValidation.ResetGridFriendlyCaptions(Form, bindingSource, entityCollection.Caption);
    }

    #endregion

    #region Control 事件

    private void Control_Enter(object sender, EventArgs e)
    {
      BindingSource bindingSource = null;
      foreach (Binding item in ((Control)sender).DataBindings)
      {
        BindingSource itemBindingSource = item.DataSource as BindingSource;
        if (itemBindingSource != null)
        {
          bindingSource = itemBindingSource;
          break;
        }
      }
      if (bindingSource == null)
        bindingSource = BindingSourceHelper.GetDataSource(sender);
      if (bindingSource != null)
        foreach (BindingSourceStatus item in BindingSources)
          if (item.BindingSource == bindingSource || item.CriteriaBindingSource == bindingSource)
          {
            if (item.Editabled)
              BindingSource = item.BindingSource;
            break;
          }
    }

    private void Control_Validated(object sender, EventArgs e)
    {
      if (_bypassInvalidChecks)
        return;
      Control control = (Control)sender;
      if (!control.Enabled || !control.Visible)
        return;
      foreach (Binding item in control.DataBindings)
        if (item.DataSource != null)
          if (item.DataSource == BindingSource && !EditMode && !NotUndoable || item.DataSource == CriteriaBindingSource)
          {
            Thread thread = new Thread(ExecuteCheckValid);
            thread.IsBackground = true;
            thread.Start(control);
            break;
          }
    }

    private void TextEdit_ParseEditValue(object sender, DevExpress.XtraEditors.Controls.ConvertEditValueEventArgs e)
    {
      if (e.Value == null || String.IsNullOrEmpty(e.Value.ToString()))
        e.Value = DBNull.Value;
    }

    private void TextEdit_FormatEditValue(object sender, DevExpress.XtraEditors.Controls.ConvertEditValueEventArgs e)
    {
      if (e.Value == null || e.Value == DBNull.Value)
        e.Value = String.Empty;
    }

    private void LookUpEdit_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
    {
      RepositoryItemLookUpEdit_ButtonClick(((LookUpEditBase)sender).Properties, e);
    }

    private void RepositoryItemLookUpEdit_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
    {
      if (e.Button.Kind == DevExpress.XtraEditors.Controls.ButtonPredefines.Plus ||
        e.Button.Kind == DevExpress.XtraEditors.Controls.ButtonPredefines.Delete)
      {
        RepositoryItemLookUpEditBase repositoryItemLookUpEdit = (RepositoryItemLookUpEditBase)sender;
        foreach (PromptCodeBindingSourceStatus item in PromptCodeBindingSources)
          if (item.BindingSource == repositoryItemLookUpEdit.DataSource)
          {
            if (e.Button.Kind == DevExpress.XtraEditors.Controls.ButtonPredefines.Plus)
            {
              string value;
              if (PromptCodeEditDialog.ExecuteAdd(item.BindingSource.DataSource as PromptCodeKeyCaptionCollection, item.DefaultReadLevel, out value))
                repositoryItemLookUpEdit.OwnerEdit.EditValue = value;
            }
            else if (e.Button.Kind == DevExpress.XtraEditors.Controls.ButtonPredefines.Delete)
            {
              if (PromptCodeEditDialog.ExecuteDelete(item.BindingSource.DataSource as PromptCodeKeyCaptionCollection,
                repositoryItemLookUpEdit.OwnerEdit.EditValue as string))
                repositoryItemLookUpEdit.OwnerEdit.EditValue = null;
            }
            break;
          }
      }
    }

    private void CheckedComboBoxEdit_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
    {
      RepositoryItemCheckedComboBoxEdit_ButtonClick(((CheckedComboBoxEdit)sender).Properties, e);
    }

    private void RepositoryItemCheckedComboBoxEdit_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
    {
      if (e.Button.Kind == DevExpress.XtraEditors.Controls.ButtonPredefines.Plus ||
        e.Button.Kind == DevExpress.XtraEditors.Controls.ButtonPredefines.Delete)
      {
        RepositoryItemCheckedComboBoxEdit repositoryItemCheckedComboBoxEdit = (RepositoryItemCheckedComboBoxEdit)sender;
        foreach (PromptCodeBindingSourceStatus item in PromptCodeBindingSources)
          if (item.BindingSource == repositoryItemCheckedComboBoxEdit.DataSource)
          {
            if (e.Button.Kind == DevExpress.XtraEditors.Controls.ButtonPredefines.Plus)
            {
              string value;
              if (PromptCodeEditDialog.ExecuteAdd(item.BindingSource.DataSource as PromptCodeKeyCaptionCollection, item.DefaultReadLevel, out value))
                repositoryItemCheckedComboBoxEdit.OwnerEdit.EditValue = value;
            }
            else if (e.Button.Kind == DevExpress.XtraEditors.Controls.ButtonPredefines.Delete)
            {
              if (PromptCodeEditDialog.ExecuteDelete(item.BindingSource.DataSource as PromptCodeKeyCaptionCollection,
                repositoryItemCheckedComboBoxEdit.OwnerEdit.EditValue as string))
                repositoryItemCheckedComboBoxEdit.OwnerEdit.EditValue = null;
            }
            break;
          }
      }
    }

    private void ColumnView_CellValueChanging(object sender, CellValueChangedEventArgs e)
    {
      if (String.CompareOrdinal(e.Column.FieldName, "Selected") == 0 && e.Value is bool)
      {
        IBusinessObject business = e.Column.View.GetRow(e.RowHandle) as IBusinessObject;
        if (business != null)
          business.Selected = (bool)e.Value;
      }
    }
    
    private void VGridControl_CellValueChanging(object sender, DevExpress.XtraVerticalGrid.Events.CellValueChangedEventArgs e)
    {
      if (String.CompareOrdinal(e.Row.Properties.FieldName, "Selected") == 0 && e.Value is bool)
      {
        IBusinessObject business = e.Row.Grid.GetRecordObject(e.Row.Grid.FocusedRecord) as IBusinessObject;
        if (business != null)
          business.Selected = (bool)e.Value;
      }
    }

    private void EditControl_KeyPress(object sender, KeyPressEventArgs e)
    {
      if (e.KeyChar == '\r' && BindingSourceAllowLoopAdd && OperateState == DataOperateState.Add)
      {
        Control control = (Control)sender;
        if (BindingSource != null)
          foreach (Binding item in control.DataBindings)
            if (item.DataSource == BindingSource)
            {
              if (InMultistepSubmitMode || ClickSaveButton(false).Succeed)
                ClickAddButton();
              break;
            }
      }
    }

    private void EditControl_DoubleClick(object sender, EventArgs e)
    {
      if (AutoEditOnEditControlByDoubleClick && OperateState == DataOperateState.Browse)
        ClickModifyButton();
    }

    private void ColumnView_KeyPress(object sender, KeyPressEventArgs e)
    {
      if (e.KeyChar == '\r' && BindingSourceAllowLoopAdd && (EditMode || NotUndoable))
      {
        ColumnView columnView = (ColumnView)sender;
        if (columnView.OptionsBehavior.Editable && !columnView.OptionsBehavior.ReadOnly &&
          columnView.FocusedColumn != null && columnView.FocusedColumn.AbsoluteIndex == columnView.Columns.Count - 1 &&
          columnView.GetRow(columnView.FocusedRowHandle) == BindingSourceHelper.GetDataSourceCurrent(BindingSource))
          if (OperateState == DataOperateState.Add)
          {
            if (InMultistepSubmitMode || ClickSaveButton(false).Succeed)
            {
              if (!columnView.IsLastRow)
                columnView.MovePrev();
              ClickAddButton();
            }
          }
          else
          {
            if (InMultistepSubmitMode)
              columnView.MoveNext();
            columnView.FocusedColumn = columnView.Columns[0];
          }
      }
    }

    private void ColumnView_MouseWheel(object sender, MouseEventArgs e)
    {
      ColumnView columnView = (ColumnView)sender;
      if (!columnView.IsEditing)
        return;
      if (columnView.FocusedColumn != null && columnView.FocusedColumn.ColumnEdit != null)
        return;
      if (e.Delta > 0)
      {
        SendKeys.Send("{UP}");
      }
      else if (e.Delta < 0)
      {
        SendKeys.Send("{DOWN}");
      }
    }

    private void GridControl_FocusedViewChanged(object sender, ViewFocusEventArgs e)
    {
      ColumnViewPositionChanged((ColumnView)e.View);
    }

    private void ColumnView_ShownEditor(object sender, EventArgs e)
    {
      ColumnViewPositionChanged((ColumnView)sender);
    }

    private void ColumnViewPositionChanged(ColumnView columnView)
    {
      if (columnView == null)
        return;
      if (columnView.DataSource is BindingSource)
        BindingSource = (BindingSource)columnView.DataSource;
      else
      {
        List<BaseView> baseViews = new List<BaseView>();
        BaseView c = columnView;
        while (c != null)
        {
          baseViews.Add(c);
          c = c.ParentView;
        }
        for (int i = baseViews.Count - 1; i >= 0; i--)
        {
          ColumnView p = baseViews[i] as ColumnView;
          if (p == null || p.DataSource == null)
            continue;
          ColumnView d = i > 0 ? baseViews[i - 1] as ColumnView : null;
          foreach (BindingSourceStatus item in BindingSources)
            if (item.BindingSource == p.DataSource || item.CurrentBusinessList == p.DataSource)
              try
              {
                if (d != null)
                {
                  IBusinessObject obj = (IBusinessObject)d.GetRow(d.FocusedRowHandle);
                  if (obj != null && obj.MasterBusiness != null)
                  {
                    item.CurrentBusiness = obj.MasterBusiness;
                    break;
                  }
                }
                item.CurrentBusiness = (IBusinessObject)p.GetRow(p.FocusedRowHandle);
                break;
              }
              finally
              {
                if (item.Editabled)
                  BindingSource = item.BindingSource;
              }
        }
      }
      ReadWriteAuthorization.ResetControlAuthorizationRules(columnView.GridControl, EditMode || NotUndoable, ReadOnly, null);
    }

    #endregion

    #region ObjectCache 事件

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
    private void ObjectCache_Cleared(string groupName)
    {
      foreach (BindingSourceStatus item in BindingSources)
        if (item.BindingSource != null && item.AutoRefreshValue && item.EqualGroupName(groupName))
          try
          {
            Fetch(item, true, true);
          }
          catch (Exception ex)
          {
            string hint = AppUtilities.GetErrorHint(ex, typeof(Csla.DataPortalException), typeof(Csla.Reflection.CallMethodException));
            hint = String.Format(Phenix.Windows.Properties.Resources.DataFetchAborted, hint);
            InvokeShowHint(hint);
          }
    }

    #endregion

    #region PromptCode 事件

    private void DataRuleHub_PromptCodeChanged(string promptCodeName)
    {
      foreach (PromptCodeBindingSourceStatus item in PromptCodeBindingSources)
        if (item.AutoRefresh && String.CompareOrdinal(item.PromptCodeName, promptCodeName) == 0)
          FetchPromptCode(item);
    }

    #endregion

    #region Host 事件

    private void Host_Shown(object sender, EventArgs e)
    {
      if (UserIdentity.CurrentIdentity == null)
        return;

      if (NeedLockProcess)
        try
        {
          UserIdentity.CurrentIdentity.LockProcess(Form.GetType().FullName, Form.Text, Phenix.Windows.Properties.Resources.LockProcessHint);
        }
        catch (ProcessLockException ex)
        {
          foreach (Control item in Form.Controls)
            item.Enabled = false;
          MessageBox.Show(ex.Message, Form.Text, MessageBoxButtons.OK, MessageBoxIcon.Warning);
          return;
        }

      //初始化
      InitializeHelperComponent();
      InitializeBindingSources();
      InitializeFetchEnumBindingSources();
      InitializeFetchPromptCodeBindingSources();
      InitializeEvent();
      ResetComponentAuthorizationRules();
      ResetRecordState();
      //拦截ObjectCache事件
      if (_autoRefreshBindingSources)
        foreach (BindingSourceStatus item in BindingSources)
          if (item.AutoRefreshValue && item.BindingSourceType != null)
            ObjectCache.Register(item.BindingSourceType, ObjectCache_Cleared);
      //拦截PromptCode事件
      if (_autoRefreshPromptCodeBindingSources)
        DataRuleHub.PromptCodeChanged += new PromptCodeChangedEventHandler(DataRuleHub_PromptCodeChanged);
      //恢复Layout
      if (AppConfig.AutoStoreLayout)
      {
        //for Developer Express .NET
        if (AutoStoreGridControlLayout)
        {
          foreach (GridControl item in _gridControls)
            GridControlHelper.RestoreLayout(Form, item);
          foreach (VGridControl item in _vGridControls)
            VGridControlHelper.RestoreLayout(Form, item);
        }
        if (AutoStoreLayoutControlLayout)
          foreach (LayoutControl item in _layoutControls)
            LayoutControlHelper.RestoreLayout(Form, item);
      }
      AnalyseFormWorkSource();
      FocusControl(BindingSource);

      OnInitialized();
    }

    private void Host_FormClosed(object sender, FormClosedEventArgs e)
    {
      if (UserIdentity.CurrentIdentity == null)
        return;

      //撤销ObjectCache事件
      if (_autoRefreshBindingSources)
        foreach (BindingSourceStatus item in BindingSources)
          if (item.AutoRefreshValue && item.BindingSourceType != null)
            ObjectCache.Unregister(item.BindingSourceType, ObjectCache_Cleared);
      //撤销PromptCode事件
      if (_autoRefreshPromptCodeBindingSources)
        DataRuleHub.PromptCodeChanged -= new PromptCodeChangedEventHandler(DataRuleHub_PromptCodeChanged);
      //终止下载绑定数据线程
      AbortExecuteSetBindingSource();
      //保存Layout
      if (AppConfig.AutoStoreLayout)
      {
        //for Developer Express .NET
        if (AutoStoreGridControlLayout)
        {
          foreach (GridControl item in _gridControls)
            GridControlHelper.SaveLayout(Form, item);
          foreach (VGridControl item in _vGridControls)
            VGridControlHelper.SaveLayout(Form, item);
        }
        //for Developer Express .NET
        if (AutoStoreLayoutControlLayout)
          foreach (LayoutControl item in _layoutControls)
            LayoutControlHelper.SaveLayout(Form, item);
      }

      if (NeedLockProcess)
        try
        {
          UserIdentity.CurrentIdentity.UnlockProcess(Form.GetType().FullName);
        }
        catch (ProcessLockException)
        {
        }
    }

    private void Host_FormClosing(object sender, FormClosingEventArgs e)
    {
      if (UserIdentity.CurrentIdentity == null)
        return;

      if (PromptInClosing && e.CloseReason == CloseReason.UserClosing)
        if (EditDirty)
          if (MessageBox.Show(Phenix.Windows.Properties.Resources.ConfirmExit, Phenix.Windows.Properties.Resources.DataModifying,
            MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.No)
            e.Cancel = true;
          else
            e.Cancel = !DoCancel(true);
    }

    #endregion

    #region ToolBar 事件

    private void BarManager_ItemClick(object sender, ItemClickEventArgs e)
    {
      //if (UserIdentity.CurrentIdentity.IsAdmin || UserIdentity.CurrentIdentity.HaveAdminRole)
      //  if ((BarItemId)e.Item.Id != BarItemId.Fetch && (BarItemId)e.Item.Id != BarItemId.Exit)
      //    return;

      switch ((BarItemId)e.Item.Id)
      {
        case BarItemId.Fetch:
          ClickFetchButton(e);
          break;
        case BarItemId.Reset:
          ClickResetButton(e);
          break;
        case BarItemId.Restore:
          ClickRestoreButton(e);
          break;
        case BarItemId.Add:
          ClickAddButton(e);
          break;
        case BarItemId.AddClone:
          ClickAddCloneButton(e);
          break;
        case BarItemId.Modify:
          ClickModifyButton(e);
          break;
        case BarItemId.Delete:
          ClickDeleteButton(e);
          break;
        case BarItemId.Locate:
          ClickLocateButton(e);
          break;
        case BarItemId.Cancel:
          ClickCancelButton(e);
          break;
        case BarItemId.Save:
          ClickSaveButton(e);
          break;
        case BarItemId.Import:
          ClickImportButton(e);
          break;
        case BarItemId.Export:
          ClickExportButton(e);
          break;
        case BarItemId.Print:
          ClickPrintButton(e);
          break;
        case BarItemId.Help:
          ClickHelpButton(e);
          break;
        case BarItemId.Exit:
          ClickExitButton(e);
          break;
      }
    }

    /// <summary>
    /// 检索按钮触发前事件
    /// </summary>
    [Description("当点击检索按钮时触发\n可将 e.Applied 置为 true 屏蔽掉本组件的缺省处理过程"), Category("ToolBar")]
    public event EventHandler<BarItemClickEventArgs> Fetching;

    /// <summary>
    /// 检索按钮触发后事件
    /// </summary>
    [Description("当点击检索按钮的过程处理之后触发"), Category("ToolBar")]
    public event EventHandler<BarItemClickEventArgs> Fetched;

    /// <summary>
    /// 复位按钮触发前事件
    /// </summary>
    [Description("当点击复位按钮时触发\n可将 e.Applied 置为 true 屏蔽掉本组件的缺省处理过程"), Category("ToolBar")]
    public event EventHandler<BarItemClickEventArgs> Resetting;

    /// <summary>
    /// 复位按钮触发后事件
    /// </summary>
    [Description("当点击复位按钮的过程处理之后触发"), Category("ToolBar")]
    public event EventHandler<BarItemClickEventArgs> Reset;

    /// <summary>
    /// 恢复按钮触发前事件
    /// </summary>
    [Description("当点击恢复按钮时触发\n可将 e.Applied 置为 true 屏蔽掉本组件的缺省处理过程"), Category("ToolBar")]
    public event EventHandler<BarItemClickEventArgs> Restoring;

    /// <summary>
    /// 恢复按钮触发后事件
    /// </summary>
    [Description("当点击恢复按钮的过程处理之后触发"), Category("ToolBar")]
    public event EventHandler<BarItemClickEventArgs> Restored;

    /// <summary>
    /// 新增按钮触发前事件
    /// </summary>
    [Description("当点击新增按钮时触发\n可将 e.Applied 置为 true 屏蔽掉本组件的缺省处理过程"), Category("ToolBar")]
    public event EventHandler<BarItemClickEventArgs> Adding;

    /// <summary>
    /// 新增按钮触发后事件
    /// </summary>
    [Description("当点击新增按钮的过程处理之后触发"), Category("ToolBar")]
    public event EventHandler<BarItemClickEventArgs> Added;

    /// <summary>
    /// 克隆按钮触发前事件
    /// </summary>
    [Description("当点击克隆按钮时触发\n可将 e.Applied 置为 true 屏蔽掉本组件的缺省处理过程"), Category("ToolBar")]
    public event EventHandler<BarItemClickEventArgs> AddCloning;

    /// <summary>
    /// 克隆按钮触发后事件
    /// </summary>
    [Description("当点击克隆按钮的过程处理之后触发"), Category("ToolBar")]
    public event EventHandler<BarItemClickEventArgs> AddCloned;

    /// <summary>
    /// 编辑按钮触发前事件
    /// </summary>
    [Description("当点击编辑按钮时触发\n可将 e.Applied 置为 true 屏蔽掉本组件的缺省处理过程"), Category("ToolBar")]
    public event EventHandler<BarItemClickEventArgs> Modifying;

    /// <summary>
    /// 编辑按钮触发后事件
    /// </summary>
    [Description("当点击编辑按钮的过程处理之后触发"), Category("ToolBar")]
    public event EventHandler<BarItemClickEventArgs> Modified;

    /// <summary>
    /// 删除按钮触发前事件
    /// </summary>
    [Description("当点击删除按钮时触发\n可将 e.Applied 置为 true 屏蔽掉本组件的缺省处理过程"), Category("ToolBar")]
    public event EventHandler<BarItemDeleteEventArgs> Deleting;

    /// <summary>
    /// 删除按钮触发后事件
    /// </summary>
    [Description("当点击删除按钮的过程处理之后触发"), Category("ToolBar")]
    public event EventHandler<BarItemDeleteEventArgs> Deleted;

    /// <summary>
    /// 删除按钮触发提示被否决后事件
    /// </summary>
    [Description("当点击删除按钮提示被否决之后触发"), Category("ToolBar")]
    public event EventHandler<BarItemDeleteEventArgs> DeleteRejected;

    /// <summary>
    /// 定位按钮触发前事件
    /// </summary>
    [Description("当点击定位按钮时触发\n可将 e.Applied 置为 true 屏蔽掉本组件的缺省处理过程"), Category("ToolBar")]
    public event EventHandler<BarItemClickEventArgs> Locating;

    /// <summary>
    /// 定位按钮触发后事件
    /// </summary>
    [Description("当点击定位按钮的过程处理之后触发"), Category("ToolBar")]
    public event EventHandler<BarItemClickEventArgs> Located;

    /// <summary>
    /// 取消按钮触发前事件
    /// </summary>
    [Description("当点击取消按钮时触发\n可将 e.Applied 置为 true 屏蔽掉本组件的缺省处理过程"), Category("ToolBar")]
    public event EventHandler<BarItemClickEventArgs> Canceling;

    /// <summary>
    /// 取消按钮触发后事件
    /// </summary>
    [Description("当点击取消按钮的过程处理之后触发"), Category("ToolBar")]
    public event EventHandler<BarItemClickEventArgs> Canceled;

    /// <summary>
    /// 取消按钮触发提示被否决后事件
    /// </summary>
    [Description("当点击取消按钮提示被否决之后触发"), Category("ToolBar")]
    public event EventHandler<BarItemClickEventArgs> CancelRejected;

    /// <summary>
    /// 提交按钮触发前事件
    /// </summary>
    [Description("当点击提交按钮时触发\n可将 e.Applied 置为 true 屏蔽掉本组件的缺省处理过程"), Category("ToolBar")]
    public event EventHandler<BarItemSaveEventArgs> Saving;

    /// <summary>
    /// 提交按钮触发后事件
    /// </summary>
    [Description("当点击提交按钮的过程处理之后触发"), Category("ToolBar")]
    public event EventHandler<BarItemSaveEventArgs> Saved;

    /// <summary>
    /// 导入按钮触发前事件
    /// </summary>
    [Description("当点击导入按钮时触发\n可将 e.Applied 置为 true 屏蔽掉本组件的缺省处理过程"), Category("ToolBar")]
    public event EventHandler<BarItemImportEventArgs> Importing;

    /// <summary>
    /// 导入按钮触发后事件
    /// </summary>
    [Description("当点击导入按钮的过程处理之后触发"), Category("ToolBar")]
    public event EventHandler<BarItemImportEventArgs> Imported;

    /// <summary>
    /// 导出按钮触发前事件
    /// </summary>
    [Description("当点击导出按钮时触发\n可将 e.Applied 置为 true 屏蔽掉本组件的缺省处理过程"), Category("ToolBar")]
    public event EventHandler<BarItemClickEventArgs> Exporting;

    /// <summary>
    /// 导出按钮触发后事件
    /// </summary>
    [Description("当点击导出按钮的过程处理之后触发"), Category("ToolBar")]
    public event EventHandler<BarItemClickEventArgs> Exported;

    /// <summary>
    /// 打印按钮触发前触发
    /// </summary>
    [Description("当点击打印按钮时触发\n可将 e.Applied 置为 true 屏蔽掉本组件的缺省处理过程"), Category("ToolBar")]
    public event EventHandler<BarItemClickEventArgs> Printing;

    /// <summary>
    /// 打印按钮触发后事件
    /// </summary>
    [Description("当点击打印按钮的过程处理之后触发"), Category("ToolBar")]
    public event EventHandler<BarItemClickEventArgs> Printed;

    /// <summary>
    /// 帮助按钮触发前事件
    /// </summary>
    [Description("当点击帮助按钮时触发\n可将 e.Applied 置为 true 屏蔽掉本组件的缺省处理过程"), Category("ToolBar")]
    public event EventHandler<BarItemClickEventArgs> Helping;

    /// <summary>
    /// 帮助按钮触发后事件
    /// </summary>
    [Description("当点击帮助按钮的过程处理之后触发"), Category("ToolBar")]
    public event EventHandler<BarItemClickEventArgs> Helped;

    /// <summary>
    /// 退出按钮触发前事件
    /// </summary>
    [Description("当点击退出按钮时触发\n可将 e.Applied 置为 true 屏蔽掉本组件的缺省处理过程"), Category("ToolBar")]
    public event EventHandler<BarItemClickEventArgs> Exiting;

    /// <summary>
    /// 退出按钮触发后事件
    /// </summary>
    [Description("当点击退出按钮的过程处理之后触发"), Category("ToolBar")]
    public event EventHandler<BarItemClickEventArgs> Exited;

    #endregion

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

    private bool AllowMultistepSubmitMode(BindingSource source)
    {
      return AllowMultistepSubmit && (source != MasterBindingSource || AllowMultistepSubmitMaster);
    }

    private void FetchPromptCode(PromptCodeBindingSourceStatus status)
    {
      if (status.BindingSource == null)
        return;
      if (String.IsNullOrEmpty(status.PromptCodeName))
        return;
      SetBindingSource(status.BindingSource, new object[] { status.PromptCodeName, UserIdentity.CurrentIdentity }, status.IsAsynchronousFetch);
    }

    private void ApplyBindingSourceStatus(BindingSourceStatus status)
    {
      MasterBindingSource = status.MasterBindingSource;
      CriteriaBindingSource = status.CriteriaBindingSource;
      CriteriaCombineControl = status.CriteriaCombineControl;
      BindingSourceEditabled = status.Editabled;
      BindingSourceAllowLoopAdd = status.AllowLoopAddValue;
      HyperResetAuthorization = status.HyperResetAuthorization;
      GroupName = status.GroupName;
      if (status.Editabled && _bindingSourceGroups.ContainsKey(status.GroupName ?? String.Empty))
        OperateState = _bindingSourceGroups[status.GroupName ?? String.Empty];
      else
        OperateState = DataOperateState.Browse;
      ResetComponentAuthorizationRules();
      ResetRecordState();
    }

    private void InitializeFetchEnumBindingSources()
    {
      foreach (EnumBindingSourceStatus item in EnumBindingSources)
        if (item.BindingSource != null && item.BindingSourceItemType != null)
        {
          item.BindingSource.DataSourceChanged += new EventHandler(EnumBindingSource_DataSourceChanged);
          item.BindingSource.DataSource = EnumKeyCaptionCollection.Fetch(item.BindingSourceItemType);
        }
    }

    private void InitializeFetchPromptCodeBindingSources()
    {
      foreach (PromptCodeBindingSourceStatus item in PromptCodeBindingSources)
        if (item.BindingSource != null && !String.IsNullOrEmpty(item.PromptCodeName))
        {
          if (item.AutoFetch)
            FetchPromptCode(item);
          if (item.AutoRefresh)
            _autoRefreshPromptCodeBindingSources = true;
        }
    }

    private void InitializeHelperComponent()
    {
      foreach (FieldInfo fieldInfo in Form.GetType().GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic))
      {
        Component component = fieldInfo.GetValue(Form) as Component;
        if (component == null)
          continue;
        DXErrorProvider dxErrorProvider = component as DXErrorProvider;
        if (dxErrorProvider != null)
        {
          _dxErrorProvider = dxErrorProvider;
          continue;
        }
        ExecuteAuthorization executeAuthorization = component as ExecuteAuthorization;
        if (executeAuthorization != null)
        {
          _executeAuthorization = executeAuthorization;
          continue;
        }
        TreeListManager treeListManager = component as TreeListManager;
        if (treeListManager != null)
        {
          _treeListManager = treeListManager;
          continue;
        }
      }
      if (_dxErrorProvider == null)
        _dxErrorProvider = new DXErrorProvider();
    }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes"), System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Reliability", "CA2000:DisposeObjectsBeforeLosingScope")]
    private void InitializeBindingSources()
    {
      InFetchBindingSources = true;
      try
      {
        if (BindingSources.Count == 0)
        {
          if (BindingSource != null)
            BindingSources.Add(new BindingSourceStatus()
            {
              BindingSource = this.BindingSource,
              MasterBindingSource = this.MasterBindingSource,
              CriteriaBindingSource = this.CriteriaBindingSource,
              CriteriaCombineControl = this.CriteriaCombineControl,
              GroupName = this.GroupName
            });
        }
        else
        {
          if (BindingSource == null)
            foreach (BindingSourceStatus item in BindingSources)
              if (item.BindingSource != null)
              {
                BindingSource = item.BindingSource;
                break;
              }
        }
        List<BindingSource> editabledBindingSourceList = new List<BindingSource>();
        List<BindingSource> resetEditFriendlyBindingSourceList = new List<BindingSource>();
        foreach (BindingSourceStatus item in BindingSources)
          if (item.BindingSource != null)
          {
            if (item.Editabled)
              editabledBindingSourceList.Add(item.BindingSource);
            resetEditFriendlyBindingSourceList.Add(item.BindingSource);
            if (item.BindingSource == BindingSource)
              ApplyBindingSourceStatus(item);
            if (item.CriteriaBindingSource != null)
            {
              editabledBindingSourceList.Add(item.CriteriaBindingSource);
              resetEditFriendlyBindingSourceList.Add(item.CriteriaBindingSource);
              if (item.AutoStoreCriteria)
                Phenix.Core.Windows.ControlHelper.RestoreCriteriaBindingSourceData(Form, item.CriteriaBindingSource, item.BindingSource, true);
              else
              {
                Type type = item.CriteriaBindingSource.DataSource as Type;
                if (type != null)
                  item.CriteriaBindingSource.DataSource = Activator.CreateInstance(type, true);
              }
            }
            if (item.AutoFetchValue && item.MustFetchBy(GroupName))
              try
              {
                Fetch(item, false);
              }
              catch (Exception ex)
              {
                string hint = AppUtilities.GetErrorHint(ex, typeof(Csla.DataPortalException), typeof(Csla.Reflection.CallMethodException));
                hint = String.Format(Phenix.Windows.Properties.Resources.DataFetchAborted, hint);
                InvokeShowHint(hint);
              }
            else if (!item.BindingSourceIsDetail)
            {
              Type type = item.BindingSource.DataSource as Type;
              if (type != null)
                item.BindingSource.DataSource = Activator.CreateInstance(type, true);
            }
            if (item.AutoRefreshValue)
              _autoRefreshBindingSources = true;
            item.ResetControlAuthorizationRules(Form, EditMode || NotUndoable, true);
          }
        EditValidation.ResetEditValidationRules(Form, editabledBindingSourceList.ToArray());
        if (EditValidation.NeedResetFriendlyCaption)
        {
          EditValidation.ResetEditFriendlyCaptions(_friendlyCaptionSources, resetEditFriendlyBindingSourceList.ToArray());
          EditValidation.ResetGridFriendlyCaptions(Form);
        }
      }
      finally
      {
        InFetchBindingSources = false;
      }
    }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
    private void InitializeEvent()
    {
      //拦截按钮事件
      this.ItemClick += new ItemClickEventHandler(BarManager_ItemClick);
      //拦截控件事件
      foreach (BindingSourceStatus item in BindingSources)
        if (item.BindingSource != null && item.Editabled)
        {
          IList<Control> controls = item.GetEditControls(Form);
          if (controls != null)
            for (int i = controls.Count - 1; i >= 0; i--)
            {
              Control control = controls[i];
              if (control.Enabled && control.Visible && control.CanFocus)
              {
                control.KeyPress += new KeyPressEventHandler(EditControl_KeyPress);
                control.DoubleClick += new EventHandler(EditControl_DoubleClick);
                break;
              }
            }
        }
      foreach (Control item in Phenix.Core.Windows.ControlHelper.FindControls<Control>(Form))
      {
        item.Enter += new EventHandler(Control_Enter);
        item.Validated += new EventHandler(Control_Validated);

        //for Developer Express .NET
        TextEdit textEdit = item as TextEdit;
        if (textEdit != null)
        {
          textEdit.ParseEditValue += new DevExpress.XtraEditors.Controls.ConvertEditValueEventHandler(TextEdit_ParseEditValue);
          textEdit.FormatEditValue += new DevExpress.XtraEditors.Controls.ConvertEditValueEventHandler(TextEdit_FormatEditValue);

          LookUpEditBase lookUpEdit = item as LookUpEditBase;
          if (lookUpEdit != null)
          {
            if (lookUpEdit.Properties != null)
              lookUpEdit.Properties.ButtonClick += new DevExpress.XtraEditors.Controls.ButtonPressedEventHandler(LookUpEdit_ButtonClick);
            continue;
          }

          CheckedComboBoxEdit checkedComboBoxEdit = item as CheckedComboBoxEdit;
          if (checkedComboBoxEdit != null)
          {
            if (checkedComboBoxEdit.Properties != null)
              checkedComboBoxEdit.Properties.ButtonClick += new DevExpress.XtraEditors.Controls.ButtonPressedEventHandler(CheckedComboBoxEdit_ButtonClick);
            continue;
          }
          continue;
        }

        //for Developer Express .NET
        GridControl gridControl = item as GridControl;
        if (gridControl != null)
        {
          _gridControls.Add(gridControl);

          bool setFocusedViewChanged = false;
          foreach (BaseView baseView in gridControl.ViewCollection)
          {
            ColumnView columnView = baseView as ColumnView;
            if (columnView != null && columnView.OptionsBehavior.Editable)
            {
              columnView.CellValueChanging += new CellValueChangedEventHandler(ColumnView_CellValueChanging);
              columnView.KeyPress += new KeyPressEventHandler(ColumnView_KeyPress);
              columnView.MouseWheel += new MouseEventHandler(ColumnView_MouseWheel);
              if (columnView != gridControl.MainView)
              {
                if (!setFocusedViewChanged)
                {
                  setFocusedViewChanged = true;
                  gridControl.FocusedViewChanged += new ViewFocusEventHandler(GridControl_FocusedViewChanged);
                }
                columnView.ShownEditor += new EventHandler(ColumnView_ShownEditor);
              }
              foreach (GridColumn column in columnView.Columns)
              {
                RepositoryItemLookUpEditBase repositoryItemLookUpEdit = column.ColumnEdit as RepositoryItemLookUpEditBase;
                if (repositoryItemLookUpEdit != null)
                  repositoryItemLookUpEdit.ButtonClick += new DevExpress.XtraEditors.Controls.ButtonPressedEventHandler(RepositoryItemLookUpEdit_ButtonClick);
                RepositoryItemCheckedComboBoxEdit repositoryItemCheckedComboBoxEdit = column.ColumnEdit as RepositoryItemCheckedComboBoxEdit;
                if (repositoryItemCheckedComboBoxEdit != null)
                  repositoryItemCheckedComboBoxEdit.ButtonClick += new DevExpress.XtraEditors.Controls.ButtonPressedEventHandler(RepositoryItemCheckedComboBoxEdit_ButtonClick);
              }
            }
          }
          continue;
        }

        //for Developer Express .NET
        VGridControl vGridControl = item as VGridControl;
        if (vGridControl != null)
        {
          _vGridControls.Add(vGridControl);

          if (vGridControl.OptionsBehavior.Editable)
          {
            vGridControl.CellValueChanging += new DevExpress.XtraVerticalGrid.Events.CellValueChangedEventHandler(VGridControl_CellValueChanging);
            foreach (BaseRow row in vGridControl.Rows)
            {
              RepositoryItemLookUpEditBase repositoryItemLookUpEdit = row.Properties.RowEdit as RepositoryItemLookUpEditBase;
              if (repositoryItemLookUpEdit != null)
                repositoryItemLookUpEdit.ButtonClick += new DevExpress.XtraEditors.Controls.ButtonPressedEventHandler(RepositoryItemLookUpEdit_ButtonClick);
              RepositoryItemCheckedComboBoxEdit repositoryItemCheckedComboBoxEdit = row.Properties.RowEdit as RepositoryItemCheckedComboBoxEdit;
              if (repositoryItemCheckedComboBoxEdit != null)
                repositoryItemCheckedComboBoxEdit.ButtonClick += new DevExpress.XtraEditors.Controls.ButtonPressedEventHandler(RepositoryItemCheckedComboBoxEdit_ButtonClick);
            }
          }
          continue;
        }

        LayoutControl layoutControl = item as LayoutControl;
        if (layoutControl != null)
        {
          _layoutControls.Add(layoutControl);
          continue;
        }
      }
    }

    /// <summary>
    /// 加载后
    /// </summary>
    protected override void OnLoaded()
    {
      base.OnLoaded();

      this.Images = this.imageList;
      this.AllowQuickCustomization = false;
      this.AllowCustomization = false;

      foreach (Bar item in this.Bars)
        if (item.OptionsBar != null)
        {
          //规范Bar的Layout
          item.OptionsBar.AllowQuickCustomization = false;
          item.OptionsBar.DisableCustomization = true;
          item.OptionsBar.DisableClose = true;
          item.OptionsBar.DrawDragBorder = false;
          item.OptionsBar.UseWholeRow = true;
        }

      foreach (BarItem item in this.Items)
        switch ((BarItemId)item.Id)
        {
          case BarItemId.Fetch:
            item.Caption = Phenix.Windows.Properties.Resources.ToolBarFetchHeaderTip;
            item.SuperTip = CreateToolTip(
              Phenix.Windows.Properties.Resources.ToolBarFetchHeaderTip,
              Phenix.Windows.Properties.Resources.ToolBarFetchBodyTip, item.ItemShortcut.ToString());
            break;
          case BarItemId.Reset:
            item.Caption = Phenix.Windows.Properties.Resources.ToolBarResetHeaderTip;
            item.SuperTip = CreateToolTip(
              Phenix.Windows.Properties.Resources.ToolBarResetHeaderTip,
              Phenix.Windows.Properties.Resources.ToolBarResetBodyTip, item.ItemShortcut.ToString());
            break;
          case BarItemId.Restore:
            item.Caption = Phenix.Windows.Properties.Resources.ToolBarRestoreHeaderTip;
            item.SuperTip = CreateToolTip(
              Phenix.Windows.Properties.Resources.ToolBarRestoreHeaderTip,
              Phenix.Windows.Properties.Resources.ToolBarRestoreBodyTip, item.ItemShortcut.ToString());
            break;
          case BarItemId.Add:
            item.Caption = Phenix.Windows.Properties.Resources.ToolBarAddHeaderTip;
            item.SuperTip = CreateToolTip(
              Phenix.Windows.Properties.Resources.ToolBarAddHeaderTip,
              Phenix.Windows.Properties.Resources.ToolBarAddBodyTip, item.ItemShortcut.ToString());
            break;
          case BarItemId.AddClone:
            item.Caption = Phenix.Windows.Properties.Resources.ToolBarAddCloneHeaderTip;
            item.SuperTip = CreateToolTip(
              Phenix.Windows.Properties.Resources.ToolBarAddCloneHeaderTip,
              Phenix.Windows.Properties.Resources.ToolBarAddCloneBodyTip, item.ItemShortcut.ToString());
            break;
          case BarItemId.Modify:
            item.Caption = Phenix.Windows.Properties.Resources.ToolBarModifyHeaderTip;
            item.SuperTip = CreateToolTip(
              Phenix.Windows.Properties.Resources.ToolBarModifyHeaderTip,
              Phenix.Windows.Properties.Resources.ToolBarModifyBodyTip, item.ItemShortcut.ToString());
            break;
          case BarItemId.Delete:
            item.Caption = Phenix.Windows.Properties.Resources.ToolBarDeleteHeaderTip;
            item.SuperTip = CreateToolTip(
              Phenix.Windows.Properties.Resources.ToolBarDeleteHeaderTip,
              Phenix.Windows.Properties.Resources.ToolBarDeleteBodyTip, item.ItemShortcut.ToString());
            break;
          case BarItemId.Locate:
            item.Caption = Phenix.Windows.Properties.Resources.ToolBarLocateHeaderTip;
            item.SuperTip = CreateToolTip(
              Phenix.Windows.Properties.Resources.ToolBarLocateHeaderTip,
              Phenix.Windows.Properties.Resources.ToolBarLocateBodyTip, item.ItemShortcut.ToString());
            break;
          case BarItemId.Cancel:
            item.Caption = Phenix.Windows.Properties.Resources.ToolBarCancelHeaderTip;
            item.SuperTip = CreateToolTip(
              Phenix.Windows.Properties.Resources.ToolBarCancelHeaderTip,
              Phenix.Windows.Properties.Resources.ToolBarCancelBodyTip, item.ItemShortcut.ToString());
            break;
          case BarItemId.Save:
            item.Caption = Phenix.Windows.Properties.Resources.ToolBarSaveHeaderTip;
            item.SuperTip = CreateToolTip(
              Phenix.Windows.Properties.Resources.ToolBarSaveHeaderTip,
              Phenix.Windows.Properties.Resources.ToolBarSaveBodyTip, item.ItemShortcut.ToString());
            break;
          case BarItemId.Import:
            item.Caption = Phenix.Windows.Properties.Resources.ToolBarImportHeaderTip;
            item.SuperTip = CreateToolTip(
              Phenix.Windows.Properties.Resources.ToolBarImportHeaderTip,
              Phenix.Windows.Properties.Resources.ToolBarImportBodyTip, item.ItemShortcut.ToString());
            break;
          case BarItemId.Export:
            item.Caption = Phenix.Windows.Properties.Resources.ToolBarExportHeaderTip;
            item.SuperTip = CreateToolTip(
              Phenix.Windows.Properties.Resources.ToolBarExportHeaderTip,
              Phenix.Windows.Properties.Resources.ToolBarExportBodyTip, item.ItemShortcut.ToString());
            break;
          case BarItemId.Print:
            item.Caption = Phenix.Windows.Properties.Resources.ToolBarPrintHeaderTip;
            item.SuperTip = CreateToolTip(
              Phenix.Windows.Properties.Resources.ToolBarPrintHeaderTip,
              Phenix.Windows.Properties.Resources.ToolBarPrintBodyTip, item.ItemShortcut.ToString());
            break;
          case BarItemId.Help:
            item.Caption = Phenix.Windows.Properties.Resources.ToolBarHelpHeaderTip;
            item.SuperTip = CreateToolTip(
              Phenix.Windows.Properties.Resources.ToolBarHelpHeaderTip,
              Phenix.Windows.Properties.Resources.ToolBarHelpBodyTip, item.ItemShortcut.ToString());
            break;
          case BarItemId.Exit:
            item.Caption = Phenix.Windows.Properties.Resources.ToolBarExitHeaderTip;
            item.SuperTip = CreateToolTip(
              Phenix.Windows.Properties.Resources.ToolBarExitHeaderTip,
              Phenix.Windows.Properties.Resources.ToolBarExitBodyTip, item.ItemShortcut.ToString());
            break;
          case BarItemId.Hint:
            item.ItemClick += (sender, args) => { ShowExecuteActionForm.Execute(CurrentBusiness); };
            break;
        }
    }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Reliability", "CA2000:DisposeObjectsBeforeLosingScope")]
    internal static SuperToolTip CreateToolTip(string title, string contents, string footer)
    {
      SuperToolTip tip = new SuperToolTip();
      ToolTipTitleItem titleItem = new ToolTipTitleItem();
      ToolTipItem contentsItem = new ToolTipItem();
      ToolTipTitleItem footerItem = new ToolTipTitleItem();
      titleItem.Text = title;
      contentsItem.Text = contents;
      footerItem.Text = footer;
      tip.Items.Add(titleItem);
      tip.Items.Add(contentsItem);
      tip.Items.Add(footerItem);
      return tip;
    }

    /// <summary>
    /// 分析Form操作数据源
    /// </summary>
    public void AnalyseFormWorkSource()
    {
      IForm form = Form as IForm;
      if (form == null || form.WorkSource == null)
        return;
      List<ICriteria> criterias;
      if (form.WorkSource is ICriteria)
        criterias = new List<ICriteria> { (ICriteria)form.WorkSource };
      else if (form.WorkSource is IEnumerable<ICriteria>)
        criterias = new List<ICriteria>((IEnumerable<ICriteria>)form.WorkSource);
      else
        return;
      foreach (ICriteria criteria in criterias)
      {
        Type criterionsType = criteria.GetType();
        foreach (BindingSourceStatus bindingSourceStatus in BindingSources)
          if (BindingSourceHelper.GetDataSourceType(bindingSourceStatus.CriteriaBindingSource) == criterionsType)
          {
            bindingSourceStatus.CriteriaBindingSource.DataSource = criteria;
            bindingSourceStatus.DisposableCriteria = true;
            break;
          }
      }
      ClickFetchButton();
    }

    #region FindBindingSource

    /// <summary>
    /// 检索匹配的BindingSource
    /// </summary>
    /// <param name="business">业务对象</param>
    public BindingSource FindBindingSource(IBusinessObject business)
    {
      if (business != null)
        foreach (BindingSourceStatus item in BindingSources)
          if (item.CurrentBusiness == business || item.CurrentBusinessList == business.Owner)
            return item.BindingSource;
      return null;
    }

    /// <summary>
    /// 检索匹配的BindingSource
    /// </summary>
    /// <param name="businesses">业务集合对象</param>
    public BindingSource FindBindingSource(IBusinessCollection businesses)
    {
      if (businesses != null)
        foreach (BindingSourceStatus item in BindingSources)
          if (item.CurrentBusinessList == businesses)
            return item.BindingSource;
      return null;
    }

    /// <summary>
    /// 检索匹配的BindingSource
    /// </summary>
    /// <param name="view">BaseView</param>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters")]
    public BindingSource FindBindingSource(BaseView view)
    {
      if (view != null && view.DataSource != null)
        foreach (BindingSourceStatus item in BindingSources)
          if (item.BindingSource == view.DataSource || item.CurrentBusinessList == view.DataSource)
            return item.BindingSource;
      return null;
    }

    #endregion

    /// <summary>
    /// 搜索第一个匹配的控件，并定位焦点
    /// </summary>
    /// <param name="source">数据源</param>
    public Control FocusControl(BindingSource source)
    {
      if (source == null)
        return null;

      Control result = null;
      foreach (BindingSourceStatus item in BindingSources)
        if (item.BindingSource == source)
        {
          result = EditControlHelper.FocusControl(item.GetEditControls(Form));
          break;
        }
        else if (item.CriteriaBindingSource == source)
        {
          result = EditControlHelper.FocusControl(item.GetCriteriaControls(Form));
          break;
        }
      if (result != null)
        return result;

      //for Developer Express .NET
      result = GridControlHelper.FocusColumn(_gridControls, source);
      if (result != null)
        return result;
      result = VGridControlHelper.FocusEditableGridControl(_vGridControls, source);
      if (result != null)
        return result;

      return EditControlHelper.FocusControl(Form, source);
    }

    /// <summary>
    /// 搜索第一个匹配的可编辑控件，并定位焦点
    /// </summary>
    /// <param name="source">数据源</param>
    public Control FocusEditableControl(BindingSource source)
    {
      if (source == null)
        return null;

      Control result = null;
      foreach (BindingSourceStatus item in BindingSources)
        if (item.BindingSource == source)
        {
          result = EditControlHelper.FocusEditableControl(item.GetEditControls(Form));
          break;
        }
        else if (item.CriteriaBindingSource == source)
        {
          result = EditControlHelper.FocusEditableControl(item.GetCriteriaControls(Form));
          break;
        }
      if (result != null)
        return result;

      //for Developer Express .NET
      result = GridControlHelper.FocusEditableColumn(_gridControls, source);
      if (result != null)
        return result;
      result = VGridControlHelper.FocusEditableGridControl(_vGridControls, source);
      if (result != null)
        return result;

      return EditControlHelper.FocusEditableControl(Form, source);
    }

    /// <summary>
    /// 检索指定的菜单项
    /// </summary>
    /// <param name="barItemId">菜单项ID</param>
    public BarItem FindBarItem(BarItemId barItemId)
    {
      int id = Convert.ToInt32(barItemId);
      foreach (BarItem item in this.Items)
        if (item.Id == id)
          return item;
      return null;
    }

    /// <summary>
    /// 点击检索按钮（线程安全）
    /// e = null
    /// needPrompt = PromptInFetching
    /// reset = true
    /// </summary>
    public void InvokeClickFetchButton()
    {
      if (this.Form.InvokeRequired)
        this.Form.BeginInvoke(new Function<BarItemClickEventArgs>(ClickFetchButton));
      else
        ClickFetchButton();
    }

    /// <summary>
    /// 点击检索按钮
    /// e = null
    /// needPrompt = PromptInFetching
    /// reset = true
    /// </summary>
    public BarItemClickEventArgs ClickFetchButton()
    {
      return ClickFetchButton(null, PromptInFetching, true);
    }

    /// <summary>
    /// 点击检索按钮
    /// e = null
    /// reset = true
    /// </summary>
    /// <param name="needPrompt">是否需要提示</param>
    public BarItemClickEventArgs ClickFetchButton(bool needPrompt)
    {
      return ClickFetchButton(null, needPrompt, true);
    }

    /// <summary>
    /// 点击检索按钮
    /// needPrompt = PromptInFetching
    /// reset = true
    /// </summary>
    /// <param name="e">点击按钮事件数据</param>
    public BarItemClickEventArgs ClickFetchButton(ItemClickEventArgs e)
    {
      return ClickFetchButton(e, PromptInFetching, true);
    }

    /// <summary>
    /// 点击检索按钮
    /// reset = true
    /// </summary>
    /// <param name="e">点击按钮事件数据</param>
    /// <param name="needPrompt">是否需要提示</param>
    public BarItemClickEventArgs ClickFetchButton(ItemClickEventArgs e, bool needPrompt)
    {
      return ClickFetchButton(e, needPrompt, true);
    }

    /// <summary>
    /// 点击检索按钮
    /// </summary>
    /// <param name="e">点击按钮事件数据</param>
    /// <param name="needPrompt">是否需要提示</param>
    /// <param name="reset">是否强制覆盖, true表示虽然数据源已有数据也将被覆盖</param>
    public BarItemClickEventArgs ClickFetchButton(ItemClickEventArgs e, bool needPrompt, bool reset)
    {
      if (e != null)
        lock (_barItemSwitchs)
        {
          if (_barItemSwitchs.Contains(e.Item))
            return null;
          _barItemSwitchs.Add(e.Item);
          e.Item.Enabled = false;
        }
      try
      {
        Phenix.Core.Windows.ControlHelper.ResetFocus(Form);
        BarItemClickEventArgs result = new BarItemClickEventArgs(e != null ? e.Item : null, BindingSource);
        if (needPrompt && EditDirty)
        {
          if (MessageBox.Show(Phenix.Windows.Properties.Resources.ConfirmFetch, Phenix.Windows.Properties.Resources.DataFetch,
            MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.No)
          {
            result.Stop = true;
            result.Succeed = false;
            return result;
          }
          DoCancel(false);
        }
        OperateState = DataOperateState.Fetching;
        if (Fetching != null)
          Fetching(this, result);
        if (result.Stop)
        {
          OperateState = DataOperateState.FetchSuspend;
          return result;
        }
        if (!result.Applied)
          result.Succeed = DoFetch(reset);
        if (result.Succeed)
        {
          DoFetched();
          if (Fetched != null)
            Fetched(this, result);
        }
        else
          OperateState = DataOperateState.FetchAborted;
        return result;
      }
      finally
      {
        ResetComponentAuthorizationRules();
        ResetRecordState();
        if (e != null)
          lock (_barItemSwitchs)
          {
            _barItemSwitchs.Remove(e.Item);
          }
      }
    }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
    private bool DoFetch(bool reset)
    {
      try
      {
        FetchGroup(GroupName, reset);
        return true;
      }
      catch (AuthenticationException ex)
      {
        string hint = AppUtilities.GetErrorHint(ex);
        ShowHint(hint);
        MessageBox.Show(hint, Phenix.Windows.Properties.Resources.DataFetch, MessageBoxButtons.OK, MessageBoxIcon.Error);
        Application.Exit();
        return false;
      }
      catch (Exception ex)
      {
        string hint = AppUtilities.GetErrorHint(ex, typeof(Csla.DataPortalException), typeof(Csla.Reflection.CallMethodException));
        hint = String.Format(Phenix.Windows.Properties.Resources.DataFetchAborted, hint);
        ShowHint(hint);
        MessageBox.Show(hint, Phenix.Windows.Properties.Resources.DataFetch, MessageBoxButtons.OK, MessageBoxIcon.Error);
        return false;
      }
    }

    private void DoFetched()
    {
      if (NotUndoable)
        OperateState = DataOperateState.Modify;
      else if (AutoEditOnFetched)
        ClickModifyButton();
      else
      {
        ClearEditObject();
        OperateState = DataOperateState.Browse;
        FocusControl(BindingSource);
      }
    }

    /// <summary>
    /// 检索
    /// </summary>
    /// <param name="groupName">分组名</param>
    /// <param name="reset">是否强制覆盖, true表示虽然数据源已有数据也将被覆盖</param>
    public void FetchGroup(string groupName, bool reset)
    {
      if (String.IsNullOrEmpty(groupName))
        groupName = null;
      InFetchBindingSources = true;
      try
      {
        foreach (BindingSourceStatus item in BindingSources)
          if (item.BindingSource != null && item.InFetchValue && item.MustFetchBy(groupName))
          {
            Fetch(item, reset);
            if (item.CriteriaBindingSource != null)
              if (item.DisposableCriteria)
              {
                item.DisposableCriteria = false;
                item.CriteriaBindingSource.DataSource = Activator.CreateInstance(BindingSourceHelper.GetDataSourceType(item.CriteriaBindingSource), true);
              }
              else if (item.AutoStoreCriteria)
                Phenix.Core.Windows.ControlHelper.SaveCriteriaBindingSourceData(Form, item.CriteriaBindingSource, item.BindingSource);
          }
        foreach (PromptCodeBindingSourceStatus item in PromptCodeBindingSources)
          if (item.InFetch && item.MustFetchBy(groupName))
            FetchPromptCode(item);
      }
      finally
      {
        InFetchBindingSources = false;
      }
    }

    /// <summary>
    /// 检索
    /// source = status.BindingSource
    /// criteriaSource = status.CriteriaBindingSource
    /// criteriaCombineControl =  status.CriteriaCombineControl
    /// cacheEnabled = status.CacheEnabledValue
    /// includeDisabled = status.IncludeDisabledValue
    /// isAsynchronous = status.IsAsynchronousFetchValue
    /// </summary>
    /// <param name="status">数据源状况</param>
    /// <param name="reset">是否强制覆盖, true表示虽然数据源已有数据也将被覆盖</param>
    public void Fetch(BindingSourceStatus status, bool reset)
    {
      if (status == null)
        throw new ArgumentNullException("status");
      Fetch(status.BindingSource, status.CriteriaBindingSource, status.CriteriaCombineControl, status.CacheEnabledValue, status.IncludeDisabledValue, reset, status.IsAsynchronousFetchValue);
    }

    /// <summary>
    /// 检索
    /// source = status.BindingSource
    /// criteriaSource = status.CriteriaBindingSource
    /// criteriaCombineControl =  status.CriteriaCombineControl
    /// cacheEnabled = status.CacheEnabledValue
    /// includeDisabled = status.IncludeDisabledValue
    /// </summary>
    /// <param name="status">数据源状况</param>
    /// <param name="reset">是否强制覆盖, true表示虽然数据源已有数据也将被覆盖</param>
    /// <param name="isAsynchronous">是否异步Fetch?</param>
    public void Fetch(BindingSourceStatus status, bool reset, bool isAsynchronous)
    {
      if (status == null)
        throw new ArgumentNullException("status");
      Fetch(status.BindingSource, status.CriteriaBindingSource, status.CriteriaCombineControl, status.CacheEnabledValue, status.IncludeDisabledValue, reset, isAsynchronous);
    }

    /// <summary>
    /// 检索
    /// criteriaCombineControl = null
    /// cacheEnabled = true
    /// includeDisabled = false
    /// </summary>
    /// <param name="source">数据源</param>
    /// <param name="criteriaSource">条件数据源</param>
    /// <param name="reset">是否强制覆盖, true表示虽然数据源已有数据也将被覆盖</param>
    /// <param name="isAsynchronous">是否异步Fetch?</param>
    public void Fetch(BindingSource source, BindingSource criteriaSource, bool reset, bool isAsynchronous)
    {
      Fetch(source, criteriaSource, null, true, false, reset, isAsynchronous);
    }

    /// <summary>
    /// 检索
    /// </summary>
    /// <param name="source">数据源</param>
    /// <param name="criteriaSource">条件数据源</param>
    /// <param name="criteriaCombineControl">查询组合框控件</param>
    /// <param name="cacheEnabled">是否需要缓存对象?</param>
    /// <param name="includeDisabled">是否包含禁用记录?</param>
    /// <param name="reset">是否强制覆盖, true表示虽然数据源已有数据也将被覆盖</param>
    /// <param name="isAsynchronous">是否异步Fetch?</param>
    public void Fetch(BindingSource source, BindingSource criteriaSource, CriteriaCombineControl criteriaCombineControl, 
      bool cacheEnabled, bool includeDisabled, bool reset, bool isAsynchronous)
    {
      if (criteriaSource == null || criteriaSource.DataSource == null)
        Fetch(source, criteriaCombineControl != null ? criteriaCombineControl.WorkingCriteriaExpression : null, cacheEnabled, includeDisabled, reset, isAsynchronous);
      else
      {
        if (criteriaSource.DataSource is Type)
          Phenix.Core.Windows.ControlHelper.RestoreCriteriaBindingSourceData(Form, criteriaSource, source, false);
        Fetch(source, criteriaSource.DataSource, cacheEnabled, includeDisabled, reset, isAsynchronous);
      }
    }

    /// <summary>
    /// 检索
    /// </summary>
    /// <param name="source">数据源</param>
    /// <param name="criteria">条件对象</param>
    /// <param name="cacheEnabled">是否需要缓存对象?</param>
    /// <param name="includeDisabled">是否包含禁用记录?</param>
    /// <param name="reset">是否强制覆盖, true表示虽然数据源已有数据也将被覆盖</param>
    /// <param name="isAsynchronous">是否异步Fetch?</param>
    public void Fetch(BindingSource source, object criteria, bool cacheEnabled, bool includeDisabled, bool reset, bool isAsynchronous)
    {
      if (source == null)
        throw new ArgumentNullException("source");
      if (!(source.DataSource is Type) && !reset)
        return;
      Type dataSourceType = BindingSourceHelper.GetDataSourceType(source);
      if (dataSourceType == null)
        return;
      if (!typeof(IBusinessCollection).IsAssignableFrom(dataSourceType))
        return;
      List<object> args = criteria != null
        ? criteria is CriteriaExpression
            ? new List<object>
                {
                  new Phenix.Business.Criterions(BindingSourceHelper.GetDataSourceType(source),
                    (CriteriaExpression)criteria, cacheEnabled,
                    (IBusinessObject)(!AllowMultistepSubmit && OperateState == DataOperateState.Add ? EditObject : null))
                }
            : new List<object>
                {
                  new Phenix.Business.Criterions(BindingSourceHelper.GetDataSourceType(source),
                    (ICriteria)criteria, cacheEnabled,
                    (IBusinessObject)(!AllowMultistepSubmit && OperateState == DataOperateState.Add ? EditObject : null))
                }
        : new List<object>
            {
              new Phenix.Business.Criterions(BindingSourceHelper.GetDataSourceType(source),
                cacheEnabled, includeDisabled,
                (IBusinessObject)(!AllowMultistepSubmit && OperateState == DataOperateState.Add ? EditObject : null))
            };
      SetBindingSource(source, args.ToArray(), isAsynchronous);
    }

    private bool BeginEdit()
    {
      IBusiness currentBusinessRoot = CurrentBusinessRoot;
      if (currentBusinessRoot == null)
        return false;
      if (!currentBusinessRoot.EditMode)
        currentBusinessRoot.BeginEdit();
      if (!String.IsNullOrEmpty(GroupName))
      {
        List<IBusiness> ignores = new List<IBusiness>();
        ignores.Add(currentBusinessRoot);
        foreach (BindingSourceStatus item in BindingSources)
          if (item.BindingSource != null && String.CompareOrdinal(item.GroupName, GroupName) == 0)
          {
            IBusiness business = item.FindBusinessRoot();
            if (business != null && !ignores.Contains(business))
            {
              if (!business.EditMode)
                business.BeginEdit();
              ignores.Add(business);
            }
            item.ResetControlAuthorizationRules(Form, EditMode || NotUndoable, false);
          }
      }
      return true;
    }

    private void CancelEdit()
    {
      IBusiness currentBusinessRoot = CurrentBusinessRoot;
      if (currentBusinessRoot == null)
        return;
      currentBusinessRoot.CancelEdit();
      if (!String.IsNullOrEmpty(GroupName))
      {
        List<IBusiness> ignores = new List<IBusiness>();
        ignores.Add(currentBusinessRoot);
        foreach (BindingSourceStatus item in BindingSources)
          if (item.BindingSource != null && String.CompareOrdinal(item.GroupName, GroupName) == 0)
          {
            IBusiness business = item.FindBusinessRoot();
            if (business != null && !ignores.Contains(business))
            {
              business.CancelEdit();
              ignores.Add(business);
            }
            item.ResetControlAuthorizationRules(Form, EditMode || NotUndoable, true);
          }
      }
    }

    /// <summary>
    /// 点击复位按钮（线程安全）
    /// e = null
    /// </summary>
    public void InvokeClickResetButton()
    {
      if (this.Form.InvokeRequired)
        this.Form.BeginInvoke(new Function<BarItemClickEventArgs>(ClickResetButton));
      else
        ClickResetButton();
    }

    /// <summary>
    /// 点击复位按钮
    /// e = null
    /// </summary>
    public BarItemClickEventArgs ClickResetButton()
    {
      return ClickResetButton(null);
    }

    /// <summary>
    /// 点击复位按钮
    /// </summary>
    /// <param name="e">点击按钮事件数据</param>
    public BarItemClickEventArgs ClickResetButton(ItemClickEventArgs e)
    {
      if (e != null)
        lock (_barItemSwitchs)
        {
          if (_barItemSwitchs.Contains(e.Item))
            return null;
          _barItemSwitchs.Add(e.Item);
          e.Item.Enabled = false;
        }
      try
      {
        Phenix.Core.Windows.ControlHelper.ResetFocus(Form);
        BarItemClickEventArgs result = new BarItemClickEventArgs(e != null ? e.Item : null, BindingSource);
        if (Resetting != null)
          Resetting(this, result);
        if (result.Stop)
          return result;
        if (!result.Applied)
          result.Succeed = DoReset();
        if (result.Succeed)
        {
          if (Reset != null)
            Reset(this, result);
        }
        return result;
      }
      finally
      {
        ResetComponentAuthorizationRules();
        if (e != null)
          lock (_barItemSwitchs)
          {
            _barItemSwitchs.Remove(e.Item);
          }
      }
    }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
    private bool DoReset()
    {
      try
      {
        using (new DevExpress.Utils.WaitDialogForm(Phenix.Windows.Properties.Resources.DataReseting, Phenix.Core.Properties.Resources.PleaseWait))
        {
          ShowHint(Phenix.Windows.Properties.Resources.DataReseting);
          IBusinessObject currentBusiness = CurrentBusiness;
          if (currentBusiness != null && (currentBusiness.EditMode || currentBusiness.NotUndoable))
          {
            if (currentBusiness.IsNew)
              if (currentBusiness.FillFieldValuesToDefault())
                FocusEditableControl(BindingSource);
          }
          else if (CriteriaBindingSource != null && BindingSource != null)
          {
            bool succeed = false;
            foreach (BindingSourceStatus item in BindingSources)
              if (item.AutoStoreCriteria && item.CriteriaBindingSource != CriteriaBindingSource && item.BindingSource == BindingSource)
              {
                Phenix.Core.Windows.ControlHelper.RestoreCriteriaBindingSourceData(Form, item.CriteriaBindingSource, item.BindingSource, true);
                succeed = true;
                break;
              }
            if (succeed)
              FocusControl(CriteriaBindingSource);
          }
        }
        ShowHint(Phenix.Windows.Properties.Resources.DataResetSucceed);
        return true;
      }
      catch (Exception ex)
      {
        string hint = String.Format(Phenix.Windows.Properties.Resources.DataResetAborted, AppUtilities.GetErrorHint(ex));
        ShowHint(hint);
        MessageBox.Show(hint, Phenix.Windows.Properties.Resources.DataReset, MessageBoxButtons.OK, MessageBoxIcon.Error);
        return false;
      }
    }

    /// <summary>
    /// 点击恢复按钮（线程安全）
    /// e = null
    /// </summary>
    public void InvokeClickRestoreButton()
    {
      if (this.Form.InvokeRequired)
        this.Form.BeginInvoke(new Function<BarItemClickEventArgs>(ClickRestoreButton));
      else
        ClickRestoreButton();
    }

    /// <summary>
    /// 点击恢复按钮
    /// e = null
    /// source = BindingSource
    /// needPrompt = PromptInSaved
    /// </summary>
    public BarItemClickEventArgs ClickRestoreButton()
    {
      return ClickRestoreButton(null, BindingSource, PromptInSaved);
    }

    /// <summary>
    /// 点击恢复按钮
    /// source = BindingSource
    /// needPrompt = PromptInSaved
    /// </summary>
    /// <param name="e">点击按钮事件数据</param>
    public BarItemClickEventArgs ClickRestoreButton(ItemClickEventArgs e)
    {
      return ClickRestoreButton(e, BindingSource, PromptInSaved);
    }

    /// <summary>
    /// 点击恢复按钮
    /// source = BindingSource
    /// </summary>
    /// <param name="e">点击按钮事件数据</param>
    /// <param name="needPrompt">是否需要提示</param>
    public BarItemClickEventArgs ClickRestoreButton(ItemClickEventArgs e, bool needPrompt)
    {
      return ClickRestoreButton(e, BindingSource, needPrompt);
    }

    /// <summary>
    /// 点击恢复按钮
    /// e = null
    /// source = BindingSource
    /// </summary>
    /// <param name="needPrompt">是否需要提示</param>
    public BarItemClickEventArgs ClickRestoreButton(bool needPrompt)
    {
      return ClickRestoreButton(null, BindingSource, needPrompt);
    }

    /// <summary>
    /// 点击恢复按钮
    /// e = null
    /// needPrompt = PromptInSaved
    /// </summary>
    /// <param name="source">数据源</param>
    public BarItemClickEventArgs ClickRestoreButton(BindingSource source)
    {
      return ClickRestoreButton(null, source, PromptInSaved);
    }

    /// <summary>
    /// 点击恢复按钮
    /// e = null
    /// </summary>
    /// <param name="source">数据源</param>
    /// <param name="needPrompt">是否需要提示</param>
    public BarItemClickEventArgs ClickRestoreButton(BindingSource source, bool needPrompt)
    {
      return ClickRestoreButton(null, source, needPrompt);
    }

    /// <summary>
    /// 点击恢复按钮
    /// </summary>
    /// <param name="e">点击按钮事件数据</param>
    /// <param name="source">数据源</param>
    /// <param name="needPrompt">是否需要提示</param>
    public BarItemClickEventArgs ClickRestoreButton(ItemClickEventArgs e, BindingSource source, bool needPrompt)
    {
      if (e != null)
        lock (_barItemSwitchs)
        {
          if (_barItemSwitchs.Contains(e.Item))
            return null;
          _barItemSwitchs.Add(e.Item);
          e.Item.Enabled = false;
        }
      try
      {
        Phenix.Core.Windows.ControlHelper.ResetFocus(Form);
        BarItemClickEventArgs result = new BarItemClickEventArgs(e != null ? e.Item : null, source);
        if (Restoring != null)
          Restoring(this, result);
        if (result.Stop)
          return result;
        if (!result.Applied)
          result.Succeed = DoRestore(source, needPrompt);
        if (result.Succeed)
        {
          DoRestored(source);
          if (Restored != null)
            Restored(this, result);
        }
        return result;
      }
      finally
      {
        ResetComponentAuthorizationRules();
        ResetRecordState();
        if (e != null)
          lock (_barItemSwitchs)
          {
            _barItemSwitchs.Remove(e.Item);
          }
      }
    }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
    private bool DoRestore(BindingSource source, bool needPrompt)
    {
      if (source == null || source.Count == 0)
        return false;
      IBusinessCollection businessList = BindingSourceHelper.GetDataSourceList(source) as IBusinessCollection;
      if (businessList == null)
        return false;
      IBusinessObject currentBusiness = BindingSourceHelper.GetDataSourceCurrent(source) as IBusinessObject;
      if (currentBusiness == null)
        return false;
      IBusiness currentBusinessRoot = CurrentBusinessRoot;
      if (currentBusinessRoot == null)
        return false;

      bool immediateSave = !currentBusinessRoot.EditMode || currentBusinessRoot.NotUndoable;
      _bypassInvalidChecks = true;
      try
      {
        try
        {
          if (immediateSave)
            BeginEdit();
          IList<object> multiSelected = GridControlHelper.FindMultiSelected(_gridControls, source);
          if (multiSelected != null && multiSelected.Count > 0)
            foreach (IBusinessObject item in multiSelected)
              item.IsDisabled = false;
          else
            currentBusiness.IsDisabled = false;
          if (immediateSave)
            if (!DoSave(needPrompt, false, false, null, null))
              goto Label;
        }
        catch (Exception ex)
        {
          string hint = String.Format(Phenix.Windows.Properties.Resources.DataRestoreAborted, businessList.Caption, currentBusiness.Caption, AppUtilities.GetErrorHint(ex));
          ShowHint(hint);
          MessageBox.Show(hint, Phenix.Windows.Properties.Resources.DataRestore, MessageBoxButtons.OK, MessageBoxIcon.Error);
          goto Label;
        }
        return true;
      }
      finally
      {
        _bypassInvalidChecks = false;
      }

      Label:
      if (immediateSave)
        CancelEdit();
      return false;
    }

    private void DoRestored(BindingSource source)
    {
      BindingSource = source;
      if (EditMode)
      {
        SetEditObject(source);
        OperateState = DataOperateState.Modify;
      }
      else
      {
        ClearEditObject();
        OperateState = DataOperateState.Browse;
      }
    }

    /// <summary>
    /// 点击增加按钮（线程安全）
    /// e = null
    /// source = BindingSource
    /// </summary>
    public void InvokeClickAddButton()
    {
      if (this.Form.InvokeRequired)
        this.Form.BeginInvoke(new Function<BarItemClickEventArgs>(ClickAddButton));
      else
        ClickAddButton();
    }

    /// <summary>
    /// 点击增加按钮
    /// e = null
    /// source = BindingSource
    /// </summary>
    public BarItemClickEventArgs ClickAddButton()
    {
      return ClickAddButton(null, BindingSource);
    }

    /// <summary>
    /// 点击增加按钮
    /// source = BindingSource
    /// </summary>
    /// <param name="e">点击按钮事件数据</param>
    public BarItemClickEventArgs ClickAddButton(ItemClickEventArgs e)
    {
      return ClickAddButton(e, BindingSource);
    }

    /// <summary>
    /// 点击增加按钮
    /// e = null 
    /// </summary>
    /// <param name="source">数据源</param>
    public BarItemClickEventArgs ClickAddButton(BindingSource source)
    {
      return ClickAddButton(null, source);
    }

    /// <summary>
    /// 点击增加按钮
    /// </summary>
    /// <param name="e">点击按钮事件数据</param>
    /// <param name="source">数据源</param>
    public BarItemClickEventArgs ClickAddButton(ItemClickEventArgs e, BindingSource source)
    {
      if (e != null)
        lock (_barItemSwitchs)
        {
          if (_barItemSwitchs.Contains(e.Item))
            return null;
          _barItemSwitchs.Add(e.Item);
          e.Item.Enabled = false;
        }
      try
      {
        Phenix.Core.Windows.ControlHelper.ResetFocus(Form);
        BarItemClickEventArgs result = new BarItemClickEventArgs(e != null ? e.Item : null, source);
        if (CheckValid(source, false))
        {
          result.Succeed = false;
          return result;
        }
        if (Adding != null)
          Adding(this, result);
        if (result.Stop)
          return result;
        if (!result.Applied)
        {
          if (source == null)
          {
            result.Succeed = false;
            MessageBox.Show(Phenix.Windows.Properties.Resources.BindingSourcePropertyIsNull, Phenix.Windows.Properties.Resources.DataAdding, MessageBoxButtons.OK, MessageBoxIcon.Error);
          }
          else
          {
            BindingSource = source;
            result.Succeed = DoAdd(source, false);
          }
        }
        if (result.Succeed)
        {
          DoAdded(result.Source);
          if (Added != null)
            Added(this, result);
        }
        return result;
      }
      finally
      {
        ResetComponentAuthorizationRules();
        if (e != null)
          lock (_barItemSwitchs)
          {
            _barItemSwitchs.Remove(e.Item);
          }
      }
    }

    /// <summary>
    /// 点击克隆按钮（线程安全）
    /// e = null
    /// source = BindingSource
    /// </summary>
    public void InvokeClickAddCloneButton()
    {
      if (this.Form.InvokeRequired)
        this.Form.BeginInvoke(new Function<BarItemClickEventArgs>(ClickAddCloneButton));
      else
        ClickAddCloneButton();
    }

    /// <summary>
    /// 点击克隆按钮
    /// e = null
    /// source = BindingSource
    /// </summary>
    public BarItemClickEventArgs ClickAddCloneButton()
    {
      return ClickAddCloneButton(null, BindingSource);
    }

    /// <summary>
    /// 点击克隆按钮
    /// source = BindingSource
    /// </summary>
    /// <param name="e">点击按钮事件数据</param>
    public BarItemClickEventArgs ClickAddCloneButton(ItemClickEventArgs e)
    {
      return ClickAddCloneButton(e, BindingSource);
    }

    /// <summary>
    /// 点击克隆按钮
    /// e = null
    /// </summary>
    /// <param name="source">数据源</param>
    public BarItemClickEventArgs ClickAddCloneButton(BindingSource source)
    {
      return ClickAddCloneButton(null, source);
    }

    /// <summary>
    /// 点击克隆按钮
    /// </summary>
    /// <param name="e">点击按钮事件数据</param>
    /// <param name="source">数据源</param>
    public BarItemClickEventArgs ClickAddCloneButton(ItemClickEventArgs e, BindingSource source)
    {
      if (e != null)
        lock (_barItemSwitchs)
        {
          if (_barItemSwitchs.Contains(e.Item))
            return null;
          _barItemSwitchs.Add(e.Item);
          e.Item.Enabled = false;
        }
      try
      {
        Phenix.Core.Windows.ControlHelper.ResetFocus(Form);
        BarItemClickEventArgs result = new BarItemClickEventArgs(e != null ? e.Item : null, source);
        if (CheckValid(source, false))
        {
          result.Succeed = false;
          return result;
        }
        if (AddCloning != null)
          AddCloning(this, result);
        if (result.Stop)
          return result;
        if (!result.Applied)
        {
          if (source == null)
          {
            result.Succeed = false;
            MessageBox.Show(Phenix.Windows.Properties.Resources.BindingSourcePropertyIsNull, Phenix.Windows.Properties.Resources.DataAdding, MessageBoxButtons.OK, MessageBoxIcon.Error);
          }
          else
          {
            BindingSource = source;
            result.Succeed = DoAdd(source, true);
          }
        }
        if (result.Succeed)
        {
          DoAdded(result.Source);
          if (AddCloned != null)
            AddCloned(this, result);
        }
        return result;
      }
      finally
      {
        ResetComponentAuthorizationRules();
        ResetRecordState();
        if (e != null)
          lock (_barItemSwitchs)
          {
            _barItemSwitchs.Remove(e.Item);
          }
      }
    }

    private bool DoAdd(BindingSource source, bool clone)
    {
      if (source == null)
        return false;
      IBusinessCollection businessList = BindingSourceHelper.GetDataSourceList(source) as IBusinessCollection;
      if (businessList == null)
        return false;
      if (!BeginEdit())
        return false;

      _bypassInvalidChecks = true;
      try
      {
        bool isMultiSelect = GridControlHelper.SetMultiSelect(_gridControls, source, false);
        try
        {
          int index = ToAddLast ? businessList.Count : source.Position + 1;
          if (clone)
            businessList.AddNew(index, BindingSourceHelper.GetDataSourceCurrent(source) as IBusinessObject);
          else
            businessList.AddNew(index);
          source.Position = index;
        }
        finally
        {
          if (isMultiSelect)
            GridControlHelper.SetMultiSelect(_gridControls, source, true);
        }
        return true;
      }
      finally
      {
        _bypassInvalidChecks = false;
      }
    }

    private void DoAdded(BindingSource source)
    {
      _bypassInvalidChecks = true;
      try
      {
        BindingSource = source;
        SetEditObject(source);
        OperateState = DataOperateState.Add;
        FocusEditableControl(source);
      }
      finally
      {
        _bypassInvalidChecks = false;
      }
    }

    /// <summary>
    /// 点击编辑按钮（线程安全）
    /// e = null
    /// source = BindingSource
    /// </summary>
    public void InvokeClickModifyButton()
    {
      if (this.Form.InvokeRequired)
        this.Form.BeginInvoke(new Function<BarItemClickEventArgs>(ClickModifyButton));
      else
        ClickModifyButton();
    }

    /// <summary>
    /// 点击编辑按钮
    /// e = null
    /// source = BindingSource
    /// </summary>
    public BarItemClickEventArgs ClickModifyButton()
    {
      return ClickModifyButton(null, BindingSource);
    }

    /// <summary>
    /// 点击编辑按钮
    /// source = BindingSource
    /// </summary>
    /// <param name="e">点击按钮事件数据</param>
    public BarItemClickEventArgs ClickModifyButton(ItemClickEventArgs e)
    {
      return ClickModifyButton(e, BindingSource);
    }

    /// <summary>
    /// 点击编辑按钮
    /// e = null
    /// </summary>
    /// <param name="source">数据源</param>
    public BarItemClickEventArgs ClickModifyButton(BindingSource source)
    {
      return ClickModifyButton(null, source);
    }

    /// <summary>
    /// 点击编辑按钮
    /// </summary>
    /// <param name="e">点击按钮事件数据</param>
    /// <param name="source">数据源</param>
    public BarItemClickEventArgs ClickModifyButton(ItemClickEventArgs e, BindingSource source)
    {
      if (e != null)
        lock (_barItemSwitchs)
        {
          if (_barItemSwitchs.Contains(e.Item))
            return null;
          _barItemSwitchs.Add(e.Item);
          e.Item.Enabled = false;
        }
      try
      {
        Phenix.Core.Windows.ControlHelper.ResetFocus(Form);
        BarItemClickEventArgs result = new BarItemClickEventArgs(e != null ? e.Item : null, source);
        if (Modifying != null)
          Modifying(this, result);
        if (result.Stop)
          return result;
        if (!result.Applied)
        {
          if (source == null)
          {
            result.Succeed = false;
            MessageBox.Show(Phenix.Windows.Properties.Resources.BindingSourcePropertyIsNull, Phenix.Windows.Properties.Resources.DataModifying, MessageBoxButtons.OK, MessageBoxIcon.Error);
          }
          else
          {
            BindingSource = source;
            result.Succeed = DoModify(source);
          }
        }
        if (result.Succeed)
        {
          DoModified(result.Source);
          if (Modified != null)
            Modified(this, result);
        }
        return result;
      }
      finally
      {
        ResetComponentAuthorizationRules();
        ResetRecordState();
        if (e != null)
          lock (_barItemSwitchs)
          {
            _barItemSwitchs.Remove(e.Item);
          }
      }
    }

    private bool DoModify(BindingSource source)
    {
      if (source == null)
        return false;
      if (!BeginEdit())
        return false;
      return true;
    }

    private void DoModified(BindingSource source)
    {
      BindingSource = source;
      SetEditObject(source);
      OperateState = DataOperateState.Modify;
      FocusEditableControl(source);
    }

    /// <summary>
    /// 点击删除按钮（线程安全）
    /// e = null
    /// source = BindingSource
    /// needPrompt = PromptInDeleting
    /// </summary>
    public void InvokeClickDeleteButton()
    {
      if (this.Form.InvokeRequired)
        this.Form.BeginInvoke(new Function<BarItemDeleteEventArgs>(ClickDeleteButton));
      else
        ClickDeleteButton();
    }

    /// <summary>
    /// 点击删除按钮
    /// e = null
    /// source = BindingSource
    /// needPrompt = PromptInDeleting
    /// </summary>
    public BarItemDeleteEventArgs ClickDeleteButton()
    {
      return ClickDeleteButton(null, BindingSource, PromptInDeleting);
    }

    /// <summary>
    /// 点击删除按钮
    /// source = BindingSource
    /// needPrompt = PromptInDeleting
    /// </summary>
    /// <param name="e">点击按钮事件数据</param>
    public BarItemDeleteEventArgs ClickDeleteButton(ItemClickEventArgs e)
    {
      return ClickDeleteButton(e, BindingSource, PromptInDeleting);
    }

    /// <summary>
    /// 点击删除按钮
    /// source = BindingSource
    /// </summary>
    /// <param name="e">点击按钮事件数据</param>
    /// <param name="needPrompt">是否需要提示</param>
    public BarItemDeleteEventArgs ClickDeleteButton(ItemClickEventArgs e, bool needPrompt)
    {
      return ClickDeleteButton(e, BindingSource, needPrompt);
    }

    /// <summary>
    /// 点击删除按钮
    /// e = null
    /// source = BindingSource
    /// </summary>
    /// <param name="needPrompt">是否需要提示</param>
    public BarItemDeleteEventArgs ClickDeleteButton(bool needPrompt)
    {
      return ClickDeleteButton(null, BindingSource, needPrompt);
    }

    /// <summary>
    /// 点击删除按钮
    /// e = null
    /// needPrompt = PromptInDeleting
    /// </summary>
    /// <param name="source">数据源</param>
    public BarItemDeleteEventArgs ClickDeleteButton(BindingSource source)
    {
      return ClickDeleteButton(null, source, PromptInDeleting);
    }

    /// <summary>
    /// 点击删除按钮
    /// e = null
    /// </summary>
    /// <param name="source">数据源</param>
    /// <param name="needPrompt">是否需要提示</param>
    public BarItemDeleteEventArgs ClickDeleteButton(BindingSource source, bool needPrompt)
    {
      return ClickDeleteButton(null, source, needPrompt);
    }

    /// <summary>
    /// 点击删除按钮
    /// </summary>
    /// <param name="e">点击按钮事件数据</param>
    /// <param name="source">数据源</param>
    /// <param name="needPrompt">是否需要提示</param>
    public BarItemDeleteEventArgs ClickDeleteButton(ItemClickEventArgs e, BindingSource source, bool needPrompt)
    {
      if (e != null)
        lock (_barItemSwitchs)
        {
          if (_barItemSwitchs.Contains(e.Item))
            return null;
          _barItemSwitchs.Add(e.Item);
          e.Item.Enabled = false;
        }
      try
      {
        Phenix.Core.Windows.ControlHelper.ResetFocus(Form);
        BarItemDeleteEventArgs result = new BarItemDeleteEventArgs(e != null ? e.Item : null, source);
        if (Deleting != null)
          Deleting(this, result);
        if (result.Stop)
          return result;
        if (!result.Applied)
        {
          if (source == null)
          {
            result.Succeed = false;
            MessageBox.Show(Phenix.Windows.Properties.Resources.BindingSourcePropertyIsNull, Phenix.Windows.Properties.Resources.DataDeleting, MessageBoxButtons.OK, MessageBoxIcon.Error);
          }
          else
          {
            BindingSource = source;
            result.Succeed = DoDelete(source, needPrompt, result.NeedCheckDirty, result.OnlySaveSelected, result.GetFirstTransactionData(), result.GetLastTransactionData());
          }
        }
        if (result.Succeed)
        {
          DoDeleted(result.Source);
          if (Deleted != null)
            Deleted(this, result);
        }
        else
        {
          if (DeleteRejected != null)
            DeleteRejected(this, result);
        }
        return result;
      }
      finally
      {
        ResetComponentAuthorizationRules();
        ResetRecordState();
        if (e != null)
          lock (_barItemSwitchs)
          {
            _barItemSwitchs.Remove(e.Item);
          }
      }
    }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
    private bool DoDelete(BindingSource source, bool needPrompt, bool needCheckDirty, bool? onlySaveSelected, IBusiness[] firstTransactionData, IBusiness[] lastTransactionData)
    {
      if (source == null || source.Count == 0)
        return false;
      IBusinessCollection businessList = BindingSourceHelper.GetDataSourceList(source) as IBusinessCollection;
      if (businessList == null || businessList.Count == 0)
        return false;
      IBusinessObject currentBusiness = BindingSourceHelper.GetDataSourceCurrent(source) as IBusinessObject;
      if (currentBusiness == null)
        return false;
      IBusiness currentBusinessRoot = CurrentBusinessRoot;
      if (currentBusinessRoot == null)
        return false;

      bool immediateSave = !currentBusinessRoot.EditMode || currentBusinessRoot.NotUndoable;
      if (needPrompt)
        if (immediateSave)
        {
          if (MessageBox.Show(String.Format(Phenix.Windows.Properties.Resources.ConfirmDelete, businessList.Caption, currentBusiness.Caption), Phenix.Windows.Properties.Resources.DataDelete,
            MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.No)
            return false;
        }
        else
        {
          if (MessageBox.Show(String.Format(Phenix.Windows.Properties.Resources.ConfirmDeleteMultistepSubmit, businessList.Caption, currentBusiness.Caption), Phenix.Windows.Properties.Resources.DataDelete,
            MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.No)
            return false;
        }

      string primaryKey = currentBusiness.PrimaryKey;
      _bypassInvalidChecks = true;
      try
      {
        try
        {
          if (immediateSave)
            BeginEdit();
          IList<object> multiSelected = GridControlHelper.FindMultiSelected(_gridControls, source);
          if (multiSelected != null && multiSelected.Count > 0)
            foreach (object item in multiSelected)
              businessList.Remove(item);
          else
            businessList.Remove(currentBusiness);
          if (immediateSave)
            if (!DoSave(needPrompt, needCheckDirty, onlySaveSelected, firstTransactionData, lastTransactionData))
              goto Label;
        }
        catch (Exception ex)
        {
          string hint = String.Format(Phenix.Windows.Properties.Resources.DataDeleteAborted, businessList.Caption, currentBusiness.Caption, AppUtilities.GetErrorHint(ex));
          ShowHint(hint);
          MessageBox.Show(hint, Phenix.Windows.Properties.Resources.DataDelete, MessageBoxButtons.OK, MessageBoxIcon.Error);
          goto Label;
        }
        return true;
      }
      finally
      {
        _bypassInvalidChecks = false;
      }

      Label:
      if (immediateSave)
        CancelEdit();
      if (!String.IsNullOrEmpty(primaryKey) && source.Count > 0 && source.Count <= LocatePositionMaximum)
      {
        currentBusiness = businessList.FindItem(primaryKey);
        BindingSourceHelper.LocatePosition(source, currentBusiness, LocatePositionMaximum);
      }
      return false;
    }

    private void DoDeleted(BindingSource source)
    {
      BindingSource = source;
      if (EditMode)
      {
        SetEditObject(source);
        OperateState = DataOperateState.Delete;
      }
      else
      {
        ClearEditObject();
        OperateState = DataOperateState.Browse;
      }
    }

    /// <summary>
    /// 点击定位按钮（线程安全）
    /// e = null
    /// </summary>
    public void InvokeClickLocateButton()
    {
      if (this.Form.InvokeRequired)
        this.Form.BeginInvoke(new Function<BarItemClickEventArgs>(ClickLocateButton));
      else
        ClickLocateButton();
    }

    /// <summary>
    /// 点击定位按钮
    /// e = null
    /// </summary>
    public BarItemClickEventArgs ClickLocateButton()
    {
      return ClickLocateButton(null);
    }

    /// <summary>
    /// 点击定位按钮
    /// </summary>
    /// <param name="e">点击按钮事件数据</param>
    public BarItemClickEventArgs ClickLocateButton(ItemClickEventArgs e)
    {
      BarItemClickEventArgs result = new BarItemClickEventArgs(e != null ? e.Item : null, BindingSource);
      if (EditBindingSource == null)
      {
        result.Succeed = false;
        return result;
      }
      if (Locating != null)
        Locating(this, result);
      if (result.Stop)
        return result;
      if (!result.Applied)
        result.Succeed = LocateEditObject();
      if (result.Succeed)
      {
        if (Located != null)
          Located(this, result);
      }
      return result;
    }

    /// <summary>
    /// 点击取消按钮（线程安全）
    /// e = null
    /// needPrompt = PromptInCanceling
    /// </summary>
    public void InvokeClickCancelButton()
    {
      if (this.Form.InvokeRequired)
        this.Form.BeginInvoke(new Function<BarItemClickEventArgs>(ClickCancelButton));
      else
        ClickCancelButton();
    }

    /// <summary>
    /// 点击取消按钮
    /// e = null
    /// needPrompt = PromptInCanceling
    /// </summary>
    public BarItemClickEventArgs ClickCancelButton()
    {
      return ClickCancelButton(null, PromptInCanceling);
    }

    /// <summary>
    /// 点击取消按钮
    /// e = null
    /// </summary>
    /// <param name="needPrompt">是否需要提示</param>
    public BarItemClickEventArgs ClickCancelButton(bool needPrompt)
    {
      return ClickCancelButton(null, needPrompt);
    }

    /// <summary>
    /// 点击取消按钮
    /// needPrompt = PromptInCanceling
    /// </summary>
    /// <param name="e">点击按钮事件数据</param>
    public BarItemClickEventArgs ClickCancelButton(ItemClickEventArgs e)
    {
      return ClickCancelButton(e, PromptInCanceling);
    }

    /// <summary>
    /// 点击取消按钮
    /// </summary>
    /// <param name="e">点击按钮事件数据</param>
    /// <param name="needPrompt">是否需要提示</param>
    public BarItemClickEventArgs ClickCancelButton(ItemClickEventArgs e, bool needPrompt)
    {
      if (e != null)
        lock (_barItemSwitchs)
        {
          if (_barItemSwitchs.Contains(e.Item))
            return null;
          _barItemSwitchs.Add(e.Item);
          e.Item.Enabled = false;
        }
      try
      {
        Phenix.Core.Windows.ControlHelper.ResetFocus(Form);
        BarItemClickEventArgs result = new BarItemClickEventArgs(e != null ? e.Item : null, BindingSource);
        if (Canceling != null)
          Canceling(this, result);
        if (result.Stop)
          return result;
        if (!result.Applied)
          result.Succeed = DoCancel(needPrompt);
        if (result.Succeed)
        {
          DoCanceled();
          if (Canceled != null)
            Canceled(this, result);
        }
        else
        {
          if (CancelRejected != null)
            CancelRejected(this, result);
        }
        return result;
      }
      finally
      {
        ResetComponentAuthorizationRules();
        ResetRecordState();
        if (e != null)
          lock (_barItemSwitchs)
          {
            _barItemSwitchs.Remove(e.Item);
          }
      }
    }

    private bool DoCancel(bool needPrompt)
    {
      IBusiness currentBusinessRoot = CurrentBusinessRoot;
      if (currentBusinessRoot == null)
        return false;

      if (needPrompt)
        if (MessageBox.Show(String.Format(Phenix.Windows.Properties.Resources.ConfirmCancel, currentBusinessRoot.Caption), Phenix.Windows.Properties.Resources.DataModifying,
          MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.No)
          return false;

      _bypassInvalidChecks = true;
      try
      {
        CancelEdit();
        if (!AllowMultistepSubmit)
          LocateEditObject();
        return true;
      }
      finally
      {
        _bypassInvalidChecks = false;
      }
    }

    private void DoCanceled()
    {
      if (NotUndoable)
        OperateState = DataOperateState.Modify;
      else 
      {
        ClearEditObject();
        OperateState = DataOperateState.Browse;
        ShowHint(String.Format(Phenix.Windows.Properties.Resources.DataCanceled, CurrentBusinessRoot.Caption));
      }
    }

    /// <summary>
    /// 点击保存按钮（线程安全）
    /// e = null
    /// needPrompt = PromptInSaved
    /// </summary>
    public void InvokeClickSaveButton()
    {
      if (this.Form.InvokeRequired)
        this.Form.BeginInvoke(new Function<BarItemSaveEventArgs>(ClickSaveButton));
      else
        ClickSaveButton();
    }

    /// <summary>
    /// 点击保存按钮
    /// e = null
    /// needPrompt = PromptInSaved
    /// </summary>
    public BarItemSaveEventArgs ClickSaveButton()
    {
      return ClickSaveButton(null, PromptInSaved);
    }

    /// <summary>
    /// 点击保存按钮
    /// e = null
    /// </summary>
    /// <param name="needPrompt">是否需要提示</param>
    public BarItemSaveEventArgs ClickSaveButton(bool needPrompt)
    {
      return ClickSaveButton(null, needPrompt);
    }

    /// <summary>
    /// 点击保存按钮
    /// needPrompt = PromptInCanceling
    /// </summary>
    /// <param name="e">点击按钮事件数据</param>
    public BarItemSaveEventArgs ClickSaveButton(ItemClickEventArgs e)
    {
      return ClickSaveButton(e, PromptInSaved);
    }

    /// <summary>
    /// 点击保存按钮
    /// </summary>
    /// <param name="e">点击按钮事件数据</param>
    /// <param name="needPrompt">是否需要提示</param>
    public BarItemSaveEventArgs ClickSaveButton(ItemClickEventArgs e, bool needPrompt)
    {
      if (e != null)
        lock (_barItemSwitchs)
        {
          if (_barItemSwitchs.Contains(e.Item))
            return null;
          _barItemSwitchs.Add(e.Item);
          e.Item.Enabled = false;
        }
      try
      {
        Phenix.Core.Windows.ControlHelper.ResetFocus(Form);
        //for Developer Express .NET
        GridControlHelper.PostEditor(_gridControls);
        VGridControlHelper.PostEditor(_vGridControls);

        BarItemSaveEventArgs result = new BarItemSaveEventArgs(e != null ? e.Item : null, BindingSource);
        if (Saving != null)
          Saving(this, result);
        if (result.Stop)
          return result;
        if (!result.Applied)
          result.Succeed = DoSave(needPrompt, result.NeedCheckDirty, result.OnlySaveSelected, result.GetFirstTransactionData(), result.GetLastTransactionData());
        if (result.Succeed)
        {
          DoSaved();
          if (Saved != null)
            Saved(this, result);
        }
        else
          ClickLocateButton();
        return result;
      }
      finally
      {
        ResetComponentAuthorizationRules();
        ResetRecordState();
        if (e != null)
          lock (_barItemSwitchs)
          {
            _barItemSwitchs.Remove(e.Item);
          }
      }
    }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
    private bool DoSave(bool needPrompt, bool needCheckDirty, bool? onlySaveSelected, IBusiness[] firstTransactionData, IBusiness[] lastTransactionData)
    {
      IBusiness currentBusinessRoot = CurrentBusinessRoot;
      if (currentBusinessRoot == null)
        return false;
      if (!String.IsNullOrEmpty(GroupName))
      {
        List<IBusiness> roots = lastTransactionData != null ? new List<IBusiness>(lastTransactionData) : new List<IBusiness>();
        foreach (BindingSourceStatus item in BindingSources)
          if (item.BindingSource != null && String.CompareOrdinal(item.GroupName, GroupName) == 0)
          {
            IBusiness businessRoot = item.FindBusinessRoot();
            if (businessRoot != null && businessRoot != currentBusinessRoot && (businessRoot.EditMode || businessRoot.NotUndoable))
              roots.Add(businessRoot);
          }
        if (roots.Count > 0)
          lastTransactionData = roots.ToArray();
      }
      do
      {
        try
        {
          using (new DevExpress.Utils.WaitDialogForm(String.Format(Phenix.Windows.Properties.Resources.DataSaving, currentBusinessRoot.Caption, AppConfig.Debugging ? currentBusinessRoot.GetType().Name : null), Phenix.Core.Properties.Resources.PleaseWait))
          {
            try
            {
              currentBusinessRoot.Save(needCheckDirty, onlySaveSelected, firstTransactionData, lastTransactionData);
            }
            catch (Exception ex)
            {
              ExceptionEventArgs e = new ExceptionEventArgs(currentBusinessRoot, ex);
              OnSaveFailed(e);
              if (!e.Succeed)
                if (e.Applied)
                  return false;
                else
                  throw;
            }
          }
          string hint = String.Format(Phenix.Windows.Properties.Resources.DataSaveSucceed, currentBusinessRoot.Caption);
          ShowHint(hint);
          if (needPrompt)
            MessageBox.Show(hint, Phenix.Windows.Properties.Resources.DataSave, MessageBoxButtons.OK, MessageBoxIcon.Information);
          return true;
        }
        catch (CheckDirtyException ex)
        {
          string hint = String.Format(Phenix.Windows.Properties.Resources.DataSaveForcibly,
            currentBusinessRoot.Caption,
            AppConfig.Debugging ? currentBusinessRoot.GetType().FullName : null,
            AppUtilities.GetErrorHint(ex));
          if (MessageBox.Show(hint, Phenix.Windows.Properties.Resources.DataSave, MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
            return false;
          needCheckDirty = false;
          continue;
        }
        catch (AuthenticationException ex)
        {
          string hint = AppUtilities.GetErrorHint(ex);
          ShowHint(hint);
          MessageBox.Show(hint, Phenix.Windows.Properties.Resources.DataSave, MessageBoxButtons.OK, MessageBoxIcon.Error);
          Application.Exit();
          return false;
        }
        catch (Exception ex)
        {
          string hint = String.Format(Phenix.Windows.Properties.Resources.DataSaveAborted,
            currentBusinessRoot.Caption,
            AppConfig.Debugging ? currentBusinessRoot.GetType().FullName : null,
            AppUtilities.GetErrorHint(ex, typeof(Csla.DataPortalException), typeof(Csla.Reflection.CallMethodException)));
          ShowHint(hint);
          MessageBox.Show(hint, Phenix.Windows.Properties.Resources.DataSave, MessageBoxButtons.OK, MessageBoxIcon.Error);
          CheckAllValid(MasterBindingSource, false);
          CheckSaveException(ex);
          return false;
        }
      } while (true);
    }

    private void CheckSaveException(Exception error)
    {
      CheckRepeatedException checkRepeatedException = AppUtilities.FindException<CheckRepeatedException>(error);
      if (checkRepeatedException != null && checkRepeatedException.Entity != null)
      {
        Type type = checkRepeatedException.Entity.GetType();
        foreach (BindingSourceStatus item in BindingSources)
          if (item.BindingSourceCoreType == type)
          {
            if (MessageBox.Show(String.Format(Phenix.Windows.Properties.Resources.ConfirmExecute, Phenix.Windows.Properties.Resources.DataRestore),
              Phenix.Windows.Properties.Resources.DataSave, MessageBoxButtons.YesNo, MessageBoxIcon.Error) != DialogResult.Yes)
              return;
            checkRepeatedException.Entity.IsDisabled = false;
            IBusinessObject repeatedBusiness = item.CurrentBusinessList.FindItem(checkRepeatedException.Entity.PrimaryKey);
            if (repeatedBusiness == null)
            {
              repeatedBusiness = (IBusinessObject)checkRepeatedException.Entity;
              item.CurrentBusinessList.Add(repeatedBusiness);
            }
            item.CurrentBusiness = repeatedBusiness;
            BindingSource = item.BindingSource;
            break;
          }
      }
    }

    private void DoSaved()
    {
      if (NotUndoable)
        OperateState = DataOperateState.Modify;
      else if (AutoEditOnSaved)
        ClickModifyButton();
      else
      {
        ClearEditObject();
        OperateState = DataOperateState.Browse;
      }
    }

    /// <summary>
    /// 点击导入按钮（线程安全）
    /// e = null
    /// source = BindingSource
    /// </summary>
    public void InvokeClickImportButton()
    {
      if (this.Form.InvokeRequired)
        this.Form.BeginInvoke(new Function<BarItemImportEventArgs>(ClickImportButton));
      else
        ClickImportButton();
    }

    /// <summary>
    /// 点击导入按钮
    /// e = null
    /// source = BindingSource
    /// </summary>
    public BarItemImportEventArgs ClickImportButton()
    {
      return ClickImportButton(null, BindingSource);
    }

    /// <summary>
    /// 点击导入按钮
    /// source = BindingSource
    /// </summary>
    /// <param name="e">点击按钮事件数据</param>
    public BarItemImportEventArgs ClickImportButton(ItemClickEventArgs e)
    {
      return ClickImportButton(e, BindingSource);
    }

    /// <summary>
    /// 点击导入按钮
    /// e = null
    /// </summary>
    /// <param name="source">数据源</param>
    public BarItemImportEventArgs ClickImportButton(BindingSource source)
    {
      return ClickImportButton(null, source);
    }

    /// <summary>
    /// 点击导入按钮
    /// </summary>
    /// <param name="e">点击按钮事件数据</param>
    /// <param name="source">数据源</param>
    public BarItemImportEventArgs ClickImportButton(ItemClickEventArgs e, BindingSource source)
    {
      if (e != null)
        lock (_barItemSwitchs)
        {
          if (_barItemSwitchs.Contains(e.Item))
            return null;
          _barItemSwitchs.Add(e.Item);
          e.Item.Enabled = false;
        }
      try
      {
        Phenix.Core.Windows.ControlHelper.ResetFocus(Form);
        BarItemImportEventArgs result = new BarItemImportEventArgs(e != null ? e.Item : null, source);
        if (Importing != null)
          Importing(this, result);
        if (result.Stop)
          return result;
        if (!result.Applied)
        {
          if (source == null)
          {
            result.Succeed = false;
            MessageBox.Show(Phenix.Windows.Properties.Resources.BindingSourcePropertyIsNull, Phenix.Windows.Properties.Resources.DataImporting, MessageBoxButtons.OK, MessageBoxIcon.Error);
          }
          else
          {
            BindingSource = source;
            result.Succeed = DoImport(source, result.SheetName, result.PropertyInfos);
          }
        }
        if (result.Succeed)
        {
          DoAdded(result.Source);
          if (Imported != null)
            Imported(this, result);
        }
        return result;
      }
      finally
      {
        ResetComponentAuthorizationRules();
        if (e != null)
          lock (_barItemSwitchs)
          {
            _barItemSwitchs.Remove(e.Item);
          }
      }
    }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
    private bool DoImport(BindingSource source, string sheetName, IList<Phenix.Core.Mapping.IPropertyInfo> propertyInfos)
    {
      Type dataSourceType = BindingSourceHelper.GetDataSourceType(source);
      if (dataSourceType == null)
        return false;
      if (!typeof(IBusinessCollection).IsAssignableFrom(dataSourceType))
        return false;

      bool isFirst = true;
      do
      {
        DataTable dataTable = Phenix.Windows.Helper.ExcelHelper.Import(sheetName);
        if (dataTable == null)
          return false;
        try
        {
          IBusinessCollection businessList;
          DateTime startDateTime = DateTime.Now;
          using (new DevExpress.Utils.WaitDialogForm(String.Format(Phenix.Windows.Properties.Resources.DataFetching, AppConfig.Debugging ? dataSourceType.Name : null), Phenix.Core.Properties.Resources.PleaseWait))
          {
            businessList = dataSourceType.InvokeMember("New",
              BindingFlags.Static | BindingFlags.Public | BindingFlags.FlattenHierarchy | BindingFlags.InvokeMethod, null, null,
              new object[] { dataTable, propertyInfos }) as IBusinessCollection;
          }
          DateTime fillDateTime = DateTime.Now;
          using (new DevExpress.Utils.WaitDialogForm(String.Format(Phenix.Windows.Properties.Resources.BindingSourceFilling, businessList.Caption, AppConfig.Debugging ? dataSourceType.Name : null), Phenix.Core.Properties.Resources.PleaseWait))
          {
            source.DataSource = businessList;
            if (source.Count == 0)
              source.Position = -1;
            else
              source.Position = 0;
          }
          DateTime endDateTime = DateTime.Now;
          ShowHint(String.Format(Phenix.Windows.Properties.Resources.DataImportSucceed, businessList.Caption, businessList.Count,
            fillDateTime.Subtract(startDateTime).TotalSeconds, endDateTime.Subtract(fillDateTime).TotalSeconds,
            endDateTime.Subtract(startDateTime).TotalSeconds));
          return true;
        }
        catch (InvalidOperationException)
        {
          string key = String.Format("SheetName_{0}", dataSourceType.FullName);
          if (isFirst)
          {
            isFirst = false;
            sheetName = AppSettings.ReadValue(key);
          }
          else
          {
            bool saveSheetConfig = true;
            if (!SheetNameSetDialog.Execute(ref sheetName, ref saveSheetConfig))
              throw;
            if (saveSheetConfig)
              AppSettings.SaveValue(key, sheetName);
          }
        }
        catch (Exception ex)
        {
          string hint = String.Format(Phenix.Windows.Properties.Resources.DataFetchAborted, AppUtilities.GetErrorHint(ex));
          ShowHint(hint);
          MessageBox.Show(hint, Phenix.Windows.Properties.Resources.DataImport, MessageBoxButtons.OK, MessageBoxIcon.Error);
          return false;
        }
      } while (true);
    }

    /// <summary>
    /// 点击导出按钮（线程安全）
    /// e = null
    /// </summary>
    public void InvokeClickExportButton()
    {
      if (this.Form.InvokeRequired)
        this.Form.BeginInvoke(new Function<BarItemClickEventArgs>(ClickExportButton));
      else
        ClickExportButton();
    }

    /// <summary>
    /// 点击导出按钮
    /// e = null
    /// </summary>
    public BarItemClickEventArgs ClickExportButton()
    {
      return ClickExportButton(null);
    }

    /// <summary>
    /// 点击导出按钮
    /// </summary>
    /// <param name="e">点击按钮事件数据</param>
    public BarItemClickEventArgs ClickExportButton(ItemClickEventArgs e)
    {
      if (e != null)
        lock (_barItemSwitchs)
        {
          if (_barItemSwitchs.Contains(e.Item))
            return null;
          _barItemSwitchs.Add(e.Item);
          e.Item.Enabled = false;
        }
      try
      {
        Phenix.Core.Windows.ControlHelper.ResetFocus(Form);
        BarItemClickEventArgs result = new BarItemClickEventArgs(e != null ? e.Item : null, BindingSource);
        if (Exporting != null)
          Exporting(this, result);
        if (result.Stop)
          return result;
        if (!result.Applied)
          result.Succeed = DoExport(BindingSource);
        if (result.Succeed)
        {
          if (Exported != null)
            Exported(this, result);
        }
        return result;
      }
      finally
      {
        ResetComponentAuthorizationRules();
        if (e != null)
          lock (_barItemSwitchs)
          {
            _barItemSwitchs.Remove(e.Item);
          }
      }
    }

    private bool DoExport(BindingSource source)
    {
      Form.Cursor = Cursors.WaitCursor;
      try
      {
        IBusinessCollection businessList = BindingSourceHelper.GetDataSourceList(source) as IBusinessCollection;
        bool itemLazyGetDetail = false;
        if (businessList != null)
        {
          itemLazyGetDetail = businessList.ItemLazyGetDetail;
          businessList.ItemLazyGetDetail = true;
        }
        try
        {
          //for Developer Express .NET
          GridControl gridControl = GridControlHelper.FindGridControl(_gridControls, source);
          if (gridControl != null)
            return GridControlHelper.Export(gridControl);
          VGridControl vGridControl = VGridControlHelper.FindGridControl(_vGridControls, source);
          if (vGridControl != null)
            return VGridControlHelper.Export(vGridControl);
          return false;
        }
        finally
        {
          if (businessList != null)
            businessList.ItemLazyGetDetail = itemLazyGetDetail;
        }
      }
      finally
      {
        Form.Cursor = Cursors.Default;
      }
    }

    /// <summary>
    /// 点击打印按钮（线程安全）
    /// e = null
    /// </summary>
    public void InvokeClickPrintButton()
    {
      if (this.Form.InvokeRequired)
        this.Form.BeginInvoke(new Function<BarItemClickEventArgs>(ClickPrintButton));
      else
        ClickPrintButton();
    }

    /// <summary>
    /// 点击打印按钮
    /// e = null
    /// </summary>
    public BarItemClickEventArgs ClickPrintButton()
    {
      return ClickPrintButton(null);
    }

    /// <summary>
    /// 点击打印按钮
    /// </summary>
    /// <param name="e">点击按钮事件数据</param>
    public BarItemClickEventArgs ClickPrintButton(ItemClickEventArgs e)
    {
      if (e != null)
        lock (_barItemSwitchs)
        {
          if (_barItemSwitchs.Contains(e.Item))
            return null;
          _barItemSwitchs.Add(e.Item);
          e.Item.Enabled = false;
        }
      try
      {
        Phenix.Core.Windows.ControlHelper.ResetFocus(Form);
        BarItemClickEventArgs result = new BarItemClickEventArgs(e != null ? e.Item : null, BindingSource);
        if (Printing != null)
          Printing(this, result);
        if (result.Stop)
          return result;
        if (!result.Applied)
          result.Succeed = DoPrint(BindingSource);
        if (result.Succeed)
        {
          if (Printed != null)
            Printed(this, result);
        }
        return result;
      }
      finally
      {
        ResetComponentAuthorizationRules();
        if (e != null)
          lock (_barItemSwitchs)
          {
            _barItemSwitchs.Remove(e.Item);
          }
      }
    }

    private bool DoPrint(BindingSource source)
    {
      Form.Cursor = Cursors.WaitCursor;
      try
      {
        IBusinessCollection businessList = BindingSourceHelper.GetDataSourceList(source) as IBusinessCollection;
        bool itemLazyGetDetail = false;
        if (businessList != null)
        {
          itemLazyGetDetail = businessList.ItemLazyGetDetail;
          businessList.ItemLazyGetDetail = true;

          if (ReportType != null)
          {
            XtraReportHelper.ShowPreviewDialog(ReportType, businessList.Root);
            return true;
          }
        }
        try
        {
          //for Developer Express .NET
          GridControl gridControl = GridControlHelper.FindGridControl(_gridControls, source);
          if (gridControl != null)
          {
            gridControl.ShowPrintPreview();
            return true;
          }
          VGridControl vGridControl = VGridControlHelper.FindGridControl(_vGridControls, source);
          if (vGridControl != null)
          {
            vGridControl.ShowPrintPreview();
            return true;
          }
          return false;
        }
        finally
        {
          if (businessList != null)
            businessList.ItemLazyGetDetail = itemLazyGetDetail;
        }
      }
      finally
      {
        Form.Cursor = Cursors.Default;
      }
    }

    /// <summary>
    /// 点击帮助按钮（线程安全）
    /// e = null
    /// </summary>
    public void InvokeClickHelpButton()
    {
      if (this.Form.InvokeRequired)
        this.Form.BeginInvoke(new Function<BarItemClickEventArgs>(ClickHelpButton));
      else
        ClickHelpButton();
    }

    /// <summary>
    /// 点击帮助按钮
    /// e = null
    /// </summary>
    public BarItemClickEventArgs ClickHelpButton()
    {
      return ClickHelpButton(null);
    }

    /// <summary>
    /// 点击帮助按钮
    /// </summary>
    /// <param name="e">点击按钮事件数据</param>
    public BarItemClickEventArgs ClickHelpButton(ItemClickEventArgs e)
    {
      if (e != null)
        lock (_barItemSwitchs)
        {
          if (_barItemSwitchs.Contains(e.Item))
            return null;
          _barItemSwitchs.Add(e.Item);
          e.Item.Enabled = false;
        }
      try
      {
        Phenix.Core.Windows.ControlHelper.ResetFocus(Form);
        BarItemClickEventArgs args = new BarItemClickEventArgs(e != null ? e.Item : null, BindingSource);
        if (Helping != null)
          Helping(this, args);
        if (args.Succeed)
        {
          if (Helped != null)
            Helped(this, args);
        }
        return args;
      }
      finally 
      {
        if (e != null)
          lock (_barItemSwitchs)
          {
            _barItemSwitchs.Remove(e.Item);
            e.Item.Enabled = true;
          }
      }
    }

    /// <summary>
    /// 点击退出按钮（线程安全）
    /// e = null
    /// </summary>
    public void InvokeClickExitButton()
    {
      if (this.Form.InvokeRequired)
        this.Form.BeginInvoke(new Function<BarItemClickEventArgs>(ClickExitButton));
      else
        ClickExitButton();
    }

    /// <summary>
    /// 点击退出按钮
    /// e = null
    /// </summary>
    public BarItemClickEventArgs ClickExitButton()
    {
      return ClickExitButton(null);
    }

    /// <summary>
    /// 点击退出按钮
    /// </summary>
    /// <param name="e">点击按钮事件数据</param>
    public BarItemClickEventArgs ClickExitButton(ItemClickEventArgs e)
    {
      if (e != null)
        lock (_barItemSwitchs)
        {
          if (_barItemSwitchs.Contains(e.Item))
            return null;
          _barItemSwitchs.Add(e.Item);
          e.Item.Enabled = false;
        }
      try
      {
        Phenix.Core.Windows.ControlHelper.ResetFocus(Form);
        BarItemClickEventArgs args = new BarItemClickEventArgs(e != null ? e.Item : null, BindingSource);
        if (Exiting != null)
          Exiting(this, args);
        if (args.Stop)
          return args;
        if (!args.Applied)
        {
          Form form = Form as Form;
          if (form != null)
          {
            form.Close();
            args.Succeed = true;
          }
        }
        if (args.Succeed)
        {
          if (Exited != null)
            Exited(this, args);
        }
        return args;
      }
      finally 
      {
        if (e != null)
          lock (_barItemSwitchs)
          {
            _barItemSwitchs.Remove(e.Item);
            e.Item.Enabled = true;
          }
      }
    }

    private void SetEditObject(BindingSource source)
    {
      if (source == null)
        return;
      object currentObject = BindingSourceHelper.GetDataSourceCurrent(source);
      if (currentObject == null)
        return;
      List<object> editObjects;
      if (!_editObjectsCache.TryGetValue(source, out editObjects))
        editObjects  = new List<object>();
      if (!editObjects.Contains(currentObject))
        editObjects.Add(currentObject);
      _editObjectsCache[source] = editObjects;
      _editBindingSource = source;
      _editObject = currentObject;
    }

    private void ClearEditObject()
    {
      _editObjectsCache.Clear();
      _editBindingSource = null;
      _editObject = null;
    }

    /// <summary>
    /// 定位编辑对象
    /// </summary>
    public bool LocateEditObject()
    {
      int position = EditPosition;
      if (position >= 0)
      {
        EditBindingSource.Position = position;
        return true;
      }
      return false;
    }

    /// <summary>
    /// 校验控件容器内编辑控件的失效数据
    /// </summary>
    /// <param name="source">数据源</param>
    /// <param name="toFocused">聚焦失效控件</param>
    /// <returns>失效数据事件数据</returns>
    public DataInvalidEventArgs CheckRules(BindingSource source, bool toFocused)
    {
      if (source == null)
        throw new ArgumentNullException("source");
      foreach (BindingSourceStatus item in BindingSources)
        if (item.BindingSource == source || item.CriteriaBindingSource == source)
          return item.CheckRules(Form, toFocused, _dxErrorProvider);
      return _dxErrorProvider.CheckRules(Form, source, toFocused);
    }

    private void ExecuteCheckValid(object data)
    {
      Control control = (Control)data;
      if (this.Form.InvokeRequired)
        this.Form.BeginInvoke(new Action<Control>(CheckValid), control);
      else
        CheckValid(control);
    }

    /// <summary>
    /// 校验数据是否有效
    /// </summary>
    /// <param name="control">控件</param>
    /// <returns>有效</returns>
    public void CheckValid(Control control)
    {
      if (control == null)
        throw new ArgumentNullException("control");
      if (_bypassInvalidChecks)
        return;
      //编辑框
      foreach (Binding item in control.DataBindings)
      {
        if (String.IsNullOrEmpty(item.BindingMemberInfo.BindingField))
          continue;
        BindingSource bindingSource = item.DataSource as BindingSource;
        if (bindingSource == null)
          continue;
        string error;
        object obj = BindingSourceHelper.GetDataSourceCurrent(bindingSource);
        IBusinessObject business = obj as IBusinessObject;
        if (business != null)
        {
          if (!business.IsDirty)
            return;
          error = business.CheckRule(item.BindingMemberInfo.BindingField);
        }
        else
        {
          ICriteria criteria = obj as ICriteria;
          if (criteria != null)
            error = criteria.CheckRule(item.BindingMemberInfo.BindingField);
          else
            continue;
        }
        DataInvalidEventArgs e = _dxErrorProvider.CheckRule(control, true);
        if (e == null && !String.IsNullOrEmpty(error))
          e = new DataInvalidEventArgs(bindingSource, bindingSource.Position, null, error);
        if (e != null)
        {
          InvokeShowHint(String.Format(Phenix.Windows.Properties.Resources.RuleCheckHint, e.Message));
          OnDataInvalid(e);
        }
        else
          InvokeShowHint(Phenix.Windows.Properties.Resources.RuleChecked);
        return;
      }
    }

    /// <summary>
    /// 校验数据是否有效
    /// </summary>
    /// <param name="source">数据源</param>
    /// <param name="onlyOldError">仅检查原有错误</param>
    /// <returns>存在无效项</returns>
    public bool CheckValid(BindingSource source, bool onlyOldError)
    {
      if (source == null)
        throw new ArgumentNullException("source");
      if (_bypassInvalidChecks)
        return false;
      string error;
      object obj = BindingSourceHelper.GetDataSourceCurrent(source);
      IBusinessObject business = obj as IBusinessObject;
      if (business != null)
      {
        if (!business.IsDirty)
          return false;
        error = business.CheckRules(onlyOldError);
      }
      else
      {
        ICriteria criteria = obj as ICriteria;
        if (criteria != null)
          error = criteria.CheckRules(false);
        else
          return false;
      }
      DataInvalidEventArgs e = CheckRules(source, true);
      if (e == null && !String.IsNullOrEmpty(error))
        e = new DataInvalidEventArgs(source, source.Position, null, error);
      if (e != null)
      {
        InvokeShowHint(String.Format(Phenix.Windows.Properties.Resources.RuleCheckHint, e.Message));
        OnDataInvalid(e);
        return true;
      }
      InvokeShowHint(Phenix.Windows.Properties.Resources.RuleChecked);
      return false;
    }

    /// <summary>
    /// 校验数据是否有效
    /// </summary>
    /// <param name="source">数据源</param>
    /// <param name="onlyOldError">仅检查原有错误</param>
    /// <returns>存在无效项</returns>
    public bool CheckAllValid(BindingSource source, bool onlyOldError)
    {
      if (source == null)
        throw new ArgumentNullException("source");
      if (_bypassInvalidChecks)
        return false;
      IBusinessCollection businessCollection = BindingSourceHelper.GetDataSourceList(source) as IBusinessCollection;
      if (businessCollection != null)
      {
        if (!businessCollection.IsDirty)
          return false;
        IBusinessObject invalidItem = businessCollection.FindInvalidItem(onlyOldError);
        if (invalidItem != null)
          source.Position = source.IndexOf(invalidItem);
      }
      return CheckValid(source, onlyOldError);
    }

    /// <summary>
    /// 重置控件读写授权
    /// </summary>
    /// <param name="sources">数据源队列</param>
    public void ResetControlAuthorizationRules(params BindingSource[] sources)
    {
      ResetControlAuthorizationRules(ReadOnly, sources ?? new BindingSource[] { BindingSource });
    }

    /// <summary>
    /// 重置控件读写授权
    /// </summary>
    /// <param name="readOnly">只读</param>
    /// <param name="sources">数据源队列</param>
    public void ResetControlAuthorizationRules(bool readOnly, params BindingSource[] sources)
    {
      if (sources == null)
        ReadWriteAuthorization.ResetControlAuthorizationRules(Form, EditMode || NotUndoable, readOnly, null);
      else if (sources.Length == 0)
      {
        foreach (BindingSourceStatus item in BindingSources)
          if (item.BindingSource != null)
            item.ResetControlAuthorizationRules(Form, EditMode || NotUndoable, readOnly);
      }
      else
        foreach (BindingSource bindingSource in sources)
          if (bindingSource != null)
          {
            bool find = false;
            foreach (BindingSourceStatus bindingSourceStatus in BindingSources)
              if (bindingSourceStatus.BindingSource == bindingSource)
              {
                bindingSourceStatus.ResetControlAuthorizationRules(Form, EditMode || NotUndoable, readOnly);
                find = true;
                break;
              }
            if (!find)
              ReadWriteAuthorization.ResetControlAuthorizationRules(Form, EditMode || NotUndoable, readOnly, bindingSource);
          }
    }

    /// <summary>
    /// 是否拒绝执行
    /// </summary>
    public bool DenyExecute(Component component)
    {
      return _executeAuthorization != null && _executeAuthorization.DenyExecute(component);
    }

    /// <summary>
    /// 重置组件执行规则
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
    public void ResetComponentAuthorizationRules()
    {
      if (_executeAuthorization != null)
        _executeAuthorization.ResetComponentAuthorizationRules();
      if (_treeListManager != null)
        _treeListManager.ResetAuthorizationRules(EditMode || NotUndoable);

      //Tools Bar
      IBusinessObject currentBusiness = CurrentBusiness;
      IBusinessCollection currentBusinessList = CurrentBusinessList;
      BarItem barItem = FindBarItem(BarItemId.Fetch);
      if (barItem != null)
        if (DenyExecute(barItem))
          barItem.Enabled = false;
        else
          barItem.Enabled = true;
      barItem = FindBarItem(BarItemId.Reset);
      if (barItem != null)
        if (DenyExecute(barItem))
          barItem.Enabled = false;
        else if (currentBusiness != null && (currentBusiness.EditMode || currentBusiness.NotUndoable))
          barItem.Enabled = currentBusiness.IsNew;
        else
          barItem.Enabled = CriteriaBindingSource != null;
      barItem = FindBarItem(BarItemId.Restore);
      if (barItem != null)
        if (DenyExecute(barItem))
          barItem.Enabled = false;
        else if (currentBusiness != null && currentBusiness.DeletedAsDisabled)
        {
          barItem.Visibility = BarItemVisibility.Always;
          barItem.Enabled = currentBusiness.IsDisabled;
        }
        else
        {
          barItem.Visibility = BarItemVisibility.Never;
          barItem.Enabled = false;
        }
      barItem = FindBarItem(BarItemId.Add);
      if (barItem != null)
        if (DenyExecute(barItem))
          barItem.Enabled = false;
        else
          barItem.Enabled = AllowAdd;
      barItem = FindBarItem(BarItemId.AddClone);
      if (barItem != null)
        if (DenyExecute(barItem))
          barItem.Enabled = false;
        else
          barItem.Enabled = AllowAdd && currentBusiness != null;
      barItem = FindBarItem(BarItemId.Modify);
      if (barItem != null)
        if (DenyExecute(barItem))
          barItem.Enabled = false;
        else
          barItem.Enabled = AllowModify;
      barItem = FindBarItem(BarItemId.Delete);
      if (barItem != null)
        if (DenyExecute(barItem))
          barItem.Enabled = false;
        else
          barItem.Enabled = AllowDelete;
      barItem = FindBarItem(BarItemId.Locate);
      if (barItem != null)
        if (DenyExecute(barItem))
          barItem.Enabled = false;
        else if (InMultistepSubmitMode || NeedLockPositionInOnestepSubmit)
        {
          barItem.Enabled = false;
          barItem.Visibility = BarItemVisibility.Never;
        }
        else
        {
          barItem.Enabled = currentBusinessList != null && currentBusinessList.EditMode;
          barItem.Visibility = BarItemVisibility.Always;
        }
      barItem = FindBarItem(BarItemId.Cancel);
      if (barItem != null)
        if (DenyExecute(barItem))
          barItem.Enabled = false;
        else
          barItem.Enabled = EditMode || NotUndoable;
      barItem = FindBarItem(BarItemId.Save);
      if (barItem != null)
        if (DenyExecute(barItem))
          barItem.Enabled = false;
        else
          barItem.Enabled = EditMode || NotUndoable;
      barItem = FindBarItem(BarItemId.Import);
      if (barItem != null)
        if (DenyExecute(barItem))
          barItem.Enabled = false;
        else
          barItem.Enabled = currentBusinessList != null && !currentBusinessList.EditMode || currentBusiness != null && !currentBusiness.EditMode;
      barItem = FindBarItem(BarItemId.Export);
      if (barItem != null)
        if (DenyExecute(barItem))
          barItem.Enabled = false;
        else
          barItem.Enabled = currentBusinessList != null && !currentBusinessList.EditMode || currentBusiness != null && !currentBusiness.EditMode;
      barItem = FindBarItem(BarItemId.Print);
      if (barItem != null)
        if (DenyExecute(barItem))
          barItem.Enabled = false;
        else
          barItem.Enabled = currentBusinessList != null;

      OnRulesApplied();
    }

    #region Designer

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Reliability", "CA2000:DisposeObjectsBeforeLosingScope")]
    internal bool ResetBindingSources(IDesignerHost designerHost)
    {
      //重置
      bool result = false;
      using (new DevExpress.Utils.WaitDialogForm(Phenix.Windows.Properties.Resources.ResetBindingSources, Phenix.Core.Properties.Resources.PleaseWait)) foreach (IComponent item in designerHost.Container.Components)
      {
        BindingSource bindingSource = item as BindingSource;
        if (bindingSource == null)
          continue;
        bool? dataSourceIsEnumerable = BindingSourceHelper.DataSourceIsEnumerable(bindingSource);
        if (!dataSourceIsEnumerable.HasValue || !dataSourceIsEnumerable.Value)
          continue;
        bool find = false;
        for (int i = 0; i < BindingSources.Count; i++)
          if (BindingSources[i].BindingSource == bindingSource)
          {
            find = true;
            if (BindingSources[i].BindingSource == this.BindingSource)
            {
              BindingSources[i].CriteriaBindingSource = this.CriteriaBindingSource;
              BindingSources[i].CriteriaCombineControl = this.CriteriaCombineControl;
            }
            break;
          }
        if (!find)
        {
          Type dataSourceType = BindingSourceHelper.GetDataSourceType(bindingSource);
          if (dataSourceType != null && typeof(IBusiness).IsAssignableFrom(dataSourceType))
          {
            BindingSourceStatus bindingSourceStatus;
            if (this.BindingSource == bindingSource)
              bindingSourceStatus = new BindingSourceStatus()
              {
                BindingSource = this.BindingSource, 
                CriteriaBindingSource = this.CriteriaBindingSource,
                CriteriaCombineControl = this.CriteriaCombineControl
              };
            else
              bindingSourceStatus = new BindingSourceStatus() { BindingSource = bindingSource };
            string s = (!String.IsNullOrEmpty(bindingSource.DataMember) ? bindingSource.DataMember : dataSourceType.Name) + typeof(BindingSourceStatus).Name;
            ComponentHelper.AddComponent(designerHost.Container, bindingSourceStatus, s);
            BindingSources.Add(bindingSourceStatus);
            result = true;
          }
        }
      }
      return result;
    }

    internal void ResetEditFriendlyCaptions(IDesignerHost designerHost)
    {
      ResetBindingSources(designerHost);
      EditValidation.ResetEditFriendlyCaptions(_friendlyCaptionSources);
    }

    internal void ResetGridFriendlyCaptions(ContainerControl rootComponent)
    {
      EditValidation.ResetGridFriendlyCaptions(rootComponent);
      foreach (EnumBindingSourceStatus item in EnumBindingSources)
        if (item.BindingSource != null && item.BindingSourceItemType != null)
        {
          EnumKeyCaptionCollection enumKeyCaptionCollection = EnumKeyCaptionCollection.Fetch(item.BindingSourceItemType);
          EditValidation.ResetGridFriendlyCaptions(rootComponent, item.BindingSource, enumKeyCaptionCollection.Caption);
        }
    }

    internal string ResetFormRules(IDesignerHost designerHost)
    {
      ResetBindingSources(designerHost);
      return 
        ResetRules(designerHost, designerHost.RootComponent as ContainerControl) +
        EditValidation.ResetRules(designerHost.RootComponent as ContainerControl);
    }

    private string ResetRules(IDesignerHost designerHost, Control container)
    {
      //for Developer Express .NET
      string result = ResetRules(designerHost, container as GridControl);
      if (!String.IsNullOrEmpty(result))
        goto Label;
      result = ResetRules(designerHost, container as VGridControl);
      if (!String.IsNullOrEmpty(result))
        goto Label;
      result = ResetRules(designerHost, container as GridLookUpEdit);
      if (!String.IsNullOrEmpty(result))
        goto Label;
      result = ResetRules(designerHost, container as LookUpEdit);
      if (!String.IsNullOrEmpty(result))
        goto Label;
      result = ResetRules(designerHost, container as CheckedComboBoxEdit);
      if (!String.IsNullOrEmpty(result))
        goto Label;

      foreach (Control item in container.Controls)
        result += ResetRules(designerHost, item);

      Label:
      if (!String.IsNullOrEmpty(result))
        result = container.Name + Environment.NewLine + result + Environment.NewLine;
      return result;
    }

    //for Developer Express .NET
    private string ResetRules(IDesignerHost designerHost, GridControl grid)
    {
      string result = String.Empty;
      if (grid == null)
        return result;
      foreach (BaseView view in grid.ViewCollection)
      {
        ColumnView columnView = view as ColumnView;
        if (columnView != null)
        {
          Type listItemType = Utilities.FindListItemType(GridControlHelper.FindDataSourceType(columnView));
          if (listItemType == null)
            continue;

          //GridView gridView = columnView as GridView;
          //if (gridView != null)
          //  gridView.OptionsView.ColumnAutoWidth = gridView.Columns.Count >= 10; //10列以上自动调整列宽
          //result += String.Format("change {0}{1}", columnView.Name, Environment.NewLine);

          foreach (GridColumn column in columnView.Columns)
          {
            FieldMapInfo fieldMapInfo = ClassMemberHelper.GetFieldMapInfo(listItemType, column.FieldName, true);
            if (fieldMapInfo != null)
            {
              if (String.CompareOrdinal(fieldMapInfo.PropertyName, "Selected") == 0)
              {
                column.Fixed = DevExpress.XtraGrid.Columns.FixedStyle.Left;
                column.Width = 50;
                result += String.Format("change {0}{1}", column.Name, Environment.NewLine);
              }
              result += ApplyRepositoryItemGridLookUpEdit(designerHost, column.ColumnEdit as RepositoryItemGridLookUpEdit, fieldMapInfo);
              result += ApplyRepositoryItemLookUpEdit(designerHost, column.ColumnEdit as RepositoryItemLookUpEdit, fieldMapInfo);
              result += ApplyRepositoryItemCheckedComboBoxEdit(designerHost, column.ColumnEdit as RepositoryItemCheckedComboBoxEdit, fieldMapInfo);
            }
          }
        }
      }
      result += CheckDetailBindingSource(designerHost, grid);
      return result;
    }

    //for Developer Express .NET
    private string ResetRules(IDesignerHost designerHost, VGridControl grid)
    {
      string result = String.Empty;
      if (grid == null)
        return result;
      Type listItemType = Utilities.FindListItemType(BindingSourceHelper.GetDataSourceType(grid.DataSource as BindingSource));
      if (listItemType == null)
        return result;
      foreach (BaseRow item in grid.Rows)
      {
        FieldMapInfo fieldMapInfo = ClassMemberHelper.GetFieldMapInfo(listItemType, item.Properties.FieldName, true);
        if (fieldMapInfo != null)
        {
          if (String.CompareOrdinal(fieldMapInfo.PropertyName, "Selected") == 0)
          {
            item.Fixed = DevExpress.XtraVerticalGrid.Rows.FixedStyle.Top;
            result += String.Format("change {0}{1}", item.Name, Environment.NewLine);
          }
          result += ApplyRepositoryItemLookUpEdit(designerHost, item.Properties.RowEdit as RepositoryItemLookUpEdit, fieldMapInfo);
          result += ApplyRepositoryItemCheckedComboBoxEdit(designerHost, item.Properties.RowEdit as RepositoryItemCheckedComboBoxEdit, fieldMapInfo);
        }
      }
      return result;
    }

    private string ResetRules(IDesignerHost designerHost, GridLookUpEdit lookUpEdit)
    {
      string result = String.Empty;
      if (lookUpEdit == null)
        return result;
      foreach (Binding item in lookUpEdit.DataBindings)
      {
        Type coreType = BindingSourceHelper.GetDataSourceCoreType(item.DataSource as BindingSource);
        if (coreType != null)
        {
          FieldMapInfo fieldMapInfo = ClassMemberHelper.GetFieldMapInfo(coreType, item.BindingMemberInfo.BindingField, true);
          if (fieldMapInfo != null)
            result += ApplyRepositoryItemGridLookUpEdit(designerHost, lookUpEdit.Properties, fieldMapInfo);
        }
      }
      return result;
    }

    private string ResetRules(IDesignerHost designerHost, LookUpEdit lookUpEdit)
    {
      string result = String.Empty;
      if (lookUpEdit == null)
        return result;
      foreach (Binding item in lookUpEdit.DataBindings)
      {
        Type coreType = BindingSourceHelper.GetDataSourceCoreType(item.DataSource as BindingSource);
        if (coreType != null)
        {
          FieldMapInfo fieldMapInfo = ClassMemberHelper.GetFieldMapInfo(coreType, item.BindingMemberInfo.BindingField, true);
          if (fieldMapInfo != null)
            result += ApplyRepositoryItemLookUpEdit(designerHost, lookUpEdit.Properties, fieldMapInfo);
        }
      }
      return result;
    }

    private string ResetRules(IDesignerHost designerHost, CheckedComboBoxEdit checkedComboBoxEdit)
    {
      string result = String.Empty;
      if (checkedComboBoxEdit == null)
        return result;
      foreach (Binding item in checkedComboBoxEdit.DataBindings)
      {
        Type coreType = BindingSourceHelper.GetDataSourceCoreType(item.DataSource as BindingSource);
        if (coreType != null)
        {
          FieldMapInfo fieldMapInfo = ClassMemberHelper.GetFieldMapInfo(coreType, item.BindingMemberInfo.BindingField, true);
          if (fieldMapInfo != null)
            result += ApplyRepositoryItemCheckedComboBoxEdit(designerHost, checkedComboBoxEdit.Properties, fieldMapInfo);
        }
      }
      return result;
    }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Reliability", "CA2000:DisposeObjectsBeforeLosingScope")]
    private string CheckEnumBindingSource(IDesignerHost designerHost, IFieldMapInfo fieldMapInfo, ref BindingSource bindingSource)
    {
      string result = String.Empty;
      foreach (EnumBindingSourceStatus item in EnumBindingSources)
        if (item.BindingSourceItemType == fieldMapInfo.FieldCoreUnderlyingType)
        {
          bindingSource = item.BindingSource;
          break;
        }
      if (bindingSource == null)
      {
        bindingSource = new BindingSource(typeof(EnumKeyCaptionCollection), null);
        string s = fieldMapInfo.FieldCoreUnderlyingType.Name + typeof(BindingSource).Name;
        s = ComponentHelper.AddComponent(designerHost.Container, bindingSource, s);
        result += String.Format("new {0}{1}", s, Environment.NewLine);
        EnumBindingSourceStatus enumBindingSourceStatus = new EnumBindingSourceStatus()
          {
            BindingSource = bindingSource,
            BindingSourceItemType = fieldMapInfo.FieldCoreUnderlyingType
          };
        s = fieldMapInfo.FieldCoreUnderlyingType.Name + typeof(EnumBindingSourceStatus).Name;
        s = ComponentHelper.AddComponent(designerHost.Container, enumBindingSourceStatus, s);
        EnumBindingSources.Add(enumBindingSourceStatus);
        result += String.Format("new {0}{1}", s, Environment.NewLine);
      }
      return result;
    }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Reliability", "CA2000:DisposeObjectsBeforeLosingScope")]
    private string CheckPromptCodeBindingSource(IDesignerHost designerHost, IFieldMapInfo fieldMapInfo, ref BindingSource bindingSource)
    {
      string result = String.Empty;
      foreach (PromptCodeBindingSourceStatus item in PromptCodeBindingSources)
        if (String.CompareOrdinal(item.PromptCodeName, fieldMapInfo.PromptCodeName) == 0)
        {
          bindingSource = item.BindingSource;
          break;
        }
      if (bindingSource == null)
      {
        bindingSource = new BindingSource(typeof(PromptCodeKeyCaptionCollection), null);
        string s = fieldMapInfo.PromptCodeName + typeof(BindingSource).Name;
        s = ComponentHelper.AddComponent(designerHost.Container, bindingSource, s);
        result += String.Format("new {0}{1}", s, Environment.NewLine);
        PromptCodeBindingSourceStatus promptCodeBindingSourceStatus = new PromptCodeBindingSourceStatus()
          {
            BindingSource = bindingSource,
            PromptCodeName = fieldMapInfo.PromptCodeName
          };
        s = fieldMapInfo.PromptCodeName + typeof(PromptCodeBindingSourceStatus).Name;
        s = ComponentHelper.AddComponent(designerHost.Container, promptCodeBindingSourceStatus, s);
        PromptCodeBindingSources.Add(promptCodeBindingSourceStatus);
        result += String.Format("new {0}{1}", s, Environment.NewLine);
      }
      return result;
    }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Reliability", "CA2000:DisposeObjectsBeforeLosingScope")]
    private string CheckLinkBindingSource(IDesignerHost designerHost, IFieldMapInfo fieldMapInfo, ref FieldMapInfo linkPrimaryKeyInfo, ref BindingSourceStatus bindingSourceStatus)
    {
      string result = String.Empty;
      foreach (BindingSourceStatus item in BindingSources)
        if (item.BindingSource != null && !item.Editabled)
        {
          Type type = BindingSourceHelper.GetDataSourceType(item.BindingSource);
          linkPrimaryKeyInfo = ClassMemberHelper.GetMasterFieldMapInfo(fieldMapInfo, Utilities.FindListItemType(type));
          if (linkPrimaryKeyInfo != null)
          {
            bindingSourceStatus = item;
            return result;
          }
        }

      List<string> assemblyNames = new List<string>();
      foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies())
        try
        {
          string assemblyName = assembly.GetName().Name;
          if (AppUtilities.IsNotApplicationAssembly(assembly) || assemblyNames.Contains(assemblyName))
            continue;
          assemblyNames.Add(assemblyName);

          Type linkType = null;
          foreach (Type type in assembly.GetExportedTypes())
            try
            {
              FieldMapInfo masterFieldMapInfo = ClassMemberHelper.GetMasterFieldMapInfo(fieldMapInfo, Utilities.FindListItemType(type));
              if (masterFieldMapInfo != null)
              {
                linkType = type;
                linkPrimaryKeyInfo = masterFieldMapInfo;
                if (masterFieldMapInfo.ClassMapInfo != null && masterFieldMapInfo.ClassMapInfo.IsReadOnly)
                  break;
              }
            }
            catch (TypeLoadException)
            {
              // ignored
            }
          if (linkType != null)
          {
            BindingSource bindingSource = new BindingSource(linkType, null);
            string s = linkType.Name + typeof(BindingSource).Name;
            s = ComponentHelper.AddComponent(designerHost.Container, bindingSource, s);
            result += String.Format("new {0}{1}", s, Environment.NewLine);
            bindingSourceStatus = new BindingSourceStatus()
            {
              BindingSource = bindingSource,
              Editabled = false
            };
            s = linkType.Name + typeof(BindingSourceStatus).Name;
            s = ComponentHelper.AddComponent(designerHost.Container, bindingSourceStatus, s);
            BindingSources.Add(bindingSourceStatus);
            result += String.Format("new {0}{1}", s, Environment.NewLine);
            break;
          }
        }
        catch (NotSupportedException)
        {
          // ignored
        }
        catch (ArgumentException)
        {
          // ignored
        }
        catch (Exception ex)
        {
          if (MessageBox.Show(AppUtilities.GetErrorMessage(ex),
            MethodBase.GetCurrentMethod().Name.Substring(4), MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.Cancel)
            break;
        }
      return result;
    }

    //for Developer Express .NET
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Reliability", "CA2000:DisposeObjectsBeforeLosingScope")]
    private string CheckDetailBindingSource(IDesignerHost designerHost, GridLevelNodeCollection nodes, BindingSource masterBindingSource)
    {
      string result = String.Empty;
      if (nodes == null)
        return result;
      foreach (GridLevelNode node in nodes)
      {
        bool find = false;
        foreach (BindingSourceStatus item in BindingSources)
          if (item.BindingSource != null && 
            item.BindingSource.DataSource == masterBindingSource && 
            String.CompareOrdinal(item.BindingSource.DataMember, node.RelationName) == 0)
          {
            find = true;
            break;
          }
        if (!find)
        {
          BindingSource bindingSource = new BindingSource(masterBindingSource, node.RelationName);
          string s = node.RelationName + typeof(BindingSource).Name;
          s = ComponentHelper.AddComponent(designerHost.Container, bindingSource, s);
          result += String.Format("new {0}{1}", s, Environment.NewLine);
          BindingSourceStatus bindingSourceStatus = new BindingSourceStatus()
            {
              BindingSource = bindingSource
            };
          s = node.RelationName + typeof(BindingSourceStatus).Name;
          s = ComponentHelper.AddComponent(designerHost.Container, bindingSourceStatus, s);
          BindingSources.Add(bindingSourceStatus);
          result += String.Format("new {0}{1}", s, Environment.NewLine);
          result += CheckDetailBindingSource(designerHost, node.Nodes, bindingSource);
        }
      }
      return result;
    }

    //for Developer Express .NET
    private string CheckDetailBindingSource(IDesignerHost designerHost, GridControl gridControl)
    {
      string result = String.Empty;
      if (gridControl.LevelTree != null)
        result += CheckDetailBindingSource(designerHost, gridControl.LevelTree.Nodes, gridControl.DataSource as BindingSource);
      return result;
    }

    //for Developer Express .NET
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Reliability", "CA2000:DisposeObjectsBeforeLosingScope")]
    private string ApplyRepositoryItemGridLookUpEdit(IDesignerHost designerHost,
      RepositoryItemGridLookUpEdit repositoryItemLookUpEdit, IFieldMapInfo fieldMapInfo)
    {
      string result = String.Empty;
      if (repositoryItemLookUpEdit == null)
        return result;
      if (repositoryItemLookUpEdit.DataSource != null)
        return result;
      if (fieldMapInfo.FieldUnderlyingType.IsEnum)
      {
        //处理枚举关系
        BindingSource bindingSource = null;
        result += CheckEnumBindingSource(designerHost, fieldMapInfo, ref bindingSource);
        repositoryItemLookUpEdit.View.Columns.Clear();
        repositoryItemLookUpEdit.View.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[]
          {
            new DevExpress.XtraGrid.Columns.GridColumn() {FieldName = "Key"},
            new DevExpress.XtraGrid.Columns.GridColumn() {FieldName = "Caption"}
          });
        repositoryItemLookUpEdit.DataSource = bindingSource;
        repositoryItemLookUpEdit.DisplayMember = "Caption";
        repositoryItemLookUpEdit.ValueMember = "Value";
        result += String.Format("change {0}{1}", repositoryItemLookUpEdit.Name, Environment.NewLine);
      }
      else if (fieldMapInfo.IsPromptCodeValue)
      {
        //处理提示码关系
        BindingSource bindingSource = null;
        result += CheckPromptCodeBindingSource(designerHost, fieldMapInfo, ref bindingSource);
        repositoryItemLookUpEdit.View.Columns.Clear();
        repositoryItemLookUpEdit.View.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[]
          {
            new DevExpress.XtraGrid.Columns.GridColumn() {FieldName = "Caption"}
          });
        repositoryItemLookUpEdit.DataSource = bindingSource;
        repositoryItemLookUpEdit.DisplayMember = "Caption";
        repositoryItemLookUpEdit.ValueMember = "Value";
        repositoryItemLookUpEdit.Buttons.Clear();
        repositoryItemLookUpEdit.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
          new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo),
          new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Plus),
          new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Delete)});
        result += String.Format("change {0}{1}", repositoryItemLookUpEdit.Name, Environment.NewLine);
      }
      else if (!String.IsNullOrEmpty(fieldMapInfo.LinkFullTableColumnName))
      {
        //处理link关系
        FieldMapInfo linkPrimaryKeyInfo = null;
        BindingSourceStatus bindingSourceStatus = null;
        result += CheckLinkBindingSource(designerHost, fieldMapInfo, ref linkPrimaryKeyInfo, ref bindingSourceStatus);
        if (linkPrimaryKeyInfo != null)
        {
          List<DevExpress.XtraGrid.Columns.GridColumn> infos = new List<DevExpress.XtraGrid.Columns.GridColumn>();
          foreach (FieldMapInfo item in ClassMemberHelper.GetFieldMapInfos(bindingSourceStatus.BindingSourceCoreType))
            if (item.InLookUpColumnSelect)
            {
              infos.Add(new DevExpress.XtraGrid.Columns.GridColumn() { FieldName = item.PropertyName });
              break;
            }
          foreach (FieldMapInfo item in ClassMemberHelper.GetFieldMapInfos(bindingSourceStatus.BindingSourceCoreType))
            if (item.InLookUpColumnDisplay && !item.InLookUpColumnSelect)
            {
              infos.Add(new DevExpress.XtraGrid.Columns.GridColumn() { FieldName = item.PropertyName });
              repositoryItemLookUpEdit.DisplayMember = item.PropertyName;
              break;
            }
          foreach (FieldMapInfo item in ClassMemberHelper.GetFieldMapInfos(bindingSourceStatus.BindingSourceCoreType))
            if (item.InLookUpColumn && !item.InLookUpColumnDisplay && !item.InLookUpColumnSelect)
              infos.Add(new DevExpress.XtraGrid.Columns.GridColumn() { FieldName = item.PropertyName });
          if (infos.Count > 0)
          {
            repositoryItemLookUpEdit.View.Columns.Clear();
            repositoryItemLookUpEdit.View.Columns.AddRange(infos.ToArray());
            foreach (DevExpress.XtraGrid.Columns.GridColumn item in repositoryItemLookUpEdit.View.Columns)
              item.Visible = true;
          }
          repositoryItemLookUpEdit.DataSource = bindingSourceStatus.BindingSource;
          repositoryItemLookUpEdit.ValueMember = linkPrimaryKeyInfo.PropertyName;
          result += String.Format("change {0}{1}", repositoryItemLookUpEdit.Name, Environment.NewLine);
        }
      }
      return result;
    }

    //for Developer Express .NET
    private string ApplyRepositoryItemLookUpEdit(IDesignerHost designerHost, 
      RepositoryItemLookUpEdit repositoryItemLookUpEdit, IFieldMapInfo fieldMapInfo)
    {
      string result = String.Empty;
      if (repositoryItemLookUpEdit == null)
        return result;
      if (repositoryItemLookUpEdit.DataSource != null)
        return result;
      if (fieldMapInfo.FieldUnderlyingType.IsEnum)
      {
        //处理枚举关系
        BindingSource bindingSource = null;
        result += CheckEnumBindingSource(designerHost, fieldMapInfo, ref bindingSource);
        repositoryItemLookUpEdit.Columns.Clear();
        repositoryItemLookUpEdit.Columns.AddRange(new DevExpress.XtraEditors.Controls.LookUpColumnInfo[]
          {
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("Key"),
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("Caption")
          });
        repositoryItemLookUpEdit.DataSource = bindingSource;
        repositoryItemLookUpEdit.DisplayMember = "Caption";
        repositoryItemLookUpEdit.ValueMember = "Value";
        result += String.Format("change {0}{1}", repositoryItemLookUpEdit.Name, Environment.NewLine);
      }
      else if (fieldMapInfo.IsPromptCodeValue)
      {
        //处理提示码关系
        BindingSource bindingSource = null;
        result += CheckPromptCodeBindingSource(designerHost, fieldMapInfo, ref bindingSource);
        repositoryItemLookUpEdit.Columns.Clear();
        repositoryItemLookUpEdit.Columns.AddRange(new DevExpress.XtraEditors.Controls.LookUpColumnInfo[]
          {
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("Caption")
          });
        repositoryItemLookUpEdit.DataSource = bindingSource;
        repositoryItemLookUpEdit.DisplayMember = "Caption";
        repositoryItemLookUpEdit.ValueMember = "Value";
        repositoryItemLookUpEdit.Buttons.Clear();
        repositoryItemLookUpEdit.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[]
          {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo),
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Plus),
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Delete)
          });
        result += String.Format("change {0}{1}", repositoryItemLookUpEdit.Name, Environment.NewLine);
      }
      else if (!String.IsNullOrEmpty(fieldMapInfo.LinkFullTableColumnName))
      {
        //处理link关系
        FieldMapInfo linkPrimaryKeyInfo = null;
        BindingSourceStatus bindingSourceStatus = null;
        result += CheckLinkBindingSource(designerHost, fieldMapInfo, ref linkPrimaryKeyInfo, ref bindingSourceStatus);
        if (linkPrimaryKeyInfo != null)
        {
          List<DevExpress.XtraEditors.Controls.LookUpColumnInfo> infos = new List<DevExpress.XtraEditors.Controls.LookUpColumnInfo>();
          foreach (FieldMapInfo item in ClassMemberHelper.GetFieldMapInfos(bindingSourceStatus.BindingSourceCoreType))
            if (item.InLookUpColumnSelect)
            {
              infos.Add(new DevExpress.XtraEditors.Controls.LookUpColumnInfo(item.PropertyName));
              break;
            }
          foreach (FieldMapInfo item in ClassMemberHelper.GetFieldMapInfos(bindingSourceStatus.BindingSourceCoreType))
            if (item.InLookUpColumnDisplay && !item.InLookUpColumnSelect)
            {
              infos.Add(new DevExpress.XtraEditors.Controls.LookUpColumnInfo(item.PropertyName));
              repositoryItemLookUpEdit.DisplayMember = item.PropertyName;
              break;
            }
          foreach (FieldMapInfo item in ClassMemberHelper.GetFieldMapInfos(bindingSourceStatus.BindingSourceCoreType))
            if (item.InLookUpColumn && !item.InLookUpColumnDisplay && !item.InLookUpColumnSelect)
              infos.Add(new DevExpress.XtraEditors.Controls.LookUpColumnInfo(item.PropertyName));
          if (infos.Count > 0)
          {
            repositoryItemLookUpEdit.Columns.Clear();
            repositoryItemLookUpEdit.Columns.AddRange(infos.ToArray());
          }
          repositoryItemLookUpEdit.DataSource = bindingSourceStatus.BindingSource;
          repositoryItemLookUpEdit.ValueMember = linkPrimaryKeyInfo.PropertyName;
          result += String.Format("change {0}{1}", repositoryItemLookUpEdit.Name, Environment.NewLine);
        }
      }
      return result;
    }

    //for Developer Express .NET
    private string ApplyRepositoryItemCheckedComboBoxEdit(IDesignerHost designerHost,
      RepositoryItemCheckedComboBoxEdit repositoryItemCheckedComboBoxEdit, IFieldMapInfo fieldMapInfo)
    {
      string result = String.Empty;
      if (repositoryItemCheckedComboBoxEdit == null)
        return result;
      if (repositoryItemCheckedComboBoxEdit.DataSource != null)
        return result;
      Type fieldType = fieldMapInfo.FieldUnderlyingType;
      if (!fieldType.IsArray)
        return result;
      fieldType = Utilities.GetUnderlyingType(fieldType.GetElementType());
      if (fieldType.IsEnum)
      {
        //处理枚举关系
        BindingSource bindingSource = null;
        result += CheckEnumBindingSource(designerHost, fieldMapInfo, ref bindingSource);
        repositoryItemCheckedComboBoxEdit.DataSource = bindingSource;
        repositoryItemCheckedComboBoxEdit.DisplayMember = "Caption";
        repositoryItemCheckedComboBoxEdit.ValueMember = "Flag";
        repositoryItemCheckedComboBoxEdit.AllowMultiSelect = true;
        repositoryItemCheckedComboBoxEdit.AllowNullInput = DevExpress.Utils.DefaultBoolean.True;
        result += String.Format("change {0}{1}", repositoryItemCheckedComboBoxEdit.Name, Environment.NewLine);
      }
      else if (fieldMapInfo.IsPromptCodeValue)
      {
        //处理提示码关系
        BindingSource bindingSource = null;
        result += CheckPromptCodeBindingSource(designerHost, fieldMapInfo, ref bindingSource);
        repositoryItemCheckedComboBoxEdit.DataSource = bindingSource;
        repositoryItemCheckedComboBoxEdit.DisplayMember = "Caption";
        repositoryItemCheckedComboBoxEdit.ValueMember = "Value";
        repositoryItemCheckedComboBoxEdit.AllowMultiSelect = true;
        repositoryItemCheckedComboBoxEdit.AllowNullInput = DevExpress.Utils.DefaultBoolean.True;
        repositoryItemCheckedComboBoxEdit.Buttons.Clear();
        repositoryItemCheckedComboBoxEdit.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[]
          {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo),
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Plus),
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Delete)
          });
        result += String.Format("change {0}{1}", repositoryItemCheckedComboBoxEdit.Name, Environment.NewLine);
      }
      else if (!String.IsNullOrEmpty(fieldMapInfo.LinkFullTableColumnName))
      {
        //处理link关系
        FieldMapInfo linkPrimaryKeyInfo = null;
        BindingSourceStatus bindingSourceStatus = null;
        result += CheckLinkBindingSource(designerHost, fieldMapInfo, ref linkPrimaryKeyInfo, ref bindingSourceStatus);
        if (linkPrimaryKeyInfo != null)
        {
          foreach (FieldMapInfo item in ClassMemberHelper.GetFieldMapInfos(bindingSourceStatus.BindingSourceCoreType))
            if (item.InLookUpColumnDisplay && !item.InLookUpColumnSelect)
            {
              repositoryItemCheckedComboBoxEdit.DisplayMember = item.PropertyName;
              break;
            }
          repositoryItemCheckedComboBoxEdit.DataSource = bindingSourceStatus.BindingSource;
          repositoryItemCheckedComboBoxEdit.ValueMember = linkPrimaryKeyInfo.PropertyName;
          repositoryItemCheckedComboBoxEdit.AllowMultiSelect = true;
          repositoryItemCheckedComboBoxEdit.AllowNullInput = DevExpress.Utils.DefaultBoolean.True;
          result += String.Format("change {0}{1}", repositoryItemCheckedComboBoxEdit.Name, Environment.NewLine);
        }
      }
      return result;
    }

    #endregion

    /// <summary>
    /// 重置状态栏
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Globalization", "CA1303:DoNotPassLiteralsAsLocalizedParameters")]
    public void ResetRecordState()
    {
      BarItem item = FindBarItem(BarItemId.DataRecordState);
      if (item == null)
        return;
      if (BindingSource == null)
      {
        item.Caption = "";
        return;
      }
      IBusinessObject currentBusiness = CurrentBusiness;
      IBusinessCollection currentBusinessList = CurrentBusinessList;
      string s = String.Format("{0} / {1}", BindingSource.Position + 1, BindingSource.Count);
      if (currentBusinessList != null)
      {
        if (!String.IsNullOrEmpty(currentBusinessList.Caption))
          s = String.Format("{0}: {1}", currentBusinessList.Caption, s);
        IBusinessCollectionPage page = currentBusinessList as IBusinessCollectionPage;
        if (page != null)
          s = String.Format("{0}: {1} / {2}({3})", s, page.LocalPagesCount, page.MaxPageNo, page.MaxCount);
      }
      else if (currentBusiness != null)
      {
        if (!String.IsNullOrEmpty(currentBusiness.Caption))
          s = String.Format("{0}: {1}", currentBusiness.Caption, s);
      }
      if (AppConfig.Debugging)
      {
        IBusiness currentBusinessRoot = CurrentBusinessRoot;
        if (currentBusiness != null)
          s = String.Format("R({0})D({1}[{2}]) - {3}_{4}",
            currentBusinessRoot != null ? currentBusinessRoot.GetType().Name : Phenix.Core.Code.Converter.NullSymbolic,
            currentBusinessList != null ? currentBusinessList.GetType().Name : currentBusiness.GetType().Name,
            s, currentBusiness.EditLevel, currentBusiness.PrimaryKey);
        else
          s = String.Format("R({0})D({1}[{2}]) - {3}",
            currentBusinessRoot != null ? currentBusinessRoot.GetType().Name : Phenix.Core.Code.Converter.NullSymbolic,
            currentBusinessList != null ? currentBusinessList.GetType().Name : Phenix.Core.Code.Converter.NullSymbolic,
            s,
            Phenix.Core.Code.Converter.NullSymbolic);
      }
      item.Caption = s;
    }

    private void ShowExecuteActionInfo()
    {
      IBusinessObject currentBusiness = CurrentBusiness;
      if (currentBusiness != null)
      {
        IList<ExecuteActionInfo> infos = currentBusiness.FetchExecuteAction();
        if (infos != null)
          for (int i = infos.Count - 1; i >= 0; i--)
          {
            ExecuteActionInfo info = infos[i];
            if (info != null && !info.FieldMapInfo.FieldAttribute.IsPrimaryKey)
            {
              ShowHint(info.ToString());
              return;
            }
          }
      }
      ShowHint(null);
    }

    /// <summary>
    /// 显示提示信息（线程安全）
    /// </summary>
    /// <param name="text">文本</param>
    public void InvokeShowHint(string text)
    {
      if (this.Form.InvokeRequired)
        this.Form.BeginInvoke(new Action<string>(ShowHint), new object[] {text});
      else
        ShowHint(text);
    }

    /// <summary>
    /// 显示提示信息
    /// </summary>
    /// <param name="text">文本</param>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Globalization", "CA1303:DoNotPassLiteralsAsLocalizedParameters")]
    public void ShowHint(string text)
    {
      BarItem item = FindBarItem(BarItemId.Hint);
      if (item != null)
      {
        text = String.IsNullOrEmpty(text) ? String.Empty : ": " + text.Replace('\n', ' ').Replace('\r', ' ').Trim();
        switch (OperateState)
        {
          case DataOperateState.FetchSuspend:
            item.Caption = Phenix.Windows.Properties.Resources.DataFetchSuspend + text;
            break;
          case DataOperateState.Browse:
            item.Caption = Phenix.Windows.Properties.Resources.DataBrowse + text;
            break;
          case DataOperateState.Add:
            if (!InMultistepSubmitMode && !NeedLockPositionInOnestepSubmit)
              item.Caption = Phenix.Windows.Properties.Resources.DataAddingOrAllowFetch + text;
            else
              item.Caption = Phenix.Windows.Properties.Resources.DataAdding + text;
            break;
          case DataOperateState.Modify:
            item.Caption = Phenix.Windows.Properties.Resources.DataModifying + text;
            break;
          case DataOperateState.Delete:
            item.Caption = Phenix.Windows.Properties.Resources.DataDeleting + text;
            break;
          default:
            if (!String.IsNullOrEmpty(text))
              item.Caption = text;
            break;
        }
      }
      Application.DoEvents();
    }
    
    #region SetBindingSource

    #region 内嵌类

    private class SetBindingSourceData : IComparable
    {
      public SetBindingSourceData(BindingSource source, object[] args, bool isAsynchronous)
      {
        _source = source;
        _args = args;
        _isAsynchronous = isAsynchronous;
      }

      #region 属性

      private readonly BindingSource _source;
      public BindingSource Source
      {
        get { return _source; }
      }

      private readonly object[] _args;
      public object[] Args
      {
        get { return _args; }
      }

      private readonly bool _isAsynchronous;
      public bool IsAsynchronous
      {
        get { return _isAsynchronous; }
      }

      #endregion

      #region 方法

      public override int GetHashCode()
      {
        int result = _source != null ? _source.ToString().GetHashCode() : 0;
        foreach (object item in _args)
          if (item != null)
            result = result ^ item.GetHashCode();
        return result;
      }

      public override bool Equals(object obj)
      {
        if (object.ReferenceEquals(obj, this))
          return true;
        SetBindingSourceData other = obj as SetBindingSourceData;
        if (object.ReferenceEquals(other, null))
          return false;
        if (_source != other._source)
          return false;
        if (_args != null || other._args != null)
        {
          if (_args == null || other._args == null)
            return false;
          if (_args.Length != other._args.Length)
            return false;
          for (int i = 0; i < _args.Length - 1; i++)
             if (!object.Equals(_args[i], other._args[i]))
              return false;
        }
        return true;
      }

      #region IComparable 成员

      /// <summary>
      /// 比较对象
      /// </summary>
      /// <param name="obj">对象</param>
      public int CompareTo(object obj)
      {
        if (object.ReferenceEquals(obj, this))
          return 0;
        SetBindingSourceData other = obj as SetBindingSourceData;
        if (object.ReferenceEquals(other, null))
          return 1;
        return GetHashCode().CompareTo(other.GetHashCode());
      }

      #endregion

      #endregion
    }

    #endregion

    private void SetBindingSource(BindingSource source, object[] args, bool isAsynchronous)
    {
      SetBindingSourceData setBindingSourceData = new SetBindingSourceData(source, args, isAsynchronous);
      if (!ParallelFetch && !isAsynchronous)
      {
        ExecuteSetBindingSource(setBindingSourceData);
        return;
      }
      if (_setBindingSourceThreads == null)
        lock (_setBindingSourceThreadsLock)
          if (_setBindingSourceThreads == null)
          {
            _setBindingSourceThreads = new SortedList<SetBindingSourceData, Thread>();
          }
      if (!_setBindingSourceThreads.ContainsKey(setBindingSourceData))
        lock (_setBindingSourceThreadsLock)
          if (!_setBindingSourceThreads.ContainsKey(setBindingSourceData))
          {
            if (ParallelFetch && !setBindingSourceData.IsAsynchronous)
              Interlocked.Increment(ref _inFetchBindingSourceCount);
            Thread thread = new Thread(ExecuteSetBindingSource);
            thread.IsBackground = true;
            _setBindingSourceThreads[setBindingSourceData] = thread;
            thread.Start(setBindingSourceData);
          }
    }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
    private void ExecuteSetBindingSource(object data)
    {
      SetBindingSourceData setBindingSourceData = (SetBindingSourceData)data;
      try
      {
        Type dataSourceType = BindingSourceHelper.GetDataSourceType(setBindingSourceData.Source);
        if (dataSourceType == null)
          return;

        if (PromptCodeKeyCaptionCollection.Equals(dataSourceType))
        {
          PromptCodeKeyCaption business = BindingSourceHelper.GetDataSourceCurrent(setBindingSourceData.Source) as PromptCodeKeyCaption;
          string primaryKey = business != null ? business.Key : null;
          PromptCodeKeyCaptionCollection businessList;
          DateTime startDateTime = DateTime.Now;
          DevExpress.Utils.WaitDialogForm waitDialog = null;
          if (!setBindingSourceData.IsAsynchronous)
            waitDialog = new DevExpress.Utils.WaitDialogForm(String.Format(Phenix.Windows.Properties.Resources.DataFetching,
              AppConfig.Debugging ? setBindingSourceData.Args[0] : null),
              Phenix.Core.Properties.Resources.PleaseWait);
          try
          {
            businessList = PromptCodeKeyCaptionCollection.Fetch(setBindingSourceData.Args[0] as string);
          }
          finally
          {
            if (waitDialog != null)
            {
              waitDialog.Close();
              waitDialog = null;
            }
          }
          DateTime fetchDateTime = DateTime.Now;
          if (!setBindingSourceData.IsAsynchronous)
            waitDialog = new DevExpress.Utils.WaitDialogForm(String.Format(Phenix.Windows.Properties.Resources.BindingSourceFilling, null,
              AppConfig.Debugging ? setBindingSourceData.Args[0] : null),
              Phenix.Core.Properties.Resources.PleaseWait);
          try
          {
            if (!String.IsNullOrEmpty(primaryKey) && businessList.Count > 0 && businessList.Count <= LocatePositionMaximum)
              business = businessList.FindByKey(primaryKey);
            else
              business = null;
            string hint = String.Format(Phenix.Windows.Properties.Resources.DataFetchSucceed, businessList.Name, businessList.Count, fetchDateTime.Subtract(startDateTime).TotalSeconds);
            BindingSourceHelper.SetDataSource(this.Form, setBindingSourceData.Source, dataSourceType, null, LocatePositionMaximum, FindBarItem(BarItemId.Hint), Phenix.Core.Properties.Resources.PleaseWait);
            BindingSourceHelper.SetDataSource(this.Form, setBindingSourceData.Source, businessList, business, LocatePositionMaximum, FindBarItem(BarItemId.Hint), hint);
          }
          finally
          {
            if (waitDialog != null)
            {
              waitDialog.Close();
              waitDialog = null;
            }
          }
        }
        else
        {
          IBusinessObject business = BindingSourceHelper.GetDataSourceCurrent(setBindingSourceData.Source) as IBusinessObject;
          string primaryKey = business != null ? business.PrimaryKey : null;
          IBusinessCollection businessList;
          DateTime startDateTime = DateTime.Now;
          DevExpress.Utils.WaitDialogForm waitDialog = null;
          string hint = String.Format(Phenix.Windows.Properties.Resources.DataFetching,
            AppConfig.Debugging ? dataSourceType.Name : null);
          if (!setBindingSourceData.IsAsynchronous)
            waitDialog = new DevExpress.Utils.WaitDialogForm(hint, Phenix.Core.Properties.Resources.PleaseWait);
          else
            InvokeShowHint(hint);
          try
          {
            businessList = dataSourceType.InvokeMember("Fetch",
              BindingFlags.Static | BindingFlags.Public | BindingFlags.FlattenHierarchy | BindingFlags.InvokeMethod, null, null, setBindingSourceData.Args) as IBusinessCollection;
            if (businessList == null)
              return;
          }
          finally
          {
            if (waitDialog != null)
            {
              waitDialog.Close();
              waitDialog = null;
            }
          }
          DateTime fetchDateTime = DateTime.Now;
          hint = String.Format(Phenix.Windows.Properties.Resources.BindingSourceFilling, businessList.Caption,
            AppConfig.Debugging ? dataSourceType.Name : null);
          if (!setBindingSourceData.IsAsynchronous)
            waitDialog = new DevExpress.Utils.WaitDialogForm(hint, Phenix.Core.Properties.Resources.PleaseWait);
          else
            InvokeShowHint(hint);
          try
          {
            if (!String.IsNullOrEmpty(primaryKey) && businessList.Count > 0 && businessList.Count <= LocatePositionMaximum)
              business = businessList.FindItem(primaryKey);
            else
              business = null;
            hint = String.Format(Phenix.Windows.Properties.Resources.DataFetchSucceed, businessList.Caption, businessList.Count, fetchDateTime.Subtract(startDateTime).TotalSeconds);
            BindingSourceHelper.SetDataSource(this.Form, setBindingSourceData.Source, dataSourceType, null, LocatePositionMaximum, FindBarItem(BarItemId.Hint), Phenix.Core.Properties.Resources.PleaseWait);
            BindingSourceHelper.SetDataSource(this.Form, setBindingSourceData.Source, businessList, business, LocatePositionMaximum, FindBarItem(BarItemId.Hint), hint);
          }
          finally
          {
            if (waitDialog != null)
            {
              waitDialog.Close();
              waitDialog = null;
            }
          }
        }
      }
      catch (ObjectDisposedException)
      {
        return;
      }
      catch (ThreadAbortException)
      {
        Thread.ResetAbort();
        return;
      }
      catch (Exception ex)
      {
        string hint = String.Format(Phenix.Windows.Properties.Resources.DataFetchAborted, AppUtilities.GetErrorMessage(ex));
        InvokeShowHint(hint);
        if (!setBindingSourceData.IsAsynchronous)
          MessageBox.Show(hint, Phenix.Windows.Properties.Resources.DataFetch, MessageBoxButtons.OK, MessageBoxIcon.Error);
        if (ex is TargetInvocationException && ex.InnerException is ValidationException)
          CheckRules(CriteriaBindingSource, true);
        else
          EventLog.SaveLocal(MethodBase.GetCurrentMethod(), ex);
      }
      finally
      {
        if (_setBindingSourceThreads != null)
          lock (_setBindingSourceThreadsLock)
          {
            _setBindingSourceThreads.Remove(setBindingSourceData);
          }
        if (ParallelFetch && !setBindingSourceData.IsAsynchronous)
          Interlocked.Decrement(ref _inFetchBindingSourceCount);
      }
    }

    private void AbortExecuteSetBindingSource()
    {
      if (_setBindingSourceThreads != null)
        lock (_setBindingSourceThreadsLock)
          if (_setBindingSourceThreads.Count > 0)
          {
            foreach (KeyValuePair<SetBindingSourceData, Thread> kvp in _setBindingSourceThreads)
              kvp.Value.Abort();
            _setBindingSourceThreads.Clear();
            _inFetchBindingSourceCount = 0;
          }
    }

    #endregion

    #endregion
  }
}