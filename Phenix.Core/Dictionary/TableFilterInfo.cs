using System;
using System.Collections.Generic;
using System.Data.Common;
using Phenix.Core.Data;

namespace Phenix.Core.Dictionary
{
  /// <summary>
  /// 表过滤器资料
  /// </summary>
  [Serializable]
  public sealed class TableFilterInfo
  {
    /// <summary>
    /// 初始化
    /// </summary>
    [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
    public TableFilterInfo(string name, string compareColumnName, bool noneSectionIsDeny, 
      IList<TableFilterSectionInfo> sectionInfos)
    {
      _name = name;
      _compareColumnName = compareColumnName;
      _noneSectionIsDeny = noneSectionIsDeny;
      _sectionInfos = sectionInfos;
    }

    #region 属性

    private readonly string _name;
    /// <summary>
    /// 表名
    /// </summary>
    public string Name
    {
      get { return _name; }
    }

    private readonly string _compareColumnName;
    /// <summary>
    /// 用于比较的字段的字段名
    /// </summary>
    public string CompareColumnName
    {
      get { return _compareColumnName; }
    }

    private readonly bool _noneSectionIsDeny;
    /// <summary>
    /// 无切片时认为是被拒绝
    /// </summary>
    public bool NoneSectionIsDeny
    {
      get { return _noneSectionIsDeny; }
    }

    private readonly IList<TableFilterSectionInfo> _sectionInfos;
    /// <summary>
    /// 表过滤器切片资料队列
    /// </summary>
    public IList<TableFilterSectionInfo> SectionInfos
    {
      get { return _sectionInfos; }
    }

    [NonSerialized]
    private Dictionary<string, IList<TableFilterSectionInfo>> _sectionInfosCache;

    #endregion

    #region 方法

    private IList<TableFilterSectionInfo> FilterSectionInfos(string sectionName)
    {
      if (_sectionInfos == null)
        return null;
      if (_sectionInfosCache == null)
        lock (_sectionInfos)
          if (_sectionInfosCache == null)
          {
            _sectionInfosCache = new Dictionary<string, IList<TableFilterSectionInfo>>(StringComparer.Ordinal);
          }
      IList<TableFilterSectionInfo> result;
      if (!_sectionInfosCache.TryGetValue(sectionName, out result))
        lock (_sectionInfosCache)
          if (!_sectionInfosCache.TryGetValue(sectionName, out result))
          {
            List<TableFilterSectionInfo> value = new List<TableFilterSectionInfo>();
            foreach (TableFilterSectionInfo item in _sectionInfos)
              if (String.CompareOrdinal(item.Name, sectionName) == 0)
                value.Add(item);
            result = value.AsReadOnly();
            _sectionInfosCache[sectionName] = result;
          }
      return result;
    }

    internal string FormatExpression(DbCommand command, string expression, Type valueType, IList<string> sectionNames)
    {
      string result = String.Empty;
      if (sectionNames != null)
      {
        foreach (string sectionName in sectionNames)
        {
          IList<TableFilterSectionInfo> sectionInfos = FilterSectionInfos(sectionName);
          if (sectionInfos != null)
            foreach (TableFilterSectionInfo sectionInfo in sectionInfos)
            {
              string parameterName = SqlHelper.UniqueParameterName();
              result = String.Format("{0}{1}=:{2} or ", result, expression, parameterName);
              DbCommandHelper.CreateParameter(command, parameterName, sectionInfo.AllowReadColumnValue, valueType);
            }
        }
        if (!String.IsNullOrEmpty(result))
          return String.Format("({0})", result.Remove(result.TrimEnd().LastIndexOf(' ')));
        else if (NoneSectionIsDeny)
          return String.Format("({0}=null)", expression);
      }
      return result;
    }

    #endregion
  }
}