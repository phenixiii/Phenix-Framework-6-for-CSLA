using System.Collections.Generic;
using Phenix.Core.Log;
using Phenix.Core.Mapping;
using Phenix.Core.Operate;

namespace Phenix.Business
{
  /// <summary>
  /// 业务对象接口
  /// </summary>
  public interface IBusinessObject : IBusiness, IEntity, ISelectable,
    System.ComponentModel.IDataErrorInfo,
    System.ComponentModel.INotifyPropertyChanged, 
    System.ComponentModel.INotifyPropertyChanging
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
    /// 对象ID
    /// </summary>
    long IdValue { get; }
    
    /// <summary>
    /// 所属业务对象集合
    /// </summary>
    new IBusinessCollection Owner { get; }

    /// <summary>
    /// 是否允许设置本对象
    /// </summary>
    bool AllowSet { get; }

    /// <summary>
    /// 是否允许编辑本对象
    /// </summary>
    bool AllowEdit { get; }

    /// <summary>
    /// 是否允许删除本对象
    /// </summary>
    bool AllowDelete { get; }

    /// <summary>
    /// 新增状态
    /// </summary>
    new bool IsNew { get; }

    /// <summary>
    /// 删除状态
    /// </summary>
    new bool IsDeleted { get; }

    /// <summary>
    /// 删除状态
    /// </summary>
    new bool IsSelfDeleted { get; }

    /// <summary>
    /// 更新状态
    /// </summary>
    new bool IsSelfDirty { get; }

    /// <summary>
    /// 已经Fetch
    /// </summary>
    bool SelfFetched { get; }

    #endregion

    #region 方法

    /// <summary>
    /// 纯净克隆
    /// </summary>
    IBusinessObject PureClone();

    /// <summary>
    /// 添加缓存类型
    /// </summary>
    void AddCacheType(IBusiness business);

    /// <summary>
    /// 构建自己
    /// </summary>
    void FetchSelf(IBusinessObject source, params Phenix.Core.Mapping.IPropertyInfo[] propertyInfos);

    /// <summary>
    /// 移动到新队列中
    /// </summary>
    void MoveTo(IBusinessCollection newOwner);

    /// <summary>
    /// 填充字段值到缺省值
    /// </summary>
    bool FillFieldValuesToDefault();

    /// <summary>
    /// 设置从业务对象
    /// 从业务对象与本业务对象是一对一的关系
    /// </summary>
    /// <param name="detail">从业务对象</param>
    void SetDetail<TDetailBusiness>(TDetailBusiness detail)
      where TDetailBusiness : BusinessBase<TDetailBusiness>;

    /// <summary>
    /// 设置从业务对象集合
    /// </summary>
    /// <param name="detail">从业务对象集合</param>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
    void SetDetail<TDetail, TDetailBusiness>(TDetail detail)
      where TDetail : BusinessListBase<TDetail, TDetailBusiness>
      where TDetailBusiness : BusinessBase<TDetailBusiness>;

    /// <summary>
    /// 检索从业务对象集合
    /// </summary>
    /// <param name="criterions">条件集</param>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
    TDetail FindDetail<TDetail, TDetailBusiness>(Criterions criterions)
      where TDetail : BusinessListBase<TDetail, TDetailBusiness>
      where TDetailBusiness : BusinessBase<TDetailBusiness>;

    /// <summary>
    /// 是否关联
    /// </summary>
    /// <param name="link">关联对象</param>
    /// <param name="groupName">分组名</param>
    bool IsLink(object link, string groupName);

    /// <summary>
    /// 校验数据是否有效
    /// </summary>
    /// <param name="propertyName">属性名</param>
    /// <returns>错误信息</returns>
    string CheckRule(string propertyName);

    /// <summary>
    /// 设置强制按照Update方式提交
    /// </summary>
    /// <param name="propertyInfos">需提交的属性信息, 当为null、空队列时提交全部属性</param>
    void SetForceUpdate(params Csla.Core.IPropertyInfo[] propertyInfos);

    /// <summary>
    /// 允许属性可读
    /// 与Phenix.Services.Client.Security.ReadWriteAuthorization配套使用
    /// </summary>
    /// <param name="propertyName">属性名</param>
    /// <returns>属性可读</returns>
    bool AllowReadProperty(string propertyName);

    /// <summary>
    /// 允许属性可写
    /// 与Phenix.Services.Client.Security.ReadWriteAuthorization配套使用
    /// </summary>
    /// <param name="propertyName">属性名</param>
    /// <returns>属性可写</returns>
    bool AllowWriteProperty(string propertyName);

    /// <summary>
    /// 检索执行动作
    /// </summary>
    /// <returns>执行动作信息队列</returns>
    IList<ExecuteActionInfo> FetchExecuteAction();

    #endregion
  }
}
