using System;

namespace Phenix.Security.Business
{
  /// <summary>
  /// 用户-切片
  /// </summary>
  [Serializable]
  [Phenix.Core.Mapping.ReadOnly]
  public class UserSectionReadOnly : UserSection<UserSectionReadOnly>
  {
  }

  /// <summary>
  /// 用户-切片清单
  /// </summary>
  [Serializable]
  public class UserSectionReadOnlyList : Phenix.Business.BusinessListBase<UserSectionReadOnlyList, UserSectionReadOnly>
  {
  }

  /// <summary>
  /// 用户-切片
  /// </summary>
  [Serializable]
  public class UserSection : UserSection<UserSection>
  {
  }

  /// <summary>
  /// 用户-切片清单
  /// </summary>
  [Serializable]
  public class UserSectionList : Phenix.Business.BusinessListBase<UserSectionList, UserSection>
  {
  }

  /// <summary>
  /// 用户-切片
  /// </summary>
  [Phenix.Core.Mapping.ClassAttribute("PH_USER_SECTION", FriendlyName = "用户-切片"), System.SerializableAttribute(), System.ComponentModel.DisplayNameAttribute("用户-切片")]
  public abstract class UserSection<T> : Phenix.Business.BusinessBase<T> where T : UserSection<T>
  {
    /// <summary>
    /// US_ID
    /// </summary>
    public static readonly Phenix.Business.PropertyInfo<long?> US_IDProperty = RegisterProperty<long?>(c => c.US_ID);
    [Phenix.Core.Mapping.Field(FriendlyName = "US_ID", TableName = "PH_USER_SECTION", ColumnName = "US_ID", IsPrimaryKey = true, NeedUpdate = true)]
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

    [System.ComponentModel.Browsable(false)]
    [System.ComponentModel.DataAnnotations.Display(AutoGenerateField = false)]
    public override string PrimaryKey
    {
      get { return US_ID.ToString(); }
    }

    /// <summary>
    /// 用户
    /// </summary>
    public static readonly Phenix.Business.PropertyInfo<long?> US_US_IDProperty = RegisterProperty<long?>(c => c.US_US_ID);
    [Phenix.Core.Mapping.Field(FriendlyName = "用户", TableName = "PH_USER_SECTION", ColumnName = "US_US_ID", NeedUpdate = true)]
    [Phenix.Core.Mapping.FieldLink("PH_USER", "US_ID")]
    private long? _US_US_ID;
    /// <summary>
    /// 用户
    /// </summary>
    [System.ComponentModel.DisplayName("用户")]
    public long? US_US_ID
    {
      get { return GetProperty(US_US_IDProperty, _US_US_ID); }
      set { SetProperty(US_US_IDProperty, ref _US_US_ID, value); }
    }

    /// <summary>
    /// 切片
    /// </summary>
    public static readonly Phenix.Business.PropertyInfo<long?> US_ST_IDProperty = RegisterProperty<long?>(c => c.US_ST_ID);
    [Phenix.Core.Mapping.Field(FriendlyName = "切片", TableName = "PH_USER_SECTION", ColumnName = "US_ST_ID", NeedUpdate = true)]
    [Phenix.Core.Mapping.FieldLink("PH_SECTION", "ST_ID")]
    private long? _US_ST_ID;
    /// <summary>
    /// 切片
    /// </summary>
    [System.ComponentModel.DisplayName("切片")]
    public long? US_ST_ID
    {
      get { return GetProperty(US_ST_IDProperty, _US_ST_ID); }
      set { SetProperty(US_ST_IDProperty, ref _US_ST_ID, value); }
    }

    /// <summary>
    /// 录入人
    /// </summary>
    public static readonly Phenix.Business.PropertyInfo<string> InputerProperty = RegisterProperty<string>(c => c.Inputer);
    [Phenix.Core.Mapping.Field(FriendlyName = "录入人", Alias = "US_INPUTER", TableName = "PH_USER_SECTION", ColumnName = "US_INPUTER", NeedUpdate = true, OverwritingOnUpdate = true, IsInputerColumn = true)]
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
    [Phenix.Core.Mapping.Field(FriendlyName = "录入时间", Alias = "US_INPUTTIME", TableName = "PH_USER_SECTION", ColumnName = "US_INPUTTIME", NeedUpdate = true, OverwritingOnUpdate = true, IsInputTimeColumn = true)]
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
