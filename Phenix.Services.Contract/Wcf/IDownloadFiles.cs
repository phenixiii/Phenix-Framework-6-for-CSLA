using System.Collections.Generic;
using System.ServiceModel;
using Phenix.Core.Security;

namespace Phenix.Services.Contract.Wcf
{
  [ServiceContract]
  public interface IDownloadFiles
  {
    [OperationContract]
    [UseNetDataContract]
    object GetDownloadFileInfos(string applicationName, IList<string> searchPatterns, UserIdentity identity);

    [OperationContract]
    [UseNetDataContract]
    object GetDownloadFile(string directoryName, string sourceDirectoryName, string fileName, int chunkNumber);
  }
}
