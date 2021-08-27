using System;
using Phenix.Core.Data;
using Phenix.Core.Mapping;

/* 
   builder:    phenixiii
   build time: 2018-07-02 09:37:30
   notes:      
*/

namespace Phenix.Test.使用指南._21._7.Plugin
{
  /// <summary>
  /// 
  /// </summary>
  [Serializable]
  [System.ComponentModel.DisplayName("")]
  public class AssemblyEasyList : EntityListBase<AssemblyEasyList, AssemblyEasy>
  {
    /// <summary>
    /// 构建实体
    /// </summary>
    protected override object CreateInstance()
    {
      return new AssemblyEasyList();
    }
  }
}