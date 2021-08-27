using System;
using System.Collections;
using System.Reflection;
using System.ServiceModel;
using Phenix.Business;
using Phenix.Core.Cache;
using Phenix.Core.Log;
using Phenix.Core.Mapping;
using Phenix.Core.Net;
using Phenix.Services.Contract;

namespace Phenix.Services.Client.Library.Wcf
{
  internal class DataPortalProxy : Csla.DataPortalClient.IDataPortalProxy
  {
    #region 属性

    #region IDataPortalProxy 成员

    public bool IsServerRemote
    {
      get { return NetConfig.ProxyType != ProxyType.Embedded; }
    }

    #endregion

    #endregion

    #region 方法

    private static ChannelFactory<Csla.Server.Hosts.IWcfPortal> GetChannelFactory()
    {
      return new ChannelFactory<Csla.Server.Hosts.IWcfPortal>(WcfHelper.CreateBinding(),
        new EndpointAddress(WcfHelper.CreateUrl(NetConfig.ServicesAddress, ServicesInfo.DATA_PORTAL_URI)));
    }

    private static ChannelFactory<Csla.Server.Hosts.IWcfPortal> GetChannelFactory(string host)
    {
      if (String.IsNullOrEmpty(host))
        return GetChannelFactory();
      return new ChannelFactory<Csla.Server.Hosts.IWcfPortal>(WcfHelper.CreateBinding(),
        new EndpointAddress(WcfHelper.CreateUrl(host, ServicesInfo.DATA_PORTAL_URI)));
    }

    private static ChannelFactory<Csla.Server.Hosts.IWcfPortal> GetChannelFactory(Type objectType, object criteria)
    {
      if (NetConfig.ProxyType == ProxyType.Embedded)
        return GetChannelFactory();
      Type rootType = objectType;
      ICriterions criterions = criteria as ICriterions;
      if (criterions != null)
      {
        IBusinessObject masterBusiness = criterions.Master as IBusinessObject;
        if (masterBusiness != null && masterBusiness.Root != null)
          rootType = masterBusiness.Root.GetType();
      }
      return GetChannelFactory(NetConfig.GetServicesClusterAddress(ServicesClusterAttribute.Fetch(rootType)));
    }

    #region IDataPortalProxy 成员

    public Csla.Server.DataPortalResult Create(Type objectType, object criteria, Csla.Server.DataPortalContext context)
    {
      NetConfig.InitializeSwitch();
      do
      {
        try
        {
          ChannelFactory<Csla.Server.Hosts.IWcfPortal> channelFactory = GetChannelFactory(objectType, criteria);
          Csla.Server.Hosts.IWcfPortal channel = channelFactory.CreateChannel();
          Csla.Server.Hosts.WcfChannel.WcfResponse response = null;
          try
          {
            response = channel.Create(new Csla.Server.Hosts.WcfChannel.CreateRequest(objectType, criteria, context));
            channelFactory.Close();
          }
          catch
          {
            channelFactory.Abort();
            throw;
          }
          object result = response.Result;
          Exception exception = result as Exception;
          if (exception != null)
            throw exception;
          return (Csla.Server.DataPortalResult)result;
        }
        catch (EndpointNotFoundException)
        {
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
            ChannelFactory<Csla.Server.Hosts.IWcfPortal> channelFactory = GetChannelFactory(objectType, criteria);
            Csla.Server.Hosts.IWcfPortal channel = channelFactory.CreateChannel();
            Csla.Server.Hosts.WcfChannel.WcfResponse response = null;
            try
            {
              response = channel.Fetch(new Csla.Server.Hosts.WcfChannel.FetchRequest(objectType, criteria, context));
              channelFactory.Close();
            }
            catch
            {
              channelFactory.Abort();
              throw;
            }
            Exception exception = response.Result as Exception;
            if (exception != null)
              throw exception;
            result = (Csla.Server.DataPortalResult)response.Result;
            if (actionTime != null)
              ObjectCache.Add(criteria, result.ReturnObject, actionTime.Value);
            break;
          }
          catch (EndpointNotFoundException)
          {
            if (!NetConfig.SwitchServicesAddress())
              throw;
          }
        } while (true);
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
          ChannelFactory<Csla.Server.Hosts.IWcfPortal> channelFactory = GetChannelFactory(obj.GetType(), null);
          Csla.Server.Hosts.IWcfPortal channel = channelFactory.CreateChannel();
          Csla.Server.Hosts.WcfChannel.WcfResponse response = null;
          try
          {
            response = channel.Update(new Csla.Server.Hosts.WcfChannel.UpdateRequest(obj, context));
            channelFactory.Close();
          }
          catch
          {
            channelFactory.Abort();
            throw;
          }
          object result = response.Result;
          Exception exception = result as Exception;
          if (exception != null)
            throw exception;
          return (Csla.Server.DataPortalResult)result;
        }
        catch (EndpointNotFoundException)
        {
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
          ChannelFactory<Csla.Server.Hosts.IWcfPortal> channelFactory = GetChannelFactory(objectType, criteria);
          Csla.Server.Hosts.IWcfPortal channel = channelFactory.CreateChannel();
          Csla.Server.Hosts.WcfChannel.WcfResponse response = null;
          try
          {
            response = channel.Delete(new Csla.Server.Hosts.WcfChannel.DeleteRequest(objectType, criteria, context));
            channelFactory.Close();
          }
          catch
          {
            channelFactory.Abort();
            throw;
          }
          object result = response.Result;
          Exception exception = result as Exception;
          if (exception != null)
            throw exception;
          return (Csla.Server.DataPortalResult)result;
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