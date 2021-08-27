using System;
using System.Collections.Generic;
using System.ServiceModel;
using Phenix.Core.IO;
using Phenix.Core.Net;
using Phenix.Core.Security;
using Phenix.Services.Contract;

namespace Phenix.Services.Client.Library.Wcf
{
  internal class DownloadFilesProxy : IDownloadFiles
  {
    public DownloadFilesProxy(string servicesAddress)
    {
      _servicesAddress = servicesAddress;
    }

    #region 属性

    private readonly string _servicesAddress;
    
    #endregion

    #region 方法

    private ChannelFactory<Phenix.Services.Contract.Wcf.IDownloadFiles> GetChannelFactory()
    {
      return new ChannelFactory<Phenix.Services.Contract.Wcf.IDownloadFiles>(WcfHelper.CreateBinding(),
        new EndpointAddress(WcfHelper.CreateUrl(_servicesAddress ?? NetConfig.ServicesAddress, ServicesInfo.DOWNLOAD_FILES_URI)));
    }

    #region IDownloadFiles 成员

    public string GetDownloadFileInfos(string applicationName, IList<string> searchPatterns, UserIdentity identity)
    {
      NetConfig.InitializeSwitch();
      do
      {
        try
        {
          ChannelFactory<Phenix.Services.Contract.Wcf.IDownloadFiles> channelFactory = GetChannelFactory();
          Phenix.Services.Contract.Wcf.IDownloadFiles channel = channelFactory.CreateChannel();
          object result = null;
          try
          {
            result = channel.GetDownloadFileInfos(applicationName, searchPatterns, identity);
            channelFactory.Close();
          }
          catch
          {
            channelFactory.Abort();
            throw;
          }
          Exception exception = result as Exception;
          if (exception != null)
            throw exception;
          return (string)result;
        }
        catch (EndpointNotFoundException)
        {
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
          ChannelFactory<Phenix.Services.Contract.Wcf.IDownloadFiles> channelFactory = GetChannelFactory();
          Phenix.Services.Contract.Wcf.IDownloadFiles channel = channelFactory.CreateChannel();
          object result = null;
          try
          {
            result = channel.GetDownloadFile(directoryName, sourceDirectoryName, fileName, chunkNumber);
            channelFactory.Close();
          }
          catch
          {
            channelFactory.Abort();
            throw;
          }
          Exception exception = result as Exception;
          if (exception != null)
            throw exception;
          return (DownloadFileInfo)result;
        }
        catch (EndpointNotFoundException)
        {
          if (!NetConfig.SwitchServicesAddress())
            throw;
        }
      } while (true);
    }

    #endregion

    #endregion
  }
}