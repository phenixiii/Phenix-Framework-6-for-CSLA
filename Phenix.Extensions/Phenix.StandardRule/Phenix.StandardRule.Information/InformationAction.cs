using System;

namespace Phenix.StandardRule.Information
{
  /// <summary>
  /// 资料动作
  /// </summary>
  [Serializable]
  [Phenix.Core.Operate.KeyCaptionAttribute(FriendlyName = "资料动作")]
  public enum InformationAction
  {
    /// <summary>
    /// 创建资料
    /// </summary>
    [Phenix.Core.Rule.EnumCaptionAttribute("创建资料")]
    Create,
    
    /// <summary>
    /// 提交审核
    /// </summary>
    [Phenix.Core.Rule.EnumCaptionAttribute("提交审核")]
    SubmitVerify,
    
    /// <summary>
    /// 通过审核
    /// </summary>
    [Phenix.Core.Rule.EnumCaptionAttribute("通过审核")]
    VerifyPass,

    /// <summary>
    /// 未通过审核
    /// </summary>
    [Phenix.Core.Rule.EnumCaptionAttribute("未通过审核")]
    VerifyNotPass,

    /// <summary>
    /// 合并资料
    /// </summary>
    [Phenix.Core.Rule.EnumCaptionAttribute("合并资料")]
    Merge,

    /// <summary>
    /// 作废资料
    /// </summary>
    [Phenix.Core.Rule.EnumCaptionAttribute("作废资料")]
    Invalid,

    /// <summary>
    /// 暂停使用资料
    /// </summary>
    [Phenix.Core.Rule.EnumCaptionAttribute("暂停使用资料")]
    Suspend,

    /// <summary>
    /// 恢复使用资料
    /// </summary>
    [Phenix.Core.Rule.EnumCaptionAttribute("恢复使用资料")]
    Apply,
  }
}
