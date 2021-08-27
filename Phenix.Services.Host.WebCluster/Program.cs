using System;
using System.Windows.Forms;

namespace Phenix.Services.Host.WebCluster
{
  static class Program
  {
    /// <summary>
    /// 应用程序的主入口点。
    /// </summary>
    [STAThread]
    static void Main()
    {
      AppDomain.CurrentDomain.UnhandledException += (sender, eventArgs) =>
      {
        Phenix.Core.Log.EventLog.SaveLocal("current domain unhandled exception", (Exception)eventArgs.ExceptionObject);
      };

      Application.EnableVisualStyles();
      Application.SetCompatibleTextRenderingDefault(false);
      Application.Run(new MainForm());
    }
  }
}