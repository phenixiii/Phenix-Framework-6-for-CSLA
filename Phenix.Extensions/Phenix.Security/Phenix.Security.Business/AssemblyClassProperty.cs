using System;

namespace Phenix.Security.Business
{
  /// <summary>
  /// 类属性
  /// </summary>
  [Serializable]
  [Phenix.Core.Mapping.ReadOnly]
  public class AssemblyClassPropertyReadOnly : AssemblyClassProperty<AssemblyClassPropertyReadOnly>
  {
  }

  /// <summary>
  /// 类属性清单
  /// </summary>
  [Serializable]
  public class AssemblyClassPropertyReadOnlyList : Phenix.Business.BusinessListBase<AssemblyClassPropertyReadOnlyList, AssemblyClassPropertyReadOnly>
  {
  }

  /// <summary>
  /// 类属性
  /// </summary>
  [Serializable]
  public class AssemblyClassProperty : AssemblyClassProperty<AssemblyClassProperty>
  {
    #region 属性

    /// <summary>
    /// 类属性配置值
    /// </summary>
    public AssemblyClassPropertyValueList AssemblyClassPropertyValues
    {
      get { return GetCompositionDetail<AssemblyClassPropertyValueList, AssemblyClassPropertyValue>(true); }
    }

    #region 勾选角色全集

    /// <summary>
    /// 分配的角色
    /// </summary>
    public AssemblyClassPropertyRoleList AssemblyClassPropertyRoles
    {
      get { return GetCompositionDetail<AssemblyClassPropertyRoleList, AssemblyClassPropertyRole>(AssemblyClassPropertyRoleList.Fetch()); }
    }

    /// <summary>
    /// 供勾选的角色全集
    /// </summary>
    public SelectableRoleForAssemblyClassPropertyList SelectableRoles
    {
      get 
      {
        AssemblyClass assemblyClass = MasterBusiness as AssemblyClass;
        if (assemblyClass != null && !(assemblyClass.Authorised ?? true))
          return new SelectableRoleForAssemblyClassPropertyList();
        return AssemblyClassPropertyRoles.CollatingSelectableList<SelectableRoleForAssemblyClassPropertyList, SelectableRoleForAssemblyClassProperty>(
          SelectableRoleForAssemblyClassPropertyList.Fetch(), !Phenix.Core.Security.UserIdentity.CurrentIdentity.EmptyRolesIsDeny); 
      }
    }

    /// <summary>
    /// 是否允许全选角色全集
    /// </summary>
    [System.ComponentModel.Browsable(false)]
    [System.ComponentModel.DataAnnotations.Display(AutoGenerateField = false)]
    public Phenix.Core.Data.BooleanOption AllowSelectAllRolesOption
    {
      get { return Owner.EditModeOption; }
    }

    /// <summary>
    /// 是否允许反选角色全集
    /// </summary>
    [System.ComponentModel.Browsable(false)]
    [System.ComponentModel.DataAnnotations.Display(AutoGenerateField = false)]
    public Phenix.Core.Data.BooleanOption AllowInverseAllRolesOption
    {
      get
      {
        return Owner.EditMode && (Phenix.Core.Security.UserIdentity.CurrentIdentity.EmptyRolesIsDeny || AssemblyClassPropertyRoles.Count > 0)
          ? Phenix.Core.Data.BooleanOption.Y
          : Phenix.Core.Data.BooleanOption.N;
      }
    }

    #endregion

    #endregion
  }

  /// <summary>
  /// 类属性清单
  /// </summary>
  [Serializable]
  public class AssemblyClassPropertyList : Phenix.Business.BusinessListBase<AssemblyClassPropertyList, AssemblyClassProperty>
  {
    /// <summary>
    /// 是否允许添加业务对象
    /// </summary>
    [System.ComponentModel.Browsable(false)]
    [System.ComponentModel.DataAnnotations.Display(AutoGenerateField = false)]
    public override bool AllowAddItem
    {
      get { return false; }
    }
  }

  /// <summary>
  /// 类属性
  /// </summary>
  [Phenix.Core.Mapping.ClassAttribute("PH_ASSEMBLYCLASSPROPERTY", FriendlyName = "类属性"), System.SerializableAttribute(), System.ComponentModel.DisplayNameAttribute("类属性")]
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
      //internal set
      //{
      //  SetProperty(AP_IDProperty, ref _AP_ID, value);
      //}
    }

    [System.ComponentModel.Browsable(false)]
    [System.ComponentModel.DataAnnotations.Display(AutoGenerateField = false)]
    public override string PrimaryKey
    {
      get { return AP_ID.ToString(); }
    }

    /// <summary>
    /// 所属类
    /// </summary>
    public static readonly Phenix.Business.PropertyInfo<long?> AP_AC_IDProperty = RegisterProperty<long?>(c => c.AP_AC_ID);
    [Phenix.Core.Mapping.Field(FriendlyName = "所属类", TableName = "PH_ASSEMBLYCLASSPROPERTY", ColumnName = "AP_AC_ID", NeedUpdate = true)]
    [Phenix.Core.Mapping.FieldLink("PH_ASSEMBLYCLASS", "AC_ID")]
    private long? _AP_AC_ID;
    /// <summary>
    /// 所属类
    /// </summary>
    [System.ComponentModel.DisplayName("所属类")]
    public long? AP_AC_ID
    {
      get { return GetProperty(AP_AC_IDProperty, _AP_AC_ID); }
      //set { SetProperty(AP_AC_IDProperty, ref _AP_AC_ID, value); }
    }

    /// <summary>
    /// 名称
    /// </summary>
    public static readonly Phenix.Business.PropertyInfo<string> NameProperty = RegisterProperty<string>(c => c.Name);
    [Phenix.Core.Mapping.Field(FriendlyName = "名称", Alias = "AP_NAME", TableName = "PH_ASSEMBLYCLASSPROPERTY", ColumnName = "AP_NAME", NeedUpdate = true, IsNameColumn = true, InLookUpColumn = true, InLookUpColumnDisplay = true)]
    private string _name;
    /// <summary>
    /// 名称
    /// </summary>
    [System.ComponentModel.DisplayName("名称")]
    public string Name
    {
      get { return GetProperty(NameProperty, _name); }
      //set { SetProperty(NameProperty, ref _name, value); }
    }

    /// <summary>
    /// 标签
    /// </summary>
    public static readonly Phenix.Business.PropertyInfo<string> CaptionProperty = RegisterProperty<string>(c => c.Caption);
    [Phenix.Core.Mapping.Field(FriendlyName = "标签", Alias = "AP_CAPTION", TableName = "PH_ASSEMBLYCLASSPROPERTY", ColumnName = "AP_CAPTION", NeedUpdate = true)]
    private string _caption;
    /// <summary>
    /// 标签
    /// </summary>
    [System.ComponentModel.DisplayName("标签")]
    public new string Caption
    {
      get { return GetProperty(CaptionProperty, _caption); }
      set { SetProperty(CaptionProperty, ref _caption, value); }
    }

    /// <summary>
    /// 是否可配置的
    /// </summary>
    public static readonly Phenix.Business.PropertyInfo<bool?> ConfigurableProperty = RegisterProperty<bool?>(c => c.Configurable);
    [Phenix.Core.Mapping.Field(FriendlyName = "是否可配置的", Alias = "AP_CONFIGURABLE", TableName = "PH_ASSEMBLYCLASSPROPERTY", ColumnName = "AP_CONFIGURABLE", NeedUpdate = true)]
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
    /// 配置的值
    /// </summary>
    public static readonly Phenix.Business.PropertyInfo<string> ConfigValueProperty = RegisterProperty<string>(c => c.ConfigValue);
    [Phenix.Core.Mapping.Field(FriendlyName = "配置的值", Alias = "AP_CONFIGVALUE", TableName = "PH_ASSEMBLYCLASSPROPERTY", ColumnName = "AP_CONFIGVALUE", NeedUpdate = true)]
    private string _configValue;
    /// <summary>
    /// 配置的值
    /// </summary>
    [System.ComponentModel.DisplayName("配置的值")]
    public string ConfigValue
    {
      get { return GetProperty(ConfigValueProperty, _configValue); }
      set { SetProperty(ConfigValueProperty, ref _configValue, value); }
    }
  }
}
