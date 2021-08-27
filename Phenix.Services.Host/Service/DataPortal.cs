using System;
using System.Collections;
using System.Globalization;
using System.Reflection;
using Phenix.Business;
using Phenix.Core;
using Phenix.Core.Cache;
using Phenix.Core.Log;
using Phenix.Core.Mapping;
using Phenix.Core.Security;
using Phenix.Services.Host.Core;

namespace Phenix.Services.Host.Service
{

  public sealed class DataPortal : MarshalByRefObject, Csla.Server.IDataPortalServer
  {
    #region 事件

    //internal static event Action<DataSecurityEventArgs> DataSecurityChanged;
    //private static void OnDataSecurityChanged(DataSecurityEventArgs e)
    //{
    //  Action<DataSecurityEventArgs> action = DataSecurityChanged;
    //  if (action != null)
    //    action(e);
    //}

    #endregion

    #region 方法

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods")]
    public Csla.Server.DataPortalResult Create(Type objectType, object criteria, Csla.Server.DataPortalContext context)
    {
      throw new NotSupportedException(MethodBase.GetCurrentMethod().Name);
    }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods")]
    public Csla.Server.DataPortalResult Fetch(Type objectType, object criteria, Csla.Server.DataPortalContext context)
    {
      ServiceManager.CheckIn(context.Principal);
      DataSecurityHub.CheckIn(objectType, ExecuteAction.Fetch, context.Principal);

      DateTime dt = DateTime.Now;

      Csla.Server.DataPortalResult result;
      DateTime? actionTime;
      object obj = ObjectCache.Find(objectType, criteria, out actionTime);
      if (obj != null)
        result = new Csla.Server.DataPortalResult(obj);
      else
      {
        Csla.Server.IDataPortalServer portal = new Csla.Server.DataPortal();
        result = portal.Fetch(objectType, criteria, context);
        if (actionTime != null)
          ObjectCache.Add(criteria, result.ReturnObject, actionTime.Value);
      }
      //OnDataSecurityChanged(new DataSecurityEventArgs(context.Principal.Identity.Name));

      //跟踪日志
      if (AppConfig.Debugging)
      {
        int count = result.ReturnObject is IList ? ((IList)result.ReturnObject).Count : result.ReturnObject != null ? 1 : 0;

        Criterions criterions = criteria as Criterions;
        EventLog.SaveLocal(MethodBase.GetCurrentMethod().Name + ' ' + objectType.FullName +
          (criterions != null && criterions.Criteria != null ? " with " + criterions.Criteria.GetType().FullName : String.Empty) +
          " take " + DateTime.Now.Subtract(dt).TotalMilliseconds.ToString(CultureInfo.InvariantCulture) + " millisecond," +
          " count = " + count);

        PerformanceAnalyse.Default.CheckFetchMaxCount(objectType.FullName, count, context.Principal);
        PerformanceAnalyse.Default.CheckFetchMaxElapsedTime(objectType.FullName, DateTime.Now.Subtract(dt).TotalSeconds, count, context.Principal);
      }

      return result;
    }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods")]
    public Csla.Server.DataPortalResult Update(object obj, Csla.Server.DataPortalContext context)
    {
      ServiceManager.CheckIn(context.Principal);
      DataSecurityHub.CheckIn(obj.GetType(), ExecuteAction.Update, context.Principal);

      DateTime dt = DateTime.Now;

      Csla.Server.IDataPortalServer portal = new Csla.Server.DataPortal();
      //OnDataSecurityChanged(new DataSecurityEventArgs(context.Principal.Identity.Name));
      Csla.Server.DataPortalResult result = portal.Update(obj, context);

      //跟踪日志
      if (AppConfig.Debugging)
        PerformanceAnalyse.Default.CheckSaveMaxElapsedTime(obj.GetType().FullName, DateTime.Now.Subtract(dt).TotalSeconds, obj is IList ? ((IList)obj).Count : 1, context.Principal);

      if (result.ReturnObject is Csla.Core.ICommandObject)
        return result;
      IBusiness business = result.ReturnObject as IBusiness;
      if (business != null && business.NeedRefresh)
        return result;
      return new Csla.Server.DataPortalResult();
    }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods")]
    public Csla.Server.DataPortalResult Delete(Type objectType, object criteria, Csla.Server.DataPortalContext context)
    {
      ServiceManager.CheckIn(context.Principal);
      DataSecurityHub.CheckIn(objectType, ExecuteAction.Delete, context.Principal);

      DateTime dt = DateTime.Now;

      Csla.Server.IDataPortalServer portal = new Csla.Server.DataPortal();
      //OnDataSecurityChanged(new DataSecurityEventArgs(context.Principal.Identity.Name));
      portal.Delete(objectType, criteria, context);

      //跟踪日志
      if (AppConfig.Debugging)
        PerformanceAnalyse.Default.CheckSaveMaxElapsedTime(objectType.FullName, DateTime.Now.Subtract(dt).TotalSeconds, -1, context.Principal);

      return new Csla.Server.DataPortalResult();
    }

    #endregion
  }
}
