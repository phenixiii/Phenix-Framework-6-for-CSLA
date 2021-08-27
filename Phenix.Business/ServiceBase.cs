using System;

namespace Phenix.Business
{
  /// <summary>
  /// 服务基类
  /// </summary>
  [Serializable]
  public abstract class ServiceBase<T> : Phenix.Core.Data.ServiceBase<T>
    where T : ServiceBase<T>
  {
  }
}
