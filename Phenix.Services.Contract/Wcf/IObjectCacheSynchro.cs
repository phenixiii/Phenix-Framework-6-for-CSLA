using System.Collections.Generic;
using System.ServiceModel;

namespace Phenix.Services.Contract.Wcf
{
  [ServiceContract]
  public interface IObjectCacheSynchro
  {
    [OperationContract]
    [UseNetDataContract]
    object GetActionTime(string typeName);

    [OperationContract(IsOneWay = true)]
    [UseNetDataContract]
    void ClearAll();

    [OperationContract(IsOneWay = true)]
    [UseNetDataContract]
    void Clear(IList<string> typeNames);

    [OperationContract(IsOneWay = true)]
    [UseNetDataContract]
    void RecordHasChanged(string tableName);
  }
}
