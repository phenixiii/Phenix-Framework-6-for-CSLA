using System;
using Phenix.Core;

namespace Phenix.Services.Library
{
  /// <summary>
  /// Data提交事件数据
  /// </summary>
  public class SaveEventArgs : ShallEventArgs
  {
    /// <summary>
    /// 初始化
    /// </summary>
    public SaveEventArgs(Type objectType, string jsonSource)
      : base()
    {
      _objectType = objectType;
      _jsonSource = jsonSource;
    }

    /// <summary>
    /// 初始化
    /// </summary>
    public SaveEventArgs(Type objectType, string jsonSource, int result)
      : this(objectType, jsonSource)
    {
      _result = result;
    }

    #region 属性

    private readonly Type _objectType;
    /// <summary>
    /// 数据类
    /// </summary>
    public Type ObjectType
    {
      get { return _objectType; }
    }

    private readonly string _jsonSource;
    /// <summary>
    /// JSON格式数据源
    /// </summary>
    public string JsonSource
    {
      get { return _jsonSource; }
    }

    private int _result;
    /// <summary>
    /// 结果
    /// </summary>
    public int Result
    {
      get { return _result; }
      set
      {
        _result = value;
        Applied = true;
      }
    }

    #endregion
  }
}