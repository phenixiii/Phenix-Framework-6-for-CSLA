using System;

namespace Phenix.Test.使用指南._12._6._2._8
{
  /// <summary>
  /// Serial
  /// </summary>
  [Serializable]
  public class Serial : Serial<Serial>
  {
  }

  /// <summary>
  /// Serial清单
  /// </summary>
  [Serializable]
  public class SerialList : Phenix.Business.BusinessListBase<SerialList, Serial>
  {
  }

  /// <summary>
  /// Serial
  /// </summary>
  [Phenix.Core.Mapping.ClassAttribute("PH_SERIAL", FriendlyName = "Serial"), System.ComponentModel.DisplayNameAttribute("Serial"), System.SerializableAttribute()]
  public abstract class Serial<T> : Phenix.Business.BusinessBase<T> where T : Serial<T>
  {
    /// <summary>
    /// SR_KEY
    /// </summary>
    public static readonly Phenix.Business.PropertyInfo<string> KeyProperty = RegisterProperty<string>(c => c.Key);
    [Phenix.Core.Mapping.Field(FriendlyName = "SR_KEY", Alias = "SR_KEY", TableName = "PH_SERIAL", ColumnName = "SR_KEY", IsPrimaryKey = true, NeedUpdate = true)]
    private string _key;
    /// <summary>
    /// SR_KEY
    /// </summary>
    [System.ComponentModel.DisplayName("SR_KEY")]
    public string Key
    {
      get { return GetProperty(KeyProperty, _key); }
      internal set
      {
        SetProperty(KeyProperty, ref _key, value);
      }
    }

    [System.ComponentModel.Browsable(false)]
    [System.ComponentModel.DataAnnotations.Display(AutoGenerateField = false)]
    public override string PrimaryKey
    {
      get { return Key.ToString(); }
    }

    /// <summary>
    /// SR_VALUE
    /// </summary>
    public static readonly Phenix.Business.PropertyInfo<long?> ValueProperty = RegisterProperty<long?>(c => c.Value);
    [Phenix.Core.Mapping.Field(FriendlyName = "SR_VALUE", Alias = "SR_VALUE", TableName = "PH_SERIAL", ColumnName = "SR_VALUE", NeedUpdate = true)]
    private long? _value;
    /// <summary>
    /// SR_VALUE
    /// </summary>
    [System.ComponentModel.DisplayName("SR_VALUE")]
    public long? Value
    {
      get { return GetProperty(ValueProperty, _value); }
      set { SetProperty(ValueProperty, ref _value, value); }
    }

    /// <summary>
    /// SR_TIME
    /// </summary>
    public static readonly Phenix.Business.PropertyInfo<DateTime?> TimeProperty = RegisterProperty<DateTime?>(c => c.Time);
    //* 指定本字段用于乐观锁：CheckDirtyOnSaving = true
    [Phenix.Core.Mapping.Field(FriendlyName = "SR_TIME", Alias = "SR_TIME", TableName = "PH_SERIAL", ColumnName = "SR_TIME", NeedUpdate = true, CheckDirtyOnSaving = true)]
    private DateTime? _time;
    /// <summary>
    /// SR_TIME
    /// </summary>
    [System.ComponentModel.DisplayName("SR_TIME")]
    public DateTime? Time
    {
      get { return GetProperty(TimeProperty, _time); }
      set { SetProperty(TimeProperty, ref _time, value); }
    }

    /// <summary>
    /// New
    /// </summary>
    public static T New(string key, long? value, DateTime? time)
    {
      T result = NewPure();
      result._key = key;
      result._value = value;
      result._time = time;
      return result;
    }

    /// <summary>
    /// SetFieldValues
    /// </summary>
    protected void SetFieldValues(string key, long? value, DateTime? time)
    {
      InitOldFieldValues();
      _key = key;
      _value = value;
      _time = time;
      MarkDirty();
    }
  }
}
