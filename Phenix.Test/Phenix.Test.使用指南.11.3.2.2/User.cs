using System;

namespace Phenix.Test.使用指南._11._3._2._2
{
  /// <summary>
  /// User
  /// </summary>
  [Serializable]
  public class User : User<User>
  {
    private User()
    {
      //禁止添加代码
    }

    #region 属性

    #region 普通方法

    //* 组合关系的从业务对象集合
    /// <summary>
    /// 用户角色
    /// </summary>
    public UserRoleInfoList UserRoleInfos_Normal
    {
      get { return GetCompositionDetail<UserRoleInfoList, UserRoleInfo>(); }
    }

    #endregion

    #region 特殊方法

    //* 组合关系的从业务对象集合（从Owner的AllUserRoleInfoList里过滤出来）
    /// <summary>
    /// 用户角色
    /// </summary>
    public UserRoleInfoList UserRoleInfos_Special
    {
      get { return GetCompositionDetail<UserRoleInfoList, UserRoleInfo>(Owner is UserList ? ((UserList)Owner).AllUserRoleInfoList : null); }
    }

    #endregion

    #endregion
  }

  /// <summary>
  /// User清单
  /// </summary>
  [Serializable]
  public class UserList : Phenix.Business.BusinessListBase<UserList, User>
  {
    private UserList()
    {
      //禁止添加代码
    }

    #region 特殊方法

    //* 所有User下的UserRoleInfo对象
    [NonSerialized]
    [Csla.NotUndoable]
    private UserRoleInfoList _allUserRoleInfoList;
    internal UserRoleInfoList AllUserRoleInfoList
    {
      get
      {
        if (_allUserRoleInfoList == null)
          _allUserRoleInfoList = FetchDetail<UserRoleInfoList, UserRoleInfo>();
        return _allUserRoleInfoList;
      }
    }

    //* 提交自己之后清空缓存
    /// <summary>
    /// 保存业务对象集合之后
    /// 在执行Save()的程序域里被调用
    /// </summary>
    /// <param name="ex">错误信息</param>
    /// <returns>发生错误时的友好提示信息, 缺省为 null 直接抛出原异常，否则会抛出SaveException并打包本信息及原异常</returns>
    protected override string OnSavedSelf(Exception ex)
    {
      _allUserRoleInfoList = null;
      return base.OnSavedSelf(ex);
    }

    #endregion
  }

  /// <summary>
  /// User
  /// </summary>
  [Phenix.Core.Mapping.ClassAttribute("PH_USER", FriendlyName = "User"), System.ComponentModel.DisplayNameAttribute("User"), System.SerializableAttribute()]
  public abstract class User<T> : Phenix.Business.BusinessBase<T> where T : User<T>
  {
    /// <summary>
    /// US_ID
    /// </summary>
    public static readonly Phenix.Business.PropertyInfo<long?> US_IDProperty = RegisterProperty<long?>(c => c.US_ID);
    [Phenix.Core.Mapping.Field(FriendlyName = "US_ID", TableName = "PH_USER", ColumnName = "US_ID", IsPrimaryKey = true, NeedUpdate = true)]
    private long? _US_ID;
    /// <summary>
    /// US_ID
    /// </summary>
    [System.ComponentModel.DisplayName("US_ID")]
    public long? US_ID
    {
      get { return GetProperty(US_IDProperty, _US_ID); }
      internal set
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
    /// US_USERNUMBER
    /// </summary>
    public static readonly Phenix.Business.PropertyInfo<string> UsernumberProperty = RegisterProperty<string>(c => c.Usernumber);
    [Phenix.Core.Mapping.Field(FriendlyName = "US_USERNUMBER", Alias = "US_USERNUMBER", TableName = "PH_USER", ColumnName = "US_USERNUMBER", NeedUpdate = true)]
    private string _usernumber;
    /// <summary>
    /// US_USERNUMBER
    /// </summary>
    [System.ComponentModel.DisplayName("US_USERNUMBER")]
    public string Usernumber
    {
      get { return GetProperty(UsernumberProperty, _usernumber); }
      set { SetProperty(UsernumberProperty, ref _usernumber, value); }
    }

    /// <summary>
    /// US_NAME
    /// </summary>
    public static readonly Phenix.Business.PropertyInfo<string> NameProperty = RegisterProperty<string>(c => c.Name);
    [Phenix.Core.Mapping.Field(FriendlyName = "US_NAME", Alias = "US_NAME", TableName = "PH_USER", ColumnName = "US_NAME", NeedUpdate = true, IsNameColumn = true, InLookUpColumn = true, InLookUpColumnDisplay = true)]
    private string _name;
    /// <summary>
    /// US_NAME
    /// </summary>
    [System.ComponentModel.DisplayName("US_NAME")]
    public string Name
    {
      get { return GetProperty(NameProperty, _name); }
      set { SetProperty(NameProperty, ref _name, value); }
    }

    /// <summary>
    /// US_LOCKED
    /// </summary>
    public static readonly Phenix.Business.PropertyInfo<bool?> LockedProperty = RegisterProperty<bool?>(c => c.Locked);
    [Phenix.Core.Mapping.Field(FriendlyName = "US_LOCKED", Alias = "US_LOCKED", TableName = "PH_USER", ColumnName = "US_LOCKED", NeedUpdate = true)]
    private int? _locked;
    /// <summary>
    /// US_LOCKED
    /// </summary>
    [System.ComponentModel.DisplayName("US_LOCKED")]
    public bool? Locked
    {
      get { return GetPropertyConvert(LockedProperty, _locked); }
      set { SetPropertyConvert(LockedProperty, ref _locked, value); }
    }

    /// <summary>
    /// New
    /// </summary>
    public static T New(string usernumber, string name, int? locked)
    {
      T result = NewPure();
      result._usernumber = usernumber;
      result._name = name;
      result._locked = locked;
      return result;
    }

    /// <summary>
    /// SetFieldValues
    /// </summary>
    protected void SetFieldValues(string usernumber, string name, int? locked)
    {
      InitOldFieldValues();
      _usernumber = usernumber;
      _name = name;
      _locked = locked;
      MarkDirty();
    }
  }
}
