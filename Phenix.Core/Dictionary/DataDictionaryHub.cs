using System;
using System.Collections.Generic;
using System.Reflection;
using Phenix.Core.Data.Schema;
using Phenix.Core.Log;
using Phenix.Core.Mapping;
using Phenix.Core.Reflection;
using Phenix.Core.Rule;
using Phenix.Core.Security;
using Phenix.Core.SyncCollections;

namespace Phenix.Core.Dictionary
{
  /// <summary>
  /// 数据字典中心
  /// </summary>
  public static class DataDictionaryHub
  {
    #region 属性

    private static IDataDictionary _worker;
    /// <summary>
    /// 实施者
    /// </summary>
    public static IDataDictionary Worker
    {
      get
      {
        if (_worker == null)
        {
          OnMessageNotify(new MessageNotifyEventArgs(MessageNotifyType.Information, typeof(DataDictionaryHub).Name, MethodBase.GetCurrentMethod().Name));
          AppUtilities.RegisterWorker();
        }
        return _worker;
      }
      set { _worker = value; }
    }

    private static string _enterprise;
    /// <summary>
    /// 企业
    /// </summary>
    public static string Enterprise
    {
      get
      {
        if (_enterprise == null)
        {
          OnMessageNotify(new MessageNotifyEventArgs(MessageNotifyType.Information, typeof(DataDictionaryHub).Name, MethodBase.GetCurrentMethod().Name));
          CheckActive();
          _enterprise = Worker.Enterprise;
        }
        return _enterprise;
      }
    }

    private static readonly object _departmentInfosLock = new object();
    private static IDictionary<long, DepartmentInfo> _departmentInfos;
    /// <summary>
    /// 部门资料队列
    /// </summary>
    public static IDictionary<long, DepartmentInfo> DepartmentInfos
    {
      get
      {
        if (_departmentInfos == null)
          lock (_departmentInfosLock)
            if (_departmentInfos == null)
            {
              OnMessageNotify(new MessageNotifyEventArgs(MessageNotifyType.Information, typeof(DataDictionaryHub).Name, MethodBase.GetCurrentMethod().Name));
              CheckActive();
              _departmentInfos = Worker.DepartmentInfos;
            }
        return _departmentInfos;
      }
    }

    private static readonly object _positionInfosLock = new object();
    private static IDictionary<long, PositionInfo> _positionInfos;
    /// <summary>
    /// 岗位资料队列
    /// </summary>
    public static IDictionary<long, PositionInfo> PositionInfos
    {
      get
      {
        if (_positionInfos == null)
          lock (_positionInfosLock)
            if (_positionInfos == null)
            {
              OnMessageNotify(new MessageNotifyEventArgs(MessageNotifyType.Information, typeof(DataDictionaryHub).Name, MethodBase.GetCurrentMethod().Name));
              CheckActive();
              _positionInfos = Worker.PositionInfos;
            }
        return _positionInfos;
      }
    }

    private static readonly SynchronizedDictionary<string, AssemblyInfo> _assemblyInfos =
      new SynchronizedDictionary<string, AssemblyInfo>(StringComparer.OrdinalIgnoreCase);

    private static readonly object _tableFilterInfosLock = new object();
    private static IDictionary<string, TableFilterInfo> _tableFilterInfos;
    /// <summary>
    /// 表过滤器资料队列
    /// </summary>
    public static IDictionary<string, TableFilterInfo> TableFilterInfos
    {
      get
      {
        if (_tableFilterInfos == null)
          lock (_tableFilterInfosLock)
            if (_tableFilterInfos == null)
            {
              OnMessageNotify(new MessageNotifyEventArgs(MessageNotifyType.Information, typeof(DataDictionaryHub).Name, MethodBase.GetCurrentMethod().Name));
              CheckActive();
              _tableFilterInfos = Worker.TableFilterInfos;
            }
        return _tableFilterInfos;
      }
    }

    private static readonly object _roleInfosLock = new object();
    private static IDictionary<string, RoleInfo> _roleInfos;
    /// <summary>
    /// 角色资料队列
    /// </summary>
    public static IDictionary<string, RoleInfo> RoleInfos
    {
      get
      {
        if (_roleInfos == null)
          lock (_roleInfosLock)
            if (_roleInfos == null)
            {
              OnMessageNotify(new MessageNotifyEventArgs(MessageNotifyType.Information, typeof(DataDictionaryHub).Name, MethodBase.GetCurrentMethod().Name));
              CheckActive();
              _roleInfos = Worker.RoleInfos;
            }
        return _roleInfos;
      }
    }

    private static readonly object _sectionInfosLock = new object();
    private static IDictionary<string, SectionInfo> _sectionInfos;
    /// <summary>
    /// 切片资料队列
    /// </summary>
    public static IDictionary<string, SectionInfo> SectionInfos
    {
      get
      {
        if (_sectionInfos == null)
          lock (_sectionInfosLock)
            if (_sectionInfos == null)
            {
              OnMessageNotify(new MessageNotifyEventArgs(MessageNotifyType.Information, typeof(DataDictionaryHub).Name, MethodBase.GetCurrentMethod().Name));
              CheckActive();
              _sectionInfos = Worker.SectionInfos;
            }
        return _sectionInfos;
      }
    }

    #region BusinessCode

    /// <summary>
    /// 业务码格式队列
    /// </summary>
    public static IDictionary<string, BusinessCodeFormat> BusinessCodeFormats
    {
      get
      {
        OnMessageNotify(new MessageNotifyEventArgs(MessageNotifyType.Information, typeof(DataDictionaryHub).Name, MethodBase.GetCurrentMethod().Name));
        CheckActive();
        return Worker.BusinessCodeFormats;
      }
    }

    #endregion

    #endregion

    #region 事件

    /// <summary>
    /// 消息通报事件
    /// </summary>
    public static event EventHandler<MessageNotifyEventArgs> MessageNotify;
    private static void OnMessageNotify(MessageNotifyEventArgs e)
    {
      EventHandler<MessageNotifyEventArgs> handler = MessageNotify;
      if (handler != null)
        handler.Invoke(null, e);
    }

    #endregion

    #region 方法

    /// <summary>
    /// 清理缓存
    /// </summary>
    public static void ClearCache()
    {
      OnMessageNotify(new MessageNotifyEventArgs(MessageNotifyType.Information, typeof(DataDictionaryHub).Name, MethodBase.GetCurrentMethod().Name));
      
      lock (_departmentInfosLock)
      {
        _departmentInfos = null;
      }
      lock (_positionInfosLock)
      {
        _positionInfos = null;
      }
      _assemblyInfos.Clear();
      lock (_tableFilterInfosLock)
      {
        _tableFilterInfos = null;
      }
      lock (_roleInfosLock)
      {
        _roleInfos = null;
      }
      lock (_sectionInfosLock)
      {
        _sectionInfos = null;
      }

      ClassMemberHelper.ClearCache();
      Mapper.ClearCache();
      SectionInfo.ClearCache();
      DataRuleHub.ClearCache();
    }

    /// <summary>
    /// 检查活动
    /// </summary>
    public static void CheckActive()
    {
      if (Worker == null)
      {
        Exception ex = new InvalidOperationException(Phenix.Core.Properties.Resources.NoService);
        OnMessageNotify(new MessageNotifyEventArgs(MessageNotifyType.Error, typeof(DataDictionaryHub).Name, ex));
        EventLog.SaveLocal(MethodBase.GetCurrentMethod(), ex);
        throw ex;
      }
    }

    /// <summary>
    /// 所有资料已更新
    /// </summary>
    public static void AllInfoHasChanged()
    {
      OnMessageNotify(new MessageNotifyEventArgs(MessageNotifyType.Information, typeof(DataDictionaryHub).Name, MethodBase.GetCurrentMethod().Name));
      CheckActive();
      Worker.DepartmentInfoHasChanged();
      Worker.PositionInfoHasChanged();
      Worker.AssemblyInfoHasChanged();
      Worker.TableInfoHasChanged();
      Worker.TableFilterInfoHasChanged();
      Worker.RoleInfoHasChanged();
      Worker.SectionInfoHasChanged();
      ClearCache();
    }

    #region DepartmentInfo

    /// <summary>
    /// 部门资料已更新
    /// </summary>
    public static void DepartmentInfoHasChanged()
    {
      OnMessageNotify(new MessageNotifyEventArgs(MessageNotifyType.Information, typeof(DataDictionaryHub).Name, MethodBase.GetCurrentMethod().Name));
      CheckActive();
      Worker.DepartmentInfoHasChanged();
      ClearCache();
    }

    #endregion

    #region PositionInfo

    /// <summary>
    /// 岗位资料已更新
    /// </summary>
    public static void PositionInfoHasChanged()
    {
      OnMessageNotify(new MessageNotifyEventArgs(MessageNotifyType.Information, typeof(DataDictionaryHub).Name, MethodBase.GetCurrentMethod().Name));
      CheckActive();
      Worker.PositionInfoHasChanged();
      ClearCache();
    }

    #endregion

    #region AssemblyInfo

    /// <summary>
    /// 程序集资料已更新
    /// </summary>
    public static void AssemblyInfoHasChanged()
    {
      OnMessageNotify(new MessageNotifyEventArgs(MessageNotifyType.Information, typeof(DataDictionaryHub).Name, MethodBase.GetCurrentMethod().Name));
      CheckActive();
      Worker.AssemblyInfoHasChanged();
      ClearCache();
    }

    /// <summary>
    /// 取程序集资料
    /// </summary>
    public static IDictionary<string, AssemblyInfo> GetAssemblyInfos(Action<MessageNotifyEventArgs> messageNotify)
    {
        IDictionary<string, AssemblyInfo> result = Worker.GetAssemblyInfos();
        foreach (KeyValuePair<string, AssemblyInfo> kvp in result)
        {
            _assemblyInfos[kvp.Key] = kvp.Value;
            if (messageNotify != null)
                messageNotify(new MessageNotifyEventArgs(MessageNotifyType.Information, typeof(DataDictionaryHub).Name, String.Format("{0} - {1}", MethodBase.GetCurrentMethod().Name, kvp.Key)));
        }

        return result;
    }

    /// <summary>
    /// 取程序集资料
    /// </summary>
    public static IDictionary<string, AssemblyInfo> GetAssemblyInfos()
    {
        IDictionary<string, AssemblyInfo> result = Worker.GetAssemblyInfos();
        foreach (KeyValuePair<string, AssemblyInfo> kvp in result)
        {
            _assemblyInfos[kvp.Key] = kvp.Value;
            OnMessageNotify(new MessageNotifyEventArgs(MessageNotifyType.Information, typeof(DataDictionaryHub).Name, String.Format("{0} - {1}", MethodBase.GetCurrentMethod().Name, kvp.Key)));
        }

        return result;
    }

    /// <summary>
    /// 取程序集资料
    /// </summary>
    /// <param name="assemblyName">程序集名或类全名</param>
    public static AssemblyInfo GetAssemblyInfo(string assemblyName)
    {
      if (String.IsNullOrEmpty(assemblyName))
        return null;
      return _assemblyInfos.GetValue(assemblyName, () =>
      {
        OnMessageNotify(new MessageNotifyEventArgs(MessageNotifyType.Information, typeof(DataDictionaryHub).Name, String.Format("{0} - {1}", MethodBase.GetCurrentMethod().Name, assemblyName)));
        CheckActive();
        return Worker.GetAssemblyInfo(assemblyName);
      }, false);
    }

    #region ClassInfo

    /// <summary>
    /// 取类资料
    /// </summary>
    public static AssemblyClassInfo GetClassInfo(Type objectType)
    {
      return GetClassInfo(objectType, false);
    }

    /// <summary>
    /// 取类资料
    /// </summary>
    public static AssemblyClassInfo GetClassInfo(Type objectType, bool autoRegister)
    {
      if (objectType == null)
        return null;
      AssemblyClassInfo result = GetClassInfo(objectType.Assembly.GetName().Name, objectType.FullName);
      if (result == null && autoRegister)
        if (AppUtilities.Register(objectType))
          return GetClassInfo(objectType, false);
      return result;
    }

    private static AssemblyClassInfo GetClassInfo(string assemblyName, string className)
    {
      if (String.IsNullOrEmpty(assemblyName))
        return null;
      AssemblyInfo assemblyInfo = GetAssemblyInfo(assemblyName);
      if (assemblyInfo != null)
        return assemblyInfo.GetClassInfo(className);
      return null;
    }

    #endregion

    #endregion

    #region TableInfo

    /// <summary>
    /// 表结构已更新
    /// </summary>
    public static void TableInfoHasChanged()
    {
      OnMessageNotify(new MessageNotifyEventArgs(MessageNotifyType.Information, typeof(DataDictionaryHub).Name, MethodBase.GetCurrentMethod().Name));
      CheckActive();
      Worker.TableInfoHasChanged();
      ClearCache();
    }

    #endregion

    #region TableFilterInfo

    /// <summary>
    /// 表过滤器资料已更新
    /// </summary>
    public static void TableFilterInfoHasChanged()
    {
      OnMessageNotify(new MessageNotifyEventArgs(MessageNotifyType.Information, typeof(DataDictionaryHub).Name, MethodBase.GetCurrentMethod().Name));
      CheckActive();
      Worker.TableFilterInfoHasChanged();
      ClearCache();
    }

    /// <summary>
    /// 取表过滤器切片资料
    /// </summary>
    public static TableFilterInfo GetTableFilterInfo(string fullTableColumnName)
    {
      if (!String.IsNullOrEmpty(fullTableColumnName))
      {
        TableFilterInfo result;
        if (TableFilterInfos.TryGetValue(fullTableColumnName, out result))
          return result;
      }
      return null;
    }

    #endregion

    #region RoleInfo

    /// <summary>
    /// 角色资料已更新
    /// </summary>
    public static void RoleInfoHasChanged()
    {
      OnMessageNotify(new MessageNotifyEventArgs(MessageNotifyType.Information, typeof(DataDictionaryHub).Name, MethodBase.GetCurrentMethod().Name));
      CheckActive();
      Worker.RoleInfoHasChanged();
      ClearCache();
    }

    /// <summary>
    /// 反选角色资料队列
    /// </summary>
    public static IList<string> InverseRoles(IList<string> roles)
    {
      if (roles == null)
        return null;
      List<string> result = new List<string>(RoleInfos.Count);
      foreach (KeyValuePair<string, RoleInfo> kvp in RoleInfos)
        if (!roles.Contains(kvp.Value.Name))
          result.Add(kvp.Value.Name);
      return result.AsReadOnly();
    }

    #endregion

    #region SectionInfo

    /// <summary>
    /// 切片资料已更新
    /// </summary>
    public static void SectionInfoHasChanged()
    {
      OnMessageNotify(new MessageNotifyEventArgs(MessageNotifyType.Information, typeof(DataDictionaryHub).Name, MethodBase.GetCurrentMethod().Name));
      CheckActive();
      Worker.SectionInfoHasChanged();
      ClearCache();
    }

    #endregion

    #region AddAssemblyClassInfo

    /// <summary>
    /// 新增程序集类资料
    /// </summary>
    [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
    public static void AddAssemblyClassInfo(string assemblyName, string assemblyCaption, string className, string classCaption, ExecuteAction? permanentExecuteAction, string[] groupNames, AssemblyClassType classType)
    {
      try
      {
        OnMessageNotify(new MessageNotifyEventArgs(MessageNotifyType.Information, typeof(DataDictionaryHub).Name, String.Format("{0} - {1}.{2}", MethodBase.GetCurrentMethod().Name, assemblyName, className)));
        CheckActive();
        Worker.AddAssemblyClassInfo(assemblyName, assemblyCaption, className, classCaption, permanentExecuteAction, groupNames, classType);
      }
      finally
      {
        AssemblyInfoHasChanged();
      }
    }

    /// <summary>
    /// 新增程序集类资料
    /// </summary>
    public static void AddAssemblyClassInfo(Type objectType, string caption, AssemblyClassType classType)
    {
      if (objectType == null)
        throw new ArgumentNullException("objectType");
      AssemblyDescriptionAttribute assemblyDescriptionAttribute = (AssemblyDescriptionAttribute)Attribute.GetCustomAttribute(objectType.Assembly, typeof(AssemblyDescriptionAttribute));
      AddAssemblyClassInfo(objectType.Assembly.GetName().Name, assemblyDescriptionAttribute != null ? assemblyDescriptionAttribute.Description : null, objectType.FullName, caption, null, null, classType);
    }

    /// <summary>
    /// 新增程序集类资料
    /// </summary>
    public static void AddAssemblyClassInfo(Type objectType, string caption, ExecuteAction? permanentExecuteAction, string[] groupNames, AssemblyClassType classType)
    {
      if (objectType == null)
        throw new ArgumentNullException("objectType");
      AssemblyDescriptionAttribute assemblyDescriptionAttribute = (AssemblyDescriptionAttribute)Attribute.GetCustomAttribute(objectType.Assembly, typeof(AssemblyDescriptionAttribute));
      AddAssemblyClassInfo(objectType.Assembly.GetName().Name, assemblyDescriptionAttribute != null ? assemblyDescriptionAttribute.Description : null, objectType.FullName, caption, permanentExecuteAction, groupNames, classType);
    }

    #endregion

    #region AddAssemblyClassPropertyInfos

    /// <summary>
    /// 新增程序集类属性资料
    /// </summary>
    [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
    public static void AddAssemblyClassPropertyInfos(string assemblyName, string className, string[] names, string[] captions,
      string[] tableNames, string[] columnNames, string[] aliases, ExecuteModify[] permanentExecuteModifies)
    {
      try
      {
        OnMessageNotify(new MessageNotifyEventArgs(MessageNotifyType.Information, typeof(DataDictionaryHub).Name, String.Format("{0} - {1}.{2}", MethodBase.GetCurrentMethod().Name, assemblyName, className)));
        CheckActive();
        Worker.AddAssemblyClassPropertyInfos(assemblyName, className, names, captions, tableNames, columnNames, aliases, permanentExecuteModifies);
      }
      finally
      {
        AssemblyInfoHasChanged();
      }
    }

    /// <summary>
    /// 新增程序集类属性资料
    /// </summary>
    public static void AddAssemblyClassPropertyInfos(Type objectType, string[] names, string[] captions,
      string[] tableNames, string[] columnNames, string[] aliases, ExecuteModify[] permanentExecuteModifies)
    {
      if (objectType == null)
        throw new ArgumentNullException("objectType");
      AddAssemblyClassPropertyInfos(objectType.Assembly.GetName().Name, objectType.FullName, names, captions, tableNames, columnNames, aliases, permanentExecuteModifies);
    }

    /// <summary>
    /// 新增程序集类属性资料
    /// </summary>
    [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
    public static void AddAssemblyClassPropertyInfos(string assemblyName, string className, string[] names, string[] captions,
      bool[] configurables, string[] configKeys, string[] configValues, AssemblyClassType classType)
    {
      try
      {
        OnMessageNotify(new MessageNotifyEventArgs(MessageNotifyType.Information, typeof(DataDictionaryHub).Name, String.Format("{0} - {1}.{2}", MethodBase.GetCurrentMethod().Name, assemblyName, className)));
        CheckActive();
        Worker.AddAssemblyClassPropertyConfigInfos(assemblyName, className, names, captions, configurables, configKeys, configValues, classType);
      }
      finally
      {
        AssemblyInfoHasChanged();
      }
    }

    /// <summary>
    /// 新增程序集类属性资料
    /// </summary>
    public static void AddAssemblyClassPropertyInfos(Type objectType, string[] names, string[] captions,
      AssemblyClassType classType)
    {
      if (objectType == null)
        throw new ArgumentNullException("objectType");
      AddAssemblyClassPropertyInfos(objectType.Assembly.GetName().Name, objectType.FullName, names, captions, null, null, null, classType);
    }

    /// <summary>
    /// 新增程序集类属性资料
    /// </summary>
    public static void AddAssemblyClassPropertyInfo(Type objectType, string name, string caption,
      bool configurable, string configKey, string configValue, AssemblyClassType classType)
    {
      if (objectType == null)
        throw new ArgumentNullException("objectType");
      AddAssemblyClassPropertyInfos(objectType.Assembly.GetName().Name, objectType.FullName, new string[] { name }, new string[] { caption },
        new bool[] { configurable }, new string[] { configKey }, new string[] { configValue }, classType);
    }

    #endregion

    #region AddAssemblyClassMethodInfos

    /// <summary>
    /// 新增程序集类方法资料
    /// </summary>
    [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
    public static void AddAssemblyClassMethodInfos(string assemblyName, string className, string[] names, string[] captions, string[] tags, bool[] allowVisibles)
    {
      try
      {
        OnMessageNotify(new MessageNotifyEventArgs(MessageNotifyType.Information, typeof(DataDictionaryHub).Name, String.Format("{0} - {1}.{2}", MethodBase.GetCurrentMethod().Name, assemblyName, className)));
        CheckActive();
        Worker.AddAssemblyClassMethodInfos(assemblyName, className, names, captions, tags, allowVisibles);
      }
      finally
      {
        AssemblyInfoHasChanged();
      }
    }

    /// <summary>
    /// 新增程序集类方法资料
    /// </summary>
    public static void AddAssemblyClassMethodInfos(Type objectType, string[] names, string[] captions, string[] tags)
    {
      if (objectType == null)
        throw new ArgumentNullException("objectType");
      AddAssemblyClassMethodInfos(objectType.Assembly.GetName().Name, objectType.FullName, names, captions, tags, null);
    }

    /// <summary>
    /// 新增程序集类方法资料
    /// </summary>
    public static void AddAssemblyClassMethodInfos(Type objectType, string[] names, string[] captions, string[] tags, bool[] allowVisibles)
    {
      if (objectType == null)
        throw new ArgumentNullException("objectType");
      AddAssemblyClassMethodInfos(objectType.Assembly.GetName().Name, objectType.FullName, names, captions, tags, allowVisibles);
    }

    #endregion

    #region BusinessCode

    /// <summary>
    /// 获取业务码格式
    /// </summary>
    public static BusinessCodeFormat GetBusinessCodeFormat(string businessCodeName)
    {
      OnMessageNotify(new MessageNotifyEventArgs(MessageNotifyType.Information, typeof(DataDictionaryHub).Name, String.Format("{0} - {1}", MethodBase.GetCurrentMethod().Name, businessCodeName)));
      CheckActive();
      return Worker.GetBusinessCodeFormat(businessCodeName);
    }

    /// <summary>
    /// 设置业务码格式
    /// </summary>
    public static void SetBusinessCodeFormat(BusinessCodeFormat format)
    {
      OnMessageNotify(new MessageNotifyEventArgs(MessageNotifyType.Information, typeof(DataDictionaryHub).Name, String.Format("{0} - {1}", MethodBase.GetCurrentMethod().Name, format.BusinessCodeName)));
      CheckActive();
      Worker.SetBusinessCodeFormat(format);
    }

    /// <summary>
    /// 移除业务码格式
    /// </summary>
    public static void RemoveBusinessCodeFormat(BusinessCodeFormat format)
    {
      RemoveBusinessCodeFormat(format.BusinessCodeName);
    }

    /// <summary>
    /// 移除业务码格式
    /// </summary>
    public static void RemoveBusinessCodeFormat(string businessCodeName)
    {
      OnMessageNotify(new MessageNotifyEventArgs(MessageNotifyType.Information, typeof(DataDictionaryHub).Name, String.Format("{0} - {1}", MethodBase.GetCurrentMethod().Name, businessCodeName)));
      CheckActive();
      Worker.RemoveBusinessCodeFormat(businessCodeName);
    }

    #endregion

    #endregion
  }
}