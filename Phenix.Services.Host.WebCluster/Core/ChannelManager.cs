using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using Phenix.Core;
using Phenix.Core.Net;
using Phenix.Core.Web;

namespace Phenix.Services.Host.WebCluster.Core
{
  internal class ChannelManager : BaseDisposable
  {
    #region 单例

    private ChannelManager() { }

    private static readonly object _defaultLock = new object();
    private static ChannelManager _default;
    public static ChannelManager Default
    {
      get
      {
        lock (_defaultLock)
        {
          if (_default == null)
          {
            _default = new ChannelManager();
            try
            {
              _default._channels = File.Exists(ChannelsPath)
                ? Phenix.Core.Reflection.Utilities.JsonDeserialize<List<Channel>>(File.ReadAllText(ChannelsPath))
                : new List<Channel>();
              if (_default._channels.Count == 0)
                _default._channels.Add(new Channel(String.Format(@"http:\\{0}", NetConfig.LOCAL_IP), WebConfig.WebApiPort, WebConfig.WebSocketPort));
            }
            catch
            {
              _default.Dispose();
              _default = null;
              throw;
            }
          }
          return _default;
        }
      }
    }

    #endregion

    #region 属性

    private static string ChannelsPath
    {
      get { return Path.Combine(AppConfig.BaseDirectory, "channels.json"); }
    }

    private readonly ReaderWriterLock _rwLock = new ReaderWriterLock();
    private List<Channel> _channels;

    private readonly Random _random = new Random();

    #endregion

    #region 方法

    #region 实现 BaseDisposable 抽象函数

    protected override void DisposeManagedResources()
    {
      lock (_defaultLock)
      {
        _default = null;
      }
      _rwLock.AcquireWriterLock(Timeout.Infinite);
      try
      {
        if (_channels != null)
        {
          foreach (Channel item in _channels)
            item.Dispose();
          _channels = null;
        }
      }
      finally
      {
        _rwLock.ReleaseWriterLock();
      }
    }

    protected override void DisposeUnmanagedResources()
    {
    }

    #endregion

    public void AddChannel(Channel channelInfo)
    {
      _rwLock.AcquireWriterLock(Timeout.Infinite);
      try
      {
        _channels.Add(channelInfo);
        File.WriteAllText(ChannelsPath, Phenix.Core.Reflection.Utilities.JsonSerialize(_channels));
      }
      finally
      {
        _rwLock.ReleaseWriterLock();
      }
    }

    public void DeleteChannel(Channel channelInfo)
    {
      _rwLock.AcquireWriterLock(Timeout.Infinite);
      try
      {
        _channels.Remove(channelInfo);
        File.WriteAllText(ChannelsPath, Phenix.Core.Reflection.Utilities.JsonSerialize(_channels));
      }
      finally
      {
        _rwLock.ReleaseWriterLock();
      }
    }

    public Channel GetChannel()
    {
      _rwLock.AcquireReaderLock(Timeout.Infinite);
      try
      {
        if (_channels != null)
        {
          //随机挑选
          Channel channel = _channels[_random.Next(_channels.Count - 1)];
          //最近一次不出错或超过出错30分钟的Channel
          if (!channel.LastErrorTime.HasValue || DateTime.Now.Subtract(channel.LastErrorTime.Value).TotalMinutes >= 30)
            return channel;
          //顺序挑选最近一次不出错的Channel
          foreach (Channel item in _channels)
            if (!item.LastErrorTime.HasValue)
              return item;
          return channel;
        }
      }
      finally
      {
        _rwLock.ReleaseReaderLock();
      }
      return null;
    }

    public IEnumerator GetEnumerator()
    {
      _rwLock.AcquireReaderLock(Timeout.Infinite);
      try
      {
        return _channels.GetEnumerator();
      }
      finally
      {
        _rwLock.ReleaseReaderLock();
      }
    }

    #endregion
  }
}
