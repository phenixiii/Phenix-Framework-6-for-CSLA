using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using Phenix.Core.IO;
using Phenix.Core.Mapping;

namespace Phenix.Core.Web
{
  /// <summary>
  /// 数据代理接口
  /// </summary>
  public interface IDataProxy
  {
    #region 属性

    /// <summary>
    /// HttpClient
    /// </summary>
    HttpClient HttpClient { get; }

    #endregion

    #region 方法

    #region Sequence

    /// <summary>
    /// 获取64位序号
    /// </summary>
    /// <returns>64位序号</returns>
    Task<long> FetchSequenceValueAsync();

    /// <summary>
    /// 获取64位序号
    /// </summary>
    /// <returns>64位序号</returns>
    long FetchSequenceValue();

    #endregion

    #region CanXXX

    /// <summary>
    /// 是否允许Fetch
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
    Task<bool> CanFetchAsync<T>();

    /// <summary>
    /// 是否允许Fetch
    /// </summary>
    /// <param name="dataName">数据名, 在服务端注册的实体/实体集合类全名(需实现IEntity/IEntityCollection接口)</param>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
    Task<bool> CanFetchAsync<T>(string dataName);

    /// <summary>
    /// 是否允许Fetch
    /// </summary>
    /// <returns>是否被拒绝</returns>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
    bool CanFetch<T>();

    /// <summary>
    /// 是否允许Fetch
    /// </summary>
    /// <param name="dataName">数据名, 在服务端注册的实体/实体集合类全名(需实现IEntity/IEntityCollection接口)</param>
    /// <returns>是否被拒绝</returns>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
    bool CanFetch<T>(string dataName);

    /// <summary>
    /// 是否允许Create
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
    Task<bool> CanCreateAsync<T>();

    /// <summary>
    /// 是否允许Create
    /// </summary>
    /// <param name="dataName">数据名, 在服务端注册的实体/实体集合类全名(需实现IEntity/IEntityCollection接口)</param>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
    Task<bool> CanCreateAsync<T>(string dataName);

    /// <summary>
    /// 是否允许Create
    /// </summary>
    /// <returns>是否被拒绝</returns>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
    bool CanCreate<T>();

    /// <summary>
    /// 是否允许Create
    /// </summary>
    /// <param name="dataName">数据名, 在服务端注册的实体/实体集合类全名(需实现IEntity/IEntityCollection接口)</param>
    /// <returns>是否被拒绝</returns>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
    bool CanCreate<T>(string dataName);

    /// <summary>
    /// 是否允许Edit
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
    Task<bool> CanEditAsync<T>();

    /// <summary>
    /// 是否允许Edit
    /// </summary>
    /// <param name="dataName">数据名, 在服务端注册的实体/实体集合类全名(需实现IEntity/IEntityCollection接口)</param>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
    Task<bool> CanEditAsync<T>(string dataName);

    /// <summary>
    /// 是否允许Edit
    /// </summary>
    /// <returns>是否被拒绝</returns>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
    bool CanEdit<T>();

    /// <summary>
    /// 是否允许Edit
    /// </summary>
    /// <param name="dataName">数据名, 在服务端注册的实体/实体集合类全名(需实现IEntity/IEntityCollection接口)</param>
    /// <returns>是否被拒绝</returns>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
    bool CanEdit<T>(string dataName);

    /// <summary>
    /// 是否允许Delete
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
    Task<bool> CanDeleteAsync<T>();

    /// <summary>
    /// 是否允许Delete
    /// </summary>
    /// <param name="dataName">数据名, 在服务端注册的实体/实体集合类全名(需实现IEntity/IEntityCollection接口)</param>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
    Task<bool> CanDeleteAsync<T>(string dataName);

    /// <summary>
    /// 是否允许Delete
    /// </summary>
    /// <returns>是否被拒绝</returns>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
    bool CanDelete<T>();

    /// <summary>
    /// 是否允许Delete
    /// </summary>
    /// <param name="dataName">数据名, 在服务端注册的实体/实体集合类全名(需实现IEntity/IEntityCollection接口)</param>
    /// <returns>是否被拒绝</returns>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
    bool CanDelete<T>(string dataName);

    /// <summary>
    /// 是否允许Execute
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
    Task<bool> CanExecuteAsync<T>();

    /// <summary>
    /// 是否允许Execute
    /// </summary>
    /// <param name="serviceName">服务名, 在服务端注册的服务类全名(需实现IService接口)</param>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
    Task<bool> CanExecuteAsync<T>(string serviceName);

    /// <summary>
    /// 是否允许Execute
    /// </summary>
    /// <returns>是否被拒绝</returns>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
    bool CanExecute<T>();

    /// <summary>
    /// 是否允许Execute
    /// </summary>
    /// <param name="serviceName">服务名, 在服务端注册的服务类全名(需实现IService接口)</param>
    /// <returns>是否被拒绝</returns>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
    bool CanExecute<T>(string serviceName);

    #endregion

    #region Fetch

    /// <summary>
    /// 获取实体
    /// </summary>
    /// <param name="id">主键值</param>
    /// <returns>实体</returns>
    Task<T> FetchAsync<T>(long id)
      where T : IEntity;

    /// <summary>
    /// 获取实体
    /// </summary>
    /// <param name="dataName">数据名, 在服务端注册的实体集合类全名(需实现IEntityCollection接口)</param>
    /// <param name="id">主键值</param>
    /// <returns>实体</returns>
    Task<T> FetchAsync<T>(string dataName, long id);

    /// <summary>
    /// 获取实体
    /// </summary>
    /// <param name="id">主键值</param>
    /// <returns>实体</returns>
    T Fetch<T>(long id)
      where T : IEntity;

    /// <summary>
    /// 获取实体
    /// </summary>
    /// <param name="dataName">数据名, 在服务端注册的实体集合类全名(需实现IEntityCollection接口)</param>
    /// <param name="id">主键值</param>
    /// <returns>实体</returns>
    T Fetch<T>(string dataName, long id);

    #endregion

    #region FetchList

    /// <summary>
    /// 获取实体集合
    /// </summary>
    /// <returns>实体集合</returns>
    Task<T> FetchListAsync<T>()
      where T : IEntityCollection;

    /// <summary>
    /// 获取实体集合
    /// </summary>
    /// <param name="pageSize">分页大小</param>
    /// <param name="pageNo">分页号, 从1起始</param>
    /// <returns>实体集合</returns>
    Task<T> FetchListAsync<T>(int pageSize, int pageNo)
      where T : IEntityCollection;

    /// <summary>
    /// 获取实体集合
    /// 如果服务端未定义criteria对象的类则要求服务端的实体类上申明了(ClassAttribute.DefaultCriteriaType)缺省条件类或者实体类程序集里定义有带同名Criteria后缀的条件类(需实现ICriteria接口)
    /// </summary>
    /// <param name="criteria">条件对象</param>
    /// <returns>实体集合</returns>
    Task<T> FetchListAsync<T>(ICriteria criteria)
      where T : IEntityCollection;

    /// <summary>
    /// 获取实体集合
    /// 如果服务端未定义criteria对象的类则要求服务端的实体类上申明了(ClassAttribute.DefaultCriteriaType)缺省条件类或者实体类程序集里定义有带同名Criteria后缀的条件类(需实现ICriteria接口)
    /// </summary>
    /// <param name="criteria">条件对象</param>
    /// <param name="pageSize">分页大小</param>
    /// <param name="pageNo">分页号, 从1起始</param>
    /// <returns>实体集合</returns>
    Task<T> FetchListAsync<T>(ICriteria criteria, int pageSize, int pageNo)
      where T : IEntityCollection;

    /// <summary>
    /// 获取实体集合
    /// </summary>
    /// <param name="dataName">数据名, 在服务端注册的实体集合类全名(需实现IEntityCollection接口)</param>
    /// <param name="criteriaName">条件名, 在服务端定义的条件类全名(需实现ICriteria接口)</param>
    /// <param name="criteria">JSON格式条件对象, 在服务端将被反序列化为criteriaName指定条件类的对象</param>
    /// <param name="pageSize">分页大小</param>
    /// <param name="pageNo">分页号, 从1起始</param>
    /// <returns>实体集合</returns>
    Task<T> FetchListAsync<T>(string dataName, string criteriaName, object criteria, int? pageSize, int? pageNo)
      where T : IList;

    /// <summary>
    /// 获取实体集合
    /// </summary>
    /// <returns>实体集合</returns>
    T FetchList<T>()
      where T : IEntityCollection;

    /// <summary>
    /// 获取实体集合
    /// </summary>
    /// <param name="pageSize">分页大小</param>
    /// <param name="pageNo">分页号, 从1起始</param>
    /// <returns>实体集合</returns>
    T FetchList<T>(int pageSize, int pageNo)
      where T : IEntityCollection;

    /// <summary>
    /// 获取实体集合
    /// 如果服务端未定义criteria对象的类则要求服务端的实体类上申明了(ClassAttribute.DefaultCriteriaType)缺省条件类或者实体类程序集里定义有带同名Criteria后缀的条件类(需实现ICriteria接口)
    /// </summary>
    /// <param name="criteria">条件对象</param>
    /// <returns>实体集合</returns>
    T FetchList<T>(ICriteria criteria)
      where T : IEntityCollection;

    /// <summary>
    /// 获取实体集合
    /// 如果服务端未定义criteria对象的类则要求服务端的实体类上申明了(ClassAttribute.DefaultCriteriaType)缺省条件类或者实体类程序集里定义有带同名Criteria后缀的条件类(需实现ICriteria接口)
    /// </summary>
    /// <param name="criteria">条件对象</param>
    /// <param name="pageSize">分页大小</param>
    /// <param name="pageNo">分页号, 从1起始</param>
    /// <returns>实体集合</returns>
    T FetchList<T>(ICriteria criteria, int pageSize, int pageNo)
      where T : IEntityCollection;

    /// <summary>
    /// 获取实体集合
    /// </summary>
    /// <param name="dataName">数据名, 在服务端注册的实体集合类全名(需实现IEntityCollection接口)</param>
    /// <param name="criteriaName">条件名, 在服务端定义的条件类全名(需实现ICriteria接口)</param>
    /// <param name="criteria">JSON格式条件对象, 在服务端将被反序列化为criteriaName指定条件类的对象</param>
    /// <param name="pageSize">分页大小</param>
    /// <param name="pageNo">分页号, 从1起始</param>
    /// <returns>实体集合</returns>
    T FetchList<T>(string dataName, string criteriaName, object criteria, int? pageSize, int? pageNo)
      where T : IList;
    
    /// <summary>
    /// 获取实体集合
    /// </summary>
    /// <param name="master">主对象</param>
    /// <param name="groupName">分组名, null代表全部</param>
    /// <returns>实体集合</returns>
    Task<T> FetchListAsync<T>(IEntity master, string groupName)
      where T : IEntityCollection;

    /// <summary>
    /// 获取实体集合
    /// </summary>
    /// <param name="master">主对象</param>
    /// <param name="groupName">分组名, null代表全部</param>
    /// <param name="pageSize">分页大小</param>
    /// <param name="pageNo">分页号, 从1起始</param>
    /// <returns>实体集合</returns>
    Task<T> FetchListAsync<T>(IEntity master, string groupName, int pageSize, int pageNo)
      where T : IEntityCollection;


    /// <summary>
    /// 获取实体集合
    /// 如果服务端未定义criteria对象的类则要求服务端的实体类上申明了(ClassAttribute.DefaultCriteriaType)缺省条件类或者实体类程序集里定义有带同名Criteria后缀的条件类(需实现ICriteria接口)
    /// </summary>
    /// <param name="master">主对象</param>
    /// <param name="groupName">分组名, null代表全部</param>
    /// <param name="criteria">条件对象</param>
    /// <returns>实体集合</returns>
    Task<T> FetchListAsync<T>(IEntity master, string groupName, ICriteria criteria)
      where T : IEntityCollection;

    /// <summary>
    /// 获取实体集合
    /// 如果服务端未定义criteria对象的类则要求服务端的实体类上申明了(ClassAttribute.DefaultCriteriaType)缺省条件类或者实体类程序集里定义有带同名Criteria后缀的条件类(需实现ICriteria接口)
    /// </summary>
    /// <param name="master">主对象</param>
    /// <param name="groupName">分组名, null代表全部</param>
    /// <param name="criteria">条件对象</param>
    /// <param name="pageSize">分页大小</param>
    /// <param name="pageNo">分页号, 从1起始</param>
    /// <returns>实体集合</returns>
    Task<T> FetchListAsync<T>(IEntity master, string groupName, ICriteria criteria, int pageSize, int pageNo)
      where T : IEntityCollection;

    /// <summary>
    /// 获取实体集合
    /// </summary>
    /// <param name="dataName">数据名, 在服务端注册的实体集合类全名(需实现IEntityCollection接口)</param>
    /// <param name="masterName">主对象名, 在服务端注册的实体类全名(需实现IEntity接口)</param>
    /// <param name="master">主对象(也可以是主键ID), 在服务端将被反序列化为masterName指定实体类的对象</param>
    /// <param name="groupName">分组名, null代表全部</param>
    /// <param name="criteriaName">条件名, 在服务端定义的条件类全名(需实现ICriteria接口)</param>
    /// <param name="criteria">JSON格式条件对象, 在服务端将被反序列化为criteriaName指定条件类的对象</param>
    /// <param name="pageSize">分页大小</param>
    /// <param name="pageNo">分页号, 从1起始</param>
    /// <returns>实体集合</returns>
    Task<T> FetchListAsync<T>(string dataName, string masterName, object master, string groupName, string criteriaName, object criteria, int? pageSize, int? pageNo)
      where T : IList;

    /// <summary>
    /// 获取实体集合
    /// </summary>
    /// <param name="master">主对象</param>
    /// <param name="groupName">分组名, null代表全部</param>
    /// <returns>实体集合</returns>
    T FetchList<T>(IEntity master, string groupName)
      where T : IEntityCollection;

    /// <summary>
    /// 获取实体集合
    /// </summary>
    /// <param name="master">主对象</param>
    /// <param name="groupName">分组名, null代表全部</param>
    /// <param name="pageSize">分页大小</param>
    /// <param name="pageNo">分页号, 从1起始</param>
    /// <returns>实体集合</returns>
    T FetchList<T>(IEntity master, string groupName, int pageSize, int pageNo)
      where T : IEntityCollection;

    /// <summary>
    /// 获取实体集合
    /// 如果服务端未定义criteria对象的类则要求服务端的实体类上申明了(ClassAttribute.DefaultCriteriaType)缺省条件类或者实体类程序集里定义有带同名Criteria后缀的条件类(需实现ICriteria接口)
    /// </summary>
    /// <param name="master">主对象</param>
    /// <param name="groupName">分组名, null代表全部</param>
    /// <param name="criteria">条件对象</param>
    /// <returns>实体集合</returns>
    T FetchList<T>(IEntity master, string groupName, ICriteria criteria)
      where T : IEntityCollection;

    /// <summary>
    /// 获取实体集合
    /// 如果服务端未定义criteria对象的类则要求服务端的实体类上申明了(ClassAttribute.DefaultCriteriaType)缺省条件类或者实体类程序集里定义有带同名Criteria后缀的条件类(需实现ICriteria接口)
    /// </summary>
    /// <param name="master">主对象</param>
    /// <param name="groupName">分组名, null代表全部</param>
    /// <param name="criteria">条件对象</param>
    /// <param name="pageSize">分页大小</param>
    /// <param name="pageNo">分页号, 从1起始</param>
    /// <returns>实体集合</returns>
    T FetchList<T>(IEntity master, string groupName, ICriteria criteria, int pageSize, int pageNo)
      where T : IEntityCollection;

    /// <summary>
    /// 获取实体集合
    /// </summary>
    /// <param name="dataName">数据名, 在服务端注册的实体集合类全名(需实现IEntityCollection接口)</param>
    /// <param name="masterName">主对象名, 在服务端注册的实体类全名(需实现IEntity接口)</param>
    /// <param name="master">主对象(也可以是主键ID), 在服务端将被反序列化为masterName指定实体类的对象</param>
    /// <param name="groupName">分组名, null代表全部</param>
    /// <param name="criteriaName">条件名, 在服务端定义的条件类全名(需实现ICriteria接口)</param>
    /// <param name="criteria">JSON格式条件对象, 在服务端将被反序列化为criteriaName指定条件类的对象</param>
    /// <param name="pageSize">分页大小</param>
    /// <param name="pageNo">分页号, 从1起始</param>
    /// <returns>实体集合</returns>
    T FetchList<T>(string dataName, string masterName, object master, string groupName, string criteriaName, object criteria, int? pageSize, int? pageNo)
      where T : IList;

    #endregion
    
    #region GetRecordCount

    /// <summary>
    /// 获取记录数量
    /// </summary>
    /// <returns>记录数量</returns>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
    Task<long> GetRecordCountAsync<T>()
      where T : IEntityCollection;

    /// <summary>
    /// 获取记录数量
    /// 如果服务端未定义criteria对象的类则要求服务端的实体类上申明了(ClassAttribute.DefaultCriteriaType)缺省条件类或者实体类程序集里定义有带同名Criteria后缀的条件类(需实现ICriteria接口)
    /// </summary>
    /// <param name="criteria">条件对象</param>
    /// <returns>记录数量</returns>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
    Task<long> GetRecordCountAsync<T>(ICriteria criteria)
      where T : IEntityCollection;

    /// <summary>
    /// 获取记录数量
    /// </summary>
    /// <param name="dataName">数据名, 在服务端注册的实体集合类全名(需实现IEntityCollection接口)</param>
    /// <param name="criteriaName">条件名, 在服务端定义的条件类全名(需实现ICriteria接口)</param>
    /// <param name="criteria">JSON格式条件对象, 在服务端将被反序列化为criteriaName指定条件类的对象</param>
    /// <returns>记录数量</returns>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
    Task<long> GetRecordCountAsync<T>(string dataName, string criteriaName, object criteria)
      where T : IList;

    /// <summary>
    /// 获取记录数量
    /// </summary>
    /// <returns>记录数量</returns>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
    long GetRecordCount<T>()
      where T : IEntityCollection;

    /// <summary>
    /// 获取记录数量
    /// 如果服务端未定义criteria对象的类则要求服务端的实体类上申明了(ClassAttribute.DefaultCriteriaType)缺省条件类或者实体类程序集里定义有带同名Criteria后缀的条件类(需实现ICriteria接口)
    /// </summary>
    /// <param name="criteria">条件对象</param>
    /// <returns>记录数量</returns>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
    long GetRecordCount<T>(ICriteria criteria)
      where T : IEntityCollection;

    /// <summary>
    /// 获取记录数量
    /// </summary>
    /// <param name="dataName">数据名, 在服务端注册的实体集合类全名(需实现IEntityCollection接口)</param>
    /// <param name="criteriaName">条件名, 在服务端定义的条件类全名(需实现ICriteria接口)</param>
    /// <param name="criteria">JSON格式条件对象, 在服务端将被反序列化为criteriaName指定条件类的对象</param>
    /// <returns>记录数量</returns>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
    long GetRecordCount<T>(string dataName, string criteriaName, object criteria)
      where T : IList;

    /// <summary>
    /// 获取记录数量
    /// </summary>
    /// <param name="master">主对象</param>
    /// <param name="groupName">分组名, null代表全部</param>
    /// <returns>记录数量</returns>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
    Task<long> GetRecordCountAsync<T>(IEntity master, string groupName)
      where T : IEntityCollection;

    /// <summary>
    /// 获取记录数量
    /// 如果服务端未定义criteria对象的类则要求服务端的实体类上申明了(ClassAttribute.DefaultCriteriaType)缺省条件类或者实体类程序集里定义有带同名Criteria后缀的条件类(需实现ICriteria接口)
    /// </summary>
    /// <param name="master">主对象</param>
    /// <param name="groupName">分组名, null代表全部</param>
    /// <param name="criteria">条件对象</param>
    /// <returns>记录数量</returns>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
    Task<long> GetRecordCountAsync<T>(IEntity master, string groupName, ICriteria criteria)
      where T : IEntityCollection;

    /// <summary>
    /// 获取记录数量
    /// </summary>
    /// <param name="dataName">数据名, 在服务端注册的实体集合类全名(需实现IEntityCollection接口)</param>
    /// <param name="masterName">主对象名, 在服务端注册的实体类全名(需实现IEntity接口)</param>
    /// <param name="master">主对象(也可以是主键ID), 在服务端将被反序列化为masterName指定实体类的对象</param>
    /// <param name="groupName">分组名, null代表全部</param>
    /// <param name="criteriaName">条件名, 在服务端定义的条件类全名(需实现ICriteria接口)</param>
    /// <param name="criteria">JSON格式条件对象, 在服务端将被反序列化为criteriaName指定条件类的对象</param>
    /// <returns>记录数量</returns>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
    Task<long> GetRecordCountAsync<T>(string dataName, string masterName, object master, string groupName, string criteriaName, object criteria)
      where T : IList;

    /// <summary>
    /// 获取记录数量
    /// </summary>
    /// <param name="master">主对象</param>
    /// <param name="groupName">分组名, null代表全部</param>
    /// <returns>记录数量</returns>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
    long GetRecordCount<T>(IEntity master, string groupName)
      where T : IEntityCollection;

    /// <summary>
    /// 获取记录数量
    /// 如果服务端未定义criteria对象的类则要求服务端的实体类上申明了(ClassAttribute.DefaultCriteriaType)缺省条件类或者实体类程序集里定义有带同名Criteria后缀的条件类(需实现ICriteria接口)
    /// </summary>
    /// <param name="master">主对象</param>
    /// <param name="groupName">分组名, null代表全部</param>
    /// <param name="criteria">条件对象</param>
    /// <returns>记录数量</returns>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
    long GetRecordCount<T>(IEntity master, string groupName, ICriteria criteria)
      where T : IEntityCollection;

    /// <summary>
    /// 获取记录数量
    /// </summary>
    /// <param name="dataName">数据名, 在服务端注册的实体集合类全名(需实现IEntityCollection接口)</param>
    /// <param name="masterName">主对象名, 在服务端注册的实体类全名(需实现IEntity接口)</param>
    /// <param name="master">主对象(也可以是主键ID), 在服务端将被反序列化为masterName指定实体类的对象</param>
    /// <param name="groupName">分组名, null代表全部</param>
    /// <param name="criteriaName">条件名, 在服务端定义的条件类全名(需实现ICriteria接口)</param>
    /// <param name="criteria">JSON格式条件对象, 在服务端将被反序列化为criteriaName指定条件类的对象</param>
    /// <returns>记录数量</returns>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
    long GetRecordCount<T>(string dataName, string masterName, object master, string groupName, string criteriaName, string criteria)
      where T : IList;

    #endregion
    
    #region Save

    /// <summary>
    /// 提交实体
    /// </summary>
    /// <param name="entity">实体</param>
    /// <returns>成功数量</returns>
    Task<int> SaveAsync(IEntity entity);

    /// <summary>
    /// 提交实体
    /// </summary>
    /// <param name="dataName">数据名, 在服务端注册的实体类全名(需实现IEntity接口)</param>
    /// <param name="entity">实体</param>
    /// <returns>成功数量</returns>
    Task<int> SaveAsync(string dataName, IEntity entity);

    /// <summary>
    /// 提交实体
    /// </summary>
    /// <param name="entity">实体</param>
    /// <returns>成功数量</returns>
    int Save(IEntity entity);


    /// <summary>
    /// 提交实体
    /// </summary>
    /// <param name="dataName">数据名, 在服务端注册的实体类全名(需实现IEntity接口)</param>
    /// <param name="entity">实体</param>
    /// <returns>成功数量</returns>
    int Save(string dataName, IEntity entity);

    /// <summary>
    /// 提交实体集合
    /// </summary>
    /// <param name="entityCollection">实体集合</param>
    /// <returns>成功数量</returns>
    Task<int> SaveAsync(IEntityCollection entityCollection);

    /// <summary>
    /// 提交实体集合
    /// </summary>
    /// <param name="dataName">数据名, 在服务端注册的实体集合类/实体类全名(需实现IEntityCollection/IEntity接口)</param>
    /// <param name="entityCollection">实体集合</param>
    /// <returns>成功数量</returns>
    Task<int> SaveAsync(string dataName, IEntityCollection entityCollection);

    /// <summary>
    /// 提交实体集合
    /// </summary>
    /// <param name="entityCollection">实体集合</param>
    /// <returns>成功数量</returns>
    int Save(IEntityCollection entityCollection);

    /// <summary>
    /// 提交实体集合
    /// </summary>
    /// <param name="dataName">数据名, 在服务端注册的实体集合类/实体类全名(需实现IEntityCollection/IEntity接口)</param>
    /// <param name="entityCollection">实体集合</param>
    /// <returns>成功数量</returns>
    int Save(string dataName, IEntityCollection entityCollection);

    /// <summary>
    /// 提交对象
    /// </summary>
    /// <param name="dataName">数据名, 在服务端注册的实体集合类/实体类全名(需实现IEntityCollection/IEntity接口)</param>
    /// <param name="obj">对象, 将被序列化传到服务端, 需包含IsNew、IsSelfDeleted、IsSelfDirty属性以指明对象状态</param>
    /// <returns>成功数量</returns>
    Task<int> SaveAsync(string dataName, object obj);

    /// <summary>
    /// 提交对象
    /// </summary>
    /// <param name="dataName">数据名, 在服务端注册的实体集合类/实体类全名(需实现IEntityCollection/IEntity接口)</param>
    /// <param name="obj">对象, 将被序列化传到服务端, 需包含IsNew、IsSelfDeleted、IsSelfDirty属性以指明对象状态</param>
    /// <returns>成功数量</returns>
    int Save(string dataName, object obj);

    #endregion

    #region Execute

    /// <summary>
    /// 执行服务
    /// </summary>
    /// <param name="service">服务对象, 将被序列化传到服务端</param>
    /// <returns>服务结果</returns>
    Task<T> ExecuteAsync<T>(T service)
      where T : IService;


    /// <summary>
    /// 执行服务
    /// </summary>
    /// <param name="serviceName">服务名, 在服务端注册的服务类全名(需实现IService接口)</param>
    /// <param name="obj">JSON格式对象, 将被传到服务端</param>
    /// <returns>服务结果</returns>
    Task<object> ExecuteAsync(string serviceName, object obj);

    /// <summary>
    /// 执行服务
    /// </summary>
    /// <param name="service">服务对象, 将被序列化传到服务端</param>
    /// <returns>服务结果</returns>
    T Execute<T>(T service)
      where T : IService;

    /// <summary>
    /// 执行服务
    /// </summary>
    /// <param name="serviceName">服务名, 在服务端注册的服务类全名(需实现IService接口)</param>
    /// <param name="obj">对象, 将被序列化传到服务端</param>
    /// <returns>服务结果</returns>
    object Execute(string serviceName, object obj);

    #endregion

    #region UploadFiles

    /// <summary>
    /// 上传文件
    /// </summary>
    /// <param name="service">服务对象, 将被序列化传到服务端</param>
    /// <param name="fileNames">待上传的文件路径</param>
    /// <returns>服务结果</returns>
    Task<T> UploadFilesAsync<T>(T service, params string[] fileNames)
      where T : IService;
    
    /// <summary>
    /// 上传文件
    /// </summary>
    /// <param name="serviceName">服务名, 在服务端注册的服务类全名(需实现IService接口)</param>
    /// <param name="obj">JSON格式对象, 将被传到服务端</param>
    /// <param name="fileNames">待上传的文件路径</param>
    /// <returns>服务结果</returns>
    Task<object> UploadFilesAsync(string serviceName, object obj, params string[] fileNames);

    /// <summary>
    /// 上传文件
    /// </summary>
    /// <param name="service">服务对象, 将被序列化传到服务端</param>
    /// <param name="fileNames">待上传的文件路径</param>
    /// <returns>服务结果</returns>
    T UploadFiles<T>(T service, params string[] fileNames)
      where T : IService;

    /// <summary>
    /// 上传文件
    /// </summary>
    /// <param name="serviceName">服务名, 在服务端注册的服务类全名(需实现IService接口)</param>
    /// <param name="obj">对象, 将被序列化传到服务端</param>
    /// <param name="fileNames">待上传的文件路径</param>
    /// <returns>服务结果</returns>
    object UploadFiles(string serviceName, object obj, params string[] fileNames);

    /// <summary>
    /// 上传文件
    /// </summary>
    /// <param name="service">服务对象, 将被序列化传到服务端</param>
    /// <param name="fileStreams">待上传的文件流</param>
    /// <returns>服务结果</returns>
    Task<T> UploadFilesAsync<T>(T service, IDictionary<string, Stream> fileStreams)
      where T : IService;

    /// <summary>
    /// 上传文件
    /// </summary>
    /// <param name="serviceName">服务名, 在服务端注册的服务类全名(需实现IService接口)</param>
    /// <param name="obj">JSON格式对象, 将被传到服务端</param>
    /// <param name="fileStreams">待上传的文件流</param>
    /// <returns>服务结果</returns>
    Task<object> UploadFilesAsync(string serviceName, object obj, IDictionary<string, Stream> fileStreams);

    /// <summary>
    /// 上传文件
    /// </summary>
    /// <param name="service">服务对象, 将被序列化传到服务端</param>
    /// <param name="fileStreams">待上传的文件流</param>
    /// <returns>服务结果</returns>
    T UploadFiles<T>(T service, IDictionary<string, Stream> fileStreams)
      where T : IService;

    /// <summary>
    /// 上传文件
    /// </summary>
    /// <param name="serviceName">服务名, 在服务端注册的服务类全名(需实现IService接口)</param>
    /// <param name="obj">对象, 将被序列化传到服务端</param>
    /// <param name="fileStreams">待上传的文件流</param>
    /// <returns>服务结果</returns>
    object UploadFiles(string serviceName, object obj, IDictionary<string, Stream> fileStreams);

    #endregion

    #region UploadBigFile

    /// <summary>
    /// 上传大文件
    /// </summary>
    /// <param name="service">服务对象, 将被序列化传到服务端</param>
    /// <param name="fileName">待上传的文件路径</param>
    /// <param name="doProgress">执行进度干预</param>
    /// <returns>服务结果</returns>
    Task<T> UploadBigFileAsync<T>(T service, string fileName, Func<object, FileChunkInfo, bool> doProgress)
      where T : IService;

    /// <summary>
    /// 上传大文件
    /// </summary>
    /// <param name="serviceName">服务名, 在服务端注册的服务类全名(需实现IService接口)</param>
    /// <param name="obj">JSON格式对象, 将被传到服务端</param>
    /// <param name="fileName">待上传的文件路径</param>
    /// <param name="doProgress">执行进度干预</param>
    /// <returns>服务结果</returns>
    Task<object> UploadBigFileAsync(string serviceName, object obj, string fileName, Func<object, FileChunkInfo, bool> doProgress);

    /// <summary>
    /// 上传大文件
    /// </summary>
    /// <param name="service">服务对象, 将被序列化传到服务端</param>
    /// <param name="fileName">待上传的文件路径</param>
    /// <param name="doProgress">执行进度干预</param>
    /// <returns>服务结果</returns>
    T UploadBigFile<T>(T service, string fileName, Func<object, FileChunkInfo, bool> doProgress)
      where T : IService;

    /// <summary>
    /// 上传大文件
    /// </summary>
    /// <param name="serviceName">服务名, 在服务端注册的服务类全名(需实现IService接口)</param>
    /// <param name="obj">对象, 将被序列化传到服务端</param>
    /// <param name="fileName">待上传的文件路径</param>
    /// <param name="doProgress">执行进度干预</param>
    /// <returns>服务结果</returns>
    object UploadBigFile(string serviceName, object obj, string fileName, Func<object, FileChunkInfo, bool> doProgress);

    /// <summary>
    /// 上传大文件
    /// </summary>
    /// <param name="service">服务对象, 将被序列化传到服务端</param>
    /// <param name="fileName">待上传的文件名</param>
    /// <param name="fileStream">待上传的文件流</param>
    /// <param name="doProgress">执行进度干预</param>
    /// <returns>服务结果</returns>
    Task<T> UploadBigFileAsync<T>(T service, string fileName, Stream fileStream, Func<object, FileChunkInfo, bool> doProgress)
      where T : IService;

    /// <summary>
    /// 上传大文件
    /// </summary>
    /// <param name="serviceName">服务名, 在服务端注册的服务类全名(需实现IService接口)</param>
    /// <param name="obj">JSON格式对象, 将被传到服务端</param>
    /// <param name="fileName">待上传的文件名</param>
    /// <param name="fileStream">待上传的文件流</param>
    /// <param name="doProgress">执行进度干预</param>
    /// <returns>服务结果</returns>
    Task<object> UploadBigFileAsync(string serviceName, object obj, string fileName, Stream fileStream, Func<object, FileChunkInfo, bool> doProgress);

    /// <summary>
    /// 上传大文件
    /// </summary>
    /// <param name="service">服务对象, 将被序列化传到服务端</param>
    /// <param name="fileName">待上传的文件名</param>
    /// <param name="fileStream">待上传的文件流</param>
    /// <param name="doProgress">执行进度干预</param>
    /// <returns>服务结果</returns>
    T UploadBigFile<T>(T service, string fileName, Stream fileStream, Func<object, FileChunkInfo, bool> doProgress)
      where T : IService;

    /// <summary>
    /// 上传文件
    /// </summary>
    /// <param name="serviceName">服务名, 在服务端注册的服务类全名(需实现IService接口)</param>
    /// <param name="obj">对象, 将被序列化传到服务端</param>
    /// <param name="fileName">待上传的文件名</param>
    /// <param name="fileStream">待上传的文件流</param>
    /// <param name="doProgress">执行进度干预</param>
    /// <returns>服务结果</returns>
    object UploadBigFile(string serviceName, object obj, string fileName, Stream fileStream, Func<object, FileChunkInfo, bool> doProgress);

    /// <summary>
    /// 上传大文件
    /// </summary>
    /// <param name="service">服务对象, 将被序列化传到服务端</param>
    /// <param name="fileChunkInfo">待处理的文件块信息</param>
    /// <returns>服务结果</returns>
    Task<T> UploadBigFileAsync<T>(T service, FileChunkInfo fileChunkInfo)
      where T : IService;

    /// <summary>
    /// 上传大文件
    /// </summary>
    /// <param name="serviceName">服务名, 在服务端注册的服务类全名(需实现IService接口)</param>
    /// <param name="obj">JSON格式对象, 将被传到服务端</param>
    /// <param name="fileChunkInfo">待处理的文件块信息</param>
    /// <returns>服务结果</returns>
    Task<object> UploadBigFileAsync(string serviceName, object obj, FileChunkInfo fileChunkInfo);

    /// <summary>
    /// 上传大文件
    /// </summary>
    /// <param name="service">服务对象, 将被序列化传到服务端</param>
    /// <param name="fileChunkInfo">待处理的文件块信息</param>
    /// <returns>服务结果</returns>
    T UploadBigFile<T>(T service, FileChunkInfo fileChunkInfo)
      where T : IService;

    /// <summary>
    /// 上传文件
    /// </summary>
    /// <param name="serviceName">服务名, 在服务端注册的服务类全名(需实现IService接口)</param>
    /// <param name="obj">对象, 将被序列化传到服务端</param>
    /// <param name="fileChunkInfo">待处理的文件块信息</param>
    /// <returns>服务结果</returns>
    object UploadBigFile(string serviceName, object obj, FileChunkInfo fileChunkInfo);

    #endregion

    #region DownloadFile

    /// <summary>
    /// 下载文件
    /// </summary>
    /// <param name="service">服务对象, 将被序列化传到服务端</param>
    /// <param name="fileName">待保存的文件路径</param>
    Task DownloadFileAsync<T>(T service, string fileName)
      where T : IService;

    /// <summary>
    /// 下载文件
    /// </summary>
    /// <param name="serviceName">服务名, 在服务端注册的服务类全名(需实现IService接口)</param>
    /// <param name="obj">JSON格式对象, 将被传到服务端</param>
    /// <param name="fileName">待保存的文件路径</param>
    Task DownloadFileAsync(string serviceName, object obj, string fileName);

    /// <summary>
    /// 下载文件
    /// </summary>
    /// <param name="service">服务对象, 将被序列化传到服务端</param>
    /// <param name="fileName">待保存的文件路径</param>
    void DownloadFile<T>(T service, string fileName)
      where T : IService;

    /// <summary>
    /// 下载文件
    /// </summary>
    /// <param name="serviceName">服务名, 在服务端注册的服务类全名(需实现IService接口)</param>
    /// <param name="obj">对象, 将被序列化传到服务端</param>
    /// <param name="fileName">待保存的文件路径</param>
    void DownloadFile(string serviceName, object obj, string fileName);

    /// <summary>
    /// 下载文件
    /// </summary>
    /// <param name="service">服务对象, 将被序列化传到服务端</param>
    /// <returns>已下载的文件流</returns>
    Task<Stream> DownloadFileAsync<T>(T service)
      where T : IService;

    /// <summary>
    /// 下载文件
    /// </summary>
    /// <param name="serviceName">服务名, 在服务端注册的服务类全名(需实现IService接口)</param>
    /// <param name="obj">JSON格式对象, 将被传到服务端</param>
    /// <returns>已下载的文件流</returns>
    Task<Stream> DownloadFileAsync(string serviceName, object obj);

    /// <summary>
    /// 下载文件
    /// </summary>
    /// <param name="service">服务对象, 将被序列化传到服务端</param>
    /// <returns>已下载的文件流</returns>
    Stream DownloadFile<T>(T service)
      where T : IService;

    /// <summary>
    /// 下载文件
    /// </summary>
    /// <param name="serviceName">服务名, 在服务端注册的服务类全名(需实现IService接口)</param>
    /// <param name="obj">对象, 将被序列化传到服务端</param>
    /// <returns>已下载的文件流</returns>
    Stream DownloadFile(string serviceName, object obj);

    #endregion

    #region DownloadBigFile

    /// <summary>
    /// 下载大文件
    /// </summary>
    /// <param name="service">服务对象, 将被序列化传到服务端</param>
    /// <param name="fileName">待保存的文件路径</param>
    /// <param name="doProgress">执行进度干预</param>
    Task DownloadBigFileAsync<T>(T service, string fileName, Func<object, FileChunkInfo, bool> doProgress)
      where T : IService;

    /// <summary>
    /// 下载大文件
    /// </summary>
    /// <param name="serviceName">服务名, 在服务端注册的服务类全名(需实现IService接口)</param>
    /// <param name="obj">JSON格式对象, 将被传到服务端</param>
    /// <param name="fileName">待保存的文件路径</param>
    /// <param name="doProgress">执行进度干预</param>
    Task DownloadBigFileAsync(string serviceName, object obj, string fileName, Func<object, FileChunkInfo, bool> doProgress);

    /// <summary>
    /// 下载大文件
    /// </summary>
    /// <param name="service">服务对象, 将被序列化传到服务端</param>
    /// <param name="fileName">待保存的文件路径</param>
    /// <param name="doProgress">执行进度干预</param>
    void DownloadBigFile<T>(T service, string fileName, Func<object, FileChunkInfo, bool> doProgress)
      where T : IService;

    /// <summary>
    /// 下载大文件
    /// </summary>
    /// <param name="serviceName">服务名, 在服务端注册的服务类全名(需实现IService接口)</param>
    /// <param name="obj">对象, 将被序列化传到服务端</param>
    /// <param name="fileName">待保存的文件路径</param>
    /// <param name="doProgress">执行进度干预</param>
    void DownloadBigFile(string serviceName, object obj, string fileName, Func<object, FileChunkInfo, bool> doProgress);
    
    /// <summary>
    /// 下载大文件
    /// </summary>
    /// <param name="service">服务对象, 将被序列化传到服务端</param>
    /// <param name="fileStream">待保存的文件流</param>
    /// <param name="doProgress">执行进度干预</param>
    Task DownloadBigFileAsync<T>(T service, Stream fileStream, Func<object, FileChunkInfo, bool> doProgress)
      where T : IService;

    /// <summary>
    /// 下载大文件
    /// </summary>
    /// <param name="serviceName">服务名, 在服务端注册的服务类全名(需实现IService接口)</param>
    /// <param name="obj">对象, 将被序列化传到服务端</param>
    /// <param name="fileStream">待保存的文件流</param>
    /// <param name="doProgress">执行进度干预</param>
    Task DownloadBigFileAsync(string serviceName, object obj, Stream fileStream, Func<object, FileChunkInfo, bool> doProgress);

    /// <summary>
    /// 下载大文件
    /// </summary>
    /// <param name="service">服务对象, 将被序列化传到服务端</param>
    /// <param name="fileStream">待保存的文件流</param>
    /// <param name="doProgress">执行进度干预</param>
    void DownloadBigFile<T>(T service, Stream fileStream, Func<object, FileChunkInfo, bool> doProgress)
      where T : IService;

    /// <summary>
    /// 下载大文件
    /// </summary>
    /// <param name="serviceName">服务名, 在服务端注册的服务类全名(需实现IService接口)</param>
    /// <param name="obj">对象, 将被序列化传到服务端</param>
    /// <param name="fileStream">待保存的文件流</param>
    /// <param name="doProgress">执行进度干预</param>
    void DownloadBigFile(string serviceName, object obj, Stream fileStream, Func<object, FileChunkInfo, bool> doProgress);

    /// <summary>
    /// 下载文件
    /// </summary>
    /// <param name="service">服务对象, 将被序列化传到服务端</param>
    /// <param name="chunkNumber">块号</param>
    /// <returns>文件块信息</returns>
    Task<FileChunkInfo> DownloadBigFileAsync<T>(T service, int chunkNumber)
      where T : IService;

    /// <summary>
    /// 下载文件
    /// </summary>
    /// <param name="serviceName">服务名, 在服务端注册的服务类全名(需实现IService接口)</param>
    /// <param name="obj">JSON格式对象, 将被传到服务端</param>
    /// <param name="chunkNumber">块号</param>
    /// <returns>文件块信息</returns>
    Task<FileChunkInfo> DownloadBigFileAsync(string serviceName, object obj, int chunkNumber);

    /// <summary>
    /// 下载文件
    /// </summary>
    /// <param name="service">服务对象, 将被序列化传到服务端</param>
    /// <param name="chunkNumber">块号</param>
    /// <returns>文件块信息</returns>
    FileChunkInfo DownloadBigFile<T>(T service, int chunkNumber)
      where T : IService;

    /// <summary>
    /// 下载文件
    /// </summary>
    /// <param name="serviceName">服务名, 在服务端注册的服务类全名(需实现IService接口)</param>
    /// <param name="obj">对象, 将被序列化传到服务端</param>
    /// <param name="chunkNumber">块号</param>
    /// <returns>文件块信息</returns>
    FileChunkInfo DownloadBigFile(string serviceName, object obj, int chunkNumber);

    #endregion

    #endregion
  }
}
