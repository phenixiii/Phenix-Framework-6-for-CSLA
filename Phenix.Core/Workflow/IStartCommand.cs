namespace Phenix.Core.Workflow
{
  /// <summary>
  /// 工作流启动指令接口
  /// 与WorkflowIdentityAttribute配套使用
  /// </summary>
  public interface IStartCommand
  {
    /// <summary>
    /// 任务上下文
    /// </summary>
    TaskContext TaskContext { get; }
  }
}
