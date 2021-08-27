using System.Collections.Generic;
using Phenix.Core.IO;
using Phenix.Core.Security;

namespace Phenix.Services.Contract
{
  public interface IDownloadFiles
  {
    #region ·½·¨

    string GetDownloadFileInfos(string applicationName, IList<string> searchPatterns, UserIdentity identity);

    DownloadFileInfo GetDownloadFile(string directoryName, string sourceDirectoryName, string fileName, int chunkNumber);

    #endregion
  }
}
