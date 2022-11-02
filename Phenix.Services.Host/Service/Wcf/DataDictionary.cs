using System;
using System.Reflection;
using System.ServiceModel;
using System.ServiceModel.Activation;
using Phenix.Core.Dictionary;
using Phenix.Core.Log;
using Phenix.Core.Mapping;
using Phenix.Services.Host.Core;

namespace Phenix.Services.Host.Service.Wcf
{
  [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerCall)]
  [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
  public sealed class DataDictionary : Phenix.Services.Contract.Wcf.IDataDictionary
  {
    [OperationBehavior(Impersonation = ImpersonationOption.Allowed)]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
    public object GetEnterprise()
    {
      try
      {
        ServiceManager.CheckActive();
        return DataDictionaryHub.Enterprise;
      }
      catch (Exception ex)
      {
        return ex;
      }
    }

    [OperationBehavior(Impersonation = ImpersonationOption.Allowed)]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
    public object GetDepartmentInfos()
    {
      try
      {
        ServiceManager.CheckActive();
        return DataDictionaryHub.DepartmentInfos;
      }
      catch (Exception ex)
      {
        return ex;
      }
    }

    [OperationBehavior(Impersonation = ImpersonationOption.Allowed)]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
    public object GetPositionInfos()
    {
      try
      {
        ServiceManager.CheckActive();
        return DataDictionaryHub.PositionInfos;
      }
      catch (Exception ex)
      {
        return ex;
      }
    }

    [OperationBehavior(Impersonation = ImpersonationOption.Allowed)]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
    public object GetTableFilterInfos()
    {
      try
      {
        ServiceManager.CheckActive();
        return DataDictionaryHub.TableFilterInfos;
      }
      catch (Exception ex)
      {
        return ex;
      }
    }

    [OperationBehavior(Impersonation = ImpersonationOption.Allowed)]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
    public object GetRoleInfos()
    {
      try
      {
        ServiceManager.CheckActive();
        return DataDictionaryHub.RoleInfos;
      }
      catch (Exception ex)
      {
        return ex;
      }
    }

    [OperationBehavior(Impersonation = ImpersonationOption.Allowed)]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
    public object GetSectionInfos()
    {
      try
      {
        ServiceManager.CheckActive();
        return DataDictionaryHub.SectionInfos;
      }
      catch (Exception ex)
      {
        return ex;
      }
    }

    [OperationBehavior(Impersonation = ImpersonationOption.Allowed)]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
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

    [OperationBehavior(Impersonation = ImpersonationOption.Allowed)]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
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


    [OperationBehavior(Impersonation = ImpersonationOption.Allowed)]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
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

    [OperationBehavior(Impersonation = ImpersonationOption.Allowed)]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
    public object GetAssemblyInfos()
    {
      try
      {
        ServiceManager.CheckActive();
        return DataDictionaryHub.GetAssemblyInfos();
      }
      catch (Exception ex)
      {
        return ex;
      }
    }

    [OperationBehavior(Impersonation = ImpersonationOption.Allowed)]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
    public object GetAssemblyInfo(string assemblyName)
    {
      try
      {
        ServiceManager.CheckActive();
        return DataDictionaryHub.GetAssemblyInfo(assemblyName);
      }
      catch (Exception ex)
      {
        return ex;
      }
    }

    [OperationBehavior(Impersonation = ImpersonationOption.Allowed)]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
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

    [OperationBehavior(Impersonation = ImpersonationOption.Allowed)]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
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

    [OperationBehavior(Impersonation = ImpersonationOption.Allowed)]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
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

    [OperationBehavior(Impersonation = ImpersonationOption.Allowed)]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
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

    [OperationBehavior(Impersonation = ImpersonationOption.Allowed)]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
    public object AddAssemblyClassInfo(string assemblyName, string assemblyCaption, string className, string classCaption,/* ExecuteAction? permanentExecuteAction,*/ string[] groupNames, AssemblyClassType classType)
    {
      try
      {
        ServiceManager.CheckActive();
        DataDictionaryHub.AddAssemblyClassInfo(assemblyName, assemblyCaption, className, classCaption,/* permanentExecuteAction,*/ groupNames, classType);
        return null;
      }
      catch (Exception ex)
      {
        return ex;
      }
    }

    [OperationBehavior(Impersonation = ImpersonationOption.Allowed)]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
    public object AddAssemblyClassPropertyInfos(string assemblyName, string className, string[] names, string[] captions/*,
      string[] tableNames, string[] columnNames, string[] aliases, ExecuteModify[] permanentExecuteModifies*/)
    {
      try
      {
        ServiceManager.CheckActive();
        DataDictionaryHub.AddAssemblyClassPropertyInfos(assemblyName, className, names, captions/*, tableNames, columnNames, aliases, permanentExecuteModifies*/);
        return null;
      }
      catch (Exception ex)
      {
        return ex;
      }
    }

    [OperationBehavior(Impersonation = ImpersonationOption.Allowed)]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
    public object AddAssemblyClassPropertyConfigInfos(string assemblyName, string className, string[] names, string[] captions,
      bool[] configurables, string[] configKeys, string[] configValues, AssemblyClassType classType)
    {
      try
      {
        ServiceManager.CheckActive();
        DataDictionaryHub.AddAssemblyClassPropertyInfos(assemblyName, className, names, captions, configurables, configKeys, configValues, classType);
        return null;
      }
      catch (Exception ex)
      {
        return ex;
      }
    }

    [OperationBehavior(Impersonation = ImpersonationOption.Allowed)]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
    public object AddAssemblyClassMethodInfos(string assemblyName, string className, string[] names, string[] captions/*, string[] tags, bool[] allowVisibles*/)
    {
      try
      {
        ServiceManager.CheckActive();
        DataDictionaryHub.AddAssemblyClassMethodInfos(assemblyName, className, names, captions/*, tags, allowVisibles*/);
        return null;
      }
      catch (Exception ex)
      {
        return ex;
      }
    }
    
    #region BusinessCode

    [OperationBehavior(Impersonation = ImpersonationOption.Allowed)]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
    public object GetBusinessCodeFormats()
    {
      try
      {
        ServiceManager.CheckActive();
        return DataDictionaryHub.BusinessCodeFormats;
      }
      catch (Exception ex)
      {
        return ex;
      }
    }

    [OperationBehavior(Impersonation = ImpersonationOption.Allowed)]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
    public object GetBusinessCodeFormat(string businessCodeName)
    {
      try
      {
        ServiceManager.CheckActive();
        return DataDictionaryHub.GetBusinessCodeFormat(businessCodeName);
      }
      catch (Exception ex)
      {
        return ex;
      }
    }

    [OperationBehavior(Impersonation = ImpersonationOption.Allowed)]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
    public object SetBusinessCodeFormat(BusinessCodeFormat format)
    {
      try
      {
        ServiceManager.CheckActive();
        DataDictionaryHub.SetBusinessCodeFormat(format);
        return null;
      }
      catch (Exception ex)
      {
        return ex;
      }
    }

    [OperationBehavior(Impersonation = ImpersonationOption.Allowed)]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
    public object RemoveBusinessCodeFormat(string businessCodeName)
    {
      try
      {
        ServiceManager.CheckActive();
        DataDictionaryHub.RemoveBusinessCodeFormat(businessCodeName);
        return null;
      }
      catch (Exception ex)
      {
        return ex;
      }
    }

    #endregion
  }
}