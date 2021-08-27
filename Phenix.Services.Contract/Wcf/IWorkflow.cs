using System;
using System.ServiceModel;
using Phenix.Core.Security;
using Phenix.Core.Workflow;

namespace Phenix.Services.Contract.Wcf
{
  [ServiceContract]
  public interface IWorkflow
  {
    #region WorkflowInfo

    [OperationContract]
    [UseNetDataContract]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate")]
    object GetWorkflowInfos();

    [OperationContract]
    [UseNetDataContract]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate")]
    object GetWorkflowInfoChangedTime();

    [OperationContract(IsOneWay = true)]
    [UseNetDataContract]
    void WorkflowInfoHasChanged();
    
    [OperationContract]
    [UseNetDataContract]
    object AddWorkflowInfo(string typeNamespace, string typeName, string caption, string xamlCode, UserIdentity identity);

    [OperationContract]
    [UseNetDataContract]
    object DisableWorkflowInfo(string typeNamespace, string typeName, UserIdentity identity);

    #endregion

    #region WorkflowInstance

    [OperationContract]
    [UseNetDataContract]
    object SaveWorkflowInstance(Guid id, string typeNamespace, string typeName, string content, TaskContext taskContext);

    [OperationContract]
    [UseNetDataContract]
    object FetchWorkflowInstance(Guid id);

    [OperationContract]
    [UseNetDataContract]
    object ClearWorkflowInstance(Guid id);

    #endregion

    #region WorkflowTask

    [OperationContract]
    [UseNetDataContract]
    object DispatchWorkflowTask(Guid id, string bookmarkName,
      string pluginAssemblyName, string workerRole, string caption, string message, bool urgent);

    [OperationContract]
    [UseNetDataContract]
    object ReceiveWorkflowTask(Guid id, string bookmarkName);

    [OperationContract]
    [UseNetDataContract]
    object HoldWorkflowTask(Guid id, string bookmarkName);

    [OperationContract]
    [UseNetDataContract]
    object AbortWorkflowTask(Guid id, string bookmarkName);

    [OperationContract]
    [UseNetDataContract]
    object CompleteWorkflowTask(Guid id, string bookmarkName);

    [OperationContract]
    [UseNetDataContract]
    object ProceedWorkflow(WorkflowTaskInfo workflowTaskInfo);

    [OperationContract]
    [UseNetDataContract]
    object FetchWorkflowTask(TaskState taskState, DateTime startDispatchTime, DateTime finishDispatchTime, UserIdentity identity);

    #endregion
  }
}