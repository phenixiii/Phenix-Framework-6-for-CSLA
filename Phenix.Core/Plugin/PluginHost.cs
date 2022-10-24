#if Top
using System.Threading.Tasks;
#endif

using System;
using System.ComponentModel;
using System.Drawing;
using System.Reflection;
using System.Security.Permissions;
using Phenix.Core.Log;
using Phenix.Core.Reflection;
using Phenix.Core.SyncCollections;

namespace Phenix.Core.Plugin
{
  /// <summary>
  /// 插件容器组件
  /// </summary>
  [Description("插件容器")]
  [ToolboxItem(true), ToolboxBitmap(typeof(PluginHost), "Phenix.Core.Plugin.PluginHost")]
  public sealed class PluginHost : Component
  {
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1810:InitializeReferenceTypeStaticFieldsInline")]
    static PluginHost()
    {
      StartListenAssemblyLoadEvent();
    }

    /// <summary>
    /// 初始化
    /// </summary>
    public PluginHost()
      : base() { }

    /// <summary>
    /// 初始化
    /// </summary>
    public PluginHost(IContainer container)
      : base()
    {
      if (container == null)
        throw new ArgumentNullException("container");
      container.Add(this);
    }

    #region 单例

    private static readonly object _defaultLock = new object();
    private static PluginHost _default;
    /// <summary>
    /// 单例
    /// </summary>
    public static PluginHost Default
    {
      get
      {
        if (_default == null)
          lock (_defaultLock)
            if (_default == null)
            {
              _default = new PluginHost();
            }
        return _default;
      }
    }

    #endregion

    #region 属性

    private static readonly SynchronizedDictionary<string, Type> _typeCache =
      new SynchronizedDictionary<string, Type>(StringComparer.Ordinal);

    private readonly SynchronizedDictionary<string, IPlugin> _singletonCache =
      new SynchronizedDictionary<string, IPlugin>(StringComparer.Ordinal);

    private static readonly SynchronizedDictionary<string, PluginInfo> _pluginInfoCache =
      new SynchronizedDictionary<string, PluginInfo>(StringComparer.Ordinal);

    #endregion

    #region 事件

    private static void CurrentDomain_AssemblyLoad(object sender, AssemblyLoadEventArgs e)
    {
      Type type = GetType(e.LoadedAssembly);
      if (type != null)
        RegisterType(type);
    }

    /// <summary>
    /// 插件消息
    /// </summary>
    [Description("当收到插件消息时被触发"), Category("Behavior")]
    public event EventHandler<PluginEventArgs> Message;
    private void OnMessage(PluginEventArgs e)
    {
      if (Message != null)
        Message(this, e);
    }

    /// <summary>
    /// 插件终止
    /// </summary>
    [Description("当插件终止时被触发"), Category("Behavior")]
    public event EventHandler<PluginEventArgs> Finalized;
    private void OnFinalized(PluginEventArgs e)
    {
      if (Finalized != null)
        Finalized(this, e);
    }

    #endregion

    #region 方法

    [EnvironmentPermission(SecurityAction.Demand, Unrestricted = true)]
    private static void StartListenAssemblyLoadEvent()
    {
      AppDomain.CurrentDomain.AssemblyLoad += new AssemblyLoadEventHandler(CurrentDomain_AssemblyLoad);
    }

    private static Assembly LoadAssembly(string assemblyName)
    {
      Assembly assembly = Utilities.LoadAssembly(assemblyName);
      if (assembly == null)
        throw new InvalidOperationException(assemblyName + "文件不存在");
      return assembly;
    }

    /// <summary>
    /// 注册类
    /// </summary>
    /// <param name="assemblyName">程序集名</param>
    /// <param name="typeName">插件类名</param>
    /// <returns>Id</returns>
    public static string RegisterType(string assemblyName, string typeName)
    {
      Type type = LoadAssembly(assemblyName).GetType(typeName, false);
      if (type == null)
        throw new InvalidOperationException(assemblyName + "未找到类定义" + typeName);
      return RegisterType(type);
    }

    /// <summary>
    /// 注册类
    /// </summary>
    /// <param name="assemblyName">程序集名</param>
    /// <returns>Id</returns>
    public static string RegisterType(string assemblyName)
    {
      Type type = GetType(assemblyName);
      if (type == null)
        throw new InvalidOperationException(assemblyName + "未找到 IPlugin 接口");
      return RegisterType(type);
    }

    /// <summary>
    /// 注册类
    /// </summary>
    /// <param name="assembly">程序集</param>
    /// <returns>Id</returns>
    public static string RegisterType(Assembly assembly)
    {
      if (assembly == null)
        throw new ArgumentNullException("assembly");
      Type type = GetType(assembly);
      if (type == null)
        throw new InvalidOperationException(assembly.FullName + "未找到 IPlugin 接口");
      return RegisterType(type);
    }

    /// <summary>
    /// 注册类
    /// </summary>
    /// <param name="type">类</param>
    /// <returns>Id</returns>
    public static string RegisterType(Type type)
    {
      if (type == null)
        throw new ArgumentNullException("type");
      PluginInfo pluginInfo = GetPluginInfo(type, true);
      _typeCache[pluginInfo.Key] = type;
      return pluginInfo.Key;
    }

    /// <summary>
    /// 取插件类
    /// </summary>
    /// <param name="assemblyName">程序集名</param>
    public static Type GetType(string assemblyName)
    {
      return GetType(LoadAssembly(assemblyName));
    }

    /// <summary>
    /// 取插件类
    /// </summary>
    /// <param name="assembly">程序集</param>
    public static Type GetType(Assembly assembly)
    {
      if (assembly == null)
        throw new ArgumentNullException("assembly");
      foreach (Type item in assembly.GetExportedTypes())
      {
        if (!item.IsClass || item.IsAbstract || item.IsGenericType || item.IsCOMObject)
          continue;
        if (typeof(IPlugin).IsAssignableFrom(item))
          return item;
      }
      return null;
    }

    /// <summary>
    /// 查找插件类
    /// </summary>
    /// <param name="key">PluginAttribute.Key</param>
    /// <returns>类</returns>
    public static Type FindType(string key)
    {
      if (!String.IsNullOrEmpty(key))
      {
        Type result;
        if (_typeCache.TryGetValue(key, out result))
          return result;
      }
      return null;
    }

    internal void DoMessage(PluginEventArgs e)
    {
      OnMessage(e);
    }

    internal void DoFinalized(PluginEventArgs e)
    {
      _singletonCache.Remove(e.Plugin.Key);
      OnFinalized(e);
    }

    #region PluginInfo

    /// <summary>
    /// 取插件信息
    /// </summary>
    public static PluginInfo GetPluginInfo(Type type, bool throwIfNotFound)
    {
      PluginInfo result = _pluginInfoCache.GetValue(type.FullName, () =>
      {
        if (!typeof(IPlugin).IsAssignableFrom(type))
          throw new InvalidOperationException(String.Format("请为类{0}实现{1}接口", type.FullName, typeof(IPlugin).FullName));
        return PluginInfo.Fetch(type);
      }, true);
      if (throwIfNotFound && result == null)
        throw new InvalidOperationException(String.Format("请为{0}类标记上{1}标签", type.FullName, typeof(PluginAttribute).FullName));
      return result;
    }

    /// <summary>
    /// 取插件信息
    /// </summary>
    public static PluginInfo GetPluginInfo(IPlugin plugin, bool throwIfNotFound)
    {
      return GetPluginInfo(plugin.GetType(), throwIfNotFound);
    }

    #endregion

    #region CreatePlugin

    private IPlugin DoCreatePlugin(string key, bool isSingleton)
    {
      Type objectType = FindType(key);
      if (objectType != null)
        try
        {
          IPlugin result = PluginBase.New(objectType, this, key);
          if (isSingleton)
            _singletonCache[key] = result;
          return result;
        }
        catch (Exception ex)
        {
          EventLog.SaveLocal(MethodBase.GetCurrentMethod(), key, ex);
          throw;
        }
      return null;
    }

    /// <summary>
    /// 构建插件
    /// </summary>
    /// <param name="assemblyName">程序集名</param>
    /// <param name="typeName">插件类名</param>
    /// <param name="isSingleton">单例</param>
    public IPlugin CreatePlugin(string assemblyName, string typeName, bool isSingleton)
    {
      return DoCreatePlugin(RegisterType(assemblyName, typeName), isSingleton);
    }

    /// <summary>
    /// 构建插件
    /// </summary>
    /// <param name="assemblyName">程序集名</param>
    /// <param name="isSingleton">单例</param>
    public IPlugin CreatePlugin(string assemblyName, bool isSingleton)
    {
      return DoCreatePlugin(RegisterType(assemblyName), isSingleton);
    }

    /// <summary>
    /// 构建插件
    /// </summary>
    /// <param name="assembly">程序集</param>
    /// <param name="isSingleton">是单例</param>
    public IPlugin CreatePlugin(Assembly assembly, bool isSingleton)
    {
      return DoCreatePlugin(RegisterType(assembly), isSingleton);
    }

    #endregion

    #region FindSingleton

    private IPlugin DoFindSingleton(string key)
    {
      IPlugin result;
      if (_singletonCache.TryGetValue(key, out result))
        return result;
      return null;
    }

    /// <summary>
    /// 查找单例插件
    /// </summary>
    /// <param name="assemblyName">程序集名</param>
    /// <param name="typeName">插件类名</param>
    public IPlugin FindSingleton(string assemblyName, string typeName)
    {
      return DoFindSingleton(RegisterType(assemblyName, typeName));
    }

    /// <summary>
    /// 查找单例插件
    /// </summary>
    /// <param name="assemblyName">程序集名</param>
    public IPlugin FindSingleton(string assemblyName)
    {
      return DoFindSingleton(RegisterType(assemblyName));
    }

    #endregion

    #region InitializeSingleton

    private void DoInitializeSingleton(string key)
    {
      try
      {
        IPlugin plugin = DoFindSingleton(key);
        if (plugin != null)
          plugin.Initialization();
      }
      catch (Exception ex)
      {
        EventLog.SaveLocal(MethodBase.GetCurrentMethod(), key, ex);
        throw;
      }
    }

    /// <summary>
    /// 初始化单例插件
    /// </summary>
    /// <param name="assemblyName">程序集名</param>
    /// <param name="typeName">插件类名</param>
    public void InitializeSingleton(string assemblyName, string typeName)
    {
      DoInitializeSingleton(RegisterType(assemblyName, typeName));
    }

    /// <summary>
    /// 初始化单例插件
    /// </summary>
    /// <param name="assemblyName">程序集名</param>
    public void InitializeSingleton(string assemblyName)
    {
      DoInitializeSingleton(RegisterType(assemblyName));
    }

    /// <summary>
    /// 初始化单例插件
    /// </summary>
    /// <param name="assembly">程序集</param>
    public void InitializeSingleton(Assembly assembly)
    {
      DoInitializeSingleton(RegisterType(assembly));
    }

    #endregion

    #region FinalizeSingleton

    private void DoFinalizeSingleton(string key)
    {
      try
      {
        IPlugin plugin = DoFindSingleton(key);
        if (plugin != null)
          plugin.Finalization();
      }
      catch (Exception ex)
      {
        EventLog.SaveLocal(MethodBase.GetCurrentMethod(), key, ex);
        throw;
      }
    }

    /// <summary>
    /// 终止化单例插件
    /// </summary>
    /// <param name="assemblyName">程序集名</param>
    /// <param name="typeName">插件类名</param>
    public void FinalizeSingleton(string assemblyName, string typeName)
    {
      DoFinalizeSingleton(RegisterType(assemblyName, typeName));
    }

    /// <summary>
    /// 终止化单例插件
    /// </summary>
    /// <param name="assemblyName">程序集名</param>
    public void FinalizeSingleton(string assemblyName)
    {
      DoFinalizeSingleton(RegisterType(assemblyName));
    }

    #endregion

    #region SetupSingleton

    private object DoSetupSingleton(string key, object sender)
    {
      try
      {
        IPlugin plugin = DoFindSingleton(key);
        if (plugin != null)
          return plugin.Setup(sender);
      }
      catch (Exception ex)
      {
        EventLog.SaveLocal(MethodBase.GetCurrentMethod(), key, ex);
        throw;
      }
      return false;
    }

    /// <summary>
    /// 设置单例插件
    /// </summary>
    /// <param name="assemblyName">程序集名</param>
    /// <param name="typeName">插件类名</param>
    /// <param name="sender">发起对象</param>
    /// <returns>按需返回</returns>
    public object SetupSingleton(string assemblyName, string typeName, object sender)
    {
      return DoSetupSingleton(RegisterType(assemblyName, typeName), sender);
    }

    /// <summary>
    /// 启动单例插件
    /// </summary>
    /// <param name="assemblyName">程序集名</param>
    /// <param name="sender">发起对象</param>
    /// <returns>按需返回</returns>
    public object SetupSingleton(string assemblyName, object sender)
    {
      return DoSetupSingleton(RegisterType(assemblyName), sender);
    }

    #endregion

    #region StartSingleton

    private bool DoStartSingleton(string key)
    {
      try
      {
        IPlugin plugin = DoFindSingleton(key);
        if (plugin != null)
          return plugin.Start();
      }
      catch (Exception ex)
      {
        EventLog.SaveLocal(MethodBase.GetCurrentMethod(), key, ex);
        throw;
      }
      return false;
    }

    /// <summary>
    /// 启动单例插件
    /// </summary>
    /// <param name="assemblyName">程序集名</param>
    /// <param name="typeName">插件类名</param>
    /// <returns>确定启动</returns>
    public bool StartSingleton(string assemblyName, string typeName)
    {
      return DoStartSingleton(RegisterType(assemblyName, typeName));
    }

    /// <summary>
    /// 启动单例插件
    /// </summary>
    /// <param name="assemblyName">程序集名</param>
    /// <returns>确定启动</returns>
    public bool StartSingleton(string assemblyName)
    {
      return DoStartSingleton(RegisterType(assemblyName));
    }

    #endregion

    #region SuspendSingleton

    private bool DoSuspendSingleton(string key)
    {
      try
      {
        IPlugin plugin = DoFindSingleton(key);
        if (plugin != null)
          return plugin.Suspend();
      }
      catch (Exception ex)
      {
        EventLog.SaveLocal(MethodBase.GetCurrentMethod(), key, ex);
        throw;
      }
      return false;
    }

    /// <summary>
    /// 停止单例插件
    /// </summary>
    /// <param name="assemblyName">程序集名</param>
    /// <param name="typeName">插件类名</param>
    /// <returns>确定停止</returns>
    public bool SuspendSingleton(string assemblyName, string typeName)
    {
      return DoSuspendSingleton(RegisterType(assemblyName, typeName));
    }

    /// <summary>
    /// 停止单例插件
    /// </summary>
    /// <param name="assemblyName">程序集名</param>
    /// <returns>确定停止</returns>
    public bool SuspendSingleton(string assemblyName)
    {
      return DoSuspendSingleton(RegisterType(assemblyName));
    }

    #endregion

    #region SendSingletonMessage

    /// <summary>
    /// 发送给单例插件消息
    /// </summary>
    /// <param name="assemblyName">程序集名</param>
    /// <param name="typeName">插件类名</param>
    /// <param name="message">消息</param>
    /// <returns>按需返回</returns>
    public object SendSingletonMessage(string assemblyName, string typeName, object message)
    {
      try
      {
        IPlugin plugin = FindSingleton(assemblyName, typeName) ?? CreatePlugin(assemblyName, typeName, true);
        if (plugin != null)
          return plugin.AnalyseMessage(message);
      }
      catch (Exception ex)
      {
        EventLog.SaveLocal(MethodBase.GetCurrentMethod(), assemblyName + '.' + typeName, ex);
        throw;
      }
      return null;
    }

    /// <summary>
    /// 发送给单例插件消息
    /// </summary>
    /// <param name="assemblyName">程序集名</param>
    /// <param name="message">消息</param>
    /// <returns>按需返回</returns>
    public object SendSingletonMessage(string assemblyName, object message)
    {
      try
      {
        IPlugin plugin = FindSingleton(assemblyName) ?? CreatePlugin(assemblyName, true);
        if (plugin != null)
          return plugin.AnalyseMessage(message);
      }
      catch (Exception ex)
      {
        EventLog.SaveLocal(MethodBase.GetCurrentMethod(), assemblyName, ex);
        throw;
      }
      return null;
    }

#if Top

    /// <summary>
    /// 发送给单例插件消息(异步)
    /// </summary>
    /// <param name="assemblyName">程序集名</param>
    /// <param name="typeName">插件类名</param>
    /// <param name="message">消息</param>
    /// <returns>按需返回</returns>
    public async Task<object> SendSingletonMessageAsync(string assemblyName, string typeName, object message)
    {
      try
      {
        IPlugin plugin = FindSingleton(assemblyName, typeName) ?? CreatePlugin(assemblyName, typeName, true);
        if (plugin != null)
          return await plugin.AnalyseMessageAsync(message);
      }
      catch (Exception ex)
      {
        EventLog.SaveLocal(MethodBase.GetCurrentMethod(), assemblyName + '.' + typeName, ex);
        throw;
      }
      return null;
    }

    /// <summary>
    /// 发送给单例插件消息(异步)
    /// </summary>
    /// <param name="assemblyName">程序集名</param>
    /// <param name="message">消息</param>
    /// <returns>按需返回</returns>
    public async Task<object> SendSingletonMessageAsync(string assemblyName, object message)
    {
      try
      {
        IPlugin plugin = FindSingleton(assemblyName) ?? CreatePlugin(assemblyName, true);
        if (plugin != null)
          return await plugin.AnalyseMessageAsync(message);
      }
      catch (Exception ex)
      {
        EventLog.SaveLocal(MethodBase.GetCurrentMethod(), assemblyName, ex);
        throw;
      }
      return null;
    }

#endif

    #endregion

    #endregion
  }
}