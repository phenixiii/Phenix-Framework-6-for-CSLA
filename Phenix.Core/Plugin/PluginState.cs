using System;
using Phenix.Core.Operate;
using Phenix.Core.Rule;

namespace Phenix.Core.Plugin
{
  /// <summary>
  /// 插件状态
  /// </summary>
  [Serializable]
  [KeyCaption(FriendlyName = "插件状态")]
  public enum PluginState
  {
    /// <summary>
    /// 构建
    /// </summary>
    [EnumCaption("构建")]
    Created,

    /// <summary>
    /// 初始化
    /// </summary>
    [EnumCaption("初始化")]
    Initialized,

    /// <summary>
    /// 终止化
    /// </summary>
    [EnumCaption("终止化")]
    Finalizing,

    /// <summary>
    /// 启动
    /// </summary>
    [EnumCaption("启动")]
    Started,

    /// <summary>
    /// 停止
    /// </summary>
    [EnumCaption("停止")]
    Suspended
  }
}
