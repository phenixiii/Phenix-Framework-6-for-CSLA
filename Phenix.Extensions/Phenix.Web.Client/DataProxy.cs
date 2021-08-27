using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Phenix.Core.IO;
using Phenix.Core.Mapping;
using Phenix.Core.Web;

namespace Phenix.Web.Client
{
  /// <summary>
  /// 数据代理
  /// </summary>
  public sealed class DataProxy : IDataProxy
  {
    /// <summary>
    /// 初始化
    /// </summary>
    public DataProxy(HttpClient httpClient)
    {
      _httpClient = httpClient;
    }

    #region 属性

    private const string DATA_URI = "/api/Data";

    private readonly HttpClient _httpClient;
    /// <summary>
    /// HttpClient
    /// </summary>
    public HttpClient HttpClient
    {
      get { return _httpClient; }
    }
    System.Net.Http.HttpClient IDataProxy.HttpClient
    {
      get { return HttpClient; }
    }

    #endregion

    #region 方法

    internal static string EscapeDataName(string dataName, Type dataType)
    {
#if Clip
      if (String.IsNullOrEmpty(dataName))
        dataName = ClassMapInfo.GetClassSourceName(dataType);
#endif
      return Uri.EscapeDataString(String.IsNullOrEmpty(dataName) ? dataType.FullName : dataName);
    }

    private static string EscapeCriteriaName(string criteriaName, Type criteriaType)
    {
#if Clip
      if (String.IsNullOrEmpty(criteriaName))
        criteriaName = ClassMapInfo.GetClassSourceName(criteriaType);
#endif
      return Uri.EscapeDataString(String.IsNullOrEmpty(criteriaName) ? criteriaType.FullName : criteriaName);
    }

    private static string EscapeMasterName(string masterName, Type masterType)
    {
#if Clip
      if (String.IsNullOrEmpty(masterName))
        masterName = ClassMapInfo.GetClassSourceName(masterType);
#endif
      return Uri.EscapeDataString(String.IsNullOrEmpty(masterName) ? masterType.FullName : masterName);
    }

    #region Sequence

    /// <summary>
    /// 获取64位序号
    /// </summary>
    /// <returns>64位序号</returns>
    public async Task<long> FetchSequenceValueAsync()
    {
      using (HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, DATA_URI))
      using (HttpResponseMessage response = await HttpClient.SendAsync(request))
      {
        string result = await response.Content.ReadAsStringAsync();
        if (response.StatusCode != HttpStatusCode.OK)
          throw new HttpRequestException(result);
        return Int64.Parse(result);
      }
    }

    /// <summary>
    /// 获取64位序号
    /// </summary>
    /// <returns>64位序号</returns>
    public long FetchSequenceValue()
    {
      try
      {
        return FetchSequenceValueAsync().Result;
      }
      catch (AggregateException ex)
      {
        throw ex.InnerException;
      }
    }

    #endregion

    #region CanXXX

    /// <summary>
    /// 是否允许Fetch
    /// </summary>
    public async Task<bool> CanFetchAsync<T>()
    {
      bool result = await HttpClient.SecurityProxy.IsByDenyAsync<T>(ExecuteAction.Fetch);
      return !result;
    }

    /// <summary>
    /// 是否允许Fetch
    /// </summary>
    /// <param name="dataName">数据名, 在服务端注册的实体/实体集合类全名(需实现IEntity/IEntityCollection接口)</param>
    public async Task<bool> CanFetchAsync<T>(string dataName)
    {
      bool result = await HttpClient.SecurityProxy.IsByDenyAsync<T>(dataName, ExecuteAction.Fetch);
      return !result;
    }
    
    /// <summary>
    /// 是否允许Fetch
    /// </summary>
    /// <returns>是否被拒绝</returns>
    public bool CanFetch<T>()
    {
      try
      {
        return CanFetchAsync<T>().Result;
      }
      catch (AggregateException ex)
      {
        throw ex.InnerException;
      }
    }

    /// <summary>
    /// 是否允许Fetch
    /// </summary>
    /// <param name="dataName">数据名, 在服务端注册的实体/实体集合类全名(需实现IEntity/IEntityCollection接口)</param>
    /// <returns>是否被拒绝</returns>
    public bool CanFetch<T>(string dataName)
    {
      try
      {
        return CanFetchAsync<T>(dataName).Result;
      }
      catch (AggregateException ex)
      {
        throw ex.InnerException;
      }
    }

    /// <summary>
    /// 是否允许Create
    /// </summary>
    public async Task<bool> CanCreateAsync<T>()
    {
      bool result = await HttpClient.SecurityProxy.IsByDenyAsync<T>(ExecuteAction.Insert);
      return !result;
    }

    /// <summary>
    /// 是否允许Create
    /// </summary>
    /// <param name="dataName">数据名, 在服务端注册的实体/实体集合类全名(需实现IEntity/IEntityCollection接口)</param>
    public async Task<bool> CanCreateAsync<T>(string dataName)
    {
      bool result = await HttpClient.SecurityProxy.IsByDenyAsync<T>(dataName, ExecuteAction.Insert);
      return !result;
    }

    /// <summary>
    /// 是否允许Create
    /// </summary>
    /// <returns>是否被拒绝</returns>
    public bool CanCreate<T>()
    {
      try
      {
        return CanCreateAsync<T>().Result;
      }
      catch (AggregateException ex)
      {
        throw ex.InnerException;
      }
    }

    /// <summary>
    /// 是否允许Create
    /// </summary>
    /// <param name="dataName">数据名, 在服务端注册的实体/实体集合类全名(需实现IEntity/IEntityCollection接口)</param>
    /// <returns>是否被拒绝</returns>
    public bool CanCreate<T>(string dataName)
    {
      try
      {
        return CanCreateAsync<T>(dataName).Result;
      }
      catch (AggregateException ex)
      {
        throw ex.InnerException;
      }
    }

    /// <summary>
    /// 是否允许Edit
    /// </summary>
    public async Task<bool> CanEditAsync<T>()
    {
      bool result = await HttpClient.SecurityProxy.IsByDenyAsync<T>(ExecuteAction.Update);
      return !result;
    }

    /// <summary>
    /// 是否允许Edit
    /// </summary>
    /// <param name="dataName">数据名, 在服务端注册的实体/实体集合类全名(需实现IEntity/IEntityCollection接口)</param>
    public async Task<bool> CanEditAsync<T>(string dataName)
    {
      bool result = await HttpClient.SecurityProxy.IsByDenyAsync<T>(dataName, ExecuteAction.Update);
      return !result;
    }

    /// <summary>
    /// 是否允许Edit
    /// </summary>
    /// <returns>是否被拒绝</returns>
    public bool CanEdit<T>()
    {
      try
      {
        return CanEditAsync<T>().Result;
      }
      catch (AggregateException ex)
      {
        throw ex.InnerException;
      }
    }

    /// <summary>
    /// 是否允许Edit
    /// </summary>
    /// <param name="dataName">数据名, 在服务端注册的实体/实体集合类全名(需实现IEntity/IEntityCollection接口)</param>
    /// <returns>是否被拒绝</returns>
    public bool CanEdit<T>(string dataName)
    {
      try
      {
        return CanEditAsync<T>(dataName).Result;
      }
      catch (AggregateException ex)
      {
        throw ex.InnerException;
      }
    }

    /// <summary>
    /// 是否允许Delete
    /// </summary>
    public async Task<bool> CanDeleteAsync<T>()
    {
      bool result = await HttpClient.SecurityProxy.IsByDenyAsync<T>(ExecuteAction.Delete);
      return !result;
    }

    /// <summary>
    /// 是否允许Delete
    /// </summary>
    /// <param name="dataName">数据名, 在服务端注册的实体/实体集合类全名(需实现IEntity/IEntityCollection接口)</param>
    public async Task<bool> CanDeleteAsync<T>(string dataName)
    {
      bool result = await HttpClient.SecurityProxy.IsByDenyAsync<T>(dataName, ExecuteAction.Delete);
      return !result;
    }

    /// <summary>
    /// 是否允许Delete
    /// </summary>
    /// <returns>是否被拒绝</returns>
    public bool CanDelete<T>()
    {
      try
      {
        return CanDeleteAsync<T>().Result;
      }
      catch (AggregateException ex)
      {
        throw ex.InnerException;
      }
    }

    /// <summary>
    /// 是否允许Delete
    /// </summary>
    /// <param name="dataName">数据名, 在服务端注册的实体/实体集合类全名(需实现IEntity/IEntityCollection接口)</param>
    /// <returns>是否被拒绝</returns>
    public bool CanDelete<T>(string dataName)
    {
      try
      {
        return CanDeleteAsync<T>(dataName).Result;
      }
      catch (AggregateException ex)
      {
        throw ex.InnerException;
      }
    }

    /// <summary>
    /// 是否允许Execute
    /// </summary>
    public async Task<bool> CanExecuteAsync<T>()
    {
      bool result = await HttpClient.SecurityProxy.IsByDenyAsync<T>(ExecuteAction.Update);
      return !result;
    }

    /// <summary>
    /// 是否允许Execute
    /// </summary>
    /// <param name="serviceName">服务名, 在服务端注册的服务类全名(需实现IService接口)</param>
    public async Task<bool> CanExecuteAsync<T>(string serviceName)
    {
      bool result = await HttpClient.SecurityProxy.IsByDenyAsync<T>(serviceName, ExecuteAction.Update);
      return !result;
    }

    /// <summary>
    /// 是否允许Execute
    /// </summary>
    /// <returns>是否被拒绝</returns>
    public bool CanExecute<T>()
    {
      try
      {
        return CanExecuteAsync<T>().Result;
      }
      catch (AggregateException ex)
      {
        throw ex.InnerException;
      }
    }
    
    /// <summary>
    /// 是否允许Execute
    /// </summary>
    /// <param name="serviceName">服务名, 在服务端注册的服务类全名(需实现IService接口)</param>
    /// <returns>是否被拒绝</returns>
    public bool CanExecute<T>(string serviceName)
    {
      try
      {
        return CanExecuteAsync<T>(serviceName).Result;
      }
      catch (AggregateException ex)
      {
        throw ex.InnerException;
      }
    }

    #endregion

    #region Fetch

    /// <summary>
    /// 获取实体
    /// </summary>
    /// <param name="id">主键值</param>
    /// <returns>实体</returns>
    public async Task<T> FetchAsync<T>(long id)
      where T : IEntity
    {
      return await FetchAsync<T>(String.Empty, id);
    }

    /// <summary>
    /// 获取实体
    /// </summary>
    /// <param name="dataName">数据名, 在服务端注册的实体集合类全名(需实现IEntity接口)</param>
    /// <param name="id">主键值</param>
    /// <returns>实体</returns>
    public async Task<T> FetchAsync<T>(string dataName, long id)
    {
      using (HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get,
        String.Format("{0}/{1}?id={2}&pageSize=&pageNo=",
          DATA_URI, 
          EscapeDataName(dataName, typeof(T)), 
          id)))
      using (HttpResponseMessage response = await HttpClient.SendAsync(request))
      {
        string result = await response.Content.ReadAsStringAsync();
        if (response.StatusCode != HttpStatusCode.OK)
          throw new HttpRequestException(result);
        return EntityHelper.JsonDeserialize<T>(result);
      }
    }

    /// <summary>
    /// 获取实体
    /// </summary>
    /// <param name="id">主键值</param>
    /// <returns>实体</returns>
    public T Fetch<T>(long id)
      where T : IEntity
    {
      try
      {
        return FetchAsync<T>(id).Result;
      }
      catch (AggregateException ex)
      {
        throw ex.InnerException;
      }
    }

    /// <summary>
    /// 获取实体
    /// </summary>
    /// <param name="dataName">数据名, 在服务端注册的实体集合类全名(需实现IEntity接口)</param>
    /// <param name="id">主键值</param>
    /// <returns>实体</returns>
    public T Fetch<T>(string dataName, long id)
    {
      try
      {
        return FetchAsync<T>(dataName, id).Result;
      }
      catch (AggregateException ex)
      {
        throw ex.InnerException;
      }
    }

    #endregion

    #region FetchList

    /// <summary>
    /// 获取实体集合
    /// </summary>
    /// <returns>实体集合</returns>
    public async Task<T> FetchListAsync<T>()
      where T : IEntityCollection
    {
      return await FetchListAsync<T>(String.Empty, String.Empty, null, null, null);
    }

    /// <summary>
    /// 获取实体集合
    /// </summary>
    /// <param name="pageSize">分页大小</param>
    /// <param name="pageNo">分页号, 从1起始</param>
    /// <returns>实体集合</returns>
    public async Task<T> FetchListAsync<T>(int pageSize, int pageNo)
      where T : IEntityCollection
    {
      return await FetchListAsync<T>(String.Empty, String.Empty, null, pageSize, pageNo);
    }

    /// <summary>
    /// 获取实体集合
    /// 如果服务端未定义criteria对象的类则要求服务端的实体类上申明了(ClassAttribute.DefaultCriteriaType)缺省条件类或者实体类程序集里定义有带同名Criteria后缀的条件类(需实现ICriteria接口)
    /// </summary>
    /// <param name="criteria">条件对象</param>
    /// <returns>实体集合</returns>
    public async Task<T> FetchListAsync<T>(ICriteria criteria)
      where T : IEntityCollection
    {
      return await FetchListAsync<T>(String.Empty, String.Empty, criteria, null, null);
    }

    /// <summary>
    /// 获取实体集合
    /// 如果服务端未定义criteria对象的类则要求服务端的实体类上申明了(ClassAttribute.DefaultCriteriaType)缺省条件类或者实体类程序集里定义有带同名Criteria后缀的条件类(需实现ICriteria接口)
    /// </summary>
    /// <param name="criteria">条件对象</param>
    /// <param name="pageSize">分页大小</param>
    /// <param name="pageNo">分页号, 从1起始</param>
    /// <returns>实体集合</returns>
    public async Task<T> FetchListAsync<T>(ICriteria criteria, int pageSize, int pageNo)
      where T : IEntityCollection
    {
      return await FetchListAsync<T>(String.Empty, String.Empty, criteria, pageSize, pageNo);
    }

    /// <summary>
    /// 获取实体集合
    /// </summary>
    /// <param name="dataName">数据名, 在服务端注册的实体集合类全名(需实现IEntityCollection接口)</param>
    /// <param name="criteriaName">条件名, 在服务端定义的条件类全名(需实现ICriteria接口)</param>
    /// <param name="criteria">JSON格式条件对象, 在服务端将被反序列化为criteriaName指定条件类的对象</param>
    /// <param name="pageSize">分页大小</param>
    /// <param name="pageNo">分页号, 从1起始</param>
    /// <returns>实体集合</returns>
    public async Task<T> FetchListAsync<T>(string dataName, string criteriaName, object criteria, int? pageSize, int? pageNo)
      where T : IList
    {
      using (HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get,
        String.Format("{0}/{1}:{2}?id={3}&pageSize={4}&pageNo={5}",
          DATA_URI,
          EscapeDataName(dataName, typeof(T)),
          criteria != null ? EscapeCriteriaName(criteriaName, criteria.GetType()) : null,
          criteria != null ? Uri.EscapeDataString(criteria is ICriteria ? CriteriaHelper.JsonSerialize(criteria) : Phenix.Core.Reflection.Utilities.JsonSerialize(criteria)) : null,
          pageSize.HasValue ? pageSize.Value.ToString() : String.Empty,
          pageNo.HasValue ? pageNo.Value.ToString() : String.Empty)))
      using (HttpResponseMessage response = await HttpClient.SendAsync(request))
      {
        string result = await response.Content.ReadAsStringAsync();
        if (response.StatusCode != HttpStatusCode.OK)
          throw new HttpRequestException(result);
        return EntityListHelper.JsonDeserialize<T>(result);
      }
    }

    /// <summary>
    /// 获取实体集合
    /// </summary>
    /// <returns>实体集合</returns>
    public T FetchList<T>()
      where T : IEntityCollection
    {
      try
      {
        return FetchListAsync<T>().Result;
      }
      catch (AggregateException ex)
      {
        throw ex.InnerException;
      }
    }

    /// <summary>
    /// 获取实体集合
    /// </summary>
    /// <param name="pageSize">分页大小</param>
    /// <param name="pageNo">分页号, 从1起始</param>
    /// <returns>实体集合</returns>
    public T FetchList<T>(int pageSize, int pageNo)
      where T : IEntityCollection
    {
      try
      {
        return FetchListAsync<T>(pageSize, pageNo).Result;
      }
      catch (AggregateException ex)
      {
        throw ex.InnerException;
      }
    }

    /// <summary>
    /// 获取实体集合
    /// 如果服务端未定义criteria对象的类则要求服务端的实体类上申明了(ClassAttribute.DefaultCriteriaType)缺省条件类或者实体类程序集里定义有带同名Criteria后缀的条件类(需实现ICriteria接口)
    /// </summary>
    /// <param name="criteria">条件对象</param>
    /// <returns>实体集合</returns>
    public T FetchList<T>(ICriteria criteria)
      where T : IEntityCollection
    {
      try
      {
        return FetchListAsync<T>(criteria).Result;
      }
      catch (AggregateException ex)
      {
        throw ex.InnerException;
      }
    }

    /// <summary>
    /// 获取实体集合
    /// 如果服务端未定义criteria对象的类则要求服务端的实体类上申明了(ClassAttribute.DefaultCriteriaType)缺省条件类或者实体类程序集里定义有带同名Criteria后缀的条件类(需实现ICriteria接口)
    /// </summary>
    /// <param name="criteria">条件对象</param>
    /// <param name="pageSize">分页大小</param>
    /// <param name="pageNo">分页号, 从1起始</param>
    /// <returns>实体集合</returns>
    public T FetchList<T>(ICriteria criteria, int pageSize, int pageNo)
      where T : IEntityCollection
    {
      try
      {
        return FetchListAsync<T>(criteria, pageSize, pageNo).Result;
      }
      catch (AggregateException ex)
      {
        throw ex.InnerException;
      }
    }

    /// <summary>
    /// 获取实体集合
    /// </summary>
    /// <param name="dataName">数据名, 在服务端注册的实体集合类全名(需实现IEntityCollection接口)</param>
    /// <param name="criteriaName">条件名, 在服务端定义的条件类全名(需实现ICriteria接口)</param>
    /// <param name="criteria">JSON格式条件对象, 在服务端将被反序列化为criteriaName指定条件类的对象</param>
    /// <param name="pageSize">分页大小</param>
    /// <param name="pageNo">分页号, 从1起始</param>
    /// <returns>实体集合</returns>
    public T FetchList<T>(string dataName, string criteriaName, object criteria, int? pageSize, int? pageNo)
      where T : IList
    {
      try
      {
        return FetchListAsync<T>(dataName, criteriaName, criteria, pageSize, pageNo).Result;
      }
      catch (AggregateException ex)
      {
        throw ex.InnerException;
      }
    }

    /// <summary>
    /// 获取实体集合
    /// </summary>
    /// <param name="master">主对象</param>
    /// <param name="groupName">分组名, null代表全部</param>
    /// <returns>实体集合</returns>
    public async Task<T> FetchListAsync<T>(IEntity master, string groupName)
      where T : IEntityCollection
    {
      return await FetchListAsync<T>(String.Empty, String.Empty, master, groupName, String.Empty, null, null, null);
    }

    /// <summary>
    /// 获取实体集合
    /// </summary>
    /// <param name="master">主对象</param>
    /// <param name="groupName">分组名, null代表全部</param>
    /// <param name="pageSize">分页大小</param>
    /// <param name="pageNo">分页号, 从1起始</param>
    /// <returns>实体集合</returns>
    public async Task<T> FetchListAsync<T>(IEntity master, string groupName, int pageSize, int pageNo)
      where T : IEntityCollection
    {
      return await FetchListAsync<T>(String.Empty, String.Empty, master, groupName, String.Empty, null, pageSize, pageNo);
    }

    /// <summary>
    /// 获取实体集合
    /// 如果服务端未定义criteria对象的类则要求服务端的实体类上申明了(ClassAttribute.DefaultCriteriaType)缺省条件类或者实体类程序集里定义有带同名Criteria后缀的条件类(需实现ICriteria接口)
    /// </summary>
    /// <param name="master">主对象</param>
    /// <param name="groupName">分组名, null代表全部</param>
    /// <param name="criteria">条件对象</param>
    /// <returns>实体集合</returns>
    public async Task<T> FetchListAsync<T>(IEntity master, string groupName, ICriteria criteria)
      where T : IEntityCollection
    {
      return await FetchListAsync<T>(String.Empty, String.Empty, master, groupName, String.Empty, criteria, null, null);
    }

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
    public async Task<T> FetchListAsync<T>(IEntity master, string groupName, ICriteria criteria, int pageSize, int pageNo)
      where T : IEntityCollection
    {
      return await FetchListAsync<T>(String.Empty, String.Empty, master, groupName, String.Empty, criteria, pageSize, pageNo);
    }

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
    public async Task<T> FetchListAsync<T>(string dataName, string masterName, object master, string groupName, string criteriaName, object criteria, int? pageSize, int? pageNo)
      where T : IList
    {
      if (master == null)
        throw new ArgumentNullException("master");
      Type masterType = master.GetType();
      using (HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get,
        String.Format("{0}/{1}:{2}:{3}?masterId={4}&groupName={5}&id={6}&pageSize={7}&pageNo={8}",
          DATA_URI,
          EscapeDataName(dataName, typeof(T)),
          EscapeMasterName(masterName, masterType),
          criteria != null ? EscapeCriteriaName(criteriaName, criteria.GetType()) : null,
          Uri.EscapeDataString(master is string || masterType.IsValueType ? master.ToString() : EntityHelper.GetPrimaryKeyFieldValue(master).ToString()), 
          groupName,
          criteria != null ? Uri.EscapeDataString(criteria is ICriteria ? CriteriaHelper.JsonSerialize(criteria) : Phenix.Core.Reflection.Utilities.JsonSerialize(criteria)) : null,
          pageSize.HasValue ? pageSize.Value.ToString() : String.Empty, 
          pageNo.HasValue ? pageNo.Value.ToString() : String.Empty)))
      using (HttpResponseMessage response = await HttpClient.SendAsync(request))
      {
        string result = await response.Content.ReadAsStringAsync();
        if (response.StatusCode != HttpStatusCode.OK)
          throw new HttpRequestException(result);
        return EntityListHelper.JsonDeserialize<T>(result);
      }
    }

    /// <summary>
    /// 获取实体集合
    /// </summary>
    /// <param name="master">主对象</param>
    /// <param name="groupName">分组名, null代表全部</param>
    /// <returns>实体集合</returns>
    public T FetchList<T>(IEntity master, string groupName)
      where T : IEntityCollection
    {
      try
      {
        return FetchListAsync<T>(master, groupName).Result;
      }
      catch (AggregateException ex)
      {
        throw ex.InnerException;
      }
    }

    /// <summary>
    /// 获取实体集合
    /// </summary>
    /// <param name="master">主对象</param>
    /// <param name="groupName">分组名, null代表全部</param>
    /// <param name="pageSize">分页大小</param>
    /// <param name="pageNo">分页号, 从1起始</param>
    /// <returns>实体集合</returns>
    public T FetchList<T>(IEntity master, string groupName, int pageSize, int pageNo)
      where T : IEntityCollection
    {
      try
      {
        return FetchListAsync<T>(master, groupName, pageSize, pageNo).Result;
      }
      catch (AggregateException ex)
      {
        throw ex.InnerException;
      }
    }

    /// <summary>
    /// 获取实体集合
    /// 如果服务端未定义criteria对象的类则要求服务端的实体类上申明了(ClassAttribute.DefaultCriteriaType)缺省条件类或者实体类程序集里定义有带同名Criteria后缀的条件类(需实现ICriteria接口)
    /// </summary>
    /// <param name="master">主对象</param>
    /// <param name="groupName">分组名, null代表全部</param>
    /// <param name="criteria">条件对象</param>
    /// <returns>实体集合</returns>
    public T FetchList<T>(IEntity master, string groupName, ICriteria criteria)
      where T : IEntityCollection
    {
      try
      {
        return FetchListAsync<T>(master, groupName, criteria).Result;
      }
      catch (AggregateException ex)
      {
        throw ex.InnerException;
      }
    }

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
    public T FetchList<T>(IEntity master, string groupName, ICriteria criteria, int pageSize, int pageNo)
      where T : IEntityCollection
    {
      try
      {
        return FetchListAsync<T>(master, groupName, criteria, pageSize, pageNo).Result;
      }
      catch (AggregateException ex)
      {
        throw ex.InnerException;
      }
    }

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
    public T FetchList<T>(string dataName, string masterName, object master, string groupName, string criteriaName, object criteria, int? pageSize, int? pageNo)
      where T : IList
    {
      try
      {
        return FetchListAsync<T>(dataName, masterName, master, groupName, criteriaName, criteria, pageSize, pageNo).Result;
      }
      catch (AggregateException ex)
      {
        throw ex.InnerException;
      }
    }

    #endregion

    #region GetRecordCount

    /// <summary>
    /// 获取记录数量
    /// </summary>
    /// <returns>记录数量</returns>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
    public async Task<long> GetRecordCountAsync<T>()
       where T : IEntityCollection
    {
      return await GetRecordCountAsync<T>(String.Empty, String.Empty, null);
    }

    /// <summary>
    /// 获取记录数量
    /// 如果服务端未定义criteria对象的类则要求服务端的实体类上申明了(ClassAttribute.DefaultCriteriaType)缺省条件类或者实体类程序集里定义有带同名Criteria后缀的条件类(需实现ICriteria接口)
    /// </summary>
    /// <param name="criteria">条件对象</param>
    /// <returns>记录数量</returns>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
    public async Task<long> GetRecordCountAsync<T>(ICriteria criteria)
      where T : IEntityCollection
    {
      return await GetRecordCountAsync<T>(String.Empty, String.Empty, criteria);
    }

    /// <summary>
    /// 获取记录数量
    /// </summary>
    /// <param name="dataName">数据名, 在服务端注册的实体集合类全名(需实现IEntityCollection接口)</param>
    /// <param name="criteriaName">条件名, 在服务端定义的条件类全名(需实现ICriteria接口)</param>
    /// <param name="criteria">JSON格式条件对象, 在服务端将被反序列化为criteriaName指定条件类的对象</param>
    /// <returns>记录数量</returns>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
    public async Task<long> GetRecordCountAsync<T>(string dataName, string criteriaName, object criteria)
      where T : IList
    {
      using (HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get,
        String.Format("{0}/{1}:{2}?id={3}&pageSize=0&pageNo=0",
          DATA_URI,
          EscapeDataName(dataName, typeof(T)),
          criteria != null ? EscapeCriteriaName(criteriaName, criteria.GetType()) : null,
          criteria != null ? Uri.EscapeDataString(criteria is ICriteria ? CriteriaHelper.JsonSerialize(criteria) : Phenix.Core.Reflection.Utilities.JsonSerialize(criteria)) : null)))
      using (HttpResponseMessage response = await HttpClient.SendAsync(request))
      {
        string result = await response.Content.ReadAsStringAsync();
        if (response.StatusCode != HttpStatusCode.OK)
          throw new HttpRequestException(result);
        return Int64.Parse(result);
      }
    }

    /// <summary>
    /// 获取记录数量
    /// </summary>
    /// <returns>记录数量</returns>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
    public long GetRecordCount<T>()
      where T : IEntityCollection
    {
      try
      {
        return GetRecordCountAsync<T>().Result;
      }
      catch (AggregateException ex)
      {
        throw ex.InnerException;
      }
    }

    /// <summary>
    /// 获取记录数量
    /// 如果服务端未定义criteria对象的类则要求服务端的实体类上申明了(ClassAttribute.DefaultCriteriaType)缺省条件类或者实体类程序集里定义有带同名Criteria后缀的条件类(需实现ICriteria接口)
    /// </summary>
    /// <param name="criteria">条件对象</param>
    /// <returns>记录数量</returns>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
    public long GetRecordCount<T>(ICriteria criteria)
      where T : IEntityCollection
    {
      try
      {
        return GetRecordCountAsync<T>(criteria).Result;
      }
      catch (AggregateException ex)
      {
        throw ex.InnerException;
      }
    }

    /// <summary>
    /// 获取记录数量
    /// </summary>
    /// <param name="dataName">数据名, 在服务端注册的实体集合类全名(需实现IEntityCollection接口)</param>
    /// <param name="criteriaName">条件名, 在服务端定义的条件类全名(需实现ICriteria接口)</param>
    /// <param name="criteria">JSON格式条件对象, 在服务端将被反序列化为criteriaName指定条件类的对象</param>
    /// <returns>记录数量</returns>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
    public long GetRecordCount<T>(string dataName, string criteriaName, object criteria)
      where T : IList
    {
      try
      {
        return GetRecordCountAsync<T>(dataName, criteriaName, criteria).Result;
      }
      catch (AggregateException ex)
      {
        throw ex.InnerException;
      }
    }

    /// <summary>
    /// 获取记录数量
    /// </summary>
    /// <param name="master">主对象</param>
    /// <param name="groupName">分组名, null代表全部</param>
    /// <returns>记录数量</returns>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
    public async Task<long> GetRecordCountAsync<T>(IEntity master, string groupName)
      where T : IEntityCollection
    {
      return await GetRecordCountAsync<T>(String.Empty, String.Empty, master, groupName, String.Empty, null);
    }

    /// <summary>
    /// 获取记录数量
    /// 如果服务端未定义criteria对象的类则要求服务端的实体类上申明了(ClassAttribute.DefaultCriteriaType)缺省条件类或者实体类程序集里定义有带同名Criteria后缀的条件类(需实现ICriteria接口)
    /// </summary>
    /// <param name="master">主对象</param>
    /// <param name="groupName">分组名, null代表全部</param>
    /// <param name="criteria">条件对象</param>
    /// <returns>记录数量</returns>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
    public async Task<long> GetRecordCountAsync<T>(IEntity master, string groupName, ICriteria criteria)
      where T : IEntityCollection
    {
      return await GetRecordCountAsync<T>(String.Empty, String.Empty, master, groupName, String.Empty, criteria);
    }

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
    public async Task<long> GetRecordCountAsync<T>(string dataName, string masterName, object master, string groupName, string criteriaName, object criteria)
      where T : IList
    {
      if (master == null)
        throw new ArgumentNullException("master");
      Type masterType = master.GetType();
      using (HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get,
        String.Format("{0}/{1}:{2}:{3}?masterId={4}&groupName={5}&id={6}&pageSize=0&pageNo=0",
          DATA_URI,
          EscapeDataName(dataName, typeof(T)),
          EscapeMasterName(masterName, masterType),
          criteria != null ? EscapeCriteriaName(criteriaName, criteria.GetType()) : null,
          Uri.EscapeDataString(master is string || masterType.IsValueType ? master.ToString() : EntityHelper.GetPrimaryKeyFieldValue(master).ToString()),
          groupName,
          criteria != null ? Uri.EscapeDataString(criteria is ICriteria ? CriteriaHelper.JsonSerialize(criteria) : Phenix.Core.Reflection.Utilities.JsonSerialize(criteria)) : null)))
      {
        using (HttpResponseMessage response = await HttpClient.SendAsync(request))
        {
          string result = await response.Content.ReadAsStringAsync();
          if (response.StatusCode != HttpStatusCode.OK)
            throw new HttpRequestException(result);
          return Int64.Parse(result);
        }
      }
    }

    /// <summary>
    /// 获取记录数量
    /// </summary>
    /// <param name="master">主对象</param>
    /// <param name="groupName">分组名, null代表全部</param>
    /// <returns>记录数量</returns>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
    public long GetRecordCount<T>(IEntity master, string groupName)
      where T : IEntityCollection
    {
      try
      {
        return GetRecordCountAsync<T>(master, groupName).Result;
      }
      catch (AggregateException ex)
      {
        throw ex.InnerException;
      }
    }

    /// <summary>
    /// 获取记录数量
    /// 如果服务端未定义criteria对象的类则要求服务端的实体类上申明了(ClassAttribute.DefaultCriteriaType)缺省条件类或者实体类程序集里定义有带同名Criteria后缀的条件类(需实现ICriteria接口)
    /// </summary>
    /// <param name="master">主对象</param>
    /// <param name="groupName">分组名, null代表全部</param>
    /// <param name="criteria">条件对象</param>
    /// <returns>记录数量</returns>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
    public long GetRecordCount<T>(IEntity master, string groupName, ICriteria criteria)
      where T : IEntityCollection
    {
      try
      {
        return GetRecordCountAsync<T>(master, groupName, criteria).Result;
      }
      catch (AggregateException ex)
      {
        throw ex.InnerException;
      }
    }

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
    public long GetRecordCount<T>(string dataName, string masterName, object master, string groupName, string criteriaName, string criteria)
      where T : IList
    {
      try
      {
        return GetRecordCountAsync<T>(dataName, masterName, master, groupName, criteriaName, criteria).Result;
      }
      catch (AggregateException ex)
      {
        throw ex.InnerException;
      }
    }

    #endregion

    #region Save

    /// <summary>
    /// 提交实体
    /// </summary>
    /// <param name="entity">实体</param>
    /// <returns>成功数量</returns>
    public async Task<int> SaveAsync(IEntity entity)
    {
      return await SaveAsync(String.Empty, entity);
    }

    /// <summary>
    /// 提交实体
    /// </summary>
    /// <param name="dataName">数据名, 在服务端注册的实体类全名(需实现IEntity接口)</param>
    /// <param name="entity">实体</param>
    /// <returns>成功数量</returns>
    public async Task<int> SaveAsync(string dataName, IEntity entity)
    {
      if (entity == null)
        return 0;
      using (HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post,
        String.Format("{0}/{1}",
          DATA_URI,
          EscapeDataName(dataName, entity.GetType()))))
      {
        request.Content = new StringContent(Phenix.Core.Reflection.Utilities.JsonSerialize(entity), Encoding.UTF8); //EntityHelper.JsonPackChangedValues(entity));
        using (HttpResponseMessage response = await HttpClient.SendAsync(request))
        {
          string result = await response.Content.ReadAsStringAsync();
          if (response.StatusCode != HttpStatusCode.OK)
            throw new HttpRequestException(result);
          return Int32.Parse(result);
        }
      }
    }

    /// <summary>
    /// 提交实体
    /// </summary>
    /// <param name="entity">实体</param>
    /// <returns>成功数量</returns>
    public int Save(IEntity entity)
    {
      try
      {
        return SaveAsync(entity).Result;
      }
      catch (AggregateException ex)
      {
        throw ex.InnerException;
      }
    }

    /// <summary>
    /// 提交实体
    /// </summary>
    /// <param name="dataName">数据名, 在服务端注册的实体类全名(需实现IEntity接口)</param>
    /// <param name="entity">实体</param>
    /// <returns>成功数量</returns>
    public int Save(string dataName, IEntity entity)
    {
      try
      {
        return SaveAsync(dataName, entity).Result;
      }
      catch (AggregateException ex)
      {
        throw ex.InnerException;
      }
    }

    /// <summary>
    /// 提交实体集合
    /// </summary>
    /// <param name="entityCollection">实体集合</param>
    /// <returns>成功数量</returns>
    public async Task<int> SaveAsync(IEntityCollection entityCollection)
    {
      return await SaveAsync(String.Empty, entityCollection);
    }

    /// <summary>
    /// 提交实体集合
    /// </summary>
    /// <param name="dataName">数据名, 在服务端注册的实体集合类/实体类全名(需实现IEntityCollection/IEntity接口)</param>
    /// <param name="entityCollection">实体集合</param>
    /// <returns>成功数量</returns>
    public async Task<int> SaveAsync(string dataName, IEntityCollection entityCollection)
    {
      if (entityCollection == null)
        return 0;
      using (HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post,
        String.Format("{0}/{1}",
          DATA_URI,
          EscapeDataName(dataName, entityCollection.GetType()))))
      {
        request.Content = new StringContent(Phenix.Core.Reflection.Utilities.JsonSerialize(entityCollection), Encoding.UTF8); //EntityListHelper.JsonPackChangedValues(entityCollection));
        using (HttpResponseMessage response = await HttpClient.SendAsync(request))
        {
          string result = await response.Content.ReadAsStringAsync();
          if (response.StatusCode != HttpStatusCode.OK)
            throw new HttpRequestException(result);
          return Int32.Parse(result);
        }
      }
    }

    /// <summary>
    /// 提交实体集合
    /// </summary>
    /// <param name="entityCollection">实体集合</param>
    /// <returns>成功数量</returns>
    public int Save(IEntityCollection entityCollection)
    {
      try
      {
        return SaveAsync(entityCollection).Result;
      }
      catch (AggregateException ex)
      {
        throw ex.InnerException;
      }
    }

    /// <summary>
    /// 提交实体集合
    /// </summary>
    /// <param name="dataName">数据名, 在服务端注册的实体集合类/实体类全名(需实现IEntityCollection/IEntity接口)</param>
    /// <param name="entityCollection">实体集合</param>
    /// <returns>成功数量</returns>
    public int Save(string dataName, IEntityCollection entityCollection)
    {
      try
      {
        return SaveAsync(dataName, entityCollection).Result;
      }
      catch (AggregateException ex)
      {
        throw ex.InnerException;
      }
    }

    /// <summary>
    /// 提交对象
    /// </summary>
    /// <param name="dataName">数据名, 在服务端注册的实体集合类/实体类全名(需实现IEntityCollection/IEntity接口)</param>
    /// <param name="obj">对象, 将被序列化传到服务端, 需包含IsNew、IsSelfDeleted、IsSelfDirty属性以指明对象状态</param>
    /// <returns>成功数量</returns>
    public async Task<int> SaveAsync(string dataName, object obj)
    {
      if (obj == null)
        return 0;
      using (HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, 
        String.Format("{0}/{1}",
          DATA_URI,
          EscapeDataName(dataName, obj.GetType()))))
      {
        request.Content = new StringContent(Phenix.Core.Reflection.Utilities.JsonSerialize(obj), Encoding.UTF8);
        using (HttpResponseMessage response = await HttpClient.SendAsync(request))
        {
          string result = await response.Content.ReadAsStringAsync();
          if (response.StatusCode != HttpStatusCode.OK)
            throw new HttpRequestException(result);
          return Int32.Parse(result);
        }
      }
    }

    /// <summary>
    /// 提交对象
    /// </summary>
    /// <param name="dataName">数据名, 在服务端注册的实体集合类/实体类全名(需实现IEntityCollection/IEntity接口)</param>
    /// <param name="obj">对象, 将被序列化传到服务端, 需包含IsNew、IsSelfDeleted、IsSelfDirty属性以指明对象状态</param>
    /// <returns>成功数量</returns>
    public int Save(string dataName, object obj)
    {
      try
      {
        return SaveAsync(dataName, obj).Result;
      }
      catch (AggregateException ex)
      {
        throw ex.InnerException;
      }
    }

    #endregion

    #region Execute

    /// <summary>
    /// 执行服务
    /// </summary>
    /// <param name="service">服务对象, 将被序列化传到服务端</param>
    /// <returns>服务结果</returns>
    public async Task<T> ExecuteAsync<T>(T service)
      where T : IService
    {
      return (T)await ExecuteAsync(String.Empty, service);
    }

    /// <summary>
    /// 执行服务
    /// </summary>
    /// <param name="serviceName">服务名, 在服务端注册的服务类全名(需实现IService接口)</param>
    /// <param name="obj">JSON格式对象, 将被传到服务端</param>
    /// <returns>服务结果</returns>
    public async Task<object> ExecuteAsync(string serviceName, object obj)
    {
      if (obj == null)
        throw new ArgumentNullException("obj");
      using (HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, 
        String.Format("{0}/{1}",
          DATA_URI,
          EscapeDataName(serviceName, obj.GetType()))))
      {
        request.Content = new StringContent(Phenix.Core.Reflection.Utilities.JsonSerialize(obj), Encoding.UTF8);
        using (HttpResponseMessage response = await HttpClient.SendAsync(request))
        {
          string result = await response.Content.ReadAsStringAsync();
          if (response.StatusCode != HttpStatusCode.OK)
            throw new HttpRequestException(result);
          return Phenix.Core.Reflection.Utilities.JsonDeserialize(result, obj.GetType());
        }
      }
    }

    /// <summary>
    /// 执行服务
    /// </summary>
    /// <param name="service">服务对象, 将被序列化传到服务端</param>
    /// <returns>服务结果</returns>
    public T Execute<T>(T service)
      where T : IService
    {
      try
      {
        return ExecuteAsync(service).Result;
      }
      catch (AggregateException ex)
      {
        throw ex.InnerException;
      }
    }

    /// <summary>
    /// 执行服务
    /// </summary>
    /// <param name="serviceName">服务名, 在服务端注册的服务类全名(需实现IService接口)</param>
    /// <param name="obj">对象, 将被序列化传到服务端</param>
    /// <returns>服务结果</returns>
    public object Execute(string serviceName, object obj)
    {
      try
      {
        return ExecuteAsync(serviceName, obj).Result;
      }
      catch (AggregateException ex)
      {
        throw ex.InnerException;
      }
    }

    #endregion

    #region UploadFiles

    /// <summary>
    /// 上传文件
    /// </summary>
    /// <param name="service">服务对象, 将被序列化传到服务端</param>
    /// <param name="fileNames">待上传的文件路径</param>
    /// <returns>服务结果</returns>
    public async Task<T> UploadFilesAsync<T>(T service, params string[] fileNames)
      where T : IService
    {
      return (T)await UploadFilesAsync(String.Empty, service, fileNames);
    }

    /// <summary>
    /// 上传文件
    /// </summary>
    /// <param name="serviceName">服务名, 在服务端注册的服务类全名(需实现IService接口)</param>
    /// <param name="obj">JSON格式对象, 将被传到服务端</param>
    /// <param name="fileNames">待上传的文件路径</param>
    /// <returns>服务结果</returns>
    public async Task<object> UploadFilesAsync(string serviceName, object obj, params string[] fileNames)
    {
      Dictionary<string, Stream> fileStreams = null;
      if (fileNames != null)
      {
        fileStreams = new Dictionary<string, Stream>(fileNames.Length, StringComparer.OrdinalIgnoreCase);
        foreach (string s in fileNames)
        {
          if (String.IsNullOrEmpty(s))
            throw new InvalidOperationException("不允许fileNames参数值内含空项");
          fileStreams.Add(s, new FileStream(s, FileMode.Open, FileAccess.Read, FileShare.Read));
        }
      }
      return await UploadFilesAsync(serviceName, obj, fileStreams);
    }

    /// <summary>
    /// 上传文件
    /// </summary>
    /// <param name="service">服务对象, 将被序列化传到服务端</param>
    /// <param name="fileNames">待上传的文件路径</param>
    /// <returns>服务结果</returns>
    public T UploadFiles<T>(T service, params string[] fileNames)
      where T : IService
    {
      try
      {
        return UploadFilesAsync(service, fileNames).Result;
      }
      catch (AggregateException ex)
      {
        throw ex.InnerException;
      }
    }

    /// <summary>
    /// 上传文件
    /// </summary>
    /// <param name="serviceName">服务名, 在服务端注册的服务类全名(需实现IService接口)</param>
    /// <param name="obj">对象, 将被序列化传到服务端</param>
    /// <param name="fileNames">待上传的文件路径</param>
    /// <returns>服务结果</returns>
    public object UploadFiles(string serviceName, object obj, params string[] fileNames)
    {
      try
      {
        return UploadFilesAsync(serviceName, obj, fileNames).Result;
      }
      catch (AggregateException ex)
      {
        throw ex.InnerException;
      }
    }

    /// <summary>
    /// 上传文件
    /// </summary>
    /// <param name="service">服务对象, 将被序列化传到服务端</param>
    /// <param name="fileStreams">待上传的文件流</param>
    /// <returns>服务结果</returns>
    public async Task<T> UploadFilesAsync<T>(T service, IDictionary<string, Stream> fileStreams)
      where T : IService
    {
      return (T)await UploadFilesAsync(String.Empty, service, fileStreams);
    }

    /// <summary>
    /// 上传文件
    /// </summary>
    /// <param name="serviceName">服务名, 在服务端注册的服务类全名(需实现IService接口)</param>
    /// <param name="obj">JSON格式对象, 将被传到服务端</param>
    /// <param name="fileStreams">待上传的文件流</param>
    /// <returns>服务结果</returns>
    public async Task<object> UploadFilesAsync(string serviceName, object obj, IDictionary<string, Stream> fileStreams)
    {
      if (obj == null)
        throw new ArgumentNullException("obj");
      using (HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, 
        String.Format("{0}/{1}",
          DATA_URI,
          EscapeDataName(serviceName, obj.GetType()))))
      {
        MultipartFormDataContent formDataContent = new MultipartFormDataContent();
        request.Content = formDataContent;
        formDataContent.Add(new StringContent(Phenix.Core.Reflection.Utilities.JsonSerialize(obj), Encoding.UTF8), "data");
        if (fileStreams != null)
          foreach (KeyValuePair<string, Stream> kvp in fileStreams)
          {
            if (kvp.Value == null)
              throw new InvalidOperationException("不允许fileStreams参数值内含空项");
            StreamContent fileContent = new StreamContent(kvp.Value);
            fileContent.Headers.ContentDisposition = new ContentDispositionHeaderValue("form-data");
            fileContent.Headers.ContentDisposition.FileName = Path.GetFileName(kvp.Key);
            fileContent.Headers.ContentDisposition.Name = "file";
            formDataContent.Add(fileContent);
          }
        using (HttpResponseMessage response = await HttpClient.SendAsync(request))
        {
          string result = await response.Content.ReadAsStringAsync();
          if (response.StatusCode != HttpStatusCode.OK)
            throw new HttpRequestException(result);
          return Phenix.Core.Reflection.Utilities.JsonDeserialize(result, obj.GetType());
        }
      }
    }

    /// <summary>
    /// 上传文件
    /// </summary>
    /// <param name="service">服务对象, 将被序列化传到服务端</param>
    /// <param name="fileStreams">待上传的文件流</param>
    /// <returns>服务结果</returns>
    public T UploadFiles<T>(T service, IDictionary<string, Stream> fileStreams)
      where T : IService
    {
      try
      {
        return UploadFilesAsync(service, fileStreams).Result;
      }
      catch (AggregateException ex)
      {
        throw ex.InnerException;
      }
    }

    /// <summary>
    /// 上传文件
    /// </summary>
    /// <param name="serviceName">服务名, 在服务端注册的服务类全名(需实现IService接口)</param>
    /// <param name="obj">对象, 将被序列化传到服务端</param>
    /// <param name="fileStreams">待上传的文件流</param>
    /// <returns>服务结果</returns>
    public object UploadFiles(string serviceName, object obj, IDictionary<string, Stream> fileStreams)
    {
      try
      {
        return UploadFilesAsync(serviceName, obj, fileStreams).Result;
      }
      catch (AggregateException ex)
      {
        throw ex.InnerException;
      }
    }

    #endregion

    #region UploadBigFile

    /// <summary>
    /// 上传大文件
    /// </summary>
    /// <param name="service">服务对象, 将被序列化传到服务端</param>
    /// <param name="fileName">待上传的文件路径</param>
    /// <param name="doProgress">执行进度干预</param>
    /// <returns>服务结果</returns>
    public async Task<T> UploadBigFileAsync<T>(T service, string fileName, Func<object, FileChunkInfo, bool> doProgress)
      where T : IService
    {
      return (T)await UploadBigFileAsync(String.Empty, service, fileName, doProgress);
    }

    /// <summary>
    /// 上传大文件
    /// </summary>
    /// <param name="serviceName">服务名, 在服务端注册的服务类全名(需实现IService接口)</param>
    /// <param name="obj">JSON格式对象, 将被传到服务端</param>
    /// <param name="fileName">待上传的文件路径</param>
    /// <param name="doProgress">执行进度干预</param>
    /// <returns>服务结果</returns>
    public async Task<object> UploadBigFileAsync(string serviceName, object obj, string fileName, Func<object, FileChunkInfo, bool> doProgress)
    {
      if (String.IsNullOrEmpty(fileName))
        throw new ArgumentNullException("fileName");
      using (Stream fileStream = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.Read))
      {
        return await UploadBigFileAsync(serviceName, obj, fileName, fileStream, doProgress);
      }
    }

    /// <summary>
    /// 上传大文件
    /// </summary>
    /// <param name="service">服务对象, 将被序列化传到服务端</param>
    /// <param name="fileName">待上传的文件路径</param>
    /// <param name="doProgress">执行进度干预</param>
    /// <returns>服务结果</returns>
    public T UploadBigFile<T>(T service, string fileName, Func<object, FileChunkInfo, bool> doProgress)
      where T : IService
    {
      try
      {
        return UploadBigFileAsync(service, fileName, doProgress).Result;
      }
      catch (AggregateException ex)
      {
        throw ex.InnerException;
      }
    }


    /// <summary>
    /// 上传大文件
    /// </summary>
    /// <param name="serviceName">服务名, 在服务端注册的服务类全名(需实现IService接口)</param>
    /// <param name="obj">对象, 将被序列化传到服务端</param>
    /// <param name="fileName">待上传的文件路径</param>
    /// <param name="doProgress">执行进度干预</param>
    /// <returns>服务结果</returns>
    public object UploadBigFile(string serviceName, object obj, string fileName, Func<object, FileChunkInfo, bool> doProgress)
    {
      try
      {
        return UploadBigFileAsync(serviceName, obj, fileName, doProgress).Result;
      }
      catch (AggregateException ex)
      {
        throw ex.InnerException;
      }
    }

    /// <summary>
    /// 上传大文件
    /// </summary>
    /// <param name="service">服务对象, 将被序列化传到服务端</param>
    /// <param name="fileName">待上传的文件名</param>
    /// <param name="fileStream">待上传的文件流</param>
    /// <param name="doProgress">执行进度干预</param>
    /// <returns>服务结果</returns>
    public async Task<T> UploadBigFileAsync<T>(T service, string fileName, Stream fileStream, Func<object, FileChunkInfo, bool> doProgress)
      where T : IService
    {
      return (T)await UploadBigFileAsync(String.Empty, service, fileName, fileStream, doProgress);
    }

    /// <summary>
    /// 上传大文件
    /// </summary>
    /// <param name="serviceName">服务名, 在服务端注册的服务类全名(需实现IService接口)</param>
    /// <param name="obj">JSON格式对象, 将被传到服务端</param>
    /// <param name="fileName">待上传的文件名</param>
    /// <param name="fileStream">待上传的文件流</param>
    /// <param name="doProgress">执行进度干预</param>
    /// <returns>服务结果</returns>
    public async Task<object> UploadBigFileAsync(string serviceName, object obj, string fileName, Stream fileStream, Func<object, FileChunkInfo, bool> doProgress)
    {
      if (fileStream == null)
        throw new ArgumentNullException("fileStream");
      object result = obj;
      for (int i = 1; i <= FileHelper.GetChunkCount(fileStream.Length); i++)
      {
        FileChunkInfo fileChunkInfo = FileHelper.ReadChunkInfo(fileName, fileStream, i);
        if (doProgress == null || doProgress(result, fileChunkInfo))
          result = await UploadBigFileAsync(serviceName, obj, fileChunkInfo);
        else
        {
          result = await UploadBigFileAsync(serviceName, obj, new FileChunkInfo(fileName));
          break;
        }
      }
      return result;
    }

    /// <summary>
    /// 上传大文件
    /// </summary>
    /// <param name="service">服务对象, 将被序列化传到服务端</param>
    /// <param name="fileName">待上传的文件名</param>
    /// <param name="fileStream">待上传的文件流</param>
    /// <param name="doProgress">执行进度干预</param>
    /// <returns>服务结果</returns>
    public T UploadBigFile<T>(T service, string fileName, Stream fileStream, Func<object, FileChunkInfo, bool> doProgress)
      where T : IService
    {
      try
      {
        return UploadBigFileAsync(service, fileName, fileStream, doProgress).Result;
      }
      catch (AggregateException ex)
      {
        throw ex.InnerException;
      }
    }

    /// <summary>
    /// 上传文件
    /// </summary>
    /// <param name="serviceName">服务名, 在服务端注册的服务类全名(需实现IService接口)</param>
    /// <param name="obj">对象, 将被序列化传到服务端</param>
    /// <param name="fileName">待上传的文件名</param>
    /// <param name="fileStream">待上传的文件流</param>
    /// <param name="doProgress">执行进度干预</param>
    /// <returns>服务结果</returns>
    public object UploadBigFile(string serviceName, object obj, string fileName, Stream fileStream, Func<object, FileChunkInfo, bool> doProgress)
    {
      try
      {
        return UploadBigFileAsync(serviceName, obj, fileName, fileStream, doProgress).Result;
      }
      catch (AggregateException ex)
      {
        throw ex.InnerException;
      }
    }

    /// <summary>
    /// 上传大文件
    /// </summary>
    /// <param name="service">服务对象, 将被序列化传到服务端</param>
    /// <param name="fileChunkInfo">待处理的文件块信息</param>
    /// <returns>服务结果</returns>
    public async Task<T> UploadBigFileAsync<T>(T service, FileChunkInfo fileChunkInfo)
      where T : IService
    {
      return (T)await UploadBigFileAsync(String.Empty, service, fileChunkInfo);
    }

    /// <summary>
    /// 上传大文件
    /// </summary>
    /// <param name="serviceName">服务名, 在服务端注册的服务类全名(需实现IService接口)</param>
    /// <param name="obj">JSON格式对象, 将被传到服务端</param>
    /// <param name="fileChunkInfo">待处理的文件块信息</param>
    /// <returns>服务结果</returns>
    public async Task<object> UploadBigFileAsync(string serviceName, object obj, FileChunkInfo fileChunkInfo)
    {
      if (obj == null)
        throw new ArgumentNullException("obj");
      if (fileChunkInfo == null)
        throw new ArgumentNullException("fileChunkInfo");
      using (HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, 
        String.Format("{0}/{1}",
          DATA_URI,
          EscapeDataName(serviceName, obj.GetType()))))
      {
        MultipartFormDataContent formDataContent = new MultipartFormDataContent();
        request.Content = formDataContent;
        formDataContent.Add(new StringContent(Phenix.Core.Reflection.Utilities.JsonSerialize(obj), Encoding.UTF8), "data");
        formDataContent.Add(new StringContent(Phenix.Core.Reflection.Utilities.JsonSerialize(fileChunkInfo), Encoding.UTF8), "chunkInfo");
        using (HttpResponseMessage response = await HttpClient.SendAsync(request))
        {
          string result = await response.Content.ReadAsStringAsync();
          if (response.StatusCode != HttpStatusCode.OK)
            throw new HttpRequestException(result);
          return Phenix.Core.Reflection.Utilities.JsonDeserialize(result, obj.GetType());
        }
      }
    }

    /// <summary>
    /// 上传大文件
    /// </summary>
    /// <param name="service">服务对象, 将被序列化传到服务端</param>
    /// <param name="fileChunkInfo">待处理的文件块信息</param>
    /// <returns>服务结果</returns>
    public T UploadBigFile<T>(T service, FileChunkInfo fileChunkInfo)
      where T : IService
    {
      try
      {
        return UploadBigFileAsync(service, fileChunkInfo).Result;
      }
      catch (AggregateException ex)
      {
        throw ex.InnerException;
      }
    }

    /// <summary>
    /// 上传文件
    /// </summary>
    /// <param name="serviceName">服务名, 在服务端注册的服务类全名(需实现IService接口)</param>
    /// <param name="obj">对象, 将被序列化传到服务端</param>
    /// <param name="fileChunkInfo">待处理的文件块信息</param>
    /// <returns>服务结果</returns>
    public object UploadBigFile(string serviceName, object obj, FileChunkInfo fileChunkInfo)
    {
      try
      {
        return UploadBigFileAsync(serviceName, obj, fileChunkInfo).Result;
      }
      catch (AggregateException ex)
      {
        throw ex.InnerException;
      }
    }

    #endregion

    #region DownloadFile

    /// <summary>
    /// 下载文件
    /// </summary>
    /// <param name="service">服务对象, 将被序列化传到服务端</param>
    /// <param name="fileName">待保存的文件路径</param>
    public async Task DownloadFileAsync<T>(T service, string fileName)
      where T : IService
    {
      await DownloadFileAsync(String.Empty, service, fileName);
    }

    /// <summary>
    /// 下载文件
    /// </summary>
    /// <param name="serviceName">服务名, 在服务端注册的服务类全名(需实现IService接口)</param>
    /// <param name="obj">JSON格式对象, 将被传到服务端</param>
    /// <param name="fileName">待保存的文件路径</param>
    public async Task DownloadFileAsync(string serviceName, object obj, string fileName)
    {
      if (String.IsNullOrEmpty(fileName))
        throw new ArgumentNullException("fileName");
      using (Stream sourceStream = await DownloadFileAsync(serviceName, obj))
      using (FileStream targetStream = new FileStream(fileName, FileMode.Create, FileAccess.Write))
      {
        sourceStream.Position = 0;
        Phenix.Core.IO.StreamHelper.CopyBuffer(sourceStream, targetStream);
      }
    }

    /// <summary>
    /// 下载文件
    /// </summary>
    /// <param name="service">服务对象, 将被序列化传到服务端</param>
    /// <param name="fileName">待保存的文件路径</param>
    public void DownloadFile<T>(T service, string fileName)
      where T : IService
    {
      try
      {
        DownloadFileAsync(service, fileName).Wait();
      }
      catch (AggregateException ex)
      {
        throw ex.InnerException;
      }
    }

    /// <summary>
    /// 下载文件
    /// </summary>
    /// <param name="serviceName">服务名, 在服务端注册的服务类全名(需实现IService接口)</param>
    /// <param name="obj">对象, 将被序列化传到服务端</param>
    /// <param name="fileName">待保存的文件路径</param>
    public void DownloadFile(string serviceName, object obj, string fileName)
    {
      try
      {
        DownloadFileAsync(serviceName, obj, fileName).Wait();
      }
      catch (AggregateException ex)
      {
        throw ex.InnerException;
      }
    }

    /// <summary>
    /// 下载文件
    /// </summary>
    /// <param name="service">服务对象, 将被序列化传到服务端</param>
    /// <returns>已下载的文件流</returns>
    public async Task<Stream> DownloadFileAsync<T>(T service)
      where T : IService
    {
      return await DownloadFileAsync(String.Empty, service);
    }

    /// <summary>
    /// 下载文件
    /// </summary>
    /// <param name="serviceName">服务名, 在服务端注册的服务类全名(需实现IService接口)</param>
    /// <param name="obj">JSON格式对象, 将被传到服务端</param>
    /// <returns>已下载的文件流</returns>
    public async Task<Stream> DownloadFileAsync(string serviceName, object obj)
    {
      if (obj == null)
        throw new ArgumentNullException("obj");
      using (HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, 
        String.Format("{0}/{1}",
          DATA_URI,
          EscapeDataName(serviceName, obj.GetType()))))
      {
        MultipartFormDataContent formDataContent = new MultipartFormDataContent();
        request.Content = formDataContent;
        formDataContent.Add(new StringContent(Phenix.Core.Reflection.Utilities.JsonSerialize(obj), Encoding.UTF8), "data");
        formDataContent.Add(new StringContent("0", Encoding.UTF8), "chunkNumber");
        using (HttpResponseMessage response = await HttpClient.SendAsync(request))
        {
          if (response.StatusCode != HttpStatusCode.OK)
            throw new HttpRequestException(await response.Content.ReadAsStringAsync());
          using (Stream sourceStream = await response.Content.ReadAsStreamAsync())
          {
            MemoryStream result = new MemoryStream();
            Phenix.Core.IO.StreamHelper.CopyBuffer(sourceStream, result);
            return result;
          }
        }
      }
    }

    /// <summary>
    /// 下载文件
    /// </summary>
    /// <param name="service">服务对象, 将被序列化传到服务端</param>
    /// <returns>已下载的文件流</returns>
    public Stream DownloadFile<T>(T service)
      where T : IService
    {
      try
      {
        return DownloadFileAsync(service).Result;
      }
      catch (AggregateException ex)
      {
        throw ex.InnerException;
      }
    }

    /// <summary>
    /// 下载文件
    /// </summary>
    /// <param name="serviceName">服务名, 在服务端注册的服务类全名(需实现IService接口)</param>
    /// <param name="obj">对象, 将被序列化传到服务端</param>
    /// <returns>已下载的文件流</returns>
    public Stream DownloadFile(string serviceName, object obj)
    {
      try
      {
        return DownloadFileAsync(serviceName, obj).Result;
      }
      catch (AggregateException ex)
      {
        throw ex.InnerException;
      }
    }

    #endregion

    #region DownloadBigFile

    /// <summary>
    /// 下载大文件
    /// </summary>
    /// <param name="service">服务对象, 将被序列化传到服务端</param>
    /// <param name="fileName">待保存的文件路径</param>
    /// <param name="doProgress">执行进度干预</param>
    public async Task DownloadBigFileAsync<T>(T service, string fileName, Func<object, FileChunkInfo, bool> doProgress)
      where T : IService
    {
      await DownloadBigFileAsync(String.Empty, service, fileName, doProgress);
    }

    /// <summary>
    /// 下载大文件
    /// </summary>
    /// <param name="serviceName">服务名, 在服务端注册的服务类全名(需实现IService接口)</param>
    /// <param name="obj">JSON格式对象, 将被传到服务端</param>
    /// <param name="fileName">待保存的文件路径</param>
    /// <param name="doProgress">执行进度干预</param>
    public async Task DownloadBigFileAsync(string serviceName, object obj, string fileName, Func<object, FileChunkInfo, bool> doProgress)
    {
      if (String.IsNullOrEmpty(fileName))
        throw new ArgumentNullException("fileName");
      using (FileStream fileStream = new FileStream(fileName, FileMode.Create, FileAccess.Write))
      {
        await DownloadBigFileAsync(serviceName, obj, fileStream, doProgress);
      }
    }

    /// <summary>
    /// 下载大文件
    /// </summary>
    /// <param name="service">服务对象, 将被序列化传到服务端</param>
    /// <param name="fileName">待保存的文件路径</param>
    /// <param name="doProgress">执行进度干预</param>
    public void DownloadBigFile<T>(T service, string fileName, Func<object, FileChunkInfo, bool> doProgress)
      where T : IService
    {
      try
      {
        DownloadBigFileAsync(service, fileName, doProgress).Wait();
      }
      catch (AggregateException ex)
      {
        throw ex.InnerException;
      }
    }

    /// <summary>
    /// 下载大文件
    /// </summary>
    /// <param name="serviceName">服务名, 在服务端注册的服务类全名(需实现IService接口)</param>
    /// <param name="obj">对象, 将被序列化传到服务端</param>
    /// <param name="fileName">待保存的文件路径</param>
    /// <param name="doProgress">执行进度干预</param>
    public void DownloadBigFile(string serviceName, object obj, string fileName, Func<object, FileChunkInfo, bool> doProgress)
    {
      try
      {
        DownloadBigFileAsync(serviceName, obj, fileName, doProgress).Wait();
      }
      catch (AggregateException ex)
      {
        throw ex.InnerException;
      }
    }

    /// <summary>
    /// 下载大文件
    /// </summary>
    /// <param name="service">服务对象, 将被序列化传到服务端</param>
    /// <param name="fileStream">待保存的文件流</param>
    /// <param name="doProgress">执行进度干预</param>
    public async Task DownloadBigFileAsync<T>(T service, Stream fileStream, Func<object, FileChunkInfo, bool> doProgress)
      where T : IService
    {
      await DownloadBigFileAsync(String.Empty, service, fileStream, doProgress);
    }

    /// <summary>
    /// 下载大文件
    /// </summary>
    /// <param name="serviceName">服务名, 在服务端注册的服务类全名(需实现IService接口)</param>
    /// <param name="obj">对象, 将被序列化传到服务端</param>
    /// <param name="fileStream">待保存的文件流</param>
    /// <param name="doProgress">执行进度干预</param>
    public async Task DownloadBigFileAsync(string serviceName, object obj, Stream fileStream, Func<object, FileChunkInfo, bool> doProgress)
    {
      if (fileStream == null)
        throw new ArgumentNullException("fileStream");
      for (int i = 1; i < Int32.MaxValue; i++)
      {
        FileChunkInfo fileChunkInfo = await DownloadBigFileAsync(serviceName, obj, i);
        if (doProgress != null && !doProgress(obj, fileChunkInfo))
          break;
        if (fileChunkInfo == null)
          break;
        FileHelper.WriteChunkInfo(fileStream, fileChunkInfo);
        if (fileChunkInfo.Over)
          break;
      }
    }

    /// <summary>
    /// 下载大文件
    /// </summary>
    /// <param name="service">服务对象, 将被序列化传到服务端</param>
    /// <param name="fileStream">待保存的文件流</param>
    /// <param name="doProgress">执行进度干预</param>
    public void DownloadBigFile<T>(T service, Stream fileStream, Func<object, FileChunkInfo, bool> doProgress)
      where T : IService
    {
      try
      {
        DownloadBigFileAsync(service, fileStream, doProgress).Wait();
      }
      catch (AggregateException ex)
      {
        throw ex.InnerException;
      }
    }

    /// <summary>
    /// 下载大文件
    /// </summary>
    /// <param name="serviceName">服务名, 在服务端注册的服务类全名(需实现IService接口)</param>
    /// <param name="obj">对象, 将被序列化传到服务端</param>
    /// <param name="fileStream">待保存的文件流</param>
    /// <param name="doProgress">执行进度干预</param>
    public void DownloadBigFile(string serviceName, object obj, Stream fileStream, Func<object, FileChunkInfo, bool> doProgress)
    {
      try
      {
        DownloadBigFileAsync(serviceName, obj, fileStream, doProgress).Wait();
      }
      catch (AggregateException ex)
      {
        throw ex.InnerException;
      }
    }

    /// <summary>
    /// 下载文件
    /// </summary>
    /// <param name="service">服务对象, 将被序列化传到服务端</param>
    /// <param name="chunkNumber">块号</param>
    /// <returns>文件块信息</returns>
    public async Task<FileChunkInfo> DownloadBigFileAsync<T>(T service, int chunkNumber)
      where T : IService
    {
      return await DownloadBigFileAsync(String.Empty, service, chunkNumber);
    }

    /// <summary>
    /// 下载文件
    /// </summary>
    /// <param name="serviceName">服务名, 在服务端注册的服务类全名(需实现IService接口)</param>
    /// <param name="obj">JSON格式对象, 将被传到服务端</param>
    /// <param name="chunkNumber">块号</param>
    /// <returns>文件块信息</returns>
    public async Task<FileChunkInfo> DownloadBigFileAsync(string serviceName, object obj, int chunkNumber)
    {
      if (obj == null)
        throw new ArgumentNullException("obj");
      using (HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post,
        String.Format("{0}/{1}",
          DATA_URI,
          EscapeDataName(serviceName, obj.GetType()))))
      {
        MultipartFormDataContent formDataContent = new MultipartFormDataContent();
        request.Content = formDataContent;
        formDataContent.Add(new StringContent(Phenix.Core.Reflection.Utilities.JsonSerialize(obj), Encoding.UTF8), "data");
        formDataContent.Add(new StringContent(chunkNumber.ToString(), Encoding.UTF8), "chunkNumber");
        using (HttpResponseMessage response = await HttpClient.SendAsync(request))
        {
          string result = await response.Content.ReadAsStringAsync();
          if (response.StatusCode != HttpStatusCode.OK)
            throw new HttpRequestException(result);
          return (FileChunkInfo)Phenix.Core.Reflection.Utilities.JsonDeserialize(result, typeof(FileChunkInfo));
        }
      }
    }

    /// <summary>
    /// 下载文件
    /// </summary>
    /// <param name="service">服务对象, 将被序列化传到服务端</param>
    /// <param name="chunkNumber">块号</param>
    /// <returns>文件块信息</returns>
    public FileChunkInfo DownloadBigFile<T>(T service, int chunkNumber)
      where T : IService
    {
      try
      {
        return DownloadBigFileAsync(service, chunkNumber).Result;
      }
      catch (AggregateException ex)
      {
        throw ex.InnerException;
      }
    }

    /// <summary>
    /// 下载文件
    /// </summary>
    /// <param name="serviceName">服务名, 在服务端注册的服务类全名(需实现IService接口)</param>
    /// <param name="obj">对象, 将被序列化传到服务端</param>
    /// <param name="chunkNumber">块号</param>
    /// <returns>文件块信息</returns>
    public FileChunkInfo DownloadBigFile(string serviceName, object obj, int chunkNumber)
    {
      try
      {
        return DownloadBigFileAsync(serviceName, obj, chunkNumber).Result;
      }
      catch (AggregateException ex)
      {
        throw ex.InnerException;
      }
    }

    #endregion

    #endregion
  }
}
