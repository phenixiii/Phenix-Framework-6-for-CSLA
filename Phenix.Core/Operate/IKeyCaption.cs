using Phenix.Core.Mapping;

namespace Phenix.Core.Operate
{
  /// <summary>
  /// 对象选择接口
  /// </summary>
  public interface IKeyCaption : IEntity, ISelectable
  {
    /// <summary>
    /// 键
    /// </summary>
    object Key { get; }

    /// <summary>
    /// 标签
    /// </summary>
    new string Caption { get; }

    /// <summary>
    /// 值
    /// </summary>
    object Value { get; }
    
    /// <summary>
    /// 标记
    /// </summary>
    object Tag { get; }
  }
}