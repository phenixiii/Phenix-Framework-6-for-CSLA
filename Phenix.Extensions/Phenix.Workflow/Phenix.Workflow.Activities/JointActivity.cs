using System;
using System.Activities;
using System.ComponentModel;
using Phenix.Core.Data;
using Phenix.Core.Workflow;

namespace Phenix.Workflow.Activities
{
  /// <summary>
  /// 断点活动
  /// </summary>
  [Description("断点")]
  public class JointActivity : NativeActivity, IJointActivity
  {
    #region 属性

    /// <summary>
    /// 插件程序集名
    /// </summary>
    public string PluginAssemblyName { get; set; }
    
    /// <summary>
    /// 作业角色
    /// </summary>
    public string WorkerRole { get; set; }

    /// <summary>
    /// 标签
    /// </summary>
    public string Caption { get; set; }

    /// <summary>
    /// 消息
    /// </summary>
    public string Message { get; set; }

    /// <summary>
    /// 是否急件
    /// </summary>
    public bool Urgent { get; set; }

    /// <summary>
    /// 指示活动是否会使工作流进入空闲状态
    /// </summary>
    protected override bool CanInduceIdle
    {
      get { return true; }
    }

    #endregion

    /// <summary>
    /// 运行活动的执行逻辑
    /// </summary>
    protected override void Execute(NativeActivityContext context)
    {
      string bookmarkName = String.Format("{0}.{1}", this.GetType().FullName, Sequence.Value);
      WorkflowHub.DispatchWorkflowTask(context.WorkflowInstanceId, bookmarkName, this);
      context.CreateBookmark(bookmarkName, new BookmarkCallback(BookmarkCallback), BookmarkOptions.None);
    }

    /// <summary>
    /// 当通知指示要 NativeActivity 恢复时，要调用的方法
    /// </summary>
    protected virtual void BookmarkCallback(NativeActivityContext context, Bookmark bookmark, object obj)
    {
      WorkflowHub.CompleteWorkflowTask(context.WorkflowInstanceId, bookmark.Name);
    } 
  }

  /// <summary>
  /// 断点活动
  /// </summary>
  [Description("断点")]
  public abstract class JointActivity<TResult> : NativeActivity<TResult>, IJointActivity
  {
    #region 属性

    /// <summary>
    /// 插件程序集名
    /// </summary>
    public string PluginAssemblyName { get; set; }

    /// <summary>
    /// 作业角色
    /// </summary>
    public string WorkerRole { get; set; }

    /// <summary>
    /// 标签
    /// </summary>
    public string Caption { get; set; }

    /// <summary>
    /// 消息
    /// </summary>
    public string Message { get; set; }

    /// <summary>
    /// 是否急件
    /// </summary>
    public bool Urgent { get; set; }

    /// <summary>
    /// 指示活动是否会使工作流进入空闲状态
    /// </summary>
    protected override bool CanInduceIdle
    {
      get { return true; }
    }

    #endregion

    /// <summary>
    /// 运行活动的执行逻辑
    /// </summary>
    protected override void Execute(NativeActivityContext context)
    {
      string bookmarkName = String.Format("{0}.{1}", this.GetType().FullName, Sequence.Value);
      WorkflowHub.DispatchWorkflowTask(context.WorkflowInstanceId, bookmarkName, this);
      context.CreateBookmark(bookmarkName, new BookmarkCallback(BookmarkCallback), BookmarkOptions.None);
    }

    /// <summary>
    /// 当通知指示要 NativeActivity 恢复时，要调用的方法
    /// </summary>
    protected virtual void BookmarkCallback(NativeActivityContext context, Bookmark bookmark, object obj)
    {
      WorkflowHub.CompleteWorkflowTask(context.WorkflowInstanceId, bookmark.Name);
      context.SetValue(Result, (TResult)obj);
    } 
  }
}
