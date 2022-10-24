using System;
using System.Collections.Generic;
using Phenix.Core.Security;

namespace Phenix.Core.Workflow
{
  /// <summary>
  /// 工作流接口
  /// </summary>
  public interface IWorkflow
  {
    #region 属性

    /// <summary>
    /// 工作流资料队列
    /// </summary>
    IDictionary<string, WorkflowInfo> WorkflowInfos { get; }

    /// <summary>
    /// 工作流资料更新时间
    /// </summary>
    DateTime? WorkflowInfoChangedTime { get; }

    #endregion

    #region 方法

    #region WorkflowInfo

    /// <summary>
    /// 工作流资料已更新
    /// </summary>
    void WorkflowInfoHasChanged();

    /// <summary>
    /// 新增工作流资料
    /// </summary>
    void AddWorkflowInfo(string typeNamespace, string typeName, string caption, string xamlCode, UserIdentity identity);

    /// <summary>
    /// 禁用工作流资料
    /// </summary>
    void DisableWorkflowInfo(string typeNamespace, string typeName, UserIdentity identity);
    
    #endregion

    #region WorkflowInstance

    /// <summary>
    /// 保存工作流实例
    /// </summary>
    void SaveWorkflowInstance(Guid id, string typeNamespace, string typeName, string content, TaskContext taskContext);

    /// <summary>
    /// 检索工作流实例
    /// </summary>
    string FetchWorkflowInstance(Guid id);

    /// <summary>
    /// 清除工作流实例
    /// </summary>
    void ClearWorkflowInstance(Guid id);
    
    #endregion

    #region WorkflowTask

    /// <summary>
    /// 发布工作流任务
    /// </summary>
    void DispatchWorkflowTask(Guid id, string bookmarkName, 
      string pluginAssemblyName, string workerRole, string caption, string message, bool urgent);

    /// <summary>
    /// 收到工作流任务
    /// </summary>
    void ReceiveWorkflowTask(Guid id, string bookmarkName);

    /// <summary>
    /// 挂起工作流任务
    /// </summary>
    void HoldWorkflowTask(Guid id, string bookmarkName);

    /// <summary>
    /// 中断工作流任务
    /// </summary>
    void AbortWorkflowTask(Guid id, string bookmarkName);

    /// <summary>
    /// 继续工作流
    /// </summary>
    void ProceedWorkflow(WorkflowTaskInfo workflowTaskInfo);

    /// <summary>
    /// 完结工作流任务
    /// </summary>
    void CompleteWorkflowTask(Guid id, string bookmarkName);

    /// <summary>
    /// 检索工作流任务
    /// </summary>
    IList<WorkflowTaskInfo> FetchWorkflowTask(TaskState taskState, DateTime startDispatchTime, DateTime finishDispatchTime, UserIdentity identity);

    #endregion
    
    #endregion
  }
}
