using System;
using System.Linq.Expressions;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Controllers;
using Phenix.Core.Data;
using Phenix.Core.Dictionary;
using Phenix.Core.Mapping;
using Phenix.Core.Security;

namespace Phenix.Core.Web
{
    /// <summary>
    /// API controller
    /// </summary>
    [DataDictionary(AssemblyClassType.ApiController)]
    [ClassAttribute(null)]
    public abstract class ApiController<T> : ApiController
        where T : ApiController<T>
    {
        #region 属性

        /// <summary>
        /// 当前用户身份
        /// </summary>
        protected static UserIdentity CurrentIdentity
        {
            get { return UserIdentity.CurrentIdentity; }
        }

        #endregion

        #region 方法

        /// <summary>
        /// Executes asynchronously a single HTTP operation.
        /// </summary>
        /// <param name="controllerContext">The controller context for a single HTTP operation.</param>
        /// <param name="cancellationToken">The cancellation token assigned for the HTTP operation.</param>
        /// <returns>The newly started task.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
        [Method(InAuthorization = false)]
        public override Task<HttpResponseMessage> ExecuteAsync(HttpControllerContext controllerContext, CancellationToken cancellationToken)
        {
            if (Attribute.GetCustomAttributes(this.GetType(), typeof(AllowAnonymousAttribute)).Length > 0)
                return base.ExecuteAsync(controllerContext, cancellationToken);

            HttpControllerDescriptor controllerDescriptor = controllerContext.ControllerDescriptor;
            ServicesContainer controllerServices = controllerDescriptor.Configuration.Services;
            HttpActionDescriptor actionDescriptor = controllerServices.GetActionSelector().SelectAction(controllerContext);
            try
            {
                UserIdentity.IsByDeny(CurrentIdentity, this.GetType(), actionDescriptor.ActionName, true);
            }
            catch (Exception ex)
            {
                return Task.FromResult(Utilities.PackErrorResponse(controllerContext.Request, ex));
            }

            return base.ExecuteAsync(controllerContext, cancellationToken);
        }

        /// <summary>
        /// 解包请求消息
        /// </summary>
        protected TResult UnpackRequest<TResult>()
            where TResult : class
        {
            return Utilities.UnpackContent<TResult>(Request.Content);
        }

        /// <summary>
        /// 解包请求消息
        /// </summary>
        protected object UnpackRequest(Type resultType)
        {
            return Utilities.UnpackContent(Request.Content, resultType);
        }

        /// <summary>
        /// 打包响应消息
        /// </summary>
        /// <param name="content">响应内容</param>
        protected HttpResponseMessage PackResponse(object content)
        {
            return Utilities.PackResponse(Request, content);
        }

        /// <summary>
        /// 打包错误消息
        /// </summary>
        protected HttpResponseMessage PackErrorResponse(Exception error)
        {
            return Utilities.PackErrorResponse(Request, error);
        }

        #region Fetch

        private HttpResponseMessage Fetch(ICriterions criterions)
        {
            return PackResponse(DataHub.FetchContent(criterions));
        }

        private HttpResponseMessage Fetch(ICriterions criterions, int pageSize, int pageNo)
        {
            criterions.PageSize = pageSize;
            criterions.PageNo = pageNo;
            return Fetch(criterions);
        }

        /// <summary>
        /// 按照指定主键值来获取对应的数据库记录构建对象内容
        /// </summary>
        /// <param name="primaryKeyValue">主键值</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
        protected HttpResponseMessage Fetch<TEntity>(long primaryKeyValue)
            where TEntity : IEntity
        {
            TEntity itself = (TEntity) Activator.CreateInstance(typeof(TEntity), true);
            if (EntityHelper.FillPrimaryKeyFieldValue(itself, primaryKeyValue))
                return Fetch(itself);
            return null;
        }

        /// <summary>
        /// 按照指定主键值来获取对应的数据库记录构建对象内容
        /// </summary>
        /// <param name="primaryKeyValue">主键值</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
        protected HttpResponseMessage Fetch<TEntity>(string primaryKeyValue)
            where TEntity : IEntity
        {
            TEntity itself = (TEntity) Activator.CreateInstance(typeof(TEntity), true);
            if (EntityHelper.FillPrimaryKeyFieldValue(itself, primaryKeyValue))
                return Fetch(itself);
            return null;
        }

        /// <summary>
        /// 按照指定主键/唯一键值来获取对应的数据库记录构建对象内容
        /// </summary>
        /// <param name="itself">带主键/唯一键值的对象</param>
        protected HttpResponseMessage Fetch(object itself)
        {
            if (itself == null)
                throw new ArgumentNullException("itself");
            return Fetch(new Criterions(itself.GetType(), itself));
        }

        /// <summary>
        /// 构建对象内容
        /// 表中仅一条记录
        /// 否则仅取表的第一条记录
        /// </summary>
        /// <param name="orderByInfos">数据排列顺序队列</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
        protected HttpResponseMessage Fetch<TEntity>(params OrderByInfo[] orderByInfos)
            where TEntity : IEntity
        {
            return Fetch(new Criterions(typeof(TEntity), orderByInfos));
        }

        /// <summary>
        /// 构建对象内容
        /// 条件类的字段映射关系请用Phenix.Core.Mapping.CriteriaFieldAttribute标注
        /// </summary>
        /// <param name="criteria">条件对象</param>
        /// <param name="orderByInfos">数据排列顺序队列</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
        protected HttpResponseMessage Fetch<TEntity>(ICriteria criteria, params OrderByInfo[] orderByInfos)
            where TEntity : IEntity
        {
            return Fetch(new Criterions(typeof(TEntity), criteria, orderByInfos));
        }

        /// <summary>
        /// 构建对象内容
        /// </summary>
        /// <param name="criteriaExpression">条件表达式</param>
        /// <param name="orderByInfos">数据排列顺序队列</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures")]
        protected HttpResponseMessage Fetch<TEntity>(Expression<Func<TEntity, bool>> criteriaExpression, params OrderByInfo[] orderByInfos)
            where TEntity : IEntity
        {
            return Fetch<TEntity>(CriteriaHelper.ToCriteriaExpression(criteriaExpression), orderByInfos);
        }

        /// <summary>
        /// 构建对象内容
        /// </summary>
        /// <param name="dataSourceKey">数据源键</param>
        /// <param name="criteriaExpression">条件表达式</param>
        /// <param name="orderByInfos">数据排列顺序队列</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures")]
        protected HttpResponseMessage Fetch<TEntity>(string dataSourceKey, Expression<Func<TEntity, bool>> criteriaExpression, params OrderByInfo[] orderByInfos)
            where TEntity : IEntity
        {
            return Fetch<TEntity>(CriteriaHelper.ToCriteriaExpression(dataSourceKey, criteriaExpression), orderByInfos);
        }

        /// <summary>
        /// 构建对象内容
        /// </summary>
        /// <param name="criteriaExpression">条件表达式</param>
        /// <param name="orderByInfos">数据排列顺序队列</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
        protected HttpResponseMessage Fetch<TEntity>(CriteriaExpression criteriaExpression, params OrderByInfo[] orderByInfos)
            where TEntity : IEntity
        {
            return Fetch(new Criterions(typeof(TEntity), criteriaExpression, orderByInfos));
        }

        /// <summary>
        /// 构建对象集合内容
        /// </summary>
        /// <param name="orderByInfos">数据排列顺序队列</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
        protected HttpResponseMessage FetchList<TEntity>(params OrderByInfo[] orderByInfos)
            where TEntity : IEntity
        {
            return Fetch(new Criterions(typeof(TEntity), true, orderByInfos));
        }

        /// <summary>
        /// 构建对象集合内容
        /// </summary>
        /// <param name="master">主对象</param>
        /// <param name="groupName">分组名, null代表全部</param>
        /// <param name="orderByInfos">数据排列顺序队列</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
        protected HttpResponseMessage FetchList<TEntity>(object master, string groupName, params OrderByInfo[] orderByInfos)
            where TEntity : IEntity
        {
            return Fetch(new Criterions(typeof(TEntity), master, groupName, orderByInfos));
        }

        /// <summary>
        /// 构建对象集合内容
        /// </summary>
        /// <param name="pageSize">分页大小</param>
        /// <param name="pageNo">分页号</param>
        /// <param name="orderByInfos">数据排列顺序队列</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
        protected HttpResponseMessage FetchList<TEntity>(int pageSize, int pageNo, params OrderByInfo[] orderByInfos)
            where TEntity : IEntity
        {
            return Fetch(new Criterions(typeof(TEntity), true, orderByInfos), pageSize, pageNo);
        }

        /// <summary>
        /// 构建对象集合内容
        /// </summary>
        /// <param name="master">主对象</param>
        /// <param name="groupName">分组名, null代表全部</param>
        /// <param name="pageSize">分页大小</param>
        /// <param name="pageNo">分页号</param>
        /// <param name="orderByInfos">数据排列顺序队列</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
        protected HttpResponseMessage FetchList<TEntity>(object master, string groupName, int pageSize, int pageNo, params OrderByInfo[] orderByInfos)
            where TEntity : IEntity
        {
            return Fetch(new Criterions(typeof(TEntity), master, groupName, orderByInfos), pageSize, pageNo);
        }

        /// <summary>
        /// 构建对象集合内容
        /// 条件类的字段映射关系请用Phenix.Core.Mapping.CriteriaFieldAttribute标注
        /// </summary>
        /// <param name="criteria">条件对象</param>
        /// <param name="orderByInfos">数据排列顺序队列</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
        protected HttpResponseMessage FetchList<TEntity>(ICriteria criteria, params OrderByInfo[] orderByInfos)
            where TEntity : IEntity
        {
            return Fetch(new Criterions(typeof(TEntity), true, criteria, orderByInfos));
        }

        /// <summary>
        /// 构建对象集合内容
        /// 条件类的字段映射关系请用Phenix.Core.Mapping.CriteriaFieldAttribute标注
        /// </summary>
        /// <param name="criteria">条件对象</param>
        /// <param name="master">主对象</param>
        /// <param name="groupName">分组名, null代表全部</param>
        /// <param name="orderByInfos">数据排列顺序队列</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
        protected HttpResponseMessage FetchList<TEntity>(ICriteria criteria, object master, string groupName, params OrderByInfo[] orderByInfos)
            where TEntity : IEntity
        {
            return Fetch(new Criterions(typeof(TEntity), criteria, master, groupName, orderByInfos));
        }

        /// <summary>
        /// 构建对象集合内容
        /// 条件类的字段映射关系请用Phenix.Core.Mapping.CriteriaFieldAttribute标注
        /// </summary>
        /// <param name="criteria">条件对象</param>
        /// <param name="pageSize">分页大小</param>
        /// <param name="pageNo">分页号</param>
        /// <param name="orderByInfos">数据排列顺序队列</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
        protected HttpResponseMessage FetchList<TEntity>(ICriteria criteria, int pageSize, int pageNo, params OrderByInfo[] orderByInfos)
            where TEntity : IEntity
        {
            return Fetch(new Criterions(typeof(TEntity), true, criteria, orderByInfos), pageSize, pageNo);
        }

        /// <summary>
        /// 构建实体集合
        /// 条件类的字段映射关系请用Phenix.Core.Mapping.CriteriaFieldAttribute标注
        /// </summary>
        /// <param name="criteria">条件对象</param>
        /// <param name="master">主对象</param>
        /// <param name="groupName">分组名, null代表全部</param>
        /// <param name="pageSize">分页大小</param>
        /// <param name="pageNo">分页号</param>
        /// <param name="orderByInfos">数据排列顺序队列</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
        protected HttpResponseMessage FetchList<TEntity>(ICriteria criteria, object master, string groupName, int pageSize, int pageNo, params OrderByInfo[] orderByInfos)
            where TEntity : IEntity
        {
            return Fetch(new Criterions(typeof(TEntity), criteria, master, groupName, orderByInfos), pageSize, pageNo);
        }

        /// <summary>
        /// 构建对象集合内容
        /// </summary>
        /// <param name="criteriaExpression">条件表达式</param>
        /// <param name="orderByInfos">数据排列顺序队列</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures")]
        protected HttpResponseMessage FetchList<TEntity>(Expression<Func<TEntity, bool>> criteriaExpression, params OrderByInfo[] orderByInfos)
            where TEntity : IEntity
        {
            return FetchList<TEntity>(CriteriaHelper.ToCriteriaExpression(criteriaExpression), orderByInfos);
        }

        /// <summary>
        /// 构建对象集合内容
        /// </summary>
        /// <param name="criteriaExpression">条件表达式</param>
        /// <param name="master">主对象</param>
        /// <param name="groupName">分组名, null代表全部</param>
        /// <param name="orderByInfos">数据排列顺序队列</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures")]
        protected HttpResponseMessage FetchList<TEntity>(Expression<Func<TEntity, bool>> criteriaExpression, object master, string groupName, params OrderByInfo[] orderByInfos)
            where TEntity : IEntity
        {
            return FetchList<TEntity>(CriteriaHelper.ToCriteriaExpression(criteriaExpression), master, groupName, orderByInfos);
        }

        /// <summary>
        /// 构建对象集合内容
        /// </summary>
        /// <param name="criteriaExpression">条件表达式</param>
        /// <param name="pageSize">分页大小</param>
        /// <param name="pageNo">分页号</param>
        /// <param name="orderByInfos">数据排列顺序队列</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures")]
        protected HttpResponseMessage FetchList<TEntity>(Expression<Func<TEntity, bool>> criteriaExpression, int pageSize, int pageNo, params OrderByInfo[] orderByInfos)
            where TEntity : IEntity
        {
            return FetchList<TEntity>(CriteriaHelper.ToCriteriaExpression(criteriaExpression), pageSize, pageNo, orderByInfos);
        }

        /// <summary>
        /// 构建对象集合内容
        /// </summary>
        /// <param name="criteriaExpression">条件表达式</param>
        /// <param name="master">主对象</param>
        /// <param name="groupName">分组名, null代表全部</param>
        /// <param name="pageSize">分页大小</param>
        /// <param name="pageNo">分页号</param>
        /// <param name="orderByInfos">数据排列顺序队列</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures")]
        protected HttpResponseMessage FetchList<TEntity>(Expression<Func<TEntity, bool>> criteriaExpression, object master, string groupName, int pageSize, int pageNo, params OrderByInfo[] orderByInfos)
            where TEntity : IEntity
        {
            return FetchList<TEntity>(CriteriaHelper.ToCriteriaExpression(criteriaExpression), master, groupName, pageSize, pageNo, orderByInfos);
        }

        /// <summary>
        /// 构建对象集合内容
        /// </summary>
        /// <param name="dataSourceKey">数据源键</param>
        /// <param name="criteriaExpression">条件表达式</param>
        /// <param name="orderByInfos">数据排列顺序队列</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures")]
        protected HttpResponseMessage FetchList<TEntity>(string dataSourceKey, Expression<Func<TEntity, bool>> criteriaExpression, params OrderByInfo[] orderByInfos)
            where TEntity : IEntity
        {
            return FetchList<TEntity>(CriteriaHelper.ToCriteriaExpression(dataSourceKey, criteriaExpression), orderByInfos);
        }

        /// <summary>
        /// 构建对象集合内容
        /// </summary>
        /// <param name="dataSourceKey">数据源键</param>
        /// <param name="criteriaExpression">条件表达式</param>
        /// <param name="master">主对象</param>
        /// <param name="groupName">分组名, null代表全部</param>
        /// <param name="orderByInfos">数据排列顺序队列</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures")]
        protected HttpResponseMessage FetchList<TEntity>(string dataSourceKey, Expression<Func<TEntity, bool>> criteriaExpression, object master, string groupName, params OrderByInfo[] orderByInfos)
            where TEntity : IEntity
        {
            return FetchList<TEntity>(CriteriaHelper.ToCriteriaExpression(dataSourceKey, criteriaExpression), master, groupName, orderByInfos);
        }

        /// <summary>
        /// 构建对象集合内容
        /// </summary>
        /// <param name="dataSourceKey">数据源键</param>
        /// <param name="criteriaExpression">条件表达式</param>
        /// <param name="pageSize">分页大小</param>
        /// <param name="pageNo">分页号</param>
        /// <param name="orderByInfos">数据排列顺序队列</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures")]
        protected HttpResponseMessage FetchList<TEntity>(string dataSourceKey, Expression<Func<TEntity, bool>> criteriaExpression, int pageSize, int pageNo, params OrderByInfo[] orderByInfos)
            where TEntity : IEntity
        {
            return FetchList<TEntity>(CriteriaHelper.ToCriteriaExpression(dataSourceKey, criteriaExpression), pageSize, pageNo, orderByInfos);
        }

        /// <summary>
        /// 构建对象集合内容
        /// </summary>
        /// <param name="dataSourceKey">数据源键</param>
        /// <param name="criteriaExpression">条件表达式</param>
        /// <param name="master">主对象</param>
        /// <param name="groupName">分组名, null代表全部</param>
        /// <param name="pageSize">分页大小</param>
        /// <param name="pageNo">分页号</param>
        /// <param name="orderByInfos">数据排列顺序队列</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures")]
        protected HttpResponseMessage FetchList<TEntity>(string dataSourceKey, Expression<Func<TEntity, bool>> criteriaExpression, object master, string groupName, int pageSize, int pageNo, params OrderByInfo[] orderByInfos)
            where TEntity : IEntity
        {
            return FetchList<TEntity>(CriteriaHelper.ToCriteriaExpression(dataSourceKey, criteriaExpression), master, groupName, pageSize, pageNo, orderByInfos);
        }

        /// <summary>
        /// 构建对象集合内容
        /// </summary>
        /// <param name="criteriaExpression">条件表达式</param>
        /// <param name="orderByInfos">数据排列顺序队列</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
        protected HttpResponseMessage FetchList<TEntity>(CriteriaExpression criteriaExpression, params OrderByInfo[] orderByInfos)
            where TEntity : IEntity
        {
            return Fetch(new Criterions(typeof(TEntity), true, criteriaExpression, orderByInfos));
        }

        /// <summary>
        /// 构建对象集合内容
        /// </summary>
        /// <param name="criteriaExpression">条件表达式</param>
        /// <param name="master">主对象</param>
        /// <param name="groupName">分组名, null代表全部</param>
        /// <param name="orderByInfos">数据排列顺序队列</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
        protected HttpResponseMessage FetchList<TEntity>(CriteriaExpression criteriaExpression, object master, string groupName, params OrderByInfo[] orderByInfos)
            where TEntity : IEntity
        {
            return Fetch(new Criterions(typeof(TEntity), criteriaExpression, master, groupName, orderByInfos));
        }

        /// <summary>
        /// 构建对象集合内容
        /// </summary>
        /// <param name="criteriaExpression">条件表达式</param>
        /// <param name="pageSize">分页大小</param>
        /// <param name="pageNo">分页号</param>
        /// <param name="orderByInfos">数据排列顺序队列</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
        protected HttpResponseMessage FetchList<TEntity>(CriteriaExpression criteriaExpression, int pageSize, int pageNo, params OrderByInfo[] orderByInfos)
            where TEntity : IEntity
        {
            return Fetch(new Criterions(typeof(TEntity), true, criteriaExpression, orderByInfos), pageSize, pageNo);
        }

        /// <summary>
        /// 构建对象集合内容
        /// </summary>
        /// <param name="criteriaExpression">条件表达式</param>
        /// <param name="master">主对象</param>
        /// <param name="groupName">分组名, null代表全部</param>
        /// <param name="pageSize">分页大小</param>
        /// <param name="pageNo">分页号</param>
        /// <param name="orderByInfos">数据排列顺序队列</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
        protected HttpResponseMessage FetchList<TEntity>(CriteriaExpression criteriaExpression, object master, string groupName, int pageSize, int pageNo, params OrderByInfo[] orderByInfos)
            where TEntity : IEntity
        {
            return Fetch(new Criterions(typeof(TEntity), criteriaExpression, master, groupName, orderByInfos), pageSize, pageNo);
        }

        #endregion

        #region Save

        /// <summary>
        /// 保存对象内容
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
        protected HttpResponseMessage Save<TEntity>()
            where TEntity : IEntity
        {
            return PackResponse(DataHub.SaveContent(typeof(TEntity), Request.Content.ReadAsStringAsync().Result).ToString());
        }

        /// <summary>
        /// 保存对象集合内容
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
        protected HttpResponseMessage SaveList<TEntityCollection>()
            where TEntityCollection : IEntityCollection
        {
            return PackResponse(DataHub.SaveContent(typeof(TEntityCollection), Request.Content.ReadAsStringAsync().Result).ToString());
        }

        #endregion

        #region ExecuteService

        /// <summary>
        /// 执行服务
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
        protected HttpResponseMessage ExecuteService<TService>()
            where TService : IService
        {
            return Utilities.ExecuteService<TService>(Request);
        }

        /// <summary>
        /// 执行服务
        /// </summary>
        protected HttpResponseMessage ExecuteService(Type serviceType)
        {
            return Utilities.ExecuteService(Request, serviceType);
        }

        #endregion

        #region ExecuteFunc

        /// <summary>
        /// 执行操作
        /// </summary>
        /// <param name="doExecute">执行操作处理函数</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
        protected HttpResponseMessage Execute<TResult>(Func<TResult> doExecute)
        {
            try
            {
                return PackResponse(doExecute());
            }
            catch (Exception ex)
            {
                return PackErrorResponse(ex);
            }
        }

        /// <summary>
        /// 执行操作
        /// </summary>
        /// <param name="doExecute">执行操作处理函数</param>
        /// <param name="in1">in参数1</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
        protected HttpResponseMessage Execute<TIn1, TResult>(Func<TIn1, TResult> doExecute,
            TIn1 in1)
        {
            try
            {
                return PackResponse(doExecute(in1));
            }
            catch (Exception ex)
            {
                return PackErrorResponse(ex);
            }
        }

        /// <summary>
        /// 执行操作
        /// </summary>
        /// <param name="doExecute">执行操作处理函数</param>
        /// <param name="in1">in参数1</param>
        /// <param name="in2">in参数2</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
        protected HttpResponseMessage Execute<TIn1, TIn2, TResult>(Func<TIn1, TIn2, TResult> doExecute,
            TIn1 in1, TIn2 in2)
        {
            try
            {
                return PackResponse(doExecute(in1, in2));
            }
            catch (Exception ex)
            {
                return PackErrorResponse(ex);
            }
        }

        /// <summary>
        /// 执行操作
        /// </summary>
        /// <param name="doExecute">执行操作处理函数</param>
        /// <param name="in1">in参数1</param>
        /// <param name="in2">in参数2</param>
        /// <param name="in3">in参数3</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
        protected HttpResponseMessage Execute<TIn1, TIn2, TIn3, TResult>(Func<TIn1, TIn2, TIn3, TResult> doExecute,
            TIn1 in1, TIn2 in2, TIn3 in3)
        {
            try
            {
                return PackResponse(doExecute(in1, in2, in3));
            }
            catch (Exception ex)
            {
                return PackErrorResponse(ex);
            }
        }

        /// <summary>
        /// 执行操作
        /// </summary>
        /// <param name="doExecute">执行操作处理函数</param>
        /// <param name="in1">in参数1</param>
        /// <param name="in2">in参数2</param>
        /// <param name="in3">in参数3</param>
        /// <param name="in4">in参数4</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
        protected HttpResponseMessage Execute<TIn1, TIn2, TIn3, TIn4, TResult>(Func<TIn1, TIn2, TIn3, TIn4, TResult> doExecute,
            TIn1 in1, TIn2 in2, TIn3 in3, TIn4 in4)
        {
            try
            {
                return PackResponse(doExecute(in1, in2, in3, in4));
            }
            catch (Exception ex)
            {
                return PackErrorResponse(ex);
            }
        }

        /// <summary>
        /// 执行操作
        /// </summary>
        /// <param name="doExecute">执行操作处理函数</param>
        /// <param name="in1">in参数1</param>
        /// <param name="in2">in参数2</param>
        /// <param name="in3">in参数3</param>
        /// <param name="in4">in参数4</param>
        /// <param name="in5">in参数5</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
        protected HttpResponseMessage Execute<TIn1, TIn2, TIn3, TIn4, TIn5, TResult>(Func<TIn1, TIn2, TIn3, TIn4, TIn5, TResult> doExecute,
            TIn1 in1, TIn2 in2, TIn3 in3, TIn4 in4, TIn5 in5)
        {
            try
            {
                return PackResponse(doExecute(in1, in2, in3, in4, in5));
            }
            catch (Exception ex)
            {
                return PackErrorResponse(ex);
            }
        }

        /// <summary>
        /// 执行操作
        /// </summary>
        /// <param name="doExecute">执行操作处理函数</param>
        /// <param name="in1">in参数1</param>
        /// <param name="in2">in参数2</param>
        /// <param name="in3">in参数3</param>
        /// <param name="in4">in参数4</param>
        /// <param name="in5">in参数5</param>
        /// <param name="in6">in参数6</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
        protected HttpResponseMessage Execute<TIn1, TIn2, TIn3, TIn4, TIn5, TIn6, TResult>(Func<TIn1, TIn2, TIn3, TIn4, TIn5, TIn6, TResult> doExecute,
            TIn1 in1, TIn2 in2, TIn3 in3, TIn4 in4, TIn5 in5, TIn6 in6)
        {
            try
            {
                return PackResponse(doExecute(in1, in2, in3, in4, in5, in6));
            }
            catch (Exception ex)
            {
                return PackErrorResponse(ex);
            }
        }

        /// <summary>
        /// 执行操作
        /// </summary>
        /// <param name="doExecute">执行操作处理函数</param>
        /// <param name="in1">in参数1</param>
        /// <param name="in2">in参数2</param>
        /// <param name="in3">in参数3</param>
        /// <param name="in4">in参数4</param>
        /// <param name="in5">in参数5</param>
        /// <param name="in6">in参数6</param>
        /// <param name="in7">in参数7</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
        protected HttpResponseMessage Execute<TIn1, TIn2, TIn3, TIn4, TIn5, TIn6, TIn7, TResult>(Func<TIn1, TIn2, TIn3, TIn4, TIn5, TIn6, TIn7, TResult> doExecute,
            TIn1 in1, TIn2 in2, TIn3 in3, TIn4 in4, TIn5 in5, TIn6 in6, TIn7 in7)
        {
            try
            {
                return PackResponse(doExecute(in1, in2, in3, in4, in5, in6, in7));
            }
            catch (Exception ex)
            {
                return PackErrorResponse(ex);
            }
        }

        /// <summary>
        /// 执行操作
        /// </summary>
        /// <param name="doExecute">执行操作处理函数</param>
        /// <param name="in1">in参数1</param>
        /// <param name="in2">in参数2</param>
        /// <param name="in3">in参数3</param>
        /// <param name="in4">in参数4</param>
        /// <param name="in5">in参数5</param>
        /// <param name="in6">in参数6</param>
        /// <param name="in7">in参数7</param>
        /// <param name="in8">in参数8</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
        protected HttpResponseMessage Execute<TIn1, TIn2, TIn3, TIn4, TIn5, TIn6, TIn7, TIn8, TResult>(Func<TIn1, TIn2, TIn3, TIn4, TIn5, TIn6, TIn7, TIn8, TResult> doExecute,
            TIn1 in1, TIn2 in2, TIn3 in3, TIn4 in4, TIn5 in5, TIn6 in6, TIn7 in7, TIn8 in8)
        {
            try
            {
                return PackResponse(doExecute(in1, in2, in3, in4, in5, in6, in7, in8));
            }
            catch (Exception ex)
            {
                return PackErrorResponse(ex);
            }
        }

        /// <summary>
        /// 执行操作
        /// </summary>
        /// <param name="doExecute">执行操作处理函数</param>
        /// <param name="in1">in参数1</param>
        /// <param name="in2">in参数2</param>
        /// <param name="in3">in参数3</param>
        /// <param name="in4">in参数4</param>
        /// <param name="in5">in参数5</param>
        /// <param name="in6">in参数6</param>
        /// <param name="in7">in参数7</param>
        /// <param name="in8">in参数8</param>
        /// <param name="in9">in参数9</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
        protected HttpResponseMessage Execute<TIn1, TIn2, TIn3, TIn4, TIn5, TIn6, TIn7, TIn8, TIn9, TResult>(Func<TIn1, TIn2, TIn3, TIn4, TIn5, TIn6, TIn7, TIn8, TIn9, TResult> doExecute,
            TIn1 in1, TIn2 in2, TIn3 in3, TIn4 in4, TIn5 in5, TIn6 in6, TIn7 in7, TIn8 in8, TIn9 in9)
        {
            try
            {
                return PackResponse(doExecute(in1, in2, in3, in4, in5, in6, in7, in8, in9));
            }
            catch (Exception ex)
            {
                return PackErrorResponse(ex);
            }
        }

        /// <summary>
        /// 执行操作
        /// </summary>
        /// <param name="doExecute">执行操作处理函数</param>
        /// <param name="in1">in参数1</param>
        /// <param name="in2">in参数2</param>
        /// <param name="in3">in参数3</param>
        /// <param name="in4">in参数4</param>
        /// <param name="in5">in参数5</param>
        /// <param name="in6">in参数6</param>
        /// <param name="in7">in参数7</param>
        /// <param name="in8">in参数8</param>
        /// <param name="in9">in参数9</param>
        /// <param name="in10">in参数10</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
        protected HttpResponseMessage Execute<TIn1, TIn2, TIn3, TIn4, TIn5, TIn6, TIn7, TIn8, TIn9, TIn10, TResult>(Func<TIn1, TIn2, TIn3, TIn4, TIn5, TIn6, TIn7, TIn8, TIn9, TIn10, TResult> doExecute,
            TIn1 in1, TIn2 in2, TIn3 in3, TIn4 in4, TIn5 in5, TIn6 in6, TIn7 in7, TIn8 in8, TIn9 in9, TIn10 in10)
        {
            try
            {
                return PackResponse(doExecute(in1, in2, in3, in4, in5, in6, in7, in8, in9, in10));
            }
            catch (Exception ex)
            {
                return PackErrorResponse(ex);
            }
        }

        /// <summary>
        /// 执行操作
        /// </summary>
        /// <param name="doExecute">执行操作处理函数</param>
        /// <param name="in1">in参数1</param>
        /// <param name="in2">in参数2</param>
        /// <param name="in3">in参数3</param>
        /// <param name="in4">in参数4</param>
        /// <param name="in5">in参数5</param>
        /// <param name="in6">in参数6</param>
        /// <param name="in7">in参数7</param>
        /// <param name="in8">in参数8</param>
        /// <param name="in9">in参数9</param>
        /// <param name="in10">in参数10</param>
        /// <param name="in11">in参数11</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
        protected HttpResponseMessage Execute<TIn1, TIn2, TIn3, TIn4, TIn5, TIn6, TIn7, TIn8, TIn9, TIn10, TIn11, TResult>(Func<TIn1, TIn2, TIn3, TIn4, TIn5, TIn6, TIn7, TIn8, TIn9, TIn10, TIn11, TResult> doExecute,
            TIn1 in1, TIn2 in2, TIn3 in3, TIn4 in4, TIn5 in5, TIn6 in6, TIn7 in7, TIn8 in8, TIn9 in9, TIn10 in10, TIn11 in11)
        {
            try
            {
                return PackResponse(doExecute(in1, in2, in3, in4, in5, in6, in7, in8, in9, in10, in11));
            }
            catch (Exception ex)
            {
                return PackErrorResponse(ex);
            }
        }

        /// <summary>
        /// 执行操作
        /// </summary>
        /// <param name="doExecute">执行操作处理函数</param>
        /// <param name="in1">in参数1</param>
        /// <param name="in2">in参数2</param>
        /// <param name="in3">in参数3</param>
        /// <param name="in4">in参数4</param>
        /// <param name="in5">in参数5</param>
        /// <param name="in6">in参数6</param>
        /// <param name="in7">in参数7</param>
        /// <param name="in8">in参数8</param>
        /// <param name="in9">in参数9</param>
        /// <param name="in10">in参数10</param>
        /// <param name="in11">in参数11</param>
        /// <param name="in12">in参数12</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
        protected HttpResponseMessage Execute<TIn1, TIn2, TIn3, TIn4, TIn5, TIn6, TIn7, TIn8, TIn9, TIn10, TIn11, TIn12, TResult>(Func<TIn1, TIn2, TIn3, TIn4, TIn5, TIn6, TIn7, TIn8, TIn9, TIn10, TIn11, TIn12, TResult> doExecute,
            TIn1 in1, TIn2 in2, TIn3 in3, TIn4 in4, TIn5 in5, TIn6 in6, TIn7 in7, TIn8 in8, TIn9 in9, TIn10 in10, TIn11 in11, TIn12 in12)
        {
            try
            {
                return PackResponse(doExecute(in1, in2, in3, in4, in5, in6, in7, in8, in9, in10, in11, in12));
            }
            catch (Exception ex)
            {
                return PackErrorResponse(ex);
            }
        }

        #endregion

        #region ExecuteAction

        /// <summary>
        /// 执行操作
        /// </summary>
        /// <param name="doExecute">执行操作处理函数</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
        protected HttpResponseMessage Execute(Action doExecute)
        {
            try
            {
                doExecute();
                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                return PackErrorResponse(ex);
            }
        }

        /// <summary>
        /// 执行操作
        /// </summary>
        /// <param name="doExecute">执行操作处理函数</param>
        /// <param name="in1">in参数1</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
        protected HttpResponseMessage Execute<TIn1>(Action<TIn1> doExecute,
            TIn1 in1)
        {
            try
            {
                doExecute(in1);
                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                return PackErrorResponse(ex);
            }
        }

        /// <summary>
        /// 执行操作
        /// </summary>
        /// <param name="doExecute">执行操作处理函数</param>
        /// <param name="in1">in参数1</param>
        /// <param name="in2">in参数2</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
        protected HttpResponseMessage Execute<TIn1, TIn2>(Action<TIn1, TIn2> doExecute,
            TIn1 in1, TIn2 in2)
        {
            try
            {
                doExecute(in1, in2);
                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                return PackErrorResponse(ex);
            }
        }

        /// <summary>
        /// 执行操作
        /// </summary>
        /// <param name="doExecute">执行操作处理函数</param>
        /// <param name="in1">in参数1</param>
        /// <param name="in2">in参数2</param>
        /// <param name="in3">in参数3</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
        protected HttpResponseMessage Execute<TIn1, TIn2, TIn3>(Action<TIn1, TIn2, TIn3> doExecute,
            TIn1 in1, TIn2 in2, TIn3 in3)
        {
            try
            {
                doExecute(in1, in2, in3);
                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                return PackErrorResponse(ex);
            }
        }

        /// <summary>
        /// 执行操作
        /// </summary>
        /// <param name="doExecute">执行操作处理函数</param>
        /// <param name="in1">in参数1</param>
        /// <param name="in2">in参数2</param>
        /// <param name="in3">in参数3</param>
        /// <param name="in4">in参数4</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
        protected HttpResponseMessage Execute<TIn1, TIn2, TIn3, TIn4>(Action<TIn1, TIn2, TIn3, TIn4> doExecute,
            TIn1 in1, TIn2 in2, TIn3 in3, TIn4 in4)
        {
            try
            {
                doExecute(in1, in2, in3, in4);
                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                return PackErrorResponse(ex);
            }
        }

        /// <summary>
        /// 执行操作
        /// </summary>
        /// <param name="doExecute">执行操作处理函数</param>
        /// <param name="in1">in参数1</param>
        /// <param name="in2">in参数2</param>
        /// <param name="in3">in参数3</param>
        /// <param name="in4">in参数4</param>
        /// <param name="in5">in参数5</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
        protected HttpResponseMessage Execute<TIn1, TIn2, TIn3, TIn4, TIn5>(Action<TIn1, TIn2, TIn3, TIn4, TIn5> doExecute,
            TIn1 in1, TIn2 in2, TIn3 in3, TIn4 in4, TIn5 in5)
        {
            try
            {
                doExecute(in1, in2, in3, in4, in5);
                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                return PackErrorResponse(ex);
            }
        }

        /// <summary>
        /// 执行操作
        /// </summary>
        /// <param name="doExecute">执行操作处理函数</param>
        /// <param name="in1">in参数1</param>
        /// <param name="in2">in参数2</param>
        /// <param name="in3">in参数3</param>
        /// <param name="in4">in参数4</param>
        /// <param name="in5">in参数5</param>
        /// <param name="in6">in参数6</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
        protected HttpResponseMessage Execute<TIn1, TIn2, TIn3, TIn4, TIn5, TIn6>(Action<TIn1, TIn2, TIn3, TIn4, TIn5, TIn6> doExecute,
            TIn1 in1, TIn2 in2, TIn3 in3, TIn4 in4, TIn5 in5, TIn6 in6)
        {
            try
            {
                doExecute(in1, in2, in3, in4, in5, in6);
                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                return PackErrorResponse(ex);
            }
        }

        /// <summary>
        /// 执行操作
        /// </summary>
        /// <param name="doExecute">执行操作处理函数</param>
        /// <param name="in1">in参数1</param>
        /// <param name="in2">in参数2</param>
        /// <param name="in3">in参数3</param>
        /// <param name="in4">in参数4</param>
        /// <param name="in5">in参数5</param>
        /// <param name="in6">in参数6</param>
        /// <param name="in7">in参数7</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
        protected HttpResponseMessage Execute<TIn1, TIn2, TIn3, TIn4, TIn5, TIn6, TIn7>(Action<TIn1, TIn2, TIn3, TIn4, TIn5, TIn6, TIn7> doExecute,
            TIn1 in1, TIn2 in2, TIn3 in3, TIn4 in4, TIn5 in5, TIn6 in6, TIn7 in7)
        {
            try
            {
                doExecute(in1, in2, in3, in4, in5, in6, in7);
                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                return PackErrorResponse(ex);
            }
        }

        /// <summary>
        /// 执行操作
        /// </summary>
        /// <param name="doExecute">执行操作处理函数</param>
        /// <param name="in1">in参数1</param>
        /// <param name="in2">in参数2</param>
        /// <param name="in3">in参数3</param>
        /// <param name="in4">in参数4</param>
        /// <param name="in5">in参数5</param>
        /// <param name="in6">in参数6</param>
        /// <param name="in7">in参数7</param>
        /// <param name="in8">in参数8</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
        protected HttpResponseMessage Execute<TIn1, TIn2, TIn3, TIn4, TIn5, TIn6, TIn7, TIn8>(Action<TIn1, TIn2, TIn3, TIn4, TIn5, TIn6, TIn7, TIn8> doExecute,
            TIn1 in1, TIn2 in2, TIn3 in3, TIn4 in4, TIn5 in5, TIn6 in6, TIn7 in7, TIn8 in8)
        {
            try
            {
                doExecute(in1, in2, in3, in4, in5, in6, in7, in8);
                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                return PackErrorResponse(ex);
            }
        }

        /// <summary>
        /// 执行操作
        /// </summary>
        /// <param name="doExecute">执行操作处理函数</param>
        /// <param name="in1">in参数1</param>
        /// <param name="in2">in参数2</param>
        /// <param name="in3">in参数3</param>
        /// <param name="in4">in参数4</param>
        /// <param name="in5">in参数5</param>
        /// <param name="in6">in参数6</param>
        /// <param name="in7">in参数7</param>
        /// <param name="in8">in参数8</param>
        /// <param name="in9">in参数9</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
        protected HttpResponseMessage Execute<TIn1, TIn2, TIn3, TIn4, TIn5, TIn6, TIn7, TIn8, TIn9>(Action<TIn1, TIn2, TIn3, TIn4, TIn5, TIn6, TIn7, TIn8, TIn9> doExecute,
            TIn1 in1, TIn2 in2, TIn3 in3, TIn4 in4, TIn5 in5, TIn6 in6, TIn7 in7, TIn8 in8, TIn9 in9)
        {
            try
            {
                doExecute(in1, in2, in3, in4, in5, in6, in7, in8, in9);
                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                return PackErrorResponse(ex);
            }
        }

        /// <summary>
        /// 执行操作
        /// </summary>
        /// <param name="doExecute">执行操作处理函数</param>
        /// <param name="in1">in参数1</param>
        /// <param name="in2">in参数2</param>
        /// <param name="in3">in参数3</param>
        /// <param name="in4">in参数4</param>
        /// <param name="in5">in参数5</param>
        /// <param name="in6">in参数6</param>
        /// <param name="in7">in参数7</param>
        /// <param name="in8">in参数8</param>
        /// <param name="in9">in参数9</param>
        /// <param name="in10">in参数10</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
        protected HttpResponseMessage Execute<TIn1, TIn2, TIn3, TIn4, TIn5, TIn6, TIn7, TIn8, TIn9, TIn10>(Action<TIn1, TIn2, TIn3, TIn4, TIn5, TIn6, TIn7, TIn8, TIn9, TIn10> doExecute,
            TIn1 in1, TIn2 in2, TIn3 in3, TIn4 in4, TIn5 in5, TIn6 in6, TIn7 in7, TIn8 in8, TIn9 in9, TIn10 in10)
        {
            try
            {
                doExecute(in1, in2, in3, in4, in5, in6, in7, in8, in9, in10);
                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                return PackErrorResponse(ex);
            }
        }

        /// <summary>
        /// 执行操作
        /// </summary>
        /// <param name="doExecute">执行操作处理函数</param>
        /// <param name="in1">in参数1</param>
        /// <param name="in2">in参数2</param>
        /// <param name="in3">in参数3</param>
        /// <param name="in4">in参数4</param>
        /// <param name="in5">in参数5</param>
        /// <param name="in6">in参数6</param>
        /// <param name="in7">in参数7</param>
        /// <param name="in8">in参数8</param>
        /// <param name="in9">in参数9</param>
        /// <param name="in10">in参数10</param>
        /// <param name="in11">in参数11</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
        protected HttpResponseMessage Execute<TIn1, TIn2, TIn3, TIn4, TIn5, TIn6, TIn7, TIn8, TIn9, TIn10, TIn11>(Action<TIn1, TIn2, TIn3, TIn4, TIn5, TIn6, TIn7, TIn8, TIn9, TIn10, TIn11> doExecute,
            TIn1 in1, TIn2 in2, TIn3 in3, TIn4 in4, TIn5 in5, TIn6 in6, TIn7 in7, TIn8 in8, TIn9 in9, TIn10 in10, TIn11 in11)
        {
            try
            {
                doExecute(in1, in2, in3, in4, in5, in6, in7, in8, in9, in10, in11);
                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                return PackErrorResponse(ex);
            }
        }

        /// <summary>
        /// 执行操作
        /// </summary>
        /// <param name="doExecute">执行操作处理函数</param>
        /// <param name="in1">in参数1</param>
        /// <param name="in2">in参数2</param>
        /// <param name="in3">in参数3</param>
        /// <param name="in4">in参数4</param>
        /// <param name="in5">in参数5</param>
        /// <param name="in6">in参数6</param>
        /// <param name="in7">in参数7</param>
        /// <param name="in8">in参数8</param>
        /// <param name="in9">in参数9</param>
        /// <param name="in10">in参数10</param>
        /// <param name="in11">in参数11</param>
        /// <param name="in12">in参数12</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
        protected HttpResponseMessage Execute<TIn1, TIn2, TIn3, TIn4, TIn5, TIn6, TIn7, TIn8, TIn9, TIn10, TIn11, TIn12>(Action<TIn1, TIn2, TIn3, TIn4, TIn5, TIn6, TIn7, TIn8, TIn9, TIn10, TIn11, TIn12> doExecute,
            TIn1 in1, TIn2 in2, TIn3 in3, TIn4 in4, TIn5 in5, TIn6 in6, TIn7 in7, TIn8 in8, TIn9 in9, TIn10 in10, TIn11 in11, TIn12 in12)
        {
            try
            {
                doExecute(in1, in2, in3, in4, in5, in6, in7, in8, in9, in10, in11, in12);
                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                return PackErrorResponse(ex);
            }
        }

        #endregion

        #endregion
    }
}