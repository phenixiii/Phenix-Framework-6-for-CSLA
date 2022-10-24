using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Threading;
using Phenix.Core.Data;
using Phenix.Core.Log;
using Phenix.Core.Net;
using Phenix.Core.Reflection;
using Phenix.Core.Security;
using Phenix.Core.Security.Cryptography;
using Phenix.Core.SyncCollections;

namespace Phenix.Core.Workflow
{
  /// <summary>
  /// 工作流中心
  /// </summary>
  public static class WorkflowHub
  {
    #region 属性

    private static IWorkflow _worker;
    /// <summary>
    /// 实施者
    /// </summary>
    public static IWorkflow Worker
    {
      get
      {
        if (_worker == null)
          AppUtilities.RegisterWorker();
        return _worker;
      }
      set { _worker = value; }
    }

    private static string _cacheKey;
    private static string CacheKey
    {
      get
      {
        if (_cacheKey == null)
          _cacheKey = MD5CryptoTextProvider.ComputeHash(DefaultDatabase.DbConnectionInfo.Key ?? NetConfig.ServicesAddress ?? String.Empty);
        return _cacheKey;
      }
    }

    private static DateTime _workflowInfoActionTime = DateTime.MinValue;
    private static string _workflowInfosPath;
    private static string WorkflowInfosPath
    {
      get
      {
        if (_workflowInfosPath == null)
          _workflowInfosPath = Path.Combine(AppConfig.CacheDirectory, CacheKey, MethodBase.GetCurrentMethod().Name.Substring(4));
        return _workflowInfosPath;
      }
    }
    private static readonly object _workflowInfosLock = new object();
    private static Thread _workflowInfosRefreshThread;
    private static IDictionary<string, WorkflowInfo> _workflowInfos;
    /// <summary>
    /// 工作流资料队列
    /// </summary>
    public static IDictionary<string, WorkflowInfo> WorkflowInfos
    {
      get
      {
        if (_workflowInfos != null)
          return _workflowInfos;
        //if (AppConfig.AppType == AppType.Webform)
        //{
        //  _workflowInfos = HttpRuntime.Cache[WorkflowInfosPath] as IDictionary<string, WorkflowInfo>;
        //  if (_workflowInfos == null)
        //  {
        //    _workflowInfos = RefreshWorkflowInfos();
        //    HttpRuntime.Cache.Insert(WorkflowInfosPath, _workflowInfos, null,
        //      System.Web.Caching.Cache.NoAbsoluteExpiration,
        //      System.Web.Caching.Cache.NoSlidingExpiration,
        //      CacheItemPriority.NotRemovable, null);
        //  }
        //  return _workflowInfos;
        //}
        RefreshWorkflowInfos(false);
        return _workflowInfos;
      }
    }

    /// <summary>
    /// 工作流资料更新时间
    /// </summary>
    [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
    public static DateTime? WorkflowInfoChangedTime
    {
      get
      {
        if (Worker == null)
          return null;
        return Worker.WorkflowInfoChangedTime;
      }
    }
    private static DateTime? _workflowInfoChangedLocalTime;
    private static DateTime WorkflowInfoChangedLocalTime
    {
      get { return AppSettings.GetProperty(CacheKey, ref _workflowInfoChangedLocalTime, DateTime.MinValue); }
      set { AppSettings.SetProperty(CacheKey, ref _workflowInfoChangedLocalTime, value); }
    }

    private static readonly SynchronizedDictionary<string, WorkflowIdentityInfo> _workflowIdentityInfoCache =
      new SynchronizedDictionary<string, WorkflowIdentityInfo>(StringComparer.Ordinal);

    #endregion
    
    #region 方法

    /// <summary>
    /// 清理缓存
    /// </summary>
    public static void RefreshCache(bool reset)
    {
      RefreshWorkflowInfos(reset);
    }

    /// <summary>
    /// 检查活动
    /// </summary>
    public static void CheckActive()
    {
      if (Worker == null)
      {
        Exception ex = new InvalidOperationException(Phenix.Core.Properties.Resources.NoService);
        EventLog.SaveLocal(MethodBase.GetCurrentMethod(), ex);
        throw ex;
      }
    }

    /// <summary>
    /// 所有资料已更新
    /// </summary>
    public static void AllInfoHasChanged()
    {
      CheckActive();
      Worker.WorkflowInfoHasChanged();
      RefreshCache(true);
    }

    #region WorkflowInfo

    /// <summary>
    /// 工作流资料已更新
    /// </summary>
    public static void WorkflowInfoHasChanged()
    {
      try
      {
        CheckActive();
        Worker.WorkflowInfoHasChanged();
      }
      finally
      {
        RefreshWorkflowInfos(true);
      }
    }

    /// <summary>
    /// 刷新工作流资料队列
    /// </summary>
    public static void RefreshWorkflowInfos(bool reset)
    {
      if (_workflowInfos == null)
        lock (_workflowInfosLock)
          if (_workflowInfos == null)
          {
            ExecuteRefreshWorkflowInfos(reset);
          }
      if (_workflowInfosRefreshThread == null && (_workflowInfoActionTime == DateTime.MinValue || reset))
        lock (_workflowInfosLock)
          if (_workflowInfosRefreshThread == null && (_workflowInfoActionTime == DateTime.MinValue || reset))
          {
            _workflowInfosRefreshThread = new Thread(ExecuteRefreshWorkflowInfos);
            _workflowInfosRefreshThread.IsBackground = true;
            _workflowInfosRefreshThread.Start(reset);
          }
    }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
    private static void ExecuteRefreshWorkflowInfos(object data)
    {
      bool reset = (bool)data;
      try
      {
        if (reset)
          _workflowInfoActionTime = DateTime.MinValue;
        IDictionary<string, WorkflowInfo> workflowInfos = null;
        if (AppConfig.AppType == AppType.Webform && File.Exists(WorkflowInfosPath))
        {
          workflowInfos = Utilities.Restore(WorkflowInfosPath) as IDictionary<string, WorkflowInfo>;
          if (workflowInfos != null)
          {
            _workflowInfos = workflowInfos;
            _workflowInfoActionTime = DateTime.Now;
            return;
          }
        }
        DateTime? actionTime = WorkflowInfoChangedTime;
        if (!actionTime.HasValue)
        {
          CheckActive();
          _workflowInfos = Worker.WorkflowInfos;
          _workflowInfoActionTime = DateTime.Now;
          return;
        }
        if (_workflowInfos != null && _workflowInfoActionTime >= actionTime.Value)
          return;
        if (WorkflowInfoChangedLocalTime >= actionTime.Value && File.Exists(WorkflowInfosPath))
        {
          FileInfo fileInfo = new FileInfo(WorkflowInfosPath);
          if (fileInfo.LastWriteTime > actionTime.Value)
          {
            actionTime = fileInfo.LastWriteTime;
            workflowInfos = Utilities.Restore(WorkflowInfosPath) as IDictionary<string, WorkflowInfo>;
          }
        }
        if (workflowInfos == null)
        {
          CheckActive();
          workflowInfos = Worker.WorkflowInfos;
          Utilities.Save(workflowInfos, WorkflowInfosPath);
          WorkflowInfoChangedLocalTime = actionTime.Value;
        }
        if (workflowInfos != null)
        {
          _workflowInfos = workflowInfos;
          _workflowInfoActionTime = actionTime.Value;
        }
      }
      catch (ObjectDisposedException)
      {
        return;
      }
      catch (ThreadAbortException)
      {
        Thread.ResetAbort();
        return;
      }
      catch (Exception ex)
      {
        EventLog.SaveLocal(MethodBase.GetCurrentMethod(), ex);
        return;
      }
      finally
      {
        _workflowInfosRefreshThread = null;
      }
    }

    /// <summary>
    /// 取工作流资料
    /// </summary>
    [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
    public static WorkflowInfo GetWorkflowInfo(string typeNamespace, string typeName, bool throwIfNotFound)
    {
      WorkflowInfo result;
      if (WorkflowInfos.TryGetValue(Utilities.AssembleFullTypeName(typeNamespace, typeName), out result))
        return result;
      if (throwIfNotFound)
        throw new InvalidOperationException(String.Format(Phenix.Core.Properties.Resources.WorkflowInfoDoesNotExist, typeNamespace, typeName));
      return null;
    }

    /// <summary>
    /// 取工作流资料
    /// </summary>
    public static WorkflowInfo GetWorkflowInfo(IStartCommand command, bool throwIfNotFound)
    {
      WorkflowIdentityInfo workflowIdentityInfo = GetWorkflowIdentityInfo(command, throwIfNotFound);
      return workflowIdentityInfo != null 
        ? GetWorkflowInfo(workflowIdentityInfo.TypeNamespace, workflowIdentityInfo.TypeName, throwIfNotFound) 
        : null;
    }

    /// <summary>
    /// 取工作流资料
    /// </summary>
    public static WorkflowInfo GetWorkflowInfo(WorkflowTaskInfo workflowTaskInfo, bool throwIfNotFound)
    {
      return GetWorkflowInfo(workflowTaskInfo.TypeNamespace, workflowTaskInfo.TypeName, throwIfNotFound);
    }

    /// <summary>
    /// 新增工作流资料
    /// </summary>
    public static void AddWorkflowInfo(WorkflowInfo workflowInfo)
    {
      AddWorkflowInfo(workflowInfo, UserIdentity.CurrentIdentity);
    }

    /// <summary>
    /// 新增工作流资料
    /// </summary>
    [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
    public static void AddWorkflowInfo(WorkflowInfo workflowInfo, UserIdentity identity)
    {
      if (workflowInfo == null)
        throw new ArgumentNullException("workflowInfo");
      AddWorkflowInfo(workflowInfo.TypeNamespace, workflowInfo.TypeName, workflowInfo.Caption, workflowInfo.XamlCode, identity);
    }

    /// <summary>
    /// 新增工作流资料
    /// </summary>
    [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
    public static void AddWorkflowInfo(string typeNamespace, string typeName, string caption, string xamlCode, UserIdentity identity)
    {
      try
      {
        CheckActive();
        identity = identity ?? UserIdentity.CurrentIdentity;
        Worker.AddWorkflowInfo(typeNamespace, typeName, caption, xamlCode, identity);
      }
      finally
      {
        WorkflowInfoHasChanged();
      }
    }

    /// <summary>
    /// 禁用工作流资料
    /// </summary>
    public static void DisableWorkflowInfo(WorkflowInfo workflowInfo)
    {
      DisableWorkflowInfo(workflowInfo, UserIdentity.CurrentIdentity);
    }

    /// <summary>
    /// 禁用工作流资料
    /// </summary>
    [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
    public static void DisableWorkflowInfo(WorkflowInfo workflowInfo, UserIdentity identity)
    {
      if (workflowInfo == null)
        throw new ArgumentNullException("workflowInfo");
      DisableWorkflowInfo(workflowInfo.TypeNamespace, workflowInfo.TypeName, identity);
    }

    /// <summary>
    /// 禁用工作流资料
    /// </summary>
    public static void DisableWorkflowInfo(string typeNamespace, string typeName)
    {
      DisableWorkflowInfo(typeNamespace, typeName, UserIdentity.CurrentIdentity);
    }

    /// <summary>
    /// 禁用工作流资料
    /// </summary>
    [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
    public static void DisableWorkflowInfo(string typeNamespace, string typeName, UserIdentity identity)
    {
      try
      {
        CheckActive();
        identity = identity ?? UserIdentity.CurrentIdentity;
        Worker.DisableWorkflowInfo(typeNamespace, typeName, identity);
      }
      finally
      {
        WorkflowInfoHasChanged();
      }
    }

    #endregion

    #region WorkflowInstance

    /// <summary>
    /// 保存工作流实例
    /// </summary>
    public static void SaveWorkflowInstance(Guid id, string typeNamespace, string typeName, string content, TaskContext taskContext)
    {
      CheckActive();
      Worker.SaveWorkflowInstance(id, typeNamespace, typeName, content, taskContext);
    }

    /// <summary>
    /// 检索工作流实例
    /// </summary>
    public static string FetchWorkflowInstance(Guid id)
    {
      CheckActive();
      return Worker.FetchWorkflowInstance(id);
    }

    /// <summary>
    /// 清除工作流实例
    /// </summary>
    public static void ClearWorkflowInstance(Guid id)
    {
      CheckActive();
      Worker.ClearWorkflowInstance(id);
    }
    
    #endregion

    #region WorkflowTask

    /// <summary>
    /// 发布工作流任务
    /// </summary>
    [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
    public static void DispatchWorkflowTask(Guid id, string bookmarkName,
      string pluginAssemblyName, string workerRole, string caption, string message, bool urgent)
    {
      CheckActive();
      Worker.DispatchWorkflowTask(id, bookmarkName, pluginAssemblyName, workerRole, caption, message, urgent);
    }

    /// <summary>
    /// 发布工作流任务
    /// </summary>
    /// <param name="id">工作流实例句柄</param>
    /// <param name="bookmarkName">书签名称</param>
    /// <param name="jointActivity">断点活动</param>
    public static void DispatchWorkflowTask(Guid id, string bookmarkName, IJointActivity jointActivity)
    {
      DispatchWorkflowTask(id, bookmarkName, jointActivity.PluginAssemblyName, jointActivity.WorkerRole, jointActivity.Caption, jointActivity.Message, jointActivity.Urgent);
    }

    /// <summary>
    /// 收到工作流任务
    /// </summary>
    /// <param name="id">工作流实例句柄</param>
    /// <param name="bookmarkName">书签名称</param>
    public static void ReceiveWorkflowTask(Guid id, string bookmarkName)
    {
      CheckActive();
      Worker.ReceiveWorkflowTask(id, bookmarkName);
    }

    /// <summary>
    /// 挂起工作流任务
    /// </summary>
    /// <param name="id">工作流实例句柄</param>
    /// <param name="bookmarkName">书签名称</param>
    public static void HoldWorkflowTask(Guid id, string bookmarkName)
    {
      CheckActive();
      Worker.HoldWorkflowTask(id, bookmarkName);
    }

    /// <summary>
    /// 中断工作流任务
    /// </summary>
    /// <param name="id">工作流实例句柄</param>
    /// <param name="bookmarkName">书签名称</param>
    public static void AbortWorkflowTask(Guid id, string bookmarkName)
    {
      CheckActive();
      Worker.AbortWorkflowTask(id, bookmarkName);
    }

    /// <summary>
    /// 继续工作流
    /// </summary>
    public static void ProceedWorkflow(WorkflowTaskInfo workflowTaskInfo)
    {
      CheckActive();
      Worker.ProceedWorkflow(workflowTaskInfo);
    }

    /// <summary>
    /// 完结工作流任务
    /// </summary>
    /// <param name="id">工作流实例句柄</param>
    /// <param name="bookmarkName">书签名称</param>
    public static void CompleteWorkflowTask(Guid id, string bookmarkName)
    {
      CheckActive();
      Worker.CompleteWorkflowTask(id, bookmarkName);
    }

    /// <summary>
    /// 检索工作流任务
    /// </summary>
    public static IList<WorkflowTaskInfo> FetchWorkflowTask(TaskState taskState, DateTime startDispatchTime, DateTime finishDispatchTime)
    {
      return FetchWorkflowTask(taskState, startDispatchTime, finishDispatchTime, UserIdentity.CurrentIdentity);
    }

    /// <summary>
    /// 检索工作流任务
    /// </summary>
    [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
    public static IList<WorkflowTaskInfo> FetchWorkflowTask(TaskState taskState, DateTime startDispatchTime, DateTime finishDispatchTime, UserIdentity identity)
    {
      CheckActive();
      identity = identity ?? UserIdentity.CurrentIdentity;
      if (identity == null && AppConfig.AutoMode)
        return new List<WorkflowTaskInfo>(0).AsReadOnly();
      return Worker.FetchWorkflowTask(taskState, startDispatchTime, finishDispatchTime, identity);
    }

    #endregion

    #region WorkflowIdentityInfo

    /// <summary>
    /// 取工作流标识信息
    /// </summary>
    public static WorkflowIdentityInfo GetWorkflowIdentityInfo(Type commandType, bool throwIfNotFound)
    {
      WorkflowIdentityInfo result = _workflowIdentityInfoCache.GetValue(commandType.FullName, () =>
      {
        if (!typeof(IStartCommand).IsAssignableFrom(commandType))
          throw new InvalidOperationException(String.Format("请为类{0}实现{1}接口", commandType.FullName, typeof(IStartCommand).FullName));
        return WorkflowIdentityInfo.Fetch(commandType);
      }, true);
      if (throwIfNotFound && result == null)
        throw new InvalidOperationException(String.Format("请为{0}类标记上{1}标签", commandType.FullName, typeof(WorkflowIdentityAttribute).FullName));
      return result;
    }

    /// <summary>
    /// 取工作流标识信息
    /// </summary>
    public static WorkflowIdentityInfo GetWorkflowIdentityInfo(IStartCommand command, bool throwIfNotFound)
    {
      return GetWorkflowIdentityInfo(command.GetType(), throwIfNotFound);
    }

    #endregion

    #endregion
  }
}