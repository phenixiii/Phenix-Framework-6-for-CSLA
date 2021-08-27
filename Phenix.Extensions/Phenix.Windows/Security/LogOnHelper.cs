using Phenix.Core.Net;
using Phenix.Core.Security;

namespace Phenix.Windows.Security
{
  /// <summary>
  /// 登录助手
  /// </summary>
  public static class LogOnHelper
  {
    #region 方法

    /// <summary>
    /// 登录
    /// </summary>
    /// <param name="userNumber">工号</param>
    /// <param name="password">口令</param>
    public static DataSecurityContext LogOn(string userNumber, string password)
    {
      Phenix.Core.AppSettings.DefaultKey = userNumber;
      DataSecurityContext context = DataSecurityHub.CheckIn(new UserIdentity(userNumber, password), true);
      if (context != null)
      {
        //配置环境
        Phenix.Core.Win32.NativeMethods.SetClock(context.SystemDate);
        Phenix.Core.Win32.NativeMethods.SetDateTimeFormat();
        if (!NetConfig.ServicesFixed)
          NetConfig.ServicesAddresses = context.Hosts;
        Phenix.Core.AppUtilities.ClearTempDirectory();
      }
      return context;
    }

    /// <summary>
    /// 尝试登录
    /// </summary>
    /// <param name="servicesAddress">主机IP地址</param>
    /// <param name="userNumber">工号</param>
    /// <param name="password">口令</param>
    /// <param name="context">数据安全上下文</param>
    public static bool TryLogOn(string servicesAddress, string userNumber, string password, out DataSecurityContext context)
    {
      if (Phenix.Services.Client.Library.Registration.RegisterWorker(servicesAddress))
      {
        context = LogOn(userNumber, password);
        return context != null && context.Identity != null && context.Identity.IsAuthenticated;
      }
      else
      {
        context = null;
        return false;
      }
    }

    /// <summary>
    /// 登出
    /// </summary>
    public static bool LogOff()
    {
      if (UserIdentity.CurrentIdentity != null)
      {
        UserIdentity.CurrentIdentity.LogOff();
        return true;
      }
      return false;
    }

    /// <summary>
    /// 修改登录口令
    /// </summary>
    /// <param name="servicesAddress">主机IP地址</param>
    /// <param name="newPassword">新登录口令</param>
    /// <param name="identity">用户身份</param>
    public static bool ChangePassword(string servicesAddress, string newPassword, UserIdentity identity)
    {
      if (Phenix.Services.Client.Library.Registration.RegisterWorker(servicesAddress))
        return DataSecurityHub.ChangePassword(newPassword, identity);
      return false;
    }
  }

  #endregion
}
