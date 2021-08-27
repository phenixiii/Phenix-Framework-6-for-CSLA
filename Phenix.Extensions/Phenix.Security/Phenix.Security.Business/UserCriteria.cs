
namespace Phenix.Security.Business
{
  /// <summary>
  /// 用户查询
  /// </summary>
  [System.SerializableAttribute(), System.ComponentModel.DisplayNameAttribute("用户查询")]
  public class UserCriteria : Phenix.Business.CriteriaBase
  {
    [Phenix.Core.Mapping.CriteriaField(FriendlyName = "工号", Logical = Phenix.Core.Mapping.CriteriaLogical.Or, Operate = Phenix.Core.Mapping.CriteriaOperate.Equal, TableName = "PH_USER", ColumnName = "US_USERNUMBER")]
    private string _userNumber;
    /// <summary>
    /// 工号
    /// </summary>
    [System.ComponentModel.DisplayName("工号")]
    public string UserNumber
    {
      get { return _userNumber; }
      set { _userNumber = value; PropertyHasChanged(); }
    }

    [Phenix.Core.Mapping.CriteriaField(FriendlyName = "姓名", Logical = Phenix.Core.Mapping.CriteriaLogical.Or, Operate = Phenix.Core.Mapping.CriteriaOperate.Equal, TableName = "PH_USER", ColumnName = "US_NAME")]
    private string _name;
    /// <summary>
    /// 姓名
    /// </summary>
    [System.ComponentModel.DisplayName("姓名")]
    public string Name
    {
      get { return _name; }
      set { _name = value; PropertyHasChanged(); }
    }
  }
}
