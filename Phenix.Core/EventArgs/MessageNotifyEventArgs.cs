using System;
using System.Drawing;

namespace Phenix.Core
{
  /// <summary>
  /// 消息通报事件数据
  /// </summary>
  [Serializable]
  public class MessageNotifyEventArgs : EventArgs
  {
    /// <summary>
    /// 初始化
    /// </summary>
    /// <param name="messageNotifyType">消息通报类型</param>
    /// <param name="title">标题</param>
    /// <param name="message">消息</param>
    public MessageNotifyEventArgs(MessageNotifyType messageNotifyType, string title, string message)
      : base()
    {
      _messageNotifyType = messageNotifyType;
      _title = title;
      _message = message;
    }

    /// <summary>
    /// 初始化
    /// </summary>
    /// <param name="messageNotifyType">消息通报类型</param>
    /// <param name="title">标题</param>
    /// <param name="message">消息</param>
    /// <param name="error">错误信息</param>
    public MessageNotifyEventArgs(MessageNotifyType messageNotifyType, string title, string message, Exception error)
      : this(messageNotifyType, title, message)
    {
      _error = error;
    }

    /// <summary>
    /// 初始化
    /// </summary>
    /// <param name="messageNotifyType">消息通报类型</param>
    /// <param name="title">标题</param>
    /// <param name="error">错误信息</param>
    public MessageNotifyEventArgs(MessageNotifyType messageNotifyType, string title, Exception error)
      : this(messageNotifyType, title, String.Empty, error) { }

    #region 属性

    private readonly MessageNotifyType _messageNotifyType;
    /// <summary>
    /// 消息通报类型
    /// </summary>
    public MessageNotifyType MessageNotifyType
    {
      get { return _messageNotifyType; }
    }

    private readonly string _title;
    /// <summary>
    /// 标题
    /// </summary>
    public string Title
    {
      get { return _title; }
    }

    private readonly string _message;
    /// <summary>
    /// 消息
    /// </summary>
    public string Message
    {
      get { return _message; }
    }

    private readonly Exception _error;
    /// <summary>
    /// 错误信息
    /// </summary>
    public Exception Error
    {
      get { return _error; }
    }

    private static Color? _informationColor;
    /// <summary>
    /// MessageNotifyType.Information颜色
    /// </summary>
    public static Color InformationColor
    {
      get { return AppSettings.GetProperty(ref _informationColor, Color.Black); }
      set { AppSettings.SetProperty(ref _informationColor, value); }
    }

    private static Color? _warningColor;
    /// <summary>
    /// MessageNotifyType.Warning颜色
    /// </summary>
    public static Color WarningColor
    {
      get { return AppSettings.GetProperty(ref _warningColor, Color.Yellow); }
      set { AppSettings.SetProperty(ref _warningColor, value); }
    }
    
    private static Color? _errorColor;
    /// <summary>
    /// MessageNotifyType.Error颜色
    /// </summary>
    public static Color ErrorColor
    {
      get { return AppSettings.GetProperty(ref _errorColor, Color.Red); }
      set { AppSettings.SetProperty(ref _errorColor, value); }
    }

    /// <summary>
    /// 颜色
    /// </summary>
    public Color Color
    {
      get
      {
        switch (_messageNotifyType)
        {
          case MessageNotifyType.Warning:
            return WarningColor;
          case MessageNotifyType.Error:
            return ErrorColor;
          default:
            return _error != null ? ErrorColor : InformationColor;
        }
      }
    }

    #endregion

    #region 方法

    /// <summary>
    /// 字符串表示
    /// </summary>
    public override string ToString()
    {
      return Error == null
        ? String.Format("{0} - {1}", Title, Message)
        : String.Format("{0} - {1}: {2}", Title, Message, AppUtilities.GetErrorHint(Error));
    }

    #endregion
  }
}
