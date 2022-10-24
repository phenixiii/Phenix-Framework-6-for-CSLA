using System;
using System.Runtime.Serialization;

namespace Phenix.Core.Dictionary
{
  /// <summary>
  /// 业务码格式异常
  /// </summary>
  [Serializable]
  public class BusinessCodeFormatException : Exception
  {
    /// <summary>
    /// 业务码格式异常
    /// </summary>
    public BusinessCodeFormatException()
      : base() { }

    /// <summary>
    /// 业务码格式异常
    /// </summary>
    public BusinessCodeFormatException(string businessCodeName)
      : base(businessCodeName) { }

    /// <summary>
    /// 用户验证异常
    /// </summary>
    public BusinessCodeFormatException(string businessCodeName, Exception innerException)
      : base(businessCodeName, innerException) { }

    /// <summary>
    /// 业务码格式异常
    /// </summary>
    public BusinessCodeFormatException(BusinessCodeFormat format, Exception innerException)
      : base(String.Format(Phenix.Core.Properties.Resources.BusinessCodeFormatException,
        format != null ? String.Format("{0}[{1}]", format.BusinessCodeName, format.FormatString) : String.Empty, AppUtilities.GetErrorMessage(innerException)),
        innerException)
    {
      _format = format;
    }

    #region Serialization

    /// <summary>
    /// 序列化
    /// </summary>
    protected BusinessCodeFormatException(SerializationInfo info, StreamingContext context)
      : base(info, context)
    {
      if (info == null)
        throw new ArgumentNullException("info");
      _format = (BusinessCodeFormat)info.GetValue("_format", typeof(BusinessCodeFormat));
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
      info.AddValue("_format", _format);
    }

    #endregion

    #region 属性

    private readonly BusinessCodeFormat _format;
    /// <summary>
    /// 业务码格式
    /// </summary>
    public BusinessCodeFormat Format
    {
      get { return _format; }
    }

    #endregion
  }
}
