using System;

namespace Phenix.Security.Business
{
  /// <summary>
  /// 部门
  /// </summary>
  [Serializable]
  [Phenix.Core.Mapping.ReadOnly]
  public class DepartmentReadOnly : Department<DepartmentReadOnly>
  {
  }

  /// <summary>
  /// 部门清单
  /// </summary>
  [Serializable]
  public class DepartmentReadOnlyList : Phenix.Business.BusinessListBase<DepartmentReadOnlyList, DepartmentReadOnly>
  {
  }

  /// <summary>
  /// 部门
  /// </summary>
  [Serializable]
  public class Department : Department<Department>
  {
  }

  /// <summary>
  /// 部门清单
  /// </summary>
  [Serializable]
  public class DepartmentList : Phenix.Business.BusinessListBase<DepartmentList, Department>
  {
  }

  /// <summary>
  /// 部门
  /// </summary>
  [Phenix.Core.Mapping.ClassAttribute("PH_DEPARTMENT", FriendlyName = "部门"), System.SerializableAttribute(), System.ComponentModel.DisplayNameAttribute("部门")]
  [Phenix.Core.Mapping.ClassDetail("PH_USER", "US_DP_ID", null, CascadingDelete = false, FriendlyName = "用户")]
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
    /// 上级部门
    /// </summary>
    public static readonly Phenix.Business.PropertyInfo<long?> DP_DP_IDProperty = RegisterProperty<long?>(c => c.DP_DP_ID);
    [Phenix.Core.Mapping.Field(FriendlyName = "上级部门", TableName = "PH_DEPARTMENT", ColumnName = "DP_DP_ID", NeedUpdate = true)]
    [Phenix.Core.Mapping.FieldLink("PH_DEPARTMENT", "DP_ID")]
    private long? _DP_DP_ID;
    /// <summary>
    /// 上级部门
    /// </summary>
    [System.ComponentModel.DisplayName("上级部门")]
    public long? DP_DP_ID
    {
      get { return GetProperty(DP_DP_IDProperty, _DP_DP_ID); }
      set { SetProperty(DP_DP_IDProperty, ref _DP_DP_ID, value); }
    }

    /// <summary>
    /// 名称
    /// </summary>
    public static readonly Phenix.Business.PropertyInfo<string> NameProperty = RegisterProperty<string>(c => c.Name);
    [Phenix.Core.Mapping.Field(FriendlyName = "名称", Alias = "DP_NAME", TableName = "PH_DEPARTMENT", ColumnName = "DP_NAME", NeedUpdate = true, IsNameColumn = true, InLookUpColumn = true, InLookUpColumnDisplay = true)]
    private string _name;
    /// <summary>
    /// 名称
    /// </summary>
    [System.ComponentModel.DisplayName("名称")]
    public string Name
    {
      get { return GetProperty(NameProperty, _name); }
      set { SetProperty(NameProperty, ref _name, value); }
    }

    /// <summary>
    /// 代码
    /// </summary>
    public static readonly Phenix.Business.PropertyInfo<string> CodeProperty = RegisterProperty<string>(c => c.Code);
    [Phenix.Core.Mapping.Field(FriendlyName = "代码", Alias = "DP_CODE", TableName = "PH_DEPARTMENT", ColumnName = "DP_CODE", NeedUpdate = true, IsCodeColumn = true, InLookUpColumn = true, InLookUpColumnSelect = true)]
    private string _code;
    /// <summary>
    /// 代码
    /// </summary>
    [System.ComponentModel.DisplayName("代码")]
    public string Code
    {
      get { return GetProperty(CodeProperty, _code); }
      set { SetProperty(CodeProperty, ref _code, value); }
    }

    /// <summary>
    /// 岗位树
    /// </summary>
    public static readonly Phenix.Business.PropertyInfo<long?> DP_PT_IDProperty = RegisterProperty<long?>(c => c.DP_PT_ID);
    [Phenix.Core.Mapping.Field(FriendlyName = "岗位树", TableName = "PH_DEPARTMENT", ColumnName = "DP_PT_ID", NeedUpdate = true)]
    [Phenix.Core.Mapping.FieldLink("PH_POSITION", "PT_ID")]
    private long? _DP_PT_ID;
    /// <summary>
    /// 岗位树
    /// </summary>
    [System.ComponentModel.DisplayName("岗位树")]
    public long? DP_PT_ID
    {
      get { return GetProperty(DP_PT_IDProperty, _DP_PT_ID); }
      set { SetProperty(DP_PT_IDProperty, ref _DP_PT_ID, value); }
    }

    /// <summary>
    /// 属于总部
    /// </summary>
    public static readonly Phenix.Business.PropertyInfo<bool?> InHeadquartersProperty = RegisterProperty<bool?>(c => c.InHeadquarters);
    [Phenix.Core.Mapping.Field(FriendlyName = "属于总部", Alias = "DP_In_Headquarters", TableName = "PH_DEPARTMENT", ColumnName = "DP_In_Headquarters", NeedUpdate = true)]
    private bool? _inHeadquarters;
    /// <summary>
    /// 属于总部
    /// </summary>
    [System.ComponentModel.DataAnnotations.Display(Name = "属于总部")]
    [System.ComponentModel.DisplayName("属于总部")]
    public bool? InHeadquarters
    {
      get { return GetProperty(InHeadquartersProperty, _inHeadquarters); }
      set { SetProperty(InHeadquartersProperty, ref _inHeadquarters, value); }
    }

    /// <summary>
    /// New
    /// </summary>
    public static T New(long? DP_DP_ID, string name, string code, long? DP_PT_ID, bool? inHeadquarters)
    {
      T result = NewPure();
      result._DP_DP_ID = DP_DP_ID;
      result._name = name;
      result._code = code;
      result._DP_PT_ID = DP_PT_ID;
      result._inHeadquarters = inHeadquarters;
      return result;
    }
  }
}
