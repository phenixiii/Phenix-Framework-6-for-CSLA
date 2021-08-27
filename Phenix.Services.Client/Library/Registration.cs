using System;
using System.Reflection;
using System.Windows.Forms;
using Phenix.Core;
using Phenix.Core.Cache;
using Phenix.Core.Data;
using Phenix.Core.Dictionary;
using Phenix.Core.Log;
using Phenix.Core.Message;
using Phenix.Core.Net;
using Phenix.Core.Rule;
using Phenix.Core.Security;
using Phenix.Core.Workflow;

namespace Phenix.Services.Client.Library
{
  /// <summary>
  /// 注册器
  /// </summary>
  public static class Registration
  {
    #region 属性
    
    /// <summary>
    /// 已注册
    /// </summary>
    public static bool Registered { get; private set; }

    #endregion

    #region 方法

    /// <summary>
    /// 注册嵌入式实施者
    /// reset = true
    /// </summary>
    public static bool RegisterEmbeddedWorker()
    {
      return RegisterWorker(NetConfig.EMBEDDED_SERVICE, true);
    }
    
    /// <summary>
    /// 注册嵌入式实施者
    /// </summary>
    /// <param name="reset">重新设定</param>
    public static bool RegisterEmbeddedWorker(bool reset)
    {
      return RegisterWorker(NetConfig.EMBEDDED_SERVICE, reset);
    }

    /// <summary>
    /// 注册实施者
    /// reset = true
    /// </summary>
    /// <param name="servicesAddress">主机IP地址</param>
    public static bool RegisterWorker(string servicesAddress)
    {
      return RegisterWorker(servicesAddress, true);
    }

    /// <summary>
    /// 注册实施者
    /// </summary>
    /// <param name="reset">重新设定</param>
    public static bool RegisterWorker(bool reset)
    {
      return RegisterWorker(null, reset);
    }

    /// <summary>
    /// 注册实施者
    /// </summary>
    /// <param name="servicesAddress">主机IP地址</param>
    /// <param name="reset">重新设定</param>
    public static bool RegisterWorker(string servicesAddress, bool reset)
    {
      if (reset || !Registered)
      {
        NetConfig.ServicesAddress = servicesAddress;
        return RegisterWorker();
      }
      return Registered;
    }

    /// <summary>
    /// 注册实施者
    /// </summary>
    public static bool RegisterWorker()
    {
      switch (NetConfig.ProxyType)
      {
        case ProxyType.Embedded:
          Registered = RegisterEmbedded();
          break;
        case ProxyType.Remoting:
          RegisterProxy();
          Registered = true;
          break;
        case ProxyType.Wcf:
          RegisterWcfProxy();
          Registered = true;
          break;
      }
      //配置CSLA
      AppSettings.SaveValue("CslaAutoCloneOnUpdate", Boolean.FalseString, false, true);
      AppSettings.SaveValue("CslaAuthentication", DataSecurityContext.InternalAuthenticationType, false, true);

      return Registered;
    }

    private static string ChangeEmbeddedPath(string path)
    {
      using (OpenFileDialog openFileDialog = new OpenFileDialog())
      {
        openFileDialog.FileName = path;
        openFileDialog.RestoreDirectory = true;
        openFileDialog.Filter = "Phenix.Services.Library Assembly|" + AppConfig.ServicesLibraryName;
        openFileDialog.Title = "Load Assembly";
        if (openFileDialog.ShowDialog() == DialogResult.OK)
          return openFileDialog.FileName;
      }
      return null;
    }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
    private static bool RegisterEmbedded()
    {
      do
      {
        Assembly assembly = null;
        try
        {
          assembly = Assembly.LoadFrom(AppConfig.ServicesLibraryPath);
        }
        catch (Exception ex)
        {
          if (AppConfig.AppType == AppType.Webform)
            throw;
          MessageBox.Show(String.Format("{0}:\n{1}", AppConfig.ServicesLibraryPath, Phenix.Core.AppUtilities.GetErrorHint(ex)), "Fail to load", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        if (assembly == null)
        {
          string s = String.Format("{0} doesn't exist", AppConfig.ServicesLibraryPath);
          if (AppConfig.AppType == AppType.Webform)
            throw new InvalidOperationException(s);
          MessageBox.Show(s, "Fail to load", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        else
        {
          Type type = assembly.GetType("Phenix.Services.Library.Registration", false);
          if (type == null)
          {
            string s = String.Format("{0} illegal", AppConfig.ServicesLibraryPath);
            if (AppConfig.AppType == AppType.Webform)
              throw new InvalidOperationException(s);
            MessageBox.Show(s, "Fail to load", MessageBoxButtons.OK, MessageBoxIcon.Error);
          }
          else if (!AppConfig.AutoMode &&
            (AppConfig.AppType == AppType.Winform && !AppConfig.DesignDebugMode && !DbConnectionInfo.Fetch().Setup() ||
              (AppConfig.AppType != AppType.Winform || AppConfig.DesignDebugMode) && !DbConnectionInfo.Fetch().IsValid(true)))
          {
            DbConnectionInfo.Fetch().Clear();
            if (AppConfig.AppType == AppType.Webform)
              throw new InvalidOperationException(Phenix.Core.Properties.Resources.DatabaseConnectionFailed);
            return false;
          }
          else
          {
            type.InvokeMember("RegisterWorker", BindingFlags.InvokeMethod | BindingFlags.Public | BindingFlags.Static, null, null, null);
            //配置CSLA
            //AppSettings.SaveValue("CslaDataPortalProxy", typeof(Phenix.Services.Client.Library.DataPortalProxy).AssemblyQualifiedName, false, true);
            Csla.ApplicationContext.DataPortalProxy = typeof(Phenix.Services.Client.Library.DataPortalProxy).AssemblyQualifiedName;
            return true;
          }
        }
        string path = ChangeEmbeddedPath(AppConfig.ServicesLibraryPath);
        if (path == null)
          return false;
        AppConfig.ServicesLibraryPath = path;
      } while (true);
    }

    private static void RegisterProxy()
    {
      DataDictionaryHub.Worker = new Phenix.Services.Client.Library.DataDictionaryProxy();
      DataSecurityHub.Worker = new Phenix.Services.Client.Library.DataSecurityProxy();
      DataHub.Worker = new Phenix.Services.Client.Library.DataProxy();
      DataRuleHub.Worker = new Phenix.Services.Client.Library.DataRuleProxy();
      PermanentLogHub.Worker = new Phenix.Services.Client.Library.PermanentLogProxy();
      ObjectCache.Worker = new Phenix.Services.Client.Library.ObjectCacheSynchroProxy();
      WorkflowHub.Worker = new Phenix.Services.Client.Library.WorkflowProxy();
      MessageHub.Worker = new Phenix.Services.Client.Library.MessageProxy();
      //配置CSLA
      //AppSettings.SaveValue("CslaDataPortalProxy", typeof(Phenix.Services.Client.Library.DataPortalProxy).AssemblyQualifiedName, false, true);
      Csla.ApplicationContext.DataPortalProxy = typeof(Phenix.Services.Client.Library.DataPortalProxy).AssemblyQualifiedName;
    }

    private static void RegisterWcfProxy()
    {
      DataDictionaryHub.Worker = new Phenix.Services.Client.Library.Wcf.DataDictionaryProxy();
      DataSecurityHub.Worker = new Phenix.Services.Client.Library.Wcf.DataSecurityProxy();
      DataHub.Worker = new Phenix.Services.Client.Library.Wcf.DataProxy();
      DataRuleHub.Worker = new Phenix.Services.Client.Library.Wcf.DataRuleProxy();
      PermanentLogHub.Worker = new Phenix.Services.Client.Library.Wcf.PermanentLogProxy();
      ObjectCache.Worker = new Phenix.Services.Client.Library.Wcf.ObjectCacheSynchroProxy();
      WorkflowHub.Worker = new Phenix.Services.Client.Library.Wcf.WorkflowProxy();
      MessageHub.Worker = new Phenix.Services.Client.Library.Wcf.MessageProxy();
      //配置CSLA
      //AppSettings.SaveValue("CslaDataPortalProxy", typeof(Phenix.Services.Client.Library.Wcf.DataPortalProxy).AssemblyQualifiedName, false, true);
      Csla.ApplicationContext.DataPortalProxy = typeof(Phenix.Services.Client.Library.Wcf.DataPortalProxy).AssemblyQualifiedName;
    }

    /// <summary>
    /// 测试连接速度
    /// </summary>
    /// <param name="servicesAddress">主机IP地址</param>
    /// <returns>历时毫米数</returns>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1804:RemoveUnusedLocals", MessageId = "l")]
    public static double TestServicesSpeed(string servicesAddress)
    {
      if (RegisterWorker(servicesAddress, true))
      {
        DateTime dt = DateTime.Now;
        long l = DataHub.SequenceValue;
        return DateTime.Now.Subtract(dt).TotalMilliseconds;
      }
      return -1;
    }


    #endregion
  }
}