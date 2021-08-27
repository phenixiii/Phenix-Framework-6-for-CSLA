using System.Collections.Generic;
using System.Windows.Forms;
using DevExpress.XtraBars;
using Phenix.Core.Mapping;

namespace Phenix.Windows
{
  /// <summary>
  /// 菜单项Click事件数据: Importing、Imported
  /// </summary>
  public class BarItemImportEventArgs : BarItemClickEventArgs
  {
    /// <summary>
    /// 初始化
    /// </summary>
    /// <param name="item">BarItem</param>
    /// <param name="source">BindingSource</param>
    public BarItemImportEventArgs(BarItem item, BindingSource source)
      : base(item, source) { }

    #region 属性

    /// <summary>
    /// 表单名
    /// 如为空则取Execl表单第一个Sheet内数据
    /// </summary>
    public string SheetName { get; set; }

    private List<IPropertyInfo> _propertyInfos;
    /// <summary>
    /// 属性信息队列, 顺序与表单columnIndex一致
    /// 如为空则按Execl表单列名与业务类属性名匹配条件进行数据填充
    /// </summary>
    public IList<IPropertyInfo> PropertyInfos
    {
      get
      {
        if (_propertyInfos == null)
          _propertyInfos = new List<IPropertyInfo>();
        return _propertyInfos;
      }
    }

    #endregion
  }
}
