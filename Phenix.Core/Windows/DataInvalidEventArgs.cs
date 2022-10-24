using System;
using System.Windows.Forms;

namespace Phenix.Core.Windows
{
  /// <summary>
  /// 失效数据事件数据
  /// </summary>
  public sealed class DataInvalidEventArgs : EventArgs
  {
    /// <summary>
    /// 初始化
    /// </summary>
    /// <param name="source">失效数据所在的数据源</param>
    /// <param name="position">失效数据所在的数据源游标</param>
    /// <param name="control">失效数据所在的控件</param>
    /// <param name="message">提示信息</param>
    public DataInvalidEventArgs(BindingSource source, int position, Control control, string message)
      : base()
    {
      _source = source;
      _position = position;
      _control = control;
      _message = message;
    }

    #region 属性

    private readonly BindingSource _source;
    /// <summary>
    /// 失效数据所在的数据源
    /// </summary>
    public BindingSource Source
    {
      get { return _source; }
    }

    private readonly int _position;
    /// <summary>
    /// 失效数据所在数据源的游标
    /// </summary>
    public int Position
    {
      get { return _position; }
    }

    private readonly Control _control;
    /// <summary>
    /// 失效数据所在的控件
    /// </summary>
    public Control Control
    {
      get { return _control; }
    }

    private readonly string _message;
    /// <summary>
    /// 提示信息
    /// </summary>
    public string Message
    {
      get { return _message; }
    }

    #endregion
  }
}