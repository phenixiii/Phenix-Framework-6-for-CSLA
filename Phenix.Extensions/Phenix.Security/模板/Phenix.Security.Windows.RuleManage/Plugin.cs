using System;
using System.Windows.Forms;
using Phenix.Core.Plugin;
using Phenix.Core.Security;
using Phenix.Core.Windows;

namespace Phenix.Security.Windows.RuleManage
{
  public class Plugin : PluginBase<Plugin>
  {
    private BaseForm _mainForm;

    /// <summary>
    /// 分析消息
    /// 由 PluginHost 调用
    /// </summary>
    /// <param name="message">消息</param>
    /// <returns>按需返回</returns>
    public override object AnalyseMessage(object message)
    {
      BaseForm ownerForm = message as BaseForm;
      if (ownerForm != null && ownerForm.IsMdiContainer)
      {
        _mainForm = BaseForm.ExecuteMdi<RuleManageForm>(ownerForm);
        return _mainForm;
      }
      return BaseForm.ExecuteDialog<RuleManageForm>(message);
    }
  }

  /// <summary>
  /// 可变更程序集输出类型以用于调试
  /// </summary>
  static class Program
  {
    [STAThread]
    static void Main()
    {
      Application.EnableVisualStyles();
      Application.SetCompatibleTextRenderingDefault(false); 

      //设为调试状态
      Phenix.Core.AppConfig.Debugging = true;
      //模拟登陆
      Phenix.Business.Security.UserPrincipal.User =
        Phenix.Business.Security.UserPrincipal.CreateAuthenticated(
          UserIdentity.AdminId, UserIdentity.AdminUserName, UserIdentity.AdminUserNumber, 
          String.Empty, new long?(), new long?());
      Phenix.Services.Client.Library.Registration.RegisterEmbeddedWorker(false);
      //模拟启动插件
      PluginHost.Default.SendSingletonMessage("Phenix.Security.Windows.RuleManage", null);
    }
  }
}