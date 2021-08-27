using System;
using Phenix.Core.Data;

namespace Phenix.Test.使用指南._25._2
{
  /// <summary>
  /// 
  /// </summary>
  [Serializable]
  [System.ComponentModel.DisplayName("")]
  [Phenix.Core.Mapping.Class("PH_USER", FriendlyName = "")]
  public class UserEasy : EntityBase<UserEasy>
  {
    private UserEasy()
    {
      //禁止添加代码
    }

    /// <summary>
    /// 构建实体
    /// </summary>
    protected override object CreateInstance()
    {
      return new UserEasy();
    }

    /// <summary>
    /// 标签
    /// 缺省为唯一键值 
    /// 用于提示信息等
    /// </summary>
    [System.ComponentModel.Browsable(false)]
    [System.ComponentModel.DataAnnotations.Display(AutoGenerateField = false)]
    public override string Caption
    {
      get { return base.Caption; }
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
    public static readonly Phenix.Core.Mapping.PropertyInfo<long?> US_IDProperty = RegisterProperty<long?>(c => c.US_ID, "US_ID");

    /// <summary>
    /// US_ID
    /// </summary>
    [Phenix.Core.Mapping.Field(FriendlyName = "US_ID", TableName = "PH_USER", ColumnName = "US_ID", IsPrimaryKey = true, NeedUpdate = true)] private long? _US_ID;

    /// <summary>
    /// US_ID
    /// </summary>
    [System.ComponentModel.DisplayName("US_ID")]
    public long? US_ID
    {
      get { return _US_ID; }
      set { SetProperty(US_IDProperty, ref _US_ID, value); }
    }

    /// <summary>
    /// US_USERNUMBER
    /// </summary>
    public static readonly Phenix.Core.Mapping.PropertyInfo<string> UsernumberProperty = RegisterProperty<string>(c => c.Usernumber, "US_USERNUMBER");

    /// <summary>
    /// US_USERNUMBER
    /// </summary>
    [Phenix.Core.Mapping.Field(FriendlyName = "US_USERNUMBER", Alias = "US_USERNUMBER", TableName = "PH_USER", ColumnName = "US_USERNUMBER", NeedUpdate = true)] private string _usernumber;

    /// <summary>
    /// US_USERNUMBER
    /// </summary>
    [System.ComponentModel.DisplayName("US_USERNUMBER")]
    public string Usernumber
    {
      get { return _usernumber; }
      set { SetProperty(UsernumberProperty, ref _usernumber, value); }
    }

    /// <summary>
    /// US_PASSWORD
    /// </summary>
    public static readonly Phenix.Core.Mapping.PropertyInfo<string> PasswordProperty = RegisterProperty<string>(c => c.Password, "US_PASSWORD");

    /// <summary>
    /// US_PASSWORD
    /// </summary>
    [Phenix.Core.Mapping.Field(FriendlyName = "US_PASSWORD", Alias = "US_PASSWORD", TableName = "PH_USER", ColumnName = "US_PASSWORD", NeedUpdate = true)] private string _password;

    /// <summary>
    /// US_PASSWORD
    /// </summary>
    [System.ComponentModel.DisplayName("US_PASSWORD")]
    public string Password
    {
      get { return _password; }
      set { SetProperty(PasswordProperty, ref _password, value); }
    }

    /// <summary>
    /// US_NAME
    /// </summary>
    public static readonly Phenix.Core.Mapping.PropertyInfo<string> NameProperty = RegisterProperty<string>(c => c.Name, "US_NAME");

    /// <summary>
    /// US_NAME
    /// </summary>
    [Phenix.Core.Mapping.Field(FriendlyName = "US_NAME", Alias = "US_NAME", TableName = "PH_USER", ColumnName = "US_NAME", NeedUpdate = true, IsNameColumn = true, InLookUpColumn = true, InLookUpColumnDisplay = true)] private string _name;

    /// <summary>
    /// US_NAME
    /// </summary>
    [System.ComponentModel.DisplayName("US_NAME")]
    public string Name
    {
      get { return _name; }
      set { SetProperty(NameProperty, ref _name, value); }
    }

    /// <summary>
    /// US_LOGIN
    /// </summary>
    public static readonly Phenix.Core.Mapping.PropertyInfo<System.DateTime?> LoginProperty = RegisterProperty<System.DateTime?>(c => c.Login, "US_LOGIN");

    /// <summary>
    /// US_LOGIN
    /// </summary>
    [Phenix.Core.Mapping.Field(FriendlyName = "US_LOGIN", Alias = "US_LOGIN", TableName = "PH_USER", ColumnName = "US_LOGIN", NeedUpdate = true)] private System.DateTime? _login;

    /// <summary>
    /// US_LOGIN
    /// </summary>
    [System.ComponentModel.DisplayName("US_LOGIN")]
    public System.DateTime? Login
    {
      get { return _login; }
      set { SetProperty(LoginProperty, ref _login, value); }
    }

    /// <summary>
    /// US_LOGOUT
    /// </summary>
    public static readonly Phenix.Core.Mapping.PropertyInfo<System.DateTime?> LogoutProperty = RegisterProperty<System.DateTime?>(c => c.Logout, "US_LOGOUT");

    /// <summary>
    /// US_LOGOUT
    /// </summary>
    [Phenix.Core.Mapping.Field(FriendlyName = "US_LOGOUT", Alias = "US_LOGOUT", TableName = "PH_USER", ColumnName = "US_LOGOUT", NeedUpdate = true)] private System.DateTime? _logout;

    /// <summary>
    /// US_LOGOUT
    /// </summary>
    [System.ComponentModel.DisplayName("US_LOGOUT")]
    public System.DateTime? Logout
    {
      get { return _logout; }
      set { SetProperty(LogoutProperty, ref _logout, value); }
    }

    /// <summary>
    /// US_LOGINFAILURE
    /// </summary>
    public static readonly Phenix.Core.Mapping.PropertyInfo<System.DateTime?> LoginfailureProperty = RegisterProperty<System.DateTime?>(c => c.Loginfailure, "US_LOGINFAILURE");

    /// <summary>
    /// US_LOGINFAILURE
    /// </summary>
    [Phenix.Core.Mapping.Field(FriendlyName = "US_LOGINFAILURE", Alias = "US_LOGINFAILURE", TableName = "PH_USER", ColumnName = "US_LOGINFAILURE", NeedUpdate = true)] private System.DateTime? _loginfailure;

    /// <summary>
    /// US_LOGINFAILURE
    /// </summary>
    [System.ComponentModel.DisplayName("US_LOGINFAILURE")]
    public System.DateTime? Loginfailure
    {
      get { return _loginfailure; }
      set { SetProperty(LoginfailureProperty, ref _loginfailure, value); }
    }

    /// <summary>
    /// US_LOGINFAILURECOUNT
    /// </summary>
    public static readonly Phenix.Core.Mapping.PropertyInfo<int?> LoginfailurecountProperty = RegisterProperty<int?>(c => c.Loginfailurecount, "US_LOGINFAILURECOUNT");

    /// <summary>
    /// US_LOGINFAILURECOUNT
    /// </summary>
    [Phenix.Core.Mapping.Field(FriendlyName = "US_LOGINFAILURECOUNT", Alias = "US_LOGINFAILURECOUNT", TableName = "PH_USER", ColumnName = "US_LOGINFAILURECOUNT", NeedUpdate = true)] private int? _loginfailurecount;

    /// <summary>
    /// US_LOGINFAILURECOUNT
    /// </summary>
    [System.ComponentModel.DisplayName("US_LOGINFAILURECOUNT")]
    public int? Loginfailurecount
    {
      get { return _loginfailurecount; }
      set { SetProperty(LoginfailurecountProperty, ref _loginfailurecount, value); }
    }

    /// <summary>
    /// US_LOGINADDRESS
    /// </summary>
    public static readonly Phenix.Core.Mapping.PropertyInfo<string> LoginaddressProperty = RegisterProperty<string>(c => c.Loginaddress, "US_LOGINADDRESS");

    /// <summary>
    /// US_LOGINADDRESS
    /// </summary>
    [Phenix.Core.Mapping.Field(FriendlyName = "US_LOGINADDRESS", Alias = "US_LOGINADDRESS", TableName = "PH_USER", ColumnName = "US_LOGINADDRESS", NeedUpdate = true)] private string _loginaddress;

    /// <summary>
    /// US_LOGINADDRESS
    /// </summary>
    [System.ComponentModel.DisplayName("US_LOGINADDRESS")]
    public string Loginaddress
    {
      get { return _loginaddress; }
      set { SetProperty(LoginaddressProperty, ref _loginaddress, value); }
    }

    /// <summary>
    /// US_DP_ID
    /// </summary>
    public static readonly Phenix.Core.Mapping.PropertyInfo<long?> US_DP_IDProperty = RegisterProperty<long?>(c => c.US_DP_ID, "US_DP_ID");

    /// <summary>
    /// US_DP_ID
    /// </summary>
    [Phenix.Core.Mapping.Field(FriendlyName = "US_DP_ID", TableName = "PH_USER", ColumnName = "US_DP_ID", NeedUpdate = true)] private long? _US_DP_ID;

    /// <summary>
    /// US_DP_ID
    /// </summary>
    [System.ComponentModel.DisplayName("US_DP_ID")]
    public long? US_DP_ID
    {
      get { return _US_DP_ID; }
      set { SetProperty(US_DP_IDProperty, ref _US_DP_ID, value); }
    }

    /// <summary>
    /// US_PT_ID
    /// </summary>
    public static readonly Phenix.Core.Mapping.PropertyInfo<long?> US_PT_IDProperty = RegisterProperty<long?>(c => c.US_PT_ID, "US_PT_ID");

    /// <summary>
    /// US_PT_ID
    /// </summary>
    [Phenix.Core.Mapping.Field(FriendlyName = "US_PT_ID", TableName = "PH_USER", ColumnName = "US_PT_ID", NeedUpdate = true)] private long? _US_PT_ID;

    /// <summary>
    /// US_PT_ID
    /// </summary>
    [System.ComponentModel.DisplayName("US_PT_ID")]
    public long? US_PT_ID
    {
      get { return _US_PT_ID; }
      set { SetProperty(US_PT_IDProperty, ref _US_PT_ID, value); }
    }

    /// <summary>
    /// US_LOCKED
    /// </summary>
    public static readonly Phenix.Core.Mapping.PropertyInfo<int?> LockedProperty = RegisterProperty<int?>(c => c.Locked, "US_LOCKED");

    /// <summary>
    /// US_LOCKED
    /// </summary>
    [Phenix.Core.Mapping.Field(FriendlyName = "US_LOCKED", Alias = "US_LOCKED", TableName = "PH_USER", ColumnName = "US_LOCKED", NeedUpdate = true)] private int? _locked;

    /// <summary>
    /// US_LOCKED
    /// </summary>
    [System.ComponentModel.DisplayName("US_LOCKED")]
    public int? Locked
    {
      get { return _locked; }
      set { SetProperty(LockedProperty, ref _locked, value); }
    }

    /// <summary>
    /// US_PASSWORDCHANGEDTIME
    /// </summary>
    public static readonly Phenix.Core.Mapping.PropertyInfo<System.DateTime?> PasswordchangedtimeProperty = RegisterProperty<System.DateTime?>(c => c.Passwordchangedtime, "US_PASSWORDCHANGEDTIME");

    /// <summary>
    /// US_PASSWORDCHANGEDTIME
    /// </summary>
    [Phenix.Core.Mapping.Field(FriendlyName = "US_PASSWORDCHANGEDTIME", Alias = "US_PASSWORDCHANGEDTIME", TableName = "PH_USER", ColumnName = "US_PASSWORDCHANGEDTIME", NeedUpdate = true)] private System.DateTime? _passwordchangedtime;

    /// <summary>
    /// US_PASSWORDCHANGEDTIME
    /// </summary>
    [System.ComponentModel.DisplayName("US_PASSWORDCHANGEDTIME")]
    public System.DateTime? Passwordchangedtime
    {
      get { return _passwordchangedtime; }
      set { SetProperty(PasswordchangedtimeProperty, ref _passwordchangedtime, value); }
    }

    /// <summary>
    /// New
    /// </summary>
    public static UserEasy New(string usernumber, string password, string name, DateTime? login, DateTime? logout, DateTime? loginfailure, int? loginfailurecount, string loginaddress, long? US_DP_ID, long? US_PT_ID, int? locked, DateTime? passwordchangedtime)
    {
      UserEasy result = NewPure();
      result._usernumber = usernumber;
      result._password = password;
      result._name = name;
      result._login = login;
      result._logout = logout;
      result._loginfailure = loginfailure;
      result._loginfailurecount = loginfailurecount;
      result._loginaddress = loginaddress;
      result._US_DP_ID = US_DP_ID;
      result._US_PT_ID = US_PT_ID;
      result._locked = locked;
      result._passwordchangedtime = passwordchangedtime;
      return result;
    }
  }
}