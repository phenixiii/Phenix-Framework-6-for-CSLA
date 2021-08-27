using System;
using Phenix.Business;

/* 
   builder:    phenixiii
   build time: 2017-06-04 10:04:02
   notes:      
*/

namespace Phenix.Test.使用指南._24
{
  /// <summary></summary>
  [Core.Mapping.Class("PH_USER_ROLE", FriendlyName = "")]
  [System.Serializable]
  [System.ComponentModel.DisplayName("")]
  public abstract class UserRole<T> : Phenix.Business.BusinessBase<T>
    where T : UserRole<T>
  {
    [System.ComponentModel.Browsable(false)]
    [System.ComponentModel.DataAnnotations.Display(AutoGenerateField = false)]
    public override string PrimaryKey
    {
      get { return base.PrimaryKey; }
    }

    /// <summary>
    /// UR_ID
    /// </summary>
    public static readonly Phenix.Business.PropertyInfo<long?> UR_IDProperty = RegisterProperty<long?>(c => c.UR_ID);
    [Phenix.Core.Mapping.Field(FriendlyName = "UR_ID", TableName = "PH_USER_ROLE", ColumnName = "UR_ID", IsPrimaryKey = true, NeedUpdate = true)]
    private long? _UR_ID;
    /// <summary>
    /// UR_ID
    /// </summary>
    [System.ComponentModel.DataAnnotations.Display(Name = "UR_ID")]
    [System.ComponentModel.DisplayName("UR_ID")]
    public long? UR_ID
    {
      get { return GetProperty(UR_IDProperty, _UR_ID); }
    }

    /// <summary>
    /// UR_US_ID
    /// </summary>
    public static readonly Phenix.Business.PropertyInfo<long?> UR_US_IDProperty = RegisterProperty<long?>(c => c.UR_US_ID);
    [Phenix.Core.Mapping.Field(FriendlyName = "UR_US_ID", TableName = "PH_USER_ROLE", ColumnName = "UR_US_ID", NeedUpdate = true)]
    [Phenix.Core.Mapping.FieldLink("PH_USER", "US_ID")]
    private long? _UR_US_ID;
    /// <summary>
    /// UR_US_ID
    /// </summary>
    [System.ComponentModel.DataAnnotations.Display(Name = "UR_US_ID")]
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
    [Phenix.Core.Mapping.FieldLink("PH_ROLE", "RL_ID")]
    private long? _UR_RL_ID;
    /// <summary>
    /// UR_RL_ID
    /// </summary>
    [System.ComponentModel.DataAnnotations.Display(Name = "UR_RL_ID")]
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
    [System.ComponentModel.DataAnnotations.Display(Name = "UR_INPUTER")]
    [System.ComponentModel.DisplayName("UR_INPUTER")]
    public string Inputer
    {
      get { return GetProperty(InputerProperty, _inputer); }
      set { SetProperty(InputerProperty, ref _inputer, value); }
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
    [System.ComponentModel.DataAnnotations.Display(Name = "UR_INPUTTIME")]
    [System.ComponentModel.DisplayName("UR_INPUTTIME")]
    public DateTime? Inputtime
    {
      get { return GetProperty(InputtimeProperty, _inputtime); }
      set { SetProperty(InputtimeProperty, ref _inputtime, value); }
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

    protected UserRole()
    {
      //禁止添加代码
    }

    protected UserRole(bool? isNew, bool? isSelfDirty, bool? isSelfDeleted, long? UR_ID, long? UR_US_ID, long? UR_RL_ID, string inputer, DateTime? inputtime)
      : base(isNew, isSelfDirty, isSelfDeleted)
    {
      if (UR_ID != null)
      {
        _UR_ID = UR_ID;
        SetDirtyProperty(UR_IDProperty);
      }
      if (UR_US_ID != null)
      {
        _UR_US_ID = UR_US_ID;
        SetDirtyProperty(UR_US_IDProperty);
      }
      if (UR_RL_ID != null)
      {
        _UR_RL_ID = UR_RL_ID;
        SetDirtyProperty(UR_RL_IDProperty);
      }
      if (inputer != null)
      {
        _inputer = inputer;
        SetDirtyProperty(InputerProperty);
      }
      if (inputtime != null)
      {
        _inputtime = inputtime;
        SetDirtyProperty(InputtimeProperty);
      }
    }
  }
}