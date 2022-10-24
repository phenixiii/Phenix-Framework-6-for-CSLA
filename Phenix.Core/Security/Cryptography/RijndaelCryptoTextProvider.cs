using System;
using System.IO;
using System.Security.Cryptography;
using Phenix.Core.Code;

namespace Phenix.Core.Security.Cryptography
{
  /// <summary>
  /// Rijndael加解密字符串
  /// </summary>
  public static class RijndaelCryptoTextProvider
  {
    #region 方法

    /// <summary>
    /// 加密
    /// </summary>
    /// <param name="rgbKey">密钥</param>
    /// <param name="rgbIV">初始化向量</param>
    /// <param name="value">需加密的字符串</param>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2202:DoNotDisposeObjectsMultipleTimes")]
    public static byte[] Encrypt(byte[] rgbKey, byte[] rgbIV, string value)
    {
      if (value == null)
        return null;
      if (rgbKey == null || rgbKey.Length <= 0)
        throw new ArgumentNullException("rgbKey");
      if (rgbIV == null || rgbIV.Length <= 0)
        throw new ArgumentNullException("rgbIV");
      using (MemoryStream memoryStream = new MemoryStream())
      using (RijndaelManaged managed = new RijndaelManaged())
      {
        managed.Mode = CipherMode.CBC;
        //managed.Padding = PaddingMode.Zeros;
        managed.Key = rgbKey;
        managed.IV = rgbIV;
        using (ICryptoTransform encryptor = managed.CreateEncryptor(managed.Key, managed.IV))
        using (CryptoStream cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write))
        using (StreamWriter streamWriter = new StreamWriter(cryptoStream))
        {
          streamWriter.Write(value);
          streamWriter.Flush();
        }
        return memoryStream.ToArray();
      }
    }

    /// <summary>
    /// 解密
    /// </summary>
    /// <param name="rgbKey">密钥</param>
    /// <param name="rgbIV">初始化向量</param>
    /// <param name="value">需解密的字节数组</param>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2202:DoNotDisposeObjectsMultipleTimes")]
    public static string Decrypt(byte[] rgbKey, byte[] rgbIV, byte[] value)
    {
      if (value == null || value.Length <= 0)
        return null;
      if (rgbKey == null || rgbKey.Length <= 0)
        throw new ArgumentNullException("rgbKey");
      if (rgbIV == null || rgbIV.Length <= 0)
        throw new ArgumentNullException("rgbIV");
      using (MemoryStream memoryStream = new MemoryStream(value))
      using (RijndaelManaged managed = new RijndaelManaged())
      {
        managed.Mode = CipherMode.CBC;
        //managed.Padding = PaddingMode.Zeros;
        managed.Key = rgbKey;
        managed.IV = rgbIV;
        using (ICryptoTransform transform = managed.CreateDecryptor(managed.Key, managed.IV))
        using (CryptoStream cryptoStream = new CryptoStream(memoryStream, transform, CryptoStreamMode.Read))
        using (StreamReader streamReader = new StreamReader(cryptoStream))
        {
          return streamReader.ReadToEnd()/*.TrimEnd('\0')*/;
        }
      }
    }

    /// <summary>
    /// 加密
    /// </summary>
    /// <param name="key">密钥</param>
    /// <param name="IV">初始化向量</param>
    /// <param name="value">需加密的字符串</param>
    public static string Encrypt(string key, string IV, string value)
    {
      if (key == null || IV == null)
        return value;
      using (MD5 md5 = new MD5CryptoServiceProvider())
      {
        return Converter.BytesToHexString(Encrypt(md5.ComputeHash(System.Text.Encoding.UTF8.GetBytes(key)), md5.ComputeHash(System.Text.Encoding.UTF8.GetBytes(IV)), value));
      }
    }

    /// <summary>
    /// 加密
    /// IV等于Key
    /// </summary>
    /// <param name="key">密钥</param>
    /// <param name="value">需加密的字符串</param>
    public static string Encrypt(string key, string value)
    {
      return Encrypt(key, key, value);
    }

    /// <summary>
    /// 解密
    /// </summary>
    /// <param name="key">密钥</param>
    /// <param name="IV">初始化向量</param>
    /// <param name="value">需解密的字节串</param>
    public static string Decrypt(string key, string IV, string value)
    {
      if (key == null || IV == null)
        return value;
      using (MD5 md5 = new MD5CryptoServiceProvider())
      {
        return Decrypt(md5.ComputeHash(System.Text.Encoding.UTF8.GetBytes(key)), md5.ComputeHash(System.Text.Encoding.UTF8.GetBytes(IV)), Converter.HexStringToBytes(value));
      }
    }

    /// <summary>
    /// 解密
    /// IV等于Key
    /// </summary>
    /// <param name="key">密钥</param>
    /// <param name="value">需解密的字节串</param>
    public static string Decrypt(string key, string value)
    {
      return Decrypt(key, key, value);
    }

    #endregion
  }
}
