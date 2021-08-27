using System;
using System.Reflection;
using System.ServiceModel;
using System.ServiceModel.Activation;
using Phenix.Core.Log;
using Phenix.Core.Mapping;
using Phenix.Core.Rule;
using Phenix.Core.Security;
using Phenix.Services.Host.Core;

namespace Phenix.Services.Host.Service.Wcf
{
  [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerCall)]
  [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
  public sealed class DataRule : Phenix.Services.Contract.Wcf.IDataRule
  {
    #region PromptCode

    [OperationBehavior(Impersonation = ImpersonationOption.Allowed)]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
    public object GetPromptCodes(string name, UserIdentity identity)
    {
      try
      {
        ServiceManager.CheckIn(identity);
        DataSecurityContext context = DataSecurityHub.CheckIn(identity, false);
        return DataRuleHub.GetPromptCodes(name, context.Identity);
      }
      catch (Exception ex)
      {
        return ex;
      }
    }

    [OperationBehavior(Impersonation = ImpersonationOption.Allowed)]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
    public object GetPromptCodeChangedTime(string name)
    {
      try
      {
        ServiceManager.CheckActive();
        return DataRuleHub.GetPromptCodeChangedTime(name);
      }
      catch (Exception ex)
      {
        return ex;
      }
    }

    [OperationBehavior(Impersonation = ImpersonationOption.Allowed)]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
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

    [OperationBehavior(Impersonation = ImpersonationOption.Allowed)]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
    public object AddPromptCode(string name, string key, string caption, string value, ReadLevel readLevel, UserIdentity identity)
    {
      try
      {
        ServiceManager.CheckIn(identity);
        DataSecurityContext context = DataSecurityHub.CheckIn(identity, false);
        return DataRuleHub.AddPromptCode(name, key, caption, value, readLevel, context.Identity);
      }
      catch (Exception ex)
      {
        return ex;
      }
    }

    [OperationBehavior(Impersonation = ImpersonationOption.Allowed)]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
    public object RemovePromptCode(string name, string key)
    {
      try
      {
        ServiceManager.CheckActive();
        DataRuleHub.RemovePromptCode(name, key);
        return null;
      }
      catch (Exception ex)
      {
        return ex;
      }
    }

    #endregion

    #region CriteriaExpression

    [OperationBehavior(Impersonation = ImpersonationOption.Allowed)]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
    public object GetCriteriaExpressions(string name, UserIdentity identity)
    {
      try
      {
        ServiceManager.CheckIn(identity);
        DataSecurityContext context = DataSecurityHub.CheckIn(identity, false);
        return DataRuleHub.GetCriteriaExpressions(null, name, context.Identity);
      }
      catch (Exception ex)
      {
        return ex;
      }
    }

    [OperationBehavior(Impersonation = ImpersonationOption.Allowed)]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
    public object GetCriteriaExpressionChangedTime(string name)
    {
      try
      {
        ServiceManager.CheckActive();
        return DataRuleHub.GetCriteriaExpressionChangedTime(name);
      }
      catch (Exception ex)
      {
        return ex;
      }
    }

    [OperationBehavior(Impersonation = ImpersonationOption.Allowed)]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
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

    [OperationBehavior(Impersonation = ImpersonationOption.Allowed)]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
    public object AddCriteriaExpression(string name, string key, string caption, CriteriaExpression value, ReadLevel readLevel, UserIdentity identity)
    {
      try
      {
        ServiceManager.CheckIn(identity);
        DataSecurityContext context = DataSecurityHub.CheckIn(identity, false);
        return DataRuleHub.AddCriteriaExpression(name, key, caption, value, readLevel, context.Identity);
      }
      catch (Exception ex)
      {
        return ex;
      }
    }

    [OperationBehavior(Impersonation = ImpersonationOption.Allowed)]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
    public object RemoveCriteriaExpression(string name, string key)
    {
      try
      {
        ServiceManager.CheckActive();
        DataRuleHub.RemoveCriteriaExpression(name, key);
        return null;
      }
      catch (Exception ex)
      {
        return ex;
      }
    }

    #endregion
  }
}
