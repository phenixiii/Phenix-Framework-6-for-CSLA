using System;
using System.Collections.Generic;
using System.Data.Common;
using Phenix.Core.Data;
using Phenix.Core.Dictionary.Windows;
using Phenix.Core.Mapping;
using Phenix.Core.Operate;
using Phenix.Core.Rule;
using Phenix.Core.Security;

namespace Phenix.Core.Dictionary
{
  /// <summary>
  /// 业务码格式
  /// </summary>
  [Serializable]
  public sealed class BusinessCodeFormat
  {
    internal BusinessCodeFormat(FieldMapInfo fieldMapInfo, bool checkCriteriaPropertyName)
    {
      _name = fieldMapInfo.BusinessCodeName;
      if (checkCriteriaPropertyName)
        _criteriaPropertyName = fieldMapInfo.BusinessCodeCriteriaPropertyName;
      _caption = fieldMapInfo.FullFriendlyName;
      _defaultFormatString = fieldMapInfo.BusinessCodeDefaultFormat;
    }

    /// <summary>
    /// 初始化
    /// </summary>
    /// <param name="businessCodeName">业务码名称</param>
    /// <param name="caption">标签</param>
    public BusinessCodeFormat(string businessCodeName, string caption)
    {
      int i = businessCodeName.IndexOf(AppConfig.EQUAL_SEPARATOR);
      int j = businessCodeName.IndexOf(AppConfig.VALUE_SEPARATOR);
      if (i == -1 || j == -1)
        _name = businessCodeName;
      else
      {
        //格式: name + ','+ criteriaPropertyName + '=' + criteriaPropertyValue
        _criteriaPropertyValue = businessCodeName.Remove(0, i + 1);
        businessCodeName = businessCodeName.Substring(0, i);
        _criteriaPropertyName = businessCodeName.Remove(0, j + 1);
        _name = businessCodeName.Substring(0, j);
      }
      _caption = caption;
    }

    /// <summary>
    /// 初始化
    /// </summary>
    /// <param name="businessCodeName">业务码名称</param>
    /// <param name="caption">标签</param>
    /// <param name="formatString">格式字符串</param>
    /// <param name="fillOnSaving">是否提交时填充值</param>
    public BusinessCodeFormat(string businessCodeName, string caption, string formatString, bool fillOnSaving)
      : this(businessCodeName, caption)
    {
      _formatString = formatString;
      _fillOnSaving = fillOnSaving;
    }

    #region 工厂

    /// <summary>
    /// 构建业务码
    /// </summary>
    /// <param name="businessCodeName">业务码名称</param>
    public static BusinessCodeFormat Fetch(string businessCodeName)
    {
      return DataDictionaryHub.GetBusinessCodeFormat(businessCodeName);
    }

    /// <summary>
    /// 构建业务码
    /// </summary>
    /// <param name="propertyInfo">属性信息</param>
    public static BusinessCodeFormat Fetch(IPropertyInfo propertyInfo)
    {
      return Fetch(null, propertyInfo);
    }

    /// <summary>
    /// 构建业务码
    /// </summary>
    /// <param name="obj">对象</param>
    /// <param name="propertyInfo">属性信息</param>
    public static BusinessCodeFormat Fetch(object obj, IPropertyInfo propertyInfo)
    {
      CodingStandards.CheckFieldMapInfo(propertyInfo);
      return Fetch(obj, propertyInfo.FieldMapInfo, false);
    }

    internal static BusinessCodeFormat Fetch(object obj, FieldMapInfo fieldMapInfo, bool throwIfNotFound)
    {
      return Fetch(fieldMapInfo.GetBusinessCodeName(obj), fieldMapInfo, throwIfNotFound);
    }

    internal static BusinessCodeFormat Fetch(IDictionary<string, object> propertyValues, FieldMapInfo fieldMapInfo, bool throwIfNotFound)
    {
      return Fetch(fieldMapInfo.GetBusinessCodeName(propertyValues), fieldMapInfo, throwIfNotFound);
    }

    private static BusinessCodeFormat Fetch(string businessCodeName, FieldMapInfo fieldMapInfo, bool throwIfNotFound)
    {
      BusinessCodeFormat result = Fetch(businessCodeName);
      if (result != null)
        result.OwnerType = fieldMapInfo.OwnerType;
      else if (throwIfNotFound)
        throw new BusinessCodeFormatException(businessCodeName);
      return result;
    }

    #endregion

    #region 属性

    private string _ownerTypeAssemblyQualifiedName;
    [NonSerialized]
    private Type _ownerType;
    /// <summary>
    /// 所属类
    /// </summary>
    [System.ComponentModel.Browsable(false)]
    [System.ComponentModel.DataAnnotations.Display(AutoGenerateField = false)]
    public Type OwnerType
    {
      get
      {
        if (_ownerType == null)
        {
          if (_ownerTypeAssemblyQualifiedName != null)
            _ownerType = Type.GetType(_ownerTypeAssemblyQualifiedName);
        }
        return _ownerType;
      }
      private set
      {
        _ownerTypeAssemblyQualifiedName = value != null ? value.AssemblyQualifiedName : null;
        _ownerType = value;
      }
    }

    private readonly string _defaultFormatString;
    /// <summary>
    /// 业务码缺省格式字符串
    /// </summary>
    public string DefaultFormatString
    {
      get { return _defaultFormatString; }
    }

    private readonly string _name;
    /// <summary>
    /// 业务码名称
    /// </summary>
    public string BusinessCodeName
    {
      get { return FormatBusinessCodeName(_name, CriteriaPropertyName, CriteriaPropertyValue); }
    }

    private readonly string _criteriaPropertyName;
    /// <summary>
    /// 条件属性名称
    /// </summary>
    public string CriteriaPropertyName
    {
      get { return _criteriaPropertyName; }
    }

    /// <summary>
    /// 是否缺省
    /// </summary>
    public bool IsDefault
    {
      get { return String.IsNullOrEmpty(_criteriaPropertyName); }
    }

    private string _criteriaPropertyValue;
    /// <summary>
    /// 条件属性值
    /// </summary>
    public string CriteriaPropertyValue
    {
      get { return _criteriaPropertyValue; }
      set { _criteriaPropertyValue = value; }
    }

    private readonly string _caption;
    /// <summary>
    /// 标签
    /// </summary>
    public string Caption
    {
      get { return _caption; }
    }

    private string _formatString;
    /// <summary>
    /// 格式字符串
    /// </summary>
    public string FormatString
    {
      get { return _formatString; }
      set
      {
        if (!String.IsNullOrEmpty(value))
          value = value.Replace('\n', ' ').Replace('\r', ' ');
        if (String.CompareOrdinal(_formatString, value) == 0) 
          return;
        _formatString = value;
        _serialFormats = null;
      }
    }

    private bool _fillOnSaving;
    /// <summary>
    /// 是否提交时填充值
    /// </summary>
    public bool FillOnSaving
    {
      get { return _fillOnSaving; }
      set { _fillOnSaving = value; }
    }

    [NonSerialized]
    private IList<BusinessCodeSerialFormat> _serialFormats;
    /// <summary>
    /// 业务码流水号格式队列
    /// </summary>
    [System.ComponentModel.Browsable(false)]
    [System.ComponentModel.DataAnnotations.Display(AutoGenerateField = false)]
    public IList<BusinessCodeSerialFormat> SerialFormats
    {
      get
      {
        if (_serialFormats == null)
          _serialFormats = BusinessCodeSerialFormat.Fetch(this);
        return _serialFormats;
      }
    }

    [NonSerialized]
    private IList<BusinessCodePlaceholderFormat> _placeholderFormats;
    /// <summary>
    /// 业务码占位符格式队列
    /// </summary>
    [System.ComponentModel.Browsable(false)]
    [System.ComponentModel.DataAnnotations.Display(AutoGenerateField = false)]
    public IList<BusinessCodePlaceholderFormat> PlaceholderFormats
    {
      get
      {
        if (_placeholderFormats == null)
          _placeholderFormats = BusinessCodePlaceholderFormat.Fetch(this);
        return _placeholderFormats;
      }
    }

    #endregion

    #region 方法

    internal static string FormatBusinessCodeName(string name, string criteriaPropertyName, string criteriaPropertyValue)
    {
      if (String.IsNullOrEmpty(criteriaPropertyName))
        return name;
      //格式: name + ','+ criteriaPropertyName + '=' + criteriaPropertyValue
      return String.Format("{0}{1}{2}{3}{4}", name, AppConfig.VALUE_SEPARATOR, criteriaPropertyName, AppConfig.EQUAL_SEPARATOR, criteriaPropertyValue);
    }

    /// <summary>
    /// 提取名称
    /// </summary>
    /// <param name="businessCodeName">业务码名称</param>
    public static string ExtractName(string businessCodeName)
    {
      int i = businessCodeName.IndexOf(AppConfig.VALUE_SEPARATOR);
      return i != -1 ? businessCodeName.Remove(i) : businessCodeName;
    }

    /// <summary>
    /// 编辑
    /// </summary>
    public bool Edit()
    {
      return BusinessCodeFormatEditDialog.Execute(this) != null;
    }

    /// <summary>
    /// 编辑
    /// </summary>
    public bool Edit(KeyCaptionCollection criteriaPropertySelectValues)
    {
      return BusinessCodeFormatEditDialog.Execute(this, criteriaPropertySelectValues) != null;
    }

    /// <summary>
    /// 编辑
    /// </summary>
    public bool Edit(EnumKeyCaptionCollection criteriaPropertySelectValues)
    {
      return BusinessCodeFormatEditDialog.Execute(this, criteriaPropertySelectValues) != null;
    }

    /// <summary>
    /// 编辑
    /// </summary>
    public bool Edit(Type ownerType)
    {
      return BusinessCodeFormatEditDialog.Execute(this, ownerType) != null;
    }

    /// <summary>
    /// 编辑
    /// </summary>
    public bool Edit(Type ownerType, KeyCaptionCollection criteriaPropertySelectValues)
    {
      return BusinessCodeFormatEditDialog.Execute(this, ownerType, criteriaPropertySelectValues) != null;
    }

    /// <summary>
    /// 编辑
    /// </summary>
    public bool Edit(Type ownerType,  EnumKeyCaptionCollection criteriaPropertySelectValues)
    {
      return BusinessCodeFormatEditDialog.Execute(this, ownerType, criteriaPropertySelectValues) != null;
    }

    /// <summary>
    /// 编辑
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
    public bool Edit<T>()
      where T : IEntity
    {
      return Edit(typeof(T));
    }

    /// <summary>
    /// 编辑
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
    public bool Edit<T>(KeyCaptionCollection criteriaPropertySelectValues)
      where T : IEntity
    {
      return Edit(typeof(T), criteriaPropertySelectValues);
    }

    /// <summary>
    /// 编辑
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
    public bool Edit<T>(EnumKeyCaptionCollection criteriaPropertySelectValues)
      where T : IEntity
    {
      return Edit(typeof(T), criteriaPropertySelectValues);
    }

    /// <summary>
    /// 保存
    /// </summary>
    public void Save()
    {
      DataDictionaryHub.SetBusinessCodeFormat(this);
    }

    /// <summary>
    /// 获取业务码(忽略属性名标记的占位符)
    /// </summary>
    public string GetValue()
    {
      return GetValue(null);
    }

    /// <summary>
    /// 获取业务码(忽略属性名标记的占位符)
    /// </summary>
    public string GetValue(DbConnection connection)
    {
      return RelacePlaceholder((object)null, GetSemisBusinessCode(connection, UserIdentity.CurrentIdentity));
    }

    /// <summary>
    /// 获取业务码(忽略属性名标记的占位符)
    /// </summary>
    public string[] GetValues(int count)
    {
      return GetValues(null, count);
    }

    /// <summary>
    /// 获取业务码(忽略属性名标记的占位符)
    /// </summary>
    public string[] GetValues(DbConnection connection, int count)
    {
      List<string> result = new List<string>(count);
      foreach(string s in GetSemisBusinessCodes(connection, count, UserIdentity.CurrentIdentity))
        result.Add(RelacePlaceholder((object)null, s));
      return result.ToArray();
    }

    private string GetSemisBusinessCode(DateTime now, UserIdentity identity)
    {
      string result = FormatString;
      foreach (EnumKeyCaption item in EnumKeyCaptionCollection.Fetch<BusinessCodeFormatItemType>())
        switch ((BusinessCodeFormatItemType)item.Value)
        {
          case BusinessCodeFormatItemType.LengthYear:
            result = result.Replace(item.Key, now.ToString("yyyy"));
            break;
          case BusinessCodeFormatItemType.ShortYear:
            result = result.Replace(item.Key, now.ToString("yy"));
            break;
          case BusinessCodeFormatItemType.Month:
            result = result.Replace(item.Key, now.ToString("MM"));
            break;
          case BusinessCodeFormatItemType.Day:
            result = result.Replace(item.Key, now.ToString("dd"));
            break;
          case BusinessCodeFormatItemType.Department:
            result = result.Replace(item.Key, identity != null && identity.Department != null ? identity.Department.Code : String.Empty);
            break;
          case BusinessCodeFormatItemType.UserNumber:
            result = result.Replace(item.Key, identity != null ? identity.UserNumber : String.Empty);
            break;
        }
      return result;
    }

    private string GetSemisBusinessCode(DbConnection connection, UserIdentity identity)
    {
      try
      {
        DateTime now = DateTime.Now;
        string result = GetSemisBusinessCode(now, identity);
        if (connection != null)
          foreach (BusinessCodeSerialFormat item in SerialFormats)
            result = result.Replace(item.FormatString, item.TidyValue(DataHub.GetBusinessCodeSerial(connection, item.GetKey(now, identity), item.InitialValue).ToString()));
        else
          foreach (BusinessCodeSerialFormat item in SerialFormats)
            result = result.Replace(item.FormatString, item.TidyValue(DataHub.GetBusinessCodeSerial(item.GetKey(now, identity), item.InitialValue, identity).ToString()));
        return result;
      }
      catch (Exception ex)
      {
        throw new BusinessCodeFormatException(this, ex);
      }
    }

    private IList<string> GetSemisBusinessCodes(DbConnection connection, int count, UserIdentity identity)
    {
      try
      {
        List<string> result = new List<string>(count);
        DateTime now = DateTime.Now;
        string semisBusinessCode = GetSemisBusinessCode(now, identity);
        for (int i = 0; i < count; i++)
          result.Add(semisBusinessCode);
        if (connection != null)
          foreach (BusinessCodeSerialFormat item in SerialFormats)
          {
            long[] businessCodeSerials = DataHub.GetBusinessCodeSerials(connection, item.GetKey(now, identity), item.InitialValue, count);
            for (int i = 0; i < count; i++)
              result[i] = result[i].Replace(item.FormatString, item.TidyValue(businessCodeSerials[i].ToString()));
          }
        else
          foreach (BusinessCodeSerialFormat item in SerialFormats)
          {
            long[] businessCodeSerials = DataHub.GetBusinessCodeSerials(item.GetKey(now, identity), item.InitialValue, count, identity);
            for (int i = 0; i < count; i++)
              result[i] = result[i].Replace(item.FormatString, item.TidyValue(businessCodeSerials[i].ToString()));
          }
        return result;
      }
      catch (Exception ex)
      {
        throw new BusinessCodeFormatException(this, ex);
      }
    }

    private string RelacePlaceholder(object source, string semisBusinessCode)
    {
      try
      {
        string result = semisBusinessCode;
        foreach (BusinessCodePlaceholderFormat item in PlaceholderFormats)
          result = result.Replace(item.FormatString, item.GetBusinessCodePropertyValue(source));
        return result;
      }
      catch (Exception ex)
      {
        throw new BusinessCodeFormatException(this, ex);
      }
    }

    internal static string RelacePlaceholder(object source, FieldMapInfo fieldMapInfo)
    {
      BusinessCodeFormat format = Fetch(source, fieldMapInfo, true);
      return format.RelacePlaceholder(source, format.GetValue());
    }

    internal static string RelacePlaceholder(object source, FieldMapInfo fieldMapInfo, string semisBusinessCode)
    {
      BusinessCodeFormat format = Fetch(source, fieldMapInfo, true);
      return format.RelacePlaceholder(source, semisBusinessCode);
    }

    private string RelacePlaceholder(IDictionary<string, object> propertyValues, string semisBusinessCode)
    {
      try
      {
        string result = semisBusinessCode;
        foreach (BusinessCodePlaceholderFormat item in PlaceholderFormats)
          result = result.Replace(item.FormatString, item.GetBusinessCodePropertyValue(propertyValues));
        return result;
      }
      catch (Exception ex)
      {
        throw new BusinessCodeFormatException(this, ex);
      }
    }

    internal static string RelacePlaceholder(IDictionary<string, object> propertyValues, FieldMapInfo fieldMapInfo)
    {
      BusinessCodeFormat format = Fetch(propertyValues, fieldMapInfo, true);
      return format.RelacePlaceholder(propertyValues, format.GetValue());
    }

    #endregion
  }
}
