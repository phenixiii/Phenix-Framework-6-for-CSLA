using System;
using System.IO;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Messaging;
using Phenix.Core.IO;

namespace Phenix.Core.Net.RemotingCompression
{
  internal class CompressServerChannelSink : BaseChannelSinkWithProperties, IServerChannelSink
  {
    /// <summary>
    /// Constructor with properties.
    /// </summary>
    /// <param name="nextSink">Next sink.</param>
    /// <param name="compressionThreshold">Compression threshold. If 0, compression is disabled globally.</param>
    public CompressServerChannelSink(IServerChannelSink nextSink, int compressionThreshold)
    {
      // Set the next sink.
      _next = nextSink;
      // Set the compression threshold.
      _compressionThreshold = compressionThreshold;
    }

    #region  Ù–‘

    private readonly IServerChannelSink _next;
    /// <summary>
    /// The next sink in the sink chain.
    /// </summary>
    public IServerChannelSink NextChannelSink
    {
      get { return _next; }
    }

    // The compression threshold.
    private readonly int _compressionThreshold;

    #endregion

    #region ∑Ω∑®

    public void AsyncProcessResponse(IServerResponseChannelSinkStack sinkStack, object state, IMessage msg, ITransportHeaders headers, Stream stream)
    {
      if (sinkStack == null)
        throw new ArgumentNullException("sinkStack");

      // Send the response to the client.
      sinkStack.AsyncProcessResponse(msg, headers, stream);
    }

    public Stream GetResponseStream(IServerResponseChannelSinkStack sinkStack, object state, IMessage msg, ITransportHeaders headers)
    {
      // Always return null
      return null;
    }

    /// <summary>
    /// Returns true if the message contains the compression exempt parameters, marked as
    /// NonCompressible
    /// </summary>
    /// <param name="msg"></param>
    /// <returns></returns>
    public static bool IsCompressionExempt(IMessage msg)
    {
      if (msg != null && msg.Properties.Contains("__Return"))
      {
        object obj = msg.Properties["__Return"];
        if (obj.GetType().IsDefined(typeof(NonCompressibleAttribute), false))
          return true;
        ICompressible compressible = obj as ICompressible;
        if (compressible != null && !compressible.PerformCompression())
          return true;
      }
      return false;
    }

    public ServerProcessing ProcessMessage(IServerChannelSinkStack sinkStack, IMessage requestMsg, ITransportHeaders requestHeaders, Stream requestStream,
        out IMessage responseMsg, out ITransportHeaders responseHeaders, out Stream responseStream)
    {
      if (sinkStack == null)
        throw new ArgumentNullException("sinkStack");
      if (requestHeaders == null)
        throw new ArgumentNullException("requestHeaders");

      // Push this onto the sink stack
      sinkStack.Push(this, null);

      // If the request has the compression flag, decompress the stream.
      if (requestHeaders[CompressCommonHeaders.COMPERSSION_ENABLED] != null)
      {
        // Process the message and decompress it.
        requestStream = CompressHelper.Decompress(requestStream);
      }

      // Retrieve the response from the server.
      ServerProcessing processingResult =
        _next.ProcessMessage(sinkStack, requestMsg, requestHeaders, requestStream,
        out responseMsg, out responseHeaders, out responseStream);

      // If the response stream length is greater than the threshold,
      // message is not exempt from compression, and client supports compression,
      // compress the stream.
      if (responseStream != null &&
        processingResult == ServerProcessing.Complete &&
        _compressionThreshold > 0 && responseStream.Length > _compressionThreshold &&
        !IsCompressionExempt(responseMsg) &&
        requestHeaders[CompressCommonHeaders.COMPERSSION_SUPPORTED] != null)
      {
        // Process the message and compress it.
        responseStream = CompressHelper.Compress(responseStream);

        // Send the compression flag to the client.
        if (responseHeaders != null)
          responseHeaders[CompressCommonHeaders.COMPERSSION_ENABLED] = true;
      }

      // Take off the stack and return the result.
      sinkStack.Pop(this);
      return processingResult;
    }

    #endregion
  }
}