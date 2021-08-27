using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Phenix.Test.使用指南._21._7
{
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            Console.WriteLine("请通过检索“//*”了解代码中的特殊处理");
            Console.WriteLine("请事先编译Phenix.Test.使用指南.21.7.Business工程输出DLL到Bin.Top目录");
            Console.WriteLine("然后启动Bin.Top目录下的Phenix.Services.Host.x86/x64.exe程序");
            Console.WriteLine("通过Host程序注册Phenix.Test.使用指南.21.7.Plugin插件");
            Console.WriteLine("请不要关闭Host程序");
            Console.WriteLine("如需观察日志（被保存在TempDirectory子目录里）可将Host的Debugging功能菜单点亮");
            Console.Write("准备好后请点回车继续：");
            Console.ReadLine();

            Console.WriteLine("**** 测试公开访问 AssemblyController WabAPI服务 ****");
            Task[] tasks = new[]
            {
                Task.Run(() => Call(1)),
                Task.Run(() => Call(2)),
                Task.Run(() => Call(3)),
                Task.Run(() => Call(4)),
                Task.Run(() => Call(5)),
                Task.Run(() => Call(6)),
                Task.Run(() => Call(7)),
                Task.Run(() => Call(8)),
                Task.Run(() => Call(9)),
                Task.Run(() => Call(10)),
            };
            Task.WaitAll(tasks);

            Console.WriteLine("结束, 与数据库交互细节见日志");
            Console.ReadLine();
        }

        private static void Call(int index)
        {
            for (int i = 0; i < 10000000; i++)
            {
                using (HttpClient client = new HttpClient())
                {
                    client.BaseAddress = new Uri("http://127.0.0.1:8080");
                    using (HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, "/api/Assembly/?Name=Phenix.Test."))
                    using (HttpResponseMessage response = client.SendAsync(request).Result)
                    {
                        string result = response.Content.ReadAsStringAsync().Result;
                        if (response.StatusCode != HttpStatusCode.OK)
                            throw new HttpRequestException(result);
                        IList<LocalAssembly> assemblyClassList = (IList<LocalAssembly>) JsonConvert.DeserializeObject(result, typeof(IList<LocalAssembly>));
                        Console.WriteLine("返回对象数量=" + assemblyClassList.Count + ' ' + (assemblyClassList.Count > 0 ? "ok" : "error"));
                    }

                    Console.WriteLine("{0}:{1}", index, i);
                }

                Thread.Sleep(1000);
            }
        }
    }
}
