using System;
using System.Collections.Generic;
using System.DirectoryServices;
using Phenix.Core;
using Phenix.Core.Plugin;
using Phenix.Core.Security;
using Phenix.Services.Library;

namespace Phenix.Security.Plugin.Authoriser
{
  public class Plugin : PluginBase<Plugin>, IAuthoriser
  {
    public Plugin()
      :base() { }

    #region 属性

    private string _ldapPath;
    /// <summary>
    /// LDAP路径
    /// </summary>
    public string LdapPath
    {
      get { return AppSettings.GetProperty(ref _ldapPath, @"127.0.0.1/DC=[domain name]"); }
      set { AppSettings.SetProperty(ref _ldapPath, value); }
    }

    private string _userNamePrdfix;
    /// <summary>
    /// 用户名前缀
    /// </summary>
    public string UserNamePrdfix
    {
      get { return AppSettings.GetProperty(ref _userNamePrdfix, String.Empty); }
      set { AppSettings.SetProperty(ref _userNamePrdfix, value); }
    }

    private string _adminName;
    /// <summary>
    /// 管理员用户名
    /// </summary>
    public string AdminName
    {
      get { return AppSettings.GetProperty(ref _adminName, "administrator"); }
      set { AppSettings.SetProperty(ref _adminName, value); }
    }

    private string _adminPassword;
    /// <summary>
    /// 管理员口令
    /// </summary>
    public string AdminPassword
    {
      get { return AppSettings.GetProperty(ref _adminPassword, String.Empty, true, true); }
      set { AppSettings.SetProperty(ref _adminPassword, value, true, true); }
    }

    #endregion

    #region 方法

    #region 覆写 PluginBase

    /// <summary>
    /// 初始化
    /// 由 PluginHost 调用
    /// </summary>
    protected override IList<MessageNotifyEventArgs> Initialization()
    {
      Phenix.Services.Library.AppHub.Authoriser = this;
      return null;
    }

    /// <summary>
    /// 终止化
    /// 由 PluginHost 调用
    /// </summary>
    protected override void Finalization()
    {
      Phenix.Services.Library.AppHub.Authoriser = null;
    }

    /// <summary>
    /// 设置
    /// 由 PluginHost 调用
    /// </summary>
    /// <param name="sender">发起对象</param>
    /// <returns>按需返回</returns>
    public override object Setup(object sender)
    {
      if (SetupForm.Execute(this))
        SendMessage(new MessageNotifyEventArgs(MessageNotifyType.Information, "Setup", String.Format("LdapPath = {0}, UserNamePrdfix = {1}", LdapPath, UserNamePrdfix)));
      return this;
    }

    /// <summary>
    /// 启动
    /// 由 PluginHost 调用
    /// </summary>
    /// <returns>确定启动</returns>
    protected override bool Start()
    {
      Phenix.Services.Library.AppHub.Authoriser = this;
      return true;
    }

    /// <summary>
    /// 暂停
    /// 由 PluginHost 调用
    /// </summary>
    /// <returns>确定停止</returns>
    protected override bool Suspend()
    {
      Phenix.Services.Library.AppHub.Authoriser = null;
      return true;
    }

    #endregion

    /// <summary>
    /// 转译用户
    /// </summary>
    public UserIdentity Translation(string userNumber)
    {
      //在客户端上调用logOn()/logOnVerify()函数会在服务端执行到本函数
      return null;
    }

    /// <summary>
    /// 登录验证
    /// </summary>
    public bool? LogOn(string userNumber, string password)
    {
      //在客户端上调用logOn()/logOnVerify()函数会在服务端执行到本函数
      try
      {
        return LogOn(LdapPath, UserNamePrdfix, userNumber, password);
      }
      catch (Exception)
      {
        return false;
      }
    }

    internal static bool LogOn(string ldapPath, string userNamePrdfix, string userNumber, string password)
    {
      userNumber = String.Format("{0}{1}", userNamePrdfix, userNumber);
      using (DirectoryEntry entry = new DirectoryEntry(String.Format(@"LDAP://{0}", ldapPath), userNumber, password, AuthenticationTypes.Secure))
      using (DirectorySearcher searcher = new DirectorySearcher(entry))
      {
        searcher.Filter = "(&(objectClass=user)(SAMAccountName=" + userNumber + "))";
        SearchResult one = searcher.FindOne();
        return one != null;
      }
    }

    /// <summary>
    /// 登录核实
    /// </summary>
    public bool? LogOnVerify(string userNumber, string tag)
    {
      //在客户端上调用logOnVerify()函数会在服务端执行到本函数
      return null;
    }

    /// <summary>
    /// 修改登录口令
    /// </summary>
    public bool ChangePassword(string userNumber, string newPassword)
    {
      //在客户端上调用changePassword()函数会在服务端执行到本函数
      try
      {
        return ChangePassword(LdapPath, UserNamePrdfix, AdminName, AdminPassword, userNumber, newPassword);
      }
      catch (Exception)
      {
        return false;
      }
    }

    internal static bool ChangePassword(string ldapPath, string userNamePrdfix, string adminName, string adminPassword, string userNumber, string newPassword)
    {
      using (DirectoryEntry entry = new DirectoryEntry(String.Format(@"LDAP://{0}", ldapPath), adminName, adminPassword, AuthenticationTypes.Secure))
      using (DirectorySearcher searcher = new DirectorySearcher(entry))
      {
        searcher.Filter = "(&(objectClass=user)(SAMAccountName=" + String.Format("{0}{1}", userNamePrdfix, userNumber) + "))";
        SearchResult one = searcher.FindOne();
        if (one != null)
          using (DirectoryEntry userEntry = one.GetDirectoryEntry())
          {
            userEntry.Invoke("SetPassword", newPassword);
            userEntry.CommitChanges();
            return true;
          }
      }
      return false;
    }

    #endregion
  }
}