using System;

namespace Phenix.Security.Business
{
  /// <summary>
  /// 切片
  /// </summary>
  [Serializable]
  [Phenix.Core.Mapping.ReadOnly]
  public class SectionReadOnly : Section<SectionReadOnly>
  {
  }

  /// <summary>
  /// 切片清单
  /// </summary>
  [Serializable]
  public class SectionReadOnlyList : Phenix.Business.BusinessListBase<SectionReadOnlyList, SectionReadOnly>
  {
  }

  /// <summary>
  /// 切片
  /// </summary>
  [Serializable]
  public class Section : Section<Section>
  {
    #region 勾选表过滤器全集

    /// <summary>
    /// 分配的表过滤器
    /// </summary>
    public SectionTableFilterList SectionTableFilters
    {
      get { return GetCompositionDetail<SectionTableFilterList, SectionTableFilter>(SectionTableFilterList.Fetch()); }
    }

    /// <summary>
    /// 供勾选的表过滤器全集
    /// </summary>
    public SelectableTableFilterForSectionList SelectableTableFilters
    {
      get { return SectionTableFilters.CollatingSelectableList<SelectableTableFilterForSectionList, SelectableTableFilterForSection>(SelectableTableFilterForSectionList.Fetch()); }
    }

    #endregion
  }

  /// <summary>
  /// 切片清单
  /// </summary>
  [Serializable]
  public class SectionList : Phenix.Business.BusinessListBase<SectionList, Section>
  {
  }

  /// <summary>
  /// 切片
  /// </summary>
  [Phenix.Core.Mapping.ClassAttribute("PH_SECTION", FriendlyName = "切片"), System.SerializableAttribute(), System.ComponentModel.DisplayNameAttribute("切片")]
  [Phenix.Core.Mapping.ClassDetail("PH_SECTION_TABLEFILTER", "ST_ST_ID", null, CascadingDelete = true, FriendlyName = "切片-表过滤器")]
  [Phenix.Core.Mapping.ClassDetail("PH_USER_SECTION", "US_ST_ID", null, CascadingDelete = true, FriendlyName = "用户-切片")]
  public abstract class Section<T> : Phenix.Business.BusinessBase<T> where T : Section<T>
  {
    /// <summary>
    /// ST_ID
    /// </summary>
    public static readonly Phenix.Business.PropertyInfo<long?> ST_IDProperty = RegisterProperty<long?>(c => c.ST_ID);
    [Phenix.Core.Mapping.Field(FriendlyName = "ST_ID", TableName = "PH_SECTION", ColumnName = "ST_ID", IsPrimaryKey = true, NeedUpdate = true)]
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
    /// 名称
    /// </summary>
    public static readonly Phenix.Business.PropertyInfo<string> NameProperty = RegisterProperty<string>(c => c.Name);
    [Phenix.Core.Mapping.Field(FriendlyName = "名称", Alias = "ST_NAME", TableName = "PH_SECTION", ColumnName = "ST_NAME", NeedUpdate = true, IsNameColumn = true, InLookUpColumn = true, InLookUpColumnDisplay = true)]
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
    /// 标签
    /// </summary>
    public static readonly Phenix.Business.PropertyInfo<string> CaptionProperty = RegisterProperty<string>(c => c.Caption);
    [Phenix.Core.Mapping.Field(FriendlyName = "标签", Alias = "ST_CAPTION", TableName = "PH_SECTION", ColumnName = "ST_CAPTION", NeedUpdate = true)]
    private string _caption;
    /// <summary>
    /// 标签
    /// </summary>
    [System.ComponentModel.DisplayName("标签")]
    public new string Caption
    {
      get { return GetProperty(CaptionProperty, _caption); }
      set { SetProperty(CaptionProperty, ref _caption, value); }
    }
  }
}
