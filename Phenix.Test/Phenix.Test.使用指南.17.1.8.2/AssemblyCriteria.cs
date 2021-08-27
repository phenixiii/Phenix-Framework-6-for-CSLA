namespace Phenix.Test.使用指南._17._1._8._2
{
  /// <summary>
  /// 程序集查询
  /// </summary>
  [System.SerializableAttribute(), System.ComponentModel.DisplayNameAttribute("程序集查询")]
  public class AssemblyCriteria : Phenix.Business.CriteriaBase
  {
    [Phenix.Core.Mapping.CriteriaField(FriendlyName = "程序集名称", Logical = Phenix.Core.Mapping.CriteriaLogical.And, Operate = Phenix.Core.Mapping.CriteriaOperate.Like, TableName = "PH_ASSEMBLY", ColumnName = "AS_NAME")]
    private string _name;
    /// <summary>
    /// 程序集名称
    /// </summary>
    [System.ComponentModel.DisplayName("程序集名称")]
    public string Name
    {
      get { return _name; }
      set { _name = value; PropertyHasChanged(); }
    }

    #region 按类信息查询

    [Phenix.Core.Mapping.CriteriaField(FriendlyName = "类信息查询", Logical = Phenix.Core.Mapping.CriteriaLogical.And, Operate = Phenix.Core.Mapping.CriteriaOperate.Exists)]
    [Phenix.Core.Mapping.CriteriaLink("PH_ASSEMBLY", "AS_ID", "PH_ASSEMBLYCLASS", "AC_AS_ID")]
    private readonly AssemblyClassCriteria _assemblyClassCriteria = new AssemblyClassCriteria();
    /// <summary>
    /// 类信息查询
    /// </summary>
    [System.ComponentModel.DisplayName("类信息查询")]
    protected AssemblyClassCriteria AssemblyClassCriteria
    {
      get { return _assemblyClassCriteria; }
    }
    /// <summary>
    /// 类名
    /// </summary>
    [System.ComponentModel.DisplayName("类名")]
    [Phenix.Core.Mapping.Property("AssemblyClassCriteria", typeof(Phenix.Test.使用指南._17._1._8._2.AssemblyClassCriteria), "Name", FriendlyName = "类名")]
    public string AssemblyClassCriteria_Name
    {
      get { return AssemblyClassCriteria.Name; }
      set { AssemblyClassCriteria.Name = value; PropertyHasChanged(); }
    }
    /// <summary>
    /// 属性名
    /// </summary>
    [System.ComponentModel.DisplayName("属性名")]
    [Phenix.Core.Mapping.Property("AssemblyClassCriteria", typeof(Phenix.Test.使用指南._17._1._8._2.AssemblyClassCriteria), "AssemblyClassPropertyCriteria_Name", FriendlyName = "属性名")]
    public string AssemblyClassCriteria_AssemblyClassPropertyCriteria_Name
    {
      get { return AssemblyClassCriteria.AssemblyClassPropertyCriteria_Name; }
      set { AssemblyClassCriteria.AssemblyClassPropertyCriteria_Name = value; PropertyHasChanged(); }
    }
    /// <summary>
    /// 方法名
    /// </summary>
    [System.ComponentModel.DisplayName("方法名")]
    [Phenix.Core.Mapping.Property("AssemblyClassCriteria", typeof(Phenix.Test.使用指南._17._1._8._2.AssemblyClassCriteria), "AssemblyClassMethodCriteria_Name", FriendlyName = "方法名")]
    public string AssemblyClassCriteria_AssemblyClassMethodCriteria_Name
    {
      get { return AssemblyClassCriteria.AssemblyClassMethodCriteria_Name; }
      set { AssemblyClassCriteria.AssemblyClassMethodCriteria_Name = value; PropertyHasChanged(); }
    }
    /// <summary>
    /// 方法名2
    /// </summary>
    [System.ComponentModel.DisplayName("方法名2")]
    [Phenix.Core.Mapping.Property("AssemblyClassCriteria", typeof(Phenix.Test.使用指南._17._1._8._2.AssemblyClassCriteria), "AssemblyClassMethodCriteria_Name2", FriendlyName = "方法名2")]
    public string AssemblyClassCriteria_AssemblyClassMethodCriteria_Name2
    {
      get { return AssemblyClassCriteria.AssemblyClassMethodCriteria_Name2; }
      set { AssemblyClassCriteria.AssemblyClassMethodCriteria_Name2 = value; PropertyHasChanged(); }
    }

    #endregion
  }
}
