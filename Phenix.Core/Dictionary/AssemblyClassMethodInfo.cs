using System;
using System.Collections.Generic;
using Phenix.Core.Security;

namespace Phenix.Core.Dictionary
{
  /// <summary>
  /// 程序集类方法资料
  /// </summary>
  [Serializable]
  public sealed class AssemblyClassMethodInfo
  {
    /// <summary>
    /// 初始化
    /// </summary>
    [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
    public AssemblyClassMethodInfo(string caption, bool captionConfigured, string tag,
      bool? allowVisible,
      IList<string> denyExecuteRoles,
      IList<long> departmentIds)
    {
      _caption = caption;
      _captionConfigured = captionConfigured;
      _tag = tag;
      _allowVisible = allowVisible;
      _denyExecuteRoles = denyExecuteRoles;
      _departmentIds = departmentIds;
    }

    #region 属性

    private AssemblyClassInfo _owner;
    /// <summary>
    /// 所属程序集类
    /// </summary>
    public AssemblyClassInfo Owner
    {
      get { return _owner; }
      internal set { _owner = value; }
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

    private readonly string _tag;
    /// <summary>
    /// 标记
    /// </summary>
    public string Tag
    {
      get { return _tag; }
    }

    private readonly bool? _allowVisible;
    /// <summary>
    /// 是否允许显示即使没权限
    /// </summary>
    public bool? AllowVisible
    {
      get { return _allowVisible; }
    }

    private readonly IList<string> _denyExecuteRoles;
    /// <summary>
    /// 拒绝Execute的角色
    /// </summary>
    public IList<string> DenyExecuteRoles
    {
      get { return _denyExecuteRoles; }
    }

    [NonSerialized]
    private IList<string> _allowExecuteRoles;
    /// <summary>
    /// 允许Execute的角色
    /// </summary>
    public IList<string> AllowExecuteRoles
    {
      get
      {
        if (_allowExecuteRoles == null)
          _allowExecuteRoles = DataDictionaryHub.InverseRoles(DenyExecuteRoles);
        return _allowExecuteRoles;
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

    #endregion

    #region 方法

    #region Authentication

    /// <summary>
    /// 是否拒绝执行
    /// identity = Phenix.Core.Security.UserIdentity.CurrentIdentity
    /// </summary>
    public bool DenyExecute()
    {
      return DenyExecute(UserIdentity.CurrentIdentity);
    }

    /// <summary>
    /// 是否拒绝执行
    /// </summary>
    public bool DenyExecute(UserIdentity identity)
    {
      if (identity == null)
        return !AppConfig.AutoMode;
      if (!Owner.Authorised)
        return false;
      if (!identity.IsInDepartments(DepartmentIds))
        return true;
      return identity.IsByDeny(AllowExecuteRoles, DenyExecuteRoles);
    }

    #endregion

    #endregion
  }
}