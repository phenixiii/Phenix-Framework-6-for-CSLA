using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Threading;

namespace Phenix.Core.SyncCollections
{
  /// <summary>
  /// 表示键和值的集合
  /// </summary>
  /// <typeparam name="TKey">字典中的键的类型</typeparam>
  /// <typeparam name="TValue">字典中的值的类型</typeparam>
  [Serializable]
  public class SynchronizedDictionary<TKey, TValue> : IDictionary<TKey, TValue>, ISerializable
  {
    /// <summary>
    /// 初始化
    /// 该实例为空且具有默认的初始容量, 并使用键类型的默认相等比较器
    /// </summary>
    public SynchronizedDictionary()
    {
      _rwLock = new ReaderWriterLock();
      _infos = new Dictionary<TKey, TValue>();
    }

    /// <summary>
    /// 初始化
    /// 该实例为空且具有指定的初始容量, 并为键类型使用默认的相等比较器
    /// </summary>
    /// <param name="capacity">可包含的初始元素数</param>
    public SynchronizedDictionary(int capacity)
    {
      _rwLock = new ReaderWriterLock();
      _infos = new Dictionary<TKey, TValue>(capacity);
    }

    /// <summary>
    /// 初始化
    /// 该实例包含从指定的集合中复制的元素并为键类型使用默认的相等比较器
    /// </summary>
    /// <param name="dictionary">它的元素被复制到本实例中</param>
    public SynchronizedDictionary(IDictionary<TKey, TValue> dictionary)
    {
      _rwLock = new ReaderWriterLock();
      _infos = new Dictionary<TKey, TValue>(dictionary);
    }

    /// <summary>
    /// 初始化
    /// 该实例为空且具有默认的初始容量
    /// </summary>
    /// <param name="comparer">比较键时要使用的实现; 或者为 null, 以便为键类型使用默认的 EqualityComparer</param>
    public SynchronizedDictionary(IEqualityComparer<TKey> comparer)
    {
      _rwLock = new ReaderWriterLock();
      _infos = new Dictionary<TKey, TValue>(comparer);
    }

    /// <summary>
    /// 初始化
    /// 该实例包含从指定的集合中复制的元素
    /// </summary>
    /// <param name="dictionary">它的元素被复制到本实例中</param>
    /// <param name="comparer">比较键时要使用的实现; 或者为 null, 以便为键类型使用默认的 EqualityComparer</param>
    public SynchronizedDictionary(IDictionary<TKey, TValue> dictionary, IEqualityComparer<TKey> comparer)
    {
      _rwLock = new ReaderWriterLock();
      _infos = new Dictionary<TKey, TValue>(dictionary, comparer);
    }

    /// <summary>
    /// 初始化
    /// 该实例为空且具有指定的初始容量
    /// </summary>
    /// <param name="capacity">可包含的初始元素数</param>
    /// <param name="comparer">比较键时要使用的实现; 或者为 null, 以便为键类型使用默认的 EqualityComparer</param>
    public SynchronizedDictionary(int capacity, IEqualityComparer<TKey> comparer)
    {
      _rwLock = new ReaderWriterLock();
      _infos = new Dictionary<TKey, TValue>(capacity, comparer);
    }

    #region Serialization

    /// <summary>
    /// 序列化
    /// </summary>
    protected SynchronizedDictionary(SerializationInfo info, StreamingContext context)
    {
      if (info == null)
        throw new ArgumentNullException("info");
      _rwLock = new ReaderWriterLock();
      _infos = (Dictionary<TKey, TValue>)info.GetValue("_infos", typeof(Dictionary<TKey, TValue>));
    }

    /// <summary>
    /// 反序列化
    /// </summary>
    [System.Security.SecurityCritical]
    public virtual void GetObjectData(SerializationInfo info, StreamingContext context)
    {
      if (info == null)
        throw new ArgumentNullException("info");
      info.AddValue("_infos", _infos);
    }

    #endregion

    #region 属性

    [NonSerialized]
    private readonly ReaderWriterLock _rwLock;

    private readonly Dictionary<TKey, TValue> _infos;

    /// <summary>
    /// 获取用于确定字典中的键是否相等
    /// </summary>
    public IEqualityComparer<TKey> Comparer
    {
      get { return _infos.Comparer; }
    }

    /// <summary>
    /// 获取键/值对的数目
    /// </summary>
    public int Count
    {
      get
      {
        _rwLock.AcquireReaderLock(Timeout.Infinite);
        try
        {
          return _infos.Count;
        }
        finally
        {
          _rwLock.ReleaseReaderLock();
        }
      }
    }

    /// <summary>
    /// 获取或设置与指定的键相关联的值
    /// </summary>
    /// <param name="key">要获取或设置的值的键</param>
    /// <returns>与指定的键相关联的值. 如果找不到指定的键, get 操作便会引发 KeyNotFoundException, 而 set 操作会创建一个具有指定键的新元素</returns>
    public TValue this[TKey key]
    {
      get
      {
        _rwLock.AcquireReaderLock(Timeout.Infinite);
        try
        {
          return _infos[key];
        }
        finally
        {
          _rwLock.ReleaseReaderLock();
        }
      }
      set
      {
        _rwLock.AcquireWriterLock(Timeout.Infinite);
        try
        {
          _infos[key] = value;
        }
        finally
        {
          _rwLock.ReleaseWriterLock();
        }
      }
    }

    /// <summary>
    /// 获取键的集合, 为静态副本
    /// </summary>
    public ICollection<TKey> Keys
    {
      get
      {
        ICollection<TKey> result;
        _rwLock.AcquireReaderLock(Timeout.Infinite);
        try
        {
          result = new List<TKey>(_infos.Keys);
        }
        finally
        {
          _rwLock.ReleaseReaderLock();
        }
        return result;
      }
    }

    /// <summary>
    /// 获取值的集合, 为静态副本
    /// </summary>
    public ICollection<TValue> Values
    {
      get
      {
        ICollection<TValue> result;
        _rwLock.AcquireReaderLock(Timeout.Infinite);
        try
        {
          result = new List<TValue>(_infos.Values);
        }
        finally
        {
          _rwLock.ReleaseReaderLock();
        }
        return result;
      }
    }

    /// <summary>
    /// 是否只读
    /// </summary>
    public bool IsReadOnly
    {
      get
      {
        _rwLock.AcquireReaderLock(Timeout.Infinite);
        try
        {
          return ((ICollection<KeyValuePair<TKey, TValue>>)_infos).IsReadOnly;
        }
        finally
        {
          _rwLock.ReleaseReaderLock();
        }
      }
    }

    #endregion

    #region 方法

    #region Contains

    /// <summary>
    /// 确定是否包含项
    /// </summary>
    /// <param name="item">项</param>
    public bool Contains(KeyValuePair<TKey, TValue> item)
    {
      _rwLock.AcquireReaderLock(Timeout.Infinite);
      try
      {
        return ((ICollection<KeyValuePair<TKey, TValue>>)_infos).Contains(item);
      }
      finally
      {
        _rwLock.ReleaseReaderLock();
      }
    }

    /// <summary>
    /// 确定是否包含指定的键
    /// </summary>
    /// <param name="key">键</param>
    public bool ContainsKey(TKey key)
    {
      _rwLock.AcquireReaderLock(Timeout.Infinite);
      try
      {
        return _infos.ContainsKey(key);
      }
      finally
      {
        _rwLock.ReleaseReaderLock();
      }
    }

    /// <summary>
    /// 确定是否包含特定值
    /// </summary>
    /// <param name="value">要定位的值. 对于引用类型, 该值可以为 null</param>
    public bool ContainsValue(TValue value)
    {
      _rwLock.AcquireReaderLock(Timeout.Infinite);
      try
      {
        return _infos.ContainsValue(value);
      }
      finally
      {
        _rwLock.ReleaseReaderLock();
      }
    }

    #endregion

    #region GetValue

    /// <summary>
    /// 获取与指定的键相关联的值
    /// </summary>
    /// <param name="key">键</param>
    /// <param name="value">当此方法返回值时, 如果找到该键, 便会返回与指定的键相关联的值; 否则, 则会返回 item 参数的类型默认值</param>
    public bool TryGetValue(TKey key, out TValue value)
    {
      _rwLock.AcquireReaderLock(Timeout.Infinite);
      try
      {
        return _infos.TryGetValue(key, out value);
      }
      finally
      {
        _rwLock.ReleaseReaderLock();
      }
    }

    /// <summary>
    /// 获取与指定的键相关联的值
    /// </summary>
    /// <param name="key">键</param>
    /// <param name="doCreate">构建值的函数</param>
    /// <param name="lockCreate">构建值时加锁</param>
    /// <returns>如果找到该键, 便会返回与指定的键相关联的值; 否则, 则会执行 doCreate 函数构建构建 item 的值关联到键并返回</returns>
    public TValue GetValue(TKey key, Func<TValue> doCreate, bool lockCreate)
    {
      _rwLock.AcquireReaderLock(Timeout.Infinite);
      try
      {
        TValue result;
        if (_infos.TryGetValue(key, out result))
          return result;
        if (!lockCreate)
          result = doCreate != null ? doCreate() : (TValue)Activator.CreateInstance(typeof(TValue), true);
        LockCookie lockCookie = _rwLock.UpgradeToWriterLock(Timeout.Infinite);
        try
        {
          if (lockCreate)
            result = doCreate != null ? doCreate() : (TValue)Activator.CreateInstance(typeof(TValue), true);
          _infos[key] = result;
          return result;
        }
        finally
        {
          _rwLock.DowngradeFromWriterLock(ref lockCookie);
        }
      }
      finally
      {
        _rwLock.ReleaseReaderLock();
      }
    }
    
    #endregion

    #region Add

    /// <summary>
    /// 添加项
    /// </summary>
    /// <param name="item">项</param>
    public void Add(KeyValuePair<TKey, TValue> item)
    {
      _rwLock.AcquireWriterLock(Timeout.Infinite);
      try
      {
        ((ICollection<KeyValuePair<TKey, TValue>>)_infos).Add(item);
      }
      finally
      {
        _rwLock.ReleaseWriterLock();
      }
    }

    /// <summary>
    /// 一次添加项(如果已含则不添加)
    /// </summary>
    /// <param name="item">项</param>
    public bool AddOnce(KeyValuePair<TKey, TValue> item)
    {
      _rwLock.AcquireReaderLock(Timeout.Infinite);
      try
      {
        if (!((ICollection<KeyValuePair<TKey, TValue>>)_infos).Contains(item))
        {
          LockCookie lockCookie = _rwLock.UpgradeToWriterLock(Timeout.Infinite);
          try
          {
            ((ICollection<KeyValuePair<TKey, TValue>>)_infos).Add(item);
            return true;
          }
          finally
          {
            _rwLock.DowngradeFromWriterLock(ref lockCookie);
          }
        }
        return false;
      }
      finally
      {
        _rwLock.ReleaseReaderLock();
      }
    }

    /// <summary>
    /// 将指定的键和值添加到字典中
    /// </summary>
    /// <param name="key">键</param>
    /// <param name="value">要添加的元素的值. 对于引用类型, 该值可以为 null</param>
    public void Add(TKey key, TValue value)
    {
      _rwLock.AcquireWriterLock(Timeout.Infinite);
      try
      {
        _infos.Add(key, value);
      }
      finally
      {
        _rwLock.ReleaseWriterLock();
      }
    }

    /// <summary>
    /// 一次将指定的键和值添加到字典中(如果已含则不添加)
    /// </summary>
    /// <param name="key">键</param>
    /// <param name="value">要添加的元素的值. 对于引用类型, 该值可以为 null</param>
    public bool AddOnce(TKey key, TValue value)
    {
      _rwLock.AcquireReaderLock(Timeout.Infinite);
      try
      {
        if (!_infos.ContainsKey(key))
        {
          LockCookie lockCookie = _rwLock.UpgradeToWriterLock(Timeout.Infinite);
          try
          {
            _infos.Add(key, value);
            return true;
          }
          finally
          {
            _rwLock.DowngradeFromWriterLock(ref lockCookie);
          }
        }
        return false;
      }
      finally
      {
        _rwLock.ReleaseReaderLock();
      }
    }

    #endregion

    #region Remove

    /// <summary>
    /// 移除项
    /// </summary>
    /// <param name="item">项</param>
    public bool Remove(KeyValuePair<TKey, TValue> item)
    {
      _rwLock.AcquireWriterLock(Timeout.Infinite);
      try
      {
        return ((ICollection<KeyValuePair<TKey, TValue>>)_infos).Remove(item);
      }
      finally
      {
        _rwLock.ReleaseWriterLock();
      }
    }

    /// <summary>
    /// 移除所指定的键的值
    /// </summary>
    /// <param name="key">键</param>
    public bool Remove(TKey key)
    {
      _rwLock.AcquireWriterLock(Timeout.Infinite);
      try
      {
        return _infos.Remove(key);
      }
      finally
      {
        _rwLock.ReleaseWriterLock();
      }
    }

    /// <summary>
    /// 移除所指定的键的值
    /// </summary>
    /// <param name="key">键</param>
    /// <param name="doDispose">释放值的函数</param>
    public bool Remove(TKey key, Action<TValue> doDispose)
    {
      _rwLock.AcquireWriterLock(Timeout.Infinite);
      try
      {
        TValue value;
        if (_infos.TryGetValue(key, out value))
        {
          if (doDispose != null)
            doDispose(value);
          return _infos.Remove(key);
        }
      }
      finally
      {
        _rwLock.ReleaseWriterLock();
      }
      return false;
    }

    #endregion

    #region Clear

    /// <summary>
    /// 移除所有的键和值
    /// </summary>
    public void Clear()
    {
      _rwLock.AcquireWriterLock(Timeout.Infinite);
      try
      {
        _infos.Clear();
      }
      finally
      {
        _rwLock.ReleaseWriterLock();
      }
    }

    /// <summary>
    /// 移除所有的键和值
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures")]
    public void Clear(Action<IEnumerable<KeyValuePair<TKey, TValue>>> doDispose)
    {
      _rwLock.AcquireWriterLock(Timeout.Infinite);
      try
      {
        if (doDispose != null)
          doDispose(new Dictionary<TKey, TValue>(_infos));
        _infos.Clear();
      }
      finally
      {
        _rwLock.ReleaseWriterLock();
      }
    }

    #endregion

    #region Replace

    /// <summary>
    /// 替换值
    /// </summary>
    /// <param name="key">键</param>
    /// <param name="doReplace">替换值的函数</param>
    public bool ReplaceValue(TKey key, Func<TValue, TValue> doReplace)
    {
      _rwLock.AcquireWriterLock(Timeout.Infinite);
      try
      {
        TValue value;
        if (_infos.TryGetValue(key, out value))
        {
          _infos[key] = doReplace(value);
          return true;
        }
      }
      finally
      {
        _rwLock.ReleaseWriterLock();
      }
      return false;
    }

    /// <summary>
    /// 替换所有
    /// </summary>
    public void ReplaceAll(IDictionary<TKey, TValue> source)
    {
      _rwLock.AcquireWriterLock(Timeout.Infinite);
      try
      {
        _infos.Clear();
        foreach (KeyValuePair<TKey, TValue> kvp in source)
          _infos.Add(kvp.Key, kvp.Value);
      }
      finally
      {
        _rwLock.ReleaseWriterLock();
      }
    }

    /// <summary>
    /// 替换所有
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures")]
    public void ReplaceAll(Func<KeyValuePair<TKey, TValue>, TValue> doReplace)
    {
      _rwLock.AcquireWriterLock(Timeout.Infinite);
      try
      {
        foreach (KeyValuePair<TKey, TValue> kvp in _infos)
          _infos[kvp.Key] = doReplace(kvp);
      }
      finally
      {
        _rwLock.ReleaseWriterLock();
      }
    }

    #endregion

    #region IEnumerator

    /// <summary>
    /// 返回循环访问的枚举数, 为静态副本
    /// </summary>
    public IEnumerator GetEnumerator()
    {
      Dictionary<TKey, TValue> result;
      _rwLock.AcquireReaderLock(Timeout.Infinite);
      try
      {
        result = new Dictionary<TKey, TValue>(_infos);
      }
      finally
      {
        _rwLock.ReleaseReaderLock();
      }
      return result.GetEnumerator();
    }

    /// <summary>
    /// 返回循环访问的枚举数, 为静态副本
    /// </summary>
    IEnumerator<KeyValuePair<TKey, TValue>> IEnumerable<KeyValuePair<TKey, TValue>>.GetEnumerator()
    {
      Dictionary<TKey, TValue> result;
      _rwLock.AcquireReaderLock(Timeout.Infinite);
      try
      {
        result = new Dictionary<TKey, TValue>(_infos);
      }
      finally
      {
        _rwLock.ReleaseReaderLock();
      }
      return result.GetEnumerator();
    }

    #endregion

    /// <summary>
    /// 从指定的数组索引开始, 将元素复制到一个数组中
    /// </summary>
    /// <param name="array">数组</param>
    /// <param name="arrayIndex">数组索引</param>
    public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
    {
      _rwLock.AcquireReaderLock(Timeout.Infinite);
      try
      {
        ((ICollection<KeyValuePair<TKey, TValue>>)_infos).CopyTo(array, arrayIndex);
      }
      finally
      {
        _rwLock.ReleaseReaderLock();
      }
    }

    #endregion
  }
}