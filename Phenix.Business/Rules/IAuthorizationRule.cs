using Phenix.Core.Mapping;

namespace Phenix.Business.Rules
{
  /// <summary>
  /// 授权规则接口
  /// </summary>
  public interface IAuthorizationRule
  {
    #region 属性
    
    /// <summary>
    /// 元素(属性/方法)名称
    /// </summary>
    string ElementName { get; }

    /// <summary>
    /// 元素(属性/方法)
    /// </summary>
    IMemberInfo Element { get; }

    /// <summary>
    /// 授权活动
    /// </summary>
    MethodAction Action { get; }

    #endregion

    #region 方法

    /// <summary>
    /// 执行
    /// </summary>
    /// <param name="context">授权上下文</param>
    void Execute(AuthorizationContext context);

    #endregion
  }
}
