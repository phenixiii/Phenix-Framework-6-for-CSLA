using System;

namespace Phenix.Security.Business
{
  /// <summary>
  /// 用户可授权角色
  /// </summary>
  [Serializable]
  [Phenix.Core.Mapping.ReadOnly]
  public class UserGrantRoleReadOnly : UserGrantRole<UserGrantRoleReadOnly>
  {
  }

  /// <summary>
  /// 用户可授权角色清单
  /// </summary>
  [Serializable]
  public class UserGrantRoleReadOnlyList : Phenix.Business.BusinessListBase<UserGrantRoleReadOnlyList, UserGrantRoleReadOnly>
  {
  }

  /// <summary>
  /// 用户可授权角色
  /// </summary>
  [Serializable]
  public class UserGrantRole : UserGrantRole<UserGrantRole>
  {
  }

  /// <summary>
  /// 用户可授权角色清单
  /// </summary>
  [Serializable]
  public class UserGrantRoleList : Phenix.Business.BusinessListBase<UserGrantRoleList, UserGrantRole>
  {
  }

  /// <summary>
  /// 用户可授权角色
  /// </summary>
  [Phenix.Core.Mapping.ClassAttribute("PH_USER_GRANT_ROLE", FriendlyName = "用户可授权角色"), System.SerializableAttribute(), System.ComponentModel.DisplayNameAttribute("用户可授权角色")]
  public abstract class UserGrantRole<T> : Phenix.Business.BusinessBase<T> where T : UserGrantRole<T>
  {
    /// <summary>
    /// GR_ID
    /// </summary>
    public static readonly Phenix.Business.PropertyInfo<long?> GR_IDProperty = RegisterProperty<long?>(c => c.GR_ID);
    [Phenix.Core.Mapping.Field(FriendlyName = "GR_ID", TableName = "PH_USER_GRANT_ROLE", ColumnName = "GR_ID", IsPrimaryKey = true, NeedUpdate = true)]
    private long? _GR_ID;
    /// <summary>
    /// GR_ID
    /// </summary>
    [System.ComponentModel.DisplayName("GR_ID")]
    public long? GR_ID
    {
      get { return GetProperty(GR_IDProperty, _GR_ID); }
      internal set
      {
        SetProperty(GR_IDProperty, ref _GR_ID, value);
      }
    }

    [System.ComponentModel.Browsable(false)]
    [System.ComponentModel.DataAnnotations.Display(AutoGenerateField = false)]
    public override string PrimaryKey
    {
      get { return GR_ID.ToString(); }
    }

    /// <summary>
    /// 用户
    /// </summary>
    public static readonly Phenix.Business.PropertyInfo<long?> GR_US_IDProperty = RegisterProperty<long?>(c => c.GR_US_ID);
    [Phenix.Core.Mapping.Field(FriendlyName = "用户", TableName = "PH_USER_GRANT_ROLE", ColumnName = "GR_US_ID", NeedUpdate = true)]
    [Phenix.Core.Mapping.FieldLink("PH_USER", "US_ID")]
    private long? _GR_US_ID;
    /// <summary>
    /// 用户
    /// </summary>
    [System.ComponentModel.DisplayName("用户")]
    public long? GR_US_ID
    {
      get { return GetProperty(GR_US_IDProperty, _GR_US_ID); }
      set { SetProperty(GR_US_IDProperty, ref _GR_US_ID, value); }
    }

    /// <summary>
    /// 角色
    /// </summary>
    public static readonly Phenix.Business.PropertyInfo<long?> GR_RL_IDProperty = RegisterProperty<long?>(c => c.GR_RL_ID);
    [Phenix.Core.Mapping.Field(FriendlyName = "角色", TableName = "PH_USER_GRANT_ROLE", ColumnName = "GR_RL_ID", NeedUpdate = true)]
    [Phenix.Core.Mapping.FieldLink("PH_ROLE", "RL_ID")]
    private long? _GR_RL_ID;
    /// <summary>
    /// 角色
    /// </summary>
    [System.ComponentModel.DisplayName("角色")]
    public long? GR_RL_ID
    {
      get { return GetProperty(GR_RL_IDProperty, _GR_RL_ID); }
      set { SetProperty(GR_RL_IDProperty, ref _GR_RL_ID, value); }
    }

    /// <summary>
    /// 录入人
    /// </summary>
    public static readonly Phenix.Business.PropertyInfo<string> InputerProperty = RegisterProperty<string>(c => c.Inputer);
    [Phenix.Core.Mapping.Field(FriendlyName = "录入人", Alias = "GR_INPUTER", TableName = "PH_USER_GRANT_ROLE", ColumnName = "GR_INPUTER", NeedUpdate = true, OverwritingOnUpdate = true, IsInputerColumn = true)]
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
    [Phenix.Core.Mapping.Field(FriendlyName = "录入时间", Alias = "GR_INPUTTIME", TableName = "PH_USER_GRANT_ROLE", ColumnName = "GR_INPUTTIME", NeedUpdate = true, OverwritingOnUpdate = true, IsInputTimeColumn = true)]
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
