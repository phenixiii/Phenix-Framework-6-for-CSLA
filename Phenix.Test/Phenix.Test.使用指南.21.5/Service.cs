using System.Collections.Generic;
using System.IO;
using Phenix.Core.IO;
using Phenix.Core.Mapping;

namespace Phenix.Test.使用指南._21._5.Business
{
  //* 允许匿名访问
  [Class("", OnAnonymity = true)]
  [System.SerializableAttribute()]
  public class Service : Phenix.Core.Data.ServiceBase<Service>
  {
    private Assembly _assembly;
    public Assembly Assembly
    {
      get { return _assembly; }
      set { _assembly = value; }
    }

    private string _result;
    public string Result
    {
      get { return _result; }
    }

    /// <summary>
    /// 处理执行指令(运行在持久层的程序域里)
    /// </summary>
    protected override void DoExecute()
    {
      _result = _assembly != null ? "服务端取到：" + _assembly.AS_ID : "服务端被调用但未有传递来Assembly对象";
      _assembly = null; //不用返回客户端
    }

    /// <summary>
    /// 处理上传文件(运行在持久层的程序域里)
    /// </summary>
    /// <param name="fileStreams">待处理的文件流</param>
    protected override void DoUploadFiles(IDictionary<string, Stream> fileStreams)
    {
      _result = _assembly != null ? "服务端取到：" + _assembly.Caption : "服务端被调用";
      _result = _result + ", 处理文件流：";
      foreach (KeyValuePair<string, Stream> kvp in fileStreams)
      {
        using (FileStream targetStream = new FileStream(kvp.Key, FileMode.Create, FileAccess.Write)) //kvp.Key仅是文件名，可以改成服务端的某个保存路径+文件名
        {
          kvp.Value.Position = 0;
          Phenix.Core.IO.StreamHelper.CopyBuffer(kvp.Value, targetStream);
        }
        Phenix.Core.AppSettings.SaveValue("最近一个文件名", kvp.Key); //为DoDownloadFile()、DoDownloadBigFile()暂存可下载的测试文件路径，也可用静态变量
        _result = _result + kvp.Key + " ";
      }
      _assembly = null; //不用返回客户端
    }

    /// <summary>
    /// 处理上传大文件(运行在持久层的程序域里)
    /// </summary>
    /// <param name="fileChunkInfo">待处理的文件块信息</param>
    protected override void DoUploadBigFile(FileChunkInfo fileChunkInfo)
    {
      _result = _assembly != null ? "服务端取到：" + _assembly.Caption : "服务端被调用";
      _result = _result + ", 处理文件流：" + fileChunkInfo.FileName;
      if (fileChunkInfo.Stop)
      {
        if (File.Exists(fileChunkInfo.FileName))
          File.Delete(fileChunkInfo.FileName);
        _result = _result + "...中止!";
      }
      else
      {
        FileHelper.WriteChunkInfo(fileChunkInfo.FileName, fileChunkInfo); //fileChunkInfo.FileName仅是文件名，可以改成服务端的某个保存路径+文件名
        if (fileChunkInfo.Over)
        {
          Phenix.Core.AppSettings.SaveValue("最近一个文件名", fileChunkInfo.FileName); //为DoDownloadFile()、DoDownloadBigFile()暂存可下载的测试文件路径，也可用静态变量
          _result = _result + "...结束";
        }
        else
          _result = _result + "...第" + fileChunkInfo.ChunkNumber + "块";
      }
      _assembly = null; //不用返回客户端
    }

    /// <summary>
    /// 获取下载文件(运行在持久层的程序域里)
    /// </summary>
    /// <returns>文件字节串</returns>
    protected override Stream DoDownloadFile()
    {
      string path = Phenix.Core.AppSettings.ReadValue("最近一个文件名");
      return new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read);
    }

    /// <summary>
    /// 获取下载大文件(运行在持久层的程序域里)
    /// </summary>
    /// <param name="chunkNumber">块号</param>
    /// <returns>文件块信息</returns>
    protected override FileChunkInfo DoDownloadBigFile(int chunkNumber)
    {
      string path = Phenix.Core.AppSettings.ReadValue("最近一个文件名");
      using (FileStream fileStream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read))
      {
        return FileHelper.ReadChunkInfo(path, fileStream, chunkNumber);
      }
    }
  }
}
