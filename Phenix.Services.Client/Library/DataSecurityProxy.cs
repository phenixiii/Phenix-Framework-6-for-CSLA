using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Net.Sockets;
using System.Reflection;
using Phenix.Core.Dictionary;
using Phenix.Core.Net;
using Phenix.Core.Security;
using Phenix.Services.Contract;

namespace Phenix.Services.Client.Library
{
  internal class DataSecurityProxy : IDataSecurity
  {
    #region 属性

    private IDataSecurity _service;
    private IDataSecurity Service
    {
      get
      {
        if (_service == null)
        {
          RemotingHelper.RegisterClientChannel();
          _service = (IDataSecurity)RemotingHelper.CreateRemoteObjectProxy(typeof(IDataSecurity), ServicesInfo.DATA_SECURITY_URI);
        }
        return _service;
      }
    }

    #region IDataSecurity 成员

    public bool AllowUserMultipleAddressLogin
    {
      get
      {
        NetConfig.InitializeSwitch();
        do
        {
          try
          {
            return Service.AllowUserMultipleAddressLogin;
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

    public int SessionExpiresMinutes
    {
      get
      {
        NetConfig.InitializeSwitch();
        do
        {
          try
          {
            return Service.SessionExpiresMinutes;
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

    public bool EmptyRolesIsDeny
    {
      get
      {
        NetConfig.InitializeSwitch();
        do
        {
          try
          {
            return Service.EmptyRolesIsDeny;
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

    public bool EasyAuthorization
    {
      get
      {
        NetConfig.InitializeSwitch();
        do
        {
          try
          {
            return Service.EasyAuthorization;
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

    #region 方法

    private void InvalidateCache()
    {
      _service = null;
    }

    #region IDataSecurity 成员

    public IDictionary<string, RoleInfo> GetRoleInfos(UserIdentity identity)
    {
      NetConfig.InitializeSwitch();
      do
      {
        try
        {
          return Service.GetRoleInfos(identity);
        }
        catch (SocketException)
        {
          InvalidateCache();
          if (!NetConfig.SwitchServicesAddress())
            throw;
        }
      } while (true);
    }

    public IDictionary<string, RoleInfo> GetGrantRoleInfos(UserIdentity identity)
    {
      NetConfig.InitializeSwitch();
      do
      {
        try
        {
          return Service.GetGrantRoleInfos(identity);
        }
        catch (SocketException)
        {
          InvalidateCache();
          if (!NetConfig.SwitchServicesAddress())
            throw;
        }
      } while (true);
    }

    public IDictionary<string, SectionInfo> GetSectionInfos(UserIdentity identity)
    {
      NetConfig.InitializeSwitch();
      do
      {
        try
        {
          return Service.GetSectionInfos(identity);
        }
        catch (SocketException)
        {
          InvalidateCache();
          if (!NetConfig.SwitchServicesAddress())
            throw;
        }
      } while (true);
    }

    public IDictionary<string, IIdentity> GetIdentities(long departmentId, IList<long> positionIds, UserIdentity identity)
    {
      NetConfig.InitializeSwitch();
      do
      {
        try
        {
          return Service.GetIdentities(departmentId, positionIds, identity);
        }
        catch (SocketException)
        {
          InvalidateCache();
          if (!NetConfig.SwitchServicesAddress())
            throw;
        }
      } while (true);
    }

    public DataSecurityContext CheckIn(UserIdentity identity, bool reset)
    {
      InvalidateCache();
      NetConfig.ServicesAddress = identity.ServicesAddress;
      NetConfig.InitializeSwitch();
      do
      {
        try
        {
          return Service.CheckIn(identity, reset);
        }
        catch (SocketException)
        {
          InvalidateCache();
          if (!NetConfig.SwitchServicesAddress())
            throw;
        }
      } while (true);
    }

    public DataSecurityContext CheckIn(string localAddress, string servicesAddress, string userNumber, string timestamp, string signature, bool reset)
    {
      InvalidateCache();
      NetConfig.ServicesAddress = servicesAddress;
      NetConfig.InitializeSwitch();
      do
      {
        try
        {
          return Service.CheckIn(localAddress, servicesAddress, userNumber, timestamp, signature, reset);
        }
        catch (SocketException)
        {
          InvalidateCache();
          if (!NetConfig.SwitchServicesAddress())
            throw;
        }
      } while (true);
    }

    public bool? LogOnVerify(string userNumber, string tag)
    {
      NetConfig.InitializeSwitch();
      do
      {
        try
        {
          return Service.LogOnVerify(userNumber, tag);
        }
        catch (SocketException)
        {
          InvalidateCache();
          if (!NetConfig.SwitchServicesAddress())
            throw;
        }
      } while (true);
    }

    public void LogOff(UserIdentity identity)
    {
      NetConfig.InitializeSwitch();
      do
      {
        try
        {
          Service.LogOff(identity);
          break;
        }
        catch (SocketException)
        {
          InvalidateCache();
          if (!NetConfig.SwitchServicesAddress())
            return;
        }
      } while (true);
    }

    public bool ChangePassword(string newPassword, UserIdentity identity)
    {
      InvalidateCache();
      NetConfig.ServicesAddress = identity.ServicesAddress;
      NetConfig.InitializeSwitch();
      do
      {
        try
        {
          return Service.ChangePassword(newPassword, identity);
        }
        catch (SocketException)
        {
          InvalidateCache();
          if (!NetConfig.SwitchServicesAddress())
            throw;
        }
      } while (true);
    }

    public bool UnlockPassword(string userNumber, UserIdentity identity)
    {
      NetConfig.InitializeSwitch();
      do
      {
        try
        {
          return Service.UnlockPassword(userNumber, identity);
        }
        catch (SocketException)
        {
          InvalidateCache();
          if (!NetConfig.SwitchServicesAddress())
            throw;
        }
      } while (true);
    }

    public bool ResetPassword(string userNumber, UserIdentity identity)
    {
      NetConfig.InitializeSwitch();
      do
      {
        try
        {
          return Service.ResetPassword(userNumber, identity);
        }
        catch (SocketException)
        {
          InvalidateCache();
          if (!NetConfig.SwitchServicesAddress())
            throw;
        }
      } while (true);
    }

    public void SetProcessLockInfo(string processName, string caption, bool toLocked, TimeSpan expiryTime, string remark, UserIdentity identity)
    {
      NetConfig.InitializeSwitch();
      do
      {
        try
        {
          Service.SetProcessLockInfo(processName, caption, toLocked, expiryTime, remark, identity);
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

    public ProcessLockInfo GetProcessLockInfo(string processName, UserIdentity identity)
    {
      NetConfig.InitializeSwitch();
      do
      {
        try
        {
          return Service.GetProcessLockInfo(processName, identity);
        }
        catch (SocketException)
        {
          InvalidateCache();
          if (!NetConfig.SwitchServicesAddress())
            throw;
        }
      } while (true);
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

    #endregion
  }
}