using System;

namespace Phenix.Test.使用指南._11._5._1._1
{
  /// <summary>
  /// Assembly
  /// </summary>
  [Serializable]
  [Phenix.Core.Mapping.ReadOnly]
  public class AssemblyReadOnly : Assembly<AssemblyReadOnly>
  {
    private AssemblyReadOnly()
    {
      //禁止添加代码
    }
  }

  /// <summary>
  /// Assembly清单
  /// </summary>
  [Serializable]
  public class AssemblyReadOnlyList : Phenix.Business.BusinessListBase<AssemblyReadOnlyList, AssemblyReadOnly>
  {
    private AssemblyReadOnlyList()
    {
      //禁止添加代码
    }
  }

  /// <summary>
  /// Assembly
  /// </summary>
  [Serializable]
  public class Assembly : Assembly<Assembly>
  {
    private Assembly()
    {
      //禁止添加代码
    }

    #region 属性

    //* 组合关系的从业务对象集合
    /// <summary>
    /// 类信息
    /// </summary>
    public AssemblyClassList AssemblyClasses
    {
      get { return GetCompositionDetail<AssemblyClassList, AssemblyClass>(); }
    }

    #endregion
  }

  /// <summary>
  /// Assembly清单
  /// </summary>
  [Serializable]
  public class AssemblyList : Phenix.Business.BusinessListBase<AssemblyList, Assembly>
  {
    private AssemblyList()
    {
      //禁止添加代码
    }
  }

  /// <summary>
  /// Assembly
  /// </summary>
  [Phenix.Core.Mapping.ClassAttribute("PH_ASSEMBLY", FriendlyName = "Assembly"), System.ComponentModel.DisplayNameAttribute("Assembly"), System.SerializableAttribute()]
  public abstract class Assembly<T> : Phenix.Business.BusinessBase<T> where T : Assembly<T>
  {
    /// <summary>
    /// AS_ID
    /// </summary>
    public static readonly Phenix.Business.PropertyInfo<long?> AS_IDProperty = RegisterProperty<long?>(c => c.AS_ID);
    [Phenix.Core.Mapping.Field(FriendlyName = "AS_ID", TableName = "PH_ASSEMBLY", ColumnName = "AS_ID", IsPrimaryKey = true, NeedUpdate = true)]
    private long? _AS_ID;
    /// <summary>
    /// AS_ID
    /// </summary>
    [System.ComponentModel.DisplayName("AS_ID")]
    public long? AS_ID
    {
      get { return GetProperty(AS_IDProperty, _AS_ID); }
      internal set
      {
        SetProperty(AS_IDProperty, ref _AS_ID, value);
      }
    }

    [System.ComponentModel.Browsable(false)]
    [System.ComponentModel.DataAnnotations.Display(AutoGenerateField = false)]
    public override string PrimaryKey
    {
      get { return AS_ID.ToString(); }
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
    [System.ComponentModel.DisplayName("AS_CAPTION")]
    public string Caption
    {
      get { return GetProperty(CaptionProperty, _caption); }
      set { SetProperty(CaptionProperty, ref _caption, value); }
    }

    /// <summary>
    /// New
    /// </summary>
    public static T New(string name, string caption, int? enabled)
    {
      T result = NewPure();
      result._name = name;
      result._caption = caption;
      return result;
    }

    /// <summary>
    /// SetFieldValues
    /// </summary>
    protected void SetFieldValues(string name, string caption, int? enabled)
    {
      InitOldFieldValues();
      _name = name;
      _caption = caption;
      MarkDirty();
    }
  }
}
