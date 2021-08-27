using System.Collections.Generic;
using System.Net.Sockets;
using Phenix.Core.IO;
using Phenix.Core.Net;
using Phenix.Core.Security;
using Phenix.Services.Contract;

namespace Phenix.Services.Client.Library
{
  internal class DownloadFilesProxy : IDownloadFiles
  {
    public DownloadFilesProxy(string servicesAddress)
    {
      _servicesAddress = servicesAddress;
    }

    #region 属性

    private readonly string _servicesAddress;
    
    private IDownloadFiles _service;
    private IDownloadFiles Service
    {
      get
      {
        if (_service == null)
        {
          RemotingHelper.RegisterClientChannel();
          _service = (IDownloadFiles)RemotingHelper.CreateRemoteObjectProxy(typeof(IDownloadFiles), _servicesAddress ?? NetConfig.ServicesAddress, ServicesInfo.DOWNLOAD_FILES_URI);
        }
        return _service;
      }
    }

    #endregion

    #region 方法

    private void InvalidateCache()
    {
      _service = null;
    }

    #region IDownloadFiles 成员

    public string GetDownloadFileInfos(string applicationName, IList<string> searchPatterns, UserIdentity identity)
    {
      NetConfig.InitializeSwitch();
      do
      {
        try
        {
          return Service.GetDownloadFileInfos(applicationName, searchPatterns, identity);
        }
        catch (SocketException)
        {
          InvalidateCache();
          if (!NetConfig.SwitchServicesAddress())
            throw;
        }
      } while (true);
    }

    public DownloadFileInfo GetDownloadFile(string directoryName, string sourceDirectoryName, string fileName, int chunkNumber)
    {
      NetConfig.InitializeSwitch();
      do
      {
        try
        {
          return Service.GetDownloadFile(directoryName, sourceDirectoryName, fileName, chunkNumber);
        }
        catch (SocketException)
        {
          InvalidateCache();
          if (!NetConfig.SwitchServicesAddress())
            throw;
        }
      } while (true);
    }

    #endregion

    #endregion
  }
}