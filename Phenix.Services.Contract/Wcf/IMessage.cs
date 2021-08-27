using System.ServiceModel;
using Phenix.Core.Security;

namespace Phenix.Services.Contract.Wcf
{
  [ServiceContract]
  public interface IMessage
  {
    [OperationContract(IsOneWay = true)]
    [UseNetDataContract]
    void Send(string receiver, string content, UserIdentity identity);

    [OperationContract]
    [UseNetDataContract]
    object Receive(UserIdentity identity);

    [OperationContract(IsOneWay = true)]
    [UseNetDataContract]
    void AffirmReceived(long id, bool burn, UserIdentity identity);
  }
}
