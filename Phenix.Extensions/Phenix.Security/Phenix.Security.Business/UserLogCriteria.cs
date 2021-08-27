
namespace Phenix.Security.Business
{
  /// <summary>
  /// 用户日志查询
  /// </summary>
  [System.SerializableAttribute(), System.ComponentModel.DisplayNameAttribute("用户日志查询")]
  public class UserLogCriteria : Phenix.Business.CriteriaBase
  {
    [Phenix.Core.Mapping.CriteriaField(FriendlyName = "工号", Logical = Phenix.Core.Mapping.CriteriaLogical.And, Operate = Phenix.Core.Mapping.CriteriaOperate.Equal, TableName = "PH_USERLOG", ColumnName = "US_USERNUMBER")]
    private string _userNumber;  //private string _userNumber = Phenix.Business.Security.UserPrincipal.User != null ?  Phenix.Business.Security.UserPrincipal.User.Identity.UserNumber : null;
    /// <summary>
    /// 工号
    /// </summary>
    [System.ComponentModel.DisplayName("工号")]
    public string UserNumber
    {
      get { return _userNumber; }
      set { _userNumber = value; PropertyHasChanged(); }
    }
  }
}
