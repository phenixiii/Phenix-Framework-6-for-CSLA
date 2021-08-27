using System;
using System.Collections.Generic;
using System.Net.Sockets;
using Phenix.Core.Cache;
using Phenix.Core.Net;
using Phenix.Services.Contract;

namespace Phenix.Services.Client.Library
{
  internal class ObjectCacheSynchroProxy : IObjectCacheSynchro
  {
    #region 属性

    private IObjectCacheSynchro _service;
    private IObjectCacheSynchro Service
    {
      get
      {
        if (_service == null)
        {
          RemotingHelper.RegisterClientChannel();
          _service = (IObjectCacheSynchro)RemotingHelper.CreateRemoteObjectProxy(typeof(IObjectCacheSynchro), ServicesInfo.OBJECT_CACHE_SYNCHRO_URI);
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

    #region IObjectCacheSynchro 成员

    public DateTime? GetActionTime(string typeName)
    {
      NetConfig.InitializeSwitch();
      do
      {
        try
        {
          return Service.GetActionTime(typeName);
        }
        catch (SocketException)
        {
          InvalidateCache();
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
          Service.ClearAll();
          break;
        }
        catch (SocketException)
        {
          InvalidateCache();
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
          Service.Clear(typeNames);
          break;
        }
        catch (SocketException)
        {
          InvalidateCache();
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
          Service.RecordHasChanged(tableName);
          break;
        }
        catch (SocketException)
        {
          InvalidateCache();
          if (!NetConfig.SwitchServicesAddress())
            throw;
        }
      } while (true);
    }

    #endregion

    #endregion
  }
}