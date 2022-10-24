using System;
using System.Diagnostics;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Security.Permissions;

namespace Phenix.Core.Win32
{
  /// <summary>
  /// 非托管方法
  /// </summary>
  public static class NativeMethods
  {
    #region SetClock

    [DllImport("kernel32.dll")]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static extern bool SetLocalTime(ref SYSTEMTIME time);

    [StructLayout(LayoutKind.Sequential)]
    private struct SYSTEMTIME
    {
      public short Year;
      public short Month;
      public short DayOfWeek;
      public short Day;
      public short Hour;
      public short Minute;
      public short Second;
      public short Milliseconds;
    }

    /// <summary>
    /// 设置时钟
    /// </summary>
    public static void SetClock(DateTime dateTime)
    {
      SYSTEMTIME systemTime;

      systemTime.Year = (short)dateTime.Year;
      systemTime.Month = (short)dateTime.Month;
      systemTime.DayOfWeek = (short)dateTime.DayOfWeek;
      systemTime.Day = (short)dateTime.Day;
      systemTime.Hour = (short)dateTime.Hour;
      systemTime.Minute = (short)dateTime.Minute;
      systemTime.Second = (short)dateTime.Second;
      systemTime.Milliseconds = (short)dateTime.Millisecond;

      SetLocalTime(ref systemTime);
    }
    
    #endregion

    #region SettingChange

    [DllImport("user32.dll")]
    private static extern IntPtr SendMessageTimeout(IntPtr hWnd, uint Msg, UIntPtr wParam, IntPtr lParam, SendMessageTimeoutFlags fuFlags, uint uTimeout, out UIntPtr lpdwResult);

    [Flags]
    [Serializable]
    private enum SendMessageTimeoutFlags : uint
    {
      SMTO_NORMAL = 0x0000,
      SMTO_BLOCK = 0x0001,
      SMTO_ABORTIFHUNG = 0x0002,
      SMTO_NOTIMEOUTIFNOTHUNG = 0x0008
    }

    private static readonly IntPtr HWND_BROADCAST = new IntPtr(0xFFFF);
    private const uint WM_SETTINGCHANGE = 0x001A;

    private static void SettingChange()
    {
      UIntPtr result;
      SendMessageTimeout(HWND_BROADCAST, WM_SETTINGCHANGE, new UIntPtr(0), new IntPtr(0), SendMessageTimeoutFlags.SMTO_ABORTIFHUNG, 0, out result);
    }

    #endregion

    #region SetDateTimeFormat

    [DllImport("kernel32.dll", EntryPoint = "SetLocaleInfoA", CharSet = CharSet.Ansi, BestFitMapping = false, ThrowOnUnmappableChar = true)]
    private static extern bool SetLocaleInfo(uint Locale, uint LCType, string lpLCData);

    private const uint LOCALE_SSHORTDATE = 0x001F;
    private const uint LOCALE_STIMEFORMAT = 0x1003;

    /// <summary>
    /// 设置时间格式
    /// </summary>
    public static void SetDateTimeFormat()
    {
      DateTimeFormatInfo currentInfo = CultureInfo.InstalledUICulture.DateTimeFormat;
      if (String.CompareOrdinal(currentInfo.ShortDatePattern, AppConfig.ShortDatePattern) == 0 &&
        String.CompareOrdinal(currentInfo.LongTimePattern, AppConfig.LongTimePattern) == 0)
        return;

      uint locale = (uint)CultureInfo.InstalledUICulture.LCID;
      SetLocaleInfo(locale, LOCALE_SSHORTDATE, AppConfig.ShortDatePattern);
      SetLocaleInfo(locale, LOCALE_STIMEFORMAT, AppConfig.LongTimePattern);
      SettingChange();
    }

    #endregion

    #region Process

    [DllImport("kernel32.dll", SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static extern bool TerminateProcess(IntPtr hProcess, uint uExitCode);

    /// <summary>
    /// 中止进程
    /// </summary>
    [EnvironmentPermission(SecurityAction.LinkDemand)]
    public static bool TerminateProcess(Process process)
    {
      return TerminateProcess(process.Handle, 1);
    }

    #endregion
  }
}
