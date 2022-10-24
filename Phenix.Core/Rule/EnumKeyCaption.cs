using System;
using Phenix.Core.Dictionary;
using Phenix.Core.Operate;

namespace Phenix.Core.Rule
{
  /// <summary>
  /// 枚举"键-标签"
  /// 主要用于填充下拉列表框内容
  /// </summary>
  [Serializable]
  public sealed class EnumKeyCaption : KeyCaption<EnumKeyCaption, Enum>, IKeyCaption, IComparable, IComparable<EnumKeyCaption>
  {
    internal EnumKeyCaption(Enum value, EnumCaptionAttribute enumCaptionAttribute)
      : base(null, null, value)
    {
      _enumCaptionAttribute = enumCaptionAttribute;
    }
    
    #region 工厂

    /// <summary>
    /// 根据枚举值构建填充
    /// </summary>
    public static EnumKeyCaption Fetch(Enum value)
    {
      if (value != null)
        foreach (EnumKeyCaption item in EnumKeyCaptionCollection.Fetch(value.GetType()))
          if (Enum.Equals(item.Value, value))
            return item;
      return null;
    }

    #endregion

    #region 属性
    
    [NonSerialized]
    private AssemblyClassPropertyInfo _classPropertyInfo;
    private AssemblyClassPropertyInfo ClassPropertyInfo
    {
      get
      {
        if (_classPropertyInfo == null)
        {
          AssemblyClassInfo classInfo = DataDictionaryHub.GetClassInfo(ValueType);
          if (classInfo != null)
            _classPropertyInfo = classInfo.GetClassPropertyInfo(Value.ToString());
        }
        return _classPropertyInfo;
      }
    }

    private readonly EnumCaptionAttribute _enumCaptionAttribute;
    private EnumCaptionAttribute EnumCaptionAttribute
    {
      get { return _enumCaptionAttribute; }
    }

    /// <summary>
    /// 键
    /// </summary>
    public new string Key
    {
      get
      {
        if (ClassPropertyInfo != null && ClassPropertyInfo.Configurable && !String.IsNullOrEmpty(ClassPropertyInfo.ConfigValue))
          return ClassPropertyInfo.ConfigValue;
        return EnumCaptionAttribute != null && EnumCaptionAttribute.Key != null ? EnumCaptionAttribute.Key : Value.ToString("d");
      }
    }
    object IKeyCaption.Key
    {
      get { return Key; }
    }

    /// <summary>
    /// 标签
    /// </summary>
    public override string Caption
    {
      get
      {
        if (ClassPropertyInfo != null && ClassPropertyInfo.CaptionConfigured && !String.IsNullOrEmpty(ClassPropertyInfo.Caption))
          return ClassPropertyInfo.Caption;
        return EnumCaptionAttribute != null && EnumCaptionAttribute.Caption != null ? EnumCaptionAttribute.Caption : Value.ToString();
      }
    }

    /// <summary>
    /// 标记
    /// </summary>
    public new string Tag
    {
      get
      {
        if (base.Tag != null)
          return base.Tag.ToString();
        return EnumCaptionAttribute != null ? EnumCaptionAttribute.Tag : null;
      }
    }
    object IKeyCaption.Tag
    {
      get { return Tag; }
    }

    /// <summary>
    /// 标记
    /// </summary>
    public int Flag
    {
      get { return (int)Convert.ChangeType(Value, typeof(int)); }
    }

    /// <summary>
    /// 索引号
    /// </summary>
    public int IndexNumber
    {
      get
      {
        if (ClassPropertyInfo != null)
          return ClassPropertyInfo.IndexNumber;
        return -1;
      }
    }

    /// <summary>
    /// 是否可见
    /// </summary>
    [System.ComponentModel.Browsable(false)]
    [System.ComponentModel.DataAnnotations.Display(AutoGenerateField = false)]
    public bool Visible
    {
      get
      {
        if (ClassPropertyInfo != null)
          return ClassPropertyInfo.Visible;
        return true;
      }
    }

    /// <summary>
    /// 所属对象集合
    /// </summary>
    [System.ComponentModel.Browsable(false)]
    [System.ComponentModel.DataAnnotations.Display(AutoGenerateField = false)]
    public new EnumKeyCaptionCollection Owner
    {
      get { return (EnumKeyCaptionCollection) base.Owner; }
    }

    [NonSerialized]
    private int? _hashCode;

    #endregion

    #region 方法

    /// <summary>
    /// 取枚举标签
    /// </summary>
    /// <param name="value">枚举值</param>
    public static string GetCaption(Enum value)
    {
      EnumKeyCaption result = Fetch(value);
      return result != null ? result.Caption : null;
    }

    /// <summary>
    /// 取枚举键
    /// </summary>
    /// <param name="value">枚举值</param>
    public static string GetKey(Enum value)
    {
      EnumKeyCaption result = Fetch(value);
      return result != null ? result.Key : null;
    }

    /// <summary>
    /// 取枚举值
    /// </summary>
    /// <param name="caption">枚举值</param>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
    public static Enum GetValue<TEnum>(string caption)
    {
      return EnumKeyCaptionCollection.TryGetValue<TEnum>(caption, out EnumKeyCaption result) ? result.Value : null;
    }

    /// <summary>
    /// 比较对象
    /// </summary>
    /// <param name="obj">对象</param>
    public override bool Equals(object obj)
    {
      if (object.ReferenceEquals(obj, this))
        return true;
      EnumKeyCaption other = obj as EnumKeyCaption;
      if (object.ReferenceEquals(other, null))
        return false;
      return Value.Equals(other.Value);
    }

    #region IComparable 成员

    /// <summary>
    /// 比较对象
    /// </summary>
    public int CompareTo(object obj)
    {
      return CompareTo(obj as EnumKeyCaption);
    }

    /// <summary>
    /// 比较对象
    /// </summary>
    public int CompareTo(EnumKeyCaption other)
    {
      if (object.ReferenceEquals(other, this))
        return 0;
      if (object.ReferenceEquals(other, null))
        return 1;
      return Value.CompareTo(other.Value);
    }

    /// <summary>
    /// 比较对象
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters")]
    public static int Compare(EnumKeyCaption x, EnumKeyCaption y)
    {
      if (object.ReferenceEquals(x, y))
        return 0;
      if (object.ReferenceEquals(x, null))
        return -1;
      return x.CompareTo(y);
    }

    /// <summary>
    /// 等于
    /// </summary>
    public static bool operator ==(EnumKeyCaption left, EnumKeyCaption right)
    {
      return Compare(left, right) == 0;
    }

    /// <summary>
    /// 不等于
    /// </summary>
    public static bool operator !=(EnumKeyCaption left, EnumKeyCaption right)
    {
      return Compare(left, right) != 0;
    }

    /// <summary>
    /// 小于
    /// </summary>
    public static bool operator <(EnumKeyCaption left, EnumKeyCaption right)
    {
      return Compare(left, right) < 0;
    }

    /// <summary>
    /// 大于
    /// </summary>
    public static bool operator >(EnumKeyCaption left, EnumKeyCaption right)
    {
      return Compare(left, right) > 0;
    }

    #endregion

    /// <summary>
    /// 取哈希值(注意字符串在32位和64位系统有不同的算法得到不同的结果) 
    /// </summary>
    public override int GetHashCode()
    {
      if (!_hashCode.HasValue)
        _hashCode = Value.GetType().FullName.GetHashCode() ^ Value.ToString().GetHashCode();
      return _hashCode.Value;
    }

    /// <summary>
    /// 字符串表示
    /// </summary>
    public override string ToString()
    {
      return String.Format("{0}.{1}", Value.GetType().FullName, Value.ToString());
    }

    #endregion
  }
}
