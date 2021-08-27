using System;

namespace Phenix.Test.使用指南._12._6._2._7
{
  /// <summary>
  /// FinancialBigClass
  /// </summary>
  [Serializable]
  [Phenix.Core.Mapping.ReadOnly]
  public class FinancialBigClassReadOnly : FinancialBigClass<FinancialBigClassReadOnly>
  {
    private FinancialBigClassReadOnly()
    {
      //禁止添加代码
    }
  }

  /// <summary>
  /// FinancialBigClass清单
  /// </summary>
  [Serializable]
  public class FinancialBigClassReadOnlyList : Phenix.Business.BusinessListBase<FinancialBigClassReadOnlyList, FinancialBigClassReadOnly>
  {
    private FinancialBigClassReadOnlyList()
    {
      //禁止添加代码
    }
  }

  /// <summary>
  /// FinancialBigClass
  /// </summary>
  [Serializable]
  public class FinancialBigClass : FinancialBigClass<FinancialBigClass>
  {
    private FinancialBigClass()
    {
      //禁止添加代码
    }
  }

  /// <summary>
  /// FinancialBigClass清单
  /// </summary>
  [Serializable]
  public class FinancialBigClassList : Phenix.Business.BusinessListBase<FinancialBigClassList, FinancialBigClass>
  {
    private FinancialBigClassList()
    {
      //禁止添加代码
    }
  }

  /// <summary>
  /// 财务大类
  /// </summary>
  [Phenix.Core.Mapping.ClassAttribute("CFC_FINANCIAL_BIG_CLASS", FriendlyName = "财务大类"), System.ComponentModel.DisplayNameAttribute("财务大类"), System.SerializableAttribute()]
  public abstract class FinancialBigClass<T> : Phenix.Business.BusinessBase<T> where T : FinancialBigClass<T>
  {
    /// <summary>
    /// ID
    /// </summary>
    public static readonly Phenix.Business.PropertyInfo<long?> FBC_IDProperty = RegisterProperty<long?>(c => c.FBC_ID);
    [Phenix.Core.Mapping.Field(FriendlyName = "ID", TableName = "CFC_FINANCIAL_BIG_CLASS", ColumnName = "FBC_ID", IsPrimaryKey = true, NeedUpdate = true)]
    private long? _FBC_ID;
    /// <summary>
    /// ID
    /// </summary>
    [System.ComponentModel.DisplayName("ID")]
    public long? FBC_ID
    {
      get { return GetProperty(FBC_IDProperty, _FBC_ID); }
      internal set
      {
        SetProperty(FBC_IDProperty, ref _FBC_ID, value);
      }
    }

    [System.ComponentModel.Browsable(false)]
    [System.ComponentModel.DataAnnotations.Display(AutoGenerateField = false)]
    public override string PrimaryKey
    {
      get { return FBC_ID.ToString(); }
    }
  }
}
