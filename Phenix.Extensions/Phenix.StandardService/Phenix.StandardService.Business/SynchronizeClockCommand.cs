using System;

namespace Phenix.StandardService.Business
{
  /// <summary>
  /// 与数据库时钟保持同步
  /// </summary>
  [Serializable]
  public class SynchronizeClockCommand : Phenix.Business.CommandBase<SynchronizeClockCommand>
  {
    #region 属性

    private DateTime? _value;
    /// <summary>
    /// 值
    /// </summary>
    public DateTime? Value
    {
      get { return _value; }
      private set { _value = value; }
    }

    #endregion

    #region 方法

    /// <summary>
    /// 执行指令之后
    /// </summary>
    /// <param name="ex">错误信息</param>
    /// <returns>发生错误时的友好提示信息, 缺省为 null</returns>
    protected override string OnExecuted(Exception ex)
    {
      if (Value.HasValue)
        Phenix.Core.Win32.NativeMethods.SetClock(Value.Value);
      return null;
    }

    /// <summary>
    /// 处理执行指令(运行在持久层的程序域里)
    /// </summary>
    protected override void DoExecute()
    {
      if (Phenix.StandardService.Contract.AppHub.Worker != null)
        Value = Phenix.StandardService.Contract.AppHub.Worker.SynchronizeClock();
    }

    #endregion
  }
}
