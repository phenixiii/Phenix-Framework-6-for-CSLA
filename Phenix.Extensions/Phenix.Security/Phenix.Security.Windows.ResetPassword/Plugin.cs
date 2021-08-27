using Phenix.Core.Plugin;

namespace Phenix.Security.Windows.ResetPassword
{
  public class Plugin : PluginBase<Plugin>
  {
    /// <summary>
    /// 分析消息
    /// 由 PluginHost 调用
    /// </summary>
    /// <param name="message">消息</param>
    /// <returns>按需返回</returns>
    public override object AnalyseMessage(object message)
    {
      return Phenix.Services.Client.Security.ChangePasswordDialog.Execute();
    }
  }
}