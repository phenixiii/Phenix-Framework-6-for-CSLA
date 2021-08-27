using System;

namespace Phenix.Security.Business
{
  /// <summary>
  /// 类属性配置值
  /// </summary>
  [Serializable]
  [Phenix.Core.Mapping.ReadOnly]
  public class AssemblyClassPropertyValueReadOnly : AssemblyClassPropertyValue<AssemblyClassPropertyValueReadOnly>
  {
  }

  /// <summary>
  /// 类属性配置值清单
  /// </summary>
  [Serializable]
  public class AssemblyClassPropertyValueReadOnlyList : Phenix.Business.BusinessListBase<AssemblyClassPropertyValueReadOnlyList, AssemblyClassPropertyValueReadOnly>
  {
  }

  /// <summary>
  /// 类属性配置值
  /// </summary>
  [Serializable]
  public class AssemblyClassPropertyValue : AssemblyClassPropertyValue<AssemblyClassPropertyValue>
  {
  }

  /// <summary>
  /// 类属性配置值清单
  /// </summary>
  [Serializable]
  public class AssemblyClassPropertyValueList : Phenix.Business.BusinessListBase<AssemblyClassPropertyValueList, AssemblyClassPropertyValue>
  {
  }

  /// <summary>
  /// 类属性配置值
  /// </summary>
  [Phenix.Core.Mapping.ClassAttribute("PH_ASSEMBLYCLASSPROPERTY_VALUE", FriendlyName = "类属性配置值"), System.SerializableAttribute(), System.ComponentModel.DisplayNameAttribute("类属性配置值")]
  public abstract class AssemblyClassPropertyValue<T> : Phenix.Business.BusinessBase<T> where T : AssemblyClassPropertyValue<T>
  {
    /// <summary>
    /// AV_ID
    /// </summary>
    public static readonly Phenix.Business.PropertyInfo<long?> AV_IDProperty = RegisterProperty<long?>(c => c.AV_ID);
    [Phenix.Core.Mapping.Field(FriendlyName = "AV_ID", TableName = "PH_ASSEMBLYCLASSPROPERTY_VALUE", ColumnName = "AV_ID", IsPrimaryKey = true, NeedUpdate = true)]
    private long? _AV_ID;
    /// <summary>
    /// AV_ID
    /// </summary>
    [System.ComponentModel.DisplayName("AV_ID")]
    public long? AV_ID
    {
      get { return GetProperty(AV_IDProperty, _AV_ID); }
      internal set
      {
        SetProperty(AV_IDProperty, ref _AV_ID, value);
      }
    }

    [System.ComponentModel.Browsable(false)]
    [System.ComponentModel.DataAnnotations.Display(AutoGenerateField = false)]
    public override string PrimaryKey
    {
      get { return AV_ID.ToString(); }
    }

    /// <summary>
    /// 所属类属性
    /// </summary>
    public static readonly Phenix.Business.PropertyInfo<long?> AV_AP_IDProperty = RegisterProperty<long?>(c => c.AV_AP_ID);
    [Phenix.Core.Mapping.Field(FriendlyName = "所属类属性", TableName = "PH_ASSEMBLYCLASSPROPERTY_VALUE", ColumnName = "AV_AP_ID", NeedUpdate = true)]
    [Phenix.Core.Mapping.FieldLink("PH_ASSEMBLYCLASSPROPERTY", "AP_ID")]
    private long? _AV_AP_ID;
    /// <summary>
    /// 所属类属性
    /// </summary>
    [System.ComponentModel.DisplayName("所属类属性")]
    public long? AV_AP_ID
    {
      get { return GetProperty(AV_AP_IDProperty, _AV_AP_ID); }
      set { SetProperty(AV_AP_IDProperty, ref _AV_AP_ID, value); }
    }

    /// <summary>
    /// 配置键
    /// </summary>
    public static readonly Phenix.Business.PropertyInfo<string> ConfigKeyProperty = RegisterProperty<string>(c => c.ConfigKey);
    [Phenix.Core.Mapping.Field(FriendlyName = "配置键", Alias = "AV_CONFIGKEY", TableName = "PH_ASSEMBLYCLASSPROPERTY_VALUE", ColumnName = "AV_CONFIGKEY", NeedUpdate = true)]
    private string _configKey;
    /// <summary>
    /// 配置键
    /// </summary>
    [System.ComponentModel.DisplayName("配置键")]
    public string ConfigKey
    {
      get { return GetProperty(ConfigKeyProperty, _configKey); }
      set { SetProperty(ConfigKeyProperty, ref _configKey, value); }
    }

    /// <summary>
    /// 是否可配置的
    /// </summary>
    public static readonly Phenix.Business.PropertyInfo<bool?> ConfigurableProperty = RegisterProperty<bool?>(c => c.Configurable);
    [Phenix.Core.Mapping.Field(FriendlyName = "是否可配置的", Alias = "AV_CONFIGURABLE", TableName = "PH_ASSEMBLYCLASSPROPERTY_VALUE", ColumnName = "AV_CONFIGURABLE", NeedUpdate = true)]
    private bool? _configurable;
    /// <summary>
    /// 是否可配置的
    /// </summary>
    [System.ComponentModel.DisplayName("是否可配置的")]
    public bool? Configurable
    {
      get { return GetProperty(ConfigurableProperty, _configurable); }
      set { SetProperty(ConfigurableProperty, ref _configurable, value); }
    }

    /// <summary>
    /// 配置值
    /// </summary>
    public static readonly Phenix.Business.PropertyInfo<string> ConfigValueProperty = RegisterProperty<string>(c => c.ConfigValue);
    [Phenix.Core.Mapping.Field(FriendlyName = "配置值", Alias = "AV_CONFIGVALUE", TableName = "PH_ASSEMBLYCLASSPROPERTY_VALUE", ColumnName = "AV_CONFIGVALUE", NeedUpdate = true)]
    private string _configValue;
    /// <summary>
    /// 配置值
    /// </summary>
    [System.ComponentModel.DisplayName("配置值")]
    public string ConfigValue
    {
      get { return GetProperty(ConfigValueProperty, _configValue); }
      set { SetProperty(ConfigValueProperty, ref _configValue, value); }
    }
  }
}
