using System.Windows.Forms;
using Phenix.Core;

namespace Phenix.Windows
{
  /// <summary>
  /// 属性BindingSource值更改事件数据: Changing、Changed
  /// </summary>
  public sealed class BindingSourceChangeEventArgs : StopEventArgs
  {
    /// <summary>
    /// 初始化
    /// </summary>
    /// <param name="oldValue">原值</param>
    /// <param name="newValue">新值</param>
    public BindingSourceChangeEventArgs(BindingSource oldValue, BindingSource newValue)
      : base()
    {
      _oldValue = oldValue;
      _newValue = newValue;
    }

    #region 属性

    private readonly BindingSource _oldValue;
    /// <summary>
    /// 原值
    /// </summary>
    public BindingSource OldValue
    {
      get { return _oldValue; }
    }

    private readonly BindingSource _newValue;
    /// <summary>
    /// 新值
    /// </summary>
    public BindingSource NewValue
    {
      get { return _newValue; }
    }
        
    #endregion
  }
}
