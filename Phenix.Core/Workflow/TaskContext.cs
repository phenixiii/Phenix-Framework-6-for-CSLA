using System;

namespace Phenix.Core.Workflow
{
  /// <summary>
  /// 任务上下文
  /// </summary>
  [Serializable]
  public class TaskContext
  {
    /// <summary>
    /// 初始化
    /// </summary>
    public TaskContext(object content)
      : this(null, content, null, false) { }

    /// <summary>
    /// 初始化
    /// </summary>
    public TaskContext(object content, string message)
      : this(null, content, message, false) { }

    /// <summary>
    /// 初始化
    /// </summary>
    public TaskContext(object content, bool needLeaveTrace)
      : this(null, content, null, needLeaveTrace) { }

    /// <summary>
    /// 初始化
    /// </summary>
    public TaskContext(string workerUserNumber, object content, string message)
      : this(workerUserNumber, content, message, false) { }

    /// <summary>
    /// 初始化
    /// </summary>
    public TaskContext(string workerUserNumber, object content, bool needLeaveTrace)
      : this(workerUserNumber, content, null, needLeaveTrace) { }

    /// <summary>
    /// 初始化
    /// </summary>
    public TaskContext(string workerUserNumber, object content, string message, bool needLeaveTrace)
    {
      _workerUserNumber = workerUserNumber;
      _content = content;
      _needLeaveTrace = needLeaveTrace;
      _message = message;
    }

    #region 属性

    private string _workerUserNumber;
    /// <summary>
    /// 作业工号
    /// </summary>
    public string WorkerUserNumber
    {
      get { return _workerUserNumber; }
      set { _workerUserNumber = value; }
    }

    private object _content;
    /// <summary>
    /// 内容
    /// </summary>
    public object Content
    {
      get { return _content; }
      set { _content = value; }
    }

    private bool _needLeaveTrace;
    /// <summary>
    /// 需要留下痕迹?
    /// 缺省为 false
    /// </summary>
    public bool NeedLeaveTrace
    {
      get { return _needLeaveTrace; }
      set { _needLeaveTrace = value; }
    }

    private string _dispatchUserNumber;
    /// <summary>
    /// 发布工号
    /// </summary>
    public string DispatchUserNumber
    {
      get { return _dispatchUserNumber; }
      set { _dispatchUserNumber = value; }
    }

    private string _message;
    /// <summary>
    /// 消息
    /// </summary>
    public string Message
    {
      get { return _message; }
      set { _message = value; }
    }

    #endregion
  }
}
