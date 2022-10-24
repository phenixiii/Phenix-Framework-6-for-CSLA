using System;
using System.IO;
using System.Security.Cryptography;

namespace Phenix.Core.Security.Cryptography
{
  /// <summary>
  /// DES�ӽ����ַ���
  /// </summary>
  public static class DesCryptoTextProvider
  {
    #region ����

    /// <summary>
    /// ����
    /// </summary>
    /// <param name="rgbKey">��Կ</param>
    /// <param name="rgbIV">��ʼ������</param>
    /// <param name="value">����ܵ��ַ���</param>
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
      {
        using (DESCryptoServiceProvider cryptoServiceProvider = new DESCryptoServiceProvider())
        using (ICryptoTransform transform = cryptoServiceProvider.CreateEncryptor(rgbKey, rgbIV))
        using (CryptoStream cryptoStream = new CryptoStream(memoryStream, transform, CryptoStreamMode.Write))
        using (StreamWriter streamWriter = new StreamWriter(cryptoStream))
        {
          streamWriter.Write(value);
          streamWriter.Flush();
        }
        return memoryStream.ToArray();
      }
    }

    /// <summary>
    /// ����
    /// </summary>
    /// <param name="rgbKey">��Կ</param>
    /// <param name="rgbIV">��ʼ������</param>
    /// <param name="value">����ܵ��ֽ�����</param>
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
      using (DESCryptoServiceProvider cryptoServiceProvider = new DESCryptoServiceProvider())
      using (ICryptoTransform transform = cryptoServiceProvider.CreateDecryptor(rgbKey, rgbIV))
      using (CryptoStream cryptoStream = new CryptoStream(memoryStream, transform, CryptoStreamMode.Read))
      using (StreamReader streamReader = new StreamReader(cryptoStream))
      {
        return streamReader.ReadToEnd();
      }
    }

    #endregion
  }
}