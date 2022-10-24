using System;
using Phenix.Core.Operate;
using Phenix.Core.Rule;

namespace Phenix.Core.Workflow
{
  /// <summary>
  /// 任务状态
  /// </summary>
  [Flags]
  [Serializable]
  [KeyCaption(FriendlyName = "任务状态")]
  public enum TaskState : int
  {
    /// <summary>
    /// 已发送
    /// </summary>
    [EnumCaption("已发送")]
    Dispatch = 1,

    /// <summary>
    /// 已送达
    /// </summary>
    [EnumCaption("已送达")]
    Received = 2,

    /// <summary>
    /// 已挂起
    /// </summary>
    [EnumCaption("已挂起")]
    Holded = 4,
    
    /// <summary>
    /// 已中断
    /// </summary>
    [EnumCaption("已中断")]
    Aborted = 8,

    /// <summary>
    /// 已完结
    /// </summary>
    [EnumCaption("已完结")]
    Completed = 16,
  }
}
