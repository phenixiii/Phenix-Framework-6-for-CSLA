using System;
using System.Runtime.Serialization;

namespace Phenix.Business
{
  /// <summary>
  /// 校验限制保存时异常
  /// </summary>
  [Serializable]
  public class CheckSaveException : Exception
  {
    /// <summary>
    /// 校验限制保存时异常
    /// </summary>
    public CheckSaveException()
      : base() { }

    /// <summary>
    /// 校验限制保存时异常
    /// </summary>
    public CheckSaveException(string message)
     : base(message) { }

    /// <summary>
    /// 校验限制保存时异常
    /// </summary>
    public CheckSaveException(string message, Exception innerException)
      : base(message, innerException) { }

    /// <summary>
    /// 校验限制保存时异常
    /// </summary>
    public CheckSaveException(IBusiness business)
      : base(business == null ? null : String.Format(Phenix.Business.Properties.Resources.CheckSaveException, business.Caption))
    {
      _business = business;
    }

    /// <summary>
    /// 校验限制保存时异常
    /// </summary>
    public CheckSaveException(IBusiness business, string message)
      : base(business == null ? message : String.Format(Phenix.Business.Properties.Resources.CheckSaveException + '\n' + message, business.Caption))
    {
      _business = business;
    }

    /// <summary>
    /// 校验限制保存时异常
    /// </summary>
    public CheckSaveException(IBusiness business, Exception exception)
      : base(business == null ? exception.Message : String.Format(Phenix.Business.Properties.Resources.CheckSaveException + '\n' + exception.Message, business.Caption), exception)
    {
      _business = business;
    }

    #region Serialization

    /// <summary>
    /// 序列化
    /// </summary>
    protected CheckSaveException(SerializationInfo info, StreamingContext context)
      : base(info, context)
    {
      if (info == null)
        throw new ArgumentNullException("info");
      _business = (IBusiness)info.GetValue("_business", typeof(IBusiness));
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
      info.AddValue("_business", _business);
    }

    #endregion

    #region 属性

    private readonly IBusiness _business;
    /// <summary>
    /// 业务对象
    /// </summary>
    public IBusiness Business
    {
      get { return _business; }
    }

    #endregion
  }
}
