using System;
using System.Windows.Forms;
using Phenix.Core;
using Phenix.Services.Host.Core;

namespace Phenix.Services.Host
{
  static class Program
  {
    /// <summary>
    /// 应用程序的主入口点。
    /// </summary>
    [STAThread]
    static void Main(string[] args)
    {
      //System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("zh-CN");
      //System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("zh-CN");

      AppConfig.Debugging = false;
      AppConfig.AutoMode = true;

      if (args.Length == 1)
      {
        UpgradeState upgradeState;
        if (Enum.TryParse(args[0], false, out upgradeState))
          ServiceManager.UpgradeState = upgradeState;
      }

      Phenix.Core.Win32.NativeMethods.SetDateTimeFormat();
      
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