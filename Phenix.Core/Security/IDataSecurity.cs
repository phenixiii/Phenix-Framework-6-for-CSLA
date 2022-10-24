using System;
using System.Collections.Generic;
using System.Data.Common;
using Phenix.Core.Dictionary;

namespace Phenix.Core.Security
{
  /// <summary>
  /// ���ݰ�ȫ�ӿ�
  /// </summary>
  public interface IDataSecurity
  {
    #region ����

    /// <summary>
    /// �����û���IP��¼
    /// </summary>
    bool AllowUserMultipleAddressLogin { get; }

    /// <summary>
    /// �Ự����ʱ��(����)
    /// С�ڵ���0������
    /// </summary>
    int SessionExpiresMinutes { get; }

    /// <summary>
    /// δ���ý�ɫ������Ȩ����Ϊ����
    /// </summary>
    bool EmptyRolesIsDeny { get; }

    /// <summary>
    /// ���ɵ���Ȩ
    /// true: ֻҪ�û�ӵ�еĽ�ɫ֮һ���ڽ��ý�ɫ������Ͳ�������
    /// false: ֻҪ�û�ӵ�еĽ�ɫ֮һ�ڽ��ý�ɫ������ͱ�����
    /// </summary>
    bool EasyAuthorization { get; }

    #endregion

    #region ����

    /// <summary>
    /// ȡ��ɫ���϶���
    /// </summary>
    IDictionary<string, RoleInfo> GetRoleInfos(UserIdentity identity);

    /// <summary>
    /// ȡ����Ȩ��ɫ���϶���
    /// </summary>
    IDictionary<string, RoleInfo> GetGrantRoleInfos(UserIdentity identity);

    /// <summary>
    /// ȡ��Ƭ���϶���
    /// </summary>
    IDictionary<string, SectionInfo> GetSectionInfos(UserIdentity identity);

    /// <summary>
    /// ȡ���Ÿ�λ���û����
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
    /// ��¼��ʵ
    /// </summary>
    bool? LogOnVerify(string userNumber, string tag);

    /// <summary>
    /// �ǳ�
    /// </summary>
    void LogOff(UserIdentity identity);

    /// <summary>
    /// �޸ĵ�¼����
    /// </summary>
    bool ChangePassword(string newPassword, UserIdentity identity);

    /// <summary>
    /// ������¼����
    /// </summary>
    bool UnlockPassword(string userNumber, UserIdentity identity);

    /// <summary>
    /// ���õ�¼����
    /// </summary>
    bool ResetPassword(string userNumber, UserIdentity identity);

    /// <summary>
    /// ���ù���������
    /// </summary>
    void SetProcessLockInfo(string processName, string caption, bool toLocked, TimeSpan expiryTime, string remark, UserIdentity identity);

    /// <summary>
    /// ��ȡ����������
    /// </summary>
    ProcessLockInfo GetProcessLockInfo(string processName, UserIdentity identity);

    #region �������ݿ�����

    /// <summary>
    /// ����û�
    /// </summary>
    void AddUser(DbTransaction transaction, long id, string userName, string userNumber, string password);

    /// <summary>
    /// ���õ�¼����
    /// </summary>
    bool ChangePassword(DbTransaction transaction, string userNumber, string newPassword);

    #endregion

    #endregion
  }
}