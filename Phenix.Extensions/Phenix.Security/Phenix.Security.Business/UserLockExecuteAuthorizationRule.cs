
namespace Phenix.Security.Business
{
  /// <summary>
  /// 执行"登录加锁"授权规则
  /// </summary>
  public class UserLockExecuteAuthorizationRule : Phenix.Business.Rules.ExecuteAuthorizationRule
  {
    /// <summary>
    /// 初始化
    /// </summary>
    public UserLockExecuteAuthorizationRule()
      : base(User.LockMethod) { }

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
      authorizationRules.AddRule(new UserLockExecuteAuthorizationRule());
    }

    #endregion

    /// <summary>
    /// 执行
    /// </summary>
    protected override void Execute(Phenix.Business.Rules.AuthorizationContext context)
    {
      context.HasPermission = !(((User)context.Target).Locked ?? false);
      if (!context.HasPermission)
        context.DenyMessage = "不允许加锁";
    }
  }
}