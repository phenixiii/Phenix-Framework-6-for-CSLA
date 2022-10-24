using System;
using System.Configuration;
using System.Reflection;
using Phenix.Core.Code;
using Phenix.Core.Log;
using Phenix.Core.Reflection;
using Phenix.Core.Security.Cryptography;

namespace Phenix.Core
{
  /// <summary>
  /// Ӧ��ϵͳ���ù���
  /// </summary>
  public static class AppSettings
  {
    #region ����

    private static bool? _allowSave;
    /// <summary>
    /// ������״̬��Ϣ?
    /// </summary>
    public static bool AllowSave
    {
      get
      {
        if (!_allowSave.HasValue)
          _allowSave = AppConfig.AppType != AppType.Webform;
        return _allowSave.Value;
      }
      set { _allowSave = value; }
    }

    private static readonly object _defaultKeyDisabledLock = new object();
    private static bool _defaultKeyDisabled;
    private static string _defaultKey = String.Empty;
    /// <summary>
    /// ȱʡID
    /// </summary>
    public static string DefaultKey
    {
      get
      {
        lock (_defaultKeyDisabledLock)
        {
          if (_defaultKeyDisabled)
            return String.Empty;
        }
        return _defaultKey;
      }
      set
      {
        if (String.IsNullOrEmpty(value))
          _defaultKey = String.Empty;
        else if (!value.EndsWith(AppConfig.PARAM_SEPARATOR.ToString()))
          _defaultKey = value + AppConfig.PARAM_SEPARATOR;
        else
          _defaultKey = value;
      }
    }

    private static string _defaultConfigFilename;
    /// <summary>
    /// ȱʡ�����ļ���(��·��)
    /// Application.ExecutablePath + ".config"
    /// </summary>
    public static string DefaultConfigFilename
    {
      get
      {
        if (_defaultConfigFilename == null)
          switch (AppConfig.AppType)
          {
            case AppType.Winform:
              _defaultConfigFilename = String.Format("{0}.config", System.Windows.Forms.Application.ExecutablePath);
              break;
            case AppType.Webform:
              _defaultConfigFilename = System.Web.Configuration.WebConfigurationManager.OpenWebConfiguration("~").FilePath;
              break;
          }
        return _defaultConfigFilename;
      }
    }

    private static Configuration _defaultConfiguration;
    private static Configuration DefaultConfiguration
    {
      get
      {
        if (_defaultConfiguration == null)
          switch (AppConfig.AppType)
          {
            case AppType.Winform:
              ExeConfigurationFileMap configFile = new ExeConfigurationFileMap();
              configFile.ExeConfigFilename = DefaultConfigFilename;
              _defaultConfiguration = ConfigurationManager.OpenMappedExeConfiguration(configFile, ConfigurationUserLevel.None);
              break;
            case AppType.Webform:
              _defaultConfiguration = System.Web.Configuration.WebConfigurationManager.OpenWebConfiguration("~");
              break;
          }
        return _defaultConfiguration;
      }
    }

    private static string _configFilename;
    /// <summary>
    /// �����ļ���(��·��)
    /// ȱʡΪ DefaultConfigFilename
    /// </summary>
    public static string ConfigFilename
    {
      get { return _configFilename ?? DefaultConfigFilename; }
      set
      {
        lock (_configurationLock)
        {
          _configuration = null;
          _configFilename = value;
        }
      }
    }

    private static readonly object _configurationLock = new object();
    private static Configuration _configuration;
    private static Configuration Configuration
    {
      get
      {
        if (_configuration == null)
          switch (AppConfig.AppType)
          {
            case AppType.Winform:
              ExeConfigurationFileMap configFile = new ExeConfigurationFileMap();
              configFile.ExeConfigFilename = ConfigFilename;
              _configuration = ConfigurationManager.OpenMappedExeConfiguration(configFile, ConfigurationUserLevel.None);
              break;
            case AppType.Webform:
              _configuration = System.Web.Configuration.WebConfigurationManager.OpenWebConfiguration("~");
              break;
          }
        return _configuration;
      }
    }

    private static readonly byte[] _key = { 211, 90, 226, 153, 182, 179, 209, 78 };
    private static readonly byte[] _iv = { 103, 169, 72, 192, 158, 246, 136, 189 };

    #endregion
    
    #region ����

    private static string Encrypt(string text)
    {
      return Converter.BytesToHexString(DesCryptoTextProvider.Encrypt(_key, _iv, text));
    }

    private static string Decrypt(string text)
    {
      return DesCryptoTextProvider.Decrypt(_key, _iv, Converter.HexStringToBytes(text));
    }

    #region FileSystemWatcher

    //[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Reliability", "CA2000:DisposeObjectsBeforeLosingScope")]
    //private static FileSystemWatcher CreateConfigurationWatcher()
    //{
    //  FileSystemWatcher result = null;
    //  if (AppConfig.AppType != AppType.Webform)
    //  {
    //    Configuration configuration = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
    //    result = new FileSystemWatcher(Path.GetDirectoryName(configuration.FilePath));
    //    result.Filter = Path.GetFileName(configuration.FilePath);
    //    result.NotifyFilter = NotifyFilters.LastWrite;
    //    result.Changed += new FileSystemEventHandler(Configuration_Changed);
    //    result.EnableRaisingEvents = true;
    //  }
    //  return result;
    //}

    //private static void Configuration_Changed(object source, FileSystemEventArgs e)
    //{
    //  lock (_configurationLock)
    //  {
    //    _configuration = null;
    //    ConfigurationManager.RefreshSection(SectionName);
    //  }
    //}

    #endregion

    /// <summary>
    /// ������Ϣ
    /// </summary>
    /// <param name="key">��</param>
    /// <param name="value">ֵ</param>
    /// <param name="inEncrypt">�Ƿ����</param>
    /// <param name="defaultKeyDisabled">����ȱʡID?</param>
    public static void SaveValue(string key, string value, bool inEncrypt, bool defaultKeyDisabled)
    {
      if (!AllowSave)
        return;
      if (String.CompareOrdinal(ReadValue(key, inEncrypt, defaultKeyDisabled), value) == 0)
        return;
      lock (_defaultKeyDisabledLock)
      {
        bool oldDefaultKeyDisabled = _defaultKeyDisabled;
        _defaultKeyDisabled = defaultKeyDisabled;
        try
        {
          lock (_configurationLock)
          {
            if (Configuration.AppSettings.Settings[DefaultKey + key] != null)
              Configuration.AppSettings.Settings.Remove(DefaultKey + key);
            Configuration.AppSettings.Settings.Add(DefaultKey + key, inEncrypt ? Encrypt(value) : value);
            Configuration.Save(ConfigurationSaveMode.Modified);
          }
        }
        finally
        {
          _defaultKeyDisabled = oldDefaultKeyDisabled;
        }
      }
    }

    /// <summary>
    /// ������Ϣ
    /// inEncrypt = false
    /// defaultKeyDisabled = false
    /// </summary>
    /// <param name="key">��</param>
    /// <param name="value">ֵ</param>
    public static void SaveValue(string key, string value)
    {
      SaveValue(key, value, false, false);
    }

    /// <summary>
    /// ������Ϣ
    /// inEncrypt = false
    /// defaultKeyDisabled = false
    /// </summary>
    /// <param name="key">��</param>
    /// <param name="fieldInfo">���ֶ����Ե���Ϣ</param>
    /// <param name="fieldValue">���ֶ����Ե�ֵ</param>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters")]
    public static void SaveValue(string key, FieldInfo fieldInfo, object fieldValue)
    {
      if (fieldValue == null)
        RemoveValue(key + Utilities.GetMemberFullName(fieldInfo), false);
      else
        SaveValue(key + Utilities.GetMemberFullName(fieldInfo), (string)Utilities.ChangeType(fieldValue, typeof(string)), false, false);
    }

    /// <summary>
    /// �Ƴ���Ϣ
    /// </summary>
    /// <param name="key">��</param>
    /// <param name="defaultKeyDisabled">����ȱʡID?</param>
    public static void RemoveValue(string key, bool defaultKeyDisabled)
    {
      if (!AllowSave)
        return;
      lock (_defaultKeyDisabledLock)
      {
        bool oldDefaultKeyDisabled = _defaultKeyDisabled;
        _defaultKeyDisabled = defaultKeyDisabled;
        try
        {
          lock (_configurationLock)
          {
            if (Configuration.AppSettings.Settings[DefaultKey + key] != null)
            {
              Configuration.AppSettings.Settings.Remove(DefaultKey + key);
              Configuration.Save(ConfigurationSaveMode.Modified);
            }
          }
        }
        finally
        {
          _defaultKeyDisabled = oldDefaultKeyDisabled;
        }
      }
    }

    /// <summary>
    /// �Ƴ���Ϣ
    /// defaultKeyDisabled = false
    /// </summary>
    /// <param name="key">��</param>
    public static void RemoveValue(string key)
    {
      RemoveValue(key, false);
    }

    /// <summary>
    /// �Ƴ���Ϣ
    /// defaultKeyDisabled = false
    /// </summary>
    /// <param name="key">��</param>
    /// <param name="fieldInfo">���ֶ����Ե���Ϣ</param>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters")]
    public static void RemoveValue(string key, FieldInfo fieldInfo)
    {
      RemoveValue(key + Utilities.GetMemberFullName(fieldInfo), false);
    }

    /// <summary>
    /// ��ȡ��Ϣ
    /// </summary>
    /// <param name="key">��</param>
    /// <param name="inEncrypt">�Ƿ����</param>
    /// <param name="defaultKeyDisabled">����ȱʡID?</param>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
    public static string ReadValue(string key, bool inEncrypt, bool defaultKeyDisabled)
    {
      string result;
      lock (_defaultKeyDisabledLock)
      {
        bool oldDefaultKeyDisabled = _defaultKeyDisabled;
        _defaultKeyDisabled = defaultKeyDisabled;
        try
        {
          lock (_configurationLock)
          {
            KeyValueConfigurationElement config = Configuration.AppSettings.Settings[DefaultKey + key];
            if (config != null)
              result = config.Value;
            else if (String.CompareOrdinal(ConfigFilename, DefaultConfigFilename) != 0)
            {
              config = DefaultConfiguration.AppSettings.Settings[DefaultKey + key];
              if (config != null)
                result = config.Value;
              else
                return null;
            }
            else
              return null;
          }
        }
        finally
        {
          _defaultKeyDisabled = oldDefaultKeyDisabled;
        }
      }
      if (inEncrypt)
        try
        {
          return Decrypt(result);
        }
        catch (Exception ex)
        {
          EventLog.SaveLocal(MethodBase.GetCurrentMethod(), key, ex);
          return null;
        }
      return result;
    }

    /// <summary>
    /// ��ȡ��Ϣ
    /// inEncrypt = false
    /// defaultKeyDisabled = false
    /// </summary>
    /// <param name="key">��</param>
    public static string ReadValue(string key)
    {
      return ReadValue(key, false, false);
    }

    /// <summary>
    /// ��ȡ��Ϣ
    /// inEncrypt = false
    /// defaultKeyDisabled = false
    /// </summary>
    /// <param name="key">��</param>
    /// <param name="fieldInfo">���ֶ����Ե���Ϣ</param>
    public static object ReadValue(string key, FieldInfo fieldInfo)
    {
      if (fieldInfo == null)
        return null;
      string s = ReadValue(key + Utilities.GetMemberFullName(fieldInfo), false, false);
      if (string.IsNullOrEmpty(s))
        return null;
      return Utilities.ChangeType(s, fieldInfo.FieldType); 
    }

    #region Property

    /// <summary>
    /// ��������ֵ
    /// key = null
    /// allowSave = true
    /// inEncrypt = false
    /// </summary>
    /// <param name="field">�����ֶ�</param>
    /// <param name="newValue">��ֵ</param>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1045:DoNotPassTypesByReference")]
    public static void SetProperty<TField, TValue>(ref TField field, TValue newValue)
    {
      if (IsEquality(field, newValue))
        return;
      DoSetProperty(new System.Diagnostics.StackTrace().GetFrame(1).GetMethod(), null, ref field, newValue, true, false);
    }

    /// <summary>
    /// ��������ֵ
    /// key = null
    /// inEncrypt = false
    /// </summary>
    /// <param name="field">�����ֶ�</param>
    /// <param name="newValue">��ֵ</param>
    /// <param name="allowSave">������״̬��Ϣ?</param>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1045:DoNotPassTypesByReference")]
    public static void SetProperty<TField, TValue>(ref TField field, TValue newValue, bool allowSave)
    {
      if (IsEquality(field, newValue))
        return;
      DoSetProperty(new System.Diagnostics.StackTrace().GetFrame(1).GetMethod(), null, ref field, newValue, allowSave, false);
    }

    /// <summary>
    /// ��������ֵ
    /// key = null
    /// </summary>
    /// <param name="field">�����ֶ�</param>
    /// <param name="newValue">��ֵ</param>
    /// <param name="allowSave">������״̬��Ϣ?</param>
    /// <param name="inEncrypt">�Ƿ����</param>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1045:DoNotPassTypesByReference")]
    public static void SetProperty<TField, TValue>(ref TField field, TValue newValue, bool allowSave, bool inEncrypt)
    {
      if (IsEquality(field, newValue))
        return;
      DoSetProperty(new System.Diagnostics.StackTrace().GetFrame(1).GetMethod(), null, ref field, newValue, allowSave, inEncrypt);
    }

    /// <summary>
    /// ��������ֵ
    /// allowSave = true
    /// inEncrypt = false
    /// </summary>
    /// <param name="key">��</param>
    /// <param name="field">�����ֶ�</param>
    /// <param name="newValue">��ֵ</param>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1045:DoNotPassTypesByReference")]
    public static void SetProperty<TField, TValue>(string key, ref TField field, TValue newValue)
    {
      if (IsEquality(field, newValue))
        return;
      DoSetProperty(new System.Diagnostics.StackTrace().GetFrame(1).GetMethod(), key, ref field, newValue, true, false);
    }

    /// <summary>
    /// ��������ֵ
    /// inEncrypt = false
    /// </summary>
    /// <param name="key">��</param>
    /// <param name="field">�����ֶ�</param>
    /// <param name="newValue">��ֵ</param>
    /// <param name="allowSave">������״̬��Ϣ?</param>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1045:DoNotPassTypesByReference")]
    public static void SetProperty<TField, TValue>(string key, ref TField field, TValue newValue, bool allowSave)
    {
      if (IsEquality(field, newValue))
        return;
      DoSetProperty(new System.Diagnostics.StackTrace().GetFrame(1).GetMethod(), key, ref field, newValue, allowSave, false);
    }

    /// <summary>
    /// ��������ֵ
    /// </summary>
    /// <param name="key">��</param>
    /// <param name="field">�����ֶ�</param>
    /// <param name="newValue">��ֵ</param>
    /// <param name="allowSave">������״̬��Ϣ?</param>
    /// <param name="inEncrypt">�Ƿ����</param>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1045:DoNotPassTypesByReference")]
    public static void SetProperty<TField, TValue>(string key, ref TField field, TValue newValue, bool allowSave, bool inEncrypt)
    {
      if (IsEquality(field, newValue))
        return;
      DoSetProperty(new System.Diagnostics.StackTrace().GetFrame(1).GetMethod(), key, ref field, newValue, allowSave, inEncrypt);
    }

    private static bool IsEquality<TField, TValue>(TField field, TValue newValue)
    {
      if (typeof(TField).IsClass && typeof(TValue).IsClass)
        return object.Equals(field, newValue);
      string fieldString = (string)Utilities.ChangeType(field, typeof(string));
      string newValueString = (string)Utilities.ChangeType(newValue, typeof(string));
      return String.CompareOrdinal(fieldString, newValueString) == 0;
    }

    private static void DoSetProperty<TField, TValue>(MethodBase method, string key, ref TField field, TValue newValue, bool allowSave, bool inEncrypt)
    {
      if (allowSave)
        SaveValue(String.Format("{0}.{1}.{2}", method.DeclaringType.FullName, method.Name.Substring(4), key), (string)Utilities.ChangeType(newValue, typeof(string)), inEncrypt, true);
      field = (TField)Utilities.ChangeType(newValue, typeof(TField));
    }

    /// <summary>
    /// ��ȡ����ֵ
    /// key = null
    /// allowSave = true
    /// inEncrypt = false
    /// </summary>
    /// <param name="field">�����ֶ�</param>
    /// <param name="defaultValue">ȱʡֵ</param>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1045:DoNotPassTypesByReference")]
    public static TValue GetProperty<TField, TValue>(ref TField field, TValue defaultValue)
    {
      object result = null;
      if (TryGetProperty<TField, TValue>(field, ref result))
        return (TValue)result;
      return DoGetProperty(new System.Diagnostics.StackTrace().GetFrame(1).GetMethod(), null, ref field, defaultValue, true, false);
    }

    /// <summary>
    /// ��ȡ����ֵ
    /// key = null
    /// inEncrypt = false
    /// </summary>
    /// <param name="field">�����ֶ�</param>
    /// <param name="defaultValue">ȱʡֵ</param>
    /// <param name="allowSave">������״̬��Ϣ?</param>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1045:DoNotPassTypesByReference")]
    public static TValue GetProperty<TField, TValue>(ref TField field, TValue defaultValue, bool allowSave)
    {
      object result = null;
      if (TryGetProperty<TField, TValue>(field, ref result))
        return (TValue)result;
      return DoGetProperty(new System.Diagnostics.StackTrace().GetFrame(1).GetMethod(), null, ref field, defaultValue, allowSave, false);
    }

    /// <summary>
    /// ��ȡ����ֵ
    /// key = null
    /// </summary>
    /// <param name="field">�����ֶ�</param>
    /// <param name="defaultValue">ȱʡֵ</param>
    /// <param name="allowSave">������״̬��Ϣ?</param>
    /// <param name="inEncrypt">�Ƿ����</param>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1045:DoNotPassTypesByReference")]
    public static TValue GetProperty<TField, TValue>(ref TField field, TValue defaultValue, bool allowSave, bool inEncrypt)
    {
      object result = null;
      if (TryGetProperty<TField, TValue>(field, ref result))
        return (TValue)result;
      return DoGetProperty(new System.Diagnostics.StackTrace().GetFrame(1).GetMethod(), null, ref field, defaultValue, allowSave, inEncrypt);
    }

    /// <summary>
    /// ��ȡ����ֵ
    /// allowSave = true
    /// inEncrypt = false
    /// </summary>
    /// <param name="key">��</param>
    /// <param name="field">�����ֶ�</param>
    /// <param name="defaultValue">ȱʡֵ</param>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1045:DoNotPassTypesByReference")]
    public static TValue GetProperty<TField, TValue>(string key, ref TField field, TValue defaultValue)
    {
      object result = null;
      if (TryGetProperty<TField, TValue>(key, field, ref result))
        return (TValue)result;
      return DoGetProperty(new System.Diagnostics.StackTrace().GetFrame(1).GetMethod(), key, ref field, defaultValue, true, false);
    }

    /// <summary>
    /// ��ȡ����ֵ
    /// inEncrypt = false
    /// </summary>
    /// <param name="key">��</param>
    /// <param name="field">�����ֶ�</param>
    /// <param name="defaultValue">ȱʡֵ</param>
    /// <param name="allowSave">������״̬��Ϣ?</param>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1045:DoNotPassTypesByReference")]
    public static TValue GetProperty<TField, TValue>(string key, ref TField field, TValue defaultValue, bool allowSave)
    {
      object result = null;
      if (TryGetProperty<TField, TValue>(key, field, ref result))
        return (TValue)result;
      return DoGetProperty(new System.Diagnostics.StackTrace().GetFrame(1).GetMethod(), key, ref field, defaultValue, allowSave, false);
    }

    /// <summary>
    /// ��ȡ����ֵ
    /// </summary>
    /// <param name="key">��</param>
    /// <param name="field">�����ֶ�</param>
    /// <param name="defaultValue">ȱʡֵ</param>
    /// <param name="allowSave">������״̬��Ϣ?</param>
    /// <param name="inEncrypt">�Ƿ����</param>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1045:DoNotPassTypesByReference")]
    public static TValue GetProperty<TField, TValue>(string key, ref TField field, TValue defaultValue, bool allowSave, bool inEncrypt)
    {
      object result = null;
      if (TryGetProperty<TField, TValue>(key, field, ref result))
        return (TValue)result;
      return DoGetProperty(new System.Diagnostics.StackTrace().GetFrame(1).GetMethod(), key, ref field, defaultValue, allowSave, inEncrypt);
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

    private static TValue DoGetProperty<TField, TValue>(MethodBase method, string key, ref TField field, TValue defaultValue, bool allowSave, bool inEncrypt)
    {
      string value = ReadValue(String.Format("{0}.{1}.{2}", method.DeclaringType.FullName, method.Name.Substring(4), key), inEncrypt, true);
      if (value != null)
        try
        {
          field = (TField)Utilities.ChangeType(value, typeof(TField));
          return (TValue)Utilities.ChangeType(value, typeof(TValue));
        }
        catch (InvalidCastException ex)
        {
          EventLog.SaveLocal(MethodBase.GetCurrentMethod(), value, ex);
        }
      DoSetProperty(method, key, ref field, defaultValue, allowSave, inEncrypt);
      return defaultValue;
    }

    #endregion

    #endregion
  }
}