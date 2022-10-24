using System;
using Phenix.Core.Operate;
using Phenix.Core.Rule;

namespace Phenix.Core.Dictionary
{
  /// <summary>
  /// 业务码格式项类型
  /// </summary> 
  [Serializable]
  [KeyCaption(FriendlyName = "BusinessCodeFormat ItemType")]
  public enum BusinessCodeFormatItemType
  {
    /// <summary>
    /// 流水号 
    /// </summary>
    [EnumCaption("SerialNumber", Key = "(" + BusinessCodeSerialFormat.REGEX_PATTERN + ")")]
    SerialNumber,

    /// <summary>
    /// 长年
    /// </summary>
    [EnumCaption("LengthYear", Key = "(YYYY)")]
    LengthYear,

    /// <summary>
    /// 短年
    /// </summary>
    [EnumCaption("ShortYear", Key = "(YY)")]
    ShortYear,

    /// <summary>
    /// 月
    /// </summary>
    [EnumCaption("Month", Key = "(MM)")]
    Month,

    /// <summary>
    /// 日
    /// </summary>
    [EnumCaption("Day", Key = "(DD)")]
    Day,

    /// <summary>
    /// 部门
    /// </summary>
    [EnumCaption("Department", Key = "(BM)")]
    Department,

    /// <summary>
    /// 工号
    /// </summary>
    [EnumCaption("UserNumber", Key = "(GH)")]
    UserNumber
  }
}
