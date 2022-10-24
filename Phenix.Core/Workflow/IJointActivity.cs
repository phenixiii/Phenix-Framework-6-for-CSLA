namespace Phenix.Core.Workflow
{
  /// <summary>
  /// 断点活动接口
  /// </summary>
  public interface IJointActivity
  {
    #region 属性

    /// <summary>
    /// 插件程序集名
    /// </summary>
    string PluginAssemblyName { get; }

    /// <summary>
    /// 作业角色
    /// </summary>
    string WorkerRole { get; }

    /// <summary>
    /// 标签
    /// </summary>
    string Caption { get; }

    /// <summary>
    /// 消息
    /// </summary>
    string Message { get; }

    /// <summary>
    /// 是否急件
    /// </summary>
    bool Urgent { get; }

    #endregion
  }
}
