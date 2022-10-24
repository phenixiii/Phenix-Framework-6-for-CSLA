using System;
using System.Collections.Generic;
using Phenix.Core.Mapping;
using Phenix.Core.SyncCollections;

namespace Phenix.Core.Dictionary
{
  /// <summary>
  /// 切片资料
  /// </summary>
  [Serializable]
  public sealed class SectionInfo
  {
    /// <summary>
    /// 初始化
    /// </summary>
    [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
    [Newtonsoft.Json.JsonConstructor]
    public SectionInfo(string name, string caption)
    {
      _name = name;
      _caption = caption;
    }

    #region 属性

    private readonly string _name;
    /// <summary>
    /// 名称
    /// </summary>
    public string Name
    {
      get { return _name; }
    }

    private readonly string _caption;
    /// <summary>
    /// 标签
    /// </summary>
    public string Caption
    {
      get { return _caption; }
    }

    private static readonly SynchronizedDictionary<string, IList<string>> _sectionNamesCache =
      new SynchronizedDictionary<string, IList<string>>(StringComparer.Ordinal);

    #endregion

    #region 方法

    internal static void ClearCache()
    {
      _sectionNamesCache.Clear();
    }

    /// <summary>
    /// 取类所关联切片
    /// </summary>
    /// <param name="objectType">类</param>
    public static IList<string> GetSectionNames(Type objectType)
    {
      if (objectType == null)
        throw new ArgumentNullException("objectType");
      return _sectionNamesCache.GetValue(objectType.FullName, () =>
      {
        List<string> value = new List<string>();
        foreach (FieldMapInfo fieldMapInfo in ClassMemberHelper.DoGetFieldMapInfos(objectType))
        {
          TableFilterInfo tableFilterInfo = fieldMapInfo.TableFilterInfo;
          if (tableFilterInfo != null && tableFilterInfo.SectionInfos != null)
            foreach (TableFilterSectionInfo sectionInfo in tableFilterInfo.SectionInfos)
              if (!value.Contains(sectionInfo.Name))
                value.Add(sectionInfo.Name);
        }
        return value.AsReadOnly();
      }, false);
    }

    #endregion
  }
}