using System;
using System.Collections.Generic;
using System.Data.Common;
using Phenix.Core.Dictionary;

namespace Phenix.Core.Security
{
  /// <summary>
  /// 数据安全接口
  /// </summary>
  public interface IDataSecurity
  {
    #region 属性

    /// <summary>
    /// 允许用户多IP登录
    /// </summary>
    bool AllowUserMultipleAddressLogin { get; }

    /// <summary>
    /// 会话过期时间(分钟)
    /// 小于等于0不限制
    /// </summary>
    int SessionExpiresMinutes { get; }

    /// <summary>
    /// 未配置角色代表授权规则为禁用
    /// </summary>
    bool EmptyRolesIsDeny { get; }

    /// <summary>
    /// 宽松的授权
    /// true: 只要用户拥有的角色之一不在禁用角色队列里，就不被禁用
    /// false: 只要用户拥有的角色之一在禁用角色队列里，就被禁用
    /// </summary>
    bool EasyAuthorization { get; }

    #endregion

    #region 方法

    /// <summary>
    /// 取角色资料队列
    /// </summary>
    IDictionary<string, RoleInfo> GetRoleInfos(UserIdentity identity);

    /// <summary>
    /// 取可授权角色资料队列
    /// </summary>
    IDictionary<string, RoleInfo> GetGrantRoleInfos(UserIdentity identity);

    /// <summary>
    /// 取切片资料队列
    /// </summary>
    IDictionary<string, SectionInfo> GetSectionInfos(UserIdentity identity);

    /// <summary>
    /// 取部门岗位的用户身份
    /// </summary>
    IDictionary<string, IIdentity> GetIdentities(long departmentId, IList<long> positionIds, UserIdentity identity);

    /// <summary>
    /// CheckIn
    /// </summary>
    DataSecurityContext CheckIn(UserIdentity identity, bool reset);

    /// <summary>
    /// CheckIn
    /// </summary>
    DataSecurityContext CheckIn(string localAddress, string servicesAddress, string userNumber, string timestamp, string signature, bool reset);

    /// <summary>
    /// 登录核实
    /// </summary>
    bool? LogOnVerify(string userNumber, string tag);

    /// <summary>
    /// 登出
    /// </summary>
    void LogOff(UserIdentity identity);

    /// <summary>
    /// 修改登录口令
    /// </summary>
    bool ChangePassword(string newPassword, UserIdentity identity);

    /// <summary>
    /// 解锁登录口令
    /// </summary>
    bool UnlockPassword(string userNumber, UserIdentity identity);

    /// <summary>
    /// 重置登录口令
    /// </summary>
    bool ResetPassword(string userNumber, UserIdentity identity);

    /// <summary>
    /// 设置过程锁资料
    /// </summary>
    void SetProcessLockInfo(string processName, string caption, bool toLocked, TimeSpan expiryTime, string remark, UserIdentity identity);

    /// <summary>
    /// 提取过程锁资料
    /// </summary>
    ProcessLockInfo GetProcessLockInfo(string processName, UserIdentity identity);

    #region 代入数据库事务

    /// <summary>
    /// 添加用户
    /// </summary>
    void AddUser(DbTransaction transaction, long id, string userName, string userNumber, string password);

    /// <summary>
    /// 重置登录口令
    /// </summary>
    bool ChangePassword(DbTransaction transaction, string userNumber, string newPassword);

    #endregion

    #endregion
  }
}