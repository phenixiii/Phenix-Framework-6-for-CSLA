using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using Phenix.Core.Dictionary;
using Phenix.Core.Operate;
using Phenix.Core.SyncCollections;

namespace Phenix.Core.Rule
{
  /// <summary>
  /// 枚举"键-标签"数组
  /// 主要用于填充下拉列表框内容
  /// </summary>
  [Serializable]
  public sealed class EnumKeyCaptionCollection : KeyCaptionCollection<EnumKeyCaption, Enum>
  {
    private EnumKeyCaptionCollection(Type enumType, IList<EnumKeyCaption> list)
      : base(list)
    {
      _enumType = enumType;
    }

    #region 工厂

    /// <summary>
    /// 根据枚举类型定义构建
    /// match = null
    /// </summary>
    /// <param name="enumType">枚举类型</param>
    public static EnumKeyCaptionCollection Fetch(Type enumType)
    {
      return DoFetch(enumType, null);
    }

    /// <summary>
    /// 根据枚举类型定义构建
    /// </summary>
    /// <param name="enumType">枚举类型</param>
    /// <param name="match">用于定义要搜索的元素应满足的条件</param>
    public static EnumKeyCaptionCollection Fetch(Type enumType, Predicate<EnumKeyCaption> match)
    {
      return DoFetch(enumType, match);
    }

    /// <summary>
    /// 根据枚举类型定义构建
    /// match = null
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
    public static EnumKeyCaptionCollection Fetch<TEnum>()
    {
      return DoFetch(typeof(TEnum), null);
    }

    /// <summary>
    /// 根据枚举类型定义构建
    /// </summary>
    /// <param name="match">用于定义要搜索的元素应满足的条件</param>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
    public static EnumKeyCaptionCollection Fetch<TEnum>(Predicate<EnumKeyCaption> match)
    {
      return DoFetch(typeof(TEnum), match);
    }
    
    /// <summary>
    /// 根据枚举类型定义构建
    /// </summary>
    /// <param name="match">用于定义要搜索的元素应满足的条件</param>
    public static EnumKeyCaptionCollection Fetch<TEnum>(Predicate<TEnum> match)
    {
      return DoFetch<TEnum>(match);
    }

    private static EnumKeyCaptionCollection DoFetch(Type enumType, Predicate<EnumKeyCaption> match)
    {
      if (!enumType.IsEnum)
        throw new InvalidOperationException(String.Format("类{0}未定义为枚举", enumType.FullName));

      EnumKeyCaptionCollection result;
      if (match != null || !_enumKeyCaptionCollectionCache.TryGetValue(enumType.FullName, out result))
      {
        FieldInfo[] fieldInfos = enumType.GetFields(BindingFlags.Public | BindingFlags.Static);
        List<EnumKeyCaption> sorter = new List<EnumKeyCaption>(fieldInfos.Length);
        foreach (FieldInfo item in fieldInfos)
        {
          EnumKeyCaption value = new EnumKeyCaption((Enum)item.GetValue(enumType), (EnumCaptionAttribute)Attribute.GetCustomAttribute(item, typeof(EnumCaptionAttribute)));
          if (match == null || match(value))
            sorter.Add(value);
        }
        sorter.Sort(EnumKeyCaption.Compare);
        result = new EnumKeyCaptionCollection(enumType, sorter);
        if (match == null)
          _enumKeyCaptionCollectionCache[enumType.FullName] = result;
      }
      return result;
    }

    private static EnumKeyCaptionCollection DoFetch<TEnum>(Predicate<TEnum> match)
    {
      Type enumType = typeof(TEnum);
      if (!enumType.IsEnum)
        throw new InvalidOperationException(String.Format("类{0}未定义为枚举", enumType.FullName));

      EnumKeyCaptionCollection result;
      if (match != null || !_enumKeyCaptionCollectionCache.TryGetValue(enumType.FullName, out result))
      {
        FieldInfo[] fieldInfos = enumType.GetFields(BindingFlags.Public | BindingFlags.Static);
        List<EnumKeyCaption> sorter = new List<EnumKeyCaption>(fieldInfos.Length);
        foreach (FieldInfo item in fieldInfos)
        {
          TEnum value = (TEnum)item.GetValue(enumType);
          if (match == null || match(value))
            sorter.Add(new EnumKeyCaption(value as Enum, (EnumCaptionAttribute)Attribute.GetCustomAttribute(item, typeof(EnumCaptionAttribute))));
        }
        sorter.Sort(EnumKeyCaption.Compare);
        result = new EnumKeyCaptionCollection(enumType, sorter);
        if (match == null)
          _enumKeyCaptionCollectionCache[enumType.FullName] = result;
      }
      return result;
    }

    #endregion

    #region 属性

    private readonly Type _enumType;
    /// <summary>
    /// 枚举类型
    /// </summary>
    [System.ComponentModel.Browsable(false)]
    [System.ComponentModel.DataAnnotations.Display(AutoGenerateField = false)]
    public Type EnumType
    {
      get { return _enumType; }
    }
    
    [NonSerialized]
    private AssemblyClassInfo _enumClassInfo;
    private AssemblyClassInfo EnumClassInfo
    {
      get
      {
        if (_enumClassInfo == null)
          _enumClassInfo = DataDictionaryHub.GetClassInfo(EnumType);
        return _enumClassInfo;
      }
    }

    [NonSerialized]
    private bool _keyCaptionAttributeChecked;
    [NonSerialized]
    private KeyCaptionAttribute _keyCaptionAttribute;
    private KeyCaptionAttribute KeyCaptionAttribute
    {
      get
      {
        if (!_keyCaptionAttributeChecked)
        {
          _keyCaptionAttribute = (KeyCaptionAttribute)Attribute.GetCustomAttribute(EnumType, typeof(KeyCaptionAttribute));
          _keyCaptionAttributeChecked = true;
        }
        return _keyCaptionAttribute;
      }
    }

    /// <summary>
    /// 标签
    /// </summary>
    public override string Caption
    {
      get
      {
        return EnumClassInfo != null && EnumClassInfo.CaptionConfigured && !String.IsNullOrEmpty(EnumClassInfo.Caption) ? EnumClassInfo.Caption :
          KeyCaptionAttribute != null && KeyCaptionAttribute.FriendlyName != null ? KeyCaptionAttribute.FriendlyName : EnumType.Name;
      }
    }

    private static readonly SynchronizedDictionary<string, EnumKeyCaptionCollection> _enumKeyCaptionCollectionCache =
      new SynchronizedDictionary<string, EnumKeyCaptionCollection>(StringComparer.Ordinal);

    #endregion

    #region 方法

    /// <summary>
    /// 克隆
    /// </summary>
    public new EnumKeyCaptionCollection Clone()
    {
      return (EnumKeyCaptionCollection)base.Clone();
    }

    #region Flags和Captions互转

    /// <summary>
    /// 将标签组合替换成标记组合
    /// separator = AppConfig.VALUE_SEPARATOR
    /// </summary>
    /// <param name="captions">标签组合</param>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
    public static string CaptionsToFlags<TEnum>(string captions)
    {
      return CaptionsToFlags<TEnum>(captions, AppConfig.VALUE_SEPARATOR);
    }

    /// <summary>
    /// 将标签组合替换成标记组合
    /// </summary>
    /// <param name="captions">标签组合</param>
    /// <param name="separator">标签分隔符</param>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
    public static string CaptionsToFlags<TEnum>(string captions, char separator)
    {
      StringBuilder result = new StringBuilder();
      if (captions != null)
      {
        string[] strings = captions.Split(separator);
        foreach (EnumKeyCaption item in Fetch<TEnum>())
          foreach (string s in strings)
            if (String.CompareOrdinal(item.Caption, s.Trim()) == 0)
            {
              result.Append(item.Flag.ToString());
              result.Append(separator);
              break;
            }
        if (result.Length > 0)
          result.Remove(result.Length - 1, 1);
      }
      return result.ToString();
    }

    /// <summary>
    /// 将标记组合替换成标签组合
    /// separator = AppConfig.VALUE_SEPARATOR
    /// </summary>
    /// <param name="flags">标记组合</param>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
    public static string FlagsToCaptions<TEnum>(string flags)
    {
      return FlagsToCaptions<TEnum>(flags, AppConfig.VALUE_SEPARATOR);
    }

    /// <summary>
    /// 将标记组合替换成标签组合
    /// </summary>
    /// <param name="flags">标记组合</param>
    /// <param name="separator">标签分隔符</param>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
    public static string FlagsToCaptions<TEnum>(string flags, char separator)
    {
      StringBuilder result = new StringBuilder();
      if (flags != null)
      {
        string[] strings = flags.Split(separator);
        foreach (EnumKeyCaption item in Fetch<TEnum>())
          foreach (string s in strings)
            if (String.CompareOrdinal(item.Flag.ToString(), s.Trim()) == 0)
            {
              result.Append(item.Caption);
              result.Append(separator);
              break;
            }
        if (result.Length > 0)
          result.Remove(result.Length - 1, 1);
      }
      return result.ToString();
    }

    #endregion

    #region TryGetValue

    /// <summary>
    /// 尝试取枚举"键-标签"
    /// </summary>
    /// <param name="caption">标签</param>
    /// <param name="value">值</param>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
    public static bool TryGetValue<TEnum>(string caption, out EnumKeyCaption value)
    {
      foreach (EnumKeyCaption item in Fetch<TEnum>())
        if (String.CompareOrdinal(item.Caption, caption) == 0)
        {
          value = item;
          return true;
        }
      value = null;
      return false;
    }

    #endregion

    /// <summary>
    /// 比较对象
    /// </summary>
    /// <param name="obj">对象</param>
    public override bool Equals(object obj)
    {
      if (object.ReferenceEquals(obj, this))
        return true;
      EnumKeyCaptionCollection other = obj as EnumKeyCaptionCollection;
      if (object.ReferenceEquals(other, null))
        return false;
      return String.CompareOrdinal(ToString(), other.ToString()) == 0;
    }

    /// <summary>
    /// 比较类型
    /// 主要用于IDE环境
    /// </summary>
    public static bool Equals(Type objectType)
    {
      if (objectType == null)
        return false;
      return String.CompareOrdinal(objectType.FullName, typeof(EnumKeyCaptionCollection).FullName) == 0;
    }

    /// <summary>
    /// 取哈希值(注意字符串在32位和64位系统有不同的算法得到不同的结果) 
    /// </summary>
    public override int GetHashCode()
    {
      int result = _enumType.FullName.GetHashCode();
      foreach (EnumKeyCaption item in this)
        result = result ^ item.Value.ToString().GetHashCode();
      return result;
    }

    /// <summary>
    /// 字符串表示
    /// </summary>
    public override string ToString()
    {
      StringBuilder result = new StringBuilder();
      result.Append(_enumType.FullName);
      result.Append(Phenix.Core.AppConfig.VALUE_SEPARATOR);
      foreach (EnumKeyCaption item in this)
      {
        result.Append(item.Value.ToString());
        result.Append(Phenix.Core.AppConfig.VALUE_SEPARATOR);
      }
      return result.ToString();
    }
    
    #endregion
  }
}
