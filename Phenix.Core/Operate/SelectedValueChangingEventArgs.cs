
namespace Phenix.Core.Operate
{
  /// <summary>
  /// Selected属性值被更改前事件数据
  /// </summary>
  public sealed class SelectedValueChangingEventArgs : StopEventArgs
  {
    /// <summary>
    /// 初始化
    /// </summary>
    public SelectedValueChangingEventArgs(ISelectable selectable)
      : base()
    {
      _selectable = selectable;
    }

    #region 属性

    private readonly ISelectable _selectable;
    /// <summary>
    /// 勾选项
    /// </summary>
    public ISelectable Selectable
    {
      get { return _selectable; }
    }
    
    #endregion
  }
}