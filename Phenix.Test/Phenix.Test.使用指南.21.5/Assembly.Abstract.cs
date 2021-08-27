using System;
using Phenix.Business;

/* 
   builder:    phenixiii
   build time: 2015-09-29 09:50:11
   notes:      
*/

namespace Phenix.Test.使用指南._21._5.Business
{
  /// <summary>
  /// </summary>
  [Phenix.Core.Mapping.ClassAttribute("PH_ASSEMBLY", FriendlyName = ""), System.Serializable]
  [System.ComponentModel.DisplayName("")]
  public abstract class Assembly<T> : Phenix.Business.BusinessBase<T>
    where T : Assembly<T>
  {
    protected Assembly()
    {
      //禁止添加代码
    }

    protected Assembly(bool? isNew, bool? isSelfDirty, bool? isSelfDeleted,
      long? AS_ID, string name, string caption)
      : base(isNew, isSelfDirty, isSelfDeleted)
    {
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

    [System.ComponentModel.Browsable(false)]
    [System.ComponentModel.DataAnnotations.Display(AutoGenerateField = false)]
    public override string PrimaryKey
    {
      get { return base.PrimaryKey; }
    }

    /// <summary>
    /// AS_ID
    /// </summary>
    public static readonly Phenix.Business.PropertyInfo<long?> AS_IDProperty = RegisterProperty<long?>(c => c.AS_ID);
    [Phenix.Core.Mapping.Field(FriendlyName = "AS_ID", TableName = "PH_ASSEMBLY", ColumnName = "AS_ID", IsPrimaryKey = true, NeedUpdate = true)]
    private long? _AS_ID;
    /// <summary>
    /// AS_ID
    /// </summary>
    [System.ComponentModel.DataAnnotations.Display(Name = "AS_ID")]
    [System.ComponentModel.DisplayName("AS_ID")]
    public long? AS_ID
    {
      get { return GetProperty(AS_IDProperty, _AS_ID); }
      internal set { SetProperty(AS_IDProperty, ref _AS_ID, value); }
    }

    /// <summary>
    /// AS_NAME
    /// </summary>
    public static readonly Phenix.Business.PropertyInfo<string> NameProperty = RegisterProperty<string>(c => c.Name);
    [Phenix.Core.Mapping.Field(FriendlyName = "AS_NAME", Alias = "AS_NAME", TableName = "PH_ASSEMBLY", ColumnName = "AS_NAME", NeedUpdate = true, IsNameColumn = true, InLookUpColumn = true, InLookUpColumnDisplay = true)]
    private string _name;
    /// <summary>
    /// AS_NAME
    /// </summary>
    [System.ComponentModel.DataAnnotations.Display(Name = "AS_NAME")]
    [System.ComponentModel.DisplayName("AS_NAME")]
    public string Name
    {
      get { return GetProperty(NameProperty, _name); }
      set { SetProperty(NameProperty, ref _name, value); }
    }

    /// <summary>
    /// AS_CAPTION
    /// </summary>
    public static readonly Phenix.Business.PropertyInfo<string> CaptionProperty = RegisterProperty<string>(c => c.Caption);
    [Phenix.Core.Mapping.Field(FriendlyName = "AS_CAPTION", Alias = "AS_CAPTION", TableName = "PH_ASSEMBLY", ColumnName = "AS_CAPTION", NeedUpdate = true)]
    private string _caption;
    /// <summary>
    /// AS_CAPTION
    /// </summary>
    [System.ComponentModel.DataAnnotations.Display(Name = "AS_CAPTION")]
    [System.ComponentModel.DisplayName("AS_CAPTION")]
    public new string Caption
    {
      get { return GetProperty(CaptionProperty, _caption); }
      set { SetProperty(CaptionProperty, ref _caption, value); }
    }

    /// <summary>
    /// New
    /// </summary>
    public static T New(string name, string caption)
    {
      T result = NewPure();
      result._name = name;
      result._caption = caption;
      return result;
    }
  }
}