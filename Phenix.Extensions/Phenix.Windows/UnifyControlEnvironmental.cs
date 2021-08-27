using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Drawing;
using System.Reflection;
using System.Windows.Forms;
using Phenix.Core;

namespace Phenix.Windows
{
  /// <summary>
  /// 统一控件环境属性组件
  /// </summary>
  [Description("统一控件环境属性")]
  [ProvideProperty("SetFont", typeof(Component))] //统一设置控件Font
  [ProvideProperty("SetBackColor", typeof(Component))] //统一设置控件BackColor
  [ProvideProperty("SetForeColor", typeof(Component))] //统一设置控件ForeColor
  [ProvideProperty("SetRightToLeft", typeof(Component))] //统一设置控件RightToLeft
  [ToolboxItem(true), ToolboxBitmap(typeof(UnifyControlEnvironmental), "Phenix.Windows.UnifyControlEnvironmental")]
  public sealed class UnifyControlEnvironmental : Component, IExtenderProvider, ISupportInitialize
  {
    /// <summary>
    /// 初始化
    /// </summary>
    public UnifyControlEnvironmental()
      : base() { }

    /// <summary>
    /// 初始化
    /// </summary>
    /// <param name="container">组件容器</param>
    public UnifyControlEnvironmental(IContainer container)
      : base()
    {
      if (container == null)
        throw new ArgumentNullException("container");
      container.Add(this);
    }

    #region 属性

    private const string FONT_NAME = "Font";
    private const string BACK_COLOR_NAME = "BackColor";
    private const string FORE_COLOR_NAME = "ForeColor";

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

    private static Font _font;
    /// <summary>
    /// 统一控件 Font
    /// 当为 SystemFonts.DefaultFont 时禁用本功能
    /// </summary>
    public static Font Font
    {
      get { return AppSettings.GetProperty(ref _font, SystemFonts.DefaultFont); }
      set { AppSettings.SetProperty(ref _font, value); }
    }
    /// <summary>
    /// 禁用"统一控件 Font"功能?
    /// </summary>
    public static bool FontDisabled
    {
      get { return SystemFonts.DefaultFont.Equals(Font); }
    }

    private static Color? _backColor;
    /// <summary>
    /// 统一控件 BackColor
    /// 当为 Color.Empty 时禁用本功能
    /// </summary>
    public static Color BackColor
    {
      get { return AppSettings.GetProperty(ref _backColor, Color.Empty); }
      set { AppSettings.SetProperty(ref _backColor, value); }
    }
    /// <summary>
    /// 禁用"统一控件 BackColor"功能?
    /// </summary>
    public static bool BackColorDisabled
    {
      get { return BackColor.ToArgb() == Color.Empty.ToArgb(); }
    }

    private static Color? _foreColor;
    /// <summary>
    /// 统一控件 ForeColor
    /// 当为 Color.Empty 时禁用本功能
    /// </summary>
    public static Color ForeColor
    {
      get { return AppSettings.GetProperty(ref _foreColor, Color.Empty); }
      set { AppSettings.SetProperty(ref _foreColor, value); }
    }
    /// <summary>
    /// 禁用"统一控件 ForeColor"功能?
    /// </summary>
    public static bool ForeColorDisabled
    {
      get { return ForeColor.ToArgb() == Color.Empty.ToArgb(); }
    }

    private static RightToLeft? _rightToLeft;
    /// <summary>
    /// 统一控件 RightToLeft
    /// 当为 RightToLeft.Inherit 时禁用本功能
    /// </summary>
    public static RightToLeft RightToLeft
    {
      get { return AppSettings.GetProperty(ref _rightToLeft, RightToLeft.Inherit); }
      set { AppSettings.SetProperty(ref _rightToLeft, value); }
    }
    /// <summary>
    /// 禁用"统一控件 RightToLeft"功能?
    /// </summary>
    public static bool RightToLeftDisabled
    {
      get { return RightToLeft == RightToLeft.Inherit; }
    }

    private readonly Dictionary<Control, RuleStatus> _controlRuleSources = new Dictionary<Control, RuleStatus>();
    //private readonly Dictionary<DevExpress.XtraBars.BarManager, RuleStatus> _barManagerRuleSources =
    //  new Dictionary<DevExpress.XtraBars.BarManager, RuleStatus>();
    private readonly Dictionary<Component, RuleStatus> _ruleSources = new Dictionary<Component, RuleStatus>();

    #endregion

    #region 扩展程序属性

    /// <summary>
    /// 统一设置控件Font
    /// </summary>
    [Description("统一设置控件Font"), Category("Phenix")]
    public bool GetSetFont(Component source)
    {
      RuleStatus result;

      Control control = source as Control;
      if (control != null)
        if (_controlRuleSources.TryGetValue(control, out result))
          return result.SetFont;

      //DevExpress.XtraBars.BarManager barManager = source as DevExpress.XtraBars.BarManager;
      //if (barManager != null)
      //  if (_barManagerRuleSources.TryGetValue(barManager, out result))
      //    return result.SetFont;

      if (_ruleSources.TryGetValue(source, out result))
        return result.SetFont;

      return true;
    }

    /// <summary>
    /// 统一设置控件Font
    /// </summary>
    public void SetSetFont(Component source, bool value)
    {
      RuleStatus status;

      Control control = source as Control;
      if (control != null)
      {
        if (_controlRuleSources.TryGetValue(control, out status))
          status.SetFont = value;
        else
          _controlRuleSources.Add(control, new RuleStatus {SetFont = value});
      }

      //DevExpress.XtraBars.BarManager barManager = source as DevExpress.XtraBars.BarManager;
      //if (barManager != null)
      //{
      //  if (_barManagerRuleSources.TryGetValue(barManager, out status))
      //    status.SetFont = value;
      //  else
      //    _barManagerRuleSources.Add(barManager, new RuleStatus { SetFont = value });
      //}

      if (_ruleSources.TryGetValue(source, out status))
        status.SetFont = value;
      else
        _ruleSources.Add(source, new RuleStatus { SetFont = value });
    }

    /// <summary>
    /// 统一设置控件BackColor
    /// </summary>
    [Description("统一设置控件BackColor"), Category("Phenix")]
    public bool GetSetBackColor(Component source)
    {
      RuleStatus result;

      Control control = source as Control;
      if (control != null)
        if (_controlRuleSources.TryGetValue(control, out result))
        return result.SetBackColor;
      
      //DevExpress.XtraBars.BarManager barManager = source as DevExpress.XtraBars.BarManager;
      //if (barManager != null)
      //  if (_barManagerRuleSources.TryGetValue(barManager, out result))
      //    return result.SetBackColor;

      if (_ruleSources.TryGetValue(source, out result))
        return result.SetBackColor;

      return true;
    }

    /// <summary>
    /// 统一设置控件BackColor
    /// </summary>
    public void SetSetBackColor(Component source, bool value)
    {
      RuleStatus status;

      Control control = source as Control;
      if (control != null)
      {
        if (_controlRuleSources.TryGetValue(control, out status))
          status.SetBackColor = value;
        else
          _controlRuleSources.Add(control, new RuleStatus { SetBackColor = value });
      }

      //DevExpress.XtraBars.BarManager barManager = source as DevExpress.XtraBars.BarManager;
      //if (barManager != null)
      //{
      //  if (_barManagerRuleSources.TryGetValue(barManager, out status))
      //    status.SetBackColor = value;
      //  else
      //    _barManagerRuleSources.Add(barManager, new RuleStatus { SetBackColor = value });
      //}

      if (_ruleSources.TryGetValue(source, out status))
        status.SetBackColor = value;
      else
        _ruleSources.Add(source, new RuleStatus { SetBackColor = value });
    }

    /// <summary>
    /// 统一设置控件ForeColor
    /// </summary>
    [Description("统一设置控件ForeColor"), Category("Phenix")]
    public bool GetSetForeColor(Component source)
    {
      RuleStatus result;

      Control control = source as Control;
      if (control != null)
        if (_controlRuleSources.TryGetValue(control, out result))
          return result.SetForeColor;
      
      //DevExpress.XtraBars.BarManager barManager = source as DevExpress.XtraBars.BarManager;
      //if (barManager != null)
      //  if (_barManagerRuleSources.TryGetValue(barManager, out result))
      //    return result.SetForeColor;

      if (_ruleSources.TryGetValue(source, out result))
        return result.SetForeColor;

      return true;
    }

    /// <summary>
    /// 统一设置控件ForeColor
    /// </summary>
    public void SetSetForeColor(Component source, bool value)
    {
      RuleStatus status;

      Control control = source as Control;
      if (control != null)
      {
        if (_controlRuleSources.TryGetValue(control, out status))
          status.SetForeColor = value;
        else
          _controlRuleSources.Add(control, new RuleStatus { SetForeColor = value });
      }

      //DevExpress.XtraBars.BarManager barManager = source as DevExpress.XtraBars.BarManager;
      //if (barManager != null)
      //{
      //  if (_barManagerRuleSources.TryGetValue(barManager, out status))
      //    status.SetForeColor = value;
      //  else
      //    _barManagerRuleSources.Add(barManager, new RuleStatus { SetForeColor = value });
      //}

      if (_ruleSources.TryGetValue(source, out status))
        status.SetForeColor = value;
      else
        _ruleSources.Add(source, new RuleStatus { SetForeColor = value });
    }

    /// <summary>
    /// 统一设置控件RightToLeft
    /// </summary>
    [Description("统一设置控件RightToLeft"), Category("Phenix")]
    public bool GetSetRightToLeft(Component source)
    {
      RuleStatus result;

      Control control = source as Control;
      if (control != null)
        if (_controlRuleSources.TryGetValue(control, out result))
          return result.SetRightToLeft;

      //DevExpress.XtraBars.BarManager barManager = source as DevExpress.XtraBars.BarManager;
      //if (barManager != null)
      //  if (_barManagerRuleSources.TryGetValue(barManager, out result))
      //    return result.SetRightToLeft;

      if (_ruleSources.TryGetValue(source, out result))
        return result.SetRightToLeft;

      return true;
    }

    /// <summary>
    /// 统一设置控件RightToLeft
    /// </summary>
    public void SetSetRightToLeft(Component source, bool value)
    {
      RuleStatus status;

      Control control = source as Control;
      if (control != null)
      {
        if (_controlRuleSources.TryGetValue(control, out status))
          status.SetRightToLeft = value;
        else
          _controlRuleSources.Add(control, new RuleStatus { SetRightToLeft = value });
      }


      //DevExpress.XtraBars.BarManager barManager = source as DevExpress.XtraBars.BarManager;
      //if (barManager != null)
      //{
      //  if (_barManagerRuleSources.TryGetValue(barManager, out status))
      //    status.SetRightToLeft = value;
      //  else
      //    _barManagerRuleSources.Add(barManager, new RuleStatus { SetRightToLeft = value });
      //}

      if (_ruleSources.TryGetValue(source, out status))
        status.SetRightToLeft = value;
      else
        _ruleSources.Add(source, new RuleStatus { SetRightToLeft = value });
    }

    #endregion

    #region 事件

    private void Host_Shown(object sender, EventArgs e)
    {
      InitializeRule();
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
      if (extendee is Control)
        return true;
      //for Developer Express .NET
      //if (extendee is DevExpress.XtraBars.BarManager)
      //  return true;
      foreach (PropertyInfo item in extendee.GetType().GetProperties())
        if (!item.PropertyType.IsValueType && item.CanRead && item.GetGetMethod() != null &&
          (item.PropertyType == typeof(DevExpress.Utils.AppearanceObject) || item.PropertyType.IsSubclassOf(typeof(DevExpress.Utils.AppearanceObject))))
        {
          DevExpress.Utils.AppearanceObject appearanceObject = item.GetValue(extendee, null) as DevExpress.Utils.AppearanceObject;
          if (appearanceObject != null)
           return true;
        }
      //for other
      System.Reflection.PropertyInfo fontInfo = extendee.GetType().GetProperty(FONT_NAME);
      System.Reflection.PropertyInfo backColorInfo = extendee.GetType().GetProperty(BACK_COLOR_NAME);
      System.Reflection.PropertyInfo foreColorInfo = extendee.GetType().GetProperty(FORE_COLOR_NAME);
      return ((fontInfo != null && fontInfo.CanWrite && fontInfo.GetSetMethod() != null && !fontInfo.GetSetMethod().IsStatic)
        && (backColorInfo != null && backColorInfo.CanWrite && backColorInfo.GetSetMethod() != null && !backColorInfo.GetSetMethod().IsStatic)
        && (foreColorInfo != null && foreColorInfo.CanWrite && foreColorInfo.GetSetMethod() != null && !foreColorInfo.GetSetMethod().IsStatic));
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
      Font font = Font;
      Color backColor = BackColor;
      Color foreColor = ForeColor;
      RightToLeft rightToLeft = RightToLeft;
      foreach (KeyValuePair<Control, RuleStatus> kvp in _controlRuleSources)
        InitializeRule(kvp.Key, kvp.Value, font, backColor, foreColor, rightToLeft);
      //foreach (KeyValuePair<DevExpress.XtraBars.BarManager, RuleStatus> kvp in _barManagerRuleSources)
      //  InitializeRule(kvp.Key, kvp.Value, font, backColor, foreColor);
      foreach (KeyValuePair<Component, RuleStatus> kvp in _ruleSources)
        InitializeRule(kvp.Key, kvp.Value, font, backColor, foreColor);
    }

    private static void InitializeRule(Control control, RuleStatus ruleStatus, Font font, Color backColor, Color foreColor, RightToLeft rightToLeft)
    {
      if (ruleStatus.SetFont && !SystemFonts.DefaultFont.Equals(font))
        control.Font = font;
      if (ruleStatus.SetBackColor && backColor.ToArgb() != Color.Empty.ToArgb())
        control.BackColor = backColor;
      if (ruleStatus.SetForeColor && foreColor.ToArgb() != Color.Empty.ToArgb())
        control.ForeColor = foreColor;
      if (ruleStatus.SetRightToLeft && rightToLeft != RightToLeft.Inherit)
        control.RightToLeft = rightToLeft;
    }

    ////for Developer Express .NET
    //private static void InitializeRule(DevExpress.XtraBars.BarManager barManager, RuleStatus ruleStatus, Font font, Color backColor, Color foreColor)
    //{
    //  foreach (DevExpress.XtraBars.Bar item in barManager.Bars)
    //  {
    //    if (ruleStatus.SetFont && !SystemFonts.DefaultFont.Equals(font))
    //    {
    //      item.BarAppearance.Hovered.Font = font;
    //      item.BarAppearance.Normal.Font = font;
    //      item.BarAppearance.Pressed.Font = font;
    //      item.BarAppearance.Disabled.Font = font;
    //    }
    //    if (ruleStatus.SetBackColor && backColor.ToArgb() != Color.Empty.ToArgb())
    //    {
    //      item.BarAppearance.Hovered.BackColor = backColor;
    //      item.BarAppearance.Normal.BackColor = backColor;
    //      item.BarAppearance.Pressed.BackColor = backColor;
    //      item.BarAppearance.Disabled.BackColor = backColor;
    //    }
    //    if (ruleStatus.SetForeColor && foreColor.ToArgb() != Color.Empty.ToArgb())
    //    {
    //      item.BarAppearance.Hovered.ForeColor = foreColor;
    //      item.BarAppearance.Normal.ForeColor = foreColor;
    //      item.BarAppearance.Pressed.ForeColor = foreColor;
    //      item.BarAppearance.Disabled.ForeColor = foreColor;
    //    }
    //  }
    //}

    //for Developer Express .NET
    private static void InitializeRule(DevExpress.Utils.AppearanceObject appearanceObject, RuleStatus ruleStatus, Font font, Color backColor, Color foreColor)
    {
      if (ruleStatus.SetFont && !SystemFonts.DefaultFont.Equals(font))
        appearanceObject.Font = font;
      if (ruleStatus.SetBackColor && backColor.ToArgb() != Color.Empty.ToArgb())
        appearanceObject.BackColor = backColor;
      if (ruleStatus.SetForeColor && foreColor.ToArgb() != Color.Empty.ToArgb())
        appearanceObject.ForeColor = foreColor;
    }

    private static void InitializeRule(Component component, RuleStatus ruleStatus, Font font, Color backColor, Color foreColor)
    {
      //for Developer Express .NET
       foreach (PropertyInfo item in component.GetType().GetProperties())
         if (!item.PropertyType.IsValueType && item.CanRead && item.GetGetMethod() != null &&
           (item.PropertyType == typeof(DevExpress.Utils.AppearanceObject) || item.PropertyType.IsSubclassOf(typeof(DevExpress.Utils.AppearanceObject))))
         {
           DevExpress.Utils.AppearanceObject appearanceObject = item.GetValue(component, null) as DevExpress.Utils.AppearanceObject;
           if (appearanceObject != null)
           {
             InitializeRule(appearanceObject, ruleStatus, font, backColor, foreColor);
             return;
           }
         }
      //for other
      if (ruleStatus.SetFont && !SystemFonts.DefaultFont.Equals(font))
        component.GetType().GetProperty(FONT_NAME).SetValue(component, font, null);
      if (ruleStatus.SetBackColor && backColor.ToArgb() != Color.Empty.ToArgb())
        component.GetType().GetProperty(BACK_COLOR_NAME).SetValue(component, backColor, null);
      if (ruleStatus.SetForeColor && foreColor.ToArgb() != Color.Empty.ToArgb())
        component.GetType().GetProperty(FORE_COLOR_NAME).SetValue(component, foreColor, null);
    }

    /// <summary>
    /// 重置规则
    /// </summary>
    public bool ResetRule(Control control)
    {
      foreach (KeyValuePair<Control, RuleStatus> kvp in _controlRuleSources)
        if (kvp.Key == control)
        {
          InitializeRule(kvp.Key, kvp.Value, Font, BackColor, ForeColor, RightToLeft);
          return true;
        }
      return false;
    }

    ///// <summary>
    ///// 重置规则
    ///// </summary>
    //public bool ResetRule(DevExpress.XtraBars.BarManager barManager)
    //{
    //  foreach (KeyValuePair<DevExpress.XtraBars.BarManager, RuleStatus> kvp in _barManagerRuleSources)
    //    if (kvp.Key == barManager)
    //    {
    //      InitializeRule(kvp.Key, kvp.Value, Font, BackColor, ForeColor);
    //      return true;
    //    }
    //  return false;
    //}

    /// <summary>
    /// 重置规则
    /// </summary>
    public bool ResetRule(Component component)
    {
      foreach (KeyValuePair<Component, RuleStatus> kvp in _ruleSources)
        if (kvp.Key == component)
        {
          InitializeRule(kvp.Key, kvp.Value, Font, BackColor, ForeColor);
          return true;
        }
      return false;
    }

    #endregion

    #region 内嵌类

    [Serializable]
    private class RuleStatus
    {
      private bool _setFont = true;
      public bool SetFont
      {
        get { return _setFont; }
        set { _setFont = value; }
      }

      private bool _setBackColor = true;
      public bool SetBackColor
      {
        get { return _setBackColor; }
        set { _setBackColor = value; }
      }

      private bool _setForeColor = true;
      public bool SetForeColor
      {
        get { return _setForeColor; }
        set { _setForeColor = value; }
      }

      private bool _setRightToLeft = true;
      public bool SetRightToLeft
      {
        get { return _setRightToLeft; }
        set { _setRightToLeft = value; }
      }
    }

    #endregion
  }
}  