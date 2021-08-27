using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.Xml;

namespace Phenix.Services.Contract.Wcf
{
  /// <summary>
  /// 表示 System.Runtime.Serialization.DataContractSerializer 的运行时行为
  /// </summary>
  public class NetDataContractOperationBehavior : DataContractSerializerOperationBehavior
  {
    #region Constructors

    /// <summary>
    /// 初始化
    /// </summary>
    /// <param name="operation">一个表示操作的 System.ServiceModel.Description.OperationDescription</param>
    public NetDataContractOperationBehavior(OperationDescription operation)
      : base(operation) { }

    /// <summary>
    /// 初始化
    /// </summary>
    /// <param name="operation">一个表示操作的 System.ServiceModel.Description.OperationDescription</param>
    /// <param name="dataContractFormatAttribute">用于控制序列化过程的 System.ServiceModel.DataContractFormatAttribute</param>
    public NetDataContractOperationBehavior(OperationDescription operation, DataContractFormatAttribute dataContractFormatAttribute)
      : base(operation, dataContractFormatAttribute) { }

    #endregion

    #region Overrides

    /// <summary>
    /// 创建类的一个实例
    /// 该类从序列化和反序列化过程的 System.Runtime.Serialization.XmlObjectSerializer 中继承
    /// </summary>
    /// <param name="type">要序列化或反序列化的类型</param>
    /// <param name="name">序列化类型的名称</param>
    /// <param name="ns">生成类型的命名空间</param>
    /// <param name="knownTypes">包含已知类型的 System.Type</param>
    public override XmlObjectSerializer CreateSerializer(Type type, string name,
      string ns, IList<Type> knownTypes)
    {
      return new NetDataContractSerializer(name, ns);
    }

    /// <summary>
    /// 创建类的一个实例
    /// 该类从序列化和反序列化过程（其 System.Xml.XmlDictionaryString 包含命名空间）的 System.Runtime.Serialization.XmlObjectSerializer 中继承
    /// </summary>
    /// <param name="type">要序列化或反序列化的类型</param>
    /// <param name="name">序列化类型的名称</param>
    /// <param name="ns">包含序列化类型的命名空间的 System.Xml.XmlDictionaryString</param>
    /// <param name="knownTypes">包含已知类型的 System.Type</param>
    public override XmlObjectSerializer CreateSerializer(Type type, XmlDictionaryString name,
        XmlDictionaryString ns, IList<Type> knownTypes)
    {
      return new NetDataContractSerializer(name, ns);
    }

    #endregion
  }
}