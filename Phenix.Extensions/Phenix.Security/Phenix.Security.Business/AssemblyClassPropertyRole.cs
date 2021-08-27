using System;
using System.Data.Common;

namespace Phenix.Security.Business
{
  #region SelectableRoleForAssemblyClassProperty

  /// <summary>
  /// 供类属性勾选的角色清单
  /// </summary>
  [Serializable]
  public class SelectableRoleForAssemblyClassPropertyList : Phenix.Business.BusinessListBase<SelectableRoleForAssemblyClassPropertyList, SelectableRoleForAssemblyClassProperty>
  {
  }

  /// <summary>
  /// 供类属性勾选的角色
  /// </summary>
  [Serializable]
  public class SelectableRoleForAssemblyClassProperty : Role<SelectableRoleForAssemblyClassProperty>
  {
    #region 需覆写AssemblyClassPropertyRole的属性: NoMapping = true, IsRedundanceColumn = true

    /// <summary>
    /// 是否允许写
    /// </summary>
    public static readonly Phenix.Business.PropertyInfo<bool?> AllowWriteProperty_ = RegisterProperty<bool?>(c => c.AllowWrite);
    [Phenix.Core.Mapping.Field(FriendlyName = "是否允许写", Alias = "AR_ALLOWWRITE", TableName = "PH_ASSEMBLYCLASSPROPERTY_ROLE", ColumnName = "AR_ALLOWWRITE", NoMapping = true, IsRedundanceColumn = true)]
    private bool? _allowWrite;
    /// <summary>
    /// 是否允许写
    /// </summary>
    [System.ComponentModel.DisplayName("是否允许写")]
    public bool? AllowWrite
    {
      get
      {
        if (!Selected)
          return false;
        return GetProperty(AllowWriteProperty_, _allowWrite);
      }
      set { SetProperty(AllowWriteProperty_, ref _allowWrite, value); }
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
      //非“需覆写AssemblyClassPropertyRole的属性”一概不允许写
      if (property != null)
        if (!property.Equals(AllowWriteProperty_))
          return false;
      return base.CanWriteProperty(property);
    }

    #endregion
  }

  #endregion

  /// <summary>
  /// 类属性角色
  /// </summary>
  [Serializable]
  [Phenix.Core.Mapping.ReadOnly]
  public class AssemblyClassPropertyRoleReadOnly : AssemblyClassPropertyRole<AssemblyClassPropertyRoleReadOnly>
  {
  }

  /// <summary>
  /// 类属性角色清单
  /// </summary>
  [Serializable]
  public class AssemblyClassPropertyRoleReadOnlyList : Phenix.Business.BusinessListBase<AssemblyClassPropertyRoleReadOnlyList, AssemblyClassPropertyRoleReadOnly>
  {
  }

  /// <summary>
  /// 类属性角色
  /// </summary>
  [Serializable]
  public class AssemblyClassPropertyRole : AssemblyClassPropertyRole<AssemblyClassPropertyRole>
  {
    /// <summary>
    /// 保存(增删改)本业务对象之前(运行在持久层的程序域里)
    /// </summary>
    /// <param name="transaction">数据库事务</param>
    protected override void OnSavingSelf(DbTransaction transaction)
    {
      //未被授权，就禁止提交类属性角色
      AssemblyClassProperty assemblyClassProperty = MasterBusiness as AssemblyClassProperty;
      if (assemblyClassProperty != null)
      {
        AssemblyClass assemblyClass = assemblyClassProperty.MasterBusiness as AssemblyClass;
        if (assemblyClass != null && !(assemblyClass.Authorised ?? true))
          MarkOld();
      }
    }
  }

  /// <summary>
  /// 类属性角色清单
  /// </summary>
  [Serializable]
  public class AssemblyClassPropertyRoleList : Phenix.Business.BusinessListBase<AssemblyClassPropertyRoleList, AssemblyClassPropertyRole>
  {
  }

  /// <summary>
  /// 类属性角色
  /// </summary>
  [Phenix.Core.Mapping.ClassAttribute("PH_ASSEMBLYCLASSPROPERTY_ROLE", FriendlyName = "类属性角色"), System.SerializableAttribute(), System.ComponentModel.DisplayNameAttribute("类属性角色")]
  public abstract class AssemblyClassPropertyRole<T> : Phenix.Business.BusinessBase<T> where T : AssemblyClassPropertyRole<T>
  {
    /// <summary>
    /// AR_ID
    /// </summary>
    public static readonly Phenix.Business.PropertyInfo<long?> AR_IDProperty = RegisterProperty<long?>(c => c.AR_ID);
    [Phenix.Core.Mapping.Field(FriendlyName = "AR_ID", TableName = "PH_ASSEMBLYCLASSPROPERTY_ROLE", ColumnName = "AR_ID", IsPrimaryKey = true, NeedUpdate = true)]
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
    /// 类属性
    /// </summary>
    public static readonly Phenix.Business.PropertyInfo<long?> AR_AP_IDProperty = RegisterProperty<long?>(c => c.AR_AP_ID);
    [Phenix.Core.Mapping.Field(FriendlyName = "类属性", TableName = "PH_ASSEMBLYCLASSPROPERTY_ROLE", ColumnName = "AR_AP_ID", NeedUpdate = true)]
    [Phenix.Core.Mapping.FieldLink("PH_ASSEMBLYCLASSPROPERTY", "AP_ID")]
    private long? _AR_AP_ID;
    /// <summary>
    /// 类属性
    /// </summary>
    [System.ComponentModel.DisplayName("类属性")]
    public long? AR_AP_ID
    {
      get { return GetProperty(AR_AP_IDProperty, _AR_AP_ID); }
      set { SetProperty(AR_AP_IDProperty, ref _AR_AP_ID, value); }
    }

    /// <summary>
    /// 角色
    /// </summary>
    public static readonly Phenix.Business.PropertyInfo<long?> AR_RL_IDProperty = RegisterProperty<long?>(c => c.AR_RL_ID);
    [Phenix.Core.Mapping.Field(FriendlyName = "角色", TableName = "PH_ASSEMBLYCLASSPROPERTY_ROLE", ColumnName = "AR_RL_ID", NeedUpdate = true)]
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
    /// 是否允许写
    /// </summary>
    public static readonly Phenix.Business.PropertyInfo<bool?> AllowWriteProperty_ = RegisterProperty<bool?>(c => c.AllowWrite);
    [Phenix.Core.Mapping.Field(FriendlyName = "是否允许写", Alias = "AR_ALLOWWRITE", TableName = "PH_ASSEMBLYCLASSPROPERTY_ROLE", ColumnName = "AR_ALLOWWRITE", NeedUpdate = true)]
    private bool? _allowWrite;
    /// <summary>
    /// 是否允许写
    /// </summary>
    [System.ComponentModel.DisplayName("是否允许写")]
    public bool? AllowWrite
    {
      get { return GetProperty(AllowWriteProperty_, _allowWrite); }
      set { SetProperty(AllowWriteProperty_, ref _allowWrite, value); }
    }
  }
}
