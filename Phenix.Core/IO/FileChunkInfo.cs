using System;
using System.IO;

namespace Phenix.Core.IO
{
  /// <summary>
  /// 文件块信息
  /// </summary>
  [Serializable]
  public class FileChunkInfo
  {
    /// <summary>
    /// 下载文件信息
    /// </summary>
    public FileChunkInfo(string fileName)
    {
      _fileName = Path.GetFileName(fileName);
    }

    /// <summary>
    /// 下载文件信息
    /// </summary>
    private FileChunkInfo(string fileName, int chunkCount, int chunkNumber, int chunkSize)
      : this(fileName)
    {
      _chunkCount = chunkCount;
      _chunkNumber = chunkNumber;
      _chunkSize = chunkSize;
    }

    /// <summary>
    /// 下载文件信息
    /// </summary>
    public FileChunkInfo(string fileName, int chunkCount, int chunkNumber, int chunkSize, byte[] chunkBuffer)
      : this(fileName, chunkCount, chunkNumber, chunkSize)
    {
      _chunkBuffer = chunkBuffer;
    }

    /// <summary>
    /// 下载文件信息
    /// </summary>
    public FileChunkInfo(FileChunkInfo fileChunkInfo)
      : this(fileChunkInfo._fileName, fileChunkInfo._chunkCount, fileChunkInfo._chunkNumber, fileChunkInfo._chunkSize, fileChunkInfo._chunkBuffer) { }

    /// <summary>
    /// 下载文件信息
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
    [Newtonsoft.Json.JsonConstructor]
    private FileChunkInfo(string fileName, int chunkCount, int chunkNumber, int chunkSize, string chunkBuffer)
      : this(fileName, chunkCount, chunkNumber, chunkSize, chunkBuffer != null ? Convert.FromBase64String(chunkBuffer) : null) { }

    #region 属性

    private readonly string _fileName;
    /// <summary>
    /// 文件名
    /// </summary>
    public string FileName
    {
      get { return _fileName; }
    }
    
    private readonly int _chunkCount;
    /// <summary>
    /// 块数
    /// </summary>
    public int ChunkCount
    {
      get { return _chunkCount; }
    }

    private readonly int _chunkNumber;
    /// <summary>
    /// 块号
    /// </summary>
    public int ChunkNumber
    {
      get { return _chunkNumber; }
    }

    private readonly int _chunkSize;
    /// <summary>
    /// 块大小
    /// </summary>
    public int ChunkSize
    {
      get { return _chunkSize; }
    }

    private byte[] _chunkBuffer;
    /// <summary>
    /// 块缓存
    /// </summary>
    public string ChunkBuffer
    {
      get { return _chunkBuffer != null ? Convert.ToBase64String(_chunkBuffer) : null; }
    }

    /// <summary>
    /// 是否终止
    /// </summary>
    [Newtonsoft.Json.JsonIgnore]
    public bool Stop
    {
      get { return _chunkCount <= 0 || _chunkNumber <= 0 || _chunkSize <= 0 || _chunkBuffer == null; }
    }
    
    /// <summary>
    /// 是否结束
    /// </summary>
    [Newtonsoft.Json.JsonIgnore]
    public bool Over
    {
      get { return _chunkCount <= _chunkNumber; }
    }

    #endregion

    #region 方法

    /// <summary>
    /// 获取块缓存
    /// </summary>
    public byte[] GetChunkBuffer()
    {
      return _chunkBuffer;
    }

    /// <summary>
    /// 设值块缓存
    /// </summary>
    public void SetChunkBuffer(byte[] value)
    {
      _chunkBuffer = value;
    }

    #endregion
  }
}
