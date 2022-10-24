using System.Security.Cryptography;
using System.Text;

namespace Phenix.Core.Security.Cryptography
{
  /// <summary>
  /// MD5加密字符串
  /// </summary>
  public static class MD5CryptoTextProvider
  {
    /// <summary>
    /// 取Hash字符串
    /// toUpper = true
    /// </summary>
    /// <param name="value">需加密的字符串</param>
    /// <returns>Hash字符串</returns>
    public static string ComputeHash(string value)
    {
      return ComputeHash(value, true);
    }

    /// <summary>
    /// 取Hash字符串
    /// </summary>
    /// <param name="value">需加密的字符串</param>
    /// <param name="toUpper">返回大写字符串</param>
    /// <returns>Hash字符串</returns>
    public static string ComputeHash(string value, bool toUpper)
    {
      if (value == null)
        return null;
      StringBuilder result = new StringBuilder();
      using (MD5 md5 = new MD5CryptoServiceProvider())
      {
        byte[] data = md5.ComputeHash(Encoding.UTF8.GetBytes(value));
        if (toUpper)
          for (int i = 0; i < data.Length; i++)
            result.Append(data[i].ToString("X2"));
        else
          for (int i = 0; i < data.Length; i++)
            result.Append(data[i].ToString("x2"));
      }
      return result.ToString();
    }
  }
}
