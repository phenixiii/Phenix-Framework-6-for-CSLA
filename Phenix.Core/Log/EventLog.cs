using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Phenix.Core.Net;
using Phenix.Core.Security;

namespace Phenix.Core.Log
{
  /// <summary>
  /// 事件日志
  /// </summary>
  public static class EventLog
  {
    #region 属性

    private static bool _enabled = true;
    /// <summary>
    /// 需要保存日志?
    /// </summary>
    public static bool Enabled
    {
      get { return _enabled; }
      set { _enabled = value; }
    }

    private static bool? _mustSaveLog;
    /// <summary>
    /// 是否必须记录日志?
    /// </summary>
    public static bool MustSaveLog 
    {
      get
      {
        if (AppConfig.Debugging)
          return true;
        if (!_mustSaveLog.HasValue)
          _mustSaveLog = !AppConfig.AutoMode && NetConfig.ProxyType == ProxyType.Embedded;
        return _mustSaveLog.Value;
      } 
    }

    private const string DEF_EXTENSION = "log";

    private static readonly object _lock = new object();

    #endregion

    #region 方法

    /// <summary>
    /// 保存日志
    /// </summary>
    /// <param name="log">日志</param>
    public static void SaveLocal(string log)
    {
      SaveLocal(log, DEF_EXTENSION);
    }

    /// <summary>
    /// 保存错误日志
    /// </summary>
    /// <param name="log">日志</param>
    /// <param name="extension">后缀</param>
    public static void SaveLocal(string log, string extension)
    {
      if (!Enabled)
        return;
      try
      {
        lock (_lock)
        {
          string directory = Path.Combine(AppConfig.TempDirectory, DateTime.Today.ToString("yyyyMMdd"));
          if (!Directory.Exists(directory))
            Directory.CreateDirectory(directory);
          using (StreamWriter logFile = File.AppendText(Path.Combine(directory, String.Format("{0}Log.{1}", AppSettings.DefaultKey, extension))))
          {
            logFile.WriteLine("{0} {1}", DateTime.Now, log);
            logFile.WriteLine();
          }
        }
      }
      catch (IOException)
      {
        SaveLocal(log, String.Format("{0}.{1}", extension, Guid.NewGuid().ToString("N").Substring(20)));
      }
    }

    /// <summary>
    /// 保存错误日志
    /// </summary>
    /// <param name="log">日志</param>
    /// <param name="error">错误</param>
    public static void SaveLocal(string log, Exception error)
    {
      SaveLocal(log, error, DEF_EXTENSION);
    }

    /// <summary>
    /// 保存错误日志
    /// </summary>
    /// <param name="log">日志</param>
    /// <param name="error">错误</param>
    /// <param name="extension">后缀</param>
    public static void SaveLocal(string log, Exception error, string extension)
    {
      SaveLocal(error != null ? String.Format("{0} : {1}[{2}]", log, AppUtilities.GetErrorMessage(error), error.StackTrace) : log, extension);
    }

    /// <summary>
    /// 保存对象消息
    /// </summary>
    /// <param name="objectType">类</param>
    /// <param name="log">日志</param>
    public static void SaveLocal(Type objectType, string log)
    {
      Save((IIdentity)null, objectType, log);
    }
    
    /// <summary>
    /// 保存对象消息
    /// </summary>
    /// <param name="objectType">类</param>
    /// <param name="log">日志</param>
    public static void Save(Type objectType, string log)
    {
      Save(UserIdentity.CurrentIdentity, objectType, log);
    }

    /// <summary>
    /// 保存对象消息
    /// </summary>
    /// <param name="user">登录用户</param>
    /// <param name="objectType">类</param>
    /// <param name="log">日志</param>
    public static void Save(IPrincipal user, Type objectType, string log)
    {
      Save(user != null ? user.Identity : null, objectType, log);
    }

    /// <summary>
    /// 保存对象消息
    /// </summary>
    /// <param name="identity">用户身份</param>
    /// <param name="objectType">类</param>
    /// <param name="log">日志</param>
    public static void Save(IIdentity identity, Type objectType, string log)
    {
      if (objectType == null)
        throw new ArgumentNullException("objectType");
      if (AppConfig.Debugging || identity == null || identity.IsAdmin)
        SaveLocal(String.Format("{0}: {1}", objectType.FullName, log));
      else
        PermanentLogHub.Save(identity, objectType, log);
    }

    /// <summary>
    /// 保存对象消息
    /// </summary>
    /// <param name="objectType">类</param>
    /// <param name="error">错误</param>
    public static void SaveLocal(Type objectType, Exception error)
    {
      Save((IIdentity)null, objectType, error);
    }

    /// <summary>
    /// 保存对象消息
    /// </summary>
    /// <param name="objectType">类</param>
    /// <param name="error">错误</param>
    public static void Save(Type objectType, Exception error)
    {
      Save(UserIdentity.CurrentIdentity, objectType, error);
    }

    /// <summary>
    /// 保存对象消息
    /// </summary>
    /// <param name="user">登录用户</param>
    /// <param name="objectType">类</param>
    /// <param name="error">错误</param>
    public static void Save(IPrincipal user, Type objectType, Exception error)
    {
      Save(user != null ? user.Identity : null, objectType, error);
    }

    /// <summary>
    /// 保存对象消息
    /// </summary>
    /// <param name="identity">用户身份</param>
    /// <param name="objectType">类</param>
    /// <param name="error">错误</param>
    public static void Save(IIdentity identity, Type objectType, Exception error)
    {
      if (objectType == null)
        throw new ArgumentNullException("objectType");
      if (AppConfig.Debugging || identity == null || identity.IsAdmin)
        SaveLocal(objectType.FullName, error);
      else
        PermanentLogHub.Save(identity, objectType, null, error);
    }

    /// <summary>
    /// 保存对象消息
    /// </summary>
    /// <param name="method">函数的信息</param>
    /// <param name="log">日志</param>
    public static void SaveLocal(MethodBase method, string log)
    {
      Save((IIdentity)null, method, log);
    }

    /// <summary>
    /// 保存对象消息
    /// </summary>
    /// <param name="method">函数的信息</param>
    /// <param name="log">日志</param>
    public static void Save(MethodBase method, string log)
    {
      Save(UserIdentity.CurrentIdentity, method, log);
    }

    /// <summary>
    /// 保存对象消息
    /// </summary>
    /// <param name="user">登录用户</param>
    /// <param name="method">函数的信息</param>
    /// <param name="log">日志</param>
    public static void Save(IPrincipal user, MethodBase method, string log)
    {
      Save(user != null ? user.Identity : null, method, log);
    }

    /// <summary>
    /// 保存对象消息
    /// </summary>
    /// <param name="identity">用户身份</param>
    /// <param name="method">函数的信息</param>
    /// <param name="log">日志</param>
    public static void Save(IIdentity identity, MethodBase method, string log)
    {
      if (method == null)
        throw new ArgumentNullException("method");
      Save(identity, method.DeclaringType, method, log);
    }

    /// <summary>
    /// 保存对象消息
    /// </summary>
    /// <param name="objectType">类</param>
    /// <param name="method">函数的信息</param>
    /// <param name="log">日志</param>
    public static void SaveLocal(Type objectType, MethodBase method, string log)
    {
      Save((IIdentity)null, objectType, method, log);
    }

    /// <summary>
    /// 保存对象消息
    /// </summary>
    /// <param name="objectType">类</param>
    /// <param name="method">函数的信息</param>
    /// <param name="log">日志</param>
    public static void Save(Type objectType, MethodBase method, string log)
    {
      Save(UserIdentity.CurrentIdentity, objectType, method, log);
    }

    /// <summary>
    /// 保存对象消息
    /// </summary>
    /// <param name="user">登录用户</param>
    /// <param name="objectType">类</param>
    /// <param name="method">函数的信息</param>
    /// <param name="log">日志</param>
    public static void Save(IPrincipal user, Type objectType, MethodBase method, string log)
    {
      Save(user != null ? user.Identity : null, objectType, method, log);
    }

    /// <summary>
    /// 保存对象消息
    /// </summary>
    /// <param name="identity">用户身份</param>
    /// <param name="objectType">类</param>
    /// <param name="method">函数的信息</param>
    /// <param name="log">日志</param>
    public static void Save(IIdentity identity, Type objectType, MethodBase method, string log)
    {
      if (objectType == null)
        throw new ArgumentNullException("objectType");
      if (method == null)
        throw new ArgumentNullException("method");
      if (AppConfig.Debugging || identity == null || identity.IsAdmin)
        SaveLocal(String.Format("{0}.{1}: {2}", objectType.FullName, method.Name, log));
      else
        PermanentLogHub.Save(identity, objectType, String.Format("{0}: {1}", method.Name, log));
    }

    /// <summary>
    /// 保存对象消息
    /// </summary>
    /// <param name="method">函数的信息</param>
    /// <param name="error">错误</param>
    public static void SaveLocal(MethodBase method, Exception error)
    {
      Save((IIdentity)null, method, error);
    }

    /// <summary>
    /// 保存对象消息
    /// </summary>
    /// <param name="method">函数的信息</param>
    /// <param name="error">错误</param>
    public static void Save(MethodBase method, Exception error)
    {
      Save(UserIdentity.CurrentIdentity, method, error);
    }

    /// <summary>
    /// 保存对象消息
    /// </summary>
    /// <param name="user">登录用户</param>
    /// <param name="method">函数的信息</param>
    /// <param name="error">错误</param>
    public static void Save(IPrincipal user, MethodBase method, Exception error)
    {
      Save(user != null ? user.Identity : null, method, error);
    }

    /// <summary>
    /// 保存对象消息
    /// </summary>
    /// <param name="identity">用户身份</param>
    /// <param name="method">函数的信息</param>
    /// <param name="error">错误</param>
    public static void Save(IIdentity identity, MethodBase method, Exception error)
    {
      if (method == null)
        throw new ArgumentNullException("method");
      Save(identity, method.DeclaringType, method, error);
    }

    /// <summary>
    /// 保存对象消息
    /// </summary>
    /// <param name="objectType">类</param>
    /// <param name="method">函数的信息</param>
    /// <param name="error">错误</param>
    public static void SaveLocal(Type objectType, MethodBase method, Exception error)
    {
      Save((IIdentity)null, objectType, method, error);
    }

    /// <summary>
    /// 保存对象消息
    /// </summary>
    /// <param name="objectType">类</param>
    /// <param name="method">函数的信息</param>
    /// <param name="error">错误</param>
    public static void Save(Type objectType, MethodBase method, Exception error)
    {
      Save(UserIdentity.CurrentIdentity, objectType, method, error);
    }

    /// <summary>
    /// 保存对象消息
    /// </summary>
    /// <param name="user">登录用户</param>
    /// <param name="objectType">类</param>
    /// <param name="method">函数的信息</param>
    /// <param name="error">错误</param>
    public static void Save(IPrincipal user, Type objectType, MethodBase method, Exception error)
    {
      Save(user != null ? user.Identity : null, objectType, method, error);
    }

    /// <summary>
    /// 保存对象消息
    /// </summary>
    /// <param name="identity">用户身份</param>
    /// <param name="objectType">类</param>
    /// <param name="method">函数的信息</param>
    /// <param name="error">错误</param>
    public static void Save(IIdentity identity, Type objectType, MethodBase method, Exception error)
    {
      if (objectType == null)
        throw new ArgumentNullException("objectType");
      if (method == null)
        throw new ArgumentNullException("method");
      if (AppConfig.Debugging || identity == null || identity.IsAdmin)
        SaveLocal(String.Format("{0}.{1}", objectType.FullName, method.Name), error);
      else
        PermanentLogHub.Save(identity, objectType, method.Name, error);
    }

    /// <summary>
    /// 保存对象消息
    /// </summary>
    /// <param name="method">函数的信息</param>
    /// <param name="log">日志</param>
    /// <param name="error">错误</param>
    public static void SaveLocal(MethodBase method, string log, Exception error)
    {
      Save((IIdentity)null, method, log, error);
    }

    /// <summary>
    /// 保存对象消息
    /// </summary>
    /// <param name="method">函数的信息</param>
    /// <param name="log">日志</param>
    /// <param name="error">错误</param>
    public static void Save(MethodBase method, string log, Exception error)
    {
      Save(UserIdentity.CurrentIdentity, method, log, error);
    }

    /// <summary>
    /// 保存对象消息
    /// </summary>
    /// <param name="user">登录用户</param>
    /// <param name="method">函数的信息</param>
    /// <param name="log">日志</param>
    /// <param name="error">错误</param>
    public static void Save(IPrincipal user, MethodBase method, string log, Exception error)
    {
      Save(user != null ? user.Identity : null, method, log, error);
    }

    /// <summary>
    /// 保存对象消息
    /// </summary>
    /// <param name="identity">用户身份</param>
    /// <param name="method">函数的信息</param>
    /// <param name="log">日志</param>
    /// <param name="error">错误</param>
    public static void Save(IIdentity identity, MethodBase method, string log, Exception error)
    {
      if (method == null)
        throw new ArgumentNullException("method");
      Save(identity, method.DeclaringType, method, log, error);
    }

    /// <summary>
    /// 保存对象消息
    /// </summary>
    /// <param name="objectType">类</param>
    /// <param name="method">函数的信息</param>
    /// <param name="log">日志</param>
    /// <param name="error">错误</param>
    public static void SaveLocal(Type objectType, MethodBase method, string log, Exception error)
    {
      Save((IIdentity)null, objectType, method, log, error);
    }

    /// <summary>
    /// 保存对象消息
    /// </summary>
    /// <param name="objectType">类</param>
    /// <param name="method">函数的信息</param>
    /// <param name="log">日志</param>
    /// <param name="error">错误</param>
    public static void Save(Type objectType, MethodBase method, string log, Exception error)
    {
      Save(UserIdentity.CurrentIdentity, objectType, method, log, error);
    }

    /// <summary>
    /// 保存对象消息
    /// </summary>
    /// <param name="user">登录用户</param>
    /// <param name="objectType">类</param>
    /// <param name="method">函数的信息</param>
    /// <param name="log">日志</param>
    /// <param name="error">错误</param>
    public static void Save(IPrincipal user, Type objectType, MethodBase method, string log, Exception error)
    {
      Save(user != null ? user.Identity : null, objectType, method, log, error);
    }

    /// <summary>
    /// 保存对象消息
    /// </summary>
    /// <param name="identity">用户身份</param>
    /// <param name="objectType">类</param>
    /// <param name="method">函数的信息</param>
    /// <param name="log">日志</param>
    /// <param name="error">错误</param>
    public static void Save(IIdentity identity, Type objectType, MethodBase method, string log, Exception error)
    {
      if (objectType == null)
        throw new ArgumentNullException("objectType");
      if (method == null)
        throw new ArgumentNullException("method");
      if (AppConfig.Debugging || identity == null || identity.IsAdmin)
        SaveLocal(String.Format("{0}.{1}: {2}", objectType.FullName, method.Name, log), error);
      else
        PermanentLogHub.Save(identity, objectType, String.Format("{0}: {1}", method.Name, log), error);
    }

    /// <summary>
    /// 检索日志消息
    /// userNumber = null
    /// objectType = null
    /// </summary>
    /// <param name="startTime">起始时间</param>
    /// <param name="finishTime">结束时间</param>
    /// <returns>日志队列</returns>
    public static IList<EventLogInfo> Fetch(DateTime startTime, DateTime finishTime)
    {
      return PermanentLogHub.Fetch(null, null as Type, startTime, finishTime);
    }

    /// <summary>
    /// 检索日志消息
    /// objectType = null
    /// </summary>
    /// <param name="userNumber">登录工号, null代表全部</param>
    /// <param name="startTime">起始时间</param>
    /// <param name="finishTime">结束时间</param>
    /// <returns>日志队列</returns>
    public static IList<EventLogInfo> Fetch(string userNumber, DateTime startTime, DateTime finishTime)
    {
      return PermanentLogHub.Fetch(userNumber, null as Type, startTime, finishTime);
    }

    /// <summary>
    /// 检索日志消息
    /// </summary>
    /// <param name="userNumber">登录工号, null代表全部</param>
    /// <param name="objectType">类, null代表全部</param>
    /// <param name="startTime">起始时间</param>
    /// <param name="finishTime">结束时间</param>
    /// <returns>日志队列</returns>
    public static IList<EventLogInfo> Fetch(string userNumber, Type objectType, DateTime startTime, DateTime finishTime)
    {
      return PermanentLogHub.Fetch(userNumber, objectType, startTime, finishTime);
    }

    /// <summary>
    /// 清除日志消息
    /// userNumber = null
    /// objectType = null
    /// </summary>
    /// <param name="startTime">起始时间</param>
    /// <param name="finishTime">结束时间</param>
    public static void Clear(DateTime startTime, DateTime finishTime)
    {
      PermanentLogHub.Clear(null, null as Type, startTime, finishTime);
    }

    /// <summary>
    /// 清除日志消息
    /// objectType = null
    /// </summary>
    /// <param name="userNumber">登录工号, null代表全部</param>
    /// <param name="startTime">起始时间</param>
    /// <param name="finishTime">结束时间</param>
    public static void Clear(string userNumber, DateTime startTime, DateTime finishTime)
    {
      PermanentLogHub.Clear(userNumber, null as Type, startTime, finishTime);
    }

    /// <summary>
    /// 清除日志消息
    /// </summary>
    /// <param name="userNumber">登录工号, null代表全部</param>
    /// <param name="objectType">类, null代表全部</param>
    /// <param name="startTime">起始时间</param>
    /// <param name="finishTime">结束时间</param>
    public static void Clear(string userNumber, Type objectType, DateTime startTime, DateTime finishTime)
    {
      PermanentLogHub.Clear(userNumber, objectType, startTime, finishTime);
    }

    #endregion
  }
}