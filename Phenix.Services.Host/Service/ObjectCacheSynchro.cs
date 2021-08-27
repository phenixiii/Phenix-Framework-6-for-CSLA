using System;
using System.Collections.Generic;
using System.Reflection;
using Phenix.Core.Cache;
using Phenix.Core.Log;

namespace Phenix.Services.Host.Service
{
  public sealed class ObjectCacheSynchro : MarshalByRefObject, IObjectCacheSynchro
  {
    #region 方法

    public DateTime? GetActionTime(string typeName)
    {
      return ObjectCache.GetActionTime(typeName);
    }

    [System.Runtime.Remoting.Messaging.OneWay]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
    public void ClearAll()
    {
      try
      {
        ObjectCache.ClearAll();
      }
      catch (Exception ex)
      {
        EventLog.SaveLocal(MethodBase.GetCurrentMethod(), ex);
      }
    }

    [System.Runtime.Remoting.Messaging.OneWay]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
    public void Clear(IList<string> typeNames)
    {
      try
      {
        ObjectCache.Clear(typeNames);
      }
      catch (Exception ex)
      {
        EventLog.SaveLocal(MethodBase.GetCurrentMethod(), ex);
      }
    }

    [System.Runtime.Remoting.Messaging.OneWay]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
    public void RecordHasChanged(string tableName)
    {
      try
      {
        ObjectCache.RecordHasChanged(tableName);
      }
      catch (Exception ex)
      {
        EventLog.SaveLocal(MethodBase.GetCurrentMethod(), ex);
      }
    }

    #endregion
  }
}