using System;
using System.Security.Cryptography;
using System.Text;

namespace Phenix.Core.Security.Cryptography
{
  /// <summary>
  /// RSA加解密字符串
  /// </summary>
  public static class RSACryptoTextProvider
  {
    #region 方法

    /// <summary>
    /// 生成公钥密钥对
    /// </summary>
    public static KeyPair CreateKeyPair()
    {
      using (RSACryptoServiceProvider cryptoServiceProvider = new RSACryptoServiceProvider())
      {
        return CreateKeyPair(cryptoServiceProvider);
      }
    }

    /// <summary>
    /// 生成公钥密钥对
    /// </summary>
    /// <param name="dwKeySize">密钥尺寸</param>
    public static KeyPair CreateKeyPair(int dwKeySize)
    {
      using (RSACryptoServiceProvider cryptoServiceProvider = new RSACryptoServiceProvider(dwKeySize))
      {
        return CreateKeyPair(cryptoServiceProvider);
      }
    }

    /// <summary>
    /// 生成公钥密钥对
    /// </summary>
    /// <param name="parameters">服务参数</param>
    public static KeyPair CreateKeyPair(CspParameters parameters)
    {
      using (RSACryptoServiceProvider cryptoServiceProvider = new RSACryptoServiceProvider(parameters))
      {
        return CreateKeyPair(cryptoServiceProvider);
      }
    }

    /// <summary>
    /// 生成公钥密钥对
    /// </summary>
    /// <param name="dwKeySize">密钥尺寸</param>
    /// <param name="parameters">服务参数</param>
    public static KeyPair CreateKeyPair(int dwKeySize, CspParameters parameters)
    {
      using (RSACryptoServiceProvider cryptoServiceProvider = new RSACryptoServiceProvider(dwKeySize, parameters))
      {
        return CreateKeyPair(cryptoServiceProvider);
      }
    }

    /// <summary>
    /// 生成公钥密钥对
    /// </summary>
    /// <param name="cryptoServiceProvider">加密服务</param>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters")]
    public static KeyPair CreateKeyPair(RSACryptoServiceProvider cryptoServiceProvider)
    {
      return new KeyPair(cryptoServiceProvider.ToXmlString(false), cryptoServiceProvider.ToXmlString(true));
    }

    /// <summary>
    /// 加密
    /// </summary>
    /// <param name="publicKey">公钥</param>
    /// <param name="value">需加密的字符串</param>
    /// <param name="fOAEP">如果为 true，则使用OAEP填充（仅在运行Microsoft Windows XP或更高版本的计算机上可用）执行直接的RSA加密；否则，如果为false，则使用PKCS#1 1.5版填充</param>
    public static string Encrypt(string publicKey, string value, bool fOAEP)
    {
      using (RSACryptoServiceProvider cryptoServiceProvider = new RSACryptoServiceProvider())
      {
        return Encrypt(cryptoServiceProvider, publicKey, value, fOAEP);
      }
    }

    /// <summary>
    /// 加密
    /// </summary>
    /// <param name="parameters">服务参数</param>
    /// <param name="value">需加密的字符串</param>
    /// <param name="fOAEP">如果为 true，则使用OAEP填充（仅在运行Microsoft Windows XP或更高版本的计算机上可用）执行直接的RSA加密；否则，如果为false，则使用PKCS#1 1.5版填充</param>
    public static string Encrypt(CspParameters parameters, string value, bool fOAEP)
    {
      using (RSACryptoServiceProvider cryptoServiceProvider = new RSACryptoServiceProvider(parameters))
      {
        return Encrypt(cryptoServiceProvider, null, value, fOAEP);
      }
    }

    /// <summary>
    /// 加密
    /// </summary>
    /// <param name="cryptoServiceProvider">加密服务</param>
    /// <param name="publicKey">公钥</param>
    /// <param name="value">需加密的字符串</param>
    /// <param name="fOAEP">如果为 true，则使用OAEP填充（仅在运行Microsoft Windows XP或更高版本的计算机上可用）执行直接的RSA加密；否则，如果为false，则使用PKCS#1 1.5版填充</param>
    public static string Encrypt(RSACryptoServiceProvider cryptoServiceProvider, string publicKey, string value, bool fOAEP)
    {
      if (!String.IsNullOrEmpty(publicKey))
        cryptoServiceProvider.FromXmlString(publicKey);
      byte[] result = cryptoServiceProvider.Encrypt(Encoding.UTF8.GetBytes(value), fOAEP);
      return Encoding.UTF8.GetString(result);
    }

    /// <summary>
    /// 解密
    /// </summary>
    /// <param name="privateKey">密钥</param>
    /// <param name="value">需加密的字符串</param>
    /// <param name="fOAEP">如果为 true，则使用OAEP填充（仅在运行Microsoft Windows XP或更高版本的计算机上可用）执行直接的RSA加密；否则，如果为false，则使用PKCS#1 1.5版填充</param>
    public static string Decrypt(string privateKey, string value, bool fOAEP)
    {
      using (RSACryptoServiceProvider cryptoServiceProvider = new RSACryptoServiceProvider())
      {
        return Decrypt(cryptoServiceProvider, privateKey, value, fOAEP);
      }
    }

    /// <summary>
    /// 解密
    /// </summary>
    /// <param name="parameters">服务参数</param>
    /// <param name="value">需加密的字符串</param>
    /// <param name="fOAEP">如果为 true，则使用OAEP填充（仅在运行Microsoft Windows XP或更高版本的计算机上可用）执行直接的RSA加密；否则，如果为false，则使用PKCS#1 1.5版填充</param>
    public static string Decrypt(CspParameters parameters, string value, bool fOAEP)
    {
      using (RSACryptoServiceProvider cryptoServiceProvider = new RSACryptoServiceProvider(parameters))
      {
        return Decrypt(cryptoServiceProvider, null, value, fOAEP);
      }
    }

    /// <summary>
    /// 解密
    /// </summary>
    /// <param name="cryptoServiceProvider">加密服务</param>
    /// <param name="privateKey">密钥</param>
    /// <param name="value">需解密的字符串</param>
    /// <param name="fOAEP">如果为 true，则使用OAEP填充（仅在运行Microsoft Windows XP或更高版本的计算机上可用）执行直接的RSA加密；否则，如果为false，则使用PKCS#1 1.5版填充</param>
    public static string Decrypt(RSACryptoServiceProvider cryptoServiceProvider, string privateKey, string value, bool fOAEP)
    {
      if (!String.IsNullOrEmpty(privateKey))
        cryptoServiceProvider.FromXmlString(privateKey);
      byte[] result = cryptoServiceProvider.Decrypt(Encoding.UTF8.GetBytes(value), fOAEP);
      return Encoding.UTF8.GetString(result);
    }

    #endregion
  }
}
