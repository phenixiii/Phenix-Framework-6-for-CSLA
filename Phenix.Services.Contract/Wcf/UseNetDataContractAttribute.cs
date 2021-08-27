using System;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;

namespace Phenix.Services.Contract.Wcf
{
  /// <summary>
  /// ʵ�ֿ�������չ�����ͻ���Ӧ�ó����еĲ���������ʱ��Ϊ�ķ���
  /// ��DataContractSerializerOperationBehavior�滻ΪNetDataContractOperationBehavior
  /// </summary>
  [AttributeUsage(AttributeTargets.Method)]
  internal sealed class UseNetDataContractAttribute : Attribute, IOperationBehavior
  {
    #region IOperationBehavior ��Ա

    /// <summary>
    /// �ڲ�����Χ��ִ�з�����޸Ļ���չ
    /// </summary>
    /// <param name="operationDescription">���ڼ��Ĳ���</param>
    /// <param name="bindingParameters">��Ԫ��֧�ָ���Ϊ����Ķ���ļ���</param>
    public void AddBindingParameters(OperationDescription operationDescription, BindingParameterCollection bindingParameters)
    {
    }

    /// <summary>
    /// �ڲ�����Χ��ִ�з�����޸Ļ���չ
    /// </summary>
    /// <param name="operationDescription">���ڼ��Ĳ���</param>
    /// <param name="clientOperation">���� operationDescription �������Ĳ������Զ������Ե�����ʱ����</param>
    public void ApplyClientBehavior(OperationDescription operationDescription, System.ServiceModel.Dispatcher.ClientOperation clientOperation)
    {
      ReplaceDataContractSerializerOperationBehavior(operationDescription);
    }

    /// <summary>
    /// �ڲ�����Χ��ִ�з�����޸Ļ���չ
    /// </summary>
    /// <param name="operationDescription">���ڼ��Ĳ���</param>
    /// <param name="dispatchOperation">���� operationDescription �������Ĳ������Զ������Ե�����ʱ����</param>
    public void ApplyDispatchBehavior(OperationDescription operationDescription, System.ServiceModel.Dispatcher.DispatchOperation dispatchOperation)
    {
      ReplaceDataContractSerializerOperationBehavior(operationDescription);
    }

    /// <summary>
    /// ʵ�ִ˷�������ȷ�������Ƿ�����ĳЩ�趨����
    /// </summary>
    /// <param name="operationDescription">���ڼ��Ĳ���</param>
    public void Validate(OperationDescription operationDescription)
    {
    }

    #endregion

    private static void ReplaceDataContractSerializerOperationBehavior(OperationDescription operationDescription)
    {
      DataContractSerializerOperationBehavior dcsOperationBehavior = operationDescription.Behaviors.Find<DataContractSerializerOperationBehavior>();
      if (dcsOperationBehavior != null)
      {
        operationDescription.Behaviors.Remove(dcsOperationBehavior);
        operationDescription.Behaviors.Add(new NetDataContractOperationBehavior(operationDescription));
      }
    }
  }
}