using System;
using System.ServiceModel;
using Phenix.Core.Mapping;
using Phenix.Core.Net;
using Phenix.Core.Rule;
using Phenix.Core.Security;
using Phenix.Services.Contract;

namespace Phenix.Services.Client.Library.Wcf
{
  internal class DataRuleProxy : IDataRule
  {
    #region 方法

    private static ChannelFactory<Phenix.Services.Contract.Wcf.IDataRule> GetChannelFactory()
    {
      return new ChannelFactory<Phenix.Services.Contract.Wcf.IDataRule>(WcfHelper.CreateBinding(),
        new EndpointAddress(WcfHelper.CreateUrl(NetConfig.ServicesAddress, ServicesInfo.DATA_RULE_URI)));
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
          ChannelFactory<Phenix.Services.Contract.Wcf.IDataRule> channelFactory = GetChannelFactory();
          Phenix.Services.Contract.Wcf.IDataRule channel = channelFactory.CreateChannel();
          object result = null;
          try
          {
            result = channel.GetPromptCodes(name, identity);
            channelFactory.Close();
          }
          catch
          {
            channelFactory.Abort();
            throw;
          }
          Exception exception = result as Exception;
          if (exception != null)
            throw exception;
          return (PromptCodeKeyCaptionCollection)result;
        }
        catch (EndpointNotFoundException)
        {
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
          ChannelFactory<Phenix.Services.Contract.Wcf.IDataRule> channelFactory = GetChannelFactory();
          Phenix.Services.Contract.Wcf.IDataRule channel = channelFactory.CreateChannel();
          object result = null;
          try
          {
            result = channel.GetPromptCodeChangedTime(name);
            channelFactory.Close();
          }
          catch
          {
            channelFactory.Abort();
            throw;
          }
          Exception exception = result as Exception;
          if (exception != null)
            throw exception;
          return (DateTime?)result;
        }
        catch (EndpointNotFoundException)
        {
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
          ChannelFactory<Phenix.Services.Contract.Wcf.IDataRule> channelFactory = GetChannelFactory();
          Phenix.Services.Contract.Wcf.IDataRule channel = channelFactory.CreateChannel();
          try
          {
            channel.PromptCodeHasChanged(name);
            channelFactory.Close();
          }
          catch
          {
            channelFactory.Abort();
            throw;
          }
          break;
        }
        catch (EndpointNotFoundException)
        {
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
          ChannelFactory<Phenix.Services.Contract.Wcf.IDataRule> channelFactory = GetChannelFactory();
          Phenix.Services.Contract.Wcf.IDataRule channel = channelFactory.CreateChannel();
          object result = null;
          try
          {
            result = channel.AddPromptCode(name, key, caption, value, readLevel, identity);
            channelFactory.Close();
          }
          catch
          {
            channelFactory.Abort();
            throw;
          }
          Exception exception = result as Exception;
          if (exception != null)
            throw exception;
          return (bool)result;
        }
        catch (EndpointNotFoundException)
        {
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
          ChannelFactory<Phenix.Services.Contract.Wcf.IDataRule> channelFactory = GetChannelFactory();
          Phenix.Services.Contract.Wcf.IDataRule channel = channelFactory.CreateChannel();
          object result = null;
          try
          {
            result = channel.RemovePromptCode(name, key);
            channelFactory.Close();
          }
          catch
          {
            channelFactory.Abort();
            throw;
          }
          Exception exception = result as Exception;
          if (exception != null)
            throw exception;
          break;
        }
        catch (EndpointNotFoundException)
        {
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
          ChannelFactory<Phenix.Services.Contract.Wcf.IDataRule> channelFactory = GetChannelFactory();
          Phenix.Services.Contract.Wcf.IDataRule channel = channelFactory.CreateChannel();
          object result = null;
          try
          {
            result = channel.GetCriteriaExpressions(name, identity);
            channelFactory.Close();
          }
          catch
          {
            channelFactory.Abort();
            throw;
          }
          Exception exception = result as Exception;
          if (exception != null)
            throw exception;
          return (CriteriaExpressionKeyCaptionCollection)result;
        }
        catch (EndpointNotFoundException)
        {
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
          ChannelFactory<Phenix.Services.Contract.Wcf.IDataRule> channelFactory = GetChannelFactory();
          Phenix.Services.Contract.Wcf.IDataRule channel = channelFactory.CreateChannel();
          object result = null;
          try
          {
            result = channel.GetCriteriaExpressionChangedTime(name);
            channelFactory.Close();
          }
          catch
          {
            channelFactory.Abort();
            throw;
          }
          Exception exception = result as Exception;
          if (exception != null)
            throw exception;
          return (DateTime?)result;
        }
        catch (EndpointNotFoundException)
        {
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
          ChannelFactory<Phenix.Services.Contract.Wcf.IDataRule> channelFactory = GetChannelFactory();
          Phenix.Services.Contract.Wcf.IDataRule channel = channelFactory.CreateChannel();
          try
          {
            channel.CriteriaExpressionHasChanged(name);
            channelFactory.Close();
          }
          catch
          {
            channelFactory.Abort();
            throw;
          }
          break;
        }
        catch (EndpointNotFoundException)
        {
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
          ChannelFactory<Phenix.Services.Contract.Wcf.IDataRule> channelFactory = GetChannelFactory();
          Phenix.Services.Contract.Wcf.IDataRule channel = channelFactory.CreateChannel();
          object result = null;
          try
          {
            result = channel.AddCriteriaExpression(name, key, caption, value, readLevel, identity);
            channelFactory.Close();
          }
          catch
          {
            channelFactory.Abort();
            throw;
          }
          Exception exception = result as Exception;
          if (exception != null)
            throw exception;
          return (bool)result;
        }
        catch (EndpointNotFoundException)
        {
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
          ChannelFactory<Phenix.Services.Contract.Wcf.IDataRule> channelFactory = GetChannelFactory();
          Phenix.Services.Contract.Wcf.IDataRule channel = channelFactory.CreateChannel();
          object result = null;
          try
          {
            result = channel.RemoveCriteriaExpression(name, key);
            channelFactory.Close();
          }
          catch
          {
            channelFactory.Abort();
            throw;
          }
          Exception exception = result as Exception;
          if (exception != null)
            throw exception;
          break;
        }
        catch (EndpointNotFoundException)
        {
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