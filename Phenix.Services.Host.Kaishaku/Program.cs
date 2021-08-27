/*
 * 服务介错程序
 * 传入参数为被介错的服务程序进程名
 * 
 * 只要启动，就会定时1秒杀进程，没有进程才结束程序
 */

using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Threading;

namespace Phenix.Services.Host.Kaishaku
{
  class Program
  {
    /// <summary>
    /// 应用程序的主入口点。
    /// </summary>
    [SuppressMessage("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
    static void Main(string[] args)
    {
      if (args == null || args.Length == 0)
        return;
      
      string processName = args[0];

      //开始循环
      do
      {
        try
        {
          Process[] processes = Process.GetProcessesByName(processName);
          if (processes.Length == 0)
            break;

          foreach (Process item in processes)
          {
            try
            {
              if (item.HasExited)
                break;
            }
            catch (InvalidOperationException ex)
            {
              Console.WriteLine(processName + " HasExited error: " + ex.ToString());
            }
            catch (Win32Exception ex)
            {
              Console.WriteLine(processName + " HasExited error: " + ex.ToString());
            }
            catch (NotSupportedException ex)
            {
              Console.WriteLine(processName + " HasExited error: " + ex.ToString());
            }
              try
              {
                Console.WriteLine("Kill: " + processName);
                //item.Kill();
                //item.WaitForExit(1000);
                Phenix.Core.Win32.NativeMethods.TerminateProcess(item);
              }
              catch (SystemException ex)
              {
                Console.WriteLine(processName + " Kill error: " + ex.ToString());
              }
          }

          Thread.Sleep(1000);
        }
        catch (Exception ex)
        {
          Console.WriteLine("Error: " + ex.ToString());
          Thread.Sleep(1000 * 3);
        }
      } while (true);
    }
  }
}
