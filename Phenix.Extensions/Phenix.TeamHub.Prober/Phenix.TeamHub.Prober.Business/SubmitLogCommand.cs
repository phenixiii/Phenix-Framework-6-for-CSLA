using System;
using System.Data.Common;
using System.Reflection;
using Phenix.Core.Data;
using Phenix.Core.Security;

namespace Phenix.TeamHub.Prober.Business
{
  /// <summary>
  /// 提交日志
  /// </summary>
  [Serializable]
  internal class SubmitLogCommand : Phenix.Business.CommandBase<SubmitLogCommand>
  {
    /// <summary>
    /// 提交日志
    /// </summary>
    /// <param name="message">消息</param>
    /// <param name="method">函数的信息</param>
    /// <param name="exception">错误</param>
    public SubmitLogCommand(string message, MethodBase method, Exception exception)
    {
      UserIdentity identity = UserIdentity.CurrentIdentity;
      if (identity != null)
      {
        _userId = identity.UserId.ToString();
        _userNumber = identity.UserNumber;
      }
      _message = Phenix.Core.Reflection.Utilities.SubString(message, 4000, false);
      if (method != null && method.DeclaringType != null)
      {
        _assemblyName = method.DeclaringType.Assembly.FullName;
        _namespaceName = method.DeclaringType.Namespace;
        _className = method.DeclaringType.FullName;
        _methodName = method.Name;
      }
      if (exception != null)
      {
        _exceptionName = exception.GetType().FullName;
        _exceptionMessage = Phenix.Core.Reflection.Utilities.SubString(exception.Message, 4000, false);
        _exceptionStackTrace = exception.StackTrace;
      }
    }

    #region 属性

    private readonly string _userId;
    private readonly string _userNumber;
    private readonly string _message;
    private readonly string _assemblyName;
    private readonly string _namespaceName;
    private readonly string _className;
    private readonly string _methodName;
    private readonly string _exceptionName;
    private readonly string _exceptionMessage;
    private readonly string _exceptionStackTrace;

    #endregion

    #region 方法

    /// <summary>
    /// 处理执行指令(运行在持久层的程序域里)
    /// </summary>
    protected override void DoExecute()
    {
      try
      {
        DefaultDatabase.Execute(DoExecute,
          _userId, _userNumber, _message,
          _assemblyName, _namespaceName, _className, _methodName,
          _exceptionName, _exceptionMessage, _exceptionStackTrace);
      }
      catch (DbException ex)
      {
        try
        {
          DefaultDatabase.Execute(DoInitialize);
        }
        catch
        {
          throw ex;
        }
        DefaultDatabase.Execute(DoExecute,
           _userId, _userNumber, _message,
           _assemblyName, _namespaceName, _className, _methodName,
           _exceptionName, _exceptionMessage, _exceptionStackTrace);
      }
    }

    private static void DoInitialize(DbConnection connection)
    {
      DbCommandHelper.ExecuteNonQuery(connection,
       @"
CREATE TABLE PT_ExecuteLog (                 --执行日志
  EL_ID NUMERIC(15) NOT NULL,
  EL_Time DATE NOT NULL,                     --时间
  EL_UserID VARCHAR(100) NULL,               --用户ID
  EL_UserNumber VARCHAR(100) NULL,           --用户工号
  EL_Message VARCHAR(4000) NULL,             --消息
  EL_AssemblyName VARCHAR(255) NULL,         --程序集名
  EL_NamespaceName VARCHAR(255) NULL,        --命名空间名
  EL_ClassName VARCHAR(255) NULL,            --类名
  EL_MethodName VARCHAR(255) NULL,           --方法名
  EL_ExceptionName VARCHAR(255) NULL,        --错误名
  EL_ExceptionMessage VARCHAR(4000) NULL,    --错误消息
  EL_ExceptionStackTrace LONG /*TEXT*/ NULL, --错误调用堆栈
  PRIMARY KEY(EL_ID)
)", true);
    }

    private static void DoExecute(DbTransaction transaction, 
      string userId, string userNumber, string message, 
      string assemblyName, string namespaceName, string className, string methodName, 
      string exceptionName, string exceptionMessage, string exceptionStackTrace)
    {
      long id = Sequence.Value;
      using (DbCommand command = DbCommandHelper.CreateCommand(transaction,
@"insert into PT_ExecuteLog
  (EL_ID, EL_Time, EL_UserID, EL_UserNumber, EL_Message,
    EL_AssemblyName, EL_NamespaceName, EL_ClassName, EL_MethodName,
    EL_ExceptionName, EL_ExceptionMessage)
  values
  (:EL_ID, sysdate, :EL_UserID, :EL_UserNumber, :EL_Message,
    :EL_AssemblyName, :EL_NamespaceName, :EL_ClassName, :EL_MethodName,
    :EL_ExceptionName, :EL_ExceptionMessage)"
        ))
      {
        DbCommandHelper.CreateParameter(command, "EL_ID", id);
        DbCommandHelper.CreateParameter(command, "EL_UserID", userId);
        DbCommandHelper.CreateParameter(command, "EL_UserNumber", userNumber);
        DbCommandHelper.CreateParameter(command, "EL_Message", message);
        DbCommandHelper.CreateParameter(command, "EL_AssemblyName", assemblyName);
        DbCommandHelper.CreateParameter(command, "EL_NamespaceName", namespaceName);
        DbCommandHelper.CreateParameter(command, "EL_ClassName", className);
        DbCommandHelper.CreateParameter(command, "EL_MethodName", methodName);
        DbCommandHelper.CreateParameter(command, "EL_ExceptionName", exceptionName);
        DbCommandHelper.CreateParameter(command, "EL_ExceptionMessage", exceptionMessage);
        DbCommandHelper.ExecuteNonQuery(command, false);
      }
      if (!String.IsNullOrEmpty(exceptionStackTrace))
        using (DbCommand command = DbCommandHelper.CreateCommand(transaction,
@"update PT_ExecuteLog set
    EL_ExceptionStackTrace = :EL_ExceptionStackTrace
  where EL_ID = :EL_ID"
          ))
        {
          DbCommandHelper.CreateParameter(command, "EL_ExceptionStackTrace", exceptionStackTrace);
          DbCommandHelper.CreateParameter(command, "EL_ID", id);
          DbCommandHelper.ExecuteNonQuery(command, false);
        }
    }

    #endregion
  }
}
