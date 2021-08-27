using System;

namespace Phenix.Security.Business
{
  /// <summary>
  /// 用户角色
  /// </summary>
  [Serializable]
  [Phenix.Core.Mapping.ReadOnly]
  public class UserRoleReadOnly : UserRole<UserRoleReadOnly>
  {
  }

  /// <summary>
  /// 用户角色清单
  /// </summary>
  [Serializable]
  public class UserRoleReadOnlyList : Phenix.Business.BusinessListBase<UserRoleReadOnlyList, UserRoleReadOnly>
  {
  }

  /// <summary>
  /// 用户角色
  /// </summary>
  [Serializable]
  public class UserRole : UserRole<UserRole>
  {
  }

  /// <summary>
  /// 用户角色清单
  /// </summary>
  [Serializable]
  public class UserRoleList : Phenix.Business.BusinessListBase<UserRoleList, UserRole>
  {
  }

  /// <summary>
  /// 用户角色
  /// </summary>
  [Phenix.Core.Mapping.ClassAttribute("PH_USER_ROLE", FriendlyName = "用户角色"), System.SerializableAttribute(), System.ComponentModel.DisplayNameAttribute("用户角色")]
  public abstract class UserRole<T> : Phenix.Business.BusinessBase<T> where T : UserRole<T>
  {
    /// <summary>
    /// UR_ID
    /// </summary>
    public static readonly Phenix.Business.PropertyInfo<long?> UR_IDProperty = RegisterProperty<long?>(c => c.UR_ID);
    [Phenix.Core.Mapping.Field(FriendlyName = "UR_ID", TableName = "PH_USER_ROLE", ColumnName = "UR_ID", IsPrimaryKey = true, NeedUpdate = true)]
    private long? _UR_ID;
    /// <summary>
    /// UR_ID
    /// </summary>
    [System.ComponentModel.DisplayName("UR_ID")]
    public long? UR_ID
    {
      get { return GetProperty(UR_IDProperty, _UR_ID); }
      internal set
      {
        SetProperty(UR_IDProperty, ref _UR_ID, value);
      }
    }

    [System.ComponentModel.Browsable(false)]
    [System.ComponentModel.DataAnnotations.Display(AutoGenerateField = false)]
    public override string PrimaryKey
    {
      get { return UR_ID.ToString(); }
    }

    /// <summary>
    /// 用户
    /// </summary>
    public static readonly Phenix.Business.PropertyInfo<long?> UR_US_IDProperty = RegisterProperty<long?>(c => c.UR_US_ID);
    [Phenix.Core.Mapping.Field(FriendlyName = "用户", TableName = "PH_USER_ROLE", ColumnName = "UR_US_ID", NeedUpdate = true)]
    [Phenix.Core.Mapping.FieldLink("PH_USER", "US_ID")]
    private long? _UR_US_ID;
    /// <summary>
    /// 用户
    /// </summary>
    [System.ComponentModel.DisplayName("用户")]
    public long? UR_US_ID
    {
      get { return GetProperty(UR_US_IDProperty, _UR_US_ID); }
      set { SetProperty(UR_US_IDProperty, ref _UR_US_ID, value); }
    }

    /// <summary>
    /// 角色
    /// </summary>
    public static readonly Phenix.Business.PropertyInfo<long?> UR_RL_IDProperty = RegisterProperty<long?>(c => c.UR_RL_ID);
    [Phenix.Core.Mapping.Field(FriendlyName = "角色", TableName = "PH_USER_ROLE", ColumnName = "UR_RL_ID", NeedUpdate = true)]
    [Phenix.Core.Mapping.FieldLink("PH_ROLE", "RL_ID")]
    private long? _UR_RL_ID;
    /// <summary>
    /// 角色
    /// </summary>
    [System.ComponentModel.DisplayName("角色")]
    public long? UR_RL_ID
    {
      get { return GetProperty(UR_RL_IDProperty, _UR_RL_ID); }
      set { SetProperty(UR_RL_IDProperty, ref _UR_RL_ID, value); }
    }

    /// <summary>
    /// 录入人
    /// </summary>
    public static readonly Phenix.Business.PropertyInfo<string> InputerProperty = RegisterProperty<string>(c => c.Inputer);
    [Phenix.Core.Mapping.Field(FriendlyName = "录入人", Alias = "UR_INPUTER", TableName = "PH_USER_ROLE", ColumnName = "UR_INPUTER", NeedUpdate = true, OverwritingOnUpdate = true, IsInputerColumn = true)]
    private string _inputer;
    /// <summary>
    /// 录入人
    /// </summary>
    [System.ComponentModel.DisplayName("录入人")]
    public string Inputer
    {
      get { return GetProperty(InputerProperty, _inputer); }
    }

    /// <summary>
    /// 录入时间
    /// </summary>
    public static readonly Phenix.Business.PropertyInfo<DateTime?> InputTimeProperty = RegisterProperty<DateTime?>(c => c.InputTime);
    [Phenix.Core.Mapping.Field(FriendlyName = "录入时间", Alias = "UR_INPUTTIME", TableName = "PH_USER_ROLE", ColumnName = "UR_INPUTTIME", NeedUpdate = true, OverwritingOnUpdate = true, IsInputTimeColumn = true)]
    private DateTime? _inputTime;
    /// <summary>
    /// 录入时间
    /// </summary>
    [System.ComponentModel.DisplayName("录入时间")]
    public DateTime? InputTime
    {
      get { return GetProperty(InputTimeProperty, _inputTime); }
    }
  }
}
