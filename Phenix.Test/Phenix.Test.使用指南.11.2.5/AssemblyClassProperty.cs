using System;

namespace Phenix.Test.使用指南._11._2._5
{
  /// <summary>
  /// AssemblyClassProperty
  /// </summary>
  [Serializable]
  [Phenix.Core.Mapping.ReadOnly]
  public class AssemblyClassPropertyReadOnly : AssemblyClassProperty<AssemblyClassPropertyReadOnly>
  {
    private AssemblyClassPropertyReadOnly()
    {
      //禁止添加代码
    }
  }

  /// <summary>
  /// AssemblyClassProperty清单
  /// </summary>
  [Serializable]
  public class AssemblyClassPropertyReadOnlyList : Phenix.Business.BusinessListBase<AssemblyClassPropertyReadOnlyList, AssemblyClassPropertyReadOnly>
  {
    private AssemblyClassPropertyReadOnlyList()
    {
      //禁止添加代码
    }
  }

  /// <summary>
  /// AssemblyClassProperty
  /// </summary>
  [Serializable]
  public class AssemblyClassProperty : AssemblyClassProperty<AssemblyClassProperty>
  {
    private AssemblyClassProperty()
    {
      //禁止添加代码
    }
  }

  /// <summary>
  /// AssemblyClassProperty清单
  /// </summary>
  [Serializable]
  public class AssemblyClassPropertyList : Phenix.Business.BusinessListBase<AssemblyClassPropertyList, AssemblyClassProperty>
  {
    private AssemblyClassPropertyList()
    {
      //禁止添加代码
    }
  }

  /// <summary>
  /// AssemblyClassProperty
  /// </summary>
  [Phenix.Core.Mapping.ClassAttribute("PH_ASSEMBLYCLASSPROPERTY", FriendlyName = "AssemblyClassProperty"), System.ComponentModel.DisplayNameAttribute("AssemblyClassProperty"), System.SerializableAttribute()]
  public abstract class AssemblyClassProperty<T> : Phenix.Business.BusinessBase<T> where T : AssemblyClassProperty<T>
  {
    /// <summary>
    /// AP_ID
    /// </summary>
    public static readonly Phenix.Business.PropertyInfo<long?> AP_IDProperty = RegisterProperty<long?>(c => c.AP_ID);
    [Phenix.Core.Mapping.Field(FriendlyName = "AP_ID", TableName = "PH_ASSEMBLYCLASSPROPERTY", ColumnName = "AP_ID", IsPrimaryKey = true, NeedUpdate = true)]
    private long? _AP_ID;
    /// <summary>
    /// AP_ID
    /// </summary>
    [System.ComponentModel.DisplayName("AP_ID")]
    public long? AP_ID
    {
      get { return GetProperty(AP_IDProperty, _AP_ID); }
      internal set
      {
        SetProperty(AP_IDProperty, ref _AP_ID, value);
      }
    }

    [System.ComponentModel.Browsable(false)]
    [System.ComponentModel.DataAnnotations.Display(AutoGenerateField = false)]
    public override string PrimaryKey
    {
      get { return AP_ID.ToString(); }
    }

    /// <summary>
    /// AP_AC_ID
    /// </summary>
    public static readonly Phenix.Business.PropertyInfo<long?> AP_AC_IDProperty = RegisterProperty<long?>(c => c.AP_AC_ID);
    [Phenix.Core.Mapping.Field(FriendlyName = "AP_AC_ID", TableName = "PH_ASSEMBLYCLASSPROPERTY", ColumnName = "AP_AC_ID", NeedUpdate = true)]
    //* 因未构建物理外键，需显式申明关联主表
    [Phenix.Core.Mapping.FieldLink("PH_ASSEMBLYCLASS", "AC_ID")]
    private long? _AP_AC_ID;
    /// <summary>
    /// AP_AC_ID
    /// </summary>
    [System.ComponentModel.DisplayName("AP_AC_ID")]
    public long? AP_AC_ID
    {
      get { return GetProperty(AP_AC_IDProperty, _AP_AC_ID); }
      set { SetProperty(AP_AC_IDProperty, ref _AP_AC_ID, value); }
    }

    /// <summary>
    /// AP_NAME
    /// </summary>
    public static readonly Phenix.Business.PropertyInfo<string> NameProperty = RegisterProperty<string>(c => c.Name);
    [Phenix.Core.Mapping.Field(FriendlyName = "AP_NAME", Alias = "AP_NAME", TableName = "PH_ASSEMBLYCLASSPROPERTY", ColumnName = "AP_NAME", NeedUpdate = true, IsNameColumn = true, InLookUpColumn = true, InLookUpColumnDisplay = true)]
    private string _name;
    /// <summary>
    /// AP_NAME
    /// </summary>
    [System.ComponentModel.DisplayName("AP_NAME")]
    public string Name
    {
      get { return GetProperty(NameProperty, _name); }
      set { SetProperty(NameProperty, ref _name, value); }
    }

    /// <summary>
    /// AP_CAPTION
    /// </summary>
    public static readonly Phenix.Business.PropertyInfo<string> CaptionProperty = RegisterProperty<string>(c => c.Caption);
    [Phenix.Core.Mapping.Field(FriendlyName = "AP_CAPTION", Alias = "AP_CAPTION", TableName = "PH_ASSEMBLYCLASSPROPERTY", ColumnName = "AP_CAPTION", NeedUpdate = true)]
    private string _caption;
    /// <summary>
    /// AP_CAPTION
    /// </summary>
    [System.ComponentModel.DisplayName("AP_CAPTION")]
    public string Caption
    {
      get { return GetProperty(CaptionProperty, _caption); }
      set { SetProperty(CaptionProperty, ref _caption, value); }
    }

    /// <summary>
    /// AP_CAPTIONCONFIGURED
    /// </summary>
    public static readonly Phenix.Business.PropertyInfo<int?> CaptionconfiguredProperty = RegisterProperty<int?>(c => c.Captionconfigured);
    [Phenix.Core.Mapping.Field(FriendlyName = "AP_CAPTIONCONFIGURED", Alias = "AP_CAPTIONCONFIGURED", TableName = "PH_ASSEMBLYCLASSPROPERTY", ColumnName = "AP_CAPTIONCONFIGURED", NeedUpdate = true)]
    private int? _captionconfigured;
    /// <summary>
    /// AP_CAPTIONCONFIGURED
    /// </summary>
    [System.ComponentModel.DisplayName("AP_CAPTIONCONFIGURED")]
    public int? Captionconfigured
    {
      get { return GetProperty(CaptionconfiguredProperty, _captionconfigured); }
      set { SetProperty(CaptionconfiguredProperty, ref _captionconfigured, value); }
    }

    /// <summary>
    /// AP_PERMANENTEXECUTEMODIFY
    /// </summary>
    public static readonly Phenix.Business.PropertyInfo<int?> PermanentexecutemodifyProperty = RegisterProperty<int?>(c => c.Permanentexecutemodify);
    [Phenix.Core.Mapping.Field(FriendlyName = "AP_PERMANENTEXECUTEMODIFY", Alias = "AP_PERMANENTEXECUTEMODIFY", TableName = "PH_ASSEMBLYCLASSPROPERTY", ColumnName = "AP_PERMANENTEXECUTEMODIFY", NeedUpdate = true)]
    private int? _permanentexecutemodify;
    /// <summary>
    /// AP_PERMANENTEXECUTEMODIFY
    /// </summary>
    [System.ComponentModel.DisplayName("AP_PERMANENTEXECUTEMODIFY")]
    public int? Permanentexecutemodify
    {
      get { return GetProperty(PermanentexecutemodifyProperty, _permanentexecutemodify); }
      set { SetProperty(PermanentexecutemodifyProperty, ref _permanentexecutemodify, value); }
    }

    /// <summary>
    /// AP_PERMANENTEXECUTECONFIGURED
    /// </summary>
    public static readonly Phenix.Business.PropertyInfo<int?> PermanentexecuteconfiguredProperty = RegisterProperty<int?>(c => c.Permanentexecuteconfigured);
    [Phenix.Core.Mapping.Field(FriendlyName = "AP_PERMANENTEXECUTECONFIGURED", Alias = "AP_PERMANENTEXECUTECONFIGURED", TableName = "PH_ASSEMBLYCLASSPROPERTY", ColumnName = "AP_PERMANENTEXECUTECONFIGURED", NeedUpdate = true)]
    private int? _permanentexecuteconfigured;
    /// <summary>
    /// AP_PERMANENTEXECUTECONFIGURED
    /// </summary>
    [System.ComponentModel.DisplayName("AP_PERMANENTEXECUTECONFIGURED")]
    public int? Permanentexecuteconfigured
    {
      get { return GetProperty(PermanentexecuteconfiguredProperty, _permanentexecuteconfigured); }
      set { SetProperty(PermanentexecuteconfiguredProperty, ref _permanentexecuteconfigured, value); }
    }

    /// <summary>
    /// AP_CONFIGURABLE
    /// </summary>
    public static readonly Phenix.Business.PropertyInfo<int?> ConfigurableProperty = RegisterProperty<int?>(c => c.Configurable);
    [Phenix.Core.Mapping.Field(FriendlyName = "AP_CONFIGURABLE", Alias = "AP_CONFIGURABLE", TableName = "PH_ASSEMBLYCLASSPROPERTY", ColumnName = "AP_CONFIGURABLE", NeedUpdate = true)]
    private int? _configurable;
    /// <summary>
    /// AP_CONFIGURABLE
    /// </summary>
    [System.ComponentModel.DisplayName("AP_CONFIGURABLE")]
    public int? Configurable
    {
      get { return GetProperty(ConfigurableProperty, _configurable); }
      set { SetProperty(ConfigurableProperty, ref _configurable, value); }
    }

    /// <summary>
    /// AP_CONFIGVALUE
    /// </summary>
    public static readonly Phenix.Business.PropertyInfo<string> ConfigvalueProperty = RegisterProperty<string>(c => c.Configvalue);
    [Phenix.Core.Mapping.Field(FriendlyName = "AP_CONFIGVALUE", Alias = "AP_CONFIGVALUE", TableName = "PH_ASSEMBLYCLASSPROPERTY", ColumnName = "AP_CONFIGVALUE", NeedUpdate = true)]
    private string _configvalue;
    /// <summary>
    /// AP_CONFIGVALUE
    /// </summary>
    [System.ComponentModel.DisplayName("AP_CONFIGVALUE")]
    public string Configvalue
    {
      get { return GetProperty(ConfigvalueProperty, _configvalue); }
      set { SetProperty(ConfigvalueProperty, ref _configvalue, value); }
    }

    /// <summary>
    /// AP_INDEXNUMBER
    /// </summary>
    public static readonly Phenix.Business.PropertyInfo<int?> IndexnumberProperty = RegisterProperty<int?>(c => c.Indexnumber);
    [Phenix.Core.Mapping.Field(FriendlyName = "AP_INDEXNUMBER", Alias = "AP_INDEXNUMBER", TableName = "PH_ASSEMBLYCLASSPROPERTY", ColumnName = "AP_INDEXNUMBER", NeedUpdate = true)]
    private int? _indexnumber;
    /// <summary>
    /// AP_INDEXNUMBER
    /// </summary>
    [System.ComponentModel.DisplayName("AP_INDEXNUMBER")]
    public int? Indexnumber
    {
      get { return GetProperty(IndexnumberProperty, _indexnumber); }
      set { SetProperty(IndexnumberProperty, ref _indexnumber, value); }
    }

    /// <summary>
    /// AP_VISIBLE
    /// </summary>
    public static readonly Phenix.Business.PropertyInfo<int?> VisibleProperty = RegisterProperty<int?>(c => c.Visible);
    [Phenix.Core.Mapping.Field(FriendlyName = "AP_VISIBLE", Alias = "AP_VISIBLE", TableName = "PH_ASSEMBLYCLASSPROPERTY", ColumnName = "AP_VISIBLE", NeedUpdate = true)]
    private int? _visible;
    /// <summary>
    /// AP_VISIBLE
    /// </summary>
    [System.ComponentModel.DisplayName("AP_VISIBLE")]
    public int? Visible
    {
      get { return GetProperty(VisibleProperty, _visible); }
      set { SetProperty(VisibleProperty, ref _visible, value); }
    }

    /// <summary>
    /// AP_REQUIRED
    /// </summary>
    public static readonly Phenix.Business.PropertyInfo<int?> RequiredProperty = RegisterProperty<int?>(c => c.Required);
    [Phenix.Core.Mapping.Field(FriendlyName = "AP_REQUIRED", Alias = "AP_REQUIRED", TableName = "PH_ASSEMBLYCLASSPROPERTY", ColumnName = "AP_REQUIRED", NeedUpdate = true)]
    private int? _required;
    /// <summary>
    /// AP_REQUIRED
    /// </summary>
    [System.ComponentModel.DisplayName("AP_REQUIRED")]
    public int? Required
    {
      get { return GetProperty(RequiredProperty, _required); }
      set { SetProperty(RequiredProperty, ref _required, value); }
    }

    /// <summary>
    /// New
    /// </summary>
    public static T New(long? AP_AC_ID, string name, string caption, int? captionconfigured, int? permanentexecutemodify, int? permanentexecuteconfigured, int? configurable, string configvalue, int? indexnumber, int? visible, int? required)
    {
      T result = NewPure();
      result._AP_AC_ID = AP_AC_ID;
      result._name = name;
      result._caption = caption;
      result._captionconfigured = captionconfigured;
      result._permanentexecutemodify = permanentexecutemodify;
      result._permanentexecuteconfigured = permanentexecuteconfigured;
      result._configurable = configurable;
      result._configvalue = configvalue;
      result._indexnumber = indexnumber;
      result._visible = visible;
      result._required = required;
      return result;
    }

    /// <summary>
    /// SetFieldValues
    /// </summary>
    protected void SetFieldValues(long? AP_AC_ID, string name, string caption, int? captionconfigured, int? permanentexecutemodify, int? permanentexecuteconfigured, int? configurable, string configvalue, int? indexnumber, int? visible, int? required)
    {
      InitOldFieldValues();
      _AP_AC_ID = AP_AC_ID;
      _name = name;
      _caption = caption;
      _captionconfigured = captionconfigured;
      _permanentexecutemodify = permanentexecutemodify;
      _permanentexecuteconfigured = permanentexecuteconfigured;
      _configurable = configurable;
      _configvalue = configvalue;
      _indexnumber = indexnumber;
      _visible = visible;
      _required = required;
      MarkDirty();
    }
  }
}
