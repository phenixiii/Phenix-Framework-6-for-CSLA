using System;
using System.Activities.DurableInstancing;
using System.Collections.Generic;
using System.IO;
using System.Runtime.DurableInstancing;
using System.Runtime.Serialization;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using Phenix.Core.Threading;

namespace Phenix.Core.Workflow
{
  internal class InstanceStore : System.Runtime.DurableInstancing.InstanceStore
  {
    public InstanceStore(Guid workflowInstanceId, IStartCommand command)
      : base()
    {
      _workflowInstanceId = workflowInstanceId;
      WorkflowIdentityInfo workflowIdentityInfo = WorkflowHub.GetWorkflowIdentityInfo(command, true);
      _typeNamespace = workflowIdentityInfo.TypeNamespace;
      _typeName = workflowIdentityInfo.TypeName;
      _taskContext = command.TaskContext;
    }

    public InstanceStore(WorkflowTaskInfo workflowTaskInfo)
      : base()
    {
      _workflowInstanceId = workflowTaskInfo.WorkflowInstanceId;
      _typeNamespace = workflowTaskInfo.TypeNamespace;
      _typeName = workflowTaskInfo.TypeName;
      _taskContext = workflowTaskInfo.TaskContext;
    }

    #region 属性

    private readonly Guid _workflowInstanceId;
    private readonly string _typeNamespace;
    private readonly string _typeName;

    private TaskContext _taskContext;

    #endregion

    #region 方法

    public void ChangeTaskContext(TaskContext taskContext)
    {
      _taskContext = taskContext;
    }

    /// <summary>
    /// 永久性提供程序可实现此方法
    /// 这可确定是否可执行特定的永久性指令和是否可异步执行此指令
    /// </summary>
    protected override bool TryCommand(InstancePersistenceContext context, InstancePersistenceCommand command, TimeSpan timeout)
    {
      return EndTryCommand(BeginTryCommand(context, command, timeout, null, null));
    }

    /// <summary>
    /// 永久性提供程序可实现此方法
    /// 这可确定是否可执行特定的永久性指令
    /// 如果可以执行此指令，请异步执行此指令
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Reliability", "CA2000:DisposeObjectsBeforeLosingScope")]
    protected override IAsyncResult BeginTryCommand(InstancePersistenceContext context, InstancePersistenceCommand command, TimeSpan timeout, AsyncCallback callback, object state)
    {
      if (command is CreateWorkflowOwnerCommand)
        context.BindInstanceOwner(_workflowInstanceId, Guid.NewGuid());
      else if (command is SaveWorkflowCommand)
        SaveInstanceData(((SaveWorkflowCommand)command).InstanceData);
      else if (command is LoadWorkflowCommand)
        context.LoadedInstance(InstanceState.Initialized, LoadInstanceData(), null, null, null);
      return new CompletedAsyncResult<bool>(true, callback, state);
    }

    /// <summary>
    /// 结束异步操作
    /// </summary>
    protected override bool EndTryCommand(IAsyncResult result)
    {
      return CompletedAsyncResult<bool>.End((CompletedAsyncResult<bool>)result);
    }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2202:DoNotDisposeObjectsMultipleTimes")]
    private void SaveInstanceData(IDictionary<XName, InstanceValue> value)
    {
      XmlDocument document = new XmlDocument();
      document.LoadXml("<InstanceValues/>");
      foreach (KeyValuePair<XName, InstanceValue> valPair in value)
      {
        XmlElement newInstance = document.CreateElement("InstanceValue");
        XmlElement newKey = SerializeObject(document, "key", valPair.Key);
        newInstance.AppendChild(newKey);
        XmlElement newValue = SerializeObject(document, "value", valPair.Value.Value);
        newInstance.AppendChild(newValue);
        document.DocumentElement.AppendChild(newInstance);
      }

      string content;
      using (MemoryStream memoryStream = new MemoryStream())
      {
        document.Save(memoryStream);
        memoryStream.Position = 0;
        using (StreamReader reader = new StreamReader(memoryStream))
        {
          content = reader.ReadToEnd();
        }
      }
      WorkflowHub.SaveWorkflowInstance(_workflowInstanceId, _typeNamespace, _typeName, content, _taskContext);
    }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2202:DoNotDisposeObjectsMultipleTimes")]
    private static XmlElement SerializeObject(XmlDocument document, string elementName, object value)
    {
      XmlElement result = document.CreateElement(elementName);
      using (MemoryStream memoryStream = new MemoryStream())
      {
        NetDataContractSerializer serializer = new NetDataContractSerializer();
        serializer.Serialize(memoryStream, value);
        memoryStream.Position = 0;
        using (StreamReader reader = new StreamReader(memoryStream))
        {
          result.InnerXml = reader.ReadToEnd();
        }
      }
      return result;
    }

    private IDictionary<XName, InstanceValue> LoadInstanceData()
    {
      Dictionary<XName, InstanceValue> result = new Dictionary<XName, InstanceValue>();
      NetDataContractSerializer serializer = new NetDataContractSerializer();
      XmlDocument document = new XmlDocument();
      document.LoadXml(WorkflowHub.FetchWorkflowInstance(_workflowInstanceId));
      foreach (XmlElement item in document.GetElementsByTagName("InstanceValue"))
      {
        XmlElement keyElement = (XmlElement)item.SelectSingleNode("descendant::key");
        XName key = (XName)DeserializeObject(serializer, keyElement);
        XmlElement valueElement = (XmlElement)item.SelectSingleNode("descendant::value");
        object value = DeserializeObject(serializer, valueElement);
        result.Add(key, new InstanceValue(value));
      }
      return result;
    }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2202:DoNotDisposeObjectsMultipleTimes")]
    private static object DeserializeObject(NetDataContractSerializer serializer, XmlElement element)
    {
      using (MemoryStream memoryStream = new MemoryStream())
      {
        using (XmlDictionaryWriter writer = XmlDictionaryWriter.CreateTextWriter(memoryStream, Encoding.UTF8, false))
        {
          element.WriteContentTo(writer);
          writer.Flush();
        }
        memoryStream.Position = 0;
        return serializer.Deserialize(memoryStream);
      }
    }

    #endregion
  }
}
