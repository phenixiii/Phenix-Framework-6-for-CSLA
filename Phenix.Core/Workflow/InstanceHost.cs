using System;
using System.Activities;
using System.Activities.Hosting;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using Phenix.Core.SyncCollections;

namespace Phenix.Core.Workflow
{
  /// <summary>
  /// 工作流实体容器
  /// </summary>
  [Description("工作流实体容器")]
  [ToolboxItem(true), ToolboxBitmap(typeof(InstanceHost), "Phenix.Core.Workflow.InstanceHost")]
  public sealed class InstanceHost : Component
  {
    /// <summary>
    /// 初始化
    /// </summary>
    public InstanceHost()
      : base() { }

    /// <summary>
    /// 初始化
    /// </summary>
    public InstanceHost(IContainer container)
      : base()
    {
      if (container == null)
        throw new ArgumentNullException("container");
      container.Add(this);
    }

    #region 单例

    private static readonly object _defaultLock = new object();
    private static InstanceHost _default;
    /// <summary>
    /// 单例
    /// </summary>
    public static InstanceHost Default
    {
      get
      {
        if (_default == null)
          lock (_defaultLock)
            if (_default == null)
            {
              _default = new InstanceHost();
            }
        return _default;
      }
    }

    #endregion

    #region 属性

    private readonly SynchronizedDictionary<Guid, WorkflowApplication> _instances =
      new SynchronizedDictionary<Guid, WorkflowApplication>();

    #endregion

    /// <summary>
    /// 构建并执行实体
    /// </summary>
    public WorkflowApplication CreateAndRun(IStartCommand command)
    {
      return CreateAndRun(command, null);
    }
    
    /// <summary>
    /// 构建并执行实体
    /// </summary>
    public WorkflowApplication CreateAndRun(IStartCommand command, IDictionary<string, object> inputs)
    {
      WorkflowApplication result = inputs == null || inputs.Count == 0 
        ? new WorkflowApplication(WorkflowHub.GetWorkflowInfo(command, true).ActivityDefinition)
        : new WorkflowApplication(WorkflowHub.GetWorkflowInfo(command, true).ActivityDefinition, inputs);
      result.Idle = OnIdle;
      result.PersistableIdle = OnIdleAndPersistable;
      if (command.TaskContext.NeedLeaveTrace)
        result.Completed = OnWorkflowCompleted;
      else
        result.Completed = OnWorkflowCompletedNeedClearWorkflowInstance;
      result.InstanceStore = new InstanceStore(result.Id, command);
      _instances[result.Id] = result;
      result.Run();
      return result;
    }

    /// <summary>
    /// 加载实体
    /// </summary>
    public WorkflowApplication LoadInstance(WorkflowTaskInfo workflowTaskInfo)
    {
      return LoadInstance(workflowTaskInfo, null);
    }

    /// <summary>
    /// 加载实体
    /// </summary>
    public WorkflowApplication LoadInstance(WorkflowTaskInfo workflowTaskInfo, IDictionary<string, object> inputs)
    {
      WorkflowApplication result;
      if (!_instances.TryGetValue(workflowTaskInfo.WorkflowInstanceId, out result))
      {
        result = inputs == null || inputs.Count == 0
          ? new WorkflowApplication(WorkflowHub.GetWorkflowInfo(workflowTaskInfo, true).ActivityDefinition)
          : new WorkflowApplication(WorkflowHub.GetWorkflowInfo(workflowTaskInfo, true).ActivityDefinition, inputs);
        result.Idle = OnIdle;
        result.PersistableIdle = OnIdleAndPersistable;
        if (workflowTaskInfo.TaskContext.NeedLeaveTrace)
          result.Completed = OnWorkflowCompleted;
        else
          result.Completed = OnWorkflowCompletedNeedClearWorkflowInstance;
        result.InstanceStore = new InstanceStore(workflowTaskInfo);
        result.Load(workflowTaskInfo.WorkflowInstanceId);
        _instances[workflowTaskInfo.WorkflowInstanceId] = result;
      }
      else
        ((InstanceStore)result.InstanceStore).ChangeTaskContext(workflowTaskInfo.TaskContext);
      return result;
    }

    private void OnIdle(WorkflowApplicationIdleEventArgs e)
    {
    }

    private PersistableIdleAction OnIdleAndPersistable(WorkflowApplicationIdleEventArgs e)
    {
      return PersistableIdleAction.Persist;
    }

    private void OnWorkflowCompleted(WorkflowApplicationCompletedEventArgs e)
    {
      _instances.Remove(e.InstanceId);
    }

    private void OnWorkflowCompletedNeedClearWorkflowInstance(WorkflowApplicationCompletedEventArgs e)
    {
      OnWorkflowCompleted(e);
      WorkflowHub.ClearWorkflowInstance(e.InstanceId);
    }

    /// <summary>
    /// 书签是否被挂起
    /// </summary>
    public bool CanResumeBookmark(WorkflowTaskInfo workflowTaskInfo)
    {
      WorkflowApplication application = LoadInstance(workflowTaskInfo);
      foreach (BookmarkInfo item in application.GetBookmarks())
        if (item.BookmarkName.Equals(workflowTaskInfo.BookmarkName))
          return true;
      return false;
    }

    /// <summary>
    /// 启动操作以恢复书签
    /// </summary>
    public BookmarkResumptionResult ResumeBookmark(WorkflowTaskInfo workflowTaskInfo)
    {
      WorkflowApplication application = LoadInstance(workflowTaskInfo);
      return application.ResumeBookmark(workflowTaskInfo.BookmarkName, workflowTaskInfo.TaskContext);
    }

    /// <summary>
    /// 是否存在挂起的书签
    /// </summary>
    public bool HaveBookmark(WorkflowTaskInfo workflowTaskInfo)
    {
      WorkflowApplication application = LoadInstance(workflowTaskInfo);
      return application.GetBookmarks().Count > 0;
    }
  }
}