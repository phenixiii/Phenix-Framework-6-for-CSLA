using Phenix.Business;

namespace Phenix.StandardRule.Information
{
  /// <summary>
  /// 资料接口
  /// </summary>
  public interface IInformation : IBusinessObject
  {
    #region 属性

    /// <summary>
    /// 是否禁用
    /// </summary>
    bool Disabled { get; set; }

    /// <summary>
    /// 资料状态
    /// </summary>
    InformationStatus InformationStatus { get; }

    /// <summary>
    /// 是否需要审核
    /// </summary>
    bool NeedVerify { get; }

    /// <summary>
    /// 审核状态
    /// </summary>
    InformationVerifyStatus VerifyStatus { get; }

    #endregion

    #region 方法

    /// <summary>
    /// 资料状态发生变化前
    /// </summary>
    /// <param name="e">资料状态变更事件内容</param>
    void OnInformationStatusChanging(InformationStatusChangingEventArgs e);

    /// <summary>
    /// 资料状态发生变化后
    /// </summary>
    /// <param name="e">资料状态变更事件内容</param>
    void OnInformationStatusChanged(InformationStatusChangedEventArgs e);

    #endregion
  }
}