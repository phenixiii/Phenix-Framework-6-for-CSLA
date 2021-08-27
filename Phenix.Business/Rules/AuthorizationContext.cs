using System.Collections.Generic;

namespace Phenix.Business.Rules
{
  /// <summary>
  /// 授权上下文
  /// </summary>
  public class AuthorizationContext
  {
    internal AuthorizationContext(IAuthorizationRule rule, object target, params object[] arguments)
    {
      _rule = rule;
      _target = target;
      _arguments = arguments;
    }
    
    #region 属性
    
    private readonly IAuthorizationRule _rule;
    /// <summary>
    /// 授权规则
    /// </summary>
    public IAuthorizationRule Rule
    {
      get { return _rule; }
    }

    private readonly object _target;
    /// <summary>
    /// 目标对象
    /// </summary>
    public object Target
    { 
      get { return _target; } 
    }

    private readonly IList<object> _arguments;
    /// <summary>
    /// 参数
    /// </summary>
    public IList<object> Arguments
    {
      get { return _arguments; }
    }

    /// <summary>
    /// 表明是否许可
    /// </summary>
    public bool HasPermission { get; set; }

    /// <summary>
    /// 被拒绝时消息
    /// </summary>
    public string DenyMessage { get; set; }

    #endregion
  }
}
