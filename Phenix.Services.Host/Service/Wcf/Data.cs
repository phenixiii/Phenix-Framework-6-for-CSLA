using System;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;
using System.ServiceModel;
using System.ServiceModel.Activation;
using Phenix.Core;
using Phenix.Core.Data;
using Phenix.Core.IO;
using Phenix.Core.Log;
using Phenix.Core.Mapping;
using Phenix.Core.Security;
using Phenix.Services.Host.Core;

namespace Phenix.Services.Host.Service.Wcf
{
  [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerCall)]
  [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
  public sealed class Data : Phenix.Services.Contract.Wcf.IData
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

    #region 方法

    #region Sequence

    [OperationBehavior(Impersonation = ImpersonationOption.Allowed)]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
    public object GetSequenceValue()
    {
      try
      {
        ServiceManager.CheckActive();
        return DataHub.SequenceValue;
      }
      catch (Exception ex)
      {
        return ex;
      }
    }

    [OperationBehavior(Impersonation = ImpersonationOption.Allowed)]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
    public object GetSequenceValues(int count)
    {
      try
      {
        ServiceManager.CheckActive();
        return DataHub.GetSequenceValues(count);
      }
      catch (Exception ex)
      {
        return ex;
      }
    }

    #endregion

    #region Schema

    [OperationBehavior(Impersonation = ImpersonationOption.Allowed)]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
    public object GetTablesContent(string dataSourceKey, UserIdentity identity)
    {
      try
      {
        ServiceManager.CheckIn(identity);
        DataSecurityContext context = DataSecurityHub.CheckIn(identity, false);
        return DataHub.GetTablesContent(dataSourceKey, context.Identity);
      }
      catch (Exception ex)
      {
        return ex;
      }
    }
    
    [OperationBehavior(Impersonation = ImpersonationOption.Allowed)]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
    public object GetTableColumnsContent(string dataSourceKey, string tableName, UserIdentity identity)
    {
      try
      {
        ServiceManager.CheckIn(identity);
        DataSecurityContext context = DataSecurityHub.CheckIn(identity, false);
        return DataHub.GetTableColumnsContent(dataSourceKey, tableName, context.Identity);
      }
      catch (Exception ex)
      {
        return ex;
      }
    }
    
    [OperationBehavior(Impersonation = ImpersonationOption.Allowed)]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
    public object GetTablePrimaryKeysContent(string dataSourceKey, string tableName, UserIdentity identity)
    {
      try
      {
        ServiceManager.CheckIn(identity);
        DataSecurityContext context = DataSecurityHub.CheckIn(identity, false);
        return DataHub.GetTablePrimaryKeysContent(dataSourceKey, tableName, context.Identity);
      }
      catch (Exception ex)
      {
        return ex;
      }
    }

    [OperationBehavior(Impersonation = ImpersonationOption.Allowed)]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
    public object GetTableForeignKeysContent(string dataSourceKey, string tableName, UserIdentity identity)
    {
      try
      {
        ServiceManager.CheckIn(identity);
        DataSecurityContext context = DataSecurityHub.CheckIn(identity, false);
        return DataHub.GetTableForeignKeysContent(dataSourceKey, tableName, context.Identity);
      }
      catch (Exception ex)
      {
        return ex;
      }
    }

    [OperationBehavior(Impersonation = ImpersonationOption.Allowed)]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
    public object GetTableDetailForeignKeysContent(string dataSourceKey, string tableName, UserIdentity identity)
    {
      try
      {
        ServiceManager.CheckIn(identity);
        DataSecurityContext context = DataSecurityHub.CheckIn(identity, false);
        return DataHub.GetTableDetailForeignKeysContent(dataSourceKey, tableName, context.Identity);
      }
      catch (Exception ex)
      {
        return ex;
      }
    }

    [OperationBehavior(Impersonation = ImpersonationOption.Allowed)]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
    public object GetTableIndexesContent(string dataSourceKey, string tableName, UserIdentity identity)
    {
      try
      {
        ServiceManager.CheckIn(identity);
        DataSecurityContext context = DataSecurityHub.CheckIn(identity, false);
        return DataHub.GetTableIndexesContent(dataSourceKey, tableName, context.Identity);
      }
      catch (Exception ex)
      {
        return ex;
      }
    }

    [OperationBehavior(Impersonation = ImpersonationOption.Allowed)]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
    public object GetViewsContent(string dataSourceKey, UserIdentity identity)
    {
      try
      {
        ServiceManager.CheckIn(identity);
        DataSecurityContext context = DataSecurityHub.CheckIn(identity, false);
        return DataHub.GetViewsContent(dataSourceKey, context.Identity);
      }
      catch (Exception ex)
      {
        return ex;
      }
    }

    [OperationBehavior(Impersonation = ImpersonationOption.Allowed)]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
    public object GetViewColumnsContent(string dataSourceKey, string viewName, UserIdentity identity)
    {
      try
      {
        ServiceManager.CheckIn(identity);
        DataSecurityContext context = DataSecurityHub.CheckIn(identity, false);
        return DataHub.GetViewColumnsContent(dataSourceKey, viewName, context.Identity);
      }
      catch (Exception ex)
      {
        return ex;
      }
    }

    #endregion

    #region Execute

    [OperationBehavior(Impersonation = ImpersonationOption.Allowed)]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
    public object Execute(IService service, UserIdentity identity)
    {
      try
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
      catch (Exception ex)
      {
        return ex;
      }
    }

    [OperationBehavior(Impersonation = ImpersonationOption.Allowed)]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
    public object UploadFiles(IService service, IDictionary<string, byte[]> fileByteses, UserIdentity identity)
    {
      try
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
      catch (Exception ex)
      {
        return ex;
      }
    }

    [OperationBehavior(Impersonation = ImpersonationOption.Allowed)]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
    public object UploadBigFile(IService service, FileChunkInfo fileChunkInfo, UserIdentity identity)
    {
      try
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
      catch (Exception ex)
      {
        return ex;
      }
    }

    [OperationBehavior(Impersonation = ImpersonationOption.Allowed)]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
    public object DownloadFileBytes(IService service, UserIdentity identity)
    {
      try
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
      catch (Exception ex)
      {
        return ex;
      }
    }

    [OperationBehavior(Impersonation = ImpersonationOption.Allowed)]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
    public object DownloadBigFile(IService service, int chunkNumber, UserIdentity identity)
    {
      try
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
      catch (Exception ex)
      {
        return ex;
      }
    }

    #endregion

    #region Fetch

    [OperationBehavior(Impersonation = ImpersonationOption.Allowed)]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
    public object Fetch(ICriterions criterions, UserIdentity identity)
    {
      try
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
      catch (Exception ex)
      {
        return ex;
      }
    }

    [OperationBehavior(Impersonation = ImpersonationOption.Allowed)]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
    public object FetchList(ICriterions criterions, UserIdentity identity)
    {
      try
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
      catch (Exception ex)
      {
        return ex;
      }
    }

    [OperationBehavior(Impersonation = ImpersonationOption.Allowed)]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
    public object FetchContent(ICriterions criterions, UserIdentity identity)
    {
      try
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
      catch (Exception ex)
      {
        return ex;
      }
    }

    #endregion

    #region Save

    [OperationBehavior(Impersonation = ImpersonationOption.Allowed)]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
    public object Save(IEntity entity, bool needCheckDirty, UserIdentity identity)
    {
      try
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
      catch (Exception ex)
      {
        return ex;
      }
    }

    [OperationBehavior(Impersonation = ImpersonationOption.Allowed)]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
    public object SaveEntityCollection(IEntityCollection entityCollection, UserIdentity identity)
    {
      try
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
      catch (Exception ex)
      {
        return ex;
      }
    }

    [OperationBehavior(Impersonation = ImpersonationOption.Allowed)]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
    public object SaveContent(string objectTypeName, string source, UserIdentity identity)
    {
      try
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
      catch (Exception ex)
      {
        return ex;
      }
    }

    #endregion

    #region Insert

    [OperationBehavior(Impersonation = ImpersonationOption.Allowed)]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
    public object Insert(object obj, UserIdentity identity)
    {
      try
      {
        ServiceManager.CheckIn(identity);
        DataSecurityContext context = DataSecurityHub.CheckIn(obj.GetType(), ExecuteAction.Insert, identity);
        return DataHub.Insert(obj, context.Identity);
      }
      catch (Exception ex)
      {
        return ex;
      }
    }

    [OperationBehavior(Impersonation = ImpersonationOption.Allowed)]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
    public object InsertList(IList<IEntity> entities, UserIdentity identity)
    {
      try
      {
        ServiceManager.CheckIn(identity);
        DataSecurityContext context = DataSecurityHub.CheckIn(entities.GetType(), ExecuteAction.Insert, identity);
        return DataHub.InsertList(entities, context.Identity);
      }
      catch (Exception ex)
      {
        return ex;
      }
    }

    #endregion

    #region Update

    [OperationBehavior(Impersonation = ImpersonationOption.Allowed)]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
    public object Update(object obj, IList<FieldValue> oldFieldValues, bool needCheckDirty, UserIdentity identity)
    {
      try
      {
        ServiceManager.CheckIn(identity);
        DataSecurityContext context = DataSecurityHub.CheckIn(obj.GetType(), ExecuteAction.Update, identity);
        return DataHub.Update(obj, oldFieldValues, needCheckDirty, context.Identity);
      }
      catch (Exception ex)
      {
        return ex;
      }
    }

    [OperationBehavior(Impersonation = ImpersonationOption.Allowed)]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
    public object UpdateList(IList<IEntity> entities, UserIdentity identity)
    {
      try
      {
        ServiceManager.CheckIn(identity);
        DataSecurityContext context = DataSecurityHub.CheckIn(entities.GetType(), ExecuteAction.Update, identity);
        return DataHub.UpdateList(entities, context.Identity);
      }
      catch (Exception ex)
      {
        return ex;
      }
    }

    #endregion

    #region Delete

    [OperationBehavior(Impersonation = ImpersonationOption.Allowed)]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
    public object Delete(object obj, IList<FieldValue> oldFieldValues, bool needCheckDirty, UserIdentity identity)
    {
      try
      {
        ServiceManager.CheckIn(identity);
        DataSecurityContext context = DataSecurityHub.CheckIn(obj.GetType(), ExecuteAction.Delete, identity);
        return DataHub.Delete(obj, oldFieldValues, needCheckDirty, context.Identity);
      }
      catch (Exception ex)
      {
        return ex;
      }
    }

    [OperationBehavior(Impersonation = ImpersonationOption.Allowed)]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
    public object DeleteList(IList<IEntity> entities, UserIdentity identity)
    {
      try
      {
        ServiceManager.CheckIn(identity);
        DataSecurityContext context = DataSecurityHub.CheckIn(entities.GetType(), ExecuteAction.Delete, identity);
        return DataHub.DeleteList(entities, context.Identity);
      }
      catch (Exception ex)
      {
        return ex;
      }
    }

    #endregion

    #region UpdateRecord

    [OperationBehavior(Impersonation = ImpersonationOption.Allowed)]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
    public object UpdateRecord(ICriterions criterions, IDictionary<string, object> data, UserIdentity identity)
    {
      try
      {
        ServiceManager.CheckIn(identity);
        DataSecurityContext context = DataSecurityHub.CheckIn(criterions.ResultType, ExecuteAction.Update, identity);
        return DataHub.UpdateRecord(criterions, data, context.Identity);
      }
      catch (Exception ex)
      {
        return ex;
      }
    }

    #endregion

    #region DeleteRecord

    [OperationBehavior(Impersonation = ImpersonationOption.Allowed)]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
    public object DeleteRecord(ICriterions criterions, UserIdentity identity)
    {
      try
      {
        ServiceManager.CheckIn(identity);
        DataSecurityContext context = DataSecurityHub.CheckIn(criterions.ResultType, ExecuteAction.Delete, identity);
        return DataHub.DeleteRecord(criterions, context.Identity);
      }
      catch (Exception ex)
      {
        return ex;
      }
    }

    #endregion

    #region RecordCount

    [OperationBehavior(Impersonation = ImpersonationOption.Allowed)]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
    public object GetRecordCount(ICriterions criterions, UserIdentity identity)
    {
      try
      {
        ServiceManager.CheckIn(identity);
        DataSecurityContext context = DataSecurityHub.CheckIn(criterions.ResultType, ExecuteAction.Fetch, identity);
        return DataHub.GetRecordCount(criterions, context.Identity);
      }
      catch (Exception ex)
      {
        return ex;
      }
    }

    #endregion

    #region CheckRepeated

    [OperationBehavior(Impersonation = ImpersonationOption.Allowed)]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
    public object CheckRepeated(IEntity entity, UserIdentity identity)
    {
      try
      {
        ServiceManager.CheckIn(identity);
        DataSecurityContext context = DataSecurityHub.CheckIn(identity, false);
        return DataHub.CheckRepeated(entity, context.Identity);
      }
      catch (Exception ex)
      {
        return ex;
      }
    }

    [OperationBehavior(Impersonation = ImpersonationOption.Allowed)]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
    public object CheckEntitiesRepeated(string rootTypeName, IList<IEntity> entities, UserIdentity identity)
    {
      try
      {
        ServiceManager.CheckIn(identity);
        DataSecurityContext context = DataSecurityHub.CheckIn(identity, false);
        return DataHub.CheckRepeated(rootTypeName, entities, context.Identity);
      }
      catch (Exception ex)
      {
        return ex;
      }
    }

    #endregion

    #region BusinessCode

    [OperationBehavior(Impersonation = ImpersonationOption.Allowed)]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
    public object GetBusinessCodeSerial(string key, long initialValue, UserIdentity identity)
    {
      try
      {
        ServiceManager.CheckIn(identity);
        DataSecurityContext context = DataSecurityHub.CheckIn(identity, false);
        return DataHub.GetBusinessCodeSerial(key, initialValue, context.Identity);
      }
      catch (Exception ex)
      {
        return ex;
      }
    }

    [OperationBehavior(Impersonation = ImpersonationOption.Allowed)]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
    public object GetBusinessCodeSerials(string key, long initialValue, int count, UserIdentity identity)
    {
      try
      {
        ServiceManager.CheckIn(identity);
        DataSecurityContext context = DataSecurityHub.CheckIn(identity, false);
        return DataHub.GetBusinessCodeSerials(key, initialValue, count, context.Identity);
      }
      catch (Exception ex)
      {
        return ex;
      }
    }

    #endregion

    #endregion
  }
}