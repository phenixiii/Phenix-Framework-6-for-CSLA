using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using Phenix.Core.Data;
using Phenix.Core.Log;
using Phenix.Core.Mapping;

namespace Phenix.Services.Library
{
  internal class PermanentLog : IPermanentLog
  {
    #region 方法

    public void Save(string userNumber, string typeName, string message, Exception error)
    {
      DefaultDatabase.Execute(ExecuteSave, userNumber, typeName, message, error);
    }

    private static void ExecuteSave(DbTransaction transaction, string userNumber, string typeName, string message, Exception error)
    {
      long id = Sequence.Value;
      using (DbCommand command = DbCommandHelper.CreateCommand(transaction,
@"insert into PH_ExecuteLog
  (EL_ID, EL_Time, EL_UserNumber, EL_BusinessName, EL_Message, EL_ExceptionName, EL_ExceptionMessage)
  values
  (:EL_ID, sysdate, :EL_UserNumber, :EL_BusinessName, :EL_Message, :EL_ExceptionName, :EL_ExceptionMessage)"))
      {
        DbCommandHelper.CreateParameter(command, "EL_ID", id);
        DbCommandHelper.CreateParameter(command, "EL_UserNumber", userNumber);
        DbCommandHelper.CreateParameter(command, "EL_BusinessName", typeName);
        DbCommandHelper.CreateParameter(command, "EL_Message", Phenix.Core.Reflection.Utilities.SubString(message, 4000, false));
        DbCommandHelper.CreateParameter(command, "EL_ExceptionName", error != null ? error.GetType().ToString() : null);
        DbCommandHelper.CreateParameter(command, "EL_ExceptionMessage", Phenix.Core.Reflection.Utilities.SubString(Phenix.Core.AppUtilities.GetErrorMessage(error), 4000, false));
        DbCommandHelper.ExecuteNonQuery(command, false);
      }
    }

    public IList<EventLogInfo> Fetch(string userNumber, string typeName,
      DateTime startTime, DateTime finishTime)
    {
      return DefaultDatabase.ExecuteGet(ExecuteFetch, userNumber, typeName, startTime, finishTime);
    }

    private static IList<EventLogInfo> ExecuteFetch(DbConnection connection, string userNumber, string typeName,
      DateTime startTime, DateTime finishTime)
    {
      List<EventLogInfo> result = new List<EventLogInfo>();
      using (DataReader reader = new DataReader(connection,
@"select EL_Time, EL_UserNumber, EL_Message, EL_ExceptionName, EL_ExceptionMessage
  from PH_ExecuteLog
  where (EL_UserNumber = :EL_UserNumber1 or :EL_UserNumber2 is null)
    and (EL_BusinessName = :EL_BusinessName1 or :EL_BusinessName2 is null)
    and EL_Time >= :EL_Time1 and EL_Time <= :EL_Time2
  order by EL_Time", 
        CommandBehavior.SingleResult, false))
      {
        reader.CreateParameter("EL_UserNumber1", userNumber);
        reader.CreateParameter("EL_UserNumber2", userNumber);
        reader.CreateParameter("EL_BusinessName1", typeName);
        reader.CreateParameter("EL_BusinessName2", typeName);
        reader.CreateParameter("EL_Time1", startTime);
        reader.CreateParameter("EL_Time2", finishTime);
        while (reader.Read())
          result.Add(new EventLogInfo(reader.GetDateTime(0), reader.GetNullableString(1), reader.GetNullableString(2), 
            reader.GetNullableString(3), reader.GetNullableString(4)));
      }
      return result;
    }

    public void Clear(string userNumber, string typeName,
      DateTime startTime, DateTime finishTime)
    {
      DefaultDatabase.Execute(ExecuteClear, userNumber, typeName, startTime, finishTime);
    }

    private static void ExecuteClear(DbTransaction transaction, string userNumber, string typeName,
      DateTime startTime, DateTime finishTime)
    {
      using (DbCommand command = DbCommandHelper.CreateCommand(transaction,
@"delete from PH_ExecuteLog
  where (EL_UserNumber = :EL_UserNumber1 or :EL_UserNumber2 is null)
    and (EL_BusinessName = :EL_BusinessName1 or :EL_BusinessName2 is null)
    and EL_Time >= :EL_Time1 and EL_Time <= :EL_Time2"))
      {
        DbCommandHelper.CreateParameter(command, "EL_UserNumber1", userNumber);
        DbCommandHelper.CreateParameter(command, "EL_UserNumber2", userNumber);
        DbCommandHelper.CreateParameter(command, "EL_BusinessName1", typeName);
        DbCommandHelper.CreateParameter(command, "EL_BusinessName2", typeName);
        DbCommandHelper.CreateParameter(command, "EL_Time1", startTime);
        DbCommandHelper.CreateParameter(command, "EL_Time2", finishTime);
        DbCommandHelper.ExecuteNonQuery(command, false);
      }
    }

    public void SaveExecuteAction(string userNumber, string typeName, string primaryKey, ExecuteAction action, string log)
    {
      DefaultDatabase.Execute(ExecuteSaveExecuteAction, userNumber, typeName, primaryKey, action, log); //for Oracle LONG column
    }

    private static void ExecuteSaveExecuteAction(DbTransaction transaction, string userNumber, string typeName, string primaryKey, ExecuteAction action, string log)
    {
      long id = Sequence.Value;
      using (DbCommand command = DbCommandHelper.CreateCommand(transaction,
@"insert into PH_ExecuteActionLog
  (EA_ID, EA_Time, EA_UserNumber, EA_BusinessName, EA_BusinessPrimaryKey, EA_Action)
  values
  (:EA_ID, sysdate, :EA_UserNumber, :EA_BusinessName, :EA_BusinessPrimaryKey, :EA_Action)"))
      {
        DbCommandHelper.CreateParameter(command, "EA_ID", id);
        DbCommandHelper.CreateParameter(command, "EA_UserNumber", userNumber);
        DbCommandHelper.CreateParameter(command, "EA_BusinessName", typeName);
        DbCommandHelper.CreateParameter(command, "EA_BusinessPrimaryKey", primaryKey);
        DbCommandHelper.CreateParameter(command, "EA_Action", action);
        DbCommandHelper.ExecuteNonQuery(command, false);
      }
      using (DbCommand command = DbCommandHelper.CreateCommand(transaction,
@"update PH_ExecuteActionLog set
    EA_Log = :EA_Log
  where EA_ID = :EA_ID"))
      {
        DbCommandHelper.CreateParameter(command, "EA_Log", log);
        DbCommandHelper.CreateParameter(command, "EA_ID", id);
        DbCommandHelper.ExecuteNonQuery(command, false);
      }
    }

    public IList<string> FetchExecuteAction(string typeName, string primaryKey)
    {
      return DefaultDatabase.ExecuteGet(ExecuteFetchExecuteAction, typeName, primaryKey); //for Oracle LONG column
    }

    private static IList<string> ExecuteFetchExecuteAction(DbConnection connection, string typeName, string primaryKey)
    {
      List<string> result = new List<string>();
      using (DataReader reader = new DataReader(connection,
@"select EA_Log from PH_ExecuteActionLog
  where EA_BusinessName = :EA_BusinessName
    and EA_BusinessPrimaryKey = :EA_BusinessPrimaryKey
  order by EA_Time",
        CommandBehavior.SingleResult, false))
      {
        reader.CreateParameter("EA_BusinessName", typeName);
        reader.CreateParameter("EA_BusinessPrimaryKey", primaryKey);
        while (reader.Read())
          result.Add(reader.GetNullableString(0));
      }
      return result;
    }

    public IList<string> FetchExecuteAction(string userNumber, string typeName,
      ExecuteAction action, DateTime startTime, DateTime finishTime)
    {
      return DefaultDatabase.ExecuteGet(ExecuteFetchExecuteAction, userNumber, typeName, action, startTime, finishTime); //for Oracle LONG column
    }

    private static IList<string> ExecuteFetchExecuteAction(DbConnection connection, string userNumber, string typeName,
      ExecuteAction action, DateTime startTime, DateTime finishTime)
    {
      List<string> result = new List<string>();
      using (DataReader reader = new DataReader(connection,
@"select EA_Log from PH_ExecuteActionLog
  where (EA_UserNumber = :EA_UserNumber1 or :EA_UserNumber2 is null)
    and EA_BusinessName = :EA_BusinessName
    and (EA_Action = :EA_Action_Insert or EA_Action = :EA_Action_Update or EA_Action = :EA_Action_Delete)
    and EA_Time >= :EA_Time1 and EA_Time <= :EA_Time2
  order by EA_Time", 
        CommandBehavior.SingleResult, false))
      {
        reader.CreateParameter("EA_UserNumber1", userNumber);
        reader.CreateParameter("EA_UserNumber2", userNumber);
        reader.CreateParameter("EA_BusinessName", typeName);
        if ((action & ExecuteAction.Insert) == ExecuteAction.Insert)
          reader.CreateParameter("EA_Action_Insert", ExecuteAction.Insert);
        else
          reader.CreateParameter("EA_Action_Insert", ExecuteAction.None);
        if ((action & ExecuteAction.Update) == ExecuteAction.Update)
          reader.CreateParameter("EA_Action_Update", ExecuteAction.Update);
        else
          reader.CreateParameter("EA_Action_Update", ExecuteAction.None);
        if ((action & ExecuteAction.Delete) == ExecuteAction.Delete)
          reader.CreateParameter("EA_Action_Delete", ExecuteAction.Delete);
        else
          reader.CreateParameter("EA_Action_Delete", ExecuteAction.None);
        reader.CreateParameter("EA_Time1", startTime);
        reader.CreateParameter("EA_Time2", finishTime);
        while (reader.Read())
          result.Add(reader.GetNullableString(0));
      }
      return result;
    }

    public void ClearExecuteAction(string userNumber, string typeName,
      ExecuteAction action, DateTime startTime, DateTime finishTime)
    {
      DefaultDatabase.Execute(ExecuteClearExecuteAction, userNumber, typeName, action, startTime, finishTime);
    }

    private static void ExecuteClearExecuteAction(DbTransaction transaction, string userNumber, string typeName,
      ExecuteAction action, DateTime startTime, DateTime finishTime)
    {
      using (DbCommand command = DbCommandHelper.CreateCommand(transaction,
@"delete from PH_ExecuteActionLog
  where (EA_UserNumber = :EA_UserNumber1 or :EA_UserNumber2 is null)
    and EA_BusinessName = :EA_BusinessName
    and (EA_Action = :EA_Action_Insert or EA_Action = :EA_Action_Update or EA_Action = :EA_Action_Delete)
    and EA_Time >= :EA_Time1 and EA_Time <= :EA_Time2"))
      {
        DbCommandHelper.CreateParameter(command, "EA_UserNumber1", userNumber);
        DbCommandHelper.CreateParameter(command, "EA_UserNumber2", userNumber);
        DbCommandHelper.CreateParameter(command, "EA_BusinessName", typeName);
        if ((action & ExecuteAction.Insert) == ExecuteAction.Insert)
          DbCommandHelper.CreateParameter(command, "EA_Action_Insert", ExecuteAction.Insert);
        else
          DbCommandHelper.CreateParameter(command, "EA_Action_Insert", ExecuteAction.None);
        if ((action & ExecuteAction.Update) == ExecuteAction.Update)
          DbCommandHelper.CreateParameter(command, "EA_Action_Update", ExecuteAction.Update);
        else
          DbCommandHelper.CreateParameter(command, "EA_Action_Update", ExecuteAction.None);
        if ((action & ExecuteAction.Delete) == ExecuteAction.Delete)
          DbCommandHelper.CreateParameter(command, "EA_Action_Delete", ExecuteAction.Delete);
        else
          DbCommandHelper.CreateParameter(command, "EA_Action_Delete", ExecuteAction.None);
        DbCommandHelper.CreateParameter(command, "EA_Time1", startTime);
        DbCommandHelper.CreateParameter(command, "EA_Time2", finishTime);
        DbCommandHelper.ExecuteNonQuery(command, false);
      }
    }

    #region 代入数据库事务

    public void SaveRenovate(DbTransaction transaction, string tableName, ExecuteAction action, IList<FieldValue> fieldValues)
    {
      ExecuteSaveRenovate(transaction, tableName, action, fieldValues);
    }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2100:ReviewSqlQueriesForSecurityVulnerabilities")]
    private static void ExecuteSaveRenovate(DbTransaction transaction, string tableName, ExecuteAction action, IList<FieldValue> fieldValues)
    {
      List<string> rowIds = new List<string>();
      using (DbCommand command = DbCommandHelper.CreateCommand(transaction))
      {
        string whereSql = String.Empty;
        foreach (FieldValue item in fieldValues)
        {
          FieldMapInfo fieldMapInfo = item.GetFieldMapInfo();
          if (fieldMapInfo.FieldAttribute.IsPrimaryKey && String.Compare(fieldMapInfo.TableName, tableName, StringComparison.OrdinalIgnoreCase) == 0)
          {
            string parameterName = SqlHelper.UniqueParameterName();
            whereSql = String.Format("{0}{1}=:{2} and ", whereSql, fieldMapInfo.ShortColumnName, parameterName);
            DbCommandHelper.CreateParameter(command, parameterName, item.Value);
          }
        }
        if (String.IsNullOrEmpty(whereSql))
          throw new InvalidOperationException("类" + (fieldValues.Count > 0 ? fieldValues[0].GetFieldMapInfo().OwnerType.FullName : String.Empty) + "未能提供足够的字段数据!\n请为主键字段打上标签：[Phenix.Core.Mapping.Field(IsPrimaryKey = true)]");
        command.CommandText = String.Format(
@"select RowIDToChar(RowID)
  from {0}
  where {1}",
          tableName, whereSql.Remove(whereSql.TrimEnd().LastIndexOf(' ')));
        using (DataReader reader = new DataReader(command, CommandBehavior.SingleResult, false))
        {
          while (reader.Read())
            rowIds.Add(reader.GetNullableString(0));
        }
      }
      using (DbCommand command = DbCommandHelper.CreateCommand(transaction,
@"insert into PH_RenovateLog
  (RL_TableName, RL_ROWID, RL_Time, RL_Action)
  values
  (:RL_TableName, :RL_ROWID, sysdate, :RL_Action)"))
      {
        foreach (string s in rowIds)
        {
          DbCommandHelper.CreateParameter(command, "RL_TableName", tableName);
          DbCommandHelper.CreateParameter(command, "RL_ROWID", s);
          DbCommandHelper.CreateParameter(command, "RL_Action", action);
          DbCommandHelper.ExecuteNonQuery(command, false);
        }
      }
    }

    #endregion

    #endregion
  }
}