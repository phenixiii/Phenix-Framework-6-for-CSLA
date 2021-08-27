using System;
using System.Data;
using System.Windows.Forms;
using Phenix.Core;

namespace Phenix.Windows.Helper
{
  /// <summary>
  /// Excel助手
  /// </summary>
  public static class ExcelHelper
  {
    /// <summary>
    /// 读取Excel文件
    /// 取第一个表单数据
    /// </summary>
    public static DataTable Import()
    {
      return Import(null);
    }

    /// <summary>
    /// 读取Excel文件
    /// 取指定表单数据
    /// </summary>
    /// <param name="sheetName">表单名</param>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
    public static DataTable Import(string sheetName)
    {
      using (OpenFileDialog openFileDialog = new OpenFileDialog())
      {
        openFileDialog.Title = Phenix.Windows.Properties.Resources.DataImport;
        openFileDialog.Filter = Phenix.Windows.Properties.Resources.ExcelImportFilter;
        openFileDialog.RestoreDirectory = true;
        do
        {
          try
          {
            if (openFileDialog.ShowDialog() != DialogResult.OK)
              return null;
            using (new DevExpress.Utils.WaitDialogForm(String.Format(Phenix.Windows.Properties.Resources.DataImporting, openFileDialog.FileName), Phenix.Core.Properties.Resources.PleaseWait))
            {
              return Phenix.Core.Data.ExcelHelper.Read(openFileDialog.FileName, sheetName);
            }
          }
          catch (Exception ex)
          {
            if (MessageBox.Show(String.Format(Properties.Resources.DataImportAborted, openFileDialog.FileName, AppUtilities.GetErrorHint(ex)),
              openFileDialog.Title, MessageBoxButtons.YesNo, MessageBoxIcon.Error) != DialogResult.Yes)
              return null;
          }
        } while (true);
      }
    }
  } 
}