using System;
using System.Collections.Generic;
using System.Data.Common;
using Phenix.Core.Mapping;

namespace Phenix.Core.Log
{
  /// <summary>
  /// 持久化日志接口
  /// </summary>
  public interface IPermanentLog
  {
    #region 方法

    /// <summary>
    /// 保存对象消息
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1716:IdentifiersShouldNotMatchKeywords")]
    void Save(string userNumber, string typeName, string message, Exception error);

    /// <summary>
    /// 检索日志消息
    /// </summary>
    IList<EventLogInfo> Fetch(string userNumber, string typeName,
      DateTime startTime, DateTime finishTime);

    /// <summary>
    /// 清除日志消息
    /// </summary>
    void Clear(string userNumber, string typeName,
      DateTime startTime, DateTime finishTime);

    /// <summary>
    /// 保存对象持久化的执行动作
    /// </summary>
    void SaveExecuteAction(string userNumber, string typeName, string primaryKey,
      ExecuteAction action, string log);

    /// <summary>
    /// 检索对象持久化的执行动作
    /// </summary>
    IList<string> FetchExecuteAction(string typeName, string primaryKey);

    /// <summary>
    /// 检索对象持久化的执行动作
    /// </summary>
    IList<string> FetchExecuteAction(string userNumber, string typeName, 
      ExecuteAction action, DateTime startTime, DateTime finishTime);

    /// <summary>
    /// 清除对象持久化的执行动作
    /// </summary>
    void ClearExecuteAction(string userNumber, string typeName, 
      ExecuteAction action, DateTime startTime, DateTime finishTime);

    #region 代入数据库事务

    /// <summary>
    /// 保存对象持久化的动态刷新
    /// </summary>
    void SaveRenovate(DbTransaction transaction, string tableName, ExecuteAction action, IList<FieldValue> fieldValues);

    #endregion

    #endregion
  }
}
