using System;
using System.Data.Common;
using Phenix.Core.Security;
using Phenix.Core.Workflow;

namespace Phenix.Business.Workflow
{
  /// <summary>
  /// 启动工作流指令基类 
  /// </summary>
  [Serializable]
  public abstract class StartCommandBase<T> : CommandBase<T>, IStartCommand
    where T : StartCommandBase<T>
  {
    #region 工厂

    /// <summary>
    /// 执行指令
    /// </summary>
    /// <param name="taskContext">任务上下文</param>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1000:DoNotDeclareStaticMembersOnGenericTypes")]
    public static void Execute(TaskContext taskContext)
    {
      T result = DynamicCreateInstance();
      result.TaskContext = taskContext;
      result.Execute();
    }

    /// <summary>
    /// 执行指令
    /// </summary>
    /// <param name="connection">数据库连接</param>
    /// <param name="taskContext">任务上下文</param>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1000:DoNotDeclareStaticMembersOnGenericTypes")]
    public static void Execute(DbConnection connection, TaskContext taskContext)
    {
      T result = DynamicCreateInstance();
      result.TaskContext = taskContext;
      result.Execute(connection);
    }

    /// <summary>
    /// 执行指令
    /// </summary>
    /// <param name="transaction">数据库事务</param>
    /// <param name="taskContext">任务上下文</param>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1000:DoNotDeclareStaticMembersOnGenericTypes")]
    public static void Execute(DbTransaction transaction, TaskContext taskContext)
    {
      T result = DynamicCreateInstance();
      result.TaskContext = taskContext;
      result.Execute(transaction);
    }

    #endregion

    #region 属性

    [Csla.NotUndoable]
    private TaskContext _taskContext;
    /// <summary>
    /// 任务上下文
    /// </summary>
    [System.ComponentModel.Browsable(false)]
    [System.ComponentModel.DataAnnotations.Display(AutoGenerateField = false)]
    public TaskContext TaskContext
    {
      get { return _taskContext; }
      private set
      {
        value = value ?? new TaskContext(null);
        if (UserIdentity.CurrentIdentity != null)
          value.DispatchUserNumber = UserIdentity.CurrentIdentity.UserNumber;
        _taskContext = value;
      }
    }

    #endregion

    #region 方法

    /// <summary>
    /// DoExecute
    /// </summary>
    protected override void DoExecute()
    {
      InstanceHost.Default.CreateAndRun(this);
    }

    #endregion
  }
}
