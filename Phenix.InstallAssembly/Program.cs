using System;
using System.IO;
using Phenix.Core;

namespace Phenix.InstallAssembly
{
    internal static class Program
    {
        [STAThread]
        private static void Main(string[] args)
        {
            //拷贝出来客户端程序集
            RegisterClientLibrary(Phenix.Core.AppConfig.ClientLibrarySubdirectory);
            //符合命名规范的程序集
            foreach (string s in Directory.GetFiles(Phenix.Core.AppConfig.BaseDirectory, "*.Business.*.dll"))
                CopyFile(Path.GetFileName(s), Phenix.Core.AppConfig.ClientLibrarySubdirectory);
            foreach (string s in Directory.GetFiles(Phenix.Core.AppConfig.BaseDirectory, "*.Windows.*.dll"))
                CopyFile(Path.GetFileName(s), Phenix.Core.AppConfig.ClientLibrarySubdirectory);

            //拷贝出来服务端程序集
            RegisterServicesLibrary(Phenix.Core.AppConfig.ServiceLibrarySubdirectory);
            //符合命名规范的程序集
            foreach (string s in Directory.GetFiles(Phenix.Core.AppConfig.BaseDirectory, "*.Business.*.dll"))
                CopyFile(Path.GetFileName(s), Phenix.Core.AppConfig.ServiceLibrarySubdirectory);

            Console.WriteLine("完成");
            Console.ReadLine();
        }

        private static void RegisterClientLibrary(string directory)
        {
            foreach (string s in Directory.GetFiles(Phenix.Core.AppConfig.BaseDirectory, "DevExpress.*.dll"))
                CopyFile(Path.GetFileName(s), directory);
            CopyFile("ChnCharInfo.dll", directory);
            CopyFile("EastAsiaNumericFormatter.dll", directory);
            CopyFile("Newtonsoft.Json.dll", directory);
            CopyFile("Csla.dll", directory);
            CopyFile("Phenix.Core.dll", directory);
            CopyFile("Phenix.Core.dll.config", directory);
            CopyFile("Phenix.Business.dll", directory);
            CopyFile("Phenix.Services.Contract.dll", directory);
            CopyFile("Phenix.Services.Client.dll", directory);
            CopyFile("Phenix.Windows.dll", directory);
            CopyFile("Phenix.TeamHub.Prober.dll", directory);
            CopyFile("Phenix.TeamHub.Prober.Business.dll", directory);

            string resourcesDirectory = directory + @"\zh-CHS";
            if (!Directory.Exists(resourcesDirectory))
                Directory.CreateDirectory(resourcesDirectory);
            CopyFile(@"zh-CHS\Csla.resources.dll", directory);
            CopyFile(@"zh-CHS\Phenix.Core.resources.dll", directory);
            CopyFile(@"zh-CHS\Phenix.Business.resources.dll", directory);
            CopyFile(@"zh-CHS\Phenix.Services.Client.resources.dll", directory);
            CopyFile(@"zh-CHS\Phenix.Windows.resources.dll", directory);

            resourcesDirectory = directory + @"\zh-CN";
            if (!Directory.Exists(resourcesDirectory))
                Directory.CreateDirectory(resourcesDirectory);
            CopyFile(@"zh-CN\Csla.resources.dll", directory);
            CopyFile(@"zh-CN\Phenix.Core.resources.dll", directory);
            CopyFile(@"zh-CN\Phenix.Business.resources.dll", directory);
            CopyFile(@"zh-CN\Phenix.Services.Client.resources.dll", directory);
            CopyFile(@"zh-CN\Phenix.Windows.resources.dll", directory);

            resourcesDirectory = directory + @"\zh-Hans";
            if (!Directory.Exists(resourcesDirectory))
                Directory.CreateDirectory(resourcesDirectory);
            CopyFile(@"zh-Hans\Csla.resources.dll", directory);
            CopyFile(@"zh-Hans\Phenix.Core.resources.dll", directory);
            CopyFile(@"zh-Hans\Phenix.Business.resources.dll", directory);
            CopyFile(@"zh-Hans\Phenix.Services.Client.resources.dll", directory);
            CopyFile(@"zh-Hans\Phenix.Windows.resources.dll", directory);


            //框架提供的程序入口示例
            //一般实际部署时，是与以上程序集一起打包分发，登录时自动下载或升级ClientLibrary里的程序集
            CopyFile("Phenix.Windows.Client.exe", directory);
            CopyFile("Phenix.Windows.Client.exe.config", directory);
            CopyFile("Phenix.Windows.Main.dll", directory);
        }

        private static void RegisterServicesLibrary(string directory)
        {
            foreach (string s in Directory.GetFiles(Phenix.Core.AppConfig.BaseDirectory, "System.Web.*.dll"))
                CopyFile(Path.GetFileName(s), directory);
            foreach (string s in Directory.GetFiles(Phenix.Core.AppConfig.BaseDirectory, "System.Net.*.dll"))
                CopyFile(Path.GetFileName(s), directory);
            foreach (string s in Directory.GetFiles(Phenix.Core.AppConfig.BaseDirectory, "Microsoft.Owin.*.dll"))
                CopyFile(Path.GetFileName(s), directory);
            foreach (string s in Directory.GetFiles(Phenix.Core.AppConfig.BaseDirectory, "Microsoft.Web.*.dll"))
                CopyFile(Path.GetFileName(s), directory);
            foreach (string s in Directory.GetFiles(Phenix.Core.AppConfig.BaseDirectory, "Microsoft.AspNet.*.dll"))
                CopyFile(Path.GetFileName(s), directory);
            CopyFile("Microsoft.Owin.dll", directory);
            CopyFile("Owin.dll", directory);
            CopyFile("ChnCharInfo.dll", directory);
            CopyFile("EastAsiaNumericFormatter.dll", directory);
            CopyFile("Newtonsoft.Json.dll", directory);
            CopyFile("Csla.dll", directory);
            CopyFile("Phenix.Core.dll", directory);
            CopyFile("Phenix.Core.dll.config", directory);
            CopyFile("Phenix.Business.dll", directory);
            CopyFile("Phenix.Services.Contract.dll", directory);
            CopyFile("Phenix.Services.Library.dll", directory);
            CopyFile("Phenix.Services.Host.x64.exe", directory);
            CopyFile("Phenix.Services.Host.x86.exe", directory);
            CopyFile("Phenix.Services.Host.x64.exe.config", directory);
            CopyFile("Phenix.Services.Host.x86.exe.config", directory);
            CopyFile("Phenix.Services.Host.Kaishaku.exe", directory);
            CopyFile("Phenix.Services.Host.Monitor.exe", directory);
            CopyFile("Phenix.Services.Host.Monitor.x64.bat", directory);
            CopyFile("Phenix.Services.Host.Monitor.x86.bat", directory);
            CopyFile("Phenix.TeamHub.Prober.Business.dll", directory);

            string resourcesDirectory = directory + @"\zh-CHS";
            if (!Directory.Exists(resourcesDirectory))
                Directory.CreateDirectory(resourcesDirectory);
            CopyFile(@"zh-CHS\Csla.resources.dll", directory);
            CopyFile(@"zh-CHS\Phenix.Core.resources.dll", directory);
            CopyFile(@"zh-CHS\Phenix.Business.resources.dll", directory);

            resourcesDirectory = directory + @"\zh-CN";
            if (!Directory.Exists(resourcesDirectory))
                Directory.CreateDirectory(resourcesDirectory);
            CopyFile(@"zh-CN\Csla.resources.dll", directory);
            CopyFile(@"zh-CN\Phenix.Core.resources.dll", directory);
            CopyFile(@"zh-CN\Phenix.Business.resources.dll", directory);

            resourcesDirectory = directory + @"\zh-Hans";
            if (!Directory.Exists(resourcesDirectory))
                Directory.CreateDirectory(resourcesDirectory);
            CopyFile(@"zh-Hans\Csla.resources.dll", directory);
            CopyFile(@"zh-Hans\Phenix.Core.resources.dll", directory);
            CopyFile(@"zh-Hans\Phenix.Business.resources.dll", directory);
        }

        private static void CopyFile(string fileName, string destDirectory)
        {
            try
            {
                string sourcePath = Path.Combine(AppConfig.BaseDirectory, fileName);
                if (!File.Exists(sourcePath))
                    Console.WriteLine(new MessageNotifyEventArgs(MessageNotifyType.Warning, fileName, String.Format("文件 {0} 不存在!", sourcePath)));

                string destPath = Path.Combine(destDirectory, fileName);
                if (File.Exists(destPath))
                    File.Delete(destPath);
                File.Copy(sourcePath, destPath, true);
                Console.WriteLine(new MessageNotifyEventArgs(MessageNotifyType.Information, fileName, "拷贝成功"));
            }
            catch (Exception ex)
            {
                Console.WriteLine(new MessageNotifyEventArgs(MessageNotifyType.Error, fileName, AppUtilities.GetErrorHint(ex)));
            }
        }
    }
}