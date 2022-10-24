using System;
using System.Collections.Generic;

namespace Phenix.Core.Operate
{
  /// <summary>
  /// 对象勾选集合接口
  /// </summary>
  public interface ISelectableCollection : ICloneable
  {
    #region 属性

    /// <summary>
    /// 被勾选的对象队列
    /// </summary>
    IList<ISelectable> SelectedItems { get; }
    
    #endregion

    #region 事件

    /// <summary>
    /// 勾选项被更改前
    /// </summary>
    event EventHandler<SelectedValueChangingEventArgs> ItemSelectedValueChanging;

    /// <summary>
    /// 勾选项被更改后
    /// </summary>
    event EventHandler<SelectedValueChangedEventArgs> ItemSelectedValueChanged;

    #endregion

    #region 方法

    /// <summary>
    /// 勾选所有
    /// </summary>
    void SelectAll();

    /// <summary>
    /// 取消所有勾选
    /// </summary>
    void UnselectAll();

    /// <summary>
    /// 反选所有
    /// </summary>
    void InverseAll();

    #endregion
  }
}
