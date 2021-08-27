namespace Phenix.StandardPush.Contract
{
  public interface IWorker
  {
    #region ����

    #region Socket���ò���

    /// <summary>
    /// ȱʡ�˿�
    /// </summary>
    int Port { get; }

    /// <summary>
    /// Socket����ߴ�
    /// </summary>
    int SocketBufferSize { get; }

    /// <summary>
    /// ��Ϣ����ߴ�
    /// </summary>
    int MessageBufferSize { get; }

    /// <summary>
    /// ���ü����
    /// </summary>
    int IdleCheckInterval { get; }

    /// <summary>
    /// ���ó�ʱʱ��
    /// </summary>
    int IdleTimeOutValue { get; }

    #endregion

    #endregion
  }
}
