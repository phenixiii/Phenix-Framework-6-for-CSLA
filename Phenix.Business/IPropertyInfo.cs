using System;

namespace Phenix.Business
{
  /// <summary>
  /// 属性信息接口
  /// </summary>
  public interface IPropertyInfo : Csla.Core.IPropertyInfo, Phenix.Core.Mapping.IPropertyInfo
  {
    /// <summary>
    /// 类
    /// </summary>
    new Type Type { get; }

    /// <summary>
    /// 属性名
    /// </summary>
    new string Name { get; }

    /// <summary>
    /// 友好名
    /// </summary>
    new string FriendlyName { get; }

    /// <summary>
    /// 缺省值
    /// </summary>
    new object DefaultValue { get; }
  }
}
