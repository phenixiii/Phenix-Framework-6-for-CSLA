#if Top
using Phenix.Core.Web;
#endif

using System;
using System.Collections.Generic;
using System.Data.Common;
using System.IO;
using System.Reflection;
using Phenix.Core;
using Phenix.Core.Data;
using Phenix.Core.IO;
using Phenix.Core.Log;

namespace Phenix.Business
{
  /// <summary>
  /// 指令基类 
  /// 在DataPortal_Execute()、DoExecute()函数(任选一处)中编写运行在服务端的指令处理代码
  /// 消费者调用Execute()函数提交指令
  /// </summary>
  [Serializable]
  public abstract class CommandBase<T> : Phenix.Business.Core.CommandBase<T>
    where T : CommandBase<T>
  {
    #region 属性

    private ShallEventArgs _executeResult = new ShallEventArgs();
    /// <summary>
    /// 执行结果
    /// </summary>
    [Newtonsoft.Json.JsonIgnore]
    [System.ComponentModel.Browsable(false)]
    [System.ComponentModel.DataAnnotations.Display(AutoGenerateField = false)]
    public ShallEventArgs ExecuteResult
    {
      get { return _executeResult; }
    }
    
    #endregion

    #region 方法

    /// <summary>
    /// 置换为与source相同内容的对象
    /// </summary>
    protected virtual void ReplaceFrom(T source)
    {
      if (object.ReferenceEquals(source, null) || object.ReferenceEquals(source, this))
        return;

      Phenix.Core.Reflection.Utilities.FillFieldValues(source, this, true);
      _executeResult = source._executeResult;
    }

    /// <summary>
    /// 执行指令
    /// </summary>
    /// <returns>this</returns>
    public T Execute()
    {
      ReplaceFrom(Execute((T)this));
      return (T)this;
    }

    /// <summary>
    /// 执行指令
    /// </summary>
    /// <param name="inParam">输入参数对象</param>
    /// <returns>输出参数对象</returns>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1000:DoNotDeclareStaticMembersOnGenericTypes")]
    public static T Execute(T inParam)
    {
      if (inParam == null)
        throw new ArgumentNullException("inParam");
      if (!inParam.OnExecuting())
        return null;
      try
      {
        inParam._executeResult.Applied = true;
        T result = Csla.DataPortal.Update(inParam);
        inParam.OnExecuted(null as Exception);
        return result;
      }
      catch (Exception ex)
      {
        inParam._executeResult.Succeed = false;
        EventLog.Save(inParam.GetType(), MethodBase.GetCurrentMethod(), ex);
        string s = inParam.OnExecuted(ex);
        if (String.IsNullOrEmpty(s))
          throw;
        else
          throw new ExecuteException(s, ex);
      }
    }

    /// <summary>
    /// 上传文件
    /// </summary>
    /// <param name="fileNames">待上传的文件路径</param>
    /// <returns>this</returns>
    public T UploadFiles(params string[] fileNames)
    {
      ReplaceFrom(UploadFiles((T)this, fileNames));
      return (T)this;
    }

    /// <summary>
    /// 上传文件
    /// </summary>
    /// <param name="inParam">输入参数对象</param>
    /// <param name="fileNames">待上传的文件路径</param>
    /// <returns>输出参数对象</returns>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1000:DoNotDeclareStaticMembersOnGenericTypes")]
    public static T UploadFiles(T inParam, params string[] fileNames)
    {
      if (inParam == null)
        throw new ArgumentNullException("inParam");
      if (!inParam.OnExecuting())
        return null;
      try
      {
        inParam._executeResult.Applied = true;
        T result;
#if Top
        if (AppHub.DataProxy != null)
          result = AppHub.DataProxy.UploadFiles(inParam, fileNames);
        else
#endif
          result = (T)DataHub.UploadFiles(inParam, fileNames);
        inParam.OnExecuted(null as Exception);
        return result;
      }
      catch (Exception ex)
      {
        inParam._executeResult.Succeed = false;
        EventLog.Save(inParam.GetType(), MethodBase.GetCurrentMethod(), ex);
        string s = inParam.OnExecuted(ex);
        if (String.IsNullOrEmpty(s))
          throw;
        else
          throw new ExecuteException(s, ex);
      }
    }

    /// <summary>
    /// 上传大文件
    /// </summary>
    /// <param name="fileName">待上传的文件路径</param>
    /// <param name="doProgress">执行进度干预</param>
    /// <returns>this</returns>
    public T UploadBigFile(string fileName, Func<object, FileChunkInfo, bool> doProgress)
    {
      ReplaceFrom(UploadBigFile((T)this, fileName, doProgress));
      return (T)this;
    }

    /// <summary>
    /// 上传大文件
    /// </summary>
    /// <param name="inParam">输入参数对象</param>
    /// <param name="fileName">待上传的文件路径</param>
    /// <param name="doProgress">执行进度干预</param>
    /// <returns>输出参数对象</returns>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1000:DoNotDeclareStaticMembersOnGenericTypes")]
    public static T UploadBigFile(T inParam, string fileName, Func<object, FileChunkInfo, bool> doProgress)
    {
      if (inParam == null)
        throw new ArgumentNullException("inParam");
      try
      {
        inParam._executeResult.Applied = true;
        T result;
#if Top
        if (AppHub.DataProxy != null)
          result = AppHub.DataProxy.UploadBigFile(inParam, fileName, doProgress);
        else
#endif
          result = (T)DataHub.UploadBigFile(inParam, fileName, doProgress);
        inParam.OnExecuted(null as Exception);
        return result;
      }
      catch (Exception ex)
      {
        inParam._executeResult.Succeed = false;
        EventLog.Save(inParam.GetType(), MethodBase.GetCurrentMethod(), ex);
        string s = inParam.OnExecuted(ex);
        if (String.IsNullOrEmpty(s))
          throw;
        else
          throw new ExecuteException(s, ex);
      }
    }

    /// <summary>
    /// 下载文件
    /// </summary>
    /// <param name="fileName">待保存的文件路径</param>
    public void DownloadFile(string fileName)
    {
      DownloadFile((T)this, fileName);
    }

    /// <summary>
    /// 下载文件
    /// </summary>
    /// <param name="inParam">输入参数对象</param>
    /// <param name="fileName">待保存的文件路径</param>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1000:DoNotDeclareStaticMembersOnGenericTypes")]
    public static void DownloadFile(T inParam, string fileName)
    {
      if (inParam == null)
        throw new ArgumentNullException("inParam");
      if (!inParam.OnExecuting())
        return;
      try
      {
        inParam._executeResult.Applied = true;
#if Top
        if (AppHub.DataProxy != null)
          AppHub.DataProxy.DownloadFile(inParam, fileName);
        else
#endif
          DataHub.DownloadFileBytes(inParam, fileName);
        inParam.OnExecuted(null as Exception);
      }
      catch (Exception ex)
      {
        inParam._executeResult.Succeed = false;
        EventLog.Save(inParam.GetType(), MethodBase.GetCurrentMethod(), ex);
        string s = inParam.OnExecuted(ex);
        if (String.IsNullOrEmpty(s))
          throw;
        else
          throw new ExecuteException(s, ex);
      }
    }

    /// <summary>
    /// 下载大文件
    /// </summary>
    /// <param name="fileName">待保存的文件路径</param>
    /// <param name="doProgress">执行进度干预</param>
    public void DownloadBigFile(string fileName, Func<object, FileChunkInfo, bool> doProgress)
    {
      DownloadBigFile((T)this, fileName, doProgress);
    }

    /// <summary>
    /// 下载大文件
    /// </summary>
    /// <param name="inParam">输入参数对象</param>
    /// <param name="fileName">待保存的文件路径</param>
    /// <param name="doProgress">执行进度干预</param>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1000:DoNotDeclareStaticMembersOnGenericTypes")]
    public static void DownloadBigFile(T inParam, string fileName, Func<object, FileChunkInfo, bool> doProgress)
    {
      if (inParam == null)
        throw new ArgumentNullException("inParam");
      if (!inParam.OnExecuting())
        return;
      try
      {
        inParam._executeResult.Applied = true;
#if Top
        if (AppHub.DataProxy != null)
          AppHub.DataProxy.DownloadBigFile(inParam, fileName, doProgress);
        else
#endif
          DataHub.DownloadBigFile(inParam, fileName, doProgress);
        inParam.OnExecuted(null as Exception);
      }
      catch (Exception ex)
      {
        inParam._executeResult.Succeed = false;
        EventLog.Save(inParam.GetType(), MethodBase.GetCurrentMethod(), ex);
        string s = inParam.OnExecuted(ex);
        if (String.IsNullOrEmpty(s))
          throw;
        else
          throw new ExecuteException(s, ex);
      }
    }
    
    /// <summary>
    /// 执行指令
    /// </summary>
    /// <param name="connection">数据库连接</param>
    /// <returns>this</returns>
    public T Execute(DbConnection connection)
    {
      return Execute(connection, (T)this);
    }

    /// <summary>
    /// 执行指令
    /// </summary>
    /// <param name="connection">数据库连接</param>
    /// <param name="inParam">输入参数对象</param>
    /// <returns>输出参数对象</returns>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1000:DoNotDeclareStaticMembersOnGenericTypes")]
    public static T Execute(DbConnection connection, T inParam)
    {
      if (inParam == null)
        throw new ArgumentNullException("inParam");
      if (!inParam.OnExecuting())
        return null;
      try
      {
        inParam._executeResult.Applied = true;
        inParam.DoExecute(connection);
        inParam.OnExecuted(null as Exception);
        return inParam;
      }
      catch (Exception ex)
      {
        inParam._executeResult.Succeed = false;
        EventLog.Save(inParam.GetType(), MethodBase.GetCurrentMethod(), ex);
        string s = inParam.OnExecuted(ex);
        if (String.IsNullOrEmpty(s))
          throw;
        else
          throw new ExecuteException(s, ex);
      }
    }

    /// <summary>
    /// 执行指令
    /// </summary>
    /// <param name="transaction">数据库事务</param>
    /// <returns>this</returns>
    public T Execute(DbTransaction transaction)
    {
      return Execute(transaction, (T)this);
    }

    /// <summary>
    /// 执行指令
    /// </summary>
    /// <param name="transaction">数据库事务</param>
    /// <param name="inParam">输入参数对象</param>
    /// <returns>输出参数对象</returns>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1000:DoNotDeclareStaticMembersOnGenericTypes")]
    public static T Execute(DbTransaction transaction, T inParam)
    {
      if (inParam == null)
        throw new ArgumentNullException("inParam");
      if (!inParam.OnExecuting())
        return null;
      try
      {
        inParam._executeResult.Applied = true;
        inParam.DoExecute(transaction);
        inParam.OnExecuted(null as Exception);
        return inParam;
      }
      catch (Exception ex)
      {
        inParam._executeResult.Succeed = false;
        EventLog.Save(inParam.GetType(), MethodBase.GetCurrentMethod(), ex);
        string s = inParam.OnExecuted(ex);
        if (String.IsNullOrEmpty(s))
          throw;
        else
          throw new ExecuteException(s, ex);
      }
    }

    /// <summary>
    /// 执行指令之前
    /// </summary>
    /// <returns>是否继续, 缺省为 true</returns>
    protected virtual bool OnExecuting()
    {
      return true;
    }

    /// <summary>
    /// 执行指令之后
    /// </summary>
    /// <param name="ex">错误信息</param>
    /// <returns>发生错误时的友好提示信息, 缺省为 null</returns>
    protected virtual string OnExecuted(Exception ex)
    {
      return null;
    }

    #region Data Access

    /// <summary>
    /// 处理执行指令(运行在持久层的程序域里)
    /// </summary>
    protected override void DoExecute()
    {
      try
      {
        if (typeof(T).GetMethod("DoExecute",
          BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic,
          null, new Type[] { typeof(DbConnection) }, null) != null)
          DbConnectionHelper.Execute(DataSourceKey, (Action<DbConnection>)DoExecute);
        if (typeof(T).GetMethod("DoExecute",
          BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic,
          null, new Type[] { typeof(DbTransaction) }, null) != null)
          DbConnectionHelper.Execute(DataSourceKey, (Action<DbTransaction>)DoExecute);
      }
      finally 
      {
        base.DoExecute();
      }
    }

    /// <summary>
    /// 处理执行指令(运行在持久层的程序域里)
    /// 请使用业务对象处理逻辑
    /// 如直接操作数据也请用Phenix.Core.Data.DbCommandHelper与数据库交互以保证可移植性
    /// 如果重载了DoExecute()且未调用base.DoExecute()，则本函数将不会执行到
    /// 如果重载了DataPortal_Execute()且未调用base.DataPortal_Execute()，则本函数将不会执行到
    /// </summary>
    /// <param name="connection">数据库连接</param>
    protected virtual void DoExecute(DbConnection connection) { }

    /// <summary>
    /// 处理执行指令(运行在持久层的程序域里)
    /// 请使用业务对象处理逻辑
    /// 如直接操作数据也请用Phenix.Core.Data.DbCommandHelper与数据库交互以保证可移植性
    /// 如果重载了DoExecute()且未调用base.DoExecute()，则本函数将不会执行到
    /// 如果重载了DataPortal_Execute()且未调用base.DataPortal_Execute()，则本函数将不会执行到
    /// 如果拦截了异常，请处理后继续抛出，以便交给基类执行Rollback()
    /// </summary>
    /// <param name="transaction">数据库事务</param>
    protected virtual void DoExecute(DbTransaction transaction) { }

    /// <summary>
    /// 处理上传文件(运行在持久层的程序域里)
    /// </summary>
    /// <param name="fileStreams">待处理的文件流</param>
    protected override void DoUploadFiles(IDictionary<string, Stream> fileStreams)
    {
      try
      {
        if (typeof(T).GetMethod("DoUploadFiles",
          BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic,
          null, new Type[] { typeof(DbConnection), typeof(IDictionary<string, Stream>) }, null) != null)
          DbConnectionHelper.Execute(DataSourceKey, (Action<DbConnection, IDictionary<string, Stream>>)DoUploadFiles, fileStreams);
        if (typeof(T).GetMethod("DoUploadFiles",
          BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic,
          null, new Type[] { typeof(DbTransaction), typeof(IDictionary<string, Stream>) }, null) != null)
          DbConnectionHelper.Execute(DataSourceKey, (Action<DbTransaction, IDictionary<string, Stream>>)DoUploadFiles, fileStreams);
      }
      finally
      {
        base.DoUploadFiles(fileStreams);
      }
    }

    /// <summary>
    /// 处理上传文件(运行在持久层的程序域里)
    /// 请使用业务对象处理逻辑
    /// 如直接操作数据也请用Phenix.Core.Data.DbCommandHelper与数据库交互以保证可移植性
    /// 如果重载了DoUploadFiles()且未调用base.DoUploadFiles()，则本函数将不会执行到
    /// </summary>
    /// <param name="connection">数据库连接</param>
    /// <param name="fileStreams">待处理的文件流</param>
    protected virtual void DoUploadFiles(DbConnection connection, IDictionary<string, Stream> fileStreams) { }

    /// <summary>
    /// 处理上传文件(运行在持久层的程序域里)
    /// 请使用业务对象处理逻辑
    /// 如直接操作数据也请用Phenix.Core.Data.DbCommandHelper与数据库交互以保证可移植性
    /// 如果重载了DoUploadFiles()且未调用base.DoUploadFiles()，则本函数将不会执行到
    /// 如果拦截了异常，请处理后继续抛出，以便交给基类执行Rollback()
    /// </summary>
    /// <param name="transaction">数据库事务</param>
    /// <param name="fileStreams">待处理的文件流</param>
    protected virtual void DoUploadFiles(DbTransaction transaction, IDictionary<string, Stream> fileStreams) { }

    /// <summary>
    /// 处理上传大文件(运行在持久层的程序域里)
    /// </summary>
    /// <param name="fileChunkInfo">待处理的文件块信息</param>
    protected override void DoUploadBigFile(FileChunkInfo fileChunkInfo)
    {
      try
      {
        if (typeof(T).GetMethod("DoUploadBigFile",
          BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic,
          null, new Type[] { typeof(DbConnection), typeof(FileChunkInfo) }, null) != null)
          DbConnectionHelper.Execute(DataSourceKey, (Action<DbConnection, FileChunkInfo>)DoUploadBigFile, fileChunkInfo);
        if (typeof(T).GetMethod("DoUploadBigFile",
          BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic,
          null, new Type[] { typeof(DbTransaction), typeof(FileChunkInfo) }, null) != null)
          DbConnectionHelper.Execute(DataSourceKey, (Action<DbTransaction, FileChunkInfo>)DoUploadBigFile, fileChunkInfo);
      }
      finally
      {
        base.DoUploadBigFile(fileChunkInfo);
      }
    }

    /// <summary>
    /// 处理上传大文件(运行在持久层的程序域里)
    /// 请使用业务对象处理逻辑
    /// 如直接操作数据也请用Phenix.Core.Data.DbCommandHelper与数据库交互以保证可移植性
    /// 如果重载了DoUploadBigFile()且未调用base.DoUploadBigFile()，则本函数将不会执行到
    /// </summary>
    /// <param name="connection">数据库连接</param>
    /// <param name="fileChunkInfo">待处理的文件块信息</param>
    protected virtual void DoUploadBigFile(DbConnection connection, FileChunkInfo fileChunkInfo) { }

    /// <summary>
    /// 处理上传大文件(运行在持久层的程序域里)
    /// 请使用业务对象处理逻辑
    /// 如直接操作数据也请用Phenix.Core.Data.DbCommandHelper与数据库交互以保证可移植性
    /// 如果重载了DoUploadBigFile()且未调用base.DoUploadBigFile()，则本函数将不会执行到
    /// 如果拦截了异常，请处理后继续抛出，以便交给基类执行Rollback()
    /// </summary>
    /// <param name="transaction">数据库事务</param>
    /// <param name="fileChunkInfo">待处理的文件块信息</param>
    protected virtual void DoUploadBigFile(DbTransaction transaction, FileChunkInfo fileChunkInfo) { }

    /// <summary>
    /// 获取下载文件(运行在持久层的程序域里)
    /// </summary>
    /// <returns>文件流</returns>
    protected override Stream DoDownloadFile()
    {
      try
      {
        if (typeof(T).GetMethod("DoDownloadFile",
          BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic,
          null, new Type[] { typeof(DbConnection) }, null) != null)
          return DbConnectionHelper.ExecuteGet<Stream>(DataSourceKey, (Func<DbConnection, Stream>)DoDownloadFile);
        if (typeof(T).GetMethod("DoDownloadFile",
          BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic,
          null, new Type[] { typeof(DbTransaction) }, null) != null)
          return DbConnectionHelper.ExecuteGet<Stream>(DataSourceKey, (Func<DbTransaction, Stream>)DoDownloadFile);
        return null;
      }
      finally
      {
        base.DoDownloadFile();
      }
    }

    /// <summary>
    /// 获取下载文件(运行在持久层的程序域里)
    /// 请使用业务对象处理逻辑
    /// 如直接操作数据也请用Phenix.Core.Data.DbCommandHelper与数据库交互以保证可移植性
    /// 如果重载了DoDownloadFile()且未调用base.DoDownloadFile()，则本函数将不会执行到
    /// </summary>
    /// <param name="connection">数据库连接</param>
    /// <returns>文件流</returns>
    protected virtual Stream DoDownloadFile(DbConnection connection)
    {
      return null;
    }

    /// <summary>
    /// 获取下载文件(运行在持久层的程序域里)
    /// 请使用业务对象处理逻辑
    /// 如直接操作数据也请用Phenix.Core.Data.DbCommandHelper与数据库交互以保证可移植性
    /// 如果重载了DoDownloadFile()且未调用base.DoDownloadFile()，则本函数将不会执行到
    /// 如果拦截了异常，请处理后继续抛出，以便交给基类执行Rollback()
    /// </summary>
    /// <param name="transaction">数据库事务</param>
    /// <returns>文件流</returns>
    protected virtual Stream DoDownloadFile(DbTransaction transaction)
    {
      return null;
    }

    /// <summary>
    /// 获取下载大文件(运行在持久层的程序域里)
    /// </summary>
    /// <param name="chunkNumber">块号</param>
    /// <returns>文件块信息</returns>
    protected override FileChunkInfo DoDownloadBigFile(int chunkNumber)
    {
      try
      {
        if (typeof(T).GetMethod("DoDownloadBigFile",
          BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic,
          null, new Type[] { typeof(DbConnection), typeof(int) }, null) != null)
          return DbConnectionHelper.ExecuteGet<int, FileChunkInfo>(DataSourceKey, (Func<DbConnection, int, FileChunkInfo>)DoDownloadBigFile, chunkNumber);
        if (typeof(T).GetMethod("DoDownloadBigFile",
          BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic,
          null, new Type[] { typeof(DbTransaction), typeof(int) }, null) != null)
          return DbConnectionHelper.ExecuteGet<int, FileChunkInfo>(DataSourceKey, (Func<DbTransaction, int, FileChunkInfo>)DoDownloadBigFile, chunkNumber);
        return null;
      }
      finally
      {
        base.DoDownloadBigFile(chunkNumber);
      }
    }

    /// <summary>
    /// 获取下载大文件(运行在持久层的程序域里)
    /// 请使用业务对象处理逻辑
    /// 如直接操作数据也请用Phenix.Core.Data.DbCommandHelper与数据库交互以保证可移植性
    /// 如果重载了DoDownloadBigFile()且未调用base.DoDownloadBigFile()，则本函数将不会执行到
    /// </summary>
    /// <param name="connection">数据库连接</param>
    /// <param name="chunkNumber">块号</param>
    /// <returns>文件块信息</returns>
    protected virtual FileChunkInfo DoDownloadBigFile(DbConnection connection, int chunkNumber)
    {
      return null;
    }

    /// <summary>
    /// 获取下载大文件(运行在持久层的程序域里)
    /// 请使用业务对象处理逻辑
    /// 如直接操作数据也请用Phenix.Core.Data.DbCommandHelper与数据库交互以保证可移植性
    /// 如果重载了DoDownloadBigFile()且未调用base.DoDownloadBigFile()，则本函数将不会执行到
    /// 如果拦截了异常，请处理后继续抛出，以便交给基类执行Rollback()
    /// </summary>
    /// <param name="transaction">数据库事务</param>
    /// <param name="chunkNumber">块号</param>
    /// <returns>文件块信息</returns>
    protected virtual FileChunkInfo DoDownloadBigFile(DbTransaction transaction, int chunkNumber)
    {
      return null;
    }

    #endregion

    #endregion
  }
}