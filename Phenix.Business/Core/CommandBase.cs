using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using Csla;
using Phenix.Business.Rules;
using Phenix.Core.Dictionary;
using Phenix.Core.IO;
using Phenix.Core.Log;
using Phenix.Core.Mapping;
using Phenix.Core.Reflection;
using Phenix.Core.Security;

namespace Phenix.Business.Core
{
  /// <summary>
  /// 指令基类
  /// </summary>
  [Serializable]
  [DataDictionary(AssemblyClassType.Command)]
  [ClassAttribute(null)]
  public abstract class CommandBase<T> : Csla.CommandBase<T>, IFactory, IService
    where T : CommandBase<T>
  {
    #region CreateInstance

    /// <summary>
    /// 构建实体
    /// </summary>
    protected virtual T CreateInstance()
    {
      return (T)Csla.Reflection.MethodCaller.CreateInstance(typeof(T));
    }
    object IFactory.CreateInstance()
    {
      return CreateInstance();
    }

    private static readonly IFactory _factory = (IFactory)FormatterServices.GetUninitializedObject(typeof(T));

    /// <summary>
    /// 构建实体
    /// </summary>
    protected static T DynamicCreateInstance()
    {
      return (T)_factory.CreateInstance();
    }

    #endregion

    #region 属性

    /// <summary>
    /// 数据源键
    /// 缺省为 T 上的 ClassAttribute.DataSourceKey
    /// </summary>
    [Newtonsoft.Json.JsonIgnore]
    [System.ComponentModel.Browsable(false)]
    [System.ComponentModel.DataAnnotations.Display(AutoGenerateField = false)]
    public virtual string DataSourceKey
    {
      get { return ClassMemberHelper.GetDataSourceKey(this.GetType()); }
    }

    /// <summary>
    /// 标签
    /// 缺省为 T 上的 ClassAttribute.FriendlyName
    /// 用于提示信息等
    /// </summary>
    [Newtonsoft.Json.JsonIgnore]
    public virtual string Caption
    {
      get { return ClassMemberHelper.GetFriendlyName(this.GetType()); }
    }

    /// <summary>
    /// 主键值
    /// 缺省为 Caption
    /// </summary>
    [Newtonsoft.Json.JsonIgnore]
    [System.ComponentModel.Browsable(false)]
    [System.ComponentModel.DataAnnotations.Display(AutoGenerateField = false)]
    public virtual string PrimaryKey
    {
      get { return Caption; }
    }

    #region Authorization Rules

    private static AuthorizationRules _authorizationRules;
    /// <summary>
    /// 授权规则集合
    /// </summary>
    protected static AuthorizationRules AuthorizationRules
    {
      get
      {
        if (_authorizationRules == null)
          _authorizationRules = new AuthorizationRules(typeof(T));
        return _authorizationRules;
      }
    }

    /// <summary>
    /// 是否允许Execute
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1000:DoNotDeclareStaticMembersOnGenericTypes")]
    public static bool CanExecute
    {
      get
      {
        if (ApplicationContext.LogicalExecutionLocation == ApplicationContext.LogicalExecutionLocations.Server)
          return true;
        return !UserIdentity.IsByDeny(UserIdentity.CurrentIdentity, typeof(T), ExecuteAction.Update, false) && 
          Csla.Rules.BusinessRules.HasPermission(Csla.Rules.AuthorizationActions.EditObject, typeof(T));
      }
    }

    #endregion

    #endregion

    #region 方法

    /// <summary>
    /// 属性已更改
    /// </summary>
    protected void PropertyHasChanged()
    {
      string propertyName = new System.Diagnostics.StackTrace().GetFrame(1).GetMethod().Name.Substring(4);
      OnPropertyChanged(propertyName);
    }
    
    #region Data Access

    /// <summary>
    /// DataPortal_Execute
    /// </summary>
    protected override void DataPortal_Execute()
    {
      DoExecute();
    }
    /// <summary>
    /// 处理执行指令(运行在持久层的程序域里)
    /// </summary>
    protected virtual void DoExecute()
    {
      PermanentLog();
    }
    void IService.DoExecute()
    {
      DoExecute();
    }

    /// <summary>
    /// 处理上传文件(运行在持久层的程序域里)
    /// </summary>
    /// <param name="fileStreams">待处理的文件流</param>
    protected virtual void DoUploadFiles(IDictionary<string, Stream> fileStreams)
    {
      PermanentLog();
    }
    void IService.DoUploadFiles(IDictionary<string, Stream> fileStreams)
    {
      DoUploadFiles(fileStreams);
    }
    void IService.DoUploadFiles(IDictionary<string, byte[]> fileByteses)
    {
      Dictionary<string, Stream> fileStreams = new Dictionary<string, Stream>(fileByteses.Count, StringComparer.OrdinalIgnoreCase);
      try
      {
        foreach (KeyValuePair<string, byte[]> kvp in fileByteses)
          fileStreams.Add(kvp.Key, new MemoryStream(kvp.Value));
        DoUploadFiles(fileStreams);
      }
      finally
      {
        foreach (KeyValuePair<string, Stream> kvp in fileStreams)
          kvp.Value.Dispose();
      }
    }

    /// <summary>
    /// 处理上传大文件(运行在持久层的程序域里)
    /// </summary>
    /// <param name="fileChunkInfo">待处理的文件块信息</param>
    protected virtual void DoUploadBigFile(FileChunkInfo fileChunkInfo)
    {
      PermanentLog();
    }
    void IService.DoUploadBigFile(FileChunkInfo fileChunkInfo)
    {
      DoUploadBigFile(fileChunkInfo);
    }

    /// <summary>
    /// 获取下载文件(运行在持久层的程序域里)
    /// </summary>
    /// <returns>文件流</returns>
    protected virtual Stream DoDownloadFile()
    {
      PermanentLog();
      return null;
    }
    Stream IService.DoDownloadFile()
    {
      return DoDownloadFile();
    }

    /// <summary>
    /// 获取下载文件(运行在持久层的程序域里)
    /// </summary>
    /// <returns>文件字节串</returns>
    protected byte[] DoDownloadFileBytes()
    {
      using (Stream stream = DoDownloadFile())
      {
        return stream != null ? Phenix.Core.IO.StreamHelper.CopyBuffer(stream).ToArray() : null;
      }
    }
    byte[] IService.DoDownloadFileBytes()
    {
      return DoDownloadFileBytes();
    }

    /// <summary>
    /// 获取下载大文件(运行在持久层的程序域里)
    /// </summary>
    /// <param name="chunkNumber">块号</param>
    /// <returns>文件块信息</returns>
    protected virtual FileChunkInfo DoDownloadBigFile(int chunkNumber)
    {
      PermanentLog();
      return null;
    }
    FileChunkInfo IService.DoDownloadBigFile(int chunkNumber)
    {
      return DoDownloadBigFile(chunkNumber);
    }

    #endregion

    #region Permanent Log

    /// <summary>
    /// 持久化执行动作日志
    /// </summary>
    protected void PermanentLog()
    {
      ClassMapInfo classMapInfo = ClassMemberHelper.GetClassMapInfo(this.GetType());
      if (classMapInfo.PermanentExecuteAction != ExecuteAction.None)
        PermanentLogHub.SaveExecuteAction(this.GetType(), Caption, PrimaryKey, ExecuteAction.Update, EntityHelper.GetFieldValues(this), null);
    }

    /// <summary>
    /// 检索执行动作
    /// </summary>
    /// <returns>执行动作信息队列</returns>
    public IList<ExecuteActionInfo> FetchExecuteAction()
    {
      ClassMapInfo classMapInfo = ClassMemberHelper.GetClassMapInfo(this.GetType());
      if (classMapInfo.PermanentExecuteAction != ExecuteAction.None)
        return PermanentLogHub.FetchExecuteAction(this.GetType(), PrimaryKey);
      return null;
    }

    /// <summary>
    /// 检索执行动作
    /// </summary>
    /// <param name="startTime">起始时间</param>
    /// <param name="finishTime">结束时间</param>
    /// <returns>执行动作信息队列</returns>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1000:DoNotDeclareStaticMembersOnGenericTypes")]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures")]
    public static IList<ExecuteActionInfo> FetchExecuteAction(
      DateTime startTime, DateTime finishTime)
    {
      return PermanentLogHub.FetchExecuteAction(null, typeof(T), ExecuteAction.Update, startTime, finishTime);
    }

    /// <summary>
    /// 检索执行动作
    /// </summary>
    /// <param name="userNumber">登录工号, null代表全部</param>
    /// <param name="startTime">起始时间</param>
    /// <param name="finishTime">结束时间</param>
    /// <returns>执行动作信息队列</returns>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1000:DoNotDeclareStaticMembersOnGenericTypes")]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures")]
    public static IList<ExecuteActionInfo> FetchExecuteAction(string userNumber,
      DateTime startTime, DateTime finishTime)
    {
      return PermanentLogHub.FetchExecuteAction(userNumber, typeof(T), ExecuteAction.Update, startTime, finishTime);
    }

    /// <summary>
    /// 清除执行动作
    /// </summary>
    /// <param name="action">执行动作</param>
    /// <param name="startTime">起始时间</param>
    /// <param name="finishTime">结束时间</param>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1000:DoNotDeclareStaticMembersOnGenericTypes")]
    public static void ClearExecuteAction(ExecuteAction action,
      DateTime startTime, DateTime finishTime)
    {
      PermanentLogHub.ClearExecuteAction(null, typeof(T), action, startTime, finishTime);
    }

    /// <summary>
    /// 清除执行动作
    /// </summary>
    /// <param name="userNumber">登录工号, null代表全部</param>
    /// <param name="action">执行动作</param>
    /// <param name="startTime">起始时间</param>
    /// <param name="finishTime">结束时间</param>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1000:DoNotDeclareStaticMembersOnGenericTypes")]
    public static void ClearExecuteAction(string userNumber, ExecuteAction action,
      DateTime startTime, DateTime finishTime)
    {
      PermanentLogHub.ClearExecuteAction(userNumber, typeof(T), action, startTime, finishTime);
    }

    #endregion

    #endregion
  }
}
