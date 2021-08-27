using System.IO;
using Phenix.Core;
using Phenix.Core.IO;

namespace Phenix.Services.Client.UpgradeFile
{
  /// <summary>
  /// DownloadFileEventArgs
  /// </summary>
  public class DownloadFileEventArgs : ShallEventArgs
  {
    /// <summary>
    /// DownloadFileEventArgs
    /// </summary>
    public DownloadFileEventArgs(FileStream fileStream, DownloadFileInfo info)
      : base()
    {
      _fileStream = fileStream;
      _info = info;
    }

    #region  Ù–‘

    private readonly FileStream _fileStream;
    /// <summary>
    /// FileStream
    /// </summary>
    public FileStream FileStream
    {
      get { return _fileStream; }
    }

    private readonly DownloadFileInfo _info;
    /// <summary>
    /// Info
    /// </summary>
    public DownloadFileInfo Info
    {
      get { return _info; }
    }

    #endregion
  }
}
