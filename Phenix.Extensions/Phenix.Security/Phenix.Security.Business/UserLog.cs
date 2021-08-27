using System;

namespace Phenix.Security.Business
{
  /// <summary>
  /// 用户日志
  /// </summary>
  [Serializable]
  [Phenix.Core.Mapping.ReadOnly]
  public class UserLogReadOnly : UserLog<UserLogReadOnly>
  {
  }

  /// <summary>
  /// 用户日志清单
  /// </summary>
  [Serializable]
  public class UserLogReadOnlyList : Phenix.Business.BusinessListBase<UserLogReadOnlyList, UserLogReadOnly>
  {
  }

  /// <summary>
  /// 用户日志
  /// </summary>
  [Serializable]
  public class UserLog : UserLog<UserLog>
  {
    /// <summary>
    /// 是否允许编辑数据
    /// </summary>
    [System.ComponentModel.Browsable(false)]
    [System.ComponentModel.DataAnnotations.Display(AutoGenerateField = false)]
    public override bool AllowEdit
    {
      get { return false; }
    }
    
    #region 方法

    /// <summary>
    /// 删除所在集合中被选择的日志
    /// 如果一个也未被选择则删除自己
    /// </summary>
    public void DeleteOwnerSelectedOrSelf()
    {
      if (Root.EditMode)
        throw new InvalidOperationException("Root.EditMode = true下不允许操作");
      UserLogList owner = Owner as UserLogList;
      if (owner == null)
        throw new InvalidOperationException("Owner非UserLogList下不允许操作");

      if (owner.Remove(item => item.Selected) == 0)
        Delete();
      Root.Save();
    }

    #endregion
  }

  /// <summary>
  /// 用户日志清单
  /// </summary>
  [Serializable]
  public class UserLogList : Phenix.Business.BusinessListBase<UserLogList, UserLog>
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

    /// <summary>
    /// 是否允许编辑业务对象
    /// </summary>
    [System.ComponentModel.Browsable(false)]
    [System.ComponentModel.DataAnnotations.Display(AutoGenerateField = false)]
    public override bool AllowEditItem
    {
      get { return false; }
    }
  }

  /// <summary>
  /// 用户日志
  /// </summary>
  [Phenix.Core.Mapping.ClassAttribute("PH_USERLOG", FriendlyName = "用户日志"), System.SerializableAttribute(), System.ComponentModel.DisplayNameAttribute("用户日志")]
  public abstract class UserLog<T> : Phenix.Business.BusinessBase<T> where T : UserLog<T>
  {
    [System.ComponentModel.Browsable(false)]
    [System.ComponentModel.DataAnnotations.Display(AutoGenerateField = false)]
    public override string PrimaryKey
    {
      get { return base.PrimaryKey; }
    }

    /// <summary>
    /// US_ID
    /// </summary>
    public static readonly Phenix.Business.PropertyInfo<long?> US_IDProperty = RegisterProperty<long?>(c => c.US_ID);
    [Phenix.Core.Mapping.Field(FriendlyName = "US_ID", TableName = "PH_USERLOG", ColumnName = "US_ID", NeedUpdate = true)]
    [Phenix.Core.Mapping.FieldLink("PH_USER", "US_ID")]
    private long? _US_ID;
    /// <summary>
    /// US_ID
    /// </summary>
    [System.ComponentModel.DisplayName("US_ID")]
    public long? US_ID
    {
      get { return GetProperty(US_IDProperty, _US_ID); }
      set { SetProperty(US_IDProperty, ref _US_ID, value); }
    }

    /// <summary>
    /// 工号
    /// </summary>
    public static readonly Phenix.Business.PropertyInfo<string> UserNumberProperty = RegisterProperty<string>(c => c.UserNumber);
    [Phenix.Core.Mapping.Field(FriendlyName = "工号", Alias = "US_USERNUMBER", TableName = "PH_USERLOG", ColumnName = "US_USERNUMBER", NeedUpdate = true)]
    [Phenix.Core.Mapping.FieldUnique("UserLog")]
    private string _userNumber;
    /// <summary>
    /// 工号
    /// </summary>
    [System.ComponentModel.DisplayName("工号")]
    public string UserNumber
    {
      get { return GetProperty(UserNumberProperty, _userNumber); }
      set { SetProperty(UserNumberProperty, ref _userNumber, value); }
    }

    /// <summary>
    /// 登录时间
    /// </summary>
    public static readonly Phenix.Business.PropertyInfo<DateTime?> LoginProperty = RegisterProperty<DateTime?>(c => c.Login);
    [Phenix.Core.Mapping.Field(FriendlyName = "登录时间", Alias = "US_LOGIN", TableName = "PH_USERLOG", ColumnName = "US_LOGIN", NeedUpdate = true)]
    [Phenix.Core.Mapping.FieldUnique("UserLog")]
    private DateTime? _login;
    /// <summary>
    /// 登录时间
    /// </summary>
    [System.ComponentModel.DisplayName("登录时间")]
    public DateTime? Login
    {
      get { return GetProperty(LoginProperty, _login); }
      set { SetProperty(LoginProperty, ref _login, value); }
    }

    /// <summary>
    /// 登出时间
    /// </summary>
    public static readonly Phenix.Business.PropertyInfo<DateTime?> LogoutProperty = RegisterProperty<DateTime?>(c => c.Logout);
    [Phenix.Core.Mapping.Field(FriendlyName = "登出时间", Alias = "US_LOGOUT", TableName = "PH_USERLOG", ColumnName = "US_LOGOUT", NeedUpdate = true)]
    [Phenix.Core.Mapping.FieldUnique("UserLog")]
    private DateTime? _logout;
    /// <summary>
    /// 登出时间
    /// </summary>
    [System.ComponentModel.DisplayName("登出时间")]
    public DateTime? Logout
    {
      get { return GetProperty(LogoutProperty, _logout); }
      set { SetProperty(LogoutProperty, ref _logout, value); }
    }

    /// <summary>
    /// 登录地址
    /// </summary>
    public static readonly Phenix.Business.PropertyInfo<string> LoginAddressProperty = RegisterProperty<string>(c => c.LoginAddress);
    [Phenix.Core.Mapping.Field(FriendlyName = "登录地址", Alias = "US_LOGINADDRESS", TableName = "PH_USERLOG", ColumnName = "US_LOGINADDRESS", NeedUpdate = true)]
    [Phenix.Core.Mapping.FieldUnique("UserLog")]
    private string _loginAddress;
    /// <summary>
    /// 登录地址
    /// </summary>
    [System.ComponentModel.DisplayName("登录地址")]
    public string LoginAddress
    {
      get { return GetProperty(LoginAddressProperty, _loginAddress); }
      set { SetProperty(LoginAddressProperty, ref _loginAddress, value); }
    }
  }
}
