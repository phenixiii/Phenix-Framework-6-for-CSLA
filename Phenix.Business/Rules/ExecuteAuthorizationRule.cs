using System;
using Phenix.Core.Mapping;

namespace Phenix.Business.Rules
{
  /// <summary>
  /// 方法授权规则基类
  /// </summary>
  public abstract class ExecuteAuthorizationRule : Csla.Rules.AuthorizationRule, IAuthorizationRule
  {
    /// <summary>
    /// 初始化
    /// </summary>
    protected ExecuteAuthorizationRule(Phenix.Core.Mapping.IMethodInfo methodInfo)
      : base(Csla.Rules.AuthorizationActions.ExecuteMethod, new Csla.MethodInfo(methodInfo.Name))
    {
      _methodInfo = methodInfo;
    }

    #region 属性

    private readonly Phenix.Core.Mapping.IMethodInfo _methodInfo;
    /// <summary>
    /// 方法信息
    /// </summary>
    protected Phenix.Core.Mapping.IMethodInfo MethodInfo
    {
      get { return _methodInfo; }
    }

    /// <summary>
    /// 元素(属性/方法)名称
    /// </summary>
    protected string ElementName
    {
      get { return ((Csla.Rules.IAuthorizationRule)this).Element.Name; }
    }
    string IAuthorizationRule.ElementName
    {
      get { return ElementName; }
    }

    /// <summary>
    /// 元素(属性/方法)
    /// </summary>
    protected new IMemberInfo Element
    {
      get { return MethodInfo; }
    }
    IMemberInfo IAuthorizationRule.Element
    {
      get { return Element; }
    }

    /// <summary>
    /// 授权活动
    /// </summary>
    public new MethodAction Action
    {
      get
      {
        switch (((Csla.Rules.IAuthorizationRule)this).Action)
        {
          case Csla.Rules.AuthorizationActions.ExecuteMethod:
            return MethodAction.Execute;
          default:
            throw new InvalidOperationException(Action.ToString());
        }
      }
    }

    /// <summary>
    /// 是否缓存
    /// </summary>
    public override bool CacheResult
    {
      get { return false; }
    }

    #endregion

    #region 方法

    /// <summary>
    /// 执行
    /// </summary>
    /// <param name="context">授权上下文</param>
    protected override void Execute(Csla.Rules.AuthorizationContext context)
    {
      throw new NotImplementedException();
    }
    /// <summary>
    /// 执行
    /// </summary>
    /// <param name="context">授权上下文</param>
    protected abstract void Execute(AuthorizationContext context);
    void IAuthorizationRule.Execute(AuthorizationContext context)
    {
      Execute(context);
    }

    #endregion
  }
}