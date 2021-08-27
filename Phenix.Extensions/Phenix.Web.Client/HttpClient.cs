using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Phenix.Core.Web;
using Phenix.Web.Client.Security;

namespace Phenix.Web.Client
{
  /// <summary>
  /// HttpClient
  /// 配套Bin.Top目录下Host程序提供的WabAPI服务
  /// </summary>
  public class HttpClient : System.Net.Http.HttpClient
  {
    private HttpClient(HttpClientHandler handler, Uri baseAddress, UserIdentity userIdentity)
      : base(handler)
    {
      base.BaseAddress = baseAddress;

      _userIdentity = userIdentity;

      _securityProxy = new SecurityProxy(this);
      _dataProxy = new DataProxy(this);
      _messageProxy = new MessageProxy(this);

      AppHub.SecurityProxy = _securityProxy;
      AppHub.DataProxy = _dataProxy;
      AppHub.MessageProxy = _messageProxy;
    }

    private HttpClient(HttpClientHandler handler, string host, int port, UserIdentity userIdentity)
      : this(handler, new Uri(String.Format(@"http://{0}:{1}", host, port)), userIdentity) { }

    /// <summary>
    /// 初始化
    /// </summary>
    /// <param name="baseAddress">服务地址</param>
    /// <param name="userIdentity">用户身份</param>
    public HttpClient(Uri baseAddress, UserIdentity userIdentity)
      : this(new ClientHandler(userIdentity), baseAddress, userIdentity) { }

    /// <summary>
    /// 初始化
    /// </summary>
    /// <param name="host">服务地址</param>
    /// <param name="userIdentity">用户身份</param>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1057:StringUriOverloadsCallSystemUriOverloads")]
    public HttpClient(string host, UserIdentity userIdentity)
      : this(new ClientHandler(userIdentity), host, DEFAULT_PORT, userIdentity) { }

    /// <summary>
    /// 初始化
    /// </summary>
    /// <param name="host">服务地址</param>
    /// <param name="port">端口号</param>
    /// <param name="userIdentity">用户身份</param>
    public HttpClient(string host, int port, UserIdentity userIdentity)
      : this(new ClientHandler(userIdentity), host, port, userIdentity) { }

    /// <summary>
    /// 初始化
    /// </summary>
    /// <param name="baseAddress">服务地址</param>
    /// <param name="userIdentity">用户身份</param>
    /// <param name="contentVerifing">报文核实</param>
    /// <param name="contentEncrypted">报文加密</param>
    public HttpClient(Uri baseAddress, UserIdentity userIdentity, bool contentVerifing, bool contentEncrypted)
      : this(new ClientHandler(userIdentity, contentVerifing, contentEncrypted), baseAddress, userIdentity) { }

    /// <summary>
    /// 初始化
    /// </summary>
    /// <param name="host">服务地址</param>
    /// <param name="userIdentity">用户身份</param>
    /// <param name="contentVerifing">报文核实</param>
    /// <param name="contentEncrypted">报文加密</param>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1057:StringUriOverloadsCallSystemUriOverloads")]
    public HttpClient(string host, UserIdentity userIdentity, bool contentVerifing, bool contentEncrypted)
      : this(new ClientHandler(userIdentity, contentVerifing, contentEncrypted), host, DEFAULT_PORT, userIdentity) { }

    /// <summary>
    /// 初始化
    /// </summary>
    /// <param name="host">服务地址</param>
    /// <param name="port">端口号</param>
    /// <param name="userIdentity">用户身份</param>
    /// <param name="contentVerifing">报文核实</param>
    /// <param name="contentEncrypted">报文加密</param>
    public HttpClient(string host, int port, UserIdentity userIdentity, bool contentVerifing, bool contentEncrypted)
      : this(new ClientHandler(userIdentity, contentVerifing, contentEncrypted), host, port, userIdentity) { }

    #region 属性

    internal const string AUTHORIZATION_HEADER_NAME = "Phenix-Authorization";
    
    private const int DEFAULT_PORT = 8080;

    private readonly UserIdentity _userIdentity;
    /// <summary>
    /// 用户身份
    /// </summary>
    public UserIdentity UserIdentity
    {
      get { return _userIdentity; }
    }

    private readonly SecurityProxy _securityProxy;
    /// <summary>
    /// 安全代理
    /// </summary>
    public SecurityProxy SecurityProxy
    {
      get { return _securityProxy; }
    }

    private readonly DataProxy _dataProxy;
    /// <summary>
    /// 数据代理
    /// </summary>
    public DataProxy DataProxy
    {
      get { return _dataProxy; }
    }

    private readonly MessageProxy _messageProxy;
    /// <summary>
    /// 消息代理
    /// </summary>
    public MessageProxy MessageProxy
    {
      get { return _messageProxy; }
    }

    #endregion

    #region 方法

    /// <summary>
    /// 释放
    /// </summary>
    protected override void Dispose(bool disposing)
    {
      if (disposing)
        _messageProxy.Dispose();
      base.Dispose(disposing);
    }

    #region CanCall

    /// <summary>
    /// 是否允许Call
    /// </summary>
    /// <param name="controllerName">标识服务端ApiController名称</param>
    /// <param name="actionName">标识服务端ApiController函数名</param>
    /// <returns>是否被拒绝</returns>
    public async Task<bool> CanCallAsync(string controllerName, string actionName)
    {
      bool result = await SecurityProxy.IsByDenyAsync(controllerName, actionName);
      return !result;
    }

    /// <summary>
    /// 是否允许Call
    /// </summary>
    /// <param name="controllerName">标识服务端ApiController名称</param>
    /// <param name="actionName">标识服务端ApiController函数名</param>
    /// <returns>是否被拒绝</returns>
    public bool CanCall(string controllerName, string actionName)
    {
      try
      {
        return CanCallAsync(controllerName, actionName).Result;
      }
      catch (AggregateException ex)
      {
        throw ex.InnerException;
      }
    }

    #endregion

    #region Call

    /// <summary>
    /// 呼叫
    /// actionName = null
    /// parameters = null
    /// </summary>
    /// <param name="method">请求方法</param>
    /// <param name="controllerName">标识服务端ApiController名称</param>
    /// <param name="data">传到服务端的数据</param>
    /// <returns>返回对象</returns>
    public async Task<T> CallAsync<T>(HttpMethod method, string controllerName, object data)
      where T : class
    {
      return await CallAsync<T>(method, controllerName, null, null, data);
    }

    /// <summary>
    /// 呼叫
    /// </summary>
    /// <param name="method">请求方法</param>
    /// <param name="controllerName">标识服务端ApiController名称</param>
    /// <param name="actionName">标识服务端ApiController函数名</param>
    /// <param name="parameters">标识服务端ApiController函数参数集合</param>
    /// <param name="data">传到服务端的数据</param>
    /// <returns>返回对象</returns>
    public async Task<T> CallAsync<T>(HttpMethod method, string controllerName, string actionName, IDictionary<string, object> parameters, object data)
      where T : class 
    {
      if (controllerName == null)
        throw new ArgumentNullException("controllerName");
      StringBuilder parameterExpr = new StringBuilder();
      if (parameters != null && parameters.Count > 0)
      {
        foreach (KeyValuePair<string, object> kvp in parameters)
          parameterExpr.AppendFormat("{0}={1}&", kvp.Key, kvp.Value);
        parameterExpr.Remove(parameterExpr.Length - 1, 1);
      }
      using (HttpRequestMessage request = new HttpRequestMessage(method, 
        String.Format("/api/{0}{1}{2}{3}{4}", controllerName,
          !String.IsNullOrEmpty(actionName) ? "/" : null, actionName,
          parameterExpr.Length > 0 ? "?" : null, parameterExpr.ToString())))
      {
        request.Content = new StringContent(Phenix.Core.Reflection.Utilities.JsonSerialize(data), Encoding.UTF8);
        using (HttpResponseMessage response = await SendAsync(request))
        {
          string result = await response.Content.ReadAsStringAsync();
          if (response.StatusCode != HttpStatusCode.OK)
            throw new HttpRequestException(result);
          return (T)Phenix.Core.Reflection.Utilities.JsonDeserialize(result, typeof(T));
        }
      }
    }

    /// <summary>
    /// 呼叫
    /// actionName = null
    /// parameters = null
    /// </summary>
    /// <param name="method">请求方法</param>
    /// <param name="controllerName">标识服务端ApiController名称</param>
    /// <param name="data">传到服务端的数据</param>
    /// <returns>返回对象</returns>
    public T Call<T>(HttpMethod method, string controllerName, object data)
      where T : class
    {
      return CallAsync<T>(method, controllerName, data).Result;
    }

    /// <summary>
    /// 呼叫
    /// </summary>
    /// <param name="method">请求方法</param>
    /// <param name="controllerName">标识服务端的ApiController名称</param>
    /// <param name="actionName">标识服务端的ApiController函数名称</param>
    /// <param name="parameters">标识服务端ApiController函数参数集合</param>
    /// <param name="data">传到服务端的数据</param>
    /// <returns>返回对象</returns>
    public T Call<T>(HttpMethod method, string controllerName, string actionName, IDictionary<string, object> parameters, object data)
      where T : class
    {
      try
      {
        return CallAsync<T>(method, controllerName, actionName, parameters, data).Result;
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
