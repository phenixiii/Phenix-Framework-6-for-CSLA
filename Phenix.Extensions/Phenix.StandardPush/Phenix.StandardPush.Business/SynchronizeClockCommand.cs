using System;
using System.Net;

namespace Phenix.StandardPush.Business
{
  /// <summary>
  /// 与数据库时钟保持同步
  /// </summary>
  [Serializable]
  public class SynchronizeClockCommand : Phenix.Business.CommandBase<SynchronizeClockCommand>
  {
    #region 属性

    #region Socket配置参数

    private int _port;
    private int _socketBufferSize;
    private int _messageBufferSize;
    private int _idleCheckInterval;
    private int _idleTimeOutValue;

    #endregion
    
    #endregion

    #region 方法

    /// <summary>
    /// 调用入口
    /// </summary>
    public new static SynchronizeClocker Execute()
    {
      IPAddress address;
      if (IPAddress.TryParse(Phenix.Core.Net.NetConfig.ServicesAddress, out address))
      {
        SynchronizeClockCommand command = Execute(new SynchronizeClockCommand());
        return new SynchronizeClocker(address, command._port, command._socketBufferSize, command._messageBufferSize, command._idleCheckInterval, command._idleTimeOutValue);
      }
      return null;
    }

    /// <summary>
    /// 处理执行指令(运行在持久层的程序域里)
    /// </summary>
    protected override void DoExecute()
    {
      if (Phenix.StandardPush.Contract.AppHub.Worker != null)
      {
        _port = Phenix.StandardPush.Contract.AppHub.Worker.Port;
        _socketBufferSize = Phenix.StandardPush.Contract.AppHub.Worker.SocketBufferSize;
        _messageBufferSize = Phenix.StandardPush.Contract.AppHub.Worker.MessageBufferSize;
        _idleCheckInterval = Phenix.StandardPush.Contract.AppHub.Worker.IdleCheckInterval;
        _idleTimeOutValue = Phenix.StandardPush.Contract.AppHub.Worker.IdleTimeOutValue;
      }
    }

    #endregion
  }
}
