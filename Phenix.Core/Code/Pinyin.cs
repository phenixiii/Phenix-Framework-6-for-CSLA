using System;
using Microsoft.International.Converters.PinYinConverter;

namespace Phenix.Core.Code
{
  /// <summary>
  /// 拼音
  /// </summary>
  public static class Pinyin
  {
    #region 方法

    /// <summary>
    /// 文本缩写
    /// </summary>
    /// <param name="text">文本</param>
    public static string Abbreviation(string text)
    {
      if (String.IsNullOrEmpty(text))
        return String.Empty;
      string result = String.Empty;
      foreach (char c in text)
      {
        if ((int)c >= 32 && (int)c <= 126)
          result += c; //ASCII原样保留
        else
          result += GetPrefix(c); //累加拼音声母
      }
      return result;
    }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
    private static char GetPrefix(char c)
    {
      if (!ChineseChar.IsValidChar(c))
        return c;
      string pinyin = new ChineseChar(c).Pinyins[0];
      return pinyin[0];
    }

    #endregion
  }
}
