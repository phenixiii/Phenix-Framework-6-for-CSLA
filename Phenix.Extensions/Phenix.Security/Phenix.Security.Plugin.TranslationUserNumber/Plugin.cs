using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using Phenix.Core;
using Phenix.Core.Data;
using Phenix.Core.Plugin;
using Phenix.Core.Security;
using Phenix.Core.Security.Exception;
using Phenix.Services.Library;

namespace Phenix.Security.Plugin.TranslationUserNumber
{
  public class Plugin : PluginBase<Plugin>, IAuthoriser
  {
    public Plugin()
      :base() { }

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
      if (userNumber == UserIdentity.GuestUserNumber)
        return null;
      return DefaultDatabase.ExecuteGet(Translation, userNumber);
    }

    private static UserIdentity Translation(DbConnection connection, string userNumber)
    {
      using (DataReader reader = new DataReader(connection,
@"select SMI_MENMBER_NO
  from SYS_MENMBER_INFO
  where SMI_MENMBER_NO_MD5 = :SMI_MENMBER_NO_MD5
    or SMI_TEL_MD5 = :SMI_TEL_MD5
    or SMI_EMAIL_MD5 = :SMI_EMAIL_MD5
    or SMI_TAX_ID_NO_MD5 = :SMI_TAX_ID_NOL_MD5",
        CommandBehavior.SingleRow))
      {
        //XXX_MD5字段值是XXX字段值经Phenix.Core.Security.Cryptography.MD5CryptoTextProvider.ComputeHash(XXX)处理后事先存储的
        //在客户端上调用logOn()/logOnVerify()函数时所传入的userNumber参数值需经CryptoJS.MD5(会员号/手机/邮箱/身份证号/税号))处理
        reader.CreateParameter("SMI_MENMBER_NO_MD5", userNumber);
        reader.CreateParameter("SMI_TEL_MD5", userNumber);
        reader.CreateParameter("SMI_EMAIL_MD5", userNumber);
        reader.CreateParameter("SMI_TAX_ID_NOL_MD5", userNumber);
        if (reader.Read())
          return new UserIdentity(reader.GetNullableString(0), null); //返回值应该与Ph_User表US_UserNumber字段值匹配上
      }
//      using (DataReader reader = new DataReader(connection,
//@"select US_UserNumber
//  from PH_User
//  where US_Name = :US_Name or US_UserNumber = :US_UserNumber",
//        CommandBehavior.SingleRow, false))
//      {
//        reader.CreateParameter("US_Name", userNumber);
//        reader.CreateParameter("US_UserNumber", userNumber);
//        if (reader.Read())
//          return reader.GetNullableString(0);
//      }
      throw new UserNotFoundException(userNumber);
    }

    /// <summary>
    /// 登录验证
    /// </summary>
    public bool? LogOn(string userNumber, string password)
    {
      //在客户端上调用logOn()/logOnVerify()函数会在服务端执行到本函数
      return null;
    }

    /// <summary>
    /// 登录核实
    /// </summary>
    public bool? LogOnVerify(string userNumber, string tag)
    {
      //在客户端上调用logOnVerify()函数会在服务端执行到本函数
      if (userNumber == UserIdentity.GuestUserNumber)
        return null;
      return DefaultDatabase.ExecuteGet(LogOnVerify, userNumber, tag);
    }

    private static bool? LogOnVerify(DbConnection connection, string userNumber, string tag)
    {
      //请与客户端约定传入的tag参数值内容，比如第三方业务域信息（业务ID、业务流水ID和业务用户ID）
      return null; 
    }

    /// <summary>
    /// 修改登录口令
    /// </summary>
    public bool ChangePassword(string userNumber, string newPassword)
    {
      //在客户端上调用changePassword()函数会在服务端执行到本函数
      return true;
    }

    #endregion
  }
}