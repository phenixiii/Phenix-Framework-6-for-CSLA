using System;

namespace Phenix.Test.使用指南._12._6._2._7
{
  /// <summary>
  /// FinancialSmallClass
  /// </summary>
  [Serializable]
  [Phenix.Core.Mapping.ReadOnly]
  public class FinancialSmallClassReadOnly : FinancialSmallClass<FinancialSmallClassReadOnly>
  {
    private FinancialSmallClassReadOnly()
    {
      //禁止添加代码
    }
  }

  /// <summary>
  /// FinancialSmallClass清单
  /// </summary>
  [Serializable]
  public class FinancialSmallClassReadOnlyList : Phenix.Business.BusinessListBase<FinancialSmallClassReadOnlyList, FinancialSmallClassReadOnly>
  {
    private FinancialSmallClassReadOnlyList()
    {
      //禁止添加代码
    }
  }

  /// <summary>
  /// FinancialSmallClass
  /// </summary>
  [Serializable]
  public class FinancialSmallClass : FinancialSmallClass<FinancialSmallClass>
  {
    private FinancialSmallClass()
    {
      //禁止添加代码
    }
  }

  /// <summary>
  /// FinancialSmallClass清单
  /// </summary>
  [Serializable]
  public class FinancialSmallClassList : Phenix.Business.BusinessListBase<FinancialSmallClassList, FinancialSmallClass>
  {
    private FinancialSmallClassList()
    {
      //禁止添加代码
    }
  }

  /// <summary>
  /// 财务小类
  /// </summary>
  //* CascadingDelete = true
  [Phenix.Core.Mapping.ClassDetailAttribute("CFC_FINANCIAL_CLASS_RELATION_I", "FCI_FCR_ID", null, "CFC_FINANCIAL_CLASS_RELATION", FriendlyName = "财务类关系", CascadingDelete = true)]
  [Phenix.Core.Mapping.ClassDetailAttribute("CFC_FINANCIAL_CLASS_RELATION", "FCR_FSC_ID", "FK_FCR_FSC_ID", FriendlyName = "财务类关系", CascadingDelete = true), Phenix.Core.Mapping.ClassAttribute("CFC_FINANCIAL_SMALL_CLASS", FriendlyName = "财务小类"), System.ComponentModel.DisplayNameAttribute("财务小类"), System.SerializableAttribute()]
  public abstract class FinancialSmallClass<T> : Phenix.Business.BusinessBase<T> where T : FinancialSmallClass<T>
  {
    /// <summary>
    /// ID
    /// </summary>
    public static readonly Phenix.Business.PropertyInfo<long?> FSC_IDProperty = RegisterProperty<long?>(c => c.FSC_ID);
    [Phenix.Core.Mapping.Field(FriendlyName = "ID", TableName = "CFC_FINANCIAL_SMALL_CLASS", ColumnName = "FSC_ID", IsPrimaryKey = true, NeedUpdate = true)]
    private long? _FSC_ID;
    /// <summary>
    /// ID
    /// </summary>
    [System.ComponentModel.DisplayName("ID")]
    public long? FSC_ID
    {
      get { return GetProperty(FSC_IDProperty, _FSC_ID); }
      internal set
      {
        SetProperty(FSC_IDProperty, ref _FSC_ID, value);
      }
    }

    [System.ComponentModel.Browsable(false)]
    [System.ComponentModel.DataAnnotations.Display(AutoGenerateField = false)]
    public override string PrimaryKey
    {
      get { return FSC_ID.ToString(); }
    }
  }
}
