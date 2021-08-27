using Phenix.Core.Mapping;
using Phenix.Core.Operate;

namespace Phenix.Business
{
  /// <summary>
  /// 业务对象集合接口
  /// </summary>
  public interface IBusinessCollection : Csla.Core.IParent, IBusiness, IEntityCollection, ISelectableCollection
  {
    #region 属性

    /// <summary>
    /// 数据源键
    /// </summary>
    new string DataSourceKey { get; }

    /// <summary>
    /// 条件集
    /// </summary>
    new Criterions Criterions { get; }

    /// <summary>
    /// 标签
    /// </summary>
    new string Caption { get; }

    /// <summary>
    /// 业务对象惰性GetDetail
    /// </summary>
    bool ItemLazyGetDetail { get; set; }

    /// <summary>
    /// 是否只读
    /// </summary>
    new bool IsReadOnly { get; }

    /// <summary>
    /// 是否允许添加业务对象
    /// </summary>
    bool AllowAddItem { get; }

    /// <summary>
    /// 是否允许编辑业务对象
    /// </summary>
    bool AllowEditItem { get; }

    /// <summary>
    /// 是否允许删除业务对象
    /// </summary>
    bool AllowDeleteItem { get; }
    
    /// <summary>
    /// 仅提交被勾选的业务对象
    /// 缺省为 false
    /// </summary>
    bool OnlySaveSelected { get; }

    /// <summary>
    /// 是否业务对象各自使用独立事务
    /// 缺省为 false
    /// </summary>
    bool AloneTransaction { get; }

    #endregion

    #region 方法
    
    /// <summary>
    /// 检索第一个匹配对象, 根据 PrimaryKey 匹配
    /// </summary>
    /// <param name="primaryKey">主键</param>
    IBusinessObject FindItem(string primaryKey);

    /// <summary>
    /// 新增
    /// </summary>
    /// <param name="index">索引</param>
    /// <returns>对象</returns>
    IBusinessObject AddNew(int index);

    /// <summary>
    /// 新增
    /// </summary>
    /// <param name="index">索引</param>
    /// <param name="cloneSource">Clone数据源</param>
    IBusinessObject AddNew(int index, IBusinessObject cloneSource);
    
    /// <summary>
    /// 搜索无效对象
    /// onlyOldError = false
    /// onlySelfDirty = false
    /// </summary>
    /// <returns>无效对象</returns>
    IBusinessObject FindInvalidItem();

    /// <summary>
    /// 搜索无效对象
    /// onlySelfDirty = false
    /// </summary>
    /// <param name="onlyOldError">仅检查原有错误</param>
    /// <returns>无效对象</returns>
    IBusinessObject FindInvalidItem(bool onlyOldError);

    /// <summary>
    /// 搜索无效对象
    /// </summary>
    /// <param name="onlyOldError">仅检查原有错误</param>
    /// <param name="onlySelfDirty">仅检查脏数据</param>
    /// <returns>无效对象</returns>
    IBusinessObject FindInvalidItem(bool onlyOldError, bool onlySelfDirty);

    #endregion
  }
}