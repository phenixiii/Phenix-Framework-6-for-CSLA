using System;

namespace Phenix.Test.使用指南._11._2._5
{
  /// <summary>
  /// Department
  /// </summary>
  [Serializable]
  [Phenix.Core.Mapping.ReadOnly]
  public class DepartmentReadOnly : Department<DepartmentReadOnly>
  {
  }

  /// <summary>
  /// Department清单
  /// </summary>
  [Serializable]
  public class DepartmentReadOnlyList : Phenix.Business.BusinessListBase<DepartmentReadOnlyList, DepartmentReadOnly>
  {
  }

  /// <summary>
  /// Department
  /// </summary>
  [Serializable]
  public class Department : Department<Department>
  {
    //* 组合关系的从业务对象集合
    /// <summary>
    /// 下级Department
    /// </summary>
    public DepartmentList SubDepartments
    {
      get { return GetCompositionDetail<DepartmentList, Department>(); }
    }
  }

  /// <summary>
  /// Department清单
  /// </summary>
  [Serializable]
  public class DepartmentList : Phenix.Business.BusinessListBase<DepartmentList, Department>
  {
  }

  /// <summary>
  /// Department
  /// </summary>
  [Phenix.Core.Mapping.ClassAttribute("PH_DEPARTMENT", FriendlyName = "Department"), System.ComponentModel.DisplayNameAttribute("Department"), System.SerializableAttribute()]
  public abstract class Department<T> : Phenix.Business.BusinessBase<T> where T : Department<T>
  {
    /// <summary>
    /// DP_ID
    /// </summary>
    public static readonly Phenix.Business.PropertyInfo<long?> DP_IDProperty = RegisterProperty<long?>(c => c.DP_ID);
    [Phenix.Core.Mapping.Field(FriendlyName = "DP_ID", TableName = "PH_DEPARTMENT", ColumnName = "DP_ID", IsPrimaryKey = true, NeedUpdate = true)]
    private long? _DP_ID;
    /// <summary>
    /// DP_ID
    /// </summary>
    [System.ComponentModel.DisplayName("DP_ID")]
    public long? DP_ID
    {
      get { return GetProperty(DP_IDProperty, _DP_ID); }
      internal set
      {
        SetProperty(DP_IDProperty, ref _DP_ID, value);
      }
    }

    [System.ComponentModel.Browsable(false)]
    [System.ComponentModel.DataAnnotations.Display(AutoGenerateField = false)]
    public override string PrimaryKey
    {
      get { return DP_ID.ToString(); }
    }

    /// <summary>
    /// DP_DP_ID
    /// </summary>
    public static readonly Phenix.Business.PropertyInfo<long?> DP_DP_IDProperty = RegisterProperty<long?>(c => c.DP_DP_ID);
    [Phenix.Core.Mapping.Field(FriendlyName = "DP_DP_ID", TableName = "PH_DEPARTMENT", ColumnName = "DP_DP_ID", NeedUpdate = true)]
    //* 因未构建物理外键，需显式申明关联主表
    [Phenix.Core.Mapping.FieldLink("PH_DEPARTMENT", "DP_ID")]
    private long? _DP_DP_ID;
    /// <summary>
    /// DP_DP_ID
    /// </summary>
    [System.ComponentModel.DisplayName("DP_DP_ID")]
    public long? DP_DP_ID
    {
      get { return GetProperty(DP_DP_IDProperty, _DP_DP_ID); }
      set { SetProperty(DP_DP_IDProperty, ref _DP_DP_ID, value); }
    }

    /// <summary>
    /// DP_NAME
    /// </summary>
    public static readonly Phenix.Business.PropertyInfo<string> NameProperty = RegisterProperty<string>(c => c.Name);
    [Phenix.Core.Mapping.Field(FriendlyName = "DP_NAME", Alias = "DP_NAME", TableName = "PH_DEPARTMENT", ColumnName = "DP_NAME", NeedUpdate = true, IsNameColumn = true, InLookUpColumn = true, InLookUpColumnDisplay = true)]
    private string _name;
    /// <summary>
    /// DP_NAME
    /// </summary>
    [System.ComponentModel.DisplayName("DP_NAME")]
    public string Name
    {
      get { return GetProperty(NameProperty, _name); }
      set { SetProperty(NameProperty, ref _name, value); }
    }

    /// <summary>
    /// DP_CODE
    /// </summary>
    public static readonly Phenix.Business.PropertyInfo<string> CodeProperty = RegisterProperty<string>(c => c.Code);
    [Phenix.Core.Mapping.Field(FriendlyName = "DP_CODE", Alias = "DP_CODE", TableName = "PH_DEPARTMENT", ColumnName = "DP_CODE", NeedUpdate = true, IsCodeColumn = true, InLookUpColumn = true, InLookUpColumnSelect = true)]
    private string _code;
    /// <summary>
    /// DP_CODE
    /// </summary>
    [System.ComponentModel.DisplayName("DP_CODE")]
    public string Code
    {
      get { return GetProperty(CodeProperty, _code); }
      set { SetProperty(CodeProperty, ref _code, value); }
    }

    /// <summary>
    /// DP_PT_ID
    /// </summary>
    public static readonly Phenix.Business.PropertyInfo<long?> DP_PT_IDProperty = RegisterProperty<long?>(c => c.DP_PT_ID);
    [Phenix.Core.Mapping.Field(FriendlyName = "DP_PT_ID", TableName = "PH_DEPARTMENT", ColumnName = "DP_PT_ID", NeedUpdate = true)]
    private long? _DP_PT_ID;
    /// <summary>
    /// DP_PT_ID
    /// </summary>
    [System.ComponentModel.DisplayName("DP_PT_ID")]
    public long? DP_PT_ID
    {
      get { return GetProperty(DP_PT_IDProperty, _DP_PT_ID); }
      set { SetProperty(DP_PT_IDProperty, ref _DP_PT_ID, value); }
    }

    /// <summary>
    /// New
    /// </summary>
    public static T New(long? DP_DP_ID, string name, string code, long? DP_PT_ID)
    {
      T result = NewPure();
      result._DP_DP_ID = DP_DP_ID;
      result._name = name;
      result._code = code;
      result._DP_PT_ID = DP_PT_ID;
      return result;
    }

    /// <summary>
    /// SetFieldValues
    /// </summary>
    protected void SetFieldValues(long? DP_DP_ID, string name, string code, long? DP_PT_ID)
    {
      InitOldFieldValues();
      _DP_DP_ID = DP_DP_ID;
      _name = name;
      _code = code;
      _DP_PT_ID = DP_PT_ID;
      MarkDirty();
    }
  }
}
