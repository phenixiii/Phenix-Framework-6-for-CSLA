using Phenix.Core.Mapping;

namespace Phenix.Business
{
  /// <summary>
  /// 业务对象分页接口
  /// </summary>
  public interface IBusinessObjectPage : IBusinessObject, IEntityPage
  {
    #region 属性

    /// <summary>
    /// 分页号
    /// </summary>
    int PageNo { get; }

    #endregion
  }
}
