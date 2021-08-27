using Phenix.Core;
using Phenix.Core.Mapping;

namespace Phenix.Services.Library
{
  /// <summary>
  /// Data检索事件数据
  /// </summary>
  public class FetchEventArgs : ShallEventArgs
  {
    /// <summary>
    /// 初始化
    /// </summary>
    public FetchEventArgs(ICriterions criterions)
      : base()
    {
      _criterions = criterions;
    }

    /// <summary>
    /// 初始化
    /// </summary>
    public FetchEventArgs(ICriterions criterions, string result)
      : this(criterions)
    {
      _result = result;
    }

    #region 属性

    private readonly ICriterions _criterions;
    /// <summary>
    /// 条件集
    /// </summary>
    public ICriterions Criterions
    {
      get { return _criterions; }
    }

    private string _result;
    /// <summary>
    /// 结果
    /// </summary>
    public string Result
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