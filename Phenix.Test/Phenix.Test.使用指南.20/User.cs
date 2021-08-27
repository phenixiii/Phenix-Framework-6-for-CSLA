using System;
using System.Collections.Generic;
using System.Data;

namespace Phenix.Test.使用指南._20
{
  /// <summary>
  /// User
  /// </summary>
  [Serializable]
  [Phenix.Core.Mapping.ReadOnly]
  public class UserReadOnly : User<UserReadOnly>
  {
    private UserReadOnly()
    {
      //禁止添加代码
    }
  }

  /// <summary>
  /// User清单
  /// </summary>
  [Serializable]
  public class UserReadOnlyList : Phenix.Business.BusinessListBase<UserReadOnlyList, UserReadOnly>
  {
    private UserReadOnlyList()
    {
      //禁止添加代码
    }

    //* 可拦截到针对本集合对象Item的动态刷新事件
    #region 拦截动态刷新事件

    //private List<string> _renovateFieldNames;
    private bool _dataLoaded;
    /// <summary>
    /// 收到并已分析数据集资料
    /// </summary>
    public bool DataLoaded
    {
      //get { return _renovateFieldNames != null; }
      get { return _dataLoaded; }
    }

    /// <summary>
    /// 分析数据集资料
    /// </summary>
    protected override bool AnalyseDataInfo(DataTable data)
    {
      bool result = base.AnalyseDataInfo(data);

      if (result)
      {
        Console.WriteLine("收到数据集资料: {0} 条 {1}", data.Rows.Count, data.Rows.Count == Count ? "ok" : "error");
        _dataLoaded = true;
      }
      else
        Console.WriteLine("接收数据集资料 error");

      //int i = 0;
      //foreach (DataRow row in data.Rows)
      //{
      //  i = i + 1;
      //  Console.Write("第{0}条： ", i);
      //  foreach (object fieldValue in row.ItemArray)
      //    Console.Write("{0},", fieldValue);
      //  Console.WriteLine();
      //}
      //Console.WriteLine();

      //List<string> renovateFieldNames = new List<string>(data.Columns.Count);
      //foreach (DataColumn item in data.Columns)
      //  renovateFieldNames.Add(item.ColumnName);
      //renovateFieldNames.RemoveAt(renovateFieldNames.Count - 1); //剔除最后一个无用字段
      //_renovateFieldNames = renovateFieldNames;

      return result;
    }

    /// <summary>
    /// 分析动态刷新资料
    /// </summary>
    protected override Phenix.Core.Mapping.IEntity AnalyseRenovateInfo(Phenix.Core.Mapping.ExecuteAction action, object[] values)
    {
      UserReadOnly result = (UserReadOnly)base.AnalyseRenovateInfo(action, values);

      Console.WriteLine("{0} 有变更: {1} {2}, 清单数量 {3}", result.Name, Phenix.Core.Rule.EnumKeyCaption.GetCaption(action),
        !result.IsNew && !result.IsSelfDirty && !result.IsSelfDeleted ? "ok" : "error", Count);

      //Console.WriteLine("收到数据变更：{0}", Phenix.Core.Rule.EnumKeyCaption.GetCaption(action));
      //for (int i = 0; i < _renovateFieldNames.Count; i++)
      //  Console.WriteLine("{0}={1}", _renovateFieldNames[i], values[i]);
      //Console.WriteLine();

      return result;
    }

    #endregion
  }

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
  }

  /// <summary>
  /// User
  /// </summary>
  //* NeedPermanentRenovate = true
  [Phenix.Core.Mapping.ClassAttribute("PH_USER", NeedPermanentRenovate = true, FriendlyName = "User"), System.ComponentModel.DisplayNameAttribute("User"), System.SerializableAttribute()]
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
    private bool? _locked;
    /// <summary>
    /// US_LOCKED
    /// </summary>
    [System.ComponentModel.DisplayName("US_LOCKED")]
    public bool? Locked
    {
      get { return GetProperty(LockedProperty, _locked); }
      set { SetProperty(LockedProperty, ref _locked, value); }
    }

    /// <summary>
    /// 口令
    /// </summary>
    public static readonly Phenix.Business.PropertyInfo<string> PasswordProperty = RegisterProperty<string>(c => c.Password);
    [Phenix.Core.Mapping.Field(FriendlyName = "口令", Alias = "US_PASSWORD", TableName = "PH_USER", ColumnName = "US_PASSWORD", IsWatermarkColumn = true, NeedUpdate = true)]
    private string _password;
    /// <summary>
    /// 口令
    /// </summary>
    [System.ComponentModel.DisplayName("口令")]
    public string Password
    {
      get { return GetProperty(PasswordProperty, _password); }
      set { SetProperty(PasswordProperty, ref _password, value); }
    }

    /// <summary>
    /// New
    /// </summary>
    public static T New(string usernumber, string name, bool? locked, string password)
    {
      T result = NewPure();
      result._usernumber = usernumber;
      result._name = name;
      result._locked = locked;
      result._password = password;
      return result;
    }
    
    /// <summary>
    /// SetFieldValues
    /// </summary>
    protected void SetFieldValues(string usernumber, string name, bool? locked, string password)
    {
      InitOldFieldValues();
      _usernumber = usernumber;
      _name = name;
      _locked = locked;
      _password = password;
      MarkDirty();
    }
  }
}
