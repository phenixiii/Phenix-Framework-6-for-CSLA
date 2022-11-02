using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Globalization;
using System.IO;
using System.Net;
using System.Runtime.Serialization;
using System.Text;
using System.Threading;
using Newtonsoft.Json;
using Phenix.Core;
using Phenix.Core.Cache;
using Phenix.Core.Data;
using Phenix.Core.Data.Schema;
using Phenix.Core.IO;
using Phenix.Core.Mapping;
using Phenix.Core.Net;
using Phenix.Core.Reflection;
using Phenix.Core.Security;

namespace Phenix.Services.Library
{
  internal class Data : IData
  {
    #region 属性

    #region Sequence

    private static readonly object _sequenceValueLock = new object();
    private static readonly long _minValue = DateTime.MinValue.AddYears(2000).Ticks;
    private static bool _haveSequenceMarker;
    private static int? _sequenceMarker;
    private static long? _sequenceValue;
    public long SequenceValue
    {
      get
      {
        if (!_haveSequenceMarker)
        {
          _sequenceMarker = DefaultDatabase.ExecuteGet(GetSequenceMarker);
          _haveSequenceMarker = true;
        }
        if (!_sequenceMarker.HasValue)
          return Sequence.LocalValue;
        lock (_sequenceValueLock)
        {
          if (!_sequenceValue.HasValue)
          {
            long value;
            if (Int64.TryParse(AppSettings.ReadValue(typeof(Sequence).FullName), out value))
              _sequenceValue = value;
            AppDomain.CurrentDomain.ProcessExit += delegate
            {
              AppSettings.SaveValue(typeof(Sequence).FullName, SequenceValue.ToString());
            };
          }
          long i = (DateTime.Now.Ticks - _minValue) / 10000 * 1000 + _sequenceMarker.Value;
          while (i <= _sequenceValue) { i = i + 1000; }
          _sequenceValue = i;
          return _sequenceValue.Value;
        }
      }
    }

    #endregion

    #endregion

    #region 方法
    
    #region Sequence

    private static int? GetSequenceMarker(DbConnection connection)
    {
      int? result = null;
      using (DataReader reader = new DataReader(connection,
@"select SM_ID
  from PH_SequenceMarker
  where SM_HostAddress = :SM_HostAddress",
        CommandBehavior.SingleRow, false))
      {
        reader.CreateParameter("SM_HostAddress", NetConfig.LocalAddress);
        if (reader.Read())
          result = reader.GetInt32(0);
      }
      if (result.HasValue)
      {
        using (DbCommand command = DbCommandHelper.CreateCommand(connection,
@"update PH_SequenceMarker set
    SM_ActiveTime = sysdate
  where SM_HostAddress = :SM_HostAddress"))
        {
          DbCommandHelper.CreateParameter(command, "SM_HostAddress", NetConfig.LocalAddress);
          DbCommandHelper.ExecuteNonQuery(command, false);
        }
        return result.Value;
      }

      using (DbCommand command = DbCommandHelper.CreateCommand(connection,
@"delete PH_SequenceMarker
  where SM_ActiveTime <= :SM_ActiveTime"))
      {
        DbCommandHelper.CreateParameter(command, "SM_ActiveTime", DateTime.Now.AddYears(-1));
        if (DbCommandHelper.ExecuteNonQuery(command, false) > 0)
          result = 0;
      }
      if (!result.HasValue)
        result = Convert.ToInt32(DbCommandHelper.ExecuteScalar(connection, @"select count(*) from PH_SequenceMarker"));
      using (DbCommand command = DbCommandHelper.CreateCommand(connection,
@"insert into PH_SequenceMarker
  (SM_ID, SM_HostAddress, SM_HostName, SM_ActiveTime)
  values
  (:SM_ID, :SM_HostAddress, :SM_HostName, sysdate)"))
      {
        while (result < 1000)
        {
          DbCommandHelper.CreateParameter(command, "SM_ID", result);
          DbCommandHelper.CreateParameter(command, "SM_HostAddress", NetConfig.LocalAddress);
          DbCommandHelper.CreateParameter(command, "SM_HostName", Dns.GetHostName());
          if (DbCommandHelper.ExecuteNonQuery(command, false) == 1)
            return result.Value;
          result = result + 1;
        }
      }
      return null;
    }

    public long[] GetSequenceValues(int count)
    {
      if (count <= 0 || count > Int16.MaxValue)
        throw new ArgumentOutOfRangeException("count", count, @"count > 0 && count <= Int16.MaxValue");
      long[] result = new long[count];
      for (int i = 0; i < count; i++)
        result[i] = SequenceValue;
      return result;
    }

    #endregion

    #region Schema

    public string GetTablesContent(string dataSourceKey, UserIdentity identity)
    {
      return Utilities.JsonSerialize(Database.Fetch(dataSourceKey).Tables);
    }

    public string GetTableColumnsContent(string dataSourceKey, string tableName, UserIdentity identity)
    {
      return Utilities.JsonSerialize(Database.Fetch(dataSourceKey).Tables[tableName].Columns);
    }

    public string GetTablePrimaryKeysContent(string dataSourceKey, string tableName, UserIdentity identity)
    {
      return Utilities.JsonSerialize(Database.Fetch(dataSourceKey).Tables[tableName].PrimaryKeys);
    }

    public string GetTableForeignKeysContent(string dataSourceKey, string tableName, UserIdentity identity)
    {
      return Utilities.JsonSerialize(Database.Fetch(dataSourceKey).Tables[tableName].ForeignKeys);
    }

    public string GetTableDetailForeignKeysContent(string dataSourceKey, string tableName, UserIdentity identity)
    {
      return Utilities.JsonSerialize(Database.Fetch(dataSourceKey).Tables[tableName].DetailForeignKeys);
    }

    public string GetTableIndexesContent(string dataSourceKey, string tableName, UserIdentity identity)
    {
      return Utilities.JsonSerialize(Database.Fetch(dataSourceKey).Tables[tableName].Indexes);
    }

    public string GetViewsContent(string dataSourceKey, UserIdentity identity)
    {
      return Utilities.JsonSerialize(Database.Fetch(dataSourceKey).Views);
    }

    public string GetViewColumnsContent(string dataSourceKey, string viewName, UserIdentity identity)
    {
      return Utilities.JsonSerialize(Database.Fetch(dataSourceKey).Views[viewName].Columns);
    }

    #endregion

    #region Execute

    public IService Execute(IService service, UserIdentity identity)
    {
      service.DoExecute();
      return service;
    }

    public IService UploadFiles(IService service, IDictionary<string, Stream> fileStreams, UserIdentity identity)
    {
      service.DoUploadFiles(fileStreams);
      return service;
    }

    public IService UploadFiles(IService service, IDictionary<string, byte[]> fileByteses, UserIdentity identity)
    {
      service.DoUploadFiles(fileByteses);
      return service;
    }

    public IService UploadBigFile(IService service, FileChunkInfo fileChunkInfo, UserIdentity identity)
    {
      service.DoUploadBigFile(fileChunkInfo);
      return service;
    }

    public Stream DownloadFile(IService service, UserIdentity identity)
    {
      return service.DoDownloadFile();
    }

    public byte[] DownloadFileBytes(IService service, UserIdentity identity)
    {
      return service.DoDownloadFileBytes();
    }

    public FileChunkInfo DownloadBigFile(IService service, int chunkNumber, UserIdentity identity)
    {
      return service.DoDownloadBigFile(chunkNumber);
    }

    #endregion

    #region Fetch

    public object Fetch(ICriterions criterions, UserIdentity identity)
    {
      return DbConnectionHelper.ExecuteGet(criterions.DataSourceKey, (Func<DbConnection, ICriterions, object>)Fetch, criterions);
    }

    public IList<object> FetchList(ICriterions criterions, UserIdentity identity)
    {
      return DbConnectionHelper.ExecuteGet(criterions.DataSourceKey, (Func<DbConnection, ICriterions, IList<object>>)FetchList, criterions);
    }

    public string FetchContent(ICriterions criterions, UserIdentity identity)
    {
      return DbConnectionHelper.ExecuteGet(criterions.DataSourceKey, ExecuteFetchContent, criterions);
    }

    private static string ExecuteFetchContent(DbConnection connection, ICriterions criterions)
    {
      using (DbCommand command = DbCommandHelper.CreateCommand(connection))
      {
        Mapper.SetSelectCommand(command, criterions);
        return criterions.ResultIsArray ? JsonSerializeObjectList(command, criterions.ResultCoreType) : JsonSerializeObject(command, criterions.ResultCoreType);
      }
    }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2202:DoNotDisposeObjectsMultipleTimes")]
    private static string JsonSerializeObjectList(DbCommand command, Type objectType)
    {
      using (DbDataReader reader = DbCommandHelper.ExecuteReader(command, CommandBehavior.SingleResult))
      {
        IList<FieldMapInfo> fieldMapInfos = ClassMemberHelper.GetFieldMapInfos(objectType, reader);
        StringBuilder result = new StringBuilder();
        using (StringWriter stringWriter = new StringWriter(result, CultureInfo.InvariantCulture))
        using (JsonTextWriter jsonWriter = new JsonTextWriter(stringWriter))
        {
          jsonWriter.DateFormatString = "yyyy'-'MM'-'dd' 'HH':'mm':'ss";
          jsonWriter.Formatting = Formatting.None;
          jsonWriter.WriteStartArray();
          while (reader.Read())
            JsonSerializeFieldValues(reader, jsonWriter, fieldMapInfos);
          jsonWriter.WriteEndArray();
          jsonWriter.Flush();
        }
        return result.ToString();
      }
    }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2202:DoNotDisposeObjectsMultipleTimes")]
    private static string JsonSerializeObject(DbCommand command, Type objectType)
    {
      using (DbDataReader reader = DbCommandHelper.ExecuteReader(command, CommandBehavior.SingleRow))
      {
        if (reader.Read())
        {
          IList<FieldMapInfo> fieldMapInfos = ClassMemberHelper.GetFieldMapInfos(objectType, reader);
          StringBuilder result = new StringBuilder();
          using (StringWriter stringWriter = new StringWriter(result, CultureInfo.InvariantCulture))
          using (JsonTextWriter jsonWriter = new JsonTextWriter(stringWriter))
          {
            jsonWriter.DateFormatString = "yyyy'-'MM'-'dd' 'HH':'mm':'ss";
            jsonWriter.Formatting = Formatting.None;
            JsonSerializeFieldValues(reader, jsonWriter, fieldMapInfos);
            jsonWriter.Flush();
          }
          return result.ToString();
        }
      }
      return null;
    }

    private static void JsonSerializeFieldValues(IDataRecord sourceFieldValues, JsonTextWriter target, IList<FieldMapInfo> targetFieldMapInfos)
    {
      target.WriteStartObject();
      for (int i = 0; i < targetFieldMapInfos.Count; i++)
      {
        FieldMapInfo fieldMapInfo = targetFieldMapInfos[i];
        target.WritePropertyName(fieldMapInfo.PropertyName);
        target.WriteValue(Utilities.ChangeType(sourceFieldValues.IsDBNull(i) ? null : sourceFieldValues.GetValue(i), fieldMapInfo.Property.PropertyType));
      }
      target.WriteEndObject();
    }

    #endregion

    #region Save

    public bool Save(IEntity entity, bool needCheckDirty, UserIdentity identity)
    {
      return DbConnectionHelper.ExecuteGet(entity.DataSourceKey, (Func<DbTransaction, IEntity, bool, bool>)Save, entity, needCheckDirty);
    }

    public int Save(IEntityCollection entityCollection, UserIdentity identity)
    {
      return DbConnectionHelper.ExecuteGet(entityCollection.DataSourceKey, (Func<DbTransaction, IEntityCollection, int>)Save, entityCollection);
    }

    public int SaveContent(string objectTypeName, string source, UserIdentity identity)
    {
      if (String.IsNullOrEmpty(source))
        return -1;
      Type objectType = Type.GetType(objectTypeName);
      ClassMapInfo classMapInfo = ClassMemberHelper.GetClassMapInfo(objectType, true, true);
      if (typeof(IList).IsAssignableFrom(objectType))
      {
        IList<IDictionary<string, object>> propertyValuesList = EntityListHelper.JsonDeserializeNameValues(source);
        if (propertyValuesList != null)
          return DbConnectionHelper.ExecuteGet(classMapInfo.ClassAttribute.DataSourceKey, ExecuteSave, classMapInfo, propertyValuesList);
      }
      else
      {
        IDictionary<string, object> propertyValue = EntityHelper.JsonDeserializeNameValues(source);
        if (propertyValue != null)
          return DbConnectionHelper.ExecuteGet(classMapInfo.ClassAttribute.DataSourceKey, ExecuteSave, classMapInfo, propertyValue);
      }
      if (typeof(Csla.Core.ISavable).IsAssignableFrom(objectType))
      {
        Csla.Core.ISavable savable = (Csla.Core.ISavable)Utilities.JsonDeserialize(source, objectType);
        savable.Save();
        return Int32.MaxValue;
      }
      return typeof(IList).IsAssignableFrom(objectType)
        ? DbConnectionHelper.ExecuteGet(classMapInfo.ClassAttribute.DataSourceKey, ExecuteSave, classMapInfo, EntityListHelper.JsonDeserializeValues(source))
        : DbConnectionHelper.ExecuteGet(classMapInfo.ClassAttribute.DataSourceKey, ExecuteSave, classMapInfo, EntityHelper.JsonDeserializeValues(source));
    }

    #endregion

    #region Insert

    public bool Insert(object obj, UserIdentity identity)
    {
      return DbConnectionHelper.ExecuteGet(obj is IEntity ? ((IEntity)obj).DataSourceKey : ClassMemberHelper.GetDataSourceKey(obj.GetType()), (Func<DbTransaction, object, bool>)Insert, obj);
    }

    public int InsertList(IList<IEntity> entities, UserIdentity identity)
    {
      return DbConnectionHelper.ExecuteGet(ClassMemberHelper.GetDataSourceKey(entities.GetType()), (Func<DbTransaction, IList<IEntity>, int>)InsertList, entities);
    }

    #endregion

    #region Update

    public bool Update(object obj, IList<FieldValue> oldFieldValues, bool needCheckDirty, UserIdentity identity)
    {
      return DbConnectionHelper.ExecuteGet(obj is IEntity ? ((IEntity)obj).DataSourceKey : ClassMemberHelper.GetDataSourceKey(obj.GetType()), (Func<DbTransaction, object, IList<FieldValue>, bool, bool>)Update, obj, oldFieldValues, needCheckDirty);
    }

    public int UpdateList(IList<IEntity> entities, UserIdentity identity)
    {
      return DbConnectionHelper.ExecuteGet(ClassMemberHelper.GetDataSourceKey(entities.GetType()), (Func<DbTransaction, IList<IEntity>, int>)UpdateList, entities);
    }

    #endregion

    #region Delete

    public bool Delete(object obj, IList<FieldValue> oldFieldValues, bool needCheckDirty, UserIdentity identity)
    {
      return DbConnectionHelper.ExecuteGet(obj is IEntity ? ((IEntity)obj).DataSourceKey : ClassMemberHelper.GetDataSourceKey(obj.GetType()), (Func<DbTransaction, object, IList<FieldValue>, bool, bool>)Delete, obj, oldFieldValues, needCheckDirty);
    }

    public int DeleteList(IList<IEntity> entities, UserIdentity identity)
    {
      return DbConnectionHelper.ExecuteGet(ClassMemberHelper.GetDataSourceKey(entities.GetType()), (Func<DbTransaction, IList<IEntity>, int>)DeleteList, entities);
    }

    #endregion

    #region UpdateRecord

    public int UpdateRecord(ICriterions criterions, IDictionary<string, object> data, UserIdentity identity)
    {
      return DbConnectionHelper.ExecuteGet(criterions.DataSourceKey, (Func<DbConnection, ICriterions, IDictionary<string, object>, int>)UpdateRecord, criterions, data);
    }
    
    #endregion
    
    #region DeleteRecord

    public int DeleteRecord(ICriterions criterions, UserIdentity identity)
    {
      return DbConnectionHelper.ExecuteGet(criterions.DataSourceKey, (Func<DbConnection, ICriterions, int>)DeleteRecord, criterions);
    }

    #endregion

    #region RecordCount

    public long GetRecordCount(ICriterions criterions, UserIdentity identity)
    {
      return DbConnectionHelper.ExecuteGet(criterions.DataSourceKey, (Func<DbConnection, ICriterions, long>)GetRecordCount, criterions);
    }

    #endregion

    #region CheckRepeated

    public bool CheckRepeated(IEntity entity, UserIdentity identity)
    {
      return DbConnectionHelper.ExecuteGet(entity.DataSourceKey, ExecuteCheckRepeated, entity);
    }

    private static bool ExecuteCheckRepeated(DbConnection connection, IEntity entity)
    {
      foreach (FieldUniqueMapInfo item in ClassMemberHelper.GetFieldUniqueMapInfos(entity.GetType()))
        if (item.IsRepeated(connection, entity))
          return true;
      return false;
    }

    public IList<IEntity> CheckRepeated(string rootTypeName, IList<IEntity> entities, UserIdentity identity)
    {
      return DbConnectionHelper.ExecuteGet(ClassMemberHelper.GetDataSourceKey(Type.GetType(rootTypeName)), ExecuteCheckRepeated, entities);
    }

    private static IList<IEntity> ExecuteCheckRepeated(DbConnection connection, IList<IEntity> entities)
    {
      List<IEntity> result = new List<IEntity>();
      foreach (IEntity item in entities)
        if (item != null && ExecuteCheckRepeated(connection, item))
          result.Add(item);
      return result;
    }

    #endregion

    #region BusinessCode

    public long GetBusinessCodeSerial(string key, long initialValue, UserIdentity identity)
    {
      return DefaultDatabase.ExecuteGet(ExecuteGetBusinessCodeSerial, key, initialValue);
    }

    public long[] GetBusinessCodeSerials(string key, long initialValue, int count, UserIdentity identity)
    {
      return DefaultDatabase.ExecuteGet(ExecuteGetBusinessCodeSerials, key, initialValue, count);
    }

    #endregion

    #region 代入数据库事务

    #region Fetch

    private static object Fetch(DbCommand command, ICriterions criterions)
    {
      if (criterions.ResultIsArray)
      {
        IEntityCollection result = FetchEntityCollection(command, criterions);
        if (result != null)
          return result;
        return FetchList(command, criterions);
      }

      using (DbDataReader reader = DbCommandHelper.ExecuteReader(command, CommandBehavior.SingleRow))
      {
        if (reader.Read())
        {
          object result;
          if (typeof(IFactory).IsAssignableFrom(criterions.ResultCoreType))
          {
            IFactory factory = (IFactory)FormatterServices.GetUninitializedObject(criterions.ResultCoreType);
            result = factory.CreateInstance();
          }
          else
            result = Activator.CreateInstance(criterions.ResultCoreType, true);
          if (EntityHelper.Fetch(reader, result, ClassMemberHelper.GetFieldMapInfos(criterions.ResultCoreType, reader)))
            return result;
        }
      }
      return null;
    }

    private static IEntityCollection FetchEntityCollection(DbCommand command, ICriterions criterions)
    {
      IEntityCollection result;
      if (typeof(IFactory).IsAssignableFrom(criterions.ResultType))
      {
        IFactory factory = (IFactory)FormatterServices.GetUninitializedObject(criterions.ResultType);
        result = factory.CreateInstance() as IEntityCollection;
      }
      else
        result = Activator.CreateInstance(criterions.ResultType, true) as IEntityCollection;
      if (result == null)
        return null;

      using (DbDataReader reader = DbCommandHelper.ExecuteReader(command, CommandBehavior.SingleResult))
      {
        IList<FieldMapInfo> fieldMapInfos = ClassMemberHelper.GetFieldMapInfos(criterions.ResultCoreType, reader);
        bool oldRaiseListChangedEvents = result.RaiseListChangedEvents;
        try
        {
          result.RaiseListChangedEvents = false;
          result.SelfFetching = true;
          if (typeof(IFactory).IsAssignableFrom(criterions.ResultCoreType))
          {
            IFactory factory = (IFactory)FormatterServices.GetUninitializedObject(criterions.ResultCoreType);
            while (reader.Read())
            {
              object obj = factory.CreateInstance();
              if (EntityHelper.Fetch(reader, obj, fieldMapInfos))
                result.Add(obj);
            }
          }
          else
          {
            while (reader.Read())
            {
              object obj = Activator.CreateInstance(criterions.ResultCoreType, true);
              if (EntityHelper.Fetch(reader, obj, fieldMapInfos))
                result.Add(obj);
            }
          }
          result.SelfFetching = false;
        }
        finally
        {
          result.RaiseListChangedEvents = oldRaiseListChangedEvents;
        }
      }
      return result;
    }

    private static IList<object> FetchList(DbCommand command, ICriterions criterions)
    {
      IList<object> result = new List<object>();
      using (DbDataReader reader = DbCommandHelper.ExecuteReader(command, CommandBehavior.SingleResult))
      {
        IList<FieldMapInfo> fieldMapInfos = ClassMemberHelper.GetFieldMapInfos(criterions.ResultCoreType, reader);
        if (typeof(IFactory).IsAssignableFrom(criterions.ResultCoreType))
        {
          IFactory factory = (IFactory)FormatterServices.GetUninitializedObject(criterions.ResultCoreType);
          while (reader.Read())
          {
            object obj = factory.CreateInstance();
            if (EntityHelper.Fetch(reader, obj, fieldMapInfos))
              result.Add(obj);
          }
        }
        else
        {
          while (reader.Read())
          {
            object obj = Activator.CreateInstance(criterions.ResultCoreType, true);
            if (EntityHelper.Fetch(reader, obj, fieldMapInfos))
              result.Add(obj);
          }
        }
      }
      return result;
    }

    public object Fetch(DbConnection connection, ICriterions criterions)
    {
      using (DbCommand command = DbCommandHelper.CreateCommand(connection))
      {
        Mapper.SetSelectCommand(command, criterions);
        return Fetch(command, criterions);
      }
    }

    public object Fetch(DbTransaction transaction, ICriterions criterions)
    {
      using (DbCommand command = DbCommandHelper.CreateCommand(transaction))
      {
        Mapper.SetSelectCommand(command, criterions);
        return Fetch(command, criterions);
      }
    }

    public IList<object> FetchList(DbConnection connection, ICriterions criterions)
    {
      using (DbCommand command = DbCommandHelper.CreateCommand(connection))
      {
        Mapper.SetSelectCommand(command, criterions);
        return FetchList(command, criterions);
      }
    }

    public IList<object> FetchList(DbTransaction transaction, ICriterions criterions)
    {
      using (DbCommand command = DbCommandHelper.CreateCommand(transaction))
      {
        Mapper.SetSelectCommand(command, criterions);
        return FetchList(command, criterions);
      }
    }

    #endregion

    #region Save

    private static bool Save(DbCommand command, IEntity entity, bool needCheckDirty)
    {
      if (entity.IsNew)
      {
        if (!entity.IsSelfDeleted)
          return Insert(command, entity);
      }
      else if (entity.IsSelfDeleted)
        return Delete(command, entity, entity.OldFieldValues, needCheckDirty);
      else if (entity.IsSelfDirty)
        return Update(command, entity, entity.OldFieldValues, needCheckDirty);
      return false;
    }

    public bool Save(DbConnection connection, IEntity entity, bool needCheckDirty)
    {
      using (DbCommand command = DbCommandHelper.CreateCommand(connection))
      {
        return Save(command, entity, needCheckDirty);
      }
    }

    public bool Save(DbTransaction transaction, IEntity entity, bool needCheckDirty)
    {
      using (DbCommand command = DbCommandHelper.CreateCommand(transaction))
      {
        return Save(command, entity, needCheckDirty);
      }
    }

    public int Save(DbConnection connection, IEntityCollection entityCollection)
    {
      int result = 0;
      foreach (IEntity item in entityCollection)
        if (item != null)
          if (Save(connection, item, item.NeedCheckDirty))
            result = result + 1;
      return result;
    }

    public int Save(DbTransaction transaction, IEntityCollection entityCollection)
    {
      int result = 0;
      foreach (IEntity item in entityCollection)
        if (item != null)
          if (Save(transaction, item, item.NeedCheckDirty))
            result = result + 1;
      return result;
    }

    private static int ExecuteSave(DbTransaction transaction, ClassMapInfo classMapInfo, IDictionary<string, object> propertyValues)
    {
      int result = Save(transaction, classMapInfo, propertyValues);
      if (result > 0)
        ObjectCache.Clear(classMapInfo.OwnerType);
      return result;
    }

    private static int ExecuteSave(DbTransaction transaction, ClassMapInfo classMapInfo, IList<IDictionary<string, object>> propertyValuesList)
    {
      int result = 0;
      foreach (IDictionary<string, object> item in propertyValuesList)
        result = result + Save(transaction, classMapInfo, item);
      if (result > 0)
        ObjectCache.Clear(classMapInfo.OwnerType);
      return result;
    }

    private static int Save(DbTransaction transaction, ClassMapInfo classMapInfo, IDictionary<string, object> propertyValues)
    {
      object isNew;
      if (propertyValues.TryGetValue("IsNew", out isNew) && (bool)Utilities.ChangeType(isNew, typeof(bool)))
      {
        object isSelfDeleted;
        if (!propertyValues.TryGetValue("IsSelfDeleted", out isSelfDeleted) || !(bool)Utilities.ChangeType(isSelfDeleted, typeof(bool)))
          return Insert(transaction, classMapInfo, propertyValues) + SaveDetail(transaction, classMapInfo, propertyValues);
      }
      else
      {
        object isSelfDeleted;
        if (propertyValues.TryGetValue("IsSelfDeleted", out isSelfDeleted) && (bool)Utilities.ChangeType(isSelfDeleted, typeof(bool)))
          return SaveDetail(transaction, classMapInfo, propertyValues) + Delete(transaction, classMapInfo, propertyValues);
        object isSelfDirty;
        if (propertyValues.TryGetValue("IsSelfDirty", out isSelfDirty) && (bool)Utilities.ChangeType(isSelfDirty, typeof(bool)))
          return Update(transaction, classMapInfo, propertyValues) + SaveDetail(transaction, classMapInfo, propertyValues);
      }
      if (!propertyValues.ContainsKey("IsNew") && !propertyValues.ContainsKey("IsSelfDeleted") && !propertyValues.ContainsKey("IsSelfDirty"))
        return Update(transaction, classMapInfo, propertyValues) + SaveDetail(transaction, classMapInfo, propertyValues);
      return 0;
    }

    private static int SaveDetail(DbTransaction transaction, ClassMapInfo classMapInfo, IDictionary<string, object> propertyValues)
    {
      int result = 0;
      foreach (KeyValuePair<string, PropertyMapInfo> kvp in classMapInfo.PropertyMapInfos)
        if (kvp.Value.Property.PropertyType.IsClass)
        {
          object propertyValue;
          if (propertyValues.TryGetValue(kvp.Key, out propertyValue) && propertyValue != null)
          {
            IList<IDictionary<string, object>> propertyValueValuesList = propertyValue as IList<IDictionary<string, object>>;
            if (propertyValueValuesList != null && propertyValueValuesList.Count > 0)
            {
              result = result + ExecuteSave(transaction, ClassMemberHelper.GetClassMapInfo(kvp.Value.Property.PropertyType, true, true), propertyValueValuesList);
              continue;
            }
            IDictionary<string, object> propertyValueValues = propertyValue as IDictionary<string, object>;
            if (propertyValueValues != null && propertyValueValues.Count > 0)
            {
              result = result + ExecuteSave(transaction, ClassMemberHelper.GetClassMapInfo(kvp.Value.Property.PropertyType, true, true), propertyValueValues);
              continue;
            }
          }
        }
      return result;
    }

    private static int Insert(DbTransaction transaction, ClassMapInfo classMapInfo, IDictionary<string, object> propertyValues)
    {
      using (DbCommand command = DbCommandHelper.CreateCommand(transaction))
      {
        Mapper.SetInsertCommand(command, classMapInfo, propertyValues);
        return DbCommandHelper.ExecuteNonQuery(command);
      }
    }

    private static int Update(DbTransaction transaction, ClassMapInfo classMapInfo, IDictionary<string, object> propertyValues)
    {
      using (DbCommand command = DbCommandHelper.CreateCommand(transaction))
      {
        if (Mapper.SetUpdateCommand(command, classMapInfo, propertyValues))
          return DbCommandHelper.ExecuteNonQuery(command);
      }
      return 0;
    }

    private static int Delete(DbTransaction transaction, ClassMapInfo classMapInfo, IDictionary<string, object> propertyValues)
    {
      using (DbCommand command = DbCommandHelper.CreateCommand(transaction))
      {
        if (classMapInfo.DeletedAsDisabled)
        {
          if (!Mapper.SetDisableCommand(command, classMapInfo, propertyValues, true))
            return 0;
        }
        else
          Mapper.SetDeleteCommand(command, classMapInfo, propertyValues);
        return DbCommandHelper.ExecuteNonQuery(command);
      }
    }

    #endregion

    #region Insert

    private static bool Insert(DbCommand command, object obj)
    {
      try
      {
        Mapper.SetInsertCommand(command, obj, null);
        if (DbCommandHelper.ExecuteNonQuery(command) > 0)
        {
          ObjectCache.Clear(obj.GetType());
          return true;
        }
      }
      catch (Exception ex)
      {
        throw new InsertException(obj as IEntity, EntityHelper.CheckRepeated(obj as IEntity, ex));
      }
      return false;
    }

    public bool Insert(DbConnection connection, object obj)
    {
      using (DbCommand command = DbCommandHelper.CreateCommand(connection))
      {
        return Insert(command, obj);
      }
    }

    public bool Insert(DbTransaction transaction, object obj)
    {
      using (DbCommand command = DbCommandHelper.CreateCommand(transaction))
      {
        return Insert(command, obj);
      }
    }

    public int InsertList(DbConnection connection, IList<IEntity> entities)
    {
      int result = 0;
      foreach (IEntity item in entities)
        if (item != null)
          if (Insert(connection, item))
            result = result + 1;
      return result;
    }

    public int InsertList(DbTransaction transaction, IList<IEntity> entities)
    {
      int result = 0;
      foreach (IEntity item in entities)
        if (item != null)
          if (Insert(transaction, item))
            result = result + 1;
      return result;
    }

    #endregion

    #region Update

    private static bool Update(DbCommand command, object obj, IList<FieldValue> oldFieldValues, bool needCheckDirty)
    {
      try
      {
        if (!Mapper.SetUpdateCommand(command, obj, oldFieldValues, needCheckDirty, null))
          return true;
        if (DbCommandHelper.ExecuteNonQuery(command) > 0)
        {
          ObjectCache.Clear(obj.GetType());
          return true;
        }
      }
      catch (Exception ex)
      {
        throw new UpdateException(obj as IEntity, EntityHelper.CheckRepeated(obj as IEntity, ex));
      }
      if (needCheckDirty)
        throw new CheckDirtyException(obj as IEntity);
      return false;
    }

    public bool Update(DbConnection connection, object obj, IList<FieldValue> oldFieldValues, bool needCheckDirty)
    {
      using (DbCommand command = DbCommandHelper.CreateCommand(connection))
      {
        return Update(command, obj, oldFieldValues, needCheckDirty);
      }
    }

    public bool Update(DbTransaction transaction, object obj, IList<FieldValue> oldFieldValues, bool needCheckDirty)
    {
      using (DbCommand command = DbCommandHelper.CreateCommand(transaction))
      {
        return Update(command, obj, oldFieldValues, needCheckDirty);
      }
    }

    public int UpdateList(DbConnection connection, IList<IEntity> entities)
    {
      int result = 0;
      foreach (IEntity item in entities)
        if (item != null)
          if (Update(connection, item, item.OldFieldValues, item.NeedCheckDirty))
            result = result + 1;
      return result;
    }

    public int UpdateList(DbTransaction transaction, IList<IEntity> entities)
    {
      int result = 0;
      foreach (IEntity item in entities)
        if (item != null)
          if (Update(transaction, item, item.OldFieldValues, item.NeedCheckDirty))
            result = result + 1;
      return result;
    }

    #endregion

    #region Delete

    private static bool Delete(DbCommand command, object obj, IList<FieldValue> oldFieldValues, bool needCheckDirty)
    {
      IEntity entity = obj as IEntity;
      try
      {
        if (entity != null && entity.DeletedAsDisabled)
        {
          if (!Mapper.SetDisableCommand(command, obj, oldFieldValues, needCheckDirty, null, true))
            return false;
        }
        else
          Mapper.SetDeleteCommand(command, obj, oldFieldValues, needCheckDirty, null);
        if (DbCommandHelper.ExecuteNonQuery(command) > 0)
        {
          ObjectCache.Clear(obj.GetType());
          return true;
        }
      }
      catch (Exception ex)
      {
        if (entity != null && entity.DeletedAsDisabled)
          throw new DeleteException(entity, EntityHelper.CheckRepeated(entity, ex));
        else
          throw new DeleteException(entity, EntityHelper.CheckOccupied(entity, ex));
      }
      if (needCheckDirty)
        throw new CheckDirtyException(entity);
      return false;
    }

    public bool Delete(DbConnection connection, object obj, IList<FieldValue> oldFieldValues, bool needCheckDirty)
    {
      using (DbCommand command = DbCommandHelper.CreateCommand(connection))
      {
        return Delete(command, obj, oldFieldValues, needCheckDirty);
      }
    }

    public bool Delete(DbTransaction transaction, object obj, IList<FieldValue> oldFieldValues, bool needCheckDirty)
    {
      using (DbCommand command = DbCommandHelper.CreateCommand(transaction))
      {
        return Delete(command, obj, oldFieldValues, needCheckDirty);
      }
    }

    public int DeleteList(DbConnection connection, IList<IEntity> entities)
    {
      int result = 0;
      foreach (IEntity item in entities)
        if (item != null)
          if (Delete(connection, item, item.OldFieldValues, item.NeedCheckDirty))
            result = result + 1;
      return result;
    }

    public int DeleteList(DbTransaction transaction, IList<IEntity> entities)
    {
      int result = 0;
      foreach (IEntity item in entities)
        if (item != null)
          if (Delete(transaction, item, item.OldFieldValues, item.NeedCheckDirty))
            result = result + 1;
      return result;
    }

    #endregion

    #region UpdateRecord

    private static int UpdateRecord(DbCommand command, ICriterions criterions, IDictionary<string, object> data)
    {
      if (Mapper.SetUpdateCommand(command, criterions, data))
      {
        int result = DbCommandHelper.ExecuteNonQuery(command);
        if (result > 0)
          ObjectCache.Clear(criterions.ResultCoreType);
        return result;
      }
      return -1;
    }

    public int UpdateRecord(DbConnection connection, ICriterions criterions, IDictionary<string, object> data)
    {
      using (DbCommand command = DbCommandHelper.CreateCommand(connection))
      {
        return UpdateRecord(command, criterions, data);
      }
    }

    public int UpdateRecord(DbTransaction transaction, ICriterions criterions, IDictionary<string, object> data)
    {
      using (DbCommand command = DbCommandHelper.CreateCommand(transaction))
      {
        return UpdateRecord(command, criterions, data);
      }
    }

    #endregion

    #region DeleteRecord

    private static int DeleteRecord(DbCommand command, ICriterions criterions)
    {
      Mapper.SetDeleteCommand(command, criterions);
      int result = DbCommandHelper.ExecuteNonQuery(command);
      if (result > 0)
        ObjectCache.Clear(criterions.ResultCoreType);
      return result;
    }

    public int DeleteRecord(DbConnection connection, ICriterions criterions)
    {
      using (DbCommand command = DbCommandHelper.CreateCommand(connection))
      {
        return DeleteRecord(command, criterions);
      }
    }

    public int DeleteRecord(DbTransaction transaction, ICriterions criterions)
    {
      using (DbCommand command = DbCommandHelper.CreateCommand(transaction))
      {
        return DeleteRecord(command, criterions);
      }
    }

    #endregion

    #region RecordCount

    private static long GetRecordCount(DbCommand command, ICriterions criterions)
    {
      Mapper.SetSelectCommand(command, criterions, SelectSqlType.RecordCount);
      using (DbDataReader reader = DbCommandHelper.ExecuteReader(command, CommandBehavior.SingleRow))
      {
        if (reader.Read())
          return (long)Utilities.ChangeType(reader.GetValue(0), typeof(long));
      }
      return -1;
    }

    public long GetRecordCount(DbConnection connection, ICriterions criterions)
    {
      using (DbCommand command = DbCommandHelper.CreateCommand(connection))
      {
        return GetRecordCount(command, criterions);
      }
    }

    public long GetRecordCount(DbTransaction transaction, ICriterions criterions)
    {
      using (DbCommand command = DbCommandHelper.CreateCommand(transaction))
      {
        return GetRecordCount(command, criterions);
      }
    }

    #endregion

    #region BusinessCode

    public long GetBusinessCodeSerial(DbConnection connection, string key, long initialValue)
    {
      return ExecuteGetBusinessCodeSerial(connection, key, initialValue);
    }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
    private static long ExecuteGetBusinessCodeSerial(DbConnection connection, string key, long initialValue)
    {
      long? oldValue = null;
      using (DataReader reader = new DataReader(connection,
@"select SR_Value
  from PH_Serial
  where SR_Key = :SR_Key",
        CommandBehavior.SingleRow, false))
      {
        reader.CreateParameter("SR_Key", key);
        if (reader.Read())
          oldValue = reader.GetInt64ForDecimal(0);
      }
      if (!oldValue.HasValue)
      {
        using (DbCommand command = DbCommandHelper.CreateCommand(connection,
@"insert into PH_Serial
  (SR_Key, SR_Value, SR_Time)
  values
  (:SR_Key, :SR_Value, sysdate)"))
        {
          DbCommandHelper.CreateParameter(command, "SR_Key", key);
          DbCommandHelper.CreateParameter(command, "SR_Value", initialValue);
          try
          {
            DbCommandHelper.ExecuteNonQuery(command, false);
            return initialValue;
          }
          catch (Exception)
          {
            // ignored
          }
        }
        oldValue = initialValue;
      }
      using (DbCommand command = DbCommandHelper.CreateCommand(connection,
@"update PH_Serial set
    SR_Value = :New_SR_Value,
    SR_Time = sysdate
  where SR_Key = :SR_Key and SR_Value = :Old_SR_Value"))
      {
        long newValue = oldValue.Value + 1;
        DbParameter newValueParameter = DbCommandHelper.CreateParameter(command, "New_SR_Value", newValue);
        DbCommandHelper.CreateParameter(command, "SR_Key", key);
        DbParameter oldValueParameter = DbCommandHelper.CreateParameter(command, "Old_SR_Value", oldValue);
        do
        {
          if (DbCommandHelper.ExecuteNonQuery(command, false) == 1)
            return newValue;
          Thread.Sleep(100);
          oldValue = newValue;
          newValue = newValue + 1;
          newValueParameter.Value = newValue;
          oldValueParameter.Value = oldValue;
        } while (true);
      }
    }

    public long[] GetBusinessCodeSerials(DbConnection connection, string key, long initialValue, int count)
    {
      return ExecuteGetBusinessCodeSerials(connection, key, initialValue, count);
    }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
    private static long[] ExecuteGetBusinessCodeSerials(DbConnection connection, string key, long initialValue, int count)
    {
      List<long> result = new List<long>(count);
      long? oldValue = null;
      using (DataReader reader = new DataReader(connection,
@"select SR_Value
  from PH_Serial
  where SR_Key = :SR_Key",
        CommandBehavior.SingleRow, false))
      {
        reader.CreateParameter("SR_Key", key);
        if (reader.Read())
          oldValue = reader.GetInt64ForDecimal(0);
      }
      if (!oldValue.HasValue)
      {
        using (DbCommand command = DbCommandHelper.CreateCommand(connection,
@"insert into PH_Serial
  (SR_Key, SR_Value, SR_Time)
  values
  (:SR_Key, :SR_Value, sysdate)"))
        {
          DbCommandHelper.CreateParameter(command, "SR_Key", key);
          DbCommandHelper.CreateParameter(command, "SR_Value", initialValue);
          try
          {
            DbCommandHelper.ExecuteNonQuery(command, false);
            result.Add(initialValue);
            count = count - 1;
          }
          catch (Exception)
          {
            // ignored
          }
        }
        oldValue = initialValue;
      }
      using (DbCommand command = DbCommandHelper.CreateCommand(connection,
@"update PH_Serial set
    SR_Value = :New_SR_Value,
    SR_Time = sysdate
  where SR_Key = :SR_Key and SR_Value = :Old_SR_Value"))
      {
        long newValue = oldValue.Value + 1;
        DbParameter newValueParameter = DbCommandHelper.CreateParameter(command, "New_SR_Value", newValue);
        DbCommandHelper.CreateParameter(command, "SR_Key", key);
        DbParameter oldValueParameter = DbCommandHelper.CreateParameter(command, "Old_SR_Value", oldValue);
        for (int i = 0; i < count; i++)
        {
          do
          {
            try
            {
              if (DbCommandHelper.ExecuteNonQuery(command, false) == 1)
              {
                result.Add(newValue);
                break;
              }
              Thread.Sleep(100);
            }
            finally
            {
              oldValue = newValue;
              newValue = newValue + 1;
              newValueParameter.Value = newValue;
              oldValueParameter.Value = oldValue;
            }
          } while (true);
        }
      }
      return result.ToArray();
    }

    #endregion

    #endregion

    #endregion
  }
}