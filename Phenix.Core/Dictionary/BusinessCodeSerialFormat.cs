using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using Phenix.Core.Rule;
using Phenix.Core.Security;

namespace Phenix.Core.Dictionary
{
  /// <summary>
  /// 业务码流水号格式
  /// </summary> 
  public class BusinessCodeSerialFormat
  {
    internal BusinessCodeSerialFormat()
    {
      _formatString = DEFAULT_FORMAT_STRING;
    }

    private BusinessCodeSerialFormat(BusinessCodeFormat owner, string formatString)
    {
      _owner = owner;
      _formatString = formatString;
    }
    
    #region 工厂

    internal static IList<BusinessCodeSerialFormat> Fetch(BusinessCodeFormat owner)
    {
      Regex regex = new Regex(REGEX_PATTERN, RegexOptions.IgnoreCase | RegexOptions.Multiline | RegexOptions.Compiled);
      MatchCollection regexMatches = regex.Matches(owner.FormatString);
      List<BusinessCodeSerialFormat> result = new List<BusinessCodeSerialFormat>(regexMatches.Count);
      foreach (Match item in regexMatches)
        result.Add(new BusinessCodeSerialFormat(owner, String.Format("({0})", item.Value)));
      return result.AsReadOnly();
    }
    
    #endregion

    #region 属性
    
    /// <summary>
    /// 正则表达式模式
    /// </summary>
    public const string REGEX_PATTERN = "SN:[a-zA-Z0-9=,]*";

    /// <summary>
    /// 缺省格式字符串
    /// </summary>
    public const string DEFAULT_FORMAT_STRING = "(SN:L=3,S=1,C=D)";

    /// <summary>
    /// 固定长度标记
    /// </summary>
    public const string LENGTH_TAG = "L=";

    /// <summary>
    /// 固定长度正则表达式模式
    /// </summary>
    public const string LENGTH_REGEX_PATTERN =  LENGTH_TAG + @"\d+";

    /// <summary>
    /// 起始值标记
    /// </summary>
    public const string INITIAL_VALUE_TAG = "S=";

    /// <summary>
    /// 起始值正则表达式模式
    /// </summary>
    public const string INITIAL_VALUE_REGEX_PATTERN = INITIAL_VALUE_TAG + @"\d+";

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

    /// <summary>
    /// 键
    /// </summary>
    public string Key
    {
      get { return String.Format("{0}.{1}=", Owner.BusinessCodeName, FormatString); }
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
          _length = match.Success ? Int32.Parse(match.Value.Replace(LENGTH_TAG, String.Empty)) : 3;
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

    private long? _initialValue;
    /// <summary>
    /// 起始值
    /// </summary>
    public long InitialValue
    {
      get
      {
        if (!_initialValue.HasValue)
        {
          Match match = Regex.Match(FormatString, INITIAL_VALUE_REGEX_PATTERN);
          _initialValue = match.Success ? Int64.Parse(match.Value.Replace(INITIAL_VALUE_TAG, String.Empty)) : 1;
        }
        return _initialValue.Value;
      }
      set
      {
        if (InitialValue == value)
          return;

        _initialValue = value;

        ResetFormatString();
      }
    }

    private BusinessCodeSerialGroupMode? _groupMode;
    /// <summary>
    /// 分组模式
    /// </summary>
    public BusinessCodeSerialGroupMode GroupMode
    {
      get
      {
        if (!_groupMode.HasValue)
        {
          _groupMode = BusinessCodeSerialGroupMode.None;
          foreach (EnumKeyCaption item in EnumKeyCaptionCollection.Fetch<BusinessCodeSerialGroupMode>())
            if (FormatString.IndexOf(item.Key, System.StringComparison.Ordinal) != -1)
            {
              _groupMode = (BusinessCodeSerialGroupMode)item.Value;
              break;
            }
        }
        return _groupMode.Value;
      }
      set
      {
        if (GroupMode == value)
          return;

        _groupMode = value;
        
        ResetFormatString();
      }
    }

    private BusinessCodeSerialResetCycle? _resetCycle;
    /// <summary>
    /// 重置周期
    /// </summary>
    public BusinessCodeSerialResetCycle ResetCycle 
    {
      get
      {
        if (!_resetCycle.HasValue)
        {
          _resetCycle = BusinessCodeSerialResetCycle.Day;
          foreach (EnumKeyCaption item in EnumKeyCaptionCollection.Fetch<BusinessCodeSerialResetCycle>())
            if (FormatString.IndexOf(item.Key, System.StringComparison.Ordinal) != -1)
            {
              _resetCycle = (BusinessCodeSerialResetCycle)item.Value;
              break;
            }
        }
        return _resetCycle.Value;
      }
      set
      {
        if (ResetCycle == value)
          return;

        _resetCycle = value;

        ResetFormatString();
      }
    }

    #endregion

    #region 方法

    private void ResetFormatString()
    {
      _formatString = String.Format("(SN:{0}{1},{2}{3},{4},{5})",
        LENGTH_TAG, _length, INITIAL_VALUE_TAG, _initialValue, EnumKeyCaption.GetKey(_groupMode), EnumKeyCaption.GetKey(_resetCycle));
    }

    internal string GetKey(DateTime now, UserIdentity identity)
    {
      StringBuilder key = new StringBuilder(Key);
      switch (GroupMode)
      {
        case BusinessCodeSerialGroupMode.Department:
          key.Append(identity != null ? identity.DepartmentId : null);
          break;
        case BusinessCodeSerialGroupMode.UserNumber:
          key.Append(identity != null ? identity.UserNumber : null);
          break;
      }
      key.Append(AppConfig.VALUE_SEPARATOR);
      switch (ResetCycle)
      {
        case BusinessCodeSerialResetCycle.Day:
          key.Append(now.Day);
          break;
        case BusinessCodeSerialResetCycle.Month:
          key.Append(now.Month);
          break;
        case BusinessCodeSerialResetCycle.Year:
          key.Append(now.Year);
          break;
      }
      return key.ToString();
    }

    internal string TidyValue(string value)
    {
      return value.Length <= Length
        ? value.PadLeft(Length, '0')
        : value.Substring(value.Length - Length);
    }

    /// <summary>
    /// 拼装LikeKey值
    /// </summary>
    [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
    public string AssembleLikeKeyValue(int index)
    {
      StringBuilder key = new StringBuilder(Key);
      if (GroupMode != BusinessCodeSerialGroupMode.None)
        key.Append("%");
      key.Append(AppConfig.VALUE_SEPARATOR);
      key.Append(index);
      return key.ToString();
    }

    #endregion
  }
}