using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Phenix.Core.Mapping;
using Phenix.Core.Security.Cryptography;
using Phenix.Core.Web;

namespace Phenix.Web.Client
{
  /// <summary>
  /// 消息代理
  /// </summary>
  public sealed class SecurityProxy : ISecurityProxy
  {
    /// <summary>
    /// 初始化
    /// </summary>
    public SecurityProxy(HttpClient httpClient)
    {
      _httpClient = httpClient;
    }

    #region 属性

    private const string SECURITY_URI = "/api/Security";

    private readonly HttpClient _httpClient;
    /// <summary>
    /// HttpClient
    /// </summary>
    public HttpClient HttpClient
    {
      get { return _httpClient; }
    }
    System.Net.Http.HttpClient ISecurityProxy.HttpClient
    {
      get { return HttpClient; }
    }

    #endregion

    #region 方法

    /// <summary>
    /// 是否允许操作
    /// </summary>
    /// <param name="action">执行动作</param>
    /// <returns>是否被拒绝</returns>
    public async Task<bool> IsByDenyAsync<T>(ExecuteAction action)
    {
      return await IsByDenyAsync<T>(String.Empty, action);
    }

    /// <summary>
    /// 是否允许操作
    /// </summary>
    /// <param name="dataName">数据名, 在服务端注册的实体/实体集合/服务类全名(需实现IEntity/IEntityCollection/IService接口)</param>
    /// <param name="action">执行动作</param>
    /// <returns>是否被拒绝</returns>
    public async Task<bool> IsByDenyAsync<T>(string dataName, ExecuteAction action)
    {
      return await IsByDenyAsync(DataProxy.EscapeDataName(dataName, typeof(T)), action.ToString());
    }

    /// <summary>
    /// 是否允许操作
    /// </summary>
    /// <param name="action">执行动作</param>
    /// <returns>是否被拒绝</returns>
    public bool IsByDeny<T>(ExecuteAction action)
    {
      try
      {
        return IsByDenyAsync<T>(action).Result;
      }
      catch (AggregateException ex)
      {
        throw ex.InnerException;
      }
    }

    /// <summary>
    /// 是否允许操作
    /// </summary>
    /// <param name="dataName">数据名, 在服务端注册的实体集合类全名(需实现IEntity/IEntityCollection/IService接口)</param>
    /// <param name="action">执行动作</param>
    /// <returns>是否被拒绝</returns>
    public bool IsByDeny<T>(string dataName, ExecuteAction action)
    {
      try
      {
        return IsByDenyAsync<T>(dataName, action).Result;
      }
      catch (AggregateException ex)
      {
        throw ex.InnerException;
      }
    }

    /// <summary>
    /// 是否允许操作
    /// </summary>
    /// <param name="controllerName">标识服务端ApiController名称</param>
    /// <param name="actionName">标识服务端ApiController函数名</param>
    /// <returns>是否被拒绝</returns>
    public async Task<bool> IsByDenyAsync(string controllerName, string actionName)
    {
      using (HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get,
        String.Format("{0}?typeName={1}&actionName={2}",
          SECURITY_URI,
          Uri.EscapeDataString(controllerName), Uri.EscapeDataString(actionName))))
      using (HttpResponseMessage response = await HttpClient.SendAsync(request))
      {
        string result = await response.Content.ReadAsStringAsync();
        if (response.StatusCode != HttpStatusCode.OK)
          throw new HttpRequestException(result);
        return Boolean.Parse(result);
      }
    }

    /// <summary>
    /// 是否允许操作
    /// </summary>
    /// <param name="controllerName">标识服务端ApiController名称</param>
    /// <param name="actionName">标识服务端ApiController函数名</param>
    /// <returns>是否被拒绝</returns>
    public bool IsByDeny(string controllerName, string actionName)
    {
      try
      {
        return IsByDenyAsync(controllerName, actionName).Result;
      }
      catch (AggregateException ex)
      {
        throw ex.InnerException;
      }
    }

    /// <summary>
    /// 登录验证
    /// </summary>
    /// <returns>成功否</returns>
    public async Task<bool> LogOnAsync()
    {
      return await LogOnVerifyAsync(HttpClient.UserIdentity.UserNumber);
    }

    /// <summary>
    /// 登录验证
    /// </summary>
    /// <returns>成功否</returns>
    public bool LogOn()
    {
      return LogOnVerify(DateTime.Now.ToString("u"));
    }

    /// <summary>
    /// 登录验证
    /// </summary>
    /// <returns>返回HttpResponseMessage的Content.ReadAsStringAsync()可获取提示信息</returns>
    public async Task<HttpResponseMessage> TryLogOnAsync()
    {
      return await TryLogOnVerifyAsync(DateTime.Now.ToString("u"));
    }

    /// <summary>
    /// 登录验证
    /// </summary>
    /// <returns>返回HttpResponseMessage的Content.ReadAsStringAsync()可获取提示信息</returns>
    public HttpResponseMessage TryLogOn()
    {
      return TryLogOnVerify(DateTime.Now.ToString("u"));
    }

    /// <summary>
    /// 登录核实
    /// </summary>
    /// <param name="tag">标签</param>
    /// <returns>成功否</returns>
    public async Task<bool> LogOnVerifyAsync(string tag)
    {
      using (HttpResponseMessage response = await TryLogOnVerifyAsync(tag))
      {
        return response.StatusCode == HttpStatusCode.OK;
      }
    }

    /// <summary>
    /// 登录核实
    /// </summary>
    /// <param name="tag">标签</param>
    /// <returns>成功否</returns>
    public bool LogOnVerify(string tag)
    {
      try
      {
        return LogOnVerifyAsync(tag).Result;
      }
      catch (AggregateException ex)
      {
        throw ex.InnerException;
      }
    }

    /// <summary>
    /// 登录核实
    /// </summary>
    /// <param name="tag">标签</param>
    /// <returns>返回HttpResponseMessage的Content.ReadAsStringAsync()可获取提示信息</returns>
    public async Task<HttpResponseMessage> TryLogOnVerifyAsync(string tag)
    {
      HttpResponseMessage response = await HttpClient.PostAsync(SECURITY_URI,
        new StringContent(RijndaelCryptoTextProvider.Encrypt(HttpClient.UserIdentity.Password, tag), Encoding.UTF8));
      HttpClient.UserIdentity.IsAuthenticated = response.StatusCode == HttpStatusCode.OK;
      HttpClient.UserIdentity.AuthenticatedMessage = await response.Content.ReadAsStringAsync();
      return response;
    }

    /// <summary>
    /// 登录核实
    /// </summary>
    /// <returns>返回HttpResponseMessage的Content.ReadAsStringAsync()可获取提示信息</returns>
    public HttpResponseMessage TryLogOnVerify(string tag)
    {
      try
      {
        return TryLogOnVerifyAsync(tag).Result;
      }
      catch (AggregateException ex)
      {
        throw ex.InnerException;
      }
    }

    /// <summary>
    /// 修改登录口令
    /// </summary>
    /// <param name="newPassword">新登录口令</param>
    public async Task<bool> ChangePasswordAsync(string newPassword)
    {
      using (HttpResponseMessage response = await HttpClient.PutAsync(SECURITY_URI,
        new StringContent(RijndaelCryptoTextProvider.Encrypt(HttpClient.UserIdentity.Password, newPassword), Encoding.UTF8)))
      {
        HttpClient.UserIdentity.IsAuthenticated = response.StatusCode == HttpStatusCode.OK;
        HttpClient.UserIdentity.AuthenticatedMessage = await response.Content.ReadAsStringAsync();
        if (HttpClient.UserIdentity.IsAuthenticated)
          HttpClient.UserIdentity.Password = newPassword;
        return HttpClient.UserIdentity.IsAuthenticated;
      }
    }

    /// <summary>
    /// 修改登录口令
    /// </summary>
    /// <param name="newPassword">新登录口令</param>
    public bool ChangePassword(string newPassword)
    {
      try
      {
        return ChangePasswordAsync(newPassword).Result;
      }
      catch (AggregateException ex)
      {
        throw ex.InnerException;
      }
    }

    /// <summary>
    /// 登出
    /// </summary>
    public async Task<bool> LogOffAsync()
    {
      using (HttpResponseMessage response = await HttpClient.DeleteAsync(SECURITY_URI))
      {
        return response.StatusCode == HttpStatusCode.OK;
      }
    }


    #endregion
  }
}
