using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using DevExpress.XtraReports.Parameters;
using DevExpress.XtraReports.UI;
using Phenix.Core;

namespace Phenix.Windows.Helper
{
  /// <summary>
  /// 模板文件
  /// </summary>
  public sealed class TemplateFile
  {
    internal TemplateFile(XtraReport originalReport)
      : this(originalReport, null) { }

    internal TemplateFile(XtraReport originalReport, string templateName)
    {
      _originalReport = originalReport;
      _templateName = TidyTemplateName(templateName);
    }

    #region 属性

    private static readonly string DEF_TEMPLATE_NAME = Phenix.Windows.Properties.Resources.DefTemplateName;
    private const string DEF_EXTENSION = "repx";

    private readonly XtraReport _originalReport;
    /// <summary>
    /// 原始报表
    /// </summary>
    public XtraReport OriginalReport
    {
      get { return _originalReport; }
    }

    private XtraReport _report;
    /// <summary>
    /// 报表
    /// </summary>
    public XtraReport Report
    {
      get
      {
        if (_report == null)
        {
          if (File.Exists(Path))
          {
            _report = XtraReport.FromFile(Path, true);
            _report.LoadLayout(Path);
          }
          else
          {
            string defaultTemplatePath = System.IO.Path.Combine(GetDirectory(OriginalReport), DEF_TEMPLATE_NAME);
            if (!File.Exists(defaultTemplatePath))
              OriginalReport.SaveLayout(defaultTemplatePath);
            _report = XtraReport.FromFile(defaultTemplatePath, true);
            _report.LoadLayout(defaultTemplatePath);
          }
          foreach (Parameter item in OriginalReport.Parameters)
            if (_report.Parameters.Cast<Parameter>().All(p => p.Name != item.Name))
              _report.Parameters.Add(item);
          _report.DataSource = _report.DataSource;
        } 
        return _report;
      }
    }

    private readonly string _templateName;
    /// <summary>
    /// 模板名
    /// </summary>
    public string TemplateName
    {
      get { return _templateName; }
    }

    private string _path;
    /// <summary>
    /// 路径
    /// </summary>
    public string Path
    {
      get
      {
        if (_path == null)
          _path = System.IO.Path.Combine(GetDirectory(OriginalReport), TemplateName);
        return _path;
      }
    }

    /// <summary>
    /// 是否为默认模板
    /// </summary>
    public bool IsDefaultTemplate
    {
      get { return String.Compare(TemplateName, DEF_TEMPLATE_NAME, StringComparison.Ordinal) == 0; }
    }

    #endregion

    #region 方法

    /// <summary>
    /// 取哈希值
    /// </summary>
    public override int GetHashCode()
    {
      return Path.GetHashCode();
    }

    /// <summary>
    /// 比较对象
    /// </summary>
    /// <param name="obj">对象</param>
    public override bool Equals(object obj)
    {
      if (object.ReferenceEquals(obj, this))
        return true;
      TemplateFile other = obj as TemplateFile;
      if (object.ReferenceEquals(other, null))
        return false;
      return String.CompareOrdinal(Path, other.Path) == 0;
    }
    
    /// <summary>
    /// 保存模板
    /// </summary>
    public void Save()
    {
      Report.SaveLayout(Path);
    }

    /// <summary>
    /// 整理模板名
    /// </summary>
    /// <param name="templateName">模板名</param>
    /// <returns>模板名</returns>
    public static string TidyTemplateName(string templateName)
    {
      string result = System.IO.Path.GetFileName(templateName);
      if (String.IsNullOrEmpty(result) || String.CompareOrdinal(result, DEF_TEMPLATE_NAME) == 0)
        return DEF_TEMPLATE_NAME;
      return System.IO.Path.ChangeExtension(result, DEF_EXTENSION);
    }

    /// <summary>
    /// 获取模板目录
    /// </summary>
    /// <param name="report">报表</param>
    /// <returns>模板目录</returns>
    public static string GetDirectory(XtraReport report)
    {
      string result = System.IO.Path.Combine(AppConfig.BaseDirectory, "ReportTemplates", report.GetType().FullName);
      if (!Directory.Exists(result))
        Directory.CreateDirectory(result);
      return result;
    }
    
    /// <summary>
    /// 获取模板文件
    /// </summary>
    /// <param name="report">报表</param>
    /// <returns>模板文件队列</returns>
    public static IList<TemplateFile> GetTemplates(XtraReport report)
    {
      List<TemplateFile> result = new List<TemplateFile>();
      result.Add(new TemplateFile(report));
      foreach (string s in Directory.GetFiles(GetDirectory(report), String.Format("*.{0}", DEF_EXTENSION), SearchOption.AllDirectories))
        result.Add(new TemplateFile(report, s));
      return result;
    }

    #endregion
  }
}
