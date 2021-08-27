using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq.Expressions;
using Phenix.Business.Core;
using Phenix.Core.Data;
using Phenix.Core.Mapping;

namespace Phenix.Business.Tunnel
{
  /// <summary>
  /// 快速下载业务对象，一次下载服务请求
  /// </summary>
  [Serializable]
  public class FastFetchCommand : Csla.CommandBase<FastSaveCommand>
  {
    /// <summary>
    /// 初始化
    /// </summary>
    public FastFetchCommand()
      : this(null) { }

    /// <summary>
    /// 初始化
    /// </summary>
    /// <param name="dataSourceKey">数据源键</param>
    public FastFetchCommand(string dataSourceKey)
     : base()
    {
      _dataSourceKey = dataSourceKey;
    }

    #region 属性

    [Csla.NotUndoable]
    private readonly string _dataSourceKey;
    /// <summary>
    /// 数据源键
    /// </summary>
    public string DataSourceKey
    {
      get { return _dataSourceKey; }
    }

    [Csla.NotUndoable]
    private readonly Dictionary<Criterions, Type> _criteriaInfos = new Dictionary<Criterions, Type>();

    [Csla.NotUndoable]
    private Dictionary<Criterions, IBusiness> _resultInfos;

    #endregion

    #region 方法

    /// <summary>
    /// 构建业务对象
    /// 表中仅一条记录
    /// 否则仅取表的第一条记录
    /// </summary>
    /// <param name="orderByInfos">数据排列顺序队列</param>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
    public void AddFetch<T>(params OrderByInfo[] orderByInfos)
      where T : BusinessBase<T>
    {
      AddFetch<T>(new Criterions(typeof(T), orderByInfos));
    }

    /// <summary>
    /// 构建业务对象
    /// 条件类的字段映射关系请用Phenix.Core.Mapping.CriteriaFieldAttribute标注
    /// </summary>
    /// <param name="criteria">条件对象</param>
    /// <param name="orderByInfos">数据排列顺序队列</param>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
    public void AddFetch<T>(ICriteria criteria, params OrderByInfo[] orderByInfos)
      where T : BusinessBase<T>
    {
      AddFetch<T>(new Criterions(typeof(T), criteria, orderByInfos));
    }

    /// <summary>
    /// 构建业务对象
    /// </summary>
    /// <param name="criteriaExpression">条件表达式</param>
    /// <param name="orderByInfos">数据排列顺序队列</param>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures")]
    public void AddFetch<T>(Expression<Func<T, bool>> criteriaExpression, params OrderByInfo[] orderByInfos)
      where T : BusinessBase<T>
    {
      AddFetch<T>(CriteriaHelper.ToCriteriaExpression(criteriaExpression), orderByInfos);
    }

    /// <summary>
    /// 构建业务对象
    /// </summary>
    /// <param name="criteriaExpression">条件表达式</param>
    /// <param name="orderByInfos">数据排列顺序队列</param>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
    public void AddFetch<T>(CriteriaExpression criteriaExpression, params OrderByInfo[] orderByInfos)
      where T : BusinessBase<T>
    {
      AddFetch<T>(new Criterions(typeof(T), criteriaExpression, orderByInfos));
    }

    /// <summary>
    /// 构建业务对象
    /// </summary>
    /// <param name="criterions">条件集</param>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
    public void AddFetch<T>(Criterions criterions)
      where T : BusinessBase<T>
    {
      if (criterions == null)
        criterions = new Criterions(typeof(T));
      _criteriaInfos[criterions] = typeof(T);
    }

    /// <summary>
    /// 构建业务对象集合
    /// </summary>
    /// <param name="orderByInfos">数据排列顺序队列</param>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
    public void AddFetch<T, TBusiness>(params OrderByInfo[] orderByInfos)
      where T : BusinessListBase<T, TBusiness>
      where TBusiness : BusinessBase<TBusiness>
    {
      AddFetch<T, TBusiness>(new Criterions(typeof(T), orderByInfos));
    }

    /// <summary>
    /// 构建业务对象集合
    /// 条件类的字段映射关系请用Phenix.Core.Mapping.CriteriaFieldAttribute标注
    /// </summary>
    /// <param name="criteria">条件对象</param>
    /// <param name="orderByInfos">数据排列顺序队列</param>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
    public void AddFetch<T, TBusiness>(ICriteria criteria, params OrderByInfo[] orderByInfos)
      where T : BusinessListBase<T, TBusiness>
      where TBusiness : BusinessBase<TBusiness>
    {
      AddFetch<T, TBusiness>(new Criterions(typeof(T), criteria, orderByInfos));
    }

    /// <summary>
    /// 构建业务对象集合
    /// </summary>
    /// <param name="criteriaExpression">条件表达式</param>
    /// <param name="orderByInfos">数据排列顺序队列</param>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures")]
    public void AddFetch<T, TBusiness>(Expression<Func<TBusiness, bool>> criteriaExpression, params OrderByInfo[] orderByInfos)
      where T : BusinessListBase<T, TBusiness>
      where TBusiness : BusinessBase<TBusiness>
    {
      AddFetch<T, TBusiness>(CriteriaHelper.ToCriteriaExpression(criteriaExpression), orderByInfos);
    }

    /// <summary>
    /// 构建业务对象集合
    /// </summary>
    /// <param name="criteriaExpression">条件表达式</param>
    /// <param name="orderByInfos">数据排列顺序队列</param>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
    public void AddFetch<T, TBusiness>(CriteriaExpression criteriaExpression, params OrderByInfo[] orderByInfos)
      where T : BusinessListBase<T, TBusiness>
      where TBusiness : BusinessBase<TBusiness>
    {
      AddFetch<T, TBusiness>(new Criterions(typeof(T), criteriaExpression, orderByInfos));
    }

    /// <summary>
    /// 构建业务对象集合
    /// </summary>
    /// <param name="criterions">条件集</param>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
    public void AddFetch<T, TBusiness>(Criterions criterions)
      where T : BusinessListBase<T, TBusiness>
      where TBusiness : BusinessBase<TBusiness>
    {
      if (criterions == null)
        criterions = new Criterions(typeof(T));
      _criteriaInfos[criterions] = typeof(T);
    }

    /// <summary>
    /// 执行指令
    /// </summary>
    public IDictionary<Criterions, IBusiness> Execute()
    {
      return Csla.DataPortal.Update(this)._resultInfos;
    }

    #region Data Access

    /// <summary>
    /// 处理执行指令(运行在持久层的程序域里)
    /// </summary>
    [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
    protected override void DataPortal_Execute()
    {
      DbConnectionHelper.Execute(DataSourceKey, DoExecute);
    }

    private void DoExecute(DbConnection connection)
    {
      _resultInfos = new Dictionary<Criterions, IBusiness>(_criteriaInfos.Count);
      foreach (KeyValuePair<Criterions, Type> item in _criteriaInfos)
      {
        IRefinedly result = (IRefinedly)Csla.Reflection.MethodCaller.CreateInstance(item.Value);
        result.ExecuteFetchSelf(connection, item.Key);
        _resultInfos[item.Key] = (IBusiness)result;
      }
    }

    #endregion

    #endregion
  }
}
