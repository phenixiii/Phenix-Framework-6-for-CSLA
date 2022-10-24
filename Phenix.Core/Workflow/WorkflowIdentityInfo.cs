using System;
using Phenix.Core.Reflection;

namespace Phenix.Core.Workflow
{
  /// <summary>
  /// 工作流标识信息
  /// </summary>
  public sealed class WorkflowIdentityInfo
  {
    private WorkflowIdentityInfo(Type ownerType, WorkflowIdentityAttribute workflowIdentityAttribute)
    {
      _ownerType = ownerType;
      _workflowIdentityAttribute = workflowIdentityAttribute;
    }

    #region 工厂

    internal static WorkflowIdentityInfo Fetch(Type ownerType)
    {
      ownerType = Utilities.LoadType(ownerType); //主要用于IDE环境

      WorkflowIdentityAttribute workflowIdentityAttribute = AppUtilities.GetFirstCustomAttribute<WorkflowIdentityAttribute>(ownerType);
      return workflowIdentityAttribute != null ? new WorkflowIdentityInfo(ownerType, workflowIdentityAttribute) : null;
    }

    #endregion

    #region 属性

    private readonly Type _ownerType;
    /// <summary>
    /// 所属类
    /// </summary>
    public Type OwnerType
    {
      get { return _ownerType; }
    }

    private readonly WorkflowIdentityAttribute _workflowIdentityAttribute;

    private string _typeNamespace;
    /// <summary>
    /// 命名空间
    /// </summary> 
    public string TypeNamespace
    {
      get
      {
        if (_typeNamespace == null)
          _typeNamespace = !String.IsNullOrEmpty(_workflowIdentityAttribute.TypeNamespace)
            ? _workflowIdentityAttribute.TypeNamespace 
            : _ownerType.Namespace;
        return _typeNamespace;
      }
    }

    private string _typeName;
    /// <summary>
    /// 类型名称
    /// </summary> 
    public string TypeName
    {
      get
      {
        if (_typeName == null)
          _typeName = !String.IsNullOrEmpty(_workflowIdentityAttribute.TypeName)
            ? _workflowIdentityAttribute.TypeName
            : _ownerType.Name;
        return _typeName;
      }
    }

    /// <summary>
    /// 完整类名
    /// </summary>
    public string FullTypeName
    {
      get { return Utilities.AssembleFullTypeName(TypeNamespace, TypeName); }
    }

    private string _caption;
    /// <summary>
    /// 标签
    /// </summary> 
    public string Caption
    {
      get
      {
        if (_caption == null)
          _caption = !String.IsNullOrEmpty(_workflowIdentityAttribute.Caption)
            ? _workflowIdentityAttribute.Caption
            : _ownerType.Name;
        return _caption;
      }
    }

    #endregion

    #region 方法

    /// <summary>
    /// 字符串表示
    /// </summary>
    public override string ToString()
    {
      return String.Format("{0}[{1}]", Caption, FullTypeName);
    }

    #endregion
  }
}