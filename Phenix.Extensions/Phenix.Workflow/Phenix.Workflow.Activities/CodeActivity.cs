using System.ComponentModel;
using Phenix.Core;
using Phenix.Core.Workflow;

namespace Phenix.Workflow.Activities
{
  /// <summary>
  /// 用于创建具有强制行为（该行为是使用 Execute 方法定义的，利用该方法可以访问变量以及参数解析和扩展）的自定义活动
  /// </summary>
  public abstract class CodeActivity : System.Activities.CodeActivity, IActivity
  {
    /// <summary>
    /// 初始化
    /// </summary>
    protected CodeActivity()
      : base()
    {
      DescriptionAttribute descriptionAttribute = AppUtilities.GetFirstCustomAttribute<DescriptionAttribute>(this.GetType());
      if (descriptionAttribute != null)
        DisplayName = descriptionAttribute.Description;
    }
  }

  /// <summary>
  /// 用于创建具有强制行为（该行为使用 Execute 方法定义）的自定义活动，从而提供对变量和参数解析及扩展的访问
  /// </summary>
  public abstract class CodeActivity<TResult> : System.Activities.CodeActivity<TResult>, IActivity
  {
    /// <summary>
    /// 初始化
    /// </summary>
    protected CodeActivity()
      : base()
    {
      DescriptionAttribute descriptionAttribute = AppUtilities.GetFirstCustomAttribute<DescriptionAttribute>(this.GetType());
      if (descriptionAttribute != null)
        DisplayName = descriptionAttribute.Description;
    }
  }
}
