using System;
using System.Reflection;
using System.Threading;

namespace Phenix.Core.Threading
{
  /// <summary>
  /// 用于通用异步编程方案中使用的基类
  /// </summary>
  public abstract class AsyncResult : BaseDisposable, IAsyncResult
  {
    /// <summary>
    /// 初始化
    /// </summary>
    protected AsyncResult(AsyncCallback callback, object asyncState)
    {
      _callback = callback;
      _asyncState = asyncState;
    }

    #region 定义

    /// <summary>
    /// 异步完成处理函数
    /// can be utilized by subclasses to write core completion code for both the sync and async paths
    /// in one location, signaling chainable synchronous completion with the boolean result,
    /// and leveraging PrepareAsyncCompletion for conversion to an AsyncCallback.
    /// NOTE: requires that "this" is passed in as the state object to the asynchronous sub-call being used with a completion routine.
    /// </summary>
    /// <param name="result">异步操作</param>
    protected delegate bool AsyncCompletionCallback(IAsyncResult result);

    #endregion

    #region 属性

    #region IAsyncResult 成员

    private bool _isCompleted;
    /// <summary>
    /// 指示异步操作是否已完成
    /// </summary>
    public bool IsCompleted
    {
      get { return _isCompleted; }
    }

    private bool _completedSynchronously;
    /// <summary>
    /// 指示异步操作是否同步完成
    /// </summary>
    public bool CompletedSynchronously
    {
      get { return _completedSynchronously; }
    }

    private readonly object _asyncWaitHandleLock = new object();
    private ManualResetEvent _asyncWaitHandle;
    /// <summary>
    /// 获取用于等待异步操作完成的 WaitHandle
    /// </summary>
    public WaitHandle AsyncWaitHandle
    {
      get
      {
        if (_asyncWaitHandle == null)
          lock (_asyncWaitHandleLock)
            if (_asyncWaitHandle == null)
              _asyncWaitHandle = new ManualResetEvent(_isCompleted);
        return _asyncWaitHandle;
      }
    }

    private readonly object _asyncState;
    /// <summary>
    /// 获取用户定义的对象
    /// 它限定或包含关于异步操作的信息
    /// </summary>
    public object AsyncState
    {
      get { return _asyncState; }
    }

    #endregion

    private readonly AsyncCallback _callback;
    private static AsyncCallback _asyncCompletionWrapperCallback;
    private AsyncCompletionCallback _nextAsyncCompletionCallback;

    private bool _endCalled;
    private Exception _exception;

    #endregion

    #region 方法

    #region 实现 BaseDisposable 抽象函数

    /// <summary>
    /// 释放托管资源
    /// </summary>
    protected override void DisposeManagedResources()
    {
      CloseAsyncWaitHandle();
    }

    /// <summary>
    /// 释放非托管资源
    /// </summary>
    protected override void DisposeUnmanagedResources()
    {
    }

    #endregion

    /// <summary>
    /// 完成
    /// </summary>
    protected void Complete(bool completedSynchronously)
    {
      if (_isCompleted)
        throw new InvalidOperationException(String.Format("{0}不允许被重复调用!", MethodBase.GetCurrentMethod().Name));

      _isCompleted = true;
      _completedSynchronously = completedSynchronously;

      if (!completedSynchronously)
        if (_asyncWaitHandle != null)
          lock (_asyncWaitHandleLock)
            if (_asyncWaitHandle != null)
              _asyncWaitHandle.Set();

      if (_callback != null)
        _callback(this);
    }

    /// <summary>
    /// 完成
    /// </summary>
    protected void Complete(bool completedSynchronously, Exception exception)
    {
      _exception = exception;
      Complete(completedSynchronously);
    }

    /// <summary>
    /// 准备异步完成
    /// </summary>
    protected AsyncCallback PrepareAsyncCompletion(AsyncCompletionCallback callback)
    {
      _nextAsyncCompletionCallback = callback;
      if (_asyncCompletionWrapperCallback == null)
        _asyncCompletionWrapperCallback = new AsyncCallback(AsyncCompletionWrapperCallback);
      return _asyncCompletionWrapperCallback;
    }

    private static void AsyncCompletionWrapperCallback(IAsyncResult result)
    {
      if (result.CompletedSynchronously)
        return;

      AsyncResult thisPtr = (AsyncResult)result.AsyncState;
      AsyncCompletionCallback callback = thisPtr._nextAsyncCompletionCallback;
      thisPtr._nextAsyncCompletionCallback = null;
      

      bool completeSelf;
      Exception completionException = null;
      try
      {
        completeSelf = callback(result);
      }
      catch (Exception ex)
      {
        if (AppUtilities.IsFatal(ex))
          throw;
        completeSelf = true;
        completionException = ex;
      }

      if (completeSelf)
        thisPtr.Complete(false, completionException);
    }

    /// <summary>
    /// 中止
    /// </summary>
    protected static TAsyncResult End<TAsyncResult>(TAsyncResult result)
      where TAsyncResult : AsyncResult
    {
      if (result == null)
        throw new ArgumentNullException("result");
      if (result._endCalled)
        throw new InvalidOperationException(String.Format("{0}不允许被重复调用!", MethodBase.GetCurrentMethod().Name));
      
      result._endCalled = true;

      if (!result.IsCompleted)
        result.AsyncWaitHandle.WaitOne();
      result.CloseAsyncWaitHandle();

      if (result._exception != null)
        throw result._exception;
      return result;
    }

    private void CloseAsyncWaitHandle()
    {
      if (_asyncWaitHandle != null)
      {
        _asyncWaitHandle.Close();
        _asyncWaitHandle = null;
      }
    }

    #endregion
  }
}
