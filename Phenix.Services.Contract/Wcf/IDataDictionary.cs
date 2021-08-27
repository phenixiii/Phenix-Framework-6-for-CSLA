using System.ServiceModel;
using Phenix.Core.Dictionary;
using Phenix.Core.Mapping;

namespace Phenix.Services.Contract.Wcf
{
  [ServiceContract]
  public interface IDataDictionary
  {
    [OperationContract]
    [UseNetDataContract]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate")]
    object GetEnterprise();

    [OperationContract]
    [UseNetDataContract]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate")]
    object GetDepartmentInfos();

    [OperationContract]
    [UseNetDataContract]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate")]
    object GetPositionInfos();

    [OperationContract]
    [UseNetDataContract]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate")]
    object GetTableFilterInfos();

    [OperationContract]
    [UseNetDataContract]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate")]
    object GetRoleInfos();

    [OperationContract]
    [UseNetDataContract]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate")]
    object GetSectionInfos();

    [OperationContract(IsOneWay = true)]
    [UseNetDataContract]
    void DepartmentInfoHasChanged();

    [OperationContract(IsOneWay = true)]
    [UseNetDataContract]
    void PositionInfoHasChanged();

    [OperationContract(IsOneWay = true)]
    [UseNetDataContract]
    void AssemblyInfoHasChanged();

    [OperationContract]
    [UseNetDataContract]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate")]
    object GetAssemblyInfos();

    [OperationContract]
    [UseNetDataContract]
    object GetAssemblyInfo(string assemblyName);

    [OperationContract(IsOneWay = true)]
    [UseNetDataContract]
    void TableFilterInfoHasChanged();

    [OperationContract(IsOneWay = true)]
    [UseNetDataContract]
    void RoleInfoHasChanged();

    [OperationContract(IsOneWay = true)]
    [UseNetDataContract]
    void SectionInfoHasChanged();

    [OperationContract(IsOneWay = true)]
    [UseNetDataContract]
    void TableInfoHasChanged();

    [OperationContract]
    [UseNetDataContract]
    object AddAssemblyClassInfo(string assemblyName, string assemblyCaption, string className, string classCaption, ExecuteAction? permanentExecuteAction, string[] groupNames, AssemblyClassType classType);

    [OperationContract]
    [UseNetDataContract]
    object AddAssemblyClassPropertyInfos(string assemblyName, string className, string[] names, string[] captions, string[] tableNames, string[] columnNames, string[] aliases, ExecuteModify[] permanentExecuteModifies);

    [OperationContract]
    [UseNetDataContract]
    object AddAssemblyClassPropertyConfigInfos(string assemblyName, string className, string[] names, string[] captions,
      bool[] configurables, string[] configKeys, string[] configValues, AssemblyClassType classType);

    [OperationContract]
    [UseNetDataContract]
    object AddAssemblyClassMethodInfos(string assemblyName, string className, string[] names, string[] captions, string[] tags, bool[] allowVisibles);
    
    #region BusinessCode

    [OperationContract]
    [UseNetDataContract]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate")]
    object GetBusinessCodeFormats();

    [OperationContract]
    [UseNetDataContract]
    object GetBusinessCodeFormat(string businessCodeName);

    [OperationContract]
    [UseNetDataContract]
    object SetBusinessCodeFormat(BusinessCodeFormat format);

    [OperationContract]
    [UseNetDataContract]
    object RemoveBusinessCodeFormat(string businessCodeName);

    #endregion
  }
}