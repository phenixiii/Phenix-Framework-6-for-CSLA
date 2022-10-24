using System.Net.Http;
using System.Threading.Tasks;
using Phenix.Core.Mapping;

namespace Phenix.Core.Web
{
  /// <summary>
  /// 安全代理接口
  /// </summary>
  public interface ISecurityProxy
  {
    #region 属性

    /// <summary>
    /// HttpClient
    /// </summary>
    HttpClient HttpClient { get; }

    #endregion

    #region 方法

    /// <summary>
    /// 是否允许操作
    /// </summary>
    /// <param name="action">执行动作</param>
    /// <returns>是否被拒绝</returns>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
    Task<bool> IsByDenyAsync<T>(ExecuteAction action);

    /// <summary>
    /// 是否允许操作
    /// </summary>
    /// <param name="dataName">数据名, 在服务端注册的实体集合类全名(需实现IEntity/IEntityCollection/IService接口)</param>
    /// <param name="action">执行动作</param>
    /// <returns>是否被拒绝</returns>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
    Task<bool> IsByDenyAsync<T>(string dataName, ExecuteAction action);

    /// <summary>
    /// 是否允许操作
    /// </summary>
    /// <param name="action">执行动作</param>
    /// <returns>是否被拒绝</returns>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
    bool IsByDeny<T>(ExecuteAction action);

    /// <summary>
    /// 是否允许操作
    /// </summary>
    /// <param name="dataName">数据名, 在服务端注册的实体集合类全名(需实现IEntity/IEntityCollection/IService接口)</param>
    /// <param name="action">执行动作</param>
    /// <returns>是否被拒绝</returns>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
    bool IsByDeny<T>(string dataName, ExecuteAction action);

    /// <summary>
    /// 是否允许操作
    /// </summary>
    /// <param name="controllerName">标识服务端ApiController名称</param>
    /// <param name="actionName">标识服务端ApiController函数名</param>
    /// <returns>是否被拒绝</returns>
    Task<bool> IsByDenyAsync(string controllerName, string actionName);

    /// <summary>
    /// 是否允许操作
    /// </summary>
    /// <param name="controllerName">标识服务端ApiController名称</param>
    /// <param name="actionName">标识服务端ApiController函数名</param>
    /// <returns>是否被拒绝</returns>
   bool IsByDeny(string controllerName, string actionName);

    /// <summary>
    /// 登录验证
    /// </summary>
    /// <returns>成功否</returns>
    Task<bool> LogOnAsync();

    /// <summary>
    /// 登录验证
    /// </summary>
    /// <returns>成功否</returns>
    bool LogOn();

    /// <summary>
    /// 登录验证
    /// </summary>
    /// <returns>返回HttpResponseMessage的Content.ReadAsStringAsync()可获取提示信息</returns>
    Task<HttpResponseMessage> TryLogOnAsync();

    /// <summary>
    /// 登录验证
    /// </summary>
    /// <returns>返回HttpResponseMessage的Content.ReadAsStringAsync()可获取提示信息</returns>
    HttpResponseMessage TryLogOn();

    /// <summary>
    /// 登录核实
    /// </summary>
    /// <param name="tag">标签</param>
    /// <returns>成功否</returns>
    Task<bool> LogOnVerifyAsync(string tag);

    /// <summary>
    /// 登录核实
    /// </summary>
    /// <param name="tag">标签</param>
    /// <returns>成功否</returns>
    bool LogOnVerify(string tag);

    /// <summary>
    /// 登录核实
    /// </summary>
    /// <param name="tag">标签</param>
    /// <returns>返回HttpResponseMessage的Content.ReadAsStringAsync()可获取提示信息</returns>
    Task<HttpResponseMessage> TryLogOnVerifyAsync(string tag);

    /// <summary>
    /// 登录核实
    /// </summary>
    /// <returns>返回HttpResponseMessage的Content.ReadAsStringAsync()可获取提示信息</returns>
    HttpResponseMessage TryLogOnVerify(string tag);

    /// <summary>
    /// 修改登录口令
    /// </summary>
    /// <param name="newPassword">新登录口令</param>
    Task<bool> ChangePasswordAsync(string newPassword);

    /// <summary>
    /// 修改登录口令
    /// </summary>
    /// <param name="newPassword">新登录口令</param>
    bool ChangePassword(string newPassword);

    /// <summary>
    /// 登出
    /// </summary>
    Task<bool> LogOffAsync();

    #endregion
  }
}
