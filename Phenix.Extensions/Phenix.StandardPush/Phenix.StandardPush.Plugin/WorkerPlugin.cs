using System.Collections.Generic;
using Phenix.Core;
using Phenix.Core.Plugin;

namespace Phenix.StandardPush.Plugin
{
  public class WorkerPlugin : PluginBase<WorkerPlugin>
  {
    #region 方法

    #region 覆写 PluginBase

    /// <summary>
    /// 初始化
    /// 由 PluginHost 调用
    /// </summary>
    protected override IList<MessageNotifyEventArgs> Initialization()
    {
      Phenix.StandardPush.Contract.AppHub.Worker = Worker.Run(SendMessage);
      return null;
    }

    /// <summary>
    /// 终止化
    /// 由 PluginHost 调用
    /// </summary>
    protected override void Finalization()
    {
      Worker.Stop();
    }

    /// <summary>
    /// 设置
    /// 由 PluginHost 调用
    /// </summary>
    /// <param name="sender">发起对象</param>
    /// <returns>按需返回</returns>
    public override object Setup(object sender)
    {
      return this;
    }

    /// <summary>
    /// 启动
    /// 由 PluginHost 调用
    /// </summary>
    /// <returns>确定启动</returns>
    protected override bool Start()
    {
      Phenix.StandardPush.Contract.AppHub.Worker = Worker.Run(SendMessage);
      return true;
    }

    /// <summary>
    /// 暂停
    /// 由 PluginHost 调用
    /// </summary>
    /// <returns>确定停止</returns>
    protected override bool Suspend()
    {
      Worker worker = Phenix.StandardPush.Contract.AppHub.Worker as Worker;
      if (worker != null)
        worker.Suspending = true;
      return true;
    }

    #endregion

    #endregion
  }
}