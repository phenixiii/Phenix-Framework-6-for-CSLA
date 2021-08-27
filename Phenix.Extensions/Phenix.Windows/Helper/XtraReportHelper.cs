using System;
using DevExpress.XtraReports.UI;

namespace Phenix.Windows.Helper
{
  /// <summary>
  /// XtraReport助手
  /// </summary>
  public static class XtraReportHelper
  {
    /// <summary>
    /// 显示报表预览
    /// canDesign = true
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
    public static void ShowPreviewDialog<T>()
      where T : XtraReport
    {
      ShowPreviewDialog(typeof(T), null, true);
    }

    /// <summary>
    /// 显示报表预览
    /// </summary>
    /// <param name="canDesign">是否可以编辑报表模板</param>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
    public static void ShowPreviewDialog<T>(bool canDesign)
      where T : XtraReport
    {
      ShowPreviewDialog(typeof(T), null, canDesign);
    }

    /// <summary>
    /// 显示报表预览
    /// canDesign = true
    /// </summary>
    /// <param name="dataSource">数据源</param>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
    public static void ShowPreviewDialog<T>(object dataSource)
      where T : XtraReport
    {
      ShowPreviewDialog(typeof(T), dataSource, true);
    }

    /// <summary>
    /// 显示报表预览
    /// </summary>
    /// <param name="dataSource">数据源</param>
    /// <param name="canDesign">是否可以编辑报表模板</param>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
    public static void ShowPreviewDialog<T>(object dataSource, bool canDesign)
      where T : XtraReport
    {
      ShowPreviewDialog(typeof(T), dataSource, canDesign);
    }

    /// <summary>
    /// 显示报表预览
    /// canDesign = true
    /// </summary>
    /// <param name="reportType">报表类型</param>
    public static void ShowPreviewDialog(Type reportType)
    {
      ShowPreviewDialog(reportType, null, true);
    }

    /// <summary>
    /// 显示报表预览
    /// </summary>
    /// <param name="reportType">报表类型</param>
    /// <param name="canDesign">是否可以编辑报表模板</param>
    public static void ShowPreviewDialog(Type reportType, bool canDesign)
    {
      ShowPreviewDialog(reportType, null, canDesign);
    }

    /// <summary>
    /// 显示报表预览
    /// canDesign = true
    /// </summary>
    /// <param name="reportType">报表类型</param>
    /// <param name="dataSource">数据源</param>
    public static void ShowPreviewDialog(Type reportType, object dataSource)
    {
      ShowPreviewDialog(reportType, dataSource, true);
    }

    /// <summary>
    /// 显示报表预览
    /// </summary>
    /// <param name="reportType">报表类型</param>
    /// <param name="dataSource">数据源</param>
    /// <param name="canDesign">是否可以编辑报表模板</param>
    public static void ShowPreviewDialog(Type reportType, object dataSource, bool canDesign)
    {
      if (reportType == null)
        throw new ArgumentNullException("reportType");
      if (!reportType.IsSubclassOf(typeof(XtraReport)))
        throw new InvalidOperationException(String.Format("类{0}需继承自{1}", reportType.FullName, typeof(XtraReport).FullName));
      XtraReport report = (XtraReport)Activator.CreateInstance(reportType, true);
      report.ShowPreviewDialog(dataSource, canDesign);
    }
  }
}