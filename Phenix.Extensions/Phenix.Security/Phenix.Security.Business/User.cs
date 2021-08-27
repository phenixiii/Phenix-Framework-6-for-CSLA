using System;
using System.Data.Common;
using Phenix.Core.Mapping;

namespace Phenix.Security.Business
{
  /// <summary>
  /// 用户
  /// </summary>
  [Serializable]
  [Phenix.Core.Mapping.ReadOnly]
  public class UserReadOnly : User<UserReadOnly>
  {
  }

  /// <summary>
  /// 用户清单
  /// </summary>
  [Serializable]
  public class UserReadOnlyList : Phenix.Business.BusinessListBase<UserReadOnlyList, UserReadOnly>
  {
  }

  /// <summary>
  /// 用户
  /// </summary>
  [Serializable]
  public class User : User<User>
  {
    #region 属性

    /// <summary>
    /// 是否允许设置数据
    /// 只读则为false
    /// </summary>
    [System.ComponentModel.Browsable(false)]
    [System.ComponentModel.DataAnnotations.Display(AutoGenerateField = false)]
    public override bool AllowSet
    {
      get
      {
        if (!base.AllowSet)
          return false;
        return String.Compare(this.UserNumber, Phenix.Core.Security.UserIdentity.AdminUserNumber, false) != 0;
      }
    }

    #region 勾选角色全集

    /// <summary>
    /// 分配的角色
    /// </summary>
    public UserRoleList UserRoles
    {
      get { return GetCompositionDetail<UserRoleList, UserRole>(UserRoleList.Fetch(true)); }
    }

    /// <summary>
    /// 供勾选的角色全集
    /// </summary>
    public RoleReadOnlyList SelectableRoles
    {
      get { return UserRoles.CollatingSelectableList<RoleReadOnlyList, RoleReadOnly>(RoleReadOnlyList.Fetch(true)); }
    }

    #endregion

    #region 勾选角色全集

    /// <summary>
    /// 分配的可授权角色
    /// </summary>
    public UserGrantRoleList UserGrantRoles
    {
      get { return GetCompositionDetail<UserGrantRoleList, UserGrantRole>(UserGrantRoleList.Fetch(true)); }
    }

    /// <summary>
    /// 供勾选的可授权角色全集
    /// </summary>
    public RoleReadOnlyList SelectableGrantRoles
    {
      get { return UserGrantRoles.CollatingSelectableList<RoleReadOnlyList, RoleReadOnly>(RoleReadOnlyList.Fetch(true)); }
    }

    #endregion

    #region 勾选切片全集

    /// <summary>
    /// 分配的切片
    /// </summary>
    public UserSectionList UserSections
    {
      get { return GetCompositionDetail<UserSectionList, UserSection>(UserSectionList.Fetch()); }
    }

    /// <summary>
    /// 供勾选的切片全集
    /// </summary>
    public SectionReadOnlyList SelectableSections
    {
      get { return UserSections.CollatingSelectableList<SectionReadOnlyList, SectionReadOnly>(SectionReadOnlyList.Fetch()); }
    }

    #endregion

    /// <summary>
    /// 用户日志
    /// </summary>
    public UserLogList UserLogs
    {
      get { return GetCompositionDetail<UserLogList, UserLog>(); }
    }

    #endregion

    #region 方法

    /// <summary>
    /// 登录加锁
    /// </summary>
    /// <returns>是否正常提交</returns>
    [Phenix.Core.Mapping.MethodAttribute(FriendlyName = "登录加锁")]
    public bool Lock()
    {
      CanExecuteMethod(LockMethod, true);

      bool editMode = Root.EditMode;
      if (!editMode)
        Root.BeginEdit();
      try
      {
        Locked = true;
        if (!editMode)
          return Root.Save() != null;
        return false;
      }
      catch (Exception)
      {
        if (!editMode)
          Root.CancelEdit();
        throw;
      }
    }
    /// <summary>
    /// 登录加锁
    /// </summary>
    public static readonly Phenix.Business.MethodInfo LockMethod = RegisterMethod(c => c.Lock());

    /// <summary>
    /// 登录解锁
    /// </summary>
    /// <returns>是否正常提交</returns>
    [Phenix.Core.Mapping.MethodAttribute(FriendlyName = "登录解锁")]
    public bool Unlock()
    {
      CanExecuteMethod(UnlockMethod, true);

      bool editMode = Root.EditMode;
      if (!editMode)
        Root.BeginEdit();
      try
      {
        Locked = false;
        if (!editMode)
          return Root.Save() != null;
        return false;
      }
      catch (Exception)
      {
        if (!editMode)
          Root.CancelEdit();
        throw;
      }
    }
    /// <summary>
    /// 登录解锁
    /// </summary>
    public static readonly Phenix.Business.MethodInfo UnlockMethod = RegisterMethod(c => c.Unlock());

    /// <summary>
    /// 解锁口令
    /// </summary>
    /// <returns>是否正常提交</returns>
    public bool UnlockPassword()
    {
      bool result = Phenix.Core.Security.UserIdentity.UnlockPassword(UserNumber);
      //if (result)
      //  UserUnlockPasswordWorkflowStartCommand.Execute(new TaskContext(UserNumber, null, true));
      return result;
    }

    /// <summary>
    /// 重置口令
    /// </summary>
    /// <returns>是否正常提交</returns>
    public bool ResetPassword()
    {
      bool result = Phenix.Core.Security.UserIdentity.ResetPassword(UserNumber);
      //if (result)
      //  UserResetPasswordWorkflowStartCommand.Execute(new TaskContext(UserNumber, null, true));
      return result;
    }

    #endregion

    /// <summary>
    /// 注册授权规则
    /// </summary>
    protected override void AddAuthorizationRules()
    {
      AuthorizationRules.AddRule(new UserLockExecuteAuthorizationRule());
      AuthorizationRules.AddRule(new UserUnlockExecuteAuthorizationRule());
      base.AddAuthorizationRules();
    }
  }

  /// <summary>
  /// 用户清单
  /// </summary>
  [Serializable]
  public class UserList : Phenix.Business.BusinessListBase<UserList, User>
  {
    /// <summary>
    /// 是否业务对象各自使用独立事务
    /// 缺省为 false
    /// </summary>
    protected override bool AloneTransaction
    {
      get { return true; }
    }

    ///// <summary>
    ///// 不参与回滚机制并阻断Detail的回滚处理
    ///// 缺省为 false
    ///// </summary>
    //protected override bool NotUndoable
    //{
    //  get { return true; }
    //}
  }

  /// <summary>
  /// 用户
  /// </summary>
  [Phenix.Core.Mapping.ClassAttribute("PH_User", FriendlyName = "用户", PermanentExecuteAction = ExecuteAction.All), System.SerializableAttribute(), System.ComponentModel.DisplayNameAttribute("用户")]
  [Phenix.Core.Mapping.ClassDetail("PH_USER_ROLE", "UR_US_ID", null, CascadingDelete = true, FriendlyName = "用户角色")]
  [Phenix.Core.Mapping.ClassDetail("PH_USER_GRANT_ROLE", "GR_US_ID", null, CascadingDelete = true, FriendlyName = "用户可授权角色")]
  [Phenix.Core.Mapping.ClassDetail("PH_USERLOG", "US_ID", null, CascadingDelete = true, FriendlyName = "用户日志")]
  [Phenix.Core.Mapping.ClassDetail("PH_USER_SECTION", "US_US_ID", null, CascadingDelete = true, FriendlyName = "用户-切片")]
  public abstract class User<T> : Phenix.Business.BusinessBase<T> where T : User<T>
  {
    /// <summary>
    /// US_ID
    /// </summary>
    public static readonly Phenix.Business.PropertyInfo<long?> US_IDProperty = RegisterProperty<long?>(c => c.US_ID);
    [Phenix.Core.Mapping.Field(FriendlyName = "US_ID", TableName = "PH_User", ColumnName = "US_ID", IsPrimaryKey = true, NeedUpdate = true)]
    private long? _US_ID;
    /// <summary>
    /// US_ID
    /// </summary>
    [System.ComponentModel.DisplayName("US_ID")]
    public long? US_ID
    {
      get { return GetProperty(US_IDProperty, _US_ID); }
      set
      {
        SetProperty(US_IDProperty, ref _US_ID, value);
      }
    }

    [System.ComponentModel.Browsable(false)]
    [System.ComponentModel.DataAnnotations.Display(AutoGenerateField = false)]
    public override string PrimaryKey
    {
      get { return US_ID.ToString(); }
    }

    /// <summary>
    /// 工号
    /// </summary>
    public static readonly Phenix.Business.PropertyInfo<string> UserNumberProperty = RegisterProperty<string>(c => c.UserNumber);
    [Phenix.Core.Mapping.FieldRule(StringUpperCase = true)]
    [Phenix.Core.Mapping.Field(FriendlyName = "工号", Alias = "US_UserNumber", TableName = "PH_User", ColumnName = "US_UserNumber", NeedUpdate = true)]
    private string _userNumber;
    /// <summary>
    /// 工号
    /// </summary>
    [System.ComponentModel.DisplayName("工号")]
    public string UserNumber
    {
      get { return GetProperty(UserNumberProperty, _userNumber); }
      set { SetProperty(UserNumberProperty, ref _userNumber, value); }
    }

    /// <summary>
    /// 姓名
    /// </summary>
    public static readonly Phenix.Business.PropertyInfo<string> NameProperty = RegisterProperty<string>(c => c.Name);
    [Phenix.Core.Mapping.Field(FriendlyName = "姓名", Alias = "US_Name", TableName = "PH_User", ColumnName = "US_Name", NeedUpdate = true, IsNameColumn = true, InLookUpColumn = true, InLookUpColumnDisplay = true,
      PermanentExecuteModify = ExecuteModify.All)]
    private string _name;
    /// <summary>
    /// 姓名
    /// </summary>
    [System.ComponentModel.DisplayName("姓名")]
    public string Name
    {
      get { return GetProperty(NameProperty, _name); }
      set { SetProperty(NameProperty, ref _name, value); }
    }

    /// <summary>
    /// 登录时间
    /// </summary>
    public static readonly Phenix.Business.PropertyInfo<DateTime?> LoginProperty = RegisterProperty<DateTime?>(c => c.Login);
    [Phenix.Core.Mapping.Field(FriendlyName = "登录时间", Alias = "US_Login", TableName = "PH_User", ColumnName = "US_Login", NeedUpdate = true)]
    private DateTime? _login;
    /// <summary>
    /// 登录时间
    /// </summary>
    [System.ComponentModel.DisplayName("登录时间")]
    public DateTime? Login
    {
      get { return GetProperty(LoginProperty, _login); }
      set { SetProperty(LoginProperty, ref _login, value); }
    }

    /// <summary>
    /// 登出时间
    /// </summary>
    public static readonly Phenix.Business.PropertyInfo<DateTime?> LogoutProperty = RegisterProperty<DateTime?>(c => c.Logout);
    [Phenix.Core.Mapping.Field(FriendlyName = "登出时间", Alias = "US_Logout", TableName = "PH_User", ColumnName = "US_Logout", NeedUpdate = true)]
    private DateTime? _logout;
    /// <summary>
    /// 登出时间
    /// </summary>
    [System.ComponentModel.DisplayName("登出时间")]
    public DateTime? Logout
    {
      get { return GetProperty(LogoutProperty, _logout); }
      set { SetProperty(LogoutProperty, ref _logout, value); }
    }

    /// <summary>
    /// 登录失败时间
    /// </summary>
    public static readonly Phenix.Business.PropertyInfo<DateTime?> LoginFailureProperty = RegisterProperty<DateTime?>(c => c.LoginFailure);
    [Phenix.Core.Mapping.Field(FriendlyName = "登录失败时间", Alias = "US_LoginFailure", TableName = "PH_User", ColumnName = "US_LoginFailure", NeedUpdate = true)]
    private DateTime? _loginFailure;
    /// <summary>
    /// 登录失败时间
    /// </summary>
    [System.ComponentModel.DisplayName("登录失败时间")]
    public DateTime? LoginFailure
    {
      get { return GetProperty(LoginFailureProperty, _loginFailure); }
      set { SetProperty(LoginFailureProperty, ref _loginFailure, value); }
    }

    /// <summary>
    /// 登录失败次数
    /// </summary>
    public static readonly Phenix.Business.PropertyInfo<int?> LoginFailureCountProperty = RegisterProperty<int?>(c => c.LoginFailureCount);
    [Phenix.Core.Mapping.Field(FriendlyName = "登录失败次数", Alias = "US_LoginFailureCount", TableName = "PH_User", ColumnName = "US_LoginFailureCount", NeedUpdate = true)]
    private int? _loginFailureCount;
    /// <summary>
    /// 登录失败次数
    /// </summary>
    [System.ComponentModel.DisplayName("登录失败次数")]
    public int? LoginFailureCount
    {
      get { return GetProperty(LoginFailureCountProperty, _loginFailureCount); }
      set { SetProperty(LoginFailureCountProperty, ref _loginFailureCount, value); }
    }

    /// <summary>
    /// 登录地址
    /// </summary>
    public static readonly Phenix.Business.PropertyInfo<string> LoginAddressProperty = RegisterProperty<string>(c => c.LoginAddress);
    [Phenix.Core.Mapping.Field(FriendlyName = "登录地址", Alias = "US_LoginAddress", TableName = "PH_User", ColumnName = "US_LoginAddress", NeedUpdate = true)]
    private string _loginAddress;
    /// <summary>
    /// 登录地址
    /// </summary>
    [System.ComponentModel.DisplayName("登录地址")]
    public string LoginAddress
    {
      get { return GetProperty(LoginAddressProperty, _loginAddress); }
      set { SetProperty(LoginAddressProperty, ref _loginAddress, value); }
    }

    /// <summary>
    /// 所属部门
    /// </summary>
    public static readonly Phenix.Business.PropertyInfo<long?> US_DP_IDProperty = RegisterProperty<long?>(c => c.US_DP_ID);
    [Phenix.Core.Mapping.Field(FriendlyName = "所属部门", TableName = "PH_User", ColumnName = "US_DP_ID", NeedUpdate = true)]
    [Phenix.Core.Mapping.FieldLink("PH_DEPARTMENT", "DP_ID")]
    private long? _US_DP_ID;
    /// <summary>
    /// 所属部门
    /// </summary>
    [System.ComponentModel.DisplayName("所属部门")]
    public long? US_DP_ID
    {
      get { return GetProperty(US_DP_IDProperty, _US_DP_ID); }
      set { SetProperty(US_DP_IDProperty, ref _US_DP_ID, value); }
    }

    /// <summary>
    /// 担任岗位
    /// </summary>
    public static readonly Phenix.Business.PropertyInfo<long?> US_PT_IDProperty = RegisterProperty<long?>(c => c.US_PT_ID);
    [Phenix.Core.Mapping.Field(FriendlyName = "担任岗位", TableName = "PH_User", ColumnName = "US_PT_ID", NeedUpdate = true)]
    [Phenix.Core.Mapping.FieldLink("PH_POSITION", "PT_ID")]
    private long? _US_PT_ID;
    /// <summary>
    /// 担任岗位
    /// </summary>
    [System.ComponentModel.DisplayName("担任岗位")]
    public long? US_PT_ID
    {
      get { return GetProperty(US_PT_IDProperty, _US_PT_ID); }
      set { SetProperty(US_PT_IDProperty, ref _US_PT_ID, value); }
    }

    /// <summary>
    /// 锁定
    /// </summary>
    public static readonly Phenix.Business.PropertyInfo<bool?> LockedProperty = RegisterProperty<bool?>(c => c.Locked);
    [Phenix.Core.Mapping.Field(FriendlyName = "锁定", Alias = "US_Locked", TableName = "PH_User", ColumnName = "US_Locked", NeedUpdate = true)]
    private bool? _locked;
    /// <summary>
    /// 锁定
    /// </summary>
    [System.ComponentModel.DisplayName("锁定")]
    public bool? Locked
    {
      get { return GetProperty(LockedProperty, _locked); }
      set { SetProperty(LockedProperty, ref _locked, value); }
    }

    /// <summary>
    /// US_PASSWORD
    /// </summary>
    public static readonly Phenix.Business.PropertyInfo<string> PasswordProperty = RegisterProperty<string>(c => c.Password, arg => "*");
    [Phenix.Core.Mapping.Field(FriendlyName = "US_PASSWORD", Alias = "US_Password", TableName = "PH_User", ColumnName = "US_Password", NeedUpdate = true)]
    private string _password;
    /// <summary>
    /// US_PASSWORD
    /// </summary>
    [System.ComponentModel.DataAnnotations.Display(Name = "US_PASSWORD")]
    [System.ComponentModel.DisplayName("US_PASSWORD")]
    public string Password
    {
      get { return GetProperty(PasswordProperty, _password); }
      set { SetProperty(PasswordProperty, ref _password, value); }
    }

    /// <summary>
    /// New
    /// </summary>
    public static T New(string userNumber, string name, DateTime? login, DateTime? logout, DateTime? loginFailure, int? loginFailureCount, string loginAddress, long? US_DP_ID, long? US_PT_ID, bool? locked, string password)
    {
      T result = NewPure();
      result._userNumber = userNumber;
      result._name = name;
      result._login = login;
      result._logout = logout;
      result._loginFailure = loginFailure;
      result._loginFailureCount = loginFailureCount;
      result._loginAddress = loginAddress;
      result._US_DP_ID = US_DP_ID;
      result._US_PT_ID = US_PT_ID;
      result._locked = locked;
      result._password = password;
      return result;
    }

    #region 方法

    /// <summary>
    /// 新增本对象数据之后(运行在持久层的程序域里)
    /// </summary>
    /// <param name="transaction">数据库事务</param>
    protected override void OnInsertedSelf(DbTransaction transaction)
    {
      Phenix.Core.Security.UserIdentity.ChangePassword(transaction, UserNumber, UserNumber);
    }

    #endregion
  }
}
