
namespace Phenix.Security.Business
{
  /// <summary>
  /// 角色查询
  /// </summary>
  [System.SerializableAttribute(), System.ComponentModel.DisplayNameAttribute("角色查询")]
  public class RoleCriteria : Phenix.Business.CriteriaBase
  {
    [Phenix.Core.Mapping.CriteriaField(Operate = Phenix.Core.Mapping.CriteriaOperate.Equal, Logical = Phenix.Core.Mapping.CriteriaLogical.And,
      FriendlyName = "名称", SourceName = "PH_ROLE", Alias = "RL_NAME", TableName = "PH_ROLE", ColumnName = "RL_NAME")]
    private string _name;
    /// <summary>
    /// 名称
    /// </summary>
    [System.ComponentModel.DisplayName("名称")]
    public string Name
    {
      get { return _name; }
      set { _name = value; PropertyHasChanged(); }
    }

    [Phenix.Core.Mapping.CriteriaField(Operate = Phenix.Core.Mapping.CriteriaOperate.Unequal, Logical = Phenix.Core.Mapping.CriteriaLogical.And,
      FriendlyName = "含超级角色", SourceName = "PH_ROLE", Alias = "RL_NAME", TableName = "PH_ROLE", ColumnName = "RL_NAME")]
    private string _includeAdmin = Phenix.Core.Security.UserIdentity.AdminRoleName;
    /// <summary>
    /// 含超级角色
    /// </summary>
    [System.ComponentModel.DisplayName("含超级角色")]
    public bool IncludeAdmin
    {
      get { return _includeAdmin == null; }
      set
      {
        _includeAdmin = !value ? Phenix.Core.Security.UserIdentity.AdminRoleName : null;
        PropertyHasChanged();
      }
    }
  }
}
