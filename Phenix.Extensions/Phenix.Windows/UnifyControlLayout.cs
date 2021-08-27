using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Drawing;
using System.Windows.Forms;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraTreeList;
using Phenix.Core;
using Phenix.Windows.Helper;

namespace Phenix.Windows
{
  /// <summary>
  /// 统一界面风格组件
  /// </summary>
  [Description("统一界面风格")]
  [ProvideProperty("SetGridViewBackColor", typeof(Component))] //统一设置GridView背景色
  [ProvideProperty("DisplayRowNumber", typeof(Component))] //统一显示GridView行号
  [ToolboxItem(true), ToolboxBitmap(typeof(UnifyControlLayout), "Phenix.Windows.UnifyControlLayout")]
  public sealed class UnifyControlLayout : Component, IExtenderProvider, ISupportInitialize
  {
    /// <summary>
    /// 初始化
    /// </summary>
    public UnifyControlLayout()
      : base() { }

    /// <summary>
    /// 初始化
    /// </summary>
    /// <param name="container">组件容器</param>
    public UnifyControlLayout(IContainer container)
      : base()
    {
      if (container == null)
        throw new ArgumentNullException("container");
      container.Add(this);
    }

    #region 属性

    private new bool DesignMode
    {
      get { return base.DesignMode || AppConfig.DesignMode; }
    }

    private Control _host;
    /// <summary>
    /// 所属容器
    /// </summary>
    [DefaultValue(null), Browsable(false)]
    public Control Host
    {
      get
      {
        if (_host == null)
        {
          if (DesignMode)
          {
            IDesignerHost designer = this.GetService(typeof(IDesignerHost)) as IDesignerHost;
            if (designer != null)
              _host = designer.RootComponent as Control;
          }
        }
        return _host;
      }
      set
      {
        if (!DesignMode && _host != null)
          throw new InvalidOperationException("运行期不允许修改Host");
        _host = value;
        if (!DesignMode)
        {
          Form form = value as Form;
          if (form != null)
            form.Shown += new EventHandler(Host_Shown);
        }
      }
    }

    private static Color? _gridViewOddRowColor;
    /// <summary>
    /// 统一 GridView 奇数行颜色
    /// 当为 Color.Empty 时禁用本功能
    /// </summary>
    public static Color GridViewOddRowColor
    {
      get { return AppSettings.GetProperty(ref _gridViewOddRowColor, Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))))); }
      set { AppSettings.SetProperty(ref _gridViewOddRowColor, value); }
    }
    /// <summary>
    /// 禁用"统一 GridView 奇数行颜色"功能?
    /// </summary>
    public static bool GridViewOddRowColorDisabled
    {
      get { return GridViewOddRowColor.ToArgb() == Color.Empty.ToArgb(); }
    }

    private static Color? _gridViewEvenRowColor;
    /// <summary>
    /// 统一 GridView 偶数行颜色
    /// 当为 Color.Empty 时禁用本功能
    /// </summary>
    public static Color GridViewEvenRowColor
    {
      get { return AppSettings.GetProperty(ref _gridViewEvenRowColor, Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))))); }
      set { AppSettings.SetProperty(ref _gridViewEvenRowColor, value); }
    }
    /// <summary>
    /// 禁用"统一 GridView 偶数行颜色"功能?
    /// </summary>
    public static bool GridViewEvenRowColorDisabled
    {
      get { return GridViewEvenRowColor.ToArgb() == Color.Empty.ToArgb(); }
    }

    private static Color? _gridViewFocusedRowColor;
    /// <summary>
    /// 统一 GridView 焦点行颜色
    /// 当为 Color.Empty 时禁用本功能
    /// </summary>
    public static Color GridViewFocusedRowColor
    {
      get { return AppSettings.GetProperty(ref _gridViewFocusedRowColor, Color.Empty); }
      set { AppSettings.SetProperty(ref _gridViewFocusedRowColor, value); }
    }
    /// <summary>
    /// 禁用"统一 GridView 焦点行颜色"功能?
    /// </summary>
    public static bool GridViewFocusedRowColorDisabled
    {
      get { return GridViewFocusedRowColor.ToArgb() == Color.Empty.ToArgb(); }
    }

    private static bool? _gridViewDisplayRowNumber;
    /// <summary>
    /// 统一显示GridView行号?
    /// 缺省为 true
    /// </summary>
    public static bool GridViewDisplayRowNumber
    {
      get { return AppSettings.GetProperty(ref _gridViewDisplayRowNumber, true); }
      set { AppSettings.SetProperty(ref _gridViewDisplayRowNumber, value); }
    }

    private readonly Dictionary<Component, GridViewRuleStatus> _gridViewRuleSources = new Dictionary<Component, GridViewRuleStatus>();

    #endregion

    #region 扩展程序属性

    /// <summary>
    /// 统一设置GridView背景色
    /// </summary>
    [Description("统一设置GridView背景色"), Category("Phenix")]
    public bool GetSetGridViewBackColor(Component source)
    {
      GridViewRuleStatus result;
      if (_gridViewRuleSources.TryGetValue(source, out result))
        return result.SetGridViewBackColor;
      return true;
    }

    /// <summary>
    /// 统一设置GridView背景色
    /// </summary>
    public void SetSetGridViewBackColor(Component source, bool value)
    {
      GridViewRuleStatus status;
      if (_gridViewRuleSources.TryGetValue(source, out status))
        status.SetGridViewBackColor = value;
      else
        _gridViewRuleSources.Add(source, new GridViewRuleStatus { SetGridViewBackColor = value });
    }

    /// <summary>
    /// 统一显示GridView行号
    /// </summary>
    [Description("统一显示GridView行号"), Category("Phenix")]
    public bool GetDisplayRowNumber(Component source)
    {
      GridViewRuleStatus result;
      if (_gridViewRuleSources.TryGetValue(source, out result))
        return result.DisplayRowNumber;
      return true;
    }

    /// <summary>
    /// 统一显示GridView行号
    /// </summary>
    public void SetDisplayRowNumber(Component source, bool value)
    {
      GridViewRuleStatus status;
      if (_gridViewRuleSources.TryGetValue(source, out status))
        status.DisplayRowNumber = value;
      else
        _gridViewRuleSources.Add(source, new GridViewRuleStatus { DisplayRowNumber = value });
    }

    #endregion

    #region 事件

    private void Host_Shown(object sender, EventArgs e)
    {
      InitializeRule();
    }

    private void GridView_RowCountChanged(object sender, EventArgs e)
    {
      GridView gridView = (GridView)sender;
      gridView.IndicatorWidth = ((int)(Math.Log10((double)gridView.DataRowCount))) * 7 + 27;
    }

    private void GridView_CustomDrawRowIndicator(object sender, RowIndicatorCustomDrawEventArgs e)
    {
      if (e.Info.IsRowIndicator && e.RowHandle >= 0)
      {
        e.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
        e.Info.DisplayText = (e.RowHandle + 1).ToString();
      }
    }

    #endregion

    #region 方法

    #region IExtenderProvider 成员

    /// <summary>
    /// 是否可以将扩展程序属性提供给指定的对象
    /// </summary>
    /// <param name="extendee">要接收扩展程序属性的对象</param>
    public bool CanExtend(object extendee)
    {
      return extendee is GridView || extendee is TreeList;
    }

    #endregion

    #region ISupportInitialize 成员

    ///<summary>
    /// 开始初始化
    ///</summary>
    public void BeginInit()
    {
    }

    ///<summary>
    /// 结束初始化
    ///</summary>
    public void EndInit()
    {
      if (!DesignMode && !(Host is Form))
        Host_Shown(null, null);
    }

    #endregion

    private void InitializeRule()
    {
      Color gridViewOddRowColor = GridViewOddRowColor;
      Color gridViewEvenRowColor = GridViewEvenRowColor;
      Color gridViewFocusedRowColor = GridViewFocusedRowColor;
      bool gridViewDisplayRowNumber = GridViewDisplayRowNumber;
      foreach (KeyValuePair<Component, GridViewRuleStatus> kvp in _gridViewRuleSources)
      {
        if (kvp.Value.SetGridViewBackColor)
          if (kvp.Key is GridView)
            ((GridView)kvp.Key).SetBackColor(gridViewOddRowColor, gridViewEvenRowColor, gridViewFocusedRowColor);
          else if (kvp.Key is TreeList)
            ((TreeList)kvp.Key).SetBackColor(gridViewOddRowColor, gridViewEvenRowColor);
        if (kvp.Value.DisplayRowNumber && gridViewDisplayRowNumber)
          if (kvp.Key is GridView)
          {
            ((GridView)kvp.Key).RowCountChanged += new EventHandler(GridView_RowCountChanged);
            ((GridView)kvp.Key).CustomDrawRowIndicator += new RowIndicatorCustomDrawEventHandler(GridView_CustomDrawRowIndicator);
          }
      }
    }

    /// <summary>
    /// 重置规则
    /// </summary>
    public bool ResetRule(GridView gridView)
    {
      foreach (KeyValuePair<Component, GridViewRuleStatus> kvp in _gridViewRuleSources)
        if (kvp.Key == gridView)
        {
          if (kvp.Value.SetGridViewBackColor)
            ((GridView)kvp.Key).SetBackColor(GridViewOddRowColor, GridViewEvenRowColor, GridViewFocusedRowColor);
          return true;
        }
      return false;
    }

    /// <summary>
    /// 重置规则
    /// </summary>
    public bool ResetRule(TreeList treeList)
    {
      foreach (KeyValuePair<Component, GridViewRuleStatus> kvp in _gridViewRuleSources)
        if (kvp.Key == treeList)
        {
          if (kvp.Value.SetGridViewBackColor)
            ((TreeList)kvp.Key).SetBackColor(GridViewOddRowColor, GridViewEvenRowColor);
          return true;
        }
      return false;
    }

    #endregion

    #region 内嵌类

    [Serializable]
    private class GridViewRuleStatus
    {
      private bool _setGridViewBackColor = true;
      public bool SetGridViewBackColor
      {
        get { return _setGridViewBackColor; }
        set { _setGridViewBackColor = value; }
      }

      private bool _displayRowNumber = true;
      public bool DisplayRowNumber
      {
        get { return _displayRowNumber; }
        set { _displayRowNumber = value; }
      }
    }

    #endregion
  }
}