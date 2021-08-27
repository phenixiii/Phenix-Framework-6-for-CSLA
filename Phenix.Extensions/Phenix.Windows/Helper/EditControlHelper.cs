using System.Collections.Generic;
using System.Windows.Forms;
using DevExpress.XtraEditors;

namespace Phenix.Windows.Helper
{
  /// <summary>
  /// EditControl助手
  /// </summary>
  public static class EditControlHelper
  {
    #region 方法

    /// <summary>
    /// 搜索第一个匹配的控件，并定位焦点
    /// </summary>
    /// <param name="controls">控件队列</param>
    /// <returns>被定位焦点的编辑控件</returns>
    public static Control FocusControl(IList<Control> controls)
    {
      if (controls == null)
        return null;
      foreach (Control item in controls)
        if (item.Enabled && item.Visible && item.CanFocus)
        {
          item.Focus();
          return item;
        }
      return null;
    }

    /// <summary>
    /// 搜索第一个匹配的控件，并定位焦点
    /// </summary>
    /// <param name="container">控件容器</param>
    /// <param name="source">数据源</param>
    /// <returns>被定位焦点的编辑控件</returns>
    public static Control FocusControl(Control container, BindingSource source)
    {
      return FocusControl(Phenix.Core.Windows.ControlHelper.FindEditControls(container, source));
    }

    /// <summary>
    /// 搜索第一个匹配的可编辑控件，并定位焦点
    /// </summary>
    /// <param name="controls">控件队列</param>
    /// <returns>被定位焦点的编辑控件</returns>
    public static Control FocusEditableControl(IList<Control> controls)
    {
      if (controls == null)
        return null;
      foreach (Control item in controls)
        if (item.Enabled && item.Visible && item.CanFocus)
        {
          //for Developer Express .NET
          BaseEdit edit = item as BaseEdit;
          if (edit != null && edit.Properties.ReadOnly)
            continue;
          item.Focus();
          return item;
        }
      return null;
    }

    /// <summary>
    /// 搜索第一个匹配的可编辑控件，并定位焦点
    /// </summary>
    /// <param name="container">控件容器</param>
    /// <param name="source">数据源</param>
    /// <returns>被定位焦点的编辑控件</returns>
    public static Control FocusEditableControl(Control container, BindingSource source)
    {
      return FocusEditableControl(Phenix.Core.Windows.ControlHelper.FindEditControls(container, source));
    }
  }
  
  #endregion
}
