using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading;
using Phenix.Core.Log;
using Phenix.Core.Mapping;
using Phenix.Core.Net;
using Phenix.Core.Reflection;
using Phenix.Core.SyncCollections;

namespace Phenix.Core.Cache
{
  /// <summary>
  /// 申明清除后事件处理函数
  /// </summary>
  public delegate void ClearedEventHandler(string groupName);

  /// <summary>
  /// 对象缓存
  /// 不支持IDisposable对象
  /// </summary>
  public static class ObjectCache
  {
    #region 属性

    private static IObjectCacheSynchro _worker;
    /// <summary>
    /// 同步器
    /// </summary>
    public static IObjectCacheSynchro Worker
    {
      get
      {
        if (_worker == null)
          AppUtilities.RegisterWorker();
        return _worker;
      }
      set
      {
        _worker = value;
        ClearCache();
      }
    }

    private static readonly SynchronizedDictionary<string, ObjectCacheInfo> _cacheInfos =
      new SynchronizedDictionary<string, ObjectCacheInfo>(StringComparer.Ordinal);

    private static Thread _clearThread;
    private static readonly SynchronizedQueue<string> _clearingTypeNames = new SynchronizedQueue<string>(256);

    private static bool _extremely = true;
    /// <summary>
    /// 极端方式: 将缓存保存到本地
    /// 缺省为 !AppConfig.AutoMode
    /// </summary>
    public static bool Extremely
    {
      get { return !AppConfig.AutoMode && _extremely; }
      set { _extremely = value; }
    }

    private static int _extremelyThresholdMin = 1024 * 1024 / 2;
    /// <summary>
    /// 极端方式最小阈值: 缓存流长度
    /// 缺省为 1024 * 1024 / 2
    /// </summary>
    public static int ExtremelyThresholdMin
    {
      get { return _extremelyThresholdMin; }
      set { _extremelyThresholdMin = value; }
    }

    private static int _extremelyThresholdMax = 1024 * 1024 * 4;
    /// <summary>
    /// 极端方式最大阈值: 缓存流长度
    /// 缺省为 1024 * 1024 * 4
    /// </summary>
    public static int ExtremelyThresholdMax
    {
      get { return _extremelyThresholdMax; }
      set { _extremelyThresholdMax = value; }
    }

    private static string _localCacheDirectory;
    /// <summary>
    /// 本地缓存目录
    /// </summary>
    public static string LocalCacheDirectory
    {
      get
      {
        if (String.IsNullOrEmpty(_localCacheDirectory))
          _localCacheDirectory = Path.Combine(AppConfig.BaseDirectory, AppSettings.DefaultKey + "Cache");
        return _localCacheDirectory;
      }
    }

    private static readonly object _extremelyLock = new object();

    private static readonly SynchronizedDictionary<string, SynchronizedList<ClearedEventHandler>> _clearedEventHandlerCache =
      new SynchronizedDictionary<string, SynchronizedList<ClearedEventHandler>>(StringComparer.Ordinal);

    #endregion

    #region 事件

    /// <summary>
    /// 注册ClearedEventHandler
    /// </summary>
    /// <param name="type">类</param>
    /// <param name="eventHandler">事件</param>
    public static void Register(Type type, ClearedEventHandler eventHandler)
    {
      bool find = false;
      foreach (string s in ClassMemberHelper.GetGroupNames(type))
        if (!String.IsNullOrEmpty(s))
        {
          find = true;
          DoRegister(s, eventHandler);
        }
      if (!find)
      {
        type = Utilities.FindFirstAbstractType(Utilities.GetCoreType(type)) ?? type;
        DoRegister(type.FullName, eventHandler);
      }
    }

    private static void DoRegister(string groupName, ClearedEventHandler eventHandler)
    {
      SynchronizedList<ClearedEventHandler> eventHandlers = _clearedEventHandlerCache.GetValue(groupName, () => new SynchronizedList<ClearedEventHandler>(), true);
      eventHandlers.AddOnce(eventHandler);
    }

    /// <summary>
    /// 注销ClearedEventHandler
    /// </summary>
    /// <param name="type">类</param>
    /// <param name="eventHandler">事件</param>
    public static void Unregister(Type type, ClearedEventHandler eventHandler)
    {
      SynchronizedList<ClearedEventHandler> eventHandlers;
      if (_clearedEventHandlerCache.TryGetValue(type.FullName, out eventHandlers))
        eventHandlers.Remove(eventHandler);
    }

    private static void OnCleared(string typeName)
    {
      Type type = Utilities.LoadType(typeName);
      if (type != null)
        OnCleared(type);
    }

    private static void OnCleared(Type type)
    {
      bool find = false;
      foreach (string s in ClassMemberHelper.GetGroupNames(type))
        if (!String.IsNullOrEmpty(s))
        {
          find = true;
          DoCleared(s);
        }
      if (!find)
      {
        type = Utilities.FindFirstAbstractType(Utilities.GetCoreType(type)) ?? type;
        DoCleared(type.FullName);
      }
    }

    private static void DoCleared(string groupName)
    {
      SynchronizedList<ClearedEventHandler> eventHandlers;
      if (_clearedEventHandlerCache.TryGetValue(groupName, out eventHandlers))
        foreach (ClearedEventHandler item in eventHandlers)
          item.Invoke(groupName);
    }

    #endregion

    #region 方法
    
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
    private static void ExecuteClear()
    {
      try
      {
        List<string> typeNames = new List<string>(256);
        while (true)
          try
          {
            if (_clearingTypeNames.Count > 0)
            {
              DateTime dt = DateTime.Now;
              while (DateTime.Now.Subtract(dt).TotalSeconds <= 2)
              {
                while (_clearingTypeNames.Count > 0)
                {
                  string typeName = _clearingTypeNames.Dequeue();
                  if (!typeNames.Contains(typeName))
                    typeNames.Add(typeName);
                }
                Thread.Sleep(100);
              }
              Clear(typeNames);
              typeNames.Clear();
            }
            Thread.Sleep(100);
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
          catch (Exception)
          {
            Thread.Sleep(NetConfig.TcpTimedWaitDelay);
          }
      }
      finally
      {
        _clearThread = null;
      }
    }

    /// <summary>
    /// 清理缓存
    /// </summary>
    public static void ClearCache()
    {
      _cacheInfos.Clear();
    }

    /// <summary>
    /// 清除全部缓存
    /// </summary>
    public static void ClearAll()
    {
      try
      {
        if (Worker != null)
          Worker.ClearAll();
      }
      finally
      {
        ClearCache();
      }
    }

    /// <summary>
    /// 清除某类的缓存
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
    public static void Clear<T>()
    {
      Clear(typeof(T));
    }

    /// <summary>
    /// 清除某类的缓存
    /// </summary>
    public static void Clear(Type type)
    {
      if (type == null)
        return;
      string typeName = type.FullName;
      try
      {
        if (_clearThread == null)
          lock (_clearingTypeNames)
            if (_clearThread == null)
            {
              _clearThread = new Thread(ExecuteClear);
              _clearThread.IsBackground = true;
              _clearThread.Start();
            }
        _clearingTypeNames.Enqueue(typeName);
      }
      finally
      {
        ObjectCacheInfo info;
        if (_cacheInfos.TryGetValue(typeName, out info))
          info.Clear();
        OnCleared(type);
      }
    }

    /// <summary>
    /// 清除某类的缓存
    /// </summary>
    public static void Clear(IEnumerable<Type> types)
    {
      if (types == null)
        return;
      foreach (Type item in types)
        Clear(item);
    }

    /// <summary>
    /// 清除某类的缓存
    /// </summary>
    [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
    public static void Clear(IList<string> typeNames)
    {
      try
      {
        if (Worker != null)
          Worker.Clear(typeNames);
      }
      finally
      {
        foreach (string s in typeNames)
        {
          ObjectCacheInfo info;
          if (_cacheInfos.TryGetValue(s, out info))
            info.Clear();
          OnCleared(s);
        }
      }
    }

    /// <summary>
    /// 声明某表记录发生更改
    /// </summary>
    public static void RecordHasChanged(string tableName)
    {
      if (Worker != null)
        Worker.RecordHasChanged(tableName);
    }

    /// <summary>
    /// 获取活动时间
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
    public static DateTime? GetActionTime<T>()
    {
      return GetActionTime(typeof(T));
    }

    /// <summary>
    /// 获取活动时间
    /// </summary>
    public static DateTime? GetActionTime(Type type)
    {
      if (type == null)
        return null;
      return GetActionTime(type.FullName);
    }

    /// <summary>
    /// 获取活动时间
    /// </summary>
    [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
    public static DateTime? GetActionTime(string typeName)
    {
      DateTime? result = Worker != null ? Worker.GetActionTime(typeName) : null;
      if (!result.HasValue)
        return null;
      ObjectCacheInfo info;
      if (_cacheInfos.TryGetValue(typeName, out info))
      {
        if (info.ActionTime < result.Value)
        {
          info.Clear();
          OnCleared(typeName);
        }
      }
      return result;
    }
    
    /// <summary>
    /// 查找
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
    public static object Find<T>(object key, out DateTime? actionTime)
    {
      return DoFind(typeof(T), key, out actionTime);
    }

    /// <summary>
    /// 查找
    /// </summary>
    public static object Find(Type type, object key, out DateTime? actionTime)
    {
      return DoFind(type, key, out actionTime);
    }

    private static object DoFind(Type type, object key, out DateTime? actionTime)
    {
      IObjectCacheKey cacheKey = null;
      if (key != null)
      {
        cacheKey = key as IObjectCacheKey;
        if (cacheKey == null || !cacheKey.CacheEnabled)
        {
          actionTime = null;
          return null;
        }
      }
      ObjectCacheInfo info;
      if (_cacheInfos.TryGetValue(type.FullName, out info))
        if (info.Steady)
        {
          ObjectCacheValue value = info.FindValue(key);
          if (value != null)
          {
            actionTime = null;
            return Utilities.Clone(value.Value);
          }
        }
      actionTime = GetActionTime(type.FullName);
      if (actionTime.HasValue)
      {
        if (info != null && info.ActionTime >= actionTime.Value)
        {
          actionTime = info.ActionTime;
          ObjectCacheValue value = info.FindValue(key);
          if (value != null)
            return Utilities.Clone(value.Value);
        }
        if (Extremely && (cacheKey == null || cacheKey.ExtremelyCacheEnabled))
        {
          object result;
          if (ExtremelyRead(type.FullName, out result, actionTime.Value))
            if (DoAdd(key, result, actionTime.Value, true, false))
              return result;
        }
      }
      return null;
    }

    /// <summary>
    /// 添加
    /// </summary>
    public static bool Add(object key, object value, DateTime actionTime)
    {
      return DoAdd(key, value, actionTime, false, true);
    }

    private static bool DoAdd(object key, object value, DateTime actionTime, bool isForce, bool allowExtremely)
    {
      if (value == null)
        return false;
      IObjectCacheKey cacheKey = null;
      if (key != null)
      {
        cacheKey = key as IObjectCacheKey;
        if (cacheKey == null || !cacheKey.CacheEnabled)
          return false;
      }
      Type valueType = value.GetType();
      ObjectCacheInfo info = _cacheInfos.GetValue(valueType.FullName, () => new ObjectCacheInfo(valueType), true);
      if (!info.IsValid)
        return false;
      if (info.AddInfo(key, value, actionTime, isForce))
      {
        if (allowExtremely && Extremely && (cacheKey == null || cacheKey.ExtremelyCacheEnabled))
          ExtremelyWrite(valueType.FullName, value, actionTime);
        return true;
      }
      return false;
    }

    /// <summary>
    /// 移除
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
    public static bool Remove<T>(object key)
    {
      return DoRemove(typeof(T), key);
    }

    /// <summary>
    /// 移除
    /// </summary>
    public static bool Remove(Type type, object key)
    {
      return DoRemove(type, key);
    }

    private static bool DoRemove(Type type, object key)
    {
      if (key != null)
      {
        IObjectCacheKey cacheKey = key as IObjectCacheKey;
        if (cacheKey == null || !cacheKey.CacheEnabled)
          return false;
      }
      ObjectCacheInfo info;
      if (_cacheInfos.TryGetValue(type.FullName, out info))
        return info.RemoveInfo(key);
      return false;
    }

    #region Extremely

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
    private static bool ExtremelyRead(string typeName, out object value, DateTime actionTime)
    {
      DateTime valueTime;
      if (DateTime.TryParse(AppSettings.ReadValue(typeName), out valueTime) && valueTime >= actionTime)
      {
        string path = Path.Combine(LocalCacheDirectory, typeName);
        if (File.Exists(path))
          try
          {
            lock (_extremelyLock)
            {
              value = Utilities.Restore(path);
            }
            return value != null;
          }
          catch (Exception ex)
          {
            EventLog.SaveLocal(MethodBase.GetCurrentMethod(), path, ex);
          }
      }
      value = null;
      return false;
    }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
    private static void ExtremelyWrite(string typeName, object value, DateTime actionTime)
    {
      string path = Path.Combine(LocalCacheDirectory, typeName);
      if (File.Exists(path))
      {
        DateTime valueTime;
        if (DateTime.TryParse(AppSettings.ReadValue(typeName), out valueTime) && valueTime >= actionTime)
          return;
      }
      try
      {
        lock (_extremelyLock)
        {
          using (MemoryStream memoryStream = new MemoryStream())
          {
            BinaryFormatter formatter = new BinaryFormatter();
            formatter.Serialize(memoryStream, value);
            if (memoryStream.Length >= ExtremelyThresholdMin && memoryStream.Length <= ExtremelyThresholdMax)
            {
              Utilities.Save(memoryStream, path);
              AppSettings.SaveValue(typeName, actionTime.ToString(CultureInfo.InvariantCulture));
            }
          }
        }
      }
      catch (Exception ex)
      {
        EventLog.SaveLocal(MethodBase.GetCurrentMethod(), typeName, ex);
      }
    }

    #endregion

    #endregion
  }
}