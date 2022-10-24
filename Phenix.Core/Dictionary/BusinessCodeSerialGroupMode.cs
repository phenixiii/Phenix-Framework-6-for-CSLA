using System;
using Phenix.Core.Operate;
using Phenix.Core.Rule;

namespace Phenix.Core.Dictionary
{
  /// <summary>
  /// 业务码流水号分组模式
  /// </summary>
  [Serializable]
  [KeyCaption(FriendlyName = "BusinessCodeSerial GroupMode")]
  public enum BusinessCodeSerialGroupMode
  {
    /// <summary>
    /// 无 
    /// </summary>
    [EnumCaption("None", Key = "G=NO")]
    None,

    /// <summary>
    /// 部门
    /// </summary>
    [EnumCaption("Department", Key = "G=BM")]
    Department,

    /// <summary>
    /// 工号
    /// </summary>
    [EnumCaption("UserNumber", Key = "G=GH")]
    UserNumber
  }
}
