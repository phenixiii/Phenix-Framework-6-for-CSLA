using System;
using System.Runtime.Serialization;

namespace Phenix.Business
{
  /// <summary>
  /// 执行指令异常
  /// </summary>
  [Serializable]
  public class ExecuteException : Exception
  {
    /// <summary>
    /// 执行指令异常
    /// </summary>
    public ExecuteException()
      : base(Phenix.Business.Properties.Resources.ExecuteException) { }

    /// <summary>
    /// 执行指令异常
    /// </summary>
    public ExecuteException(string message)
      : base(Phenix.Business.Properties.Resources.ExecuteException + '\n' + message) { }

    /// <summary>
    /// 执行指令异常
    /// </summary>
    public ExecuteException(string message, Exception innerException)
      : base(Phenix.Business.Properties.Resources.ExecuteException + '\n' + message, innerException) { }

    #region Serialization

    /// <summary>
    /// 序列化
    /// </summary>
    protected ExecuteException(SerializationInfo info, StreamingContext context)
      : base(info, context) { }

    #endregion
  }
}