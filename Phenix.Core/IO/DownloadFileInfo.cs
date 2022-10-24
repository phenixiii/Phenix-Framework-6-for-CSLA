using System;

namespace Phenix.Core.IO
{
  /// <summary>
  /// 下载文件信息
  /// </summary>
  [Serializable]
  public sealed class DownloadFileInfo : FileChunkInfo
  {
    /// <summary>
    /// 下载文件信息
    /// </summary>
    public DownloadFileInfo(string directoryName, FileChunkInfo fileChunkInfo)
      : base(fileChunkInfo)
    {
      _directoryName = directoryName;
    }

    #region 属性

    private readonly string _directoryName;
    /// <summary>
    /// 子目录名
    /// </summary>
    public string DirectoryName
    {
      get { return _directoryName; }
    }

    #endregion
  }
}
