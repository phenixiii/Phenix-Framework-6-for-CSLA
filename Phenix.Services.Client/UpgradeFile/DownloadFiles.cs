using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows.Forms;
using System.Xml;
using Phenix.Core;
using Phenix.Core.IO;
using Phenix.Core.Net;
using Phenix.Core.Security;
using Phenix.Services.Contract;

namespace Phenix.Services.Client.UpgradeFile
{
    /// <summary>
    /// 下载文件
    /// </summary>
  public class DownloadFiles
  {
    #region 属性

    /// <summary>
    /// UpgradeProxyType
    /// </summary>
    public ProxyType? UpgradeProxyType { get; set; }

    /// <summary>
    /// UpgradeServicesAddress 
    /// </summary>
    public string UpgradeServicesAddress { get; set; }

    private readonly Collection<string> _searchPatterns = new Collection<string>();
    /// <summary>
    /// SearchPatterns 
    /// </summary>
    public Collection<string> SearchPatterns
    {
      get { return _searchPatterns; }
    }

    /// <summary>
    /// ShutDown
    /// </summary>
    public bool ShutDown { get; set; }

    #endregion

    #region 事件

    /// <summary>
    /// DownloadFileInfo
    /// </summary>
    public event EventHandler<DownloadFileInfoEventArgs> DownloadFileInfo;
    private void OnDownloadFileInfo(DownloadFileInfoEventArgs e)
    {
      if (DownloadFileInfo != null)
        DownloadFileInfo(this, e);
      else
        CheckNeedDownload(e);
    }

    /// <summary>
    /// DownloadFile
    /// </summary>
    public event EventHandler<DownloadFileEventArgs> DownloadFile;
    private void OnDownloadFile(DownloadFileEventArgs e)
    {
      if (DownloadFile != null)
        DownloadFile(this, e);
      else
        FileHelper.WriteChunkInfo(e.FileStream, e.Info);
    }

    #endregion

    #region 方法

    /// <summary>
    /// CheckNeedDownload
    /// </summary>
    public static void CheckNeedDownload(DownloadFileInfoEventArgs e)
    {
      string path = Path.Combine(AppConfig.BaseDirectory, e.DirectoryName ?? String.Empty, e.FileName);
      if (File.Exists(path))
      {
        FileInfo fileInfo = new FileInfo(path);
        e.Applied = fileInfo.Length == e.FileLength && fileInfo.LastWriteTime >= e.FileLastWriteTime;
      }
      else
        e.Applied = false;
    }

    private static void ClearBackupFile()
    {
      string directory = Path.Combine(AppConfig.BaseDirectory, AppConfig.BACKUP_SUBDIRECTORY);
      if (Directory.Exists(directory))
        Directory.Delete(directory, true);
    }

    private static void RecoverFile()
    {
      string directory = Path.Combine(AppConfig.BaseDirectory, AppConfig.BACKUP_SUBDIRECTORY);
      if (Directory.Exists(directory))
        foreach (string s in Directory.GetFiles(directory))
          File.Copy(s, Path.Combine(AppConfig.BaseDirectory, Path.GetFileName(s)), true);
    }

    private static string GetPath(string subdirectory, string fileName)
    {
      string directory = Path.Combine(AppConfig.BaseDirectory, subdirectory ?? String.Empty);
      if (!Directory.Exists(directory))
        Directory.CreateDirectory(directory);
      return Path.Combine(directory, fileName);
    }

    private static string GetBackupPath(string subdirectory, string fileName)
    {
      string backupDirectory = Path.Combine(AppConfig.BaseDirectory, AppConfig.BACKUP_SUBDIRECTORY, subdirectory ?? String.Empty);
      if (!Directory.Exists(backupDirectory))
        Directory.CreateDirectory(backupDirectory);
      return Path.Combine(backupDirectory, fileName);
    }

    private IDownloadFiles GetDownloadFilesProxy()
    {
      if (UpgradeProxyType.HasValue && UpgradeProxyType == ProxyType.Remoting || (!UpgradeProxyType.HasValue || UpgradeServicesAddress != null) && Phenix.Core.Net.NetConfig.ProxyType == ProxyType.Remoting)
        return new Phenix.Services.Client.Library.DownloadFilesProxy(UpgradeServicesAddress);
      if (UpgradeProxyType.HasValue && UpgradeProxyType == ProxyType.Wcf || (!UpgradeProxyType.HasValue || UpgradeServicesAddress != null) && Phenix.Core.Net.NetConfig.ProxyType == ProxyType.Wcf)
        return new Phenix.Services.Client.Library.Wcf.DownloadFilesProxy(UpgradeServicesAddress);
      if (UpgradeServicesAddress != null)
        return new Phenix.Services.Client.Library.DownloadFilesProxy(UpgradeServicesAddress);
      return null;
    }

    /// <summary>
    /// Execute
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2202:DoNotDisposeObjectsMultipleTimes")]
    public bool Execute()
    {
      IDownloadFiles proxy = GetDownloadFilesProxy();
      if (proxy == null)
        return true;
      try
      {
        ClearBackupFile();
        string fileInfos = proxy.GetDownloadFileInfos(AppDomain.CurrentDomain.FriendlyName, SearchPatterns, UserIdentity.CurrentIdentity);
        using (StringReader stringReader = new StringReader(fileInfos))
        using (XmlReader xmlReader = XmlReader.Create(stringReader, new XmlReaderSettings { ConformanceLevel = ConformanceLevel.Fragment }))
        {
          while (xmlReader.Read())
          {
            Application.DoEvents();
            if (ShutDown)
              return false;
            if (xmlReader.NodeType == XmlNodeType.Element)
            {
              string directoryName = xmlReader.GetAttribute(0);
              string sourceDirectoryName = xmlReader.GetAttribute(1);
              string fileName = xmlReader.GetAttribute(2);
              int fileLength = Int32.Parse(xmlReader.GetAttribute(3));
              DateTime fileLastWriteTime = DateTime.FromBinary(Int64.Parse(xmlReader.GetAttribute(4)));
              DownloadFileInfoEventArgs downloadFileInfoEventArgs = new DownloadFileInfoEventArgs(directoryName, fileName, fileLength, fileLastWriteTime);
              OnDownloadFileInfo(downloadFileInfoEventArgs);
              if (downloadFileInfoEventArgs.Applied)
                continue;

              string path = GetPath(directoryName, fileName);
              if (File.Exists(path))
              {
                string backupPath = GetBackupPath(directoryName, fileName);
                if (File.Exists(backupPath))
                  File.Delete(backupPath);
                File.Move(path, backupPath);
              }
              try
              {
                using (FileStream fileStream = new FileStream(path, FileMode.OpenOrCreate, FileAccess.Write))
                {
                  for (int i = 1; i <= FileHelper.GetChunkCount(fileLength); i++)
                  {
                    DownloadFileInfo info = proxy.GetDownloadFile(directoryName, sourceDirectoryName, fileName, i);
                    if (info != null)
                    {
                      DownloadFileEventArgs downloadFileEventArgs = new DownloadFileEventArgs(fileStream, info);
                      OnDownloadFile(downloadFileEventArgs);
                      if (downloadFileEventArgs.Stop)
                        return false;
                    }
                  }
                }
              }
              catch
              {
                if (File.Exists(path))
                  File.Delete(path);
                throw;
              }
            }
          }
        }
        return true;
      }
      catch
      {
        RecoverFile();
        throw;
      }
    }

    #endregion
  }
}