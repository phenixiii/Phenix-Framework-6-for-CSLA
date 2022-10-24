using System;

namespace Phenix.Core.Operate
{
  /// <summary>
  /// 对象勾选接口
  /// </summary>
  public interface ISelectable
  {
    #region 属性

    /// <summary>
    /// 所属对象勾选集合
    /// </summary>
    ISelectableCollection Owner { get; }

    /// <summary>
    /// 是否被勾选
    /// 用于标记本对象
    /// 缺省为 false
    /// </summary>
    bool Selected { get; set; }

    #endregion

    #region 事件

    /// <summary>
    /// Selected属性值被更改前
    /// </summary>
    event EventHandler<SelectedValueChangingEventArgs> SelectedValueChanging;

    /// <summary>
    /// Selected属性值被更改后
    /// </summary>
    event EventHandler<SelectedValueChangedEventArgs> SelectedValueChanged;

    #endregion
  }
}
