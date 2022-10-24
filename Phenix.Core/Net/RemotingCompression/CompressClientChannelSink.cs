using System;
using System.IO;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Messaging;
using Phenix.Core.IO;

namespace Phenix.Core.Net.RemotingCompression
{
  internal class CompressClientChannelSink : BaseChannelSinkWithProperties, IClientChannelSink
  {
    /// <summary>
    /// Constructor with properties.
    /// </summary>
    /// <param name="nextSink">Next sink.</param>
    /// <param name="compressionThreshold">Compression threshold. If 0, compression is disabled globally.</param>
    public CompressClientChannelSink(IClientChannelSink nextSink, int compressionThreshold)
    {
      // Set the next sink.
      _next = nextSink;
      // Set the compression threshold.
      _compressionThreshold = compressionThreshold;
    }

    #region  Ù–‘

    private readonly IClientChannelSink _next;
    /// <summary>
    /// The next sink in the sink chain.
    /// </summary>
    public IClientChannelSink NextChannelSink
    {
      get { return _next; }
    }

    // The compression threshold.
    private readonly int _compressionThreshold;

    #endregion

    #region ∑Ω∑®

    public void AsyncProcessRequest(IClientChannelSinkStack sinkStack, IMessage msg, ITransportHeaders headers, Stream stream)
    {
      if (sinkStack == null)
        throw new ArgumentNullException("sinkStack");

      // Push this onto the sink stack.
      sinkStack.Push(this, null);
      // Send the request to the client.
      _next.AsyncProcessRequest(sinkStack, msg, headers, stream);
    }

    public void AsyncProcessResponse(IClientResponseChannelSinkStack sinkStack, object state, ITransportHeaders headers, Stream stream)
    {
      if (sinkStack == null)
        throw new ArgumentNullException("sinkStack");

      // Send the request to the server.
      sinkStack.AsyncProcessResponse(headers, stream);
    }

    public Stream GetRequestStream(IMessage msg, ITransportHeaders headers)
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
      if (msg != null && msg.Properties.Contains("__Args"))
        foreach (object obj in (object[])msg.Properties["__Args"])
        {
          if (obj == null)
            continue;
          Type type = obj.GetType();
          if (type.IsDefined(typeof(NonCompressibleAttribute), false))
            return true;
          ICompressible compressible = obj as ICompressible;
          if (compressible != null && !compressible.PerformCompression())
            return true;
        }
      return false;
    }

    public void ProcessMessage(IMessage msg, ITransportHeaders requestHeaders, Stream requestStream,
      out ITransportHeaders responseHeaders, out Stream responseStream)
    {
      if (requestHeaders == null)
        throw new ArgumentNullException("requestHeaders");

      // If the request stream length is greater than the threshold
      // and message is not exempt from compression, compress the stream.
      if (requestStream != null &&
        _compressionThreshold > 0 && requestStream.Length > _compressionThreshold &&
        !IsCompressionExempt(msg))
      {
        // Process the message and compress it.
        requestStream = CompressHelper.Compress(requestStream);

        // Send the compression flag to the server.
        requestHeaders[CompressCommonHeaders.COMPERSSION_ENABLED] = true;
      }

      // Send the compression supported flag to the server.
      requestHeaders[CompressCommonHeaders.COMPERSSION_SUPPORTED] = true;

      // Send the request to the server.
      _next.ProcessMessage(
        msg, requestHeaders, requestStream,
        out responseHeaders, out responseStream);

      // If the response has the compression flag, decompress the stream.
      if (responseHeaders != null && responseHeaders[CompressCommonHeaders.COMPERSSION_ENABLED] != null)
      {
        // Process the message and decompress it.
        responseStream = CompressHelper.Decompress(responseStream);
      }
    }

    #endregion
  }
}