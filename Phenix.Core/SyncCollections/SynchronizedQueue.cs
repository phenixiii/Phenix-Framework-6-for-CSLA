using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Threading;

namespace Phenix.Core.SyncCollections
{
	/// <summary>
  /// ��ʾ������Ƚ��ȳ�����
	/// </summary>
  /// <typeparam name="T">ָ��������Ԫ�ص�����</typeparam>
  [Serializable]
  public class SynchronizedQueue<T> : IEnumerable<T>, ISerializable
	{
		/// <summary>
    /// ��ʼ��
		/// </summary>
		public SynchronizedQueue()
		{
      _rwLock = new ReaderWriterLock();
			_infos = new Queue<T>();
		}

		/// <summary>
    ///	��ʼ��
    ///	��ʵ��������ָ���ļ����и��Ƶ�Ԫ�ز����������������������Ƶ�Ԫ����
		/// </summary>
    /// <param name="collection">��Ԫ�ر����Ƶ��µļ����еļ���</param>
		public SynchronizedQueue(IEnumerable<T> collection)
		{
      _rwLock = new ReaderWriterLock();
      _infos = new Queue<T>(collection);
		}

		/// <summary>
    /// ��ʼ��
    /// ��ʵ��Ϊ�ղ��Ҿ���ָ���ĳ�ʼ����
		/// </summary>
    /// <param name="capacity">�ɰ����ĳ�ʼԪ����</param>
		public SynchronizedQueue(int capacity)
		{
      _rwLock = new ReaderWriterLock();
      _infos = new Queue<T>(capacity);
    }
        
    #region Serialization

    /// <summary>
    /// ���л�
    /// </summary>
    protected SynchronizedQueue(SerializationInfo info, StreamingContext context)
    {
      if (info == null)
        throw new ArgumentNullException("info");
      _rwLock = new ReaderWriterLock();
      _infos = (Queue<T>)info.GetValue("_infos", typeof(Queue<T>));
    }

    /// <summary>
    /// �����л�
    /// </summary>
    [System.Security.SecurityCritical]
    public virtual void GetObjectData(SerializationInfo info, StreamingContext context)
    {
      if (info == null)
        throw new ArgumentNullException("info");
      info.AddValue("_infos", _infos);
    }

    #endregion

    #region ����

    [NonSerialized]
    private readonly ReaderWriterLock _rwLock;

		private readonly Queue<T> _infos;

		/// <summary>
    /// ��ȡ�����а�����Ԫ����
		/// </summary>
		public int Count
		{
			get
			{
        _rwLock.AcquireReaderLock(Timeout.Infinite);
        try
				{
					return _infos.Count;
				}
				finally
				{
					_rwLock.ReleaseReaderLock();
				}
			}
		}

		#endregion

    #region ����

    #region Contains
    
    /// <summary>
    /// ȷ��ĳԪ���Ƿ��ڼ�����
		/// </summary>
    /// <param name="item">Ҫ��λ�Ķ���. ������������, ��ֵ����Ϊ null</param>
		public bool Contains(T item)
		{
      _rwLock.AcquireReaderLock(Timeout.Infinite);
      try
			{
				return _infos.Contains(item);
			}
			finally
			{
				_rwLock.ReleaseReaderLock();
			}
		}

    #endregion

    #region Peek

    /// <summary>
    /// ����λ�ڼ��Ͽ�ʼ���Ķ��󵫲������Ƴ�
    /// </summary>
    public T Peek()
    {
      _rwLock.AcquireReaderLock(Timeout.Infinite);
      try
      {
        return _infos.Peek();
      }
      finally
      {
        _rwLock.ReleaseReaderLock();
      }
    }

    #endregion

    #region Enqueue

    /// <summary>
    /// ��������ӵ����ϵĽ�β��
    /// </summary>
    /// <param name="item">Ҫ��ӵ������еĶ���. ������������, ��ֵ����Ϊ null</param>
    public void Enqueue(T item)
    {
      _rwLock.AcquireWriterLock(Timeout.Infinite);
      try
      {
        _infos.Enqueue(item);
      }
      finally
      {
        _rwLock.ReleaseWriterLock();
      }
    }

	  /// <summary>
	  /// ��������ӵ����ϵĽ�β��
	  /// </summary>
	  /// <param name="item">Ҫ��ӵ������еĶ���. ������������, ��ֵ����Ϊ null</param>
    /// <param name="checkRepeat">������β����ͬһ������Ļ��Ͳ����</param>
    /// <returns>�Ƿ�ɹ����</returns>
	  public bool Enqueue(T item, bool checkRepeat)
    {
      _rwLock.AcquireWriterLock(Timeout.Infinite);
      try
      {
        if (checkRepeat && object.Equals(item, Peek()))
          return false;
        _infos.Enqueue(item);
        return true;
      }
      finally
      {
        _rwLock.ReleaseWriterLock();
      }
    }

    #endregion

    #region Dequeue

    /// <summary>
    ///	�Ƴ�������λ�ڼ��Ͽ�ʼ���Ķ���
		/// </summary>
		public T Dequeue()
		{
      _rwLock.AcquireWriterLock(Timeout.Infinite);
      try
			{
				return _infos.Dequeue();
			}
			finally
			{
				_rwLock.ReleaseWriterLock();
			}
		}

    /// <summary>
    ///	�Ƴ�������λ�ڼ��Ͽ�ʼ���Ķ���
    /// </summary>
    /// <param name="item">Ҫ�Ƴ��Ķ���Ӧ�����β����ͬһ������. ������������, ��ֵ����Ϊ null</param>
    /// <returns>�������ֵΪitem˵���ѳɹ��Ƴ�</returns>
    public T Dequeue(T item)
    {
      _rwLock.AcquireWriterLock(Timeout.Infinite);
      try
      {
        if (!object.Equals(item, Peek()))
          return default(T);
        return _infos.Dequeue();
      }
      finally
      {
        _rwLock.ReleaseWriterLock();
      }
    }

    #endregion

    #region Clear

    /// <summary>
    /// �Ƴ����ж���
    /// </summary>
    public void Clear()
    {
      _rwLock.AcquireWriterLock(Timeout.Infinite);
      try
      {
        _infos.Clear();
      }
      finally
      {
        _rwLock.ReleaseWriterLock();
      }
    }
    
	  /// <summary>
	  /// �Ƴ����нڵ�
	  /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures")]
    public void Clear(Action<IEnumerable<T>> doDispose)
	  {
	    _rwLock.AcquireWriterLock(Timeout.Infinite);
	    try
	    {
	      if (doDispose != null)
	        doDispose(new List<T>(_infos));
	      _infos.Clear();
	    }
	    finally
	    {
	      _rwLock.ReleaseWriterLock();
	    }
	  }

    #endregion

    #region IEnumerator

    /// <summary>
    /// ����ѭ�����ʵ�ö����, Ϊ��̬����
    /// </summary>
    public IEnumerator GetEnumerator()
    {
      Queue<T> result;
      _rwLock.AcquireReaderLock(Timeout.Infinite);
      try
      {
        result = new Queue<T>(_infos);
      }
      finally
      {
        _rwLock.ReleaseReaderLock();
      }
      return result.GetEnumerator();
    }

    /// <summary>
    ///	����ѭ�����ʵ�ö����, Ϊ��̬����
    /// </summary>
    IEnumerator<T> IEnumerable<T>.GetEnumerator()
    {
      Queue<T> result;
      _rwLock.AcquireReaderLock(Timeout.Infinite);
      try
      {
        result = new Queue<T>(_infos);
      }
      finally
      {
        _rwLock.ReleaseReaderLock();
      }
      return result.GetEnumerator();
    }

    #endregion
    
    /// <summary>
    /// ���Ԫ����С�ڵ�ǰ������ 90%, ����������Ϊ�����е�ʵ��Ԫ����
    /// </summary>
    public void TrimExcess()
    {
      _rwLock.AcquireWriterLock(Timeout.Infinite);
      try
      {
        _infos.TrimExcess();
      }
      finally
      {
        _rwLock.ReleaseWriterLock();
      }
    }

	  /// <summary>
	  /// �����ϵ�Ԫ�ظ��Ƶ���������
	  /// clear = false
	  /// </summary>
	  public T[] ToArray()
	  {
	    return ToArray(false);
	  }

	  /// <summary>
	  /// �����ϵ�Ԫ�ظ��Ƶ���������
	  /// <param name="clear">�����</param>
	  /// </summary>
	  public T[] ToArray(bool clear)
	  {
	    _rwLock.AcquireReaderLock(Timeout.Infinite);
	    try
	    {
	      T[] result = _infos.ToArray();
	      if (clear)
	      {
	        LockCookie lockCookie = _rwLock.UpgradeToWriterLock(Timeout.Infinite);
	        try
	        {
	          _infos.Clear();
	        }
	        finally
	        {
	          _rwLock.DowngradeFromWriterLock(ref lockCookie);
	        }
	      }
	      return result;
	    }
	    finally
	    {
	      _rwLock.ReleaseReaderLock();
	    }
	  }

    /// <summary>
    /// ��ָ������������ʼ�������е�Ԫ�ظ��Ƶ�����һά Array ��
    /// </summary>
    /// <param name="array">��Ϊ�Ӽ����и��Ƶ�Ԫ�ص�Ŀ��λ�õ�һά Array. Array ������д��㿪ʼ������</param>
    /// <param name="arrayIndex">array �д��㿪ʼ������, �ڴ˴���ʼ����</param>
    public void CopyTo(T[] array, int arrayIndex)
    {
      _rwLock.AcquireReaderLock(Timeout.Infinite);
      try
      {
        _infos.CopyTo(array, arrayIndex);
      }
      finally
      {
        _rwLock.ReleaseReaderLock();
      }
    }

    #endregion
  }
}
