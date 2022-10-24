using System;
using System.IO;
using System.IO.Compression;
using System.Linq;

namespace Phenix.Core.IO
{
  /// <summary>
  /// 压缩助手
  /// </summary>
  public static class CompressHelper
  {
    /// <summary>
    /// 压缩
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2202:DoNotDisposeObjectsMultipleTimes")]
    public static ArraySegment<byte> Compress(ArraySegment<byte> data)
    {
      using (MemoryStream outputStream = new MemoryStream())
      {
        using (DeflateStream compressStream = new DeflateStream(outputStream, CompressionMode.Compress, true))
        {
          compressStream.Write(data.Array, 0, data.Count);
        }
        return new ArraySegment<byte>(outputStream.ToArray());
      }
    }

    /// <summary>
    /// 压缩
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Reliability", "CA2000:DisposeObjectsBeforeLosingScope")]
    public static Stream Compress(Stream inputStream)
    {
      Stream result = new MemoryStream();
      Compress(inputStream, result);
      return result;
    }

    /// <summary>
    /// 压缩
    /// </summary>
    public static void Compress(Stream inputStream, Stream outputStream)
    {
      using (DeflateStream compressStream = new DeflateStream(outputStream, CompressionMode.Compress, true))
      {
        StreamHelper.CopyBuffer(inputStream, compressStream);
      }
      outputStream.Seek(0, SeekOrigin.Begin);
    }

    /// <summary>
    /// 解压
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2202:DoNotDisposeObjectsMultipleTimes")]
    public static ArraySegment<byte> Decompress(ArraySegment<byte> data)
    {
      using (MemoryStream inputStream = new MemoryStream(data.Array))
      using (DeflateStream decompressStream = new DeflateStream(inputStream, CompressionMode.Decompress, true))
      {
        return new ArraySegment<byte>(StreamHelper.CopyBuffer(decompressStream).ToArray());
      }
    }

    /// <summary>
    /// 解压
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Reliability", "CA2000:DisposeObjectsBeforeLosingScope")]
    public static Stream Decompress(Stream inputStream)
    {
      Stream result = new MemoryStream();
      Decompress(inputStream, result);
      return result;
    }

    /// <summary>
    /// 解压
    /// </summary>
    public static void Decompress(Stream inputStream, Stream outputStream)
    {
      using (DeflateStream decompressStream = new DeflateStream(inputStream, CompressionMode.Decompress, true))
      {
        StreamHelper.CopyBuffer(decompressStream, outputStream);
      }
      outputStream.Seek(0, SeekOrigin.Begin);
    }
  }
}