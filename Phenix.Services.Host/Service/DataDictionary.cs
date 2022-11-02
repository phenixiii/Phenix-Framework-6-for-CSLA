using System;
using System.Collections.Generic;
using System.Reflection;
using Phenix.Core.Dictionary;
using Phenix.Core.Log;
using Phenix.Core.Mapping;
using Phenix.Core.Security;
using Phenix.Services.Host.Core;

namespace Phenix.Services.Host.Service
{
  public sealed class DataDictionary : MarshalByRefObject, IDataDictionary
  {
    #region 属性

    public string Enterprise
    {
      get
      {
        ServiceManager.CheckActive();
        return DataDictionaryHub.Enterprise;
      }
    }

    public IDictionary<long, DepartmentInfo> DepartmentInfos
    {
      get
      {
        ServiceManager.CheckActive();
        return DataDictionaryHub.DepartmentInfos;
      }
    }

    public IDictionary<long, PositionInfo> PositionInfos
    {
      get
      {
        ServiceManager.CheckActive();
        return DataDictionaryHub.PositionInfos;
      }
    }

    public IDictionary<string, TableFilterInfo> TableFilterInfos
    {
      get
      {
        ServiceManager.CheckActive();
        return DataDictionaryHub.TableFilterInfos;
      }
    }

    public IDictionary<string, RoleInfo> RoleInfos
    {
      get
      {
        ServiceManager.CheckActive();
        return DataDictionaryHub.RoleInfos;
      }
    }

    public IDictionary<string, SectionInfo> SectionInfos
    {
      get
      {
        ServiceManager.CheckActive();
        return DataDictionaryHub.SectionInfos;
      }
    }

    #region BusinessCode

    public IDictionary<string, BusinessCodeFormat> BusinessCodeFormats
    {
      get
      {
        ServiceManager.CheckActive();
        return DataDictionaryHub.BusinessCodeFormats;
      }
    }

    #endregion

    #endregion

    #region 方法

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
    [System.Runtime.Remoting.Messaging.OneWay]
    public void DepartmentInfoHasChanged()
    {
      try
      {
        DataDictionaryHub.DepartmentInfoHasChanged();
      }
      catch (Exception ex)
      {
        EventLog.SaveLocal(MethodBase.GetCurrentMethod(), ex);
      }
    }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
    [System.Runtime.Remoting.Messaging.OneWay]
    public void PositionInfoHasChanged()
    {
      try
      {
        DataDictionaryHub.PositionInfoHasChanged();
      }
      catch (Exception ex)
      {
        EventLog.SaveLocal(MethodBase.GetCurrentMethod(), ex);
      }
    }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
    [System.Runtime.Remoting.Messaging.OneWay]
    public void AssemblyInfoHasChanged()
    {
      try
      {
        DataDictionaryHub.AssemblyInfoHasChanged();
      }
      catch (Exception ex)
      {
        EventLog.SaveLocal(MethodBase.GetCurrentMethod(), ex);
      }
    }

    public IDictionary<string, AssemblyInfo> GetAssemblyInfos()
    {
      ServiceManager.CheckActive();
      return DataDictionaryHub.GetAssemblyInfos();
    }

    public AssemblyInfo GetAssemblyInfo(string assemblyName)
    {
      ServiceManager.CheckActive();
      return DataDictionaryHub.GetAssemblyInfo(assemblyName);
    }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
    [System.Runtime.Remoting.Messaging.OneWay]
    public void TableFilterInfoHasChanged()
    {
      try
      {
        DataDictionaryHub.TableFilterInfoHasChanged();
      }
      catch (Exception ex)
      {
        EventLog.SaveLocal(MethodBase.GetCurrentMethod(), ex);
      }
    }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
    [System.Runtime.Remoting.Messaging.OneWay]
    public void RoleInfoHasChanged()
    {
      try
      {
        DataDictionaryHub.RoleInfoHasChanged();
      }
      catch (Exception ex)
      {
        EventLog.SaveLocal(MethodBase.GetCurrentMethod(), ex);
      }
    }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
    [System.Runtime.Remoting.Messaging.OneWay]
    public void SectionInfoHasChanged()
    {
      try
      {
        DataDictionaryHub.SectionInfoHasChanged();
      }
      catch (Exception ex)
      {
        EventLog.SaveLocal(MethodBase.GetCurrentMethod(), ex);
      }
    }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
    [System.Runtime.Remoting.Messaging.OneWay]
    public void TableInfoHasChanged()
    {
      try
      {
        DataDictionaryHub.TableInfoHasChanged();
      }
      catch (Exception ex)
      {
        EventLog.SaveLocal(MethodBase.GetCurrentMethod(), ex);
      }
    }
    
    public void AddAssemblyClassInfo(string assemblyName, string assemblyCaption, string className, string classCaption,/* ExecuteAction? permanentExecuteAction,*/ string[] groupNames, AssemblyClassType classType)
    {
      ServiceManager.CheckActive();
      DataDictionaryHub.AddAssemblyClassInfo(assemblyName, assemblyCaption, className, classCaption,/* permanentExecuteAction,*/ groupNames, classType);
    }

    public void AddAssemblyClassPropertyInfos(string assemblyName, string className, string[] names, string[] captions/*,
      string[] tableNames, string[] columnNames, string[] aliases, ExecuteModify[] permanentExecuteModifies*/)
    {
      ServiceManager.CheckActive();
      DataDictionaryHub.AddAssemblyClassPropertyInfos(assemblyName, className, names, captions/*, tableNames, columnNames, aliases, permanentExecuteModifies*/);
    }

    public void AddAssemblyClassPropertyConfigInfos(string assemblyName, string className, string[] names, string[] captions,
      bool[] configurables, string[] configKeys, string[] configValues, AssemblyClassType classType)
    {
      ServiceManager.CheckActive();
      DataDictionaryHub.AddAssemblyClassPropertyInfos(assemblyName, className, names, captions, configurables, configKeys, configValues, classType);
    }

    public void AddAssemblyClassMethodInfos(string assemblyName, string className, string[] names, string[] captions/*, string[] tags, bool[] allowVisibles*/)
    {
      ServiceManager.CheckActive();
      DataDictionaryHub.AddAssemblyClassMethodInfos(assemblyName, className, names, captions/*, tags, allowVisibles*/);
    }

    #region BusinessCode

    public BusinessCodeFormat GetBusinessCodeFormat(string businessCodeName)
    {
      ServiceManager.CheckActive();
      return DataDictionaryHub.GetBusinessCodeFormat(businessCodeName);
    }

    public void SetBusinessCodeFormat(BusinessCodeFormat format)
    {
      ServiceManager.CheckActive();
      DataDictionaryHub.SetBusinessCodeFormat(format);
    }

    public void RemoveBusinessCodeFormat(string businessCodeName)
    {
      ServiceManager.CheckActive();
      DataDictionaryHub.RemoveBusinessCodeFormat(businessCodeName);
    }
    
    #endregion
    
    #endregion
  }
}