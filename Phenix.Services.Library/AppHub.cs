namespace Phenix.Services.Library
{
  /// <summary>
  /// 应用系统中心
  /// </summary>
  public static class AppHub
  {
    #region 属性

    private static IAuthoriser _authoriser;
    /// <summary>
    /// 授权者
    /// </summary>
    public static IAuthoriser Authoriser
    {
      get { return _authoriser; }
      set { _authoriser = value; }
    }

    #endregion
  }
}
