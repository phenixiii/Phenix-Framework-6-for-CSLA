using System;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;

namespace Phenix.Services.Contract.Wcf
{
  /// <summary>
  /// 实现可用于扩展服务或客户端应用程序中的操作的运行时行为的方法
  /// 将DataContractSerializerOperationBehavior替换为NetDataContractOperationBehavior
  /// </summary>
  [AttributeUsage(AttributeTargets.Method)]
  internal sealed class UseNetDataContractAttribute : Attribute, IOperationBehavior
  {
    #region IOperationBehavior 成员

    /// <summary>
    /// 在操作范围内执行服务的修改或扩展
    /// </summary>
    /// <param name="operationDescription">正在检查的操作</param>
    /// <param name="bindingParameters">绑定元素支持该行为所需的对象的集合</param>
    public void AddBindingParameters(OperationDescription operationDescription, BindingParameterCollection bindingParameters)
    {
    }

    /// <summary>
    /// 在操作范围内执行服务的修改或扩展
    /// </summary>
    /// <param name="operationDescription">正在检查的操作</param>
    /// <param name="clientOperation">公开 operationDescription 所描述的操作的自定义属性的运行时对象</param>
    public void ApplyClientBehavior(OperationDescription operationDescription, System.ServiceModel.Dispatcher.ClientOperation clientOperation)
    {
      ReplaceDataContractSerializerOperationBehavior(operationDescription);
    }

    /// <summary>
    /// 在操作范围内执行服务的修改或扩展
    /// </summary>
    /// <param name="operationDescription">正在检查的操作</param>
    /// <param name="dispatchOperation">公开 operationDescription 所描述的操作的自定义属性的运行时对象</param>
    public void ApplyDispatchBehavior(OperationDescription operationDescription, System.ServiceModel.Dispatcher.DispatchOperation dispatchOperation)
    {
      ReplaceDataContractSerializerOperationBehavior(operationDescription);
    }

    /// <summary>
    /// 实现此方法可以确定操作是否满足某些设定条件
    /// </summary>
    /// <param name="operationDescription">正在检查的操作</param>
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