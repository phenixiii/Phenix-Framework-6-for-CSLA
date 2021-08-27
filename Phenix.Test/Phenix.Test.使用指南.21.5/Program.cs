using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Windows.Forms;

namespace Phenix.Test.使用指南._21._5
{
  class Program
  {
    [STAThread]
    static void Main(string[] args)
    {
      Console.WriteLine("请通过检索“//*”了解代码中的特殊处理");
      Console.WriteLine("请事先编译Phenix.Test.使用指南.21.5.Business工程输出DLL到Bin.Top目录");
      Console.WriteLine("然后启动Bin.Top目录下的Phenix.Services.Host.x86/x64.exe程序");
      Console.WriteLine("通过Host程序注册Phenix.Test.使用指南.21.5.Business.DLL内的业务类");
      Console.WriteLine("请不要关闭Host程序");
      Console.WriteLine("如需观察日志（被保存在TempDirectory子目录里）可将Host的Debugging功能菜单点亮");
      Console.Write("准备好后请点回车继续：");
      Console.ReadLine();

      Phenix.Web.Client.Security.UserIdentity guestIdentity = Phenix.Web.Client.Security.UserIdentity.CreateGuest();
      using (Phenix.Web.Client.HttpClient client = new Phenix.Web.Client.HttpClient("127.0.0.1", 8080, guestIdentity))
      {
        Console.WriteLine("**** 测试匿名下Phenix.Core.AppHub.DataProxy.Execute()调用WabAPI服务 ****");
        ServiceData serviceData = (ServiceData)Phenix.Core.Web.AppHub.DataProxy.Execute("Phenix.Test.使用指南._21._5.Business.Service",
          new ServiceData()
          {
            Assembly = (Assembly)Activator.CreateInstance(typeof(Assembly), true) //随便赋个值，不希望服务端Phenix.Test.使用指南._21._5.Business.Service.OnlyDownloadFile=true而下载文件
          });
        Console.WriteLine();
      }

      Phenix.Web.Client.Security.UserIdentity userIdentity = new Phenix.Web.Client.Security.UserIdentity("ADMIN", "ADMIN");
      using (Phenix.Web.Client.HttpClient client = new Phenix.Web.Client.HttpClient("127.0.0.1", 8080, userIdentity))
      {
        while (true)
        {
          using (HttpResponseMessage message = client.SecurityProxy.TryLogOnAsync().Result)
          {
            if (message.StatusCode == HttpStatusCode.OK)
            {
              Console.WriteLine("登录成功");
              break;
            }
            Console.WriteLine("登录未成功：" + message.Content.ReadAsStringAsync().Result);
            Console.Write("请重新登录，输入ADMIN的口令：");
          }
          userIdentity.Password = Console.ReadLine();
        }
        Console.WriteLine();

        Console.WriteLine("*****************************");
        Console.WriteLine("**** 开始测试Fetch功能 ****");
        Console.WriteLine();

        Console.WriteLine("**** 测试Phenix.Core.AppHub.DataProxy.FetchSequenceValue()调用WabAPI服务获取序列号 ****");
        for (int i = 0; i < 10; i++)
          Console.WriteLine(Phenix.Core.Web.AppHub.DataProxy.FetchSequenceValue());
        Console.WriteLine();

        AssemblyList assemblyList;
        long assemblyId = 0;
        const int pageSize = 5;

        while (true)
        {
          try
          {
            Console.WriteLine("**** 测试业务集合类Fetch()间接通过Phenix.Core.AppHub.DataProxy.FetchList<T>()调用WabAPI服务获取完整实体集合 ****");
            assemblyList = AssemblyList.Fetch(0);
            Console.WriteLine("Fetch()实体数量=" + assemblyList.Count + ' ' + (assemblyList.Count > 0 ? "ok" : "error"));
            Console.WriteLine();
            break;
          }
          catch (HttpRequestException ex)
          {
            Console.Write("请通过Phenix.Services.Host.x86.exe注册Phenix.Test.使用指南.21.5.Business.dll程序集! error: " + Phenix.Core.AppUtilities.GetErrorMessage(ex));
            Console.ReadLine();
          }
        }
        Console.WriteLine();

        Console.WriteLine("**** 测试业务集合类Fetch()间接通过Phenix.Core.AppHub.DataProxy.FetchList<T>()调用WabAPI服务获取分页实体集合 ****");
        assemblyList = AssemblyList.Fetch(pageSize, 1);
        Console.WriteLine("Fetch()实体数量=" + assemblyList.Count + ' ' + (assemblyList.Count > 0 && assemblyList.Count <= pageSize ? "ok" : "error"));
        for (int i = 0; i < assemblyList.Count; i++)
        {
          Assembly item = assemblyList[i];
          Console.WriteLine("index " + i + ": ID=" + item.AS_ID + ",Name=" + item.Name + ",Caption=" + item.Caption);
          assemblyId = item.AS_ID.Value; //最后一次赋值将被用于之后的Fetch单个实体时传入id值
        }
        Console.WriteLine();

        Console.WriteLine("**** 测试业务集合类Fetch()间接通过Phenix.Core.AppHub.DataProxy.FetchList<T>()调用WabAPI服务按Criteria获取完整实体集合 ****");
        assemblyList = AssemblyList.Fetch(
          new AssemblyCriteria()
          {
            Name = "Phenix.Test." //CriteriaOperate.Like
          }, 0);
        Console.WriteLine("Fetch()实体数量=" + assemblyList.Count + ' ' + (assemblyList.Count > 0 ? "ok" : "error"));
        Console.WriteLine();

        Console.WriteLine("**** 测试业务集合类Fetch()间接通过Phenix.Core.AppHub.DataProxy.FetchList<T>()调用WabAPI服务按Criteria获取分页实体集合 ****");
        assemblyList = AssemblyList.Fetch(
          new AssemblyCriteria()
          {
            Name = "Phenix.Test." //CriteriaOperate.Like
          },
          pageSize, 1);
        Console.WriteLine("Fetch()实体数量=" + assemblyList.Count + ' ' + (assemblyList.Count > 0 && assemblyList.Count <= pageSize ? "ok" : "error"));
        Console.WriteLine();

        Console.WriteLine("**** 测试业务类Fetch()间接通过Phenix.Core.AppHub.DataProxy.Fetch<T>()调用WabAPI服务获取单个业务对象 ****");
        Assembly assembly = Assembly.Fetch(assemblyId);
        if (assembly != null)
          Console.WriteLine("Fetch()单个业务对象: ID=" + assembly.AS_ID + ",Name=" + assembly.Name + ",Caption=" + assembly.Caption + ' ' + (assembly.AS_ID == assemblyId ? "ok" : "error"));
        else
          Console.WriteLine("未能Fetch()到单个业务对象: ID=" + assemblyId + " error");
        Console.WriteLine();

        Console.WriteLine("**** 测试业务集合类Fetch()间接通过Phenix.Core.AppHub.DataProxy.FetchList<T>()调用WabAPI服务获取主实体下完整从实体集合 ****");
        AssemblyClassList assemblyClassList = AssemblyClassList.Fetch(assembly, null, 0);
        Console.WriteLine("Fetch()从实体数量=" + assemblyClassList.Count + ' ' + (assemblyClassList.Count > 0 ? "ok" : "error"));
        for (int i = 0; i < assemblyClassList.Count; i++)
        {
          AssemblyClass item = assemblyClassList[i];
          Console.WriteLine("index " + i + ": ID=" + item.AC_ID + ",Name=" + item.Name + ",Caption=" + item.Caption);
        }
        Console.WriteLine();

        Console.WriteLine("**** 测试Phenix.Core.AppHub.DataProxy.FetchList<T>()调用WabAPI服务按主键值获取完整从实体集合 ****");
        assemblyClassList = Phenix.Core.Web.AppHub.DataProxy.FetchList<AssemblyClassList>(
          "Phenix.Test.使用指南._21._5.Business.AssemblyClassList",
          "Phenix.Test.使用指南._21._5.Business.Assembly", assemblyId, null, null, null, null, null);
        Console.WriteLine("Fetch()从实体数量=" + assemblyClassList.Count + ' ' + (assemblyClassList.Count > 0 ? "ok" : "error"));
        Console.WriteLine();

        if (assembly == null)
        {
          Console.WriteLine("测试Fetch()功能失败，无法继续测试Save()功能");
          Console.ReadLine();
          return;
        }

        string assemblyCaption = assembly.Caption;
        assembly.AssemblyClasses = AssemblyClassList.Fetch(assembly, null);
        string assemblyClassCaption = assembly.AssemblyClasses[0].Caption;

        Console.WriteLine("****************************");
        Console.WriteLine("**** 开始测试Save功能 ****");
        Console.WriteLine();

        Console.WriteLine("**** 测试业务对象Save()间接通过Phenix.Core.AppHub.DataProxy.Save()调用WabAPI服务保存 ****");
        assembly.Caption = DateTime.Now.ToString();
        assembly.AssemblyClasses[0].Caption = DateTime.Now.ToString();
        int count = assembly.Save();
        Console.WriteLine("Save()更新" + (count > 0 ? "成功 ok" : "未成功 error"));
        Console.WriteLine("当前对象仍然是" + (assembly.IsSelfDirty ? "脏对象 ok" : "非脏对象 error"));
        assembly.ApplyEdit();
        Console.WriteLine("调用ApplyEdit()后是" + (!assembly.IsSelfDirty ? "非脏对象 ok" : "脏对象 error"));
        Console.WriteLine("从表实体仍然是" + (assembly.AssemblyClasses[0].IsSelfDirty ? "脏对象 ok" : "非脏对象 error"));
        assembly.AssemblyClasses.ApplyEdit();
        Console.WriteLine("调用ApplyEdit()后是" + (!assembly.AssemblyClasses[0].IsSelfDirty ? "非脏对象 ok" : "脏对象 error"));
        Console.WriteLine();

        Console.WriteLine("**** 测试Phenix.Core.AppHub.DataProxy.Save()调用WabAPI服务保存单个实体 ****");
        count = Phenix.Core.Web.AppHub.DataProxy.Save(
          "Phenix.Test.使用指南._21._5.Business.Assembly",
          new LocalAssembly()
          {
            IsSelfDirty = true,
            AS_ID = assembly.AS_ID,
            Name = assembly.Name,
            Caption = DateTime.Now.ToString()
          });
        Console.WriteLine("Save()" + (count > 0 ? "成功 ok" : "未成功 error"));
        Console.WriteLine();

        Console.WriteLine("**** 测试业务集合对象Save()间接通过Phenix.Core.AppHub.DataProxy.Save()调用WabAPI服务保存 ****");
        Assembly newAssembly = Assembly.New();
        newAssembly.Name = DateTime.Now.ToString();
        assemblyList.Add(newAssembly);
        count = assemblyList.Save();
        Console.WriteLine("Save()新增" + (count > 0 ? "成功 ok" : "未成功 error"));
        Console.WriteLine("新增对象仍然是" + (newAssembly.IsNew ? "新对象 ok" : "非新对象 error"));
        assemblyList.ApplyEdit();
        Console.WriteLine("调用ApplyEdit()后是" + (!newAssembly.IsNew ? "非新对象 ok" : "新对象 error"));
        newAssembly.Delete();
        count = assemblyList.Save();
        Console.WriteLine("Save()删除" + (count > 0 ? "成功 ok" : "未成功 error"));
        Console.WriteLine();

        Console.WriteLine("****************************");
        Console.WriteLine("**** 开始测试Execute功能 ****");
        Console.WriteLine();

        Console.WriteLine("**** 测试间接通过Phenix.Core.AppHub.DataProxy.Execute()调用WabAPI服务并执行 ****");
        Service service = Service.Execute(
          new Service()
          {
            Assembly = assembly
          });
        Console.WriteLine("Execute()返回结果: " + service.Result);
        Console.WriteLine();

        Console.WriteLine("**** 测试Phenix.Core.AppHub.DataProxy.Execute()调用WabAPI服务并执行 ****");
        ServiceData serviceData = (ServiceData) Phenix.Core.Web.AppHub.DataProxy.Execute("Phenix.Test.使用指南._21._5.Business.Service",
          new ServiceData()
          {
            Assembly = assembly
          });
        Console.WriteLine("Execute()返回结果: " + serviceData.Result);
        Console.WriteLine();

        Console.WriteLine("****************************");
        Console.WriteLine("**** 开始测试UploadFiles功能 ****");
        Console.WriteLine();

        Console.WriteLine("**** 测试间接通过Phenix.Core.AppHub.DataProxy.UploadFiles()调用WabAPI服务并上传文件 ****");
        using (OpenFileDialog openFileDialog = new OpenFileDialog())
        {
          openFileDialog.RestoreDirectory = true;
          openFileDialog.Multiselect = true;
          openFileDialog.Filter = "图片|*.jpg";
          openFileDialog.Title = "上传文件";
          if (openFileDialog.ShowDialog() == DialogResult.OK)
          {
            service = Service.UploadFiles(
              new Service()
              {
                Assembly = assembly
              }, openFileDialog.FileNames);
            Console.WriteLine("UploadFiles()返回结果: " + service.Result);
            Console.WriteLine();
          }
        }

        Console.WriteLine("**** 测试Phenix.Core.AppHub.DataProxy.UploadFiles()调用WabAPI服务并上传文件 ****");
        using (OpenFileDialog openFileDialog = new OpenFileDialog())
        {
          openFileDialog.RestoreDirectory = true;
          openFileDialog.Multiselect = true;
          openFileDialog.Filter = "图片|*.jpg";
          openFileDialog.Title = "上传文件";
          if (openFileDialog.ShowDialog() == DialogResult.OK)
          {
            serviceData = (ServiceData) Phenix.Core.Web.AppHub.DataProxy.UploadFiles("Phenix.Test.使用指南._21._5.Business.Service",
              new ServiceData()
              {
                Assembly = assembly
              }, openFileDialog.FileNames);
            Console.WriteLine("UploadFiles()返回结果: " + serviceData.Result);
            Console.WriteLine();
          }
        }


        Console.WriteLine("****************************");
        Console.WriteLine("**** 开始测试UploadBigFile功能 ****");
        Console.WriteLine();

        Console.WriteLine("**** 测试间接通过Phenix.Core.AppHub.DataProxy.UploadBigFile()调用WabAPI服务并上传文件 ****");
        using (OpenFileDialog openFileDialog = new OpenFileDialog())
        {
          openFileDialog.RestoreDirectory = true;
          openFileDialog.Multiselect = false;
          openFileDialog.Title = "上传文件";
          if (openFileDialog.ShowDialog() == DialogResult.OK)
          {
            service = Service.UploadBigFile(
              new Service()
              {
                Assembly = assembly
              }, openFileDialog.FileName, (sender, fileChunkInfo) =>
              {
                Console.WriteLine(String.Format("上传进度：{0}/{1}", fileChunkInfo.ChunkNumber, fileChunkInfo.ChunkCount));
                return true; //继续上传
              });
            Console.WriteLine("UploadBigFile()返回结果: " + service.Result);
            Console.WriteLine();
          }
        }

        Console.WriteLine("**** 测试Phenix.Core.AppHub.DataProxy.UploadBigFile()调用WabAPI服务并上传文件 ****");
        using (OpenFileDialog openFileDialog = new OpenFileDialog())
        {
          openFileDialog.RestoreDirectory = true;
          openFileDialog.Multiselect = false;
          openFileDialog.Title = "上传文件";
          if (openFileDialog.ShowDialog() == DialogResult.OK)
          {
            serviceData = (ServiceData) Phenix.Core.Web.AppHub.DataProxy.UploadBigFile("Phenix.Test.使用指南._21._5.Business.Service",
              new ServiceData()
              {
                Assembly = assembly
              }, openFileDialog.FileName, (sender, fileChunkInfo) =>
              {
                Console.WriteLine(String.Format("上传进度：{0}/{1}", fileChunkInfo.ChunkNumber, fileChunkInfo.ChunkCount));
                return true; //继续上传
              });
            Console.WriteLine("UploadFiles()返回结果: " + serviceData.Result);
            Console.WriteLine();
          }
        }

        string fileName = "Phenix.Test.使用指南.21.5.下载文件本地保存.jpg";

        Console.WriteLine("****************************");
        Console.WriteLine("**** 开始测试DownloadFile功能 ****");
        Console.WriteLine();

        fileName = "_1" + fileName;
        Console.WriteLine("**** 测试间接通过Phenix.Core.AppHub.DataProxy.DownloadFile()调用WabAPI服务并执行 ****");
        Service.DownloadFile(
          new Service()
          {
            Assembly = assembly
          }, fileName);
        Console.WriteLine("下载文件: " + fileName);
        Console.WriteLine();

        fileName = "_2" + fileName;
        Console.WriteLine("**** 测试Phenix.Core.AppHub.DataProxy.DownloadFile()调用WabAPI服务并执行 ****");
        Phenix.Core.Web.AppHub.DataProxy.DownloadFile("Phenix.Test.使用指南._21._5.Business.Service",
          new ServiceData()
          {
            Assembly = assembly
          }, fileName);
        Console.WriteLine("下载文件: " + fileName);
        Console.WriteLine();

        Console.WriteLine("****************************");
        Console.WriteLine("**** 开始测试DownloadBigFile功能 ****");
        Console.WriteLine();

        fileName = "_3" + fileName;
        Console.WriteLine("**** 测试间接通过Phenix.Core.AppHub.DataProxy.DownloadBigFile()调用WabAPI服务并执行 ****");
        Service.DownloadBigFile(
          new Service()
          {
            Assembly = assembly
          }, fileName, (sender, fileChunkInfo) =>
          {
            Console.WriteLine(String.Format("下载进度：{0}/{1}", fileChunkInfo.ChunkNumber, fileChunkInfo.ChunkCount));
            return true; //继续上传
          });
        Console.WriteLine("下载文件: " + fileName);
        Console.WriteLine();

        fileName = "_4" + fileName;
        Console.WriteLine("**** 测试Phenix.Core.AppHub.DataProxy.DownloadBigFile()调用WabAPI服务并执行 ****");
        Phenix.Core.Web.AppHub.DataProxy.DownloadBigFile("Phenix.Test.使用指南._21._5.Business.Service",
          new ServiceData()
          {
            Assembly = assembly
          }, fileName, (sender, fileChunkInfo) =>
          {
            Console.WriteLine(String.Format("下载进度：{0}/{1}", fileChunkInfo.ChunkNumber, fileChunkInfo.ChunkCount));
            return true; //继续上传
          });
        Console.WriteLine("下载文件: " + fileName);
        Console.WriteLine();

        Console.WriteLine("**** 恢复测试环境 ****");
        assembly.Caption = assemblyCaption;
        assembly.AssemblyClasses[0].Caption = assemblyClassCaption;
        count = assembly.Save();
        Console.WriteLine("恢复测试环境" + (count > 0 ? "ok" : "error"));
        Console.WriteLine();
      }

      Console.WriteLine("结束, 与数据库交互细节见日志");
      Console.ReadLine();
    }
  }
}
