
namespace Phenix.Test.使用指南._12._10._3
{
    /// <summary>
    /// 身份类型
    /// </summary>
    [Phenix.Core.Operate.KeyCaptionAttribute(FriendlyName = "身份类型"), System.SerializableAttribute()]
    public enum Identity
    {
        /// <summary>
        /// 经营人
        /// </summary>
        [Phenix.Core.Rule.EnumCaptionAttribute("经营人")]
        Manager,
        /// <summary>
        /// 代理
        /// </summary>
        [Phenix.Core.Rule.EnumCaptionAttribute("代理")]
        Agent,
        /// <summary>
        /// 修理
        /// </summary>
        [Phenix.Core.Rule.EnumCaptionAttribute("修理")]
        Repair
    }
}