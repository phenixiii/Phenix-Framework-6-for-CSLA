using System;

namespace Phenix.TeamHub.Prober.Business
{
  /// <summary>
  /// 操作者
  /// </summary>
  public class Worker : Phenix.TeamHub.Prober.IWorker
  {
    /// <summary>
    /// 注册
    /// </summary>
    public static void Register()
    {
      Phenix.TeamHub.Prober.AppHub.Worker = new Worker();
    }

    /// <summary>
    /// 提交日志
    /// </summary>
    void Phenix.TeamHub.Prober.IWorker.SubmitLog(string message, System.Reflection.MethodBase method, Exception exception)
    {
      new SubmitLogCommand(message, method, exception).Execute();
    }
  }
}
