namespace Phenix.Test.使用指南._21._7.Plugin
{
  /// <summary>
  /// </summary>
  [System.SerializableAttribute(), System.ComponentModel.DisplayNameAttribute("")]
  public class AssemblyCriteria : Phenix.Business.CriteriaBase
  {
    [Phenix.Core.Mapping.CriteriaField(FriendlyName = "AS_NAME", Logical = Phenix.Core.Mapping.CriteriaLogical.Or, Operate = Phenix.Core.Mapping.CriteriaOperate.Like, TableName = "PH_ASSEMBLY", ColumnName = "AS_NAME")]
    private string _name;
    /// <summary>
    /// AS_NAME
    /// </summary>
    [System.ComponentModel.DataAnnotations.Display(Name = "AS_NAME")]
    [System.ComponentModel.DisplayName("AS_NAME")]
    public string Name
    {
      get { return _name; }
      set { _name = value; PropertyHasChanged(); }
    }
  }
}
