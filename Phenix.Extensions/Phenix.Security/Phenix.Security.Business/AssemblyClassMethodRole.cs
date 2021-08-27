using System;
using System.Data.Common;

namespace Phenix.Security.Business
{
  #region SelectableRoleForAssemblyClassMethod

  /// <summary>
  /// 供类方法勾选的角色清单
  /// </summary>
  [Serializable]
  public class SelectableRoleForAssemblyClassMethodList : Phenix.Business.BusinessListBase<SelectableRoleForAssemblyClassMethodList, SelectableRoleForAssemblyClassMethod>
  {
  }

  /// <summary>
  /// 供类方法勾选的角色
  /// </summary>
  [Serializable]
  public class SelectableRoleForAssemblyClassMethod : Role<SelectableRoleForAssemblyClassMethod>
  {
  }

  #endregion

  /// <summary>
  /// 类方法角色
  /// </summary>
  [Serializable]
  [Phenix.Core.Mapping.ReadOnly]
  public class AssemblyClassMethodRoleReadOnly : AssemblyClassMethodRole<AssemblyClassMethodRoleReadOnly>
  {
  }

  /// <summary>
  /// 类方法角色清单
  /// </summary>
  [Serializable]
  public class AssemblyClassMethodRoleReadOnlyList : Phenix.Business.BusinessListBase<AssemblyClassMethodRoleReadOnlyList, AssemblyClassMethodRoleReadOnly>
  {
  }

  /// <summary>
  /// 类方法角色
  /// </summary>
  [Serializable]
  public class AssemblyClassMethodRole : AssemblyClassMethodRole<AssemblyClassMethodRole>
  {
    /// <summary>
    /// 保存(增删改)本业务对象之前(运行在持久层的程序域里)
    /// </summary>
    /// <param name="transaction">数据库事务</param>
    protected override void OnSavingSelf(DbTransaction transaction)
    {
      //未被授权，就禁止提交类方法角色
      AssemblyClassMethod assemblyClassMethod = MasterBusiness as AssemblyClassMethod;
      if (assemblyClassMethod != null)
      {
        AssemblyClass assemblyClass = assemblyClassMethod.MasterBusiness as AssemblyClass;
        if (assemblyClass != null && !(assemblyClass.Authorised ?? true))
          MarkOld();
      }
    }
  }

  /// <summary>
  /// 类方法角色清单
  /// </summary>
  [Serializable]
  public class AssemblyClassMethodRoleList : Phenix.Business.BusinessListBase<AssemblyClassMethodRoleList, AssemblyClassMethodRole>
  {
  }

  /// <summary>
  /// 类方法角色
  /// </summary>
  [Phenix.Core.Mapping.ClassAttribute("PH_ASSEMBLYCLASSMETHOD_ROLE", FriendlyName = "类方法角色"), System.SerializableAttribute(), System.ComponentModel.DisplayNameAttribute("类方法角色")]
  public abstract class AssemblyClassMethodRole<T> : Phenix.Business.BusinessBase<T> where T : AssemblyClassMethodRole<T>
  {
    /// <summary>
    /// AR_ID
    /// </summary>
    public static readonly Phenix.Business.PropertyInfo<long?> AR_IDProperty = RegisterProperty<long?>(c => c.AR_ID);
    [Phenix.Core.Mapping.Field(FriendlyName = "AR_ID", TableName = "PH_ASSEMBLYCLASSMETHOD_ROLE", ColumnName = "AR_ID", IsPrimaryKey = true, NeedUpdate = true)]
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
    /// 类方法
    /// </summary>
    public static readonly Phenix.Business.PropertyInfo<long?> AR_AM_IDProperty = RegisterProperty<long?>(c => c.AR_AM_ID);
    [Phenix.Core.Mapping.Field(FriendlyName = "类方法", TableName = "PH_ASSEMBLYCLASSMETHOD_ROLE", ColumnName = "AR_AM_ID", NeedUpdate = true)]
    [Phenix.Core.Mapping.FieldLink("PH_ASSEMBLYCLASSMETHOD", "AM_ID")]
    private long? _AR_AM_ID;
    /// <summary>
    /// 类方法
    /// </summary>
    [System.ComponentModel.DisplayName("类方法")]
    public long? AR_AM_ID
    {
      get { return GetProperty(AR_AM_IDProperty, _AR_AM_ID); }
      set { SetProperty(AR_AM_IDProperty, ref _AR_AM_ID, value); }
    }

    /// <summary>
    /// 角色
    /// </summary>
    public static readonly Phenix.Business.PropertyInfo<long?> AR_RL_IDProperty = RegisterProperty<long?>(c => c.AR_RL_ID);
    [Phenix.Core.Mapping.Field(FriendlyName = "角色", TableName = "PH_ASSEMBLYCLASSMETHOD_ROLE", ColumnName = "AR_RL_ID", NeedUpdate = true)]
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
  }
}
