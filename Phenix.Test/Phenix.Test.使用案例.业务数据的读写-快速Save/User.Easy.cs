using System;
using Phenix.Core.Data;
using Phenix.Core.Mapping;

/* 
   builder:    phenixiii
   build time: 2017-04-10 14:30:23
   notes:      User
*/

namespace Phenix.Test.使用案例.业务数据的读写.快速Save
{
  /// <summary>
  /// User
  /// </summary>
  [Serializable]
  [System.ComponentModel.DisplayName("User")]
  [Phenix.Core.Mapping.Class("PH_USER", FriendlyName = "User")]
  public class UserEasy : EntityBase<UserEasy>
  {
    private UserEasy()
    {
      //禁止添加代码
    }

    [Newtonsoft.Json.JsonConstructor]
    private UserEasy(bool? isNew, bool? isSelfDirty, bool? isSelfDeleted, long? US_ID, string usernumber, string password, DateTime? passwordchangedtime, string name, DateTime? login, DateTime? logout, DateTime? loginfailure, int? loginfailurecount, string loginaddress, long? US_DP_ID, long? US_PT_ID, int? locked)
      : base(isNew, isSelfDirty, isSelfDeleted)
    {
      if (US_ID != null)
      {
        _US_ID = US_ID;
        SetDirtyProperty(US_IDProperty);
      }
      if (usernumber != null)
      {
        _usernumber = usernumber;
        SetDirtyProperty(UsernumberProperty);
      }
      if (password != null)
      {
        _password = password;
        SetDirtyProperty(PasswordProperty);
      }
      if (passwordchangedtime != null)
      {
        _passwordchangedtime = passwordchangedtime;
        SetDirtyProperty(PasswordchangedtimeProperty);
      }
      if (name != null)
      {
        _name = name;
        SetDirtyProperty(NameProperty);
      }
      if (login != null)
      {
        _login = login;
        SetDirtyProperty(LoginProperty);
      }
      if (logout != null)
      {
        _logout = logout;
        SetDirtyProperty(LogoutProperty);
      }
      if (loginfailure != null)
      {
        _loginfailure = loginfailure;
        SetDirtyProperty(LoginfailureProperty);
      }
      if (loginfailurecount != null)
      {
        _loginfailurecount = loginfailurecount;
        SetDirtyProperty(LoginfailurecountProperty);
      }
      if (loginaddress != null)
      {
        _loginaddress = loginaddress;
        SetDirtyProperty(LoginaddressProperty);
      }
      if (US_DP_ID != null)
      {
        _US_DP_ID = US_DP_ID;
        SetDirtyProperty(US_DP_IDProperty);
      }
      if (US_PT_ID != null)
      {
        _US_PT_ID = US_PT_ID;
        SetDirtyProperty(US_PT_IDProperty);
      }
      if (locked != null)
      {
        _locked = locked;
        SetDirtyProperty(LockedProperty);
      }
    }

    /// <summary>
    /// 构建实体
    /// </summary>
    protected override object CreateInstance()
    {
      return new UserEasy();
    }

    /// <summary>
    /// 主键值
    /// </summary>
    [System.ComponentModel.Browsable(false)]
    [System.ComponentModel.DataAnnotations.Display(AutoGenerateField = false)]
    public override string PrimaryKey
    {
      get { return US_ID.ToString(); }
    }

    /// <summary>
    /// US_ID
    /// </summary>
    public static readonly Phenix.Core.Mapping.PropertyInfo<long?> US_IDProperty = RegisterProperty<long?>(c => c.US_ID);
    [Phenix.Core.Mapping.Field(FriendlyName = "US_ID", TableName = "PH_USER", ColumnName = "US_ID", IsPrimaryKey = true, NeedUpdate = true)]
    private long? _US_ID;
    /// <summary>
    /// US_ID
    /// </summary>
    [System.ComponentModel.DataAnnotations.Display(Name = "US_ID")]
    [System.ComponentModel.DisplayName("US_ID")]
    public long? US_ID
    {
      get { return GetProperty(US_IDProperty, _US_ID); }
    }

    /// <summary>
    /// US_USERNUMBER
    /// </summary>
    public static readonly Phenix.Core.Mapping.PropertyInfo<string> UsernumberProperty = RegisterProperty<string>(c => c.Usernumber);
    [Phenix.Core.Mapping.Field(FriendlyName = "US_USERNUMBER", Alias = "US_USERNUMBER", TableName = "PH_USER", ColumnName = "US_USERNUMBER", NeedUpdate = true)]
    private string _usernumber;
    /// <summary>
    /// US_USERNUMBER
    /// </summary>
    [System.ComponentModel.DataAnnotations.Display(Name = "US_USERNUMBER")]
    [System.ComponentModel.DisplayName("US_USERNUMBER")]
    public string Usernumber
    {
      get { return GetProperty(UsernumberProperty, _usernumber); }
      set { SetProperty(UsernumberProperty, ref _usernumber, value); }
    }

    /// <summary>
    /// US_PASSWORD
    /// </summary>
    public static readonly Phenix.Core.Mapping.PropertyInfo<string> PasswordProperty = RegisterProperty<string>(c => c.Password);
    [Phenix.Core.Mapping.Field(FriendlyName = "US_PASSWORD", Alias = "US_PASSWORD", TableName = "PH_USER", ColumnName = "US_PASSWORD", NeedUpdate = true)]
    private string _password;
    /// <summary>
    /// US_PASSWORD
    /// </summary>
    [System.ComponentModel.DataAnnotations.Display(Name = "US_PASSWORD")]
    [System.ComponentModel.DisplayName("US_PASSWORD")]
    public string Password
    {
      get { return GetProperty(PasswordProperty, _password); }
      set { SetProperty(PasswordProperty, ref _password, value); }
    }

    /// <summary>
    /// US_PASSWORDCHANGEDTIME
    /// </summary>
    public static readonly Phenix.Core.Mapping.PropertyInfo<System.DateTime?> PasswordchangedtimeProperty = RegisterProperty<System.DateTime?>(c => c.Passwordchangedtime);
    [Phenix.Core.Mapping.Field(FriendlyName = "US_PASSWORDCHANGEDTIME", Alias = "US_PASSWORDCHANGEDTIME", TableName = "PH_USER", ColumnName = "US_PASSWORDCHANGEDTIME", NeedUpdate = true)]
    private System.DateTime? _passwordchangedtime;
    /// <summary>
    /// US_PASSWORDCHANGEDTIME
    /// </summary>
    [System.ComponentModel.DataAnnotations.Display(Name = "US_PASSWORDCHANGEDTIME")]
    [System.ComponentModel.DisplayName("US_PASSWORDCHANGEDTIME")]
    public System.DateTime? Passwordchangedtime
    {
      get { return GetProperty(PasswordchangedtimeProperty, _passwordchangedtime); }
      set { SetProperty(PasswordchangedtimeProperty, ref _passwordchangedtime, value); }
    }

    /// <summary>
    /// US_NAME
    /// </summary>
    public static readonly Phenix.Core.Mapping.PropertyInfo<string> NameProperty = RegisterProperty<string>(c => c.Name);
    [Phenix.Core.Mapping.Field(FriendlyName = "US_NAME", Alias = "US_NAME", TableName = "PH_USER", ColumnName = "US_NAME", NeedUpdate = true, IsNameColumn = true, InLookUpColumn = true, InLookUpColumnDisplay = true)]
    private string _name;
    /// <summary>
    /// US_NAME
    /// </summary>
    [System.ComponentModel.DataAnnotations.Display(Name = "US_NAME")]
    [System.ComponentModel.DisplayName("US_NAME")]
    public string Name
    {
      get { return GetProperty(NameProperty, _name); }
      set { SetProperty(NameProperty, ref _name, value); }
    }

    /// <summary>
    /// US_LOGIN
    /// </summary>
    public static readonly Phenix.Core.Mapping.PropertyInfo<System.DateTime?> LoginProperty = RegisterProperty<System.DateTime?>(c => c.Login);
    [Phenix.Core.Mapping.Field(FriendlyName = "US_LOGIN", Alias = "US_LOGIN", TableName = "PH_USER", ColumnName = "US_LOGIN", NeedUpdate = true)]
    private System.DateTime? _login;
    /// <summary>
    /// US_LOGIN
    /// </summary>
    [System.ComponentModel.DataAnnotations.Display(Name = "US_LOGIN")]
    [System.ComponentModel.DisplayName("US_LOGIN")]
    public System.DateTime? Login
    {
      get { return GetProperty(LoginProperty, _login); }
      set { SetProperty(LoginProperty, ref _login, value); }
    }

    /// <summary>
    /// US_LOGOUT
    /// </summary>
    public static readonly Phenix.Core.Mapping.PropertyInfo<System.DateTime?> LogoutProperty = RegisterProperty<System.DateTime?>(c => c.Logout);
    [Phenix.Core.Mapping.Field(FriendlyName = "US_LOGOUT", Alias = "US_LOGOUT", TableName = "PH_USER", ColumnName = "US_LOGOUT", NeedUpdate = true)]
    private System.DateTime? _logout;
    /// <summary>
    /// US_LOGOUT
    /// </summary>
    [System.ComponentModel.DataAnnotations.Display(Name = "US_LOGOUT")]
    [System.ComponentModel.DisplayName("US_LOGOUT")]
    public System.DateTime? Logout
    {
      get { return GetProperty(LogoutProperty, _logout); }
      set { SetProperty(LogoutProperty, ref _logout, value); }
    }

    /// <summary>
    /// US_LOGINFAILURE
    /// </summary>
    public static readonly Phenix.Core.Mapping.PropertyInfo<System.DateTime?> LoginfailureProperty = RegisterProperty<System.DateTime?>(c => c.Loginfailure);
    [Phenix.Core.Mapping.Field(FriendlyName = "US_LOGINFAILURE", Alias = "US_LOGINFAILURE", TableName = "PH_USER", ColumnName = "US_LOGINFAILURE", NeedUpdate = true)]
    private System.DateTime? _loginfailure;
    /// <summary>
    /// US_LOGINFAILURE
    /// </summary>
    [System.ComponentModel.DataAnnotations.Display(Name = "US_LOGINFAILURE")]
    [System.ComponentModel.DisplayName("US_LOGINFAILURE")]
    public System.DateTime? Loginfailure
    {
      get { return GetProperty(LoginfailureProperty, _loginfailure); }
      set { SetProperty(LoginfailureProperty, ref _loginfailure, value); }
    }

    /// <summary>
    /// US_LOGINFAILURECOUNT
    /// </summary>
    public static readonly Phenix.Core.Mapping.PropertyInfo<int?> LoginfailurecountProperty = RegisterProperty<int?>(c => c.Loginfailurecount);
    [Phenix.Core.Mapping.Field(FriendlyName = "US_LOGINFAILURECOUNT", Alias = "US_LOGINFAILURECOUNT", TableName = "PH_USER", ColumnName = "US_LOGINFAILURECOUNT", NeedUpdate = true)]
    private int? _loginfailurecount;
    /// <summary>
    /// US_LOGINFAILURECOUNT
    /// </summary>
    [System.ComponentModel.DataAnnotations.Display(Name = "US_LOGINFAILURECOUNT")]
    [System.ComponentModel.DisplayName("US_LOGINFAILURECOUNT")]
    public int? Loginfailurecount
    {
      get { return GetProperty(LoginfailurecountProperty, _loginfailurecount); }
      set { SetProperty(LoginfailurecountProperty, ref _loginfailurecount, value); }
    }

    /// <summary>
    /// US_LOGINADDRESS
    /// </summary>
    public static readonly Phenix.Core.Mapping.PropertyInfo<string> LoginaddressProperty = RegisterProperty<string>(c => c.Loginaddress);
    [Phenix.Core.Mapping.Field(FriendlyName = "US_LOGINADDRESS", Alias = "US_LOGINADDRESS", TableName = "PH_USER", ColumnName = "US_LOGINADDRESS", NeedUpdate = true)]
    private string _loginaddress;
    /// <summary>
    /// US_LOGINADDRESS
    /// </summary>
    [System.ComponentModel.DataAnnotations.Display(Name = "US_LOGINADDRESS")]
    [System.ComponentModel.DisplayName("US_LOGINADDRESS")]
    public string Loginaddress
    {
      get { return GetProperty(LoginaddressProperty, _loginaddress); }
      set { SetProperty(LoginaddressProperty, ref _loginaddress, value); }
    }

    /// <summary>
    /// US_DP_ID
    /// </summary>
    public static readonly Phenix.Core.Mapping.PropertyInfo<long?> US_DP_IDProperty = RegisterProperty<long?>(c => c.US_DP_ID);
    [Phenix.Core.Mapping.Field(FriendlyName = "US_DP_ID", TableName = "PH_USER", ColumnName = "US_DP_ID", NeedUpdate = true)]
    private long? _US_DP_ID;
    /// <summary>
    /// US_DP_ID
    /// </summary>
    [System.ComponentModel.DataAnnotations.Display(Name = "US_DP_ID")]
    [System.ComponentModel.DisplayName("US_DP_ID")]
    public long? US_DP_ID
    {
      get { return GetProperty(US_DP_IDProperty, _US_DP_ID); }
      set { SetProperty(US_DP_IDProperty, ref _US_DP_ID, value); }
    }

    /// <summary>
    /// US_PT_ID
    /// </summary>
    public static readonly Phenix.Core.Mapping.PropertyInfo<long?> US_PT_IDProperty = RegisterProperty<long?>(c => c.US_PT_ID);
    [Phenix.Core.Mapping.Field(FriendlyName = "US_PT_ID", TableName = "PH_USER", ColumnName = "US_PT_ID", NeedUpdate = true)]
    private long? _US_PT_ID;
    /// <summary>
    /// US_PT_ID
    /// </summary>
    [System.ComponentModel.DataAnnotations.Display(Name = "US_PT_ID")]
    [System.ComponentModel.DisplayName("US_PT_ID")]
    public long? US_PT_ID
    {
      get { return GetProperty(US_PT_IDProperty, _US_PT_ID); }
      set { SetProperty(US_PT_IDProperty, ref _US_PT_ID, value); }
    }

    /// <summary>
    /// US_LOCKED
    /// </summary>
    public static readonly Phenix.Core.Mapping.PropertyInfo<int?> LockedProperty = RegisterProperty<int?>(c => c.Locked);
    [Phenix.Core.Mapping.Field(FriendlyName = "US_LOCKED", Alias = "US_LOCKED", TableName = "PH_USER", ColumnName = "US_LOCKED", NeedUpdate = true)]
    private int? _locked;
    /// <summary>
    /// US_LOCKED
    /// </summary>
    [System.ComponentModel.DataAnnotations.Display(Name = "US_LOCKED")]
    [System.ComponentModel.DisplayName("US_LOCKED")]
    public int? Locked
    {
      get { return GetProperty(LockedProperty, _locked); }
      set { SetProperty(LockedProperty, ref _locked, value); }
    }

    /// <summary>
    /// New
    /// </summary>
    public static UserEasy New(string usernumber, string password, DateTime? passwordchangedtime, string name, DateTime? login, DateTime? logout, DateTime? loginfailure, int? loginfailurecount, string loginaddress, long? US_DP_ID, long? US_PT_ID, int? locked)
    {
      UserEasy result = NewPure();
      result._usernumber = usernumber;
      result._password = password;
      result._passwordchangedtime = passwordchangedtime;
      result._name = name;
      result._login = login;
      result._logout = logout;
      result._loginfailure = loginfailure;
      result._loginfailurecount = loginfailurecount;
      result._loginaddress = loginaddress;
      result._US_DP_ID = US_DP_ID;
      result._US_PT_ID = US_PT_ID;
      result._locked = locked;
      return result;
    }
  }
}