using System;
using System.Collections.Generic;
using Phenix.Core.Mapping;

namespace Phenix.Business
{
  /// <summary>
  /// 业务树节点基类
  /// </summary>
  [Serializable]
  public abstract class BusinessTreeNodeBase<T> : BusinessBase<T>, IBusinessTreeNode
    where T : BusinessTreeNodeBase<T>
  {
    /// <summary>
    /// for CreateInstance
    /// </summary>
    protected BusinessTreeNodeBase()
    {
      //禁止添加代码
    }

    /// <summary>
    /// for Newtonsoft.Json.JsonConstructor
    /// </summary>
    protected BusinessTreeNodeBase(bool? isNew, bool? isSelfDirty, bool? isSelfDeleted)
      : base(isNew, isSelfDirty, isSelfDeleted) { }

    #region 属性
    
    /// <summary>
    /// 父亲
    /// </summary>
    [Newtonsoft.Json.JsonIgnore]
    [System.ComponentModel.Browsable(false)]
    [System.ComponentModel.DataAnnotations.Display(AutoGenerateField = false)]
    public IBusinessTreeNode ParentNode
    {
      get { return ((IBusinessTree)Owner).FilterParentNode(this); }
    }

    /// <summary>
    /// 兄弟们
    /// </summary>
    [Newtonsoft.Json.JsonIgnore]
    [System.ComponentModel.Browsable(false)]
    [System.ComponentModel.DataAnnotations.Display(AutoGenerateField = false)]
    public IList<IBusinessTreeNode> Brethren
    {
      get { return ((IBusinessTree)Owner).FilterBrethren(this); }
    }

    /// <summary>
    /// 孩子们
    /// </summary>
    [Newtonsoft.Json.JsonIgnore]
    [System.ComponentModel.Browsable(false)]
    [System.ComponentModel.DataAnnotations.Display(AutoGenerateField = false)]
    public IList<IBusinessTreeNode> Children
    {
      get { return ((IBusinessTree)Owner).FilterChildren(this); }
    }
    
    /// <summary>
    /// 是否允许移动
    /// 缺省为 true
    /// </summary>
    [System.ComponentModel.Browsable(false)]
    [System.ComponentModel.DataAnnotations.Display(AutoGenerateField = false)]
    public virtual bool AllowMove
    {
      get { return true; }
    }

    #endregion

    #region 方法

    /// <summary>
    /// 是否允许添加孩子
    /// 缺省为 Owner.AllowAddItem
    /// </summary>
    /// <param name="childNode">子节点业务对象</param>
    public virtual bool AllowAddChild(T childNode)
    {
      return Owner.AllowAddItem;
    }
    bool IBusinessTreeNode.AllowAddChild(IBusinessTreeNode childNode)
    {
      return AllowAddChild(childNode as T);
    }

    /// <summary>
    /// 是否允许在一起
    /// 缺省为 Owner.AllowAddItem
    /// </summary>
    /// <param name="brotherNode">兄弟节点业务对象</param>
    public virtual bool AllowTogether(T brotherNode)
    {
      return Owner.AllowAddItem;
    }
    bool IBusinessTreeNode.AllowTogether(IBusinessTreeNode brotherNode)
    {
      return AllowTogether(brotherNode as T);
    }

    #region Brother

    /// <summary>
    /// 是否兄弟
    /// groupName = null
    /// </summary>
    /// <param name="brotherNode">兄弟节点业务对象</param>
    public bool IsBrother(T brotherNode)
    {
      return IsBrother(brotherNode, null);
    }

    /// <summary>
    /// 是否兄弟
    /// throwIfNotFound = true
    /// </summary>
    /// <param name="brotherNode">兄弟节点业务对象</param>
    /// <param name="groupName">分组名</param>
    public bool IsBrother(T brotherNode, string groupName)
    {
      return EntityLinkHelper.IsBrother(this, brotherNode, this.GetType(), groupName);
    }
    bool IBusinessTreeNode.IsBrother(IBusinessTreeNode brotherNode, string groupName)
    {
      return IsBrother(brotherNode as T, groupName);
    }

    #endregion

    #endregion
  }
}
