using System;
using System.Collections.Generic;
using System.ComponentModel;
using Phenix.Core.Mapping;
using Phenix.Core.Reflection;

namespace Phenix.Core.Operate
{
  /// <summary>
  /// 枚举"键-标签"
  /// 主要用于填充下拉列表框内容
  /// </summary>
  [Serializable]
  public sealed class KeyCaptionCollection : KeyCaptionCollection<KeyCaption, object>, IEntityCollection
  {
    /// <summary>
    /// 初始化
    /// </summary>
    /// <param name="itemValueType">项值类型</param>
    public KeyCaptionCollection(Type itemValueType)
      : base()
    {
      _itemValueType = itemValueType;
    }

    /// <summary>
    /// 初始化
    /// </summary>
    /// <param name="itemValueType">项值类型</param>
    /// <param name="list">项值队列</param>
    public KeyCaptionCollection(Type itemValueType, IList<KeyCaption> list)
      : base(list)
    {
      _itemValueType = itemValueType;
    }

    #region 工厂

    /// <summary>
    /// 根据实体队列构建填充
    /// </summary>
    /// <param name="source">数据源</param>
    /// <param name="keyPropertyInfo">键所在属性</param>
    /// <param name="captionPropertyInfo">标签所在属性</param>
    public static KeyCaptionCollection Fetch<TEntity>(IList<TEntity> source, IPropertyInfo keyPropertyInfo, IPropertyInfo captionPropertyInfo)
      where TEntity : IEntity
    {
      CodingStandards.CheckFieldMapInfo(keyPropertyInfo);
      CodingStandards.CheckFieldMapInfo(captionPropertyInfo);

      KeyCaptionCollection result = new KeyCaptionCollection(typeof(TEntity));
      bool oldRaiseListChangedEvents = result.RaiseListChangedEvents;
      try
      {
        result.RaiseListChangedEvents = false;
        result.SelfFetching = true;
        foreach (TEntity item in source)
        {
          object key = keyPropertyInfo.FieldMapInfo.GetValue(item);
          object caption = captionPropertyInfo.FieldMapInfo.GetValue(item);
          result.Add(new KeyCaption(key, caption != null ? caption.ToString() : null, item));
        }
        result.SelfFetching = false;
      }
      finally
      {
        result.RaiseListChangedEvents = oldRaiseListChangedEvents;
      }
      return result;
    }

    #endregion
    
    #region 属性

    private readonly Type _itemValueType;
    /// <summary>
    /// 项值类型
    /// </summary>
    [System.ComponentModel.Browsable(false)]
    [System.ComponentModel.DataAnnotations.Display(AutoGenerateField = false)]
    public new Type ItemValueType
    {
      get { return _itemValueType; }
    }
    Type IEntityCollection.ItemValueType
    {
      get { return ItemValueType; }
    }

    #endregion
  }

  /// <summary>
  /// "键-标签"数组
  /// 主要用于填充下拉列表框内容
  /// </summary>
  [Serializable]
  public abstract class KeyCaptionCollection<TKeyCaption, TValue> : BindingList<TKeyCaption>, IKeyCaptionCollection
    where TKeyCaption : KeyCaption<TKeyCaption, TValue>
  {
    /// <summary>
    /// 初始化
    /// </summary>
    protected KeyCaptionCollection()
      : base() { }

    /// <summary>
    /// 初始化
    /// </summary>
    protected KeyCaptionCollection(IList<TKeyCaption> list)
      : base(list) { }

    #region 属性

    /// <summary>
    /// 数据源键
    /// </summary>
    protected string DataSourceKey
    {
      get { return String.Empty; }
    }
    string IEntityCollection.DataSourceKey
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
    ICriterions IEntityCollection.Criterions
    {
      get { return Criterions; }
    }

    /// <summary>
    /// 标签
    /// </summary>
    public virtual string Caption
    {
      get { return String.Empty; }
    }

    /// <summary>
    /// 项值类型
    /// </summary>
    [System.ComponentModel.Browsable(false)]
    [System.ComponentModel.DataAnnotations.Display(AutoGenerateField = false)]
    public Type ItemValueType
    {
      get { return typeof(TValue); }
    }

    private bool _selfFetching;
    /// <summary>
    /// 正在检索中
    /// </summary>
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

    private List<ISelectable> _selectedItems;
    /// <summary>
    /// 被勾选的对象队列
    /// </summary>
    [System.ComponentModel.Browsable(false)]
    [System.ComponentModel.DataAnnotations.Display(AutoGenerateField = false)]
    public IList<ISelectable> SelectedItems
    {
      get
      {
        if (_selectedItems == null)
          _selectedItems = new List<ISelectable>(Count);
        return _selectedItems;
      }
    }

    bool IEntityCollection.RaiseListChangedEvents
    {
      get { return RaiseListChangedEvents; }
      set { RaiseListChangedEvents = value; }
    }

    #endregion

    #region 事件

    #region ISelectedCollection 成员

    [NonSerialized]
    private List<EventHandler<SelectedValueChangingEventArgs>> _itemSelectedValueChangingHandlers;
    /// <summary>
    /// 勾选项被更改前
    /// </summary>
    public event EventHandler<SelectedValueChangingEventArgs> ItemSelectedValueChanging
    {
      add
      {
        if (_itemSelectedValueChangingHandlers == null)
          _itemSelectedValueChangingHandlers = new List<EventHandler<SelectedValueChangingEventArgs>>();
        _itemSelectedValueChangingHandlers.Add(value);
        foreach (TKeyCaption item in this)
          item.SelectedValueChanging += value;
      }
      remove
      {
        if (_itemSelectedValueChangingHandlers != null)
          _itemSelectedValueChangingHandlers.Remove(value);
        foreach (TKeyCaption item in this)
          item.SelectedValueChanging -= value;
      }
    }

    [NonSerialized]
    private List<EventHandler<SelectedValueChangedEventArgs>> _itemSelectedValueChangedHandlers;
    /// <summary>
    /// 勾选项被更改后
    /// </summary>
    public event EventHandler<SelectedValueChangedEventArgs> ItemSelectedValueChanged
    {
      add
      {
        if (_itemSelectedValueChangedHandlers == null)
          _itemSelectedValueChangedHandlers = new List<EventHandler<SelectedValueChangedEventArgs>>();
        _itemSelectedValueChangedHandlers.Add(value);
        foreach (TKeyCaption item in this)
          item.SelectedValueChanged += value;
      }
      remove
      {
        if (_itemSelectedValueChangedHandlers != null)
          _itemSelectedValueChangedHandlers.Remove(value);
        foreach (TKeyCaption item in this)
          item.SelectedValueChanged -= value;
      }
    }

    #endregion

    #endregion

    #region 方法

    private TKeyCaption FixItem(TKeyCaption item)
    {
      item.Owner = this;
      if (item.Selected && !SelectedItems.Contains(item))
        SelectedItems.Add(item);
      if (!SelfFetching)
      {
        if (_itemSelectedValueChangingHandlers != null)
          foreach (EventHandler<SelectedValueChangingEventArgs> handler in _itemSelectedValueChangingHandlers)
            item.SelectedValueChanging += handler;
        if (_itemSelectedValueChangedHandlers != null)
          foreach (EventHandler<SelectedValueChangedEventArgs> handler in _itemSelectedValueChangedHandlers)
            item.SelectedValueChanged += handler;
      }
      return item;
    }

    /// <summary>
    /// 克隆
    /// </summary>
    public object Clone()
    {
      return Utilities.Clone(this);
    }

    #region Item

    /// <summary>
    /// 检索第一个匹配对象
    /// 根据 Key 匹配
    /// </summary>
    /// <param name="key">键</param>
    public TKeyCaption FindByKey(object key)
    {
      foreach (TKeyCaption item in this)
        if (object.Equals(item.Key, key))
          return item;
      return null;
    }

    /// <summary>
    /// 检索第一个匹配对象
    /// 根据 Value 匹配
    /// </summary>
    /// <param name="value">值</param>
    public TKeyCaption FindByValue(TValue value)
    {
      foreach (TKeyCaption item in this)
        if (object.Equals(item.Value, value))
          return item;
      return null;
    }
    
    /// <summary>
    /// 添加项到集合中
    /// </summary>
    /// <param name="index">索引</param>
    /// <param name="item">项</param>
    protected override void InsertItem(int index, TKeyCaption item)
    {
      base.InsertItem(index, FixItem(item));
    }

    /// <summary>
    /// 使用指定项替换指定索引处的项
    /// </summary>
    /// <param name="index">索引</param>
    /// <param name="item">项</param>
    protected override void SetItem(int index, TKeyCaption item)
    {
      base.SetItem(index, FixItem(item));
    }

    /// <summary>
    /// 移除指定索引处的项
    /// </summary>
    /// <param name="index">索引</param>
    protected override void RemoveItem(int index)
    {
      if (_itemSelectedValueChangingHandlers != null)
      {
        TKeyCaption item = this[index];
        foreach (EventHandler<SelectedValueChangingEventArgs> handler in _itemSelectedValueChangingHandlers)
          item.SelectedValueChanging -= handler;
      }
      if (_itemSelectedValueChangedHandlers != null)
      {
        TKeyCaption item = this[index];
        foreach (EventHandler<SelectedValueChangedEventArgs> handler in _itemSelectedValueChangedHandlers)
          item.SelectedValueChanged -= handler;
      }
      base.RemoveItem(index);
    }
    
    #endregion

    #region Select

    /// <summary>
    /// 按照条件勾选
    /// </summary>
    /// <param name="toSelected">使得被勾选</param>
    /// <param name="match">定义要勾选的元素应满足的条件</param>
    public void SelectAll(bool toSelected, Predicate<TKeyCaption> match)
    {
      foreach (TKeyCaption item in this)
        if (match == null || match(item))
          item.Selected = toSelected;
    }

    /// <summary>
    /// 勾选所有
    /// match = null
    /// </summary>
    /// <param name="toSelected">使得被勾选</param>
    public void SelectAll(bool toSelected)
    {
      SelectAll(toSelected, null);
    }

    #region ISelectedCollection 成员

    /// <summary>
    /// 勾选所有
    /// </summary>
    public void SelectAll()
    {
      SelectAll(true);
    }

    /// <summary>
    /// 取消所有勾选
    /// </summary>
    public void UnselectAll()
    {
      SelectAll(false);
    }

    /// <summary>
    /// 反选所有
    /// </summary>
    public void InverseAll()
    {
      foreach (TKeyCaption item in this)
        item.Selected = !item.Selected;
    }

    #endregion

    #endregion

    #region GetValues

    /// <summary>
    /// 获取值的集合
    /// </summary>
    public IList<TValue> GetValues()
    {
      List<TValue> result = new List<TValue>(this.Count);
      foreach (TKeyCaption item in this)
        result.Add(item.Value);
      return result;
    }

    /// <summary>
    /// 获取值的集合
    /// </summary>
    public IList<T> GetValues<T>()
      where T : TValue
    {
      List<T> result = new List<T>(this.Count);
      foreach (TKeyCaption item in this)
        result.Add((T)item.Value);
      return result;
    }

    #endregion

    #endregion
  }
}
