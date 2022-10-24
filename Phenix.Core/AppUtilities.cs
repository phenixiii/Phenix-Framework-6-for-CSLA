using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using System.Threading;
using System.Windows.Forms;
using Phenix.Core.Dictionary;
using Phenix.Core.Mapping;
using Phenix.Core.Net;
using Phenix.Core.Operate;
using Phenix.Core.Reflection;
using Phenix.Core.Rule;

namespace Phenix.Core
{
  /// <summary>
  /// 应用系统工具集
  /// </summary>
  public static class AppUtilities
  {
    #region 方法

    #region Attribute

    /// <summary>
    /// 获取成员信息的标签
    /// </summary>
    /// <typeparam name="T">Attribute</typeparam>
    /// <param name="objectType">类</param>
    /// <returns>标签</returns>
    public static T GetFirstCustomAttribute<T>(Type objectType)
      where T : Attribute
    {
      objectType = Utilities.LoadType(objectType); //主要用于IDE环境

      Type type = objectType;
      while (type != null)
      {
        T result = (T)Attribute.GetCustomAttribute(type, typeof(T), false);
        if (result != null)
          return result;
        type = type.BaseType;
      }
      return null;
    }

    #endregion

    #region Exception

    /// <summary>
    /// 检索错误类型
    /// </summary>
    /// <param name="error">错误</param>
    public static T FindException<T>(Exception error)
      where T : Exception
    {
      if (error == null)
        return null;
      if (String.CompareOrdinal(error.GetType().FullName, typeof(T).FullName) == 0)
        return error as T;
      return FindException<T>(error.InnerException);
    }

    /// <summary>
    /// 取错误信息
    /// </summary>
    /// <param name="error">错误</param>
    public static string GetErrorMessage(Exception error)
    {
      if (error == null)
        return String.Empty;
      string result = AppConfig.Debugging 
        ? error.GetType().FullName + ": " + error.Message 
        : error.Message;
      string message = GetErrorMessage(error.InnerException);
      if (!String.IsNullOrEmpty(message))
        return result + " -> " + message;
      return result;
    }

    /// <summary>
    /// 取错误提示
    /// </summary>
    /// <param name="error">错误</param>
    /// <param name="ignoreErrorTypes">忽略错误类型</param>
    public static string GetErrorHint(Exception error, IList<Type> ignoreErrorTypes)
    {
      if (error == null)
        return String.Empty;
      string result = ignoreErrorTypes == null || !ignoreErrorTypes.Contains(error.GetType())
        ? AppConfig.Debugging
          ? error.GetType().FullName + ": " + error.Message
          : error.Message
        : String.Empty;
      string hint = GetErrorHint(error.InnerException, ignoreErrorTypes);
      if (!String.IsNullOrEmpty(hint))
        return result + AppConfig.CR_LF + hint;
      return result;
    }

    /// <summary>
    /// 取错误提示
    /// </summary>
    /// <param name="error">错误</param>
    /// <param name="ignoreErrorTypes">忽略错误类型</param>
    public static string GetErrorHint(Exception error, params Type[] ignoreErrorTypes)
    {
      return GetErrorHint(error, ignoreErrorTypes != null ? new List<Type>(ignoreErrorTypes) : null);
    }

    /// <summary>
    /// 是否致命的错误
    /// </summary>
    /// <param name="error">错误</param>
    public static bool IsFatal(Exception error)
    {
      while (error != null)
      {
        if ((error is OutOfMemoryException && !(error is InsufficientMemoryException)) ||
          error is ThreadAbortException ||
          error is AccessViolationException ||
          error is SEHException)
          return true;

        // These exceptions aren't themselves fatal, but since the CLR uses them to wrap other exceptions,
        // we want to check to see whether they've been used to wrap a fatal exception.  If so, then they
        // count as fatal.
        if (error is TypeInitializationException ||
          error is TargetInvocationException)
          error = error.InnerException;
        else
          break;
      }
      return false;
    }

    #endregion

    #region Process

    /// <summary>
    /// 进程数量
    /// </summary>
    /// <param name="processName">进程名</param>
    [EnvironmentPermission(SecurityAction.Demand, Unrestricted = true)]
    public static int ProcessCount(string processName)
    {
      try
      {
        return Process.GetProcessesByName(processName).Length;
      }
      catch (InvalidOperationException)
      {
        return -1;
      }
    }

    #endregion

    #region Register

    /// <summary>
    /// 注册程序集类
    /// </summary>
    /// <param name="type">类</param>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1809:AvoidExcessiveLocals")]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
    public static bool Register(Type type)
    {
      if (IsNotApplicationType(type))
        return false;
      if (type.IsEnum)
      {
        KeyCaptionAttribute keyCaptionAttribute = (KeyCaptionAttribute)Attribute.GetCustomAttribute(type, typeof(KeyCaptionAttribute));
        if (keyCaptionAttribute == null)
          return false;
        EnumKeyCaptionCollection enumKeyCaptionCollection = EnumKeyCaptionCollection.Fetch(type);
        DataDictionaryHub.AddAssemblyClassInfo(type, keyCaptionAttribute.FriendlyName, AssemblyClassType.Enum);
        List<string> names = new List<string>(enumKeyCaptionCollection.Count);
        List<string> captions = new List<string>(enumKeyCaptionCollection.Count);
        foreach (EnumKeyCaption item in enumKeyCaptionCollection)
        {
          names.Add(item.Value.ToString());
          captions.Add(item.Caption);
        }
        DataDictionaryHub.AddAssemblyClassPropertyInfos(type, names.ToArray(), captions.ToArray(), AssemblyClassType.Enum);
        return true;
      }

      if (!type.IsClass || type.IsAbstract || type.IsGenericType || type.IsCOMObject)
        return false;

      IDictionary<string, PropertyMapInfo> propertyMapInfos = ClassMemberHelper.DoGetPropertyMapInfos(type);
      if (propertyMapInfos.Count > 0)
      {
        KeyCaptionAttribute keyCaptionAttribute = (KeyCaptionAttribute)Attribute.GetCustomAttribute(type, typeof(KeyCaptionAttribute));
        if (keyCaptionAttribute != null)
          DataDictionaryHub.AddAssemblyClassInfo(type, keyCaptionAttribute.FriendlyName, AssemblyClassType.Ordinary);
        List<string> names = new List<string>(propertyMapInfos.Count);
        List<string> captions = new List<string>(propertyMapInfos.Count);
        foreach (KeyValuePair<string, PropertyMapInfo> kvp in propertyMapInfos)
        {
          names.Add(kvp.Value.PropertyName);
          captions.Add(kvp.Value.FriendlyName);
        }
        DataDictionaryHub.AddAssemblyClassPropertyInfos(type, names.ToArray(), captions.ToArray(), AssemblyClassType.Ordinary);
      }

      DataDictionaryAttribute attribute = (DataDictionaryAttribute)Attribute.GetCustomAttribute(type, typeof(DataDictionaryAttribute));
      if (attribute == null)
        return false;
      try
      {
        if (attribute.ClassType == AssemblyClassType.Form)
        {
          object obj = Activator.CreateInstance(type, true);
          try
          {
            Form form = obj as Form;
            if (form == null)
              throw new InvalidOperationException(String.Format("类{0}的定义为非窗体, 无法实例化窗体对象", type.FullName));
            form.Show();
            Application.DoEvents(); //触发窗体Shown事件从而执行到ExecuteAuthorizationEvent.HostShown()
          }
          finally
          {
            IDisposable disposable = obj as IDisposable;
            if (disposable != null)
              disposable.Dispose();
          }
        }
        else if (attribute.ClassType == AssemblyClassType.Business || attribute.ClassType == AssemblyClassType.Entity)
        {
          ClassMapInfo classMapInfo = ClassMemberHelper.DoGetClassMapInfo(type);
          if (classMapInfo != null)
            DataDictionaryHub.AddAssemblyClassInfo(type, classMapInfo.FriendlyName, classMapInfo.PermanentExecuteAction, classMapInfo.GroupNames.ToArray(), attribute.ClassType);
          else
            DataDictionaryHub.AddAssemblyClassInfo(type, ClassMemberHelper.GetFriendlyName(type), attribute.ClassType);
          IList<FieldMapInfo> fieldMapInfos = ClassMemberHelper.DoGetFieldMapInfos(type);
          List<string> names = new List<string>(fieldMapInfos.Count);
          List<string> captions = new List<string>(fieldMapInfos.Count);
          List<string> tableNames = new List<string>(fieldMapInfos.Count);
          List<string> columnNames = new List<string>(fieldMapInfos.Count);
          List<string> aliases = new List<string>(fieldMapInfos.Count);
          List<ExecuteModify> permanentExecuteModifies = new List<ExecuteModify>(fieldMapInfos.Count);
          foreach (FieldMapInfo item in fieldMapInfos)
          {
            if (item.FieldAttribute.NoRegister)
              continue;
            if (item.FieldAttribute.InAuthorization)
            {
              names.Add(item.PropertyName);
              captions.Add(item.FriendlyName);
              tableNames.Add(item.TableName);
              columnNames.Add(item.ColumnName);
              aliases.Add(item.Alias);
              permanentExecuteModifies.Add(item.PermanentExecuteModify);
            }
            if (item.IsBusinessCodeColumn && item.NeedSave && !item.ClassMapInfo.IsReadOnly)
            {
              BusinessCodeFormat businessCodeFormat = new BusinessCodeFormat(item, true);
              businessCodeFormat.Save();
              if (!businessCodeFormat.IsDefault)
              {
                businessCodeFormat = new BusinessCodeFormat(item, false);
                businessCodeFormat.Save();
              }
            }
          }
          DataDictionaryHub.AddAssemblyClassPropertyInfos(type, names.ToArray(), captions.ToArray(),
            tableNames.ToArray(), columnNames.ToArray(), aliases.ToArray(), permanentExecuteModifies.ToArray());
          IDictionary<string, MethodMapInfo> methodMapInfos = ClassMemberHelper.DoGetMethodMapInfos(type, false);
          names = new List<string>(methodMapInfos.Count);
          captions = new List<string>(methodMapInfos.Count);
          List<string> tags = new List<string>(methodMapInfos.Count);
          foreach (KeyValuePair<string, MethodMapInfo> kvp in methodMapInfos)
            if (kvp.Value.MethodAttribute.InAuthorization)
            {
              names.Add(kvp.Value.MethodName);
              captions.Add(kvp.Value.FriendlyName);
              tags.Add(kvp.Value.MethodAttribute.Tag);
            }
          if (names.Count > 0)
            DataDictionaryHub.AddAssemblyClassMethodInfos(type, names.ToArray(), captions.ToArray(), tags.ToArray());
        }
        else if (attribute.ClassType == AssemblyClassType.Businesses || attribute.ClassType == AssemblyClassType.EntityList)
        {
          ClassMapInfo classMapInfo = ClassMemberHelper.DoGetClassMapInfo(type);
          if (classMapInfo != null)
            DataDictionaryHub.AddAssemblyClassInfo(type, classMapInfo.FriendlyName, null, classMapInfo.GroupNames.ToArray(), attribute.ClassType);
          else
            DataDictionaryHub.AddAssemblyClassInfo(type, ClassMemberHelper.GetFriendlyName(type), attribute.ClassType);
          IDictionary<string, MethodMapInfo> methodMapInfos = ClassMemberHelper.DoGetMethodMapInfos(type, false);
          List<string> names = new List<string>(methodMapInfos.Count);
          List<string> captions = new List<string>(methodMapInfos.Count);
          List<string> tags = new List<string>(methodMapInfos.Count);
          foreach (KeyValuePair<string, MethodMapInfo> kvp in methodMapInfos)
            if (kvp.Value.MethodAttribute.InAuthorization)
            {
              names.Add(kvp.Value.MethodName);
              captions.Add(kvp.Value.FriendlyName);
              tags.Add(kvp.Value.MethodAttribute.Tag);
            }
          if (names.Count > 0)
            DataDictionaryHub.AddAssemblyClassMethodInfos(type, names.ToArray(), captions.ToArray(), tags.ToArray());
        }
        else if (attribute.ClassType == AssemblyClassType.Command || attribute.ClassType == AssemblyClassType.Service)
          DataDictionaryHub.AddAssemblyClassInfo(type, ClassMemberHelper.GetFriendlyName(type), attribute.ClassType);
        else if (attribute.ClassType == AssemblyClassType.ApiController)
        {
          DataDictionaryHub.AddAssemblyClassInfo(type, ClassMemberHelper.GetFriendlyName(type), attribute.ClassType);
          IDictionary<string, MethodMapInfo> methodMapInfos = ClassMemberHelper.DoGetMethodMapInfos(type, true);
          List<string> names = new List<string>(methodMapInfos.Count);
          List<string> captions = new List<string>(methodMapInfos.Count);
          List<string> tags = new List<string>(methodMapInfos.Count);
          foreach (KeyValuePair<string, MethodMapInfo> kvp in methodMapInfos)
            if (kvp.Value.MethodAttribute.InAuthorization)
            {
              names.Add(kvp.Value.MethodName);
              captions.Add(kvp.Value.FriendlyName);
              tags.Add(kvp.Value.MethodAttribute.Tag);
            }
          if (names.Count > 0)
            DataDictionaryHub.AddAssemblyClassMethodInfos(type, names.ToArray(), captions.ToArray(), tags.ToArray());
        }
        return true;
      }
      catch (Exception ex)
      {
        Phenix.Core.Log.EventLog.SaveLocal(MethodBase.GetCurrentMethod(), type.FullName, ex);
        throw;
      }
    }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
    internal static void RegisterWorker()
    {
      if (AppConfig.AutoMode || AppConfig.DesignDebugMode || NetConfig.ProxyType == ProxyType.Embedded)
        try
        {
          Assembly assembly = Assembly.LoadFrom(AppConfig.ServicesLibraryPath);
          Type type = assembly.GetType("Phenix.Services.Library.Registration", false);
          if (type != null)
            type.InvokeMember("RegisterWorker", BindingFlags.InvokeMethod | BindingFlags.Public | BindingFlags.Static, null, null, null);
        }
        catch (Exception ex)
        {
          Phenix.Core.Log.EventLog.SaveLocal(AppConfig.ServicesLibraryPath, ex);
        }
    }

    #endregion

    /// <summary>
    /// 非应用系统类
    /// </summary>
    public static bool IsNotApplicationAssembly(Assembly assembly)
    {
      if (assembly == null)
        return true;
      string assemblyName = assembly.GetName().Name;
      return assembly is AssemblyBuilder ||
        String.CompareOrdinal(assemblyName, "ChnCharInfo") == 0 ||
        String.CompareOrdinal(assemblyName, "stdole") == 0 ||
        assemblyName.IndexOf("VSLangProj", StringComparison.Ordinal) == 0 ||
        assemblyName.IndexOf("Microsoft.", StringComparison.Ordinal) == 0 ||
        assemblyName.IndexOf("EnvDTE", StringComparison.Ordinal) == 0 ||
        String.CompareOrdinal(assemblyName, "Extensibility") == 0 ||
        String.CompareOrdinal(assemblyName, "System") == 0 || assemblyName.IndexOf("System.", StringComparison.Ordinal) == 0 ||
        assemblyName.IndexOf("Newtonsoft.", StringComparison.Ordinal) == 0 ||
        assemblyName.IndexOf("DevExpress.", StringComparison.Ordinal) == 0 ||
        assemblyName.IndexOf("ALAZ.", StringComparison.Ordinal) == 0 ||
        String.CompareOrdinal(assemblyName, "Csla") == 0 || assemblyName.IndexOf("Csla.", StringComparison.Ordinal) == 0;
    }

    /// <summary>
    /// 非应用系统类
    /// </summary>
    public static bool IsNotApplicationType(Type type)
    {
      if (type == null)
        return true;
      string typeNamespace = type.Namespace;
      return String.IsNullOrEmpty(typeNamespace) ||
        typeNamespace.IndexOf("Microsoft.", StringComparison.Ordinal) == 0 ||
        typeNamespace.IndexOf("EnvDTE", StringComparison.Ordinal) == 0 ||
        String.CompareOrdinal(typeNamespace, "Extensibility") == 0 || typeNamespace.IndexOf("Extensibility.", StringComparison.Ordinal) == 0 ||
        String.CompareOrdinal(typeNamespace, "System") == 0 || typeNamespace.IndexOf("System.", StringComparison.Ordinal) == 0 ||
        typeNamespace.IndexOf("Newtonsoft.", StringComparison.Ordinal) == 0 ||
        typeNamespace.IndexOf("DevExpress.", StringComparison.Ordinal) == 0 ||
        typeNamespace.IndexOf("ALAZ.", StringComparison.Ordinal) == 0 ||
        String.CompareOrdinal(typeNamespace, "Csla") == 0 || typeNamespace.IndexOf("Csla.", StringComparison.Ordinal) == 0;
    }

    /// <summary>
    /// 清理临时目录
    /// </summary>
    public static void ClearTempDirectory()
    {
      string directory = Path.Combine(AppConfig.BaseDirectory, AppConfig.TempDirectory);
      if (Directory.Exists(directory))
        Directory.Delete(directory, true);
    }

    #endregion
  }
}
