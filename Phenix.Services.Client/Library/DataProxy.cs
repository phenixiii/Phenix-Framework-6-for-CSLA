using System;
using System.Collections.Generic;
using System.Data.Common;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Reflection;
using Phenix.Core.Data;
using Phenix.Core.IO;
using Phenix.Core.Mapping;
using Phenix.Core.Net;
using Phenix.Core.Security;
using Phenix.Core.SyncCollections;
using Phenix.Services.Contract;

namespace Phenix.Services.Client.Library
{
  internal class DataProxy : IData
  {
    #region 属性

    private IData _service;
    private IData Service
    {
      get
      {
        if (_service == null)
        {
          RemotingHelper.RegisterClientChannel();
          _service = (IData)RemotingHelper.CreateRemoteObjectProxy(typeof(IData), ServicesInfo.DATA_URI);
        }
        return _service;
      }
    }

    private readonly SynchronizedDictionary<string, IData> _serviceCluster =
      new SynchronizedDictionary<string, IData>(StringComparer.Ordinal);

    #region IData 成员

    #region Sequence

    public long SequenceValue
    {
      get
      {
        NetConfig.InitializeSwitch();
        do
        {
          try
          {
            return Service.SequenceValue;
          }
          catch (SocketException)
          {
            InvalidateCache();
            if (!NetConfig.SwitchServicesAddress())
              throw;
          }
        } while (true);
      }
    }

    #endregion

    #endregion

    #endregion

    #region 方法

    private void InvalidateCache()
    {
      _service = null;
    }

    private IData GetService(Type objectType)
    {
      if (NetConfig.ProxyType == ProxyType.Embedded)
        return Service;
      ServicesClusterAttribute clusterAttribute = ServicesClusterAttribute.Fetch(objectType);
      string servicesClusterAddress = NetConfig.GetServicesClusterAddress(clusterAttribute);
      if (String.IsNullOrEmpty(servicesClusterAddress))
        return Service;

      return _serviceCluster.GetValue(clusterAttribute.Key, () =>
      {
        RemotingHelper.RegisterClientChannel();
        return (IData)RemotingHelper.CreateRemoteObjectProxy(typeof(IData), servicesClusterAddress, ServicesInfo.DATA_URI);
      }, true);
    }

    #region IData 成员

    #region Sequence

    public long[] GetSequenceValues(int count)
    {
      NetConfig.InitializeSwitch();
      do
      {
        try
        {
          return Service.GetSequenceValues(count);
        }
        catch (SocketException)
        {
          InvalidateCache();
          if (!NetConfig.SwitchServicesAddress())
            throw;
        }
      } while (true);
    }

    #endregion

    #region Schema

    public string GetTablesContent(string dataSourceKey, UserIdentity identity)
    {
      NetConfig.InitializeSwitch();
      do
      {
        try
        {
          return Service.GetTablesContent(dataSourceKey, identity);
        }
        catch (SocketException)
        {
          InvalidateCache();
          if (!NetConfig.SwitchServicesAddress())
            throw;
        }
      } while (true);
    }

    public string GetTableColumnsContent(string dataSourceKey, string tableName, UserIdentity identity)
    {
      NetConfig.InitializeSwitch();
      do
      {
        try
        {
          return Service.GetTableColumnsContent(dataSourceKey, tableName, identity);
        }
        catch (SocketException)
        {
          InvalidateCache();
          if (!NetConfig.SwitchServicesAddress())
            throw;
        }
      } while (true);
    }

    public string GetTablePrimaryKeysContent(string dataSourceKey, string tableName, UserIdentity identity)
    {
      NetConfig.InitializeSwitch();
      do
      {
        try
        {
          return Service.GetTablePrimaryKeysContent(dataSourceKey, tableName, identity);
        }
        catch (SocketException)
        {
          InvalidateCache();
          if (!NetConfig.SwitchServicesAddress())
            throw;
        }
      } while (true);
    }

    public string GetTableForeignKeysContent(string dataSourceKey, string tableName, UserIdentity identity)
    {
      NetConfig.InitializeSwitch();
      do
      {
        try
        {
          return Service.GetTableForeignKeysContent(dataSourceKey, tableName, identity);
        }
        catch (SocketException)
        {
          InvalidateCache();
          if (!NetConfig.SwitchServicesAddress())
            throw;
        }
      } while (true);
    }

    public string GetTableDetailForeignKeysContent(string dataSourceKey, string tableName, UserIdentity identity)
    {
      NetConfig.InitializeSwitch();
      do
      {
        try
        {
          return Service.GetTableDetailForeignKeysContent(dataSourceKey, tableName, identity);
        }
        catch (SocketException)
        {
          InvalidateCache();
          if (!NetConfig.SwitchServicesAddress())
            throw;
        }
      } while (true);
    }

    public string GetTableIndexesContent(string dataSourceKey, string tableName, UserIdentity identity)
    {
      NetConfig.InitializeSwitch();
      do
      {
        try
        {
          return Service.GetTableIndexesContent(dataSourceKey, tableName, identity);
        }
        catch (SocketException)
        {
          InvalidateCache();
          if (!NetConfig.SwitchServicesAddress())
            throw;
        }
      } while (true);
    }

    public string GetViewsContent(string dataSourceKey, UserIdentity identity)
    {
      NetConfig.InitializeSwitch();
      do
      {
        try
        {
          return Service.GetViewsContent(dataSourceKey, identity);
        }
        catch (SocketException)
        {
          InvalidateCache();
          if (!NetConfig.SwitchServicesAddress())
            throw;
        }
      } while (true);
    }

    public string GetViewColumnsContent(string dataSourceKey, string viewName, UserIdentity identity)
    {
      NetConfig.InitializeSwitch();
      do
      {
        try
        {
          return Service.GetViewColumnsContent(dataSourceKey, viewName, identity);
        }
        catch (SocketException)
        {
          InvalidateCache();
          if (!NetConfig.SwitchServicesAddress())
            throw;
        }
      } while (true);
    }

    #endregion

    #region Execute

    public IService Execute(IService service, UserIdentity identity)
    {
      NetConfig.InitializeSwitch();
      do
      {
        try
        {
          return GetService(service.GetType()).Execute(service, identity);
        }
        catch (SocketException)
        {
          InvalidateCache();
          if (!NetConfig.SwitchServicesAddress())
            throw;
        }
      } while (true);
    }

    public IService UploadFiles(IService service, IDictionary<string, Stream> fileStreams, UserIdentity identity)
    {
      Dictionary<string, byte[]> fileByteses = null;
      if (fileStreams != null)
      {
        fileByteses = new Dictionary<string, byte[]>(fileStreams.Count, StringComparer.OrdinalIgnoreCase);
        foreach (KeyValuePair<string, Stream> kvp in fileStreams)
        {
          if (kvp.Value == null)
            throw new InvalidOperationException("不允许fileStreams参数值内含空项");
          fileByteses.Add(kvp.Key, Phenix.Core.IO.StreamHelper.CopyBuffer(kvp.Value).ToArray());
        }
      }
      return UploadFiles(service, fileByteses, identity);
    }

    public IService UploadFiles(IService service, IDictionary<string, byte[]> fileByteses, UserIdentity identity)
    {
      NetConfig.InitializeSwitch();
      do
      {
        try
        {
          return GetService(service.GetType()).UploadFiles(service, fileByteses, identity);
        }
        catch (SocketException)
        {
          InvalidateCache();
          if (!NetConfig.SwitchServicesAddress())
            throw;
        }
      } while (true);
    }

    public IService UploadBigFile(IService service, FileChunkInfo fileChunkInfo, UserIdentity identity)
    {
      NetConfig.InitializeSwitch();
      do
      {
        try
        {
          return GetService(service.GetType()).UploadBigFile(service, fileChunkInfo, identity);
        }
        catch (SocketException)
        {
          InvalidateCache();
          if (!NetConfig.SwitchServicesAddress())
            throw;
        }
      } while (true);
    }

    public Stream DownloadFile(IService service, UserIdentity identity)
    {
      byte[] fileBytes = DownloadFileBytes(service, identity);
      return fileBytes != null ? new MemoryStream(fileBytes) : null;
    }

    public byte[] DownloadFileBytes(IService service, UserIdentity identity)
    {
      NetConfig.InitializeSwitch();
      do
      {
        try
        {
          return GetService(service.GetType()).DownloadFileBytes(service, identity);
        }
        catch (SocketException)
        {
          InvalidateCache();
          if (!NetConfig.SwitchServicesAddress())
            throw;
        }
      } while (true);
    }

    public FileChunkInfo DownloadBigFile(IService service, int chunkNumber, UserIdentity identity)
    {
      NetConfig.InitializeSwitch();
      do
      {
        try
        {
          return GetService(service.GetType()).DownloadBigFile(service, chunkNumber, identity);
        }
        catch (SocketException)
        {
          InvalidateCache();
          if (!NetConfig.SwitchServicesAddress())
            throw;
        }
      } while (true);
    }

    #endregion

    #region Fetch

    public object Fetch(ICriterions criterions, UserIdentity identity)
    {
      NetConfig.InitializeSwitch();
      do
      {
        try
        {
          return GetService(criterions.ResultType).Fetch(criterions, identity);
        }
        catch (SocketException)
        {
          InvalidateCache();
          if (!NetConfig.SwitchServicesAddress())
            throw;
        }
      } while (true);
    }

    public IList<object> FetchList(ICriterions criterions, UserIdentity identity)
    {
      NetConfig.InitializeSwitch();
      do
      {
        try
        {
          return GetService(criterions.ResultType).FetchList(criterions, identity);
        }
        catch (SocketException)
        {
          InvalidateCache();
          if (!NetConfig.SwitchServicesAddress())
            throw;
        }
      } while (true);
    }

    public string FetchContent(ICriterions criterions, UserIdentity identity)
    {
      NetConfig.InitializeSwitch();
      do
      {
        try
        {
          return GetService(criterions.ResultType).FetchContent(criterions, identity);
        }
        catch (SocketException)
        {
          InvalidateCache();
          if (!NetConfig.SwitchServicesAddress())
            throw;
        }
      } while (true);
    }

    #endregion

    #region Save

    public bool Save(IEntity entity, bool needCheckDirty, UserIdentity identity)
    {
      NetConfig.InitializeSwitch();
      do
      {
        try
        {
          return GetService(entity.GetType()).Save(entity, needCheckDirty, identity);
        }
        catch (SocketException)
        {
          InvalidateCache();
          if (!NetConfig.SwitchServicesAddress())
            throw;
        }
      } while (true);
    }

    public int Save(IEntityCollection entityCollection, UserIdentity identity)
    {
      NetConfig.InitializeSwitch();
      do
      {
        try
        {
          return GetService(entityCollection.GetType()).Save(entityCollection, identity);
        }
        catch (SocketException)
        {
          InvalidateCache();
          if (!NetConfig.SwitchServicesAddress())
            throw;
        }
      } while (true);
    }

    public int SaveContent(string objectTypeName, string source, UserIdentity identity)
    {
      NetConfig.InitializeSwitch();
      do
      {
        try
        {
          return GetService(Type.GetType(objectTypeName)).SaveContent(objectTypeName, source, identity);
        }
        catch (SocketException)
        {
          InvalidateCache();
          if (!NetConfig.SwitchServicesAddress())
            throw;
        }
      } while (true);
    }

    #endregion

    #region Insert

    public bool Insert(object obj, UserIdentity identity)
    {
      NetConfig.InitializeSwitch();
      do
      {
        try
        {
          return GetService(obj.GetType()).Insert(obj, identity);
        }
        catch (SocketException)
        {
          InvalidateCache();
          if (!NetConfig.SwitchServicesAddress())
            throw;
        }
      } while (true);
    }

    public int InsertList(IList<IEntity> entities, UserIdentity identity)
    {
      NetConfig.InitializeSwitch();
      do
      {
        try
        {
          return GetService(entities.GetType()).InsertList(entities, identity);
        }
        catch (SocketException)
        {
          InvalidateCache();
          if (!NetConfig.SwitchServicesAddress())
            throw;
        }
      } while (true);
    }

    #endregion

    #region Update

    public bool Update(object obj, IList<FieldValue> oldFieldValues, bool needCheckDirty, UserIdentity identity)
    {
      NetConfig.InitializeSwitch();
      do
      {
        try
        {
          return GetService(obj.GetType()).Update(obj, oldFieldValues, needCheckDirty, identity);
        }
        catch (SocketException)
        {
          InvalidateCache();
          if (!NetConfig.SwitchServicesAddress())
            throw;
        }
      } while (true);
    }

    public int UpdateList(IList<IEntity> entities, UserIdentity identity)
    {
      NetConfig.InitializeSwitch();
      do
      {
        try
        {
          return GetService(entities.GetType()).UpdateList(entities, identity);
        }
        catch (SocketException)
        {
          InvalidateCache();
          if (!NetConfig.SwitchServicesAddress())
            throw;
        }
      } while (true);
    }

    #endregion

    #region Delete

    public bool Delete(object obj, IList<FieldValue> oldFieldValues, bool needCheckDirty, UserIdentity identity)
    {
      NetConfig.InitializeSwitch();
      do
      {
        try
        {
          return GetService(obj.GetType()).Delete(obj, oldFieldValues, needCheckDirty, identity);
        }
        catch (SocketException)
        {
          InvalidateCache();
          if (!NetConfig.SwitchServicesAddress())
            throw;
        }
      } while (true);
    }

    public int DeleteList(IList<IEntity> entities, UserIdentity identity)
    {
      NetConfig.InitializeSwitch();
      do
      {
        try
        {
          return GetService(entities.GetType()).DeleteList(entities, identity);
        }
        catch (SocketException)
        {
          InvalidateCache();
          if (!NetConfig.SwitchServicesAddress())
            throw;
        }
      } while (true);
    }

    #endregion

    #region UpdateRecord

    public int UpdateRecord(ICriterions criterions, IDictionary<string, object> data, UserIdentity identity)
    {
      NetConfig.InitializeSwitch();
      do
      {
        try
        {
          return GetService(criterions.ResultType).UpdateRecord(criterions, data, identity);
        }
        catch (SocketException)
        {
          InvalidateCache();
          if (!NetConfig.SwitchServicesAddress())
            throw;
        }
      } while (true);
    }

    #endregion

    #region DeleteRecord

    public int DeleteRecord(ICriterions criterions, UserIdentity identity)
    {
      NetConfig.InitializeSwitch();
      do
      {
        try
        {
          return GetService(criterions.ResultType).DeleteRecord(criterions, identity);
        }
        catch (SocketException)
        {
          InvalidateCache();
          if (!NetConfig.SwitchServicesAddress())
            throw;
        }
      } while (true);
    }

    #endregion

    #region RecordCount

    public long GetRecordCount(ICriterions criterions, UserIdentity identity)
    {
      NetConfig.InitializeSwitch();
      do
      {
        try
        {
          return GetService(criterions.ResultType).GetRecordCount(criterions, identity);
        }
        catch (SocketException)
        {
          InvalidateCache();
          if (!NetConfig.SwitchServicesAddress())
            throw;
        }
      } while (true);
    }

    #endregion

    #region CheckRepeated

    public bool CheckRepeated(IEntity entity, UserIdentity identity)
    {
      NetConfig.InitializeSwitch();
      do
      {
        try
        {
          return GetService(entity.GetType()).CheckRepeated(entity, identity);
        }
        catch (SocketException)
        {
          InvalidateCache();
          if (!NetConfig.SwitchServicesAddress())
            throw;
        }
      } while (true);
    }

    public IList<IEntity> CheckRepeated(string rootTypeName, IList<IEntity> entities, UserIdentity identity)
    {
      NetConfig.InitializeSwitch();
      do
      {
        try
        {
          return GetService(Type.GetType(rootTypeName)).CheckRepeated(rootTypeName, entities, identity);
        }
        catch (SocketException)
        {
          InvalidateCache();
          if (!NetConfig.SwitchServicesAddress())
            throw;
        }
      } while (true);
    }

    #endregion

    #region BusinessCode

    public long GetBusinessCodeSerial(string key, long initialValue, UserIdentity identity)
    {
      NetConfig.InitializeSwitch();
      do
      {
        try
        {
          return Service.GetBusinessCodeSerial(key, initialValue, identity);
        }
        catch (SocketException)
        {
          InvalidateCache();
          if (!NetConfig.SwitchServicesAddress())
            throw;
        }
      } while (true);
    }
    
    public long[] GetBusinessCodeSerials(string key, long initialValue, int count, UserIdentity identity)
    {
      NetConfig.InitializeSwitch();
      do
      {
        try
        {
          return Service.GetBusinessCodeSerials(key, initialValue, count, identity);
        }
        catch (SocketException)
        {
          InvalidateCache();
          if (!NetConfig.SwitchServicesAddress())
            throw;
        }
      } while (true);
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

    #endregion
  }
}