using System;
using System.Collections.Generic;
using System.ServiceModel;
using Phenix.Core.IO;
using Phenix.Core.Mapping;
using Phenix.Core.Security;

namespace Phenix.Services.Contract.Wcf
{
  [ServiceContract]
  public interface IData
  {
    #region 方法
    
    #region Schema

    [OperationContract]
    [UseNetDataContract]
    object GetTablesContent(string dataSourceKey, UserIdentity identity);

    [OperationContract]
    [UseNetDataContract]
    object GetTableColumnsContent(string dataSourceKey, string tableName, UserIdentity identity);

    [OperationContract]
    [UseNetDataContract]
    object GetTablePrimaryKeysContent(string dataSourceKey, string tableName, UserIdentity identity);

    [OperationContract]
    [UseNetDataContract]
    object GetTableForeignKeysContent(string dataSourceKey, string tableName, UserIdentity identity);

    [OperationContract]
    [UseNetDataContract]
    object GetTableDetailForeignKeysContent(string dataSourceKey, string tableName, UserIdentity identity);

    [OperationContract]
    [UseNetDataContract]
    object GetTableIndexesContent(string dataSourceKey, string tableName, UserIdentity identity);

    [OperationContract]
    [UseNetDataContract]
    object GetViewsContent(string dataSourceKey, UserIdentity identity);

    [OperationContract]
    [UseNetDataContract]
    object GetViewColumnsContent(string dataSourceKey, string viewName, UserIdentity identity);

    #endregion

    #region Execute

    [OperationContract]
    [UseNetDataContract]
    object Execute(IService service, UserIdentity identity);

    [OperationContract]
    [UseNetDataContract]
    object UploadFiles(IService service, IDictionary<string, byte[]> fileByteses, UserIdentity identity);

    [OperationContract]
    [UseNetDataContract]
    object UploadBigFile(IService service, FileChunkInfo fileChunkInfo, UserIdentity identity);

    [OperationContract]
    [UseNetDataContract]
    object DownloadFileBytes(IService service, UserIdentity identity);

    [OperationContract]
    [UseNetDataContract]
    object DownloadBigFile(IService service, int chunkNumber, UserIdentity identity);

    #endregion

    #region Fetch

    [OperationContract]
    [UseNetDataContract]
    object Fetch(ICriterions criterions, UserIdentity identity);

    [OperationContract]
    [UseNetDataContract]
    object FetchList(ICriterions criterions, UserIdentity identity);

    [OperationContract]
    [UseNetDataContract]
    object FetchContent(ICriterions criterions, UserIdentity identity);

    #endregion

    #region Save

    [OperationContract]
    [UseNetDataContract]
    object Save(IEntity entity, bool needCheckDirty, UserIdentity identity);

    [OperationContract]
    [UseNetDataContract]
    object SaveEntityCollection(IEntityCollection entityCollection, UserIdentity identity);

    [OperationContract]
    [UseNetDataContract]
    object SaveContent(string objectTypeName, string source, UserIdentity identity);

    #endregion

    #region Insert

    [OperationContract]
    [UseNetDataContract]
    object Insert(object obj, UserIdentity identity);

    [OperationContract]
    [UseNetDataContract]
    object InsertList(IList<IEntity> entities, UserIdentity identity);

    #endregion

    #region Update

    [OperationContract]
    [UseNetDataContract]
    object Update(object obj, IList<FieldValue> oldFieldValues, bool needCheckDirty, UserIdentity identity);

    [OperationContract]
    [UseNetDataContract]
    object UpdateList(IList<IEntity> entities, UserIdentity identity);

    #endregion

    #region Delete

    [OperationContract]
    [UseNetDataContract]
    object Delete(object obj, IList<FieldValue> oldFieldValues, bool needCheckDirty, UserIdentity identity);

    [OperationContract]
    [UseNetDataContract]
    object DeleteList(IList<IEntity> entities, UserIdentity identity);

    #endregion

    #region UpdateRecord

    [OperationContract]
    [UseNetDataContract]
    object UpdateRecord(ICriterions criterions, IDictionary<string, object> data, UserIdentity identity);

    #endregion

    #region DeleteRecord

    [OperationContract]
    [UseNetDataContract]
    object DeleteRecord(ICriterions criterions, UserIdentity identity);

    #endregion

    #region RecordCount

    [OperationContract]
    [UseNetDataContract]
    object GetRecordCount(ICriterions criterions, UserIdentity identity);

    #endregion

    #region Sequence

    [OperationContract]
    [UseNetDataContract]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate")]
    object GetSequenceValue();

    [OperationContract]
    [UseNetDataContract]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate")]
    object GetSequenceValues(int count);

    #endregion

    #region CheckRepeated

    [OperationContract]
    [UseNetDataContract]
    object CheckRepeated(IEntity entity, UserIdentity identity);

    [OperationContract]
    [UseNetDataContract]
    object CheckEntitiesRepeated(string rootTypeName, IList<IEntity> entities, UserIdentity identity);

    #endregion

    #region BusinessCode

    [OperationContract]
    [UseNetDataContract]
    object GetBusinessCodeSerial(string key, long initialValue, UserIdentity identity);

    [OperationContract]
    [UseNetDataContract]
    object GetBusinessCodeSerials(string key, long initialValue, int count, UserIdentity identity);

    #endregion
    
    #endregion
  }
}
