using System;

namespace Phenix.StandardService.Contract
{
  public interface IWorker
  {
    #region 方法

    /// <summary>
    /// 与数据库时钟保持同步
    /// </summary>
    DateTime? SynchronizeClock();

    #endregion
  }
}
