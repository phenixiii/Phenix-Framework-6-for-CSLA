using System;
using System.Collections.Generic;
using System.ServiceModel;
using Phenix.Core.Net;
using Phenix.Core.Security;
using Phenix.Core.Workflow;
using Phenix.Services.Contract;

namespace Phenix.Services.Client.Library.Wcf
{
  internal class WorkflowProxy : IWorkflow
  {
    #region 属性

    #region IWorkflow 成员

    public IDictionary<string, WorkflowInfo> WorkflowInfos
    {
      get
      {
        NetConfig.InitializeSwitch();
        do
        {
          try
          {
            ChannelFactory<Phenix.Services.Contract.Wcf.IWorkflow> channelFactory = GetChannelFactory();
            Phenix.Services.Contract.Wcf.IWorkflow channel = channelFactory.CreateChannel();
            object result = null;
            try
            {
              result = channel.GetWorkflowInfos();
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
            return (IDictionary<string, WorkflowInfo>)result;
          }
          catch (EndpointNotFoundException)
          {
            if (!NetConfig.SwitchServicesAddress())
              throw;
          }
        } while (true);
      }
    }

    public DateTime? WorkflowInfoChangedTime
    {
      get
      {
        NetConfig.InitializeSwitch();
        do
        {
          try
          {
            ChannelFactory<Phenix.Services.Contract.Wcf.IWorkflow> channelFactory = GetChannelFactory();
            Phenix.Services.Contract.Wcf.IWorkflow channel = channelFactory.CreateChannel();
            object result = null;
            try
            {
              result = channel.GetWorkflowInfoChangedTime();
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
    }
    
    #endregion

    #endregion

    #region 方法

    private static ChannelFactory<Phenix.Services.Contract.Wcf.IWorkflow> GetChannelFactory()
    {
      return new ChannelFactory<Phenix.Services.Contract.Wcf.IWorkflow>(WcfHelper.CreateBinding(),
        new EndpointAddress(WcfHelper.CreateUrl(NetConfig.ServicesAddress, ServicesInfo.WORKFLOW_URI)));
    }

    #region IWorkflow 成员

    #region WorkflowInfo

    public void WorkflowInfoHasChanged()
    {
      NetConfig.InitializeSwitch();
      do
      {
        try
        {
          ChannelFactory<Phenix.Services.Contract.Wcf.IWorkflow> channelFactory = GetChannelFactory();
          Phenix.Services.Contract.Wcf.IWorkflow channel = channelFactory.CreateChannel();
          try
          {
            channel.WorkflowInfoHasChanged();
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

    public void AddWorkflowInfo(string typeNamespace, string typeName, string caption, string xamlCode, UserIdentity identity)
    {
      NetConfig.InitializeSwitch();
      do
      {
        try
        {
          ChannelFactory<Phenix.Services.Contract.Wcf.IWorkflow> channelFactory = GetChannelFactory();
          Phenix.Services.Contract.Wcf.IWorkflow channel = channelFactory.CreateChannel();
          object result = null;
          try
          {
            result = channel.AddWorkflowInfo(typeNamespace, typeName, caption, xamlCode, identity);
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

    public void DisableWorkflowInfo(string typeNamespace, string typeName, UserIdentity identity)
    {
      NetConfig.InitializeSwitch();
      do
      {
        try
        {
          ChannelFactory<Phenix.Services.Contract.Wcf.IWorkflow> channelFactory = GetChannelFactory();
          Phenix.Services.Contract.Wcf.IWorkflow channel = channelFactory.CreateChannel();
          object result = null;
          try
          {
            result = channel.DisableWorkflowInfo(typeNamespace, typeName, identity);
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

    #region WorkflowInstance

    public void SaveWorkflowInstance(Guid id, string typeNamespace, string typeName, string content, TaskContext taskContext)
    {
      NetConfig.InitializeSwitch();
      do
      {
        try
        {
          ChannelFactory<Phenix.Services.Contract.Wcf.IWorkflow> channelFactory = GetChannelFactory();
          Phenix.Services.Contract.Wcf.IWorkflow channel = channelFactory.CreateChannel();
          object result = null;
          try
          {
            result = channel.SaveWorkflowInstance(id, typeNamespace, typeName, content, taskContext);
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

    public string FetchWorkflowInstance(Guid id)
    {
      NetConfig.InitializeSwitch();
      do
      {
        try
        {
          ChannelFactory<Phenix.Services.Contract.Wcf.IWorkflow> channelFactory = GetChannelFactory();
          Phenix.Services.Contract.Wcf.IWorkflow channel = channelFactory.CreateChannel();
          object result = null;
          try
          {
            result = channel.FetchWorkflowInstance(id);
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
          return (string)result;
        }
        catch (EndpointNotFoundException)
        {
          if (!NetConfig.SwitchServicesAddress())
            throw;
        }
      } while (true);
    }

    public void ClearWorkflowInstance(Guid id)
    {
      NetConfig.InitializeSwitch();
      do
      {
        try
        {
          ChannelFactory<Phenix.Services.Contract.Wcf.IWorkflow> channelFactory = GetChannelFactory();
          Phenix.Services.Contract.Wcf.IWorkflow channel = channelFactory.CreateChannel();
          object result = null;
          try
          {
            result = channel.ClearWorkflowInstance(id);
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

    #region WorkflowTask

    public void DispatchWorkflowTask(Guid id, string bookmarkName,
      string pluginAssemblyName, string workerRole, string caption, string message, bool urgent)
    {
      NetConfig.InitializeSwitch();
      do
      {
        try
        {
          ChannelFactory<Phenix.Services.Contract.Wcf.IWorkflow> channelFactory = GetChannelFactory();
          Phenix.Services.Contract.Wcf.IWorkflow channel = channelFactory.CreateChannel();
          object result = null;
          try
          {
            result = channel.DispatchWorkflowTask(id, bookmarkName, pluginAssemblyName, workerRole, caption, message, urgent);
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

    public void ReceiveWorkflowTask(Guid id, string bookmarkName)
    {
      NetConfig.InitializeSwitch();
      do
      {
        try
        {
          ChannelFactory<Phenix.Services.Contract.Wcf.IWorkflow> channelFactory = GetChannelFactory();
          Phenix.Services.Contract.Wcf.IWorkflow channel = channelFactory.CreateChannel();
          object result = null;
          try
          {
            result = channel.ReceiveWorkflowTask(id, bookmarkName);
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

    public void HoldWorkflowTask(Guid id, string bookmarkName)
    {
      NetConfig.InitializeSwitch();
      do
      {
        try
        {
          ChannelFactory<Phenix.Services.Contract.Wcf.IWorkflow> channelFactory = GetChannelFactory();
          Phenix.Services.Contract.Wcf.IWorkflow channel = channelFactory.CreateChannel();
          object result = null;
          try
          {
            result = channel.HoldWorkflowTask(id, bookmarkName);
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

    public void AbortWorkflowTask(Guid id, string bookmarkName)
    {
      NetConfig.InitializeSwitch();
      do
      {
        try
        {
          ChannelFactory<Phenix.Services.Contract.Wcf.IWorkflow> channelFactory = GetChannelFactory();
          Phenix.Services.Contract.Wcf.IWorkflow channel = channelFactory.CreateChannel();
          object result = null;
          try
          {
            result = channel.AbortWorkflowTask(id, bookmarkName);
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

    public void ProceedWorkflow(WorkflowTaskInfo workflowTaskInfo)
    {
      NetConfig.InitializeSwitch();
      do
      {
        try
        {
          ChannelFactory<Phenix.Services.Contract.Wcf.IWorkflow> channelFactory = GetChannelFactory();
          Phenix.Services.Contract.Wcf.IWorkflow channel = channelFactory.CreateChannel();
          object result = null;
          try
          {
            result = channel.ProceedWorkflow(workflowTaskInfo);
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

    public void CompleteWorkflowTask(Guid id, string bookmarkName)
    {
      NetConfig.InitializeSwitch();
      do
      {
        try
        {
          ChannelFactory<Phenix.Services.Contract.Wcf.IWorkflow> channelFactory = GetChannelFactory();
          Phenix.Services.Contract.Wcf.IWorkflow channel = channelFactory.CreateChannel();
          object result = null;
          try
          {
            result = channel.CompleteWorkflowTask(id, bookmarkName);
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

    public IList<WorkflowTaskInfo> FetchWorkflowTask(TaskState taskState, DateTime startDispatchTime, DateTime finishDispatchTime, UserIdentity identity)
    {
      NetConfig.InitializeSwitch();
      do
      {
        try
        {
          ChannelFactory<Phenix.Services.Contract.Wcf.IWorkflow> channelFactory = GetChannelFactory();
          Phenix.Services.Contract.Wcf.IWorkflow channel = channelFactory.CreateChannel();
          object result = null;
          try
          {
            result = channel.FetchWorkflowTask(taskState, startDispatchTime, finishDispatchTime, identity);
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
          return (IList<WorkflowTaskInfo>)result;
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