using System;
using System.Drawing;

namespace Phenix.Core
{
  /// <summary>
  /// ��Ϣͨ���¼�����
  /// </summary>
  [Serializable]
  public class MessageNotifyEventArgs : EventArgs
  {
    /// <summary>
    /// ��ʼ��
    /// </summary>
    /// <param name="messageNotifyType">��Ϣͨ������</param>
    /// <param name="title">����</param>
    /// <param name="message">��Ϣ</param>
    public MessageNotifyEventArgs(MessageNotifyType messageNotifyType, string title, string message)
      : base()
    {
      _messageNotifyType = messageNotifyType;
      _title = title;
      _message = message;
    }

    /// <summary>
    /// ��ʼ��
    /// </summary>
    /// <param name="messageNotifyType">��Ϣͨ������</param>
    /// <param name="title">����</param>
    /// <param name="message">��Ϣ</param>
    /// <param name="error">������Ϣ</param>
    public MessageNotifyEventArgs(MessageNotifyType messageNotifyType, string title, string message, Exception error)
      : this(messageNotifyType, title, message)
    {
      _error = error;
    }

    /// <summary>
    /// ��ʼ��
    /// </summary>
    /// <param name="messageNotifyType">��Ϣͨ������</param>
    /// <param name="title">����</param>
    /// <param name="error">������Ϣ</param>
    public MessageNotifyEventArgs(MessageNotifyType messageNotifyType, string title, Exception error)
      : this(messageNotifyType, title, String.Empty, error) { }

    #region ����

    private readonly MessageNotifyType _messageNotifyType;
    /// <summary>
    /// ��Ϣͨ������
    /// </summary>
    public MessageNotifyType MessageNotifyType
    {
      get { return _messageNotifyType; }
    }

    private readonly string _title;
    /// <summary>
    /// ����
    /// </summary>
    public string Title
    {
      get { return _title; }
    }

    private readonly string _message;
    /// <summary>
    /// ��Ϣ
    /// </summary>
    public string Message
    {
      get { return _message; }
    }

    private readonly Exception _error;
    /// <summary>
    /// ������Ϣ
    /// </summary>
    public Exception Error
    {
      get { return _error; }
    }

    private static Color? _informationColor;
    /// <summary>
    /// MessageNotifyType.Information��ɫ
    /// </summary>
    public static Color InformationColor
    {
      get { return AppSettings.GetProperty(ref _informationColor, Color.Black); }
      set { AppSettings.SetProperty(ref _informationColor, value); }
    }

    private static Color? _warningColor;
    /// <summary>
    /// MessageNotifyType.Warning��ɫ
    /// </summary>
    public static Color WarningColor
    {
      get { return AppSettings.GetProperty(ref _warningColor, Color.Yellow); }
      set { AppSettings.SetProperty(ref _warningColor, value); }
    }
    
    private static Color? _errorColor;
    /// <summary>
    /// MessageNotifyType.Error��ɫ
    /// </summary>
    public static Color ErrorColor
    {
      get { return AppSettings.GetProperty(ref _errorColor, Color.Red); }
      set { AppSettings.SetProperty(ref _errorColor, value); }
    }

    /// <summary>
    /// ��ɫ
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

    #region ����

    /// <summary>
    /// �ַ�����ʾ
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
