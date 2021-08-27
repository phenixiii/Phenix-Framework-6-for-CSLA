using System.ComponentModel;
using Phenix.Core;
using Phenix.Core.Workflow;

namespace Phenix.Workflow.Activities
{
  /// <summary>
  /// 适用于特定自定义活动（这些活动使用 Execute 方法实现执行逻辑，该方法具有对运行时功能的完全访问权限）的抽象基类
  /// </summary>
  public abstract class NativeActivity : System.Activities.NativeActivity, IActivity
  {
    /// <summary>
    /// 初始化
    /// </summary>
    protected NativeActivity()
      : base()
    {
      DescriptionAttribute descriptionAttribute = AppUtilities.GetFirstCustomAttribute<DescriptionAttribute>(this.GetType());
      if (descriptionAttribute != null)
        DisplayName = descriptionAttribute.Description;
    }
  }

  /// <summary>
  /// 适用于特定自定义活动（这些活动使用 Execute 方法实现执行逻辑，该方法具有对运行时功能的完全访问权限）的抽象基类
  /// </summary>
  public abstract class NativeActivity<TResult> : System.Activities.NativeActivity<TResult>, IActivity
  {
    /// <summary>
    /// 初始化
    /// </summary>
    protected NativeActivity()
      : base()
    {
      DescriptionAttribute descriptionAttribute = AppUtilities.GetFirstCustomAttribute<DescriptionAttribute>(this.GetType());
      if (descriptionAttribute != null)
        DisplayName = descriptionAttribute.Description;
    }
  }
}
