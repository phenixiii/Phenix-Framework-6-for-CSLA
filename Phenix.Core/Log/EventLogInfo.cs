using System;

namespace Phenix.Core.Log
{
  /// <summary>
  /// 日志信息
  /// </summary>
  [Serializable]
  public sealed class EventLogInfo
  {
    /// <summary>
    /// 初始化
    /// </summary>
    /// <param name="time">时间</param>
    /// <param name="userNumber">登录工号</param>
    /// <param name="message">消息</param>
    /// <param name="exceptionName">错误名</param>
    /// <param name="exceptionMessage">错误消息</param>
    public EventLogInfo(DateTime time, string userNumber, string message,
      string exceptionName, string exceptionMessage)
    {
      _time = time;
      _userNumber = userNumber;
      _message = message;
      _exceptionName = exceptionName;
      _exceptionMessage = exceptionMessage;
    }

    #region 属性

    private readonly DateTime _time;
    /// <summary>
    /// 时间
    /// </summary>
    public DateTime Time
    {
      get { return _time; }
    }

    private readonly string _userNumber;
    /// <summary>
    /// 登录工号
    /// </summary>
    public string UserNumber
    {
      get { return _userNumber; }
    }

    private readonly string _message;
    /// <summary>
    /// 消息
    /// </summary>
    public string Message
    {
      get { return _message; }
    }

    private readonly string _exceptionName;
    /// <summary>
    /// 错误名
    /// </summary>
    public string ExceptionName
    {
      get { return _exceptionName; }
    }

    private readonly string _exceptionMessage;
    /// <summary>
    /// 错误消息
    /// </summary>
    public string ExceptionMessage
    {
      get { return _exceptionMessage; }
    }

    #endregion
  }
}
