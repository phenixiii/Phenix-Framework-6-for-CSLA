using System;
using Phenix.Core.Operate;
using Phenix.Core.Rule;

namespace Phenix.Core.Dictionary
{
  /// <summary>
  /// 业务码流水号重置周期
  /// </summary>
  [Serializable]
  [KeyCaption(FriendlyName = "BusinessCodeSerial ResetCycle")]
  public enum BusinessCodeSerialResetCycle
  {
    /// <summary>
    /// 日
    /// </summary>
    [EnumCaption("Day", Key = "C=D")]
    Day,

    /// <summary>
    /// 月
    /// </summary>
    [EnumCaption("Month", Key = "C=M")]
    Month,

    /// <summary>
    /// 年
    /// </summary>
    [EnumCaption("Year", Key = "C=Y")]
    Year,
  }
}
