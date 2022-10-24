using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml.Serialization;
using Newtonsoft.Json;
using Phenix.Core.Data;
using Phenix.Core.Dictionary;
using Phenix.Core.IO;
using Phenix.Core.Log;
using Phenix.Core.SyncCollections;

namespace Phenix.Core.Reflection
{
  /// <summary>
  /// 工具集
  /// </summary>
  public static class Utilities
  {
    #region 属性

    private static readonly SynchronizedDictionary<string, Type> _typeCache =
      new SynchronizedDictionary<string, Type>(StringComparer.Ordinal);
    private static readonly SynchronizedDictionary<string, IList<FieldInfo>> _instanceFieldsCache =
      new SynchronizedDictionary<string, IList<FieldInfo>>(StringComparer.Ordinal);
    private static readonly SynchronizedDictionary<string, IList<PropertyInfo>> _instancePropertiesCache =
      new SynchronizedDictionary<string, IList<PropertyInfo>>(StringComparer.Ordinal);

    #endregion

    #region 方法

    #region Assembly

    /// <summary>
    /// 加载程序集
    /// </summary>
    /// <param name="assemblyName">程序集名</param>
    /// <returns>程序集</returns>
    public static Assembly LoadAssembly(string assemblyName)
    {
      try
      {
        return AppDomain.CurrentDomain.Load(assemblyName);
      }
      catch (FileNotFoundException ex)
      {
        if (AppConfig.Debugging)
          EventLog.SaveLocal(MethodBase.GetCurrentMethod(), assemblyName, ex);
        return null;
      }
    }

    #endregion

    #region TypeName

    /// <summary>
    /// 拼装完整类名 = 命名空间.类名
    /// 当typeName中存在命名空间时返回值不变
    /// </summary>
    /// <param name="typeNamespace">命名空间</param>
    /// <param name="typeName">类名</param>
    public static string AssembleFullTypeName(string typeNamespace, string typeName)
    {
      if (typeName != null)
      {
        int index = typeName.IndexOf('.');
        if (index == -1 && !String.IsNullOrEmpty(typeNamespace))
          return String.Format("{0}.{1}", typeNamespace, typeName);
      }
      return typeName;
    }

    /// <summary>
    /// 抽取命名空间
    /// </summary>
    /// <param name="fullTypeName">完整类名 </param>
    public static string ExtractTypeNamespace(string fullTypeName)
    {
      if (fullTypeName == null)
        return null;
      string result = null;
      if (IsTypeName(fullTypeName))
      {
        string temp = fullTypeName;
        int index = temp.IndexOf('.');
        while (index != -1)
        {
          result = result != null ? String.Format("{0}.{1}", result, temp.Substring(0, index)) : temp.Substring(0, index);
          temp = temp.Substring(index + 1);
          index = temp.IndexOf('.');
        }
      }
      return result;
    }

    /// <summary>
    /// 抽取类名
    /// </summary>
    /// <param name="fullTypeName">完整类名 </param>
    public static string ExtractTypeName(string fullTypeName)
    {
      if (fullTypeName == null)
        return null;
      string result = fullTypeName;
      if (IsTypeName(fullTypeName))
      {
        int index = result.IndexOf('.');
        while (index != -1)
        {
          result = result.Substring(index + 1);
          index = result.IndexOf('.');
        }
      }
      return result;
    }

    private static bool IsTypeName(string typeName)
    {
      if (String.IsNullOrEmpty(typeName))
        return false;
      for (int i = 0; i < typeName.Length; i++)
        if (!Char.IsLetterOrDigit(typeName[i]) && typeName[i] != '.' && typeName[i] != '_')
          return false;
      return true;
    }

    #endregion

    #region Type

    /// <summary>
    /// 从类型集合中摘取派生类型
    /// </summary>
    ///<param name="sourceTypes">类型集合</param>
    ///<param name="baseTypes">基础类型、Interface</param>
    ///<returns>类型队列</returns>
    public static IList<Type> GetExportedSubclassTypes(IList<Type> sourceTypes, params Type[] baseTypes)
    {
      if (baseTypes == null || baseTypes.Length == 0)
        return new List<Type>(0);
      List<Type> result = new List<Type>(sourceTypes.Count);
      foreach (Type sourceType in sourceTypes)
        foreach (Type baseType in baseTypes)
          if (baseType.IsInterface && baseType.IsAssignableFrom(sourceType) || 
            sourceType.IsSubclassOf(baseType))
            result.Add(sourceType);
      return result;
    }
    
    /// <summary>
    /// 从基础目录中加载派生类型
    /// includeAbstract = false
    /// </summary>
    /// <param name="baseTypes">基础类型、Interface</param>
    ///<returns>类型队列</returns>
    public static IList<Type> LoadExportedSubclassTypesFromBaseDirectory(params Type[] baseTypes)
    {
      return LoadExportedSubclassTypesFromBaseDirectory(false, baseTypes);
    }

    /// <summary>
    /// 从基础目录中加载派生类型
    /// </summary>
    /// <param name="baseTypes">基础类型、Interface</param>
    /// <param name="includeAbstract">是否包括抽象类型</param>
    /// <returns>类型队列</returns>
    public static IList<Type> LoadExportedSubclassTypesFromBaseDirectory(bool includeAbstract, params Type[] baseTypes)
    {
      return GetExportedSubclassTypes(LoadExportedClassTypesFromBaseDirectory(includeAbstract), baseTypes);
    }

    /// <summary>
    /// 加载派生类型
    /// includeAbstract = false
    /// </summary>
    /// <param name="fileName">程序集文件名</param>
    /// <param name="baseTypes">基础类型、Interface</param>
    ///<returns>类型队列</returns>
    public static IList<Type> LoadExportedSubclassTypes(string fileName, params Type[] baseTypes)
    {
      return LoadExportedSubclassTypes(fileName, false, baseTypes);
    }

    /// <summary>
    /// 加载派生类型
    /// </summary>
    /// <param name="fileName">程序集文件名</param>
    /// <param name="includeAbstract">是否包括抽象类型</param>
    /// <param name="baseTypes">基础类型、Interface</param>
    /// <returns>类型队列</returns>
    public static IList<Type> LoadExportedSubclassTypes(string fileName, bool includeAbstract, params Type[] baseTypes)
    {
      return GetExportedSubclassTypes(LoadExportedClassTypes(fileName, includeAbstract), baseTypes);
    }

    /// <summary>
    /// 从基础目录中加载公共类类型
    /// includeAbstract = false
    /// </summary>
    /// <returns>类型队列</returns>
    public static IList<Type> LoadExportedClassTypesFromBaseDirectory()
    {
      return LoadExportedClassTypesFromBaseDirectory(false);
    }

    /// <summary>
    /// 从基础目录中加载公共类类型
    /// </summary>
    /// <param name="includeAbstract">是否包括抽象类型</param>
    /// <returns>类型队列</returns>
    public static IList<Type> LoadExportedClassTypesFromBaseDirectory(bool includeAbstract)
    {
      string[] fileNames = Directory.GetFiles(AppConfig.BaseDirectory, "*.dll");
      List<Type> result = new List<Type>(fileNames.Length);
      foreach (string s in fileNames)
        result.AddRange(LoadExportedClassTypes(s, includeAbstract));
      return result;
    }

    /// <summary>
    /// 加载公共类类型
    /// includeAbstract = false
    /// </summary>
    /// <param name="fileName">程序集文件名</param>
    /// <returns>类型队列</returns>
    public static IList<Type> LoadExportedClassTypes(string fileName)
    {
      return LoadExportedClassTypes(fileName, false);
    }

    /// <summary>
    /// 加载公共类类型
    /// </summary>
    /// <param name="fileName">程序集文件名</param>
    /// <param name="includeAbstract">是否包括抽象类型</param>
    /// <returns>类型队列</returns>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
    public static IList<Type> LoadExportedClassTypes(string fileName, bool includeAbstract)
    {
      try
      {
        return GetExportedClassTypes(Assembly.LoadFile(fileName), includeAbstract);
      }
      catch (Exception ex)
      {
        if (AppConfig.Debugging)
          EventLog.SaveLocal(MethodBase.GetCurrentMethod(), fileName, ex);
        return new List<Type>(0);
      }
    }

    /// <summary>
    /// 获取公共类类型
    /// </summary>
    /// <param name="assemblie">程序集</param>
    /// <param name="includeAbstract">是否包括抽象类型</param>
    /// <returns>类型队列</returns>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
    public static IList<Type> GetExportedClassTypes(Assembly assemblie, bool includeAbstract)
    {
      List<Type> result = new List<Type>();
      foreach (Type item in assemblie.GetExportedTypes())
        try
        {
          if (AppUtilities.IsNotApplicationType(item))
            continue;
          if (!item.IsClass || item.IsAbstract && !includeAbstract || item.IsGenericType || item.IsCOMObject)
            continue;
          result.Add(item);
        }
        catch (Exception ex)
        {
          if (AppConfig.Debugging)
            EventLog.SaveLocal(MethodBase.GetCurrentMethod(), item.FullName, ex);
        }
      return result;
    }

    /// <summary>
    /// 加载类型
    /// 主要用于IDE环境
    /// </summary>
    /// <param name="type">类</param>
    /// <returns>类</returns>
    public static Type LoadType(Type type)
    {
      if (type == null)
        return null;
      try
      {
        Type result = Type.GetType(type.FullName, false);
        return result ?? type;
      }
      catch (ArgumentException)
      {
        return type;
      }
    }

    /// <summary>
    /// 加载类型
    /// </summary>
    /// <param name="typeName">类名</param>
    /// <returns>类</returns>
    public static Type LoadType(string typeName)
    {
      return LoadType(null, typeName);
    }

    /// <summary>
    /// 加载类型
    /// </summary>
    /// <param name="assemblyName">程序集名</param>
    /// <param name="typeName">类名</param>
    /// <returns>类</returns>
    public static Type LoadType(string assemblyName, string typeName)
    {
      if (String.IsNullOrEmpty(typeName))
        return null;
      Type result = Type.GetType(typeName, false);
      if (result != null)
        return result;
      if (!String.IsNullOrEmpty(assemblyName))
      {
        Assembly assembly = LoadAssembly(assemblyName);
        return assembly != null ? assembly.GetType(typeName, false) : null;
      }
      return _typeCache.GetValue(typeName, () =>
      {
        assemblyName = typeName;
        while (true)
        {
          int i = assemblyName.LastIndexOf('.');
          if (i > 0)
          {
            assemblyName = assemblyName.Remove(i);
            Type value = LoadType(assemblyName, typeName);
            if (value != null)
              return value;
          }
          else
            break;
        }
        if (DataDictionaryHub.Worker != null)
        {
          AssemblyInfo assemblyInfo = DataDictionaryHub.GetAssemblyInfo(typeName);
          if (assemblyInfo != null)
          {
            Type value = LoadType(assemblyInfo.Name, typeName);
            if (value != null)
              return value;
          }
        }
        return null;
      }, false);
    }

    /// <summary>
    /// 加载派生类型
    /// </summary>
    /// <param name="key">AppSettings的键</param>
    /// <returns>类</returns>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
    public static Type LoadSubclassType<T>(string key)
    {
      return LoadSubclassType(typeof(T), key);
    }

    /// <summary>
    /// 加载派生类型
    /// </summary>
    /// <param name="baseType">基础类型、Interface</param>
    /// <param name="key">AppSettings的键</param>
    /// <returns>类</returns>
    public static Type LoadSubclassType(Type baseType, string key)
    {
      string assemblyName = AppSettings.ReadValue(key, false, true);
      if (!String.IsNullOrEmpty(assemblyName))
      {
        Assembly assembly = LoadAssembly(assemblyName);
        if (assembly != null)
          foreach (Type item in assembly.GetExportedTypes())
          {
            if (!item.IsClass || item.IsAbstract || item.IsGenericType || item.IsCOMObject)
              continue;
            if (baseType.IsInterface && baseType.IsAssignableFrom(item) ||
              item.IsSubclassOf(baseType))
              return item;
          }
      }
      return null;
    }

    /// <summary>
    /// 返回基础类型
    /// </summary>
    /// <param name="type">类</param>
    public static Type GetUnderlyingType(Type type)
    {
      if (type != null && type.IsGenericType && (type.GetGenericTypeDefinition() == typeof(Nullable<>)))
        return Nullable.GetUnderlyingType(type);
      return type;
    }

    /// <summary>
    /// 返回数据源的核心类型
    /// </summary>
    /// <param name="type">类</param>
    public static Type GetCoreType(Type type)
    {
      return FindListItemType(type) ?? type;
    }

    /// <summary>
    /// 返回队列项类型
    /// </summary>
    /// <param name="type">类</param>
    public static Type FindListItemType(Type type)
    {
      if (type == null)
        return null;
      if (!typeof(IEnumerable).IsAssignableFrom(type))
        return null;
      if (type == typeof(string))
        return null;
      if (type.IsArray)
        return type.GetElementType();
      foreach (Type interfaceType in type.GetInterfaces())
        if (String.CompareOrdinal(interfaceType.Name, "IEnumerable`1") == 0)
          foreach (Type itemType in interfaceType.GetGenericArguments())
            if (!itemType.IsGenericParameter)
              return itemType;
      return null;
    }

    /// <summary>
    /// 返回队列项类型
    /// </summary>
    public static IList<Type> FindKnownTypes(object value, Type valueType)
    {
      List<Type> result = new List<Type>();
      if (valueType == null && value != null)
        valueType = value.GetType();
      if (valueType != null)
      {
        result.Add(valueType);
        Type listItemType = FindListItemType(GetUnderlyingType(valueType));
        if (listItemType != null)
        {
          result.Add(listItemType);
          if (value != null)
            foreach (object item in (IEnumerable)value)
              if (item != null)
              {
                Type itemType = item.GetType();
                if (!result.Contains(itemType))
                  result.AddRange(FindKnownTypes(item, itemType));
              }
        }
      }
      return result;
    }

    /// <summary>
    /// 返回具有指定 resultType 类型而且其值等效于指定对象的值
    /// </summary>
    /// <param name="value">值</param>
    /// <param name="resultType">返回值的类型</param>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
    public static object ChangeType(object value, Type resultType)
    {
      if (resultType == null)
        return value;

      string vs = value as string;
      if (vs != null && resultType != typeof(string))
      {
        string s = vs.Trim();
        if (String.IsNullOrEmpty(s) || String.Compare(s, Phenix.Core.Code.Converter.NullSymbolic, StringComparison.OrdinalIgnoreCase) == 0)
          value = null;
      }

      if (value == null || value == DBNull.Value)
        return resultType.IsValueType ? Activator.CreateInstance(resultType) : null;

      Type valueType = value.GetType();
      if (valueType == resultType)
        return value;

      Type resultUnderlyingType = GetUnderlyingType(resultType);
      if (valueType == resultUnderlyingType)
        return value;

      Type valueUnderlyingType = GetUnderlyingType(valueType);
      if (valueUnderlyingType == resultUnderlyingType)
        goto Label;

      if (valueUnderlyingType.IsValueType || resultUnderlyingType.IsValueType)
      {
        if (resultUnderlyingType.IsEnum)
          return Enum.Parse(resultUnderlyingType, Convert.ToString(value));

        if (resultUnderlyingType == typeof(bool))
        {
          if (valueUnderlyingType == typeof(string))
          {
            string s = ((string)value).Trim();
            BooleanOption booleanOption;
            if (Enum.TryParse(s, true, out booleanOption))
              return booleanOption == BooleanOption.Y;
            if (IsNumeric(s))
              return !IsZero(s);
          }
          try
          {
            return Convert.ToBoolean(value);
          }
          catch (Exception)
          {
            goto Label;
          }
        }

        if (resultUnderlyingType == typeof(System.Drawing.Color))
          return System.Drawing.Color.FromArgb((int)Convert.ChangeType(value, typeof(int)));
        if (valueUnderlyingType == typeof(System.Drawing.Color))
          return Convert.ChangeType(((System.Drawing.Color)value).ToArgb(), resultUnderlyingType);

        if (resultUnderlyingType == typeof(Guid))
          return valueUnderlyingType == typeof(byte[]) 
            ? new Guid((byte[])value)
            : new Guid(Convert.ToString(value));
        if (valueUnderlyingType == typeof(Guid))
          return resultUnderlyingType == typeof(byte[]) 
            ? ((Guid)value).ToByteArray()
            : Convert.ChangeType(((Guid)value).ToString(), resultUnderlyingType);

        goto Label;
      }

      if (valueUnderlyingType.IsArray || resultUnderlyingType.IsArray)
      {
        if (valueUnderlyingType == typeof(byte[]) && (resultUnderlyingType == typeof(System.Drawing.Image) || resultUnderlyingType.IsSubclassOf(typeof(System.Drawing.Image))))
          using (MemoryStream memoryStream = new MemoryStream((byte[])value))
          {
            return System.Drawing.Image.FromStream(memoryStream);
          }
        if (resultUnderlyingType == typeof(byte[]) && (valueUnderlyingType == typeof(System.Drawing.Image) || valueUnderlyingType.IsSubclassOf(typeof(System.Drawing.Image))))
          using (MemoryStream memoryStream = new MemoryStream())
          {
            ((System.Drawing.Image)value).Save(memoryStream, System.Drawing.Imaging.ImageFormat.Jpeg);
            return memoryStream.ToArray();
          }

        if (valueUnderlyingType.IsArray && resultUnderlyingType == typeof(string))
        {
          StringBuilder result = new StringBuilder();
          foreach (object item in (object[])value)
          {
            result.Append(ChangeType(item, resultUnderlyingType));
            result.Append(AppConfig.VALUE_SEPARATOR);
          }
          return result.ToString();
        }
        if (valueUnderlyingType == typeof(string) && resultUnderlyingType.IsArray)
        {
          string[] strings = ((string)value).Split(AppConfig.VALUE_SEPARATOR);
          List<object> result = new List<object>(strings.Length);
          foreach (string s in strings)
            result.Add(ChangeType(s, resultUnderlyingType));
          return result.ToArray();
        }
      }

      if (valueUnderlyingType == typeof(Type) && resultUnderlyingType == typeof(string))
        return valueUnderlyingType.FullName;
      if (valueUnderlyingType == typeof(string) && resultUnderlyingType == typeof(Type))
        return LoadType((string)value);

    Label:
      try
      {
        if (valueUnderlyingType == typeof(DateTime))
          if ((DateTime)value == System.Data.SqlTypes.SqlDateTime.MinValue.Value)
            value = DateTime.MinValue;
        try
        {
          return Convert.ChangeType(value, resultUnderlyingType);
        }
        catch (InvalidCastException)
        {
          if (valueUnderlyingType.IsClass && resultUnderlyingType == typeof(string))
            return JsonConvert.SerializeObject(value);
          if (valueUnderlyingType == typeof(string) && resultUnderlyingType.IsClass)
            return JsonConvert.DeserializeObject((string)value, resultUnderlyingType);
          throw;
        }
      }
      catch (Exception ex)
      {
        throw new InvalidCastException(String.Format("{0}: value = '{1}', value type = '{2}', result type = '{3}[{4}]'", ex.Message, value, valueType, resultType, resultUnderlyingType), ex);
      }
    }

    /// <summary>
    /// 返回与数据库字段类型匹配的值
    /// </summary>
    /// <param name="value">值</param>
    public static object ConvertToDbValue(object value)
    {
      if (value == null || value == DBNull.Value)
        return DBNull.Value;
      Type valueUnderlyingType = GetUnderlyingType(value.GetType());
      if (valueUnderlyingType.IsEnum)
        return (int)value;
      if (valueUnderlyingType == typeof(bool))
        return (bool)value ? 1 : 0;
      if (valueUnderlyingType == typeof(DateTime))
        return (DateTime)value < System.Data.SqlTypes.SqlDateTime.MinValue.Value ? System.Data.SqlTypes.SqlDateTime.MinValue.Value : value;
      if (valueUnderlyingType == typeof(System.Drawing.Color))
        return ((System.Drawing.Color)value).ToArgb();
      if (valueUnderlyingType == typeof(Guid))
        return ((Guid)value).ToString();
      if (value is System.Drawing.Image)
        using (MemoryStream memoryStream = new MemoryStream())
        {
          ((System.Drawing.Image)value).Save(memoryStream, System.Drawing.Imaging.ImageFormat.Jpeg);
          return memoryStream.ToArray();
        }
      return value;
    }

    /// <summary>
    /// 检索主从结构中符合条件的队列类型
    /// </summary>
    /// <param name="type">类</param>
    /// <param name="propertyName">属性名</param>
    /// <param name="levelNumber">层级数</param>
    public static Type FindDetailListType(Type type, string propertyName, int levelNumber)
    {
      int levelTally = 0;
      return DoFindDetailListType(type, propertyName, levelNumber, ref levelTally);
    }

    private static Type DoFindDetailListType(Type type, string propertyName, int levelNumber, ref int levelTally)
    {
      levelTally = levelTally + 1;
      if (levelTally > levelNumber)
        return null;
      foreach (PropertyInfo item in GetCoreType(type).GetProperties())
        if (!item.PropertyType.IsValueType && typeof(IList).IsAssignableFrom(item.PropertyType))
        {
          if (levelTally == levelNumber && String.CompareOrdinal(item.Name, propertyName) == 0)
            return item.PropertyType;
          Type result = DoFindDetailListType(item.PropertyType, propertyName, levelNumber, ref levelTally);
          if (result != null)
            return result;
        }
      return null;
    }

    /// <summary>
    /// 检索第一个抽象类
    /// </summary>
    public static Type FindFirstAbstractType(Type type)
    {
      while (type != null)
      {
        if (type.IsAbstract)
          return type;
        type = type.BaseType;
      }
      return null;
    }

    #endregion

    #region MemberInfo

    /// <summary>
    /// 类成员属性的全名
    /// </summary>
    /// <param name="memberInfo">类成员属性的信息</param>
    public static string GetMemberFullName(MemberInfo memberInfo)
    {
      if (memberInfo == null)
        throw new ArgumentNullException("memberInfo");
      return String.Format("{0}.{1}", memberInfo.DeclaringType.FullName, memberInfo.Name);
    }

    /// <summary>
    /// 取类型匹配的实例字段
    /// </summary>
    /// <param name="objectType">类</param>
    /// <param name="fieldBaseType">字段基础类型、Interface</param>
    public static IList<FieldInfo> GetInstanceFields(Type objectType, Type fieldBaseType)
    {
      return GetInstanceFields(objectType, null, fieldBaseType);
    }

    /// <summary>
    /// 取类型匹配的实例字段
    /// </summary>
    /// <param name="objectType">类</param>
    /// <param name="objectBaseType">类的基础类型、Interface</param>
    /// <param name="fieldBaseType">字段基础类型、Interface</param>
    public static IList<FieldInfo> GetInstanceFields(Type objectType, Type objectBaseType, Type fieldBaseType)
    {
      if (objectType == null)
        throw new ArgumentNullException("objectType");
      if (fieldBaseType == null)
        throw new ArgumentNullException("fieldBaseType");
      return _instanceFieldsCache.GetValue(String.Format("{0},{1},{2}", objectType.FullName, objectBaseType != null ? objectBaseType.FullName : null, fieldBaseType.FullName), () =>
      {
        List<FieldInfo> value = new List<FieldInfo>();
        Type type = objectType;
        while (type != null && (objectBaseType == null || objectBaseType.IsInterface && objectBaseType.IsAssignableFrom(type) || type.IsSubclassOf(objectBaseType)))
        {
          foreach (FieldInfo item in type.GetFields(BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic))
          {
            if (fieldBaseType.IsInterface && fieldBaseType.IsAssignableFrom(item.FieldType) || item.FieldType.IsSubclassOf(fieldBaseType))
              value.Add(item);
            else
            {
              Type fieldCoreUnderlyingType = GetUnderlyingType(GetCoreType(item.FieldType));
              if (fieldBaseType.IsInterface && fieldBaseType.IsAssignableFrom(fieldCoreUnderlyingType) || fieldCoreUnderlyingType.IsSubclassOf(fieldBaseType))
                value.Add(item);
            }
          }
          type = type.BaseType;
        }
        return value.AsReadOnly();
      }, true);
    }

    /// <summary>
    /// 取类型匹配的实例属性
    /// </summary>
    /// <param name="objectType">类</param>
    /// <param name="propertyBaseType">属性基础类型、Interface</param>
    public static IList<PropertyInfo> GetInstanceProperties(Type objectType, Type propertyBaseType)
    {
      return GetInstanceProperties(objectType, null, propertyBaseType);
    }

    /// <summary>
    /// 取类型匹配的实例属性
    /// </summary>
    /// <param name="objectType">类</param>
    /// <param name="objectBaseType">类的基础类型、Interface</param>
    /// <param name="propertyBaseType">属性基础类型、Interface</param>
    public static IList<PropertyInfo> GetInstanceProperties(Type objectType, Type objectBaseType, Type propertyBaseType)
    {
      if (objectType == null)
        throw new ArgumentNullException("objectType");
      if (propertyBaseType == null)
        throw new ArgumentNullException("propertyBaseType");
      return _instancePropertiesCache.GetValue(String.Format("{0},{1},{2}", objectType.FullName, objectBaseType != null ? objectBaseType.FullName : null, propertyBaseType.FullName), () =>
      {
        List<PropertyInfo> value = new List<PropertyInfo>();
        Type type = objectType;
        while (type != null && (objectBaseType == null || objectBaseType.IsInterface && objectBaseType.IsAssignableFrom(type) || type.IsSubclassOf(objectBaseType)))
        {
          foreach (PropertyInfo item in type.GetProperties(BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic))
          {
            if (propertyBaseType.IsInterface && propertyBaseType.IsAssignableFrom(item.PropertyType) || item.PropertyType.IsSubclassOf(propertyBaseType))
              value.Add(item);
            else
            {
              Type propertCoreUnderlyingType = GetUnderlyingType(GetCoreType(item.PropertyType));
              if (propertyBaseType.IsInterface && propertyBaseType.IsAssignableFrom(propertCoreUnderlyingType) || propertCoreUnderlyingType.IsSubclassOf(propertyBaseType))
                value.Add(item);
            }
          }
          type = type.BaseType;
        }
        return value.AsReadOnly();
      }, true);
    }
    
    /// <summary>
    /// 检索属性信息
    /// </summary>
    /// <param name="objectType">类</param>
    /// <param name="propertyName">属性名</param>
    public static PropertyInfo FindPropertyInfo(Type objectType, string propertyName)
    {
      if (objectType == null || String.IsNullOrEmpty(propertyName))
        return null;
      Type type = objectType;
      do
      {
        PropertyInfo result = type.GetProperty(propertyName, BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
        if (result != null)
          return result;
        type = type.BaseType;
      } while (type != null);
      return null;
    }

    #endregion

    #region Serialization

    /// <summary>
    /// 序列化
    /// </summary>
    /// <param name="obj">对象</param>
    public static string BinarySerialize(object obj)
    {
      if (obj == null)
        return null;
      using (MemoryStream memoryStream = new MemoryStream())
      {
        BinaryFormatter serializer = new BinaryFormatter();
        serializer.Serialize(memoryStream, obj);
        return Phenix.Core.Code.Converter.BytesToHexString(memoryStream.ToArray());
      }
    }

    /// <summary>
    /// 反序列化
    /// </summary>
    /// <param name="value">Binary值</param>
    public static object BinaryDeserialize(string value)
    {
      if (String.IsNullOrEmpty(value))
        return null;
      using (MemoryStream memoryStream = new MemoryStream(Phenix.Core.Code.Converter.HexStringToBytes(value)))
      {
        BinaryFormatter serializer = new BinaryFormatter();
        return serializer.Deserialize(memoryStream);
      }
    }

    /// <summary>
    /// 反序列化
    /// </summary>
    /// <param name="value">Binary值</param>
    public static T BinaryDeserialize<T>(string value)
    {
      return (T)BinaryDeserialize(value);
    }

    /// <summary>
    /// 序列化
    /// </summary>
    /// <param name="obj">对象</param>
    public static string XmlSerialize(object obj)
    {
      if (obj == null)
        return null;
      XmlSerializer serializer = new XmlSerializer(obj.GetType());
      using (MemoryStream memoryStream = new MemoryStream())
      {
        serializer.Serialize(memoryStream, obj);
        return Encoding.UTF8.GetString(memoryStream.ToArray());
      }
    }

    /// <summary>
    /// 反序列化
    /// </summary>
    /// <param name="value">XML值</param>
    /// <param name="type">反序列化类型</param>
    public static object XmlDeserialize(string value, Type type)
    {
      if (String.IsNullOrEmpty(value))
        return null;
      using (MemoryStream memoryStream = new MemoryStream(Encoding.UTF8.GetBytes(value)))
      {
        XmlSerializer serializer = new XmlSerializer(type);
        return serializer.Deserialize(memoryStream);
      }
    }

    /// <summary>
    /// 反序列化
    /// </summary>
    /// <param name="value">XML值</param>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
    public static T XmlDeserialize<T>(string value)
    {
      return (T)XmlDeserialize(value, typeof(T));
    }

    /// <summary>
    /// 序列化
    /// </summary>
    /// <param name="obj">对象</param>
    public static string JsonSerialize(object obj)
    {
      if (obj == null)
        return null;
      string s = obj as string;
      if (s != null)
        return s;
      JsonSerializerSettings settings = new JsonSerializerSettings();
      settings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
      Newtonsoft.Json.Converters.IsoDateTimeConverter timeConverter = new Newtonsoft.Json.Converters.IsoDateTimeConverter();
      timeConverter.DateTimeFormat = "yyyy'-'MM'-'dd' 'HH':'mm':'ss";
      settings.Converters.Add(timeConverter);
      return JsonConvert.SerializeObject(obj, settings);
    }

    /// <summary>
    /// 反序列化
    /// </summary>
    /// <param name="value">JSON值</param>
    /// <param name="type">反序列化类型</param>
    public static object JsonDeserialize(string value, Type type)
    {
      if (type == typeof(string))
        return value;
      if (String.IsNullOrEmpty(value))
        return null;
      return JsonConvert.DeserializeObject(value, type);
    }

    /// <summary>
    /// 反序列化
    /// </summary>
    /// <param name="value">序列化值</param>
    public static T JsonDeserialize<T>(string value)
    {
      return (T)JsonDeserialize(value, typeof(T));
    }

    #endregion

    #region Object

    /// <summary>
    /// 是零
    /// </summary>
    /// <param name="value">值</param>
    public static bool IsZero(string value)
    {
      return new Regex(@"^[0]+$|^[0]+(\.)?[0]+$").IsMatch(value);
    }

    /// <summary>
    /// 是数字
    /// </summary>
    /// <param name="value">值</param>
    public static bool IsNumeric(string value)
    {
      return new Regex(@"^\d+$|^\d+(\.)?\d+$").IsMatch(value);
    }

    /// <summary>
    /// 是整数
    /// </summary>
    /// <param name="value">值</param>
    public static bool IsInteger(string value)
    {
      return new Regex(@"^\d+$").IsMatch(value);
    }

    /// <summary>
    /// 字符串长度
    /// </summary>
    /// <param name="value">值</param>
    /// <param name="isUnicode">是否Unicode</param>
    public static int Length(string value, bool isUnicode)
    {
      if (String.IsNullOrEmpty(value))
        return 0;
      return isUnicode ? value.Length : Encoding.Default.GetByteCount(value);
    }

    /// <summary>
    /// 截取字符串
    /// </summary>
    /// <param name="value">值</param>
    /// <param name="length">截取长度</param>
    /// <param name="isUnicode">是否Unicode</param>
    public static string SubString(string value, int length, bool isUnicode)
    {
      if (String.IsNullOrEmpty(value))
        return value;
      if (isUnicode)
      {
        if (value.Length > length)
          return value.Remove(length);
      }
      else
      {
        if (Encoding.Default.GetByteCount(value) > length)
          return Encoding.Default.GetString(Encoding.Default.GetBytes(value), 0, length);
      }
      return value;
    }

    /// <summary>
    /// 比较List对象
    /// </summary>
    /// <param name="arrayA">对象A</param>
    /// <param name="arrayB">对象B</param>
    public static bool CompareList(IList arrayA, IList arrayB)
    {
      if (object.Equals(arrayA, arrayB))
        return true;
      if (arrayA != null && arrayB != null && arrayA.Count == arrayB.Count)
      {
        for (int i = 0; i < arrayA.Count; i++)
          if (!object.Equals(arrayA[i], arrayB[i]))
            return false;
        return true;
      }
      return false;
    }

    #endregion

    #region Clone

    /// <summary>
    ///  深克隆完整对象
    /// </summary>
    /// <param name="source">源对象</param>
    public static object Clone(object source)
    {
      if (source == null)
        return null;
      using (MemoryStream memoryStream = new MemoryStream())
      {
        BinaryFormatter formatter = new BinaryFormatter();
        formatter.Serialize(memoryStream, source);
        memoryStream.Position = 0;
        return formatter.Deserialize(memoryStream);
      }
    }

    /// <summary>
    /// 填充对象字段值
    /// </summary>
    /// <param name="source">数据源</param>
    /// <param name="target">目标对象</param>
    /// <param name="reset">重新设定</param>
    public static void FillFieldValues<T>(T source, T target, bool reset)
      where T : class 
    {
      if (source == null)
        return;

      foreach (FieldInfo item in source.GetType().GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic))
      {
        if (reset || item.GetValue(target) == null)
          item.SetValue(target, item.GetValue(source));
      }
    }

    #endregion

    #region File

    /// <summary>
    /// 保存对象
    /// </summary>
    /// <param name="sourceStream">数据源</param>
    /// <param name="path">文件名</param>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
    public static bool Save(Stream sourceStream, string path)
    {
      try
      {
        if (sourceStream == null)
        {
          if (File.Exists(path))
            File.Delete(path);
          return false;
        }
        string directory = Path.GetDirectoryName(path);
        if (!String.IsNullOrEmpty(directory) && !Directory.Exists(directory))
          Directory.CreateDirectory(directory);
        using (FileStream fileStream = new FileStream(path, FileMode.Create, FileAccess.Write))
        {
          sourceStream.Position = 0;
          CompressHelper.Compress(sourceStream, fileStream);
        }
        return true;
      }
      catch (Exception ex)
      {
        if (AppConfig.Debugging)
          EventLog.SaveLocal(MethodBase.GetCurrentMethod(), path, ex);
        return false;
      }
    }

    /// <summary>
    /// 保存对象
    /// </summary>
    /// <param name="source">源对象</param>
    /// <param name="path">文件名</param>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
    public static bool Save(object source, string path)
    {
      try
      {
        if (source == null)
        {
          if (File.Exists(path))
            File.Delete(path);
          return false;
        }
        string directory = Path.GetDirectoryName(path);
        if (!String.IsNullOrEmpty(directory) && !Directory.Exists(directory))
          Directory.CreateDirectory(directory);
        using (MemoryStream memoryStream = new MemoryStream())
        using (FileStream fileStream = new FileStream(path, FileMode.Create, FileAccess.Write))
        {
          BinaryFormatter formatter = new BinaryFormatter();
          formatter.Serialize(memoryStream, source);
          memoryStream.Position = 0;
          CompressHelper.Compress(memoryStream, fileStream);
        }
        return true;
      }
      catch (Exception ex)
      {
        if (AppConfig.Debugging)
          EventLog.SaveLocal(MethodBase.GetCurrentMethod(), path, ex);
        return false;
      }
    }

    /// <summary>
    /// 还原对象
    /// </summary>
    /// <param name="path">文件名</param>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
    public static object Restore(string path)
    {
      if (!File.Exists(path))
        return null;
      try
      {
        using (FileStream fileStream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read))
        using (Stream stream = CompressHelper.Decompress(fileStream))
        {
          BinaryFormatter formatter = new BinaryFormatter();
          return formatter.Deserialize(stream);
        }
      }
      catch (Exception ex)
      {
        if (AppConfig.Debugging)
          EventLog.SaveLocal(MethodBase.GetCurrentMethod(), path, ex);
        return null;
      }
    }

    #endregion

    #endregion
  }
}