using System;
using System.Activities;
using System.Activities.XamlIntegration;
using System.IO;
using Phenix.Core.Reflection;
using Phenix.Core.Security;

namespace Phenix.Core.Workflow
{
  /// <summary>
  /// 工作流资料
  /// </summary>
  [Serializable]
  public sealed class WorkflowInfo
  {
    /// <summary>
    /// 初始化
    /// </summary>
    [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
    public WorkflowInfo(string typeNamespace, string typeName, string caption, string xamlCode,
      string createUserNumber, DateTime createTime, string changeUserNumber, DateTime? changeTime, 
      string disableUserNumber, DateTime? disableTime)
    {
      _typeNamespace = typeNamespace;
      _typeName = typeName;
      _caption = caption;
      _xamlCode = xamlCode;
      _createUserNumber = createUserNumber;
      _createTime = createTime;
      _changeUserNumber = changeUserNumber;
      _changeTime = changeTime;
      _disableUserNumber = disableUserNumber;
      _disableTime = disableTime;
    }

    #region 工厂

    /// <summary>
    /// 新增
    /// </summary>
    public static WorkflowInfo New(string typeNamespace, string typeName, string caption, string xamlCode,
      IIdentity identity)
    {
      return new WorkflowInfo(typeNamespace, typeName, caption, xamlCode,
        identity.UserNumber, DateTime.Now, null, null,
        null, null);
    }

    #endregion

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

    /// <summary>
    /// 完整类名
    /// </summary>
    public string FullTypeName
    {
      get { return Utilities.AssembleFullTypeName(TypeNamespace, TypeName); }
    }

    private readonly string _caption;
    /// <summary>
    /// 标签
    /// </summary>
    public string Caption
    {
      get { return _caption; }
    }

    private string _xamlCode;
    /// <summary>
    /// xaml代码
    /// </summary>
    public string XamlCode
    {
      get { return _xamlCode; }
      set
      {
        if (value != null)
        {
          if (value.IndexOf(" PluginAssemblyName=\"{x:Null}\" ", System.StringComparison.Ordinal) > 0)
            throw new InvalidOperationException(String.Format(Phenix.Core.Properties.Resources.PluginAssemblyNameRequired, FullTypeName, Caption));
        }
        _xamlCode = value;
        _activityDefinition = null;
      }
    }

    [NonSerialized]
    private Activity _activityDefinition;
    /// <summary>
    /// 活动定义
    /// </summary>
    public Activity ActivityDefinition
    {
      get
      {
        if (_activityDefinition == null)
          using (StringReader stringReader = new StringReader(_xamlCode))
          {
            _activityDefinition = ActivityXamlServices.Load(stringReader);
          }
        return _activityDefinition;
      }
    }

    private readonly string _createUserNumber;
    /// <summary>
    /// 构建工号
    /// </summary>
    public string CreateUserNumber
    {
      get { return _createUserNumber; }
    }

    private readonly DateTime _createTime;
    /// <summary>
    /// 构建时间
    /// </summary>
    public DateTime CreateTime
    {
      get { return _createTime; }
    }

    private readonly string _changeUserNumber;
    /// <summary>
    /// 更新工号
    /// </summary>
    public string ChangeUserNumber
    {
      get { return _changeUserNumber; }
    }

    private readonly DateTime? _changeTime;
    /// <summary>
    /// 更新时间
    /// </summary>
    public DateTime? ChangeTime
    {
      get { return _changeTime; }
    }

    private readonly string _disableUserNumber;
    /// <summary>
    /// 禁用工号
    /// </summary>
    public string DisableUserNumber
    {
      get { return _disableUserNumber; }
    }

    private readonly DateTime? _disableTime;
    /// <summary>
    /// 禁用时间
    /// </summary>
    public DateTime? DisableTime
    {
      get { return _disableTime; }
    }

    #endregion

    #region 方法

    /// <summary>
    /// 保存到文件
    /// </summary>
    public string SaveToFile()
    {
      string result = Path.Combine(AppConfig.TempDirectory, String.Format("{0}.xaml", FullTypeName));
      using (StreamWriter logFile = File.CreateText(result))
      {
        logFile.Write(XamlCode);
        logFile.Flush();
      }
      return result;
    }

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
