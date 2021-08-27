using System;
using System.Collections.Generic;
using System.Reflection;
using System.ServiceModel;
using System.ServiceModel.Activation;
using Phenix.Core.Log;
using Phenix.Core.Security;
using Phenix.Services.Host.Core;

namespace Phenix.Services.Host.Service.Wcf
{
  [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerCall)]
  [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
  public sealed class DataSecurity : Phenix.Services.Contract.Wcf.IDataSecurity
  {
    #region 事件

    internal static event Action<DataSecurityEventArgs> Changed;
    private static void OnChanged(DataSecurityEventArgs e)
    {
      Action<DataSecurityEventArgs> action = Changed;
      if (action != null)
        action(e);
    }

    #endregion

    [OperationBehavior(Impersonation = ImpersonationOption.Allowed)]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
    public object GetAllowUserMultipleAddressLogin()
    {
      try
      {
        ServiceManager.CheckActive();
        return DataSecurityHub.AllowUserMultipleAddressLogin;
      }
      catch (Exception ex)
      {
        return ex;
      }
    }

    [OperationBehavior(Impersonation = ImpersonationOption.Allowed)]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
    public object GetSessionExpiresMinutes()
    {
      try
      {
        ServiceManager.CheckActive();
        return DataSecurityHub.SessionExpiresMinutes;
      }
      catch (Exception ex)
      {
        return ex;
      }
    }

    [OperationBehavior(Impersonation = ImpersonationOption.Allowed)]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
    public object GetEmptyRolesIsDeny()
    {
      try
      {
        ServiceManager.CheckActive();
        return DataSecurityHub.EmptyRolesIsDeny;
      }
      catch (Exception ex)
      {
        return ex;
      }
    }

    [OperationBehavior(Impersonation = ImpersonationOption.Allowed)]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
    public object GetEasyAuthorization()
    {
      try
      {
        ServiceManager.CheckActive();
        return DataSecurityHub.EasyAuthorization;
      }
      catch (Exception ex)
      {
        return ex;
      }
    }

    [OperationBehavior(Impersonation = ImpersonationOption.Allowed)]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
    public object GetRoleInfos(UserIdentity identity)
    {
      try
      {
        ServiceManager.CheckIn(identity);
        DataSecurityContext context = DataSecurityHub.CheckIn(identity, false);
        return DataSecurityHub.GetRoleInfos(context.Identity);
      }
      catch (Exception ex)
      {
        return ex;
      }
    }

    [OperationBehavior(Impersonation = ImpersonationOption.Allowed)]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
    public object GetGrantRoleInfos(UserIdentity identity)
    {
      try
      {
        ServiceManager.CheckIn(identity);
        DataSecurityContext context = DataSecurityHub.CheckIn(identity, false);
        return DataSecurityHub.GetGrantRoleInfos(context.Identity);
      }
      catch (Exception ex)
      {
        return ex;
      }
    }

    [OperationBehavior(Impersonation = ImpersonationOption.Allowed)]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
    public object GetSectionInfos(UserIdentity identity)
    {
      try
      {
        ServiceManager.CheckIn(identity);
        DataSecurityContext context = DataSecurityHub.CheckIn(identity, false);
        return DataSecurityHub.GetSectionInfos(context.Identity);
      }
      catch (Exception ex)
      {
        return ex;
      }
    }

    [OperationBehavior(Impersonation = ImpersonationOption.Allowed)]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
    public object GetIdentities(long departmentId, IList<long> positionIds, UserIdentity identity)
    {
      try
      {
        ServiceManager.CheckIn(identity);
        DataSecurityContext context = DataSecurityHub.CheckIn(identity, false);
        return DataSecurityHub.GetIdentities(departmentId, positionIds, context.Identity);
      }
      catch (Exception ex)
      {
        return ex;
      }
    }

    [OperationBehavior(Impersonation = ImpersonationOption.Allowed)]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
    public object CheckIn(UserIdentity identity, bool reset)
    {
      try
      {
        ServiceManager.CheckActive();
        OnChanged(new DataSecurityEventArgs(identity.LocalAddress, identity.UserNumber, true));
        return DataSecurityHub.CheckIn(identity, reset);
      }
      catch (Exception ex)
      {
        return ex;
      }
    }

    [OperationBehavior(Impersonation = ImpersonationOption.Allowed)]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
    public object CheckIn(string localAddress, string servicesAddress, string userNumber, string timestamp, string signature, bool reset)
    {
      try
      {
        ServiceManager.CheckActive();
        OnChanged(new DataSecurityEventArgs(localAddress, userNumber, true));
        return DataSecurityHub.CheckIn(localAddress, servicesAddress, userNumber, timestamp, signature, reset);
      }
      catch (Exception ex)
      {
        return ex;
      }
    }

    [OperationBehavior(Impersonation = ImpersonationOption.Allowed)]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
    public object LogOnVerify(string userNumber, string tag)
    {
      try
      {
        ServiceManager.CheckActive();
        return DataSecurityHub.LogOnVerify(userNumber, tag);
      }
      catch (Exception ex)
      {
        return ex;
      }
    }

    [OperationBehavior(Impersonation = ImpersonationOption.Allowed)]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
    public void LogOff(UserIdentity identity)
    {
      try
      {
        OnChanged(new DataSecurityEventArgs(identity, false));
        DataSecurityHub.LogOff(identity);
      }
      catch (Exception ex)
      {
        EventLog.SaveLocal(MethodBase.GetCurrentMethod(), String.Format("{0}-{1}", identity.LocalAddress, identity.UserNumber), ex);
      }
    }

    [OperationBehavior(Impersonation = ImpersonationOption.Allowed)]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
    public object ChangePassword(string newPassword, UserIdentity identity)
    {
      try
      {
        ServiceManager.CheckActive();
        OnChanged(new DataSecurityEventArgs(identity, true));
        return DataSecurityHub.ChangePassword(newPassword, false, identity);
      }
      catch (Exception ex)
      {
        return ex;
      }
    }

    [OperationBehavior(Impersonation = ImpersonationOption.Allowed)]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
    public object UnlockPassword(string userNumber, UserIdentity identity)
    {
      try
      {
        ServiceManager.CheckActive();
        return DataSecurityHub.UnlockPassword(userNumber, identity);
      }
      catch (Exception ex)
      {
        return ex;
      }
    }

    [OperationBehavior(Impersonation = ImpersonationOption.Allowed)]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
    public object ResetPassword(string userNumber, UserIdentity identity)
    {
      try
      {
        ServiceManager.CheckActive();
        return DataSecurityHub.ResetPassword(userNumber, identity);
      }
      catch (Exception ex)
      {
        return ex;
      }
    }

    [OperationBehavior(Impersonation = ImpersonationOption.Allowed)]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
    public object SetProcessLockInfo(string processName, string caption, bool toLocked, TimeSpan expiryTime, string remark, UserIdentity identity)
    {
      try
      {
        ServiceManager.CheckIn(identity);
        DataSecurityContext context = DataSecurityHub.CheckIn(identity, false);
        DataSecurityHub.SetProcessLockInfo(processName, caption, toLocked, expiryTime, remark, context.Identity);
        return null;
      }
      catch (Exception ex)
      {
        return ex;
      }
    }

    [OperationBehavior(Impersonation = ImpersonationOption.Allowed)]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
    public object GetProcessLockInfo(string processName, UserIdentity identity)
    {
      try
      {
        ServiceManager.CheckIn(identity);
        DataSecurityContext context = DataSecurityHub.CheckIn(identity, false);
        return DataSecurityHub.GetProcessLockInfo(processName, context.Identity);
      }
      catch (Exception ex)
      {
        return ex;
      }
    }
  }
}