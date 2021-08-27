using System;
using System.ComponentModel;

namespace Phenix.Windows
{
  /// <summary>
  /// 标准检索栏控件
  /// </summary>
  [Description("标准检索栏")]
  public sealed partial class CriteriaGroupControl : DevExpress.XtraEditors.GroupControl
  {
    /// <summary>
    /// 初始化
    /// </summary>
    public CriteriaGroupControl()
      : base()
    {
      InitializeComponent();
    }

    /// <summary>
    /// 初始化
    /// </summary>
    public CriteriaGroupControl(IContainer container)
      : base()
    {
      if (container == null)
        throw new ArgumentNullException("container"); 
      container.Add(this);

      InitializeComponent();
    }

    #region 方法

    #endregion
  }
}
