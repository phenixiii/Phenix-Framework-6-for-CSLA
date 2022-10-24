using System.Runtime.Remoting.Channels;
using System.Security.Permissions;

namespace Phenix.Core.Net.RemotingCompression
{
  /// <summary>
  /// ΪԶ�̴�����Ϣ���������Ŀͻ����ŵ������ͻ����ŵ�������
  /// </summary>
  public class CompressClientChannelSinkProvider : IClientChannelSinkProvider
  {
    /// <summary>
    /// ΪԶ�̴�����Ϣ���������Ŀͻ����ŵ������ͻ����ŵ�������
    /// </summary>
    public CompressClientChannelSinkProvider(int compressionThreshold)
    {
      _compressionThreshold = compressionThreshold;
    }

    #region ����

    private IClientChannelSinkProvider _next;
    /// <summary>
    /// Gets or sets the next sink provider in the channel sink provider chain.
    /// </summary>
    public IClientChannelSinkProvider Next
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
    public IClientChannelSink CreateSink(IChannelSender channel, string url, object remoteChannelData)
    {
      IClientChannelSink nextSink = null;

      if (_next != null)
      {
        // Call CreateSink on the next sink provider in the chain.  This will return
        // to us the actual next sink object.  If the next sink is null, uh oh!
        if ((nextSink = _next.CreateSink(channel, url, remoteChannelData)) == null)
          return null;
      }

      // Create this sink, passing to it the previous sink in the chain so that it knows
      // to whom messages should be passed.
      return new CompressClientChannelSink(nextSink, _compressionThreshold);
    }

    #endregion
  }
}
