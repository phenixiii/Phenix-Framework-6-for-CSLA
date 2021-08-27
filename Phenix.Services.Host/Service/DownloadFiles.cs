using System;
using System.Collections.Generic;
using Phenix.Core.IO;
using Phenix.Core.Security;
using Phenix.Services.Host.Core;

namespace Phenix.Services.Host.Service
{
  public sealed class DownloadFiles : MarshalByRefObject, Phenix.Services.Contract.IDownloadFiles
  {
    #region ·½·¨

    public string GetDownloadFileInfos(string applicationName, IList<string> searchPatterns, UserIdentity identity)
    {
      ServiceManager.CheckIn(identity);
      DataSecurityContext context = DataSecurityHub.CheckIn(identity, false);
      return DownloadFileHelper.GetDownloadFileInfos(applicationName, searchPatterns, context.Identity);
    }

    public DownloadFileInfo GetDownloadFile(string directoryName, string sourceDirectoryName, string fileName, int chunkNumber)
    {
      ServiceManager.CheckActive();
      return DownloadFileHelper.GetDownloadFile(directoryName, sourceDirectoryName, fileName, chunkNumber);
    }

    #endregion
  }
}