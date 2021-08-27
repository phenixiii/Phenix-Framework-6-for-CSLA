using System;
using System.Drawing;
using DevExpress.XtraGrid.Views.Grid;

namespace Phenix.Windows.Helper
{
  /// <summary>
  /// GridView扩展
  /// </summary>
  public static class GridViewExtentions
  {
    #region BackColor

    /// <summary>
    /// 设置背景色
    /// </summary>
    /// <param name="view">GridView</param>
    /// <param name="oddRowColor">view.Appearance.OddRow.BackColor</param>
    /// <param name="evenRowColor">view.Appearance.EvenRow.BackColor</param>
    /// <param name="focusedRowColor">view.Appearance.FocusedRow.BackColor</param>
    public static void SetBackColor(this GridView view, Color oddRowColor, Color evenRowColor, Color focusedRowColor)
    {
      if (view == null)
        throw new ArgumentNullException("view");
      if (oddRowColor.ToArgb() != Color.Empty.ToArgb())
      {
        view.OptionsView.EnableAppearanceOddRow = true;
        view.Appearance.OddRow.Options.UseBackColor = true;
        view.Appearance.OddRow.BackColor = oddRowColor;
      }
      if (evenRowColor.ToArgb() != Color.Empty.ToArgb())
      {
        view.OptionsView.EnableAppearanceEvenRow = true;
        view.Appearance.EvenRow.Options.UseBackColor = true;
        view.Appearance.EvenRow.BackColor = evenRowColor;
      }
      if (focusedRowColor.ToArgb() != Color.Empty.ToArgb())
      {
        view.OptionsView.EnableAppearanceEvenRow = true;
        view.Appearance.FocusedRow.Options.UseBackColor = true;
        view.Appearance.FocusedRow.BackColor = focusedRowColor;
      }
    }

    #endregion
  }
}
