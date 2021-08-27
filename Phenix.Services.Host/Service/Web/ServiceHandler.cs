using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http.Dispatcher;
using Phenix.Core.Net;
using Phenix.Core.Security;
using Phenix.Core.Security.Cryptography;
using Phenix.Core.Web;

namespace Phenix.Services.Host.Service.Web
{
  internal class ServiceHandler : DelegatingHandler
  {
    public ServiceHandler(System.Web.Http.SelfHost.HttpSelfHostConfiguration configuration)
      : base(new HttpControllerDispatcher(configuration)) { }

    #region 方法

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
    protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
      bool isSecurityController = String.Compare(request.RequestUri.AbsolutePath, WebConfig.SECURITY_URI, StringComparison.OrdinalIgnoreCase) == 0;
      try
      {
        IEnumerable<string> values;
        if (request.Headers.TryGetValues(WebConfig.METHOD_OVERRIDE_HEADER_NAME, out values))
          request.Method = new HttpMethod(values.First());
        if (request.Headers.TryGetValues(WebConfig.AUTHORIZATION_HEADER_NAME, out values))
        {
          string contentMD5 = null;
          bool contentEncrypted = false;
          string timestamp;
          string signature;
          string[] strings = values.First().Split(',');
          if (strings.Length == 3)
          {
            //身份认证格式: [UserNumber],[timestamp],[signature = Encrypt(Password, timestamp)]
            timestamp = strings[1];
            signature = strings[2];
          }
          else if (strings.Length == 5)
          {
            //身份认证格式: [UserNumber],[timestamp],[contentMD5 = MD5(content)/""],[contentEncrypted = 0/1],[signature = Encrypt(Password, timestamp+contentMD5)]
            contentMD5 = strings[2];
            contentEncrypted = strings[3] == "1";
            timestamp = strings[1] + strings[2];
            signature = strings[4];
          }
          else
            throw new InvalidOperationException(String.Format("报文头{0}内容({1})格式错误!", WebConfig.AUTHORIZATION_HEADER_NAME, values.First()));
          DataSecurityContext context = DataSecurityHub.CheckIn(NetConfig.LocalAddress, NetConfig.LocalAddress, Uri.UnescapeDataString(strings[0]), timestamp, signature, isSecurityController);
          if (!String.IsNullOrEmpty(contentMD5) || contentEncrypted)
          {
            string content = request.Content.ReadAsStringAsync().Result;
            if (contentEncrypted)
            {
              content = RijndaelCryptoTextProvider.Decrypt(context.Identity.DynamicPassword ?? context.Identity.Password, content);
              request.Content = new StringContent(content,
                request.Content.Headers.ContentType != null && !String.IsNullOrEmpty(request.Content.Headers.ContentType.CharSet) ? Encoding.GetEncoding(request.Content.Headers.ContentType.CharSet) : null,
                request.Content.Headers.ContentType != null ? request.Content.Headers.ContentType.MediaType : null);
            }
            if (!String.IsNullOrEmpty(contentMD5) && String.Compare(contentMD5, MD5CryptoTextProvider.ComputeHash(content), StringComparison.OrdinalIgnoreCase) != 0)
              throw new InvalidOperationException(String.Format("报文体MD5签名({0})无效!", contentMD5));
          }
        }
        else
          DataSecurityHub.CheckIn(NetConfig.LocalAddress, NetConfig.LocalAddress, UserIdentity.GuestUserNumber, null, null, false);
        return base.SendAsync(request, cancellationToken);
      }
      catch (UserPasswordComplexityException ex)
      {
        if (isSecurityController && request.Method == HttpMethod.Put)
        {
          UserIdentity.CurrentIdentity = ex.Identity;
          return base.SendAsync(request, cancellationToken);
        }
        return Task.FromResult(Phenix.Core.Web.Utilities.PackErrorResponse(request, ex));
      }
      catch (Exception ex)
      {
        return Task.FromResult(Phenix.Core.Web.Utilities.PackErrorResponse(request, ex));
      }
    }
    
    #endregion
  }
}
