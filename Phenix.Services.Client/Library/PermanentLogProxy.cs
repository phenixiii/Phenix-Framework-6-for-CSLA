using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Net.Sockets;
using System.Reflection;
using Phenix.Core.Log;
using Phenix.Core.Mapping;
using Phenix.Core.Net;
using Phenix.Services.Contract;

namespace Phenix.Services.Client.Library
{
  internal class PermanentLogProxy : IPermanentLog
  {
    #region 属性

    private IPermanentLog _service;
    private IPermanentLog Service
    {
      get
      {
        if (_service == null)
        {
          RemotingHelper.RegisterClientChannel();
          _service = (IPermanentLog)RemotingHelper.CreateRemoteObjectProxy(typeof(IPermanentLog), ServicesInfo.PERMANENT_LOG_URI);
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

    #region IPermanentLog 成员

    public void Save(string userNumber, string typeName, string message, Exception error)
    {
      NetConfig.InitializeSwitch();
      do
      {
        try
        {
          Service.Save(userNumber, typeName, message, error);
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

    public IList<EventLogInfo> Fetch(string userNumber, string typeName,
      DateTime startTime, DateTime finishTime)
    {
      NetConfig.InitializeSwitch();
      do
      {
        try
        {
          return Service.Fetch(userNumber, typeName, startTime, finishTime);
        }
        catch (SocketException)
        {
          InvalidateCache();
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
          Service.Clear(userNumber, typeName, startTime, finishTime);
          return;
        }
        catch (SocketException)
        {
          InvalidateCache();
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
          Service.SaveExecuteAction(userNumber, typeName, primaryKey, action, log);
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

    public IList<string> FetchExecuteAction(string typeName, string primaryKey)
    {
      NetConfig.InitializeSwitch();
      do
      {
        try
        {
          return Service.FetchExecuteAction(typeName, primaryKey);
        }
        catch (SocketException)
        {
          InvalidateCache();
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
          return Service.FetchExecuteAction(userNumber, typeName, action, startTime, finishTime);
        }
        catch (SocketException)
        {
          InvalidateCache();
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
          Service.ClearExecuteAction(userNumber, typeName, action, startTime, finishTime);
          return;
        }
        catch (SocketException)
        {
          InvalidateCache();
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