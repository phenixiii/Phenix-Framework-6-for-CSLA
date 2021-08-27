
namespace Phenix.Security.Business
{
  /// <summary>
  /// 执行"登录解锁"授权规则
  /// </summary>
  public class UserUnlockExecuteAuthorizationRule : Phenix.Business.Rules.ExecuteAuthorizationRule
  {
    /// <summary>
    /// 初始化
    /// </summary>
    public UserUnlockExecuteAuthorizationRule()
      : base(User.UnlockMethod) { }

    #region Register

    /// <summary>
    /// 注册授权规则
    /// </summary>
    public static void Register()
    {
      User.AuthorizationRuleRegistering += new Phenix.Business.Core.AuthorizationRuleRegisteringEventHandler(User_AuthorizationRuleRegistering);  
    }

    private static void User_AuthorizationRuleRegistering(Phenix.Business.Rules.AuthorizationRules authorizationRules)
    {
      authorizationRules.AddRule(new UserUnlockExecuteAuthorizationRule());
    }

    #endregion

    /// <summary>
    /// 执行
    /// </summary>
    protected override void Execute(Phenix.Business.Rules.AuthorizationContext context)
    {
      context.HasPermission = ((User)context.Target).Locked ?? false;
      if (!context.HasPermission)
        context.DenyMessage = "不允许解锁";
    }
  }
}