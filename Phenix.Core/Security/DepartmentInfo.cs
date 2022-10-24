using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Phenix.Core.Dictionary;

namespace Phenix.Core.Security
{
  /// <summary>
  /// 部门资料
  /// </summary>
  [Serializable]
  public sealed class DepartmentInfo
  {
    [Newtonsoft.Json.JsonConstructor]
    private DepartmentInfo(long id, string name, string code, bool? inHeadquarters)
    {
      _id = id;
      _name = name;
      _code = code;
      _inHeadquarters = inHeadquarters;
    }

    /// <summary>
    /// 初始化
    /// </summary>
    [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
    public DepartmentInfo(long id, string name, string code, long? superiorId, long? positionTreeId, bool? inHeadquarters)
      : this(id, name, code, inHeadquarters)
    {
      _superiorId = superiorId;
      _positionTreeId = positionTreeId;
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
    private DepartmentInfo _superior;
    /// <summary>
    /// 上级(一级)部门
    /// </summary>
    [Newtonsoft.Json.JsonIgnore]
    public DepartmentInfo Superior
    {
      get
      {
        if (_superior == null && _superiorId.HasValue)
          DataDictionaryHub.DepartmentInfos.TryGetValue(_superiorId.Value, out _superior);
        return _superior;
      }
    }

    [NonSerialized]
    private DepartmentInfo _rootSuperior;
    /// <summary>
    /// 顶级部门
    /// </summary>
    [Newtonsoft.Json.JsonIgnore]
    public DepartmentInfo RootSuperior
    {
      get
      {
        if (_rootSuperior == null)
        {
          DepartmentInfo result = Superior;
          if (result != null)
          {
            while (true)
            {
              if (result.Superior == null || result.Superior == result)
                break;
              result = result.Superior;
            }
            _rootSuperior = result;
          }
        }
        return _rootSuperior;
      }
    }

    [NonSerialized]
    private ReadOnlyCollection<DepartmentInfo> _allSuperior;
    /// <summary>
    /// 上级(全部)部门
    /// </summary>
    [Newtonsoft.Json.JsonIgnore]
    public IList<DepartmentInfo> AllSuperior
    {
      get
      {
        if (_allSuperior == null)
        {
          List<DepartmentInfo> result = new List<DepartmentInfo>();
          DepartmentInfo superior = Superior;
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
    private ReadOnlyCollection<DepartmentInfo> _subordinates;
    /// <summary>
    /// 下级(一级)部门
    /// </summary>
    [Newtonsoft.Json.JsonIgnore]
    public IList<DepartmentInfo> Subordinates
    {
      get
      {
        if (_subordinates == null)
        {
          List<DepartmentInfo> result = new List<DepartmentInfo>();
          foreach (KeyValuePair<long, DepartmentInfo> kvp in DataDictionaryHub.DepartmentInfos)
            if (kvp.Value._superiorId == _id)
              result.Add(kvp.Value);
          _subordinates = result.AsReadOnly();
        }
        return _subordinates;
      }
    }

    [NonSerialized]
    private ReadOnlyCollection<DepartmentInfo> _allSubordinates;
    /// <summary>
    /// 下级(全部)部门
    /// </summary>
    [Newtonsoft.Json.JsonIgnore]
    public IList<DepartmentInfo> AllSubordinates
    {
      get
      {
        if (_allSubordinates == null)
        {
          List<DepartmentInfo> result = new List<DepartmentInfo>(Subordinates);
          foreach (DepartmentInfo item in Subordinates)
            result.AddRange(item.AllSubordinates);
          _allSubordinates = result.AsReadOnly();
        }
        return _allSubordinates;
      }
    }

    private long? _positionTreeId;
    [NonSerialized]
    private PositionInfo _positionTree;
    /// <summary>
    /// 岗位树
    /// </summary>
    [Newtonsoft.Json.JsonIgnore]
    public PositionInfo PositionTree
    {
      get
      {
        if (_positionTree == null && _positionTreeId.HasValue)
          DataDictionaryHub.PositionInfos.TryGetValue(_positionTreeId.Value, out _positionTree);
        return _positionTree;
      }
    }

    [NonSerialized]
    private ReadOnlyCollection<PositionInfo> _allPositions;
    /// <summary>
    /// 全部岗位
    /// </summary>
    [Newtonsoft.Json.JsonIgnore]
    public IList<PositionInfo> AllPositions
    {
      get
      {
        if (_allPositions == null)
        {
          List<PositionInfo> result = new List<PositionInfo>();
          if (PositionTree != null)
          {
            result.Add(PositionTree);
            result.AddRange(PositionTree.AllSubordinates);
          }
          _allPositions = result.AsReadOnly();
        }
        return _allPositions;
      }
    }

    private readonly bool? _inHeadquarters;
    /// <summary>
    /// 是否属于总部
    /// </summary>
    public bool? InHeadquarters
    {
      get { return _inHeadquarters; }
    }

    #endregion
  }
}