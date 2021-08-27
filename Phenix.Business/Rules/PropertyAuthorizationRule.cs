using System;
using Phenix.Core.Mapping;

namespace Phenix.Business.Rules
{
  /// <summary>
  /// 属性授权规则基类
  /// </summary>
  public abstract class PropertyAuthorizationRule : Csla.Rules.AuthorizationRule, IAuthorizationRule
  {
    internal PropertyAuthorizationRule(Csla.Rules.AuthorizationActions action, Phenix.Core.Mapping.IPropertyInfo propertyInfo)
      : base(action, new Csla.MethodInfo(propertyInfo.Name))
    {
      _propertyInfo = propertyInfo;
    }

    #region 属性

    private readonly Phenix.Core.Mapping.IPropertyInfo _propertyInfo;
    /// <summary>
    /// 属性信息
    /// </summary>
    protected Phenix.Core.Mapping.IPropertyInfo PropertyInfo
    {
      get { return _propertyInfo; }
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
      get { return PropertyInfo; }
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
          case Csla.Rules.AuthorizationActions.ReadProperty:
            return MethodAction.ReadProperty;
          case Csla.Rules.AuthorizationActions.WriteProperty:
            return MethodAction.WriteProperty;
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