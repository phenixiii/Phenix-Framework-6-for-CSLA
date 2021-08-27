using System;
using Phenix.Core.Mapping;

namespace Phenix.Business
{
  /// <summary>
  /// 业务对象分页基类
  /// </summary>
  [Serializable]
  public abstract class BusinessPageBase<T> : BusinessBase<T>, IBusinessObjectPage
    where T : BusinessPageBase<T>
  {
    /// <summary>
    /// for CreateInstance
    /// </summary>
    protected BusinessPageBase()
    {
      //禁止添加代码
    }

    /// <summary>
    /// for Newtonsoft.Json.JsonConstructor
    /// </summary>
    protected BusinessPageBase(bool? isNew, bool? isSelfDirty, bool? isSelfDeleted)
      : base(isNew, isSelfDirty, isSelfDeleted) { }

    #region 属性

    [Csla.NotUndoable]
    private int _pageNo;
    /// <summary>
    /// 分页号
    /// </summary>
    public int PageNo
    {
      get { return _pageNo; }
    }

    #endregion

    #region 方法

    #region Parent

    /// <summary>
    /// 设置父对象
    /// </summary>
    /// <param name="parent">父对象</param>
    protected override void SetParent(Csla.Core.IParent parent)
    {
      IEntityCollectionPage page = parent as IEntityCollectionPage;
      if (page != null)
        _pageNo = page.PageNo;
      base.SetParent(parent);
    }

    #endregion

    #endregion
  }
}
