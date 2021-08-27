using System;

namespace Phenix.Test.使用指南._11._5._2._2
{
  /// <summary>
  /// UserRoleInfo
  /// </summary>
  [Serializable]
  public class UserRoleInfo : UserRoleInfo<UserRoleInfo>
  {
  }

  /// <summary>
  /// UserRoleInfo清单
  /// </summary>
  [Serializable]
  public class UserRoleInfoList : Phenix.Business.BusinessListBase<UserRoleInfoList, UserRoleInfo>
  {
  }

  /// <summary>
  /// UserRoleInfo
  /// </summary>
  //* 指定本类操作的表是"PH_USER_ROLE"
  [Phenix.Core.Mapping.ClassAttribute("PH_USER_ROLE", FetchScript = "PH_USERROLEINFO_V", FriendlyName = "UserRoleInfo"), System.ComponentModel.DisplayNameAttribute("UserRoleInfo"), System.SerializableAttribute()]
  public abstract class UserRoleInfo<T> : Phenix.Business.BusinessBase<T> where T : UserRoleInfo<T>
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

    /// <summary>
    /// US_ID
    /// </summary>
    public static readonly Phenix.Business.PropertyInfo<long?> US_IDProperty = RegisterProperty<long?>(c => c.US_ID);
    [Phenix.Core.Mapping.Field(FriendlyName = "US_ID", TableName = "PH_USER", ColumnName = "US_ID", IsPrimaryKey = true)]
    private long? _US_ID;
    /// <summary>
    /// US_ID
    /// </summary>
    [System.ComponentModel.DisplayName("US_ID")]
    public long? US_ID
    {
      get { return GetProperty(US_IDProperty, _US_ID); }
      internal set
      {
        SetProperty(US_IDProperty, ref _US_ID, value);
      }
    }

    /// <summary>
    /// RL_ID
    /// </summary>
    public static readonly Phenix.Business.PropertyInfo<long?> RL_IDProperty = RegisterProperty<long?>(c => c.RL_ID);
    [Phenix.Core.Mapping.Field(FriendlyName = "RL_ID", TableName = "PH_ROLE", ColumnName = "RL_ID", IsPrimaryKey = true)]
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

    /// <summary>
    /// DP_ID
    /// </summary>
    public static readonly Phenix.Business.PropertyInfo<long?> DP_IDProperty = RegisterProperty<long?>(c => c.DP_ID);
    [Phenix.Core.Mapping.Field(FriendlyName = "DP_ID", TableName = "PH_DEPARTMENT", ColumnName = "DP_ID", IsPrimaryKey = true)]
    private long? _DP_ID;
    /// <summary>
    /// DP_ID
    /// </summary>
    [System.ComponentModel.DisplayName("DP_ID")]
    public long? DP_ID
    {
      get { return GetProperty(DP_IDProperty, _DP_ID); }
      internal set
      {
        SetProperty(DP_IDProperty, ref _DP_ID, value);
      }
    }

    [System.ComponentModel.Browsable(false)]
    [System.ComponentModel.DataAnnotations.Display(AutoGenerateField = false)]
    public override string PrimaryKey
    {
      get { return String.Format("{0},{1},{2},{3}", UR_ID, US_ID, RL_ID, DP_ID); }
    }

    /// <summary>
    /// UR_US_ID
    /// </summary>
    public static readonly Phenix.Business.PropertyInfo<long?> UR_US_IDProperty = RegisterProperty<long?>(c => c.UR_US_ID);
    [Phenix.Core.Mapping.Field(FriendlyName = "UR_US_ID", TableName = "PH_USER_ROLE", ColumnName = "UR_US_ID", NeedUpdate = true)]
    private long? _UR_US_ID;
    /// <summary>
    /// UR_US_ID
    /// </summary>
    [System.ComponentModel.DisplayName("UR_US_ID")]
    public long? UR_US_ID
    {
      get { return GetProperty(UR_US_IDProperty, _UR_US_ID); }
      set { SetProperty(UR_US_IDProperty, ref _UR_US_ID, value); }
    }

    /// <summary>
    /// UR_RL_ID
    /// </summary>
    public static readonly Phenix.Business.PropertyInfo<long?> UR_RL_IDProperty = RegisterProperty<long?>(c => c.UR_RL_ID);
    [Phenix.Core.Mapping.Field(FriendlyName = "UR_RL_ID", TableName = "PH_USER_ROLE", ColumnName = "UR_RL_ID", NeedUpdate = true)]
    private long? _UR_RL_ID;
    /// <summary>
    /// UR_RL_ID
    /// </summary>
    [System.ComponentModel.DisplayName("UR_RL_ID")]
    public long? UR_RL_ID
    {
      get { return GetProperty(UR_RL_IDProperty, _UR_RL_ID); }
      set { SetProperty(UR_RL_IDProperty, ref _UR_RL_ID, value); }
    }

    /// <summary>
    /// US_NAME
    /// </summary>
    public static readonly Phenix.Business.PropertyInfo<string> UserNameProperty = RegisterProperty<string>(c => c.UserName);
    [Phenix.Core.Mapping.Field(FriendlyName = "US_NAME", Alias = "US_NAME", TableName = "PH_USER", ColumnName = "US_NAME", IsNameColumn = true, InLookUpColumn = true, InLookUpColumnDisplay = true)]
    private string _userName;
    /// <summary>
    /// US_NAME
    /// </summary>
    [System.ComponentModel.DisplayName("US_NAME")]
    public string UserName
    {
      get { return GetProperty(UserNameProperty, _userName); }
      set { SetProperty(UserNameProperty, ref _userName, value); }
    }

    /// <summary>
    /// US_USERNUMBER
    /// </summary>
    public static readonly Phenix.Business.PropertyInfo<string> UsernumberProperty = RegisterProperty<string>(c => c.Usernumber);
    [Phenix.Core.Mapping.Field(FriendlyName = "US_USERNUMBER", Alias = "US_USERNUMBER", TableName = "PH_USER", ColumnName = "US_USERNUMBER")]
    private string _usernumber;
    /// <summary>
    /// US_USERNUMBER
    /// </summary>
    [System.ComponentModel.DisplayName("US_USERNUMBER")]
    public string Usernumber
    {
      get { return GetProperty(UsernumberProperty, _usernumber); }
      set { SetProperty(UsernumberProperty, ref _usernumber, value); }
    }

    /// <summary>
    /// RL_NAME
    /// </summary>
    public static readonly Phenix.Business.PropertyInfo<string> RoleNameProperty = RegisterProperty<string>(c => c.RoleName);
    [Phenix.Core.Mapping.Field(FriendlyName = "RL_NAME", Alias = "RL_NAME", TableName = "PH_ROLE", ColumnName = "RL_NAME")]
    private string _roleName;
    /// <summary>
    /// RL_NAME
    /// </summary>
    [System.ComponentModel.DisplayName("RL_NAME")]
    public string RoleName
    {
      get { return GetProperty(RoleNameProperty, _roleName); }
      set { SetProperty(RoleNameProperty, ref _roleName, value); }
    }

    /// <summary>
    /// DP_NAME
    /// </summary>
    public static readonly Phenix.Business.PropertyInfo<string> DepartmentNameProperty = RegisterProperty<string>(c => c.DepartmentName);
    [Phenix.Core.Mapping.Field(FriendlyName = "DP_NAME", Alias = "DP_NAME", TableName = "PH_DEPARTMENT", ColumnName = "DP_NAME")]
    private string _departmentName;
    /// <summary>
    /// DP_NAME
    /// </summary>
    [System.ComponentModel.DisplayName("DP_NAME")]
    public string DepartmentName
    {
      get { return GetProperty(DepartmentNameProperty, _departmentName); }
      set { SetProperty(DepartmentNameProperty, ref _departmentName, value); }
    }

    /// <summary>
    /// New
    /// </summary>
    public static T New(long? UR_ID, long? US_ID, long? RL_ID, long? DP_ID, long? UR_US_ID, long? UR_RL_ID, string userName, string usernumber, string roleName, string departmentName)
    {
      T result = NewPure();
      result._UR_ID = UR_ID;
      result._US_ID = US_ID;
      result._RL_ID = RL_ID;
      result._DP_ID = DP_ID;
      result._UR_US_ID = UR_US_ID;
      result._UR_RL_ID = UR_RL_ID;
      result._userName = userName;
      result._usernumber = usernumber;
      result._roleName = roleName;
      result._departmentName = departmentName;
      return result;
    }

    /// <summary>
    /// SetFieldValues
    /// </summary>
    protected void SetFieldValues(long? UR_ID, long? US_ID, long? RL_ID, long? DP_ID, long? UR_US_ID, long? UR_RL_ID, string userName, string usernumber, string roleName, string departmentName)
    {
      InitOldFieldValues();
      _UR_ID = UR_ID;
      _US_ID = US_ID;
      _RL_ID = RL_ID;
      _DP_ID = DP_ID;
      _UR_US_ID = UR_US_ID;
      _UR_RL_ID = UR_RL_ID;
      _userName = userName;
      _usernumber = usernumber;
      _roleName = roleName;
      _departmentName = departmentName;
      MarkDirty();
    }
  }
}
