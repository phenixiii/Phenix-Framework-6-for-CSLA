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
  [Phenix.Core.Mapping.Class("PH_ASSEMBLY", FriendlyName = "")]
  public class AssemblyEasy : EntityBase<AssemblyEasy>
  {
    private AssemblyEasy()
    {
      //禁止添加代码
    }

    [Newtonsoft.Json.JsonConstructor]
    private AssemblyEasy(bool? isNew, bool? isSelfDirty, bool? isSelfDeleted, long? AS_ID, string name, string caption)
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

    /// <summary>
    /// 构建实体
    /// </summary>
    protected override object CreateInstance()
    {
      return new AssemblyEasy();
    }

    /// <summary>
    /// 主键值
    /// </summary>
    [System.ComponentModel.Browsable(false)]
    [System.ComponentModel.DataAnnotations.Display(AutoGenerateField = false)]
    public override string PrimaryKey
    {
      get { return AS_ID.ToString(); }
    }

    /// <summary>
    /// AS_ID
    /// </summary>
    public static readonly Phenix.Core.Mapping.PropertyInfo<long?> AS_IDProperty = RegisterProperty<long?>(c => c.AS_ID);
    [Phenix.Core.Mapping.Field(FriendlyName = "AS_ID", TableName = "PH_ASSEMBLY", ColumnName = "AS_ID", IsPrimaryKey = true, NeedUpdate = true)]
    private long? _AS_ID;
    /// <summary>
    /// AS_ID
    /// </summary>
    [System.ComponentModel.DataAnnotations.Display(Name = "AS_ID")]
    [System.ComponentModel.DisplayName("AS_ID")]
    public long? AS_ID
    {
      get { return _AS_ID; }
    }

    /// <summary>
    /// AS_NAME
    /// </summary>
    public static readonly Phenix.Core.Mapping.PropertyInfo<string> NameProperty = RegisterProperty<string>(c => c.Name);
    [Phenix.Core.Mapping.Field(FriendlyName = "AS_NAME", Alias = "AS_NAME", TableName = "PH_ASSEMBLY", ColumnName = "AS_NAME", NeedUpdate = true, IsNameColumn = true, InLookUpColumn = true, InLookUpColumnDisplay = true)]
    private string _name;
    /// <summary>
    /// AS_NAME
    /// </summary>
    [System.ComponentModel.DataAnnotations.Display(Name = "AS_NAME")]
    [System.ComponentModel.DisplayName("AS_NAME")]
    public string Name
    {
      get { return _name; }
      set { _name = value; }
    }

    /// <summary>
    /// AS_CAPTION
    /// </summary>
    public static readonly Phenix.Core.Mapping.PropertyInfo<string> CaptionProperty = RegisterProperty<string>(c => c.Caption);
    [Phenix.Core.Mapping.Field(FriendlyName = "AS_CAPTION", Alias = "AS_CAPTION", TableName = "PH_ASSEMBLY", ColumnName = "AS_CAPTION", NeedUpdate = true)]
    private string _caption;
    /// <summary>
    /// AS_CAPTION
    /// </summary>
    [System.ComponentModel.DataAnnotations.Display(Name = "AS_CAPTION")]
    [System.ComponentModel.DisplayName("AS_CAPTION")]
    public string Caption
    {
      get { return _caption; }
      set { _caption = value; }
    }

    /// <summary>
    /// New
    /// </summary>
    public static AssemblyEasy New(string name, string caption)
    {
      AssemblyEasy result = NewPure();
      result._name = name;
      result._caption = caption;
      return result;
    }
  }
}