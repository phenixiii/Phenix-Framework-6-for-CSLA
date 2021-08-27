using System;
using Phenix.Core.Data;

namespace Phenix.Test.使用指南._21._5
{
  /// <summary>
  /// 
  /// </summary>
  [Serializable]
  [Phenix.Core.Mapping.Class("Phenix.Test.使用指南._21._5.Business.AssemblyList", FriendlyName = "")]
  public class AssemblyList : EntityListPageBase<AssemblyList, Assembly>
  {
    /// <summary>
    /// 构建实体
    /// </summary>
    protected override object CreateInstance()
    {
      return new AssemblyList();
    }
  }

  /// <summary>
  /// 
  /// </summary>
  [Serializable]
  [Phenix.Core.Mapping.Class("Phenix.Test.使用指南._21._5.Business.Assembly", FriendlyName = "")]
  public class Assembly : EntityPageBase<Assembly>
  {
    private Assembly()
    {
      //禁止添加代码
    }

    [Newtonsoft.Json.JsonConstructor]
    private Assembly(bool? isNew, bool? isSelfDirty, bool? isSelfDeleted,
      AssemblyClassList assemblyClasses,
      long? AS_ID, string name, string caption)
      : base(isNew, isSelfDirty, isSelfDeleted)
    {
      _assemblyClasses = assemblyClasses;
      if (AS_ID != null)
      {
        _AS_ID = AS_ID;
        SetDirtyProperty(AS_IDProperty);
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
    }

    /// <summary>
    /// 构建实体
    /// </summary>
    protected override object CreateInstance()
    {
      return new Assembly();
    }

    #region 属性

    private AssemblyClassList _assemblyClasses;
    //* 可序列化
    //* 组合关系的从业务对象集合 
    /// <summary>
    /// 类信息
    /// </summary>
    [Phenix.Core.Mapping.Property(Serializable = true)]
    public AssemblyClassList AssemblyClasses
    {
      get { return _assemblyClasses; }
      set { _assemblyClasses = value; }
    }

    #endregion

    /// <summary>
    /// 主键值
    /// </summary>
    [System.ComponentModel.Browsable(false)]
    public override string PrimaryKey
    {
      get { return base.PrimaryKey; }
    }

    /// <summary>
    /// AS_ID
    /// </summary>
    public static readonly Phenix.Core.Mapping.PropertyInfo<long?> AS_IDProperty = RegisterProperty<long?>(c => c.AS_ID);
    [Phenix.Core.Mapping.Field(FriendlyName = "AS_ID", IsPrimaryKey = true, IsWatermarkColumn = false, NeedUpdate = true, OverwritingOnUpdate = false)]
    private long? _AS_ID;
    /// <summary>
    /// AS_ID
    /// </summary>
    [System.ComponentModel.DisplayName("AS_ID")]
    public long? AS_ID
    {
      get { return GetProperty(AS_IDProperty, _AS_ID); }
    }

    /// <summary>
    /// AS_NAME
    /// </summary>
    public static readonly Phenix.Core.Mapping.PropertyInfo<string> NameProperty = RegisterProperty<string>(c => c.Name);
    [Phenix.Core.Mapping.Field(FriendlyName = "AS_NAME", IsPrimaryKey = false, IsWatermarkColumn = false, NeedUpdate = true, OverwritingOnUpdate = false)]
    private string _name;
    /// <summary>
    /// AS_NAME
    /// </summary>
    [System.ComponentModel.DisplayName("AS_NAME")]
    public string Name
    {
      get { return GetProperty(NameProperty, _name); }
      set { SetProperty(NameProperty, ref _name, value); }
    }

    /// <summary>
    /// AS_CAPTION
    /// </summary>
    public static readonly Phenix.Core.Mapping.PropertyInfo<string> CaptionProperty = RegisterProperty<string>(c => c.Caption);
    [Phenix.Core.Mapping.Field(FriendlyName = "AS_CAPTION", IsPrimaryKey = false, IsWatermarkColumn = false, NeedUpdate = true, OverwritingOnUpdate = false)]
    private string _caption;
    /// <summary>
    /// AS_CAPTION
    /// </summary>
    [System.ComponentModel.DisplayName("AS_CAPTION")]
    public new string Caption
    {
      get { return GetProperty(CaptionProperty, _caption); }
      set { SetProperty(CaptionProperty, ref _caption, value); }
    }
  }
}