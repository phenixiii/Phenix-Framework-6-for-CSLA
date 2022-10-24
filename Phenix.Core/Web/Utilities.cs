using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Security.Authentication;
using System.Text;
using Phenix.Core.Data;
using Phenix.Core.IO;
using Phenix.Core.Mapping;

namespace Phenix.Core.Web
{
  /// <summary>
  /// 工具集
  /// </summary>
  public static class Utilities
  {
    /// <summary>
    /// 解包消息
    /// </summary>
    public static TResult UnpackContent<TResult>(HttpContent content)
      where TResult : class
    {
      return (TResult)UnpackContent(content, typeof(TResult));
    }

    /// <summary>
    /// 解包消息
    /// </summary>
    public static object UnpackContent(HttpContent content, Type resultType)
    {
      return Phenix.Core.Reflection.Utilities.JsonDeserialize(content.ReadAsStringAsync().Result, resultType);
    }

    /// <summary>
    /// 打包响应消息
    /// </summary>
    /// <param name="request">请求消息</param>
    /// <param name="content">响应内容</param>
    public static HttpResponseMessage PackResponse(HttpRequestMessage request, object content)
    {
      Exception error = content as Exception;
      if (error != null)
        return PackErrorResponse(request, error);
      HttpResponseMessage result = request.CreateResponse(HttpStatusCode.OK);
      if (content != null)
        result.Content = content as HttpContent ?? new StringContent(Phenix.Core.Reflection.Utilities.JsonSerialize(content), 
          request.Content.Headers.ContentType != null && !String.IsNullOrEmpty(request.Content.Headers.ContentType.CharSet) ? Encoding.GetEncoding(request.Content.Headers.ContentType.CharSet) : null);
      return result;
    }

    /// <summary>
    /// 打包错误消息
    /// </summary>
    public static HttpResponseMessage PackErrorResponse(HttpRequestMessage request, Exception error)
    {
      if (error is InvalidOperationException) //等效于 HTTP 状态 400
        return AppConfig.Debugging ? request.CreateErrorResponse(HttpStatusCode.BadRequest, error) : request.CreateErrorResponse(HttpStatusCode.BadRequest, error.Message);
      if (error is AuthenticationException) //等效于 HTTP 状态 401
        return AppConfig.Debugging ? request.CreateErrorResponse(HttpStatusCode.Unauthorized, error) : request.CreateErrorResponse(HttpStatusCode.Unauthorized, error.Message);
      if (error is NotSupportedException || error is System.Security.SecurityException) //等效于 HTTP 状态 403
        return AppConfig.Debugging ? request.CreateErrorResponse(HttpStatusCode.Forbidden, error) : request.CreateErrorResponse(HttpStatusCode.Forbidden, error.Message); 
      if (error is NotImplementedException) //等效于 HTTP 状态 501
        return AppConfig.Debugging ? request.CreateErrorResponse(HttpStatusCode.NotImplemented, error) : request.CreateErrorResponse(HttpStatusCode.NotImplemented, error.Message);
      //等效于 HTTP 状态 500
      return request.CreateErrorResponse(HttpStatusCode.InternalServerError, error);
    }

    #region ExecuteService

    /// <summary>
    /// 执行服务
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
    public static HttpResponseMessage ExecuteService<TService>(HttpRequestMessage request)
      where TService : IService
    {
      return ExecuteService(request, typeof(TService));
    }

    /// <summary>
    /// 执行服务
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
    public static HttpResponseMessage ExecuteService(HttpRequestMessage request, Type serviceType)
    {
      if (serviceType == null)
        throw new ArgumentNullException("serviceType");
      if (!typeof(IService).IsAssignableFrom(serviceType))
        throw new InvalidOperationException(serviceType.FullName + "需实现IService接口");
      try
      {
        if (request.Content.IsMimeMultipartContent())
        {
          MultipartMemoryStreamProvider provider = request.Content.ReadAsMultipartAsync().Result;
          Dictionary<string, Stream> fileStreams = null;
          try
          {
            IService service = null;
            FileChunkInfo fileChunkInfo = null;
            byte[] chunkBuffer = null;
            int? chunkNumber = null;
            foreach (HttpContent content in provider.Contents)
            {
              string name = (content.Headers.ContentDisposition.Name ?? String.Empty).Replace("\"", "");
              if (name == "data")
                service = (IService)UnpackContent(content, serviceType);
              else
              {
                string fileName = (content.Headers.ContentDisposition.FileName ?? String.Empty).Replace("\"", "");
                if (name == "file")
                {
                  if (fileStreams == null)
                    fileStreams = new Dictionary<string, Stream>(provider.Contents.Count, StringComparer.OrdinalIgnoreCase);
                  if (fileStreams.ContainsKey(fileName))
                    throw new InvalidOperationException("上传文件中不允许有重复文件名'" + fileName + "'");
                  fileStreams.Add(fileName, content.ReadAsStreamAsync().Result);
                }
                else if (name == "chunkInfo")
                {
                  if (String.IsNullOrEmpty(fileName))
                  {
                    fileChunkInfo = (FileChunkInfo)UnpackContent(content, typeof(FileChunkInfo));
                    if (chunkBuffer != null)
                      fileChunkInfo.SetChunkBuffer(chunkBuffer);
                  }
                  else
                  {
                    chunkBuffer = content.ReadAsByteArrayAsync().Result;
                    if (fileChunkInfo != null)
                      fileChunkInfo.SetChunkBuffer(chunkBuffer);
                  }
                }
                else if (name == "chunkNumber")
                  chunkNumber = Int32.Parse(content.ReadAsStringAsync().Result);
                else
                  throw new InvalidOperationException("Content-Disposition内容标头值的Name仅限于'data'、'file'、'chunkInfo'、'chunkNumber'之一而非'" + name + "'");
              }
            }
            if (service == null)
              service = Activator.CreateInstance(serviceType, true) as IService;
            if (chunkNumber.HasValue)
            {
              if (chunkNumber.Value <= 0)
                return PackResponse(request, new StreamContent(DataHub.DownloadFile(service)));
              else
                return PackResponse(request, DataHub.DownloadBigFile(service, chunkNumber.Value));
            }
            else
            {
              if (fileStreams != null)
                service = DataHub.UploadFiles(service, fileStreams);
              else if (fileChunkInfo != null)
                service = DataHub.UploadBigFile(service, fileChunkInfo);
              else
                throw new InvalidOperationException("Content-Disposition内容须包含'file'、'chunkInfo'、'chunkNumber'之一项");
              return PackResponse(request, service);
            }
          }
          finally
          {
            if (fileStreams != null)
              foreach (KeyValuePair<string, Stream> kvp in fileStreams)
                kvp.Value.Dispose();
          }
        }
        else
        {
          IService service = UnpackContent(request.Content, serviceType) as IService ?? Activator.CreateInstance(serviceType, true) as IService;
          return PackResponse(request, DataHub.Execute(service));
        }
      }
      catch (Exception ex)
      {
        return PackErrorResponse(request, ex);
      }
    }

    #endregion
  }
}
