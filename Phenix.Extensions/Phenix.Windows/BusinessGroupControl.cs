using System;
using System.ComponentModel;

namespace Phenix.Windows
{
  /// <summary>
  /// 标准业务栏控件
  /// </summary>
  [Description("标准业务栏")]
  public sealed partial class BusinessGroupControl : DevExpress.XtraEditors.GroupControl
  {
    /// <summary>
    /// 初始化
    /// </summary>
    public BusinessGroupControl()
      : base()
    {
      InitializeComponent();
    }

    /// <summary>
    /// 初始化
    /// </summary>
    public BusinessGroupControl(IContainer container)
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
