using System.Collections.Generic;
using System.Windows.Forms;
using DevExpress.XtraBars;
using Phenix.Business;

namespace Phenix.Windows
{
  /// <summary>
  /// 菜单项Save事件数据: Saving、Saved
  /// </summary>
  public class BarItemSaveEventArgs : BarItemClickEventArgs
  {
    /// <summary>
    /// 初始化
    /// </summary>
    /// <param name="item">BarItem</param>
    /// <param name="source">BindingSource</param>
    public BarItemSaveEventArgs(BarItem item, BindingSource source)
      : base(item, source) { }

    #region 属性

    private bool _needCheckDirty = true;
    /// <summary>
    /// 校验数据库数据在下载到提交期间是否被更改过, 一旦发现将报错: CheckDirtyException
    /// 缺省为 true
    /// </summary>
    public bool NeedCheckDirty
    {
      get { return _needCheckDirty; }
      set { _needCheckDirty = value; }
    }

    /// <summary>
    /// 仅提交被勾选的业务对象
    /// 设置为 null 时, 以Source上的 OnlySaveSelected 属性值为准
    /// 仅适用于业务集合对象的提交
    /// </summary>
    public bool? OnlySaveSelected { get; set; }

    private List<IBusiness> _firstTransactionData;
    /// <summary>
    /// 与Source数据在一个事务里提交数据库, 并提前于Source数据执行保存操作
    /// </summary>
    public IList<IBusiness> FirstTransactionData
    {
      get
      {
        if (_firstTransactionData == null)
          _firstTransactionData = new List<IBusiness>();
        return _firstTransactionData;
      }
    }

    private List<IBusiness> _lastTransactionData;
    /// <summary>
    /// 与Source数据在一个事务里提交数据库, 并滞后于Source数据执行保存操作
    /// </summary>
    public IList<IBusiness> LastTransactionData
    {
      get
      {
        if (_lastTransactionData == null)
          _lastTransactionData = new List<IBusiness>();
        return _lastTransactionData;
      }
    }

    #endregion

    #region 方法

    internal IBusiness[] GetFirstTransactionData()
    {
      return _firstTransactionData != null ? _firstTransactionData.ToArray() : null;
    }

    internal IBusiness[] GetLastTransactionData()
    {
      return _lastTransactionData != null ? _lastTransactionData.ToArray() : null;
    }

    #endregion
  }
}
