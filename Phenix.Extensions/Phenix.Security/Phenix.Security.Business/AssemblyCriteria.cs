
namespace Phenix.Security.Business
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
    /// 类型
    /// </summary>
    [System.ComponentModel.DisplayName("类型")]
    [Phenix.Core.Mapping.Property("AssemblyClassCriteria", typeof(Phenix.Security.Business.AssemblyClassCriteria), "Type", FriendlyName = "类型")]
    public string AssemblyClassCriteria_Type
    {
      get { return AssemblyClassCriteria.Type; }
      set { AssemblyClassCriteria.Type = value; PropertyHasChanged(); }
    }
    /// <summary>
    /// 类名
    /// </summary>
    [System.ComponentModel.DisplayName("类名")]
    [Phenix.Core.Mapping.Property("AssemblyClassCriteria", typeof(Phenix.Security.Business.AssemblyClassCriteria), "Name", FriendlyName = "类名")]
    public string AssemblyClassCriteria_Name
    {
      get { return AssemblyClassCriteria.Name; }
      set { AssemblyClassCriteria.Name = value; PropertyHasChanged(); }
    }
    
    #endregion
  }
}
