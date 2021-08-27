using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Windows.Forms;
using DevExpress.Utils;
using DevExpress.XtraVerticalGrid;
using Phenix.Core;
using Phenix.Core.Log;

namespace Phenix.Windows.Helper
{
  /// <summary>
  /// VGridControl助手
  /// </summary>
  public static class VGridControlHelper
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
    /// 检索第一个匹配VGridControl
    /// </summary>
    /// <param name="grids">VGridControl队列</param>
    /// <param name="source">数据源</param>
    public static VGridControl FindGridControl(IList<VGridControl> grids, BindingSource source)
    {
      if (grids == null || grids.Count == 0 || source == null)
        return null;
      foreach (VGridControl item in grids)
        if (item.DataSource == source)
          return item;
      return null;
    }

    /// <summary>
    /// 搜索第一个匹配的VGridControl，并定位焦点
    /// </summary>
    /// <param name="grids">VGridControl队列</param>
    /// <param name="source">数据源</param>
    /// <returns>被定位焦点的VGridControl</returns>
    public static VGridControl FocusGridControl(IList<VGridControl> grids, BindingSource source)
    {
      if (grids == null || grids.Count == 0 || source == null)
        return null;
      foreach (VGridControl item in grids)
        if (item.DataSource == source && item.Enabled && item.Visible && item.CanFocus && item.Rows.Count > 0)
        {
          item.Focus();
          return item;
        }
      return null;
    }

    /// <summary>
    /// 搜索第一个匹配的可编辑VGridControl，并定位焦点
    /// </summary>
    /// <param name="grids">VGridControl队列</param>
    /// <param name="source">数据源</param>
    /// <returns>被定位焦点的VGridControl</returns>
    public static VGridControl FocusEditableGridControl(IList<VGridControl> grids, BindingSource source)
    {
      if (grids == null || grids.Count == 0 || source == null)
        return null;
      foreach (VGridControl item in grids)
        if (item.DataSource == source && item.Enabled && item.Visible && item.CanFocus 
          && item.OptionsBehavior.Editable && item.Rows.Count > 0)
        {
          item.Focus();
          return item;
        }
      return null;
    }

    #region Export

    /// <summary>
    /// 导出报表
    /// </summary>
    /// <param name="grid">GridControl</param>
    public static bool Export(VGridControlBase grid)
    {
      return Export(grid, Phenix.Windows.Properties.Resources.DataExport, Phenix.Windows.Properties.Resources.GridExportFilter);
    }

    /// <summary>
    /// 导出报表
    /// </summary>
    /// <param name="grid">GridControl</param>
    /// <param name="title">Title</param>
    public static bool Export(VGridControlBase grid, string title)
    {
      return Export(grid, title, Phenix.Windows.Properties.Resources.GridExportFilter);
    }

    /// <summary>
    /// 导出报表
    /// </summary>
    /// <param name="grid">VGridControlBase</param>
    /// <param name="title">Title</param>
    /// <param name="filter">Filter</param>
    public static bool Export(VGridControlBase grid, string title, string filter)
    {
      if (grid == null)
        return false;
      using (SaveFileDialog saveFileDialog = new SaveFileDialog())
      {
        saveFileDialog.RestoreDirectory = true;
        saveFileDialog.Title = title;
        saveFileDialog.Filter = filter;
        do
        {
          try
          {
            if (saveFileDialog.ShowDialog() != DialogResult.OK)
              return false;
            string path = saveFileDialog.FileName;
            string fileType = path.Substring(path.LastIndexOf(".") + 1, path.Length - 1 - path.LastIndexOf(".")).ToLower();
            using (new DevExpress.Utils.WaitDialogForm(String.Format(Properties.Resources.DataExporting, saveFileDialog.FileName), Phenix.Core.Properties.Resources.PleaseWait))
            {
              switch (fileType)
              {
                case "xls":
                  grid.ExportToXls(saveFileDialog.FileName);
                  break;
                case "txt":
                  grid.ExportToText(saveFileDialog.FileName);
                  break;
                case "html":
                  grid.ExportToHtml(saveFileDialog.FileName);
                  break;
                default:
                  grid.ExportToXls(saveFileDialog.FileName);
                  break;
              }
            }
            if (MessageBox.Show(String.Format(Phenix.Windows.Properties.Resources.DataExportOpenFile, saveFileDialog.FileName),
              Phenix.Windows.Properties.Resources.ToolBarExportBodyTip, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
              try
              {
                System.Diagnostics.Process.Start(saveFileDialog.FileName);
              }
              catch (System.ComponentModel.Win32Exception ex)
              {
                MessageBox.Show(String.Format(Phenix.Windows.Properties.Resources.OpenFileAborted, saveFileDialog.FileName, AppUtilities.GetErrorHint(ex)),
                  saveFileDialog.Title, MessageBoxButtons.OK, MessageBoxIcon.Error);
              }
            return true;
          }
          catch (TargetInvocationException ex)
          {
            if (MessageBox.Show(String.Format(Phenix.Windows.Properties.Resources.DataExportAborted, saveFileDialog.FileName, AppUtilities.GetErrorHint(ex)),
              saveFileDialog.Title, MessageBoxButtons.YesNo, MessageBoxIcon.Error) != DialogResult.Yes)
              return false;
          }
        } while (true);
      }
    }

    #endregion

    #region Layout

    private static string LayoutPathName(Control container, VGridControl grid)
    {
      string result = String.Empty;
      string location = container.GetType().Assembly.Location;
      if (File.Exists(location))
      {
        FileInfo fileInfo = new FileInfo(location);
        result = String.Format("{0}{1}{2}", result, fileInfo.Length, fileInfo.LastWriteTime);
      }
      result = String.Format("{0}.{1}.{2}.{3}", result, container.Name, grid.Name, grid.Rows.Count);
      return Path.Combine(LocalCacheDirectory, Path.GetFileNameWithoutExtension(location) + result.GetHashCode().ToString());
    }

    private static string OriginalLayoutPathName(Control container, VGridControl grid)
    {
      return LayoutPathName(container, grid) + ".Origin";
    }

    /// <summary>
    /// 保存布局
    /// </summary>
    /// <param name="container">控件容器</param>
    /// <param name="grid">VGridControl</param>
    public static void SaveLayout(Control container, VGridControl grid)
    {
      if (grid == null)
        throw new ArgumentNullException("grid");
      grid.SaveLayoutToXml(LayoutPathName(container, grid), OptionsLayoutBase.FullLayout);
      string pathName = OriginalLayoutPathName(container, grid);
      if (!File.Exists(pathName))
        grid.SaveLayoutToXml(pathName, OptionsLayoutBase.FullLayout);
    }

    /// <summary>
    /// 恢复布局
    /// </summary>
    /// <param name="container">控件容器</param>
    /// <param name="grid">VGridControl</param>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
    public static bool RestoreLayout(Control container, VGridControl grid)
    {
      if (grid == null)
        throw new ArgumentNullException("grid");
      try
      {
        string pathName = LayoutPathName(container, grid);
        if (File.Exists(pathName))
        {
          grid.RestoreLayoutFromXml(pathName, OptionsLayoutBase.FullLayout);
          return true;
        }
        else
          return ResetLayout(container, grid);
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
    /// <param name="grid">VGridControl</param>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
    public static bool ResetLayout(Control container, VGridControl grid)
    {
      if (grid == null)
        throw new ArgumentNullException("grid");
      try
      {
        string pathName = OriginalLayoutPathName(container, grid);
        if (File.Exists(pathName))
        {
          grid.RestoreLayoutFromXml(pathName, OptionsLayoutBase.FullLayout);
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

    #region 编辑管理

    /// <summary>
    /// 交付编辑
    /// </summary>
    /// <param name="grids">GridControl队列</param>
    public static void PostEditor(IList<VGridControl> grids)
    {
      foreach (VGridControl item in grids)
      {
        item.PostEditor();
        item.UpdateFocusedRecord();
      }
    }

    #endregion

    #endregion
  }
}