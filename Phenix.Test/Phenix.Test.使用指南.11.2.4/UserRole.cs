using System;

namespace Phenix.Test.使用指南._11._2._4
{
  /// <summary>
  /// UserRole
  /// </summary>
  [Serializable]
  public class UserRole : UserRole<UserRole>
  {
    private UserRole()
    {
      //禁止添加代码
    }
  }

  /// <summary>
  /// UserRole清单
  /// </summary>
  [Serializable]
  public class UserRoleList : Phenix.Business.BusinessListBase<UserRoleList, UserRole>
  {
    private UserRoleList()
    {
      //禁止添加代码
    }
  }

  /// <summary>
  /// UserRole
  /// </summary>
  [Phenix.Core.Mapping.ClassAttribute("PH_USER_ROLE", FriendlyName = "UserRole"), System.ComponentModel.DisplayNameAttribute("UserRole"), System.SerializableAttribute()]
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
    /// UR_US_ID
    /// </summary>
    public static readonly Phenix.Business.PropertyInfo<long?> UR_US_IDProperty = RegisterProperty<long?>(c => c.UR_US_ID);
    [Phenix.Core.Mapping.Field(FriendlyName = "UR_US_ID", TableName = "PH_USER_ROLE", ColumnName = "UR_US_ID", NeedUpdate = true)]
    //* 因未构建物理外键，需显式申明关联主表
    [Phenix.Core.Mapping.FieldLink("PH_USER", "US_ID")]
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
    //* 因未构建物理外键，需显式申明关联主表
    [Phenix.Core.Mapping.FieldLink("PH_ROLE", "RL_ID")]
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
    /// UR_INPUTER
    /// </summary>
    public static readonly Phenix.Business.PropertyInfo<string> InputerProperty = RegisterProperty<string>(c => c.Inputer);
    [Phenix.Core.Mapping.Field(FriendlyName = "UR_INPUTER", Alias = "UR_INPUTER", TableName = "PH_USER_ROLE", ColumnName = "UR_INPUTER", NeedUpdate = true, OverwritingOnUpdate = true, IsInputerColumn = true)]
    private string _inputer;
    /// <summary>
    /// UR_INPUTER
    /// </summary>
    [System.ComponentModel.DisplayName("UR_INPUTER")]
    public string Inputer
    {
      get { return GetProperty(InputerProperty, _inputer); }
    }

    /// <summary>
    /// UR_INPUTTIME
    /// </summary>
    public static readonly Phenix.Business.PropertyInfo<DateTime?> InputtimeProperty = RegisterProperty<DateTime?>(c => c.Inputtime);
    [Phenix.Core.Mapping.Field(FriendlyName = "UR_INPUTTIME", Alias = "UR_INPUTTIME", TableName = "PH_USER_ROLE", ColumnName = "UR_INPUTTIME", NeedUpdate = true, OverwritingOnUpdate = true, IsInputTimeColumn = true)]
    private DateTime? _inputtime;
    /// <summary>
    /// UR_INPUTTIME
    /// </summary>
    [System.ComponentModel.DisplayName("UR_INPUTTIME")]
    public DateTime? Inputtime
    {
      get { return GetProperty(InputtimeProperty, _inputtime); }
    }

    /// <summary>
    /// New
    /// </summary>
    public static T New(long? UR_US_ID, long? UR_RL_ID)
    {
      T result = NewPure();
      result._UR_US_ID = UR_US_ID;
      result._UR_RL_ID = UR_RL_ID;
      return result;
    }

    /// <summary>
    /// SetFieldValues
    /// </summary>
    protected void SetFieldValues(long? UR_US_ID, long? UR_RL_ID)
    {
      InitOldFieldValues();
      _UR_US_ID = UR_US_ID;
      _UR_RL_ID = UR_RL_ID;
      MarkDirty();
    }
  }
}
