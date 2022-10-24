using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Text;
using System.Xml;
using Phenix.Core.Security;

namespace Phenix.Core.IO
{
  /// <summary>
  /// �����ļ�����
  /// </summary>
  public static class DownloadFileHelper
  {
    #region ����

    private static void WriteOwnDownloadFileInfos(XmlWriter xmlWriter,
      DirectoryInfo directoryInfo, string directoryName, string sourceDirectoryName, IList<string> searchPatterns, UserIdentity identity, 
      ref Dictionary<string, FileInfo> ignoreFileInfos)
    {
      foreach (DirectoryInfo item in directoryInfo.GetDirectories())
        if (String.Compare(item.Name, identity.Department.RootSuperior.Code, StringComparison.OrdinalIgnoreCase) == 0)
        {
          WriteDownloadFileInfos(xmlWriter, item, directoryName, Path.Combine(sourceDirectoryName, item.Name), searchPatterns, identity, ref ignoreFileInfos);
          break;
        }
    }

    private static void WriteDownloadFileInfos(XmlWriter xmlWriter,
      DirectoryInfo directoryInfo, string directoryName, string sourceDirectoryName, IList<string> searchPatterns, UserIdentity identity, 
      ref Dictionary<string, FileInfo> ignoreFileInfos)
    {
      sourceDirectoryName = sourceDirectoryName ?? String.Empty;
      foreach (DirectoryInfo item in directoryInfo.GetDirectories())
        if (String.Compare(item.Name, Phenix.Core.AppConfig.CLIENT_LIBRARY_OWN_SUBDIRECTORY_NAME, StringComparison.OrdinalIgnoreCase) == 0)
          WriteOwnDownloadFileInfos(xmlWriter, item, directoryName, Path.Combine(sourceDirectoryName, item.Name), searchPatterns, identity, ref ignoreFileInfos);
        else
          WriteDownloadFileInfos(xmlWriter, item, Path.Combine(directoryName, item.Name), Path.Combine(sourceDirectoryName, item.Name), searchPatterns, identity);
      foreach (string s in searchPatterns)
      foreach (FileInfo item in directoryInfo.GetFiles(s))
        if (String.Compare(item.Name, AppDomain.CurrentDomain.FriendlyName, StringComparison.OrdinalIgnoreCase) != 0 &&
          String.Compare(item.Name, Path.GetFileName(AppDomain.CurrentDomain.SetupInformation.ConfigurationFile), StringComparison.OrdinalIgnoreCase) != 0)
        {
          string path = Path.Combine(directoryName, item.Name);
          if (ignoreFileInfos.ContainsKey(path))
              continue;
          ignoreFileInfos[path] = item;
          xmlWriter.WriteStartElement("FileInfo");
          xmlWriter.WriteAttributeString("DirectoryName", directoryName);
          xmlWriter.WriteAttributeString("SourceDirectoryName", sourceDirectoryName);
          xmlWriter.WriteAttributeString("Name", item.Name);
          xmlWriter.WriteAttributeString("Length", item.Length.ToString());
          xmlWriter.WriteAttributeString("LastWriteTime", item.LastWriteTime.ToBinary().ToString());
          xmlWriter.WriteEndElement();
        }
    }

    private static void WriteDownloadFileInfos(XmlWriter xmlWriter,
      DirectoryInfo directoryInfo, string directoryName, string sourceDirectoryName, IList<string> searchPatterns, UserIdentity identity)
    {
      Dictionary<string, FileInfo> ignoreFileInfos = new Dictionary<string, FileInfo>(StringComparer.OrdinalIgnoreCase);
      WriteDownloadFileInfos(xmlWriter, directoryInfo, directoryName, sourceDirectoryName, searchPatterns, identity, ref ignoreFileInfos);
    }

    /// <summary>
    /// ȡ�����ļ���Ϣ
    /// </summary>
    /// <param name="applicationName">Ӧ�ó�����</param>
    /// <param name="searchPatterns">�����ļ�����������</param>
    /// <param name="identity">�û����</param>
    public static string GetDownloadFileInfos(string applicationName, IList<string> searchPatterns, UserIdentity identity)
    {
      if (searchPatterns == null)
        searchPatterns = new Collection<string> {"*.*"};
      else
        searchPatterns.Add(applicationName + ".*");
      StringBuilder result = new StringBuilder();
      using (XmlWriter xmlWriter = XmlWriter.Create(result, new XmlWriterSettings { ConformanceLevel = ConformanceLevel.Fragment }))
      {
        WriteDownloadFileInfos(xmlWriter, new DirectoryInfo(AppConfig.ClientLibrarySubdirectory), String.Empty, String.Empty, searchPatterns, identity);
      }
      return result.ToString();
    }

    /// <summary>
    /// ȡ�����ļ�
    /// </summary>
    /// <param name="directoryName">Ŀ¼��</param>
    /// <param name="sourceDirectoryName">ԴĿ¼��</param>
    /// <param name="fileName">�ļ���</param>
    /// <param name="chunkNumber">���</param>
    public static DownloadFileInfo GetDownloadFile(string directoryName, string sourceDirectoryName, string fileName, int chunkNumber)
    {
      string path = Path.Combine(AppConfig.ClientLibrarySubdirectory, sourceDirectoryName ?? String.Empty, fileName);
      return File.Exists(path) ? new DownloadFileInfo(directoryName, FileHelper.ReadChunkInfo(path, chunkNumber)) : null;
    }
  }

  #endregion
}