using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Reflection;
using System.ServiceModel;
using Phenix.Core.Log;
using Phenix.Core.Mapping;
using Phenix.Core.Net;
using Phenix.Services.Contract;

namespace Phenix.Services.Client.Library.Wcf
{
  internal class PermanentLogProxy : IPermanentLog
  {
    #region 方法

    private static ChannelFactory<Phenix.Services.Contract.Wcf.IPermanentLog> GetChannelFactory()
    {
      return new ChannelFactory<Phenix.Services.Contract.Wcf.IPermanentLog>(WcfHelper.CreateBinding(),
        new EndpointAddress(WcfHelper.CreateUrl(NetConfig.ServicesAddress, ServicesInfo.PERMANENT_LOG_URI)));
    }

    #region IPermanentLog 成员

    public void Save(string userNumber, string typeName, string message, Exception error)
    {
      NetConfig.InitializeSwitch();
      do
      {
        try
        {
          ChannelFactory<Phenix.Services.Contract.Wcf.IPermanentLog> channelFactory = GetChannelFactory();
          Phenix.Services.Contract.Wcf.IPermanentLog channel = channelFactory.CreateChannel();
          try
          {
            channel.Save(userNumber, typeName, message, error);
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

    public IList<EventLogInfo> Fetch(string userNumber, string typeName,
      DateTime startTime, DateTime finishTime)
    {
      NetConfig.InitializeSwitch();
      do
      {
        try
        {
          ChannelFactory<Phenix.Services.Contract.Wcf.IPermanentLog> channelFactory = GetChannelFactory();
          Phenix.Services.Contract.Wcf.IPermanentLog channel = channelFactory.CreateChannel();
          object result = null;
          try
          {
            result = channel.Fetch(userNumber, typeName, startTime, finishTime);
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
          return (IList<EventLogInfo>)result;
        }
        catch (EndpointNotFoundException)
        {
          if (!NetConfig.SwitchServicesAddress())
            throw;
        }
      } while (true);
    }

    public void Clear(string userNumber, string typeName,
      DateTime startTime, DateTime finishTime)
    {
      NetConfig.InitializeSwitch();
      do
      {
        try
        {
          ChannelFactory<Phenix.Services.Contract.Wcf.IPermanentLog> channelFactory = GetChannelFactory();
          Phenix.Services.Contract.Wcf.IPermanentLog channel = channelFactory.CreateChannel();
          try
          {
            channel.Clear(userNumber, typeName, startTime, finishTime);
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

    public void SaveExecuteAction(string userNumber, string typeName, string primaryKey,
      ExecuteAction action, string log)
    {
      NetConfig.InitializeSwitch();
      do
      {
        try
        {
          ChannelFactory<Phenix.Services.Contract.Wcf.IPermanentLog> channelFactory = GetChannelFactory();
          Phenix.Services.Contract.Wcf.IPermanentLog channel = channelFactory.CreateChannel();
          try
          {
            channel.SaveExecuteAction(userNumber, typeName, primaryKey, action, log);
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

    public IList<string> FetchExecuteAction(string typeName, string primaryKey)
    {
      NetConfig.InitializeSwitch();
      do
      {
        try
        {
          ChannelFactory<Phenix.Services.Contract.Wcf.IPermanentLog> channelFactory = GetChannelFactory();
          Phenix.Services.Contract.Wcf.IPermanentLog channel = channelFactory.CreateChannel();
          object result = null;
          try
          {
            result = channel.FetchExecuteAction(typeName, primaryKey);
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
          return (IList<string>)result;
        }
        catch (EndpointNotFoundException)
        {
          if (!NetConfig.SwitchServicesAddress())
            throw;
        }
      } while (true);
    }


    public IList<string> FetchExecuteAction(string userNumber, string typeName,
      ExecuteAction action, DateTime startTime, DateTime finishTime)
    {
      NetConfig.InitializeSwitch();
      do
      {
        try
        {
          ChannelFactory<Phenix.Services.Contract.Wcf.IPermanentLog> channelFactory = GetChannelFactory();
          Phenix.Services.Contract.Wcf.IPermanentLog channel = channelFactory.CreateChannel();
          object result = null;
          try
          {
            result = channel.FetchUserExecuteAction(userNumber, typeName, action, startTime, finishTime);
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
          return (IList<string>)result;
        }
        catch (EndpointNotFoundException)
        {
          if (!NetConfig.SwitchServicesAddress())
            throw;
        }
      } while (true);
    }

    public void ClearExecuteAction(string userNumber, string typeName,
      ExecuteAction action, DateTime startTime, DateTime finishTime)
    {
      NetConfig.InitializeSwitch();
      do
      {
        try
        {
          ChannelFactory<Phenix.Services.Contract.Wcf.IPermanentLog> channelFactory = GetChannelFactory();
          Phenix.Services.Contract.Wcf.IPermanentLog channel = channelFactory.CreateChannel();
          try
          {
            channel.ClearUserExecuteAction(userNumber, typeName, action, startTime, finishTime);
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

    #region 应用服务不支持传事务

    public void SaveRenovate(DbTransaction transaction, string tableName, ExecuteAction action, IList<FieldValue> fieldValues)
    {
      throw new NotSupportedException(MethodBase.GetCurrentMethod().Name);
    }

    #endregion

    #endregion

    #endregion
  }
}