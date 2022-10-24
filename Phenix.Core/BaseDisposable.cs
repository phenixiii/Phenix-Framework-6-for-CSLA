using System;

namespace Phenix.Core
{
  /// <summary>
  /// �ͷ���Դ������
  /// </summary>
  public abstract class BaseDisposable<T> : BaseDisposable
    where T : BaseDisposable<T>
  {
    #region ����

    private static readonly object _defaultLock = new object();
    private static T _default;
    /// <summary>
    /// ����
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1000:DoNotDeclareStaticMembersOnGenericTypes")]
    public static T Default
    {
      get
      {
        if (_default == null)
          lock (_defaultLock)
            if (_default == null)
            {
              _default = (T)Activator.CreateInstance(typeof(T), true);
            }
        return _default;
      }
    }

    #endregion

    #region ����

    #region ʵ�� BaseDisposable ������

    /// <summary>
    /// �ͷ��й���Դ
    /// </summary>
    protected override void DisposeManagedResources()
    {
      if (_default == this)
        lock (_defaultLock)
          if (_default == this)
          {
            _default = null;
          }
    }

    /// <summary>
    /// �ͷŷ��й���Դ
    /// </summary>
    protected override void DisposeUnmanagedResources()
    {
    }

    #endregion

    #endregion
  }

  /// <summary>
  /// �ͷ���Դ������
  /// </summary>
  public abstract class BaseDisposable : IDisposable
  {
    /// <summary>
    /// �ͷ���Դ������
    /// </summary>
    ~BaseDisposable()
    {
      Dispose(false);
    }

    #region ����

    private bool _disposing;
    /// <summary>
    /// �����ͷ���
    /// </summary>
    public bool Disposing
    {
      get { return _disposing; }
    }

    private bool _resourcesDisposed;

    #endregion

    #region ����

    #region IDisposable ��Ա

    /// <summary>
    /// �ͷ��Լ�
    /// </summary>
    public void Dispose()
    {
      Dispose(true);
      GC.SuppressFinalize(this);
    }

    #endregion

    /// <summary>
    /// �ͷ�
    /// </summary>
    /// <param name="disposing">���Ϊ true, ���ͷ��й���Դ�ͷ��й���Դ; ���Ϊ false, ����ͷŷ��й���Դ</param>
    private void Dispose(bool disposing)
    {
      _disposing = true;
      if (!_resourcesDisposed)
      {
        if (disposing)
          DisposeManagedResources();
        DisposeUnmanagedResources();
        _resourcesDisposed = true;
      }
    }

    /// <summary>
    /// �ͷ��й���Դ
    /// </summary>
    protected abstract void DisposeManagedResources();

    /// <summary>
    /// �ͷŷ��й���Դ
    /// </summary>
    protected abstract void DisposeUnmanagedResources();

    /// <summary>
    /// �ͷ���Դ
    /// </summary>
    public void Close()
    {
      Dispose(true);
    }

    /// <summary>
    /// �����ͷŶ���
    /// </summary>
    /// <param name="value">����</param>
    public static bool TryDispose(object value)
    {
      IDisposable disposable = value as IDisposable;
      if (disposable != null)
      {
        disposable.Dispose();
        return true;
      }
      return false;
    }

    #endregion
  }
}
