using System;
using System.Collections.Generic;
using System.ServiceModel;
using Phenix.Core.Dictionary;
using Phenix.Core.Mapping;
using Phenix.Core.Net;
using Phenix.Core.Security;
using Phenix.Services.Contract;

namespace Phenix.Services.Client.Library.Wcf
{
  internal class DataDictionaryProxy : IDataDictionary
  {
    #region 属性

    #region IDataDictionary 成员

    public string Enterprise
    {
      get
      {
        NetConfig.InitializeSwitch();
        do
        {
          try
          {
            ChannelFactory<Phenix.Services.Contract.Wcf.IDataDictionary> channelFactory = GetChannelFactory();
            Phenix.Services.Contract.Wcf.IDataDictionary channel = channelFactory.CreateChannel();
            object result = null;
            try
            {
              result = channel.GetEnterprise();
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
    }

    public IDictionary<long, DepartmentInfo> DepartmentInfos
    {
      get
      {
        NetConfig.InitializeSwitch();
        do
        {
          try
          {
            ChannelFactory<Phenix.Services.Contract.Wcf.IDataDictionary> channelFactory = GetChannelFactory();
            Phenix.Services.Contract.Wcf.IDataDictionary channel = channelFactory.CreateChannel();
            object result = null;
            try
            {
              result = channel.GetDepartmentInfos();
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
            return (IDictionary<long, DepartmentInfo>)result;
          }
          catch (EndpointNotFoundException)
          {
            if (!NetConfig.SwitchServicesAddress())
              throw;
          }
        } while (true);
      }
    }

    public IDictionary<long, PositionInfo> PositionInfos
    {
      get
      {
        NetConfig.InitializeSwitch();
        do
        {
          try
          {
            ChannelFactory<Phenix.Services.Contract.Wcf.IDataDictionary> channelFactory = GetChannelFactory();
            Phenix.Services.Contract.Wcf.IDataDictionary channel = channelFactory.CreateChannel();
            object result = null;
            try
            {
              result = channel.GetPositionInfos();
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
            return (IDictionary<long, PositionInfo>)result;
          }
          catch (EndpointNotFoundException)
          {
            if (!NetConfig.SwitchServicesAddress())
              throw;
          }
        } while (true);
      }
    }
    
    public IDictionary<string, TableFilterInfo> TableFilterInfos
    {
      get
      {
        NetConfig.InitializeSwitch();
        do
        {
          try
          {
            ChannelFactory<Phenix.Services.Contract.Wcf.IDataDictionary> channelFactory = GetChannelFactory();
            Phenix.Services.Contract.Wcf.IDataDictionary channel = channelFactory.CreateChannel();
            object result = null;
            try
            {
              result = channel.GetTableFilterInfos();
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
            return (IDictionary<string, TableFilterInfo>)result;
          }
          catch (EndpointNotFoundException)
          {
            if (!NetConfig.SwitchServicesAddress())
              throw;
          }
        } while (true);
      }
    }

    public IDictionary<string, RoleInfo> RoleInfos
    {
      get
      {
        NetConfig.InitializeSwitch();
        do
        {
          try
          {
            ChannelFactory<Phenix.Services.Contract.Wcf.IDataDictionary> channelFactory = GetChannelFactory();
            Phenix.Services.Contract.Wcf.IDataDictionary channel = channelFactory.CreateChannel();
            object result = null;
            try
            {
              result = channel.GetRoleInfos();
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
            return (IDictionary<string, RoleInfo>)result;
          }
          catch (EndpointNotFoundException)
          {
            if (!NetConfig.SwitchServicesAddress())
              throw;
          }
        } while (true);
      }
    }

    public IDictionary<string, SectionInfo> SectionInfos
    {
      get
      {
        NetConfig.InitializeSwitch();
        do
        {
          try
          {
            ChannelFactory<Phenix.Services.Contract.Wcf.IDataDictionary> channelFactory = GetChannelFactory();
            Phenix.Services.Contract.Wcf.IDataDictionary channel = channelFactory.CreateChannel();
            object result = null;
            try
            {
              result = channel.GetSectionInfos();
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
            return (IDictionary<string, SectionInfo>)result;
          }
          catch (EndpointNotFoundException)
          {
            if (!NetConfig.SwitchServicesAddress())
              throw;
          }
        } while (true);
      }
    }

    #region BusinessCode

    public IDictionary<string, BusinessCodeFormat> BusinessCodeFormats
    {
      get
      {
        NetConfig.InitializeSwitch();
        do
        {
          try
          {
            ChannelFactory<Phenix.Services.Contract.Wcf.IDataDictionary> channelFactory = GetChannelFactory();
            Phenix.Services.Contract.Wcf.IDataDictionary channel = channelFactory.CreateChannel();
            object result = null;
            try
            {
              result = channel.GetBusinessCodeFormats();
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
            return (IDictionary<string, BusinessCodeFormat>)result;
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

    private static ChannelFactory<Phenix.Services.Contract.Wcf.IDataDictionary> GetChannelFactory()
    {
      return new ChannelFactory<Phenix.Services.Contract.Wcf.IDataDictionary>(WcfHelper.CreateBinding(),
        new EndpointAddress(WcfHelper.CreateUrl(NetConfig.ServicesAddress, ServicesInfo.DATA_DICTIONARY_URI)));
    }

    #region IDataDictionary 成员

    public void DepartmentInfoHasChanged()
    {
      NetConfig.InitializeSwitch();
      do
      {
        try
        {
          ChannelFactory<Phenix.Services.Contract.Wcf.IDataDictionary> channelFactory = GetChannelFactory();
          Phenix.Services.Contract.Wcf.IDataDictionary channel = channelFactory.CreateChannel();
          try
          {
            channel.DepartmentInfoHasChanged();
            channelFactory.Close();
          }
          catch
          {
            channelFactory.Abort();
            throw;
          }
          break;
        }
        catch (EndpointNotFoundException)
        {
          if (!NetConfig.SwitchServicesAddress())
            throw;
        }
      } while (true);
    }

    public void PositionInfoHasChanged()
    {
      NetConfig.InitializeSwitch();
      do
      {
        try
        {
          ChannelFactory<Phenix.Services.Contract.Wcf.IDataDictionary> channelFactory = GetChannelFactory();
          Phenix.Services.Contract.Wcf.IDataDictionary channel = channelFactory.CreateChannel();
          try
          {
            channel.PositionInfoHasChanged();
            channelFactory.Close();
          }
          catch
          {
            channelFactory.Abort();
            throw;
          }
          break;
        }
        catch (EndpointNotFoundException)
        {
          if (!NetConfig.SwitchServicesAddress())
            throw;
        }
      } while (true);
    }

    public void AssemblyInfoHasChanged()
    {
      NetConfig.InitializeSwitch();
      do
      {
        try
        {
          ChannelFactory<Phenix.Services.Contract.Wcf.IDataDictionary> channelFactory = GetChannelFactory();
          Phenix.Services.Contract.Wcf.IDataDictionary channel = channelFactory.CreateChannel();
          try
          {
            channel.AssemblyInfoHasChanged();
            channelFactory.Close();
          }
          catch
          {
            channelFactory.Abort();
            throw;
          }
          break;
        }
        catch (EndpointNotFoundException)
        {
          if (!NetConfig.SwitchServicesAddress())
            throw;
        }
      } while (true);
    }

    public IDictionary<string, AssemblyInfo> GetAssemblyInfos()
    {
      NetConfig.InitializeSwitch();
      do
      {
        try
        {
          ChannelFactory<Phenix.Services.Contract.Wcf.IDataDictionary> channelFactory = GetChannelFactory();
          Phenix.Services.Contract.Wcf.IDataDictionary channel = channelFactory.CreateChannel();
          object result = null;
          try
          {
            result = channel.GetAssemblyInfos();
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
          return (IDictionary<string, AssemblyInfo>)result;
        }
        catch (EndpointNotFoundException)
        {
          if (!NetConfig.SwitchServicesAddress())
            throw;
        }
      } while (true);
    }

    public AssemblyInfo GetAssemblyInfo(string assemblyName)
    {
      NetConfig.InitializeSwitch();
      do
      {
        try
        {
          ChannelFactory<Phenix.Services.Contract.Wcf.IDataDictionary> channelFactory = GetChannelFactory();
          Phenix.Services.Contract.Wcf.IDataDictionary channel = channelFactory.CreateChannel();
          object result = null;
          try
          {
            result = channel.GetAssemblyInfo(assemblyName);
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
          return (AssemblyInfo)result;
        }
        catch (EndpointNotFoundException)
        {
          if (!NetConfig.SwitchServicesAddress())
            throw;
        }
      } while (true);
    }

    public void TableFilterInfoHasChanged()
    {
      NetConfig.InitializeSwitch();
      do
      {
        try
        {
          ChannelFactory<Phenix.Services.Contract.Wcf.IDataDictionary> channelFactory = GetChannelFactory();
          Phenix.Services.Contract.Wcf.IDataDictionary channel = channelFactory.CreateChannel();
          try
          {
            channel.TableFilterInfoHasChanged();
            channelFactory.Close();
          }
          catch
          {
            channelFactory.Abort();
            throw;
          }
          break;
        }
        catch (EndpointNotFoundException)
        {
          if (!NetConfig.SwitchServicesAddress())
            throw;
        }
      } while (true);
    }

    public void RoleInfoHasChanged()
    {
      NetConfig.InitializeSwitch();
      do
      {
        try
        {
          ChannelFactory<Phenix.Services.Contract.Wcf.IDataDictionary> channelFactory = GetChannelFactory();
          Phenix.Services.Contract.Wcf.IDataDictionary channel = channelFactory.CreateChannel();
          try
          {
            channel.RoleInfoHasChanged();
            channelFactory.Close();
          }
          catch
          {
            channelFactory.Abort();
            throw;
          }
          break;
        }
        catch (EndpointNotFoundException)
        {
          if (!NetConfig.SwitchServicesAddress())
            throw;
        }
      } while (true);
    }

    public void SectionInfoHasChanged()
    {
      NetConfig.InitializeSwitch();
      do
      {
        try
        {
          ChannelFactory<Phenix.Services.Contract.Wcf.IDataDictionary> channelFactory = GetChannelFactory();
          Phenix.Services.Contract.Wcf.IDataDictionary channel = channelFactory.CreateChannel();
          try
          {
            channel.SectionInfoHasChanged();
            channelFactory.Close();
          }
          catch
          {
            channelFactory.Abort();
            throw;
          }
          break;
        }
        catch (EndpointNotFoundException)
        {
          if (!NetConfig.SwitchServicesAddress())
            throw;
        }
      } while (true);
    }

    public void TableInfoHasChanged()
    {
      NetConfig.InitializeSwitch();
      do
      {
        try
        {
          ChannelFactory<Phenix.Services.Contract.Wcf.IDataDictionary> channelFactory = GetChannelFactory();
          Phenix.Services.Contract.Wcf.IDataDictionary channel = channelFactory.CreateChannel();
          try
          {
            channel.TableInfoHasChanged();
            channelFactory.Close();
          }
          catch
          {
            channelFactory.Abort();
            throw;
          }
          break;
        }
        catch (EndpointNotFoundException)
        {
          if (!NetConfig.SwitchServicesAddress())
            throw;
        }
      } while (true);
    }

    public void AddAssemblyClassInfo(string assemblyName, string assemblyCaption,
      string className, string classCaption,/* ExecuteAction? permanentExecuteAction,*/ string[] groupNames, AssemblyClassType classType)
    {
      NetConfig.InitializeSwitch();
      do
      {
        try
        {
          ChannelFactory<Phenix.Services.Contract.Wcf.IDataDictionary> channelFactory = GetChannelFactory();
          Phenix.Services.Contract.Wcf.IDataDictionary channel = channelFactory.CreateChannel();
          object result = null;
          try
          {
            result = channel.AddAssemblyClassInfo(assemblyName, assemblyCaption, className, classCaption,/* permanentExecuteAction,*/ groupNames, classType);
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
          break;
        }
        catch (EndpointNotFoundException)
        {
          if (!NetConfig.SwitchServicesAddress())
            throw;
        }
      } while (true);
    }

    public void AddAssemblyClassPropertyInfos(string assemblyName, string className, string[] names, string[] captions/*,
      string[] tableNames, string[] columnNames, string[] aliases, ExecuteModify[] permanentExecuteModifies*/)
    {
      NetConfig.InitializeSwitch();
      do
      {
        try
        {
          ChannelFactory<Phenix.Services.Contract.Wcf.IDataDictionary> channelFactory = GetChannelFactory();
          Phenix.Services.Contract.Wcf.IDataDictionary channel = channelFactory.CreateChannel();
          object result = null;
          try
          {
            result = channel.AddAssemblyClassPropertyInfos(assemblyName, className, names, captions/*, tableNames, columnNames, aliases, permanentExecuteModifies*/);
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
          break;
        }
        catch (EndpointNotFoundException)
        {
          if (!NetConfig.SwitchServicesAddress())
            throw;
        }
      } while (true);
    }

    public void AddAssemblyClassPropertyConfigInfos(string assemblyName, string className, string[] names, string[] captions,
      bool[] configurables, string[] configKeys, string[] configValues, AssemblyClassType classType)
    {
      NetConfig.InitializeSwitch();
      do
      {
        try
        {
          ChannelFactory<Phenix.Services.Contract.Wcf.IDataDictionary> channelFactory = GetChannelFactory();
          Phenix.Services.Contract.Wcf.IDataDictionary channel = channelFactory.CreateChannel();
          object result = null;
          try
          {
            result = channel.AddAssemblyClassPropertyConfigInfos(assemblyName, className, names, captions, configurables, configKeys, configValues, classType);
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
          break;
        }
        catch (EndpointNotFoundException)
        {
          if (!NetConfig.SwitchServicesAddress())
            throw;
        }
      } while (true);
    }

    public void AddAssemblyClassMethodInfos(string assemblyName, string className, string[] names, string[] captions/*, string[] tags, bool[] allowVisibles*/)
    {
      NetConfig.InitializeSwitch();
      do
      {
        try
        {
          ChannelFactory<Phenix.Services.Contract.Wcf.IDataDictionary> channelFactory = GetChannelFactory();
          Phenix.Services.Contract.Wcf.IDataDictionary channel = channelFactory.CreateChannel();
          object result = null;
          try
          {
            result = channel.AddAssemblyClassMethodInfos(assemblyName, className, names, captions/*, tags, allowVisibles*/);
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
          break;
        }
        catch (EndpointNotFoundException)
        {
          if (!NetConfig.SwitchServicesAddress())
            throw;
        }
      } while (true);
    }

    #region BusinessCode

    public BusinessCodeFormat GetBusinessCodeFormat(string businessCodeName)
    {
      NetConfig.InitializeSwitch();
      do
      {
        try
        {
          ChannelFactory<Phenix.Services.Contract.Wcf.IDataDictionary> channelFactory = GetChannelFactory();
          Phenix.Services.Contract.Wcf.IDataDictionary channel = channelFactory.CreateChannel();
          object result = null;
          try
          {
            result = channel.GetBusinessCodeFormat(businessCodeName);
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
          return (BusinessCodeFormat)result;
        }
        catch (EndpointNotFoundException)
        {
          if (!NetConfig.SwitchServicesAddress())
            throw;
        }
      } while (true);
    }

    public void SetBusinessCodeFormat(BusinessCodeFormat format)
    {
      NetConfig.InitializeSwitch();
      do
      {
        try
        {
          ChannelFactory<Phenix.Services.Contract.Wcf.IDataDictionary> channelFactory = GetChannelFactory();
          Phenix.Services.Contract.Wcf.IDataDictionary channel = channelFactory.CreateChannel();
          object result = null;
          try
          {
            result = channel.SetBusinessCodeFormat(format);
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
          break;
        }
        catch (EndpointNotFoundException)
        {
          if (!NetConfig.SwitchServicesAddress())
            throw;
        }
      } while (true);
    }

    public void RemoveBusinessCodeFormat(string businessCodeName)
    {
      NetConfig.InitializeSwitch();
      do
      {
        try
        {
          ChannelFactory<Phenix.Services.Contract.Wcf.IDataDictionary> channelFactory = GetChannelFactory();
          Phenix.Services.Contract.Wcf.IDataDictionary channel = channelFactory.CreateChannel();
          object result = null;
          try
          {
            result = channel.RemoveBusinessCodeFormat(businessCodeName);
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
          break;
        }
        catch (EndpointNotFoundException)
        {
          if (!NetConfig.SwitchServicesAddress())
            throw;
        }
      } while (true);
    }
    
    #endregion

    #endregion

    #endregion
  }
}