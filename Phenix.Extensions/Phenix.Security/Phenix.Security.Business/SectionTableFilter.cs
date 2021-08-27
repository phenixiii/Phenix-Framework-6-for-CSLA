using System;

namespace Phenix.Security.Business
{
  #region SelectableTableFilterForSection

  /// <summary>
  /// 供切片勾选的表过滤器清单
  /// </summary>
  [Serializable]
  public class SelectableTableFilterForSectionList : Phenix.Business.BusinessListBase<SelectableTableFilterForSectionList, SelectableTableFilterForSection>
  {
  }

  /// <summary>
  /// 供切片勾选的表过滤器
  /// </summary>
  [Serializable]
  public class SelectableTableFilterForSection : TableFilter<SelectableTableFilterForSection>
  {
    #region 需覆写SectionTableFilter的属性: NoMapping = true, IsRedundanceColumn = true

    /// <summary>
    /// 用于显示的字段值
    /// </summary>
    public static readonly Phenix.Business.PropertyInfo<string> FriendlyColumnValueProperty = RegisterProperty<string>(c => c.FriendlyColumnValue);
    [Phenix.Core.Mapping.Field(FriendlyName = "用于显示的字段值", Alias = "ST_FRIENDLY_COLUMNVALUE", TableName = "PH_SECTION_TABLEFILTER", ColumnName = "ST_FRIENDLY_COLUMNVALUE", NoMapping = true, IsRedundanceColumn = true)]
    private string _friendlyColumnValue;
    /// <summary>
    /// 用于显示的字段值
    /// </summary>
    [System.ComponentModel.DisplayName("用于显示的字段值")]
    public string FriendlyColumnValue
    {
      get { return GetProperty(FriendlyColumnValueProperty, _friendlyColumnValue); }
      set { SetProperty(FriendlyColumnValueProperty, ref _friendlyColumnValue, value); }
    }

    /// <summary>
    /// 用于比较的字段值
    /// </summary>
    public static readonly Phenix.Business.PropertyInfo<string> AllowReadColumnValueProperty = RegisterProperty<string>(c => c.AllowReadColumnValue);
    [Phenix.Core.Mapping.Field(FriendlyName = "用于比较的字段值", Alias = "ST_ALLOWREAD_COLUMNVALUE", TableName = "PH_SECTION_TABLEFILTER", ColumnName = "ST_ALLOWREAD_COLUMNVALUE", NoMapping = true, IsRedundanceColumn = true)]
    private string _allowReadColumnValue;
    /// <summary>
    /// 用于比较的字段值
    /// </summary>
    [System.ComponentModel.DisplayName("用于比较的字段值")]
    public string AllowReadColumnValue
    {
      get { return GetProperty(AllowReadColumnValueProperty, _allowReadColumnValue); }
      set { SetProperty(AllowReadColumnValueProperty, ref _allowReadColumnValue, value); }
    }

    /// <summary>
    /// 是否允许编辑
    /// </summary>
    public static readonly Phenix.Business.PropertyInfo<bool?> AllowEditProperty = RegisterProperty<bool?>(c => c.AllowEdit_);
    [Phenix.Core.Mapping.Field(FriendlyName = "是否允许编辑", Alias = "ST_ALLOWEDIT", TableName = "PH_SECTION_TABLEFILTER", ColumnName = "ST_ALLOWEDIT", NoMapping = true, IsRedundanceColumn = true)]
    private bool? _allowEdit_;
    /// <summary>
    /// 是否允许编辑
    /// </summary>
    [System.ComponentModel.DisplayName("是否允许编辑")]
    public bool? AllowEdit_
    {
      get { return GetProperty(AllowEditProperty, _allowEdit_); }
      set { SetProperty(AllowEditProperty, ref _allowEdit_, value); }
    }

    #endregion

    #region 方法

    /// <summary>
    /// 属性可写
    /// </summary>
    /// <param name="property">属性信息</param>
    /// <returns>属性可写</returns>
    public override bool CanWriteProperty(Csla.Core.IPropertyInfo property)
    {
      //非“需覆写SectionTableFilter的属性”一概不允许写
      if (property != null)
        if (!property.Equals(FriendlyColumnValueProperty) &&
          !property.Equals(AllowReadColumnValueProperty) &&
          !property.Equals(AllowEditProperty))
          return false;
      return base.CanWriteProperty(property);
    }

    #endregion
  }

  #endregion

  /// <summary>
  /// 切片-表过滤器
  /// </summary>
  [Serializable]
  [Phenix.Core.Mapping.ReadOnly]
  public class SectionTableFilterReadOnly : SectionTableFilter<SectionTableFilterReadOnly>
  {
  }

  /// <summary>
  /// 切片-表过滤器清单
  /// </summary>
  [Serializable]
  public class SectionTableFilterReadOnlyList : Phenix.Business.BusinessListBase<SectionTableFilterReadOnlyList, SectionTableFilterReadOnly>
  {
  }

  /// <summary>
  /// 切片-表过滤器
  /// </summary>
  [Serializable]
  public class SectionTableFilter : SectionTableFilter<SectionTableFilter>
  {
  }

  /// <summary>
  /// 切片-表过滤器清单
  /// </summary>
  [Serializable]
  public class SectionTableFilterList : Phenix.Business.BusinessListBase<SectionTableFilterList, SectionTableFilter>
  {
  }

  /// <summary>
  /// 切片-表过滤器
  /// </summary>
  [Phenix.Core.Mapping.ClassAttribute("PH_SECTION_TABLEFILTER", FriendlyName = "切片-表过滤器"), System.SerializableAttribute(), System.ComponentModel.DisplayNameAttribute("切片-表过滤器")]
  public abstract class SectionTableFilter<T> : Phenix.Business.BusinessBase<T> where T : SectionTableFilter<T>
  {
    /// <summary>
    /// ST_ID
    /// </summary>
    public static readonly Phenix.Business.PropertyInfo<long?> ST_IDProperty = RegisterProperty<long?>(c => c.ST_ID);
    [Phenix.Core.Mapping.Field(FriendlyName = "ST_ID", TableName = "PH_SECTION_TABLEFILTER", ColumnName = "ST_ID", IsPrimaryKey = true, NeedUpdate = true)]
    private long? _ST_ID;
    /// <summary>
    /// ST_ID
    /// </summary>
    [System.ComponentModel.DisplayName("ST_ID")]
    public long? ST_ID
    {
      get { return GetProperty(ST_IDProperty, _ST_ID); }
      internal set
      {
        SetProperty(ST_IDProperty, ref _ST_ID, value);
      }
    }

    [System.ComponentModel.Browsable(false)]
    [System.ComponentModel.DataAnnotations.Display(AutoGenerateField = false)]
    public override string PrimaryKey
    {
      get { return ST_ID.ToString(); }
    }

    /// <summary>
    /// 所属切片
    /// </summary>
    public static readonly Phenix.Business.PropertyInfo<long?> ST_ST_IDProperty = RegisterProperty<long?>(c => c.ST_ST_ID);
    [Phenix.Core.Mapping.Field(FriendlyName = "所属切片", TableName = "PH_SECTION_TABLEFILTER", ColumnName = "ST_ST_ID", NeedUpdate = true)]
    [Phenix.Core.Mapping.FieldLink("PH_SECTION", "ST_ID")]
    private long? _ST_ST_ID;
    /// <summary>
    /// 所属切片
    /// </summary>
    [System.ComponentModel.DisplayName("所属切片")]
    public long? ST_ST_ID
    {
      get { return GetProperty(ST_ST_IDProperty, _ST_ST_ID); }
      set { SetProperty(ST_ST_IDProperty, ref _ST_ST_ID, value); }
    }

    /// <summary>
    /// 所属表过滤器
    /// </summary>
    public static readonly Phenix.Business.PropertyInfo<long?> ST_TF_IDProperty = RegisterProperty<long?>(c => c.ST_TF_ID);
    [Phenix.Core.Mapping.Field(FriendlyName = "所属表过滤器", TableName = "PH_SECTION_TABLEFILTER", ColumnName = "ST_TF_ID", NeedUpdate = true)]
    [Phenix.Core.Mapping.FieldLink("PH_TABLEFILTER", "TF_ID")]
    private long? _ST_TF_ID;
    /// <summary>
    /// 所属表过滤器
    /// </summary>
    [System.ComponentModel.DisplayName("所属表过滤器")]
    public long? ST_TF_ID
    {
      get { return GetProperty(ST_TF_IDProperty, _ST_TF_ID); }
      set { SetProperty(ST_TF_IDProperty, ref _ST_TF_ID, value); }
    }

    /// <summary>
    /// 用于显示的字段值
    /// </summary>
    public static readonly Phenix.Business.PropertyInfo<string> FriendlyColumnValueProperty = RegisterProperty<string>(c => c.FriendlyColumnValue);
    [Phenix.Core.Mapping.Field(FriendlyName = "用于显示的字段值", Alias = "ST_FRIENDLY_COLUMNVALUE", TableName = "PH_SECTION_TABLEFILTER", ColumnName = "ST_FRIENDLY_COLUMNVALUE", NeedUpdate = true)]
    private string _friendlyColumnValue;
    /// <summary>
    /// 用于显示的字段值
    /// </summary>
    [System.ComponentModel.DisplayName("用于显示的字段值")]
    public string FriendlyColumnValue
    {
      get { return GetProperty(FriendlyColumnValueProperty, _friendlyColumnValue); }
      set { SetProperty(FriendlyColumnValueProperty, ref _friendlyColumnValue, value); }
    }

    /// <summary>
    /// 用于比较的字段值
    /// </summary>
    public static readonly Phenix.Business.PropertyInfo<string> AllowReadColumnValueProperty = RegisterProperty<string>(c => c.AllowReadColumnValue);
    [Phenix.Core.Mapping.Field(FriendlyName = "用于比较的字段值", Alias = "ST_ALLOWREAD_COLUMNVALUE", TableName = "PH_SECTION_TABLEFILTER", ColumnName = "ST_ALLOWREAD_COLUMNVALUE", NeedUpdate = true)]
    private string _allowReadColumnValue;
    /// <summary>
    /// 用于比较的字段值
    /// </summary>
    [System.ComponentModel.DisplayName("用于比较的字段值")]
    public string AllowReadColumnValue
    {
      get { return GetProperty(AllowReadColumnValueProperty, _allowReadColumnValue); }
      set { SetProperty(AllowReadColumnValueProperty, ref _allowReadColumnValue, value); }
    }

    /// <summary>
    /// 是否允许编辑
    /// </summary>
    public static readonly Phenix.Business.PropertyInfo<bool?> AllowEditProperty = RegisterProperty<bool?>(c => c.AllowEdit_);
    [Phenix.Core.Mapping.Field(FriendlyName = "是否允许编辑", Alias = "ST_ALLOWEDIT", TableName = "PH_SECTION_TABLEFILTER", ColumnName = "ST_ALLOWEDIT", NeedUpdate = true)]
    private bool? _allowEdit_;
    /// <summary>
    /// 是否允许编辑
    /// </summary>
    [System.ComponentModel.DisplayName("是否允许编辑")]
    public bool? AllowEdit_
    {
      get { return GetProperty(AllowEditProperty, _allowEdit_); }
      set { SetProperty(AllowEditProperty, ref _allowEdit_, value); }
    }
  }
}
