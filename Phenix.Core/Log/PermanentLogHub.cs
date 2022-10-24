using System;
using System.Collections.Generic;
using System.Data.Common;
using System.IO;
using System.Reflection;
using System.Text;
using System.Xml;
using Phenix.Core.Mapping;
using Phenix.Core.Reflection;
using Phenix.Core.Security;

namespace Phenix.Core.Log
{
  /// <summary>
  /// 持久化日志中心
  /// </summary>
  public static class PermanentLogHub
  {
    #region 属性

    private static IPermanentLog _worker;
    /// <summary>
    /// 实施者
    /// </summary>
    public static IPermanentLog Worker
    {
      get
      {
        if (_worker == null)
          AppUtilities.RegisterWorker();
        return _worker;
      }
      set { _worker = value; }
    }

    #endregion

    #region 方法
    
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

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
    private static string PackPermanentLog(string userNumber, Type objectType, string entityCaption, ExecuteAction action, IList<FieldValue> oldFieldValues, IList<FieldValue> newFieldValues)
    {
      int length = oldFieldValues != null ? oldFieldValues.Count : newFieldValues != null ? newFieldValues.Count : 0;
      if (length == 0)
        return null;
      StringBuilder result = new StringBuilder();
      bool find = action != ExecuteAction.Update;
      using (XmlWriter xmlWriter = XmlWriter.Create(result, new XmlWriterSettings { ConformanceLevel = ConformanceLevel.Fragment, CheckCharacters = false }))
      {
        xmlWriter.WriteStartElement("Date");
        xmlWriter.WriteAttributeString("Time", DateTime.Now.ToString("u"));
        xmlWriter.WriteEndElement();
        xmlWriter.WriteStartElement("User");
        xmlWriter.WriteAttributeString("Number", userNumber);
        xmlWriter.WriteEndElement();
        xmlWriter.WriteStartElement("Entity");
        xmlWriter.WriteAttributeString("Caption", entityCaption);
        xmlWriter.WriteEndElement();
        xmlWriter.WriteStartElement("Execute");
        xmlWriter.WriteAttributeString("Action", action.ToString());
        xmlWriter.WriteEndElement();
        for (int i = 0; i < length; i++)
        {
          FieldMapInfo fieldMapInfo = oldFieldValues != null ? oldFieldValues[i].GetFieldMapInfo(objectType) : newFieldValues != null ? newFieldValues[i].GetFieldMapInfo(objectType) : null;
          if (fieldMapInfo == null)
            continue;
          if (action == ExecuteAction.Update &&
            !fieldMapInfo.FieldAttribute.IsPrimaryKey)
          {
            if (oldFieldValues != null && newFieldValues != null)
            {
              ExecuteModify executeModify = fieldMapInfo.PermanentExecuteModify;
              object oldValue = oldFieldValues[i].Value;
              object newValue = newFieldValues[i].Value;
              if (!(((executeModify & ExecuteModify.NulltoNonnull) == ExecuteModify.NulltoNonnull && oldValue == null && newValue != null) ||
              ((executeModify & ExecuteModify.NonnullToNull) == ExecuteModify.NonnullToNull && oldValue != null && newValue == null) ||
              ((executeModify & ExecuteModify.NonnullToNonnull) == ExecuteModify.NonnullToNonnull && oldValue != null && newValue != null && !object.Equals(oldValue, newValue))))
                continue;
            }
            find = true;
          }
          xmlWriter.WriteStartElement("Field");
          xmlWriter.WriteAttributeString("Name", fieldMapInfo.Field.Name);
          if (oldFieldValues != null)
            xmlWriter.WriteAttributeString("OldValue",
              oldFieldValues[i].Value == null ? Phenix.Core.Code.Converter.NullSymbolic : oldFieldValues[i].Value.ToString());
          else
            xmlWriter.WriteAttributeString("OldValue", Phenix.Core.Code.Converter.NullSymbolic);
          if (newFieldValues != null)
            xmlWriter.WriteAttributeString("NewValue",
              newFieldValues[i].Value == null ? Phenix.Core.Code.Converter.NullSymbolic : newFieldValues[i].Value.ToString());
          else
            xmlWriter.WriteAttributeString("NewValue", Phenix.Core.Code.Converter.NullSymbolic);
          xmlWriter.WriteEndElement();
        }
      }
      return find ? result.ToString() : null;
    }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2202:DoNotDisposeObjectsMultipleTimes")]
    private static List<ExecuteActionInfo> UnpackPermanentLog(Type objectType, string log)
    {
      IList<FieldMapInfo> fieldMapInfos = ClassMemberHelper.DoGetFieldMapInfos(objectType, true, true, true, false);
      List<ExecuteActionInfo> result = new List<ExecuteActionInfo>();
      if (fieldMapInfos.Count > 0)
      {
        using (StringReader stringReader = new StringReader(log))
        using (XmlReader xmlReader = XmlReader.Create(stringReader, new XmlReaderSettings { ConformanceLevel = ConformanceLevel.Fragment }))
        {
          DateTime time = DateTime.Now;
          string userNumber = null;
          string entityCaption = null;
          ExecuteAction action = ExecuteAction.None;
          while (xmlReader.Read())
            if (xmlReader.NodeType == XmlNodeType.Element)
            {
              if (xmlReader.LocalName == "Date")
                DateTime.TryParse(xmlReader.GetAttribute(0), out time);
              else if (xmlReader.LocalName == "User")
                userNumber = xmlReader.GetAttribute(0);
              else if (xmlReader.LocalName == "Entity")
                entityCaption = xmlReader.GetAttribute(0);
              else if (xmlReader.LocalName == "Execute")
                action = (ExecuteAction)Enum.Parse(typeof(ExecuteAction), xmlReader.GetAttribute(0) ?? ExecuteAction.None.ToString());
              else if (xmlReader.LocalName == "Field")
              {
                string name = xmlReader.GetAttribute(0);
                foreach (FieldMapInfo item in fieldMapInfos)
                  if (String.CompareOrdinal(item.Field.Name, name) == 0)
                  {
                    string oldValue = xmlReader.GetAttribute(1);
                    string newValue = xmlReader.GetAttribute(2);
                    result.Add(new ExecuteActionInfo(time, userNumber, entityCaption, action, item,
                      String.CompareOrdinal(oldValue, Phenix.Core.Code.Converter.NullSymbolic) == 0 ? null : Utilities.ChangeType(oldValue, item.FieldUnderlyingType),
                      String.CompareOrdinal(newValue, Phenix.Core.Code.Converter.NullSymbolic) == 0 ? null : Utilities.ChangeType(newValue, item.FieldUnderlyingType)));
                    break;
                  }
              }
            }
        }
      }
      return result;
    }

    /// <summary>
    /// 保存对象消息
    /// </summary>
    public static void Save(Type objectType, string message)
    {
      Save(UserIdentity.CurrentIdentity, objectType, message, null);
    }

    /// <summary>
    /// 保存对象消息
    /// </summary>
    public static void Save(Type objectType, string message, Exception error)
    {
      Save(UserIdentity.CurrentIdentity, objectType, message, error);
    }

    /// <summary>
    /// 保存对象消息
    /// </summary>
    public static void Save(IPrincipal user, Type objectType, string message)
    {
      Save(user != null ? user.Identity : null, objectType, message, null);
    }

    /// <summary>
    /// 保存对象消息
    /// </summary>
    public static void Save(IPrincipal user, Type objectType, string message, Exception error)
    {
      Save(user != null ? user.Identity : null, objectType, message, error);
    }

    /// <summary>
    /// 保存对象消息
    /// </summary>
    public static void Save(IIdentity identity, Type objectType, string message)
    {
      Save(identity, objectType, message, null);
    }

    /// <summary>
    /// 保存对象消息
    /// </summary>
    public static void Save(IIdentity identity, Type objectType, string message, Exception error)
    {
      if (objectType == null)
        throw new ArgumentNullException("objectType");
      Save(identity != null ? identity.UserNumber : AppConfig.UNKNOWN_VALUE, objectType.FullName, message, error);
    }

    /// <summary>
    /// 保存对象消息
    /// </summary>
    [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
    public static void Save(string userNumber, string typeName, string message, Exception error)
    {
      CheckActive();
      Worker.Save(userNumber, typeName, message, error);
    }

    /// <summary>
    /// 检索日志消息
    /// </summary>
    public static IList<EventLogInfo> Fetch(string userNumber, Type objectType, DateTime startTime, DateTime finishTime)
    {
      return Fetch(userNumber, objectType != null ? objectType.FullName : null, startTime, finishTime);
    }

    /// <summary>
    /// 检索日志消息
    /// </summary>
    [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
    public static IList<EventLogInfo> Fetch(string userNumber, string typeName, DateTime startTime, DateTime finishTime)
    {
      CheckActive();
      return Worker.Fetch(userNumber, typeName, startTime, finishTime);
    }

    /// <summary>
    /// 清除日志消息
    /// </summary>
    public static void Clear(string userNumber, Type objectType, DateTime startTime, DateTime finishTime)
    {
      Clear(userNumber, objectType != null ? objectType.FullName : null, startTime, finishTime);
    }

    /// <summary>
    /// 清除日志消息
    /// </summary>
    [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
    public static void Clear(string userNumber, string typeName, DateTime startTime, DateTime finishTime)
    {
      CheckActive();
      Worker.Clear(userNumber, typeName, startTime, finishTime);
    }

    /// <summary>
    /// 保存对象持久化的执行动作
    /// </summary>
    public static void SaveExecuteAction(Type objectType, string entityCaption, string primaryKey,
      ExecuteAction action, IList<FieldValue> oldFieldValues, IList<FieldValue> newFieldValues)
    {
      SaveExecuteAction(UserIdentity.CurrentIdentity, objectType, entityCaption, primaryKey, action, oldFieldValues, newFieldValues);
    }

    /// <summary>
    /// 保存对象持久化的执行动作
    /// </summary>
    public static void SaveExecuteAction(IPrincipal user, Type objectType, string entityCaption, string primaryKey,
      ExecuteAction action, IList<FieldValue> oldFieldValues, IList<FieldValue> newFieldValues)
    {
      SaveExecuteAction(user != null ? user.Identity : null, objectType, entityCaption, primaryKey, action, oldFieldValues, newFieldValues);
    }

    /// <summary>
    /// 保存对象持久化的执行动作
    /// </summary>
    public static void SaveExecuteAction(IIdentity identity, Type objectType, string entityCaption, string primaryKey,
      ExecuteAction action, IList<FieldValue> oldFieldValues, IList<FieldValue> newFieldValues)
    {
      string log = PackPermanentLog(identity != null ? identity.UserNumber : AppConfig.UNKNOWN_VALUE, objectType, entityCaption, action, oldFieldValues, newFieldValues);
      if (log != null)
        SaveExecuteAction(identity != null ? identity.UserNumber : AppConfig.UNKNOWN_VALUE, objectType.FullName, primaryKey, action, log);
    }

    /// <summary>
    /// 保存对象持久化的执行动作
    /// </summary>
    [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
    public static void SaveExecuteAction(string userNumber, string typeName, string primaryKey,
      ExecuteAction action, string log)
    {
      CheckActive();
      Worker.SaveExecuteAction(userNumber, typeName, primaryKey, action, log);
    }

    /// <summary>
    /// 检索对象执行动作
    /// </summary>
    [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
    public static IList<string> FetchExecuteAction(string typeName, string primaryKey)
    {
      CheckActive();
      return Worker.FetchExecuteAction(typeName, primaryKey);
    }

    /// <summary>
    /// 检索对象执行动作
    /// </summary>
    public static IList<ExecuteActionInfo> FetchExecuteAction(Type objectType, string primaryKey)
    {
      if (objectType == null)
        throw new ArgumentNullException("objectType");
      IList<string> logs = FetchExecuteAction(objectType.FullName, primaryKey);
      if (logs == null)
        return null;
      List<ExecuteActionInfo> result = new List<ExecuteActionInfo>(logs.Count);
      foreach (string s in logs)
        result.AddRange(UnpackPermanentLog(objectType, s));
      return result;
    }

    /// <summary>
    /// 检索对象执行动作
    /// </summary>
    [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
    public static IList<string> FetchExecuteAction(string userNumber, string typeName,
      ExecuteAction action, DateTime startTime, DateTime finishTime)
    {
      CheckActive();
      return Worker.FetchExecuteAction(userNumber, typeName, action, startTime, finishTime);
    }
    
    /// <summary>
    /// 检索对象执行动作
    /// </summary>
    public static IList<ExecuteActionInfo> FetchExecuteAction(string userNumber, Type objectType,
      ExecuteAction action, DateTime startTime, DateTime finishTime)
    {
      if (objectType == null)
        throw new ArgumentNullException("objectType");
      IList<string> logs = FetchExecuteAction(userNumber, objectType.FullName, action, startTime, finishTime);
      if (logs == null)
        return null;
      List<ExecuteActionInfo> result = new List<ExecuteActionInfo>(logs.Count);
      foreach (string s in logs)
        result.AddRange(UnpackPermanentLog(objectType, s));
      return result;
    }

    /// <summary>
    /// 清除对象执行动作
    /// </summary>
    [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
    public static void ClearExecuteAction(string userNumber, string typeName, 
      ExecuteAction action, DateTime startTime, DateTime finishTime)
    {
      CheckActive();
      Worker.ClearExecuteAction(userNumber, typeName, action, startTime, finishTime);
    }

    /// <summary>
    /// 清除对象执行动作
    /// </summary>
    public static void ClearExecuteAction(string userNumber, Type objectType, 
      ExecuteAction action, DateTime startTime, DateTime finishTime)
    {
      if (objectType == null)
        throw new ArgumentNullException("objectType");
      ClearExecuteAction(userNumber, objectType.FullName, action, startTime, finishTime);
    }

    #region 代入数据库事务

    /// <summary>
    /// 保存对象持久化的动态刷新
    /// </summary>
    public static void SaveRenovate(DbTransaction transaction, string tableName, ExecuteAction action, IList<FieldValue> fieldValues)
    {
      CheckActive();
      Worker.SaveRenovate(transaction, tableName, action, fieldValues);
    }

    #endregion

    #endregion
  }
}