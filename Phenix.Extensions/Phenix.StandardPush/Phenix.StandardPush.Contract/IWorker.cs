namespace Phenix.StandardPush.Contract
{
  public interface IWorker
  {
    #region 属性

    #region Socket配置参数

    /// <summary>
    /// 缺省端口
    /// </summary>
    int Port { get; }

    /// <summary>
    /// Socket缓存尺寸
    /// </summary>
    int SocketBufferSize { get; }

    /// <summary>
    /// 消息缓存尺寸
    /// </summary>
    int MessageBufferSize { get; }

    /// <summary>
    /// 闲置检查间隔
    /// </summary>
    int IdleCheckInterval { get; }

    /// <summary>
    /// 闲置超时时间
    /// </summary>
    int IdleTimeOutValue { get; }

    #endregion

    #endregion
  }
}
