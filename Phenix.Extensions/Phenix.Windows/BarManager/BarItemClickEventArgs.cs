using DevExpress.XtraBars;
using System.Windows.Forms;
using Phenix.Core;

namespace Phenix.Windows
{
  /// <summary>
  /// 菜单项Click事件数据
  /// </summary>
  public class BarItemClickEventArgs : ShallEventArgs
  {
    /// <summary>
    /// 初始化
    /// </summary>
    /// <param name="item">BarItem</param>
    /// <param name="source">BindingSource</param>
    public BarItemClickEventArgs(BarItem item, BindingSource source)
      : base()
    {
      _item = item;
      _source = source;
    }

    #region 属性

    private readonly BarItem _item;
    /// <summary>
    /// BarItem
    /// </summary>
    public BarItem Item
    {
      get { return _item; }
    }

    private BindingSource _source;
    /// <summary>
    /// BindingSource
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
    public BindingSource Source
    {
      get { return _source; }
      set { _source = value; }
    }

    #endregion
  }
}