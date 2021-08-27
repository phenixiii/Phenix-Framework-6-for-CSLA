using DevExpress.XtraReports.UI;

namespace Phenix.Windows.Helper
{
  /// <summary>
  /// XtraReport扩展
  /// </summary>
  public static class XtraReportExtentions
  {
    /// <summary>
    /// 显示报表预览
    /// canDesign = true
    /// </summary>
    /// <param name="report">报表</param>
    public static void ShowPreviewDialog(this XtraReport report)
    {
      ShowPreviewDialog(report, null, true);
    }

    /// <summary>
    /// 显示报表预览
    /// </summary>
    /// <param name="report">报表</param>
    /// <param name="canDesign">是否可以编辑报表模板</param>
    public static void ShowPreviewDialog(this XtraReport report, bool canDesign)
    {
      ShowPreviewDialog(report, null, canDesign);
    }

    /// <summary>
    /// 显示报表预览
    /// canDesign = true
    /// </summary>
    /// <param name="report">报表</param>
    /// <param name="dataSource">数据源</param>
    public static void ShowPreviewDialog(this XtraReport report, object dataSource)
    {
      ShowPreviewDialog(report, dataSource, true);
    }

    /// <summary>
    /// 显示报表预览
    /// </summary>
    /// <param name="report">报表</param>
    /// <param name="dataSource">数据源</param>
    /// <param name="canDesign">是否可以编辑报表模板</param>
    public static void ShowPreviewDialog(this XtraReport report, object dataSource, bool canDesign)
    {
      if (dataSource != null)
        report.DataSource = dataSource;

      if (canDesign)
      {
        using (PreviewForm previewForm = new PreviewForm(report))
        {
          previewForm.ShowDialog();
        }
      }
      else
        ((DevExpress.XtraReports.IReport)report).ShowPreviewDialog();
    }
   
    /// <summary>
    /// 显示报表设计窗口
    /// </summary>
    /// <param name="report"></param>
    public static void ShowDesignerDialog(this XtraReport report)
    {
      using (DesignForm designForm = new DesignForm(report))
      {
        designForm.ShowDialog();
      }
    }
  }
}