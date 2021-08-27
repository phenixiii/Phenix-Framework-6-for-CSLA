/*
 * 监控服务程序
 * 传入参数为被监控的服务程序进程名
 * 
 * 服务程序需设计成：
 * 1，定时1秒设置主窗体的 Text = 原标题 + AppConfig.ROW_SEPARATOR + DateTime.Now.ToString("G", DateTimeFormatInfo.InvariantInfo);
 * 2，在主窗体FormClosing事件里将 Text = 原标题
 */

using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.IO;
using System.Threading;
using Phenix.Core;
using Phenix.Core.Net;

namespace Phenix.Services.Host.Monitor
{
  static class Program
  {
    /// <summary>
    /// 应用程序的主入口点。
    /// </summary>
    [SuppressMessage("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
    static void Main(string[] args)
    {
      string processName = "Phenix.Services.Host.x64";
      int interval = 3;
      int intervalAlert = 2;
      int workingSetAlert = 2 * 1024;
      int workingProcessorAlert = 50;
      if (args != null && args.Length > 0)
        if (!int.TryParse(args[0], out interval))
        {
          processName = args[0];
          if (args.Length >= 2)
            int.TryParse(args[1], out interval);
          if (args.Length >= 3)
            int.TryParse(args[2], out intervalAlert);
          if (args.Length >= 4)
            int.TryParse(args[3], out workingSetAlert);
          if (args.Length >= 5)
            int.TryParse(args[4], out workingProcessorAlert);
        }
      if (interval < 3)
        interval = 3;
      if (intervalAlert < 2)
        intervalAlert = 2;
      if (workingSetAlert > 16 * 1024)
        workingSetAlert = 16 * 1024;
      if (workingProcessorAlert > 100)
        workingProcessorAlert = 100;

      if (String.Compare(processName, "help", StringComparison.OrdinalIgnoreCase) == 0)
      {
        Console.WriteLine(@" Phenix.Services.Host.Monitor [Phenix.Services.Host.XXX program name] [interval(>= 3second)] [intervalAlert(>= 2second)] [maxWorkingSetSize(<= 16 * 1024M)] [maxWorkingProcessorProportion(<= 100%)]");
        Console.WriteLine(@" Phenix.Services.Host.Monitor [Phenix.Services.Host.XXX program name] [interval(>= 3second)] [intervalAlert(>= 2second)] [maxWorkingSetSize(<= 16 * 1024M)]");
        Console.WriteLine(@" Phenix.Services.Host.Monitor [Phenix.Services.Host.XXX program name] [interval(>= 3second)] [intervalAlert(>= 2second)]");
        Console.WriteLine(@" Phenix.Services.Host.Monitor [Phenix.Services.Host.XXX program name] [interval(>= 3second)]");
        Console.WriteLine(@" Phenix.Services.Host.Monitor [Phenix.Services.Host.XXX program name]");
        Console.WriteLine(@" Phenix.Services.Host.Monitor [interval(>= 3second)]");
        return;
      }

      Console.WriteLine(@"processName = {0}", processName);
      Console.WriteLine(@"interval = {0}s", interval);
      Console.WriteLine(@"intervalAlert = {0}s", intervalAlert);
      Console.WriteLine(@"workingSetAlert = {0}M", workingSetAlert);
      Console.WriteLine(@"workingProcessorAlert = {0}%", workingProcessorAlert);
      Console.WriteLine();

      bool needStart = true;
      //开始循环
      do
      {
        try
        {
          Process oldProcess = null;
          Process[] processes = Process.GetProcessesByName(processName);
          foreach (Process item in processes)
          {
            try
            {
              if (item.HasExited)
                break;
            }
            catch (InvalidOperationException ex)
            {
              Console.WriteLine(DateTime.Now + " " + processName + " HasExited error: " + ex.ToString());
              Phenix.Core.Log.EventLog.SaveLocal(processName + " HasExited error", ex);
            }
            catch (Win32Exception ex)
            {
              Console.WriteLine(DateTime.Now + " " + processName + " HasExited error: " + ex.ToString());
              Phenix.Core.Log.EventLog.SaveLocal(processName + " HasExited error", ex);
            }
            catch (NotSupportedException ex)
            {
              Console.WriteLine(DateTime.Now + " " + processName + " HasExited error: " + ex.ToString());
              Phenix.Core.Log.EventLog.SaveLocal(processName + " HasExited error", ex);
            }

            if (DateTime.Now.Second % 3 == 0)
              ShowPerformanceCounter(processName, workingSetAlert, workingProcessorAlert, false);

            string s = null;
            try
            {
              s = item.MainWindowTitle;
            }
            catch (InvalidOperationException ex)
            {
              Console.WriteLine(DateTime.Now + " " + processName + " MainWindowTitle error: " + ex.ToString());
              Phenix.Core.Log.EventLog.SaveLocal(processName + " MainWindowTitle error", ex);
            }
            catch (NotSupportedException ex)
            {
              Console.WriteLine(DateTime.Now + " " + processName + " MainWindowTitle error: " + ex.ToString());
              Phenix.Core.Log.EventLog.SaveLocal(processName + " MainWindowTitle error", ex);
            }
            if (s == null)
            {
              needStart = true;
              oldProcess = item;
              Console.WriteLine(DateTime.Now + " " + processName + " MainWindowTitle is null");
              break;
            }
            int i = s.IndexOf(AppConfig.ROW_SEPARATOR);
            if (i == -1)
            {
              needStart = false;
              break;
            }
            try
            {
              s = s.Substring(i + 1);
              DateTime d = DateTime.ParseExact(s, "G", DateTimeFormatInfo.InvariantInfo);
              needStart = d.AddSeconds(interval) < DateTime.Now;
              if (needStart)
              {
                oldProcess = item;
                ShowPerformanceCounter(processName, workingSetAlert, workingProcessorAlert, true);
                Console.WriteLine(DateTime.Now + " " + processName + " stuck in " + String.Format("{0:N}s", DateTime.Now.Subtract(d).Seconds));
              }
              else if (d.AddSeconds(intervalAlert) < DateTime.Now)
              {
                ShowPerformanceCounter(processName, workingSetAlert, workingProcessorAlert, true);
                Console.WriteLine(DateTime.Now + " " + processName + " stuck in " + String.Format("{0:N}s", DateTime.Now.Subtract(d).Seconds));
              }
              break;
            }
            catch (FormatException ex)
            {
              Console.WriteLine(DateTime.Now + " " + s + " DateTime.ParseExact error: " + ex.ToString());
              Phenix.Core.Log.EventLog.SaveLocal(s + " DateTime.ParseExact error", ex);
            }
          }

          if (processes.Length == 0 || needStart)
          {
            if (oldProcess != null)
              try
              {
                Console.WriteLine(DateTime.Now + " " + "Kill: " + processName);
                Phenix.Core.Log.EventLog.SaveLocal("Kill: " + processName);
                //oldProcess.Kill();
                //oldProcess.WaitForExit(1000);
                Phenix.Core.Win32.NativeMethods.TerminateProcess(oldProcess);
              }
              catch (SystemException ex)
              {
                Console.WriteLine(DateTime.Now + " " + processName + " Kill error: " + ex.ToString());
                Phenix.Core.Log.EventLog.SaveLocal(processName + " Kill error", ex);
              }

            string path = Path.Combine(Path.GetDirectoryName(AppConfig.BaseDirectory), processName + ".exe");
            try
            {
              Console.WriteLine(DateTime.Now + " " + "Start: " + path);
              Phenix.Core.Log.EventLog.SaveLocal("Start: " + path);
              Process.Start(path).WaitForInputIdle();
              needStart = false;
            }
            catch (Win32Exception ex)
            {
              Console.WriteLine(DateTime.Now + " " + path + " Start error: " + ex.ToString());
              Phenix.Core.Log.EventLog.SaveLocal(path + " Start error", ex);
            }
            catch (System.IO.FileNotFoundException ex)
            {
              Console.WriteLine(DateTime.Now + " " + path + " Start error: " + ex.ToString());
              Phenix.Core.Log.EventLog.SaveLocal(path + " Start error", ex);
            }
            catch (ObjectDisposedException ex)
            {
              Console.WriteLine(DateTime.Now + " " + path + " Start error: " + ex.ToString());
              Phenix.Core.Log.EventLog.SaveLocal(path + " Start error", ex);
            }
          }

          Thread.Sleep(1000);
        }
        catch (Exception ex)
        {
          Console.WriteLine(DateTime.Now + " " + "Error: " + ex.ToString());
          Phenix.Core.Log.EventLog.SaveLocal("Error", ex);
          Thread.Sleep(NetConfig.TcpTimedWaitDelay);
        }
      } while (true);
    }

    private static void ShowPerformanceCounter(string processName, int workingSetAlert, int processorTimeAlert, bool must)
    {
      PerformanceCounter workingSetCounter = new PerformanceCounter("Process", "Working Set", processName);
      float workingSet = workingSetCounter.NextValue() / 1024 / 1024;
      if (must || workingSet >= workingSetAlert)
        Console.WriteLine(DateTime.Now + " " + processName + " Working Set " + String.Format("{0:N}M", workingSet));

      PerformanceCounter processorTimeCounter = new PerformanceCounter("Process", "% Processor Time", processName);
      float processorTime = processorTimeCounter.NextValue() / Environment.ProcessorCount;
      if (must || processorTime >= processorTimeAlert)
        Console.WriteLine(DateTime.Now + " " + processName + " Processor Time " + String.Format("{0:N}%", processorTime));
    }
  }
}
