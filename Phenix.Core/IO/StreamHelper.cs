using System;
using System.Collections.Generic;
using System.IO;

namespace Phenix.Core.IO
{
  /// <summary>
  /// Stream助手
  /// </summary>
  public static class StreamHelper
  {
    private const int BUFFER_SIZE = 4096;

    /// <summary>
    /// 拷贝数据
    /// </summary>
    /// <param name="sourceStream">数据源</param>
    /// <param name="targetStream">目的流</param>
    public static void CopyBuffer(Stream sourceStream, Stream targetStream)
    {
      if (sourceStream == null || targetStream == null)
        return;
      int i;
      byte[] buffer = new byte[BUFFER_SIZE];
      while ((i = sourceStream.Read(buffer, 0, BUFFER_SIZE)) > 0)
      {
        targetStream.Write(buffer, 0, i);
        targetStream.Flush();
      }
    }

    /// <summary>
    /// 拷贝数据
    /// </summary>
    /// <param name="sourceStream">数据源</param>
    /// <returns>目的字节串</returns>
    public static IList<byte> CopyBuffer(Stream sourceStream)
    {
      if (sourceStream == null)
        return null;
      List<byte> result = new List<byte>();
      int i;
      byte[] buffer = new byte[BUFFER_SIZE];
      while ((i = sourceStream.Read(buffer, 0, BUFFER_SIZE)) > 0)
      {
        byte[] buffers = new byte[i];
        Array.Copy(buffer, buffers, i);
        result.AddRange(buffers);
      }
      return result;
    }
  }
}
