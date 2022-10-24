using System;
using System.Runtime.Serialization;
using System.Threading;

namespace Phenix.Core.Threading
{
  /// <summary>
  /// 加锁比较
  /// </summary>
  /// <typeparam name="T">IComparable</typeparam>
  [Serializable]
  public sealed class ComparableLocked<T> : ISerializable, IComparable, IComparable<ComparableLocked<T>>
    where T: IComparable<T>
  {
    /// <summary>
    /// 初始化
    /// </summary>
    public ComparableLocked(T value)
    {
      _rwLock = new ReaderWriterLock();
      _value = value;
    }

    #region Serialization

    /// <summary>
    /// 序列化
    /// </summary>
    private ComparableLocked(SerializationInfo info, StreamingContext context)
    {
      if (info == null)
        throw new ArgumentNullException("info");
      _rwLock = new ReaderWriterLock();
      _value = (T)info.GetValue("_value", typeof(T));
    }

    /// <summary>
    /// 反序列化
    /// </summary>
    [System.Security.SecurityCritical]
     void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
    {
      if (info == null)
        throw new ArgumentNullException("info");
      info.AddValue("_value", _value);
    }

    #endregion

    #region 属性

    [NonSerialized]
    private readonly ReaderWriterLock _rwLock;

    private T _value;
    /// <summary>
    /// 值
    /// </summary>
    public T Value
    {
      get
      {
        _rwLock.AcquireReaderLock(Timeout.Infinite);
        try
        {
          return _value;
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
          _value = value;
        }
        finally
        {
          _rwLock.ReleaseWriterLock();
        }
      }
    }

    #endregion

    #region 方法

    private void SetValue(T value)
    {
      LockCookie lockCookie = _rwLock.UpgradeToWriterLock(Timeout.Infinite);
      try
      {
        _value = value;
      }
      finally
      {
        _rwLock.DowngradeFromWriterLock(ref lockCookie);
      }
    }

    /// <summary>
    /// 改为更小值
    /// </summary>
    public bool ChangeToSmaller(T value)
    {
      _rwLock.AcquireReaderLock(Timeout.Infinite);
      try
      {
        bool result = (object)_value == null || _value.CompareTo(value) > 0;
        if (result)
          SetValue(value);
        return result;
      }
      finally
      {
        _rwLock.ReleaseReaderLock();
      }
    }

    /// <summary>
    /// 改为更大值
    /// </summary>
    public bool ChangeToLarger(T value)
    {
      _rwLock.AcquireReaderLock(Timeout.Infinite);
      try
      {
        bool result = (object)_value == null || _value.CompareTo(value) < 0;
        if (result)
          SetValue(value);
        return result;
      }
      finally
      {
        _rwLock.ReleaseReaderLock();
      }
    }

    /// <summary>
    /// 取哈希值(注意字符串在32位和64位系统有不同的算法得到不同的结果) 
    /// </summary>
    public override int GetHashCode()
    {
      return (object)_value != null ? _value.GetHashCode() : 0;
    }

    /// <summary>
    /// 比较对象
    /// </summary>
    /// <param name="obj">对象</param>
    public override bool Equals(object obj)
    {
      if (object.ReferenceEquals(obj, this))
        return true;
      ComparableLocked<T> other = obj as ComparableLocked<T>;
      if (object.ReferenceEquals(other, null))
        return false;

      _rwLock.AcquireReaderLock(Timeout.Infinite);
      try
      {
        if (object.ReferenceEquals(_value, null))
          return false;
        return _value.Equals(other._value);
      }
      finally
      {
        _rwLock.ReleaseReaderLock();
      }
    }

    #region IComparable 成员

    /// <summary>
    /// 比较对象
    /// </summary>
    public int CompareTo(object obj)
    {
      return CompareTo(obj as ComparableLocked<T>);
    }

    /// <summary>
    /// 比较对象
    /// </summary>
    public int CompareTo(ComparableLocked<T> other)
    {
      if (object.ReferenceEquals(other, this))
        return 0;
      if (object.ReferenceEquals(other, null))
        return 1;

      _rwLock.AcquireReaderLock(Timeout.Infinite);
      try
      {
        if (object.ReferenceEquals(_value, null))
          return -1;
        return _value.CompareTo(other._value);
      }
      finally
      {
        _rwLock.ReleaseReaderLock();
      }
    }

    /// <summary>
    /// 比较对象
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1000:DoNotDeclareStaticMembersOnGenericTypes")]
    public static int Compare(ComparableLocked<T> x, ComparableLocked<T> y)
    {
      if (object.ReferenceEquals(x, y))
        return 0;
      if (object.ReferenceEquals(x, null))
        return -1;
      return x.CompareTo(y);
    }

    /// <summary>
    /// 等于
    /// </summary>
    public static bool operator ==(ComparableLocked<T> left, ComparableLocked<T> right)
    {
      return Compare(left, right) == 0;
    }

    /// <summary>
    /// 不等于
    /// </summary>
    public static bool operator !=(ComparableLocked<T> left, ComparableLocked<T> right)
    {
      return Compare(left, right) != 0;
    }

    /// <summary>
    /// 小于
    /// </summary>
    public static bool operator <(ComparableLocked<T> left, ComparableLocked<T> right)
    {
      return Compare(left, right) < 0;
    }

    /// <summary>
    /// 大于
    /// </summary>
    public static bool operator >(ComparableLocked<T> left, ComparableLocked<T> right)
    {
      return Compare(left, right) > 0;
    }

    #endregion

    #endregion
  }
}
