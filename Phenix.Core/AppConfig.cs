using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Security.Permissions;
using Microsoft.Win32;
using Phenix.Core.Net;
using Phenix.Core.Security;

namespace Phenix.Core
{
  /// <summary>
  /// 应用系统配置信息
  /// </summary>
  public static class AppConfig
  {
    #region 属性

    /// <summary>
    /// 系统名
    /// </summary>
    public const string SYSTEM_NAME = "Phenix .NET Framework";

    /// <summary>
    /// 客户端程序集子目录名称
    /// </summary>
    public const string CLIENT_LIBRARY_SUBDIRECTORY_NAME = "ClientLibrary";
    private static readonly string _defaultClientLibrarySubdirectory = Path.Combine(BaseDirectory, CLIENT_LIBRARY_SUBDIRECTORY_NAME + "\\");
    private static string _clientLibrarySubdirectory;
    /// <summary>
    /// 客户端程序集子目录
    /// </summary>
    public static string ClientLibrarySubdirectory
    {
      get
      {
        AppSettings.GetProperty(ref _clientLibrarySubdirectory, _defaultClientLibrarySubdirectory);
        if (!Directory.Exists(_clientLibrarySubdirectory))
          Directory.CreateDirectory(_clientLibrarySubdirectory);
        return _clientLibrarySubdirectory;
      }
      set { AppSettings.SetProperty(ref _clientLibrarySubdirectory, value); }
    }

    /// <summary>
    /// 私有客户端程序集子目录名称
    /// </summary>
    public const string CLIENT_LIBRARY_OWN_SUBDIRECTORY_NAME = "own";
    private static string _clientLibraryOwnSubdirectory;
    /// <summary>
    /// 私有客户端程序集子目录
    /// </summary>
    public static string ClientLibraryOwnSubdirectory
    {
      get
      {
        if (_clientLibraryOwnSubdirectory == null)
          _clientLibraryOwnSubdirectory = Path.Combine(ClientLibrarySubdirectory, CLIENT_LIBRARY_OWN_SUBDIRECTORY_NAME);
        if (!Directory.Exists(_clientLibraryOwnSubdirectory))
          Directory.CreateDirectory(_clientLibraryOwnSubdirectory);
        return _clientLibraryOwnSubdirectory;
      }
    }

    /// <summary>
    /// 服务端程序集子目录名称
    /// </summary>
    public const string SERVICE_LIBRARY_SUBDIRECTORY_NAME = "ServiceLibrary";
    private static readonly string _defaultServiceLibrarySubdirectory = Path.Combine(BaseDirectory, SERVICE_LIBRARY_SUBDIRECTORY_NAME + "\\");
    private static string _serviceLibrarySubdirectory;
    /// <summary>
    /// 服务端程序集子目录
    /// </summary>
    public static string ServiceLibrarySubdirectory
    {
      get
      {
        AppSettings.GetProperty(ref _serviceLibrarySubdirectory, _defaultServiceLibrarySubdirectory);
        if (!Directory.Exists(_serviceLibrarySubdirectory))
          Directory.CreateDirectory(_serviceLibrarySubdirectory);
        return _serviceLibrarySubdirectory;
      }
      set { AppSettings.SetProperty(ref _serviceLibrarySubdirectory, value); }
    }

    /// <summary>
    /// 备份子目录
    /// </summary>
    public const string BACKUP_SUBDIRECTORY = "Backup";

    /// <summary>
    /// 换行字串
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly")]
    public const string CR_LF = "\r\n";

    /// <summary>
    /// 分隔字串
    /// </summary>
    public const string SEPARATOR = " #-*-# ";

    /// <summary>
    /// 块分隔符
    /// </summary>
    public const char BLOCK_SEPARATOR = '\u0017';

    /// <summary>
    /// 字段分隔符
    /// </summary>
    public const char FIELD_SEPARATOR = '\u0003';

    /// <summary>
    /// 行分隔符
    /// </summary>
    public const char ROW_SEPARATOR = '\u0004';

    /// <summary>
    /// 参数分隔符
    /// </summary>
    public const char PARAM_SEPARATOR = '_';

    /// <summary>
    /// 值分隔符
    /// </summary>
    public const char VALUE_SEPARATOR = ',';

    /// <summary>
    /// 标点分隔符
    /// </summary>
    public const char POINT_SEPARATOR = '.';

    /// <summary>
    /// 隶属分隔符
    /// </summary>
    public const char EQUAL_SEPARATOR = '=';

    /// <summary>
    /// 未知值
    /// </summary>
    public const string UNKNOWN_VALUE = "*";

    /// <summary>
    /// 企业名
    /// </summary>
    public static string EnterpriseName { get; set; }

    private const string ENVIRONMENT_DIRECTORY_NAME = "EnvironmentDirectory";

    private static Dictionary<string, int> _designPaths;
    /// <summary>
    /// 设计器路径
    /// </summary>
    public static IDictionary<string, int> DesignPaths
    {
      get
      {
        if (_designPaths == null)
        {
          Dictionary<string, int> result = new Dictionary<string, int>(8, StringComparer.OrdinalIgnoreCase);
          string path = FindDesignPathInRegistryCurrentUser(@"Software\Microsoft\VisualStudio\10.0_Config\Setup\VS"); //Win7x64
          if (!String.IsNullOrEmpty(path))
            result[path] = 10; //Visual Studio 2010
          path = FindDesignPathInRegistryCurrentUser(@"Software\Microsoft\VisualStudio\11.0_Config\Setup\VS"); //Win7x64
          if (!String.IsNullOrEmpty(path))
            result[path] = 11; //Visual Studio 2012
          path = FindDesignPathInRegistryCurrentUser(@"Software\Microsoft\VisualStudio\12.0_Config\Setup\VS"); //Win7x64
          if (!String.IsNullOrEmpty(path))
            result[path] = 12; //Visual Studio 2013
          path = FindDesignPathInRegistryLocalMachine(@"SOFTWARE\Microsoft\VisualStudio\10.0\Setup\VS");  //Win7x86
          if (!String.IsNullOrEmpty(path))
            result[path] = 10;
          path = FindDesignPathInRegistryLocalMachine(@"SOFTWARE\Microsoft\VisualStudio\11.0\Setup\VS"); //Win7x86
          if (!String.IsNullOrEmpty(path))
            result[path] = 11;
          path = FindDesignPathInRegistryLocalMachine(@"SOFTWARE\Microsoft\VisualStudio\12.0\Setup\VS"); //Win7x86
          if (!String.IsNullOrEmpty(path))
            result[path] = 12;
          path = FindDesignPathInRegistryUsers(@".DEFAULT\Software\Microsoft\VisualStudio\10.0_Config\Setup\VS"); //Win7x64
          if (!String.IsNullOrEmpty(path))
            result[path] = 10;
          path = FindDesignPathInRegistryUsers(@".DEFAULT\Software\Microsoft\VisualStudio\11.0_Config\Setup\VS"); //Win7x64
          if (!String.IsNullOrEmpty(path))
            result[path] = 11;
          path = FindDesignPathInRegistryUsers(@".DEFAULT\Software\Microsoft\VisualStudio\12.0_Config\Setup\VS"); //Win7x64
          if (!String.IsNullOrEmpty(path))
            result[path] = 12;

          path = FindDesignPathInRegistryClassesRoot("{27E23E49-0BAD-437C-9F4A-F1F8682535CA}");
          if (!String.IsNullOrEmpty(path))
            result[path] = 15;  //Visual Studio 2017
          path = FindDesignPathInRegistryClassesRoot("{57105B1F-A5B8-482A-880A-746AF3B07F4C}");
          if (!String.IsNullOrEmpty(path))
            result[path] = 16;  //Visual Studio 2019
          _designPaths = result;
        }
        return _designPaths;
      }
    }

    private static bool? _designMode;
    /// <summary>
    /// 处于设计模式
    /// </summary>
    public static bool DesignMode
    {
      [EnvironmentPermission(SecurityAction.Demand, Unrestricted = true)]
      get
      {
        if (!_designMode.HasValue)
          _designMode = LicenseManager.UsageMode == LicenseUsageMode.Designtime ||
            Process.GetCurrentProcess().ProcessName == "devenv";
        return _designMode.Value;
      }
    }

    /// <summary>
    /// 处于设计调试模式
    /// </summary>
    public static bool DesignDebugMode
    {
      get { return Debugger.IsAttached; }
    }

    private static bool? _autoMode;
    /// <summary>
    /// 处于容器模式
    /// 缺省为 false
    /// </summary>
    [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
    public static bool AutoMode
    {
      get
      {
        if (!_autoMode.HasValue)
          _autoMode = AppType == AppType.Webform;
        return _autoMode.Value;
      }
      set { _autoMode = value; }
    }

    /// <summary>
    /// 调试中?
    /// 缺省为 false
    /// </summary>
    public static bool Debugging { get; set; }

    private static string _baseDirectory;
    /// <summary>
    /// 基础目录
    /// </summary>
    public static string BaseDirectory
    {
      get
      {
        if (_baseDirectory == null)
          _baseDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().CodeBase.Substring(8).Replace("/", "\\")) + "\\"; //"file:///D:/Phenix.NET4/Bin/Phenix.Core.DLL" -> "D:\Phenix.NET4\Bin"
        return _baseDirectory;
      }
    }

    /// <summary>
    /// ServicesLibrary名称
    /// </summary>
    public const string ServicesLibraryName = "Phenix.Services.Library.dll";

    private static string _servicesLibraryPath;
    /// <summary>
    /// ServicesLibrary路径
    /// </summary>
    public static string ServicesLibraryPath
    {
      get
      {
        if (_servicesLibraryPath == null)
        {
          string result = Path.Combine(BaseDirectory, ServicesLibraryName);
          if (!File.Exists(result))
          {
            string directoryName = Environment.GetEnvironmentVariable(DataSecurityContext.InternalAuthenticationType, EnvironmentVariableTarget.User);
            if (directoryName != null)
              result = Path.Combine(directoryName, ServicesLibraryName);
          }
          _servicesLibraryPath = result;
        }
        return _servicesLibraryPath;
      }
      set
      {
        if (value == null)
          return;
        if (AppType != AppType.Webform)
          Environment.SetEnvironmentVariable(DataSecurityContext.InternalAuthenticationType, Path.GetDirectoryName(value), EnvironmentVariableTarget.User);
        _servicesLibraryPath = value;
      }
    }

    private static string _cacheDirectory;
    /// <summary>
    /// 缓存目录
    /// </summary>
    public static string CacheDirectory
    {
      get
      {
        if (_cacheDirectory == null)
          _cacheDirectory = Path.Combine(AppConfig.BaseDirectory,
            String.Format("Cache_{0}", Assembly.GetExecutingAssembly().GetName().Version.ToString().Replace('.', '_')));
        if (!Directory.Exists(_cacheDirectory))
          Directory.CreateDirectory(_cacheDirectory);
        return _cacheDirectory;
      }
    }

    private static string _tempDirectory;
    /// <summary>
    /// 临时目录
    /// </summary>
    public static string TempDirectory
    {
      get
      {
        if (_tempDirectory == null)
          _tempDirectory = Path.Combine(AppType == AppType.Webform ? Path.GetTempPath() : BaseDirectory, 
            String.Format("Temp_{0}", Assembly.GetExecutingAssembly().GetName().Version.ToString().Replace('.', '_')));
        if (!Directory.Exists(_tempDirectory))
          Directory.CreateDirectory(_tempDirectory);
        return _tempDirectory;
      }
    }

    private static bool? _autoStoreLayout;
    /// <summary>
    /// 是否自动保存和恢复界面布局?
    /// 缺省为 true
    /// </summary>
    public static bool AutoStoreLayout
    {
      get
      {
        if (!_autoStoreLayout.HasValue && (Debugging || !AutoMode && NetConfig.ProxyType == ProxyType.Embedded))
          return false;
        return AppSettings.GetProperty(ref _autoStoreLayout, true);
      }
      set { AppSettings.SetProperty(ref _autoStoreLayout, value); }
    }

    private static AppType _appType = AppType.Unknown;
    /// <summary>
    /// 应用类型
    /// </summary>
    public static AppType AppType
    {
      [EnvironmentPermission(SecurityAction.Demand, Unrestricted = true)]
      get
      {
        if (_appType == AppType.Unknown)
        {
          string processName = Process.GetCurrentProcess().ProcessName.ToLower();
          if (processName.Contains(".webserver") || processName.Contains("w3wp") || processName.Contains("aspnet_wp") || processName.Contains("iisexpress"))
            _appType = AppType.Webform;
          else
            _appType = AppType.Winform;
        }
        return _appType;
      }
    }

    private static string _yearMonthPattern = "yyyy-MM";
    /// <summary>
    /// 短日期值的格式模式
    /// 该模式与“d”格式模式关联
    /// </summary>
    public static string YearMonthPattern
    {
      get { return _yearMonthPattern; }
      set { _yearMonthPattern = value; }
    }

    private static string _shortDatePattern = "yyyy-MM-dd";
    /// <summary>
    /// 短日期值的格式模式
    /// 该模式与“d”格式模式关联
    /// </summary>
    public static string ShortDatePattern
    {
      get { return _shortDatePattern; }
      set { _shortDatePattern = value; }
    }

    private static string _fullDateTimePattern = "yyyy-MM-dd HH:mm:ss";
    /// <summary>
    /// 短日期值的格式模式
    /// 该模式与“d”格式模式关联
    /// </summary>
    public static string FullDateTimePattern
    {
      get { return _fullDateTimePattern; }
      set { _fullDateTimePattern = value; }
    }

    private static string _longTimePattern = "HH:mm:ss";
    /// <summary>
    /// 长时间值的格式模式
    /// 该模式与“T”格式模式关联
    /// </summary>
    public static string LongTimePattern
    {
      get { return _longTimePattern; }
      set { _longTimePattern = value; }
    }

    #endregion

    #region 方法

    private static string FindDesignPathInRegistryCurrentUser(string key)
    {
      RegistryKey registryKey = Registry.CurrentUser.OpenSubKey(key);
      if (registryKey != null)
        try
        {
          object registryValue = registryKey.GetValue(ENVIRONMENT_DIRECTORY_NAME);
          if (registryValue != null)
            return registryValue.ToString();
        }
        finally
        {
          registryKey.Close();
        }
      return null;
    }

    private static string FindDesignPathInRegistryLocalMachine(string key)
    {
      RegistryKey registryKey = Registry.LocalMachine.OpenSubKey(key);
      if (registryKey != null)
        try
        {
          object registryValue = registryKey.GetValue(ENVIRONMENT_DIRECTORY_NAME);
          if (registryValue != null)
            return registryValue.ToString();
        }
        finally
        {
          registryKey.Close();
        }
      return null;
    }

    private static string FindDesignPathInRegistryUsers(string key)
    {
      RegistryKey registryKey = Registry.Users.OpenSubKey(key);
      if (registryKey != null)
        try
        {
          object registryValue = registryKey.GetValue(ENVIRONMENT_DIRECTORY_NAME);
          if (registryValue != null)
            return registryValue.ToString();
        }
        finally
        {
          registryKey.Close();
        }
      return null;
    }

    private static string FindDesignPathInRegistryClassesRoot(string key)
    {
      RegistryKey registryKey = Registry.ClassesRoot.OpenSubKey(String.Format(@"Wow6432Node\CLSID\{0}\LocalServer32", key));
      if (registryKey != null)
        try
        {
          string path = registryKey.GetValue(null) as string;
          if (!String.IsNullOrEmpty(path))
            return path.Remove(path.IndexOf("devenv.exe")).Trim('"');
        }
        finally
        {
          registryKey.Close();
        }
      return null;
    }

    #endregion
  }
}