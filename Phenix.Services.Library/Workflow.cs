#if Top
using System.Collections.ObjectModel;
#endif

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Reflection;
using Phenix.Core.Cache;
using Phenix.Core.Data;
using Phenix.Core.Log;
using Phenix.Core.Rule;
using Phenix.Core.Security;
using Phenix.Core.Workflow;

namespace Phenix.Services.Library
{
  internal class Workflow : IWorkflow
  {
    #region 属性

    public IDictionary<string, WorkflowInfo> WorkflowInfos
    {
      get { return DefaultDatabase.ExecuteGet(ExecuteGetWorkflowInfos); } //for Oracle LONG column
    }

    public DateTime? WorkflowInfoChangedTime
    {
      get { return DefaultDatabase.ExecuteGet(ExecuteGetWorkflowInfoChangedTime); }
    }

    #endregion

    #region 方法

    #region WorkflowInfo

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
    private static IDictionary<string, WorkflowInfo> ExecuteGetWorkflowInfos(DbConnection connection)
    {
      Dictionary<string, WorkflowInfo> result = new Dictionary<string, WorkflowInfo>(StringComparer.Ordinal);
      using (DataReader reader = new DataReader(connection,
@"select WF_Namespace, WF_TypeName, WF_Caption, WF_XamlCode,
  WF_Create_UserNumber, WF_Create_Time, WF_Change_UserNumber, WF_Change_Time,
  WF_Disable_UserNumber, WF_Disable_Time
  from PH_Workflow",
        CommandBehavior.SingleResult, false))
      {
        while (reader.Read())
          result.Add(Phenix.Core.Reflection.Utilities.AssembleFullTypeName(reader.GetNullableString(0), reader.GetNullableString(1)),
            new WorkflowInfo(reader.GetNullableString(0), reader.GetNullableString(1), reader.GetNullableString(2), reader.GetNullableString(3),
              reader.GetNullableString(4), reader.GetDateTime(5), reader.GetNullableString(6), reader.GetNullableDateTime(7),
              reader.GetNullableString(8), reader.GetNullableDateTime(9)));
      }
#if Top
      return new ReadOnlyDictionary<string, WorkflowInfo>(result);
#else
      return result;
#endif
    }

    private static DateTime? ExecuteGetWorkflowInfoChangedTime(DbConnection connection)
    {
      using (DataReader reader = new DataReader(connection,
@"select SI_WorkflowInfoChangedTime
  from PH_SystemInfo
  where SI_Name = :SI_Name",
        CommandBehavior.SingleRow, false))
      {
        reader.CreateParameter("SI_Name", Phenix.Core.AppConfig.SYSTEM_NAME);
        if (reader.Read())
          return reader.GetNullableDateTime(0);
      }
      return null;
    }

    public void WorkflowInfoHasChanged()
    {
      DefaultDatabase.Execute(ExecuteWorkflowInfoHasChanged);
    }

    private static void ExecuteWorkflowInfoHasChanged(DbTransaction transaction)
    {
      using (DbCommand command = DbCommandHelper.CreateCommand(transaction,
@"update PH_SystemInfo set
    SI_WorkflowInfoChangedTime = sysdate
  where SI_Name = :SI_Name"))
      {
        DbCommandHelper.CreateParameter(command, "SI_Name", Phenix.Core.AppConfig.SYSTEM_NAME);
        DbCommandHelper.ExecuteNonQuery(command, false);
      }
    }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
    public void AddWorkflowInfo(string typeNamespace, string typeName, string caption, string xamlCode, UserIdentity identity)
    {
      try
      {
        DefaultDatabase.Execute(ExecuteAddWorkflowInfo, typeNamespace, typeName, caption, xamlCode, identity.UserNumber); //for Oracle LONG column
        ObjectCache.RecordHasChanged("PH_Workflow");
      }
      catch (Exception ex)
      {
        EventLog.SaveLocal(MethodBase.GetCurrentMethod(), typeNamespace + "." + typeName, ex);
      }
    }

    private static void ExecuteAddWorkflowInfo(DbTransaction transaction, string typeNamespace, string typeName, string caption, string xamlCode, string userNumber)
    {
      bool succeed;
      using (DbCommand command = DbCommandHelper.CreateCommand(transaction,
@"update PH_Workflow set
    WF_Caption = :WF_Caption,
    WF_Change_UserNumber = :WF_Change_UserNumber,
    WF_Change_Time = sysdate,
    WF_Disable_Time = null
  where WF_Namespace = :WF_Namespace and WF_TypeName = :WF_TypeName"))
      {
        DbCommandHelper.CreateParameter(command, "WF_Caption", caption);
        DbCommandHelper.CreateParameter(command, "WF_Change_UserNumber", userNumber);
        DbCommandHelper.CreateParameter(command, "WF_Namespace", typeNamespace);
        DbCommandHelper.CreateParameter(command, "WF_TypeName", typeName);
        succeed = DbCommandHelper.ExecuteNonQuery(command, false) != 0;
      }
      if (!succeed)
      {
        using (DbCommand command = DbCommandHelper.CreateCommand(transaction,
@"insert into PH_Workflow
  (WF_ID, WF_Namespace, WF_TypeName, WF_Caption, WF_Create_UserNumber, WF_Create_Time)
  values
  (:WF_ID, :WF_Namespace, :WF_TypeName, :WF_Caption, :WF_Create_UserNumber, sysdate)"))
        {
          DbCommandHelper.CreateParameter(command, "WF_ID", Sequence.Value);
          DbCommandHelper.CreateParameter(command, "WF_Namespace", typeNamespace);
          DbCommandHelper.CreateParameter(command, "WF_TypeName", typeName);
          DbCommandHelper.CreateParameter(command, "WF_Caption", caption);
          DbCommandHelper.CreateParameter(command, "WF_Create_UserNumber", userNumber);
          DbCommandHelper.ExecuteNonQuery(command, false);
        }
      }
      using (DbCommand command = DbCommandHelper.CreateCommand(transaction,
@"update PH_Workflow set
    WF_XamlCode = :WF_XamlCode
  where WF_Namespace = :WF_Namespace and WF_TypeName = :WF_TypeName"))
      {
        DbCommandHelper.CreateParameter(command, "WF_XamlCode", xamlCode);
        DbCommandHelper.CreateParameter(command, "WF_Namespace", typeNamespace);
        DbCommandHelper.CreateParameter(command, "WF_TypeName", typeName);
        DbCommandHelper.ExecuteNonQuery(command, false);
      }
    }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
    public void DisableWorkflowInfo(string typeNamespace, string typeName, UserIdentity identity)
    {
      try
      {
        DefaultDatabase.Execute(ExecuteDisableWorkflowInfo, typeNamespace, typeName, identity.UserNumber);
        ObjectCache.RecordHasChanged("PH_Workflow");
      }
      catch (Exception ex)
      {
        EventLog.SaveLocal(MethodBase.GetCurrentMethod(), typeNamespace + "." + typeName, ex);
      }
    }

    private static void ExecuteDisableWorkflowInfo(DbTransaction transaction, string typeNamespace, string typeName, string userNumber)
    {
      using (DbCommand command = DbCommandHelper.CreateCommand(transaction,
@"update PH_Workflow set
    WF_Disable_UserNumber = :WF_Disable_UserNumber,
    WF_Disable_Time = sysdate
  where WF_Namespace = :WF_Namespace and WF_TypeName = :WF_TypeName"))
      {
        DbCommandHelper.CreateParameter(command, "WF_Disable_UserNumber", userNumber);
        DbCommandHelper.CreateParameter(command, "WF_Namespace", typeNamespace);
        DbCommandHelper.CreateParameter(command, "WF_TypeName", typeName);
        DbCommandHelper.ExecuteNonQuery(command, false);
      }
    }
    
    #endregion

    #region WorkflowInstance

    public void SaveWorkflowInstance(Guid id, string typeNamespace, string typeName, string content, TaskContext taskContext)
    {
      DefaultDatabase.Execute(ExecuteSaveWorkflowInstance, id, typeNamespace, typeName, content, taskContext); //for Oracle LONG column
    }

    private static void ExecuteSaveWorkflowInstance(DbTransaction transaction, Guid id, string typeNamespace, string typeName, string content, TaskContext taskContext)
    {
      bool succeed;
      using (DbCommand command = DbCommandHelper.CreateCommand(transaction,
@"update PH_Workflow_Instance set
    WI_Time = sysdate
  where WI_ID = :WI_ID"))
      {
        DbCommandHelper.CreateParameter(command, "WI_ID", id);
        succeed = DbCommandHelper.ExecuteNonQuery(command, false) != 0;
      }
      if (!succeed)
      {
        using (DbCommand command = DbCommandHelper.CreateCommand(transaction,
@"insert into PH_Workflow_Instance
  (WI_ID, WI_WF_Namespace, WI_WF_TypeName, WI_Time)
  values
  (:WI_ID, :WI_WF_Namespace, :WI_WF_TypeName, sysdate)"))
        {
          DbCommandHelper.CreateParameter(command, "WI_ID", id);
          DbCommandHelper.CreateParameter(command, "WI_WF_Namespace", typeNamespace);
          DbCommandHelper.CreateParameter(command, "WI_WF_TypeName", typeName);
          DbCommandHelper.ExecuteNonQuery(command, false);
        }
      }
      using (DbCommand command = DbCommandHelper.CreateCommand(transaction,
@"update PH_Workflow_Instance set
    WI_Content = :WI_Content
  where WI_ID = :WI_ID"))
      {
        DbCommandHelper.CreateParameter(command, "WI_Content", content);
        DbCommandHelper.CreateParameter(command, "WI_ID", id);
        DbCommandHelper.ExecuteNonQuery(command, false);
      }

      using (DbCommand command = DbCommandHelper.CreateCommand(transaction,
@"update PH_Workflow_TaskContext set
    WC_Worker_UserNumber = :WC_Worker_UserNumber,
    WC_Time = sysdate
  where WC_WI_ID = :WC_WI_ID"))
      {
        DbCommandHelper.CreateParameter(command, "WC_Worker_UserNumber", taskContext.WorkerUserNumber);
        DbCommandHelper.CreateParameter(command, "WC_WI_ID", id);
        succeed = DbCommandHelper.ExecuteNonQuery(command, false) != 0;
      }
      if (!succeed)
      {
        using (DbCommand command = DbCommandHelper.CreateCommand(transaction,
@"insert into PH_Workflow_TaskContext
  (WC_WI_ID, WC_Worker_UserNumber, WC_Time)
  values
  (:WC_WI_ID, :WC_Worker_UserNumber, sysdate)"))
        {
          DbCommandHelper.CreateParameter(command, "WC_WI_ID", id);
          DbCommandHelper.CreateParameter(command, "WC_Worker_UserNumber", taskContext.WorkerUserNumber);
          DbCommandHelper.ExecuteNonQuery(command, false);
        }
      }
      using (DbCommand command = DbCommandHelper.CreateCommand(transaction,
@"update PH_Workflow_TaskContext set
    WC_Content = :WC_Content
  where WC_WI_ID = :WC_WI_ID"))
      {
        DbCommandHelper.CreateParameter(command, "WC_Content", Phenix.Core.Reflection.Utilities.BinarySerialize(taskContext));
        DbCommandHelper.CreateParameter(command, "WC_WI_ID", id);
        DbCommandHelper.ExecuteNonQuery(command, false);
      }
    }

    public string FetchWorkflowInstance(Guid id)
    {
      return DefaultDatabase.ExecuteGet(ExecutFetchWorkflowInstance, id); //for Oracle LONG column
    }

    private static string ExecutFetchWorkflowInstance(DbConnection connection, Guid id)
    {
      using (DataReader reader = new DataReader(connection,
@"select WI_Content
  from PH_Workflow_Instance
  where WI_ID = :WI_ID", 
        CommandBehavior.SingleRow, false))
      {
        reader.CreateParameter("WI_ID", id);
        if (reader.Read())
          return reader.GetNullableString(0);
      }
      return null;
    }

    public void ClearWorkflowInstance(Guid id)
    {
      DefaultDatabase.Execute(ExecuteClearWorkflowInstance, id);
    }

    private static void ExecuteClearWorkflowInstance(DbTransaction transaction, Guid id)
    {
      using (DbCommand command = DbCommandHelper.CreateCommand(transaction,
@"delete from PH_Workflow_TaskContext
  where WC_WI_ID = :WC_WI_ID"))
      {
        DbCommandHelper.CreateParameter(command, "WC_WI_ID", id);
        DbCommandHelper.ExecuteNonQuery(command, false);
      }
      using (DbCommand command = DbCommandHelper.CreateCommand(transaction,
@"delete from PH_Workflow_Task
  where WT_WI_ID = :WT_WI_ID"))
      {
        DbCommandHelper.CreateParameter(command, "WT_WI_ID", id);
        DbCommandHelper.ExecuteNonQuery(command, false);
      }
      using (DbCommand command = DbCommandHelper.CreateCommand(transaction,
@"delete from PH_Workflow_Instance
  where WI_ID = :WI_ID"))
      {
        DbCommandHelper.CreateParameter(command, "WI_ID", id);
        DbCommandHelper.ExecuteNonQuery(command, false);
      }
    }

    #endregion
    
    #region WorkflowTask

    public void DispatchWorkflowTask(Guid id, string bookmarkName,
      string pluginAssemblyName, string workerRole, string caption, string message, bool urgent)
    {
      DefaultDatabase.Execute(ExecuteDispatchWorkflowTask, id, bookmarkName, pluginAssemblyName, workerRole, caption, message, urgent);
    }

    private static void ExecuteDispatchWorkflowTask(DbTransaction transaction, Guid id, string bookmarkName,
      string pluginAssemblyName, string workerRole, string caption, string message, bool urgent)
    {
      using (DbCommand command = DbCommandHelper.CreateCommand(transaction,
@"insert into PH_Workflow_Task
  (WT_WI_ID, WT_BookmarkName, WT_Plugin_AssemblyName, WT_Worker_RL_Name, WT_Caption, WT_Message, WT_Urgent, WT_State, WT_Dispatch_Time)
  values
  (:WT_WI_ID, :WT_BookmarkName, :WT_Plugin_AssemblyName, :WT_Worker_RL_Name, :WT_Caption, :WT_Message, :WT_Urgent, :WT_State, sysdate)"))
      {
        DbCommandHelper.CreateParameter(command, "WT_WI_ID", id);
        DbCommandHelper.CreateParameter(command, "WT_BookmarkName", bookmarkName);
        DbCommandHelper.CreateParameter(command, "WT_Plugin_AssemblyName", pluginAssemblyName);
        DbCommandHelper.CreateParameter(command, "WT_Worker_RL_Name", workerRole);
        DbCommandHelper.CreateParameter(command, "WT_Caption", caption);
        DbCommandHelper.CreateParameter(command, "WT_Message", message);
        DbCommandHelper.CreateParameter(command, "WT_Urgent", urgent);
        DbCommandHelper.CreateParameter(command, "WT_State", TaskState.Dispatch);
        DbCommandHelper.ExecuteNonQuery(command, false);
      }
    }

    public void ReceiveWorkflowTask(Guid id, string bookmarkName)
    {
      DefaultDatabase.Execute(ExecuteReceiveWorkflowTask, id, bookmarkName);
    }

    private static void ExecuteReceiveWorkflowTask(DbTransaction transaction, Guid id, string bookmarkName)
    {
      using (DbCommand command = DbCommandHelper.CreateCommand(transaction,
@"update PH_Workflow_Task set
    WT_State = :WT_State,
    WT_Receive_Time = sysdate
  where WT_WI_ID = :WT_WI_ID and WT_BookmarkName = :WT_BookmarkName"))
      {
        DbCommandHelper.CreateParameter(command, "WT_State", TaskState.Received);
        DbCommandHelper.CreateParameter(command, "WT_WI_ID", id);
        DbCommandHelper.CreateParameter(command, "WT_BookmarkName", bookmarkName);
        DbCommandHelper.ExecuteNonQuery(command, false);
      }
    }

    public void HoldWorkflowTask(Guid id, string bookmarkName)
    {
      DefaultDatabase.Execute(ExecuteHoldWorkflowTask, id, bookmarkName);
    }

    private static void ExecuteHoldWorkflowTask(DbTransaction transaction, Guid id, string bookmarkName)
    {
      using (DbCommand command = DbCommandHelper.CreateCommand(transaction,
@"update PH_Workflow_Task set
    WT_State = :WT_State,
    WT_Hold_Time = sysdate
  where WT_WI_ID = :WT_WI_ID and WT_BookmarkName = :WT_BookmarkName"))
      {
        DbCommandHelper.CreateParameter(command, "WT_State", TaskState.Holded);
        DbCommandHelper.CreateParameter(command, "WT_WI_ID", id);
        DbCommandHelper.CreateParameter(command, "WT_BookmarkName", bookmarkName);
        DbCommandHelper.ExecuteNonQuery(command, false);
      }
    }

    public void AbortWorkflowTask(Guid id, string bookmarkName)
    {
      DefaultDatabase.Execute(ExecuteAbortWorkflowTask, id, bookmarkName);
    }

    private static void ExecuteAbortWorkflowTask(DbTransaction transaction, Guid id, string bookmarkName)
    {
      using (DbCommand command = DbCommandHelper.CreateCommand(transaction,
@"update PH_Workflow_Task set
    WT_State = :WT_State,
    WT_Abortive_Time = sysdate
  where WT_WI_ID = :WT_WI_ID and WT_BookmarkName = :WT_BookmarkName"))
      {
        DbCommandHelper.CreateParameter(command, "WT_State", TaskState.Aborted);
        DbCommandHelper.CreateParameter(command, "WT_WI_ID", id);
        DbCommandHelper.CreateParameter(command, "WT_BookmarkName", bookmarkName);
        DbCommandHelper.ExecuteNonQuery(command, false);
      }
    }
    
    public void ProceedWorkflow(WorkflowTaskInfo workflowTaskInfo)
    {
      InstanceHost.Default.ResumeBookmark(workflowTaskInfo);
    }

    public void CompleteWorkflowTask(Guid id, string bookmarkName)
    {
      DefaultDatabase.Execute(ExecuteCompleteWorkflowTask, id, bookmarkName);
    }

    private static void ExecuteCompleteWorkflowTask(DbTransaction transaction, Guid id, string bookmarkName)
    {
      using (DbCommand command = DbCommandHelper.CreateCommand(transaction,
@"update PH_Workflow_Task set
    WT_State = :WT_State,
    WT_Complete_Time = sysdate
  where WT_WI_ID = :WT_WI_ID and WT_BookmarkName = :WT_BookmarkName"))
      {
        DbCommandHelper.CreateParameter(command, "WT_State", TaskState.Completed);
        DbCommandHelper.CreateParameter(command, "WT_WI_ID", id);
        DbCommandHelper.CreateParameter(command, "WT_BookmarkName", bookmarkName);
        DbCommandHelper.ExecuteNonQuery(command, false);
      }
    }

    public IList<WorkflowTaskInfo> FetchWorkflowTask(TaskState taskState, DateTime startDispatchTime, DateTime finishDispatchTime, UserIdentity identity)
    {
      return DefaultDatabase.ExecuteGet(ExecuteFetchWorkflowTask, taskState, startDispatchTime, finishDispatchTime, identity.UserNumber); //for Oracle LONG column
    }

    private static IList<WorkflowTaskInfo> ExecuteFetchWorkflowTask(DbConnection connection, TaskState taskState, DateTime startDispatchTime, DateTime finishDispatchTime, string userNumber)
    {
      List<WorkflowTaskInfo> result = new List<WorkflowTaskInfo>();
      string whereSql = String.Empty;
      foreach (EnumKeyCaption item in EnumKeyCaptionCollection.Fetch<TaskState>())
      {
        TaskState taskStateItem = (TaskState)item.Value;
        if ((taskState & taskStateItem) == taskStateItem)
          whereSql = String.Format("{0}WT_State = {1} or ", whereSql, (int)taskStateItem);
      }
      using (DataReader reader = new DataReader(connection,
        String.Format(
@"select WI_ID, WI_WF_Namespace, WI_WF_TypeName, WC_Content,
  WT_BookmarkName, WT_Plugin_AssemblyName, WT_Worker_RL_Name,
  WT_Caption, WT_Message, WT_Urgent, WT_State,
  WT_Dispatch_Time, WT_Receive_Time, WT_Hold_Time, WT_Abortive_Time, WT_Complete_Time
  from PH_Workflow_Task, PH_Workflow_TaskContext, PH_Workflow_Instance
  where WI_ID = WT_WI_ID and WI_ID = WC_WI_ID and WT_Dispatch_Time >= :Start_WT_Dispatch_Time and WT_Dispatch_Time <= :Finish_WT_Dispatch_Time
    and ({0})
    and (WC_Worker_UserNumber = :WC_Worker_UserNumber or WT_Worker_RL_Name in (select RL_Name from PH_Role, PH_User_Role, PH_User where RL_ID = UR_RL_ID and UR_US_ID = US_ID and US_UserNumber = :US_UserNumber))
  order by WI_WF_Namespace, WI_WF_TypeName",
          whereSql.Remove(whereSql.TrimEnd().LastIndexOf(' '))),
        CommandBehavior.SingleResult, false))
      {
        reader.CreateParameter("Start_WT_Dispatch_Time", startDispatchTime);
        reader.CreateParameter("Finish_WT_Dispatch_Time", finishDispatchTime);
        reader.CreateParameter("WC_Worker_UserNumber", userNumber);
        reader.CreateParameter("US_UserNumber", userNumber); 
        while (reader.Read())
        {
          result.Add(new WorkflowTaskInfo(Guid.Parse(reader.GetNullableString(0)), reader.GetNullableString(1), reader.GetNullableString(2), (TaskContext)Phenix.Core.Reflection.Utilities.BinaryDeserialize(reader.GetNullableString(3)), 
            reader.GetNullableString(4), reader.GetNullableString(5), reader.GetNullableString(6),
            reader.GetNullableString(7), reader.GetNullableString(8), reader.GetBooleanForDecimal(9), (TaskState)reader.GetDecimal(10),
            reader.GetDateTime(11), reader.GetNullableDateTime(12), reader.GetNullableDateTime(13), reader.GetNullableDateTime(14), reader.GetNullableDateTime(15)));
        }
      }
      return result.AsReadOnly();
    }

    #endregion
 
    #endregion
  }
}