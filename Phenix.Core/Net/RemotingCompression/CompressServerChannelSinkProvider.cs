using System.Runtime.Remoting.Channels;
using System.Security.Permissions;

namespace Phenix.Core.Net.RemotingCompression
{  
  /// <summary>
  /// ΪԶ�̴�����Ϣ���������ķ������ŵ������������ŵ�������
  /// </summary>
  public class CompressServerChannelSinkProvider : IServerChannelSinkProvider
  {
    /// <summary>
    /// ΪԶ�̴�����Ϣ���������ķ������ŵ������������ŵ�������
    /// </summary>
    public CompressServerChannelSinkProvider(int compressionThreshold)
    {
      _compressionThreshold = compressionThreshold;
    }

    #region ����

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

    #region ����

    /// <summary>
    /// ������������
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
    /// ȡ�ŵ�����
    /// </summary>
    [SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.Infrastructure)]
    public void GetChannelData(IChannelDataStore channelData)
    {
      // Do nothing.  No channel specific data.
    }

    #endregion
  }
}