using System;

namespace Phenix.StandardService.Contract
{
  public interface IWorker
  {
    #region ����

    /// <summary>
    /// �����ݿ�ʱ�ӱ���ͬ��
    /// </summary>
    DateTime? SynchronizeClock();

    #endregion
  }
}
