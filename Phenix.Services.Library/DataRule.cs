using System;
using System.Data;
using System.Data.Common;
using System.Reflection;
using Phenix.Core.Data;
using Phenix.Core.Log;
using Phenix.Core.Mapping;
using Phenix.Core.Rule;
using Phenix.Core.Security;

namespace Phenix.Services.Library
{
  internal class DataRule : IDataRule
  {
    #region 方法

    public PromptCodeKeyCaptionCollection GetPromptCodes(string name, UserIdentity identity)
    {
      return DefaultDatabase.ExecuteGet(ExecuteGetPromptCodes, name, identity);
    }

    private static PromptCodeKeyCaptionCollection ExecuteGetPromptCodes(DbConnection connection, string name, UserIdentity identity)
    {
      PromptCodeKeyCaptionCollection result = new PromptCodeKeyCaptionCollection(name, ExecuteGetPromptCodeChangedTime(connection, name));
      using (DataReader reader = new DataReader(connection,
@"select PC_Key, PC_Caption, PC_Value,
  PC_ReadLevel, PC_Addtime, PC_UserNumber,
  PC_DP_ID, PC_PT_ID
  from PH_PromptCode
  where PC_Name = :PC_Name
  order by PC_Key",
        CommandBehavior.SingleResult, false))
      {
        reader.CreateParameter("PC_Name", name);
        while (reader.Read())
        {
          PromptCodeKeyCaption promptCode = new PromptCodeKeyCaption(
            reader.GetNullableString(0), reader.GetNullableString(1), reader.GetNullableString(2), 
            (ReadLevel)reader.GetDecimal(3), reader.GetDateTime(4), reader.GetNullableString(5));
          if (promptCode.ReadLevel == ReadLevel.Public ||
            (promptCode.ReadLevel == ReadLevel.Private && String.CompareOrdinal(promptCode.UserNumber, identity.UserNumber) == 0))
            result.Add(promptCode);
          else
          {
            long? departmentId = reader.GetNullableInt64ForDecimal(6);
            long? positionId = reader.GetNullableInt64ForDecimal(7);
            if ((promptCode.ReadLevel == ReadLevel.Department && identity.DepartmentId == departmentId) ||
              (promptCode.ReadLevel == ReadLevel.Position && identity.PositionId == positionId) ||
              (promptCode.ReadLevel == ReadLevel.DepartmentPosition && identity.DepartmentId == departmentId && identity.PositionId == positionId))
              result.Add(promptCode);
          }
        }
      }
      return result;
    }

    public DateTime? GetPromptCodeChangedTime(string name)
    {
      return DefaultDatabase.ExecuteGet(ExecuteGetPromptCodeChangedTime, name);
    }

    private static DateTime? ExecuteGetPromptCodeChangedTime(DbConnection connection, string name)
    {
      using (DataReader reader = new DataReader(connection,
@"select PC_ActionTime
  from PH_PromptCode_Action
  where PC_Name = :PC_Name",
        CommandBehavior.SingleRow, false))
      {
        reader.CreateParameter("PC_Name", name);
        if (reader.Read())
          return reader.GetDateTime(0);
      }
      return null;
    }

    public void PromptCodeHasChanged(string name)
    {
      DefaultDatabase.Execute(ExecutePromptCodeHasChanged, name);
    }

    private static void ExecutePromptCodeHasChanged(DbTransaction transaction, string name)
    {
      bool succeed;
      using (DbCommand command = DbCommandHelper.CreateCommand(transaction,
@"update PH_PromptCode_Action set
    PC_ActionTime = sysdate
  where PC_Name = :PC_Name"))
      {
        DbCommandHelper.CreateParameter(command, "PC_Name", name);
        succeed = DbCommandHelper.ExecuteNonQuery(command, false) != 0;
      }
      if (!succeed)
      {
        using (DbCommand command = DbCommandHelper.CreateCommand(transaction,
@"insert into PH_PromptCode_Action
  (PC_Name, PC_ActionTime)
  values
  (:PC_Name, sysdate)"))
        {
          DbCommandHelper.CreateParameter(command, "PC_Name", name);
          DbCommandHelper.ExecuteNonQuery(command, false);
        }
      }
    }

    public bool AddPromptCode(string name, string key, string caption, string value, ReadLevel readLevel, UserIdentity identity)
    {
      return DefaultDatabase.ExecuteGet(ExecuteAddPromptCode, name, key, caption, value, readLevel, identity);
    }

    private static bool ExecuteAddPromptCode(DbTransaction transaction, string name, string key, string caption, string value, ReadLevel readLevel, UserIdentity identity)
    {
      bool result = false;
      bool succeed;
      using (DbCommand command = DbCommandHelper.CreateCommand(transaction,
@"update PH_PromptCode set
    PC_Caption = :PC_Caption,
    PC_Value = :PC_Value,
    PC_ReadLevel = :PC_ReadLevel,
    PC_Addtime = sysdate,
    PC_UserNumber = :PC_UserNumber,
    PC_DP_ID = :PC_DP_ID,
    PC_PT_ID = :PC_PT_ID
  where PC_Name = :PC_Name and PC_Key = :PC_Key"))
      {
        DbCommandHelper.CreateParameter(command, "PC_Caption", caption);
        DbCommandHelper.CreateParameter(command, "PC_Value", value);
        DbCommandHelper.CreateParameter(command, "PC_ReadLevel", readLevel);
        DbCommandHelper.CreateParameter(command, "PC_UserNumber", identity.UserNumber);
        DbCommandHelper.CreateParameter(command, "PC_DP_ID", identity.DepartmentId);
        DbCommandHelper.CreateParameter(command, "PC_PT_ID", identity.PositionId);
        DbCommandHelper.CreateParameter(command, "PC_Name", name);
        DbCommandHelper.CreateParameter(command, "PC_Key", key);
        succeed = DbCommandHelper.ExecuteNonQuery(command, false) != 0;
      }
      if (!succeed)
      {
        using (DbCommand command = DbCommandHelper.CreateCommand(transaction,
@"insert into PH_PromptCode
  (PC_Name, PC_Key, PC_Caption, PC_Value, PC_ReadLevel, PC_Addtime, PC_UserNumber, PC_DP_ID, PC_PT_ID)
  values
  (:PC_Name, :PC_Key, :PC_Caption, :PC_Value, :PC_ReadLevel, sysdate, :PC_UserNumber, :PC_DP_ID, :PC_PT_ID)"))
        {
          DbCommandHelper.CreateParameter(command, "PC_Name", name);
          DbCommandHelper.CreateParameter(command, "PC_Key", key);
          DbCommandHelper.CreateParameter(command, "PC_Caption", caption);
          DbCommandHelper.CreateParameter(command, "PC_Value", value);
          DbCommandHelper.CreateParameter(command, "PC_ReadLevel", readLevel);
          DbCommandHelper.CreateParameter(command, "PC_UserNumber", identity.UserNumber);
          DbCommandHelper.CreateParameter(command, "PC_DP_ID", identity.DepartmentId);
          DbCommandHelper.CreateParameter(command, "PC_PT_ID", identity.PositionId);
          DbCommandHelper.ExecuteNonQuery(command, false);
          result = true;
        }
      }
      ExecutePromptCodeHasChanged(transaction, name);
      return result;
    }

    public void RemovePromptCode(string name, string key)
    {
      DefaultDatabase.Execute(ExecuteRemovePromptCode, name, key);
    }

    private static void ExecuteRemovePromptCode(DbTransaction transaction, string name, string key)
    {
      using (DbCommand command = DbCommandHelper.CreateCommand(transaction,
@"delete PH_PromptCode
  where PC_Name = :PC_Name and PC_Key = :PC_Key"))
      {
        DbCommandHelper.CreateParameter(command, "PC_Name", name);
        DbCommandHelper.CreateParameter(command, "PC_Key", key);
        DbCommandHelper.ExecuteNonQuery(command, false);
      }
      ExecutePromptCodeHasChanged(transaction, name);
    }

    public CriteriaExpressionKeyCaptionCollection GetCriteriaExpressions(string name, UserIdentity identity)
    {
      return DefaultDatabase.ExecuteGet(ExecuteGetCriteriaExpressions, name, identity); //for Oracle LONG column
    }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
    private static CriteriaExpressionKeyCaptionCollection ExecuteGetCriteriaExpressions(DbConnection connection, string name, UserIdentity identity)
    {
      CriteriaExpressionKeyCaptionCollection result = new CriteriaExpressionKeyCaptionCollection(name, ExecuteGetCriteriaExpressionChangedTime(connection, name));
      using (DataReader reader = new DataReader(connection,
@"select CE_Key, CE_Caption, CE_Tree,
  CE_ReadLevel, CE_Addtime, CE_UserNumber,
  CE_DP_ID, CE_PT_ID
  from PH_CriteriaExpression
  where CE_Name = :CE_Name
  order by CE_Key", 
        CommandBehavior.SingleResult, false))
      {
        reader.CreateParameter("CE_Name", name);
        while (reader.Read())
          try
          {
            CriteriaExpression criteriaExpression = Phenix.Core.Reflection.Utilities.JsonDeserialize(reader.GetNullableString(2), typeof(CriteriaExpression)) as CriteriaExpression; //Phenix.Core.Reflection.Utilities.BinaryDeserialize<CriteriaExpression>(reader.GetNullableString(2));
            if (criteriaExpression != null)
            {
              CriteriaExpressionKeyCaption criteriaExpressionKeyCaption = new CriteriaExpressionKeyCaption(
                reader.GetNullableString(0), reader.GetNullableString(1), criteriaExpression, (ReadLevel)reader.GetDecimal(3), reader.GetDateTime(4), reader.GetNullableString(5));
              if (criteriaExpressionKeyCaption.ReadLevel == ReadLevel.Public ||
                (criteriaExpressionKeyCaption.ReadLevel == ReadLevel.Private && String.CompareOrdinal(criteriaExpressionKeyCaption.UserNumber, identity.UserNumber) == 0))
                result.Add(criteriaExpressionKeyCaption);
              else
              {
                long? departmentId = reader.GetNullableInt64ForDecimal(6);
                long? positionId = reader.GetNullableInt64ForDecimal(7);
                if ((criteriaExpressionKeyCaption.ReadLevel == ReadLevel.Department && identity.DepartmentId == departmentId) ||
                  (criteriaExpressionKeyCaption.ReadLevel == ReadLevel.Position && identity.PositionId == positionId) ||
                  (criteriaExpressionKeyCaption.ReadLevel == ReadLevel.DepartmentPosition && identity.DepartmentId == departmentId && identity.PositionId == positionId))
                  result.Add(criteriaExpressionKeyCaption);
              }
            }
          }
          catch (Exception ex)
          {
            EventLog.SaveLocal(MethodBase.GetCurrentMethod(), reader.GetNullableString(0), ex);
          }
      }
      return result;
    }

    public DateTime? GetCriteriaExpressionChangedTime(string name)
    {
      return DefaultDatabase.ExecuteGet(ExecuteGetCriteriaExpressionChangedTime, name);
    }

    private static DateTime? ExecuteGetCriteriaExpressionChangedTime(DbConnection connection, string name)
    {
      using (DataReader reader = new DataReader(connection,
@"select CE_ActionTime
  from PH_CriteriaExpression_Action
  where CE_Name = :CE_Name",
        CommandBehavior.SingleRow, false))
      {
        reader.CreateParameter("CE_Name", name);
        if (reader.Read())
          return reader.GetDateTime(0);
      }
      return null;
    }

    public void CriteriaExpressionHasChanged(string name)
    {
      DefaultDatabase.Execute(ExecuteCriteriaExpressionHasChanged, name);
    }

    private static void ExecuteCriteriaExpressionHasChanged(DbTransaction transaction, string name)
    {
      bool succeed;
      using (DbCommand command = DbCommandHelper.CreateCommand(transaction,
@"update PH_CriteriaExpression_Action set
    CE_ActionTime = sysdate
  where CE_Name = :CE_Name"))
      {
        DbCommandHelper.CreateParameter(command, "CE_Name", name);
        succeed = DbCommandHelper.ExecuteNonQuery(command, false) != 0;
      }
      if (!succeed)
      {
        using (DbCommand command = DbCommandHelper.CreateCommand(transaction,
@"insert into PH_CriteriaExpression_Action
  (CE_Name, CE_ActionTime)
  values
  (:CE_Name, sysdate)"))
        {
          DbCommandHelper.CreateParameter(command, "CE_Name", name);
          DbCommandHelper.ExecuteNonQuery(command, false);
        }
      }
    }

    public bool AddCriteriaExpression(string name, string key, string caption, CriteriaExpression value, ReadLevel readLevel, UserIdentity identity)
    {
      return DefaultDatabase.ExecuteGet(ExecuteAddCriteriaExpression, name, key, caption, value, readLevel, identity); //for Oracle LONG column
    }

    private static bool ExecuteAddCriteriaExpression(DbTransaction transaction, string name, string key, string caption, CriteriaExpression value, ReadLevel readLevel, UserIdentity identity)
    {
      bool result = false;
      bool succeed;
      using (DbCommand command = DbCommandHelper.CreateCommand(transaction,
@"update PH_CriteriaExpression set
    CE_Caption = :CE_Caption,
    CE_ReadLevel = :CE_ReadLevel,
    CE_Addtime = sysdate,
    CE_UserNumber = :CE_UserNumber,
    CE_DP_ID = :CE_DP_ID,
    CE_PT_ID = :CE_PT_ID
  where CE_Name = :CE_Name and CE_Key = :CE_Key"))
      {
        DbCommandHelper.CreateParameter(command, "CE_Caption", caption);
        DbCommandHelper.CreateParameter(command, "CE_ReadLevel", readLevel);
        DbCommandHelper.CreateParameter(command, "CE_UserNumber", identity.UserNumber);
        DbCommandHelper.CreateParameter(command, "CE_DP_ID", identity.DepartmentId);
        DbCommandHelper.CreateParameter(command, "CE_PT_ID", identity.PositionId);
        DbCommandHelper.CreateParameter(command, "CE_Name", name);
        DbCommandHelper.CreateParameter(command, "CE_Key", key);
        succeed = DbCommandHelper.ExecuteNonQuery(command, false) != 0;
      }
      if (!succeed)
      {
        using (DbCommand command = DbCommandHelper.CreateCommand(transaction,
@"insert into PH_CriteriaExpression
  (CE_Name, CE_Key, CE_Caption, CE_ReadLevel, CE_Addtime, CE_UserNumber, CE_DP_ID, CE_PT_ID)
  values
  (:CE_Name, :CE_Key, :CE_Caption, :CE_ReadLevel, sysdate, :CE_UserNumber, :CE_DP_ID, :CE_PT_ID)"))
        {
          DbCommandHelper.CreateParameter(command, "CE_Name", name);
          DbCommandHelper.CreateParameter(command, "CE_Key", key);
          DbCommandHelper.CreateParameter(command, "CE_Caption", caption);
          DbCommandHelper.CreateParameter(command, "CE_ReadLevel", readLevel);
          DbCommandHelper.CreateParameter(command, "CE_UserNumber", identity.UserNumber);
          DbCommandHelper.CreateParameter(command, "CE_DP_ID", identity.DepartmentId);
          DbCommandHelper.CreateParameter(command, "CE_PT_ID", identity.PositionId);
          DbCommandHelper.ExecuteNonQuery(command, false);
          result = true;
        }
      }
      using (DbCommand command = DbCommandHelper.CreateCommand(transaction,
@"update PH_CriteriaExpression set
    CE_Tree = :CE_Tree
  where CE_Name = :CE_Name and CE_Key = :CE_Key"))
      {
        DbCommandHelper.CreateParameter(command, "CE_Tree", Phenix.Core.Reflection.Utilities.JsonSerialize(value)); //Phenix.Core.Reflection.Utilities.BinarySerialize(value));
        DbCommandHelper.CreateParameter(command, "CE_Name", name);
        DbCommandHelper.CreateParameter(command, "CE_Key", key);
        DbCommandHelper.ExecuteNonQuery(command, false);
      }
      ExecuteCriteriaExpressionHasChanged(transaction, name);
      return result;
    }

    public void RemoveCriteriaExpression(string name, string key)
    {
      DefaultDatabase.Execute(ExecuteRemoveCriteriaExpression, name, key);
    }

    private static void ExecuteRemoveCriteriaExpression(DbTransaction transaction, string name, string key)
    {
      using (DbCommand command = DbCommandHelper.CreateCommand(transaction,
@"delete PH_CriteriaExpression
  where CE_Name = :CE_Name and CE_Key = :CE_Key"))
      {
        DbCommandHelper.CreateParameter(command, "CE_Name", name);
        DbCommandHelper.CreateParameter(command, "CE_Key", key);
        DbCommandHelper.ExecuteNonQuery(command, false);
      }
      ExecuteCriteriaExpressionHasChanged(transaction, name);
    }

    #endregion
  }
}