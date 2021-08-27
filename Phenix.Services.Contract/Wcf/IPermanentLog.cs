using System;
using System.ServiceModel;
using Phenix.Core.Mapping;

namespace Phenix.Services.Contract.Wcf
{
  [ServiceContract]
  public interface IPermanentLog
  {
    [OperationContract(IsOneWay = true)]
    [UseNetDataContract]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1716:IdentifiersShouldNotMatchKeywords")]
    void Save(string userNumber, string typeName, string message, Exception error);

    [OperationContract]
    [UseNetDataContract]
    object Fetch(string userNumber, string typeName,
      DateTime startTime, DateTime finishTime);

    [OperationContract(IsOneWay = true)]
    [UseNetDataContract]
    void Clear(string userNumber, string typeName,
      DateTime startTime, DateTime finishTime);

    [OperationContract(IsOneWay = true)]
    [UseNetDataContract]
    void SaveExecuteAction(string userNumber, string typeName, string primaryKey,
      ExecuteAction action, string log);

    [OperationContract]
    [UseNetDataContract]
    object FetchExecuteAction(string typeName, string primaryKey);

    [OperationContract]
    [UseNetDataContract]
    object FetchUserExecuteAction(string userNumber, string typeName,
      ExecuteAction action, DateTime startTime, DateTime finishTime);

    [OperationContract(IsOneWay = true)]
    [UseNetDataContract]
    void ClearUserExecuteAction(string userNumber, string typeName,
      ExecuteAction action, DateTime startTime, DateTime finishTime);
  }
}
