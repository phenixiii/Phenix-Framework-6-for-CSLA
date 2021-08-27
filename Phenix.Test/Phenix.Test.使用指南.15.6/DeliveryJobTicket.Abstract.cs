using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Phenix.Test.使用指南._15._6.Business
{
  /// <summary>
  /// 出库作业单
  /// </summary>
  [Phenix.Core.Mapping.ClassAttribute("WGW_DELIVERY_JOB_TICKET_TEST", FriendlyName = "出库作业单"), System.ComponentModel.DisplayNameAttribute("出库作业单"), System.SerializableAttribute()]
  public abstract class DeliveryJobTicket<T> : Phenix.Business.BusinessBase<T> where T : DeliveryJobTicket<T>
  {
    /// <summary>
    /// SetFieldValues
    /// </summary>
    protected void SetFieldValues()
    {
      InitOldFieldValues();
      MarkDirty();
    }
    /// <summary>
    /// DJT_ID
    /// </summary>
    public static readonly Phenix.Business.PropertyInfo<long?> DJT_IDProperty = RegisterProperty<long?>(c => c.DJT_ID);
    [Phenix.Core.Mapping.Field(FriendlyName = "DJT_ID", TableName = "WGW_DELIVERY_JOB_TICKET_TEST", ColumnName = "DJT_ID", IsPrimaryKey = true, NeedUpdate = true)]
    private long? _DJT_ID;
    /// <summary>
    /// DJT_ID
    /// </summary>
    [System.ComponentModel.DisplayName("DJT_ID")]
    public long? DJT_ID
    {
      get { return GetProperty(DJT_IDProperty, _DJT_ID); }
      internal set
      {
        SetProperty(DJT_IDProperty, ref _DJT_ID, value);
      }
    }

    [System.ComponentModel.Browsable(false)]
    [System.ComponentModel.DataAnnotations.Display(AutoGenerateField = false)]
    public override string PrimaryKey
    {
      get { return DJT_ID.ToString(); }
    }

    /// <summary>
    /// 出库单号
    /// </summary>
    public static readonly Phenix.Business.PropertyInfo<string> SerialProperty = RegisterProperty<string>(c => c.Serial);
    [Phenix.Core.Mapping.Field(FriendlyName = "出库单号", Alias = "DJT_SERIAL", TableName = "WGW_DELIVERY_JOB_TICKET_TEST", ColumnName = "DJT_SERIAL", NeedUpdate = true, IsBusinessCodeColumn = true)]
    private string _serial;
    /// <summary>
    /// 出库单号
    /// </summary>
    [System.ComponentModel.DisplayName("出库单号")]
    public string Serial
    {
      get { return GetProperty(SerialProperty, _serial); }
      set { SetProperty(SerialProperty, ref _serial, value); }
    }

    /// <summary>
    /// 录入人
    /// </summary>
    public static readonly Phenix.Business.PropertyInfo<string> InputerProperty = RegisterProperty<string>(c => c.Inputer);
    [Phenix.Core.Mapping.Field(FriendlyName = "录入人", Alias = "DJT_INPUTER", TableName = "WGW_DELIVERY_JOB_TICKET_TEST", ColumnName = "DJT_INPUTER", NeedUpdate = true, OverwritingOnUpdate = true, IsInputerColumn = true)]
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
    public static readonly Phenix.Business.PropertyInfo<DateTime?> InputtimeProperty = RegisterProperty<DateTime?>(c => c.Inputtime);
    [Phenix.Core.Mapping.Field(FriendlyName = "录入时间", Alias = "DJT_INPUTTIME", TableName = "WGW_DELIVERY_JOB_TICKET_TEST", ColumnName = "DJT_INPUTTIME", NeedUpdate = true, OverwritingOnUpdate = true, IsInputTimeColumn = true)]
    private DateTime? _inputtime;
    /// <summary>
    /// 录入时间
    /// </summary>
    [System.ComponentModel.DisplayName("录入时间")]
    public DateTime? Inputtime
    {
      get { return GetProperty(InputtimeProperty, _inputtime); }
    }
  }
}
