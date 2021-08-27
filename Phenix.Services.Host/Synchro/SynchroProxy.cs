using System;
using System.IO;
using System.Net.Sockets;
using System.Windows.Forms;
using Phenix.Core;
using Phenix.Core.IO;
using Phenix.Core.Net;

namespace Phenix.Services.Host.Synchro
{
  internal class SynchroProxy
  {
    #region 属性

    private ISynchro _service;
    private ISynchro Service
    {
      get
      {
        if (_service == null)
        {
          RemotingHelper.RegisterClientChannel();
          _service = (ISynchro)RemotingHelper.CreateRemoteObjectProxy(typeof(ISynchro), SynchroService.URI);
        }
        return _service;
      }
    }

    private bool _shutDown = true;
    /// <summary>
    /// 中止
    /// </summary>
    public bool ShutDown
    {
      get { return _shutDown; }
      set { _shutDown = value; }
    }

    #endregion

    #region 事件

    public event Action<MessageNotifyEventArgs> MessageNotify;
    private void OnMessageNotify(MessageNotifyEventArgs e)
    {
      Action<MessageNotifyEventArgs> handler = MessageNotify;
      if (handler != null)
        handler(e);
    }

    #endregion

    #region 方法

    private void InvalidateCache()
    {
      _service = null;
    }

    private void ChangeServer(string address)
    {
      InvalidateCache();
      NetConfig.ServicesAddress = address;
    }

    private void ClearServiceLibrarySubdirectory()
    {
      try
      {
        Service.ClearServiceLibrarySubdirectory();
      }
      catch (SocketException)
      {
        InvalidateCache();
        throw;
      }
    }

    private void Upload(string subdirectoryName, string fileName, int fileLength, byte[] fileBytes)
    {
      try
      {
        Service.Upload(subdirectoryName, fileName, fileLength, fileBytes);
      }
      catch (SocketException)
      {
        InvalidateCache();
        throw;
      }
    }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
    private bool Upload(string subdirectoryName, string path)
    {
      do
      {
        try
        {
          Application.DoEvents();
          if (ShutDown)
            return false;
          using (MemoryStream outputStream = new MemoryStream())
          using (FileStream inputStream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read))
          {
            CompressHelper.Compress(inputStream, outputStream);
            Upload(subdirectoryName, Path.GetFileName(path), (int)inputStream.Length, outputStream.ToArray());
          }
          OnMessageNotify(new MessageNotifyEventArgs(MessageNotifyType.Information, "Uploaded", path));
          break;
        }
        catch (Exception ex)
        {
          DialogResult result = MessageBox.Show("Upload fails:\n" + AppUtilities.GetErrorHint(ex), path, MessageBoxButtons.AbortRetryIgnore, MessageBoxIcon.Error);
          if (result == DialogResult.Abort)
            return false;
          if (result == DialogResult.Ignore)
            break;
        }
      } while (true);
      return true;
    }

    //private bool Upload(DirectoryInfo directoryInfo, string subdirectoryName)
    //{
    //  //上传subdirectoryName目录里文件
    //  foreach (FileInfo item in directoryInfo.GetFiles())
    //    if (!Upload(subdirectoryName, item.FullName))
    //      return false;
    //  //上传subdirectoryName子目录里文件
    //  foreach (DirectoryInfo item in directoryInfo.GetDirectories())
    //    if (!Upload(item, Path.Combine(subdirectoryName, item.Name)))
    //      return false;
    //  return true;
    //}

    private bool Upload()
    {
      ClearServiceLibrarySubdirectory();
      //上传AppConfig.BaseDirectory目录里文件
      foreach (string s in Directory.GetFiles(AppConfig.BaseDirectory))
        if (!Upload(String.Empty, s))
          return false;
      ////上传AppConfig.DefaultClientLibrarySubdirectory目录里文件
      //if (!Upload(new DirectoryInfo(AppConfig.ClientLibrarySubdirectory), AppConfig.CLIENT_LIBRARY_SUBDIRECTORY_NAME))
        //return false;
      return true;
    }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
    public void Deploy(string[] hosts)
    {
      ShutDown = false;
      try
      {
        foreach (string s in hosts)
          do
          {
            try
            {
              ChangeServer(s);
              OnMessageNotify(new MessageNotifyEventArgs(MessageNotifyType.Information, "Start to upload files", s));
              if (Upload())
                OnMessageNotify(new MessageNotifyEventArgs(MessageNotifyType.Information, "Upload completed", s));
              break;
            }
            catch (Exception ex)
            {
              DialogResult result = MessageBox.Show("Upload fails:\n" + AppUtilities.GetErrorHint(ex), s, MessageBoxButtons.AbortRetryIgnore, MessageBoxIcon.Error);
              if (result == DialogResult.Abort)
                return;
              if (result == DialogResult.Ignore)
                break;
            }
          } while (true);
      }
      finally
      {
        ShutDown = true;
      }
    }

    #endregion
  }
}