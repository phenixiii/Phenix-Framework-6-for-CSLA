using System;

namespace Phenix.Core
{
  /// <summary>
  /// 要不要…事件数据
  /// Stop+：终止事件流，业务码完全接管事件处理过程及其状态变化
  /// Stop-Applied-：代表框架需继续实施常规的处理过程及其状态变化
  /// Stop-Applied+Succeed+：代表框架不必实施常规的处理过程，但仍处理业务码已成功处理的状态变化
  /// Stop-Applied+Succeed-：代表框架不必实施常规的处理过程，但仍处理业务码未成功处理的状态变化
  /// </summary>
  [Serializable]
  public class ShallEventArgs : StopEventArgs
  {
    /// <summary>
    /// 初始化
    /// </summary>
    public ShallEventArgs()
      : base() { }

    /// <summary>
    /// 初始化
    /// </summary>
    /// <param name="applied">是否已应用</param>
    public ShallEventArgs(bool applied)
      : base()
    {
      _applied = applied;
    }

    #region 属性

    private bool _applied;
    /// <summary>
    /// 是否已应用
    /// 依此判断是否需要实施常规的处理过程
    /// 缺省为 false
    /// </summary>
    public bool Applied
    {
      get { return _applied; }
      set { _applied = value; }
    }

    private bool _succeed = true;
    /// <summary>
    /// 是否已成功
    /// 依此判断是否需要更改相应的成功标志等
    /// 缺省为 true
    /// </summary>
    public bool Succeed
    {
      get { return _succeed; }
      set { _succeed = value; }
    }

    private object _message;
    /// <summary>
    /// 消息对象
    /// </summary>
    public object Message
    {
      get { return _message; }
      set { _message = value; }
    }

    #endregion
  }
}