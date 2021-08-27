
namespace Phenix.Security.Business
{
  /// <summary>
  /// 切片查询
  /// </summary>
  [System.SerializableAttribute(), System.ComponentModel.DisplayNameAttribute("切片查询")]
  public class SectionCriteria : Phenix.Business.CriteriaBase
  {
    [Phenix.Core.Mapping.CriteriaField(FriendlyName = "名称", Logical = Phenix.Core.Mapping.CriteriaLogical.Or, Operate = Phenix.Core.Mapping.CriteriaOperate.Equal, TableName = "PH_SECTION", ColumnName = "ST_NAME")]
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

    [Phenix.Core.Mapping.CriteriaField(FriendlyName = "标签", Logical = Phenix.Core.Mapping.CriteriaLogical.Or, Operate = Phenix.Core.Mapping.CriteriaOperate.Equal, TableName = "PH_SECTION", ColumnName = "ST_CAPTION")]
    private string _caption;
    /// <summary>
    /// 标签
    /// </summary>
    [System.ComponentModel.DisplayName("标签")]
    public string Caption
    {
      get { return _caption; }
      set { _caption = value; PropertyHasChanged(); }
    }
  }
}
