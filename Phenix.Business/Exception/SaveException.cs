using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Phenix.Core;

namespace Phenix.Business
{
  /// <summary>
  /// 保存数据异常
  /// </summary>
  [Serializable]
  public class SaveException : Exception
  {
    /// <summary>
    /// 保存数据异常
    /// </summary>
    public SaveException()
      : base(Phenix.Business.Properties.Resources.SaveException) { }

    /// <summary>
    /// 保存数据异常
    /// </summary>
    public SaveException(string message)
      : base(Phenix.Business.Properties.Resources.SaveException + '\n' + message) { }

    /// <summary>
    /// 保存数据异常
    /// </summary>
    public SaveException(string message, Exception innerException)
      : base(Phenix.Business.Properties.Resources.SaveException + '\n' + message, innerException) { }

    /// <summary>
    /// 保存数据异常
    /// </summary>
    public SaveException(IList<ExceptionEventArgs> saveErrors)
      : base(Phenix.Business.Properties.Resources.SaveException) 
    {
      _saveErrors = saveErrors;
    }

    #region Serialization

    /// <summary>
    /// 序列化
    /// </summary>
    protected SaveException(SerializationInfo info, StreamingContext context)
      : base(info, context)
    {
      if (info == null)
        throw new ArgumentNullException("info");
      _saveErrors = (IList<ExceptionEventArgs>)info.GetValue("_saveErrors", typeof(IList<ExceptionEventArgs>));
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
      info.AddValue("_saveErrors", _saveErrors);
    }

    #endregion

    #region 属性

    private readonly IList<ExceptionEventArgs> _saveErrors;
    /// <summary>
    /// Save时发生的错误
    /// </summary>
    public IList<ExceptionEventArgs> SaveErrors
    {
      get { return _saveErrors; }
    }
    
    #endregion
  }
}