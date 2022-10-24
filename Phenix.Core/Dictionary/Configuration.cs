using System;
using System.Reflection;
using Phenix.Core.Log;
using Phenix.Core.Mapping;
using Phenix.Core.Reflection;

namespace Phenix.Core.Dictionary
{
  /// <summary>
  /// 配置
  /// </summary>
  public static class Configuration
  {
    #region 方法

    /// <summary>
    /// 保存信息
    /// </summary>
    /// <param name="key">键</param>
    /// <param name="objectType">类</param>
    /// <param name="propertyName">属性名</param>
    /// <param name="propertyValue">属性值</param>
    public static void SaveValue(string key, Type objectType, string propertyName, string propertyValue)
    {
      string friendlyName = null;
      PropertyMapInfo propertyMapInfo = ClassMemberHelper.GetPropertyMapInfo(objectType, propertyName);
      if (propertyMapInfo != null)
        friendlyName = propertyMapInfo.FriendlyName;
      DataDictionaryHub.AddAssemblyClassPropertyInfo(objectType, propertyName, friendlyName, true, key, propertyValue, AssemblyClassType.Ordinary);
    }

    /// <summary>
    /// 读取信息
    /// </summary>
    /// <param name="key">键</param>
    /// <param name="objectType">类</param>
    /// <param name="propertyName">属性名</param>
    public static String ReadValue(string key, Type objectType, string propertyName)
    {
      AssemblyClassInfo classInfo = DataDictionaryHub.GetClassInfo(objectType);
      if (classInfo == null)
        return null;
      AssemblyClassPropertyInfo classPropertyInfo = classInfo.GetClassPropertyInfo(propertyName);
      if (classPropertyInfo == null || !classPropertyInfo.Configurable)
        return null;
      return classPropertyInfo.GetConfigValue(key);
    }

    #region Property

    /// <summary>
    /// 设置属性值
    /// key = null
    /// allowSave = true
    /// </summary>
    /// <param name="field">属性字段</param>
    /// <param name="newValue">新值</param>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1045:DoNotPassTypesByReference")]
    public static void SetProperty<TField, TValue>(ref TField field, TValue newValue)
    {
      if (IsEquality(field, newValue))
        return;
      DoSetProperty(new System.Diagnostics.StackTrace().GetFrame(1).GetMethod(), null, ref field, newValue, true);
    }

    /// <summary>
    /// 设置属性值
    /// key = null
    /// </summary>
    /// <param name="field">属性字段</param>
    /// <param name="newValue">新值</param>
    /// <param name="allowSave">允许保存状态信息?</param>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1045:DoNotPassTypesByReference")]
    public static void SetProperty<TField, TValue>(ref TField field, TValue newValue, bool allowSave)
    {
      if (IsEquality(field, newValue))
        return;
      DoSetProperty(new System.Diagnostics.StackTrace().GetFrame(1).GetMethod(), null, ref field, newValue, allowSave);
    }

    /// <summary>
    /// 设置属性值
    /// allowSave = true
    /// </summary>
    /// <param name="key">键</param>
    /// <param name="field">属性字段</param>
    /// <param name="newValue">新值</param>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1045:DoNotPassTypesByReference")]
    public static void SetProperty<TField, TValue>(string key, ref TField field, TValue newValue)
    {
      if (IsEquality(field, newValue))
        return;
      DoSetProperty(new System.Diagnostics.StackTrace().GetFrame(1).GetMethod(), key, ref field, newValue, true);
    }

    /// <summary>
    /// 设置属性值
    /// </summary>
    /// <param name="key">键</param>
    /// <param name="field">属性字段</param>
    /// <param name="newValue">新值</param>
    /// <param name="allowSave">允许保存状态信息?</param>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1045:DoNotPassTypesByReference")]
    public static void SetProperty<TField, TValue>(string key, ref TField field, TValue newValue, bool allowSave)
    {
      if (IsEquality(field, newValue))
        return;
      DoSetProperty(new System.Diagnostics.StackTrace().GetFrame(1).GetMethod(), key, ref field, newValue, allowSave);
    }

    private static bool IsEquality<TField, TValue>(TField field, TValue newValue)
    {
      if (typeof(TField).IsClass && typeof(TValue).IsClass)
        return object.Equals(field, newValue);
      string fieldString = (string)Utilities.ChangeType(field, typeof(string));
      string newValueString = (string)Utilities.ChangeType(newValue, typeof(string));
      return String.CompareOrdinal(fieldString, newValueString) == 0;
    }

    private static void DoSetProperty<TField, TValue>(MethodBase method, string key, ref TField field, TValue newValue, bool allowSave)
    {
      if (allowSave)
        SaveValue(key, method.DeclaringType, method.Name.Substring(4), (string)Utilities.ChangeType(newValue, typeof(string)));
      field = (TField)Utilities.ChangeType(newValue, typeof(TField));
    }

    /// <summary>
    /// 获取属性值
    /// key = null
    /// allowSave = true
    /// </summary>
    /// <param name="field">属性字段</param>
    /// <param name="defaultValue">缺省值</param>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1045:DoNotPassTypesByReference")]
    public static TValue GetProperty<TField, TValue>(ref TField field, TValue defaultValue)
    {
      object result = null;
      if (TryGetProperty<TField, TValue>(field, ref result))
        return (TValue)result;
      return DoGetProperty(new System.Diagnostics.StackTrace().GetFrame(1).GetMethod(), null, ref field, defaultValue, true);
    }

    /// <summary>
    /// 获取属性值
    /// key = null
    /// </summary>
    /// <param name="field">属性字段</param>
    /// <param name="defaultValue">缺省值</param>
    /// <param name="allowSave">允许保存状态信息?</param>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1045:DoNotPassTypesByReference")]
    public static TValue GetProperty<TField, TValue>(ref TField field, TValue defaultValue, bool allowSave)
    {
      object result = null;
      if (TryGetProperty<TField, TValue>(field, ref result))
        return (TValue)result;
      return DoGetProperty(new System.Diagnostics.StackTrace().GetFrame(1).GetMethod(), null, ref field, defaultValue, allowSave);
    }

    /// <summary>
    /// 获取属性值
    /// allowSave = true
    /// </summary>
    /// <param name="key">键</param>
    /// <param name="field">属性字段</param>
    /// <param name="defaultValue">缺省值</param>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1045:DoNotPassTypesByReference")]
    public static TValue GetProperty<TField, TValue>(string key, ref TField field, TValue defaultValue)
    {
      object result = null;
      if (TryGetProperty<TField, TValue>(key, field, ref result))
        return (TValue)result;
      return DoGetProperty(new System.Diagnostics.StackTrace().GetFrame(1).GetMethod(), key, ref field, defaultValue, true);
    }

    /// <summary>
    /// 获取属性值
    /// </summary>
    /// <param name="key">键</param>
    /// <param name="field">属性字段</param>
    /// <param name="defaultValue">缺省值</param>
    /// <param name="allowSave">允许保存状态信息?</param>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1045:DoNotPassTypesByReference")]
    public static TValue GetProperty<TField, TValue>(string key, ref TField field, TValue defaultValue, bool allowSave)
    {
      object result = null;
      if (TryGetProperty<TField, TValue>(key, field, ref result))
        return (TValue)result;
      return DoGetProperty(new System.Diagnostics.StackTrace().GetFrame(1).GetMethod(), key, ref field, defaultValue, allowSave);
    }
    
    private static bool TryGetProperty<TField, TValue>(string key, TField field, ref object value)
    {
      if (String.IsNullOrEmpty(key))
        return TryGetProperty<TField, TValue>(field, ref value);
      return false;
    }

    private static bool TryGetProperty<TField, TValue>(TField field, ref object value)
    {
      if (!object.Equals(field, null))
        try
        {
          value = Utilities.ChangeType(field, typeof(TValue));
          return true;
        }
        catch (InvalidCastException ex)
        {
          EventLog.SaveLocal(MethodBase.GetCurrentMethod(), field.ToString(), ex);
        }
      return false;
    }

    private static TValue DoGetProperty<TField, TValue>(MethodBase method, string key, ref TField field, TValue defaultValue, bool allowSave)
    {
      string value = ReadValue(key, method.DeclaringType, method.Name.Substring(4));
      if (value != null)
        try
        {
          if (allowSave)
            field = (TField)Utilities.ChangeType(value, typeof(TField));
          return (TValue)Utilities.ChangeType(value, typeof(TValue));
        }
        catch (InvalidCastException ex)
        {
          EventLog.SaveLocal(MethodBase.GetCurrentMethod(), value, ex);
        }
      DoSetProperty(method, key, ref field, defaultValue, allowSave);
      return defaultValue;
    }

    #endregion

    #endregion
  }
}