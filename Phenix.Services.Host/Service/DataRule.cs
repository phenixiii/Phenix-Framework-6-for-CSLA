using System;
using System.Reflection;
using Phenix.Core.Log;
using Phenix.Core.Mapping;
using Phenix.Core.Rule;
using Phenix.Core.Security;
using Phenix.Services.Host.Core;

namespace Phenix.Services.Host.Service
{
  public sealed class DataRule : MarshalByRefObject, IDataRule
  {
    #region 方法

    #region PromptCode

    public PromptCodeKeyCaptionCollection GetPromptCodes(string name, UserIdentity identity)
    {
      ServiceManager.CheckIn(identity);
      DataSecurityContext context = DataSecurityHub.CheckIn(identity, false);
      return DataRuleHub.GetPromptCodes(name, context.Identity);
    }

    public DateTime? GetPromptCodeChangedTime(string name)
    {
      ServiceManager.CheckActive();
      return DataRuleHub.GetPromptCodeChangedTime(name);
    }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
    [System.Runtime.Remoting.Messaging.OneWay]
    public void PromptCodeHasChanged(string name)
    {
      try
      {
        DataRuleHub.PromptCodeHasChanged(name);
      }
      catch (Exception ex)
      {
        EventLog.SaveLocal(MethodBase.GetCurrentMethod(), name, ex);
      }
    }

    public bool AddPromptCode(string name, string key, string caption, string value, ReadLevel readLevel, UserIdentity identity)
    {
      ServiceManager.CheckIn(identity);
      DataSecurityContext context = DataSecurityHub.CheckIn(identity, false);
      return DataRuleHub.AddPromptCode(name, key, caption, value, readLevel, context.Identity);
    }

    public void RemovePromptCode(string name, string key)
    {
      ServiceManager.CheckActive();
      DataRuleHub.RemovePromptCode(name, key);
    }

    #endregion

    #region CriteriaExpression

    public CriteriaExpressionKeyCaptionCollection GetCriteriaExpressions(string name, UserIdentity identity)
    {
      ServiceManager.CheckIn(identity);
      DataSecurityContext context = DataSecurityHub.CheckIn(identity, false);
      return DataRuleHub.GetCriteriaExpressions(null, name, context.Identity);
    }

    public DateTime? GetCriteriaExpressionChangedTime(string name)
    {
      ServiceManager.CheckActive();
      return DataRuleHub.GetCriteriaExpressionChangedTime(name);
    }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
    [System.Runtime.Remoting.Messaging.OneWay]
    public void CriteriaExpressionHasChanged(string name)
    {
      try
      {
        DataRuleHub.CriteriaExpressionHasChanged(name);
      }
      catch (Exception ex)
      {
        EventLog.SaveLocal(MethodBase.GetCurrentMethod(), name, ex);
      }
    }

    public bool AddCriteriaExpression(string name, string key, string caption, CriteriaExpression value, ReadLevel readLevel, UserIdentity identity)
    {
      ServiceManager.CheckIn(identity);
      DataSecurityContext context = DataSecurityHub.CheckIn(identity, false);
      return DataRuleHub.AddCriteriaExpression(name, key, caption, value, readLevel, context.Identity);
    }

    public void RemoveCriteriaExpression(string name, string key)
    {
      ServiceManager.CheckActive();
      DataRuleHub.RemoveCriteriaExpression(name, key);
    }

    #endregion

    #endregion
  }
}