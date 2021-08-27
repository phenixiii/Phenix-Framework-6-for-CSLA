using System;

namespace Phenix.StandardRule.Information
{
  /// <summary>
  /// 资料状态
  /// </summary>
  [Serializable]
  [Phenix.Core.Operate.KeyCaptionAttribute(FriendlyName = "资料状态")]
  public enum InformationStatus : int
  {
    /// <summary>
    /// 初始的
    /// </summary>
    [Phenix.Core.Rule.EnumCaptionAttribute("初始的")]
    Init,

    /// <summary>
    /// 预登记
    /// </summary>
    [Phenix.Core.Rule.EnumCaptionAttribute("预登记")]
    Registration,

    /// <summary>
    /// 正式的
    /// </summary>
    [Phenix.Core.Rule.EnumCaptionAttribute("正式的")]
    Formal,

    /// <summary>
    /// 暂停的
    /// </summary>
    [Phenix.Core.Rule.EnumCaptionAttribute("暂停的")]
    Suspended,

    /// <summary>
    /// 被合并
    /// </summary>
    [Phenix.Core.Rule.EnumCaptionAttribute("被合并")]
    Merged,

    /// <summary>
    /// 作废的
    /// </summary>
    [Phenix.Core.Rule.EnumCaptionAttribute("作废的")]
    Invalided,
  }
}
