using System.Collections.Generic;
using System.Net.Sockets;
using Phenix.Core.Dictionary;
using Phenix.Core.Mapping;
using Phenix.Core.Net;
using Phenix.Core.Security;
using Phenix.Services.Contract;

namespace Phenix.Services.Client.Library
{
  internal class DataDictionaryProxy : IDataDictionary
  {
    #region 属性

    private IDataDictionary _service;
    private IDataDictionary Service
    {
      get
      {
        if (_service == null)
        {
          RemotingHelper.RegisterClientChannel();
          _service = (IDataDictionary)RemotingHelper.CreateRemoteObjectProxy(typeof(IDataDictionary), ServicesInfo.DATA_DICTIONARY_URI);
        }
        return _service;
      }
    }

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
            return Service.Enterprise;
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

    public IDictionary<long, DepartmentInfo> DepartmentInfos
    {
      get
      {
        NetConfig.InitializeSwitch();
        do
        {
          try
          {
            return Service.DepartmentInfos;
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

    public IDictionary<long, PositionInfo> PositionInfos
    {
      get
      {
        NetConfig.InitializeSwitch();
        do
        {
          try
          {
            return Service.PositionInfos;
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

    public IDictionary<string, TableFilterInfo> TableFilterInfos
    {
      get
      {
        NetConfig.InitializeSwitch();
        do
        {
          try
          {
            return Service.TableFilterInfos;
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

    public IDictionary<string, RoleInfo> RoleInfos
    {
      get
      {
        NetConfig.InitializeSwitch();
        do
        {
          try
          {
            return Service.RoleInfos;
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

    public IDictionary<string, SectionInfo> SectionInfos
    {
      get
      {
        NetConfig.InitializeSwitch();
        do
        {
          try
          {
            return Service.SectionInfos;
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
            return Service.BusinessCodeFormats;
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

    #region IDataDictionary 成员

    public void DepartmentInfoHasChanged()
    {
      NetConfig.InitializeSwitch();
      do
      {
        try
        {
          Service.DepartmentInfoHasChanged();
          break;
        }
        catch (SocketException)
        {
          InvalidateCache();
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
          Service.PositionInfoHasChanged();
          break;
        }
        catch (SocketException)
        {
          InvalidateCache();
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
          Service.AssemblyInfoHasChanged();
          break;
        }
        catch (SocketException)
        {
          InvalidateCache();
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
          return Service.GetAssemblyInfos();
        }
        catch (SocketException)
        {
          InvalidateCache();
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
          return Service.GetAssemblyInfo(assemblyName);
        }
        catch (SocketException)
        {
          InvalidateCache();
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
          Service.TableFilterInfoHasChanged();
          break;
        }
        catch (SocketException)
        {
          InvalidateCache();
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
          Service.RoleInfoHasChanged();
          break;
        }
        catch (SocketException)
        {
          InvalidateCache();
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
          Service.SectionInfoHasChanged();
          break;
        }
        catch (SocketException)
        {
          InvalidateCache();
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
          Service.TableInfoHasChanged();
          break;
        }
        catch (SocketException)
        {
          InvalidateCache();
          if (!NetConfig.SwitchServicesAddress())
            throw;
        }
      } while (true);
    }

    public void AddAssemblyClassInfo(string assemblyName, string assemblyCaption, string className, string classCaption,/* ExecuteAction? permanentExecuteAction,*/ string[] groupNames, AssemblyClassType classType)
    {
      NetConfig.InitializeSwitch();
      do
      {
        try
        {
          Service.AddAssemblyClassInfo(assemblyName, assemblyCaption, className, classCaption,/* permanentExecuteAction,*/ groupNames, classType);
          break;
        }
        catch (SocketException)
        {
          InvalidateCache();
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
          Service.AddAssemblyClassPropertyInfos(assemblyName, className, names, captions/*, tableNames, columnNames, aliases, permanentExecuteModifies*/);
          break;
        }
        catch (SocketException)
        {
          InvalidateCache();
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
          Service.AddAssemblyClassPropertyConfigInfos(assemblyName, className, names, captions, configurables, configKeys, configValues, classType);
          break;
        }
        catch (SocketException)
        {
          InvalidateCache();
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
          Service.AddAssemblyClassMethodInfos(assemblyName, className, names, captions/*, tags, allowVisibles*/);
          break;
        }
        catch (SocketException)
        {
          InvalidateCache();
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
          return Service.GetBusinessCodeFormat(businessCodeName);
        }
        catch (SocketException)
        {
          InvalidateCache();
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
          Service.SetBusinessCodeFormat(format);
          break;
        }
        catch (SocketException)
        {
          InvalidateCache();
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
          Service.RemoveBusinessCodeFormat(businessCodeName);
          break;
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

    #endregion

    #endregion
  }
}