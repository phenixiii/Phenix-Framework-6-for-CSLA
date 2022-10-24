using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Reflection;
using Phenix.Core.Data;
using Phenix.Core.Dictionary;
using Phenix.Core.Log;
using Phenix.Core.Mapping;
using Phenix.Core.Security.Cryptography;

namespace Phenix.Core.Security
{
  /// <summary>
  /// 数据安全中心
  /// </summary>
  public static class DataSecurityHub
  {
    #region 属性

    private static IDataSecurity _worker;
    /// <summary>
    /// 实施者
    /// </summary>
    public static IDataSecurity Worker
    {
      get
      {
        if (_worker == null)
          AppUtilities.RegisterWorker();
        return _worker;
      }
      set { _worker = value; }
    }

    /// <summary>
    /// 允许用户多IP登录
    /// 不控制Guest用户
    /// </summary>
    public static bool AllowUserMultipleAddressLogin
    {
      get
      {
        CheckActive();
        return Worker.AllowUserMultipleAddressLogin;
      }
    }

    /// <summary>
    /// 会话过期时间(分钟)
    /// 小于等于0不限制
    /// 不控制Guest用户
    /// </summary>
    public static int SessionExpiresMinutes
    {
      get
      {
        CheckActive();
        return Worker.SessionExpiresMinutes;
      }
    }

    /// <summary>
    /// 未配置角色代表授权规则为禁用
    /// </summary>
    public static bool EmptyRolesIsDeny
    {
      get
      {
        CheckActive();
        return Worker.EmptyRolesIsDeny;
      }
    }

    /// <summary>
    /// 宽松的授权
    /// true: 只要用户拥有的角色之一不在禁用角色队列里，就不被禁用
    /// false: 只要用户拥有的角色之一在禁用角色队列里，就被禁用
    /// </summary>
    public static bool EasyAuthorization
    {
      get
      {
        CheckActive();
        return Worker.EasyAuthorization;
      }
    }

    #endregion

    #region 方法

    /// <summary>
    /// 检查活动
    /// </summary>
    public static void CheckActive()
    {
      if (Worker == null)
      {
        System.Exception ex = new InvalidOperationException(Phenix.Core.Properties.Resources.NoService);
        EventLog.SaveLocal(MethodBase.GetCurrentMethod(), ex);
        throw ex;
      }
    }

    /// <summary>
    /// 取角色资料队列
    /// identity = Phenix.Core.Security.UserIdentity.CurrentIdentity
    /// </summary>
    public static IDictionary<string, RoleInfo> GetRoleInfos()
    {
      return GetRoleInfos(UserIdentity.CurrentIdentity);
    }

    /// <summary>
    /// 取角色资料队列
    /// </summary>
    [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
    public static IDictionary<string, RoleInfo> GetRoleInfos(UserIdentity identity)
    {
      identity = identity ?? UserIdentity.CurrentIdentity;
      CheckActive();
      return Worker.GetRoleInfos(identity);
    }

    /// <summary>
    /// 取可授权角色资料队列
    /// identity = Phenix.Core.Security.UserIdentity.CurrentIdentity
    /// </summary>
    public static IDictionary<string, RoleInfo> GetGrantRoleInfos()
    {
      return GetGrantRoleInfos(UserIdentity.CurrentIdentity);
    }

    /// <summary>
    /// 取可授权角色资料队列
    /// </summary>
    [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
    public static IDictionary<string, RoleInfo> GetGrantRoleInfos(UserIdentity identity)
    {
      identity = identity ?? UserIdentity.CurrentIdentity;
      CheckActive();
      return Worker.GetGrantRoleInfos(identity);
    }

    /// <summary>
    /// 取切片资料队列
    /// identity = Phenix.Core.Security.UserIdentity.CurrentIdentity
    /// </summary>
    public static IDictionary<string, SectionInfo> GetSectionInfos()
    {
      return GetSectionInfos(UserIdentity.CurrentIdentity);
    }

    /// <summary>
    /// 取切片资料队列
    /// </summary>
    [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
    public static IDictionary<string, SectionInfo> GetSectionInfos(UserIdentity identity)
    {
      identity = identity ?? UserIdentity.CurrentIdentity;
      CheckActive();
      return Worker.GetSectionInfos(identity);
    }

    /// <summary>
    /// 取部门岗位的用户身份
    /// identity = Phenix.Core.Security.UserIdentity.CurrentIdentity
    /// </summary>
    public static IDictionary<string, IIdentity> GetIdentities(DepartmentInfo department, IList<PositionInfo> positions)
    {
      return GetIdentities(department, positions, UserIdentity.CurrentIdentity);
    }

    /// <summary>
    /// 取部门岗位的用户身份
    /// </summary>
    [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
    public static IDictionary<string, IIdentity> GetIdentities(DepartmentInfo department, IList<PositionInfo> positions, UserIdentity identity)
    {
      List<long> positionIds = null;
      if (positions != null)
      {
        positionIds = new List<long>(positions.Count);
        foreach (PositionInfo item in positions)
          positionIds.Add(item.Id);
      }
      return GetIdentities(department.Id, positionIds, identity);
    }

    /// <summary>
    /// 取部门岗位的用户身份
    /// </summary>
    [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
    public static IDictionary<string, IIdentity> GetIdentities(long departmentId, IList<long> positionIds, UserIdentity identity)
    {
      identity = identity ?? UserIdentity.CurrentIdentity;
      CheckActive();
      return Worker.GetIdentities(departmentId, positionIds, identity);
    }

    /// <summary>
    /// CheckIn
    /// </summary>
    public static DataSecurityContext CheckIn(Type objectType, ExecuteAction action, System.Security.Principal.IPrincipal principal)
    {
      return CheckIn(objectType, action, principal.Identity as UserIdentity);
    }

    /// <summary>
    /// CheckIn
    /// </summary>
    public static DataSecurityContext CheckIn(Type objectType, ExecuteAction action, UserIdentity identity)
    {
      DataSecurityContext result = CheckIn(identity, false);
      return !UserIdentity.IsByDeny(result.Identity, objectType, action, true) ? result : null;
    }

    /// <summary>
    /// CheckIn
    /// </summary>
    public static DataSecurityContext CheckIn(UserIdentity identity, bool reset)
    {
      identity = identity ?? UserIdentity.CurrentIdentity;
      if (identity == null)
        throw new UserVerifyException();
      DataSecurityContext result = Worker.CheckIn(identity, reset);
      if (result.Identity.Password == null)
        result.Identity.Password = identity.Password;
      if (result.Identity.DynamicPassword == null)
        result.Identity.DynamicPassword = identity.DynamicPassword;
      UserIdentity.CurrentIdentity = result.Identity;
      return result;
    }

    /// <summary>
    /// CheckIn
    /// </summary>
    [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
    public static DataSecurityContext CheckIn(string localAddress, string servicesAddress, string userNumber, string timestamp, string signature, bool reset)
    {
      CheckActive();
      DataSecurityContext result = Worker.CheckIn(localAddress, servicesAddress, userNumber, timestamp, signature, reset);
      UserIdentity.CurrentIdentity = result.Identity;
      return result;
    }

    /// <summary>
    /// 登录核实
    /// </summary>
    public static bool? LogOnVerify(string userNumber, string tag)
    {
      CheckActive();
      return Worker.LogOnVerify(userNumber, tag);
    }

    /// <summary>
    /// 登出
    /// </summary>
    public static void LogOff(UserIdentity identity)
    {
      identity = identity ?? UserIdentity.CurrentIdentity;
      CheckActive();
      Worker.LogOff(identity);
    }

    /// <summary>
    /// 修改登录口令
    /// </summary>
    public static bool ChangePassword(string newPassword, UserIdentity identity)
    {
      return ChangePassword(newPassword, true, identity);
    }

    /// <summary>
    /// 修改登录口令
    /// </summary>
    [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
    public static bool ChangePassword(string newPassword, bool inEncrypt, UserIdentity identity)
    {
      identity = identity ?? UserIdentity.CurrentIdentity;
      CheckActive();
      bool result = Worker.ChangePassword(inEncrypt ? RijndaelCryptoTextProvider.Encrypt(identity.DynamicPassword ?? identity.Password, newPassword) : newPassword, identity);
      if (result)
      {
        identity.Password = newPassword;
        UserIdentity.CurrentIdentity = identity;
      }
      return result;
    }

    /// <summary>
    /// 解锁登录口令
    /// </summary>
    public static bool UnlockPassword(string userNumber, UserIdentity identity)
    {
      identity = identity ?? UserIdentity.CurrentIdentity;
      CheckActive();
      return Worker.UnlockPassword(userNumber, identity);
    }

    /// <summary>
    /// 重置登录口令
    /// </summary>
    public static bool ResetPassword(string userNumber, UserIdentity identity)
    {
      identity = identity ?? UserIdentity.CurrentIdentity;
      CheckActive();
      return Worker.ResetPassword(userNumber, identity);
    }

    /// <summary>
    /// 设置过程锁资料
    /// identity = Phenix.Core.Security.UserIdentity.CurrentIdentity
    /// </summary>
    public static void SetProcessLockInfo(string processName, string caption, bool toLocked, TimeSpan expiryTime, string remark)
    {
      SetProcessLockInfo(processName, caption, toLocked, expiryTime, remark, UserIdentity.CurrentIdentity);
    }

    /// <summary>
    /// 设置过程锁资料
    /// </summary>
    [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
    public static void SetProcessLockInfo(string processName, string caption, bool toLocked, TimeSpan expiryTime, string remark, UserIdentity identity)
    {
      identity = identity ?? UserIdentity.CurrentIdentity;
      CheckActive();
      Worker.SetProcessLockInfo(processName, caption, toLocked, expiryTime, remark, identity);
    }

    /// <summary>
    /// 取过程锁资料
    /// identity = Phenix.Core.Security.UserIdentity.CurrentIdentity
    /// </summary>
    public static ProcessLockInfo GetProcessLockInfo(string processName)
    {
      return GetProcessLockInfo(processName, UserIdentity.CurrentIdentity);
    }

    /// <summary>
    /// 取过程锁资料
    /// </summary>
    [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
    public static ProcessLockInfo GetProcessLockInfo(string processName, UserIdentity identity)
    {
      identity = identity ?? UserIdentity.CurrentIdentity;
      CheckActive();
      return Worker.GetProcessLockInfo(processName, identity);
    }

    #region 代入数据库事务

    /// <summary>
    /// 添加用户
    /// </summary>
    public static void AddUser(DbTransaction transaction, string userName, string userNumber, string password)
    {
      AddUser(transaction, Sequence.Value, userName, userNumber, password);
    }

    /// <summary>
    /// 添加用户
    /// </summary>
    public static void AddUser(DbTransaction transaction, long id, string userName, string userNumber, string password)
    {
      CheckActive();
      Worker.AddUser(transaction, id, userName, userNumber, password);
    }

    /// <summary>
    /// 重置登录口令
    /// </summary>
    /// <returns>找到登录工号并重置成功</returns>
    public static bool ChangePassword(DbTransaction transaction, string userNumber, string newPassword)
    {
      CheckActive();
      return Worker.ChangePassword(transaction, userNumber, newPassword);
    }

    #endregion

    #endregion
  }
}
