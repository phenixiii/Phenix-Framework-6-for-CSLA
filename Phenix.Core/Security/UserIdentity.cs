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
  /// 用户身份
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
    /// 初始化
    /// </summary>
    public UserIdentity(string userNumber, string password)
      : this(userNumber, password, null, NetConfig.LocalAddress, NetConfig.ServicesAddress) { }

    #region 工厂

    /// <summary>
    /// 构建测试用户
    /// </summary>
    public static UserIdentity CreateTester()
    {
      return new UserIdentity(true, AdminId, AdminUserName, AdminUserNumber, AdminUserNumber, null,
        0, NetConfig.LocalAddress, NetConfig.ServicesAddress, DateTime.Now, 
        null, null);
    }

    /// <summary>
    /// 构建未验证用户
    /// </summary>
    /// <param name="userId">用户ID</param>
    /// <param name="userName">用户名称</param>
    /// <param name="userNumber">登录工号</param>
    /// <param name="departmentId">部门ID</param>
    /// <param name="positionId">岗位ID</param>
    [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Advanced)]
    public static UserIdentity CreateUnauthenticated(long userId, string userName, string userNumber, 
      long? departmentId, long? positionId)
    {
      return new UserIdentity(false, userId, userName, userNumber, null, null,
        0, NetConfig.LocalAddress, NetConfig.ServicesAddress,
        DateTime.Now, departmentId, positionId);
    }

    /// <summary>
    /// 构建验证用户
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
    /// 构建匿名用户
    /// </summary>
    public static UserIdentity CreateGuest()
    {
      return new UserIdentity(GuestUserNumber, GuestUserNumber);
    }

    #endregion

    #region 属性

    #region Guest

    /// <summary>
    /// 匿名用户ID
    /// </summary>
    public static long GuestId
    {
      get { return -1; }
    }

    /// <summary>
    /// 匿名用户名称
    /// </summary>
    public static string GuestUserName
    {
      get { return "Guest"; }
    }

    /// <summary>
    /// 匿名用户登录工号
    /// </summary>
    public static string GuestUserNumber
    {
      get { return "Guest"; }
    }

    /// <summary>
    /// 是否匿名用户
    /// </summary>
    public bool IsGuest
    {
      get { return IsGuestUserNumber(UserNumber); }
    }

    #endregion

    #region Admin

    /// <summary>
    /// 管理员ID
    /// </summary>
    public static long AdminId
    {
      get { return 0; }
    }

    /// <summary>
    /// 管理员名称
    /// </summary>
    public static string AdminUserName
    {
      get { return "Administrator"; }
    }

    /// <summary>
    /// 管理员登录工号
    /// </summary>
    public static string AdminUserNumber
    {
      get { return "ADMIN"; }
    }

    /// <summary>
    /// 管理员角色名称
    /// </summary>
    public static string AdminRoleName
    {
      get { return "Admin"; }
    }

    [NonSerialized]
    private bool? _isAdmin;
    /// <summary>
    /// 是否管理员
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
    /// 是否拥有管理员角色
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

    #region IIdentity 成员

    /// <summary>
    /// 名称
    /// </summary>
    public string Name
    {
      get { return _userNumber; }
    }

    /// <summary>
    /// 身份验证类型
    /// </summary>
    public string AuthenticationType
    {
      get { return DataSecurityContext.InternalAuthenticationType; }
    }

    private bool _isAuthenticated;
    /// <summary>
    /// 已验证?
    /// </summary>
    public bool IsAuthenticated
    {
      get { return _isAuthenticated; }
      protected set { _isAuthenticated = value; }
    }

    private string _authenticatedMessage;
    /// <summary>
    /// 验证消息
    /// </summary>
    public string AuthenticatedMessage
    {
      get { return _authenticatedMessage; }
      protected internal set { _authenticatedMessage = value; }
    }

    private readonly long? _userId;
    /// <summary>
    /// 用户ID
    /// </summary>
    public long? UserId
    {
      get { return _userId; }
    }

    private readonly string _userName;
    /// <summary>
    /// 用户名称
    /// </summary>
    public string UserName
    {
      get { return _userName; }
    }

    private readonly string _userNumber;
    /// <summary>
    /// 登录工号
    /// </summary>
    public string UserNumber
    {
      get { return _userNumber; }
    }

    [NonSerialized]
    private string _password;
    /// <summary>
    /// 登录口令
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
    /// 登录口令
    /// </summary>
    [Newtonsoft.Json.JsonIgnore]
    public string DynamicPassword
    {
      get { return _dynamicPassword; }
      set { _dynamicPassword = value; }
    }

    private readonly long _serverVersion;
    /// <summary>
    /// 服务器版本
    /// </summary>
    public long ServerVersion
    {
      get { return _serverVersion; }
    }

    private readonly string _localAddress;
    /// <summary>
    /// 本地IP地址
    /// </summary>
    [Newtonsoft.Json.JsonIgnore]
    public string LocalAddress
    {
      get { return _localAddress; }
    }

    private readonly string _servicesAddress;
    /// <summary>
    /// 服务IP地址
    /// </summary>
    [Newtonsoft.Json.JsonIgnore]
    public string ServicesAddress
    {
      get { return _servicesAddress; }
    }

    /// <summary>
    /// 所属企业
    /// </summary>
    public string Enterprise
    {
      get { return DataDictionaryHub.Enterprise; }
    }

    private readonly long? _departmentId;
    /// <summary>
    /// 所属部门ID
    /// </summary>
    public long? DepartmentId
    {
      get { return _departmentId; }
    }

    [NonSerialized]
    private DepartmentInfo _department;
    /// <summary>
    /// 所属部门
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
    /// 担任岗位ID
    /// </summary>
    public long? PositionId
    {
      get { return _positionId; }
    }

    [NonSerialized]
    private PositionInfo _position;
    /// <summary>
    /// 担任岗位
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
    /// 上级(一级)
    /// (本部门的)上级岗位的用户、(本部门根岗位用户的)上级部门根岗位的用户
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
    /// 上级(全部)
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
    /// 工友
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
    /// 下级(一级)
    /// (本部门的)下级岗位的用户、(本部门根岗位用户的)下级部门根岗位的用户
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
    /// 下级(全部)
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
    /// 角色资料队列
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
    /// 可授权角色资料队列
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
    /// 切片资料队列
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
    /// 区域性名称
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
    /// 时间戳
    /// </summary>
    [Newtonsoft.Json.JsonIgnore]
    public string Timestamp
    {
      get { return _timestamp; }
    }

    private string _signature;
    /// <summary>
    /// 签名
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
    /// 最后活动时间
    /// </summary>
    public DateTime LastOperationTime
    {
      get { return _lastOperationTime; }
    }

    [NonSerialized]
    private bool? _allowUserMultipleAddressLogin;
    /// <summary>
    /// 允许用户多IP登录
    /// 不控制Guest用户
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
    /// 会话过期时间(分钟)
    /// 小于等于0不限制
    /// 不控制Guest用户
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
    /// (用户/功能)未配置角色代表授权规则为禁用
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
    /// 宽松的授权
    /// 缺省为 false
    /// true: 只要用户拥有的角色之一不在禁用角色队列里，就不被禁用
    /// false: 只要用户拥有的角色之一在禁用角色队列里，就被禁用
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
    /// 当前用户身份
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

    #region 方法

    #region 上下级关系

    /// <summary>
    /// 拥有上级
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
    /// 拥有下属
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

    #region 过程锁

    /// <summary>
    /// 过程加锁
    /// expiryTime = TimeSpan.Zero代表不自动失效
    /// </summary>
    /// <param name="processName">过程名</param>
    /// <param name="caption">标签</param>
    /// <param name="remark">备注</param>
    public void LockProcess(string processName, string caption, string remark)
    {
      DataSecurityHub.SetProcessLockInfo(processName, caption, true, TimeSpan.Zero, remark, this);
    }

    /// <summary>
    /// 过程加锁
    /// </summary>
    /// <param name="processName">过程名</param>
    /// <param name="caption">标签</param>
    /// <param name="expiryTime">限期, TimeSpan.Zero代表不自动失效</param>
    /// <param name="remark">备注</param>
    public void LockProcess(string processName, string caption, TimeSpan expiryTime, string remark)
    {
      DataSecurityHub.SetProcessLockInfo(processName, caption, true, expiryTime, remark, this);
    }

    /// <summary>
    /// 过程解锁
    /// remark = null
    /// </summary>
    /// <param name="processName">过程名</param>
    public void UnlockProcess(string processName)
    {
      DataSecurityHub.SetProcessLockInfo(processName, null, false, TimeSpan.Zero, null, this);
    }

    /// <summary>
    /// 过程解锁
    /// </summary>
    /// <param name="processName">过程名</param>
    /// <param name="remark">备注</param>
    public void UnlockProcess(string processName, string remark)
    {
      DataSecurityHub.SetProcessLockInfo(processName, null, false, TimeSpan.Zero, remark, this);
    }

    /// <summary>
    /// 取过程锁资料
    /// </summary>
    /// <param name="processName">过程名</param>
    public ProcessLockInfo GetProcessLockInfo(string processName)
    {
      return DataSecurityHub.GetProcessLockInfo(processName, this);
    }

    /// <summary>
    /// 是否允许操作过程锁
    /// </summary>
    /// <param name="processName">过程名</param>
    public bool AllowSetProcessLockInfo(string processName)
    {
      return AllowSetProcessLockInfo(GetProcessLockInfo(processName));
    }

    /// <summary>
    /// 是否允许操作过程锁
    /// </summary>
    /// <param name="info">过程锁资料</param>
    public bool AllowSetProcessLockInfo(ProcessLockInfo info)
    {
      if (info == null)
        return true;
      return info.AllowSet(this);
    }

    #endregion

    #region DefaultValue

    /// <summary>
    /// 设置正则表达式字段缺省值
    /// </summary>
    /// <param name="regexColumnName">正则表达式字段名</param>
    /// <param name="value">缺省值</param>
    public void SetFieldDefaultValue(string regexColumnName, object value)
    {
      if (regexColumnName == null)
        throw new ArgumentNullException("regexColumnName");
      _fieldDefaultValueCache[regexColumnName] = value;
    }

    /// <summary>
    /// 删除正则表达式字段缺省值
    /// </summary>
    /// <param name="regexColumnName">正则表达式字段名</param>
    /// <returns>是否删除</returns>
    public bool RemoveFieldDefaultValue(string regexColumnName)
    {
      if (regexColumnName == null)
        throw new ArgumentNullException("regexColumnName");
      return _fieldDefaultValueCache.Remove(regexColumnName);
    }

    /// <summary>
    /// 获取正则表达式字段缺省值
    /// </summary>
    /// <param name="regexColumnName">正则表达式字段名</param>
    /// <returns>字段缺省值</returns>
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
    /// 获取字段缺省值
    /// </summary>
    /// <param name="fieldMapInfo">数据映射字段</param>
    /// <returns>字段缺省值</returns>
    public object GetFieldDefaultValue(FieldMapInfo fieldMapInfo)
    {
      if (fieldMapInfo != null && !String.IsNullOrEmpty(fieldMapInfo.FullTableColumnName))
        foreach (KeyValuePair<string, object> kvp in _fieldDefaultValueCache)
          if (Regex.IsMatch(fieldMapInfo.FullTableColumnName, kvp.Key, RegexOptions.IgnoreCase))
            return Utilities.ChangeType(kvp.Value, fieldMapInfo.Field.FieldType);
      return null;
    }

    #endregion

    #region 过滤字段

    /// <summary>
    /// 获取过滤字段值队列
    /// </summary>
    /// <param name="fieldMapInfo">数据映射字段</param>
    /// <returns>字段值队列</returns>
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
    /// 注册正则表达式过滤字段值
    /// </summary>
    /// <param name="regexColumnName">正则表达式字段名</param>
    /// <param name="fieldValues">字段值</param>
    public void RegisterFilterFieldValue(string regexColumnName, params object[] fieldValues)
    {
      if (regexColumnName == null)
        throw new ArgumentNullException("regexColumnName");
      SynchronizedList<object> value = _filterFieldValuesCache.GetValue(regexColumnName, ()=> new SynchronizedList<object>(), true);
      if (fieldValues != null && fieldValues.Length > 0)
        value.AddRange(fieldValues);
    }

    /// <summary>
    /// 存在注册的正则表达式过滤字段值
    /// </summary>
    /// <param name="regexColumnName">正则表达式字段名</param>
    /// <param name="fieldValue">字段值</param>
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
    /// 注销正则表达式过滤字段值
    /// </summary>
    /// <param name="regexColumnName">正则表达式字段名</param>
    /// <param name="fieldValues">字段值</param>
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
    /// 获取注册的正则表达式过滤字段值
    /// </summary>
    /// <param name="regexColumnName">正则表达式字段名</param>
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
    /// 设置单一的正则表达式过滤字段值
    /// 设置后将禁用RegisterFilterFieldValue注册内容
    /// </summary>
    /// <param name="regexColumnName">正则表达式字段名</param>
    /// <param name="value">字段值</param>
    public void SetSingleFilterFieldValue(string regexColumnName, object value)
    {
      if (regexColumnName == null)
        throw new ArgumentNullException("regexColumnName");
      _singleFilterFieldValueCache[regexColumnName] = value;
    }

    /// <summary>
    /// 删除单一的正则表达式过滤字段值
    /// 删除后将恢复RegisterFilterFieldValue注册内容
    /// </summary>
    /// <param name="regexColumnName">正则表达式字段名</param>
    public bool RemoveSingleFilterFieldValue(string regexColumnName)
    {
      if (regexColumnName == null)
        throw new ArgumentNullException("regexColumnName");
      return _singleFilterFieldValueCache.Remove(regexColumnName);
    }

    /// <summary>
    /// 获取单一的正则表达式过滤字段值
    /// </summary>
    /// <param name="regexColumnName">正则表达式字段名</param>
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

    #region 切片

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
    /// 取本用户可操作的类所关联切片
    /// null代表不存在切片
    /// </summary>
    /// <param name="objectType">类</param>
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
    /// 取本用户可操作的类及其条件类所关联切片
    /// null代表不存在切片
    /// </summary>
    /// <param name="objectType">类</param>
    /// <param name="criteriaType">条件类</param>
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

    #region 权限验证

    /// <summary>
    /// 是否允许设置数据
    /// 只读则为false
    /// </summary>
    /// <param name="info">数据</param>
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
    /// 是否允许设置数据
    /// 只读则为false
    /// </summary>
    /// <param name="data">数据</param>
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
    /// 确定是否属于指定的角色
    /// </summary>
    /// <param name="role">角色</param>
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
    /// 确定是否被拒绝
    /// </summary>
    /// <param name="allowRoles">可用角色队列</param>
    /// <param name="denyRoles">禁用角色队列</param>
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
          //只要用户拥有的角色之一不在禁用角色队列里，就不被禁用
          foreach (KeyValuePair<string, RoleInfo> kvp in Roles)
            if (!denyRoles.Contains(kvp.Key))
              return false;
        }
        //只要用户拥有的角色之一在禁用角色队列里，就被禁用
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
    /// 确定是否被拒绝
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
    /// 确定是否被拒绝
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
    /// 确定是否被拒绝
    /// </summary>
    public static bool IsByDeny(UserIdentity identity, Type objectType, ExecuteAction action, bool throwIfDeny)
    {
      return IsByDeny(identity, ClassMemberHelper.GetClassMapInfo(objectType), action, throwIfDeny);
    }

    /// <summary>
    /// 确定是否被拒绝
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
    /// 确定是否属于指定的部门
    /// </summary>
    /// <param name="departmentIds">部门ID</param>
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
    /// 是Guest工号
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
    /// 登出
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
    /// 修改登录口令
    /// </summary>
    /// <param name="newPassword">新登录口令</param>
    public bool ChangePassword(string newPassword)
    {
      return DataSecurityHub.ChangePassword(newPassword, this);
    }

    #endregion

    /// <summary>
    /// 解锁登录口令
    /// </summary>
    /// <returns>找到登录工号并解锁成功</returns>
    public static bool UnlockPassword(string userNumber)
    {
      return DataSecurityHub.UnlockPassword(userNumber, CurrentIdentity);
    }

    /// <summary>
    /// 重置登录口令
    /// </summary>
    /// <returns>找到登录工号并重置成功</returns>
    public static bool ResetPassword(string userNumber)
    {
      return DataSecurityHub.ResetPassword(userNumber, CurrentIdentity);
    }

    /// <summary>
    /// 重置登录口令
    /// </summary>
    /// <returns>找到登录工号并重置成功</returns>
    public static bool ResetPassword(DbTransaction transaction, string userNumber)
    {
      return DataSecurityHub.ChangePassword(transaction, userNumber, userNumber);
    }
    
    /// <summary>
    /// 重置登录口令
    /// </summary>
    /// <returns>找到登录工号并重置成功</returns>
    public static bool ChangePassword(DbTransaction transaction, string userNumber, string newPassword)
    {
      return DataSecurityHub.ChangePassword(transaction, userNumber, newPassword);
    }
  }
}