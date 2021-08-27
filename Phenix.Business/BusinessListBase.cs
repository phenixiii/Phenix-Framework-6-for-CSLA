using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Data.Common;
using System.Linq;
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
using Phenix.Core.Renovate;
using Phenix.Core.Rule;

namespace Phenix.Business
{
    /// <summary>
    /// ҵ�񼯺ϻ���
    /// </summary>
    [Serializable]
  [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Maintainability", "CA1506:AvoidExcessiveClassCoupling")]
  public abstract class BusinessListBase<T, TBusiness> : Phenix.Business.Core.BusinessListBase<T, TBusiness>, IRefinedlyCollection, IBusinessCollection, IAnalyser
    where T : BusinessListBase<T, TBusiness>
    where TBusiness : BusinessBase<TBusiness>
  {
    #region ����

    #region CreateInstance

    /// <summary>
    /// ����ʵ��
    /// </summary>
    protected static T DynamicCreateInstance(Criterions criterions)
    {
      T result = DynamicCreateInstance();
      if (criterions == null)
        criterions = new Criterions(typeof(T));
      result.Criterions = criterions;
      if (criterions.MasterBusiness != null)
        criterions.MasterBusiness.SetDetail<T, TBusiness>(result);
      return result;
    }

    #endregion

    #region New

    /// <summary>
    /// ����ҵ����󼯺�
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1000:DoNotDeclareStaticMembersOnGenericTypes")]
    public static T New()
    {
      return DynamicCreateInstance(null);
    }

    internal static T New(Criterions criterions)
    {
      return DynamicCreateInstance(criterions);
    }

    /// <summary>
    /// ����ҵ����󼯺�
    /// </summary>
    /// <param name="source">����Դ</param>
    /// <param name="propertyInfos">����������Ϣ����, ˳��������ԴcolumnIndexһ��, ��Ϊnull���ն���ʱ��source������ҵ����������ƥ�����������������</param>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1000:DoNotDeclareStaticMembersOnGenericTypes")]
    public static T New(DataTable source, params Phenix.Core.Mapping.IPropertyInfo[] propertyInfos)
    {
      if (source == null)
        return null;
      List<FieldMapInfo> fieldMapInfos = new List<FieldMapInfo>();
      if (propertyInfos == null || propertyInfos.Length == 0)
      {
        IDictionary<string, Phenix.Core.Mapping.IPropertyInfo> infos = ClassMemberHelper.GetPropertyInfos(typeof(TBusiness));
        Phenix.Core.Mapping.IPropertyInfo propertyInfo;
        foreach (DataColumn dataColumn in source.Columns)
          if (infos.TryGetValue(dataColumn.ColumnName, out propertyInfo))
            fieldMapInfos.Add(propertyInfo.FieldMapInfo);
      }
      else
      {
        foreach (Phenix.Core.Mapping.IPropertyInfo item in propertyInfos)
        {
          CodingStandards.CheckFieldMapInfo(item);
          fieldMapInfos.Add(item.FieldMapInfo);
        }
      }
      if (fieldMapInfos.Count == 0)
        throw new InvalidOperationException("���ṩ���ٰ���һ�����ݵ�importPropertyInfos����");
      T result = DynamicCreateInstance(null);
      bool oldRaiseListChangedEvents = result.RaiseListChangedEvents;
      try
      {
        result.RaiseListChangedEvents = false;
        foreach (DataRow item in source.Rows)
          result.Add(BusinessBase<TBusiness>.New(item, fieldMapInfos));
        return result;
      }
      finally
      {
        result.RaiseListChangedEvents = oldRaiseListChangedEvents;
      }
    }

    #endregion

    #region Fetch

    /// <summary>
    /// ����ҵ����󼯺�
    /// </summary>
    /// <param name="source">����Դ</param>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1000:DoNotDeclareStaticMembersOnGenericTypes")]
    public static T Fetch(IEnumerable<TBusiness> source)
    {
      T result = DynamicCreateInstance(source is T ? ((T)source).Criterions : null);
      result.FillRange(source);
      return result;
    }

    /// <summary>
    /// ����ҵ����󼯺�
    /// </summary>
    /// <param name="criteria">�Զ�������</param>
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
        if (result != null)
        {
          result.InLazyFetch = false;
          result.OnFetchedSelf(criteria);
        }
        return result;
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

    #region �¼�

    /// <summary>
    /// ������ҵ����󼯺�֮��
    /// </summary>
    protected virtual void OnFetchedSelf(object criteria)
    {
    }

    #endregion

    /// <summary>
    /// ����ָ��Ψһ��ֵ����ȡ��Ӧ�����ݿ��¼����ҵ�����
    /// </summary>
    /// <param name="itself">��Ψһ��ֵ��ҵ�����</param>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1000:DoNotDeclareStaticMembersOnGenericTypes")]
    public static T Fetch(TBusiness itself)
    {
      if (itself == null)
        throw new ArgumentNullException("itself");
      return Fetch((object)itself.PureClone());
    }

    /// <summary>
    /// ����ָ��Ψһ��ֵ����ȡ��Ӧ�����ݿ��¼����ҵ�����
    /// </summary>
    /// <param name="connection">���ݿ�����</param>
    /// <param name="itself">��Ψһ��ֵ��ҵ�����</param>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1000:DoNotDeclareStaticMembersOnGenericTypes")]
    public static T Fetch(DbConnection connection, TBusiness itself)
    {
      if (connection == null)
        throw new ArgumentNullException("connection");
      if (itself == null)
        throw new ArgumentNullException("itself");
      T result = DynamicCreateInstance();
      ((IRefinedly)result).ExecuteFetchSelf(connection, new Criterions(typeof(T), itself));
      return result;
    }

    /// <summary>
    /// ����ָ��Ψһ��ֵ����ȡ��Ӧ�����ݿ��¼����ҵ�����
    /// </summary>
    /// <param name="transaction">���ݿ�����</param>
    /// <param name="itself">��Ψһ��ֵ��ҵ�����</param>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1000:DoNotDeclareStaticMembersOnGenericTypes")]
    public static T Fetch(DbTransaction transaction, TBusiness itself)
    {
      if (transaction == null)
        throw new ArgumentNullException("transaction");
      if (itself == null)
        throw new ArgumentNullException("itself");
      T result = DynamicCreateInstance();
      ((IRefinedly)result).ExecuteFetchSelf(transaction, new Criterions(typeof(T), itself));
      return result;
    }

    /// <summary>
    /// ����ҵ����󼯺�
    /// </summary>
    /// <param name="orderByInfos">��������˳�����</param>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1000:DoNotDeclareStaticMembersOnGenericTypes")]
    public static T Fetch(params OrderByInfo[] orderByInfos)
    {
      return Fetch(new Criterions(typeof(T), orderByInfos));
    }

    /// <summary>
    /// ����ҵ����󼯺�
    /// </summary>
    /// <param name="cacheEnabled">�Ƿ���Ҫ�������?</param>
    /// <param name="orderByInfos">��������˳�����</param>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1000:DoNotDeclareStaticMembersOnGenericTypes")]
    public static T Fetch(bool cacheEnabled, params OrderByInfo[] orderByInfos)
    {
      return Fetch(new Criterions(typeof(T), cacheEnabled, false, orderByInfos));
    }

    /// <summary>
    /// ����ҵ����󼯺�
    /// </summary>
    /// <param name="cacheEnabled">�Ƿ���Ҫ�������?</param>
    /// <param name="lazyFetch">�Ƿ����Fetch(</param>
    /// <param name="orderByInfos">��������˳�����</param>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1000:DoNotDeclareStaticMembersOnGenericTypes")]
    public static T Fetch(bool cacheEnabled, bool lazyFetch, params OrderByInfo[] orderByInfos)
    {
      return Fetch(new Criterions(typeof(T), cacheEnabled, false, orderByInfos), lazyFetch);
    }

    /// <summary>
    /// ����ҵ����󼯺�
    /// ��������ֶ�ӳ���ϵ����Phenix.Core.Mapping.CriteriaFieldAttribute��ע
    /// </summary>
    /// <param name="criteria">��������</param>
    /// <param name="orderByInfos">��������˳�����</param>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1000:DoNotDeclareStaticMembersOnGenericTypes")]
    public static T Fetch(ICriteria criteria, params OrderByInfo[] orderByInfos)
    {
      return Fetch(new Criterions(typeof(T), criteria, orderByInfos));
    }
    
    /// <summary>
    /// ����ҵ����󼯺�
    /// ��������ֶ�ӳ���ϵ����Phenix.Core.Mapping.CriteriaFieldAttribute��ע
    /// </summary>
    /// <param name="criteria">��������</param>
    /// <param name="cacheEnabled">�Ƿ���Ҫ�������?</param>
    /// <param name="orderByInfos">��������˳�����</param>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1000:DoNotDeclareStaticMembersOnGenericTypes")]
    public static T Fetch(ICriteria criteria, bool cacheEnabled, params OrderByInfo[] orderByInfos)
    {
      return Fetch(new Criterions(typeof(T), criteria, cacheEnabled, orderByInfos));
    }

    /// <summary>
    /// ����ҵ����󼯺�
    /// ��������ֶ�ӳ���ϵ����Phenix.Core.Mapping.CriteriaFieldAttribute��ע
    /// </summary>
    /// <param name="criteria">��������</param>
    /// <param name="cacheEnabled">�Ƿ���Ҫ�������?</param>
    /// <param name="lazyFetch">�Ƿ����Fetch(</param>
    /// <param name="orderByInfos">��������˳�����</param>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1000:DoNotDeclareStaticMembersOnGenericTypes")]
    public static T Fetch(ICriteria criteria, bool cacheEnabled, bool lazyFetch, params OrderByInfo[] orderByInfos)
    {
      return Fetch(new Criterions(typeof(T), criteria, cacheEnabled, orderByInfos), lazyFetch);
    }

    /// <summary>
    /// ����ҵ����󼯺�
    /// </summary>
    /// <param name="criteriaExpression">�������ʽ</param>
    /// <param name="orderByInfos">��������˳�����</param>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1000:DoNotDeclareStaticMembersOnGenericTypes")]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures")]
    public static T Fetch(Expression<Func<TBusiness, bool>> criteriaExpression, params OrderByInfo[] orderByInfos)
    {
      return Fetch(CriteriaHelper.ToCriteriaExpression(criteriaExpression), orderByInfos);
    }

    /// <summary>
    /// ����ҵ����󼯺�
    /// </summary>
    /// <param name="dataSourceKey">����Դ��</param>
    /// <param name="criteriaExpression">�������ʽ</param>
    /// <param name="orderByInfos">��������˳�����</param>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1000:DoNotDeclareStaticMembersOnGenericTypes")]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures")]
    public static T Fetch(string dataSourceKey, Expression<Func<TBusiness, bool>> criteriaExpression, params OrderByInfo[] orderByInfos)
    {
      return Fetch(CriteriaHelper.ToCriteriaExpression(dataSourceKey, criteriaExpression), orderByInfos);
    }

    /// <summary>
    /// ����ҵ����󼯺�
    /// </summary>
    /// <param name="criteriaExpression">�������ʽ</param>
    /// <param name="cacheEnabled">���Ի������?</param>
    /// <param name="orderByInfos">��������˳�����</param>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1000:DoNotDeclareStaticMembersOnGenericTypes")]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures")]
    public static T Fetch(Expression<Func<TBusiness, bool>> criteriaExpression, bool cacheEnabled, params OrderByInfo[] orderByInfos)
    {
      return Fetch(CriteriaHelper.ToCriteriaExpression(criteriaExpression), cacheEnabled, orderByInfos);
    }

    /// <summary>
    /// ����ҵ����󼯺�
    /// </summary>
    /// <param name="dataSourceKey">����Դ��</param>
    /// <param name="criteriaExpression">�������ʽ</param>
    /// <param name="cacheEnabled">���Ի������?</param>
    /// <param name="orderByInfos">��������˳�����</param>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1000:DoNotDeclareStaticMembersOnGenericTypes")]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures")]
    public static T Fetch(string dataSourceKey, Expression<Func<TBusiness, bool>> criteriaExpression, bool cacheEnabled, params OrderByInfo[] orderByInfos)
    {
      return Fetch(CriteriaHelper.ToCriteriaExpression(dataSourceKey, criteriaExpression), cacheEnabled, orderByInfos);
    }

    /// <summary>
    /// ����ҵ����󼯺�
    /// </summary>
    /// <param name="criteriaExpression">�������ʽ</param>
    /// <param name="cacheEnabled">���Ի������?</param>
    /// <param name="lazyFetch">�Ƿ����Fetch(</param>
    /// <param name="orderByInfos">��������˳�����</param>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1000:DoNotDeclareStaticMembersOnGenericTypes")]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures")]
    public static T Fetch(Expression<Func<TBusiness, bool>> criteriaExpression, bool cacheEnabled, bool lazyFetch, params OrderByInfo[] orderByInfos)
    {
      return Fetch(CriteriaHelper.ToCriteriaExpression(criteriaExpression), cacheEnabled, lazyFetch, orderByInfos);
    }

    /// <summary>
    /// ����ҵ����󼯺�
    /// </summary>
    /// <param name="dataSourceKey">����Դ��</param>
    /// <param name="criteriaExpression">�������ʽ</param>
    /// <param name="cacheEnabled">���Ի������?</param>
    /// <param name="lazyFetch">�Ƿ����Fetch(</param>
    /// <param name="orderByInfos">��������˳�����</param>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1000:DoNotDeclareStaticMembersOnGenericTypes")]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures")]
    public static T Fetch(string dataSourceKey, Expression<Func<TBusiness, bool>> criteriaExpression, bool cacheEnabled, bool lazyFetch, params OrderByInfo[] orderByInfos)
    {
      return Fetch(CriteriaHelper.ToCriteriaExpression(dataSourceKey, criteriaExpression), cacheEnabled, lazyFetch, orderByInfos);
    }

    /// <summary>
    /// ����ҵ����󼯺�
    /// </summary>
    /// <param name="criteriaExpression">�������ʽ</param>
    /// <param name="orderByInfos">��������˳�����</param>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1000:DoNotDeclareStaticMembersOnGenericTypes")]
    public static T Fetch(CriteriaExpression criteriaExpression, params OrderByInfo[] orderByInfos)
    {
      return Fetch(new Criterions(typeof(T), criteriaExpression, orderByInfos));
    }

    /// <summary>
    /// ����ҵ����󼯺�
    /// </summary>
    /// <param name="criteriaExpression">�������ʽ</param>
    /// <param name="cacheEnabled">���Ի������?</param>
    /// <param name="orderByInfos">��������˳�����</param>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1000:DoNotDeclareStaticMembersOnGenericTypes")]
    public static T Fetch(CriteriaExpression criteriaExpression, bool cacheEnabled, params OrderByInfo[] orderByInfos)
    {
      return Fetch(new Criterions(typeof(T), criteriaExpression, cacheEnabled, orderByInfos));
    }

    /// <summary>
    /// ����ҵ����󼯺�
    /// </summary>
    /// <param name="criteriaExpression">�������ʽ</param>
    /// <param name="cacheEnabled">���Ի������?</param>
    /// <param name="lazyFetch">�Ƿ����Fetch(</param>
    /// <param name="orderByInfos">��������˳�����</param>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1000:DoNotDeclareStaticMembersOnGenericTypes")]
    public static T Fetch(CriteriaExpression criteriaExpression, bool cacheEnabled, bool lazyFetch, params OrderByInfo[] orderByInfos)
    {
      return Fetch(new Criterions(typeof(T), criteriaExpression, cacheEnabled, orderByInfos), lazyFetch);
    }
    
    /// <summary>
    /// ����ҵ����󼯺�
    /// </summary>
    /// <param name="criterions">������</param>
    /// <param name="lazyFetch">�Ƿ����Fetch(</param>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1000:DoNotDeclareStaticMembersOnGenericTypes")]
    public static T Fetch(Criterions criterions, bool lazyFetch)
    {
      if (lazyFetch)
      {
        T result = DynamicCreateInstance(criterions);
        result.InLazyFetch = true;
        return result;
      }
      return Fetch(criterions);
    }
    
    /// <summary>
    /// ����ҵ����󼯺�
    /// </summary>
    /// <param name="criterions">������</param>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1000:DoNotDeclareStaticMembersOnGenericTypes")]
    public static T Fetch(Criterions criterions)
    {
      return DoFetch(criterions, true);
    }

    internal static T DoFetch(Criterions criterions, bool setDetail)
    {
      if (criterions == null)
        criterions = new Criterions(typeof(T));
      T result = Fetch((object)criterions.PureClone());
      if (result != null)
      {
        result.Criterions = criterions;
        if (setDetail && result.MasterBusiness != null)
          result.MasterBusiness.SetDetail<T, TBusiness>(result);
      }
      return result;
    }

    /// <summary>
    /// ����ҵ����󼯺�
    /// </summary>
    /// <param name="connection">���ݿ�����</param>
    /// <param name="orderByInfos">��������˳�����</param>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1000:DoNotDeclareStaticMembersOnGenericTypes")]
    public static T Fetch(DbConnection connection, params OrderByInfo[] orderByInfos)
    {
      return Fetch(connection, new Criterions(typeof(T), orderByInfos));
    }

    /// <summary>
    /// ����ҵ����󼯺�
    /// ��������ֶ�ӳ���ϵ����Phenix.Core.Mapping.CriteriaFieldAttribute��ע
    /// </summary>
    /// <param name="connection">���ݿ�����</param>
    /// <param name="criteria">��������</param>
    /// <param name="orderByInfos">��������˳�����</param>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1000:DoNotDeclareStaticMembersOnGenericTypes")]
    public static T Fetch(DbConnection connection, ICriteria criteria, params OrderByInfo[] orderByInfos)
    {
      return Fetch(connection, new Criterions(typeof(T), criteria, orderByInfos));
    }

    /// <summary>
    /// ����ҵ����󼯺�
    /// </summary>
    /// <param name="connection">���ݿ�����</param>
    /// <param name="criteriaExpression">�������ʽ</param>
    /// <param name="orderByInfos">��������˳�����</param>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1000:DoNotDeclareStaticMembersOnGenericTypes")]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures")]
    public static T Fetch(DbConnection connection, Expression<Func<TBusiness, bool>> criteriaExpression, params OrderByInfo[] orderByInfos)
    {
      return Fetch(connection, CriteriaHelper.ToCriteriaExpression(criteriaExpression), orderByInfos);
    }

    /// <summary>
    /// ����ҵ����󼯺�
    /// </summary>
    /// <param name="connection">���ݿ�����</param>
    /// <param name="criteriaExpression">�������ʽ</param>
    /// <param name="orderByInfos">��������˳�����</param>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1000:DoNotDeclareStaticMembersOnGenericTypes")]
    public static T Fetch(DbConnection connection, CriteriaExpression criteriaExpression, params OrderByInfo[] orderByInfos)
    {
      return Fetch(connection, new Criterions(typeof(T), criteriaExpression, orderByInfos));
    }

    /// <summary>
    /// ����ҵ����󼯺�
    /// </summary>
    /// <param name="connection">���ݿ�����</param>
    /// <param name="criterions">������</param>
    /// <param name="lazyFetch">�Ƿ����Fetch(</param>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1000:DoNotDeclareStaticMembersOnGenericTypes")]
    public static T Fetch(DbConnection connection, Criterions criterions, bool lazyFetch)
    {
      if (lazyFetch)
      {
        T result = DynamicCreateInstance(criterions);
        result.InLazyFetch = true;
        return result;
      }
      return Fetch(connection, criterions);
    }

    /// <summary>
    /// ����ҵ����󼯺�
    /// </summary>
    /// <param name="connection">���ݿ�����</param>
    /// <param name="criterions">������</param>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1000:DoNotDeclareStaticMembersOnGenericTypes")]
    public static T Fetch(DbConnection connection, Criterions criterions)
    {
      if (connection == null)
        throw new ArgumentNullException("connection");
      return DoFetch(connection, criterions, true);
    }

    internal static T DoFetch(DbConnection connection, Criterions criterions, bool setDetail)
    {
      if (criterions == null)
        criterions = new Criterions(typeof(T));
      T result = DynamicCreateInstance();
      ((IRefinedly)result).ExecuteFetchSelf(connection, criterions);
      result.Criterions = criterions;
      if (setDetail && result.MasterBusiness != null)
        result.MasterBusiness.SetDetail<T, TBusiness>(result);
      return result;
    }

    /// <summary>
    /// ����ҵ����󼯺�
    /// </summary>
    /// <param name="transaction">���ݿ�����</param>
    /// <param name="orderByInfos">��������˳�����</param>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1000:DoNotDeclareStaticMembersOnGenericTypes")]
    public static T Fetch(DbTransaction transaction, params OrderByInfo[] orderByInfos)
    {
      return Fetch(transaction, new Criterions(typeof(T), orderByInfos));
    }

    /// <summary>
    /// ����ҵ����󼯺�
    /// ��������ֶ�ӳ���ϵ����Phenix.Core.Mapping.CriteriaFieldAttribute��ע
    /// </summary>
    /// <param name="transaction">���ݿ�����</param>
    /// <param name="criteria">��������</param>
    /// <param name="orderByInfos">��������˳�����</param>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1000:DoNotDeclareStaticMembersOnGenericTypes")]
    public static T Fetch(DbTransaction transaction, ICriteria criteria, params OrderByInfo[] orderByInfos)
    {
      return Fetch(transaction, new Criterions(typeof(T), criteria, orderByInfos));
    }

    /// <summary>
    /// ����ҵ����󼯺�
    /// </summary>
    /// <param name="transaction">���ݿ�����</param>
    /// <param name="criteriaExpression">�������ʽ</param>
    /// <param name="orderByInfos">��������˳�����</param>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1000:DoNotDeclareStaticMembersOnGenericTypes")]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures")]
    public static T Fetch(DbTransaction transaction, Expression<Func<TBusiness, bool>> criteriaExpression, params OrderByInfo[] orderByInfos)
    {
      return Fetch(transaction, CriteriaHelper.ToCriteriaExpression(criteriaExpression), orderByInfos);
    }

    /// <summary>
    /// ����ҵ����󼯺�
    /// </summary>
    /// <param name="transaction">���ݿ�����</param>
    /// <param name="criteriaExpression">�������ʽ</param>
    /// <param name="orderByInfos">��������˳�����</param>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1000:DoNotDeclareStaticMembersOnGenericTypes")]
    public static T Fetch(DbTransaction transaction, CriteriaExpression criteriaExpression, params OrderByInfo[] orderByInfos)
    {
      return Fetch(transaction, new Criterions(typeof(T), criteriaExpression, orderByInfos));
    }

    /// <summary>
    /// ����ҵ����󼯺�
    /// </summary>
    /// <param name="transaction">���ݿ�����</param>
    /// <param name="criterions">������</param>
    /// <param name="lazyFetch">�Ƿ����Fetch(</param>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1000:DoNotDeclareStaticMembersOnGenericTypes")]
    public static T Fetch(DbTransaction transaction, Criterions criterions, bool lazyFetch)
    {
      if (lazyFetch)
      {
        T result = DynamicCreateInstance(criterions);
        result.InLazyFetch = true;
        result.DbTransaction = transaction;
        return result;
      }
      return Fetch(transaction, criterions);
    }

    /// <summary>
    /// ����ҵ����󼯺�
    /// </summary>
    /// <param name="transaction">���ݿ�����</param>
    /// <param name="criterions">������</param>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1000:DoNotDeclareStaticMembersOnGenericTypes")]
    public static T Fetch(DbTransaction transaction, Criterions criterions)
    {
      if (transaction == null)
        throw new ArgumentNullException("transaction");
      return DoFetch(transaction, criterions, true);
    }

    internal static T DoFetch(DbTransaction transaction, Criterions criterions, bool setDetail)
    {
      if (criterions == null)
        criterions = new Criterions(typeof(T));
      T result = DynamicCreateInstance();
      ((IRefinedly)result).ExecuteFetchSelf(transaction, criterions);
      result.Criterions = criterions;
      if (setDetail && result.MasterBusiness != null)
        result.MasterBusiness.SetDetail<T, TBusiness>(result);
      return result;
    }

    private void FetchSelf(DbCommand command)
    {
      using (DbDataReader reader = DbCommandHelper.ExecuteReader(command, CommandBehavior.SingleResult))
      {
        IList<FieldMapInfo> fieldMapInfos = ClassMemberHelper.GetFieldMapInfos(ItemValueType, reader);
        while (reader.Read())
        {
          TBusiness business = BusinessBase<TBusiness>.Fetch(reader, fieldMapInfos);
          lock (this)
          {
            Add(business);
          }
        }
      }
    }

    #endregion

    #region DeleteRecord

    /// <summary>
    /// ɾ����¼
    /// ��������ֶ�ӳ���ϵ����Phenix.Core.Mapping.CriteriaFieldAttribute��ע
    /// </summary>
    /// <param name="criteria">��������</param>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1000:DoNotDeclareStaticMembersOnGenericTypes")]
    [Obsolete("����ʹ��: Phenix.Core.Data.EntityListBase.DeleteRecord()", false)]
    public static int DeleteRecord(ICriteria criteria)
    {
      if (criteria == null)
        throw new ArgumentNullException("criteria");
      return DataHub.DeleteRecord(new Criterions(typeof(T), criteria));
    }

    /// <summary>
    /// ɾ����¼
    /// </summary>
    /// <param name="criteriaExpression">�������ʽ</param>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1000:DoNotDeclareStaticMembersOnGenericTypes")]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures")]
    [Obsolete("����ʹ��: Phenix.Core.Data.EntityListBase.DeleteRecord()", false)]
    public static int DeleteRecord(Expression<Func<TBusiness, bool>> criteriaExpression)
    {
      return DeleteRecord(CriteriaHelper.ToCriteriaExpression(criteriaExpression));
    }

    /// <summary>
    /// ɾ����¼
    /// </summary>
    /// <param name="dataSourceKey">����Դ��</param>
    /// <param name="criteriaExpression">�������ʽ</param>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1000:DoNotDeclareStaticMembersOnGenericTypes")]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures")]
    [Obsolete("����ʹ��: Phenix.Core.Data.EntityListBase.DeleteRecord()", false)]
    public static int DeleteRecord(string dataSourceKey, Expression<Func<TBusiness, bool>> criteriaExpression)
    {
      return DeleteRecord(CriteriaHelper.ToCriteriaExpression(dataSourceKey, criteriaExpression));
    }

    /// <summary>
    /// ɾ����¼
    /// </summary>
    /// <param name="criteriaExpression">�������ʽ</param>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1000:DoNotDeclareStaticMembersOnGenericTypes")]
    [Obsolete("����ʹ��: Phenix.Core.Data.EntityListBase.DeleteRecord()", false)]
    public static int DeleteRecord(CriteriaExpression criteriaExpression)
    {
      if (criteriaExpression == null)
        throw new ArgumentNullException("criteriaExpression");
      return DataHub.DeleteRecord(new Criterions(typeof(T), criteriaExpression));
    }

    /// <summary>
    /// ɾ����¼
    /// ��������ֶ�ӳ���ϵ����Phenix.Core.Mapping.CriteriaFieldAttribute��ע
    /// </summary>
    /// <param name="connection">���ݿ�����</param>
    /// <param name="criteria">��������</param>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1000:DoNotDeclareStaticMembersOnGenericTypes")]
    [Obsolete("����ʹ��: Phenix.Core.Data.EntityListBase.DeleteRecord()", false)]
    public static int DeleteRecord(DbConnection connection, ICriteria criteria)
    {
      if (connection == null)
        throw new ArgumentNullException("connection");
      if (criteria == null)
        throw new ArgumentNullException("criteria");
      return DataHub.DeleteRecord(connection, new Criterions(typeof(T), criteria));
    }

    /// <summary>
    /// ɾ����¼
    /// </summary>
    /// <param name="connection">���ݿ�����</param>
    /// <param name="criteriaExpression">�������ʽ</param>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1000:DoNotDeclareStaticMembersOnGenericTypes")]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures")]
    [Obsolete("����ʹ��: Phenix.Core.Data.EntityListBase.DeleteRecord()", false)]
    public static int DeleteRecord(DbConnection connection, Expression<Func<TBusiness, bool>> criteriaExpression)
    {
      return DeleteRecord(connection, CriteriaHelper.ToCriteriaExpression(criteriaExpression));
    }

    /// <summary>
    /// ɾ����¼
    /// </summary>
    /// <param name="connection">���ݿ�����</param>
    /// <param name="criteriaExpression">�������ʽ</param>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1000:DoNotDeclareStaticMembersOnGenericTypes")]
    [Obsolete("����ʹ��: Phenix.Core.Data.EntityListBase.DeleteRecord()", false)]
    public static int DeleteRecord(DbConnection connection, CriteriaExpression criteriaExpression)
    {
      if (connection == null)
        throw new ArgumentNullException("connection");
      if (criteriaExpression == null)
        throw new ArgumentNullException("criteriaExpression");
      return DataHub.DeleteRecord(connection, new Criterions(typeof(T), criteriaExpression));
    }

    /// <summary>
    /// ɾ����¼
    /// ��������ֶ�ӳ���ϵ����Phenix.Core.Mapping.CriteriaFieldAttribute��ע
    /// </summary>
    /// <param name="transaction">���ݿ�����</param>
    /// <param name="criteria">��������</param>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1000:DoNotDeclareStaticMembersOnGenericTypes")]
    [Obsolete("����ʹ��: Phenix.Core.Data.EntityListBase.DeleteRecord()", false)]
    public static int DeleteRecord(DbTransaction transaction, ICriteria criteria)
    {
      if (transaction == null)
        throw new ArgumentNullException("transaction");
      if (criteria == null)
        throw new ArgumentNullException("criteria");
      return DataHub.DeleteRecord(transaction, new Criterions(typeof(T), criteria));
    }

    /// <summary>
    /// ɾ����¼
    /// </summary>
    /// <param name="transaction">���ݿ�����</param>
    /// <param name="criteriaExpression">�������ʽ</param>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1000:DoNotDeclareStaticMembersOnGenericTypes")]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures")]
    [Obsolete("����ʹ��: Phenix.Core.Data.EntityListBase.DeleteRecord()", false)]
    public static int DeleteRecord(DbTransaction transaction, Expression<Func<TBusiness, bool>> criteriaExpression)
    {
      return DeleteRecord(transaction, CriteriaHelper.ToCriteriaExpression(criteriaExpression));
    }

    /// <summary>
    /// ɾ����¼
    /// </summary>
    /// <param name="transaction">���ݿ�����</param>
    /// <param name="criteriaExpression">�������ʽ</param>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1000:DoNotDeclareStaticMembersOnGenericTypes")]
    [Obsolete("����ʹ��: Phenix.Core.Data.EntityListBase.DeleteRecord()", false)]
    public static int DeleteRecord(DbTransaction transaction, CriteriaExpression criteriaExpression)
    {
      if (transaction == null)
        throw new ArgumentNullException("transaction");
      if (criteriaExpression == null)
        throw new ArgumentNullException("criteriaExpression");
      return DataHub.DeleteRecord(transaction, new Criterions(typeof(T), criteriaExpression));
    }

    #endregion

    #region RecordCount

    /// <summary>
    /// ��ȡ��¼����
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1000:DoNotDeclareStaticMembersOnGenericTypes")]
    public static long GetRecordCount()
    {
      return DataHub.GetRecordCount(new Criterions(typeof(T)));
    }

    /// <summary>
    /// ��ȡ��¼����
    /// </summary>
    /// <param name="masterBusiness">��ҵ�����</param>
    /// <param name="groupName">������, null����ȫ��</param>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1000:DoNotDeclareStaticMembersOnGenericTypes")]
    public static long GetRecordCount(IBusinessObject masterBusiness, string groupName)
    {
      return DataHub.GetRecordCount(new Criterions(typeof(T), masterBusiness, groupName));
    }

    /// <summary>
    /// ��ȡ��¼����
    /// ��������ֶ�ӳ���ϵ����Phenix.Core.Mapping.CriteriaFieldAttribute��ע
    /// </summary>
    /// <param name="criteria">��������</param>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1000:DoNotDeclareStaticMembersOnGenericTypes")]
    public static long GetRecordCount(ICriteria criteria)
    {
      return DataHub.GetRecordCount(new Criterions(typeof(T), criteria));
    }
    
    /// <summary>
    /// ��ȡ��¼����
    /// ��������ֶ�ӳ���ϵ����Phenix.Core.Mapping.CriteriaFieldAttribute��ע
    /// </summary>
    /// <param name="criteria">��������</param>
    /// <param name="masterBusiness">��ҵ�����</param>
    /// <param name="groupName">������, null����ȫ��</param>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1000:DoNotDeclareStaticMembersOnGenericTypes")]
    public static long GetRecordCount(ICriteria criteria, IBusinessObject masterBusiness, string groupName)
    {
      return DataHub.GetRecordCount(new Criterions(typeof(T), criteria, masterBusiness, groupName));
    }
    
    /// <summary>
    /// ��ȡ��¼����
    /// </summary>
    /// <param name="criteriaExpression">�������ʽ</param>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1000:DoNotDeclareStaticMembersOnGenericTypes")]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures")]
    public static long GetRecordCount(Expression<Func<TBusiness, bool>> criteriaExpression)
    {
      return GetRecordCount(CriteriaHelper.ToCriteriaExpression(criteriaExpression));
    }

    /// <summary>
    /// ��ȡ��¼����
    /// </summary>
    /// <param name="dataSourceKey">����Դ��</param>
    /// <param name="criteriaExpression">�������ʽ</param>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1000:DoNotDeclareStaticMembersOnGenericTypes")]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures")]
    public static long GetRecordCount(string dataSourceKey, Expression<Func<TBusiness, bool>> criteriaExpression)
    {
      return GetRecordCount(CriteriaHelper.ToCriteriaExpression(dataSourceKey, criteriaExpression));
    }

    /// <summary>
    /// ��ȡ��¼����
    /// </summary>
    /// <param name="criteriaExpression">�������ʽ</param>
    /// <param name="masterBusiness">��ҵ�����</param>
    /// <param name="groupName">������, null����ȫ��</param>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1000:DoNotDeclareStaticMembersOnGenericTypes")]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures")]
    public static long GetRecordCount(Expression<Func<TBusiness, bool>> criteriaExpression, IBusinessObject masterBusiness, string groupName)
    {
      return GetRecordCount(CriteriaHelper.ToCriteriaExpression(criteriaExpression), masterBusiness, groupName);
    }

    /// <summary>
    /// ��ȡ��¼����
    /// </summary>
    /// <param name="dataSourceKey">����Դ��</param>
    /// <param name="criteriaExpression">�������ʽ</param>
    /// <param name="masterBusiness">��ҵ�����</param>
    /// <param name="groupName">������, null����ȫ��</param>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1000:DoNotDeclareStaticMembersOnGenericTypes")]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures")]
    public static long GetRecordCount(string dataSourceKey, Expression<Func<TBusiness, bool>> criteriaExpression, IBusinessObject masterBusiness, string groupName)
    {
      return GetRecordCount(CriteriaHelper.ToCriteriaExpression(dataSourceKey, criteriaExpression), masterBusiness, groupName);
    }
    
    /// <summary>
    /// ��ȡ��¼����
    /// </summary>
    /// <param name="criteriaExpression">�������ʽ</param>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1000:DoNotDeclareStaticMembersOnGenericTypes")]
    public static long GetRecordCount(CriteriaExpression criteriaExpression)
    {
      return DataHub.GetRecordCount(new Criterions(typeof(T), criteriaExpression));
    }

    /// <summary>
    /// ��ȡ��¼����
    /// </summary>
    /// <param name="criteriaExpression">�������ʽ</param>
    /// <param name="masterBusiness">��ҵ�����</param>
    /// <param name="groupName">������, null����ȫ��</param>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1000:DoNotDeclareStaticMembersOnGenericTypes")]
    public static long GetRecordCount(CriteriaExpression criteriaExpression, IBusinessObject masterBusiness, string groupName)
    {
      return DataHub.GetRecordCount(new Criterions(typeof(T), criteriaExpression, masterBusiness, groupName));
    }

    /// <summary>
    /// ��ȡ��¼����
    /// </summary>
    /// <param name="connection">���ݿ�����</param>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1000:DoNotDeclareStaticMembersOnGenericTypes")]
    public static long GetRecordCount(DbConnection connection)
    {
      if (connection == null)
        throw new ArgumentNullException("connection");
      return DataHub.GetRecordCount(connection, new Criterions(typeof(T)));
    }

    /// <summary>
    /// ��ȡ��¼����
    /// </summary>
    /// <param name="connection">���ݿ�����</param>
    /// <param name="masterBusiness">��ҵ�����</param>
    /// <param name="groupName">������, null����ȫ��</param>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1000:DoNotDeclareStaticMembersOnGenericTypes")]
    public static long GetRecordCount(DbConnection connection, IBusinessObject masterBusiness, string groupName)
    {
      if (connection == null)
        throw new ArgumentNullException("connection");
      return DataHub.GetRecordCount(connection, new Criterions(typeof(T), masterBusiness, groupName));
    }

    /// <summary>
    /// ��ȡ��¼����
    /// ��������ֶ�ӳ���ϵ����Phenix.Core.Mapping.CriteriaFieldAttribute��ע
    /// </summary>
    /// <param name="connection">���ݿ�����</param>
    /// <param name="criteria">��������</param>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1000:DoNotDeclareStaticMembersOnGenericTypes")]
    public static long GetRecordCount(DbConnection connection, ICriteria criteria)
    {
      if (connection == null)
        throw new ArgumentNullException("connection");
      return DataHub.GetRecordCount(connection, new Criterions(typeof(T), criteria));
    }

    /// <summary>
    /// ��ȡ��¼����
    /// ��������ֶ�ӳ���ϵ����Phenix.Core.Mapping.CriteriaFieldAttribute��ע
    /// </summary>
    /// <param name="connection">���ݿ�����</param>
    /// <param name="criteria">��������</param>
    /// <param name="masterBusiness">��ҵ�����</param>
    /// <param name="groupName">������, null����ȫ��</param>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1000:DoNotDeclareStaticMembersOnGenericTypes")]
    public static long GetRecordCount(DbConnection connection, ICriteria criteria, IBusinessObject masterBusiness, string groupName)
    {
      if (connection == null)
        throw new ArgumentNullException("connection");
      return DataHub.GetRecordCount(connection, new Criterions(typeof(T), criteria, masterBusiness, groupName));
    }

    /// <summary>
    /// ��ȡ��¼����
    /// </summary>
    /// <param name="connection">���ݿ�����</param>
    /// <param name="criteriaExpression">�������ʽ</param>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1000:DoNotDeclareStaticMembersOnGenericTypes")]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures")]
    public static long GetRecordCount(DbConnection connection, Expression<Func<TBusiness, bool>> criteriaExpression)
    {
      return GetRecordCount(connection, CriteriaHelper.ToCriteriaExpression(criteriaExpression));
    }

    /// <summary>
    /// ��ȡ��¼����
    /// </summary>
    /// <param name="connection">���ݿ�����</param>
    /// <param name="criteriaExpression">�������ʽ</param>
    /// <param name="masterBusiness">��ҵ�����</param>
    /// <param name="groupName">������, null����ȫ��</param>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1000:DoNotDeclareStaticMembersOnGenericTypes")]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures")]
    public static long GetRecordCount(DbConnection connection, Expression<Func<TBusiness, bool>> criteriaExpression, IBusinessObject masterBusiness, string groupName)
    {
      return GetRecordCount(connection, CriteriaHelper.ToCriteriaExpression(criteriaExpression), masterBusiness, groupName);
    }

    /// <summary>
    /// ��ȡ��¼����
    /// </summary>
    /// <param name="connection">���ݿ�����</param>
    /// <param name="criteriaExpression">�������ʽ</param>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1000:DoNotDeclareStaticMembersOnGenericTypes")]
    public static long GetRecordCount(DbConnection connection, CriteriaExpression criteriaExpression)
    {
      if (connection == null)
        throw new ArgumentNullException("connection");
      return DataHub.GetRecordCount(connection, new Criterions(typeof(T), criteriaExpression));
    }

    /// <summary>
    /// ��ȡ��¼����
    /// </summary>
    /// <param name="connection">���ݿ�����</param>
    /// <param name="criteriaExpression">�������ʽ</param>
    /// <param name="masterBusiness">��ҵ�����</param>
    /// <param name="groupName">������, null����ȫ��</param>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1000:DoNotDeclareStaticMembersOnGenericTypes")]
    public static long GetRecordCount(DbConnection connection, CriteriaExpression criteriaExpression, IBusinessObject masterBusiness, string groupName)
    {
      if (connection == null)
        throw new ArgumentNullException("connection");
      return DataHub.GetRecordCount(connection, new Criterions(typeof(T), criteriaExpression, masterBusiness, groupName));
    }

    /// <summary>
    /// ��ȡ��¼����
    /// </summary>
    /// <param name="transaction">���ݿ�����</param>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1000:DoNotDeclareStaticMembersOnGenericTypes")]
    public static long GetRecordCount(DbTransaction transaction)
    {
      if (transaction == null)
        throw new ArgumentNullException("transaction");
      return DataHub.GetRecordCount(transaction, new Criterions(typeof(T)));
    }

    /// <summary>
    /// ��ȡ��¼����
    /// </summary>
    /// <param name="transaction">���ݿ�����</param>
    /// <param name="masterBusiness">��ҵ�����</param>
    /// <param name="groupName">������, null����ȫ��</param>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1000:DoNotDeclareStaticMembersOnGenericTypes")]
    public static long GetRecordCount(DbTransaction transaction, IBusinessObject masterBusiness, string groupName)
    {
      if (transaction == null)
        throw new ArgumentNullException("transaction");
      return DataHub.GetRecordCount(transaction, new Criterions(typeof(T), masterBusiness, groupName));
    }

    /// <summary>
    /// ��ȡ��¼����
    /// ��������ֶ�ӳ���ϵ����Phenix.Core.Mapping.CriteriaFieldAttribute��ע
    /// </summary>
    /// <param name="transaction">���ݿ�����</param>
    /// <param name="criteria">��������</param>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1000:DoNotDeclareStaticMembersOnGenericTypes")]
    public static long GetRecordCount(DbTransaction transaction, ICriteria criteria)
    {
      if (transaction == null)
        throw new ArgumentNullException("transaction");
      return DataHub.GetRecordCount(transaction, new Criterions(typeof(T), criteria));
    }

    /// <summary>
    /// ��ȡ��¼����
    /// ��������ֶ�ӳ���ϵ����Phenix.Core.Mapping.CriteriaFieldAttribute��ע
    /// </summary>
    /// <param name="transaction">���ݿ�����</param>
    /// <param name="criteria">��������</param>
    /// <param name="masterBusiness">��ҵ�����</param>
    /// <param name="groupName">������, null����ȫ��</param>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1000:DoNotDeclareStaticMembersOnGenericTypes")]
    public static long GetRecordCount(DbTransaction transaction, ICriteria criteria, IBusinessObject masterBusiness, string groupName)
    {
      if (transaction == null)
        throw new ArgumentNullException("transaction");
      return DataHub.GetRecordCount(transaction, new Criterions(typeof(T), criteria, masterBusiness, groupName));
    }

    /// <summary>
    /// ��ȡ��¼����
    /// </summary>
    /// <param name="transaction">���ݿ�����</param>
    /// <param name="criteriaExpression">�������ʽ</param>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1000:DoNotDeclareStaticMembersOnGenericTypes")]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures")]
    public static long GetRecordCount(DbTransaction transaction, Expression<Func<TBusiness, bool>> criteriaExpression)
    {
      return GetRecordCount(transaction, CriteriaHelper.ToCriteriaExpression(criteriaExpression));
    }

    /// <summary>
    /// ��ȡ��¼����
    /// </summary>
    /// <param name="transaction">���ݿ�����</param>
    /// <param name="criteriaExpression">�������ʽ</param>
    /// <param name="masterBusiness">��ҵ�����</param>
    /// <param name="groupName">������, null����ȫ��</param>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1000:DoNotDeclareStaticMembersOnGenericTypes")]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures")]
    public static long GetRecordCount(DbTransaction transaction, Expression<Func<TBusiness, bool>> criteriaExpression, IBusinessObject masterBusiness, string groupName)
    {
      return GetRecordCount(transaction, CriteriaHelper.ToCriteriaExpression(criteriaExpression), masterBusiness, groupName);
    }

    /// <summary>
    /// ��ȡ��¼����
    /// </summary>
    /// <param name="transaction">���ݿ�����</param>
    /// <param name="criteriaExpression">�������ʽ</param>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1000:DoNotDeclareStaticMembersOnGenericTypes")]
    public static long GetRecordCount(DbTransaction transaction, CriteriaExpression criteriaExpression)
    {
      if (transaction == null)
        throw new ArgumentNullException("transaction");
      return DataHub.GetRecordCount(transaction, new Criterions(typeof(T), criteriaExpression));
    }

    /// <summary>
    /// ��ȡ��¼����
    /// </summary>
    /// <param name="transaction">���ݿ�����</param>
    /// <param name="criteriaExpression">�������ʽ</param>
    /// <param name="masterBusiness">��ҵ�����</param>
    /// <param name="groupName">������, null����ȫ��</param>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1000:DoNotDeclareStaticMembersOnGenericTypes")]
    public static long GetRecordCount(DbTransaction transaction, CriteriaExpression criteriaExpression, IBusinessObject masterBusiness, string groupName)
    {
      if (transaction == null)
        throw new ArgumentNullException("transaction");
      return DataHub.GetRecordCount(transaction, new Criterions(typeof(T), criteriaExpression, masterBusiness, groupName));
    }

    #endregion

    #region Exists

    /// <summary>
    /// ������
    /// </summary>
    /// <param name="criterions">������</param>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1000:DoNotDeclareStaticMembersOnGenericTypes")]
    public static CriteriaExpressionSub Exists(ICriterions criterions)
    {
      if (criterions == null)
        criterions = new Criterions(typeof(T));
      return CriteriaExpressionSub.Exists(criterions);
    }

    /// <summary>
    /// ������
    /// </summary>
    /// <param name="orderByInfos">��������˳�����</param>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1000:DoNotDeclareStaticMembersOnGenericTypes")]
    public static CriteriaExpressionSub Exists(params OrderByInfo[] orderByInfos)
    {
      return CriteriaExpressionSub.Exists(new Criterions(typeof(T), orderByInfos));
    }

    /// <summary>
    /// ������
    /// </summary>
    /// <param name="masterBusiness">��ҵ�����</param>
    /// <param name="groupName">������, null����ȫ��</param>
    /// <param name="orderByInfos">��������˳�����</param>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1000:DoNotDeclareStaticMembersOnGenericTypes")]
    public static CriteriaExpressionSub Exists(IBusinessObject masterBusiness, string groupName, params OrderByInfo[] orderByInfos)
    {
      return CriteriaExpressionSub.Exists(new Criterions(typeof(T), masterBusiness, groupName, orderByInfos));
    }

    /// <summary>
    /// ������
    /// ��������ֶ�ӳ���ϵ����Phenix.Core.Mapping.CriteriaFieldAttribute��ע
    /// </summary>
    /// <param name="criteria">��������</param>
    /// <param name="orderByInfos">��������˳�����</param>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1000:DoNotDeclareStaticMembersOnGenericTypes")]
    public static CriteriaExpressionSub Exists(ICriteria criteria, params OrderByInfo[] orderByInfos)
    {
      return CriteriaExpressionSub.Exists(new Criterions(typeof(T), criteria, orderByInfos));
    }

    /// <summary>
    /// ������
    /// ��������ֶ�ӳ���ϵ����Phenix.Core.Mapping.CriteriaFieldAttribute��ע
    /// </summary>
    /// <param name="criteria">��������</param>
    /// <param name="masterBusiness">��ҵ�����</param>
    /// <param name="groupName">������, null����ȫ��</param>
    /// <param name="orderByInfos">��������˳�����</param>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1000:DoNotDeclareStaticMembersOnGenericTypes")]
    public static CriteriaExpressionSub Exists(ICriteria criteria, IBusinessObject masterBusiness, string groupName, params OrderByInfo[] orderByInfos)
    {
      return CriteriaExpressionSub.Exists(new Criterions(typeof(T), criteria, masterBusiness, groupName, orderByInfos));
    }

    /// <summary>
    /// ������
    /// </summary>
    /// <param name="criteriaExpression">�������ʽ</param>
    /// <param name="orderByInfos">��������˳�����</param>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1000:DoNotDeclareStaticMembersOnGenericTypes")]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures")]
    public static CriteriaExpressionSub Exists(Expression<Func<TBusiness, bool>> criteriaExpression, params OrderByInfo[] orderByInfos)
    {
      return Exists(CriteriaHelper.ToCriteriaExpression(criteriaExpression), orderByInfos);
    }

    /// <summary>
    /// ������
    /// </summary>
    /// <param name="criteriaExpression">�������ʽ</param>
    /// <param name="masterBusiness">��ҵ�����</param>
    /// <param name="groupName">������, null����ȫ��</param>
    /// <param name="orderByInfos">��������˳�����</param>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1000:DoNotDeclareStaticMembersOnGenericTypes")]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures")]
    public static CriteriaExpressionSub Exists(Expression<Func<TBusiness, bool>> criteriaExpression, IBusinessObject masterBusiness, string groupName, params OrderByInfo[] orderByInfos)
    {
      return Exists(CriteriaHelper.ToCriteriaExpression(criteriaExpression), masterBusiness, groupName, orderByInfos);
    }

    /// <summary>
    /// ������
    /// </summary>
    /// <param name="criteriaExpression">�������ʽ</param>
    /// <param name="orderByInfos">��������˳�����</param>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1000:DoNotDeclareStaticMembersOnGenericTypes")]
    public static CriteriaExpressionSub Exists(CriteriaExpression criteriaExpression, params OrderByInfo[] orderByInfos)
    {
      return CriteriaExpressionSub.Exists(new Criterions(typeof(T), criteriaExpression, orderByInfos));
    }

    /// <summary>
    /// ������
    /// </summary>
    /// <param name="criteriaExpression">�������ʽ</param>
    /// <param name="masterBusiness">��ҵ�����</param>
    /// <param name="groupName">������, null����ȫ��</param>
    /// <param name="orderByInfos">��������˳�����</param>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1000:DoNotDeclareStaticMembersOnGenericTypes")]
    public static CriteriaExpressionSub Exists(CriteriaExpression criteriaExpression, IBusinessObject masterBusiness, string groupName, params OrderByInfo[] orderByInfos)
    {
      return CriteriaExpressionSub.Exists(new Criterions(typeof(T), criteriaExpression, masterBusiness, groupName, orderByInfos));
    }

    /// <summary>
    /// ��������
    /// </summary>
    /// <param name="criterions">������</param>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1000:DoNotDeclareStaticMembersOnGenericTypes")]
    public static CriteriaExpressionSub NotExists(ICriterions criterions)
    {
      if (criterions == null)
        criterions = new Criterions(typeof(T));
      return CriteriaExpressionSub.NotExists(criterions);
    }

    /// <summary>
    /// ��������
    /// </summary>
    /// <param name="orderByInfos">��������˳�����</param>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1000:DoNotDeclareStaticMembersOnGenericTypes")]
    public static CriteriaExpressionSub NotExists(params OrderByInfo[] orderByInfos)
    {
      return CriteriaExpressionSub.NotExists(new Criterions(typeof(T), orderByInfos));
    }

    /// <summary>
    /// ��������
    /// </summary>
    /// <param name="masterBusiness">��ҵ�����</param>
    /// <param name="groupName">������, null����ȫ��</param>
    /// <param name="orderByInfos">��������˳�����</param>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1000:DoNotDeclareStaticMembersOnGenericTypes")]
    public static CriteriaExpressionSub NotExists(IBusinessObject masterBusiness, string groupName, params OrderByInfo[] orderByInfos)
    {
      return CriteriaExpressionSub.NotExists(new Criterions(typeof(T), masterBusiness, groupName, orderByInfos));
    }

    /// <summary>
    /// ��������
    /// ��������ֶ�ӳ���ϵ����Phenix.Core.Mapping.CriteriaFieldAttribute��ע
    /// </summary>
    /// <param name="criteria">��������</param>
    /// <param name="orderByInfos">��������˳�����</param>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1000:DoNotDeclareStaticMembersOnGenericTypes")]
    public static CriteriaExpressionSub NotExists(ICriteria criteria, params OrderByInfo[] orderByInfos)
    {
      return CriteriaExpressionSub.NotExists(new Criterions(typeof(T), criteria, orderByInfos));
    }

    /// <summary>
    /// ��������
    /// ��������ֶ�ӳ���ϵ����Phenix.Core.Mapping.CriteriaFieldAttribute��ע
    /// </summary>
    /// <param name="criteria">��������</param>
    /// <param name="masterBusiness">��ҵ�����</param>
    /// <param name="groupName">������, null����ȫ��</param>
    /// <param name="orderByInfos">��������˳�����</param>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1000:DoNotDeclareStaticMembersOnGenericTypes")]
    public static CriteriaExpressionSub NotExists(ICriteria criteria, IBusinessObject masterBusiness, string groupName, params OrderByInfo[] orderByInfos)
    {
      return CriteriaExpressionSub.NotExists(new Criterions(typeof(T), criteria, masterBusiness, groupName, orderByInfos));
    }

    /// <summary>
    /// ��������
    /// </summary>
    /// <param name="criteriaExpression">�������ʽ</param>
    /// <param name="orderByInfos">��������˳�����</param>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1000:DoNotDeclareStaticMembersOnGenericTypes")]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures")]
    public static CriteriaExpressionSub NotExists(Expression<Func<TBusiness, bool>> criteriaExpression, params OrderByInfo[] orderByInfos)
    {
      return NotExists(CriteriaHelper.ToCriteriaExpression(criteriaExpression), orderByInfos);
    }

    /// <summary>
    /// ��������
    /// </summary>
    /// <param name="criteriaExpression">�������ʽ</param>
    /// <param name="masterBusiness">��ҵ�����</param>
    /// <param name="groupName">������, null����ȫ��</param>
    /// <param name="orderByInfos">��������˳�����</param>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1000:DoNotDeclareStaticMembersOnGenericTypes")]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures")]
    public static CriteriaExpressionSub NotExists(Expression<Func<TBusiness, bool>> criteriaExpression, IBusinessObject masterBusiness, string groupName, params OrderByInfo[] orderByInfos)
    {
      return NotExists(CriteriaHelper.ToCriteriaExpression(criteriaExpression), masterBusiness, groupName, orderByInfos);
    }

    /// <summary>
    /// ��������
    /// </summary>
    /// <param name="criteriaExpression">�������ʽ</param>
    /// <param name="orderByInfos">��������˳�����</param>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1000:DoNotDeclareStaticMembersOnGenericTypes")]
    public static CriteriaExpressionSub NotExists(CriteriaExpression criteriaExpression, params OrderByInfo[] orderByInfos)
    {
      return CriteriaExpressionSub.NotExists(new Criterions(typeof(T), criteriaExpression, orderByInfos));
    }

    /// <summary>
    /// ��������
    /// </summary>
    /// <param name="criteriaExpression">�������ʽ</param>
    /// <param name="masterBusiness">��ҵ�����</param>
    /// <param name="groupName">������, null����ȫ��</param>
    /// <param name="orderByInfos">��������˳�����</param>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1000:DoNotDeclareStaticMembersOnGenericTypes")]
    public static CriteriaExpressionSub NotExists(CriteriaExpression criteriaExpression, IBusinessObject masterBusiness, string groupName, params OrderByInfo[] orderByInfos)
    {
      return CriteriaExpressionSub.NotExists(new Criterions(typeof(T), criteriaExpression, masterBusiness, groupName, orderByInfos));
    }

    #endregion

    /// <summary>
    /// ˢ��ҵ����󼯺�
    /// </summary>
    /// <param name="self">ҵ����󼯺�</param>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1000:DoNotDeclareStaticMembersOnGenericTypes")]
    public static T Refresh(T self)
    {
      return Fetch(self.Criterions);
    }

    #endregion

    #region ����

    /// <summary>
    /// ����Դ��
    /// ȱʡΪ Root.DataSourceKey
    /// ȱʡΪ Criterions.DataSourceKey
    /// ȱʡΪ T��TBusiness �ϵ� ClassAttribute.DataSourceKey
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
    /// ������
    /// </summary>
    [Newtonsoft.Json.JsonIgnore]
    [System.ComponentModel.Browsable(false)]
    [System.ComponentModel.DataAnnotations.Display(AutoGenerateField = false)]
    public new Criterions Criterions
    {
      get { return _criterions; }
      internal set { _criterions = value; }
    }
    ICriterions IEntityCollection.Criterions
    {
      get { return Criterions; }
    }
    Criterions IBusiness.Criterions
    {
      get { return Criterions; }
    }
    Criterions IBusinessCollection.Criterions
    {
      get { return Criterions; }
    }

    [NonSerialized]
    [Csla.NotUndoable]
    private ReadOnlyCollection<OrderByInfo> _orderByInfos;
    /// <summary>
    /// ��������˳�����
    /// ȱʡΪ��ѯʱ�����OrderByInfo���ֶ��ϵ�FieldOrderByAttribute��ǩ
    /// </summary>
    [Newtonsoft.Json.JsonIgnore]
    [System.ComponentModel.Browsable(false)]
    [System.ComponentModel.DataAnnotations.Display(AutoGenerateField = false)]
    public virtual IList<OrderByInfo> OrderByInfos
    {
      get
      {
        if (_orderByInfos == null)
        {
          List<OrderByInfo> result = new List<OrderByInfo>();
          if (Criterions != null && Criterions.OrderByInfos != null)
            result.AddRange(Criterions.OrderByInfos);
          ClassMapInfo classMapInfo = ClassMemberHelper.GetClassMapInfo(ItemValueType);
          result.AddRange(classMapInfo.OrderByInfos);
          _orderByInfos = result.AsReadOnly();
        }
        return _orderByInfos;
      }
    }

    /// <summary>
    /// �ȴ�Fetchִ�е�ʱ��(������, null ָʾ������
    /// ȱʡΪ null
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
    /// ������ȫ����
    /// Ϊ Root �ϵ� HistoryAttribute.FetchAll
    /// null: ������
    /// true: ����+��ʷ��
    /// false: ��ʷ��
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

    #region LazyFetch

    /// <summary>
    /// ��ȡʵ�ʰ�����Ԫ����
    /// </summary>
    public new int Count
    {
      get
      {
        LazyFetch(false);
        return base.Count;
      }
    }

    /// <summary>
    /// ��ȡ��Χ�� IList
    /// </summary>
    protected new IList<TBusiness> Items
    {
      get
      {
        LazyFetch(false);
        return base.Items;
      }
    }

    /// <summary>
    /// ��ȡ������ָ����������Ԫ��
    /// </summary>
    /// <param name="index">Ҫ��û����õ�Ԫ�ش��㿪ʼ������</param>
    [System.ComponentModel.Browsable(false)]
    [System.ComponentModel.DataAnnotations.Display(AutoGenerateField = false)]
    public new TBusiness this[int index]
    {
      get
      {
        LazyFetch(false);
        return base[index];
      }
      set
      {
        LazyFetch(false);
        base[index] = value;
      }
    }

    [NonSerialized]
    [Csla.NotUndoable]
    private bool _inLazyFetch;
    /// <summary>
    /// �Ƿ��ڶ���Fetch��
    /// Fetchʱ����������, ���������������ҵ�����ʱ����ʽ����, ������ɺ��Զ���Ϊ false
    /// </summary>
    [Newtonsoft.Json.JsonIgnore]
    [System.ComponentModel.Browsable(false)]
    [System.ComponentModel.DataAnnotations.Display(AutoGenerateField = false)]
    public bool InLazyFetch
    {
      get { return _inLazyFetch; }
      private set { _inLazyFetch = value; }
    }

    #endregion

    #region �ṹ��ϵ

    /// <summary>
    /// Parent
    /// </summary>
    [Obsolete("����ʹ��: MasterBusiness", false)]
    [Newtonsoft.Json.JsonIgnore]
    [System.ComponentModel.Browsable(false)]
    [System.ComponentModel.DataAnnotations.Display(AutoGenerateField = false)]
    public new Csla.Core.IParent Parent
    {
      get { return base.Parent; }
    }

    /// <summary>
    /// �Ƿ��Ǹ�����
    /// ������Ӹ������Ͻ���(����)����
    /// </summary>
    [Newtonsoft.Json.JsonIgnore]
    [System.ComponentModel.Browsable(false)]
    [System.ComponentModel.DataAnnotations.Display(AutoGenerateField = false)]
    public bool IsRoot
    {
      get { return object.ReferenceEquals(Root, this); }
    }

    /// <summary>
    /// ������
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
          return this;
        IBusiness result = masterBusiness.Root;
        if (result != null)
          return result;
        return masterBusiness;
      }
    }

    /// <summary>
    /// ��ҵ�����
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
          return null;
        IBusinessObject result = masterBusiness.RootBusiness;
        if (result != null)
          return result;
        return masterBusiness;
      }
    }

    /// <summary>
    /// ��ҵ�����
    /// </summary>
    [Newtonsoft.Json.JsonIgnore]
    [System.ComponentModel.Browsable(false)]
    [System.ComponentModel.DataAnnotations.Display(AutoGenerateField = false)]
    public IBusinessObject MasterBusiness
    {
      get { return Criterions != null ? Criterions.MasterBusiness : base.Parent as IBusinessObject; }
    }

    /// <summary>
    /// ������
    /// </summary>
    [Newtonsoft.Json.JsonIgnore]
    [System.ComponentModel.Browsable(false)]
    [System.ComponentModel.DataAnnotations.Display(AutoGenerateField = false)]
    public string GroupName
    {
      get { return Criterions != null ? Criterions.GroupName : null; }
    }

    Type IEntityCollection.ItemValueType
    {
      get { return ItemValueType; }
    }

    [NonSerialized]
    [Csla.NotUndoable]
    private bool _itemLazyGetDetail;
    /// <summary>
    /// ҵ��������GetDetail
    /// ȱʡΪ false
    /// </summary>
    [Newtonsoft.Json.JsonIgnore]
    public bool ItemLazyGetDetail
    {
      get { return _itemLazyGetDetail; }
      set { _itemLazyGetDetail = value; }
    }

    #endregion

    #region �༭����

    bool IBusiness.IsReadOnly
    {
      get { return IsReadOnly; }
    }
    bool IBusinessCollection.IsReadOnly
    {
      get { return IsReadOnly; }
    }

    /// <summary>
    /// ������༶���������Detail����Ķ༶����
    /// ȱʡΪ false
    /// </summary>
    protected virtual bool NotUndoable
    {
      get { return MasterBusiness != null && MasterBusiness.NotUndoable; }
    }
    bool IBusiness.NotUndoable
    {
      get { return NotUndoable; }
    }

    /// <summary>
    /// �ڱ༭״̬
    /// </summary>
    [Newtonsoft.Json.JsonIgnore]
    [System.ComponentModel.Browsable(false)]
    [System.ComponentModel.DataAnnotations.Display(AutoGenerateField = false)]
    public bool EditMode
    {
      get { return base.EditLevel > 0 || NotUndoable; }
    }
    /// <summary>
    /// �ڱ༭״̬
    /// </summary>
    [Newtonsoft.Json.JsonIgnore]
    [System.ComponentModel.Browsable(false)]
    [System.ComponentModel.DataAnnotations.Display(AutoGenerateField = false)]
    public BooleanOption EditModeOption
    {
      get { return EditMode ? BooleanOption.Y : BooleanOption.N; }
    }

    /// <summary>
    /// �༭�㼶
    /// </summary>
    [Newtonsoft.Json.JsonIgnore]
    [System.ComponentModel.Browsable(false)]
    [System.ComponentModel.DataAnnotations.Display(AutoGenerateField = false)]
    public new int EditLevel
    {
      get { return base.EditLevel; }
    }


    /// <summary>
    /// �Ƿ��������ҵ�����
    /// </summary>
    [System.ComponentModel.Browsable(false)]
    [System.ComponentModel.DataAnnotations.Display(AutoGenerateField = false)]
    public override bool AllowAddItem
    {
      get
      {
        if (InSelectableList)
          return false;
        return base.AllowAddItem;
      }
    }

    /// <summary>
    /// �Ƿ�����ɾ��ҵ�����
    /// </summary>
    [System.ComponentModel.Browsable(false)]
    [System.ComponentModel.DataAnnotations.Display(AutoGenerateField = false)]
    public override bool AllowDeleteItem
    {
      get
      {
        if (InSelectableList)
          return false;
        return base.AllowDeleteItem;
      }
    }

    /// <summary>
    /// �Ƿ���Ա�������
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
        bool auth = BusinessBase<TBusiness>.CanEdit;
        return auth && IsDirty && IsValid && !IsBusy;
      }
    }

    #endregion

    #region Selectable

    [Csla.NotUndoable]
    private bool _onlySaveSelected;
    /// <summary>
    /// ���ύ����ѡ��ҵ�����
    /// ȱʡΪ false
    /// </summary>
    [Newtonsoft.Json.JsonIgnore]
    [System.ComponentModel.Browsable(false)]
    [System.ComponentModel.DataAnnotations.Display(AutoGenerateField = false)]
    public virtual bool OnlySaveSelected
    {
      get { return _onlySaveSelected; }
      private set { _onlySaveSelected = value; }
    }

    [NonSerialized]
    [Csla.NotUndoable]
    private bool _inSelectableList;
    /// <summary>
    /// �ڹ�ѡ���嵥
    /// </summary>
    [Newtonsoft.Json.JsonIgnore]
    [System.ComponentModel.Browsable(false)]
    [System.ComponentModel.DataAnnotations.Display(AutoGenerateField = false)]
    public bool InSelectableList
    {
      get { return _inSelectableList; }
      private set { _inSelectableList = value; }
    }
    
    [NonSerialized]
    [Csla.NotUndoable]
    private bool _emptyIsAllSelected;
    private bool EmptyIsAllSelected
    {
      get { return _emptyIsAllSelected; }
      set { _emptyIsAllSelected = value; }
    }

    #region ISelectableCollection ��Ա

    [NonSerialized]
    [Csla.NotUndoable]
    private Dictionary<Type, EnumKeyCaptionCollection> _selectableEnumKeyCaptionCollections;
    private Dictionary<Type, EnumKeyCaptionCollection> SelectableEnumKeyCaptionCollections
    {
      get
      {
        if (_selectableEnumKeyCaptionCollections == null)
          _selectableEnumKeyCaptionCollections = new Dictionary<Type, EnumKeyCaptionCollection>();
        return _selectableEnumKeyCaptionCollections;
      }
    }

    [NonSerialized]
    [Csla.NotUndoable]
    private Dictionary<string, T> _selectables;
    private Dictionary<string, T> Selectables
    {
      get
      {
        if (_selectables == null)
          _selectables = new Dictionary<string, T>(StringComparer.Ordinal);
        return _selectables;
      }
    }

    [NonSerialized]
    [Csla.NotUndoable]
    private Dictionary<string, IBusinessCollection> _selectableBusinessCollections;
    private Dictionary<string, IBusinessCollection> SelectableBusinessCollections
    {
      get
      {
        if (_selectableBusinessCollections == null)
          _selectableBusinessCollections = new Dictionary<string, IBusinessCollection>(StringComparer.Ordinal);
        return _selectableBusinessCollections;
      }
    }

    [NonSerialized]
    [Csla.NotUndoable]
    private List<ISelectable> _selectedItems;
    /// <summary>
    /// ����ѡ�Ķ������
    /// </summary>
    [Newtonsoft.Json.JsonIgnore]
    [System.ComponentModel.Browsable(false)]
    [System.ComponentModel.DataAnnotations.Display(AutoGenerateField = false)]
    public IList<ISelectable> SelectedItems
    {
      get
      {
        if (_selectedItems == null)
          _selectedItems = new List<ISelectable>(Count);
        return _selectedItems;
      }
    }

    bool IEntityCollection.RaiseListChangedEvents
    {
      get { return RaiseListChangedEvents; }
      set { RaiseListChangedEvents = value; }
    }

    #endregion

    #endregion

    #region ��̬ˢ��

    [NonSerialized]
    [Csla.NotUndoable]
    private int _renovateDataKeyCount;
    private int RenovateDataKeyCount
    {
      get { return _renovateDataKeyCount; }
      set { _renovateDataKeyCount = value; }
    }

    [NonSerialized]
    [Csla.NotUndoable]
    private IList<FieldMapInfo> _renovateFieldMapInfos;
    private IList<FieldMapInfo> RenovateFieldMapInfos
    {
      get { return _renovateFieldMapInfos; }
      set { _renovateFieldMapInfos = value; }
    }

    [NonSerialized]
    [Csla.NotUndoable]
    private Dictionary<string, TBusiness> _renovateIndex;
    private Dictionary<string, TBusiness> RenovateIndex
    {
      get
      {
        if (_renovateIndex == null)
          _renovateIndex = new Dictionary<string, TBusiness>(StringComparer.Ordinal);
        return _renovateIndex;
      }
    }

    #endregion

    #region Data Access

    /// <summary>
    /// �Ƿ���Save?
    /// </summary>
    [Newtonsoft.Json.JsonIgnore]
    [System.ComponentModel.Browsable(false)]
    [System.ComponentModel.DataAnnotations.Display(AutoGenerateField = false)]
    public bool CascadingSave
    {
      get { return MasterBusiness == null || (Criterions == null || Criterions.CascadingSave); }
    }

    /// <summary>
    /// �Ƿ���Delete?
    /// ��� CascadingSave = false ����Ա�����ֵ
    /// </summary>
    [Newtonsoft.Json.JsonIgnore]
    [System.ComponentModel.Browsable(false)]
    [System.ComponentModel.DataAnnotations.Display(AutoGenerateField = false)]
    public bool CascadingDelete
    {
      get { return MasterBusiness == null || (Criterions == null || Criterions.CascadingDelete); }
    }

    [Csla.NotUndoable]
    private Collection<IBusiness> _firstTransactionData;
    /// <summary>
    /// ����������ǰ�˵�ҵ�����
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
    /// ����������ĩ�˵�ҵ�����
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
    /// ��������������ύ������˵Ĵ���ʹ��
    /// ȱʡΪ false
    /// </summary>
    protected virtual bool EnsembleOnSaving
    {
      get { return false; }
    }
    bool IBusiness.EnsembleOnSaving
    {
      get { return EnsembleOnSaving; }
    }

    /// <summary>
    /// �ύ���ݺ���Ҫˢ�±����Լ�
    /// ȱʡΪ false
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

    /// <summary>
    /// �Ƿ�ҵ��������ʹ�ö�������
    /// ȱʡΪ false
    /// </summary>
    protected virtual bool AloneTransaction
    {
      get { return false; }
    }
    bool IBusinessCollection.AloneTransaction
    {
      get { return AloneTransaction; }
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
          DbTransaction result = MasterBusiness != null ? MasterBusiness.DbTransaction ?? _dbTransaction : _dbTransaction;
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

    #region �¼�

    #region ISelectedCollection ��Ա

    [NonSerialized]
    [Csla.NotUndoable]
    private List<EventHandler<SelectedValueChangingEventArgs>> _itemSelectedValueChangingHandlers;
    /// <summary>
    /// Selected���Ա�����ǰ
    /// </summary>
    public event EventHandler<SelectedValueChangingEventArgs> ItemSelectedValueChanging
    {
      add
      {
        if (_itemSelectedValueChangingHandlers == null)
          _itemSelectedValueChangingHandlers = new List<EventHandler<SelectedValueChangingEventArgs>>();
        _itemSelectedValueChangingHandlers.Add(value);
        foreach (TBusiness item in this)
          item.SelectedValueChanging += value;
      }
      remove
      {
        if (_itemSelectedValueChangingHandlers != null)
          _itemSelectedValueChangingHandlers.Remove(value);
        foreach (TBusiness item in this)
          item.SelectedValueChanging -= value;
      }
    }

    [NonSerialized]
    [Csla.NotUndoable]
    private List<EventHandler<SelectedValueChangedEventArgs>> _itemSelectedValueChangedHandlers;
    /// <summary>
    /// Selected���Ա����ĺ�
    /// </summary>
    public event EventHandler<SelectedValueChangedEventArgs> ItemSelectedValueChanged
    {
      add
      {
        if (_itemSelectedValueChangedHandlers == null)
          _itemSelectedValueChangedHandlers = new List<EventHandler<SelectedValueChangedEventArgs>>();
        _itemSelectedValueChangedHandlers.Add(value);
        foreach (TBusiness item in this)
          item.SelectedValueChanged += value;
      }
      remove
      {
        if (_itemSelectedValueChangedHandlers != null)
          _itemSelectedValueChangedHandlers.Remove(value);
        foreach (TBusiness item in this)
          item.SelectedValueChanged -= value;
      }
    }
        
    #endregion

    #endregion

    #region ����

    private T MemberwiseClone(IBusinessObject masterBusiness)
    {
      T result = (T)MemberwiseClone();
      result.__NewItems();
      if (_criterions != null)
        result._criterions = _criterions.MemberwiseClone(masterBusiness);
      return result;
    }

    #region ObjectCache

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
    private IDictionary<string, Type> GetCacheTypes()
    {
      return BusinessBase<TBusiness>.CacheTypes;
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

    #region ��ʼ��

    private void CascadingMarkNew(bool cascadingDelete)
    {
      foreach (IRefinedly item in this)
        item.CascadingMarkNew(cascadingDelete);
    }
    void IRefinedly.CascadingMarkNew(bool cascadingDelete)
    {
      CascadingMarkNew(cascadingDelete);
    }

    #endregion
    
    #region ��ɾ��

    private void AddDeleted(IRefinedlyObject item)
    {
      DeletedList.Add((TBusiness)item);
    }
    void IRefinedlyCollection.AddDeleted(IRefinedlyObject item)
    {
      AddDeleted(item);
    }

    private bool ClearRemove(IRefinedlyObject item)
    {
      TBusiness business = (TBusiness)item;
      bool isNew = business.IsNew;
      bool isSelfDirty = business.IsSelfDirty;
      Remove(business);
      bool result = DeletedList.Remove(business);
      if (isNew)
        business.MarkNewDirty();
      else if (isSelfDirty)
        business.MarkOldDirty();
      else
        business.MarkOldClean();
      return result;
    }
    bool IRefinedlyCollection.ClearRemove(IRefinedlyObject item)
    {
      return ClearRemove(item);
    }

    private TBusiness FixItem(TBusiness item)
    {
      if (item.Selected && !SelectedItems.Contains(item))
        SelectedItems.Add(item);
      if (!SelfFetching)
      {
        if (_itemSelectedValueChangingHandlers != null)
          foreach (EventHandler<SelectedValueChangingEventArgs> handler in _itemSelectedValueChangingHandlers)
            item.SelectedValueChanging += handler;
        if (_itemSelectedValueChangedHandlers != null)
          foreach (EventHandler<SelectedValueChangedEventArgs> handler in _itemSelectedValueChangedHandlers)
            item.SelectedValueChanged += handler;
      }
      item.MarkAsChild(this, !SelfFetching);
      return item;
    }

    /// <summary>
    /// ����
    /// </summary>
    /// <param name="index">����</param>
    public TBusiness AddNew(int index)
    {
      TBusiness result = BusinessBase<TBusiness>.New(this);
      InsertItem(index, result);
      return result;
    }
    /// <summary>
    /// ����
    /// </summary>
    /// <param name="index">����</param>
    /// <returns>����</returns>
    IBusinessObject IBusinessCollection.AddNew(int index)
    {
      return AddNew(index);
    }

    /// <summary>
    /// ����
    /// </summary>
    /// <param name="cloneSource">Clone����Դ</param>
    public TBusiness AddNew(TBusiness cloneSource)
    {
      return AddNew(Count, cloneSource);
    }

    /// <summary>
    /// ����
    /// </summary>
    /// <param name="index">����</param>
    /// <param name="cloneSource">Clone����Դ</param>
    public TBusiness AddNew(int index, TBusiness cloneSource)
    {
      if (cloneSource == null)
        return AddNew(index);
      TBusiness result = BusinessBase<TBusiness>.New(cloneSource);
      InsertItem(index, result);
      return result;
    }
    /// <summary>
    /// ����
    /// </summary>
    /// <param name="index">����</param>
    /// <param name="cloneSource">Clone����Դ</param>
    IBusinessObject IBusinessCollection.AddNew(int index, IBusinessObject cloneSource)
    {
      return AddNew(index, cloneSource as TBusiness);
    }

    /// <summary>
    /// ����
    /// </summary>
    /// <param name="source">����Դ</param>
    /// <param name="propertyInfos">��ƥ���������Ϣ, ��Ϊnull���ն���ʱƥ��ȫ������</param>
    public TBusiness AddNew(IBusinessObject source, params Phenix.Core.Mapping.IPropertyInfo[] propertyInfos)
    {
      return AddNew(Count, source, propertyInfos);
    }

    /// <summary>
    /// ����
    /// </summary>
    /// <param name="index">����</param>
    /// <param name="source">����Դ</param>
    /// <param name="propertyInfos">��ƥ���������Ϣ, ��Ϊnull���ն���ʱƥ��ȫ������</param>
    public TBusiness AddNew(int index, IBusinessObject source, params Phenix.Core.Mapping.IPropertyInfo[] propertyInfos)
    {
      if (source == null)
        return AddNew(index);
      TBusiness result = BusinessBase<TBusiness>.New(source, propertyInfos);
      InsertItem(index, result);
      return result;
    }

    /// <summary>
    /// ����������ĩβ
    /// </summary>
    protected override object AddNewCore()
    {
      return AddNew(Count);
    }

    /// <summary>
    /// �����
    /// </summary>
    protected override void InsertItem(int index, TBusiness item)
    {
      base.InsertItem(SelfFetching ? index : OrderByInfo.RectifyIndex(index, item, this, OrderByInfos), FixItem(item));
    }

    /// <summary>
    /// ʹ��ָ�����滻ָ������������
    /// </summary>
    protected override void SetItem(int index, TBusiness item)
    {
      base.SetItem(index, FixItem(item));
    }

    /// <summary>
    /// �Ƴ�ָ������������
    /// </summary>
    /// <param name="index">����</param>
    protected override void RemoveItem(int index)
    {
      if (_itemSelectedValueChangingHandlers != null)
      {
        TBusiness item = Items[index];
        foreach (EventHandler<SelectedValueChangingEventArgs> handler in _itemSelectedValueChangingHandlers)
          item.SelectedValueChanging -= handler;
      }
      if (_itemSelectedValueChangedHandlers != null)
      {
        TBusiness item = Items[index];
        foreach (EventHandler<SelectedValueChangedEventArgs> handler in _itemSelectedValueChangedHandlers)
          item.SelectedValueChanged -= handler;
      }
      base.RemoveItem(index);
    }

    /// <summary>
    /// ���������Ƴ�ҵ�����
    /// </summary>
    /// <param name="match">����Ҫ�Ƴ���Ԫ��Ӧ���������</param>
    public int Remove(Predicate<TBusiness> match)
    {
      int result = 0;
      for (int i = Count - 1; i >= 0; i--)
        if (match == null || match(Items[i]))
        {
          RemoveItem(i);
          result = result + 1;
        }
      return result;
    }

    /// <summary>
    /// ����ѭ�����ʵ�ö����
    /// </summary>
    public new IEnumerator<TBusiness> GetEnumerator()
    {
      LazyFetch(false);
      return base.GetEnumerator();
    }

    /// <summary>
    /// ȷ��ĳԪ���Ƿ����
    /// </summary>
    /// <param name="item">Ҫ��λ�Ķ���</param>
    public new bool Contains(TBusiness item)
    {
      LazyFetch(false);
      return base.Contains(item);
    }

    /// <summary>
    /// ����ָ���Ķ���
    /// </summary>
    /// <param name="item">Ҫ��λ�Ķ���</param>
    public new int IndexOf(TBusiness item)
    {
      LazyFetch(false);
      return base.IndexOf(item);
    }

    internal void LazyFetch(bool reset)
    {
      if (!InLazyFetch && !reset)
        return;
      InLazyFetch = false;
      __ClearItems();
      FillRange(DbTransaction != null
        ? DoFetch(DbTransaction, Criterions, false)
        : DoFetch(Criterions, false));
    }

    /// <summary>
    /// �������item������������
    /// </summary>
    /// <param name="source">����Դ</param>
    public override void FillRange(IEnumerable<TBusiness> source)
    {
      SelfFetching = true;
      try
      {
        base.FillRange(source);
      }
      finally
      {
        SelfFetching = false;
      }
    }

    #endregion

    #region Orderby

    /// <summary>
    /// ����(��¡)
    /// </summary>
    /// <param name="orderByInfo">��������˳��</param>
    public T OrderBy(OrderByInfo orderByInfo)
    {
      if (orderByInfo == null)
        return Filter(this, (Expression<Func<TBusiness, bool>>)null);
      return Filter(orderByInfo.Execute(this), null, MasterBusiness, GroupName, CascadingSave, CascadingDelete);
    }

    /// <summary>
    /// ����(��¡)
    /// </summary>
    /// <param name="orderByInfo">��������˳��</param>
    /// <param name="comparer">�ȽϷ���</param>
    public T OrderBy<TKey>(OrderByInfo orderByInfo, IComparer<TKey> comparer)
    {
      if (orderByInfo == null)
        return Filter(this, (Expression<Func<TBusiness, bool>>)null);
      return Filter(orderByInfo.Execute(this, comparer), null, MasterBusiness, GroupName, CascadingSave, CascadingDelete);
    }

    /// <summary>
    /// ����(��¡)
    /// </summary>
    /// <param name="source">����Դ</param>
    /// <param name="orderByInfo">��������˳��</param>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1000:DoNotDeclareStaticMembersOnGenericTypes")]
    public static T OrderBy(IEnumerable<TBusiness> source, OrderByInfo orderByInfo)
    {
      if (source == null)
        return null;
      if (orderByInfo == null)
        return Filter(source, (Expression<Func<TBusiness, bool>>)null);
      if (source is T)
      {
        Criterions criterions = ((T)source).Criterions;
        return Filter(orderByInfo.Execute(source), null, criterions.MasterBusiness, criterions.GroupName, criterions.CascadingSave, criterions.CascadingDelete);
      }
      return Filter(orderByInfo.Execute(source), (Expression<Func<TBusiness, bool>>)null);
    }

    /// <summary>
    /// ����(��¡)
    /// </summary>
    /// <param name="source">����Դ</param>
    /// <param name="orderByInfo">��������˳��</param>
    /// <param name="comparer">�ȽϷ���</param>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1000:DoNotDeclareStaticMembersOnGenericTypes")]
    public static T OrderBy<TKey>(IEnumerable<TBusiness> source, OrderByInfo orderByInfo, IComparer<TKey> comparer)
    {
      if (source == null)
        return null;
      if (orderByInfo == null)
        return Filter(source, (Expression<Func<TBusiness, bool>>)null);
      if (source is T)
      {
        Criterions criterions = ((T)source).Criterions;
        return Filter(orderByInfo.Execute(source, comparer), null, criterions.MasterBusiness, criterions.GroupName, criterions.CascadingSave, criterions.CascadingDelete);
      }
      return Filter(orderByInfo.Execute(source, comparer), (Expression<Func<TBusiness, bool>>)null);
    }

    #endregion

    #region Filter

    /// <summary>
    /// �Ӷ�����(��ɾ��)�����޵�����������ҵ�����
    /// </summary>
    /// <param name="expression">�������ʽ</param>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures")]
    public IList<TBusiness> FilterSelf(Expression<Func<TBusiness, bool>> expression)
    {
      if (expression == null)
        return null;
      bool oldRaiseListChangedEvents = RaiseListChangedEvents;
      try
      {
        RaiseListChangedEvents = false;
        IList<TBusiness> result = new List<TBusiness>(this.Where(expression.Compile()));
        foreach (TBusiness item in result)
          ClearRemove(item);
        return result;
      }
      finally
      {
        RaiseListChangedEvents = oldRaiseListChangedEvents;
        if (RaiseListChangedEvents)
          OnListChanged(new ListChangedEventArgs(ListChangedType.Reset, -1));
      }
    }

    /// <summary>
    /// ����(��¡)������������ҵ��������
    /// </summary>
    /// <param name="expression">�������ʽ</param>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures")]
    public T Filter(Expression<Func<TBusiness, bool>> expression)
    {
      return DoFilter(this, expression, MasterBusiness, GroupName, CascadingSave, CascadingDelete);
    }

    /// <summary>
    /// ����(��¡)������������ҵ��������
    /// </summary>
    /// <param name="source">����Դ</param>
    /// <param name="expression">�������ʽ</param>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1000:DoNotDeclareStaticMembersOnGenericTypes")]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures")]
    public static T Filter(IEnumerable<TBusiness> source, Expression<Func<TBusiness, bool>> expression)
    {
      return DoFilter(source, expression, null, null, true, true);
    }

    /// <summary>
    /// ����(��¡)����ҵ����������ֵ��ҵ���������ֵ��ȵ�ҵ����󼯺�
    /// cascadingSave = true
    /// cascadingDelete = true
    /// </summary>
    /// <param name="source">����Դ</param>
    /// <param name="masterBusiness">��ҵ�����</param>
    [Obsolete("����ʹ��: CompositionFilter(IEnumerable<TBusiness> source, IBusinessObject masterBusiness)", false)]
    public static T Filter(IEnumerable<TBusiness> source, IBusinessObject masterBusiness)
    {
      return CompositionFilter(source, masterBusiness);
    }

    /// <summary>
    /// ����(��¡)����ҵ����������ֵ��ҵ���������ֵ��ȵ�ҵ����󼯺�(��Ϲ�ϵ)
    /// cascadingSave = true
    /// cascadingDelete = true
    /// </summary>
    /// <param name="source">����Դ</param>
    /// <param name="masterBusiness">��ҵ�����</param>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1000:DoNotDeclareStaticMembersOnGenericTypes")]
    public static T CompositionFilter(IEnumerable<TBusiness> source, IBusinessObject masterBusiness)
    {
      return DoFilter(source, null, masterBusiness, null, true, true);
    }

    /// <summary>
    /// ����(��¡)����ҵ����������ֵ��ҵ���������ֵ��ȵ�ҵ����󼯺�(�ۺϹ�ϵ)
    /// cascadingSave = true
    /// cascadingDelete = false
    /// </summary>
    /// <param name="source">����Դ</param>
    /// <param name="masterBusiness">��ҵ�����</param>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1000:DoNotDeclareStaticMembersOnGenericTypes")]
    public static T AggregationFilter(IEnumerable<TBusiness> source, IBusinessObject masterBusiness)
    {
      return DoFilter(source, null, masterBusiness, null, true, false);
    }

    /// <summary>
    /// ����(��¡)����ҵ����������ֵ��ҵ���������ֵ��ȵ�ҵ����󼯺�
    /// </summary>
    /// <param name="source">����Դ</param>
    /// <param name="masterBusiness">��ҵ�����</param>
    /// <param name="cascadingSave">�Ƿ�������?</param>
    /// <param name="cascadingDelete">�Ƿ���ɾ��?</param>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1000:DoNotDeclareStaticMembersOnGenericTypes")]
    public static T Filter(IEnumerable<TBusiness> source, IBusinessObject masterBusiness, bool cascadingSave, bool cascadingDelete)
    {
      return DoFilter(source, null, masterBusiness, null, cascadingSave, cascadingDelete);
    }

    /// <summary>
    /// ����(��¡)����ҵ����������ֵ��ҵ���������ֵ��ȵ�ҵ����󼯺�(��Ϲ�ϵ)
    /// cascadingSave = true
    /// cascadingDelete = true
    /// </summary>
    /// <param name="source">����Դ</param>
    /// <param name="masterBusiness">��ҵ�����</param>
    /// <param name="groupName">������, null����ȫ��</param>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1000:DoNotDeclareStaticMembersOnGenericTypes")]
    public static T CompositionFilter(IEnumerable<TBusiness> source, IBusinessObject masterBusiness, string groupName)
    {
      return DoFilter(source, null, masterBusiness, groupName, true, true);
    }

    /// <summary>
    /// ����(��¡)����ҵ����������ֵ��ҵ���������ֵ��ȵ�ҵ����󼯺�(�ۺϹ�ϵ)
    /// cascadingSave = true
    /// cascadingDelete = false
    /// </summary>
    /// <param name="source">����Դ</param>
    /// <param name="masterBusiness">��ҵ�����</param>
    /// <param name="groupName">������, null����ȫ��</param>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1000:DoNotDeclareStaticMembersOnGenericTypes")]
    public static T AggregationFilter(IEnumerable<TBusiness> source, IBusinessObject masterBusiness, string groupName)
    {
      return DoFilter(source, null, masterBusiness, groupName, true, false);
    }

    /// <summary>
    /// ����(��¡)����ҵ����������ֵ��ҵ���������ֵ��ȵ�ҵ����󼯺�
    /// </summary>
    /// <param name="source">����Դ</param>
    /// <param name="masterBusiness">��ҵ�����</param>
    /// <param name="groupName">������, null����ȫ��</param>
    /// <param name="cascadingSave">�Ƿ�������?</param>
    /// <param name="cascadingDelete">�Ƿ���ɾ��?</param>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1000:DoNotDeclareStaticMembersOnGenericTypes")]
    public static T Filter(IEnumerable<TBusiness> source, IBusinessObject masterBusiness, string groupName, bool cascadingSave, bool cascadingDelete)
    {
      return DoFilter(source, null, masterBusiness, groupName, cascadingSave, cascadingDelete);
    }

    /// <summary>
    /// ����(��¡)�����������ġ���ҵ����������ֵ��ҵ���������ֵ��ȵ�ҵ����󼯺�(��Ϲ�ϵ)
    /// cascadingSave = true
    /// cascadingDelete = true
    /// </summary>
    /// <param name="source">����Դ</param>
    /// <param name="expression">�������ʽ</param>
    /// <param name="masterBusiness">��ҵ�����</param>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1000:DoNotDeclareStaticMembersOnGenericTypes")]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures")]
    public static T CompositionFilter(IEnumerable<TBusiness> source, Expression<Func<TBusiness, bool>> expression, IBusinessObject masterBusiness)
    {
      return DoFilter(source, expression, masterBusiness, null, true, true);
    }

    /// <summary>
    /// ����(��¡)�����������ġ���ҵ����������ֵ��ҵ���������ֵ��ȵ�ҵ����󼯺�(�ۺϹ�ϵ)
    /// cascadingSave = true
    /// cascadingDelete = false
    /// </summary>
    /// <param name="source">����Դ</param>
    /// <param name="expression">�������ʽ</param>
    /// <param name="masterBusiness">��ҵ�����</param>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1000:DoNotDeclareStaticMembersOnGenericTypes")]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures")]
    public static T AggregationFilter(IEnumerable<TBusiness> source, Expression<Func<TBusiness, bool>> expression, IBusinessObject masterBusiness)
    {
      return DoFilter(source, expression, masterBusiness, null, true, false);
    }

    /// <summary>
    /// ����(��¡)�����������ġ���ҵ����������ֵ��ҵ���������ֵ��ȵ�ҵ����󼯺�
    /// </summary>
    /// <param name="source">����Դ</param>
    /// <param name="expression">�������ʽ</param>
    /// <param name="masterBusiness">��ҵ�����</param>
    /// <param name="cascadingSave">�Ƿ�������?</param>
    /// <param name="cascadingDelete">�Ƿ���ɾ��?</param>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1000:DoNotDeclareStaticMembersOnGenericTypes")]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures")]
    public static T Filter(IEnumerable<TBusiness> source, Expression<Func<TBusiness, bool>> expression, IBusinessObject masterBusiness, bool cascadingSave, bool cascadingDelete)
    {
      return DoFilter(source, expression, masterBusiness, null, cascadingSave, cascadingDelete);
    }

    /// <summary>
    /// ����(��¡)�����������ġ���ҵ����������ֵ��ҵ���������ֵ��ȵ�ҵ����󼯺�(��Ϲ�ϵ)
    /// cascadingSave = true
    /// cascadingDelete = true
    /// </summary>
    /// <param name="source">����Դ</param>
    /// <param name="expression">�������ʽ</param>
    /// <param name="masterBusiness">��ҵ�����</param>
    /// <param name="groupName">������, null����ȫ��</param>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1000:DoNotDeclareStaticMembersOnGenericTypes")]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures")]
    public static T CompositionFilter(IEnumerable<TBusiness> source, Expression<Func<TBusiness, bool>> expression, IBusinessObject masterBusiness, string groupName)
    {
      return DoFilter(source, expression, masterBusiness, groupName, true, true);
    }

    /// <summary>
    /// ����(��¡)�����������ġ���ҵ����������ֵ��ҵ���������ֵ��ȵ�ҵ����󼯺�(�ۺϹ�ϵ)
    /// cascadingSave = true
    /// cascadingDelete = false
    /// </summary>
    /// <param name="source">����Դ</param>
    /// <param name="expression">�������ʽ</param>
    /// <param name="masterBusiness">��ҵ�����</param>
    /// <param name="groupName">������, null����ȫ��</param>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1000:DoNotDeclareStaticMembersOnGenericTypes")]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures")]
    public static T AggregationFilter(IEnumerable<TBusiness> source, Expression<Func<TBusiness, bool>> expression, IBusinessObject masterBusiness, string groupName)
    {
      return DoFilter(source, expression, masterBusiness, groupName, true, false);
    }

    /// <summary>
    /// ����(��¡)�����������ġ���ҵ����������ֵ��ҵ���������ֵ��ȵ�ҵ����󼯺�
    /// </summary>
    /// <param name="source">����Դ</param>
    /// <param name="expression">�������ʽ</param>
    /// <param name="masterBusiness">��ҵ�����</param>
    /// <param name="groupName">������, null����ȫ��</param>
    /// <param name="cascadingSave">�Ƿ�������?</param>
    /// <param name="cascadingDelete">�Ƿ���ɾ��?</param>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1000:DoNotDeclareStaticMembersOnGenericTypes")]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures")]
    public static T Filter(IEnumerable<TBusiness> source, Expression<Func<TBusiness, bool>> expression, IBusinessObject masterBusiness, string groupName, bool cascadingSave, bool cascadingDelete)
    {
      return DoFilter(source, expression, masterBusiness, groupName, cascadingSave, cascadingDelete);
    }

    /// <summary>
    /// ����(��¡)����ҵ����������ֵ��ҵ���������ֵ��ȵ�ҵ����󼯺�
    /// cascadingSave = true
    /// cascadingDelete = true
    /// </summary>
    /// <param name="masterBusiness">��ҵ�����</param>
    [Obsolete("����ʹ��: CompositionFilter(IBusinessObject masterBusiness)", false)]
    public T Filter(IBusinessObject masterBusiness)
    {
      return CompositionFilter(masterBusiness);
    }

    /// <summary>
    /// ����(��¡)����ҵ����������ֵ��ҵ���������ֵ��ȵ�ҵ����󼯺�(��Ϲ�ϵ)
    /// cascadingSave = true
    /// cascadingDelete = true
    /// </summary>
    /// <param name="masterBusiness">��ҵ�����</param>
    public T CompositionFilter(IBusinessObject masterBusiness)
    {
      return DoFilter(this, null, masterBusiness, null, true, true);
    }

    /// <summary>
    /// ����(��¡)����ҵ����������ֵ��ҵ���������ֵ��ȵ�ҵ����󼯺�(�ۺϹ�ϵ)
    /// cascadingSave = true
    /// cascadingDelete = false
    /// </summary>
    /// <param name="masterBusiness">��ҵ�����</param>
    public T AggregationFilter(IBusinessObject masterBusiness)
    {
      return DoFilter(this, null, masterBusiness, null, true, false);
    }

    /// <summary>
    /// ����(��¡)����ҵ����������ֵ��ҵ���������ֵ��ȵ�ҵ����󼯺�
    /// </summary>
    /// <param name="masterBusiness">��ҵ�����</param>
    /// <param name="cascadingSave">�Ƿ�������?</param>
    /// <param name="cascadingDelete">�Ƿ���ɾ��?</param>
    public T Filter(IBusinessObject masterBusiness, bool cascadingSave, bool cascadingDelete)
    {
      return DoFilter(this, null, masterBusiness, null, cascadingSave, cascadingDelete);
    }

    /// <summary>
    /// ����(��¡)����ҵ����������ֵ��ҵ���������ֵ��ȵ�ҵ����󼯺�(��Ϲ�ϵ)
    /// cascadingSave = true
    /// cascadingDelete = true
    /// </summary>
    /// <param name="masterBusiness">��ҵ�����</param>
    /// <param name="groupName">������, null����ȫ��</param>
    public T CompositionFilter(IBusinessObject masterBusiness, string groupName)
    {
      return DoFilter(this, null, masterBusiness, groupName, true, true);
    }

    /// <summary>
    /// ����(��¡)����ҵ����������ֵ��ҵ���������ֵ��ȵ�ҵ����󼯺�(�ۺϹ�ϵ)
    /// cascadingSave = true
    /// cascadingDelete = false
    /// </summary>
    /// <param name="masterBusiness">��ҵ�����</param>
    /// <param name="groupName">������, null����ȫ��</param>
    public T AggregationFilter(IBusinessObject masterBusiness, string groupName)
    {
      return DoFilter(this, null, masterBusiness, groupName, true, false);
    }

    /// <summary>
    /// ����(��¡)����ҵ����������ֵ��ҵ���������ֵ��ȵ�ҵ����󼯺�
    /// </summary>
    /// <param name="masterBusiness">��ҵ�����</param>
    /// <param name="groupName">������, null����ȫ��</param>
    /// <param name="cascadingSave">�Ƿ�������?</param>
    /// <param name="cascadingDelete">�Ƿ���ɾ��?</param>
    public T Filter(IBusinessObject masterBusiness, string groupName, bool cascadingSave, bool cascadingDelete)
    {
      return DoFilter(this, null, masterBusiness, groupName, cascadingSave, cascadingDelete);
    }

    /// <summary>
    /// ����(��¡)�����������ġ���ҵ����������ֵ��ҵ���������ֵ��ȵ�ҵ����󼯺�(��Ϲ�ϵ)
    /// cascadingSave = true
    /// cascadingDelete = true
    /// </summary>
    /// <param name="expression">�������ʽ</param>
    /// <param name="masterBusiness">��ҵ�����</param>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures")]
    public T CompositionFilter(Expression<Func<TBusiness, bool>> expression, IBusinessObject masterBusiness)
    {
      return DoFilter(this, expression, masterBusiness, null, true, true);
    }

    /// <summary>
    /// ����(��¡)�����������ġ���ҵ����������ֵ��ҵ���������ֵ��ȵ�ҵ����󼯺�(�ۺϹ�ϵ)
    /// cascadingSave = true
    /// cascadingDelete = false
    /// </summary>
    /// <param name="expression">�������ʽ</param>
    /// <param name="masterBusiness">��ҵ�����</param>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures")]
    public T AggregationFilter(Expression<Func<TBusiness, bool>> expression, IBusinessObject masterBusiness)
    {
      return DoFilter(this, expression, masterBusiness, null, true, false);
    }

    /// <summary>
    /// ����(��¡)�����������ġ���ҵ����������ֵ��ҵ���������ֵ��ȵ�ҵ����󼯺�
    /// </summary>
    /// <param name="expression">�������ʽ</param>
    /// <param name="masterBusiness">��ҵ�����</param>
    /// <param name="cascadingSave">�Ƿ�������?</param>
    /// <param name="cascadingDelete">�Ƿ���ɾ��?</param>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures")]
    public T Filter(Expression<Func<TBusiness, bool>> expression, IBusinessObject masterBusiness, bool cascadingSave, bool cascadingDelete)
    {
      return DoFilter(this, expression, masterBusiness, null, cascadingSave, cascadingDelete);
    }

    /// <summary>
    /// ����(��¡)�����������ġ���ҵ����������ֵ��ҵ���������ֵ��ȵ�ҵ����󼯺�(��Ϲ�ϵ)
    /// cascadingSave = true
    /// cascadingDelete = true
    /// </summary>
    /// <param name="expression">�������ʽ</param>
    /// <param name="masterBusiness">��ҵ�����</param>
    /// <param name="groupName">������, null����ȫ��</param>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures")]
    public T CompositionFilter(Expression<Func<TBusiness, bool>> expression, IBusinessObject masterBusiness, string groupName)
    {
      return DoFilter(this, expression, masterBusiness, groupName, true, true);
    }

    /// <summary>
    /// ����(��¡)�����������ġ���ҵ����������ֵ��ҵ���������ֵ��ȵ�ҵ����󼯺�(�ۺϹ�ϵ)
    /// cascadingSave = true
    /// cascadingDelete = false
    /// </summary>
    /// <param name="expression">�������ʽ</param>
    /// <param name="masterBusiness">��ҵ�����</param>
    /// <param name="groupName">������, null����ȫ��</param>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures")]
    public T AggregationFilter(Expression<Func<TBusiness, bool>> expression, IBusinessObject masterBusiness, string groupName)
    {
      return DoFilter(this, expression, masterBusiness, groupName, true, false);
    }

    /// <summary>
    /// ����(��¡)�����������ġ���ҵ����������ֵ��ҵ���������ֵ��ȵ�ҵ����󼯺�
    /// </summary>
    /// <param name="expression">�������ʽ</param>
    /// <param name="masterBusiness">��ҵ�����</param>
    /// <param name="groupName">������, null����ȫ��</param>
    /// <param name="cascadingSave">�Ƿ�������?</param>
    /// <param name="cascadingDelete">�Ƿ���ɾ��?</param>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures")]
    public T Filter(Expression<Func<TBusiness, bool>> expression, IBusinessObject masterBusiness, string groupName, bool cascadingSave, bool cascadingDelete)
    {
      return DoFilter(this, expression, masterBusiness, groupName, cascadingSave, cascadingDelete);
    }

    private static T DoFilter(IEnumerable<TBusiness> source, Expression<Func<TBusiness, bool>> expression, IBusinessObject masterBusiness, string groupName, bool cascadingSave, bool cascadingDelete)
    {
      if (source == null)
        return null;
      T result = null;
      if (masterBusiness == null && source is T)
      {
        masterBusiness = ((T)source).MasterBusiness;
        groupName = ((T)source).GroupName;
        cascadingSave = ((T)source).CascadingSave;
        cascadingDelete = ((T)source).CascadingDelete;
      }
      Criterions criterions = new Criterions(typeof(T), expression, masterBusiness, groupName, cascadingSave, cascadingDelete);
      if (masterBusiness != null)
        result = masterBusiness.FindDetail<T, TBusiness>(criterions);
      if (result == null)
      {
        result = DynamicCreateInstance();
        if (source is T)
          for (int i = 0; i < ((T)source).EditLevel; i++)
            result.BeginEdit();
        bool oldRaiseListChangedEvents = result.RaiseListChangedEvents;
        try
        {
          result.RaiseListChangedEvents = false;
          result.SelfFetching = true;
          if (masterBusiness == null)
            foreach (TBusiness item in expression != null ? source.Where(expression.Compile()) : source)
              result.Add(item.Clone(false));
          else
            foreach (TBusiness item in expression != null ? source.Where(expression.Compile()) : source)
              if (item.IsLink((object)masterBusiness, groupName))
                result.Add(item.Clone(false));
          result.SelfFetching = false;
        }
        finally
        {
          result.RaiseListChangedEvents = oldRaiseListChangedEvents;
        }
        result.Criterions = criterions;
        if (masterBusiness != null)
          masterBusiness.SetDetail<T, TBusiness>(result);
      }
      return result;
    }

    /// <summary>
    /// ����(��¡)����ҵ����������ֵ��ҵ���������ֵ��ȵ�ҵ����󼯺�
    /// cascadingSave = true
    /// cascadingDelete = true
    /// </summary>
    /// <param name="masterBusinesses">��ҵ����󼯺�</param>
    [Obsolete("����ʹ��: CompositionFilter(IBusinessCollection masterBusinesses)", false)]
    public IDictionary<IBusinessObject, T> Filter(IBusinessCollection masterBusinesses)
    {
      return CompositionFilter(masterBusinesses);
    }

    /// <summary>
    /// ����(��¡)����ҵ����������ֵ��ҵ���������ֵ��ȵ�ҵ����󼯺�(��Ϲ�ϵ)
    /// cascadingSave = true
    /// cascadingDelete = true
    /// </summary>
    /// <param name="masterBusinesses">��ҵ����󼯺�</param>
    public IDictionary<IBusinessObject, T> CompositionFilter(IBusinessCollection masterBusinesses)
    {
      return Filter(null, masterBusinesses, null, true, true);
    }

    /// <summary>
    /// ����(��¡)����ҵ����������ֵ��ҵ���������ֵ��ȵ�ҵ����󼯺�(�ۺϹ�ϵ)
    /// cascadingSave = true
    /// cascadingDelete = false
    /// </summary>
    /// <param name="masterBusinesses">��ҵ����󼯺�</param>
    public IDictionary<IBusinessObject, T> AggregationFilter(IBusinessCollection masterBusinesses)
    {
      return Filter(null, masterBusinesses, null, true, false);
    }

    /// <summary>
    /// ����(��¡)����ҵ����������ֵ��ҵ���������ֵ��ȵ�ҵ����󼯺�
    /// </summary>
    /// <param name="masterBusinesses">��ҵ����󼯺�</param>
    /// <param name="cascadingSave">�Ƿ�������?</param>
    /// <param name="cascadingDelete">�Ƿ���ɾ��?</param>
    public IDictionary<IBusinessObject, T> Filter(IBusinessCollection masterBusinesses, bool cascadingSave, bool cascadingDelete)
    {
      return Filter(null, masterBusinesses, null, cascadingSave, cascadingDelete);
    }

    /// <summary>
    /// ����(��¡)����ҵ����������ֵ��ҵ���������ֵ��ȵ�ҵ����󼯺�(��Ϲ�ϵ)
    /// cascadingSave = true
    /// cascadingDelete = true
    /// </summary>
    /// <param name="masterBusinesses">��ҵ����󼯺�</param>
    /// <param name="groupName">������, null����ȫ��</param>
    public IDictionary<IBusinessObject, T> CompositionFilter(IBusinessCollection masterBusinesses, string groupName)
    {
      return Filter(null, masterBusinesses, groupName, true, true);
    }

    /// <summary>
    /// ����(��¡)����ҵ����������ֵ��ҵ���������ֵ��ȵ�ҵ����󼯺�(�ۺϹ�ϵ)
    /// cascadingSave = true
    /// cascadingDelete = false
    /// </summary>
    /// <param name="masterBusinesses">��ҵ����󼯺�</param>
    /// <param name="groupName">������, null����ȫ��</param>
    public IDictionary<IBusinessObject, T> AggregationFilter(IBusinessCollection masterBusinesses, string groupName)
    {
      return Filter(null, masterBusinesses, groupName, true, false);
    }

    /// <summary>
    /// ����(��¡)����ҵ����������ֵ��ҵ���������ֵ��ȵ�ҵ����󼯺�
    /// </summary>
    /// <param name="masterBusinesses">��ҵ����󼯺�</param>
    /// <param name="groupName">������, null����ȫ��</param>
    /// <param name="cascadingSave">�Ƿ�������?</param>
    /// <param name="cascadingDelete">�Ƿ���ɾ��?</param>
    public IDictionary<IBusinessObject, T> Filter(IBusinessCollection masterBusinesses, string groupName, bool cascadingSave, bool cascadingDelete)
    {
      return Filter(null, masterBusinesses, groupName, cascadingSave, cascadingDelete);
    }

    /// <summary>
    /// ����(��¡)�����������ġ���ҵ����������ֵ��ҵ���������ֵ��ȵ�ҵ����󼯺�(��Ϲ�ϵ)
    /// cascadingSave = true
    /// cascadingDelete = true
    /// </summary>
    /// <param name="expression">�������ʽ</param>
    /// <param name="masterBusinesses">��ҵ����󼯺�</param>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures")]
    public IDictionary<IBusinessObject, T> CompositionFilter(Expression<Func<TBusiness, bool>> expression, IBusinessCollection masterBusinesses)
    {
      return Filter(expression, masterBusinesses, null, true, true);
    }

    /// <summary>
    /// ����(��¡)�����������ġ���ҵ����������ֵ��ҵ���������ֵ��ȵ�ҵ����󼯺�(�ۺϹ�ϵ)
    /// cascadingSave = true
    /// cascadingDelete = false
    /// </summary>
    /// <param name="expression">�������ʽ</param>
    /// <param name="masterBusinesses">��ҵ����󼯺�</param>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures")]
    public IDictionary<IBusinessObject, T> AggregationFilter(Expression<Func<TBusiness, bool>> expression, IBusinessCollection masterBusinesses)
    {
      return Filter(expression, masterBusinesses, null, true, false);
    }

    /// <summary>
    /// ����(��¡)�����������ġ���ҵ����������ֵ��ҵ���������ֵ��ȵ�ҵ����󼯺�
    /// </summary>
    /// <param name="expression">�������ʽ</param>
    /// <param name="masterBusinesses">��ҵ����󼯺�</param>
    /// <param name="cascadingSave">�Ƿ�������?</param>
    /// <param name="cascadingDelete">�Ƿ���ɾ��?</param>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures")]
    public IDictionary<IBusinessObject, T> Filter(Expression<Func<TBusiness, bool>> expression, IBusinessCollection masterBusinesses, bool cascadingSave, bool cascadingDelete)
    {
      return Filter(expression, masterBusinesses, null, cascadingSave, cascadingDelete);
    }

    /// <summary>
    /// ����(��¡)�����������ġ���ҵ����������ֵ��ҵ���������ֵ��ȵ�ҵ����󼯺�(��Ϲ�ϵ)
    /// cascadingSave = true
    /// cascadingDelete = true
    /// </summary>
    /// <param name="expression">�������ʽ</param>
    /// <param name="masterBusinesses">��ҵ����󼯺�</param>
    /// <param name="groupName">������, null����ȫ��</param>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures")]
    public IDictionary<IBusinessObject, T> CompositionFilter(Expression<Func<TBusiness, bool>> expression, IBusinessCollection masterBusinesses, string groupName)
    {
      return Filter(expression, masterBusinesses, groupName, true, true);
    }

    /// <summary>
    /// ����(��¡)�����������ġ���ҵ����������ֵ��ҵ���������ֵ��ȵ�ҵ����󼯺�(�ۺϹ�ϵ)
    /// cascadingSave = true
    /// cascadingDelete = false
    /// </summary>
    /// <param name="expression">�������ʽ</param>
    /// <param name="masterBusinesses">��ҵ����󼯺�</param>
    /// <param name="groupName">������, null����ȫ��</param>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures")]
    public IDictionary<IBusinessObject, T> AggregationFilter(Expression<Func<TBusiness, bool>> expression, IBusinessCollection masterBusinesses, string groupName)
    {
      return Filter(expression, masterBusinesses, groupName, true, false);
    }

    /// <summary>
    /// ����(��¡)�����������ġ���ҵ����������ֵ��ҵ���������ֵ��ȵ�ҵ����󼯺�
    /// </summary>
    /// <param name="expression">�������ʽ</param>
    /// <param name="masterBusinesses">��ҵ����󼯺�</param>
    /// <param name="groupName">������, null����ȫ��</param>
    /// <param name="cascadingSave">�Ƿ�������?</param>
    /// <param name="cascadingDelete">�Ƿ���ɾ��?</param>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures")]
    public IDictionary<IBusinessObject, T> Filter(Expression<Func<TBusiness, bool>> expression, IBusinessCollection masterBusinesses, string groupName, bool cascadingSave, bool cascadingDelete)
    {
      if (masterBusinesses == null)
        throw new ArgumentNullException("masterBusinesses");

      Dictionary<IBusinessObject, T> result = new Dictionary<IBusinessObject, T>();
      foreach (TBusiness selfBusiness in expression != null ? this.Where(expression.Compile()) : this)
        foreach (IBusinessObject masterBusiness in masterBusinesses)
          if (selfBusiness.IsLink((object)masterBusiness, groupName))
          {
            T detail;
            if (!result.TryGetValue(masterBusiness, out detail))
            {
              detail = DynamicCreateInstance();
              for (int i = 0; i < EditLevel; i++)
                detail.BeginEdit();
              detail.SelfFetching = true;
            }
            detail.Add(selfBusiness.Clone(false));
            result[masterBusiness] = detail;
            break;
          }
      foreach (KeyValuePair<IBusinessObject, T> kvp in result)
      {
        kvp.Value.SelfFetching = false;
        kvp.Value.Criterions = new Criterions(typeof(T), expression, kvp.Key, groupName, cascadingSave, cascadingDelete);
        kvp.Key.SetDetail<T, TBusiness>(kvp.Value);
      }
      return result;
    }
    
    #endregion

    #region Detail

    /// <summary>
    /// �����ҵ�����Cache
    /// </summary>
    protected bool ClearDetailCache()
    {
      bool result = false;
      foreach (TBusiness item in this)
        if (item.ClearDetailCache())
          result = true;
      return result;
    }

    /// <summary>
    /// �����ҵ�����Cache
    /// </summary>
    protected bool ClearDetailCache(string key)
    {
      bool result = false;
      foreach (TBusiness item in this)
        if (item.ClearDetailCache(key))
          result = true;
      return result;
    }

    /// <summary>
    /// �����ҵ�����Cache
    /// </summary>
    protected bool ClearDetailCache(Type type)
    {
      bool result = false;
      foreach (TBusiness item in this)
        if (item.ClearDetailCache(type))
          result = true;
      return result;
    }

    /// <summary>
    /// �����ҵ�����Cache
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
    protected bool ClearDetailCache<TDetailBusiness>()
      where TDetailBusiness : BusinessBase<TDetailBusiness>
    {
      return ClearDetailCache(typeof(TDetailBusiness));
    }

    /// <summary>
    /// �����ҵ�����Cache
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
    protected bool ClearDetailCache<TDetail, TDetailBusiness>()
      where TDetail : BusinessListBase<TDetail, TDetailBusiness>
      where TDetailBusiness : BusinessBase<TDetailBusiness>
    {
      return ClearDetailCache(typeof(TDetail));
    }

    /// <summary>
    /// ��������ҵ��������
    /// </summary>
    public IList<TDetailBusiness> FindDetailBusinesses<TDetailBusiness>()
      where TDetailBusiness : BusinessBase<TDetailBusiness>
    {
      List<TDetailBusiness> result = new List<TDetailBusiness>();
      foreach (TBusiness item in this)
        result.AddRange(item.FindDetailBusinesses<TDetailBusiness>());
      return result;
    }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
    private TDetail DoFetchDetail<TDetail, TDetailBusiness>(Criterions criterions, bool lazyFetch)
      where TDetail : BusinessListBase<TDetail, TDetailBusiness>
      where TDetailBusiness : BusinessBase<TDetailBusiness>
    {
      if (criterions == null)
        criterions = new Criterions(typeof(TDetail), false, false);
      CriteriaExpression criteriaExpression = Exists(Criterions).Where(BusinessBase<TDetailBusiness>.Link<TBusiness>());
      criterions.CriteriaExpression = criterions.CriteriaExpression != null ? criterions.CriteriaExpression & criteriaExpression : criteriaExpression;
      return DbTransaction != null 
        ? BusinessListBase<TDetail, TDetailBusiness>.Fetch(DbTransaction, criterions, lazyFetch) 
        : BusinessListBase<TDetail, TDetailBusiness>.Fetch(criterions, lazyFetch);
    }

    /// <summary>
    /// ȡ��ҵ����󼯺�
    /// ��������ֶ�ӳ���ϵ����Phenix.Core.Mapping.CriteriaFieldAttribute��ע
    /// </summary>
    /// <param name="criterions">������</param>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
    public TDetail FetchDetail<TDetail, TDetailBusiness>(Criterions criterions)
      where TDetail : BusinessListBase<TDetail, TDetailBusiness>
      where TDetailBusiness : BusinessBase<TDetailBusiness>
    {
      return DoFetchDetail<TDetail, TDetailBusiness>(criterions, false);
    }

    /// <summary>
    /// ȡ��ҵ����󼯺�
    /// ��������ֶ�ӳ���ϵ����Phenix.Core.Mapping.CriteriaFieldAttribute��ע
    /// </summary>
    /// <param name="criterions">������</param>
    /// <param name="lazyFetch">�Ƿ����Fetch(</param>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
    public TDetail FetchDetail<TDetail, TDetailBusiness>(Criterions criterions, bool lazyFetch)
      where TDetail : BusinessListBase<TDetail, TDetailBusiness>
      where TDetailBusiness : BusinessBase<TDetailBusiness>
    {
      return DoFetchDetail<TDetail, TDetailBusiness>(criterions, lazyFetch);
    }

    /// <summary>
    /// ȡ��ҵ����󼯺�
    /// </summary>
    /// <param name="orderByInfos">��������˳�����</param>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
    public TDetail FetchDetail<TDetail, TDetailBusiness>(params OrderByInfo[] orderByInfos)
      where TDetail : BusinessListBase<TDetail, TDetailBusiness>
      where TDetailBusiness : BusinessBase<TDetailBusiness>
    {
      return DoFetchDetail<TDetail, TDetailBusiness>(new Criterions(typeof(TDetail), orderByInfos), false);
    }

    /// <summary>
    /// ȡ��ҵ����󼯺�
    /// </summary>
    /// <param name="lazyFetch">�Ƿ����Fetch(</param>
    /// <param name="orderByInfos">��������˳�����</param>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
    public TDetail FetchDetail<TDetail, TDetailBusiness>(bool lazyFetch, params OrderByInfo[] orderByInfos)
      where TDetail : BusinessListBase<TDetail, TDetailBusiness>
      where TDetailBusiness : BusinessBase<TDetailBusiness>
    {
      return DoFetchDetail<TDetail, TDetailBusiness>(new Criterions(typeof(TDetail), orderByInfos), lazyFetch);
    }

    /// <summary>
    /// ȡ��ҵ����󼯺�
    /// </summary>
    /// <param name="groupName">������</param>
    /// <param name="orderByInfos">��������˳�����</param>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
    public TDetail FetchDetail<TDetail, TDetailBusiness>(string groupName, params OrderByInfo[] orderByInfos)
      where TDetail : BusinessListBase<TDetail, TDetailBusiness>
      where TDetailBusiness : BusinessBase<TDetailBusiness>
    {
      return DoFetchDetail<TDetail, TDetailBusiness>(new Criterions(typeof(TDetail), null, groupName, orderByInfos), false);
    }

    /// <summary>
    /// ȡ��ҵ����󼯺�
    /// </summary>
    /// <param name="groupName">������</param>
    /// <param name="lazyFetch">�Ƿ����Fetch(</param>
    /// <param name="orderByInfos">��������˳�����</param>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
    public TDetail FetchDetail<TDetail, TDetailBusiness>(string groupName, bool lazyFetch, params OrderByInfo[] orderByInfos)
      where TDetail : BusinessListBase<TDetail, TDetailBusiness>
      where TDetailBusiness : BusinessBase<TDetailBusiness>
    {
      return DoFetchDetail<TDetail, TDetailBusiness>(new Criterions(typeof(TDetail), null, groupName, orderByInfos), lazyFetch);
    }

    /// <summary>
    /// ȡ��ҵ����󼯺�
    /// ��������ֶ�ӳ���ϵ����Phenix.Core.Mapping.CriteriaFieldAttribute��ע
    /// </summary>
    /// <param name="criteria">��ҵ����������</param>
    /// <param name="orderByInfos">��������˳�����</param>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
    public TDetail FetchDetail<TDetail, TDetailBusiness>(ICriteria criteria, params OrderByInfo[] orderByInfos)
      where TDetail : BusinessListBase<TDetail, TDetailBusiness>
      where TDetailBusiness : BusinessBase<TDetailBusiness>
    {
      return DoFetchDetail<TDetail, TDetailBusiness>(new Criterions(typeof(TDetail), criteria, orderByInfos), false);
    }

    /// <summary>
    /// ȡ��ҵ����󼯺�
    /// ��������ֶ�ӳ���ϵ����Phenix.Core.Mapping.CriteriaFieldAttribute��ע
    /// </summary>
    /// <param name="criteria">��ҵ����������</param>
    /// <param name="groupName">������</param>
    /// <param name="orderByInfos">��������˳�����</param>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
    public TDetail FetchDetail<TDetail, TDetailBusiness>(ICriteria criteria, string groupName, params OrderByInfo[] orderByInfos)
      where TDetail : BusinessListBase<TDetail, TDetailBusiness>
      where TDetailBusiness : BusinessBase<TDetailBusiness>
    {
      return DoFetchDetail<TDetail, TDetailBusiness>(new Criterions(typeof(TDetail), criteria, null, groupName, orderByInfos), false);
    }

    /// <summary>
    /// ȡ��ҵ����󼯺�
    /// ��������ֶ�ӳ���ϵ����Phenix.Core.Mapping.CriteriaFieldAttribute��ע
    /// </summary>
    /// <param name="criteria">��ҵ����������</param>
    /// <param name="groupName">������</param>
    /// <param name="lazyFetch">�Ƿ����Fetch(</param>
    /// <param name="orderByInfos">��������˳�����</param>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
    public TDetail FetchDetail<TDetail, TDetailBusiness>(ICriteria criteria, string groupName, bool lazyFetch, params OrderByInfo[] orderByInfos)
      where TDetail : BusinessListBase<TDetail, TDetailBusiness>
      where TDetailBusiness : BusinessBase<TDetailBusiness>
    {
      return DoFetchDetail<TDetail, TDetailBusiness>(new Criterions(typeof(TDetail), criteria, null, groupName, orderByInfos), lazyFetch);
    }

    /// <summary>
    /// ȡ��ҵ����󼯺�
    /// </summary>
    /// <param name="criteriaExpression">��ҵ���������ʽ</param>
    /// <param name="orderByInfos">��������˳�����</param>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
    public TDetail FetchDetail<TDetail, TDetailBusiness>(CriteriaExpression criteriaExpression, params OrderByInfo[] orderByInfos)
      where TDetail : BusinessListBase<TDetail, TDetailBusiness>
      where TDetailBusiness : BusinessBase<TDetailBusiness>
    {
      return DoFetchDetail<TDetail, TDetailBusiness>(new Criterions(typeof(TDetail), criteriaExpression, orderByInfos), false);
    }

    /// <summary>
    /// ȡ��ҵ����󼯺�
    /// </summary>
    /// <param name="criteriaExpression">��ҵ���������ʽ</param>
    /// <param name="groupName">������</param>
    /// <param name="orderByInfos">��������˳�����</param>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
    public TDetail FetchDetail<TDetail, TDetailBusiness>(CriteriaExpression criteriaExpression, string groupName, params OrderByInfo[] orderByInfos)
      where TDetail : BusinessListBase<TDetail, TDetailBusiness>
      where TDetailBusiness : BusinessBase<TDetailBusiness>
    {
      return DoFetchDetail<TDetail, TDetailBusiness>(new Criterions(typeof(TDetail), criteriaExpression, null, groupName, orderByInfos), false);
    }

    /// <summary>
    /// ȡ��ҵ����󼯺�
    /// </summary>
    /// <param name="criteriaExpression">��ҵ���������ʽ</param>
    /// <param name="groupName">������</param>
    /// <param name="lazyFetch">�Ƿ����Fetch(</param>
    /// <param name="orderByInfos">��������˳�����</param>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
    public TDetail FetchDetail<TDetail, TDetailBusiness>(CriteriaExpression criteriaExpression, string groupName, bool lazyFetch, params OrderByInfo[] orderByInfos)
      where TDetail : BusinessListBase<TDetail, TDetailBusiness>
      where TDetailBusiness : BusinessBase<TDetailBusiness>
    {
      return DoFetchDetail<TDetail, TDetailBusiness>(new Criterions(typeof(TDetail), criteriaExpression, null, groupName, orderByInfos), lazyFetch);
    }

    /// <summary>
    /// ȡ��ҵ����󼯺�
    /// </summary>
    /// <param name="criteriaExpression">��ҵ���������ʽ</param>
    /// <param name="orderByInfos">��������˳�����</param>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures")]
    public TDetail FetchDetail<TDetail, TDetailBusiness>(Expression<Func<TDetailBusiness, bool>> criteriaExpression, params OrderByInfo[] orderByInfos)
      where TDetail : BusinessListBase<TDetail, TDetailBusiness>
      where TDetailBusiness : BusinessBase<TDetailBusiness>
    {
      return FetchDetail<TDetail, TDetailBusiness>(CriteriaHelper.ToCriteriaExpression(criteriaExpression), orderByInfos);
    }

    /// <summary>
    /// ȡ��ҵ����󼯺�
    /// </summary>
    /// <param name="criteriaExpression">��ҵ���������ʽ</param>
    /// <param name="groupName">������</param>
    /// <param name="orderByInfos">��������˳�����</param>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures")]
    public TDetail FetchDetail<TDetail, TDetailBusiness>(Expression<Func<TDetailBusiness, bool>> criteriaExpression, string groupName, params OrderByInfo[] orderByInfos)
      where TDetail : BusinessListBase<TDetail, TDetailBusiness>
      where TDetailBusiness : BusinessBase<TDetailBusiness>
    {
      return FetchDetail<TDetail, TDetailBusiness>(CriteriaHelper.ToCriteriaExpression(criteriaExpression), groupName, orderByInfos);
    }

    /// <summary>
    /// ȡ��ҵ����󼯺�
    /// </summary>
    /// <param name="criteriaExpression">��ҵ���������ʽ</param>
    /// <param name="groupName">������</param>
    /// <param name="lazyFetch">�Ƿ����Fetch(</param>
    /// <param name="orderByInfos">��������˳�����</param>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures")]
    public TDetail FetchDetail<TDetail, TDetailBusiness>(Expression<Func<TDetailBusiness, bool>> criteriaExpression, string groupName, bool lazyFetch, params OrderByInfo[] orderByInfos)
      where TDetail : BusinessListBase<TDetail, TDetailBusiness>
      where TDetailBusiness : BusinessBase<TDetailBusiness>
    {
      return FetchDetail<TDetail, TDetailBusiness>(CriteriaHelper.ToCriteriaExpression(criteriaExpression), groupName, lazyFetch, orderByInfos);
    }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
    private TDetail DoFetchDetail<TDetail, TDetailBusiness>(DbConnection connection, Criterions criterions, bool lazyFetch)
      where TDetail : BusinessListBase<TDetail, TDetailBusiness>
      where TDetailBusiness : BusinessBase<TDetailBusiness>
    {
      if (criterions == null)
        criterions = new Criterions(typeof(TDetail), false, false);
      CriteriaExpression criteriaExpression = Exists(Criterions).Where(BusinessBase<TDetailBusiness>.Link<TBusiness>());
      criterions.CriteriaExpression = criterions.CriteriaExpression != null ? criterions.CriteriaExpression & criteriaExpression : criteriaExpression;
      return BusinessListBase<TDetail, TDetailBusiness>.Fetch(connection, criterions, lazyFetch);
    }

    /// <summary>
    /// ȡ��ҵ����󼯺�
    /// ��������ֶ�ӳ���ϵ����Phenix.Core.Mapping.CriteriaFieldAttribute��ע
    /// </summary>
    /// <param name="connection">���ݿ�����</param>
    /// <param name="criterions">������</param>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
    public TDetail FetchDetail<TDetail, TDetailBusiness>(DbConnection connection, Criterions criterions)
      where TDetail : BusinessListBase<TDetail, TDetailBusiness>
      where TDetailBusiness : BusinessBase<TDetailBusiness>
    {
      return DoFetchDetail<TDetail, TDetailBusiness>(connection, criterions, false);
    }

    /// <summary>
    /// ȡ��ҵ����󼯺�
    /// ��������ֶ�ӳ���ϵ����Phenix.Core.Mapping.CriteriaFieldAttribute��ע
    /// </summary>
    /// <param name="connection">���ݿ�����</param>
    /// <param name="criterions">������</param>
    /// <param name="lazyFetch">�Ƿ����Fetch(</param>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
    public TDetail FetchDetail<TDetail, TDetailBusiness>(DbConnection connection, Criterions criterions, bool lazyFetch)
      where TDetail : BusinessListBase<TDetail, TDetailBusiness>
      where TDetailBusiness : BusinessBase<TDetailBusiness>
    {
      return DoFetchDetail<TDetail, TDetailBusiness>(connection, criterions, lazyFetch);
    }

    /// <summary>
    /// ȡ��ҵ����󼯺�
    /// </summary>
    /// <param name="connection">���ݿ�����</param>
    /// <param name="orderByInfos">��������˳�����</param>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
    public TDetail FetchDetail<TDetail, TDetailBusiness>(DbConnection connection, params OrderByInfo[] orderByInfos)
      where TDetail : BusinessListBase<TDetail, TDetailBusiness>
      where TDetailBusiness : BusinessBase<TDetailBusiness>
    {
      return DoFetchDetail<TDetail, TDetailBusiness>(connection, new Criterions(typeof(TDetail), orderByInfos), false);
    }

    /// <summary>
    /// ȡ��ҵ����󼯺�
    /// </summary>
    /// <param name="connection">���ݿ�����</param>
    /// <param name="groupName">������</param>
    /// <param name="orderByInfos">��������˳�����</param>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
    public TDetail FetchDetail<TDetail, TDetailBusiness>(DbConnection connection, string groupName, params OrderByInfo[] orderByInfos)
      where TDetail : BusinessListBase<TDetail, TDetailBusiness>
      where TDetailBusiness : BusinessBase<TDetailBusiness>
    {
      return DoFetchDetail<TDetail, TDetailBusiness>(connection, new Criterions(typeof(TDetail), null, groupName, orderByInfos), false);
    }

    /// <summary>
    /// ȡ��ҵ����󼯺�
    /// ��������ֶ�ӳ���ϵ����Phenix.Core.Mapping.CriteriaFieldAttribute��ע
    /// </summary>
    /// <param name="connection">���ݿ�����</param>
    /// <param name="criteria">��ҵ����������</param>
    /// <param name="orderByInfos">��������˳�����</param>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
    public TDetail FetchDetail<TDetail, TDetailBusiness>(DbConnection connection, ICriteria criteria, params OrderByInfo[] orderByInfos)
      where TDetail : BusinessListBase<TDetail, TDetailBusiness>
      where TDetailBusiness : BusinessBase<TDetailBusiness>
    {
      return DoFetchDetail<TDetail, TDetailBusiness>(connection, new Criterions(typeof(TDetail), criteria, orderByInfos), false);
    }

    /// <summary>
    /// ȡ��ҵ����󼯺�
    /// ��������ֶ�ӳ���ϵ����Phenix.Core.Mapping.CriteriaFieldAttribute��ע
    /// </summary>
    /// <param name="connection">���ݿ�����</param>
    /// <param name="criteria">��ҵ����������</param>
    /// <param name="groupName">������</param>
    /// <param name="orderByInfos">��������˳�����</param>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
    public TDetail FetchDetail<TDetail, TDetailBusiness>(DbConnection connection, ICriteria criteria, string groupName, params OrderByInfo[] orderByInfos)
      where TDetail : BusinessListBase<TDetail, TDetailBusiness>
      where TDetailBusiness : BusinessBase<TDetailBusiness>
    {
      return DoFetchDetail<TDetail, TDetailBusiness>(connection, new Criterions(typeof(TDetail), criteria, null, groupName, orderByInfos), false);
    }

    /// <summary>
    /// ȡ��ҵ����󼯺�
    /// </summary>
    /// <param name="connection">���ݿ�����</param>
    /// <param name="criteriaExpression">��ҵ���������ʽ</param>
    /// <param name="orderByInfos">��������˳�����</param>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
    public TDetail FetchDetail<TDetail, TDetailBusiness>(DbConnection connection, CriteriaExpression criteriaExpression, params OrderByInfo[] orderByInfos)
      where TDetail : BusinessListBase<TDetail, TDetailBusiness>
      where TDetailBusiness : BusinessBase<TDetailBusiness>
    {
      return DoFetchDetail<TDetail, TDetailBusiness>(connection, new Criterions(typeof(TDetail), criteriaExpression, orderByInfos), false);
    }

    /// <summary>
    /// ȡ��ҵ����󼯺�
    /// </summary>
    /// <param name="connection">���ݿ�����</param>
    /// <param name="criteriaExpression">��ҵ���������ʽ</param>
    /// <param name="groupName">������</param>
    /// <param name="orderByInfos">��������˳�����</param>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
    public TDetail FetchDetail<TDetail, TDetailBusiness>(DbConnection connection, CriteriaExpression criteriaExpression, string groupName, params OrderByInfo[] orderByInfos)
      where TDetail : BusinessListBase<TDetail, TDetailBusiness>
      where TDetailBusiness : BusinessBase<TDetailBusiness>
    {
      return DoFetchDetail<TDetail, TDetailBusiness>(connection, new Criterions(typeof(TDetail), criteriaExpression, null, groupName, orderByInfos), false);
    }

    /// <summary>
    /// ȡ��ҵ����󼯺�
    /// </summary>
    /// <param name="connection">���ݿ�����</param>
    /// <param name="criteriaExpression">��ҵ���������ʽ</param>
    /// <param name="orderByInfos">��������˳�����</param>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures")]
    public TDetail FetchDetail<TDetail, TDetailBusiness>(DbConnection connection, Expression<Func<TDetailBusiness, bool>> criteriaExpression, params OrderByInfo[] orderByInfos)
      where TDetail : BusinessListBase<TDetail, TDetailBusiness>
      where TDetailBusiness : BusinessBase<TDetailBusiness>
    {
      return FetchDetail<TDetail, TDetailBusiness>(connection, CriteriaHelper.ToCriteriaExpression(criteriaExpression), orderByInfos);
    }

    /// <summary>
    /// ȡ��ҵ����󼯺�
    /// </summary>
    /// <param name="connection">���ݿ�����</param>
    /// <param name="criteriaExpression">��ҵ���������ʽ</param>
    /// <param name="groupName">������</param>
    /// <param name="orderByInfos">��������˳�����</param>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures")]
    public TDetail FetchDetail<TDetail, TDetailBusiness>(DbConnection connection, Expression<Func<TDetailBusiness, bool>> criteriaExpression, string groupName, params OrderByInfo[] orderByInfos)
      where TDetail : BusinessListBase<TDetail, TDetailBusiness>
      where TDetailBusiness : BusinessBase<TDetailBusiness>
    {
      return FetchDetail<TDetail, TDetailBusiness>(connection, CriteriaHelper.ToCriteriaExpression(criteriaExpression), groupName, orderByInfos);
    }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
    private TDetail DoFetchDetail<TDetail, TDetailBusiness>(DbTransaction transaction, Criterions criterions, bool lazyFetch)
      where TDetail : BusinessListBase<TDetail, TDetailBusiness>
      where TDetailBusiness : BusinessBase<TDetailBusiness>
    {
      if (criterions == null)
        criterions = new Criterions(typeof(TDetail), false, false);
      CriteriaExpression criteriaExpression = Exists(Criterions).Where(BusinessBase<TDetailBusiness>.Link<TBusiness>());
      criterions.CriteriaExpression = criterions.CriteriaExpression != null ? criterions.CriteriaExpression & criteriaExpression : criteriaExpression;
      return BusinessListBase<TDetail, TDetailBusiness>.Fetch(transaction, criterions, lazyFetch);
    }

    /// <summary>
    /// ȡ��ҵ����󼯺�
    /// ��������ֶ�ӳ���ϵ����Phenix.Core.Mapping.CriteriaFieldAttribute��ע
    /// </summary>
    /// <param name="transaction">���ݿ�����</param>
    /// <param name="criterions">������</param>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
    public TDetail FetchDetail<TDetail, TDetailBusiness>(DbTransaction transaction, Criterions criterions)
      where TDetail : BusinessListBase<TDetail, TDetailBusiness>
      where TDetailBusiness : BusinessBase<TDetailBusiness>
    {
      return DoFetchDetail<TDetail, TDetailBusiness>(transaction, criterions, false);
    }

    /// <summary>
    /// ȡ��ҵ����󼯺�
    /// ��������ֶ�ӳ���ϵ����Phenix.Core.Mapping.CriteriaFieldAttribute��ע
    /// </summary>
    /// <param name="transaction">���ݿ�����</param>
    /// <param name="criterions">������</param>
    /// <param name="lazyFetch">�Ƿ����Fetch(</param>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
    public TDetail FetchDetail<TDetail, TDetailBusiness>(DbTransaction transaction, Criterions criterions, bool lazyFetch)
      where TDetail : BusinessListBase<TDetail, TDetailBusiness>
      where TDetailBusiness : BusinessBase<TDetailBusiness>
    {
      return DoFetchDetail<TDetail, TDetailBusiness>(transaction, criterions, lazyFetch);
    }

    /// <summary>
    /// ȡ��ҵ����󼯺�
    /// </summary>
    /// <param name="transaction">���ݿ�����</param>
    /// <param name="orderByInfos">��������˳�����</param>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
    public TDetail FetchDetail<TDetail, TDetailBusiness>(DbTransaction transaction, params OrderByInfo[] orderByInfos)
      where TDetail : BusinessListBase<TDetail, TDetailBusiness>
      where TDetailBusiness : BusinessBase<TDetailBusiness>
    {
      return DoFetchDetail<TDetail, TDetailBusiness>(transaction, new Criterions(typeof(TDetail), orderByInfos), false);
    }

    /// <summary>
    /// ȡ��ҵ����󼯺�
    /// </summary>
    /// <param name="transaction">���ݿ�����</param>
    /// <param name="groupName">������</param>
    /// <param name="orderByInfos">��������˳�����</param>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
    public TDetail FetchDetail<TDetail, TDetailBusiness>(DbTransaction transaction, string groupName, params OrderByInfo[] orderByInfos)
      where TDetail : BusinessListBase<TDetail, TDetailBusiness>
      where TDetailBusiness : BusinessBase<TDetailBusiness>
    {
      return DoFetchDetail<TDetail, TDetailBusiness>(transaction, new Criterions(typeof(TDetail), null, groupName, orderByInfos), false);
    }

    /// <summary>
    /// ȡ��ҵ����󼯺�
    /// ��������ֶ�ӳ���ϵ����Phenix.Core.Mapping.CriteriaFieldAttribute��ע
    /// </summary>
    /// <param name="transaction">���ݿ�����</param>
    /// <param name="criteria">��ҵ����������</param>
    /// <param name="orderByInfos">��������˳�����</param>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
    public TDetail FetchDetail<TDetail, TDetailBusiness>(DbTransaction transaction, ICriteria criteria, params OrderByInfo[] orderByInfos)
      where TDetail : BusinessListBase<TDetail, TDetailBusiness>
      where TDetailBusiness : BusinessBase<TDetailBusiness>
    {
      return DoFetchDetail<TDetail, TDetailBusiness>(transaction, new Criterions(typeof(TDetail), criteria, orderByInfos), false);
    }

    /// <summary>
    /// ȡ��ҵ����󼯺�
    /// ��������ֶ�ӳ���ϵ����Phenix.Core.Mapping.CriteriaFieldAttribute��ע
    /// </summary>
    /// <param name="transaction">���ݿ�����</param>
    /// <param name="criteria">��ҵ����������</param>
    /// <param name="groupName">������</param>
    /// <param name="orderByInfos">��������˳�����</param>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
    public TDetail FetchDetail<TDetail, TDetailBusiness>(DbTransaction transaction, ICriteria criteria, string groupName, params OrderByInfo[] orderByInfos)
      where TDetail : BusinessListBase<TDetail, TDetailBusiness>
      where TDetailBusiness : BusinessBase<TDetailBusiness>
    {
      return DoFetchDetail<TDetail, TDetailBusiness>(transaction, new Criterions(typeof(TDetail), criteria, null, groupName, orderByInfos), false);
    }

    /// <summary>
    /// ȡ��ҵ����󼯺�
    /// </summary>
    /// <param name="transaction">���ݿ�����</param>
    /// <param name="criteriaExpression">��ҵ���������ʽ</param>
    /// <param name="orderByInfos">��������˳�����</param>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
    public TDetail FetchDetail<TDetail, TDetailBusiness>(DbTransaction transaction, CriteriaExpression criteriaExpression, params OrderByInfo[] orderByInfos)
      where TDetail : BusinessListBase<TDetail, TDetailBusiness>
      where TDetailBusiness : BusinessBase<TDetailBusiness>
    {
      return DoFetchDetail<TDetail, TDetailBusiness>(transaction, new Criterions(typeof(TDetail), criteriaExpression, orderByInfos), false);
    }

    /// <summary>
    /// ȡ��ҵ����󼯺�
    /// </summary>
    /// <param name="transaction">���ݿ�����</param>
    /// <param name="criteriaExpression">��ҵ���������ʽ</param>
    /// <param name="groupName">������</param>
    /// <param name="orderByInfos">��������˳�����</param>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
    public TDetail FetchDetail<TDetail, TDetailBusiness>(DbTransaction transaction, CriteriaExpression criteriaExpression, string groupName, params OrderByInfo[] orderByInfos)
      where TDetail : BusinessListBase<TDetail, TDetailBusiness>
      where TDetailBusiness : BusinessBase<TDetailBusiness>
    {
      return DoFetchDetail<TDetail, TDetailBusiness>(transaction, new Criterions(typeof(TDetail), criteriaExpression, null, groupName, orderByInfos), false);
    }

    /// <summary>
    /// ȡ��ҵ����󼯺�
    /// </summary>
    /// <param name="transaction">���ݿ�����</param>
    /// <param name="criteriaExpression">��ҵ���������ʽ</param>
    /// <param name="orderByInfos">��������˳�����</param>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures")]
    public TDetail FetchDetail<TDetail, TDetailBusiness>(DbTransaction transaction, Expression<Func<TDetailBusiness, bool>> criteriaExpression, params OrderByInfo[] orderByInfos)
      where TDetail : BusinessListBase<TDetail, TDetailBusiness>
      where TDetailBusiness : BusinessBase<TDetailBusiness>
    {
      return FetchDetail<TDetail, TDetailBusiness>(transaction, CriteriaHelper.ToCriteriaExpression(criteriaExpression), orderByInfos);
    }

    /// <summary>
    /// ȡ��ҵ����󼯺�
    /// </summary>
    /// <param name="transaction">���ݿ�����</param>
    /// <param name="criteriaExpression">��ҵ���������ʽ</param>
    /// <param name="groupName">������</param>
    /// <param name="orderByInfos">��������˳�����</param>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures")]
    public TDetail FetchDetail<TDetail, TDetailBusiness>(DbTransaction transaction, Expression<Func<TDetailBusiness, bool>> criteriaExpression, string groupName, params OrderByInfo[] orderByInfos)
      where TDetail : BusinessListBase<TDetail, TDetailBusiness>
      where TDetailBusiness : BusinessBase<TDetailBusiness>
    {
      return FetchDetail<TDetail, TDetailBusiness>(transaction, CriteriaHelper.ToCriteriaExpression(criteriaExpression), groupName, orderByInfos);
    }

    #endregion

    #region Selectable

    private void EnumKeyCaptionCollection_ItemSelectedValueChanged(object sender, SelectedValueChangedEventArgs e)
    {
      EnumKeyCaption selectable = (EnumKeyCaption)e.Selectable;
      if (selectable.Selected)
      {
        TBusiness business = AddNew(Count);
        business.LinkTo(selectable, null, true);
      }
      else
        for (int i = Count - 1; i >= 0; i--)
          if (Items[i].IsLink((object)selectable, null))
          {
            RemoveAt(i);
            break;
          }
    }

    private EnumKeyCaptionCollection DoGetSelectableList(EnumKeyCaptionCollection source)
    {
      EnumKeyCaptionCollection result = source.Clone();
      //ע��Selectedֵ����¼�
      result.ItemSelectedValueChanged -= new EventHandler<SelectedValueChangedEventArgs>(EnumKeyCaptionCollection_ItemSelectedValueChanged);
      //����Selectedֵ
      bool oldRaiseListChangedEvents = result.RaiseListChangedEvents;
      try
      {
        result.RaiseListChangedEvents = false;
        foreach (EnumKeyCaption enumKeyCaption in result)
        {
          bool find = false;
          foreach (TBusiness business in this)
            if (business.IsLink((object)enumKeyCaption, null))
            {
              find = true;
              enumKeyCaption.Selected = true;
              break;
            }
          if (!find)
            enumKeyCaption.Selected = false;
        }
      }
      finally
      {
        result.RaiseListChangedEvents = oldRaiseListChangedEvents;
      }
      //ע��Selectedֵ����¼�
      result.ItemSelectedValueChanged += new EventHandler<SelectedValueChangedEventArgs>(EnumKeyCaptionCollection_ItemSelectedValueChanged);
      return result;
    }

    /// <summary>
    /// ȡ��ѡ���嵥
    /// ��������ΪA(����ҵ����󼯺�)��B����(source)�Ľ����������ʱ���ɷ���ˢ�¹�(�뱾��������ڹ�������Selected������Ϊtrue)��B���ϣ��������������ʱ����ʱ��ӳ��������
    /// </summary>
    /// <param name="source">Դö�ټ���</param>
    public EnumKeyCaptionCollection GetSelectableList(EnumKeyCaptionCollection source)
    {
      EnumKeyCaptionCollection result;
      if (!SelectableEnumKeyCaptionCollections.TryGetValue(source.EnumType, out result))
      {
        result = DoGetSelectableList(source);
        SelectableEnumKeyCaptionCollections[source.EnumType] = result;
      }
      return result;
    }

    private void EnumBusinessList_ItemSelectedValueChanged(object sender, SelectedValueChangedEventArgs e)
    {
      TBusiness selectable = (TBusiness)e.Selectable;
      if (selectable.Selected)
      {
        TBusiness item = AddNew(selectable);
        item.IdValue = selectable.IdValue;
        item.SetMergeMode(selectable);
      }
      else
        for (int i = Count - 1; i >= 0; i--)
          if (Items[i].IdValue == selectable.IdValue)
          {
            Items[i].RemoveMergeMode(selectable);
            RemoveAt(i);
            break;
          }
    }

    private T DoCollatingSelectableList(EnumKeyCaptionCollection source)
    {
      T result = Clone();
      while (result.EditLevel > 0)
        result.ApplyEdit();
      //ע��Selectedֵ����¼�
      result.ItemSelectedValueChanged -= new EventHandler<SelectedValueChangedEventArgs>(EnumBusinessList_ItemSelectedValueChanged);
      //����Selectedֵ
      bool oldRaiseListChangedEvents = result.RaiseListChangedEvents;
      try
      {
        result.RaiseListChangedEvents = false;
        IList<FieldMapInfo> fieldMapInfos = ClassMemberHelper.GetFieldMapInfos(ItemValueType, source.EnumType, true);
        foreach (EnumKeyCaption keyCaption in source)
        {
          TBusiness find = null;
          foreach (TBusiness selectable in result)
            if (selectable.IsLink((object)keyCaption, null))
            {
              find = selectable;
              break;
            }

          if (find != null)
          {
            find.Selected = true;
            foreach (TBusiness business in this)
              if (business.IsLink((object)keyCaption, null))
              {
                find.IdValue = business.IdValue;
                business.SetMergeMode(find);
                break;
              }
           }
          else
          {
            List<object> values = new List<object>(fieldMapInfos.Count);
            for (int i = 0; i < fieldMapInfos.Count; i++)
              values.Add(keyCaption.Value);
            result.Add(BusinessBase<TBusiness>.Fetch(values.ToArray(), fieldMapInfos));
          }
        }
        result.InSelectableList = true;
      }
      finally
      {
        result.RaiseListChangedEvents = oldRaiseListChangedEvents;
      }
      //ע��Selectedֵ����¼�
      result.ItemSelectedValueChanged += new EventHandler<SelectedValueChangedEventArgs>(EnumBusinessList_ItemSelectedValueChanged);
      return result;
    }

    /// <summary>
    /// ����ѡ���嵥
    /// ��������ΪA(����ҵ����󼯺�)��B����(source)�Ľ����������ʱ���ɷ���ˢ�¹�(�뱾��������ڹ�������Selected������Ϊtrue)��B���ϣ��������������ʱ����ʱ��ӳ��������
    /// </summary>
    /// <param name="source">Դö�ټ���</param>
    public T CollatingSelectableList(EnumKeyCaptionCollection source)
    {
      T result;
      string key = String.Format("{0}", source.EnumType.FullName);
      if (!Selectables.TryGetValue(key, out result))
      {
        result = DoCollatingSelectableList(source);
        result.SynchronizeEditLevel(this);
        Selectables[key] = result;
      }
      return result;
    }

    private void SelectableList_ItemSelectedValueChanging(object sender, SelectedValueChangingEventArgs e)
    {
      IBusinessObject selectable = (IBusinessObject)e.Selectable;
      if (selectable.Selected)
        if (EmptyIsAllSelected && Count == 1)
        {
          //������ȫ�����
          e.Stop = true;
        }
    }

    private void SelectableList_ItemSelectedValueChanged(object sender, SelectedValueChangedEventArgs e)
    {
      IBusinessObject selectable = (IBusinessObject)e.Selectable;
      if (selectable.Selected)
      {
        if (EmptyIsAllSelected && Count == selectable.Owner.Count - 1)
        {
          //��N-1����ȫѡN��ʱ������N-1�����
          Clear();
        }
        else
        {
          TBusiness business = AddNew(selectable);
          business.LinkTo(selectable, GroupName, true);
          business.LinkTo(selectable.MasterBusiness, selectable.GroupName, true);
          business.SetMergeMode(selectable);
        }
      }
      else
      {
        if (EmptyIsAllSelected && Count == 0)
        {
          //��ȫѡN����N-1��ʱ������N-1�����ɲ�������
          foreach (IBusinessObject item in selectable.Owner)
            if (item.Selected)
            {
              TBusiness business = AddNew(item);
              business.LinkTo(item, GroupName, true);
              business.LinkTo(item.MasterBusiness, item.GroupName, true);
              business.SetMergeMode(item);
            }
        }
        else
        {
          for (int i = Count - 1; i >= 0; i--)
            if (Items[i].IsLink((object)selectable, GroupName))
            {
              Items[i].RemoveMergeMode(selectable);
              RemoveAt(i);
              break;
            }
        }
      }
    }

    private TSelectableList DoCollatingSelectableList<TSelectableList, TSelectable>(TSelectableList source, bool emptyIsAllSelected)
      where TSelectableList : BusinessListBase<TSelectableList, TSelectable>
      where TSelectable : BusinessBase<TSelectable>
    {
      EmptyIsAllSelected = emptyIsAllSelected;
      TSelectableList result = source.Clone();
      if (result.Criterions == null)
        result.Criterions = new Criterions(typeof(TSelectableList), false, false);
      if (Criterions != null)
        result.Criterions.SetLink(Criterions.MasterBusiness, Criterions.GroupName);
      while (result.EditLevel > 0)
        result.ApplyEdit();
      //ע��Selectedֵ����¼�
      result.ItemSelectedValueChanging -= new EventHandler<SelectedValueChangingEventArgs>(SelectableList_ItemSelectedValueChanging);
      result.ItemSelectedValueChanged -= new EventHandler<SelectedValueChangedEventArgs>(SelectableList_ItemSelectedValueChanged);
      //����Selectedֵ
      bool oldRaiseListChangedEvents = result.RaiseListChangedEvents;
      try
      {
        result.RaiseListChangedEvents = false;
        TBusiness template = BusinessBase<TBusiness>.New();
        if (Count == 0)
          foreach (TSelectable selectable in result)
          {
            EntityHelper.FillIdenticalFieldValuesByTargetProperty(template, selectable, false);
            selectable.Selected = emptyIsAllSelected;
          }
        else
          foreach (TSelectable selectable in result)
          {
            bool find = false;
            foreach (TBusiness business in this)
              if (business.IsLink(selectable, GroupName))
              {
                find = true;
                EntityHelper.FillIdenticalFieldValuesByTargetProperty(business, selectable, false);
                selectable.Selected = true;
                business.SetMergeMode(selectable);
                break;
              }
            if (!find)
            {
              EntityHelper.FillIdenticalFieldValuesByTargetProperty(template, selectable, false);
              selectable.Selected = false;
            }
          }
        result.InSelectableList = true;
      }
      finally
      {
        result.RaiseListChangedEvents = oldRaiseListChangedEvents;
      }
      //ע��Selectedֵ����¼�
      result.ItemSelectedValueChanging += new EventHandler<SelectedValueChangingEventArgs>(SelectableList_ItemSelectedValueChanging);
      result.ItemSelectedValueChanged += new EventHandler<SelectedValueChangedEventArgs>(SelectableList_ItemSelectedValueChanged);
      return result;
    }

    /// <summary>
    /// ����ѡ���嵥
    /// ��������ΪA(����ҵ����󼯺�)��B����(source)�Ľ����������ʱ���ɷ���ˢ�¹�(�뱾��������ڹ�������Selected������Ϊtrue)��B���ϣ��������������ʱ����ʱ��ӳ��������
    /// emptyIsAllSelected = false
    /// </summary>
    /// <param name="source">Դҵ�񼯺�</param>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
    public TSelectableList CollatingSelectableList<TSelectableList, TSelectable>(TSelectableList source)
      where TSelectableList : BusinessListBase<TSelectableList, TSelectable>
      where TSelectable : BusinessBase<TSelectable>
    {
      return CollatingSelectableList<TSelectableList, TSelectable>(source, false);
    }

    /// <summary>
    /// ����ѡ���嵥
    /// ��������ΪA(����ҵ����󼯺�)��B����(source)�Ľ����������ʱ���ɷ���ˢ�¹�(�뱾��������ڹ�������Selected������Ϊtrue)��B���ϣ��������������ʱ����ʱ��ӳ��������
    /// </summary>
    /// <param name="source">Դҵ�񼯺�</param>
    /// <param name="emptyIsAllSelected">�ռ��ϴ���ȫѡ</param>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
    public TSelectableList CollatingSelectableList<TSelectableList, TSelectable>(TSelectableList source, bool emptyIsAllSelected)
      where TSelectableList : BusinessListBase<TSelectableList, TSelectable>
      where TSelectable : BusinessBase<TSelectable>
    {
      IBusinessCollection result;
      string key = String.Format("{0},{1}", source.Criterions.ToString(), emptyIsAllSelected);
      if (!SelectableBusinessCollections.TryGetValue(key, out result))
      {
        result = DoCollatingSelectableList<TSelectableList, TSelectable>(source, emptyIsAllSelected);
        ((IRefinedly)result).SynchronizeEditLevel(this);
        SelectableBusinessCollections[key] = result;
      }
      return (TSelectableList)result;
    }

    private void SimilBusinessList_ItemSelectedValueChanged(object sender, SelectedValueChangedEventArgs e)
    {
      string primaryKey = ((TBusiness)e.Selectable).PrimaryKey;
      foreach (TBusiness item in this)
        if (String.Compare(primaryKey, item.PrimaryKey, StringComparison.OrdinalIgnoreCase) == 0)
        {
          if (e.Selectable.Selected)
            item.LinkTo(MasterBusiness, GroupName, true);
          else
            item.Unlink(MasterBusiness, GroupName, true);
          break;
        }
    }

    private T DoCollatingSelectableList(IBusinessObject masterBusiness, string groupName)
    {
      Criterions criterions = Criterions != null ? Criterions.PureClone() : new Criterions(typeof(T), false, false);
      criterions.SetLink(masterBusiness, false);
      T result = masterBusiness.FindDetail<T, TBusiness>(criterions);
      if (result == null)
      {
        result = Clone();
        if (result.Criterions == null)
          result.Criterions = new Criterions(typeof(T), false, false);
        result.Criterions.SetLink(masterBusiness, false);
        masterBusiness.SetDetail<T, TBusiness>(result);
      }
      while (result.EditLevel > 0)
        result.ApplyEdit();
      //ע��Selectedֵ����¼�
      result.ItemSelectedValueChanged -= new EventHandler<SelectedValueChangedEventArgs>(result.SimilBusinessList_ItemSelectedValueChanged);
      //����Selectedֵ
      bool oldRaiseListChangedEvents = result.RaiseListChangedEvents;
      try
      {
        result.RaiseListChangedEvents = false;
        foreach (TBusiness selectable in result)
          selectable.Selected = selectable.IsLink(masterBusiness, groupName);
        result.InSelectableList = true;
      }
      finally
      {
        result.RaiseListChangedEvents = oldRaiseListChangedEvents;
      }
      //ע��Selectedֵ����¼�
      result.ItemSelectedValueChanged += new EventHandler<SelectedValueChangedEventArgs>(result.SimilBusinessList_ItemSelectedValueChanged);
      return result;
    }

    /// <summary>
    /// ����ѡ���嵥
    /// ����ˢ�¹�(��masterBusiness���ڹ�������Selected������Ϊtrue)�ļ��ϣ���Щ��Selected���ʱ�������ֵ����������masterBusiness.Root��һ���ύ
    /// </summary>
    /// <param name="masterBusiness">��ҵ�����</param>
    public T CollatingSelectableList(IBusinessObject masterBusiness)
    {
      return DoCollatingSelectableList(masterBusiness, null);
    }

    /// <summary>
    /// ����ѡ���嵥
    /// ����ˢ�¹�(��masterBusiness���ڹ�������Selected������Ϊtrue)�ļ��ϣ���Щ��Selected���ʱ�������ֵ����������masterBusiness.Root��һ���ύ
    /// </summary>
    /// <param name="masterBusiness">��ҵ�����</param>
    /// <param name="groupName">������, null����ȫ��</param>
    public T CollatingSelectableList(IBusinessObject masterBusiness, string groupName)
    {
      return DoCollatingSelectableList(masterBusiness, groupName);
    }

    #endregion

    #region Select

    /// <summary>
    /// ����������ѡҵ�����
    /// </summary>
    /// <param name="toSelected">ʹ�ñ���ѡ</param>
    /// <param name="match">����Ҫ��ѡ��Ԫ��Ӧ���������</param>
    public void SelectAll(bool toSelected, Predicate<TBusiness> match)
    {
      foreach (TBusiness item in this)
        if (match == null || match(item))
          item.Selected = toSelected;
    }

    /// <summary>
    /// ��ѡ����
    /// match = null
    /// </summary>
    /// <param name="toSelected">ʹ�ñ���ѡ</param>
    public void SelectAll(bool toSelected)
    {
      SelectAll(toSelected, null);
    }

    #region ISelectableCollection ��Ա

    /// <summary>
    /// ��ѡ����
    /// </summary>
    public void SelectAll()
    {
      SelectAll(true);
    }

    /// <summary>
    /// ȡ�����й�ѡ
    /// </summary>
    public void UnselectAll()
    {
      SelectAll(false);
    }

    /// <summary>
    /// ��ѡ����
    /// </summary>
    public void InverseAll()
    {
      foreach (TBusiness item in this)
        item.Selected = !item.Selected;
    }

    #endregion

    #endregion

    #region Parent

    /// <summary>
    /// ���ø�����
    /// </summary>
    /// <param name="parent">������</param>
    protected override void SetParent(Csla.Core.IParent parent)
    {
      if (parent != null)
        SynchronizeEditLevel((IRefinedly)parent);
      base.SetParent(parent);
    }

    #endregion

    #region FindItem

    /// <summary>
    /// ������һ��ƥ�����, ���� PrimaryKey ƥ��
    /// </summary>
    /// <param name="primaryKey">����</param>
    public virtual TBusiness FindItem(string primaryKey)
    {
      foreach (TBusiness item in this)
        if (String.CompareOrdinal(item.PrimaryKey, primaryKey) == 0)
          return item;
      return null;
    }
    /// <summary>
    /// ������һ��ƥ�����, ���� PrimaryKey ƥ��
    /// </summary>
    /// <param name="primaryKey">����</param>
    IBusinessObject IBusinessCollection.FindItem(string primaryKey)
    {
      return FindItem(primaryKey);
    }

    #endregion

    #region �༭����

    private bool GetIsDirty(ref List<IRefinedlyObject> ignoreLinks)
    {
      foreach (TBusiness item in DeletedList)
        if (!item.IsNew)
          return true;

      foreach (IRefinedly item in this)
        if (item.GetIsDirty(ref ignoreLinks))
          return true;
      return false;
    }
    bool IRefinedly.GetIsDirty(ref List<IRefinedlyObject> ignoreLinks)
    {
      return GetIsDirty(ref ignoreLinks);
    }

    private bool GetIsValid(ref List<IRefinedlyObject> ignoreLinks)
    {
      foreach (IRefinedly item in this)
        if (!item.GetIsValid(ref ignoreLinks))
          return false;
      return true;
    }
    bool IRefinedly.GetIsValid(ref List<IRefinedlyObject> ignoreLinks)
    {
      return GetIsValid(ref ignoreLinks);
    }

    private bool GetNeedRefresh(ref List<IRefinedlyObject> ignoreLinks)
    {
      foreach (IRefinedly item in this)
        if (item.GetNeedRefresh(ref ignoreLinks))
          return true;
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
        throw new Csla.Core.UndoException(String.Format("{0}: EditLevel={1}Ӧ��<={2}!", this.GetType().FullName, EditLevel, business.EditLevel));
      for (int i = 0; i < business.EditLevel - EditLevel; i++)
        BeginEdit();
    }
    void IRefinedly.SynchronizeEditLevel(IRefinedly source)
    {
      SynchronizeEditLevel(source);
    }

    private static void CheckBusinessObjectMember()
    {
      Type type = typeof(T);
      string message = String.Empty;
      foreach (FieldInfo item in Utilities.GetInstanceFields(type, typeof(IBusiness), typeof(IBusiness)))
        if (Attribute.GetCustomAttribute(item, typeof(Csla.NotUndoableAttribute)) == null)
          message = message + String.Format("��Ϊ{0}.{1}�ֶα����{2}��ǩ",
            (item.DeclaringType ?? type).FullName, item.Name, typeof(Csla.NotUndoableAttribute).FullName) + AppConfig.CR_LF;
      if (!String.IsNullOrEmpty(message))
      {
        Csla.Core.UndoException undoException = new Csla.Core.UndoException(message);
        EventLog.Save(type, undoException);
        throw undoException;
      }
      BusinessBase<TBusiness>.CheckBusinessObjectMember();
    }

    /// <summary>
    /// ��ʼ�༭
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
      foreach (TBusiness item in this)
        item.BeginEditInDetailsAndLinks(editLevel, ref ignoreLinks);
      foreach (TBusiness item in this.DeletedList)
        item.BeginEditInDetailsAndLinks(editLevel, ref ignoreLinks);
    }
    void IRefinedly.BeginEdit(int editLevel, ref List<IRefinedlyObject> ignoreLinks)
    {
      BeginEdit(editLevel, ref ignoreLinks);
    }

    /// <summary>
    /// ȡ���༭
    /// </summary>
    public new void CancelEdit()
    {
      List<IRefinedlyObject> ignoreLinks = new List<IRefinedlyObject>();
      CancelEdit(EditLevel - 1, ref ignoreLinks);
    }

    private bool CancelEdit(int editLevel, ref List<IRefinedlyObject> ignoreLinks)
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
        bool oldRaiseListChangedEvents = RaiseListChangedEvents;
        try
        {
          RaiseListChangedEvents = false;
          Remove(item => item.IsNew);
          foreach (TBusiness item in this)
            if (item.CancelEdit(editLevel, ref ignoreLinks))
              result = true;
          foreach (TBusiness item in DeletedList)
            if (!item.IsNew)
            {
              if (item.CancelEdit(editLevel, ref ignoreLinks))
                result = true;
              base.Add(item);
            }
          DeletedList.Clear();
        }
        finally
        {
          RaiseListChangedEvents = oldRaiseListChangedEvents;
        }
      }
      else
      {
        foreach (TBusiness item in this)
          if (item.CancelEditInDetailsAndLinks(editLevel, ref ignoreLinks))
            result = true;
        foreach (TBusiness item in DeletedList)
          if (item.CancelEditInDetailsAndLinks(editLevel, ref ignoreLinks))
            result = true;
      }
      if (_selectables != null || _selectableBusinessCollections != null)
      {
        _selectables = null;
        _selectableBusinessCollections = null;
        result = true;
      }
      if (result && RaiseListChangedEvents)
        OnListChanged(new ListChangedEventArgs(ListChangedType.Reset, -1));
      return result;
    }
    bool IRefinedly.CancelEdit(int editLevel, ref List<IRefinedlyObject> ignoreLinks)
    {
      return CancelEdit(editLevel, ref ignoreLinks);
    }

    /// <summary>
    /// ���ܱ༭
    /// </summary>
    public new void ApplyEdit()
    {
      List<IRefinedlyObject> ignoreLinks = new List<IRefinedlyObject>();
      ApplyEdit(EditLevel - 1, ref ignoreLinks);
    }

    private void ApplyEdit(int editLevel, ref List<IRefinedlyObject> ignoreLinks)
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
      {
        bool oldRaiseListChangedEvents = RaiseListChangedEvents;
        try
        {
          RaiseListChangedEvents = false;
          foreach (TBusiness item in this)
            item.ApplyEdit(editLevel, ref ignoreLinks);
          DeletedList.Clear();
        }
        finally
        {
          RaiseListChangedEvents = oldRaiseListChangedEvents;
        }
      }
      else
      {
        foreach (TBusiness item in this)
          item.ApplyEditInDetailsAndLinks(editLevel, ref ignoreLinks);
        foreach (TBusiness item in this.DeletedList)
          item.ApplyEditInDetailsAndLinks(editLevel, ref ignoreLinks);
      }
    }
    void IRefinedly.ApplyEdit(int editLevel, ref List<IRefinedlyObject> ignoreLinks)
    {
      ApplyEdit(editLevel, ref ignoreLinks);
    }
    
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
    private void CompletelyApplyEdit(bool toMarkOld, bool inCascadingDelete, bool needCheckDirty, bool keepEnsemble, ref List<IRefinedlyObject> ignoreLinks)
    {
      inCascadingDelete = inCascadingDelete && CascadingDelete;

      bool oldRaiseListChangedEvents = RaiseListChangedEvents;
      try
      {
        RaiseListChangedEvents = false;

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

        bool oldAllowRemove = AllowRemove;
        try
        {
          AllowRemove = true;
          
          if (!CascadingSave && !toMarkOld && !keepEnsemble)
          {
            __ClearItems();
            return;
          }

          for (int i = Count - 1; i >= 0; i--)
          {
            if (i >= Count)
              continue;
            TBusiness item = Items[i];
            if (!toMarkOld && MasterBusiness != null && !CascadingDelete && MasterBusiness.IsDeleted)
              item.Unlink(MasterBusiness, GroupName, true);
            if (inCascadingDelete)
              RemoveItem(i);
            else if (!toMarkOld && !keepEnsemble && !EnsembleOnSaving && !((IBusiness)item).EnsembleOnSaving && (!item.IsDirty || (OnlySaveSelected && !item.Selected)))
            {
              RemoveItem(i);
              DeletedList.Remove(item);
            }
            else
              ((IRefinedly)item).CompletelyApplyEdit(toMarkOld, false, needCheckDirty, keepEnsemble || EnsembleOnSaving, ref ignoreLinks);
          }
          if (toMarkOld)
            DeletedList.Clear();
          else
            for (int i = DeletedList.Count - 1; i >= 0; i--)
            {
              if (i >= DeletedList.Count)
                continue;
              TBusiness item = DeletedList[i];
              if (MasterBusiness != null && !CascadingDelete)
                item.Unlink(MasterBusiness, GroupName, true);
              if (item.IsNew || !item.IsSelfDeleted ||
                !keepEnsemble && !EnsembleOnSaving && !((IBusiness)item).EnsembleOnSaving && (!item.IsSelfDirty || OnlySaveSelected && !item.Selected))
                DeletedList.RemoveAt(i);
              else
                ((IRefinedly)item).CompletelyApplyEdit(toMarkOld, true, needCheckDirty, keepEnsemble || EnsembleOnSaving, ref ignoreLinks);
            }
        }
        finally
        {
          AllowRemove = oldAllowRemove;
        }
      }
      finally
      {
        RaiseListChangedEvents = oldRaiseListChangedEvents;
      }
    }
    void IRefinedly.CompletelyApplyEdit(bool toMarkOld, bool inCascadingDelete, bool needCheckDirty, bool keepEnsemble, ref List<IRefinedlyObject> ignoreLinks)
    {
      CompletelyApplyEdit(toMarkOld, inCascadingDelete, needCheckDirty, keepEnsemble, ref ignoreLinks);
    }

    #region CascadingRefresh

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
    private bool CascadingRefresh(T source)
    {
      if (object.ReferenceEquals(source, null) || object.ReferenceEquals(source, this))
        return false;

      bool result = false;
      bool oldRaiseListChangedEvents = RaiseListChangedEvents;
      try
      {
        RaiseListChangedEvents = false;

        if (source.Count > 0 && Count > 0)
          foreach (TBusiness sourceBusiness in source)
            if (((IBusiness)sourceBusiness).NeedRefresh)
            {
              bool find = false;
              foreach (TBusiness targetBusiness in this)
                if (sourceBusiness.IdValue == targetBusiness.IdValue ||
                  !String.IsNullOrEmpty(sourceBusiness.PrimaryKey) && String.CompareOrdinal(sourceBusiness.PrimaryKey, targetBusiness.PrimaryKey) == 0)
                {
                  find = true;
                  if (targetBusiness.CascadingRefresh(sourceBusiness))
                    result = true;
                  break;
                }
              if (!find)
              {
                Add(sourceBusiness.Clone(false));
                result = true;
              }
            }
        if (source.DeletedList.Count > 0 && Count > 0)
          foreach (TBusiness sourceBusiness in source.DeletedList)
            if (((IBusiness)sourceBusiness).NeedRefresh)
              if (sourceBusiness.IsSelfDeleted)
                for (int i = Count - 1; i >= 0; i--)
                {
                  TBusiness targetBusiness = Items[i];
                  if (sourceBusiness.IdValue == targetBusiness.IdValue ||
                    !String.IsNullOrEmpty(sourceBusiness.PrimaryKey) && String.CompareOrdinal(sourceBusiness.PrimaryKey, targetBusiness.PrimaryKey) == 0)
                  {
                    RemoveItem(i);
                    DeletedList.Remove(targetBusiness);
                    result = true;
                    break;
                  }
                }
              else
              {
                bool find = false;
                foreach (TBusiness targetBusiness in this)
                  if (sourceBusiness.IdValue == targetBusiness.IdValue ||
                    !String.IsNullOrEmpty(sourceBusiness.PrimaryKey) && String.CompareOrdinal(sourceBusiness.PrimaryKey, targetBusiness.PrimaryKey) == 0)
                  {
                    find = true;
                    if (targetBusiness.CascadingRefresh(sourceBusiness))
                      result = true;
                    break;
                  }
                if (!find)
                {
                  Add(sourceBusiness.Clone(false));
                  result = true;
                }
              }
      }
      finally
      {
        RaiseListChangedEvents = oldRaiseListChangedEvents;
        if (RaiseListChangedEvents && result)
          OnListChanged(new ListChangedEventArgs(ListChangedType.Reset, -1));
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
      foreach (IRefinedly item in this)
        item.CascadingLinkTo(link, groupName, throwIfNotFound);
    }
    void IRefinedly.CascadingLinkTo(object link, string groupName, bool throwIfNotFound)
    {
      CascadingLinkTo(link, groupName, throwIfNotFound);
    }

    #endregion

    #region CascadingUnlink

    private void CascadingUnlink(object link, string groupName, bool throwIfNotFound)
    {
      foreach (IRefinedly item in this)
        item.CascadingUnlink(link, groupName, throwIfNotFound);
    }
    void IRefinedly.CascadingUnlink(object link, string groupName, bool throwIfNotFound)
    {
      CascadingUnlink(link, groupName, throwIfNotFound);
    }

    #endregion

    #region CascadingExistLink

    private bool CascadingExistLink(Type masterType, string groupName)
    {
      if (ClassMemberHelper.ExistLink(ItemValueType, masterType, groupName))
        return true;
      foreach (IRefinedly item in this)
        if (item.CascadingExistLink(masterType, groupName))
          return true;
      return false;
    }
    bool IRefinedly.CascadingExistLink(Type masterType, string groupName)
    {
      return CascadingExistLink(masterType, groupName);
    }

    #endregion

    #region CascadingFillIdenticalValues

    private void CascadingFillIdenticalValues(IRefinedlyObject source, string[] sourcePropertyNames, ref List<IRefinedlyObject> ignoreLinks)
    {
      foreach (IRefinedly item in this)
        item.CascadingFillIdenticalValues(source, sourcePropertyNames, ref ignoreLinks);
    }
    void IRefinedly.CascadingFillIdenticalValues(IRefinedlyObject source, string[] sourcePropertyNames, ref List<IRefinedlyObject> ignoreLinks)
    {
      CascadingFillIdenticalValues(source, sourcePropertyNames, ref ignoreLinks);
    }

    #endregion

    #region FillAggregateValues

    /// <summary>
    /// ����ۺ��ֶ�
    /// property = null
    /// </summary>
    public void ComputeMasterAggregate()
    {
      ComputeMasterAggregate((IPropertyInfo)null);
    }

    /// <summary>
    /// ����ۺ��ֶ�
    /// </summary>
    /// <param name="propertyInfo">������Ϣ</param>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters")]
    public void ComputeMasterAggregate(IPropertyInfo propertyInfo)
    {
      if (MasterBusiness == null)
        throw new InvalidOperationException(String.Format("����{0}û����ҵ�����MasterBusiness����Ϊ��", Caption));
      if (propertyInfo != null)
      {
        CodingStandards.CheckFieldMapInfo(propertyInfo);

        if (propertyInfo.FieldMapInfo.FieldAggregateMapInfos.Count == 0)
          throw new InvalidOperationException(String.Format("��Ϊ{0}.{1}�ֶα����{2}��ǩ",
            typeof(TBusiness).FullName, propertyInfo.FieldMapInfo.Field.Name, typeof(FieldAggregateAttribute).FullName));
        foreach (FieldAggregateMapInfo item in propertyInfo.FieldMapInfo.FieldAggregateMapInfos)
          ((IRefinedlyObject)MasterBusiness).FillAggregateValues(item);
      }
      else
      {
        foreach (FieldAggregateMapInfo item in ClassMemberHelper.GetFieldAggregateMapInfos(ItemValueType, true))
          ((IRefinedlyObject)MasterBusiness).FillAggregateValues(item);
      }
    }

    /// <summary>
    /// ����ۺ��ֶ�
    /// </summary>
    /// <param name="propertyName">������</param>
    public void ComputeMasterAggregate(string propertyName)
    {
      if (MasterBusiness == null)
        throw new InvalidOperationException(String.Format("����{0}û����ҵ�����MasterBusiness����Ϊ��", Caption));
      if (String.IsNullOrEmpty(propertyName))
      {
        foreach (FieldAggregateMapInfo item in ClassMemberHelper.GetFieldAggregateMapInfos(ItemValueType, true))
          ((IRefinedlyObject)MasterBusiness).FillAggregateValues(item);
      }
      else
      {
        bool find = false;
        foreach (FieldAggregateMapInfo item in ClassMemberHelper.GetFieldAggregateMapInfos(ItemValueType, true))
          if (String.CompareOrdinal(item.Owner.PropertyName, propertyName) == 0)
          {
            find = true;
            ((IRefinedlyObject)MasterBusiness).FillAggregateValues(item);
          }
        if (!find)
          throw new InvalidOperationException(String.Format("��Ϊ{0}.{1}���Թ������ֶα����{2}��ǩ",
            typeof(TBusiness).FullName, propertyName, typeof(FieldAggregateAttribute).FullName));
      }
    }

    #endregion

    #endregion

    #region Data Access

    /// <summary>
    /// �鵵
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
    public void Archive()
    {
      if (AloneTransaction)
      {
        List<ExceptionEventArgs> saveErrors = new List<ExceptionEventArgs>();
        foreach (TBusiness item in this)
          try
          {
            item.Archive();
          }
          catch (Exception ex)
          {
            saveErrors.Add(new ExceptionEventArgs(item, ex));
          }
        if (saveErrors.Count > 0)
          throw new SaveException(saveErrors);
      }
      else
      {
        foreach (TBusiness item in this)
          item.Archive();
      }
    }

    /// <summary>
    /// �鵵(�����ڳ־ò�ĳ�������)
    /// <param name="connection">��������</param>
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
    public void Archive(DbConnection connection)
    {
      if (AloneTransaction)
      {
        List<ExceptionEventArgs> saveErrors = new List<ExceptionEventArgs>();
        foreach (TBusiness item in this)
          try
          {
            item.ExecuteArchive(connection);
          }
          catch (Exception ex)
          {
            saveErrors.Add(new ExceptionEventArgs(item, ex));
          }
        if (saveErrors.Count > 0)
          throw new SaveException(saveErrors);
      }
      else
      {
        foreach (TBusiness item in this)
          item.ExecuteArchive(connection);
      }
    }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
    private void ExecuteArchive(DbTransaction target, DbConnection source)
    {
      if (!CascadingSave || !CascadingDelete)
        return;
      
      foreach (TBusiness item in this)
        item.ExecuteArchive(target, source);
    }
    void IRefinedly.ExecuteArchive(DbTransaction target, DbConnection source)
    {
      ExecuteArchive(target, source);
    }

    /// <summary>
    /// ����
    /// </summary>
    /// <param name="needCheckDirty">У�����ݿ����������ص��ύ�ڼ��Ƿ񱻸��Ĺ�, һ�����ֽ�����: CheckDirtyException; ���ClassAttribute.AllowIgnoreCheckDirty = false��������Ч, �ض�����: CheckSaveException</param>
    /// <param name="onlySaveSelected">���ύ����ѡ��ҵ�����</param>
    /// <param name="firstTransactionData">����������ǰ�˵�ҵ�����</param>
    /// <param name="lastTransactionData">����������ĩ�˵�ҵ�����</param>
    /// <returns>�ɹ��ύ��ҵ����󼯺�</returns>
    public T Save(bool needCheckDirty, bool? onlySaveSelected, IBusiness[] firstTransactionData, IBusiness[] lastTransactionData)
    {
      if (onlySaveSelected.HasValue)
        OnlySaveSelected = onlySaveSelected.Value;
      return Save(needCheckDirty, firstTransactionData, lastTransactionData);
    }
    /// <summary>
    /// ����
    /// </summary>
    /// <param name="needCheckDirty">У�����ݿ����������ص��ύ�ڼ��Ƿ񱻸��Ĺ�, һ�����ֽ�����: CheckDirtyException; ���ClassAttribute.AllowIgnoreCheckDirty = false��������Ч, �ض�����: CheckSaveException</param>
    /// <param name="onlySaveSelected">���ύ����ѡ��ҵ�����</param>
    /// <param name="firstTransactionData">����������ǰ�˵�ҵ�����</param>
    /// <param name="lastTransactionData">����������ĩ�˵�ҵ�����</param>
    /// <returns>�ɹ��ύ�Ķ���</returns>
    IBusiness IBusiness.Save(bool needCheckDirty, bool? onlySaveSelected, IBusiness[] firstTransactionData, IBusiness[] lastTransactionData)
    {
      return Save(needCheckDirty, onlySaveSelected, firstTransactionData, lastTransactionData);
    }

    /// <summary>
    /// ����
    /// </summary>
    /// <returns>�ɹ��ύ��ҵ����󼯺�</returns>
    public new T Save()
    {
      return Save(true, null as IBusiness[], null as IBusiness[]);
    }
    object Csla.Core.ISavable.Save()
    {
      return Save();
    }

    /// <summary>
    /// ǿ�ư���Update��ʽ�ύ����
    /// </summary>
    /// <returns>�ɹ��ύ��ҵ����󼯺�</returns>
    public T SaveForceUpdate()
    {
      foreach (TBusiness item in this)
        if (item.IsNew)
          item.SetForceUpdate();
      return Save();
    }
    object Csla.Core.ISavable.Save(bool forceUpdate)
    {
      return SaveForceUpdate();
    }

    /// <summary>
    /// ����
    /// </summary>
    /// <param name="lastTransactionData">����������ĩ�˵�ҵ�����</param>
    /// <returns>�ɹ��ύ��ҵ����󼯺�</returns>
    public T Save(IBusiness[] lastTransactionData)
    {
      return Save(true, null as IBusiness[], lastTransactionData);
    }

    /// <summary>
    /// ����
    /// </summary>
    /// <param name="firstTransactionData">����������ǰ�˵�ҵ�����</param>
    /// <param name="lastTransactionData">����������ĩ�˵�ҵ�����</param>
    /// <returns>�ɹ��ύ��ҵ����󼯺�</returns>
    public T Save(IBusiness[] firstTransactionData, IBusiness[] lastTransactionData)
    {
      return Save(true, firstTransactionData, lastTransactionData);
    }

    /// <summary>
    /// ����
    /// </summary>
    /// <param name="needCheckDirty">У�����ݿ����������ص��ύ�ڼ��Ƿ񱻸��Ĺ�, һ�����ֽ�����: CheckDirtyException; ���ClassAttribute.AllowIgnoreCheckDirty = false��������Ч, �ض�����: CheckSaveException</param>
    /// <param name="lastTransactionData">����������ĩ�˵�ҵ�����</param>
    /// <returns>�ɹ��ύ��ҵ����󼯺�</returns>
    public T Save(bool needCheckDirty, params IBusiness[] lastTransactionData)
    {
      return Save(needCheckDirty, null as IBusiness[], lastTransactionData);
    }

    /// <summary>
    /// ����
    /// </summary>
    /// <param name="needCheckDirty">У�����ݿ����������ص��ύ�ڼ��Ƿ񱻸��Ĺ�, һ�����ֽ�����: CheckDirtyException; ���ClassAttribute.AllowIgnoreCheckDirty = false��������Ч, �ض�����: CheckSaveException</param>
    /// <param name="firstTransactionData">����������ǰ�˵�ҵ�����</param>
    /// <param name="lastTransactionData">����������ĩ�˵�ҵ�����</param>
    /// <returns>�ɹ��ύ��ҵ����󼯺�</returns>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2200:RethrowToPreserveStackDetails")]
    public virtual T Save(bool needCheckDirty, IBusiness[] firstTransactionData, IBusiness[] lastTransactionData)
    {
      ////������־
      //if (AppConfig.Debugging)
      //  EventLog.Save(String.Format("{0}.{1}: EditLevel={2}", this.GetType().FullName, MethodBase.GetCurrentMethod().Name, EditLevel));
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
    /// ����
    /// </summary>
    /// <param name="transaction">���ݿ�����, ���Ϊ��������������</param>
    /// <param name="needCheckDirty">У�����ݿ����������ص��ύ�ڼ��Ƿ񱻸��Ĺ�, һ�����ֽ�����: CheckDirtyException; ���ClassAttribute.AllowIgnoreCheckDirty = false��������Ч, �ض�����: CheckSaveException</param>
    /// <param name="onlySaveSelected">���ύ����ѡ��ҵ�����</param>
    /// <param name="firstTransactionData">����������ǰ�˵�ҵ�����</param>
    /// <param name="lastTransactionData">����������ĩ�˵�ҵ�����</param>
    /// <returns>�ɹ��ύ��ҵ����󼯺�</returns>
    public T Save(DbTransaction transaction, bool needCheckDirty, bool? onlySaveSelected, IBusiness[] firstTransactionData, IBusiness[] lastTransactionData)
    {
      if (onlySaveSelected.HasValue)
        OnlySaveSelected = onlySaveSelected.Value;
      return Save(transaction, needCheckDirty, firstTransactionData, lastTransactionData);
    }

    /// <summary>
    /// ����(�����ڳ־ò�ĳ�������)
    /// </summary>
    /// <param name="transaction">���ݿ�����, ���Ϊ��������������</param>
    /// <param name="lastTransactionData">����������ĩ�˵�ҵ�����</param>
    /// <returns>�ɹ��ύ��ҵ����󼯺�</returns>
    public T Save(DbTransaction transaction, params IBusiness[] lastTransactionData)
    {
      return Save(transaction, true, null as IBusiness[], lastTransactionData);
    }

    /// <summary>
    /// ����(�����ڳ־ò�ĳ�������)
    /// </summary>
    /// <param name="transaction">���ݿ�����, ���Ϊ��������������</param>
    /// <param name="firstTransactionData">����������ǰ�˵�ҵ�����</param>
    /// <param name="lastTransactionData">����������ĩ�˵�ҵ�����</param>
    /// <returns>�ɹ��ύ��ҵ����󼯺�</returns>
    public T Save(DbTransaction transaction, IBusiness[] firstTransactionData, IBusiness[] lastTransactionData)
    {
      return Save(transaction, true, firstTransactionData, lastTransactionData);
    }

    /// <summary>
    /// ����(�����ڳ־ò�ĳ�������)
    /// </summary>
    /// <param name="transaction">���ݿ�����, ���Ϊ��������������</param>
    /// <param name="needCheckDirty">У�����ݿ����������ص��ύ�ڼ��Ƿ񱻸��Ĺ�, һ�����ֽ�����: CheckDirtyException; ���ClassAttribute.AllowIgnoreCheckDirty = false��������Ч, �ض�����: CheckSaveException</param>
    /// <param name="lastTransactionData">����������ĩ�˵�ҵ�����</param>
    /// <returns>�ɹ��ύ��ҵ����󼯺�</returns>
    public T Save(DbTransaction transaction, bool needCheckDirty, params IBusiness[] lastTransactionData)
    {
      return Save(transaction, needCheckDirty, null as IBusiness[], lastTransactionData);
    }

    /// <summary>
    /// ����(�����ڳ־ò�ĳ�������)
    /// </summary>
    /// <param name="transaction">���ݿ�����, ���Ϊ��������������</param>
    /// <param name="needCheckDirty">У�����ݿ����������ص��ύ�ڼ��Ƿ񱻸��Ĺ�, һ�����ֽ�����: CheckDirtyException; ���ClassAttribute.AllowIgnoreCheckDirty = false��������Ч, �ض�����: CheckSaveException</param>
    /// <param name="firstTransactionData">����������ǰ�˵�ҵ�����</param>
    /// <param name="lastTransactionData">����������ĩ�˵�ҵ�����</param>
    /// <returns>�ɹ��ύ��ҵ����󼯺�</returns>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2200:RethrowToPreserveStackDetails")]
    public virtual T Save(DbTransaction transaction, bool needCheckDirty, IBusiness[] firstTransactionData, IBusiness[] lastTransactionData)
    {
      ////������־
      //if (AppConfig.Debugging)
      //  EventLog.Save(String.Format("{0}.{1}: EditLevel={2}", this.GetType().FullName, MethodBase.GetCurrentMethod().Name, EditLevel));
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
    /// ����(�����ڳ־ò�ĳ�������)
    /// </summary>
    /// <param name="transaction">���ݿ�����, ���Ϊ��������������</param>
    /// <param name="needCheckDirty">У�����ݿ����������ص��ύ�ڼ��Ƿ񱻸��Ĺ�, һ�����ֽ�����: CheckDirtyException; ���ClassAttribute.AllowIgnoreCheckDirty = false��������Ч, �ض�����: CheckSaveException</param>
    /// <param name="firstTransactionData">����������ǰ�˵�ҵ�����</param>
    /// <param name="lastTransactionData">����������ĩ�˵�ҵ�����</param>
    /// <returns>�ɹ��ύ��ҵ�����</returns>
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

      T result = EnsembleOnSaving ? Clone() : this.Filter(p => p.IsDirty || p.IsDeleted || p.IsNew); //GetAllDirty((IRefinedlyObject)null) as T ?? MemberwiseClone(null) as T;
      result.DeletedList.AddRange(this.DeletedList);

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
    /// ����ҵ����󼯺�֮ǰ
    /// ��ִ��Save()�ĳ������ﱻ����
    /// </summary>
    /// <returns>�Ƿ����, ȱʡΪ true</returns>
    protected virtual bool OnSavingSelf()
    {
      return true;
    }

    /// <summary>
    /// ����ҵ����󼯺�֮��
    /// ��ִ��Save()�ĳ������ﱻ����
    /// </summary>
    /// <param name="ex">������Ϣ</param>
    /// <returns>��������ʱ���Ѻ���ʾ��Ϣ, ȱʡΪ null ֱ���׳�ԭ�쳣��������׳�SaveException���������Ϣ��ԭ�쳣</returns>
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
          Mapper.SetSelectCommand(command, criterions ?? new Criterions(typeof(T)));
        FetchSelf(command);
      }
      OnFetchedSelf(connection, criterions);
    }
    void IRefinedly.ExecuteFetchSelf(DbConnection connection, Criterions criterions)
    {
      bool oldRaiseListChangedEvents = RaiseListChangedEvents;
      try
      {
        RaiseListChangedEvents = false;
        SelfFetching = true;
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
        SelfFetching = false;
      }
      finally
      {
        RaiseListChangedEvents = oldRaiseListChangedEvents;
      }
    }

    private void ExecuteFetchSelf(DbTransaction transaction, Criterions criterions)
    {
      using (DbCommand command = DbCommandHelper.CreateCommand(transaction))
      {
        if (FetchTimeout.HasValue && FetchTimeout.Value >= 0)
          command.CommandTimeout = FetchTimeout.Value;
        OnFetchingSelf(transaction, command, criterions);
        if (String.IsNullOrEmpty(command.CommandText))
          Mapper.SetSelectCommand(command, criterions ?? new Criterions(typeof(T)));
        FetchSelf(command);
      }
      OnFetchedSelf(transaction, criterions);
    }
    void IRefinedly.ExecuteFetchSelf(DbTransaction transaction, Criterions criterions)
    {
      DbTransaction = transaction;

      bool oldRaiseListChangedEvents = RaiseListChangedEvents;
      try
      {
        RaiseListChangedEvents = false;
        SelfFetching = true;
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
        SelfFetching = false;
      }
      finally
      {
        RaiseListChangedEvents = oldRaiseListChangedEvents;
      }
    }

    /// <summary>
    /// ������
    /// </summary>
    [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
    protected override void DataPortal_Fetch(object criteria)
    {
      Criterions criterions = criteria as Criterions;
      if (criterions != null)
        DbConnectionHelper.Execute(criterions.DataSourceKey ?? DataSourceKey, (Action<DbConnection, Criterions>)((IRefinedly)this).ExecuteFetchSelf, criterions);
      else if (criteria != null && String.CompareOrdinal(criteria.GetType().FullName, ItemValueType.FullName) == 0)
        DbConnectionHelper.Execute(DataSourceKey, (Action<DbConnection, Criterions>)((IRefinedly)this).ExecuteFetchSelf, new Criterions(this.GetType(), (TBusiness)criteria));
      else
        DoFetchSelf(criteria);
    }

    /// <summary>
    /// �Զ��幹��ҵ����󼯺�
    /// </summary>
    /// <param name="criteria">�Զ�������</param>
    protected virtual void DoFetchSelf(object criteria)
    {
      NotSupportedException ex = new NotSupportedException(Phenix.Core.Properties.Resources.MethodNotImplemented);
      EventLog.Save(this.GetType(), MethodBase.GetCurrentMethod(), ex);
      throw ex;
    }

    /// <summary>
    /// �ύ����
    /// </summary>
    [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
    protected override void DataPortal_Update()
    {
      DbConnectionHelper.Execute(DataSourceKey, ExecuteSave);
    }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
    private void ExecuteSave(DbTransaction transaction)
    {
      List<IRefinedlyObject> ignoreLinks = new List<IRefinedlyObject>();
      if (AloneTransaction)
      {
        List<ExceptionEventArgs> saveErrors = new List<ExceptionEventArgs>();
        if (_firstTransactionData != null)
          foreach (IRefinedly item in _firstTransactionData)
            if (item != null)
              try
              {
                item.SaveSelf(null, ref ignoreLinks);
              }
              catch (Exception ex)
              {
                saveErrors.Add(new ExceptionEventArgs(item, ex));
              }
        try
        {
          SaveSelf(transaction, ref ignoreLinks);
        }
        catch (SaveException ex)
        {
          saveErrors.AddRange(ex.SaveErrors);
        }
        catch (Exception ex)
        {
          saveErrors.Add(new ExceptionEventArgs(this, ex));
        }
        if (_lastTransactionData != null)
          foreach (IRefinedly item in _lastTransactionData)
            if (item != null)
              try
              {
                item.SaveSelf(null, ref ignoreLinks);
              }
              catch (Exception ex)
              {
                saveErrors.Add(new ExceptionEventArgs(item, ex));
              }
        if (saveErrors.Count > 0)
          throw new SaveException(saveErrors);
      }
      else
      {
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
    }

    private void SaveSelf(DbTransaction transaction, ref List<IRefinedlyObject> ignoreLinks)
    {
      ignoreLinks = ExecuteSaveSelf(transaction, ignoreLinks);
      CascadingAggregate(transaction);
    }
    void IRefinedly.SaveSelf(DbTransaction transaction, ref List<IRefinedlyObject> ignoreLinks)
    {
      SaveSelf(transaction, ref ignoreLinks);
    }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
    private List<IRefinedlyObject> ExecuteSaveSelf(DbTransaction transaction, List<IRefinedlyObject> ignoreLinks)
    {
      if (!CascadingSave)
        return ignoreLinks;

      //��ɾ���������Ϊ�˷�ֹΨһ����ͻ
      if (AloneTransaction)
      {
        DbTransaction = transaction;

        OnSavingSelf(transaction);
        List<ExceptionEventArgs> saveErrors = new List<ExceptionEventArgs>();
        foreach (TBusiness item in DeletedList)
          try
          {
            item.SaveSelf(null, ref ignoreLinks);
          }
          catch (Exception ex)
          {
            saveErrors.Add(new ExceptionEventArgs(item, ex));
          }
        foreach (TBusiness item in this)
          try
          {
            item.SaveSelf(null, ref ignoreLinks);
          }
          catch (Exception ex)
          {
            saveErrors.Add(new ExceptionEventArgs(item, ex));
          }
        OnSavedSelf(transaction);
        if (saveErrors.Count > 0)
          throw new SaveException(saveErrors);
      }
      else
      {
        if (transaction == null)
          return DbConnectionHelper.ExecuteGet(DataSourceKey, ExecuteSaveSelf, ignoreLinks);

        DbTransaction = transaction;

        OnSavingSelf(transaction);
        foreach (TBusiness item in DeletedList)
          item.SaveSelf(transaction, ref ignoreLinks);
        foreach (TBusiness item in this)
          item.SaveSelf(transaction, ref ignoreLinks);
        OnSavedSelf(transaction);
      }
      return ignoreLinks;
    }

    private void CascadingAggregate(DbTransaction transaction)
    {
      IBusinessObject masterBusiness = MasterBusiness;
      if (masterBusiness != null)
      {
        if (!masterBusiness.IsDeleted)
        {
          foreach (FieldAggregateMapInfo item in ClassMemberHelper.GetFieldAggregateMapInfos(ItemValueType, false))
            item.Compute(transaction, masterBusiness, true);
          ((IRefinedly)masterBusiness).CascadingAggregate(transaction);
        }
      }
      else
      {
        foreach (TBusiness item in this)
          item.CascadingAggregate(transaction);
      }
    }
    void IRefinedly.CascadingAggregate(DbTransaction transaction)
    {
      CascadingAggregate(transaction);
    }

    private bool DeleteSelf(DbTransaction transaction, IRefinedlyObject masterBusiness, ref List<IRefinedlyObject> ignoreLinks)
    {
      if (!CascadingSave)
        return false;
      if (CascadingDelete && MasterBusiness == masterBusiness)
      {
        foreach (TBusiness item in DeletedList)
          item.DeleteDetails(transaction, ref ignoreLinks);
        foreach (TBusiness item in this)
          item.DeleteDetails(transaction, ref ignoreLinks);
        if (!BusinessBase<TBusiness>.DeletedAsDisabled)
          DoDeleteSelf(transaction);
        return true;
      }
      else
      {
        SaveSelf(transaction, ref ignoreLinks);
        return false;
      }
    }
    bool IRefinedlyCollection.DeleteSelf(DbTransaction transaction, IRefinedlyObject masterBusiness, ref List<IRefinedlyObject> ignoreLinks)
    {
      return DeleteSelf(transaction, masterBusiness, ref ignoreLinks);
    }

    /// <summary>
    /// ������ҵ����󼯺�֮ǰ(�����ڳ־ò�ĳ�������)
    /// </summary>
    /// <param name="connection">���ݿ�����</param>
    /// <param name="command">DbCommand</param>
    /// <param name="criterions">������</param>
    protected virtual void OnFetchingSelf(DbConnection connection, DbCommand command, Criterions criterions)
    {
    }

    /// <summary>
    /// ������ҵ����󼯺�֮ǰ(�����ڳ־ò�ĳ�������)
    /// </summary>
    /// <param name="transaction">���ݿ�����</param>
    /// <param name="command">DbCommand</param>
    /// <param name="criterions">������</param>
    protected virtual void OnFetchingSelf(DbTransaction transaction, DbCommand command, Criterions criterions)
    {
    }

    /// <summary>
    /// ������ҵ����󼯺�֮��(�����ڳ־ò�ĳ�������)
    /// </summary>
    /// <param name="connection">���ݿ�����</param>
    /// <param name="criterions">������</param>
    protected virtual void OnFetchedSelf(DbConnection connection, Criterions criterions)
    {
    }

    /// <summary>
    /// ������ҵ����󼯺�֮��(�����ڳ־ò�ĳ�������)
    /// </summary>
    /// <param name="transaction">���ݿ�����</param>
    /// <param name="criterions">������</param>
    protected virtual void OnFetchedSelf(DbTransaction transaction, Criterions criterions)
    {
    }

    /// <summary>
    /// ����(��ɾ��)ҵ����󼯺�֮ǰ(�����ڳ־ò�ĳ�������)
    /// </summary>
    /// <param name="transaction">���ݿ�����</param>
    protected virtual void OnSavingSelf(DbTransaction transaction)
    {
    }

    /// <summary>
    /// ����(��ɾ��)ҵ����󼯺�֮��(�����ڳ־ò�ĳ�������)
    /// </summary>
    /// <param name="transaction">���ݿ�����</param>
    protected virtual void OnSavedSelf(DbTransaction transaction)
    {
    }

    /// <summary>
    /// ����ɾ��ҵ���������֮ǰ(�����ڳ־ò�ĳ�������)
    /// </summary>
    /// <param name="transaction">���ݿ�����</param>
    /// <param name="limitingCriteriaExpressions">���Ʊ��������(not exists �������)</param>
    protected virtual void OnDeletingSelf(DbTransaction transaction, ref List<CriteriaExpression> limitingCriteriaExpressions)
    {
    }
    /// <summary>
    /// ɾ������������֮ǰ(�����ڳ־ò�ĳ�������)
    /// </summary>
    /// <param name="transaction">���ݿ�����</param>
    /// <param name="limitingCriteriaExpressions">���Ʊ��������(not exists �������)</param>
    void IBusiness.OnDeletingSelf(DbTransaction transaction, ref List<CriteriaExpression> limitingCriteriaExpressions)
    {
      OnDeletingSelf(transaction, ref limitingCriteriaExpressions);
    }

    /// <summary>
    /// ����ɾ��ҵ���������֮��(�����ڳ־ò�ĳ�������)
    /// </summary>
    /// <param name="transaction">���ݿ�����</param>
    protected virtual void OnDeletedSelf(DbTransaction transaction)
    {
    }
    /// <summary>
    /// ɾ������������֮��(�����ڳ־ò�ĳ�������)
    /// </summary>
    /// <param name="transaction">���ݿ�����</param>
    void IBusiness.OnDeletedSelf(DbTransaction transaction)
    {
      OnDeletedSelf(transaction);
    }

    private void DoDeleteSelf(DbTransaction transaction)
    {
      try
      {
        List<CriteriaExpression> limitingCriteriaExpressions = null;
        OnDeletingSelf(transaction, ref limitingCriteriaExpressions);
        List<ICriterions> limitingConditions = Criterions.GetConditions(limitingCriteriaExpressions);
        try
        {
          ClassMapInfo.CascadingDeleteOrUnlinkDetail(transaction, MasterBusiness);
          if (Count == 0 && DeletedList.Count > 0 && Criterions.Criteria == null && Criterions.CriteriaExpression == null)
            using (DbCommand command = DbCommandHelper.CreateCommand(transaction))
            {
              Mapper.SetDeleteDetailCommand(command, Criterions, limitingConditions);
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
          OnDeletedSelf(transaction);
        }
      }
      catch (CheckSaveException)
      {
        throw;
      }
      catch (Exception ex)
      {
        throw new DeleteException(this, EntityHelper.CheckOccupied(MasterBusiness, ex));
      }
    }

    #endregion

    #region Validation Rules

    /// <summary>
    /// У���Ƿ�����ظ�����
    /// </summary>
    /// <returns>�ظ��Ķ���</returns>
    public IList<IEntity> CheckRepeated()
    {
      return DataHub.CheckRepeated(typeof(TBusiness), GetAllDirty(new List<IEntity>()));
    }

    #region GetAllDirty

    private List<IEntity> GetAllDirty(List<IEntity> entities)
    {
      foreach (TBusiness item in this)
        if (!OnlySaveSelected || item.Selected)
          entities = ((IRefinedly)item).GetAllDirty(entities);
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
      bool oldRaiseListChangedEvents = result.RaiseListChangedEvents;
      try
      {
        result.RaiseListChangedEvents = false;
        foreach (TBusiness item in this)
          if (!OnlySaveSelected || item.Selected)
          {
            TBusiness business = (TBusiness)((IRefinedly)item).GetAllDirty((IRefinedlyObject)null);
            if (business != null)
              result.Add(business);
          }
      }
      finally
      {
        result.RaiseListChangedEvents = oldRaiseListChangedEvents;
      }
      return result.DeletedList.Count > 0 || result.Count > 0 ? result : null;
    }
    IRefinedly IRefinedly.GetAllDirty(IRefinedlyObject masterBusiness)
    {
      return GetAllDirty(masterBusiness);
    }

    #endregion

    /// <summary>
    /// У�鱾�����ڵ�ҵ����������Ƿ���Ч
    /// onlyOldError = false
    /// onlySelfDirty = false
    /// throwIfException = false
    /// </summary>
    /// <returns>������Ϣ</returns>
    public IDataErrorInfo CheckSelfRules()
    {
      return CheckSelfRules(false, false, false);
    }

    /// <summary>
    /// У�鱾�����ڵ�ҵ����������Ƿ���Ч
    /// onlySelfDirty = false
    /// throwIfException = false
    /// </summary>
    /// <param name="onlyOldError">�����ԭ�д���</param>
    /// <returns>������Ϣ</returns>
    public IDataErrorInfo CheckSelfRules(bool onlyOldError)
    {
      return CheckSelfRules(onlyOldError, false, false);
    }

    /// <summary>
    /// У�鱾�����ڵ�ҵ����������Ƿ���Ч
    /// throwIfException = false
    /// </summary>
    /// <param name="onlyOldError">�����ԭ�д���</param>
    /// <param name="onlySelfDirty">�����������</param>
    /// <returns>������Ϣ</returns>
    public IDataErrorInfo CheckSelfRules(bool onlyOldError, bool onlySelfDirty)
    {
      return CheckSelfRules(onlyOldError, onlySelfDirty, false);
    }

    /// <summary>
    /// У�鱾�����ڵ�ҵ����������Ƿ���Ч
    /// </summary>
    /// <param name="onlyOldError">�����ԭ�д���</param>
    /// <param name="onlySelfDirty">�����������</param>
    /// <param name="throwIfException">���Ϊ true, �򵱷���!IsSelfValidʱ�׳�Csla.Rules.ValidationException�쳣</param>
    /// <returns>������Ϣ</returns>
    public IDataErrorInfo CheckSelfRules(bool onlyOldError, bool onlySelfDirty, bool throwIfException)
    {
      for (int i = Count - 1; i >= 0; i--)
      {
        IDataErrorInfo result = Items[i].CheckSelfRules(onlyOldError, onlySelfDirty, throwIfException);
        if (result != null)
          return result;
      }
      return null;
    }

    /// <summary>
    /// У�鱾�����ڵ�ҵ����������Ƿ���Ч(��ObjectRules)
    /// onlySelfDirty = false
    /// throwIfException = false
    /// </summary>
    /// <returns>������Ϣ</returns>
    public IDataErrorInfo CheckSelfObjectRules()
    {
      return CheckSelfObjectRules(false, false);
    }

    /// <summary>
    /// У�鱾�����ڵ�ҵ����������Ƿ���Ч(��ObjectRules)
    /// throwIfException = false
    /// </summary>
    /// <param name="onlySelfDirty">�����������</param>
    /// <returns>������Ϣ</returns>
    public IDataErrorInfo CheckSelfObjectRules(bool onlySelfDirty)
    {
      return CheckSelfObjectRules(onlySelfDirty, false);
    }

    /// <summary>
    /// У�鱾�����ڵ�ҵ����������Ƿ���Ч(��ObjectRules)
    /// </summary>
    /// <param name="onlySelfDirty">�����������</param>
    /// <param name="throwIfException">���Ϊ true, �򵱷���!IsSelfValidʱ�׳�Csla.Rules.ValidationException�쳣</param>
    /// <returns>������Ϣ</returns>
    public IDataErrorInfo CheckSelfObjectRules(bool onlySelfDirty, bool throwIfException)
    {
      for (int i = Count - 1; i >= 0; i--)
      {
        IDataErrorInfo result = Items[i].CheckSelfObjectRules(onlySelfDirty, throwIfException);
        if (result != null)
          return result;
      }
      return null;
    }

    /// <summary>
    /// У�鱾�����ڵ�ҵ��������ҵ����󼯺��������Ƿ���Ч
    /// onlyOldError = false
    /// onlySelfDirty = false
    /// </summary>
    /// <returns>������Ϣ</returns>
    public string CheckRules()
    {
      return CheckRules(false, false);
    }

    /// <summary>
    /// У�鱾�����ڵ�ҵ��������ҵ����󼯺��������Ƿ���Ч
    /// onlySelfDirty = false
    /// </summary>
    /// <returns>������Ϣ</returns>
    public string CheckRules(bool onlyOldError)
    {
      return CheckRules(onlyOldError, false);
    }

    /// <summary>
    /// У�鱾�����ڵ�ҵ��������ҵ����󼯺��������Ƿ���Ч
    /// </summary>
    /// <param name="onlyOldError">���ԭ�д���</param>
    /// <param name="onlySelfDirty">�����������</param>
    /// <returns>������Ϣ</returns>
    public string CheckRules(bool onlyOldError, bool onlySelfDirty)
    {
      List<IRefinedlyObject> ignoreLinks = new List<IRefinedlyObject>();
      return CheckRules(onlyOldError, onlySelfDirty, ref ignoreLinks);
    }

    private string CheckRules(bool onlyOldError, bool onlySelfDirty, ref List<IRefinedlyObject> ignoreLinks)
    {
      for (int i = Count - 1; i >= 0; i--)
      {
        string error = ((IRefinedly)Items[i]).CheckRules(onlyOldError, onlySelfDirty, ref ignoreLinks);
        if (!String.IsNullOrEmpty(error))
          return error;
      }
      return null;
    }
    string IRefinedly.CheckRules(bool onlyOldError, bool onlySelfDirty, ref List<IRefinedlyObject> ignoreLinks)
    {
      return CheckRules(onlyOldError, onlySelfDirty, ref ignoreLinks);
    }
    
    /// <summary>
    /// ������Ч����
    /// onlyOldError = false
    /// onlySelfDirty = false
    /// </summary>
    /// <returns>��Ч����</returns>
    public TBusiness FindInvalidItem()
    {
      return FindInvalidItem(false, false);
    }
    /// <summary>
    /// ������Ч����
    /// onlyOldError = false
    /// onlySelfDirty = false
    /// </summary>
    /// <returns>��Ч����</returns>
    IBusinessObject IBusinessCollection.FindInvalidItem()
    {
      return FindInvalidItem();
    }

    /// <summary>
    /// ������Ч����
    /// onlySelfDirty = false
    /// </summary>
    /// <param name="onlyOldError">�����ԭ�д���</param>
    /// <returns>��Ч����</returns>
    public TBusiness FindInvalidItem(bool onlyOldError)
    {
      return FindInvalidItem(onlyOldError, false); ;
    }
    /// <summary>
    /// ������Ч����
    /// onlySelfDirty = false
    /// </summary>
    /// <param name="onlyOldError">�����ԭ�д���</param>
    /// <returns>��Ч����</returns>
    IBusinessObject IBusinessCollection.FindInvalidItem(bool onlyOldError)
    {
      return FindInvalidItem(onlyOldError);
    }

    /// <summary>
    /// ������Ч����
    /// </summary>
    /// <param name="onlyOldError">�����ԭ�д���</param>
    /// <param name="onlySelfDirty">�����������</param>
    /// <returns>��Ч����</returns>
    public TBusiness FindInvalidItem(bool onlyOldError, bool onlySelfDirty)
    {
      for (int i = Count - 1; i >= 0; i--)
        if (Items[i].CheckSelfRules(onlyOldError, onlySelfDirty, false) != null)
          return Items[i];
      return null;
    }
    /// <summary>
    /// ������Ч����
    /// </summary>
    /// <param name="onlyOldError">�����ԭ�д���</param>
    /// <param name="onlySelfDirty">�����������</param>
    /// <returns>��Ч����</returns>
    IBusinessObject IBusinessCollection.FindInvalidItem(bool onlyOldError, bool onlySelfDirty)
    {
      return FindInvalidItem(onlyOldError, onlySelfDirty);
    }

    #endregion

    #region ��̬ˢ��

    private TBusiness FulfillFetch(object[] values)
    {
      TBusiness result = BusinessBase<TBusiness>.Fetch(values, RenovateFieldMapInfos);
      FillRange(new List<TBusiness>(1) { result });
      return result;
    }

    private string AssembleDataKey(IList<object> values)
    {
      StringBuilder result = new StringBuilder();
      for (int i = values.Count - RenovateDataKeyCount; i < values.Count; i++)
      {
        object value = values[i];
        result.Append(value != null ? value.ToString() : String.Empty);
        result.Append(AppConfig.VALUE_SEPARATOR);
      }
      return result.ToString();
    }

    /// <summary>
    /// ���
    /// </summary>
    protected override void ClearItems()
    {
      base.ClearItems();
      RenovateIndex.Clear();
    }

    #region IAnalyser ��Ա

    /// <summary>
    /// �������ݼ�������
    /// </summary>
    protected void AnalyseDataKeyInfo(int dataKeyCount)
    {
      RenovateDataKeyCount = dataKeyCount;
    }
    void IAnalyser.AnalyseDataKeyInfo(int dataKeyCount)
    {
      AnalyseDataKeyInfo(dataKeyCount);
    }

    /// <summary>
    /// �������ݼ�����
    /// </summary>
    protected virtual bool AnalyseDataInfo(DataTable data)
    {
      if (data == null)
      {
        EventLog.SaveLocal(MethodBase.GetCurrentMethod(), new ArgumentNullException("data"));
        return false;
      }

      RenovateFieldMapInfos = ClassMemberHelper.GetFieldMapInfos(ItemValueType, data);

      bool oldRaiseListChangedEvents = RaiseListChangedEvents;
      try
      {
        RaiseListChangedEvents = false;
        __ClearItems();
        foreach (DataRow row in data.Rows)
        {
          string key = AssembleDataKey(row.ItemArray);
          RenovateIndex[key] = FulfillFetch(row.ItemArray);
        }
        return true;
      }
      finally
      {
        RaiseListChangedEvents = oldRaiseListChangedEvents;
        if (RaiseListChangedEvents)
          OnListChanged(new ListChangedEventArgs(ListChangedType.Reset, -1));
      }
    }
    bool IAnalyser.AnalyseDataInfo(DataTable data)
    {
      return AnalyseDataInfo(data);
    }

    /// <summary>
    /// ������̬ˢ������
    /// </summary>
    protected virtual IEntity AnalyseRenovateInfo(ExecuteAction action, object[] values)
    {
      string key = AssembleDataKey(values);
      TBusiness result;
      if (RenovateIndex.TryGetValue(key, out result))
      {
        if (action == ExecuteAction.Delete)
        {
          RenovateIndex.Remove(key);
          ClearRemove(result);
        }
        else
        {
          result.FetchSelf(values, RenovateFieldMapInfos);
          result.ClearDetailCache();
          result.ClearLinkCache();
        }
      }
      else
      {
        result = FulfillFetch(values);
        RenovateIndex[key] = result;
      }
      return result;
    }
    IEntity IAnalyser.AnalyseRenovateInfo(ExecuteAction action, object[] values)
    {
      return AnalyseRenovateInfo(action, values);
    }

    /// <summary>
    /// ��������
    /// </summary>
    protected virtual void LoadData()
    {
      LazyFetch(true);
    }
    void IAnalyser.LoadData()
    {
      LoadData();
    }

    #endregion

    #endregion

    #endregion
  }
}