using System;
using System.Collections.Generic;
using Phenix.Core.Mapping;
using Phenix.Core.Security;

namespace Phenix.Core.Dictionary
{
  /// <summary>
  /// 程序集类资料
  /// </summary>
  [Serializable]
  public sealed class AssemblyClassInfo
  {
    internal AssemblyClassInfo(AssemblyInfo owner, 
      string name, string caption, bool captionConfigured,
      ExecuteAction permanentExecuteAction, bool permanentExecuteConfigured,
      AssemblyClassType classType, bool authorised,
      IList<string> denyCreateRoles, IList<string> denyGetRoles,
      IList<string> denyEditRoles, IList<string> denyDeleteRoles,
      IList<long> departmentIds,
      IDictionary<string, AssemblyClassPropertyInfo> classPropertyInfos,
      IDictionary<string, AssemblyClassMethodInfo> classMethodInfos)
    {
      _owner = owner;
      _name = name;
      _caption = caption;
      _captionConfigured = captionConfigured;
      _permanentExecuteAction = permanentExecuteAction;
      _permanentExecuteConfigured = permanentExecuteConfigured;
      _classType = classType;
      _authorised = authorised;
      _denyCreateRoles = denyCreateRoles;
      _denyGetRoles = denyGetRoles;
      _denyEditRoles = denyEditRoles;
      _denyDeleteRoles = denyDeleteRoles;
      _departmentIds = departmentIds;
      _classPropertyInfos = classPropertyInfos;
      _classMethodInfos = classMethodInfos;

      foreach (KeyValuePair<string, AssemblyClassPropertyInfo> kvp in classPropertyInfos)
        kvp.Value.Owner = this;
      foreach (KeyValuePair<string, AssemblyClassMethodInfo> kvp in classMethodInfos)
        kvp.Value.Owner = this;
    }

    #region 属性

    private readonly AssemblyInfo _owner;
    /// <summary>
    /// 拥有者
    /// </summary>
    public AssemblyInfo Owner
    {
      get { return _owner; }
    }

    private readonly string _name;
    /// <summary>
    /// 名称
    /// </summary>
    public string Name
    {
      get { return _name; }
    }

    private readonly string _caption;
    /// <summary>
    /// 标签
    /// </summary>
    public string Caption
    {
      get { return _caption; }
    }

    private readonly bool _captionConfigured;
    /// <summary>
    /// 标签已被配置
    /// </summary>
    public bool CaptionConfigured
    {
      get { return _captionConfigured; }
    }

    private readonly ExecuteAction _permanentExecuteAction;
    /// <summary>
    /// 指示当处于哪种执行动作时本字段需要记录新旧值
    /// </summary>
    public ExecuteAction PermanentExecuteAction
    {
      get { return _permanentExecuteAction; }
    }

    private readonly bool _permanentExecuteConfigured;
    /// <summary>
    /// 持久化执行变更方式已被配置
    /// </summary>
    public bool PermanentExecuteConfigured
    {
      get { return _permanentExecuteConfigured; }
    }

    private readonly AssemblyClassType _classType;
    /// <summary>
    /// 类型
    /// </summary>
    public AssemblyClassType ClassType
    {
      get { return _classType; }
    }

    private readonly bool _authorised;
    /// <summary>
    /// 可被授权
    /// 当为false时XXXRoles属性都为空
    /// </summary>
    public bool Authorised
    {
      get { return _authorised; }
    }

    private readonly IList<string> _denyCreateRoles;
    /// <summary>
    /// 拒绝Create的角色
    /// </summary>
    public IList<string> DenyCreateRoles
    {
      get { return _denyCreateRoles; }
    }

    [NonSerialized]
    private IList<string> _allowCreateRoles;
    /// <summary>
    /// 允许Create的角色
    /// </summary>
    public IList<string> AllowCreateRoles
    {
      get 
      {
        if (_allowCreateRoles == null)
          _allowCreateRoles = DataDictionaryHub.InverseRoles(DenyCreateRoles);
        return _allowCreateRoles; 
      }
    }

    private readonly IList<string> _denyGetRoles;
    /// <summary>
    /// 拒绝Get的角色
    /// </summary>
    public IList<string> DenyGetRoles
    {
      get { return _denyGetRoles; }
    }

    [NonSerialized]
    private IList<string> _allowGetRoles;
    /// <summary>
    /// 允许Get的角色
    /// </summary>
    public IList<string> AllowGetRoles
    {
      get
      {
        if (_allowGetRoles == null)
          _allowGetRoles = DataDictionaryHub.InverseRoles(DenyGetRoles);
        return _allowGetRoles;
      }
    }

    private readonly IList<string> _denyEditRoles;
    /// <summary>
    /// 拒绝Edit的角色
    /// </summary>
    public IList<string> DenyEditRoles
    {
      get { return _denyEditRoles; }
    }

    [NonSerialized]
    private IList<string> _allowEditRoles;
    /// <summary>
    /// 允许Edit的角色
    /// </summary>
    public IList<string> AllowEditRoles
    {
      get
      {
        if (_allowEditRoles == null)
          _allowEditRoles = DataDictionaryHub.InverseRoles(DenyEditRoles);
        return _allowEditRoles;
      }
    }

    private readonly IList<string> _denyDeleteRoles;
    /// <summary>
    /// 拒绝Delete的角色
    /// </summary>
    public IList<string> DenyDeleteRoles
    {
      get { return _denyDeleteRoles; }
    }

    [NonSerialized]
    private IList<string> _allowDeleteRoles;
    /// <summary>
    /// 允许Delete的角色
    /// </summary>
    public IList<string> AllowDeleteRoles
    {
      get
      {
        if (_allowDeleteRoles == null)
          _allowDeleteRoles = DataDictionaryHub.InverseRoles(DenyDeleteRoles);
        return _allowDeleteRoles;
      }
    }
    
    private readonly IList<long> _departmentIds;
    /// <summary>
    /// 所属部门ID
    /// </summary>
    public IList<long> DepartmentIds
    {
      get { return _departmentIds; }
    }

    private readonly IDictionary<string, AssemblyClassPropertyInfo> _classPropertyInfos;
    /// <summary>
    /// 程序集类属性资料队列
    /// </summary>
    public IDictionary<string, AssemblyClassPropertyInfo> ClassPropertyInfos
    {
      get { return _classPropertyInfos; }
    }

    private readonly IDictionary<string, AssemblyClassMethodInfo> _classMethodInfos;
    /// <summary>
    /// 程序集类方法资料队列
    /// </summary>
    public IDictionary<string, AssemblyClassMethodInfo> ClassMethodInfos
    {
      get { return _classMethodInfos; }
    }

    #endregion

    #region 方法

    /// <summary>
    /// 取程序集类属性资料
    /// </summary>
    /// <param name="propertyName">属性名称</param>
    public AssemblyClassPropertyInfo GetClassPropertyInfo(string propertyName)
    {
      if (!String.IsNullOrEmpty(propertyName))
      {
        AssemblyClassPropertyInfo result;
        if (_classPropertyInfos != null && _classPropertyInfos.TryGetValue(propertyName, out result))
          return result;
      }
      return null;
    }

    /// <summary>
    /// 取程序集类方法资料
    /// </summary>
    /// <param name="methodName">方法名称</param>
    public AssemblyClassMethodInfo GetClassMethodInfo(string methodName)
    {
      if (!String.IsNullOrEmpty(methodName))
      {
        AssemblyClassMethodInfo result;
        if (_classMethodInfos != null && _classMethodInfos.TryGetValue(methodName, out result))
          return result;
      }
      return null;
    }

    #region Authentication

    /// <summary>
    /// 是否拒绝Create
    /// identity = Phenix.Core.Security.UserIdentity.CurrentIdentity
    /// </summary>
    public bool DenyCreate()
    {
      return DenyCreate(UserIdentity.CurrentIdentity);
    }

    /// <summary>
    /// 是否拒绝Create
    /// </summary>
    public bool DenyCreate(UserIdentity identity)
    {
      if (identity == null)
        return !AppConfig.AutoMode;
      if (!Authorised)
        return false;
      if (!identity.IsInDepartments(DepartmentIds))
        return true;
      return identity.IsByDeny(AllowCreateRoles, DenyCreateRoles);
    }

    /// <summary>
    /// 是否拒绝Get
    /// identity = Phenix.Core.Security.UserIdentity.CurrentIdentity
    /// </summary>
    public bool DenyGet()
    {
      return DenyGet(UserIdentity.CurrentIdentity);
    }

    /// <summary>
    /// 是否拒绝Get
    /// </summary>
    public bool DenyGet(UserIdentity identity)
    {
      if (identity == null)
        return !AppConfig.AutoMode;
      if (!Authorised)
        return false;
      if (!identity.IsInDepartments(DepartmentIds))
        return true;
      return identity.IsByDeny(AllowGetRoles, DenyGetRoles);
    }

    /// <summary>
    /// 是否拒绝Edit
    /// identity = Phenix.Core.Security.UserIdentity.CurrentIdentity
    /// </summary>
    public bool DenyEdit()
    {
      return DenyEdit(UserIdentity.CurrentIdentity);
    }

    /// <summary>
    /// 是否拒绝Edit
    /// </summary>
    public bool DenyEdit(UserIdentity identity)
    {
      if (identity == null)
        return !AppConfig.AutoMode;
      if (!Authorised)
        return false;
      if (!identity.IsInDepartments(DepartmentIds))
        return true;
      return identity.IsByDeny(AllowEditRoles, DenyEditRoles);
    }

    /// <summary>
    /// 是否拒绝Delete
    /// identity = Phenix.Core.Security.UserIdentity.CurrentIdentity
    /// </summary>
    public bool DenyDelete()
    {
      return DenyDelete(UserIdentity.CurrentIdentity);
    }

    /// <summary>
    /// 是否拒绝Delete
    /// </summary>
    public bool DenyDelete(UserIdentity identity)
    {
      if (identity == null)
        return !AppConfig.AutoMode;
      if (!Authorised)
        return false;
      if (!identity.IsInDepartments(DepartmentIds))
        return true;
      return identity.IsByDeny(AllowDeleteRoles, DenyDeleteRoles);
    }

    #endregion

    #endregion
  }
}