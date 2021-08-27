using System;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Phenix.Core;
using Phenix.Core.Mapping;
using Phenix.Core.Security;
using Phenix.Core.Security.Cryptography;
using Phenix.Core.Security.Exception;
using Phenix.Services.Host.Core;

namespace Phenix.Services.Host.Service.Web
{
    public sealed class SecurityController : ApiController
    {
        #region 属性

        #region 配置项

        private static bool? _existThirdParty;

        /// <summary>
        /// 存在第三方服务
        /// 默认：false
        /// </summary>
        public static bool ExistThirdParty
        {
            get { return AppSettings.GetProperty(ref _existThirdParty, false); }
            set { AppSettings.SetProperty(ref _existThirdParty, value); }
        }

        private static string _thirdPartyService;

        /// <summary>
        /// 第三方服务地址
        /// 默认：http://localhost:5000
        /// </summary>
        public static string ThirdPartyService
        {
            get { return AppSettings.GetProperty(ref _thirdPartyService, "http://localhost:5000"); }
            set { AppSettings.SetProperty(ref _thirdPartyService, value); }
        }

        #endregion

        #endregion

        #region 事件

        internal static event Action<DataSecurityEventArgs> Changed;

        private static void OnChanged(DataSecurityEventArgs e)
        {
            Action<DataSecurityEventArgs> action = Changed;
            if (action != null)
                action(e);
        }

        #endregion

        #region 方法

        /// <summary>
        /// 取身份信息
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
        public HttpResponseMessage Get()
        {
            try
            {
                return Phenix.Core.Web.Utilities.PackResponse(Request, UserIdentity.CurrentIdentity);
            }
            catch (Exception ex)
            {
                return Phenix.Core.Web.Utilities.PackErrorResponse(Request, ex);
            }
        }

        /// <summary>
        /// 确定是否被拒绝
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
        public HttpResponseMessage Get(string typeName, string actionName)
        {
            try
            {
                ServiceManager.CheckIn();
                ExecuteAction action;
                if (Enum.TryParse(actionName, out action))
                    return Phenix.Core.Web.Utilities.PackResponse(Request,
                        UserIdentity.IsByDeny(UserIdentity.CurrentIdentity, DataController.LoadDataType(typeName), action, false));
                else
                {
                    Type type = Phenix.Core.Reflection.Utilities.LoadType(typeName);
                    if (type == null)
                    {
                        if (!typeName.EndsWith("Controller", StringComparison.Ordinal))
                            type = Phenix.Core.Reflection.Utilities.LoadType(typeName + "Controller");
                        if (type == null)
                            throw new InvalidOperationException(typeName + "在服务端未定义");
                    }

                    return Phenix.Core.Web.Utilities.PackResponse(Request,
                        UserIdentity.IsByDeny(UserIdentity.CurrentIdentity, type, actionName, false));
                }
            }
            catch (Exception ex)
            {
                return Phenix.Core.Web.Utilities.PackErrorResponse(Request, ex);
            }
        }

        /// <summary>
        /// 登录
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
        public HttpResponseMessage Post()
        {
            try
            {
                ServiceManager.CheckIn();
                UserIdentity identity = UserIdentity.CurrentIdentity;
                OnChanged(new DataSecurityEventArgs(identity, true));
                string tag = Request.Content.ReadAsStringAsync().Result;
                try
                {
                    tag = RijndaelCryptoTextProvider.Decrypt(identity.DynamicPassword ?? identity.Password, tag);
                }
                catch (FormatException)
                {
                    tag = RijndaelCryptoTextProvider7.Decrypt(MD5CryptoTextProvider.ComputeHash(identity.Password), tag);
                }

                if (String.IsNullOrEmpty(tag))
                    throw new InvalidOperationException("报文体tag不允许为空!");
                DateTime now;
                if (!DateTime.TryParse(tag, out now))
                    try
                    {
                        bool? passed = DataSecurityHub.LogOnVerify(identity.UserNumber, tag);
                        if (passed.HasValue && !passed.Value)
                            throw new UserVerifyException();
                    }
                    catch (Exception ex)
                    {
                        throw new UserVerifyException(ex);
                    }

                return Phenix.Core.Web.Utilities.PackResponse(Request, ExistThirdParty
                    ? RSACryptoTextProvider7.Encrypt(GetPublicKey(identity.Name), MD5CryptoTextProvider.ComputeHash(identity.Password), true)
                    : identity.AuthenticatedMessage);
                ;
            }
            catch (UserNotFoundException ex)
            {
                return Phenix.Core.Web.Utilities.PackResponse(Request, ExistThirdParty
                    ? null
                    : Phenix.Core.Web.Utilities.PackErrorResponse(Request, ex));
            }
            catch (Exception ex)
            {
                return Phenix.Core.Web.Utilities.PackErrorResponse(Request, ex);
            }
        }

        private static string GetPublicKey(string name)
        {
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(ThirdPartyService);
                using (HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, "/api/security/one-off-key-pair?name=" + Uri.EscapeDataString(name)))
                using (HttpResponseMessage response = client.SendAsync(request).Result)
                {
                    string result = response.Content.ReadAsStringAsync().Result;
                    if (response.StatusCode != HttpStatusCode.OK)
                        throw new HttpRequestException(result);
                    return result;
                }
            }
        }

        /// <summary>
        /// 修改登录口令
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
        public HttpResponseMessage Put()
        {
            try
            {
                ServiceManager.CheckIn();
                UserIdentity identity = UserIdentity.CurrentIdentity;
                OnChanged(new DataSecurityEventArgs(identity, true));
                bool result = DataSecurityHub.ChangePassword(Request.Content.ReadAsStringAsync().Result, false, identity);
                return Request.CreateResponse(result ? HttpStatusCode.OK : HttpStatusCode.BadRequest);
            }
            catch (Exception ex)
            {
                return Phenix.Core.Web.Utilities.PackErrorResponse(Request, ex);
            }
        }

        /// <summary>
        /// 登出
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
        public HttpResponseMessage Delete()
        {
            try
            {
                UserIdentity identity = UserIdentity.CurrentIdentity;
                OnChanged(new DataSecurityEventArgs(identity, false));
                DataSecurityHub.LogOff(identity);
                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                return Phenix.Core.Web.Utilities.PackErrorResponse(Request, ex);
            }
        }

        #endregion
    }
}