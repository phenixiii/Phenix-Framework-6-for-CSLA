using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Common;
using System.Linq.Expressions;
using System.Reflection;
using Phenix.Core;
using Phenix.Core.Data;
using Phenix.Core.Mapping;
using Phenix.Core.SyncCollections;

namespace Phenix.Business
{
  /// <summary>
  /// 业务集合分页基类
  /// </summary>
  [Serializable]
  public abstract class BusinessListPageBase<T, TBusiness> : BusinessListBase<T, TBusiness>, IBusinessCollectionPage
    where T : BusinessListPageBase<T, TBusiness>
    where TBusiness : BusinessPageBase<TBusiness>
  {
    #region 工厂

    /// <summary>
    /// 构建业务对象集合
    /// </summary>
    /// <param name="orderByInfos">数据排列顺序队列</param>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1000:DoNotDeclareStaticMembersOnGenericTypes")]
    public new static T Fetch(params OrderByInfo[] orderByInfos)
    {
      return Fetch(new Criterions(typeof(T), orderByInfos), DefaultPageSize);
    }

    /// <summary>
    /// 构建业务对象集合
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters")]
    [Obsolete("请使用: Fetch(int pageSize)", true)]
    [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
    public new static T Fetch(bool cacheEnabled, params OrderByInfo[] orderByInfos)
    {
      throw new NotSupportedException(MethodBase.GetCurrentMethod().Name);
    }

    /// <summary>
    /// 构建业务对象集合
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters")]
    [Obsolete("请使用: Fetch(int pageSize)", true)]
    [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
    public new static T Fetch(bool cacheEnabled, bool lazyFetch, params OrderByInfo[] orderByInfos)
    {
      throw new NotSupportedException(MethodBase.GetCurrentMethod().Name);
    }

    /// <summary>
    /// 构建业务对象集合
    /// </summary>
    /// <param name="pageSize">分页大小</param>
    /// <param name="orderByInfos">数据排列顺序队列</param>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1000:DoNotDeclareStaticMembersOnGenericTypes")]
    public static T Fetch(int pageSize, params OrderByInfo[] orderByInfos)
    {
      return Fetch(new Criterions(typeof(T), orderByInfos), pageSize);
    }

    /// <summary>
    /// 构建业务对象集合
    /// </summary>
    /// <param name="pageSize">分页大小</param>
    /// <param name="pageNo">分页号</param>
    /// <param name="orderByInfos">数据排列顺序队列</param>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1000:DoNotDeclareStaticMembersOnGenericTypes")]
    public static T Fetch(int pageSize, int pageNo, params OrderByInfo[] orderByInfos)
    {
      return Fetch(new Criterions(typeof(T), orderByInfos), pageSize, pageNo);
    }

    /// <summary>
    /// 构建业务对象集合
    /// 条件类的字段映射关系请用Phenix.Core.Mapping.CriteriaFieldAttribute标注
    /// </summary>
    /// <param name="criteria">条件对象</param>
    /// <param name="orderByInfos">数据排列顺序队列</param>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1000:DoNotDeclareStaticMembersOnGenericTypes")]
    public new static T Fetch(ICriteria criteria, params OrderByInfo[] orderByInfos)
    {
      return Fetch(new Criterions(typeof(T), criteria, orderByInfos), DefaultPageSize);
    }

    /// <summary>
    /// 构建业务对象集合
    /// 条件类的字段映射关系请用Phenix.Core.Mapping.CriteriaFieldAttribute标注
    /// </summary>
    /// <param name="criteria">条件对象</param>
    /// <param name="cacheEnabled">是否需要缓存对象?</param>
    /// <param name="orderByInfos">数据排列顺序队列</param>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters")]
    [Obsolete("请使用: Fetch(ICriteria criteria, int pageSize)", true)]
    [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
    public new static T Fetch(ICriteria criteria, bool cacheEnabled, params OrderByInfo[] orderByInfos)
    {
      throw new NotSupportedException(MethodBase.GetCurrentMethod().Name);
    }

    /// <summary>
    /// 构建业务对象集合
    /// 条件类的字段映射关系请用Phenix.Core.Mapping.CriteriaFieldAttribute标注
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters")]
    [Obsolete("请使用: Fetch(ICriteria criteria, int pageSize)", true)]
    [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
    public new static T Fetch(ICriteria criteria, bool cacheEnabled, bool lazyFetch, params OrderByInfo[] orderByInfos)
    {
      throw new NotSupportedException(MethodBase.GetCurrentMethod().Name);
    }

    /// <summary>
    /// 构建业务对象集合
    /// 条件类的字段映射关系请用Phenix.Core.Mapping.CriteriaFieldAttribute标注
    /// </summary>
    /// <param name="criteria">条件对象</param>
    /// <param name="pageSize">分页大小</param>
    /// <param name="orderByInfos">数据排列顺序队列</param>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1000:DoNotDeclareStaticMembersOnGenericTypes")]
    public static T Fetch(ICriteria criteria, int pageSize, params OrderByInfo[] orderByInfos)
    {
      return Fetch(new Criterions(typeof(T), criteria, orderByInfos), pageSize);
    }

    /// <summary>
    /// 构建业务对象集合
    /// 条件类的字段映射关系请用Phenix.Core.Mapping.CriteriaFieldAttribute标注
    /// </summary>
    /// <param name="criteria">条件对象</param>
    /// <param name="pageSize">分页大小</param>
    /// <param name="pageNo">分页号</param>
    /// <param name="orderByInfos">数据排列顺序队列</param>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1000:DoNotDeclareStaticMembersOnGenericTypes")]
    public static T Fetch(ICriteria criteria, int pageSize, int pageNo, params OrderByInfo[] orderByInfos)
    {
      return Fetch(new Criterions(typeof(T), criteria, orderByInfos), pageSize, pageNo);
    }

    /// <summary>
    /// 构建业务对象集合
    /// </summary>
    /// <param name="criteriaExpression">条件表达式</param>
    /// <param name="orderByInfos">数据排列顺序队列</param>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1000:DoNotDeclareStaticMembersOnGenericTypes")]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures")]
    public new static T Fetch(Expression<Func<TBusiness, bool>> criteriaExpression, params OrderByInfo[] orderByInfos)
    {
      return Fetch(CriteriaHelper.ToCriteriaExpression(criteriaExpression), DefaultPageSize, orderByInfos);
    }

    /// <summary>
    /// 构建业务对象集合
    /// </summary>
    /// <param name="dataSourceKey">数据源键</param>
    /// <param name="criteriaExpression">条件表达式</param>
    /// <param name="orderByInfos">数据排列顺序队列</param>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1000:DoNotDeclareStaticMembersOnGenericTypes")]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures")]
    public new static T Fetch(string dataSourceKey, Expression<Func<TBusiness, bool>> criteriaExpression, params OrderByInfo[] orderByInfos)
    {
      return Fetch(CriteriaHelper.ToCriteriaExpression(dataSourceKey, criteriaExpression), DefaultPageSize, orderByInfos);
    }

    /// <summary>
    /// 构建业务对象集合
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters")]
    [Obsolete("请使用: Fetch(Expression<Func<TBusiness, bool>> criteriaExpression, int pageSize)", true)]
    [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
    public new static T Fetch(Expression<Func<TBusiness, bool>> criteriaExpression, bool cacheEnabled, params OrderByInfo[] orderByInfos)
    {
      throw new NotSupportedException(MethodBase.GetCurrentMethod().Name);
    }

    /// <summary>
    /// 构建业务对象集合
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters")]
    [Obsolete("请使用: Fetch(Expression<Func<TBusiness, bool>> criteriaExpression, int pageSize)", true)]
    [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
    public new static T Fetch(string dataSourceKey, Expression<Func<TBusiness, bool>> criteriaExpression, bool cacheEnabled, params OrderByInfo[] orderByInfos)
    {
      throw new NotSupportedException(MethodBase.GetCurrentMethod().Name);
    }

    /// <summary>
    /// 构建业务对象集合
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters")]
    [Obsolete("请使用: Fetch(Expression<Func<TBusiness, bool>> criteriaExpression, int pageSize)", true)]
    [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
    public new static T Fetch(Expression<Func<TBusiness, bool>> criteriaExpression, bool cacheEnabled, bool lazyFetch, params OrderByInfo[] orderByInfos)
    {
      throw new NotSupportedException(MethodBase.GetCurrentMethod().Name);
    }

    /// <summary>
    /// 构建业务对象集合
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters")]
    [Obsolete("请使用: Fetch(Expression<Func<TBusiness, bool>> criteriaExpression, int pageSize)", true)]
    [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
    public new static T Fetch(string dataSourceKey, Expression<Func<TBusiness, bool>> criteriaExpression, bool cacheEnabled, bool lazyFetch, params OrderByInfo[] orderByInfos)
    {
      throw new NotSupportedException(MethodBase.GetCurrentMethod().Name);
    }

    /// <summary>
    /// 构建业务对象集合
    /// </summary>
    /// <param name="criteriaExpression">条件表达式</param>
    /// <param name="pageSize">分页大小</param>
    /// <param name="orderByInfos">数据排列顺序队列</param>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1000:DoNotDeclareStaticMembersOnGenericTypes")]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures")]
    public static T Fetch(Expression<Func<TBusiness, bool>> criteriaExpression, int pageSize, params OrderByInfo[] orderByInfos)
    {
      return Fetch(CriteriaHelper.ToCriteriaExpression(criteriaExpression), pageSize, orderByInfos);
    }

    /// <summary>
    /// 构建业务对象集合
    /// </summary>
    /// <param name="dataSourceKey">数据源键</param>
    /// <param name="criteriaExpression">条件表达式</param>
    /// <param name="pageSize">分页大小</param>
    /// <param name="orderByInfos">数据排列顺序队列</param>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1000:DoNotDeclareStaticMembersOnGenericTypes")]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures")]
    public static T Fetch(string dataSourceKey, Expression<Func<TBusiness, bool>> criteriaExpression, int pageSize, params OrderByInfo[] orderByInfos)
    {
      return Fetch(CriteriaHelper.ToCriteriaExpression(dataSourceKey, criteriaExpression), pageSize, orderByInfos);
    }

    /// <summary>
    /// 构建业务对象集合
    /// </summary>
    /// <param name="criteriaExpression">条件表达式</param>
    /// <param name="pageSize">分页大小</param>
    /// <param name="pageNo">分页号</param>
    /// <param name="orderByInfos">数据排列顺序队列</param>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1000:DoNotDeclareStaticMembersOnGenericTypes")]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures")]
    public static T Fetch(Expression<Func<TBusiness, bool>> criteriaExpression, int pageSize, int pageNo, params OrderByInfo[] orderByInfos)
    {
      return Fetch(CriteriaHelper.ToCriteriaExpression(criteriaExpression), pageSize, pageNo, orderByInfos);
    }

    /// <summary>
    /// 构建业务对象集合
    /// </summary>
    /// <param name="dataSourceKey">数据源键</param>
    /// <param name="criteriaExpression">条件表达式</param>
    /// <param name="pageSize">分页大小</param>
    /// <param name="pageNo">分页号</param>
    /// <param name="orderByInfos">数据排列顺序队列</param>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1000:DoNotDeclareStaticMembersOnGenericTypes")]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures")]
    public static T Fetch(string dataSourceKey, Expression<Func<TBusiness, bool>> criteriaExpression, int pageSize, int pageNo, params OrderByInfo[] orderByInfos)
    {
      return Fetch(CriteriaHelper.ToCriteriaExpression(dataSourceKey, criteriaExpression), pageSize, pageNo, orderByInfos);
    }

    /// <summary>
    /// 构建业务对象集合
    /// </summary>
    /// <param name="criteriaExpression">条件表达式</param>
    /// <param name="orderByInfos">数据排列顺序队列</param>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1000:DoNotDeclareStaticMembersOnGenericTypes")]
    public new static T Fetch(CriteriaExpression criteriaExpression, params OrderByInfo[] orderByInfos)
    {
      return Fetch(new Criterions(typeof(T), criteriaExpression, orderByInfos), DefaultPageSize);
    }

    /// <summary>
    /// 构建业务对象集合
    /// </summary>
    /// <param name="criteriaExpression">条件表达式</param>
    /// <param name="cacheEnabled">可以缓存对象?</param>
    /// <param name="orderByInfos">数据排列顺序队列</param>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters")]
    [Obsolete("请使用: Fetch(CriteriaExpression criteriaExpression, int pageSize)", true)]
    [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
    public new static T Fetch(CriteriaExpression criteriaExpression, bool cacheEnabled, params OrderByInfo[] orderByInfos)
    {
      throw new NotSupportedException(MethodBase.GetCurrentMethod().Name);
    }

    /// <summary>
    /// 构建业务对象集合
    /// </summary>
    /// <param name="criteriaExpression">条件表达式</param>
    /// <param name="cacheEnabled">可以缓存对象?</param>
    /// <param name="lazyFetch">是否惰性Fetch(</param>
    /// <param name="orderByInfos">数据排列顺序队列</param>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters")]
    [Obsolete("请使用: Fetch(CriteriaExpression criteriaExpression, int pageSize)", true)]
    [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
    public new static T Fetch(CriteriaExpression criteriaExpression, bool cacheEnabled, bool lazyFetch, params OrderByInfo[] orderByInfos)
    {
      throw new NotSupportedException(MethodBase.GetCurrentMethod().Name);
    }
    
    /// <summary>
    /// 构建业务对象集合
    /// </summary>
    /// <param name="criteriaExpression">条件表达式</param>
    /// <param name="pageSize">分页大小</param>
    /// <param name="orderByInfos">数据排列顺序队列</param>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1000:DoNotDeclareStaticMembersOnGenericTypes")]
    public static T Fetch(CriteriaExpression criteriaExpression, int pageSize, params OrderByInfo[] orderByInfos)
    {
      return Fetch(new Criterions(typeof(T), criteriaExpression, orderByInfos), pageSize);
    }

    /// <summary>
    /// 构建业务对象集合
    /// </summary>
    /// <param name="criteriaExpression">条件表达式</param>
    /// <param name="pageSize">分页大小</param>
    /// <param name="pageNo">分页号</param>
    /// <param name="orderByInfos">数据排列顺序队列</param>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1000:DoNotDeclareStaticMembersOnGenericTypes")]
    public static T Fetch(CriteriaExpression criteriaExpression, int pageSize, int pageNo, params OrderByInfo[] orderByInfos)
    {
      return Fetch(new Criterions(typeof(T), criteriaExpression, orderByInfos), pageSize, pageNo);
    }

    /// <summary>
    /// 构建业务对象集合
    /// </summary>
    /// <param name="criterions">条件集</param>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1000:DoNotDeclareStaticMembersOnGenericTypes")]
    public new static T Fetch(Criterions criterions)
    {
      return DoFetch(criterions, DefaultPageSize, null, true);
    }

    /// <summary>
    /// 构建业务对象集合
    /// </summary>
    /// <param name="criterions">条件集</param>
    /// <param name="lazyFetch">是否惰性Fetch(</param>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters")]
    [Obsolete("请使用: Fetch(Criterions criterions, int pageSize)", true)]
    [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
    public new static T Fetch(Criterions criterions, bool lazyFetch)
    {
      throw new NotSupportedException(MethodBase.GetCurrentMethod().Name);
    }

    /// <summary>
    /// 构建业务对象集合
    /// </summary>
    /// <param name="criterions">条件集</param>
    /// <param name="pageSize">分页大小</param>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1000:DoNotDeclareStaticMembersOnGenericTypes")]
    public static T Fetch(Criterions criterions, int pageSize)
    {
      return DoFetch(criterions, pageSize, null, true);
    }

    /// <summary>
    /// 构建业务对象集合
    /// </summary>
    /// <param name="criterions">条件集</param>
    /// <param name="pageSize">分页大小</param>
    /// <param name="pageNo">分页号</param>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1000:DoNotDeclareStaticMembersOnGenericTypes")]
    public static T Fetch(Criterions criterions, int pageSize, int pageNo)
    {
      return DoFetch(criterions, pageSize, pageNo, true);
    }

    private static T DoFetch(Criterions criterions, int? pageSize, int? pageNo, bool setDetail)
    {
      if (criterions == null)
      {
        criterions = new Criterions(typeof(T));
        criterions.PageSize = pageSize ?? DefaultPageSize;
      }
      else
      {
        criterions = criterions.ShallowCopy();
        if (pageSize.HasValue)
          criterions.PageSize = pageSize.Value;
      }
      if (pageNo.HasValue)
        criterions.PageNo = pageNo.Value;

      return DoFetch(criterions, setDetail);
    }

    /// <summary>
    /// 构建本业务对象集合之后
    /// </summary>
    protected override void OnFetchedSelf(object criteria)
    {
      LocalPages[((ICriterions)criteria).PageNo] = new ReadOnlyCollection<TBusiness>(this);
    }

    /// <summary>
    /// 构建业务对象集合
    /// </summary>
    /// <param name="connection">数据库连接</param>
    /// <param name="orderByInfos">数据排列顺序队列</param>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1000:DoNotDeclareStaticMembersOnGenericTypes")]
    public new static T Fetch(DbConnection connection, params OrderByInfo[] orderByInfos)
    {
      return Fetch(connection, new Criterions(typeof(T), orderByInfos), DefaultPageSize);
    }
    
    /// <summary>
    /// 构建业务对象集合
    /// </summary>
    /// <param name="connection">数据库连接</param>
    /// <param name="pageSize">分页大小</param>
    /// <param name="orderByInfos">数据排列顺序队列</param>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1000:DoNotDeclareStaticMembersOnGenericTypes")]
    public static T Fetch(DbConnection connection, int pageSize, params OrderByInfo[] orderByInfos)
    {
      return Fetch(connection, new Criterions(typeof(T), orderByInfos), pageSize);
    }

    /// <summary>
    /// 构建业务对象集合
    /// </summary>
    /// <param name="connection">数据库连接</param>
    /// <param name="pageSize">分页大小</param>
    /// <param name="pageNo">分页号</param>
    /// <param name="orderByInfos">数据排列顺序队列</param>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1000:DoNotDeclareStaticMembersOnGenericTypes")]
    public static T Fetch(DbConnection connection, int pageSize, int pageNo, params OrderByInfo[] orderByInfos)
    {
      return Fetch(connection, new Criterions(typeof(T), orderByInfos), pageSize, pageNo);
    }

    /// <summary>
    /// 构建业务对象集合
    /// 条件类的字段映射关系请用Phenix.Core.Mapping.CriteriaFieldAttribute标注
    /// </summary>
    /// <param name="connection">数据库连接</param>
    /// <param name="criteria">条件对象</param>
    /// <param name="orderByInfos">数据排列顺序队列</param>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1000:DoNotDeclareStaticMembersOnGenericTypes")]
    public new static T Fetch(DbConnection connection, ICriteria criteria, params OrderByInfo[] orderByInfos)
    {
      return Fetch(connection, new Criterions(typeof(T), criteria, orderByInfos), DefaultPageSize);
    }

    /// <summary>
    /// 构建业务对象集合
    /// 条件类的字段映射关系请用Phenix.Core.Mapping.CriteriaFieldAttribute标注
    /// </summary>
    /// <param name="connection">数据库连接</param>
    /// <param name="criteria">条件对象</param>
    /// <param name="pageSize">分页大小</param>
    /// <param name="orderByInfos">数据排列顺序队列</param>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1000:DoNotDeclareStaticMembersOnGenericTypes")]
    public static T Fetch(DbConnection connection, ICriteria criteria, int pageSize, params OrderByInfo[] orderByInfos)
    {
      return Fetch(connection, new Criterions(typeof(T), criteria, orderByInfos), pageSize);
    }

    /// <summary>
    /// 构建业务对象集合
    /// 条件类的字段映射关系请用Phenix.Core.Mapping.CriteriaFieldAttribute标注
    /// </summary>
    /// <param name="connection">数据库连接</param>
    /// <param name="criteria">条件对象</param>
    /// <param name="pageSize">分页大小</param>
    /// <param name="pageNo">分页号</param>
    /// <param name="orderByInfos">数据排列顺序队列</param>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1000:DoNotDeclareStaticMembersOnGenericTypes")]
    public static T Fetch(DbConnection connection, ICriteria criteria, int pageSize, int pageNo, params OrderByInfo[] orderByInfos)
    {
      return Fetch(connection, new Criterions(typeof(T), criteria, orderByInfos), pageSize, pageNo);
    }

    /// <summary>
    /// 构建业务对象集合
    /// </summary>
    /// <param name="connection">数据库连接</param>
    /// <param name="criteriaExpression">条件表达式</param>
    /// <param name="orderByInfos">数据排列顺序队列</param>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1000:DoNotDeclareStaticMembersOnGenericTypes")]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures")]
    public new static T Fetch(DbConnection connection, Expression<Func<TBusiness, bool>> criteriaExpression, params OrderByInfo[] orderByInfos)
    {
      return Fetch(connection, CriteriaHelper.ToCriteriaExpression(criteriaExpression), DefaultPageSize, orderByInfos);
    }

    /// <summary>
    /// 构建业务对象集合
    /// </summary>
    /// <param name="connection">数据库连接</param>
    /// <param name="criteriaExpression">条件表达式</param>
    /// <param name="pageSize">分页大小</param>
    /// <param name="orderByInfos">数据排列顺序队列</param>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1000:DoNotDeclareStaticMembersOnGenericTypes")]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures")]
    public static T Fetch(DbConnection connection, Expression<Func<TBusiness, bool>> criteriaExpression, int pageSize, params OrderByInfo[] orderByInfos)
    {
      return Fetch(connection, CriteriaHelper.ToCriteriaExpression(criteriaExpression), pageSize, orderByInfos);
    }

    /// <summary>
    /// 构建业务对象集合
    /// </summary>
    /// <param name="connection">数据库连接</param>
    /// <param name="criteriaExpression">条件表达式</param>
    /// <param name="pageSize">分页大小</param>
    /// <param name="pageNo">分页号</param>
    /// <param name="orderByInfos">数据排列顺序队列</param>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1000:DoNotDeclareStaticMembersOnGenericTypes")]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures")]
    public static T Fetch(DbConnection connection, Expression<Func<TBusiness, bool>> criteriaExpression, int pageSize, int pageNo, params OrderByInfo[] orderByInfos)
    {
      return Fetch(connection, CriteriaHelper.ToCriteriaExpression(criteriaExpression), pageSize, pageNo, orderByInfos);
    }

    /// <summary>
    /// 构建业务对象集合
    /// </summary>
    /// <param name="connection">数据库连接</param>
    /// <param name="criteriaExpression">条件表达式</param>
    /// <param name="orderByInfos">数据排列顺序队列</param>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1000:DoNotDeclareStaticMembersOnGenericTypes")]
    public new static T Fetch(DbConnection connection, CriteriaExpression criteriaExpression, params OrderByInfo[] orderByInfos)
    {
      return Fetch(connection, new Criterions(typeof(T), criteriaExpression, orderByInfos), DefaultPageSize);
    }

    /// <summary>
    /// 构建业务对象集合
    /// </summary>
    /// <param name="connection">数据库连接</param>
    /// <param name="criteriaExpression">条件表达式</param>
    /// <param name="pageSize">分页大小</param>
    /// <param name="orderByInfos">数据排列顺序队列</param>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1000:DoNotDeclareStaticMembersOnGenericTypes")]
    public static T Fetch(DbConnection connection, CriteriaExpression criteriaExpression, int pageSize, params OrderByInfo[] orderByInfos)
    {
      return Fetch(connection, new Criterions(typeof(T), criteriaExpression, orderByInfos), pageSize);
    }

    /// <summary>
    /// 构建业务对象集合
    /// </summary>
    /// <param name="connection">数据库连接</param>
    /// <param name="criteriaExpression">条件表达式</param>
    /// <param name="pageSize">分页大小</param>
    /// <param name="pageNo">分页号</param>
    /// <param name="orderByInfos">数据排列顺序队列</param>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1000:DoNotDeclareStaticMembersOnGenericTypes")]
    public static T Fetch(DbConnection connection, CriteriaExpression criteriaExpression, int pageSize, int pageNo, params OrderByInfo[] orderByInfos)
    {
      return Fetch(connection, new Criterions(typeof(T), criteriaExpression, orderByInfos), pageSize, pageNo);
    }

    /// <summary>
    /// 构建业务对象集合
    /// </summary>
    /// <param name="connection">数据库连接</param>
    /// <param name="criterions">条件集</param>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1000:DoNotDeclareStaticMembersOnGenericTypes")]
    public new static T Fetch(DbConnection connection, Criterions criterions)
    {
      if (connection == null)
        throw new ArgumentNullException("connection");
      return DoFetch(connection, criterions, DefaultPageSize, null, true);
    }

    /// <summary>
    /// 构建业务对象集合
    /// </summary>
    /// <param name="connection">数据库连接</param>
    /// <param name="criterions">条件集</param>
    /// <param name="pageSize">分页大小</param>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1000:DoNotDeclareStaticMembersOnGenericTypes")]
    public static T Fetch(DbConnection connection, Criterions criterions, int pageSize)
    {
      if (connection == null)
        throw new ArgumentNullException("connection");
      return DoFetch(connection, criterions, pageSize, null, true);
    }

    /// <summary>
    /// 构建业务对象集合
    /// </summary>
    /// <param name="connection">数据库连接</param>
    /// <param name="criterions">条件集</param>
    /// <param name="pageSize">分页大小</param>
    /// <param name="pageNo">分页号</param>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1000:DoNotDeclareStaticMembersOnGenericTypes")]
    public static T Fetch(DbConnection connection, Criterions criterions, int pageSize, int pageNo)
    {
      if (connection == null)
        throw new ArgumentNullException("connection");
      return DoFetch(connection, criterions, pageSize, pageNo, true);
    }

    private static T DoFetch(DbConnection connection, Criterions criterions, int? pageSize, int? pageNo, bool setDetail)
    {
      if (criterions == null)
      {
        criterions = new Criterions(typeof(T));
        criterions.PageSize = pageSize ?? DefaultPageSize;
      }
      else
      {
        criterions = criterions.ShallowCopy();
        if (pageSize.HasValue)
          criterions.PageSize = pageSize.Value;
      }
      if (pageNo.HasValue)
        criterions.PageNo = pageNo.Value;

      return DoFetch(connection, criterions, setDetail);
    }

    /// <summary>
    /// 构建本业务对象集合之后(运行在持久层的程序域里)
    /// </summary>
    /// <param name="connection">数据库连接</param>
    /// <param name="criterions">条件集</param>
    protected override void OnFetchedSelf(DbConnection connection, Criterions criterions)
    {
      LocalPages[criterions.PageNo] = new ReadOnlyCollection<TBusiness>(this);
    }

    /// <summary>
    /// 构建业务对象集合
    /// </summary>
    /// <param name="transaction">数据库事务</param>
    /// <param name="orderByInfos">数据排列顺序队列</param>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1000:DoNotDeclareStaticMembersOnGenericTypes")]
    public new static T Fetch(DbTransaction transaction, params OrderByInfo[] orderByInfos)
    {
      return Fetch(transaction, new Criterions(typeof(T), orderByInfos), DefaultPageSize);
    }

    /// <summary>
    /// 构建业务对象集合
    /// </summary>
    /// <param name="transaction">数据库事务</param>
    /// <param name="pageSize">分页大小</param>
    /// <param name="orderByInfos">数据排列顺序队列</param>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1000:DoNotDeclareStaticMembersOnGenericTypes")]
    public static T Fetch(DbTransaction transaction, int pageSize, params OrderByInfo[] orderByInfos)
    {
      return Fetch(transaction, new Criterions(typeof(T), orderByInfos), pageSize);
    }

    /// <summary>
    /// 构建业务对象集合
    /// </summary>
    /// <param name="transaction">数据库事务</param>
    /// <param name="pageSize">分页大小</param>
    /// <param name="pageNo">分页号</param>
    /// <param name="orderByInfos">数据排列顺序队列</param>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1000:DoNotDeclareStaticMembersOnGenericTypes")]
    public static T Fetch(DbTransaction transaction, int pageSize, int pageNo, params OrderByInfo[] orderByInfos)
    {
      return Fetch(transaction, new Criterions(typeof(T), orderByInfos), pageSize, pageNo);
    }

    /// <summary>
    /// 构建业务对象集合
    /// 条件类的字段映射关系请用Phenix.Core.Mapping.CriteriaFieldAttribute标注
    /// </summary>
    /// <param name="transaction">数据库事务</param>
    /// <param name="criteria">条件对象</param>
    /// <param name="orderByInfos">数据排列顺序队列</param>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1000:DoNotDeclareStaticMembersOnGenericTypes")]
    public new static T Fetch(DbTransaction transaction, ICriteria criteria, params OrderByInfo[] orderByInfos)
    {
      return Fetch(transaction, new Criterions(typeof(T), criteria, orderByInfos), DefaultPageSize);
    }

    /// <summary>
    /// 构建业务对象集合
    /// 条件类的字段映射关系请用Phenix.Core.Mapping.CriteriaFieldAttribute标注
    /// </summary>
    /// <param name="transaction">数据库事务</param>
    /// <param name="criteria">条件对象</param>
    /// <param name="pageSize">分页大小</param>
    /// <param name="orderByInfos">数据排列顺序队列</param>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1000:DoNotDeclareStaticMembersOnGenericTypes")]
    public static T Fetch(DbTransaction transaction, ICriteria criteria, int pageSize, params OrderByInfo[] orderByInfos)
    {
      return Fetch(transaction, new Criterions(typeof(T), criteria, orderByInfos), pageSize);
    }

    /// <summary>
    /// 构建业务对象集合
    /// 条件类的字段映射关系请用Phenix.Core.Mapping.CriteriaFieldAttribute标注
    /// </summary>
    /// <param name="transaction">数据库事务</param>
    /// <param name="criteria">条件对象</param>
    /// <param name="pageSize">分页大小</param>
    /// <param name="pageNo">分页号</param>
    /// <param name="orderByInfos">数据排列顺序队列</param>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1000:DoNotDeclareStaticMembersOnGenericTypes")]
    public static T Fetch(DbTransaction transaction, ICriteria criteria, int pageSize, int pageNo, params OrderByInfo[] orderByInfos)
    {
      return Fetch(transaction, new Criterions(typeof(T), criteria, orderByInfos), pageSize, pageNo);
    }

    /// <summary>
    /// 构建业务对象集合
    /// </summary>
    /// <param name="transaction">数据库事务</param>
    /// <param name="criteriaExpression">条件表达式</param>
    /// <param name="orderByInfos">数据排列顺序队列</param>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1000:DoNotDeclareStaticMembersOnGenericTypes")]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures")]
    public new static T Fetch(DbTransaction transaction, Expression<Func<TBusiness, bool>> criteriaExpression, params OrderByInfo[] orderByInfos)
    {
      return Fetch(transaction, CriteriaHelper.ToCriteriaExpression(criteriaExpression), DefaultPageSize, orderByInfos);
    }

    /// <summary>
    /// 构建业务对象集合
    /// </summary>
    /// <param name="transaction">数据库事务</param>
    /// <param name="criteriaExpression">条件表达式</param>
    /// <param name="pageSize">分页大小</param>
    /// <param name="orderByInfos">数据排列顺序队列</param>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1000:DoNotDeclareStaticMembersOnGenericTypes")]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures")]
    public static T Fetch(DbTransaction transaction, Expression<Func<TBusiness, bool>> criteriaExpression, int pageSize, params OrderByInfo[] orderByInfos)
    {
      return Fetch(transaction, CriteriaHelper.ToCriteriaExpression(criteriaExpression), pageSize, orderByInfos);
    }

    /// <summary>
    /// 构建业务对象集合
    /// </summary>
    /// <param name="transaction">数据库事务</param>
    /// <param name="criteriaExpression">条件表达式</param>
    /// <param name="pageSize">分页大小</param>
    /// <param name="pageNo">分页号</param>
    /// <param name="orderByInfos">数据排列顺序队列</param>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1000:DoNotDeclareStaticMembersOnGenericTypes")]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures")]
    public static T Fetch(DbTransaction transaction, Expression<Func<TBusiness, bool>> criteriaExpression, int pageSize, int pageNo, params OrderByInfo[] orderByInfos)
    {
      return Fetch(transaction, CriteriaHelper.ToCriteriaExpression(criteriaExpression), pageSize, pageNo, orderByInfos);
    }

    /// <summary>
    /// 构建业务对象集合
    /// </summary>
    /// <param name="transaction">数据库事务</param>
    /// <param name="criteriaExpression">条件表达式</param>
    /// <param name="orderByInfos">数据排列顺序队列</param>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1000:DoNotDeclareStaticMembersOnGenericTypes")]
    public new static T Fetch(DbTransaction transaction, CriteriaExpression criteriaExpression, params OrderByInfo[] orderByInfos)
    {
      return Fetch(transaction, new Criterions(typeof(T), criteriaExpression, orderByInfos), DefaultPageSize);
    }

    /// <summary>
    /// 构建业务对象集合
    /// </summary>
    /// <param name="transaction">数据库事务</param>
    /// <param name="criteriaExpression">条件表达式</param>
    /// <param name="pageSize">分页大小</param>
    /// <param name="orderByInfos">数据排列顺序队列</param>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1000:DoNotDeclareStaticMembersOnGenericTypes")]
    public static T Fetch(DbTransaction transaction, CriteriaExpression criteriaExpression, int pageSize, params OrderByInfo[] orderByInfos)
    {
      return Fetch(transaction, new Criterions(typeof(T), criteriaExpression, orderByInfos), pageSize);
    }

    /// <summary>
    /// 构建业务对象集合
    /// </summary>
    /// <param name="transaction">数据库事务</param>
    /// <param name="criteriaExpression">条件表达式</param>
    /// <param name="pageSize">分页大小</param>
    /// <param name="pageNo">分页号</param>
    /// <param name="orderByInfos">数据排列顺序队列</param>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1000:DoNotDeclareStaticMembersOnGenericTypes")]
    public static T Fetch(DbTransaction transaction, CriteriaExpression criteriaExpression, int pageSize, int pageNo, params OrderByInfo[] orderByInfos)
    {
      return Fetch(transaction, new Criterions(typeof(T), criteriaExpression, orderByInfos), pageSize, pageNo);
    }

    /// <summary>
    /// 构建业务对象集合
    /// </summary>
    /// <param name="transaction">数据库事务</param>
    /// <param name="criterions">条件集</param>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1000:DoNotDeclareStaticMembersOnGenericTypes")]
    public new static T Fetch(DbTransaction transaction, Criterions criterions)
    {
      if (transaction == null)
        throw new ArgumentNullException("transaction");
      return DoFetch(transaction, criterions, DefaultPageSize, null, true);
    }

    /// <summary>
    /// 构建业务对象集合
    /// </summary>
    /// <param name="transaction">数据库事务</param>
    /// <param name="criterions">条件集</param>
    /// <param name="pageSize">分页大小</param>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1000:DoNotDeclareStaticMembersOnGenericTypes")]
    public static T Fetch(DbTransaction transaction, Criterions criterions, int pageSize)
    {
      if (transaction == null)
        throw new ArgumentNullException("transaction");
      return DoFetch(transaction, criterions, pageSize, null, true);
    }

    /// <summary>
    /// 构建业务对象集合
    /// </summary>
    /// <param name="transaction">数据库事务</param>
    /// <param name="criterions">条件集</param>
    /// <param name="pageSize">分页大小</param>
    /// <param name="pageNo">分页号</param>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1000:DoNotDeclareStaticMembersOnGenericTypes")]
    public static T Fetch(DbTransaction transaction, Criterions criterions, int pageSize, int pageNo)
    {
      if (transaction == null)
        throw new ArgumentNullException("transaction");
      return DoFetch(transaction, criterions, pageSize, pageNo, true);
    }

    private static T DoFetch(DbTransaction transaction, Criterions criterions, int? pageSize, int? pageNo, bool setDetail)
    {
      if (criterions == null)
      {
        criterions = new Criterions(typeof(T));
        criterions.PageSize = pageSize ?? DefaultPageSize;
      }
      else
      {
        criterions = criterions.ShallowCopy();
        if (pageSize.HasValue)
          criterions.PageSize = pageSize.Value;
      }
      if (pageNo.HasValue)
        criterions.PageNo = pageNo.Value;

      return DoFetch(transaction, criterions, setDetail);
    }

    /// <summary>
    /// 构建本业务对象集合之后(运行在持久层的程序域里)
    /// </summary>
    /// <param name="transaction">数据库事务</param>
    /// <param name="criterions">条件集</param>
    protected override void OnFetchedSelf(DbTransaction transaction, Criterions criterions)
    {
      LocalPages[criterions.PageNo] = new ReadOnlyCollection<TBusiness>(this);
    }

    #endregion

    #region 属性

    private static int? _defaultPageSize;
    /// <summary>
    /// 缺省分页大小
    /// 缺省为 1000
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1000:DoNotDeclareStaticMembersOnGenericTypes")]
    public static int DefaultPageSize
    {
      get { return AppSettings.GetProperty(typeof(T).FullName, ref _defaultPageSize, 1000); }
      set { AppSettings.SetProperty(typeof(T).FullName, ref _defaultPageSize, value >= 1 ? value : 1000); }
    }

    /// <summary>
    /// 分页大小
    /// 缺省为 DefaultPageSize
    /// </summary>
    public int PageSize
    {
      get { return Criterions != null ? Criterions.PageSize : DefaultPageSize; }
    }

    [NonSerialized]
    [Csla.NotUndoable]
    private long? _maxCount;
    /// <summary>
    /// 最大项数
    /// </summary>
    [Newtonsoft.Json.JsonIgnore]
    public long MaxCount
    {
      get
      {
        if (Criterions != null)
        {
          if (!_maxCount.HasValue)
            _maxCount = DataHub.GetRecordCount(Criterions);
          return _maxCount.Value;
        }
        return Count;
      }
    }

    /// <summary>
    /// 最大分页号
    /// </summary>
    [Newtonsoft.Json.JsonIgnore]
    public int MaxPageNo
    {
      get
      {
        if (PageSize > 0)
          if (MaxCount % PageSize == 0)
            return (int)(MaxCount / PageSize);
          else
            return (int)(MaxCount / PageSize + 1);
        return 0;
      }
    }

    private static readonly object _localPagesLock = new object();
    [NonSerialized]
    [Csla.NotUndoable]
    private SynchronizedSortedList<int, IList<TBusiness>> _localPages;
    private IDictionary<int, IList<TBusiness>> LocalPages
    {
      get
      {
        if (_localPages == null)
          lock (_localPagesLock)
            if (_localPages == null)
            {
              _localPages = new SynchronizedSortedList<int, IList<TBusiness>>();
            }
        return _localPages;
      }
    }

    /// <summary>
    /// 本地分页数量
    /// </summary>
    public int LocalPagesCount
    {
      get { return LocalPages.Count; }
    }

    /// <summary>
    /// 分页号
    /// </summary>
    public int PageNo
    {
      get { return Criterions != null ? Criterions.PageNo : 1; }
    }

    #endregion

    #region 方法

    /// <summary>
    /// 获取页
    /// </summary>
    /// <param name="pageNo">分页号</param>
    public IList<TBusiness> FetchPage(int pageNo)
    {
      if (Criterions == null)
      {
        Criterions = new Criterions(typeof(T));
        Criterions.PageSize = DefaultPageSize;
      }
      Criterions.PageNo = pageNo;
      return ((SynchronizedSortedList<int, IList<TBusiness>>)LocalPages).GetValue(pageNo, () =>
      {
        T value = DbTransaction != null
          ? DoFetch(DbTransaction, Criterions, null, pageNo, false)
          : DoFetch(Criterions, null, pageNo, false);
        FillRange(value);
        return new ReadOnlyCollection<TBusiness>(value);
      });
    }
    IList<IEntityPage> IEntityCollectionPage.FetchPage(int pageNo)
    {
      return new List<IEntityPage>(FetchPage(pageNo)).AsReadOnly();
    }

    /// <summary>
    /// 获取上一页
    /// </summary>
    public IList<TBusiness> FetchPrevPage()
    {
      return FetchPage(PageNo - 1);
    }
    IList<IEntityPage> IEntityCollectionPage.FetchPrevPage()
    {
      return new List<IEntityPage>(FetchPrevPage()).AsReadOnly();
    }

    /// <summary>
    /// 获取下一页
    /// </summary>
    public IList<TBusiness> FetchNextPage()
    {
      return FetchPage(PageNo + 1);
    }
    IList<IEntityPage> IEntityCollectionPage.FetchNextPage()
    {
      return new List<IEntityPage>(FetchNextPage()).AsReadOnly();
    }

    /// <summary>
    /// 获取全部页
    /// </summary>
    public T FetchAllPage()
    {
      int i = 1;
      do
      {
        if (FetchPage(i).Count < PageSize)
          break;
        i = i + 1;
      } while (true);
      _maxCount = Count;
      return (T)this;
    }
    IEntityCollectionPage IEntityCollectionPage.FetchAllPage()
    {
      return FetchAllPage();
    }

    #endregion
  }
}
