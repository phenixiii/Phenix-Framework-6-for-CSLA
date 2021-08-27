#if Top
using System.Collections.ObjectModel;
#endif

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Net;
using System.Text;
using System.Threading;
using Phenix.Core.Code;
using Phenix.Core.Data;
using Phenix.Core.Dictionary;
using Phenix.Core.Net;
using Phenix.Core.Security;
using Phenix.Core.Security.Cryptography;
using Phenix.Core.Security.Exception;
using Phenix.Core.SyncCollections;

namespace Phenix.Services.Library
{
  internal class DataSecurity : IDataSecurity
  {
    #region 属性

    public bool AllowUserMultipleAddressLogin
    {
      get { return AppConfig.AllowUserMultipleAddressLogin; }
    }

    public int SessionExpiresMinutes
    {
      get { return AppConfig.SessionExpiresMinutes; }
    }

    public bool EmptyRolesIsDeny
    {
      get { return GetEmptyRolesIsDeny(); }
    }

    public bool EasyAuthorization
    {
      get { return GetEasyAuthorization(); }
    }

    private static readonly byte[] _key = { 26, 77, 172, 42, 65, 62, 98, 52 };
    private static readonly byte[] _iv = { 70, 87, 73, 41, 58, 62, 71, 52 };

    private static readonly SynchronizedDictionary<string, UserIdentity> _userInfosCache =
      new SynchronizedDictionary<string, UserIdentity>(StringComparer.Ordinal);

    #endregion

    #region 方法

    private static string Encrypt(string text)
    {
      return Converter.BytesToHexString(DesCryptoTextProvider.Encrypt(_key, _iv, text));
    }

    private static string Decrypt(string text)
    {
      return DesCryptoTextProvider.Decrypt(_key, _iv, Converter.HexStringToBytes(text));
    }

    private static void CheckActive()
    {
      if (AppConfig.NoLogin && Phenix.Core.AppConfig.AutoMode)
        throw new InvalidOperationException(AppConfig.NoLoginReason);
    }

    public static bool GetEmptyRolesIsDeny()
    {
      return DefaultDatabase.ExecuteGet(ExecuteGetEmptyRolesIsDeny);
    }

    private static bool ExecuteGetEmptyRolesIsDeny(DbConnection connection)
    {
      using (DataReader reader = new DataReader(connection,
@"select SI_EmptyRolesIsDeny
  from PH_SystemInfo
  where SI_Name = :SI_Name",
        CommandBehavior.SingleRow, false))
      {
        reader.CreateParameter("SI_Name", Phenix.Core.AppConfig.SYSTEM_NAME);
        if (reader.Read())
          return reader.GetBooleanForDecimal(0);
      }
      return false;
    }

    internal static void SetEmptyRolesIsDeny(bool value)
    {
      DefaultDatabase.Execute(ExecuteSetEmptyRolesIsDeny, value);
    }

    private static void ExecuteSetEmptyRolesIsDeny(DbTransaction transaction, bool value)
    {
      using (DbCommand command = DbCommandHelper.CreateCommand(transaction,
@"update PH_SystemInfo set
    SI_EmptyRolesIsDeny = :SI_EmptyRolesIsDeny
  where SI_Name = :SI_Name"))
      {
        DbCommandHelper.CreateParameter(command, "SI_EmptyRolesIsDeny", value);
        DbCommandHelper.CreateParameter(command, "SI_Name", Phenix.Core.AppConfig.SYSTEM_NAME);
        DbCommandHelper.ExecuteNonQuery(command, false);
      }
    }

    internal static bool GetEasyAuthorization()
    {
      return DefaultDatabase.ExecuteGet(ExecuteGetEasyAuthorization);
    }

    private static bool ExecuteGetEasyAuthorization(DbConnection connection)
    {
      using (DataReader reader = new DataReader(connection,
@"select SI_EasyAuthorization
  from PH_SystemInfo
  where SI_Name = :SI_Name",
        CommandBehavior.SingleRow, false))
      {
        reader.CreateParameter("SI_Name", Phenix.Core.AppConfig.SYSTEM_NAME);
        if (reader.Read())
          return reader.GetBooleanForDecimal(0);
      }
      return false;
    }

    internal static void SetEasyAuthorization(bool value)
    {
      DefaultDatabase.Execute(ExecuteSetEasyAuthorization, value);
    }

    private static void ExecuteSetEasyAuthorization(DbTransaction transaction, bool value)
    {
      using (DbCommand command = DbCommandHelper.CreateCommand(transaction,
@"update PH_SystemInfo set
    SI_EasyAuthorization = :SI_EasyAuthorization
  where SI_Name = :SI_Name"))
      {
        DbCommandHelper.CreateParameter(command, "SI_EasyAuthorization", value);
        DbCommandHelper.CreateParameter(command, "SI_Name", Phenix.Core.AppConfig.SYSTEM_NAME);
        DbCommandHelper.ExecuteNonQuery(command, false);
      }
    }

    public IDictionary<string, RoleInfo> GetRoleInfos(UserIdentity identity)
    {
      return DefaultDatabase.ExecuteGet(ExecuteGetRoleInfos, identity);
    }

    private static IDictionary<string, RoleInfo> ExecuteGetRoleInfos(DbConnection connection, UserIdentity identity)
    {
      if (identity.IsAdmin)
        return DataDictionaryHub.RoleInfos;

      Dictionary<string, RoleInfo> result = new Dictionary<string, RoleInfo>(StringComparer.Ordinal);
      using (DataReader reader = new DataReader(connection,
@"select RL_Name, RL_Caption
  from PH_Role, PH_User_Role, PH_User
  where RL_ID = UR_RL_ID and UR_US_ID = US_ID and US_UserNumber = :US_UserNumber
  order by RL_Name",
        CommandBehavior.SingleResult, false))
      {
        reader.CreateParameter("US_UserNumber", identity.UserNumber);
        while (reader.Read())
          result.Add(reader.GetNullableString(0), new RoleInfo(reader.GetNullableString(0), reader.GetNullableString(1)));
      }
#if Top
      return new ReadOnlyDictionary<string, RoleInfo>(result);
#else
      return result;
#endif
    }

    public IDictionary<string, RoleInfo> GetGrantRoleInfos(UserIdentity identity)
    {
      return DefaultDatabase.ExecuteGet(ExecuteGetGrantRoleInfos, identity);
    }

    private static IDictionary<string, RoleInfo> ExecuteGetGrantRoleInfos(DbConnection connection, UserIdentity identity)
    {
      if (identity.IsAdmin)
        return DataDictionaryHub.RoleInfos;

      Dictionary<string, RoleInfo> result = new Dictionary<string, RoleInfo>(StringComparer.Ordinal);
      using (DataReader reader = new DataReader(connection,
@"select RL_Name, RL_Caption
  from PH_Role, PH_User_Grant_Role, PH_User
  where RL_ID = GR_RL_ID and GR_US_ID = US_ID and US_UserNumber = :US_UserNumber
  order by RL_Name",
        CommandBehavior.SingleResult, false))
      {
        reader.CreateParameter("US_UserNumber", identity.UserNumber);
        while (reader.Read())
          result.Add(reader.GetNullableString(0), new RoleInfo(reader.GetNullableString(0), reader.GetNullableString(1)));
      }
#if Top
      return new ReadOnlyDictionary<string, RoleInfo>(result);
#else
      return result;
#endif
    }

    public IDictionary<string, SectionInfo> GetSectionInfos(UserIdentity identity)
    {
      return DefaultDatabase.ExecuteGet(ExecuteGetSectionInfos, identity);
    }

    private static IDictionary<string, SectionInfo> ExecuteGetSectionInfos(DbConnection connection, UserIdentity identity)
    {
      if (identity.IsAdmin)
        return DataDictionaryHub.SectionInfos;

      Dictionary<string, SectionInfo> result = new Dictionary<string, SectionInfo>(StringComparer.Ordinal);
      using (DataReader reader = new DataReader(connection,
@"select ST_Name, ST_Caption
  from PH_Section, PH_User_Section, PH_User
  where ST_ID = US_ST_ID and PH_User_Section.US_US_ID = PH_User.US_ID and US_UserNumber = :US_UserNumber
  order by ST_Name",
        CommandBehavior.SingleResult, false))
      {
        reader.CreateParameter("US_UserNumber", identity.UserNumber);
        while (reader.Read())
          result.Add(reader.GetNullableString(0), new SectionInfo(reader.GetNullableString(0), reader.GetNullableString(1)));
      }
#if Top
      return new ReadOnlyDictionary<string, SectionInfo>(result);
#else
      return result;
#endif
    }

    public IDictionary<string, IIdentity> GetIdentities(long departmentId, IList<long> positionIds, UserIdentity identity)
    {
      return DefaultDatabase.ExecuteGet(ExecuteGetIdentities, departmentId, positionIds, identity);
    }

    private static IDictionary<string, IIdentity> ExecuteGetIdentities(DbConnection connection, long departmentId, IList<long> positionIds, UserIdentity identity)
    {
      Dictionary<string, IIdentity> result = new Dictionary<string, IIdentity>(StringComparer.Ordinal);
      StringBuilder whereSql = new StringBuilder();
      whereSql.AppendLine("US_Locked = 0 and US_DP_ID = :US_DP_ID");
      if (positionIds != null)
        foreach (long l in positionIds)
        {
          whereSql.Append("and US_PT_ID = ");
          whereSql.AppendLine(l.ToString());
        }
      using (DataReader reader = new DataReader(connection,
        String.Format(
@"select US_ID, US_Name, US_UserNumber, US_PT_ID
  from PH_User
  where {0}
  order by US_UserNumber",
          whereSql),
        CommandBehavior.SingleResult, false))
      {
        reader.CreateParameter("US_DP_ID", departmentId);
        while (reader.Read())
        {
          result.Add(reader.GetNullableString(2),
            UserIdentity.CreateUnauthenticated(reader.GetInt64ForDecimal(0), reader.GetNullableString(1), reader.GetNullableString(2), departmentId, reader.GetNullableInt64ForDecimal(3)));
        }
      }
#if Top
      return new ReadOnlyDictionary<string, IIdentity>(result);
#else
      return result;
#endif
    }

    private static bool CheckPasswordComplexityIsSimple(string userNumber, string password)
    {
      if (!AppConfig.RemindPasswordComplexity && !AppConfig.ForcedPasswordComplexity)
        return false;
      //判断与工号重复
      if (String.CompareOrdinal(userNumber, password) == 0)
        return true;
      //判断长度
      if (String.IsNullOrEmpty(password) || password.Length < AppConfig.PasswordLengthMinimize)
        return true;
      //判断复杂度
      int numberCount = 0; //数字个数
      int uppercaseCount = 0; //大写字母个数
      int lowcaseCount = 0; //小写字母个数
      int specialCount = 0; //特殊字符个数
      for (int i = 0; i < password.Length; i++)
      {
        if (password[i] >= 48 && password[i] <= 57)
          numberCount = numberCount + 1;
        else if (password[i] >= 65 && password[i] <= 90)
          uppercaseCount = uppercaseCount + 1;
        else if (password[i] >= 97 && password[i] <= 122)
          lowcaseCount = lowcaseCount + 1;
        else
          specialCount = specialCount + 1;
        int times = 0;
        if (numberCount > 0)
          times = times + 1;
        if (uppercaseCount > 0)
          times = times + 1;
        if (lowcaseCount > 0)
          times = times + 1;
        if (specialCount > 0)
          times = times + 1;
        if (times >= AppConfig.PasswordComplexityMinimize)
          return false;
      }
      return true;
    }

    private static bool CheckPasswordExpirationRemindDaysIsOver(DateTime passwordChangedTime)
    {
      if (AppConfig.PasswordExpirationRemindDays <= 0)
        return false;
      return passwordChangedTime.AddDays(AppConfig.PasswordExpirationRemindDays) < DateTime.Now;
    }

    private static bool CheckPasswordExpirationDaysIsOver(DateTime passwordChangedTime)
    {
      if (AppConfig.PasswordExpirationDays <= 0)
        return false;
      return passwordChangedTime.AddDays(AppConfig.PasswordExpirationDays) < DateTime.Now;
    }

    public DataSecurityContext CheckIn(UserIdentity identity, bool reset)
    {
      return DefaultDatabase.ExecuteGet(ExecuteCheckIn, identity.LocalAddress, identity.ServicesAddress, identity.UserNumber, identity.Timestamp, identity.Signature, false, reset);
    }

    public DataSecurityContext CheckIn(string localAddress, string servicesAddress, string userNumber, string timestamp, string signature, bool reset)
    {
      return DefaultDatabase.ExecuteGet(ExecuteCheckIn, localAddress, servicesAddress, userNumber, timestamp, signature, true, reset);
    }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
    private static DataSecurityContext ExecuteCheckIn(DbConnection connection, string localAddress, string servicesAddress, string userNumber, 
      string timestamp, string signature, bool allowTimestampRepeated, bool reset)
    {
      if (reset)
        CheckActive();

      UserIdentity identity;
      string userInfoKey = userNumber;
      string dynamicPassword = null;
      if (AppHub.Authoriser != null)
      {
        identity = AppHub.Authoriser.Translation(userNumber);
        if (identity != null)
        {
          userNumber = identity.UserNumber;
          dynamicPassword = identity.Password;
        }
      }

      DateTime passwordChangedTime = DateTime.MinValue;
      if (reset || !_userInfosCache.TryGetValue(userInfoKey, out identity) || identity.UserNumber != userNumber)
        if (UserIdentity.IsGuestUserNumber(userNumber) && signature == null)
          identity = UserIdentity.CreateGuest();
        else
        {
          using (SafeDataReader reader = new SafeDataReader(connection,
@"select US_ID, US_Name, US_Password, US_PasswordChangedTime, US_LoginFailureCount, US_LastOperationTime, US_DP_ID, US_PT_ID, US_Locked
  from PH_User
  where US_UserNumber = :US_UserNumber",
            CommandBehavior.SingleRow, false))
          {
            reader.CreateParameter("US_UserNumber", userNumber);
            if (reader.Read())
            {
              if (AppConfig.LoginFailureCountMaximum > 0 && reader.GetDecimal(4) >= AppConfig.LoginFailureCountMaximum)
                throw new UserLockedException(userNumber, Phenix.Services.Library.Properties.Resources.UserLoginFailureOverrunException);
              if (reader.GetBooleanForDecimal(8))
                throw new UserLockedException(userNumber);
              identity = UserIdentity.CreateAuthenticated(reader.GetInt64ForDecimal(0), reader.GetNullableString(1), userNumber, Decrypt(reader.GetNullableString(2)), dynamicPassword,
                AppUtilities.ServerVersion, localAddress, servicesAddress,
                signature != null && !reset ? reader.GetNullableDateTime(5) ?? DateTime.Now : DateTime.Now, reader.GetNullableInt64ForDecimal(6), reader.GetNullableInt64ForDecimal(7));
              passwordChangedTime = reader.GetNullableDateTime(3) ?? DateTime.MinValue;
            }
            else
              throw new UserNotFoundException(userNumber);
          }
          if (AppHub.Authoriser != null)
            try
            {
              bool? passed = AppHub.Authoriser.LogOn(identity.UserNumber, identity.DynamicPassword ?? identity.Password);
              if (passed.HasValue && !passed.Value)
                throw new UserVerifyException();
            }
            catch (Exception ex)
            {
              throw new UserVerifyException(ex);
            }
        }

      try
      {
        UserIdentity.CheckIn(identity, timestamp, signature, allowTimestampRepeated, reset);
        _userInfosCache[userInfoKey] = identity;
      }
      catch (UserVerifyException)
      {
        _userInfosCache.Remove(userInfoKey);
        if (reset)
        {
          using (DbCommand command = DbCommandHelper.CreateCommand(connection,
@"update PH_User set
    US_LoginFailure = sysdate,
    US_LoginFailureCount = US_LoginFailureCount + 1,
    US_LoginAddress = :US_LoginAddress
  where US_UserNumber = :US_UserNumber"))
          {
            DbCommandHelper.CreateParameter(command, "US_LoginAddress", localAddress);
            DbCommandHelper.CreateParameter(command, "US_UserNumber", userNumber);
            DbCommandHelper.ExecuteNonQuery(command, false);
          }
          throw;
        }
        ExecuteCheckIn(connection, localAddress, servicesAddress, userNumber, timestamp, signature, allowTimestampRepeated, true);
      }

      if (!identity.IsGuest)
      {
        if (AppConfig.SessionExpiresMinutes > 0)
        {
          if (_updateThread == null)
            lock (_updatingUsers)
              if (_updateThread == null)
              {
                _updateThread = new Thread(ExecuteUpdate);
                _updateThread.IsBackground = true;
                _updateThread.Start();
              }
          _updatingUsers.Enqueue(identity);
        }

        if (reset)
        {
          using (DbCommand command = DbCommandHelper.CreateCommand(connection,
@"update PH_User set
    US_Login = sysdate,
    US_LoginFailureCount = 0,
    US_LoginAddress = :US_LoginAddress,
    US_LastOperationTime = sysdate
  where US_UserNumber = :US_UserNumber"))
          {
            DbCommandHelper.CreateParameter(command, "US_LoginAddress", localAddress);
            DbCommandHelper.CreateParameter(command, "US_UserNumber", userNumber);
            DbCommandHelper.ExecuteNonQuery(command, false);
          }

          StringBuilder addresses = new StringBuilder();
          if (String.Compare(servicesAddress, NetConfig.EMBEDDED_SERVICE, StringComparison.OrdinalIgnoreCase) != 0 &&
            String.Compare(servicesAddress, NetConfig.LOCAL_HOST, StringComparison.OrdinalIgnoreCase) != 0 &&
            String.Compare(servicesAddress, NetConfig.LOCAL_IP, StringComparison.OrdinalIgnoreCase) != 0 &&
            String.Compare(localAddress, NetConfig.LocalAddress, StringComparison.OrdinalIgnoreCase) != 0)
          {
            bool succeed;
            using (DbCommand command = DbCommandHelper.CreateCommand(connection,
@"update PH_HostInfo set
    HI_LinkCount = HI_LinkCount + 1,
    HI_ActiveTime = sysdate
  where HI_Address = :HI_Address and HI_Name = :HI_Name"))
            {
              DbCommandHelper.CreateParameter(command, "HI_Address", servicesAddress);
              DbCommandHelper.CreateParameter(command, "HI_Name", Dns.GetHostName());
              succeed = DbCommandHelper.ExecuteNonQuery(command, false) != 0;
            }
            if (!succeed)
            {
              using (DbCommand command = DbCommandHelper.CreateCommand(connection,
@"insert into PH_HostInfo
  (HI_Address, HI_Name, HI_Active, HI_LinkCount, HI_ActiveTime)
  values
  (:HI_Address, :HI_Name, 1, 1, sysdate)"))
              {
                DbCommandHelper.CreateParameter(command, "HI_Address", servicesAddress);
                DbCommandHelper.CreateParameter(command, "HI_Name", Dns.GetHostName());
                DbCommandHelper.ExecuteNonQuery(command, false);
              }
            }

            using (DataReader reader = new DataReader(connection,
@"select HI_Address
  from PH_HostInfo
  where HI_Address like :HI_Address and HI_Active = 1 and HI_ActiveTime > sysdate - 1
  order by HI_LinkCount",
              CommandBehavior.SingleResult, false))
            {
              string s = servicesAddress; //127.0.0.1
              if (AppConfig.IpSegmentCount >= 4)
                reader.CreateParameter("HI_Address", s); //127.0.0.1
              else if (AppConfig.IpSegmentCount >= 1)
              {
                for (int i = 1; i <= 4 - AppConfig.IpSegmentCount; i++)
                {
                  int j = s.LastIndexOf('.');
                  if (j >= 0)
                    s = s.Remove(j);
                }
                reader.CreateParameter("HI_Address", s + ".%"); //127.0.0.%
              }
              else
                reader.CreateParameter("HI_Address", "%.%"); //%.%
              while (reader.Read())
              {
                addresses.Append(reader.GetNullableString(0));
                addresses.Append(Phenix.Core.AppConfig.SEPARATOR);
              }
            }
          }
          if (CheckPasswordComplexityIsSimple(identity.UserNumber, identity.Password))
            if (Phenix.Services.Library.AppConfig.ForcedPasswordComplexity)
              throw new UserPasswordComplexityException(identity, Phenix.Services.Library.AppConfig.PasswordLengthMinimize, Phenix.Services.Library.AppConfig.PasswordComplexityMinimize);
            else
              return new DataSecurityContext(identity, addresses.ToString(),
                String.Format(Phenix.Core.Properties.Resources.PasswordComplexityReminder, Phenix.Services.Library.AppConfig.PasswordLengthMinimize, Phenix.Services.Library.AppConfig.PasswordComplexityMinimize));
          if (CheckPasswordExpirationRemindDaysIsOver(passwordChangedTime))
            return new DataSecurityContext(identity, addresses.ToString(),
              String.Format(Phenix.Services.Library.Properties.Resources.PasswordExpirationReminder, AppConfig.PasswordExpirationRemindDays));
          if (CheckPasswordExpirationDaysIsOver(passwordChangedTime))
            throw new UserLockedException(userNumber, String.Format(Phenix.Services.Library.Properties.Resources.PasswordExpirationReminder, AppConfig.PasswordExpirationDays));
          return new DataSecurityContext(identity, addresses.ToString());
        }
      }
      return new DataSecurityContext(identity);
    }

    private static Thread _updateThread;
    private static readonly SynchronizedQueue<UserIdentity> _updatingUsers = new SynchronizedQueue<UserIdentity>(256);

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
    private static void ExecuteUpdate()
    {
      try
      {
        Dictionary<string, UserIdentity> users = new Dictionary<string, UserIdentity>(StringComparer.Ordinal);
        while (true)
          try
          {
            if (_updatingUsers.Count > 0)
            {
              DateTime dt = DateTime.Now;
              while (DateTime.Now.Subtract(dt).TotalMinutes <= 1)
              {
                while (_updatingUsers.Count > 0)
                {
                  UserIdentity userIdentity = _updatingUsers.Dequeue();
                  users[userIdentity.UserNumber] = userIdentity;
                }
                Thread.Sleep(100);
              }
              DefaultDatabase.Execute(ExecuteUpdate, users);
              users.Clear();
            }
            Thread.Sleep(1000);
          }
          catch (ObjectDisposedException)
          {
            return;
          }
          catch (ThreadAbortException)
          {
            Thread.ResetAbort();
            return;
          }
          catch (Exception)
          {
            Thread.Sleep(1000);
          }
      }
      finally
      {
        _updateThread = null;
      }
    }

    private static void ExecuteUpdate(DbConnection connection, IDictionary<string, UserIdentity> users)
    {
      using (DbCommand command = DbCommandHelper.CreateCommand(connection,
@"update PH_User set
    US_LastOperationTime = :US_LastOperationTime
  where US_UserNumber = :US_UserNumber"))
      {
        foreach (KeyValuePair<string, UserIdentity> kvp in users)
        {
          DbCommandHelper.CreateParameter(command, "US_LastOperationTime", kvp.Value.LastOperationTime);
          DbCommandHelper.CreateParameter(command, "US_UserNumber", kvp.Key);
          DbCommandHelper.ExecuteNonQuery(command, false);
        }
      }
    }

    public bool? LogOnVerify(string userNumber, string tag)
    {
      return AppHub.Authoriser != null ? AppHub.Authoriser.LogOnVerify(userNumber, tag) : null;
    }

    public void LogOff(UserIdentity identity)
    {
      DefaultDatabase.Execute(ExecuteLogOff, identity.LocalAddress, identity.ServicesAddress, identity.UserNumber);
    }

    private static void ExecuteLogOff(DbTransaction transaction, string localAddress, string servicesAddress, string userNumber)
    {
      string userInfoKey = userNumber;
      if (AppHub.Authoriser != null)
      {
        UserIdentity identity = AppHub.Authoriser.Translation(userNumber);
        if (identity != null)
          userNumber = identity.UserNumber;
      }
      using (DbCommand command = DbCommandHelper.CreateCommand(transaction,
@"update PH_User set
    US_Logout = :US_Logout
  where US_UserNumber = :US_UserNumber"))
      {
        DbCommandHelper.CreateParameter(command, "US_Logout", DateTime.Now);
        DbCommandHelper.CreateParameter(command, "US_UserNumber", userNumber);
        DbCommandHelper.ExecuteNonQuery(command, false);
      }
      if (AppConfig.NeedMarkLogin)
        using (DbCommand command = DbCommandHelper.CreateCommand(transaction,
@"insert into PH_UserLog
  (US_ID, US_UserNumber, US_Login, US_Logout, US_LoginAddress)
  select US_ID, US_UserNumber, US_Login, US_Logout, US_LoginAddress
  from PH_User
  where US_UserNumber = :US_UserNumber"))
        {
          DbCommandHelper.CreateParameter(command, "US_UserNumber", userNumber);
          DbCommandHelper.ExecuteNonQuery(command, false);
        }
      if (String.Compare(servicesAddress, NetConfig.EMBEDDED_SERVICE, StringComparison.OrdinalIgnoreCase) != 0 &&
        String.Compare(servicesAddress, NetConfig.LOCAL_HOST, StringComparison.OrdinalIgnoreCase) != 0 &&
        String.Compare(servicesAddress, NetConfig.LOCAL_IP, StringComparison.OrdinalIgnoreCase) != 0 &&
        String.Compare(localAddress, NetConfig.LocalAddress, StringComparison.OrdinalIgnoreCase) != 0)
      {
        using (DbCommand command = DbCommandHelper.CreateCommand(transaction,
@"update PH_HostInfo set
    HI_LinkCount = HI_LinkCount - 1,
    HI_ActiveTime = sysdate
  where HI_Address = :HI_Address and HI_Name = :HI_Name"))
        {
          DbCommandHelper.CreateParameter(command, "HI_Address", servicesAddress);
          DbCommandHelper.CreateParameter(command, "HI_Name", Dns.GetHostName());
          DbCommandHelper.ExecuteNonQuery(command, false);
        }
      }
      _userInfosCache.Remove(userInfoKey);
    }

    public bool ChangePassword(string newPassword, UserIdentity identity)
    {
      string userInfoKey = identity.UserNumber;
      try
      {
        DataSecurityContext context = CheckIn(identity, true);
        identity = context.Identity;
      }
      catch (UserPasswordComplexityException ex)
      {
        identity = ex.Identity;
      }
      if (!identity.IsAuthenticated || identity.IsGuest)
        return false;
      newPassword = RijndaelCryptoTextProvider.Decrypt(identity.DynamicPassword ?? identity.Password, newPassword);
      return DefaultDatabase.ExecuteGet(ExecuteChangePassword, userInfoKey, identity.UserNumber, newPassword, true, true);
    }

    private static bool ExecuteChangePassword(DbTransaction transaction, string userInfoKey, string userNumber, string newPassword, 
      bool forcedPasswordComplexity, bool needChangeAuthoriser)
    {
      if (forcedPasswordComplexity && Phenix.Services.Library.AppConfig.ForcedPasswordComplexity)
        if (CheckPasswordComplexityIsSimple(userNumber, newPassword))
          throw new UserPasswordComplexityException(Phenix.Services.Library.AppConfig.PasswordLengthMinimize, Phenix.Services.Library.AppConfig.PasswordComplexityMinimize);

      bool result;
      using (DbCommand command = DbCommandHelper.CreateCommand(transaction,
@"update PH_User set
    US_Password = :US_Password,
    US_PasswordChangedTime = sysdate,
    US_LoginFailureCount = 0
  where US_UserNumber = :US_UserNumber"))
      {
        DbCommandHelper.CreateParameter(command, "US_Password", Encrypt(newPassword));
        DbCommandHelper.CreateParameter(command, "US_UserNumber", userNumber);
        result = DbCommandHelper.ExecuteNonQuery(command, false) == 1;
      }

      _userInfosCache.Remove(userInfoKey);

      if (result && needChangeAuthoriser)
          if (AppHub.Authoriser != null)
            if (!AppHub.Authoriser.ChangePassword(userNumber, newPassword))
              return false;
      return result;
    }

    public bool UnlockPassword(string userNumber, UserIdentity identity)
    {
      DataSecurityContext context = CheckIn(identity, false);
      if (!context.Identity.IsAuthenticated || context.Identity.IsGuest)
        return false;
      if (AppHub.Authoriser != null)
      {
        identity = AppHub.Authoriser.Translation(userNumber);
        if (identity != null)
          userNumber = identity.UserNumber;
      }
      if (!context.Identity.HaveAdminRole && !context.Identity.HaveSubordinate(userNumber))
        throw new UserVerifyException(Phenix.Services.Library.Properties.Resources.UnlockPasswordException);
      return DefaultDatabase.ExecuteGet(ExecuteUnlockPassword, userNumber);
    }

    private bool ExecuteUnlockPassword(DbConnection connection, string userNumber)
    {
      using (DbCommand command = DbCommandHelper.CreateCommand(connection,
@"update PH_User set
    US_LoginFailureCount = 0
  where US_UserNumber = :US_UserNumber"))
      {
        DbCommandHelper.CreateParameter(command, "US_UserNumber", userNumber);
        return DbCommandHelper.ExecuteNonQuery(command, false) == 1;
      }
    }

    public bool ResetPassword(string userNumber, UserIdentity identity)
    {
      DataSecurityContext context = CheckIn(identity, false);
      if (!context.Identity.IsAuthenticated || context.Identity.IsGuest)
        return false;
      string userInfoKey = userNumber;
      if (AppHub.Authoriser != null)
      {
        identity = AppHub.Authoriser.Translation(userNumber);
        if (identity != null)
          userNumber = identity.UserNumber;
      }
      if (String.CompareOrdinal(userNumber, context.Identity.UserNumber) != 0 && !context.Identity.HaveAdminRole && !context.Identity.HaveSubordinate(userNumber))
        throw new UserVerifyException(Phenix.Services.Library.Properties.Resources.ResetPasswordException);
      return DefaultDatabase.ExecuteGet(ExecuteChangePassword, userInfoKey, userNumber, userNumber, false, true);
    }

    public void SetProcessLockInfo(string processName, string caption, bool toLocked, TimeSpan expiryTime, string remark, UserIdentity identity)
    {
      DefaultDatabase.Execute(ExecuteSetProcessLockInfo, processName, caption, toLocked, expiryTime, remark, identity);
    }

    private static void ExecuteSetProcessLockInfo(DbTransaction transaction, string processName, string caption, bool toLocked, TimeSpan expiryTime, string remark, UserIdentity identity)
    {
      bool succeed;
      using (DbCommand command = DbCommandHelper.CreateCommand(transaction,
        String.Format(
@"update PH_ProcessLock set
    {0}
    PL_AllowExecute = :PL_AllowExecute,
    PL_Time = :PL_Time,
    {1}
    PL_UserNumber = :New_PL_UserNumber,
    PL_Remark = :PL_Remark
  where PL_Name = :PL_Name
    and (PL_AllowExecute = 0 or PL_ExpiryTime < sysdate {2})",
        toLocked ? " PL_Caption = :PL_Caption," : String.Empty,
        toLocked ? " PL_ExpiryTime = :PL_ExpiryTime," : String.Empty,
        !identity.HaveAdminRole ? " or PL_UserNumber = :Old_PL_UserNumber" : String.Empty)))
      {
        if (toLocked)
          DbCommandHelper.CreateParameter(command, "PL_Caption", caption);
        DbCommandHelper.CreateParameter(command, "PL_AllowExecute", toLocked);
        DbCommandHelper.CreateParameter(command, "PL_Time", DateTime.Now);
        if (toLocked)
          if (expiryTime == TimeSpan.Zero)
            DbCommandHelper.CreateParameter(command, "PL_ExpiryTime", null);
          else
            DbCommandHelper.CreateParameter(command, "PL_ExpiryTime", DateTime.Now.Add(expiryTime));
        DbCommandHelper.CreateParameter(command, "New_PL_UserNumber", identity.UserNumber);
        DbCommandHelper.CreateParameter(command, "PL_Remark", remark);
        DbCommandHelper.CreateParameter(command, "PL_Name", processName);
        if (!identity.HaveAdminRole)
          DbCommandHelper.CreateParameter(command, "Old_PL_UserNumber", identity.UserNumber);
        succeed = DbCommandHelper.ExecuteNonQuery(command, false) != 0;
      }
      if (!succeed)
      {
        ProcessLockInfo info = ExecuteGetProcessLockInfo(transaction, processName);
        if (info != null)
        {
          if (!identity.AllowSetProcessLockInfo(info))
            throw new ProcessLockException(info);
        }
        else if (toLocked)
        {
          using (DbCommand command = DbCommandHelper.CreateCommand(transaction,
@"insert into PH_ProcessLock
  (PL_Name, PL_Caption, PL_AllowExecute, PL_Time, PL_ExpiryTime, PL_UserNumber, PL_Remark)
  values
  (:PL_Name, :PL_Caption, :PL_AllowExecute, :PL_Time, :PL_ExpiryTime, :PL_UserNumber, :PL_Remark)"))
          {
            DbCommandHelper.CreateParameter(command, "PL_Name", processName);
            DbCommandHelper.CreateParameter(command, "PL_Caption", caption);
            DbCommandHelper.CreateParameter(command, "PL_AllowExecute", toLocked);
            DbCommandHelper.CreateParameter(command, "PL_Time", DateTime.Now);
            if (expiryTime == TimeSpan.Zero)
              DbCommandHelper.CreateParameter(command, "PL_ExpiryTime", null);
            else
              DbCommandHelper.CreateParameter(command, "PL_ExpiryTime", DateTime.Now.Add(expiryTime));
            DbCommandHelper.CreateParameter(command, "PL_UserNumber", identity.UserNumber);
            DbCommandHelper.CreateParameter(command, "PL_Remark", remark);
            DbCommandHelper.ExecuteNonQuery(command, false);
          }
        }
      }
    }

    public ProcessLockInfo GetProcessLockInfo(string processName, UserIdentity identity)
    {
      return DefaultDatabase.ExecuteGet(ExecuteGetProcessLockInfo, processName);
    }

    private static ProcessLockInfo ExecuteGetProcessLockInfo(DbTransaction transaction, string processName)
    {
      using (DataReader reader = new DataReader(transaction, 
@"select PL_Caption, PL_AllowExecute, PL_Time,
  PL_ExpiryTime, PL_UserNumber, PL_Remark
  from PH_ProcessLock
  where PL_Name = :PL_Name",
        CommandBehavior.SingleRow, false))
      {
        reader.CreateParameter("PL_Name", processName);
        if (reader.Read())
          return new ProcessLockInfo(processName, reader.GetNullableString(0), reader.GetBooleanForDecimal(1), reader.GetDateTime(2),
            reader.IsDBNull(3) ? DateTime.MaxValue : reader.GetDateTime(3), 
            reader.GetNullableString(4), reader.GetNullableString(5));
      }
      return null;
    }

    #region 代入数据库事务

    public void AddUser(DbTransaction transaction, long id, string userName, string userNumber, string password)
    {
      ExecuteAddUser(transaction, id, userName, userNumber, password);
    }

    private static void ExecuteAddUser(DbTransaction transaction, long id, string userName, string userNumber, string password)
    {
      if (AppHub.Authoriser != null)
      {
        UserIdentity identity = AppHub.Authoriser.Translation(userNumber);
        if (identity != null)
          userNumber = identity.UserNumber;
      }
      using (DbCommand command = DbCommandHelper.CreateCommand(transaction,
@"insert into PH_User
  (US_ID, US_Name, US_UserNumber, US_Password)
  values
  (:US_ID, :US_Name, :US_UserNumber, :US_Password)"))
      {
        DbCommandHelper.CreateParameter(command, "US_ID", id);
        DbCommandHelper.CreateParameter(command, "US_Name", userName);
        DbCommandHelper.CreateParameter(command, "US_UserNumber", userNumber);
        DbCommandHelper.CreateParameter(command, "US_Password", Encrypt(password));
        DbCommandHelper.ExecuteNonQuery(command, false);
      }
    }

    public bool ChangePassword(DbTransaction transaction, string userNumber, string newPassword)
    {
      string userInfoKey = userNumber;
      if (AppHub.Authoriser != null)
      {
        UserIdentity identity = AppHub.Authoriser.Translation(userNumber);
        if (identity != null)
          userNumber = identity.UserNumber;
      }
      return ExecuteChangePassword(transaction, userInfoKey, userNumber, newPassword, false, true);
    }

    #endregion

    #endregion
  }
}