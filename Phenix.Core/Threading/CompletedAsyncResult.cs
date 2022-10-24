using System;

namespace Phenix.Core.Threading
{
  /// <summary>
  /// 异步完成
  /// </summary>
  public class CompletedAsyncResult<T> : AsyncResult
  {
    /// <summary>
    /// 初始化
    /// </summary>
    public CompletedAsyncResult(T data, AsyncCallback callback, object asyncState)
      : base(callback, asyncState)
    {
      _data = data;
      Complete(true);
    }
    
    #region 属性

    private readonly T _data;

    #endregion

    #region 方法

    /// <summary>
    /// 中止
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1000:DoNotDeclareStaticMembersOnGenericTypes")]
    public static T End(CompletedAsyncResult<T> result)
    {
      CompletedAsyncResult<T> completedResult = End<CompletedAsyncResult<T>>(result);
      return completedResult._data;
    }

    #endregion
  }
}
