using System;
using System.Collections.Generic;
using System.ServiceModel;
using Phenix.Core.Message;
using Phenix.Core.Net;
using Phenix.Core.Security;
using Phenix.Services.Contract;

namespace Phenix.Services.Client.Library.Wcf
{
  internal class MessageProxy : IMessage
  {
    #region 方法

    private static ChannelFactory<Phenix.Services.Contract.Wcf.IMessage> GetChannelFactory()
    {
      return new ChannelFactory<Phenix.Services.Contract.Wcf.IMessage>(WcfHelper.CreateBinding(),
        new EndpointAddress(WcfHelper.CreateUrl(NetConfig.ServicesAddress, ServicesInfo.MESSAGE_URI)));
    }

    #region IMessage 成员

    public void Send(string receiver, string content, UserIdentity identity)
    {
      NetConfig.InitializeSwitch();
      do
      {
        try
        {
          ChannelFactory<Phenix.Services.Contract.Wcf.IMessage> channelFactory = GetChannelFactory();
          Phenix.Services.Contract.Wcf.IMessage channel = channelFactory.CreateChannel();
          try
          {
            channel.Send(receiver, content, identity);
            channelFactory.Close();
          }
          catch
          {
            channelFactory.Abort();
            throw;
          }
          break;
        }
        catch (EndpointNotFoundException)
        {
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
          ChannelFactory<Phenix.Services.Contract.Wcf.IMessage> channelFactory = GetChannelFactory();
          Phenix.Services.Contract.Wcf.IMessage channel = channelFactory.CreateChannel();
          object result = null;
          try
          {
            result = channel.Receive(identity);
            channelFactory.Close();
          }
          catch
          {
            channelFactory.Abort();
            throw;
          }
          Exception exception = result as Exception;
          if (exception != null)
            throw exception;
          return (IDictionary<long, string>)result;
        }
        catch (EndpointNotFoundException)
        {
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
          ChannelFactory<Phenix.Services.Contract.Wcf.IMessage> channelFactory = GetChannelFactory();
          Phenix.Services.Contract.Wcf.IMessage channel = channelFactory.CreateChannel();
          try
          {
            channel.AffirmReceived(id, burn, identity);
            channelFactory.Close();
          }
          catch
          {
            channelFactory.Abort();
            throw;
          }
          break;
        }
        catch (EndpointNotFoundException)
        {
          if (!NetConfig.SwitchServicesAddress())
            return;
        }
      } while (true);
    }

    #endregion

    #endregion
  }
}