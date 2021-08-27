using System;
using Phenix.Core;

namespace Phenix.Services.Client.UpgradeFile
{
  /// <summary>
  /// DownloadFileInfoEventArgs
  /// </summary>
  public class DownloadFileInfoEventArgs : ShallEventArgs
  {
    /// <summary>
    /// DownloadFileInfoEventArgs
    /// </summary>
    public DownloadFileInfoEventArgs(string directoryName, string fileName, int fileLength, DateTime fileLastWriteTime)
      : base()
    {
      _directoryName = directoryName;
      _fileName = fileName;
      _fileLength = fileLength;
      _fileLastWriteTime = fileLastWriteTime;
    }

    #region  Ù–‘

    private readonly string _directoryName;
    /// <summary>
    /// DirectoryName
    /// </summary>
    public string DirectoryName
    {
      get { return _directoryName; }
    }

    private readonly string _fileName;
    /// <summary>
    /// FileName
    /// </summary>
    public string FileName
    {
      get { return _fileName; }
    }

    private readonly long _fileLength;
    /// <summary>
    /// FileLength
    /// </summary>
    public long FileLength
    {
      get { return _fileLength; }
    }

    private readonly DateTime _fileLastWriteTime;
    /// <summary>
    /// FileLastWriteTime
    /// </summary>
    public DateTime FileLastWriteTime
    {
      get { return _fileLastWriteTime; }
    }

    #endregion
  }
}
