using System.Runtime.Remoting.Channels;
using System.Security.Permissions;

namespace Phenix.Core.Net.RemotingCompression
{  
  /// <summary>
  /// 为远程处理消息从其流过的服务器信道创建服务器信道接收器
  /// </summary>
  public class CompressServerChannelSinkProvider : IServerChannelSinkProvider
  {
    /// <summary>
    /// 为远程处理消息从其流过的服务器信道创建服务器信道接收器
    /// </summary>
    public CompressServerChannelSinkProvider(int compressionThreshold)
    {
      _compressionThreshold = compressionThreshold;
    }

    #region 属性

    private IServerChannelSinkProvider _next;
    /// <summary>
    /// Gets or sets the next sink provider in the channel sink provider chain.
    /// </summary>
    public IServerChannelSinkProvider Next
    {
      [SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.Infrastructure)]
      get { return _next; }
      [SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.Infrastructure)]
      set { _next = value; }
    }

    // The compression threshold.
    private readonly int _compressionThreshold;

    #endregion

    #region 方法

    /// <summary>
    /// 构建接收器链
    /// </summary>
    [SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.Infrastructure)]
    public IServerChannelSink CreateSink(IChannelReceiver channel)
    {
      IServerChannelSink nextSink = null;

      if (_next != null)
      {
        // Call CreateSink on the next sink provider in the chain.  This will return
        // to us the actual next sink object.  If the next sink is null, uh oh!
        if ((nextSink = _next.CreateSink(channel)) == null)
          return null;
      }

      // Create this sink, passing to it the previous sink in the chain so that it knows
      // to whom messages should be passed.
      return new CompressServerChannelSink(nextSink, _compressionThreshold);
    }

    /// <summary>
    /// 取信道数据
    /// </summary>
    [SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.Infrastructure)]
    public void GetChannelData(IChannelDataStore channelData)
    {
      // Do nothing.  No channel specific data.
    }

    #endregion
  }
}