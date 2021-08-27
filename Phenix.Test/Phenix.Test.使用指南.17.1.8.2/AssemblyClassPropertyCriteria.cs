namespace Phenix.Test.使用指南._17._1._8._2
{
  /// <summary>
  /// </summary>
  [System.SerializableAttribute(), System.ComponentModel.DisplayNameAttribute("")]
  public class AssemblyClassPropertyCriteria : Phenix.Business.CriteriaBase
  {
    [Phenix.Core.Mapping.CriteriaField(Operate = Phenix.Core.Mapping.CriteriaOperate.Like, Logical = Phenix.Core.Mapping.CriteriaLogical.And,
      FriendlyName = "属性名", SourceName = "PH_ASSEMBLYCLASSPROPERTY", Alias = "AP_NAME", TableName = "PH_ASSEMBLYCLASSPROPERTY", ColumnName = "AP_NAME")]
    private string _name;
    /// <summary>
    /// 属性名
    /// </summary>
    [System.ComponentModel.DisplayName("属性名")]
    public string Name
    {
      get { return _name; }
      set { _name = value; PropertyHasChanged(); }
    }
  }
}
