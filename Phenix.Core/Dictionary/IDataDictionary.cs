using System.Collections.Generic;
using Phenix.Core.Mapping;
using Phenix.Core.Security;

namespace Phenix.Core.Dictionary
{
  /// <summary>
  /// 数据字典接口
  /// </summary>
  public interface IDataDictionary
  {
    #region 属性

    /// <summary>
    /// 企业
    /// </summary>
    string Enterprise { get; }

    /// <summary>
    /// 部门资料队列
    /// </summary>
    IDictionary<long, DepartmentInfo> DepartmentInfos { get; }

    /// <summary>
    /// 岗位资料队列
    /// </summary>
    IDictionary<long, PositionInfo> PositionInfos { get; }

    /// <summary>
    /// 表过滤器资料队列
    /// </summary>
    IDictionary<string, TableFilterInfo> TableFilterInfos { get; }

    /// <summary>
    /// 角色资料队列
    /// </summary>
    IDictionary<string, RoleInfo> RoleInfos { get; }

    /// <summary>
    /// 切片资料队列
    /// </summary>
    IDictionary<string, SectionInfo> SectionInfos { get; }

    #region BusinessCode

    /// <summary>
    /// 业务码格式队列
    /// </summary>
    IDictionary<string, BusinessCodeFormat> BusinessCodeFormats { get; }

    #endregion

    #endregion

    #region 方法

    /// <summary>
    /// 部门资料已更新
    /// </summary>
    void DepartmentInfoHasChanged();

    /// <summary>
    /// 岗位资料已更新
    /// </summary>
    void PositionInfoHasChanged();

    /// <summary>
    /// 程序集资料已更新
    /// </summary>
    void AssemblyInfoHasChanged();

    /// <summary>
    /// 取程序集资料
    /// </summary>
    IDictionary<string, AssemblyInfo> GetAssemblyInfos();

    /// <summary>
    /// 取程序集资料
    /// </summary>
    /// <param name="assemblyName">程序集名或类全名</param>
    AssemblyInfo GetAssemblyInfo(string assemblyName);

    /// <summary>
    /// 表过滤器资料已更新
    /// </summary>
    void TableFilterInfoHasChanged();

    /// <summary>
    /// 角色资料已更新
    /// </summary>
    void RoleInfoHasChanged();

    /// <summary>
    /// 切片资料已更新
    /// </summary>
    void SectionInfoHasChanged();

    /// <summary>
    /// 表结构已更新
    /// </summary>
    void TableInfoHasChanged();

    /// <summary>
    /// 新增程序集类资料
    /// </summary>
    void AddAssemblyClassInfo(string assemblyName, string assemblyCaption, string className, string classCaption, ExecuteAction? permanentExecuteAction, string[] groupNames, AssemblyClassType classType);

    /// <summary>
    /// 新增程序集类属性资料
    /// </summary>
    void AddAssemblyClassPropertyInfos(string assemblyName, string className, string[] names, string[] captions,
      string[] tableNames, string[] columnNames, string[] aliases, ExecuteModify[] permanentExecuteModifies);

    /// <summary>
    /// 新增程序集类属性资料
    /// </summary>
    void AddAssemblyClassPropertyConfigInfos(string assemblyName, string className, string[] names, string[] captions,
      bool[] configurables, string[] configKeys, string[] configValues, AssemblyClassType classType);

    /// <summary>
    /// 新增程序集类方法资料
    /// </summary>
    void AddAssemblyClassMethodInfos(string assemblyName, string className, string[] names, string[] captions, string[] tags, bool[] allowVisibles);

    #region BusinessCode

    /// <summary>
    /// 获取业务码格式
    /// </summary>
    BusinessCodeFormat GetBusinessCodeFormat(string businessCodeName);

    /// <summary>
    /// 设置业务码格式
    /// </summary>
    void SetBusinessCodeFormat(BusinessCodeFormat format);

    /// <summary>
    /// 移除业务码格式
    /// </summary>
    void RemoveBusinessCodeFormat(string businessCodeName);

    #endregion

    #endregion
  }
}