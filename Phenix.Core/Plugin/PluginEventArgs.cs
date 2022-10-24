using System;

namespace Phenix.Core.Plugin
{
  /// <summary>
  /// 插件消息事件数据
  /// </summary>
  public sealed class PluginEventArgs : EventArgs
  {
    /// <summary>
    /// 初始化
    /// </summary>
    /// <param name="plugin">插件</param>
    public PluginEventArgs(IPlugin plugin)
      : base()
    {
      _plugin = plugin;
      _time = DateTime.Now;
    }

    /// <summary>
    /// 初始化
    /// </summary>
    /// <param name="plugin">插件</param>
    /// <param name="message">消息</param>
    public PluginEventArgs(IPlugin plugin, object message)
      : this(plugin)
    {
      _message = message;
    }

    /// <summary>
    /// 初始化
    /// </summary>
    /// <param name="plugin">插件</param>
    /// <param name="path">路径</param>
    public PluginEventArgs(IPlugin plugin, string path)
      : this(plugin)
    {
      _path = path;
    }

    #region 属性

    private readonly IPlugin _plugin;
    /// <summary>
    /// 插件
    /// </summary>
    public IPlugin Plugin
    {
      get { return _plugin; }
    }

    private readonly DateTime _time;
    /// <summary>
    /// 时间
    /// </summary>
    public DateTime Time
    {
      get { return _time; }
    }

    private readonly object _message;
    /// <summary>
    /// 消息
    /// </summary>
    public object Message
    {
      get { return _message; }
    }

    private readonly string _path;
    /// <summary>
    /// 路径
    /// </summary>
    public string Path
    {
      get { return _path; }
    }

    #endregion
  }
}