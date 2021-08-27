using System;
using System.Collections;
using System.Globalization;
using System.Reflection;
using System.ServiceModel;
using System.ServiceModel.Activation;
using Phenix.Business;
using Phenix.Core;
using Phenix.Core.Cache;
using Phenix.Core.Log;
using Phenix.Core.Mapping;
using Phenix.Core.Security;
using Phenix.Services.Host.Core;

namespace Phenix.Services.Host.Service.Wcf
{
  [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerCall)]
  [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
  public sealed class DataPortal : Csla.Server.Hosts.IWcfPortal
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

    [OperationBehavior(Impersonation = ImpersonationOption.Allowed)]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods")]
    public Csla.Server.Hosts.WcfChannel.WcfResponse Create(Csla.Server.Hosts.WcfChannel.CreateRequest request)
    {
      throw new NotSupportedException(MethodBase.GetCurrentMethod().Name);
    }

    [OperationBehavior(Impersonation = ImpersonationOption.Allowed)]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods")]
    public Csla.Server.Hosts.WcfChannel.WcfResponse Fetch(Csla.Server.Hosts.WcfChannel.FetchRequest request)
    {
      try
      {
        ServiceManager.CheckIn(request.Context.Principal);
        DataSecurityHub.CheckIn(request.ObjectType, ExecuteAction.Fetch, request.Context.Principal);

        DateTime dt = DateTime.Now;

        Csla.Server.DataPortalResult result;
        DateTime? actionTime;
        object obj = ObjectCache.Find(request.ObjectType, request.Criteria, out actionTime);
        if (obj != null)
          result = new Csla.Server.DataPortalResult(obj);
        else
        {
          Csla.Server.IDataPortalServer portal = new Csla.Server.DataPortal();
          result = portal.Fetch(request.ObjectType, request.Criteria, request.Context);
          if (actionTime != null)
            ObjectCache.Add(request.Criteria, result.ReturnObject, actionTime.Value);
        }
        //OnDataSecurityChanged(new DataSecurityEventArgs(request.Context != null ? request.Context.Principal.Identity.Name : null));

        //跟踪日志
        if (AppConfig.Debugging)
        {
          int count = result.ReturnObject is IList ? ((IList)result.ReturnObject).Count : result.ReturnObject != null ? 1 : 0;

          Criterions criterions = request.Criteria as Criterions;
          EventLog.SaveLocal(MethodBase.GetCurrentMethod().Name + ' ' + request.ObjectType.FullName +
            (criterions != null && criterions.Criteria != null ? " with " + criterions.Criteria.GetType().FullName : String.Empty) +
            " take " + DateTime.Now.Subtract(dt).TotalMilliseconds.ToString(CultureInfo.InvariantCulture) + " millisecond," +
            " count = " + count);

          PerformanceAnalyse.Default.CheckFetchMaxCount(request.ObjectType.FullName, count, request.Context.Principal);
          PerformanceAnalyse.Default.CheckFetchMaxElapsedTime(request.ObjectType.FullName, DateTime.Now.Subtract(dt).TotalSeconds, count, request.Context.Principal);
        }

        return new Csla.Server.Hosts.WcfChannel.WcfResponse(result);
      }
      catch (Exception ex)
      {
        return new Csla.Server.Hosts.WcfChannel.WcfResponse(ex);
      }
    }

    [OperationBehavior(Impersonation = ImpersonationOption.Allowed)]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods")]
    public Csla.Server.Hosts.WcfChannel.WcfResponse Update(Csla.Server.Hosts.WcfChannel.UpdateRequest request)
    {
      try
      {
        ServiceManager.CheckIn(request.Context.Principal);
        DataSecurityHub.CheckIn(request.Object.GetType(), ExecuteAction.Update, request.Context.Principal);

        DateTime dt = DateTime.Now;

        Csla.Server.IDataPortalServer portal = new Csla.Server.DataPortal();
        //OnDataSecurityChanged(new DataSecurityEventArgs(request.Context != null ? request.Context.Principal.Identity.Name : null));
        Csla.Server.DataPortalResult result = portal.Update(request.Object, request.Context);

        //跟踪日志
        if (AppConfig.Debugging)
          PerformanceAnalyse.Default.CheckSaveMaxElapsedTime(request.Object.GetType().FullName, DateTime.Now.Subtract(dt).TotalSeconds, request.Object is IList ? ((IList)request.Object).Count : 1, request.Context.Principal);

        if (result.ReturnObject is Csla.Core.ICommandObject)
          return new Csla.Server.Hosts.WcfChannel.WcfResponse(result);
        IBusiness business = result.ReturnObject as IBusiness;
        if (business != null && business.NeedRefresh)
          return new Csla.Server.Hosts.WcfChannel.WcfResponse(result);
        return new Csla.Server.Hosts.WcfChannel.WcfResponse(new Csla.Server.DataPortalResult());
      }
      catch (Exception ex)
      {
        return new Csla.Server.Hosts.WcfChannel.WcfResponse(ex);
      }
    }

    [OperationBehavior(Impersonation = ImpersonationOption.Allowed)]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods")]
    public Csla.Server.Hosts.WcfChannel.WcfResponse Delete(Csla.Server.Hosts.WcfChannel.DeleteRequest request)
    {
      try
      {
        ServiceManager.CheckIn(request.Context.Principal);
        DataSecurityHub.CheckIn(request.ObjectType, ExecuteAction.Delete, request.Context.Principal);

        DateTime dt = DateTime.Now;

        Csla.Server.IDataPortalServer portal = new Csla.Server.DataPortal();
        //OnDataSecurityChanged(new DataSecurityEventArgs(request.Context != null ? request.Context.Principal.Identity.Name : null));
        portal.Delete(request.ObjectType, request.Criteria, request.Context);

        //跟踪日志
        if (AppConfig.Debugging)
          PerformanceAnalyse.Default.CheckSaveMaxElapsedTime(request.ObjectType.FullName, DateTime.Now.Subtract(dt).TotalSeconds, -1, request.Context.Principal);

        return new Csla.Server.Hosts.WcfChannel.WcfResponse(new Csla.Server.DataPortalResult());
      }
      catch (Exception ex)
      {
        return new Csla.Server.Hosts.WcfChannel.WcfResponse(ex);
      }
    }

    #endregion
  }
}