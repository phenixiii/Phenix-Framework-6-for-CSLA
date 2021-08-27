using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Windows.Forms;
using DevExpress.Utils;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Views.Base;
using Phenix.Business;
using Phenix.Core;
using Phenix.Core.Log;
using Phenix.Core.Reflection;
using Phenix.Core.Windows;

namespace Phenix.Windows.Helper
{
  /// <summary>
  /// GridControl助手
  /// </summary>
  public static class GridControlHelper
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
    /// 检索第一个匹配GridControl
    /// </summary>
    /// <param name="grids">GridControl队列</param>
    /// <param name="source">数据源</param>
    public static GridControl FindGridControl(IList<GridControl> grids, BindingSource source)
    {
      if (grids == null || grids.Count == 0 || source == null)
        return null;
      IBusinessCollection sourceList = BindingSourceHelper.GetDataSourceList(source) as IBusinessCollection;
      foreach (GridControl gridControl in grids)
      {
        if (gridControl.DataSource == source)
          return gridControl;
        foreach (BaseView baseView in gridControl.ViewCollection)
          if (baseView != gridControl.MainView)
          {
            ColumnView columnView = baseView as ColumnView;
            if (columnView != null)
            {
              IBusinessObject business = columnView.GetRow(columnView.FocusedRowHandle) as IBusinessObject;
              if (business != null && business.Owner == sourceList)
                return gridControl;
            }
          }
      }
      return null;
    }

    /// <summary>
    /// 搜索第一个匹配的GridControl，并定位列焦点
    /// </summary>
    /// <param name="grids">GridControl队列</param>
    /// <param name="source">数据源</param>
    /// <returns>被定位焦点的GridControl</returns>
    public static GridControl FocusColumn(IList<GridControl> grids, BindingSource source)
    {
      if (grids == null || grids.Count == 0 || source == null)
        return null;
      IBusinessCollection sourceList = BindingSourceHelper.GetDataSourceList(source) as IBusinessCollection;
      foreach (GridControl item in grids)
        if (item.Enabled && item.Visible && item.CanFocus)
        {
          ColumnView columnView = item.FocusedView as ColumnView;
          if (columnView != null && columnView.Columns.Count > 0)
          {
            IBusinessObject business = columnView.GetRow(columnView.FocusedRowHandle) as IBusinessObject;
            if (business != null && business.Owner == sourceList)
            {
              columnView.FocusedColumn = columnView.Columns[0];
              return item;
            }
          }
        }
      return null;
    }

    /// <summary>
    /// 搜索第一个匹配的GridControl，并定位可编辑的列焦点
    /// </summary>
    /// <param name="grids">GridControl队列</param>
    /// <param name="source">数据源</param>
    /// <returns>被定位焦点的GridControl</returns>
    public static GridControl FocusEditableColumn(IList<GridControl> grids, BindingSource source)
    {
      if (grids == null || grids.Count == 0 || source == null)
        return null;
      IBusinessCollection sourceList = BindingSourceHelper.GetDataSourceList(source) as IBusinessCollection;
      foreach (GridControl item in grids)
        if (item.Enabled && item.Visible && item.CanFocus)
        {
          ColumnView columnView = item.FocusedView as ColumnView;
          if (columnView != null && columnView.OptionsBehavior.Editable && !columnView.OptionsBehavior.ReadOnly && columnView.Columns.Count > 0)
          {
            IBusinessObject business = columnView.GetRow(columnView.FocusedRowHandle) as IBusinessObject;
            if (business != null && business.Owner == sourceList)
            {
              columnView.FocusedColumn = columnView.Columns[0];
              return item;
            }
          }
        }
      return null;
    }

    #region MultiSelected

    /// <summary>
    /// 设置多选值
    /// </summary>
    /// <param name="grids">GridControl队列</param>
    /// <param name="source">数据源</param>
    /// <param name="value">值</param>
    public static bool SetMultiSelect(IList<GridControl> grids, BindingSource source, bool value)
    {
      GridControl gridControl = FindGridControl(grids, source);
      if (gridControl == null)
        return false;
      ColumnView columnView = gridControl.FocusedView as ColumnView;
      if (columnView != null && columnView.OptionsSelection.MultiSelect != value)
      {
        columnView.OptionsSelection.MultiSelect = value;
        return true;
      }
      return false;
    }

    /// <summary>
    /// 检索被多选选择的对象
    /// </summary>
    /// <param name="grids">GridControl队列</param>
    /// <param name="source">数据源</param>
    public static IList<object> FindMultiSelected(IList<GridControl> grids, BindingSource source)
    {
      GridControl gridControl = FindGridControl(grids, source);
      if (gridControl == null)
        return null;
      ColumnView columnView = gridControl.FocusedView as ColumnView;
      if (columnView != null)
      {
        List<object> result = new List<object>();
        foreach (int i in columnView.GetSelectedRows())
          result.Add(columnView.GetRow(i));
        return result;
      }
      return null;
    }

    #endregion

    #region GridLevelNode

    private static GridLevelNode FindGridLevelNode(GridLevelNodeCollection nodes, ColumnView view)
    {
      if (nodes == null)
        return null;
      foreach (GridLevelNode node in nodes)
        if (node.LevelTemplate == view)
          return node;
        else
        {
          GridLevelNode result = FindGridLevelNode(node.Nodes, view);
          if (result != null)
            return result;
        }
      return null;
    }

    /// <summary>
    /// 检索匹配的GridLevelNode
    /// </summary>
    /// <param name="view">ColumnView</param>
    public static GridLevelNode FindGridLevelNode(ColumnView view)
    {
      if (view.GridControl.LevelTree != null)
        return FindGridLevelNode(view.GridControl.LevelTree.Nodes, view);
      return null;
    }

    /// <summary>
    /// 检索ColumnView关联的类
    /// </summary>
    /// <param name="view">GridView</param>
    public static Type FindDataSourceType(ColumnView view)
    {
      Type type = BindingSourceHelper.GetDataSourceType(view.GridControl.DataSource as BindingSource);
      if (type == null)
        return null;
      if (view == view.GridControl.MainView)
        return type;
      GridLevelNode node = FindGridLevelNode(view);
      if (node == null)
        return null;
      string propertyName = node.RelationName;
      int levelNumber = 0;
      while (!node.IsRootLevel)
      {
        levelNumber = levelNumber + 1;
        node = node.Parent;
      }
      return Utilities.FindDetailListType(type, propertyName, levelNumber);
    }

    #endregion

    #region Export

    /// <summary>
    /// 导出报表
    /// </summary>
    /// <param name="grid">GridControl</param>
    public static bool Export(GridControl grid)
    {
      return Export(grid, Phenix.Windows.Properties.Resources.DataExport, Phenix.Windows.Properties.Resources.GridExportFilter);
    }

    /// <summary>
    /// 导出报表
    /// </summary>
    /// <param name="grid">GridControl</param>
    /// <param name="title">Title</param>
    public static bool Export(GridControl grid, string title)
    {
      return Export(grid, title, Phenix.Windows.Properties.Resources.GridExportFilter);
    }

    /// <summary>
    /// 导出报表
    /// </summary>
    /// <param name="grid">GridControl</param>
    /// <param name="title">Title</param>
    /// <param name="filter">Filter</param>
    public static bool Export(GridControl grid, string title, string filter)
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
            using (new DevExpress.Utils.WaitDialogForm(String.Format(Phenix.Windows.Properties.Resources.DataExporting, saveFileDialog.FileName), Phenix.Core.Properties.Resources.PleaseWait))
            {
              switch (fileType)
              {
                case "xls":
                  grid.ExportToXls(saveFileDialog.FileName);
                  break;
                case "xlsx":
                  //如升级到Dev14.2版本需替换为以下注释掉的代码
                  //DevExpress.XtraPrinting.XlsxExportOptionsEx xlsxExportOptions = new DevExpress.XtraPrinting.XlsxExportOptionsEx();
                  //xlsxExportOptions.ExportType = DevExpress.Export.ExportType.WYSIWYG;
                  //grid.ExportToXlsx(saveFileDialog.FileName, xlsxExportOptions);
                  grid.ExportToXlsx(saveFileDialog.FileName);
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

    private static string LayoutPathName(Control container, GridControl grid)
    {
      string result = String.Empty;
      string location = container.GetType().Assembly.Location;
      if (File.Exists(location))
      {
        FileInfo fileInfo = new FileInfo(location);
        result = String.Format("{0}{1}{2}", result, fileInfo.Length, fileInfo.LastWriteTime);
      }
      result = String.Format("{0}.{1}.{2}.{3}", result,
        container.Name, grid.Name, (grid.MainView is ColumnView ? ((ColumnView)grid.MainView).Columns.Count : 0));
      return Path.Combine(LocalCacheDirectory, Path.GetFileNameWithoutExtension(location) + result.GetHashCode().ToString());
    }

    private static string OriginalLayoutPathName(Control container, GridControl grid)
    {
      return LayoutPathName(container, grid) + ".Origin";
    }

    /// <summary>
    /// 保存布局
    /// </summary>
    /// <param name="container">控件容器</param>
    public static void SaveLayout(Control container)
    {
      foreach (GridControl item in ControlHelper.FindControls<GridControl>(container))
        SaveLayout(container, item);
    }

    /// <summary>
    /// 保存布局
    /// </summary>
    /// <param name="container">控件容器</param>
    /// <param name="grid">GridControl</param>
    public static void SaveLayout(Control container, GridControl grid)
    {
      if (grid == null)
        throw new ArgumentNullException("grid");
      grid.MainView.SaveLayoutToXml(LayoutPathName(container, grid), OptionsLayoutBase.FullLayout);
      string pathName = OriginalLayoutPathName(container, grid);
      if (!File.Exists(pathName))
        grid.MainView.SaveLayoutToXml(pathName, OptionsLayoutBase.FullLayout);
    }

    /// <summary>
    /// 恢复布局
    /// </summary>
    /// <param name="container">控件容器</param>
    public static bool RestoreLayout(Control container)
    {
      bool result = true;
      foreach (GridControl item in ControlHelper.FindControls<GridControl>(container))
        if (!RestoreLayout(container, item))
          result = false;
      return result;
    }

    /// <summary>
    /// 恢复布局
    /// </summary>
    /// <param name="container">控件容器</param>
    /// <param name="grid">GridControl</param>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
    public static bool RestoreLayout(Control container, GridControl grid)
    {
      if (grid == null)
        throw new ArgumentNullException("grid");
      try
      {
        string pathName = LayoutPathName(container, grid);
        if (File.Exists(pathName))
        {
          grid.MainView.RestoreLayoutFromXml(pathName, OptionsLayoutBase.FullLayout);
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
    /// <param name="grid">GridControl</param>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
    public static bool ResetLayout(Control container, GridControl grid)
    {
      if (grid == null)
        throw new ArgumentNullException("grid");
      try
      {
        string pathName = OriginalLayoutPathName(container, grid);
        if (File.Exists(pathName))
        {
          grid.MainView.RestoreLayoutFromXml(pathName, OptionsLayoutBase.FullLayout);
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
    public static void PostEditor(IList<GridControl> grids)
    {
      foreach (GridControl item in grids)
      {
        BaseView view = item.FocusedView;
        if (view != null)
        {
          view.PostEditor();
          view.UpdateCurrentRow();
        }
      }
    }

    #endregion

    #endregion
  }
}