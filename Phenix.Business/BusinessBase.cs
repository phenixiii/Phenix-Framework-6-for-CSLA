using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Data.Common;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Phenix.Business.Core;
using Phenix.Core;
using Phenix.Core.Data;
using Phenix.Core.Log;
using Phenix.Core.Mapping;
using Phenix.Core.Operate;
using Phenix.Core.Reflection;

namespace Phenix.Business
{
    /// <summary>
    /// 业务对象基类
    /// </summary>
    [Serializable]
  [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Maintainability", "CA1506:AvoidExcessiveClassCoupling")]
  public abstract class BusinessBase<T> : Phenix.Business.Core.BusinessBase<T>, IRefinedlyObject, IBusinessObject
    where T : BusinessBase<T>
  {
    /// <summary>
    /// for CreateInstance
    /// </summary>
    protected BusinessBase()
    {
      //禁止添加代码
    }

    /// <summary>
    /// for Newtonsoft.Json.JsonConstructor
    /// </summary>
    protected BusinessBase(bool? isNew, bool? isSelfDirty, bool? isSelfDeleted)
    {
      __isNew = isNew.HasValue && isNew.Value;
      __isSelfDirty = isSelfDirty.HasValue && isSelfDirty.Value;
      __isSelfDeleted = isSelfDeleted.HasValue && isSelfDeleted.Value;

      _initializedNew = true;
    }

    #region 工厂

    #region New

    /// <summary>
    /// 新增纯净对象
    /// 除WatermarkField外字段不用被填充缺省值
    /// needFillBusinessCodeFieldValues = true
    /// needInitializeNew = false
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1000:DoNotDeclareStaticMembersOnGenericTypes")]
    public static T NewPure()
    {
      return NewPure(true, false);
    }

    /// <summary>
    /// 新增纯净对象
    /// 除WatermarkField外字段不用被填充缺省值
    /// needInitializeNew = false
    /// </summary>
    /// <param name="needFillBusinessCodeFieldValues">是否需要填充业务码字段值</param>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1000:DoNotDeclareStaticMembersOnGenericTypes")]
    public static T NewPure(bool needFillBusinessCodeFieldValues)
    {
      return NewPure(needFillBusinessCodeFieldValues, false);
    }

    /// <summary>
    /// 新增纯净对象
    /// 除WatermarkField外字段不用被填充缺省值
    /// </summary>
    /// <param name="needFillBusinessCodeFieldValues">是否需要填充业务码字段值</param>
    /// <param name="needInitializeNew">是否需要调用OnInitializeNew函数</param>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1000:DoNotDeclareStaticMembersOnGenericTypes")]
    public static T NewPure(bool needFillBusinessCodeFieldValues, bool needInitializeNew)
    {
      T result = DynamicCreateInstance();
      result.InitializePureSelf(needFillBusinessCodeFieldValues, needInitializeNew);
      return result;
    }

    /// <summary>
    /// 新增对象
    /// 用缺省值填充字段值
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1000:DoNotDeclareStaticMembersOnGenericTypes")]
    public static T New()
    {
      return New(true);
    }
    
    /// <summary>
    /// 新增对象
    /// 用缺省值填充字段值
    /// </summary>
    /// <param name="needFillBusinessCodeFieldValues">是否需要填充业务码字段值</param>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1000:DoNotDeclareStaticMembersOnGenericTypes")]
    public static T New(bool needFillBusinessCodeFieldValues)
    {
      T result = DynamicCreateInstance();
      result.InitializeSelf(needFillBusinessCodeFieldValues, true);
      return result;
    }

    private static T New(Criterions criterions)
    {
      T result = DynamicCreateInstance();
      result.Criterions = criterions ?? new Criterions(typeof(T));
      result.InitializeSelf(null, true);
      return result;
    }

    /// <summary>
    /// 新增对象
    /// 用缺省值填充字段值
    /// </summary>
    /// <param name="owner">所属业务对象集合</param>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1000:DoNotDeclareStaticMembersOnGenericTypes")]
    internal static T New(IBusinessCollection owner)
    {
      T result = DynamicCreateInstance();
      result.InitializeSelf(owner, true);
      return result;
    }

    /// <summary>
    /// 新增对象
    /// 按照数据源填充(指定属性, 属性名或映射的表字段需一致, 忽略对映到本类主键的属性)
    /// </summary>
    /// <param name="source">数据源</param>
    /// <param name="propertyInfos">需匹配的属性信息, 当为null、空队列时匹配全部属性</param>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1000:DoNotDeclareStaticMembersOnGenericTypes")]
    public static T New(IBusinessObject source, params Phenix.Core.Mapping.IPropertyInfo[] propertyInfos)
    {
      if (source is T && (propertyInfos == null || propertyInfos.Length == 0))
        return New(source as T);

      T result = NewPure();
      EntityHelper.FillIdenticalFieldValuesByTargetProperty(source, result, false, propertyInfos);
      return result;
    }

    internal static T New(DataRow sourceFieldValues, IList<FieldMapInfo> fieldMapInfos)
    {
      T result = NewPure();
      EntityHelper.FillFieldValues(sourceFieldValues.ItemArray, result, fieldMapInfos);
      return result;
    }

    /// <summary>
    /// 新增对象
    /// 主键PrimaryKeyField字段(包括Details)重新生成
    /// </summary>
    /// <param name="cloneSource">Clone数据源</param>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1000:DoNotDeclareStaticMembersOnGenericTypes")]
    public static T New(T cloneSource)
    {
      return cloneSource != null ? cloneSource.Clone(true) : New();
    }

    #endregion

    #region Fetch

    /// <summary>
    /// 构建业务对象
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1000:DoNotDeclareStaticMembersOnGenericTypes")]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2200:RethrowToPreserveStackDetails")]
    protected static T Fetch(object criteria)
    {
      try
      {
        Criterions criterions = criteria as Criterions;
        if (criterions != null)
          criterions.CheckRules(true);
        T result = Csla.DataPortal.Fetch<T>(criteria);
        if (result != null && result.SelfFetched)
        {
          result.OnFetchedSelf(criteria);
          return result;
        }
        return null;
      }
      catch (Exception ex)
      {
        if (!(ex is ValidationException))
        {
          if (ex is Csla.DataPortalException && ex.InnerException != null)
            ex = ex.InnerException;
          if (ex is Csla.Reflection.CallMethodException && ex.InnerException != null)
            ex = ex.InnerException;
          EventLog.Save(typeof(T), MethodBase.GetCurrentMethod(), ex);
        }
        throw ex;
      }
    }

    #region 事件

    /// <summary>
    /// 构建本业务对象之后
    /// </summary>
    protected virtual void OnFetchedSelf(object criteria)
    {
    }

    #endregion

    /// <summary>
    /// 按照指定主键值来获取对应的数据库记录构建业务对象
    /// </summary>
    /// <param name="primaryKeyValue">主键值</param>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1000:DoNotDeclareStaticMembersOnGenericTypes")]
    public static T Fetch(long primaryKeyValue)
    {
      T itself = DynamicCreateInstance();
      if (EntityHelper.FillPrimaryKeyFieldValue(itself, primaryKeyValue))
        return Fetch(itself);
      return null;
    }

    /// <summary>
    /// 按照指定主键值来获取对应的数据库记录构建业务对象
    /// </summary>
    /// <param name="primaryKeyValue">主键值</param>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1000:DoNotDeclareStaticMembersOnGenericTypes")]
    public static T Fetch(string primaryKeyValue)
    {
      T itself = DynamicCreateInstance();
      if (EntityHelper.FillPrimaryKeyFieldValue(itself, primaryKeyValue))
        return Fetch(itself);
      return null;
    }

    /// <summary>
    /// 按照指定主键/唯一键值来获取对应的数据库记录构建业务对象
    /// </summary>
    /// <param name="itself">带主键/唯一键值的业务对象</param>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1000:DoNotDeclareStaticMembersOnGenericTypes")]
    public static T Fetch(T itself)
    {
      if (itself == null)
        throw new ArgumentNullException("itself");
      return Fetch((object)itself.PureClone());
    }

    /// <summary>
    /// 按照指定主键值来获取对应的数据库记录构建业务对象
    /// </summary>
    /// <param name="connection">数据库连接</param>
    /// <param name="primaryKeyValue">主键值</param>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1000:DoNotDeclareStaticMembersOnGenericTypes")]
    public static T Fetch(DbConnection connection, long primaryKeyValue)
    {
      T itself = DynamicCreateInstance();
      if (EntityHelper.FillPrimaryKeyFieldValue(itself, primaryKeyValue))
        return Fetch(connection, itself);
      return null;
    }

    /// <summary>
    /// 按照指定主键值来获取对应的数据库记录构建业务对象
    /// </summary>
    /// <param name="connection">数据库连接</param>
    /// <param name="primaryKeyValue">主键值</param>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1000:DoNotDeclareStaticMembersOnGenericTypes")]
    public static T Fetch(DbConnection connection, string primaryKeyValue)
    {
      T itself = DynamicCreateInstance();
      if (EntityHelper.FillPrimaryKeyFieldValue(itself, primaryKeyValue))
        return Fetch(connection, itself);
      return null;
    }

    /// <summary>
    /// 按照指定主键/唯一键值来获取对应的数据库记录构建业务对象
    /// </summary>
    /// <param name="connection">数据库连接</param>
    /// <param name="itself">带主键/唯一键值的业务对象</param>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1000:DoNotDeclareStaticMembersOnGenericTypes")]
    public static T Fetch(DbConnection connection, T itself)
    {
      if (connection == null)
        throw new ArgumentNullException("connection");
      if (itself == null)
        throw new ArgumentNullException("itself");
      T result = DynamicCreateInstance();
      ((IRefinedly)result).ExecuteFetchSelf(connection, new Criterions(typeof(T), itself));
      return result.SelfFetched ? result : null;
    }

    /// <summary>
    /// 按照指定主键值来获取对应的数据库记录构建业务对象
    /// </summary>
    /// <param name="transaction">数据库事务</param>
    /// <param name="primaryKeyValue">主键值</param>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1000:DoNotDeclareStaticMembersOnGenericTypes")]
    public static T Fetch(DbTransaction transaction, long primaryKeyValue)
    {
      T itself = DynamicCreateInstance();
      if (EntityHelper.FillPrimaryKeyFieldValue(itself, primaryKeyValue))
        return Fetch(transaction, itself);
      return null;
    }

    /// <summary>
    /// 按照指定主键值来获取对应的数据库记录构建业务对象
    /// </summary>
    /// <param name="transaction">数据库事务</param>
    /// <param name="primaryKeyValue">主键值</param>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1000:DoNotDeclareStaticMembersOnGenericTypes")]
    public static T Fetch(DbTransaction transaction, string primaryKeyValue)
    {
      T itself = DynamicCreateInstance();
      if (EntityHelper.FillPrimaryKeyFieldValue(itself, primaryKeyValue))
        return Fetch(transaction, itself);
      return null;
    }

    /// <summary>
    /// 按照指定主键/唯一键值来获取对应的数据库记录构建业务对象
    /// </summary>
    /// <param name="transaction">数据库事务</param>
    /// <param name="itself">带主键/唯一键值的业务对象</param>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1000:DoNotDeclareStaticMembersOnGenericTypes")]
    public static T Fetch(DbTransaction transaction, T itself)
    {
      if (transaction == null)
        throw new ArgumentNullException("transaction");
      if (itself == null)
        throw new ArgumentNullException("itself");
      T result = DynamicCreateInstance();
      ((IRefinedly)result).ExecuteFetchSelf(transaction, new Criterions(typeof(T), itself));
      return result.SelfFetched ? result : null;
    }

    /// <summary>
    /// 构建业务对象
    /// 表中仅一条记录
    /// 否则仅取表的第一条记录
    /// </summary>
    /// <param name="orderByInfos">数据排列顺序队列</param>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1000:DoNotDeclareStaticMembersOnGenericTypes")]
    public static T Fetch(params OrderByInfo[] orderByInfos)
    {
      return Fetch(new Criterions(typeof(T), orderByInfos));
    }

    /// <summary>
    /// 构建业务对象
    /// 条件类的字段映射关系请用Phenix.Core.Mapping.CriteriaFieldAttribute标注
    /// </summary>
    /// <param name="criteria">条件对象</param>
    /// <param name="orderByInfos">数据排列顺序队列</param>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1000:DoNotDeclareStaticMembersOnGenericTypes")]
    public static T Fetch(ICriteria criteria, params OrderByInfo[] orderByInfos)
    {
      return Fetch(new Criterions(typeof(T), criteria, orderByInfos));
    }

    /// <summary>
    /// 构建业务对象
    /// </summary>
    /// <param name="criteriaExpression">条件表达式</param>
    /// <param name="orderByInfos">数据排列顺序队列</param>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1000:DoNotDeclareStaticMembersOnGenericTypes")]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures")]
    public static T Fetch(Expression<Func<T, bool>> criteriaExpression, params OrderByInfo[] orderByInfos)
    {
      return Fetch(CriteriaHelper.ToCriteriaExpression(criteriaExpression), orderByInfos);
    }

    /// <summary>
    /// 构建业务对象
    /// </summary>
    /// <param name="dataSourceKey">数据源键</param>
    /// <param name="criteriaExpression">条件表达式</param>
    /// <param name="orderByInfos">数据排列顺序队列</param>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1000:DoNotDeclareStaticMembersOnGenericTypes")]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures")]
    public static T Fetch(string dataSourceKey, Expression<Func<T, bool>> criteriaExpression, params OrderByInfo[] orderByInfos)
    {
      return Fetch(CriteriaHelper.ToCriteriaExpression(dataSourceKey, criteriaExpression), orderByInfos);
    }

    /// <summary>
    /// 构建业务对象
    /// </summary>
    /// <param name="criteriaExpression">条件表达式</param>
    /// <param name="orderByInfos">数据排列顺序队列</param>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1000:DoNotDeclareStaticMembersOnGenericTypes")]
    public static T Fetch(CriteriaExpression criteriaExpression, params OrderByInfo[] orderByInfos)
    {
      return Fetch(new Criterions(typeof(T), criteriaExpression, orderByInfos));
    }

    /// <summary>
    /// 构建业务对象
    /// </summary>
    /// <param name="criterions">条件集</param>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1000:DoNotDeclareStaticMembersOnGenericTypes")]
    public static T Fetch(Criterions criterions)
    {
      if (criterions == null)
        throw new ArgumentNullException("criterions");
      T result = Fetch((object)criterions.PureClone());
      if (result != null)
      {
        result.Criterions = criterions;
        if (result.MasterBusiness != null)
          result.MasterBusiness.SetDetail(result);
        return result;
      }
      return null;
    }

    /// <summary>
    /// 构建业务对象
    /// 表中仅一条记录
    /// 否则仅取表的第一条记录
    /// </summary>
    /// <param name="connection">数据库连接</param>
    /// <param name="orderByInfos">数据排列顺序队列</param>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1000:DoNotDeclareStaticMembersOnGenericTypes")]
    public static T Fetch(DbConnection connection, params OrderByInfo[] orderByInfos)
    {
      return Fetch(connection, new Criterions(typeof(T), orderByInfos));
    }

    /// <summary>
    /// 构建业务对象
    /// 条件类的字段映射关系请用Phenix.Core.Mapping.CriteriaFieldAttribute标注
    /// </summary>
    /// <param name="connection">数据库连接</param>
    /// <param name="criteria">条件对象</param>
    /// <param name="orderByInfos">数据排列顺序队列</param>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1000:DoNotDeclareStaticMembersOnGenericTypes")]
    public static T Fetch(DbConnection connection, ICriteria criteria, params OrderByInfo[] orderByInfos)
    {
      return Fetch(connection, new Criterions(typeof(T), criteria, orderByInfos));
    }

    /// <summary>
    /// 构建业务对象
    /// </summary>
    /// <param name="connection">数据库连接</param>
    /// <param name="criteriaExpression">条件表达式</param>
    /// <param name="orderByInfos">数据排列顺序队列</param>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1000:DoNotDeclareStaticMembersOnGenericTypes")]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures")]
    public static T Fetch(DbConnection connection, Expression<Func<T, bool>> criteriaExpression, params OrderByInfo[] orderByInfos)
    {
      return Fetch(connection, CriteriaHelper.ToCriteriaExpression(criteriaExpression), orderByInfos);
    }

    /// <summary>
    /// 构建业务对象
    /// </summary>
    /// <param name="connection">数据库连接</param>
    /// <param name="criteriaExpression">条件表达式</param>
    /// <param name="orderByInfos">数据排列顺序队列</param>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1000:DoNotDeclareStaticMembersOnGenericTypes")]
    public static T Fetch(DbConnection connection, CriteriaExpression criteriaExpression, params OrderByInfo[] orderByInfos)
    {
      return Fetch(connection, new Criterions(typeof(T), criteriaExpression, orderByInfos));
    }

    /// <summary>
    /// 构建业务对象
    /// </summary>
    /// <param name="connection">数据库连接</param>
    /// <param name="criterions">条件集</param>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1000:DoNotDeclareStaticMembersOnGenericTypes")]
    public static T Fetch(DbConnection connection, Criterions criterions)
    {
      if (connection == null)
        throw new ArgumentNullException("connection");
      if (criterions == null)
        throw new ArgumentNullException("criterions");
      T result = DynamicCreateInstance();
      ((IRefinedly)result).ExecuteFetchSelf(connection, criterions);
      if (result.SelfFetched)
      {
        result.Criterions = criterions;
        if (result.MasterBusiness != null)
          result.MasterBusiness.SetDetail(result);
        return result;
      }
      return null;
    }

    /// <summary>
    /// 构建业务对象
    /// 表中仅一条记录
    /// 否则仅取表的第一条记录
    /// </summary>
    /// <param name="transaction">数据库事务</param>
    /// <param name="orderByInfos">数据排列顺序队列</param>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1000:DoNotDeclareStaticMembersOnGenericTypes")]
    public static T Fetch(DbTransaction transaction, params OrderByInfo[] orderByInfos)
    {
      return Fetch(transaction, new Criterions(typeof(T), orderByInfos));
    }

    /// <summary>
    /// 构建业务对象
    /// 条件类的字段映射关系请用Phenix.Core.Mapping.CriteriaFieldAttribute标注
    /// </summary>
    /// <param name="transaction">数据库事务</param>
    /// <param name="criteria">条件对象</param>
    /// <param name="orderByInfos">数据排列顺序队列</param>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1000:DoNotDeclareStaticMembersOnGenericTypes")]
    public static T Fetch(DbTransaction transaction, ICriteria criteria, params OrderByInfo[] orderByInfos)
    {
      return Fetch(transaction, new Criterions(typeof(T), criteria, orderByInfos));
    }

    /// <summary>
    /// 构建业务对象
    /// </summary>
    /// <param name="transaction">数据库事务</param>
    /// <param name="criteriaExpression">条件表达式</param>
    /// <param name="orderByInfos">数据排列顺序队列</param>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1000:DoNotDeclareStaticMembersOnGenericTypes")]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures")]
    public static T Fetch(DbTransaction transaction, Expression<Func<T, bool>> criteriaExpression, params OrderByInfo[] orderByInfos)
    {
      return Fetch(transaction, CriteriaHelper.ToCriteriaExpression(criteriaExpression), orderByInfos);
    }

    /// <summary>
    /// 构建业务对象
    /// </summary>
    /// <param name="transaction">数据库事务</param>
    /// <param name="criteriaExpression">条件表达式</param>
    /// <param name="orderByInfos">数据排列顺序队列</param>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1000:DoNotDeclareStaticMembersOnGenericTypes")]
    public static T Fetch(DbTransaction transaction, CriteriaExpression criteriaExpression, params OrderByInfo[] orderByInfos)
    {
      return Fetch(transaction, new Criterions(typeof(T), criteriaExpression, orderByInfos));
    }

    /// <summary>
    /// 构建业务对象
    /// </summary>
    /// <param name="transaction">数据库事务</param>
    /// <param name="criterions">条件集</param>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1000:DoNotDeclareStaticMembersOnGenericTypes")]
    public static T Fetch(DbTransaction transaction, Criterions criterions)
    {
      if (transaction == null)
        throw new ArgumentNullException("transaction");
      if (criterions == null)
        throw new ArgumentNullException("criterions");
      T result = DynamicCreateInstance();
      ((IRefinedly)result).ExecuteFetchSelf(transaction, criterions);
      if (result.SelfFetched)
      {
        result.Criterions = criterions;
        if (result.MasterBusiness != null)
          result.MasterBusiness.SetDetail(result);
        return result;
      }
      return null;
    }

    /// <summary>
    /// 构建业务对象
    /// 按照数据源填充(指定属性或映射的表字段需一致)
    /// </summary>
    /// <param name="source">数据源</param>
    /// <param name="propertyInfos">需匹配的属性信息, 当为null、空队列时匹配全部属性, 映射的表字段(或主外键)需一致</param>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1000:DoNotDeclareStaticMembersOnGenericTypes")]
    public static T Fetch(IBusinessObject source, params Phenix.Core.Mapping.IPropertyInfo[] propertyInfos)
    {
      T result = DynamicCreateInstance();
      result.FetchSelf(source, propertyInfos);
      return result;
    }

    internal static T Fetch(IDataRecord sourceFieldValues, IList<FieldMapInfo> fieldMapInfos)
    {
      T result = DynamicCreateInstance();
      result.FetchSelf(sourceFieldValues, fieldMapInfos);
      return result;
    }

    internal static T Fetch(object[] sourceFieldValues, IList<FieldMapInfo> fieldMapInfos)
    {
      T result = DynamicCreateInstance();
      result.FetchSelf(sourceFieldValues, fieldMapInfos);
      return result;
    }

    /// <summary>
    /// 构建自己
    /// </summary>
    protected void FetchSelf(IBusinessObject source, params Phenix.Core.Mapping.IPropertyInfo[] propertyInfos)
    {
      EntityHelper.FillIdenticalFieldValuesByTargetProperty(source, this, true, propertyInfos);
      MarkFetched();
      InitOldFieldValues(false, true);
    }
    void IBusinessObject.FetchSelf(IBusinessObject source, params Phenix.Core.Mapping.IPropertyInfo[] propertyInfos)
    {
      FetchSelf(source, propertyInfos);
    }

    [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
    bool IEntity.FetchSelf(IDataRecord sourceFieldValues, IList<FieldMapInfo> fieldMapInfos)
    {
      return FetchSelf(sourceFieldValues, fieldMapInfos);
    }

    private void FetchSelf(DbCommand command)
    {
      using (DbDataReader reader = DbCommandHelper.ExecuteReader(command, CommandBehavior.SingleRow))
      {
        if (reader.Read())
          FetchSelf(reader, ClassMemberHelper.GetFieldMapInfos(this.GetType(), reader));
      }
    }

    internal void FetchSelf(object[] sourceFieldValues, IList<FieldMapInfo> fieldMapInfos)
    {
      if (EntityHelper.FillFieldValues(sourceFieldValues, this, fieldMapInfos))
        MarkFetched();
    }

    #endregion

    #endregion

    #region 属性

    /// <summary>
    /// 数据源键
    /// 缺省为 Root.DataSourceKey
    /// 缺省为 Criterions.DataSourceKey
    /// 缺省为 T 上的 ClassAttribute.DataSourceKey
    /// </summary>
    [Newtonsoft.Json.JsonIgnore]
    [System.ComponentModel.Browsable(false)]
    [System.ComponentModel.DataAnnotations.Display(AutoGenerateField = false)]
    public override string DataSourceKey
    {
      get
      {
        if (!object.ReferenceEquals(Root, this))
          return Root.DataSourceKey;
        return Criterions != null ? Criterions.DataSourceKey : base.DataSourceKey;
      }
    }

    [Csla.NotUndoable]
    private Criterions _criterions;
    /// <summary>
    /// 条件集
    /// </summary>
    [Newtonsoft.Json.JsonIgnore]
    [System.ComponentModel.Browsable(false)]
    [System.ComponentModel.DataAnnotations.Display(AutoGenerateField = false)]
    public new Criterions Criterions
    {
      get { return _criterions; }
      private set { _criterions = value; }
    }
    ICriterions IEntity.Criterions
    {
      get { return Criterions; }
    }
    Criterions IBusiness.Criterions
    {
      get { return Criterions; }
    }
    Criterions IBusinessObject.Criterions
    {
      get { return Criterions; }
    }

    /// <summary>
    /// 等待Fetch执行的时间(秒数）, null 指示不限制
    /// 缺省为 null
    /// </summary>
    [Newtonsoft.Json.JsonIgnore]
    [System.ComponentModel.Browsable(false)]
    [System.ComponentModel.DataAnnotations.Display(AutoGenerateField = false)]
    public virtual int? FetchTimeout
    {
      get { return null; }
    }

    private static bool _historyFetchAllChecked;
    private static bool? _historyFetchAll;
    /// <summary>
    /// 构建自全部库
    /// 为 Root 上的 HistoryAttribute.FetchAll
    /// null: 仅主库
    /// true: 主库+历史库
    /// false: 历史库
    /// </summary>
    [Newtonsoft.Json.JsonIgnore]
    [System.ComponentModel.Browsable(false)]
    [System.ComponentModel.DataAnnotations.Display(AutoGenerateField = false)]
    public bool? HistoryFetchAll
    {
      get
      {
        if (!object.ReferenceEquals(Root, this))
          return Root.HistoryFetchAll;
        if (!_historyFetchAllChecked)
        {
          HistoryAttribute historyAttribute = (HistoryAttribute)Attribute.GetCustomAttribute(typeof(T), typeof(HistoryAttribute));
          if (historyAttribute != null)
            _historyFetchAll = historyAttribute.FetchAll;
          _historyFetchAllChecked = true;
        }
        return _historyFetchAll;
      }
    }

    [Csla.NotUndoable]
    private bool _initializedNew;
    /// <summary>
    /// 完成新增初始化
    /// </summary>
    protected bool InitializedNew
    {
      get { return _initializedNew; }
      private set { _initializedNew = value; }
    }

    #region 结构关系

    /// <summary>
    /// Parent
    /// </summary>
    [Obsolete("建议使用: Owner、MasterBusiness", false)]
    [Newtonsoft.Json.JsonIgnore]
    [System.ComponentModel.Browsable(false)]
    [System.ComponentModel.DataAnnotations.Display(AutoGenerateField = false)]
    public new Csla.Core.IParent Parent
    {
      get { return base.Parent; }
    }

    /// <summary>
    /// 所属业务对象集合
    /// </summary>
    [Newtonsoft.Json.JsonIgnore]
    [System.ComponentModel.Browsable(false)]
    [System.ComponentModel.DataAnnotations.Display(AutoGenerateField = false)]
    public IBusinessCollection Owner
    {
      get { return base.Parent as IBusinessCollection; }
    }

    /// <summary>
    /// 是否是根对象
    /// 仅允许从根对象上进行(级联)保存
    /// </summary>
    [Newtonsoft.Json.JsonIgnore]
    [System.ComponentModel.Browsable(false)]
    [System.ComponentModel.DataAnnotations.Display(AutoGenerateField = false)]
    public bool IsRoot
    {
      get { return object.ReferenceEquals(Root, this); }
    }

    /// <summary>
    /// 根对象
    /// </summary>
    [Newtonsoft.Json.JsonIgnore]
    [System.ComponentModel.Browsable(false)]
    [System.ComponentModel.DataAnnotations.Display(AutoGenerateField = false)]
    public IBusiness Root
    {
      get
      {
        IBusinessObject masterBusiness = MasterBusiness;
        if (masterBusiness == null)
        {
          if (Owner != null)
            return Owner;
          return this;
        }
        IBusiness result = masterBusiness.Root;
        if (result != null)
          return result;
        return masterBusiness;
      }
    }

    /// <summary>
    /// 根业务对象
    /// </summary>
    [Newtonsoft.Json.JsonIgnore]
    [System.ComponentModel.Browsable(false)]
    [System.ComponentModel.DataAnnotations.Display(AutoGenerateField = false)]
    public IBusinessObject RootBusiness
    {
      get
      {
        IBusinessObject masterBusiness = MasterBusiness;
        if (masterBusiness == null)
          return this;
        IBusinessObject result = masterBusiness.RootBusiness;
        if (result != null)
          return result;
        return masterBusiness;
      }
    }

    /// <summary>
    /// 主业务对象
    /// </summary>
    [Newtonsoft.Json.JsonIgnore]
    [System.ComponentModel.Browsable(false)]
    [System.ComponentModel.DataAnnotations.Display(AutoGenerateField = false)]
    public IBusinessObject MasterBusiness
    {
      get
      {
        if (Owner != null)
          return Owner.MasterBusiness;
        return Criterions != null ? Criterions.MasterBusiness : base.Parent as IBusinessObject;
      }
    }

    /// <summary>
    /// 分组名
    /// </summary>
    [Newtonsoft.Json.JsonIgnore]
    [System.ComponentModel.Browsable(false)]
    [System.ComponentModel.DataAnnotations.Display(AutoGenerateField = false)]
    public string GroupName
    {
      get
      {
        if (Owner != null)
          return Owner.GroupName;
        return Criterions != null ? Criterions.GroupName : null;
      }
    }

    [Csla.NotUndoable]
    private Dictionary<string, IBusiness> _details;

    [NonSerialized]
    [Csla.NotUndoable]
    private LinkedList<IBusiness> _sortedDetails;
    private LinkedList<IBusiness> SortedDetails
    {
      get
      {
        if (_sortedDetails == null && _details != null && _details.Count > 0)
        {
          LinkedList<IBusiness> result = new LinkedList<IBusiness>();
          foreach (KeyValuePair<string, IBusiness> kvp in _details)
          {
            LinkedListNode<IBusiness> node = result.First;
            while (node != null)
            {
              if (((IRefinedly)node.Value).CascadingExistLink(kvp.Value.Criterions.ResultCoreType, kvp.Value.Criterions.GroupName))
              {
                result.AddBefore(node, kvp.Value);
                break;
              }
              node = node.Next;
            }
            if (node == null)
              result.AddLast(kvp.Value);
          }
          _sortedDetails = result;
        }
        return _sortedDetails;
      }
    }

    private static List<CascadingDeleteDetailInfo> _cascadingDeleteDetailInfos;

    private const string AUTO_LINK_SIGN = AppConfig.UNKNOWN_VALUE;
    [Csla.NotUndoable]
    private Dictionary<string, IBusinessObject> _links;

    #endregion

    #region Selectable

    /// <summary>
    /// 所属对象勾选集合
    /// </summary>
    ISelectableCollection ISelectable.Owner
    {
      get { return Owner; }
    }

    /// <summary>
    /// Selected的属性信息
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1000:DoNotDeclareStaticMembersOnGenericTypes")]
    public static readonly PropertyInfo<bool> SelectedProperty =
      RegisterProperty<bool>(c => c.Selected, Phenix.Core.Properties.Resources.SelectedFriendlyName, true);
    [Csla.NotUndoable]
    [Field(NoMapping = true, FieldConfineType = FieldConfineType.Unconfined)]
    private bool _selected;
    /// <summary>
    /// 是否被勾选
    /// 用于标记本对象
    /// 缺省为 false
    /// </summary>
    public bool Selected
    {
      get { return _selected; }
      set
      {
        bool changed = _selected != value;
        if (changed)
        {
          if (!OnSelectedValueChanging())
            return;
          base.OnPropertyChanging(SelectedProperty.Name);
        }
        _selected = value;
        if (Owner != null)
          if (value)
          {
            if (!Owner.SelectedItems.Contains(this))
              Owner.SelectedItems.Add(this);
          }
          else
            Owner.SelectedItems.Remove(this);
        if (changed)
        {
          base.OnPropertyChanged(SelectedProperty.Name);
          OnSelectedValueChanged();
        }
      }
    }
    /// <summary>
    /// 是否被勾选
    /// </summary>
    [Newtonsoft.Json.JsonIgnore]
    [System.ComponentModel.Browsable(false)]
    [System.ComponentModel.DataAnnotations.Display(AutoGenerateField = false)]
    public BooleanOption SelectedOption
    {
      get { return Selected ? BooleanOption.Y : BooleanOption.N; }
    }

    /// <summary>
    /// 在勾选项清单
    /// </summary>
    [Newtonsoft.Json.JsonIgnore]
    [System.ComponentModel.Browsable(false)]
    [System.ComponentModel.DataAnnotations.Display(AutoGenerateField = false)]
    public bool InSelectableList
    {
      get { return Owner != null && Owner.InSelectableList; }
    }

    #endregion

    #region 编辑控制

    bool IBusiness.IsReadOnly
    {
      get { return IsReadOnly; }
    }

    /// <summary>
    /// 不参与多级撤销并阻断Detail对象的多级撤销
    /// 缺省为 false
    /// </summary>
    protected virtual bool NotUndoable
    {
      get { return (Owner != null && Owner.NotUndoable) || (MasterBusiness != null && MasterBusiness.NotUndoable); }
    }
    bool IBusiness.NotUndoable
    {
      get { return NotUndoable; }
    }

    /// <summary>
    /// 在编辑状态
    /// </summary>
    [Newtonsoft.Json.JsonIgnore]
    [System.ComponentModel.Browsable(false)]
    [System.ComponentModel.DataAnnotations.Display(AutoGenerateField = false)]
    public bool EditMode
    {
      get { return base.EditLevel > 0 || NotUndoable; }
    }
    /// <summary>
    /// 在编辑状态
    /// </summary>
    [Newtonsoft.Json.JsonIgnore]
    [System.ComponentModel.Browsable(false)]
    [System.ComponentModel.DataAnnotations.Display(AutoGenerateField = false)]
    public BooleanOption EditModeOption
    {
      get { return EditMode ? BooleanOption.Y : BooleanOption.N; }
    }

    /// <summary>
    /// 编辑层级
    /// </summary>
    [Newtonsoft.Json.JsonIgnore]
    [System.ComponentModel.Browsable(false)]
    [System.ComponentModel.DataAnnotations.Display(AutoGenerateField = false)]
    public new int EditLevel
    {
      get { return base.EditLevel; }
    }

    /// <summary>
    /// 是否允许被勾选
    /// </summary>
    [System.ComponentModel.Browsable(false)]
    [System.ComponentModel.DataAnnotations.Display(AutoGenerateField = false)]
    public override bool AllowEdit
    {
      get
      {
        if (Owner != null && !Owner.AllowEditItem)
          return false;
        return base.AllowEdit;
      }
    }

    /// <summary>
    /// 是否允许删除本对象
    /// </summary>
    [System.ComponentModel.Browsable(false)]
    [System.ComponentModel.DataAnnotations.Display(AutoGenerateField = false)]
    public override bool AllowDelete
    {
      get
      {
        if (Owner != null && !Owner.AllowDeleteItem)
          return false;
        if (InSelectableList)
          return false;
        return base.AllowDelete;
      }
    }

    /// <summary>
    /// 是否本业务对象及其主、从业务对象集合中含有脏数据
    /// 本属性不能用于判断是否处于编辑状态
    /// </summary>
    [Newtonsoft.Json.JsonIgnore]
    [System.ComponentModel.Browsable(false)]
    [System.ComponentModel.DataAnnotations.Display(AutoGenerateField = false)]
    public override bool IsDirty
    {
      get
      {
        List<IRefinedlyObject> ignoreLinks = new List<IRefinedlyObject>();
        return GetIsDirty(ref ignoreLinks);
      }
    }

    /// <summary>
    /// 是否本业务对象及其主、从业务对象集合中数据具备有效性
    /// </summary>
    [Newtonsoft.Json.JsonIgnore]
    [System.ComponentModel.Browsable(false)]
    [System.ComponentModel.DataAnnotations.Display(AutoGenerateField = false)]
    public override bool IsValid
    {
      get
      {
        List<IRefinedlyObject> ignoreLinks = new List<IRefinedlyObject>();
        return GetIsValid(ref ignoreLinks);
      }
    }

    /// <summary>
    /// 是否可以保存数据
    /// </summary>
    [Newtonsoft.Json.JsonIgnore]
    [System.ComponentModel.Browsable(false)]
    [System.ComponentModel.DataAnnotations.Display(AutoGenerateField = false)]
    public override bool IsSavable
    {
      get
      {
        if (IsReadOnly)
          return false;
        bool auth;
        if (IsDeleted)
          auth = CanDelete;
        else if (IsNew)
          auth = CanEdit && CanCreate;
        else
          auth = CanEdit;
        return auth && IsDirty && IsValid && !IsBusy; 
      }
    }

    /// <summary>
    /// 删除状态
    /// </summary>
    [Newtonsoft.Json.JsonIgnore]
    [System.ComponentModel.Browsable(false)]
    [System.ComponentModel.DataAnnotations.Display(AutoGenerateField = false)]
    public new bool IsDeleted
    {
      get
      {
        if (IsSelfDeleted)
          return true;
        if (!CascadingDelete)
          return false;
        IBusinessObject masterBusiness = MasterBusiness;
        while (masterBusiness != null)
        {
          if (masterBusiness.IsSelfDeleted)
            return true;
          if (!masterBusiness.CascadingDelete)
            return false;
          masterBusiness = masterBusiness.MasterBusiness;
        }
        return base.IsDeleted;
      }
    }

    bool IBusinessObject.IsNew
    {
      get { return IsNew; }
    }

    bool IBusinessObject.IsDeleted
    {
      get { return IsDeleted; }
    }

    bool IBusinessObject.IsSelfDeleted
    {
      get { return IsSelfDeleted; }
    }

    bool IBusinessObject.IsSelfDirty
    {
      get { return IsSelfDirty; }
    }

    #endregion
    
    #region BusinessCode

    [NonSerialized]
    [Csla.NotUndoable]
    private Dictionary<FieldMapInfo, string> _semisBusinessCodes;

    [Csla.NotUndoable]
    private bool _needFillBusinessCodeFieldValues = true;
    internal bool NeedFillBusinessCodeFieldValues
    {
      get { return _needFillBusinessCodeFieldValues; }
      set { _needFillBusinessCodeFieldValues = value; }
    }

    #endregion

    #region Data Access
    
    /// <summary>
    /// 是否级联Save?
    /// </summary>
    [Newtonsoft.Json.JsonIgnore]
    [System.ComponentModel.Browsable(false)]
    [System.ComponentModel.DataAnnotations.Display(AutoGenerateField = false)]
    public bool CascadingSave
    {
      get
      {
        if (Owner != null)
          return Owner.CascadingSave;
        return MasterBusiness == null || (Criterions == null || Criterions.CascadingSave);
      }
    }

    /// <summary>
    /// 是否级联Delete?
    /// CompositionDetail: CascadingDelete = true
    /// AggregationDetail: CascadingDelete = false
    /// 如果 CascadingSave = false 则忽略本属性值
    /// </summary>
    [Newtonsoft.Json.JsonIgnore]
    [System.ComponentModel.Browsable(false)]
    [System.ComponentModel.DataAnnotations.Display(AutoGenerateField = false)]
    public bool CascadingDelete
    {
      get
      {
        if (Owner != null)
          return Owner.CascadingDelete;
        return MasterBusiness == null || (Criterions == null || Criterions.CascadingDelete);
      }
    }

    [Csla.NotUndoable]
    private Collection<IBusiness> _firstTransactionData;
    /// <summary>
    /// 参与事务处理前端的业务队列
    /// </summary>
    [Newtonsoft.Json.JsonIgnore]
    [System.ComponentModel.Browsable(false)]
    [System.ComponentModel.DataAnnotations.Display(AutoGenerateField = false)]
    public Collection<IBusiness> FirstTransactionData
    {
      get
      {
        if (_firstTransactionData == null)
          _firstTransactionData = new Collection<IBusiness>();
        return _firstTransactionData;
      }
    }

    [Csla.NotUndoable]
    private Collection<IBusiness> _lastTransactionData;
    /// <summary>
    /// 参与事务处理末端的业务队列
    /// </summary>
    [Newtonsoft.Json.JsonIgnore]
    [System.ComponentModel.Browsable(false)]
    [System.ComponentModel.DataAnnotations.Display(AutoGenerateField = false)]
    public Collection<IBusiness> LastTransactionData
    {
      get
      {
        if (_lastTransactionData == null)
          _lastTransactionData = new Collection<IBusiness>();
        return _lastTransactionData;
      }
    }

    /// <summary>
    /// 需要提交关联的业务对象到服务端
    /// 缺省为 true
    /// </summary>
    protected virtual bool NeedSaveLinks
    {
      get { return true; }
    }

    /// <summary>
    /// 可自动保存关联的业务对象
    /// 缺省为 NeedSaveLinks
    /// </summary>
    protected virtual bool AutoSaveLinks
    {
      get { return NeedSaveLinks; }
    }

    /// <summary>
    /// 保留非脏对象以提交到服务端的代码使用
    /// 缺省为 false
    /// </summary>
    protected virtual bool EnsembleOnSaving
    {
      get { return false; }
    }
    bool IBusiness.EnsembleOnSaving
    {
      get { return EnsembleOnSaving; }
    }

    [Csla.NotUndoable]
    private bool _needRefreshSelf;
    /// <summary>
    /// 提交数据后需要刷新本地自己
    /// 缺省为 false
    /// </summary>
    protected virtual bool NeedRefreshSelf
    {
      get { return _needRefreshSelf; }
      private set
      {
        if (value)
        {
          if (Csla.ApplicationContext.LogicalExecutionLocation != Csla.ApplicationContext.LogicalExecutionLocations.Server)
            return;
        }
        else
        {
          if (Csla.ApplicationContext.LogicalExecutionLocation == Csla.ApplicationContext.LogicalExecutionLocations.Server)
            return;
        }
        _needRefreshSelf = value;
      }
    }

    /// <summary>
    /// 提交数据后需要刷新本地
    /// 缺省为 false
    /// </summary>
    protected bool NeedRefresh
    {
      get
      {
        List<IRefinedlyObject> ignoreLinks = new List<IRefinedlyObject>();
        return GetNeedRefresh(ref ignoreLinks);
      }
    }
    bool IBusiness.NeedRefresh
    {
      get { return NeedRefresh; }
    }

    [NonSerialized]
    [Csla.NotUndoable]
    private DbTransaction _dbTransaction;
    /// <summary>
    /// DbTransaction
    /// </summary>
    protected DbTransaction DbTransaction
    {
      get
      {
        try
        {
          DbTransaction result = Owner != null ? Owner.DbTransaction ?? _dbTransaction : _dbTransaction;
          if (result != null)
            if (result.Connection != null && result.Connection.State == ConnectionState.Open)
              return result;
            else
             _dbTransaction = null;
        }
        catch
        {
          _dbTransaction = null;
        }

        return null;
      }
      private set { _dbTransaction = value; }
    }
    /// <summary>
    /// DbTransaction
    /// </summary>
    DbTransaction IBusiness.DbTransaction
    {
      get { return DbTransaction; }
    }

    #endregion

    #endregion

    #region 事件

    #region ISelectable 成员

    [NonSerialized]
    [Csla.NotUndoable]
    private EventHandler<SelectedValueChangingEventArgs> _selectedValueChangingHandlers;
    /// <summary>
    /// Selected属性值被更改前
    /// </summary>
    public event EventHandler<SelectedValueChangingEventArgs> SelectedValueChanging
    {
      add { _selectedValueChangingHandlers = (EventHandler<SelectedValueChangingEventArgs>)Delegate.Combine(_selectedValueChangingHandlers, value); }
      remove { _selectedValueChangingHandlers = (EventHandler<SelectedValueChangingEventArgs>)Delegate.Remove(_selectedValueChangingHandlers, value); }
    }
    /// <summary>
    /// Selected属性值被更改前
    /// </summary>
    protected virtual bool OnSelectedValueChanging()
    {
      if (_selectedValueChangingHandlers != null)
      {
        SelectedValueChangingEventArgs e = new SelectedValueChangingEventArgs(this);
        _selectedValueChangingHandlers.Invoke(this, e);
        if (e.Stop)
          return false;
      }
      return true;
    }

    [NonSerialized]
    [Csla.NotUndoable]
    private EventHandler<SelectedValueChangedEventArgs> _selectedValueChangedHandlers;
    /// <summary>
    /// Selected属性值被更改后
    /// </summary>
    public event EventHandler<SelectedValueChangedEventArgs> SelectedValueChanged
    {
      add { _selectedValueChangedHandlers = (EventHandler<SelectedValueChangedEventArgs>)Delegate.Combine(_selectedValueChangedHandlers, value); }
      remove { _selectedValueChangedHandlers = (EventHandler<SelectedValueChangedEventArgs>)Delegate.Remove(_selectedValueChangedHandlers, value); }
    }
    /// <summary>
    /// Selected属性值被更改前
    /// </summary>
    protected virtual void OnSelectedValueChanged()
    {
      if (_selectedValueChangedHandlers != null)
        _selectedValueChangedHandlers.Invoke(this, new SelectedValueChangedEventArgs(this));
    }

    #endregion

    #endregion

    #region 方法

    #region ObjectCache

    /// <summary>
    /// 添加缓存类型
    /// </summary>
    protected void AddCacheType(IBusiness business)
    {
      AddCacheType((IRefinedly)business);
      if (MasterBusiness != null)
        MasterBusiness.AddCacheType(business);
    }
    void IBusinessObject.AddCacheType(IBusiness business)
    {
      AddCacheType(business);
    }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
    private IDictionary<string, Type> GetCacheTypes()
    {
      return CacheTypes;
    }
    IDictionary<string, Type> IRefinedly.GetCacheTypes()
    {
      return GetCacheTypes();
    }

    void IRefinedly.RecordHasChanged()
    {
      RecordHasChanged();
    }

    #endregion

    #region 克隆

    /// <summary>
    /// 克隆
    /// </summary>
    /// <param name="isNew">全新的</param>
    public virtual T Clone(bool isNew)
    {
      T result = Clone();
      if (isNew)
      {
        if (IsReadOnly)
          throw new InvalidOperationException(String.Format("标记了{0}的{1}对象是不允许全新克隆的",
            typeof(Phenix.Core.Mapping.ReadOnlyAttribute).FullName, this.GetType().FullName));

        ((Csla.Core.IEditableBusinessObject)result).SetParent(null);
        if (_details != null && _details.Count > 0)
          foreach (KeyValuePair<string, IBusiness> kvp in _details)
            if (kvp.Value.IsReadOnly || kvp.Value.InSelectableList)
              result.ClearDetailCache(kvp.Key);

        while (result.EditLevel > 0)
          result.ApplyEdit();
        result.MarkNew();
        result.IdValue = Sequence.Value;
      }
      return result;
    }

    /// <summary>
    /// 纯净克隆
    /// </summary>
    internal protected T PureClone()
    {
      T result = (T)MemberwiseClone();
      result.ClearFieldManagerFieldInfo();
      result._criterions = null;
      result._details = null;
      result._links = null;
      result._firstTransactionData = null;
      result._lastTransactionData = null;
      return result;
    }
    IBusinessObject IBusinessObject.PureClone()
    {
      return PureClone();
    }

    private T MemberwiseClone(IBusinessObject masterBusiness)
    {
      T result = (T)MemberwiseClone();
      result.ClearFieldManagerFieldInfo();
      if (_criterions != null)
        result._criterions = _criterions.MemberwiseClone(masterBusiness);
      return result;
    }

    #endregion

    #region 初始化

    /// <summary>
    /// 当新增初始化之后
    /// 本函数仅通过业务类工厂函数New()或将新增业务对象添加到业务集合时才被调用
    /// </summary>
    protected virtual void OnInitializeNew()
    {
    }

    private void InitializePureSelf(bool needFillBusinessCodeFieldValues, bool needInitializeNew)
    {
      NeedFillBusinessCodeFieldValues = needFillBusinessCodeFieldValues;
      InitializedNew = !needInitializeNew;

      FillWatermarkFieldValues(true);
      
      if (!InitializedNew)
      {
        InitializedNew = true;
        OnInitializeNew();
      }
    }

    private void InitializeSelf(bool needFillBusinessCodeFieldValues, bool reset)
    {
      NeedFillBusinessCodeFieldValues = needFillBusinessCodeFieldValues;
      InitializeSelf(null, reset);
    }

    private void InitializeSelf(IBusinessCollection owner, bool reset)
    {
      if (owner != null)
      {
        SetParent(owner);
        if (IsSelfDeleted)
          ((IRefinedlyCollection)owner).AddDeleted(this);
      }

      if (IsNew && !InitializedNew) //不再支持直接new业务对象后通过在Add到集合时来补填缺省值的做法：(!InitializedNew || owner != null))
      {
        FillFieldValuesToDefault(reset);
        FillWatermarkFieldValues(reset);
      }

      if (Criterions != null && Criterions.MasterBusiness != null)
      {
        CascadingLinkTo(Criterions.MasterBusiness, Criterions.GroupName, false);
        Criterions.MasterBusiness.SetDetail<T>((T)this);
      }

      if (owner != null && owner.MasterBusiness != null)
      {
        CascadingLinkTo(owner.MasterBusiness, owner.GroupName, false);
      }

      if (IsNew && !InitializedNew)
      {
        InitializedNew = true;
        OnInitializeNew();
      }
    }

    internal void MarkAsChild(IBusinessCollection owner, bool needInitializeSelf)
    {
      if (needInitializeSelf)
        InitializeSelf(owner, false);
      MarkAsChild();
    }

    /// <summary>
    /// 标为 IsNew = true
    /// 主键(包括Details)重新生成
    /// </summary>
    protected override void MarkNew()
    {
      base.MarkNew();
      
      InitializedNew = true;
      NeedRefreshSelf = true;

      FillFieldValuesToDefault(false);
      FillWatermarkFieldValues(true);

      if (MasterBusiness != null)
        CascadingLinkTo(MasterBusiness, GroupName, false);

      CascadingMarkNewInDetails(Criterions == null || Criterions.CascadingDelete);
    }
    private void CascadingMarkNew(bool cascadingDelete)
    {
      cascadingDelete = cascadingDelete && (Criterions == null || Criterions.CascadingDelete);

      if (cascadingDelete)
      {
        base.MarkNew();

        InitializedNew = true;
        NeedRefreshSelf = true;

        FillFieldValuesToDefault(false);
        FillWatermarkFieldValues(true);
      }

      if (MasterBusiness != null)
        CascadingLinkTo(MasterBusiness, GroupName, false);

      CascadingMarkNewInDetails(cascadingDelete);
    }
    void IRefinedly.CascadingMarkNew(bool cascadingDelete)
    {
      CascadingMarkNew(cascadingDelete);
    }

    private void CascadingMarkNewInDetails(bool cascadingDelete)
    {
      if (_details != null && _details.Count > 0)
      {
        IBusiness[] array = new IBusiness[_details.Count];
        _details.Values.CopyTo(array, 0);
        _details.Clear();
        foreach (IBusiness item in array)
        {
          item.Criterions.SetLink(this, item.Criterions.GroupName);
          ((IRefinedly)item).CascadingMarkNew(cascadingDelete);
          _details[item.Criterions.ToString()] = item;
        }
        _sortedDetails = null;
      }
    }

    /// <summary>
    /// 标为 IsNew = false 且 IsSelfDirty = false
    /// resetOldFieldValues = false
    /// </summary>
    protected new void MarkOld()
    {
      base.MarkOld();
    }

    /// <summary>
    /// 标为 IsNew = false 且 IsSelfDirty = false
    /// </summary>
    [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Advanced)]
    protected override void MarkOld(bool resetOldFieldValues)
    {
      NeedRefreshSelf = false;
      base.MarkOld(resetOldFieldValues);
    }
    
    /// <summary>
    /// 标为 IsSelfDirty = true
    /// suppressEvent = false
    /// </summary>
    protected new void MarkDirty()
    {
      MarkDirty(false);
    }

    /// <summary>
    /// 标为 IsSelfDirty = true
    /// </summary>
   [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Advanced)]
    protected new void MarkDirty(bool suppressEvent)
    {
      InitOldFieldValues(false, false);
      base.MarkDirty(suppressEvent);
      NeedRefreshSelf = true;
    }

    /// <summary>
    /// 置换为与source相同内容的对象
    /// </summary>
    public override void ReplaceFrom(T source)
    {
      if (object.ReferenceEquals(source, null) || object.ReferenceEquals(source, this))
        return;

      base.ReplaceFrom(source);
      if (source._details != null && source._details.Count > 0)
        foreach (KeyValuePair<string, IBusiness> kvp in source._details)
          DoSetDetail(kvp.Key, kvp.Value);
      if (source._links != null && source._links.Count > 0)
        foreach (KeyValuePair<string, IBusinessObject> kvp in source._links)
          DoSetLink(kvp.Key, kvp.Value);
    }

    /// <summary>
    /// 刷新本地
    /// </summary>
    public void Refresh()
    {
      ReplaceFrom(Fetch((T)this));
    }

    #endregion

    #region  Register Properties

    private static PropertyInfo<P> RegisterProperty<P>(PropertyInfo<P> info)
    {
      try
      {
        Csla.BusinessBase<T>.RegisterProperty(info);
        return info;
      }
      catch (Exception ex)
      {
        throw new InvalidOperationException(String.Format("类{0}属性{1}上RegisterProperty()注册代码有误!", info.OwnerType, info.Name), ex);
      }
    }

    /// <summary>
    /// 注册属性信息
    /// </summary>
    /// <typeparam name="P">属性类</typeparam>
    /// <param name="info">属性信息</param>
    /// <returns>属性信息</returns>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1061:DoNotHideBaseClassMethods")]
    protected new static PropertyInfo<P> RegisterProperty<P>(Csla.PropertyInfo<P> info)
    {
      FieldMapInfo fieldMapInfo = GetFieldMapInfo(info.Name);
      PropertyInfo<P> result = new PropertyInfo<P>(info, typeof(T),
        fieldMapInfo, fieldMapInfo != null ? fieldMapInfo.PropertyMapInfo : ClassMemberHelper.GetPropertyMapInfo(typeof(T), info.Name), null);
      return RegisterProperty(result);
    }

    /// <summary>
    /// 注册属性信息
    /// </summary>
    /// <typeparam name="P">属性类</typeparam>
    /// <param name="info">属性信息</param>
    /// <param name="defaultValueFunc">缺省值函数</param>
    /// <returns>属性信息</returns>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1061:DoNotHideBaseClassMethods")]
    protected static PropertyInfo<P> RegisterProperty<P>(Csla.PropertyInfo<P> info,
      Func<object> defaultValueFunc)
    {
      FieldMapInfo fieldMapInfo = GetFieldMapInfo(info.Name);
      PropertyInfo<P> result = new PropertyInfo<P>(info, typeof(T),
        fieldMapInfo, fieldMapInfo != null ? fieldMapInfo.PropertyMapInfo : ClassMemberHelper.GetPropertyMapInfo(typeof(T), info.Name), defaultValueFunc);
      return RegisterProperty(result);
    }

    /// <summary>
    /// 注册属性信息
    /// </summary>
    /// <typeparam name="P">属性类</typeparam>
    /// <param name="info">属性信息</param>
    /// <param name="defaultValueFunc">缺省值函数</param>
    /// <returns>属性信息</returns>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1061:DoNotHideBaseClassMethods")]
    protected static PropertyInfo<P> RegisterProperty<P>(Csla.PropertyInfo<P> info,
      Func<T, object> defaultValueFunc)
    {
      FieldMapInfo fieldMapInfo = GetFieldMapInfo(info.Name);
      DoSetDefaultValue(fieldMapInfo, defaultValueFunc);
      PropertyInfo<P> result = new PropertyInfo<P>(info, typeof(T),
        fieldMapInfo, fieldMapInfo != null ? fieldMapInfo.PropertyMapInfo : ClassMemberHelper.GetPropertyMapInfo(typeof(T), info.Name), null);
      return RegisterProperty(result);
    }

    /// <summary>
    /// 注册属性信息
    /// </summary>
    /// <typeparam name="P">属性类</typeparam>
    /// <param name="propertyLambdaExpression">属性表达式</param>
    /// <returns>属性信息</returns>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures")]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly")]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1715:IdentifiersShouldHaveCorrectPrefix")]
    protected new static PropertyInfo<P> RegisterProperty<P>(Expression<Func<T, object>> propertyLambdaExpression)
    {
      System.Reflection.PropertyInfo reflectedPropertyInfo = Phenix.Core.Reflection.Reflect<T>.GetProperty(propertyLambdaExpression);
      Csla.PropertyInfo<P> info =
        Csla.Core.FieldManager.PropertyInfoFactory.Factory.Create<P>(typeof(T), reflectedPropertyInfo.Name);
      return RegisterProperty(info);
    }

    /// <summary>
    /// 注册属性信息
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters")]
    [Obsolete("请使用: RegisterProperty<P>(Expression<Func<T, object>> propertyLambdaExpression)", true)]
    [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
    protected new static PropertyInfo<P> RegisterProperty<P>(Expression<Func<T, object>> propertyLambdaExpression,
      Csla.RelationshipTypes relationship)
    {
      throw new NotSupportedException(MethodBase.GetCurrentMethod().Name);
    }

    /// <summary>
    /// 注册属性信息
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters")]
    [Obsolete("请使用: RegisterProperty<P>(Expression<Func<T, object>> propertyLambdaExpression, P defaultValue, string friendlyName)", true)]
    [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
    protected new static PropertyInfo<P> RegisterProperty<P>(Expression<Func<T, object>> propertyLambdaExpression,
      string friendlyName)
    {
      throw new NotSupportedException(MethodBase.GetCurrentMethod().Name);
    }

    /// <summary>
    /// 注册属性信息
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters")]
    [Obsolete("请使用: RegisterProperty<P>(Expression<Func<T, object>> propertyLambdaExpression, P defaultValue, string friendlyName)", true)]
    [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
    protected new static PropertyInfo<P> RegisterProperty<P>(Expression<Func<T, object>> propertyLambdaExpression,
      string friendlyName, Csla.RelationshipTypes relationship)
    {
      throw new NotSupportedException(MethodBase.GetCurrentMethod().Name);
    }

    /// <summary>
    /// 注册属性信息
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters")]
    [Obsolete("请使用: RegisterProperty<P>(Expression<Func<T, object>> propertyLambdaExpression, P defaultValue, string friendlyName)", true)]
    [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
    protected new static PropertyInfo<P> RegisterProperty<P>(Expression<Func<T, object>> propertyLambdaExpression, 
      string friendlyName, P defaultValue)
    {
      throw new NotSupportedException(MethodBase.GetCurrentMethod().Name);
    }

    /// <summary>
    /// 注册属性信息
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters")]
    [Obsolete("请使用: RegisterProperty<P>(Expression<Func<T, object>> propertyLambdaExpression, P defaultValue, string friendlyName)", true)]
    [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
    protected new static PropertyInfo<P> RegisterProperty<P>(Expression<Func<T, object>> propertyLambdaExpression,
      string friendlyName, P defaultValue, Csla.RelationshipTypes relationship)
    {
      throw new NotSupportedException(MethodBase.GetCurrentMethod().Name);
    }

    private static PropertyInfo<P> RegisterProperty<P>(Expression<Func<T, object>> propertyLambdaExpression,
      string friendlyName, bool selectable)
    {
      PropertyInfo<P> result = RegisterProperty<P>(propertyLambdaExpression, (Func<object>)null, friendlyName);
      result.Selectable = selectable;
      return result;
    }

    /// <summary>
    /// 注册属性信息
    /// </summary>
    /// <typeparam name="P">属性类</typeparam>
    /// <param name="propertyLambdaExpression">属性表达式</param>
    /// <param name="defaultValue">缺省值</param>
    /// <returns>属性信息</returns>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures")]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1061:DoNotHideBaseClassMethods")]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly")]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1715:IdentifiersShouldHaveCorrectPrefix")]
    protected static PropertyInfo<P> RegisterProperty<P>(Expression<Func<T, object>> propertyLambdaExpression,
      object defaultValue)
    {
      return RegisterProperty(propertyLambdaExpression, (P)Utilities.ChangeType(defaultValue, typeof(P)), null);
    }

    /// <summary>
    /// 注册属性信息
    /// </summary>
    /// <typeparam name="P">属性类</typeparam>
    /// <param name="propertyLambdaExpression">属性表达式</param>
    /// <param name="defaultValue">缺省值</param>
    /// <returns>属性信息</returns>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures")]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly")]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1715:IdentifiersShouldHaveCorrectPrefix")]
    protected static PropertyInfo<P> RegisterProperty<P>(Expression<Func<T, object>> propertyLambdaExpression,
      P defaultValue)
    {
      return RegisterProperty(propertyLambdaExpression, defaultValue, null);
    }

    /// <summary>
    /// 注册属性信息
    /// </summary>
    /// <typeparam name="P">属性类</typeparam>
    /// <param name="propertyLambdaExpression">属性表达式</param>
    /// <param name="defaultValue">缺省值</param>
    /// <param name="friendlyName">友好名</param>
    /// <returns>属性信息</returns>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures")]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly")]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1715:IdentifiersShouldHaveCorrectPrefix")]
    protected static PropertyInfo<P> RegisterProperty<P>(Expression<Func<T, object>> propertyLambdaExpression,
      object defaultValue, string friendlyName)
    {
      return RegisterProperty(propertyLambdaExpression, (P)Utilities.ChangeType(defaultValue, typeof(P)), friendlyName);
    }

    /// <summary>
    /// 注册属性信息
    /// </summary>
    /// <typeparam name="P">属性类</typeparam>
    /// <param name="propertyLambdaExpression">属性表达式</param>
    /// <param name="defaultValue">缺省值</param>
    /// <param name="friendlyName">友好名</param>
    /// <returns>属性信息</returns>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures")]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly")]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1715:IdentifiersShouldHaveCorrectPrefix")]
    protected static PropertyInfo<P> RegisterProperty<P>(Expression<Func<T, object>> propertyLambdaExpression,
      P defaultValue, string friendlyName)
    {
      System.Reflection.PropertyInfo reflectedPropertyInfo = Phenix.Core.Reflection.Reflect<T>.GetProperty(propertyLambdaExpression);
      Csla.PropertyInfo<P> info =
        Csla.Core.FieldManager.PropertyInfoFactory.Factory.Create<P>(typeof(T), reflectedPropertyInfo.Name, friendlyName, defaultValue);
      return RegisterProperty(info);
    }

    /// <summary>
    /// 注册属性信息
    /// </summary>
    /// <typeparam name="P">属性类</typeparam>
    /// <param name="propertyLambdaExpression">属性表达式</param>
    /// <param name="defaultValueFunc">缺省值函数</param>
    /// <returns>属性信息</returns>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures")]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly")]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1715:IdentifiersShouldHaveCorrectPrefix")]
    protected static PropertyInfo<P> RegisterProperty<P>(Expression<Func<T, object>> propertyLambdaExpression,
      Func<object> defaultValueFunc)
    {
      System.Reflection.PropertyInfo reflectedPropertyInfo = Phenix.Core.Reflection.Reflect<T>.GetProperty(propertyLambdaExpression);
      Csla.PropertyInfo<P> info =
        Csla.Core.FieldManager.PropertyInfoFactory.Factory.Create<P>(typeof(T), reflectedPropertyInfo.Name, null);
      return RegisterProperty(info, defaultValueFunc);
    }

    /// <summary>
    /// 注册属性信息
    /// </summary>
    /// <typeparam name="P">属性类</typeparam>
    /// <param name="propertyLambdaExpression">属性表达式</param>
    /// <param name="defaultValueFunc">缺省值函数</param>
    /// <returns>属性信息</returns>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures")]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly")]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1715:IdentifiersShouldHaveCorrectPrefix")]
    protected static PropertyInfo<P> RegisterProperty<P>(Expression<Func<T, object>> propertyLambdaExpression,
      Func<T, object> defaultValueFunc)
    {
      System.Reflection.PropertyInfo reflectedPropertyInfo = Phenix.Core.Reflection.Reflect<T>.GetProperty(propertyLambdaExpression);
      Csla.PropertyInfo<P> info =
        Csla.Core.FieldManager.PropertyInfoFactory.Factory.Create<P>(typeof(T), reflectedPropertyInfo.Name, null);
      return RegisterProperty(info, defaultValueFunc);
    }

    /// <summary>
    /// 注册属性信息
    /// </summary>
    /// <typeparam name="P">属性类</typeparam>
    /// <param name="propertyLambdaExpression">属性表达式</param>
    /// <param name="defaultValueFunc">缺省值函数</param>
    /// <param name="friendlyName">友好名</param>
    /// <returns>属性信息</returns>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures")]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly")]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1715:IdentifiersShouldHaveCorrectPrefix")]
    protected static PropertyInfo<P> RegisterProperty<P>(Expression<Func<T, object>> propertyLambdaExpression,
      Func<object> defaultValueFunc, string friendlyName)
    {
      System.Reflection.PropertyInfo reflectedPropertyInfo = Phenix.Core.Reflection.Reflect<T>.GetProperty(propertyLambdaExpression);
      Csla.PropertyInfo<P> info =
        Csla.Core.FieldManager.PropertyInfoFactory.Factory.Create<P>(typeof(T), reflectedPropertyInfo.Name, friendlyName);
      return RegisterProperty(info, defaultValueFunc);
    }

    /// <summary>
    /// 注册属性信息
    /// </summary>
    /// <typeparam name="P">属性类</typeparam>
    /// <param name="propertyLambdaExpression">属性表达式</param>
    /// <param name="defaultValueFunc">缺省值函数</param>
    /// <param name="friendlyName">友好名</param>
    /// <returns>属性信息</returns>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures")]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly")]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1715:IdentifiersShouldHaveCorrectPrefix")]
    protected static PropertyInfo<P> RegisterProperty<P>(Expression<Func<T, object>> propertyLambdaExpression,
      Func<T, object> defaultValueFunc, string friendlyName)
    {
      System.Reflection.PropertyInfo reflectedPropertyInfo = Phenix.Core.Reflection.Reflect<T>.GetProperty(propertyLambdaExpression);
      Csla.PropertyInfo<P> info =
        Csla.Core.FieldManager.PropertyInfoFactory.Factory.Create<P>(typeof(T), reflectedPropertyInfo.Name, friendlyName);
      return RegisterProperty(info, defaultValueFunc);
    }

    #endregion

    #region Property

    /// <summary>
    /// 属性可读
    /// </summary>
    /// <param name="property">属性信息</param>
    /// <returns>属性可读</returns>
    public override bool CanReadProperty(Csla.Core.IPropertyInfo property)
    {
      if (base.CanReadProperty(property))
        return true;
      FieldConfineType fieldConfineType = ClassMemberHelper.GetFieldConfineType(property as IPropertyInfo);
      switch (fieldConfineType)
      {
        case FieldConfineType.Unconfined:
        case FieldConfineType.Selectable:
          return true;
        default:
          return false;
      }
    }

    /// <summary>
    /// 属性可写
    /// </summary>
    /// <param name="property">属性信息</param>
    /// <returns>属性可写</returns>
    public override bool CanWriteProperty(Csla.Core.IPropertyInfo property)
    {
      if (base.CanWriteProperty(property))
        return true;
      FieldConfineType fieldConfineType = ClassMemberHelper.GetFieldConfineType(property as IPropertyInfo);
      switch (fieldConfineType)
      {
        case FieldConfineType.Unconfined:
        case FieldConfineType.Selectable:
          return true;
        default:
          return false;
      }
    }

    /// <summary>
    /// 属性发生更改时
    /// </summary>
    /// <param name="propertyInfo">属性信息</param>
    protected new void OnPropertyChanging(Csla.Core.IPropertyInfo propertyInfo)
    {
      base.OnPropertyChanging(propertyInfo);
    }

    /// <summary>
    /// 属性发生更改后
    /// </summary>
    /// <param name="propertyInfo">属性信息</param>
    protected new void OnPropertyChanged(Csla.Core.IPropertyInfo propertyInfo)
    {
      base.OnPropertyChanged(propertyInfo);
    }

    /// <summary>
    /// 属性发生更改后
    /// </summary>
    /// <param name="propertyName">属性名</param>
    protected override void OnPropertyChanged(string propertyName)
    {
      if (!String.IsNullOrEmpty(propertyName))
        NeedRefreshSelf = true;
      base.OnPropertyChanged(propertyName);
    }

    /// <summary>
    /// 执行业务规则
    /// </summary>
    protected override void ExecuteRules<F>(FieldMapInfo fieldMapInfo, ref F field, F newValue)
    {
      base.ExecuteRules(fieldMapInfo, ref field, newValue);
      if (fieldMapInfo != null)
      {
        List<string> propertyNames = null;
        if (_details != null && _details.Count > 0 || _links != null && _links.Count > 0)
          propertyNames = new List<string>
          {
            fieldMapInfo.PropertyName
          };
        if (field != null)
        {
          if (fieldMapInfo.IsBusinessCodeCriteriaProperty && NeedFillBusinessCodeFieldValues)
            EntityHelper.FillBusinessCodeFieldValues(this, fieldMapInfo, ref _semisBusinessCodes, false);
          else
            EntityHelper.FillBusinessCodeFieldValues(this, fieldMapInfo, _semisBusinessCodes);
          if (propertyNames != null && _semisBusinessCodes != null)
            foreach (KeyValuePair<FieldMapInfo, string> kvp in _semisBusinessCodes)
              propertyNames.Add(kvp.Key.PropertyName);
        }
        if (propertyNames != null)
        {
          if (fieldMapInfo.FieldAttribute.IsPrimaryKey)
            CascadingLinkToInDetails(this, null);
          else if (fieldMapInfo.FieldLinkAttribute != null)
            TidyLinks(fieldMapInfo);
          CascadingFillIdenticalValuesInDetailsAndLinks(this, propertyNames.ToArray());
        }
      }
    }

    #endregion

    #region Owner

    /// <summary>
    /// 移动到新队列中
    /// </summary>
    public void MoveTo(IBusinessCollection newOwner)
    {
      if (Owner != null)
      {
        if (Owner.Criterions.MasterBusiness != null)
          Unlink(Owner.Criterions.MasterBusiness, Owner.Criterions.GroupName, false);
        ((IRefinedlyCollection)Owner).ClearRemove(this);
      }
      if (newOwner != null)
      {
        newOwner.Add(this);
        if (newOwner.Criterions == null || newOwner.Criterions.MasterBusiness == null)
          InitOldFieldValues(false, false);
        if (newOwner.Criterions.MasterBusiness != null)
          LinkTo(newOwner.Criterions.MasterBusiness, newOwner.Criterions.GroupName, false);
      }
      MarkDirty();
    }

    /// <summary>
    /// 删除
    /// </summary>
    public override void Delete()
    {
      if (Owner != null)
        Owner.Remove(this);
      else
        base.Delete();
    }

    #endregion

    #region Link

    /// <summary>
    /// 清除关联业务对象Cache
    /// </summary>
    internal protected void ClearLinkCache()
    {
      if (_links != null && _links.Count > 0)
      {
        List<string> keys = new List<string>(_links.Count);
        foreach (KeyValuePair<string, IBusinessObject> kvp in _links)
          if (!IsAutoLinkKey(kvp.Key))
            keys.Add(kvp.Key);
        foreach (string s in keys)
          _links.Remove(s);
      }
    }
    
    /// <summary>
    /// 清除关联业务对象Cache
    /// </summary>
    protected void ClearLinkCache(string key)
    {
      if (_links != null && _links.Count > 0)
        _links.Remove(key);
    }

    /// <summary>
    /// 清除关联业务对象Cache
    /// </summary>
    protected void ClearLinkCache(Type type)
    {
      if (_links != null && _links.Count > 0)
      {
        List<string> keys = new List<string>(_links.Count);
        foreach (KeyValuePair<string, IBusinessObject> kvp in _links)
          if (kvp.Value == null || String.CompareOrdinal(kvp.Value.GetType().FullName, type.FullName) == 0)
            if (!IsAutoLinkKey(kvp.Key))
              keys.Add(kvp.Key);
        foreach (string s in keys)
          _links.Remove(s);
      }
    }

    /// <summary>
    /// 清除关联业务对象Cache
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
    protected void ClearLinkCache<TLink>()
      where TLink : BusinessBase<TLink>
    {
      ClearLinkCache(typeof(TLink));
    }

    private static string AssembleLinkKey(string linkTypeName, string groupName, bool isAutoLink)
    {
      return String.Format("{0}{1},{2}", (isAutoLink ? AUTO_LINK_SIGN : String.Empty), linkTypeName, groupName);
    }

    private static bool IsAutoLinkKey(string key)
    {
      return key.StartsWith(AUTO_LINK_SIGN);
    }

    private void MergeSource_PropertyChanged(object sender, PropertyChangedEventArgs e)
    {
      if (!String.IsNullOrEmpty(e.PropertyName))
        CascadingFillIdenticalValues((IBusinessObject)sender);
    }

    internal void SetMergeMode(IBusinessObject mergeSource)
    {
      if (mergeSource != null)
        mergeSource.PropertyChanged += new PropertyChangedEventHandler(MergeSource_PropertyChanged);
    }

    internal void RemoveMergeMode(IBusinessObject mergeSource)
    {
      if (mergeSource != null)
        mergeSource.PropertyChanged -= new PropertyChangedEventHandler(MergeSource_PropertyChanged);
    }

    private void DoSetLink(string key, IBusinessObject linkBusiness)
    {
      if (_links == null)
        _links = new Dictionary<string, IBusinessObject>(StringComparer.Ordinal);
      _links[key] = linkBusiness;

      if (linkBusiness != null)
      {
        AddCacheType(linkBusiness);
        SetMergeMode(linkBusiness);
      }
    }

    private void DoSetLink(IBusinessObject linkBusiness, string groupName, bool isAutoLink)
    {
      DoSetLink(AssembleLinkKey(linkBusiness != null ? linkBusiness.GetType().FullName : null, groupName, isAutoLink), linkBusiness);
    }

    private void DoRemoveLink(string key, IBusinessObject linkBusiness)
    {
      ClearLinkCache(key);
      RemoveMergeMode(linkBusiness);
    }

    private void TidyAutoLinks()
    {
      ClassMapInfo classMapInfo = ClassMemberHelper.GetClassMapInfo(this.GetType());
      if (classMapInfo.ForeignKeyFieldMapInfos.Count > 0)
        foreach (FieldMapInfo item in classMapInfo.ForeignKeyFieldMapInfos)
          if (item.FieldLinkAttribute != null && item.FieldLinkAttribute.AutoLinkType != null)
          {
            IBusinessObject link = Activator.CreateInstance(item.FieldLinkAttribute.AutoLinkType, true) as IBusinessObject;
            if (link != null)
            {
              link.FetchSelf(this);
              DoSetLink(link, item.LinkGroupName, true);
            }
          }
    }

    private void TidyLinks(FieldMapInfo fieldMapInfo)
    {
      if (_links != null && _links.Count > 0)
      {
        List<string> keys = new List<string>(_links.Count);
        foreach (KeyValuePair<string, IBusinessObject> kvp in _links)
          if (kvp.Value != null && !IsAutoLinkKey(kvp.Key))
          {
            bool? isLink = EntityLinkHelper.IsLink(this, kvp.Value, fieldMapInfo);
            if (isLink.HasValue && !isLink.Value)
              keys.Add(kvp.Key);
          }
        foreach (string s in keys)
          _links.Remove(s);
      }
    }

    /// <summary>
    /// 设置关联业务对象
    /// groupName = null
    /// </summary>
    /// <param name="linkBusiness">关联业务对象</param>
    public void SetLink<TLink>(TLink linkBusiness)
      where TLink : BusinessBase<TLink>
    {
      SetLink(linkBusiness, null);
    }

    /// <summary>
    /// 设置关联业务对象
    /// </summary>
    /// <param name="linkBusiness">关联业务对象</param>
    /// <param name="groupName">分组名</param>
    public void SetLink<TLink>(TLink linkBusiness, string groupName)
       where TLink : BusinessBase<TLink>
    {
      DoSetLink(linkBusiness, groupName, false);
    }
    
    /// <summary>
    /// Link表达式
    /// groupName = null
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1000:DoNotDeclareStaticMembersOnGenericTypes")]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
    public static CriteriaExpression Link<TLink>()
       where TLink : BusinessBase<TLink>
    {
      return Link<TLink>(null);
    }

    /// <summary>
    /// Link表达式
    /// </summary>
    /// <param name="groupName">分组名</param>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1000:DoNotDeclareStaticMembersOnGenericTypes")]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
    public static CriteriaExpression Link<TLink>(string groupName)
       where TLink : BusinessBase<TLink>
    {
      return CriteriaExpression.Link(typeof(T), typeof(TLink), groupName);
    }

    /// <summary>
    /// 是否关联
    /// throwIfNotFound = true
    /// groupName = null
    /// </summary>
    /// <param name="link">关联</param>
    public bool IsLink(object link)
    {
      return IsLink(link, null, true);
    }

    /// <summary>
    /// 是否关联
    /// throwIfNotFound = true
    /// </summary>
    /// <param name="link">关联对象</param>
    /// <param name="groupName">分组名</param>
    public bool IsLink(object link, string groupName)
    {
      return IsLink(link, groupName, true);
    }

    /// <summary>
    /// 是否关联
    /// </summary>
    /// <param name="link">关联对象</param>
    /// <param name="groupName">分组名</param>
    /// <param name="throwIfNotFound">如果为 true, 则会在找不到信息时引发 InvalidOperationException; 如果为 false, 则在找不到信息时返回 null</param>
    public virtual bool IsLink(object link, string groupName, bool throwIfNotFound)
    {
      return EntityLinkHelper.IsLink(this, link, groupName, throwIfNotFound);
    }

    /// <summary>
    /// 关联
    /// groupName = null
    /// throwIfNotFound = true
    /// </summary>
    /// <param name="linkBusiness">关联业务对象</param>
    public void LinkTo<TLink>(TLink linkBusiness)
      where TLink : BusinessBase<TLink>
    {
      LinkTo(linkBusiness, null, true);
    }

    /// <summary>
    /// 关联
    /// throwIfNotFound = true
    /// </summary>
    /// <param name="linkBusiness">关联业务对象</param>
    /// <param name="groupName">分组名</param>
    public void LinkTo<TLink>(TLink linkBusiness, string groupName)
      where TLink : BusinessBase<TLink>
    {
      LinkTo(linkBusiness, groupName, true);
    }

    /// <summary>
    /// 关联
    /// </summary>
    /// <param name="link">关联对象</param>
    /// <param name="groupName">分组名</param>
    /// <param name="throwIfNotFound">如果为 true, 则会在找不到信息时引发 InvalidOperationException; 如果为 false, 则在找不到信息时返回 null</param>
    public virtual void LinkTo(object link, string groupName, bool throwIfNotFound)
    {
      if (link == null)
        return;
      IBusinessObject linkBusiness = link as IBusinessObject;
      if (linkBusiness != null)
      {
        IBusinessObject p = linkBusiness;
        do
        {
          CascadingLinkTo(p, groupName, throwIfNotFound);
          p = p.MasterBusiness;
          throwIfNotFound = false;
        } while (p != null && String.CompareOrdinal(p.GetType().FullName, this.GetType().FullName) != 0);
        DoSetLink(linkBusiness, groupName, false);
      }
      else
        CascadingLinkTo(link, groupName, throwIfNotFound);
    }

    /// <summary>
    /// 解除关联
    /// groupName = null
    /// throwIfNotFound = true
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
    public void Unlink<TLink>()
      where TLink : BusinessBase<TLink>
    {
      Unlink(typeof(TLink).FullName, null);
    }

    /// <summary>
    /// 解除关联
    /// throwIfNotFound = true
    /// </summary>
    /// <param name="groupName">分组名</param>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
    public void Unlink<TLink>(string groupName)
      where TLink : BusinessBase<TLink>
    {
      Unlink(typeof(TLink).FullName, groupName);
    }

    private void Unlink(string linkTypeName, string groupName)
    {
      IBusinessObject linkBusiness;
      if (TryGetLink(linkTypeName, groupName, out linkBusiness))
        Unlink(linkBusiness, groupName, true);
    }

    /// <summary>
    /// 解除关联
    /// throwIfNotFound = true
    /// </summary>
    /// <param name="linkBusiness">关联业务对象</param>
    /// <param name="groupName">分组名</param>
    public void Unlink<TLink>(TLink linkBusiness, string groupName)
       where TLink : BusinessBase<TLink>
    {
      Unlink(linkBusiness, groupName, true);
    }

    /// <summary>
    /// 解除关联
    /// </summary>
    /// <param name="link">关联对象</param>
    /// <param name="groupName">分组名</param>
    /// <param name="throwIfNotFound">如果为 true, 则会在找不到信息时引发 InvalidOperationException; 如果为 false, 则在找不到信息时返回 null</param>
    public virtual void Unlink(object link, string groupName, bool throwIfNotFound)
    {
      if (link == null)
        return;
      IBusinessObject linkBusiness = link as IBusinessObject;
      if (linkBusiness != null)
      {
        IBusinessObject p = linkBusiness;
        do
        {
          CascadingUnlink(p, groupName, throwIfNotFound);
          p = p.MasterBusiness;
          throwIfNotFound = false;
        } while (p != null && String.CompareOrdinal(p.GetType().FullName, this.GetType().FullName) != 0);
        DoRemoveLink(AssembleLinkKey(linkBusiness.GetType().FullName, groupName, false), linkBusiness);
      }
      else
        CascadingUnlink(link, groupName, throwIfNotFound);
    }

    /// <summary>
    /// 尝试检索关联业务对象
    /// groupName = null
    /// </summary>
    /// <param name="linkBusiness">关联业务对象</param>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
    public bool TryGetLink<TLink>(out TLink linkBusiness)
      where TLink : BusinessBase<TLink>
    {
      linkBusiness = FindLink<TLink>();
      return linkBusiness != null;
    }

    /// <summary>
    /// 尝试检索关联业务对象
    /// </summary>
    /// <param name="groupName">分组名</param>
    /// <param name="linkBusiness">关联业务对象</param>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
    public bool TryGetLink<TLink>(string groupName, out TLink linkBusiness)
      where TLink : BusinessBase<TLink>
    {
      linkBusiness = FindLink<TLink>(groupName);
      return linkBusiness != null;
    }

    private bool TryGetLink(string linkTypeName, string groupName, out IBusinessObject linkBusiness)
    {
      linkBusiness = FindLink(linkTypeName, groupName);
      return linkBusiness != null;
    }

    /// <summary>
    /// 检索关联业务对象
    /// groupName = null
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
    public TLink FindLink<TLink>()
      where TLink : BusinessBase<TLink>
    {
      return FindLink(typeof(TLink).FullName, null) as TLink;
    }

    /// <summary>
    /// 检索关联业务对象
    /// </summary>
    /// <param name="groupName">分组名</param>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
    public TLink FindLink<TLink>(string groupName)
      where TLink : BusinessBase<TLink>
    {
      return FindLink(typeof(TLink).FullName, groupName) as TLink;
    }

    private IBusinessObject FindLink(string linkTypeName, string groupName)
    {
      string key = AssembleLinkKey(linkTypeName, groupName, false);
      if (Owner != null && Owner.Criterions != null && Owner.Criterions.MasterBusiness != null)
        if (String.CompareOrdinal(key, AssembleLinkKey(Owner.Criterions.MasterBusiness.GetType().FullName, Owner.Criterions.GroupName, false)) == 0)
          return Owner.Criterions.MasterBusiness;
      IBusinessObject result = null;
      if (_links != null && _links.Count > 0)
        _links.TryGetValue(key, out result);
      return result;
    }

    /// <summary>
    /// 取关联业务对象
    /// groupName = null
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
    public TLink GetLink<TLink>()
      where TLink : BusinessBase<TLink>
    {
      return GetLink<TLink>((string)null);
    }

    /// <summary>
    /// 取关联业务对象
    /// </summary>
    /// <param name="groupName">分组名</param>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
    public TLink GetLink<TLink>(string groupName)
      where TLink : BusinessBase<TLink>
    {
      TLink result;
      if (!TryGetLink(groupName, out result))
      {
        result = DbTransaction != null
          ? BusinessBase<TLink>.Fetch(DbTransaction, EntityLinkHelper.CreateMasterInstance<TLink>(this, groupName))
          : BusinessBase<TLink>.Fetch(EntityLinkHelper.CreateMasterInstance<TLink>(this, groupName));
        SetLink(result, groupName);
      }
      return result;
    }

    /// <summary>
    /// 取关联业务对象
    /// groupName = null
    /// </summary>
    /// <param name="source">数据源</param>
    public TLink GetLink<TLinkSource, TLink>(TLinkSource source)
      where TLinkSource : BusinessListBase<TLinkSource, TLink>
      where TLink : BusinessBase<TLink>
    {
      return GetLink<TLinkSource, TLink>(source, null);
    }

    /// <summary>
    /// 取关联业务对象
    /// </summary>
    /// <param name="source">数据源</param>
    /// <param name="groupName">分组名</param>
    public TLink GetLink<TLinkSource, TLink>(TLinkSource source, string groupName)
      where TLinkSource : BusinessListBase<TLinkSource, TLink>
      where TLink : BusinessBase<TLink>
    {
      if (source == null)
        throw new ArgumentNullException("source");
      TLink result;
      if (!TryGetLink(groupName, out result))
        foreach (TLink item in source)
          if (IsLink(item, groupName))
          {
            result = item.Clone(false);
            SetLink(result, groupName);
            break;
          }
      return result;
    }

    /// <summary>
    /// 取关联业务对象
    /// groupName = null
    /// </summary>
    /// <param name="connection">数据库连接</param>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
    public TLink GetLink<TLink>(DbConnection connection)
      where TLink : BusinessBase<TLink>
    {
      return GetLink<TLink>(connection, null);
    }

    /// <summary>
    /// 取关联业务对象
    /// </summary>
    /// <param name="connection">数据库连接</param>
    /// <param name="groupName">分组名</param>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
    public TLink GetLink<TLink>(DbConnection connection, string groupName)
      where TLink : BusinessBase<TLink>
    {
      TLink result;
      if (!TryGetLink(groupName, out result))
      {
        result = BusinessBase<TLink>.Fetch(connection, EntityLinkHelper.CreateMasterInstance<TLink>(this, groupName));
        SetLink(result, groupName);
      }
      return result;
    }

    /// <summary>
    /// 取关联业务对象
    /// groupName = null
    /// </summary>
    /// <param name="transaction">数据库事务</param>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
    public TLink GetLink<TLink>(DbTransaction transaction)
      where TLink : BusinessBase<TLink>
    {
      return GetLink<TLink>(transaction, null);
    }

    /// <summary>
    /// 取关联业务对象
    /// </summary>
    /// <param name="transaction">数据库事务</param>
    /// <param name="groupName">分组名</param>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
    public TLink GetLink<TLink>(DbTransaction transaction, string groupName)
      where TLink : BusinessBase<TLink>
    {
      TLink result;
      if (!TryGetLink(groupName, out result))
      {
        result = BusinessBase<TLink>.Fetch(transaction, EntityLinkHelper.CreateMasterInstance<TLink>(this, groupName));
        SetLink(result, groupName);
      }
      return result;
    }

    #endregion

    #region Parent

    /// <summary>
    /// 设置父对象
    /// </summary>
    /// <param name="parent">父对象</param>
    protected override void SetParent(Csla.Core.IParent parent)
    {
      if (parent != null)
        SynchronizeEditLevel((IRefinedly)parent);
      base.SetParent(parent);
    }

    #endregion

    #region Detail

    /// <summary>
    /// 清除从业务对象Cache
    /// </summary>
    internal protected bool ClearDetailCache()
    {
      bool result = _details != null && _details.Count > 0;
      _details = null;
      _sortedDetails = null;
      return result;
    }

    /// <summary>
    /// 清除从业务对象Cache
    /// </summary>
    internal protected bool ClearDetailCache(string key)
    {
      bool result = false;
      if (_details != null && _details.Count > 0)
        if (_details.Remove(key))
          result = true;
      _sortedDetails = null;
      return result;
    }

    /// <summary>
    /// 清除从业务对象Cache
    /// </summary>
    internal protected bool ClearDetailCache(Type type)
    {
      bool result = false;
      if (_details != null && _details.Count > 0)
      {
        List<string> keys = new List<string>(_details.Count);
        foreach (KeyValuePair<string, IBusiness> kvp in _details)
          if (String.CompareOrdinal(kvp.Value.GetType().FullName, type.FullName) == 0
            || String.CompareOrdinal(Utilities.GetCoreType(kvp.Value.GetType()).FullName, type.FullName) == 0)
            keys.Add(kvp.Key);
        foreach (string s in keys)
          if (_details.Remove(s))
            result = true;
      }
      _sortedDetails = null;
      return result;
    }

    /// <summary>
    /// 清除从业务对象Cache
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
    protected bool ClearDetailCache<TDetailBusiness>()
      where TDetailBusiness : BusinessBase<TDetailBusiness>
    {
      return ClearDetailCache(typeof(TDetailBusiness));
    }

    /// <summary>
    /// 清除从业务对象Cache
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
    protected bool ClearDetailCache<TDetail, TDetailBusiness>()
      where TDetail : BusinessListBase<TDetail, TDetailBusiness>
      where TDetailBusiness : BusinessBase<TDetailBusiness>
    {
      return ClearDetailCache(typeof(TDetail));
    }

    private void DoSetDetail(string key, IBusiness detail)
    {
      //if (detail.IsReadOnly != IsReadOnly)
      //  throw new InvalidOperationException(
      //    String.Format("{0}不允许有Detail的{1}对象，需满足要么都只读(标记了{2})要么都不只读的条件.",
      //      this.GetType().FullName, detail.GetType().FullName, typeof(Phenix.Core.Mapping.ReadOnlyAttribute).FullName));

      if (_details == null)
        _details = new Dictionary<string, IBusiness>(StringComparer.Ordinal);
      _details[key] = detail;

      AddCacheType(detail);
    }

    private void DoSetDetail(string key, IBusiness detail, bool? cascadingDelete)
    {
      if (detail == null)
        return;

      if (detail.Criterions != null)
        detail.Criterions.SetLink(this, cascadingDelete);

      DoSetDetail(key, detail);
    }

    /// <summary>
    /// 设置从业务对象(组合关系)
    /// </summary>
    /// <param name="key">比对键值</param>
    /// <param name="detail">从业务对象</param>
    public void SetCompositionDetail(string key, IBusiness detail)
    {
      DoSetDetail(key, detail, true);
    }

    /// <summary>
    /// 设置从业务对象(聚合关系)
    /// </summary>
    /// <param name="key">比对键值</param>
    /// <param name="detail">从业务对象</param>
    public void SetAggregationDetail(string key, IBusiness detail)
    {
      DoSetDetail(key, detail, false);
    }

    private void SetDetail<TDetailBusiness>(TDetailBusiness detail, bool? cascadingDelete)
      where TDetailBusiness : BusinessBase<TDetailBusiness>
    {
      if (detail == null)
        return;

      ((Csla.Core.IEditableBusinessObject)detail).SetParent(this);
      if (detail.Criterions == null)
        detail.Criterions = new Criterions(typeof(TDetailBusiness), false, false);
      detail.Criterions.SetLink(this, cascadingDelete);

      DoSetDetail(detail.Criterions.ToString(), detail);
    }

    /// <summary>
    /// 设置从业务对象
    /// 从业务对象与本业务对象是一对一的关系
    /// </summary>
    /// <param name="detail">从业务对象</param>
    public void SetDetail<TDetailBusiness>(TDetailBusiness detail)
      where TDetailBusiness : BusinessBase<TDetailBusiness>
    {
      SetDetail<TDetailBusiness>(detail, null);
    }

    /// <summary>
    /// 设置从业务对象(组合关系)
    /// 从业务对象与本业务对象是一对一的关系
    /// </summary>
    /// <param name="detail">从业务对象</param>
    public void SetCompositionDetail<TDetailBusiness>(TDetailBusiness detail)
      where TDetailBusiness : BusinessBase<TDetailBusiness>
    {
      SetDetail<TDetailBusiness>(detail, true);
    }

    /// <summary>
    /// 设置从业务对象(聚合关系)
    /// 从业务对象与本业务对象是一对一的关系
    /// </summary>
    /// <param name="detail">从业务对象</param>
    public void SetAggregationDetail<TDetailBusiness>(TDetailBusiness detail)
      where TDetailBusiness : BusinessBase<TDetailBusiness>
    {
      SetDetail<TDetailBusiness>(detail, false);
    }

    private void SetDetail<TDetail, TDetailBusiness>(TDetail detail, bool? cascadingDelete)
      where TDetail : BusinessListBase<TDetail, TDetailBusiness>
      where TDetailBusiness : BusinessBase<TDetailBusiness>
    {
      if (detail == null)
        return;

      ((Csla.Core.IEditableCollection)detail).SetParent(this);
      if (detail.Criterions == null)
        detail.Criterions = new Criterions(typeof(TDetail), false, false);
      detail.Criterions.SetLink(this, cascadingDelete);

      DoSetDetail(detail.Criterions.ToString(), detail);
    }

    /// <summary>
    /// 设置从业务对象集合
    /// </summary>
    /// <param name="detail">从业务对象集合</param>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
    public void SetDetail<TDetail, TDetailBusiness>(TDetail detail)
      where TDetail : BusinessListBase<TDetail, TDetailBusiness>
      where TDetailBusiness : BusinessBase<TDetailBusiness>
    {
      SetDetail<TDetail, TDetailBusiness>(detail, null);
    }

    /// <summary>
    /// 设置从业务对象集合(组合关系)
    /// </summary>
    /// <param name="detail">从业务对象集合</param>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
    public void SetCompositionDetail<TDetail, TDetailBusiness>(TDetail detail)
      where TDetail : BusinessListBase<TDetail, TDetailBusiness>
      where TDetailBusiness : BusinessBase<TDetailBusiness>
    {
      SetDetail<TDetail, TDetailBusiness>(detail, true);
    }

    /// <summary>
    /// 设置从业务对象集合(聚合关系)
    /// </summary>
    /// <param name="detail">从业务对象集合</param>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
    public void SetAggregationDetail<TDetail, TDetailBusiness>(TDetail detail)
      where TDetail : BusinessListBase<TDetail, TDetailBusiness>
      where TDetailBusiness : BusinessBase<TDetailBusiness>
    {
      SetDetail<TDetail, TDetailBusiness>(detail, false);
    }

    /// <summary>
    /// 检索从业务对象
    /// </summary>
    /// <param name="key">比对键值</param>
    public IBusiness FindDetail(string key)
    {
      IBusiness result = null;
      if (_details != null && _details.Count > 0)
        _details.TryGetValue(key, out result);
      return result;
    }

    /// <summary>
    /// 检索出从业务对象队列
    /// </summary>
    public IList<TDetailBusiness> FindDetailBusinesses<TDetailBusiness>()
      where TDetailBusiness : BusinessBase<TDetailBusiness>
    {
      List<TDetailBusiness> result = new List<TDetailBusiness>();
      if (_details != null && _details.Count > 0)
      {
        Type type = typeof(TDetailBusiness);
        foreach (KeyValuePair<string, IBusiness> kvp in _details)
          if (String.CompareOrdinal(Utilities.GetCoreType(kvp.Value.GetType()).FullName, type.FullName) == 0)
            if (kvp.Value is IBusinessObject)
              result.Add((TDetailBusiness)kvp.Value);
            else
              foreach (TDetailBusiness item in (IBusinessCollection)kvp.Value)
                result.Add(item);
      }
      return result;
    }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
    private TDetailBusiness DoFindDetail<TDetailBusiness>(Criterions criterions, bool? cascadingDelete)
      where TDetailBusiness : BusinessBase<TDetailBusiness>
    {
      IBusiness result = null;
      if (_details != null && _details.Count > 0)
      {
        if (criterions == null)
          criterions = new Criterions(typeof(TDetailBusiness), false, false);
        criterions.SetLink(this, cascadingDelete);
        _details.TryGetValue(criterions.ToString(), out result);
      }
      return result as TDetailBusiness;
    }

    /// <summary>
    /// 检索从业务对象
    /// 从业务对象与本业务对象是一对一的关系
    /// </summary>
    /// <param name="criterions">条件集</param>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
    public TDetailBusiness FindDetail<TDetailBusiness>(Criterions criterions)
       where TDetailBusiness : BusinessBase<TDetailBusiness>
    {
      return DoFindDetail<TDetailBusiness>(criterions, null);
    }

    /// <summary>
    /// 检索从业务对象
    /// 从业务对象与本业务对象是一对一的关系
    /// </summary>
    [Obsolete("建议使用: FindCompositionDetail<TDetailBusiness>()", false)]
    public TDetailBusiness FindDetail<TDetailBusiness>()
      where TDetailBusiness : BusinessBase<TDetailBusiness>
    {
      return FindCompositionDetail<TDetailBusiness>();
    }

    /// <summary>
    /// 检索从业务对象(组合关系)
    /// 从业务对象与本业务对象是一对一的关系
    /// </summary>
    public TDetailBusiness FindCompositionDetail<TDetailBusiness>()
      where TDetailBusiness : BusinessBase<TDetailBusiness>
    {
      return DoFindDetail<TDetailBusiness>(null, true);
    }

    /// <summary>
    /// 检索从业务对象(聚合关系)
    /// 从业务对象与本业务对象是一对一的关系
    /// </summary>
    public TDetailBusiness FindAggregationDetail<TDetailBusiness>()
      where TDetailBusiness : BusinessBase<TDetailBusiness>
    {
      return DoFindDetail<TDetailBusiness>(null, false);
    }

    /// <summary>
    /// 检索从业务对象(组合关系)
    /// </summary>
    /// <param name="groupName">分组名</param>
    public TDetailBusiness FindCompositionDetail<TDetailBusiness>(string groupName)
      where TDetailBusiness : BusinessBase<TDetailBusiness>
    {
      return DoFindDetail<TDetailBusiness>(new Criterions(typeof(TDetailBusiness), this, groupName), true);
    }

    /// <summary>
    /// 检索从业务对象(聚合关系)
    /// </summary>
    /// <param name="groupName">分组名</param>
    public TDetailBusiness FindAggregationDetail<TDetailBusiness>(string groupName)
      where TDetailBusiness : BusinessBase<TDetailBusiness>
    {
      return DoFindDetail<TDetailBusiness>(new Criterions(typeof(TDetailBusiness), this, groupName), false);
    }

    /// <summary>
    /// 检索从业务对象(组合关系)
    /// 条件类的字段映射关系请用Phenix.Core.Mapping.CriteriaFieldAttribute标注
    /// </summary>
    /// <param name="criteria">从业务条件对象</param>
    public TDetailBusiness FindCompositionDetail<TDetailBusiness>(ICriteria criteria)
      where TDetailBusiness : BusinessBase<TDetailBusiness>
    {
      return DoFindDetail<TDetailBusiness>(new Criterions(typeof(TDetailBusiness), criteria, this, null), true);
    }

    /// <summary>
    /// 检索从业务对象(聚合关系)
    /// 条件类的字段映射关系请用Phenix.Core.Mapping.CriteriaFieldAttribute标注
    /// </summary>
    /// <param name="criteria">从业务条件对象</param>
    public TDetailBusiness FindAggregationDetail<TDetailBusiness>(ICriteria criteria)
      where TDetailBusiness : BusinessBase<TDetailBusiness>
    {
      return DoFindDetail<TDetailBusiness>(new Criterions(typeof(TDetailBusiness), criteria, this, null), false);
    }

    /// <summary>
    /// 检索从业务对象(组合关系)
    /// 条件类的字段映射关系请用Phenix.Core.Mapping.CriteriaFieldAttribute标注
    /// </summary>
    /// <param name="criteria">从业务条件对象</param>
    /// <param name="groupName">分组名</param>
    public TDetailBusiness FindCompositionDetail<TDetailBusiness>(ICriteria criteria, string groupName)
      where TDetailBusiness : BusinessBase<TDetailBusiness>
    {
      return DoFindDetail<TDetailBusiness>(new Criterions(typeof(TDetailBusiness), criteria, this, groupName), true);
    }

    /// <summary>
    /// 检索从业务对象(聚合关系)
    /// 条件类的字段映射关系请用Phenix.Core.Mapping.CriteriaFieldAttribute标注
    /// </summary>
    /// <param name="criteria">从业务条件对象</param>
    /// <param name="groupName">分组名</param>
    public TDetailBusiness FindAggregationDetail<TDetailBusiness>(ICriteria criteria, string groupName)
      where TDetailBusiness : BusinessBase<TDetailBusiness>
    {
      return DoFindDetail<TDetailBusiness>(new Criterions(typeof(TDetailBusiness), criteria, this, groupName), false);
    }

    /// <summary>
    /// 检索从业务对象(组合关系)
    /// </summary>
    /// <param name="criteriaExpression">从业务条件表达式</param>
    public TDetailBusiness FindCompositionDetail<TDetailBusiness>(CriteriaExpression criteriaExpression)
      where TDetailBusiness : BusinessBase<TDetailBusiness>
    {
      return DoFindDetail<TDetailBusiness>(new Criterions(typeof(TDetailBusiness), criteriaExpression, this, null), true);
    }

    /// <summary>
    /// 检索从业务对象(聚合关系)
    /// </summary>
    /// <param name="criteriaExpression">从业务条件表达式</param>
    public TDetailBusiness FindAggregationDetail<TDetailBusiness>(CriteriaExpression criteriaExpression)
      where TDetailBusiness : BusinessBase<TDetailBusiness>
    {
      return DoFindDetail<TDetailBusiness>(new Criterions(typeof(TDetailBusiness), criteriaExpression, this, null), false);
    }

    /// <summary>
    /// 检索从业务对象(组合关系)
    /// </summary>
    /// <param name="criteriaExpression">从业务条件表达式</param>
    /// <param name="groupName">分组名</param>
    public TDetailBusiness FindCompositionDetail<TDetailBusiness>(CriteriaExpression criteriaExpression, string groupName)
      where TDetailBusiness : BusinessBase<TDetailBusiness>
    {
      return DoFindDetail<TDetailBusiness>(new Criterions(typeof(TDetailBusiness), criteriaExpression, this, groupName), true);
    }

    /// <summary>
    /// 检索从业务对象(聚合关系)
    /// </summary>
    /// <param name="criteriaExpression">从业务条件表达式</param>
    /// <param name="groupName">分组名</param>
    public TDetailBusiness FindAggregationDetail<TDetailBusiness>(CriteriaExpression criteriaExpression, string groupName)
      where TDetailBusiness : BusinessBase<TDetailBusiness>
    {
      return DoFindDetail<TDetailBusiness>(new Criterions(typeof(TDetailBusiness), criteriaExpression, this, groupName), false);
    }

    /// <summary>
    /// 检索从业务对象(组合关系)
    /// </summary>
    /// <param name="criteriaExpression">从业务条件表达式</param>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures")]
    public TDetailBusiness FindCompositionDetail<TDetailBusiness>(Expression<Func<TDetailBusiness, bool>> criteriaExpression)
      where TDetailBusiness : BusinessBase<TDetailBusiness>
    {
      return FindCompositionDetail<TDetailBusiness>(CriteriaHelper.ToCriteriaExpression(criteriaExpression));
    }

    /// <summary>
    /// 检索从业务对象(聚合关系)
    /// </summary>
    /// <param name="criteriaExpression">从业务条件表达式</param>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures")]
    public TDetailBusiness FindAggregationDetail<TDetailBusiness>(Expression<Func<TDetailBusiness, bool>> criteriaExpression)
      where TDetailBusiness : BusinessBase<TDetailBusiness>
    {
      return FindAggregationDetail<TDetailBusiness>(CriteriaHelper.ToCriteriaExpression(criteriaExpression));
    }

    /// <summary>
    /// 检索从业务对象(组合关系)
    /// </summary>
    /// <param name="criteriaExpression">从业务条件表达式</param>
    /// <param name="groupName">分组名</param>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures")]
    public TDetailBusiness FindCompositionDetail<TDetailBusiness>(Expression<Func<TDetailBusiness, bool>> criteriaExpression, string groupName)
      where TDetailBusiness : BusinessBase<TDetailBusiness>
    {
      return FindCompositionDetail<TDetailBusiness>(CriteriaHelper.ToCriteriaExpression(criteriaExpression), groupName);
    }

    /// <summary>
    /// 检索从业务对象(聚合关系)
    /// </summary>
    /// <param name="criteriaExpression">从业务条件表达式</param>
    /// <param name="groupName">分组名</param>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures")]
    public TDetailBusiness FindAggregationDetail<TDetailBusiness>(Expression<Func<TDetailBusiness, bool>> criteriaExpression, string groupName)
      where TDetailBusiness : BusinessBase<TDetailBusiness>
    {
      return FindAggregationDetail<TDetailBusiness>(CriteriaHelper.ToCriteriaExpression(criteriaExpression), groupName);
    }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
    private TDetail DoFindDetail<TDetail, TDetailBusiness>(Criterions criterions, bool? cascadingDelete)
      where TDetail : BusinessListBase<TDetail, TDetailBusiness>
      where TDetailBusiness : BusinessBase<TDetailBusiness>
    {
      IBusiness result = null;
      if (_details != null && _details.Count > 0)
      {
        if (criterions == null)
          criterions = new Criterions(typeof(TDetail), false, false);
        criterions.SetLink(this, cascadingDelete);
        _details.TryGetValue(criterions.ToString(), out result);
      }
      return result as TDetail;
    }

    /// <summary>
    /// 检索从业务对象集合
    /// </summary>
    /// <param name="criterions">条件集</param>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
    public TDetail FindDetail<TDetail, TDetailBusiness>(Criterions criterions)
      where TDetail : BusinessListBase<TDetail, TDetailBusiness>
      where TDetailBusiness : BusinessBase<TDetailBusiness>
    {
      return DoFindDetail<TDetail, TDetailBusiness>(criterions, null);
    }

    /// <summary>
    /// 检索从业务对象集合
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
    [Obsolete("建议使用: FindCompositionDetail<TDetail, TDetailBusiness>()", false)]
    public TDetail FindDetail<TDetail, TDetailBusiness>()
      where TDetail : BusinessListBase<TDetail, TDetailBusiness>
      where TDetailBusiness : BusinessBase<TDetailBusiness>
    {
      return FindCompositionDetail<TDetail, TDetailBusiness>();
    }

    /// <summary>
    /// 检索从业务对象集合(组合关系)
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
    public TDetail FindCompositionDetail<TDetail, TDetailBusiness>()
      where TDetail : BusinessListBase<TDetail, TDetailBusiness>
      where TDetailBusiness : BusinessBase<TDetailBusiness>
    {
      return DoFindDetail<TDetail, TDetailBusiness>(null, true);
    }

    /// <summary>
    /// 检索从业务对象集合(聚合关系)
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
    public TDetail FindAggregationDetail<TDetail, TDetailBusiness>()
      where TDetail : BusinessListBase<TDetail, TDetailBusiness>
      where TDetailBusiness : BusinessBase<TDetailBusiness>
    {
      return DoFindDetail<TDetail, TDetailBusiness>(null, false);
    }

    /// <summary>
    /// 检索从业务对象集合(组合关系)
    /// </summary>
    /// <param name="groupName">分组名</param>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
    public TDetail FindCompositionDetail<TDetail, TDetailBusiness>(string groupName)
      where TDetail : BusinessListBase<TDetail, TDetailBusiness>
      where TDetailBusiness : BusinessBase<TDetailBusiness>
    {
      return DoFindDetail<TDetail, TDetailBusiness>(new Criterions(typeof(TDetail), this, groupName), true);
    }

    /// <summary>
    /// 检索从业务对象集合(聚合关系)
    /// </summary>
    /// <param name="groupName">分组名</param>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
    public TDetail FindAggregationDetail<TDetail, TDetailBusiness>(string groupName)
      where TDetail : BusinessListBase<TDetail, TDetailBusiness>
      where TDetailBusiness : BusinessBase<TDetailBusiness>
    {
      return DoFindDetail<TDetail, TDetailBusiness>(new Criterions(typeof(TDetail), this, groupName), false);
    }

    /// <summary>
    /// 检索从业务对象集合(组合关系)
    /// </summary>
    /// <param name="criteria">从业务条件对象</param>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
    public TDetail FindCompositionDetail<TDetail, TDetailBusiness>(ICriteria criteria)
      where TDetail : BusinessListBase<TDetail, TDetailBusiness>
      where TDetailBusiness : BusinessBase<TDetailBusiness>
    {
      return DoFindDetail<TDetail, TDetailBusiness>(new Criterions(typeof(TDetail), criteria, this, null), true);
    }

    /// <summary>
    /// 检索从业务对象集合(聚合关系)
    /// </summary>
    /// <param name="criteria">从业务条件对象</param>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
    public TDetail FindAggregationDetail<TDetail, TDetailBusiness>(ICriteria criteria)
      where TDetail : BusinessListBase<TDetail, TDetailBusiness>
      where TDetailBusiness : BusinessBase<TDetailBusiness>
    {
      return DoFindDetail<TDetail, TDetailBusiness>(new Criterions(typeof(TDetail), criteria, this, null), false);
    }

    /// <summary>
    /// 检索从业务对象集合(组合关系)
    /// </summary>
    /// <param name="criteria">从业务条件对象</param>
    /// <param name="groupName">分组名</param>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
    public TDetail FindCompositionDetail<TDetail, TDetailBusiness>(ICriteria criteria, string groupName)
      where TDetail : BusinessListBase<TDetail, TDetailBusiness>
      where TDetailBusiness : BusinessBase<TDetailBusiness>
    {
      return DoFindDetail<TDetail, TDetailBusiness>(new Criterions(typeof(TDetail), criteria, this, groupName), true);
    }

    /// <summary>
    /// 检索从业务对象集合(聚合关系)
    /// </summary>
    /// <param name="criteria">从业务条件对象</param>
    /// <param name="groupName">分组名</param>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
    public TDetail FindAggregationDetail<TDetail, TDetailBusiness>(ICriteria criteria, string groupName)
      where TDetail : BusinessListBase<TDetail, TDetailBusiness>
      where TDetailBusiness : BusinessBase<TDetailBusiness>
    {
      return DoFindDetail<TDetail, TDetailBusiness>(new Criterions(typeof(TDetail), criteria, this, groupName), false);
    }

    /// <summary>
    /// 检索从业务对象集合(组合关系)
    /// </summary>
    /// <param name="criteriaExpression">从业务条件表达式</param>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
    public TDetail FindCompositionDetail<TDetail, TDetailBusiness>(CriteriaExpression criteriaExpression)
      where TDetail : BusinessListBase<TDetail, TDetailBusiness>
      where TDetailBusiness : BusinessBase<TDetailBusiness>
    {
      return DoFindDetail<TDetail, TDetailBusiness>(new Criterions(typeof(TDetail), criteriaExpression, this, null), true);
    }

    /// <summary>
    /// 检索从业务对象集合(聚合关系)
    /// </summary>
    /// <param name="criteriaExpression">从业务条件表达式</param>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
    public TDetail FindAggregationDetail<TDetail, TDetailBusiness>(CriteriaExpression criteriaExpression)
      where TDetail : BusinessListBase<TDetail, TDetailBusiness>
      where TDetailBusiness : BusinessBase<TDetailBusiness>
    {
      return DoFindDetail<TDetail, TDetailBusiness>(new Criterions(typeof(TDetail), criteriaExpression, this, null), false);
    }

    /// <summary>
    /// 检索从业务对象集合(组合关系)
    /// </summary>
    /// <param name="criteriaExpression">从业务条件表达式</param>
    /// <param name="groupName">分组名</param>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
    public TDetail FindCompositionDetail<TDetail, TDetailBusiness>(CriteriaExpression criteriaExpression, string groupName)
      where TDetail : BusinessListBase<TDetail, TDetailBusiness>
      where TDetailBusiness : BusinessBase<TDetailBusiness>
    {
      return DoFindDetail<TDetail, TDetailBusiness>(new Criterions(typeof(TDetail), criteriaExpression, this, groupName), true);
    }

    /// <summary>
    /// 检索从业务对象集合(聚合关系)
    /// </summary>
    /// <param name="criteriaExpression">从业务条件表达式</param>
    /// <param name="groupName">分组名</param>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
    public TDetail FindAggregationDetail<TDetail, TDetailBusiness>(CriteriaExpression criteriaExpression, string groupName)
      where TDetail : BusinessListBase<TDetail, TDetailBusiness>
      where TDetailBusiness : BusinessBase<TDetailBusiness>
    {
      return DoFindDetail<TDetail, TDetailBusiness>(new Criterions(typeof(TDetail), criteriaExpression, this, groupName), false);
    }

    /// <summary>
    /// 检索从业务对象集合(组合关系)
    /// </summary>
    /// <param name="criteriaExpression">从业务条件表达式</param>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures")]
    public TDetail FindCompositionDetail<TDetail, TDetailBusiness>(Expression<Func<TDetailBusiness, bool>> criteriaExpression)
      where TDetail : BusinessListBase<TDetail, TDetailBusiness>
      where TDetailBusiness : BusinessBase<TDetailBusiness>
    {
      return FindCompositionDetail<TDetail, TDetailBusiness>(CriteriaHelper.ToCriteriaExpression(criteriaExpression));
    }

    /// <summary>
    /// 检索从业务对象集合(聚合关系)
    /// </summary>
    /// <param name="criteriaExpression">从业务条件表达式</param>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures")]
    public TDetail FindAggregationDetail<TDetail, TDetailBusiness>(Expression<Func<TDetailBusiness, bool>> criteriaExpression)
      where TDetail : BusinessListBase<TDetail, TDetailBusiness>
      where TDetailBusiness : BusinessBase<TDetailBusiness>
    {
      return FindAggregationDetail<TDetail, TDetailBusiness>(CriteriaHelper.ToCriteriaExpression(criteriaExpression));
    }

    /// <summary>
    /// 检索从业务对象集合(组合关系)
    /// </summary>
    /// <param name="criteriaExpression">从业务条件表达式</param>
    /// <param name="groupName">分组名</param>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures")]
    public TDetail FindCompositionDetail<TDetail, TDetailBusiness>(Expression<Func<TDetailBusiness, bool>> criteriaExpression, string groupName)
      where TDetail : BusinessListBase<TDetail, TDetailBusiness>
      where TDetailBusiness : BusinessBase<TDetailBusiness>
    {
      return FindCompositionDetail<TDetail, TDetailBusiness>(CriteriaHelper.ToCriteriaExpression(criteriaExpression), groupName);
    }

    /// <summary>
    /// 检索从业务对象集合(聚合关系)
    /// </summary>
    /// <param name="criteriaExpression">从业务条件表达式</param>
    /// <param name="groupName">分组名</param>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures")]
    public TDetail FindAggregationDetail<TDetail, TDetailBusiness>(Expression<Func<TDetailBusiness, bool>> criteriaExpression, string groupName)
      where TDetail : BusinessListBase<TDetail, TDetailBusiness>
      where TDetailBusiness : BusinessBase<TDetailBusiness>
    {
      return FindAggregationDetail<TDetail, TDetailBusiness>(CriteriaHelper.ToCriteriaExpression(criteriaExpression), groupName);
    }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
    private TDetailBusiness DoGetDetail<TDetailBusiness>(Criterions criterions, bool? cascadingDelete)
      where TDetailBusiness : BusinessBase<TDetailBusiness>
    {
      if (criterions == null)
        criterions = new Criterions(typeof(TDetailBusiness), false, false);
      criterions.SetLink(this, cascadingDelete);

      TDetailBusiness result = DoFindDetail<TDetailBusiness>(criterions, cascadingDelete);
      if (result != null)
        return result;

      if (IsNew)
        return BusinessBase<TDetailBusiness>.New(criterions);
      else if (DbTransaction != null)
        return BusinessBase<TDetailBusiness>.Fetch(DbTransaction, criterions);
      else
        return BusinessBase<TDetailBusiness>.Fetch(criterions);
    }

    /// <summary>
    /// 取从业务对象
    /// 从业务对象与本业务对象是一对一的关系
    /// </summary>
    /// <param name="criterions">条件集</param>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
    public TDetailBusiness GetDetail<TDetailBusiness>(Criterions criterions)
      where TDetailBusiness : BusinessBase<TDetailBusiness>
    {
      return DoGetDetail<TDetailBusiness>(criterions, null);
    }

    /// <summary>
    /// 取从业务对象
    /// 从业务对象与本业务对象是一对一的关系
    /// </summary>
    /// <param name="orderByInfos">数据排列顺序队列</param>
    [Obsolete("建议使用: GetCompositionDetail<TDetailBusiness>()", false)]
    public TDetailBusiness GetDetail<TDetailBusiness>(params OrderByInfo[] orderByInfos)
      where TDetailBusiness : BusinessBase<TDetailBusiness>
    {
      return GetCompositionDetail<TDetailBusiness>(orderByInfos);
    }

    /// <summary>
    /// 取从业务对象(组合关系)
    /// 从业务对象与本业务对象是一对一的关系
    /// </summary>
    /// <param name="orderByInfos">数据排列顺序队列</param>
    public TDetailBusiness GetCompositionDetail<TDetailBusiness>(params OrderByInfo[] orderByInfos)
      where TDetailBusiness : BusinessBase<TDetailBusiness>
    {
      return DoGetDetail<TDetailBusiness>(new Criterions(typeof(TDetailBusiness), this, null, orderByInfos), true);
    }

    /// <summary>
    /// 取从业务对象(聚合关系)
    /// 从业务对象与本业务对象是一对一的关系
    /// </summary>
    /// <param name="orderByInfos">数据排列顺序队列</param>
    public TDetailBusiness GetAggregationDetail<TDetailBusiness>(params OrderByInfo[] orderByInfos)
      where TDetailBusiness : BusinessBase<TDetailBusiness>
    {
      return DoGetDetail<TDetailBusiness>(new Criterions(typeof(TDetailBusiness), this, null, orderByInfos), false);
    }

    /// <summary>
    /// 取从业务对象(组合关系)
    /// 从业务对象与本业务对象是一对一的关系
    /// </summary>
    /// <param name="groupName">分组名</param>
    /// <param name="orderByInfos">数据排列顺序队列</param>
    public TDetailBusiness GetCompositionDetail<TDetailBusiness>(string groupName, params OrderByInfo[] orderByInfos)
      where TDetailBusiness : BusinessBase<TDetailBusiness>
    {
      return DoGetDetail<TDetailBusiness>(new Criterions(typeof(TDetailBusiness), this, groupName, orderByInfos), true);
    }

    /// <summary>
    /// 取从业务对象(聚合关系)
    /// 从业务对象与本业务对象是一对一的关系
    /// </summary>
    /// <param name="groupName">分组名</param>
    /// <param name="orderByInfos">数据排列顺序队列</param>
    public TDetailBusiness GetAggregationDetail<TDetailBusiness>(string groupName, params OrderByInfo[] orderByInfos)
      where TDetailBusiness : BusinessBase<TDetailBusiness>
    {
      return DoGetDetail<TDetailBusiness>(new Criterions(typeof(TDetailBusiness), this, groupName, orderByInfos), false);
    }

    /// <summary>
    /// 取从业务对象(组合关系)
    /// 条件类的字段映射关系请用Phenix.Core.Mapping.CriteriaFieldAttribute标注
    /// </summary>
    /// <param name="criteria">从业务条件对象</param>
    /// <param name="orderByInfos">数据排列顺序队列</param>
    public TDetailBusiness GetCompositionDetail<TDetailBusiness>(ICriteria criteria, params OrderByInfo[] orderByInfos)
      where TDetailBusiness : BusinessBase<TDetailBusiness>
    {
      return DoGetDetail<TDetailBusiness>(new Criterions(typeof(TDetailBusiness), criteria, this, null, orderByInfos), true);
    }

    /// <summary>
    /// 取从业务对象(聚合关系)
    /// 条件类的字段映射关系请用Phenix.Core.Mapping.CriteriaFieldAttribute标注
    /// </summary>
    /// <param name="criteria">从业务条件对象</param>
    /// <param name="orderByInfos">数据排列顺序队列</param>
    public TDetailBusiness GetAggregationDetail<TDetailBusiness>(ICriteria criteria, params OrderByInfo[] orderByInfos)
      where TDetailBusiness : BusinessBase<TDetailBusiness>
    {
      return DoGetDetail<TDetailBusiness>(new Criterions(typeof(TDetailBusiness), criteria, this, null, orderByInfos), false);
    }

    /// <summary>
    /// 取从业务对象(组合关系)
    /// 条件类的字段映射关系请用Phenix.Core.Mapping.CriteriaFieldAttribute标注
    /// </summary>
    /// <param name="criteria">从业务条件对象</param>
    /// <param name="groupName">分组名</param>
    /// <param name="orderByInfos">数据排列顺序队列</param>
    public TDetailBusiness GetCompositionDetail<TDetailBusiness>(ICriteria criteria, string groupName, params OrderByInfo[] orderByInfos)
      where TDetailBusiness : BusinessBase<TDetailBusiness>
    {
      return DoGetDetail<TDetailBusiness>(new Criterions(typeof(TDetailBusiness), criteria, this, groupName, orderByInfos), true);
    }

    /// <summary>
    /// 取从业务对象(聚合关系)
    /// 条件类的字段映射关系请用Phenix.Core.Mapping.CriteriaFieldAttribute标注
    /// </summary>
    /// <param name="criteria">从业务条件对象</param>
    /// <param name="groupName">分组名</param>
    /// <param name="orderByInfos">数据排列顺序队列</param>
    public TDetailBusiness GetAggregationDetail<TDetailBusiness>(ICriteria criteria, string groupName, params OrderByInfo[] orderByInfos)
      where TDetailBusiness : BusinessBase<TDetailBusiness>
    {
      return DoGetDetail<TDetailBusiness>(new Criterions(typeof(TDetailBusiness), criteria, this, groupName, orderByInfos), false);
    }

    /// <summary>
    /// 取从业务对象(组合关系)
    /// </summary>
    /// <param name="criteriaExpression">从业务条件表达式</param>
    /// <param name="orderByInfos">数据排列顺序队列</param>
    public TDetailBusiness GetCompositionDetail<TDetailBusiness>(CriteriaExpression criteriaExpression, params OrderByInfo[] orderByInfos)
      where TDetailBusiness : BusinessBase<TDetailBusiness>
    {
      return DoGetDetail<TDetailBusiness>(new Criterions(typeof(TDetailBusiness), criteriaExpression, this, null, orderByInfos), true);
    }

    /// <summary>
    /// 取从业务对象(聚合关系)
    /// </summary>
    /// <param name="criteriaExpression">从业务条件表达式</param>
    /// <param name="orderByInfos">数据排列顺序队列</param>
    public TDetailBusiness GetAggregationDetail<TDetailBusiness>(CriteriaExpression criteriaExpression, params OrderByInfo[] orderByInfos)
      where TDetailBusiness : BusinessBase<TDetailBusiness>
    {
      return DoGetDetail<TDetailBusiness>(new Criterions(typeof(TDetailBusiness), criteriaExpression, this, null, orderByInfos), false);
    }

    /// <summary>
    /// 取从业务对象(组合关系)
    /// </summary>
    /// <param name="criteriaExpression">从业务条件表达式</param>
    /// <param name="groupName">分组名</param>
    /// <param name="orderByInfos">数据排列顺序队列</param>
    public TDetailBusiness GetCompositionDetail<TDetailBusiness>(CriteriaExpression criteriaExpression, string groupName, params OrderByInfo[] orderByInfos)
      where TDetailBusiness : BusinessBase<TDetailBusiness>
    {
      return DoGetDetail<TDetailBusiness>(new Criterions(typeof(TDetailBusiness), criteriaExpression, this, groupName, orderByInfos), true);
    }

    /// <summary>
    /// 取从业务对象(聚合关系)
    /// </summary>
    /// <param name="criteriaExpression">从业务条件表达式</param>
    /// <param name="groupName">分组名</param>
    /// <param name="orderByInfos">数据排列顺序队列</param>
    public TDetailBusiness GetAggregationDetail<TDetailBusiness>(CriteriaExpression criteriaExpression, string groupName, params OrderByInfo[] orderByInfos)
      where TDetailBusiness : BusinessBase<TDetailBusiness>
    {
      return DoGetDetail<TDetailBusiness>(new Criterions(typeof(TDetailBusiness), criteriaExpression, this, groupName, orderByInfos), false);
    }

    /// <summary>
    /// 取从业务对象(组合关系)
    /// </summary>
    /// <param name="criteriaExpression">从业务条件表达式</param>
    /// <param name="orderByInfos">数据排列顺序队列</param>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures")]
    public TDetailBusiness GetCompositionDetail<TDetailBusiness>(Expression<Func<TDetailBusiness, bool>> criteriaExpression, params OrderByInfo[] orderByInfos)
      where TDetailBusiness : BusinessBase<TDetailBusiness>
    {
      return GetCompositionDetail<TDetailBusiness>(CriteriaHelper.ToCriteriaExpression(criteriaExpression), orderByInfos);
    }

    /// <summary>
    /// 取从业务对象(聚合关系)
    /// </summary>
    /// <param name="criteriaExpression">从业务条件表达式</param>
    /// <param name="orderByInfos">数据排列顺序队列</param>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures")]
    public TDetailBusiness GetAggregationDetail<TDetailBusiness>(Expression<Func<TDetailBusiness, bool>> criteriaExpression, params OrderByInfo[] orderByInfos)
      where TDetailBusiness : BusinessBase<TDetailBusiness>
    {
      return GetAggregationDetail<TDetailBusiness>(CriteriaHelper.ToCriteriaExpression(criteriaExpression), orderByInfos);
    }

    /// <summary>
    /// 取从业务对象(组合关系)
    /// </summary>
    /// <param name="criteriaExpression">从业务条件表达式</param>
    /// <param name="groupName">分组名</param>
    /// <param name="orderByInfos">数据排列顺序队列</param>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures")]
    public TDetailBusiness GetCompositionDetail<TDetailBusiness>(Expression<Func<TDetailBusiness, bool>> criteriaExpression, string groupName, params OrderByInfo[] orderByInfos)
      where TDetailBusiness : BusinessBase<TDetailBusiness>
    {
      return GetCompositionDetail<TDetailBusiness>(CriteriaHelper.ToCriteriaExpression(criteriaExpression), groupName, orderByInfos);
    }

    /// <summary>
    /// 取从业务对象(聚合关系)
    /// </summary>
    /// <param name="criteriaExpression">从业务条件表达式</param>
    /// <param name="groupName">分组名</param>
    /// <param name="orderByInfos">数据排列顺序队列</param>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures")]
    public TDetailBusiness GetAggregationDetail<TDetailBusiness>(Expression<Func<TDetailBusiness, bool>> criteriaExpression, string groupName, params OrderByInfo[] orderByInfos)
      where TDetailBusiness : BusinessBase<TDetailBusiness>
    {
      return GetAggregationDetail<TDetailBusiness>(CriteriaHelper.ToCriteriaExpression(criteriaExpression), groupName, orderByInfos);
    }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
    private TDetailBusiness DoGetDetail<TDetailBusiness>(DbConnection connection, Criterions criterions, bool? cascadingDelete)
      where TDetailBusiness : BusinessBase<TDetailBusiness>
    {
      if (criterions == null)
        criterions = new Criterions(typeof(TDetailBusiness), false, false);
      criterions.SetLink(this, cascadingDelete);

      //TDetailBusiness result = DoFindDetail<TDetailBusiness>(criterions, cascadingDelete);
      //if (result != null)
      //  return result;

      //return IsNew ?
      //  BusinessBase<TDetailBusiness>.New(criterions) :
      return BusinessBase<TDetailBusiness>.Fetch(connection, criterions);
    }

    /// <summary>
    /// 取从业务对象
    /// 从业务对象与本业务对象是一对一的关系
    /// </summary>
    /// <param name="connection">数据库连接</param>
    /// <param name="criterions">条件集</param>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
    public TDetailBusiness GetDetail<TDetailBusiness>(DbConnection connection, Criterions criterions)
      where TDetailBusiness : BusinessBase<TDetailBusiness>
    {
      return DoGetDetail<TDetailBusiness>(connection, criterions, null);
    }

    /// <summary>
    /// 取从业务对象
    /// 从业务对象与本业务对象是一对一的关系
    /// </summary>
    /// <param name="connection">数据库连接</param>
    /// <param name="orderByInfos">数据排列顺序队列</param>
    [Obsolete("建议使用: GetCompositionDetail<TDetailBusiness>(DbConnection connection)", false)]
    public TDetailBusiness GetDetail<TDetailBusiness>(DbConnection connection, params OrderByInfo[] orderByInfos)
      where TDetailBusiness : BusinessBase<TDetailBusiness>
    {
      return GetCompositionDetail<TDetailBusiness>(connection, orderByInfos);
    }

    /// <summary>
    /// 取从业务对象(组合关系)
    /// 从业务对象与本业务对象是一对一的关系
    /// </summary>
    /// <param name="connection">数据库连接</param>
    /// <param name="orderByInfos">数据排列顺序队列</param>
    public TDetailBusiness GetCompositionDetail<TDetailBusiness>(DbConnection connection, params OrderByInfo[] orderByInfos)
      where TDetailBusiness : BusinessBase<TDetailBusiness>
    {
      return DoGetDetail<TDetailBusiness>(connection, new Criterions(typeof(TDetailBusiness), this, null, orderByInfos), true);
    }

    /// <summary>
    /// 取从业务对象(聚合关系)
    /// 从业务对象与本业务对象是一对一的关系
    /// </summary>
    /// <param name="connection">数据库连接</param>
    /// <param name="orderByInfos">数据排列顺序队列</param>
    public TDetailBusiness GetAggregationDetail<TDetailBusiness>(DbConnection connection, params OrderByInfo[] orderByInfos)
      where TDetailBusiness : BusinessBase<TDetailBusiness>
    {
      return DoGetDetail<TDetailBusiness>(connection, new Criterions(typeof(TDetailBusiness), this, null, orderByInfos), false);
    }

    /// <summary>
    /// 取从业务对象(组合关系)
    /// 从业务对象与本业务对象是一对一的关系
    /// </summary>
    /// <param name="connection">数据库连接</param>
    /// <param name="groupName">分组名</param>
    /// <param name="orderByInfos">数据排列顺序队列</param>
    public TDetailBusiness GetCompositionDetail<TDetailBusiness>(DbConnection connection, string groupName, params OrderByInfo[] orderByInfos)
      where TDetailBusiness : BusinessBase<TDetailBusiness>
    {
      return DoGetDetail<TDetailBusiness>(connection, new Criterions(typeof(TDetailBusiness), this, groupName, orderByInfos), true);
    }

    /// <summary>
    /// 取从业务对象(聚合关系)
    /// 从业务对象与本业务对象是一对一的关系
    /// </summary>
    /// <param name="connection">数据库连接</param>
    /// <param name="groupName">分组名</param>
    /// <param name="orderByInfos">数据排列顺序队列</param>
    public TDetailBusiness GetAggregationDetail<TDetailBusiness>(DbConnection connection, string groupName, params OrderByInfo[] orderByInfos)
      where TDetailBusiness : BusinessBase<TDetailBusiness>
    {
      return DoGetDetail<TDetailBusiness>(connection, new Criterions(typeof(TDetailBusiness), this, groupName, orderByInfos), false);
    }

    /// <summary>
    /// 取从业务对象(组合关系)
    /// 条件类的字段映射关系请用Phenix.Core.Mapping.CriteriaFieldAttribute标注
    /// </summary>
    /// <param name="connection">数据库连接</param>
    /// <param name="criteria">从业务条件对象</param>
    /// <param name="orderByInfos">数据排列顺序队列</param>
    public TDetailBusiness GetCompositionDetail<TDetailBusiness>(DbConnection connection, ICriteria criteria, params OrderByInfo[] orderByInfos)
      where TDetailBusiness : BusinessBase<TDetailBusiness>
    {
      return DoGetDetail<TDetailBusiness>(connection, new Criterions(typeof(TDetailBusiness), criteria, this, null, orderByInfos), true);
    }

    /// <summary>
    /// 取从业务对象(聚合关系)
    /// 条件类的字段映射关系请用Phenix.Core.Mapping.CriteriaFieldAttribute标注
    /// </summary>
    /// <param name="connection">数据库连接</param>
    /// <param name="criteria">从业务条件对象</param>
    /// <param name="orderByInfos">数据排列顺序队列</param>
    public TDetailBusiness GetAggregationDetail<TDetailBusiness>(DbConnection connection, ICriteria criteria, params OrderByInfo[] orderByInfos)
      where TDetailBusiness : BusinessBase<TDetailBusiness>
    {
      return DoGetDetail<TDetailBusiness>(connection, new Criterions(typeof(TDetailBusiness), criteria, this, null, orderByInfos), false);
    }

    /// <summary>
    /// 取从业务对象(组合关系)
    /// 条件类的字段映射关系请用Phenix.Core.Mapping.CriteriaFieldAttribute标注
    /// </summary>
    /// <param name="connection">数据库连接</param>
    /// <param name="criteria">从业务条件对象</param>
    /// <param name="groupName">分组名</param>
    /// <param name="orderByInfos">数据排列顺序队列</param>
    public TDetailBusiness GetCompositionDetail<TDetailBusiness>(DbConnection connection, ICriteria criteria, string groupName, params OrderByInfo[] orderByInfos)
      where TDetailBusiness : BusinessBase<TDetailBusiness>
    {
      return DoGetDetail<TDetailBusiness>(connection, new Criterions(typeof(TDetailBusiness), criteria, this, groupName, orderByInfos), true);
    }

    /// <summary>
    /// 取从业务对象(聚合关系)
    /// 条件类的字段映射关系请用Phenix.Core.Mapping.CriteriaFieldAttribute标注
    /// </summary>
    /// <param name="connection">数据库连接</param>
    /// <param name="criteria">从业务条件对象</param>
    /// <param name="groupName">分组名</param>
    /// <param name="orderByInfos">数据排列顺序队列</param>
    public TDetailBusiness GetAggregationDetail<TDetailBusiness>(DbConnection connection, ICriteria criteria, string groupName, params OrderByInfo[] orderByInfos)
      where TDetailBusiness : BusinessBase<TDetailBusiness>
    {
      return DoGetDetail<TDetailBusiness>(connection, new Criterions(typeof(TDetailBusiness), criteria, this, groupName, orderByInfos), false);
    }

    /// <summary>
    /// 取从业务对象(组合关系)
    /// </summary>
    /// <param name="connection">数据库连接</param>
    /// <param name="criteriaExpression">从业务条件表达式</param>
    /// <param name="orderByInfos">数据排列顺序队列</param>
    public TDetailBusiness GetCompositionDetail<TDetailBusiness>(DbConnection connection, CriteriaExpression criteriaExpression, params OrderByInfo[] orderByInfos)
      where TDetailBusiness : BusinessBase<TDetailBusiness>
    {
      return DoGetDetail<TDetailBusiness>(connection, new Criterions(typeof(TDetailBusiness), criteriaExpression, this, null, orderByInfos), true);
    }

    /// <summary>
    /// 取从业务对象(聚合关系)
    /// </summary>
    /// <param name="connection">数据库连接</param>
    /// <param name="criteriaExpression">从业务条件表达式</param>
    /// <param name="orderByInfos">数据排列顺序队列</param>
    public TDetailBusiness GetAggregationDetail<TDetailBusiness>(DbConnection connection, CriteriaExpression criteriaExpression, params OrderByInfo[] orderByInfos)
      where TDetailBusiness : BusinessBase<TDetailBusiness>
    {
      return DoGetDetail<TDetailBusiness>(connection, new Criterions(typeof(TDetailBusiness), criteriaExpression, this, null, orderByInfos), false);
    }

    /// <summary>
    /// 取从业务对象(组合关系)
    /// </summary>
    /// <param name="connection">数据库连接</param>
    /// <param name="criteriaExpression">从业务条件表达式</param>
    /// <param name="groupName">分组名</param>
    /// <param name="orderByInfos">数据排列顺序队列</param>
    public TDetailBusiness GetCompositionDetail<TDetailBusiness>(DbConnection connection, CriteriaExpression criteriaExpression, string groupName, params OrderByInfo[] orderByInfos)
      where TDetailBusiness : BusinessBase<TDetailBusiness>
    {
      return DoGetDetail<TDetailBusiness>(connection, new Criterions(typeof(TDetailBusiness), criteriaExpression, this, groupName, orderByInfos), true);
    }

    /// <summary>
    /// 取从业务对象(聚合关系)
    /// </summary>
    /// <param name="connection">数据库连接</param>
    /// <param name="criteriaExpression">从业务条件表达式</param>
    /// <param name="groupName">分组名</param>
    /// <param name="orderByInfos">数据排列顺序队列</param>
    public TDetailBusiness GetAggregationDetail<TDetailBusiness>(DbConnection connection, CriteriaExpression criteriaExpression, string groupName, params OrderByInfo[] orderByInfos)
      where TDetailBusiness : BusinessBase<TDetailBusiness>
    {
      return DoGetDetail<TDetailBusiness>(connection, new Criterions(typeof(TDetailBusiness), criteriaExpression, this, groupName, orderByInfos), false);
    }

    /// <summary>
    /// 取从业务对象(组合关系)
    /// </summary>
    /// <param name="connection">数据库连接</param>
    /// <param name="criteriaExpression">从业务条件表达式</param>
    /// <param name="orderByInfos">数据排列顺序队列</param>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures")]
    public TDetailBusiness GetCompositionDetail<TDetailBusiness>(DbConnection connection, Expression<Func<TDetailBusiness, bool>> criteriaExpression, params OrderByInfo[] orderByInfos)
      where TDetailBusiness : BusinessBase<TDetailBusiness>
    {
      return GetCompositionDetail<TDetailBusiness>(connection, CriteriaHelper.ToCriteriaExpression(criteriaExpression), orderByInfos);
    }

    /// <summary>
    /// 取从业务对象(聚合关系)
    /// </summary>
    /// <param name="connection">数据库连接</param>
    /// <param name="criteriaExpression">从业务条件表达式</param>
    /// <param name="orderByInfos">数据排列顺序队列</param>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures")]
    public TDetailBusiness GetAggregationDetail<TDetailBusiness>(DbConnection connection, Expression<Func<TDetailBusiness, bool>> criteriaExpression, params OrderByInfo[] orderByInfos)
      where TDetailBusiness : BusinessBase<TDetailBusiness>
    {
      return GetAggregationDetail<TDetailBusiness>(connection, CriteriaHelper.ToCriteriaExpression(criteriaExpression), orderByInfos);
    }

    /// <summary>
    /// 取从业务对象(组合关系)
    /// </summary>
    /// <param name="connection">数据库连接</param>
    /// <param name="criteriaExpression">从业务条件表达式</param>
    /// <param name="groupName">分组名</param>
    /// <param name="orderByInfos">数据排列顺序队列</param>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures")]
    public TDetailBusiness GetCompositionDetail<TDetailBusiness>(DbConnection connection, Expression<Func<TDetailBusiness, bool>> criteriaExpression, string groupName, params OrderByInfo[] orderByInfos)
      where TDetailBusiness : BusinessBase<TDetailBusiness>
    {
      return GetCompositionDetail<TDetailBusiness>(connection, CriteriaHelper.ToCriteriaExpression(criteriaExpression), groupName, orderByInfos);
    }

    /// <summary>
    /// 取从业务对象(聚合关系)
    /// </summary>
    /// <param name="connection">数据库连接</param>
    /// <param name="criteriaExpression">从业务条件表达式</param>
    /// <param name="groupName">分组名</param>
    /// <param name="orderByInfos">数据排列顺序队列</param>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures")]
    public TDetailBusiness GetAggregationDetail<TDetailBusiness>(DbConnection connection, Expression<Func<TDetailBusiness, bool>> criteriaExpression, string groupName, params OrderByInfo[] orderByInfos)
      where TDetailBusiness : BusinessBase<TDetailBusiness>
    {
      return GetAggregationDetail<TDetailBusiness>(connection, CriteriaHelper.ToCriteriaExpression(criteriaExpression), groupName, orderByInfos);
    }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
    private TDetailBusiness DoGetDetail<TDetailBusiness>(DbTransaction transaction, Criterions criterions, bool? cascadingDelete)
      where TDetailBusiness : BusinessBase<TDetailBusiness>
    {
      if (criterions == null)
        criterions = new Criterions(typeof(TDetailBusiness), false, false);
      criterions.SetLink(this, cascadingDelete);

      //TDetailBusiness result = DoFindDetail<TDetailBusiness>(criterions, cascadingDelete);
      //if (result != null)
      //  return result;

      //return IsNew ?
      //  BusinessBase<TDetailBusiness>.New(criterions) :
      return BusinessBase<TDetailBusiness>.Fetch(transaction, criterions);
    }

    /// <summary>
    /// 取从业务对象
    /// 从业务对象与本业务对象是一对一的关系
    /// </summary>
    /// <param name="transaction">数据库事务</param>
    /// <param name="criterions">条件集</param>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
    public TDetailBusiness GetDetail<TDetailBusiness>(DbTransaction transaction, Criterions criterions)
      where TDetailBusiness : BusinessBase<TDetailBusiness>
    {
      return DoGetDetail<TDetailBusiness>(transaction, criterions, null);
    }

    /// <summary>
    /// 取从业务对象
    /// 从业务对象与本业务对象是一对一的关系
    /// </summary>
    /// <param name="transaction">数据库事务</param>
    /// <param name="orderByInfos">数据排列顺序队列</param>
    [Obsolete("建议使用: GetCompositionDetail<TDetailBusiness>(DbTransaction transaction)", false)]
    public TDetailBusiness GetDetail<TDetailBusiness>(DbTransaction transaction, params OrderByInfo[] orderByInfos)
      where TDetailBusiness : BusinessBase<TDetailBusiness>
    {
      return GetCompositionDetail<TDetailBusiness>(transaction, orderByInfos);
    }

    /// <summary>
    /// 取从业务对象(组合关系)
    /// 从业务对象与本业务对象是一对一的关系
    /// </summary>
    /// <param name="transaction">数据库事务</param>
    /// <param name="orderByInfos">数据排列顺序队列</param>
    public TDetailBusiness GetCompositionDetail<TDetailBusiness>(DbTransaction transaction, params OrderByInfo[] orderByInfos)
      where TDetailBusiness : BusinessBase<TDetailBusiness>
    {
      return DoGetDetail<TDetailBusiness>(transaction, new Criterions(typeof(TDetailBusiness), this, null, orderByInfos), true);
    }

    /// <summary>
    /// 取从业务对象(聚合关系)
    /// 从业务对象与本业务对象是一对一的关系
    /// </summary>
    /// <param name="transaction">数据库事务</param>
    /// <param name="orderByInfos">数据排列顺序队列</param>
    public TDetailBusiness GetAggregationDetail<TDetailBusiness>(DbTransaction transaction, params OrderByInfo[] orderByInfos)
      where TDetailBusiness : BusinessBase<TDetailBusiness>
    {
      return DoGetDetail<TDetailBusiness>(transaction, new Criterions(typeof(TDetailBusiness), this, null, orderByInfos), false);
    }

    /// <summary>
    /// 取从业务对象(组合关系)
    /// 从业务对象与本业务对象是一对一的关系
    /// </summary>
    /// <param name="transaction">数据库事务</param>
    /// <param name="groupName">分组名</param>
    /// <param name="orderByInfos">数据排列顺序队列</param>
    public TDetailBusiness GetCompositionDetail<TDetailBusiness>(DbTransaction transaction, string groupName, params OrderByInfo[] orderByInfos)
      where TDetailBusiness : BusinessBase<TDetailBusiness>
    {
      return DoGetDetail<TDetailBusiness>(transaction, new Criterions(typeof(TDetailBusiness), this, groupName, orderByInfos), true);
    }

    /// <summary>
    /// 取从业务对象(聚合关系)
    /// 从业务对象与本业务对象是一对一的关系
    /// </summary>
    /// <param name="transaction">数据库事务</param>
    /// <param name="groupName">分组名</param>
    /// <param name="orderByInfos">数据排列顺序队列</param>
    public TDetailBusiness GetAggregationDetail<TDetailBusiness>(DbTransaction transaction, string groupName, params OrderByInfo[] orderByInfos)
      where TDetailBusiness : BusinessBase<TDetailBusiness>
    {
      return DoGetDetail<TDetailBusiness>(transaction, new Criterions(typeof(TDetailBusiness), this, groupName, orderByInfos), false);
    }

    /// <summary>
    /// 取从业务对象(组合关系)
    /// 条件类的字段映射关系请用Phenix.Core.Mapping.CriteriaFieldAttribute标注
    /// </summary>
    /// <param name="transaction">数据库事务</param>
    /// <param name="criteria">从业务条件对象</param>
    /// <param name="orderByInfos">数据排列顺序队列</param>
    public TDetailBusiness GetCompositionDetail<TDetailBusiness>(DbTransaction transaction, ICriteria criteria, params OrderByInfo[] orderByInfos)
      where TDetailBusiness : BusinessBase<TDetailBusiness>
    {
      return DoGetDetail<TDetailBusiness>(transaction, new Criterions(typeof(TDetailBusiness), criteria, this, null, orderByInfos), true);
    }

    /// <summary>
    /// 取从业务对象(聚合关系)
    /// 条件类的字段映射关系请用Phenix.Core.Mapping.CriteriaFieldAttribute标注
    /// </summary>
    /// <param name="transaction">数据库事务</param>
    /// <param name="criteria">从业务条件对象</param>
    /// <param name="orderByInfos">数据排列顺序队列</param>
    public TDetailBusiness GetAggregationDetail<TDetailBusiness>(DbTransaction transaction, ICriteria criteria, params OrderByInfo[] orderByInfos)
      where TDetailBusiness : BusinessBase<TDetailBusiness>
    {
      return DoGetDetail<TDetailBusiness>(transaction, new Criterions(typeof(TDetailBusiness), criteria, this, null, orderByInfos), false);
    }

    /// <summary>
    /// 取从业务对象(组合关系)
    /// 条件类的字段映射关系请用Phenix.Core.Mapping.CriteriaFieldAttribute标注
    /// </summary>
    /// <param name="transaction">数据库事务</param>
    /// <param name="criteria">从业务条件对象</param>
    /// <param name="groupName">分组名</param>
    /// <param name="orderByInfos">数据排列顺序队列</param>
    public TDetailBusiness GetCompositionDetail<TDetailBusiness>(DbTransaction transaction, ICriteria criteria, string groupName, params OrderByInfo[] orderByInfos)
      where TDetailBusiness : BusinessBase<TDetailBusiness>
    {
      return DoGetDetail<TDetailBusiness>(transaction, new Criterions(typeof(TDetailBusiness), criteria, this, groupName, orderByInfos), true);
    }

    /// <summary>
    /// 取从业务对象(聚合关系)
    /// 条件类的字段映射关系请用Phenix.Core.Mapping.CriteriaFieldAttribute标注
    /// </summary>
    /// <param name="transaction">数据库事务</param>
    /// <param name="criteria">从业务条件对象</param>
    /// <param name="groupName">分组名</param>
    /// <param name="orderByInfos">数据排列顺序队列</param>
    public TDetailBusiness GetAggregationDetail<TDetailBusiness>(DbTransaction transaction, ICriteria criteria, string groupName, params OrderByInfo[] orderByInfos)
      where TDetailBusiness : BusinessBase<TDetailBusiness>
    {
      return DoGetDetail<TDetailBusiness>(transaction, new Criterions(typeof(TDetailBusiness), criteria, this, groupName, orderByInfos), false);
    }

    /// <summary>
    /// 取从业务对象(组合关系)
    /// </summary>
    /// <param name="transaction">数据库事务</param>
    /// <param name="criteriaExpression">从业务条件表达式</param>
    /// <param name="orderByInfos">数据排列顺序队列</param>
    public TDetailBusiness GetCompositionDetail<TDetailBusiness>(DbTransaction transaction, CriteriaExpression criteriaExpression, params OrderByInfo[] orderByInfos)
      where TDetailBusiness : BusinessBase<TDetailBusiness>
    {
      return DoGetDetail<TDetailBusiness>(transaction, new Criterions(typeof(TDetailBusiness), criteriaExpression, this, null, orderByInfos), true);
    }

    /// <summary>
    /// 取从业务对象(聚合关系)
    /// </summary>
    /// <param name="transaction">数据库事务</param>
    /// <param name="criteriaExpression">从业务条件表达式</param>
    /// <param name="orderByInfos">数据排列顺序队列</param>
    public TDetailBusiness GetAggregationDetail<TDetailBusiness>(DbTransaction transaction, CriteriaExpression criteriaExpression, params OrderByInfo[] orderByInfos)
      where TDetailBusiness : BusinessBase<TDetailBusiness>
    {
      return DoGetDetail<TDetailBusiness>(transaction, new Criterions(typeof(TDetailBusiness), criteriaExpression, this, null, orderByInfos), false);
    }

    /// <summary>
    /// 取从业务对象(组合关系)
    /// </summary>
    /// <param name="transaction">数据库事务</param>
    /// <param name="criteriaExpression">从业务条件表达式</param>
    /// <param name="groupName">分组名</param>
    /// <param name="orderByInfos">数据排列顺序队列</param>
    public TDetailBusiness GetCompositionDetail<TDetailBusiness>(DbTransaction transaction, CriteriaExpression criteriaExpression, string groupName, params OrderByInfo[] orderByInfos)
      where TDetailBusiness : BusinessBase<TDetailBusiness>
    {
      return DoGetDetail<TDetailBusiness>(transaction, new Criterions(typeof(TDetailBusiness), criteriaExpression, this, groupName, orderByInfos), true);
    }

    /// <summary>
    /// 取从业务对象(聚合关系)
    /// </summary>
    /// <param name="transaction">数据库事务</param>
    /// <param name="criteriaExpression">从业务条件表达式</param>
    /// <param name="groupName">分组名</param>
    /// <param name="orderByInfos">数据排列顺序队列</param>
    public TDetailBusiness GetAggregationDetail<TDetailBusiness>(DbTransaction transaction, CriteriaExpression criteriaExpression, string groupName, params OrderByInfo[] orderByInfos)
      where TDetailBusiness : BusinessBase<TDetailBusiness>
    {
      return DoGetDetail<TDetailBusiness>(transaction, new Criterions(typeof(TDetailBusiness), criteriaExpression, this, groupName, orderByInfos), false);
    }

    /// <summary>
    /// 取从业务对象(组合关系)
    /// </summary>
    /// <param name="transaction">数据库事务</param>
    /// <param name="criteriaExpression">从业务条件表达式</param>
    /// <param name="orderByInfos">数据排列顺序队列</param>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures")]
    public TDetailBusiness GetCompositionDetail<TDetailBusiness>(DbTransaction transaction, Expression<Func<TDetailBusiness, bool>> criteriaExpression, params OrderByInfo[] orderByInfos)
      where TDetailBusiness : BusinessBase<TDetailBusiness>
    {
      return GetCompositionDetail<TDetailBusiness>(transaction, CriteriaHelper.ToCriteriaExpression(criteriaExpression), orderByInfos);
    }

    /// <summary>
    /// 取从业务对象(聚合关系)
    /// </summary>
    /// <param name="transaction">数据库事务</param>
    /// <param name="criteriaExpression">从业务条件表达式</param>
    /// <param name="orderByInfos">数据排列顺序队列</param>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures")]
    public TDetailBusiness GetAggregationDetail<TDetailBusiness>(DbTransaction transaction, Expression<Func<TDetailBusiness, bool>> criteriaExpression, params OrderByInfo[] orderByInfos)
      where TDetailBusiness : BusinessBase<TDetailBusiness>
    {
      return GetAggregationDetail<TDetailBusiness>(transaction, CriteriaHelper.ToCriteriaExpression(criteriaExpression), orderByInfos);
    }

    /// <summary>
    /// 取从业务对象(组合关系)
    /// </summary>
    /// <param name="transaction">数据库事务</param>
    /// <param name="criteriaExpression">从业务条件表达式</param>
    /// <param name="groupName">分组名</param>
    /// <param name="orderByInfos">数据排列顺序队列</param>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures")]
    public TDetailBusiness GetCompositionDetail<TDetailBusiness>(DbTransaction transaction, Expression<Func<TDetailBusiness, bool>> criteriaExpression, string groupName, params OrderByInfo[] orderByInfos)
      where TDetailBusiness : BusinessBase<TDetailBusiness>
    {
      return GetCompositionDetail<TDetailBusiness>(transaction, CriteriaHelper.ToCriteriaExpression(criteriaExpression), groupName, orderByInfos);
    }

    /// <summary>
    /// 取从业务对象(聚合关系)
    /// </summary>
    /// <param name="transaction">数据库事务</param>
    /// <param name="criteriaExpression">从业务条件表达式</param>
    /// <param name="groupName">分组名</param>
    /// <param name="orderByInfos">数据排列顺序队列</param>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures")]
    public TDetailBusiness GetAggregationDetail<TDetailBusiness>(DbTransaction transaction, Expression<Func<TDetailBusiness, bool>> criteriaExpression, string groupName, params OrderByInfo[] orderByInfos)
      where TDetailBusiness : BusinessBase<TDetailBusiness>
    {
      return GetAggregationDetail<TDetailBusiness>(transaction, CriteriaHelper.ToCriteriaExpression(criteriaExpression), groupName, orderByInfos);
    }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
    private TDetail DoGetDetail<TDetail, TDetailBusiness>(Criterions criterions, bool lazyFetch, bool? cascadingDelete)
      where TDetail : BusinessListBase<TDetail, TDetailBusiness>
      where TDetailBusiness : BusinessBase<TDetailBusiness>
    {
      if (criterions == null)
        criterions = new Criterions(typeof(TDetail), false, false);
      criterions.SetLink(this, cascadingDelete);

      TDetail result = DoFindDetail<TDetail, TDetailBusiness>(criterions, cascadingDelete);
      if (result != null)
        return result;

      if (IsNew)
        return BusinessListBase<TDetail, TDetailBusiness>.New(criterions);
      else if (DbTransaction != null)
        return BusinessListBase<TDetail, TDetailBusiness>.Fetch(DbTransaction, criterions, (Owner != null && Owner.ItemLazyGetDetail) || lazyFetch);
      else
        return BusinessListBase<TDetail, TDetailBusiness>.Fetch(criterions, (Owner != null && Owner.ItemLazyGetDetail) || lazyFetch);
    }

    /// <summary>
    /// 取从业务对象集合
    /// 条件类的字段映射关系请用Phenix.Core.Mapping.CriteriaFieldAttribute标注
    /// </summary>
    /// <param name="criterions">条件集</param>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
    public TDetail GetDetail<TDetail, TDetailBusiness>(Criterions criterions)
      where TDetail : BusinessListBase<TDetail, TDetailBusiness>
      where TDetailBusiness : BusinessBase<TDetailBusiness>
    {
      return DoGetDetail<TDetail, TDetailBusiness>(criterions, false, null);
    }

    /// <summary>
    /// 取从业务对象集合
    /// 条件类的字段映射关系请用Phenix.Core.Mapping.CriteriaFieldAttribute标注
    /// </summary>
    /// <param name="criterions">条件集</param>
    /// <param name="lazyFetch">是否惰性Fetch(</param>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
    public TDetail GetDetail<TDetail, TDetailBusiness>(Criterions criterions, bool lazyFetch)
      where TDetail : BusinessListBase<TDetail, TDetailBusiness>
      where TDetailBusiness : BusinessBase<TDetailBusiness>
    {
      return DoGetDetail<TDetail, TDetailBusiness>(criterions, lazyFetch, null);
    }

    /// <summary>
    /// 取从业务对象集合
    /// </summary>
    /// <param name="orderByInfos">数据排列顺序队列</param>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
    [Obsolete("建议使用: GetCompositionDetail<TDetail, TDetailBusiness>(params OrderByInfo[] orderByInfos)", false)]
    public TDetail GetDetail<TDetail, TDetailBusiness>(params OrderByInfo[] orderByInfos)
      where TDetail : BusinessListBase<TDetail, TDetailBusiness>
      where TDetailBusiness : BusinessBase<TDetailBusiness>
    {
      return GetCompositionDetail<TDetail, TDetailBusiness>(orderByInfos);
    }

    /// <summary>
    /// 取从业务对象集合(组合关系)
    /// </summary>
    /// <param name="orderByInfos">数据排列顺序队列</param>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
    public TDetail GetCompositionDetail<TDetail, TDetailBusiness>(params OrderByInfo[] orderByInfos)
      where TDetail : BusinessListBase<TDetail, TDetailBusiness>
      where TDetailBusiness : BusinessBase<TDetailBusiness>
    {
      return DoGetDetail<TDetail, TDetailBusiness>(new Criterions(typeof(TDetail), false, false, orderByInfos), false, true);
    }

    /// <summary>
    /// 取从业务对象集合(聚合关系)
    /// </summary>
    /// <param name="orderByInfos">数据排列顺序队列</param>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
    public TDetail GetAggregationDetail<TDetail, TDetailBusiness>(params OrderByInfo[] orderByInfos)
      where TDetail : BusinessListBase<TDetail, TDetailBusiness>
      where TDetailBusiness : BusinessBase<TDetailBusiness>
    {
      return DoGetDetail<TDetail, TDetailBusiness>(new Criterions(typeof(TDetail), false, false, orderByInfos), false, false);
    }
    
    /// <summary>
    /// 取从业务对象集合(组合关系)
    /// </summary>
    /// <param name="lazyFetch">是否惰性Fetch(</param>
    /// <param name="orderByInfos">数据排列顺序队列</param>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
    public TDetail GetCompositionDetail<TDetail, TDetailBusiness>(bool lazyFetch, params OrderByInfo[] orderByInfos)
      where TDetail : BusinessListBase<TDetail, TDetailBusiness>
      where TDetailBusiness : BusinessBase<TDetailBusiness>
    {
      return DoGetDetail<TDetail, TDetailBusiness>(new Criterions(typeof(TDetail), false, false, orderByInfos), lazyFetch, true);
    }

    /// <summary>
    /// 取从业务对象集合(聚合关系)
    /// </summary>
    /// <param name="lazyFetch">是否惰性Fetch(</param>
    /// <param name="orderByInfos">数据排列顺序队列</param>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
    public TDetail GetAggregationDetail<TDetail, TDetailBusiness>(bool lazyFetch, params OrderByInfo[] orderByInfos)
      where TDetail : BusinessListBase<TDetail, TDetailBusiness>
      where TDetailBusiness : BusinessBase<TDetailBusiness>
    {
      return DoGetDetail<TDetail, TDetailBusiness>(new Criterions(typeof(TDetail), false, false, orderByInfos), lazyFetch, false);
    }

    /// <summary>
    /// 取从业务对象集合(组合关系)
    /// </summary>
    /// <param name="groupName">分组名</param>
    /// <param name="orderByInfos">数据排列顺序队列</param>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
    public TDetail GetCompositionDetail<TDetail, TDetailBusiness>(string groupName, params OrderByInfo[] orderByInfos)
      where TDetail : BusinessListBase<TDetail, TDetailBusiness>
      where TDetailBusiness : BusinessBase<TDetailBusiness>
    {
      return DoGetDetail<TDetail, TDetailBusiness>(new Criterions(typeof(TDetail), this, groupName, orderByInfos), false, true);
    }

    /// <summary>
    /// 取从业务对象集合(聚合关系)
    /// </summary>
    /// <param name="groupName">分组名</param>
    /// <param name="orderByInfos">数据排列顺序队列</param>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
    public TDetail GetAggregationDetail<TDetail, TDetailBusiness>(string groupName, params OrderByInfo[] orderByInfos)
      where TDetail : BusinessListBase<TDetail, TDetailBusiness>
      where TDetailBusiness : BusinessBase<TDetailBusiness>
    {
      return DoGetDetail<TDetail, TDetailBusiness>(new Criterions(typeof(TDetail), this, groupName, orderByInfos), false, false);
    }

    /// <summary>
    /// 取从业务对象集合(组合关系)
    /// </summary>
    /// <param name="groupName">分组名</param>
    /// <param name="lazyFetch">是否惰性Fetch(</param>
    /// <param name="orderByInfos">数据排列顺序队列</param>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
    public TDetail GetCompositionDetail<TDetail, TDetailBusiness>(string groupName, bool lazyFetch, params OrderByInfo[] orderByInfos)
      where TDetail : BusinessListBase<TDetail, TDetailBusiness>
      where TDetailBusiness : BusinessBase<TDetailBusiness>
    {
      return DoGetDetail<TDetail, TDetailBusiness>(new Criterions(typeof(TDetail), this, groupName, orderByInfos), lazyFetch, true);
    }

    /// <summary>
    /// 取从业务对象集合(聚合关系)
    /// </summary>
    /// <param name="groupName">分组名</param>
    /// <param name="lazyFetch">是否惰性Fetch(</param>
    /// <param name="orderByInfos">数据排列顺序队列</param>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
    public TDetail GetAggregationDetail<TDetail, TDetailBusiness>(string groupName, bool lazyFetch, params OrderByInfo[] orderByInfos)
      where TDetail : BusinessListBase<TDetail, TDetailBusiness>
      where TDetailBusiness : BusinessBase<TDetailBusiness>
    {
      return DoGetDetail<TDetail, TDetailBusiness>(new Criterions(typeof(TDetail), this, groupName, orderByInfos), lazyFetch, false);
    }

    /// <summary>
    /// 取从业务对象集合
    /// </summary>
    /// <param name="groupName">分组名</param>
    /// <param name="cascadingSave">是否级联保存?</param>
    /// <param name="cascadingDelete">是否级联删除?</param>
    /// <param name="orderByInfos">数据排列顺序队列</param>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
    public TDetail GetDetail<TDetail, TDetailBusiness>(string groupName, bool cascadingSave, bool cascadingDelete, params OrderByInfo[] orderByInfos)
      where TDetail : BusinessListBase<TDetail, TDetailBusiness>
      where TDetailBusiness : BusinessBase<TDetailBusiness>
    {
      return DoGetDetail<TDetail, TDetailBusiness>(new Criterions(typeof(TDetail), this, groupName, cascadingSave, cascadingDelete, orderByInfos), false, null);
    }

    /// <summary>
    /// 取从业务对象集合
    /// </summary>
    /// <param name="groupName">分组名</param>
    /// <param name="cascadingSave">是否级联保存?</param>
    /// <param name="cascadingDelete">是否级联删除?</param>
    /// <param name="lazyFetch">是否惰性Fetch(</param>
    /// <param name="orderByInfos">数据排列顺序队列</param>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
    public TDetail GetDetail<TDetail, TDetailBusiness>(string groupName, bool cascadingSave, bool cascadingDelete, bool lazyFetch, params OrderByInfo[] orderByInfos)
      where TDetail : BusinessListBase<TDetail, TDetailBusiness>
      where TDetailBusiness : BusinessBase<TDetailBusiness>
    {
      return DoGetDetail<TDetail, TDetailBusiness>(new Criterions(typeof(TDetail), this, groupName, cascadingSave, cascadingDelete, orderByInfos), lazyFetch, null);
    }

    /// <summary>
    /// 取从业务对象集合(组合关系)
    /// 条件类的字段映射关系请用Phenix.Core.Mapping.CriteriaFieldAttribute标注
    /// </summary>
    /// <param name="criteria">从业务条件对象</param>
    /// <param name="orderByInfos">数据排列顺序队列</param>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
    public TDetail GetCompositionDetail<TDetail, TDetailBusiness>(ICriteria criteria, params OrderByInfo[] orderByInfos)
      where TDetail : BusinessListBase<TDetail, TDetailBusiness>
      where TDetailBusiness : BusinessBase<TDetailBusiness>
    {
      return DoGetDetail<TDetail, TDetailBusiness>(new Criterions(typeof(TDetail), criteria, this, null, orderByInfos), false, true);
    }

    /// <summary>
    /// 取从业务对象集合(聚合关系)
    /// 条件类的字段映射关系请用Phenix.Core.Mapping.CriteriaFieldAttribute标注
    /// </summary>
    /// <param name="criteria">从业务条件对象</param>
    /// <param name="orderByInfos">数据排列顺序队列</param>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
    public TDetail GetAggregationDetail<TDetail, TDetailBusiness>(ICriteria criteria, params OrderByInfo[] orderByInfos)
      where TDetail : BusinessListBase<TDetail, TDetailBusiness>
      where TDetailBusiness : BusinessBase<TDetailBusiness>
    {
      return DoGetDetail<TDetail, TDetailBusiness>(new Criterions(typeof(TDetail), criteria, this, null, orderByInfos), false, false);
    }

    /// <summary>
    /// 取从业务对象集合(组合关系)
    /// 条件类的字段映射关系请用Phenix.Core.Mapping.CriteriaFieldAttribute标注
    /// </summary>
    /// <param name="criteria">从业务条件对象</param>
    /// <param name="groupName">分组名</param>
    /// <param name="orderByInfos">数据排列顺序队列</param>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
    public TDetail GetCompositionDetail<TDetail, TDetailBusiness>(ICriteria criteria, string groupName, params OrderByInfo[] orderByInfos)
      where TDetail : BusinessListBase<TDetail, TDetailBusiness>
      where TDetailBusiness : BusinessBase<TDetailBusiness>
    {
      return DoGetDetail<TDetail, TDetailBusiness>(new Criterions(typeof(TDetail), criteria, this, groupName, orderByInfos), false, true);
    }

    /// <summary>
    /// 取从业务对象集合(聚合关系)
    /// 条件类的字段映射关系请用Phenix.Core.Mapping.CriteriaFieldAttribute标注
    /// </summary>
    /// <param name="criteria">从业务条件对象</param>
    /// <param name="groupName">分组名</param>
    /// <param name="orderByInfos">数据排列顺序队列</param>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
    public TDetail GetAggregationDetail<TDetail, TDetailBusiness>(ICriteria criteria, string groupName, params OrderByInfo[] orderByInfos)
      where TDetail : BusinessListBase<TDetail, TDetailBusiness>
      where TDetailBusiness : BusinessBase<TDetailBusiness>
    {
      return DoGetDetail<TDetail, TDetailBusiness>(new Criterions(typeof(TDetail), criteria, this, groupName, orderByInfos), false, false);
    }

    /// <summary>
    /// 取从业务对象集合(组合关系)
    /// 条件类的字段映射关系请用Phenix.Core.Mapping.CriteriaFieldAttribute标注
    /// </summary>
    /// <param name="criteria">从业务条件对象</param>
    /// <param name="groupName">分组名</param>
    /// <param name="lazyFetch">是否惰性Fetch(</param>
    /// <param name="orderByInfos">数据排列顺序队列</param>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
    public TDetail GetCompositionDetail<TDetail, TDetailBusiness>(ICriteria criteria, string groupName, bool lazyFetch, params OrderByInfo[] orderByInfos)
      where TDetail : BusinessListBase<TDetail, TDetailBusiness>
      where TDetailBusiness : BusinessBase<TDetailBusiness>
    {
      return DoGetDetail<TDetail, TDetailBusiness>(new Criterions(typeof(TDetail), criteria, this, groupName, orderByInfos), lazyFetch, true);
    }

    /// <summary>
    /// 取从业务对象集合(聚合关系)
    /// 条件类的字段映射关系请用Phenix.Core.Mapping.CriteriaFieldAttribute标注
    /// </summary>
    /// <param name="criteria">从业务条件对象</param>
    /// <param name="groupName">分组名</param>
    /// <param name="lazyFetch">是否惰性Fetch(</param>
    /// <param name="orderByInfos">数据排列顺序队列</param>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
    public TDetail GetAggregationDetail<TDetail, TDetailBusiness>(ICriteria criteria, string groupName, bool lazyFetch, params OrderByInfo[] orderByInfos)
      where TDetail : BusinessListBase<TDetail, TDetailBusiness>
      where TDetailBusiness : BusinessBase<TDetailBusiness>
    {
      return DoGetDetail<TDetail, TDetailBusiness>(new Criterions(typeof(TDetail), criteria, this, groupName, orderByInfos), lazyFetch, false);
    }

    /// <summary>
    /// 取从业务对象集合
    /// 条件类的字段映射关系请用Phenix.Core.Mapping.CriteriaFieldAttribute标注
    /// </summary>
    /// <param name="criteria">从业务条件对象</param>
    /// <param name="groupName">分组名</param>
    /// <param name="cascadingSave">是否级联保存?</param>
    /// <param name="cascadingDelete">是否级联删除?</param>
    /// <param name="orderByInfos">数据排列顺序队列</param>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
    public TDetail GetDetail<TDetail, TDetailBusiness>(ICriteria criteria, string groupName, bool cascadingSave, bool cascadingDelete, params OrderByInfo[] orderByInfos)
      where TDetail : BusinessListBase<TDetail, TDetailBusiness>
      where TDetailBusiness : BusinessBase<TDetailBusiness>
    {
      return DoGetDetail<TDetail, TDetailBusiness>(new Criterions(typeof(TDetail), criteria, this, groupName, cascadingSave, cascadingDelete, orderByInfos), false, null);
    }

    /// <summary>
    /// 取从业务对象集合
    /// 条件类的字段映射关系请用Phenix.Core.Mapping.CriteriaFieldAttribute标注
    /// </summary>
    /// <param name="criteria">从业务条件对象</param>
    /// <param name="groupName">分组名</param>
    /// <param name="cascadingSave">是否级联保存?</param>
    /// <param name="cascadingDelete">是否级联删除?</param>
    /// <param name="lazyFetch">是否惰性Fetch(</param>
    /// <param name="orderByInfos">数据排列顺序队列</param>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
    public TDetail GetDetail<TDetail, TDetailBusiness>(ICriteria criteria, string groupName, bool cascadingSave, bool cascadingDelete, bool lazyFetch, params OrderByInfo[] orderByInfos)
      where TDetail : BusinessListBase<TDetail, TDetailBusiness>
      where TDetailBusiness : BusinessBase<TDetailBusiness>
    {
      return DoGetDetail<TDetail, TDetailBusiness>(new Criterions(typeof(TDetail), criteria, this, groupName, cascadingSave, cascadingDelete, orderByInfos), lazyFetch, null);
    }

    /// <summary>
    /// 取从业务对象集合(组合关系)
    /// </summary>
    /// <param name="criteriaExpression">从业务条件表达式</param>
    /// <param name="orderByInfos">数据排列顺序队列</param>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
    public TDetail GetCompositionDetail<TDetail, TDetailBusiness>(CriteriaExpression criteriaExpression, params OrderByInfo[] orderByInfos)
      where TDetail : BusinessListBase<TDetail, TDetailBusiness>
      where TDetailBusiness : BusinessBase<TDetailBusiness>
    {
      return DoGetDetail<TDetail, TDetailBusiness>(new Criterions(typeof(TDetail), criteriaExpression, this, null, orderByInfos), false, true);
    }

    /// <summary>
    /// 取从业务对象集合(聚合关系)
    /// </summary>
    /// <param name="criteriaExpression">从业务条件表达式</param>
    /// <param name="orderByInfos">数据排列顺序队列</param>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
    public TDetail GetAggregationDetail<TDetail, TDetailBusiness>(CriteriaExpression criteriaExpression, params OrderByInfo[] orderByInfos)
      where TDetail : BusinessListBase<TDetail, TDetailBusiness>
      where TDetailBusiness : BusinessBase<TDetailBusiness>
    {
      return DoGetDetail<TDetail, TDetailBusiness>(new Criterions(typeof(TDetail), criteriaExpression, this, null, orderByInfos), false, false);
    }

    /// <summary>
    /// 取从业务对象集合(组合关系)
    /// </summary>
    /// <param name="criteriaExpression">从业务条件表达式</param>
    /// <param name="groupName">分组名</param>
    /// <param name="orderByInfos">数据排列顺序队列</param>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
    public TDetail GetCompositionDetail<TDetail, TDetailBusiness>(CriteriaExpression criteriaExpression, string groupName, params OrderByInfo[] orderByInfos)
      where TDetail : BusinessListBase<TDetail, TDetailBusiness>
      where TDetailBusiness : BusinessBase<TDetailBusiness>
    {
      return DoGetDetail<TDetail, TDetailBusiness>(new Criterions(typeof(TDetail), criteriaExpression, this, groupName, orderByInfos), false, true);
    }

    /// <summary>
    /// 取从业务对象集合(聚合关系)
    /// </summary>
    /// <param name="criteriaExpression">从业务条件表达式</param>
    /// <param name="groupName">分组名</param>
    /// <param name="orderByInfos">数据排列顺序队列</param>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
    public TDetail GetAggregationDetail<TDetail, TDetailBusiness>(CriteriaExpression criteriaExpression, string groupName, params OrderByInfo[] orderByInfos)
      where TDetail : BusinessListBase<TDetail, TDetailBusiness>
      where TDetailBusiness : BusinessBase<TDetailBusiness>
    {
      return DoGetDetail<TDetail, TDetailBusiness>(new Criterions(typeof(TDetail), criteriaExpression, this, groupName, orderByInfos), false, false);
    }

    /// <summary>
    /// 取从业务对象集合(组合关系)
    /// </summary>
    /// <param name="criteriaExpression">从业务条件表达式</param>
    /// <param name="groupName">分组名</param>
    /// <param name="lazyFetch">是否惰性Fetch(</param>
    /// <param name="orderByInfos">数据排列顺序队列</param>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
    public TDetail GetCompositionDetail<TDetail, TDetailBusiness>(CriteriaExpression criteriaExpression, string groupName, bool lazyFetch, params OrderByInfo[] orderByInfos)
      where TDetail : BusinessListBase<TDetail, TDetailBusiness>
      where TDetailBusiness : BusinessBase<TDetailBusiness>
    {
      return DoGetDetail<TDetail, TDetailBusiness>(new Criterions(typeof(TDetail), criteriaExpression, this, groupName, orderByInfos), lazyFetch, true);
    }

    /// <summary>
    /// 取从业务对象集合(聚合关系)
    /// </summary>
    /// <param name="criteriaExpression">从业务条件表达式</param>
    /// <param name="groupName">分组名</param>
    /// <param name="lazyFetch">是否惰性Fetch(</param>
    /// <param name="orderByInfos">数据排列顺序队列</param>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
    public TDetail GetAggregationDetail<TDetail, TDetailBusiness>(CriteriaExpression criteriaExpression, string groupName, bool lazyFetch, params OrderByInfo[] orderByInfos)
      where TDetail : BusinessListBase<TDetail, TDetailBusiness>
      where TDetailBusiness : BusinessBase<TDetailBusiness>
    {
      return DoGetDetail<TDetail, TDetailBusiness>(new Criterions(typeof(TDetail), criteriaExpression, this, groupName, orderByInfos), lazyFetch, false);
    }

    /// <summary>
    /// 取从业务对象集合
    /// </summary>
    /// <param name="criteriaExpression">从业务条件表达式</param>
    /// <param name="groupName">分组名</param>
    /// <param name="cascadingSave">是否级联保存?</param>
    /// <param name="cascadingDelete">是否级联删除?</param>
    /// <param name="orderByInfos">数据排列顺序队列</param>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
    public TDetail GetDetail<TDetail, TDetailBusiness>(CriteriaExpression criteriaExpression, string groupName, bool cascadingSave, bool cascadingDelete, params OrderByInfo[] orderByInfos)
      where TDetail : BusinessListBase<TDetail, TDetailBusiness>
      where TDetailBusiness : BusinessBase<TDetailBusiness>
    {
      return DoGetDetail<TDetail, TDetailBusiness>(new Criterions(typeof(TDetail), criteriaExpression, this, groupName, cascadingSave, cascadingDelete, orderByInfos), false, null);
    }

    /// <summary>
    /// 取从业务对象集合
    /// </summary>
    /// <param name="criteriaExpression">从业务条件表达式</param>
    /// <param name="groupName">分组名</param>
    /// <param name="cascadingSave">是否级联保存?</param>
    /// <param name="cascadingDelete">是否级联删除?</param>
    /// <param name="lazyFetch">是否惰性Fetch(</param>
    /// <param name="orderByInfos">数据排列顺序队列</param>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
    public TDetail GetDetail<TDetail, TDetailBusiness>(CriteriaExpression criteriaExpression, string groupName, bool cascadingSave, bool cascadingDelete, bool lazyFetch, params OrderByInfo[] orderByInfos)
      where TDetail : BusinessListBase<TDetail, TDetailBusiness>
      where TDetailBusiness : BusinessBase<TDetailBusiness>
    {
      return DoGetDetail<TDetail, TDetailBusiness>(new Criterions(typeof(TDetail), criteriaExpression, this, groupName, cascadingSave, cascadingDelete, orderByInfos), lazyFetch, null);
    }

    /// <summary>
    /// 取从业务对象集合(组合关系)
    /// </summary>
    /// <param name="criteriaExpression">从业务条件表达式</param>
    /// <param name="orderByInfos">数据排列顺序队列</param>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures")]
    public TDetail GetCompositionDetail<TDetail, TDetailBusiness>(Expression<Func<TDetailBusiness, bool>> criteriaExpression, params OrderByInfo[] orderByInfos)
      where TDetail : BusinessListBase<TDetail, TDetailBusiness>
      where TDetailBusiness : BusinessBase<TDetailBusiness>
    {
      return GetCompositionDetail<TDetail, TDetailBusiness>(CriteriaHelper.ToCriteriaExpression(criteriaExpression), orderByInfos);
    }

    /// <summary>
    /// 取从业务对象集合(聚合关系)
    /// </summary>
    /// <param name="criteriaExpression">从业务条件表达式</param>
    /// <param name="orderByInfos">数据排列顺序队列</param>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures")]
    public TDetail GetAggregationDetail<TDetail, TDetailBusiness>(Expression<Func<TDetailBusiness, bool>> criteriaExpression, params OrderByInfo[] orderByInfos)
      where TDetail : BusinessListBase<TDetail, TDetailBusiness>
      where TDetailBusiness : BusinessBase<TDetailBusiness>
    {
      return GetAggregationDetail<TDetail, TDetailBusiness>(CriteriaHelper.ToCriteriaExpression(criteriaExpression), orderByInfos);
    }

    /// <summary>
    /// 取从业务对象集合(组合关系)
    /// </summary>
    /// <param name="criteriaExpression">从业务条件表达式</param>
    /// <param name="groupName">分组名</param>
    /// <param name="orderByInfos">数据排列顺序队列</param>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures")]
    public TDetail GetCompositionDetail<TDetail, TDetailBusiness>(Expression<Func<TDetailBusiness, bool>> criteriaExpression, string groupName, params OrderByInfo[] orderByInfos)
      where TDetail : BusinessListBase<TDetail, TDetailBusiness>
      where TDetailBusiness : BusinessBase<TDetailBusiness>
    {
      return GetCompositionDetail<TDetail, TDetailBusiness>(CriteriaHelper.ToCriteriaExpression(criteriaExpression), groupName, orderByInfos);
    }

    /// <summary>
    /// 取从业务对象集合(聚合关系)
    /// </summary>
    /// <param name="criteriaExpression">从业务条件表达式</param>
    /// <param name="groupName">分组名</param>
    /// <param name="orderByInfos">数据排列顺序队列</param>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures")]
    public TDetail GetAggregationDetail<TDetail, TDetailBusiness>(Expression<Func<TDetailBusiness, bool>> criteriaExpression, string groupName, params OrderByInfo[] orderByInfos)
      where TDetail : BusinessListBase<TDetail, TDetailBusiness>
      where TDetailBusiness : BusinessBase<TDetailBusiness>
    {
      return GetAggregationDetail<TDetail, TDetailBusiness>(CriteriaHelper.ToCriteriaExpression(criteriaExpression), groupName, orderByInfos);
    }

    /// <summary>
    /// 取从业务对象集合(组合关系)
    /// </summary>
    /// <param name="criteriaExpression">从业务条件表达式</param>
    /// <param name="groupName">分组名</param>
    /// <param name="lazyFetch">是否惰性Fetch(</param>
    /// <param name="orderByInfos">数据排列顺序队列</param>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures")]
    public TDetail GetCompositionDetail<TDetail, TDetailBusiness>(Expression<Func<TDetailBusiness, bool>> criteriaExpression, string groupName, bool lazyFetch, params OrderByInfo[] orderByInfos)
      where TDetail : BusinessListBase<TDetail, TDetailBusiness>
      where TDetailBusiness : BusinessBase<TDetailBusiness>
    {
      return GetCompositionDetail<TDetail, TDetailBusiness>(CriteriaHelper.ToCriteriaExpression(criteriaExpression), groupName, lazyFetch, orderByInfos);
    }

    /// <summary>
    /// 取从业务对象集合(聚合关系)
    /// </summary>
    /// <param name="criteriaExpression">从业务条件表达式</param>
    /// <param name="groupName">分组名</param>
    /// <param name="lazyFetch">是否惰性Fetch(</param>
    /// <param name="orderByInfos">数据排列顺序队列</param>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures")]
    public TDetail GetAggregationDetail<TDetail, TDetailBusiness>(Expression<Func<TDetailBusiness, bool>> criteriaExpression, string groupName, bool lazyFetch, params OrderByInfo[] orderByInfos)
      where TDetail : BusinessListBase<TDetail, TDetailBusiness>
      where TDetailBusiness : BusinessBase<TDetailBusiness>
    {
      return GetAggregationDetail<TDetail, TDetailBusiness>(CriteriaHelper.ToCriteriaExpression(criteriaExpression), groupName, lazyFetch, orderByInfos);
    }

    /// <summary>
    /// 取从业务对象集合
    /// </summary>
    /// <param name="criteriaExpression">从业务条件表达式</param>
    /// <param name="groupName">分组名</param>
    /// <param name="cascadingSave">是否级联保存?</param>
    /// <param name="cascadingDelete">是否级联删除?</param>
    /// <param name="orderByInfos">数据排列顺序队列</param>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures")]
    public TDetail GetDetail<TDetail, TDetailBusiness>(Expression<Func<TDetailBusiness, bool>> criteriaExpression, string groupName, bool cascadingSave, bool cascadingDelete, params OrderByInfo[] orderByInfos)
      where TDetail : BusinessListBase<TDetail, TDetailBusiness>
      where TDetailBusiness : BusinessBase<TDetailBusiness>
    {
      return GetDetail<TDetail, TDetailBusiness>(CriteriaHelper.ToCriteriaExpression(criteriaExpression), groupName, cascadingSave, cascadingDelete, false, orderByInfos);
    }

    /// <summary>
    /// 取从业务对象集合
    /// </summary>
    /// <param name="criteriaExpression">从业务条件表达式</param>
    /// <param name="groupName">分组名</param>
    /// <param name="cascadingSave">是否级联保存?</param>
    /// <param name="cascadingDelete">是否级联删除?</param>
    /// <param name="lazyFetch">是否惰性Fetch(</param>
    /// <param name="orderByInfos">数据排列顺序队列</param>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures")]
    public TDetail GetDetail<TDetail, TDetailBusiness>(Expression<Func<TDetailBusiness, bool>> criteriaExpression, string groupName, bool cascadingSave, bool cascadingDelete, bool lazyFetch, params OrderByInfo[] orderByInfos)
      where TDetail : BusinessListBase<TDetail, TDetailBusiness>
      where TDetailBusiness : BusinessBase<TDetailBusiness>
    {
      return GetDetail<TDetail, TDetailBusiness>(CriteriaHelper.ToCriteriaExpression(criteriaExpression), groupName, cascadingSave, cascadingDelete, lazyFetch, orderByInfos);
    }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
    private TDetail DoGetDetail<TDetail, TDetailBusiness>(DbConnection connection, Criterions criterions, bool lazyFetch, bool? cascadingDelete)
      where TDetail : BusinessListBase<TDetail, TDetailBusiness>
      where TDetailBusiness : BusinessBase<TDetailBusiness>
    {
      if (criterions == null)
        criterions = new Criterions(typeof(TDetail), false, false);
      criterions.SetLink(this, cascadingDelete);

      //TDetail result = DoFindDetail<TDetail, TDetailBusiness>(criterions, cascadingDelete);
      //if (result != null)
      //  return result;

      //return IsNew ?
      //  BusinessListBase<TDetail, TDetailBusiness>.New(criterions) :
      return BusinessListBase<TDetail, TDetailBusiness>.Fetch(connection, criterions, (Owner != null && Owner.ItemLazyGetDetail) || lazyFetch);
    }

    /// <summary>
    /// 取从业务对象集合
    /// 条件类的字段映射关系请用Phenix.Core.Mapping.CriteriaFieldAttribute标注
    /// </summary>
    /// <param name="connection">数据库连接</param>
    /// <param name="criterions">条件集</param>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
    public TDetail GetDetail<TDetail, TDetailBusiness>(DbConnection connection, Criterions criterions)
      where TDetail : BusinessListBase<TDetail, TDetailBusiness>
      where TDetailBusiness : BusinessBase<TDetailBusiness>
    {
      return DoGetDetail<TDetail, TDetailBusiness>(connection, criterions, false, null);
    }
    
    /// <summary>
    /// 取从业务对象集合
    /// 条件类的字段映射关系请用Phenix.Core.Mapping.CriteriaFieldAttribute标注
    /// </summary>
    /// <param name="connection">数据库连接</param>
    /// <param name="criterions">条件集</param>
    /// <param name="lazyFetch">是否惰性Fetch(</param>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
    public TDetail GetDetail<TDetail, TDetailBusiness>(DbConnection connection, Criterions criterions, bool lazyFetch)
      where TDetail : BusinessListBase<TDetail, TDetailBusiness>
      where TDetailBusiness : BusinessBase<TDetailBusiness>
    {
      return DoGetDetail<TDetail, TDetailBusiness>(connection, criterions, lazyFetch, null);
    }

    /// <summary>
    /// 取从业务对象集合
    /// </summary>
    /// <param name="connection">数据库连接</param>
    /// <param name="orderByInfos">数据排列顺序队列</param>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
    [Obsolete("建议使用: GetCompositionDetail<TDetail, TDetailBusiness>(DbConnection connection, params OrderByInfo[] orderByInfos)", false)]
    public TDetail GetDetail<TDetail, TDetailBusiness>(DbConnection connection, params OrderByInfo[] orderByInfos)
      where TDetail : BusinessListBase<TDetail, TDetailBusiness>
      where TDetailBusiness : BusinessBase<TDetailBusiness>
    {
      return GetCompositionDetail<TDetail, TDetailBusiness>(connection, orderByInfos);
    }

    /// <summary>
    /// 取从业务对象集合(组合关系)
    /// </summary>
    /// <param name="connection">数据库连接</param>
    /// <param name="orderByInfos">数据排列顺序队列</param>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
    public TDetail GetCompositionDetail<TDetail, TDetailBusiness>(DbConnection connection, params OrderByInfo[] orderByInfos)
      where TDetail : BusinessListBase<TDetail, TDetailBusiness>
      where TDetailBusiness : BusinessBase<TDetailBusiness>
    {
      return DoGetDetail<TDetail, TDetailBusiness>(connection, new Criterions(typeof(TDetail), false, false, orderByInfos), false, true);
    }

    /// <summary>
    /// 取从业务对象集合(聚合关系)
    /// </summary>
    /// <param name="connection">数据库连接</param>
    /// <param name="orderByInfos">数据排列顺序队列</param>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
    public TDetail GetAggregationDetail<TDetail, TDetailBusiness>(DbConnection connection, params OrderByInfo[] orderByInfos)
      where TDetail : BusinessListBase<TDetail, TDetailBusiness>
      where TDetailBusiness : BusinessBase<TDetailBusiness>
    {
      return DoGetDetail<TDetail, TDetailBusiness>(connection, new Criterions(typeof(TDetail), false, false, orderByInfos), false, false);
    }

    /// <summary>
    /// 取从业务对象集合(组合关系)
    /// </summary>
    /// <param name="connection">数据库连接</param>
    /// <param name="groupName">分组名</param>
    /// <param name="orderByInfos">数据排列顺序队列</param>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
    public TDetail GetCompositionDetail<TDetail, TDetailBusiness>(DbConnection connection, string groupName, params OrderByInfo[] orderByInfos)
      where TDetail : BusinessListBase<TDetail, TDetailBusiness>
      where TDetailBusiness : BusinessBase<TDetailBusiness>
    {
      return DoGetDetail<TDetail, TDetailBusiness>(connection, new Criterions(typeof(TDetail), this, groupName, orderByInfos), false, true);
    }

    /// <summary>
    /// 取从业务对象集合(聚合关系)
    /// </summary>
    /// <param name="connection">数据库连接</param>
    /// <param name="groupName">分组名</param>
    /// <param name="orderByInfos">数据排列顺序队列</param>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
    public TDetail GetAggregationDetail<TDetail, TDetailBusiness>(DbConnection connection, string groupName, params OrderByInfo[] orderByInfos)
      where TDetail : BusinessListBase<TDetail, TDetailBusiness>
      where TDetailBusiness : BusinessBase<TDetailBusiness>
    {
      return DoGetDetail<TDetail, TDetailBusiness>(connection, new Criterions(typeof(TDetail), this, groupName, orderByInfos), false, false);
    }

    /// <summary>
    /// 取从业务对象集合
    /// </summary>
    /// <param name="connection">数据库连接</param>
    /// <param name="groupName">分组名</param>
    /// <param name="cascadingSave">是否级联保存?</param>
    /// <param name="cascadingDelete">是否级联删除?</param>
    /// <param name="orderByInfos">数据排列顺序队列</param>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
    public TDetail GetDetail<TDetail, TDetailBusiness>(DbConnection connection, string groupName, bool cascadingSave, bool cascadingDelete, params OrderByInfo[] orderByInfos)
      where TDetail : BusinessListBase<TDetail, TDetailBusiness>
      where TDetailBusiness : BusinessBase<TDetailBusiness>
    {
      return GetDetail<TDetail, TDetailBusiness>(connection, new Criterions(typeof(TDetail), this, groupName, cascadingSave, cascadingDelete, orderByInfos));
    }

    /// <summary>
    /// 取从业务对象集合(组合关系)
    /// 条件类的字段映射关系请用Phenix.Core.Mapping.CriteriaFieldAttribute标注
    /// </summary>
    /// <param name="connection">数据库连接</param>
    /// <param name="criteria">从业务条件对象</param>
    /// <param name="orderByInfos">数据排列顺序队列</param>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
    public TDetail GetCompositionDetail<TDetail, TDetailBusiness>(DbConnection connection, ICriteria criteria, params OrderByInfo[] orderByInfos)
      where TDetail : BusinessListBase<TDetail, TDetailBusiness>
      where TDetailBusiness : BusinessBase<TDetailBusiness>
    {
      return DoGetDetail<TDetail, TDetailBusiness>(connection, new Criterions(typeof(TDetail), criteria, this, null, orderByInfos), false, true);
    }

    /// <summary>
    /// 取从业务对象集合(聚合关系)
    /// 条件类的字段映射关系请用Phenix.Core.Mapping.CriteriaFieldAttribute标注
    /// </summary>
    /// <param name="connection">数据库连接</param>
    /// <param name="criteria">从业务条件对象</param>
    /// <param name="orderByInfos">数据排列顺序队列</param>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
    public TDetail GetAggregationDetail<TDetail, TDetailBusiness>(DbConnection connection, ICriteria criteria, params OrderByInfo[] orderByInfos)
      where TDetail : BusinessListBase<TDetail, TDetailBusiness>
      where TDetailBusiness : BusinessBase<TDetailBusiness>
    {
      return DoGetDetail<TDetail, TDetailBusiness>(connection, new Criterions(typeof(TDetail), criteria, this, null, orderByInfos), false, false);
    }

    /// <summary>
    /// 取从业务对象集合(组合关系)
    /// 条件类的字段映射关系请用Phenix.Core.Mapping.CriteriaFieldAttribute标注
    /// </summary>
    /// <param name="connection">数据库连接</param>
    /// <param name="criteria">从业务条件对象</param>
    /// <param name="groupName">分组名</param>
    /// <param name="orderByInfos">数据排列顺序队列</param>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
    public TDetail GetCompositionDetail<TDetail, TDetailBusiness>(DbConnection connection, ICriteria criteria, string groupName, params OrderByInfo[] orderByInfos)
      where TDetail : BusinessListBase<TDetail, TDetailBusiness>
      where TDetailBusiness : BusinessBase<TDetailBusiness>
    {
      return DoGetDetail<TDetail, TDetailBusiness>(connection, new Criterions(typeof(TDetail), criteria, this, groupName, orderByInfos), false, true);
    }

    /// <summary>
    /// 取从业务对象集合(聚合关系)
    /// 条件类的字段映射关系请用Phenix.Core.Mapping.CriteriaFieldAttribute标注
    /// </summary>
    /// <param name="connection">数据库连接</param>
    /// <param name="criteria">从业务条件对象</param>
    /// <param name="groupName">分组名</param>
    /// <param name="orderByInfos">数据排列顺序队列</param>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
    public TDetail GetAggregationDetail<TDetail, TDetailBusiness>(DbConnection connection, ICriteria criteria, string groupName, params OrderByInfo[] orderByInfos)
      where TDetail : BusinessListBase<TDetail, TDetailBusiness>
      where TDetailBusiness : BusinessBase<TDetailBusiness>
    {
      return DoGetDetail<TDetail, TDetailBusiness>(connection, new Criterions(typeof(TDetail), criteria, this, groupName, orderByInfos), false, false);
    }

    /// <summary>
    /// 取从业务对象集合
    /// 条件类的字段映射关系请用Phenix.Core.Mapping.CriteriaFieldAttribute标注
    /// </summary>
    /// <param name="connection">数据库连接</param>
    /// <param name="criteria">从业务条件对象</param>
    /// <param name="groupName">分组名</param>
    /// <param name="cascadingSave">是否级联保存?</param>
    /// <param name="cascadingDelete">是否级联删除?</param>
    /// <param name="orderByInfos">数据排列顺序队列</param>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
    public TDetail GetDetail<TDetail, TDetailBusiness>(DbConnection connection, ICriteria criteria, string groupName, bool cascadingSave, bool cascadingDelete, params OrderByInfo[] orderByInfos)
      where TDetail : BusinessListBase<TDetail, TDetailBusiness>
      where TDetailBusiness : BusinessBase<TDetailBusiness>
    {
      return GetDetail<TDetail, TDetailBusiness>(connection, new Criterions(typeof(TDetail), criteria, this, groupName, cascadingSave, cascadingDelete, orderByInfos));
    }

    /// <summary>
    /// 取从业务对象集合(组合关系)
    /// </summary>
    /// <param name="connection">数据库连接</param>
    /// <param name="criteriaExpression">从业务条件表达式</param>
    /// <param name="orderByInfos">数据排列顺序队列</param>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
    public TDetail GetCompositionDetail<TDetail, TDetailBusiness>(DbConnection connection, CriteriaExpression criteriaExpression, params OrderByInfo[] orderByInfos)
      where TDetail : BusinessListBase<TDetail, TDetailBusiness>
      where TDetailBusiness : BusinessBase<TDetailBusiness>
    {
      return DoGetDetail<TDetail, TDetailBusiness>(connection, new Criterions(typeof(TDetail), criteriaExpression, this, null, orderByInfos), false, true);
    }

    /// <summary>
    /// 取从业务对象集合(聚合关系)
    /// </summary>
    /// <param name="connection">数据库连接</param>
    /// <param name="criteriaExpression">从业务条件表达式</param>
    /// <param name="orderByInfos">数据排列顺序队列</param>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
    public TDetail GetAggregationDetail<TDetail, TDetailBusiness>(DbConnection connection, CriteriaExpression criteriaExpression, params OrderByInfo[] orderByInfos)
      where TDetail : BusinessListBase<TDetail, TDetailBusiness>
      where TDetailBusiness : BusinessBase<TDetailBusiness>
    {
      return DoGetDetail<TDetail, TDetailBusiness>(connection, new Criterions(typeof(TDetail), criteriaExpression, this, null, orderByInfos), false, false);
    }

    /// <summary>
    /// 取从业务对象集合(组合关系)
    /// </summary>
    /// <param name="connection">数据库连接</param>
    /// <param name="criteriaExpression">从业务条件表达式</param>
    /// <param name="groupName">分组名</param>
    /// <param name="orderByInfos">数据排列顺序队列</param>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
    public TDetail GetCompositionDetail<TDetail, TDetailBusiness>(DbConnection connection, CriteriaExpression criteriaExpression, string groupName, params OrderByInfo[] orderByInfos)
      where TDetail : BusinessListBase<TDetail, TDetailBusiness>
      where TDetailBusiness : BusinessBase<TDetailBusiness>
    {
      return DoGetDetail<TDetail, TDetailBusiness>(connection, new Criterions(typeof(TDetail), criteriaExpression, this, groupName, orderByInfos), false, true);
    }

    /// <summary>
    /// 取从业务对象集合(聚合关系)
    /// </summary>
    /// <param name="connection">数据库连接</param>
    /// <param name="criteriaExpression">从业务条件表达式</param>
    /// <param name="groupName">分组名</param>
    /// <param name="orderByInfos">数据排列顺序队列</param>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
    public TDetail GetAggregationDetail<TDetail, TDetailBusiness>(DbConnection connection, CriteriaExpression criteriaExpression, string groupName, params OrderByInfo[] orderByInfos)
      where TDetail : BusinessListBase<TDetail, TDetailBusiness>
      where TDetailBusiness : BusinessBase<TDetailBusiness>
    {
      return DoGetDetail<TDetail, TDetailBusiness>(connection, new Criterions(typeof(TDetail), criteriaExpression, this, groupName, orderByInfos), false, false);
    }

    /// <summary>
    /// 取从业务对象集合
    /// </summary>
    /// <param name="connection">数据库连接</param>
    /// <param name="criteriaExpression">从业务条件表达式</param>
    /// <param name="groupName">分组名</param>
    /// <param name="cascadingSave">是否级联保存?</param>
    /// <param name="cascadingDelete">是否级联删除?</param>
    /// <param name="orderByInfos">数据排列顺序队列</param>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
    public TDetail GetDetail<TDetail, TDetailBusiness>(DbConnection connection, CriteriaExpression criteriaExpression, string groupName, bool cascadingSave, bool cascadingDelete, params OrderByInfo[] orderByInfos)
      where TDetail : BusinessListBase<TDetail, TDetailBusiness>
      where TDetailBusiness : BusinessBase<TDetailBusiness>
    {
      return GetDetail<TDetail, TDetailBusiness>(connection, new Criterions(typeof(TDetail), criteriaExpression, this, groupName, cascadingSave, cascadingDelete, orderByInfos));
    }

    /// <summary>
    /// 取从业务对象集合(组合关系)
    /// </summary>
    /// <param name="connection">数据库连接</param>
    /// <param name="criteriaExpression">从业务条件表达式</param>
    /// <param name="orderByInfos">数据排列顺序队列</param>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures")]
    public TDetail GetCompositionDetail<TDetail, TDetailBusiness>(DbConnection connection, Expression<Func<TDetailBusiness, bool>> criteriaExpression, params OrderByInfo[] orderByInfos)
      where TDetail : BusinessListBase<TDetail, TDetailBusiness>
      where TDetailBusiness : BusinessBase<TDetailBusiness>
    {
      return GetCompositionDetail<TDetail, TDetailBusiness>(connection, CriteriaHelper.ToCriteriaExpression(criteriaExpression), orderByInfos);
    }

    /// <summary>
    /// 取从业务对象集合(聚合关系)
    /// </summary>
    /// <param name="connection">数据库连接</param>
    /// <param name="criteriaExpression">从业务条件表达式</param>
    /// <param name="orderByInfos">数据排列顺序队列</param>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures")]
    public TDetail GetAggregationDetail<TDetail, TDetailBusiness>(DbConnection connection, Expression<Func<TDetailBusiness, bool>> criteriaExpression, params OrderByInfo[] orderByInfos)
      where TDetail : BusinessListBase<TDetail, TDetailBusiness>
      where TDetailBusiness : BusinessBase<TDetailBusiness>
    {
      return GetAggregationDetail<TDetail, TDetailBusiness>(connection, CriteriaHelper.ToCriteriaExpression(criteriaExpression), orderByInfos);
    }

    /// <summary>
    /// 取从业务对象集合(组合关系)
    /// </summary>
    /// <param name="connection">数据库连接</param>
    /// <param name="criteriaExpression">从业务条件表达式</param>
    /// <param name="groupName">分组名</param>
    /// <param name="orderByInfos">数据排列顺序队列</param>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures")]
    public TDetail GetCompositionDetail<TDetail, TDetailBusiness>(DbConnection connection, Expression<Func<TDetailBusiness, bool>> criteriaExpression, string groupName, params OrderByInfo[] orderByInfos)
      where TDetail : BusinessListBase<TDetail, TDetailBusiness>
      where TDetailBusiness : BusinessBase<TDetailBusiness>
    {
      return GetCompositionDetail<TDetail, TDetailBusiness>(connection, CriteriaHelper.ToCriteriaExpression(criteriaExpression), groupName, orderByInfos);
    }

    /// <summary>
    /// 取从业务对象集合(聚合关系)
    /// </summary>
    /// <param name="connection">数据库连接</param>
    /// <param name="criteriaExpression">从业务条件表达式</param>
    /// <param name="groupName">分组名</param>
    /// <param name="orderByInfos">数据排列顺序队列</param>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures")]
    public TDetail GetAggregationDetail<TDetail, TDetailBusiness>(DbConnection connection, Expression<Func<TDetailBusiness, bool>> criteriaExpression, string groupName, params OrderByInfo[] orderByInfos)
      where TDetail : BusinessListBase<TDetail, TDetailBusiness>
      where TDetailBusiness : BusinessBase<TDetailBusiness>
    {
      return GetAggregationDetail<TDetail, TDetailBusiness>(connection, CriteriaHelper.ToCriteriaExpression(criteriaExpression), groupName, orderByInfos);
    }

    /// <summary>
    /// 取从业务对象集合
    /// </summary>
    /// <param name="connection">数据库连接</param>
    /// <param name="criteriaExpression">从业务条件表达式</param>
    /// <param name="groupName">分组名</param>
    /// <param name="cascadingSave">是否级联保存?</param>
    /// <param name="cascadingDelete">是否级联删除?</param>
    /// <param name="orderByInfos">数据排列顺序队列</param>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures")]
    public TDetail GetDetail<TDetail, TDetailBusiness>(DbConnection connection, Expression<Func<TDetailBusiness, bool>> criteriaExpression, string groupName, bool cascadingSave, bool cascadingDelete, params OrderByInfo[] orderByInfos)
      where TDetail : BusinessListBase<TDetail, TDetailBusiness>
      where TDetailBusiness : BusinessBase<TDetailBusiness>
    {
      return GetDetail<TDetail, TDetailBusiness>(connection, CriteriaHelper.ToCriteriaExpression(criteriaExpression), groupName, cascadingSave, cascadingDelete, orderByInfos);
    }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
    private TDetail DoGetDetail<TDetail, TDetailBusiness>(DbTransaction transaction, Criterions criterions, bool lazyFetch, bool? cascadingDelete)
      where TDetail : BusinessListBase<TDetail, TDetailBusiness>
      where TDetailBusiness : BusinessBase<TDetailBusiness>
    {
      if (criterions == null)
        criterions = new Criterions(typeof(TDetail), false, false);
      criterions.SetLink(this, cascadingDelete);

      //TDetail result = DoFindDetail<TDetail, TDetailBusiness>(criterions, cascadingDelete);
      //if (result != null)
      //  return result;

      //return IsNew ?
      //  BusinessListBase<TDetail, TDetailBusiness>.New(criterions) :
      return BusinessListBase<TDetail, TDetailBusiness>.Fetch(transaction, criterions, (Owner != null && Owner.ItemLazyGetDetail) || lazyFetch);
    }

    /// <summary>
    /// 取从业务对象集合
    /// 条件类的字段映射关系请用Phenix.Core.Mapping.CriteriaFieldAttribute标注
    /// </summary>
    /// <param name="transaction">数据库事务</param>
    /// <param name="criterions">条件集</param>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
    public TDetail GetDetail<TDetail, TDetailBusiness>(DbTransaction transaction, Criterions criterions)
      where TDetail : BusinessListBase<TDetail, TDetailBusiness>
      where TDetailBusiness : BusinessBase<TDetailBusiness>
    {
      return DoGetDetail<TDetail, TDetailBusiness>(transaction, criterions, false, null);
    }

    /// <summary>
    /// 取从业务对象集合
    /// 条件类的字段映射关系请用Phenix.Core.Mapping.CriteriaFieldAttribute标注
    /// </summary>
    /// <param name="transaction">数据库事务</param>
    /// <param name="criterions">条件集</param>
    /// <param name="lazyFetch">是否惰性Fetch(</param>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
    public TDetail GetDetail<TDetail, TDetailBusiness>(DbTransaction transaction, Criterions criterions, bool lazyFetch)
      where TDetail : BusinessListBase<TDetail, TDetailBusiness>
      where TDetailBusiness : BusinessBase<TDetailBusiness>
    {
      return DoGetDetail<TDetail, TDetailBusiness>(transaction, criterions, lazyFetch, null);
    }

    /// <summary>
    /// 取从业务对象集合
    /// </summary>
    /// <param name="transaction">数据库事务</param>
    /// <param name="orderByInfos">数据排列顺序队列</param>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
    [Obsolete("建议使用: GetCompositionDetail<TDetail, TDetailBusiness>(DbTransaction transaction, params OrderByInfo[] orderByInfos)", false)]
    public TDetail GetDetail<TDetail, TDetailBusiness>(DbTransaction transaction, params OrderByInfo[] orderByInfos)
      where TDetail : BusinessListBase<TDetail, TDetailBusiness>
      where TDetailBusiness : BusinessBase<TDetailBusiness>
    {
      return GetCompositionDetail<TDetail, TDetailBusiness>(transaction, orderByInfos);
    }

    /// <summary>
    /// 取从业务对象集合(组合关系)
    /// </summary>
    /// <param name="transaction">数据库事务</param>
    /// <param name="orderByInfos">数据排列顺序队列</param>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
    public TDetail GetCompositionDetail<TDetail, TDetailBusiness>(DbTransaction transaction, params OrderByInfo[] orderByInfos)
      where TDetail : BusinessListBase<TDetail, TDetailBusiness>
      where TDetailBusiness : BusinessBase<TDetailBusiness>
    {
      return DoGetDetail<TDetail, TDetailBusiness>(transaction, new Criterions(typeof(TDetail), false, false, orderByInfos), false, true);
    }

    /// <summary>
    /// 取从业务对象集合(聚合关系)
    /// </summary>
    /// <param name="transaction">数据库事务</param>
    /// <param name="orderByInfos">数据排列顺序队列</param>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
    public TDetail GetAggregationDetail<TDetail, TDetailBusiness>(DbTransaction transaction, params OrderByInfo[] orderByInfos)
      where TDetail : BusinessListBase<TDetail, TDetailBusiness>
      where TDetailBusiness : BusinessBase<TDetailBusiness>
    {
      return DoGetDetail<TDetail, TDetailBusiness>(transaction, new Criterions(typeof(TDetail), false, false, orderByInfos), false, false);
    }

    /// <summary>
    /// 取从业务对象集合(组合关系)
    /// </summary>
    /// <param name="transaction">数据库事务</param>
    /// <param name="groupName">分组名</param>
    /// <param name="orderByInfos">数据排列顺序队列</param>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
    public TDetail GetCompositionDetail<TDetail, TDetailBusiness>(DbTransaction transaction, string groupName, params OrderByInfo[] orderByInfos)
      where TDetail : BusinessListBase<TDetail, TDetailBusiness>
      where TDetailBusiness : BusinessBase<TDetailBusiness>
    {
      return DoGetDetail<TDetail, TDetailBusiness>(transaction, new Criterions(typeof(TDetail), this, groupName, orderByInfos), false, true);
    }

    /// <summary>
    /// 取从业务对象集合(聚合关系)
    /// </summary>
    /// <param name="transaction">数据库事务</param>
    /// <param name="groupName">分组名</param>
    /// <param name="orderByInfos">数据排列顺序队列</param>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
    public TDetail GetAggregationDetail<TDetail, TDetailBusiness>(DbTransaction transaction, string groupName, params OrderByInfo[] orderByInfos)
      where TDetail : BusinessListBase<TDetail, TDetailBusiness>
      where TDetailBusiness : BusinessBase<TDetailBusiness>
    {
      return DoGetDetail<TDetail, TDetailBusiness>(transaction, new Criterions(typeof(TDetail), this, groupName, orderByInfos), false, false);
    }

    /// <summary>
    /// 取从业务对象集合
    /// </summary>
    /// <param name="transaction">数据库事务</param>
    /// <param name="groupName">分组名</param>
    /// <param name="cascadingSave">是否级联保存?</param>
    /// <param name="cascadingDelete">是否级联删除?</param>
    /// <param name="orderByInfos">数据排列顺序队列</param>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
    public TDetail GetDetail<TDetail, TDetailBusiness>(DbTransaction transaction, string groupName, bool cascadingSave, bool cascadingDelete, params OrderByInfo[] orderByInfos)
      where TDetail : BusinessListBase<TDetail, TDetailBusiness>
      where TDetailBusiness : BusinessBase<TDetailBusiness>
    {
      return GetDetail<TDetail, TDetailBusiness>(transaction, new Criterions(typeof(TDetail), this, groupName, cascadingSave, cascadingDelete, orderByInfos));
    }

    /// <summary>
    /// 取从业务对象集合(组合关系)
    /// 条件类的字段映射关系请用Phenix.Core.Mapping.CriteriaFieldAttribute标注
    /// </summary>
    /// <param name="transaction">数据库事务</param>
    /// <param name="criteria">从业务条件对象</param>
    /// <param name="orderByInfos">数据排列顺序队列</param>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
    public TDetail GetCompositionDetail<TDetail, TDetailBusiness>(DbTransaction transaction, ICriteria criteria, params OrderByInfo[] orderByInfos)
      where TDetail : BusinessListBase<TDetail, TDetailBusiness>
      where TDetailBusiness : BusinessBase<TDetailBusiness>
    {
      return DoGetDetail<TDetail, TDetailBusiness>(transaction, new Criterions(typeof(TDetail), criteria, this, null, orderByInfos), false, true);
    }

    /// <summary>
    /// 取从业务对象集合(聚合关系)
    /// 条件类的字段映射关系请用Phenix.Core.Mapping.CriteriaFieldAttribute标注
    /// </summary>
    /// <param name="transaction">数据库事务</param>
    /// <param name="criteria">从业务条件对象</param>
    /// <param name="orderByInfos">数据排列顺序队列</param>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
    public TDetail GetAggregationDetail<TDetail, TDetailBusiness>(DbTransaction transaction, ICriteria criteria, params OrderByInfo[] orderByInfos)
      where TDetail : BusinessListBase<TDetail, TDetailBusiness>
      where TDetailBusiness : BusinessBase<TDetailBusiness>
    {
      return DoGetDetail<TDetail, TDetailBusiness>(transaction, new Criterions(typeof(TDetail), criteria, this, null, orderByInfos), false, false);
    }

    /// <summary>
    /// 取从业务对象集合(组合关系)
    /// 条件类的字段映射关系请用Phenix.Core.Mapping.CriteriaFieldAttribute标注
    /// </summary>
    /// <param name="transaction">数据库事务</param>
    /// <param name="criteria">从业务条件对象</param>
    /// <param name="groupName">分组名</param>
    /// <param name="orderByInfos">数据排列顺序队列</param>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
    public TDetail GetCompositionDetail<TDetail, TDetailBusiness>(DbTransaction transaction, ICriteria criteria, string groupName, params OrderByInfo[] orderByInfos)
      where TDetail : BusinessListBase<TDetail, TDetailBusiness>
      where TDetailBusiness : BusinessBase<TDetailBusiness>
    {
      return DoGetDetail<TDetail, TDetailBusiness>(transaction, new Criterions(typeof(TDetail), criteria, this, groupName, orderByInfos), false, true);
    }

    /// <summary>
    /// 取从业务对象集合(聚合关系)
    /// 条件类的字段映射关系请用Phenix.Core.Mapping.CriteriaFieldAttribute标注
    /// </summary>
    /// <param name="transaction">数据库事务</param>
    /// <param name="criteria">从业务条件对象</param>
    /// <param name="groupName">分组名</param>
    /// <param name="orderByInfos">数据排列顺序队列</param>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
    public TDetail GetAggregationDetail<TDetail, TDetailBusiness>(DbTransaction transaction, ICriteria criteria, string groupName, params OrderByInfo[] orderByInfos)
      where TDetail : BusinessListBase<TDetail, TDetailBusiness>
      where TDetailBusiness : BusinessBase<TDetailBusiness>
    {
      return DoGetDetail<TDetail, TDetailBusiness>(transaction, new Criterions(typeof(TDetail), criteria, this, groupName, orderByInfos), false, false);
    }

    /// <summary>
    /// 取从业务对象集合
    /// 条件类的字段映射关系请用Phenix.Core.Mapping.CriteriaFieldAttribute标注
    /// </summary>
    /// <param name="transaction">数据库事务</param>
    /// <param name="criteria">从业务条件对象</param>
    /// <param name="groupName">分组名</param>
    /// <param name="cascadingSave">是否级联保存?</param>
    /// <param name="cascadingDelete">是否级联删除?</param>
    /// <param name="orderByInfos">数据排列顺序队列</param>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
    public TDetail GetDetail<TDetail, TDetailBusiness>(DbTransaction transaction, ICriteria criteria, string groupName, bool cascadingSave, bool cascadingDelete, params OrderByInfo[] orderByInfos)
      where TDetail : BusinessListBase<TDetail, TDetailBusiness>
      where TDetailBusiness : BusinessBase<TDetailBusiness>
    {
      return GetDetail<TDetail, TDetailBusiness>(transaction, new Criterions(typeof(TDetail), criteria, this, groupName, cascadingSave, cascadingDelete, orderByInfos));
    }

    /// <summary>
    /// 取从业务对象集合(组合关系)
    /// </summary>
    /// <param name="transaction">数据库事务</param>
    /// <param name="criteriaExpression">从业务条件表达式</param>
    /// <param name="orderByInfos">数据排列顺序队列</param>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
    public TDetail GetCompositionDetail<TDetail, TDetailBusiness>(DbTransaction transaction, CriteriaExpression criteriaExpression, params OrderByInfo[] orderByInfos)
      where TDetail : BusinessListBase<TDetail, TDetailBusiness>
      where TDetailBusiness : BusinessBase<TDetailBusiness>
    {
      return DoGetDetail<TDetail, TDetailBusiness>(transaction, new Criterions(typeof(TDetail), criteriaExpression, this, null, orderByInfos), false, true);
    }

    /// <summary>
    /// 取从业务对象集合(聚合关系)
    /// </summary>
    /// <param name="transaction">数据库事务</param>
    /// <param name="criteriaExpression">从业务条件表达式</param>
    /// <param name="orderByInfos">数据排列顺序队列</param>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
    public TDetail GetAggregationDetail<TDetail, TDetailBusiness>(DbTransaction transaction, CriteriaExpression criteriaExpression, params OrderByInfo[] orderByInfos)
      where TDetail : BusinessListBase<TDetail, TDetailBusiness>
      where TDetailBusiness : BusinessBase<TDetailBusiness>
    {
      return DoGetDetail<TDetail, TDetailBusiness>(transaction, new Criterions(typeof(TDetail), criteriaExpression, this, null, orderByInfos), false, false);
    }

    /// <summary>
    /// 取从业务对象集合(组合关系)
    /// </summary>
    /// <param name="transaction">数据库事务</param>
    /// <param name="criteriaExpression">从业务条件表达式</param>
    /// <param name="groupName">分组名</param>
    /// <param name="orderByInfos">数据排列顺序队列</param>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
    public TDetail GetCompositionDetail<TDetail, TDetailBusiness>(DbTransaction transaction, CriteriaExpression criteriaExpression, string groupName, params OrderByInfo[] orderByInfos)
      where TDetail : BusinessListBase<TDetail, TDetailBusiness>
      where TDetailBusiness : BusinessBase<TDetailBusiness>
    {
      return DoGetDetail<TDetail, TDetailBusiness>(transaction, new Criterions(typeof(TDetail), criteriaExpression, this, groupName, orderByInfos), false, true);
    }

    /// <summary>
    /// 取从业务对象集合(聚合关系)
    /// </summary>
    /// <param name="transaction">数据库事务</param>
    /// <param name="criteriaExpression">从业务条件表达式</param>
    /// <param name="groupName">分组名</param>
    /// <param name="orderByInfos">数据排列顺序队列</param>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
    public TDetail GetAggregationDetail<TDetail, TDetailBusiness>(DbTransaction transaction, CriteriaExpression criteriaExpression, string groupName, params OrderByInfo[] orderByInfos)
      where TDetail : BusinessListBase<TDetail, TDetailBusiness>
      where TDetailBusiness : BusinessBase<TDetailBusiness>
    {
      return DoGetDetail<TDetail, TDetailBusiness>(transaction, new Criterions(typeof(TDetail), criteriaExpression, this, groupName, orderByInfos), false, false);
    }

    /// <summary>
    /// 取从业务对象集合
    /// </summary>
    /// <param name="transaction">数据库事务</param>
    /// <param name="criteriaExpression">从业务条件表达式</param>
    /// <param name="groupName">分组名</param>
    /// <param name="cascadingSave">是否级联保存?</param>
    /// <param name="cascadingDelete">是否级联删除?</param>
    /// <param name="orderByInfos">数据排列顺序队列</param>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
    public TDetail GetDetail<TDetail, TDetailBusiness>(DbTransaction transaction, CriteriaExpression criteriaExpression, string groupName, bool cascadingSave, bool cascadingDelete, params OrderByInfo[] orderByInfos)
      where TDetail : BusinessListBase<TDetail, TDetailBusiness>
      where TDetailBusiness : BusinessBase<TDetailBusiness>
    {
      return GetDetail<TDetail, TDetailBusiness>(transaction, new Criterions(typeof(TDetail), criteriaExpression, this, groupName, cascadingSave, cascadingDelete, orderByInfos));
    }

    /// <summary>
    /// 取从业务对象集合(组合关系)
    /// </summary>
    /// <param name="transaction">数据库事务</param>
    /// <param name="criteriaExpression">从业务条件表达式</param>
    /// <param name="orderByInfos">数据排列顺序队列</param>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures")]
    public TDetail GetCompositionDetail<TDetail, TDetailBusiness>(DbTransaction transaction, Expression<Func<TDetailBusiness, bool>> criteriaExpression, params OrderByInfo[] orderByInfos)
      where TDetail : BusinessListBase<TDetail, TDetailBusiness>
      where TDetailBusiness : BusinessBase<TDetailBusiness>
    {
      return GetCompositionDetail<TDetail, TDetailBusiness>(transaction, CriteriaHelper.ToCriteriaExpression(criteriaExpression), orderByInfos);
    }

    /// <summary>
    /// 取从业务对象集合(聚合关系)
    /// </summary>
    /// <param name="transaction">数据库事务</param>
    /// <param name="criteriaExpression">从业务条件表达式</param>
    /// <param name="orderByInfos">数据排列顺序队列</param>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures")]
    public TDetail GetAggregationDetail<TDetail, TDetailBusiness>(DbTransaction transaction, Expression<Func<TDetailBusiness, bool>> criteriaExpression, params OrderByInfo[] orderByInfos)
      where TDetail : BusinessListBase<TDetail, TDetailBusiness>
      where TDetailBusiness : BusinessBase<TDetailBusiness>
    {
      return GetAggregationDetail<TDetail, TDetailBusiness>(transaction, CriteriaHelper.ToCriteriaExpression(criteriaExpression), orderByInfos);
    }

    /// <summary>
    /// 取从业务对象集合(组合关系)
    /// </summary>
    /// <param name="transaction">数据库事务</param>
    /// <param name="criteriaExpression">从业务条件表达式</param>
    /// <param name="groupName">分组名</param>
    /// <param name="orderByInfos">数据排列顺序队列</param>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures")]
    public TDetail GetCompositionDetail<TDetail, TDetailBusiness>(DbTransaction transaction, Expression<Func<TDetailBusiness, bool>> criteriaExpression, string groupName, params OrderByInfo[] orderByInfos)
      where TDetail : BusinessListBase<TDetail, TDetailBusiness>
      where TDetailBusiness : BusinessBase<TDetailBusiness>
    {
      return GetCompositionDetail<TDetail, TDetailBusiness>(transaction, CriteriaHelper.ToCriteriaExpression(criteriaExpression), groupName, orderByInfos);
    }
    
    /// <summary>
    /// 取从业务对象集合(聚合关系)
    /// </summary>
    /// <param name="transaction">数据库事务</param>
    /// <param name="criteriaExpression">从业务条件表达式</param>
    /// <param name="groupName">分组名</param>
    /// <param name="orderByInfos">数据排列顺序队列</param>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures")]
    public TDetail GetAggregationDetail<TDetail, TDetailBusiness>(DbTransaction transaction, Expression<Func<TDetailBusiness, bool>> criteriaExpression, string groupName, params OrderByInfo[] orderByInfos)
      where TDetail : BusinessListBase<TDetail, TDetailBusiness>
      where TDetailBusiness : BusinessBase<TDetailBusiness>
    {
      return GetAggregationDetail<TDetail, TDetailBusiness>(transaction, CriteriaHelper.ToCriteriaExpression(criteriaExpression), groupName, orderByInfos);
    }

    /// <summary>
    /// 取从业务对象集合
    /// </summary>
    /// <param name="transaction">数据库事务</param>
    /// <param name="criteriaExpression">从业务条件表达式</param>
    /// <param name="groupName">分组名</param>
    /// <param name="cascadingSave">是否级联保存?</param>
    /// <param name="cascadingDelete">是否级联删除?</param>
    /// <param name="orderByInfos">数据排列顺序队列</param>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures")]
    public TDetail GetDetail<TDetail, TDetailBusiness>(DbTransaction transaction, Expression<Func<TDetailBusiness, bool>> criteriaExpression, string groupName, bool cascadingSave, bool cascadingDelete, params OrderByInfo[] orderByInfos)
      where TDetail : BusinessListBase<TDetail, TDetailBusiness>
      where TDetailBusiness : BusinessBase<TDetailBusiness>
    {
      return GetDetail<TDetail, TDetailBusiness>(transaction, CriteriaHelper.ToCriteriaExpression(criteriaExpression), groupName, cascadingSave, cascadingDelete, orderByInfos);
    }

    /// <summary>
    /// 取从业务对象集合
    /// </summary>
    /// <param name="source">过滤(克隆)数据源</param>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
    [Obsolete("建议使用: GetCompositionDetail<TDetail, TDetailBusiness>(TDetail source)", false)]
    public TDetail GetDetail<TDetail, TDetailBusiness>(TDetail source)
      where TDetail : BusinessListBase<TDetail, TDetailBusiness>
      where TDetailBusiness : BusinessBase<TDetailBusiness>
    {
      return GetCompositionDetail<TDetail, TDetailBusiness>(source);
    }

    /// <summary>
    /// 取从业务对象集合(组合关系)
    /// </summary>
    /// <param name="source">过滤(克隆)数据源</param>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
    public TDetail GetCompositionDetail<TDetail, TDetailBusiness>(TDetail source)
      where TDetail : BusinessListBase<TDetail, TDetailBusiness>
      where TDetailBusiness : BusinessBase<TDetailBusiness>
    {
      return source != null ? source.CompositionFilter(this) : GetCompositionDetail<TDetail, TDetailBusiness>();
    }

    /// <summary>
    /// 取从业务对象集合(聚合关系)
    /// </summary>
    /// <param name="source">过滤(克隆)数据源</param>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
    public TDetail GetAggregationDetail<TDetail, TDetailBusiness>(TDetail source)
      where TDetail : BusinessListBase<TDetail, TDetailBusiness>
      where TDetailBusiness : BusinessBase<TDetailBusiness>
    {
      return source != null ? source.AggregationFilter(this) : GetAggregationDetail<TDetail, TDetailBusiness>();
    }

    /// <summary>
    /// 取从业务对象集合(组合关系)
    /// </summary>
    /// <param name="source">过滤(克隆)数据源</param>
    /// <param name="groupName">分组名</param>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
    public TDetail GetCompositionDetail<TDetail, TDetailBusiness>(TDetail source, string groupName)
      where TDetail : BusinessListBase<TDetail, TDetailBusiness>
      where TDetailBusiness : BusinessBase<TDetailBusiness>
    {
      return source != null ? source.CompositionFilter(this, groupName) : GetCompositionDetail<TDetail, TDetailBusiness>(groupName);
    }

    /// <summary>
    /// 取从业务对象集合(聚合关系)
    /// </summary>
    /// <param name="source">过滤(克隆)数据源</param>
    /// <param name="groupName">分组名</param>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
    public TDetail GetAggregationDetail<TDetail, TDetailBusiness>(TDetail source, string groupName)
      where TDetail : BusinessListBase<TDetail, TDetailBusiness>
      where TDetailBusiness : BusinessBase<TDetailBusiness>
    {
      return source != null ? source.AggregationFilter(this, groupName) : GetAggregationDetail<TDetail, TDetailBusiness>(groupName);
    }

    /// <summary>
    /// 取从业务对象集合
    /// </summary>
    /// <param name="source">过滤(克隆)数据源</param>
    /// <param name="groupName">分组名</param>
    /// <param name="cascadingSave">是否级联保存?</param>
    /// <param name="cascadingDelete">是否级联删除?</param>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
    public TDetail GetDetail<TDetail, TDetailBusiness>(TDetail source, string groupName, bool cascadingSave, bool cascadingDelete)
      where TDetail : BusinessListBase<TDetail, TDetailBusiness>
      where TDetailBusiness : BusinessBase<TDetailBusiness>
    {
      return source != null ? source.Filter(this, groupName, cascadingSave, cascadingDelete) : GetDetail<TDetail, TDetailBusiness>(groupName, cascadingSave, cascadingDelete);
    }

    /// <summary>
    /// 取从业务对象集合(组合关系)
    /// </summary>
    /// <param name="expression">条件表达式</param>
    /// <param name="source">过滤(克隆)数据源</param>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures")]
    public TDetail GetCompositionDetail<TDetail, TDetailBusiness>(Expression<Func<TDetailBusiness, bool>> expression, TDetail source)
      where TDetail : BusinessListBase<TDetail, TDetailBusiness>
      where TDetailBusiness : BusinessBase<TDetailBusiness>
    {
      return source != null ? source.CompositionFilter(expression, this) : GetCompositionDetail<TDetail, TDetailBusiness>(expression);
    }

    /// <summary>
    /// 取从业务对象集合(聚合关系)
    /// </summary>
    /// <param name="expression">条件表达式</param>
    /// <param name="source">过滤(克隆)数据源</param>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures")]
    public TDetail GetAggregationDetail<TDetail, TDetailBusiness>(Expression<Func<TDetailBusiness, bool>> expression, TDetail source)
      where TDetail : BusinessListBase<TDetail, TDetailBusiness>
      where TDetailBusiness : BusinessBase<TDetailBusiness>
    {
      return source != null ? source.AggregationFilter(expression, this) : GetAggregationDetail<TDetail, TDetailBusiness>(expression);
    }

    /// <summary>
    /// 取从业务对象集合(组合关系)
    /// </summary>
    /// <param name="expression">条件表达式</param>
    /// <param name="source">过滤(克隆)数据源</param>
    /// <param name="groupName">分组名</param>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures")]
    public TDetail GetCompositionDetail<TDetail, TDetailBusiness>(Expression<Func<TDetailBusiness, bool>> expression, TDetail source, string groupName)
      where TDetail : BusinessListBase<TDetail, TDetailBusiness>
      where TDetailBusiness : BusinessBase<TDetailBusiness>
    {
      return source != null ? source.CompositionFilter(expression, this, groupName) : GetCompositionDetail<TDetail, TDetailBusiness>(expression, groupName);
    }

    /// <summary>
    /// 取从业务对象集合(聚合关系)
    /// </summary>
    /// <param name="expression">条件表达式</param>
    /// <param name="source">过滤(克隆)数据源</param>
    /// <param name="groupName">分组名</param>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures")]
    public TDetail GetAggregationDetail<TDetail, TDetailBusiness>(Expression<Func<TDetailBusiness, bool>> expression, TDetail source, string groupName)
      where TDetail : BusinessListBase<TDetail, TDetailBusiness>
      where TDetailBusiness : BusinessBase<TDetailBusiness>
    {
      return source != null ? source.AggregationFilter(expression, this, groupName) : GetAggregationDetail<TDetail, TDetailBusiness>(expression, groupName);
    }

    /// <summary>
    /// 取从业务对象集合
    /// </summary>
    /// <param name="expression">条件表达式</param>
    /// <param name="source">过滤(克隆)数据源</param>
    /// <param name="groupName">分组名</param>
    /// <param name="cascadingSave">是否级联保存?</param>
    /// <param name="cascadingDelete">是否级联删除?</param>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures")]
    public TDetail GetDetail<TDetail, TDetailBusiness>(Expression<Func<TDetailBusiness, bool>> expression, TDetail source, string groupName, bool cascadingSave, bool cascadingDelete)
      where TDetail : BusinessListBase<TDetail, TDetailBusiness>
      where TDetailBusiness : BusinessBase<TDetailBusiness>
    {
      return source != null ? source.Filter(expression, this, groupName, cascadingSave, cascadingDelete) : GetDetail<TDetail, TDetailBusiness>(expression, groupName, cascadingSave, cascadingDelete);
    }

    #endregion

    #region 编辑控制

    private bool GetIsDirty(ref List<IRefinedlyObject> ignoreLinks)
    {
      if (IsSelfDirty)
        return true;

      if (ignoreLinks.Contains(this))
        return false;
      if (_details != null && _details.Count > 0)
        foreach (KeyValuePair<string, IBusiness> kvp in _details)
          if (((IRefinedly)kvp.Value).GetIsDirty(ref ignoreLinks))
            return true;
      if (_links != null && _links.Count > 0 && NeedSaveLinks)
      {
        ignoreLinks.Add(this);
        foreach (KeyValuePair<string, IBusinessObject> kvp in _links)
          if (kvp.Value != null && kvp.Value != MasterBusiness && ((IRefinedly)kvp.Value).GetIsDirty(ref ignoreLinks))
            return true;
      }
      return false;
    }
    bool IRefinedly.GetIsDirty(ref List<IRefinedlyObject> ignoreLinks)
    {
      return GetIsDirty(ref ignoreLinks);
    }

    private bool GetIsValid(ref List<IRefinedlyObject> ignoreLinks)
    {
      if (!base.IsValid)
        return false;

      if (ignoreLinks.Contains(this))
        return true;
      if (_details != null && _details.Count > 0)
        foreach (KeyValuePair<string, IBusiness> kvp in _details)
          if (!((IRefinedly)kvp.Value).GetIsValid(ref ignoreLinks))
            return false;
      if (_links != null && _links.Count > 0 && NeedSaveLinks)
      {
        ignoreLinks.Add(this);
        foreach (KeyValuePair<string, IBusinessObject> kvp in _links)
          if (kvp.Value != null && kvp.Value != MasterBusiness && !((IRefinedly)kvp.Value).GetIsValid(ref ignoreLinks))
            return false;
      }
      return true;
    }
    bool IRefinedly.GetIsValid(ref List<IRefinedlyObject> ignoreLinks)
    {
      return GetIsValid(ref ignoreLinks);
    }

    private bool GetNeedRefresh(ref List<IRefinedlyObject> ignoreLinks)
    {
      if (NeedRefreshSelf)
        return true;

      if (ignoreLinks.Contains(this))
        return false;
      if (_details != null && _details.Count > 0)
        foreach (KeyValuePair<string, IBusiness> kvp in _details)
          if (((IRefinedly)kvp.Value).GetNeedRefresh(ref ignoreLinks))
            return true;
      if (_links != null && _links.Count > 0 && NeedSaveLinks)
      {
        ignoreLinks.Add(this);
        foreach (KeyValuePair<string, IBusinessObject> kvp in _links)
          if (kvp.Value != null && kvp.Value != MasterBusiness && ((IRefinedly)kvp.Value).GetNeedRefresh(ref ignoreLinks))
            return true;
      }
      return false;
    }
    bool IRefinedly.GetNeedRefresh(ref List<IRefinedlyObject> ignoreLinks)
    {
      return GetNeedRefresh(ref ignoreLinks);
    }

    private void SynchronizeEditLevel(IRefinedly source)
    {
      IBusiness business = (IBusiness)source;
      if (business.EditLevel < EditLevel)
        throw new Csla.Core.UndoException(String.Format("{0}({1}): EditLevel={2}应该<={3}!", this.GetType().FullName, PrimaryKey, EditLevel, business.EditLevel));
      for (int i = 0; i < business.EditLevel - EditLevel; i++)
        BeginEdit();
    }
    void IRefinedly.SynchronizeEditLevel(IRefinedly source)
    {
      SynchronizeEditLevel(source);
    }

    internal static void CheckBusinessObjectMember()
    {
      Type type = typeof(T);
      string message = String.Empty;
      foreach (FieldInfo item in Utilities.GetInstanceFields(type, typeof(IBusiness), typeof(IBusiness)))
        if (Attribute.GetCustomAttribute(item, typeof(Csla.NotUndoableAttribute)) == null)
          message = message + String.Format("请为{0}.{1}字段标记上{2}标签",
            (item.DeclaringType ?? type).FullName, item.Name, typeof(Csla.NotUndoableAttribute).FullName) + AppConfig.CR_LF;
      if (!String.IsNullOrEmpty(message))
      {
        Csla.Core.UndoException undoException = new Csla.Core.UndoException(message);
        EventLog.Save(type, undoException);
        throw undoException;
      }
    }

    /// <summary>
    /// 开始编辑
    /// </summary>
    public new void BeginEdit()
    {
      List<IRefinedlyObject> ignoreLinks = new List<IRefinedlyObject>();
      BeginEdit(EditLevel + 1, ref ignoreLinks);
    }

    private void BeginEdit(int editLevel, ref List<IRefinedlyObject> ignoreLinks)
    {
      if (!NotUndoable)
      {
        while (editLevel > EditLevel)
        {
          int oldEditLevel = EditLevel;
          try
          {
            base.BeginEdit();
          }
          catch (Csla.Core.UndoException ex)
          {
            CheckBusinessObjectMember();
            if (EditLevel == oldEditLevel)
            {
              EventLog.Save(this.GetType(), ex);
              throw;
            }
          }
        }
      }
      BeginEditInDetailsAndLinks(editLevel, ref ignoreLinks);
    }
    void IRefinedly.BeginEdit(int editLevel, ref List<IRefinedlyObject> ignoreLinks)
    {
      BeginEdit(editLevel, ref ignoreLinks);
    }

    internal void BeginEditInDetailsAndLinks(int editLevel, ref List<IRefinedlyObject> ignoreLinks)
    {
      if (ignoreLinks.Contains(this))
        return;
      if (_details != null && _details.Count > 0)
        foreach (KeyValuePair<string, IBusiness> kvp in _details)
          ((IRefinedly)kvp.Value).BeginEdit(editLevel, ref ignoreLinks);
      if (_links != null && _links.Count > 0 && NeedSaveLinks)
      {
        ignoreLinks.Add(this);
        foreach (KeyValuePair<string, IBusinessObject> kvp in _links)
          if (kvp.Value != null && kvp.Value != MasterBusiness)
            ((IRefinedly)kvp.Value).BeginEdit(editLevel, ref ignoreLinks);
      }
    }

    /// <summary>
    /// 取消编辑
    /// </summary>
    public new void CancelEdit()
    {
      List<IRefinedlyObject> ignoreLinks = new List<IRefinedlyObject>();
      CancelEdit(EditLevel - 1, ref ignoreLinks);
    }

    internal bool CancelEdit(int editLevel, ref List<IRefinedlyObject> ignoreLinks)
    {
      bool result = false;
      if (editLevel >= 0)
        while (editLevel < EditLevel)
        {
          int oldEditLevel = EditLevel;
          try
          {
            base.CancelEdit();
            result = true;
          }
          catch (Csla.Core.UndoException ex)
          {
            if (EditLevel == oldEditLevel)
            {
              EventLog.Save(this.GetType(), ex);
              throw;
            }
          }
        }
      if (editLevel <= 0)
      {
        if (IsNew)
        {
          if (FillFieldValuesToDefault(true))
          {
            PropertyHasChanged();
            result = true;
          }
          if (IsSelfDeleted)
            MarkNewDirty();
        }
        else if (IsSelfDirty)
        {
          if (EntityHelper.FillFieldValues(OldFieldValues, this, true))
          {
            PropertyHasChanged();
            result = true;
          }
          MarkFetched();
          MarkClean();
        }
      }
      if (CancelEditInDetailsAndLinks(editLevel, ref ignoreLinks))
        result = true;
      return result;
    }
    bool IRefinedly.CancelEdit(int editLevel, ref List<IRefinedlyObject> ignoreLinks)
    {
      return CancelEdit(editLevel, ref ignoreLinks);
    }

    internal bool CancelEditInDetailsAndLinks(int editLevel, ref List<IRefinedlyObject> ignoreLinks)
    {
      if (ignoreLinks.Contains(this))
        return false;
      bool result = false;
      if (_details != null && _details.Count > 0)
        foreach (KeyValuePair<string, IBusiness> kvp in _details)
          if (((IRefinedly)kvp.Value).CancelEdit(editLevel, ref ignoreLinks))
            result = true;
      if (_links != null && _links.Count > 0 && NeedSaveLinks)
      {
        ignoreLinks.Add(this);
        foreach (KeyValuePair<string, IBusinessObject> kvp in _links)
          if (kvp.Value != null && kvp.Value != MasterBusiness)
            if (((IRefinedly)kvp.Value).CancelEdit(editLevel, ref ignoreLinks))
              result = true;
      }
      return result;
    }

    /// <summary>
    /// 接受编辑
    /// </summary>
    public new void ApplyEdit()
    {
      List<IRefinedlyObject> ignoreLinks = new List<IRefinedlyObject>();
      ApplyEdit(EditLevel - 1, ref ignoreLinks);
    }

    internal void ApplyEdit(int editLevel, ref List<IRefinedlyObject> ignoreLinks)
    {
      if (editLevel >= 0)
        while (editLevel < EditLevel)
        {
          int oldEditLevel = EditLevel;
          try
          {
            base.ApplyEdit();
          }
          catch (Csla.Core.UndoException ex)
          {
            if (EditLevel == oldEditLevel)
            {
              EventLog.Save(this.GetType(), ex);
              throw;
            }
          }
        }
      if (editLevel <= 0)
        MarkFetched();
      ApplyEditInDetailsAndLinks(editLevel, ref ignoreLinks);
    }
    void IRefinedly.ApplyEdit(int editLevel, ref List<IRefinedlyObject> ignoreLinks)
    {
      ApplyEdit(editLevel, ref ignoreLinks);
    }

    internal void ApplyEditInDetailsAndLinks(int editLevel, ref List<IRefinedlyObject> ignoreLinks)
    {
      if (ignoreLinks.Contains(this))
        return;
      if (_details != null && _details.Count > 0)
        foreach (KeyValuePair<string, IBusiness> kvp in _details)
          ((IRefinedly)kvp.Value).ApplyEdit(editLevel, ref ignoreLinks);
      if (_links != null && _links.Count > 0 && NeedSaveLinks)
      {
        ignoreLinks.Add(this);
        foreach (KeyValuePair<string, IBusinessObject> kvp in _links)
          if (kvp.Value != null && kvp.Value != MasterBusiness)
            ((IRefinedly)kvp.Value).ApplyEdit(editLevel, ref ignoreLinks);
      }
    }

    private void CompletelyApplyEdit(bool toMarkOld, bool inCascadingDelete, bool needCheckDirty, bool keepEnsemble, ref List<IRefinedlyObject> ignoreLinks)
    {
      if (ignoreLinks.Contains(this))
        return;

      if (!toMarkOld)
        CheckSelfRules(false, false, true);

      while (EditLevel > 0)
      {
        int oldEditLevel = EditLevel;
        try
        {
          base.ApplyEdit();
        }
        catch (Csla.Core.UndoException ex)
        {
          if (EditLevel == oldEditLevel)
          {
            EventLog.Save(this.GetType(), ex);
            throw;
          }
        }
      }

      if (_details != null && _details.Count > 0)
      {
        IBusiness[] array = new IBusiness[_details.Count];
        _details.Values.CopyTo(array, 0);
        foreach (IRefinedly item in array)
          item.CompletelyApplyEdit(toMarkOld, inCascadingDelete, needCheckDirty, keepEnsemble || EnsembleOnSaving, ref ignoreLinks);
      }
      if (_links != null && _links.Count > 0 && NeedSaveLinks)
      {
        ignoreLinks.Add(this);
        IBusinessObject[] array = new IBusinessObject[_links.Count];
        _links.Values.CopyTo(array, 0);
        foreach (IRefinedly item in array)
          if (item != null && item != MasterBusiness)
            item.CompletelyApplyEdit(toMarkOld, false, needCheckDirty, keepEnsemble || EnsembleOnSaving, ref ignoreLinks);
      }

      if (toMarkOld)
      {
        MarkOld(IsSelfDirty || (Owner != null && Owner.OnlySaveSelected && Selected));
        if (Owner != null)
          for (int i = 0; i < Owner.EditLevel - EditLevel; i++)
            BeginEdit();
      }
      else
      {
        ClearFieldManagerFieldInfo();
        NeedCheckDirty = needCheckDirty;
      }
    }
    void IRefinedly.CompletelyApplyEdit(bool toMarkOld, bool inCascadingDelete, bool needCheckDirty, bool keepEnsemble, ref List<IRefinedlyObject> ignoreLinks)
    {
      CompletelyApplyEdit(toMarkOld, inCascadingDelete, needCheckDirty, keepEnsemble, ref ignoreLinks);
    }

    #endregion
    
    #region DefaultValue

    /// <summary>
    /// 填充字段值到缺省值
    /// </summary>
    /// <param name="reset">重新设定</param>
    public override bool FillFieldValuesToDefault(bool reset)
    {
      bool result = false;
      if (Owner != null && Owner.Criterions != null && Owner.Criterions.ItselfBusiness != null)
        if (EntityHelper.FillFieldValues(Owner.Criterions.ItselfBusiness, this, true, true))
        {
          MarkDirty();
          result = true;
        }
      if (base.FillFieldValuesToDefault(reset))
         result = true;
      return result;
    }

    private void FillWatermarkFieldValues(bool reset)
    {
      bool changed = EntityHelper.FillPrimaryKeyFieldValues(this, reset);
      InitOldFieldValues(false, reset);
      if (EntityHelper.FillRequiredFieldValues(this, IsNew))
        changed = true;
      if (reset || NeedFillBusinessCodeFieldValues)
        if (EntityHelper.FillBusinessCodeFieldValues(this, null, ref _semisBusinessCodes, reset))
          changed = true;
      if (changed)
      {
        MarkDirty();
        CascadingLinkToInDetails(this, null);
        CascadingFillIdenticalValuesInDetailsAndLinks(this);
      }
    }

    #endregion

    #region CascadingRefresh

    internal bool CascadingRefresh(T source)
    {
      if (object.ReferenceEquals(source, null) || object.ReferenceEquals(source, this))
        return false;

      bool result = false;
      if (NeedRefreshSelf || source.NeedRefreshSelf)
      {
        if (EntityHelper.FillFieldValues(source, this, true, true))
        {
          MarkOld(true);
          result = true;
        }
        NeedRefreshSelf = false;
        source.NeedRefreshSelf = false;
      }

      if (source._details != null && source._details.Count > 0 && _details != null && _details.Count > 0)
        foreach (KeyValuePair<string, IBusiness> kvp in source._details)
          if (kvp.Value.NeedRefresh)
          {
            IBusiness detail;
            if (_details.TryGetValue(kvp.Key, out detail))
              if (((IRefinedly)detail).CascadingRefresh((IRefinedly)kvp.Value))
                result = true;
          }
      if (source._links != null && source._links.Count > 0 && _links != null && _links.Count > 0 && NeedSaveLinks)
        foreach (KeyValuePair<string, IBusinessObject> kvp in source._links)
          if (kvp.Value != null && kvp.Value.NeedRefresh)
          {
            IBusinessObject link;
            if (_links.TryGetValue(kvp.Key, out link) && link != null)
              if (((IRefinedly)link).CascadingRefresh((IRefinedly)kvp.Value))
                result = true;
          }
      return result;
    }
    private bool CascadingRefresh(IRefinedly source)
    {
      return CascadingRefresh((T)source);
    }
    bool IRefinedly.CascadingRefresh(IRefinedly source)
    {
      return CascadingRefresh(source);
    }

    #endregion

    #region CascadingLinkTo

    private void CascadingLinkTo(object link, string groupName, bool throwIfNotFound)
    {
      CascadingLinkToInDetails(link, groupName);
      InitOldFieldValues(false, false);
      bool changed = EntityHelper.FillIdenticalFieldValuesBySourceProperty(link, this, false);
      if (EntityLinkHelper.LinkTo(this, link, groupName ?? (link == MasterBusiness ? GroupName : null), throwIfNotFound))
        MarkDirty();
      else if (changed)
        MarkDirty();
    }
    void IRefinedly.CascadingLinkTo(object link, string groupName, bool throwIfNotFound)
    {
      CascadingLinkTo(link, groupName, throwIfNotFound);
    }

    private void CascadingLinkToInDetails(object link, string groupName)
    {
      if (_details != null && _details.Count > 0)
      {
        IBusiness[] array = new IBusiness[_details.Count];
        _details.Values.CopyTo(array, 0);
        _details.Clear();
        foreach (IBusiness item in array)
        {
          if (String.CompareOrdinal(Utilities.GetCoreType(item.GetType()).FullName, link.GetType().FullName) != 0)
            ((IRefinedly)item).CascadingLinkTo(link, groupName ?? (link == item.MasterBusiness ? item.GroupName : null), false);
          _details[item.Criterions.ToString()] = item;
        }
        _sortedDetails = null;
      }
    }

    #endregion

    #region CascadingUnlink

    private void CascadingUnlink(object link, string groupName, bool throwIfNotFound)
    {
      CascadingUnlinkInDetails(link, groupName);
      InitOldFieldValues(false, false);
      if (EntityLinkHelper.Unlink(this, link, groupName ?? (link == MasterBusiness ? GroupName : null), throwIfNotFound))
        MarkDirty();
    }
    void IRefinedly.CascadingUnlink(object link, string groupName, bool throwIfNotFound)
    {
      CascadingUnlink(link, groupName, throwIfNotFound);
    }

    private void CascadingUnlinkInDetails(object link, string groupName)
    {
      if (_details != null && _details.Count > 0)
      {
        IBusiness[] array = new IBusiness[_details.Count];
        _details.Values.CopyTo(array, 0);
        _details.Clear();
        foreach (IBusiness item in array)
        {
          if (String.CompareOrdinal(Utilities.GetCoreType(item.GetType()).FullName, link.GetType().FullName) != 0)
            ((IRefinedly)item).CascadingUnlink(link, groupName ?? (link == item.MasterBusiness ? item.GroupName : null), false);
          _details[item.Criterions.ToString()] = item;
        }
        _sortedDetails = null;
      }
    }

    #endregion

    #region CascadingExistLink

    private bool CascadingExistLink(Type masterType, string groupName)
    {
      if (ClassMemberHelper.ExistLink(this.GetType(), masterType, groupName))
        return true;
      return CascadingExistLinkInDetails(masterType, groupName);
    }
    bool IRefinedly.CascadingExistLink(Type masterType, string groupName)
    {
      return CascadingExistLink(masterType, groupName);
    }

    private bool CascadingExistLinkInDetails(Type masterType, string groupName)
    {
      if (_details != null && _details.Count > 0)
        foreach (KeyValuePair<string, IBusiness> kvp in _details)
          if (((IRefinedly)kvp.Value).CascadingExistLink(masterType, groupName))
            return true;
      return false;
    }

    #endregion

    #region CascadingFillIdenticalValues

    private void CascadingFillIdenticalValues(IBusinessObject source)
    {
      List<IRefinedlyObject> ignoreLinks = new List<IRefinedlyObject>();
      ignoreLinks.Add((IRefinedlyObject)source);
      CascadingFillIdenticalValues((IRefinedlyObject)source, null, ref ignoreLinks);
    }

    private void CascadingFillIdenticalValues(IRefinedlyObject source, string[] sourcePropertyNames, ref List<IRefinedlyObject> ignoreLinks)
    {
      if (ignoreLinks.Contains(this))
        return;
      CascadingFillIdenticalValuesInDetailsAndLinks((IBusinessObject)source, sourcePropertyNames, ref ignoreLinks);
      InitOldFieldValues(false, false);
      if (EntityHelper.FillIdenticalFieldValuesBySourceProperty(source, this, false, sourcePropertyNames))
        MarkDirty();
    }
    void IRefinedly.CascadingFillIdenticalValues(IRefinedlyObject source, string[] sourcePropertyNames, ref List<IRefinedlyObject> ignoreLinks)
    {
      CascadingFillIdenticalValues(source, sourcePropertyNames, ref ignoreLinks);
    }

    private void CascadingFillIdenticalValuesInDetailsAndLinks(IBusinessObject source, string[] sourcePropertyNames, ref List<IRefinedlyObject> ignoreLinks)
    {
      if (_details != null && _details.Count > 0)
        foreach (KeyValuePair<string, IBusiness> kvp in _details)
          ((IRefinedly)kvp.Value).CascadingFillIdenticalValues((IRefinedlyObject)source, sourcePropertyNames, ref ignoreLinks);
      if (_links != null && _links.Count > 0 && NeedSaveLinks)
      {
        ignoreLinks.Add(this);
        foreach (KeyValuePair<string, IBusinessObject> kvp in _links)
          if (kvp.Value != null && kvp.Value != MasterBusiness)
            ((IRefinedly)kvp.Value).CascadingFillIdenticalValues((IRefinedlyObject)source, sourcePropertyNames, ref ignoreLinks);
      }
    }

    private void CascadingFillIdenticalValuesInDetailsAndLinks(IBusinessObject source, params string[] sourcePropertyNames)
    {
      List<IRefinedlyObject> ignoreLinks = new List<IRefinedlyObject>();
      ignoreLinks.Add((IRefinedlyObject)source);
      CascadingFillIdenticalValuesInDetailsAndLinks(source, sourcePropertyNames, ref ignoreLinks);
    }

    #endregion

    #region FillAggregateValues

    /// <summary>
    /// 计算聚合字段
    /// property = null
    /// </summary>
    public void ComputeMasterAggregate()
    {
      ComputeMasterAggregate((IPropertyInfo)null);
    }

    /// <summary>
    /// 计算聚合字段
    /// </summary>
    /// <param name="propertyInfo">属性信息</param>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters")]
    public void ComputeMasterAggregate(IPropertyInfo propertyInfo)
    {
      if (MasterBusiness == null)
        throw new InvalidOperationException(String.Format("对象{0}没有主业务对象MasterBusiness属性为空", Caption));
      if (propertyInfo != null)
      {
        CodingStandards.CheckFieldMapInfo(propertyInfo);

        if (propertyInfo.FieldMapInfo.FieldAggregateMapInfos.Count == 0)
          throw new InvalidOperationException(String.Format("请为{0}.{1}字段标记上{2}标签",
            typeof(T).FullName, propertyInfo.FieldMapInfo.Field.Name, typeof(FieldAggregateAttribute).FullName));
        foreach (FieldAggregateMapInfo item in propertyInfo.FieldMapInfo.FieldAggregateMapInfos)
          ((IRefinedlyObject)MasterBusiness).FillAggregateValues(item);
      }
      else
      {
        foreach (FieldAggregateMapInfo item in  ClassMemberHelper.GetFieldAggregateMapInfos(this.GetType(), true))
          ((IRefinedlyObject)MasterBusiness).FillAggregateValues(item);
      }
    }

    /// <summary>
    /// 计算聚合字段
    /// </summary>
    /// <param name="propertyName">属性名</param>
    public void ComputeMasterAggregate(string propertyName)
    {
      if (MasterBusiness == null)
        throw new InvalidOperationException(String.Format("对象{0}没有主业务对象MasterBusiness属性为空", Caption));
      if (String.IsNullOrEmpty(propertyName))
      {
        foreach (FieldAggregateMapInfo item in ClassMemberHelper.GetFieldAggregateMapInfos(this.GetType(), true))
          ((IRefinedlyObject)MasterBusiness).FillAggregateValues(item);
      }
      else
      {
        bool find = false;
        foreach (FieldAggregateMapInfo item in ClassMemberHelper.GetFieldAggregateMapInfos(this.GetType(), true))
          if (String.CompareOrdinal(item.Owner.PropertyName, propertyName) == 0)
          {
            find = true;
            ((IRefinedlyObject)MasterBusiness).FillAggregateValues(item);
          }
        if (!find)
          throw new InvalidOperationException(String.Format("请为{0}.{1}属性关联的字段标记上{2}标签",
            typeof(T).FullName, propertyName, typeof(FieldAggregateAttribute).FullName));
      }
    }

    private void FillAggregateValues(FieldAggregateMapInfo fieldAggregateMapInfo)
    {
      if (_details != null && _details.Count > 0)
      {
        List<object> source = new List<object>();
        foreach (KeyValuePair<string, IBusiness> kvp in _details)
          if (String.CompareOrdinal(kvp.Value.Criterions.ResultCoreType.FullName, fieldAggregateMapInfo.Owner.OwnerType.FullName) == 0)
            if (kvp.Value is IBusinessCollection)
            {
              foreach (IBusinessObject item in (IBusinessCollection)kvp.Value)
                if (!item.IsDeleted)
                  source.Add(item);
            }
            else
            {
              if (!kvp.Value.IsDeleted)
                source.Add(kvp.Value);
            }
        fieldAggregateMapInfo.Compute(source, this);
      }
    }
    void IRefinedlyObject.FillAggregateValues(FieldAggregateMapInfo fieldAggregateMapInfo)
    {
      FillAggregateValues(fieldAggregateMapInfo);
    }

    #endregion

    #region Data Access

    #region OldFieldValues

    /// <summary>
    /// 初始化旧值
    /// </summary>
    protected override bool InitOldFieldValues(bool must, bool reset)
    {
      bool result = base.InitOldFieldValues(must, reset);
      if (result)
        TidyAutoLinks();
      return result;
    }

    #endregion

    /// <summary>
    /// 归档
    /// </summary>
    public void Archive()
    {
      if (IsNew || IsDeleted)
        throw new InvalidOperationException("不允许归档新增或被删的业务对象!");
      MarkArchiving();
      Csla.DataPortal.Update((T)this);
    }

    /// <summary>
    /// 归档(运行在持久层的程序域里)
    /// </summary>
    /// <param name="connection">主库连接</param>
    public void Archive(DbConnection connection)
    {
      if (IsNew || IsDeleted)
        throw new InvalidOperationException("不允许归档新增或被删的业务对象!");
      ExecuteArchive(connection);
    }

    internal void ExecuteArchive(DbConnection connection)
    {
      List<string> splitKeys = new List<string>();
      foreach (FieldMapInfo item in ClassMemberHelper.GetFieldMapInfos(this.GetType()))
        if (item.TableFilterInfo != null)
        {
          string value = (string)Utilities.ChangeType(item.GetValue(this), typeof(string)) ?? String.Empty;
          if (!splitKeys.Contains(value))
            splitKeys.Add(value);
        }
      if (splitKeys.Count == 0)
        foreach (FieldMapInfo item in ClassMemberHelper.GetFieldMapInfos(this.GetType(), true, false, true))
        {
          string value = (string)Utilities.ChangeType(item.GetValue(this), typeof(string)) ?? String.Empty;
          if (!splitKeys.Contains(value))
            splitKeys.Add(value);
        }
      foreach (string s in splitKeys)
        DbConnectionHelper.ExecuteHistory(DataSourceKey, s, ExecuteArchive, connection);
    }

    internal void ExecuteArchive(DbTransaction target, DbConnection source)
    {
      if (!CascadingSave || !CascadingDelete)
        return;
      if (Owner != null && Owner.OnlySaveSelected && !Selected)
        return;

      if (DoArchiveSelf(target))
      {
        foreach (System.Reflection.PropertyInfo item in this.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic))
          if (typeof(IRefinedly).IsAssignableFrom(item.PropertyType) && item.CanRead)
          {
            IRefinedly refinedly = (IRefinedly)item.GetValue(this, BindingFlags.GetProperty | BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, null, null);
            if (refinedly != null)
              refinedly.ExecuteArchive(target, source);
          }
      }
    }
    void IRefinedly.ExecuteArchive(DbTransaction target, DbConnection source)
    {
      ExecuteArchive(target, source);
    }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
    private bool DoArchiveSelf(DbTransaction transaction)
    {
      using (DbCommand command = DbCommandHelper.CreateCommand(transaction))
      {
        Mapper.SetInsertArchiveCommand(command, this);
        try
        {
          return DbCommandHelper.ExecuteNonQuery(command) == 1;
        }
        catch
        {
          Mapper.SetUpdateArchiveCommand(command, this);
          return DbCommandHelper.ExecuteNonQuery(command) == 1;
        }
      }
    }

    private T Save(bool needCheckDirty, bool? onlySaveSelected, IBusiness[] firstTransactionData, IBusiness[] lastTransactionData)
    {
      if (onlySaveSelected.HasValue)
        throw new InvalidOperationException("参数onlySaveSelected无意义!");
      return Save(needCheckDirty, firstTransactionData, lastTransactionData);
    }
    /// <summary>
    /// 保存
    /// </summary>
    /// <param name="needCheckDirty">校验数据库数据在下载到提交期间是否被更改过, 一旦发现将报错: CheckDirtyException; 如果ClassAttribute.AllowIgnoreCheckDirty = false本功能无效, 必定报错: CheckSaveException</param>
    /// <param name="onlySaveSelected">仅提交被勾选的业务对象</param>
    /// <param name="firstTransactionData">参与事务处理前端的业务队列</param>
    /// <param name="lastTransactionData">参与事务处理末端的业务队列</param>
    /// <returns>成功提交的对象</returns>
    IBusiness IBusiness.Save(bool needCheckDirty, bool? onlySaveSelected, IBusiness[] firstTransactionData, IBusiness[] lastTransactionData)
    {
      return Save(needCheckDirty, onlySaveSelected, firstTransactionData, lastTransactionData);
    }

    /// <summary>
    /// 保存
    /// </summary>
    /// <returns>成功提交的业务对象</returns>
    public new T Save()
    {
      return Save(true, null as IBusiness[], null as IBusiness[]);
    }
    object Csla.Core.ISavable.Save()
    {
      return Save();
    }

    /// <summary>
    /// 强制按照Update方式提交保存
    /// </summary>
    /// <returns>成功提交的业务对象</returns>
    public T SaveForceUpdate()
    {
      if (IsNew)
        SetForceUpdate();
      return Save();
    }
    object Csla.Core.ISavable.Save(bool forceUpdate)
    {
      return SaveForceUpdate();
    }

    /// <summary>
    /// 保存
    /// </summary>
    /// <param name="lastTransactionData">参与事务处理末端的业务队列</param>
    /// <returns>成功提交的业务对象</returns>
    public T Save(IBusiness[] lastTransactionData)
    {
      return Save(true, null as IBusiness[], lastTransactionData);
    }

    /// <summary>
    /// 保存
    /// </summary>
    /// <param name="firstTransactionData">参与事务处理前端的业务队列</param>
    /// <param name="lastTransactionData">参与事务处理末端的业务队列</param>
    /// <returns>成功提交的业务对象</returns>
    public T Save(IBusiness[] firstTransactionData, IBusiness[] lastTransactionData)
    {
      return Save(true, firstTransactionData, lastTransactionData);
    }

    /// <summary>
    /// 保存
    /// </summary>
    /// <param name="needCheckDirty">校验数据库数据在下载到提交期间是否被更改过, 一旦发现将报错: CheckDirtyException; 如果ClassAttribute.AllowIgnoreCheckDirty = false本功能无效, 必定报错: CheckSaveException</param>
    /// <param name="lastTransactionData">参与事务处理末端的业务队列</param>
    /// <returns>成功提交的业务对象</returns>
    public T Save(bool needCheckDirty, params IBusiness[] lastTransactionData)
    {
      return Save(needCheckDirty, null as IBusiness[], lastTransactionData);
    }

    /// <summary>
    /// 保存
    /// </summary>
    /// <param name="needCheckDirty">校验数据库数据在下载到提交期间是否被更改过, 一旦发现将报错: CheckDirtyException; 如果ClassAttribute.AllowIgnoreCheckDirty = false本功能无效, 必定报错: CheckSaveException</param>
    /// <param name="firstTransactionData">参与事务处理前端的业务队列</param>
    /// <param name="lastTransactionData">参与事务处理末端的业务队列</param>
    /// <returns>成功提交的业务对象</returns>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2200:RethrowToPreserveStackDetails")]
    public virtual T Save(bool needCheckDirty, IBusiness[] firstTransactionData, IBusiness[] lastTransactionData)
    {
      ////跟踪日志
      //if (AppConfig.Debugging)
      //  EventLog.Save(String.Format("{0}.{1}({2}): EditLevel={3}", this.GetType().FullName, MethodBase.GetCurrentMethod().Name, PrimaryKey, EditLevel));
      lock (this)
      {
        if (firstTransactionData != null && firstTransactionData.Length > 0)
          _firstTransactionData = new Collection<IBusiness>(firstTransactionData);
        if (lastTransactionData != null && lastTransactionData.Length > 0)
          _lastTransactionData = new Collection<IBusiness>(lastTransactionData);
        T result = (T)this;
        try
        {
          if (!OnSavingSelf())
            return null;
          T data = result.BeforeSave(needCheckDirty);
          data = Csla.DataPortal.Update(data);
          result.AfterSave(data, firstTransactionData, lastTransactionData);
          OnSaved(data ?? result, null, null);
          OnSavedSelf(null as Exception);
        }
        catch (Exception ex)
        {
          if (ex is Csla.DataPortalException && ex.InnerException != null)
            ex = ex.InnerException;
          if (ex is Csla.Reflection.CallMethodException && ex.InnerException != null)
            ex = ex.InnerException;
          if (ex is Csla.Rules.ValidationException)
            CheckRules();
          OnSaved(result, ex, null);
          string s = OnSavedSelf(ex);
          if (!String.IsNullOrEmpty(s))
            throw new SaveException(s, ex);
          throw ex;
        }
        return result;
      }
    }

    /// <summary>
    /// 保存(运行在持久层的程序域里)
    /// </summary>
    /// <param name="transaction">数据库事务, 如果为空则将重启新事务</param>
    /// <param name="lastTransactionData">参与事务处理末端的业务队列</param>
    /// <returns>成功提交的业务对象</returns>
    public T Save(DbTransaction transaction, params IBusiness[] lastTransactionData)
    {
      return Save(transaction, true, null as IBusiness[], lastTransactionData);
    }

    /// <summary>
    /// 保存(运行在持久层的程序域里)
    /// </summary>
    /// <param name="transaction">数据库事务, 如果为空则将重启新事务</param>
    /// <param name="firstTransactionData">参与事务处理前端的业务队列</param>
    /// <param name="lastTransactionData">参与事务处理末端的业务队列</param>
    /// <returns>成功提交的业务对象</returns>
    public T Save(DbTransaction transaction, IBusiness[] firstTransactionData, IBusiness[] lastTransactionData)
    {
      return Save(transaction, true, firstTransactionData, lastTransactionData);
    }

    /// <summary>
    /// 保存(运行在持久层的程序域里)
    /// </summary>
    /// <param name="transaction">数据库事务, 如果为空则将重启新事务</param>
    /// <param name="needCheckDirty">校验数据库数据在下载到提交期间是否被更改过, 一旦发现将报错: CheckDirtyException; 如果ClassAttribute.AllowIgnoreCheckDirty = false本功能无效, 必定报错: CheckSaveException</param>
    /// <param name="lastTransactionData">参与事务处理末端的业务队列</param>
    /// <returns>成功提交的业务对象</returns>
    public T Save(DbTransaction transaction, bool needCheckDirty, params IBusiness[] lastTransactionData)
    {
      return Save(transaction, needCheckDirty, null as IBusiness[], lastTransactionData);
    }

    /// <summary>
    /// 保存(运行在持久层的程序域里)
    /// </summary>
    /// <param name="transaction">数据库事务, 如果为空则将重启新事务</param>
    /// <param name="needCheckDirty">校验数据库数据在下载到提交期间是否被更改过, 一旦发现将报错: CheckDirtyException; 如果ClassAttribute.AllowIgnoreCheckDirty = false本功能无效, 必定报错: CheckSaveException</param>
    /// <param name="firstTransactionData">参与事务处理前端的业务队列</param>
    /// <param name="lastTransactionData">参与事务处理末端的业务队列</param>
    /// <returns>成功提交的业务对象</returns>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2200:RethrowToPreserveStackDetails")]
    public virtual T Save(DbTransaction transaction, bool needCheckDirty, IBusiness[] firstTransactionData, IBusiness[] lastTransactionData)
    {
      ////跟踪日志
      //if (AppConfig.Debugging)
      //  EventLog.Save(String.Format("{0}.{1}({2}): EditLevel={3}", this.GetType().FullName, MethodBase.GetCurrentMethod().Name, PrimaryKey, EditLevel));
      lock (this)
      {
        if (firstTransactionData != null && firstTransactionData.Length > 0)
          _firstTransactionData = new Collection<IBusiness>(firstTransactionData);
        if (lastTransactionData != null && lastTransactionData.Length > 0)
          _lastTransactionData = new Collection<IBusiness>(lastTransactionData);

        T result = (T)this;
        try
        {
          DbTransaction = transaction;

          if (!OnSavingSelf())
            return null;
          T data = result.BeforeSave(needCheckDirty);
          data.ExecuteSave(transaction);
          result.AfterSave(data, firstTransactionData, lastTransactionData);
          OnSaved(data, null, null);
          OnSavedSelf(null as Exception);
        }
        catch (Exception ex)
        {
          if (ex is Csla.Rules.ValidationException)
            CheckRules();
          OnSaved(result, ex, null);
          string s = OnSavedSelf(ex);
          if (!String.IsNullOrEmpty(s))
            throw new SaveException(s, ex);
          throw;
        }
        return result;
      }
    }
    /// <summary>
    /// 保存(运行在持久层的程序域里)
    /// </summary>
    /// <param name="transaction">数据库事务, 如果为空则将重启新事务</param>
    /// <param name="needCheckDirty">校验数据库数据在下载到提交期间是否被更改过, 一旦发现将报错: CheckDirtyException; 如果ClassAttribute.AllowIgnoreCheckDirty = false本功能无效, 必定报错: CheckSaveException</param>
    /// <param name="firstTransactionData">参与事务处理前端的业务队列</param>
    /// <param name="lastTransactionData">参与事务处理末端的业务队列</param>
    /// <returns>成功提交的业务对象</returns>
    IBusiness IBusiness.Save(DbTransaction transaction, bool needCheckDirty, IBusiness[] firstTransactionData, IBusiness[] lastTransactionData)
    {
      return Save(transaction, needCheckDirty, firstTransactionData, lastTransactionData);
    }

    private T BeforeSave(bool needCheckDirty)
    {
      List<IRefinedlyObject> ignoreLinks = new List<IRefinedlyObject>();
      string error = CheckRules(false, false, ref ignoreLinks);
      if (!String.IsNullOrEmpty(error))
        throw new Csla.Rules.ValidationException(String.Format(Phenix.Core.Properties.Resources.ValidationException, error));
      if (_firstTransactionData != null)
        foreach (IRefinedly item in _firstTransactionData)
          if (item != null)
          {
            error = item.CheckRules(false, false, ref ignoreLinks);
            if (!String.IsNullOrEmpty(error))
              throw new Csla.Rules.ValidationException(String.Format(Phenix.Core.Properties.Resources.ValidationException, error));
          }
      if (_lastTransactionData != null)
        foreach (IRefinedly item in _lastTransactionData)
          if (item != null)
          {
            error = item.CheckRules(false, false, ref ignoreLinks);
            if (!String.IsNullOrEmpty(error))
              throw new Csla.Rules.ValidationException(String.Format(Phenix.Core.Properties.Resources.ValidationException, error));
          }

      T result = Clone(); //GetAllDirty((IRefinedlyObject)null) as T ?? MemberwiseClone(null)) as T;

      ignoreLinks.Clear();
      result.CompletelyApplyEdit(false, false, needCheckDirty, false, ref ignoreLinks);
      if (result._firstTransactionData != null)
        foreach (IRefinedly item in result._firstTransactionData)
          if (item != null)
            item.CompletelyApplyEdit(false, false, needCheckDirty, false, ref ignoreLinks);
      if (result._lastTransactionData != null)
        foreach (IRefinedly item in result._lastTransactionData)
          if (item != null)
            item.CompletelyApplyEdit(false, false, needCheckDirty, false, ref ignoreLinks);
      return result;
    }

    private void AfterSave(T refreshSource, IBusiness[] firstTransactionData, IBusiness[] lastTransactionData)
    {
      List<IRefinedlyObject> ignoreLinks = new List<IRefinedlyObject>();
      CompletelyApplyEdit(true, false, false, true, ref ignoreLinks);
      CascadingRefresh(refreshSource);
      RecordHasChanged();
      if (_firstTransactionData != null)
      {
        foreach (IRefinedly item in _firstTransactionData)
          if (item != null)
          {
            item.CompletelyApplyEdit(true, false, false, true, ref ignoreLinks);
            item.RecordHasChanged();
          }
        if (firstTransactionData != null && firstTransactionData.Length > 0)
        {
          _firstTransactionData.CopyTo(firstTransactionData, 0);
          _firstTransactionData = null;
        }
      }
      if (_lastTransactionData != null)
      {
        foreach (IRefinedly item in _lastTransactionData)
          if (item != null)
          {
            item.CompletelyApplyEdit(true, false, false, true, ref ignoreLinks);
            item.RecordHasChanged();
          }
        if (lastTransactionData != null && lastTransactionData.Length > 0)
        {
          _lastTransactionData.CopyTo(lastTransactionData, 0);
          _lastTransactionData = null;
        }
      }
    }

    /// <summary>
    /// 保存本业务对象之前
    /// 在执行Save()的程序域里被调用
    /// </summary>
    /// <returns>是否继续, 缺省为 true</returns>
    protected virtual bool OnSavingSelf()
    {
      return true;
    }

    /// <summary>
    /// 保存本业务对象之后
    /// 在执行Save()的程序域里被调用
    /// </summary>
    /// <param name="ex">错误信息</param>
    /// <returns>发生错误时的友好提示信息, 缺省为 null 直接抛出原异常，否则会抛出SaveException并打包本信息及原异常</returns>
    protected virtual string OnSavedSelf(Exception ex)
    {
      return null;
    }

    private void ExecuteFetchSelf(DbConnection connection, Criterions criterions)
    {
      using (DbCommand command = DbCommandHelper.CreateCommand(connection))
      {
        if (FetchTimeout.HasValue && FetchTimeout.Value >= 0)
          command.CommandTimeout = FetchTimeout.Value;
        OnFetchingSelf(connection, command, criterions);
        if (String.IsNullOrEmpty(command.CommandText))
          Mapper.SetSelectCommand(command, criterions);
        FetchSelf(command);
      }
      OnFetchedSelf(connection, criterions);
    }
    void IRefinedly.ExecuteFetchSelf(DbConnection connection, Criterions criterions)
    {
      if (HistoryFetchAll.HasValue)
      {
        DbConnectionInfo dbConnectionInfo = DbConnectionInfo.Fetch(criterions.DataSourceKey ?? DataSourceKey);
        Task[] tasks = new Task[dbConnectionInfo.HistoryCount];
        for (int i = 0; i < dbConnectionInfo.HistoryCount; i++)
        {
          int index = i;
          tasks[index] = Task.Factory.StartNew(() =>
          {
            DbConnectionHelper.ExecuteHistory(dbConnectionInfo, index.ToString(), (Action<DbConnection, Criterions>)ExecuteFetchSelf, criterions);
          });
        }
        Task.WaitAll(tasks);
      }
      if (!HistoryFetchAll.HasValue || HistoryFetchAll.Value)
        ExecuteFetchSelf(connection, criterions);
    }

    private void ExecuteFetchSelf(DbTransaction transaction, Criterions criterions)
    {
      using (DbCommand command = DbCommandHelper.CreateCommand(transaction))
      {
        if (FetchTimeout.HasValue && FetchTimeout.Value >= 0)
          command.CommandTimeout = FetchTimeout.Value;
        OnFetchingSelf(transaction, command, criterions);
        if (String.IsNullOrEmpty(command.CommandText))
          Mapper.SetSelectCommand(command, criterions);
        FetchSelf(command);
      }
      OnFetchedSelf(transaction, criterions);
    }
    void IRefinedly.ExecuteFetchSelf(DbTransaction transaction, Criterions criterions)
    {
      DbTransaction = transaction;

      if (HistoryFetchAll.HasValue)
      {
        DbConnectionInfo dbConnectionInfo = DbConnectionInfo.Fetch(criterions.DataSourceKey ?? DataSourceKey);
        Task[] tasks = new Task[dbConnectionInfo.HistoryCount];
        for (int i = 0; i < dbConnectionInfo.HistoryCount; i++)
        {
          int index = i;
          tasks[index] = Task.Factory.StartNew(() =>
          {
            DbConnectionHelper.ExecuteHistory(dbConnectionInfo, index.ToString(), (Action<DbConnection, Criterions>)ExecuteFetchSelf, criterions);
          });
        }
        Task.WaitAll(tasks);
      }
      if (!HistoryFetchAll.HasValue || HistoryFetchAll.Value)
        ExecuteFetchSelf(transaction, criterions);

    }

    /// <summary>
    /// 填充对象
    /// </summary>
    [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
    protected virtual void DataPortal_Fetch(object criteria)
    {
      Criterions criterions = criteria as Criterions;
      if (criterions != null)
        DbConnectionHelper.Execute(criterions.DataSourceKey ?? DataSourceKey, (Action<DbConnection, Criterions>)((IRefinedly)this).ExecuteFetchSelf, criterions);
      else if (criteria != null && String.CompareOrdinal(criteria.GetType().FullName, this.GetType().FullName) == 0)
        DbConnectionHelper.Execute(DataSourceKey, (Action<DbConnection, Criterions>)((IRefinedly)this).ExecuteFetchSelf, new Criterions(this.GetType(), (T)criteria));
      else
        DoFetchSelf(criteria);
    }

    /// <summary>
    /// 自定义构建业务对象
    /// </summary>
    /// <param name="criteria">自定义条件</param>
    protected virtual void DoFetchSelf(object criteria)
    {
      Exception ex = new NotSupportedException(Phenix.Core.Properties.Resources.MethodNotImplemented);
      EventLog.Save(this.GetType(), MethodBase.GetCurrentMethod(), ex);
      throw ex;
    }

    /// <summary>
    /// 提交更新
    /// </summary>
    [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
    protected override void DataPortal_Insert()
    {
      DbConnectionHelper.Execute(DataSourceKey, ExecuteSave);
    }

    /// <summary>
    /// 提交更新
    /// </summary>
    [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
    protected override void DataPortal_Update()
    {
      if (Archiving)
        DbConnectionHelper.Execute(DataSourceKey, ExecuteArchive);
      else
        DbConnectionHelper.Execute(DataSourceKey, ExecuteSave);
    }

    /// <summary>
    /// 提交更新
    /// </summary>
    [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
    protected override void DataPortal_DeleteSelf()
    {
      DbConnectionHelper.Execute(DataSourceKey, ExecuteSave);
    }

    private void ExecuteSave(DbTransaction transaction)
    {
      List<IRefinedlyObject> ignoreLinks = new List<IRefinedlyObject>();
      if (_firstTransactionData != null)
        foreach (IRefinedly item in _firstTransactionData)
          if (item != null)
            item.SaveSelf(transaction, ref ignoreLinks);
      SaveSelf(transaction, ref ignoreLinks);
      if (_lastTransactionData != null)
        foreach (IRefinedly item in _lastTransactionData)
          if (item != null)
            item.SaveSelf(transaction, ref ignoreLinks);
    }

    internal void SaveSelf(DbTransaction transaction, ref List<IRefinedlyObject> ignoreLinks)
    {
      ignoreLinks = ExecuteSaveSelf(transaction, ignoreLinks);
      CascadingAggregate(transaction);
    }
    void IRefinedly.SaveSelf(DbTransaction transaction, ref List<IRefinedlyObject> ignoreLinks)
    {
      SaveSelf(transaction, ref ignoreLinks);
    }

    private List<IRefinedlyObject> ExecuteSaveSelf(DbTransaction transaction, List<IRefinedlyObject> ignoreLinks)
    {
      if (!CascadingSave)
        return ignoreLinks;
      if (Owner != null && Owner.OnlySaveSelected && !Selected)
        return ignoreLinks;
      if (ignoreLinks.Contains(this))
        return ignoreLinks;

      if (transaction == null)
        return DbConnectionHelper.ExecuteGet(DataSourceKey, ExecuteSaveSelf, ignoreLinks);

      DbTransaction = transaction;

      OnSavingSelf(transaction);
      if (_links != null && _links.Count > 0 && AutoSaveLinks)
      {
        ignoreLinks.Add(this);
        foreach (KeyValuePair<string, IBusinessObject> kvp in _links)
          if (kvp.Value != null && kvp.Value != MasterBusiness)
            ((IRefinedly)kvp.Value).SaveSelf(transaction, ref ignoreLinks);
      }
      if (IsDeleted)
      {
        if (!IsNew)
          DeleteSelf(transaction, ref ignoreLinks);
      }
      else if (IsNew)
        InsertSelf(transaction, ref ignoreLinks);
      else
        UpdateSelf(transaction, ref ignoreLinks);
      OnSavedSelf(transaction);
      return ignoreLinks;
    }

    internal void CascadingAggregate(DbTransaction transaction)
    {
      IBusinessObject masterBusiness = MasterBusiness;
      if (masterBusiness == null || !masterBusiness.IsDeleted)
      {
        foreach (FieldAggregateMapInfo item in ClassMemberHelper.GetFieldAggregateMapInfos(this.GetType(), false))
          item.Compute(transaction, this, false);
        if (masterBusiness != null)
          ((IRefinedly)masterBusiness).CascadingAggregate(transaction);
      }
    }
    void IRefinedly.CascadingAggregate(DbTransaction transaction)
    {
      CascadingAggregate(transaction);
    }
    
    private void InsertSelf(DbTransaction transaction, ref List<IRefinedlyObject> ignoreLinks)
    {
      if (NeedFillBusinessCodeFieldValues)
        if (EntityHelper.FillBusinessCodeFieldValues(this, false))
        {
          CascadingLinkToInDetails(this, null);
          CascadingFillIdenticalValuesInDetailsAndLinks(this);
          NeedRefreshSelf = true;
        }

      if (IsSelfDirty)
      {
        //CheckSelfObjectRules(false, true);
        DoInsertSelf(transaction);
        PermanentLog(transaction, ExecuteAction.Insert);
        MarkOldClean();
      }

      if (SortedDetails != null)
      {
        LinkedListNode<IBusiness> node = SortedDetails.First;
        while (node != null)
        {
          ((IRefinedly)node.Value).SaveSelf(transaction, ref ignoreLinks);
          node = node.Next;
        }
      }
    }

    private bool UpdateSelf(DbTransaction transaction, ref List<IRefinedlyObject> ignoreLinks)
    {
      bool result = true;
      if (IsSelfDirty)
      {
        //CheckSelfObjectRules(false, true);
        result = DoUpdateSelf(transaction);
        PermanentLog(transaction, ExecuteAction.Update);
        MarkOldClean();
      }

      if (SortedDetails != null)
      {
        LinkedListNode<IBusiness> node = SortedDetails.First;
        while (node != null)
        {
          ((IRefinedly)node.Value).SaveSelf(transaction, ref ignoreLinks);
          node = node.Next;
        }
      }
      return result;
    }

    private bool DeleteSelf(DbTransaction transaction, ref List<IRefinedlyObject> ignoreLinks)
    {
      DeleteDetails(transaction, ref ignoreLinks);

      bool result = true;
      if (IsSelfDirty)
      {
        //CheckSelfObjectRules(false, true);
        PermanentLog(transaction, DeletedAsDisabled ? ExecuteAction.Update : ExecuteAction.Delete);
        result = DoDeleteSelf(transaction);
        MarkOldDelete();
      }
      return result;
    }
    bool IRefinedlyObject.DeleteSelf(DbTransaction transaction, ref List<IRefinedlyObject> ignoreLinks)
    {
      return DeleteSelf(transaction, ref ignoreLinks);
    }

    private static List<CascadingDeleteDetailInfo> AddCascadingDeleteDetailInfo(List<CascadingDeleteDetailInfo> cascadingDeleteDetailInfos, IBusiness business)
    {
      CascadingDeleteDetailInfo cascadingDeleteDetailInfo = new CascadingDeleteDetailInfo(business);
      if (cascadingDeleteDetailInfos == null)
        cascadingDeleteDetailInfos = new List<CascadingDeleteDetailInfo>();
      if (!cascadingDeleteDetailInfos.Contains(cascadingDeleteDetailInfo))
        cascadingDeleteDetailInfos.Add(cascadingDeleteDetailInfo);
      return cascadingDeleteDetailInfos;
    }

    private static List<Type> AddDeletedDetailItemType(List<Type> deletedDetailItemTypes, Type itemType)
    {
      if (deletedDetailItemTypes == null)
        deletedDetailItemTypes = new List<Type>();
      if (!deletedDetailItemTypes.Contains(itemType))
        deletedDetailItemTypes.Add(itemType);
      return deletedDetailItemTypes;
    }

    internal void DeleteDetails(DbTransaction transaction, ref List<IRefinedlyObject> ignoreLinks)
    {
      OnDeletingDetails(transaction);
      try
      {
        List<Type> deletedDetailItemTypes = null;

        if (SortedDetails != null)
        {
          LinkedListNode<IBusiness> node = SortedDetails.Last;
          while (node != null)
          {
            if (node.Value is IBusinessObject)
            {
              if (((IRefinedlyObject)node.Value).DeleteSelf(transaction, ref ignoreLinks))
              {
                _cascadingDeleteDetailInfos = AddCascadingDeleteDetailInfo(_cascadingDeleteDetailInfos, node.Value);
                deletedDetailItemTypes = AddDeletedDetailItemType(deletedDetailItemTypes, node.Value.GetType());
              }
            }
            else if (node.Value is IBusinessCollection)
            {
              if (((IRefinedlyCollection)node.Value).DeleteSelf(transaction, this, ref ignoreLinks))
              {
                _cascadingDeleteDetailInfos = AddCascadingDeleteDetailInfo(_cascadingDeleteDetailInfos, node.Value);
                deletedDetailItemTypes = AddDeletedDetailItemType(deletedDetailItemTypes, ((IBusinessCollection)node.Value).ItemValueType);
              }
            }
            node = node.Previous;
          }
        }

        if (!DeletedAsDisabled)
        {
          if (_cascadingDeleteDetailInfos != null)
            foreach (CascadingDeleteDetailInfo item in _cascadingDeleteDetailInfos)
              if (deletedDetailItemTypes == null || !deletedDetailItemTypes.Contains(item.ItemType))
                DoCascadingDeleteDetail(transaction, item);
          ClassMapInfo.CascadingDeleteOrUnlinkDetail(transaction, this);
        }
      }
      finally
      {
        OnDeletedDetails(transaction);
      }
    }

    /// <summary>
    /// 构建本业务对象之前(运行在持久层的程序域里)
    /// </summary>
    /// <param name="connection">数据库连接</param>
    /// <param name="command">DbCommand</param>
    /// <param name="criterions">条件集</param>
    protected virtual void OnFetchingSelf(DbConnection connection, DbCommand command, Criterions criterions)
    {
    }

    /// <summary>
    /// 构建本业务对象之前(运行在持久层的程序域里)
    /// </summary>
    /// <param name="transaction">数据库事务</param>
    /// <param name="command">DbCommand</param>
    /// <param name="criterions">条件集</param>
    protected virtual void OnFetchingSelf(DbTransaction transaction, DbCommand command, Criterions criterions)
    {
    }

    /// <summary>
    /// 构建本业务对象之后(运行在持久层的程序域里)
    /// </summary>
    /// <param name="connection">数据库连接</param>
    /// <param name="criterions">条件集</param>
    protected virtual void OnFetchedSelf(DbConnection connection, Criterions criterions)
    {
    }

    /// <summary>
    /// 构建本业务对象之后(运行在持久层的程序域里)
    /// </summary>
    /// <param name="transaction">数据库事务</param>
    /// <param name="criterions">条件集</param>
    protected virtual void OnFetchedSelf(DbTransaction transaction, Criterions criterions)
    {
    }

    /// <summary>
    /// 保存(增删改)本业务对象之前(运行在持久层的程序域里)
    /// </summary>
    /// <param name="transaction">数据库事务</param>
    protected virtual void OnSavingSelf(DbTransaction transaction)
    {
    }

    /// <summary>
    /// 保存(增删改)本业务对象之后(运行在持久层的程序域里)
    /// </summary>
    /// <param name="transaction">数据库事务</param>
    protected virtual void OnSavedSelf(DbTransaction transaction)
    {
    }

    /// <summary>
    /// 新增本对象数据之前(运行在持久层的程序域里)
    /// </summary>
    /// <param name="transaction">数据库事务</param>
    /// <param name="limitingCriteriaExpressions">限制保存的条件(not exists 条件语句)</param>
    protected virtual void OnInsertingSelf(DbTransaction transaction, ref List<CriteriaExpression> limitingCriteriaExpressions)
    {
    }

    /// <summary>
    /// 新增本对象数据之后(运行在持久层的程序域里)
    /// </summary>
    /// <param name="transaction">数据库事务</param>
    protected virtual void OnInsertedSelf(DbTransaction transaction)
    {
    }

    /// <summary>
    /// 更新本对象数据之前(运行在持久层的程序域里)
    /// </summary>
    /// <param name="transaction">数据库事务</param>
    /// <param name="limitingCriteriaExpressions">限制保存的条件(not exists 条件语句)</param>
    protected virtual void OnUpdatingSelf(DbTransaction transaction, ref List<CriteriaExpression> limitingCriteriaExpressions)
    {
    }

    /// <summary>
    /// 更新本对象数据之后(运行在持久层的程序域里)
    /// </summary>
    /// <param name="transaction">数据库事务</param>
    protected virtual void OnUpdatedSelf(DbTransaction transaction)
    {
    }

    /// <summary>
    /// 删除本从业务对象数据之前(运行在持久层的程序域里)
    /// </summary>
    /// <param name="transaction">数据库事务</param>
    protected virtual void OnDeletingDetails(DbTransaction transaction)
    {
    }

    /// <summary>
    /// 删除本从业务对象数据之后(运行在持久层的程序域里)
    /// </summary>
    /// <param name="transaction">数据库事务</param>
    protected virtual void OnDeletedDetails(DbTransaction transaction)
    {
    }

    /// <summary>
    /// 删除本对象数据之前(运行在持久层的程序域里)
    /// </summary>
    /// <param name="transaction">数据库事务</param>
    /// <param name="limitingCriteriaExpressions">限制保存的条件(not exists 条件语句)</param>
    protected virtual void OnDeletingSelf(DbTransaction transaction, ref List<CriteriaExpression> limitingCriteriaExpressions)
    {
    }
    /// <summary>
    /// 删除本对象数据之前(运行在持久层的程序域里)
    /// </summary>
    /// <param name="transaction">数据库事务</param>
    /// <param name="limitingCriteriaExpressions">限制保存的条件(not exists 条件语句)</param>
    void IBusiness.OnDeletingSelf(DbTransaction transaction, ref List<CriteriaExpression> limitingCriteriaExpressions)
    {
      OnDeletingSelf(transaction, ref limitingCriteriaExpressions);
    }

    /// <summary>
    /// 删除本对象数据之后(运行在持久层的程序域里)
    /// </summary>
    /// <param name="transaction">数据库事务</param>
    protected virtual void OnDeletedSelf(DbTransaction transaction)
    {
    }
    /// <summary>
    /// 删除本对象数据之后(运行在持久层的程序域里)
    /// </summary>
    /// <param name="transaction">数据库事务</param>
    void IBusiness.OnDeletedSelf(DbTransaction transaction)
    {
      OnDeletedSelf(transaction);
    }

    private void DoInsertSelf(DbTransaction transaction)
    {
      try
      {
        List<CriteriaExpression> limitingCriteriaExpressions = null;
        OnInsertingSelf(transaction, ref limitingCriteriaExpressions);
        List<ICriterions> limitingConditions = Criterions.GetConditions(limitingCriteriaExpressions);
        try
        {
          using (DbCommand command = DbCommandHelper.CreateCommand(transaction))
          {
            Mapper.SetInsertCommand(command, this, limitingConditions);
            int count = DbCommandHelper.ExecuteNonQuery(command);
            if (count == 0)
            {
              if (limitingConditions != null && limitingConditions.Count > 0)
              {
                DbCommandHelper.SaveLog(this.GetType(), MethodBase.GetCurrentMethod(), command, new CheckSaveException(this));
                throw new CheckSaveException(this);
              }
            }
          }
        }
        finally
        {
          OnInsertedSelf(transaction);
        }
      }
      catch (CheckSaveException)
      {
        throw;
      }
      catch (Exception ex)
      {
        throw new InsertException(this, EntityHelper.CheckRepeated(this, ex));
      }
    }

    private bool DoUpdateSelf(DbTransaction transaction)
    {
      try
      {
        List<CriteriaExpression> limitingCriteriaExpressions = null;
        OnUpdatingSelf(transaction, ref limitingCriteriaExpressions);
        List<ICriterions> limitingConditions = Criterions.GetConditions(limitingCriteriaExpressions);
        try
        {
          using (DbCommand command = DbCommandHelper.CreateCommand(transaction))
          {
            if (!Mapper.SetUpdateCommand(command, this, limitingConditions))
              return true;
            int count = DbCommandHelper.ExecuteNonQuery(command);
            if (count == 0)
            {
              if (limitingConditions != null && limitingConditions.Count > 0)
              {
                DbCommandHelper.SaveLog(this.GetType(), MethodBase.GetCurrentMethod(), command, new CheckSaveException(this));
                throw new CheckSaveException(this);
              }
              if (MustCheckDirty)
              {
                DbCommandHelper.SaveLog(this.GetType(), MethodBase.GetCurrentMethod(), command, new CheckSaveException(this, new CheckDirtyException(this)));
                throw new CheckSaveException(this, new CheckDirtyException(this));
              }
              if (NeedCheckDirty)
              {
                DbCommandHelper.SaveLog(this.GetType(), MethodBase.GetCurrentMethod(), command, new CheckDirtyException(this));
                throw new CheckDirtyException(this);
              }
            }
            return count == 1;
          }
        }
        finally
        {
          OnUpdatedSelf(transaction);
        }
      }
      catch (CheckSaveException)
      {
        throw;
      }
      catch (CheckDirtyException)
      {
        throw;
      }
      catch (Exception ex)
      {
        throw new UpdateException(this, EntityHelper.CheckRepeated(this, ex));
      }
    }

    private bool DoDeleteSelf(DbTransaction transaction)
    {
      try
      {
        List<CriteriaExpression> limitingCriteriaExpressions = null;
        OnDeletingSelf(transaction, ref limitingCriteriaExpressions);
        List<ICriterions> limitingConditions = Criterions.GetConditions(limitingCriteriaExpressions);
        try
        {
          using (DbCommand command = DbCommandHelper.CreateCommand(transaction))
          {
            if (DeletedAsDisabled)
            {
              if (!Mapper.SetDisableCommand(command, this, limitingConditions, true))
                return false;
            }
            else
              Mapper.SetDeleteCommand(command, this, limitingConditions);
            int count = DbCommandHelper.ExecuteNonQuery(command);
            if (count == 0)
            {
              if (limitingConditions != null && limitingConditions.Count > 0)
              {
                DbCommandHelper.SaveLog(this.GetType(), MethodBase.GetCurrentMethod(), command, new CheckSaveException(this));
                throw new CheckSaveException(this);
              }
              if (MustCheckDirty)
              {
                DbCommandHelper.SaveLog(this.GetType(), MethodBase.GetCurrentMethod(), command, new CheckSaveException(this, new CheckDirtyException(this)));
                throw new CheckSaveException(this, new CheckDirtyException(this));
              }
              if (NeedCheckDirty)
              {
                DbCommandHelper.SaveLog(this.GetType(), MethodBase.GetCurrentMethod(), command, new CheckDirtyException(this));
                throw new CheckDirtyException(this);
              }
            }
            return count == 1;
          }
        }
        finally
        {
          OnDeletedSelf(transaction);
        }
      }
      catch (CheckSaveException)
      {
        throw;
      }
      catch (CheckDirtyException)
      {
        throw;
      }
      catch (Exception ex)
      {
        if (DeletedAsDisabled)
          throw new DeleteException(this, EntityHelper.CheckRepeated(this, ex));
        else
          throw new DeleteException(this, EntityHelper.CheckOccupied(this, ex));
      }
    }

    private void DoCascadingDeleteDetail(DbTransaction transaction, CascadingDeleteDetailInfo cascadingDeleteDetailInfo)
    {
      try
      {
        List<CriteriaExpression> limitingCriteriaExpressions = null;
        cascadingDeleteDetailInfo.Owner.OnDeletingSelf(transaction, ref limitingCriteriaExpressions);
        List<ICriterions> limitingConditions = Criterions.GetConditions(limitingCriteriaExpressions);
        try
        {
          using (DbCommand command = DbCommandHelper.CreateCommand(transaction))
          {
            Mapper.SetDeleteDetailCommand(command,
              new Criterions(cascadingDeleteDetailInfo.Owner.GetType(), this, cascadingDeleteDetailInfo.GroupName), limitingConditions);
            int count = DbCommandHelper.ExecuteNonQuery(command);
            if (count == 0)
            {
              if (limitingConditions != null && limitingConditions.Count > 0)
              {
                DbCommandHelper.SaveLog(this.GetType(), MethodBase.GetCurrentMethod(), command, new CheckSaveException(this));
                throw new CheckSaveException(this);
              }
            }
          }
        }
        finally
        {
          cascadingDeleteDetailInfo.Owner.OnDeletedSelf(transaction);
        }
      }
      catch (CheckSaveException)
      {
        throw;
      }
      catch (Exception ex)
      {
        throw new DeleteException(this, ex);
      }
    }

    #endregion

    #region Validation Rules

    /// <summary>
    /// 校验是否存在重复数据
    /// </summary>
    /// <returns>重复的对象</returns>
    public IList<IEntity> CheckRepeated()
    {
      return DataHub.CheckRepeated(typeof(T), GetAllDirty(new List<IEntity>()));
    }

    #region GetAllDirty

    private List<IEntity> GetAllDirty(List<IEntity> entities)
    {
      if (entities.Contains(this))
        return entities;

      if (IsSelfDirty)
        entities.Add(this);

      if (_details != null && _details.Count > 0)
        foreach (KeyValuePair<string, IBusiness> kvp in _details)
          entities = ((IRefinedly)kvp.Value).GetAllDirty(entities);
      if (_links != null && _links.Count > 0 && NeedSaveLinks)
        foreach (KeyValuePair<string, IBusinessObject> kvp in _links)
          if (kvp.Value != null && kvp.Value != MasterBusiness)
            entities = ((IRefinedly)kvp.Value).GetAllDirty(entities);
      return entities;
    }
    List<IEntity> IRefinedly.GetAllDirty(List<IEntity> entities)
    {
      return GetAllDirty(entities);
    }

    private IRefinedly GetAllDirty(IRefinedlyObject masterBusiness)
    {
      if (EnsembleOnSaving)
        return this;
      T result = MemberwiseClone(masterBusiness as IBusinessObject);
      if (_details != null && _details.Count > 0)
      {
        result._details = new Dictionary<string, IBusiness>(_details.Count, StringComparer.Ordinal);
        foreach (KeyValuePair<string, IBusiness> kvp in _details)
        {
          IBusiness business = (IBusiness)((IRefinedly)kvp.Value).GetAllDirty(result);
          if (business != null)
            result._details[kvp.Key] = business;
        }
      }
      if (_links != null && _links.Count > 0 && NeedSaveLinks)
      {
        result._links = new Dictionary<string, IBusinessObject>(_links.Count, StringComparer.Ordinal);
        foreach (KeyValuePair<string, IBusinessObject> kvp in _links)
        {
          IBusinessObject businessObject = (IBusinessObject)((IRefinedly)kvp.Value).GetAllDirty(result);
          if (businessObject != null)
            result._links[kvp.Key] = businessObject;
        }
      }
      return result.IsSelfDirty || (result._details != null && result._details.Count > 0) || (result._links != null && result._links.Count > 0) ? result : null;
    }
    IRefinedly IRefinedly.GetAllDirty(IRefinedlyObject masterBusiness)
    {
      return GetAllDirty(masterBusiness);
    }

    #endregion

    /// <summary>
    /// 校验本业务对象是否有效
    /// onlyOldError = false
    /// onlySelfDirty = false
    /// throwIfException = false
    /// </summary>
    /// <returns>错误信息</returns>
    public IDataErrorInfo CheckSelfRules()
    {
      return CheckSelfRules(false, false, false);
    }

    /// <summary>
    /// 校验本业务对象是否有效
    /// onlySelfDirty = false
    /// throwIfException = false
    /// </summary>
    /// <param name="onlyOldError">仅检查原有错误</param>
    /// <returns>错误信息</returns>
    public IDataErrorInfo CheckSelfRules(bool onlyOldError)
    {
      return CheckSelfRules(onlyOldError, false, false);
    }

    /// <summary>
    /// 校验本业务对象是否有效
    /// throwIfException = false
    /// </summary>
    /// <param name="onlyOldError">仅检查原有错误</param>
    /// <param name="onlySelfDirty">仅检查脏数据</param>
    /// <returns>错误信息</returns>
    public IDataErrorInfo CheckSelfRules(bool onlyOldError, bool onlySelfDirty)
    {
      return CheckSelfRules(onlyOldError, onlySelfDirty, false);
    }

    /// <summary>
    /// 校验本业务对象是否有效
    /// </summary>
    /// <param name="onlyOldError">仅检查原有错误</param>
    /// <param name="onlySelfDirty">仅检查脏数据</param>
    /// <param name="throwIfException">如果为 true, 则当发现!IsSelfValid时抛出Csla.Rules.ValidationException异常</param>
    /// <returns>错误信息</returns>
    public IDataErrorInfo CheckSelfRules(bool onlyOldError, bool onlySelfDirty, bool throwIfException)
    {
      if (!onlySelfDirty || (IsSelfDirty && !IsSelfDeleted) || (IsSelfDeleted && !IsNew))
      {
        if (!onlyOldError)
          BusinessRules.CheckRules();
        else
          foreach (Csla.Core.IPropertyInfo item in FieldManager.GetRegisteredProperties())
            if (!String.IsNullOrEmpty(((IDataErrorInfo)this)[item.Name]))
              BusinessRules.CheckRules(item);
        if (!IsSelfValid)
          if (throwIfException)
            throw new Csla.Rules.ValidationException(String.Format(Phenix.Core.Properties.Resources.ValidationException, ((IDataErrorInfo)this).Error));
          else
            return this;
      }
      return null;
    }

    /// <summary>
    /// 校验本业务对象是否有效(仅ObjectRules)
    /// onlySelfDirty = false
    /// throwIfException = false
    /// </summary>
    /// <returns>错误信息</returns>
    public IDataErrorInfo CheckSelfObjectRules()
    {
      return CheckSelfObjectRules(false, false);
    }

    /// <summary>
    /// 校验本业务对象是否有效(仅ObjectRules)
    /// throwIfException = false
    /// </summary>
    /// <param name="onlySelfDirty">仅检查脏数据</param>
    /// <returns>错误信息</returns>
    public IDataErrorInfo CheckSelfObjectRules(bool onlySelfDirty)
    {
      return CheckSelfObjectRules(onlySelfDirty, false);
    }

    /// <summary>
    /// 校验本业务对象是否有效(仅ObjectRules)
    /// </summary>
    /// <param name="onlySelfDirty">仅检查脏数据</param>
    /// <param name="throwIfException">如果为 true, 则当发现!IsSelfValid时抛出Csla.Rules.ValidationException异常</param>
    /// <returns>错误信息</returns>
    public IDataErrorInfo CheckSelfObjectRules(bool onlySelfDirty, bool throwIfException)
    {
      if (!onlySelfDirty || (IsSelfDirty && !IsSelfDeleted) || (IsSelfDeleted && !IsNew))
      {
        BusinessRules.CheckObjectRules();
        if (!IsSelfValid)
          if (throwIfException)
            throw new Csla.Rules.ValidationException(
              String.Format(Phenix.Core.Properties.Resources.ValidationException, ((IDataErrorInfo)this).Error));
          else
            return this;
      }
      return null;
    }

    /// <summary>
    /// 校验本业务对象及其从业务对象集合中数据是否有效
    /// onlyOldError = false
    /// onlySelfDirty = false
    /// </summary>
    /// <returns>错误信息</returns>
    public string CheckRules()
    {
      return CheckRules(false, false);
    }

    /// <summary>
    /// 校验本业务对象及其从业务对象集合中数据是否有效
    /// onlySelfDirty = false
    /// </summary>
    /// <param name="onlyOldError">仅检查原有错误</param>
    /// <returns>错误信息</returns>
    public string CheckRules(bool onlyOldError)
    {
      return CheckRules(onlyOldError, false);
    }

    /// <summary>
    /// 校验本业务对象及其从业务对象集合中数据是否有效
    /// </summary>
    /// <param name="onlyOldError">仅检查原有错误</param>
    /// <param name="onlySelfDirty">仅检查脏数据</param>
    /// <returns>错误信息</returns>
    public string CheckRules(bool onlyOldError, bool onlySelfDirty)
    {
      List<IRefinedlyObject> ignoreLinks = new List<IRefinedlyObject>();
      return CheckRules(onlyOldError, onlySelfDirty, ref ignoreLinks);
    }

    private string CheckRules(bool onlyOldError, bool onlySelfDirty, ref List<IRefinedlyObject> ignoreLinks)
    {
      if (ignoreLinks.Contains(this))
        return null;

      StringBuilder result = null;

      IDataErrorInfo errorInfo = CheckSelfRules(onlyOldError, onlySelfDirty, false);
      if (onlyOldError)
      {
        result = new StringBuilder();
        if (errorInfo != null && !String.IsNullOrEmpty(errorInfo.Error))
          result.Append(errorInfo.Error);
      }
      else
      {
        if (errorInfo != null && !String.IsNullOrEmpty(errorInfo.Error))
          return errorInfo.Error;
      }

      if (_details != null && _details.Count > 0)
      {
        IBusiness[] array = new IBusiness[_details.Count];
        _details.Values.CopyTo(array, 0);
        foreach (IRefinedly item in array)
        {
          string error = item.CheckRules(onlyOldError, onlySelfDirty, ref ignoreLinks);
          if (!String.IsNullOrEmpty(error))
            if (onlyOldError)
              result.Append(error);
            else
              return error;
        }
      }
      if (_links != null && _links.Count > 0 && NeedSaveLinks)
      {
        ignoreLinks.Add(this);
        foreach (KeyValuePair<string, IBusinessObject> kvp in _links)
          if (kvp.Value != null && kvp.Value != MasterBusiness && !IsAutoLinkKey(kvp.Key))
          {
            string error = ((IRefinedly)kvp.Value).CheckRules(onlyOldError, onlySelfDirty, ref ignoreLinks);
            if (!String.IsNullOrEmpty(error))
              if (onlyOldError)
                result.Append(error);
              else
                return error;
          }
      }

      if (onlyOldError)
      {
        if (result.Length > 0)
          return result.ToString();
      }
      return null;
    }
    string IRefinedly.CheckRules(bool onlyOldError, bool onlySelfDirty, ref List<IRefinedlyObject> ignoreLinks)
    {
      return CheckRules(onlyOldError, onlySelfDirty, ref ignoreLinks);
    }

    #endregion

    #endregion

    #region 内嵌类

    private class CascadingDeleteDetailInfo
    {
      public CascadingDeleteDetailInfo(IBusiness owner)
      {
        _owner = owner;
        IBusinessCollection businessCollection = owner as IBusinessCollection;
        if (businessCollection != null)
        {
          _itemType = businessCollection.ItemValueType;
          if (businessCollection.Criterions != null)
            _groupName = businessCollection.Criterions.GroupName;
        }
        else
          _itemType = owner.GetType();
      }

      #region 属性

      private readonly IBusiness _owner;
      /// <summary>
      /// 所属对象
      /// </summary>
      public IBusiness Owner
      {
        get { return _owner; }
      }

      private readonly Type _itemType;
      /// <summary>
      /// 业务对象类型
      /// </summary>
      public Type ItemType
      {
        get { return _itemType; }
      }

      private readonly string _groupName;
      /// <summary>
      /// 分组名
      /// </summary>
      public string GroupName
      {
        get { return _groupName; }
      }

      private int? _hashCode;

      #endregion

      #region 方法

      /// <summary>
      /// 取哈希值(注意字符串在32位和64位系统有不同的算法得到不同的结果) 
      /// </summary>
      public override int GetHashCode()
      {
        if (!_hashCode.HasValue)
          _hashCode = _itemType.FullName.GetHashCode() ^ (!String.IsNullOrEmpty(_groupName) ? _groupName.GetHashCode() : 0);
        return _hashCode.Value;
      }

      /// <summary>
      /// 比较对象
      /// </summary>
      /// <param name="obj">对象</param>
      public override bool Equals(object obj)
      {
        if (object.ReferenceEquals(obj, this))
          return true;
        CascadingDeleteDetailInfo other = obj as CascadingDeleteDetailInfo;
        if (object.ReferenceEquals(other, null))
          return false;
        return
          String.CompareOrdinal(_itemType.FullName, other._itemType.FullName) == 0 &&
          String.CompareOrdinal(_groupName, other._groupName) == 0;
      }

      #endregion
    }

    #endregion
  }
}