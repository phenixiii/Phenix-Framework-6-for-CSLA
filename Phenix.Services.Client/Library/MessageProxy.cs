using System.Collections.Generic;
using System.Net.Sockets;
using Phenix.Core.Message;
using Phenix.Core.Net;
using Phenix.Core.Security;
using Phenix.Services.Contract;

namespace Phenix.Services.Client.Library
{
  internal class MessageProxy : IMessage
  {
    #region 属性
    
    private IMessage _service;
    private IMessage Service
    {
      get
      {
        if (_service == null)
        {
          RemotingHelper.RegisterClientChannel();
          _service = (IMessage)RemotingHelper.CreateRemoteObjectProxy(typeof(IMessage), ServicesInfo.MESSAGE_URI);
        }
        return _service;
      }
    }

    #endregion

    #region 方法

    private void InvalidateCache()
    {
      _service = null;
    }

    #region IMessage 成员

    public void Send(string receiver, string content, UserIdentity identity)
    {
      NetConfig.InitializeSwitch();
      do
      {
        try
        {
          Service.Send(receiver, content, identity);
          break;
        }
        catch (SocketException)
        {
          InvalidateCache();
          if (!NetConfig.SwitchServicesAddress())
            return;
        }
      } while (true);
    }

    public IDictionary<long, string> Receive(UserIdentity identity)
    {
      NetConfig.InitializeSwitch();
      do
      {
        try
        {
          return Service.Receive(identity);
        }
        catch (SocketException)
        {
          InvalidateCache();
          if (!NetConfig.SwitchServicesAddress())
            throw;
        }
      } while (true);
    }

    public void AffirmReceived(long id, bool burn, UserIdentity identity)
    {
      NetConfig.InitializeSwitch();
      do
      {
        try
        {
          Service.AffirmReceived(id, burn, identity);
          break;
        }
        catch (SocketException)
        {
          InvalidateCache();
          if (!NetConfig.SwitchServicesAddress())
            return;
        }
      } while (true);
    }

    #endregion

    #endregion
  }
}