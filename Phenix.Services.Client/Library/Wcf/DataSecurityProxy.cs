using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Reflection;
using System.ServiceModel;
using Phenix.Core.Dictionary;
using Phenix.Core.Net;
using Phenix.Core.Security;
using Phenix.Services.Contract;

namespace Phenix.Services.Client.Library.Wcf
{
  internal class DataSecurityProxy : IDataSecurity
  {
    #region 属性

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
            ChannelFactory<Phenix.Services.Contract.Wcf.IDataSecurity> channelFactory = GetChannelFactory();
            Phenix.Services.Contract.Wcf.IDataSecurity channel = channelFactory.CreateChannel();
            object result = null;
            try
            {
              result = channel.GetAllowUserMultipleAddressLogin();
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
            ChannelFactory<Phenix.Services.Contract.Wcf.IDataSecurity> channelFactory = GetChannelFactory();
            Phenix.Services.Contract.Wcf.IDataSecurity channel = channelFactory.CreateChannel();
            object result = null;
            try
            {
              result = channel.GetSessionExpiresMinutes();
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
            ChannelFactory<Phenix.Services.Contract.Wcf.IDataSecurity> channelFactory = GetChannelFactory();
            Phenix.Services.Contract.Wcf.IDataSecurity channel = channelFactory.CreateChannel();
            object result = null;
            try
            {
              result = channel.GetEmptyRolesIsDeny();
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
            ChannelFactory<Phenix.Services.Contract.Wcf.IDataSecurity> channelFactory = GetChannelFactory();
            Phenix.Services.Contract.Wcf.IDataSecurity channel = channelFactory.CreateChannel();
            object result = null;
            try
            {
              result = channel.GetEasyAuthorization();
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
    }

    #endregion

    #endregion

    #region 方法

    private static ChannelFactory<Phenix.Services.Contract.Wcf.IDataSecurity> GetChannelFactory()
    {
      return new ChannelFactory<Phenix.Services.Contract.Wcf.IDataSecurity>(WcfHelper.CreateBinding(),
        new EndpointAddress(WcfHelper.CreateUrl(NetConfig.ServicesAddress, ServicesInfo.DATA_SECURITY_URI)));
    }

    #region IDataSecurity 成员

    public IDictionary<string, RoleInfo> GetRoleInfos(UserIdentity identity)
    {
      NetConfig.InitializeSwitch();
      do
      {
        try
        {
          ChannelFactory<Phenix.Services.Contract.Wcf.IDataSecurity> channelFactory = GetChannelFactory();
          Phenix.Services.Contract.Wcf.IDataSecurity channel = channelFactory.CreateChannel();
          object result = null;
          try
          {
            result = channel.GetRoleInfos(identity);
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

    public IDictionary<string, RoleInfo> GetGrantRoleInfos(UserIdentity identity)
    {
      NetConfig.InitializeSwitch();
      do
      {
        try
        {
          ChannelFactory<Phenix.Services.Contract.Wcf.IDataSecurity> channelFactory = GetChannelFactory();
          Phenix.Services.Contract.Wcf.IDataSecurity channel = channelFactory.CreateChannel();
          object result = null;
          try
          {
            result = channel.GetGrantRoleInfos(identity);
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

    public IDictionary<string, SectionInfo> GetSectionInfos(UserIdentity identity)
    {
      NetConfig.InitializeSwitch();
      do
      {
        try
        {
          ChannelFactory<Phenix.Services.Contract.Wcf.IDataSecurity> channelFactory = GetChannelFactory();
          Phenix.Services.Contract.Wcf.IDataSecurity channel = channelFactory.CreateChannel();
          object result = null;
          try
          {
            result = channel.GetSectionInfos(identity);
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

    public IDictionary<string, IIdentity> GetIdentities(long departmentId, IList<long> positionIds, UserIdentity identity)
    {
      NetConfig.InitializeSwitch();
      do
      {
        try
        {
          ChannelFactory<Phenix.Services.Contract.Wcf.IDataSecurity> channelFactory = GetChannelFactory();
          Phenix.Services.Contract.Wcf.IDataSecurity channel = channelFactory.CreateChannel();
          object result = null;
          try
          {
            result = channel.GetIdentities(departmentId, positionIds, identity);
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
          return (IDictionary<string, IIdentity>)result;
        }
        catch (EndpointNotFoundException)
        {
          if (!NetConfig.SwitchServicesAddress())
            throw;
        }
      } while (true);
    }

    public DataSecurityContext CheckIn(UserIdentity identity, bool reset)
    {
      NetConfig.ServicesAddress = identity.ServicesAddress;
      NetConfig.InitializeSwitch();
      do
      {
        try
        {
          ChannelFactory<Phenix.Services.Contract.Wcf.IDataSecurity> channelFactory = GetChannelFactory();
          Phenix.Services.Contract.Wcf.IDataSecurity channel = channelFactory.CreateChannel();
          object result = null;
          try
          {
            result = channel.CheckInIdentity(identity, reset);
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
          return (DataSecurityContext)result;
        }
        catch (EndpointNotFoundException)
        {
          if (!NetConfig.SwitchServicesAddress())
            throw;
        }
      } while (true);
    }

    public DataSecurityContext CheckIn(string localAddress, string servicesAddress, string userNumber, string timestamp, string signature, bool reset)
    {
      NetConfig.ServicesAddress = servicesAddress;
      NetConfig.InitializeSwitch();
      do
      {
        try
        {
          ChannelFactory<Phenix.Services.Contract.Wcf.IDataSecurity> channelFactory = GetChannelFactory();
          Phenix.Services.Contract.Wcf.IDataSecurity channel = channelFactory.CreateChannel();
          object result = null;
          try
          {
            result = channel.CheckIn(localAddress, servicesAddress, userNumber, timestamp, signature, reset);
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
          return (DataSecurityContext)result;
        }
        catch (EndpointNotFoundException)
        {
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
          ChannelFactory<Phenix.Services.Contract.Wcf.IDataSecurity> channelFactory = GetChannelFactory();
          Phenix.Services.Contract.Wcf.IDataSecurity channel = channelFactory.CreateChannel();
          object result = null;
          try
          {
            result = channel.LogOnVerify(userNumber, tag);
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
          return (bool?)result;
        }
        catch (EndpointNotFoundException)
        {
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
          ChannelFactory<Phenix.Services.Contract.Wcf.IDataSecurity> channelFactory = GetChannelFactory();
          Phenix.Services.Contract.Wcf.IDataSecurity channel = channelFactory.CreateChannel();
          try
          {
            channel.LogOff(identity);
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
            return;
        }
      } while (true);
    }

    public bool ChangePassword(string newPassword, UserIdentity identity)
    {
      NetConfig.ServicesAddress = identity.ServicesAddress;
      NetConfig.InitializeSwitch();
      do
      {
        try
        {
          ChannelFactory<Phenix.Services.Contract.Wcf.IDataSecurity> channelFactory = GetChannelFactory();
          Phenix.Services.Contract.Wcf.IDataSecurity channel = channelFactory.CreateChannel();
          object result = null;
          try
          {
            result = channel.ChangePassword(newPassword, identity);
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

    public bool UnlockPassword(string userNumber, UserIdentity identity)
    {
      NetConfig.InitializeSwitch();
      do
      {
        try
        {
          ChannelFactory<Phenix.Services.Contract.Wcf.IDataSecurity> channelFactory = GetChannelFactory();
          Phenix.Services.Contract.Wcf.IDataSecurity channel = channelFactory.CreateChannel();
          object result = null;
          try
          {
            result = channel.UnlockPassword(userNumber, identity);
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

    public bool ResetPassword(string userNumber, UserIdentity identity)
    {
      NetConfig.InitializeSwitch();
      do
      {
        try
        {
          ChannelFactory<Phenix.Services.Contract.Wcf.IDataSecurity> channelFactory = GetChannelFactory();
          Phenix.Services.Contract.Wcf.IDataSecurity channel = channelFactory.CreateChannel();
          object result = null;
          try
          {
            result = channel.ResetPassword(userNumber, identity);
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

    public void SetProcessLockInfo(string processName, string caption, bool toLocked, TimeSpan expiryTime, string remark, UserIdentity identity)
    {
      NetConfig.InitializeSwitch();
      do
      {
        try
        {
          ChannelFactory<Phenix.Services.Contract.Wcf.IDataSecurity> channelFactory = GetChannelFactory();
          Phenix.Services.Contract.Wcf.IDataSecurity channel = channelFactory.CreateChannel();
          object result = null;
          try
          {
            result = channel.SetProcessLockInfo(processName, caption, toLocked, expiryTime, remark, identity);
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

    public ProcessLockInfo GetProcessLockInfo(string processName, UserIdentity identity)
    {
      NetConfig.InitializeSwitch();
      do
      {
        try
        {
          ChannelFactory<Phenix.Services.Contract.Wcf.IDataSecurity> channelFactory = GetChannelFactory();
          Phenix.Services.Contract.Wcf.IDataSecurity channel = channelFactory.CreateChannel();
          object result = null;
          try
          {
            result = channel.GetProcessLockInfo(processName, identity);
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
          return (ProcessLockInfo)result;
        }
        catch (EndpointNotFoundException)
        {
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