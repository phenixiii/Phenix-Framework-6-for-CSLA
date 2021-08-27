using System;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Phenix.Services.Host.Core;

namespace Phenix.Services.Host.Service.Web
{
  public sealed class MessageController : ApiController
  {
    /// <summary>
    /// 发送消息
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
    public HttpResponseMessage Post(string receiver)
    {
      try
      {
        ServiceManager.CheckIn();
        Phenix.Core.Message.MessageHub.Send(receiver, Request.Content.ReadAsStringAsync().Result);
        return Request.CreateResponse(HttpStatusCode.OK);
      }
      catch (Exception ex)
      {
        return Phenix.Core.Web.Utilities.PackErrorResponse(Request, ex);
      }
    }

    /// <summary>
    /// 收取消息
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
    public HttpResponseMessage Get()
    {
      try
      {
        ServiceManager.CheckIn();
        return Phenix.Core.Web.Utilities.PackResponse(Request, Phenix.Core.Message.MessageHub.Receive());
      }
      catch (Exception ex)
      {
        return Phenix.Core.Web.Utilities.PackErrorResponse(Request, ex);
      }
    }

    /// <summary>
    /// 确认收到
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
    public HttpResponseMessage Delete(long id, bool burn)
    {
      try
      {
        ServiceManager.CheckIn();
        Phenix.Core.Message.MessageHub.AffirmReceived(id, burn);
        return Request.CreateResponse(HttpStatusCode.OK);
      }
      catch (Exception ex)
      {
        return Phenix.Core.Web.Utilities.PackErrorResponse(Request, ex);
      }
    }
  }
}
