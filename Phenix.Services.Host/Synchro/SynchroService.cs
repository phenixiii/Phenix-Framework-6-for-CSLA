using System;
using System.IO;
using System.Runtime.Remoting;
using Phenix.Core;
using Phenix.Core.IO;

namespace Phenix.Services.Host.Synchro
{
  public sealed class SynchroService : MarshalByRefObject, ISynchro
  {
    internal const string URI = "SynchroService";

    #region 方法

    internal static void RegisterService()
    {
      RemotingConfiguration.RegisterWellKnownServiceType(typeof(SynchroService), URI, WellKnownObjectMode.SingleCall);
    }

    public void ClearServiceLibrarySubdirectory()
    {
      Directory.Delete(AppConfig.ServiceLibrarySubdirectory, true);
      Directory.CreateDirectory(AppConfig.ServiceLibrarySubdirectory);
    }
    
    public void Upload(string subdirectoryName, string fileName, int fileLength, byte[] fileBytes)
    {
      string fullSubdirectory = !String.IsNullOrEmpty(subdirectoryName)
        ? Path.Combine(AppConfig.ServiceLibrarySubdirectory, subdirectoryName)
        : AppConfig.ServiceLibrarySubdirectory;
      if (!Directory.Exists(fullSubdirectory))
        Directory.CreateDirectory(fullSubdirectory);
      using (MemoryStream inputStream = new MemoryStream(fileBytes))
      using (FileStream outputStream = new FileStream(Path.Combine(fullSubdirectory, fileName), FileMode.Create, FileAccess.Write))
      {
        CompressHelper.Decompress(inputStream, outputStream);
      }
    }

    #endregion
  }
}
