using System;
using System.Collections.Generic;
using Phenix.Core.Mapping;
using Phenix.Core.Security;

namespace Phenix.Core.Dictionary
{
  /// <summary>
  /// 程序集类属性资料
  /// </summary>
  [Serializable]
  public sealed class AssemblyClassPropertyInfo
  {
    /// <summary>
    /// 初始化
    /// </summary>
    [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
    public AssemblyClassPropertyInfo(string caption, bool captionConfigured,
      ExecuteModify permanentExecuteModify, bool permanentExecuteConfigured,
      IList<string> denyReadRoles, IList<string> denyWriteRoles,
      bool configurable, string configValue, IDictionary<string, string> configValues,
      int indexNumber, bool? required, bool visible)
    {
      _caption = caption;
      _captionConfigured = captionConfigured;
      _permanentExecuteModify = permanentExecuteModify;
      _permanentExecuteConfigured = permanentExecuteConfigured;
      _denyReadRoles = denyReadRoles;
      _denyWriteRoles = denyWriteRoles;
      _configurable = configurable;
      _configValue = configValue;
      _configValues = configValues;
      _indexNumber = indexNumber;
      _required = required;
      _visible = visible;
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

    private readonly ExecuteModify _permanentExecuteModify;
    /// <summary>
    /// 指示当处于哪种执行变更时本字段需要记录新旧值
    /// </summary>
    public ExecuteModify PermanentExecuteModify
    {
      get { return _permanentExecuteModify; }
    }

    private readonly bool _permanentExecuteConfigured;
    /// <summary>
    /// 持久化执行变更方式已被配置
    /// </summary>
    public bool PermanentExecuteConfigured
    {
      get { return _permanentExecuteConfigured; }
    }

    private readonly int _indexNumber;
    /// <summary>
    /// 索引号
    /// </summary>
    public int IndexNumber
    {
      get { return _indexNumber; }
    }

    private readonly bool? _required;
    /// <summary>
    /// 是否必输
    /// </summary>
    public bool? Required
    {
      get { return _required; }
    }

    private readonly bool _visible;
    /// <summary>
    /// 是否可见
    /// </summary>
    public bool Visible
    {
      get { return _visible; }
    }

    private readonly IList<string> _denyReadRoles;
    /// <summary>
    /// 拒绝Read的角色
    /// </summary>
    public IList<string> DenyReadRoles
    {
      get { return _denyReadRoles; }
    }

    [NonSerialized]
    private IList<string> _allowReadRoles;
    /// <summary>
    /// 允许Read的角色
    /// </summary>
    public IList<string> AllowReadRoles
    {
      get
      {
        if (_allowReadRoles == null)
          _allowReadRoles = DataDictionaryHub.InverseRoles(DenyReadRoles);
        return _allowReadRoles;
      }
    }

    private readonly IList<string> _denyWriteRoles;
    /// <summary>
    /// 拒绝Write的角色
    /// </summary>
    public IList<string> DenyWriteRoles
    {
      get { return _denyWriteRoles; }
    }

    [NonSerialized]
    private IList<string> _allowWriteRoles;
    /// <summary>
    /// 允许Write的角色
    /// </summary>
    public IList<string> AllowWriteRoles
    {
      get
      {
        if (_allowWriteRoles == null)
          _allowWriteRoles = DataDictionaryHub.InverseRoles(DenyWriteRoles);
        return _allowWriteRoles;
      }
    }

    private readonly bool _configurable;
    /// <summary>
    /// 是否可配置的
    /// </summary>
    public bool Configurable
    {
      get { return _configurable; }
    }

    private readonly string _configValue;
    /// <summary>
    /// 配置的值
    /// </summary>
    public string ConfigValue
    {
      get { return _configValue; }
    }

    private readonly IDictionary<string, string> _configValues;

    #endregion

    #region 方法

    /// <summary>
    /// 取配置的值
    /// </summary>
    /// <param name="key">键</param>
    public string GetConfigValue(string key)
    {
      if (String.IsNullOrEmpty(key))
        return _configValue;
      string result;
      if (_configValues != null && _configValues.TryGetValue(key, out result))
        return result;
      return null;
    }

    #region Authentication

    /// <summary>
    /// 是否拒绝Read
    /// identity = Phenix.Core.Security.UserIdentity.CurrentIdentity
    /// </summary>
    public bool DenyRead()
    {
      return DenyRead(UserIdentity.CurrentIdentity);
    }

    /// <summary>
    /// 是否拒绝Read
    /// </summary>
    public bool DenyRead(UserIdentity identity)
    {
      if (identity == null)
        return !AppConfig.AutoMode;
      if (!Owner.Authorised)
        return false;
      return identity.IsByDeny(AllowReadRoles, DenyReadRoles);
    }

    /// <summary>
    /// 是否拒绝Write
    /// identity = Phenix.Core.Security.UserIdentity.CurrentIdentity
    /// </summary>
    public bool DenyWrite()
    {
      return DenyWrite(UserIdentity.CurrentIdentity);
    }

    /// <summary>
    /// 是否拒绝Write
    /// </summary>
    public bool DenyWrite(UserIdentity identity)
    {
      if (identity == null)
        return !AppConfig.AutoMode;
      if (!Owner.Authorised)
        return false;
      return identity.IsByDeny(AllowWriteRoles, DenyWriteRoles);
    }

    #endregion

    #endregion
  }
}