using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.Serialization;
using Phenix.Business.Rules;
using Phenix.Core.Cache;
using Phenix.Core.Data;
using Phenix.Core.Dictionary;
using Phenix.Core.Log;
using Phenix.Core.Mapping;
using Phenix.Core.Reflection;

namespace Phenix.Business.Core
{
  /// <summary>
  /// 业务集合基类
  /// </summary>
  [Serializable]
  [DataDictionary(AssemblyClassType.Businesses)]
  public abstract class BusinessListBase<T, TBusiness> : Csla.BusinessBindingListBase<T, TBusiness>, 
    IAuthorizationObject, IFactory, IEntityCollection
    where T : BusinessListBase<T, TBusiness>
    where TBusiness : BusinessBase<TBusiness>
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

    #region 属性

    /// <summary>
    /// 数据源键
    /// 缺省为 T、TBusiness 上的 ClassAttribute.DataSourceKey
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
    ICriterions IEntityCollection.Criterions
    {
      get { return Criterions; }
    }

    /// <summary>
    /// 友好名
    /// 缺省为 TBusiness 上的 ClassAttribute.FriendlyName
    /// 用于提示信息等
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1000:DoNotDeclareStaticMembersOnGenericTypes")]
    public static string FriendlyName
    {
      get { return ClassMemberHelper.GetFriendlyName(typeof(T)); }
    }

    /// <summary>
    /// 标签
    /// 缺省为 TBusiness 上的 ClassAttribute.FriendlyName
    /// 用于提示信息等
    /// </summary>
    [Newtonsoft.Json.JsonIgnore]
    public virtual string Caption
    {
      get { return FriendlyName; }
    }

    /// <summary>
    /// 业务对象类型
    /// </summary>
    protected Type ItemValueType
    {
      get { return typeof(TBusiness); }
    }
    Type IEntityCollection.ItemValueType
    {
      get { return ItemValueType; }
    }

    [NonSerialized]
    [Csla.NotUndoable]
    private bool _selfFetching;
    /// <summary>
    /// 正在检索中
    /// </summary>
    [Newtonsoft.Json.JsonIgnore]
    [System.ComponentModel.Browsable(false)]
    [System.ComponentModel.DataAnnotations.Display(AutoGenerateField = false)]
    public bool SelfFetching
    {
      get { return _selfFetching; }
      protected set { _selfFetching = value; }
    }
     bool IEntityCollection.SelfFetching
    {
      get { return SelfFetching; }
      set { SelfFetching = value; }
    }

    /// <summary>
    /// 是否空集合
    /// </summary>
    [Newtonsoft.Json.JsonIgnore]
    [System.ComponentModel.Browsable(false)]
    [System.ComponentModel.DataAnnotations.Display(AutoGenerateField = false)]
    public bool IsEmpty
    {
      get { return Items.Count == 0/* && DeletedList.Count == 0*/; }
    }

    /// <summary>
    /// 是否空集合
    /// </summary>
    [Newtonsoft.Json.JsonIgnore]
    [System.ComponentModel.Browsable(false)]
    [System.ComponentModel.DataAnnotations.Display(AutoGenerateField = false)]
    public BooleanOption IsEmptyOption
    {
      get { return IsEmpty ? BooleanOption.Y : BooleanOption.N; }
    }
    
    #region Authorization Rules

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

    /// <summary>
    /// 是否只读
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1000:DoNotDeclareStaticMembersOnGenericTypes")]
    public static bool IsReadOnly
    {
      get { return BusinessBase<TBusiness>.IsReadOnly; }
    }

    /// <summary>
    /// 是否允许添加业务对象
    /// </summary>
    [System.ComponentModel.Browsable(false)]
    [System.ComponentModel.DataAnnotations.Display(AutoGenerateField = false)]
    public virtual bool AllowAddItem
    {
      get { return BusinessBase<TBusiness>.CanCreate; }
    }
    /// <summary>
    /// 是否允许添加业务对象
    /// </summary>
    [Newtonsoft.Json.JsonIgnore]
    [System.ComponentModel.Browsable(false)]
    [System.ComponentModel.DataAnnotations.Display(AutoGenerateField = false)]
    public BooleanOption AllowAddItemOption
    {
      get { return AllowAddItem ? BooleanOption.Y : BooleanOption.N; }
    }

    /// <summary>
    /// 是否允许编辑业务对象
    /// </summary>
    [System.ComponentModel.Browsable(false)]
    [System.ComponentModel.DataAnnotations.Display(AutoGenerateField = false)]
    public virtual bool AllowEditItem
    {
      get { return BusinessBase<TBusiness>.CanEdit; }
    }
    /// <summary>
    /// 是否允许编辑业务对象
    /// </summary>
    [Newtonsoft.Json.JsonIgnore]
    [System.ComponentModel.Browsable(false)]
    [System.ComponentModel.DataAnnotations.Display(AutoGenerateField = false)]
    public BooleanOption AllowEditItemOption
    {
      get { return AllowEditItem ? BooleanOption.Y : BooleanOption.N; }
    }

    /// <summary>
    /// 是否允许删除业务对象
    /// </summary>
    [System.ComponentModel.Browsable(false)]
    [System.ComponentModel.DataAnnotations.Display(AutoGenerateField = false)]
    public virtual bool AllowDeleteItem
    {
      get { return BusinessBase<TBusiness>.CanDelete; }
    }
    /// <summary>
    /// 是否允许删除业务对象
    /// </summary>
    [Newtonsoft.Json.JsonIgnore]
    [System.ComponentModel.Browsable(false)]
    [System.ComponentModel.DataAnnotations.Display(AutoGenerateField = false)]
    public BooleanOption AllowDeleteItemOption
    {
      get { return AllowDeleteItem ? BooleanOption.Y : BooleanOption.N; }
    }

    private static DynamicMemberGetDelegate __itemsFieldGetValue;
    private static DynamicMemberSetDelegate __itemsFieldSetValue;
    private IList<TBusiness> __items
    {
      get
      {
        if (__itemsFieldGetValue == null)
          __itemsFieldGetValue = DynamicFactory.CreateFieldGetter(typeof(Collection<TBusiness>).GetField("items", BindingFlags.NonPublic | BindingFlags.Instance));
        return (IList<TBusiness>)__itemsFieldGetValue(this);
      }
      set
      {
        if (__itemsFieldSetValue == null)
          __itemsFieldSetValue = DynamicFactory.CreateFieldSetter(typeof(Collection<TBusiness>).GetField("items", BindingFlags.NonPublic | BindingFlags.Instance));
        __itemsFieldSetValue(this, value);
      }
    }

    #endregion

    #endregion

    #region 事件

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
    /// 批量添加item（非新增对象）
    /// </summary>
    /// <param name="source">数据源</param>
    public virtual void FillRange(IEnumerable<TBusiness> source)
    {
      bool oldRaiseListChangedEvents = RaiseListChangedEvents;
      try
      {
        RaiseListChangedEvents = false;
        AddRange(source);
      }
      finally
      {
        RaiseListChangedEvents = oldRaiseListChangedEvents;
        if (RaiseListChangedEvents)
          OnListChanged(new ListChangedEventArgs(ListChangedType.Reset, -1));
      }
    }

    internal void __NewItems()
    {
      __items = new List<TBusiness>();
    }

    internal void __ClearItems()
    {
      __items.Clear();
    }

    #region ObjectCache

    private static void DoRecordHasChanged()
    {
      BusinessBase<TBusiness>.DoRecordHasChanged();
      ObjectCache.Clear(typeof(T));
    }

    /// <summary>
    /// 数据发生变更
    /// </summary>
    protected void RecordHasChanged()
    {
      DoRecordHasChanged();
    }

    #endregion

    #region Method

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
    protected static MethodInfo RegisterMethod(Expression<Action<T>> methodLambdaExpression)
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
    public bool CanExecuteMethod(Csla.Core.IMemberInfo method)
    {
      return CanExecuteMethod(method, false, null);
    }

    /// <summary>
    /// 过程可执行
    /// </summary>
    public bool CanExecuteMethod(Csla.Core.IMemberInfo method, bool throwIfDeny)
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
    public bool CanExecuteMethod(string methodName)
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