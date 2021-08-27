using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.Xml;

namespace Phenix.Services.Contract.Wcf
{
  /// <summary>
  /// ��ʾ System.Runtime.Serialization.DataContractSerializer ������ʱ��Ϊ
  /// </summary>
  public class NetDataContractOperationBehavior : DataContractSerializerOperationBehavior
  {
    #region Constructors

    /// <summary>
    /// ��ʼ��
    /// </summary>
    /// <param name="operation">һ����ʾ������ System.ServiceModel.Description.OperationDescription</param>
    public NetDataContractOperationBehavior(OperationDescription operation)
      : base(operation) { }

    /// <summary>
    /// ��ʼ��
    /// </summary>
    /// <param name="operation">һ����ʾ������ System.ServiceModel.Description.OperationDescription</param>
    /// <param name="dataContractFormatAttribute">���ڿ������л����̵� System.ServiceModel.DataContractFormatAttribute</param>
    public NetDataContractOperationBehavior(OperationDescription operation, DataContractFormatAttribute dataContractFormatAttribute)
      : base(operation, dataContractFormatAttribute) { }

    #endregion

    #region Overrides

    /// <summary>
    /// �������һ��ʵ��
    /// ��������л��ͷ����л����̵� System.Runtime.Serialization.XmlObjectSerializer �м̳�
    /// </summary>
    /// <param name="type">Ҫ���л������л�������</param>
    /// <param name="name">���л����͵�����</param>
    /// <param name="ns">�������͵������ռ�</param>
    /// <param name="knownTypes">������֪���͵� System.Type</param>
    public override XmlObjectSerializer CreateSerializer(Type type, string name,
      string ns, IList<Type> knownTypes)
    {
      return new NetDataContractSerializer(name, ns);
    }

    /// <summary>
    /// �������һ��ʵ��
    /// ��������л��ͷ����л����̣��� System.Xml.XmlDictionaryString ���������ռ䣩�� System.Runtime.Serialization.XmlObjectSerializer �м̳�
    /// </summary>
    /// <param name="type">Ҫ���л������л�������</param>
    /// <param name="name">���л����͵�����</param>
    /// <param name="ns">�������л����͵������ռ�� System.Xml.XmlDictionaryString</param>
    /// <param name="knownTypes">������֪���͵� System.Type</param>
    public override XmlObjectSerializer CreateSerializer(Type type, XmlDictionaryString name,
        XmlDictionaryString ns, IList<Type> knownTypes)
    {
      return new NetDataContractSerializer(name, ns);
    }

    #endregion
  }
}