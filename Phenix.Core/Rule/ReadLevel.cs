using System;
using Phenix.Core.Operate;

namespace Phenix.Core.Rule
{
  /// <summary>
  /// 数据读取级别
  /// </summary>
  [Serializable]
  [KeyCaption(FriendlyName = "数据读取级别")]
  public enum ReadLevel
  {
    /// <summary>
    /// 公共
    /// </summary>
    [EnumCaption("公共")]
    Public,
    
    /// <summary>
    /// 私有
    /// </summary>
    [EnumCaption("私有")]
    Private,

    /// <summary>
    /// 同部门
    /// </summary>
    [EnumCaption("同部门")]
    Department,

    /// <summary>
    /// 同岗位
    /// </summary>
    [EnumCaption("同岗位")]
    Position,

    /// <summary>
    /// 同部门同岗位
    /// </summary>
    [EnumCaption("同部门同岗位")]
    DepartmentPosition
  }
}
