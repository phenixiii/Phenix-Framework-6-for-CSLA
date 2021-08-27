using System;
using System.Collections.Generic;

namespace Phenix.Security.Business
{
  /// <summary>
  /// 表过滤器
  /// </summary>
  [Serializable]
  [Phenix.Core.Mapping.ReadOnly]
  public class TableFilterReadOnly : TableFilter<TableFilterReadOnly>
  {
  }

  /// <summary>
  /// 表过滤器清单
  /// </summary>
  [Serializable]
  public class TableFilterReadOnlyList : Phenix.Business.BusinessListBase<TableFilterReadOnlyList, TableFilterReadOnly>
  {
  }

  /// <summary>
  /// 表过滤器
  /// </summary>
  [Serializable]
  public class TableFilter : TableFilter<TableFilter>
  {
    #region 关联表及字段

    private Phenix.Core.Data.Schema.Database Database
    {
      get { return Phenix.Core.Data.Schema.Database.Fetch(); }
    }

    /// <summary>
    /// 数据库表清单
    /// </summary>
    public IList<Phenix.Core.Data.Schema.Table> DatabaseTables
    {
      get { return new List<Phenix.Core.Data.Schema.Table>(Database.Tables.Values).AsReadOnly(); }
    }

    /// <summary>
    /// 表
    /// </summary>
    public Phenix.Core.Data.Schema.Table Table
    {
      get { return Database.FindTable(Name); }
    }

    /// <summary>
    /// 表字段清单
    /// </summary>
    public IList<Phenix.Core.Data.Schema.Column> TableColumns
    {
      get
      {
        return Table != null
          ? new List<Phenix.Core.Data.Schema.Column>(Table.Columns.Values).AsReadOnly()
          : new List<Phenix.Core.Data.Schema.Column>(0).AsReadOnly();
      }
    }

    #endregion

    #region 勾选切片全集

    /// <summary>
    /// 分配的切片
    /// </summary>
    public SectionTableFilterList SectionTableFilters
    {
      get { return GetCompositionDetail<SectionTableFilterList, SectionTableFilter>(SectionTableFilterList.Fetch()); }
    }

    /// <summary>
    /// 供勾选的切片全集
    /// </summary>
    public SectionReadOnlyList SelectableSections
    {
      get { return SectionTableFilters.CollatingSelectableList<SectionReadOnlyList, SectionReadOnly>(SectionReadOnlyList.Fetch()); }
    }

    #endregion
    
    #region 覆写属性

    /// <summary>
    /// 表名
    /// </summary>
    public new string Name
    {
      get { return base.Name; }
      set
      {
        base.Name = value;
        if (Table != null)
          Caption = Table.Description;
      }
    }

    #endregion
  }

  /// <summary>
  /// 表过滤器清单
  /// </summary>
  [Serializable]
  public class TableFilterList : Phenix.Business.BusinessListBase<TableFilterList, TableFilter>
  {
  }

  /// <summary>
  /// 表过滤器
  /// </summary>
  [Phenix.Core.Mapping.ClassAttribute("PH_TABLEFILTER", FriendlyName = "表过滤器"), System.SerializableAttribute(), System.ComponentModel.DisplayNameAttribute("表过滤器")]
  [Phenix.Core.Mapping.ClassDetail("PH_SECTION_TABLEFILTER", "ST_TF_ID", null, CascadingDelete = true, FriendlyName = "切片-表过滤器")]
  public abstract class TableFilter<T> : Phenix.Business.BusinessBase<T> where T : TableFilter<T>
  {
    /// <summary>
    /// TF_ID
    /// </summary>
    public static readonly Phenix.Business.PropertyInfo<long?> TF_IDProperty = RegisterProperty<long?>(c => c.TF_ID);
    [Phenix.Core.Mapping.Field(FriendlyName = "TF_ID", TableName = "PH_TABLEFILTER", ColumnName = "TF_ID", IsPrimaryKey = true, NeedUpdate = true)]
    private long? _TF_ID;
    /// <summary>
    /// TF_ID
    /// </summary>
    [System.ComponentModel.DisplayName("TF_ID")]
    public long? TF_ID
    {
      get { return GetProperty(TF_IDProperty, _TF_ID); }
      internal set
      {
        SetProperty(TF_IDProperty, ref _TF_ID, value);
      }
    }

    [System.ComponentModel.Browsable(false)]
    [System.ComponentModel.DataAnnotations.Display(AutoGenerateField = false)]
    public override string PrimaryKey
    {
      get { return TF_ID.ToString(); }
    }

    /// <summary>
    /// 表名
    /// </summary>
    public static readonly Phenix.Business.PropertyInfo<string> NameProperty = RegisterProperty<string>(c => c.Name);
    [Phenix.Core.Mapping.Field(FriendlyName = "表名", Alias = "TF_NAME", TableName = "PH_TABLEFILTER", ColumnName = "TF_NAME", NeedUpdate = true, IsNameColumn = true, InLookUpColumn = true, InLookUpColumnDisplay = true)]
    private string _name;
    /// <summary>
    /// 表名
    /// </summary>
    [System.ComponentModel.DisplayName("表名")]
    public string Name
    {
      get { return GetProperty(NameProperty, _name); }
      set { SetProperty(NameProperty, ref _name, value); }
    }

    /// <summary>
    /// 标签
    /// </summary>
    public static readonly Phenix.Business.PropertyInfo<string> CaptionProperty = RegisterProperty<string>(c => c.Caption);
    [Phenix.Core.Mapping.Field(FriendlyName = "标签", Alias = "TF_CAPTION", TableName = "PH_TABLEFILTER", ColumnName = "TF_CAPTION", NeedUpdate = true)]
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

    /// <summary>
    /// 用于比较的字段
    /// </summary>
    public static readonly Phenix.Business.PropertyInfo<string> CompareColumnNameProperty = RegisterProperty<string>(c => c.CompareColumnName);
    [Phenix.Core.Mapping.Field(FriendlyName = "用于比较的字段", Alias = "TF_COMPARE_COLUMNNAME", TableName = "PH_TABLEFILTER", ColumnName = "TF_COMPARE_COLUMNNAME", NeedUpdate = true, IsNameColumn = true, InLookUpColumn = true, InLookUpColumnDisplay = true)]
    private string _compareColumnName;
    /// <summary>
    /// 用于比较的字段
    /// </summary>
    [System.ComponentModel.DisplayName("用于比较的字段")]
    public string CompareColumnName
    {
      get { return GetProperty(CompareColumnNameProperty, _compareColumnName); }
      set { SetProperty(CompareColumnNameProperty, ref _compareColumnName, value); }
    }

    /// <summary>
    /// 用于显示的字段
    /// </summary>
    public static readonly Phenix.Business.PropertyInfo<string> FriendlyColumnNameProperty = RegisterProperty<string>(c => c.FriendlyColumnName);
    [Phenix.Core.Mapping.Field(FriendlyName = "用于显示的字段", Alias = "TF_FRIENDLY_COLUMNNAME", TableName = "PH_TABLEFILTER", ColumnName = "TF_FRIENDLY_COLUMNNAME", NeedUpdate = true, IsNameColumn = true, InLookUpColumn = true, InLookUpColumnDisplay = true)]
    private string _friendlyColumnName;
    /// <summary>
    /// 用于显示的字段
    /// </summary>
    [System.ComponentModel.DisplayName("用于显示的字段")]
    public string FriendlyColumnName
    {
      get { return GetProperty(FriendlyColumnNameProperty, _friendlyColumnName); }
      set { SetProperty(FriendlyColumnNameProperty, ref _friendlyColumnName, value); }
    }

    /// <summary>
    /// 无切片时认为是被拒绝
    /// </summary>
    public static readonly Phenix.Business.PropertyInfo<bool?> NoneSectionIsDenyProperty = RegisterProperty<bool?>(c => c.NoneSectionIsDeny);
    [Phenix.Core.Mapping.Field(FriendlyName = "无切片时认为是被拒绝", Alias = "TF_NONESECTIONISDENY", TableName = "PH_TABLEFILTER", ColumnName = "TF_NONESECTIONISDENY", NeedUpdate = true)]
    private bool? _noneSectionIsDeny;
    /// <summary>
    /// 无切片时认为是被拒绝
    /// </summary>
    [System.ComponentModel.DisplayName("无切片时认为是被拒绝")]
    public bool? NoneSectionIsDeny
    {
      get { return GetProperty(NoneSectionIsDenyProperty, _noneSectionIsDeny); }
      set { SetProperty(NoneSectionIsDenyProperty, ref _noneSectionIsDeny, value); }
    }
  }
}
