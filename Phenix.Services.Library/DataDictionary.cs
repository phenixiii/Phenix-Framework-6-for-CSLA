#if Top
using System.Collections.ObjectModel;
#endif

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Reflection;
using Phenix.Core.Cache;
using Phenix.Core.Data;
using Phenix.Core.Dictionary;
using Phenix.Core.Log;
using Phenix.Core.Reflection;
using Phenix.Core.Security;
using Phenix.Core.Threading;

namespace Phenix.Services.Library
{
  internal class DataDictionary : IDataDictionary
  {
    #region 属性

    public string Enterprise
    {
      get { return DefaultDatabase.ExecuteGet(ExecuteGetEnterprise); }
    }

    public IDictionary<long, DepartmentInfo> DepartmentInfos
    {
      get { return DefaultDatabase.ExecuteGet(ExecuteGetDepartmentInfos); }
    }

    public IDictionary<long, PositionInfo> PositionInfos
    {
      get { return DefaultDatabase.ExecuteGet(ExecuteGetPositionInfos); }
    }

    public IDictionary<string, TableFilterInfo> TableFilterInfos
    {
      get { return DefaultDatabase.ExecuteGet(ExecuteGetTableFilterInfos); }
    }

    public IDictionary<string, RoleInfo> RoleInfos
    {
      get { return DefaultDatabase.ExecuteGet(ExecuteGetRoleInfos); }
    }

    public IDictionary<string, SectionInfo> SectionInfos
    {
      get { return DefaultDatabase.ExecuteGet(ExecuteGetSectionInfos); }
    }
    
    #region BusinessCode

    public IDictionary<string, BusinessCodeFormat> BusinessCodeFormats
    {
      get { return DefaultDatabase.ExecuteGet(ExecuteGetBusinessCodeFormats); }
    }

    #endregion

    private static readonly ComparableLocked<DateTime> _changedTime = new ComparableLocked<DateTime>(DateTime.MinValue);

    #endregion

    #region 方法

    internal static bool HaveChanged()
    {
      return DefaultDatabase.ExecuteGet(HaveChanged);
    }

    private static bool HaveChanged(DbConnection connection)
    {
      DateTime changedTime = DateTime.MinValue;
      using (SafeDataReader reader = new SafeDataReader(connection,
@"select
    SI_AssemblyInfoChangedTime,
    SI_TableFilterInfoChangedTime,
    SI_RoleInfoChangedTime,
    SI_SectionInfoChangedTime,
    SI_DepartmentInfoChangedTime,
    SI_PositionInfoChangedTime,
    SI_TableInfoChangedTime,
    SI_WorkflowInfoChangedTime
  from PH_SystemInfo
  where SI_Name = :SI_Name",
        CommandBehavior.SingleRow, false))
      {
        reader.CreateParameter("SI_Name", Phenix.Core.AppConfig.SYSTEM_NAME);
        if (reader.Read())
        {
          for (int i = 0; i < reader.FieldCount; i++)
            if (changedTime < reader.GetDateTime(i))
              changedTime = reader.GetDateTime(i);
        }
      }
      return _changedTime.ChangeToLarger(changedTime);
    }

    private static string ExecuteGetEnterprise(DbConnection connection)
    {
      using (DataReader reader = new DataReader(connection,
@"select SI_Enterprise
  from PH_SystemInfo
  where SI_Name = :SI_Name",
        CommandBehavior.SingleRow, false))
      {
        reader.CreateParameter("SI_Name", Phenix.Core.AppConfig.SYSTEM_NAME);
        if (reader.Read())
          return reader.GetNullableString(0);
      }
      return null;
    }

    private static IDictionary<long, DepartmentInfo> ExecuteGetDepartmentInfos(DbConnection connection)
    {
      Dictionary<long, DepartmentInfo> result = new Dictionary<long, DepartmentInfo>();
      using (DataReader reader = new DataReader(connection,
@"select DP_ID, DP_Name, DP_Code,
  DP_DP_ID, DP_PT_ID, DP_In_Headquarters
  from PH_Department
  order by DP_ID",
        CommandBehavior.SingleResult, false))
      {
        while (reader.Read())
          result.Add(reader.GetInt64ForDecimal(0),
            new DepartmentInfo(reader.GetInt64ForDecimal(0), reader.GetNullableString(1), reader.GetNullableString(2),
              reader.GetNullableInt64ForDecimal(3), reader.GetNullableInt64ForDecimal(4), reader.GetNullableBooleanForDecimal(5)));
      }
#if Top
      return new ReadOnlyDictionary<long, DepartmentInfo>(result);
#else
      return result;
#endif
    }

    private static IDictionary<long, PositionInfo> ExecuteGetPositionInfos(DbConnection connection)
    {
      Dictionary<long, PositionInfo> result = new Dictionary<long, PositionInfo>();
      using (DataReader reader = new DataReader(connection,
@"select PT_ID, PT_Name, PT_Code, PT_PT_ID
  from PH_Position
  order by PT_ID",
        CommandBehavior.SingleResult, false))
      {
        while (reader.Read())
          result.Add(reader.GetInt64ForDecimal(0),
            new PositionInfo(reader.GetInt64ForDecimal(0), reader.GetNullableString(1), reader.GetNullableString(2), reader.GetNullableInt64ForDecimal(3)));
      }
#if Top
      return new ReadOnlyDictionary<long, PositionInfo>(result);
#else
      return result;
#endif
    }

    private static IDictionary<string, TableFilterInfo> ExecuteGetTableFilterInfos(DbConnection connection)
    {
      using (DataSet 
        tableFilterDataSet = DataSetHelper.ExecuteReader(connection,
@"select TF_ID, TF_Name, TF_Compare_ColumnName, TF_NoneSectionIsDeny
  from PH_TableFilter
  order by TF_Name", 
          false),
        tableFilterSectionDataSet = DataSetHelper.ExecuteReader(connection,
@"select A.ST_Name, B.ST_AllowRead_ColumnValue, B.ST_AllowEdit, B.ST_TF_ID ST_TF_ID
  from PH_Section A, PH_Section_TableFilter B
  where A.ST_ID = B.ST_ST_ID
  order by B.ST_TF_ID, A.ST_Name",
          false))
      {
        using (DataView
          tableFilterDataView = new DataView(tableFilterDataSet.Tables[0]),
          tableFilterSectionDataView = new DataView(tableFilterSectionDataSet.Tables[0]))
        {
          Dictionary<string, TableFilterInfo> result = new Dictionary<string, TableFilterInfo>(tableFilterDataView.Count, StringComparer.OrdinalIgnoreCase);
          foreach (DataRowView tableFilterRow in tableFilterDataView)
          {
            tableFilterSectionDataView.RowFilter = String.Format("ST_TF_ID={0}", tableFilterRow[0]);
            List<TableFilterSectionInfo> sectionInfos = new List<TableFilterSectionInfo>(tableFilterSectionDataView.Count);
            if (tableFilterSectionDataView.Count > 0)
              foreach (DataRowView tableFilterSectionRow in tableFilterSectionDataView)
                sectionInfos.Add(new TableFilterSectionInfo(tableFilterSectionRow[0] as string, tableFilterSectionRow[1] as string, (bool)Utilities.ChangeType(tableFilterSectionRow[2], typeof(bool))));
            result.Add(SqlHelper.AssembleFullTableColumnName(tableFilterRow[1] as string, tableFilterRow[2] as string),
              new TableFilterInfo(tableFilterRow[1] as string, tableFilterRow[2] as string, (bool)Utilities.ChangeType(tableFilterRow[3], typeof(bool)), sectionInfos.AsReadOnly()));
          }
#if Top
          return new ReadOnlyDictionary<string, TableFilterInfo>(result);
#else
          return result;
#endif
        }
      }
    }

    private static IDictionary<string, RoleInfo> ExecuteGetRoleInfos(DbConnection connection)
    {
      Dictionary<string, RoleInfo> result = new Dictionary<string, RoleInfo>(StringComparer.Ordinal);
      using (DataReader reader = new DataReader(connection,
@"select RL_Name, RL_Caption
  from PH_Role
  order by RL_Name",
        CommandBehavior.SingleResult, false))
      {
        while (reader.Read())
          result.Add(reader.GetNullableString(0), new RoleInfo(reader.GetNullableString(0), reader.GetNullableString(1)));
      }
#if Top
      return new ReadOnlyDictionary<string, RoleInfo>(result);
#else
      return result;
#endif
    }

    private static IDictionary<string, SectionInfo> ExecuteGetSectionInfos(DbConnection connection)
    {
      Dictionary<string, SectionInfo> result = new Dictionary<string, SectionInfo>(StringComparer.Ordinal);
      using (DataReader reader = new DataReader(connection,
@"select ST_Name, ST_Caption
  from PH_Section
  order by ST_Name",
        CommandBehavior.SingleResult, false))
      {
        while (reader.Read())
          result.Add(reader.GetNullableString(0), new SectionInfo(reader.GetNullableString(0), reader.GetNullableString(1)));
      }
#if Top
      return new ReadOnlyDictionary<string, SectionInfo>(result);
#else
      return result;
#endif
    }

    #region IDataDictionary 成员
    
    public void DepartmentInfoHasChanged()
    {
      DefaultDatabase.Execute(ExecuteDepartmentInfoHasChanged);
    }

    private static void ExecuteDepartmentInfoHasChanged(DbTransaction transaction)
    {
      using (DbCommand command = DbCommandHelper.CreateCommand(transaction,
@"update PH_SystemInfo set
    SI_DepartmentInfoChangedTime = sysdate
  where SI_Name = :SI_Name"))
      {
        DbCommandHelper.CreateParameter(command, "SI_Name", Phenix.Core.AppConfig.SYSTEM_NAME);
        DbCommandHelper.ExecuteNonQuery(command, false);
      }
    }

    public void PositionInfoHasChanged()
    {
      DefaultDatabase.Execute(ExecutePositionInfoHasChanged);
    }

    private static void ExecutePositionInfoHasChanged(DbTransaction transaction)
    {
      using (DbCommand command = DbCommandHelper.CreateCommand(transaction,
@"update PH_SystemInfo set
    SI_PositionInfoChangedTime = sysdate
  where SI_Name = :SI_Name"))
      {
        DbCommandHelper.CreateParameter(command, "SI_Name", Phenix.Core.AppConfig.SYSTEM_NAME);
        DbCommandHelper.ExecuteNonQuery(command, false);
      }
    }

    public void AssemblyInfoHasChanged()
    {
      DefaultDatabase.Execute(ExecuteAssemblyInfoHasChanged);
    }

    private static void ExecuteAssemblyInfoHasChanged(DbTransaction transaction)
    {
      using (DbCommand command = DbCommandHelper.CreateCommand(transaction,
@"update PH_SystemInfo set
    SI_AssemblyInfoChangedTime = sysdate
  where SI_Name = :SI_Name"))
      {
        DbCommandHelper.CreateParameter(command, "SI_Name", Phenix.Core.AppConfig.SYSTEM_NAME);
        DbCommandHelper.ExecuteNonQuery(command, false);
      }
    }

    public IDictionary<string, AssemblyInfo> GetAssemblyInfos()
    {
      return DefaultDatabase.ExecuteGet(ExecuteGetAssemblyInfos);
    }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1809:AvoidExcessiveLocals")]
    private static IDictionary<string, AssemblyInfo> ExecuteGetAssemblyInfos(DbConnection connection)
    {
        using (DataSet
//            assemblyDataSet = DataSetHelper.ExecuteReader(connection, @"
//select AS_ID, AS_Name, AS_Caption
//from PH_Assembly
//order by AS_Name",
//                false),
            assemblyDataSet = DataSetHelper.ExecuteReader(connection, @"
select AS_ID, AS_Name
from PH_Assembly
order by AS_Name",
                false),
//            classDataSet = DataSetHelper.ExecuteReader(connection, @"
//select AC_AS_ID, AC_ID, AC_Name, AC_Caption, AC_CaptionConfigured,
//  AC_PermanentExecuteAction, AC_PermanentExecuteConfigured, AC_Type, AC_Authorised
//from PH_AssemblyClass",
//                false),
            classDataSet = DataSetHelper.ExecuteReader(connection, @"
select AC_AS_ID, AC_ID, AC_Name, AC_Type, AC_Authorised
from PH_AssemblyClass",
                false),
            classRoleDataSet = DataSetHelper.ExecuteReader(connection, @"
select AR_AC_ID, RL_Name, AR_AllowCreate, AR_AllowEdit, AR_AllowDelete
from PH_Role, PH_AssemblyClass_Role
where RL_ID = AR_RL_ID",
                false),
            classDepartmentDataSet = DataSetHelper.ExecuteReader(connection, @"
select AD_AC_ID, AD_DP_ID
from PH_AssemblyClass_Department",
                false),
//            classPropertyDataSet = DataSetHelper.ExecuteReader(connection, @"
//select AP_AC_ID, AP_ID, AP_Name, AP_Caption, AP_CaptionConfigured,
//  AP_PermanentExecuteModify, AP_PermanentExecuteConfigured,
//  AP_Configurable, AP_ConfigValue,
//  AP_IndexNumber, AP_Required, AP_Visible
//from PH_AssemblyClassProperty",
//                false),
            classPropertyDataSet = DataSetHelper.ExecuteReader(connection, @"
select AP_AC_ID, AP_ID, AP_Name,
  AP_Configurable, AP_ConfigValue
from PH_AssemblyClassProperty",
                false),
            classPropertyRoleDataSet = DataSetHelper.ExecuteReader(connection, @"
select AR_AP_ID, RL_Name, AR_AllowWrite
from PH_Role, PH_AssemblyClassProperty_Role
where RL_ID = AR_RL_ID",
                false),
            classPropertyValueDataSet = DataSetHelper.ExecuteReader(connection, @"
select AV_AP_ID, AV_ConfigKey, AV_ConfigValue
from PH_AssemblyClassProperty_Value
where AV_Configurable = 1",
                false),
//            classMethodDataSet = DataSetHelper.ExecuteReader(connection, @"
//select AM_AC_ID, AM_ID, AM_Name, AM_Caption, AM_CaptionConfigured, AM_Tag, AM_AllowVisible
//from PH_AssemblyClassMethod",
//                false),
            classMethodDataSet = DataSetHelper.ExecuteReader(connection, @"
select AM_AC_ID, AM_ID, AM_Name
from PH_AssemblyClassMethod",
                false),
            classMethodRoleDataSet = DataSetHelper.ExecuteReader(connection, @"
select AR_AM_ID, RL_Name
from PH_Role, PH_AssemblyClassMethod_Role
where RL_ID = AR_RL_ID",
                false),
            classMethodDepartmentDataSet = DataSetHelper.ExecuteReader(connection, @"
select AD_AM_ID, AD_DP_ID
from PH_AssemblyClassMethod_Departm",
                false))
        {
            using (DataView
                assemblyView = new DataView(assemblyDataSet.Tables[0]),
                classView = new DataView(classDataSet.Tables[0]),
                classRoleView = new DataView(classRoleDataSet.Tables[0]),
                classDepartmentView = new DataView(classDepartmentDataSet.Tables[0]),
                classPropertyView = new DataView(classPropertyDataSet.Tables[0]),
                classPropertyRoleView = new DataView(classPropertyRoleDataSet.Tables[0]),
                classPropertyValueView = new DataView(classPropertyValueDataSet.Tables[0]),
                classMethodView = new DataView(classMethodDataSet.Tables[0]),
                classMethodRoleView = new DataView(classMethodRoleDataSet.Tables[0]),
                classMethodDepartmentView = new DataView(classMethodDepartmentDataSet.Tables[0]))
            {
                Dictionary<string, AssemblyInfo> result = new Dictionary<string, AssemblyInfo>(assemblyView.Count, StringComparer.OrdinalIgnoreCase);
                ICollection<string> roles = ExecuteGetRoleInfos(connection).Keys;
                if (roles.Count == 0)
#if Top
                    return new ReadOnlyDictionary<string, AssemblyInfo>(result);
#else
                    return result;
#endif
                bool emptyRolesIsDeny = AppConfig.EmptyRolesIsDeny;
                foreach (DataRowView assemblyRow in assemblyView)
                {
                    //AssemblyInfo assemblyInfo = new AssemblyInfo(assemblyRow[1] as string, assemblyRow[2] as string);
                    AssemblyInfo assemblyInfo = new AssemblyInfo(assemblyRow[1] as string);
                    classView.RowFilter = String.Format("AC_AS_ID={0}", assemblyRow[0]);
                    if (classView.Count > 0)
                    {
                        List<string> denyAllRoles = new List<string>(roles.Count);
                        foreach (DataRowView classRow in classView)
                        {
                            string classId = classRow[1].ToString();
                            AssemblyClassType classType = (AssemblyClassType) Utilities.ChangeType(classRow[3], typeof(AssemblyClassType));
                            bool authorised = (bool) Utilities.ChangeType(classRow[4], typeof(bool));

                            List<string> denyCreateRoles = null;
                            List<string> denyGetRoles = null;
                            List<string> denyEditRoles = null;
                            List<string> denyDeleteRoles = null;
                            if (authorised)
                            {
                                denyCreateRoles = new List<string>(roles.Count);
                                denyGetRoles = new List<string>(roles.Count);
                                denyEditRoles = new List<string>(roles.Count);
                                denyDeleteRoles = new List<string>(roles.Count);
                                classRoleView.RowFilter = String.Format("AR_AC_ID={0}", classId);
                                if (classRoleView.Count > 0)
                                {
                                    denyAllRoles.Clear();
                                    denyAllRoles.AddRange(roles);
                                    foreach (DataRowView classRoleRow in classRoleView)
                                    {
                                        string role = classRoleRow[1] as string;
                                        if ((bool) Utilities.ChangeType(classRoleRow[2], typeof(bool)))
                                            denyCreateRoles.Add(role);
                                        if ((bool) Utilities.ChangeType(classRoleRow[3], typeof(bool)))
                                            denyEditRoles.Add(role);
                                        if ((bool) Utilities.ChangeType(classRoleRow[4], typeof(bool)))
                                            denyDeleteRoles.Add(role);
                                        denyAllRoles.Remove(role);
                                    }

                                    if (denyAllRoles.Count > 0)
                                    {
                                        denyCreateRoles.AddRange(denyAllRoles);
                                        denyGetRoles.AddRange(denyAllRoles);
                                        denyEditRoles.AddRange(denyAllRoles);
                                        denyDeleteRoles.AddRange(denyAllRoles);
                                    }

                                    //denyCreateRoles.TrimExcess();
                                    //denyGetRoles.TrimExcess();
                                    //denyEditRoles.TrimExcess();
                                    //denyDeleteRoles.TrimExcess();
                                }
                                else if (emptyRolesIsDeny)
                                {
                                    denyCreateRoles.AddRange(roles);
                                    denyGetRoles.AddRange(roles);
                                    denyEditRoles.AddRange(roles);
                                    denyDeleteRoles.AddRange(roles);
                                }
                            }

                            List<long> departmentIds = null;
                            classDepartmentView.RowFilter = String.Format("AD_AC_ID={0}", classId);
                            if (classDepartmentView.Count > 0)
                            {
                                departmentIds = new List<long>(classDepartmentView.Count);
                                foreach (DataRowView classDepartmentRow in classDepartmentView)
                                    departmentIds.Add((long) Utilities.ChangeType(classDepartmentRow[1], typeof(long)));
                            }

                            classPropertyView.RowFilter = String.Format("AP_AC_ID={0}", classId);
                            Dictionary<string, AssemblyClassPropertyInfo> classPropertyInfos = new Dictionary<string, AssemblyClassPropertyInfo>(classPropertyView.Count, StringComparer.Ordinal);
                            if (classPropertyView.Count > 0)
                                foreach (DataRowView classPropertyRow in classPropertyView)
                                {
                                    string classPropertyId = classPropertyRow[1].ToString();

                                    List<string> denyReadRoles = null;
                                    List<string> denyWriteRoles = null;
                                    if (authorised)
                                    {
                                        denyReadRoles = new List<string>(roles.Count);
                                        denyWriteRoles = new List<string>(roles.Count);
                                        classPropertyRoleView.RowFilter = String.Format("AR_AP_ID={0}", classPropertyId);
                                        if (classPropertyRoleView.Count > 0)
                                        {
                                            denyAllRoles.Clear();
                                            denyAllRoles.AddRange(roles);
                                            foreach (DataRowView classPropertyRoleRow in classPropertyRoleView)
                                            {
                                                string role = classPropertyRoleRow[1] as string;
                                                if ((bool) Utilities.ChangeType(classPropertyRoleRow[2], typeof(bool)))
                                                    denyWriteRoles.Add(role);
                                                denyAllRoles.Remove(role);
                                            }

                                            if (denyAllRoles.Count > 0)
                                            {
                                                denyReadRoles.AddRange(denyAllRoles);
                                                denyWriteRoles.AddRange(denyAllRoles);
                                            }

                                            //denyReadRoles.TrimExcess();
                                            //denyWriteRoles.TrimExcess();
                                        }
                                        else if (emptyRolesIsDeny)
                                        {
                                            denyReadRoles.AddRange(roles);
                                            denyWriteRoles.AddRange(roles);
                                        }
                                    }

                                    Dictionary<string, string> configValues = null;
                                    classPropertyValueView.RowFilter = String.Format("AV_AP_ID={0}", classPropertyId);
                                    if (classPropertyValueView.Count > 0)
                                    {
                                        configValues = new Dictionary<string, string>(classPropertyValueView.Count, StringComparer.Ordinal);
                                        foreach (DataRowView classPropertyValueRow in classPropertyValueView)
                                            configValues.Add(classPropertyValueRow[1] as string, classPropertyValueRow[2] as string ?? String.Empty);
                                    }

//                                    classPropertyInfos.Add(classPropertyRow[2] as string,
//                                        new AssemblyClassPropertyInfo(
//                                            classPropertyRow[3] as string, (bool) Utilities.ChangeType(classPropertyRow[4], typeof(bool)),
//                                            (ExecuteModify) Utilities.ChangeType(classPropertyRow[5], typeof(ExecuteModify)), (bool) Utilities.ChangeType(classPropertyRow[6], typeof(bool)),
//                                            denyReadRoles != null ? denyReadRoles.AsReadOnly() : null, denyWriteRoles != null ? denyWriteRoles.AsReadOnly() : null,
//                                            (bool) Utilities.ChangeType(classPropertyRow[7], typeof(bool)), classPropertyRow[8] as string,
//#if Top
//                                            configValues != null ? new ReadOnlyDictionary<string, string>(configValues) : null,
//#else
//                                            configValues,
//#endif
//                                            (int) Utilities.ChangeType(classPropertyRow[9], typeof(int)), classPropertyRow[10] == DBNull.Value ? new bool?() : (bool) Utilities.ChangeType(classPropertyRow[10], typeof(bool)), /*(bool)Utilities.ChangeType(classPropertyRow[11], typeof(bool)), */(bool) Utilities.ChangeType(classPropertyRow[11], typeof(bool))));
                                    classPropertyInfos.Add(classPropertyRow[2] as string,
                                        new AssemblyClassPropertyInfo(
                                            denyReadRoles != null ? denyReadRoles.AsReadOnly() : null, denyWriteRoles != null ? denyWriteRoles.AsReadOnly() : null,
                                            (bool) Utilities.ChangeType(classPropertyRow[3], typeof(bool)), classPropertyRow[4] as string,
#if Top
                                            configValues != null ? new ReadOnlyDictionary<string, string>(configValues) : null
#else
                                            configValues
#endif
                                           ));
                                }

                            classMethodView.RowFilter = String.Format("AM_AC_ID={0}", classId);
                            Dictionary<string, AssemblyClassMethodInfo> classMethodInfos = new Dictionary<string, AssemblyClassMethodInfo>(classMethodView.Count, StringComparer.Ordinal);
                            if (classMethodView.Count > 0)
                                foreach (DataRowView classMethodRow in classMethodView)
                                {
                                    List<string> denyExecuteRoles = null;
                                    if (authorised)
                                    {
                                        denyExecuteRoles = new List<string>(roles.Count);
                                        classMethodRoleView.RowFilter = String.Format("AR_AM_ID={0}", classMethodRow[1]);
                                        if (classMethodRoleView.Count > 0)
                                        {
                                            denyAllRoles.Clear();
                                            denyAllRoles.AddRange(roles);
                                            foreach (DataRowView classMethodRoleRow in classMethodRoleView)
                                            {
                                                string role = classMethodRoleRow[1] as string;
                                                denyAllRoles.Remove(role);
                                            }

                                            if (denyAllRoles.Count > 0)
                                                denyExecuteRoles.AddRange(denyAllRoles);
                                            //denyExecuteRoles.TrimExcess();
                                        }
                                        else if (emptyRolesIsDeny)
                                        {
                                            denyExecuteRoles.AddRange(roles);
                                        }
                                    }

                                    List<long> methodDepartmentIds = null;
                                    classMethodDepartmentView.RowFilter = String.Format("AD_AM_ID={0}", classMethodRow[1]);
                                    if (classMethodDepartmentView.Count > 0)
                                    {
                                        methodDepartmentIds = new List<long>(classMethodDepartmentView.Count);
                                        foreach (DataRowView classMethodDepartmentRow in classMethodDepartmentView)
                                            methodDepartmentIds.Add((long) Utilities.ChangeType(classMethodDepartmentRow[1], typeof(long)));
                                    }

                                    //classMethodInfos.Add(classMethodRow[2] as string,
                                    //    new AssemblyClassMethodInfo(
                                    //        classMethodRow[3] as string, (bool) Utilities.ChangeType(classMethodRow[4], typeof(bool)), classMethodRow[5] as string,
                                    //        classMethodRow[6] == DBNull.Value ? new bool?() : (decimal) classMethodRow[6] == 1,
                                    //        denyExecuteRoles != null ? denyExecuteRoles.AsReadOnly() : null,
                                    //        methodDepartmentIds != null ? methodDepartmentIds.AsReadOnly() : null));
                                    classMethodInfos.Add(classMethodRow[2] as string,
                                        new AssemblyClassMethodInfo(
                                            denyExecuteRoles != null ? denyExecuteRoles.AsReadOnly() : null,
                                            methodDepartmentIds != null ? methodDepartmentIds.AsReadOnly() : null));
                                }

//                            assemblyInfo.AddAssemblyClassInfo(
//                                classRow[2] as string, classRow[3] as string, (bool) Utilities.ChangeType(classRow[4], typeof(bool)),
//                                (ExecuteAction) Utilities.ChangeType(classRow[5], typeof(ExecuteAction)), (bool) Utilities.ChangeType(classRow[6], typeof(bool)),
//                                classType, authorised,
//                                denyCreateRoles != null ? denyCreateRoles.AsReadOnly() : null, denyGetRoles != null ? denyGetRoles.AsReadOnly() : null,
//                                denyEditRoles != null ? denyEditRoles.AsReadOnly() : null, denyDeleteRoles != null ? denyDeleteRoles.AsReadOnly() : null,
//                                departmentIds != null ? departmentIds.AsReadOnly() : null,
//#if Top
//                                new ReadOnlyDictionary<string, AssemblyClassPropertyInfo>(classPropertyInfos), new ReadOnlyDictionary<string, AssemblyClassMethodInfo>(classMethodInfos));
//#else
//                                classPropertyInfos, classMethodInfos);
//#endif
                            assemblyInfo.AddAssemblyClassInfo(
                                classRow[2] as string,
                                classType, authorised,
                                denyCreateRoles != null ? denyCreateRoles.AsReadOnly() : null, denyGetRoles != null ? denyGetRoles.AsReadOnly() : null,
                                denyEditRoles != null ? denyEditRoles.AsReadOnly() : null, denyDeleteRoles != null ? denyDeleteRoles.AsReadOnly() : null,
                                departmentIds != null ? departmentIds.AsReadOnly() : null,
#if Top
                                new ReadOnlyDictionary<string, AssemblyClassPropertyInfo>(classPropertyInfos), new ReadOnlyDictionary<string, AssemblyClassMethodInfo>(classMethodInfos));
#else
                                classPropertyInfos, classMethodInfos);
#endif
                        }
                    }
                    result.Add(assemblyInfo.Name, assemblyInfo);
                }
#if Top
                return new ReadOnlyDictionary<string, AssemblyInfo>(result);
#else
                return result;
#endif
            }
        }
    }

    public AssemblyInfo GetAssemblyInfo(string assemblyName)
    {
      return DefaultDatabase.ExecuteGet(ExecuteGetAssemblyInfo, assemblyName);
    }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1809:AvoidExcessiveLocals")]
    private static AssemblyInfo ExecuteGetAssemblyInfo(DbConnection connection, string assemblyName)
    {
      AssemblyInfo result = null;
      long assemblyId = 0;
      using (DataReader assemblyReader = new DataReader(connection,
//@"select AS_ID, AS_Name, AS_Caption
//  from PH_Assembly
//  where upper(AS_Name) = :AS_Name",
@"select AS_ID, AS_Name
  from PH_Assembly
  where upper(AS_Name) = :AS_Name",
        CommandBehavior.SingleRow, false))
      {
        assemblyReader.CreateParameter("AS_Name", assemblyName.ToUpper());
        if (assemblyReader.Read())
        {
          //result = new AssemblyInfo(assemblyReader.GetNullableString(1), assemblyReader.GetNullableString(2));
          result = new AssemblyInfo(assemblyReader.GetNullableString(1));
          assemblyId = assemblyReader.GetInt64ForDecimal(0);
        }
      }
      if (result == null)
      {
        //如果assemblyName值为类全名
        using (DataReader assemblyReader = new DataReader(connection,
//@"select AS_ID, AS_Name, AS_Caption
//  from PH_Assembly, PH_AssemblyClass
//  where AS_ID = AC_AS_ID and AC_Name = :AC_Name",
@"select AS_ID, AS_Name
  from PH_Assembly, PH_AssemblyClass
  where AS_ID = AC_AS_ID and AC_Name = :AC_Name",
          CommandBehavior.SingleRow, false))
        {
          assemblyReader.CreateParameter("AC_Name", assemblyName);
          if (assemblyReader.Read())
          {
            //result = new AssemblyInfo(assemblyReader.GetNullableString(1), assemblyReader.GetNullableString(2));
            result = new AssemblyInfo(assemblyReader.GetNullableString(1));
            assemblyId = assemblyReader.GetInt64ForDecimal(0);
          }
          else
            return null;
        }
      }
      ICollection<string> roles = ExecuteGetRoleInfos(connection).Keys;
      if (roles.Count == 0)
        return result;
      bool emptyRolesIsDeny = AppConfig.EmptyRolesIsDeny;
      using (DataSet
//        classDataSet = DataSetHelper.ExecuteReader(connection,
//@"select AC_AS_ID, AC_ID, AC_Name, AC_Caption, AC_CaptionConfigured,
//    AC_PermanentExecuteAction, AC_PermanentExecuteConfigured, AC_Type, AC_Authorised
//  from PH_AssemblyClass
//  where AC_AS_ID = :AC_AS_ID",
//          false, ParamValue.Input("AC_AS_ID", assemblyId)),
        classDataSet = DataSetHelper.ExecuteReader(connection, 
 @"select AC_AS_ID, AC_ID, AC_Name, AC_Type, AC_Authorised
  from PH_AssemblyClass
  where AC_AS_ID = :AC_AS_ID",
          false, ParamValue.Input("AC_AS_ID", assemblyId)),
        classRoleDataSet = DataSetHelper.ExecuteReader(connection,
@"select AR_AC_ID, RL_Name, AR_AllowCreate, AR_AllowEdit, AR_AllowDelete
  from PH_Role, PH_AssemblyClass_Role, PH_AssemblyClass
  where RL_ID = AR_RL_ID and AR_AC_ID = AC_ID and AC_AS_ID = :AC_AS_ID",
          false, ParamValue.Input("AC_AS_ID", assemblyId)),
        classDepartmentDataSet = DataSetHelper.ExecuteReader(connection,
@"select AD_AC_ID, AD_DP_ID
  from PH_AssemblyClass_Department, PH_AssemblyClass
  where AD_AC_ID = AC_ID and AC_AS_ID = :AC_AS_ID",
          false, ParamValue.Input("AC_AS_ID", assemblyId)),
//      classPropertyDataSet = DataSetHelper.ExecuteReader(connection,
//@"select AP_AC_ID, AP_ID, AP_Name, AP_Caption, AP_CaptionConfigured,         
//  AP_PermanentExecuteModify, AP_PermanentExecuteConfigured,
//  AP_Configurable, AP_ConfigValue,
//  AP_IndexNumber, AP_Required, AP_Visible
//from PH_AssemblyClassProperty, PH_AssemblyClass
//where AP_AC_ID = AC_ID and AC_AS_ID = :AC_AS_ID",
//        false, ParamValue.Input("AC_AS_ID", assemblyId)),
        classPropertyDataSet = DataSetHelper.ExecuteReader(connection,
@"select AP_AC_ID, AP_ID, AP_Name,
    AP_Configurable, AP_ConfigValue
  from PH_AssemblyClassProperty, PH_AssemblyClass
  where AP_AC_ID = AC_ID and AC_AS_ID = :AC_AS_ID",
          false, ParamValue.Input("AC_AS_ID", assemblyId)),
        classPropertyRoleDataSet = DataSetHelper.ExecuteReader(connection,
@"select AR_AP_ID, RL_Name, AR_AllowWrite
  from PH_Role, PH_AssemblyClassProperty_Role, PH_AssemblyClassProperty, PH_AssemblyClass
  where RL_ID = AR_RL_ID and AR_AP_ID = AP_ID and AP_AC_ID = AC_ID and AC_AS_ID = :AC_AS_ID",
          false, ParamValue.Input("AC_AS_ID", assemblyId)),
        classPropertyValueDataSet = DataSetHelper.ExecuteReader(connection,
@"select AV_AP_ID, AV_ConfigKey, AV_ConfigValue
  from PH_AssemblyClassProperty_Value, PH_AssemblyClassProperty, PH_AssemblyClass
  where AV_Configurable = 1 and AV_AP_ID = AP_ID and AP_AC_ID = AC_ID and AC_AS_ID = :AC_AS_ID",
          false, ParamValue.Input("AC_AS_ID", assemblyId)),
//        classMethodDataSet = DataSetHelper.ExecuteReader(connection,
//@"select AM_AC_ID, AM_ID, AM_Name, AM_Caption, AM_CaptionConfigured, AM_Tag, AM_AllowVisible
//  from PH_AssemblyClassMethod, PH_AssemblyClass
//  where AM_AC_ID = AC_ID and AC_AS_ID = :AC_AS_ID",
//          false, ParamValue.Input("AC_AS_ID", assemblyId)),
        classMethodDataSet = DataSetHelper.ExecuteReader(connection,
@"select AM_AC_ID, AM_ID, AM_Name
  from PH_AssemblyClassMethod, PH_AssemblyClass
  where AM_AC_ID = AC_ID and AC_AS_ID = :AC_AS_ID",
          false, ParamValue.Input("AC_AS_ID", assemblyId)),
        classMethodRoleDataSet = DataSetHelper.ExecuteReader(connection,
@"select AR_AM_ID, RL_Name
  from PH_Role, PH_AssemblyClassMethod_Role, PH_AssemblyClassMethod, PH_AssemblyClass
  where RL_ID = AR_RL_ID and AR_AM_ID = AM_ID and AM_AC_ID = AC_ID and AC_AS_ID = :AC_AS_ID",
          false, ParamValue.Input("AC_AS_ID", assemblyId)),
        classMethodDepartmentDataSet = DataSetHelper.ExecuteReader(connection,
@"select AD_AM_ID, AD_DP_ID
  from PH_AssemblyClassMethod_Departm, PH_AssemblyClassMethod, PH_AssemblyClass
  where AD_AM_ID = AM_ID and AM_AC_ID = AC_ID and AC_AS_ID = :AC_AS_ID",
          false, ParamValue.Input("AC_AS_ID", assemblyId)))
      {
        using (DataView
          classView = new DataView(classDataSet.Tables[0]),
          classRoleView = new DataView(classRoleDataSet.Tables[0]),
          classDepartmentView = new DataView(classDepartmentDataSet.Tables[0]),
          classPropertyView = new DataView(classPropertyDataSet.Tables[0]),
          classPropertyRoleView = new DataView(classPropertyRoleDataSet.Tables[0]),
          classPropertyValueView = new DataView(classPropertyValueDataSet.Tables[0]),
          classMethodView = new DataView(classMethodDataSet.Tables[0]),
          classMethodRoleView = new DataView(classMethodRoleDataSet.Tables[0]),
          classMethodDepartmentView = new DataView(classMethodDepartmentDataSet.Tables[0]))
        {
          if (classView.Count > 0)
          {
            List<string> denyAllRoles = new List<string>(roles.Count);
            foreach (DataRowView classRow in classView)
            {
              string classId = classRow[1].ToString();
              AssemblyClassType classType = (AssemblyClassType)Utilities.ChangeType(classRow[3], typeof(AssemblyClassType));
              bool authorised = (bool)Utilities.ChangeType(classRow[4], typeof(bool));

              List<string> denyCreateRoles = null;
              List<string> denyGetRoles = null;
              List<string> denyEditRoles = null;
              List<string> denyDeleteRoles = null;
              if (authorised)
              {
                denyCreateRoles = new List<string>(roles.Count);
                denyGetRoles = new List<string>(roles.Count);
                denyEditRoles = new List<string>(roles.Count);
                denyDeleteRoles = new List<string>(roles.Count);
                classRoleView.RowFilter = String.Format("AR_AC_ID={0}", classId);
                if (classRoleView.Count > 0)
                {
                  denyAllRoles.Clear();
                  denyAllRoles.AddRange(roles);
                  foreach (DataRowView classRoleRow in classRoleView)
                  {
                    string role = classRoleRow[1] as string;
                    if ((bool)Utilities.ChangeType(classRoleRow[2], typeof(bool)))
                      denyCreateRoles.Add(role);
                    if ((bool)Utilities.ChangeType(classRoleRow[3], typeof(bool)))
                      denyEditRoles.Add(role);
                    if ((bool)Utilities.ChangeType(classRoleRow[4], typeof(bool)))
                      denyDeleteRoles.Add(role);
                    denyAllRoles.Remove(role);
                  }
                  if (denyAllRoles.Count > 0)
                  {
                    denyCreateRoles.AddRange(denyAllRoles);
                    denyGetRoles.AddRange(denyAllRoles);
                    denyEditRoles.AddRange(denyAllRoles);
                    denyDeleteRoles.AddRange(denyAllRoles);
                  }
                  //denyCreateRoles.TrimExcess();
                  //denyGetRoles.TrimExcess();
                  //denyEditRoles.TrimExcess();
                  //denyDeleteRoles.TrimExcess();
                }
                else if (emptyRolesIsDeny)
                {
                  denyCreateRoles.AddRange(roles);
                  denyGetRoles.AddRange(roles);
                  denyEditRoles.AddRange(roles);
                  denyDeleteRoles.AddRange(roles);
                }
              }

              List<long> departmentIds = null;
              classDepartmentView.RowFilter = String.Format("AD_AC_ID={0}", classId);
              if (classDepartmentView.Count > 0)
              {
                departmentIds = new List<long>(classDepartmentView.Count);
                foreach (DataRowView classDepartmentRow in classDepartmentView)
                  departmentIds.Add((long)Utilities.ChangeType(classDepartmentRow[1], typeof(long)));
              }

              classPropertyView.RowFilter = String.Format("AP_AC_ID={0}", classId);
              Dictionary<string, AssemblyClassPropertyInfo> classPropertyInfos = new Dictionary<string, AssemblyClassPropertyInfo>(classPropertyView.Count, StringComparer.Ordinal);
              if (classPropertyView.Count > 0)
                foreach (DataRowView classPropertyRow in classPropertyView)
                {
                  string classPropertyId = classPropertyRow[1].ToString();

                  List<string> denyReadRoles = null;
                  List<string> denyWriteRoles = null;
                  if (authorised)
                  {
                    denyReadRoles = new List<string>(roles.Count);
                    denyWriteRoles = new List<string>(roles.Count);
                    classPropertyRoleView.RowFilter = String.Format("AR_AP_ID={0}", classPropertyId);
                    if (classPropertyRoleView.Count > 0)
                    {
                      denyAllRoles.Clear();
                      denyAllRoles.AddRange(roles);
                      foreach (DataRowView classPropertyRoleRow in classPropertyRoleView)
                      {
                        string role = classPropertyRoleRow[1] as string;
                        if ((bool)Utilities.ChangeType(classPropertyRoleRow[2], typeof(bool)))
                          denyWriteRoles.Add(role);
                        denyAllRoles.Remove(role);
                      }
                      if (denyAllRoles.Count > 0)
                      {
                        denyReadRoles.AddRange(denyAllRoles);
                        denyWriteRoles.AddRange(denyAllRoles);
                      }
                      //denyReadRoles.TrimExcess();
                      //denyWriteRoles.TrimExcess();
                    }
                    else if (emptyRolesIsDeny)
                    {
                      denyReadRoles.AddRange(roles);
                      denyWriteRoles.AddRange(roles);
                    }
                  }

                  Dictionary<string, string> configValues = null;
                  classPropertyValueView.RowFilter = String.Format("AV_AP_ID={0}", classPropertyId);
                  if (classPropertyValueView.Count > 0)
                  {
                    configValues = new Dictionary<string, string>(classPropertyValueView.Count, StringComparer.Ordinal);
                    foreach (DataRowView classPropertyValueRow in classPropertyValueView)
                      configValues.Add(classPropertyValueRow[1] as string, classPropertyValueRow[2] as string ?? String.Empty);
                  }

//                  classPropertyInfos.Add(classPropertyRow[2] as string,
//                      new AssemblyClassPropertyInfo(
//                          classPropertyRow[3] as string, (bool)Utilities.ChangeType(classPropertyRow[4], typeof(bool)),
//                          (ExecuteModify)Utilities.ChangeType(classPropertyRow[5], typeof(ExecuteModify)), (bool)Utilities.ChangeType(classPropertyRow[6], typeof(bool)),
//                          denyReadRoles != null ? denyReadRoles.AsReadOnly() : null, denyWriteRoles != null ? denyWriteRoles.AsReadOnly() : null,
//                          (bool)Utilities.ChangeType(classPropertyRow[7], typeof(bool)), classPropertyRow[8] as string,
//#if Top
//                          configValues != null ? new ReadOnlyDictionary<string, string>(configValues) : null,
//#else
//                      configValues,
//#endif
//                          (int)Utilities.ChangeType(classPropertyRow[9], typeof(int)), classPropertyRow[10] == DBNull.Value ? new bool?() : (bool)Utilities.ChangeType(classPropertyRow[10], typeof(bool)), /*(bool)Utilities.ChangeType(classPropertyRow[11], typeof(bool)), */(bool)Utilities.ChangeType(classPropertyRow[11], typeof(bool))));
                  classPropertyInfos.Add(classPropertyRow[2] as string,
                    new AssemblyClassPropertyInfo(
                      denyReadRoles != null ? denyReadRoles.AsReadOnly() : null, denyWriteRoles != null ? denyWriteRoles.AsReadOnly() : null,
                      (bool)Utilities.ChangeType(classPropertyRow[3], typeof(bool)), classPropertyRow[4] as string,
#if Top
                      configValues != null ? new ReadOnlyDictionary<string, string>(configValues) : null
#else
                      configValues
#endif
                      ));
                }

              classMethodView.RowFilter = String.Format("AM_AC_ID={0}", classId);
              Dictionary<string, AssemblyClassMethodInfo> classMethodInfos = new Dictionary<string, AssemblyClassMethodInfo>(classMethodView.Count, StringComparer.Ordinal);
              if (classMethodView.Count > 0)
                foreach (DataRowView classMethodRow in classMethodView)
                {
                  List<string> denyExecuteRoles = null;
                  if (authorised)
                  {
                    denyExecuteRoles = new List<string>(roles.Count);
                    classMethodRoleView.RowFilter = String.Format("AR_AM_ID={0}", classMethodRow[1]);
                    if (classMethodRoleView.Count > 0)
                    {
                      denyAllRoles.Clear();
                      denyAllRoles.AddRange(roles);
                      foreach (DataRowView classMethodRoleRow in classMethodRoleView)
                      {
                        string role = classMethodRoleRow[1] as string;
                        denyAllRoles.Remove(role);
                      }
                      if (denyAllRoles.Count > 0)
                        denyExecuteRoles.AddRange(denyAllRoles);
                      //denyExecuteRoles.TrimExcess();
                    }
                    else if (emptyRolesIsDeny)
                    {
                      denyExecuteRoles.AddRange(roles);
                    }
                  }

                  List<long> methodDepartmentIds = null;
                  classMethodDepartmentView.RowFilter = String.Format("AD_AM_ID={0}", classMethodRow[1]);
                  if (classMethodDepartmentView.Count > 0)
                  {
                    methodDepartmentIds = new List<long>(classMethodDepartmentView.Count);
                    foreach (DataRowView classMethodDepartmentRow in classMethodDepartmentView)
                      methodDepartmentIds.Add((long)Utilities.ChangeType(classMethodDepartmentRow[1], typeof(long)));
                  }

                  //classMethodInfos.Add(classMethodRow[2] as string,
                  //  new AssemblyClassMethodInfo(
                  //    classMethodRow[3] as string, (bool)Utilities.ChangeType(classMethodRow[4], typeof(bool)), classMethodRow[5] as string,
                  //    classMethodRow[6] == DBNull.Value ? new bool?() : (decimal)classMethodRow[6] == 1,
                  //    denyExecuteRoles != null ? denyExecuteRoles.AsReadOnly() : null, 
                  //    methodDepartmentIds != null ? methodDepartmentIds.AsReadOnly() : null));
                  classMethodInfos.Add(classMethodRow[2] as string,
                      new AssemblyClassMethodInfo(
                          denyExecuteRoles != null ? denyExecuteRoles.AsReadOnly() : null, 
                          methodDepartmentIds != null ? methodDepartmentIds.AsReadOnly() : null));
                }

//              result.AddAssemblyClassInfo(
//                classRow[2] as string, classRow[3] as string, (bool)Utilities.ChangeType(classRow[4], typeof(bool)),
//                (ExecuteAction)Utilities.ChangeType(classRow[5], typeof(ExecuteAction)), (bool)Utilities.ChangeType(classRow[6], typeof(bool)),
//                classType, authorised,
//                denyCreateRoles != null ? denyCreateRoles.AsReadOnly() : null, denyGetRoles != null ? denyGetRoles.AsReadOnly() : null,
//                denyEditRoles != null ? denyEditRoles.AsReadOnly() : null, denyDeleteRoles != null ? denyDeleteRoles.AsReadOnly() : null,
//                departmentIds != null ? departmentIds.AsReadOnly() : null,
//#if Top
//                new ReadOnlyDictionary<string, AssemblyClassPropertyInfo>(classPropertyInfos), new ReadOnlyDictionary<string, AssemblyClassMethodInfo>(classMethodInfos));
//#else
//                classPropertyInfos, classMethodInfos);
//#endif
                result.AddAssemblyClassInfo(
                    classRow[2] as string, 
                    classType, authorised,
                    denyCreateRoles != null ? denyCreateRoles.AsReadOnly() : null, denyGetRoles != null ? denyGetRoles.AsReadOnly() : null,
                    denyEditRoles != null ? denyEditRoles.AsReadOnly() : null, denyDeleteRoles != null ? denyDeleteRoles.AsReadOnly() : null,
                    departmentIds != null ? departmentIds.AsReadOnly() : null,
#if Top
                    new ReadOnlyDictionary<string, AssemblyClassPropertyInfo>(classPropertyInfos), new ReadOnlyDictionary<string, AssemblyClassMethodInfo>(classMethodInfos));
#else
                classPropertyInfos, classMethodInfos);
#endif
            }
          }
        }
      }
      return result;
    }

    public void TableFilterInfoHasChanged()
    {
      DefaultDatabase.Execute(ExecuteTableFilterInfoHasChanged);
    }

    private static void ExecuteTableFilterInfoHasChanged(DbTransaction transaction)
    {
      using (DbCommand command = DbCommandHelper.CreateCommand(transaction,
@"update PH_SystemInfo set
    SI_TableFilterInfoChangedTime = sysdate
  where SI_Name = :SI_Name"))
      {
        DbCommandHelper.CreateParameter(command, "SI_Name", Phenix.Core.AppConfig.SYSTEM_NAME);
        DbCommandHelper.ExecuteNonQuery(command, false);
      }
    }

    public void RoleInfoHasChanged()
    {
      DefaultDatabase.Execute(ExecuteRoleInfoHasChanged);
    }

    private static void ExecuteRoleInfoHasChanged(DbTransaction transaction)
    {
      using (DbCommand command = DbCommandHelper.CreateCommand(transaction,
@"update PH_SystemInfo set
    SI_RoleInfoChangedTime = sysdate
  where SI_Name = :SI_Name"))
      {
        DbCommandHelper.CreateParameter(command, "SI_Name", Phenix.Core.AppConfig.SYSTEM_NAME);
        DbCommandHelper.ExecuteNonQuery(command, false);
      }
    }

    public void SectionInfoHasChanged()
    {
      DefaultDatabase.Execute(ExecuteSectionInfoHasChanged);
    }

    private static void ExecuteSectionInfoHasChanged(DbTransaction transaction)
    {
      using (DbCommand command = DbCommandHelper.CreateCommand(transaction,
@"update PH_SystemInfo set
    SI_SectionInfoChangedTime = sysdate
  where SI_Name = :SI_Name"))
      {
        DbCommandHelper.CreateParameter(command, "SI_Name", Phenix.Core.AppConfig.SYSTEM_NAME);
        DbCommandHelper.ExecuteNonQuery(command, false);
      }
    }

    public void TableInfoHasChanged()
    {
      DefaultDatabase.Execute(ExecuteTableInfoHasChanged);
    }

    private static void ExecuteTableInfoHasChanged(DbTransaction transaction)
    {
      using (DbCommand command = DbCommandHelper.CreateCommand(transaction,
@"update PH_SystemInfo set
    SI_TableInfoChangedTime = sysdate
  where SI_Name = :SI_Name"))
      {
        DbCommandHelper.CreateParameter(command, "SI_Name", Phenix.Core.AppConfig.SYSTEM_NAME);
        DbCommandHelper.ExecuteNonQuery(command, false);
      }
    }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
    public void AddAssemblyClassInfo(string assemblyName, string assemblyCaption, 
      string className, string classCaption,/* ExecuteAction? permanentExecuteAction,*/ string[] groupNames, AssemblyClassType classType)
    {
      try
      {
        DefaultDatabase.ExecuteGet(ExecuteAddAssemblyClassInfo, assemblyName, assemblyCaption, className, classCaption,/* permanentExecuteAction,*/ groupNames, classType);
        ObjectCache.RecordHasChanged("PH_Assembly");
        ObjectCache.RecordHasChanged("PH_AssemblyClass");
        ObjectCache.RecordHasChanged("PH_AssemblyClass_Group");
      }
      catch (Exception ex)
      {
        EventLog.SaveLocal(MethodBase.GetCurrentMethod(), className, ex);
      }
    }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
    private static long ExecuteAddAssemblyClassInfo(DbTransaction transaction, string assemblyName, string assemblyCaption, 
      string className, string classCaption,/* ExecuteAction? permanentExecuteAction,*/ string[] groupNames, AssemblyClassType classType)
    {
      long? assemblyId = null;
      using (DataReader reader = new DataReader(transaction, 
@"select AS_ID
  from PH_Assembly
  where upper(AS_Name) = :AS_Name",
        CommandBehavior.SingleRow, false))
      {
        reader.CreateParameter("AS_Name", assemblyName.ToUpper());
        if (reader.Read())
          assemblyId = reader.GetInt64ForDecimal(0);
      }
      if (!assemblyId.HasValue)
      {
        assemblyId = Sequence.Value;
        using (DbCommand assemblyInsertCommand = DbCommandHelper.CreateCommand(transaction,
@"insert into PH_Assembly
  (AS_ID, AS_Name, AS_Caption)
  values
  (:AS_ID, :AS_Name, :AS_Caption)"))
        {
          DbCommandHelper.CreateParameter(assemblyInsertCommand, "AS_ID", assemblyId);
          DbCommandHelper.CreateParameter(assemblyInsertCommand, "AS_Name", assemblyName);
          DbCommandHelper.CreateParameter(assemblyInsertCommand, "AS_Caption", assemblyCaption);
          DbCommandHelper.ExecuteNonQuery(assemblyInsertCommand, false);
        }
      }
      long? classId = null;
      AssemblyClassType? assemblyClassType = null;
      using (DataReader reader = new DataReader(transaction, 
@"select AC_ID, AC_Type
  from PH_AssemblyClass
  where AC_AS_ID = :AC_AS_ID and AC_Name = :AC_Name", 
        CommandBehavior.SingleRow, false))
      {
        reader.CreateParameter("AC_AS_ID", assemblyId);
        reader.CreateParameter("AC_Name", className);
        if (reader.Read())
        {
          classId = reader.GetInt64ForDecimal(0);
          assemblyClassType = (AssemblyClassType)reader.GetDecimal(1);
        }
      }
      if (classId.HasValue)
      {
        if (assemblyClassType == AssemblyClassType.Ordinary && classType != AssemblyClassType.Ordinary)
        {
          using (DbCommand assemblyClassUpdateTypeCommand = DbCommandHelper.CreateCommand(transaction,
@"update PH_AssemblyClass set
    AC_Type = :AC_Type,
    AC_Authorised = :AC_Authorised
  where AC_ID = :AC_ID"))
          {
            DbCommandHelper.CreateParameter(assemblyClassUpdateTypeCommand, "AC_Type", classType);
            DbCommandHelper.CreateParameter(assemblyClassUpdateTypeCommand, "AC_Authorised", classType == AssemblyClassType.Form || classType == AssemblyClassType.ApiController ? 1 : 0);
            DbCommandHelper.CreateParameter(assemblyClassUpdateTypeCommand, "AC_ID", classId);
            DbCommandHelper.ExecuteNonQuery(assemblyClassUpdateTypeCommand, false);
          }
        }
      }
      else
      {
        classId = Sequence.Value;
        using (DbCommand assemblyClassInsertCommand = DbCommandHelper.CreateCommand(transaction,
@"insert into PH_AssemblyClass
  (AC_ID, AC_AS_ID, AC_Name, AC_Caption, AC_Type, AC_Authorised)
  values
  (:AC_ID, :AC_AS_ID, :AC_Name, :AC_Caption, :AC_Type, :AC_Authorised)"))
        {
          DbCommandHelper.CreateParameter(assemblyClassInsertCommand, "AC_ID", classId);
          DbCommandHelper.CreateParameter(assemblyClassInsertCommand, "AC_AS_ID", assemblyId);
          DbCommandHelper.CreateParameter(assemblyClassInsertCommand, "AC_Name", className);
          DbCommandHelper.CreateParameter(assemblyClassInsertCommand, "AC_Caption", classCaption);
          DbCommandHelper.CreateParameter(assemblyClassInsertCommand, "AC_Type", classType);
          DbCommandHelper.CreateParameter(assemblyClassInsertCommand, "AC_Authorised", classType == AssemblyClassType.Form || classType == AssemblyClassType.ApiController ? 1 : 0);
          DbCommandHelper.ExecuteNonQuery(assemblyClassInsertCommand, false);
        }
      }
//      if (!String.IsNullOrEmpty(classCaption))
//      {
//        using (DbCommand assemblyClassUpdateCaptionCommand = DbCommandHelper.CreateCommand(transaction,
//@"update PH_AssemblyClass set
//    AC_Caption = :AC_Caption
//  where AC_ID = :AC_ID and AC_CaptionConfigured = 0"))
//        {
//          DbCommandHelper.CreateParameter(assemblyClassUpdateCaptionCommand, "AC_Caption", classCaption);
//          DbCommandHelper.CreateParameter(assemblyClassUpdateCaptionCommand, "AC_ID", classId);
//          DbCommandHelper.ExecuteNonQuery(assemblyClassUpdateCaptionCommand, false);
//        }
//      }
//      if (permanentExecuteAction.HasValue)
//      {
//        using (DbCommand assemblyClassUpdatePermanentExecuteCommand = DbCommandHelper.CreateCommand(transaction,
//@"update PH_AssemblyClass set
//    AC_PermanentExecuteAction = :AC_PermanentExecuteAction
//  where AC_ID = :AC_ID and AC_PermanentExecuteConfigured = 0"))
//        {
//          DbCommandHelper.CreateParameter(assemblyClassUpdatePermanentExecuteCommand, "AC_PermanentExecuteAction", permanentExecuteAction.Value);
//          DbCommandHelper.CreateParameter(assemblyClassUpdatePermanentExecuteCommand, "AC_ID", classId);
//          DbCommandHelper.ExecuteNonQuery(assemblyClassUpdatePermanentExecuteCommand, false);
//        }
//      }
      if (groupNames != null)
      {
        using (DbCommand assemblyClassGroupDeleteCommand = DbCommandHelper.CreateCommand(transaction,
@"delete PH_AssemblyClass_Group
  where AG_AC_ID = :AG_AC_ID"))
        {
          DbCommandHelper.CreateParameter(assemblyClassGroupDeleteCommand, "AG_AC_ID", classId);
          DbCommandHelper.ExecuteNonQuery(assemblyClassGroupDeleteCommand, false);
        }
        using (DbCommand assemblyClassGroupInsertCommand = DbCommandHelper.CreateCommand(transaction,
@"insert into PH_AssemblyClass_Group
  (AG_ID, AG_AC_ID, AG_Name)
  values
  (:AG_ID, :AG_AC_ID, :AG_Name)"))
        {
          foreach (string s in groupNames)
          {
            DbCommandHelper.CreateParameter(assemblyClassGroupInsertCommand, "AG_ID", Sequence.Value);
            DbCommandHelper.CreateParameter(assemblyClassGroupInsertCommand, "AG_AC_ID", classId);
            DbCommandHelper.CreateParameter(assemblyClassGroupInsertCommand, "AG_Name", s);
            DbCommandHelper.ExecuteNonQuery(assemblyClassGroupInsertCommand, false);
          }
        }
      }
      return classId.Value;
    }

    private static long? GetClassId(DbTransaction transaction, string assemblyName, string className)
    {
      using (DataReader reader = new DataReader(transaction, 
@"select AC_ID
  from PH_Assembly, PH_AssemblyClass
  where AS_ID = AC_AS_ID and upper(AS_Name) = :AS_Name and AC_Name = :AC_Name", 
        CommandBehavior.SingleRow, false))
      {
        reader.CreateParameter("AS_Name", assemblyName.ToUpper());
        reader.CreateParameter("AC_Name", className);
        if (reader.Read())
          return reader.GetInt64ForDecimal(0);
      }
      return null;
    }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
    public void AddAssemblyClassPropertyInfos(string assemblyName, string className, string[] names, string[] captions/*,
      string[] tableNames, string[] columnNames, string[] aliases, ExecuteModify[] permanentExecuteModifies*/)
    {
      try
      {
        DefaultDatabase.Execute(ExecuteAddAssemblyClassPropertyInfos, assemblyName, className, names, captions/*,
          tableNames, columnNames, aliases, permanentExecuteModifies*/);
        ObjectCache.RecordHasChanged("PH_AssemblyClassProperty");
        ObjectCache.RecordHasChanged("PH_AssemblyClassProperty_Role");
      }
      catch (Exception ex)
      {
        EventLog.SaveLocal(MethodBase.GetCurrentMethod(), className, ex);
      }
    }

    private static void ExecuteAddAssemblyClassPropertyInfos(DbTransaction transaction, string assemblyName, string className, string[] names, string[] captions/*,
      string[] tableNames, string[] columnNames, string[] aliases, ExecuteModify[] permanentExecuteModifies*/)
    {
      long classId = GetClassId(transaction, assemblyName, className) ??
        ExecuteAddAssemblyClassInfo(transaction, assemblyName, null, className, null,/* null,*/ null, AssemblyClassType.Business);

      using (DbCommand
        assemblyClassPropertyInsertCommand = DbCommandHelper.CreateCommand(transaction,
@"insert into PH_AssemblyClassProperty
  (AP_ID, AP_AC_ID, AP_Name, AP_Caption)
  values
  (:AP_ID, :AP_AC_ID, :AP_Name, :AP_Caption)")/*,
        assemblyClassPropertyUpdateCaptionCommand = DbCommandHelper.CreateCommand(transaction,
@"update PH_AssemblyClassProperty set
    AP_Caption = :AP_Caption
  where AP_ID = :AP_ID and AP_CaptionConfigured = 0"),
        assemblyClassPropertyUpdateMappingCommand = DbCommandHelper.CreateCommand(transaction,
@"update PH_AssemblyClassProperty set
    AP_TableName = :AP_TableName,
    AP_ColumnName = :AP_ColumnName,
    AP_Alias = :AP_Alias
  where AP_ID = :AP_ID"),
        assemblyClassPropertyUpdatePermanentExecuteCommand = DbCommandHelper.CreateCommand(transaction,
@"update PH_AssemblyClassProperty set
    AP_PermanentExecuteModify = :AP_PermanentExecuteModify
  where AP_ID = :AP_ID and AP_PermanentExecuteConfigured = 0")*/)
      {
        for (int i = 0; i < names.Length; i++)
        {
          long? classPropertyId = null;
          using (DataReader reader = new DataReader(transaction, 
@"select AP_ID
  from PH_AssemblyClassProperty
  where AP_AC_ID = :AP_AC_ID and AP_Name = :AP_Name",
            CommandBehavior.SingleRow, false))
          {
            reader.CreateParameter("AP_AC_ID", classId);
            reader.CreateParameter("AP_Name", names[i]);
            if (reader.Read())
              classPropertyId = reader.GetInt64ForDecimal(0);
          }
          if (!classPropertyId.HasValue)
          {
            classPropertyId = Sequence.Value;
            DbCommandHelper.CreateParameter(assemblyClassPropertyInsertCommand, "AP_ID", classPropertyId);
            DbCommandHelper.CreateParameter(assemblyClassPropertyInsertCommand, "AP_AC_ID", classId);
            DbCommandHelper.CreateParameter(assemblyClassPropertyInsertCommand, "AP_Name", names[i]);
            DbCommandHelper.CreateParameter(assemblyClassPropertyInsertCommand, "AP_Caption", captions[i]);
            DbCommandHelper.ExecuteNonQuery(assemblyClassPropertyInsertCommand, false);
          }
          //if (!String.IsNullOrEmpty(captions[i]))
          //{
          //  DbCommandHelper.CreateParameter(assemblyClassPropertyUpdateCaptionCommand, "AP_Caption", captions[i]);
          //  DbCommandHelper.CreateParameter(assemblyClassPropertyUpdateCaptionCommand, "AP_ID", classPropertyId);
          //  DbCommandHelper.ExecuteNonQuery(assemblyClassPropertyUpdateCaptionCommand, false);
          //}
          //if (tableNames != null)
          //{
          //  DbCommandHelper.CreateParameter(assemblyClassPropertyUpdateMappingCommand, "AP_TableName", tableNames[i]);
          //  DbCommandHelper.CreateParameter(assemblyClassPropertyUpdateMappingCommand, "AP_ColumnName", columnNames[i]);
          //  DbCommandHelper.CreateParameter(assemblyClassPropertyUpdateMappingCommand, "AP_Alias", aliases[i]);
          //  DbCommandHelper.CreateParameter(assemblyClassPropertyUpdateMappingCommand, "AP_ID", classPropertyId);
          //  DbCommandHelper.ExecuteNonQuery(assemblyClassPropertyUpdateMappingCommand, false);
          //}
          //if (permanentExecuteModifies != null)
          //{
          //  DbCommandHelper.CreateParameter(assemblyClassPropertyUpdatePermanentExecuteCommand, "AP_PermanentExecuteModify", permanentExecuteModifies[i]);
          //  DbCommandHelper.CreateParameter(assemblyClassPropertyUpdatePermanentExecuteCommand, "AP_ID", classPropertyId);
          //  DbCommandHelper.ExecuteNonQuery(assemblyClassPropertyUpdatePermanentExecuteCommand, false);
          //}
        }
      }
    }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
    public void AddAssemblyClassPropertyConfigInfos(string assemblyName, string className, string[] names, string[] captions,
      bool[] configurables, string[] configKeys, string[] configValues, AssemblyClassType classType)
    {
      try
      {
        DefaultDatabase.Execute(ExecuteAddAssemblyClassPropertyConfigInfos, assemblyName, className, names, captions,
          configurables, configKeys, configValues, classType);
        ObjectCache.RecordHasChanged("PH_AssemblyClassProperty");
      }
      catch (Exception ex)
      {
        EventLog.SaveLocal(MethodBase.GetCurrentMethod(), className, ex);
      }
    }

    private static void ExecuteAddAssemblyClassPropertyConfigInfos(DbTransaction transaction, string assemblyName, string className, string[] names, string[] captions,
      bool[] configurables, string[] configKeys, string[] configValues, AssemblyClassType classType)
    {
      long classId = GetClassId(transaction, assemblyName, className) ??
        ExecuteAddAssemblyClassInfo(transaction, assemblyName, null, className, null,/* null,*/ null, classType);

      using (DbCommand
        assemblyClassPropertyInsertCommand = DbCommandHelper.CreateCommand(transaction,
@"insert into PH_AssemblyClassProperty
  (AP_ID, AP_AC_ID, AP_Name, AP_Caption)
  values
  (:AP_ID, :AP_AC_ID, :AP_Name, :AP_Caption)"),
//        assemblyClassPropertyUpdateCaptionCommand = DbCommandHelper.CreateCommand(transaction,
//@"update PH_AssemblyClassProperty set
//    AP_Caption = :AP_Caption
//  where AP_ID = :AP_ID and AP_CaptionConfigured = 0"),
        assemblyClassPropertyUpdateConfigCommand = DbCommandHelper.CreateCommand(transaction,
@"update PH_AssemblyClassProperty set
    AP_Configurable = :AP_Configurable,
    AP_ConfigValue = :AP_ConfigValue
  where AP_ID = :AP_ID"),
        assemblyClassPropertyValueInsertCommand = DbCommandHelper.CreateCommand(transaction,
@"insert into PH_AssemblyClassProperty_Value
  (AV_ID, AV_AP_ID, AV_ConfigKey)
  values
  (:AV_ID, :AV_AP_ID, :AV_ConfigKey)"),
        assemblyClassPropertyValueUpdateCommand = DbCommandHelper.CreateCommand(transaction,
@"update PH_AssemblyClassProperty_Value set
    AV_Configurable = :AV_Configurable,
    AV_ConfigValue = :AV_ConfigValue
  where AV_ID = :AV_ID"))
      {
        for (int i = 0; i < names.Length; i++)
        {
          long? classPropertyId = null;
          using (DataReader reader = new DataReader(transaction, 
@"select AP_ID
  from PH_AssemblyClassProperty
  where AP_AC_ID = :AP_AC_ID and AP_Name = :AP_Name",
            CommandBehavior.SingleRow, false))
          {
            reader.CreateParameter("AP_AC_ID", classId);
            reader.CreateParameter("AP_Name", names[i]);
            if (reader.Read())
              classPropertyId = reader.GetInt64ForDecimal(0);
          }
          if (!classPropertyId.HasValue)
          {
            classPropertyId = Sequence.Value;
            DbCommandHelper.CreateParameter(assemblyClassPropertyInsertCommand, "AP_ID", classPropertyId);
            DbCommandHelper.CreateParameter(assemblyClassPropertyInsertCommand, "AP_AC_ID", classId);
            DbCommandHelper.CreateParameter(assemblyClassPropertyInsertCommand, "AP_Name", names[i]);
            DbCommandHelper.CreateParameter(assemblyClassPropertyInsertCommand, "AP_Caption", captions[i]);
            DbCommandHelper.ExecuteNonQuery(assemblyClassPropertyInsertCommand, false);
          }
          //if (!String.IsNullOrEmpty(captions[i]))
          //{
          //  DbCommandHelper.CreateParameter(assemblyClassPropertyUpdateCaptionCommand, "AP_Caption", captions[i]);
          //  DbCommandHelper.CreateParameter(assemblyClassPropertyUpdateCaptionCommand, "AP_ID", classPropertyId);
          //  DbCommandHelper.ExecuteNonQuery(assemblyClassPropertyUpdateCaptionCommand, false);
          //}
          if (configurables != null)
          {
            if (configKeys == null || String.IsNullOrEmpty(configKeys[i]))
            {
              DbCommandHelper.CreateParameter(assemblyClassPropertyUpdateConfigCommand, "AP_Configurable", configurables[i]);
              DbCommandHelper.CreateParameter(assemblyClassPropertyUpdateConfigCommand, "AP_ConfigValue", configValues[i]);
              DbCommandHelper.CreateParameter(assemblyClassPropertyUpdateConfigCommand, "AP_ID", classPropertyId);
              DbCommandHelper.ExecuteNonQuery(assemblyClassPropertyUpdateConfigCommand, false);
            }
            else
            {
              long? classPropertyValueId = null;
              using (DataReader reader = new DataReader(transaction, 
@"select AV_ID
  from PH_AssemblyClassProperty_Value
  where AV_AP_ID = :AV_AP_ID and AV_ConfigKey = :AV_ConfigKey",
                CommandBehavior.SingleRow, false))
              {
                reader.CreateParameter("AV_AP_ID", classPropertyId);
                reader.CreateParameter("AV_ConfigKey", configKeys[i]);
                if (reader.Read())
                  classPropertyValueId = reader.GetInt64ForDecimal(0);
              }
              if (!classPropertyValueId.HasValue)
              {
                classPropertyValueId = Sequence.Value;
                DbCommandHelper.CreateParameter(assemblyClassPropertyValueInsertCommand, "AV_ID", classPropertyValueId);
                DbCommandHelper.CreateParameter(assemblyClassPropertyValueInsertCommand, "AV_AP_ID", classPropertyId);
                DbCommandHelper.CreateParameter(assemblyClassPropertyValueInsertCommand, "AV_ConfigKey", configKeys[i]);
                DbCommandHelper.ExecuteNonQuery(assemblyClassPropertyValueInsertCommand, false);
              }
              DbCommandHelper.CreateParameter(assemblyClassPropertyValueUpdateCommand, "AV_Configurable", configurables[i]);
              DbCommandHelper.CreateParameter(assemblyClassPropertyValueUpdateCommand, "AV_ConfigValue", configValues[i]);
              DbCommandHelper.CreateParameter(assemblyClassPropertyValueUpdateCommand, "AV_ID", classPropertyValueId);
              DbCommandHelper.ExecuteNonQuery(assemblyClassPropertyValueUpdateCommand, false);
            }
          }
        }
      }
    }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
    public void AddAssemblyClassMethodInfos(string assemblyName, string className, string[] names, string[] captions/*, string[] tags,
      bool[] allowVisibles*/)
    {
      try
      {
        DefaultDatabase.Execute(ExecuteAddAssemblyClassMethodInfos, assemblyName, className, names, captions/*, tags, allowVisibles*/);
        ObjectCache.RecordHasChanged("PH_AssemblyClassMethod");
        ObjectCache.RecordHasChanged("PH_AssemblyClassMethod_Role");
        ObjectCache.RecordHasChanged("PH_AssemblyClassMethod_Departm");
      }
      catch (Exception ex)
      {
        EventLog.SaveLocal(MethodBase.GetCurrentMethod(), className, ex);
      }
    }

    private static void ExecuteAddAssemblyClassMethodInfos(DbTransaction transaction, string assemblyName, string className,
      string[] names, string[] captions/*, string[] tags, bool[] allowVisibles*/)
    {
      long classId = GetClassId(transaction, assemblyName, className) ??
        ExecuteAddAssemblyClassInfo(transaction, assemblyName, null, className, null,/* null,*/ null, AssemblyClassType.Ordinary);

      using (DbCommand
        assemblyClassMethodInsertCommand = DbCommandHelper.CreateCommand(transaction,
@"insert into PH_AssemblyClassMethod
  (AM_ID, AM_AC_ID, AM_Name, AM_Caption)
  values
  (:AM_ID, :AM_AC_ID, :AM_Name, :AM_Caption)")/*,
        assemblyClassMethodUpdateCaptionCommand = DbCommandHelper.CreateCommand(transaction,
@"update PH_AssemblyClassMethod set
    AM_Caption = :AM_Caption
  where AM_ID = :AM_ID and AM_CaptionConfigured = 0"),
        assemblyClassMethodUpdateTagCommand = DbCommandHelper.CreateCommand(transaction,
@"update PH_AssemblyClassMethod set
    AM_Tag = :AM_Tag
  where AM_ID = :AM_ID"),
        assemblyClassMethodUpdateAllowVisibleCommand = DbCommandHelper.CreateCommand(transaction,
@"update PH_AssemblyClassMethod set
    AM_AllowVisible = :AM_AllowVisible
  where AM_ID = :AM_ID")*/)
      {
        for (int i = 0; i < names.Length; i++)
        {
          long? classMethodId = null;
          using (DataReader reader = new DataReader(transaction, 
@"select AM_ID
  from PH_AssemblyClassMethod
  where AM_AC_ID = :AM_AC_ID and AM_Name = :AM_Name",
            CommandBehavior.SingleRow, false))
          {
            reader.CreateParameter("AM_AC_ID", classId);
            reader.CreateParameter("AM_Name", names[i]);
            if (reader.Read())
              classMethodId = reader.GetInt64ForDecimal(0);
          }
          if (!classMethodId.HasValue)
          {
            classMethodId = Sequence.Value;
            DbCommandHelper.CreateParameter(assemblyClassMethodInsertCommand, "AM_ID", classMethodId);
            DbCommandHelper.CreateParameter(assemblyClassMethodInsertCommand, "AM_AC_ID", classId);
            DbCommandHelper.CreateParameter(assemblyClassMethodInsertCommand, "AM_Name", names[i]);
            DbCommandHelper.CreateParameter(assemblyClassMethodInsertCommand, "AM_Caption", captions[i]);
            DbCommandHelper.ExecuteNonQuery(assemblyClassMethodInsertCommand, false);
          }
          //if (!String.IsNullOrEmpty(captions[i]))
          //{
          //  DbCommandHelper.CreateParameter(assemblyClassMethodUpdateCaptionCommand, "AM_Caption", captions[i]);
          //  DbCommandHelper.CreateParameter(assemblyClassMethodUpdateCaptionCommand, "AM_ID", classMethodId);
          //  DbCommandHelper.ExecuteNonQuery(assemblyClassMethodUpdateCaptionCommand, false);
          //}
          //if (!String.IsNullOrEmpty(tags[i]))
          //{
          //  DbCommandHelper.CreateParameter(assemblyClassMethodUpdateTagCommand, "AM_Tag", tags[i]);
          //  DbCommandHelper.CreateParameter(assemblyClassMethodUpdateTagCommand, "AM_ID", classMethodId);
          //  DbCommandHelper.ExecuteNonQuery(assemblyClassMethodUpdateTagCommand, false);
          //}
          //if (allowVisibles != null)
          //{
          //  DbCommandHelper.CreateParameter(assemblyClassMethodUpdateAllowVisibleCommand, "AM_AllowVisible", allowVisibles[i]);
          //  DbCommandHelper.CreateParameter(assemblyClassMethodUpdateAllowVisibleCommand, "AM_ID", classMethodId);
          //  DbCommandHelper.ExecuteNonQuery(assemblyClassMethodUpdateAllowVisibleCommand, false);
          //}
        }
      }
    }

    #region BusinessCode

    private static IDictionary<string, BusinessCodeFormat> ExecuteGetBusinessCodeFormats(DbConnection connection)
    {
      Dictionary<string, BusinessCodeFormat> result = new Dictionary<string, BusinessCodeFormat>(StringComparer.Ordinal);
      using (DataReader reader = new DataReader(connection,
@"select BC_Name, BC_Caption, BC_FormatString, BC_FillOnSaving
  from PH_BusinessCode",
        CommandBehavior.SingleResult, false))
      {
        while (reader.Read())
          result.Add(reader.GetNullableString(0),
            new BusinessCodeFormat(reader.GetNullableString(0), reader.GetNullableString(1), reader.GetNullableString(2), reader.GetBooleanForDecimal(3)));
      }
#if Top
      return new ReadOnlyDictionary<string, BusinessCodeFormat>(result);
#else
      return result;
#endif
    }

    public BusinessCodeFormat GetBusinessCodeFormat(string businessCodeName)
    {
      return DefaultDatabase.ExecuteGet(ExecuteGetBusinessCodeFormat, businessCodeName);
    }

    private static BusinessCodeFormat ExecuteGetBusinessCodeFormat(DbConnection connection, string businessCodeName)
    {
      using (DataReader reader = new DataReader(connection,
@"select BC_Caption, BC_FormatString, BC_FillOnSaving
  from PH_BusinessCode
  where BC_Name = :BC_Name",
        CommandBehavior.SingleRow, false))
      {
        reader.CreateParameter("BC_Name", businessCodeName);
        if (reader.Read())
          return new BusinessCodeFormat(businessCodeName, reader.GetNullableString(0), reader.GetNullableString(1), reader.GetBooleanForDecimal(2));
      }
      string name = BusinessCodeFormat.ExtractName(businessCodeName);
      if (String.CompareOrdinal(name, businessCodeName) != 0)
        return ExecuteGetBusinessCodeFormat(connection, name);
      return null;
    }

    public void SetBusinessCodeFormat(BusinessCodeFormat format)
    {
      DefaultDatabase.Execute(ExecuteSetBusinessCodeFormat, format);
    }

    private static void ExecuteSetBusinessCodeFormat(DbTransaction transaction, BusinessCodeFormat format)
    {
      if (String.IsNullOrEmpty(format.FormatString))
      {
        using (DataReader reader = new DataReader(transaction,
@"select BC_Name
  from PH_BusinessCode
  where BC_Name = :BC_Name",
          CommandBehavior.SingleRow, false))
        {
          reader.CreateParameter("BC_Name", format.BusinessCodeName);
          if (reader.Read())
            return;
        }
        using (DbCommand command = DbCommandHelper.CreateCommand(transaction,
@"insert into PH_BusinessCode
  (BC_Name, BC_Caption, BC_FormatString, BC_FillOnSaving)
  values
  (:BC_Name, :BC_Caption, :BC_FormatString, :BC_FillOnSaving)"))
        {
          DbCommandHelper.CreateParameter(command, "BC_Name", format.BusinessCodeName);
          DbCommandHelper.CreateParameter(command, "BC_Caption", format.Caption);
          DbCommandHelper.CreateParameter(command, "BC_FormatString", format.DefaultFormatString);
          DbCommandHelper.CreateParameter(command, "BC_FillOnSaving", format.FillOnSaving);
          DbCommandHelper.ExecuteNonQuery(command, false);
        }
      }
      else
      {
        bool succeed;
        using (DbCommand command = DbCommandHelper.CreateCommand(transaction,
@"update PH_BusinessCode set
    BC_Caption = :BC_Caption,
    BC_FormatString = :BC_FormatString,
    BC_FillOnSaving = :BC_FillOnSaving
  where BC_Name = :BC_Name"))
        {
          DbCommandHelper.CreateParameter(command, "BC_Caption", format.Caption);
          DbCommandHelper.CreateParameter(command, "BC_FormatString", format.FormatString);
          DbCommandHelper.CreateParameter(command, "BC_FillOnSaving", format.FillOnSaving);
          DbCommandHelper.CreateParameter(command, "BC_Name", format.BusinessCodeName);
          succeed = DbCommandHelper.ExecuteNonQuery(command, false) != 0;
        }
        if (!succeed)
        {
          using (DbCommand command = DbCommandHelper.CreateCommand(transaction,
@"insert into PH_BusinessCode
  (BC_Name, BC_Caption, BC_FormatString, BC_FillOnSaving)
  values
  (:BC_Name, :BC_Caption, :BC_FormatString, :BC_FillOnSaving)"))
          {
            DbCommandHelper.CreateParameter(command, "BC_Name", format.BusinessCodeName);
            DbCommandHelper.CreateParameter(command, "BC_Caption", format.Caption);
            DbCommandHelper.CreateParameter(command, "BC_FormatString", format.FormatString);
            DbCommandHelper.CreateParameter(command, "BC_FillOnSaving", format.FillOnSaving);
            DbCommandHelper.ExecuteNonQuery(command, false);
          }
        }
      }
    }

    public void RemoveBusinessCodeFormat(string businessCodeName)
    {
      DefaultDatabase.Execute(ExecuteRemoveBusinessCodeFormat, businessCodeName);
    }

    private static void ExecuteRemoveBusinessCodeFormat(DbTransaction transaction, string businessCodeName)
    {
      using (DbCommand command = DbCommandHelper.CreateCommand(transaction,
@"delete PH_BusinessCode
  where BC_Name = :BC_Name"))
      {
        DbCommandHelper.CreateParameter(command, "BC_Name", businessCodeName);
        DbCommandHelper.ExecuteNonQuery(command, false);
      }
    }

    #endregion

    #endregion

    #endregion
  }
}