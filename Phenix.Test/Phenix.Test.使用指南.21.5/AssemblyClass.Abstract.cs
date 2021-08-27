using System;
using Phenix.Business;

/* 
   builder:    phenixiii
   build time: 2016-05-26 14:40:53
   notes:      
*/

namespace Phenix.Test.使用指南._21._5.Business
{
  /// <summary>
  /// </summary>
  [Phenix.Core.Mapping.ClassAttribute("PH_AssemblyClass", FriendlyName = ""), System.Serializable]
  [System.ComponentModel.DisplayName("")]
  public abstract class AssemblyClass<T> : Phenix.Business.BusinessBase<T>
    where T : AssemblyClass<T>
  {
    [System.ComponentModel.Browsable(false)]
    [System.ComponentModel.DataAnnotations.Display(AutoGenerateField = false)]
    public override string PrimaryKey
    {
      get { return base.PrimaryKey; }
    }

    /// <summary>
    /// AC_ID
    /// </summary>
    public static readonly Phenix.Business.PropertyInfo<long?> AC_IDProperty = RegisterProperty<long?>(c => c.AC_ID);
    [Phenix.Core.Mapping.Field(FriendlyName = "AC_ID", TableName = "PH_AssemblyClass", ColumnName = "AC_ID", IsPrimaryKey = true, NeedUpdate = true)]
    private long? _AC_ID;
    /// <summary>
    /// AC_ID
    /// </summary>
    [System.ComponentModel.DataAnnotations.Display(Name = "AC_ID")]
    [System.ComponentModel.DisplayName("AC_ID")]
    public long? AC_ID
    {
      get { return GetProperty(AC_IDProperty, _AC_ID); }
    }

    /// <summary>
    /// AC_AS_ID
    /// </summary>
    public static readonly Phenix.Business.PropertyInfo<long?> AC_AS_IDProperty = RegisterProperty<long?>(c => c.AC_AS_ID);
    //* 申明外键关联的表和字段
    [Phenix.Core.Mapping.FieldLink("PH_ASSEMBLY", "AS_ID")]
    [Phenix.Core.Mapping.Field(FriendlyName = "AC_AS_ID", TableName = "PH_AssemblyClass", ColumnName = "AC_AS_ID", NeedUpdate = true)]
    private long? _AC_AS_ID;
    /// <summary>
    /// AC_AS_ID
    /// </summary>
    [System.ComponentModel.DataAnnotations.Display(Name = "AC_AS_ID")]
    [System.ComponentModel.DisplayName("AC_AS_ID")]
    public long? AC_AS_ID
    {
      get { return GetProperty(AC_AS_IDProperty, _AC_AS_ID); }
      set { SetProperty(AC_AS_IDProperty, ref _AC_AS_ID, value); }
    }

    /// <summary>
    /// AC_Name
    /// </summary>
    public static readonly Phenix.Business.PropertyInfo<string> NameProperty = RegisterProperty<string>(c => c.Name);
    [Phenix.Core.Mapping.Field(FriendlyName = "AC_Name", Alias = "AC_Name", TableName = "PH_AssemblyClass", ColumnName = "AC_Name", NeedUpdate = true, IsNameColumn = true, InLookUpColumn = true, InLookUpColumnDisplay = true)]
    private string _name;
    /// <summary>
    /// AC_Name
    /// </summary>
    [System.ComponentModel.DataAnnotations.Display(Name = "AC_Name")]
    [System.ComponentModel.DisplayName("AC_Name")]
    public string Name
    {
      get { return GetProperty(NameProperty, _name); }
      set { SetProperty(NameProperty, ref _name, value); }
    }

    /// <summary>
    /// AC_Caption
    /// </summary>
    public static readonly Phenix.Business.PropertyInfo<string> CaptionProperty = RegisterProperty<string>(c => c.Caption);
    [Phenix.Core.Mapping.Field(FriendlyName = "AC_Caption", Alias = "AC_Caption", TableName = "PH_AssemblyClass", ColumnName = "AC_Caption", NeedUpdate = true)]
    private string _caption;
    /// <summary>
    /// AC_Caption
    /// </summary>
    [System.ComponentModel.DataAnnotations.Display(Name = "AC_Caption")]
    [System.ComponentModel.DisplayName("AC_Caption")]
    public new string Caption
    {
      get { return GetProperty(CaptionProperty, _caption); }
      set { SetProperty(CaptionProperty, ref _caption, value); }
    }

    /// <summary>
    /// AC_CaptionConfigured
    /// </summary>
    public static readonly Phenix.Business.PropertyInfo<int?> CaptionconfiguredProperty = RegisterProperty<int?>(c => c.Captionconfigured);
    [Phenix.Core.Mapping.Field(FriendlyName = "AC_CaptionConfigured", Alias = "AC_CaptionConfigured", TableName = "PH_AssemblyClass", ColumnName = "AC_CaptionConfigured", NeedUpdate = true)]
    private int? _captionconfigured;
    /// <summary>
    /// AC_CaptionConfigured
    /// </summary>
    [System.ComponentModel.DataAnnotations.Display(Name = "AC_CaptionConfigured")]
    [System.ComponentModel.DisplayName("AC_CaptionConfigured")]
    public int? Captionconfigured
    {
      get { return GetProperty(CaptionconfiguredProperty, _captionconfigured); }
      set { SetProperty(CaptionconfiguredProperty, ref _captionconfigured, value); }
    }

    /// <summary>
    /// New
    /// </summary>
    public static T New(long? AC_AS_ID, string name, string caption, int? captionconfigured)
    {
      T result = NewPure();
      result._AC_AS_ID = AC_AS_ID;
      result._name = name;
      result._caption = caption;
      result._captionconfigured = captionconfigured;
      return result;
    }
  
    protected AssemblyClass()
    {
      //禁止添加代码
    }

    protected AssemblyClass(bool? isNew, bool? isSelfDirty, bool? isSelfDeleted, long? AC_ID, long? AC_AS_ID, string name, string caption, int? captionconfigured)
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
  }
}