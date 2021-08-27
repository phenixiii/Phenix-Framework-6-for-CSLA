using Phenix.Core.Data;

namespace Phenix.Services.Library
{
  /// <summary>
  /// 应用系统工具集
  /// </summary>
  public static class AppUtilities
  {
    #region 属性

    private static readonly object _serverVersionLock = new object();
    private static long? _serverVersion;
    /// <summary>
    /// 服务器版本
    /// </summary>
    public static long ServerVersion
    {
      get
      {
        if (!_serverVersion.HasValue)
          lock (_serverVersionLock)
            if (!_serverVersion.HasValue)
              _serverVersion = Sequence.Value;
        return _serverVersion.Value;
      }
    }

    #endregion

    #region 方法

    /// <summary>
    /// 重置服务器版本
    /// </summary>
    public static void ResetServerVersion()
    {
      lock (_serverVersionLock)
      {
        _serverVersion = null;
      }
    }

    #endregion
  }
}