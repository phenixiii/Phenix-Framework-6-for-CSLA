using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Phenix.Core.Mapping;
using Phenix.Core.Reflection;

namespace Phenix.Core.Dictionary
{
  /// <summary>
  /// 业务码占位符格式
  /// </summary> 
  public class BusinessCodePlaceholderFormat
  {
    internal BusinessCodePlaceholderFormat()
    {
      _formatString = DEFAULT_FORMAT_STRING;
    }

    private BusinessCodePlaceholderFormat(BusinessCodeFormat owner, string formatString)
    {
      _owner = owner;
      _formatString = formatString;
    }
    
    #region 工厂

    internal static IList<BusinessCodePlaceholderFormat> Fetch(BusinessCodeFormat owner)
    {
      Regex regex = new Regex(REGEX_PATTERN, RegexOptions.IgnoreCase | RegexOptions.Multiline | RegexOptions.Compiled);
      MatchCollection regexMatches = regex.Matches(owner.FormatString);
      List<BusinessCodePlaceholderFormat> result = new List<BusinessCodePlaceholderFormat>(regexMatches.Count);
      foreach (Match item in regexMatches)
        result.Add(new BusinessCodePlaceholderFormat(owner, String.Format("({0})", item.Value)));
      return result.AsReadOnly();
    }
    
    #endregion

    #region 属性
    
    /// <summary>
    /// 正则表达式模式
    /// </summary>
    public const string REGEX_PATTERN = "PH:[_a-zA-Z0-9=,]*";

    /// <summary>
    /// 缺省格式字符串
    /// </summary>
    public const string DEFAULT_FORMAT_STRING = "(PH:L=5,P=?)";

    /// <summary>
    /// 固定长度标记
    /// </summary>
    public const string LENGTH_TAG = "L=";

    /// <summary>
    /// 固定长度正则表达式模式
    /// </summary>
    public const string LENGTH_REGEX_PATTERN =  LENGTH_TAG + @"\d+";

    /// <summary>
    /// 属性名标记
    /// </summary>
    public const string PROPERTY_NAME_TAG = "P=";

    /// <summary>
    /// 属性名正则表达式模式
    /// </summary>
    public const string PROPERTY_NAME_REGEX_PATTERN = PROPERTY_NAME_TAG + "[_a-zA-Z0-9]*";

    private readonly BusinessCodeFormat _owner;
    /// <summary>
    /// 拥有者
    /// </summary>
    [System.ComponentModel.Browsable(false)]
    [System.ComponentModel.DataAnnotations.Display(AutoGenerateField = false)]
    public BusinessCodeFormat Owner
    {
      get { return _owner; }
    }

    private string _formatString;
    /// <summary>
    /// 格式串
    /// </summary>
    public string FormatString
    {
      get { return _formatString; }
    }

    private int? _length;
    /// <summary>
    /// 固定长度
    /// </summary>
    public int Length
    {
      get
      {
        if (!_length.HasValue)
        {
          Match match = Regex.Match(FormatString, LENGTH_REGEX_PATTERN);
          _length = match.Success ? Int32.Parse(match.Value.Replace(LENGTH_TAG, String.Empty)) : 5;
        }
        return _length.Value;
      }
      set
      {
        if (Length == value)
          return;

        _length = value;

        ResetFormatString();
      }
    }

    private string _propertyName;
    /// <summary>
    /// 属性名
    /// </summary>
    public string PropertyName
    {
      get
      {
        if (_propertyName == null)
        {
          Match match = Regex.Match(FormatString, PROPERTY_NAME_REGEX_PATTERN);
          _propertyName = match.Success ? match.Value.Replace(PROPERTY_NAME_TAG, String.Empty) : String.Empty;
        }
        return _propertyName;
      }
      set
      {
        if (PropertyName == value)
          return;

        _propertyName = value;

        ResetFormatString();
      }
    }

    #endregion

    #region 方法

    private void ResetFormatString()
    {
      _formatString = String.Format("(PH:{0}{1},{2}{3})", LENGTH_TAG, _length, PROPERTY_NAME_TAG, _propertyName);
    }

    internal string GetBusinessCodePropertyValue(object obj)
    {
      if (obj != null)
      {
        FieldMapInfo fieldMapInfo = ClassMemberHelper.DoGetFieldMapInfo(obj.GetType(), PropertyName);
        if (fieldMapInfo != null)
          return TidyValue(fieldMapInfo.GetValue(obj));
      }
      return TidyValue(null);
    }

    internal string GetBusinessCodePropertyValue(IDictionary<string, object> propertyValues)
    {
      object propertyValue;
      return TidyValue(propertyValues.TryGetValue(PropertyName, out propertyValue) ? propertyValue : null);
    }

    private string TidyValue(object value)
    {
      if (value != null)
      {
        string result = (string)Utilities.ChangeType(value, typeof(string)) ?? String.Empty;
        return result.Length <= Length
          ? result.PadRight(Length)
          : result.Substring(0, Length);
      }
      return String.Empty.PadRight(Length);
    }

    #endregion
  }
}