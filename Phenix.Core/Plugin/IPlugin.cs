#if Top
using System.Threading.Tasks;
#endif

using System.Collections.Generic;

namespace Phenix.Core.Plugin
{
  /// <summary>
  /// 插件接口
  /// </summary>
  public interface IPlugin
  {
    #region 属性

    /// <summary>
    /// 唯一键
    /// </summary>
    string Key { get; }

    /// <summary>
    /// 插件状态
    /// </summary>
    PluginState State { get; }

    /// <summary>
    /// 设置中
    /// </summary>
    bool Setuping { get; }

    #endregion

    #region 方法

    /// <summary>
    /// 初始化
    /// 由 PluginHost 调用
    /// </summary>
    IList<MessageNotifyEventArgs> Initialization();

    /// <summary>
    /// 终止化
    /// 由 PluginHost 调用
    /// </summary>
    void Finalization();

    /// <summary>
    /// 设置
    /// 由 PluginHost 调用
    /// </summary>
    /// <param name="sender">发起对象</param>
    /// <returns>按需返回</returns>
    object Setup(object sender);

    /// <summary>
    /// 启动
    /// 由 PluginHost 调用
    /// </summary>
    /// <returns>确定启动</returns>
    bool Start();

    /// <summary>
    /// 暂停
    /// 由 PluginHost 调用
    /// </summary>
    /// <returns>确定停止</returns>
    bool Suspend();

    /// <summary>
    /// 分析消息
    /// 由 PluginHost 调用
    /// </summary>
    /// <param name="message">消息</param>
    /// <returns>按需返回</returns>
    object AnalyseMessage(object message);

#if Top

    /// <summary>
    /// 分析消息(异步)
    /// 由 PluginHost 调用
    /// </summary>
    /// <param name="message">消息</param>
    /// <returns>按需返回</returns>
    Task<object> AnalyseMessageAsync(object message);

#endif

    #endregion
  }
}
