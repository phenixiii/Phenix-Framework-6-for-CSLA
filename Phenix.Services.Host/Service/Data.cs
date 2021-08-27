using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Globalization;
using System.IO;
using System.Reflection;
using Phenix.Core;
using Phenix.Core.Data;
using Phenix.Core.IO;
using Phenix.Core.Log;
using Phenix.Core.Mapping;
using Phenix.Core.Security;
using Phenix.Services.Host.Core;

namespace Phenix.Services.Host.Service
{
  public sealed class Data : MarshalByRefObject, IData
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

    #region 属性

    #region IData 成员

    #region Sequence

    public long SequenceValue
    {
      get
      {
        ServiceManager.CheckActive();
        return DataHub.SequenceValue; 
      }
    }

    #endregion

    #endregion

    #endregion

    #region 方法

    #region Sequence

    public long[] GetSequenceValues(int count)
    {
      ServiceManager.CheckActive();
      return DataHub.GetSequenceValues(count);
    }

    #endregion

    #region Schema

    public string GetTablesContent(string dataSourceKey, UserIdentity identity)
    {
      ServiceManager.CheckIn(identity);
      DataSecurityContext context = DataSecurityHub.CheckIn(identity, false);
      return DataHub.GetTablesContent(dataSourceKey, context.Identity);
    }

    public string GetTableColumnsContent(string dataSourceKey, string tableName, UserIdentity identity)
    {
      ServiceManager.CheckIn(identity);
      DataSecurityContext context = DataSecurityHub.CheckIn(identity, false);
      return DataHub.GetTableColumnsContent(dataSourceKey, tableName, context.Identity);
    }

    public string GetTablePrimaryKeysContent(string dataSourceKey, string tableName, UserIdentity identity)
    {
      ServiceManager.CheckIn(identity);
      DataSecurityContext context = DataSecurityHub.CheckIn(identity, false);
      return DataHub.GetTablePrimaryKeysContent(dataSourceKey, tableName, context.Identity);
    }

    public string GetTableForeignKeysContent(string dataSourceKey, string tableName, UserIdentity identity)
    {
      ServiceManager.CheckIn(identity);
      DataSecurityContext context = DataSecurityHub.CheckIn(identity, false);
      return DataHub.GetTableForeignKeysContent(dataSourceKey, tableName, context.Identity);
    }

    public string GetTableDetailForeignKeysContent(string dataSourceKey, string tableName, UserIdentity identity)
    {
      ServiceManager.CheckIn(identity);
      DataSecurityContext context = DataSecurityHub.CheckIn(identity, false);
      return DataHub.GetTableDetailForeignKeysContent(dataSourceKey, tableName, context.Identity);
    }

    public string GetTableIndexesContent(string dataSourceKey, string tableName, UserIdentity identity)
    {
      ServiceManager.CheckIn(identity);
      DataSecurityContext context = DataSecurityHub.CheckIn(identity, false);
      return DataHub.GetTableIndexesContent(dataSourceKey, tableName, context.Identity);
    }

    public string GetViewsContent(string dataSourceKey, UserIdentity identity)
    {
      ServiceManager.CheckIn(identity);
      DataSecurityContext context = DataSecurityHub.CheckIn(identity, false);
      return DataHub.GetViewsContent(dataSourceKey, context.Identity);
    }

    public string GetViewColumnsContent(string dataSourceKey, string viewName, UserIdentity identity)
    {
      ServiceManager.CheckIn(identity);
      DataSecurityContext context = DataSecurityHub.CheckIn(identity, false);
      return DataHub.GetViewColumnsContent(dataSourceKey, viewName, context.Identity);
    }

    #endregion

    #region Execute

    public IService Execute(IService service, UserIdentity identity)
    {
      ServiceManager.CheckIn(identity);
      DataSecurityContext context = DataSecurityHub.CheckIn(service.GetType(), ExecuteAction.Update, identity);

      DateTime dt = DateTime.Now;

      IService result = DataHub.Execute(service, context.Identity);
      //OnDataSecurityChanged(new DataSecurityEventArgs(identity.Name));

      //跟踪日志
      if (AppConfig.Debugging)
        PerformanceAnalyse.Default.CheckSaveMaxElapsedTime(service.GetType().FullName, DateTime.Now.Subtract(dt).TotalSeconds, -1, context.Identity);

      return result;
    }

    public IService UploadFiles(IService service, IDictionary<string, Stream> fileStreams, UserIdentity identity)
    {
      throw new NotSupportedException(MethodBase.GetCurrentMethod().Name);
    }

    public IService UploadFiles(IService service, IDictionary<string, byte[]> fileByteses, UserIdentity identity)
    {
      ServiceManager.CheckIn(identity);
      DataSecurityContext context = DataSecurityHub.CheckIn(service.GetType(), ExecuteAction.Update, identity);

      DateTime dt = DateTime.Now;

      IService result = DataHub.UploadFiles(service, fileByteses, context.Identity);
      //OnDataSecurityChanged(new DataSecurityEventArgs(identity.Name));

      //跟踪日志
      if (AppConfig.Debugging)
        PerformanceAnalyse.Default.CheckSaveMaxElapsedTime(service.GetType().FullName, DateTime.Now.Subtract(dt).TotalSeconds, -1, context.Identity);

      return result;
    }

    public IService UploadBigFile(IService service, FileChunkInfo fileChunkInfo, UserIdentity identity)
    {
      ServiceManager.CheckIn(identity);
      DataSecurityContext context = DataSecurityHub.CheckIn(service.GetType(), ExecuteAction.Update, identity);

      DateTime dt = DateTime.Now;

      IService result = DataHub.UploadBigFile(service, fileChunkInfo, context.Identity);
      //OnDataSecurityChanged(new DataSecurityEventArgs(identity.Name));

      //跟踪日志
      if (AppConfig.Debugging)
        PerformanceAnalyse.Default.CheckSaveMaxElapsedTime(service.GetType().FullName, DateTime.Now.Subtract(dt).TotalSeconds, -1, context.Identity);

      return result;
    }

    public Stream DownloadFile(IService service, UserIdentity identity)
    {
      throw new NotSupportedException(MethodBase.GetCurrentMethod().Name);
    }

    public byte[] DownloadFileBytes(IService service, UserIdentity identity)
    {
      ServiceManager.CheckIn(identity);
      DataSecurityContext context = DataSecurityHub.CheckIn(service.GetType(), ExecuteAction.Update, identity);

      DateTime dt = DateTime.Now;

      byte[] result = DataHub.DownloadFileBytes(service, context.Identity);
      //OnDataSecurityChanged(new DataSecurityEventArgs(identity.Name));

      //跟踪日志
      if (AppConfig.Debugging)
        PerformanceAnalyse.Default.CheckSaveMaxElapsedTime(service.GetType().FullName, DateTime.Now.Subtract(dt).TotalSeconds, -1, context.Identity);

      return result;
    }
    
    public FileChunkInfo DownloadBigFile(IService service, int chunkNumber, UserIdentity identity)
    {
      ServiceManager.CheckIn(identity);
      DataSecurityContext context = DataSecurityHub.CheckIn(service.GetType(), ExecuteAction.Update, identity);

      DateTime dt = DateTime.Now;

      FileChunkInfo result = DataHub.DownloadBigFile(service, chunkNumber, context.Identity);
      //OnDataSecurityChanged(new DataSecurityEventArgs(identity.Name));

      //跟踪日志
      if (AppConfig.Debugging)
        PerformanceAnalyse.Default.CheckSaveMaxElapsedTime(service.GetType().FullName, DateTime.Now.Subtract(dt).TotalSeconds, -1, context.Identity);

      return result;
    }

    #endregion

    #region Fetch

    public object Fetch(ICriterions criterions, UserIdentity identity)
    {
      ServiceManager.CheckIn(identity);
      DataSecurityContext context = DataSecurityHub.CheckIn(criterions.ResultType, ExecuteAction.Fetch, identity);

      DateTime dt = DateTime.Now;

      object result = DataHub.Fetch(criterions, context.Identity);
      //OnDataSecurityChanged(new DataSecurityEventArgs(identity.Name));

      //跟踪日志
      if (AppConfig.Debugging)
      {
        int count = result != null ? 1 : 0;

        EventLog.SaveLocal(MethodBase.GetCurrentMethod().Name + ' ' + criterions.ResultType.FullName +
          (criterions.Criteria != null ? " with " + criterions.Criteria.GetType().FullName : String.Empty) +
          " take " + DateTime.Now.Subtract(dt).TotalMilliseconds.ToString(CultureInfo.InvariantCulture) + " millisecond," +
          " count = " + count);

        PerformanceAnalyse.Default.CheckFetchMaxCount(criterions.ResultType.FullName, count, context.Identity);
        PerformanceAnalyse.Default.CheckFetchMaxElapsedTime(criterions.ResultType.FullName, DateTime.Now.Subtract(dt).TotalSeconds, count, context.Identity);
      }

      return result;
    }

    public IList<object> FetchList(ICriterions criterions, UserIdentity identity)
    {
      ServiceManager.CheckIn(identity);
      DataSecurityContext context = DataSecurityHub.CheckIn(criterions.ResultType, ExecuteAction.Fetch, identity);

      DateTime dt = DateTime.Now;

      IList<object> result = DataHub.FetchList(criterions, context.Identity);
      //OnDataSecurityChanged(new DataSecurityEventArgs(identity.Name));

      //跟踪日志
      if (AppConfig.Debugging)
      {
        int count = result != null ? result.Count : 0;

        EventLog.SaveLocal(MethodBase.GetCurrentMethod().Name + ' ' + criterions.ResultType.FullName +
          (criterions.Criteria != null ? " with " + criterions.Criteria.GetType().FullName : String.Empty) +
          " take " + DateTime.Now.Subtract(dt).TotalMilliseconds.ToString(CultureInfo.InvariantCulture) + " millisecond," +
          " count = " + count);

        PerformanceAnalyse.Default.CheckFetchMaxCount(criterions.ResultType.FullName, count, context.Identity);
        PerformanceAnalyse.Default.CheckFetchMaxElapsedTime(criterions.ResultType.FullName, DateTime.Now.Subtract(dt).TotalSeconds, count, context.Identity);
      }

      return result;
    }

    public string FetchContent(ICriterions criterions, UserIdentity identity)
    {
      ServiceManager.CheckIn(identity);
      DataSecurityContext context = DataSecurityHub.CheckIn(criterions.ResultType, ExecuteAction.Fetch, identity);

      DateTime dt = DateTime.Now;

      string result = DataHub.FetchContent(criterions, context.Identity);
      //OnDataSecurityChanged(new DataSecurityEventArgs(identity.Name));

      //跟踪日志
      if (AppConfig.Debugging)
      {
        int length = result != null ? result.Length : 0;

        EventLog.SaveLocal(MethodBase.GetCurrentMethod().Name + ' ' + criterions.ResultType.FullName +
          (criterions.Criteria != null ? " with " + criterions.Criteria.GetType().FullName : String.Empty) +
          " take " + DateTime.Now.Subtract(dt).TotalMilliseconds.ToString(CultureInfo.InvariantCulture) + " millisecond," +
          " length = " + length);

        PerformanceAnalyse.Default.CheckFetchMaxCount(criterions.ResultType.FullName, length, context.Identity);
        PerformanceAnalyse.Default.CheckFetchMaxElapsedTime(criterions.ResultType.FullName, DateTime.Now.Subtract(dt).TotalSeconds, length, context.Identity);
      }

      return result;
    }

    #endregion

    #region Save

    public bool Save(IEntity entity, bool needCheckDirty, UserIdentity identity)
    {
      ServiceManager.CheckIn(identity);
      DataSecurityContext context = DataSecurityHub.CheckIn(entity.GetType(), ExecuteAction.Update, identity);
      
      DateTime dt = DateTime.Now;

      bool result = DataHub.Save(entity, needCheckDirty, context.Identity);
      //OnDataSecurityChanged(new DataSecurityEventArgs(identity.Name));
      
      //跟踪日志
      if (AppConfig.Debugging)
        PerformanceAnalyse.Default.CheckSaveMaxElapsedTime(entity.GetType().FullName, DateTime.Now.Subtract(dt).TotalSeconds, result ? 1 : 0, context.Identity);

      return result;
    }

    public int Save(IEntityCollection entityCollection, UserIdentity identity)
    {
      ServiceManager.CheckIn(identity);
      DataSecurityContext context = DataSecurityHub.CheckIn(entityCollection.GetType(), ExecuteAction.Update, identity);

      DateTime dt = DateTime.Now;

      int result = DataHub.Save(entityCollection, context.Identity);
      //OnDataSecurityChanged(new DataSecurityEventArgs(identity.Name));

      //跟踪日志
      if (AppConfig.Debugging)
        PerformanceAnalyse.Default.CheckSaveMaxElapsedTime(entityCollection.GetType().FullName, DateTime.Now.Subtract(dt).TotalSeconds, entityCollection.Count, context.Identity);

      return result;
    }

    public int SaveContent(string objectTypeName, string source, UserIdentity identity)
    {
      ServiceManager.CheckIn(identity);
      DataSecurityContext context = DataSecurityHub.CheckIn(Type.GetType(objectTypeName), ExecuteAction.Update, identity);

      DateTime dt = DateTime.Now;

      int result = DataHub.SaveContent(objectTypeName, source, identity);
      //OnDataSecurityChanged(new DataSecurityEventArgs(identity.Name));

      //跟踪日志
      if (AppConfig.Debugging)
        PerformanceAnalyse.Default.CheckSaveMaxElapsedTime(objectTypeName, DateTime.Now.Subtract(dt).TotalSeconds, result, context.Identity);

      return result;
    }

    #endregion

    #region Insert

    public bool Insert(object obj, UserIdentity identity)
    {
      ServiceManager.CheckIn(identity);
      DataSecurityContext context = DataSecurityHub.CheckIn(obj.GetType(), ExecuteAction.Insert, identity);
      return DataHub.Insert(obj, context.Identity);
    }

    public int InsertList(IList<IEntity> entities, UserIdentity identity)
    {
      ServiceManager.CheckIn(identity);
      DataSecurityContext context = DataSecurityHub.CheckIn(entities.GetType(), ExecuteAction.Insert, identity);
      return DataHub.InsertList(entities, context.Identity);
    }

    #endregion

    #region Update

    public bool Update(object obj, IList<FieldValue> oldFieldValues, bool needCheckDirty, UserIdentity identity)
    {
      ServiceManager.CheckIn(identity);
      DataSecurityContext context = DataSecurityHub.CheckIn(obj.GetType(), ExecuteAction.Update, identity);
      return DataHub.Update(obj, oldFieldValues, needCheckDirty, context.Identity);
    }

    public int UpdateList(IList<IEntity> entities, UserIdentity identity)
    {
      ServiceManager.CheckIn(identity);
      DataSecurityContext context = DataSecurityHub.CheckIn(entities.GetType(), ExecuteAction.Update, identity);
      return DataHub.UpdateList(entities, context.Identity);
    }

    #endregion

    #region Delete

    public bool Delete(object obj, IList<FieldValue> oldFieldValues, bool needCheckDirty, UserIdentity identity)
    {
      ServiceManager.CheckIn(identity);
      DataSecurityContext context = DataSecurityHub.CheckIn(obj.GetType(), ExecuteAction.Delete, identity);
      return DataHub.Delete(obj, oldFieldValues, needCheckDirty, context.Identity);
    }

    public int DeleteList(IList<IEntity> entities, UserIdentity identity)
    {
      ServiceManager.CheckIn(identity);
      DataSecurityContext context = DataSecurityHub.CheckIn(entities.GetType(), ExecuteAction.Delete, identity);
      return DataHub.DeleteList(entities, context.Identity);
    }

    #endregion

    #region UpdateRecord

    public int UpdateRecord(ICriterions criterions, IDictionary<string, object> data, UserIdentity identity)
    {
      ServiceManager.CheckIn(identity);
      DataSecurityContext context = DataSecurityHub.CheckIn(criterions.ResultType, ExecuteAction.Update, identity);
      return DataHub.UpdateRecord(criterions, data, context.Identity);
    }

    #endregion

    #region DeleteRecord

    public int DeleteRecord(ICriterions criterions, UserIdentity identity)
    {
      ServiceManager.CheckIn(identity);
      DataSecurityContext context = DataSecurityHub.CheckIn(criterions.ResultType, ExecuteAction.Delete, identity);
      return DataHub.DeleteRecord(criterions, context.Identity);
    }

    #endregion

    #region RecordCount

    public long GetRecordCount(ICriterions criterions, UserIdentity identity)
    {
      ServiceManager.CheckIn(identity);
      DataSecurityContext context = DataSecurityHub.CheckIn(criterions.ResultType, ExecuteAction.Fetch, identity);
      return DataHub.GetRecordCount(criterions, context.Identity);
    }

    #endregion

    #region CheckRepeated

    public bool CheckRepeated(IEntity entity, UserIdentity identity)
    {
      ServiceManager.CheckIn(identity);
      DataSecurityContext context = DataSecurityHub.CheckIn(identity, false);
      return DataHub.CheckRepeated(entity, context.Identity);
    }

    public IList<IEntity> CheckRepeated(string rootTypeName, IList<IEntity> entities, UserIdentity identity)
    {
      ServiceManager.CheckIn(identity);
      DataSecurityContext context = DataSecurityHub.CheckIn(identity, false);
      return DataHub.CheckRepeated(rootTypeName, entities, context.Identity);
    }

    #endregion

    #region BusinessCode

    public long GetBusinessCodeSerial(string key, long initialValue, UserIdentity identity)
    {
      ServiceManager.CheckIn(identity);
      DataSecurityContext context = DataSecurityHub.CheckIn(identity, false);
      return DataHub.GetBusinessCodeSerial(key, initialValue, context.Identity);
    }

    public long[] GetBusinessCodeSerials(string key, long initialValue, int count, UserIdentity identity)
    {
      ServiceManager.CheckIn(identity);
      DataSecurityContext context = DataSecurityHub.CheckIn(identity, false);
      return DataHub.GetBusinessCodeSerials(key, initialValue, count, context.Identity);
    }

    #endregion

    #region 应用服务不支持传事务

    public object Fetch(DbConnection connection, ICriterions criterions)
    {
      throw new NotSupportedException(MethodBase.GetCurrentMethod().Name);
    }

    public object Fetch(DbTransaction transaction, ICriterions criterions)
    {
      throw new NotSupportedException(MethodBase.GetCurrentMethod().Name);
    }

    public IList<object> FetchList(DbConnection connection, ICriterions criterions)
    {
      throw new NotSupportedException(MethodBase.GetCurrentMethod().Name);
    }

    public IList<object> FetchList(DbTransaction transaction, ICriterions criterions)
    {
      throw new NotSupportedException(MethodBase.GetCurrentMethod().Name);
    }

    public bool Save(DbConnection connection, IEntity entity, bool needCheckDirty)
    {
      throw new NotSupportedException(MethodBase.GetCurrentMethod().Name);
    }

    public bool Save(DbTransaction transaction, IEntity entity, bool needCheckDirty)
    {
      throw new NotSupportedException(MethodBase.GetCurrentMethod().Name);
    }

    public int Save(DbConnection connection, IEntityCollection entityCollection)
    {
      throw new NotSupportedException(MethodBase.GetCurrentMethod().Name);
    }

    public int Save(DbTransaction transaction, IEntityCollection entityCollection)
    {
      throw new NotSupportedException(MethodBase.GetCurrentMethod().Name);
    }

    public bool Insert(DbConnection connection, object obj)
    {
      throw new NotSupportedException(MethodBase.GetCurrentMethod().Name);
    }

    public bool Insert(DbTransaction transaction, object obj)
    {
      throw new NotSupportedException(MethodBase.GetCurrentMethod().Name);
    }

    public int InsertList(DbConnection connection, IList<IEntity> entities)
    {
      throw new NotSupportedException(MethodBase.GetCurrentMethod().Name);
    }

    public int InsertList(DbTransaction transaction, IList<IEntity> entities)
    {
      throw new NotSupportedException(MethodBase.GetCurrentMethod().Name);
    }

    public bool Update(DbConnection connection, object obj, IList<FieldValue> oldFieldValues, bool needCheckDirty)
    {
      throw new NotSupportedException(MethodBase.GetCurrentMethod().Name);
    }

    public bool Update(DbTransaction transaction, object obj, IList<FieldValue> oldFieldValues, bool needCheckDirty)
    {
      throw new NotSupportedException(MethodBase.GetCurrentMethod().Name);
    }

    public int UpdateList(DbConnection connection, IList<IEntity> entities)
    {
      throw new NotSupportedException(MethodBase.GetCurrentMethod().Name);
    }

    public int UpdateList(DbTransaction transaction, IList<IEntity> entities)
    {
      throw new NotSupportedException(MethodBase.GetCurrentMethod().Name);
    }

    public bool Delete(DbConnection connection, object obj, IList<FieldValue> oldFieldValues, bool needCheckDirty)
    {
      throw new NotSupportedException(MethodBase.GetCurrentMethod().Name);
    }

    public bool Delete(DbTransaction transaction, object obj, IList<FieldValue> oldFieldValues, bool needCheckDirty)
    {
      throw new NotSupportedException(MethodBase.GetCurrentMethod().Name);
    }

    public int DeleteList(DbConnection connection, IList<IEntity> entities)
    {
      throw new NotSupportedException(MethodBase.GetCurrentMethod().Name);
    }

    public int DeleteList(DbTransaction transaction, IList<IEntity> entities)
    {
      throw new NotSupportedException(MethodBase.GetCurrentMethod().Name);
    }

    public int UpdateRecord(DbConnection connection, ICriterions criterions, IDictionary<string, object> data)
    {
      throw new NotSupportedException(MethodBase.GetCurrentMethod().Name);
    }

    public int UpdateRecord(DbTransaction transaction, ICriterions criterions, IDictionary<string, object> data)
    {
      throw new NotSupportedException(MethodBase.GetCurrentMethod().Name);
    }

    public int DeleteRecord(DbConnection connection, ICriterions criterions)
    {
      throw new NotSupportedException(MethodBase.GetCurrentMethod().Name);
    }

    public int DeleteRecord(DbTransaction transaction, ICriterions criterions)
    {
      throw new NotSupportedException(MethodBase.GetCurrentMethod().Name);
    }

    public long GetRecordCount(DbConnection connection, ICriterions criterions)
    {
      throw new NotSupportedException(MethodBase.GetCurrentMethod().Name);
    }

    public long GetRecordCount(DbTransaction transaction, ICriterions criterions)
    {
      throw new NotSupportedException(MethodBase.GetCurrentMethod().Name);
    }

    public long GetBusinessCodeSerial(DbConnection connection, string key, long initialValue)
    {
      throw new NotSupportedException(MethodBase.GetCurrentMethod().Name);
    }

    public long[] GetBusinessCodeSerials(DbConnection connection, string key, long initialValue, int count)
    {
      throw new NotSupportedException(MethodBase.GetCurrentMethod().Name);
    }

    #endregion

    #endregion
  }
}