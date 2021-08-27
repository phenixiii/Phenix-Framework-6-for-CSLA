using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Reflection;
using Phenix.Core.Log;
using Phenix.Core.Mapping;
using Phenix.Services.Host.Core;

namespace Phenix.Services.Host.Service
{
  public sealed class PermanentLog : MarshalByRefObject, IPermanentLog
  {
    #region 方法

    [System.Runtime.Remoting.Messaging.OneWay]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
    public void Save(string userNumber, string typeName, string message, Exception error)
    {
      try
      {
        PermanentLogHub.Save(userNumber, typeName, message, error);
      }
      catch (Exception ex)
      {
        EventLog.SaveLocal(MethodBase.GetCurrentMethod(), ex);
      }
    }

    public IList<EventLogInfo> Fetch(string userNumber, string typeName,
      DateTime startTime, DateTime finishTime)
    {
      ServiceManager.CheckActive();
      return PermanentLogHub.Fetch(userNumber, typeName, startTime, finishTime);
    }

    [System.Runtime.Remoting.Messaging.OneWay]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
    public void Clear(string userNumber, string typeName,
      DateTime startTime, DateTime finishTime)
    {
      try
      {
        PermanentLogHub.Clear(userNumber, typeName, startTime, finishTime);
      }
      catch (Exception ex)
      {
        EventLog.SaveLocal(MethodBase.GetCurrentMethod(), ex);
      }
    }

    [System.Runtime.Remoting.Messaging.OneWay]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
    public void SaveExecuteAction(string userNumber, string typeName, string primaryKey,
      ExecuteAction action, string log)
    {
      try
      {
        PermanentLogHub.SaveExecuteAction(userNumber, typeName, primaryKey, action, log);
      }
      catch (Exception ex)
      {
        EventLog.SaveLocal(MethodBase.GetCurrentMethod(), ex);
      }
    }

    public IList<string> FetchExecuteAction(string typeName, string primaryKey)
    {
      ServiceManager.CheckActive();
      return PermanentLogHub.FetchExecuteAction(typeName, primaryKey);
    }

    public IList<string> FetchExecuteAction(string userNumber, string typeName,
      ExecuteAction action, DateTime startTime, DateTime finishTime)
    {
      ServiceManager.CheckActive();
      return PermanentLogHub.FetchExecuteAction(userNumber, typeName, action, startTime, finishTime);
    }

    [System.Runtime.Remoting.Messaging.OneWay]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
    public void ClearExecuteAction(string userNumber, string typeName,
      ExecuteAction action, DateTime startTime, DateTime finishTime)
    {
      try
      {
        PermanentLogHub.ClearExecuteAction(userNumber, typeName, action, startTime, finishTime);
      }
      catch (Exception ex)
      {
        EventLog.SaveLocal(MethodBase.GetCurrentMethod(), ex);
      }
    }

    #region 应用服务不支持传事务

    public void SaveRenovate(DbTransaction transaction, string tableName, ExecuteAction action, IList<FieldValue> fieldValues)
    {
      throw new NotSupportedException(MethodBase.GetCurrentMethod().Name);
    }
    
    #endregion

    #endregion
  }
}