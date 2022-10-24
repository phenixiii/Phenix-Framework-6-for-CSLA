using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq.Expressions;
using System.Reflection;
using Phenix.Core.Dictionary;
using Phenix.Core.Mapping;
using Phenix.Core.Reflection;

namespace Phenix.Core.Operate
{
  /// <summary>
  /// 枚举"键-标签"
  /// 主要用于填充下拉列表框内容
  /// </summary>
  [Serializable]
  public sealed class KeyCaption : KeyCaption<KeyCaption, object>
  {
    /// <summary>
    /// 初始化
    /// </summary>
    /// <param name="key">键</param>
    /// <param name="caption">标签</param>
    /// <param name="value">值</param>
    public KeyCaption(object key, string caption, object value)
      : base(key, caption, value) { }

    #region 属性

    /// <summary>
    /// 值类型
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1000:DoNotDeclareStaticMembersOnGenericTypes")]
    public new static Type ValueType
    {
      get { return null; }
    }

    #endregion
  }

  /// <summary>
  /// "键-标签"
  /// 主要用于填充下拉列表框内容
  /// </summary>
  [Serializable]
  [ClassAttribute(null)]
  public abstract class KeyCaption<T, TValue> : IKeyCaption, INotifyPropertyChanged
    where T : KeyCaption<T, TValue>
  {
    /// <summary>
    /// 初始化
    /// </summary>
    /// <param name="key">键</param>
    /// <param name="caption">标签</param>
    /// <param name="value">值</param>
    protected KeyCaption(object key, string caption, TValue value)
    {
      _key = key;
      _caption = caption;
      _value = value;
    }

    #region 工厂

    /// <summary>
    /// 构建自己
    /// </summary>
    protected virtual bool FetchSelf(IDataRecord sourceFieldValues, IList<FieldMapInfo> fieldMapInfos)
    {
      throw new NotImplementedException(MethodBase.GetCurrentMethod().Name);
    }
    [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
    bool IEntity.FetchSelf(IDataRecord sourceFieldValues, IList<FieldMapInfo> fieldMapInfos)
    {
      return FetchSelf(sourceFieldValues, fieldMapInfos);
    }

    #endregion

    #region 属性

    /// <summary>
    /// 值类型
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1000:DoNotDeclareStaticMembersOnGenericTypes")]
    public static Type ValueType
    {
      get { return typeof(TValue); }
    }

    private static AssemblyClassInfo _valueClassInfo;
    private static AssemblyClassInfo ValueClassInfo
    {
      get
      {
        if (_valueClassInfo == null)
          _valueClassInfo = DataDictionaryHub.GetClassInfo(ValueType);
        return _valueClassInfo;
      }
    }

    private static bool _keyCaptionAttributeChecked;
    private static KeyCaptionAttribute _keyCaptionAttribute;
    private static KeyCaptionAttribute KeyCaptionAttribute
    {
      get
      {
        if (!_keyCaptionAttributeChecked)
        {
          _keyCaptionAttribute = (KeyCaptionAttribute)Attribute.GetCustomAttribute(ValueType, typeof(KeyCaptionAttribute));
          _keyCaptionAttributeChecked = true;
        }
        return _keyCaptionAttribute;
      }
    }

    /// <summary>
    /// 键
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1000:DoNotDeclareStaticMembersOnGenericTypes")]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
    public static readonly PropertyInfo<object> KeyProperty = 
      RegisterProperty(c => c.Key, Phenix.Core.Properties.Resources.KeyFriendlyName);
    [Field(NoMapping = true, InAuthorization = false)]
    private readonly object _key;
    /// <summary>
    /// 键
    /// </summary>
    public object Key
    {
      get { return _key; }
    }

    /// <summary>
    /// 标签
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1000:DoNotDeclareStaticMembersOnGenericTypes")]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
    public static readonly PropertyInfo<string> CaptionProperty = 
      RegisterProperty(c => c.Caption, 
      ValueClassInfo != null && ValueClassInfo.CaptionConfigured && !String.IsNullOrEmpty(ValueClassInfo.Caption) ? ValueClassInfo.Caption :
      KeyCaptionAttribute != null && KeyCaptionAttribute.FriendlyName != null ? KeyCaptionAttribute.FriendlyName : Phenix.Core.Properties.Resources.CaptionFriendlyName);
    [Field(NoMapping = true, InAuthorization = false)]
    private string _caption;
    /// <summary>
    /// 标签
    /// </summary>
    public virtual string Caption
    {
      get { return _caption; }
      set
      {
        if (_caption == value)
          return;
        _caption = value;
        PropertyHasChanged();
      }
    }

    /// <summary>
    /// 主键
    /// </summary>
    [System.ComponentModel.Browsable(false)]
    [System.ComponentModel.DataAnnotations.Display(AutoGenerateField = false)]
    public string PrimaryKey
    {
      get { return Key.ToString(); }
    }

    /// <summary>
    /// 数据源键
    /// </summary>
    protected string DataSourceKey
    {
      get { return String.Empty; }
    }
    string IEntity.DataSourceKey
    {
      get { return DataSourceKey; }
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
    /// 旧值
    /// </summary>
    [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
    protected virtual IList<FieldValue> OldFieldValues
    {
      get { return null; }
    }
    [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
    IList<FieldValue> IEntity.OldFieldValues
    {
      get { return OldFieldValues; }
    }

    /// <summary>
    /// 删除即禁用
    /// </summary>
    protected virtual bool DeletedAsDisabled
    {
       get { return true; }
    }
    bool IEntity.DeletedAsDisabled
    {
      get{ return DeletedAsDisabled; }
    }

    private bool _isDisabled;
    /// <summary>
    /// 是否已禁用
    /// </summary>
    protected virtual bool IsDisabled
    {
      get { return _isDisabled; }
      set { _isDisabled = value; }
    }
    bool IEntity.IsDisabled
    {
      get { return IsDisabled; }
      set { IsDisabled = value; }
    }

    /// <summary>
    /// 校验数据库数据在下载到提交期间是否被更改过
    /// </summary>
    protected virtual bool NeedCheckDirty
    {
      get { return false; }
    }
    bool IEntity.NeedCheckDirty
    {
      get { return NeedCheckDirty; }
    }

    /// <summary>
    /// 新增状态
    /// </summary>
    protected virtual bool IsNew
    {
      get { return false; }
    }
    bool IEntity.IsNew
    {
      get { return IsNew; }
    }

    /// <summary>
    /// 删除状态
    /// </summary>
    protected virtual bool IsSelfDeleted
    {
      get { return IsDisabled; }
    }
    bool IEntity.IsSelfDeleted
    {
      get { return IsSelfDeleted; }
    }

    /// <summary>
    /// 更新状态
    /// </summary>
    protected virtual bool IsSelfDirty
    {
      get { return false; }
    }
    bool IEntity.IsSelfDirty
    {
      get { return IsSelfDirty; }
    }

    /// <summary>
    /// 值
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1000:DoNotDeclareStaticMembersOnGenericTypes")]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
    public static readonly PropertyInfo<TValue> ValueProperty = 
      RegisterProperty(c => c.Value, Phenix.Core.Properties.Resources.ValueFriendlyName);
    [Field(NoMapping = true, InAuthorization = false)]
    private TValue _value;
    /// <summary>
    /// 值
    /// </summary>
    public TValue Value
    {
      get { return _value; }
      set 
      {
        _value = value;
        PropertyHasChanged();
      }
    }
    object IKeyCaption.Value
    {
      get { return Value; }
    }

    /// <summary>
    /// 包含关联数据的对象
    /// </summary>
    [System.ComponentModel.Browsable(false)]
    [System.ComponentModel.DataAnnotations.Display(AutoGenerateField = false)]
    public object Tag { get; set; }

    #region ISelectable 成员

    /// <summary>
    /// 所属对象勾选集合
    /// </summary>
    [System.ComponentModel.Browsable(false)]
    [System.ComponentModel.DataAnnotations.Display(AutoGenerateField = false)]
    public ISelectableCollection Owner { get; internal set; }

    /// <summary>
    /// 是否被选择
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1000:DoNotDeclareStaticMembersOnGenericTypes")]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
    public static readonly PropertyInfo<bool> SelectedProperty = 
      RegisterProperty<bool>(c => c.Selected, Phenix.Core.Properties.Resources.SelectedFriendlyName, true);
    [Field(NoMapping = true, InAuthorization = false)]
    private bool _selected;
    /// <summary>
    /// 是否被勾选
    /// 用于标记本对象
    /// 缺省为 false
    /// </summary>
    public bool Selected
    {
      get { return _selected; }
      set
      {
        bool changed = _selected != value;
        if (changed)
          if (!OnSelectedValueChanging())
            return;
        _selected = value;
        if (Owner != null)
          if (value)
          {
            if (!Owner.SelectedItems.Contains(this))
              Owner.SelectedItems.Add(this);
          }
          else
            Owner.SelectedItems.Remove(this);
        if (changed)
        {
          OnSelectedValueChanged();
          PropertyHasChanged();
        }
      }
    }

    #endregion

    #endregion

    #region 事件

    #region ISelectable 成员

    [NonSerialized]
    private EventHandler<SelectedValueChangingEventArgs> _selectedValueChangingHandlers;
    /// <summary>
    /// Selected属性值被更改前
    /// </summary>
    public event EventHandler<SelectedValueChangingEventArgs> SelectedValueChanging
    {
      add { _selectedValueChangingHandlers = (EventHandler<SelectedValueChangingEventArgs>)Delegate.Combine(_selectedValueChangingHandlers, value); }
      remove { _selectedValueChangingHandlers = (EventHandler<SelectedValueChangingEventArgs>)Delegate.Remove(_selectedValueChangingHandlers, value); }
    }

    /// <summary>
    /// Selected属性值被更改前
    /// </summary>
    protected virtual bool OnSelectedValueChanging()
    {
      if (_selectedValueChangingHandlers != null)
      {
        SelectedValueChangingEventArgs e = new SelectedValueChangingEventArgs(this);
        _selectedValueChangingHandlers.Invoke(this, e);
        if (e.Stop)
          return false;
      }
      return true;
    }

    [NonSerialized]
    private EventHandler<SelectedValueChangedEventArgs> _selectedValueChangedHandlers;
    /// <summary>
    /// Selected属性值被更改后
    /// </summary>
    public event EventHandler<SelectedValueChangedEventArgs> SelectedValueChanged
    {
      add { _selectedValueChangedHandlers = (EventHandler<SelectedValueChangedEventArgs>)Delegate.Combine(_selectedValueChangedHandlers, value); }
      remove { _selectedValueChangedHandlers = (EventHandler<SelectedValueChangedEventArgs>)Delegate.Remove(_selectedValueChangedHandlers, value); }
    }

    /// <summary>
    /// Selected属性值被更改后
    /// </summary>
    protected virtual void OnSelectedValueChanged()
    {
      if (_selectedValueChangedHandlers != null)
        _selectedValueChangedHandlers.Invoke(this, new SelectedValueChangedEventArgs(this));
    }

    #endregion

    #region INotifyPropertyChanged 成员

    [NonSerialized]
    private PropertyChangedEventHandler _propertyChanged;
    /// <summary>
    /// 在更改属性值时发生
    /// </summary>
    public event PropertyChangedEventHandler PropertyChanged
    {
      add { _propertyChanged = (PropertyChangedEventHandler)Delegate.Combine(_propertyChanged, value); }
      remove { _propertyChanged = (PropertyChangedEventHandler)Delegate.Remove(_propertyChanged, value); }
    }

    private void OnPropertyChanged(string propertyName)
    {
      if (_propertyChanged != null)
        _propertyChanged(this, new PropertyChangedEventArgs(propertyName));
    }

    /// <summary>
    /// 属性已更改
    /// </summary>
    protected void PropertyHasChanged()
    {
      string propertyName = new System.Diagnostics.StackTrace().GetFrame(1).GetMethod().Name.Substring(4);
      OnPropertyChanged(propertyName);
    }

    #endregion

    #endregion

    #region 方法

    /// <summary>
    /// 标为 IsNew = false 且 IsSelfDeleted = false 且 IsSelfDirty = false
    /// </summary>
    protected virtual void MarkFetched()
    {
    }
    void IEntity.MarkFetched()
    {
      MarkFetched();
    }

    /// <summary>
    /// 是否脏属性?(如果写入时的新值与旧值相同则认为未被赋值过)
    /// ignoreCompare = false
    /// </summary>
    /// <param name="propertyInfo">属性信息</param>
    public bool IsDirtyProperty(IPropertyInfo propertyInfo)
    {
      throw new NotImplementedException(MethodBase.GetCurrentMethod().Name);
    }

    /// <summary>
    /// 是否脏属性?(如果写入时的新值与旧值相同则认为未被赋值过)
    /// </summary>
    /// <param name="propertyInfo">属性信息</param>
    /// <param name="ignoreCompare">忽略比较新旧值(仅判断是否被赋值过)</param>
    public virtual bool IsDirtyProperty(IPropertyInfo propertyInfo, bool ignoreCompare)
    {
      throw new NotImplementedException(MethodBase.GetCurrentMethod().Name);
    }

    /// <summary>
    /// 取最原始的属性值
    /// </summary>
    /// <param name="propertyInfo">属性信息</param>
    public virtual object GetOldValue(IPropertyInfo propertyInfo)
    {
      throw new NotImplementedException(MethodBase.GetCurrentMethod().Name);
    }

    #region Register

    /// <summary>
    /// 注册属性信息
    /// </summary>
    /// <typeparam name="P">属性类</typeparam>
    /// <param name="propertyLambdaExpression">属性表达式</param>
    /// <returns>属性信息</returns>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures")]
    protected static PropertyInfo<P> RegisterProperty<P>(Expression<Func<T, P>> propertyLambdaExpression)
    {
      System.Reflection.PropertyInfo reflectedPropertyInfo = Reflect<T>.GetProperty(propertyLambdaExpression);
      return new PropertyInfo<P>(typeof(T), reflectedPropertyInfo.Name);
    }

    /// <summary>
    /// 注册属性信息
    /// </summary>
    /// <typeparam name="P">属性类</typeparam>
    /// <param name="propertyLambdaExpression">属性表达式</param>
    /// <param name="friendlyName">友好名</param>
    /// <returns>属性信息</returns>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures")]
    protected static PropertyInfo<P> RegisterProperty<P>(Expression<Func<T, P>> propertyLambdaExpression,
      string friendlyName)
    {
      System.Reflection.PropertyInfo reflectedPropertyInfo = Reflect<T>.GetProperty(propertyLambdaExpression);
      return new PropertyInfo<P>(typeof(T), reflectedPropertyInfo.Name, friendlyName);
    }

    private static PropertyInfo<P> RegisterProperty<P>(Expression<Func<T, P>> propertyLambdaExpression,
      string friendlyName, bool selectable)
    {
      PropertyInfo<P> result = RegisterProperty<P>(propertyLambdaExpression, friendlyName);
      result.Selectable = selectable;
      return result;
    }

    /// <summary>
    /// 注册属性信息
    /// </summary>
    /// <typeparam name="P">属性类</typeparam>
    /// <param name="propertyLambdaExpression">属性表达式</param>
    /// <param name="defaultValue">缺省值</param>
    /// <param name="friendlyName">友好名</param>
    /// <returns>属性信息</returns>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures")]
    protected static PropertyInfo<P> RegisterProperty<P>(Expression<Func<T, P>> propertyLambdaExpression,
      P defaultValue, string friendlyName)
    {
      System.Reflection.PropertyInfo reflectedPropertyInfo = Reflect<T>.GetProperty(propertyLambdaExpression);
      return new PropertyInfo<P>(typeof(T), reflectedPropertyInfo.Name, friendlyName, defaultValue);
    }

    /// <summary>
    /// 注册属性信息
    /// </summary>
    /// <typeparam name="P">属性类</typeparam>
    /// <param name="propertyLambdaExpression">属性表达式</param>
    /// <param name="defaultValueFunc">缺省值函数</param>
    /// <param name="friendlyName">友好名</param>
    /// <returns>属性信息</returns>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures")]
    protected static PropertyInfo<P> RegisterProperty<P>(Expression<Func<T, P>> propertyLambdaExpression,
      Func<object> defaultValueFunc, string friendlyName)
    {
      System.Reflection.PropertyInfo reflectedPropertyInfo = Reflect<T>.GetProperty(propertyLambdaExpression);
      return new PropertyInfo<P>(typeof(T), reflectedPropertyInfo.Name, friendlyName, defaultValueFunc);
    }

    /// <summary>
    /// 注册方法信息
    /// </summary>
    /// <param name="objectType">方法所属类</param>
    /// <param name="methodName">方法名</param>
    /// <param name="friendlyName">友好名</param>
    /// <param name="tag">标记</param>
    /// <returns>方法信息</returns>
    protected static Phenix.Core.Mapping.MethodInfo RegisterMethod(Type objectType, string methodName,
      string friendlyName, string tag)
    {
      System.Reflection.MethodInfo methodInfo = objectType.GetMethod(methodName);
      if (methodInfo == null)
        throw new ArgumentException(string.Format(Phenix.Core.Properties.Resources.NoSuchMethod, methodName), "methodName");
      return new Phenix.Core.Mapping.MethodInfo(objectType, methodName, friendlyName, tag);
    }

    /// <summary>
    /// 注册方法信息
    /// </summary>
    /// <param name="methodLambdaExpression">方法表达式</param>
    /// <param name="friendlyName">友好名</param>
    /// <param name="tag">标记</param>
    /// <returns>方法信息</returns>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures")]
    protected static Phenix.Core.Mapping.MethodInfo RegisterMethod(Expression<Action<T>> methodLambdaExpression,
      string friendlyName, string tag)
    {
      System.Reflection.MethodInfo reflectedMethodInfo = Reflect<T>.GetMethod(methodLambdaExpression);
      return RegisterMethod(typeof(T), reflectedMethodInfo.Name, friendlyName, tag);
    }

    #endregion

    /// <summary>
    /// 字符串表示
    /// </summary>
    public override string ToString()
    {
      return Caption;
    }

    /// <summary>
    /// 取哈希值(注意字符串在32位和64位系统有不同的算法得到不同的结果) 
    /// </summary>
    public override int GetHashCode()
    {
      return PrimaryKey.GetHashCode();
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
      return String.CompareOrdinal(PrimaryKey, other.PrimaryKey) == 0;
    }

    #endregion
  }
}
