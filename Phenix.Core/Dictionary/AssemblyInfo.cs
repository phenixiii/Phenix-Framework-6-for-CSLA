using System;
using System.Collections.Generic;
using Phenix.Core.Mapping;

namespace Phenix.Core.Dictionary
{
  /// <summary>
  /// 程序集资料
  /// </summary>
  [Serializable]
  public sealed class AssemblyInfo
  {
    /// <summary>
    /// 初始化
    /// </summary>
    [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
    public AssemblyInfo(string name, string caption)
    {
      _name = name;
      _caption = caption;
    }

    #region 属性

    private readonly string _name;
    /// <summary>
    /// 名称
    /// </summary>
    public string Name
    {
      get { return _name; }
    }

    private readonly string _caption;
    /// <summary>
    /// 标签
    /// </summary>
    public string Caption
    {
      get { return _caption; }
    }

    private readonly Dictionary<string, AssemblyClassInfo> _classInfos = new Dictionary<string, AssemblyClassInfo>(StringComparer.Ordinal);
    /// <summary>
    /// 类资料队列
    /// </summary>
    public ICollection<AssemblyClassInfo> ClassInfos
    {
      get { return _classInfos.Values; }
    }

    #endregion

    #region 方法

    /// <summary>
    /// 添加类资料
    /// </summary>
    [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
    public AssemblyClassInfo AddAssemblyClassInfo(
      string name, string caption, bool captionConfigured,
      ExecuteAction permanentExecuteAction, bool permanentExecuteConfigured,
      AssemblyClassType classType, bool authorised,
      IList<string> denyCreateRoles, IList<string> denyGetRoles,
      IList<string> denyEditRoles, IList<string> denyDeleteRoles,
      IList<long> departmentIds,
      IDictionary<string, AssemblyClassPropertyInfo> classPropertyInfos, IDictionary<string, AssemblyClassMethodInfo> classMethodInfos)
    {
      AssemblyClassInfo classInfo = new AssemblyClassInfo(this,
        name, caption, captionConfigured,
        permanentExecuteAction, permanentExecuteConfigured,
        classType, authorised,
        denyCreateRoles, denyGetRoles, denyEditRoles, denyDeleteRoles,
        departmentIds,
        classPropertyInfos, classMethodInfos);
      _classInfos[classInfo.Name] = classInfo;
      return classInfo;
    }

    /// <summary>
    /// 取类资料
    /// </summary>
    /// <param name="className">类名称</param>
    public AssemblyClassInfo GetClassInfo(string className)
    {
      if (String.IsNullOrEmpty(className))
        return null;
      AssemblyClassInfo result;
      if (_classInfos.TryGetValue(className, out result))
        return result;
      return null;
    }

    #endregion
  }
}