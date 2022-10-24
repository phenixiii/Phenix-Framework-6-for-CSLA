#if Top
using System.Collections.ObjectModel;
#endif

using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Globalization;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Threading;
using System.Web;
using Phenix.Core.Dictionary;
using Phenix.Core.Log;
using Phenix.Core.Mapping;
using Phenix.Core.Net;
using Phenix.Core.Reflection;
using Phenix.Core.Security.Cryptography;
using Phenix.Core.SyncCollections;

namespace Phenix.Core.Security
{
  /// <summary>
  /// �û����
  /// </summary>
  [Serializable]
  public class UserIdentity : IIdentity
  {
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
    [Newtonsoft.Json.JsonConstructor]
    private UserIdentity(bool isAuthenticated, long? userId, string userName, string userNumber,
      long serverVersion, DateTime lastOperationTime,
      long? departmentId, DepartmentInfo department, long? positionId, PositionInfo position,
      IDictionary<string, RoleInfo> roles, IDictionary<string, RoleInfo> grantRoles, IDictionary<string, SectionInfo> sections, string cultureName)
    {
      _isAuthenticated = isAuthenticated;
      _userId = userId;
      _userName = userName;
      _userNumber = userNumber;
      _serverVersion = serverVersion;
      _lastOperationTime = lastOperationTime;
      _departmentId = departmentId;
      _department = department;
      _positionId = positionId;
      _position = position;
      _roles = roles;
      _grantRoles = grantRoles;
      _sections = sections;
      _cultureName = cultureName;
    }

    private UserIdentity(string userNumber, string password, string dynamicPassword, string localAddress, string servicesAddress, string timestamp)
    {
      _userNumber = userNumber;
      _password = password;
      _dynamicPassword = dynamicPassword;
      _localAddress = localAddress;
      _servicesAddress = servicesAddress;
      _timestamp = timestamp;
      _signature = RijndaelCryptoTextProvider.Encrypt(password, timestamp);
      _cultureName = Thread.CurrentThread.CurrentCulture.Name;
    }

    private UserIdentity(string userNumber, string password, string dynamicPassword, string localAddress, string servicesAddress)
      : this(userNumber, password, dynamicPassword, localAddress, servicesAddress, Guid.NewGuid().ToString()) { }

    private UserIdentity(bool isAuthenticated, long? userId, string userName, string userNumber, string password, string dynamicPassword,
      long serverVersion, string localAddress, string servicesAddress, DateTime lastOperationTime, 
      long? departmentId, long? positionId)
      : this(userNumber, password, dynamicPassword, localAddress, servicesAddress)
    {
      _isAuthenticated = isAuthenticated;
      _userId = userId;
      _userName = userName;
      _serverVersion = serverVersion;
      _lastOperationTime = lastOperationTime;
      _departmentId = departmentId;
      _positionId = positionId;
    }

    /// <summary>
    /// ��ʼ��
    /// </summary>
    public UserIdentity(string userNumber, string password)
      : this(userNumber, password, null, NetConfig.LocalAddress, NetConfig.ServicesAddress) { }

    #region ����

    /// <summary>
    /// ���������û�
    /// </summary>
    public static UserIdentity CreateTester()
    {
      return new UserIdentity(true, AdminId, AdminUserName, AdminUserNumber, AdminUserNumber, null,
        0, NetConfig.LocalAddress, NetConfig.ServicesAddress, DateTime.Now, 
        null, null);
    }

    /// <summary>
    /// ����δ��֤�û�
    /// </summary>
    /// <param name="userId">�û�ID</param>
    /// <param name="userName">�û�����</param>
    /// <param name="userNumber">��¼����</param>
    /// <param name="departmentId">����ID</param>
    /// <param name="positionId">��λID</param>
    [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Advanced)]
    public static UserIdentity CreateUnauthenticated(long userId, string userName, string userNumber, 
      long? departmentId, long? positionId)
    {
      return new UserIdentity(false, userId, userName, userNumber, null, null,
        0, NetConfig.LocalAddress, NetConfig.ServicesAddress,
        DateTime.Now, departmentId, positionId);
    }

    /// <summary>
    /// ������֤�û�
    /// </summary>
    [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Advanced)]
    public static UserIdentity CreateAuthenticated(long userId, string userName, string userNumber, string password, string dynamicPassword,
      long serverVersion, string localAddress, string servicesAddress, DateTime lastOperationTime,
      long? departmentId, long? positionId)
    {
      return new UserIdentity(true, userId, userName, userNumber, password, dynamicPassword,
        serverVersion, localAddress, servicesAddress, lastOperationTime,
        departmentId, positionId);
    }

    /// <summary>
    /// ���������û�
    /// </summary>
    public static UserIdentity CreateGuest()
    {
      return new UserIdentity(GuestUserNumber, GuestUserNumber);
    }

    #endregion

    #region ����

    #region Guest

    /// <summary>
    /// �����û�ID
    /// </summary>
    public static long GuestId
    {
      get { return -1; }
    }

    /// <summary>
    /// �����û�����
    /// </summary>
    public static string GuestUserName
    {
      get { return "Guest"; }
    }

    /// <summary>
    /// �����û���¼����
    /// </summary>
    public static string GuestUserNumber
    {
      get { return "Guest"; }
    }

    /// <summary>
    /// �Ƿ������û�
    /// </summary>
    public bool IsGuest
    {
      get { return IsGuestUserNumber(UserNumber); }
    }

    #endregion

    #region Admin

    /// <summary>
    /// ����ԱID
    /// </summary>
    public static long AdminId
    {
      get { return 0; }
    }

    /// <summary>
    /// ����Ա����
    /// </summary>
    public static string AdminUserName
    {
      get { return "Administrator"; }
    }

    /// <summary>
    /// ����Ա��¼����
    /// </summary>
    public static string AdminUserNumber
    {
      get { return "ADMIN"; }
    }

    /// <summary>
    /// ����Ա��ɫ����
    /// </summary>
    public static string AdminRoleName
    {
      get { return "Admin"; }
    }

    [NonSerialized]
    private bool? _isAdmin;
    /// <summary>
    /// �Ƿ����Ա
    /// </summary>
    [Newtonsoft.Json.JsonIgnore]
    public bool IsAdmin
    {
      get
      {
        if (!_isAdmin.HasValue)
          _isAdmin = String.CompareOrdinal(UserNumber, AdminUserNumber) == 0; 
        return _isAdmin.Value;
      }
    }

    [NonSerialized]
    private bool? _haveAdminRole;
    /// <summary>
    /// �Ƿ�ӵ�й���Ա��ɫ
    /// </summary>
    [Newtonsoft.Json.JsonIgnore]
    public bool HaveAdminRole
    {
      get
      {
        if (!_haveAdminRole.HasValue)
          _haveAdminRole = DoIsInRole(AdminRoleName) || IsAdmin;
        return _haveAdminRole.Value;
      }
    }

    #endregion

    #region IIdentity ��Ա

    /// <summary>
    /// ����
    /// </summary>
    public string Name
    {
      get { return _userNumber; }
    }

    /// <summary>
    /// �����֤����
    /// </summary>
    public string AuthenticationType
    {
      get { return DataSecurityContext.InternalAuthenticationType; }
    }

    private bool _isAuthenticated;
    /// <summary>
    /// ����֤?
    /// </summary>
    public bool IsAuthenticated
    {
      get { return _isAuthenticated; }
      protected set { _isAuthenticated = value; }
    }

    private string _authenticatedMessage;
    /// <summary>
    /// ��֤��Ϣ
    /// </summary>
    public string AuthenticatedMessage
    {
      get { return _authenticatedMessage; }
      protected internal set { _authenticatedMessage = value; }
    }

    private readonly long? _userId;
    /// <summary>
    /// �û�ID
    /// </summary>
    public long? UserId
    {
      get { return _userId; }
    }

    private readonly string _userName;
    /// <summary>
    /// �û�����
    /// </summary>
    public string UserName
    {
      get { return _userName; }
    }

    private readonly string _userNumber;
    /// <summary>
    /// ��¼����
    /// </summary>
    public string UserNumber
    {
      get { return _userNumber; }
    }

    [NonSerialized]
    private string _password;
    /// <summary>
    /// ��¼����
    /// </summary>
    [Newtonsoft.Json.JsonIgnore]
    public string Password
    {
      get { return _password; }
      set
      {
        _password = value;
        _signature = RijndaelCryptoTextProvider.Encrypt(value, _timestamp);
      }
    }

    [NonSerialized]
    private string _dynamicPassword;
    /// <summary>
    /// ��¼����
    /// </summary>
    [Newtonsoft.Json.JsonIgnore]
    public string DynamicPassword
    {
      get { return _dynamicPassword; }
      set { _dynamicPassword = value; }
    }

    private readonly long _serverVersion;
    /// <summary>
    /// �������汾
    /// </summary>
    public long ServerVersion
    {
      get { return _serverVersion; }
    }

    private readonly string _localAddress;
    /// <summary>
    /// ����IP��ַ
    /// </summary>
    [Newtonsoft.Json.JsonIgnore]
    public string LocalAddress
    {
      get { return _localAddress; }
    }

    private readonly string _servicesAddress;
    /// <summary>
    /// ����IP��ַ
    /// </summary>
    [Newtonsoft.Json.JsonIgnore]
    public string ServicesAddress
    {
      get { return _servicesAddress; }
    }

    /// <summary>
    /// ������ҵ
    /// </summary>
    public string Enterprise
    {
      get { return DataDictionaryHub.Enterprise; }
    }

    private readonly long? _departmentId;
    /// <summary>
    /// ��������ID
    /// </summary>
    public long? DepartmentId
    {
      get { return _departmentId; }
    }

    [NonSerialized]
    private DepartmentInfo _department;
    /// <summary>
    /// ��������
    /// </summary>
    public DepartmentInfo Department
    {
      get
      {
        if (_department == null && _departmentId.HasValue)
          DataDictionaryHub.DepartmentInfos.TryGetValue(_departmentId.Value, out _department);
        return _department;
      }
    }

    private readonly long? _positionId;
    /// <summary>
    /// ���θ�λID
    /// </summary>
    public long? PositionId
    {
      get { return _positionId; }
    }

    [NonSerialized]
    private PositionInfo _position;
    /// <summary>
    /// ���θ�λ
    /// </summary>
    public PositionInfo Position
    {
      get
      {
        if (_position == null && _positionId.HasValue)
          DataDictionaryHub.PositionInfos.TryGetValue(_positionId.Value, out _position);
        return _position;
      }
    }

    [NonSerialized]
    private IDictionary<string, IIdentity> _superiors;
    /// <summary>
    /// �ϼ�(һ��)
    /// (�����ŵ�)�ϼ���λ���û���(�����Ÿ���λ�û���)�ϼ����Ÿ���λ���û�
    /// </summary>
    [Newtonsoft.Json.JsonIgnore]
    public IDictionary<string, IIdentity> Superiors
    {
      get
      {
        if (_superiors == null)
        {
          IDictionary<string, IIdentity> result;
          if (Department != null && Position != null && Position.Superior != null)
          {
            List<PositionInfo> positions = new List<PositionInfo> { this.Position.Superior };
            result = DataSecurityHub.GetIdentities(Department, positions, this);
          }
          else
          {
            result = new Dictionary<string, IIdentity>(StringComparer.Ordinal);
            if (Department != null && Department.Superior != null &&
              (Position == null || Department.PositionTree != null && Position.Id == Department.PositionTree.Id))
            {
              List<PositionInfo> positions = null;
              if (Department.Superior.PositionTree != null)
                positions = new List<PositionInfo> { this.Department.Superior.PositionTree };
              foreach (KeyValuePair<string, IIdentity> kvp in DataSecurityHub.GetIdentities(Department.Superior, positions, this))
                result[kvp.Key] = kvp.Value;
            }
          }
          _superiors = result;
        }
        return _superiors;
      }
    }

    [NonSerialized]
    private Dictionary<string, IIdentity> _allSuperiors;
    /// <summary>
    /// �ϼ�(ȫ��)
    /// </summary>
    [Newtonsoft.Json.JsonIgnore]
    public IDictionary<string, IIdentity> AllSuperiors
    {
      get
      {
        if (_allSuperiors == null)
        {
          Dictionary<string, IIdentity> result = new Dictionary<string, IIdentity>(Superiors, StringComparer.Ordinal);
          foreach (KeyValuePair<string, IIdentity> kvp1 in Superiors)
            foreach (KeyValuePair<string, IIdentity> kvp2 in kvp1.Value.AllSuperiors)
              result[kvp2.Key] = kvp2.Value;
          _allSuperiors = result;
        }
        return _allSuperiors;
      }
    }

    [NonSerialized]
    private IDictionary<string, IIdentity> _workmates;
    /// <summary>
    /// ����
    /// </summary>
    [Newtonsoft.Json.JsonIgnore]
    public IDictionary<string, IIdentity> Workmates
    {
      get
      {
        if (_workmates == null)
        {
          IDictionary<string, IIdentity> result;
          if (Department != null)
          {
            List<PositionInfo> positions = new List<PositionInfo>();
            if (Position != null)
              positions.Add(Position);
            result = DataSecurityHub.GetIdentities(Department, positions, this);
          }
          else
          {
            result = new Dictionary<string, IIdentity>(0);
#if Top
            result = new ReadOnlyDictionary<string, IIdentity>(result);
#endif
          }
          _workmates = result;
        }
        return _workmates;
      }
    }

    [NonSerialized]
    private IDictionary<string, IIdentity> _subordinates;
    /// <summary>
    /// �¼�(һ��)
    /// (�����ŵ�)�¼���λ���û���(�����Ÿ���λ�û���)�¼����Ÿ���λ���û�
    /// </summary>
    [Newtonsoft.Json.JsonIgnore]
    public IDictionary<string, IIdentity> Subordinates
    {
      get
      {
        if (_subordinates == null)
        {
          IDictionary<string, IIdentity> result;
          if (Department != null && Position != null && Position.Subordinates.Count > 0)
            result = DataSecurityHub.GetIdentities(Department, Position.Subordinates, this);
          else
          {
            result = new Dictionary<string, IIdentity>(StringComparer.Ordinal);
            if (Department != null && Department.Subordinates.Count > 0 &&
              (Position == null || Department.PositionTree != null && Position.Id == Department.PositionTree.Id))
            {
              foreach (DepartmentInfo item in Department.Subordinates)
              {
                List<PositionInfo> positions = null;
                if (item.PositionTree != null)
                  positions = new List<PositionInfo> { item.PositionTree };
                foreach (KeyValuePair<string, IIdentity> kvp in DataSecurityHub.GetIdentities(item, positions, this))
                  result[kvp.Key] = kvp.Value;
              }
            }
          }
          _subordinates = result;
        }
        return _subordinates;
      }
    }

    [NonSerialized]
    private Dictionary<string, IIdentity> _allSubordinates;
    /// <summary>
    /// �¼�(ȫ��)
    /// </summary>
    [Newtonsoft.Json.JsonIgnore]
    public IDictionary<string, IIdentity> AllSubordinates
    {
      get
      {
        if (_allSubordinates == null)
        {
          Dictionary<string, IIdentity> result = new Dictionary<string, IIdentity>(Subordinates, StringComparer.Ordinal);
          foreach (KeyValuePair<string, IIdentity> kvp1 in Subordinates)
            foreach (KeyValuePair<string, IIdentity> kvp2 in kvp1.Value.AllSubordinates)
              result[kvp2.Key] = kvp2.Value;
          _allSubordinates = result;
        }
        return _allSubordinates;
      }
    }

    [NonSerialized]
    private IDictionary<string, RoleInfo> _roles;
    /// <summary>
    /// ��ɫ���϶���
    /// </summary>
    public IDictionary<string, RoleInfo> Roles
    {
      get
      {
        if (_roles == null)
          _roles = DataSecurityHub.GetRoleInfos(this);
        return _roles;
      }
    }

    [NonSerialized]
    private IDictionary<string, RoleInfo> _grantRoles;
    /// <summary>
    /// ����Ȩ��ɫ���϶���
    /// </summary>
    public IDictionary<string, RoleInfo> GrantRoles
    {
      get
      {
        if (_grantRoles == null)
          _grantRoles = DataSecurityHub.GetGrantRoleInfos(this);
        return _grantRoles;
      }
    }

    [NonSerialized]
    private IDictionary<string, SectionInfo> _sections;
    /// <summary>
    /// ��Ƭ���϶���
    /// </summary>
    public IDictionary<string, SectionInfo> Sections
    {
      get
      {
        if (_sections == null)
          _sections = DataSecurityHub.GetSectionInfos(this);
        return _sections;
      }
    }

    private readonly string _cultureName;
    /// <summary>
    /// ����������
    /// </summary>
    public string CultureName
    {
      get { return _cultureName ?? Thread.CurrentThread.CurrentCulture.Name; }
    }

    private readonly SynchronizedDictionary<string, object> _fieldDefaultValueCache = 
      new SynchronizedDictionary<string, object>(StringComparer.OrdinalIgnoreCase);
    private readonly SynchronizedDictionary<string, SynchronizedList<object>> _filterFieldValuesCache = 
      new SynchronizedDictionary<string, SynchronizedList<object>>(StringComparer.OrdinalIgnoreCase);
    private readonly SynchronizedDictionary<string, object> _singleFilterFieldValueCache = 
      new SynchronizedDictionary<string, object>(StringComparer.OrdinalIgnoreCase);

    #endregion

    #region Authentication

    private readonly string _timestamp;
    /// <summary>
    /// ʱ���
    /// </summary>
    [Newtonsoft.Json.JsonIgnore]
    public string Timestamp
    {
      get { return _timestamp; }
    }

    private string _signature;
    /// <summary>
    /// ǩ��
    /// </summary>
    [Newtonsoft.Json.JsonIgnore]
    public string Signature
    {
      get { return _signature; }
    }
    
    private static readonly SynchronizedDictionary<string, UserIdentity> _usersCache =
      new SynchronizedDictionary<string, UserIdentity>(StringComparer.Ordinal);

    [NonSerialized]
    private List<string> _timestampList;

    [NonSerialized]
    private readonly DateTime _lastOperationTime;
    /// <summary>
    /// ���ʱ��
    /// </summary>
    public DateTime LastOperationTime
    {
      get { return _lastOperationTime; }
    }

    [NonSerialized]
    private bool? _allowUserMultipleAddressLogin;
    /// <summary>
    /// �����û���IP��¼
    /// ������Guest�û�
    /// </summary>
    public bool AllowUserMultipleAddressLogin
    {
      get
      {
        if (!_allowUserMultipleAddressLogin.HasValue)
          _allowUserMultipleAddressLogin = DataSecurityHub.AllowUserMultipleAddressLogin;
        return _allowUserMultipleAddressLogin.Value;
      }
    }

    [NonSerialized]
    private int? _sessionExpiresMinutes;
    /// <summary>
    /// �Ự����ʱ��(����)
    /// С�ڵ���0������
    /// ������Guest�û�
    /// </summary>
    public int SessionExpiresMinutes
    {
      get
      {
        if (!_sessionExpiresMinutes.HasValue)
          _sessionExpiresMinutes = DataSecurityHub.SessionExpiresMinutes;
        return _sessionExpiresMinutes.Value;
      }
    }

    [NonSerialized]
    private bool? _emptyRolesIsDeny;
    /// <summary>
    /// (�û�/����)δ���ý�ɫ������Ȩ����Ϊ����
    /// </summary>
    public bool EmptyRolesIsDeny
    {
      get
      {
        if (!_emptyRolesIsDeny.HasValue)
          _emptyRolesIsDeny = DataSecurityHub.EmptyRolesIsDeny;
        return _emptyRolesIsDeny.Value;
      }
    }

    [NonSerialized]
    private bool? _easyAuthorization;
    /// <summary>
    /// ���ɵ���Ȩ
    /// ȱʡΪ false
    /// true: ֻҪ�û�ӵ�еĽ�ɫ֮һ���ڽ��ý�ɫ������Ͳ�������
    /// false: ֻҪ�û�ӵ�еĽ�ɫ֮һ�ڽ��ý�ɫ������ͱ�����
    /// </summary>
    public bool EasyAuthorization
    {
      get
      {
        if (!_easyAuthorization.HasValue)
          _easyAuthorization = DataSecurityHub.EasyAuthorization;
        return _easyAuthorization.Value;
      }
    }

    /// <summary>
    /// ��ǰ�û����
    /// </summary>
    public static UserIdentity CurrentIdentity
    {
      get
      {
        if (HttpContext.Current != null && HttpContext.Current.User != null)
        {
          UserIdentity result = HttpContext.Current.User.Identity as UserIdentity;
          if (result != null)
            return result;
        }
        if (Thread.CurrentPrincipal != null)
        {
          UserIdentity result = Thread.CurrentPrincipal.Identity as UserIdentity;
          if (result != null)
            return result;
        }
        return null;
      }
      set
      {
        if (HttpContext.Current != null)
          HttpContext.Current.User = value != null ? new UserPrincipal(value) : null;
        Thread.CurrentPrincipal = value != null ? new UserPrincipal(value) : null;
        if (value != null)
          Thread.CurrentThread.CurrentCulture = CultureInfo.GetCultureInfo(value.CultureName);
      }
    }

    #endregion

    #endregion

    #region ����

    #region ���¼���ϵ

    /// <summary>
    /// ӵ���ϼ�
    /// </summary>
    public bool HaveSuperior(string userNumber)
    {
      if (AllSuperiors.Count > 0)
        foreach (KeyValuePair<string, IIdentity> kvp in AllSuperiors)
          if (String.CompareOrdinal(kvp.Value.UserNumber, userNumber) == 0)
            return true;
      return false;
    }

    /// <summary>
    /// ӵ������
    /// </summary>
    public bool HaveSubordinate(string userNumber)
    {
      if (AllSubordinates.Count > 0)
        foreach (KeyValuePair<string, IIdentity> kvp in AllSubordinates)
          if (String.CompareOrdinal(kvp.Value.UserNumber, userNumber) == 0)
            return true;
      return false;
    }

    #endregion

    #region ������

    /// <summary>
    /// ���̼���
    /// expiryTime = TimeSpan.Zero�����Զ�ʧЧ
    /// </summary>
    /// <param name="processName">������</param>
    /// <param name="caption">��ǩ</param>
    /// <param name="remark">��ע</param>
    public void LockProcess(string processName, string caption, string remark)
    {
      DataSecurityHub.SetProcessLockInfo(processName, caption, true, TimeSpan.Zero, remark, this);
    }

    /// <summary>
    /// ���̼���
    /// </summary>
    /// <param name="processName">������</param>
    /// <param name="caption">��ǩ</param>
    /// <param name="expiryTime">����, TimeSpan.Zero�����Զ�ʧЧ</param>
    /// <param name="remark">��ע</param>
    public void LockProcess(string processName, string caption, TimeSpan expiryTime, string remark)
    {
      DataSecurityHub.SetProcessLockInfo(processName, caption, true, expiryTime, remark, this);
    }

    /// <summary>
    /// ���̽���
    /// remark = null
    /// </summary>
    /// <param name="processName">������</param>
    public void UnlockProcess(string processName)
    {
      DataSecurityHub.SetProcessLockInfo(processName, null, false, TimeSpan.Zero, null, this);
    }

    /// <summary>
    /// ���̽���
    /// </summary>
    /// <param name="processName">������</param>
    /// <param name="remark">��ע</param>
    public void UnlockProcess(string processName, string remark)
    {
      DataSecurityHub.SetProcessLockInfo(processName, null, false, TimeSpan.Zero, remark, this);
    }

    /// <summary>
    /// ȡ����������
    /// </summary>
    /// <param name="processName">������</param>
    public ProcessLockInfo GetProcessLockInfo(string processName)
    {
      return DataSecurityHub.GetProcessLockInfo(processName, this);
    }

    /// <summary>
    /// �Ƿ��������������
    /// </summary>
    /// <param name="processName">������</param>
    public bool AllowSetProcessLockInfo(string processName)
    {
      return AllowSetProcessLockInfo(GetProcessLockInfo(processName));
    }

    /// <summary>
    /// �Ƿ��������������
    /// </summary>
    /// <param name="info">����������</param>
    public bool AllowSetProcessLockInfo(ProcessLockInfo info)
    {
      if (info == null)
        return true;
      return info.AllowSet(this);
    }

    #endregion

    #region DefaultValue

    /// <summary>
    /// ����������ʽ�ֶ�ȱʡֵ
    /// </summary>
    /// <param name="regexColumnName">������ʽ�ֶ���</param>
    /// <param name="value">ȱʡֵ</param>
    public void SetFieldDefaultValue(string regexColumnName, object value)
    {
      if (regexColumnName == null)
        throw new ArgumentNullException("regexColumnName");
      _fieldDefaultValueCache[regexColumnName] = value;
    }

    /// <summary>
    /// ɾ��������ʽ�ֶ�ȱʡֵ
    /// </summary>
    /// <param name="regexColumnName">������ʽ�ֶ���</param>
    /// <returns>�Ƿ�ɾ��</returns>
    public bool RemoveFieldDefaultValue(string regexColumnName)
    {
      if (regexColumnName == null)
        throw new ArgumentNullException("regexColumnName");
      return _fieldDefaultValueCache.Remove(regexColumnName);
    }

    /// <summary>
    /// ��ȡ������ʽ�ֶ�ȱʡֵ
    /// </summary>
    /// <param name="regexColumnName">������ʽ�ֶ���</param>
    /// <returns>�ֶ�ȱʡֵ</returns>
    public object GetFieldDefaultValue(string regexColumnName)
    {
      if (regexColumnName == null)
        throw new ArgumentNullException("regexColumnName");
      object result;
      if (_fieldDefaultValueCache.TryGetValue(regexColumnName, out result))
        return result;
      return null;
    }

    /// <summary>
    /// ��ȡ�ֶ�ȱʡֵ
    /// </summary>
    /// <param name="fieldMapInfo">����ӳ���ֶ�</param>
    /// <returns>�ֶ�ȱʡֵ</returns>
    public object GetFieldDefaultValue(FieldMapInfo fieldMapInfo)
    {
      if (fieldMapInfo != null && !String.IsNullOrEmpty(fieldMapInfo.FullTableColumnName))
        foreach (KeyValuePair<string, object> kvp in _fieldDefaultValueCache)
          if (Regex.IsMatch(fieldMapInfo.FullTableColumnName, kvp.Key, RegexOptions.IgnoreCase))
            return Utilities.ChangeType(kvp.Value, fieldMapInfo.Field.FieldType);
      return null;
    }

    #endregion

    #region �����ֶ�

    /// <summary>
    /// ��ȡ�����ֶ�ֵ����
    /// </summary>
    /// <param name="fieldMapInfo">����ӳ���ֶ�</param>
    /// <returns>�ֶ�ֵ����</returns>
    public IList<object> FindFilterFieldValues(IFieldMapInfo fieldMapInfo)
    {
      List<object> result = new List<object>();
      foreach (KeyValuePair<string, object> kvp in _singleFilterFieldValueCache)
        if (Regex.IsMatch(fieldMapInfo.FullTableColumnName, kvp.Key, RegexOptions.IgnoreCase))
          result.Add(kvp.Value);
      if (result.Count > 0)
        return result;
      foreach (KeyValuePair<string, SynchronizedList<object>> kvp in _filterFieldValuesCache)
        if (Regex.IsMatch(fieldMapInfo.FullTableColumnName, kvp.Key, RegexOptions.IgnoreCase))
          result.AddRange(kvp.Value);
      return result;
    }

    /// <summary>
    /// ע��������ʽ�����ֶ�ֵ
    /// </summary>
    /// <param name="regexColumnName">������ʽ�ֶ���</param>
    /// <param name="fieldValues">�ֶ�ֵ</param>
    public void RegisterFilterFieldValue(string regexColumnName, params object[] fieldValues)
    {
      if (regexColumnName == null)
        throw new ArgumentNullException("regexColumnName");
      SynchronizedList<object> value = _filterFieldValuesCache.GetValue(regexColumnName, ()=> new SynchronizedList<object>(), true);
      if (fieldValues != null && fieldValues.Length > 0)
        value.AddRange(fieldValues);
    }

    /// <summary>
    /// ����ע���������ʽ�����ֶ�ֵ
    /// </summary>
    /// <param name="regexColumnName">������ʽ�ֶ���</param>
    /// <param name="fieldValue">�ֶ�ֵ</param>
    public bool HaveRegisteredFilterFieldValue(string regexColumnName, object fieldValue)
    {
      if (regexColumnName == null)
        throw new ArgumentNullException("regexColumnName");
      SynchronizedList<object> value;
      if (_filterFieldValuesCache.TryGetValue(regexColumnName, out value))
        return value.Contains(fieldValue);
      return false;
    }

    /// <summary>
    /// ע��������ʽ�����ֶ�ֵ
    /// </summary>
    /// <param name="regexColumnName">������ʽ�ֶ���</param>
    /// <param name="fieldValues">�ֶ�ֵ</param>
    public bool UnregisterFilterFieldValue(string regexColumnName, params object[] fieldValues)
    {
      if (regexColumnName == null)
        throw new ArgumentNullException("regexColumnName");
      SynchronizedList<object> value;
      if (_filterFieldValuesCache.TryGetValue(regexColumnName, out value))
        if (fieldValues != null && fieldValues.Length > 0)
        {
          bool result = false;
          foreach (object item in fieldValues)
          {
            value.Remove(item);
            result = true;
          }
          return result;
        }
        else
          return _filterFieldValuesCache.Remove(regexColumnName);
      return false;
    }

    /// <summary>
    /// ��ȡע���������ʽ�����ֶ�ֵ
    /// </summary>
    /// <param name="regexColumnName">������ʽ�ֶ���</param>
    public IList<object> GetRegisteredFilterFieldValues(string regexColumnName)
    {
      if (regexColumnName == null)
        throw new ArgumentNullException("regexColumnName");
      SynchronizedList<object> result;
      if (_filterFieldValuesCache.TryGetValue(regexColumnName, out result))
        return result.AsReadOnly();
      return new List<object>(0).AsReadOnly();
    }

    /// <summary>
    /// ���õ�һ��������ʽ�����ֶ�ֵ
    /// ���ú󽫽���RegisterFilterFieldValueע������
    /// </summary>
    /// <param name="regexColumnName">������ʽ�ֶ���</param>
    /// <param name="value">�ֶ�ֵ</param>
    public void SetSingleFilterFieldValue(string regexColumnName, object value)
    {
      if (regexColumnName == null)
        throw new ArgumentNullException("regexColumnName");
      _singleFilterFieldValueCache[regexColumnName] = value;
    }

    /// <summary>
    /// ɾ����һ��������ʽ�����ֶ�ֵ
    /// ɾ���󽫻ָ�RegisterFilterFieldValueע������
    /// </summary>
    /// <param name="regexColumnName">������ʽ�ֶ���</param>
    public bool RemoveSingleFilterFieldValue(string regexColumnName)
    {
      if (regexColumnName == null)
        throw new ArgumentNullException("regexColumnName");
      return _singleFilterFieldValueCache.Remove(regexColumnName);
    }

    /// <summary>
    /// ��ȡ��һ��������ʽ�����ֶ�ֵ
    /// </summary>
    /// <param name="regexColumnName">������ʽ�ֶ���</param>
    public object GetSingleFilterFieldValue(string regexColumnName)
    {
      if (regexColumnName == null)
        throw new ArgumentNullException("regexColumnName");
      object result;
      if (_singleFilterFieldValueCache.TryGetValue(regexColumnName, out result))
        return result;
      return null;
    }

    #endregion

    #region ��Ƭ

    private bool IsInSection(string sectionName)
    {
      if (!IsAuthenticated)
        return false;
      if (HaveAdminRole)
        return true;
      if (sectionName == null)
        return false;
      return Sections.ContainsKey(sectionName);
    }

    /// <summary>
    /// ȡ���û��ɲ���������������Ƭ
    /// null����������Ƭ
    /// </summary>
    /// <param name="objectType">��</param>
    public IList<string> GetSectionNames(Type objectType)
    {
      if (!IsAuthenticated)
        return new List<string>(0);
      if (HaveAdminRole)
        return null;
      IList<string> sectionNames = SectionInfo.GetSectionNames(objectType);
      if (sectionNames.Count == 0)
        return null;
      List<string> result = new List<string>();
      bool find = false;
      foreach (FieldMapInfo fieldMapInfo in ClassMemberHelper.DoGetFieldMapInfos(objectType))
      {
        TableFilterInfo tableFilterInfo = fieldMapInfo.TableFilterInfo;
        if (tableFilterInfo != null && tableFilterInfo.SectionInfos != null)
        {
          IList<object> filterFieldValues = FindFilterFieldValues(fieldMapInfo);
          if (filterFieldValues.Count > 0)
          {
            find = true;
            foreach (object filterFieldValue in filterFieldValues)
            {
              string value = (string)Utilities.ChangeType(filterFieldValue, typeof(string));
              foreach (TableFilterSectionInfo tableFilterSectionInfo in tableFilterInfo.SectionInfos)
                if (IsInSection(tableFilterSectionInfo.Name) && String.CompareOrdinal(tableFilterSectionInfo.AllowReadColumnValue, value) == 0)
                  if (!result.Contains(tableFilterSectionInfo.Name))
                    result.Add(tableFilterSectionInfo.Name);
            }
          }
        }
      }
      if (!find)
        foreach (string s in sectionNames)
          if (IsInSection(s))
            result.Add(s);
      return result;
    }

    /// <summary>
    /// ȡ���û��ɲ������༰����������������Ƭ
    /// null����������Ƭ
    /// </summary>
    /// <param name="objectType">��</param>
    /// <param name="criteriaType">������</param>
    public IList<string> GetSectionNames(Type objectType, Type criteriaType)
    {
      if (!IsAuthenticated)
        return new List<string>(0);
      if (HaveAdminRole)
        return null;
      IList<string> result = GetSectionNames(objectType);
      if (criteriaType == null)
        return result;
      List<Type> checkedFieldType = new List<Type>();
      return GetSectionNames(ClassMemberHelper.DoGetCriteriaFieldMapInfos(criteriaType, objectType), result, ref checkedFieldType);
    }

    private IList<string> GetSectionNames(IDictionary<string, CriteriaFieldMapInfo> criteriaFieldMapInfos, IList<string> sectionNames,
      ref List<Type> checkedFieldType)
    {
      List<string> result = sectionNames == null ? new List<string>() : new List<string>(sectionNames);
      if (criteriaFieldMapInfos != null)
        foreach (KeyValuePair<string, CriteriaFieldMapInfo> kvp in criteriaFieldMapInfos)
          if (kvp.Value.IsSubCriteria)
          {
            IList<string> itemSectionNames = null;
            switch (kvp.Value.CriteriaFieldAttribute.Operate)
            {
              case CriteriaOperate.Embed:
                if (!checkedFieldType.Contains(kvp.Value.Field.FieldType))
                {
                  checkedFieldType.Add(kvp.Value.Field.FieldType);
                  itemSectionNames = GetSectionNames(kvp.Value.GetSubCriteriaFieldMapInfos(null), null, ref checkedFieldType);
                }
                break;
              case CriteriaOperate.Exists:
              case CriteriaOperate.NotExists:
                if (!checkedFieldType.Contains(kvp.Value.Field.FieldType))
                {
                  checkedFieldType.Add(kvp.Value.Field.FieldType);
                  itemSectionNames = GetSectionNames(kvp.Value.Field.FieldType);
                }
                break;
            }
            if (itemSectionNames != null)
              foreach (string s in itemSectionNames)
                if (!result.Contains(s))
                  result.Add(s);
          }
      return result;
    }

    #endregion

    #region Ȩ����֤

    /// <summary>
    /// �Ƿ�������������
    /// ֻ����Ϊfalse
    /// </summary>
    /// <param name="info">����</param>
    public bool AllowSet(ISecurityInfo info)
    {
      if (!IsAuthenticated)
        return false;
      if (HaveAdminRole)
        return true;
      if (info == null)
        return false;
      if (String.CompareOrdinal(info.UserNumber, UserNumber) == 0)
        return true;
      return HaveSubordinate(info.UserNumber);
    }

    /// <summary>
    /// �Ƿ�������������
    /// ֻ����Ϊfalse
    /// </summary>
    /// <param name="data">����</param>
    public bool AllowSet(object data)
    {
      if (!IsAuthenticated)
        return false;
      if (HaveAdminRole)
        return true;
      if (data == null)
        return false;
      if (SectionInfo.GetSectionNames(data.GetType()).Count == 0)
        return true;
      foreach (FieldMapInfo fieldMapInfo in ClassMemberHelper.DoGetFieldMapInfos(data.GetType()))
      {
        TableFilterInfo tableFilterInfo = fieldMapInfo.TableFilterInfo;
        if (tableFilterInfo != null && tableFilterInfo.SectionInfos != null)
        {
          string value = (string)Utilities.ChangeType(fieldMapInfo.GetValue(data), typeof(string));
          bool find = false;
          foreach (TableFilterSectionInfo tableFilterSectionInfo in tableFilterInfo.SectionInfos)
            if (IsInSection(tableFilterSectionInfo.Name))
            {
              find = true;
              if (tableFilterSectionInfo.AllowEdit && String.CompareOrdinal(tableFilterSectionInfo.AllowReadColumnValue, value) == 0)
                return true;
            }
          if (!find && !tableFilterInfo.NoneSectionIsDeny)
            return true;
        }
      }
      return false;
    }

    /// <summary>
    /// ȷ���Ƿ�����ָ���Ľ�ɫ
    /// </summary>
    /// <param name="role">��ɫ</param>
    public bool IsInRole(string role)
    {
      if (!IsAuthenticated)
        return false;
      if (HaveAdminRole)
        return true;
      return DoIsInRole(role);
    }

    private bool DoIsInRole(string role)
    {
      return Roles != null && Roles.ContainsKey(role);
    }

    /// <summary>
    /// ȷ���Ƿ񱻾ܾ�
    /// </summary>
    /// <param name="allowRoles">���ý�ɫ����</param>
    /// <param name="denyRoles">���ý�ɫ����</param>
    public bool IsByDeny(IList<string> allowRoles, IList<string> denyRoles)
    {
      if (!IsAuthenticated)
        return true;
      if (HaveAdminRole)
        return false;
      if (Roles == null || Roles.Count == 0)
        return EmptyRolesIsDeny || allowRoles != null && allowRoles.Count < DataDictionaryHub.RoleInfos.Count;
      if (allowRoles == null && denyRoles == null)
        return false;
      if (denyRoles != null && denyRoles.Count > 0)
      {
        if (EasyAuthorization)
        {
          //ֻҪ�û�ӵ�еĽ�ɫ֮һ���ڽ��ý�ɫ������Ͳ�������
          foreach (KeyValuePair<string, RoleInfo> kvp in Roles)
            if (!denyRoles.Contains(kvp.Key))
              return false;
        }
        //ֻҪ�û�ӵ�еĽ�ɫ֮һ�ڽ��ý�ɫ������ͱ�����
        foreach (KeyValuePair<string, RoleInfo> kvp in Roles)
          if (denyRoles.Contains(kvp.Key))
            return true;
      }
      if (allowRoles != null && allowRoles.Count > 0)
        foreach (var item in allowRoles)
          if (Roles.ContainsKey(item))
            return false;
      return true;
    }

    /// <summary>
    /// ȷ���Ƿ񱻾ܾ�
    /// </summary>
    public static bool IsByDeny(UserIdentity identity, Type objectType, string methodName, bool throwIfDeny)
    {
      ClassMapInfo classMapInfo = ClassMemberHelper.GetClassMapInfo(objectType);
      if (classMapInfo != null)
      {
        MethodMapInfo methodMapInfo;
        if (classMapInfo.MethodMapInfo.TryGetValue(methodName, out methodMapInfo))
          if (methodMapInfo.DenyExecute(identity))
          {
            if (throwIfDeny)
              throw new System.Security.SecurityException(String.Format("{0}:Execute({1}.{2})", identity != null ? identity.Name : null, classMapInfo.FriendlyName, methodMapInfo.FriendlyName));
            return true;
          }
      }
      return false;
    }

    /// <summary>
    /// ȷ���Ƿ񱻾ܾ�
    /// </summary>
    public static bool IsByDeny(UserIdentity identity, object obj, MethodAction action, string methodName, bool? isNew, bool throwIfDeny)
    {
      ClassMapInfo classMapInfo = ClassMemberHelper.GetClassMapInfo(obj.GetType());
      if (classMapInfo != null)
        if (action == MethodAction.Execute)
        {
          MethodMapInfo methodMapInfo;
          if (classMapInfo.MethodMapInfo.TryGetValue(methodName, out methodMapInfo))
            if (methodMapInfo.DenyExecute(identity))
            {
              if (throwIfDeny)
                throw new System.Security.SecurityException(String.Format("{0}:{1}({2}.{3})", identity != null ? identity.Name : null, action, classMapInfo.FriendlyName, methodMapInfo.FriendlyName));
              return true;
            }
        }
        else
        {
          bool result = false;
          FieldMapInfo fieldMapInfo;
          if (classMapInfo.PropertyFieldMapInfos.TryGetValue(methodName, out fieldMapInfo))
            switch (action)
            {
              case MethodAction.WriteProperty:
                if (IsByDeny(identity, classMapInfo, ExecuteAction.Update, throwIfDeny))
                  result = true;
                else if (identity != null && !identity.AllowSet(obj))
                  result = true;
                else
                {
                  if (!isNew.HasValue)
                    throw new ArgumentNullException("isNew");
                  if (fieldMapInfo.DenyWrite(isNew.Value, identity))
                    result = true;
                }
                break;
              case MethodAction.ReadProperty:
                if (fieldMapInfo.DenyRead(identity))
                  result = true;
                break;
            }
          if (throwIfDeny && result)
            throw new System.Security.SecurityException(String.Format("{0}:{1}({2}.{3})", identity != null ? identity.Name : null, action, classMapInfo.FriendlyName, fieldMapInfo.FriendlyName));
        }
      return false;
    }

    /// <summary>
    /// ȷ���Ƿ񱻾ܾ�
    /// </summary>
    public static bool IsByDeny(UserIdentity identity, Type objectType, ExecuteAction action, bool throwIfDeny)
    {
      return IsByDeny(identity, ClassMemberHelper.GetClassMapInfo(objectType), action, throwIfDeny);
    }

    /// <summary>
    /// ȷ���Ƿ񱻾ܾ�
    /// </summary>
    public static bool IsByDeny(UserIdentity identity, ClassMapInfo classMapInfo, ExecuteAction action, bool throwIfDeny)
    {
      if (classMapInfo != null)
      {
        if ((action & ExecuteAction.Fetch) == ExecuteAction.Fetch && classMapInfo.DenyGet(identity) ||
          (action & ExecuteAction.Insert) == ExecuteAction.Insert && classMapInfo.DenyCreate(identity) ||
          (action & ExecuteAction.Update) == ExecuteAction.Update && classMapInfo.DenyEdit(identity) ||
          (action & ExecuteAction.Delete) == ExecuteAction.Delete && classMapInfo.DenyDelete(identity))
        {
          if (throwIfDeny)
            throw new System.Security.SecurityException(String.Format("{0}:{1}({2})", identity != null ? identity.Name : null, action, classMapInfo.FriendlyName));
          return true;
        }
      }
      return false;
    }

    /// <summary>
    /// ȷ���Ƿ�����ָ���Ĳ���
    /// </summary>
    /// <param name="departmentIds">����ID</param>
    public bool IsInDepartments(IList<long> departmentIds)
    {
      if (!IsAuthenticated)
        return false;
      if (HaveAdminRole)
        return true;
      if (departmentIds == null || DepartmentId == null)
        return true;
      if (departmentIds.Count == 0 || departmentIds.Contains(DepartmentId.Value))
        return true;
      foreach (DepartmentInfo item in Department.AllSuperior)
        if (departmentIds.Contains(item.Id))
          return true;
      return false;
    }

    /// <summary>
    /// ��Guest����
    /// </summary>
    public static bool IsGuestUserNumber(string userNumber)
    {
      return String.CompareOrdinal(userNumber, GuestUserNumber) == 0;
    }

    /// <summary>
    /// CheckIn
    /// </summary>
    public static void CheckIn(UserIdentity identity, string timestamp, string signature, bool allowTimestampRepeated, bool reset)
    {
      if (identity == null)
        throw new UserVerifyException();

      if (!identity.IsGuest)
      {
        if (!identity.IsAuthenticated)
          throw new UserVerifyException();
        if (String.Compare(RijndaelCryptoTextProvider.Encrypt(identity.Password, timestamp), signature, StringComparison.OrdinalIgnoreCase) != 0 &&
          String.Compare(RijndaelCryptoTextProvider7.Encrypt(MD5CryptoTextProvider.ComputeHash(identity.Password), timestamp), signature, StringComparison.OrdinalIgnoreCase) != 0 &&
          (identity.DynamicPassword == null || String.Compare(RijndaelCryptoTextProvider.Encrypt(identity.DynamicPassword, timestamp), signature, StringComparison.OrdinalIgnoreCase) != 0))
          throw new UserVerifyException(Phenix.Core.Properties.Resources.UserSignatureException);

        UserIdentity oldUserIdentity;
        if (!_usersCache.TryGetValue(identity.UserNumber, out oldUserIdentity))
          oldUserIdentity = identity;
        if (!reset && identity.SessionExpiresMinutes > 0 && DateTime.Now.Subtract(oldUserIdentity.LastOperationTime).TotalMinutes >= identity.SessionExpiresMinutes)
          throw new UserLockedException(identity.UserNumber, String.Format(Phenix.Core.Properties.Resources.UserStillTimeoutException, oldUserIdentity.LastOperationTime, identity.SessionExpiresMinutes));
        if (reset && !identity.AllowUserMultipleAddressLogin && String.CompareOrdinal(oldUserIdentity.LocalAddress, identity.LocalAddress) != 0)
          throw new UserVerifyException(Phenix.Core.Properties.Resources.UserMultipleAddressLoginException);
        _usersCache[identity.UserNumber] = identity;

        if (allowTimestampRepeated)
          lock (identity)
          {
            if (reset)
              identity._timestampList = new List<string>();
            else if (identity._timestampList == null)
              identity._timestampList = oldUserIdentity._timestampList != null 
                ? new List<string>(oldUserIdentity._timestampList) 
                : new List<string>();
            if (identity._timestampList.Contains(timestamp))
              throw new UserVerifyException(String.Format(Phenix.Core.Properties.Resources.UserTimestampRepeatedException, timestamp));
            while (identity._timestampList.Count >= 10000)
              identity._timestampList.RemoveAt(0);
            identity._timestampList.Add(timestamp);
          }
      }
    }

    #endregion

    /// <summary>
    /// �ǳ�
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
    public void LogOff()
    {
      try
      {
        DataSecurityHub.LogOff(this);
      }
      catch (System.Exception ex)
      {
        EventLog.SaveLocal(MethodBase.GetCurrentMethod(), ex);
      }
    }

    /// <summary>
    /// �޸ĵ�¼����
    /// </summary>
    /// <param name="newPassword">�µ�¼����</param>
    public bool ChangePassword(string newPassword)
    {
      return DataSecurityHub.ChangePassword(newPassword, this);
    }

    #endregion

    /// <summary>
    /// ������¼����
    /// </summary>
    /// <returns>�ҵ���¼���Ų������ɹ�</returns>
    public static bool UnlockPassword(string userNumber)
    {
      return DataSecurityHub.UnlockPassword(userNumber, CurrentIdentity);
    }

    /// <summary>
    /// ���õ�¼����
    /// </summary>
    /// <returns>�ҵ���¼���Ų����óɹ�</returns>
    public static bool ResetPassword(string userNumber)
    {
      return DataSecurityHub.ResetPassword(userNumber, CurrentIdentity);
    }

    /// <summary>
    /// ���õ�¼����
    /// </summary>
    /// <returns>�ҵ���¼���Ų����óɹ�</returns>
    public static bool ResetPassword(DbTransaction transaction, string userNumber)
    {
      return DataSecurityHub.ChangePassword(transaction, userNumber, userNumber);
    }
    
    /// <summary>
    /// ���õ�¼����
    /// </summary>
    /// <returns>�ҵ���¼���Ų����óɹ�</returns>
    public static bool ChangePassword(DbTransaction transaction, string userNumber, string newPassword)
    {
      return DataSecurityHub.ChangePassword(transaction, userNumber, newPassword);
    }
  }
}