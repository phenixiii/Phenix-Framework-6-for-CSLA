namespace Phenix.Core.Web
{
  /// <summary>
  /// 应用中心
  /// </summary>
  public static class AppHub
  {
    #region 属性

    private static ISecurityProxy _securityProxy;
    /// <summary>
    /// 安全代理
    /// </summary>
    public static ISecurityProxy SecurityProxy
    {
      get { return _securityProxy; }
      set { _securityProxy = value; }
    }

    private static IDataProxy _dataProxy;
    /// <summary>
    /// 数据代理
    /// </summary>
    public static IDataProxy DataProxy
    {
      get { return _dataProxy; }
      set { _dataProxy = value; }
    }

    private static IMessageProxy _messageProxy;
    /// <summary>
    /// 消息代理
    /// </summary>
    public static IMessageProxy MessageProxy
    {
      get { return _messageProxy; }
      set { _messageProxy = value; }
    }

    #endregion
  }
}
