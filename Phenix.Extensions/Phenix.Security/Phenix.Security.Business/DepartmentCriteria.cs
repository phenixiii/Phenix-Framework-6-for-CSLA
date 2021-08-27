
namespace Phenix.Security.Business
{
  /// <summary>
  /// 部门查询
  /// </summary>
  [System.SerializableAttribute(), System.ComponentModel.DisplayNameAttribute("部门查询")]
  public class DepartmentCriteria : Phenix.Business.CriteriaBase
  {
    [Phenix.Core.Mapping.CriteriaField(FriendlyName = "名称", Logical = Phenix.Core.Mapping.CriteriaLogical.Or, Operate = Phenix.Core.Mapping.CriteriaOperate.Equal, TableName = "PH_DEPARTMENT", ColumnName = "DP_NAME")]
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

    [Phenix.Core.Mapping.CriteriaField(FriendlyName = "代码", Logical = Phenix.Core.Mapping.CriteriaLogical.Or, Operate = Phenix.Core.Mapping.CriteriaOperate.Equal, TableName = "PH_DEPARTMENT", ColumnName = "DP_CODE")]
    private string _code;
    /// <summary>
    /// 代码
    /// </summary>
    [System.ComponentModel.DisplayName("代码")]
    public string Code
    {
      get { return _code; }
      set { _code = value; PropertyHasChanged(); }
    }

    [Phenix.Core.Mapping.CriteriaField(FriendlyName = "DP_ID", Logical = Phenix.Core.Mapping.CriteriaLogical.Or, Operate = Phenix.Core.Mapping.CriteriaOperate.Equal, TableName = "PH_DEPARTMENT", ColumnName = "DP_ID")]
    private long? _DP_ID;
    /// <summary>
    /// DP_ID
    /// </summary>
    [System.ComponentModel.DisplayName("DP_ID")]
    public long? DP_ID
    {
      get { return _DP_ID; }
      set { _DP_ID = value; PropertyHasChanged(); }
    }
  }
}
