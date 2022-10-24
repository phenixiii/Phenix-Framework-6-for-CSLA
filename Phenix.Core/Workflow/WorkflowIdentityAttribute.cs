using System;

namespace Phenix.Core.Workflow
{
  /// <summary>
  /// "工作流标识"标签
  /// 与IStartCommand配套使用
  /// </summary>
  [AttributeUsage(AttributeTargets.Class)]
  public sealed class WorkflowIdentityAttribute : Attribute
  {
    /// <summary>
    /// 初始化
    /// </summary>
    /// <param name="typeNamespace">命名空间</param>
    /// <param name="typeName">类型名称</param>
    public WorkflowIdentityAttribute(string typeNamespace, string typeName)
      : base()
    {
      _typeNamespace = typeNamespace;
      _typeName = typeName;
    }

    /// <summary>
    /// 初始化
    /// </summary>
    /// <param name="typeNamespace">命名空间</param>
    /// <param name="typeName">类型名称</param>
    /// <param name="caption">标签</param>
    public WorkflowIdentityAttribute(string typeNamespace, string typeName, string caption)
      : this(typeNamespace, typeName)
    {
      _caption = caption;
    }

    #region 属性
    
    private readonly string _typeNamespace;
    /// <summary>
    /// 命名空间
    /// </summary> 
    public string TypeNamespace
    {
      get { return _typeNamespace; }
    }

    private readonly string _typeName;
    /// <summary>
    /// 类型名称
    /// </summary> 
    public string TypeName
    {
      get { return _typeName; }
    }

    private readonly string _caption;
    /// <summary>
    /// 标签
    /// </summary>
    public string Caption
    {
      get { return _caption; }
    }

    #endregion
  }
}