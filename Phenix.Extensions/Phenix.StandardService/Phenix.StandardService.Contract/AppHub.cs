namespace Phenix.StandardService.Contract
{
  /// <summary>
  /// Ӧ��ϵͳ����
  /// </summary>
  public static class AppHub
  {
    #region ����

    private static IWorker _worker;
    public static IWorker Worker
    {
      get { return _worker; }
      set { _worker = value; }
    }

    #endregion
  }
}