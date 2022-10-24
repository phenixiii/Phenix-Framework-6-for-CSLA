namespace Phenix.Core.Reflection
{
  /// <summary>
  /// 工厂接口
  /// </summary>
  public interface IFactory
  {
    /// <summary>
    /// 构建实体
    /// </summary>
    object CreateInstance();
  }
}
