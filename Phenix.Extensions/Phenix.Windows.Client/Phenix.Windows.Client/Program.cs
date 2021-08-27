using System;
using System.Windows.Forms;
using Phenix.Core;
using Phenix.Core.Net;
using Phenix.Core.Plugin;
using Phenix.Core.Security;
using Phenix.Services.Client.Security;

namespace Phenix.Windows.Client
{
  static class Program
  {
    /// <summary>
    /// 应用程序的主入口点。
    /// </summary>
    [STAThread]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Reliability", "CA2000:DisposeObjectsBeforeLosingScope")]
    static void Main(string[] args)
    {
      //System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("zh-CN");
      //System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("zh-CN");

      Application.EnableVisualStyles();
      Application.SetCompatibleTextRenderingDefault(false);

      //if (AppUtilities.ProcessCount() > 1 && args.Length == 0)
      //{
      //  MessageBox.Show("该程序已存在，请查看系统任务栏或选择其他系统用户查看！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
      //  return;
      //}

      ////设置服务协议
      //Phenix.Core.Net.NetConfig.ProxyType = ProxyType.Wcf;
      //Phenix.Core.Net.WcfConfig.ServicesProtocolType = WcfProtocolType.BasicHttp;
      Phenix.Core.Net.NetConfig.ServicesFixed = true; //可由操作者自行维护服务IP清单

      //登录
      using (LogOn logOn = new LogOn())
      {
        logOn.Title = "登录XXX系统"; //title可更换
        //logOn.Logo = System.Drawing.Image.FromFile("Phenix.logo.jpg"); //logo可更换
        //logOn.UpgradeProxyType = Phenix.Core.Net.NetConfig.ProxyType; //升级文件的代理类型
        //logOn.UpgradeServicesAddress = "127.0.0.1"; //升级文件的服务地址

        logOn.UpgradeFileFilters.Add("*.dll");
        logOn.UpgradeFileFilters.Add("*.wav");
        logOn.UpgradeFileFilters.Add("*.xls");
        logOn.UpgradeFileFilters.Add("*.xml");
        logOn.UpgradeFileFilters.Add("*.msi");

        IPrincipal user = logOn.Execute<LogOnDialog>();
        if (user != null && user.Identity.IsAuthenticated)
        {
          //Phenix.Core.AppConfig.AutoStoreLayout = true; //允许自动保存界面Layout
          //Phenix.Core.Net.NetConfig.RegisterServicesCluster("本地服务", "127.0.0.1"); //可注册服务集群
          Phenix.TeamHub.Prober.Business.Worker.Register(); //注册Phenix.TeamHub.Prober.AppHub的功能实现
          //启动主Form
          PluginHost.Default.SendSingletonMessage("Phenix.Windows.Main", null);
        }
      }
    }
  }
}
