using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using Phenix.Core.Cache;
using Phenix.Core.Dictionary;
using Phenix.Core.Mapping;
using Phenix.Core.Reflection;
using Phenix.Core.Security;

namespace Phenix.Business
{
  /// <summary>
  /// 条件集
  /// </summary>
  [Serializable]
  public sealed class Criterions : ICriterions, IObjectCacheKey, IComparable, IComparable<Criterions>
  {
    #region 初始化

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
    [Newtonsoft.Json.JsonConstructor]
    private Criterions(string resultTypeAssemblyQualifiedName, bool? resultIsArray,
      ICriteria criteria, CriteriaExpression criteriaExpression,
      IBusinessObject masterBusiness, string groupName,
      bool cascadingSave, bool cascadingDelete,
      bool cacheEnabled,
      IList<string> sectionNames, bool includeDisabled,
      IBusinessObject itselfBusiness,
      bool needPage, int pageSize, int pageNo,
      IList<OrderByInfo> orderByInfos,
      object tag)
    {
      _resultTypeAssemblyQualifiedName = resultTypeAssemblyQualifiedName;
      _resultIsArray = resultIsArray;
      _criteria = criteria;
      _criteriaExpression = CriteriaExpression.Trim(criteriaExpression);
      _masterBusiness = masterBusiness;
      _groupName = groupName;
      _cascadingSave = cascadingSave;
      _cascadingDelete = cascadingDelete;
      _cacheEnabled = cacheEnabled;
      _sectionNames = sectionNames;
      _includeDisabled = includeDisabled;
      _itselfBusiness = itselfBusiness;
      _needPage = needPage;
      _pageSize = pageSize;
      _pageNo = pageNo;
      _orderByInfos = orderByInfos;
      _tag = tag;
    }

    /// <summary>
    /// 初始化
    /// criteria = null
    /// masterBusiness = null
    /// groupName = null
    /// cascadingSave = true
    /// cascadingDelete = true
    /// cacheEnabled = true
    /// includeDisabled = false
    /// itselfBusiness = null
    /// </summary>
    /// <param name="resultType">业务类</param>
    /// <param name="orderByInfos">数据排列顺序队列</param>
    public Criterions(Type resultType,
      params OrderByInfo[] orderByInfos)
      : this(resultType, null, null, null, 
      true, true,
      true, false,
      null,
      orderByInfos) { }

    /// <summary>
    /// 初始化
    /// criteria = null
    /// masterBusiness = null
    /// groupName = null
    /// cascadingSave = true
    /// cascadingDelete = true
    /// cacheEnabled = true
    /// includeDisabled = false
    /// itselfBusiness = null
    /// </summary>
    /// <param name="resultType">业务类</param>
    /// <param name="resultIsArray">返回对象是数组</param>
    /// <param name="orderByInfos">数据排列顺序队列</param>
    public Criterions(Type resultType, bool resultIsArray,
      params OrderByInfo[] orderByInfos)
      : this(resultType, null, null, null,
        true, true,
        true, false,
        null,
        orderByInfos)
    {
      _resultIsArray = resultIsArray;
    }

    /// <summary>
    /// 初始化
    /// criteria = null
    /// masterBusiness = null
    /// groupName = null
    /// cascadingSave = true
    /// cascadingDelete = true
    /// cacheEnabled = itselfBusiness == null
    /// includeDisabled = false
    /// </summary>
    /// <param name="resultType">业务类</param>
    /// <param name="itselfBusiness">自业务对象</param>
    public Criterions(Type resultType,
      IBusinessObject itselfBusiness)
      : this(resultType, null, null, null,
      true, true,
      itselfBusiness == null, false,
      itselfBusiness) { }

    /// <summary>
    /// 初始化
    /// criteria = null
    /// masterBusiness = null
    /// groupName = null
    /// cascadingSave = true
    /// cascadingDelete = true
    /// itselfBusiness = null
    /// </summary>
    /// <param name="resultType">业务类</param>
    /// <param name="cacheEnabled">需要缓存对象?</param>
    /// <param name="includeDisabled">是否包含禁用记录?</param>
    /// <param name="orderByInfos">数据排列顺序队列</param>
    public Criterions(Type resultType,
      bool cacheEnabled, bool includeDisabled,
      params OrderByInfo[] orderByInfos)
      : this(resultType, null, null, null,
      true, true, 
      cacheEnabled, includeDisabled,
      null,
      orderByInfos) { }

    /// <summary>
    /// 初始化
    /// criteria = null
    /// masterBusiness = null
    /// groupName = null
    /// cascadingSave = true
    /// cascadingDelete = true
    /// </summary>
    /// <param name="resultType">业务类</param>
    /// <param name="cacheEnabled">需要缓存对象?</param>
    /// <param name="includeDisabled">是否包含禁用记录?</param>
    /// <param name="itselfBusiness">自业务对象</param>
    public Criterions(Type resultType, 
      bool cacheEnabled, bool includeDisabled,
      IBusinessObject itselfBusiness)
      : this(resultType, null, null, null,
      true, true, 
      cacheEnabled, includeDisabled,
      itselfBusiness) { }

    /// <summary>
    /// 初始化
    /// masterBusiness = null
    /// groupName = null
    /// cascadingSave = true
    /// cascadingDelete = true
    /// cacheEnabled = criteria == null
    /// includeDisabled = false
    /// itselfBusiness = null
    /// </summary>
    /// <param name="resultType">业务类</param>
    /// <param name="criteria">条件对象</param>
    /// <param name="orderByInfos">数据排列顺序队列</param>
    public Criterions(Type resultType, ICriteria criteria,
      params OrderByInfo[] orderByInfos)
      : this(resultType, criteria, null, null, 
      true, true,
      criteria == null, false,
      null,
      orderByInfos) { }

    /// <summary>
    /// 初始化
    /// masterBusiness = null
    /// groupName = null
    /// cascadingSave = true
    /// cascadingDelete = true
    /// cacheEnabled = criteria == null
    /// includeDisabled = false
    /// itselfBusiness = null
    /// </summary>
    /// <param name="resultType">业务类</param>
    /// <param name="resultIsArray">返回对象是数组</param>
    /// <param name="criteria">条件对象</param>
    /// <param name="orderByInfos">数据排列顺序队列</param>
    public Criterions(Type resultType, bool resultIsArray, ICriteria criteria,
      params OrderByInfo[] orderByInfos)
      : this(resultType, criteria, null, null,
        true, true,
        criteria == null, false,
        null,
        orderByInfos)
    {
      _resultIsArray = resultIsArray;
    }

    /// <summary>
    /// 初始化
    /// masterBusiness = null
    /// groupName = null
    /// cascadingSave = true
    /// cascadingDelete = true
    /// includeDisabled = false
    /// itselfBusiness = null
    /// </summary>
    /// <param name="resultType">业务类</param>
    /// <param name="criteria">条件对象</param>
    /// <param name="cacheEnabled">是否需要缓存对象?</param>
    /// <param name="orderByInfos">数据排列顺序队列</param>
    public Criterions(Type resultType, ICriteria criteria,
      bool cacheEnabled,
      params OrderByInfo[] orderByInfos)
      : this(resultType, criteria, null, null, 
      true, true,
      cacheEnabled, false,
      null,
      orderByInfos) { }

    /// <summary>
    /// 初始化
    /// masterBusiness = null
    /// groupName = null
    /// cascadingSave = true
    /// cascadingDelete = true
    /// includeDisabled = false
    /// </summary>
    /// <param name="resultType">业务类</param>
    /// <param name="criteria">条件对象</param>
    /// <param name="cacheEnabled">是否需要缓存对象?</param>
    /// <param name="itselfBusiness">自业务对象</param>
    public Criterions(Type resultType, ICriteria criteria,
      bool cacheEnabled,
      IBusinessObject itselfBusiness)
      : this(resultType, criteria, null, null,
      true, true,
      cacheEnabled, false,
      itselfBusiness) { }

    /// <summary>
    /// 初始化
    /// criteria = null
    /// cascadingSave = true
    /// cascadingDelete = true
    /// cacheEnabled = masterBusiness == null 且 String.IsNullOrEmpty(groupName)
    /// includeDisabled = false
    /// itselfBusiness = null
    /// </summary>
    /// <param name="resultType">业务类</param>
    /// <param name="masterBusiness">主业务对象</param>
    /// <param name="groupName">分组名</param>
    /// <param name="orderByInfos">数据排列顺序队列</param>
    public Criterions(Type resultType, IBusinessObject masterBusiness, string groupName,
      params OrderByInfo[] orderByInfos)
      : this(resultType, null, masterBusiness, groupName, 
      true, true,
      masterBusiness == null && String.IsNullOrEmpty(groupName), false,
      null,
      orderByInfos) { }

    /// <summary>
    /// 初始化
    /// criteria = null
    /// cacheEnabled = masterBusiness == null 且 String.IsNullOrEmpty(groupName)
    /// includeDisabled = false
    /// itselfBusiness = null
    /// </summary>
    /// <param name="resultType">业务类</param>
    /// <param name="masterBusiness">主业务对象</param>
    /// <param name="groupName">分组名</param>
    /// <param name="cascadingSave">是否级联保存?</param>
    /// <param name="cascadingDelete">是否级联删除?</param>
    /// <param name="orderByInfos">数据排列顺序队列</param>
    public Criterions(Type resultType, IBusinessObject masterBusiness, string groupName,
      bool cascadingSave, bool cascadingDelete,
      params OrderByInfo[] orderByInfos)
      : this(resultType, null, masterBusiness, groupName,
      cascadingSave, cascadingDelete,
      masterBusiness == null && String.IsNullOrEmpty(groupName), false,
      null,
      orderByInfos) { }

    /// <summary>
    /// 初始化
    /// cascadingSave = true
    /// cascadingDelete = true
    /// cacheEnabled = criteria == null 且 masterBusiness == null 且 String.IsNullOrEmpty(groupName)
    /// includeDisabled = false
    /// itselfBusiness = null
    /// </summary>
    /// <param name="resultType">业务类</param>
    /// <param name="criteria">条件对象</param>
    /// <param name="masterBusiness">主业务对象</param>
    /// <param name="groupName">分组名</param>
    /// <param name="orderByInfos">数据排列顺序队列</param>
    public Criterions(Type resultType, ICriteria criteria, IBusinessObject masterBusiness, string groupName,
      params OrderByInfo[] orderByInfos)
      : this(resultType, criteria, masterBusiness, groupName,
      true, true,
      criteria == null && masterBusiness == null && String.IsNullOrEmpty(groupName), false,
      null,
      orderByInfos) { }

    /// <summary>
    /// 初始化
    /// cacheEnabled = criteria == null 且 masterBusiness == null 且 String.IsNullOrEmpty(groupName)
    /// includeDisabled = false
    /// itselfBusiness = null
    /// </summary>
    /// <param name="resultType">业务类</param>
    /// <param name="criteria">条件对象</param>
    /// <param name="masterBusiness">主业务对象</param>
    /// <param name="groupName">分组名</param>
    /// <param name="cascadingSave">是否级联保存?</param>
    /// <param name="cascadingDelete">是否级联删除?</param>
    /// <param name="orderByInfos">数据排列顺序队列</param>
    public Criterions(Type resultType, ICriteria criteria, IBusinessObject masterBusiness, string groupName,
      bool cascadingSave, bool cascadingDelete,
      params OrderByInfo[] orderByInfos)
      : this(resultType, criteria, masterBusiness, groupName,
      cascadingSave, cascadingDelete,
      criteria == null && masterBusiness == null && String.IsNullOrEmpty(groupName), false,
      null,
      orderByInfos) { }

    private Criterions(Type resultType, ICriteria criteria, IBusinessObject masterBusiness, string groupName,
      bool cascadingSave, bool cascadingDelete,
      bool cacheEnabled, bool includeDisabled,
      IBusinessObject itselfBusiness,
      params OrderByInfo[] orderByInfos)
    {
      _resultTypeAssemblyQualifiedName = resultType != null ? resultType.AssemblyQualifiedName : null;
      _resultType = resultType;
      _criteria = criteria;
      _masterBusiness = masterBusiness;
      _groupName = groupName;
      _cascadingSave = cascadingSave;
      _cascadingDelete = cascadingDelete;
      _cacheEnabled = cacheEnabled;
      _includeDisabled = includeDisabled;
      _itselfBusiness = itselfBusiness;
      _orderByInfos = orderByInfos;

      _sectionNames = UserIdentity.CurrentIdentity != null ? UserIdentity.CurrentIdentity.GetSectionNames(resultType, criteria != null ? criteria.GetType() : null) : null;
    }

    /// <summary>
    /// 初始化
    /// masterBusiness = null
    /// groupName = null
    /// cascadingSave = true
    /// cascadingDelete = true
    /// cacheEnabled = criteriaExpression == null
    /// includeDisabled = false
    /// </summary>
    /// <param name="resultType">业务类</param>
    /// <param name="criteriaExpression">条件表达式</param>
    /// <param name="orderByInfos">数据排列顺序队列</param>
    public Criterions(Type resultType, CriteriaExpression criteriaExpression,
      params OrderByInfo[] orderByInfos)
      : this(resultType, criteriaExpression, null, null, 
      true, true,
      criteriaExpression == null, false,
      orderByInfos) { }
    
    /// <summary>
    /// 初始化
    /// masterBusiness = null
    /// groupName = null
    /// cascadingSave = true
    /// cascadingDelete = true
    /// cacheEnabled = criteriaExpression == null
    /// includeDisabled = false
    /// </summary>
    /// <param name="resultType">业务类</param>
    /// <param name="resultIsArray">返回对象是数组</param>
    /// <param name="criteriaExpression">条件表达式</param>
    /// <param name="orderByInfos">数据排列顺序队列</param>
    public Criterions(Type resultType, bool resultIsArray, CriteriaExpression criteriaExpression,
      params OrderByInfo[] orderByInfos)
      : this(resultType, criteriaExpression, null, null,
        true, true,
        criteriaExpression == null, false,
        orderByInfos)
    {
      _resultIsArray = resultIsArray;
    }

    /// <summary>
    /// 初始化
    /// masterBusiness = null
    /// groupName = null
    /// cascadingSave = true
    /// cascadingDelete = true
    /// includeDisabled = false
    /// </summary>
    /// <param name="resultType">业务类</param>
    /// <param name="criteriaExpression">条件表达式</param>
    /// <param name="cacheEnabled">可以缓存对象?</param>
    /// <param name="orderByInfos">数据排列顺序队列</param>
    public Criterions(Type resultType, CriteriaExpression criteriaExpression,
      bool cacheEnabled,
      params OrderByInfo[] orderByInfos)
      : this(resultType, criteriaExpression, null, null,
      true, true,
      cacheEnabled, false,
      orderByInfos) { }

    /// <summary>
    /// 初始化
    /// cascadingSave = true
    /// cascadingDelete = true
    /// cacheEnabled = criteriaExpression == null 且 masterBusiness == null 且 String.IsNullOrEmpty(groupName)
    /// includeDisabled = false
    /// </summary>
    /// <param name="resultType">业务类</param>
    /// <param name="criteriaExpression">条件表达式</param>
    /// <param name="masterBusiness">主业务对象</param>
    /// <param name="groupName">分组名</param>
    /// <param name="orderByInfos">数据排列顺序队列</param>
    public Criterions(Type resultType, CriteriaExpression criteriaExpression, IBusinessObject masterBusiness, string groupName,
      params OrderByInfo[] orderByInfos)
      : this(resultType, criteriaExpression, masterBusiness, groupName,
      true, true,
      criteriaExpression == null && masterBusiness == null && String.IsNullOrEmpty(groupName), false,
      orderByInfos) { }

    /// <summary>
    /// 初始化
    /// cacheEnabled = criteriaExpression == null 且 masterBusiness == null 且 String.IsNullOrEmpty(groupName)
    /// includeDisabled = false
    /// </summary>
    /// <param name="resultType">业务类</param>
    /// <param name="criteriaExpression">条件表达式</param>
    /// <param name="masterBusiness">主业务对象</param>
    /// <param name="groupName">分组名</param>
    /// <param name="cascadingSave">是否级联保存?</param>
    /// <param name="cascadingDelete">是否级联删除?</param>
    /// <param name="orderByInfos">数据排列顺序队列</param>
    public Criterions(Type resultType, CriteriaExpression criteriaExpression, IBusinessObject masterBusiness, string groupName,
      bool cascadingSave, bool cascadingDelete,
      params OrderByInfo[] orderByInfos)
      : this(resultType, criteriaExpression, masterBusiness, groupName, 
      cascadingSave, cascadingDelete,
      criteriaExpression == null && masterBusiness == null && String.IsNullOrEmpty(groupName), false,
      orderByInfos) { }

    private Criterions(Type resultType, CriteriaExpression criteriaExpression, IBusinessObject masterBusiness, string groupName,
      bool cascadingSave, bool cascadingDelete,
      bool cacheEnabled, bool includeDisabled,
      params OrderByInfo[] orderByInfos)
    {
      _resultTypeAssemblyQualifiedName = resultType != null ? resultType.AssemblyQualifiedName : null;
      _resultType = resultType;
      _criteriaExpression = CriteriaExpression.Trim(criteriaExpression);
      _masterBusiness = masterBusiness;
      _groupName = groupName;
      _cascadingSave = cascadingSave;
      _cascadingDelete = cascadingDelete;
      _cacheEnabled = cacheEnabled;
      _includeDisabled = includeDisabled;
      _orderByInfos = orderByInfos;

      _sectionNames = UserIdentity.CurrentIdentity != null ? UserIdentity.CurrentIdentity.GetSectionNames(resultType) : null;
    }

    /// <summary>
    /// 初始化
    /// includeDisabled = false
    /// </summary>
    /// <param name="resultType">业务类</param>
    /// <param name="criteriaExpression">条件表达式</param>
    /// <param name="cacheEnabled">是否需要缓存对象?</param>
    /// <param name="itselfBusiness">自业务对象</param>
    public Criterions(Type resultType, CriteriaExpression criteriaExpression,
      bool cacheEnabled,
      IBusinessObject itselfBusiness)
      : this(resultType, criteriaExpression,
      cacheEnabled, false,
      itselfBusiness) { }

    private Criterions(Type resultType, CriteriaExpression criteriaExpression,
      bool cacheEnabled, bool includeDisabled,
      IBusinessObject itselfBusiness)
    {
      _resultTypeAssemblyQualifiedName = resultType != null ? resultType.AssemblyQualifiedName : null;
      _resultType = resultType;
      _criteriaExpression = CriteriaExpression.Trim(criteriaExpression);
      _cacheEnabled = cacheEnabled;
      _includeDisabled = includeDisabled;
      _itselfBusiness = itselfBusiness;

      _sectionNames = UserIdentity.CurrentIdentity != null ? UserIdentity.CurrentIdentity.GetSectionNames(resultType) : null;
    }

    internal Criterions(Type resultType, Expression expression, IBusinessObject masterBusiness, string groupName,
      bool cascadingSave, bool cascadingDelete)
      : this(resultType, null,
      masterBusiness, groupName, cascadingSave, cascadingDelete,
      expression == null && masterBusiness == null && String.IsNullOrEmpty(groupName), false)
    {
      _expression = expression;
    }

    #endregion

    #region 属性

    /// <summary>
    /// 数据源键
    /// </summary>
    [Newtonsoft.Json.JsonIgnore]
    public string DataSourceKey
    {
      get
      {
        if (MasterBusiness != null && !object.ReferenceEquals(MasterBusiness.Criterions, this))
          return MasterBusiness.DataSourceKey;
        if (ItselfBusiness != null && !object.ReferenceEquals(ItselfBusiness.Criterions, this))
          return ItselfBusiness.DataSourceKey;
        if (Criteria != null && Criteria.DataSourceKey != null)
          return Criteria.DataSourceKey;
        if (CriteriaExpression != null && CriteriaExpression.DataSourceKey != null)
          return CriteriaExpression.DataSourceKey;
        return ClassMemberHelper.GetDataSourceKey(ResultType);
      }
    }
    
    private readonly string _resultTypeAssemblyQualifiedName;
    /// <summary>
    /// 返回对象类型程序集限定名
    /// </summary>
    public string ResultTypeAssemblyQualifiedName
    {
      get { return _resultTypeAssemblyQualifiedName; }
    }

    [NonSerialized]
    private Type _resultType;
    /// <summary>
    /// 返回对象类型
    /// </summary>
    [Newtonsoft.Json.JsonIgnore]
    public Type ResultType
    {
      get
      {
        if (_resultType == null)
        {
          if (_resultTypeAssemblyQualifiedName != null)
            _resultType = Type.GetType(_resultTypeAssemblyQualifiedName);
        }
        return _resultType;
      }
    }

    [NonSerialized]
    private Type _resultCoreType;
    /// <summary>
    /// 返回对象核心类型
    /// </summary>
    [Newtonsoft.Json.JsonIgnore]
    public Type ResultCoreType
    {
      get
      {
        if (_resultCoreType == null)
          _resultCoreType = Utilities.GetCoreType(ResultType);
        return _resultCoreType;
      }
    }

    private bool? _resultIsArray;
    /// <summary>
    /// 返回对象是数组
    /// </summary>
    public bool ResultIsArray
    {
      get
      {
        if (!_resultIsArray.HasValue)
          _resultIsArray = ResultType != ResultCoreType || MasterBusiness != null;
        return _resultIsArray.Value;
      }
    }

    private readonly ICriteria _criteria;
    /// <summary>
    /// 条件对象
    /// </summary>
    public ICriteria Criteria
    {
      get { return _criteria; }
    }

    private CriteriaExpression _criteriaExpression;
    /// <summary>
    /// 条件表达式
    /// </summary>
    public CriteriaExpression CriteriaExpression
    {
      get { return _criteriaExpression; }
      internal set { _criteriaExpression = CriteriaExpression.Trim(value); }
    }

    [NonSerialized]
    private readonly Expression _expression;

    private IBusinessObject _masterBusiness;
    /// <summary>
    /// 主业务对象
    /// </summary>
    public IBusinessObject MasterBusiness
    {
      get { return _masterBusiness; }
    }
    object ICriterions.Master
    {
      get { return MasterBusiness; }
    }

    private string _groupName;
    /// <summary>
    /// 分组名
    /// 用于区分子表中存在多组外键关联主表的情况
    /// null代表全部
    /// </summary>
    public string GroupName
    {
      get { return _groupName; }
    }

    private readonly bool _cascadingSave = true;
    /// <summary>
    /// 是否级联Save?
    /// 缺省为 true
    /// </summary>
    public bool CascadingSave
    {
      get { return _cascadingSave; }
    }

    private bool _cascadingDelete = true;
    /// <summary>
    /// 是否级联Delete?
    /// 缺省为 true, 如果 CascadingSave = false 则忽略本属性值
    /// </summary>
    public bool CascadingDelete
    {
      get { return _cascadingDelete; }
    }

    #region IObjectCacheKey 成员

    private readonly bool _cacheEnabled;
    /// <summary>
    /// 可以缓存对象?
    /// </summary>
    public bool CacheEnabled
    {
      get { return _cacheEnabled && !NeedPage; }
    }

    /// <summary>
    /// 可以极端缓存对象?
    /// </summary>
    [Newtonsoft.Json.JsonIgnore]
    public bool ExtremelyCacheEnabled
    {
      get
      {
        return CacheEnabled && Criteria == null && CriteriaExpression == null &&
          MasterBusiness == null && String.IsNullOrEmpty(GroupName) && !IncludeDisabled &&
          (OrderByInfos == null || OrderByInfos.Count == 0) &&
          SectionInfo.GetSectionNames(ResultCoreType).Count == 0;
      }
    }

    #endregion

    private readonly IList<string> _sectionNames;
    /// <summary>
    /// 切片名队列
    /// </summary>
    public IList<string> SectionNames
    {
      get { return _sectionNames; }
    }

    private readonly bool _includeDisabled;
    /// <summary>
    /// 是否包含禁用记录
    /// </summary>
    public bool IncludeDisabled
    {
      get
      {
        if (_includeDisabled)
          return true;
        if (ItselfBusiness != null)
          return true;
        if (Criteria != null && CriteriaFieldMapInfo.IncludeDisabled(ResultCoreType, Criteria))
          return true;
        return _includeDisabled;
      }
    }

    private IBusinessObject _itselfBusiness;
    /// <summary>
    /// 自业务对象
    /// </summary>
    public IBusinessObject ItselfBusiness
    {
      get { return _itselfBusiness; }
    }
    object ICriterions.Itself
    {
      get { return ItselfBusiness; }
    }

    private bool _needPage;
    /// <summary>
    /// 需要分页
    /// </summary>
    public bool NeedPage
    {
      get { return _needPage; }
    }

    private int? _pageSize;
    /// <summary>
    /// 分页大小
    /// 缺省为 1000
    /// </summary>
    public int PageSize
    {
      get { return _pageSize ?? 1000; }
      set
      {
        _needPage = true;
        _pageSize = value >= 0 ? value : (int?)null;
      }
    }

    private int? _pageNo;
    /// <summary>
    /// 分页号
    /// </summary>
    public int PageNo
    {
      get { return _pageNo ?? 1; }
      set
      {
        _needPage = true;
        _pageNo = value >= 0 ? value : (int?)null;
      }
    }

    private readonly IList<OrderByInfo> _orderByInfos;
    /// <summary>
    /// 数据排列顺序队列
    /// </summary>
    public IList<OrderByInfo> OrderByInfos
    {
      get { return _orderByInfos; }
    }

    private object _tag;
    /// <summary>
    /// 附属物
    /// </summary>
    public object Tag
    {
      get { return _tag; }
      set { _tag = value; }
    }

    [NonSerialized]
    private int? _hashCode;

    #endregion

    #region 方法

    /// <summary>
    /// 为Criteria检查业务规则
    /// </summary>
    /// <param name="throwIfException">如果为 true, 则抛出ValidationException异常</param>
    public string CheckRules(bool throwIfException)
    {
      if (Criteria != null)
        Criteria.CheckRules(throwIfException);
      return null;
    }

    internal Criterions ShallowCopy()
    {
      return (Criterions)MemberwiseClone();
    }

    internal Criterions PureClone()
    {
      Criterions result = (Criterions)MemberwiseClone();
      if (_masterBusiness != null)
        result._masterBusiness = _masterBusiness.PureClone();
      if (_itselfBusiness != null)
        result._itselfBusiness = _itselfBusiness.PureClone();
      result._hashCode = null;
      return result;
    }

    internal Criterions MemberwiseClone(IBusinessObject masterBusiness)
    {
      Criterions result = (Criterions)MemberwiseClone();
      if (masterBusiness != null)
        result._masterBusiness = masterBusiness;
      else if (_masterBusiness != null)
        result._masterBusiness = _masterBusiness.PureClone();
      result._hashCode = null;
      return result;
    }

    internal void SetLink(IBusinessObject masterBusiness, string groupName)
    {
      _masterBusiness = masterBusiness;
      _groupName = groupName;
      _hashCode = null;
    }

    internal void SetLink(IBusinessObject masterBusiness, bool? cascadingDelete)
    {
      _masterBusiness = masterBusiness;
      if (cascadingDelete.HasValue)
        _cascadingDelete = cascadingDelete.Value;
      _hashCode = null;
    }

    internal static List<ICriterions> GetConditions(List<CriteriaExpression> criteriaExpressions)
    {
      List<ICriterions> result = null;
      if (criteriaExpressions != null)
      {
        result = new List<ICriterions>(criteriaExpressions.Count);
        foreach (CriteriaExpression criteriaExpression in criteriaExpressions)
          result.Add(new Criterions(criteriaExpression.OwnerType, criteriaExpression));
      }
      return result;
    }

    #region IObjectCacheKey 成员

    /// <summary>
    /// 比较对象
    /// </summary>
    /// <param name="obj">对象</param>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
    public override bool Equals(object obj)
    {
      if (object.ReferenceEquals(obj, this))
        return true;
      Criterions other = obj as Criterions;
      if (object.ReferenceEquals(other, null))
        return false;
      if (String.CompareOrdinal(_resultTypeAssemblyQualifiedName, other._resultTypeAssemblyQualifiedName) != 0)
        return false;
      if (!CriteriaHelper.CompareCriteria(_criteria, other._criteria, ResultCoreType))
        return false;
      if (String.CompareOrdinal((object)_criteriaExpression != null ? _criteriaExpression.ToString() : null, (object)other._criteriaExpression != null ? other._criteriaExpression.ToString() : null) != 0)
        return false;
      if (String.CompareOrdinal((object)_expression != null ?_expression.ToString() : null, (object)other._expression != null ? other._expression.ToString() : null) != 0)
        return false;
      if (!object.Equals(_masterBusiness, other._masterBusiness))
        if (_masterBusiness != null && other._masterBusiness != null)
        {
          if (String.CompareOrdinal(_masterBusiness.PrimaryKey, other._masterBusiness.PrimaryKey) != 0)
            return false;
        }
        else
          return false;
      if (String.CompareOrdinal(_groupName, other._groupName) != 0)
        return false;
      if (_cascadingSave != other._cascadingSave)
        return false;
      if (_cascadingDelete != other._cascadingDelete)
        return false;
      if (_sectionNames != null || other._sectionNames != null)
      {
        if (_sectionNames == null || other._sectionNames == null)
          return false;
        if (_sectionNames.Count != other._sectionNames.Count)
          return false;
        foreach (string s1 in _sectionNames)
        {
          bool find = false;
          foreach (string s2 in other._sectionNames)
            if (String.CompareOrdinal(s1, s2) == 0)
            {
              find = true;
              break;
            }
          if (!find)
            return false;
        }
      }
      if (_includeDisabled != other._includeDisabled)
        return false;
      if (!object.Equals(_itselfBusiness, other._itselfBusiness))
        if (_itselfBusiness != null && other._itselfBusiness != null)
        {
          if (String.CompareOrdinal(_itselfBusiness.PrimaryKey, other._itselfBusiness.PrimaryKey) != 0)
            return false;
        }
        else
          return false;
      if (_orderByInfos != null || other._orderByInfos != null)
      {
        if (_orderByInfos == null || other._orderByInfos == null)
          return false;
        if (_orderByInfos.Count != other._orderByInfos.Count)
          return false;
        for (int i = 0; i < _orderByInfos.Count; i++)
        {
          if (String.CompareOrdinal(_orderByInfos[i].PropertyName, other._orderByInfos[i].PropertyName) != 0 ||
            _orderByInfos[i].OrderBy != other._orderByInfos[i].OrderBy)
            return false;
        }
      }
      return true;
    }

    #endregion

    #region IComparable 成员

    /// <summary>
    /// 比较对象
    /// </summary>
    public int CompareTo(object obj)
    {
      return CompareTo(obj as Criterions);
    }

    /// <summary>
    /// 比较对象
    /// </summary>
    public int CompareTo(Criterions other)
    {
      if (object.ReferenceEquals(other, this))
        return 0;
      if (object.ReferenceEquals(other, null))
        return 1;
      return String.Compare(ToString(), other.ToString(), StringComparison.Ordinal);
    }

    /// <summary>
    /// 比较对象
    /// </summary>
    public static int Compare(Criterions x, Criterions y)
    {
      if (object.ReferenceEquals(x, y))
        return 0;
      if (object.ReferenceEquals(x, null))
        return -1;
      return x.CompareTo(y);
    }

    /// <summary>
    /// 等于
    /// </summary>
    public static bool operator ==(Criterions left, Criterions right)
    {
      return Compare(left, right) == 0;
    }

    /// <summary>
    /// 不等于
    /// </summary>
    public static bool operator !=(Criterions left, Criterions right)
    {
      return Compare(left, right) != 0;
    }

    /// <summary>
    /// 小于
    /// </summary>
    public static bool operator <(Criterions left, Criterions right)
    {
      return Compare(left, right) < 0;
    }

    /// <summary>
    /// 大于
    /// </summary>
    public static bool operator >(Criterions left, Criterions right)
    {
      return Compare(left, right) > 0;
    }

    #endregion

    /// <summary>
    /// 取哈希值(注意字符串在32位和64位系统有不同的算法得到不同的结果) 
    /// </summary>
    public override int GetHashCode()
    {
      if (!_hashCode.HasValue)
      {
        int result =
          (!String.IsNullOrEmpty(_resultTypeAssemblyQualifiedName) ? _resultTypeAssemblyQualifiedName.GetHashCode() : 0) ^
          (_expression != null ? _expression.ToString().GetHashCode() : 0) ^
          //(_masterBusiness != null ? _masterBusiness.PrimaryKey.GetHashCode() : 0) ^
          (!String.IsNullOrEmpty(_groupName) ? _groupName.GetHashCode() : 0) ^
          _cascadingSave.GetHashCode() ^ _cascadingDelete.GetHashCode();
        if (_sectionNames != null)
          foreach (string s in _sectionNames)
            result = result ^ s.GetHashCode();
        result = result ^ _includeDisabled.GetHashCode();
        //result = result ^ (_itselfBusiness != null ? _itselfBusiness.PrimaryKey.GetHashCode() : 0);
        if (_orderByInfos != null)
          foreach (OrderByInfo item in _orderByInfos)
            result = result ^ item.GetHashCode();
        _hashCode = result;
      }
      return _hashCode.Value ^
        (_criteria != null ? CriteriaHelper.GetHashCode(_criteria, ResultCoreType) : 0) ^
        (_criteriaExpression != null ? _criteriaExpression.GetHashCode() : 0);
    }

    /// <summary>
    /// 字符串表示
    /// </summary>
    public override string ToString()
    {
      StringBuilder result = new StringBuilder();
      result.Append(_resultTypeAssemblyQualifiedName);
      result.Append(Phenix.Core.AppConfig.VALUE_SEPARATOR);
      if (_expression != null)
        result.Append(_expression.ToString());
      result.Append(Phenix.Core.AppConfig.VALUE_SEPARATOR);
      result.Append(_groupName);
      result.Append(Phenix.Core.AppConfig.VALUE_SEPARATOR);
      result.Append(_cascadingSave.ToString());
      result.Append(Phenix.Core.AppConfig.VALUE_SEPARATOR);
      result.Append(_cascadingDelete.ToString());
      result.Append(Phenix.Core.AppConfig.VALUE_SEPARATOR);
      if (_sectionNames != null)
        foreach (string s in _sectionNames)
        {
          result.Append(s);
          result.Append(Phenix.Core.AppConfig.VALUE_SEPARATOR);
        }
      result.Append(Phenix.Core.AppConfig.VALUE_SEPARATOR);
      result.Append(_includeDisabled.ToString());
      result.Append(Phenix.Core.AppConfig.VALUE_SEPARATOR);
      if (_orderByInfos != null)
        foreach (OrderByInfo item in _orderByInfos)
        {
          result.Append(item.ToString());
          result.Append(Phenix.Core.AppConfig.VALUE_SEPARATOR);
        }
      result.Append(Phenix.Core.AppConfig.VALUE_SEPARATOR);
      if (_criteria != null)
        result.Append(_criteria.ToString());
      result.Append(Phenix.Core.AppConfig.VALUE_SEPARATOR);
      if (_criteriaExpression != null)
        result.Append(_criteriaExpression.ToString());
      result.Append(Phenix.Core.AppConfig.VALUE_SEPARATOR);
      return result.ToString();
    }

    #endregion
  }
}