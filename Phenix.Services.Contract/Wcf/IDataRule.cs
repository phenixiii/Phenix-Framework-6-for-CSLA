using System.ServiceModel;
using Phenix.Core.Mapping;
using Phenix.Core.Rule;
using Phenix.Core.Security;

namespace Phenix.Services.Contract.Wcf
{
  [ServiceContract]
  public interface IDataRule
  {
    #region PromptCode

    [OperationContract]
    [UseNetDataContract]
    object GetPromptCodes(string name, UserIdentity identity);

    [OperationContract]
    [UseNetDataContract]
    object GetPromptCodeChangedTime(string name);

    [OperationContract(IsOneWay = true)]
    [UseNetDataContract]
    void PromptCodeHasChanged(string name);

    [OperationContract]
    [UseNetDataContract]
    object AddPromptCode(string name, string key, string caption, string value, ReadLevel readLevel, UserIdentity identity);

    [OperationContract]
    [UseNetDataContract]
    object RemovePromptCode(string name, string key);

    #endregion

    #region CriteriaExpression

    [OperationContract]
    [UseNetDataContract]
    object GetCriteriaExpressions(string name, UserIdentity identity);

    [OperationContract]
    [UseNetDataContract]
    object GetCriteriaExpressionChangedTime(string name);

    [OperationContract(IsOneWay = true)]
    [UseNetDataContract]
    void CriteriaExpressionHasChanged(string name);

    [OperationContract]
    [UseNetDataContract]
    object AddCriteriaExpression(string name, string key, string caption, CriteriaExpression value, ReadLevel readLevel, UserIdentity identity2);

    [OperationContract]
    [UseNetDataContract]
    object RemoveCriteriaExpression(string name, string key);

    #endregion
  }
}
