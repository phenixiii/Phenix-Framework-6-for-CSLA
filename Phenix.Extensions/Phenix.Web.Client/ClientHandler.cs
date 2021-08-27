using System;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Phenix.Core.Security.Cryptography;
using Phenix.Web.Client.Security;

namespace Phenix.Web.Client
{
  internal class ClientHandler : HttpClientHandler
  {
    public ClientHandler(UserIdentity userIdentity)
      : base()
    {
      _userIdentity = userIdentity;
    }

    public ClientHandler(UserIdentity userIdentity, bool contentVerify, bool contentEncrypt)
      : this(userIdentity)
    {
      _contentVerify = contentVerify;
      _contentEncrypt = contentEncrypt;
    }

    #region 属性

    private const string METHOD_OVERRIDE_HEADER_NAME = "X-HTTP-Method-Override";

    private readonly UserIdentity _userIdentity;

    private readonly bool _contentVerify;
    private readonly bool _contentEncrypt;

    #endregion

    #region 方法

    protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
      //测试突破某些HTTP代理对方法的限制, 真实环境可注释掉
      if (request.Method == HttpMethod.Put || request.Method == HttpMethod.Delete)
      {
        request.Headers.Add(METHOD_OVERRIDE_HEADER_NAME, request.Method.ToString());
        request.Method = HttpMethod.Post;
      }

      string timestamp = Guid.NewGuid().ToString(); //也允许采用其他随机数形式, 只要在一个LogOn到LogOff周期内不发生重复即可
      if ((_contentVerify || _contentEncrypt) && !request.Content.IsMimeMultipartContent())
      {
        //身份认证格式: [UserNumber],[timestamp],[contentMD5 = MD5(content)/""],[contentEncrypted = 0/1],[signature = Encrypt(Password, timestamp+contentMD5)]
        string content = request.Content.ReadAsStringAsync().Result;
        string contentMD5 = _contentVerify ? MD5CryptoTextProvider.ComputeHash(content) : String.Empty;
        request.Headers.Add(HttpClient.AUTHORIZATION_HEADER_NAME,
          String.Format("{0},{1},{2},{3},{4}", Uri.EscapeDataString(_userIdentity.UserNumber), timestamp, contentMD5, _contentEncrypt ? 1 : 0,
            RijndaelCryptoTextProvider.Encrypt(_userIdentity.Password, timestamp + contentMD5)));
        if (_contentEncrypt)
          request.Content = new StringContent(RijndaelCryptoTextProvider.Encrypt(_userIdentity.Password, content),
            request.Content.Headers.ContentType != null && !String.IsNullOrEmpty(request.Content.Headers.ContentType.CharSet) ? Encoding.GetEncoding(request.Content.Headers.ContentType.CharSet) : null,
            request.Content.Headers.ContentType != null ? request.Content.Headers.ContentType.MediaType : null);
      }
      else
      {
        //身份认证格式: [UserNumber],[timestamp],[signature = Encrypt(Password, timestamp)]
        request.Headers.Add(HttpClient.AUTHORIZATION_HEADER_NAME,
          String.Format("{0},{1},{2}", Uri.EscapeDataString(_userIdentity.UserNumber), timestamp,
            RijndaelCryptoTextProvider.Encrypt(_userIdentity.Password, timestamp)));
      }

      return base.SendAsync(request, cancellationToken);
    }

    #endregion
  }
}
