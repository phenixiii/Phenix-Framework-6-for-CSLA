using System;
using System.Collections.Generic;
using System.Net.Sockets;
using Phenix.Core.Net;
using Phenix.Core.Security;
using Phenix.Core.Workflow;
using Phenix.Services.Contract;

namespace Phenix.Services.Client.Library
{
  internal class WorkflowProxy : IWorkflow
  {
    #region 属性

    private IWorkflow _service;
    private IWorkflow Service
    {
      get
      {
        if (_service == null)
        {
          RemotingHelper.RegisterClientChannel();
          _service = (IWorkflow)RemotingHelper.CreateRemoteObjectProxy(typeof(IWorkflow), ServicesInfo.WORKFLOW_URI);
        }
        return _service;
      }
    }

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
            return Service.WorkflowInfos;
          }
          catch (SocketException)
          {
            InvalidateCache();
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
            return Service.WorkflowInfoChangedTime;
          }
          catch (SocketException)
          {
            InvalidateCache();
            if (!NetConfig.SwitchServicesAddress())
              throw;
          }
        } while (true);
      }
    }

    #endregion

    #endregion

    #region 方法

    private void InvalidateCache()
    {
      _service = null;
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
          Service.WorkflowInfoHasChanged();
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

    public void AddWorkflowInfo(string typeNamespace, string typeName, string caption, string xamlCode, UserIdentity identity)
    {
      NetConfig.InitializeSwitch();
      do
      {
        try
        {
          Service.AddWorkflowInfo(typeNamespace, typeName, caption, xamlCode, identity);
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

    public void DisableWorkflowInfo(string typeNamespace, string typeName, UserIdentity identity)
    {
      NetConfig.InitializeSwitch();
      do
      {
        try
        {
          Service.DisableWorkflowInfo(typeNamespace, typeName, identity);
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

    #region WorkflowInstance

    public void SaveWorkflowInstance(Guid id, string typeNamespace, string typeName, string content, TaskContext taskContext)
    {
      NetConfig.InitializeSwitch();
      do
      {
        try
        {
          Service.SaveWorkflowInstance(id, typeNamespace, typeName, content, taskContext);
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

    public string FetchWorkflowInstance(Guid id)
    {
      NetConfig.InitializeSwitch();
      do
      {
        try
        {
          return Service.FetchWorkflowInstance(id);
         }
        catch (SocketException)
        {
          InvalidateCache();
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
          Service.ClearWorkflowInstance(id);
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

    #region WorkflowTask

    public void DispatchWorkflowTask(Guid id, string bookmarkName,
      string pluginAssemblyName, string workerRole, string caption, string message, bool urgent)
    {
      NetConfig.InitializeSwitch();
      do
      {
        try
        {
          Service.DispatchWorkflowTask(id, bookmarkName, pluginAssemblyName, workerRole, caption, message, urgent);
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

    public void ReceiveWorkflowTask(Guid id, string bookmarkName)
    {
      NetConfig.InitializeSwitch();
      do
      {
        try
        {
          Service.ReceiveWorkflowTask(id, bookmarkName);
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

    public void HoldWorkflowTask(Guid id, string bookmarkName)
    {
      NetConfig.InitializeSwitch();
      do
      {
        try
        {
          Service.HoldWorkflowTask(id, bookmarkName);
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

    public void AbortWorkflowTask(Guid id, string bookmarkName)
    {
      NetConfig.InitializeSwitch();
      do
      {
        try
        {
          Service.AbortWorkflowTask(id, bookmarkName);
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

    public void ProceedWorkflow(WorkflowTaskInfo workflowTaskInfo)
    {
      NetConfig.InitializeSwitch();
      do
      {
        try
        {
          Service.ProceedWorkflow(workflowTaskInfo);
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

    public void CompleteWorkflowTask(Guid id, string bookmarkName)
    {
      NetConfig.InitializeSwitch();
      do
      {
        try
        {
          Service.CompleteWorkflowTask(id, bookmarkName);
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

    public IList<WorkflowTaskInfo> FetchWorkflowTask(TaskState taskState, DateTime startDispatchTime, DateTime finishDispatchTime, UserIdentity identity)
    {
      NetConfig.InitializeSwitch();
      do
      {
        try
        {
          return Service.FetchWorkflowTask(taskState, startDispatchTime, finishDispatchTime, identity);
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