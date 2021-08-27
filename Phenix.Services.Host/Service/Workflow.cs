using System;
using System.Collections.Generic;
using System.Reflection;
using Phenix.Core.Log;
using Phenix.Core.Security;
using Phenix.Core.Workflow;
using Phenix.Services.Host.Core;

namespace Phenix.Services.Host.Service
{
  public sealed class Workflow : MarshalByRefObject, IWorkflow
  {
    #region 属性

    public IDictionary<string, WorkflowInfo> WorkflowInfos
    {
      get
      {
        ServiceManager.CheckActive();
        return WorkflowHub.WorkflowInfos;
      }
    }

    public DateTime? WorkflowInfoChangedTime
    {
      get
      {
        ServiceManager.CheckActive();
        return WorkflowHub.WorkflowInfoChangedTime;
      }
    }

    #endregion

    #region 方法

    #region WorkflowInfo

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
    [System.Runtime.Remoting.Messaging.OneWay]
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

    public void AddWorkflowInfo(string typeNamespace, string typeName, string caption, string xamlCode, UserIdentity identity)
    {
      ServiceManager.CheckIn(identity);
      DataSecurityContext context = DataSecurityHub.CheckIn(identity, false);
      WorkflowHub.AddWorkflowInfo(typeNamespace, typeName, caption, xamlCode, context.Identity);
    }

    public void DisableWorkflowInfo(string typeNamespace, string typeName, UserIdentity identity)
    {
      ServiceManager.CheckIn(identity);
      DataSecurityContext context = DataSecurityHub.CheckIn(identity, false);
      WorkflowHub.DisableWorkflowInfo(typeNamespace, typeName, context.Identity);
    }

    #endregion

    #region WorkflowInstance

    public void SaveWorkflowInstance(Guid id, string typeNamespace, string typeName, string content, TaskContext taskContext)
    {
      ServiceManager.CheckActive();
      WorkflowHub.SaveWorkflowInstance(id, typeNamespace, typeName, content, taskContext);
    }

    public string FetchWorkflowInstance(Guid id)
    {
      ServiceManager.CheckActive();
      return WorkflowHub.FetchWorkflowInstance(id);
    }

    public void ClearWorkflowInstance(Guid id)
    {
      ServiceManager.CheckActive();
      WorkflowHub.ClearWorkflowInstance(id);
    }

    #endregion

    #region WorkflowTask

    public void DispatchWorkflowTask(Guid id, string bookmarkName,
      string pluginAssemblyName, string workerRole, string caption, string message, bool urgent)
    {
      ServiceManager.CheckActive();
      WorkflowHub.DispatchWorkflowTask(id, bookmarkName, pluginAssemblyName, workerRole, caption, message, urgent);
    }

    public void ReceiveWorkflowTask(Guid id, string bookmarkName)
    {
      ServiceManager.CheckActive();
      WorkflowHub.ReceiveWorkflowTask(id, bookmarkName);
    }

    public void HoldWorkflowTask(Guid id, string bookmarkName)
    {
      ServiceManager.CheckActive();
      WorkflowHub.HoldWorkflowTask(id, bookmarkName);
    }

    public void AbortWorkflowTask(Guid id, string bookmarkName)
    {
      ServiceManager.CheckActive();
      WorkflowHub.AbortWorkflowTask(id, bookmarkName);
    }

    public void ProceedWorkflow(WorkflowTaskInfo workflowTaskInfo)
    {
      ServiceManager.CheckActive();
      WorkflowHub.ProceedWorkflow(workflowTaskInfo);
    }

    public void CompleteWorkflowTask(Guid id, string bookmarkName)
    {
      ServiceManager.CheckActive();
      WorkflowHub.CompleteWorkflowTask(id, bookmarkName);
    }

    public IList<WorkflowTaskInfo> FetchWorkflowTask(TaskState taskState, DateTime startDispatchTime, DateTime finishDispatchTime, UserIdentity identity)
    {
      ServiceManager.CheckIn(identity);
      DataSecurityContext context = DataSecurityHub.CheckIn(identity, false);
      return WorkflowHub.FetchWorkflowTask(taskState, startDispatchTime, finishDispatchTime, context.Identity);
    }

    #endregion

    #endregion
  }
}