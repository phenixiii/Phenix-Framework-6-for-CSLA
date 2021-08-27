using System;
using Phenix.Core.Data;

namespace Phenix.Test.使用指南._03._5
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
    [Phenix.Core.Mapping.Field(FriendlyName = "US_ID", TableName = "PH_USER", ColumnName = "US_ID", IsPrimaryKey = true, NeedUpdate = true)]
    private long? _US_ID;
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
    [Phenix.Core.Mapping.Field(FriendlyName = "US_USERNUMBER", Alias = "US_USERNUMBER", TableName = "PH_USER", ColumnName = "US_USERNUMBER", NeedUpdate = true)]
    private string _usernumber;
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
    [Phenix.Core.Mapping.Field(FriendlyName = "US_PASSWORD", Alias = "US_PASSWORD", TableName = "PH_USER", ColumnName = "US_PASSWORD", NeedUpdate = true)]
    private string _password;
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
    [Phenix.Core.Mapping.Field(FriendlyName = "US_NAME", Alias = "US_NAME", TableName = "PH_USER", ColumnName = "US_NAME", NeedUpdate = true, IsNameColumn = true, InLookUpColumn = true, InLookUpColumnDisplay = true)]
    private string _name;
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
    /// US_DP_ID
    /// </summary>
    public static readonly Phenix.Core.Mapping.PropertyInfo<long?> US_DP_IDProperty = RegisterProperty<long?>(c => c.US_DP_ID, "US_DP_ID");
    /// <summary>
    /// US_DP_ID
    /// </summary>
    [Phenix.Core.Mapping.Field(FriendlyName = "US_DP_ID", TableName = "PH_USER", ColumnName = "US_DP_ID", NeedUpdate = true)]
    private long? _US_DP_ID;
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
    [Phenix.Core.Mapping.Field(FriendlyName = "US_PT_ID", TableName = "PH_USER", ColumnName = "US_PT_ID", NeedUpdate = true)]
    private long? _US_PT_ID;
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
    /// US_Locked
    /// </summary>
    public static readonly Phenix.Core.Mapping.PropertyInfo<bool?> LockedProperty = RegisterProperty<bool?>(c => c.Locked);
    [Phenix.Core.Mapping.Field(FriendlyName = "US_Locked", Alias = "US_Locked", TableName = "PH_User", ColumnName = "US_Locked")]
    private int? _locked;
    /// <summary>
    /// US_Locked
    /// </summary>
    [System.ComponentModel.DataAnnotations.Display(Name = "US_Locked")]
    [System.ComponentModel.DisplayName("US_Locked")]
    public bool? Locked
    {
      get { return GetPropertyConvert(LockedProperty, _locked); }
      set { SetPropertyConvert(LockedProperty, ref _locked, value); }
    }
  }
}