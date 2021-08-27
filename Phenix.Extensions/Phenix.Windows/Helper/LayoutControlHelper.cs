using System;
using System.IO;
using System.Reflection;
using System.Windows.Forms;
using DevExpress.XtraDataLayout;
using DevExpress.XtraLayout;
using Phenix.Core;
using Phenix.Core.Log;
using Phenix.Core.Windows;

namespace Phenix.Windows.Helper
{
  /// <summary>
  /// LayoutControl助手
  /// </summary>
  public static class LayoutControlHelper
  {
    #region 属性

    private static string _localCacheDirectory;
    /// <summary>
    /// 本地缓存目录
    /// </summary>
    public static string LocalCacheDirectory
    {
      get
      {
        if (String.IsNullOrEmpty(_localCacheDirectory))
        {
          _localCacheDirectory = Path.Combine(AppConfig.BaseDirectory, AppSettings.DefaultKey + "Layout");
          if (!Directory.Exists(_localCacheDirectory))
            Directory.CreateDirectory(_localCacheDirectory);
        }
        return _localCacheDirectory;
      }
    }

    #endregion

    #region 方法

    /// <summary>
    /// 取控件关联的LayoutControlItem
    /// </summary>
    /// <param name="control">控件</param>
    public static LayoutControlItem FindLayoutControlItem(Control control)
    {
      DataLayoutControl dataLayoutControl = control.Parent as DataLayoutControl;
      if (dataLayoutControl != null)
        foreach (LayoutControlItem item in dataLayoutControl.Items)
          if (item.Control == control)
            return item;
      return null;
    }

    #region Layout

    private static string LayoutPathName(Control container, LayoutControl layout)
    {
      string result = String.Empty;
      string location = container.GetType().Assembly.Location;
      if (File.Exists(location))
      {
        FileInfo fileInfo = new FileInfo(location);
        result = result +
          fileInfo.Length.ToString() + fileInfo.LastWriteTime.ToString("u");
      }
      result = result +
        '.' + container.Name + '.' + layout.Name;
      return Path.Combine(LocalCacheDirectory, Path.GetFileNameWithoutExtension(location) + result.GetHashCode().ToString());
    }

    private static string OriginalLayoutPathName(Control container, LayoutControl layout)
    {
      return LayoutPathName(container, layout) + ".Origin";
    }

    /// <summary>
    /// 保存布局
    /// </summary>
    /// <param name="container">控件容器</param>
    public static void SaveLayout(Control container)
    {
      foreach (LayoutControl item in ControlHelper.FindControls<LayoutControl>(container))
        SaveLayout(container, item);
    }

    /// <summary>
    /// 保存布局
    /// </summary>
    /// <param name="container">控件容器</param>
    /// <param name="layout">layout</param>
    public static void SaveLayout(Control container, LayoutControl layout)
    {
      if (layout == null)
        throw new ArgumentNullException("layout");
      layout.SaveLayoutToXml(LayoutPathName(container, layout));
      if (!File.Exists(OriginalLayoutPathName(container, layout)))
        layout.SaveLayoutToXml(OriginalLayoutPathName(container, layout));
    }

    /// <summary>
    /// 恢复布局
    /// </summary>
    /// <param name="container">控件容器</param>
    public static bool RestoreLayout(Control container)
    {
      bool result = true;
      foreach (LayoutControl item in ControlHelper.FindControls<LayoutControl>(container))
        if (!RestoreLayout(container, item))
          result = false;
      return result;
    }

    /// <summary>
    /// 恢复布局
    /// </summary>
    /// <param name="container">控件容器</param>
    /// <param name="layout">layout</param>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
    public static bool RestoreLayout(Control container, LayoutControl layout)
    {
      if (layout == null)
        throw new ArgumentNullException("layout");
      try
      {
        if (File.Exists(LayoutPathName(container, layout)))
        {
          layout.RestoreLayoutFromXml(LayoutPathName(container, layout));
          return true;
        }
        else
          return ResetLayout(container, layout);
      }
      catch (Exception ex)
      {
        EventLog.SaveLocal(MethodBase.GetCurrentMethod(), ex);
      }
      return false;
    }

    /// <summary>
    /// 重置布局
    /// </summary>
    /// <param name="container">控件容器</param>
    /// <param name="layout">layout</param>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
    public static bool ResetLayout(Control container, LayoutControl layout)
    {
      if (layout == null)
        throw new ArgumentNullException("layout");
      try
      {
        if (File.Exists(OriginalLayoutPathName(container, layout)))
        {
          layout.RestoreLayoutFromXml(OriginalLayoutPathName(container, layout));
          return true;
        }
      }
      catch (Exception ex)
      {
        EventLog.SaveLocal(MethodBase.GetCurrentMethod(), ex);
      }
      return false;
    }

    #endregion

    #endregion
  }
}