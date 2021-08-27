using System;

namespace Phenix.Security.Business
{
  /// <summary>
  /// 类信息
  /// </summary>
  [Serializable]
  [Phenix.Core.Mapping.ReadOnly]
  public class AssemblyClassReadOnly : AssemblyClass<AssemblyClassReadOnly>
  {
  }

  /// <summary>
  /// 类信息清单
  /// </summary>
  [Serializable]
  public class AssemblyClassReadOnlyList : Phenix.Business.BusinessListBase<AssemblyClassReadOnlyList, AssemblyClassReadOnly>
  {
  }

  /// <summary>
  /// 类信息
  /// </summary>
  [Serializable]
  public class AssemblyClass : AssemblyClass<AssemblyClass>
  {
    #region 属性

    /// <summary>
    /// 类属性
    /// </summary>
    public AssemblyClassPropertyList AssemblyClassProperties
    {
      get { return GetCompositionDetail<AssemblyClassPropertyList, AssemblyClassProperty>(); }
    }

    /// <summary>
    /// 类方法
    /// </summary>
    public AssemblyClassMethodList AssemblyClassMethods
    {
      get { return GetCompositionDetail<AssemblyClassMethodList, AssemblyClassMethod>(); }
    }

    #region 勾选角色全集

    /// <summary>
    /// 分配的角色
    /// </summary>
    public AssemblyClassRoleList AssemblyClassRoles
    {
      get { return GetCompositionDetail<AssemblyClassRoleList, AssemblyClassRole>(AssemblyClassRoleList.Fetch()); }
    }

    /// <summary>
    /// 供勾选的角色全集
    /// </summary>
    public SelectableRoleForAssemblyClassList SelectableRoles
    {
      get 
      {
        if (!(Authorised ?? true))
          return new SelectableRoleForAssemblyClassList();
        return AssemblyClassRoles.CollatingSelectableList<SelectableRoleForAssemblyClassList, SelectableRoleForAssemblyClass>(
          SelectableRoleForAssemblyClassList.Fetch(), !Phenix.Core.Security.UserIdentity.CurrentIdentity.EmptyRolesIsDeny);
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
        return Owner.EditMode && (Phenix.Core.Security.UserIdentity.CurrentIdentity.EmptyRolesIsDeny || AssemblyClassRoles.Count > 0)
          ? Phenix.Core.Data.BooleanOption.Y
          : Phenix.Core.Data.BooleanOption.N;
      }
    }

    #endregion

    #endregion
  }

  /// <summary>
  /// 类信息清单
  /// </summary>
  [Serializable]
  public class AssemblyClassList : Phenix.Business.BusinessListBase<AssemblyClassList, AssemblyClass>
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
  /// 类信息
  /// </summary>
  [Phenix.Core.Mapping.ClassAttribute("PH_ASSEMBLYCLASS", FriendlyName = "类信息"), System.SerializableAttribute(), System.ComponentModel.DisplayNameAttribute("类信息")]
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
      //internal set
      //{
      //  SetProperty(AC_IDProperty, ref _AC_ID, value);
      //}
    }

    [System.ComponentModel.Browsable(false)]
    [System.ComponentModel.DataAnnotations.Display(AutoGenerateField = false)]
    public override string PrimaryKey
    {
      get { return AC_ID.ToString(); }
    }

    /// <summary>
    /// 所属程序集
    /// </summary>
    public static readonly Phenix.Business.PropertyInfo<long?> AC_AS_IDProperty = RegisterProperty<long?>(c => c.AC_AS_ID);
    [Phenix.Core.Mapping.Field(FriendlyName = "所属程序集", TableName = "PH_ASSEMBLYCLASS", ColumnName = "AC_AS_ID", NeedUpdate = true)]
    [Phenix.Core.Mapping.FieldLink("PH_ASSEMBLY", "AS_ID")]
    private long? _AC_AS_ID;
    /// <summary>
    /// 所属程序集
    /// </summary>
    [System.ComponentModel.DisplayName("所属程序集")]
    public long? AC_AS_ID
    {
      get { return GetProperty(AC_AS_IDProperty, _AC_AS_ID); }
      //set { SetProperty(AC_AS_IDProperty, ref _AC_AS_ID, value); }
    }

    /// <summary>
    /// 名称
    /// </summary>
    public static readonly Phenix.Business.PropertyInfo<string> NameProperty = RegisterProperty<string>(c => c.Name);
    [Phenix.Core.Mapping.Field(FriendlyName = "名称", Alias = "AC_NAME", TableName = "PH_ASSEMBLYCLASS", ColumnName = "AC_NAME", NeedUpdate = true, IsNameColumn = true, InLookUpColumn = true, InLookUpColumnDisplay = true)]
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
    [Phenix.Core.Mapping.Field(FriendlyName = "标签", Alias = "AC_CAPTION", TableName = "PH_ASSEMBLYCLASS", ColumnName = "AC_CAPTION", NeedUpdate = true)]
    private string _caption;
    /// <summary>
    /// 标签
    /// </summary>
    [System.ComponentModel.DisplayName("标签")]
    public new string Caption
    {
      get { return GetProperty(CaptionProperty, _caption); }
      set
      {
        SetProperty(CaptionProperty, ref _caption, value);
        CaptionConfigured = true;
      }
    }

    /// <summary>
    /// 标签已被配置
    /// </summary>
    public static readonly Phenix.Business.PropertyInfo<bool?> CaptionConfiguredProperty = RegisterProperty<bool?>(c => c.CaptionConfigured);
    [Phenix.Core.Mapping.Field(FriendlyName = "标签已被配置", Alias = "AC_CAPTIONCONFIGURED", TableName = "PH_ASSEMBLYCLASS", ColumnName = "AC_CAPTIONCONFIGURED", NeedUpdate = true)]
    private bool? _captionConfigured;
    /// <summary>
    /// 标签已被配置
    /// </summary>
    [System.ComponentModel.DisplayName("标签已被配置")]
    public bool? CaptionConfigured
    {
      get { return GetProperty(CaptionConfiguredProperty, _captionConfigured); }
      private set { SetProperty(CaptionConfiguredProperty, ref _captionConfigured, value); }
    }
    
    /// <summary>
    /// 指示当处于哪种执行动作时本字段需要记录新旧值
    /// </summary>
    public static readonly Phenix.Business.PropertyInfo<Phenix.Core.Mapping.ExecuteAction?> PermanentExecuteActionProperty = RegisterProperty<Phenix.Core.Mapping.ExecuteAction?>(c => c.PermanentExecuteAction);
    [Phenix.Core.Mapping.Field(FriendlyName = "指示当处于哪种执行动作时本字段需要记录新旧值", Alias = "AC_PERMANENTEXECUTEACTION", TableName = "PH_ASSEMBLYCLASS", ColumnName = "AC_PERMANENTEXECUTEACTION", NeedUpdate = true)]
    private Phenix.Core.Mapping.ExecuteAction? _permanentExecuteAction;
    /// <summary>
    /// 指示当处于哪种执行动作时本字段需要记录新旧值
    /// </summary>
    [System.ComponentModel.DisplayName("指示当处于哪种执行动作时本字段需要记录新旧值")]
    public Phenix.Core.Mapping.ExecuteAction? PermanentExecuteAction
    {
      get { return GetProperty(PermanentExecuteActionProperty, _permanentExecuteAction); }
      set 
      { 
        SetProperty(PermanentExecuteActionProperty, ref _permanentExecuteAction, value);
        PermanentExecuteConfigured = true;
      }
    }

    /// <summary>
    /// 持久化执行变更方式已被配置
    /// </summary>
    public static readonly Phenix.Business.PropertyInfo<bool?> PermanentExecuteConfiguredProperty = RegisterProperty<bool?>(c => c.PermanentExecuteConfigured);
    [Phenix.Core.Mapping.Field(FriendlyName = "持久化执行变更方式已被配置", Alias = "AC_PERMANENTEXECUTECONFIGURED", TableName = "PH_ASSEMBLYCLASS", ColumnName = "AC_PERMANENTEXECUTECONFIGURED", NeedUpdate = true)]
    private bool? _permanentExecuteConfigured;
    /// <summary>
    /// 持久化执行变更方式已被配置
    /// </summary>
    [System.ComponentModel.DisplayName("持久化执行变更方式已被配置")]
    public bool? PermanentExecuteConfigured
    {
      get { return GetProperty(PermanentExecuteConfiguredProperty, _permanentExecuteConfigured); }
      private set { SetProperty(PermanentExecuteConfiguredProperty, ref _permanentExecuteConfigured, value); }
    }

    /// <summary>
    /// 类型
    /// </summary>
    public static readonly Phenix.Business.PropertyInfo<Phenix.Core.Dictionary.AssemblyClassType?> TypeProperty = RegisterProperty<Phenix.Core.Dictionary.AssemblyClassType?>(c => c.Type);
    [Phenix.Core.Mapping.Field(FriendlyName = "类型", Alias = "AC_TYPE", TableName = "PH_ASSEMBLYCLASS", ColumnName = "AC_TYPE", NeedUpdate = true)]
    private Phenix.Core.Dictionary.AssemblyClassType? _type;
    /// <summary>
    /// 类型
    /// </summary>
    [System.ComponentModel.DisplayName("类型")]
    public Phenix.Core.Dictionary.AssemblyClassType? Type
    {
      get { return GetProperty(TypeProperty, _type); }
      //set { SetProperty(TypeProperty, ref _type, value); }
    }

    /// <summary>
    /// 可被授权
    /// </summary>
    public static readonly Phenix.Business.PropertyInfo<bool?> AuthorisedProperty = RegisterProperty<bool?>(c => c.Authorised);
    [Phenix.Core.Mapping.Field(FriendlyName = "可被授权", Alias = "AC_AUTHORISED", TableName = "PH_ASSEMBLYCLASS", ColumnName = "AC_AUTHORISED", NeedUpdate = true)]
    private bool? _authorised;
    /// <summary>
    /// 可被授权
    /// </summary>
    [System.ComponentModel.DisplayName("可被授权")]
    public bool? Authorised
    {
      get { return GetProperty(AuthorisedProperty, _authorised); }
      set { SetProperty(AuthorisedProperty, ref _authorised, value); }
    }
  }
}
