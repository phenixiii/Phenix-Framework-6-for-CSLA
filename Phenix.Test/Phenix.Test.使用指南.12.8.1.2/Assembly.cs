using System;

namespace Phenix.Test.使用指南._12._8._1._2
{
  /// <summary>
  /// Assembly
  /// </summary>
  [Serializable]
  public class Assembly : Assembly<Assembly>
  {
  }

  /// <summary>
  /// Assembly清单
  /// </summary>
  [Serializable]
  public class AssemblyList : Phenix.Business.BusinessListBase<AssemblyList, Assembly>
  {
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
    public static readonly Phenix.Business.PropertyInfo<string> AssemblyNameProperty = RegisterProperty<string>(c => c.AssemblyName);
    [Phenix.Core.Mapping.Field(FriendlyName = "AS_NAME", Alias = "AS_NAME", TableName = "PH_ASSEMBLY", ColumnName = "AS_NAME", NeedUpdate = true, IsNameColumn = true, InLookUpColumn = true, InLookUpColumnDisplay = true)]
    private string _assemblyName;
    /// <summary>
    /// AS_NAME
    /// </summary>
    [System.ComponentModel.DisplayName("AS_NAME")]
    public string AssemblyName
    {
      get { return GetProperty(AssemblyNameProperty, _assemblyName); }
      set { SetProperty(AssemblyNameProperty, ref _assemblyName, value); }
    }

    /// <summary>
    /// AS_CAPTION
    /// </summary>
    public static readonly Phenix.Business.PropertyInfo<string> AssemblyCaptionProperty = RegisterProperty<string>(c => c.AssemblyCaption);
    [Phenix.Core.Mapping.Field(FriendlyName = "AS_CAPTION", Alias = "AS_CAPTION", TableName = "PH_ASSEMBLY", ColumnName = "AS_CAPTION", NeedUpdate = true)]
    private string _assemblyCaption;
    /// <summary>
    /// AS_CAPTION
    /// </summary>
    [System.ComponentModel.DisplayName("AS_CAPTION")]
    public string AssemblyCaption
    {
      get { return GetProperty(AssemblyCaptionProperty, _assemblyCaption); }
      set { SetProperty(AssemblyCaptionProperty, ref _assemblyCaption, value); }
    }

    /// <summary>
    /// New
    /// </summary>
    public static T New(string name, string caption)
    {
      T result = NewPure();
      result._assemblyName = name;
      result._assemblyCaption = caption;
      return result;
    }

    /// <summary>
    /// SetFieldValues
    /// </summary>
    protected void SetFieldValues(string name, string caption)
    {
      InitOldFieldValues();
      _assemblyName = name;
      _assemblyCaption = caption;
      MarkDirty();
    }
  }
}
