using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Phenix.Core.Code
{
  /// <summary>
  /// ת����
  /// </summary>
  public static class Converter
  {
    #region ����

    private static readonly char[] _hexDigits = { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'A', 'B', 'C', 'D', 'E', 'F' };

    /// <summary>
    /// Null����
    /// </summary>
    public static string NullSymbolic
    {
      get { return "null"; }
    }

    #endregion

    #region ����

    #region Bytes

    /// <summary>
    /// �ֽ�����ת�����ַ���
    /// </summary>
    /// <param name="sourceBuffer">�ֽ�����</param>
    public static string BytesToString(byte[] sourceBuffer)
    {
      if (sourceBuffer == null)
        return null;
      char[] result = new char[sourceBuffer.Length];
      for (int i = 0; i < sourceBuffer.Length; i++)
        result[i] = (char)sourceBuffer[i];
      return new String(result);
    }

    /// <summary>
    /// �ַ���ת�����ֽ�����
    /// </summary>
    /// <param name="sourceBuffer">�ַ���</param>
    public static byte[] StringToBytes(string sourceBuffer)
    {
      if (sourceBuffer == null)
        return null;
      byte[] result = new byte[sourceBuffer.Length];
      for (int i = 0; i < sourceBuffer.Length; i++)
        result[i] = (byte)sourceBuffer[i];
      return result;
    }

    /// <summary>
    /// �ֽ�����ת����Hex�ַ���
    /// </summary>
    /// <param name="sourceBuffer">�ֽ�����</param>
    public static string BytesToHexString(byte[] sourceBuffer)
    {
      if (sourceBuffer == null)
        return null;
      char[] result = new char[sourceBuffer.Length * 2];
      for (int i = 0; i < sourceBuffer.Length; i++)
      {
        byte b = sourceBuffer[i];
        result[i * 2] = _hexDigits[b >> 4];
        result[i * 2 + 1] = _hexDigits[b & 0xF];
      }
      return new String(result);
    }

    /// <summary>
    /// Hex�ַ���ת�����ֽ�����
    /// </summary>
    /// <param name="sourceBuffer">Hex�ַ���</param>
    public static byte[] HexStringToBytes(string sourceBuffer)
    {
      if (String.IsNullOrEmpty(sourceBuffer))
        return null;
      byte[] result = new byte[sourceBuffer.Length / 2];
      for (int i = 0; i < result.Length; i++)
        result[i] = byte.Parse(sourceBuffer.Substring(i * 2, 2), NumberStyles.AllowHexSpecifier);
      return result;
    }
    
    #endregion

    #region String

    /// <summary>
    /// �滻�ַ���
    /// </summary>
    /// <param name="source">Դ�ַ���</param>
    /// <param name="oldPattern">�ɶ���</param>
    /// <param name="newPattern">�¶���</param>
    /// <param name="comparisonType">��������</param>
    public static string ReplaceString(string source, string oldPattern, string newPattern, StringComparison comparisonType)
    {
      if (String.IsNullOrEmpty(source))
        return source;
      if (String.IsNullOrEmpty(oldPattern))
        return source;
      int i = 0;
      do
      {
        i = source.IndexOf(oldPattern, i, comparisonType);
        if (i == -1)
          break;
        source = source.Remove(i, oldPattern.Length);
        source = source.Insert(i, newPattern);
      } while (true);
      return source;
    }
    
    #endregion

    #region Enum

    /// <summary>
    /// ����ת���ɷ���
    /// </summary>
    /// <param name="value">����ֵ</param>
    public static string BooleanToSymbolic(bool value)
    {
      return value ? "��" : "-";
    }

    #region Values��ö�����黥ת

    /// <summary>
    /// ��ö�������滻��ֵ���
    /// separator = AppConfig.VALUE_SEPARATOR
    /// </summary>
    /// <param name="enums">ö������</param>
    public static string EnumArrayToValues(params Enum[] enums)
    {
      return EnumArrayToValues(AppConfig.VALUE_SEPARATOR, enums);
    }

    /// <summary>
    /// ��ö�������滻��ֵ���
    /// </summary>
    /// <param name="separator">��ǩ�ָ���</param>
    /// <param name="enums">ö������</param>
    public static string EnumArrayToValues(char separator, params Enum[] enums)
    {
      if (enums == null)
        return null;
      StringBuilder result = new StringBuilder();
      foreach (Enum item in enums)
      {
        result.Append(item.ToString());
        result.Append(separator);
      }
      if (result.Length > 0)
        result.Remove(result.Length - 1, 1);
      return result.ToString();
    }

    /// <summary>
    /// ��ö�������滻��ֵ���
    /// separator = AppConfig.VALUE_SEPARATOR
    /// </summary>
    /// <param name="enums">ö������</param>
    public static string EnumArrayToValues<TEnum>(params TEnum[] enums)
    {
      if (enums == null)
        return null;
      return EnumArrayToValues(enums.Cast<Enum>().ToArray());
    }

    /// <summary>
    /// ��ö�������滻��ֵ���
    /// </summary>
    /// <param name="separator">��ǩ�ָ���</param>
    /// <param name="enums">ö������</param>
    public static string EnumArrayToValues<TEnum>(char separator, params TEnum[] enums)
    {
      if (enums == null)
        return null;
      return EnumArrayToValues(separator, enums.Cast<Enum>().ToArray());
    }

    /// <summary>
    /// ��ֵ����滻��ö������
    /// separator = AppConfig.VALUE_SEPARATOR
    /// </summary>
    /// <param name="values">ֵ���</param>
    public static TEnum[] ValuesToEnumArray<TEnum>(string values)
    {
      return ValuesToEnumArray<TEnum>(values, AppConfig.VALUE_SEPARATOR);
    }

    /// <summary>
    /// ��ֵ����滻��ö������
    /// </summary>
    /// <param name="values">ֵ���</param>
    /// <param name="separator">��ǩ�ָ���</param>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
    public static TEnum[] ValuesToEnumArray<TEnum>(string values, char separator)
    {
      if (String.IsNullOrEmpty(values))
        return null;
      try
      {
        string[] strings = values.Split(separator);
        List<TEnum> result = new List<TEnum>(strings.Length);
        foreach (string s in strings)
          result.Add((TEnum)Enum.Parse(typeof(TEnum), s.Trim()));
        return result.ToArray();
      }
      catch (Exception ex)
      {
        Phenix.Core.Log.EventLog.SaveLocal(MethodBase.GetCurrentMethod(), values, ex);
        return null;
      }
    }

    #endregion

    #region Flags��ö�����黥ת

    /// <summary>
    /// ��ö�������滻�ɱ�����
    /// separator = AppConfig.VALUE_SEPARATOR
    /// </summary>
    /// <param name="enums">ö������</param>
    public static string EnumArrayToFlags(params Enum[] enums)
    {
      return EnumArrayToFlags(AppConfig.VALUE_SEPARATOR, enums);
    }

    /// <summary>
    /// ��ö�������滻�ɱ�����
    /// </summary>
    /// <param name="separator">��ǩ�ָ���</param>
    /// <param name="enums">ö������</param>
    public static string EnumArrayToFlags(char separator, params Enum[] enums)
    {
      if (enums == null)
        return null;
      StringBuilder result = new StringBuilder();
      foreach (Enum item in enums)
      {
        result.Append(item.ToString("d"));
        result.Append(separator);
      }
      if (result.Length > 0)
        result.Remove(result.Length - 1, 1);
      return result.ToString();
    }

    /// <summary>
    /// ��ö�������滻�ɱ�����
    /// separator = AppConfig.VALUE_SEPARATOR
    /// </summary>
    /// <param name="enums">ö������</param>
    public static string EnumArrayToFlags<TEnum>(params TEnum[] enums)
    {
      if (enums == null)
        return null;
      return EnumArrayToFlags(enums.Cast<Enum>().ToArray());
    }

    /// <summary>
    /// ��ö�������滻�ɱ�����
    /// </summary>
    /// <param name="separator">��ǩ�ָ���</param>
    /// <param name="enums">ö������</param>
    public static string EnumArrayToFlags<TEnum>(char separator, params TEnum[] enums)
    {
      if (enums == null)
        return null;
      return EnumArrayToFlags(separator, enums.Cast<Enum>().ToArray());
    }

    /// <summary>
    /// ���������滻��ö������
    /// separator = AppConfig.VALUE_SEPARATOR
    /// </summary>
    /// <param name="flags">������</param>
    public static TEnum[] FlagsToEnumArray<TEnum>(string flags)
    {
      return FlagsToEnumArray<TEnum>(flags, AppConfig.VALUE_SEPARATOR);
    }

    /// <summary>
    /// ���������滻��ö������
    /// </summary>
    /// <param name="flags">������</param>
    /// <param name="separator">��ǩ�ָ���</param>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
    public static TEnum[] FlagsToEnumArray<TEnum>(string flags, char separator)
    {
      if (String.IsNullOrEmpty(flags))
        return null;
      try
      {
        string[] strings = flags.Split(separator);
        List<TEnum> result = new List<TEnum>(strings.Length);
        foreach (string s in strings)
          result.Add((TEnum)Enum.Parse(typeof(TEnum), s.Trim()));
        return result.ToArray();
      }
      catch (Exception ex)
      {
        Phenix.Core.Log.EventLog.SaveLocal(MethodBase.GetCurrentMethod(), flags, ex);
        return null;
      }
    }

    #endregion

    #endregion

    #endregion
  }
}