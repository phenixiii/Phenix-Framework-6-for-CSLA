 using System;

namespace Phenix.Test.使用指南._12._6._2._7
{
  /// <summary>
  /// FinancialStatisticPeriod
  /// </summary>
  [Serializable]
  [Phenix.Core.Mapping.ReadOnly]
  public class FinancialStatisticPeriodReadOnly : FinancialStatisticPeriod<FinancialStatisticPeriodReadOnly>
  {
    private FinancialStatisticPeriodReadOnly()
    {
      //禁止添加代码 
    }
  }

  /// <summary>
  /// FinancialStatisticPeriod清单
  /// </summary>
  [Serializable]
  public class FinancialStatisticPeriodReadOnlyList : Phenix.Business.BusinessListBase<FinancialStatisticPeriodReadOnlyList, FinancialStatisticPeriodReadOnly>
  {
    private FinancialStatisticPeriodReadOnlyList()
    {
      //禁止添加代码
    }
  }

  /// <summary>
  /// FinancialStatisticPeriod
  /// </summary>
  [Serializable]
  public class FinancialStatisticPeriod : FinancialStatisticPeriod<FinancialStatisticPeriod>
  {
    private FinancialStatisticPeriod()
    {
      //禁止添加代码
    }
  }

  /// <summary>
  /// FinancialStatisticPeriod清单
  /// </summary>
  [Serializable]
  public class FinancialStatisticPeriodList : Phenix.Business.BusinessListBase<FinancialStatisticPeriodList, FinancialStatisticPeriod>
  {
    private FinancialStatisticPeriodList()
    {
      //禁止添加代码
    }
  }

  /// <summary>
  /// 财务统计时段
  /// </summary>
  //* CascadingDelete = false
  [Phenix.Core.Mapping.ClassDetailAttribute("CFC_FINANCIAL_CLASS_RELATION", "FCR_FSP_ID", "FK_FCR_FSP_ID", FriendlyName = "财务类关系", CascadingDelete = false), Phenix.Core.Mapping.ClassAttribute("CFC_FINANCIAL_STATISTIC_PERIOD", FriendlyName = "财务统计时段"), System.ComponentModel.DisplayNameAttribute("财务统计时段"), System.SerializableAttribute()]
  public abstract class FinancialStatisticPeriod<T> : Phenix.Business.BusinessBase<T> where T : FinancialStatisticPeriod<T>
  {
    /// <summary>
    /// ID
    /// </summary>
    public static readonly Phenix.Business.PropertyInfo<long?> FSP_IDProperty = RegisterProperty<long?>(c => c.FSP_ID);
    [Phenix.Core.Mapping.Field(FriendlyName = "ID", TableName = "CFC_FINANCIAL_STATISTIC_PERIOD", ColumnName = "FSP_ID", IsPrimaryKey = true, NeedUpdate = true)]
    private long? _FSP_ID;
    /// <summary>
    /// ID
    /// </summary>
    [System.ComponentModel.DisplayName("ID")]
    public long? FSP_ID
    {
      get { return GetProperty(FSP_IDProperty, _FSP_ID); }
      internal set
      {
        SetProperty(FSP_IDProperty, ref _FSP_ID, value);
      }
    }

    [System.ComponentModel.Browsable(false)]
    [System.ComponentModel.DataAnnotations.Display(AutoGenerateField = false)]
    public override string PrimaryKey
    {
      get { return FSP_ID.ToString(); }
    }
  }
}
