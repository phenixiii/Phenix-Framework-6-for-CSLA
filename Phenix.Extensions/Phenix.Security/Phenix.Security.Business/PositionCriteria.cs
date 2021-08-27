
namespace Phenix.Security.Business
{
  /// <summary>
  /// 岗位查询
  /// </summary>
  [System.SerializableAttribute(), System.ComponentModel.DisplayNameAttribute("岗位查询")]
  public class PositionCriteria : Phenix.Business.CriteriaBase
  {
    [Phenix.Core.Mapping.CriteriaField(FriendlyName = "名称", Logical = Phenix.Core.Mapping.CriteriaLogical.Or, Operate = Phenix.Core.Mapping.CriteriaOperate.Equal, TableName = "PH_POSITION", ColumnName = "PT_NAME")]
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

    [Phenix.Core.Mapping.CriteriaField(FriendlyName = "代码", Logical = Phenix.Core.Mapping.CriteriaLogical.Or, Operate = Phenix.Core.Mapping.CriteriaOperate.Equal, TableName = "PH_POSITION", ColumnName = "PT_CODE")]
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

    [Phenix.Core.Mapping.CriteriaField(FriendlyName = "PT_ID", Logical = Phenix.Core.Mapping.CriteriaLogical.Or, Operate = Phenix.Core.Mapping.CriteriaOperate.Equal, TableName = "PH_POSITION", ColumnName = "PT_ID")]
    private long? _PT_ID;
    /// <summary>
    /// PT_ID
    /// </summary>
    [System.ComponentModel.DisplayName("PT_ID")]
    public long? PT_ID
    {
      get { return _PT_ID; }
      set { _PT_ID = value; PropertyHasChanged(); }
    }
  }
}
