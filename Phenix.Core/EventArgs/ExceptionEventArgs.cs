using System;

namespace Phenix.Core
{
  /// <summary>
  /// 错误异常事件数据
  /// </summary>
  [Serializable]
  public class ExceptionEventArgs : EventArgs
  {
    /// <summary>
    /// 初始化
    /// </summary>
    /// <param name="sender">发生错误的对象</param>
    /// <param name="error">错误信息</param>
    public ExceptionEventArgs(object sender, Exception error)
      : base()
    {
      _sender = sender;
      _error = error;
    }

    #region 属性

    private readonly object _sender;
    /// <summary>
    /// 发生错误的对象
    /// </summary>
    public object Sender
    {
      get { return _sender; }
    }

    private readonly Exception _error;
    /// <summary>
    /// 错误信息
    /// </summary>
    public Exception Error
    {
      get { return _error; }
    }

    /// <summary>
    /// 是否已应用
    /// 依此判断是否需要继续抛出异常
    /// 缺省为 false
    /// </summary>
    public bool Applied { get; set; }

    /// <summary>
    /// 是否已成功
    /// 依此判断是否已成功处理本异常
    /// 缺省为 false
    /// </summary>
    public bool Succeed { get; set; }

    #endregion
  }
}