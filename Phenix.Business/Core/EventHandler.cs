using Phenix.Business.Rules;

namespace Phenix.Business.Core
{
  #region Rules

  /// <summary>
  /// 申明业务规则注册中事件处理函数
  /// </summary>
  /// <param name="businessRules">业务规则库</param>
  public delegate void BusinessRuleRegisteringEventHandler(Csla.Rules.BusinessRules businessRules);

  /// <summary>
  /// 申明授权规则注册中事件处理函数
  /// </summary>
  /// <param name="authorizationRules">授权规则库</param>
  public delegate void AuthorizationRuleRegisteringEventHandler(AuthorizationRules authorizationRules);

  #endregion
}
