using System;
using System.Collections.Generic;
using System.Data.Common;
using System.IO;
using System.Linq;
using System.Reflection;
using System.ServiceModel;
using Phenix.Core.Data;
using Phenix.Core.IO;
using Phenix.Core.Mapping;
using Phenix.Core.Net;
using Phenix.Core.Security;
using Phenix.Services.Contract;

namespace Phenix.Services.Client.Library.Wcf
{
  internal class DataProxy : IData
  {
    #region 属性

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
            ChannelFactory<Phenix.Services.Contract.Wcf.IData> channelFactory = GetChannelFactory();
            Phenix.Services.Contract.Wcf.IData channel = channelFactory.CreateChannel();
            object result = null;
            try
            {
              result = channel.GetSequenceValue();
              channelFactory.Close();
            }
            catch
            {
              channelFactory.Abort();
              throw;
            }
            Exception exception = result as Exception;
            if (exception != null)
              throw exception;
            return (long)result;
          }
          catch (EndpointNotFoundException)
          {
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

    private static ChannelFactory<Phenix.Services.Contract.Wcf.IData> GetChannelFactory()
    {
      return new ChannelFactory<Phenix.Services.Contract.Wcf.IData>(WcfHelper.CreateBinding(),
        new EndpointAddress(WcfHelper.CreateUrl(NetConfig.ServicesAddress, ServicesInfo.DATA_URI)));
    }

    private static ChannelFactory<Phenix.Services.Contract.Wcf.IData> GetChannelFactory(string host)
    {
      if (String.IsNullOrEmpty(host))
        return GetChannelFactory();
      return new ChannelFactory<Phenix.Services.Contract.Wcf.IData>(WcfHelper.CreateBinding(),
        new EndpointAddress(WcfHelper.CreateUrl(host, ServicesInfo.DATA_URI)));
    }

    private static ChannelFactory<Phenix.Services.Contract.Wcf.IData> GetChannelFactory(Type objectType)
    {
      if (NetConfig.ProxyType == ProxyType.Embedded)
        return GetChannelFactory();
      return GetChannelFactory(NetConfig.GetServicesClusterAddress(ServicesClusterAttribute.Fetch(objectType)));
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
          ChannelFactory<Phenix.Services.Contract.Wcf.IData> channelFactory = GetChannelFactory();
          Phenix.Services.Contract.Wcf.IData channel = channelFactory.CreateChannel();
          object result = null;
          try
          {
            result = channel.GetSequenceValues(count);
            channelFactory.Close();
          }
          catch
          {
            channelFactory.Abort();
            throw;
          }
          Exception exception = result as Exception;
          if (exception != null)
            throw exception;
          return (long[])result;
        }
        catch (EndpointNotFoundException)
        {
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
          ChannelFactory<Phenix.Services.Contract.Wcf.IData> channelFactory = GetChannelFactory();
          Phenix.Services.Contract.Wcf.IData channel = channelFactory.CreateChannel();
          object result = null;
          try
          {
            result = channel.GetTablesContent(dataSourceKey, identity);
            channelFactory.Close();
          }
          catch
          {
            channelFactory.Abort();
            throw;
          }
          Exception exception = result as Exception;
          if (exception != null)
            throw exception;
          return (string)result;
        }
        catch (EndpointNotFoundException)
        {
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
          ChannelFactory<Phenix.Services.Contract.Wcf.IData> channelFactory = GetChannelFactory();
          Phenix.Services.Contract.Wcf.IData channel = channelFactory.CreateChannel();
          object result = null;
          try
          {
            result = channel.GetTableColumnsContent(dataSourceKey, tableName, identity);
            channelFactory.Close();
          }
          catch
          {
            channelFactory.Abort();
            throw;
          }
          Exception exception = result as Exception;
          if (exception != null)
            throw exception;
          return (string)result;
        }
        catch (EndpointNotFoundException)
        {
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
          ChannelFactory<Phenix.Services.Contract.Wcf.IData> channelFactory = GetChannelFactory();
          Phenix.Services.Contract.Wcf.IData channel = channelFactory.CreateChannel();
          object result = null;
          try
          {
            result = channel.GetTablePrimaryKeysContent(dataSourceKey, tableName, identity);
            channelFactory.Close();
          }
          catch
          {
            channelFactory.Abort();
            throw;
          }
          Exception exception = result as Exception;
          if (exception != null)
            throw exception;
          return (string)result;
        }
        catch (EndpointNotFoundException)
        {
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
          ChannelFactory<Phenix.Services.Contract.Wcf.IData> channelFactory = GetChannelFactory();
          Phenix.Services.Contract.Wcf.IData channel = channelFactory.CreateChannel();
          object result = null;
          try
          {
            result = channel.GetTableForeignKeysContent(dataSourceKey, tableName, identity);
            channelFactory.Close();
          }
          catch
          {
            channelFactory.Abort();
            throw;
          }
          Exception exception = result as Exception;
          if (exception != null)
            throw exception;
          return (string)result;
        }
        catch (EndpointNotFoundException)
        {
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
          ChannelFactory<Phenix.Services.Contract.Wcf.IData> channelFactory = GetChannelFactory();
          Phenix.Services.Contract.Wcf.IData channel = channelFactory.CreateChannel();
          object result = null;
          try
          {
            result = channel.GetTableDetailForeignKeysContent(dataSourceKey, tableName, identity);
            channelFactory.Close();
          }
          catch
          {
            channelFactory.Abort();
            throw;
          }
          Exception exception = result as Exception;
          if (exception != null)
            throw exception;
          return (string)result;
        }
        catch (EndpointNotFoundException)
        {
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
          ChannelFactory<Phenix.Services.Contract.Wcf.IData> channelFactory = GetChannelFactory();
          Phenix.Services.Contract.Wcf.IData channel = channelFactory.CreateChannel();
          object result = null;
          try
          {
            result = channel.GetTableIndexesContent(dataSourceKey, tableName, identity);
            channelFactory.Close();
          }
          catch
          {
            channelFactory.Abort();
            throw;
          }
          Exception exception = result as Exception;
          if (exception != null)
            throw exception;
          return (string)result;
        }
        catch (EndpointNotFoundException)
        {
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
          ChannelFactory<Phenix.Services.Contract.Wcf.IData> channelFactory = GetChannelFactory();
          Phenix.Services.Contract.Wcf.IData channel = channelFactory.CreateChannel();
          object result = null;
          try
          {
            result = channel.GetViewsContent(dataSourceKey, identity);
            channelFactory.Close();
          }
          catch
          {
            channelFactory.Abort();
            throw;
          }
          Exception exception = result as Exception;
          if (exception != null)
            throw exception;
          return (string)result;
        }
        catch (EndpointNotFoundException)
        {
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
          ChannelFactory<Phenix.Services.Contract.Wcf.IData> channelFactory = GetChannelFactory();
          Phenix.Services.Contract.Wcf.IData channel = channelFactory.CreateChannel();
          object result = null;
          try
          {
            result = channel.GetViewColumnsContent(dataSourceKey, viewName, identity);
            channelFactory.Close();
          }
          catch
          {
            channelFactory.Abort();
            throw;
          }
          Exception exception = result as Exception;
          if (exception != null)
            throw exception;
          return (string)result;
        }
        catch (EndpointNotFoundException)
        {
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
          ChannelFactory<Phenix.Services.Contract.Wcf.IData> channelFactory = GetChannelFactory(service.GetType());
          Phenix.Services.Contract.Wcf.IData channel = channelFactory.CreateChannel();
          object result = null;
          try
          {
            result = channel.Execute(service, identity);
            channelFactory.Close();
          }
          catch
          {
            channelFactory.Abort();
            throw;
          }
          Exception exception = result as Exception;
          if (exception != null)
            throw exception;
          return (IService)result;
        }
        catch (EndpointNotFoundException)
        {
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
          ChannelFactory<Phenix.Services.Contract.Wcf.IData> channelFactory = GetChannelFactory(service.GetType());
          Phenix.Services.Contract.Wcf.IData channel = channelFactory.CreateChannel();
          object result = null;
          try
          {
            result = channel.UploadFiles(service, fileByteses, identity);
            channelFactory.Close();
          }
          catch
          {
            channelFactory.Abort();
            throw;
          }
          Exception exception = result as Exception;
          if (exception != null)
            throw exception;
          return (IService)result;
        }
        catch (EndpointNotFoundException)
        {
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
          ChannelFactory<Phenix.Services.Contract.Wcf.IData> channelFactory = GetChannelFactory(service.GetType());
          Phenix.Services.Contract.Wcf.IData channel = channelFactory.CreateChannel();
          object result = null;
          try
          {
            result = channel.UploadBigFile(service, fileChunkInfo, identity);
            channelFactory.Close();
          }
          catch
          {
            channelFactory.Abort();
            throw;
          }
          Exception exception = result as Exception;
          if (exception != null)
            throw exception;
          return (IService)result;
        }
        catch (EndpointNotFoundException)
        {
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
          ChannelFactory<Phenix.Services.Contract.Wcf.IData> channelFactory = GetChannelFactory(service.GetType());
          Phenix.Services.Contract.Wcf.IData channel = channelFactory.CreateChannel();
          object result = null;
          try
          {
            result = channel.DownloadFileBytes(service, identity);
            channelFactory.Close();
          }
          catch
          {
            channelFactory.Abort();
            throw;
          }
          Exception exception = result as Exception;
          if (exception != null)
            throw exception;
          return (byte[])result;
        }
        catch (EndpointNotFoundException)
        {
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
          ChannelFactory<Phenix.Services.Contract.Wcf.IData> channelFactory = GetChannelFactory(service.GetType());
          Phenix.Services.Contract.Wcf.IData channel = channelFactory.CreateChannel();
          object result = null;
          try
          {
            result = channel.DownloadBigFile(service, chunkNumber, identity);
            channelFactory.Close();
          }
          catch
          {
            channelFactory.Abort();
            throw;
          }
          Exception exception = result as Exception;
          if (exception != null)
            throw exception;
          return (FileChunkInfo)result;
        }
        catch (EndpointNotFoundException)
        {
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
          ChannelFactory<Phenix.Services.Contract.Wcf.IData> channelFactory = GetChannelFactory(criterions.ResultType);
          Phenix.Services.Contract.Wcf.IData channel = channelFactory.CreateChannel();
          object result = null;
          try
          {
            result = channel.Fetch(criterions, identity);
            channelFactory.Close();
          }
          catch
          {
            channelFactory.Abort();
            throw;
          }
          Exception exception = result as Exception;
          if (exception != null)
            throw exception;
          return result;
        }
        catch (EndpointNotFoundException)
        {
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
          ChannelFactory<Phenix.Services.Contract.Wcf.IData> channelFactory = GetChannelFactory(criterions.ResultType);
          Phenix.Services.Contract.Wcf.IData channel = channelFactory.CreateChannel();
          object result = null;
          try
          {
            result = channel.FetchList(criterions, identity);
            channelFactory.Close();
          }
          catch
          {
            channelFactory.Abort();
            throw;
          }
          Exception exception = result as Exception;
          if (exception != null)
            throw exception;
          return (IList<object>)result;
        }
        catch (EndpointNotFoundException)
        {
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
          ChannelFactory<Phenix.Services.Contract.Wcf.IData> channelFactory = GetChannelFactory(criterions.ResultType);
          Phenix.Services.Contract.Wcf.IData channel = channelFactory.CreateChannel();
          object result = null;
          try
          {
            result = channel.FetchContent(criterions, identity);
            channelFactory.Close();
          }
          catch
          {
            channelFactory.Abort();
            throw;
          }
          Exception exception = result as Exception;
          if (exception != null)
            throw exception;
          return (string)result;
        }
        catch (EndpointNotFoundException)
        {
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
          ChannelFactory<Phenix.Services.Contract.Wcf.IData> channelFactory = GetChannelFactory(entity.GetType());
          Phenix.Services.Contract.Wcf.IData channel = channelFactory.CreateChannel();
          object result = null;
          try
          {
            result = channel.Save(entity, needCheckDirty, identity);
            channelFactory.Close();
          }
          catch
          {
            channelFactory.Abort();
            throw;
          }
          Exception exception = result as Exception;
          if (exception != null)
            throw exception;
          return (bool)result;
        }
        catch (EndpointNotFoundException)
        {
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
          ChannelFactory<Phenix.Services.Contract.Wcf.IData> channelFactory = GetChannelFactory(entityCollection.GetType());
          Phenix.Services.Contract.Wcf.IData channel = channelFactory.CreateChannel();
          object result = null;
          try
          {
            result = channel.SaveEntityCollection(entityCollection, identity);
            channelFactory.Close();
          }
          catch
          {
            channelFactory.Abort();
            throw;
          }
          Exception exception = result as Exception;
          if (exception != null)
            throw exception;
          return (int)result;
        }
        catch (EndpointNotFoundException)
        {
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
          ChannelFactory<Phenix.Services.Contract.Wcf.IData> channelFactory = GetChannelFactory(Type.GetType(objectTypeName));
          Phenix.Services.Contract.Wcf.IData channel = channelFactory.CreateChannel();
          object result = null;
          try
          {
            result = channel.SaveContent(objectTypeName, source, identity);
            channelFactory.Close();
          }
          catch
          {
            channelFactory.Abort();
            throw;
          }
          Exception exception = result as Exception;
          if (exception != null)
            throw exception;
          return (int)result;
        }
        catch (EndpointNotFoundException)
        {
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
          ChannelFactory<Phenix.Services.Contract.Wcf.IData> channelFactory = GetChannelFactory(obj.GetType());
          Phenix.Services.Contract.Wcf.IData channel = channelFactory.CreateChannel();
          object result = null;
          try
          {
            result = channel.Insert(obj, identity);
            channelFactory.Close();
          }
          catch
          {
            channelFactory.Abort();
            throw;
          }
          Exception exception = result as Exception;
          if (exception != null)
            throw exception;
          return (bool)result;
        }
        catch (EndpointNotFoundException)
        {
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
          ChannelFactory<Phenix.Services.Contract.Wcf.IData> channelFactory = GetChannelFactory(entities.GetType());
          Phenix.Services.Contract.Wcf.IData channel = channelFactory.CreateChannel();
          object result = null;
          try
          {
            result = channel.InsertList(entities, identity);
            channelFactory.Close();
          }
          catch
          {
            channelFactory.Abort();
            throw;
          }
          Exception exception = result as Exception;
          if (exception != null)
            throw exception;
          return (int)result;
        }
        catch (EndpointNotFoundException)
        {
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
          ChannelFactory<Phenix.Services.Contract.Wcf.IData> channelFactory = GetChannelFactory(obj.GetType());
          Phenix.Services.Contract.Wcf.IData channel = channelFactory.CreateChannel();
          object result = null;
          try
          {
            result = channel.Update(obj, oldFieldValues, needCheckDirty, identity);
            channelFactory.Close();
          }
          catch
          {
            channelFactory.Abort();
            throw;
          }
          Exception exception = result as Exception;
          if (exception != null)
            throw exception;
          return (bool)result;
        }
        catch (EndpointNotFoundException)
        {
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
          ChannelFactory<Phenix.Services.Contract.Wcf.IData> channelFactory = GetChannelFactory(entities.GetType());
          Phenix.Services.Contract.Wcf.IData channel = channelFactory.CreateChannel();
          object result = null;
          try
          {
            result = channel.UpdateList(entities, identity);
            channelFactory.Close();
          }
          catch
          {
            channelFactory.Abort();
            throw;
          }
          Exception exception = result as Exception;
          if (exception != null)
            throw exception;
          return (int)result;
        }
        catch (EndpointNotFoundException)
        {
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
          ChannelFactory<Phenix.Services.Contract.Wcf.IData> channelFactory = GetChannelFactory(obj.GetType());
          Phenix.Services.Contract.Wcf.IData channel = channelFactory.CreateChannel();
          object result = null;
          try
          {
            result = channel.Delete(obj, oldFieldValues, needCheckDirty, identity);
            channelFactory.Close();
          }
          catch
          {
            channelFactory.Abort();
            throw;
          }
          Exception exception = result as Exception;
          if (exception != null)
            throw exception;
          return (bool)result;
        }
        catch (EndpointNotFoundException)
        {
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
          ChannelFactory<Phenix.Services.Contract.Wcf.IData> channelFactory = GetChannelFactory(entities.GetType());
          Phenix.Services.Contract.Wcf.IData channel = channelFactory.CreateChannel();
          object result = null;
          try
          {
            result = channel.DeleteList(entities, identity);
            channelFactory.Close();
          }
          catch
          {
            channelFactory.Abort();
            throw;
          }
          Exception exception = result as Exception;
          if (exception != null)
            throw exception;
          return (int)result;
        }
        catch (EndpointNotFoundException)
        {
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
          ChannelFactory<Phenix.Services.Contract.Wcf.IData> channelFactory = GetChannelFactory(criterions.ResultType);
          Phenix.Services.Contract.Wcf.IData channel = channelFactory.CreateChannel();
          object result = null;
          try
          {
            result = channel.UpdateRecord(criterions, data, identity);
            channelFactory.Close();
          }
          catch
          {
            channelFactory.Abort();
            throw;
          }
          Exception exception = result as Exception;
          if (exception != null)
            throw exception;
          return (int)result;
        }
        catch (EndpointNotFoundException)
        {
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
          ChannelFactory<Phenix.Services.Contract.Wcf.IData> channelFactory = GetChannelFactory(criterions.ResultType);
          Phenix.Services.Contract.Wcf.IData channel = channelFactory.CreateChannel();
          object result = null;
          try
          {
            result = channel.DeleteRecord(criterions, identity);
            channelFactory.Close();
          }
          catch
          {
            channelFactory.Abort();
            throw;
          }
          Exception exception = result as Exception;
          if (exception != null)
            throw exception;
          return (int)result;
        }
        catch (EndpointNotFoundException)
        {
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
          ChannelFactory<Phenix.Services.Contract.Wcf.IData> channelFactory = GetChannelFactory(criterions.ResultType);
          Phenix.Services.Contract.Wcf.IData channel = channelFactory.CreateChannel();
          object result = null;
          try
          {
            result = channel.GetRecordCount(criterions, identity);
            channelFactory.Close();
          }
          catch
          {
            channelFactory.Abort();
            throw;
          }
          Exception exception = result as Exception;
          if (exception != null)
            throw exception;
          return (long)result;
        }
        catch (EndpointNotFoundException)
        {
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
          ChannelFactory<Phenix.Services.Contract.Wcf.IData> channelFactory = GetChannelFactory(entity.GetType());
          Phenix.Services.Contract.Wcf.IData channel = channelFactory.CreateChannel();
          object result = null;
          try
          {
            result = channel.CheckRepeated(entity, identity);
            channelFactory.Close();
          }
          catch
          {
            channelFactory.Abort();
            throw;
          }
          Exception exception = result as Exception;
          if (exception != null)
            throw exception;
          return (bool)result;
        }
        catch (EndpointNotFoundException)
        {
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
          ChannelFactory<Phenix.Services.Contract.Wcf.IData> channelFactory = GetChannelFactory(Type.GetType(rootTypeName));
          Phenix.Services.Contract.Wcf.IData channel = channelFactory.CreateChannel();
          object result = null;
          try
          {
            result = channel.CheckEntitiesRepeated(rootTypeName, entities, identity);
            channelFactory.Close();
          }
          catch
          {
            channelFactory.Abort();
            throw;
          }
          Exception exception = result as Exception;
          if (exception != null)
            throw exception;
          return (IList<IEntity>)result;
        }
        catch (EndpointNotFoundException)
        {
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
          ChannelFactory<Phenix.Services.Contract.Wcf.IData> channelFactory = GetChannelFactory();
          Phenix.Services.Contract.Wcf.IData channel = channelFactory.CreateChannel();
          object result = null;
          try
          {
            result = channel.GetBusinessCodeSerial(key, initialValue, identity);
            channelFactory.Close();
          }
          catch
          {
            channelFactory.Abort();
            throw;
          }
          Exception exception = result as Exception;
          if (exception != null)
            throw exception;
          return (long)result;
        }
        catch (EndpointNotFoundException)
        {
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
          ChannelFactory<Phenix.Services.Contract.Wcf.IData> channelFactory = GetChannelFactory();
          Phenix.Services.Contract.Wcf.IData channel = channelFactory.CreateChannel();
          object result = null;
          try
          {
            result = channel.GetBusinessCodeSerials(key, initialValue, count, identity);
            channelFactory.Close();
          }
          catch
          {
            channelFactory.Abort();
            throw;
          }
          Exception exception = result as Exception;
          if (exception != null)
            throw exception;
          return (long[])result;
        }
        catch (EndpointNotFoundException)
        {
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