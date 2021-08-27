using System;
using System.Runtime.Serialization;
using Csla;
using Phenix.Core.Mapping;
using Phenix.Core.Security;
using Phenix.Core.SyncCollections;

namespace Phenix.Business.Rules
{
  /// <summary>
  /// 授权规则集合
  /// </summary>
  public sealed class AuthorizationRules
  {
    internal AuthorizationRules(Type targetType)
    {
      _targetType = targetType;
    }

    #region 属性

    private readonly Type _targetType;
    /// <summary>
    /// 目标类
    /// </summary>
    public Type TargetType
    {
      get { return _targetType; }
    }

    private readonly object _rulesCacheLock = new object();
    private bool _rulesCacheChecked;
    private SynchronizedDictionary<MethodAction, SynchronizedDictionary<string, IAuthorizationRule>> _rulesCache;
    private SynchronizedDictionary<MethodAction, SynchronizedDictionary<string, IAuthorizationRule>> RulesCache
    {
      get
      {
        if (!_rulesCacheChecked)
          lock (_rulesCacheLock)
            if (!_rulesCacheChecked)
            {
              _rulesCache = new SynchronizedDictionary<MethodAction, SynchronizedDictionary<string, IAuthorizationRule>>(3);
              _rulesCacheChecked = true;
            }
        return _rulesCache;
      }
    }


    #endregion

    #region 方法

    /// <summary>
    /// 注册授权规则
    /// </summary>
    /// <param name="rule">授权规则对象</param>
    public void AddRule(IAuthorizationRule rule)
    {
      RulesCache.GetValue(rule.Action, () => new SynchronizedDictionary<string, IAuthorizationRule>(), true).Add(rule.ElementName, rule);
    }

    /// <summary>
    /// 是否许可
    /// </summary>
    public bool HasPermission(IAuthorizationObject obj, MethodAction action, string methodName, bool? isNew, bool throwIfDeny, params object[] arguments)
    {
      if (action == MethodAction.WriteProperty)
      {
        ClassMapInfo classMapInfo = ClassMemberHelper.GetClassMapInfo(TargetType);
        if (classMapInfo.IsReadOnly)
        {
          if (throwIfDeny)
            throw new System.Security.SecurityException(String.Format("{0}({1})", action, classMapInfo.FriendlyName));
          return false;
        }
        if (ApplicationContext.LogicalExecutionLocation == ApplicationContext.LogicalExecutionLocations.Server)
          return true;
        if (UserIdentity.IsByDeny(UserIdentity.CurrentIdentity, TargetType, ExecuteAction.Update, throwIfDeny) ||
          !Csla.Rules.BusinessRules.HasPermission(Csla.Rules.AuthorizationActions.EditObject, TargetType))
        {
          if (throwIfDeny)
            throw new System.Security.SecurityException(String.Format("{0}({1})", action, classMapInfo.FriendlyName));
          return false;
        }
      }
      else if (ApplicationContext.LogicalExecutionLocation == ApplicationContext.LogicalExecutionLocations.Server)
        return true;
      if (!_rulesCacheChecked)
      {
        if (obj == null)
          obj = FormatterServices.GetUninitializedObject(_targetType) as IAuthorizationObject;
        if (obj != null)
          obj.AddAuthorizationRules();
      }
      if (_rulesCache != null)
      {
        SynchronizedDictionary<string, IAuthorizationRule> rules;
        if (_rulesCache.TryGetValue(action, out rules))
        {
          IAuthorizationRule rule;
          if (rules.TryGetValue(methodName, out rule))
          {
            AuthorizationContext context = new AuthorizationContext(rule, obj, arguments);
            rule.Execute(context);
            if (!context.HasPermission)
            {
              if (throwIfDeny)
                throw new System.Security.SecurityException(context.DenyMessage ?? String.Format("{0}({1})", rule.Action, rule.Element.FriendlyName));
              return false;
            }
          }
        }
      }
      return !UserIdentity.IsByDeny(UserIdentity.CurrentIdentity, obj, action, methodName, isNew, throwIfDeny);
    }

    #endregion
  }
}
