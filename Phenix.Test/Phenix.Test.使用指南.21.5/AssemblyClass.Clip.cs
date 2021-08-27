using System;
using Phenix.Core.Data;

namespace Phenix.Test.使用指南._21._5
{
  /// <summary>
  /// 
  /// </summary>
  [Serializable]
  [Phenix.Core.Mapping.Class("Phenix.Test.使用指南._21._5.Business.AssemblyClassList", FriendlyName = "")]
  public class AssemblyClassList : EntityListPageBase<AssemblyClassList, AssemblyClass>
  {
    /// <summary>
    /// 构建实体
    /// </summary>
    protected override object CreateInstance()
    {
      return new AssemblyClassList();
    }
  }

  /// <summary>
  /// 
  /// </summary>
  [Serializable]
  [Phenix.Core.Mapping.Class("Phenix.Test.使用指南._21._5.Business.AssemblyClass", FriendlyName = "")]
  public class AssemblyClass : EntityPageBase<AssemblyClass>
  {
    private AssemblyClass()
    {
      //禁止添加代码
    }

    [Newtonsoft.Json.JsonConstructor]
    private AssemblyClass(bool? isNew, bool? isSelfDirty, bool? isSelfDeleted,
      long? AC_ID, long? AC_AS_ID, string name, string caption, int? captionconfigured)
      : base(isNew, isSelfDirty, isSelfDeleted)
    {
      if (AC_ID != null)
      {
        _AC_ID = AC_ID;
        SetDirtyProperty(AC_IDProperty);
      }
      if (AC_AS_ID != null)
      {
        _AC_AS_ID = AC_AS_ID;
        SetDirtyProperty(AC_AS_IDProperty);
      }
      if (name != null)
      {
        _name = name;
        SetDirtyProperty(NameProperty);
      }
      if (caption != null)
      {
        _caption = caption;
        SetDirtyProperty(CaptionProperty);
      }
      if (captionconfigured != null)
      {
        _captionconfigured = captionconfigured;
        SetDirtyProperty(CaptionconfiguredProperty);
      }
    }

    /// <summary>
    /// 构建实体
    /// </summary>
    protected override object CreateInstance()
    {
      return new AssemblyClass();
    }

    /// <summary>
    /// 主键值
    /// </summary>
    [System.ComponentModel.Browsable(false)]
    public override string PrimaryKey
    {
      get { return base.PrimaryKey; }
    }

    /// <summary>
    /// AC_ID
    /// </summary>
    public static readonly Phenix.Core.Mapping.PropertyInfo<long?> AC_IDProperty = RegisterProperty<long?>(c => c.AC_ID);
    [Phenix.Core.Mapping.Field(FriendlyName = "AC_ID", IsPrimaryKey = true, IsWatermarkColumn = false, NeedUpdate = true, OverwritingOnUpdate = false)]
    private long? _AC_ID;
    /// <summary>
    /// AC_ID
    /// </summary>
    [System.ComponentModel.DisplayName("AC_ID")]
    public long? AC_ID
    {
      get { return GetProperty(AC_IDProperty, _AC_ID); }
    }

    /// <summary>
    /// AC_AS_ID
    /// </summary>
    public static readonly Phenix.Core.Mapping.PropertyInfo<long?> AC_AS_IDProperty = RegisterProperty<long?>(c => c.AC_AS_ID);
    [Phenix.Core.Mapping.Field(FriendlyName = "AC_AS_ID", IsPrimaryKey = false, IsWatermarkColumn = false, NeedUpdate = true, OverwritingOnUpdate = false)]
    private long? _AC_AS_ID;
    /// <summary>
    /// AC_AS_ID
    /// </summary>
    [System.ComponentModel.DisplayName("AC_AS_ID")]
    public long? AC_AS_ID
    {
      get { return GetProperty(AC_AS_IDProperty, _AC_AS_ID); }
      set { SetProperty(AC_AS_IDProperty, ref _AC_AS_ID, value); }
    }

    /// <summary>
    /// AC_NAME
    /// </summary>
    public static readonly Phenix.Core.Mapping.PropertyInfo<string> NameProperty = RegisterProperty<string>(c => c.Name);
    [Phenix.Core.Mapping.Field(FriendlyName = "AC_NAME", IsPrimaryKey = false, IsWatermarkColumn = false, NeedUpdate = true, OverwritingOnUpdate = false)]
    private string _name;
    /// <summary>
    /// AC_NAME
    /// </summary>
    [System.ComponentModel.DisplayName("AC_NAME")]
    public string Name
    {
      get { return GetProperty(NameProperty, _name); }
      set { SetProperty(NameProperty, ref _name, value); }
    }

    /// <summary>
    /// AC_CAPTION
    /// </summary>
    public static readonly Phenix.Core.Mapping.PropertyInfo<string> CaptionProperty = RegisterProperty<string>(c => c.Caption);
    [Phenix.Core.Mapping.Field(FriendlyName = "AC_CAPTION", IsPrimaryKey = false, IsWatermarkColumn = false, NeedUpdate = true, OverwritingOnUpdate = false)]
    private string _caption;
    /// <summary>
    /// AC_CAPTION
    /// </summary>
    [System.ComponentModel.DisplayName("AC_CAPTION")]
    public new string Caption
    {
      get { return GetProperty(CaptionProperty, _caption); }
      set { SetProperty(CaptionProperty, ref _caption, value); }
    }

    /// <summary>
    /// AC_CAPTIONCONFIGURED
    /// </summary>
    public static readonly Phenix.Core.Mapping.PropertyInfo<int?> CaptionconfiguredProperty = RegisterProperty<int?>(c => c.Captionconfigured);
    [Phenix.Core.Mapping.Field(FriendlyName = "AC_CAPTIONCONFIGURED", IsPrimaryKey = false, IsWatermarkColumn = false, NeedUpdate = true, OverwritingOnUpdate = false)]
    private int? _captionconfigured;
    /// <summary>
    /// AC_CAPTIONCONFIGURED
    /// </summary>
    [System.ComponentModel.DisplayName("AC_CAPTIONCONFIGURED")]
    public int? Captionconfigured
    {
      get { return GetProperty(CaptionconfiguredProperty, _captionconfigured); }
      set { SetProperty(CaptionconfiguredProperty, ref _captionconfigured, value); }
    }

  }
}