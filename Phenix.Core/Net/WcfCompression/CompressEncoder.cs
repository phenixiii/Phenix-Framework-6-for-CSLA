using System;
using System.IO;
using Phenix.Core.IO;

namespace Phenix.Core.Net.WcfCompression
{
  /// <summary>
  /// 编码器是用于将消息写入到流中并从流中读取消息的组件
  /// </summary>
  public class CompressEncoder : System.ServiceModel.Channels.MessageEncoder
  {
    internal CompressEncoder(CompressEncoderFactory encoderFactory)
    {
      _encoderFactory = encoderFactory;
    }

    #region 属性

    private readonly CompressEncoderFactory _encoderFactory;
    private System.ServiceModel.Channels.MessageEncoder _innserEncoder;
    private System.ServiceModel.Channels.MessageEncoder InnserEncoder
    {
      get
      {
        if (_innserEncoder == null)
          _innserEncoder = _encoderFactory.InnerMessageEncodingBindingElement.CreateMessageEncoderFactory().Encoder;
        return _innserEncoder;
      }
    }

    /// <summary>
    /// 获取编码器使用的 MIME 内容类型
    /// </summary>
    public override string ContentType
    {
      get { return InnserEncoder.ContentType; }
    }
    /// <summary>
    /// 获取编码器使用的媒体类型值
    /// </summary>
    public override string MediaType
    {
      get { return InnserEncoder.MediaType; }
    }
    /// <summary>
    /// 获取编码器使用的消息版本值
    /// </summary>
    public override System.ServiceModel.Channels.MessageVersion MessageVersion
    {
      get { return InnserEncoder.MessageVersion; }
    }

    #endregion

    #region 方法

    /// <summary>
    /// 消息编码器是否支持指定的消息级内容类型值
    /// </summary>
    public override bool IsContentTypeSupported(string contentType)
    {
      return InnserEncoder.IsContentTypeSupported(contentType);
    }

    /// <summary>
    /// 从通道堆栈的适当层返回所请求的类型化对象（如果存在）
    /// </summary>
    public override T GetProperty<T>()
    {
      return InnserEncoder.GetProperty<T>();
    }

    /// <summary>
    /// 从指定的流中读取一条消息
    /// </summary>
    public override System.ServiceModel.Channels.Message ReadMessage(ArraySegment<byte> buffer, System.ServiceModel.Channels.BufferManager bufferManager, string contentType)
    {
      if (bufferManager == null)
        throw new ArgumentNullException("bufferManager");
      ArraySegment<byte> bytes = CompressHelper.Decompress(buffer);
      byte[] totalBytes = bufferManager.TakeBuffer(bytes.Count);
      Array.Copy(bytes.Array, 0, totalBytes, 0, bytes.Count);
      ArraySegment<byte> byteArray = new ArraySegment<byte>(totalBytes, 0, bytes.Count);
      bufferManager.ReturnBuffer(byteArray.Array);
      return InnserEncoder.ReadMessage(byteArray, bufferManager, contentType);
    }

    /// <summary>
    /// 从指定的流中读取一条消息
    /// </summary>
    public override System.ServiceModel.Channels.Message ReadMessage(Stream inputStream, int maxSizeOfHeaders, string contentType)
    {
      using (Stream stream = CompressHelper.Decompress(inputStream))
      {
        return InnserEncoder.ReadMessage(stream, maxSizeOfHeaders, contentType);
      }
    }

    /// <summary>
    /// 从指定的流中读取一条消息
    /// </summary>
    public override ArraySegment<byte> WriteMessage(System.ServiceModel.Channels.Message message, int maxMessageSize, System.ServiceModel.Channels.BufferManager bufferManager, int messageOffset)
    {
      if (bufferManager == null)
        throw new ArgumentNullException("bufferManager");
      ArraySegment<byte> bytes = InnserEncoder.WriteMessage(message, maxMessageSize, bufferManager);
      ArraySegment<byte> buffer = CompressHelper.Compress(bytes);
      byte[] totalBytes = bufferManager.TakeBuffer(buffer.Count + messageOffset);
      Array.Copy(buffer.Array, 0, totalBytes, messageOffset, buffer.Count);
      return new ArraySegment<byte>(totalBytes, messageOffset, buffer.Count);
    }

    /// <summary>
    /// 将消息写入指定的流中
    /// </summary>
    public override void WriteMessage(System.ServiceModel.Channels.Message message, Stream outputStream)
    {
      using (Stream stream = new MemoryStream())
      {
        InnserEncoder.WriteMessage(message, stream);
        CompressHelper.Compress(stream, outputStream);
      }
    }

    #endregion
  }
}