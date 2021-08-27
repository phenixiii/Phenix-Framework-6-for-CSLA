using System;
using Phenix.Core.Mapping;

namespace Phenix.Business
{
  /// <summary>
  /// 属性信息
  /// </summary>
  public sealed class PropertyInfo<T> : Csla.PropertyInfo<T>, IPropertyInfo
  {
    internal PropertyInfo(Csla.Core.IPropertyInfo info,
      Type ownerType, FieldMapInfo fieldMapInfo, PropertyMapInfo propertyMapInfo, Func<object> defaultValueFunc)
      : base(info.Name,
        !String.IsNullOrEmpty(info.FriendlyName) && String.CompareOrdinal(info.FriendlyName, info.Name) != 0 
        ? info.FriendlyName
        : fieldMapInfo != null && String.CompareOrdinal(fieldMapInfo.FriendlyName, info.Name) != 0
          ? fieldMapInfo.FriendlyName 
          : propertyMapInfo != null ? propertyMapInfo.FriendlyName : info.FriendlyName,
        (T)info.DefaultValue,
        Csla.RelationshipTypes.PrivateField)
    {
      _ownerType = ownerType;
      _fieldMapInfo = fieldMapInfo;
      _defaultValueFunc = defaultValueFunc;

      if (fieldMapInfo != null && String.CompareOrdinal(fieldMapInfo.FriendlyName, Name) == 0)
        fieldMapInfo.FriendlyName = FriendlyName;
      if (propertyMapInfo != null && String.CompareOrdinal(propertyMapInfo.FriendlyName, Name) == 0)
        propertyMapInfo.FriendlyName = FriendlyName;
    }

    #region 运算符重载

    /// <summary>
    /// Add
    /// </summary>
    /// <param name="leftNode">运算公式左</param>
    /// <param name="rightNode">运算公式右</param>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2225:OperatorOverloadsHaveNamedAlternates")]
    public static CriteriaExpressionNode operator +(PropertyInfo<T> leftNode, PropertyInfo<T> rightNode)
    {
      return new CriteriaExpressionNode(leftNode, OperationSign.Add, rightNode);
    }

    /// <summary>
    /// Add
    /// </summary>
    /// <param name="leftNode">运算公式左</param>
    /// <param name="value">值</param>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2225:OperatorOverloadsHaveNamedAlternates")]
    public static CriteriaExpressionNode operator +(PropertyInfo<T> leftNode, T value)
    {
      return new CriteriaExpressionNode(leftNode, OperationSign.Add, value);
    }

    /// <summary>
    /// Add
    /// </summary>
    /// <param name="value">值</param>
    /// <param name="rightNode">运算公式右</param>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2225:OperatorOverloadsHaveNamedAlternates")]
    public static CriteriaExpressionNode operator +(T value, PropertyInfo<T> rightNode)
    {
      return new CriteriaExpressionNode(value, OperationSign.Add, rightNode);
    }

    /// <summary>
    /// Minus
    /// </summary>
    /// <param name="leftNode">运算公式左</param>
    /// <param name="rightNode">运算公式右</param>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2225:OperatorOverloadsHaveNamedAlternates")]
    public static CriteriaExpressionNode operator -(PropertyInfo<T> leftNode, PropertyInfo<T> rightNode)
    {
      return new CriteriaExpressionNode(leftNode, OperationSign.Subtract, rightNode);
    }

    /// <summary>
    /// Subtract
    /// </summary>
    /// <param name="leftNode">运算公式左</param>
    /// <param name="value">值</param>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2225:OperatorOverloadsHaveNamedAlternates")]
    public static CriteriaExpressionNode operator -(PropertyInfo<T> leftNode, T value)
    {
      return new CriteriaExpressionNode(leftNode, OperationSign.Subtract, value);
    }

    /// <summary>
    /// Subtract
    /// </summary>
    /// <param name="value">值</param>
    /// <param name="rightNode">运算公式右</param>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2225:OperatorOverloadsHaveNamedAlternates")]
    public static CriteriaExpressionNode operator -(T value, PropertyInfo<T> rightNode)
    {
      return new CriteriaExpressionNode(value, OperationSign.Subtract, rightNode);
    }

    /// <summary>
    /// Subtract
    /// </summary>
    /// <param name="rightNode">运算公式右</param>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2225:OperatorOverloadsHaveNamedAlternates")]
    public static CriteriaExpressionNode operator -(PropertyInfo<T> rightNode)
    {
      return new CriteriaExpressionNode(OperationSign.Subtract, rightNode);
    }

    /// <summary>
    /// Multiply
    /// </summary>
    /// <param name="leftNode">运算公式左</param>
    /// <param name="rightNode">运算公式右</param>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2225:OperatorOverloadsHaveNamedAlternates")]
    public static CriteriaExpressionNode operator *(PropertyInfo<T> leftNode, PropertyInfo<T> rightNode)
    {
      return new CriteriaExpressionNode(leftNode, OperationSign.Multiply, rightNode);
    }

    /// <summary>
    /// Multiply
    /// </summary>
    /// <param name="leftNode">运算公式左</param>
    /// <param name="value">值</param>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2225:OperatorOverloadsHaveNamedAlternates")]
    public static CriteriaExpressionNode operator *(PropertyInfo<T> leftNode, T value)
    {
      return new CriteriaExpressionNode(leftNode, OperationSign.Multiply, value);
    }

    /// <summary>
    /// Multiply
    /// </summary>
    /// <param name="value">值</param>
    /// <param name="rightNode">运算公式右</param>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2225:OperatorOverloadsHaveNamedAlternates")]
    public static CriteriaExpressionNode operator *(T value, PropertyInfo<T> rightNode)
    {
      return new CriteriaExpressionNode(value, OperationSign.Multiply, rightNode);
    }

    /// <summary>
    /// Divide
    /// </summary>
    /// <param name="leftNode">运算公式左</param>
    /// <param name="rightNode">运算公式右</param>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2225:OperatorOverloadsHaveNamedAlternates")]
    public static CriteriaExpressionNode operator /(PropertyInfo<T> leftNode, PropertyInfo<T> rightNode)
    {
      return new CriteriaExpressionNode(leftNode, OperationSign.Divide, rightNode);
    }

    /// <summary>
    /// Divide
    /// </summary>
    /// <param name="leftNode">运算公式左</param>
    /// <param name="value">值</param>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2225:OperatorOverloadsHaveNamedAlternates")]
    public static CriteriaExpressionNode operator /(PropertyInfo<T> leftNode, T value)
    {
      return new CriteriaExpressionNode(leftNode, OperationSign.Divide, value);
    }

    /// <summary>
    /// Divide
    /// </summary>
    /// <param name="value">值</param>
    /// <param name="rightNode">运算公式右</param>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2225:OperatorOverloadsHaveNamedAlternates")]
    public static CriteriaExpressionNode operator /(T value, PropertyInfo<T> rightNode)
    {
      return new CriteriaExpressionNode(value, OperationSign.Divide, rightNode);
    }

    /// <summary>
    /// ToLower
    /// </summary>
    public CriteriaExpressionNode Length
    {
      get { return new CriteriaExpressionNode(OperationSign.Length, this); }
    }

    /// <summary>
    /// ToLower
    /// </summary>
    public CriteriaExpressionNode ToLower()
    {
      return new CriteriaExpressionNode(OperationSign.ToLower, this);
    }

    /// <summary>
    /// ToUpper
    /// </summary>
    public CriteriaExpressionNode ToUpper()
    {
      return new CriteriaExpressionNode(OperationSign.ToUpper, this);
    }

    /// <summary>
    /// TrimStart
    /// </summary>
    public CriteriaExpressionNode TrimStart()
    {
      return new CriteriaExpressionNode(OperationSign.TrimStart, this);
    }

    /// <summary>
    /// TrimEnd
    /// </summary>
    public CriteriaExpressionNode TrimEnd()
    {
      return new CriteriaExpressionNode(OperationSign.TrimEnd, this);
    }

    /// <summary>
    /// Trim
    /// </summary>
    public CriteriaExpressionNode Trim()
    {
      return new CriteriaExpressionNode(OperationSign.Trim, this);
    }

    /// <summary>
    /// Substring
    /// </summary>
    /// <param name="startIndex">起始字符位置(从零开始)</param>
    public CriteriaExpressionNode Substring(int startIndex)
    {
      return new CriteriaExpressionNode(OperationSign.Substring, this, new CriteriaExpressionNode(startIndex));
    }

    /// <summary>
    /// Substring
    /// </summary>
    /// <param name="startIndex">起始字符位置(从零开始)</param>
    /// <param name="length">截取字符数</param>
    public CriteriaExpressionNode Substring(int startIndex, int length)
    {
      return new CriteriaExpressionNode(OperationSign.Substring, this, new CriteriaExpressionNode(startIndex), new CriteriaExpressionNode(length));
    }

    #endregion

    #region 条件操作符重载

    /// <summary>
    /// Equal
    /// </summary>
    /// <param name="leftNode">条件操作表达式左</param>
    /// <param name="rightNode">条件操作表达式右</param>
    public static CriteriaExpression operator ==(PropertyInfo<T> leftNode, PropertyInfo<T> rightNode)
    {
      return new CriteriaExpression(leftNode, CriteriaOperate.Equal, rightNode);
    }

    /// <summary>
    /// Equal
    /// </summary>
    /// <param name="leftNode">条件操作表达式左</param>
    /// <param name="value">值</param>
    public static CriteriaExpression operator ==(PropertyInfo<T> leftNode, T value)
    {
      return new CriteriaExpression(leftNode, CriteriaOperate.Equal, value);
    }

    /// <summary>
    /// Equal
    /// </summary>
    /// <param name="value">值</param>
    /// <param name="rightNode">条件操作表达式右</param>
    public static CriteriaExpression operator ==(T value, PropertyInfo<T> rightNode)
    {
      return new CriteriaExpression(rightNode, CriteriaOperate.Equal, value);
    }

    /// <summary>
    /// Greater
    /// </summary>
    /// <param name="leftNode">条件操作表达式左</param>
    /// <param name="rightNode">条件操作表达式右</param>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2225:OperatorOverloadsHaveNamedAlternates")]
    public static CriteriaExpression operator >(PropertyInfo<T> leftNode, PropertyInfo<T> rightNode)
    {
      return new CriteriaExpression(leftNode, CriteriaOperate.Greater, rightNode);
    }

    /// <summary>
    /// Greater
    /// </summary>
    /// <param name="leftNode">条件操作表达式左</param>
    /// <param name="value">值</param>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2225:OperatorOverloadsHaveNamedAlternates")]
    public static CriteriaExpression operator >(PropertyInfo<T> leftNode, T value)
    {
      return new CriteriaExpression(leftNode, CriteriaOperate.Greater, value);
    }

    /// <summary>
    /// Greater
    /// </summary>
    /// <param name="value">值</param>
    /// <param name="rightNode">条件操作表达式右</param>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2225:OperatorOverloadsHaveNamedAlternates")]
    public static CriteriaExpression operator >(T value, PropertyInfo<T> rightNode)
    {
      return new CriteriaExpression(rightNode, CriteriaOperate.Lesser, value);
    }

    /// <summary>
    /// GreaterOrEqual
    /// </summary>
    /// <param name="leftNode">条件操作表达式左</param>
    /// <param name="rightNode">条件操作表达式右</param>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2225:OperatorOverloadsHaveNamedAlternates")]
    public static CriteriaExpression operator >=(PropertyInfo<T> leftNode, PropertyInfo<T> rightNode)
    {
      return new CriteriaExpression(leftNode, CriteriaOperate.GreaterOrEqual, rightNode);
    }

    /// <summary>
    /// GreaterOrEqual
    /// </summary>
    /// <param name="leftNode">条件操作表达式左</param>
    /// <param name="value">值</param>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2225:OperatorOverloadsHaveNamedAlternates")]
    public static CriteriaExpression operator >=(PropertyInfo<T> leftNode, T value)
    {
      return new CriteriaExpression(leftNode, CriteriaOperate.GreaterOrEqual, value);
    }

    /// <summary>
    /// GreaterOrEqual
    /// </summary>
    /// <param name="value">值</param>
    /// <param name="rightNode">条件操作表达式右</param>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2225:OperatorOverloadsHaveNamedAlternates")]
    public static CriteriaExpression operator >=(T value, PropertyInfo<T> rightNode)
    {
      return new CriteriaExpression(rightNode, CriteriaOperate.LesserOrEqual, value);
    }

    /// <summary>
    /// Lesser
    /// </summary>
    /// <param name="leftNode">条件操作表达式左</param>
    /// <param name="rightNode">条件操作表达式右</param>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2225:OperatorOverloadsHaveNamedAlternates")]
    public static CriteriaExpression operator <(PropertyInfo<T> leftNode, PropertyInfo<T> rightNode)
    {
      return new CriteriaExpression(leftNode, CriteriaOperate.Lesser, rightNode);
    }

    /// <summary>
    /// Lesser
    /// </summary>
    /// <param name="leftNode">条件操作表达式左</param>
    /// <param name="value">值</param>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2225:OperatorOverloadsHaveNamedAlternates")]
    public static CriteriaExpression operator <(PropertyInfo<T> leftNode, T value)
    {
      return new CriteriaExpression(leftNode, CriteriaOperate.Lesser, value);
    }

    /// <summary>
    /// Lesser
    /// </summary>
    /// <param name="value">值</param>
    /// <param name="rightNode">条件操作表达式右</param>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2225:OperatorOverloadsHaveNamedAlternates")]
    public static CriteriaExpression operator <(T value, PropertyInfo<T> rightNode)
    {
      return new CriteriaExpression(rightNode, CriteriaOperate.Greater, value);
    }

    /// <summary>
    /// LesserOrEqual
    /// </summary>
    /// <param name="leftNode">条件操作表达式左</param>
    /// <param name="rightNode">条件操作表达式右</param>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2225:OperatorOverloadsHaveNamedAlternates")]
    public static CriteriaExpression operator <=(PropertyInfo<T> leftNode, PropertyInfo<T> rightNode)
    {
      return new CriteriaExpression(leftNode, CriteriaOperate.LesserOrEqual, rightNode);
    }

    /// <summary>
    /// LesserOrEqual
    /// </summary>
    /// <param name="leftNode">条件操作表达式左</param>
    /// <param name="value">值</param>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2225:OperatorOverloadsHaveNamedAlternates")]
    public static CriteriaExpression operator <=(PropertyInfo<T> leftNode, T value)
    {
      return new CriteriaExpression(leftNode, CriteriaOperate.LesserOrEqual, value);
    }

    /// <summary>
    /// LesserOrEqual
    /// </summary>
    /// <param name="value">值</param>
    /// <param name="rightNode">条件操作表达式右</param>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2225:OperatorOverloadsHaveNamedAlternates")]
    public static CriteriaExpression operator <=(T value, PropertyInfo<T> rightNode)
    {
      return new CriteriaExpression(rightNode, CriteriaOperate.GreaterOrEqual, value);
    }

    /// <summary>
    /// Unequal
    /// </summary>
    /// <param name="leftNode">条件操作表达式左</param>
    /// <param name="rightNode">运条件操作表达式右</param>
    public static CriteriaExpression operator !=(PropertyInfo<T> leftNode, PropertyInfo<T> rightNode)
    {
      return new CriteriaExpression(leftNode, CriteriaOperate.Unequal, rightNode);
    }

    /// <summary>
    /// Unequal
    /// </summary>
    /// <param name="leftNode">条件操作表达式左</param>
    /// <param name="value">值</param>
    public static CriteriaExpression operator !=(PropertyInfo<T> leftNode, T value)
    {
      return new CriteriaExpression(leftNode, CriteriaOperate.Unequal, value);
    }

    /// <summary>
    /// Unequal
    /// </summary>
    /// <param name="value">值</param>
    /// <param name="rightNode">条件操作表达式右</param>
    public static CriteriaExpression operator !=(T value, PropertyInfo<T> rightNode)
    {
      return new CriteriaExpression(rightNode, CriteriaOperate.Unequal, value);
    }

    /// <summary>
    /// Like
    /// </summary>
    /// <param name="value">值</param>
    public CriteriaExpression Like(T value)
    {
      return new CriteriaExpression(this, CriteriaOperate.Like, false, value);
    }


    /// <summary>
    /// Like
    /// </summary>
    /// <param name="value">值</param>
    /// <param name="ignoreCase">忽略大小写</param>
    public CriteriaExpression Like(T value, bool ignoreCase)
    {
      return new CriteriaExpression(this, CriteriaOperate.Like, ignoreCase, value);
    }

    /// <summary>
    /// LikeIgnoreCase
    /// </summary>
    /// <param name="value">值</param>
    public CriteriaExpression LikeIgnoreCase(T value)
    {
      return new CriteriaExpression(this, CriteriaOperate.Like, true, value);
    }

    /// <summary>
    /// Unlike
    /// </summary>
    /// <param name="value">值</param>
    public CriteriaExpression Unlike(T value)
    {
      return new CriteriaExpression(this, CriteriaOperate.Unlike, false, value);
    }

    /// <summary>
    /// Unlike
    /// </summary>
    /// <param name="value">值</param>
    /// <param name="ignoreCase">忽略大小写</param>
    public CriteriaExpression Unlike(T value, bool ignoreCase)
    {
      return new CriteriaExpression(this, CriteriaOperate.Unlike, ignoreCase, value);
    }

    /// <summary>
    /// UnlikeIgnoreCase
    /// </summary>
    /// <param name="value">值</param>
    public CriteriaExpression UnlikeIgnoreCase(T value)
    {
      return new CriteriaExpression(this, CriteriaOperate.Unlike, true, value);
    }

    /// <summary>
    /// Like
    /// </summary>
    /// <param name="rightNode">条件操作表达式右</param>
    public CriteriaExpression Like(Phenix.Core.Mapping.IPropertyInfo rightNode)
    {
      return new CriteriaExpression(this, CriteriaOperate.Like, false, rightNode);
    }

    /// <summary>
    /// Like
    /// </summary>
    /// <param name="rightNode">条件操作表达式右</param>
    /// <param name="ignoreCase">忽略大小写</param>
    public CriteriaExpression Like(Phenix.Core.Mapping.IPropertyInfo rightNode, bool ignoreCase)
    {
      return new CriteriaExpression(this, CriteriaOperate.Like, ignoreCase, rightNode);
    }

    /// <summary>
    /// LikeIgnoreCase
    /// </summary>
    /// <param name="rightNode">条件操作表达式右</param>
    public CriteriaExpression LikeIgnoreCase(Phenix.Core.Mapping.IPropertyInfo rightNode)
    {
      return new CriteriaExpression(this, CriteriaOperate.Like, true, rightNode);
    }

    /// <summary>
    /// Unlike
    /// </summary>
    /// <param name="rightNode">条件操作表达式右</param>
    public CriteriaExpression Unlike(Phenix.Core.Mapping.IPropertyInfo rightNode)
    {
      return new CriteriaExpression(this, CriteriaOperate.Unlike, false, rightNode);
    }

    /// <summary>
    /// Unlike
    /// </summary>
    /// <param name="rightNode">条件操作表达式右</param>
    /// <param name="ignoreCase">忽略大小写</param>
    public CriteriaExpression Unlike(Phenix.Core.Mapping.IPropertyInfo rightNode, bool ignoreCase)
    {
      return new CriteriaExpression(this, CriteriaOperate.Unlike, ignoreCase, rightNode);
    }

    /// <summary>
    /// UnlikeIgnoreCase
    /// </summary>
    /// <param name="rightNode">条件操作表达式右</param>
    public CriteriaExpression UnlikeIgnoreCase(Phenix.Core.Mapping.IPropertyInfo rightNode)
    {
      return new CriteriaExpression(this, CriteriaOperate.Unlike, true, rightNode);
    }

    /// <summary>
    /// IsNull
    /// </summary>
    public CriteriaExpression IsNull
    {
      get { return new CriteriaExpression(this, CriteriaOperate.IsNull); }
    }

    /// <summary>
    /// IsNotNull
    /// </summary>
    public CriteriaExpression IsNotNull
    {
      get { return new CriteriaExpression(this, CriteriaOperate.IsNotNull); }
    }

    /// <summary>
    /// In
    /// </summary>
    /// <param name="value">值</param>
    public CriteriaExpression In(params T[] value)
    {
      return new CriteriaExpression(this, CriteriaOperate.In, value);
    }

    /// <summary>
    /// NotIn
    /// </summary>
    /// <param name="value">值</param>
    public CriteriaExpression NotIn(params T[] value)
    {
      return new CriteriaExpression(this, CriteriaOperate.NotIn, value);
    }

    #endregion

    #region 属性

    private readonly Type _ownerType;
    /// <summary>
    /// 所属类
    /// </summary>
    public Type OwnerType
    {
      get { return _ownerType; }
    }

    private readonly Func<object> _defaultValueFunc;
    /// <summary>
    /// 缺省值函数
    /// </summary>
    public Func<object> DefaultValueFunc
    {
      get { return _defaultValueFunc; }
    }

    /// <summary>
    /// 缺省值
    /// </summary>
    public override T DefaultValue
    {
      get
      {
        if (_defaultValueFunc != null)
        {
          object result = _defaultValueFunc();
          if (result is IPropertyInfo)
            result = !result.Equals(this) ? ((IPropertyInfo)result).DefaultValue : null;
          return (T)Phenix.Core.Reflection.Utilities.ChangeType(result, typeof(T));
        }
        return base.DefaultValue;
      }
    }
    object Phenix.Core.Mapping.IPropertyInfo.DefaultValue
    {
      get { return DefaultValue; }
    }
    object IPropertyInfo.DefaultValue
    {
      get { return DefaultValue; }
    }

    private readonly FieldMapInfo _fieldMapInfo;
    private FieldMapInfo FieldMapInfo
    {
      get { return _fieldMapInfo; }
    }
    FieldMapInfo Phenix.Core.Mapping.IPropertyInfo.FieldMapInfo
    {
      get { return FieldMapInfo; }
    }

    internal bool Selectable { get; set; }
    bool Phenix.Core.Mapping.IPropertyInfo.Selectable
    {
      get { return Selectable; }
    }

    #endregion

    #region 方法

    /// <summary>
    /// 取哈希值(注意字符串在32位和64位系统有不同的算法得到不同的结果) 
    /// </summary>
    public override int GetHashCode()
    {
      return OwnerType.FullName.GetHashCode() ^ Name.GetHashCode();
    }

    /// <summary>
    /// 比较对象
    /// </summary>
    /// <param name="obj">对象</param>
    public override bool Equals(object obj)
    {
      if (object.ReferenceEquals(obj, this))
        return true;
      PropertyInfo<T> other = obj as PropertyInfo<T>;
      if (object.ReferenceEquals(other, null))
        return false;
      return 
        String.CompareOrdinal(OwnerType.FullName, other.OwnerType.FullName) == 0 &&
        String.CompareOrdinal(Name, other.Name) == 0;
    }
    
    /// <summary>
    /// SetValue
    /// </summary>
    /// <param name="value">值</param>
    public PropertyValue SetValue(T value)
    {
      return PropertyValue.Set(this, value);
    }

    #endregion
  }
}