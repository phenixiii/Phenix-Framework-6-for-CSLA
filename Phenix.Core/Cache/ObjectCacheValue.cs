using System;

namespace Phenix.Core.Cache
{
  internal class ObjectCacheValue
  {
    public ObjectCacheValue(object value)
    {
      _value = value;
    }

    #region 属性

    private readonly object _value;
    public object Value
    {
      get
      {
        if (_getCount <= MAX_COUNT)
        {
          DateTime newReadTime = DateTime.Now;
          if (newReadTime.Subtract(_readTime).TotalSeconds >= MAX_INTERVAL_SECONDS)
            _getCount = _getCount + 1;
          _readTime = newReadTime;
        }
        return _value;
      }
    }

    private const int MIN_COUNT = 1;
    private const int MAX_COUNT = 10;
    private const double MAX_INTERVAL_SECONDS = 100;

    private int _getCount = MIN_COUNT;
    private DateTime _readTime = DateTime.Now;

    public bool AllowRemove
    {
      get { return _getCount <= MIN_COUNT; }
    }

    public bool AllowUpshift
    {
      get { return _getCount >= MAX_COUNT; }
    }

    #endregion
  }
}
