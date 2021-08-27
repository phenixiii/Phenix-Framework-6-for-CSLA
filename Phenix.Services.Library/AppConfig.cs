using System;
using Phenix.Core;

namespace Phenix.Services.Library
{
  /// <summary>
  /// 应用系统配置信息
  /// </summary>
  public static class AppConfig
  {
    private static int? _ipSegmentCount;
    /// <summary>
    /// 反馈给客户端可登录服务IP的筛选范围为登录服务IP前几段保持一致
    /// 小于等于0为全部
    /// 大于等于4为唯一指定
    /// 缺省为 2
    /// </summary>
    public static int IpSegmentCount
    {
      get { return AppSettings.GetProperty(ref _ipSegmentCount, 2); }
      set { AppSettings.SetProperty(ref _ipSegmentCount, value); }
    }

    private static bool? _allowUserMultipleAddressLogin;
    /// <summary>
    /// 允许用户多IP登录
    /// 不控制Guest用户
    /// 缺省为 true
    /// </summary>
    public static bool AllowUserMultipleAddressLogin
    {
      get { return AppSettings.GetProperty(ref _allowUserMultipleAddressLogin, true); }
      set { AppSettings.SetProperty(ref _allowUserMultipleAddressLogin, value); }
    }

    private static int? _loginFailureCountMaximum;
    /// <summary>
    /// 登录失败次数极限
    /// 小于等于0不限制
    /// 缺省为 0
    /// </summary>
    public static int LoginFailureCountMaximum
    {
      get { return AppSettings.GetProperty(ref _loginFailureCountMaximum, 0); }
      set { AppSettings.SetProperty(ref _loginFailureCountMaximum, value); }
    }

    private static int? _sessionExpiresMinutes;
    /// <summary>
    /// 会话过期时间(分钟)
    /// 小于等于0不限制
    /// 不控制Guest用户
    /// 缺省为 0
    /// </summary>
    public static int SessionExpiresMinutes
    {
      get { return AppSettings.GetProperty(ref _sessionExpiresMinutes, 0); }
      set { AppSettings.SetProperty(ref _sessionExpiresMinutes, value); }
    }

    private static bool? _remindPasswordComplexity;
    /// <summary>
    /// 提醒口令复杂度(长度需大于{PasswordLengthMinimize}位且含数字和大小写字母或特殊字符的类别达到{PasswordComplexityMinimize}种)
    /// 缺省为 false
    /// </summary>
    public static bool RemindPasswordComplexity
    {
      get { return AppSettings.GetProperty(ref _remindPasswordComplexity, false); }
      set { AppSettings.SetProperty(ref _remindPasswordComplexity, value); }
    }

    private static bool? _forcedPasswordComplexity;
    /// <summary>
    /// 强制口令复杂度(长度需大于{PasswordLengthMinimize}位且含数字和大小写字母或特殊字符的类别达到{PasswordComplexityMinimize}种)
    /// 缺省为 false
    /// </summary>
    public static bool ForcedPasswordComplexity
    {
      get { return AppSettings.GetProperty(ref _forcedPasswordComplexity, false); }
      set { AppSettings.SetProperty(ref _forcedPasswordComplexity, value); }
    }

    private static int? _passwordLengthMinimize;
    /// <summary>
    /// 口令长度最小值
    /// 缺省为 6
    /// </summary>
    public static int PasswordLengthMinimize
    {
      get { return AppSettings.GetProperty(ref _passwordLengthMinimize, 6); }
      set { AppSettings.SetProperty(ref _passwordLengthMinimize, value); }
    }

    private static int? _passwordComplexityMinimize;
    /// <summary>
    /// 口令复杂度最小值
    /// 缺省为 2
    /// </summary>
    public static int PasswordComplexityMinimize
    {
      get { return AppSettings.GetProperty(ref _passwordComplexityMinimize, 2); }
      set { AppSettings.SetProperty(ref _passwordComplexityMinimize, value); }
    }

    private static int? _passwordExpirationRemindDays;
    /// <summary>
    /// 口令变更天数提醒
    /// 小于等于0不提醒
    /// 缺省为 0
    /// </summary>
    public static int PasswordExpirationRemindDays
    {
      get { return AppSettings.GetProperty(ref _passwordExpirationRemindDays, 0); }
      set { AppSettings.SetProperty(ref _passwordExpirationRemindDays, value); }
    }

    private static int? _passwordExpirationDays;
    /// <summary>
    /// 口令失效过期天数
    /// 小于等于0不失效
    /// 缺省为 0
    /// </summary>
    public static int PasswordExpirationDays
    {
      get { return AppSettings.GetProperty(ref _passwordExpirationDays, 0); }
      set { AppSettings.SetProperty(ref _passwordExpirationDays, value); }
    }

    /// <summary>
    /// (用户/功能)未配置角色代表授权规则为禁用
    /// 缺省为 false
    /// </summary>
    public static bool EmptyRolesIsDeny
    {
      get { return DataSecurity.GetEmptyRolesIsDeny(); }
      set { DataSecurity.SetEmptyRolesIsDeny(value); }
    }

    /// <summary>
    /// 宽松的授权
    /// 缺省为 true
    /// true: 只要用户拥有的角色之一不在禁用角色队列里，就不被禁用
    /// false: 只要用户拥有的角色之一在禁用角色队列里，就被禁用
    /// </summary>
    public static bool EasyAuthorization
    {
      get { return DataSecurity.GetEasyAuthorization(); }
      set { DataSecurity.SetEasyAuthorization(value); }
    }

    private static bool? _needMarkLogin;
    /// <summary>
    /// 用户登录需要留痕
    /// 缺省为 false
    /// </summary>
    public static bool NeedMarkLogin
    {
      get { return AppSettings.GetProperty(ref _needMarkLogin, false); }
      set { AppSettings.SetProperty(ref _needMarkLogin, value); }
    }

    private static bool? _noLogin;
    /// <summary>
    /// 禁止用户登录
    /// </summary>
    public static bool NoLogin
    {
      get { return AppSettings.GetProperty(ref _noLogin, false); }
      set { AppSettings.SetProperty(ref _noLogin, value); }
    }

    private static string _noLoginReason;
    /// <summary>
    /// 禁止用户登录原因
    /// </summary>
    public static string NoLoginReason
    {
      get { return AppSettings.GetProperty(ref _noLoginReason, String.Empty); }
      set { AppSettings.SetProperty(ref _noLoginReason, value); }
    }
  }
}
