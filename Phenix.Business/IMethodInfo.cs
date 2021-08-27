
namespace Phenix.Business
{
  /// <summary>
  /// 方法信息接口
  /// </summary>
  public interface IMethodInfo : Csla.Core.IMemberInfo, Phenix.Core.Mapping.IMethodInfo
  {
    /// <summary>
    /// 属性名
    /// </summary>
    new string Name { get; }
  }
}
