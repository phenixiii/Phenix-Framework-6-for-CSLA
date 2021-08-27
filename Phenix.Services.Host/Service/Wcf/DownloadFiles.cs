using System;
using System.Collections.Generic;
using System.ServiceModel;
using System.ServiceModel.Activation;
using Phenix.Core.IO;
using Phenix.Core.Security;
using Phenix.Services.Host.Core;

namespace Phenix.Services.Host.Service.Wcf
{
  [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerCall)]
  [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
  public sealed class DownloadFiles : Phenix.Services.Contract.Wcf.IDownloadFiles
  {
    [OperationBehavior(Impersonation = ImpersonationOption.Allowed)]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
    public object GetDownloadFileInfos(string applicationName, IList<string> searchPatterns, UserIdentity identity)
    {
      try
      {
        ServiceManager.CheckIn(identity);
        DataSecurityContext context = DataSecurityHub.CheckIn(identity, false);
        return DownloadFileHelper.GetDownloadFileInfos(applicationName, searchPatterns, context.Identity);
      }
      catch (Exception ex)
      {
        return ex;
      }
    }

    [OperationBehavior(Impersonation = ImpersonationOption.Allowed)]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
    public object GetDownloadFile(string directoryName, string sourceDirectoryName, string fileName, int chunkNumber)
    {
      try
      {
        ServiceManager.CheckActive();
        return DownloadFileHelper.GetDownloadFile(directoryName, sourceDirectoryName, fileName, chunkNumber);
      }
      catch (Exception ex)
      {
        return ex;
      }
    }
  }
}