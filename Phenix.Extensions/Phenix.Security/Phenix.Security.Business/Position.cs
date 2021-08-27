﻿using System;

namespace Phenix.Security.Business
{
  /// <summary>
  /// 岗位
  /// </summary>
  [Serializable]
  [Phenix.Core.Mapping.ReadOnly]
  public class PositionReadOnly : Position<PositionReadOnly>
  {
  }

  /// <summary>
  /// 岗位清单
  /// </summary>
  [Serializable]
  public class PositionReadOnlyList : Phenix.Business.BusinessListBase<PositionReadOnlyList, PositionReadOnly>
  {
  }

  /// <summary>
  /// 岗位
  /// </summary>
  [Serializable]
  public class Position : Position<Position>
  {
  }

  /// <summary>
  /// 岗位清单
  /// </summary>
  [Serializable]
  public class PositionList : Phenix.Business.BusinessListBase<PositionList, Position>
  {
  }

  /// <summary>
  /// 岗位
  /// </summary>
  [Phenix.Core.Mapping.ClassAttribute("PH_POSITION", FriendlyName = "岗位"), System.SerializableAttribute(), System.ComponentModel.DisplayNameAttribute("岗位")]
  [Phenix.Core.Mapping.ClassDetail("PH_USER", "US_PT_ID", null, CascadingDelete = false, FriendlyName = "用户")]
  [Phenix.Core.Mapping.ClassDetail("PH_DEPARTMENT", "DP_PT_ID", null, CascadingDelete = false, FriendlyName = "部门")]
  public abstract class Position<T> : Phenix.Business.BusinessBase<T> where T : Position<T>
  {
    /// <summary>
    /// PT_ID
    /// </summary>
    public static readonly Phenix.Business.PropertyInfo<long?> PT_IDProperty = RegisterProperty<long?>(c => c.PT_ID);
    [Phenix.Core.Mapping.Field(FriendlyName = "PT_ID", TableName = "PH_POSITION", ColumnName = "PT_ID", IsPrimaryKey = true, NeedUpdate = true)]
    private long? _PT_ID;
    /// <summary>
    /// PT_ID
    /// </summary>
    [System.ComponentModel.DisplayName("PT_ID")]
    public long? PT_ID
    {
      get { return GetProperty(PT_IDProperty, _PT_ID); }
      internal set
      {
        SetProperty(PT_IDProperty, ref _PT_ID, value);
      }
    }

    [System.ComponentModel.Browsable(false)]
    [System.ComponentModel.DataAnnotations.Display(AutoGenerateField = false)]
    public override string PrimaryKey
    {
      get { return PT_ID.ToString(); }
    }

    /// <summary>
    /// 上级岗位
    /// </summary>
    public static readonly Phenix.Business.PropertyInfo<long?> PT_PT_IDProperty = RegisterProperty<long?>(c => c.PT_PT_ID);
    [Phenix.Core.Mapping.Field(FriendlyName = "上级岗位", TableName = "PH_POSITION", ColumnName = "PT_PT_ID", NeedUpdate = true)]
    [Phenix.Core.Mapping.FieldLink("PH_POSITION", "PT_ID")]
    private long? _PT_PT_ID;
    /// <summary>
    /// 上级岗位
    /// </summary>
    [System.ComponentModel.DisplayName("上级岗位")]
    public long? PT_PT_ID
    {
      get { return GetProperty(PT_PT_IDProperty, _PT_PT_ID); }
      set { SetProperty(PT_PT_IDProperty, ref _PT_PT_ID, value); }
    }

    /// <summary>
    /// 名称
    /// </summary>
    public static readonly Phenix.Business.PropertyInfo<string> NameProperty = RegisterProperty<string>(c => c.Name);
    [Phenix.Core.Mapping.Field(FriendlyName = "名称", Alias = "PT_NAME", TableName = "PH_POSITION", ColumnName = "PT_NAME", NeedUpdate = true, IsNameColumn = true, InLookUpColumn = true, InLookUpColumnDisplay = true)]
    private string _name;
    /// <summary>
    /// 名称
    /// </summary>
    [System.ComponentModel.DisplayName("名称")]
    public string Name
    {
      get { return GetProperty(NameProperty, _name); }
      set { SetProperty(NameProperty, ref _name, value); }
    }

    /// <summary>
    /// 代码
    /// </summary>
    public static readonly Phenix.Business.PropertyInfo<string> CodeProperty = RegisterProperty<string>(c => c.Code);
    [Phenix.Core.Mapping.Field(FriendlyName = "代码", Alias = "PT_CODE", TableName = "PH_POSITION", ColumnName = "PT_CODE", NeedUpdate = true, IsCodeColumn = true, InLookUpColumn = true, InLookUpColumnSelect = true)]
    private string _code;
    /// <summary>
    /// 代码
    /// </summary>
    [System.ComponentModel.DisplayName("代码")]
    public string Code
    {
      get { return GetProperty(CodeProperty, _code); }
      set { SetProperty(CodeProperty, ref _code, value); }
    }
  }
}
