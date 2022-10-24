using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Phenix.Core.Dictionary;

namespace Phenix.Core.Security
{
  /// <summary>
  /// 岗位资料
  /// </summary>
  [Serializable]
  public sealed class PositionInfo
  {
    [Newtonsoft.Json.JsonConstructor]
    private PositionInfo(long id, string name, string code)
    {
      _id = id;
      _name = name;
      _code = code;
    }

    /// <summary>
    /// 初始化
    /// </summary>
    [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
    public PositionInfo(long id, string name, string code, long? superiorId)
      : this(id, name, code)
    {
      _superiorId = superiorId;
    }

    #region 属性

    private readonly long _id;
    /// <summary>
    /// ID
    /// </summary>
    public long Id
    {
      get { return _id; }
    }

    private readonly string _name;
    /// <summary>
    /// 名称
    /// </summary>
    public string Name
    {
      get { return _name; }
    }

    private readonly string _code;
    /// <summary>
    /// 代码
    /// </summary>
    public string Code
    {
      get { return _code; }
    }

    private long? _superiorId;
    [NonSerialized]
    private PositionInfo _superior;
    /// <summary>
    /// 上级(一级)岗位
    /// </summary>
    [Newtonsoft.Json.JsonIgnore]
    public PositionInfo Superior
    {
      get
      {
        if (_superior == null && _superiorId.HasValue)
          DataDictionaryHub.PositionInfos.TryGetValue(_superiorId.Value, out _superior);
        return _superior;
      }
    }

    [NonSerialized]
    private ReadOnlyCollection<PositionInfo> _allSuperior;
    /// <summary>
    /// 上级(全部)岗位
    /// </summary>
    [Newtonsoft.Json.JsonIgnore]
    public IList<PositionInfo> AllSuperior
    {
      get
      {
        if (_allSuperior == null)
        {
          List<PositionInfo> result = new List<PositionInfo>();
          PositionInfo superior = Superior;
          while (superior != null)
          {
            result.Add(superior);
            superior = superior.Superior;
            if (result.Contains(superior))
              break;
          }
          _allSuperior = result.AsReadOnly();
        }
        return _allSuperior;
      }
    }

    [NonSerialized]
    private ReadOnlyCollection<PositionInfo> _subordinates;
    /// <summary>
    /// 下级(一级)岗位
    /// </summary>
    [Newtonsoft.Json.JsonIgnore]
    public IList<PositionInfo> Subordinates
    {
      get
      {
        if (_subordinates == null)
        {
          List<PositionInfo> result = new List<PositionInfo>();
          foreach (KeyValuePair<long, PositionInfo> kvp in DataDictionaryHub.PositionInfos)
            if (kvp.Value._superiorId == _id)
              result.Add(kvp.Value);
          _subordinates = result.AsReadOnly();
        }
        return _subordinates;
      }
    }

    [NonSerialized]
    private ReadOnlyCollection<PositionInfo> _allSubordinates;
    /// <summary>
    /// 下级(全部)岗位
    /// </summary>
    [Newtonsoft.Json.JsonIgnore]
    public IList<PositionInfo> AllSubordinates
    {
      get
      {
        if (_allSubordinates == null)
        {
          List<PositionInfo> result = new List<PositionInfo>(Subordinates);
          foreach (PositionInfo item in Subordinates)
            result.AddRange(item.AllSubordinates);
          _allSubordinates = result.AsReadOnly();
        }
        return _allSubordinates;
      }
    }

    #endregion
  }
}