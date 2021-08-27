using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Phenix.Test.使用指南._21._5.Business
{
  [System.Serializable]
  [System.ComponentModel.DisplayName("")]
  public class Assembly : Assembly<Assembly>
  {
    private Assembly()
    {
      //禁止添加代码
    }

    [Newtonsoft.Json.JsonConstructor]
    private Assembly(bool? isNew, bool? isSelfDirty, bool? isSelfDeleted,
      AssemblyClassList assemblyClasses,
      long? AS_ID, string name, string caption)
      : base(isNew, isSelfDirty, isSelfDeleted, AS_ID, name, caption)
    {
      AssemblyClasses = assemblyClasses;
    }

    #region 属性

    //* 可序列化
    //* 组合关系的从业务对象集合 
    /// <summary>
    /// 类信息
    /// </summary>
    [Phenix.Core.Mapping.Property(Serializable = true)]
    [Newtonsoft.Json.JsonIgnoreAttribute]
    public AssemblyClassList AssemblyClasses
    {
      get { return GetCompositionDetail<AssemblyClassList, AssemblyClass>(); }
      private set { SetCompositionDetail<AssemblyClassList, AssemblyClass>(value); }
    }

    #endregion
  }

  /// <summary>
  /// 清单
  /// </summary>
  [System.Serializable]
  [System.ComponentModel.DisplayName("")]
  public class AssemblyList : Phenix.Business.BusinessListBase<AssemblyList, Assembly>
  {
  }
}
