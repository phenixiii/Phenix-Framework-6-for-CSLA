namespace Phenix.Test.使用指南._17._1._8._2
{
  /// <summary>
  /// </summary>
  [System.SerializableAttribute(), System.ComponentModel.DisplayNameAttribute("")]
  public class AssemblyClassMethodCriteria : Phenix.Business.CriteriaBase
  {
    [Phenix.Core.Mapping.CriteriaField(Operate = Phenix.Core.Mapping.CriteriaOperate.Like, Logical = Phenix.Core.Mapping.CriteriaLogical.And,
      FriendlyName = "方法名", SourceName = "PH_ASSEMBLYCLASSMETHOD", Alias = "AM_NAME", TableName = "PH_ASSEMBLYCLASSMETHOD", ColumnName = "AM_NAME")]
    private string _name;
    /// <summary>
    /// 方法名
    /// </summary>
    [System.ComponentModel.DisplayName("方法名")]
    public string Name
    {
      get { return _name; }
      set { _name = value; PropertyHasChanged(); }
    }
  }
}
