using System.Collections.Generic;

namespace Phenix.Business
{
  /// <summary>
  /// 业务树节点接口
  /// </summary>
  public interface IBusinessTreeNode : IBusinessObject
  {
    #region 属性

    /// <summary>
    /// 父亲
    /// </summary>
    IBusinessTreeNode ParentNode { get; }

    /// <summary>
    /// 兄弟们
    /// </summary>
    IList<IBusinessTreeNode> Brethren { get; }

    /// <summary>
    /// 孩子们
    /// </summary>
    IList<IBusinessTreeNode> Children { get; }
    
    /// <summary>
    /// 是否允许移动
    /// </summary>
    bool AllowMove { get; }

    #endregion

    #region 方法

    /// <summary>
    /// 是否允许添加孩子
    /// </summary>
    /// <param name="childNode">子节点业务对象</param>
    bool AllowAddChild(IBusinessTreeNode childNode);

    /// <summary>
    /// 是否允许在一起
    /// </summary>
    /// <param name="brotherNode">兄弟节点业务对象</param>
    bool AllowTogether(IBusinessTreeNode brotherNode);

    /// <summary>
    /// 是否兄弟
    /// </summary>
    /// <param name="brotherNode">兄弟节点业务对象</param>
    /// <param name="groupName">分组名</param>
    bool IsBrother(IBusinessTreeNode brotherNode, string groupName);

    #endregion
  }
}
