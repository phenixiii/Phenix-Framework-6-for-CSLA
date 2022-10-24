using System;

namespace Phenix.Core.Dictionary
{
  /// <summary>
  /// 表过滤器切片资料
  /// </summary>
  [Serializable]
  public sealed class TableFilterSectionInfo
  {
    /// <summary>
    /// 初始化
    /// </summary>
    [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
    public TableFilterSectionInfo(string name, string allowReadColumnValue, bool allowEdit)
    {
      _name = name;
      _allowReadColumnValue = allowReadColumnValue;
      _allowEdit = allowEdit;
    }

    #region 属性

    private readonly string _name;
    /// <summary>
    /// 切片名
    /// </summary>
    public string Name
    {
      get { return _name; }
    }

    private readonly string _allowReadColumnValue;
    /// <summary>
    /// 允许Fetch的记录里用于比较的字段的字段值
    /// </summary>
    public string AllowReadColumnValue
    {
      get { return _allowReadColumnValue; }
    }

    private readonly bool _allowEdit;
    /// <summary>
    /// 是否允许Edit
    /// </summary>
    public bool AllowEdit
    {
      get { return _allowEdit; }
    }

    #endregion
  }
}