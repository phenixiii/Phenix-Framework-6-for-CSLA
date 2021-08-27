using System.Windows.Forms;
using DevExpress.XtraBars;

namespace Phenix.Windows
{
  /// <summary>
  /// 菜单项Delete事件数据: Deleting、Deleted、DeleteCanceled
  /// </summary>
  public class BarItemDeleteEventArgs : BarItemSaveEventArgs
  {
    /// <summary>
    /// 初始化
    /// </summary>
    /// <param name="item">BarItem</param>
    /// <param name="source">BindingSource</param>
    public BarItemDeleteEventArgs(BarItem item, BindingSource source)
      : base(item, source) { }
  }
}
