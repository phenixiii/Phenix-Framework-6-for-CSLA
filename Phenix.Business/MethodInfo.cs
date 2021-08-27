using System;
using Phenix.Core.Mapping;

namespace Phenix.Business
{
  /// <summary>
  /// 方法信息
  /// </summary>
  public sealed class MethodInfo : Csla.MethodInfo, IMethodInfo
  {
    internal MethodInfo(Type ownerType, string name, string friendlyName, string tag, MethodMapInfo methodMapInfo)
      : base(name)
    {
      _ownerType = ownerType;

      _friendlyName = 
        !String.IsNullOrEmpty(friendlyName) && String.CompareOrdinal(friendlyName, name) != 0 
        ? friendlyName 
        : methodMapInfo != null ? methodMapInfo.FriendlyName : friendlyName;

      if (methodMapInfo != null && String.CompareOrdinal(methodMapInfo.FriendlyName, Name) == 0)
        methodMapInfo.FriendlyName = _friendlyName;

      _tag =
        !String.IsNullOrEmpty(tag) 
        ? tag 
        : methodMapInfo != null ? methodMapInfo.Tag : tag;

      _methodMapInfo = methodMapInfo;
    }

    #region 属性

    private readonly Type _ownerType;
    /// <summary>
    /// 所属类
    /// </summary>
    public Type OwnerType
    {
      get { return _ownerType; }
    }

    private readonly string _friendlyName;
    /// <summary>
    /// 友好名
    /// </summary>
    public string FriendlyName
    {
      get { return _friendlyName; }
    }

    private readonly string _tag;
    /// <summary>
    /// 标记
    /// </summary>
    public string Tag
    {
      get { return _tag; }
    }

    private readonly MethodMapInfo _methodMapInfo;
    /// <summary>
    /// 数据映射方法信息
    /// </summary>
    private MethodMapInfo MethodMapInfo
    {
      get { return _methodMapInfo; }
    }
    MethodMapInfo Phenix.Core.Mapping.IMethodInfo.MethodMapInfo
    {
      get { return MethodMapInfo; }
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
      MethodInfo other = obj as MethodInfo;
      if (object.ReferenceEquals(other, null))
        return false;
      return 
        String.CompareOrdinal(OwnerType.FullName, other.OwnerType.FullName) == 0 &&
        String.CompareOrdinal(Name, other.Name) == 0;
    }
    
    #endregion
  }
}