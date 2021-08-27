using System;
using System.Data.Common;

namespace Phenix.Security.Business
{
  #region SelectableRoleForAssemblyClass

  /// <summary>
  /// 供类勾选的角色清单
  /// </summary>
  [Serializable]
  public class SelectableRoleForAssemblyClassList : Phenix.Business.BusinessListBase<SelectableRoleForAssemblyClassList, SelectableRoleForAssemblyClass>
  {
  }

  /// <summary>
  /// 供类勾选的角色
  /// </summary>
  [Serializable]
  public class SelectableRoleForAssemblyClass : Role<SelectableRoleForAssemblyClass>
  {
    #region 需覆写AssemblyClassRole的属性: NoMapping = true, IsRedundanceColumn = true

    /// <summary>
    /// 是否允许新增
    /// </summary>
    public static readonly Phenix.Business.PropertyInfo<bool?> AllowCreateProperty = RegisterProperty<bool?>(c => c.AllowCreate);
    [Phenix.Core.Mapping.Field(FriendlyName = "是否允许新增", Alias = "AR_ALLOWCREATE", TableName = "PH_ASSEMBLYCLASS_ROLE", ColumnName = "AR_ALLOWCREATE", NoMapping = true, IsRedundanceColumn = true)]
    private bool? _allowCreate;
    /// <summary>
    /// 是否允许新增
    /// </summary>
    [System.ComponentModel.DisplayName("是否允许新增")]
    public bool? AllowCreate
    {
      get
      {
        if (!Selected)
          return false; 
        return GetProperty(AllowCreateProperty, _allowCreate);
      }
      set { SetProperty(AllowCreateProperty, ref _allowCreate, value); }
    }

    /// <summary>
    /// 是否允许编辑
    /// </summary>
    public static readonly Phenix.Business.PropertyInfo<bool?> AllowEditProperty = RegisterProperty<bool?>(c => c.AllowEdit_);
    [Phenix.Core.Mapping.Field(FriendlyName = "是否允许编辑", Alias = "AR_ALLOWEDIT", TableName = "PH_ASSEMBLYCLASS_ROLE", ColumnName = "AR_ALLOWEDIT", NoMapping = true, IsRedundanceColumn = true)]
    private bool? _allowEdit_;
    /// <summary>
    /// 是否允许编辑
    /// </summary>
    [System.ComponentModel.DisplayName("是否允许编辑")]
    public bool? AllowEdit_
    {
      get
      {
        if (!Selected)
          return false;
        return GetProperty(AllowEditProperty, _allowEdit_);
      }
      set { SetProperty(AllowEditProperty, ref _allowEdit_, value); }
    }

    /// <summary>
    /// 是否允许删除
    /// </summary>
    public static readonly Phenix.Business.PropertyInfo<bool?> AllowDeleteProperty = RegisterProperty<bool?>(c => c.AllowDelete_);
    [Phenix.Core.Mapping.Field(FriendlyName = "是否允许删除", Alias = "AR_ALLOWDELETE", TableName = "PH_ASSEMBLYCLASS_ROLE", ColumnName = "AR_ALLOWDELETE", NoMapping = true, IsRedundanceColumn = true)]
    private bool? _allowDelete_;
    /// <summary>
    /// 是否允许删除
    /// </summary>
    [System.ComponentModel.DisplayName("是否允许删除")]
    public bool? AllowDelete_
    {
      get
      {
        if (!Selected)
          return false;
        return GetProperty(AllowDeleteProperty, _allowDelete_);
      }
      set { SetProperty(AllowDeleteProperty, ref _allowDelete_, value); }
    }

    #endregion

    #region 方法

    /// <summary>
    /// 属性可写
    /// </summary>
    /// <param name="property">属性信息</param>
    /// <returns>属性可写</returns>
    public override bool CanWriteProperty(Csla.Core.IPropertyInfo property)
    {
      //非“需覆写AssemblyClassRole的属性”一概不允许写
      if (property != null)
        if (!property.Equals(AllowCreateProperty) &&
          !property.Equals(AllowEditProperty) &&
          !property.Equals(AllowDeleteProperty))
          return false;
      return base.CanWriteProperty(property);
    }

    #endregion
  }

  #endregion

  /// <summary>
  /// 类角色
  /// </summary>
  [Serializable]
  [Phenix.Core.Mapping.ReadOnly]
  public class AssemblyClassRoleReadOnly : AssemblyClassRole<AssemblyClassRoleReadOnly>
  {
  }

  /// <summary>
  /// 类角色清单
  /// </summary>
  [Serializable]
  public class AssemblyClassRoleReadOnlyList : Phenix.Business.BusinessListBase<AssemblyClassRoleReadOnlyList, AssemblyClassRoleReadOnly>
  {
  }

  /// <summary>
  /// 类角色
  /// </summary>
  [Serializable]
  public class AssemblyClassRole : AssemblyClassRole<AssemblyClassRole>
  {
    /// <summary>
    /// 保存(增删改)本业务对象之前(运行在持久层的程序域里)
    /// </summary>
    /// <param name="transaction">数据库事务</param>
    protected override void OnSavingSelf(DbTransaction transaction)
    {
      //未被授权，就禁止提交类角色
      AssemblyClass assemblyClass = MasterBusiness as AssemblyClass;
      if (assemblyClass != null && !(assemblyClass.Authorised ?? true))
        MarkOld();
    }
  }

  /// <summary>
  /// 类角色清单
  /// </summary>
  [Serializable]
  public class AssemblyClassRoleList : Phenix.Business.BusinessListBase<AssemblyClassRoleList, AssemblyClassRole>
  {
  }

  /// <summary>
  /// 类角色
  /// </summary>
  [Phenix.Core.Mapping.ClassAttribute("PH_ASSEMBLYCLASS_ROLE", FriendlyName = "类角色"), System.SerializableAttribute(), System.ComponentModel.DisplayNameAttribute("类角色")]
  public abstract class AssemblyClassRole<T> : Phenix.Business.BusinessBase<T> where T : AssemblyClassRole<T>
  {
    /// <summary>
    /// AR_ID
    /// </summary>
    public static readonly Phenix.Business.PropertyInfo<long?> AR_IDProperty = RegisterProperty<long?>(c => c.AR_ID);
    [Phenix.Core.Mapping.Field(FriendlyName = "AR_ID", TableName = "PH_ASSEMBLYCLASS_ROLE", ColumnName = "AR_ID", IsPrimaryKey = true, NeedUpdate = true)]
    private long? _AR_ID;
    /// <summary>
    /// AR_ID
    /// </summary>
    [System.ComponentModel.DisplayName("AR_ID")]
    public long? AR_ID
    {
      get { return GetProperty(AR_IDProperty, _AR_ID); }
      internal set
      {
        SetProperty(AR_IDProperty, ref _AR_ID, value);
      }
    }

    [System.ComponentModel.Browsable(false)]
    [System.ComponentModel.DataAnnotations.Display(AutoGenerateField = false)]
    public override string PrimaryKey
    {
      get { return AR_ID.ToString(); }
    }

    /// <summary>
    /// 类
    /// </summary>
    public static readonly Phenix.Business.PropertyInfo<long?> AR_AC_IDProperty = RegisterProperty<long?>(c => c.AR_AC_ID);
    [Phenix.Core.Mapping.Field(FriendlyName = "类", TableName = "PH_ASSEMBLYCLASS_ROLE", ColumnName = "AR_AC_ID", NeedUpdate = true)]
    [Phenix.Core.Mapping.FieldLink("PH_ASSEMBLYCLASS", "AC_ID")]
    private long? _AR_AC_ID;
    /// <summary>
    /// 类
    /// </summary>
    [System.ComponentModel.DisplayName("类")]
    public long? AR_AC_ID
    {
      get { return GetProperty(AR_AC_IDProperty, _AR_AC_ID); }
      set { SetProperty(AR_AC_IDProperty, ref _AR_AC_ID, value); }
    }

    /// <summary>
    /// 角色
    /// </summary>
    public static readonly Phenix.Business.PropertyInfo<long?> AR_RL_IDProperty = RegisterProperty<long?>(c => c.AR_RL_ID);
    [Phenix.Core.Mapping.Field(FriendlyName = "角色", TableName = "PH_ASSEMBLYCLASS_ROLE", ColumnName = "AR_RL_ID", NeedUpdate = true)]
    [Phenix.Core.Mapping.FieldLink("PH_ROLE", "RL_ID")]
    private long? _AR_RL_ID;
    /// <summary>
    /// 角色
    /// </summary>
    [System.ComponentModel.DisplayName("角色")]
    public long? AR_RL_ID
    {
      get { return GetProperty(AR_RL_IDProperty, _AR_RL_ID); }
      set { SetProperty(AR_RL_IDProperty, ref _AR_RL_ID, value); }
    }

    /// <summary>
    /// 是否允许新增
    /// </summary>
    public static readonly Phenix.Business.PropertyInfo<bool?> AllowCreateProperty = RegisterProperty<bool?>(c => c.AllowCreate);
    [Phenix.Core.Mapping.Field(FriendlyName = "是否允许新增", Alias = "AR_ALLOWCREATE", TableName = "PH_ASSEMBLYCLASS_ROLE", ColumnName = "AR_ALLOWCREATE", NeedUpdate = true)]
    private bool? _allowCreate;
    /// <summary>
    /// 是否允许新增
    /// </summary>
    [System.ComponentModel.DisplayName("是否允许新增")]
    public bool? AllowCreate
    {
      get { return GetProperty(AllowCreateProperty, _allowCreate); }
      set { SetProperty(AllowCreateProperty, ref _allowCreate, value); }
    }

    /// <summary>
    /// 是否允许编辑
    /// </summary>
    public static readonly Phenix.Business.PropertyInfo<bool?> AllowEditProperty = RegisterProperty<bool?>(c => c.AllowEdit_);
    [Phenix.Core.Mapping.Field(FriendlyName = "是否允许编辑", Alias = "AR_ALLOWEDIT", TableName = "PH_ASSEMBLYCLASS_ROLE", ColumnName = "AR_ALLOWEDIT", NeedUpdate = true)]
    private bool? _allowEdit_;
    /// <summary>
    /// 是否允许编辑
    /// </summary>
    [System.ComponentModel.DisplayName("是否允许编辑")]
    public bool? AllowEdit_
    {
      get { return GetProperty(AllowEditProperty, _allowEdit_); }
      set { SetProperty(AllowEditProperty, ref _allowEdit_, value); }
    }

    /// <summary>
    /// 是否允许删除
    /// </summary>
    public static readonly Phenix.Business.PropertyInfo<bool?> AllowDeleteProperty = RegisterProperty<bool?>(c => c.AllowDelete_);
    [Phenix.Core.Mapping.Field(FriendlyName = "是否允许删除", Alias = "AR_ALLOWDELETE", TableName = "PH_ASSEMBLYCLASS_ROLE", ColumnName = "AR_ALLOWDELETE", NeedUpdate = true)]
    private bool? _allowDelete_;
    /// <summary>
    /// 是否允许删除
    /// </summary>
    [System.ComponentModel.DisplayName("是否允许删除")]
    public bool? AllowDelete_
    {
      get { return GetProperty(AllowDeleteProperty, _allowDelete_); }
      set { SetProperty(AllowDeleteProperty, ref _allowDelete_, value); }
    }
  }
}
