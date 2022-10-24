using System;

namespace Phenix.Core.Workflow
{
  /// <summary>
  /// 工作流任务资料
  /// </summary>
  [Serializable]
  public sealed class WorkflowTaskInfo
  {
    /// <summary>
    /// 初始化
    /// </summary>
    [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
    public WorkflowTaskInfo(Guid workflowInstanceId, string typeNamespace, string typeName, TaskContext taskContext, 
      string bookmarkName, string pluginAssemblyName, string workerRole,
      string caption, string message, bool urgent, TaskState state,
      DateTime dispatchTime, DateTime? receiveTime, DateTime? holdTime, DateTime? abortTime, DateTime? completeTime)
    {
      _workflowInstanceId = workflowInstanceId;
      _typeNamespace = typeNamespace;
      _typeName = typeName;
      _taskContext = taskContext;
      _bookmarkName = bookmarkName;
      _pluginAssemblyName = pluginAssemblyName;
      _workerRole = workerRole;
      _caption = caption;
      _message = message;
      _urgent = urgent;
      _state = state;
      _dispatchTime = dispatchTime;
      _receiveTime = receiveTime;
      _holdTime = holdTime;
      _abortTime = abortTime;
      _completeTime = completeTime;
    }

    #region 属性

    private readonly Guid _workflowInstanceId;
    /// <summary>
    /// 工作流实例句柄
    /// </summary>
    public Guid WorkflowInstanceId
    {
      get { return _workflowInstanceId; }
    }

    private readonly string _typeNamespace;
    /// <summary>
    /// 命名空间
    /// </summary> 
    public string TypeNamespace
    {
      get { return _typeNamespace; }
    }

    private readonly string _typeName;
    /// <summary>
    /// 类型名称
    /// </summary>
    public string TypeName
    {
      get { return _typeName; }
    }

    private readonly TaskContext _taskContext;
    /// <summary>
    /// 任务上下文
    /// </summary>
    public TaskContext TaskContext
    {
      get { return _taskContext; }
    }

    /// <summary>
    /// 作业工号
    /// </summary>
    public string WorkerUserNumber
    {
      get { return _taskContext.WorkerUserNumber; }
    }

    /// <summary>
    /// 发布工号
    /// </summary>
    public string DispatchUserNumber
    {
      get { return _taskContext.DispatchUserNumber; }
    }

    private readonly string _bookmarkName;
    /// <summary>
    /// 书签名称
    /// </summary>
    public string BookmarkName
    {
      get { return _bookmarkName; }
    }

    private readonly string _pluginAssemblyName;
    /// <summary>
    /// 插件程序集名
    /// </summary>
    public string PluginAssemblyName
    {
      get { return _pluginAssemblyName; }
    }

    private readonly string _workerRole;
    /// <summary>
    /// 作业角色
    /// </summary>
    public string WorkerRole
    {
      get { return _workerRole; }
    }

    private readonly string _caption;
    /// <summary>
    /// 标签
    /// </summary>
    public string Caption
    {
      get { return _caption; }
    }

    private readonly string _message;
    /// <summary>
    /// 消息
    /// </summary>
    public string Message
    {
      get { return String.Format("{0}{1}{2}", _message, !String.IsNullOrEmpty(_message) && !String.IsNullOrEmpty(_taskContext.Message) ? ": " : null, _taskContext.Message); }
    }

    private readonly bool _urgent;
    /// <summary>
    /// 急件
    /// </summary>
    public bool Urgent
    {
      get { return _urgent; }
    }

    private readonly TaskState _state;
    /// <summary>
    /// 任务状态
    /// </summary>
    public TaskState State
    {
      get { return _state; }
    }

    private readonly DateTime _dispatchTime;
    /// <summary>
    /// 发送时间
    /// </summary>
    public DateTime DispatchTime
    {
      get { return _dispatchTime; }
    }

    private readonly DateTime? _receiveTime;
    /// <summary>
    /// 接收时间
    /// </summary>
    public DateTime? ReceiveTime
    {
      get { return _receiveTime; }
    }

    private readonly DateTime? _holdTime;
    /// <summary>
    /// 挂起时间
    /// </summary>
    public DateTime? HoldTime
    {
      get { return _holdTime; }
    }

    private readonly DateTime? _abortTime;
    /// <summary>
    /// 中断时间
    /// </summary>
    public DateTime? AbortTime
    {
      get { return _abortTime; }
    }

    private readonly DateTime? _completeTime;
    /// <summary>
    /// 完结时间
    /// </summary>
    public DateTime? CompleteTime
    {
      get { return _completeTime; }
    }

    #endregion

    #region 方法

    /// <summary>
    /// 收到
    /// </summary>
    public void Receive()
    {
      WorkflowHub.ReceiveWorkflowTask(WorkflowInstanceId, BookmarkName);
    }

    /// <summary>
    /// 挂起
    /// </summary>
    public void Hold()
    {
      WorkflowHub.HoldWorkflowTask(WorkflowInstanceId, BookmarkName);
    }

    /// <summary>
    /// 中断
    /// </summary>
    public void Abort()
    {
      WorkflowHub.AbortWorkflowTask(WorkflowInstanceId, BookmarkName);
    }

    /// <summary>
    /// 继续工作流
    /// </summary>
    public void ProceedWorkflow()
    {
      WorkflowHub.ProceedWorkflow(this);
    }

    /// <summary>
    /// 取哈希值(注意字符串在32位和64位系统有不同的算法得到不同的结果) 
    /// </summary>
    public override int GetHashCode()
    {
      return WorkflowInstanceId.GetHashCode() ^ BookmarkName.GetHashCode();
    }

    /// <summary>
    /// 比较对象
    /// </summary>
    /// <param name="obj">对象</param>
    public override bool Equals(object obj)
    {
      if (object.ReferenceEquals(obj, this))
        return true;
      WorkflowTaskInfo other = obj as WorkflowTaskInfo;
      if (object.ReferenceEquals(other, null))
        return false;
      return
        WorkflowInstanceId == other.WorkflowInstanceId &&
        String.CompareOrdinal(BookmarkName, other.BookmarkName) == 0;
    }

    #endregion
  }
}
