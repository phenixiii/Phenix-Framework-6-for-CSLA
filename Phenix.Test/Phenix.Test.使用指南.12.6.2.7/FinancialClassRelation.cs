using System;

namespace Phenix.Test.使用指南._12._6._2._7
{
  /// <summary>
  /// FinancialClassRelation
  /// </summary>
  [Serializable]
  [Phenix.Core.Mapping.ReadOnly]
  public class FinancialClassRelationReadOnly : FinancialClassRelation<FinancialClassRelationReadOnly>
  {
    private FinancialClassRelationReadOnly()
    {
      //禁止添加代码
    }
  }

  /// <summary>
  /// FinancialClassRelation清单
  /// </summary>
  [Serializable]
  public class FinancialClassRelationReadOnlyList : Phenix.Business.BusinessListBase<FinancialClassRelationReadOnlyList, FinancialClassRelationReadOnly>
  {
    private FinancialClassRelationReadOnlyList()
    {
      //禁止添加代码
    }
  }

  /// <summary>
  /// FinancialClassRelation
  /// </summary>
  [Serializable]
  public class FinancialClassRelation : FinancialClassRelation<FinancialClassRelation>
  {
    private FinancialClassRelation()
    {
      //禁止添加代码
    }
  }

  /// <summary>
  /// FinancialClassRelation清单
  /// </summary>
  [Serializable]
  public class FinancialClassRelationList : Phenix.Business.BusinessListBase<FinancialClassRelationList, FinancialClassRelation>
  {
    private FinancialClassRelationList()
    {
      //禁止添加代码
    }
  }

  /// <summary>
  /// 财务类关系
  /// </summary>
  [Phenix.Core.Mapping.ClassAttribute("CFC_FINANCIAL_CLASS_RELATION", FriendlyName = "财务类关系"), System.ComponentModel.DisplayNameAttribute("财务类关系"), System.SerializableAttribute()]
  public abstract class FinancialClassRelation<T> : Phenix.Business.BusinessBase<T> where T : FinancialClassRelation<T>
  {
    /// <summary>
    /// ID
    /// </summary>
    public static readonly Phenix.Business.PropertyInfo<long?> FCR_IDProperty = RegisterProperty<long?>(c => c.FCR_ID);
    [Phenix.Core.Mapping.Field(FriendlyName = "ID", TableName = "CFC_FINANCIAL_CLASS_RELATION", ColumnName = "FCR_ID", IsPrimaryKey = true, NeedUpdate = true)]
    private long? _FCR_ID;
    /// <summary>
    /// ID
    /// </summary>
    [System.ComponentModel.DisplayName("ID")]
    public long? FCR_ID
    {
      get { return GetProperty(FCR_IDProperty, _FCR_ID); }
      internal set
      {
        SetProperty(FCR_IDProperty, ref _FCR_ID, value);
      }
    }

    /// <summary>
    /// 财务大类
    /// </summary>
    public static readonly Phenix.Business.PropertyInfo<long?> FCR_FBC_IDProperty = RegisterProperty<long?>(c => c.FCR_FBC_ID);
    [Phenix.Core.Mapping.Field(FriendlyName = "财务大类", TableName = "CFC_FINANCIAL_CLASS_RELATION", ColumnName = "FCR_FBC_ID", NeedUpdate = true)]
    private long? _FCR_FBC_ID;
    /// <summary>
    /// 财务大类
    /// </summary>
    [System.ComponentModel.DisplayName("财务大类")]
    public long? FCR_FBC_ID
    {
      get { return GetProperty(FCR_FBC_IDProperty, _FCR_FBC_ID); }
      set
      {
        SetProperty(FCR_FBC_IDProperty, ref _FCR_FBC_ID, value);
      }
    }

    /// <summary>
    /// 财务小类
    /// </summary>
    public static readonly Phenix.Business.PropertyInfo<long?> FCR_FSC_IDProperty = RegisterProperty<long?>(c => c.FCR_FSC_ID);
    [Phenix.Core.Mapping.Field(FriendlyName = "财务小类", TableName = "CFC_FINANCIAL_CLASS_RELATION", ColumnName = "FCR_FSC_ID", NeedUpdate = true)]
    private long? _FCR_FSC_ID;
    /// <summary>
    /// 财务小类
    /// </summary>
    [System.ComponentModel.DisplayName("财务小类")]
    public long? FCR_FSC_ID
    {
      get { return GetProperty(FCR_FSC_IDProperty, _FCR_FSC_ID); }
      set
      {
        SetProperty(FCR_FSC_IDProperty, ref _FCR_FSC_ID, value);
      }
    }

    /// <summary>
    /// 统计时段
    /// </summary>
    public static readonly Phenix.Business.PropertyInfo<long?> FCR_FSP_IDProperty = RegisterProperty<long?>(c => c.FCR_FSP_ID);
    [Phenix.Core.Mapping.Field(FriendlyName = "统计时段", TableName = "CFC_FINANCIAL_CLASS_RELATION", ColumnName = "FCR_FSP_ID", NeedUpdate = true)]
    private long? _FCR_FSP_ID;
    /// <summary>
    /// 统计时段
    /// </summary>
    [System.ComponentModel.DisplayName("统计时段")]
    public long? FCR_FSP_ID
    {
      get { return GetProperty(FCR_FSP_IDProperty, _FCR_FSP_ID); }
      set
      {
        SetProperty(FCR_FSP_IDProperty, ref _FCR_FSP_ID, value);
      }
    }

    [System.ComponentModel.Browsable(false)]
    [System.ComponentModel.DataAnnotations.Display(AutoGenerateField = false)]
    public override string PrimaryKey
    {
      get { return FCR_ID.ToString(); }
    }
  }
}
