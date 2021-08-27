using System;

namespace Phenix.Test.使用案例.业务数据的读写.快速Save
{
  /// <summary>
  /// Role
  /// </summary>
  [Serializable]
  [Phenix.Core.Mapping.ReadOnly]
  public class RoleReadOnly : Role<RoleReadOnly>
  {
  }

  /// <summary>
  /// Role清单
  /// </summary>
  [Serializable]
  public class RoleReadOnlyList : Phenix.Business.BusinessListBase<RoleReadOnlyList, RoleReadOnly>
  {
  }

  /// <summary>
  /// Role
  /// </summary>
  [Serializable]
  public class Role : Role<Role>
  {
  }

  /// <summary>
  /// Role清单
  /// </summary>
  [Serializable]
  public class RoleList : Phenix.Business.BusinessListBase<RoleList, Role>
  {
  }

  /// <summary>
  /// Role
  /// </summary>
  [Phenix.Core.Mapping.ClassAttribute("PH_ROLE", FriendlyName = "Role"), System.ComponentModel.DisplayNameAttribute("Role"), System.SerializableAttribute()]
  public abstract class Role<T> : Phenix.Business.BusinessBase<T> where T : Role<T>
  {
    /// <summary>
    /// RL_ID
    /// </summary>
    public static readonly Phenix.Business.PropertyInfo<long?> RL_IDProperty = RegisterProperty<long?>(c => c.RL_ID);
    [Phenix.Core.Mapping.Field(FriendlyName = "RL_ID", TableName = "PH_ROLE", ColumnName = "RL_ID", IsPrimaryKey = true, NeedUpdate = true)]
    private long? _RL_ID;
    /// <summary>
    /// RL_ID
    /// </summary>
    [System.ComponentModel.DisplayName("RL_ID")]
    public long? RL_ID
    {
      get { return GetProperty(RL_IDProperty, _RL_ID); }
      internal set
      {
        SetProperty(RL_IDProperty, ref _RL_ID, value);
      }
    }

    [System.ComponentModel.Browsable(false)]
    [System.ComponentModel.DataAnnotations.Display(AutoGenerateField = false)]
    public override string PrimaryKey
    {
      get { return RL_ID.ToString(); }
    }

    /// <summary>
    /// RL_NAME
    /// </summary>
    public static readonly Phenix.Business.PropertyInfo<string> NameProperty = RegisterProperty<string>(c => c.Name);
    [Phenix.Core.Mapping.Field(FriendlyName = "RL_NAME", Alias = "RL_NAME", TableName = "PH_ROLE", ColumnName = "RL_NAME", NeedUpdate = true, IsNameColumn = true, InLookUpColumn = true, InLookUpColumnDisplay = true)]
    private string _name;
    /// <summary>
    /// RL_NAME
    /// </summary>
    [System.ComponentModel.DisplayName("RL_NAME")]
    public string Name
    {
      get { return GetProperty(NameProperty, _name); }
      set { SetProperty(NameProperty, ref _name, value); }
    }

    /// <summary>
    /// RL_CAPTION
    /// </summary>
    public static readonly Phenix.Business.PropertyInfo<string> RoleCaptionProperty = RegisterProperty<string>(c => c.RoleCaption);
    [Phenix.Core.Mapping.Field(FriendlyName = "RL_CAPTION", Alias = "RL_CAPTION", TableName = "PH_ROLE", ColumnName = "RL_CAPTION", NeedUpdate = true)]
    private string _roleCaption;
    /// <summary>
    /// RL_CAPTION
    /// </summary>
    [System.ComponentModel.DisplayName("RL_CAPTION")]
    public string RoleCaption
    {
      get { return GetProperty(RoleCaptionProperty, _roleCaption); }
      set { SetProperty(RoleCaptionProperty, ref _roleCaption, value); }
    }

    /// <summary>
    /// New
    /// </summary>
    public static T New(string name, string caption)
    {
      T result = NewPure();
      result._name = name;
      result._roleCaption = caption;
      return result;
    }

    /// <summary>
    /// SetFieldValues
    /// </summary>
    protected void SetFieldValues(string name, string caption)
    {
      InitOldFieldValues();
      _name = name;
      _roleCaption = caption;
      MarkDirty();
    }
  }
}
