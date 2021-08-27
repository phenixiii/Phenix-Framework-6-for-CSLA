using System;

namespace Phenix.StandardRule.Information
{
  /// <summary>
  /// 资料审核状态
  /// </summary>
  [Serializable]
  [Phenix.Core.Operate.KeyCaptionAttribute(FriendlyName = "资料审核状态")]
  public enum InformationVerifyStatus
  {
    /// <summary>
    /// 初始的
    /// </summary>
    [Phenix.Core.Rule.EnumCaptionAttribute("初始的")]
    Init,
    
    /// <summary>
    /// 已提交
    /// </summary>
    [Phenix.Core.Rule.EnumCaptionAttribute("已提交")]
    Submitted,

    /// <summary>
    ///不通过
    /// </summary>
    [Phenix.Core.Rule.EnumCaptionAttribute("不通过")]
    NotPass,
    
    /// <summary>
    /// 通过
    /// </summary>
    [Phenix.Core.Rule.EnumCaptionAttribute("通过")]
    Pass,
  }
}
