using Phenix.Business;

namespace Phenix.StandardRule.Information
{
  /// <summary>
  /// 资料清单接口
  /// </summary>
  public interface IInformationList<T> : IBusinessCollection
    where T : IInformation
  {
  }
}
