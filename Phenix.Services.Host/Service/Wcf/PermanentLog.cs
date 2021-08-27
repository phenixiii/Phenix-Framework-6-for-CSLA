using System;
using System.Reflection;
using System.ServiceModel;
using System.ServiceModel.Activation;
using Phenix.Core.Log;
using Phenix.Core.Mapping;
using Phenix.Services.Host.Core;

namespace Phenix.Services.Host.Service.Wcf
{
  [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerCall)]
  [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
  public sealed class PermanentLog : Phenix.Services.Contract.Wcf.IPermanentLog
  {
    [OperationBehavior(Impersonation = ImpersonationOption.Allowed)]
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

    [OperationBehavior(Impersonation = ImpersonationOption.Allowed)]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
    public object Fetch(string userNumber, string typeName,
      DateTime startTime, DateTime finishTime)
    {
      try
      {
        ServiceManager.CheckActive();
        return PermanentLogHub.Fetch(userNumber, typeName, startTime, finishTime);
      }
      catch (Exception ex)
      {
        return ex;
      }
    }

    [OperationBehavior(Impersonation = ImpersonationOption.Allowed)]
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

    [OperationBehavior(Impersonation = ImpersonationOption.Allowed)]
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


    [OperationBehavior(Impersonation = ImpersonationOption.Allowed)]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
    public object FetchExecuteAction(string typeName, string primaryKey)
    {
      try
      {
        ServiceManager.CheckActive();
        return PermanentLogHub.FetchExecuteAction(typeName, primaryKey);
      }
      catch (Exception ex)
      {
        return ex;
      }
    }

    [OperationBehavior(Impersonation = ImpersonationOption.Allowed)]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
    public object FetchUserExecuteAction(string userNumber, string typeName,
      ExecuteAction action, DateTime startTime, DateTime finishTime)
    {
      try
      {
        ServiceManager.CheckActive();
        return PermanentLogHub.FetchExecuteAction(userNumber, typeName, action, startTime, finishTime);
      }
      catch (Exception ex)
      {
        return ex;
      }
    }

    [OperationBehavior(Impersonation = ImpersonationOption.Allowed)]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
    public void ClearUserExecuteAction(string userNumber, string typeName,
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
  }
}