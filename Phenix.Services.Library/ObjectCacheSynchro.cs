using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.Common;
using System.Reflection;
using System.Threading;
using Phenix.Core.Cache;
using Phenix.Core.Data;
using Phenix.Core.Log;
using Phenix.Core.SyncCollections;

namespace Phenix.Services.Library
{
  internal class ObjectCacheSynchro : IObjectCacheSynchro
  {
    #region 方法

    #region GetActionTime

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
    public DateTime? GetActionTime(string typeName)
    {
      try
      {
        return DefaultDatabase.ExecuteGet(ExecuteGetActionTime, typeName);
      }
      catch (Exception ex)
      {
        EventLog.SaveLocal(MethodBase.GetCurrentMethod(), typeName, ex);
      }
      return null;
    }

    private static DateTime? ExecuteGetActionTime(DbConnection connection, string typeName)
    {
      using (DataReader reader = new DataReader(connection,
@"select CA_ActionTime
  from PH_CacheAction
  where CA_ClassName = :CA_ClassName", 
        CommandBehavior.SingleRow, false))
      {
        reader.CreateParameter("CA_ClassName", typeName);
        if (reader.Read())
          return reader.GetDateTime(0);
      }
      using (DbCommand command = DbCommandHelper.CreateCommand(connection,
@"insert into PH_CacheAction
  (CA_ClassName, CA_ActionTime)
  values
  (:CA_ClassName, :CA_ActionTime)"))
      {
        DateTime result = DateTime.Now;
        DbCommandHelper.CreateParameter(command, "CA_ClassName", typeName);
        DbCommandHelper.CreateParameter(command, "CA_ActionTime", result);
        DbCommandHelper.ExecuteNonQuery(command, false);
        return result;
      }
    }

    #endregion

    #region ChangeActionTime

    #region Clear

    public void ClearAll()
    {
      DefaultDatabase.Execute(ExecuteClearAll);
    }

    private static void ExecuteClearAll(DbConnection connection)
    {
      using (DbCommand command = DbCommandHelper.CreateCommand(connection,
@"update PH_CacheAction set
    CA_ActionTime = sysdate"))
      {
        DbCommandHelper.ExecuteNonQuery(command, false);
      }
    }

    private static Thread _clearThread;
    private static readonly SynchronizedQueue<string> _clearingTypeNames = new SynchronizedQueue<string>(256);

    public void Clear(IList<string> typeNames)
    {
      if (_clearThread == null)
        lock (_clearingTypeNames)
          if (_clearThread == null)
          {
            _clearThread = new Thread(ExecuteClear);
            _clearThread.IsBackground = true;
            _clearThread.Start();
          }
      foreach (string s in typeNames)
        _clearingTypeNames.Enqueue(s);
    }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
    private static void ExecuteClear()
    {
      try
      {
        List<string> typeNames = new List<string>(256);
        while (true)
          try
          {
            if (_clearingTypeNames.Count > 0)
            {
              DateTime dt = DateTime.Now;
              while (DateTime.Now.Subtract(dt).TotalSeconds <= 2)
              {
                while (_clearingTypeNames.Count > 0)
                {
                  string typeName = _clearingTypeNames.Dequeue();
                  if (!typeNames.Contains(typeName))
                    typeNames.Add(typeName);
                }
                Thread.Sleep(100);
              }
              DefaultDatabase.Execute(ExecuteClear, typeNames);
              typeNames.Clear();
            }
            Thread.Sleep(100);
          }
          catch (ObjectDisposedException)
          {
            return;
          }
          catch (ThreadAbortException)
          {
            Thread.ResetAbort();
            return;
          }
          catch (Exception)
          {
            Thread.Sleep(1000);
          }
      }
      finally
      {
        _clearThread = null;
      }
    }

    private static void ExecuteClear(DbConnection connection, IList<string> typeNames)
    {
      List<string> classNames = new List<string>();
      foreach (string typeName in typeNames)
        foreach (string className in GetTypeRelations(connection, typeName))
          if (!classNames.Contains(className))
            classNames.Add(className);
      ChangeActionTime(connection, classNames);
    }

    private static readonly SynchronizedDictionary<string, ReadOnlyCollection<string>> _typeRelationsCache =
      new SynchronizedDictionary<string, ReadOnlyCollection<string>>(StringComparer.Ordinal);

    private static IList<string> GetTypeRelations(DbConnection connection, string typeName)
    {
      return _typeRelationsCache.GetValue(typeName, () =>
      {
        List<string> value = new List<string>();
        using (DataReader reader = new DataReader(connection,
@"select distinct(D.AC_Name)
  from PH_AssemblyClass A, PH_AssemblyClass_Group B, PH_AssemblyClass_Group C, PH_AssemblyClass D
  where :AC_Name = A.AC_Name and A.AC_ID = B.AG_AC_ID
    and B.AG_Name = C.AG_Name and C.AG_AC_ID = D.AC_ID",
          CommandBehavior.SingleResult, false))
        {
          reader.CreateParameter("AC_Name", typeName);
          while (reader.Read())
            value.Add(reader.GetNullableString(0));
        }
        return value.AsReadOnly();
      }, false);
    }

    #endregion

    #region RecordHasChanged

    private static Thread _recordHasChangedThread;
    private static readonly SynchronizedQueue<string> _changedTableNames = new SynchronizedQueue<string>(256);

    public void RecordHasChanged(string tableName)
    {
      if (_recordHasChangedThread == null)
        lock (_changedTableNames)
          if (_recordHasChangedThread == null)
          {
            _recordHasChangedThread = new Thread(ExecuteRecordHasChanged);
            _recordHasChangedThread.IsBackground = true;
            _recordHasChangedThread.Start();
          }
      _changedTableNames.Enqueue(tableName);
    }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
    private static void ExecuteRecordHasChanged()
    {
      try
      {
        List<string> tableNames = new List<string>(256);
        while (true)
          try
          {
            if (_changedTableNames.Count > 0)
            {
              DateTime dt = DateTime.Now;
              while (DateTime.Now.Subtract(dt).TotalSeconds <= 2)
              {
                while (_changedTableNames.Count > 0)
                {
                  string tableName = _changedTableNames.Dequeue();
                  if (!tableNames.Contains(tableName))
                    tableNames.Add(tableName);
                }
                Thread.Sleep(100);
              }
              DefaultDatabase.Execute(ExecuteRecordHasChanged, tableNames);
              tableNames.Clear();
            }
            Thread.Sleep(100);
          }
          catch (ObjectDisposedException)
          {
            return;
          }
          catch (ThreadAbortException)
          {
            Thread.ResetAbort();
            return;
          }
          catch (Exception)
          {
            Thread.Sleep(1000);
          }
      }
      finally
      {
        _recordHasChangedThread = null;
      }
    }

    private static void ExecuteRecordHasChanged(DbConnection connection, IList<string> tableNames)
    {
      List<string> classNames = new List<string>();
      foreach (string tableName in tableNames)
        foreach (string className in GetTableRelations(connection, tableName))
          if (!classNames.Contains(className))
            classNames.Add(className);
      ChangeActionTime(connection, classNames);
    }

    private static readonly SynchronizedDictionary<string, ReadOnlyCollection<string>> _tableRelationsCache =
      new SynchronizedDictionary<string, ReadOnlyCollection<string>>(StringComparer.Ordinal);

    private static IList<string> GetTableRelations(DbConnection connection, string tableName)
    {
      return _tableRelationsCache.GetValue(tableName, () =>
      {
        List<string> value = new List<string>();
        using (DataReader reader = new DataReader(connection,
@"select distinct(A.AC_Name)
  from PH_AssemblyClass A, PH_AssemblyClass_Group B
  where A.AC_ID = B.AG_AC_ID and upper(B.AG_Name) = upper(:AG_Name)",
          CommandBehavior.SingleResult, false))
        {
          reader.CreateParameter("AG_Name", tableName);
          while (reader.Read())
            value.Add(reader.GetNullableString(0));
        }
        return value.AsReadOnly();
      }, false);
    }

    #endregion

    private static void ChangeActionTime(DbConnection connection, IList<string> classNames)
    {
      List<string> brandClassNames = new List<string>();
      if (classNames.Count > 0)
        using (DbCommand command = DbCommandHelper.CreateCommand(connection,
@"update PH_CacheAction set
    CA_ActionTime = sysdate
  where CA_ClassName = :CA_ClassName"))
        {
          foreach (string s in classNames)
          {
            DbCommandHelper.CreateParameter(command, "CA_ClassName", s);
            if (DbCommandHelper.ExecuteNonQuery(command, false) == 0)
              brandClassNames.Add(s);
          }
        }
      if (brandClassNames.Count > 0)
        using (DbCommand command = DbCommandHelper.CreateCommand(connection,
@"insert into PH_CacheAction
  (CA_ClassName, CA_ActionTime)
  values
  (:CA_ClassName, sysdate)"))
        {
          foreach (string s in brandClassNames)
          {
            DbCommandHelper.CreateParameter(command, "CA_ClassName", s);
            DbCommandHelper.ExecuteNonQuery(command, false);
          }
        }
    }

    #endregion

    #endregion
  }
}