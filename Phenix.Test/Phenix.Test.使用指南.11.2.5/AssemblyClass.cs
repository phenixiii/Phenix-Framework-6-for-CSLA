﻿using System;

namespace Phenix.Test.使用指南._11._2._5
{
  /// <summary>
  /// AssemblyClass
  /// </summary>
  [Serializable]
  [Phenix.Core.Mapping.ReadOnly]
  public class AssemblyClassReadOnly : AssemblyClass<AssemblyClassReadOnly>
  {
    private AssemblyClassReadOnly()
    {
      //禁止添加代码
    }
  }

  /// <summary>
  /// AssemblyClass清单
  /// </summary>
  [Serializable]
  public class AssemblyClassReadOnlyList : Phenix.Business.BusinessListBase<AssemblyClassReadOnlyList, AssemblyClassReadOnly>
  {
    private AssemblyClassReadOnlyList()
    {
      //禁止添加代码
    }
  }

  /// <summary>
  /// AssemblyClass
  /// </summary>
  [Serializable]
  public class AssemblyClass : AssemblyClass<AssemblyClass>
  {
    private AssemblyClass()
    {
      //禁止添加代码
    }

    #region 属性

    //* 组合关系的从业务对象集合
    /// <summary>
    /// 类属性
    /// </summary>
    public AssemblyClassPropertyList AssemblyClassProperties
    {
      get { return GetCompositionDetail<AssemblyClassPropertyList, AssemblyClassProperty>(); }
    }

    #endregion
  }

  /// <summary>
  /// AssemblyClass清单
  /// </summary>
  [Serializable]
  public class AssemblyClassList : Phenix.Business.BusinessListBase<AssemblyClassList, AssemblyClass>
  {
    private AssemblyClassList()
    {
      //禁止添加代码
    }
  }

  /// <summary>
  /// AssemblyClass
  /// </summary>
  [Phenix.Core.Mapping.ClassAttribute("PH_ASSEMBLYCLASS", FetchScript = "PH_ASSEMBLYCLASS_V", FriendlyName = "AssemblyClass"), System.ComponentModel.DisplayNameAttribute("AssemblyClass"), System.SerializableAttribute()]
  public abstract class AssemblyClass<T> : Phenix.Business.BusinessBase<T> where T : AssemblyClass<T>
  {
    /// <summary>
    /// AC_ID
    /// </summary>
    public static readonly Phenix.Business.PropertyInfo<long?> AC_IDProperty = RegisterProperty<long?>(c => c.AC_ID);
    [Phenix.Core.Mapping.Field(FriendlyName = "AC_ID", TableName = "PH_ASSEMBLYCLASS", ColumnName = "AC_ID", IsPrimaryKey = true, NeedUpdate = true)]
    private long? _AC_ID;
    /// <summary>
    /// AC_ID
    /// </summary>
    [System.ComponentModel.DisplayName("AC_ID")]
    public long? AC_ID
    {
      get { return GetProperty(AC_IDProperty, _AC_ID); }
      internal set
      {
        SetProperty(AC_IDProperty, ref _AC_ID, value);
      }
    }

    [System.ComponentModel.Browsable(false)]
    [System.ComponentModel.DataAnnotations.Display(AutoGenerateField = false)]
    public override string PrimaryKey
    {
      get { return AC_ID.ToString(); }
    }

    /// <summary>
    /// AC_AS_ID
    /// </summary>
    public static readonly Phenix.Business.PropertyInfo<long?> AC_AS_IDProperty = RegisterProperty<long?>(c => c.AC_AS_ID);
    [Phenix.Core.Mapping.Field(FriendlyName = "AC_AS_ID", TableName = "PH_ASSEMBLYCLASS", ColumnName = "AC_AS_ID", NeedUpdate = true)]
    //* 因未构建物理外键，需显式申明关联主表
    [Phenix.Core.Mapping.FieldLink("PH_ASSEMBLY", "AS_ID")]
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
    public static readonly Phenix.Business.PropertyInfo<string> NameProperty = RegisterProperty<string>(c => c.Name);
    [Phenix.Core.Mapping.Field(FriendlyName = "AC_NAME", Alias = "AC_NAME", TableName = "PH_ASSEMBLYCLASS", ColumnName = "AC_NAME", NeedUpdate = true, IsNameColumn = true, InLookUpColumn = true, InLookUpColumnDisplay = true)]
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
    public static readonly Phenix.Business.PropertyInfo<string> CaptionProperty = RegisterProperty<string>(c => c.Caption);
    [Phenix.Core.Mapping.Field(FriendlyName = "AC_CAPTION", Alias = "AC_CAPTION", TableName = "PH_ASSEMBLYCLASS", ColumnName = "AC_CAPTION", NeedUpdate = true)]
    private string _caption;
    /// <summary>
    /// AC_CAPTION
    /// </summary>
    [System.ComponentModel.DisplayName("AC_CAPTION")]
    public string Caption
    {
      get { return GetProperty(CaptionProperty, _caption); }
      set { SetProperty(CaptionProperty, ref _caption, value); }
    }

    /// <summary>
    /// AC_TYPE
    /// </summary>
    public static readonly Phenix.Business.PropertyInfo<int?> TypeProperty = RegisterProperty<int?>(c => c.Type);
    [Phenix.Core.Mapping.Field(FriendlyName = "AC_TYPE", Alias = "AC_TYPE", TableName = "PH_ASSEMBLYCLASS", ColumnName = "AC_TYPE", NeedUpdate = true)]
    private int? _type;
    /// <summary>
    /// AC_TYPE
    /// </summary>
    [System.ComponentModel.DisplayName("AC_TYPE")]
    public int? Type
    {
      get { return GetProperty(TypeProperty, _type); }
      set { SetProperty(TypeProperty, ref _type, value); }
    }

    /// <summary>
    /// AS_NAME
    /// </summary>
    public static readonly Phenix.Business.PropertyInfo<string> Name_Property = RegisterProperty<string>(c => c.Name_);
    [Phenix.Core.Mapping.Field(FriendlyName = "AS_NAME", Alias = "AS_NAME", TableName = "PH_ASSEMBLY", ColumnName = "AS_NAME")]
    private string _name_;
    /// <summary>
    /// AS_NAME
    /// </summary>
    [System.ComponentModel.DisplayName("AS_NAME")]
    public string Name_
    {
      get { return GetProperty(Name_Property, _name_); }
      set { SetProperty(Name_Property, ref _name_, value); }
    }

    /// <summary>
    /// AS_CAPTION
    /// </summary>
    public static readonly Phenix.Business.PropertyInfo<string> Caption_Property = RegisterProperty<string>(c => c.Caption_);
    [Phenix.Core.Mapping.Field(FriendlyName = "AS_CAPTION", Alias = "AS_CAPTION", TableName = "PH_ASSEMBLY", ColumnName = "AS_CAPTION")]
    private string _caption_;
    /// <summary>
    /// AS_CAPTION
    /// </summary>
    [System.ComponentModel.DisplayName("AS_CAPTION")]
    public string Caption_
    {
      get { return GetProperty(Caption_Property, _caption_); }
      set { SetProperty(Caption_Property, ref _caption_, value); }
    }

    /// <summary>
    /// New
    /// </summary>
    public static T New(long? AC_AS_ID, string name, string caption, int? type, string name_, string caption_)
    {
      T result = NewPure();
      result._AC_AS_ID = AC_AS_ID;
      result._name = name;
      result._caption = caption;
      result._type = type;
      result._name_ = name_;
      result._caption_ = caption_;
      return result;
    }

    /// <summary>
    /// SetFieldValues
    /// </summary>
    protected void SetFieldValues(long? AC_AS_ID, string name, string caption, int? type, string name_, string caption_)
    {
      InitOldFieldValues();
      _AC_AS_ID = AC_AS_ID;
      _name = name;
      _caption = caption;
      _type = type;
      _name_ = name_;
      _caption_ = caption_;
      MarkDirty();
    }
  }
}
