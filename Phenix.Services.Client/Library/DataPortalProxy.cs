using System;
using System.Collections;
using System.Net.Sockets;
using System.Reflection;
using Phenix.Business;
using Phenix.Core.Cache;
using Phenix.Core.Log;
using Phenix.Core.Mapping;
using Phenix.Core.Net;
using Phenix.Core.SyncCollections;
using Phenix.Services.Contract;

namespace Phenix.Services.Client.Library
{
  internal class DataPortalProxy : Csla.DataPortalClient.IDataPortalProxy
  {
    #region 属性

    private Csla.Server.IDataPortalServer _service;
    private Csla.Server.IDataPortalServer Service
    {
      get
      {
        if (_service == null)
        {
          if (NetConfig.ProxyType == ProxyType.Embedded)
          {
            _service = new Csla.Server.DataPortal();
            return _service;
          }
          RemotingHelper.RegisterClientChannel();
          _service = (Csla.Server.IDataPortalServer)RemotingHelper.CreateRemoteObjectProxy(
            typeof(Csla.Server.IDataPortalServer), ServicesInfo.DATA_PORTAL_URI);
        }
        return _service;
      }
    }
    
    private readonly SynchronizedDictionary<string, Csla.Server.IDataPortalServer> _serviceCluster =
      new SynchronizedDictionary<string, Csla.Server.IDataPortalServer>(StringComparer.Ordinal);

    #region IDataPortalProxy 成员

    public bool IsServerRemote
    {
      get { return NetConfig.ProxyType != ProxyType.Embedded; }
    }

    #endregion

    #endregion

    #region 方法

    private void InvalidateCache()
    {
      _service = null;
      _serviceCluster.Clear();
    }

    private Csla.Server.IDataPortalServer GetService(Type objectType, object criteria)
    {
      if (NetConfig.ProxyType == ProxyType.Embedded)
        return Service;
      Type rootType = objectType;
      ICriterions criterions = criteria as ICriterions;
      if (criterions != null)
      {
        IBusinessObject masterBusiness = criterions.Master as IBusinessObject;
        if (masterBusiness != null && masterBusiness.Root != null)
          rootType = masterBusiness.Root.GetType();
      }
      ServicesClusterAttribute clusterAttribute = ServicesClusterAttribute.Fetch(rootType);
      string servicesClusterAddress = NetConfig.GetServicesClusterAddress(clusterAttribute);
      if (String.IsNullOrEmpty(servicesClusterAddress))
        return Service;

      return _serviceCluster.GetValue(clusterAttribute.Key, () =>
      {
        RemotingHelper.RegisterClientChannel();
        return (Csla.Server.IDataPortalServer)RemotingHelper.CreateRemoteObjectProxy(
          typeof(Csla.Server.IDataPortalServer), servicesClusterAddress, ServicesInfo.DATA_PORTAL_URI);
      }, true);
    }

    #region IDataPortalProxy 成员

    public Csla.Server.DataPortalResult Create(Type objectType, object criteria, Csla.Server.DataPortalContext context)
    {
      NetConfig.InitializeSwitch();
      do
      {
        try
        {
          return GetService(objectType, criteria).Create(objectType, criteria, context);
        }
        catch (SocketException)
        {
          InvalidateCache();
          if (!NetConfig.SwitchServicesAddress())
            throw;
        }
      } while (true);
    }

    public Csla.Server.DataPortalResult Fetch(Type objectType, object criteria, Csla.Server.DataPortalContext context)
    {
      if (objectType == null)
        throw new ArgumentNullException("objectType");
      DateTime dt = DateTime.Now;
      Csla.Server.DataPortalResult result;
      DateTime? actionTime;
      object obj = ObjectCache.Find(objectType, criteria, out actionTime);
      if (obj != null)
        result = new Csla.Server.DataPortalResult(obj);
      else
      {
        NetConfig.InitializeSwitch();
        do
        {
          try
          {
            result = GetService(objectType, criteria).Fetch(objectType, criteria, context);
            break;
          }
          catch (SocketException)
          {
            InvalidateCache();
            if (!NetConfig.SwitchServicesAddress())
              throw;
          }
        } while (true);
        if (actionTime != null)
          ObjectCache.Add(criteria, result.ReturnObject, actionTime.Value);
      }
      //跟踪日志
      if (EventLog.MustSaveLog)
      {
        ICriterions criterions = criteria as ICriterions;
        EventLog.SaveLocal(MethodBase.GetCurrentMethod().Name + ' ' + objectType.FullName +
          (criterions != null && criterions.Criteria != null ? " with " + criterions.Criteria.GetType().FullName : String.Empty) +
          " take " + DateTime.Now.Subtract(dt).TotalMilliseconds.ToString() + " millisecond," +
          " count = " + (result.ReturnObject is IList ? ((IList)result.ReturnObject).Count.ToString() :
            result.ReturnObject is IBusinessObject ? ((IBusinessObject)result.ReturnObject).SelfFetched ? "1" : "0" :
            result.ReturnObject != null ? "1" : "0"));
      }
      return result;
    }

    public Csla.Server.DataPortalResult Update(object obj, Csla.Server.DataPortalContext context)
    {
      NetConfig.InitializeSwitch();
      do
      {
        try
        {
          return GetService(obj.GetType(), null).Update(obj, context);
        }
        catch (SocketException)
        {
          InvalidateCache();
          if (!NetConfig.SwitchServicesAddress())
            throw;
        }
      } while (true);
    }

    public Csla.Server.DataPortalResult Delete(Type objectType, object criteria, Csla.Server.DataPortalContext context)
    {
      NetConfig.InitializeSwitch();
      do
      {
        try
        {
          return GetService(objectType, criteria).Delete(objectType, criteria, context);
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