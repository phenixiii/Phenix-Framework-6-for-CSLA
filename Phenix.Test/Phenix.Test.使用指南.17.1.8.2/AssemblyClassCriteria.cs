namespace Phenix.Test.使用指南._17._1._8._2
{
  /// <summary>
  /// 类信息查询
  /// </summary>
  [System.SerializableAttribute(), System.ComponentModel.DisplayNameAttribute("类信息查询")]
  public class AssemblyClassCriteria : Phenix.Business.CriteriaBase
  {
    [Phenix.Core.Mapping.CriteriaField(FriendlyName = "类名", Logical = Phenix.Core.Mapping.CriteriaLogical.And, Operate = Phenix.Core.Mapping.CriteriaOperate.Like, TableName = "PH_ASSEMBLYCLASS", ColumnName = "AC_NAME")]
    private string _name;
    /// <summary>
    /// 类名
    /// </summary>
    [System.ComponentModel.DisplayName("类名")]
    public string Name
    {
      get { return _name; }
      set { _name = value; PropertyHasChanged(); }
    }

    #region 按类属性信息查询

    [Phenix.Core.Mapping.CriteriaField(FriendlyName = "类属性信息查询", Logical = Phenix.Core.Mapping.CriteriaLogical.And, Operate = Phenix.Core.Mapping.CriteriaOperate.Exists)]
    [Phenix.Core.Mapping.CriteriaLink("PH_ASSEMBLYCLASS", "AC_ID", "PH_ASSEMBLYCLASSPROPERTY", "AP_AC_ID")]
    private readonly AssemblyClassPropertyCriteria _assemblyClassPropertyCriteria = new AssemblyClassPropertyCriteria();
    /// <summary>
    /// 类属性信息查询
    /// </summary>
    [System.ComponentModel.DisplayName("类属性信息查询")]
    protected AssemblyClassPropertyCriteria AssemblyClassPropertyCriteria
    {
      get { return _assemblyClassPropertyCriteria; }
    }
    /// <summary>
    /// 属性名
    /// </summary>
    [System.ComponentModel.DisplayName("属性名")]
    [Phenix.Core.Mapping.Property("AssemblyClassPropertyCriteria", typeof(Phenix.Test.使用指南._17._1._8._2.AssemblyClassPropertyCriteria), "Name", FriendlyName = "属性名")]
    public string AssemblyClassPropertyCriteria_Name
    {
      get { return AssemblyClassPropertyCriteria.Name; }
      set { AssemblyClassPropertyCriteria.Name = value; PropertyHasChanged(); }
    }

    #endregion

    #region 按类方法信息查询

    [Phenix.Core.Mapping.CriteriaField(FriendlyName = "类方法信息查询", Logical = Phenix.Core.Mapping.CriteriaLogical.And, Operate = Phenix.Core.Mapping.CriteriaOperate.Exists)]
    [Phenix.Core.Mapping.CriteriaLink("PH_ASSEMBLYCLASS", "AC_ID", "PH_ASSEMBLYCLASSMETHOD", "AM_AC_ID")]
    private readonly AssemblyClassMethodCriteria _assemblyClassMethodCriteria = new AssemblyClassMethodCriteria();
    /// <summary>
    /// 类方法信息查询
    /// </summary>
    [System.ComponentModel.DisplayName("类方法信息查询")]
    protected AssemblyClassMethodCriteria AssemblyClassMethodCriteria
    {
      get { return _assemblyClassMethodCriteria; }
    }
    /// <summary>
    /// 方法名
    /// </summary>
    [System.ComponentModel.DisplayName("方法名")]
    [Phenix.Core.Mapping.Property("AssemblyClassMethodCriteria", typeof(Phenix.Test.使用指南._17._1._8._2.AssemblyClassMethodCriteria), "Name", FriendlyName = "方法名")]
    public string AssemblyClassMethodCriteria_Name
    {
      get { return AssemblyClassMethodCriteria.Name; }
      set { AssemblyClassMethodCriteria.Name = value; PropertyHasChanged(); }
    }

    #endregion
    
    #region 按类方法2信息查询

    [Phenix.Core.Mapping.CriteriaField(FriendlyName = "类方法信息查询", Logical = Phenix.Core.Mapping.CriteriaLogical.And, Operate = Phenix.Core.Mapping.CriteriaOperate.Exists)]
    [Phenix.Core.Mapping.CriteriaLink("PH_ASSEMBLYCLASS", "AC_ID", "PH_ASSEMBLYCLASSMETHOD", "AM_AC_ID")]
    private readonly AssemblyClassMethodCriteria _assemblyClassMethodCriteria2 = new AssemblyClassMethodCriteria();
    /// <summary>
    /// 类方法信息查询
    /// </summary>
    [System.ComponentModel.DisplayName("类方法信息查询")]
    protected AssemblyClassMethodCriteria AssemblyClassMethodCriteria2
    {
      get { return _assemblyClassMethodCriteria2; }
    }
    /// <summary>
    /// 方法名
    /// </summary>
    [System.ComponentModel.DisplayName("方法名")]
    [Phenix.Core.Mapping.Property("AssemblyClassMethodCriteria2", typeof(Phenix.Test.使用指南._17._1._8._2.AssemblyClassMethodCriteria), "Name", FriendlyName = "方法名")]
    public string AssemblyClassMethodCriteria_Name2
    {
      get { return AssemblyClassMethodCriteria2.Name; }
      set { AssemblyClassMethodCriteria2.Name = value; PropertyHasChanged(); }
    }

    #endregion
  }
}
