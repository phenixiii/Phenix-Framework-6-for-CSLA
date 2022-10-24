using System;
using System.Collections.Generic;
using Phenix.Core.Reflection;
using Phenix.Core.SyncCollections;

namespace Phenix.Core.Cache
{
  internal class ObjectCacheInfo
  {
    public ObjectCacheInfo(Type valueType)
    {
      _valueType = valueType;
    }

    #region 属性

    private readonly Type _valueType;

    private ObjectCacheAttribute _objectCacheAttribute;
    private ObjectCacheAttribute ObjectCacheAttribute
    {
      get
      {
        if (_objectCacheAttribute == null)
          _objectCacheAttribute = (ObjectCacheAttribute)Attribute.GetCustomAttribute(_valueType, typeof(ObjectCacheAttribute)) ?? new ObjectCacheAttribute();
        return _objectCacheAttribute;
      }
    }

    public bool Steady
    {
      get { return !AppConfig.AutoMode && DateTime.Now.Subtract(_findValueTime).TotalSeconds <= ObjectCacheAttribute.UpdateInterval; }
    }

    public int MaxValues0Count
    {
      get { return ObjectCacheAttribute.MaxCount > VALUES_0_MAX_COUNT ? ObjectCacheAttribute.MaxCount : VALUES_0_MAX_COUNT; }
    }

    public int MaxValues1Count
    {
      get { return ObjectCacheAttribute.MaxCount < VALUES_1_MAX_COUNT ? ObjectCacheAttribute.MaxCount : VALUES_1_MAX_COUNT; }
    }

    public bool IsValid
    {
      get { return ObjectCacheAttribute.IsValid; }
    }

    private readonly SynchronizedDictionary<object, ObjectCacheValue> _values0 =
      new SynchronizedDictionary<object, ObjectCacheValue>(VALUES_0_MAX_COUNT);
    private readonly SynchronizedDictionary<object, ObjectCacheValue> _values1 =
      new SynchronizedDictionary<object, ObjectCacheValue>(VALUES_1_MAX_COUNT);

    private DateTime _actionTime = DateTime.MinValue;
    public DateTime ActionTime
    {
      get { return _actionTime; }
    }

    internal const int VALUES_0_MAX_COUNT = 10;
    private const int VALUES_1_MAX_COUNT = VALUES_0_MAX_COUNT / 2;

    private DateTime _addInfoTime = DateTime.Now;
    private DateTime _findValueTime = DateTime.MinValue;

    #endregion

    #region 方法

    public void Clear()
    {
      _actionTime = DateTime.MinValue;
      _values0.Clear();
      _values1.Clear();
    }

    public ObjectCacheValue FindValue(object key)
    {
      if (key == null)
        key = _valueType;
      ObjectCacheValue value;
      if (!_values1.TryGetValue(key, out value))
        if (_values0.TryGetValue(key, out value))
          if (value.AllowUpshift && _values1.Count < MaxValues1Count)
          {
            if (!(key is Type))
              key = Utilities.Clone(key);
            _values1[key] = value;
            _values0.Remove(key);
          }
      if (value != null)
        _findValueTime = DateTime.Now;
      return value;
    }

    public bool AddInfo(object key, object value, DateTime actionTime, bool isForce)
    {
      if (!isForce)
      {
        if (_actionTime > actionTime)
          return false;
        int count = MaxValues0Count - _values0.Count;
        if (count <= 0)
        {
          if (DateTime.Now.Subtract(_addInfoTime).TotalMinutes <= 1) //不允许小于1分钟的频繁爆档
            return false;
          if (DateTime.Now.Subtract(_findValueTime).TotalHours <= 1) //不允许对经常访问（1小时内访问过）的缓存频繁爆档
            return false;
          List<object> byRemovingKeys = new List<object>(_values0.Count);
          foreach (KeyValuePair<object, ObjectCacheValue> kvp in _values0)
            if (kvp.Value.AllowRemove)
              byRemovingKeys.Add(kvp.Key);
            else if (count <= 0)
            {
              byRemovingKeys.Add(kvp.Key);
              count = count + 1;
            }
          foreach (object item in byRemovingKeys)
            _values0.Remove(item);
        }
      }
      _values0[key == null ? _valueType : Utilities.Clone(key)] = new ObjectCacheValue(Utilities.Clone(value));
      _addInfoTime = DateTime.Now;
      _actionTime = actionTime;
      return true;
    }

    public bool RemoveInfo(object key)
    {
      return _values1.Remove(key ?? _valueType) || _values0.Remove(key ?? _valueType);
    }

    #endregion
  }
}