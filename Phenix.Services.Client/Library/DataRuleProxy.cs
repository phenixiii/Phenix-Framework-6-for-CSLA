using System;
using System.Net.Sockets;
using Phenix.Core.Mapping;
using Phenix.Core.Net;
using Phenix.Core.Rule;
using Phenix.Core.Security;
using Phenix.Services.Contract;

namespace Phenix.Services.Client.Library
{
  internal class DataRuleProxy : IDataRule
  {
    #region 属性

    private IDataRule _service;
    private IDataRule Service
    {
      get
      {
        if (_service == null)
        {
          RemotingHelper.RegisterClientChannel();
          _service = (IDataRule)RemotingHelper.CreateRemoteObjectProxy(typeof(IDataRule), ServicesInfo.DATA_RULE_URI);
        }
        return _service;
      }
    }

    #endregion

    #region 方法

    private void InvalidateCache()
    {
      _service = null;
    }

    #region IDataRule 成员

    #region PromptCode

    public PromptCodeKeyCaptionCollection GetPromptCodes(string name, UserIdentity identity)
    {
      NetConfig.InitializeSwitch();
      do
      {
        try
        {
          return Service.GetPromptCodes(name, identity);
        }
        catch (SocketException)
        {
          InvalidateCache();
          if (!NetConfig.SwitchServicesAddress())
            throw;
        }
      } while (true);
    }

    public DateTime? GetPromptCodeChangedTime(string name)
    {
      NetConfig.InitializeSwitch();
      do
      {
        try
        {
          return Service.GetPromptCodeChangedTime(name);
        }
        catch (SocketException)
        {
          InvalidateCache();
          if (!NetConfig.SwitchServicesAddress())
            throw;
        }
      } while (true);
    }

    public void PromptCodeHasChanged(string name)
    {
      NetConfig.InitializeSwitch();
      do
      {
        try
        {
          Service.PromptCodeHasChanged(name);
          break;
        }
        catch (SocketException)
        {
          InvalidateCache();
          if (!NetConfig.SwitchServicesAddress())
            throw;
        }
      } while (true);
    }

    public bool AddPromptCode(string name, string key, string caption, string value, ReadLevel readLevel, UserIdentity identity)
    {
      NetConfig.InitializeSwitch();
      do
      {
        try
        {
          return Service.AddPromptCode(name, key, caption, value, readLevel, identity);
        }
        catch (SocketException)
        {
          InvalidateCache();
          if (!NetConfig.SwitchServicesAddress())
            throw;
        }
      } while (true);
    }

    public void RemovePromptCode(string name, string key)
    {
      NetConfig.InitializeSwitch();
      do
      {
        try
        {
          Service.RemovePromptCode(name, key);
          break;
        }
        catch (SocketException)
        {
          InvalidateCache();
          if (!NetConfig.SwitchServicesAddress())
            throw;
        }
      } while (true);
    }

    #endregion

    #region CriteriaExpression

    public CriteriaExpressionKeyCaptionCollection GetCriteriaExpressions(string name, UserIdentity identity)
    {
      NetConfig.InitializeSwitch();
      do
      {
        try
        {
          return Service.GetCriteriaExpressions(name, identity);
        }
        catch (SocketException)
        {
          InvalidateCache();
          if (!NetConfig.SwitchServicesAddress())
            throw;
        }
      } while (true);
    }

    public DateTime? GetCriteriaExpressionChangedTime(string name)
    {
      NetConfig.InitializeSwitch();
      do
      {
        try
        {
          return Service.GetCriteriaExpressionChangedTime(name);
        }
        catch (SocketException)
        {
          InvalidateCache();
          if (!NetConfig.SwitchServicesAddress())
            throw;
        }
      } while (true);
    }

    public void CriteriaExpressionHasChanged(string name)
    {
      NetConfig.InitializeSwitch();
      do
      {
        try
        {
          Service.CriteriaExpressionHasChanged(name);
          break;
        }
        catch (SocketException)
        {
          InvalidateCache();
          if (!NetConfig.SwitchServicesAddress())
            throw;
        }
      } while (true);
    }

    public bool AddCriteriaExpression(string name, string key, string caption, CriteriaExpression value, ReadLevel readLevel, UserIdentity identity)
    {
      NetConfig.InitializeSwitch();
      do
      {
        try
        {
          return Service.AddCriteriaExpression(name, key, caption, value, readLevel, identity);
        }
        catch (SocketException)
        {
          InvalidateCache();
          if (!NetConfig.SwitchServicesAddress())
            throw;
        }
      } while (true);
    }

    public void RemoveCriteriaExpression(string name, string key)
    {
      NetConfig.InitializeSwitch();
      do
      {
        try
        {
          Service.RemoveCriteriaExpression(name, key);
          break;
        }
        catch (SocketException)
        {
          InvalidateCache();
          if (!NetConfig.SwitchServicesAddress())
            throw;
        }
      } while (true);
    }

    #endregion

    #endregion

    #endregion
  }
}