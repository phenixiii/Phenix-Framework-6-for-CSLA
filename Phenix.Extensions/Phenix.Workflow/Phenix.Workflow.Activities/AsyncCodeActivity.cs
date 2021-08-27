using System.ComponentModel;
using Phenix.Core;
using Phenix.Core.Workflow;

namespace Phenix.Workflow.Activities
{
  /// <summary>
  /// 自始至终管理异步代码活动的执行
  /// </summary>
  public abstract class AsyncCodeActivity : System.Activities.AsyncCodeActivity, IActivity
  {
    /// <summary>
    /// 初始化
    /// </summary>
    protected AsyncCodeActivity()
      : base()
    {
      DescriptionAttribute descriptionAttribute = AppUtilities.GetFirstCustomAttribute<DescriptionAttribute>(this.GetType());
      if (descriptionAttribute != null)
        DisplayName = descriptionAttribute.Description;
    }
  }

  /// <summary>
  /// 自始至终管理指定类型活动的异步代码活动的执行
  /// </summary>
  public abstract class AsyncCodeActivity<TResult> : System.Activities.AsyncCodeActivity<TResult>, IActivity
  {
    /// <summary>
    /// 初始化
    /// </summary>
    protected AsyncCodeActivity()
      : base()
    {
      DescriptionAttribute descriptionAttribute = AppUtilities.GetFirstCustomAttribute<DescriptionAttribute>(this.GetType());
      if (descriptionAttribute != null)
        DisplayName = descriptionAttribute.Description;
    }
  }
}
