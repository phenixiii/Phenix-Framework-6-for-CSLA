using System;

namespace Phenix.Test.使用指南._12._6._2._8
{
  /// <summary>
  /// Serial_AllowIgnoreCheckDirty
  /// </summary>
  [Serializable]
  //* 指定本类允许忽略校验数据库数据在下载到提交期间是否被更改过：AllowIgnoreCheckDirty = true
  [Phenix.Core.Mapping.ClassAttribute("PH_SERIAL", FriendlyName = "Serial", AllowIgnoreCheckDirty = true)]
  public class Serial_AllowIgnoreCheckDirty : Serial<Serial_AllowIgnoreCheckDirty>
  {
  }
}
