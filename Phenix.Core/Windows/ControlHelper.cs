using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Phenix.Core.Mapping;

namespace Phenix.Core.Windows
{
  /// <summary>
  /// Control助手
  /// </summary>
  public static class ControlHelper
  {
    #region 属性

    private static readonly object _findViewControlsLock = new object();
    private static Control _findViewControlsLastContainer;
    private static BindingSource _findViewControlsLastSource;
    private static List<Control> _findViewControlsLastControls;

    private static readonly object _findEditControlsLock = new object();
    private static Control _findEditControlsLastContainer;
    private static BindingSource _findEditControlsLastSource;
    private static List<Control> _findEditControlsLastControls;

    #endregion

    #region 方法

    /// <summary>
    /// 设置焦点
    /// </summary>
    /// <param name="control">控件</param>
    public static void SetFocus(Control control)
    {
      if (control == null)
        return;
      if (control.CanFocus)
        control.Focus();
    }

    /// <summary>
    /// 设置焦点（线程安全）
    /// </summary>
    /// <param name="control">控件</param>
    public static void InvokeSetFocus(Control control)
    {
      if (control == null)
        return;
      if (control.InvokeRequired)
        control.BeginInvoke(new Action<Control>(SetFocus), new object[] { control });
      else
        SetFocus(control); ;
    }

    /// <summary>
    /// 重置焦点
    /// </summary>
    public static void ResetFocus(Control control)
    {
      if (control == null)
        return;
      if (control is Form)
      {
        Form from = (Form)control;
        Control activeControl = from.ActiveControl;
        from.ActiveControl = null;
        from.ActiveControl = activeControl;
      }
      else
      {
        Form from = control.FindForm();
        if (from != null)
          from.ActiveControl = null;
        control.Focus();
      }
    }

    /// <summary>
    /// 检索指定类型T的全部控件
    /// </summary>
    /// <param name="container">控件容器</param>
    /// <returns>匹配的控件队列</returns>
    public static IList<T> FindControls<T>(Control container)
      where T : Control
    {
      List<T> result = new List<T>();
      if (container is T)
        result.Add((T)container);
      if (container != null)
        foreach (Control item in container.Controls)
          result.AddRange(FindControls<T>(item));
      return result;
    }

    /// <summary>
    /// 匹配指定控件的标签控件
    /// </summary>
    /// <param name="control">控件</param>
    /// <returns>匹配的标签控件</returns>
    public static Label FindFriendlyCaption(Control control)
    {
      if (control == null)
        return null;
      Control container = control.Parent;
      if (container == null)
        return null;
      IList<Label> labels = FindControls<Label>(container);
      if (labels.Count == 0)
        return null;
      foreach (Binding item in control.DataBindings)
      {
        if (String.IsNullOrEmpty(item.BindingMemberInfo.BindingField))
          continue;
        foreach (Label label in labels)
          if (!String.IsNullOrEmpty(label.Name))
          {
            string s = label.Name.TrimEnd('0', '1', '2', '3', '4', '5', '6', '7', '8', '9');
            if (String.CompareOrdinal(s.Substring(s.Length - 5), "Label") == 0)
              if (String.Compare(s.Remove(s.Length - 5), item.BindingMemberInfo.BindingField, StringComparison.OrdinalIgnoreCase) == 0)
                return label;
          }
      }
      return null;
    }

    #region ViewControl

    private static void FindViewControlTabs(Control control, BindingSource source, string parentKey,
      ref Dictionary<string, Control> tabControls)
    {
      if (control == null)
        return;
      string key;
      //找唯一键
      do
      {
        key = parentKey + control.TabIndex.ToString("D10");
        if (!tabControls.ContainsKey(key))
          break;
        control.TabIndex = control.TabIndex + 1;
      } while (true);
      //找符合条件的控件
      BindingSource bindingSource = BindingSourceHelper.GetDataSource(control);
      if (bindingSource != null && (source == null || source == bindingSource))
        tabControls.Add(key, control);
      //在子控件中找
      foreach (Control item in control.Controls)
        FindViewControlTabs(item, source, key + '.', ref tabControls);
    }

    /// <summary>
    /// 检索绑定到指定BindingSource的全部视图控件
    /// 返回值按照TabIndex排序
    /// </summary>
    /// <param name="container">控件容器</param>
    /// <param name="source">数据源</param>
    /// <returns>匹配的控件队列</returns>
    public static IList<Control> FindViewControls(Control container, BindingSource source)
    {
      lock (_findViewControlsLock)
      {
        if (container != _findViewControlsLastContainer || source != _findViewControlsLastSource)
        {
          _findViewControlsLastContainer = container;
          _findViewControlsLastSource = source;
          Dictionary<string, Control> tabControls = new Dictionary<string, Control>();
          FindViewControlTabs(container, source, String.Empty, ref tabControls);
          _findViewControlsLastControls = tabControls.Count > 0 ? new List<Control>(tabControls.Values) : new List<Control>(0);
        }
        return _findViewControlsLastControls;
      }
    }
    
    #endregion

    #region EditControl

    private static void FindEditControlTabs(Control control, BindingSource source, string parentKey,
      ref Dictionary<string, Control> tabControls)
    {
      if (control == null)
        return;
      string key;
      //找唯一键
      do
      {
        key = parentKey + control.TabIndex.ToString("D10");
        if (!tabControls.ContainsKey(key))
          break;
        control.TabIndex = control.TabIndex + 1;
      } while (true);
      //找符合条件的控件
      foreach (Binding item in control.DataBindings)
        if (item.DataSource != null && (source == null || source == item.DataSource))
        {
          tabControls.Add(key, control);
          break;
        }
      //在子控件中找
      foreach (Control item in control.Controls)
        FindEditControlTabs(item, source, key + '.', ref tabControls);
    }

    /// <summary>
    /// 检索绑定到指定BindingSource的全部编辑控件
    /// 返回值按照TabIndex排序
    /// </summary>
    /// <param name="container">控件容器</param>
    /// <param name="source">数据源</param>
    /// <returns>匹配的编辑控件队列</returns>
    public static IList<Control> FindEditControls(Control container, BindingSource source)
    {
      lock (_findEditControlsLock)
      {
        if (container != _findEditControlsLastContainer || source != _findEditControlsLastSource)
        {
          _findEditControlsLastContainer = container;
          _findEditControlsLastSource = source;
          Dictionary<string, Control> tabControls = new Dictionary<string, Control>();
          FindEditControlTabs(container, source, String.Empty, ref tabControls);
          _findEditControlsLastControls = tabControls.Count > 0 ? new List<Control>(tabControls.Values) : new List<Control>(0);
        }
        return _findEditControlsLastControls;
      }
    }

    /// <summary>
    /// 为编辑控件赋值
    /// </summary>
    /// <param name="control">控件</param>
    /// <param name="value">值</param>
    public static void SetEditControlValue(Control control, object value)
    {
      if (control == null)
        return;
      foreach (Binding item in control.DataBindings)
      {
        if (!String.IsNullOrEmpty(item.BindingMemberInfo.BindingField))
        {
          BindingSource bindingSource = item.DataSource as BindingSource;
          if (bindingSource != null)
          {
            object obj = BindingSourceHelper.GetDataSourceCurrent(bindingSource);
            if (obj != null)
            {
              System.Reflection.PropertyInfo objPropertyInfo = obj.GetType().GetProperty(item.BindingMemberInfo.BindingField);
              if (objPropertyInfo != null)
              {
                objPropertyInfo.SetValue(obj, value, null);
                bindingSource.ResetBindings(false);
                break;
              }
            }
          }
        }
        System.Reflection.PropertyInfo propertyInfo = control.GetType().GetProperty(item.PropertyName);
        if (propertyInfo != null)
        {
          propertyInfo.SetValue(control, value, null);
          break;
        }
      }
    }

    #endregion

    #region CriteriaControl

    /// <summary>
    /// 保存条件数据源的记录
    /// </summary>
    /// <param name="container">控件容器</param>
    /// <param name="criteriaSource">条件数据源</param>
    /// <param name="userSource">使用数据源</param>
    public static void SaveCriteriaBindingSourceData(Control container,
      BindingSource criteriaSource, BindingSource userSource)
    {
      if (container == null ||
        criteriaSource == null || criteriaSource.DataSource == null || criteriaSource.DataSource is Type)
        return;
      Type userType = null;
      if (userSource != null && userSource.DataSource != null)
        userType = userSource.DataSource as Type ?? userSource.DataSource.GetType();
      CriteriaHelper.SaveCriteriaInfo(container.Name, criteriaSource.DataSource, userType);
    }

    /// <summary>
    /// 恢复条件数据源的记录
    /// </summary>
    /// <param name="container">控件容器</param>
    /// <param name="criteriaSource">条件数据源</param>
    /// <param name="userSource">使用数据源</param>
    /// <param name="reset">是否强制覆盖, true表示虽然条件数据源已有数据也将被备份数据覆盖</param>
    public static void RestoreCriteriaBindingSourceData(Control container,
      BindingSource criteriaSource, BindingSource userSource, bool reset)
    {
      if (container == null || criteriaSource == null)
        return;
      Type type = criteriaSource.DataSource as Type;
      if (type != null)
        criteriaSource.DataSource = Activator.CreateInstance(type, true);
      if (type != null || (reset && criteriaSource.DataSource != null))
      {
        Type userType = null;
        if (userSource != null && userSource.DataSource != null)
          userType = userSource.DataSource as Type ?? userSource.DataSource.GetType();
        CriteriaHelper.RestoreCriteriaInfo(container.Name, criteriaSource.DataSource, userType);
        criteriaSource.ResetBindings(false);
      }
    }
 
    #endregion

   #endregion
  }
}