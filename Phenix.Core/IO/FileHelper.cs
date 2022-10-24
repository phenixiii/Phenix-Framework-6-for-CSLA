using System;
using System.IO;

namespace Phenix.Core.IO
{
  /// <summary>
  /// File助手
  /// </summary>
  public static class FileHelper
  {
    private const int CHUNK_MAX_SIZE = 1024 * 1024;

    /// <summary>
    /// 取文件块数
    /// </summary>
    /// <param name="fileSize">文件大小</param>
    public static int GetChunkCount(long fileSize)
    {
      return (int)Math.Ceiling(fileSize * 1.0 / CHUNK_MAX_SIZE);
    }

    /// <summary>
    /// 读取文件块
    /// </summary>
    /// <param name="path">文件名</param>
    /// <param name="chunkNumber">块号</param>
    public static FileChunkInfo ReadChunkInfo(string path, int chunkNumber)
    {
      if (String.IsNullOrEmpty(path))
        throw new ArgumentNullException("path");
      using (FileStream fileStream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read))
      {
        return ReadChunkInfo(path, fileStream, chunkNumber);
      }
    }

    /// <summary>
    /// 读取文件块
    /// </summary>
    /// <param name="fileName">文件名</param>
    /// <param name="fileStream">文件流</param>
    /// <param name="chunkNumber">块号</param>
    public static FileChunkInfo ReadChunkInfo(string fileName, Stream fileStream, int chunkNumber)
    {
      if (fileStream == null)
        throw new ArgumentNullException("fileStream");
      int chunkCount = GetChunkCount(fileStream.Length);
      if (chunkNumber > chunkCount || chunkNumber <= 0)
        throw new ArgumentOutOfRangeException("chunkNumber", chunkNumber, @"chunkNumber <= chunkCount && chunkNumber > 0");
      int chunkSize = chunkNumber < chunkCount ? CHUNK_MAX_SIZE : (int)(fileStream.Length - CHUNK_MAX_SIZE * (chunkCount - 1));
      byte[] chunkBuffer = new byte[chunkSize];
      fileStream.Seek(CHUNK_MAX_SIZE * (chunkNumber - 1), SeekOrigin.Begin);
      fileStream.Read(chunkBuffer, 0, chunkSize);
      return new FileChunkInfo(fileName, chunkCount, chunkNumber, chunkSize, chunkBuffer);
    }

    /// <summary>
    /// 写入文件块
    /// </summary>
    /// <param name="path">文件名</param>
    /// <param name="chunkInfo">文件块信息</param>
    public static void WriteChunkInfo(string path, FileChunkInfo chunkInfo)
    {
      if (String.IsNullOrEmpty(path))
        throw new ArgumentNullException("path");
      using (FileStream fileStream = new FileStream(path, FileMode.OpenOrCreate, FileAccess.Write))
      {
        WriteChunkInfo(fileStream, chunkInfo);
      }
    }

    /// <summary>
    /// 写入文件块
    /// </summary>
    /// <param name="fileStream">文件流</param>
    /// <param name="chunkInfo">文件块信息</param>
    public static void WriteChunkInfo(Stream fileStream, FileChunkInfo chunkInfo)
    {
      if (fileStream == null)
        throw new ArgumentNullException("fileStream");
      if (chunkInfo == null)
        throw new ArgumentNullException("chunkInfo");
      if (chunkInfo.ChunkNumber > chunkInfo.ChunkCount || chunkInfo.ChunkNumber <= 0 || chunkInfo.ChunkCount <= 0 || chunkInfo.ChunkSize <= 0)
        throw new ArgumentOutOfRangeException("chunkInfo", chunkInfo, @"chunkInfo.ChunkNumber <= chunkInfo.ChunkCount && chunkInfo.ChunkNumber > 0 && chunkInfo.ChunkCount > 0 && chunkInfo.ChunkSize > 0");
      byte[] chunkBuffer = chunkInfo.GetChunkBuffer();
      if (chunkBuffer == null)
        throw new InvalidOperationException("不允许chunkInfo参数ChunkBuffer值为空");
      fileStream.Seek(CHUNK_MAX_SIZE * (chunkInfo.ChunkNumber - 1), SeekOrigin.Begin);
      fileStream.Write(chunkBuffer, 0, chunkInfo.ChunkSize);
      fileStream.Flush();
    }
  }
}
