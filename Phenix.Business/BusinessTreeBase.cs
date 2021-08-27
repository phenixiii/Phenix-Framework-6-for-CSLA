using System;
using System.Collections.Generic;

namespace Phenix.Business
{
  /// <summary>
  /// 业务树基类
  /// </summary>
  [Serializable]
  public abstract class BusinessTreeBase<T, TBusiness> : BusinessListBase<T, TBusiness>, IBusinessTree
    where T : BusinessTreeBase<T, TBusiness>
    where TBusiness : BusinessTreeNodeBase<TBusiness>
  {
    #region 方法

    /// <summary>
    /// 过滤出父节点业务对象
    /// </summary>
    /// <param name="childNode">子节点业务对象</param>
    public IBusinessTreeNode FilterParentNode(TBusiness childNode)
    {
      if (childNode == null)
        return null;
      string groupName = GroupName;
      foreach (TBusiness item in this)
        if (childNode.IsLink(item, groupName))
          return item;
      return null;
    }
    IBusinessTreeNode IBusinessTree.FilterParentNode(IBusinessTreeNode childNode)
    {
      return FilterParentNode(childNode as TBusiness);
    }

    /// <summary>
    /// 过滤出兄弟节点业务对象集合
    /// </summary>
    /// <param name="brotherNode">兄弟点业务对象</param>
    public IList<IBusinessTreeNode> FilterBrethren(TBusiness brotherNode)
    {
      if (brotherNode == null)
        return null;
      List<IBusinessTreeNode> result = new List<IBusinessTreeNode>();
      string groupName = GroupName;
      foreach (TBusiness item in this)
        if (item.IsBrother(brotherNode, groupName))
          result.Add(item);
      return result;
    }
    IList<IBusinessTreeNode> IBusinessTree.FilterBrethren(IBusinessTreeNode brotherNode)
    {
      return FilterBrethren(brotherNode as TBusiness);
    }

    /// <summary>
    /// 过滤出子节点业务对象集合
    /// </summary>
    /// <param name="parentNode">父节点业务对象</param>
    public IList<IBusinessTreeNode> FilterChildren(TBusiness parentNode)
    {
      if (parentNode == null)
        return new List<IBusinessTreeNode>(this);
      List<IBusinessTreeNode> result = new List<IBusinessTreeNode>();
      string groupName = GroupName;
      foreach (TBusiness item in this)
        if (item.IsLink(parentNode, groupName))
          result.Add(item);
      return result;
    }
    IList<IBusinessTreeNode> IBusinessTree.FilterChildren(IBusinessTreeNode brotherNode)
    {
      return FilterChildren(brotherNode as TBusiness);
    }

    #endregion
  }
}
