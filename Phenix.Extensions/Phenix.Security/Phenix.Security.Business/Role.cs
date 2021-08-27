using System;

namespace Phenix.Security.Business
{
  /// <summary>
  /// 角色
  /// </summary>
  [Serializable]
  [Phenix.Core.Mapping.ReadOnly]
  public class RoleReadOnly : Role<RoleReadOnly>
  {
  }

  /// <summary>
  /// 角色清单
  /// </summary>
  [Serializable]
  public class RoleReadOnlyList : Phenix.Business.BusinessListBase<RoleReadOnlyList, RoleReadOnly>
  {
  }

  /// <summary>
  /// 角色
  /// </summary>
  [Serializable]
  public class Role : Role<Role>
  {
    #region 属性

    /// <summary>
    /// 是否允许设置数据
    /// 只读则为false
    /// </summary>
    [System.ComponentModel.Browsable(false)]
    [System.ComponentModel.DataAnnotations.Display(AutoGenerateField = false)]
    public override bool AllowSet
    {
      get
      {
        if (!base.AllowSet)
          return false;
        return String.Compare(this.Name, Phenix.Core.Security.UserIdentity.AdminRoleName, false) != 0;
      }
    }

    #region 勾选用户全集

    /// <summary>
    /// 分配的用户
    /// </summary>
    public UserRoleList UserRoles
    {
      get { return GetCompositionDetail<UserRoleList, UserRole>(UserRoleList.Fetch()); }
    }

    /// <summary>
    /// 供勾选的用户全集
    /// </summary>
    public UserReadOnlyList SelectableUsers
    {
      get { return UserRoles.CollatingSelectableList<UserReadOnlyList, UserReadOnly>(UserReadOnlyList.Fetch()); }
    }

    #endregion

    #endregion
  }

  /// <summary>
  /// 角色清单
  /// </summary>
  [Serializable]
  public class RoleList : Phenix.Business.BusinessListBase<RoleList, Role>
  {
  }

  /// <summary>
  /// 角色
  /// </summary>
  [Phenix.Core.Mapping.ClassAttribute("PH_ROLE", FriendlyName = "角色"), System.SerializableAttribute(), System.ComponentModel.DisplayNameAttribute("角色")]
  [Phenix.Core.Mapping.ClassDetail("PH_USER_ROLE", "UR_RL_ID", null, CascadingDelete = true, FriendlyName = "用户角色")]
  [Phenix.Core.Mapping.ClassDetail("PH_USER_GRANT_ROLE", "GR_RL_ID", null, CascadingDelete = true, FriendlyName = "用户可授权角色")]
  [Phenix.Core.Mapping.ClassDetail("PH_ASSEMBLYCLASS_ROLE", "AR_RL_ID", null, CascadingDelete = true, FriendlyName = "类-角色")]
  [Phenix.Core.Mapping.ClassDetail("PH_ASSEMBLYCLASSPROPERTY_ROLE", "AR_RL_ID", null, CascadingDelete = true, FriendlyName = "类属性-角色")]
  [Phenix.Core.Mapping.ClassDetail("PH_ASSEMBLYCLASSMETHOD_ROLE", "AR_RL_ID", null, CascadingDelete = true, FriendlyName = "类方法-角色")]
  public abstract class Role<T> : Phenix.Business.BusinessBase<T> where T : Role<T>
  {
    /// <summary>
    /// RL_ID
    /// </summary>
    public static readonly Phenix.Business.PropertyInfo<long?> RL_IDProperty = RegisterProperty<long?>(c => c.RL_ID);
    [Phenix.Core.Mapping.Field(FriendlyName = "RL_ID", TableName = "PH_ROLE", ColumnName = "RL_ID", IsPrimaryKey = true, NeedUpdate = true)]
    private long? _RL_ID;
    /// <summary>
    /// RL_ID
    /// </summary>
    [System.ComponentModel.DisplayName("RL_ID")]
    public long? RL_ID
    {
      get { return GetProperty(RL_IDProperty, _RL_ID); }
      internal set
      {
        SetProperty(RL_IDProperty, ref _RL_ID, value);
      }
    }

    [System.ComponentModel.Browsable(false)]
    [System.ComponentModel.DataAnnotations.Display(AutoGenerateField = false)]
    public override string PrimaryKey
    {
      get { return RL_ID.ToString(); }
    }

    /// <summary>
    /// 名称
    /// </summary>
    public static readonly Phenix.Business.PropertyInfo<string> NameProperty = RegisterProperty<string>(c => c.Name);
    [Phenix.Core.Mapping.Field(FriendlyName = "名称", Alias = "RL_NAME", TableName = "PH_ROLE", ColumnName = "RL_NAME", NeedUpdate = true, IsNameColumn = true, InLookUpColumn = true, InLookUpColumnDisplay = true)]
    private string _name;
    /// <summary>
    /// 名称
    /// </summary>
    [System.ComponentModel.DisplayName("名称")]
    public string Name
    {
      get { return GetProperty(NameProperty, _name); }
      set { SetProperty(NameProperty, ref _name, value); }
    }

    /// <summary>
    /// 标签
    /// </summary>
    public static readonly Phenix.Business.PropertyInfo<string> CaptionProperty = RegisterProperty<string>(c => c.Caption);
    [Phenix.Core.Mapping.Field(FriendlyName = "标签", Alias = "RL_CAPTION", TableName = "PH_ROLE", ColumnName = "RL_CAPTION", NeedUpdate = true)]
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
  }
}
