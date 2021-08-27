using System;
using System.Runtime.Serialization;

namespace Phenix.StandardRule.Information
{
  /// <summary>
  /// 资料处理异常
  /// </summary>
  [Serializable]
  public class InformationException : Exception
  {
    /// <summary>
    /// 资料处理异常
    /// </summary>
    public InformationException()
      : base() { }

    /// <summary>
    /// 资料处理异常
    /// </summary>
    public InformationException(string message)
     : base(message) { }

    /// <summary>
    /// 资料处理异常
    /// </summary>
    public InformationException(string message, Exception innerException)
      : base(message, innerException) { }

    /// <summary>
    /// 资料处理异常
    /// </summary>
    public InformationException(InformationAction action, string message)
      : base(message)
    {
      _action = action;
    }

    /// <summary>
    /// 资料处理异常
    /// </summary>
    public InformationException(InformationAction action, string message, Exception innerException)
      : base(message, innerException)
    {
      _action = action;
    }

    #region Serialization

    /// <summary>
    /// 序列化
    /// </summary>
    protected InformationException(SerializationInfo info, StreamingContext context)
      : base(info, context)
    {
      if (info == null)
        throw new ArgumentNullException("info");
      _action = (InformationAction)info.GetValue("_action", typeof(InformationAction));
    }

    /// <summary>
    /// 反序列化
    /// </summary>
    [System.Security.SecurityCritical]
    public override void GetObjectData(SerializationInfo info, StreamingContext context)
    {
      if (info == null)
        throw new ArgumentNullException("info");
      base.GetObjectData(info, context);
      info.AddValue("_action", _action);
    }
    
    #endregion

    #region 属性

    private readonly InformationAction _action;
    /// <summary>
    /// 资料动作
    /// </summary>
    public InformationAction Action
    {
      get { return _action; }
    }

    #endregion
  }
}