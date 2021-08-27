using System.Collections.Generic;

namespace Phenix.Business
{
  /// <summary>
  /// 业务树接口
  /// </summary>
  public interface IBusinessTree : IBusinessCollection
  {
    #region 方法

    /// <summary>
    /// 过滤出父节点业务对象
    /// </summary>
    /// <param name="childNode">子节点业务对象</param>
    IBusinessTreeNode FilterParentNode(IBusinessTreeNode childNode);

    /// <summary>
    /// 过滤出兄弟节点业务对象集合
    /// </summary>
    /// <param name="brotherNode">兄弟节点业务对象</param>
    IList<IBusinessTreeNode> FilterBrethren(IBusinessTreeNode brotherNode);

    /// <summary>
    /// 过滤出子节点业务对象集合
    /// </summary>
    /// <param name="parentNode">父节点业务对象</param>
    IList<IBusinessTreeNode> FilterChildren(IBusinessTreeNode parentNode);

    #endregion
  }
}
