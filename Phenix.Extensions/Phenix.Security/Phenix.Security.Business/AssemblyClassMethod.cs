using System;

namespace Phenix.Security.Business
{
  /// <summary>
  /// 类方法
  /// </summary>
  [Serializable]
  [Phenix.Core.Mapping.ReadOnly]
  public class AssemblyClassMethodReadOnly : AssemblyClassMethod<AssemblyClassMethodReadOnly>
  {
  }

  /// <summary>
  /// 类方法清单
  /// </summary>
  [Serializable]
  public class AssemblyClassMethodReadOnlyList : Phenix.Business.BusinessListBase<AssemblyClassMethodReadOnlyList, AssemblyClassMethodReadOnly>
  {
  }

  /// <summary>
  /// 类方法
  /// </summary>
  [Serializable]
  public class AssemblyClassMethod : AssemblyClassMethod<AssemblyClassMethod>
  {
    #region 属性

    #region 勾选角色全集

    /// <summary>
    /// 分配的角色
    /// </summary>
    public AssemblyClassMethodRoleList AssemblyClassMethodRoles
    {
      get { return GetCompositionDetail<AssemblyClassMethodRoleList, AssemblyClassMethodRole>(AssemblyClassMethodRoleList.Fetch()); }
    }

    /// <summary>
    /// 供勾选的角色全集
    /// </summary>
    public SelectableRoleForAssemblyClassMethodList SelectableRoles
    {
      get 
      {
        AssemblyClass assemblyClass = MasterBusiness as AssemblyClass;
        if (assemblyClass != null && !(assemblyClass.Authorised ?? true))
          return new SelectableRoleForAssemblyClassMethodList();
        return AssemblyClassMethodRoles.CollatingSelectableList<SelectableRoleForAssemblyClassMethodList, SelectableRoleForAssemblyClassMethod>(
          SelectableRoleForAssemblyClassMethodList.Fetch(), !Phenix.Core.Security.UserIdentity.CurrentIdentity.EmptyRolesIsDeny);
      }
    }

    /// <summary>
    /// 是否允许全选角色全集
    /// </summary>
    [System.ComponentModel.Browsable(false)]
    [System.ComponentModel.DataAnnotations.Display(AutoGenerateField = false)]
    public Phenix.Core.Data.BooleanOption AllowSelectAllRolesOption
    {
      get { return Owner.EditModeOption; }
    }

    /// <summary>
    /// 是否允许反选角色全集
    /// </summary>
    [System.ComponentModel.Browsable(false)]
    [System.ComponentModel.DataAnnotations.Display(AutoGenerateField = false)]
    public Phenix.Core.Data.BooleanOption AllowInverseAllRolesOption
    {
      get
      {
        return Owner.EditMode && (Phenix.Core.Security.UserIdentity.CurrentIdentity.EmptyRolesIsDeny || AssemblyClassMethodRoles.Count > 0)
          ? Phenix.Core.Data.BooleanOption.Y
          : Phenix.Core.Data.BooleanOption.N;
      }
    }

    #endregion

    #endregion
  }

  /// <summary>
  /// 类方法清单
  /// </summary>
  [Serializable]
  public class AssemblyClassMethodList : Phenix.Business.BusinessListBase<AssemblyClassMethodList, AssemblyClassMethod>
  {
    /// <summary>
    /// 是否允许添加业务对象
    /// </summary>
    [System.ComponentModel.Browsable(false)]
    [System.ComponentModel.DataAnnotations.Display(AutoGenerateField = false)]
    public override bool AllowAddItem
    {
      get { return false; }
    }
  }

  /// <summary>
  /// 类方法
  /// </summary>
  [Phenix.Core.Mapping.ClassAttribute("PH_ASSEMBLYCLASSMETHOD", FriendlyName = "类方法"), System.SerializableAttribute(), System.ComponentModel.DisplayNameAttribute("类方法")]
  public abstract class AssemblyClassMethod<T> : Phenix.Business.BusinessBase<T> where T : AssemblyClassMethod<T>
 {
   /// <summary>
   /// AM_ID
   /// </summary>
   public static readonly Phenix.Business.PropertyInfo<long?> AM_IDProperty = RegisterProperty<long?>(c => c.AM_ID);
   [Phenix.Core.Mapping.Field(FriendlyName = "AM_ID", TableName = "PH_ASSEMBLYCLASSMETHOD", ColumnName = "AM_ID", IsPrimaryKey = true, NeedUpdate = true)]
   private long? _AM_ID;
   /// <summary>
   /// AM_ID
   /// </summary>
   [System.ComponentModel.DisplayName("AM_ID")]
   public long? AM_ID
   {
     get { return GetProperty(AM_IDProperty, _AM_ID); }
     //internal set
     //{
     //  SetProperty(AM_IDProperty, ref _AM_ID, value);
     //}
   }

   [System.ComponentModel.Browsable(false)]
   [System.ComponentModel.DataAnnotations.Display(AutoGenerateField = false)]
   public override string PrimaryKey
   {
     get { return AM_ID.ToString(); }
   }

   /// <summary>
   /// 所属类
   /// </summary>
   public static readonly Phenix.Business.PropertyInfo<long?> AM_AC_IDProperty = RegisterProperty<long?>(c => c.AM_AC_ID);
   [Phenix.Core.Mapping.Field(FriendlyName = "所属类", TableName = "PH_ASSEMBLYCLASSMETHOD", ColumnName = "AM_AC_ID", NeedUpdate = true)]
   [Phenix.Core.Mapping.FieldLink("PH_ASSEMBLYCLASS", "AC_ID")]
   private long? _AM_AC_ID;
   /// <summary>
   /// 所属类
   /// </summary>
   [System.ComponentModel.DisplayName("所属类")]
   public long? AM_AC_ID
   {
     get { return GetProperty(AM_AC_IDProperty, _AM_AC_ID); }
     //set { SetProperty(AM_AC_IDProperty, ref _AM_AC_ID, value); }
   }

   /// <summary>
   /// 名称
   /// </summary>
   public static readonly Phenix.Business.PropertyInfo<string> NameProperty = RegisterProperty<string>(c => c.Name);
   [Phenix.Core.Mapping.Field(FriendlyName = "名称", Alias = "AM_NAME", TableName = "PH_ASSEMBLYCLASSMETHOD", ColumnName = "AM_NAME", NeedUpdate = true, IsNameColumn = true, InLookUpColumn = true, InLookUpColumnDisplay = true)]
   private string _name;
   /// <summary>
   /// 名称
   /// </summary>
   [System.ComponentModel.DisplayName("名称")]
   public string Name
   {
     get { return GetProperty(NameProperty, _name); }
     //set { SetProperty(NameProperty, ref _name, value); }
   }

   /// <summary>
   /// 标签
   /// </summary>
   public static readonly Phenix.Business.PropertyInfo<string> CaptionProperty = RegisterProperty<string>(c => c.Caption);
   [Phenix.Core.Mapping.Field(FriendlyName = "标签", Alias = "AM_CAPTION", TableName = "PH_ASSEMBLYCLASSMETHOD", ColumnName = "AM_CAPTION", NeedUpdate = true)]
   private string _caption;
   /// <summary>
   /// 标签
   /// </summary>
   [System.ComponentModel.DisplayName("标签")]
   public new string Caption
   {
     get { return GetProperty(CaptionProperty, _caption); }
     //set { SetProperty(CaptionProperty, ref _caption, value); }
   }
 }
}
