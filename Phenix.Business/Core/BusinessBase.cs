using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Data;
using System.Data.Common;
using System.IO;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text;
using Csla;
using Phenix.Business.Rules;
using Phenix.Core;
using Phenix.Core.Cache;
using Phenix.Core.Data;
using Phenix.Core.Dictionary;
using Phenix.Core.Log;
using Phenix.Core.Mapping;
using Phenix.Core.Reflection;
using Phenix.Core.Security;
using Phenix.Core.SyncCollections;

namespace Phenix.Business.Core
{
  /// <summary>
  /// 业务基类
  /// </summary>
  [Serializable]
  [DataDictionary(AssemblyClassType.Business)]
  [ClassAttribute(null)]
  public abstract class BusinessBase<T> : Csla.BusinessBase<T>, Csla.Security.IAuthorizeReadWrite,
    IAuthorizationObject, IFactory, IDataInvalidInfo, IEntity
    where T : BusinessBase<T>
  {
    #region CreateInstance

    /// <summary>
    /// 构建实体
    /// </summary>
    protected virtual T CreateInstance()
    {
      return (T)Csla.Reflection.MethodCaller.CreateInstance(typeof(T));
    }
    object IFactory.CreateInstance()
    {
      return CreateInstance();
    }

    private static readonly IFactory _factory = (IFactory)FormatterServices.GetUninitializedObject(typeof(T));

    /// <summary>
    /// 构建实体
    /// </summary>
    protected static T DynamicCreateInstance()
    {
      return (T)_factory.CreateInstance();
    }

    #endregion

    #region 工厂

    /// <summary>
    /// 构建自己
    /// </summary>
    protected virtual bool FetchSelf(IDataRecord sourceFieldValues, IList<FieldMapInfo> fieldMapInfos)
    {
      lock (this)
      {
        if (EntityHelper.FillFieldValues(sourceFieldValues, this, fieldMapInfos))
        {
          MarkFetched();
          return true;
        }
        return false;
      }
    }
    [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
    bool IEntity.FetchSelf(IDataRecord sourceFieldValues, IList<FieldMapInfo> fieldMapInfos)
    {
      return FetchSelf(sourceFieldValues, fieldMapInfos);
    }

    #endregion

    #region 属性

    /// <summary>
    /// 数据源键
    /// 缺省为 T 上的 ClassAttribute.DataSourceKey
    /// </summary>
    [Newtonsoft.Json.JsonIgnore]
    [System.ComponentModel.Browsable(false)]
    [System.ComponentModel.DataAnnotations.Display(AutoGenerateField = false)]
    public virtual string DataSourceKey
    {
      get { return ClassMemberHelper.GetDataSourceKey(this.GetType()); }
    }

    /// <summary>
    /// 条件集
    /// </summary>
    protected ICriterions Criterions
    {
      get { return null; }
    }
    ICriterions IEntity.Criterions
    {
      get { return Criterions; }
    }

    /// <summary>
    /// 友好名
    /// 缺省为 T 上的 ClassAttribute.FriendlyName
    /// 用于提示信息等
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1000:DoNotDeclareStaticMembersOnGenericTypes")]
    public static string FriendlyName
    {
      get { return ClassMemberHelper.GetFriendlyName(typeof(T)); }
    }

    [NonSerialized]
    [Csla.NotUndoable]
    private string _caption;
    /// <summary>
    /// 标签
    /// 缺省为唯一键值 
    /// 用于提示信息等
    /// </summary>
    [Newtonsoft.Json.JsonIgnore]
    public virtual string Caption
    {
      get
      {
        if (_caption == null)
          _caption = String.Format("{0} {1}", AppConfig.Debugging ? FriendlyName + "[" + typeof(T).FullName + "]" : FriendlyName, FieldUniqueMapInfo.GetObjectCaption(this));
        return _caption;
      }
    }

    private static readonly SynchronizedDictionary<string, Type> _cacheTypes = new SynchronizedDictionary<string, Type>(StringComparer.Ordinal);
    internal static IDictionary<string, Type> CacheTypes
    {
      get
      {
        if (_cacheTypes.Count == 0)
          lock (_cacheTypes)
            if (_cacheTypes.Count == 0)
            {
              _cacheTypes[typeof(T).FullName] = typeof(T);
            }
        return _cacheTypes;
      }
    }

    [Csla.NotUndoable]
    private long? _idValue;
    /// <summary>
    /// 对象ID
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1721:PropertyNamesShouldNotMatchGetMethods")]
    [Newtonsoft.Json.JsonIgnore]
    [System.ComponentModel.Browsable(false)]
    [System.ComponentModel.DataAnnotations.Display(AutoGenerateField = false)]
    public long IdValue
    {
      get
      {
        if (!_idValue.HasValue)
          _idValue = Sequence.Value;
        return _idValue.Value;
      }
      internal set { _idValue = value; }
    }

    [NonSerialized]
    [Csla.NotUndoable]
    private string _primaryKey;
    /// <summary>
    /// 主键值
    /// </summary>
    [Newtonsoft.Json.JsonIgnore]
    [System.ComponentModel.Browsable(false)]
    [System.ComponentModel.DataAnnotations.Display(AutoGenerateField = false)]
    public virtual string PrimaryKey
    {
      get
      {
        if (_primaryKey == null)
        {
          StringBuilder result = new StringBuilder();
          foreach (FieldMapInfo item in ClassMemberHelper.GetFieldMapInfos(this.GetType(), true, false, true))
          {
            object value = item.GetValue(this);
            if (value != null)
              result.Append(value.ToString());
            result.Append(AppConfig.VALUE_SEPARATOR);
          }
          if (result.Length > 0)
            result.Remove(result.Length - 1, 1);
          _primaryKey = result.ToString();
        }
        return _primaryKey;
      }
    }

    [Csla.NotUndoable]
    private bool _selfFetched;
    /// <summary>
    /// 已经Fetch
    /// </summary>
    [Newtonsoft.Json.JsonIgnore]
    [System.ComponentModel.Browsable(false)]
    [System.ComponentModel.DataAnnotations.Display(AutoGenerateField = false)]
    public bool SelfFetched
    {
      get { return _selfFetched; }
    }

    [NonSerialized]
    [Csla.NotUndoable]
    private bool _propertyValueChanged;
    /// <summary>
    /// 属性值被赋值过(如果写入时的新值与旧值相同则认为未被赋值过)
    /// </summary>
    [Newtonsoft.Json.JsonIgnore]
    [System.ComponentModel.Browsable(false)]
    [System.ComponentModel.DataAnnotations.Display(AutoGenerateField = false)]
    public bool PropertyValueChanged
    {
      get { return _propertyValueChanged; }
    }

    private static readonly SynchronizedDictionary<string, FieldValue> _defaultFieldValues = 
      new SynchronizedDictionary<string, FieldValue>(StringComparer.Ordinal);
    private static readonly SynchronizedDictionary<string, Func<T, object>> _defaultValueFuncs =
      new SynchronizedDictionary<string, Func<T, object>>(StringComparer.Ordinal);

    [NonSerialized]
    [Csla.NotUndoable]
    private bool _ignoreFillRequiredFieldValues;

    [Csla.NotUndoable]
    private IList<FieldValue> _oldFieldValues;
    /// <summary>
    /// 旧值
    /// </summary>
    [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
    protected IList<FieldValue> OldFieldValues
    {
      get 
      {
        InitOldFieldValues(true, false);
        return _oldFieldValues;
      }
    }
    [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
    IList<FieldValue> IEntity.OldFieldValues
    {
      get { return OldFieldValues; }
    }

    /// <summary>
    /// 归档中
    /// </summary>
    protected bool Archiving
    {
      get { return SelfFetched && IsNew; }
    }

    private static bool? _deletedAsDisabled;
    /// <summary>
    /// 删除即禁用
    /// 当包含禁用字段(FieldAttribute.IsDisabledColumn = true)且存在唯一键时为 true
    /// 删除时, 仅将禁用字段置为CodingStandards.DefaultDisabledTrueValue
    /// </summary>
    protected internal static bool DeletedAsDisabled
    {
      get
      {
        if (!_deletedAsDisabled.HasValue)
        {
          ClassMapInfo classMapInfo = ClassMemberHelper.GetClassMapInfo(typeof(T));
          _deletedAsDisabled = classMapInfo.DeletedAsDisabled;
        }
        return _deletedAsDisabled.Value;
      }
    }
    bool IEntity.DeletedAsDisabled
    {
      get { return DeletedAsDisabled; }
    }

    /// <summary>
    /// 是否已禁用
    /// </summary>
    [System.ComponentModel.Browsable(false)]
    [System.ComponentModel.DataAnnotations.Display(AutoGenerateField = false)]
    public bool IsDisabled
    {
      get
      {
        ClassMapInfo classMapInfo = ClassMemberHelper.GetClassMapInfo(this.GetType());
        if (classMapInfo.DeletedAsDisabled)
        {
          object value = classMapInfo.DisabledFieldMapInfo.GetValue(this);
          if (value != null)
            return String.CompareOrdinal(value.ToString(), CodingStandards.DefaultDisabledTrueValue.Trim('\'')) == 0;
        }
        return false;
      }
      set
      {
        ClassMapInfo classMapInfo = ClassMemberHelper.GetClassMapInfo(this.GetType());
        if (classMapInfo.DeletedAsDisabled)
        {
          InitOldFieldValues(false, false);
          classMapInfo.DisabledFieldMapInfo.Set(this, value ? CodingStandards.DefaultDisabledTrueValue.Trim('\'') : CodingStandards.DefaultDisabledFalseValue.Trim('\''));
          MarkDirty();
        }
      }
    }

    /// <summary>
    /// 必须校验数据库数据在下载到提交期间是否被更改过
    /// 与ClassAttribute.AllowIgnoreCheckDirty值取反
    /// </summary>
    protected internal static bool MustCheckDirty
    {
      get
      {
        ClassMapInfo classMapInfo = ClassMemberHelper.GetClassMapInfo(typeof(T));
        return !classMapInfo.AllowIgnoreCheckDirty;
      }
    }

    [Csla.NotUndoable]
    private bool? _needCheckDirty;
    /// <summary>
    /// 需要校验数据库数据在下载到提交期间是否被更改过
    /// 缺省为ClassAttribute.AllowIgnoreCheckDirty(缺省为 false；如果为 true, 一旦发现将报错: CheckDirtyException)
    /// </summary>
    protected internal bool NeedCheckDirty
    {
      get
      {
        if (MustCheckDirty)
          return true;
        return _needCheckDirty ?? MustCheckDirty;
      }
      internal set { _needCheckDirty = value; }
    }
    bool IEntity.NeedCheckDirty
    {
      get { return NeedCheckDirty; }
    }
    
    private static DynamicMemberSetDelegate __fieldManagerFieldSetValue;

    private static DynamicMemberSetDelegate __isNewFieldSetValue;
    internal bool __isNew
    {
      set
      {
        if (IsNew == value)
          return;
        if (__isNewFieldSetValue == null)
          __isNewFieldSetValue = DynamicFactory.CreateFieldSetter(typeof(Csla.Core.BusinessBase).GetField("_isNew", BindingFlags.NonPublic | BindingFlags.Instance));
        __isNewFieldSetValue(this, value);
      }
    }

    private static DynamicMemberSetDelegate __isDirtyFieldSetValue;
    internal bool __isSelfDirty
    {
      set
      {
        if (IsSelfDirty == value)
          return;
        if (__isDirtyFieldSetValue == null)
          __isDirtyFieldSetValue = DynamicFactory.CreateFieldSetter(typeof(Csla.Core.BusinessBase).GetField("_isDirty", BindingFlags.NonPublic | BindingFlags.Instance));
        __isDirtyFieldSetValue(this, value);
      }
    }

    private static DynamicMemberSetDelegate __isDeletedFieldSetValue;
    internal bool __isSelfDeleted
    {
      set
      {
        if (IsDeleted == value)
          return;
        if (__isDeletedFieldSetValue == null)
          __isDeletedFieldSetValue = DynamicFactory.CreateFieldSetter(typeof(Csla.Core.BusinessBase).GetField("_isDeleted", BindingFlags.NonPublic | BindingFlags.Instance));
        __isDeletedFieldSetValue(this, value);
      }
    }

    private static DynamicMemberGetDelegate __stateStackFieldGetValue;
    private Stack<byte[]> __stateStack
    {
      get
      {
        if (__stateStackFieldGetValue == null)
          __stateStackFieldGetValue = DynamicFactory.CreateFieldGetter(typeof(Csla.Core.UndoableBase).GetField("_stateStack", BindingFlags.NonPublic | BindingFlags.Instance));
        return (Stack<byte[]>)__stateStackFieldGetValue(this);
      }
    }

    bool IEntity.IsNew
    {
      get { return IsNew; }
    }

    /// <summary>
    /// 是否被删除
    /// </summary>
    [System.ComponentModel.Browsable(false)]
    [System.ComponentModel.DataAnnotations.Display(AutoGenerateField = false)]
    public bool IsSelfDeleted
    {
      get { return IsDeleted; }
    }
    bool IEntity.IsSelfDeleted
    {
      get { return IsSelfDeleted; }
    }

    bool IEntity.IsSelfDirty
    {
      get { return IsSelfDirty; }
    }

    #region Validation Rules

    #region IDataSeverityInfo 成员

    /// <summary>
    /// 错误数
    /// </summary>
    protected int ErrorCount
    {
      get { return BrokenRulesCollection.ErrorCount; }
    }
    /// <summary>
    /// 错误数
    /// </summary>
    int IDataInvalidInfo.ErrorCount
    {
      get { return ErrorCount; }
    }

    /// <summary>
    /// 警告数
    /// </summary>
    protected int WarningCount
    {
      get { return BrokenRulesCollection.WarningCount; }
    }
    /// <summary>
    /// 警告数
    /// </summary>
    int IDataInvalidInfo.WarningCount
    {
      get { return WarningCount; }
    }

    /// <summary>
    /// 消息数
    /// </summary>
    protected int InformationCount
    {
      get { return BrokenRulesCollection.InformationCount; }
    }
    /// <summary>
    /// 消息数
    /// </summary>
    int IDataInvalidInfo.InformationCount
    {
      get { return InformationCount; }
    }
    
    #endregion

    /// <summary>
    /// 属性值赋值错误时是否抛异常
    /// </summary>
    protected virtual bool PropertySetErrorThrowException
    {
      get { return false; }
    }

    #endregion

    #region Authorization Rules

    private static DynamicMemberGetDelegate __bypassPropertyChecksFieldGetValue;
    private bool __bypassPropertyChecks
    {
      get
      {
        if (__bypassPropertyChecksFieldGetValue == null)
          __bypassPropertyChecksFieldGetValue = DynamicFactory.CreateFieldGetter(typeof(Csla.Core.BusinessBase).GetField("_bypassPropertyChecks", BindingFlags.NonPublic | BindingFlags.Instance));
        return (bool)__bypassPropertyChecksFieldGetValue(this);
      }
    }

    private static AuthorizationRules _authorizationRules;
    /// <summary>
    /// 授权规则集合
    /// </summary>
    protected static AuthorizationRules AuthorizationRules
    {
      get
      {
        if (_authorizationRules == null)
          _authorizationRules = new AuthorizationRules(typeof(T));
        return _authorizationRules;
      }
    }

    private static bool? _isReadOnly;
    /// <summary>
    /// 是否只读
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1000:DoNotDeclareStaticMembersOnGenericTypes")]
    public static bool IsReadOnly
    {
      get
      {
        if (!_isReadOnly.HasValue)
        {
          ClassMapInfo classMapInfo = ClassMemberHelper.GetClassMapInfo(typeof(T));
          _isReadOnly = classMapInfo.IsReadOnly;
        }
        return _isReadOnly.Value;
      }
    }

    /// <summary>
    /// 是否允许Fetch
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1000:DoNotDeclareStaticMembersOnGenericTypes")]
    public static bool CanFetch
    {
      get
      {
        if (ApplicationContext.LogicalExecutionLocation == ApplicationContext.LogicalExecutionLocations.Server)
          return true;
        return !UserIdentity.IsByDeny(UserIdentity.CurrentIdentity, typeof(T), ExecuteAction.Fetch, false) && 
          Csla.Rules.BusinessRules.HasPermission(Csla.Rules.AuthorizationActions.GetObject, typeof(T));
      }
    }

    /// <summary>
    /// 是否允许Create
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1000:DoNotDeclareStaticMembersOnGenericTypes")]
    public static bool CanCreate
    {
      get
      {
        if (IsReadOnly)
          return false;
        if (ApplicationContext.LogicalExecutionLocation == ApplicationContext.LogicalExecutionLocations.Server)
          return true;
        return !UserIdentity.IsByDeny(UserIdentity.CurrentIdentity, typeof(T), ExecuteAction.Insert, false) && 
          Csla.Rules.BusinessRules.HasPermission(Csla.Rules.AuthorizationActions.CreateObject, typeof(T));
      }
    }

    /// <summary>
    /// 是否允许Edit
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1000:DoNotDeclareStaticMembersOnGenericTypes")]
    public static bool CanEdit
    {
      get
      {
        if (IsReadOnly)
          return false;
        if (ApplicationContext.LogicalExecutionLocation == ApplicationContext.LogicalExecutionLocations.Server)
          return true;
        return !UserIdentity.IsByDeny(UserIdentity.CurrentIdentity, typeof(T), ExecuteAction.Update, false) && 
          Csla.Rules.BusinessRules.HasPermission(Csla.Rules.AuthorizationActions.EditObject, typeof(T));
      }
    }

    /// <summary>
    /// 是否允许Delete
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1000:DoNotDeclareStaticMembersOnGenericTypes")]
    public static bool CanDelete
    {
      get
      {
        if (IsReadOnly)
          return false;
        if (ApplicationContext.LogicalExecutionLocation == ApplicationContext.LogicalExecutionLocations.Server)
          return true;
        return !UserIdentity.IsByDeny(UserIdentity.CurrentIdentity, typeof(T), ExecuteAction.Delete, false) && 
          Csla.Rules.BusinessRules.HasPermission(Csla.Rules.AuthorizationActions.DeleteObject, typeof(T));
      }
    }

    /// <summary>
    /// 是否允许设置本对象
    /// 只读则为false
    /// </summary>
    [System.ComponentModel.Browsable(false)]
    [System.ComponentModel.DataAnnotations.Display(AutoGenerateField = false)]
    public virtual bool AllowSet
    {
      get
      {
        if (IsReadOnly)
          return false;
        return UserIdentity.CurrentIdentity != null 
          ? UserIdentity.CurrentIdentity.AllowSet(this)
          : AppConfig.AutoMode;
      }
    }
    /// <summary>
    /// 是否允许设置本对象
    /// 只读则为false
    /// </summary>
    [System.ComponentModel.Browsable(false)]
    [System.ComponentModel.DataAnnotations.Display(AutoGenerateField = false)]
    [Newtonsoft.Json.JsonIgnore]
    public BooleanOption AllowSetOption
    {
      get { return AllowSet ? BooleanOption.Y : BooleanOption.N; }
    }

    /// <summary>
    /// 是否允许编辑本对象
    /// </summary>
    [System.ComponentModel.Browsable(false)]
    [System.ComponentModel.DataAnnotations.Display(AutoGenerateField = false)]
    public virtual bool AllowEdit
    {
      get { return CanEdit && AllowSet; }
    }
    /// <summary>
    /// 是否允许编辑本对象
    /// </summary>
    [System.ComponentModel.Browsable(false)]
    [System.ComponentModel.DataAnnotations.Display(AutoGenerateField = false)]
    [Newtonsoft.Json.JsonIgnore]
    public BooleanOption AllowEditOption
    {
      get { return AllowEdit ? BooleanOption.Y : BooleanOption.N; }
    }

    /// <summary>
    /// 是否允许删除本对象
    /// </summary>
    [System.ComponentModel.Browsable(false)]
    [System.ComponentModel.DataAnnotations.Display(AutoGenerateField = false)]
    public virtual bool AllowDelete
    {
      get
      {
        if (IsNew)
          return true;
        return CanDelete && AllowSet;
      }
    }
    /// <summary>
    /// 是否允许删除本对象
    /// </summary>
    [System.ComponentModel.Browsable(false)]
    [System.ComponentModel.DataAnnotations.Display(AutoGenerateField = false)]
    [Newtonsoft.Json.JsonIgnore]
    public BooleanOption AllowDeleteOption
    {
      get { return AllowDelete ? BooleanOption.Y : BooleanOption.N; }
    }

    #endregion

    #endregion

    #region 事件

    /// <summary>
    /// 业务规则注册中事件
    /// 可配置化：当应用程序初始化时，可通过本事件来添加额外的业务规则库
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1000:DoNotDeclareStaticMembersOnGenericTypes")]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1009:DeclareEventHandlersCorrectly")]
    public static event BusinessRuleRegisteringEventHandler BusinessRuleRegistering;
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
    private static void OnBusinessRuleRegistering(Csla.Rules.BusinessRules businessRules)
    {
      if (BusinessRuleRegistering == null)
        return;
      foreach (BusinessRuleRegisteringEventHandler item in BusinessRuleRegistering.GetInvocationList())
        try
        {
          item.Invoke(businessRules);
        }
        catch (Exception ex)
        {
          EventLog.Save(typeof(T), MethodBase.GetCurrentMethod(), ex);
          BusinessRuleRegistering -= item;
        }
    }

    /// <summary>
    /// 授权规则注册中事件
    /// 可配置化：当应用程序初始化时，可通过本事件来添加额外的授权规则库
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1000:DoNotDeclareStaticMembersOnGenericTypes")]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1009:DeclareEventHandlersCorrectly")]
    public static event AuthorizationRuleRegisteringEventHandler AuthorizationRuleRegistering;
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
    private static void OnAuthorizationRuleRegistering(AuthorizationRules authorizationRules)
    {
      if (AuthorizationRuleRegistering == null)
        return;
      foreach (AuthorizationRuleRegisteringEventHandler item in AuthorizationRuleRegistering.GetInvocationList())
        try
        {
          item.Invoke(authorizationRules);
        }
        catch (Exception ex)
        {
          EventLog.Save(typeof(T), MethodBase.GetCurrentMethod(), ex);
          AuthorizationRuleRegistering -= item;
        }
    }

    #endregion

    #region 方法

    /// <summary>
    /// 取对象ID
    /// </summary>
    protected override object GetIdValue()
    {
      return IdValue;
    }

    /// <summary>
    /// 取哈希值
    /// </summary>
    public override int GetHashCode()
    {
      return IdValue.GetHashCode();
    }

    /// <summary>
    /// 比较对象
    /// </summary>
    public override bool Equals(object obj)
    {
      if (object.ReferenceEquals(obj, this))
        return true;
      T other = obj as T; 
      if (object.ReferenceEquals(other, null))
        return false;
      return IdValue == other.IdValue;
    }

    #region FieldMapInfo

    internal bool FindPropertyInfo(string propertyName, out Csla.Core.IPropertyInfo property)
    {
      foreach (Csla.Core.IPropertyInfo item in FieldManager.GetRegisteredProperties())
        if (String.CompareOrdinal(item.Name, propertyName) == 0)
        {
          property = item;
          return true;
        }
      property = null;
      return false;
    }

    /// <summary>
    /// 获取数据映射字段信息
    /// </summary>
    /// <param name="property">属性信息</param>
    [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Advanced)]
    protected static FieldMapInfo GetFieldMapInfo(Csla.Core.IPropertyInfo property)
    {
      if (property == null)
        return null;
      Phenix.Core.Mapping.IPropertyInfo propertyInfo = property as Phenix.Core.Mapping.IPropertyInfo;
      return propertyInfo != null
        ? propertyInfo.FieldMapInfo
        : GetFieldMapInfo(property.Name);
    }

    /// <summary>
    /// 获取数据映射字段信息
    /// </summary>
    /// <param name="propertyName">属性名</param>
    [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Advanced)]
    protected static FieldMapInfo GetFieldMapInfo(string propertyName)
    {
      return ClassMemberHelper.GetFieldMapInfo(typeof(T), propertyName);
    }

    #endregion

    #region 克隆

    /// <summary>
    /// 置换为与source相同内容的对象
    /// </summary>
    public virtual void ReplaceFrom(T source)
    {
      if (object.ReferenceEquals(source, null) || object.ReferenceEquals(source, this))
        return;

      _idValue = source._idValue;
      _propertyValueChanged = source._propertyValueChanged;
      _oldFieldValues = (FieldValue[])Phenix.Core.Reflection.Utilities.Clone(source._oldFieldValues);
      _ignoreFillRequiredFieldValues = source._ignoreFillRequiredFieldValues;
      EntityHelper.FillFieldValues(source, this, true, true);

      if (source.IsNew)
        base.MarkNew();
      if (source.IsSelfDeleted)
        base.MarkDeleted();
      if (source.IsSelfDirty)
        base.MarkDirty();
      else
        base.MarkClean();
    }

    #endregion

    #region 初始化

    /// <summary>
    /// 初始化
    /// </summary>
    protected override void Initialize()
    {
      base.Initialize();
      //缺省下，屏蔽响应 BindingSource 的 System.ComponentModel.IEditableObject 接口
      DisableIEditableObject = true;
    }

    internal void ClearFieldManagerFieldInfo()
    {
      if (__fieldManagerFieldSetValue == null)
        __fieldManagerFieldSetValue = DynamicFactory.CreateFieldSetter(typeof(Csla.Core.BusinessBase).GetField("_fieldManager", BindingFlags.NonPublic | BindingFlags.Instance));
      __fieldManagerFieldSetValue(this, null);
    }

    #endregion

    #region Mark

    /// <summary>
    /// 标为 IsNew = true 且 IsSelfDeleted = false 且 IsSelfDirty = true
    /// </summary>
    protected internal void MarkNewDirty()
    {
      __isNew = true;
      __isSelfDeleted = false;
      __isSelfDirty = true;
    }

    ///// <summary>
    ///// 标为 IsNew = true 且 IsSelfDeleted = true 且 IsSelfDirty = true
    ///// </summary>
    //protected internal void MarkNewDeleted()
    //{
    //  __isNew = true;
    //  __isSelfDeleted = true;
    //  __isSelfDirty = true;
    //}

    /// <summary>
    /// 标为 IsNew = false 且 IsSelfDeleted = false 且 IsSelfDirty = true
    /// </summary>
    protected internal void MarkOldDirty()
    {
      __isNew = false;
      __isSelfDeleted = false;
      __isSelfDirty = true;
    }

    /// <summary>
    /// 标为 IsNew = false 且 IsSelfDeleted = false 且 IsSelfDirty = false
    /// </summary>
    protected internal void MarkOldClean()
    {
      __isNew = false;
      __isSelfDeleted = false;
      __isSelfDirty = false;
    }

    /// <summary>
    /// 标为 IsNew = false 且 IsSelfDeleted = true 且 IsSelfDirty = false
    /// </summary>
    protected internal void MarkOldDelete()
    {
      __isNew = false;
      __isSelfDeleted = true;
      __isSelfDirty = false;
    }

    /// <summary>
    /// 标为 SelfFetched = true 且 IsNew = false 且 IsSelfDeleted = false 且 IsSelfDirty = false
    /// </summary>
    protected virtual void MarkFetched()
    {
      _propertyValueChanged = false;
      _ignoreFillRequiredFieldValues = false;
      _oldFieldValues = null;

      MarkOldClean();

      _selfFetched = true;
    }
    void IEntity.MarkFetched()
    {
      MarkFetched();
    }

    /// <summary>
    /// 标为 IsNew = true 且 IsSelfDeleted = false 且 IsSelfDirty = true
    /// </summary>
    protected override void MarkNew()
    {
      _propertyValueChanged = false;
      _ignoreFillRequiredFieldValues = false;
      _oldFieldValues = null;

      base.MarkNew();
    }

    /// <summary>
    /// 标为 IsNew = false 且 IsSelfDirty = false
    /// resetOldFieldValues = false
    /// </summary>
    protected override void MarkOld()
    {
      MarkOld(false);
    }

    /// <summary>
    /// 标为 IsNew = false 且 IsSelfDirty = false
    /// </summary>
   [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Advanced)]
    protected virtual void MarkOld(bool resetOldFieldValues)
    {
      _propertyValueChanged = false;
      _ignoreFillRequiredFieldValues = false;

      if (resetOldFieldValues)
        InitOldFieldValues(true, true);

      base.MarkOld();
    }

    /// <summary>
    /// 标为 IsSelfDirty = false
    /// </summary>
    protected new void MarkClean()
    {
      _propertyValueChanged = false;
      _ignoreFillRequiredFieldValues = false;
      
      base.MarkClean();
    }

    /// <summary>
    /// 标为 SelfFetched = true 且 IsNew = true 
    /// </summary>
    protected void MarkArchiving()
    {
      MarkFetched();
      MarkNew();
    }

    #endregion

    #region Property

    #region Get Properties

    /// <summary>
    /// 读取属性
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters")]
    [Obsolete("请使用: GetProperty<P>(Csla.PropertyInfo<P> propertyInfo, P field)", true)]
    [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
    protected new object GetProperty(Csla.Core.IPropertyInfo propertyInfo)
    {
      throw new NotSupportedException(MethodBase.GetCurrentMethod().Name);
    }

    /// <summary>
    /// 读取属性
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters")]
    [Obsolete("请使用: GetProperty<P>(Csla.PropertyInfo<P> propertyInfo, P field)", true)]
    [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
    protected new P GetProperty<P>(Csla.Core.IPropertyInfo propertyInfo)
    {
      throw new NotSupportedException(MethodBase.GetCurrentMethod().Name);
    }

    /// <summary>
    /// 读取属性
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters")]
    [Obsolete("请使用: GetProperty<P>(Csla.PropertyInfo<P> propertyInfo, P field)", true)]
    [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
    protected new P GetProperty<P>(Csla.PropertyInfo<P> propertyInfo)
    {
      throw new NotSupportedException(MethodBase.GetCurrentMethod().Name);
    }

    /// <summary>
    /// 读取属性
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters")]
    [Obsolete("请使用: GetProperty<P>(Csla.PropertyInfo<P> propertyInfo, P field, Csla.Security.NoAccessBehavior noAccess)", true)]
    [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
    protected new P GetProperty<P>(Csla.PropertyInfo<P> propertyInfo, Csla.Security.NoAccessBehavior noAccess)
    {
      throw new NotSupportedException(MethodBase.GetCurrentMethod().Name);
    }

    /// <summary>
    /// 读取属性
    /// noAccess = Csla.Security.NoAccessBehavior.SuppressException
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1061:DoNotHideBaseClassMethods")]
    protected new P GetProperty<P>(Csla.PropertyInfo<P> propertyInfo, P field)
    {
      return GetProperty<P>(propertyInfo, field, Csla.Security.NoAccessBehavior.SuppressException);
    }

    /// <summary>
    /// 读取属性
    /// </summary>
    protected P GetProperty<P>(Csla.PropertyInfo<P> propertyInfo, P field, Csla.Security.NoAccessBehavior noAccess)
    {
      if (__bypassPropertyChecks || CanReadProperty(propertyInfo, noAccess == Csla.Security.NoAccessBehavior.ThrowException))
        return field;
      return propertyInfo.DefaultValue;
    }

    /// <summary>
    /// 读取属性
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters")]
    [Obsolete("请使用: GetProperty<P>(Csla.PropertyInfo<P> propertyInfo, P field, Csla.Security.NoAccessBehavior noAccess)", true)]
    [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
    protected new P GetProperty<P>(Csla.PropertyInfo<P> propertyInfo, P field, P defaultValue, Csla.Security.NoAccessBehavior noAccess)
    {
      throw new NotSupportedException(MethodBase.GetCurrentMethod().Name);
    }

    /// <summary>
    /// 读取属性
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters")]
    [Obsolete("请使用: GetProperty<P>(Csla.PropertyInfo<P> propertyInfo, P field)", true)]
    [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
    protected new P GetProperty<P>(string propertyName, P field, P defaultValue)
    {
      throw new NotSupportedException(MethodBase.GetCurrentMethod().Name);
    }

    /// <summary>
    /// 读取属性
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters")]
    [Obsolete("请使用: GetProperty<P>(Csla.PropertyInfo<P> propertyInfo, P field, Csla.Security.NoAccessBehavior noAccess)", true)]
    [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
    protected new P GetProperty<P>(string propertyName, P field, P defaultValue, Csla.Security.NoAccessBehavior noAccess)
    {
      throw new NotSupportedException(MethodBase.GetCurrentMethod().Name);
    }

    /// <summary>
    /// 读取属性
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters")]
    [Obsolete("请使用: GetPropertyConvert<P, F>(Csla.PropertyInfo<P> propertyInfo, F field)", true)]
    [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
    protected new P GetPropertyConvert<F, P>(Csla.PropertyInfo<F> propertyInfo)
    {
      throw new NotSupportedException(MethodBase.GetCurrentMethod().Name);
    }

    /// <summary>
    /// 读取属性
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters")]
    [Obsolete("请使用: GetPropertyConvert<P, F>(Csla.PropertyInfo<P> propertyInfo, F field, Csla.Security.NoAccessBehavior noAccess)", true)]
    [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
    protected new P GetPropertyConvert<F, P>(Csla.PropertyInfo<F> propertyInfo, Csla.Security.NoAccessBehavior noAccess)
    {
      throw new NotSupportedException(MethodBase.GetCurrentMethod().Name);
    }

    /// <summary>
    /// 读取属性
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters")]
    [Obsolete("请使用: GetPropertyConvert<P, F>(Csla.PropertyInfo<P> propertyInfo, F field)", true)]
    [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
    protected new P GetPropertyConvert<F, P>(Csla.PropertyInfo<F> propertyInfo, F field)
    {
      throw new NotSupportedException(MethodBase.GetCurrentMethod().Name);
    }

    /// <summary>
    /// 读取属性
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters")]
    [Obsolete("请使用: GetPropertyConvert<P, F>(Csla.PropertyInfo<P> propertyInfo, F field, Csla.Security.NoAccessBehavior noAccess)", true)]
    [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
    protected new P GetPropertyConvert<F, P>(Csla.PropertyInfo<F> propertyInfo, F field, Csla.Security.NoAccessBehavior noAccess)
    {
      throw new NotSupportedException(MethodBase.GetCurrentMethod().Name);
    }

    /// <summary>
    /// 读取属性
    /// noAccess = Csla.Security.NoAccessBehavior.SuppressException
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1061:DoNotHideBaseClassMethods")]
    protected P GetPropertyConvert<P, F>(Csla.PropertyInfo<P> propertyInfo, F field)
    {
      return GetPropertyConvert<P, F>(propertyInfo, field, Csla.Security.NoAccessBehavior.SuppressException);
    }

    /// <summary>
    /// 读取属性
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1061:DoNotHideBaseClassMethods")]
    protected P GetPropertyConvert<P, F>(Csla.PropertyInfo<P> propertyInfo, F field, Csla.Security.NoAccessBehavior noAccess)
    {
      return GetProperty<P>(propertyInfo, (P)Phenix.Core.Reflection.Utilities.ChangeType(field, typeof(P)), noAccess);
    }

    #endregion

    #region Set Properties

    /// <summary>
    /// 写入属性(新旧值相同时忽略写入)
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters")]
    [Obsolete("请使用: SetProperty<P>(Csla.PropertyInfo<P> propertyInfo, ref P field, P newValue)", true)]
    [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
    protected new void SetProperty(Csla.Core.IPropertyInfo propertyInfo, object newValue)
    {
      throw new NotSupportedException(MethodBase.GetCurrentMethod().Name);
    }

    /// <summary>
    /// 写入属性(新旧值相同时忽略写入)
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters")]
    [Obsolete("请使用: SetProperty<P>(Csla.PropertyInfo<P> propertyInfo, ref P field, P newValue)", true)]
    [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
    protected new void SetProperty<P>(Csla.Core.IPropertyInfo propertyInfo, P newValue)
    {
      throw new NotSupportedException(MethodBase.GetCurrentMethod().Name);
    }

    /// <summary>
    /// 写入属性(新旧值相同时忽略写入)
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters")]
    [Obsolete("请使用: SetProperty<P>(Csla.PropertyInfo<P> propertyInfo, ref P field, P newValue)", true)]
    [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
    protected new void SetProperty<P>(Csla.PropertyInfo<P> propertyInfo, P newValue)
    {
      throw new NotSupportedException(MethodBase.GetCurrentMethod().Name);
    }

    /// <summary>
    /// 写入属性(新旧值相同时忽略写入)
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters")]
    [Obsolete("请使用: SetProperty<P>(Csla.PropertyInfo<P> propertyInfo, ref P field, P newValue, Csla.Security.NoAccessBehavior noAccess)", true)]
    [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
    protected new void SetProperty<P>(Csla.PropertyInfo<P> propertyInfo, P newValue, Csla.Security.NoAccessBehavior noAccess)
    {
      throw new NotSupportedException(MethodBase.GetCurrentMethod().Name);
    }

    /// <summary>
    /// 写入属性(新旧值相同时忽略写入)
    /// noAccess = Csla.Security.NoAccessBehavior.ThrowException
    /// </summary>
    protected new void SetProperty<P>(Csla.PropertyInfo<P> propertyInfo, ref P field, P newValue)
    {
      SetProperty<P>(propertyInfo, ref field, newValue, Csla.Security.NoAccessBehavior.ThrowException);
    }

    /// <summary>
    /// 写入属性(新旧值相同时忽略写入)
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters")]
    protected void SetProperty<P>(Csla.PropertyInfo<P> propertyInfo, ref P field, P newValue, Csla.Security.NoAccessBehavior noAccess)
    {
      if (object.Equals(field, newValue))
        return;
      if (__bypassPropertyChecks || CanWriteProperty(propertyInfo, noAccess == Csla.Security.NoAccessBehavior.ThrowException))
      {
        if (!__bypassPropertyChecks)
          OnPropertyChanging(propertyInfo);
        ExecuteRules(GetFieldMapInfo(propertyInfo), ref field, newValue);
        if (!__bypassPropertyChecks)
          PropertyHasChanged(propertyInfo);
      }
    }

    /// <summary>
    /// 写入属性(新旧值相同时忽略写入)
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters")]
    [Obsolete("请使用: SetProperty<P>(Csla.PropertyInfo<P> propertyInfo, ref P field, P newValue)", true)]
    [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
    protected new void SetProperty<P>(string propertyName, ref P field, P newValue)
    {
      throw new NotSupportedException(MethodBase.GetCurrentMethod().Name);
    }

    /// <summary>
    /// 写入属性(新旧值相同时忽略写入)
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters")]
    [Obsolete("请使用: SetProperty<P>(Csla.PropertyInfo<P> propertyInfo, ref P field, P newValue, Csla.Security.NoAccessBehavior noAccess)", true)]
    [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
    protected new void SetProperty<P>(string propertyName, ref P field, P newValue, Csla.Security.NoAccessBehavior noAccess)
    {
      throw new NotSupportedException(MethodBase.GetCurrentMethod().Name);
    }

    /// <summary>
    /// 写入属性(新旧值相同时忽略写入)
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters")]
    [Obsolete("请使用: SetPropertyConvert<P, F>(Csla.PropertyInfo<P> propertyInfo, ref F field, P newValue, Csla.Security.NoAccessBehavior noAccess)", true)]
    [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
    protected new void SetPropertyConvert<P, V>(string propertyName, ref P field, V newValue, Csla.Security.NoAccessBehavior noAccess)
    {
      throw new NotSupportedException(MethodBase.GetCurrentMethod().Name);
    }

    /// <summary>
    /// 写入属性(新旧值相同时忽略写入)
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters")]
    [Obsolete("请使用: SetPropertyConvert<P, F>(Csla.PropertyInfo<P> propertyInfo, ref F field, P newValue)", true)]
    [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
    protected new void SetPropertyConvert<P, F>(Csla.PropertyInfo<P> propertyInfo, F newValue)
    {
      throw new NotSupportedException(MethodBase.GetCurrentMethod().Name);
    }

    /// <summary>
    /// 写入属性(新旧值相同时忽略写入)
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters")]
    [Obsolete("请使用: SetPropertyConvert<P, F>(Csla.PropertyInfo<P> propertyInfo, ref F field, P newValue, Csla.Security.NoAccessBehavior noAccess)", true)]
    [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
    protected new void SetPropertyConvert<P, F>(Csla.PropertyInfo<P> propertyInfo, F newValue, Csla.Security.NoAccessBehavior noAccess)
    {
      throw new NotSupportedException(MethodBase.GetCurrentMethod().Name);
    }

    /// <summary>
    /// 写入属性(新旧值相同时忽略写入)
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters")]
    [Obsolete("请使用: SetPropertyConvert<P, F>(Csla.PropertyInfo<P> propertyInfo, ref F field, P newValue)", true)]
    [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
    protected new void SetPropertyConvert<P, V>(Csla.PropertyInfo<P> propertyInfo, ref P field, V newValue)
    {
      throw new NotSupportedException(MethodBase.GetCurrentMethod().Name);
    }

    /// <summary>
    /// 写入属性(新旧值相同时忽略写入)
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters")]
    [Obsolete("请使用: SetPropertyConvert<P, F>(Csla.PropertyInfo<P> propertyInfo, ref F field, P newValue, Csla.Security.NoAccessBehavior noAccess)", true)]
    [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
    protected new void SetPropertyConvert<P, V>(Csla.PropertyInfo<P> propertyInfo, ref P field, V newValue, Csla.Security.NoAccessBehavior noAccess)
    {
      throw new NotSupportedException(MethodBase.GetCurrentMethod().Name);
    }

    /// <summary>
    /// 写入属性(新旧值相同时忽略写入)
    /// noAccess = Csla.Security.NoAccessBehavior.ThrowException
    /// </summary>
    protected void SetPropertyConvert<P, F>(Csla.PropertyInfo<P> propertyInfo, ref F field, P newValue)
    {
      SetPropertyConvert<P, F>(propertyInfo, ref field, newValue, Csla.Security.NoAccessBehavior.ThrowException);
    }

    /// <summary>
    /// 写入属性(新旧值相同时忽略写入)
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters")]
    protected void SetPropertyConvert<P, F>(Csla.PropertyInfo<P> propertyInfo, ref F field, P newValue, Csla.Security.NoAccessBehavior noAccess)
    {
      F newFieldValue = (F)Phenix.Core.Reflection.Utilities.ChangeType(newValue, typeof(F));
      if (object.Equals(field, newFieldValue))
        return;
      if (__bypassPropertyChecks || CanWriteProperty(propertyInfo, noAccess == Csla.Security.NoAccessBehavior.ThrowException))
      {
        if (!__bypassPropertyChecks)
          OnPropertyChanging(propertyInfo);
        ExecuteRules(GetFieldMapInfo(propertyInfo), ref field, newFieldValue);
        if (!__bypassPropertyChecks)
          PropertyHasChanged(propertyInfo);
      }
    }

    /// <summary>
    /// 执行业务规则
    /// </summary>
    protected virtual void ExecuteRules<F>(FieldMapInfo fieldMapInfo, ref F field, F newValue)
    {
      field = fieldMapInfo != null ? (F)ClassMemberHelper.ExecuteRules(fieldMapInfo, this, newValue) : newValue;
    }

    #endregion

    #region AuthorizeReadWrite

    /// <summary>
    /// 属性可读
    /// throwIfDeny = false
    /// </summary>
    public override bool CanReadProperty(Csla.Core.IPropertyInfo property)
    {
      return CanReadProperty(property, false);
    }

    /// <summary>
    /// 属性可读
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters")]
    public new bool CanReadProperty(Csla.Core.IPropertyInfo property, bool throwIfDeny)
    {
      if (property == null)
        return true;
      return CanReadProperty(property.Name, throwIfDeny);
    }

    /// <summary>
    /// 属性可写
    /// </summary>
    public virtual bool CanReadProperty(string propertyName, bool throwIfDeny)
    {
      return AuthorizationRules.HasPermission(this, MethodAction.ReadProperty, propertyName, IsNew, throwIfDeny);
    }

    /// <summary>
    /// 属性可读
    /// </summary>
    public new bool CanReadProperty(string propertyName)
    {
      return CanReadProperty(propertyName, false);
    }
    bool Csla.Security.IAuthorizeReadWrite.CanReadProperty(string propertyName)
    {
      return CanReadProperty(propertyName);
    }

    /// <summary>
    /// 允许属性可读
    /// 与Phenix.Services.Client.Security.ReadWriteAuthorization配套使用
    /// </summary>
    public virtual bool AllowReadProperty(string propertyName)
    {
      return CanReadProperty(propertyName, false);
    }

    /// <summary>
    /// 属性可写
    /// throwIfDeny = false
    /// </summary>
    public override bool CanWriteProperty(Csla.Core.IPropertyInfo property)
    {
      return CanWriteProperty(property, false);
    }

    /// <summary>
    /// 属性可写
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters")]
    public new bool CanWriteProperty(Csla.Core.IPropertyInfo property, bool throwIfDeny)
    {
      if (property == null)
        return true;
      return CanWriteProperty(property.Name, throwIfDeny);
    }

    /// <summary>
    /// 属性可写
    /// </summary>
    public virtual bool CanWriteProperty(string propertyName, bool throwIfDeny)
    {
      return AuthorizationRules.HasPermission(this, MethodAction.WriteProperty, propertyName, IsNew, throwIfDeny);
    }

    /// <summary>
    /// 属性可写
    /// throwIfDeny = false
    /// </summary>
    public new bool CanWriteProperty(string propertyName)
    {
      return CanWriteProperty(propertyName, false);
    }
    bool Csla.Security.IAuthorizeReadWrite.CanWriteProperty(string propertyName)
    {
      return CanWriteProperty(propertyName);
    }

    /// <summary>
    /// 允许属性可写
    /// 与Phenix.Services.Client.Security.ReadWriteAuthorization配套使用
    /// </summary>
    public virtual bool AllowWriteProperty(string propertyName)
    {
      return CanWriteProperty(propertyName, false);
    }
    
    #endregion

    #region PropertyChange

    /// <summary>
    /// 属性已更改
    /// </summary>
    protected void PropertyHasChanged()
    {
      string propertyName = new System.Diagnostics.StackTrace().GetFrame(1).GetMethod().Name.Substring(4);
      Csla.Core.IPropertyInfo propertyInfo;
      if (FindPropertyInfo(propertyName, out propertyInfo))
        PropertyHasChanged(propertyInfo);
      else
        OnPropertyChanged(propertyName);
    }
    
    /// <summary>
    /// 属性发生更改时
    /// </summary>
    /// <param name="propertyInfo">属性信息</param>
    protected override void OnPropertyChanging(Csla.Core.IPropertyInfo propertyInfo)
    {
      OnPropertyChanging(propertyInfo != null ? propertyInfo.Name : null);
    }

    /// <summary>
    /// 属性发生更改时
    /// </summary>
    /// <param name="propertyName">属性名</param>
    protected override void OnPropertyChanging(string propertyName)
    {
      InitOldFieldValues(false, false);
      FillRequiredFieldValues();
      base.OnPropertyChanging(propertyName);
    }

    /// <summary>
    /// 属性发生更改后
    /// </summary>
    protected override void OnUnknownPropertyChanged()
    {
      if (IsSelfDirty && !IsSelfDeleted)
      {
        InitOldFieldValues(false, false);
        FillRequiredFieldValues();
      }
      base.OnUnknownPropertyChanged();
    }

    /// <summary>
    /// 属性发生更改后
    /// </summary>
    /// <param name="propertyInfo">属性信息</param>
    protected override void OnPropertyChanged(Csla.Core.IPropertyInfo propertyInfo)
    {
      OnPropertyChanged(propertyInfo != null ? propertyInfo.Name : null);
    }

    /// <summary>
    /// 属性发生更改后
    /// </summary>
    /// <param name="propertyName">属性名</param>
    protected override void OnPropertyChanged(string propertyName)
    {
      if (!String.IsNullOrEmpty(propertyName))
      {
        if (PropertySetErrorThrowException)
        {
          string error = ((IDataErrorInfo)this)[propertyName];
          if (!String.IsNullOrEmpty(error))
            throw new Csla.Rules.ValidationException(error);
        }
        _propertyValueChanged = true;
        SetDirtyProperty(propertyName);
      }
      else
      {
        if (PropertySetErrorThrowException)
        {
          string error = ((IDataErrorInfo)this).Error;
          if (!String.IsNullOrEmpty(error))
            throw new Csla.Rules.ValidationException(error);
        }
      }
      base.OnPropertyChanged(propertyName);
    }

    /// <summary>
    /// 设置脏属性
    /// </summary>
    /// <param name="propertyName">属性名</param>
    protected void SetDirtyProperty(string propertyName)
    {
      FieldValue fieldValue = GetOldFieldValue(propertyName);
      if (fieldValue != null)
        fieldValue.IsDirty = true;
    }

    /// <summary>
    /// 设置脏属性
    /// </summary>
    /// <param name="propertyInfo">属性信息</param>
    protected void SetDirtyProperty(Phenix.Core.Mapping.IPropertyInfo propertyInfo)
    {
      FieldValue fieldValue = GetOldFieldValue(propertyInfo);
      if (fieldValue != null)
        fieldValue.IsDirty = true;
    }

    /// <summary>
    /// 是否脏属性?(如果写入时的新值与旧值相同则认为未被赋值过)
    /// ignoreCompare = false
    /// </summary>
    /// <param name="propertyInfo">属性信息</param>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters")]
    public bool IsDirtyProperty(Phenix.Core.Mapping.IPropertyInfo propertyInfo)
    {
      return IsDirtyProperty(propertyInfo, false);
    }

    /// <summary>
    /// 是否脏属性?(如果写入时的新值与旧值相同则认为未被赋值过)
    /// </summary>
    /// <param name="propertyInfo">属性信息</param>
    /// <param name="ignoreCompare">忽略比较新旧值(仅判断是否被赋值过)</param>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters")]
    public bool IsDirtyProperty(Phenix.Core.Mapping.IPropertyInfo propertyInfo, bool ignoreCompare)
    {
      if (propertyInfo == null)
        throw new ArgumentNullException("propertyInfo");
      if (IsSelfDirty)
      {
        FieldValue fieldValue = GetOldFieldValue(propertyInfo);
        if (fieldValue != null)
          return fieldValue.IsDirty.HasValue && fieldValue.IsDirty.Value ||
            !ignoreCompare && (IsNew || !fieldValue.IsDirty.HasValue && !object.Equals(GetCurrentValue(propertyInfo), fieldValue.Value));
      }
      return false;
    }

    /// <summary>
    /// 是否有效属性
    /// </summary>
    /// <param name="property">属性信息</param>
    /// <returns>有效</returns>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters")]
    public bool IsValidProperty(Csla.Core.IPropertyInfo property)
    {
      return String.IsNullOrEmpty(((IDataErrorInfo)this)[property.Name]);
    }

    #endregion

    #endregion
    
    #region Methods

    /// <summary>
    /// 注册方法信息
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters")]
    [Obsolete("请使用: RegisterMethod(Expression<Action<T>> methodLambdaExpression)", true)]
    protected new static Csla.Core.IMemberInfo RegisterMethod(Type objectType, Csla.Core.IMemberInfo info)
    {
      throw new NotSupportedException(MethodBase.GetCurrentMethod().Name);
    }

    /// <summary>
    /// 注册方法信息
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters")]
    [Obsolete("请使用: RegisterMethod(Expression<Action<T>> methodLambdaExpression)", true)]
    protected new static Csla.MethodInfo RegisterMethod(Type objectType, string methodName)
    {
      throw new NotSupportedException(MethodBase.GetCurrentMethod().Name);
    }

    /// <summary>
    /// 注册方法信息
    /// friendlyName = null
    /// tag = null
    /// </summary>
    /// <param name="methodName">方法名</param>
    /// <returns>方法信息</returns>
    protected static MethodInfo RegisterMethod(string methodName)
    {
      return RegisterMethod(methodName, null, null);
    }

    /// <summary>
    /// 注册方法信息
    /// tag = null
    /// </summary>
    /// <param name="methodName">方法名</param>
    /// <param name="friendlyName">友好名</param>
    /// <returns>方法信息</returns>
    protected static MethodInfo RegisterMethod(string methodName, string friendlyName)
    {
      return RegisterMethod(methodName, friendlyName, null);
    }

    /// <summary>
    /// 注册方法信息
    /// </summary>
    /// <param name="methodName">方法名</param>
    /// <param name="friendlyName">友好名</param>
    /// <param name="tag">标记</param>
    /// <returns>方法信息</returns>
    protected static MethodInfo RegisterMethod(string methodName, string friendlyName, string tag)
    {
      Type type = typeof(T);
      System.Reflection.MethodInfo methodInfo = type.GetMethod(methodName);
      if (methodInfo == null)
        throw new ArgumentException(String.Format(Phenix.Core.Properties.Resources.NoSuchMethod, methodName), "methodName");
      return new MethodInfo(type, methodName, friendlyName, tag, ClassMemberHelper.GetMethodMapInfo(type, methodName));
    }

    /// <summary>
    /// 注册方法信息
    /// friendlyName = null
    /// tag = null
    /// </summary>
    /// <param name="methodLambdaExpression">方法表达式</param>
    /// <returns>方法信息</returns>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures")]
    protected new static MethodInfo RegisterMethod(Expression<Action<T>> methodLambdaExpression)
    {
      return RegisterMethod(methodLambdaExpression, null, null);
    }

    /// <summary>
    /// 注册方法信息
    /// tag = null
    /// </summary>
    /// <param name="methodLambdaExpression">方法表达式</param>
    /// <param name="friendlyName">友好名</param>
    /// <returns>方法信息</returns>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures")]
    protected static MethodInfo RegisterMethod(Expression<Action<T>> methodLambdaExpression, string friendlyName)
    {
      return RegisterMethod(methodLambdaExpression, friendlyName, null);
    }

    /// <summary>
    /// 注册方法信息
    /// </summary>
    /// <param name="methodLambdaExpression">方法表达式</param>
    /// <param name="friendlyName">友好名</param>
    /// <param name="tag">标记</param>
    /// <returns>方法信息</returns>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures")]
    protected static MethodInfo RegisterMethod(Expression<Action<T>> methodLambdaExpression, string friendlyName, string tag)
    {
      System.Reflection.MethodInfo reflectedMethodInfo = Phenix.Core.Reflection.Reflect<T>.GetMethod(methodLambdaExpression);
      return RegisterMethod(reflectedMethodInfo.Name, friendlyName, tag);
    }

    /// <summary>
    /// 过程可执行
    /// throwIfDeny = false
    /// </summary>
    public override bool CanExecuteMethod(Csla.Core.IMemberInfo method)
    {
      return CanExecuteMethod(method, false, null);
    }

    /// <summary>
    /// 过程可执行
    /// </summary>
    public new bool CanExecuteMethod(Csla.Core.IMemberInfo method, bool throwIfDeny)
    {
      return CanExecuteMethod(method, throwIfDeny, null);
    }

    /// <summary>
    /// 过程可执行
    /// throwIfDeny = false
    /// </summary>
    public bool CanExecuteMethod(Csla.Core.IMemberInfo method, params object[] arguments)
    {
      return CanExecuteMethod(method, false, arguments);
    }

    /// <summary>
    /// 过程可执行
    /// </summary>
    public bool CanExecuteMethod(Csla.Core.IMemberInfo method, bool throwIfDeny, params object[] arguments)
    {
      if (method == null)
        return true;
      return CanExecuteMethod(method.Name, throwIfDeny, arguments);
    }

    /// <summary>
    /// 过程可执行
    /// </summary>
    public virtual bool CanExecuteMethod(string methodName, bool throwIfDeny, params object[] arguments)
    {
      return AuthorizationRules.HasPermission(this, MethodAction.Execute, methodName, null, throwIfDeny, arguments);
    }

    /// <summary>
    /// 过程可执行
    /// throwIfDeny = false
    /// </summary>
    public override bool CanExecuteMethod(string methodName)
    {
      return CanExecuteMethod(methodName, false);
    }

    /// <summary>
    /// 过程可执行
    /// throwIfDeny = false
    /// </summary>
    /// <param name="methodName">过程名</param>
    /// <param name="arguments">参数</param>
    public bool CanExecuteMethod(string methodName, params object[] arguments)
    {
      return CanExecuteMethod(methodName, false, arguments);
    }

    /// <summary>
    /// 允许过程可执行
    /// 与Phenix.Services.Client.Security.ExecuteRule配套使用
    /// </summary>
    public virtual bool AllowExecuteMethod(string methodName, params object[] arguments)
    {
      return CanExecuteMethod(methodName, false, arguments);
    }
    
    #endregion

    #region ObjectCache

    internal static void AddCacheType(IRefinedly business)
    {
      Type type = business.GetType();
      if (!CacheTypes.ContainsKey(type.FullName))
        CacheTypes[type.FullName]= type;
      foreach (KeyValuePair<string, Type> kvp in business.GetCacheTypes())
        if (!CacheTypes.ContainsKey(kvp.Key))
          CacheTypes[kvp.Key] = kvp.Value;
    }

    internal static void DoRecordHasChanged()
    {
      ObjectCache.Clear(CacheTypes.Values);
    }

    /// <summary>
    /// 数据发生变更
    /// </summary>
    protected void RecordHasChanged()
    {
      DoRecordHasChanged();
    }

    #endregion

    #region Data Access

    #region OldFieldValues

    /// <summary>
    /// 初始化旧值
    /// must = false
    /// reset = false
    /// </summary>
    protected bool InitOldFieldValues()
    {
      return InitOldFieldValues(false, false);
    }

    /// <summary>
    /// 初始化旧值
    /// </summary>
    protected virtual bool InitOldFieldValues(bool must, bool reset)
    {
      bool result = false;
      if ((must || !IsNew) && (reset || _oldFieldValues == null))
      {
        result = true;
        _oldFieldValues = EntityHelper.GetFieldValues(this);
      }
      if (reset)
        _ignoreFillRequiredFieldValues = false;
      return result;
    }

    /// <summary>
    /// 取旧值
    /// </summary>
    protected FieldValue GetOldFieldValue(Phenix.Core.Mapping.IPropertyInfo propertyInfo)
    {
      return propertyInfo != null ? GetOldFieldValue(propertyInfo.Name) : null;
    }

    /// <summary>
    /// 取旧值
    /// </summary>
    protected FieldValue GetOldFieldValue(string propertyName)
    {
      if (!String.IsNullOrEmpty(propertyName))
        foreach (FieldValue item in OldFieldValues)
          if (String.CompareOrdinal(item.PropertyName, propertyName) == 0)
            return item;
      return null;
    }
    
    #endregion

    #region Value

    /// <summary>
    /// 取当前的属性值
    /// </summary>
    /// <param name="propertyInfo">属性信息</param>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters")]
    public object GetCurrentValue(Phenix.Core.Mapping.IPropertyInfo propertyInfo)
    {
      if (propertyInfo == null)
        throw new ArgumentNullException("propertyInfo");
      return propertyInfo.FieldMapInfo != null
        ? propertyInfo.FieldMapInfo.GetValue(this)
        : Phenix.Core.Reflection.Utilities.FindPropertyInfo(this.GetType(), propertyInfo.Name).GetValue(this, null);
    }

    /// <summary>
    /// 取最原始的属性值
    /// editLevel 小于等于 0
    /// </summary>
    /// <param name="propertyInfo">属性信息</param>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters")]
    public object GetOldValue(Phenix.Core.Mapping.IPropertyInfo propertyInfo)
    {
      if (propertyInfo == null)
        throw new ArgumentNullException("propertyInfo");
      if (IsNew)
        return null;
      FieldValue fieldValue = GetOldFieldValue(propertyInfo);
      if (fieldValue != null)
        return fieldValue.Value;
      return GetCurrentValue(propertyInfo);
    }

    /// <summary>
    /// 取编辑层级上的旧属性值
    /// </summary>
    /// <param name="propertyInfo">属性信息</param>
    /// <param name="editLevel">编辑层级, 小于等于0则等同于取最原始值</param>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters")]
    public object GetOldValue(Phenix.Core.Mapping.IPropertyInfo propertyInfo, int editLevel)
    {
      if (propertyInfo == null)
        throw new ArgumentNullException("propertyInfo");
      if (editLevel > EditLevel)
        throw new ArgumentOutOfRangeException("editLevel", editLevel, string.Format("EditLevel = {0}", EditLevel));
      if (editLevel <= 0)
        return GetOldValue(propertyInfo);
      if (propertyInfo.FieldMapInfo != null)
      {
        HybridDictionary state;
        using (MemoryStream memoryStream = new MemoryStream(__stateStack.ToArray()[EditLevel - editLevel]))
        {
          Csla.Serialization.ISerializationFormatter formatter = Csla.Serialization.SerializationFormatterFactory.GetFormatter();
          state = (HybridDictionary)formatter.Deserialize(memoryStream);
        }
        string key = String.Format("{0}.{1}", propertyInfo.FieldMapInfo.Field.DeclaringType.FullName, propertyInfo.FieldMapInfo.Field.Name);
        if (state.Contains(key))
          return state[key];
      }
      return GetCurrentValue(propertyInfo);
    }

    /// <summary>
    /// 设置强制按照Update方式提交
    /// </summary>
    /// <param name="propertyInfos">需提交的属性信息, 当为null、空队列时提交全部属性</param>
    public void SetForceUpdate(params Csla.Core.IPropertyInfo[] propertyInfos)
    {
      MarkOldClean();
      MarkDirty();
      if (propertyInfos == null || propertyInfos.Length == 0)
        propertyInfos = FieldManager.GetRegisteredProperties().ToArray();
      foreach (FieldValue fieldValue in OldFieldValues)
      {
        fieldValue.IsDirty = false;
        foreach (Csla.Core.IPropertyInfo propertyInfo in propertyInfos)
          if (String.CompareOrdinal(propertyInfo.Name, fieldValue.PropertyName) == 0)
          {
            fieldValue.IsDirty = true;
            break;
          }
      }
    }

    /// <summary>
    /// 增量导入
    /// </summary>
    public bool IncrImport(T source, params Csla.Core.IPropertyInfo[] propertyInfos)
    {
      bool result = false;
      if (propertyInfos == null || propertyInfos.Length == 0)
        propertyInfos = FieldManager.GetRegisteredProperties().ToArray();
      foreach (FieldValue fieldValue in OldFieldValues)
      {
        fieldValue.IsDirty = false;
        FieldMapInfo fieldMapInfo = fieldValue.GetFieldMapInfo();
        if (fieldMapInfo.IsWatermarkColumn)
          continue;
        foreach (Csla.Core.IPropertyInfo propertyInfo in propertyInfos)
          if (String.CompareOrdinal(propertyInfo.Name, fieldValue.PropertyName) == 0)
          {
            object newValue = fieldMapInfo.GetValue(source);
            if (!object.Equals(newValue, fieldValue.Value))
            {
              result = true;
              fieldMapInfo.SetValue(this, newValue);
              fieldValue.IsDirty = true;
              _propertyValueChanged = true;
            }
            break;
          }
      }
      if (result)
        MarkDirty();
      return result;
    }

    #endregion

    #region DefaultValue

    private object DoGetDefaultValue(Func<T, object> func)
    {
      object result = func((T)this);
      if (result is Phenix.Core.Mapping.IPropertyInfo)
        result = ((Phenix.Core.Mapping.IPropertyInfo)result).DefaultValue;
      return result;
    }

    private void FillRequiredFieldValues()
    {
      if (!_ignoreFillRequiredFieldValues)
      {
        _ignoreFillRequiredFieldValues = true;
        EntityHelper.FillRequiredFieldValues(this, IsNew);
      }
    }

    /// <summary>
    /// 填充字段值到缺省值
    /// reset = true
    /// </summary>
    public bool FillFieldValuesToDefault()
    {
      return FillFieldValuesToDefault(true);
    }

    /// <summary>
    /// 填充字段值到缺省值
    /// </summary>
    /// <param name="reset">重新设定</param>
    public virtual bool FillFieldValuesToDefault(bool reset)
    {
      InitOldFieldValues(false, false);

      bool result = EntityHelper.FillFieldValuesToDefault(this, IsNew, reset);
      if (_defaultFieldValues.Count > 0)
        if (EntityHelper.FillFieldValues(new List<FieldValue>(_defaultFieldValues.Values), this, reset))
          result = true;
      if (_defaultValueFuncs.Count > 0)
        foreach (KeyValuePair<string, Func<T, object>> kvp in _defaultValueFuncs)
        {
          FieldMapInfo fieldMapInfo = GetFieldMapInfo(kvp.Key);
          object oldValue = fieldMapInfo.GetValue(this);
          if (reset || oldValue == null)
          {
            object newValue = Phenix.Core.Reflection.Utilities.ChangeType(DoGetDefaultValue(kvp.Value), fieldMapInfo.Field.FieldType);
            if (!object.Equals(oldValue, newValue))
            {
              fieldMapInfo.SetValue(this, newValue);
              result = true;
            }
          }
        }

      if (result)
        MarkDirty();
      return result;
    }

    /// <summary>
    /// 设置缺省值
    /// allowReplace = false
    /// </summary>
    /// <param name="property">属性信息</param>
    /// <param name="value">值</param>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1000:DoNotDeclareStaticMembersOnGenericTypes")]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters")]
    public static void SetDefaultValue(Csla.Core.IPropertyInfo property, object value)
    {
      SetDefaultValue(property, value, false);
    }

    /// <summary>
    /// 设置缺省值
    /// </summary>
    /// <param name="property">属性信息</param>
    /// <param name="value">值</param>
    /// <param name="allowReplace">如果为 true, 则当属性被赋值时允许赋值的内容覆盖本缺省值</param>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1000:DoNotDeclareStaticMembersOnGenericTypes")]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters")]
    public static void SetDefaultValue(Csla.Core.IPropertyInfo property, object value, bool allowReplace)
    {
      if (property == null)
        throw new ArgumentNullException("property");
      DoSetDefaultValue(GetFieldMapInfo(property.Name), value, allowReplace, false);
    }

    internal static void DoSetDefaultValue(FieldMapInfo fieldMapInfo, object value, bool allowReplace, bool isReplace)
    {
      if (fieldMapInfo == null)
        return;
      if (!isReplace || _defaultFieldValues.ContainsKey(fieldMapInfo.PropertyName))
      {
        FieldValue fieldValue;
        if (!_defaultFieldValues.TryGetValue(fieldMapInfo.PropertyName, out fieldValue) ||
          (isReplace && fieldValue.IsDirty.HasValue && fieldValue.IsDirty.Value))
          fieldValue = new FieldValue(fieldMapInfo, value);
        if (!isReplace && allowReplace)
          fieldValue.IsDirty = true;
        _defaultFieldValues[fieldMapInfo.PropertyName] = fieldValue;
      }
    }

    /// <summary>
    /// 设置缺省值
    /// </summary>
    /// <param name="property">属性信息</param>
    /// <param name="valueFunc">值函数</param>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1000:DoNotDeclareStaticMembersOnGenericTypes")]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters")]
    public static void SetDefaultValue(Csla.Core.IPropertyInfo property, Func<T, object> valueFunc)
    {
      if (property == null)
        throw new ArgumentNullException("property");
      DoSetDefaultValue(GetFieldMapInfo(property.Name), valueFunc);
    }

    internal static void DoSetDefaultValue(FieldMapInfo fieldMapInfo, Func<T, object> valueFunc)
    {
      if (fieldMapInfo == null)
        return;
      _defaultValueFuncs[fieldMapInfo.PropertyName] = valueFunc;
    }

    /// <summary>
    /// 移除缺省值
    /// </summary>
    /// <param name="property">属性信息</param>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1000:DoNotDeclareStaticMembersOnGenericTypes")]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters")]
    public static bool RemoveDefaultValue(Csla.Core.IPropertyInfo property)
    {
      if (property == null)
        throw new ArgumentNullException("property");
      bool result = _defaultFieldValues.Remove(property.Name);
      if (_defaultValueFuncs.Remove(property.Name))
        result = true;
      return result;
    }

    /// <summary>
    /// 获取缺省值
    /// onlyDynamic = true
    /// </summary>
    /// <param name="property">属性信息</param>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1000:DoNotDeclareStaticMembersOnGenericTypes")]
    public object GetDefaultValue(Csla.Core.IPropertyInfo property)
    {
      return GetDefaultValue(property, true);
    }

    /// <summary>
    /// 获取缺省值
    /// </summary>
    /// <param name="property">属性信息</param>
    /// <param name="onlyDynamic">如果为 true, 则仅返回通过SetDefaultValue()设置的缺省值; 否则返回完整版</param>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1000:DoNotDeclareStaticMembersOnGenericTypes")]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1716:IdentifiersShouldNotMatchKeywords")]
    public virtual object GetDefaultValue(Csla.Core.IPropertyInfo property, bool onlyDynamic)
    {
      if (property == null)
        throw new ArgumentNullException("property");
      if (_defaultFieldValues.Count > 0)
      {
        FieldValue fieldValue;
        if (_defaultFieldValues.TryGetValue(property.Name, out fieldValue))
          return fieldValue.Value;
      }
      if (_defaultValueFuncs.Count > 0)
      {
        Func<T, object> valueFunc;
        if (_defaultValueFuncs.TryGetValue(property.Name, out valueFunc))
          return DoGetDefaultValue(valueFunc);
      }
      if (!onlyDynamic)
      {
        FieldMapInfo fieldMapInfo = GetFieldMapInfo(property);
        if (fieldMapInfo != null)
          return fieldMapInfo.DefaultValue;
      }
      return null;
    }

    #endregion

    #endregion

    #region Permanent Log

    /// <summary>
    /// 持久化日志
    /// </summary>
    /// <param name="transaction">数据库事务</param>
    /// <param name="action">执行动作</param>
    protected void PermanentLog(DbTransaction transaction, ExecuteAction action)
    {
      ClassMapInfo classMapInfo = ClassMemberHelper.GetClassMapInfo(this.GetType());
      if (classMapInfo.NeedPermanentRenovate)
        PermanentLogHub.SaveRenovate(transaction, classMapInfo.TableName, action, action == ExecuteAction.Insert ? EntityHelper.GetFieldValues(this) : OldFieldValues);
      if ((classMapInfo.PermanentExecuteAction & action) == action)
        PermanentLogHub.SaveExecuteAction(this.GetType(), Caption, PrimaryKey, action, OldFieldValues, EntityHelper.GetFieldValues(this));
    }

    /// <summary>
    /// 检索执行动作
    /// </summary>
    /// <returns>执行动作信息队列</returns>
    public IList<ExecuteActionInfo> FetchExecuteAction()
    {
      ClassMapInfo classMapInfo = ClassMemberHelper.GetClassMapInfo(this.GetType());
      if (classMapInfo.PermanentExecuteAction != ExecuteAction.None)
        return PermanentLogHub.FetchExecuteAction(this.GetType(), PrimaryKey);
     return null;
    }

    /// <summary>
    /// 检索执行动作
    /// userNumber = null
    /// </summary>
    /// <param name="action">执行动作</param>
    /// <param name="startTime">起始时间</param>
    /// <param name="finishTime">结束时间</param>
    /// <returns>执行动作信息队列</returns>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1000:DoNotDeclareStaticMembersOnGenericTypes")]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures")]
    public static IList<ExecuteActionInfo> FetchExecuteAction(ExecuteAction action,
      DateTime startTime, DateTime finishTime)
    {
      return PermanentLogHub.FetchExecuteAction(null, typeof(T), action, startTime, finishTime);
    }

    /// <summary>
    /// 检索执行动作
    /// </summary>
    /// <param name="userNumber">登录工号, null代表全部</param>
    /// <param name="action">执行动作</param>
    /// <param name="startTime">起始时间</param>
    /// <param name="finishTime">结束时间</param>
    /// <returns>执行动作信息队列</returns>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1000:DoNotDeclareStaticMembersOnGenericTypes")]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures")]
    public static IList<ExecuteActionInfo> FetchExecuteAction(string userNumber, ExecuteAction action,
      DateTime startTime, DateTime finishTime)
    {
      return PermanentLogHub.FetchExecuteAction(userNumber, typeof(T), action, startTime, finishTime);
    }

    /// <summary>
    /// 清除执行动作
    /// userNumber = null
    /// </summary>
    /// <param name="action">执行动作</param>
    /// <param name="startTime">起始时间</param>
    /// <param name="finishTime">结束时间</param>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1000:DoNotDeclareStaticMembersOnGenericTypes")]
    public static void ClearExecuteAction(ExecuteAction action,
      DateTime startTime, DateTime finishTime)
    {
      PermanentLogHub.ClearExecuteAction(null, typeof(T), action, startTime, finishTime);
    }

    /// <summary>
    /// 清除执行动作
    /// </summary>
    /// <param name="userNumber">登录工号, null代表全部</param>
    /// <param name="action">执行动作</param>
    /// <param name="startTime">起始时间</param>
    /// <param name="finishTime">结束时间</param>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1000:DoNotDeclareStaticMembersOnGenericTypes")]
    public static void ClearExecuteAction(string userNumber, ExecuteAction action,
      DateTime startTime, DateTime finishTime)
    {
      PermanentLogHub.ClearExecuteAction(userNumber, typeof(T), action, startTime, finishTime);
    }

    #endregion
    
    #region Validation Rules

    #region IDataSeverityInfo 成员

    /// <summary>
    /// 取错误信息
    /// </summary>
    /// <param name="propertyInfo">属性信息</param>
    /// <returns>信息</returns>
    public string GetFirstErrorMessage(Phenix.Core.Mapping.IPropertyInfo propertyInfo)
    {
      return GetFirstErrorMessage(propertyInfo.Name);
    }

    /// <summary>
    /// 取错误信息
    /// </summary>
    /// <param name="propertyName">属性名</param>
    /// <returns>信息</returns>
    public string GetFirstErrorMessage(string propertyName)
    {
      Csla.Rules.BrokenRule brokenRule = BrokenRulesCollection.GetFirstMessage(propertyName, Csla.Rules.RuleSeverity.Error);
      return brokenRule != null ? brokenRule.Description : null;
    }

    /// <summary>
    /// 取警告信息
    /// </summary>
    /// <param name="propertyInfo">属性信息</param>
    /// <returns>信息</returns>
    public string GetFirstWarningMessage(Phenix.Core.Mapping.IPropertyInfo propertyInfo)
    {
      return GetFirstWarningMessage(propertyInfo.Name);
    }

    /// <summary>
    /// 取警告信息
    /// </summary>
    /// <param name="propertyName">属性名</param>
    /// <returns>信息</returns>
    public string GetFirstWarningMessage(string propertyName)
    {
      Csla.Rules.BrokenRule brokenRule = BrokenRulesCollection.GetFirstMessage(propertyName, Csla.Rules.RuleSeverity.Warning);
      return brokenRule != null ? brokenRule.Description : null;
    }

    /// <summary>
    /// 取消息信息
    /// </summary>
    /// <param name="propertyInfo">属性信息</param>
    /// <returns>信息</returns>
    public string GetFirstInformationMessage(Phenix.Core.Mapping.IPropertyInfo propertyInfo)
    {
      return GetFirstInformationMessage(propertyInfo.Name);
    }

    /// <summary>
    /// 取消息信息
    /// </summary>
    /// <param name="propertyName">属性名</param>
    /// <returns>信息</returns>
    public string GetFirstInformationMessage(string propertyName)
    {
      Csla.Rules.BrokenRule brokenRule = BrokenRulesCollection.GetFirstMessage(propertyName, Csla.Rules.RuleSeverity.Information);
      return brokenRule != null ? brokenRule.Description : null;
    }

    #endregion

    /// <summary>
    /// 校验数据是否有效
    /// </summary>
    /// <param name="property">属性信息</param>
    /// <returns>错误信息</returns>
    public string CheckRule(Csla.Core.IPropertyInfo property)
    {
      if (property == null)
        throw new ArgumentNullException("property");
      if (IsSelfDirty)
      {
        BusinessRules.CheckRules(property);
        return ((IDataErrorInfo)this)[property.Name];
      }
      return null;
    }

    /// <summary>
    /// 校验数据是否有效
    /// </summary>
    /// <param name="propertyName">属性名</param>
    /// <returns>错误信息</returns>
    public string CheckRule(string propertyName)
    {
      Csla.Core.IPropertyInfo propertyInfo;
      if (FindPropertyInfo(propertyName, out propertyInfo))
        return CheckRule(propertyInfo);
      return null;
    }
    
    /// <summary>
    /// 添加业务规则
    /// </summary>
    protected override void AddBusinessRules()
    {
      RegisterBusinessRules();
      OnBusinessRuleRegistering(BusinessRules);
    }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
    private void RegisterBusinessRules()
    {
      foreach (FieldMapInfo item in ClassMemberHelper.GetFieldMapInfos(this.GetType()))
      {
        if (!item.InCommand || !item.InValidation)
          continue;

        Csla.Core.IPropertyInfo primaryProperty;
        if (FindPropertyInfo(item.PropertyName, out primaryProperty))
        {
          Phenix.Core.Mapping.IPropertyInfo propertyInfo = primaryProperty as Phenix.Core.Mapping.IPropertyInfo;
          if (propertyInfo != null)
          {
            //Required规则
            if (item.IsRequired)
              BusinessRules.AddRule(new RequiredRule(propertyInfo));
            //字符串
            if (item.Field.FieldType == typeof(string))
            {
              //MaxLength规则
              if (item.MaximumLength > 0)
                BusinessRules.AddRule(new MaxLengthRule(propertyInfo, item.MaximumLength));
              //MinLength规则
              if (item.MinimumLength > 0)
                BusinessRules.AddRule(new MinLengthRule(propertyInfo, item.MinimumLength));
            }
          }
          else
          {
            ////Required规则
            //if (item.IsRequired)
            //  BusinessRules.AddRule(new Csla.Rules.CommonRules.Required(primaryProperty));
            ////字符串
            //if (item.Field.FieldType == typeof(string))
            //{
            //  //MaxLength规则
            //  if (item.MaximumLength > 0)
            //    BusinessRules.AddRule(new Csla.Rules.CommonRules.MaxLength(primaryProperty, item.MaximumLength));
            //  //MinLength规则
            //  if (item.MinimumLength > 0)
            //    BusinessRules.AddRule(new Csla.Rules.CommonRules.MinLength(primaryProperty, item.MinimumLength));
            //}
            ////是否可用
            //if (!item.Enabled)
            //  BusinessRules.AddRule(new Csla.Rules.CommonRules.Lambda(primaryProperty,
            //    delegate(Csla.Rules.RuleContext context)
            //    {
            //      context.AddErrorResult(String.Format(Properties.Resources.DisabledRule,
            //        ((IEntity)context.Target).Caption, context.Rule.PrimaryProperty.FriendlyName));
            //    }));
            //if (item.FieldUnderlyingType.IsEnum)
            //  BusinessRules.AddRule(new Csla.Rules.CommonRules.Lambda(primaryProperty,
            //    delegate(Csla.Rules.RuleContext context)
            //    {
            //      System.Reflection.PropertyInfo propertyInfo = ClassMemberHelper.FindPropertyInfo(context.Target.GetType(), context.Rule.PrimaryProperty.Name);
            //      if (propertyInfo != null && propertyInfo.GetGetMethod(true) != null)
            //      {
            //        object value = propertyInfo.GetValue(context.Target, null);
            //        if (value == null)
            //          return;
            //        foreach (EnumKeyCaption enumKeyCaption in EnumKeyCaptionCollection.Fetch(Phenix.Core.Reflection.Utilities.GetUnderlyingType(context.Rule.PrimaryProperty.Type)))
            //          if (enumKeyCaption.Value.Equals(value))
            //          {
            //            if (!enumKeyCaption.Enabled)
            //              context.AddErrorResult(String.Format(Properties.Resources.DisabledRule, enumKeyCaption.Caption));
            //            break;
            //          }
            //      }
            //    }));
          }
        }
      }
    }

    #endregion

    #region Authorization Rules

    /// <summary>
    /// 添加授权规则
    /// </summary>
    protected virtual void AddAuthorizationRules()
    {
      OnAuthorizationRuleRegistering(AuthorizationRules);
    }
    void IAuthorizationObject.AddAuthorizationRules()
    {
      AddAuthorizationRules();
    }

    #endregion
    
    #endregion
  }
}