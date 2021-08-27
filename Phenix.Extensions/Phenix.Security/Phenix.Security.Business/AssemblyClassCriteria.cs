using System;
using System.Collections.Generic;
using Phenix.Core.Dictionary;

namespace Phenix.Security.Business
{
  /// <summary>
  /// 类信息查询
  /// </summary>
  [System.SerializableAttribute(), System.ComponentModel.DisplayNameAttribute("类信息查询")]
  public class AssemblyClassCriteria : Phenix.Business.CriteriaBase
  {
    [Phenix.Core.Mapping.CriteriaField(FriendlyName = "类名", Logical = Phenix.Core.Mapping.CriteriaLogical.And, Operate = Phenix.Core.Mapping.CriteriaOperate.Like, TableName = "PH_ASSEMBLYCLASS", ColumnName = "AC_NAME")]
    private string _name;
    /// <summary>
    /// 类名
    /// </summary>
    [System.ComponentModel.DisplayName("类名")]
    //[System.ComponentModel.DataAnnotations.Required]
    public string Name
    {
      get { return _name != null ? _name.TrimEnd('%') : null; }
      set { _name = value != null ? String.Format("{0}%", value) : null; PropertyHasChanged(); }
    }

    [Phenix.Core.Mapping.CriteriaField(FriendlyName = "类型", Logical = Phenix.Core.Mapping.CriteriaLogical.And, Operate = Phenix.Core.Mapping.CriteriaOperate.In, TableName = "PH_ASSEMBLYCLASS", ColumnName = "AC_TYPE")]
    private Phenix.Core.Dictionary.AssemblyClassType[] _type;
    /// <summary>
    /// 类型
    /// </summary>
    [System.ComponentModel.DisplayName("类型")]
    public string Type
    {
      get { return Phenix.Core.Code.Converter.EnumArrayToFlags<Phenix.Core.Dictionary.AssemblyClassType>(_type); }
      set { _type = Phenix.Core.Code.Converter.FlagsToEnumArray<Phenix.Core.Dictionary.AssemblyClassType>(value); PropertyHasChanged(); }
    }

    #region 方法

    /// <summary>
    /// 添加类型
    /// </summary>
    public void AddType(AssemblyClassType assemblyClassType)
    {
      List<AssemblyClassType> types = _type != null ? new List<AssemblyClassType>(_type) : new List<AssemblyClassType>();
      if (!types.Contains(assemblyClassType))
        types.Add(assemblyClassType);
      _type = types.ToArray();
    }

    #endregion
  }
}
