using System;
using System.Drawing;
using DevExpress.XtraTreeList;

namespace Phenix.Windows.Helper
{
  /// <summary>
  /// TreeList扩展
  /// </summary>
  public static class TreeListExtentions
  {
    #region BackColor

    /// <summary>
    /// 设置背景色
    /// </summary>
    /// <param name="treeList">TreeList</param>
    /// <param name="oddRowColor">OddRow背景色</param>
    /// <param name="evenRowColor">EvenRow背景色</param>
    public static void SetBackColor(this TreeList treeList, Color oddRowColor, Color evenRowColor)
    {
      if (treeList == null)
        throw new ArgumentNullException("treeList");
      if (oddRowColor.ToArgb() != Color.Empty.ToArgb())
      {
        treeList.OptionsView.EnableAppearanceOddRow = true;
        treeList.Appearance.OddRow.Options.UseBackColor = true;
        treeList.Appearance.OddRow.BackColor = oddRowColor;
      }
      if (evenRowColor.ToArgb() != Color.Empty.ToArgb())
      {
        treeList.OptionsView.EnableAppearanceEvenRow = true;
        treeList.Appearance.EvenRow.Options.UseBackColor = true;
        treeList.Appearance.EvenRow.BackColor = evenRowColor;
      }
    }

    #endregion
  }
}
