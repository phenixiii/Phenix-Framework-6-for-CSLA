using System;
using System.Reflection;
using System.ServiceModel;
using System.ServiceModel.Activation;
using Phenix.Core.Log;
using Phenix.Core.Security;
using Phenix.Core.Workflow;
using Phenix.Services.Host.Core;

namespace Phenix.Services.Host.Service.Wcf
{
  [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerCall)]
  [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
  public sealed class Workflow : Phenix.Services.Contract.Wcf.IWorkflow
  {
    #region WorkflowInfo

    [OperationBehavior(Impersonation = ImpersonationOption.Allowed)]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
    public object GetWorkflowInfos()
    {
      try
      {
        ServiceManager.CheckActive();
        return WorkflowHub.WorkflowInfos;
      }
      catch (Exception ex)
      {
        return ex;
      }
    }

    [OperationBehavior(Impersonation = ImpersonationOption.Allowed)]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
    public object GetWorkflowInfoChangedTime()
    {
      try
      {
        ServiceManager.CheckActive();
        return WorkflowHub.WorkflowInfoChangedTime;
      }
      catch (Exception ex)
      {
        return ex;
      }
    }

    [OperationBehavior(Impersonation = ImpersonationOption.Allowed)]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
    public void WorkflowInfoHasChanged()
    {
      try
      {
        WorkflowHub.WorkflowInfoHasChanged();
      }
      catch (Exception ex)
      {
        EventLog.SaveLocal(MethodBase.GetCurrentMethod(), ex);
      }
    }

    [OperationBehavior(Impersonation = ImpersonationOption.Allowed)]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
    public object AddWorkflowInfo(string typeNamespace, string typeName, string caption, string xamlCode, UserIdentity identity)
    {
      try
      {
        ServiceManager.CheckIn(identity);
        DataSecurityContext context = DataSecurityHub.CheckIn(identity, false);
        WorkflowHub.AddWorkflowInfo(typeNamespace, typeName, caption, xamlCode, context.Identity);
        return null;
      }
      catch (Exception ex)
      {
        return ex;
      }
    }

    [OperationBehavior(Impersonation = ImpersonationOption.Allowed)]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
    public object DisableWorkflowInfo(string typeNamespace, string typeName, UserIdentity identity)
    {
      try
      {
        ServiceManager.CheckIn(identity);
        DataSecurityContext context = DataSecurityHub.CheckIn(identity, false);
        WorkflowHub.DisableWorkflowInfo(typeNamespace, typeName, context.Identity);
        return null;
      }
      catch (Exception ex)
      {
        return ex;
      }
    }

    #endregion

    #region WorkflowInstance

    [OperationBehavior(Impersonation = ImpersonationOption.Allowed)]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
    public object SaveWorkflowInstance(Guid id, string typeNamespace, string typeName, string content, TaskContext taskContext)
    {
      try
      {
        ServiceManager.CheckActive();
        WorkflowHub.SaveWorkflowInstance(id, typeNamespace, typeName, content, taskContext);
        return null;
      }
      catch (Exception ex)
      {
        return ex;
      }
    }

    [OperationBehavior(Impersonation = ImpersonationOption.Allowed)]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
    public object FetchWorkflowInstance(Guid id)
    {
      try
      {
        ServiceManager.CheckActive();
        return WorkflowHub.FetchWorkflowInstance(id);
      }
      catch (Exception ex)
      {
        return ex;
      }
    }

    [OperationBehavior(Impersonation = ImpersonationOption.Allowed)]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
    public object ClearWorkflowInstance(Guid id)
    {
      try
      {
        ServiceManager.CheckActive();
        WorkflowHub.ClearWorkflowInstance(id);
        return null;
      }
      catch (Exception ex)
      {
        return ex;
      }
    }

    #endregion

    #region WorkflowTask

    [OperationBehavior(Impersonation = ImpersonationOption.Allowed)]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
    public object DispatchWorkflowTask(Guid id, string bookmarkName,
      string pluginAssemblyName, string workerRole, string caption, string message, bool urgent)
    {
      try
      {
        ServiceManager.CheckActive();
        WorkflowHub.DispatchWorkflowTask(id, bookmarkName, pluginAssemblyName, workerRole, caption, message, urgent);
        return null;
      }
      catch (Exception ex)
      {
        return ex;
      }
    }

    [OperationBehavior(Impersonation = ImpersonationOption.Allowed)]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
    public object ReceiveWorkflowTask(Guid id, string bookmarkName)
    {
      try
      {
        ServiceManager.CheckActive();
        WorkflowHub.ReceiveWorkflowTask(id, bookmarkName);
        return null;
      }
      catch (Exception ex)
      {
        return ex;
      }
    }

    [OperationBehavior(Impersonation = ImpersonationOption.Allowed)]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
    public object HoldWorkflowTask(Guid id, string bookmarkName)
    {
      try
      {
        ServiceManager.CheckActive();
        WorkflowHub.HoldWorkflowTask(id, bookmarkName);
        return null;
      }
      catch (Exception ex)
      {
        return ex;
      }
    }

    [OperationBehavior(Impersonation = ImpersonationOption.Allowed)]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
    public object AbortWorkflowTask(Guid id, string bookmarkName)
    {
      try
      {
        ServiceManager.CheckActive();
        WorkflowHub.AbortWorkflowTask(id, bookmarkName);
        return null;
      }
      catch (Exception ex)
      {
        return ex;
      }
    }

    [OperationBehavior(Impersonation = ImpersonationOption.Allowed)]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
    public object ProceedWorkflow(WorkflowTaskInfo workflowTaskInfo)
    {
      try
      {
        ServiceManager.CheckActive();
        WorkflowHub.ProceedWorkflow(workflowTaskInfo);
        return null;
      }
      catch (Exception ex)
      {
        return ex;
      }
    }

    [OperationBehavior(Impersonation = ImpersonationOption.Allowed)]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
    public object CompleteWorkflowTask(Guid id, string bookmarkName)
    {
      try
      {
        ServiceManager.CheckActive();
        WorkflowHub.CompleteWorkflowTask(id, bookmarkName);
        return null;
      }
      catch (Exception ex)
      {
        return ex;
      }
    }

    [OperationBehavior(Impersonation = ImpersonationOption.Allowed)]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
    public object FetchWorkflowTask(TaskState taskState, DateTime startDispatchTime, DateTime finishDispatchTime, UserIdentity identity)
    {
      try
      {
        ServiceManager.CheckIn(identity);
        DataSecurityContext context = DataSecurityHub.CheckIn(identity, false);
        return WorkflowHub.FetchWorkflowTask(taskState, startDispatchTime, finishDispatchTime, context.Identity);
      }
      catch (Exception ex)
      {
        return ex;
      }
    }

    #endregion
  }
}