namespace Phenix.StandardService.Contract
{
  /// <summary>
  /// 应用系统中心
  /// </summary>
  public static class AppHub
  {
    #region 属性

    private static IWorker _worker;
    public static IWorker Worker
    {
      get { return _worker; }
      set { _worker = value; }
    }

    #endregion
  }
}