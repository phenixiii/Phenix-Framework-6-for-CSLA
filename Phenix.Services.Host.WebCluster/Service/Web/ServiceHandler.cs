using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http.Dispatcher;
using Phenix.Core;
using Phenix.Core.Web;
using Phenix.Services.Host.WebCluster.Core;

namespace Phenix.Services.Host.WebCluster.Service.Web
{
  internal class ServiceHandler : DelegatingHandler
  {
    public ServiceHandler(System.Web.Http.SelfHost.HttpSelfHostConfiguration configuration)
      : base(new HttpControllerDispatcher(configuration)) { }

    #region 方法

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
      Channel channel = ChannelManager.Default.GetChannel();
      if (channel == null)
        return request.CreateErrorResponse(HttpStatusCode.ServiceUnavailable, "请在WebCluster上设置需要代理的服务!");
      channel.ResetStatus();
      try
      {
        using (HttpRequestMessage channelRequest = new HttpRequestMessage(HttpMethod.Post, request.RequestUri.PathAndQuery))
        {
          foreach (KeyValuePair<string, IEnumerable<string>> kvp in request.Headers)
            channelRequest.Headers.Add(kvp.Key, kvp.Value);
          if (request.Method != HttpMethod.Post)
            if (!channelRequest.Headers.Contains(WebConfig.METHOD_OVERRIDE_HEADER_NAME))
              channelRequest.Headers.Add(WebConfig.METHOD_OVERRIDE_HEADER_NAME, request.Method.ToString());
          channelRequest.Content = request.Content;
          HttpResponseMessage channelResult = await channel.HttpClient.SendAsync(channelRequest, cancellationToken);
          HttpResponseMessage result = request.CreateResponse(channelResult.StatusCode);
          result.Content = channelResult.Content;
          return result;
        }
      }
      catch (AggregateException ex)
      {
        channel.ResetStatus(ex);
        return AppConfig.Debugging ? 
          request.CreateErrorResponse(HttpStatusCode.BadGateway, ex.InnerException) : 
          request.CreateErrorResponse(HttpStatusCode.BadGateway, ex.InnerException != null ? ex.InnerException.Message : ex.Message);
      }
    }

    #endregion
  }
}
