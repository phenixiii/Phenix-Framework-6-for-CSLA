using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Reflection;
using Phenix.Core.Dictionary;
using Phenix.Core.Log;
using Phenix.Core.Security;
using Phenix.Services.Host.Core;

namespace Phenix.Services.Host.Service
{
  public sealed class DataSecurity : MarshalByRefObject, IDataSecurity
  {
    #region 属性

    public bool AllowUserMultipleAddressLogin
    {
      get
      {
        ServiceManager.CheckActive();
        return DataSecurityHub.AllowUserMultipleAddressLogin;
      }
    }

    public int SessionExpiresMinutes
    {
      get
      {
        ServiceManager.CheckActive();
        return DataSecurityHub.SessionExpiresMinutes;
      }
    }

    public bool EmptyRolesIsDeny
    {
      get
      {
        ServiceManager.CheckActive();
        return DataSecurityHub.EmptyRolesIsDeny;
      }
    }

    public bool EasyAuthorization
    {
      get
      {
        ServiceManager.CheckActive();
        return DataSecurityHub.EasyAuthorization;
      }
    }

    #endregion

    #region 事件

    internal static event Action<DataSecurityEventArgs> Changed;
    private static void OnChanged(DataSecurityEventArgs e)
    {
      Action<DataSecurityEventArgs> action = Changed;
      if (action != null)
        action(e);
    }

    #endregion

    #region 方法

    public IDictionary<string, RoleInfo> GetRoleInfos(UserIdentity identity)
    {
      ServiceManager.CheckIn(identity);
      DataSecurityContext context = DataSecurityHub.CheckIn(identity, false);
      return DataSecurityHub.GetRoleInfos(context.Identity);
    }

    public IDictionary<string, RoleInfo> GetGrantRoleInfos(UserIdentity identity)
    {
      ServiceManager.CheckIn(identity);
      DataSecurityContext context = DataSecurityHub.CheckIn(identity, false);
      return DataSecurityHub.GetGrantRoleInfos(context.Identity);
    }

    public IDictionary<string, SectionInfo> GetSectionInfos(UserIdentity identity)
    {
      ServiceManager.CheckIn(identity);
      DataSecurityContext context = DataSecurityHub.CheckIn(identity, false);
      return DataSecurityHub.GetSectionInfos(context.Identity);
    }

    public IDictionary<string, IIdentity> GetIdentities(long departmentId, IList<long> positionIds, UserIdentity identity)
    {
      ServiceManager.CheckIn(identity);
      DataSecurityContext context = DataSecurityHub.CheckIn(identity, false);
      return DataSecurityHub.GetIdentities(departmentId, positionIds, context.Identity);
    }

    public DataSecurityContext CheckIn(UserIdentity identity, bool reset)
    {
      ServiceManager.CheckActive();
      OnChanged(new DataSecurityEventArgs(identity.LocalAddress, identity.UserNumber, true));
      return DataSecurityHub.CheckIn(identity, reset);
    }

    public DataSecurityContext CheckIn(string localAddress, string servicesAddress, string userNumber, string timestamp, string signature, bool reset)
    {
      ServiceManager.CheckActive();
      OnChanged(new DataSecurityEventArgs(localAddress, userNumber, true));
      return DataSecurityHub.CheckIn(localAddress, servicesAddress, userNumber, timestamp, signature, reset);
    }

    public bool? LogOnVerify(string userNumber, string tag)
    {
      ServiceManager.CheckActive();
      return DataSecurityHub.LogOnVerify(userNumber, tag);
    }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
    [System.Runtime.Remoting.Messaging.OneWay]
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

    public bool ChangePassword(string newPassword, UserIdentity identity)
    {
      ServiceManager.CheckActive();
      OnChanged(new DataSecurityEventArgs(identity, true));
      return DataSecurityHub.ChangePassword(newPassword, false, identity);
    }

    public bool UnlockPassword(string userNumber, UserIdentity identity)
    {
      ServiceManager.CheckActive();
      return DataSecurityHub.UnlockPassword(userNumber, identity);
    }

    public bool ResetPassword(string userNumber, UserIdentity identity)
    {
      ServiceManager.CheckActive();
      return DataSecurityHub.ResetPassword(userNumber, identity);
    }

    public void SetProcessLockInfo(string processName, string caption, bool toLocked, TimeSpan expiryTime, string remark, UserIdentity identity)
    {
      ServiceManager.CheckIn(identity);
      DataSecurityContext context = DataSecurityHub.CheckIn(identity, false);
      DataSecurityHub.SetProcessLockInfo(processName, caption, toLocked, expiryTime, remark, context.Identity);
    }

    public ProcessLockInfo GetProcessLockInfo(string processName, UserIdentity identity)
    {
      ServiceManager.CheckIn(identity);
      DataSecurityContext context = DataSecurityHub.CheckIn(identity, false);
      return DataSecurityHub.GetProcessLockInfo(processName, context.Identity);
    }

    #region 应用服务不支持传事务

    public void AddUser(DbTransaction transaction, long id, string userName, string userNumber, string password)
    {
      throw new NotSupportedException(MethodBase.GetCurrentMethod().Name);
    }

    public bool ChangePassword(DbTransaction transaction, string userNumber, string newPassword)
    {
      throw new NotSupportedException(MethodBase.GetCurrentMethod().Name);
    }

    #endregion

    #endregion
  }
}