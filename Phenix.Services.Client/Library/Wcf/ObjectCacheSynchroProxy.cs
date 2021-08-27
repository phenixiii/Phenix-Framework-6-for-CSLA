using System;
using System.Collections.Generic;
using System.ServiceModel;
using Phenix.Core.Cache;
using Phenix.Core.Net;
using Phenix.Services.Contract;

namespace Phenix.Services.Client.Library.Wcf
{
  internal class ObjectCacheSynchroProxy : IObjectCacheSynchro
  {
    #region 方法

    private static ChannelFactory<Phenix.Services.Contract.Wcf.IObjectCacheSynchro> GetChannelFactory()
    {
      return new ChannelFactory<Phenix.Services.Contract.Wcf.IObjectCacheSynchro>(WcfHelper.CreateBinding(),
        new EndpointAddress(WcfHelper.CreateUrl(NetConfig.ServicesAddress, ServicesInfo.OBJECT_CACHE_SYNCHRO_URI)));
    }

    #region IObjectCacheSynchro 成员

    public DateTime? GetActionTime(string typeName)
    {
      NetConfig.InitializeSwitch();
      do
      {
        try
        {
          ChannelFactory<Phenix.Services.Contract.Wcf.IObjectCacheSynchro> channelFactory = GetChannelFactory();
          Phenix.Services.Contract.Wcf.IObjectCacheSynchro channel = channelFactory.CreateChannel();
          object result = null;
          try
          {
            result = channel.GetActionTime(typeName);
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
          return (DateTime?)result;
        }
        catch (EndpointNotFoundException)
        {
          if (!NetConfig.SwitchServicesAddress())
            return DateTime.MinValue;
        }
      } while (true);
    }

    public void ClearAll()
    {
      NetConfig.InitializeSwitch();
      do
      {
        try
        {
          ChannelFactory<Phenix.Services.Contract.Wcf.IObjectCacheSynchro> channelFactory = GetChannelFactory();
          Phenix.Services.Contract.Wcf.IObjectCacheSynchro channel = channelFactory.CreateChannel();
          try
          {
            channel.ClearAll();
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
            throw;
        }
      } while (true);
    }

    public void Clear(IList<string> typeNames)
    {
      NetConfig.InitializeSwitch();
      do
      {
        try
        {
          ChannelFactory<Phenix.Services.Contract.Wcf.IObjectCacheSynchro> channelFactory = GetChannelFactory();
          Phenix.Services.Contract.Wcf.IObjectCacheSynchro channel = channelFactory.CreateChannel();
          try
          {
            channel.Clear(typeNames);
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
            throw;
        }
      } while (true);
    }

    public void RecordHasChanged(string tableName)
    {
      NetConfig.InitializeSwitch();
      do
      {
        try
        {
          ChannelFactory<Phenix.Services.Contract.Wcf.IObjectCacheSynchro> channelFactory = GetChannelFactory();
          Phenix.Services.Contract.Wcf.IObjectCacheSynchro channel = channelFactory.CreateChannel();
          try
          {
            channel.RecordHasChanged(tableName);
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
            throw;
        }
      } while (true);
    }

    #endregion

    #endregion
  }
}